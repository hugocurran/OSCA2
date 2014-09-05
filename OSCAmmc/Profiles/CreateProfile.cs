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
using OSCA.Profile;
using OSCA;

namespace OSCASnapin.Profiles
{
    public partial class CreateProfile : Form
    {
        internal Profile profile = new Profile();

        public CreateProfile()
        {
            InitializeComponent();
            lbMaster.Items.AddRange(Profile.OSCAExtensions);
            cbLifetimeUnits.SelectedItem = "Years";
            cbOverlapUnits.SelectedItem = "Hours";
        }

        // Create an extension
        private void butAdd_Click(object sender, EventArgs e)
        {
            string item = lbMaster.SelectedItem.ToString();
            if (ProfileEditor.CreateExtension(profile, item))
            {
                lbProfile.Items.Add(lbMaster.SelectedItem);
                lbMaster.Items.RemoveAt(lbMaster.SelectedIndex);
            }
        }

        // Remove an extension
        private void butDelete_Click(object sender, EventArgs e)
        {
            if (lbProfile.SelectedItem == null)
                return;

            string item = lbProfile.SelectedItem.ToString();
            lbMaster.Items.Add(lbProfile.SelectedItem);
            lbProfile.Items.RemoveAt(lbProfile.SelectedIndex);

            ProfileEditor.RemoveExtension(profile, item);
        }

        // Edit an extension
        private void extension_DoubleClick(object sender, EventArgs e)
        {
            string item = lbProfile.SelectedItem.ToString();

            ProfileEditor.EditExtension(profile, item);
        }

        private void butCreate_Click(object sender, EventArgs e)
        {
            profile.Name = tbName.Text;
            profile.Version = tbVersion.Text;
            profile.Description = tbDescription.Text;
            profile.CertificateLifetime = new ValidityPeriod(cbLifetimeUnits.SelectedItem.ToString(), Convert.ToInt32(tbLifetime.Text));
            profile.RenewOverlapPeriod = new ValidityPeriod(cbOverlapUnits.SelectedItem.ToString(), Convert.ToInt32(tbOverlap.Text));
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
