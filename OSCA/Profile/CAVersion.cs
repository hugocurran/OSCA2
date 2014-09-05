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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Asn1;
using OSCA.Crypto.X509;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Profile
{
    /// <summary>
    /// CA Version extension (MS)
    /// </summary>
    /// <remarks>   
    /// Microsoft private CA Version extension. This refers to the version of the CA key and appears to be used to identify the correct CRL to use.
    /// </remarks>
    public class caVersion : ProfileExtension
    {
        /// <summary>
        /// DER encoded value of CaVersion
        /// </summary>
        public MsCaVersion CAVersion { get { encode(); return (MsCaVersion)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// CA Key number
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Create CaVersion extension
        /// </summary>
        public caVersion() : this(false) { }

        /// <summary>
        /// Create CaVersion extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public caVersion(bool Critical)
        {
            base.oid = MsCaVersion.CaVersion;
            base.name = "CaVersion";
            base.displayName = "CA Version";
            base.critical = Critical;
        }

        /// <summary>
        /// Create CaVersion extension from XML profile file entry
        /// </summary>
        /// <param name="xml">OSCA XML description of the extension</param>
        /// <remarks>Sample OSCA XML description of the CaVersion extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="CA Version"&gt;CaVersion&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///        &lt;version&gt;0&lt;/version&gt;
        ///    &lt;/value&gt;
        ///  &lt;/Extension&gt;
        /// </code> 
        /// </remarks>
        public caVersion(XElement xml)
            : base(xml)
        {
            base.oid = MsCaVersion.CaVersion;
            Version = Convert.ToInt32(xmlValue.Element("version").Value);
        }

        /// <summary>
        /// Create CaVersion extension from X509Extension
        /// </summary>
        /// <param name="Extension">X509Extension instance</param>
        public caVersion(X509Extension Extension)
            : base(Extension)
        {
            base.oid = MsCaVersion.CaVersion;
            base.name = "CaVersion";
            base.displayName = "CA Version";

            DerInteger ver = (DerInteger)X509Extension.ConvertValueToObject(Extension);
            MsCaVersion cav = MsCaVersion.GetInstance(Extension);
            Version = cav.Version;
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <returns>XML encoding of the extension</returns>
        /// <remarks>Sample OSCA XML description of the CaVersion extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="CA Version"&gt;CaVersion&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///        &lt;version&gt;0&lt;/version&gt;
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
            entry.Add(new XElement("version", Version.ToString()));

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
            output.AppendFormat("\tCA Version (MS)\n");
            return output.AppendLine().ToString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        private void encode()
        {
            base.encValue = new MsCaVersion(Version).ToAsn1Object();
        }
    }
}
