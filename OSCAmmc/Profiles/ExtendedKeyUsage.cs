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
    public partial class ExtendedKeyUsage : Form
    {
        // KeyUsage extension instance
        internal extendedKeyUsage extKeyUsage;

        // Temp disable events while setting up to edit
        private bool enableEvents;

        // Keep track of all the checkboxes
        private CheckBox[] cblist;

        public ExtendedKeyUsage(extendedKeyUsage eku)
        {
            InitializeComponent();
            cblist = new CheckBox[] {
                cbAnyExtendedKeyUsage,
                cbServerAuth,
                cbClientAuth,
                cbCodeSigning,
                cbEmailProtection,
                cbIpsecEndSystem,
                cbIpsecTunnel,
                cbIpsecUser,
                cbTimeStamping,
                cbOcspSigning,
                cbSmartCardLogon};

            if (eku == null)
                create();
            else
                edit(eku);
        }

        // Create a new extension
        internal void create()
        {
            extKeyUsage = new extendedKeyUsage();
            enableEvents = true;
        }

        // Load an existing extension
        internal void edit(extendedKeyUsage eku)
        {
            extKeyUsage = eku;            
            enableEvents = false;

            // Setup the checkboxes
            foreach (string _eku in extKeyUsage.ExtKUsage)
            {
                foreach (CheckBox cb in cblist)
                {
                    if ((string)cb.Tag == _eku)
                        cb.Checked = true;
                }
            }
            // critical setting
            if (extKeyUsage.Critical)
                cbCritical.Checked = true;
            else
                cbCritical.Checked = false;

            enableEvents = true;                     
        }
         
        private void cb_Changed(object sender, EventArgs e)
        {
            if (enableEvents)
            {
                if (((CheckBox)sender).Checked)
                    extKeyUsage.Add((string)((CheckBox)sender).Tag);
                else
                    extKeyUsage.Remove((string)((CheckBox)sender).Tag);
            }
        }

        private void cbCritical_CheckedChanged(object sender, EventArgs e)
        {
            extKeyUsage.Critical = cbCritical.Checked;
        }
    }
}
