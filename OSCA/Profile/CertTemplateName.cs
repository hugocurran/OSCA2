/*
 * Copyright 2014 Peter Curran (peter@currans.eu). All rights reserved.
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
using System.Text;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto;
using OSCA.Crypto.X509;
using OSCA.Offline;

namespace OSCA.Profile
{
    /// <summary>
    /// Certificate Template Name extension (MS)
    /// </summary>
    /// <remarks>   
    /// Microsoft private Certificate Template Name extension. This identifies the certificate template.
    /// </remarks>
    public class certTemplateName : ProfileExtension
    {
        /// <summary>
        /// DER encoded value of CertTemplateName
        /// </summary>
        public MsCertTemplateName CertTemplateName { get { encode(); return (MsCertTemplateName)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public string TemplateName { get; set; }

        public string Value { get { return value(); } }

        /// <summary>
        /// Create CertTemplateInfo extension
        /// </summary>
        public certTemplateName() : this(false) { }

        /// <summary>
        /// Create CertTemplateInfo extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public certTemplateName(bool Critical)
        {
            base.oid = MsCertTemplateName.CertTemplateName;
            base.name = "CertTemplateName";
            base.displayName = "Certificate Template Name";
            base.critical = Critical;
        }

        /// <summary>
        /// Create CertTemplateInfo extension from XML profile file entry
        /// </summary>
        /// <param name="xml">OSCA XML description of the extension</param>
        /// <remarks>Sample OSCA XML description of the CertTemplateName extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="Certificate Template Name"&gt;CertTemplateName&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///        &lt;template&gt;subCA&lt;/template&gt;
        ///    &lt;/value&gt;
        ///  &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        public certTemplateName(XElement xml)
            : base(xml)
        {
            base.oid = MsCertTemplateName.CertTemplateName;
            TemplateName = xmlValue.Element("template").Value;
         }

        /// <summary>
        /// Create CertTemplateInfo extension from X509Extension
        /// </summary>
        /// <param name="Extension">X509Extension instance</param>
        public certTemplateName(X509Extension Extension)
            : base(Extension)
        {
            base.oid = MsCertTemplateName.CertTemplateName;
            base.name = "CertTemplateName";
            base.displayName = "Certificate Template Name";

            MsCertTemplateName ctn = MsCertTemplateName.GetInstance(Extension);
            TemplateName = Utility.UTF8ByteArrayToString(ctn.TemplateName);
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <returns>XML encoding of the extension</returns>
        /// <remarks>Sample OSCA XML description of the CertTemplateName extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="Certificate Template Name"&gt;CertTemplateName&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///        &lt;template&gt;subCA&lt;/template&gt;
        ///    &lt;/value&gt;
        ///  &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

            // Create the parameter list
            XElement entry = extension.Element("value");
            entry.Add(new XElement("template", TemplateName));
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
            output.AppendFormat("\tCertificate Template Name (MS): {0}\n", TemplateName);
            return output.AppendLine().ToString();
        }

        private string value()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("{0}", TemplateName);
            return output.ToString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        private void encode()
        {
            base.encValue = new MsCertTemplateName(Utility.StringToUTF8ByteArray(TemplateName)).ToAsn1Object();
        }
    }
}
