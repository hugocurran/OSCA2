/*
 * Copyright 2011 Peter Curran (peter@currans.eu). All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the 
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY PETER CURRAN "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL PETER CURRAN OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN 
 * IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the author alone. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;

namespace OSCA.Profile
{
    /// <summary>
    /// OSCA Access Description
    /// </summary>
    public struct AccessDesc
    {
        /// <summary>
        /// Access method
        /// </summary>
        public AccessMethod Method;
        /// <summary>
        /// Access location
        /// </summary>
        public OSCAGeneralName Location;
    }

    /// <summary>
    /// Access methods
    /// <remarks>
    /// This enum defines the Access Methods available in AIA and SIA extensions
    /// </remarks>
    /// </summary>
    public enum AccessMethod
    {
        /// <summary>
        /// CA Issuers
        /// </summary>
        CAIssuers,
        /// <summary>
        /// OCSP
        /// </summary>
        Ocsp,
        /// <summary>
        /// CA Repository
        /// </summary>
        CARepository,
        /// <summary>
        /// Time stamping
        /// </summary>
        TimeStamping
    }

    /// <summary>
    /// Abstract base class for Information Access extensions
    /// </summary>
    public abstract class InformationAccess : ProfileExtension
    {
        /// <summary>
        /// The encoded value
        /// </summary>
        protected Asn1Sequence encodedValue;

        // Local structure to hold the list of names
        /// <summary>
        /// The access desc
        /// </summary>
        protected List<AccessDesc> accessDesc = new List<AccessDesc>();

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create extension
        /// </summary>
        protected InformationAccess() : this(false) { }

        /// <summary>
        /// Create extension using supplied values
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        protected InformationAccess(bool Critical)
        {
            base.critical = Critical;
        }

        /// <summary>
        /// Create extension from XML profile file entry
        /// </summary>
        /// <param name="xml">XML version of the extension</param>
        protected InformationAccess(XElement xml) : base(xml)
        {
            foreach (XElement name in xmlValue.Descendants("accessDescription"))
            {
                AccessDesc descr = new AccessDesc()
                {
                    Method = toMethod(name.Element("method").Value),
                    Location = new OSCAGeneralName()
                    {
                        Name = name.Element("location").Value,
                        Type = generalNames.getGenName(name.Element("location").Attribute("type").Value)
                    }
                };
                accessDesc.Add(descr);
            }
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <returns>XML encoding of the extension</returns>
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

            // Create the parameter list
            XElement entry = extension.Element("value");
            foreach (AccessDesc desc in accessDesc)
            {
                entry.Add(new XElement("accessDescription",
                    new XElement("method", desc.Method),
                    new XElement("location", desc.Location.Name,
                        new XAttribute("type", desc.Location.Type.ToString()))
                        )
                    );
            }
            return extension;
        }

        /// <summary>
        /// Add a new AccessDescription Name to the extension
        /// </summary>
        /// <param name="Descr">Access description.</param>
        public void Add(AccessDesc Descr)
        {
            accessDesc.Add(Descr);
        }

        /// <summary>
        /// Remove an AccessDescription Name from the extension
        /// </summary>
        /// <param name="Descr">Access description.</param>
        public void Remove(AccessDesc Descr)
        {
            accessDesc.Remove(Descr);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        /// <returns></returns>
        protected Asn1Sequence encode()
        {
            // DER encoded names
            AccessDescription[] accDesc = new AccessDescription[accessDesc.Count()];

            for (int i = 0; i < accessDesc.Count; i++ )
            {
                GeneralName gn = generalNames.createGeneralName(
                    accessDesc[i].Location.Type.ToString(), 
                    accessDesc[i].Location.Name);
                accDesc[i] = new AccessDescription(toOID(accessDesc[i].Method), gn);
            }
            return new DerSequence(accDesc);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that describes this extension
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that describes this extension
        /// </returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("{0} ({1}) ({2})\n", name, oid, strCritical());
            output.AppendFormat("\tAccess Descriptions:\n");
            foreach (AccessDesc desc in accessDesc)
                output.AppendFormat("\t\t{0}: {1} ({2})\n", desc.Method, desc.Location.Name, desc.Location.Type);
            return output.AppendLine().ToString();
        }

        /// <summary>
        /// Decodes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected void decode(AccessDescription[] value)
        {
            foreach (AccessDescription ad in value)
            {
                AccessDesc accDesc = new AccessDesc()
                {
                    Method = fromOID(ad.AccessMethod),
                    Location = new OSCAGeneralName(ad.AccessLocation)
                };
                accessDesc.Add(accDesc);
            }
        }

        private AccessMethod toMethod(string method)
        {
            switch (method)
            {
                case "CAIssuers":
                    return AccessMethod.CAIssuers;
                case "Ocsp":
                    return AccessMethod.Ocsp;
                case "CARepository":
                    return AccessMethod.CARepository;
                case "TimeStamping":
                    return AccessMethod.TimeStamping;
            }
            return AccessMethod.CAIssuers;
        }

        private DerObjectIdentifier toOID(AccessMethod method)
        {
            switch (method)
            {
                case AccessMethod.CAIssuers:
                    return AccessDescription.IdADCAIssuers;
                case AccessMethod.Ocsp:
                    return AccessDescription.IdADOcsp;
                case AccessMethod.CARepository:
                    return AccessDescription.IdADCARepository;
                case AccessMethod.TimeStamping:
                    return AccessDescription.IdADTimeStamping;
            }
            return null;
        }

        private AccessMethod fromOID(DerObjectIdentifier method)
        {
            switch (method.Id)
            {
                case "1.3.6.1.5.5.7.48.2":
                    return AccessMethod.CAIssuers;
                case "1.3.6.1.5.5.7.48.1":
                    return AccessMethod.Ocsp;
                case "1.3.6.1.5.5.7.48.5":
                    return AccessMethod.CARepository;
                case "1.3.6.1.5.5.7.48.3":
                    return AccessMethod.TimeStamping;
            }
            return AccessMethod.CAIssuers;
        }
    }
}
