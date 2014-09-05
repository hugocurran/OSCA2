using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Xml;
using System;

namespace OSCA.Crypto
{

    public partial class DigitalSignatureManager
    {
        #region UNSIGN

        /// <summary>
        /// Remove the signature information from an XML file and return the result in a Stream
        /// </summary>
        /// <param name="sigFileName">XML pathname</param>
        /// <param name="ms">Reference to a memory stream</param>
        public static void UnSignXML(string sigFileName, ref MemoryStream ms)
        {
            // check the file exists
            if (!(System.IO.File.Exists(sigFileName)))
            {
                LogManager.Log(LOG_FILE.DRCA_BL, "DigitalSignatureManager.UnsignXML(): File not found: " + sigFileName);
                throw new ArgumentNullException("File not found");
            }

            // Create a new XML document.
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;

            // Load the signed XML file into the document. 
            doc.Load(sigFileName);

            // get the signature node
            XmlNodeList nodeList = doc.GetElementsByTagName("Signature");
            XmlNode xmlNode = nodeList[0];

            // Remove the signature node
            xmlNode.RemoveAll();
            doc.DocumentElement.RemoveChild(xmlNode);

            // Save the signed XML data to the stream
            XmlTextWriter xmltw = new XmlTextWriter(ms, new UTF8Encoding(false));
            doc.WriteTo(xmltw);
            xmltw.Flush();
            LogManager.Log(LOG_FILE.DRCA_BL, "DigitalSignatureManager.UnSignXML(): Loaded file " + sigFileName);
        }

        #endregion

        #region SIGN
        /// <summary>
        /// Read XML from a stream, sign it and save in a file
        /// </summary>
        /// <param name="xmlData">Input XML stream</param>
        /// <param name="SignedFileName">Output filename</param>
        public void SignXml(Stream xmlData, string SignedFileName)
        {
            DRCAUser user = DRCAUser.UniqueInstance;
            SignXml(xmlData, SignedFileName, user.Cert);
        }

        /// <summary>
        /// Read XML from a stream, sign it and save in a file
        /// </summary>
        /// <param name="xmlData">Input XML stream</param>
        /// <param name="SignedFileName">Output filename</param>
        public void SignXml(Stream xmlData, string SignedFileName, X509Certificate2 cert)
        {
            if (null == xmlData)
                throw new ArgumentNullException("xmlData");
            if (null == SignedFileName)
                throw new ArgumentNullException("SignedFileName");

            // Create a new XML document.
            XmlDocument doc = new XmlDocument();

            // Format the document to ignore white spaces.
            //doc.PreserveWhitespace = false;
            doc.PreserveWhitespace = true;

            // Reset the stream pointer to 0
            xmlData.Position = 0;
            doc.Load(xmlData);

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(doc);

            // Set the key to signed the Xml document 
            signedXml.SigningKey = cert.PrivateKey;

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
            KeyInfoX509Data kdata = new KeyInfoX509Data(cert);

            // Create an X509IssuerSerial element and add it to the
            // KeyInfoX509Data.
            X509IssuerSerial xserial = new X509IssuerSerial();
            xserial.IssuerName = cert.IssuerName.ToString();
            xserial.SerialNumber = cert.SerialNumber;
            kdata.AddIssuerSerial(xserial.IssuerName, xserial.SerialNumber);

            // Add the KeyInfoX509Data subelement to the KeyInfo element
            keyInfo.AddClause(kdata);

            // Add the KeyInfo element to the SignedXml object.
            signedXml.KeyInfo = keyInfo;

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the XML document.
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

            // Get rid of the <?xml version.. in the document (it is added by the writer?)
            if (doc.FirstChild is XmlDeclaration)
            {
                doc.RemoveChild(doc.FirstChild);
            }

            // Save the signed XML document to the specified file.
            XmlTextWriter xmltw = new XmlTextWriter(SignedFileName, new UTF8Encoding(false));
            using (xmltw)
            {
                doc.WriteTo(xmltw);
                xmltw.Close();
            }
            LogManager.Log(LOG_FILE.DRCA_BL, "DigitalSignatureManager.SignXML(): created file " + SignedFileName);
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
                LogManager.Log(LOG_FILE.DRCA_BL, "DigitalSignatureManager.VerifyXMLFile(): File not found: " + fileName);
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

            // Load the signature node.
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.
            bool valid = false;
            try
            {
                valid = signedXml.CheckSignature();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (valid)
                LogManager.Log(LOG_FILE.DRCA_BL, "DigitalSignatureManager.VerifyXMLFile(): " + fileName + " verifed OK");
            else
            {
                LogManager.Log(LOG_FILE.DRCA_BL, "DigitalSignatureManager.VerifyXMLFile(): " + fileName + " failed verify", System.Diagnostics.EventLogEntryType.Error);
                LogManager.AppEventLog("DigitalSignatureManager.VerifyXMLFile(): " + fileName + " failed verify", System.Diagnostics.EventLogEntryType.FailureAudit, 0);
            }
            return valid; 
        }

        #endregion

        /*
        #region GET CERT

        /// <summary>
        /// Searches the certificate store for the certificate subject 
        /// </summary>
        /// <param name="CertificateSubject"></param>
        /// <returns></returns>
        public static X509Certificate2 GetCertificateBySubject(string CertificateSubject)
        {
            if (CertificateSubject == string.Empty)
            {
                return null;
            }

            // Check the args.
            if (CertificateSubject == null)
                throw new ArgumentNullException("CertificateSubject");


            // Load the certificate from the certificate store.
            X509Certificate2 cert = null;
            X509Store store = new X509Store("My", StoreLocation.CurrentUser);

            try
            {
                // Open the store.
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                // Get the certs from the store.
                X509Certificate2Collection CertCol = store.Certificates;

                // Find the certificate with the specified subject.
                foreach (X509Certificate2 c in CertCol)
                {
                    if (c.Subject == CertificateSubject)
                    {
                        cert = c;
                        break;
                    }
                }
                if (cert == null)
                {
                    throw new CryptographicException("The certificate could not be found.");
                }
            }
            finally
            {
                // Close the store even if an exception was thrown.
                store.Close();
            }
            return cert;
        }
        #endregion
        */
    }
}
