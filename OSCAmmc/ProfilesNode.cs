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

using Microsoft.ManagementConsole;
using System.Windows.Forms;
using OSCASnapin.CAinfo;
using OSCASnapin.Profiles;
using OSCA.Profile;
using OSCASnapin.ConfigManager;
using System;


namespace OSCASnapin
{
    class ProfilesNode : ScopeNode
    {
        private Configuration mgrConfig = Configuration.Instance;
        internal CaControl caInfo { get; set; }
        private MmcListViewDescription lvd;
        
        public ProfilesNode(CaControl info)
        {
            caInfo = info;
            this.ImageIndex = 6;
            this.SelectedImageIndex = 6;
            this.EnabledStandardVerbs = StandardVerbs.Refresh;

            // Create a list view for the node.
            this.ViewDescriptions.Clear();
            lvd = null;

            // Create a list view for the node.
            lvd = new MmcListViewDescription();
            lvd.DisplayName = caInfo.CAName;
            lvd.ViewType = typeof(ProfilesListView);
            lvd.Options = MmcListViewOptions.ExcludeScopeNodes | MmcListViewOptions.SingleSelect;
            lvd.Tag = caInfo;


            // Attach the view to the node
            this.ViewDescriptions.Add(lvd);
            this.ViewDescriptions.DefaultIndex = 0;
        }

        internal void Refresh()
        {
            // Actions
            this.ActionsPaneItems.Clear();
            if (mgrConfig.Permitted("CreateProfile"))
                this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Create new profile", "Create a new profile", 6, "CreateProfile"));
        }

        protected override void OnRefresh(AsyncStatus status)
        {
            base.OnRefresh(status);
            Refresh();
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
                case "CreateProfile":
                    CreateProfile newProfile = new CreateProfile();
                    if (this.SnapIn.Console.ShowDialog(newProfile) == DialogResult.OK)
                    {                      
                        Profile caProfile = newProfile.profile;

                        // Update the caInfo object and save the profile
                        ProfileDb newEntry = new ProfileDb() {
                            profile = caProfile,
                            file = caProfile.SaveXml(caInfo.ProfilesLocation)
                        };
                        caInfo.AddProfile(newEntry);

                        // notify any listening views that a change happened
                        RaiseOnChange((string)action.Tag);
                    }                      
                    break;
            }
               
        }

        #region Event related methods  ------------------------------------------

        /// <summary>
        /// Declare the delegate type for the changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ChangedDelegate(object sender, ChangedEventArgs e);

        /// <summary>
        /// internal store of delegates needed when using event 'lock' syntax 
        /// </summary>
        private ChangedDelegate changedDelegate;

        /// <summary>
        /// Changed event to notify that the scope node has changed
        /// </summary>
        public event ChangedDelegate Changed
        {
            add
            {
                lock (this)
                    changedDelegate += value;
            }
            remove
            {
                lock (this)
                    changedDelegate -= value;
            }
        }

        /// <summary>
        /// Event arguments 
        /// </summary>
        public class ChangedEventArgs : EventArgs
        {
            private string status = "";

            /// <summary>
            /// New status changed to
            /// </summary>
            public string Status
            {
                get
                {
                    return status;
                }
                set
                {
                    status = value;
                }
            }
        }

        /// <summary>
        /// Raises the changed event using the private delegate
        /// </summary>
        private void RaiseOnChange(string status)
        {
            // safely invoke an event
            ChangedDelegate raiseChangedDelegate = changedDelegate;
            if (raiseChangedDelegate != null)
            {
                ChangedEventArgs changedEventArgs = new ChangedEventArgs();
                changedEventArgs.Status = status;

                raiseChangedDelegate(this, changedEventArgs);
            }
        }

        #endregion
    }
      

        /// <summary>
        /// ProfilesListView class - Loads and allows selection of profiles.
        /// </summary>
        public class ProfilesListView : MmcListView
        {
            private Configuration mgrConfig = Configuration.Instance;
            CaControl caInfo;

            /// <summary>
            /// Loads and allows selection of profiles.
            /// </summary>
            public ProfilesListView()
            {
            }

            protected override void OnInitialize(AsyncStatus status)
            {
                // do default handling
                base.OnInitialize(status);

                // Start listening for scope node events
                ((ProfilesNode)this.ScopeNode).Changed += new ProfilesNode.ChangedDelegate(OnScopeNodeChange);

                // Create a set of columns for use in the list view
                this.Columns[0].Title = "Name";
                this.Columns[0].SetWidth(125);

                // Add detail column
                this.Columns.Add(new MmcListViewColumn("Version", 75));
                this.Columns.Add(new MmcListViewColumn("Description", 200));
                this.Columns.Add(new MmcListViewColumn("Lifetime", 75));

                // Set to show all columns
                this.Mode = MmcListViewMode.Report;

                // set to show refresh as an option
                this.SelectionData.EnabledStandardVerbs = StandardVerbs.Refresh;

                // Load the list with values
                Refresh();
            }

            /// <summary>
            /// Do any cleanup. In this case Stop listening for scope node events
            /// </summary>
            /// <param name="status"></param>
            protected override void OnShutdown(SyncStatus status)
            {
                ((ProfilesNode)this.ScopeNode).Changed -= new ProfilesNode.ChangedDelegate(OnScopeNodeChange);
            }

            /// <summary>
            /// Handle any change to the scope node. In this case just refresh views
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            internal void OnScopeNodeChange(object sender, ProfilesNode.ChangedEventArgs e)
            {
                if (e.Status == "CreateProfile")
                {
                    // make MMC refresh the view status
                    Refresh();
                }
            }

            /// <summary>
            /// Loads the ListView with data
            /// </summary>
            public void Refresh()
            {
                // Clear existing information
                this.ResultNodes.Clear();

                caInfo = (CaControl)this.ViewDescriptionTag;

                foreach (ProfileDb profile in caInfo.Profiles)
                {
                    ResultNode node = new ResultNode();

                    node.ImageIndex = 6;
                    node.DisplayName = profile.profile.Name;
                    node.SubItemDisplayNames.Add(profile.profile.Version);
                    node.SubItemDisplayNames.Add(profile.profile.Description);
                    node.SubItemDisplayNames.Add(profile.profile.CertificateLifetime.Period.ToString() + " " + profile.profile.CertificateLifetime.Units.ToString());
                    node.Tag = profile;

                    this.ResultNodes.Add(node);
                }
            }

            protected override void OnSelectionChanged(SyncStatus status)
            {
                if (this.SelectedNodes.Count == 0)
                {
                    this.SelectionData.Clear();
                }
                else
                {
                    this.ActionsPaneItems.Clear();
                    this.SelectionData.ActionsPaneItems.Clear();
                    this.SelectionData.Update(null, this.SelectedNodes.Count > 1, null, null);
                    if (mgrConfig.Permitted("EditProfile"))
                        this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Edit profile", "Edit this profile", 6, "EditProfile"));
                    if (mgrConfig.Permitted("ViewXMLProfile"))
                        this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("View as XML", "View this profile as XML", 6, "ViewXMLProfile"));
                    if (mgrConfig.Permitted("CopyProfile"))
                        this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Copy profile", "Copy this profile", 6, "CopyProfile"));
                    if (mgrConfig.Permitted("DeleteProfile"))
                        this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Delete profile", "Delete this profile", 6, "DeleteProfile"));
                }
            }

            protected override void OnRefresh(AsyncStatus status)
            {
                Refresh();
            }

            protected override void OnSelectionAction(Microsoft.ManagementConsole.Action action, AsyncStatus status)
            {
                ProfileDb selProfile = (ProfileDb)this.SelectedNodes[0].Tag;

                switch ((string)action.Tag)
                {
                    case "EditProfile":
                        EditProfile editProfile = new EditProfile(selProfile.profile, false);
                        if (this.SnapIn.Console.ShowDialog(editProfile) == DialogResult.OK)
                        {                      
                            // Save the profile to the profiles directory
                            editProfile.profile.SaveXml(caInfo.ProfilesLocation);
                            Refresh();
                        }                      
                        break;

                    case "ViewXMLProfile":
                        ViewXml view = new ViewXml(selProfile.file);
                        this.SnapIn.Console.ShowDialog(view);
                        break;

                    case "CopyProfile":
                        EditProfile copyProfile = new EditProfile(selProfile.profile, true);
                        if (this.SnapIn.Console.ShowDialog(copyProfile) == DialogResult.OK)
                        {                      
                            // Save the profile to the profiles directory
                            copyProfile.profile.SaveXml(caInfo.ProfilesLocation);

                            // Update
                            caInfo.RefreshProfiles();
                            Refresh();
                        } 
                        break;

                    case "DeleteProfile":
                        DialogResult result = MessageBox.Show("Delete: " + selProfile.profile.Name, "Delete Profile", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.OK)
                        {
                            caInfo.RemoveProfile(selProfile);
                            Refresh();
                        }
                        break;
                }
            }
        }
    }