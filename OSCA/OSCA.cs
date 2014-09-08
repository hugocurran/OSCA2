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
using System.Xml.Linq;
using OSCA.Offline;

namespace OSCA
{
    /// <summary>
    /// Factory for loading OSCA CA instances
    /// </summary>
    public static class OSCA
    {
        /// <summary>
        /// Load a CA instance based on the CA type in the configuration file
        /// </summary>
        /// <param name="ConfigFile">Full pathname of the CA config file</param>
        /// <param name="Password">Password to access key material</param>
        /// <returns>OSCA CA instance or null</returns>
        public static ICA LoadCA(string ConfigFile, string Password)
        {
            // Read in the configuration
            XDocument config = XDocument.Load(ConfigFile);
            string version = config.Element("OSCA").Attribute("version").Value;
            XElement ca = config.Element("OSCA").Element("CA");

            // Version 2.x CA (basically either fips or simple)
            if ((version == "2.0") || (version == "2.1"))
            {
                bool fipsMode = Convert.ToBoolean(ca.Element("fips140").Value);

                if (fipsMode)
                    return new fipsCA(ConfigFile);
                else
                    return new simpleCA(ConfigFile, Password);
            }

            // Version 3.x CA - CA type based on CA_TYPE
            if ((version == "3.0") || (version == "3.1"))
            {
                CA_Type caType = Utility.SetCA_Type(ca.Element("caType").Value);
                switch (caType)
                {
                    case CA_Type.sysCA:
                        return new sysCA(ConfigFile);
                    case CA_Type.bcCA:
                        return new bcCA(ConfigFile, Password);
                    case CA_Type.dhTA:
                        return new dhTA(ConfigFile);
                    case CA_Type.cngCA:
                        return new cngCA(ConfigFile);
                    default:
                        return null;    // Should never see this
                }
            }
            return null;
        }
    }
}
