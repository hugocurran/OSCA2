using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Org.BouncyCastle.X509;
using System.Security.Cryptography;
using OSCA.Crypto;

namespace OSCA.Log
{
    /// <summary>
    /// Create an OSCA log file
    /// </summary>
    internal static class LogFile
    {
        /// <summary>
        /// Create an OSCA log file
        /// </summary>
        /// <param name="logFile">Pathname of log file</param>
        /// <param name="version">Log system version</param>
        /// <param name="cert">XML signing certificate</param>
        /// <param name="cspParam">CSP parameters for signing key</param>
        internal static void createLogFile(string logFile, string version, X509Certificate cert, CspParameters cspParam)
        {
            XDocument log = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("CA Log File"),
                new XElement("OSCA",
                    new XAttribute("version", version),
                    new XElement("lastEvent", 0),
                    new XElement("events")
                )
             );
            // Sign and save the file
            XmlSigning.SignXml(log, logFile, cert, cspParam);
        }
    }
}
