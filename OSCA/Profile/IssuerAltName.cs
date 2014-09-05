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

using System.Collections.Generic;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Profile
{
    /// <summary>
    /// Issuer Alternative Name extension
    /// </summary>
    /// <remarks>
    /// ASN.1 description from RFC 5280:
    /// <code>
    /// id-ce-issuerAltName OBJECT IDENTIFIER ::=  { id-ce 18 }
    /// 
    /// IssuerAltName ::= GeneralNames
    ///
    /// GeneralNames ::= SEQUENCE SIZE (1..MAX) OF GeneralName
    /// 
    /// GeneralName ::= CHOICE {
    ///     otherName                       [0]     OtherName,
    ///     rfc822Name                      [1]     IA5String,
    ///     dNSName                         [2]     IA5String,
    ///     x400Address                     [3]     ORAddress,
    ///     directoryName                   [4]     Name,
    ///     ediPartyName                    [5]     EDIPartyName,
    ///     uniformResourceIdentifier       [6]     IA5String,
    ///     iPAddress                       [7]     OCTET STRING,
    ///     registeredID                    [8]     OBJECT IDENTIFIER }
    ///     
    /// OtherName ::= SEQUENCE {
    ///     type-id    OBJECT IDENTIFIER,
    ///     value      [0] EXPLICIT ANY DEFINED BY type-id }
    ///     
    /// EDIPartyName ::= SEQUENCE {
    ///     nameAssigner            [0]     DirectoryString OPTIONAL,
    ///     partyName               [1]     DirectoryString }
    /// </code> 
    /// </remarks>
    public class issuerAltName : AlternativeName
    {
        /// <summary>
        /// IssuerAltName settings
        /// </summary>
        public List<OSCAGeneralName> IssAltNames { get { return base.genNames; } }

        /// <summary>
        /// Create a new IssuerAltName extension
        /// </summary>
        public issuerAltName() : this(false) { }

        /// <summary>
        /// Create a new IssuerAltName extension
        /// </summary>
        /// <param name="critical">True if critical, else false</param>
        public issuerAltName(bool critical)
            : base(critical)
        {
            base.oid = X509Extensions.IssuerAlternativeName;
            base.name = "IssuerAlternativeName";
            base.displayName = "Issuer Alternative Name";
        }

        /// <summary>
        /// Create a new IssuerAltName extension from an XML file
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the IssuerAlternativeName extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name displayName="Issuer Alternative Name"&gt;IssuerAlternativeName&lt;/name&gt;
        ///     &lt;critical&gt;false&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;altName type="rc822Name"&gt;peter@foo.com&lt;/altName&gt;
        ///         &lt;altName type="dNSName"&gt;peter.foo.com&lt;/altName&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        /// <param name="Xml"></param>
        public issuerAltName(XElement Xml) : base(Xml)
        {
            base.oid = X509Extensions.IssuerAlternativeName;
        }

        /// <summary>
        /// Create IssuerAltName extension from an X509Extension
        /// </summary>
        /// <param name="Extension">X509 extension</param>
        /// <remarks>
        /// Sub classses must provide an implementation to decode their values
        /// </remarks>
        public issuerAltName(X509Extension Extension) : base(Extension.IsCritical)
        {
            base.oid = X509Extensions.IssuerAlternativeName;
            base.name = "IssuerAlternativeName";
            base.displayName = "Issuer Alternative Name";

            decode(GeneralNames.GetInstance((Asn1Sequence)Extension.GetParsedValue()));
        }

    }
}
