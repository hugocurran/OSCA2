
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
using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Sys = System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Pkcs;
using CERTENROLLLib;

namespace OSCA.Crypto
{

    /// <summary>
    /// Class that manages keypairs using the system crypto library
    /// </summary>
    internal class SysInterface
    {
        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        internal static Pkcs10CertificationRequest createKeyPair()
        {
            var objCSPs = new CCspInformations();
            objCSPs.AddAvailableCsps();


            var objPrivateKey = new CX509PrivateKey();
            objPrivateKey.Length = 1024;
            objPrivateKey.KeySpec = X509KeySpec.XCN_AT_SIGNATURE;
            objPrivateKey.KeyUsage = X509PrivateKeyUsageFlags.XCN_NCRYPT_ALLOW_ALL_USAGES;
            objPrivateKey.MachineContext = false;
            objPrivateKey.ExportPolicy = X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_EXPORT_FLAG;
            objPrivateKey.CspInformations = objCSPs;
            objPrivateKey.Create();

            var objPkcs10 = new CX509CertificateRequestPkcs10();
            objPkcs10.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextUser,
                objPrivateKey,
                string.Empty);

            //var objExtensionKeyUsage = new CX509ExtensionKeyUsage();
            //objExtensionKeyUsage.InitializeEncode(
            //    CERTENROLLLib.X509KeyUsageFlags.XCN_CERT_KEY_CERT_SIGN_KEY_USAGE |
            //    CERTENROLLLib.X509KeyUsageFlags.XCN_CERT_CRL_SIGN_KEY_USAGE);

            //objPkcs10.X509Extensions.Add((CX509Extension)objExtensionKeyUsage);
 
            //var objObjectId = new CObjectId();
            //var objObjectIds = new CObjectIds();
            //var objX509ExtensionEnhancedKeyUsage = new CX509ExtensionEnhancedKeyUsage();
            //objObjectId.InitializeFromValue("1.3.6.1.5.5.7.3.2");
            //objObjectIds.Add(objObjectId);
            //objX509ExtensionEnhancedKeyUsage.InitializeEncode(objObjectIds);
            //objPkcs10.X509Extensions.Add((CX509Extension)objX509ExtensionEnhancedKeyUsage);

            var objDN = new CX500DistinguishedName();
            var subjectName = "CN = shaunxu.me, OU = ADCS, O = Blog, L = Beijng, S = Beijing, C = CN";
            objDN.Encode(subjectName, X500NameFlags.XCN_CERT_NAME_STR_NONE);
            objPkcs10.Subject = objDN;

            var objEnroll = new CX509Enrollment();
            objEnroll.InitializeFromRequest(objPkcs10);
            var strRequest = objEnroll.CreateRequest(EncodingType.XCN_CRYPT_STRING_BASE64);

            Pkcs10CertificationRequest p10 = new Pkcs10CertificationRequest(Convert.FromBase64String(strRequest));

            return p10;
        }


        #region Create keys

        /// <summary>
        /// Create a key pair
        /// </summary>
        /// <param name="pkSize">Key size</param>
        /// <param name="pkAlgo">Key algorithm</param>
        /// <param name="name">Key container name</param>
        /// <returns></returns>
        internal static CspParameters createKeyPair(int pkSize, string pkAlgo, string name)
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
                        rsa.PersistKeyInCsp = false;
                        if (!rsa.CspKeyContainerInfo.Exportable)
                            throw new CryptoException("Key not exportable");
                    }
                    break;
                case "DSA":
                    cp = new CspParameters(13, "Microsoft Enhanced DSS and Diffie-Hellman Cryptographic Provider");
                    cp.KeyContainerName = _name;
                    cp.Flags = CspProviderFlags.UseArchivableKey;
                    DSACryptoServiceProvider dsa = new DSACryptoServiceProvider(pkSize, cp);
                    dsa.PersistKeyInCsp = false;
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

        /*
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
                    cp = new CspParameters(13, "Microsoft Enhanced DSS and Diffie-Hellman Cryptographic Provider");
                    cp.KeyContainerName = _name;
                    cp.Flags = CspProviderFlags.UseArchivableKey;
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
         */
        #endregion


        //internal static X509Certificate storeKey(CspParameters cp, X509Certificate cert)
        internal static X509Certificate storeKey(X509Certificate cert)
        {
            //SysX509.X509KeyStorageFlags keyFlags = (SysX509.X509KeyStorageFlags.UserKeySet | SysX509.X509KeyStorageFlags.Exportable);
            //SysX509.X509KeyStorageFlags keyFlags = SysX509.X509KeyStorageFlags.Exportable;

            Sys.X509Certificate2 sCert = new Sys.X509Certificate2(cert.GetEncoded());

            Sys.X509Store store = new Sys.X509Store(Sys.StoreName.My,
                                 Sys.StoreLocation.CurrentUser);
            store.Open(Sys.OpenFlags.MaxAllowed);
            store.Add(sCert);

            Sys.X509Certificate2Collection coll = store.Certificates.Find(Sys.X509FindType.FindBySerialNumber, sCert.SerialNumber, false);

            if (coll.Count > 1)
                throw new CryptoException("Too many certs");

            if (coll.Count < 1)
                throw new CryptoException("Cert not found");

            sCert = coll[0];
            if (!sCert.HasPrivateKey)
                throw new CryptoException("No private key");



            return cert;
        }

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

        #endregion

        #region Import from PKCS#12

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

        #endregion

    }
}

