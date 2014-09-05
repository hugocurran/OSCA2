using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.X509;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;

namespace OSCA.Policy
{
    internal static class  PolicyEnforcementFactory
    {
        private static List<PolicyEnforcement> policies = new List<PolicyEnforcement>();

        /// <summary>
        /// Build a list of PolicyEnforcement modules.
        /// <para>Some policies are set by the issuing CA others are set by the CA manager</para>
        /// </summary>
        /// <param name="caCert">CA certificate</param>
        /// <param name="policyEnforcement">The policy enforcement.</param>
        /// <returns>
        /// List of PolicyEnforcement modules
        /// </returns>
        internal static List<PolicyEnforcement> initialise(X509Certificate caCert, XElement policyEnforcement)
        {
            X509Extensions extensions = caCert.GetExtensions();
            DerObjectIdentifier[] extOids = null;
            if (extensions != null)
                extOids = extensions.GetExtensionOids();

            // We always have the following modules...
            // Validity
            policies.Add(new ValidityPolicy(caCert, policyEnforcement));

            // PathLengthConstraint
            policies.Add(new PathLengthPolicy(caCert, policyEnforcement));
            
            // Other Issuer policies
            if (extOids != null)
            {
                foreach (DerObjectIdentifier ext in extOids)
                {
                    // CertificatePolicy
                    if (ext.Equals(X509Extensions.CertificatePolicies))
                        policies.Add(new CertificatePoliciesPolicy(caCert, policyEnforcement));

                    // PolicyConstraints

                    // InhibitAnyPolicy

                    // NameConstraints
                }
            }

            // Some policies do not apply to self-signed certs
            if (!caCert.SubjectDN.Equals(caCert.IssuerDN))
            {

            }

            // Local policies set by the CA Manager
            // TODO - eg cert types support (v1/v3)


            return policies;
        }


    }
}
