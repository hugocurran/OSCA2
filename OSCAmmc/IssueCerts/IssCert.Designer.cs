namespace OSCASnapin
{
    partial class IssCert
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssCert));
            this.tbFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.butBrowse = new System.Windows.Forms.Button();
            this.gbFile = new System.Windows.Forms.GroupBox();
            this.gbPaste = new System.Windows.Forms.GroupBox();
            this.tbPaste = new System.Windows.Forms.TextBox();
            this.butSubmit = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.cbProfile = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.tbValidPeriod = new System.Windows.Forms.TextBox();
            this.lbValidUnits = new System.Windows.Forms.ComboBox();
            this.lbProfiles = new System.Windows.Forms.ComboBox();
            this.lbSubject = new System.Windows.Forms.Label();
            this.lbAltNames = new System.Windows.Forms.Label();
            this.lbKeyType = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblStatusString = new System.Windows.Forms.Label();
            this.gbFile.SuspendLayout();
            this.gbPaste.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbFile
            // 
            this.tbFile.Location = new System.Drawing.Point(178, 38);
            this.tbFile.Name = "tbFile";
            this.tbFile.Size = new System.Drawing.Size(173, 24);
            this.tbFile.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "PKCS#10 request file:";
            // 
            // butBrowse
            // 
            this.butBrowse.Location = new System.Drawing.Point(358, 35);
            this.butBrowse.Name = "butBrowse";
            this.butBrowse.Size = new System.Drawing.Size(92, 35);
            this.butBrowse.TabIndex = 3;
            this.butBrowse.Text = "Browse";
            this.butBrowse.UseVisualStyleBackColor = true;
            this.butBrowse.Click += new System.EventHandler(this.butBrowse_Click);
            // 
            // gbFile
            // 
            this.gbFile.Controls.Add(this.label2);
            this.gbFile.Controls.Add(this.butBrowse);
            this.gbFile.Controls.Add(this.tbFile);
            this.gbFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFile.Location = new System.Drawing.Point(61, 12);
            this.gbFile.Name = "gbFile";
            this.gbFile.Size = new System.Drawing.Size(489, 94);
            this.gbFile.TabIndex = 4;
            this.gbFile.TabStop = false;
            this.gbFile.Text = "Import a request file";
            // 
            // gbPaste
            // 
            this.gbPaste.Controls.Add(this.tbPaste);
            this.gbPaste.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPaste.Location = new System.Drawing.Point(61, 112);
            this.gbPaste.Name = "gbPaste";
            this.gbPaste.Size = new System.Drawing.Size(489, 146);
            this.gbPaste.TabIndex = 5;
            this.gbPaste.TabStop = false;
            this.gbPaste.Text = "Paste a request";
            // 
            // tbPaste
            // 
            this.tbPaste.Location = new System.Drawing.Point(10, 25);
            this.tbPaste.Multiline = true;
            this.tbPaste.Name = "tbPaste";
            this.tbPaste.Size = new System.Drawing.Size(472, 114);
            this.tbPaste.TabIndex = 0;
            this.tbPaste.TextChanged += new System.EventHandler(this.tbPaste_TextChanged);
            // 
            // butSubmit
            // 
            this.butSubmit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butSubmit.Enabled = false;
            this.butSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butSubmit.Location = new System.Drawing.Point(146, 520);
            this.butSubmit.Name = "butSubmit";
            this.butSubmit.Size = new System.Drawing.Size(92, 35);
            this.butSubmit.TabIndex = 6;
            this.butSubmit.Text = "Submit";
            this.butSubmit.UseVisualStyleBackColor = true;
            this.butSubmit.Click += new System.EventHandler(this.butSubmit_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butCancel.Location = new System.Drawing.Point(377, 520);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(92, 35);
            this.butCancel.TabIndex = 7;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // cbProfile
            // 
            this.cbProfile.AutoSize = true;
            this.cbProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbProfile.Location = new System.Drawing.Point(67, 421);
            this.cbProfile.Name = "cbProfile";
            this.cbProfile.Size = new System.Drawing.Size(113, 22);
            this.cbProfile.TabIndex = 9;
            this.cbProfile.Text = "Use a profile";
            this.cbProfile.UseVisualStyleBackColor = true;
            this.cbProfile.CheckedChanged += new System.EventHandler(this.cbProfile_CheckedChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "All files (*.*)|*.*";
            this.openFileDialog.InitialDirectory = "C:\\";
            this.openFileDialog.Title = "Select a PKCS#10 request file";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(63, 471);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 18);
            this.label1.TabIndex = 12;
            this.label1.Text = "Certificate validity period:";
            // 
            // tbValidPeriod
            // 
            this.tbValidPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValidPeriod.Location = new System.Drawing.Point(239, 468);
            this.tbValidPeriod.Name = "tbValidPeriod";
            this.tbValidPeriod.Size = new System.Drawing.Size(41, 24);
            this.tbValidPeriod.TabIndex = 13;
            this.tbValidPeriod.Text = "0";
            this.tbValidPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbValidUnits
            // 
            this.lbValidUnits.FormattingEnabled = true;
            this.lbValidUnits.Items.AddRange(new object[] {
            "Years",
            "Months",
            "Days"});
            this.lbValidUnits.Location = new System.Drawing.Point(286, 468);
            this.lbValidUnits.Name = "lbValidUnits";
            this.lbValidUnits.Size = new System.Drawing.Size(87, 26);
            this.lbValidUnits.TabIndex = 14;
            // 
            // lbProfiles
            // 
            this.lbProfiles.FormattingEnabled = true;
            this.lbProfiles.Location = new System.Drawing.Point(186, 421);
            this.lbProfiles.Name = "lbProfiles";
            this.lbProfiles.Size = new System.Drawing.Size(300, 26);
            this.lbProfiles.TabIndex = 15;
            this.lbProfiles.SelectedIndexChanged += new System.EventHandler(this.lbProfiles_SelectedIndexChanged);
            // 
            // lbSubject
            // 
            this.lbSubject.AutoSize = true;
            this.lbSubject.Location = new System.Drawing.Point(6, 31);
            this.lbSubject.Name = "lbSubject";
            this.lbSubject.Size = new System.Drawing.Size(57, 18);
            this.lbSubject.TabIndex = 16;
            this.lbSubject.Text = "Subject";
            // 
            // lbAltNames
            // 
            this.lbAltNames.AutoSize = true;
            this.lbAltNames.Location = new System.Drawing.Point(6, 49);
            this.lbAltNames.Name = "lbAltNames";
            this.lbAltNames.Size = new System.Drawing.Size(73, 18);
            this.lbAltNames.TabIndex = 17;
            this.lbAltNames.Text = "Alt names";
            // 
            // lbKeyType
            // 
            this.lbKeyType.AutoSize = true;
            this.lbKeyType.Location = new System.Drawing.Point(6, 67);
            this.lbKeyType.Name = "lbKeyType";
            this.lbKeyType.Size = new System.Drawing.Size(64, 18);
            this.lbKeyType.TabIndex = 18;
            this.lbKeyType.Text = "Key type";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblStatusString);
            this.groupBox1.Controls.Add(this.lbSubject);
            this.groupBox1.Controls.Add(this.lbKeyType);
            this.groupBox1.Controls.Add(this.lbAltNames);
            this.groupBox1.Location = new System.Drawing.Point(71, 264);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(479, 142);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Request";
            // 
            // lblStatusString
            // 
            this.lblStatusString.AutoSize = true;
            this.lblStatusString.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusString.Location = new System.Drawing.Point(17, 98);
            this.lblStatusString.Name = "lblStatusString";
            this.lblStatusString.Size = new System.Drawing.Size(127, 24);
            this.lblStatusString.TabIndex = 20;
            this.lblStatusString.Text = "lblStatusString";
            // 
            // IssCert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(629, 577);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbProfiles);
            this.Controls.Add(this.lbValidUnits);
            this.Controls.Add(this.tbValidPeriod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbProfile);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butSubmit);
            this.Controls.Add(this.gbPaste);
            this.Controls.Add(this.gbFile);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IssCert";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Issue a new certificate";
            this.TopMost = true;
            this.gbFile.ResumeLayout(false);
            this.gbFile.PerformLayout();
            this.gbPaste.ResumeLayout(false);
            this.gbPaste.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button butBrowse;
        private System.Windows.Forms.GroupBox gbFile;
        private System.Windows.Forms.GroupBox gbPaste;
        private System.Windows.Forms.TextBox tbPaste;
        private System.Windows.Forms.Button butSubmit;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox cbProfile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbValidPeriod;
        private System.Windows.Forms.ComboBox lbValidUnits;
        private System.Windows.Forms.ComboBox lbProfiles;
        private System.Windows.Forms.Label lbSubject;
        private System.Windows.Forms.Label lbAltNames;
        private System.Windows.Forms.Label lbKeyType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblStatusString;
    
    }
}