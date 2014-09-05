using System.Collections.Generic;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using OSCA.Crypto;
using OSCA.Crypto.X509;

namespace OSCA.Policy
{
    /// <summary>
    /// Validate that the proposed certificate contains the certificate policy
    /// </summary>
    internal class CertificatePoliciesPolicy : PolicyEnforcement
    {
        private List<string> policies = new List<string>();
        private bool anyPolicy = false;

        internal CertificatePoliciesPolicy(X509Certificate caCert, XElement policy)
        {
            report = "CertificatePoliciesPolicy: ";

            CertificatePolicies cp = CertificatePolicies.GetInstance(caCert.GetExtensions().GetExtension(X509Extensions.CertificatePolicies));

            for (int i = 0; i < 100; i++) // Bit arbitrary!
            {
                if (cp.GetPolicy(i) != null)
                {
                    policies.Add(cp.GetPolicy(i));
                    if (cp.GetPolicy(i) == CertificatePolicies.anyPolicy.ToString())
                        anyPolicy = true;
                }
                else
                    break;  // run out of policies
            }
        }

        internal override bool analyse(TbsCertificateStructure tbsCert)
        {
            bool status = true;

            CertificatePolicies cp = CertificatePolicies.GetInstance(tbsCert.Extensions.GetExtension(X509Extensions.CertificatePolicies));
            for (int i = 0; i < 100; i++) // Bit arbitrary!
            {
                if (cp.GetPolicy(i) != null)
                {
                    if ((!policies.Contains(cp.GetPolicy(i))) && (!anyPolicy))
                    {
                        report = report + "Invalid Policy: " + cp.GetPolicy(i) + "; ";
                        status = false;
                        continue;
                    }
                }
                else
                    break;  // run out of policies
            }
            return status;
        }
    }
}
