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
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Crypto.X509
{

    /// <summary>
    /// Microsoft private Certificate Template Information extension
    /// </summary>
    /// <remarks>
    /// This identifies which template was used to define the certificate contents.
    /// <para>
    /// This extension is used during (automatic) re-enrollment to assign the correct template; it is also used to force re-enrollment following a template change.
    /// </para>
    /// <code>
    /// The MsCertTemplateInfo object
    /// 
    /// MsCertTemplateInfoSyntax  ::= SEQUENCE of {
    ///       Template,
    ///       TemplateMajorVersion,
    ///       TemplateMinorVersion }
    /// 
    /// Template ::= OBJECTIDENTIFIER
    /// TemplateMajorVersion ::= INTEGER (0..MAX)
    /// TemplateMinorVersion ::= INTEGER (0..MAX)
    /// </code>
    /// </remarks>
    public class MsCertTemplateInfo : Asn1Encodable
    {
        private DerObjectIdentifier template;
        private DerInteger majorVersion;
        private DerInteger minorVersion;        
        
        /// <summary>
        /// Microsoft private Certificate Template Information extension
        /// </summary>
        public static readonly DerObjectIdentifier CertTemplateInfo = new DerObjectIdentifier("1.3.6.4.1.311.21.7");
        
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public string Template { 
            get { return template.ToString(); } 
            set {template = new DerObjectIdentifier(value); } 
        }

        /// <summary>
        /// Gets or sets the major version.
        /// </summary>
        /// <value>
        /// The major version.
        /// </value>
        public int MajorVersion { 
            get { return majorVersion.Value.IntValue; }
            set { majorVersion = new DerInteger(value); }
        }

        /// <summary>
        /// Gets or sets the minor version.
        /// </summary>
        /// <value>
        /// The minor version.
        /// </value>
        public int MinorVersion {
            get { return minorVersion.Value.IntValue; }
            set { minorVersion = new DerInteger(value); }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">unknown object in factory:  + obj.GetType().Name;obj</exception>
        public static MsCertTemplateInfo GetInstance(object obj)
        {
            if (obj is MsCertTemplateInfo || obj == null)
                return (MsCertTemplateInfo)obj;

            if (obj is Asn1Sequence)
                return new MsCertTemplateInfo((Asn1Sequence)obj);

            if (obj is X509Extension)
                return GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));

            throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
        }

        private MsCertTemplateInfo(Asn1Sequence seq)
        {
            if (seq[0] is DerObjectIdentifier)
                template = DerObjectIdentifier.GetInstance(seq[0]);
            else
                throw new ArgumentException("Invalid sequence", "seq");

            if (seq[1] is DerInteger)
                majorVersion = DerInteger.GetInstance(seq[1]);
            else
                throw new ArgumentException("Invalid sequence", "seq");

            if (seq[2] is DerInteger)
                minorVersion = DerInteger.GetInstance(seq[1]);
            else
                throw new ArgumentException("Invalid sequence", "seq");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsCertTemplateInfo"/> class.
        /// </summary>
        /// <param name="Template">The template.</param>
        /// <param name="MajorVersion">The major version.</param>
        /// <param name="MinorVersion">The minor version.</param>
        public MsCertTemplateInfo(string Template, int MajorVersion, int MinorVersion)
        {
            this.Template = Template;
            this.MajorVersion = MajorVersion;
            this.MinorVersion = MinorVersion;
        }

        /// <summary>
        /// Output asn1 object.
        /// </summary>
        /// <returns></returns>
        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector();
            v.Add(template);
            v.Add(majorVersion);
            v.Add(minorVersion);
            
            return new DerSequence(v);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Certificate Template Information (MS): {0} Ver {1}.{2}", Template, MajorVersion, MinorVersion);
            return sb.ToString();
        }
    }
}
