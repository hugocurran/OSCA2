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
//using System.Numerics;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Math;

using Sys = System.Security.Cryptography.X509Certificates;


namespace OSCA.Crypto.CNG
{

    /// <summary>
    /// Class that manages keypairs using the system CNG crypto library
    /// </summary>
    /// <remarks>Only EC keypairs are supported</remarks>
    public class CngKeyManager
    {

        #region Create keys

        /// <summary>
        /// Create a CNG keypair
        /// </summary>
        /// <param name="pkAlgo">Key algorithm (from supported set)</param>
        /// <param name="name">Key container name</param>
        /// <returns>
        /// CNG keypair
        /// </returns>
        /// <exception cref="System.NotSupportedException">Creation of key not supported for:  + pkAlgo.Algorithm</exception>
        /// <remarks>
        /// Keys are exportable
        /// </remarks>
        public static CngKey Create(CngAlgorithm pkAlgo, string name)
        {
            // Normalise the name
            string _name = name.Replace(' ', '_');

            CngKeyCreationParameters keyParams = new CngKeyCreationParameters();
            keyParams.ExportPolicy = CngExportPolicies.AllowPlaintextExport;
            keyParams.KeyCreationOptions = CngKeyCreationOptions.OverwriteExistingKey;
            keyParams.Provider = CngProvider.MicrosoftSoftwareKeyStorageProvider;

            // Note that CngAlgorithm is not an enum
            if (pkAlgo.Algorithm.Contains("ECDSA"))
                keyParams.KeyUsage = CngKeyUsages.Signing;
            else if (pkAlgo.Algorithm.Contains("ECDH"))
                keyParams.KeyUsage = CngKeyUsages.KeyAgreement;
            else
                throw new NotSupportedException("Creation of key not supported for: " + pkAlgo.Algorithm);

            return CngKey.Create(pkAlgo, _name, keyParams);
        }

        #endregion
        

        #region Get Public Key

        public static ECPublicKeyParameters getPublicKey(CngKey key)
        {
            return EccKeyBlob.UnpackEccPublicBlob(key.Export(CngKeyBlobFormat.EccPublicBlob));
        }
        
        #endregion


        #region Get CngKey reference

        public static CngKey getKeyRef(string name)
        {
            // Normalise the name
            string _name = name.Replace(' ', '_');

            return CngKey.Open(_name);
        }

        #endregion


        #region Export as PKCS#12
       
        public static void ExportToP12(CngKey key, X509Certificate cert, string outputFile, string password, string name)
        {
            // Normalise the name
            string nName = name.Replace(' ', '_');

            // Use the FIPS-140 system prng
            SecureRandom random = new SecureRandom(new CryptoApiRandomGenerator());

            // Build PKCS#12
            Pkcs12StoreBuilder p12 = new Pkcs12StoreBuilder();
            Pkcs12Store pkcs = p12.Build();

            byte[] privateKey = key.Export(CngKeyBlobFormat.EccPrivateBlob);

            ECPrivateKeyParameters privateParams = EccKeyBlob.UnpackEccPrivateBlob(privateKey);

            pkcs.SetKeyEntry(nName, 
                    new AsymmetricKeyEntry(privateParams), 
                    new X509CertificateEntry[] { new X509CertificateEntry(cert) });


            Stream stream = new FileStream(outputFile, FileMode.Create);
            pkcs.Save(stream, password.ToCharArray(), random);
            stream.Close();
        }
        

        #endregion

        #region Import from PKCS#12

        /// <summary>
        /// Import key and certificate from a PKCS#12 file.
        /// </summary>
        /// <param name="P12">The PKCS#12 file</param>
        /// <param name="pkAlgo">The public key algorithm</param>
        /// <param name="name">The certificate name</param>
        /// <param name="password">The decryption password</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Algorithm not supported</exception>
        public static X509Certificate ImportFromP12(byte[] P12, string pkAlgo, string name, char[] password)
        {
            // TODO - SUPPORT ALGORITHMS OTHER THAN RSA
            pkAlgo = "RSA";

            // Normalise the name
            string nName = name.Replace(' ', '_');
            
            //Read in the private key and certificate
            MemoryStream p12stream = new MemoryStream(P12);

            Pkcs12Store p12 = new Pkcs12Store(p12stream, password);

            X509Certificate cert = p12.GetCertificate(nName).Certificate;

            CspParameters cp = null;
            switch (pkAlgo)
            {
                case "RSA":
                    cp = new CspParameters(24, "Microsoft Enhanced RSA and AES Cryptographic Provider");
                    cp.KeyContainerName = nName;
                    //cp.Flags = CspProviderFlags.UseArchivableKey;
                    
                    RSAParameters rp = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)p12.GetKey(nName).Key);

                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp))
                    {
                        rsa.ImportParameters(rp);
                        rsa.PersistKeyInCsp = true;
                    }
                    break;

                case "DSA":
                //case "ECDSA":
                //ECKeyPairGenerator ecGenerator = new ECKeyPairGenerator(pkAlgo);
                //ecGenerator.Init(genParam);
                //keyPair = ecGenerator.GenerateKeyPair();
                //break;
                default:
                    throw new ArgumentException("Algorithm not supported", pkAlgo);
            }
            return cert;
        }

        #endregion

        #region Load CngKey

        /*

        internal static CngKey LoadKey(AsymmetricKeyParameter privateKey)
        {
            // Load the BC privateKey into a CspParameters object
            CspParameters cp = new CspParameters(24, "Microsoft Enhanced RSA and AES Cryptographic Provider");

            RSAParameters rp = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)privateKey);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp))
            {
                rsa.ImportParameters(rp);
                rsa.PersistKeyInCsp = true;
            }
            return cp;
        }
         * 
         * */

        #endregion

    }
}
