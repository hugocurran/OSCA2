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
using OSCA;

namespace OSCASnapin.Profiles
{
    public partial class AuthorityInfoAccess : Form
    {

        internal authorityInfoAccess aia;

        private DataRow dr;
        private DataSet ds = new DataSet();
        private string[] permittedTypes = { "Uniform Resource Identifier", "Directory Name", "RFC822 Name" };

        public AuthorityInfoAccess(authorityInfoAccess InfoAccess)
        {
            InitializeComponent();

            // Setup the dataset
            ds.Tables.Add("caIssuers");
            ds.Tables["caIssuers"].Columns.Add("#");
            ds.Tables["caIssuers"].Columns.Add("Type");
            ds.Tables["caIssuers"].Columns.Add("Name");

            ds.Tables.Add("ocsp");
            ds.Tables["ocsp"].Columns.Add("#");
            ds.Tables["ocsp"].Columns.Add("Type");
            ds.Tables["ocsp"].Columns.Add("Name");

            // Setup the grid
            dgvCaIssuers.Refresh();
            dgvCaIssuers.DataSource = ds.Tables["caIssuers"];
            //dgvCaIssuers.Columns[0].Width = 30;
            dgvOcsp.DataSource = ds.Tables["ocsp"];
            //dgvOcsp.Columns[0].Width = 30;

            // Setup the buttons 'n stuff
            butAddCaIssuers.Enabled = false;
            butAddOcsp.Enabled = false;
            butRemoveCaIssuers.Enabled = false;
            butRemoveOcsp.Enabled = false;

            if (InfoAccess == null)
                create();
            else
                edit(InfoAccess);
        }

        // Create a new extension
        private void create()
        {
            aia = new authorityInfoAccess();
        }

        // Load an existing extension
        private void edit(authorityInfoAccess infoAccess)
        {
            aia = infoAccess;

            // Populate the dataset
            for (int i = 0; i < aia.AuthInfoAccess.Count; i++)
                updateDataSet(aia.AuthInfoAccess[i].Method, aia.AuthInfoAccess[i].Location, i);

            if (ds.Tables["caIssuers"].Rows.Count > 0)
                cbCaIssuers.Checked = true;
            if (ds.Tables["ocsp"].Rows.Count > 0)
                cbOcsp.Checked = true;
           
            // critical setting
            if (aia.Critical)
                cbCritical.Checked = true;
            else
                cbCritical.Checked = false;
        }

        private void updateDataSet(AccessMethod method, OSCAGeneralName gn, int index)
        {
            switch (method)
            {
                case AccessMethod.CAIssuers:
                    dr = ds.Tables["caIssuers"].NewRow();
                    dr["#"] = (index + 1).ToString();
                    dr["Type"] = gn.Type.ToString();
                    dr["Name"] = gn.Name;
                    ds.Tables["caIssuers"].Rows.Add(dr);
                    break;
                case AccessMethod.Ocsp:
                    dr = ds.Tables["ocsp"].NewRow();
                    dr["#"] = (index + 1).ToString();
                    dr["Type"] = gn.Type.ToString();
                    dr["Name"] = gn.Name;
                    ds.Tables["ocsp"].Rows.Add(dr);
                    break;
            } 
        }

        private void cbCaIssuers_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked){            
                butAddCaIssuers.Enabled = true;
                butRemoveCaIssuers.Enabled = true;
             }
            else
            {
                butAddCaIssuers.Enabled = false;
                butRemoveCaIssuers.Enabled = false;
                foreach (AccessDesc am in aia.AuthInfoAccess)
                    if (am.Method == AccessMethod.CAIssuers)
                        aia.Remove(am);
            }
        }

        private void cbOcsp_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                butAddOcsp.Enabled = true;
                butRemoveOcsp.Enabled = true;
            }
            else
            {
                butAddOcsp.Enabled = false;
                butRemoveOcsp.Enabled = false;
                foreach (AccessDesc am in aia.AuthInfoAccess)
                    if (am.Method == AccessMethod.Ocsp)
                        aia.Remove(am);
            }
        }

        private void butAddCaIssuers_Click(object sender, EventArgs e)
        {
            GetName getname = new GetName(permittedTypes);
            if (getname.ShowDialog() == DialogResult.OK)
            {
                AccessDesc ad = new AccessDesc() { Method = AccessMethod.CAIssuers };
                ad.Location = getname.gn;                
                aia.Add(ad);
                updateDataSet(AccessMethod.CAIssuers, getname.gn, aia.AuthInfoAccess.Count - 1);
            }
        }

        private void butRemoveCaIssuers_Click(object sender, EventArgs e)
        {
            // Easiest to reload the dataset, rather than fiddling around removing
            int index = Convert.ToInt32((string)dgvCaIssuers.SelectedRows[0].Cells[0].Value);
            aia.Remove(aia.AuthInfoAccess[index - 1]);

            ds.Tables["caIssuers"].Clear();
            for (int i = 0; i < aia.AuthInfoAccess.Count; i++)
            {
                if (aia.AuthInfoAccess[i].Method == AccessMethod.CAIssuers)
                    updateDataSet(AccessMethod.CAIssuers, aia.AuthInfoAccess[i].Location, i);
            }
        }

        private void butAddOcsp_Click(object sender, EventArgs e)
        {
            GetName getname = new GetName(permittedTypes);
            if (getname.ShowDialog() == DialogResult.OK)
            {
                AccessDesc ad = new AccessDesc() { Method = AccessMethod.Ocsp };
                ad.Location = getname.gn;
                aia.Add(ad);
                updateDataSet(AccessMethod.Ocsp, getname.gn, aia.AuthInfoAccess.Count - 1);
            }
        }

        private void butRemoveOcsp_Click(object sender, EventArgs e)
        {
            // Easiest to reload the dataset, rather than fiddling around removing
            int index = Convert.ToInt32((string)dgvOcsp.SelectedRows[0].Cells[0].Value);
            aia.Remove(aia.AuthInfoAccess[index - 1]);

            ds.Tables["ocsp"].Clear();
            // Reload the dataset
            for (int i = 0; i < aia.AuthInfoAccess.Count; i++)
            {
                if (aia.AuthInfoAccess[i].Method == AccessMethod.Ocsp)
                    updateDataSet(AccessMethod.Ocsp, aia.AuthInfoAccess[i].Location, i);
            }
        }

        private void dgvCaIssuers_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Seems to be a bug in the .Net dgv code that throws an exception in some circumstances
            try
            {
                dgvCaIssuers.Columns[0].Width = 30;
            }
            catch (NullReferenceException) { }
        }

        private void dgvOcsp_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Seems to be a bug in the .Net dgv code that throws an exception in some circumstances
            try
            {
                dgvOcsp.Columns[0].Width = 30;
            }
            catch (NullReferenceException) { }
        }

        private void cbCritical_CheckedChanged(object sender, EventArgs e)
        {
            aia.Critical = cbCritical.Checked;
        }
    }
}
