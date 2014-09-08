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

using System;
using System.Linq;
using System.Xml.Linq;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using System.Security.Cryptography;
using OSCA.Crypto;
using Org.BouncyCastle.Security;
using System.Collections.Generic;


namespace OSCA.Offline
{
    /// <summary>
    /// Class to support access to the OSCA database
    /// </summary>
    public static class Database
    {
        #region Create DB

        /// <summary>
        /// Create a CA Database file
        /// </summary>
        /// <param name="config">CA Config object</param>
        /// <param name="cert">CA certificate</param>
        /// <param name="cspParam">CSP with signing key</param>
        /// <returns>Location of the DB file</returns>
        internal static string CreateDB(CAConfig config, X509Certificate cert, CspParameters cspParam)
        {
            string dbfile = config.location + "\\CADatabase.xml";

            return CreateDB(dbfile, cert, cspParam);
        }

        /// <summary>
        /// Create a CA Database file
        /// </summary>
        /// <param name="dbFile">Database file location</param>
        /// <param name="cert">CA certificate</param>
        /// <param name="cspParam">CSP with signing key</param>
        /// <returns>Location of the DB file</returns>
        internal static string CreateDB(string dbFile, X509Certificate cert, CspParameters cspParam)
        {
            XDocument db = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("CA Database"),
                new XElement("OSCA",
                    new XAttribute("version", "1.0")
                )
            );
            
            // Sign and save the file
            XmlSigning.SignXml(db, dbFile, cert, cspParam);

            return dbFile;
        }

        #endregion

        #region AddCertificate

        /// <summary>
        /// Add a certificate to a CA Database
        /// </summary>
        /// <param name="cert">Certificate to add</param>
        /// <param name="request">PKCS#10 certificate request</param>
        /// <param name="profile">Name of profile used to create certificate</param>
        /// <param name="dbFile">Location of the Database file</param>
        /// <param name="caCert">The ca cert.</param>
        /// <param name="cspParam">The CSP parameter.</param>
        internal static void AddCertificate(X509Certificate cert,
                                          byte[] request,
                                          string profile,
                                          string dbFile,
                                          X509Certificate caCert,
                                          CspParameters cspParam)
        {

            XElement entry = new XElement("record",
                new XElement("dn", cert.SubjectDN),
                new XElement("serialNumber", cert.SerialNumber),
                new XElement("profile", profile),
                new XElement("created", cert.NotBefore.ToUniversalTime()),
                new XElement("expiry", cert.NotAfter.ToUniversalTime()),
                new XElement("certificate", Convert.ToBase64String(cert.GetEncoded())),
                new XElement("request", Convert.ToBase64String(request)),
                new XElement("revocation",
                    new XAttribute("status", "current")
                )
            );

            if (XmlSigning.VerifyXmlFile(dbFile))
            {
                XDocument doc = XDocument.Load(dbFile);
                XElement ep = doc.Element("OSCA");
                ep.Add(entry);

                // Sign and save the file
                XmlSigning.SignXml(doc, dbFile, caCert, cspParam);
            }
            else
            {
                throw new GeneralSecurityException("Signature failure on database file");
            }
        }

        #endregion

        #region Get a certificate

        /// <summary>
        /// Get a certificate from the database based on serial number search
        /// </summary>
        /// <param name="SerialNumber">Serial number</param>
        /// <param name="DbFileLocation">Location of the database file</param>
        /// <returns>X.509 certificate or null if not found</returns>
        /// <exception cref="GeneralSecurityException">Signature failure on database file</exception>
        /// <exception cref="ApplicationException">DB corrupt: Duplicate certificate entries.</exception>
        public static X509Certificate GetCertificate(BigInteger SerialNumber, string DbFileLocation)
        {
            string serialNumber = SerialNumber.ToString();

            return GetCertificate(serialNumber, DbFileLocation);
        }

        /// <summary>
        /// Get a certificate from the database based on serial number search
        /// </summary>
        /// <param name="SerialNumber">Serial number</param>
        /// <param name="DbFileLocation">Location of the database file</param>
        /// <returns>X.509 certificate or null if not found</returns>
        /// <exception cref="GeneralSecurityException">Signature failure on database file</exception>
        /// <exception cref="ApplicationException">DB corrupt: Duplicate certificate entries.</exception>
        public static X509Certificate GetCertificate(string SerialNumber, string DbFileLocation)
        {
            XElement record = findCertBySerialNumber(SerialNumber, DbFileLocation);

            X509CertificateParser cp = new X509CertificateParser();
            try
            {
                return cp.ReadCertificate(Convert.FromBase64String(record.Element("certificate").Value));
            }
            catch (InvalidParameterException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get a certificate from the database based on a Subject Key ID search
        /// </summary>
        /// <remarks>Finds the first certificate with a matching KeyID</remarks>
        /// <param name="KeyID">The KeyID to search for</param>
        /// <param name="IncludeRevoked">True to include revoked and expired certificates</param>
        /// <param name="DbFileLocation">Location of the database file</param>
        /// <returns>Matching certificate</returns>
        /// <exception cref="GeneralSecurityException">Signature failure on database file</exception>
        public static X509Certificate GetCertificate(byte[] KeyID, bool IncludeRevoked, string DbFileLocation)
        {
            return findCertBySubjKeyId(KeyID, IncludeRevoked, DbFileLocation);
        }

        #endregion

        #region RevokeCertificate

        /// <summary>
        /// Mark a certificate as revoked
        /// </summary>
        /// <param name="cert">Certificate to revoke</param>
        /// <param name="reason">Revocation reason</param>
        /// <param name="dbFileLocation">Location of CA DB file</param>
        /// <param name="caCert">The ca cert.</param>
        /// <param name="cspParam">The CSP parameter.</param>
        /// <exception cref="ApplicationException">Certificate already revoked or expired</exception>
        internal static void RevokeCertificate(X509Certificate cert, 
                                               CRLReason reason, 
                                               string dbFileLocation,
                                               X509Certificate caCert,
                                               CspParameters cspParam)
        {
            XDocument db;
            if (XmlSigning.VerifyXmlFile(dbFileLocation))
                db = XDocument.Load(dbFileLocation);
            else
                throw new GeneralSecurityException("Signature failure on database file");

            // Find the cert
            string serialNumber = cert.SerialNumber.ToString();
            XElement record = findCertBySerialNumber(serialNumber, db);

            // Check that certificate is current
            if (record.Element("revocation").Attribute("status").Value != "current")
                throw new ApplicationException("Certificate already revoked or expired: " + serialNumber);

            // Create new revocation element
            XElement revoked = new XElement("revocation",
                new XAttribute("status", "revoked"),
                new XElement("date", DateTime.Now),
                new XElement("reason", (int)reason)
             );

            // Replace the revocation record in the database
            record.Element("revocation").ReplaceWith(revoked);

            // Sign and save the database
            XmlSigning.SignXml(db, dbFileLocation, caCert, cspParam);
        }

        /// <summary>
        /// Unrevokes the specified cert.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <param name="dbFileLocation">The database file location.</param>
        /// <param name="caCert">The ca cert.</param>
        /// <param name="cspParam">The CSP parameter.</param>
        /// <exception cref="GeneralSecurityException">Signature failure on database file</exception>
        /// <exception cref="System.ApplicationException">Certificate not revoked:  + serialNumber</exception>
        internal static void unrevoke(X509Certificate cert,
                                               string dbFileLocation,
                                               X509Certificate caCert,
                                               CspParameters cspParam)
        {
            XDocument db;
            if (XmlSigning.VerifyXmlFile(dbFileLocation))
                db = XDocument.Load(dbFileLocation);
            else
                throw new GeneralSecurityException("Signature failure on database file");

            // Find the cert
            string serialNumber = cert.SerialNumber.ToString();
            XElement record = findCertBySerialNumber(serialNumber, db);

            // Check that certificate is current
            if (record.Element("revocation").Attribute("status").Value != "revoked")
                throw new ApplicationException("Certificate not revoked: " + serialNumber);

            // Create new revocation element
            XElement revoked = new XElement("revocation",
                new XAttribute("status", "current")
             );

            // Replace the revocation record in the database
            record.Element("revocation").ReplaceWith(revoked);

            // Sign and save the database
            XmlSigning.SignXml(db, dbFileLocation, caCert, cspParam);

        }

        #endregion

        #region Certificate status

        /// <summary>
        /// Status of a certificate
        /// </summary>
        public enum CertStatus
        {
            /// <summary>
            /// Not expired or revoked
            /// </summary>
            Current,
            /// <summary>
            /// Expired
            /// </summary>
            Expired,
            /// <summary>
            /// Revoked
            /// </summary>
            Revoked
        }

        /// <summary>
        /// Find the status of a certificate
        /// </summary>
        /// <param name="SerialNumber">Certificate serial number</param>
        /// <param name="DbFileLocation">DB file location</param>
        /// <returns>Certificate status</returns>
        /// <exception cref="ApplicationException">Certificate not found</exception>
        /// <exception cref="ApplicationException">Duplicate certificate</exception>
        /// <exception cref="ApplicationException">Invalid status value</exception>
        public static CertStatus GetCertificateStatus(BigInteger SerialNumber, string DbFileLocation)
        {
            string serialNumber = SerialNumber.ToString();

            return GetCertificateStatus(serialNumber, DbFileLocation);
        }

        /// <summary>
        /// Find the status of a certificate
        /// </summary>
        /// <param name="SerialNumber">Certificate serial number</param>
        /// <param name="DbFileLocation">DB file location</param>
        /// <returns>Certificate status</returns>
        /// <exception cref="ApplicationException">Certificate not found</exception>
        /// <exception cref="ApplicationException">Duplicate certificate</exception>
        /// <exception cref="ApplicationException">Invalid status value</exception>
        public static CertStatus GetCertificateStatus(string SerialNumber, string DbFileLocation)
        {
            XElement record = findCertBySerialNumber(SerialNumber, DbFileLocation);

            switch (record.Element("revocation").Attribute("status").Value)
            {
                case "current":
                    return CertStatus.Current;
                case "expired":
                    return CertStatus.Expired;
                case "revoked":
                    return CertStatus.Revoked;
                default:
                    throw new ApplicationException("DB corrupt: Invalid certificate status " + record.Element("revocation").Attribute("status").Value);
            }
        }

        #endregion

        #region Populate CRL

        /// <summary>
        /// Populate a CRL object with revocation entries
        /// </summary>
        /// <param name="Crl">Reference to to CRL object</param>
        /// <param name="DbFileLocation">Loaction of the CA Database file</param>
        internal static void PopulateCRL(X509V2CrlGenerator Crl, string DbFileLocation)
        {
            XDocument db;
            if (XmlSigning.VerifyXmlFile(DbFileLocation))
                db= XDocument.Load(DbFileLocation);
            else
                throw new GeneralSecurityException("Signature failure on database file");

            // Find all certificates where revocation status is revoked
            var records = db.Element("OSCA").Descendants("record").Where(m => m.Element("revocation").Attribute("status").Value == "revoked");


            // Iterate across records and add each certificate details to the CRL
            foreach (XElement record in records)
            {
                string certNum = record.Element("serialNumber").Value;
                DateTime revDate = Convert.ToDateTime(record.Element("revocation").Element("date").Value);
                int revReason = Convert.ToInt16(record.Element("revocation").Element("reason").Value);

                Crl.AddCrlEntry(new BigInteger(certNum), revDate, revReason);
            }
            return;
        }

        #endregion

        #region Expire Certificates

        /// <summary>
        /// Check the CA Database and mark expired certificates
        /// </summary>
        /// <param name="DbFileLocation">Location of CA DB file</param>
        /// <param name="caCert">The ca cert.</param>
        /// <param name="cspParam">The CSP parameter.</param>
        /// <returns></returns>
        /// <exception cref="GeneralSecurityException">Signature failure on database file</exception>
        internal static int ExpireCertificate(string DbFileLocation, X509Certificate caCert, CspParameters cspParam)
        {
            XDocument db;
            if (XmlSigning.VerifyXmlFile(DbFileLocation))
                db = XDocument.Load(DbFileLocation);
            else
                 throw new GeneralSecurityException("Signature failure on database file");

            // Find all certificates where revocation status is not expired
            var records = db.Element("OSCA").Descendants("record").Where(m => m.Element("revocation").Attribute("status").Value != "expired");

            // Iterate across records
            DateTime expiry;
            string status;
            int expiredRecordsCount = 0;
            foreach (XElement record in records)
            {
                expiry = Convert.ToDateTime(record.Element("expiry").Value);
                status = record.Element("revocation").Attribute("status").Value;
                if (expiry < DateTime.Now)
                {
                    // Create new revocation element
                    XElement expired = new XElement("revocation",
                        new XAttribute("status", "expired"),
                        new XElement("date", 0),
                        new XElement("reason", "")
                        );

                    // Replace the revocation record in the database
                    record.Element("revocation").ReplaceWith(expired);
                    expiredRecordsCount++;
                }
            }
                // Save the database if there are changes
                if (expiredRecordsCount > 0)
                    XmlSigning.SignXml(db, DbFileLocation, caCert, cspParam);

                return expiredRecordsCount;
        }

        #endregion

        #region Helper methods
        
        private static XElement findCertBySerialNumber(string serialNumber, string dbFileLocation)
        {
            // Open the DB file
            XDocument db;
            if (XmlSigning.VerifyXmlFile(dbFileLocation))
                db = XDocument.Load(dbFileLocation);
            else
                throw new GeneralSecurityException("Signature failure on database file");

            return findCertBySerialNumber(serialNumber, db);
        }

        private static XElement findCertBySerialNumber(string serialNumber, XDocument db)
        {
            var records = db.Element("OSCA").Descendants("record").Where(m => m.Element("serialNumber").Value == serialNumber);

            // Check one and only one entry returned
            if (records.Count() < 1)
                throw new InvalidParameterException("Certificate not found: " + serialNumber);
            if (records.Count() > 1)
                throw new ApplicationException("DB corrupt: Duplicate certificate entries.  SN:" + serialNumber);

            // Note that records is IEnumerable<XElement> so we have to iterate to get the (1) record
            XElement record = null;
            foreach (XElement _record in records)
                record = _record;


            return record;
        }

        private static X509Certificate findCertBySubjKeyId(byte[] keyId, bool inclRevoked, string dbFileLocation)
        {
            // Open the DB file
            XDocument db;
            if (XmlSigning.VerifyXmlFile(dbFileLocation))
                db = XDocument.Load(dbFileLocation);
            else
                throw new GeneralSecurityException("Signature failure on database file");

            return findCertBySubjKeyId(keyId, inclRevoked, db);
        }

        private static X509Certificate findCertBySubjKeyId(byte[] keyId, bool inclRevoked, XDocument db)
        {
            IEnumerable<XElement> records;
            if (inclRevoked)
                records = db.Element("OSCA").Descendants("record");
            else
                records = db.Element("OSCA").Descendants("record").Where(m => m.Element("serialNumber").Attribute("status").Value == "current");

            //XElement record = null;
            X509CertificateParser cp = new X509CertificateParser();
            X509Certificate cert;
            
            foreach (XElement record in records)
            {
                cert = cp.ReadCertificate(Convert.FromBase64String(record.Element("certificate").Value));
                try
                {
                    if (Utility.TestKeyId(keyId, cert, Utility.KeyIdType.SKID))
                        return cert;
                }
                catch (ApplicationException) { }  // Only interested in positive result
            }
            return null;
        }


        #endregion
    }
}

