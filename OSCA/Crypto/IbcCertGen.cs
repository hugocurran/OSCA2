using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Asn1.X509;

/// <summary>
/// Interface for BC certificate generators
/// </summary>
/// <remarks>All BC cert generators should implement this interface</remarks>
namespace OSCA.Crypto
{
    interface IbcCertGen
    {
        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <returns>An X509Certificate.</returns>
        X509Certificate Generate(AsymmetricKeyParameter privateKey);

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <param name="Extensions">Set of extensions to include in the certificate.</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        X509Certificate Generate(AsymmetricKeyParameter privateKey, X509Extensions Extensions);

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <param name="Profile">OSCA profile.</param>
        /// <param name="NotBefore">The notBefore date</param>
        /// <param name="NotAfter">The notAfter date</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        X509Certificate Generate(AsymmetricKeyParameter privateKey, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter);

    }
}
