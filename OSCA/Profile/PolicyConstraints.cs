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

using System;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto;
using System.Text;

namespace OSCA.Profile
{
    /// <summary>
    /// Policy Constraints extension
    /// </summary>
    /// <remarks>
    /// ASN.1 description from RFC5280:
    /// <code>
    ///    id-ce-policyConstraints OBJECT IDENTIFIER ::=  { id-ce 36 }
    ///    
    ///    PolicyConstraints ::= SEQUENCE {
    ///        requireExplicitPolicy           [0] SkipCerts OPTIONAL,
    ///        inhibitPolicyMapping            [1] SkipCerts OPTIONAL }
    ///        
    ///    SkipCerts ::= INTEGER (0..MAX)
    /// </code>
    /// </remarks>
    public class policyConstraints : ProfileExtension
    {

        private int reqExplicitPol = -1;
        private int inhibPolMap = -1;

        /// <summary>
        /// Get/Set the constraint type;
        /// </summary>
        /// <remarks>Value is Skip Certs (-1 = disable)</remarks>
        public int RequireExplicitPolicy { get { return reqExplicitPol; } set { reqExplicitPol = value; } }

        /// <summary>
        /// Get/Set the constraint type;
        /// </summary>
        /// <remarks>Value is Skip Certs (-1 = disable)</remarks>
        public int InhibitPolicyMapping { get { return inhibPolMap; } set { inhibPolMap = value; } }

        /// <summary>
        /// DER encoded value of PolicyConstraints
        /// </summary>
        public Asn1Sequence PolicyConstraint { get { encode(); return (Asn1Sequence)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create PolicyConstraints extension
        /// </summary>
        public policyConstraints() : this(true) { }

        /// <summary>
        /// Create PolicyConstraints extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public policyConstraints(bool Critical)
        {
            base.oid = X509Extensions.PolicyConstraints;
            base.name = "PolicyConstraints";
            base.displayName = "Policy Constraints";           
            base.critical = Critical;
        }

        /// <summary>
        /// Create PolicyConstraints extension from an OSCA XML extension description
        /// </summary>
        /// <remarks>Sample OSCA XML description of the PolicyConstraints extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="Policy Constraints"&gt;PolicyConstraints&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///      &lt;requireExplicitPolicy&gt;0&lt;/requireExplicitPolicy&gt;
        ///      &lt;inhibitPolicyMaping&gt;1&lt;/inhibitPolicyMapping&gt;
        ///    &lt;/value&gt;
        ///  &lt;/Extension&gt;
        /// </code> 
        /// </remarks>
        /// <param name="xml">OSCA XML extension description</param>
        public policyConstraints(XElement xml) : base(xml)
        {
            base.oid = X509Extensions.PolicyConstraints;

            if (xmlValue.Element("requireExplicitPolicy") != null)
                reqExplicitPol = Convert.ToInt32(xmlValue.Element("requireExplicitPolicy").Value);

            if (xmlValue.Element("inhibitPolicyMapping") != null)
                inhibPolMap = Convert.ToInt32(xmlValue.Element("inhibitPolicyMapping").Value);
        }

        /// <summary>
        /// Create PolicyConstraints extension from an X509Extension
        /// </summary>
        /// <param name="Extension">X509Extension</param>
        public policyConstraints(X509Extension Extension) : base(Extension)
        {
            base.oid = X509Extensions.PolicyConstraints;
            base.name = "PolicyConstraints";
            base.displayName = "Policy Constraints";

            Asn1Sequence pc = (Asn1Sequence)X509Extension.ConvertValueToObject(Extension);
            foreach (Asn1TaggedObject o in pc)
            {
                if (o.TagNo == 0)
                    reqExplicitPol = (((DerInteger)o.GetObject()).Value).IntValue;
                if (o.TagNo == 1)
                    inhibPolMap = (((DerInteger)o.GetObject()).Value).IntValue;
            }
        }

        /// <summary>
        /// Provide an OSCA XML description of the extension
        /// </summary>
        /// <remarks>Sample OSCA XML description of the PolicyConstraints extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="Policy Constraints"&gt;PolicyConstraints&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///      &lt;requireExplicitPolicy&gt;0&lt;/requireExplicitPolicy&gt;
        ///      &lt;inhibitPolicyMaping&gt;1&lt;/inhibitPolicyMapping&gt;
        ///    &lt;/value&gt;
        ///  &lt;/Extension&gt;
        /// </code> 
        /// </remarks>
        /// <returns>OSCA XML extension description</returns>
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

            // Create the parameter list
            XElement entry = extension.Element("value");

            if (reqExplicitPol > -1)
                entry.Add(new XElement("requireExplicitPolicy", reqExplicitPol.ToString()));

            if (inhibPolMap > -1)
                entry.Add(new XElement("inhibitPolicyMapping", inhibPolMap.ToString()));

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
            if (reqExplicitPol > -1)
                output.AppendFormat("\t\tRequire Explicit Policy: Skip {0}\n", reqExplicitPol);
            if (inhibPolMap > -1)
                output.AppendFormat("\t\tInhibit Policy Mapping: Skip {0}\n", inhibPolMap);
            return output.AppendLine().ToString();
        }

        private void encode()
        {
            Asn1EncodableVector v = new Asn1EncodableVector();

			if (reqExplicitPol > -1)
			{
				v.Add(new DerTaggedObject(true, 0, new DerInteger(reqExplicitPol)));
			}

			if (inhibPolMap > -1)
			{
				v.Add(new DerTaggedObject(true, 1, new DerInteger(inhibPolMap)));
			}

            base.encValue = new DerSequence(v);
        }
    }
}
