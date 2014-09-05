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
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using OSCA.Crypto;
using OSCA.Log;
using System.Xml;


namespace OSCA.Offline
{

    /*  This class creates a new CA.  It requires a struct with the following values defined:
     *      name        - The name of the CA (used as the subject nane (CN=<name>)
     *      pkAlgo     - Algorithm for PK key-pair (eg RSA)
     *      pkSize     - Size of the public key
     *      hash        - Hashing algorithm (eg SHA1) 
     *      KeyUsage    - Concatenation of X509KeyUsage values
     *      version     - X.509 certificate version
     *      life        - Value of certificate lifetime
     *      units       - Units of certificate lifetime
     *      FIPS140     - True to use .Net crypto library, false for BC
     *      location    - Pathname of folder to create CA files in
     *      password    - Password to encrypt CA key material
     *      crlInterval - How often CRLs are issued (in days)
     */

    /// <summary>
    /// CA Factory
    /// </summary>
    public static class CaFactory
    {

        #region Create Root CA

        // Key material
        private static AsymmetricCipherKeyPair keyPair = null;
        private static AsymmetricKeyParameter publicKey = null;
        private static CspParameters cspParam = null;
        //private static X509Certificate caCert = null;
        private static Logger eventLog = null;

        /// <summary>
        /// Create a root (self-signed) CA
        /// </summary>
        /// <param name="Config">CA Config structure containing the initialisation parameters</param>
        /// <returns>Full pathname for the configuration file</returns>
        public static string CreateRootCA(CAConfig Config)
        {
            if (Config.profile != CA_Profile.rootCA)
                throw new ArgumentException("Invalid profile specified", Config.profile.ToString());

            // Start/end dates
            DateTime startDate = DateTime.UtcNow;
            DateTime expiryDate;
            switch (Config.units)
            {
                case "Years":
                    expiryDate = startDate.AddYears(Config.life);
                    break;
                case "Months":
                    expiryDate = startDate.AddMonths(Config.life);
                    break;
                case "Days":
                    expiryDate = startDate.AddDays(Config.life);
                    break;
                default:
                    throw new ArgumentException("Invalid lifetime unit", Config.units);
            }

            // Serial number
            BigInteger serialNumber = new BigInteger(1, BitConverter.GetBytes(DateTime.Now.Ticks));

            // Certificate
            ICertGen certGen;

            // The OLD method was to use the Config.pkAlgo field to select a hard-coded CSP
            // The new approach is to have the user select a CSP
            //OLD method
            if (Config.FIPS140)
            {
                cspParam = SysKeyManager.Create(Config.pkSize, Config.pkAlgo, Config.name);
                publicKey = SysKeyManager.getPublicKey(cspParam, Config.pkAlgo);
                if (Config.version == X509ver.V1)
                    certGen = new SysV1CertGen();
                else
                    certGen = new SysV3CertGen();
            }
            else
            {
                keyPair = BcKeyManager.Create(Config.pkSize, Config.pkAlgo);
                // Create a system CspParameters entry for use by XmlSigner
                cspParam = SysKeyManager.LoadCsp(keyPair.Private);

                publicKey = keyPair.Public;
                if (Config.version == X509ver.V1)
                    certGen = new BcV1CertGen();
                else
                    certGen = new BcV3CertGen();
            }

            //NEW method
            if ((Config.FIPS140) && (Config.CSPNum > 0))    // System crypto
            {
                cspParam = SysKeyManager.Create(Config.pkSize, Config.CSPName, Config.CSPNum, Config.name);
            }
            
            // V1 and V3 fields
            certGen.SetSerialNumber(serialNumber);
            certGen.SetIssuerDN(Config.DN);
            certGen.SetNotBefore(startDate);
            certGen.SetNotAfter(expiryDate);
            certGen.SetSubjectDN(Config.DN);
            certGen.SetPublicKey(publicKey);
            certGen.SetSignatureAlgorithm(Config.sigAlgo);

            // V3 extensions
            if (Config.version == X509ver.V3)
            {
                    certGen.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, new AuthorityKeyIdentifierStructure(publicKey));
                    certGen.AddExtension(X509Extensions.SubjectKeyIdentifier, false, new SubjectKeyIdentifierStructure(publicKey));
                    certGen.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(true));
                    certGen.AddExtension(X509Extensions.KeyUsage, true, new X509KeyUsage(Config.keyUsage));
            }

            X509Certificate caCert;
            if (Config.FIPS140)
                caCert = certGen.Generate(cspParam);
            else
                caCert = certGen.Generate(keyPair.Private);

            string configFile;
            if (Config.FIPS140)
            {
                // Create the CA Config file
                configFile = createFinalCAConfig(Config, serialNumber, caCert, null);
                LogEvent.WriteEvent(eventLog, LogEvent.EventType.CreateCA, "Root CA (FIPS) Created: " + configFile);
            }
            else
            {
                // Store key material in a PKCS#12 file
                MemoryStream stream = BcKeyManager.SaveP12(keyPair.Private, caCert, Config.password, Config.name);
                string caKey = Convert.ToBase64String(stream.ToArray());

                // Create the CA Config file
                configFile = createFinalCAConfig(Config, serialNumber, null, caKey);
                LogEvent.WriteEvent(eventLog, LogEvent.EventType.CreateCA, "Root CA (BC) Created: " + configFile);
             }   
         
            // Create CA database
            string dbFile = Database.CreateDB(Config, caCert, cspParam);

            // Insert Root CA certificate
            byte[] dummy = new byte[0];
            Database.AddCertificate(caCert, dummy, "rootCA", dbFile, caCert, cspParam);

            return configFile; 
        }

        #endregion
        
        #region Create Subordinate CA
        /// <summary>
        /// Create a new Subordinate CA using the setup parameters from a CAConfig object
        /// The Issuing CA must be available to create and sign a certificate
        /// </summary>
        /// <param name="Config">CAConfig object</param>
        /// <param name="IssuingCA">Object reference for issuing CA</param>
        /// <returns>Full pathname of CA config file</returns>
        public static string CreateSubCA(CAConfig Config, ICA IssuingCA)
        {
            if (Config.profile != CA_Profile.SubCA)
                throw new ArgumentException("Invalid profile specified", Config.profile.ToString());         

            // Serial number
            BigInteger serialNumber = new BigInteger(1, BitConverter.GetBytes(DateTime.Now.Ticks));


            // Key material
            Pkcs10CertificationRequest p10;

            if (Config.FIPS140)
            {
                cspParam = SysKeyManager.Create(Config.pkSize, Config.pkAlgo, Config.name);

                // PKCS#10 Request
                p10 = new Pkcs10CertificationRequestDelaySigned(
                                                Config.sigAlgo,
                                                Config.DN,
                                                SysKeyManager.getPublicKey(cspParam, Config.pkAlgo),
                                                null);
                // Signature
                byte[] buffer = ((Pkcs10CertificationRequestDelaySigned)p10).GetDataToSign();
                byte[] signature = SysSigner.Sign(buffer, cspParam, Config.sigAlgo);
                ((Pkcs10CertificationRequestDelaySigned)p10).SignRequest(signature);              
            }
            else
            {
                keyPair = BcKeyManager.Create(Config.pkSize, Config.pkAlgo);
                // Create a system CspParameters entry for use by XmlSigner
                cspParam = SysKeyManager.LoadCsp(keyPair.Private);

                // PKCS#10 Request
                p10 = new Pkcs10CertificationRequest(
                                                Config.sigAlgo,
                                                Config.DN,
                                                keyPair.Public,
                                                null,
                                                keyPair.Private);
            }
            // Test the signature
            if (!p10.Verify())
                throw new SignatureException("Cannot validate POP signature");

            // Request cert from issuing CA
            X509Certificate cert = IssuingCA.IssueCertificate(p10, new Profile.Profile(Config.profileFile));

            string configFile;
            if (Config.FIPS140)
            {
                // Create the CA Config file
                configFile = createFinalCAConfig(Config, serialNumber, cert, null);
                LogEvent.WriteEvent(eventLog, LogEvent.EventType.CreateCA, "Subordinate CA (FIPS) Created: " + configFile);
            }
            else
            {
                // Store key material in a PKCS#12 file
                MemoryStream stream = BcKeyManager.SaveP12(keyPair.Private, cert, Config.password, Config.name);
                string caKey = Convert.ToBase64String(stream.ToArray());

                // Create the CA Config file
                configFile = createFinalCAConfig(Config, serialNumber, null, caKey);
                LogEvent.WriteEvent(eventLog, LogEvent.EventType.CreateCA, "Root CA (BC) Created: " + configFile);
            }
            // Create CA database
            Database.CreateDB(Config, cert, cspParam);

            return configFile;
        }


        /// <summary>
        /// Create a new Subordinate CA certificate request using the setup parameters from a CAConfig object
        /// </summary>
        /// <remarks>Only System cryptography supported</remarks>
        /// <param name="Config">CAConfig object</param>
        /// <returns>PKCS#10 certificate request</returns>
        public static Pkcs10CertificationRequest CreateSubCA(CAConfig Config)
        {
            if (Config.profile != CA_Profile.SubCA)
                throw new ArgumentException("Invalid profile specified", Config.profile.ToString());

            if (!Config.FIPS140)
                throw new InvalidParameterException("Only FIPS mode supported");

            // Serial number
            BigInteger serialNumber = new BigInteger(1, BitConverter.GetBytes(DateTime.Now.Ticks));

            // Key material
            CspParameters cspParam = SysKeyManager.Create(Config.pkSize, Config.pkAlgo, Config.name);
            
            // PKCS#10 Request
            Pkcs10CertificationRequestDelaySigned p10 = new Pkcs10CertificationRequestDelaySigned(
                                             Config.sigAlgo,
                                             Config.DN,
                                             SysKeyManager.getPublicKey(cspParam, Config.pkAlgo),
                                             null);
            
            // Signature
            byte[] buffer = p10.GetDataToSign();
            byte[] signature = SysSigner.Sign(buffer, cspParam, Config.sigAlgo);
            p10.SignRequest(signature);              

            if (!p10.Verify())
                throw new SignatureException("Cannot validate POP signature");

            // Create the CA Config file
            createPendingCAConfig(Config, serialNumber, p10, "");

            return p10;
        }

        /// <summary>
        /// Complete the creation of a Subordinate CA using the certificate returned from the issuing CA.
        /// A pending request must exist
        /// </summary>
        /// <param name="ConfigFile">Pathname for the CA config file</param>
        /// <param name="Certificate">SubCA certificate</param>
        public static void CreateSubCA(string ConfigFile, X509Certificate Certificate)
        {
            // Read in the configuration
            XDocument config = XDocument.Load(ConfigFile);
            XElement ca = config.Element("OSCA").Element("CA");

            // Sanity checks
            bool fips140 = Convert.ToBoolean(ca.Element("fips140").Value);
            if (!fips140)
                throw new InvalidOperationException("Invalid FIPS140 flag for this CA instance");
            if (ca.Element("rqstPending") == null)
                throw new InvalidOperationException("No pending certificate request");

            // Update the config file
            ca.Element("rqstPending").ReplaceWith(new XElement("caCert", Utility.cert64(Certificate)));

            // Sign and save the config file
            XmlSigning.SignXml(config, ConfigFile, Certificate, cspParam);

            // Create CA database
            Database.CreateDB(ConfigFile, Certificate, cspParam);
        }

        #endregion

        #region Create CA Config File

        /// <summary>
        /// Create a final CA Config file 
        /// </summary>
        /// <param name="config">CA Config object</param>
        /// <param name="serialNumber">Initial serial number</param>
        /// <param name="cert">CA certificate</param>
        /// <param name="caKey">CA keyfile</param>
        /// <returns>CA Config file location</returns>
        private static string createFinalCAConfig(CAConfig config, BigInteger serialNumber, X509Certificate cert, string caKey)
        {
            string version = "";
            string caEntry = "";
            string caValue = "";

            if (config.FIPS140)
            {
                version = "3.1";
                caEntry = "caCert";
                caValue = Utility.cert64(cert);
            }
            else
            {
                version = "3.0";
                caEntry = "caKey";
                caValue = caKey;
            }

            return createCaConfFile(config, serialNumber, version, caEntry, caValue, cert);
        }

        /// <summary>
        /// Create an initial CA Config file 
        /// </summary>
        /// <param name="config">CA Config object</param>
        /// <param name="serialNumber">Initial serial number</param>
        /// <param name="request">Certificate request</param>
        /// <param name="caKey">CA keyfile</param>
        /// <returns>CA Config file location</returns>
        private static string createPendingCAConfig(CAConfig config, BigInteger serialNumber, Pkcs10CertificationRequest request, string caKey)
        {
            string version = "3.1";
            string caEntry = "rqstPending";
            string caValue = Utility.rqst64(request);

            return createCaConfFile(config, serialNumber, version, caEntry, caValue, null);
        }
        

        private static string createCaConfFile(CAConfig config, BigInteger serialNumber, string version, string caEntry, string caValue, X509Certificate caCert)
        {
            string crlFileLocation = config.location + "\\" + config.name + ".crl";
            string dbfile = config.location + "\\CADatabase.xml";
            string logFile = config.location + "\\CALog.xml";
            string profilesLocation = config.location + "\\Profiles";

            // Create CA config file
            XDocument conf = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("CA Configuration"),
                new XElement("OSCA",
                    new XAttribute("version", version),
                    new XElement("CA",
                        new XElement("name", config.name),
                        new XElement("dn", config.DN),
                        new XElement("type", config.profile.ToString()),        // Called 'type' for historical reasons
                        new XElement("caType", config.caType.ToString()),
                        new XElement("created", DateTime.Now.ToUniversalTime()),
                        new XElement("dbFileLocation", dbfile),
                        new XElement("logFileLocation", logFile),
                        new XElement("profilesLocation", profilesLocation),
                        new XElement("publicKeyAlgorithm", config.pkAlgo),
                        new XElement("publicKeySize", config.pkSize),
                        new XElement("signatureAlgorithm", config.sigAlgo),
                        new XElement("fips140", config.FIPS140),
                        new XElement("cryptoServiceProvider",
                            new XAttribute("cspNumber", config.CSPNum), 
                            config.CSPName),
                        new XElement("lastSerial", serialNumber),
                        new XElement("crlFileLocation", crlFileLocation),
                        new XElement("lastCRL", 0),
                        new XElement("crlInterval", config.crlInterval),
                        new XElement("policyEnforcement"),
                        new XElement(caEntry, caValue)
                    )
               )
            );

            // Do the policyEnforcement bit
            XElement pe = new XElement("policyEnforcement");
            if (config.rollOver != 0)
                pe.Add(new XElement("keyRollOver", config.rollOver.ToString()));

            XElement policyEnforcement = conf.Element("OSCA").Element("CA").Element("policyEnforcement");
            policyEnforcement.ReplaceWith(pe);

            // Sign and save the config file
            // Can't sign for a pending request! (no certificate)
            string configFile = config.location + "\\CAConfig.xml";
            if (caCert != null)
            {
                XmlSigning.SignXml(conf, configFile, caCert, cspParam);
                // Setup the log file
                LogFile.createLogFile(logFile, version, caCert, cspParam);
                eventLog = new Logger(logFile, caCert, cspParam);
            }
            else
            {
                XmlWriter xmlw = XmlWriter.Create(new StreamWriter(configFile));
                conf.WriteTo(xmlw);
                xmlw.Flush();
                xmlw.Close();
            }
            return configFile;
        }

        #endregion
    }
}
