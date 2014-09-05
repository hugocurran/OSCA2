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
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto.X509;

namespace OSCA.Profile
{
    
    /// <summary>
    /// Subject Information Access extension
    /// </summary>
    /// <remarks>
    /// ASN.1 description from RFC 5280:
    /// <code>
    /// id-pe-subjectInfoAccess OBJECT IDENTIFIER ::= { id-pe 11 }
    /// 
    /// SubjectInfoAccessSyntax  ::= SEQUENCE SIZE (1..MAX) OF AccessDescription
    /// 
    /// AccessDescription  ::=  SEQUENCE {
    ///     accessMethod          OBJECT IDENTIFIER,
    ///     accessLocation        GeneralName  }
    ///     
    /// id-ad OBJECT IDENTIFIER ::= { id-pkix 48 }
    ///
    /// id-ad-caRepository OBJECT IDENTIFIER ::= { id-ad 5 }
    /// 
    /// id-ad-timeStamping OBJECT IDENTIFIER ::= { id-ad 3 }
    /// </code>
    /// </remarks>
    public class subjectInfoAccess : InformationAccess
    {
        /// <summary>
        /// SIA settings
        /// </summary>
        public List<AccessDesc> SubjectInfoAccess { get { return base.accessDesc; } }

        /// <summary>
        /// DER encoded value of SubjectInfoAccess
        /// </summary>
        public SubjectInformationAccess SubjectInformationAccess { get { encode(); return (SubjectInformationAccess)base.encValue; } }

        /// <summary>
        /// Create a new SIA extension
        /// </summary>
        public subjectInfoAccess() : this(false) { }

        /// <summary>
        /// Create a new SIA extension
        /// </summary>
        /// <param name="critical">True if critical, else false</param>
        public subjectInfoAccess(bool critical)
            : base(critical)
        {
            base.oid = X509Extensions.SubjectInfoAccess;
            base.name = "SubjectInfoAccess";
            base.displayName = "Subject Information Access";
        }

        /// <summary>
        /// Create a new SIA extension from an XML file
        /// </summary>
        /// <param name="Xml"></param>
        /// <remarks>
        /// A sample XML description of the extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Subject Info Access"&gt;SubjectInfoAccess&lt;/name&gt;
        ///     &lt;critical&gt;false&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;accessDescription&gt;
        ///             &lt;method&gt;CARepository&lt;/method&gt;
        ///             &lt;location type="uniformResourceIdentifier"&gt;http://foo.com/issuer&lt;/location&gt;
        ///         &lt;/accessDescription&gt;
        ///         &lt;accessDescription&gt;
        ///             &lt;method&gt;TimeStamping&lt;/method&gt;
        ///             &lt;location type="uniformResourceIdentifier"&gt;http://foo.com/time&lt;/location&gt;
        ///         &lt;/accessDescription&gt;    
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        public subjectInfoAccess(XElement Xml)
            : base(Xml)
        {
            base.oid = X509Extensions.SubjectInfoAccess;
        }

        /// <summary>
        /// Create SIA extension from an X509Extension
        /// </summary>
        /// <param name="Extension">X509Extension instance</param>
        public subjectInfoAccess(X509Extension Extension) : base(Extension.IsCritical)
        {            
            base.oid = X509Extensions.SubjectInfoAccess;
            base.name = "SubjectInfoAccess";
            base.displayName = "Subject Information Access";

            SubjectInformationAccess sia = SubjectInformationAccess.GetInstance(Extension);
            decode(sia.GetAccessDescriptions());
        }

        private new void encode()
        {
            base.encValue = SubjectInformationAccess.GetInstance(base.encode());
        }
    }
}