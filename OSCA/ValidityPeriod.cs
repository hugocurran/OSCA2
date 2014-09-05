using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OSCA
{
    /// <summary>
    /// Type containing a certificate validity period
    /// </summary>
    public struct ValidityPeriod
    {
        /// <summary>
        /// Units for validity period
        /// </summary>
        public enum Unit
        {
            /// <summary>
            /// Years
            /// </summary>
            Years,
            /// <summary>
            /// Months
            /// </summary>
            Months,
            /// <summary>
            /// Days
            /// </summary>
            Days,
            /// <summary>
            /// Hours
            /// </summary>
            Hours
        }

        /// <summary>
        /// Units
        /// </summary>
        public Unit Units;

        /// <summary>
        /// Period
        /// </summary>
        public int Period;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidityPeriod"/> struct.
        /// </summary>
        /// <param name="Units">Units.</param>
        /// <param name="Period">Period.</param>
        public ValidityPeriod(Unit Units, int Period)
        {
            this.Units = Units;
            this.Period = Period;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidityPeriod"/> struct.
        /// </summary>
        /// <param name="Units">Units (Years|Months|Days|Hours)</param>
        /// <param name="Period">Time period</param>
        /// <exception cref="System.ArgumentException">Unrecognised Validity Unit type: + Units</exception>
        public ValidityPeriod(string Units, int Period)
        {
            switch (Units)
            {
                case "Years":
                case "years":
                    this.Units = Unit.Years;
                    break;
                case "Months":
                case "months":
                    this.Units = Unit.Months;
                    break;
                case "Days":
                case "days":
                    this.Units = Unit.Days;
                    break;
                case "Hours":
                case "hours":
                    this.Units = Unit.Hours;
                    break;
                default:
                    throw new ArgumentException("Unrecognised Validity Unit type: " + Units);
            }
            this.Period = Period;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidityPeriod"/> struct from XML lifetime specification
        /// </summary>
        /// <param name="ValidityPeriod">XML lifetime description</param>
        /// <exception cref="System.ArgumentException">Unrecognised Validity Unit type:  + Value</exception>
        public ValidityPeriod(XElement ValidityPeriod)
        {
            // <lifetime units="Years">1</lifetime>
            switch (ValidityPeriod.Attribute("units").Value)
            {
                case "Years":
                case "years":
                    this.Units = Unit.Years;
                    break;
                case "Months":
                case "months":
                    this.Units = Unit.Months;
                    break;
                case "Days":
                case "days":
                    this.Units = Unit.Days;
                    break;
                case "Hours":
                case "hours":
                    this.Units = Unit.Hours;
                    break;
                default:
                    throw new ArgumentException("Unrecognised Validity Unit type: " + ValidityPeriod.Attribute("units").Value);
            }
            this.Period = Convert.ToInt32(ValidityPeriod.Value);
        }

        /// <summary>
        /// Calculate the NotAfter value
        /// </summary>
        /// <param name="NotBefore">NotBefore value</param>
        /// <returns>NotAfter value</returns>
        public DateTime NotAfter(DateTime NotBefore)
        {
            switch (Units)
            {
                case Unit.Years:
                    return NotBefore.AddYears(Period);
                case Unit.Months:
                    return NotBefore.AddMonths(Period);
                case Unit.Days:
                    return NotBefore.AddDays(Period);
                case Unit.Hours:
                    return NotBefore.AddHours(Period);
                default:
                    return DateTime.MinValue;   //Should never see this
            }
        }

        /// <summary>
        /// Calculate the NotBefore value
        /// </summary>
        /// <param name="NotAfter">NotAfter value</param>
        /// <returns>
        /// NotAfter value
        /// </returns>
        public DateTime NotBefore(DateTime NotAfter)
        {
            switch (Units)
            {
                case Unit.Years:
                    return NotAfter.AddYears(-Period);
                case Unit.Months:
                    return NotAfter.AddMonths(-Period);
                case Unit.Days:
                    return NotAfter.AddDays(-Period);
                case Unit.Hours:
                    return NotAfter.AddHours(-Period);
                default:
                    return DateTime.MinValue;   //Should never see this
            }
        }

        public TimeSpan ToTimeSpan()
        {
            switch (Units)
            {
                case Unit.Years:
                    return TimeSpan.FromDays(Period * 365);
                case Unit.Months:
                    return TimeSpan.FromDays(Period * 30);
                case Unit.Days:
                    return TimeSpan.FromDays(Period);
                case Unit.Hours:
                    return TimeSpan.FromHours(Period);
                default:
                    return TimeSpan.MinValue;   //Should never see this
            }
        }

        /// <summary>
        /// Provide an XML description of the validity period
        /// </summary>
        /// <returns>XML lifetime value</returns>
        public XElement ToXml(string ElementName)
        {
            return new XElement(ElementName,
                new XAttribute("units", Units.ToString()),
                Period.ToString());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1}\n", Period, Units);
            return sb.ToString();
        }
    }
}
