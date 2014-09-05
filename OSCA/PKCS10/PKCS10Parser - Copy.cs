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

using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using System.Collections.Generic;
using OSCA.Profile;
using System.Text;

namespace OSCA
{
    /// <summary>
    /// Generic PKCS#10 parser
    /// </summary>
    public class Pkcs10Parser
    {
        private Pkcs10CertificationRequest request;
        private CertificationRequestInfo info;
        private List<ProfileExtension> oscaExtensions = new List<ProfileExtension>();
        private Dictionary<DerObjectIdentifier, Asn1Set> attributes = new Dictionary<DerObjectIdentifier, Asn1Set>();

        #region Constructors

        /// <summary>
        /// Create a PKCS#10 Parser using a byte[]
        /// </summary>
        /// <param name="Request">byte[] containing request</param>
        /// <exception cref="SignatureException">POP test failed</exception>
        public Pkcs10Parser(byte[] Request)
        {
            this.request = new Pkcs10CertificationRequest(Request);
            readRequest();
        }

        /// <summary>
        /// Create a PKCS#10 Parser using a PEM object
        /// </summary>
        /// <param name="Request">PEM object containing request</param>
        /// <exception cref="SignatureException">POP test failed</exception>
        public Pkcs10Parser(PemObject Request)
        {
            this.request = new Pkcs10CertificationRequest(Request.Content);
            readRequest();
        }

        /// <summary>
        /// Create a PKCS#10 Parser using an Asn1Sequence
        /// </summary>
        /// <param name="Sequence">Asn1Sequence containing request</param>
        /// <exception cref="SignatureException">POP test failed</exception>
        public Pkcs10Parser(Asn1Sequence Sequence)
        {
            this.request = new Pkcs10CertificationRequest(Sequence);
            readRequest();
        }

        /// <summary>
        /// Create a PKCS#10 Parser using a Pkcs10CertificationRequest object
        /// </summary>
        /// <param name="Request">Pkcs10CertificationRequest object</param>
        /// <exception cref="SignatureException">POP test failed</exception>
        public Pkcs10Parser(Pkcs10CertificationRequest Request)
        {
            this.request = Request;
            readRequest();
        }

        private void readRequest()
        {
            // Perform POP on the request
            if (!request.Verify())
                throw new SignatureException("Invalid signature on PKCS#10 request");

            // Contents
            info = request.GetCertificationRequestInfo();

            // Extensions in OSCA format
            foreach (DerObjectIdentifier oid in Extensions.ExtensionOids)
            {
                oscaExtensions.Add(ProfileExtensionFactory.GetExtension(oid, Extensions.GetExtension(oid)));
            }

            // Attributes
            foreach (object entry in Attributes)
            {
                AttributePkcs attrib = AttributePkcs.GetInstance(entry);
                attributes.Add(attrib.AttrType, attrib.AttrValues);
            }
        }

        #endregion

        #region Public Variables

        /// <summary>
        /// Subject Name
        /// </summary>
        public X509Name Subject { get { return info.Subject; } }
        
        /// <summary>
        /// X509 Extensions
        /// </summary>
        public X509Extensions Extensions {get { return getExtensions(); } }

        /// <summary>
        /// OSCA Extensions
        /// </summary>
        public List<ProfileExtension> OscaExtensions { get { return oscaExtensions; } }

        /// <summary>
        /// Subject Alternative Names
        /// </summary>
        public GeneralNames SubjectAltNames { get { return getSubjectAltNames(); } }

        /// <summary>
        /// PKCS#10 version number
        /// </summary>
        public DerInteger Version {get { return info.Version; } }

        /// <summary>
        /// Subject Public Key Information
        /// </summary>
        public SubjectPublicKeyInfo SubjectPublicKeyInfo { get { return info.SubjectPublicKeyInfo; } }

        /// <summary>
        /// A string describing the public key
        /// </summary>
        public string SubjectPublicKeyDescription { get { return getSubjectPubKeyDescr(); } }

        /// <summary>
        /// Object containing the parameters of the public key
        /// </summary>
        public RsaPublicKeyStructure SubjectPublicKeyParams { get { return getSubjectPubKeyParams(); } }

        /// <summary>
        /// Signature Algorithm Information
        /// </summary>
        public DerObjectIdentifier SignatureAlgorithm { get { return request.SignatureAlgorithm.ObjectID; } }

        /// <summary>
        /// String describing the signature algorithm
        /// </summary>
        public string SignatureAlgorithmDescription { get { return Algorithms.oids[request.SignatureAlgorithm.ObjectID]; } }

        /// <summary>
        /// Public Key value
        /// </summary>
        public AsymmetricKeyParameter PublicKey { get { return request.GetPublicKey(); } }

        /// <summary>
        /// All of the attributes
        /// </summary>
        public Asn1Set AttributesSet { get { return info.Attributes; } }

        /// <summary>
        /// All of the attributes as a dictionary
        /// </summary>
        public Dictionary<DerObjectIdentifier, Asn1Set> Attributes { get { return attributes; } }
                        
        #endregion

        #region Public Methods

        /// <summary>
        /// State criticality of an extension
        /// </summary>
        /// <param name="oid">OID of extension</param>
        /// <returns>True=Critical, false=Noncritical</returns>
        public bool IsCritical(DerObjectIdentifier oid)
        {
            foreach(DerObjectIdentifier ext in Extensions.GetCriticalExtensionOids())
            {
                if(ext.Equals(oid))
                    return true;
            }
            return false;          
        }

        /// <summary>
        /// Describe the PKCS#10 request
        /// </summary>
        /// <returns>String description of the PKCS#10 request</returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append("PKCS#10 version ").Append(Version).AppendLine();
            output.Append("Subject: ").Append(Subject).AppendLine();
            output.Append("Public Key Info: ").Append(getSubjectPubKeyDescr()).AppendLine();
            output.AppendLine("Extensions:");
            foreach (ProfileExtension ext in oscaExtensions)
                output.Append("\t").Append(ext).AppendLine();
                
            //output = output + attributeString() + "\n";
            return output.ToString();
        }

        #endregion

        #region Get Subject Alternative Names

        /// <summary>
        /// Get a GeneralNames object containing the SubjectAltNames
        /// </summary>
        /// <returns>Subject Alt Names (or null)</returns>
        private GeneralNames getSubjectAltNames()
        {
            if (Extensions == null)
                return null;

            // Retrieve the SAN extension from the Extensions list
            X509Extension san = Extensions.GetExtension(X509Extensions.SubjectAlternativeName);

            if (san == null)
                return null;

            // Create a new GeneralNames object that includes the sequence of names
            return new GeneralNames((Asn1Sequence)san.GetParsedValue());
        }

        #endregion

        #region Get Extensions

        /// <summary>
        /// Get an X509Extensions object containing all extensions from the request
        /// </summary>
        /// <returns>List of extension (or null)</returns>
        private X509Extensions getExtensions()
        {
            if (Attributes == null)
                return null;

            DerObjectIdentifier ExtensionsOid = new DerObjectIdentifier("1.2.840.113549.1.9.14");

            // Iterate over the Attributes
            foreach (object entry in Attributes)
            {
                AttributePkcs attrib = AttributePkcs.GetInstance(entry);
                // Find the Attribute entry that has extensions in it
                if (ExtensionsOid.Equals(attrib.AttrType))
                {
                    X509ExtensionsGenerator gen = new X509ExtensionsGenerator();
                    bool critical;
                    foreach (DerSequence outer in attrib.AttrValues)
                        foreach (DerSequence inner in outer)
                        {
                            // Note that the extension value is wrapped in an OctetString, but the generator expects an unwrapped value
                            if (inner.Count == 3)       // Critical flag set
                            {
                                critical = isTrue((DerBoolean)inner[1]);  // Just in case it is false
                                gen.AddExtension((DerObjectIdentifier)inner[0], critical, ((DerOctetString)inner[2]).GetOctets());
                            }
                            else                       // Count==2; Critical flag not set
                            {
                                gen.AddExtension((DerObjectIdentifier)inner[0], false, ((DerOctetString)inner[1]).GetOctets());
                            }
                        }
                    return gen.Generate();
                }
            }
            return null;
        }

        #endregion

        #region Helpers

        private string attributeString()
        {
            string value = "";

            foreach (object entry in Attributes)
            {
                AttributePkcs attrib = AttributePkcs.GetInstance(entry);
                value = value + "OID :";
            }

            return value;
        }

        private bool isTrue(DerBoolean value)
        {
            return (value.ToString() == "TRUE") ? true : false;
        }

        private string getSubjectPubKeyDescr()
        {
            string value;
            // Algorithm
            AlgorithmIdentifier pkAlgo = SubjectPublicKeyInfo.AlgorithmID;
            value = Algorithms.keyAlgorithms[pkAlgo.ObjectID];

            // Parameters
            if (pkAlgo.Parameters is DerNull)
                value = value + " - NULL parameters";
            else
                value = value + " - with parameters";

            // Modulus size
            value = value + " - " + getSubjectPubKeyParams().Modulus.BitLength.ToString() + "-bit modulus";

            return value;
        }

        private RsaPublicKeyStructure getSubjectPubKeyParams()
        {
            byte[] pk = SubjectPublicKeyInfo.PublicKeyData.GetBytes();
            return RsaPublicKeyStructure.GetInstance(Asn1Sequence.GetInstance(pk));
        }


        #endregion

    }
}

