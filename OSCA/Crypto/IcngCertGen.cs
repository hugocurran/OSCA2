using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.X509;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Crypto
{
    interface IcngCertGen
    {
        /// <summary>
        /// Generate an X509 Certificate
        /// </summary>
        /// <param name="key">Reference to signing key.</param>
        /// <returns>
        /// An X509 Certificate
        /// </returns>
        X509Certificate Generate(CngKey key);

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="key">Reference to signing key.</param>
        /// <param name="Extensions">Extensions to include in the certificate</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        X509Certificate Generate(CngKey key, X509Extensions Extensions);

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="key">Reference to signing key.</param>
        /// <param name="Profile">OSCA profile.</param>
        /// <param name="NotBefore">The not before.</param>
        /// <param name="NotAfter">The not after.</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        X509Certificate Generate(CngKey key, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter);

    }
}
