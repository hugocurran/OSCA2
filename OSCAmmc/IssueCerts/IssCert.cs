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

using System;
using System.Windows.Forms;
using Org.BouncyCastle.X509;
using OSCA;
using OSCA.Offline;
using OSCA.Profile;
using OSCASnapin.CAinfo;
using OSCASnapin.ConfigManager;
using System.Drawing;

namespace OSCASnapin
{
    public partial class IssCert : Form
    {

        internal CaControl caInfo = null;
        internal X509Certificate cert;
        private Configuration config;
        private Pkcs10Parser parser = null;

        public IssCert(CaControl info)
        {
            caInfo = info;
            config = Configuration.Instance;
            InitializeComponent();
            lbValidUnits.SelectedText = "Years";

            // Populate the list of profiles in the selectBox
            lbProfiles.Items.Add("none");
            foreach (ProfileDb profile in caInfo.Profiles)
            {
                lbProfiles.Items.Add(profile.profile.Name);
            }
        }

        #region Import request from file

        private void butBrowse_Click(object sender, EventArgs e)
        {
            gbPaste.Enabled = false;

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
                    }
                    else
                    {
                        gbPaste.Enabled = true;
                        butSubmit.Enabled = false;
                    }
                }
                catch (ApplicationException ex)
                {
                    setStatus("Invalid request format", false);
                    MessageBox.Show("Not a valid request format: " + ex.Message, "OSCA - Issue Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    gbPaste.Enabled = true;
                    butSubmit.Enabled = false;
                    return;
                }
            }
        }

        #endregion

        #region Paste request

        private void tbPaste_TextChanged(object sender, EventArgs e)
        {
            try
            {
                parser = Pkcs10Parser.ReadRequestFromString(tbPaste.Text);
                if ((parser != null) && (rqstInfo()))
                {
                    gbFile.Enabled = false;
                    butSubmit.Enabled = true;
                }
                else
                {
                    gbFile.Enabled = true;
                    butSubmit.Enabled = false;
                }
            }
            catch (ApplicationException ex)
            {
                setStatus("Invalid request format", false);
                MessageBox.Show("Not a valid request format: " + ex.Message, "OSCA - Issue Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                gbFile.Enabled = true;
                butSubmit.Enabled = false;
                return;
            }
        }

        #endregion

        #region Handle profiles

        private void cbProfile_CheckedChanged(object sender, EventArgs e)
        {
            if (cbProfile.Checked)
            {
                tbValidPeriod.Enabled = false;
                lbValidUnits.Enabled = false;
                lbProfiles.Enabled = true;
                lbProfiles.SelectedIndex = 0;   // none
            }
            else
            {
                tbValidPeriod.Enabled = true;
                lbValidUnits.Enabled = true;
                lbProfiles.Enabled = false;
                lbProfiles.SelectedIndex = -1;
            }
        }

        private void lbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbProfiles.Text == "none")
            {
                tbValidPeriod.Text = "0";
                lbValidUnits.SelectedIndex = -1;
            }
            else
            {
                tbValidPeriod.Text = caInfo.Profiles[lbProfiles.SelectedIndex - 1].profile.CertificateLifetime.Period.ToString();
                lbValidUnits.Text = caInfo.Profiles[lbProfiles.SelectedIndex - 1].profile.CertificateLifetime.Units.ToString();
            }
        }

        #endregion

        #region Issue Certificate

        private void butSubmit_Click(object sender, EventArgs e)
        {
            // Get the profile
            string profileFile;
            if (cbProfile.Checked)
            {
                if (lbProfiles.Text == "none")
                {
                    MessageBox.Show("Profile not selected", "OSCA - Issue Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                profileFile = caInfo.Profiles[lbProfiles.SelectedIndex - 1].file;

                // Issue cert
                try
                {
                    cert = caInfo.IssueCertificate(parser.Request, new Profile(profileFile));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was a problem: " + ex.Message, "OSCA - Issue Certificate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    butCancel_Click(null, null);
                }
            }
            else
            {
                profileFile = null;
                if ((tbValidPeriod.Text == "") || (Convert.ToInt32(tbValidPeriod.Text) < 1))
                {
                    MessageBox.Show("Invalid certificate validity period", "OSCA - Issue Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                ValidityPeriod validity = new ValidityPeriod(lbValidUnits.Text, Convert.ToInt32(tbValidPeriod.Text));
                
                //Issue cert
                try
                {
                    cert = caInfo.IssueCertificate(parser.Request, validity);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was a problem: " + ex.Message, "OSCA - Issue Certificate", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Verify request signature
            if (parser.IsValid)
            {
                setStatus("Valid signature", true);
                return true;
            }
            else
            {
                setStatus("Invalid signature", false);
                return false;
            }
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
