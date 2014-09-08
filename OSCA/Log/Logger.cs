using System;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using OSCA.Crypto;

namespace OSCA.Log
{
    /// <summary>
    /// OSCA Event logger
    /// </summary>
    internal class Logger
    {    
        private string logFile;     // Pathname of logfile
        private int lastEvent;      // ID number of the last event written
        internal CspParameters cspParam;    // Reference to the signing key
        internal X509Certificate cert;      // Certificate for signature verification

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>Opens the event log and verifies the signature</remarks>
        /// <param name="logFile">Pathname of logfile</param>
        /// <param name="cert">Signing certificate</param>
        /// <param name="cspParam">Signing key</param>
        internal Logger(string logFile, X509Certificate cert, CspParameters cspParam)
        {
            this.logFile = logFile;
            this.cert = cert;
            this.cspParam = cspParam;
            if (XmlSigning.VerifyXmlFile(logFile))
            {
                XDocument log = XDocument.Load(logFile);
                lastEvent = Convert.ToInt32(log.Element("OSCA").Element("lastEvent").Value);
            }
            else
            {
                throw new GeneralSecurityException("Signature failure on log file");
            }
        }

        /// <summary>
        /// Write an event to the logfile
        /// </summary>
        /// <param name="id">Event type</param>
        /// <param name="message">Log message</param>
        internal void writeEvent(string id, string message)
        {
            // Update the lastEvent counter
            if (XmlSigning.VerifyXmlFile(logFile))
            {
                lastEvent++;
                //XDocument log = XDocument.Load(XmlSigning.UnSignXML(logFile));
                XmlReader foo = XmlSigning.UnSignXML(logFile);
                XDocument log = XDocument.Load(logFile);
                log.Element("OSCA").Element("lastEvent").ReplaceWith(new XElement("lastEvent", lastEvent.ToString()));

                // Write the event data
                XElement entry = new XElement("event",
                    new XAttribute("number", lastEvent.ToString()),
                    new XElement("time", DateTime.Now.ToUniversalTime()),
                    new XElement("id", id),
                    new XElement("message", message)
                    );
                XElement ep = log.Element("OSCA").Element("events");
                ep.Add(entry);

                // Sign and save the file
                XmlSigning.SignXml(log, logFile, cert, cspParam);
            }
            else
            {
                throw new GeneralSecurityException("Signature failure on log file");
            }
        }

    }
}
