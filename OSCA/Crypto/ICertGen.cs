using System;
using System.Collections;
using System.Security.Cryptography;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using OSCA.Offline;

namespace OSCA.Crypto
{
    /// <summary>
    /// Interface to X509 Certificate Generators.
    /// </summary>
    public interface ICertGen
    {
        /// <summary>
        /// Gets the X509 version returned by the certificate generator
        /// </summary>
        /// <returns></returns>
        X509ver GetVersion();

        /// <summary>
        /// Reset the Generator.
        /// </summary>
        void Reset();

        /// <summary>
        /// Set the certificate's serial number.
        /// </summary>
        /// <remarks>Make serial numbers long, if you have no serial number policy make sure the number is at least 16 bytes of secure random data.
        /// You will be surprised how ugly a serial number collision can Get.</remarks>
        /// <param name="serialNumber">The serial number.</param>
        void SetSerialNumber(BigInteger serialNumber);

        /// <summary>
        /// Set the distinguished name of the issuer.
        /// The issuer is the entity which is signing the certificate.
        /// </summary>
        /// <param name="issuer">The issuer's DN.</param>
        void SetIssuerDN(X509Name issuer);

        /// <summary>
        /// Set the date that this certificate is to be valid from.
        /// </summary>
        /// <param name="date"/>
        void SetNotBefore(DateTime date);

        /// <summary>
        /// Set the date after which this certificate will no longer be valid.
        /// </summary>
        /// <param name="date"/>
        void SetNotAfter(DateTime date);

        /// <summary>
        /// Set the DN of the entity that this certificate is about.
        /// </summary>
        /// <param name="subject"/>
        void SetSubjectDN(X509Name subject);

        /// <summary>
        /// Set the public key that this certificate identifies.
        /// </summary>
        /// <param name="publicKey"/>
        void SetPublicKey(AsymmetricKeyParameter publicKey);

        /// <summary>
        /// Set the signature algorithm that will be used to sign this certificate.
        /// </summary>
        /// <param name="signatureAlgorithm"/>
        void SetSignatureAlgorithm(string signatureAlgorithm);

        /// <summary>
        /// Allows enumeration of the signature names supported by the generator.
        /// </summary>
        //IEnumerable SignatureAlgNames  { get; }
    }
}
