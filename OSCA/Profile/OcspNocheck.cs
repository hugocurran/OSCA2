/*
 * Copyright 2011, 2012, 2013, 2014 Peter Curran (peter@currans.eu). All rights reserved.
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

using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto.X509;
using System.Text;

namespace OSCA.Profile
{
    /// <summary>
    /// OCSP NoCheck extension
    /// </summary>
    /// <remarks>   
    /// A CA may specify that an OCSP client can trust a responder for the
   /// lifetime of the responder's certificate. The CA does so by including
   /// the extension id-pkix-ocsp-nocheck. This SHOULD be a non-critical
   /// extension. The value of the extension should be NULL. CAs issuing
   /// such a certificate should realized that a compromise of the
   /// responder's key, is as serious as the compromise of a CA key used to
   /// sign CRLs, at least for the validity period of this certificate. CA's
   /// may choose to issue this type of certificate with a very short
   /// lifetime and renew it frequently. (RFC 2560)<br />
   /// <code>
   /// id-pkix-ocsp-nocheck OBJECT IDENTIFIER ::= { id-pkix-ocsp 5 }
   /// </code>
   /// </remarks>
    public class ocspNocheck : ProfileExtension
    {        
        /// <summary>
        /// DER encoded value of OcspNocheck
        /// </summary>
        public OcspNocheck OcspNocheck { get { encode(); return (OcspNocheck)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create OcspNocheck extension
        /// </summary>
        public ocspNocheck() : this(false) { }

        /// <summary>
        /// Create OcspNocheck extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public ocspNocheck(bool Critical)
        {
            base.oid = OcspNocheck.ocspNocheck;
            base.name = "OcspNocheck";
            base.displayName = "OCSP Nocheck";
            base.critical = Critical;
        }

        /// <summary>
        /// Create OcspNocheck extension from XML profile file entry
        /// </summary>
        /// <param name="xml">OSCA XML description of the extension</param>
        /// <remarks>Sample OSCA XML description of the OcspNoCheck extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="OCSP Nocheck"&gt;OcspNocheck&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value /&gt;
        ///  &lt;/Extension&gt;
        /// </code> 
        /// </remarks>
        public ocspNocheck(XElement xml) : base(xml)
        {
            base.oid = OcspNocheck.ocspNocheck;
        }

        /// <summary>
        /// Create OcspNoCheck extension from X509Extension
        /// </summary>
        /// <param name="Extension">X509Extension instance</param>
        public ocspNocheck(X509Extension Extension) : base(Extension)
        {
            base.oid = OcspNocheck.ocspNocheck;
            base.name = "OcspNocheck";
            base.displayName = "OCSP Nocheck";
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <returns>XML encoding of the extension</returns>
        /// <remarks>Sample OSCA XML description of the OcspNoCheck extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="OCSP Nocheck"&gt;OcspNocheck&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value /&gt;
        ///  &lt;/Extension&gt;
        /// </code> 
        /// </remarks>
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

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
            output.AppendFormat("\tOCSP NoCheck\n");
            return output.AppendLine().ToString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        private void encode()
        {
            base.encValue = new OcspNocheck();
        }
    }
}
