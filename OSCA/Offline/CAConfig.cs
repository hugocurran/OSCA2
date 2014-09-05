using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;

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

namespace OSCA.Offline
{
    /// <summary>
    /// X.509 certificate version
    /// </summary>    
    public enum X509ver
    {
        /// <summary>
        /// X.509 Version 1
        /// </summary>
        V1 = 1,
        /// <summary>
        /// X.509 Version 3
        /// </summary>
        V3 = 3
    }

    /// <summary>
    /// Type of CA
    /// </summary>
    public enum CA_Type
    {
        // Legacy CA types (v2)
        /// <summary>
        /// OSCA Simple CA (BC crypto) (Version 2)
        /// </summary>
        simpleCA,
        /// <summary>
        /// OSCA FIPS CA (System crypto) (Version 2)
        /// </summary>
        fipsCA,

        // Current CA types (V3)
        /// <summary>
        /// Generic Bouncy Castle Crypto CA
        /// </summary>
        bcCA,

        /// <summary>
        /// Generic System Crypto CA
        /// </summary>
        sysCA,

        /// <summary>
        /// DH Trust Anchor (System crypto, V1 certificates)
        /// </summary>
        dhTA
    }

    /// <summary>
    /// Profile of CA
    /// </summary>
    public enum CA_Profile
    {
        /// <summary>
        /// Root CA
        /// </summary>
        rootCA,         // Legacy from V1 is that was RootCA - handled on a case-by-case basis
        /// <summary>
        /// Subordinate CA
        /// </summary>
        SubCA
    }

    /// <summary>
    /// Object describing the setup parameters for a CA
    /// </summary>
    public class CAConfig
    {
        /// <summary>
        ///  Friendly name for the CA
        /// </summary>
        public string name;
        /// <summary>
        /// X.501 distinguished name of the CA
        /// </summary>
        public X509Name DN;
        /// <summary>
        /// Type of CA (rootCA|subCA)
        /// </summary>
        public CA_Profile profile;
        /// <summary>
        /// Location of the profile XML file
        /// </summary>
        public string profileFile;
        /// <summary>
        /// Public key alogorithm used by the CA
        /// </summary>
        public string pkAlgo;
        /// <summary>
        /// Size if the public key (eg RSA modulus)
        /// </summary>
        public int pkSize;
        /// <summary>
        /// Signature algorithm used by the CA
        /// </summary>
        public string sigAlgo;
        /// <summary>
        /// Key usage extension value
        /// </summary>
        public int keyUsage;
        /// <summary>
        /// X509 version for the CA certificate
        /// </summary>
        public X509ver version;
        /// <summary>
        /// Lifetime of the CA certificate
        /// </summary>
        public int life;
        /// <summary>
        /// Units used by the lifetime value
        /// </summary>
        public string units;
        /// <summary>
        /// Frequency of publishing CRLs
        /// </summary>
        public double crlInterval;
        /// <summary>
        /// Type of CA (simpleCA|fipsCA|dhTA)
        /// </summary>
        public CA_Type caType;
        /// <summary>
        /// FIPS 140 mode
        /// </summary>
        public bool FIPS140;
        /// <summary>
        /// Name of Cryptographic Service Provider
        /// </summary>
        public string CSPName;
        /// <summary>
        /// Number of Cryptographic Service Provider
        /// </summary>
        public int CSPNum;
        /// <summary>
        /// Root directory of CA
        /// </summary>
        public string location;
        /// <summary>
        /// Password for accessing key material
        /// </summary>
        public string password;
        /// <summary>
        /// %age of CA key lifetime before rollover
        /// </summary>
        public int rollOver;
    }

}
