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
using System.Xml.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto;
using OSCA.Log;
using System.IO;
using Org.BouncyCastle.Security;


namespace OSCA.Offline
{

    /// <summary>
    /// A basic implementation of a Diffie-Hellman Trust Anchor using System crypto and X.509 V1 certificates
    /// </summary>
    /// <remarks>
    /// Extends the generic System Crypto CA class (<see cref="sysCA" />)
    /// <para>This is a Version 3.x class</para>
    /// </remarks>
    public class dhTA : sysCA
    {
        /// <summary>
        /// Create an instance of a simple DH TA
        /// </summary>
        /// <param name="ConfigFile">Full pathname to config file</param>
        public dhTA(string ConfigFile) : base(ConfigFile)
        {
            // Make sure the CA_Type is correct
            if (caType != CA_Type.dhTA)
                throw new InvalidParameterException("Invalid CA Type value for this CA instance");
        }

        /// <summary>
        /// Return a certificate generator instance for this CA
        /// </summary>
        /// <returns>Certificate generator</returns>
        /// <remarks>This CA only supports V1 certificates</remarks>
        protected override ICertGen getCertificateGenerator()
        {
            return new SysV1CertGen();
        }

        /// <summary>
        /// Generate a certificate
        /// </summary>
        /// <param name="gen">Certificate generator instance</param>
        /// <param name="ext">Extensions to include in the certificate</param>
        /// <returns>
        /// New certificate
        /// </returns>
        /// <exception cref="System.NotSupportedException">This CA does not support X.509v3 extensions</exception>
        /// <remarks>
        /// Always throws an exception - this CA uses V1 certificates
        /// </remarks>
        protected override X509Certificate generate(ICertGen gen, X509Extensions ext)
        {
            throw new NotSupportedException("This CA does not support X.509v3 extensions");
        }
    }
}
