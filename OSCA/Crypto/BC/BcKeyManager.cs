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
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;
using OSCA.Offline;


namespace OSCA.Crypto
{
    /// <summary>
    /// Class that manages keypairs using the BC crypto library
    /// </summary>
    internal class BcKeyManager
    {
        internal static AsymmetricCipherKeyPair Create(int pkSize, string pkAlgo)
        {
            SecureRandom random = new SecureRandom(new CryptoApiRandomGenerator());
            KeyGenerationParameters genParam = new KeyGenerationParameters(random, pkSize);
            AsymmetricCipherKeyPair keyPair;

            switch (pkAlgo)
            {
                case "RSA":
                    RsaKeyPairGenerator rsaGenerator = new RsaKeyPairGenerator();
                    rsaGenerator.Init(genParam);
                    keyPair = rsaGenerator.GenerateKeyPair();
                    break;
                case "DSA":
                    DsaKeyPairGenerator dsaGenerator = new DsaKeyPairGenerator();
                    dsaGenerator.Init(genParam);
                    keyPair = dsaGenerator.GenerateKeyPair();
                    break;
                case "ECDSA":
                    ECKeyPairGenerator ecGenerator = new ECKeyPairGenerator(pkAlgo);
                    ecGenerator.Init(genParam);
                    keyPair = ecGenerator.GenerateKeyPair();
                    break;
                default:
                    throw new ArgumentException("Algorithm not supported", pkAlgo);
            }
            return keyPair;
        }

        internal static string SaveP12(AsymmetricKeyParameter privateKey, X509Certificate cert, string keyFile, string password, string alias)
        {
            // Use the FIPS-140 system prng
            SecureRandom random = new SecureRandom(new CryptoApiRandomGenerator());

            // Build PKCS#12
            Pkcs12StoreBuilder p12 = new Pkcs12StoreBuilder();
            Pkcs12Store pkcs = p12.Build();

            pkcs.SetKeyEntry(alias, new AsymmetricKeyEntry(privateKey), new X509CertificateEntry[] { new X509CertificateEntry(cert) });

            Stream stream = new FileStream(keyFile, FileMode.Create);
            pkcs.Save(stream, password.ToCharArray(), random);
            stream.Close();
            return alias;
        }

        /// <summary>
        /// Save a private key and certificate to a PKCS#12 object
        /// </summary>
        /// <param name="privateKey">The private key</param>
        /// <param name="cert">The certificate</param>
        /// <param name="password">Password to secure PKCS#12</param>
        /// <param name="alias">Friendly name for the PKCS#12</param>
        /// <returns></returns>
        internal static MemoryStream SaveP12(AsymmetricKeyParameter privateKey, X509Certificate cert, string password, string alias)
        {
            // Use the FIPS-140 system prng
            SecureRandom random = new SecureRandom(new CryptoApiRandomGenerator());

            // Build PKCS#12
            Pkcs12StoreBuilder p12 = new Pkcs12StoreBuilder();
            Pkcs12Store pkcs = p12.Build();

            pkcs.SetKeyEntry(alias, new AsymmetricKeyEntry(privateKey), new X509CertificateEntry[] { new X509CertificateEntry(cert) });

            MemoryStream stream = new MemoryStream();
            pkcs.Save(stream, password.ToCharArray(), random);
            stream.Position = 0;

            return stream;
        }

        /// <summary>
        /// Save a private key in an encrypted PKCS#8 object
        /// </summary>
        /// <param name="privateKey">Private key value</param>
        /// <param name="password">Password for encrypt</param>
        /// <returns>PKCS#8 object</returns>
        internal static string SaveP8(AsymmetricKeyParameter privateKey, string password)
        {
            // Use the FIPS-140 system prng
            SecureRandom random = new SecureRandom(new CryptoApiRandomGenerator());

            // Build PKCS#8
            Pkcs8Generator p8 = new Pkcs8Generator(privateKey, "PBEWITHSHA256AND128BITAES-CBC-BC");
            p8.Password = password.ToCharArray();
            p8.SecureRandom = random;
            p8.IterationCount = 2048;
            PemObject pem = p8.Generate();

            return pem.Content.ToString();
        }
    }
}

