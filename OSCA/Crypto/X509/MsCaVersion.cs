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
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Crypto.X509
{
    /// <summary>
    /// Microsoft private CA Version extension
    /// </summary>
    /// <remarks>
    /// This refers to the version of the key used by the issuing CA, and is presumably used to differentiate between CRLs.
    /// <para>
    /// Not a veru complicated extension, just contains a single Int:
    /// <code>
    /// The MsCaVersion object
    /// 
    /// MsCaVersionSyntax  ::=
    ///       INTEGER (0..MAX) 
    /// </code>
    /// </para>
    /// </remarks>
    public class MsCaVersion : Asn1Encodable
    {
        private DerInteger version;

        /// <summary>
        /// Microsoft private CA Version extension
        /// </summary>
        public static readonly DerObjectIdentifier CaVersion = new DerObjectIdentifier("1.3.6.4.1.311.21.1");

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version {
            get { return version.Value.IntValue; }
            set { version = new DerInteger(value); }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Unknown object in factory</exception>
        public static MsCaVersion GetInstance(object obj)
        {
            if (obj is MsCaVersion || obj == null)
                return (MsCaVersion)obj;

            if (obj is DerInteger)
                return new MsCaVersion((DerInteger)obj);

            if (obj is X509Extension)
                return GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));

            throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsCaVersion"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        private MsCaVersion(DerInteger version)
        {
            this.version = DerInteger.GetInstance(version);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsCaVersion"/> class.
        /// </summary>
        /// <param name="Version">CA version.</param>
        public MsCaVersion(int Version)
        {
            this.Version = Version;
        }

        /// <summary>
        /// Output asn1 object.
        /// </summary>
        /// <returns></returns>
        public override Asn1Object ToAsn1Object()
        {
            return new DerInteger(Version);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "CA Version (MS): " + Version.ToString();
        }
    }
}
