namespace OSCASnapin
{
    partial class CertSave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CertSave));
            this.rbDER = new System.Windows.Forms.RadioButton();
            this.rbBase64 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbPEM = new System.Windows.Forms.RadioButton();
            this.butSave = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.rbPk7 = new System.Windows.Forms.RadioButton();
            this.cbCRL = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbPath = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbDER
            // 
            this.rbDER.AutoSize = true;
            this.rbDER.Checked = true;
            this.rbDER.Location = new System.Drawing.Point(85, 36);
            this.rbDER.Name = "rbDER";
            this.rbDER.Size = new System.Drawing.Size(61, 22);
            this.rbDER.TabIndex = 0;
            this.rbDER.TabStop = true;
            this.rbDER.Text = "DER";
            this.rbDER.UseVisualStyleBackColor = true;
            // 
            // rbBase64
            // 
            this.rbBase64.AutoSize = true;
            this.rbBase64.Location = new System.Drawing.Point(191, 36);
            this.rbBase64.Name = "rbBase64";
            this.rbBase64.Size = new System.Drawing.Size(79, 22);
            this.rbBase64.TabIndex = 1;
            this.rbBase64.Text = "Base64";
            this.rbBase64.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.rbPEM);
            this.groupBox1.Controls.Add(this.rbDER);
            this.groupBox1.Controls.Add(this.rbBase64);
            this.groupBox1.Location = new System.Drawing.Point(15, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 85);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Format";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(212, 84);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 100);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // rbPEM
            // 
            this.rbPEM.AutoSize = true;
            this.rbPEM.Location = new System.Drawing.Point(321, 36);
            this.rbPEM.Name = "rbPEM";
            this.rbPEM.Size = new System.Drawing.Size(62, 22);
            this.rbPEM.TabIndex = 2;
            this.rbPEM.Text = "PEM";
            this.rbPEM.UseVisualStyleBackColor = true;
            // 
            // butSave
            // 
            this.butSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butSave.Location = new System.Drawing.Point(206, 226);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(92, 35);
            this.butSave.TabIndex = 3;
            this.butSave.Text = "Save";
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "cer";
            this.saveFileDialog.Filter = "X.509 Certificates |\".cer|PEM Certificates|*.pem";
            this.saveFileDialog.Title = "Save certificate";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // rbPk7
            // 
            this.rbPk7.AutoSize = true;
            this.rbPk7.Location = new System.Drawing.Point(85, 38);
            this.rbPk7.Name = "rbPk7";
            this.rbPk7.Size = new System.Drawing.Size(86, 22);
            this.rbPk7.TabIndex = 1;
            this.rbPk7.Text = "PKCS#7";
            this.rbPk7.UseVisualStyleBackColor = true;
            // 
            // cbCRL
            // 
            this.cbCRL.AutoSize = true;
            this.cbCRL.Location = new System.Drawing.Point(321, 38);
            this.cbCRL.Name = "cbCRL";
            this.cbCRL.Size = new System.Drawing.Size(89, 22);
            this.cbCRL.TabIndex = 2;
            this.cbCRL.Text = "Add CRL";
            this.cbCRL.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbPath);
            this.groupBox2.Controls.Add(this.cbCRL);
            this.groupBox2.Controls.Add(this.rbPk7);
            this.groupBox2.Location = new System.Drawing.Point(15, 117);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(460, 85);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PKCS#7";
            // 
            // cbPath
            // 
            this.cbPath.AutoSize = true;
            this.cbPath.Checked = true;
            this.cbPath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPath.Location = new System.Drawing.Point(191, 38);
            this.cbPath.Name = "cbPath";
            this.cbPath.Size = new System.Drawing.Size(108, 22);
            this.cbPath.TabIndex = 3;
            this.cbPath.Text = "Add CA cert";
            this.cbPath.UseVisualStyleBackColor = true;
            // 
            // CertSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 281);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.butSave);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CertSave";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Save certificate";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbDER;
        private System.Windows.Forms.RadioButton rbBase64;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbPEM;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbPk7;
        private System.Windows.Forms.CheckBox cbCRL;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbPath;
    }
}