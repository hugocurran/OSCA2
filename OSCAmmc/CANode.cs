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

using System.IO;
using Microsoft.ManagementConsole;
using System.Windows.Forms;
using OSCASnapin.CAinfo;
using OSCASnapin.ConfigManager;

namespace OSCASnapin
{
    public class CANode : ScopeNode
    {
        private Configuration mgrConfig = Configuration.Instance;

        internal CA ca;                         // Reference to this CA instance in the mgrConfig object
        internal CaControl caControl;           // Control interface for this CA


        private CertsNode currentCerts;     // Link to child so we can Refresh()
        private CertsNode revokedCerts;     // Link to child so we can Refresh()
        private CertsNode expiredCerts;
        private ProfilesNode profiles;

        public CANode(CA ca)
        {
            this.ca = ca;
            this.caControl = ca.CaControl;
            this.Tag = caControl;  // Make this available for child use
            
            // Add in the folders for the CA database (keeping track of the CertsNode instances)
            currentCerts = new CertsNode(caControl, CertStatus.Current) { DisplayName = "Current Certificates"};
            expiredCerts = new CertsNode(caControl, CertStatus.Expired) { DisplayName = "Expired Certificates" };
            revokedCerts = new CertsNode(caControl, CertStatus.Revoked) { DisplayName = "Revoked Certificates" };
            profiles = new ProfilesNode(caControl) { DisplayName = "Profiles" };

            this.Children.Add(currentCerts);
            this.Children.Add(revokedCerts);
            this.Children.Add(expiredCerts);
            this.Children.Add(profiles);

            // Check the status of the CA and setup accordngly
            Refresh();
        }

        /// <summary>
        /// OnAddPropertyPages is used to get the property pages to show. 
        /// (triggered by Properties verbs)
        /// </summary>
        /// <param name="propertyPageCollection">property pages</param>
        protected override void OnAddPropertyPages(PropertyPageCollection propertyPageCollection)
        {
            propertyPageCollection.Add(new CAPropertyPage(this));
        }

        protected override void OnRefresh(AsyncStatus status)
        {
            base.OnRefresh(status);
            Refresh();
        }

        internal void Refresh()
        {
            // Check the status of the CA and setup accordngly
            this.ActionsPaneItems.Clear();

            if (caControl.CAStatus == CAstatus.Running)
            {
                this.ImageIndex = 2;
                this.SelectedImageIndex = 2;
                if (mgrConfig.Permitted("StopCA"))
                    this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Stop CA", "Unload CA and deactivate", 1, "StopCA"));
                // Needs to be here otherwise the cainfo reference to the ca is null!
                if (mgrConfig.Permitted("ExportCAKey"))
                    this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Export CA Key-pair", "Export the CA key-pair to a backup file", 0, "ExportCAKey"));
            }
            else
            {
                this.ImageIndex = 1;
                this.SelectedImageIndex = 1;
                if (mgrConfig.Permitted("StartCA"))
                    this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Start CA", "Load CA and activate", 2, "StartCA"));
                if (mgrConfig.Permitted("DeleteCA"))
                    this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Delete CA", "Delete this CA", 0, "DeleteCA"));
            }
            currentCerts.Refresh();
            revokedCerts.Refresh();
            expiredCerts.Refresh();
            profiles.Refresh();            
        }

        /// <summary>
        /// Handle node actions
        /// </summary>
        /// <param name="action">action that was triggered</param>
        /// <param name="status">asynchronous status for updating the console</param>
        protected override void OnAction(Microsoft.ManagementConsole.Action action, AsyncStatus status)
        {
            switch ((string)action.Tag)
            {
                case "StartCA":
                    if (!caControl.FIPS140Mode)
                    {
                        CApassword password = new CApassword();
                        if (this.SnapIn.Console.ShowDialog(password) == DialogResult.OK)
                            try
                            {
                                caControl.StartCA(password.tbPassword.Text);
                            }
                            catch (IOException)
                            {
                                MessageBox.Show("Invalid password or corrupt key file", "Start CA", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                break;
                            }
                        else
                            break;
                    }
                    else
                        caControl.StartCA("");
                    Refresh();
                    break;

                case "StopCA":
                    caControl.StopCA();
                    Refresh();
                    break;

                case "DeleteCA":
                    DialogResult result = MessageBox.Show("Do you wish to remove this CA from the OSCA manager?\n" + caControl.CAName, "OSCA", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                    if (result == DialogResult.Yes)
                    {
                        caControl.StopCA();
                        mgrConfig.RemoveCA(ca);
                        ((OSCAroot)this.Parent).Refresh();
                    }
                    break;

                case "ExportCAKey":
                    ExportCAKey export = new ExportCAKey();
                    if (export.ShowDialog() == DialogResult.OK)
                    {
                        caControl.Backup(export.password, export.location);
                        MessageBox.Show("CA keypair exported", "Export CA keypair", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }
        }
    }
}


   