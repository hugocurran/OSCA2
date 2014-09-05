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
using System.Windows.Forms;
using OSCA.Profile;

namespace OSCASnapin.Profiles
{
    public partial class BasicConstraints : Form
    {
        // BasicConstraints extension instance
        internal basicConstraints basCon;

        // Temp disable events while setting up to edit
        private bool enableEvents;

        public BasicConstraints(basicConstraints bc)
        {
            InitializeComponent();

            if (bc == null)
                create();
            else
                edit(bc);
        }
     
        // Create a new extension
        internal void create()
        {
            basCon = new basicConstraints();
            enableEvents = true;
        }

        // Load an existing extension
        internal void edit(basicConstraints bc)
        {
            basCon = bc;            
            enableEvents = false;

            // Setup the checkboxes
            if (bc.IsCA)
                cbCA.Checked = true;
            else
                cbCA.Checked = false;

            tbPathLen.Text = bc.PathLength;

            if (basCon.Critical)
                cbCritical.Checked = true;
            else
                cbCritical.Checked = false;

            enableEvents = true;                     
        }
         
        private void ca_Changed(object sender, EventArgs e)
        {
            if (enableEvents)
            {
                if (cbCA.Checked)
                {
                    basCon.IsCA = true;
                    tbPathLen.Enabled = true;
                    cbCritical.Checked = true;
                }
                else
                {
                    basCon.IsCA = false;
                    tbPathLen.Text = "none";
                    tbPathLen.Enabled = false;
                    cbCritical.Checked = false;
                }
            }
        }

        private void cbCritical_CheckedChanged(object sender, EventArgs e)
        {
            basCon.Critical = cbCritical.Checked;
        }

        private void butCreate_Click(object sender, EventArgs e)
        {
            ca_Changed(sender, e);
            cbCritical_CheckedChanged(sender, e);
            basCon.PathLength = tbPathLen.Text;
        }
    }
}
