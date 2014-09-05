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
    public partial class NameConstraints : Form
    {

        internal nameConstraints nc;

        private DataRow dr;
        private DataSet ds = new DataSet();
        private string[] permittedTypes = { "Other Name", "Uniform Resource Identifier", "Directory Name", "RFC822 Name", "DNS Name", "IP Address" };

        public NameConstraints(nameConstraints NameCon)
        {
            InitializeComponent();

            // Setup the dataset
            ds.Tables.Add("permitted");
            ds.Tables["permitted"].Columns.Add("#");
            ds.Tables["permitted"].Columns.Add("Type");
            ds.Tables["permitted"].Columns.Add("Name");

            ds.Tables.Add("excluded");
            ds.Tables["excluded"].Columns.Add("#");
            ds.Tables["excluded"].Columns.Add("Type");
            ds.Tables["excluded"].Columns.Add("Name");

            // Setup the grid
            dgvPermitted.Refresh();
            dgvPermitted.DataSource = ds.Tables["permitted"];
            //dgvCaIssuers.Columns[0].Width = 30;
            dgvExcluded.DataSource = ds.Tables["excluded"];
            //dgvOcsp.Columns[0].Width = 30;

            // Setup the buttons 'n stuff
            butAddPermitted.Enabled = false;
            butAddExcluded.Enabled = false;
            butRemovePermitted.Enabled = false;
            butRemoveExcluded.Enabled = false;

            if (NameCon == null)
                create();
            else
                edit(NameCon);
        }

        // Create a new extension
        private void create()
        {
            nc = new nameConstraints();
        }

        // Load an existing extension
        private void edit(nameConstraints nameCon)
        {
            nc = nameCon;

            // Populate the dataset
            for (int i = 0; i < nc.Permitted.Count; i++)
                updateDataSet("permitted", nc.Permitted[i], i);

            for (int i = 0; i < nc.Excluded.Count; i++)
                updateDataSet("excluded", nc.Permitted[i], i);

            if (ds.Tables["permitted"].Rows.Count > 0)
                cbPermitted.Checked = true;
            if (ds.Tables["excluded"].Rows.Count > 0)
                cbExcluded.Checked = true;
           
            // critical setting
            if (nc.Critical)
                cbCritical.Checked = true;
            else
                cbCritical.Checked = false;
        }

        private void updateDataSet(string table, OSCAGeneralName gn, int index)
        {
            dr = ds.Tables[table].NewRow();
            dr["#"] = (index + 1).ToString();
            dr["Type"] = gn.Type.ToString();
            dr["Name"] = gn.Name;
            ds.Tables[table].Rows.Add(dr);                   
        }

        private void cbPermitted_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked){            
                butAddPermitted.Enabled = true;
                butRemovePermitted.Enabled = true;
             }
            else
            {
                butAddPermitted.Enabled = false;
                butRemovePermitted.Enabled = false;
                foreach (OSCAGeneralName gn in nc.Permitted)
                    nc.Remove(NameConstraintTree.Permitted, gn);
            }
        }

        private void cbExcluded_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                butAddExcluded.Enabled = true;
                butRemoveExcluded.Enabled = true;
            }
            else
            {
                butAddExcluded.Enabled = false;
                butRemoveExcluded.Enabled = false;
                foreach (OSCAGeneralName gn in nc.Excluded)
                    nc.Remove(NameConstraintTree.Excluded, gn);
            }
        }

        private void butAddPermitted_Click(object sender, EventArgs e)
        {
            GetName getname = new GetName(permittedTypes);
            if (getname.ShowDialog() == DialogResult.OK)
            {           
                nc.Add(NameConstraintTree.Permitted, getname.gn);
                updateDataSet("permitted", getname.gn, nc.Permitted.Count - 1);
            }
        }

        private void butRemovePermited_Click(object sender, EventArgs e)
        {
            // Easiest to reload the dataset, rather than fiddling around removing
            int index = Convert.ToInt32((string)dgvPermitted.SelectedRows[0].Cells[0].Value);
            nc.Remove(NameConstraintTree.Permitted, nc.Permitted[index - 1]);

            ds.Tables["permitted"].Clear();
            for (int i = 0; i < nc.Permitted.Count; i++)
                 updateDataSet("permitted", nc.Permitted[i], i);
        }

        private void butAddExcluded_Click(object sender, EventArgs e)
        {
            GetName getname = new GetName(permittedTypes);
            if (getname.ShowDialog() == DialogResult.OK)
            {
                nc.Add(NameConstraintTree.Excluded, getname.gn);
                updateDataSet("excluded", getname.gn, nc.Excluded.Count - 1);
            }
        }

        private void butRemoveExcluded_Click(object sender, EventArgs e)
        {
            // Easiest to reload the dataset, rather than fiddling around removing
            int index = Convert.ToInt32((string)dgvExcluded.SelectedRows[0].Cells[0].Value);
            nc.Remove(NameConstraintTree.Excluded, nc.Excluded[index - 1]);

            ds.Tables["excluded"].Clear();
            // Reload the dataset
            for (int i = 0; i < nc.Excluded.Count; i++)
                updateDataSet("excluded", nc.Excluded[i], i);
        }

        private void dgvPermitted_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Seems to be a bug in the .Net dgv code that throws an exception in some circumstances
            try
            {
                dgvPermitted.Columns[0].Width = 30;
            }
            catch (NullReferenceException) { }
        }

        private void dgvExcluded_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Seems to be a bug in the .Net dgv code that throws an exception in some circumstances
            try
            {
                dgvExcluded.Columns[0].Width = 30;
            }
            catch (NullReferenceException) { }
        }

        private void cbCritical_CheckedChanged(object sender, EventArgs e)
        {
            nc.Critical = cbCritical.Checked;
        }
    }
}

