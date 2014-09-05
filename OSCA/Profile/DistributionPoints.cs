/*
 * Copyright 2014 Peter Curran (peter@currans.eu). All rights reserved.
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
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto;

namespace OSCA.Profile
{
    /// <summary>
    /// Abstract base class for Distribution Points extensions
    /// </summary>
    /// <remarks>ASN.1 description from RFC 5280
    /// <code>
    /// id-ce-cRLDistributionPoints OBJECT IDENTIFIER ::=  { id-ce 31 }
    /// 
    /// id-ce-freshestCRL OBJECT IDENTIFIER ::=  { id-ce 46 }
    /// 
    /// FreshestCRL ::= CRLDistributionPoints
    ///     
    /// CRLDistributionPoints ::= SEQUENCE SIZE (1..MAX) OF DistributionPoint
    /// 
    /// DistributionPoint ::= SEQUENCE {
    ///     distributionPoint       [0]     DistributionPointName OPTIONAL,
    ///     reasons                 [1]     ReasonFlags OPTIONAL,
    ///     cRLIssuer               [2]     GeneralNames OPTIONAL }
    ///     
    /// DistributionPointName ::= CHOICE {
    ///     fullName                [0]     GeneralNames,
    ///     nameRelativeToCRLIssuer [1]     RelativeDistinguishedName }
    ///     
    /// ReasonFlags ::= BIT STRING {
    ///     unused                  (0),
    ///     keyCompromise           (1),
    ///     cACompromise            (2),
    ///     affiliationChanged      (3),
    ///     superseded              (4),
    ///     cessationOfOperation    (5),
    ///     certificateHold         (6),
    ///     privilegeWithdrawn      (7),
    ///     aACompromise            (8) }
    /// </code>
    /// Note that this class does not support reasons or cRLIssuer in the DistributionPoint
    /// </remarks>
    public abstract class DistributionPoints : ProfileExtension
    {
        // Local structure to hold the list of names
        // Note that base.encValue holds the DER encoded version
        /// <summary>
        /// The dist points
        /// </summary>
        protected List<OSCAGeneralName> distPoints = new List<OSCAGeneralName>();


        /// <summary>
        /// Gets the dist points.
        /// </summary>
        /// <value>
        /// The dist points.
        /// </value>
        public List<OSCAGeneralName> DistPoints { get { return distPoints; } }

        /// <summary>
        /// Distribution points
        /// </summary>
        public GeneralNames GeneralNames { get { encode(); return (GeneralNames)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create extension using supplied values
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        protected DistributionPoints(bool Critical)
        {
            base.critical = Critical;
        }

        /// <summary>
        /// Create extension from XML profile file entry
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the FreshestCRL extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Freshest CRL"&gt;FreshestCRL&lt;/name&gt;
        ///     &lt;critical&gt;true&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;cdp&gt;
        ///             &lt;name type="uniformResourceIdentifier"&gt;http://crl.foo.org/wotsit.crl&lt;/name&gt;
        ///         &lt;/cdp&gt;
        ///         &lt;cdp&gt;
        ///             &lt;name type="uniformResourceIdentifier"&gt;http://www.bar.org/wotsit.crl&lt;/name&gt;
        ///         &lt;/cdp&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// Note that FreshestCRL and CRLDistributionPoint are processed the same.
        /// </remarks>
        /// <param name="xml">XML version of the extension</param>
        protected DistributionPoints(XElement xml) 
            : base(xml)
        {
            foreach (XElement name in xmlValue.Descendants("cdp"))
            {
                OSCAGeneralName dp = new OSCAGeneralName()
                {
                    Name = name.Value,
                    Type = generalNames.getGenName(name.Attribute("type").Value)
                };
                distPoints.Add(dp);
            }
        }
      
        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <remarks>
        /// Sample OSCA XML description of the extension:
        /// <code>
        /// &lt;Extension&gt;
        ///     &lt;name description="Freshest CRL"&gt;FreshestCRL&lt;/name&gt;
        ///     &lt;critical&gt;true&lt;/critical&gt;
        ///     &lt;value&gt;
        ///         &lt;cdp&gt;
        ///             &lt;name type="uniformResourceIdentifier"&gt;http://crl.foo.org/wotsit.crl&lt;/name&gt;
        ///         &lt;/cdp&gt;
        ///         &lt;cdp&gt;
        ///             &lt;name type="uniformResourceIdentifier"&gt;http://www.bar.org/wotsit.crl&lt;/name&gt;
        ///         &lt;/cdp&gt;
        ///     &lt;/value&gt;
        /// &lt;/Extension&gt;
        /// </code>
        /// Note that FreshestCRL and CRLDistributionPoint are processed the same.
        /// </remarks>
        /// <returns>XML encoding of the extension</returns>
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

            // Create the parameter list
            XElement entry = extension.Element("value");
            foreach (OSCAGeneralName dp in distPoints)
            {
                entry.Add(new XElement("cdp", dp.Name,
                    new XAttribute("type", dp.Type.ToString()))
                    );
            }            
            return extension;
        }

        /// <summary>
        /// Add a new crlDistPoint to the extension
        /// </summary>
        /// <param name="Name"></param>
        public void Add(OSCAGeneralName Name)
        {
            distPoints.Add(Name);
        }

        /// <summary>
        /// Remove a crlDistPoint from the extension
        /// </summary>
        /// <param name="Name"></param>
        public void Remove(OSCAGeneralName Name)
        {
            distPoints.Remove(Name);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that describes this extension
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that describes this extension
        /// </returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("{0} ({1}) ({2})\n", name, oid, strCritical());
            output.AppendFormat("\tDistribution Points:\n");
            foreach (OSCAGeneralName gname in distPoints)
                output.AppendFormat("\t\t{0}: {1}\n", gname.Type, gname.Name);
            return output.AppendLine().ToString();
        }

        /// <summary>
        /// Create a Distribution Point list
        /// </summary>
        /// <returns>Distribution Point list</returns>
        protected virtual DistributionPoint[] encode()
        {
            DistributionPoint[] dplist = new DistributionPoint[distPoints.Count()];
            int i = 0;
            foreach (OSCAGeneralName cdp in distPoints)
            {
                GeneralNames gn = generalNames.createGeneralNames(cdp.Type.ToString(), cdp.Name);
                DistributionPointName dpn = new DistributionPointName(gn);
                DistributionPoint dp = new DistributionPoint(dpn, null, null);
                dplist[i] = dp;
                i++;
            }
            return dplist;
        }

        /// <summary>
        /// Read a DistributionPoint list
        /// </summary>
        /// <param name="dps">DistributionPoint list</param>
        protected void decode(DistributionPoint[] dps)
        {
            foreach (DistributionPoint dp in dps)
            {
                DistributionPointName dpn = dp.DistributionPointName;
                OSCAGeneralName ogn = new OSCAGeneralName()
                {
                    Name = dpn.Name.ToString(),
                    Type = (GenName)dpn.PointType
                };
                distPoints.Add(ogn);
            }
        }
    }
}

