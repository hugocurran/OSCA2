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
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Sys = System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto.Parameters;


namespace OSCA.Crypto
{

    /// <summary>
    /// Class that manages keypairs using the system crypto library
    /// </summary>
    public class SysKeyManager
    {

        #region Create keys

        /// <summary>
        /// Create a key pair
        /// </summary>
        /// <param name="pkSize">Key size</param>
        /// <param name="pkAlgo">Key algorithm</param>
        /// <param name="name">Key container name</param>
        /// <returns></returns>
        internal static CspParameters Create(int pkSize, string pkAlgo, string name)
        {
            // Normalise the name
            string _name = name.Replace(' ', '_');

            CspParameters cp = null;
            switch (pkAlgo)
            {
                case "RSA":
                    cp = new CspParameters(24, "Microsoft Enhanced RSA and AES Cryptographic Provider");
                    cp.KeyContainerName = _name;
                    cp.Flags = CspProviderFlags.UseArchivableKey;
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(pkSize, cp))
                    {
                        rsa.PersistKeyInCsp = true;
                        if (!rsa.CspKeyContainerInfo.Exportable)
                            throw new CryptoException("Key not exportable");
                    }
                    break;
                case "DSA":
                    /*
                    cp = new CspParameters(13, "Microsoft Enhanced DSS and Diffie-Hellman Cryptographic Provider");
                    cp.KeyContainerName = _name;
                    cp.Flags = CspProviderFlags.UseArchivableKey;
                    DSACryptoServiceProvider dsa = new DSACryptoServiceProvider(pkSize, cp);
                    dsa.PersistKeyInCsp = true;
                    break;
                     * */

                //case "ECDSA":
                    //ECKeyPairGenerator ecGenerator = new ECKeyPairGenerator(pkAlgo);
                    //ecGenerator.Init(genParam);
                    //keyPair = ecGenerator.GenerateKeyPair();
                    //break;
                default:
                    throw new ArgumentException("Algorithm not supported", pkAlgo);
            }
            return cp;
        }

        /// <summary>
        /// Create a key pair
        /// </summary>
        /// <param name="pkSize">Key size</param>
        /// <param name="cspName">Name of the CSP.</param>
        /// <param name="cspNum">The CSP number.</param>
        /// <param name="name">Key container name</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Algorithm not supported</exception>
        internal static CspParameters Create(int pkSize, string cspName, int cspNum, string name)
        {
            // Normalise the name
            string _name = name.Replace(' ', '_');

            // Differentiate between RSA and DSA
            string pkAlgo = "Unknown";
            if (cspName.Contains("RSA"))
                pkAlgo = "RSA";

            if (cspName.Contains("DSA"))
                pkAlgo = "DSA";

            CspParameters cp = null;
            cp = new CspParameters(cspNum, cspName);
            cp.KeyContainerName = _name;
            cp.Flags = CspProviderFlags.UseArchivableKey;
            switch (pkAlgo)
            {
                case "RSA":
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(pkSize, cp))
                    {
                        rsa.PersistKeyInCsp = true;
                        //if (!rsa.CspKeyContainerInfo.Exportable)
                        //    throw new CryptoException("Key not exportable");
                    }
                    break;

                case "DSA":
                    DSACryptoServiceProvider dsa = new DSACryptoServiceProvider(pkSize, cp);
                    dsa.PersistKeyInCsp = true;
                    break;
                 

                //case "ECDSA":
                //ECKeyPairGenerator ecGenerator = new ECKeyPairGenerator(pkAlgo);
                //ecGenerator.Init(genParam);
                //keyPair = ecGenerator.GenerateKeyPair();
                //break;
                default:
                    throw new ArgumentException("Algorithm not supported", pkAlgo);
            }
            return cp;
        }

        #endregion


        #region Get Public Key

        internal static AsymmetricKeyParameter getPublicKey(CspParameters cp, string pkAlgo)
        {
            switch (pkAlgo)
            {
                case "RSA":
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp))
                    {
                        return DotNetUtilities.GetRsaPublicKey(rsa);
                    }
                case "DSA":
                    using (DSACryptoServiceProvider dsa = new DSACryptoServiceProvider(cp))
                    {
                        return DotNetUtilities.GetDsaPublicKey(dsa);
                    }
                default:
                    throw new ArgumentException("Algorithm not supported", pkAlgo);
            }
        }
        
        #endregion

        #region Get Provider

        internal static ICspAsymmetricAlgorithm getProvider(CspParameters cp, string pkAlgo)
        {
            switch (pkAlgo)
            {
                case "RSA":
                    return new RSACryptoServiceProvider(cp);
                case "DSA":
                    return new DSACryptoServiceProvider(cp);
                default:
                    throw new ArgumentException("Algorithm not supported", pkAlgo);
            }
        }

        #endregion

        #region Helpers

        internal static CspParameters Read(string name)
        {
            // Normalise the name
            string _name = name.Replace(' ', '_');

            CspParameters cp = new CspParameters();
            cp.KeyContainerName = _name;

            return cp;
        }

        #endregion

        #region Export as PKCS#12

        internal static void ExportToP12(CspParameters cspParam, X509Certificate cert, string outputFile, string password, string name)
        {
            // Normalise the name
            string nName = name.Replace(' ', '_');

            // Use the FIPS-140 system prng
            SecureRandom random = new SecureRandom(new CryptoApiRandomGenerator());

            // Build PKCS#12
            Pkcs12StoreBuilder p12 = new Pkcs12StoreBuilder();
            Pkcs12Store pkcs = p12.Build();

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParam))
            {
                if (!rsa.CspKeyContainerInfo.Exportable)
                    throw new CryptoException("Private key not exportable");

                pkcs.SetKeyEntry(nName, 
                    new AsymmetricKeyEntry(DotNetUtilities.GetRsaKeyPair(rsa).Private), 
                    new X509CertificateEntry[] { new X509CertificateEntry(cert) });
            }

            Stream stream = new FileStream(outputFile, FileMode.Create);
            pkcs.Save(stream, password.ToCharArray(), random);
            stream.Close();
        }

        /*
        internal static byte[] ExportToP12(CspParameters cspParam, X509Certificate cert, string password)
        {
            Sys.X509Certificate2 sysCert = new Sys.X509Certificate2(cert.GetEncoded()); ;

            Sys.X509Store store = new Sys.X509Store(Sys.StoreLocation.CurrentUser);
            store.Open(Sys.OpenFlags.ReadWrite);
            store.Add(sysCert);

            sysCert = store.Certificates[0];

            // Export the certificate including the private key.
            return sysCert.Export(Sys.X509ContentType.Pkcs12, password);
        }
        */
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

        /*
        internal static void ImportFromP12(byte[] P12, string pkAlgo, string name, string Password)
        {
            Sys.X509Certificate2 certToImport = new Sys.X509Certificate2(P12, Password, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.Exportable);

            if (!certToImport.HasPrivateKey)
                throw new ApplicationException("No private key in PKCS#12 file");

            CspParameters cp = null;
     
            // Normalise the name
            string _name = name.Replace(' ', '_');

            switch (pkAlgo)
            {
                case "RSA":
                    cp = new CspParameters(24, "Microsoft Enhanced RSA and AES Cryptographic Provider");
                    cp.KeyContainerName = _name;
                    cp.Flags = CspProviderFlags.UseArchivableKey;
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {

                        //certToImport.PrivateKey
                        //rsa.ImportParameters(certToImport.PrivateKey);
                        rsa.PersistKeyInCsp = true;
                        if (!rsa.CspKeyContainerInfo.Exportable)
                            throw new CryptoException("Key not exportable");
                    }
                    break;
                case "DSA":
                    cp = new CspParameters(13, "Microsoft Enhanced DSS and Diffie-Hellman Cryptographic Provider");
                    cp.KeyContainerName = _name;
                    cp.Flags = CspProviderFlags.UseArchivableKey;
                    break;
                default:
                    throw new ArgumentException("Algorithm not supported", pkAlgo);
            }
            

            
            //certToImport.PrivateKey

            Sys.X509Store store = new Sys.X509Store(Sys.StoreName.My,
                                             Sys.StoreLocation.CurrentUser);
            store.Open(Sys.OpenFlags.MaxAllowed);
            store.Add(certToImport);

            //certToImport.PrivateKey
            store.Close();
        }

         * */
        #endregion

        #region Load CSP

        internal static CspParameters LoadCsp(AsymmetricKeyParameter privateKey)
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

        #endregion

    }
}
