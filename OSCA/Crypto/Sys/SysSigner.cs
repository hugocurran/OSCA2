using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using Org.BouncyCastle.Security;

namespace OSCA.Crypto
{
    /// <summary>
    /// 
    /// </summary>
    internal class SysSigner
    {
        internal static string getAsymAlgorithm(string sigAlg)
        {
            string s1 = sigAlg.ToUpper();
            string s2 = s1.Replace("WITH", "?");
            string[] s3 = s2.Split('?');
            return s3[1];
        }

        internal static HashAlgorithm getHashAlgorithm(string sigAlg)
        {
            string s1 = sigAlg.ToUpper();
            string s2 = s1.Replace("WITH", "?");
            string[] s3 = s2.Split('?');

            switch (s3[0])
            {
                case "SHA-1":
                case "SHA1":
                    return new SHA1CryptoServiceProvider();
                case "SHA-256":
                case "SHA256":
                    return new SHA256CryptoServiceProvider();
                case "SHA-384":
                case "SHA384":
                    return new SHA384CryptoServiceProvider();
                case "SHA-512":
                case "SHA512":
                    return new SHA512CryptoServiceProvider();
                default:
                    throw new InvalidParameterException("Unrecognised hashing algorithm " + s3[0]);
            }
        }

        internal static byte[] Sign(byte[] buffer, CspParameters cspParam, string sigAlg)
        {
            string algo = getAsymAlgorithm(sigAlg);
            switch (algo)
            {
                case "RSA":
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParam))
                    {
                        return rsa.SignData(buffer, SysSigner.getHashAlgorithm(sigAlg));
                    }

                case "DSA":
                    using (DSACryptoServiceProvider dsa = new DSACryptoServiceProvider(cspParam))
                    {
                        return dsa.SignData(buffer);
                    }

                default:
                    throw new InvalidParameterException("Unknown asymmetric encryption algorithm " + algo);
            }
        }
    }
}
