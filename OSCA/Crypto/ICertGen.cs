using System;
using System.Collections;
using System.Security.Cryptography;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;

namespace OSCA.Crypto
{
    /// <summary>
    /// Interface to X509 Certificate Generators.
    /// </summary>
    public interface ICertGen
    {
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
        /// Set the subject unique ID - note: it is very rare that it is correct to do this.
        /// </summary>
        /// <param name="uniqueID"/>
        void SetSubjectUniqueID(bool[] uniqueID);

        /// <summary>
        /// Set the issuer unique ID - note: it is very rare that it is correct to do this.
        /// </summary>
        /// <param name="uniqueID"/>
        void SetIssuerUniqueID(bool[] uniqueID);

        /// <summary>
        /// Add a given extension field for the standard extensions tag (tag 3).
        /// </summary>
        /// <param name="oid">string containing a dotted decimal Object Identifier.</param>
        /// <param name="critical">Is it critical.</param>
        /// <param name="extensionValue">The value.</param>
        void AddExtension(string oid, bool critical, Asn1Encodable extensionValue);

        /// <summary>
        /// Add an extension to this certificate.
        /// </summary>
        /// <param name="oid">Its Object Identifier.</param>
        /// <param name="critical">Is it critical.</param>
        /// <param name="extensionValue">The value.</param>
        void AddExtension(DerObjectIdentifier oid, bool critical, Asn1Encodable extensionValue);

        /// <summary>
        /// Add an extension using a string with a dotted decimal OID.
        /// </summary>
        /// <param name="oid">string containing a dotted decimal Object Identifier.</param>
        /// <param name="critical">Is it critical.</param>
        /// <param name="extensionValue">byte[] containing the value of this extension.</param>
        void AddExtension(string oid, bool critical, byte[] extensionValue);

        /// <summary>
        /// Add an extension to this certificate.
        /// </summary>
        /// <param name="oid">Its Object Identifier.</param>
        /// <param name="critical">Is it critical.</param>
        /// <param name="extensionValue">byte[] containing the value of this extension.</param>
        void AddExtension(DerObjectIdentifier oid, bool critical, byte[] extensionValue);

        /// <summary>
        /// Add a given extension field for the standard extensions tag (tag 3),
        /// copying the extension value from another certificate.
        /// </summary>
        void CopyAndAddExtension(string oid, bool critical, X509Certificate cert);

        /**
         * add a given extension field for the standard extensions tag (tag 3)
         * copying the extension value from another certificate.
         * @throws CertificateParsingException if the extension cannot be extracted.
         */
        void CopyAndAddExtension(DerObjectIdentifier oid, bool critical, X509Certificate cert);

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        X509Certificate Generate(AsymmetricKeyParameter privateKey);
        //X509Certificate Generate<K>(K privateKey);

        /// <summary>
        /// Generate an X509 Certificate
        /// </summary>
        /// <param name="cspParam">CspParameters instance that has the private signing key</param>
        /// <returns>
        /// An X509 Certificate
        /// </returns>
        X509Certificate Generate(CspParameters cspParam);

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <param name="Extensions">The extensions.</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        X509Certificate Generate(AsymmetricKeyParameter privateKey, X509Extensions Extensions);
        //X509Certificate Generate<K, E>(K privateKey, E Extensions);

        /// <summary>
        /// Generate an X509 Certificate
        /// </summary>
        /// <param name="cspParam">CspParameters instance that has the private signing key</param>
        /// <param name="Extensions">The extensions.</param>
        /// <returns>
        /// An X509 Certificate
        /// </returns>
        X509Certificate Generate(CspParameters cspParam, X509Extensions Extensions);

        /// <summary>
        /// Generate an X509 Certificate
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="Profile">The profile.</param>
        /// <param name="ValidityStart">The validity start.</param>
        /// <returns>X509 Certificate</returns>
        X509Certificate Generate(AsymmetricKeyParameter privateKey, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter);
        //X509Certificate Generate<K, P, B, A>(K privateKey, P Profile, B NotBefore, A NotAfter);

        /// <summary>
        /// Generate an X509 Certificate
        /// </summary>
        /// <param name="cspParam">The CSP parameter.</param>
        /// <param name="Profile">The profile.</param>
        /// <param name="ValidityStart">The validity start.</param>
        /// <returns>X509 Certificate</returns>
        X509Certificate Generate(CspParameters cspParam, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter);

        /// <summary>
        /// Allows enumeration of the signature names supported by the generator.
        /// </summary>
        IEnumerable SignatureAlgNames  { get; }
    }
}
