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
using System.Collections;
using System.Text;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System.IO;
using Org.BouncyCastle.OpenSsl;


namespace OSCA.Offline
{
    /// <summary>
    /// Collection of utility functions
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Derive a DN from a CAConfig object
        /// </summary>
        /// <param name="config">CA Config object</param>
        /// <returns>DN of CA</returns>
        public static X509Name getDN(CAConfig config)
        {
            return new X509Name("CN=" + config.name);
        }

        /// <summary>
        /// Export an X.509-related object to a PEM format string
        /// </summary>
        /// <remarks>
        /// The following object types are supported:
        /// <code>
        ///   + X.509 Public Key Certificate
        ///   + X.509 Attribute Certificate
        ///   + X509 CRL
        ///   + PKCS10 Certificate Request
        ///   + PKCS7 CMS object
        ///   + Private key
        ///   + Public key
        /// </code>
        /// </remarks>
        /// <param name="Cert">The object to export</param>
        /// <returns>A PEM encoded string</returns>
        /// <exception cref="ArgumentNullException">No object supplied</exception>
        /// <exception cref="PemGenerationException">Object type not supported</exception>
        /// <exception cref="IOException">IO Stream problem</exception>
        public static string ExportToPEM(object obj)
        {
            TextWriter textWriter = new StringWriter();
            PemWriter pemWriter = new PemWriter(textWriter);
            pemWriter.WriteObject(obj);
            pemWriter.Writer.Flush();

            return textWriter.ToString();
        }

        /// <summary>
        /// Base64 encode a X509 certificate
        /// </summary>
        /// <param name="cert">Certificate object</param>
        /// <returns>Base64 string containing the certificate</returns>
        public static string cert64(X509Certificate cert)
        {
            return Convert.ToBase64String(cert.GetEncoded());
        }

        /// <summary>
        /// Base64 encode a PKCS#10 certificate request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string rqst64(Pkcs10CertificationRequest request)
        {
            return Convert.ToBase64String(request.GetEncoded());
        }

        /// <summary>
        /// Sets the type of the CA
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>CA_Type enum</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Unexpected CA type:  + type</exception>
        public static CA_Type SetCA_Type(string type)
        {
            switch (type)
            {
                case "fipsCA":
                    return CA_Type.fipsCA;
                case "simpleCA":
                    return CA_Type.simpleCA;
                case "bcCA":
                    return CA_Type.bcCA;
                case "sysCA":
                    return CA_Type.sysCA;
                case "dhTA":
                    return CA_Type.dhTA;
                default:
                    throw new ArgumentOutOfRangeException("Unexpected CA type: " + type);
            }
        }

        /// <summary>
        /// Key ID type
        /// </summary>
        public enum KeyIdType
        {
            /// <summary>
            /// The akid
            /// </summary>
            AKID,
            /// <summary>
            /// The skid
            /// </summary>
            SKID
        }

        /// <summary>
        /// Returns true if a KeyID matches one in a certificate
        /// </summary>
        /// <param name="KeyId">KeyIDvalue to test</param>
        /// <param name="Certificate">Certificate to test</param>
        /// <param name="IdType">Authority or Subject KeyID</param>
        /// <returns>
        /// True for a key match
        /// </returns>
        /// <exception cref="System.ApplicationException">Authority KeyID extension not found in certificate
        /// or
        /// Subject KeyID extension not found in certificate</exception>
        public static bool TestKeyId(byte[] KeyId, X509Certificate Certificate, KeyIdType IdType)
        {
            X509Extensions extensions = Certificate.GetExtensions();

            X509Extension ext;
            AuthorityKeyIdentifier akid;
            SubjectKeyIdentifier skid;
            switch (IdType)
            {
                case KeyIdType.AKID:
                    ext = extensions.GetExtension(X509Extensions.AuthorityKeyIdentifier);
                    if (ext == null)
                        throw new ApplicationException("Authority KeyID extension not found in certificate");
                    else
                    {
                        akid = AuthorityKeyIdentifier.GetInstance(ext);
                        if (KeyId == akid.GetKeyIdentifier())
                            return true;
                    }
                    break;

                case KeyIdType.SKID:
                    ext = extensions.GetExtension(X509Extensions.SubjectKeyIdentifier);
                    if (ext == null)
                        throw new ApplicationException("Subject KeyID extension not found in certificate");
                    else
                    {
                        skid = SubjectKeyIdentifier.GetInstance(ext);
                        if (KeyId == skid.GetKeyIdentifier())
                            return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Test a DN for null RDN values
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns>
        /// True if all RDNs are non-null, false otherwise
        /// </returns>
        public static bool TestDN(X509Name Name)
        {
            return false;
        }

        /// <summary>
        /// Checks and reorders a DN to place CN first (default) or last
        /// </summary>
        /// <param name="Name">DN to check</param>
        /// <param name="CNFirst">if set to <c>true</c> [cn first].</param>
        /// <returns>
        /// Reordered DN
        /// </returns>
        public static X509Name OrderDN(X509Name Name, bool CNFirst=true)
        {
            IList oids;
            if (CNFirst)
            {
                oids = Name.GetOidList();
                if (!X509Name.CN.Equals(oids[0]))
                {
                    return new X509Name(CNFirst, Name.ToString());
                }
            }
            return Name;
        }

        /// <summary>
        /// Checks and reorders a DN to place CN first or last
        /// </summary>
        /// <param name="Name">DN to check</param>
        /// <param name="CNFirst">if set to <c>true</c> [cn first].</param>
        /// <returns>
        /// Reordered DN
        /// </returns>
        public static string OrderDN(string Name, bool CNFirst=true)
        {
            return OrderDN(new X509Name(Name), CNFirst).ToString();
        }

        #region UTF-8 array convertors

        /// <summary>
        /// Convert a Byte Array of Unicode values (UTF-8 encoded) to a complete string
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to a String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        public static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return constructedString;
        }

        /// <summary>
        /// Convert a String to a Unicode (UTF-8 encoded) Byte Array
        /// </summary>
        /// <param name="pXmlString">String to be converted to a Unicode Byte Array</param>
        /// <returns>Unicode (UTF-8) Byte Array converted from a String</returns>
        public static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        #endregion

    }

}

