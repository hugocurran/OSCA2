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
using System.Text;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Profile
{
    /// <summary>
    /// Unknown extension
    /// </summary>
    /// <remarks>   
    /// This class is the default (or unknown) extension
    /// <para>
    /// An instance of this class will be created by the <see cref="ProfileExtensionFactory"/> if an unknown extension is submitted
    /// </para>
    /// </remarks>
    public class unknownExtension : ProfileExtension
    {
        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { return base.encValue; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="unknownExtension"/> class.
        /// </summary>
        /// <remarks>Always throws an exception. <para>You may only create this type of extension by passing in an instance of X509Extension.</para></remarks>
        /// <exception cref="System.ApplicationException">Invalid operation: Create "Unknown" Extension</exception>
        public unknownExtension() : this(false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="unknownExtension" /> class.
        /// </summary>
        /// <remarks>Always throws an exception. <para>You may only create this type of extension by passing in an instance of X509Extension with an OID.</para></remarks>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        /// <exception cref="System.ApplicationException">Invalid operation: Create "Unknown" Extension</exception>
        public unknownExtension(bool Critical)
        {
            throw new ApplicationException("Invalid operation: Create \"Unknown\" Extension");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="unknownExtension"/> class.
        /// </summary>
        /// <param name="OID">The oid.</param>
        /// <param name="Extension">The extension.</param>
        public unknownExtension(DerObjectIdentifier OID, X509Extension Extension)
        {
            base.oid = OID;
            base.name = "Unknown";
            base.displayName = "Unknown";
            base.encValue = X509Extension.ConvertValueToObject(Extension);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="unknownExtension"/> class.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <exception cref="System.ApplicationException">Invalid operation: Create "Unknown" Extension from XML description</exception>
        public unknownExtension(XElement xml)
        {
            throw new ApplicationException("Invalid operation: Create \"Unknown\" Extension from XML description");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="unknownExtension"/> class.
        /// </summary>
        /// <remarks>Always throws an exception. <para>You may only create this type of extension by passing in an instance of X509Extension with an OID.</para></remarks>
        /// <exception cref="System.ApplicationException">Invalid operation: Create "Unknown" Extension</exception>
        /// <param name="Extension">X509Extension instance</param>
        public unknownExtension(X509Extension Extension)
            : base(Extension)
        {
            throw new ApplicationException("Invalid operation: Create \"Unknown\" Extension");
        }

        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <returns>XML encoding of the extension</returns>
        /// <exception cref="ApplicationException">Invalid operation: Create XML description from "Unknown" Extension</exception>
        public override XNode ToXml()
        {
            throw new ApplicationException("Invalid operation: Create XML description from \"Unknown\" Extension");
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
            output.AppendFormat("\tUnknown Extension:\n");
            output.AppendFormat("\t\t{0}\n", encValue);
            return output.AppendLine().ToString();
        }

    }
}
