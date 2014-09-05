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
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using System.Text;


namespace OSCA.Profile
{
    /// <summary>
    /// Basic Constraints extension
    /// </summary>
    /// <remarks>
    /// ASN.1 description from RFC 5280
    /// <code>
    /// id-ce-basicConstraints OBJECT IDENTIFIER ::=  { id-ce 19 }
    /// 
    /// BasicConstraints ::= SEQUENCE {
    ///     cA                      BOOLEAN DEFAULT FALSE,
    ///     pathLenConstraint       INTEGER (0..MAX) OPTIONAL }
    /// </code>
    /// </remarks>
    public class basicConstraints : ProfileExtension
    {
        private bool ca;
        private string pathLen;

        /// <summary>
        /// Constraint type
        /// </summary>
        /// <remarks>CA = true; Entity = false</remarks>
        public bool IsCA { get { return ca; } set { ca = value; } }

        /// <summary>
        /// Path length constraint
        /// </summary>
        /// <remarks>The value 'none' suppresses the PathLength constraint</remarks>
        public string PathLength { get { return pathLen; } set { pathLen = value; } }

        /// <summary>
        /// DER encoded value of BasicConstraints
        /// </summary>
        public BasicConstraints BasicConstraints { get { encode(); return (BasicConstraints)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create BasicConstraints extension
        /// </summary>
        /// <remarks>RFC 5280 states that CA should be false and non-critical by default</remarks>
        public basicConstraints() : this(false) { }

        /// <summary>
        /// Create BasicConstraints extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public basicConstraints(bool Critical) : base()
        {
            base.oid = X509Extensions.BasicConstraints;
            base.name = "BasicConstraints";
            base.displayName = "Basic Constraints";           
            base.critical = Critical;
            this.ca = false;    // Default
            this.pathLen = "none";
        }

        /// <summary>
        /// Create BasicConstraints extension from XML profile file entry
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the BasicConstraints extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Basic Constraints"&gt;BasicConstraints&lt;/name&gt;
        ///     &lt;critical&gt;true&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;ca&gt;true&lt;/ca&gt;
        ///         &lt;pathLen&gt;none&lt;/pathLen&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        /// <param name="xml">XML version of the extension</param>
        public basicConstraints(XElement xml) : base(xml)
        {
            base.oid = X509Extensions.BasicConstraints;
            ca = Convert.ToBoolean(((XElement)xmlValue).Element("ca").Value);
            pathLen = ((XElement)xmlValue).Element("pathLen").Value;
        }

        /// <summary>
        /// Create BasicConstraints extension from an X509Extension
        /// </summary>
        /// <param name="Extension">X509 extension</param>
        /// <remarks>
        /// Sub classses must provide an implementation to decode their values
        /// </remarks>
        public basicConstraints(X509Extension Extension) : base(Extension)
        {
            base.oid = X509Extensions.BasicConstraints;
            base.name = "BasicConstraints";
            base.displayName = "Basic Constraints";           

            BasicConstraints bc = BasicConstraints.GetInstance(Extension);
            ca = bc.IsCA();
            if (bc.PathLenConstraint != null)
                pathLen = bc.PathLenConstraint.ToString();
            else
                pathLen = "none";
        }

        /// <summary>
        /// Provide an XML description of the extension
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the BasicConstraints extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Basic Constraints"&gt;BasicConstraints&lt;/name&gt;
        ///     &lt;critical&gt;true&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;ca&gt;true&lt;/ca&gt;
        ///         &lt;pathLen&gt;none&lt;/pathLen&gt;
        ///     &lt;/value&gt;
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
            entry.Add(new XElement("ca", ca.ToString()),
                        new XElement("pathLen", pathLen)
                        );

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
            output.AppendFormat("\tCA: {0}\n", strBC());
            output.AppendFormat("\tPath Length: {0}\n", pathLen);
            return output.AppendLine().ToString();
        }

        private string strBC()
        {
            if (IsCA)
                return "CA";
            else
                return "Entity";
        }

        private void encode()
        {
            if (pathLen == "none")
                base.encValue = new BasicConstraints(ca);
            else if ((ca) && (Convert.ToInt32(pathLen) > -1))
                base.encValue = new BasicConstraints(Convert.ToInt32(pathLen));
            else if ((!ca) && (pathLen != "none"))
                throw new ApplicationException("Basic Constraints: Invalid Path Length Constraint: " + pathLen);
        }
    }
}
