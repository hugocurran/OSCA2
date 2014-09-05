/*
 * Copyright 2011-13 Peter Curran (peter@currans.eu). All rights reserved.
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

using System.Linq;
using Microsoft.ManagementConsole;
using System.Windows.Forms;
using OSCASnapin.ConfigManager;

namespace OSCASnapin
{
    public class OSCAroot : ScopeNode
    {
        // This object holds the master list of all CAs and their control interface objects
        private Configuration mgrConfig = Configuration.Instance;
        

        internal OSCAroot()
        {
            this.ImageIndex = 0;

            // Initial setup in case there are no existing CAs
            if (mgrConfig.Mode == Mode.UserMode)
            {
                this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Admin Mode", "Enter Admin Mode", 0, "AdminMode"));
            }
            else
            {
                this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("User Mode", "Enter User Mode", 0, "UserMode"));
            }
        }

        // Note that this cannot be run until after the Configuration object is initialised
        internal void Load()
        {
            // Setup a child node for each CA
            foreach (CA ca in mgrConfig.CaList)
            {
                this.Children.Add(new CANode(ca) 
                    { 
                        DisplayName = ca.CaName,
                        //EnabledStandardVerbs = StandardVerbs.Properties
                    });
            }
            // Setup actions
            refreshActions();
        }

        private void refreshActions()
        {
            // Actions in this node

            this.ActionsPaneItems.Clear();
            // Setup actions
            if (mgrConfig.Mode == Mode.UserMode)
            {
                this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Admin Mode", "Enter Admin Mode", 0, "AdminMode"));
            }
            else
            {
                this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("User Mode", "Enter User Mode", 0, "UserMode"));
                this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Change Password", "Change Admin Password", 0, "AdminPassword"));
            }

            if (mgrConfig.Permitted("CreateIntCA"))
                this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Create CA (internal)", "Create a new CA (internal)", 0, "CreateIntCA"));
            if (mgrConfig.Permitted("CreateExtCA"))
                this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Create CA (external)", "Create a new CA (external)", 0, "CreateExtCA"));
            if (mgrConfig.Permitted("AddCA"))
                this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Add existing CA", "Add existing CA", 0, "AddCA"));

            // Actions in child nodes
            foreach (var caNode in this.Children)
            {
                ((CANode)caNode).Refresh();
            }
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
                case "CreateIntCA":
                    CreateCA newCA = new CreateCA() { mgrConfig = mgrConfig };
                    if (newCA.ShowDialog() == DialogResult.OK)
                    {
                        CA ca = mgrConfig.CaList.Last<CA>();
                        status.Complete("CA Created", true);
                        MessageBox.Show("CA Created: " + ca.CaName, "Create CA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Children.Add(new CANode(ca) { DisplayName = ca.CaName });
                    }
                    else
                        status.Complete("Creation aborted", false);
                    break;

                case "CreateExtCA":

                    break;

                case "AddCA":
                    AddCA addCA = new AddCA();
                    if (addCA.ShowDialog() == DialogResult.OK)
                    {
                        CA ca = mgrConfig.CaList.Last<CA>();
                        status.Complete("CA Added", true);
                        MessageBox.Show("CA Added: " + ca.CaName, "Add CA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Children.Add(new CANode(ca) { DisplayName = ca.CaName});
                    }
                    else
                        status.Complete("Creation aborted", false);
                    break;

                case "AdminMode":
                    AdminPassword pwd = new AdminPassword();
                    if (pwd.ShowDialog() == DialogResult.OK)
                    {
                        mgrConfig.SwitchMode(Mode.AdminMode);
                        refreshActions();
                    }
                    break;

                case "AdminPassword":
                    ChangeAdminPassword newpwd = new ChangeAdminPassword();
                    if (newpwd.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Password changed", "Change Admin Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "UserMode":
                    mgrConfig.SwitchMode(Mode.UserMode);
                    refreshActions();
                    break;
            }
        }

        public void Refresh()
        {
            this.Children.Clear();

            // Create a new node for each CA
            foreach (CA ca in mgrConfig.CaList)
            {
                // Create Child node.
                this.Children.Add(new CANode(ca) { DisplayName = ca.CaName });
            }
        }

        /// <summary>
        /// OnAddPropertyPages is used to get the property pages to show. 
        /// (triggered by Properties verbs)
        /// </summary>
        /// <param name="propertyPageCollection">property pages</param>
        protected override void OnAddPropertyPages(PropertyPageCollection propertyPageCollection)
        {
            propertyPageCollection.Add(new OSCAPropertyPage(this));
        }
    }
}
