using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSCA.Profile;

namespace OSCASnapin.Profiles
{
    public partial class InhibitAnyPolicy : Form
    {
        // InhibitAnyPolicy extension instance
        internal inhibitAnyPolicy iAnypol;

        public InhibitAnyPolicy(inhibitAnyPolicy iap)
        {
            InitializeComponent();

            if (iap == null)
                create();
            else
                edit(iap);
        }

        // Create a new extension
        internal void create()
        {
            iAnypol = new inhibitAnyPolicy();
        }

        // Load an existing extension
        internal void edit(inhibitAnyPolicy iap)
        {
            iAnypol = iap;

            if (iap.Critical)
                cbCritical.Checked = true;
            else
                cbCritical.Checked = false;

            tbSkipCerts.Text = iAnypol.Skip.ToString();
        }

        private void cbCritical_CheckedChanged(object sender, EventArgs e)
        {
            iAnypol.Critical = cbCritical.Checked;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            iAnypol.Skip = Convert.ToInt32(tbSkipCerts.Text);
        }
    }
}
