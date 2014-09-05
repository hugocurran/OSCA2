namespace OSCASnapin.Profiles
{
    partial class NameConstraints
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NameConstraints));
            this.dgvExcluded = new System.Windows.Forms.DataGridView();
            this.butAddPermitted = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.cbCritical = new System.Windows.Forms.CheckBox();
            this.butRemovePermitted = new System.Windows.Forms.Button();
            this.dgvPermitted = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbPermitted = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbExcluded = new System.Windows.Forms.CheckBox();
            this.butRemoveExcluded = new System.Windows.Forms.Button();
            this.butAddExcluded = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExcluded)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPermitted)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvExcluded
            // 
            this.dgvExcluded.AllowUserToAddRows = false;
            this.dgvExcluded.AllowUserToDeleteRows = false;
            this.dgvExcluded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvExcluded.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvExcluded.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExcluded.Location = new System.Drawing.Point(0, 65);
            this.dgvExcluded.MultiSelect = false;
            this.dgvExcluded.Name = "dgvExcluded";
            this.dgvExcluded.ReadOnly = true;
            this.dgvExcluded.RowHeadersVisible = false;
            this.dgvExcluded.RowTemplate.Height = 24;
            this.dgvExcluded.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvExcluded.ShowEditingIcon = false;
            this.dgvExcluded.Size = new System.Drawing.Size(471, 78);
            this.dgvExcluded.TabIndex = 0;
            this.dgvExcluded.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvExcluded_RowsAdded);
            // 
            // butAddPermitted
            // 
            this.butAddPermitted.Location = new System.Drawing.Point(231, 23);
            this.butAddPermitted.Name = "butAddPermitted";
            this.butAddPermitted.Size = new System.Drawing.Size(82, 31);
            this.butAddPermitted.TabIndex = 1;
            this.butAddPermitted.Text = "Add";
            this.butAddPermitted.UseVisualStyleBackColor = true;
            this.butAddPermitted.Click += new System.EventHandler(this.butAddPermitted_Click);
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
            // butRemovePermitted
            // 
            this.butRemovePermitted.Location = new System.Drawing.Point(347, 23);
            this.butRemovePermitted.Name = "butRemovePermitted";
            this.butRemovePermitted.Size = new System.Drawing.Size(82, 31);
            this.butRemovePermitted.TabIndex = 5;
            this.butRemovePermitted.Text = "Remove";
            this.butRemovePermitted.UseVisualStyleBackColor = true;
            this.butRemovePermitted.Click += new System.EventHandler(this.butRemovePermited_Click);
            // 
            // dgvPermitted
            // 
            this.dgvPermitted.AllowUserToAddRows = false;
            this.dgvPermitted.AllowUserToDeleteRows = false;
            this.dgvPermitted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPermitted.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPermitted.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPermitted.Location = new System.Drawing.Point(0, 67);
            this.dgvPermitted.MultiSelect = false;
            this.dgvPermitted.Name = "dgvPermitted";
            this.dgvPermitted.ReadOnly = true;
            this.dgvPermitted.RowHeadersVisible = false;
            this.dgvPermitted.RowTemplate.Height = 24;
            this.dgvPermitted.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPermitted.ShowEditingIcon = false;
            this.dgvPermitted.Size = new System.Drawing.Size(471, 76);
            this.dgvPermitted.TabIndex = 6;
            this.dgvPermitted.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvPermitted_RowsAdded);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbPermitted);
            this.groupBox1.Controls.Add(this.dgvPermitted);
            this.groupBox1.Controls.Add(this.butRemovePermitted);
            this.groupBox1.Controls.Add(this.butAddPermitted);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(471, 149);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Permitted names";
            // 
            // cbPermitted
            // 
            this.cbPermitted.AutoSize = true;
            this.cbPermitted.Location = new System.Drawing.Point(11, 32);
            this.cbPermitted.Name = "cbPermitted";
            this.cbPermitted.Size = new System.Drawing.Size(90, 21);
            this.cbPermitted.TabIndex = 7;
            this.cbPermitted.Text = "Permitted";
            this.cbPermitted.UseVisualStyleBackColor = true;
            this.cbPermitted.CheckedChanged += new System.EventHandler(this.cbPermitted_CheckedChanged);
            this.cbPermitted.Click += new System.EventHandler(this.cbPermitted_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbExcluded);
            this.groupBox2.Controls.Add(this.butRemoveExcluded);
            this.groupBox2.Controls.Add(this.butAddExcluded);
            this.groupBox2.Controls.Add(this.dgvExcluded);
            this.groupBox2.Location = new System.Drawing.Point(12, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(471, 139);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Excluded names";
            // 
            // cbExcluded
            // 
            this.cbExcluded.AutoSize = true;
            this.cbExcluded.Location = new System.Drawing.Point(11, 32);
            this.cbExcluded.Name = "cbExcluded";
            this.cbExcluded.Size = new System.Drawing.Size(87, 21);
            this.cbExcluded.TabIndex = 3;
            this.cbExcluded.Text = "Excluded";
            this.cbExcluded.UseVisualStyleBackColor = true;
            this.cbExcluded.CheckedChanged += new System.EventHandler(this.cbExcluded_CheckedChanged);
            this.cbExcluded.Click += new System.EventHandler(this.cbExcluded_CheckedChanged);
            // 
            // butRemoveExcluded
            // 
            this.butRemoveExcluded.Location = new System.Drawing.Point(347, 23);
            this.butRemoveExcluded.Name = "butRemoveExcluded";
            this.butRemoveExcluded.Size = new System.Drawing.Size(82, 31);
            this.butRemoveExcluded.TabIndex = 2;
            this.butRemoveExcluded.Text = "Remove";
            this.butRemoveExcluded.UseVisualStyleBackColor = true;
            this.butRemoveExcluded.Click += new System.EventHandler(this.butRemoveExcluded_Click);
            // 
            // butAddExcluded
            // 
            this.butAddExcluded.Location = new System.Drawing.Point(231, 23);
            this.butAddExcluded.Name = "butAddExcluded";
            this.butAddExcluded.Size = new System.Drawing.Size(82, 31);
            this.butAddExcluded.TabIndex = 1;
            this.butAddExcluded.Text = "Add";
            this.butAddExcluded.UseVisualStyleBackColor = true;
            this.butAddExcluded.Click += new System.EventHandler(this.butAddExcluded_Click);
            // 
            // NameConstraints
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
            this.Name = "NameConstraints";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Name Constraints";
            ((System.ComponentModel.ISupportInitialize)(this.dgvExcluded)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPermitted)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvExcluded;
        private System.Windows.Forms.Button butAddPermitted;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox cbCritical;
        private System.Windows.Forms.Button butRemovePermitted;
        private System.Windows.Forms.DataGridView dgvPermitted;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbPermitted;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbExcluded;
        private System.Windows.Forms.Button butRemoveExcluded;
        private System.Windows.Forms.Button butAddExcluded;
    }
}