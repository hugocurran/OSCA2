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
using OSCA;

namespace OSCASnapin.Profiles
{
    public partial class GetName : Form
    {
        public OSCAGeneralName gn;

        public GetName(string[] permittedTypes)
        {
            InitializeComponent();

            // Setup the listbox with the permitted types
            foreach (string permitted in permittedTypes)
            {
                lbType.Items.Add(permitted);
            }                       
            lbType.SelectedIndex = 0;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            gn = new OSCAGeneralName() { Name = tbName.Text };
            switch ((string)lbType.SelectedItem)
            {
                case "Other Name":
                    gn.Type = GenName.otherName;
                    break;
                case "RFC822 Name":
                    gn.Type = GenName.rfc822Name;
                    break;
                case "DNS Name":
                    gn.Type = GenName.dNSName;
                    break;
                case "X400 Address":
                    gn.Type = GenName.x400Address;
                    break;
                case "Directory Name":
                    gn.Type = GenName.directoryName;
                    break;
                case "EDI Party Name":
                    gn.Type = GenName.ediPartyName;
                    break;
                case "Uniform Resource Identifier":
                    gn.Type = GenName.uniformResourceIdentifier;
                    break;
                case "IP Address":
                    gn.Type = GenName.iPAddress;
                    break;
                case "Registered ID":
                    gn.Type = GenName.registeredID;
                    break;
            }
        }
    }
}
