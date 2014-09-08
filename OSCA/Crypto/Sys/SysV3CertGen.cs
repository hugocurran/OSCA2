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
using OSCA.Policy;
using System.Collections.Generic;
using OSCA.Profile;
using OSCA.Offline;


namespace OSCA.Crypto
{
    /// <summary>
    /// A class to Generate Version 3 X509Certificates.
    /// Uses system crypto libraries
    /// </summary>
    public class SysV3CertGen : V3CertGen, IsysCertGen
    {
        /// <summary>
        /// Constructor for V3 Certificate Generator (system crypto libraries)
        /// </summary>
        public SysV3CertGen() : base() {}


        internal SysV3CertGen(List<PolicyEnforcement> Policy) : base(Policy) {}

        /// <summary>
        /// Generate an X509 Certificate
        /// </summary>
        /// <param name="cspParam">CspParameters instance that has the private signing key</param>
        /// <returns>An X509 Certificate</returns>
        public X509Certificate Generate(CspParameters cspParam)
        {
            return Generate(cspParam, null);
        }

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="cspParam">CspParameters instance that has the private signing key</param>
        /// <param name="Extensions">Extensions to include in the certificate</param>
        /// <returns>An X509Certificate.</returns>
        public X509Certificate Generate(CspParameters cspParam, X509Extensions Extensions)
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
        public X509Certificate Generate(CspParameters cspParam, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            // Set the validity
            tbsGen.SetStartDate(new Time(NotBefore));
            tbsGen.SetEndDate(new Time(NotAfter));

            // Extract extensions from the profile
            foreach (ProfileExtension ext in Profile.Extensions)
                extGenerator.AddExtension(ext.OID, ext.Critical, ext.DerEncoding);

            // Call the generator
            return Generate(cspParam, extGenerator.Generate());
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
