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

using OSCASnapin.CAinfo;

namespace OSCASnapin.ConfigManager
{
    /// <summary>
    /// Object that holds information about a CA
    /// </summary>
    public class CA
    {
        /// <summary>
        /// Gets or sets the name of the ca.
        /// </summary>
        /// <value>
        /// The name of the ca.
        /// </value>
        public string CaName { get; set; }                   // Name of CA
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Role { get; set; }                     // Type of CA (rootCA||subCA)
        /// <summary>
        /// Gets or sets the configuration location.
        /// </summary>
        /// <value>
        /// The configuration location.
        /// </value>
        public string ConfigLocation { get; set; }           // Location of CAconfig.xml file
        /// <summary>
        /// Gets or sets the ca control.
        /// </summary>
        /// <value>
        /// The ca control.
        /// </value>
        public CaControl CaControl { get; set; }             // CaControl object
    }

}
