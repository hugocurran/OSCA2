using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.X509;

namespace OSCA.Crypto
{
    /**
    * class to produce an X.509 Version 2 CRL using System crypto
    */
    public class SysCrlGen : Org.BouncyCastle.X509.X509V2CrlGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SysCrlGen"/> class.
        /// </summary>
        public SysCrlGen() : base() { }

        /// <summary>Generate an X509 CRL, based on the current issuer and subject.</summary>
        /// <param name="cspParam">CSP Parameters containing the key</param>
        public X509Crl Generate(CspParameters cspParam)
        {
            TbsCertificateList tbsCrl = GenerateCertList();
            byte[] signature;

            try
            {
                signature = SysSigner.Sign(tbsCrl.GetDerEncoded(), cspParam, signatureAlgorithm); 
            }
            catch (IOException e)
            {
                throw new CrlException("cannot generate CRL encoding", e);
            }

            return new X509Crl(CertificateList.GetInstance(new DerSequence(tbsCrl, sigAlgId, new DerBitString(signature))));
        }
    }
}

