/*
 * Copyright 2011-2012 Peter Curran (peter@currans.eu). All rights reserved.
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
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;


namespace OSCA.Crypto.X509
{
    /*
        
   id-ce-certificatePolicies OBJECT IDENTIFIER ::=  { id-ce 32 }

   anyPolicy OBJECT IDENTIFIER ::= { id-ce-certificate-policies 0 }

   certificatePolicies ::= SEQUENCE SIZE (1..MAX) OF PolicyInformation

   PolicyInformation ::= SEQUENCE {
        policyIdentifier   CertPolicyId,
        policyQualifiers   SEQUENCE SIZE (1..MAX) OF
                                PolicyQualifierInfo OPTIONAL }

   CertPolicyId ::= OBJECT IDENTIFIER

   PolicyQualifierInfo ::= SEQUENCE {
        policyQualifierId  PolicyQualifierId,
        qualifier          ANY DEFINED BY policyQualifierId }

   -- policyQualifierIds for Internet policy qualifiers

   id-qt          OBJECT IDENTIFIER ::=  { id-pkix 2 }
   id-qt-cps      OBJECT IDENTIFIER ::=  { id-qt 1 }
   id-qt-unotice  OBJECT IDENTIFIER ::=  { id-qt 2 }

   PolicyQualifierId ::=
        OBJECT IDENTIFIER ( id-qt-cps | id-qt-unotice )

   Qualifier ::= CHOICE {
        cPSuri           CPSuri,
        userNotice       UserNotice }

   CPSuri ::= IA5String

   UserNotice ::= SEQUENCE {
        noticeRef        NoticeReference OPTIONAL,
        explicitText     DisplayText OPTIONAL}

   NoticeReference ::= SEQUENCE {
        organization     DisplayText,
        noticeNumbers    SEQUENCE OF INTEGER }

   DisplayText ::= CHOICE {
        ia5String        IA5String      (SIZE (1..200)),
        visibleString    VisibleString  (SIZE (1..200)),
        bmpString        BMPString      (SIZE (1..200)),
        utf8String       UTF8String     (SIZE (1..200)) }
     */

    /// <summary>
    /// X.509 certificatePolicies extension
    /// </summary>
    public class CertificatePolicies : Asn1Encodable
    {

        /// <summary>
        /// Any policy OID
        /// </summary>
        public static DerObjectIdentifier anyPolicy = new DerObjectIdentifier("2.5.29.32.0");

        private List<PolicyInformation> policies = new List<PolicyInformation>();

        /// <summary>
        /// Get the instance of the certificate policies
        /// </summary>
        /// <param name="obj">ASN.1 data</param>
        /// <param name="explicitly">Explicit encoding</param>
        /// <returns>CertificatePolicies instance</returns>
        public static CertificatePolicies GetInstance(Asn1TaggedObject obj, bool explicitly)
        {
            return GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
        }

        /// <summary>
        /// Get the instance of the certificate policies
        /// </summary>
        /// <param name="obj">CertificatePolicies instance</param>
        /// <returns>An instance of the CertificatePolicies class</returns>
        /// <exception cref="System.ArgumentException">Unknown object in factory</exception>
        public static CertificatePolicies GetInstance(Object obj)
        {
            if (obj is CertificatePolicies)
                return (CertificatePolicies)obj;

            if (obj is Asn1Sequence)
                return new CertificatePolicies((Asn1Sequence)obj);

            if (obj is X509Extension)
                return GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));

            throw new ArgumentException("Unknown object in factory");
        }

        /// <summary>
        /// Add a policy to the list
        /// </summary>
        /// <param name="seq">Policy information</param>
        public CertificatePolicies(Asn1Sequence seq)
        {
            foreach (var pi in seq)
            {
                Asn1Sequence s = Asn1Sequence.GetInstance(pi);
                policies.Add(PolicyInformation.GetInstance(s));
            }
        }

        /// <summary>
        /// Add a policy to the list
        /// </summary>
        /// <param name="p">Policy information</param>
        public CertificatePolicies(PolicyInformation[] p)
        {
            foreach (PolicyInformation pi in p)
                policies.Add(pi);
        }


        /// <summary>
        /// Add a policy to the list
        /// </summary>
        /// <param name="p">Policy information</param>
        public CertificatePolicies(DerObjectIdentifier p)
        {
            policies.Add(new PolicyInformation(p));
        }


        /// <summary>
        /// Add a plicy to the list
        /// </summary>
        /// <param name="p">Policy information</param>
        public CertificatePolicies(String p)
        {
            policies.Add(new PolicyInformation(new DerObjectIdentifier(p)));
        }

        /// <summary>
        /// Add a policy to the list
        /// </summary>
        /// <param name="p">Policy information</param>
        public void AddPolicy(String p)
        {
            policies.Add(new PolicyInformation(new DerObjectIdentifier(p)));
        }

        /// <summary>
        /// Add a policy to the list
        /// </summary>
        /// <param name="p">Policy information</param>
        public void AddPolicy(PolicyInformation p)
        {
            policies.Add(p);
        }

        /// <summary>
        /// Get the policy information
        /// </summary>
        /// <returns>Array of PolicyInformation</returns>
        public PolicyInformation[] GetPolicyInformation()
        {
            PolicyInformation[] pi = new PolicyInformation[policies.Count];

            for (int i = 0; i < policies.Count; ++i)
            {
                pi[i] = policies[i];
            }
            return pi;
        }

        /// <summary>
        /// Get policy number nr
        /// </summary>
        /// <param name="nr">Index number</param>
        /// <returns></returns>
        public string GetPolicy(int nr)
        {
            if (policies.Count > nr + 1)
            {
                return (policies[nr].PolicyIdentifier.ToString());
            }
            return null;
        }

        /// <summary>
        /// certificatePolicies as an Asn1Object
        /// </summary>
        /// <returns>Asn1Object representation</returns>
        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector();

            foreach (PolicyInformation polInfo in policies)
                v.Add(polInfo);
 
            return new DerSequence(v);
        }

        /// <summary>
        /// certificatePolicies information as a string
        /// </summary>
        /// <returns>String containing certificate policies</returns>
        public override string ToString()
        {
            String p = null;
            for (int i = 0; i < policies.Count; i++)
            {
                if (p != null)
                {
                    p += ", ";
                }
                p += policies[i].PolicyIdentifier.ToString();
            }
            return "CertificatePolicies: " + p;
        }
    }
}
