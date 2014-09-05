/*
 * Copyright 2011-14 Peter Curran (peter@currans.eu). All rights reserved.
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
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto;
using Org.BouncyCastle.Asn1;

namespace OSCA.Profile
{
    /// <summary>
    /// CRL Distribution Point extension
    /// </summary>
    /// <remarks>ASN.1 description from RFC 5280
    /// <code>
    /// id-ce-cRLDistributionPoints OBJECT IDENTIFIER ::=  { id-ce 31 }
    /// 
    /// CRLDistributionPoints ::= SEQUENCE SIZE (1..MAX) OF DistributionPoint
    /// 
    /// DistributionPoint ::= SEQUENCE {
    ///     distributionPoint       [0]     DistributionPointName OPTIONAL,
    ///     reasons                 [1]     ReasonFlags OPTIONAL,
    ///     cRLIssuer               [2]     GeneralNames OPTIONAL }
    ///     
    /// DistributionPointName ::= CHOICE {
    ///     fullName                [0]     GeneralNames,
    ///     nameRelativeToCRLIssuer [1]     RelativeDistinguishedName }
    ///     
    /// ReasonFlags ::= BIT STRING {
    ///     unused                  (0),
    ///     keyCompromise           (1),
    ///     cACompromise            (2),
    ///     affiliationChanged      (3),
    ///     superseded              (4),
    ///     cessationOfOperation    (5),
    ///     certificateHold         (6),
    ///     privilegeWithdrawn      (7),
    ///     aACompromise            (8) }
    /// </code>
    /// Note that this class does not support reasons or cRLIssuer in the DistributionPoint
    /// </remarks>
    public class crlDistPoint : DistributionPoints
    {

        /// <summary>
        /// DER encoded value of CrlDistPoint
        /// </summary>
        public CrlDistPoint CrlDistributionPoint { get { encode(); return (CrlDistPoint)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create CrlDistPoint extension
        /// </summary>
        public crlDistPoint() : this(false) { }

        /// <summary>
        /// Create CrlDistPoint extension using supplied values
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public crlDistPoint(bool Critical) : base(Critical)
        {
            base.oid = X509Extensions.CrlDistributionPoints;
            base.name = "CrlDistributionPoints";
            base.displayName = "CRL Distribution Points";
        }

        /// <summary>
        /// Create CRLDistributionPoints extension from XML description
        /// </summary>
        /// <remarks>Sample OSCA XML description of the CDP extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="CRL Distribution Points"&gt;CrlDistributionPoints&lt;/name&gt;
        ///     &lt;critical&gt;true&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;cdp&gt;
        ///             &lt;name type="uniformResourceIdentifier"&gt;http://crl.foo.org/wotsit.crl&lt;/name&gt;
        ///         &lt;/cdp&gt;
        ///         &lt;cdp&gt;
        ///             &lt;name type="uniformResourceIdentifier"&gt;http://www.bar.org/wotsit.crl&lt;/name&gt;
        ///         &lt;/cdp&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        /// <param name="xml">XML version of the extension</param>
        public crlDistPoint(XElement xml) : base(xml)
        {
            base.oid = X509Extensions.CrlDistributionPoints;
        }

        /// <summary>
        /// Create CRLDistributionPoints extension from X509Extension
        /// </summary>
        /// <param name="Extension"></param>
        public crlDistPoint(X509Extension Extension) : base(Extension.IsCritical)
        {
            base.oid = X509Extensions.CrlDistributionPoints;
            base.name = "CrlDistributionPoints";
            base.displayName = "CRL Distribution Points";

            CrlDistPoint cdp = CrlDistPoint.GetInstance(Extension);

            // Call the DistributionPoints encode() method to read the DPList
            base.decode(cdp.GetDistributionPoints());
        }

        private new void encode()
        {
            // Call the DistributionPoints encode() method to generate the DPList
            base.encValue = new CrlDistPoint(base.encode());
        }
    }
}
