using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSCASnapin.ConfigManager;

namespace OSCASnapin
{
    public partial class ImportCAKey : Form
    {
        internal AddCA addCA;
        internal string keyFile;
        internal string password;
        private Configuration mgrConfig = Configuration.Instance;

        internal ImportCAKey()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = mgrConfig.OscaFolder;
            openFileDialog1.Filter = "PKCS#12|*.p12";
            AcceptButton = butImport;
        }

        private void butBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                keyFile = openFileDialog1.FileName;
                tbKeyFile.Text = openFileDialog1.FileName;
            }

        }

        private void butImport_Click(object sender, EventArgs e)
        {
            password = tbPassword.Text;
        }

    }
}