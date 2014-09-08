using System;
using System.Collections;
using System.Security.Cryptography;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.X509.Extension;
using Org.BouncyCastle.X509;

namespace OSCA.Crypto
{
    /// <summary>
    /// A class to Generate Version 3 X509Certificates.
    /// Uses system crypto libraries
    /// </summary>
    public class SysV1CertGen : V1CertGen
    {
        /// <summary>
        /// Constructor for OSCA V1 Certificate Generator (system crypto libraries)
        /// </summary>
        public SysV1CertGen() : base() { }

        /// <summary>
        /// Generate an X509 Certificate
        /// </summary>
        /// <param name="cspParam">CspParameters instance that has the private signing key</param>
        /// <returns>An X509 Certificate</returns>
        public X509Certificate Generate(CspParameters cspParam)
        {
            TbsCertificateStructure tbsCert = tbsGen.GenerateTbsCertificate();
            byte[] cert = tbsCert.GetEncoded();
            byte[] signature;

            try
            {
                signature = SysSigner.Sign(cert, cspParam, signatureAlgorithm);
            }
            catch (Exception e)
            {
                throw new CertificateEncodingException("Exception encoding TBS cert", e);
            }

            try
            {
                return new X509Certificate(new X509CertificateStructure(tbsCert, sigAlgId, new DerBitString(signature)));
            }
            catch (CertificateParsingException e)
            {
                throw new CertificateEncodingException("Exception producing certificate object", e);
            }
        }

        public X509Certificate Generate(CspParameters cspParam, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            SetNotBefore(NotBefore);
            SetNotAfter(NotAfter);
            // Ignore profile - no extensions in V1

            return Generate(cspParam);
        }

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="cspParam">CspParameters instance that has the private signing key</param>
        /// <param name="Extensions">Extensions to include in the certificate (ignored)</param>
        /// <returns>An X509Certificate.</returns>
        public X509Certificate Generate(CspParameters cspParam, X509Extensions Extensions)
        {
            // Ignore extensions
            return Generate(cspParam);
        }

        /*
        #region Stubs for BC cryptography

        /// <summary>
        /// Generate an X509Certificate.
        /// Always throws an exception
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <returns>An X509Certificate.</returns>
        public override X509Certificate Generate(AsymmetricKeyParameter privateKey)
        {
            throw new InvalidOperationException("BC Cryptography not valid in system context");
        }

        /// <summary>
        /// Generate an X509Certificate.
        /// Always throws an exception
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <param name="Extensions"></param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">BC Cryptography not valid in system context</exception>
        public override X509Certificate Generate(AsymmetricKeyParameter privateKey, X509Extensions Extensions)
        {
            throw new InvalidOperationException("BC Cryptography not valid in system context");
        }

        #endregion
         * 
         */

        /// <summary>
        /// Allows enumeration of the signature names supported by the generator.
        /// </summary>
        /*
         * TODO: Make this return the system list
         */

        public IEnumerable SignatureAlgNames
        {
            get { return X509Utilities.GetAlgNames(); }
        }
    }
}

