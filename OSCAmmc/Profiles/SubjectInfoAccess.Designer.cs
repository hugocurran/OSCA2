namespace OSCASnapin.Profiles
{
    partial class SubjectInfoAccess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubjectInfoAccess));
            this.dgvTimeStamping = new System.Windows.Forms.DataGridView();
            this.butAddCaRepository = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.cbCritical = new System.Windows.Forms.CheckBox();
            this.butRemoveCaRepository = new System.Windows.Forms.Button();
            this.dgvCaRepository = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbCaRepository = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbTimeStamping = new System.Windows.Forms.CheckBox();
            this.butRemoveTimeStamping = new System.Windows.Forms.Button();
            this.butAddTimeStamping = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimeStamping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCaRepository)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTimeStamping
            // 
            this.dgvTimeStamping.AllowUserToAddRows = false;
            this.dgvTimeStamping.AllowUserToDeleteRows = false;
            this.dgvTimeStamping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTimeStamping.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTimeStamping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTimeStamping.Location = new System.Drawing.Point(0, 65);
            this.dgvTimeStamping.MultiSelect = false;
            this.dgvTimeStamping.Name = "dgvTimeStamping";
            this.dgvTimeStamping.ReadOnly = true;
            this.dgvTimeStamping.RowHeadersVisible = false;
            this.dgvTimeStamping.RowTemplate.Height = 24;
            this.dgvTimeStamping.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTimeStamping.ShowEditingIcon = false;
            this.dgvTimeStamping.Size = new System.Drawing.Size(471, 78);
            this.dgvTimeStamping.TabIndex = 0;
            // 
            // butAddCaRepository
            // 
            this.butAddCaRepository.Location = new System.Drawing.Point(231, 23);
            this.butAddCaRepository.Name = "butAddCaRepository";
            this.butAddCaRepository.Size = new System.Drawing.Size(82, 31);
            this.butAddCaRepository.TabIndex = 1;
            this.butAddCaRepository.Text = "Add";
            this.butAddCaRepository.UseVisualStyleBackColor = true;
            this.butAddCaRepository.Click += new System.EventHandler(this.butAddCaRepository_Click);
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
            // butRemoveCaRepository
            // 
            this.butRemoveCaRepository.Location = new System.Drawing.Point(347, 23);
            this.butRemoveCaRepository.Name = "butRemoveCaRepository";
            this.butRemoveCaRepository.Size = new System.Drawing.Size(82, 31);
            this.butRemoveCaRepository.TabIndex = 5;
            this.butRemoveCaRepository.Text = "Remove";
            this.butRemoveCaRepository.UseVisualStyleBackColor = true;
            this.butRemoveCaRepository.Click += new System.EventHandler(this.butRemoveCaRepository_Click);
            // 
            // dgvCaRepository
            // 
            this.dgvCaRepository.AllowUserToAddRows = false;
            this.dgvCaRepository.AllowUserToDeleteRows = false;
            this.dgvCaRepository.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCaRepository.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCaRepository.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCaRepository.Location = new System.Drawing.Point(0, 67);
            this.dgvCaRepository.MultiSelect = false;
            this.dgvCaRepository.Name = "dgvCaRepository";
            this.dgvCaRepository.ReadOnly = true;
            this.dgvCaRepository.RowHeadersVisible = false;
            this.dgvCaRepository.RowTemplate.Height = 24;
            this.dgvCaRepository.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCaRepository.ShowEditingIcon = false;
            this.dgvCaRepository.Size = new System.Drawing.Size(471, 76);
            this.dgvCaRepository.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbCaRepository);
            this.groupBox1.Controls.Add(this.dgvCaRepository);
            this.groupBox1.Controls.Add(this.butRemoveCaRepository);
            this.groupBox1.Controls.Add(this.butAddCaRepository);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(471, 149);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CA Repository";
            // 
            // cbCaRepository
            // 
            this.cbCaRepository.AutoSize = true;
            this.cbCaRepository.Location = new System.Drawing.Point(11, 32);
            this.cbCaRepository.Name = "cbCaRepository";
            this.cbCaRepository.Size = new System.Drawing.Size(120, 21);
            this.cbCaRepository.TabIndex = 7;
            this.cbCaRepository.Text = "CA Repository";
            this.cbCaRepository.UseVisualStyleBackColor = true;
            this.cbCaRepository.CheckedChanged += new System.EventHandler(this.cbCaRepository_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbTimeStamping);
            this.groupBox2.Controls.Add(this.butRemoveTimeStamping);
            this.groupBox2.Controls.Add(this.butAddTimeStamping);
            this.groupBox2.Controls.Add(this.dgvTimeStamping);
            this.groupBox2.Location = new System.Drawing.Point(12, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(471, 139);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Time Stamping";
            // 
            // cbTimeStamping
            // 
            this.cbTimeStamping.AutoSize = true;
            this.cbTimeStamping.Location = new System.Drawing.Point(11, 32);
            this.cbTimeStamping.Name = "cbTimeStamping";
            this.cbTimeStamping.Size = new System.Drawing.Size(124, 21);
            this.cbTimeStamping.TabIndex = 3;
            this.cbTimeStamping.Text = "Time Stamping";
            this.cbTimeStamping.UseVisualStyleBackColor = true;
            this.cbTimeStamping.CheckedChanged += new System.EventHandler(this.cbTimeStamping_CheckedChanged);
            // 
            // butRemoveTimeStamping
            // 
            this.butRemoveTimeStamping.Location = new System.Drawing.Point(347, 23);
            this.butRemoveTimeStamping.Name = "butRemoveTimeStamping";
            this.butRemoveTimeStamping.Size = new System.Drawing.Size(82, 31);
            this.butRemoveTimeStamping.TabIndex = 2;
            this.butRemoveTimeStamping.Text = "Remove";
            this.butRemoveTimeStamping.UseVisualStyleBackColor = true;
            this.butRemoveTimeStamping.Click += new System.EventHandler(this.butRemoveTimeStamping_Click);
            // 
            // butAddTimeStamping
            // 
            this.butAddTimeStamping.Location = new System.Drawing.Point(231, 23);
            this.butAddTimeStamping.Name = "butAddTimeStamping";
            this.butAddTimeStamping.Size = new System.Drawing.Size(82, 31);
            this.butAddTimeStamping.TabIndex = 1;
            this.butAddTimeStamping.Text = "Add";
            this.butAddTimeStamping.UseVisualStyleBackColor = true;
            this.butAddTimeStamping.Click += new System.EventHandler(this.butAddTimeStamping_Click);
            // 
            // SubjectInfoAccess
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
            this.Name = "SubjectInfoAccess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Subject Info Access";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimeStamping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCaRepository)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTimeStamping;
        private System.Windows.Forms.Button butAddCaRepository;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox cbCritical;
        private System.Windows.Forms.Button butRemoveCaRepository;
        private System.Windows.Forms.DataGridView dgvCaRepository;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbCaRepository;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbTimeStamping;
        private System.Windows.Forms.Button butRemoveTimeStamping;
        private System.Windows.Forms.Button butAddTimeStamping;
    }
}