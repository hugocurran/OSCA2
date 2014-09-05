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
using OSCA.Crypto.X509;

namespace OSCA.Profile
{
    /// <summary>
    /// Certificate Template Information extension (MS)
    /// </summary>
    /// <remarks>   
    /// Microsoft private Certificate Template Information extension. This identifies the certificate template and version number.
    /// </remarks>
    public class certTemplateInfo : ProfileExtension
    {
        /// <summary>
        /// DER encoded value of CertTemplateInfo
        /// </summary>
        public MsCertTemplateInfo CertTemplateInfo { get { encode(); return (MsCertTemplateInfo)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Gets or sets the template OID.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the major version.
        /// </summary>
        /// <value>
        /// The major version.
        /// </value>
        public int MajorVersion { get; set; }

        /// <summary>
        /// Gets or sets the minor version.
        /// </summary>
        /// <value>
        /// The minor version.
        /// </value>
        public int MinorVersion { get; set; }

        /// <summary>
        /// Create CertTemplateInfo extension
        /// </summary>
        public certTemplateInfo() : this(false) { }

        /// <summary>
        /// Create CertTemplateInfo extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public certTemplateInfo(bool Critical)
        {
            base.oid = MsCertTemplateInfo.CertTemplateInfo;
            base.name = "CertTemplateInfo";
            base.displayName = "Certificate Template Information";
            base.critical = Critical;
        }

        /// <summary>
        /// Create CertTemplateInfo extension from XML profile file entry
        /// </summary>
        /// <param name="xml">OSCA XML description of the extension</param>
        /// <remarks>Sample OSCA XML description of the CertTemplateInfo extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="Certificate Template Information"&gt;CertTemplateInfo&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///        &lt;template&gt;1.2.3.4.5.6&lt;/template&gt;
        ///        &lt;majorVersion&gt;1&lt;/majorVersion&gt;
        ///        &lt;minorVersion&gt;4&lt;/minorVersion&gt;
        ///    &lt;/value&gt;
        ///  &lt;/Extension&gt;
        /// </code> 
        /// </remarks>
        public certTemplateInfo(XElement xml)
            : base(xml)
        {
            base.oid = MsCertTemplateInfo.CertTemplateInfo;
            Template = xmlValue.Element("template").Value;
            MajorVersion = Convert.ToInt32(xmlValue.Element("majorVersion").Value);
            MinorVersion = Convert.ToInt32(xmlValue.Element("minorVersion").Value);
        }

        /// <summary>
        /// Create CertTemplateInfo extension from X509Extension
        /// </summary>
        /// <param name="Extension">X509Extension instance</param>
        public certTemplateInfo(X509Extension Extension)
            : base(Extension)
        {
            base.oid = MsCertTemplateInfo.CertTemplateInfo;
            base.name = "CertTemplateInfo";
            base.displayName = "Certificate Template Information";

            MsCertTemplateInfo cti = MsCertTemplateInfo.GetInstance(Extension);
            Template = cti.Template;
            MajorVersion = cti.MajorVersion;
            MinorVersion = cti.MinorVersion;
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <returns>XML encoding of the extension</returns>
        /// <remarks>Sample OSCA XML description of the CertTemplateInfo extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="Certificate Template Information"&gt;CertTemplateInfo&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///        &lt;template&gt;1.2.3.4.5.6&lt;/template&gt;
        ///        &lt;majorVersion&gt;1&lt;/majorVersion&gt;
        ///        &lt;minorVersion&gt;4&lt;/minorVersion&gt;
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
            entry.Add(new XElement("template", Template));
            entry.Add(new XElement("majorVersion", MajorVersion.ToString()));
            entry.Add(new XElement("minorVersion", MinorVersion.ToString()));
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
            output.AppendFormat("\tCertificate Template Information (MS)\n");
            output.AppendFormat("\t\tTemplate: {0}, Version {1}.{2}\n", Template, MajorVersion, MinorVersion);
            return output.AppendLine().ToString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        private void encode()
        {
            base.encValue = new MsCertTemplateInfo(Template, MajorVersion, MinorVersion).ToAsn1Object();
        }
    }
}
