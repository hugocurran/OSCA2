using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Math.EC;

namespace OSCA.Crypto.CNG
{
    class EccKeyBlob
    {
        /// <summary>
        ///     Magic numbers identifying blob types
        /// </summary>
        private enum KeyBlobMagicNumber
        {
            ECDHPublicP256 = 0x314B4345,                        // BCRYPT_ECDH_PUBLIC_P256_MAGIC
            ECDHPublicP384 = 0x334B4345,                        // BCRYPT_ECDH_PUBLIC_P384_MAGIC
            ECDHPublicP521 = 0x354B4345,                        // BCRYPT_ECDH_PUBLIC_P521_MAGIC
            ECDsaPublicP256 = 0x31534345,                       // BCRYPT_ECDSA_PUBLIC_P256_MAGIC
            ECDsaPublicP384 = 0x33534345,                       // BCRYPT_ECDSA_PUBLIC_P384_MAGIC
            ECDsaPublicP521 = 0x35534345                        // BCRYPT_ECDSA_PUBLIC_P521_MAGIC
        }

        /// <summary>
        /// Curve namespace
        /// </summary>
        /// <remarks>
        /// See RFC 5480 - ANSI, NIST and Certificom (sec) use different names for the same curves.
        /// </remarks>
        private enum CurveNamespace
        {
            /// <summary>
            /// ANSI X.962
            /// </summary>
            X962,

            /// <summary>
            /// FIPS-PUB 186-2
            /// </summary>
            NIST,

            /// <summary>
            /// Certicom
            /// </summary>
            SEC
        }


        /// <summary>
        /// Determine the curve name from an EC key blob
        /// </summary>
        /// <param name="magic">Magic number</param>
        /// <returns>Curve name</returns>
        private static string getCurveName(int magic, CurveNamespace nameSpace)
        {
            switch (magic)
            {
                case (int)KeyBlobMagicNumber.ECDHPublicP256:
                case (int)KeyBlobMagicNumber.ECDsaPublicP256:
                    switch (nameSpace)
                    {
                        case CurveNamespace.X962:
                            return "prime256v1";
                        case CurveNamespace.NIST:
                            return "P-256";
                        case CurveNamespace.SEC:
                            return "SecP256r1";
                    }
                    break;
                case (int)KeyBlobMagicNumber.ECDHPublicP384:
                case (int)KeyBlobMagicNumber.ECDsaPublicP384:
                    switch (nameSpace)
                    {
                        case CurveNamespace.X962:           // These aren't in the BC list
                            return "prime384v1";
                        case CurveNamespace.NIST:
                            return "P-384";
                        case CurveNamespace.SEC:
                            return "SecP384r1";
                    }
                    break;
                case (int)KeyBlobMagicNumber.ECDHPublicP521:
                case (int)KeyBlobMagicNumber.ECDsaPublicP521:
                    switch (nameSpace)
                    {
                        case CurveNamespace.X962:       // These aren't in the BC list
                            return "prime521v1";
                        case CurveNamespace.NIST:
                            return "P-521";
                        case CurveNamespace.SEC:
                            return "SecP521r1";
                    }
                break;
            }
            return "";
        }


        /// <summary>
        /// Determine the curve OID from an EC key blob
        /// </summary>
        /// <param name="magic">Magic number</param>
        /// <returns>Curve OID</returns>
        private static DerObjectIdentifier getCurveOid(int magic, CurveNamespace nameSpace)
        {
            switch (magic)
            {
                case (int)KeyBlobMagicNumber.ECDHPublicP256:
                case (int)KeyBlobMagicNumber.ECDsaPublicP256:
                    switch (nameSpace)
                    {
                        case CurveNamespace.X962:
                            return X962NamedCurves.GetOid("prime256v1");
                        case CurveNamespace.NIST:
                            return NistNamedCurves.GetOid("P-256");
                        case CurveNamespace.SEC:
                            return SecNamedCurves.GetOid("SecP256r1");
                    }
                    break;
                case (int)KeyBlobMagicNumber.ECDHPublicP384:
                case (int)KeyBlobMagicNumber.ECDsaPublicP384:
                    switch (nameSpace)
                    {
                        case CurveNamespace.X962:           // These aren't in the BC list
                            return X962NamedCurves.GetOid("prime384v1");
                        case CurveNamespace.NIST:
                            return NistNamedCurves.GetOid("P-384");
                        case CurveNamespace.SEC:
                            return SecNamedCurves.GetOid("SecP384r1");
                    }
                    break;
                case (int)KeyBlobMagicNumber.ECDHPublicP521:
                case (int)KeyBlobMagicNumber.ECDsaPublicP521:
                    switch (nameSpace)
                    {
                        case CurveNamespace.X962:       // These aren't in the BC list
                            return X962NamedCurves.GetOid("prime521v1");
                        case CurveNamespace.NIST:
                            return NistNamedCurves.GetOid("P-521");
                        case CurveNamespace.SEC:
                            return SecNamedCurves.GetOid("SecP521r1");
                    }
                    break;
            }
            return null;
        }

        /// <summary>
        /// Get the curve from an EC key blob
        /// </summary>
        /// <param name="magic">Magic number</param>
        /// <returns>Curve </returns>
        private static X9ECParameters getCurve(int magic, CurveNamespace nameSpace)
        {
            switch (magic)
            {
                case (int)KeyBlobMagicNumber.ECDHPublicP256:
                case (int)KeyBlobMagicNumber.ECDsaPublicP256:
                    switch (nameSpace)
                    {
                        case CurveNamespace.X962:
                            return X962NamedCurves.GetByName("prime256v1");
                        case CurveNamespace.NIST:
                            return NistNamedCurves.GetByName("P-256");
                        case CurveNamespace.SEC:
                            return SecNamedCurves.GetByName("SecP256r1");
                    }
                    break;
                case (int)KeyBlobMagicNumber.ECDHPublicP384:
                case (int)KeyBlobMagicNumber.ECDsaPublicP384:
                    switch (nameSpace)
                    {
                        case CurveNamespace.X962:           // These aren't in the BC list
                            return X962NamedCurves.GetByName("prime384v1");
                        case CurveNamespace.NIST:
                            return NistNamedCurves.GetByName("P-384");
                        case CurveNamespace.SEC:
                            return SecNamedCurves.GetByName("SecP384r1");
                    }
                    break;
                case (int)KeyBlobMagicNumber.ECDHPublicP521:
                case (int)KeyBlobMagicNumber.ECDsaPublicP521:
                    switch (nameSpace)
                    {
                        case CurveNamespace.X962:       // These aren't in the BC list
                            return X962NamedCurves.GetByName("prime521v1");
                        case CurveNamespace.NIST:
                            return NistNamedCurves.GetByName("P-521");
                        case CurveNamespace.SEC:
                            return SecNamedCurves.GetByName("SecP521r1");
                    }
                    break;
            }
            return null;
        }
        /// <summary>
        /// Determin the algorithm from the magic number
        /// </summary>
        /// <param name="magic">magic number</param>
        /// <returns>algorithm name</returns>
        private static string getAlgorithm(int magic)
        {
            switch (magic)
            {
                case (int)KeyBlobMagicNumber.ECDHPublicP256:
                case (int)KeyBlobMagicNumber.ECDHPublicP384:
                case (int)KeyBlobMagicNumber.ECDHPublicP521:
                    return "ECDH";
             
                case (int)KeyBlobMagicNumber.ECDsaPublicP256:               
                case (int)KeyBlobMagicNumber.ECDsaPublicP384:
                case (int)KeyBlobMagicNumber.ECDsaPublicP521:
                    return "ECDSA";
            }
            return "";
        }

        /// <summary>
        ///     Unpack a key blob in ECC public blob format and return the EC public key parameters
        /// </summary>
        internal static ECPublicKeyParameters UnpackEccPublicBlob(byte[] blob)
        {
            // Read the magic number
            int magic = BitConverter.ToInt32(blob, 0);

            // read the size of each parameter
            int parameterSize = BitConverter.ToInt32(blob, sizeof(int));

            // Sanity check
            if (parameterSize <= 0)
                throw new InvalidParameterException("ECC blob parameterSize <= 0");
            if (blob.Length < 2 * sizeof(int) + 2 * parameterSize)
                throw new InvalidParameterException("ECC blob.Length < 2 * sizeof(int) + 2 * parameterSize");

            // Convert to ECPoint format (X, Y concatenated with leading compression byte)
            // Note that the blob format is big-endian, so we just copy as they are
            //Array.Reverse(blob, 2 * sizeof(int) + parameterSize, parameterSize);
            //Array.Reverse(blob, 2 * sizeof(int) + parameterSize, parameterSize);

            byte[] q = new byte[2 * parameterSize + 1];
            q[0] = Convert.ToByte(4);           // uncompressed
            byte[] x = new byte[parameterSize];
            byte[] y = new byte[parameterSize];
            Array.Copy(blob, 2 * sizeof(int), q, 1, parameterSize);
            Array.Copy(blob, 2 * sizeof(int) + parameterSize, q, parameterSize + 1, parameterSize);


            X9ECParameters namedCurve = getCurve(magic, CurveNamespace.NIST);

            return new ECPublicKeyParameters(getAlgorithm(magic),
                                 namedCurve.Curve.DecodePoint(q),           // Q
                                 getCurveOid(magic, CurveNamespace.NIST));
        }

        internal static ECPrivateKeyParameters UnpackEccPrivateBlob(byte[] blob)
        {
            // Read the magic number
            int magic = BitConverter.ToInt32(blob, 0);

            // read the size of each parameter
            int parameterSize = BitConverter.ToInt32(blob, sizeof(int));

            // Sanity check
            if (parameterSize <= 0)
                throw new InvalidParameterException("ECC blob parameterSize <= 0");
            if (blob.Length < 2 * sizeof(int) + 3 * parameterSize)
                throw new InvalidParameterException("ECC blob.Length < 2 * sizeof(int) + 3 * parameterSize");

            // read out the d parameter, convert to little-endian format
            // add 0x00 padding to force BigInteger to interpret these as positive numbers
            BigInteger d = new BigInteger(ReverseBytes(blob, 2 * sizeof(int) + 2 * parameterSize, parameterSize, true));

            return new ECPrivateKeyParameters(getAlgorithm(magic), d, getCurveOid(magic, CurveNamespace.NIST));
        }

        private static byte[] ReverseBytes(byte[] buffer, int offset, int count, bool padWithZeroByte)
        {
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

        private static string getBytes(byte[] buffer, int offset, int count)
        {
            byte[] reversed;
            reversed = new byte[count];
            
            int lastByte = offset + count - 1;
            for (int i = 0; i < count; i++)
            {
                reversed[i] = buffer[lastByte - i];
            }

            return BitConverter.ToString(reversed).Replace("-", string.Empty);
        }
    }
}
