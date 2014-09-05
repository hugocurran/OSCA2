using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using OSCASnapin.CAinfo;
using OSCASnapin.ConfigManager;
using System.IO;
using OSCA;
using OSCA.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace OSCASnapin
{
    public partial class AddCA : Form
    {
        private string configFile;
        private string configFolder;
        private string keyFile = "";
        private string password = "";

        private Configuration mgrConfig = Configuration.Instance;

        internal AddCA()
        {
            InitializeComponent();

            configFolder = mgrConfig.OscaFolder;
            openFileDialog1.InitialDirectory = configFolder;
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            if (cbImport.Checked && (keyFile == ""))
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            configFile = tbConfigFile.Text;

            CA newCA = new CA();

            // Load the data from the CA config file
            XDocument config = XDocument.Load(configFile);
            newCA.CaName = config.Element("OSCA").Element("CA").Element("name").Value;
            newCA.Role = config.Element("OSCA").Element("CA").Element("type").Value;
            newCA.ConfigLocation = configFile;
            bool fips140 = Convert.ToBoolean(config.Element("OSCA").Element("CA").Element("fips140").Value);

            // If this is a FIPS CA then load the keyfile if requested, otherwise ignore the keyfile
            if (fips140 && cbImport.Checked)
            {
                X509CertificateParser cp = new X509CertificateParser();
                X509Certificate caCert = cp.ReadCertificate(Convert.FromBase64String(config.Element("OSCA").Element("CA").Element("caCert").Value));

                // Import the keyfile
                byte[] p12 = File.ReadAllBytes(keyFile);
                X509Certificate importCert = null;
                try
                {
                    importCert = SysKeyManager.ImportFromP12(p12, null, newCA.CaName, password.ToCharArray());
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Import key file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                if (!caCert.SerialNumber.Equals(importCert.SerialNumber))
                {
                    MessageBox.Show("Certificate in PKCS#12 does not match certificate in CA Config file", "Import key file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    DialogResult = DialogResult.Cancel;
                    return;
                }
            }

            mgrConfig.InsertCA(newCA);
            DialogResult = DialogResult.OK;
        }

        private void butBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Check it really is a CA config file
                // We handle either V2.x or V3.x configs, so need to check the version
                XDocument config;
                try
                {
                    config = XDocument.Load(openFileDialog1.FileName);
                    if (config.Element("OSCA").Element("CA") == null)
                    {
                        MessageBox.Show("Not a valid OSCA Config File: " + openFileDialog1.FileName, "Add CA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Corrupted Config File: " + openFileDialog1.FileName + "\n" + ex.Message, "Add CA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                // Check the database file is present
                string dbFile = config.Element("OSCA").Element("CA").Element("dbFileLocation").Value;
                try
                {
                    XDocument db = XDocument.Load(dbFile);
                    if (db.Element("OSCA") == null)
                    {
                        MessageBox.Show("Not a valid OSCA DB File: " + dbFile, "Add CA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Corrupted DB File: " + dbFile + "\n" + ex.Message, "Add CA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                tbConfigFile.Text = openFileDialog1.FileName;

            }
        }

        private void cbImport_CheckedChanged(object sender, EventArgs e)
        {
            ImportCAKey import = new ImportCAKey();
            if (import.ShowDialog() == DialogResult.OK)
            {
                keyFile = import.keyFile;
                password = import.password;
            }

        }
    }
}
