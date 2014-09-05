/*
 * Copyright 2011-2013 Peter Curran (peter@currans.eu). All rights reserved.
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
using OSCASnapin.CAinfo;

namespace OSCASnapin.ConfigManager
{
    /// <summary>
    /// User role mode
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Admin
        /// </summary>
        AdminMode,
        /// <summary>
        /// User
        /// </summary>
        UserMode
    }


    /// <summary>
    /// Singleton object that holds all configuration information.
    /// </summary>
    /// 
    internal sealed class Configuration
    {
        #region Singleton setup

        // Singleton instance
        private static readonly Configuration instance = new Configuration();

        private Configuration() { }

        public static Configuration Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// List of CA objects
        /// </summary>
        internal List<CA> CaList { get; private set; }

        /// <summary>
        /// Location of the OSCA directory
        /// </summary>
        internal string OscaFolder { get; private set; }

        /// <summary>
        /// Location of the Manager profiles directory
        /// </summary>
        internal string PolicyFolder { get; private set; }

        /// <summary>
        /// Privilege mode
        /// </summary>
        internal Mode Mode { get; private set; }

        /// <summary>
        /// Dataset loaded when the snapin started
        /// </summary>
        internal InitialisationData InitData { get; private set; }

        #region Initialisation

        internal void SetInitData(InitialisationData data)
        {
            InitData = data;
        }
        
        /// <summary>
        /// Initialise a Manager Configuration object with CA data
        /// </summary>
        internal void Initialise()
        {
            if (InitData == null)
                throw new ApplicationException("Initialisation Data not found");

            CaList = new List<CA>();

            // Load the data from the manager config file
            XDocument config = XDocument.Load(InitData.configFile);
            OscaFolder = config.Element("OSCAMGR").Element("Root").Element("location").Value;
            PolicyFolder = config.Element("OSCAMGR").Element("Policy").Element("location").Value;
            var calist = config.Element("OSCAMGR").Element("CAList").Descendants("CA");
            CA ca;
            foreach (XElement item in calist)
            {
                ca = new CA();
                ca.CaName = item.Element("name").Value;
                ca.Role = item.Element("type").Value;
                ca.ConfigLocation = item.Element("config").Value;
                ca.CaControl = new CaControl(ca.ConfigLocation);
                CaList.Add(ca);
            }
        }

        #endregion

        #region CA management

        /// <summary>
        /// Remove a CA from the Manager Configuration
        /// </summary>
        /// <param name="ca">CA object to remove</param>
        public void RemoveCA(CA ca)
        {
            // Remove the entry from the config file
            XDocument config = XDocument.Load(InitData.configFile);
            var calist = config.Element("OSCAMGR").Element("CAList").Descendants("CA");
            foreach (XElement item in calist)
            {
                if (item.Element("name").Value == ca.CaName)
                {
                    item.Remove();
                    break;  // Have to break here as loop is unstable when last item removed from the list
                }
            }
            config.Save(InitData.configFile);

            // Remove the entry from the list
            CaList.Remove(ca);
        }

        /// <summary>
        /// Add a new CA to the Manager Configuration
        /// </summary>
        /// <param name="ca">CA object to add</param>
        internal void InsertCA(CA ca)
        {
            // Create a new CA entry
            XElement entry = new XElement("CA",
                new XElement("name", ca.CaName),
                new XElement("type", ca.Role),
                new XElement("config", ca.ConfigLocation)
            );

            // Insert CA entry into config file
            XDocument config = XDocument.Load(InitData.configFile);
            XElement ep = config.Element("OSCAMGR").Element("CAList");     
            ep.Add(entry);
            config.Save(InitData.configFile);

            // Insert CA entry into Config Manager object
            ca.CaControl = new CaControl(ca.ConfigLocation);
            CaList.Add(ca);
        }

        #endregion

        #region Privilege management

        internal void SwitchMode(Mode NewMode)
        {
            Mode = NewMode;
        }

        internal bool Permitted(string Action)
        {
            switch (Mode)
            {
                case Mode.UserMode:
                    return InitData.userPriv[Action];
                case Mode.AdminMode:
                    return InitData.adminPriv[Action];
            }
            return false;
        }

        #endregion
    }
}
