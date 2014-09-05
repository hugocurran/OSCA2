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
using OSCA.Offline;


namespace OSCA.Profile
{
    /// <summary>
    /// CA Version extension (MS)
    /// </summary>
    /// <remarks>   
    /// Microsoft private CA Version extension. This refers to the version of the CA key and appears to be used to identify the correct CRL to use.
    /// </remarks>
    public class prevCaCertHash : ProfileExtension
    {
        /// <summary>
        /// DER encoded value of CaVersion
        /// </summary>
        public MsPrevCaCertHash CAVersion { get { encode(); return (MsPrevCaCertHash)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Previous CA Certificate Hash
        /// </summary>
        public byte[] Hash { get; set; }

        /// <summary>
        /// Digest string describing the extension
        /// </summary>
        public string Value { get { return value(); } }

        /// <summary>
        /// Create PrevCaCertHash extension
        /// </summary>
        public prevCaCertHash() : this(false) { }

        /// <summary>
        /// Create PrevCaCertHash extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public prevCaCertHash(bool Critical)
        {
            base.oid = MsPrevCaCertHash.PrevHash;
            base.name = "PrevCaCertHash";
            base.displayName = "Previous CA Certificate Hash";
            base.critical = Critical;
        }

        /// <summary>
        /// Create CaVersion extension from XML profile file entry
        /// </summary>
        /// <param name="xml">OSCA XML description of the extension</param>
        /// <remarks>Sample OSCA XML description of the PrevCaCertHash extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="Previous CA Certificate Hash"&gt;PrevCaCertHash&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///        &lt;hash&gt;01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16&lt;/hash&gt;
        ///    &lt;/value&gt;
        ///  &lt;/Extension&gt;
        /// </code> 
        /// </remarks>
        public prevCaCertHash(XElement xml)
            : base(xml)
        {
            base.oid = MsPrevCaCertHash.PrevHash;
            Hash = Utility.StringToUTF8ByteArray(xmlValue.Element("hash").Value);
        }

        /// <summary>
        /// Create CaVersion extension from X509Extension
        /// </summary>
        /// <param name="Extension">X509Extension instance</param>
        public prevCaCertHash(X509Extension Extension)
            : base(Extension)
        {
            base.oid = MsPrevCaCertHash.PrevHash;
            base.name = "PrevCaCertHash";
            base.displayName = "Previous CA Certificate Hash";

            DerOctetString hash = (DerOctetString)X509Extension.ConvertValueToObject(Extension);
            MsPrevCaCertHash pHash = MsPrevCaCertHash.GetInstance(Extension);
            Hash = pHash.Hash;
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <returns>XML encoding of the extension</returns>
        /// <remarks>Sample OSCA XML description of the PrevCaCertHash extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="Previous CA Certificate Hash"&gt;PrevCaCertHash&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///        &lt;hash&gt;01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16&lt;/hash&gt;
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
            entry.Add(new XElement("hash", Utility.UTF8ByteArrayToString(Hash)));

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
            output.Append("\tPrevious CA Certificate Hash (MS): ");
            foreach (byte val in Hash)
                output.AppendFormat("{0} ", val.ToString("X"));
            output.AppendLine();
            return output.AppendLine().ToString();
        }

        private string value()
        {
            StringBuilder output = new StringBuilder();
            foreach (byte val in Hash)
                output.AppendFormat("{0} ", val.ToString("X"));
            return output.ToString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        private void encode()
        {
            base.encValue = new MsPrevCaCertHash(Hash).ToAsn1Object();
        }
    }
}
