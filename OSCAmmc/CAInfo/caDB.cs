/*
 * Copyright 2011-13 Peter Curran (peter@currans.eu). All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the 
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE FREEBSD PROJECT "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE FREEBSD PROJECT OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN 
 * IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the author alone. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Org.BouncyCastle.X509;
using OSCA;
using OSCA.Offline;
using OSCA.Crypto;
using Org.BouncyCastle.Security;

namespace OSCASnapin.CAinfo
{
    /// <summary>
    /// Database entry for each certificate
    /// </summary>
    public struct DataBase
    {
        public string dn;
        public string serialNumber;
        public string profile;
        public string created;  // NotBefore
        public string expiry;   // NotAfter
        public X509Certificate certificate;
        public string status;
        public string revDate;
        public string revReason;
    }

    /// <summary>
    /// An object containing information about certificates of a specified status
    /// </summary>
    public class CaDB
    {
        private List<DataBase> certs;
        private string dbLocation;
        private CertStatus certStatus;

        /// <summary>
        /// A list of certificates
        /// </summary>
        public List<DataBase> Certs { get { return certs; } }

        /// <summary>
        /// Construct a CA Database object by reading in all the data from the CA DB file
        /// and convert to a display-friendly format
        /// </summary>
        /// <param name="dbLocation">Pathname of the CA DB file</param>
        /// <param name="certStatus">Status of certificates to load</param>
        public CaDB(string DbLocation, CertStatus CertStatus)
        {
            this.dbLocation = DbLocation;
            this.certStatus = CertStatus;
            
            createDB();
        }


        /// <summary>
        /// Refresh the the certs database
        /// </summary>
        public void Refresh()
        {
            // Check that no certificates have expired
            //Database.ExpireCertificate(dbLocation);
            createDB();
        }

        private void createDB()
        {
            certs = new List<DataBase>();
            X509CertificateParser cp = new X509CertificateParser();

            XDocument db;
            if (XmlSigning.VerifyXmlFile(dbLocation))
                db = XDocument.Load(dbLocation);
            else
                throw new GeneralSecurityException("Signature failure on db file");

            // Select records of the appropriate status
            var records = db.Element("OSCA").Descendants("record").Where
                (m => m.Element("revocation").Attribute("status").Value == certStatus.ToString().ToLower());

            foreach (XElement record in records)
            {
                DataBase entry = new DataBase();

                entry.dn = Utility.OrderDN(record.Element("dn").Value);
                entry.serialNumber = record.Element("serialNumber").Value;
                entry.profile = record.Element("profile").Value;
                entry.created = friendlyDate(record.Element("created").Value);
                entry.expiry = friendlyDate(record.Element("expiry").Value);
                entry.certificate = cp.ReadCertificate(Convert.FromBase64String(record.Element("certificate").Value));
                entry.status = record.Element("revocation").Attribute("status").Value;


                if (certStatus == CertStatus.Revoked)
                {
                    entry.revDate = friendlyDate(record.Element("revocation").Element("date").Value);
                    entry.revReason = CrlReason.GetReason(record.Element("revocation").Element("reason").Value);
                }
                certs.Add(entry);
            }
        }


        private string friendlyDate(string dateString)
        {
            DateTime date = DateTime.Parse(dateString);
            return date.ToString("u");
        }
    }
}
