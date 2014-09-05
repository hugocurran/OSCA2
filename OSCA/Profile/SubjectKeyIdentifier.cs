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

using System.Collections.Generic;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using System.Text;

namespace OSCA.Profile
{
    /// <summary>
    /// Subject Key Identifier extension
    /// </summary>
    /// <remarks>
    /// Asn.1 description from RFC5280:
    /// <code>
    /// id-ce-subjectKeyIdentifier OBJECT IDENTIFIER ::=  { id-ce 14 }
    /// 
    /// SubjectKeyIdentifier ::= KeyIdentifier
    /// 
    /// KeyIdentifier ::= OCTET STRING
    /// </code>
    /// </remarks> 
    public class subjectKeyIdentifier : KeyIdentifier
    {
        /// <summary>
        /// DER encoded value of SubjectKeyIdentifier
        /// </summary>
        public SubjectKeyIdentifier SubjectKeyIdentifier { get { encode(); return (SubjectKeyIdentifier)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create a new SKID extension
        /// </summary>
        public subjectKeyIdentifier() : this(false) { }

        /// <summary>
        /// Create a new SKID extension
        /// </summary>
        /// <param name="critical">True if critical, else false</param>
        public subjectKeyIdentifier(bool critical)
            : base(critical)
        {
            base.oid = X509Extensions.SubjectKeyIdentifier;
            base.name = "SubjectKeyIdentifier";
            base.displayName = "Subject Key Identifier";
        }

        /// <summary>
        /// Create SKID extension from XML profile description
        /// </summary>
        /// <remarks>
        /// Sample XML description of the SKID extension:
        /// </remarks>
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Subject Key Identifier"&gt;SubjectKeyIdentifier&lt;/name&gt;
        ///     &lt;critical&gt;false&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;keyID method="Full"&gt;00010203040506070809101112131415&lt;/keyID&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// <param name="Xml">XML element enclosing the extension description</param>
        public subjectKeyIdentifier(XElement Xml)
            : base(Xml)
        {
            base.oid = X509Extensions.SubjectKeyIdentifier;
        }

        /// <summary>
        /// Create SKID extension from an X509Extension
        /// </summary>
        /// <param name="Extension">X509 extension</param>
        /// <remarks>
        /// The work is done in the base class
        /// </remarks>
        public subjectKeyIdentifier(X509Extension Extension)
            : base(Extension.IsCritical)
        {
            base.oid = X509Extensions.SubjectKeyIdentifier;
            base.name = "SubjectKeyIdentifier";
            base.displayName = "Subject Key Identifier";

            SubjectKeyIdentifier skid = SubjectKeyIdentifier.GetInstance(Extension);
            keyIdentifier = skid.GetKeyIdentifier();
        }

        /// <summary>
        /// Encode the extension
        /// </summary>
        private new void encode()
        {
            base.encValue = SubjectKeyIdentifier.GetInstance(keyIdentifier);
        }
    }
}

