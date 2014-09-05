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
using System.Text;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Profile
{
    /// <summary>
    /// Extended Key Usage extension
    /// </summary>
    /// <remarks>ASN.1 description from RFC 5280:
    /// <code>
    /// id-ce-extKeyUsage OBJECT IDENTIFIER ::= { id-ce 37 }
    ///  
    /// ExtKeyUsageSyntax ::= SEQUENCE SIZE (1..MAX) OF KeyPurposeId
    /// 
    /// KeyPurposeId ::= OBJECT IDENTIFIER
    /// 
    /// anyExtendedKeyUsage OBJECT IDENTIFIER ::= { id-ce-extKeyUsage 0 }
    /// 
    /// id-kp OBJECT IDENTIFIER ::= { id-pkix 3 }
    /// 
    /// id-kp-serverAuth             OBJECT IDENTIFIER ::= { id-kp 1 }
    /// id-kp-clientAuth             OBJECT IDENTIFIER ::= { id-kp 2 }
    /// id-kp-codeSigning             OBJECT IDENTIFIER ::= { id-kp 3 }
    /// id-kp-emailProtection         OBJECT IDENTIFIER ::= { id-kp 4 }
    /// id-kp-timeStamping            OBJECT IDENTIFIER ::= { id-kp 8 }
    /// id-kp-OCSPSigning            OBJECT IDENTIFIER ::= { id-kp 9 }
    /// </code>
    /// In addition, a number of private and/or legacy EKU values are supported:
    /// <code>
    /// id-kp-ipsecEndSystem        OBJECT IDENTIFIER ::= { id-kp 5 }
    /// id-kp-ipsecTunnel           OBJECT IDENTIFIER ::= { id-kp 6 }
    /// id-kp-ipsecUser             OBJECT IDENTIFIER ::= { id-kp 7 }
    /// --> Microsoft
    /// id-kp-MsSmartCardLogon        OBJECT IDENTIFIER ::= { 1 3 6 1 4 1 311 20 2 2 }
    /// id-KP-MsCodeSigningCom        OBJECT IDENTIFIER ::= { 1 3 6 1 4 1 311 2 1 22 }
    /// id-KP-MsCodeSigningInd        OBJECT IDENTIFIER ::= { 1 3 6 1 4 1 311 2 1 21 }
    /// id-KP-MsDocumentSigning       OBJECT IDENTIFIER ::= { 1 3 6 1 4 1 311 10 3 12 }
    /// id-KP-MsEFSCrypto             OBJECT IDENTIFIER ::= { 1 3 6 1 4 1 311 10 3 4 }
    /// id-KP-MsEFSRecovery           OBJECT IDENTIFIER ::= { 1 3 6 1 4 1 311 10 3 4 1 }
    /// id-KP-MsCertReqstAgent        OBJECT IDENTIFIER ::= { 1 3 6 1 4 1 311 20 2 1 }
    /// </code>
    /// </remarks>
    public class extendedKeyUsage : ProfileExtension
    {
        private List<string> usage = new List<string>();

        private static Dictionary<string, string> ekuNames = new Dictionary<string,string>(){ 
                                        {KeyPurposeID.AnyExtendedKeyUsage.ToString(), "AnyExtendedKeyUsage"},
                                        {KeyPurposeID.IdKPServerAuth.ToString(), "ServerAuth"},
                                        {KeyPurposeID.IdKPClientAuth.ToString(), "ClientAuth"},
                                        {KeyPurposeID.IdKPCodeSigning.ToString(), "CodeSigning"},
                                        {KeyPurposeID.IdKPEmailProtection.ToString(), "EmailProtection"},
                                        {KeyPurposeID.IdKPIpsecEndSystem.ToString(), "IpsecEndSystem"},
                                        {KeyPurposeID.IdKPIpsecTunnel.ToString(), "IpsecTunnel"},
                                        {KeyPurposeID.IdKPIpsecUser.ToString(), "IpsecUser"},
                                        {KeyPurposeID.IdKPTimeStamping.ToString(), "TimeStamping"},
                                        {KeyPurposeID.IdKPOcspSigning.ToString(), "OcspSigning"},
                                        {KeyPurposeID.IdKPMsSmartCardLogon.ToString(),"SmartCardLogon"},
                                        {KeyPurposeID.IdKPMsCodeSigningCom.ToString(), "CodeSigningCommercial"},
                                        {KeyPurposeID.IdKPMsCodeSigningInd.ToString(), "CodeSigningIndividual"},
                                        {KeyPurposeID.IdKPMsDocumentSigning.ToString(), "DocumentSigning"},
                                        {KeyPurposeID.IdKPMsEFSCrypto.ToString(), "EFSCrypto"},
                                        {KeyPurposeID.IdKPMsEFSRecovery.ToString(), "EFSRecovery"},
                                        {KeyPurposeID.IdKPMsCertReqstAgent.ToString(), "CertRequestAgent"}
        };
        private static Dictionary<string, string> eKuOIDs = new Dictionary<string, string>(){
                                        {"AnyExtendedKeyUsage", KeyPurposeID.AnyExtendedKeyUsage.ToString()},
                                        {"ServerAuth", KeyPurposeID.IdKPServerAuth.ToString()},
                                        {"ClientAuth", KeyPurposeID.IdKPClientAuth.ToString()},
                                        {"CodeSigning", KeyPurposeID.IdKPCodeSigning.ToString()},
                                        {"EmailProtection", KeyPurposeID.IdKPEmailProtection.ToString()},
                                        {"IpsecEndSystem", KeyPurposeID.IdKPIpsecEndSystem.ToString()},
                                        {"IpsecTunnel", KeyPurposeID.IdKPIpsecTunnel.ToString()},
                                        {"IpsecUser", KeyPurposeID.IdKPIpsecUser.ToString()},
                                        {"TimeStamping", KeyPurposeID.IdKPTimeStamping.ToString()},
                                        {"OcspSigning", KeyPurposeID.IdKPOcspSigning.ToString()},
                                        {"SmartCardLogon", KeyPurposeID.IdKPMsSmartCardLogon.ToString()},
                                        {"CodeSiginingCommercial", KeyPurposeID.IdKPMsCodeSigningCom.ToString()},
                                        {"CodeSigningIndividual", KeyPurposeID.IdKPMsCodeSigningInd.ToString()},
                                        {"DocumentSigning", KeyPurposeID.IdKPMsDocumentSigning.ToString()},
                                        {"EFSCrypto", KeyPurposeID.IdKPMsEFSCrypto.ToString()},
                                        {"EFSRecovery", KeyPurposeID.IdKPMsEFSRecovery.ToString()},
                                        {"CertRequestAgent", KeyPurposeID.IdKPMsCertReqstAgent.ToString()}
        };

        /// <summary>
        /// Lookup the OID for an EKU name
        /// </summary>
        /// <param name="EkuName"></param>
        /// <returns></returns>
        public static string LookupOID(string EkuName)
        {
            return eKuOIDs[EkuName];
        }
                        
        /// <summary>
        /// Textual list of possible ExtendedKeyUsages
        /// </summary>
        public static string[] ExtKeyUsageText { get { return ekuNames.Values.ToArray(); } }

        /// <summary>
        /// OID list of possible ExtendedKeyUsages
        /// </summary>
        public static string[] ExtKeyUsageOid { get { return ekuNames.Keys.ToArray(); } }

        /// <summary>
        /// ExtendedKeyUsage settings
        /// </summary>
        public List<string> ExtKUsage { get { return usage; } }

        /// <summary>
        /// DER encoded value of KeyUsage
        /// </summary>
        public ExtendedKeyUsage ExtendedKeyUsage { get { encode(); return (ExtendedKeyUsage)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        public string Value { get { return value(); } }

        /// <summary>
        /// Create ExtendedKeyUsage extension
        /// </summary>
        public extendedKeyUsage() : this(false) {}

        /// <summary>
        /// Create ExtendedKeyUsage extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public extendedKeyUsage(bool Critical)
        {
            base.oid = X509Extensions.ExtendedKeyUsage;
            base.name = "ExtendedKeyUsage";
            base.displayName = "Extended Key Usage";
            base.critical = Critical;
        }

        /// <summary>
        /// Create ExtendedKeyUsage extension from XML profile file entry
        /// </summary>
        /// <remarks>Example XML description of the extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name&gt;ExtendedKeyUsage&lt;/name&gt;
        ///     &lt;critical&gt;false&lt;/critical&gt; 
        ///     &lt;value&gt;
        ///         &lt;eku&gt;ClientAuth&lt;/eku&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// </remarks>
        /// <param name="xml">XML version of the extension</param>
        public extendedKeyUsage(XElement xml) : base(xml)
        {
            base.oid = X509Extensions.ExtendedKeyUsage;
            foreach (XElement eku in xmlValue.Descendants("eku"))
                usage.Add(eku.Value);     
        }

        /// <summary>
        /// Create ExtendedKeyUsage from an X509Extension
        /// </summary>
        /// <param name="Extension">X509 extension</param>
        /// <remarks>
        /// Sub classses must provide an implementation to decode their values
        /// </remarks>
        public extendedKeyUsage(X509Extension Extension) : base(Extension)
        {            
            base.oid = X509Extensions.ExtendedKeyUsage;
            base.name = "ExtendedKeyUsage";
            base.displayName = "Extended Key Usage";

            ExtendedKeyUsage eku = ExtendedKeyUsage.GetInstance(Extension);
            // Lookup each OID in the extension and add to usage List by name
            foreach(var e in eku.GetAllUsages())
                usage.Add(ekuNames[e.ToString()]);
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <remarks>Example XML description of the extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name&gt;ExtendedKeyUsage&lt;/name&gt;
        ///     &lt;critical&gt;false&lt;/critical&gt; 
        ///     &lt;value&gt;
        ///         &lt;eku&gt;ClientAuth&lt;/eku&gt;
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
            XElement ekus = extension.Element("value");
            foreach (string eku in usage)
            {
                ekus.Add(new XElement("eku", eku));
            }
            return extension;
        }

        /// <summary>
        /// Add an ExtendedKeyUsage to the extension
        /// </summary>
        /// <param name="value">EKU to add</param>
        public void Add(string value)
        {
            usage.Add(value);
        }

        /// <summary>
        /// Remove an ExtendedKeyUsage from the extension
        /// </summary>
        /// <param name="value">EKU to remove</param>
        public void Remove(string value)
        {
            usage.Remove(value);
        }

        /// <summary>
        /// Lookup the OID for an Extended Key Usage
        /// </summary>
        /// <param name="eku">Extended Key Usage name</param>
        /// <returns>OID</returns>        
        public static KeyPurposeID LookUp(string eku)
        {
            KeyPurposeID oid;
            try
            {
                oid = new KeyPurposeID(eKuOIDs[eku]);
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("Unknown EKU: " + eku);
            }
            return oid;
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
            output.AppendFormat("\tExtended Key Usages:\n");
            foreach (string eku in usage)
                output.AppendFormat("\t\t{0}: {1}\n", eku, LookUp(eku));
            return output.AppendLine().ToString();
        }

        private string value()
        {
            StringBuilder output = new StringBuilder();
            //output.AppendFormat("Extended Key Usages:");
            foreach (string eku in usage)
                output.AppendFormat("{0}: {1}\n", eku, LookUp(eku));
            return output.ToString();
        }

        // Create the Asn1Encodable version
        private void encode()
        {
            KeyPurposeID[] kpUsages = new KeyPurposeID[usage.Count()];
            int i = 0;
            foreach (string eku in usage)
            {
                kpUsages[i] = LookUp(eku);
                i++;
            }
            base.encValue = new ExtendedKeyUsage(kpUsages);
        }
    }
}
