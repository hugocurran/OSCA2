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
using System.Collections.Generic;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto.X509;

namespace OSCA.Profile
{
    internal static class ProfileExtensionFactory
    {
        // List of supported extensions
        private static Dictionary<DerObjectIdentifier, string> extTypes = new Dictionary<DerObjectIdentifier, string>() {
                {X509Extensions.AuthorityInfoAccess, "AuthorityInfoAccess"},
                {X509Extensions.BasicConstraints, "BasicConstraints"},
                {X509Extensions.CertificatePolicies, "CertificatePolicies"},
                {X509Extensions.CrlDistributionPoints, "CRLDistributionPoints"},
                {X509Extensions.InhibitAnyPolicy, "InhibitAnyPolicy"},
                {X509Extensions.IssuerAlternativeName, "IssuerAlternativeName"},
                {X509Extensions.SubjectKeyIdentifier, "SubjectKeyIdentifier"},
                {X509Extensions.KeyUsage, "KeyUsage"},
                {X509Extensions.ExtendedKeyUsage, "ExtendedKeyUsage"},
                {X509Extensions.NameConstraints, "NameConstraints"},
                {OcspNocheck.ocspNocheck, "OCSPNocheck"},
                {X509Extensions.PolicyConstraints, "PolicyConstraints"},
                {X509Extensions.PolicyMappings, "PolicyMappings"},
                {X509Extensions.SubjectAlternativeName, "SubjectAlternativeName"},
                {X509Extensions.SubjectDirectoryAttributes, "SubjectDirectoryAttributes"},
                {X509Extensions.SubjectInfoAccess, "SubjectInfoAccess"},
                {X509Extensions.FreshestCrl, "FreshestCRL"},
                {MsCaVersion.CaVersion, "CaVersion"},
                {MsCertTemplateInfo.CertTemplateInfo, "CertTemplateInfo"},
                {MsPrevCaCertHash.PrevHash, "PrevCaCertHash"},
                {MsCertTemplateName.CertTemplateName, "CertTemplateName"}
        };

        /// <summary>
        /// Return a ProfileExtension instance based on a passed-in X509Extension
        /// </summary>
        /// <param name="oid">OID of extension</param>
        /// <param name="extension">X509Extension</param>
        /// <returns>A ProfileExtension instance</returns>
        internal static ProfileExtension GetExtension(DerObjectIdentifier oid, X509Extension extension)
        {
            if (extTypes.ContainsKey(oid))
            {
                switch (extTypes[oid])
                {
                    case "AuthorityInfoAccess":
                        return new authorityInfoAccess(extension);

                    case "BasicConstraints":
                        return new basicConstraints(extension);

                    case "CertificatePolicies":
                        return new certificatePolicies(extension);

                    case "CRLDistributionPoints":
                        return new crlDistPoint(extension);

                    case "ExtendedKeyUsage":
                        return new extendedKeyUsage(extension);

                    case "InhibitAnyPolicy":
                        return new inhibitAnyPolicy(extension);

                    case "IssuerAlternativeName":
                        return new issuerAltName(extension);

                    case "SubjectKeyIdentifier":
                        return new subjectKeyIdentifier(extension);

                    case "KeyUsage":
                        return new keyUsage(extension);

                    case "NameConstraints":
                        return new nameConstraints(extension);

                    case "OCSPNocheck":
                        return new ocspNocheck(extension);

                    case "PolicyConstraints":
                        return new policyConstraints(extension);

                    case "PolicyMappings":
                        return new policyMappings(extension);

                    case "SubjectAlternativeName":
                        return new subjectAltName(extension);

                    //case "SubjectDirectoryAttributes":  
                    //    return new subjectDirectoryAttributes(extension);
                    //    break;

                    case "SubjectInfoAccess":
                        return new subjectInfoAccess(extension);

                    case "FreshestCRL":
                        return new freshestCRL(extension);

                    case "CaVersion":
                        return new caVersion(extension);

                    case "CertTemplateInfo":
                        return new certTemplateInfo(extension);

                    case "CertTemplateName":
                        return new certTemplateName(extension);

                    case "PrevCaCertHash":
                        return new prevCaCertHash(extension);

                    default:
                        throw new ArgumentOutOfRangeException("Unknown object in factory" + oid.ToString());
                }
            }
            else
            {
                return new unknownExtension(oid, extension);
            }
        }

        /// <summary>
        /// Return a ProfileExtension instance based on a passed-in XML value
        /// </summary>
        /// <param name="extension">XElement containing extension</param>
        /// <returns>A ProfileExtension instance</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws exception if extension not recognised</exception>
        internal static ProfileExtension GetExtension(XElement extension)
        {
            switch (extension.Element("name").Value)
            {
                case "BasicConstraints":
                    return new basicConstraints(extension);

                case "KeyUsage":
                    return new keyUsage(extension);

                case "ExtendedKeyUsage":
                    return new extendedKeyUsage(extension);

                case "CertificatePolicies":
                    return new certificatePolicies(extension);

                case "CrlDistributionPoints":
                    return new crlDistPoint(extension);

                case "SubjectAlternativeName":
                    return new subjectAltName(extension);

                case "IssuerAlternativeName":
                    return new issuerAltName(extension);

                case "SubjectKeyIdentifier":
                    return new subjectKeyIdentifier(extension);

                case "NameConstraints":
                    return new nameConstraints(extension);

                case "PolicyConstraints":
                    return new policyConstraints(extension);

                case "AuthorityInfoAccess":
                    return new authorityInfoAccess(extension);

                case "InhibitAnyPolicy":
                    return new inhibitAnyPolicy(extension);

                case "OcspNocheck":
                    return new ocspNocheck(extension);

                case "PolicyMappings":
                    return new policyMappings(extension);

                case "SubjectInfoAccess":
                    return new subjectInfoAccess(extension);

                case "FreshestCRL":
                    return new freshestCRL(extension);

                case "CaVersion":
                    return new caVersion(extension);

                case "CertTemplateInfo":
                    return new certTemplateInfo(extension);

                case "CertTemplateName":
                    return new certTemplateName(extension);

                case "PrevCaCertHash":
                    return new prevCaCertHash(extension);

                default:
                    throw new ArgumentOutOfRangeException("Unknown extension: " + extension.Element("name").Value);
            }
        }
    }
}
