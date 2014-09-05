namespace OSCASnapin.Profiles
{
    partial class KeyUsage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyUsage));
            this.cbDigitalSignature = new System.Windows.Forms.CheckBox();
            this.cbNonRepudiation = new System.Windows.Forms.CheckBox();
            this.cbDataEncipherment = new System.Windows.Forms.CheckBox();
            this.cbKeyEncipherment = new System.Windows.Forms.CheckBox();
            this.cbKeyAgreement = new System.Windows.Forms.CheckBox();
            this.cbKeyCertSign = new System.Windows.Forms.CheckBox();
            this.cbCRLSign = new System.Windows.Forms.CheckBox();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.cbEncipherOnly = new System.Windows.Forms.CheckBox();
            this.cbDecipherOnly = new System.Windows.Forms.CheckBox();
            this.cbCritical = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbDigitalSignature
            // 
            this.cbDigitalSignature.AutoSize = true;
            this.cbDigitalSignature.Location = new System.Drawing.Point(12, 12);
            this.cbDigitalSignature.Name = "cbDigitalSignature";
            this.cbDigitalSignature.Size = new System.Drawing.Size(136, 22);
            this.cbDigitalSignature.TabIndex = 0;
            this.cbDigitalSignature.Tag = "DigitalSignature";
            this.cbDigitalSignature.Text = "Digital Signature";
            this.cbDigitalSignature.UseVisualStyleBackColor = true;
            this.cbDigitalSignature.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbNonRepudiation
            // 
            this.cbNonRepudiation.AutoSize = true;
            this.cbNonRepudiation.Location = new System.Drawing.Point(12, 40);
            this.cbNonRepudiation.Name = "cbNonRepudiation";
            this.cbNonRepudiation.Size = new System.Drawing.Size(140, 22);
            this.cbNonRepudiation.TabIndex = 1;
            this.cbNonRepudiation.Tag = "NonRepudiation";
            this.cbNonRepudiation.Text = "Non Repudiation";
            this.cbNonRepudiation.UseVisualStyleBackColor = true;
            this.cbNonRepudiation.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbDataEncipherment
            // 
            this.cbDataEncipherment.AutoSize = true;
            this.cbDataEncipherment.Location = new System.Drawing.Point(12, 96);
            this.cbDataEncipherment.Name = "cbDataEncipherment";
            this.cbDataEncipherment.Size = new System.Drawing.Size(156, 22);
            this.cbDataEncipherment.TabIndex = 2;
            this.cbDataEncipherment.Tag = "DataEncipherment";
            this.cbDataEncipherment.Text = "Data Encipherment";
            this.cbDataEncipherment.UseVisualStyleBackColor = true;
            this.cbDataEncipherment.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbKeyEncipherment
            // 
            this.cbKeyEncipherment.AutoSize = true;
            this.cbKeyEncipherment.Location = new System.Drawing.Point(12, 68);
            this.cbKeyEncipherment.Name = "cbKeyEncipherment";
            this.cbKeyEncipherment.Size = new System.Drawing.Size(150, 22);
            this.cbKeyEncipherment.TabIndex = 3;
            this.cbKeyEncipherment.Tag = "KeyEncipherment";
            this.cbKeyEncipherment.Text = "Key Encipherment";
            this.cbKeyEncipherment.UseVisualStyleBackColor = true;
            this.cbKeyEncipherment.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbKeyAgreement
            // 
            this.cbKeyAgreement.AutoSize = true;
            this.cbKeyAgreement.Location = new System.Drawing.Point(12, 124);
            this.cbKeyAgreement.Name = "cbKeyAgreement";
            this.cbKeyAgreement.Size = new System.Drawing.Size(130, 22);
            this.cbKeyAgreement.TabIndex = 4;
            this.cbKeyAgreement.Tag = "KeyAgreement";
            this.cbKeyAgreement.Text = "Key Agreement";
            this.cbKeyAgreement.UseVisualStyleBackColor = true;
            this.cbKeyAgreement.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbKeyCertSign
            // 
            this.cbKeyCertSign.AutoSize = true;
            this.cbKeyCertSign.Location = new System.Drawing.Point(12, 152);
            this.cbKeyCertSign.Name = "cbKeyCertSign";
            this.cbKeyCertSign.Size = new System.Drawing.Size(120, 22);
            this.cbKeyCertSign.TabIndex = 5;
            this.cbKeyCertSign.Tag = "KeyCertSign";
            this.cbKeyCertSign.Text = "Key Cert Sign";
            this.cbKeyCertSign.UseVisualStyleBackColor = true;
            this.cbKeyCertSign.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbCRLSign
            // 
            this.cbCRLSign.AutoSize = true;
            this.cbCRLSign.Location = new System.Drawing.Point(12, 180);
            this.cbCRLSign.Name = "cbCRLSign";
            this.cbCRLSign.Size = new System.Drawing.Size(93, 22);
            this.cbCRLSign.TabIndex = 6;
            this.cbCRLSign.Tag = "CRLSign";
            this.cbCRLSign.Text = "CRL Sign";
            this.cbCRLSign.UseVisualStyleBackColor = true;
            this.cbCRLSign.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // butOK
            // 
            this.butOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOK.Location = new System.Drawing.Point(50, 314);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(82, 31);
            this.butOK.TabIndex = 7;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(50, 363);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(82, 31);
            this.butCancel.TabIndex = 8;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // cbEncipherOnly
            // 
            this.cbEncipherOnly.AutoSize = true;
            this.cbEncipherOnly.Location = new System.Drawing.Point(12, 208);
            this.cbEncipherOnly.Name = "cbEncipherOnly";
            this.cbEncipherOnly.Size = new System.Drawing.Size(122, 22);
            this.cbEncipherOnly.TabIndex = 9;
            this.cbEncipherOnly.Tag = "EncipherOnly";
            this.cbEncipherOnly.Text = "Encipher Only";
            this.cbEncipherOnly.UseVisualStyleBackColor = true;
            this.cbEncipherOnly.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbDecipherOnly
            // 
            this.cbDecipherOnly.AutoSize = true;
            this.cbDecipherOnly.Location = new System.Drawing.Point(12, 236);
            this.cbDecipherOnly.Name = "cbDecipherOnly";
            this.cbDecipherOnly.Size = new System.Drawing.Size(123, 22);
            this.cbDecipherOnly.TabIndex = 10;
            this.cbDecipherOnly.Tag = "DecipherOnly";
            this.cbDecipherOnly.Text = "Decipher Only";
            this.cbDecipherOnly.UseVisualStyleBackColor = true;
            this.cbDecipherOnly.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbCritical
            // 
            this.cbCritical.AutoSize = true;
            this.cbCritical.Location = new System.Drawing.Point(50, 274);
            this.cbCritical.Name = "cbCritical";
            this.cbCritical.Size = new System.Drawing.Size(75, 22);
            this.cbCritical.TabIndex = 11;
            this.cbCritical.Text = "Critical";
            this.cbCritical.UseVisualStyleBackColor = true;
            this.cbCritical.CheckedChanged += new System.EventHandler(this.cbCritical_CheckedChanged);
            // 
            // KeyUsage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(178, 406);
            this.Controls.Add(this.cbCritical);
            this.Controls.Add(this.cbDecipherOnly);
            this.Controls.Add(this.cbEncipherOnly);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.cbCRLSign);
            this.Controls.Add(this.cbKeyCertSign);
            this.Controls.Add(this.cbKeyAgreement);
            this.Controls.Add(this.cbKeyEncipherment);
            this.Controls.Add(this.cbDataEncipherment);
            this.Controls.Add(this.cbNonRepudiation);
            this.Controls.Add(this.cbDigitalSignature);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyUsage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Key Usage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbDigitalSignature;
        private System.Windows.Forms.CheckBox cbNonRepudiation;
        private System.Windows.Forms.CheckBox cbDataEncipherment;
        private System.Windows.Forms.CheckBox cbKeyEncipherment;
        private System.Windows.Forms.CheckBox cbKeyAgreement;
        private System.Windows.Forms.CheckBox cbKeyCertSign;
        private System.Windows.Forms.CheckBox cbCRLSign;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox cbEncipherOnly;
        private System.Windows.Forms.CheckBox cbDecipherOnly;
        private System.Windows.Forms.CheckBox cbCritical;
    }
}