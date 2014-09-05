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

using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.ManagementConsole;
using SystemX509 = System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.X509;
using OSCA;
using OSCASnapin.CAinfo;
using OSCASnapin.ConfigManager;

namespace OSCASnapin
{
    /// <summary>
    /// CertListView class - Loads and allows selection of certificates.
    /// </summary>
    internal class CertListView : MmcListView
    {
        private Configuration mgrConfig = Configuration.Instance;
        protected CaInfoContext context;

        // MUST be public
        public CertListView()
        {
        }

        protected override void OnShow()
        {
            base.OnShow();
            Refresh();
        }

        /// <summary>
        /// Define the ListView's structure 
        /// </summary>
        /// <param name="status">status for updating the console</param>
        protected override void OnInitialize(AsyncStatus status)
        {
            // do default handling
            base.OnInitialize(status);

            // Start listening for scope node events
            ((CertsNode)this.ScopeNode).Changed += new CertsNode.ChangedDelegate(OnScopeNodeChange);

            // get the certStatus
            context = (CaInfoContext)this.ViewDescriptionTag;

            // Create a set of columns for use in the list view
            this.Columns[0].Title = "Subject";
            this.Columns[0].SetWidth(200);

            if ((context.certStatus == CertStatus.Current) || (context.certStatus == CertStatus.Expired))
            {
                // Add detail column
                this.Columns.Add(new MmcListViewColumn("Issue Date", 100));
                this.Columns.Add(new MmcListViewColumn("Expiry Date", 100));
                this.Columns.Add(new MmcListViewColumn("Profile", 75));
                this.Columns.Add(new MmcListViewColumn("Serial Number", 200));
            }
            else
            {
                // Add detail columns
                this.Columns.Add(new MmcListViewColumn("Revocation Date", 100));
                this.Columns.Add(new MmcListViewColumn("Revocation Reason", 100));
                this.Columns.Add(new MmcListViewColumn("Profile", 75));
                this.Columns.Add(new MmcListViewColumn("Serial Number", 200));
            }
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
            ((CertsNode)this.ScopeNode).Changed -= new CertsNode.ChangedDelegate(OnScopeNodeChange);
        }

        /// <summary>
        /// Handle any change to the scope node. In this case just refresh views
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnScopeNodeChange(object sender, CertsNode.ChangedEventArgs e)
        {
            if (e.Status == "IssueCert")
            {
                Refresh();
            }
        }

        /// <summary>
        /// Define actions for selection  
        /// </summary>
        /// <param name="status">status for updating the console</param>
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

                switch (context.certStatus)
                {
                    case CertStatus.Current:
                        this.SelectionData.Update(null, this.SelectedNodes.Count > 1, null, null);

                        if (mgrConfig.Permitted("ViewCert"))
                            this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action
                                ("View", "View this certificate", 4, "ViewCert"));
                        if (mgrConfig.Permitted("ExportCert"))
                            this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action
                                ("Export", "Export this certificate", 4, "ExportCert"));

                        if (context.caInfo.CAStatus == CAstatus.Running)
                        {
                            if (mgrConfig.Permitted("RevokeCert"))
                                this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action
                                    ("Revoke", "Revoke this certificate", 4, "RevokeCert"));
                            if (mgrConfig.Permitted("RenewCert"))
                                this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action
                                    ("Renew", "Renew this certificate", 4, "RenewCert"));
                            if (mgrConfig.Permitted("RekeyCert"))
                                this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action
                                    ("Rekey", "Rekey this certificate", 4, "RekeyCert"));
                        }
                        break;
                       
                    case CertStatus.Revoked:
                        this.SelectionData.Update(null, this.SelectedNodes.Count > 1, null, null);

                        if (mgrConfig.Permitted("ViewCert"))
                            this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action
                                ("View", "View this certificate", 4, "ViewCert"));
                        if (mgrConfig.Permitted("ExportCert"))
                            this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action
                                ("Export", "Export this certificate", 4, "ExportCert"));

                        if (context.caInfo.CAStatus == CAstatus.Running)
                        {
                            if (mgrConfig.Permitted("UnRevokeCert"))
                                this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action
                                    ("UnRevoke", "Remove Revocation", 4, "UnRevokeCert"));
                        }
                         
                        break;

                    case CertStatus.Expired:
                        this.SelectionData.Update(null, this.SelectedNodes.Count > 1, null, null);

                        if (mgrConfig.Permitted("ViewCert"))
                            this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action
                                ("View", "View this certificate", 4, "ViewCert"));
                        if (mgrConfig.Permitted("ExportCert"))
                            this.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action
                                ("Export", "Export this certificate", 4, "ExportCert"));
                        break;
                }               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        protected override void OnRefresh(AsyncStatus status)
        {
            Refresh();
        }

        /// <summary>
        /// Handle short cut style menu actions for selection
        /// </summary>
        /// <param name="action">triggered action</param>
        /// <param name="status">asynchronous status used to update the console</param>
        protected override void OnSelectionAction(Microsoft.ManagementConsole.Action action, AsyncStatus status)
        {
            X509Certificate cert = (X509Certificate)this.SelectedNodes[0].Tag;

            switch ((string)action.Tag)
            {
               case "ViewCert":
                    SystemX509.X509Certificate2UI.DisplayCertificate(new SystemX509.X509Certificate2(cert.GetEncoded()));                    
                    break;

                case "ExportCert":
                    CertSave certSave = new CertSave(context.caInfo);
                    certSave.cert = cert;
                    this.SnapIn.Console.ShowDialog(certSave);
                    break;

                case "RevokeCert":
                    revokeCert revoke = new revokeCert(cert);
                    if (this.SnapIn.Console.ShowDialog(revoke) == DialogResult.OK)
                    {
                        context.caInfo.RevokeCertificate(cert, (CRLReason)revoke.cbReason.SelectedIndex);
                        Refresh();
                    }
                    break;

                case "RenewCert":

                    break;

                case "RekeyCert":
                    RekeyCert rekey = new RekeyCert(context.caInfo, cert );
                    if (this.SnapIn.Console.ShowDialog(rekey) == DialogResult.OK)
                    {                        
                        certSave = new CertSave(context.caInfo);
                        certSave.cert = rekey.cert;
                        this.SnapIn.Console.ShowDialog(certSave);
                        Refresh();
                    }
                    break;

                case "UnRevokeCert":
                        context.caInfo.UnRevokeCertificate(cert);
                        Refresh();
                    break;
            }
        }

        /// <summary>
        /// Loads the ListView with data
        /// </summary>
        internal void Refresh()
        {

            // Clear existing information
            this.ResultNodes.Clear();

            // Load current information
            List<DataBase> certs = context.caInfo.GetCerts(context.certStatus);
            foreach (var cert in certs)
            {
                ResultNode node = new ResultNode();

                node.DisplayName = cert.dn;
                if (context.certStatus == CertStatus.Revoked)
                {
                    node.ImageIndex = 5;
                    node.SubItemDisplayNames.Add(cert.revDate);
                    node.SubItemDisplayNames.Add(cert.revReason);
                    node.SubItemDisplayNames.Add(cert.profile);
                    node.SubItemDisplayNames.Add(cert.serialNumber);
                    node.Tag = cert.certificate;
                }
                else      // Current or Expired
                {
                    node.ImageIndex = 4;
                    node.SubItemDisplayNames.Add(cert.created);
                    node.SubItemDisplayNames.Add(cert.expiry);
                    node.SubItemDisplayNames.Add(cert.profile);
                    node.SubItemDisplayNames.Add(cert.serialNumber);
                    node.Tag = cert.certificate;
                }
                this.ResultNodes.Add(node);
            }

        }
    }
}
