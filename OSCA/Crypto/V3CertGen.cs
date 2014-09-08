using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using OSCA.Policy;
using OSCA.Profile;
using OSCA.Offline;

namespace OSCA.Crypto
{
    /// <summary>
    /// Abstract base class for V3 certificate generators
    /// </summary>
    public abstract class V3CertGen : ICertGen
    {
        /// <summary>
        /// The extensions generator
        /// </summary>
        protected readonly X509ExtensionsGenerator extGenerator = new X509ExtensionsGenerator();

        /// <summary>
        /// The TBS gen
        /// </summary>
        protected V3TbsCertificateGenerator tbsGen;
        /// <summary>
        /// The sig oid
        /// </summary>
        protected DerObjectIdentifier sigOid;
        /// <summary>
        /// The sig alg identifier
        /// </summary>
        protected AlgorithmIdentifier sigAlgId;
        /// <summary>
        /// The signature algorithm
        /// </summary>
        protected string signatureAlgorithm;
        /// <summary>
        /// The policy
        /// </summary>
        internal List<PolicyEnforcement> policy = null;

        /// <summary>
        /// Called by derived classes to initializes a new instance.
        /// </summary>
        public V3CertGen()
        {
            tbsGen = new V3TbsCertificateGenerator();
        }

        internal V3CertGen(List<PolicyEnforcement> Policy)
            : this()
        {
            this.policy = Policy;
        }

        private X509ver version = X509ver.V3;
        /// <summary>
        /// Return the X509 version this Certificate Generator will create.
        /// </summary>
        /// <returns></returns>
        public X509ver GetVersion()
        {
            return version;
        }

        /// <summary>
        /// Reset the Generator.
        /// </summary>
        public void Reset()
        {
            tbsGen = new V3TbsCertificateGenerator();
            extGenerator.Reset();
        }

        /// <summary>
        /// Set the certificate's serial number.
        /// </summary>
        /// <remarks>Make serial numbers long, if you have no serial number policy make sure the number is at least 16 bytes of secure random data.
        /// You will be surprised how ugly a serial number collision can get.</remarks>
        /// <param name="serialNumber">The serial number.</param>
        public void SetSerialNumber(BigInteger serialNumber)
        {
            if (serialNumber.SignValue <= 0)
            {
                throw new ArgumentException("serial number must be a positive integer", "serialNumber");
            }

            tbsGen.SetSerialNumber(new DerInteger(serialNumber));
        }

        /// <summary>
        /// Set the distinguished name of the issuer.
        /// The issuer is the entity which is signing the certificate.
        /// </summary>
        /// <param name="issuer">The issuer's DN.</param>
        public void SetIssuerDN(X509Name issuer)
        {
            tbsGen.SetIssuer(issuer);
        }

        /// <summary>
        /// Set the date that this certificate is to be valid from.
        /// </summary>
        /// <param name="Date">From date</param>
        public void SetNotBefore(DateTime Date)
        {
            tbsGen.SetStartDate(new Time(Date));
        }

        /// <summary>
        /// Set the date after which this certificate will no longer be valid.
        /// </summary>
        /// <param name="Date">Expiry date</param>
        public void SetNotAfter(DateTime Date)
        {
            tbsGen.SetEndDate(new Time(Date));
        }

        /// <summary>
        /// Set the DN of the entity that this certificate is about.
        /// </summary>
        /// <param name="Subject">Subject name</param>
        public void SetSubjectDN(X509Name Subject)
        {
            tbsGen.SetSubject(Subject);
        }

        /// <summary>
        /// Set the public key that this certificate identifies.
        /// </summary>
        /// <param name="publicKey"/>
        public void SetPublicKey(AsymmetricKeyParameter publicKey)
        {
            tbsGen.SetSubjectPublicKeyInfo(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey));
        }

        /// <summary>
        /// Set the signature algorithm that will be used to sign this certificate.
        /// </summary>
        /// <param name="signatureAlgorithm">Name of the signature algorithm</param>
        public void SetSignatureAlgorithm(string signatureAlgorithm)
        {
            this.signatureAlgorithm = signatureAlgorithm;

            try
            {
                sigOid = X509Utilities.GetAlgorithmOid(signatureAlgorithm);
            }
            catch (Exception)
            {
                throw new ArgumentException("Unknown signature type requested: " + signatureAlgorithm);
            }

            sigAlgId = X509Utilities.GetSigAlgID(sigOid, signatureAlgorithm);

            tbsGen.SetSignature(sigAlgId);
        }

        /// <summary>
        /// Set the subject unique ID - note: it is very rare that it is correct to do this.
        /// </summary>
        /// <param name="uniqueID"/>
        public void SetSubjectUniqueID(bool[] uniqueID)
        {
            tbsGen.SetSubjectUniqueID(booleanToBitString(uniqueID));
        }

        /// <summary>
        /// Set the issuer unique ID - note: it is very rare that it is correct to do this.
        /// </summary>
        /// <param name="uniqueID"/>
        public void SetIssuerUniqueID(bool[] uniqueID)
        {
            tbsGen.SetIssuerUniqueID(booleanToBitString(uniqueID));
        }

        private DerBitString booleanToBitString(bool[] id)
        {
            byte[] bytes = new byte[(id.Length + 7) / 8];

            for (int i = 0; i != id.Length; i++)
            {
                if (id[i])
                {
                    bytes[i / 8] |= (byte)(1 << ((7 - (i % 8))));
                }
            }

            int pad = id.Length % 8;

            if (pad == 0)
            {
                return new DerBitString(bytes);
            }

            return new DerBitString(bytes, 8 - pad);
        }

        /// <summary>
        /// Add a given extension field for the standard extensions tag (tag 3).
        /// </summary>
        /// <param name="oid">string containing a dotted decimal Object Identifier.</param>
        /// <param name="critical">Is it critical.</param>
        /// <param name="extensionValue">The value.</param>
        public void AddExtension(string oid, bool critical, Asn1Encodable extensionValue)
        {
            extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, extensionValue);
        }

        /// <summary>
        /// Add an extension to this certificate.
        /// </summary>
        /// <param name="oid">Its Object Identifier.</param>
        /// <param name="critical">Is it critical.</param>
        /// <param name="extensionValue">The value.</param>
        public void AddExtension(DerObjectIdentifier oid, bool critical, Asn1Encodable extensionValue)
        {
            extGenerator.AddExtension(oid, critical, extensionValue);
        }

        /// <summary>
        /// Add an extension using a string with a dotted decimal OID.
        /// </summary>
        /// <param name="oid">string containing a dotted decimal Object Identifier.</param>
        /// <param name="critical">Is it critical.</param>
        /// <param name="extensionValue">byte[] containing the value of this extension.</param>
        public void AddExtension(string oid, bool critical, byte[] extensionValue)
        {
            extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, new DerOctetString(extensionValue));
        }

        /// <summary>
        /// Add an extension to this certificate.
        /// </summary>
        /// <param name="oid">Its Object Identifier.</param>
        /// <param name="critical">Is it critical.</param>
        /// <param name="extensionValue">byte[] containing the value of this extension.</param>
        public void AddExtension(DerObjectIdentifier oid, bool critical, byte[] extensionValue)
        {
            extGenerator.AddExtension(oid, critical, new DerOctetString(extensionValue));
        }

        /// <summary>
        /// Add a given extension field for the standard extensions tag (tag 3),
        /// copying the extension value from another certificate.
        /// </summary>
        public void CopyAndAddExtension(string oid, bool critical, X509Certificate cert)
        {
            CopyAndAddExtension(new DerObjectIdentifier(oid), critical, cert);
        }

        /**
         * add a given extension field for the standard extensions tag (tag 3)
         * copying the extension value from another certificate.
         * @throws CertificateParsingException if the extension cannot be extracted.
         */
        public void CopyAndAddExtension(DerObjectIdentifier oid, bool critical, X509Certificate cert)
        {
            Asn1OctetString extValue = cert.GetExtensionValue(oid);

            if (extValue == null)
            {
                throw new CertificateParsingException("Extension " + oid + " not present");
            }

            try
            {
                Asn1Encodable value = X509ExtensionUtilities.FromExtensionValue(extValue);

                this.AddExtension(oid, critical, value);
            }
            catch (Exception e)
            {
                throw new CertificateParsingException(e.Message, e);
            }
        }

        /// <summary>
        /// Generates the TBS cert.
        /// </summary>
        /// <param name="Extensions">The extensions.</param>
        /// <returns></returns>
        protected TbsCertificateStructure GenerateTbsCert(X509Extensions Extensions)
        {
            if (!extGenerator.IsEmpty)
            {
                tbsGen.SetExtensions(extGenerator.Generate());
            }

            if (Extensions != null)
            {
                tbsGen.SetExtensions(Extensions);
            }

            return tbsGen.GenerateTbsCertificate();
        }

        /// <summary>
        /// Allows enumeration of the signature names supported by the generator.
        /// </summary>
        public virtual IEnumerable SignatureAlgNames
        {
            get { return X509Utilities.GetAlgNames(); }
        }
        
    }
}
