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
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;


namespace OSCA.Crypto.X509
{
    /*
   A CA may specify that an OCSP client can trust a responder for the
   lifetime of the responder's certificate. The CA does so by including
   the extension id-pkix-ocsp-nocheck. This SHOULD be a non-critical
   extension. The value of the extension should be NULL. CAs issuing
   such a certificate should realized that a compromise of the
   responder's key, is as serious as the compromise of a CA key used to
   sign CRLs, at least for the validity period of this certificate. CA's
   may choose to issue this type of certificate with a very short
   lifetime and renew it frequently.

   id-pkix-ocsp-nocheck OBJECT IDENTIFIER ::= { id-pkix-ocsp 5 }

     */

    /// <summary>
    /// Class that defines the OCSP NoCheck extension (per RFC2560)
    /// </summary>
    public class OcspNocheck : Asn1Encodable
    {

        /// <summary>
        /// OCSP NoCheck OID {id-pkix-ocsp 5 }
        /// </summary>
        public static DerObjectIdentifier ocspNocheck = new DerObjectIdentifier("1.3.6.1.5.5.7.48.1.5");

        /// <summary>
        /// Return an instance of the OcspNoCheck class
        /// </summary>
        /// <param name="obj">An object</param>
        /// <returns>OcspNoCheck instance</returns>
        /// <exception cref="ArgumentException">Unknown object in factory (not an instance of OcspNoCheck)</exception>
        public static OcspNocheck GetInstance(Object obj)
        {
            if (obj is OcspNocheck)
                return (OcspNocheck)obj;

            if (obj is DerNull)
                return new OcspNocheck();

            if (obj is X509Extension)
                return GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));

            throw new ArgumentException("Unknown object in factory");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OcspNocheck"/> class.
        /// </summary>
        public OcspNocheck()
        {

        }

        /// <summary>
        /// Convert to an Asn1Object.
        /// </summary>
        /// <returns>An Asn1Object</returns>
        public override Asn1Object ToAsn1Object()
        {
            return DerNull.Instance;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "OcspNocheck: ";
        }
    }
}

