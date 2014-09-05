namespace OSCASnapin.Profiles
{
    partial class ExtendedKeyUsage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtendedKeyUsage));
            this.cbAnyExtendedKeyUsage = new System.Windows.Forms.CheckBox();
            this.cbServerAuth = new System.Windows.Forms.CheckBox();
            this.cbClientAuth = new System.Windows.Forms.CheckBox();
            this.cbCodeSigning = new System.Windows.Forms.CheckBox();
            this.cbEmailProtection = new System.Windows.Forms.CheckBox();
            this.cbIpsecEndSystem = new System.Windows.Forms.CheckBox();
            this.cbIpsecTunnel = new System.Windows.Forms.CheckBox();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.cbIpsecUser = new System.Windows.Forms.CheckBox();
            this.cbTimeStamping = new System.Windows.Forms.CheckBox();
            this.cbCritical = new System.Windows.Forms.CheckBox();
            this.cbOcspSigning = new System.Windows.Forms.CheckBox();
            this.cbSmartCardLogon = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbAnyExtendedKeyUsage
            // 
            this.cbAnyExtendedKeyUsage.AutoSize = true;
            this.cbAnyExtendedKeyUsage.Location = new System.Drawing.Point(12, 12);
            this.cbAnyExtendedKeyUsage.Name = "cbAnyExtendedKeyUsage";
            this.cbAnyExtendedKeyUsage.Size = new System.Drawing.Size(187, 22);
            this.cbAnyExtendedKeyUsage.TabIndex = 0;
            this.cbAnyExtendedKeyUsage.Tag = "AnyExtendedKeyUsage";
            this.cbAnyExtendedKeyUsage.Text = "Any ExtendedKeyUsage";
            this.cbAnyExtendedKeyUsage.UseVisualStyleBackColor = true;
            this.cbAnyExtendedKeyUsage.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbServerAuth
            // 
            this.cbServerAuth.AutoSize = true;
            this.cbServerAuth.Location = new System.Drawing.Point(12, 40);
            this.cbServerAuth.Name = "cbServerAuth";
            this.cbServerAuth.Size = new System.Drawing.Size(169, 22);
            this.cbServerAuth.TabIndex = 1;
            this.cbServerAuth.Tag = "ServerAuth";
            this.cbServerAuth.Text = "Server Authentication";
            this.cbServerAuth.UseVisualStyleBackColor = true;
            this.cbServerAuth.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbClientAuth
            // 
            this.cbClientAuth.AutoSize = true;
            this.cbClientAuth.Location = new System.Drawing.Point(12, 68);
            this.cbClientAuth.Name = "cbClientAuth";
            this.cbClientAuth.Size = new System.Drawing.Size(163, 22);
            this.cbClientAuth.TabIndex = 2;
            this.cbClientAuth.Tag = "ClientAuth";
            this.cbClientAuth.Text = "Client Authentication";
            this.cbClientAuth.UseVisualStyleBackColor = true;
            this.cbClientAuth.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbCodeSigning
            // 
            this.cbCodeSigning.AutoSize = true;
            this.cbCodeSigning.Location = new System.Drawing.Point(12, 96);
            this.cbCodeSigning.Name = "cbCodeSigning";
            this.cbCodeSigning.Size = new System.Drawing.Size(118, 22);
            this.cbCodeSigning.TabIndex = 3;
            this.cbCodeSigning.Tag = "CodeSigning";
            this.cbCodeSigning.Text = "Code Signing";
            this.cbCodeSigning.UseVisualStyleBackColor = true;
            this.cbCodeSigning.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbEmailProtection
            // 
            this.cbEmailProtection.AutoSize = true;
            this.cbEmailProtection.Location = new System.Drawing.Point(12, 124);
            this.cbEmailProtection.Name = "cbEmailProtection";
            this.cbEmailProtection.Size = new System.Drawing.Size(139, 22);
            this.cbEmailProtection.TabIndex = 4;
            this.cbEmailProtection.Tag = "EmailProtection";
            this.cbEmailProtection.Text = "Email Protection";
            this.cbEmailProtection.UseVisualStyleBackColor = true;
            this.cbEmailProtection.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbIpsecEndSystem
            // 
            this.cbIpsecEndSystem.AutoSize = true;
            this.cbIpsecEndSystem.Location = new System.Drawing.Point(12, 152);
            this.cbIpsecEndSystem.Name = "cbIpsecEndSystem";
            this.cbIpsecEndSystem.Size = new System.Drawing.Size(151, 22);
            this.cbIpsecEndSystem.TabIndex = 5;
            this.cbIpsecEndSystem.Tag = "IpsecEndSystem";
            this.cbIpsecEndSystem.Text = "IPsec End System";
            this.cbIpsecEndSystem.UseVisualStyleBackColor = true;
            this.cbIpsecEndSystem.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbIpsecTunnel
            // 
            this.cbIpsecTunnel.AutoSize = true;
            this.cbIpsecTunnel.Location = new System.Drawing.Point(12, 180);
            this.cbIpsecTunnel.Name = "cbIpsecTunnel";
            this.cbIpsecTunnel.Size = new System.Drawing.Size(111, 22);
            this.cbIpsecTunnel.TabIndex = 6;
            this.cbIpsecTunnel.Tag = "IpsecTunnel";
            this.cbIpsecTunnel.Text = "IPsecTunnel";
            this.cbIpsecTunnel.UseVisualStyleBackColor = true;
            this.cbIpsecTunnel.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // butOK
            // 
            this.butOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOK.Location = new System.Drawing.Point(69, 361);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(82, 31);
            this.butOK.TabIndex = 7;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(69, 409);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(82, 31);
            this.butCancel.TabIndex = 8;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // cbIpsecUser
            // 
            this.cbIpsecUser.AutoSize = true;
            this.cbIpsecUser.Location = new System.Drawing.Point(12, 208);
            this.cbIpsecUser.Name = "cbIpsecUser";
            this.cbIpsecUser.Size = new System.Drawing.Size(103, 22);
            this.cbIpsecUser.TabIndex = 9;
            this.cbIpsecUser.Tag = "IpsecUser";
            this.cbIpsecUser.Text = "IPsec User";
            this.cbIpsecUser.UseVisualStyleBackColor = true;
            this.cbIpsecUser.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbTimeStamping
            // 
            this.cbTimeStamping.AutoSize = true;
            this.cbTimeStamping.Location = new System.Drawing.Point(12, 236);
            this.cbTimeStamping.Name = "cbTimeStamping";
            this.cbTimeStamping.Size = new System.Drawing.Size(129, 22);
            this.cbTimeStamping.TabIndex = 10;
            this.cbTimeStamping.Tag = "TimeStamping";
            this.cbTimeStamping.Text = "Time Stamping";
            this.cbTimeStamping.UseVisualStyleBackColor = true;
            this.cbTimeStamping.CheckedChanged += new System.EventHandler(this.cb_Changed);
            // 
            // cbCritical
            // 
            this.cbCritical.AutoSize = true;
            this.cbCritical.Location = new System.Drawing.Point(69, 320);
            this.cbCritical.Name = "cbCritical";
            this.cbCritical.Size = new System.Drawing.Size(75, 22);
            this.cbCritical.TabIndex = 11;
            this.cbCritical.Text = "Critical";
            this.cbCritical.UseVisualStyleBackColor = true;
            this.cbCritical.CheckedChanged += new System.EventHandler(this.cbCritical_CheckedChanged);
            // 
            // cbOcspSigning
            // 
            this.cbOcspSigning.AutoSize = true;
            this.cbOcspSigning.Location = new System.Drawing.Point(12, 264);
            this.cbOcspSigning.Name = "cbOcspSigning";
            this.cbOcspSigning.Size = new System.Drawing.Size(125, 22);
            this.cbOcspSigning.TabIndex = 12;
            this.cbOcspSigning.Tag = "OcspSigning";
            this.cbOcspSigning.Text = "OCSP Signing";
            this.cbOcspSigning.UseVisualStyleBackColor = true;
            // 
            // cbSmartCardLogon
            // 
            this.cbSmartCardLogon.AutoSize = true;
            this.cbSmartCardLogon.Location = new System.Drawing.Point(12, 292);
            this.cbSmartCardLogon.Name = "cbSmartCardLogon";
            this.cbSmartCardLogon.Size = new System.Drawing.Size(152, 22);
            this.cbSmartCardLogon.TabIndex = 13;
            this.cbSmartCardLogon.Tag = "SmartCardLogon";
            this.cbSmartCardLogon.Text = "Smart Card Logon";
            this.cbSmartCardLogon.UseVisualStyleBackColor = true;
            // 
            // ExtendedKeyUsage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 452);
            this.Controls.Add(this.cbSmartCardLogon);
            this.Controls.Add(this.cbOcspSigning);
            this.Controls.Add(this.cbCritical);
            this.Controls.Add(this.cbTimeStamping);
            this.Controls.Add(this.cbIpsecUser);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.cbIpsecTunnel);
            this.Controls.Add(this.cbIpsecEndSystem);
            this.Controls.Add(this.cbEmailProtection);
            this.Controls.Add(this.cbCodeSigning);
            this.Controls.Add(this.cbClientAuth);
            this.Controls.Add(this.cbServerAuth);
            this.Controls.Add(this.cbAnyExtendedKeyUsage);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtendedKeyUsage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extended Key Usage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        

        #endregion

        private System.Windows.Forms.CheckBox cbAnyExtendedKeyUsage;
        private System.Windows.Forms.CheckBox cbServerAuth;
        private System.Windows.Forms.CheckBox cbClientAuth;
        private System.Windows.Forms.CheckBox cbCodeSigning;
        private System.Windows.Forms.CheckBox cbEmailProtection;
        private System.Windows.Forms.CheckBox cbIpsecEndSystem;
        private System.Windows.Forms.CheckBox cbIpsecTunnel;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox cbIpsecUser;
        private System.Windows.Forms.CheckBox cbTimeStamping;
        private System.Windows.Forms.CheckBox cbCritical;
        private System.Windows.Forms.CheckBox cbOcspSigning;
        private System.Windows.Forms.CheckBox cbSmartCardLogon;
    }
}