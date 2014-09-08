using System;
using System.Collections;
using System.Security.Cryptography;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.X509;

namespace OSCA.Crypto
{
    /// <summary>
    /// Class to Generate X509V1 Certificates using BC crypto
    /// </summary>
    public class BcV1CertGen : V1CertGen
    {
        

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public BcV1CertGen() : base() {}

        /// <summary>
        /// Generate a new X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer used to sign this certificate.</param>
        /// <returns>An X509Certificate.</returns>
        public X509Certificate Generate(AsymmetricKeyParameter privateKey)
        {
            return generate(null, (AsymmetricKeyParameter)privateKey);
        }

         /// <summary>
        /// Extensions are not supported in X.509 V1 certificates.
        /// </summary>
        public X509Certificate Generate(AsymmetricKeyParameter privateKey, DateTime NotBefore, DateTime NotAfter)
        {
            SetNotBefore(NotBefore);
            SetNotAfter(NotAfter);
            return generate(null, privateKey);
        }

        /// <summary>
        /// Generate a new X509Certificate specifying a SecureRandom instance that you would like to use.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer used to sign this certificate.</param>
        /// <param name="random">The Secure Random you want to use.</param>
        /// <returns>An X509Certificate.</returns>
        private X509Certificate generate(SecureRandom random, AsymmetricKeyParameter privateKey )
        {
            TbsCertificateStructure tbsCert = tbsGen.GenerateTbsCertificate();
            byte[] signature;

            try
            {
                signature = X509Utilities.GetSignatureForObject(
                    sigOID, signatureAlgorithm, (AsymmetricKeyParameter) privateKey, random, tbsCert);
            }
            catch (Exception e)
            {

                throw new CertificateEncodingException("Exception encoding TBS cert", e);
            }

            try
            {
                return GenerateJcaObject(tbsCert, signature);
            }
            catch (CertificateParsingException e)
            {
                throw new CertificateEncodingException("Exception producing certificate object", e);
            }
        }

        /// <summary>
        /// Generates the jca object.
        /// </summary>
        /// <param name="tbsCert">The TBS cert.</param>
        /// <param name="signature">The signature.</param>
        /// <returns>The newly minted certificate</returns>
        private X509Certificate GenerateJcaObject(
            TbsCertificateStructure tbsCert,
            byte[] signature)
        {
            return new X509Certificate(
                new X509CertificateStructure(tbsCert, sigAlgId, new DerBitString(signature)));
        }

        /// <summary>
        /// Allows enumeration of the signature names supported by the generator.
        /// </summary>
        public IEnumerable SignatureAlgNames
        {
            get { return X509Utilities.GetAlgNames(); }
        }

    }
}
