using System;
using System.Collections;
using System.Security.Cryptography;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.X509;

namespace OSCA.Crypto
{
    /// <summary>
    /// Class to Generate X509V1 Certificates.
    /// </summary>
    public class BcV1CertGen : ICertGen
    {
        internal V1TbsCertificateGenerator tbsGen;
        private DerObjectIdentifier sigOID;
        internal AlgorithmIdentifier sigAlgId;
        internal string signatureAlgorithm;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public BcV1CertGen()
        {
            tbsGen = new V1TbsCertificateGenerator();
        }

        /// <summary>
        /// Reset the Generator.
        /// </summary>
        public void Reset()
        {
            tbsGen = new V1TbsCertificateGenerator();
        }

        /// <summary>
        /// Set the certificate's serial number.
        /// </summary>
        /// <remarks>Make serial numbers long, if you have no serial number policy make sure the number is at least 16 bytes of secure random data.
        /// You will be surprised how ugly a serial number collision can get.</remarks>
        /// <param name="serialNumber">The serial number.</param>
        public void SetSerialNumber(
            BigInteger serialNumber)
        {
            if (serialNumber.SignValue <= 0)
            {
                throw new ArgumentException("serial number must be a positive integer", "serialNumber");
            }

            tbsGen.SetSerialNumber(new DerInteger(serialNumber));
        }

        /// <summary>
        /// Set the issuer distinguished name.
        /// The issuer is the entity whose private key is used to sign the certificate.
        /// </summary>
        /// <param name="issuer">The issuers DN.</param>
        public void SetIssuerDN(
            X509Name issuer)
        {
            tbsGen.SetIssuer(issuer);
        }

        /// <summary>
        /// Set the date that this certificate is to be valid from.
        /// </summary>
        /// <param name="date">From data</param>
        public void SetNotBefore(
            DateTime date)
        {
            tbsGen.SetStartDate(new Time(date));
        }
        
        /// <summary>
        /// Set the date after which this certificate will no longer be valid.
        /// </summary>
        /// <param name="date">Until data</param>
        public void SetNotAfter(DateTime date)
        {
            tbsGen.SetEndDate(new Time(date));
        }

        /// <summary>
        /// Set the subject distinguished name.
        /// The subject describes the entity associated with the public key.
        /// </summary>
        /// <param name="subject">Subject DN</param>
        public void SetSubjectDN(
            X509Name subject)
        {
            tbsGen.SetSubject(subject);
        }

        /// <summary>
        /// Set the public key that this certificate identifies.
        /// </summary>
        /// <param name="publicKey">Public key value</param>
        /// <exception cref="System.ArgumentException">Unable to process key</exception>
        public void SetPublicKey(
            AsymmetricKeyParameter publicKey)
        {
            try
            {
                tbsGen.SetSubjectPublicKeyInfo(
                    SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey));
            }
            catch (Exception e)
            {
                throw new ArgumentException("Unable to process key - " + e.ToString());
            }
        }

        /// <summary>
        /// Set the signature algorithm that will be used to sign this certificate.
        /// This can be either a name or an OID, names are treated as case insensitive.
        /// </summary>
        /// <param name="signatureAlgorithm">string representation of the algorithm name</param>
        public void SetSignatureAlgorithm(
            string signatureAlgorithm)
        {
            this.signatureAlgorithm = signatureAlgorithm;

            try
            {
                sigOID = X509Utilities.GetAlgorithmOid(signatureAlgorithm);
            }
            catch (Exception)
            {
                throw new ArgumentException("Unknown signature type requested", "signatureAlgorithm");
            }

            sigAlgId = X509Utilities.GetSigAlgID(sigOID, signatureAlgorithm);

            tbsGen.SetSignature(sigAlgId);
        }

        /// <summary>
        /// Generate a new X509Certificate.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer used to sign this certificate.</param>
        /// <returns>An X509Certificate.</returns>
        public virtual X509Certificate Generate(AsymmetricKeyParameter privateKey)
        {
            return generate(null, (AsymmetricKeyParameter)privateKey);
        }

        /// <summary>
        /// Generate a new X509Certificate. (Note that Extensions is ignored)
        /// </summary>
        /// <param name="privateKey">The private key of the issuer that is signing this certificate.</param>
        /// <param name="Extensions">X.509 extensions (ignored).</param>
        /// <returns>
        /// An X509Certificate.
        /// </returns>
        public virtual X509Certificate Generate(AsymmetricKeyParameter privateKey, X509Extensions Extensions)
        {
            return generate(null, privateKey);
        }

         /// <summary>
        /// Extensions are not supported in X.509 V1 certificates.
        /// </summary>
        public X509Certificate Generate(AsymmetricKeyParameter privateKey, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            SetNotBefore(NotBefore);
            SetNotAfter(NotAfter);
            return generate(null, privateKey);
        }

        /// <summary>
        /// Generate a new X509Certificate specifying a SecureRandom instance that you would like to use.
        /// </summary>
        /// <param name="privateKey">The private key of the issuer used to sign this certificate.</param>
        /// <param name="random">The Secure Random you want to use.</param>
        /// <returns>An X509Certificate.</returns>
        private X509Certificate generate(SecureRandom random, AsymmetricKeyParameter privateKey )
        {
            TbsCertificateStructure tbsCert = tbsGen.GenerateTbsCertificate();
            byte[] signature;

            try
            {
                signature = X509Utilities.GetSignatureForObject(
                    sigOID, signatureAlgorithm, privateKey, random, tbsCert);
            }
            catch (Exception e)
            {

                throw new CertificateEncodingException("Exception encoding TBS cert", e);
            }

            try
            {
                return GenerateJcaObject(tbsCert, signature);
            }
            catch (CertificateParsingException e)
            {
                throw new CertificateEncodingException("Exception producing certificate object", e);
            }
        }

        /// <summary>
        /// Generates the jca object.
        /// </summary>
        /// <param name="tbsCert">The TBS cert.</param>
        /// <param name="signature">The signature.</param>
        /// <returns>The newly minted certificate</returns>
        private X509Certificate GenerateJcaObject(
            TbsCertificateStructure tbsCert,
            byte[] signature)
        {
            return new X509Certificate(
                new X509CertificateStructure(tbsCert, sigAlgId, new DerBitString(signature)));
        }

        /// <summary>
        /// Allows enumeration of the signature names supported by the generator.
        /// </summary>
        public virtual IEnumerable SignatureAlgNames
        {
            get { return X509Utilities.GetAlgNames(); }
        }

        #region Unsupported V3 functions

        /// <summary>
        /// subjectUniqueId is not supported in X.509 V1 certificates
        /// </summary>
        public void SetSubjectUniqueID(bool[] uniqueID)
        {
            throw new InvalidOperationException("X.509 V1 certificates do not support subjectUniqueId");
        }

        /// <summary>
        /// issuerUniqueId is not supported in X.509 V1 certificates
        /// </summary>
        public void SetIssuerUniqueID(bool[] uniqueID)
        {
            throw new InvalidOperationException("X.509 V1 certificates do not support issuerUniqueId");
        }

        /// <summary>
        /// Extensions are not supported in X.509 V1 certificates.
        /// </summary>
        public void AddExtension(string oid, bool critical, Asn1Encodable extensionValue)
        {
            throw new InvalidOperationException("X.509 V1 certificates do not support extensions");
        }

        /// <summary>
        /// Extensions are not supported in X.509 V1 certificates.
        /// </summary>
        public void AddExtension(DerObjectIdentifier oid, bool critical, Asn1Encodable extensionValue)
        {
            throw new InvalidOperationException("X.509 V1 certificates do not support extensions");
        }

        /// <summary>
        /// Extensions are not supported in X.509 V1 certificates.
        /// </summary>
        public void AddExtension(string oid, bool critical, byte[] extensionValue)
        {
            throw new InvalidOperationException("X.509 V1 certificates do not support extensions");
        }

        /// <summary>
        /// Extensions are not supported in X.509 V1 certificates.
        /// </summary>
        public void AddExtension(DerObjectIdentifier oid, bool critical, byte[] extensionValue)
        {
            throw new InvalidOperationException("X.509 V1 certificates do not support extensions");
        }

        /// <summary>
        /// Extensions are not supported in X.509 V1 certificates.
        /// </summary>
        public void CopyAndAddExtension(string oid, bool critical, X509Certificate cert)
        {
            throw new InvalidOperationException("X.509 V1 certificates do not support extensions");
        }

        /// <summary>
        /// Extensions are not supported in X.509 V1 certificates.
        /// </summary>
        public void CopyAndAddExtension(DerObjectIdentifier oid, bool critical, X509Certificate cert)
        {
            throw new InvalidOperationException("X.509 V1 certificates do not support extensions");
        }

        /// <summary>
        /// Extensions are not supported in X.509 V1 certificates.
        /// </summary>
        public virtual X509Certificate Generate(CspParameters cspParam, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            throw new InvalidOperationException("X.509 V1 certificates do not support extensions");
        }

        #endregion

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

        #endregion
    }
}
