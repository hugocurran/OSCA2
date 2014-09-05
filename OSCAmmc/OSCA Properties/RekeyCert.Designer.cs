namespace OSCASnapin
{
    partial class RekeyCert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RekeyCert));
            this.tbFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.butBrowse = new System.Windows.Forms.Button();
            this.gbFile = new System.Windows.Forms.GroupBox();
            this.gbPaste = new System.Windows.Forms.GroupBox();
            this.tbPaste = new System.Windows.Forms.TextBox();
            this.butSubmit = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.ckbProfile = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.tbNotBefore = new System.Windows.Forms.TextBox();
            this.lbProfiles = new System.Windows.Forms.ComboBox();
            this.lbSubject = new System.Windows.Forms.Label();
            this.lbAltNames = new System.Windows.Forms.Label();
            this.lbKeyType = new System.Windows.Forms.Label();
            this.gbRequest = new System.Windows.Forms.GroupBox();
            this.lblStatusString = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbNotAfter = new System.Windows.Forms.TextBox();
            this.ckbOverride = new System.Windows.Forms.CheckBox();
            this.gbFile.SuspendLayout();
            this.gbPaste.SuspendLayout();
            this.gbRequest.SuspendLayout();
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
            // ckbProfile
            // 
            this.ckbProfile.AutoSize = true;
            this.ckbProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckbProfile.Location = new System.Drawing.Point(71, 433);
            this.ckbProfile.Name = "ckbProfile";
            this.ckbProfile.Size = new System.Drawing.Size(113, 22);
            this.ckbProfile.TabIndex = 9;
            this.ckbProfile.Text = "Use a profile";
            this.ckbProfile.UseVisualStyleBackColor = true;
            this.ckbProfile.CheckedChanged += new System.EventHandler(this.cbProfile_CheckedChanged);
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
            this.label1.Location = new System.Drawing.Point(143, 480);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 12;
            this.label1.Text = "NotBefore:";
            // 
            // tbNotBefore
            // 
            this.tbNotBefore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNotBefore.Location = new System.Drawing.Point(229, 474);
            this.tbNotBefore.Name = "tbNotBefore";
            this.tbNotBefore.Size = new System.Drawing.Size(138, 24);
            this.tbNotBefore.TabIndex = 13;
            this.tbNotBefore.Text = "0";
            // 
            // lbProfiles
            // 
            this.lbProfiles.FormattingEnabled = true;
            this.lbProfiles.Location = new System.Drawing.Point(190, 429);
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
            // gbRequest
            // 
            this.gbRequest.Controls.Add(this.lblStatusString);
            this.gbRequest.Controls.Add(this.lbSubject);
            this.gbRequest.Controls.Add(this.lbKeyType);
            this.gbRequest.Controls.Add(this.lbAltNames);
            this.gbRequest.Location = new System.Drawing.Point(71, 264);
            this.gbRequest.Name = "gbRequest";
            this.gbRequest.Size = new System.Drawing.Size(479, 142);
            this.gbRequest.TabIndex = 19;
            this.gbRequest.TabStop = false;
            this.gbRequest.Text = "Request";
            // 
            // lblStatusString
            // 
            this.lblStatusString.AutoSize = true;
            this.lblStatusString.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusString.Location = new System.Drawing.Point(18, 104);
            this.lblStatusString.Name = "lblStatusString";
            this.lblStatusString.Size = new System.Drawing.Size(105, 24);
            this.lblStatusString.TabIndex = 20;
            this.lblStatusString.Text = "statusString";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(373, 477);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 18);
            this.label3.TabIndex = 20;
            this.label3.Text = "NotAfter:";
            // 
            // tbNotAfter
            // 
            this.tbNotAfter.Location = new System.Drawing.Point(445, 474);
            this.tbNotAfter.Name = "tbNotAfter";
            this.tbNotAfter.Size = new System.Drawing.Size(138, 24);
            this.tbNotAfter.TabIndex = 21;
            // 
            // ckbOverride
            // 
            this.ckbOverride.AutoSize = true;
            this.ckbOverride.Location = new System.Drawing.Point(48, 480);
            this.ckbOverride.Name = "ckbOverride";
            this.ckbOverride.Size = new System.Drawing.Size(86, 22);
            this.ckbOverride.TabIndex = 22;
            this.ckbOverride.Text = "Override";
            this.ckbOverride.UseVisualStyleBackColor = true;
            this.ckbOverride.CheckedChanged += new System.EventHandler(this.ckbOverride_CheckedChanged);
            // 
            // RekeyCert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(629, 577);
            this.Controls.Add(this.ckbOverride);
            this.Controls.Add(this.tbNotAfter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gbRequest);
            this.Controls.Add(this.lbProfiles);
            this.Controls.Add(this.tbNotBefore);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ckbProfile);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butSubmit);
            this.Controls.Add(this.gbPaste);
            this.Controls.Add(this.gbFile);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RekeyCert";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rekey a certificate";
            this.TopMost = true;
            this.gbFile.ResumeLayout(false);
            this.gbFile.PerformLayout();
            this.gbPaste.ResumeLayout(false);
            this.gbPaste.PerformLayout();
            this.gbRequest.ResumeLayout(false);
            this.gbRequest.PerformLayout();
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
        private System.Windows.Forms.CheckBox ckbProfile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbNotBefore;
        private System.Windows.Forms.ComboBox lbProfiles;
        private System.Windows.Forms.Label lbSubject;
        private System.Windows.Forms.Label lbAltNames;
        private System.Windows.Forms.Label lbKeyType;
        private System.Windows.Forms.GroupBox gbRequest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbNotAfter;
        private System.Windows.Forms.CheckBox ckbOverride;
        private System.Windows.Forms.Label lblStatusString;

    }
}