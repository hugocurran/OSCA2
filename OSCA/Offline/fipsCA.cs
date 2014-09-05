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
using System.Xml.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto;
using OSCA.Log;
using System.IO;
using Org.BouncyCastle.Security;


namespace OSCA.Offline
{

    /// <summary>
    /// A basic implementation of a Certification Authority using FIPS 140 (System) crypto
    /// </summary>
    public class fipsCA : simpleCA, ICA
    {
            
        #region Constructor

        /// <summary>
        /// Construct a CA object
        /// </summary>
        /// <param name="ConfigFile">Full pathname to config file</param>
        public fipsCA(string ConfigFile) : base ()
        {
            configFile = ConfigFile;

            // Read in the configuration
            XDocument config;
            if (XmlSigning.VerifyXmlFile(configFile))
                config = XDocument.Load(configFile);
            else
                throw new GeneralSecurityException("Signature failed on CA config file");

            XElement ca = config.Element("OSCA").Element("CA");
            fips140 = Convert.ToBoolean(ca.Element("fips140").Value);
            if (!fips140)
                throw new InvalidOperationException("Invalid FIPS140 flag for this CA instance");

            if (ca.Element("rqstPending") != null)
                throw new InvalidOperationException("CA is not configured: Request pending");

            name = ca.Element("name").Value;
            type = ca.Element("type").Value;
            dbFileLocation = ca.Element("dbFileLocation").Value;
            publicKeyAlgorithm = ca.Element("publicKeyAlgorithm").Value;
            publicKeySize = ca.Element("publicKeySize").Value;
            signatureAlgorithm = ca.Element("signatureAlgorithm").Value;
            lastSerial = ca.Element("lastSerial").Value;
            crlFileLocation = ca.Element("crlFileLocation").Value;
            lastCRL = ca.Element("lastCRL").Value;
            crlInterval = Convert.ToDouble(ca.Element("crlInterval").Value);
            profilesLocation = ca.Element("profilesLocation").Value;

            cspParam = SysKeyManager.Read(name);

            X509CertificateParser cp = new X509CertificateParser();
            caCertificate = cp.ReadCertificate(Convert.FromBase64String(ca.Element("caCert").Value));

            // Setup the Event Logger
            eventLog = new Logger(ca.Element("logFileLocation").Value, caCertificate, cspParam);

            // Log startup event
            logEvent(LogEvent.EventType.StartCA, "CA Started");

            // Expire any old certificates
            Database.ExpireCertificate(dbFileLocation, caCertificate, cspParam);
        }
        #endregion

        /// <summary>
        /// Backup the CA key material to a PKCS#12 file
        /// </summary>
        /// <param name="Password">Strong password used for encryption</param>
        /// <param name="OutputFile">Full pathname to the PKCS#12 output file</param>
        public override void Backup(string Password, string OutputFile)
        {
            try
            {
                SysKeyManager.ExportToP12(cspParam, caCertificate, OutputFile, Password, name);

                logEvent(LogEvent.EventType.BackupCAKey, "CA Key backup: " + OutputFile);
            }
            catch (Exception ex)
            {
                logEvent(LogEvent.EventType.Error, "Failed key backup: " + ex.Message);
                throw new ApplicationException("Failed Key Backup", ex);
            }
        }

        /// <summary>
        /// Generates the certificate.
        /// </summary>
        /// <param name="gen">The generator.</param>
        /// <param name="profile">The profile.</param>
        /// <param name="notBefore"></param>
        /// <param name="notAfter"></param>
        /// <returns></returns>
        protected override X509Certificate generate(ICertGen gen, Profile.Profile profile, DateTime notBefore, DateTime notAfter)
        {
            return gen.Generate(cspParam, profile, notBefore, notAfter);
        }

        /// <summary>
        /// Generate a certificate
        /// </summary>
        /// <remarks>All extensions in request will be included in the certificate</remarks>
        /// <param name="gen">Certificate generator instance</param>
        /// <returns>New certificate</returns>
        protected override X509Certificate generate(ICertGen gen)
        {
            return gen.Generate(cspParam);
        }

        /// <summary>
        /// Generate a certificate
        /// </summary>
        /// <remarks>Only the extensions supplied will be included in the certificate</remarks>
        /// <param name="gen">Certificate generator instance</param>
        /// <param name="ext">Extensions to include in the certificate</param>
        /// <returns>New certificate</returns>
        protected override X509Certificate generate(ICertGen gen, X509Extensions ext)
        {
            return gen.Generate(cspParam, ext);
        }

        /// <summary>
        /// Issue a CRL (containing all revoked certificates)
        /// </summary>
        /// <returns>CRL number</returns>
        public override string IssueCRL()
        {
            SysCrlGen crlGen = new SysCrlGen();

            // Generate CRL
            try
            {
                createCRL(crlGen);
                X509Crl crl = crlGen.Generate(cspParam);

                // Write CRL to file
                File.WriteAllBytes(crlFileLocation, crl.GetEncoded());

                logEvent(LogEvent.EventType.IssueCert, "CRL Published. Serial: " + lastCRL);
            }
            catch (Exception ex)
            {
                logEvent(LogEvent.EventType.Error, "Failed CRL issue: " + ex.Message);
                throw new ApplicationException("Failed CRL Issue", ex);
            }

            return lastCRL;
        }
 
    }
}

