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
    /// Validate that the proposed certificate meets the validity policy
    /// </summary>
    internal class ValidityPolicy : PolicyEnforcement
    {
        private DateTime caStartDate;
        private DateTime caEndDate;
        private DateTime rollOverDate = DateTime.MinValue;

        internal ValidityPolicy(X509Certificate caCert, XElement policy)
        {
            report = "ValidityPolicy: ";
            caStartDate = caCert.NotBefore;
            caEndDate = caCert.NotAfter;

            // When is the CA Key Rollover Date?
            if (policy.Element("keyRollOver") != null)
            {
                double ro = (Convert.ToDouble(policy.Element("keyRollOver").Value)/100);
                TimeSpan t = caCert.NotAfter - caCert.NotBefore;
                double rollOverDays = t.TotalDays * ro;
                rollOverDate = caCert.NotBefore.AddDays(rollOverDays);
            }
        }

        internal override bool analyse(TbsCertificateStructure tbsCert)
        {
            bool status = true;

            if (tbsCert.StartDate.ToDateTime() < caStartDate)
            {
                status = false;
                report = report + "Start date before CA start date: " + tbsCert.StartDate.ToString() + "; ";
            }

            if ((rollOverDate != DateTime.MinValue) && (tbsCert.StartDate.ToDateTime() > rollOverDate))
            {
                status = false;
                report = report + "Start date after CA Key Rollover: " + tbsCert.StartDate.ToString() + "; ";
            }

            if (tbsCert.EndDate.ToDateTime() > caEndDate)
            {
                status = false;
                report = report + "End date after CA end date: " + tbsCert.StartDate.ToString() + "; ";
            }

            return status;
        }
    }
}
