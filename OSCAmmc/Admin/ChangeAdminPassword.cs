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
    public partial class ChangeAdminPassword : Form
    {
        private Configuration mgrConfig = Configuration.Instance;

        public ChangeAdminPassword()
        {
            InitializeComponent();
            AcceptButton = butOK;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            // Check Password validity
            if (!Password.ValidatePassword(mgrConfig.InitData.adminPasswordHash, tbOldPassword.Text))
            {
                MessageBox.Show("Invalid Password", "Change Admin Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                DialogResult = DialogResult.Cancel;
                return;
            }

            if (string.Equals(tbNewPassword1.Text, tbNewPassword2.Text, StringComparison.Ordinal))
            {
                mgrConfig.InitData.adminPasswordHash = Password.hashPassword(tbNewPassword1.Text);
                DialogResult = DialogResult.OK;
                return;
            }
            
            MessageBox.Show("New Passwords do not match", "Change Admin Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            DialogResult = DialogResult.Cancel;
        }
    }
}