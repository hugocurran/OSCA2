using System;
using System.Collections;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using OSCA.Offline;

namespace OSCA.Crypto
{
    /// <summary>
    /// Abstract class used to derive all V1 certificate generators
    /// </summary>
    public abstract class V1CertGen : ICertGen
    {
        /// <summary>
        /// The TBS gen
        /// </summary>
        protected V1TbsCertificateGenerator tbsGen;
        /// <summary>
        /// The sig oid
        /// </summary>
        protected DerObjectIdentifier sigOID;
        /// <summary>
        /// The sig alg identifier
        /// </summary>
        protected AlgorithmIdentifier sigAlgId;
        /// <summary>
        /// The signature algorithm
        /// </summary>
        protected string signatureAlgorithm;

        

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public V1CertGen()
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
        
        private X509ver version = X509ver.V1;
        /// <summary>
        /// The X509 version this certificate generator will create
        /// </summary>
        /// <returns></returns>
        public X509ver GetVersion()
        {
            return version;
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
        /// Allows enumeration of the signature names supported by the generator.
        /// </summary>
        //public virtual IEnumerable SignatureAlgNames;
        
                
    }
}
