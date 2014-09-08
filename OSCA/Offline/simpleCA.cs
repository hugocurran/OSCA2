/*
 * Copyright 2011 Peter Curran (peter@currans.eu). All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the 
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY PETER CURRAN "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL PETER CURRAN OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN 
 * IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the author alone. 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using OSCA.Crypto;
using OSCA.Log;
using OSCA.Policy;
using OSCA.Profile;
using System.Collections;


namespace OSCA.Offline
{


    /// <summary>
    /// A basic implementation of a Certification Authority
    /// </summary>
    public class simpleCA : ICA
    {
        #region Inheritable properties

        /// <summary>
        /// The configuration file
        /// </summary>
        protected string configFile;
        /// <summary>
        /// The password
        /// </summary>
        protected char[] password;
        /// <summary>
        /// The name
        /// </summary>
        protected string name;
        /// <summary>
        /// The type
        /// </summary>
        protected string type;
        /// <summary>
        /// The database file location
        /// </summary>
        protected string dbFileLocation;
        /// <summary>
        /// The profiles location
        /// </summary>
        protected string profilesLocation;
        /// <summary>
        /// The public key algorithm
        /// </summary>
        protected string publicKeyAlgorithm;
        /// <summary>
        /// The public key size
        /// </summary>
        protected string publicKeySize;
        /// <summary>
        /// The signature algorithm
        /// </summary>
        protected string signatureAlgorithm;
        /// <summary>
        /// The fips140
        /// </summary>
        protected bool fips140;
        /// <summary>
        /// The last serial
        /// </summary>
        protected string lastSerial;
        /// <summary>
        /// The CRL file location
        /// </summary>
        protected string crlFileLocation;
        /// <summary>
        /// The last CRL
        /// </summary>
        protected string lastCRL;
        /// <summary>
        /// The CRL interval
        /// </summary>
        protected double crlInterval;
        /// <summary>
        /// The certificate
        /// </summary>
        protected X509Certificate caCertificate;
        /// <summary>
        /// The CSP parameter
        /// </summary>
        protected CspParameters cspParam;
        /// <summary>
        /// The policy enforcement
        /// </summary>
        internal List<PolicyEnforcement> policyEnforcement;
        /// <summary>
        /// The event log
        /// </summary>
        internal Logger eventLog;

        #endregion

        #region Private properties

        private AsymmetricKeyParameter privateKey;

        #endregion

        #region Constructors
        /// <summary>
        /// Construct a CA object
        /// </summary>
        /// <param name="ConfigFile">Full pathname to config file</param>
        /// <param name="Password">Password for key file</param>
        public simpleCA(string ConfigFile, string Password)
        {
            this.configFile = ConfigFile;
            this.password = Password.ToCharArray();

            // Read in the configuration
            XDocument config;
            if (XmlSigning.VerifyXmlFile(configFile))
                config = XDocument.Load(configFile);
            else
                throw new GeneralSecurityException("Signature failed on CA config file");

            XElement ca = config.Element("OSCA").Element("CA");
            this.name = ca.Element("name").Value;
            this.type = ca.Element("type").Value;
            this.dbFileLocation = ca.Element("dbFileLocation").Value;
            this.publicKeyAlgorithm = ca.Element("publicKeyAlgorithm").Value;
            this.publicKeySize = ca.Element("publicKeySize").Value;
            this.signatureAlgorithm = ca.Element("signatureAlgorithm").Value;
            this.fips140 = Convert.ToBoolean(ca.Element("fips140").Value);
            this.lastSerial = ca.Element("lastSerial").Value;
            this.crlFileLocation = ca.Element("crlFileLocation").Value;
            this.lastCRL = ca.Element("lastCRL").Value;
            this.crlInterval = Convert.ToDouble(ca.Element("crlInterval").Value);
            this.profilesLocation = ca.Element("profilesLocation").Value;

            //Read in the private key and certificate
            MemoryStream p12stream = new MemoryStream(Convert.FromBase64String(ca.Element("caKey").Value));
            Pkcs12Store p12 = new Pkcs12Store(p12stream, password);
            this.privateKey = p12.GetKey(this.name).Key;
            this.caCertificate = p12.GetCertificate(this.name).Certificate;

            if (ca.Element("policyEnforcement") != null)
            {
                policyEnforcement = PolicyEnforcementFactory.initialise(caCertificate, ca.Element("policyEnforcement"));
            }

            // Create CspParameters to support XML signing
            cspParam = SysKeyManager.LoadCsp(privateKey);

            // Setup the Event Logger
            eventLog = new Logger(ca.Element("logFileLocation").Value, caCertificate, cspParam);

            // Check our certificate is valid
            // --- TODO

            // Log startup event
            logEvent(LogEvent.EventType.StartCA, "CA Started");

            // Expire any old certificates
            Database.ExpireCertificate(dbFileLocation, caCertificate, cspParam);
        }

        /// <summary>
        /// Stub constructor to support inheritance
        /// </summary>
        protected simpleCA() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Serial number of the last CRL issued
        /// </summary>
        public string LastCRLNumber { get { return lastCRL; } }
        /// <summary>
        /// Serial number of the last certificate issued
        /// </summary>
        public string LastSerialNumber { get { return lastSerial; } }
        /// <summary>
        /// True if this CA is in FIPS 140 mode
        /// </summary>
        public bool FIPS140Mode { get { return fips140; } }
        /// <summary>
        /// CA Type (Root or subCA)
        /// </summary>
        public string Type { get { return type; } }
        /// <summary>
        /// Distinguished Name of the CA
        /// </summary>
        public string CAName { get { return caCertificate.SubjectDN.ToString(); } }
        /// <summary>
        /// Algorithm for the CAs key-pair
        /// </summary>
        public string PublicKeyAlgorithm { get { return publicKeyAlgorithm + "-" + publicKeySize; } }
        /// <summary>
        /// Algorithm for the CAs signature
        /// </summary>
        public string SignatureAlgorithm { get { return signatureAlgorithm; } }
        /// <summary>
        /// CA certificate
        /// </summary>
        public X509Certificate Certificate { get { return caCertificate; } }

        #endregion

        #region Internal helper methods

        private ValidityPeriod nullPeriod = new ValidityPeriod(ValidityPeriod.Unit.Years, 0);

        private BigInteger nextCertSerial()
        {
            // Calculate serial number
            BigInteger last = new BigInteger(lastSerial);
            BigInteger next = last.Add(BigInteger.One);
            lastSerial = next.ToString(); // Update the object
            saveCertSerial(next);         // Update the config file
            return next;
        }

        private BigInteger nextCrlSerial()
        {
            // Calculate serial number
            BigInteger last = new BigInteger(lastCRL);
            BigInteger next = last.Add(BigInteger.One);
            lastCRL = next.ToString();   // Update the object
            saveCRLSerial(next);         // Update the config file
            return next;
        }

        private void saveCertSerial(BigInteger certSerial)
        {
            XDocument config;
            if (XmlSigning.VerifyXmlFile(configFile))
                config= XDocument.Load(configFile);
            else
                throw new GeneralSecurityException("Signature failed on CA config file");

            XElement serial = config.Element("OSCA").Element("CA").Element("lastSerial");
            serial.ReplaceWith(new XElement("lastSerial", certSerial));

            XmlSigning.SignXml(config, configFile, caCertificate, cspParam);
        }

        private void saveCRLSerial(BigInteger crlSerial)
        {
            XDocument config;
            if (XmlSigning.VerifyXmlFile(configFile))
                config = XDocument.Load(configFile);
            else
                throw new GeneralSecurityException("Signature failed on CA config file");

            XElement serial = config.Element("OSCA").Element("CA").Element("lastCRL");
            serial.ReplaceWith(new XElement("lastCRL", crlSerial));

            XmlSigning.SignXml(config, configFile, caCertificate, cspParam);
        }

        /// <summary>
        /// Generates the certificate.
        /// </summary>
        /// <param name="gen">The generator.</param>
        /// <param name="profile">The profile.</param>
        /// <param name="notBefore">The not before.</param>
        /// <param name="notAfter">The not after.</param>
        /// <returns></returns>
        protected virtual X509Certificate generate(ICertGen gen, Profile.Profile profile, DateTime notBefore, DateTime notAfter)
        {
            return ((IbcCertGen)gen).Generate(privateKey, profile, notBefore, notAfter);
        }

        /// <summary>
        /// Generates the certificate.
        /// </summary>
        /// <param name="gen">The generator.</param>
        /// <returns></returns>
        protected virtual X509Certificate generate(ICertGen gen)
        {
            return ((IbcCertGen)gen).Generate(privateKey);
        }

        /// <summary>
        /// Generates the certificate.
        /// </summary>
        /// <param name="gen">The generator.</param>
        /// <param name="ext">The extensions.</param>
        /// <returns></returns>
        protected virtual X509Certificate generate(ICertGen gen, X509Extensions ext)
        {
            return ((IbcCertGen)gen).Generate(privateKey, ext);
        }

        /// <summary>
        /// Checks the ca cert is valid.
        /// </summary>
        /// <param name="issuingCaCert">The issuing ca cert.</param>
        /// <returns>True if valid, false if not</returns>
        protected bool checkCaCertValid(X509Certificate issuingCaCert)
        {
            try
            {
                caCertificate.CheckValidity();
                caCertificate.Verify(issuingCaCert.GetPublicKey());
            }
            catch (Exception ex)
            {
                logEvent(LogEvent.EventType.Error, "CA " + ex.Message);
                return false;
            }
            return true;
        }


        #endregion

        #region IssueCertificate

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request and OSCA profile
        /// </summary>
        /// <param name="Request">DER encoded PKCS#10 certificate request</param>
        /// <param name="Profile">OSCA Profile</param>
        /// <returns>
        /// Requested certificate
        /// </returns>
        /// <exception cref="System.Exception">Certificate issue failed</exception>
        public X509Certificate IssueCertificate(byte[] Request, Profile.Profile Profile)
        {
            Pkcs10CertificationRequest p10;
            try
            {
                p10 = new Pkcs10CertificationRequest(Request);

            }
            catch (Exception ex)
            {
                logEvent(LogEvent.EventType.Error, "Certificate issue fail: " + ex.Message);
                throw new Exception("Certificate issue failed", ex);
            }
            return IssueCertificate(p10, Profile);
        }

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request and validity period
        /// </summary>
        /// <param name="Request">DER encoded PKCS#10 certificate request</param>
        /// <param name="Validity">Validity period for certificate</param>
        /// <returns>Requested certificate</returns>
        /// <exception cref="ArgumentException">Invalid signature algorithm in request</exception>
        public X509Certificate IssueCertificate(byte[] Request, ValidityPeriod Validity)
        {
            Pkcs10CertificationRequest p10;
            try
            {
                p10 = new Pkcs10CertificationRequest(Request);

            }
            catch (Exception ex)
            {
                logEvent(LogEvent.EventType.Error, "Certificate issue fail: " + ex.Message);
                throw new Exception("Certificate issue failed", ex);
            }
            return IssueCertificate(p10, Validity);
        }

        /// <summary>
        /// Issues the certificate.
        /// </summary>
        /// <param name="Request">DER encoded PKCS#10 certificate request</param>
        /// <param name="Profile">A profile instance.</param>
        /// <param name="NotBefore">Not before date.</param>
        /// <param name="NotAfter">Not after date.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Certificate issue failed</exception>
        public X509Certificate IssueCertificate(byte[] Request, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            Pkcs10CertificationRequest p10;
            try
            {
                p10 = new Pkcs10CertificationRequest(Request);
            }
            catch (Exception ex)
            {
                logEvent(LogEvent.EventType.Error, "Certificate issue fail: " + ex.Message);
                throw new Exception("Certificate issue failed", ex);
            }

            return IssueCertificate(p10, Profile, NotBefore, NotAfter);
        }

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request object and OSCA profile
        /// </summary>
        /// <param name="Request">PKCS#10 certificate request</param>
        /// <param name="Profile">The profile.</param>
        /// <returns>
        /// Requested certificate
        /// </returns>
        /// <exception cref="System.Exception">Certificate issue failed</exception>
        public X509Certificate IssueCertificate(Pkcs10CertificationRequest Request, Profile.Profile Profile)
        {
            X509Certificate cert = null;
            try
            {
                cert = issueCertificate(Request, Profile, DateTime.Now.ToUniversalTime(), Profile.CertificateLifetime.NotAfter(DateTime.Now).ToUniversalTime());
                logEvent(LogEvent.EventType.IssueCert, "Certificate issued. Serial: " + cert.SerialNumber.ToString());
            }
            catch (Exception ex)
            {
                logEvent(LogEvent.EventType.Error, "Certificate issue fail: " + ex.Message);
                throw new Exception("Certificate issue failed: " + ex.Message, ex);
            }

            return cert;
        }

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request object and validity period
        /// </summary>
        /// <param name="Request">PKCS#10 certificate request</param>
        /// <param name="Validity">Validity period for certificate</param>
        /// <returns>Requested certificate</returns>
        /// <exception cref="ArgumentException">Invalid signature algorithm in request</exception>
        public X509Certificate IssueCertificate(Pkcs10CertificationRequest Request, ValidityPeriod Validity)
        {
            X509Certificate cert = null;
            try
            {
                cert = issueCertificate(Request, null, DateTime.Now.ToUniversalTime(), Validity.NotAfter(DateTime.Now).ToUniversalTime());
                logEvent(LogEvent.EventType.IssueCert, "Certificate issued. Serial: " + cert.SerialNumber.ToString());
            }
            catch (Exception ex)
            {
                logEvent(LogEvent.EventType.Error, "Certificate issue fail: " + ex.Message);
                throw new Exception("Certificate issue failed", ex);
            }

            return cert;
        }

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request object, OSCA profile and notBefore/notAfter dates
        /// </summary>
        /// <param name="Request">PKCS#10 certificate request</param>
        /// <param name="Profile">The profile.</param>
        /// <param name="NotBefore">The not before.</param>
        /// <param name="NotAfter">The not after.</param>
        /// <returns>
        /// Requested certificate
        /// </returns>
        /// <exception cref="System.Exception">Certificate issue failed</exception>
        /// <remarks>
        /// This method is intended to support rekey.
        /// </remarks>
        public X509Certificate IssueCertificate(Pkcs10CertificationRequest Request, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            X509Certificate cert = null;
            try
            {
                cert = issueCertificate(Request, Profile, NotBefore, NotAfter);
                logEvent(LogEvent.EventType.IssueCert, "Certificate issued. Serial: " + cert.SerialNumber.ToString());
            }
            catch (Exception ex)
            {
                logEvent(LogEvent.EventType.Error, "Certificate issue fail: " + ex.Message);
                throw new Exception("Certificate issue failed", ex);
            }

            return cert;
        }


        /// <summary>
        /// Issues the certificate.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="profile">The profile</param>
        /// <param name="notBefore">The not before.</param>
        /// <param name="notAfter">The not after.</param>
        /// <returns>
        /// Certificate
        /// </returns>
        /// <exception cref="System.ArgumentException">Invalid signature algorithm in request</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Invalid lifetime units in ValidityPeriod</exception>
        private X509Certificate issueCertificate(Pkcs10CertificationRequest request, Profile.Profile profile, DateTime notBefore, DateTime notAfter)
        {
            X509Certificate newCert;
            string profileName = "";

            // Parse the request
            Pkcs10Parser p10 = new Pkcs10Parser(request);

            // Check that correct sig algorithm has been used
            DerObjectIdentifier sigAlgOid = X509Utilities.GetAlgorithmOid(signatureAlgorithm);
            if (!p10.SignatureAlgorithm.Equals(sigAlgOid))
            {
                logEvent(LogEvent.EventType.Error, "Invalid signature algorithm in request: " + p10.SignatureAlgorithm.ToString());
                throw new ArgumentException("Invalid signature algorithm in request", p10.SignatureAlgorithm.ToString());
            }

            // Create a Cert Generator according to the FIPS 140 policy and CA Type
            ICertGen certGen;
            if ((fips140) && (type == CA_Type.dhTA.ToString()))
                certGen = new SysV1CertGen();
            else if ((fips140) && (type != CA_Type.dhTA.ToString()))
                certGen = new SysV3CertGen(policyEnforcement);
            else
                certGen = new BcV3CertGen(policyEnforcement);

            // Setup the certificate
            certGen.SetSerialNumber(nextCertSerial());
            certGen.SetIssuerDN(caCertificate.SubjectDN);
            certGen.SetSubjectDN(p10.Subject);
            certGen.SetPublicKey(p10.PublicKey);
            certGen.SetSignatureAlgorithm(signatureAlgorithm);
            if (certGen.GetVersion() == X509ver.V3)
            {
                ((V3CertGen)certGen).AddExtension(X509Extensions.AuthorityKeyIdentifier, false, new AuthorityKeyIdentifierStructure(caCertificate.GetPublicKey()));
                ((V3CertGen)certGen).AddExtension(X509Extensions.SubjectKeyIdentifier, false, new SubjectKeyIdentifierStructure(p10.PublicKey));
            }

            // Add further extensions either from profile or request attributes
            // If a profile is specified ignore all attributes apart from SubjAltName
            if (profile != null)
            {
                // Add in SubjAltName if there is one
                if ((p10.SubjectAltNames != null) && (certGen.GetVersion() == X509ver.V3))
                {
                    bool critical = p10.IsCritical(X509Extensions.SubjectAlternativeName);
                    ((V3CertGen)certGen).AddExtension(X509Extensions.SubjectAlternativeName, critical, p10.SubjectAltNames);
                }

                // Capture the profile name for database
                profileName = profile.Name;

                // cut the cert
                newCert = generate(certGen, profile, notBefore, notAfter);
            }
            else    // No profile
            {
                // Set the validity period
                certGen.SetNotBefore(notBefore.ToUniversalTime());
                certGen.SetNotAfter(notAfter.ToUniversalTime());

                // Do what it says in the request
                newCert = generate(certGen, p10.Extensions);
            }

            // Add certificate to the CA DB
            Database.AddCertificate(newCert, request.GetDerEncoded(), profileName, dbFileLocation, caCertificate, cspParam);
            logEvent(LogEvent.EventType.DBAddCert, "DB: Certificate added: " + newCert.SerialNumber.ToString());

            return newCert;
        }

        #endregion

        #region RevokeCertificate

        /// <summary>
        /// Revoke a certificate
        /// </summary>
        /// <param name="Certificate">Certificate to revoke</param>
        /// <param name="Reason">Revocation reason</param>
        /// <returns>Status of the certificate</returns>
        /// <exception cref="ApplicationException">Certificate not found</exception>
        /// <exception cref="ApplicationException">Duplicate serial number</exception>
        /// <exception cref="ApplicationException">Certificate is already revoked</exception>
        public string RevokeCertificate(X509Certificate Certificate, CRLReason Reason)
        {
            Database.RevokeCertificate(Certificate, Reason, dbFileLocation, caCertificate, cspParam);
            logEvent(LogEvent.EventType.RevokeCert, "Certificate revoked: " + Certificate.SerialNumber.ToString());

            return "revoked";
        }

        /// <summary>
        /// Unrevoke certificate. (unsuspend)
        /// </summary>
        /// <param name="Certificate">The certificate.</param>
        /// <returns></returns>
        public string UnRevokeCertificate(X509Certificate Certificate)
        {
            Database.unrevoke(Certificate, dbFileLocation, caCertificate, cspParam);
            logEvent(LogEvent.EventType.RevokeCert, "Certificate UNREVOKED: " + Certificate.SerialNumber.ToString());

            return "unrevoked";
        }

        #endregion

        #region IssueCRL

        /// <summary>
        /// Issue a CRL (containing all revoked certificates)
        /// </summary>
        /// <returns></returns>
        public virtual string IssueCRL()
        {
            X509V2CrlGenerator crlGen = new X509V2CrlGenerator();

            // Generate CRL
            try
            {
                createCRL(crlGen);
                X509Crl crl = crlGen.Generate(privateKey);

                // Write CRL to file
                File.WriteAllBytes(crlFileLocation, crl.GetEncoded());

                logEvent(LogEvent.EventType.IssueCert, "CRL Published. Serial: " + lastCRL);
            }
            catch (Exception ex)
            {
                LogEvent.WriteEvent(eventLog, LogEvent.EventType.Error, "Failed CRL issue: " + ex.Message);
                throw new ApplicationException("Failed CRL Issue", ex);
            }
            return lastCRL;
        }

        /// <summary>
        /// Creates a CRL.
        /// </summary>
        /// <param name="crlGen">The CRL generator.</param>
        protected void createCRL(X509V2CrlGenerator crlGen)
        {

            // Set generic fields
            crlGen.SetSignatureAlgorithm(signatureAlgorithm);
            crlGen.SetIssuerDN(caCertificate.SubjectDN);
            DateTime issueDate = DateTime.Now.ToUniversalTime();
            DateTime updateDate = issueDate.AddDays(crlInterval);
            crlGen.SetThisUpdate(issueDate);
            crlGen.SetNextUpdate(updateDate);

            // Set serial number
            crlGen.AddExtension(X509Extensions.CrlNumber, false, new CrlNumber(nextCrlSerial()));

            // Load the revocation entries
            Database.PopulateCRL(crlGen, dbFileLocation);
        }

        #endregion

        #region GetCRL
        /// <summary>
        /// Retrieve the current CRL
        /// </summary>
        /// <returns>The current CRL</returns>
        public X509Crl GetCRL()
        {
            X509CrlParser crlp = new X509CrlParser();
            try
            {
                return crlp.ReadCrl(File.ReadAllBytes(crlFileLocation));
            }
            catch (FileNotFoundException ex)
            {
                LogEvent.WriteEvent(eventLog, LogEvent.EventType.Error, "CRL not found: " + ex.Message);
                return null;
            }
            catch (IOException ex)
            {
                LogEvent.WriteEvent(eventLog, LogEvent.EventType.Error, "CRL not found: " + ex.Message);
                return null;
            }
        }

        #endregion

        #region Backup

        /// <summary>
        /// Backup the CA key material to a PKCS#12 file
        /// </summary>
        /// <param name="Password">Strong password used for encryption</param>
        /// <param name="OutFile">Outut file</param>
        public virtual void Backup(string Password, string OutFile)
        {
            //
            // TODO - make this do something
            //
        }

        #endregion

        #region Logging

        internal void logEvent(LogEvent.EventType id, string message)
        {
            LogEvent.WriteEvent(eventLog, id, message);
        }

        #endregion

        #region Shutdown

        /// <summary>
        /// Writes a 'CA Stopped' event to the log
        /// </summary>
        public void StopCA()
        {
            logEvent(LogEvent.EventType.StopCA, "CA Stopped");
        }

        #endregion
    }
}

