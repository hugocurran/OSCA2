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

using System.Collections.Generic;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto;
using System.Text;


namespace OSCA.Profile
{
    /// <summary>
    /// Types of name constraints
    /// </summary>
    public enum NameConstraintTree
    {
        /// <summary>
        /// Permitted names
        /// </summary>
        Permitted,
        /// <summary>
        /// Prohibited names
        /// </summary>
        Excluded
    }

    /// <summary>
    /// Name Constraints extension
    /// </summary>
    /// <remarks>
    /// ASN.1 description from RFC5280:
    /// <code>
    /// id-ce-nameConstraints OBJECT IDENTIFIER ::=  { id-ce 30 }
    /// 
    /// NameConstraints ::= SEQUENCE {
    ///    permittedSubtrees       [0]     GeneralSubtrees OPTIONAL,
    ///    excludedSubtrees        [1]     GeneralSubtrees OPTIONAL }
    ///    
    /// GeneralSubtrees ::= SEQUENCE SIZE (1..MAX) OF GeneralSubtree
    ///
    /// GeneralSubtree ::= SEQUENCE {
    ///    base                    GeneralName,
    ///    minimum         [0]     BaseDistance DEFAULT 0,
    ///    maximum         [1]     BaseDistance OPTIONAL }
    ///    
    /// BaseDistance ::= INTEGER (0..MAX)
    /// </code>
    /// </remarks>
    public class nameConstraints : ProfileExtension
    {

        private List<OSCAGeneralName> permitted = new List<OSCAGeneralName>();
        private List<OSCAGeneralName> excluded = new List<OSCAGeneralName>();

        /// <summary>
        /// List of permitted namespaces
        /// </summary>
        public List<OSCAGeneralName> Permitted { get { return permitted; } }

        /// <summary>
        /// List of excluded namespaces
        /// </summary>
        public List<OSCAGeneralName> Excluded { get { return excluded; } }

        /// <summary>
        /// DER encoded value of NameConstraints
        /// </summary>
        public NameConstraints NameConstraint { get { encode(); return (NameConstraints)base.encValue; } }

        /// <summary>
        /// DER encoded value of extension
        /// </summary>
        override public Asn1Encodable DerEncoding { get { encode(); return base.encValue; } }

        /// <summary>
        /// Create NameConstraints extension
        /// </summary>
        public nameConstraints() : this(true) { }

        /// <summary>
        /// Create NameConstraints extension
        /// </summary>
        /// <param name="Critical">True = Critical, False = Not Critical</param>
        public nameConstraints(bool Critical) : base()
        {
            base.oid = X509Extensions.NameConstraints;
            base.name = "NameConstraints";
            base.displayName = "Name Constraints";
            base.critical = Critical;
        }

        /// <summary>
        /// Create NameConstraints extension from XML profile file entry
        /// </summary>
        /// <param name="xml">XML version of the extension</param>
        /// <remarks>Sample OSCA XML description of the NameConstraints extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="Name Constraints"&gt;NameConstraints&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///       &lt;permitted&gt;
        ///         &lt;name type="rfc822Name"&gt;*.foo.org&lt;/name&gt;
        ///       &lt;/permitted&gt;
        ///       &lt;excluded&gt;
        ///         &lt;name type="dNSName"&gt;*.bar.net&lt;/name&gt;
        ///       &lt;/excluded&gt;
        ///    &lt;/value&gt;
        ///  &lt;/Extension&gt;
        /// </code> 
        /// </remarks> 
        public nameConstraints(XElement xml) : base(xml)
        {
            base.oid = X509Extensions.NameConstraints;

            if (xmlValue.Element("permitted") != null)
            {
                foreach (XElement perm in xmlValue.Element("permitted").Descendants("name"))
                {
                    OSCAGeneralName name = new OSCAGeneralName()
                    {
                        Name = perm.Value,
                        Type = generalNames.getGenName(perm.Attribute("type").Value)
                    };
                    permitted.Add(name);
                }
            }

            if (xmlValue.Element("excluded") != null)
            {
                foreach (XElement excl in xmlValue.Element("excluded").Descendants("name"))
                {
                    OSCAGeneralName name = new OSCAGeneralName()
                    {
                        Name = excl.Value,
                        Type = generalNames.getGenName(excl.Attribute("type").Value)
                    };
                    excluded.Add(name);
                }
            }
        }

        /// <summary>
        /// Create NameConstraints extension from an X509Extension
        /// </summary>
        /// <param name="Extension">X509Extension</param>
        /// <remarks>
        /// Sub classses must provide an implementation to decode their values
        /// </remarks>
        public nameConstraints(X509Extension Extension) : base(Extension)
        {
            base.oid = X509Extensions.NameConstraints;
            base.name = "NameConstraints";
            base.displayName = "Name Constraints";

            NameConstraints nc = NameConstraints.GetInstance(Extension);
            // Permitted names
            Asn1Sequence perm = Asn1Sequence.GetInstance(nc.PermittedSubtrees);
            foreach (var item in perm)
                permitted.Add(new OSCAGeneralName(GeneralName.GetInstance(item)));

            // Excluded names
            Asn1Sequence exc = Asn1Sequence.GetInstance(nc.ExcludedSubtrees);
            foreach (var item in exc)
                excluded.Add(new OSCAGeneralName(GeneralName.GetInstance(item)));     
        }
        
        /// <summary>
        /// Provide an XML version of the extension
        /// </summary>
        /// <returns>XML encoding of the extension</returns>
        /// <remarks>Sample OSCA XML description of the NameConstraints extension:
        /// <code>
        ///  &lt;Extension&gt;
        ///    &lt;name description="Name Constraints"&gt;NameConstraints&lt;/name&gt;
        ///    &lt;critical&gt;false&lt;/critical&gt;
        ///    &lt;value&gt;
        ///       &lt;permitted&gt;
        ///         &lt;name type="rfc822Name"&gt;*.foo.org&lt;/name&gt;
        ///       &lt;/permitted&gt;
        ///       &lt;excluded&gt;
        ///         &lt;name type="dNSName"&gt;*.bar.net&lt;/name&gt;
        ///       &lt;/excluded&gt;
        ///    &lt;/value&gt;
        ///  &lt;/Extension&gt;
        /// </code> 
        /// </remarks> 
        public override XNode ToXml()
        {
            // Build generic
            XElement extension = (XElement)base.ToXml();

            // Create the parameter list
            XElement entry = extension.Element("value");

            if (permitted.Count > 0)
            {
                entry.Add(new XElement("permitted"));
                foreach (OSCAGeneralName name in permitted)
                {
                    entry.Element("permitted").Add(new XElement("name", name.Name,
                        new XAttribute("type", name.Type.ToString()))
                        );
                }
            }

            if (excluded.Count > 0)
            {
                entry.Add(new XElement("excluded"));
                foreach (OSCAGeneralName name in excluded)
                {
                    entry.Element("excluded").Add(new XElement("name", name.Name,
                        new XAttribute("type", name.Type.ToString()))
                        );
                }
            }
            return extension;
        }

        /// <summary>
        /// Add a name
        /// </summary>
        /// <param name="Tree">Exclude or included</param>
        /// <param name="Name">Name to add</param>
        public void Add(NameConstraintTree Tree, OSCAGeneralName Name)
        {
            switch (Tree)
            {
                case NameConstraintTree.Permitted:
                    permitted.Add(Name);
                    break;
                case NameConstraintTree.Excluded:
                    excluded.Add(Name);
                    break;
            }
        }

        /// <summary>
        /// Remove a name
        /// </summary>
        /// <param name="Tree">Exclude or include</param>
        /// <param name="Name">Name to remove</param>
        public void Remove(NameConstraintTree Tree, OSCAGeneralName Name)
        {
            switch (Tree)
            {
                case NameConstraintTree.Permitted:
                    permitted.Remove(Name);
                    break;
                case NameConstraintTree.Excluded:
                    excluded.Remove(Name);
                    break;
            }
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
            output.AppendFormat("\tPermitted Names:\n");
            foreach (OSCAGeneralName gname in permitted)
                output.AppendFormat("\t\t{0}: {1}\n", gname.Type, gname.Name);
            output.AppendFormat("\tExcluded Names:\n");
            foreach (OSCAGeneralName gname in excluded)
                output.AppendFormat("\t\t{0}: {1}\n", gname.Type, gname.Name);
            return output.AppendLine().ToString();
        }

        private void encode()
        {
            GeneralSubtree[] permTree = null;
            GeneralSubtree[] exclTree = null;

            if (permitted.Count > 0)
            {
                permTree = new GeneralSubtree[permitted.Count];
                for (int i = 0; i < permitted.Count; i++)
                {
                    permTree[i] = new GeneralSubtree(new GeneralName((int)permitted[i].Type, permitted[i].Name));
                }
            }

            if (excluded.Count > 0)
            {
                exclTree = new GeneralSubtree[excluded.Count];
                for (int i = 0; i < excluded.Count; i++)
                {
                    exclTree[i] = new GeneralSubtree(new GeneralName((int)excluded[i].Type, excluded[i].Name));
                }
            }
            base.encValue = new NameConstraints(permTree, exclTree);
        }
    }
}
