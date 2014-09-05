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
    /// Microsoft private Previous CA Certificate Hash extension
    /// </summary>
    /// <remarks>
    /// Contains the SHA-1 hash of the previous version of the CA certificate
    /// <para>
    /// Not a veru complicated extension, just contains an OctetString:
    /// <code>
    /// The MsPrevCaCertHash object
    /// 
    /// MsPrevCaCertHashSyntax  ::=
    ///       OCTETSTRING (0..MAX) 
    /// </code>
    /// </para>
    /// </remarks>
    public class MsPrevCaCertHash : Asn1Encodable
    {
        private Asn1OctetString hash;

        /// <summary>
        /// Microsoft private Previous CA Certificate Hash extension
        /// </summary>
        public static readonly DerObjectIdentifier PrevHash = new DerObjectIdentifier("1.3.6.1.4.1.311.21.2");

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>
        /// The hash.
        /// </value>
        public byte[] Hash
        {
            get { return hash.GetOctets(); }
            set { hash = (Asn1OctetString)Asn1Object.FromByteArray(value); }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Unknown object in factory</exception>
        public static MsPrevCaCertHash GetInstance(object obj)
        {
            if (obj is MsPrevCaCertHash || obj == null)
                return (MsPrevCaCertHash)obj;

            if (obj is DerOctetString)
                return new MsPrevCaCertHash((DerOctetString)obj);

            if (obj is X509Extension)
                return GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));

            throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsCaVersion" /> class.
        /// </summary>
        /// <param name="hash">The hash.</param>
        private MsPrevCaCertHash(Asn1OctetString hash)
        {
            this.hash = hash;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsCaVersion" /> class.
        /// </summary>
        /// <param name="Hash">The hash.</param>
        public MsPrevCaCertHash(byte[] Hash)
        {
            this.hash = (Asn1OctetString)Asn1Object.FromByteArray(Hash);
        }

        /// <summary>
        /// Output asn1 object.
        /// </summary>
        /// <returns></returns>
        public override Asn1Object ToAsn1Object()
        {
            return new DerOctetString(hash);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Previous CA Certificate Hash (MS): " + Hash.ToString();
        }
    }
}
