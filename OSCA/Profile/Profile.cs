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
using System.IO;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using OSCA.Crypto;


namespace OSCA.Profile
{
    /// <summary>
    /// Class describing an OSCA profile
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// X509 extensions supported by OSCA
        /// </summary>
        /// <remarks>A subset of the PKIX extensions and known private extensions</remarks>
        public static string[] OSCAExtensions = { "Authority Information Access",
                                                  "Basic Constraints",
                                                  "Certificate Policies",
                                                  "CRL Distribution Points",
                                                  "Extended Key Usage",
                                                  "Inhibit AnyPolicy",
                                                  "Issuer Alternative Name",
                                                  "Key Usage",
                                                  "Name Constraints",
                                                  "OCSP Nocheck",
                                                  "Policy Constraints",
                                                  "Policy Mappings",
                                                  "Subject Alternative Name",
                                                  //"Subject Directory Attributes",
                                                  "Subject Information Access",
                                                  "Freshest CRL",
                                                  "CA Version",
                                                  "Certificate Template Information"};
        #region Private Properties

        /// <summary>
        /// The extensions
        /// </summary>
        protected List<ProfileExtension> extensions = new List<ProfileExtension>();   // Extensions in the profile

        #endregion

        #region Public Properties

        /// <summary>
        /// Name of the profile
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Version of the profile
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Description of profile
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the certificate lifetime.
        /// </summary>
        /// <value>
        /// The certificate lifetime.
        /// </value>
        public ValidityPeriod CertificateLifetime { get; set; }
        /// <summary>
        /// Gets or sets the renewal overlap period.
        /// </summary>
        /// <value>
        /// The renew overlap period.
        /// </value>
        public ValidityPeriod RenewOverlapPeriod { get; set; }
        /// <summary>
        /// Read-only list of extensions
        /// </summary>
        /// <remarks>To change the list use the Add/Remove methods</remarks>
        public IList<ProfileExtension> Extensions { get { return extensions.AsReadOnly(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Create an empty profile instance
        /// </summary>
        public Profile() { }

        /// <summary>
        /// Create a Profile instance from an X509Extensions object
        /// </summary>
        /// <param name="Extensions">X509Extensions object</param>
        public Profile(X509Extensions Extensions)
        {
            foreach (DerObjectIdentifier oid in Extensions.ExtensionOids)
            {
                extensions.Add(ProfileExtensionFactory.GetExtension(oid, Extensions.GetExtension(oid)));
            }
        }

        /// <summary>
        /// Create a Profile instance from an OSCA XML profile file
        /// </summary>
        /// <param name="ProfileFile">Pathname of profile file</param>
        /// <exception cref="ArgumentException">If file does not exist</exception>
        /// <remarks>
        /// Generic layout of an OSCA XML profile description:
        /// <code>
        /// &lt;?xml version="1.0" encoding="UTF-8" standalone="true"?&gt;
        /// &lt;OSCA version="2.1"&gt;
        ///   &lt;Profile>&gt;
        ///     &lt;name&gt;Profile Name&lt;/name&gt;
        ///     &lt;version&gt;1.0&lt;/version&gt;
        ///     &lt;description&gt;Profile description&lt;/description&gt;
        ///     &lt;lifetime units="Years"&gt;1&lt;/lifetime&gt;
        ///     &lt;renewOverlap units="Months"&gt;1&lt;/renewOverlap&gt;
        ///     &lt;Extensions&gt;
        ///       ...
        ///     &lt;/Extensions&gt;
        ///   &lt;/Profile&gt;
        /// &lt;/OSCA&gt;
        /// </code>
        /// </remarks>
        public Profile(string ProfileFile)
        {
            // Check the profile file exists
            if (!File.Exists(ProfileFile))
                throw new ArgumentException("Profile file not found: " + ProfileFile);

            // Read in the profile
            XDocument profile = XDocument.Load(ProfileFile);
            loadProfile(profile);        
        }

        /// <summary>
        /// Create a Profile instance from an XML Document
        /// </summary>
        /// <param name="Profile">XDocument containing profile</param>
        public Profile(XDocument Profile)
        {
            loadProfile(Profile);
        }

        #endregion

        #region Add/Remove extensions

        /// <summary>
        /// Adds an extension to the profile
        /// </summary>
        /// <param name="Extension">The extension to add.</param>
        /// <exception cref="System.ArgumentException">Duplicate extension entry</exception>
        public void AddExtension(ProfileExtension Extension)
        {
            foreach (ProfileExtension ext in extensions)
            {
                if (Extension.OID.Equals(ext.OID))
                    throw new ArgumentException("Duplicate extension entry", "Extension");
            }
            extensions.Add(Extension);
        }

        /// <summary>
        /// Removes an extension.
        /// </summary>
        /// <param name="Extension">The extension to remove.</param>
        public void RemoveExtension(ProfileExtension Extension)
        {
            extensions.Remove(Extension);
        }

        #endregion

        #region Manage XML profile descriptions

        /// <summary>
        /// Loads the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        protected virtual void loadProfile(XDocument profile)
        {
            XElement baseInfo = profile.Element("OSCA").Element("Profile");

            // Extract the base information
            Name = baseInfo.Element("name").Value;
            Description = baseInfo.Element("description").Value;
            Version = baseInfo.Element("version").Value;
            CertificateLifetime = new ValidityPeriod(baseInfo.Element("lifetime"));

            if (baseInfo.Element("renewOverlap") != null)
                RenewOverlapPeriod = new ValidityPeriod(baseInfo.Element("renewOverlap"));
            else
                RenewOverlapPeriod = new ValidityPeriod(ValidityPeriod.Unit.Hours, 0);

            // Populate the list of Extensions
            IEnumerable<XElement> exts = baseInfo.Element("Extensions").Elements("Extension");
            foreach (XElement ext in exts)
            {
                extensions.Add(ProfileExtensionFactory.GetExtension(ext));
            }
        }

        /// <summary>
        /// Generate a XML description of the profile
        /// </summary>
        /// <returns>XML description as an XDocument</returns>
        /// <remarks>
        /// Generic layout of an OSCA XML profile description:
        /// <code>
        /// &lt;?xml version="1.0" encoding="UTF-8" standalone="true"?&gt;
        /// &lt;OSCA version="2.1"&gt;
        ///   &lt;Profile>&gt;
        ///     &lt;name&gt;Profile Name&lt;/name&gt;
        ///     &lt;version&gt;1.0&lt;/version&gt;
        ///     &lt;description&gt;Profile description&lt;/description&gt;
        ///     &lt;lifetime units="Years"&gt;1&lt;/lifetime&gt;
        ///     &lt;renewOverlap units="Months"&gt;1&lt;/renewOverlap&gt;
        ///     &lt;Extensions&gt;
        ///       ...
        ///     &lt;/Extensions&gt;
        ///   &lt;/Profile&gt;
        /// &lt;/OSCA&gt;
        /// </code>
        /// </remarks>
        public XDocument ToXml()
        {
            // Create the generic profile
            XDocument profile = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("OSCA",
                    new XAttribute("version", "2.1"),
                    new XElement("Profile",
                        new XElement("name", Name),
                        new XElement("version", Version),
                        new XElement("description", Description),
                        CertificateLifetime.ToXml("lifetime"),
                        RenewOverlapPeriod.ToXml("renewOverlap"),
                        new XElement("Extensions")
                    )
                )
            );

            // Add in extension data for each item in the list
            var insert = profile.Element("OSCA").Element("Profile").Element("Extensions");
            foreach (ProfileExtension extension in extensions)
            {
                insert.Add(extension.ToXml());
            }
            return profile;
        }

        /// <summary>
        /// Save a profile to an XML file
        /// </summary>
        /// <param name="Folder">Folder to save into</param>
        public string SaveXml(string Folder)
        {
            string filename = Folder + "\\" + Name + ".xml";
            XDocument profile = ToXml();
            profile.Save(filename);
            return filename;
        }

        #endregion

        #region Create profile from a PKCS#10 request

        /// <summary>
        /// Create an OSCA Profile using a PKCS#10 Certificate Request
        /// </summary>
        /// <param name="Request">PKCS#10 Certificate Request</param>
        /// <returns></returns>
        public static Profile FromPkcs10Request(Pkcs10CertificationRequest Request)
        {
            Pkcs10Parser parser = new Pkcs10Parser(Request);
            return new Profile(parser.Extensions);
        }

        #endregion

        #region Create profile from a X509 certificate
        /// <summary>
        /// Create an OSCA Profile using an X509 Certificate
        /// </summary>
        /// <param name="Certificate">X509 Certificate</param>
        /// <returns></returns>
        public static Profile FromX509Certificate(X509Certificate Certificate)
        {
            return new Profile(Certificate.GetExtensions());
        }

        #endregion
    }
}
