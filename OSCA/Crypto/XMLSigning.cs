using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Sys=System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace OSCA.Crypto
{
    /// <summary>
    /// Generic methods for signing and verifying XML signatures
    /// </summary>
    public static class XmlSigning
    {
        //
        //  !!!!!! Important - to support the experimental cngCA, signing/validation is bypassed if there is no key reference !!!!
        //
        #region SIGN

        /// <summary>
        /// Sign an XML document and save in a file
        /// </summary>
        /// <param name="XmlData">The XML data.</param>
        /// <param name="SignedFileName">Output filename</param>
        /// <param name="Cert">The cert.</param>
        /// <param name="PrivateKey">The private key.</param>
        public static void SignXml(XDocument XmlData, string SignedFileName, X509Certificate Cert, AsymmetricKeyParameter PrivateKey)
        {
            // Load the BC privateKey into a CspParameters object
            CspParameters cp = new CspParameters(24, "Microsoft Enhanced RSA and AES Cryptographic Provider");

            RSAParameters rp = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)PrivateKey);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp))
            {
                rsa.ImportParameters(rp);
                rsa.PersistKeyInCsp = true;
            }

            SignXml(XmlData, SignedFileName, Cert, cp);
        }


        /// <summary>
        /// Sign an XML document and save in a file
        /// </summary>
        /// <param name="xmlData">XML document</param>
        /// <param name="SignedFileName">Output filename</param>
        /// <param name="cert">The cert.</param>
        /// <param name="cspParam">The CSP parameter.</param>
        /// <exception cref="System.ArgumentNullException">
        /// xmlData
        /// or
        /// SignedFileName
        /// </exception>
        public static void SignXml(XDocument xmlData, string SignedFileName, X509Certificate cert, CspParameters cspParam)
        {
            if (null == xmlData)
                throw new ArgumentNullException("xmlData");
            if (null == SignedFileName)
                throw new ArgumentNullException("SignedFileName");

            // Convert the cert into system format
            Sys.X509Certificate2 sysCert = new Sys.X509Certificate2(cert.GetEncoded());

            // Create a new XML document.
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;

            doc.Load(xmlData.CreateReader());

            // Remove old signature if there is one
            XmlNodeList nodeList = doc.GetElementsByTagName("Signature");
            if (nodeList.Count > 0)
            {
                XmlNode xmlNode = nodeList[0];
                // Remove the signature node
                xmlNode.RemoveAll();
                doc.DocumentElement.RemoveChild(xmlNode);
            }

            if (cspParam != null)   // RSA/DSA signing key
            {

                // Create a SignedXml object.
                SignedXml signedXml = new SignedXml(doc);

                // Create a reference element for the signature.
                Reference reference = new Reference();
                reference.Uri = "";

                // Add an enveloped transformation to the reference.
                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(env);

                // Add the reference to the SignedXml object.
                signedXml.AddReference(reference);

                // Create a KeyInfo element.
                KeyInfo keyInfo = new KeyInfo();

                // Create KeyInfoX509Data subelement and configure
                // Load the certificate into the KeyInfoX509Data subelement
                KeyInfoX509Data kdata = new KeyInfoX509Data(sysCert);

                // Create an X509IssuerSerial element and add it to the
                // KeyInfoX509Data.
                X509IssuerSerial xserial = new X509IssuerSerial();
                xserial.IssuerName = sysCert.IssuerName.Name;
                xserial.SerialNumber = sysCert.SerialNumber;
                kdata.AddIssuerSerial(xserial.IssuerName, xserial.SerialNumber);

                // Add the KeyInfoX509Data subelement to the KeyInfo element
                keyInfo.AddClause(kdata);

                // Add the KeyInfo element to the SignedXml object.
                signedXml.KeyInfo = keyInfo;

                // Compute the signature
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParam))
                {
                    // Set the key to sign the Xml document 
                    signedXml.SigningKey = rsa;
                    // Compute the signature.
                    signedXml.ComputeSignature();
                }

                // Get the XML representation of the signature
                XmlElement xmlDigitalSignature = signedXml.GetXml();

                // Append the element to the XML document.
                doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

                // Get rid of the <?xml version.. in the document (it is added by the writer?)
                if (doc.FirstChild is XmlDeclaration)
                    doc.RemoveChild(doc.FirstChild);
            }

            // Save the signed XML document to the specified file.
            using (XmlTextWriter xmltw = new XmlTextWriter(SignedFileName, new UTF8Encoding(false)))
            {
                doc.WriteTo(xmltw);
                xmltw.Flush();
                xmltw.Close();
            }
        }

        #endregion

        #region VERIFY

        /// <summary>
        /// Verify the signature of an XML file
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <returns>True or False</returns>
        public static Boolean VerifyXmlFile(String fileName)
        {
            // Check the args.
            if (null == fileName)
                throw new ArgumentNullException("FileName");

            // Create a new XML document.
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;

            // check the file exists
            if (!(System.IO.File.Exists(fileName)))
            {
                throw new ArgumentNullException("File not found");
            }

            // Load the passed XML file into the document. 
            xmlDocument.Load(fileName);

            // Create a new SignedXml object and pass it
            // the XML document class.
            SignedXml signedXml = new SignedXml(xmlDocument);

            // Find the "Signature" node and create a new
            // XmlNodeList object.
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

            // nodeList shoud be empty if there is no sig block (cngCA case)
            if (nodeList.Count == 0)
                return true;

            // Load the signature node.
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.
            return signedXml.CheckSignature();
        }

        #endregion

        #region UNSIGN

        /// <summary>
        /// Remove the signature information from an XML file and return the result in a Stream
        /// </summary>
        /// <param name="sigFileName">XML pathname</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">File not found</exception>
        public static XmlReader UnSignXML(string sigFileName)
        {
            // check the file exists
            if (!(System.IO.File.Exists(sigFileName)))
            {
                throw new ArgumentNullException("File not found");
            }

            // Create a new XML document.
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;

            // Load the signed XML file into the document. 
            doc.Load(sigFileName);

            // get the signature node
            XmlNodeList nodeList = doc.GetElementsByTagName("Signature");

            // If nodeList is empty there is no sig - cngCA case
            if (nodeList.Count > 0)
            {
                XmlNode xmlNode = nodeList[0];

                // Remove the signature node
                xmlNode.RemoveAll();
                doc.DocumentElement.RemoveChild(xmlNode);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms);
                ms.Flush();
                return new XmlTextReader(ms);
            }
        }

        #endregion
    }
}
