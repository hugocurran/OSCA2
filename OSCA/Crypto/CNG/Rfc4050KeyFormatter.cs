
// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.Diagnostics.Contracts;
//using Microsoft.Win32.SafeHandles;
using System.Security.Cryptography; 

namespace OSCA.Crypto.CNG
{

    /// <summary>
    ///     Utility class to convert NCrypt keys into XML and back using a format similar to the one described
    ///     in RFC 4050 (http://www.ietf.org/rfc/rfc4050.txt).
    /// 
    ///     #RFC4050ECKeyFormat
    /// 
    ///     The format looks similar to the following:
    /// 
    ///         <ECDSAKeyValue xmlns="http://www.w3.org/2001/04/xmldsig-more#">
    ///             <DomainParameters>
    ///                 <NamedCurve URN="urn:oid:1.3.132.0.35" />
    ///             </DomainParameters>
    ///             <PublicKey>
    ///                 <X Value="0123456789..." xsi:type="PrimeFieldElemType" />
    ///                 <Y Value="0123456789..." xsi:type="PrimeFieldElemType" />
    ///             </PublicKey>
    ///         </ECDSAKeyValue>
    /// </summary>
    internal static class Rfc4050KeyFormatter {
        private const string DomainParametersRoot = "DomainParameters";
        private const string ECDHRoot = "ECDHKeyValue";
        private const string ECDsaRoot = "ECDSAKeyValue";
        private const string NamedCurveElement = "NamedCurve";
        private const string Namespace = "http://www.w3.org/2001/04/xmldsig-more#";
        private const string PublicKeyRoot = "PublicKey";
        private const string UrnAttribute = "URN";
        private const string ValueAttribute = "Value";
        private const string XElement = "X";
        private const string YElement = "Y";
 
        private const string XsiTypeAttribute = "type";
        private const string XsiTypeAttributeValue = "PrimeFieldElemType";
        private const string XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
        private const string XsiNamespacePrefix = "xsi";
 
        private const string Prime256CurveUrn = "urn:oid:1.2.840.10045.3.1.7";
        private const string Prime384CurveUrn = "urn:oid:1.3.132.0.34";
        private const string Prime521CurveUrn = "urn:oid:1.3.132.0.35";
 
        /// <summary>
        ///     Restore a key from XML
        /// </summary>
        internal static CngKey FromXml(string xml) {
            Contract.Requires(xml != null);
            Contract.Ensures(Contract.Result<CngKey>() != null);
 
            // Load the XML into an XPathNavigator to access sub elements
            using (TextReader textReader = new StringReader(xml))
            using (XmlTextReader xmlReader = new XmlTextReader(textReader)) {
                XPathDocument document = new XPathDocument(xmlReader);
                XPathNavigator navigator = document.CreateNavigator();
                
                // Move into the root element - we don't do a specific namespace check here for compatibility
                // with XML that Windows generates.
                if (!navigator.MoveToFirstChild()) {
                    //throw new ArgumentException(SR.GetString(SR.Cryptography_MissingDomainParameters));
                    throw new ArgumentException("Cryptography_MissingDomainParameters");
                }
 
                // First figure out which algorithm this key belongs to
                CngAlgorithm algorithm = ReadAlgorithm(navigator);
 
                // Then read out the public key value
                if (!navigator.MoveToNext(XPathNodeType.Element)) {
                    //throw new ArgumentException(SR.GetString(SR.Cryptography_MissingPublicKey));
                    throw new ArgumentException("Cryptography_MissingPublicKey");
                }
 
                BigInteger x;
                BigInteger y;
                ReadPublicKey(navigator, out x, out y);
 
                // Finally, convert them into a key blob to import into a CngKey
                byte[] keyBlob = BuildEccPublicBlob(algorithm.Algorithm, x, y);
                return CngKey.Import(keyBlob, CngKeyBlobFormat.EccPublicBlob);
            }
        }
 
        /// <summary>
        ///     Map a curve URN to the size of the key associated with the curve
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "The parameters to the exception are in the correct order")]
        private static int GetKeySize(string urn) {
            Contract.Requires(!String.IsNullOrEmpty(urn));
            Contract.Ensures(Contract.Result<int>() > 0);
 
            switch (urn) {
                case Prime256CurveUrn:
                    return 256;
 
                case Prime384CurveUrn:
                    return 384;
 
                case Prime521CurveUrn:
                    return 521;
 
                default:
                    throw new ArgumentException("Cryptography_UnknownEllipticCurve ", "algorithm");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_UnknownEllipticCurve), "algorithm");
            }
        }
 
        /// <summary>
        ///     Get the OID which represents an elliptic curve
        /// </summary>
        private static string GetCurveUrn(CngAlgorithm algorithm) {
            Contract.Requires(algorithm != null);
 
            if (algorithm == CngAlgorithm.ECDsaP256 || algorithm == CngAlgorithm.ECDiffieHellmanP256) {
                return Prime256CurveUrn;
            }
            else if (algorithm == CngAlgorithm.ECDsaP384 || algorithm == CngAlgorithm.ECDiffieHellmanP384) {
                return Prime384CurveUrn;
            }
            else if (algorithm == CngAlgorithm.ECDsaP521 || algorithm == CngAlgorithm.ECDiffieHellmanP521) {
                return Prime521CurveUrn;
            }
            else {
                throw new ArgumentException("Cryptography_UnknownEllipticCurve", "algorithm");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_UnknownEllipticCurve), "algorithm");
            }
        }
 
        /// <summary>
        ///     Determine which ECC algorithm the key refers to
        /// </summary>
        private static CngAlgorithm ReadAlgorithm(XPathNavigator navigator) {
            Contract.Requires(navigator != null);
            Contract.Ensures(Contract.Result<CngAlgorithm>() != null);
 
            if (navigator.NamespaceURI != Namespace) {
                throw new ArgumentException("Cryptography_UnexpectedXmlNamespace",
                                                         //navigator.NamespaceURI,
                                                         Namespace);
                //throw new ArgumentException(SR.GetString(SR.Cryptography_UnexpectedXmlNamespace,
                                                         //navigator.NamespaceURI,
                                                         //Namespace));
            }
 
            //
            // The name of the root element determines which algorithm to use, while the DomainParameters
            // element specifies which curve we should be using.
            //
 
            bool isDHKey = navigator.Name == ECDHRoot;
            bool isDsaKey = navigator.Name == ECDsaRoot;
 
            if (!isDHKey && !isDsaKey) {
                throw new ArgumentException("Cryptography_UnknownEllipticCurveAlgorithm");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_UnknownEllipticCurveAlgorithm));
            }
 
            // Move into the DomainParameters element
            if (!navigator.MoveToFirstChild() || navigator.Name != DomainParametersRoot) {
                throw new ArgumentException("Cryptography_MissingDomainParameters");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_MissingDomainParameters));
            }
 
            // Now move into the NamedCurve element
            if (!navigator.MoveToFirstChild() || navigator.Name != NamedCurveElement) {
                throw new ArgumentException("Cryptography_MissingDomainParameters");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_MissingDomainParameters));
            }
 
            // And read its URN value
            if (!navigator.MoveToFirstAttribute() || navigator.Name != UrnAttribute || String.IsNullOrEmpty(navigator.Value)) {
                throw new ArgumentException("Cryptography_MissingDomainParameters");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_MissingDomainParameters));
            }
 
            int keySize = GetKeySize(navigator.Value);
 
            // position the navigator at the end of the domain parameters
            navigator.MoveToParent();   // NamedCurve
            navigator.MoveToParent();   // DomainParameters
 
            //
            // Given the algorithm type and key size, we can now map back to a CNG algorithm ID
            //
 
            if (isDHKey) {
                if (keySize == 256) {
                    return CngAlgorithm.ECDiffieHellmanP256;
                }
                else if (keySize == 384) {
                    return CngAlgorithm.ECDiffieHellmanP384;
                }
                else {
                    Debug.Assert(keySize == 521, "keySize == 521");
                    return CngAlgorithm.ECDiffieHellmanP521;
                }
            }
            else {
                Debug.Assert(isDsaKey, "isDsaKey");
 
                if (keySize == 256) {
                    return CngAlgorithm.ECDsaP256;
                }
                else if (keySize == 384) {
                    return CngAlgorithm.ECDsaP384;
                }
                else {
                    Debug.Assert(keySize == 521, "keySize == 521");
                    return CngAlgorithm.ECDsaP521;
                }
            }
        }
 
        /// <summary>
        ///     Read the x and y components of the public key
        /// </summary>
        private static void ReadPublicKey(XPathNavigator navigator, out BigInteger x, out BigInteger y) {
            Contract.Requires(navigator != null);
 
            if (navigator.NamespaceURI != Namespace) {
                throw new ArgumentException("Cryptography_UnexpectedXmlNamespace",
                                                         //navigator.NamespaceURI,
                                                         Namespace);
                //                throw new ArgumentException(SR.GetString(SR.Cryptography_UnexpectedXmlNamespace,
                                                         //navigator.NamespaceURI,
                                                         //Namespace));
            }
 
            if (navigator.Name != PublicKeyRoot) {
                throw new ArgumentException("Cryptography_MissingPublicKey");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_MissingPublicKey));
            }
 
            // First get the x parameter
            if (!navigator.MoveToFirstChild() || navigator.Name != XElement) {
                throw new ArgumentException("Cryptography_MissingPublicKey");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_MissingPublicKey));
            }
            if (!navigator.MoveToFirstAttribute() || navigator.Name != ValueAttribute || String.IsNullOrEmpty(navigator.Value)) {
                throw new ArgumentException("Cryptography_MissingPublicKey");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_MissingPublicKey));
            }
 
            x = BigInteger.Parse(navigator.Value, CultureInfo.InvariantCulture);
            navigator.MoveToParent();
 
            // Then the y parameter
            if (!navigator.MoveToNext(XPathNodeType.Element) || navigator.Name != YElement) {
                throw new ArgumentException("Cryptography_MissingPublicKey");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_MissingPublicKey));
            }
            if (!navigator.MoveToFirstAttribute() || navigator.Name != ValueAttribute || String.IsNullOrEmpty(navigator.Value)) {
                throw new ArgumentException("Cryptography_MissingPublicKey");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_MissingPublicKey));
            }
 
            y = BigInteger.Parse(navigator.Value, CultureInfo.InvariantCulture);
        }
 
        /// <summary>
        ///     Serialize out information about the elliptic curve
        /// </summary>
        private static void WriteDomainParameters(XmlWriter writer, CngKey key) {
            Contract.Requires(writer != null);
            Contract.Requires(key != null && (key.AlgorithmGroup == CngAlgorithmGroup.ECDsa || key.AlgorithmGroup == CngAlgorithmGroup.ECDiffieHellman));
 
            writer.WriteStartElement(DomainParametersRoot);
 
            // We always use OIDs for the named prime curves
            writer.WriteStartElement(NamedCurveElement);
            writer.WriteAttributeString(UrnAttribute, GetCurveUrn(key.Algorithm));
            writer.WriteEndElement();   // </NamedCurve>
 
            writer.WriteEndElement();   // </DomainParameters>
        }
 
        private static void WritePublicKeyValue(XmlWriter writer, CngKey key) {
            Contract.Requires(writer != null);
            Contract.Requires(key != null && (key.AlgorithmGroup == CngAlgorithmGroup.ECDsa || key.AlgorithmGroup == CngAlgorithmGroup.ECDiffieHellman));
 
            writer.WriteStartElement(PublicKeyRoot);
 
            byte[] exportedKey = key.Export(CngKeyBlobFormat.EccPublicBlob);
            BigInteger x;
            BigInteger y;
            UnpackEccPublicBlob(exportedKey, out x, out y);
 
            writer.WriteStartElement(XElement);
            writer.WriteAttributeString(ValueAttribute, x.ToString("R", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(XsiNamespacePrefix, XsiTypeAttribute, XsiNamespace, XsiTypeAttributeValue);
            writer.WriteEndElement();   // </X>
 
            writer.WriteStartElement(YElement);
            writer.WriteAttributeString(ValueAttribute, y.ToString("R", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(XsiNamespacePrefix, XsiTypeAttribute, XsiNamespace, XsiTypeAttributeValue);
            writer.WriteEndElement();   // </Y>
 
            writer.WriteEndElement();   // </PublicKey>
        }
 
        /// <summary>
        ///     Convert a key to XML
        /// </summary>
        internal static string ToXml(CngKey key) {
            Contract.Requires(key != null && (key.AlgorithmGroup == CngAlgorithmGroup.ECDsa || key.AlgorithmGroup == CngAlgorithmGroup.ECDiffieHellman));
            Contract.Ensures(Contract.Result<String>() != null);
 
            StringBuilder keyXml = new StringBuilder();
 
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.OmitXmlDeclaration = true;
 
            using (XmlWriter writer = XmlWriter.Create(keyXml, settings)) {
                // The root element depends upon the type of key
                string rootElement = key.AlgorithmGroup == CngAlgorithmGroup.ECDsa ? ECDsaRoot : ECDHRoot;
                writer.WriteStartElement(rootElement, Namespace);
 
                WriteDomainParameters(writer, key);
                WritePublicKeyValue(writer, key);
 
                writer.WriteEndElement();   // root element
            }
 
            return keyXml.ToString();
        }

        ////// from native methods

        /// <summary>
        ///     Unpack a key blob in ECC public blob format into its X and Y parameters
        /// 
        ///     This method expects that the blob be in the correct format -- blobs accepted from partially
        ///     trusted code need to be validated before being unpacked.
        /// </summary>
        internal static void UnpackEccPublicBlob(byte[] blob, out BigInteger x, out BigInteger y)
        {
            Contract.Requires(blob != null && blob.Length > 2 * sizeof(int));

            //
            // See code:System.Security.Cryptography.NCryptNative#ECCPublicBlobFormat  for details about the
            // format of the ECC public key blob.
            //

            // read the size of each parameter
            int parameterSize = BitConverter.ToInt32(blob, sizeof(int));
            Debug.Assert(parameterSize > 0, "parameterSize > 0");
            Debug.Assert(blob.Length >= 2 * sizeof(int) + 2 * parameterSize, "blob.Length >= 2 * sizeof(int) + 2 * parameterSize");

            // read out the X and Y parameters, in memory reversed form
            // add 0x00 padding to force BigInteger to interpret these as positive numbers
            x = new BigInteger(ReverseBytes(blob, 2 * sizeof(int), parameterSize, true));
            y = new BigInteger(ReverseBytes(blob, 2 * sizeof(int) + parameterSize, parameterSize, true));
        }


        /// <summary>
        ///     Reverse the bytes in a buffer
        /// </summary>
        private static byte[] ReverseBytes(byte[] buffer)
        {
            Contract.Requires(buffer != null);
            Contract.Ensures(Contract.Result<byte[]>() != null && Contract.Result<byte[]>().Length == buffer.Length);
            return ReverseBytes(buffer, 0, buffer.Length, false);
        }

        /// <summary>
        ///     Reverse a section of bytes within a buffer
        /// </summary>
        private static byte[] ReverseBytes(byte[] buffer, int offset, int count)
        {
            return ReverseBytes(buffer, offset, count, false);
        }

        private static byte[] ReverseBytes(byte[] buffer, int offset, int count, bool padWithZeroByte)
        {
            Contract.Requires(buffer != null);
            Contract.Requires(offset >= 0 && offset < buffer.Length);
            Contract.Requires(count >= 0 && buffer.Length - count >= offset);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == (padWithZeroByte ? count + 1 : count));
            Contract.Ensures(padWithZeroByte ? Contract.Result<byte[]>()[count] == 0 : true);

            byte[] reversed;
            if (padWithZeroByte)
            {
                reversed = new byte[count + 1]; // the last (most-significant) byte will be left as 0x00
            }
            else
            {
                reversed = new byte[count];
            }

            int lastByte = offset + count - 1;
            for (int i = 0; i < count; i++)
            {
                reversed[i] = buffer[lastByte - i];
            }

            return reversed;
        }

        /// <summary>
        ///     Build an ECC public key blob to represent the given parameters
        /// </summary>
        internal static byte[] BuildEccPublicBlob(string algorithm, BigInteger x, BigInteger y)
        {
            Contract.Requires(!String.IsNullOrEmpty(algorithm));
            Contract.Ensures(Contract.Result<byte[]>() != null);

            //
            // #ECCPublicBlobFormat
            // The ECC public key blob format is as follows:
            //
            // DWORD dwMagic
            // DWORD cbKey
            // X parameter (cbKey bytes long, byte-reversed)
            // Y parameter (cbKey bytes long, byte-reversed)
            //

            // First map the algorithm name to its magic number and key size
            KeyBlobMagicNumber algorithmMagic;
            int keySize;
            MapAlgorithmIdToMagic(algorithm, out algorithmMagic, out keySize);

            // Next generate the public key parameters
            byte[] xBytes = ReverseBytes(FillKeyParameter(x.ToByteArray(), keySize));
            byte[] yBytes = ReverseBytes(FillKeyParameter(y.ToByteArray(), keySize));

            // Finally, lay out the structure itself
            byte[] blob = new byte[2 * sizeof(int) + xBytes.Length + yBytes.Length];
            Buffer.BlockCopy(BitConverter.GetBytes((int)algorithmMagic), 0, blob, 0, sizeof(int));
            Buffer.BlockCopy(BitConverter.GetBytes(xBytes.Length), 0, blob, sizeof(int), sizeof(int));
            Buffer.BlockCopy(xBytes, 0, blob, 2 * sizeof(int), xBytes.Length);
            Buffer.BlockCopy(yBytes, 0, blob, 2 * sizeof(int) + xBytes.Length, yBytes.Length);

            return blob;
        }


                /// <summary>
        ///     Magic numbers identifying blob types
        /// </summary>
        internal enum KeyBlobMagicNumber {
            ECDHPublicP256 = 0x314B4345,                        // BCRYPT_ECDH_PUBLIC_P256_MAGIC
            ECDHPublicP384 = 0x334B4345,                        // BCRYPT_ECDH_PUBLIC_P384_MAGIC
            ECDHPublicP521 = 0x354B4345,                        // BCRYPT_ECDH_PUBLIC_P521_MAGIC
            ECDsaPublicP256 = 0x31534345,                       // BCRYPT_ECDSA_PUBLIC_P256_MAGIC
            ECDsaPublicP384 = 0x33534345,                       // BCRYPT_ECDSA_PUBLIC_P384_MAGIC
            ECDsaPublicP521 = 0x35534345                        // BCRYPT_ECDSA_PUBLIC_P521_MAGIC
        }

        /// <summary>
        ///     Map an algorithm identifier to a key size and magic number
        /// </summary>
        internal static void MapAlgorithmIdToMagic(string algorithm,
                                                   out KeyBlobMagicNumber algorithmMagic,
                                                   out int keySize)
        {
            Contract.Requires(!String.IsNullOrEmpty(algorithm));

            switch (algorithm)
            {
                case AlgorithmName.ECDHP256:
                    algorithmMagic = KeyBlobMagicNumber.ECDHPublicP256;
                    keySize = 256;
                    break;

                case AlgorithmName.ECDHP384:
                    algorithmMagic = KeyBlobMagicNumber.ECDHPublicP384;
                    keySize = 384;
                    break;

                case AlgorithmName.ECDHP521:
                    algorithmMagic = KeyBlobMagicNumber.ECDHPublicP521;
                    keySize = 521;
                    break;

                case AlgorithmName.ECDsaP256:
                    algorithmMagic = KeyBlobMagicNumber.ECDsaPublicP256;
                    keySize = 256;
                    break;

                case AlgorithmName.ECDsaP384:
                    algorithmMagic = KeyBlobMagicNumber.ECDsaPublicP384;
                    keySize = 384;
                    break;

                case AlgorithmName.ECDsaP521:
                    algorithmMagic = KeyBlobMagicNumber.ECDsaPublicP521;
                    keySize = 521;
                    break;

                default:
                    throw new ArgumentException("Cryptography_UnknownEllipticCurveAlgorithm");
                //throw new ArgumentException(SR.GetString(SR.Cryptography_UnknownEllipticCurveAlgorithm));
            }
        }

        internal static class AlgorithmName
        {
            public const string ECDHP256 = "ECDH_P256";         // BCRYPT_ECDH_P256_ALGORITHM
            public const string ECDHP384 = "ECDH_P384";         // BCRYPT_ECDH_P384_ALGORITHM
            public const string ECDHP521 = "ECDH_P521";         // BCRYPT_ECDH_P521_ALGORITHM
            public const string ECDsaP256 = "ECDSA_P256";       // BCRYPT_ECDSA_P256_ALGORITHM
            public const string ECDsaP384 = "ECDSA_P384";       // BCRYPT_ECDSA_P384_ALGORITHM
            public const string ECDsaP521 = "ECDSA_P521";       // BCRYPT_ECDSA_P521_ALGORITHM
            public const string MD5 = "MD5";                    // BCRYPT_MD5_ALGORITHM
            public const string Sha1 = "SHA1";                  // BCRYPT_SHA1_ALGORITHM
            public const string Sha256 = "SHA256";              // BCRYPT_SHA256_ALGORITHM
            public const string Sha384 = "SHA384";              // BCRYPT_SHA384_ALGORITHM
            public const string Sha512 = "SHA512";              // BCRYPT_SHA512_ALGORITHM
        }

        /// <summary>
        ///     Make sure that a key is padded out to be its full size
        /// </summary>
        private static byte[] FillKeyParameter(byte[] key, int keySize)
        {
            Contract.Requires(key != null);
            Contract.Requires(keySize > 0);
            Contract.Ensures(Contract.Result<byte[]>() != null && Contract.Result<byte[]>().Length >= keySize / 8);

            int bytesRequired = (keySize / 8) + (keySize % 8 == 0 ? 0 : 1);
            if (key.Length == bytesRequired)
            {
                return key;
            }

#if DEBUG
            // If the key is longer than required, it should have been padded out with zeros
            if (key.Length > bytesRequired)
            {
                for (int i = bytesRequired; i < key.Length; i++)
                {
                    Debug.Assert(key[i] == 0, "key[i] == 0");
                }
            }
#endif
            byte[] fullKey = new byte[bytesRequired];
            Buffer.BlockCopy(key, 0, fullKey, 0, Math.Min(key.Length, fullKey.Length));
            return fullKey;
        }
    }
}
    
