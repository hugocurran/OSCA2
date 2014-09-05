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
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using OSCA.Offline;
using OSCASnapin.CAinfo;
using OSCASnapin.ConfigManager;
using OSCA.Crypto;

namespace OSCASnapin
{
    public partial class CertSave : Form
    {
        private CaControl caInfo = null;
        internal X509Certificate cert = null;
        private Configuration config;
        private string fileName;

        public CertSave(CaControl caInfo)
        {
            this.caInfo = caInfo;
            config = Configuration.Instance;
            InitializeComponent();
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            saveFileDialog.InitialDirectory = config.OscaFolder + "\\Certificates";
            // default
            saveFileDialog.Filter = "X.509 certificate|*.cer";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = cert.SubjectDN.ToString();

            if (rbPk7.Checked)
                saveFileDialog.Filter = "PKCS#7 Data|*.p7b";
            if ((rbPEM.Checked) && (!rbPk7.Checked))
                 saveFileDialog.Filter = "PEM format certificate|*.pem";

            saveFileDialog.ShowDialog();
        }
       

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (rbDER.Checked)
            {
                if (rbPk7.Checked)
                    File.WriteAllBytes(saveFileDialog.FileName, pk7().GetEncoded());
                else
                    File.WriteAllBytes(saveFileDialog.FileName, cert.GetEncoded());
            }

            if (rbBase64.Checked)
            {
                if (rbPk7.Checked)
                    File.WriteAllText(saveFileDialog.FileName, Convert.ToBase64String(pk7().GetEncoded()));
                else
                    File.WriteAllText(saveFileDialog.FileName, Convert.ToBase64String(cert.GetEncoded()));            
            }

            if (rbPEM.Checked)
            {
                if (rbPk7.Checked)
                {
                    File.WriteAllText(saveFileDialog.FileName, Utility.ExportToPEM(pk7()));
                }
                else
                {
                    File.WriteAllText(saveFileDialog.FileName, Utility.ExportToPEM(cert));
                }
            }
        }

        private CmsSignedData pk7()
        {
            ArrayList certList = new ArrayList();

            if (cbPath.Checked)
            {
                certList.Add(caInfo.Certificate);
                certList.Add(cert);
            }
            else
                certList.Add(cert);
            
            // If CRL is required
            if (cbCRL.Checked)
            {
                ArrayList crlList = new ArrayList();
                crlList.Add(caInfo.GetCRL());
                return Pkcs7.Create(certList, crlList);
            }
            else
                return Pkcs7.Create(certList, null);
        }
    }
}
