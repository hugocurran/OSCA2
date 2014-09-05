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
    public partial class CrlDistributionPoint : Form
    {
        // KeyUsage extension instance
        internal crlDistPoint crlDP;

        private DataRow dr;
        private DataSet ds = new DataSet();
        private string[] permittedTypes = { "Uniform Resource Identifier", "Directory Name" };

        public CrlDistributionPoint(crlDistPoint cdp)
        {
            InitializeComponent();

            // Setup the dataset
            ds.Tables.Add("generalNames");
            ds.Tables["generalNames"].Columns.Add("DP");
            ds.Tables["generalNames"].Columns.Add("Type");
            ds.Tables["generalNames"].Columns.Add("Name");

            // Setup the grid            
            dgv.DataSource = ds.Tables["generalNames"];

            if (cdp == null)
                create();
            else
                edit(cdp);
        }

        // Create a new extension
        private void create()
        {
            crlDP = new crlDistPoint();
        }

        // Load an existing extension
        private void edit(crlDistPoint cdp)
        {
            crlDP = cdp;

            // Populate the dataset
            for (int i = 0; i < cdp.DistPoints.Count; i++)
                updateDataSet(cdp.DistPoints[i], i);
           
            // critical setting
            if (crlDP.Critical)
                cbCritical.Checked = true;
            else
                cbCritical.Checked = false;
        }

        private void updateDataSet(OSCAGeneralName gn, int index)
        {
            dr = ds.Tables["generalNames"].NewRow();
            dr["DP"] = (index + 1).ToString();
            dr["Type"] = gn.Type.ToString();
            dr["Name"] = gn.Name;
            ds.Tables["generalNames"].Rows.Add(dr);

            // Seems to be a bug in the .Net dgv code that throws an exception in some circumstances
            try
            {
                dgv.Columns[0].Width = 40;
            }
            catch (NullReferenceException) { }
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            GetName getname = new GetName(permittedTypes);
            if (getname.ShowDialog() == DialogResult.OK)
            {
                crlDP.Add(getname.gn);
                updateDataSet(getname.gn, crlDP.DistPoints.Count -1);
            }
        }

        private void butRemove_Click(object sender, EventArgs e)
        {
            // Easiest to reload the dataset, rather than fiddling around removing
            int index = Convert.ToInt32((string)dgv.SelectedRows[0].Cells[0].Value);
            crlDP.Remove(crlDP.DistPoints[index - 1]);

            ds.Tables["generalNames"].Clear();
            // Reload the dataset
            for (int i = 0; i < crlDP.DistPoints.Count; i++)
                updateDataSet(crlDP.DistPoints[i], i);
        }

        private void cbCritical_CheckedChanged(object sender, EventArgs e)
        {
            crlDP.Critical = cbCritical.Checked;
        }
    }
}
