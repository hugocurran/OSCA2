using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Policy
{
    /// <summary>
    /// Template for policy enforcement modules
    /// </summary>
    internal abstract class PolicyEnforcement
    {
        protected string report;

        /// <summary>
        /// The result of the policy compliance analysis
        /// </summary>
        internal string result { get { return report; } }

        /// <summary>
        /// Analyse the proposed certificate against the policy enforced by the module
        /// </summary>
        /// <param name="tbsCert">Proposed certificate</param>
        /// <returns>True if compliant, false otherwise</returns>
        internal abstract bool analyse(TbsCertificateStructure tbsCert);
    }
}
