/*
 * Copyright 2014 Peter Curran (peter@currans.eu). All rights reserved.
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
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using OSCA.Offline;

namespace OSCA.Profile
{
    /// <summary>
    /// Method used for creating Key Identifier
    /// </summary>
    public enum KeyIdentifierMethod
    {
        /// <summary>
        /// Sha-1 hash
        /// </summary>
        Full,
        /// <summary>
        /// Truncated Sha-1 hash
        /// </summary>
        Truncated
    }

    /// <summary>
    /// Abstract base class for Key Identifier extensions
    /// </summary>
    public abstract class KeyIdentifier : ProfileExtension
    {
        /// <summary>
        /// Key identifier common to both SKID and AKID
        /// </summary>
        protected byte[] keyIdentifier;
        /// <summary>
        /// The method
        /// </summary>
        protected KeyIdentifierMethod method;

        /// <summary>
        /// The encoded value
        /// </summary>
        protected Asn1Sequence encodedValue;

        public string Value { get { return value(); } }

        /// <summary>
        /// Create extension
        /// </summary>
        protected KeyIdentifier() : this(false) { }

        /// <summary>
        /// Create extension using supplied values
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        protected KeyIdentifier(bool Critical)
        {
            base.critical = Critical;
        }

        /// <summary>
        /// Create extension from XML profile file entry
        /// </summary>
        /// <param name="xml">XML version of the extension</param>
        protected KeyIdentifier(XElement xml)
            : base(xml)
        {
            switch (((XElement)xmlValue).Element("keyID").Attribute("method").Value)
            {
                case "Full":
                    method = KeyIdentifierMethod.Full;
                    break;
                case "Truncated":
                    method = KeyIdentifierMethod.Truncated;
                    break;
                default:
                    throw new ArgumentException("Unknown KeyID method");
            }
            keyIdentifier = Utility.StringToUTF8ByteArray(((XElement)xmlValue).Element("keyID").Value);
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <returns>XML encoding of the extension</returns>
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

            // Create the parameter list
            XElement entry = extension.Element("value");
            entry.Add(new XElement("KeyID", Utility.UTF8ByteArrayToString(keyIdentifier),
                new XAttribute("method", method.ToString()))
                );
            return extension;
        }

        /// <summary>
        /// Determines whether [is the same] [the specified keyto test].
        /// </summary>
        /// <param name="KeytoTest">The keyto test.</param>
        /// <returns></returns>
        public bool IsTheSame(SubjectPublicKeyInfo KeytoTest)
        {
            SubjectKeyIdentifier key = SubjectKeyIdentifier.CreateSha1KeyIdentifier(KeytoTest);
            if (keyIdentifier.SequenceEqual(key.GetKeyIdentifier()))
                return true;
            return false;
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
            output.Append("\tKey Identifier: ");
            foreach (byte val in keyIdentifier)
                output.AppendFormat("{0} ", val.ToString("X"));
            output.Append("\n");
            return output.AppendLine().ToString();
        }

        private string value()
        {
            StringBuilder output = new StringBuilder();
            foreach (byte val in keyIdentifier)
                output.AppendFormat("{0} ", val.ToString("X"));
            return output.ToString();
        }
    }
}

