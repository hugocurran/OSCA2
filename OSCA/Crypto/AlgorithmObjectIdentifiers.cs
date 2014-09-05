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

using System.Collections;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Utilities.Collections;

namespace OSCA.Crypto
{
    /// <summary>
    /// OIDs for cryptographic algorithms
    /// </summary>
    public class AlgorithmOids
    {
        /// <summary>
        /// The algorithms
        /// </summary>
        public static readonly Hashtable algorithms = new Hashtable();
        /// <summary>
        /// The ex parameters
        /// </summary>
        public static readonly Hashtable exParams = new Hashtable();
        /// <summary>
        /// The key algorithms
        /// </summary>
        public static readonly Hashtable keyAlgorithms = new Hashtable();
        /// <summary>
        /// The oids
        /// </summary>
        public static readonly Hashtable oids = new Hashtable();
        /// <summary>
        /// The no parameters
        /// </summary>
        public static readonly ISet noParams = new HashSet();

        /// <summary>
        /// OIDs for cryptographic albums
        /// </summary>
        static AlgorithmOids()
		{
			algorithms.Add("MD2WITHRSAENCRYPTION", new DerObjectIdentifier("1.2.840.113549.1.1.2"));
			algorithms.Add("MD2WITHRSA", new DerObjectIdentifier("1.2.840.113549.1.1.2"));
			algorithms.Add("MD5WITHRSAENCRYPTION", new DerObjectIdentifier("1.2.840.113549.1.1.4"));
			algorithms.Add("MD5WITHRSA", new DerObjectIdentifier("1.2.840.113549.1.1.4"));
			algorithms.Add("RSAWITHMD5", new DerObjectIdentifier("1.2.840.113549.1.1.4"));
			algorithms.Add("SHA1WITHRSAENCRYPTION", new DerObjectIdentifier("1.2.840.113549.1.1.5"));
			algorithms.Add("SHA1WITHRSA", new DerObjectIdentifier("1.2.840.113549.1.1.5"));
			algorithms.Add("SHA224WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha224WithRsaEncryption);
			algorithms.Add("SHA224WITHRSA", PkcsObjectIdentifiers.Sha224WithRsaEncryption);
			algorithms.Add("SHA256WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha256WithRsaEncryption);
			algorithms.Add("SHA256WITHRSA", PkcsObjectIdentifiers.Sha256WithRsaEncryption);
			algorithms.Add("SHA384WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha384WithRsaEncryption);
			algorithms.Add("SHA384WITHRSA", PkcsObjectIdentifiers.Sha384WithRsaEncryption);
			algorithms.Add("SHA512WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha512WithRsaEncryption);
			algorithms.Add("SHA512WITHRSA", PkcsObjectIdentifiers.Sha512WithRsaEncryption);
			algorithms.Add("SHA1WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss);
			algorithms.Add("SHA224WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss);
			algorithms.Add("SHA256WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss);
			algorithms.Add("SHA384WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss);
			algorithms.Add("SHA512WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss);
			algorithms.Add("RSAWITHSHA1", new DerObjectIdentifier("1.2.840.113549.1.1.5"));
			algorithms.Add("RIPEMD160WITHRSAENCRYPTION", new DerObjectIdentifier("1.3.36.3.3.1.2"));
			algorithms.Add("RIPEMD160WITHRSA", new DerObjectIdentifier("1.3.36.3.3.1.2"));
			algorithms.Add("SHA1WITHDSA", new DerObjectIdentifier("1.2.840.10040.4.3"));
			algorithms.Add("DSAWITHSHA1", new DerObjectIdentifier("1.2.840.10040.4.3"));
			algorithms.Add("SHA224WITHDSA", NistObjectIdentifiers.DsaWithSha224);
			algorithms.Add("SHA256WITHDSA", NistObjectIdentifiers.DsaWithSha256);
			algorithms.Add("SHA1WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha1);
			algorithms.Add("SHA224WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha224);
			algorithms.Add("SHA256WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha256);
			algorithms.Add("SHA384WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha384);
			algorithms.Add("SHA512WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha512);
			algorithms.Add("ECDSAWITHSHA1", X9ObjectIdentifiers.ECDsaWithSha1);
			algorithms.Add("GOST3411WITHGOST3410", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94);
			algorithms.Add("GOST3410WITHGOST3411", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94);
			algorithms.Add("GOST3411WITHECGOST3410", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001);
			algorithms.Add("GOST3411WITHECGOST3410-2001", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001);
			algorithms.Add("GOST3411WITHGOST3410-2001", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001);

			//
			// reverse mappings
			//
			oids.Add(new DerObjectIdentifier("1.2.840.113549.1.1.5"), "SHA1WITHRSA");
			oids.Add(PkcsObjectIdentifiers.Sha224WithRsaEncryption, "SHA224WITHRSA");
			oids.Add(PkcsObjectIdentifiers.Sha256WithRsaEncryption, "SHA256WITHRSA");
			oids.Add(PkcsObjectIdentifiers.Sha384WithRsaEncryption, "SHA384WITHRSA");
			oids.Add(PkcsObjectIdentifiers.Sha512WithRsaEncryption, "SHA512WITHRSA");
			oids.Add(CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94, "GOST3411WITHGOST3410");
			oids.Add(CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001, "GOST3411WITHECGOST3410");

			oids.Add(new DerObjectIdentifier("1.2.840.113549.1.1.4"), "MD5WITHRSA");
			oids.Add(new DerObjectIdentifier("1.2.840.113549.1.1.2"), "MD2WITHRSA");
			oids.Add(new DerObjectIdentifier("1.2.840.10040.4.3"), "SHA1WITHDSA");
			oids.Add(X9ObjectIdentifiers.ECDsaWithSha1, "SHA1WITHECDSA");
			oids.Add(X9ObjectIdentifiers.ECDsaWithSha224, "SHA224WITHECDSA");
			oids.Add(X9ObjectIdentifiers.ECDsaWithSha256, "SHA256WITHECDSA");
			oids.Add(X9ObjectIdentifiers.ECDsaWithSha384, "SHA384WITHECDSA");
			oids.Add(X9ObjectIdentifiers.ECDsaWithSha512, "SHA512WITHECDSA");
			oids.Add(OiwObjectIdentifiers.Sha1WithRsa, "SHA1WITHRSA");
			oids.Add(OiwObjectIdentifiers.DsaWithSha1, "SHA1WITHDSA");
			oids.Add(NistObjectIdentifiers.DsaWithSha224, "SHA224WITHDSA");
			oids.Add(NistObjectIdentifiers.DsaWithSha256, "SHA256WITHDSA");

			//
			// key types
			//
			keyAlgorithms.Add(PkcsObjectIdentifiers.RsaEncryption, "RSA");
			keyAlgorithms.Add(X9ObjectIdentifiers.IdDsa, "DSA");

			//
			// According to RFC 3279, the ASN.1 encoding SHALL (id-dsa-with-sha1) or MUST (ecdsa-with-SHA*) omit the parameters field. 
			// The parameters field SHALL be NULL for RSA based signature algorithms.
			//
			noParams.Add(X9ObjectIdentifiers.ECDsaWithSha1);
			noParams.Add(X9ObjectIdentifiers.ECDsaWithSha224);
			noParams.Add(X9ObjectIdentifiers.ECDsaWithSha256);
			noParams.Add(X9ObjectIdentifiers.ECDsaWithSha384);
			noParams.Add(X9ObjectIdentifiers.ECDsaWithSha512);
			noParams.Add(X9ObjectIdentifiers.IdDsaWithSha1);
			noParams.Add(NistObjectIdentifiers.DsaWithSha224);
			noParams.Add(NistObjectIdentifiers.DsaWithSha256);

			//
			// RFC 4491
			//
			noParams.Add(CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94);
			noParams.Add(CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001);

			//
			// explicit params
			//
			AlgorithmIdentifier sha1AlgId = new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1, DerNull.Instance);
			exParams.Add("SHA1WITHRSAANDMGF1", CreatePssParams(sha1AlgId, 20));

			AlgorithmIdentifier sha224AlgId = new AlgorithmIdentifier(NistObjectIdentifiers.IdSha224, DerNull.Instance);
			exParams.Add("SHA224WITHRSAANDMGF1", CreatePssParams(sha224AlgId, 28));

			AlgorithmIdentifier sha256AlgId = new AlgorithmIdentifier(NistObjectIdentifiers.IdSha256, DerNull.Instance);
			exParams.Add("SHA256WITHRSAANDMGF1", CreatePssParams(sha256AlgId, 32));

			AlgorithmIdentifier sha384AlgId = new AlgorithmIdentifier(NistObjectIdentifiers.IdSha384, DerNull.Instance);
			exParams.Add("SHA384WITHRSAANDMGF1", CreatePssParams(sha384AlgId, 48));

			AlgorithmIdentifier sha512AlgId = new AlgorithmIdentifier(NistObjectIdentifiers.IdSha512, DerNull.Instance);
			exParams.Add("SHA512WITHRSAANDMGF1", CreatePssParams(sha512AlgId, 64));
		}

        private static RsassaPssParameters CreatePssParams(
                    AlgorithmIdentifier hashAlgId,
                    int saltSize)
        {
            return new RsassaPssParameters(
                hashAlgId,
                new AlgorithmIdentifier(PkcsObjectIdentifiers.IdMgf1, hashAlgId),
                new DerInteger(saltSize),
                new DerInteger(1));
        }
    }
}
