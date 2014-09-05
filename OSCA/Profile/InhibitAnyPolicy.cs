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
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto;


namespace OSCA.Profile
{
    /// <summary>
    /// Inhibit Any Policy extension
    /// </summary>
    /// <remarks>ASN.1 description from RFC 5280:
    /// <code>
    /// id-ce-inhibitAnyPolicy OBJECT IDENTIFIER ::=  { id-ce 54 }
    /// 
    /// InhibitAnyPolicy ::= SkipCerts
    /// 
    /// SkipCerts ::= INTEGER (0..MAX)
    /// </code>
    /// </remarks>
    public class inhibitAnyPolicy : ProfileExtension
    {
        private int skip;

        /// <summary>
        /// Get/Set the skip value
        /// </summary>
        public int Skip { get { return skip; } set { skip = value; } }

        /// <summary>
        /// DER encoded value of InhibitAnyPolicy
        /// </summary>
        public DerInteger InhibitAnyPolicy { get { encode(); return (DerInteger)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        public override Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create InhibitAnyPolicy extension
        /// </summary>
        public inhibitAnyPolicy() : this(true) { }

        /// <summary>
        /// Create InhibitAnyPolicy extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public inhibitAnyPolicy(bool Critical)
        {
            base.oid = X509Extensions.InhibitAnyPolicy;
            base.name = "InhibitAnyPolicy";
            base.displayName = "Inhibit AnyPolicy";
            base.critical = Critical;
        }

        /// <summary>
        /// Create InhibitAnyPolicy extension from XML profile file entry
        /// </summary>
        /// <remarks>
        /// Sample XML description of the extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Inhibit AnyPolicy"&gt;InhibitAnyPolicy&lt;/name&gt;
        ///     &lt;critical&gt;true&lt;/critical&gt;
        ///     &lt;value&gt;1&lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        /// <param name="xml">XML version of the extension</param>
        public inhibitAnyPolicy(XElement xml) : base(xml)
        {
            base.oid = X509Extensions.InhibitAnyPolicy;
            skip = Convert.ToInt32(xmlValue.Value);
        }

        /// <summary>
        /// Create InhibitAnyPolicy extension from X509Extension
        /// </summary>
        /// <param name="Extension">X509Extension</param>
        public inhibitAnyPolicy(X509Extension Extension) : base(Extension)
        {
            base.oid = X509Extensions.InhibitAnyPolicy;
            base.name = "InhibitAnyPolicy";
            base.displayName = "Inhibit AnyPolicy";

            DerInteger skp = (DerInteger)Extension.GetParsedValue();
            this.skip = skp.Value.IntValue;
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <remarks>
        /// Sample XML description of the extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Inhibit AnyPolicy"&gt;InhibitAnyPolicy&lt;/name&gt;
        ///     &lt;critical&gt;true&lt;/critical&gt;
        ///     &lt;value&gt;1&lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        /// <returns>XML encoding of the extension</returns>
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

            // Create the parameter list
            XElement entry = extension.Element("value");
            entry.ReplaceWith(new XElement("value", skip.ToString()));

            return extension;
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
            output.AppendFormat("\tSkip: {0}\n", skip);
            return output.AppendLine().ToString();
        }

        private void encode()
        {
            base.encValue = new DerInteger(skip);
        }
    }
}