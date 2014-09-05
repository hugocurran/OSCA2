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
using System.IO;
using System.Xml.Linq;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using osca = OSCA;  // Deconflict namespace issue (the snapin has initial class OSCA (see Installer.cs))
using OSCA.Crypto;
using OSCA.Profile;
using OSCA.Offline;
using System.Windows.Forms;        


namespace OSCASnapin.CAinfo
{

    /// <summary>
    ///  The current status of the CA object
    /// </summary>
    public enum CAstatus
    {
        Stopped,    // Object not loaded
        Running     // Object loaded
    }

    /// <summary>
    /// Status of a certificate
    /// </summary>
    public enum CertStatus
    {
        Current,
        Revoked,
        Expired,
        Suspended
    }

    /// <summary>
    /// This class encapsulates all information and controls for a CA.
    /// Its is intended to be passed around the MMC Snapin so that each view
    /// has access to the information it needs.
    /// <para>
    /// This class inherits from ICA but the info is either 'static' or derived from the actual CA
    /// </para>
    /// </summary>
    public class CaControl : osca.ICA
    {

        private osca.ICA ca = null;
        private CAstatus status = CAstatus.Stopped;
        private string configFileName;
        private string version;
        private string name; 
        private string role;        // Was called 'type' renamed in the V3 changes
        private CA_Type caType;
        private string created; 
        private string dbFileLocation; 
        private string publicKeyAlgorithm; 
        private string signatureAlgorithm; 
        private bool fips140; 
        private string lastSerial; 
        private string crlFileLocation; 
        private string lastCRL;
        private string crlInterval;
        private string profilesLocation;
        private X509Certificate certificate;
        private CaDB currentCerts;
        private CaDB revokedCerts;
        private CaDB expiredCerts;
        private profileDB profiles;

        #region constructor

        /// <summary>
        /// Load up the properties of the CA from the CA config file
        /// </summary>
        /// <param name="ConfigFileName">Pathname of the CA config file</param>
        public CaControl(string ConfigFileName)
        {
            configFileName = ConfigFileName;

            XDocument doc;
            if (XmlSigning.VerifyXmlFile(configFileName))
                doc = XDocument.Load(ConfigFileName);
            else
                throw new GeneralSecurityException("Signature failed on CA config file");
            
            XElement config = doc.Element("OSCA").Element("CA");
            version = doc.Element("OSCA").Attribute("version").Value;
            name = config.Element("name").Value;
            role = config.Element("type").Value;            
            created = config.Element("created").Value;
            dbFileLocation = config.Element("dbFileLocation").Value;
            publicKeyAlgorithm = config.Element("publicKeyAlgorithm").Value + "-" + config.Element("publicKeySize").Value;
            signatureAlgorithm = config.Element("signatureAlgorithm").Value;
            fips140 = Convert.ToBoolean(config.Element("fips140").Value);
            lastSerial = config.Element("lastSerial").Value;
            crlFileLocation = config.Element("crlFileLocation").Value;
            lastCRL = config.Element("lastCRL").Value;
            crlInterval = config.Element("crlInterval").Value;
            profilesLocation = config.Element("profilesLocation").Value;
            certificate = null;
            currentCerts = new CaDB(dbFileLocation, CertStatus.Current);
            revokedCerts = new CaDB(dbFileLocation, CertStatus.Revoked);
            expiredCerts = new CaDB(dbFileLocation, CertStatus.Expired);
            profiles = new profileDB(profilesLocation);

            if ((version == "3.0") || (version == "3.1"))
                caType = Utility.SetCA_Type(config.Element("caType").Value);
        }

        #endregion

        #region Public properties

        // Note that some properties use data from the CA if it is running ('this' is a local version, 'ca' is live)

        /// <summary>
        /// The current status of the CA
        /// </summary>
        public CAstatus CAStatus { get { return status; } }

        /// <summary>
        /// Pathname of the CA Databasefile
        /// </summary>
        public string DBFileLocation { get { return this.dbFileLocation; } }

        /// <summary>
        /// Period used to calculate the nextUpdate field of a CRL
        /// </summary>
        public string CRLInterval { get { return this.crlInterval + " Days"; } }

        /// <summary>
        /// Pathname of the last CRL issued by the CA
        /// </summary>
        public string CRLFileLocation { get { return this.crlFileLocation; } }

        /// <summary>
        /// Serial number of the last CRL issued by the CA
        /// </summary>
        public string LastCRLNumber { get { return (ca == null) ? this.lastCRL : ca.LastCRLNumber; } }

        /// <summary>
        /// Serial number of the last certificate issued by the CA
        /// </summary>
        public string LastSerialNumber { get { return (ca == null) ? this.lastSerial : ca.LastSerialNumber; } }

        /// <summary>
        /// True if the CA is configured to use FIPS 140 mode
        /// </summary>
        public bool FIPS140Mode { get { return (ca == null) ? this.fips140 : ca.FIPS140Mode; } }

        /// <summary>
        /// Role of CA (rootCA | subCA)
        /// </summary>
        public string Type { get { return (ca == null) ? this.role : ca.Type; } }

        /// <summary>
        /// Distinguished name of the CA
        /// </summary>
        public string CAName { get { return (ca == null) ? this.name : ca.CAName; } }

        /// <summary>
        /// Algorithm and key size for the CA signing key
        /// </summary>
        public string PublicKeyAlgorithm { get { return (ca == null) ? this.publicKeyAlgorithm : ca.PublicKeyAlgorithm; } }

        /// <summary>
        /// CA signature algorithm
        /// </summary>
        public string SignatureAlgorithm { get { return (ca == null) ? this.signatureAlgorithm : ca.SignatureAlgorithm; } }

        /// <summary>
        /// CA Certificate
        /// </summary>
        public X509Certificate Certificate { get { return (ca == null) ? this.certificate : ca.Certificate; } }

        /// <summary>
        /// Location of Profiles files
        /// </summary>
        public string ProfilesLocation { get { return this.profilesLocation; } }

        /// <summary>
        /// A List of the current certificates
        /// </summary>
        public List<DataBase> CurrentCerts { get { return this.currentCerts.Certs; } }

        /// <summary>
        /// A List of the revoked certificates
        /// </summary>
        public List<DataBase> RevokedCerts { get { return this.revokedCerts.Certs; } }

        /// <summary>
        /// A List of expired certificate
        /// </summary>
        public List<DataBase> ExpiredCerts { get { return this.expiredCerts.Certs; } }

        /// <summary>
        /// A List of the profiles used by the CA
        /// </summary>
        public List<ProfileDb> Profiles { get { return this.profiles.Profiles; } }

        #endregion

        #region CA interface

        /// <summary>
        /// 'Start' the CA by creating an instance of the CA
        /// </summary>
        /// <param name="Password"></param>
        public void StartCA(string Password)
        {
            if (CAStatus == CAstatus.Stopped)
            {
                try
                {
                    ca = osca.OSCA.LoadCA(configFileName, Password);
                    status = CAstatus.Running;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Problem starting the CA: " + ex.Message, "OSCA", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        /// <summary>
        /// 'Stop' the CA by de-rereferencing the CA instance
        /// </summary>
        public void StopCA()
        {
            if (CAStatus == CAstatus.Running)
            {
                // Update mutable fields
                this.lastSerial = ca.LastSerialNumber;
                this.lastCRL = ca.LastCRLNumber;
                ca.StopCA();  // event log
                ca = null;
                status = CAstatus.Stopped;
            }
        }
        
        /// <summary>
        /// Issue a certificate based on an encoded PKCS#10 certificate request
        /// </summary>
        /// <param name="request">Encoded PKCS#10 certificate request</param>
        /// <param name="Profile">Full pathname of certificate profile</param>
        /// <returns></returns>
        [Obsolete]
        public X509Certificate IssueCertificate(byte[] Request, Profile Profile)
        {
            X509Certificate cert = ca.IssueCertificate(Request, Profile);
            currentCerts.Refresh();
            return cert;
        }

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request object
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Profile">Profile instance</param>
        /// <returns></returns>
        public X509Certificate IssueCertificate(Pkcs10CertificationRequest Request, Profile Profile)
        {
            X509Certificate cert = ca.IssueCertificate(Request, Profile);
            currentCerts.Refresh();
            return cert;
        }

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request object
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Profile">Profile instance</param>
        /// <param name="NotBefore">The not before.</param>
        /// <param name="NotAfter">The not after.</param>
        /// <returns>
        /// X509 certificate
        /// </returns>
        public X509Certificate IssueCertificate(Pkcs10CertificationRequest Request, Profile Profile, DateTime NotBefore, DateTime NotAfter)
        {
            X509Certificate cert = ca.IssueCertificate(Request, Profile, NotBefore, NotAfter);
            currentCerts.Refresh();
            return cert;
        }

        /// <summary>
        /// Issue a certificate based on an encoded PKCS#10 certificate request
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Validity">Validity period for the certificate</param>
        /// <returns>
        /// X509 certificate
        /// </returns>
        public X509Certificate IssueCertificate(byte[] Request, osca.ValidityPeriod Validity)
        {
            X509Certificate cert = ca.IssueCertificate(Request, Validity);
            currentCerts.Refresh();
            return cert;
        }

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request object
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Validity">Validity period for the certificate</param>
        /// <returns>
        /// X509 certificate
        /// </returns>
        public X509Certificate IssueCertificate(Pkcs10CertificationRequest Request, osca.ValidityPeriod Validity)
        {
            X509Certificate cert = ca.IssueCertificate(Request, Validity);
            currentCerts.Refresh();
            return cert;
        }

        /// <summary>
        /// Revoke a certificate
        /// </summary>
        /// <param name="Certificate">The certificate.</param>
        /// <param name="Reason">Reason for revocation</param>
        /// <returns>
        /// Status of the certificate
        /// </returns>
        public string RevokeCertificate(X509Certificate Certificate, osca.CRLReason Reason)
        {
            string result = ca.RevokeCertificate(Certificate, Reason);
            currentCerts.Refresh();
            revokedCerts.Refresh();
            return result;
        }


        public string UnRevokeCertificate(X509Certificate Certificate)
        {
            string result = ca.UnRevokeCertificate(Certificate);
            currentCerts.Refresh();
            revokedCerts.Refresh();
            return result;
        }

        
        /// <summary>
        /// Issue a CRL (containing all revoked certificates)
        /// </summary>
        /// <returns>CRL number</returns>
        public string IssueCRL()
        {
            return ca.IssueCRL();
        }

        /// <summary>
        /// Retrieve the current CRL
        /// </summary>
        /// <returns>Current CRL</returns>
        public X509Crl GetCRL()
        {
            return ca.GetCRL();
        }

        /// <summary>
        /// Backup the CA key material to a PKCS#12 file
        /// </summary>
        /// <param name="Password">Strong password used for encryption</param>
        /// <param name="OutFile">Full pathname to the PKCS#12 output file</param>
        public void Backup(string Password, string OutFile)
        {
            ca.Backup(Password, OutFile);
        }

        #endregion

        #region CA Database

        /// <summary>
        /// Return a list of certs with a given status
        /// </summary>
        /// <param name="status">Status to search on</param>
        /// <returns>List of certificates matching search term</returns>
        public List<DataBase> GetCerts(CertStatus status)
        {
            switch (status)
            {
                case CertStatus.Current:
                    return CurrentCerts;
                case CertStatus.Revoked:
                    return RevokedCerts;
                case CertStatus.Expired:
                    return ExpiredCerts;
            }
            return null;
        }

        public void RefreshProfiles()
        {
            this.profiles = new profileDB(profilesLocation);
        }

        public void AddProfile(ProfileDb newEntry)
        {
            profiles.Add(newEntry);
        }

        public void RemoveProfile(ProfileDb entry)
        {
            File.Delete(entry.file);
            profiles.Remove(entry);
        }

        #endregion

    }
}
