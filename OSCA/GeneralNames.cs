using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;

namespace OSCA
{
    internal static class generalNames
    {
        /// <summary>
        /// Return the OSCA GenName type for a given type
        /// </summary>
        /// <param name="type">Type of name</param>
        /// <returns>GenName value</returns>
        internal static GenName getGenName(string type)
        {
            GenName gn = GenName.otherName;
            switch (type)
            {
                case "otherName":
                    gn = GenName.otherName;
                    break;
                case "rfc822Name":
                    gn = GenName.rfc822Name;
                    break;
                case "dNSName":
                    gn = GenName.dNSName;
                    break;
                case "x400Address":
                    gn = GenName.x400Address;
                    break;
                case "directoryName":
                    gn = GenName.directoryName;
                    break;
                case "ediPartyName":
                    gn = GenName.ediPartyName;
                    break;
                case "uniformResourceIdentifier":
                    gn = GenName.uniformResourceIdentifier;
                    break;
                case "iPAddress":
                    gn = GenName.iPAddress;
                    break;
                case "registeredID":
                    gn = GenName.registeredID;
                    break;
            }
            return gn;
        }

        /// <summary>
        /// Create a GeneralNames object containing multiple names
        /// </summary>
        /// <param name="genNames">Array of GeneralName</param>
        /// <returns>GeneralNames object</returns>
        internal static GeneralNames createGeneralNames(GeneralName[] genNames)
        {
            return new GeneralNames(new DerSequence(genNames));
        }

        /// <summary>
        /// Create a GeneralNames object containing one name
        /// </summary>
        /// <param name="genName">Name of the gen.</param>
        /// <returns>
        /// GeneralNames object
        /// </returns>
        internal static GeneralNames createGeneralNames(GeneralName genName)
        {
            return new GeneralNames(genName);
        }

        /// <summary>
        /// Create a GeneralNames instance containing one name
        /// </summary>
        /// <param name="type">Type of name</param>
        /// <param name="name">Name value</param>
        /// <returns>GeneralNames object</returns>
        internal static GeneralNames createGeneralNames(string type, string name)
        {
            return new GeneralNames(createGeneralName(type, name));
        }

        /// <summary>
        /// Create a GeneralName instance
        /// </summary>
        /// <param name="type">Type of name</param>
        /// <param name="name">Name value</param>
        /// <returns>GeneralName object</returns>
        internal static GeneralName createGeneralName(string type, string name)
        {
            GeneralName gName = null;
            switch (type)
            {
                case "otherName":
                    gName = new GeneralName(GeneralName.OtherName, name);
                    break;
                case "rfc822Name":
                    gName = new GeneralName(GeneralName.Rfc822Name, name);
                    break;
                case "dNSName":
                    gName = new GeneralName(GeneralName.DnsName, name);
                    break;
                case "x400Address":
                    gName = new GeneralName(GeneralName.X400Address, name);
                    break;
                case "directoryName":
                    gName = new GeneralName(GeneralName.DirectoryName, name);
                    break;
                case "ediPartyName":
                    gName = new GeneralName(GeneralName.EdiPartyName, name);
                    break;
                case "uniformResourceIdentifier":
                    gName = new GeneralName(GeneralName.UniformResourceIdentifier, name);
                    break;
                case "iPAddress":
                    gName = new GeneralName(GeneralName.IPAddress, name);
                    break;
                case "registeredID":
                    gName = new GeneralName(GeneralName.RegisteredID, name);
                    break;
            }
            return gName;
        }

        /// <summary>
        /// Create an OSCAGeneralName object from a GeneralName
        /// </summary>
        /// <param name="genName">GeneralName object</param>
        /// <returns>OSCAGeneralName object</returns>
        [Obsolete("Use the OSCAGeneralName constructor instead")]
        internal static OSCAGeneralName convertGeneralName(GeneralName genName)
        {
            OSCAGeneralName ogn = new OSCAGeneralName()
            {
                Name = genName.Name.ToString(),
                Type = (GenName)genName.TagNo
            };
            return ogn;
        }

    }
}
