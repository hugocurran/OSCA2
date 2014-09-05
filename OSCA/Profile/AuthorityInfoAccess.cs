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

using System.Collections.Generic;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using System.Text;

namespace OSCA.Profile
{
    /// <summary>
    /// Authority Information Access extension
    /// </summary>
    /// <remarks>
    /// Asn.1 description from RFC5280:
    /// <code>
    /// id-pe-authorityInfoAccess OBJECT IDENTIFIER ::= { id-pe 1 }
    /// 
    /// AuthorityInfoAccessSyntax  ::=
    ///     SEQUENCE SIZE (1..MAX) OF AccessDescription
    ///     
    ///     AccessDescription  ::=  SEQUENCE {
    ///     accessMethod          OBJECT IDENTIFIER,
    ///     accessLocation        GeneralName  }
    ///     
    /// id-ad OBJECT IDENTIFIER ::= { id-pkix 48 }
    /// 
    /// id-ad-caIssuers OBJECT IDENTIFIER ::= { id-ad 2 }
    /// 
    /// id-ad-ocsp OBJECT IDENTIFIER ::= { id-ad 1 }
    /// </code>
    /// </remarks> 
    public class authorityInfoAccess : InformationAccess
    {
        /// <summary>
        /// AIA settings
        /// </summary>
        public List<AccessDesc> AuthInfoAccess { get { return base.accessDesc; } }

        /// <summary>
        /// DER encoded value of AuthorityInfoAccess
        /// </summary>
        public AuthorityInformationAccess AuthorityInformationAccess { get { encode(); return (AuthorityInformationAccess)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create a new AIA extension
        /// </summary>
        public authorityInfoAccess() : this(false) { }

        /// <summary>
        /// Create a new AIA extension
        /// </summary>
        /// <param name="critical">True if critical, else false</param>
        public authorityInfoAccess(bool critical) : base(critical)
        {
            base.oid = X509Extensions.AuthorityInfoAccess;
            base.name = "AuthorityInfoAccess";
            base.displayName = "Authority Information Access";
        }

        /// <summary>
        /// Create AIA extension from XML profile description
        /// </summary>
        /// <remarks>
        /// Sample XML description of the AIA extension:
        /// </remarks>
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Authority Info Access"&gt;AuthorityInfoAccess&lt;/name&gt;
        ///     &lt;critical&gt;false&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;accessDescription&gt;
        ///             &lt;method&gt;CAIssuers&lt;/method&gt;
        ///             &lt;location type="uniformResourceIdentifier"&gt;http://foo.com/issuer.crt&lt;/location&gt;
        ///         &lt;/accessDescription&gt;
        ///         &lt;accessDescription&gt;
        ///             &lt;method&gt;Ocsp&lt;/method&gt;
        ///             &lt;location type="uniformResourceIdentifier"&gt;http://foo.com:2560/ocsp&lt;/location&gt;
        ///         &lt;accessDescription&gt;   
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// <param name="Xml">XML element enclosing the extension description</param>
        public authorityInfoAccess(XElement Xml) : base(Xml)
        {
            base.oid = X509Extensions.AuthorityInfoAccess;
        }

        /// <summary>
        /// Create AIA extension from an X509Extension
        /// </summary>
        /// <param name="Extension">X509 extension</param>
        /// <remarks>
        /// Sub classses must provide an implementation to decode their values
        /// </remarks>
        public authorityInfoAccess(X509Extension Extension) : base(Extension.IsCritical)
        {
            base.oid = X509Extensions.AuthorityInfoAccess;
            base.name = "AuthorityInfoAccess";
            base.displayName = "Authority Information Access";

            AuthorityInformationAccess aia = AuthorityInformationAccess.GetInstance(Extension);
            decode(aia.GetAccessDescriptions());
        }

        /// <summary>
        /// Encode the extension
        /// </summary>
        private new void encode()
        {
            base.encValue = AuthorityInformationAccess.GetInstance(base.encode());
        }
    }
}
