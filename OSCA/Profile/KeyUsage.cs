/*
 * Copyright 2011, 2012, 2013, 2014 Peter Curran (peter@currans.eu). All rights reserved.
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
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using System.Text;

namespace OSCA.Profile
{
    /// <summary>
    /// Key Usage extension
    /// </summary>
    /// <remarks>
    /// ASN.1 description from RFC 5280:
    /// <code>
    /// id-ce-keyUsage OBJECT IDENTIFIER ::=  { id-ce 15 }
    /// 
    /// KeyUsage ::= BIT STRING {
    ///     digitalSignature        (0),
    ///     nonRepudiation          (1),
    ///     keyEncipherment         (2),
    ///     dataEncipherment        (3),
    ///     keyAgreement            (4),
    ///     keyCertSign             (5),
    ///     cRLSign                 (6),
    ///     encipherOnly            (7),
    ///     decipherOnly            (8) }
    /// </code>
    /// </remarks>
    public class keyUsage : ProfileExtension
    {

        private List<string> usages = new List<string>();

        private static string[] values = {"DigitalSignature",
                                   "NonRepudiation",
                                   "DataEncipherment",
                                   "KeyEncipherment",
                                   "KeyAgreement",
                                   "KeyCertSign",
                                   "CRLSign",
                                   "EncipherOnly",
                                   "DecipherOnly"
                                  };

        [Flags]
        private enum kuf
        {
            DigitalSignature    = 0x80,
            NonRepudiation      = 0x40,
            DataEncipherment    = 0x20,
            KeyEncipherment     = 0x10,
            KeyAgreement        = 0x08,
            KeyCertSign         = 0x04,
            CRLSign             = 0x02,
            EncipherOnly        = 0x01,
            DecipherOnly        = 0x0800
        }

        /// <summary>
        /// Textual list of possible KeyUsages
        /// </summary>
        public static string[] KeyUsages { get { return values; } }

        /// <summary>
        /// KeyUsage settings
        /// </summary>
        public List<string> Kusage { get { return usages; } }

        /// <summary>
        /// DER encoded value of KeyUsage
        /// </summary>
        public KeyUsage KeyUsage { get { encode(); return (KeyUsage)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create KeyUsage extension
        /// </summary>
        public keyUsage() : this(false) { }

        /// <summary>
        /// Create KeyUsage extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public keyUsage(bool Critical)
        {
            base.oid = X509Extensions.KeyUsage;
            base.name = "KeyUsage";
            base.displayName = "Key Usage";
            base.critical = Critical;
        }

        /// <summary>
        /// Create KeyUsage extension from XML profile file entry
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the KeyUsage extension:
        /// </remarks>
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name displayName="Key Usage"&gt;KeyUsage&lt;/name&gt;
        ///     &lt;critical&gt;true&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;usage&gt;KeyCertSign&lt;/usage&gt;
        ///         &lt;usage&gt;CrlSign&lt;/usage&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// <param name="xml">XML version of the extension</param>
        public keyUsage(XElement xml) : base(xml)
        {
            base.oid = X509Extensions.KeyUsage;
            foreach (XElement usage in xmlValue.Descendants("usage"))
                usages.Add(usage.Value);
        }

        /// <summary>
        /// Create KeyUsage from an X509Extension
        /// </summary>
        /// <param name="Extension">X509 extension</param>
        /// <remarks>
        /// Sub classses must provide an implementation to decode their values
        /// </remarks>
        public keyUsage(X509Extension Extension) : base(Extension)
        {            
            base.oid = X509Extensions.ExtendedKeyUsage;
            base.name = "KeyUsage";
            base.displayName = "Key Usage";

            KeyUsage ku = KeyUsage.GetInstance(Extension);
            kuf val = (kuf)ku.IntValue;
            if ((val & kuf.DigitalSignature) == kuf.DigitalSignature)
                usages.Add("DigitalSignature");
            if ((val & kuf.NonRepudiation) == kuf.NonRepudiation)
                usages.Add("NonRepudiation");
            if ((val & kuf.DataEncipherment) == kuf.DataEncipherment)
                usages.Add("DataEncipherment");
            if ((val & kuf.KeyEncipherment) == kuf.KeyEncipherment)
                usages.Add("KeyEncipherment");
            if ((val & kuf.KeyAgreement) == kuf.KeyAgreement)
                usages.Add("KeyAgreement");
            if ((val & kuf.KeyCertSign) == kuf.KeyCertSign)
                usages.Add("KeyCertSign");
            if ((val & kuf.CRLSign) == kuf.CRLSign)
                usages.Add("CRLSign");
            if ((val & kuf.EncipherOnly) == kuf.EncipherOnly)
                usages.Add("EncipherOnly");
            if ((val & kuf.DecipherOnly) == kuf.DecipherOnly)
                usages.Add("DecipherOnly");           
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the KeyUsage extension:
        /// </remarks>
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name displayName="Key Usage"&gt;KeyUsage&lt;/name&gt;
        ///     &lt;critical&gt;true&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;usage&gt;KeyCertSign&lt;/usage&gt;
        ///         &lt;usage&gt;CrlSign&lt;/usage&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// <returns>XML encoding of the extension</returns>
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

            // Create the parameter list
            XElement entry = extension.Element("value");
            foreach (string usage in usages)
            {
                entry.Add(new XElement("usage", usage));
            }
            return extension;
        }

        /// <summary>
        /// Add a KeyUsage to the extension
        /// </summary>
        /// <param name="usage">Key Usage</param>
        public void Add(string usage)
        {
            usages.Add(usage);
        }

        /// <summary>
        /// Remove a KeyUsage from the extension
        /// </summary>
        /// <param name="usage">Key Usage</param>
        public void Remove(string usage)
        {
            usages.Remove(usage);
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
            output.AppendFormat("\tKey Usages (0x{0:X02})\n", keyUse());
            foreach (string usage in usages)
                output.AppendFormat("\t\t{0}\n", usage);
            return output.AppendLine().ToString();
        }

        private void encode()
        {
            base.encValue = new KeyUsage(keyUse());
        }

        private int keyUse()
        {
            int keyUsage = 0;
            foreach (string usage in usages)
            {
                switch (usage)
                {
                    case "DigitalSignature":
                        keyUsage = keyUsage | X509KeyUsage.DigitalSignature;
                        break;
                    case "NonRepudiation":
                        keyUsage = keyUsage | X509KeyUsage.NonRepudiation;
                        break;
                    case "KeyEncipherment":
                        keyUsage = keyUsage | X509KeyUsage.KeyEncipherment;
                        break;
                    case "DataEncipherment":
                        keyUsage = keyUsage | X509KeyUsage.DataEncipherment;
                        break;
                    case "KeyAgreement":
                        keyUsage = keyUsage | X509KeyUsage.KeyAgreement;
                        break;
                    case "KeyCertSign":
                        keyUsage = keyUsage | X509KeyUsage.KeyCertSign;
                        break;
                    case "CRLSign":
                        keyUsage = keyUsage | X509KeyUsage.CrlSign;
                        break;
                    case "EncipherOnly":
                        keyUsage = keyUsage | X509KeyUsage.EncipherOnly;
                        break;
                    case "DecipherOnly":
                        keyUsage = keyUsage | X509KeyUsage.DecipherOnly;
                        break;
                    default:
                        throw new ApplicationException("Unrecognised key usage in profile: " + usage);
                }
            }
            return keyUsage;
        }
    }
}




