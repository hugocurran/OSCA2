using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using OSCA.Policy;
using OSCA.Profile;
using OSCA.Offline;

namespace OSCA.Crypto
{
    /// <summary>
    /// A class to Generate Version 3 X509Certificates.
    /// <remarks>
    /// This is modified from the BC original (X509CertificateGenerator) to:
    /// <para>
    ///  + Accept pre-defined X509Extensions (from a request)
    ///  </para>
    ///  <para>
    ///  + Accept an OSCA profile describing extensions
    ///  </para>
    ///  </remarks>
    /// </summary>
    public class BcV3CertGen : V3CertGen, IbcCertGen
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BcV3CertGen"/> class.
        /// </summary>
        public BcV3CertGen() : base() {}


        internal BcV3CertGen(List<PolicyEnforcement> Policy) : base(Policy) {}

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <returns>An X509Certificate.</returns>
        public virtual X509Certificate Generate(AsymmetricKeyParameter privateKey)
        {
            return Generate(privateKey, null);
        }

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <param name="Extensions">Set of extensions to include in the certificate.</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        /// <exception cref="PolicyEnforcementException">CA policy violation</exception>
        /// <exception cref="CertificateEncodingException">
        /// Exception encoding TBS cert
        /// or
        /// Exception producing certificate object
        /// </exception>
        public virtual X509Certificate Generate(AsymmetricKeyParameter privateKey, X509Extensions Extensions)
        {
            TbsCertificateStructure tbsCert = GenerateTbsCert(Extensions);

            // Check this complies with policy
            if (policy != null)
            {
                TestAgainstPolicy test = new TestAgainstPolicy(policy);
                if (!test.report(tbsCert))
                {
                    throw new PolicyEnforcementException(test.status.ToString());
                }
            }
            
            byte[] signature;

            try
            {
                signature = X509Utilities.GetSignatureForObject(
                    sigOid, signatureAlgorithm, privateKey, null, tbsCert);
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

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <param name="Profile">OSCA profile.</param>
        /// <param name="Start">Start date</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        /// <exception cref="PolicyEnforcementException">CA policy violation</exception>
        /// <exception cref="CertificateEncodingException">Exception encoding TBS cert
        /// or
        /// Exception producing certificate object</exception>
        public virtual X509Certificate Generate(AsymmetricKeyParameter privateKey, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            // Set the validity according to the profile
            tbsGen.SetStartDate(new Time(NotBefore));
            tbsGen.SetEndDate(new Time(NotAfter));

            // Extract extensions from the profile
            foreach (ProfileExtension ext in Profile.Extensions)
                extGenerator.AddExtension(ext.OID, ext.Critical, ext.DerEncoding);

            // Call the generator
            return Generate(privateKey, extGenerator.Generate());
        }


        internal TbsCertificateStructure GenerateTbsCert(X509Extensions Extensions)
        {
            if (!extGenerator.IsEmpty)
            {
                tbsGen.SetExtensions(extGenerator.Generate());
            }

            if (Extensions != null)
            {
                tbsGen.SetExtensions(Extensions);
            }

            return tbsGen.GenerateTbsCertificate();
        }

        /// <summary>
        /// Allows enumeration of the signature names supported by the generator.
        /// </summary>
        public virtual IEnumerable SignatureAlgNames
        {
            get { return X509Utilities.GetAlgNames(); }
        }
    }
}
