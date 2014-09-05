using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Policy
{
    internal class TestAgainstPolicy
    {
        private string[] _status = new string[20];
        private List<PolicyEnforcement> policyModules;

        internal string[] status { get { return _status; } }

        internal TestAgainstPolicy(List<PolicyEnforcement> policyModules)
        {
            this.policyModules = policyModules;
        }

        internal bool report(TbsCertificateStructure tbsCert)
        {
            bool valid = true;
            int index = 0;
            foreach (PolicyEnforcement module in policyModules)
            {
                if (!module.analyse(tbsCert))
                {
                    valid = false;
                    _status[index] = module.result;
                    index++;
                }
            }

            return valid;
        }

        public override string ToString()
        {
            StringBuilder message = new StringBuilder();

            foreach (string status in _status)
            {
                message.AppendLine(status);
            }
            return message.ToString();
        }

    }
}
