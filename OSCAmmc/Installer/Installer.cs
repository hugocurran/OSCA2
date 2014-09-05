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

using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.ManagementConsole;
using System.Runtime.Serialization.Formatters.Binary;
using OSCASnapin.Properties;
using System.IO;
using System.Runtime.Serialization;
using OSCASnapin.ConfigManager;

[assembly: PermissionSetAttribute(SecurityAction.RequestMinimum, Unrestricted = true)]

namespace OSCASnapin
{
    [RunInstaller(true)]
    public class OSCAInstaller : SnapInInstaller
    {
    }


    [SnapInSettings("{3BB45B8A-6152-49c5-B314-3885214B143E}",
    DisplayName = "OSCA",
    Description = "Offline Simple CA",
    LicenseFile = "License.txt",
    Vendor = "Peter Curran")]
    //[SnapInAbout("OSCAresource.dll",
    //ApplicationBaseRelative = true,
    //DisplayNameId = 101,
    //DescriptionId = 102,
    //VendorId = 103,
    //VersionId = 104,
    //IconId = 110,
    //LargeFolderBitmapId = 111,
    //SmallFolderBitmapId = 112,
    //SmallFolderSelectedBitmapId = 112,
    //FolderBitmapsColorMask = 0x00ff00)]
    public class OSCA : SnapIn
    {

        private Configuration mgrConfig = Configuration.Instance;
                
        public OSCA()
        {
            // Create the root node.
            this.RootNode = new OSCAroot() 
            { 
                DisplayName = "OSCA", 
                EnabledStandardVerbs = StandardVerbs.Properties
            };
            
            this.SmallImages.Add(Resources.black_server);           // 0
            this.SmallImages.Add(Resources.disable_server);         // 1
            this.SmallImages.Add(Resources.enable_server);          // 2
            this.SmallImages.Add(Resources.folder);                 // 3
            this.SmallImages.Add(Resources.certificate_sml);        // 4
            this.SmallImages.Add(Resources.crl_sml);                // 5
            this.SmallImages.Add(Resources.Tasks);                  // 6

            this.LargeImages.Add(Resources.black_server);
            this.LargeImages.Add(Resources.disable_server);
            this.LargeImages.Add(Resources.enable_server);
            this.LargeImages.Add(Resources.folder);
            this.LargeImages.Add(Resources.certificate_sml);
            this.LargeImages.Add(Resources.crl_sml);
            this.LargeImages.Add(Resources.Tasks);

            // Set flag so config data is saved every time
            this.IsModified = true;

            // Set the default User Mode
            mgrConfig.SwitchMode(Mode.UserMode);
        }

        /// <summary>
        /// Shows the Initialization Wizard when the snapin is added to console 
        /// Returning 'False' will cause MMC to cancel the loading of the snap-in 
        /// </summary>
        /// <returns>true to continue loading the snap-in. false cancels snap-in loading.</returns>
        protected override bool OnShowInitializationWizard()
        {
            // show modal dialog to get OSCA Folder
            InitialisationWizard initialisationWizard = new InitialisationWizard();
            if (initialisationWizard.ShowDialog() == DialogResult.OK)
            {
                mgrConfig.SetInitData(initialisationWizard.initData);

                if (mgrConfig.InitData.configFile == "")  // Create a new config file
                {
                    // Create a manager config file
                    mgrConfig.InitData.configFile = mgrConfig.InitData.configFolder + @"\MgrConfig.xml";
                    XDocument config = new XDocument(
                        new XDeclaration("1.0", "utf-8", "yes"),
                        new XComment("OSCA Manager Configuration"),
                        new XElement("OSCAMGR",
                            new XAttribute("version", "1.1"),
                            new XElement("Root",
                                new XElement("location", mgrConfig.InitData.configFolder)
                            ),
                            new XElement("Policy",
                                new XElement("location", mgrConfig.InitData.configFolder + @"\Policy")
                            ),
                            new XElement("CAList")
                        )
                    );
                    config.Save(mgrConfig.InitData.configFile);

                }
                //  Initialise so we can proceed to create a CA
                mgrConfig.Initialise();

                // Load the CA data into the root node
                ((OSCAroot)RootNode).Load();

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Load in any saved data
        /// </summary>
        /// <param name="status">asynchronous status for updating the console</param>
        /// <param name="persistenceData">binary data stored in the console file</param>
        protected override void OnLoadCustomData(AsyncStatus status, byte[] persistenceData)
        {
            status.ReportProgress(0, 10, "Loading..");
            
            // Deserialize the object
            try
            {
                MemoryStream stream = new MemoryStream(persistenceData);
                BinaryFormatter deserializer = new BinaryFormatter();
                mgrConfig.SetInitData((InitialisationData)deserializer.Deserialize(stream));

                // Initialise the config object
                mgrConfig.Initialise();

                // Load the CA data into the root node
                ((OSCAroot)RootNode).Load();

                status.Complete("Manager configuration: " + mgrConfig.InitData.configFile, true);
            }
            catch (SerializationException)
            {
                MessageBox.Show("No configuration data defined", "OSCA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// If snapIn 'IsModified', then save data
        /// </summary>
        /// <param name="status">status for updating the console</param>
        /// <returns>binary data to be stored in the console file</returns>
        protected override byte[] OnSaveCustomData(SyncStatus status)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, mgrConfig.InitData);
            return stream.ToArray();
        }
    }
}
