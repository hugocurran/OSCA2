/*
 * Copyright 2011, 2014 Peter Curran (peter@currans.eu). All rights reserved.
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
    /// Policy mapping information
    /// </summary>
    public struct PolicyMapping
    {
        /// <summary>
        /// Issuer policy oid
        /// </summary>
        public string issuerOid;
        /// <summary>
        /// Issuer policy name
        /// </summary>
        public string issuerPolicyName;
        /// <summary>
        /// Subject policy oid
        /// </summary>
        public string subjectOid;
        /// <summary>
        /// Subject policy name
        /// </summary>
        public string subjectPolicyName;
    }

    /// <summary>
    /// Policy Mappings Extension
    /// </summary>
    /// <remarks>
    /// ASN.1 description of the extension from RFC 5280
    /// <code>
    /// id-ce-policyMappings OBJECT IDENTIFIER ::=  { id-ce 33 }
    /// 
    /// PolicyMappings ::= SEQUENCE SIZE (1..MAX) OF SEQUENCE {
    ///     issuerDomainPolicy      CertPolicyId,
    ///     subjectDomainPolicy     CertPolicyId }
    ///     
    /// CertPolicyId ::= OBJECT IDENTIFIER
    /// </code>
    /// </remarks>
    public class policyMappings : ProfileExtension
    {
        // Local structure to hold the list of mappings
        // Note that base.encValue holds the DER encoded version
        private List<PolicyMapping> mappings = new List<PolicyMapping>();

        /// <summary>
        /// List of mappings
        /// </summary>
        public List<PolicyMapping> Mappings { get { return mappings; } }

        /// <summary>
        /// DER encoded value of PolicyMappings
        /// </summary>
        public PolicyMappings PolicyMappings { get { encode(); return (PolicyMappings)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create PolicyMappings extension
        /// </summary>
        public policyMappings() : this(false) { }

        /// <summary>
        /// Create PolicyMappings extension using supplied values
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public policyMappings(bool Critical)
        {
            base.oid = X509Extensions.PolicyMappings;
            base.name = "PolicyMappings";
            base.displayName = "Policy Mappings";
            base.critical = Critical;
        }

        /// <summary>
        /// Create PolicyMappings extension from XML profile file entry
        /// </summary>
        /// <param name="xml">XML version of the extension</param>
        /// <remarks>Sample OSCA XML description of the PolicyMappings extension:
        /// <code>
        /// &lt;Extension&gt;
        ///  &lt;name description="Policy Mappings"&gt;PolicyMappings&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///      &lt;mapping&gt;
        ///        &lt;issuerPolicy name="Some policy"&gt;1.2.3.4&lt;/issuerPolicy&gt;
        ///        &lt;subjectPolicy name="Other policy"&gt;4.5.6.7&lt;/subjectPolicy&gt;
        ///      &lt;/mapping&gt;
        ///    &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        public policyMappings(XElement xml) : base(xml)
        {
            base.oid = X509Extensions.PolicyMappings;

            foreach (XElement polID in xmlValue.Descendants("mapping"))
            {
                PolicyMapping map = new PolicyMapping() {
                    issuerOid = polID.Element("issuerPolicy").Value,
                    issuerPolicyName = polID.Element("issuerPolicy").Attribute("name").Value,
                    subjectOid = polID.Element("subjectPolicy").Value,
                    subjectPolicyName = polID.Element("subjectPolicy").Attribute("name").Value,
                };
                mappings.Add(map);
            }
        }

        /// <summary>
        /// Create PolicyMappings extension from X509Extension
        /// </summary>
        /// <param name="Extension">Extension instance</param>
        public policyMappings(X509Extension Extension) : base(Extension)
        {
            base.oid = X509Extensions.PolicyMappings;
            base.name = "PolicyMappings";
            base.displayName = "Policy Mappings";

            PolicyMappings pm = PolicyMappings.GetInstance(Extension);
            foreach (Asn1Sequence inner in pm.Mappings)
            {

                PolicyMapping map = new PolicyMapping()
                {
                    issuerOid = ((DerObjectIdentifier)inner[0]).Id,
                    subjectOid = ((DerObjectIdentifier)inner[1]).Id
                };
                mappings.Add(map);
            }
        }

        /// <summary>
        /// Add a new PolicyMapping to the extension
        /// </summary>
        /// <param name="Mapping">Policy mapping</param>
        public void Add(PolicyMapping Mapping)
        {
            mappings.Add(Mapping);
        }

        /// <summary>
        /// Remove a PolicyMapping from the extension
        /// </summary>
        /// <param name="Mapping">Policy mapping</param>
        public void Remove(PolicyMapping Mapping)
        {
            mappings.Remove(Mapping);
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <returns>XML encoding of the extension</returns>
        /// <remarks>Sample OSCA XML description of the PolicyMappings extension:
        /// <code>
        /// &lt;Extension&gt;
        ///  &lt;name description="Policy Mappings"&gt;PolicyMappings&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///      &lt;mapping&gt;
        ///        &lt;issuerPolicy name="Some policy"&gt;1.2.3.4&lt;/issuerPolicy&gt;
        ///        &lt;subjectPolicy name="Other policy"&gt;4.5.6.7&lt;/subjectPolicy&gt;
        ///      &lt;/mapping&gt;
        ///    &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

            // Create the parameter list
            XElement entry = extension.Element("value");
            foreach (PolicyMapping mapping in mappings)
            {
                entry.Add(new XElement("mapping",
                        new XElement("issuerPolicy", mapping.issuerOid,
                            new XAttribute("name", mapping.issuerPolicyName)),
                        new XElement("subjectPolicy", mapping.subjectOid,
                            new XAttribute("name", mapping.subjectPolicyName))
                        )
                    );
            }            
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
            output.AppendFormat("\tPolicy Mappings:\n");
            foreach (PolicyMapping mapping in mappings)
            {
                output.AppendFormat("\t\t{0} ({1}) --> {2} ({3})\n", mapping.issuerPolicyName, mapping.issuerOid, mapping.subjectPolicyName, mapping.subjectOid);
            } 
            return output.AppendLine().ToString();
        }

        // encoder
        private void encode()
        {
            Asn1EncodableVector v = new Asn1EncodableVector();

            foreach (PolicyMapping mapping in mappings)
            {
                v.Add(
                    new DerSequence(
                        new DerObjectIdentifier(mapping.issuerOid),
                        new DerObjectIdentifier(mapping.subjectOid)));
            }

            base.encValue = new PolicyMappings(new DerSequence(v));
        }
    }
}