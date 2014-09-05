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
using OSCA.Offline;

namespace OSCA.Crypto.X509
{

    /// <summary>
    /// Microsoft private Certificate Template Name extension
    /// </summary>
    /// <remarks>
    /// This identifies which template was used to define the certificate contents.
    /// <para>
    /// This extension is used during enrollment to specify the correct template
    /// </para>
    /// <code>
    /// The MsCertTemplateName object
    /// 
    /// MsCertTemplateName  ::= OCTET STRING
    /// </code>
    /// </remarks>
    public class MsCertTemplateName : Asn1Encodable
    {
        private Asn1OctetString templateName;

        /// <summary>
        /// Microsoft private Certificate Template Name extension
        /// </summary>
        public static readonly DerObjectIdentifier CertTemplateName = new DerObjectIdentifier("1.3.6.1.4.1.311.20.2");

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public byte[] TemplateName 
        {
            get { return templateName.GetOctets(); }
            set { templateName = new DerOctetString(value); }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">unknown object in factory:  + obj.GetType().Name;obj</exception>
        public static MsCertTemplateName GetInstance(object obj)
        {
            if (obj is MsCertTemplateName || obj == null)
                return (MsCertTemplateName)obj;

            if (obj is Asn1OctetString)
                return new MsCertTemplateName((Asn1OctetString)obj);

            if (obj is X509Extension)
            {
                DerBmpString val =  DerBmpString.GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));
                return new MsCertTemplateName(val.GetString());                
            }

            throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
        }

        private MsCertTemplateName(Asn1OctetString name)
        {
            templateName = name;
        }

        private MsCertTemplateName(string name)
        {
            templateName = new DerOctetString(Utility.StringToUTF8ByteArray(name));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsCertTemplateName" /> class.
        /// </summary>
        /// <param name="TemplateName">Name of the template.</param>
        public MsCertTemplateName(byte[] TemplateName)
        {
            this.TemplateName = TemplateName;
        }

        /// <summary>
        /// Output asn1 object.
        /// </summary>
        /// <returns></returns>
        public override Asn1Object ToAsn1Object()
        {
            return new DerOctetString(templateName);
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
            sb.AppendFormat("Certificate Template Name (MS): {0}", Utility.UTF8ByteArrayToString(TemplateName));
            return sb.ToString();
        }
    }
}
