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

using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace OSCASnapin
{
    public partial class InitialisationWizard : Form
    {

        internal InitialisationData initData = new InitialisationData();
        
        public InitialisationWizard()
        {
            InitializeComponent();
            initData.configFile = "";
        }

        private void butBrowse_Click(object sender, EventArgs e)
        {
            cbOverwrite.Enabled = false;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                tbConfigFolder.Text = folderBrowserDialog1.SelectedPath;

            // Check if there is already a config file
            string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "MgrConfig.xml");
            if (files.Count() > 0)
            {
                cbOverwrite.Enabled = true;
            }
        }

        private void butInstall_Click(object sender, EventArgs e)
        {
            initData.configFolder = tbConfigFolder.Text;
            if (cbOverwrite.Checked)
                initData.configFile = initData.configFolder + @"\MgrConfig.xml";
            initData.adminPasswordHash = Password.hashPassword(tbAdminPassword.Text);
        }

    }
}
