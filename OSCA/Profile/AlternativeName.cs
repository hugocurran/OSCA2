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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto;

namespace OSCA.Profile
{
  
    /// <summary>
    /// Abstract base class for Alternative Names extensions
    /// <remarks>
    /// ASN.1 description from RFC 5280:
    /// <code>
    /// id-ce-subjectAltName OBJECT IDENTIFIER ::=  { id-ce 17 }
    /// 
    /// SubjectAltName ::= GeneralNames
    /// 
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
    /// </summary>
    public abstract class AlternativeName : ProfileExtension
    {
        // Local structure to hold the list of names
        // Note that base.encValue holds the DER encoded version
        /// <summary>
        /// The gen names
        /// </summary>
        protected List<OSCAGeneralName> genNames = new List<OSCAGeneralName>();

        /// <summary>
        /// DER encoded value of GeneralNames
        /// </summary>
        public GeneralNames GeneralNames { get { encode(); return (GeneralNames)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create extension using supplied values
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        protected AlternativeName(bool Critical)
        {
            base.critical = Critical;
        }

        /// <summary>
        /// Create extension from XML profile file entry
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the SubjectAlternativeName extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name displayName="Subject Alternative Name"&gt;SubjectAlternativeName&lt;/name&gt;
        ///     &lt;critical&gt;false&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;altName type="rc822Name"&gt;peter@foo.com&lt;/altName&gt;
        ///         &lt;altName type="dNSName"&gt;peter.foo.com&lt;/altName&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// Note that either a SubjectAlternativeName or an IssuerAlternativeName is processed the same.
        /// </remarks>
        /// <param name="xml">XML version of the extension</param>
        protected AlternativeName(XElement xml) 
            : base(xml)
        {
            foreach (XElement name in xmlValue.Descendants("altName"))
            {
                OSCAGeneralName altName = new OSCAGeneralName()
                {
                    Name = name.Value,
                    Type = generalNames.getGenName(name.Attribute("type").Value)
                };
                genNames.Add(altName);
            }
        }
      
        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the SubjectAlternativeName extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name displayName="Subject Alternative Name"&gt;SubjectAlternativeName&lt;/name&gt;
        ///     &lt;critical&gt;false&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;altName type="rc822Name"&gt;peter@foo.com&lt;/altName&gt;
        ///         &lt;altName type="dNSName"&gt;peter.foo.com&lt;/altName&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// Note that either a SubjectAlternativeName or an IssuerAlternativeName is processed the same.
        /// </remarks>
        /// <returns>XML encoding of the extension</returns>
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

            // Create the parameter list
            XElement entry = extension.Element("value");
            foreach (OSCAGeneralName altName in genNames)
            {
                entry.Add(new XElement("altName", altName.Name,
                    new XAttribute("type", altName.Type.ToString()))
                    );
            }
            return extension;
        }

        /// <summary>
        /// Add a new Alt Name to the extension
        /// </summary>
        /// <param name="Name">The name.</param>
        public void Add(OSCAGeneralName Name)
        {
            genNames.Add(Name);
        }

        /// <summary>
        /// Remove an Alt Name from the extension
        /// </summary>
        /// <param name="Name">The name.</param>
        public void Remove(OSCAGeneralName Name)
        {
            genNames.Remove(Name);
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
            output.AppendFormat("\tAlternative Names:\n");
            foreach (OSCAGeneralName gname in genNames)                
                output.AppendFormat("\t\t{0}: {1}\n", gname.Type.ToString(), gname.Name);
            return output.AppendLine().ToString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        protected void encode()
        {
            // DER encoded names
            GeneralName[] genName = new GeneralName[genNames.Count()];

            int i = 0;
            foreach (OSCAGeneralName altName in genNames)
            {
                genName[i] = generalNames.createGeneralName(altName.Type.ToString(), altName.Name);
                i++;
            }
            base.encValue = generalNames.createGeneralNames(genName);
        }

        /// <summary>
        /// Decodes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected void decode(GeneralNames value)
        {
            GeneralName[] gnList = value.GetNames();
            foreach (GeneralName gn in gnList)
                genNames.Add(new OSCAGeneralName(gn));
        }
    }
}
