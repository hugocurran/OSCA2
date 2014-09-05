using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.X509;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Policy
{
    /// <summary>
    /// Validate that the proposed certificate does not violate PathLenghtConstraints
    /// </summary>
    class PathLengthPolicy : PolicyEnforcement
    {
        private int pathLen = int.MaxValue;

        internal PathLengthPolicy(X509Certificate caCert, XElement policy)
        {
            report = "PathLengthPolicy: ";
            pathLen = caCert.GetBasicConstraints();
        }

        internal override bool analyse(TbsCertificateStructure tbsCert)
        {
            bool status = true;

            BasicConstraints bc = BasicConstraints.GetInstance(tbsCert.Extensions.GetExtension(X509Extensions.BasicConstraints));

            // If CA PathLength == 0, then cannot issue CA certs
            if ((bc.IsCA()) && (pathLen == 0))
            {
                report = report + "CA PathLengthConstraint = 0 - cannot issue CA certificates;";
                status = false;
            }

            // If CA PathLength set cert PathLength must be < 
            if ((bc.IsCA()) && (pathLen < int.MaxValue) && (bc.PathLenConstraint.IntValue >= pathLen))
            {
                report = report + "CA PathLengthConstraint = " + pathLen.ToString() + " - invalid certificate Path Length = " + bc.PathLenConstraint.IntValue.ToString() + ";";
                status = false;
            }

            // If CA PathLength set then cert must set a PathLength
            if ((bc.IsCA()) && (pathLen < int.MaxValue) && (bc.PathLenConstraint == null))
            {
                report = report + "CA PathLengthConstraint = " + pathLen.ToString() + " - certificate must set PathLengthConstraint;";
                status = false;
            }
            return status;
        }
    }
}



