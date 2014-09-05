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
    public partial class PolicyMappings : Form
    {
        internal policyMappings mapping;
        internal certificatePolicies certpol;

        public PolicyMappings(policyMappings Mappings, certificatePolicies CertPol)
        {
            InitializeComponent();

            certpol = CertPol;
            foreach (CertPolicy policy in certpol.CertPolicies)
            {
                lbIssuerPolicies.Items.Add(policy.Oid + " (" + policy.Name + ")");
            }
            lbIssuerPolicies.SelectedIndex = 0;

            if (Mappings == null)
                create();
            else
                edit(Mappings);
        }

        // Create a new extension
        private void create()
        {
            mapping = new policyMappings();
        }

        // Load an existing extension
        private void edit(policyMappings Mappings)
        {
            mapping = Mappings;

            if (mapping.Critical)
                cbCritical.Checked = true;
            else
                cbCritical.Checked = false;

            foreach (var map in mapping.Mappings)
            {
                lbMappings.Items.Add(map.issuerOid + " (" + map.issuerPolicyName + ") -> " + map.subjectOid + " (" + map.subjectPolicyName + ")");
            }
        }

        private void butMap_Click(object sender, EventArgs e)
        {
            // Create a mapping
            PolicyMapping map = new PolicyMapping();
            string[] s = lbIssuerPolicies.SelectedItem.ToString().Split('(', ')');
            map.issuerOid = s[0];
            map.issuerPolicyName = s[1];

            // get subject oid
            GetPolicyShort subjectPolicy = new GetPolicyShort();
            if (subjectPolicy.ShowDialog() == DialogResult.OK)
            {
                map.subjectOid = subjectPolicy.oid;
                map.subjectPolicyName = subjectPolicy.name;
                mapping.Mappings.Add(map);

                lbMappings.Items.Add(map.issuerOid + " (" + map.issuerPolicyName + ") -> " + map.subjectOid + " (" + map.subjectPolicyName + ")");
            }
        }

        private void butUnmap_Click(object sender, EventArgs e)
        {
            if (lbMappings.SelectedItem == null)
                return;
            lbMappings.Items.RemoveAt(lbMappings.SelectedIndex);
            // Assume that index in the listbox is same as array
            mapping.Mappings.RemoveAt(lbMappings.SelectedIndex);
        }

        private void mappings_DoubleClick(object sender, EventArgs e)
        {
            GetPolicyShort subjectPolicy = new GetPolicyShort()
                {
                    oid = mapping.Mappings[lbMappings.SelectedIndex].subjectOid,
                    name = mapping.Mappings[lbMappings.SelectedIndex].subjectPolicyName
                };

            if (subjectPolicy.ShowDialog() == DialogResult.OK)
            {
                PolicyMapping map = new PolicyMapping()
                    {
                        issuerOid = mapping.Mappings[lbMappings.SelectedIndex].issuerOid,
                        issuerPolicyName = mapping.Mappings[lbMappings.SelectedIndex].issuerPolicyName
                    };

                map.subjectOid = subjectPolicy.oid;
                map.subjectPolicyName = subjectPolicy.name;
                mapping.Mappings.RemoveAt(lbMappings.SelectedIndex);
                mapping.Mappings.Add(map);
                lbMappings.Items.RemoveAt(lbMappings.SelectedIndex);
                lbMappings.Items.Add(map.issuerOid + " (" + map.issuerPolicyName + ") -> " + map.subjectOid + " (" + map.subjectPolicyName + ")");
            }
        }

        private void cbCritical_CheckedChanged(object sender, EventArgs e)
        {
            mapping.Critical = cbCritical.Checked;
        }

    }
}
