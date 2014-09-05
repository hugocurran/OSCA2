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
using System.Data;
using System.Windows.Forms;
using OSCA.Profile;
//using OSCA.Crypto;

namespace OSCASnapin.Profiles
{
    public partial class CertificatePolicies : Form
    {
        // extension instance
        internal certificatePolicies cp;

        private DataRow dr;
        private DataSet ds = new DataSet();

        public CertificatePolicies(certificatePolicies certPol)
        {
            InitializeComponent();

            // Setup the dataset
            ds.Tables.Add("policies");
            ds.Tables["policies"].Columns.Add("#");
            ds.Tables["policies"].Columns.Add("OID");
            ds.Tables["policies"].Columns.Add("Name");
            ds.Tables["policies"].Columns.Add("CPS");
            ds.Tables["policies"].Columns.Add("Notice");

            // Setup the grid            
            dgv.DataSource = ds.Tables["policies"];

            if (certPol == null)
                create();
            else
                edit(certPol);
        }

        // Create a new extension
        private void create()
        {
            cp = new certificatePolicies();
        }

        // Load an existing extension
        private void edit(certificatePolicies certPol)
        {
            cp = certPol;

            // Populate the dataset
            for (int i = 0; i < cp.CertPolicies.Count; i++)
                updateDataSet(cp.CertPolicies[i], i);
           
            // critical setting
            if (cp.Critical)
                cbCritical.Checked = true;
            else
                cbCritical.Checked = false;
        }

        private void updateDataSet(CertPolicy pol, int index)
        {
            dr = ds.Tables["policies"].NewRow();
            dr["#"] = (index + 1).ToString();
            dr["OID"] = pol.Oid;
            dr["Name"] = pol.Name;
            dr["CPS"] = pol.Cps;
            dr["Notice"] = pol.Unotice;
            ds.Tables["policies"].Rows.Add(dr);

            // Seems to be a bug in the .Net dgv code that throws an exception in some circumstances
            try
            {
                dgv.Columns[0].Width = 20;
            }
            catch (NullReferenceException) { }
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            GetPolicy getpolicy = new GetPolicy();
            if (getpolicy.ShowDialog() == DialogResult.OK)
            {
                cp.Add(getpolicy.pol);
                updateDataSet(getpolicy.pol, cp.CertPolicies.Count -1);
            }
        }

        private void butRemove_Click(object sender, EventArgs e)
        {
            // Easiest to reload the dataset, rather than fiddling around removing
            int index = Convert.ToInt32((string)dgv.SelectedRows[0].Cells[0].Value);
            cp.Remove(cp.CertPolicies[index - 1]);

            ds.Tables["policies"].Clear();
            // Reload the dataset
            for (int i = 0; i < cp.CertPolicies.Count; i++)
                updateDataSet(cp.CertPolicies[i], i);
        }

        private void cbCritical_CheckedChanged(object sender, EventArgs e)
        {
            cp.Critical = cbCritical.Checked;
        }
    }
}
