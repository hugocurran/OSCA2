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
using System.Text;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;
using OSCA.Crypto;

namespace OSCA.Profile
{
    /// <summary>
    /// Abstract base class for OSCA profile system 
    /// </summary>
    public abstract class ProfileExtension
    {
        /// <summary>
        /// Extension name (per RFC5280)
        /// </summary>
        protected string name;
        /// <summary>
        /// Extension name (per OSCA profile system)
        /// </summary>
        protected string displayName;
        /// <summary>
        /// OSCA XML description of extension
        /// </summary>
        protected XElement xmlValue;
        /// <summary>
        /// Extension criticality
        /// </summary>
        protected bool critical;
        /// <summary>
        /// Extension OID
        /// </summary>
        protected DerObjectIdentifier oid;  
        /// <summary>
        /// DER encoded extension value (not wrapped in OctetString)
        /// </summary>
        protected Asn1Encodable encValue;

        /// <summary>
        /// Extension name
        /// </summary>
        public string Name { get { return name; } }
        /// <summary>
        /// Extension display name
        /// </summary>
        public string DisplayName { get { return displayName; } }
        /// <summary>
        /// Extension OID
        /// </summary>
        public DerObjectIdentifier OID { get { return oid; } }
        /// <summary>
        /// XML representation of the extension
        /// </summary>
        public virtual string XmlValue { get { return xmlValue.ToString(); } }
        /// <summary>
        /// Extension criticality
        /// </summary>
        public bool Critical { get { return critical; } set { critical = value; } }

        /// <summary>
        /// DER encoding of the extension
        /// </summary>
        public abstract Asn1Encodable DerEncoding { get; }

        /// <summary>
        /// Null constructor
        /// </summary>
        public ProfileExtension() { }

        /// <summary>
        /// Create an extension from an OSCA XML extension description
        /// </summary>
        /// <remarks>Sub classses must provide an implementation to decode their values</remarks>
        /// <param name="Xml">XML description</param>
        public ProfileExtension(XElement Xml) 
        {
            fromXml(Xml);
        }

        /// <summary>
        /// Create an extension from an existing X509 extension
        /// </summary>
        /// <remarks>Sub classses must provide an implementation to decode their values</remarks>
        /// <param name="Extension">X509 extension</param>
        public ProfileExtension(X509Extension Extension)
        {
            critical = Extension.IsCritical;
        }

        /// <summary>
        /// Describe an extension using the OSCA XML extension format
        /// </summary>
        /// <remarks>Sub classses must provide an implementation to describe their values</remarks>
        /// <returns>OSCA XML extension description</returns>
        public virtual XNode ToXml() 
        {
            // Create the generic profile extension
            XElement extension = new XElement("Extension",
                    new XElement("name", name,
                        new XAttribute("displayName", displayName)
                    ),
                    new XElement("critical", critical.ToString()),
                    new XElement("value")
            );
            return extension;
        }

        /// <summary>
        /// Read the header of an OSCA XML description
        /// </summary>
        /// <remarks>Sub classses should call this method before decoding their values</remarks>
        /// <param name="ext">OSCA XML extension description</param>
        private void fromXml(XElement ext)
        {
            name = ext.Element("name").Value;
            displayName = ext.Element("name").Attribute("displayName").Value;
            critical = Convert.ToBoolean(ext.Element("critical").Value);
            xmlValue = ext.Element("value");
        }


        /// <summary>
        /// Returns a string containing the criticality
        /// </summary>
        /// <remarks>Primarily for use in the ToString() methods in subclasses</remarks>
        /// <returns>String describing criticality</returns>
        protected string strCritical()
        {
            if (critical)
                return "Critical";
            else
                return "Noncritical";
        }
    }
}
