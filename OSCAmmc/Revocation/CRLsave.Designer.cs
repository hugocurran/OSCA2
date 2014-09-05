namespace OSCASnapin
{
    partial class CRLsave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CRLsave));
            this.rbDER = new System.Windows.Forms.RadioButton();
            this.rbBase64 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbPEM = new System.Windows.Forms.RadioButton();
            this.butSave = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbDER
            // 
            this.rbDER.AutoSize = true;
            this.rbDER.Checked = true;
            this.rbDER.Location = new System.Drawing.Point(83, 36);
            this.rbDER.Name = "rbDER";
            this.rbDER.Size = new System.Drawing.Size(58, 21);
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
            this.rbBase64.Size = new System.Drawing.Size(77, 21);
            this.rbBase64.TabIndex = 1;
            this.rbBase64.Text = "Base64";
            this.rbBase64.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbPEM);
            this.groupBox1.Controls.Add(this.rbDER);
            this.groupBox1.Controls.Add(this.rbBase64);
            this.groupBox1.Location = new System.Drawing.Point(15, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 86);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Format";
            // 
            // rbPEM
            // 
            this.rbPEM.AutoSize = true;
            this.rbPEM.Location = new System.Drawing.Point(321, 36);
            this.rbPEM.Name = "rbPEM";
            this.rbPEM.Size = new System.Drawing.Size(58, 21);
            this.rbPEM.TabIndex = 2;
            this.rbPEM.Text = "PEM";
            this.rbPEM.UseVisualStyleBackColor = true;
            // 
            // butSave
            // 
            this.butSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butSave.Location = new System.Drawing.Point(200, 125);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(92, 35);
            this.butSave.TabIndex = 3;
            this.butSave.Text = "Save";
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "crl";
            this.saveFileDialog.Title = "Save CRL";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // CRLsave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 186);
            this.Controls.Add(this.butSave);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CRLsave";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Save CRL";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbDER;
        private System.Windows.Forms.RadioButton rbBase64;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbPEM;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}