using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.X509;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Crypto
{
    /// <summary>
    /// Interface to System (CAPI) certificate generators
    /// </summary>
    /// <remarks>All sys cert generators should implement this interface</remarks>
    interface IsysCertGen
    {
        /// <summary>
        /// Generate an X509 Certificate
        /// </summary>
        /// <param name="cspParam">Reference to the private signing key</param>
        /// <returns>An X509 Certificate</returns>
        X509Certificate Generate(CspParameters cspParam);


        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="cspParam">Reference to the private signing key</param>
        /// <param name="Extensions">Extensions to include in the certificate</param>
        /// <returns>An X509Certificate.</returns>
        X509Certificate Generate(CspParameters cspParam, X509Extensions Extensions);

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="cspParam">Reference to the private signing key</param>
        /// <param name="Profile">OSCA profile</param>
        /// <param name="NotBefore">The notBefore date</param>
        /// <param name="NotAfter">The notAfter date</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        X509Certificate Generate(CspParameters cspParam, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter);
    }
}
