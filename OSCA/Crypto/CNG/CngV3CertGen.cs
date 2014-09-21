using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSCA.Policy;
using Org.BouncyCastle.X509;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Asn1;
using OSCA.Profile;

namespace OSCA.Crypto.CNG
{
    class CngV3CertGen : V3CertGen, IcngCertGen
    {
        /// <summary>
        /// Constructor for V3 Certificate Generator (system crypto libraries)
        /// </summary>
        public CngV3CertGen() : base() {}


        internal CngV3CertGen(List<PolicyEnforcement> Policy) : base(Policy) {}

        /// <summary>
        /// Generate an X509 Certificate
        /// </summary>
        /// <param name="cspParam">CspParameters instance that has the private signing key</param>
        /// <returns>An X509 Certificate</returns>
        public X509Certificate Generate(CngKey key)
        {
            return Generate(key, null);
        }

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="cspParam">CspParameters instance that has the private signing key</param>
        /// <param name="Extensions">Extensions to include in the certificate</param>
        /// <returns>An X509Certificate.</returns>
        public X509Certificate Generate(CngKey key, X509Extensions Extensions)
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
          
            byte[] cert = tbsCert.GetEncoded();
            byte[] signature;

            try
            {
                //AlgorithmIdentifier sigAlg = tbsCert.Signature;
                //sigAlg.ObjectID
                signature = CngSigner.Sign(cert, key, CngAlgorithm.Sha256);
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
        /// <param name="cspParam">The CSP parameter.</param>
        /// <param name="Profile">OSCA profile.</param>
        /// <param name="NotBefore">The not before.</param>
        /// <param name="NotAfter">The not after.</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        /// <exception cref="PolicyEnforcementException">CA policy violation</exception>
        /// <exception cref="CertificateEncodingException">Exception encoding TBS cert
        /// or
        /// Exception producing certificate object</exception>
        public X509Certificate Generate(CngKey key, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            // Set the validity
            tbsGen.SetStartDate(new Time(NotBefore));
            tbsGen.SetEndDate(new Time(NotAfter));

            // Extract extensions from the profile
            foreach (ProfileExtension ext in Profile.Extensions)
                extGenerator.AddExtension(ext.OID, ext.Critical, ext.DerEncoding);

            // Call the generator
            return Generate(key, extGenerator.Generate());
        }


        /// <summary>
        /// Allows enumeration of the signature names supported by the generator.
        /// </summary>
        /*
         * TODO: Make this return the system list
         */
        /*
        public override IEnumerable SignatureAlgNames
        {
            get { return X509Utilities.GetAlgNames(); }
        }
        */
    }
}
