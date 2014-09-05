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
    public partial class AdminPassword : Form
    {
        private Configuration mgrConfig = Configuration.Instance;

        public AdminPassword()
        {
            InitializeComponent();
            AcceptButton = butOK;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            // Check Password validity
            if (!Password.ValidatePassword(mgrConfig.InitData.adminPasswordHash, tbPassword.Text))
            {
                MessageBox.Show("Invalid Password", "Admin Mode", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
