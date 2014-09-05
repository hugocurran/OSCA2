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
using OSCA.Crypto.X509;
using System.Text;

namespace OSCA.Profile
{
    /// <summary>
    /// Certificate policy description
    /// </summary>
    public struct CertPolicy
    {
        /// <summary>
        /// Policy OID
        /// </summary>
        public string Oid;
        /// <summary>
        /// URI of the CPS
        /// </summary>
        public string Cps;
        /// <summary>
        /// User notice
        /// </summary>
        public string Unotice;
        /// <summary>
        /// Policy name
        /// </summary>
        public string Name;
    }

    /// <summary>
    /// Certificate Policies extension
    /// </summary>
    /// <remarks>
    /// ASN.1 description of extension from RFC 5280:
    /// <code>
    /// id-ce-certificatePolicies OBJECT IDENTIFIER ::=  { id-ce 32 }
    /// 
    /// anyPolicy OBJECT IDENTIFIER ::= { id-ce-certificate-policies 0 }
    /// 
    /// certificatePolicies ::= SEQUENCE SIZE (1..MAX) OF PolicyInformation
    /// 
    /// PolicyInformation ::= SEQUENCE {
    ///     policyIdentifier   CertPolicyId,
    ///     policyQualifiers   SEQUENCE SIZE (1..MAX) OF
    ///     PolicyQualifierInfo OPTIONAL }
    ///     
    /// CertPolicyId ::= OBJECT IDENTIFIER
    ///
    /// PolicyQualifierInfo ::= SEQUENCE {
    ///     policyQualifierId  PolicyQualifierId,
    ///     qualifier          ANY DEFINED BY policyQualifierId }
    ///     
    /// -- policyQualifierIds for Internet policy qualifiers
    /// 
    /// id-qt          OBJECT IDENTIFIER ::=  { id-pkix 2 }
    /// id-qt-cps      OBJECT IDENTIFIER ::=  { id-qt 1 }
    /// id-qt-unotice  OBJECT IDENTIFIER ::=  { id-qt 2 }
    /// 
    /// PolicyQualifierId ::=
    ///     OBJECT IDENTIFIER ( id-qt-cps | id-qt-unotice )
    ///     
    /// Qualifier ::= CHOICE {
    ///     cPSuri           CPSuri,
    ///     userNotice       UserNotice }
    ///     
    /// CPSuri ::= IA5String
    ///
    /// UserNotice ::= SEQUENCE {
    ///     noticeRef        NoticeReference OPTIONAL,
    ///     explicitText     DisplayText OPTIONAL}
    ///     
    /// NoticeReference ::= SEQUENCE {
    ///     organization     DisplayText,
    ///     noticeNumbers    SEQUENCE OF INTEGER }
    ///     
    /// DisplayText ::= CHOICE {
    ///     ia5String        IA5String      (SIZE (1..200)),
    ///     visibleString    VisibleString  (SIZE (1..200)),
    ///     bmpString        BMPString      (SIZE (1..200)),
    ///     utf8String       UTF8String     (SIZE (1..200)) }
    ///</code>
    /// </remarks>
    public class certificatePolicies : ProfileExtension
    {
        // Local structure to hold the list of names
        // Note that base.encValue holds the DER encoded version
        private List<CertPolicy> policies = new List<CertPolicy>();

        /// <summary>
        /// List of certificate policies
        /// </summary>
        public List<CertPolicy> CertPolicies { get { return policies; } }

        /// <summary>
        /// DER encoded value of CertificatePolicies
        /// </summary>
        public CertificatePolicies CertificatePolicies { get { encode(); return (CertificatePolicies)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create CertificatePolicies extension
        /// </summary>
        public certificatePolicies() : this(false) { }

        /// <summary>
        /// Create CertificatePolicies extension using supplied values
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public certificatePolicies(bool Critical)
        {
            base.oid = X509Extensions.CertificatePolicies;
            base.name = "CertificatePolicies";
            base.displayName = "Certificate Policies";
            base.critical = Critical;
        }

        /// <summary>
        /// Create CertificatePolicies extension from XML profile file entry
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the CertificatePolicies extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Certificate Policies"&gt;CertificatePolicies&lt;/name&gt;
        ///     &lt;critical&gt;false&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;policyInfo&gt;
        ///             &lt;oid&gt;1.2.3.4&lt;/oid&gt;
        ///             &lt;cps&gt;http://foo.bar.com/cps.html&lt;/cps&gt;
        ///             --&gt; &lt;unotice&gt;&lt;/unotice&gt;
        ///             &lt;name&gt;MediumAssurance&lt;/name&gt;
        ///         &lt;/policyInfo&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        /// <param name="xml">XML version of the extension</param>
        public certificatePolicies(XElement xml) : base(xml)
        {
            base.oid = X509Extensions.CertificatePolicies;

            foreach (XElement polID in xmlValue.Descendants("policyInfo"))
            {
                CertPolicy pol = new CertPolicy() {
                    Oid = polID.Element("oid").Value,
                    Cps = polID.Element("cps").Value,
                    Unotice = polID.Element("unotice").Value,
                    Name = polID.Element("name").Value
                };
                policies.Add(pol);
            }
        }

        /// <summary>
        /// Create CertificatePolicies extension from an X509Extension
        /// </summary>
        /// <param name="Extension">X509 extension</param>
        public certificatePolicies(X509Extension Extension) : base(Extension)
        {
            base.oid = X509Extensions.CertificatePolicies;
            base.name = "CertificatePolicies";
            base.displayName = "Certificate Policies";

            Crypto.X509.CertificatePolicies certPol = Crypto.X509.CertificatePolicies.GetInstance(Extension);
            PolicyInformation[] piList = certPol.GetPolicyInformation();
            foreach (PolicyInformation pi in piList)
            {
                CertPolicy cp = new CertPolicy() { Oid = pi.PolicyIdentifier.ToString() };              
                Asn1Sequence quals = pi.PolicyQualifiers;
                if (quals != null)
                {
                    foreach (DerSequence qual in quals)
                    {
                        PolicyQualifierInfo qi = PolicyQualifierInfo.GetInstance(qual);
                        if (qi.GetPolicyQualifierId().Equals(PolicyQualifierID.IdQtCps))
                            cp.Cps = qi.GetQualifier();
                        if (qi.GetPolicyQualifierId().Equals(PolicyQualifierID.IdQtUnotice))
                            cp.Unotice = qi.GetQualifier();
                    }
                }
                policies.Add(cp);                    
            }
        }

        /// <summary>
        /// Add a new CertPolicy to the extension
        /// </summary>
        /// <param name="Policy">Certificate policy</param>
        public void Add(CertPolicy Policy)
        {
            policies.Add(Policy);
        }

        /// <summary>
        /// Remove a CertPolicy from the extension
        /// </summary>
        /// <param name="Policy">Certificate policy</param>
        public void Remove(CertPolicy Policy)
        {
            policies.Remove(Policy);
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the CertificatePolicies extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Certificate Policies"&gt;CertificatePolicies&lt;/name&gt;
        ///     &lt;critical&gt;false&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;policyInfo&gt;
        ///             &lt;oid&gt;1.2.3.4&lt;/oid&gt;
        ///             &lt;cps&gt;http://foo.bar.com/cps.html&lt;/cps&gt;
        ///             --&gt; &lt;unotice&gt;&lt;/unotice&gt;
        ///             &lt;name&gt;MediumAssurance&lt;/name&gt;
        ///         &lt;/policyInfo&gt;
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
            foreach (CertPolicy pol in policies)
            {
                entry.Add(new XElement("policyInfo",
                        new XElement("oid", pol.Oid),
                        new XElement("cps", pol.Cps),
                        new XElement("unotice", pol.Unotice),
                        new XElement("name", pol.Name)
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
            foreach (CertPolicy pol in policies)
            {
                output.AppendFormat("\t{0} ({1})\n", pol.Oid, pol.Name );
                if (pol.Cps != "")
                    output.AppendFormat("\t\tCPS: {0}\n",pol.Cps);
                if (pol.Unotice != "")
                    output.AppendFormat("\t\tNotice: {0}\n", pol.Unotice);
            }  
            return output.AppendLine().ToString();
        }

        // encoder (NOTE - only supports simple notices)
        private void encode()
        {
            PolicyInformation[] polinfo = new PolicyInformation[policies.Count];

            for (int i = 0; i < policies.Count; i++)
            {
                if ((policies[i].Cps != "") || (policies[i].Unotice != ""))
                {
                    Asn1EncodableVector vq = new Asn1EncodableVector();
                    if (policies[i].Cps != "")                                  // cPSuri Qualifier
                        vq.Add(new PolicyQualifierInfo(policies[i].Cps));  

                    if (policies[i].Unotice != "")                              //Unotice Qualifier
                    {
                        Asn1EncodableVector vn = new Asn1EncodableVector(new DerIA5String(policies[i].Unotice));
                        vq.Add(new PolicyQualifierInfo(PolicyQualifierID.IdQtUnotice, new DerSequence(vn)));
                    }

                    polinfo[i] = (new PolicyInformation(new DerObjectIdentifier(policies[i].Oid), new DerSequence(vq)));
                }
                else
                {
                    polinfo[i] = (new PolicyInformation(new DerObjectIdentifier(policies[i].Oid)));
                }
            }
            base.encValue = new CertificatePolicies(polinfo);
        }
    }
}
