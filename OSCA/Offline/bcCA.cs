using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Org.BouncyCastle.Crypto;
using System.IO;
using Org.BouncyCastle.Pkcs;
using OSCA.Policy;
using OSCA.Crypto;
using OSCA.Log;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;

namespace OSCA.Offline
{
    /// <summary>
    /// Class describing a generic Bouncy Castle Crypto CA
    /// </summary>
    /// <remarks>This is a Version 3.x class</remarks>
    public class bcCA : OSCA_CA
    {
        /// <summary>
        /// Signing key for the CA
        /// </summary>
        private AsymmetricKeyParameter privateKey;

        /// <summary>
        /// Password for the CA key file
        /// </summary>
        private char[] password;

        /// <summary>
        /// Create an instance of a generic Bouncy Castle Crypto CA
        /// </summary>
        /// <param name="ConfigFile">Full pathname to config file</param>
        /// <param name="Password">Password for CA key file</param>
        public bcCA(string ConfigFile, string Password) : base (ConfigFile)
        {
            // Make sure the CA_Type is correct
            if (fips140)
                throw new InvalidParameterException("Invalid FIPS140 flag for this CA instance");

            this.password = Password.ToCharArray();

            //Read in the private key and certificate
            MemoryStream p12stream = new MemoryStream(Convert.FromBase64String(ca.Element("caKey").Value));
            Pkcs12Store p12 = new Pkcs12Store(p12stream, password);
            this.privateKey = p12.GetKey(this.name).Key;
            this.caCertificate = p12.GetCertificate(this.name).Certificate;

            // Setup CA policy
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
        /// Return a certificate generator instance for this CA
        /// </summary>
        /// <returns>Certificate generator</returns>
        protected override ICertGen getCertificateGenerator()
        {
            // ToDo - Could test this against policy?
            return new BcV3CertGen(policyEnforcement);
        }        

        /// <summary>
        /// Generates the certificate.
        /// </summary>
        /// <param name="gen">The generator.</param>
        /// <param name="profile">The profile.</param>
        /// <param name="notBefore">The not before.</param>
        /// <param name="notAfter">The not after.</param>
        /// <returns></returns>
        protected override X509Certificate generate(ICertGen gen, Profile.Profile profile, DateTime notBefore, DateTime notAfter)
        {
            return ((BcV3CertGen)gen).Generate(privateKey, profile, notBefore, notAfter);
        }

        /// <summary>
        /// Generates the certificate.
        /// </summary>
        /// <param name="gen">The generator.</param>
        /// <returns></returns>
        protected override X509Certificate generate(ICertGen gen)
        {
            return ((BcV3CertGen)gen).Generate(privateKey);
        }

        /// <summary>
        /// Generates the certificate.
        /// </summary>
        /// <param name="gen">The generator.</param>
        /// <param name="ext">The extensions.</param>
        /// <returns></returns>
        protected override X509Certificate generate(ICertGen gen, X509Extensions ext)
        {
            return ((BcV3CertGen)gen).Generate(privateKey, ext);
        }

        /// <summary>
        /// Issue a CRL (containing all revoked certificates)
        /// </summary>
        /// <returns></returns>
        public override string IssueCRL()
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
        /// Backup the CA key material to a PKCS#12 file
        /// </summary>
        /// <param name="Password">Strong password used for encryption</param>
        /// <param name="OutFile">Outut file</param>
        public override void Backup(string Password, string OutFile)
        {
            //
            // TODO - make this do something
            //
        }
    }
}
