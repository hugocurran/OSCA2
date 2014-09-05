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
using Org.BouncyCastle.X509;
using OSCASnapin.ConfigManager;
using System;


namespace OSCASnapin
{
    class CertsNode : ScopeNode
    {
        private Configuration mgrConfig = Configuration.Instance;
        internal CaControl caInfo;
        internal CertStatus status { get; set; }
        
        internal CertsNode(CaControl info, CertStatus certStatus)
        {
            caInfo = info;
            status = certStatus;

            this.ImageIndex = 3;
            this.SelectedImageIndex = 3;

            // Create the CaInfoContext instance for use by the ListView
            CaInfoContext context = new CaInfoContext(info) { certStatus = status };

            // Create a ListView for the node.
            MmcListViewDescription lvd = new MmcListViewDescription();
            lvd.DisplayName = caInfo.CAName;
            lvd.ViewType = typeof(CertListView);
            lvd.Options = MmcListViewOptions.ExcludeScopeNodes | MmcListViewOptions.SingleSelect;
            lvd.Tag = context;
           
            this.ViewDescriptions.Add(lvd);
            this.ViewDescriptions.DefaultIndex = 0;

            Refresh();
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
            if ((status == CertStatus.Current) && (caInfo.CAStatus == CAstatus.Running))
                if (mgrConfig.Permitted("IssueCert"))
                    this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Issue Certificate", "Issue a new Certificate", 4, "IssueCert"));

            if ((status == CertStatus.Revoked) && (caInfo.CAStatus == CAstatus.Running))
                if (mgrConfig.Permitted("IssueCRL"))
                    this.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Create CRL", "Create a new CRL", 5, "IssueCRL"));
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
                case "IssueCert":     //issue cert
                    IssCert issue = new IssCert(caInfo);
                    if (this.SnapIn.Console.ShowDialog(issue) == DialogResult.OK)
                    {                        
                        CertSave certSave = new CertSave(caInfo);
                        certSave.cert = issue.cert;
                        this.SnapIn.Console.ShowDialog(certSave);

                        // notify any listening views that a change happened
                        RaiseOnChange((string)action.Tag);
                    }
                    break;

                case "IssueCRL": // Issue CRL
                    string result = caInfo.IssueCRL();
                    X509Crl crl = caInfo.GetCRL();
                    // Save the result
                    CRLsave crlSave = new CRLsave();
                    crlSave.crl = crl;
                    this.SnapIn.Console.ShowDialog(crlSave);
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

    internal class CaInfoContext
    {
        internal CaControl caInfo { get; set; }
        internal CertStatus certStatus { get; set; }

        internal CaInfoContext(CaControl info)
        {
            this.caInfo = info;
        }
    }
}
