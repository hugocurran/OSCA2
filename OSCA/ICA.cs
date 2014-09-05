/*
 * Copyright 2011-14 Peter Curran (peter@currans.eu). All rights reserved.
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
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace OSCA
{
    /// <summary>
    /// The OSCA CA interface
    /// </summary>
    /// <remarks>Always use this interface in preference to class methods etc.</remarks>
    public interface ICA
    {
        /// <summary>
        /// Serial number of the last CRL issued
        /// </summary>
        string LastCRLNumber { get; }

        /// <summary>
        /// Serial number of the last certificate issued
        /// </summary>
        string LastSerialNumber { get; }

        /// <summary>
        /// True if the CA is configured to use FIPS 140 mode
        /// </summary>
        bool FIPS140Mode { get; }

        /// <summary>
        /// Role of CA (rootCA | subCA)
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Distinguished name of the CA
        /// </summary>
        string CAName { get; }

        /// <summary>
        /// Algorithm and key size for the CA signing key
        /// </summary>
        string PublicKeyAlgorithm { get; }

        /// <summary>
        /// CA signature algorithm
        /// </summary>
        string SignatureAlgorithm { get; }

        /// <summary>
        /// CA Certificate
        /// </summary>
        X509Certificate Certificate { get; }

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request object
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Profile">OSCA Profile instance</param>
        /// <returns></returns>
        X509Certificate IssueCertificate(Pkcs10CertificationRequest Request, Profile.Profile Profile);

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request object
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Profile">OSCA Profile instance</param>
        /// <param name="NotBefore">The not before.</param>
        /// <param name="NotAfter">The not after.</param>
        /// <returns>X509 certificate</returns>
        X509Certificate IssueCertificate(Pkcs10CertificationRequest Request, Profile.Profile Profile, DateTime NotBefore, DateTime NotAfter);

        /// <summary>
        /// Issue a certificate based on an encoded PKCS#10 certificate request
        /// </summary>
        /// <param name="Request">PKCS#10 certificate request object</param>
        /// <param name="Profile">OSCA Profile instance</param>
        /// <returns>
        /// X509 certificate
        /// </returns>
        X509Certificate IssueCertificate(byte[] Request, Profile.Profile Profile);

        /// <summary>
        /// Issue a certificate based on an encoded PKCS#10 certificate request
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Validity">Validity period for the certificate</param>
        /// <returns>
        /// X509 certificate
        /// </returns>
        X509Certificate IssueCertificate(byte[] Request, ValidityPeriod Validity);

        /// <summary>
        /// Issue a certificate based on a PKCS#10 certificate request object
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Validity">Validity period for the certificate</param>
        /// <returns>
        /// X509 certificate
        /// </returns>
        X509Certificate IssueCertificate(Pkcs10CertificationRequest Request, ValidityPeriod Validity);

        /// <summary>
        /// Revoke a certificate
        /// </summary>
        /// <param name="Certificate">The certificate.</param>
        /// <param name="Reason">Reason for revocation</param>
        /// <returns>
        /// Status of the certificate
        /// </returns>
        string RevokeCertificate(X509Certificate Certificate, CRLReason Reason);

        /// <summary>
        /// Unrevoke certificate. (unsuspend)
        /// </summary>
        /// <param name="Certificate">The certificate.</param>
        /// <returns></returns>
        string UnRevokeCertificate(X509Certificate Certificate);

        /// <summary>
        /// Issue a CRL (containing all revoked certificates)
        /// </summary>
        /// <returns>CRL number</returns>
        string IssueCRL();

        /// <summary>
        /// Retrieve the current CRL
        /// </summary>
        /// <returns>Current CRL</returns>
        X509Crl GetCRL();

        /// <summary>
        /// Backup the CA key material to a PKCS#12 file
        /// </summary>
        /// <param name="Password">Strong password used for encryption</param>
        /// <param name="OutFile">The output file.</param>
        void Backup(string Password, string OutFile);

        /// <summary>
        /// Writes a 'CA Stopped' event to the log
        /// </summary>
        /// <remarks>To 'stop' the CA, delete the object</remarks>
        void StopCA();

    }
}
