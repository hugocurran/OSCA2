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
    public partial class SubjectInfoAccess : Form
    {
        internal subjectInfoAccess sia;

        private DataRow dr;
        private DataSet ds = new DataSet();
        private string[] permittedTypes = { "Uniform Resource Identifier", "Directory Name", "RFC822 Name" };

        public SubjectInfoAccess(subjectInfoAccess InfoAccess)
        {
            InitializeComponent();

            // Setup the dataset
            ds.Tables.Add("caRepository");
            ds.Tables["caRepository"].Columns.Add("#");
            ds.Tables["caRepository"].Columns.Add("Type");
            ds.Tables["caRepository"].Columns.Add("Name");

            ds.Tables.Add("timeStamping");
            ds.Tables["timeStamping"].Columns.Add("#");
            ds.Tables["timeStamping"].Columns.Add("Type");
            ds.Tables["timeStamping"].Columns.Add("Name");

            // Setup the grid
            dgvCaRepository.Refresh();
            dgvCaRepository.DataSource = ds.Tables["caRepository"];
            //dgvCaIssuers.Columns[0].Width = 30;
            dgvTimeStamping.DataSource = ds.Tables["timeStamping"];
            //dgvOcsp.Columns[0].Width = 30;

            // Setup the buttons 'n stuff
            butAddCaRepository.Enabled = false;
            butAddTimeStamping.Enabled = false;
            butRemoveCaRepository.Enabled = false;
            butRemoveTimeStamping.Enabled = false;

            if (InfoAccess == null)
                create();
            else
                edit(InfoAccess);
        }

        // Create a new extension
        private void create()
        {
            sia = new subjectInfoAccess();
        }

        // Load an existing extension
        private void edit(subjectInfoAccess infoAccess)
        {
            sia = infoAccess;

            // Populate the dataset
            for (int i = 0; i < sia.SubjectInfoAccess.Count; i++)
                updateDataSet(sia.SubjectInfoAccess[i].Method, sia.SubjectInfoAccess[i].Location, i);

            if (ds.Tables["caRepository"].Rows.Count > 0)
                cbCaRepository.Checked = true;
            if (ds.Tables["timeStamping"].Rows.Count > 0)
                cbTimeStamping.Checked = true;
           
            // critical setting
            if (sia.Critical)
                cbCritical.Checked = true;
            else
                cbCritical.Checked = false;
        }

        private void updateDataSet(AccessMethod method, OSCAGeneralName gn, int index)
        {
            switch (method)
            {
                case AccessMethod.CARepository:
                    dr = ds.Tables["caRepository"].NewRow();
                    dr["#"] = (index + 1).ToString();
                    dr["Type"] = gn.Type.ToString();
                    dr["Name"] = gn.Name;
                    ds.Tables["caRepository"].Rows.Add(dr);
                    break;
                case AccessMethod.TimeStamping:
                    dr = ds.Tables["timeStamping"].NewRow();
                    dr["#"] = (index + 1).ToString();
                    dr["Type"] = gn.Type.ToString();
                    dr["Name"] = gn.Name;
                    ds.Tables["timeStamping"].Rows.Add(dr);
                    break;
            } 
        }

        private void cbCaRepository_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked){            
                butAddCaRepository.Enabled = true;
                butRemoveCaRepository.Enabled = true;
             }
            else
            {
                butAddCaRepository.Enabled = false;
                butRemoveCaRepository.Enabled = false;
                foreach (AccessDesc am in sia.SubjectInfoAccess)
                    if (am.Method == AccessMethod.CARepository)
                        sia.Remove(am);
            }
        }

        private void cbTimeStamping_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                butAddTimeStamping.Enabled = true;
                butRemoveTimeStamping.Enabled = true;
            }
            else
            {
                butAddTimeStamping.Enabled = false;
                butRemoveTimeStamping.Enabled = false;
                foreach (AccessDesc am in sia.SubjectInfoAccess)
                    if (am.Method == AccessMethod.TimeStamping)
                        sia.Remove(am);
            }
        }

        private void butAddCaRepository_Click(object sender, EventArgs e)
        {
            GetName getname = new GetName(permittedTypes);
            if (getname.ShowDialog() == DialogResult.OK)
            {
                AccessDesc ad = new AccessDesc() { Method = AccessMethod.CARepository };
                ad.Location = getname.gn;                
                sia.Add(ad);
                updateDataSet(AccessMethod.CARepository, getname.gn, sia.SubjectInfoAccess.Count - 1);
            }
        }

        private void butRemoveCaRepository_Click(object sender, EventArgs e)
        {
            // Easiest to reload the dataset, rather than fiddling around removing
            int index = Convert.ToInt32((string)dgvCaRepository.SelectedRows[0].Cells[0].Value);
            sia.Remove(sia.SubjectInfoAccess[index - 1]);

            ds.Tables["caRepository"].Clear();
            for (int i = 0; i < sia.SubjectInfoAccess.Count; i++)
            {
                if (sia.SubjectInfoAccess[i].Method == AccessMethod.CARepository)
                    updateDataSet(AccessMethod.CARepository, sia.SubjectInfoAccess[i].Location, i);
            }
        }

        private void butAddTimeStamping_Click(object sender, EventArgs e)
        {
            GetName getname = new GetName(permittedTypes);
            if (getname.ShowDialog() == DialogResult.OK)
            {
                AccessDesc ad = new AccessDesc() { Method = AccessMethod.TimeStamping };
                ad.Location = getname.gn;
                sia.Add(ad);
                updateDataSet(AccessMethod.TimeStamping, getname.gn, sia.SubjectInfoAccess.Count - 1);
            }
        }

        private void butRemoveTimeStamping_Click(object sender, EventArgs e)
        {
            // Easiest to reload the dataset, rather than fiddling around removing
            int index = Convert.ToInt32((string)dgvTimeStamping.SelectedRows[0].Cells[0].Value);
            sia.Remove(sia.SubjectInfoAccess[index - 1]);

            ds.Tables["timeStamping"].Clear();
            // Reload the dataset
            for (int i = 0; i < sia.SubjectInfoAccess.Count; i++)
            {
                if (sia.SubjectInfoAccess[i].Method == AccessMethod.TimeStamping)
                    updateDataSet(AccessMethod.TimeStamping, sia.SubjectInfoAccess[i].Location, i);
            }
        }

        private void dgvCaRepository_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Seems to be a bug in the .Net dgv code that throws an exception in some circumstances
            try
            {
                dgvCaRepository.Columns[0].Width = 30;
            }
            catch (NullReferenceException) { }
        }

        private void dgvTimeStamping_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Seems to be a bug in the .Net dgv code that throws an exception in some circumstances
            try
            {
                dgvTimeStamping.Columns[0].Width = 30;
            }
            catch (NullReferenceException) { }
        }

        private void cbCritical_CheckedChanged(object sender, EventArgs e)
        {
            sia.Critical = cbCritical.Checked;
        }
    }
}
