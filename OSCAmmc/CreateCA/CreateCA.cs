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
using System.IO;
using System.Windows.Forms;
using OSCA.Offline;
using Org.BouncyCastle.Asn1.X509;
using OSCASnapin.ConfigManager;
using OSCASnapin.CAinfo;

namespace OSCASnapin
{
    public partial class CreateCA : Form
    {
        private CA_Profile profile = CA_Profile.rootCA;
        private X509ver version = X509ver.V3;
        private string profileFile;
        // Defaults
        private string pkAlgo = "RSA";
        private int pkSize = 2048;
        private string sigAlgo = "SHA1WITHRSA";
        private bool checkFlag = false; // gets around 'double' call when rb changed

        internal Configuration mgrConfig;   // Set by the caller

        public CreateCA()
        {
            InitializeComponent();
            lbCertUnits.SelectedIndex = 0;
        }

        // Set the folder to store the CA files
        private void butBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = mgrConfig.OscaFolder;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                tbFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void CAtype_CheckedChanged(object sender, EventArgs e)
        {
            // Updated for V3 CA configs

            if ((rbRoot.Checked) || (rbTA.Checked))
            {
                gpCertValidity.Enabled = true;
                gpCertType.Enabled = true;
                profile = CA_Profile.rootCA;
                profileFile = "";
                gpIssuingCA.Enabled = false;

                // TA is always a V1 cert
                if (rbTA.Checked)
                {
                    tbCRLInterval.Text = "0";
                    rbV1.Checked = true;
                    cbFIPS.Checked = true;
                }
            }
            else if (rbSubordinate.Checked)
            {
                // This gets called twice: when one button is unchecked and when the new one is checked
                // Workaround by setting the flag so we only process the CA list once
                if (!checkFlag)
                {
                    checkFlag = true;
                    gpCertValidity.Enabled = false;
                    gpCertType.Enabled = false;
                    profile = CA_Profile.SubCA;
                    profileFile = "subCA";

                    // Get a list of possible issuing CAs
                    foreach (CA ca in mgrConfig.CaList)
                    {
                        if (ca.CaControl.CAStatus == CAstatus.Running)
                        {
                            lbIssuingCA.Items.Add(ca.CaName);

                            // ToDo is not to add a CA if its policy forbids it acting as an issuer eg PathLen = 0
                        }
                    }
                    if (lbIssuingCA.Items.Count < 1)
                    {
                        MessageBox.Show("No CAs are available (STARTED)", "Create CA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DialogResult = DialogResult.Cancel;
                    }
                    else
                    {
                        gpIssuingCA.Enabled = true;
                        lbIssuingCA.SelectedIndex = 0;  // Should trigger a profiles search
                        lbIssuingCA_SelectedIndexChanged(sender, e);
                    }
                }
                else
                {
                    checkFlag = false;
                }
            }
        }

        private void certType_CheckedChanged(object sender, EventArgs e)
        {
            if (rbV1.Checked)
            {
                // v1 cert, so no extensions
                tbCRLInterval.Enabled = false;
                crlPubInt.Enabled = false;
                version = X509ver.V1;
            }
            else if (rbV3.Checked)
            {
                tbCRLInterval.Enabled = true;
                crlPubInt.Enabled = true;
                version = X509ver.V3;
            }
        }

        private void rbAlg_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDSA.Checked)
            {
                rb1024.Enabled = true;
                rb1024.Checked = true;
                rb2048.Enabled = true;
                rb2048.Checked = false;
                rb4096.Enabled = false;
                rb4096.Checked = false;
                rb256.Enabled = false;
                rb256.Checked = false;
                rbSHA1.Enabled = true;
                rbSHA1.Checked = true;
                rbSHA256.Enabled = false;
                rbSHA256.Checked = false;
                pkAlgo = "DSA";
            }
            else if (rbRSA.Checked)
            {
                rb1024.Enabled = true;
                rb2048.Enabled = true;
                rb4096.Enabled = true;
                rb256.Enabled = false;
                rb256.Checked = false;
                rbSHA1.Enabled = true;
                rbSHA256.Enabled = true;
                pkAlgo = "RSA";
            }
            else if (rbECDSA.Checked)
            {
                rb1024.Enabled = false;
                rb1024.Checked = false;
                rb2048.Enabled = false;
                rb2048.Checked = false;
                rb4096.Enabled = false;
                rb4096.Checked = false;
                rb256.Enabled = true;
                rb256.Checked = true;
                rbSHA1.Enabled = false;
                rbSHA1.Checked = false;
                rbSHA256.Enabled = true;
                rbSHA256.Checked = true;
                pkAlgo = "ECDSA";
            }
        }

        private void KeySize_CheckedChanged(object sender, EventArgs e)
        {
            if (rb1024.Checked)
            {
                //rb1024.Enabled = true;
                //rb1024.Checked = true;
                //rb2048.Enabled = false;
                //rb2048.Checked = false;
                //rb4096.Enabled = false;
                //rb4096.Checked = false;
                //rb256.Enabled = false;
                //rb256.Checked = false;
                rbSHA1.Enabled = true;
                //rbSHA1.Checked = true;
                rbSHA256.Enabled = true;
                //rbSHA256.Checked = false;
                pkSize = 1024;
            }
            else if (rb2048.Checked)
            {
                //rb1024.Enabled = true;
                //rb1024.Checked = true;
                //rb2048.Enabled = false;
                //rb2048.Checked = false;
                //rb4096.Enabled = false;
                //rb4096.Checked = false;
                //rb256.Enabled = false;
                //rb256.Checked = false;
                rbSHA1.Enabled = true;
                //rbSHA1.Checked = true;
                rbSHA256.Enabled = true;
                //rbSHA256.Checked = false;
                rbDSA.Enabled = true;
                rbDSA.Checked = false;
                pkSize = 2048;
            }
            else if (rb4096.Checked)
            {
                //rb1024.Enabled = true;
                //rb1024.Checked = true;
                //rb2048.Enabled = false;
                //rb2048.Checked = false;
                //rb4096.Enabled = false;
                //rb4096.Checked = false;
                //rb256.Enabled = false;
                //rb256.Checked = false;
                rbSHA1.Enabled = true;
                //rbSHA1.Checked = true;
                rbSHA256.Enabled = true;
                //rbSHA256.Checked = false;
                rbDSA.Enabled = false;
                rbDSA.Checked = false;
                pkSize = 4096;
            }
            else if (rb256.Checked)
            {
                //rb1024.Enabled = true;
                //rb1024.Checked = true;
                //rb2048.Enabled = false;
                //rb2048.Checked = false;
                //rb4096.Enabled = false;
                //rb4096.Checked = false;
                //rb256.Enabled = false;
                //rb256.Checked = false;
                rbSHA1.Enabled = true;
                //rbSHA1.Checked = true;
                rbSHA256.Enabled = true;
                //rbSHA256.Checked = false;
                rbDSA.Enabled = false;
                rbDSA.Checked = false;
                rbRSA.Enabled = false;
                rbRSA.Checked = false;
                rbECDSA.Enabled = true;
                rbECDSA.Checked = true;
                pkSize = 256;
            }
        }

        private void HashAlgo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSHA1.Checked)
            {
                rb1024.Enabled = true;
                //rb1024.Checked = true;
                rb2048.Enabled = true;
                //rb2048.Checked = false;
                rb4096.Enabled = true;
                //rb4096.Checked = false;
                rb256.Enabled = false;
                rb256.Checked = false;
                rbECDSA.Enabled = false;
                rbECDSA.Checked = false;
                sigAlgo = "SHA1WITH";
            }
            else if (rbSHA256.Checked)
            {
                rb1024.Enabled = true;
                //rb1024.Checked = true;
                rb2048.Enabled = true;
                //rb2048.Checked = false;
                rb4096.Enabled = true;
                //rb4096.Checked = false;
                rb256.Enabled = true;
                //rb256.Checked = false;
                rbECDSA.Enabled = true;
                //rbECDSA.Checked = false;
                sigAlgo = "SHA256WITH";
            }
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            tbDN.Text = "CN=" + tbName.Text;
        }

        private void butCreate_Click(object sender, EventArgs e)
        {
            CAConfig caConfig;
            // Assemble all the info

            try
            {
                caConfig = new CAConfig()
                {
                    name = tbName.Text,
                    DN = new X509Name(true, tbDN.Text),     // 'true' reverses the name to have C= on the left
                    profile = profile,
                    profileFile = profileFile,
                    pkAlgo = pkAlgo,
                    pkSize = pkSize,
                    sigAlgo = sigAlgo + pkAlgo,
                    keyUsage = 0x06,                    // Hardwired to CertSign|CRLSign per RFC 5280
                    version = version,
                    life = Convert.ToInt32(tbCertValid.Text),
                    units = lbCertUnits.Text,
                    location = tbFolder.Text,                    
                    crlInterval = Convert.ToInt32(tbCRLInterval.Text)
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Create CA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cbFIPS.Checked)
            {
                if (rbTA.Checked)
                    caConfig.caType = CA_Type.dhTA;
                else if (!rbECDSA.Checked)
                    caConfig.caType = CA_Type.sysCA;
                else
                    caConfig.caType = CA_Type.cngCA;
                caConfig.FIPS140 = true;
            }
            else
            {
                if (rbTA.Checked)
                {
                    MessageBox.Show("CA of type TA is only supported with FIPS crypto", "Create CA", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    DialogResult = DialogResult.Cancel;
                }

                caConfig.caType = CA_Type.bcCA;
                caConfig.FIPS140 = false;

                // Set a password
                Setpassword password = new Setpassword();
                if (password.ShowDialog() == DialogResult.OK)
                    caConfig.password = password.tbPassword.Text;
                else
                    // Abort
                    DialogResult = DialogResult.Cancel;
             }

            // Now lets create the CA
            CA newCA = new CA();

            // Create the profiles directory
            Directory.CreateDirectory(tbFolder.Text + "\\Profiles");

            switch (caConfig.profile)
            {
                case CA_Profile.rootCA:
                    newCA.CaName = caConfig.name;
                    newCA.Role = "rootCA";

                    // Create the CA
                    newCA.ConfigLocation = CaFactory.CreateRootCA(caConfig);
                    break;

                case CA_Profile.SubCA:
                    // find the CA entry
                    CA issuingCA = mgrConfig.CaList.Find(m => m.CaName == lbIssuingCA.Text);
                    if (issuingCA.CaControl.CAStatus == CAstatus.Stopped)
                    {
                        MessageBox.Show("Issuing CA is STOPPED", "Create CA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DialogResult = DialogResult.Cancel;
                    }
                    //find the Profile entry
                    ProfileDb profile = issuingCA.CaControl.Profiles.Find(m => m.profile.Name == lbProfile.Text);
                    caConfig.profileFile = profile.file;

                    // populate the CA entry
                    newCA.CaName = caConfig.name;
                    newCA.Role = "subCA";

                    // Create the CA
                    try
                    {
                        newCA.ConfigLocation = CaFactory.CreateSubCA(caConfig, issuingCA.CaControl);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was a problem: " + ex.Message, "OSCA - Issue Certificate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        butCancel_Click(null, null);
                    }
                    break;
            }
            // Add the new CA into the list
            mgrConfig.InsertCA(newCA);
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.Cancel;
            //Dispose();
        }

        private void lbIssuingCA_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get a list of profiles for the currently selected issuing CA
            CA issuingCA = mgrConfig.CaList.Find(m => m.CaName == lbIssuingCA.Text);
            foreach (ProfileDb profile in issuingCA.CaControl.Profiles)
            {
                lbProfile.Items.Add(profile.profile.Name);
            }
            // Need this check in case there are no profiles defined
            if (issuingCA.CaControl.Profiles.Count != 0)
                lbProfile.SelectedIndex = 0;
        }

    }
}
