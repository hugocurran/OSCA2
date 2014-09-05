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
    /// A class to Generate Version 3 X509Certificates.
    /// <remarks>
    /// This is modified from the BC original (X509CertificateGenerator) to:
    /// <para>
    ///  + Accept pre-defined X509Extensions (from a request)
    ///  </para>
    ///  <para>
    ///  + Accept an OSCA profile describing extensions
    ///  </para>
    ///  </remarks>
    /// </summary>
    public class BcV3CertGen : ICertGen
    {
        internal readonly X509ExtensionsGenerator extGenerator = new X509ExtensionsGenerator();

        internal V3TbsCertificateGenerator tbsGen;
        internal DerObjectIdentifier sigOid;
        internal AlgorithmIdentifier sigAlgId;
        internal string signatureAlgorithm;
        internal List<PolicyEnforcement> policy = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BcV3CertGen"/> class.
        /// </summary>
        public BcV3CertGen()
        {
            tbsGen = new V3TbsCertificateGenerator();
        }

        internal BcV3CertGen(List<PolicyEnforcement> Policy) : this ()
        {
            this.policy = Policy;
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
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <returns>An X509Certificate.</returns>
        public virtual X509Certificate Generate(AsymmetricKeyParameter privateKey)
        {
            return Generate(privateKey, null);
        }

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <param name="Extensions">Set of extensions to include in the certificate.</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        /// <exception cref="PolicyEnforcementException">CA policy violation</exception>
        /// <exception cref="CertificateEncodingException">
        /// Exception encoding TBS cert
        /// or
        /// Exception producing certificate object
        /// </exception>
        public virtual X509Certificate Generate(AsymmetricKeyParameter privateKey, X509Extensions Extensions)
        {
            TbsCertificateStructure tbsCert = GenerateTbsCert(Extensions);

            // Check this complies with policy
            if (policy != null)
            {
                TestAgainstPolicy test = new TestAgainstPolicy(policy);
                if (!test.report(tbsCert))
                {
                    throw new PolicyEnforcementException(test.status.ToString());
                }
            }
            
            byte[] signature;

            try
            {
                signature = X509Utilities.GetSignatureForObject(
                    sigOid, signatureAlgorithm, privateKey, null, tbsCert);
            }
            catch (Exception e)
            {
                throw new CertificateEncodingException("Exception encoding TBS cert", e);
            }

            try
            {
                return new X509Certificate(new X509CertificateStructure(tbsCert, sigAlgId, new DerBitString(signature))); 
            }
            catch (CertificateParsingException e)
            {
                throw new CertificateEncodingException("Exception producing certificate object", e);
            }
        }

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <param name="Profile">OSCA profile.</param>
        /// <param name="Start">Start date</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        /// <exception cref="PolicyEnforcementException">CA policy violation</exception>
        /// <exception cref="CertificateEncodingException">Exception encoding TBS cert
        /// or
        /// Exception producing certificate object</exception>
        public virtual X509Certificate Generate(AsymmetricKeyParameter privateKey, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            // Set the validity according to the profile
            tbsGen.SetStartDate(new Time(NotBefore));
            tbsGen.SetEndDate(new Time(NotAfter));

            // Extract extensions from the profile
            foreach (ProfileExtension ext in Profile.Extensions)
                extGenerator.AddExtension(ext.OID, ext.Critical, ext.DerEncoding);

            // Call the generator
            return Generate(privateKey, extGenerator.Generate());
        }

        #region Stubs for system cryptography

        /// <summary>
        /// Generate an X509 Certificate
        /// Always throws an InvalidOperationException
        /// </summary>
        /// <param name="cspParam">CspParameters instance that has the private signing key</param>
        /// <returns>An X509 Certificate</returns>
        public virtual X509Certificate Generate(CspParameters cspParam)
        {
            throw new InvalidOperationException("System Cryptography not valid in BC context");
        }

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <param name="Extensions">The extensions.</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">System Cryptography not valid in BC context</exception>
        public virtual X509Certificate Generate(CspParameters privateKey, X509Extensions Extensions)
        {
            throw new InvalidOperationException("System Cryptography not valid in BC context");
        }

        /// <summary>
        /// Generate an X509Certificate.
        /// </summary>
        /// <param name="cspParam">The CSP parameter.</param>
        /// <param name="Profile">OSCA profile.</param>
        /// <param name="Start">Start date</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">System Cryptography not valid in BC context</exception>
        public virtual X509Certificate Generate(CspParameters cspParam, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            throw new InvalidOperationException("System Cryptography not valid in BC context");
        }

        #endregion

        internal TbsCertificateStructure GenerateTbsCert(X509Extensions Extensions)
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
