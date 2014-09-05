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
using System.Linq;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA
{
    /// <summary>
    /// Types of GeneralName
    /// </summary>
    public enum GenName
    {
        /// <summary>
        /// Other name
        /// </summary>
        otherName = 0,
        /// <summary>
        /// RFC822 name
        /// </summary>
        rfc822Name = 1,
        /// <summary>
        /// DNS name
        /// </summary>
        dNSName = 2,
        /// <summary>
        /// X4.400 address
        /// </summary>
        x400Address = 3,
        /// <summary>
        /// Directory name
        /// </summary>
        directoryName = 4,
        /// <summary>
        /// EDI party name
        /// </summary>
        ediPartyName = 5,
        /// <summary>
        /// URI
        /// </summary>
        uniformResourceIdentifier = 6,
        /// <summary>
        /// IP address
        /// </summary>
        iPAddress = 7,
        /// <summary>
        /// Registered ID
        /// </summary>
        registeredID = 8
    }

    /// <summary>
    /// OSCA General Name type
    /// </summary>
    public struct OSCAGeneralName
    {
        /// <summary>
        /// Type of name
        /// </summary>
        public GenName Type;
        /// <summary>
        /// Name
        /// </summary>
        public string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="OSCAGeneralName"/> struct.
        /// </summary>
        /// <param name="genName">BC general name</param>
        public OSCAGeneralName(GeneralName genName)
        {
            Name = genName.Name.ToString();
            Type = (GenName)genName.TagNo;
        }

        /// <summary>
        /// OSCA GeneralName as a BC GeneralName.
        /// </summary>
        /// <returns>BC GeneralName</returns>
        public GeneralName GetGeneralName()
        {
            return new GeneralName((int)Type, Name);
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
            sb.AppendFormat("{0}: {1}\n", Type, Name);
            return sb.ToString();
        }
    }
}
