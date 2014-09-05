﻿/*
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
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Org.BouncyCastle.X509;
using OSCA.Offline;
using OSCASnapin.CAinfo;
using OSCASnapin.ConfigManager;
using System.Text;

namespace OSCASnapin
{
    public partial class CRLsave : Form
    {
        public X509Crl crl;
        private Configuration config;

        public CRLsave()
        {
            config = Configuration.Instance;
            InitializeComponent();
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            if ((rbDER.Checked) || (rbBase64.Checked))
                saveFileDialog.Filter = "X.509 CRL|*.crl";
            else
                saveFileDialog.Filter = "PEM format CRL|*.pem";

            saveFileDialog.InitialDirectory = config.OscaFolder + "\\CRLs";
            saveFileDialog.ShowDialog();
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (rbDER.Checked)
            {
                File.WriteAllBytes(saveFileDialog.FileName, crl.GetEncoded());
            }
            if (rbBase64.Checked)
            {
                File.WriteAllText(saveFileDialog.FileName, Convert.ToBase64String(crl.GetEncoded()));
            }
            if (rbPEM.Checked)
            {
                File.WriteAllText(saveFileDialog.FileName, Utility.ExportToPEM(crl));
            }
        }
    }
}
