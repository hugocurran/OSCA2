using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using OSCA.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using OSCA.Log;
using Org.BouncyCastle.Asn1.X509;
using System.IO;
using OSCA.Policy;

namespace OSCA.Offline
{
    /// <summary>
    /// Class describing a generic System Crypto CA
    /// </summary>
    /// <remarks>This is a Version 3.x class</remarks>
    public class sysCA : OSCA_CA
    {
        /// <summary>
        /// Create an instance of a generic System Crypto CA
        /// </summary>
        /// <param name="ConfigFile">Full pathname to config file</param>
        /// <exception cref="InvalidParameterException">Invalid FIPS140 flag for this CA instance</exception>
        public sysCA(string ConfigFile) : base (ConfigFile)
        {
            // Make sure the CA_Type is correct
            if (!fips140)
                throw new InvalidParameterException("Invalid FIPS140 flag for this CA instance");

            // Get a reference to the key container for the signing key
            cspParam = SysKeyManager.Read(name);

            X509CertificateParser cp = new X509CertificateParser();
            caCertificate = cp.ReadCertificate(Convert.FromBase64String(ca.Element("caCert").Value));

            // Setup CA policy
            if (ca.Element("policyEnforcement") != null)
            {
                policyEnforcement = PolicyEnforcementFactory.initialise(caCertificate, ca.Element("policyEnforcement"));
            }
            // Setup the logger
            startLogging();

            // Expire any old certificates
            Database.ExpireCertificate(dbFileLocation, caCertificate, cspParam);
        }

        /// <summary>
        /// Backup the CA key material to a PKCS#12 file
        /// </summary>
        /// <param name="Password">Strong password used for encryption</param>
        /// <param name="OutputFile">Full pathname to the PKCS#12 output file</param>
        /// <exception cref="System.ApplicationException">Failed Key Backup</exception>
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
        /// Return a certificate generator instance for this CA
        /// </summary>
        /// <returns>Certificate generator</returns>
        protected override ICertGen getCertificateGenerator()
        {
            // ToDo - test against policy
            return new SysV3CertGen(policyEnforcement);
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
            return ((SysV3CertGen)gen).Generate(cspParam, profile, notBefore, notAfter);
        }

        /// <summary>
        /// Generate a certificate
        /// </summary>
        /// <remarks>All extensions in request will be included in the certificate</remarks>
        /// <param name="gen">Certificate generator instance</param>
        /// <returns>New certificate</returns>
        protected override X509Certificate generate(ICertGen gen)
        {
            return ((SysV3CertGen)gen).Generate(cspParam);
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
            return ((SysV3CertGen)gen).Generate(cspParam, ext);
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
