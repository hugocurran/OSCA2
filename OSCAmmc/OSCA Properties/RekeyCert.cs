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
using System.Collections.Generic;
using System.Windows.Forms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using OSCA;
using OSCA.Offline;
using OSCA.Profile;
using OSCASnapin.CAinfo;
using OSCASnapin.ConfigManager;
using System.Drawing;

namespace OSCASnapin
{
    public partial class RekeyCert : Form
    {

        internal CaControl caInfo = null;
        internal X509Certificate cert;
        private X509Certificate origCert;
        private Configuration config;
        private Pkcs10Parser parser = null;
        private DateTime notBefore;
        private DateTime notAfter;

        internal RekeyCert(CaControl caInfo, X509Certificate origCert)
        {
            this.caInfo = caInfo;
            this.origCert = origCert;
            config = Configuration.Instance;
            InitializeComponent();
            lblStatusString.Visible = false;
            ckbProfile.Enabled = false;
            ckbProfile.Checked = false;
            lbProfiles.Enabled = false;
            ckbProfile.Enabled = false;
            ckbOverride.Checked = false;
            tbNotBefore.Enabled = false;
            tbNotAfter.Enabled = false;
            butSubmit.Enabled = false;
            
            // Populate the list of profiles in the selectBox
            lbProfiles.Items.Add("none");
            foreach (ProfileDb profile in caInfo.Profiles)
            {
                lbProfiles.Items.Add(profile.profile.Name);
            }

            // Check to see if the orig cert used a profile
            List<DataBase> certDb = caInfo.GetCerts(CertStatus.Current);
            DataBase certDbEntry = certDb.Find(c => c.serialNumber == origCert.SerialNumber.ToString());
            if (certDbEntry.profile != "")
            {
                ckbProfile.Checked = true;
                lbProfiles.SelectedItem = certDbEntry.profile;
                // need to make sure the index is updated?
            }
            else
                lbProfiles.SelectedItem = "none";
        }

        #region Import request from file

        private void butBrowse_Click(object sender, EventArgs e)
        {
            // Hide the status string in case this is a second attempt
            lblStatusString.Visible = false;

            openFileDialog.InitialDirectory = config.OscaFolder + "\\Requests";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string requestFile = openFileDialog.FileName;
                tbFile.Text = requestFile;

                try
                {
                    parser = Pkcs10Parser.ReadRequestFromFile(requestFile);
                    if ((parser != null) && (rqstInfo()))
                    {
                        gbPaste.Enabled = false;
                        butSubmit.Enabled = true;
                        ckbProfile.Enabled = true;
                        ckbOverride.Enabled = true;
                    }
                    else
                    {
                        gbPaste.Enabled = true;
                        butSubmit.Enabled = false;
                        ckbProfile.Enabled = false;
                        ckbOverride.Enabled = false;
                    }
                }
                catch (ApplicationException ex)
                {
                    setStatus("Invalid request format", false);
                    MessageBox.Show("Not a valid request format: " + ex.Message, "OSCA - Rekey Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    gbPaste.Enabled = true;
                    butSubmit.Enabled = false;
                    ckbProfile.Enabled = false;
                    ckbOverride.Enabled = false; 
                    return;
                }
            }
        }

        #endregion

        #region Paste request

        private void tbPaste_TextChanged(object sender, EventArgs e)
        {
            // Hide the status string in case this is a second attempt
            lblStatusString.Visible = false;

            try
            {
                parser = Pkcs10Parser.ReadRequestFromString(tbPaste.Text);
                if ((parser != null) && (rqstInfo()))
                {
                    gbFile.Enabled = false;
                    butSubmit.Enabled = true;
                    ckbProfile.Enabled = true;
                    ckbOverride.Enabled = true;
                }
                else
                {
                    gbFile.Enabled = true;
                    butSubmit.Enabled = false;
                    ckbProfile.Enabled = false;
                    ckbOverride.Enabled = false;
                }
            }
            catch (ApplicationException ex)
            {
                setStatus("Invalid request format", false);
                MessageBox.Show("Not a valid request format: " + ex.Message, "OSCA - Rekey Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                gbFile.Enabled = true;
                butSubmit.Enabled = false;
                ckbProfile.Enabled = false;
                ckbOverride.Enabled = false;
                return;
            }
        }
          
        #endregion

        #region Handle profiles

        private void cbProfile_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbProfile.Checked)
            {
                ckbOverride.Checked = false;
                ckbOverride.Enabled = false;
                lbProfiles.Enabled = true;
                lbProfiles.SelectedItem = "none";
            }
            else
            {
                ckbOverride.Checked = false;
                ckbOverride.Enabled = true;
                lbProfiles.Enabled = false;
                lbProfiles.SelectedIndex = -1;
            }
        }

        private void lbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbProfiles.SelectedItem.ToString() == "none")
            {
                // no profile so set notBefore/notAfter to today
                tbNotBefore.Text = DateTime.Now.ToUniversalTime().ToShortDateString();
                tbNotAfter.Text = tbNotBefore.Text;
                ckbOverride.Checked = false;
                ckbOverride.Enabled = true;
            }
            else
            {
                // Get the profile
                //string profileFile = caInfo.Profiles[lbProfiles.SelectedIndex - 1].file;
                //Profile profile = new Profile(profileFile);
                Profile profile = caInfo.Profiles[lbProfiles.SelectedIndex - 1].profile;

                // Calculate the validity using info from the profile and certificate
                notAfter = profile.CertificateLifetime.NotAfter(origCert.NotAfter); 
                DateTime simpleStart = notAfter.Subtract(profile.CertificateLifetime.ToTimeSpan());
                notBefore = simpleStart.Subtract(profile.RenewOverlapPeriod.ToTimeSpan());
                // notBefore cannot be older than Now()
                if (notBefore < DateTime.UtcNow)
                    notBefore = DateTime.UtcNow;

                tbNotBefore.Text = notBefore.ToUniversalTime().ToString();
                tbNotAfter.Text = notAfter.ToUniversalTime().ToString();
                ckbOverride.Checked = false;
                ckbOverride.Enabled = true;
            }
        }

        private void ckbOverride_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbOverride.Checked)
            {
                tbNotBefore.Enabled = true;
                tbNotAfter.Enabled = true;
            }
            else
            {
                tbNotBefore.Enabled = false;
                tbNotAfter.Enabled = false;
            }
        }

        #endregion

        #region Issue Certificate

        private void butSubmit_Click(object sender, EventArgs e)
        {
            // Get the profile
            //string profileFile;
            Profile profile;

            if (ckbProfile.Checked)
            {
                if (lbProfiles.SelectedItem.ToString() == "none")
                {
                    MessageBox.Show("Profile not selected", "OSCA - Rekey Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                profile = caInfo.Profiles[lbProfiles.SelectedIndex - 1].profile;

                // Issue cert
                try
                {
                    cert = caInfo.IssueCertificate(parser.Request, profile, notBefore, notAfter);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was a problem: " + ex.Message, "OSCA - Rekey Certificate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {

                //profileFile = null; 
                if ((tbNotBefore.Text == "") || (tbNotAfter.Text == ""))
                {
                    MessageBox.Show("Invalid certificate notBefore/notAfter date", "OSCA - Rekey Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (tbNotBefore.Text == tbNotAfter.Text)
                {
                    MessageBox.Show("Invalid certificate notBefore/notAfter date", "OSCA - Rekey Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //Issue cert
                try
                {
                    notBefore = DateTime.Parse(tbNotBefore.Text);
                    notAfter = DateTime.Parse(tbNotAfter.Text);
                    if (notAfter < notBefore)
                    {
                        MessageBox.Show("Invalid certificate notBefore/notAfter date", "OSCA - Rekey Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    cert = caInfo.IssueCertificate(parser.Request, null, notBefore, notAfter);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was a problem: " + ex.Message, "OSCA - Rekey Certificate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    butCancel_Click(null, null);
                }
                
            }
        }

        #endregion

        #region Tools

        private void butCancel_Click(object sender, EventArgs e)
        {
        }

        private bool rqstInfo()
        {
            lbSubject.Text = "Subject: " + Utility.OrderDN(parser.Subject.ToString());
            if (parser.SubjectAltNames != null)
                lbAltNames.Text = "Alt Names: " + parser.SubjectAltNames.ToString();
            else
                lbAltNames.Text = "Alt Names: none";

            // Key
            lbKeyType.Text = "Key: " + parser.SubjectPublicKeyDescription;

            // Tests
            // 1. Verify request signature
            if (parser.IsValid)
                setStatus("Valid Signature", true);
            else
            {
                setStatus("Invalid Signature", false);
                return false;
            }

            // 2. Verify Request Subject matches Original Cert Subject
            if (!parser.Subject.Equivalent(origCert.SubjectDN))
            {
                setStatus("Subject does not match original certificate", false);   
                return false;
            }

            // 3.  Verify that Public Key is not same as that in Original Cert
            SubjectKeyIdentifier skid = new SubjectKeyIdentifier(parser.SubjectPublicKeyInfo);
            SubjectKeyIdentifier okid = new SubjectKeyIdentifier(origCert.CertificateStructure.SubjectPublicKeyInfo);
            if (skid.Equals(okid))
            {
                setStatus("Public key is the same as original certificate", false);
                return false;
            }
            return true;
        }

        private void setStatus(string status, bool validStatus)
        {
            lblStatusString.Text = "Status: " + status;
            if (validStatus)
                lblStatusString.ForeColor = Color.Green;
            else
                lblStatusString.ForeColor = Color.Red;
            lblStatusString.Visible = true;
        }

        #endregion

    }
}

