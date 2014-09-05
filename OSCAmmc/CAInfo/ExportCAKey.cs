using System;
using System.Windows.Forms;
using OSCASnapin.ConfigManager;

namespace OSCASnapin
{
    public partial class ExportCAKey : Form
    {
        private Configuration mgrConfig = Configuration.Instance;
        internal string password;
        internal string location;

        public ExportCAKey()
        {
            InitializeComponent();
            AcceptButton = butOK;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (string.Equals(tbPassword1.Text, tbPassword2.Text, StringComparison.Ordinal))
            {
                location = tbLocation.Text;
                password = tbPassword1.Text;
                DialogResult = DialogResult.OK;
                return;
            }
            
            MessageBox.Show("Passwords do not match", "Export CA key", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            DialogResult = DialogResult.Cancel;
        }

        private void butBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PKCS#12|*.p12";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = ".p12";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbLocation.Text = saveFileDialog1.FileName;
            }
        }
    }
}
