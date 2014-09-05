namespace OSCASnapin.Profiles
{
    partial class AuthorityInfoAccess
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthorityInfoAccess));
            this.dgvOcsp = new System.Windows.Forms.DataGridView();
            this.butAddCaIssuers = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.cbCritical = new System.Windows.Forms.CheckBox();
            this.butRemoveCaIssuers = new System.Windows.Forms.Button();
            this.dgvCaIssuers = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbCaIssuers = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbOcsp = new System.Windows.Forms.CheckBox();
            this.butRemoveOcsp = new System.Windows.Forms.Button();
            this.butAddOcsp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOcsp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCaIssuers)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvOcsp
            // 
            this.dgvOcsp.AllowUserToAddRows = false;
            this.dgvOcsp.AllowUserToDeleteRows = false;
            this.dgvOcsp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOcsp.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOcsp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOcsp.Location = new System.Drawing.Point(0, 65);
            this.dgvOcsp.MultiSelect = false;
            this.dgvOcsp.Name = "dgvOcsp";
            this.dgvOcsp.ReadOnly = true;
            this.dgvOcsp.RowHeadersVisible = false;
            this.dgvOcsp.RowTemplate.Height = 24;
            this.dgvOcsp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOcsp.ShowEditingIcon = false;
            this.dgvOcsp.Size = new System.Drawing.Size(471, 78);
            this.dgvOcsp.TabIndex = 0;
            this.dgvOcsp.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvOcsp_RowsAdded);
            // 
            // butAddCaIssuers
            // 
            this.butAddCaIssuers.Location = new System.Drawing.Point(231, 23);
            this.butAddCaIssuers.Name = "butAddCaIssuers";
            this.butAddCaIssuers.Size = new System.Drawing.Size(82, 31);
            this.butAddCaIssuers.TabIndex = 1;
            this.butAddCaIssuers.Text = "Add";
            this.butAddCaIssuers.UseVisualStyleBackColor = true;
            this.butAddCaIssuers.Click += new System.EventHandler(this.butAddCaIssuers_Click);
            // 
            // butOK
            // 
            this.butOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOK.Location = new System.Drawing.Point(132, 366);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(82, 31);
            this.butOK.TabIndex = 2;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(298, 366);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(82, 31);
            this.butCancel.TabIndex = 3;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // cbCritical
            // 
            this.cbCritical.AutoSize = true;
            this.cbCritical.Location = new System.Drawing.Point(228, 332);
            this.cbCritical.Name = "cbCritical";
            this.cbCritical.Size = new System.Drawing.Size(75, 22);
            this.cbCritical.TabIndex = 4;
            this.cbCritical.Text = "Critical";
            this.cbCritical.UseVisualStyleBackColor = true;
            this.cbCritical.CheckedChanged += new System.EventHandler(this.cbCritical_CheckedChanged);
            // 
            // butRemoveCaIssuers
            // 
            this.butRemoveCaIssuers.Location = new System.Drawing.Point(347, 23);
            this.butRemoveCaIssuers.Name = "butRemoveCaIssuers";
            this.butRemoveCaIssuers.Size = new System.Drawing.Size(82, 31);
            this.butRemoveCaIssuers.TabIndex = 5;
            this.butRemoveCaIssuers.Text = "Remove";
            this.butRemoveCaIssuers.UseVisualStyleBackColor = true;
            this.butRemoveCaIssuers.Click += new System.EventHandler(this.butRemoveCaIssuers_Click);
            // 
            // dgvCaIssuers
            // 
            this.dgvCaIssuers.AllowUserToAddRows = false;
            this.dgvCaIssuers.AllowUserToDeleteRows = false;
            this.dgvCaIssuers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCaIssuers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCaIssuers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCaIssuers.Location = new System.Drawing.Point(0, 67);
            this.dgvCaIssuers.MultiSelect = false;
            this.dgvCaIssuers.Name = "dgvCaIssuers";
            this.dgvCaIssuers.ReadOnly = true;
            this.dgvCaIssuers.RowHeadersVisible = false;
            this.dgvCaIssuers.RowTemplate.Height = 24;
            this.dgvCaIssuers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCaIssuers.ShowEditingIcon = false;
            this.dgvCaIssuers.Size = new System.Drawing.Size(471, 76);
            this.dgvCaIssuers.TabIndex = 6;
            this.dgvCaIssuers.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvCaIssuers_RowsAdded);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbCaIssuers);
            this.groupBox1.Controls.Add(this.dgvCaIssuers);
            this.groupBox1.Controls.Add(this.butRemoveCaIssuers);
            this.groupBox1.Controls.Add(this.butAddCaIssuers);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(471, 149);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CA Issuers";
            // 
            // cbCaIssuers
            // 
            this.cbCaIssuers.AutoSize = true;
            this.cbCaIssuers.Location = new System.Drawing.Point(11, 32);
            this.cbCaIssuers.Name = "cbCaIssuers";
            this.cbCaIssuers.Size = new System.Drawing.Size(97, 21);
            this.cbCaIssuers.TabIndex = 7;
            this.cbCaIssuers.Text = "CA Issuers";
            this.cbCaIssuers.UseVisualStyleBackColor = true;
            this.cbCaIssuers.CheckedChanged += new System.EventHandler(this.cbCaIssuers_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbOcsp);
            this.groupBox2.Controls.Add(this.butRemoveOcsp);
            this.groupBox2.Controls.Add(this.butAddOcsp);
            this.groupBox2.Controls.Add(this.dgvOcsp);
            this.groupBox2.Location = new System.Drawing.Point(12, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(471, 139);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "OCSP";
            // 
            // cbOcsp
            // 
            this.cbOcsp.AutoSize = true;
            this.cbOcsp.Location = new System.Drawing.Point(11, 32);
            this.cbOcsp.Name = "cbOcsp";
            this.cbOcsp.Size = new System.Drawing.Size(68, 21);
            this.cbOcsp.TabIndex = 3;
            this.cbOcsp.Text = "OCSP";
            this.cbOcsp.UseVisualStyleBackColor = true;
            this.cbOcsp.CheckedChanged += new System.EventHandler(this.cbOcsp_CheckedChanged);
            // 
            // butRemoveOcsp
            // 
            this.butRemoveOcsp.Location = new System.Drawing.Point(347, 23);
            this.butRemoveOcsp.Name = "butRemoveOcsp";
            this.butRemoveOcsp.Size = new System.Drawing.Size(82, 31);
            this.butRemoveOcsp.TabIndex = 2;
            this.butRemoveOcsp.Text = "Remove";
            this.butRemoveOcsp.UseVisualStyleBackColor = true;
            this.butRemoveOcsp.Click += new System.EventHandler(this.butRemoveOcsp_Click);
            // 
            // butAddOcsp
            // 
            this.butAddOcsp.Location = new System.Drawing.Point(231, 23);
            this.butAddOcsp.Name = "butAddOcsp";
            this.butAddOcsp.Size = new System.Drawing.Size(82, 31);
            this.butAddOcsp.TabIndex = 1;
            this.butAddOcsp.Text = "Add";
            this.butAddOcsp.UseVisualStyleBackColor = true;
            this.butAddOcsp.Click += new System.EventHandler(this.butAddOcsp_Click);
            // 
            // AuthorityInfoAccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 409);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbCritical);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuthorityInfoAccess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authority Info Access";
            ((System.ComponentModel.ISupportInitialize)(this.dgvOcsp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCaIssuers)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOcsp;
        private System.Windows.Forms.Button butAddCaIssuers;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox cbCritical;
        private System.Windows.Forms.Button butRemoveCaIssuers;
        private System.Windows.Forms.DataGridView dgvCaIssuers;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbCaIssuers;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbOcsp;
        private System.Windows.Forms.Button butRemoveOcsp;
        private System.Windows.Forms.Button butAddOcsp;
    }
}