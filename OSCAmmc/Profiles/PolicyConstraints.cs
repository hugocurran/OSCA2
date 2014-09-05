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
    public partial class PolicyConstraints : Form
    {
       // InhibitAnyPolicy extension instance
        internal policyConstraints polCon;

        public PolicyConstraints(policyConstraints pc)
        {
            InitializeComponent();

            if (pc == null)
                create();
            else
                edit(pc);
        }

        // Create a new extension
        internal void create()
        {
            polCon = new policyConstraints();
        }

        // Load an existing extension
        internal void edit(policyConstraints pc)
        {
            polCon = pc;

            if (pc.Critical)
                cbCritical.Checked = true;
            else
                cbCritical.Checked = false;

            if (polCon.InhibitPolicyMapping >= 0)
            {
                cbInhibPolMap.Checked = true;
                tbInPolMapSkip.Text = polCon.InhibitPolicyMapping.ToString();
            }
            else
            {
                cbInhibPolMap.Checked = false;
                tbInPolMapSkip.Text = "0";
            }

            if (polCon.RequireExplicitPolicy >= 0)
            {
                cbReqExpPol.Checked = true;
                tbReqExpPolSkip.Text = polCon.RequireExplicitPolicy.ToString();
            }
            else
            {
                cbReqExpPol.Checked = false;
                tbReqExpPolSkip.Text = "0";
            }
        }

        private void cbCritical_CheckedChanged(object sender, EventArgs e)
        {
            polCon.Critical = cbCritical.Checked;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (cbInhibPolMap.Checked)
                polCon.InhibitPolicyMapping = Convert.ToInt32(tbInPolMapSkip.Text);
            else
                polCon.InhibitPolicyMapping = -1;

            if (cbReqExpPol.Checked)
                polCon.RequireExplicitPolicy = Convert.ToInt32(tbReqExpPolSkip.Text);
            else
                polCon.RequireExplicitPolicy = -1;
        }
    }
}
