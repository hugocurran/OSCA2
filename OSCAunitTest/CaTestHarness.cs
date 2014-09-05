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
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509.Extension;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Pkcs;
using OSCA;
using OSCA.Crypto;
using OSCA.Offline;



namespace OSCAunitTest
{
    /* This namespace contains a number of classes that can be used to exercise different parts of OSCA by the
     * unit tests.
     * CaTestHarness - Provides a root CA for certificate and CRL signing
     *      InitialiseCA() - create the minimum instantiation of a root CA
     *      LoadCA() - instantiate an existing minimum CA
     *      
     * CertTestHarness - Provide support for generating certificates and extracting data from them
     *      RunCertGenerator - Populate X509CertificateGenerator with mandatory fields
     */

    #region CaTestHarness

    class CaTestHarness
    {
        public static string testHarnessLocation = @"C:\Users\Peter\Documents\Visual Studio 2010\Projects\OfflineSimpleCA\OSCAunitTest\CaTestHarness";
        public static string testCAConfigFile = testHarnessLocation + @"\CA\CAConfig.xml";
        public static string testCADbFile = testHarnessLocation + @"\CA\CADatabase.xml";
        public static string testSubCaProfile = testHarnessLocation + @"\subCA.xml";

        public static CAConfig BcConfig = new CAConfig()
        {
            name = "Test Harness (BC)",
            profile = CA_Profile.rootCA,
            pkAlgo = "RSA",
            pkSize = 1024,
            sigAlgo = "SHA1withRSA",
            keyUsage = (X509KeyUsage.KeyCertSign | X509KeyUsage.CrlSign),
            version = X509ver.V3,
            life = 2,
            units = "Years",
            FIPS140 = false,
            caType = CA_Type.simpleCA,
            location = testHarnessLocation + @"\CA",
            password = "foobar",
            crlInterval = 90,
            profileFile = testHarnessLocation + @"\CA\Profiles",
            DN = new X509Name("CN=Test Harness BC")
        };

        public static CAConfig SysConfig = new CAConfig()
        {
            name = "Test Harness (Sys)",
            profile = CA_Profile.rootCA,
            pkAlgo = "RSA",
            pkSize = 1024,
            sigAlgo = "SHA1withRSA",
            keyUsage = (X509KeyUsage.KeyCertSign | X509KeyUsage.CrlSign),
            version = X509ver.V3,
            life = 2,
            units = "Years",
            FIPS140 = true,
            caType = CA_Type.fipsCA,
            location = testHarnessLocation + @"\CA",
            password = "foobar",
            crlInterval = 90,
            profileFile = testHarnessLocation + @"\CA\Profiles",
            DN = new X509Name("CN=Test Harness Sys")
        };

        public static void InitialiseCA(bool FipsMode)
        {
            Directory.CreateDirectory(testHarnessLocation + @"\CA");

            // Setup the CA type
            CAConfig config;

            if (FipsMode)
                config = SysConfig;
            else
                config = BcConfig;

            string cAConfigFile = CaFactory.CreateRootCA(config);

            if (cAConfigFile != testCAConfigFile)
                throw new ApplicationException("Mismatch in config file names");
        }

        public static ICA LoadCA()
        {
            return OSCA.OSCA.LoadCA(testCAConfigFile, "foobar");
        }

        public static void Cleanup()
        {
            Directory.Delete(testHarnessLocation + @"\CA", true);
        }
    }

    #endregion

    #region CertTestHarness

    public static class CertTestHarness
    {
        /*
        public static X509Certificate RunCertGenerator(bool FipsMode)
        {
            ICertGen gen;

            if (FipsMode)
                gen = new SysV3CertGen();
            else
                gen = new BcV3CertGen();
            
            return RunCertGenerator(gen);
        }
    */

        /// <summary>
        /// Populate a CertificateGenerator with the mandatory fields and create certificate
        /// </summary>
        /// <param name="gen">X509CertificateGenerator object</param>
        /// <returns>Certificate</returns>
        public static X509Certificate RunCertGenerator(BcV3CertGen gen)
        {
            return RunCertGenerator(gen, false);
        }

        /// <summary>
        /// Populate a CertificateGenerator with the mandatory fields and create certificate
        /// </summary>
        /// <param name="gen">X509CertificateGenerator object</param>
        /// <param name="expired">True for an expired certificate</param>
        /// <returns>Certificate</returns>
        public static X509Certificate RunCertGenerator(BcV3CertGen gen, bool expired)
        {
            // generate a key pair
            SecureRandom random = new SecureRandom(new CryptoApiRandomGenerator());
            KeyGenerationParameters genParam = new KeyGenerationParameters(random, 1024);
            RsaKeyPairGenerator rsaGenerator = new RsaKeyPairGenerator();
            rsaGenerator.Init(genParam);
            AsymmetricCipherKeyPair keyPair = rsaGenerator.GenerateKeyPair();

            gen.SetSerialNumber(new BigInteger("42"));
            gen.SetIssuerDN(new X509Name("CN=Test Cert"));
            gen.SetSubjectDN(new X509Name("CN=Test Cert"));
            gen.SetPublicKey(keyPair.Public);
            gen.SetSignatureAlgorithm("SHA1withRSA");
            gen.AddExtension(X509Extensions.AuthorityKeyIdentifier, true, new AuthorityKeyIdentifierStructure(keyPair.Public));
            gen.AddExtension(X509Extensions.SubjectKeyIdentifier, true, new SubjectKeyIdentifierStructure(keyPair.Public));

            if (expired)
            {
                DateTime old = new DateTime(2000, 01, 01);
                gen.SetNotBefore(old);
                gen.SetNotAfter(old.AddYears(1));
            }
            else
            {
                gen.SetNotBefore(DateTime.Today);
                gen.SetNotAfter(DateTime.Today.AddYears(1));
            }

            return gen.Generate(keyPair.Private);
        }

        /// <summary>
        /// Return an extension for a given OID
        /// </summary>
        /// <param name="Cert">Certificate to parse</param>
        /// <param name="Oid">Extension OID to retrieve</param>
        /// <returns>Extension</returns>
        public static X509Extension ExtractExtension(X509Certificate Cert, DerObjectIdentifier Oid)
        {
            X509Extensions exts = Cert.GetExtensions();
            return exts.GetExtension(Oid);
        }
    }

        #endregion

        #region RqstTestHarness

    public static class RqstTestHarness
    {
        public static Pkcs10CertificationRequest CreateRequest()
        {
            AsymmetricCipherKeyPair keyPair = BcKeyManager.Create(
                                CaTestHarness.BcConfig.pkSize,
                                CaTestHarness.BcConfig.pkAlgo);

            Pkcs10CertificationRequest p10 = new Pkcs10CertificationRequest(
                                            CaTestHarness.BcConfig.sigAlgo,
                                            CaTestHarness.BcConfig.DN,
                                            keyPair.Public,
                                            null,
                                            keyPair.Private);

            return p10;
        }
    }

        #endregion


}
