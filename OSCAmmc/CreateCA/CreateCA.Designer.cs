namespace OSCASnapin
{
    partial class CreateCA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateCA));
            this.butCreate = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.rbECDSA = new System.Windows.Forms.RadioButton();
            this.rbRSA = new System.Windows.Forms.RadioButton();
            this.rbDSA = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tbFolder = new System.Windows.Forms.TextBox();
            this.butBrowse = new System.Windows.Forms.Button();
            this.rbRoot = new System.Windows.Forms.RadioButton();
            this.rbSubordinate = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rbSHA256 = new System.Windows.Forms.RadioButton();
            this.rbSHA1 = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rb256 = new System.Windows.Forms.RadioButton();
            this.rb4096 = new System.Windows.Forms.RadioButton();
            this.rb2048 = new System.Windows.Forms.RadioButton();
            this.rb1024 = new System.Windows.Forms.RadioButton();
            this.cbFIPS = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbDN = new System.Windows.Forms.TextBox();
            this.gpCertType = new System.Windows.Forms.GroupBox();
            this.rbV3 = new System.Windows.Forms.RadioButton();
            this.rbV1 = new System.Windows.Forms.RadioButton();
            this.gpCertValidity = new System.Windows.Forms.GroupBox();
            this.lbCertUnits = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbCertValid = new System.Windows.Forms.TextBox();
            this.crlPubInt = new System.Windows.Forms.Label();
            this.tbCRLInterval = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lblIssCA = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gpIssuingCA = new System.Windows.Forms.GroupBox();
            this.lbProfile = new System.Windows.Forms.ComboBox();
            this.lbIssuingCA = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbTA = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.gpCertType.SuspendLayout();
            this.gpCertValidity.SuspendLayout();
            this.gpIssuingCA.SuspendLayout();
            this.SuspendLayout();
            // 
            // butCreate
            // 
            this.butCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butCreate.Location = new System.Drawing.Point(96, 658);
            this.butCreate.Name = "butCreate";
            this.butCreate.Size = new System.Drawing.Size(92, 35);
            this.butCreate.TabIndex = 0;
            this.butCreate.Text = "Create CA";
            this.butCreate.UseVisualStyleBackColor = true;
            this.butCreate.Click += new System.EventHandler(this.butCreate_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(287, 658);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(92, 35);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // rbECDSA
            // 
            this.rbECDSA.AutoSize = true;
            this.rbECDSA.Location = new System.Drawing.Point(22, 120);
            this.rbECDSA.Name = "rbECDSA";
            this.rbECDSA.Size = new System.Drawing.Size(85, 22);
            this.rbECDSA.TabIndex = 2;
            this.rbECDSA.Tag = "ECDSA";
            this.rbECDSA.Text = "EC-DSA";
            this.rbECDSA.UseVisualStyleBackColor = true;
            this.rbECDSA.CheckedChanged += new System.EventHandler(this.rbAlg_CheckedChanged);
            // 
            // rbRSA
            // 
            this.rbRSA.AutoSize = true;
            this.rbRSA.Checked = true;
            this.rbRSA.Location = new System.Drawing.Point(22, 92);
            this.rbRSA.Name = "rbRSA";
            this.rbRSA.Size = new System.Drawing.Size(59, 22);
            this.rbRSA.TabIndex = 1;
            this.rbRSA.TabStop = true;
            this.rbRSA.Tag = "RSA";
            this.rbRSA.Text = "RSA";
            this.rbRSA.UseVisualStyleBackColor = true;
            this.rbRSA.CheckedChanged += new System.EventHandler(this.rbAlg_CheckedChanged);
            // 
            // rbDSA
            // 
            this.rbDSA.AutoSize = true;
            this.rbDSA.Location = new System.Drawing.Point(24, 64);
            this.rbDSA.Name = "rbDSA";
            this.rbDSA.Size = new System.Drawing.Size(59, 22);
            this.rbDSA.TabIndex = 0;
            this.rbDSA.Tag = "DSA";
            this.rbDSA.Text = "DSA";
            this.rbDSA.UseVisualStyleBackColor = true;
            this.rbDSA.CheckedChanged += new System.EventHandler(this.rbAlg_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "CA folder:";
            // 
            // tbFolder
            // 
            this.tbFolder.Location = new System.Drawing.Point(110, 63);
            this.tbFolder.Name = "tbFolder";
            this.tbFolder.Size = new System.Drawing.Size(225, 24);
            this.tbFolder.TabIndex = 2;
            // 
            // butBrowse
            // 
            this.butBrowse.Location = new System.Drawing.Point(341, 60);
            this.butBrowse.Name = "butBrowse";
            this.butBrowse.Size = new System.Drawing.Size(82, 31);
            this.butBrowse.TabIndex = 3;
            this.butBrowse.Text = "Browse";
            this.butBrowse.UseVisualStyleBackColor = true;
            this.butBrowse.Click += new System.EventHandler(this.butBrowse_Click);
            // 
            // rbRoot
            // 
            this.rbRoot.AutoSize = true;
            this.rbRoot.Checked = true;
            this.rbRoot.Location = new System.Drawing.Point(12, 22);
            this.rbRoot.Name = "rbRoot";
            this.rbRoot.Size = new System.Drawing.Size(171, 22);
            this.rbRoot.TabIndex = 6;
            this.rbRoot.TabStop = true;
            this.rbRoot.Text = "Root (self-signed) CA";
            this.rbRoot.UseVisualStyleBackColor = true;
            this.rbRoot.CheckedChanged += new System.EventHandler(this.CAtype_CheckedChanged);
            // 
            // rbSubordinate
            // 
            this.rbSubordinate.AutoSize = true;
            this.rbSubordinate.Location = new System.Drawing.Point(189, 22);
            this.rbSubordinate.Name = "rbSubordinate";
            this.rbSubordinate.Size = new System.Drawing.Size(132, 22);
            this.rbSubordinate.TabIndex = 7;
            this.rbSubordinate.Text = "Subordinate CA";
            this.rbSubordinate.UseVisualStyleBackColor = true;
            this.rbSubordinate.CheckedChanged += new System.EventHandler(this.CAtype_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.cbFIPS);
            this.groupBox2.Controls.Add(this.rbECDSA);
            this.groupBox2.Controls.Add(this.rbDSA);
            this.groupBox2.Controls.Add(this.rbRSA);
            this.groupBox2.Location = new System.Drawing.Point(16, 283);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(435, 205);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cryptography";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rbSHA256);
            this.groupBox5.Controls.Add(this.rbSHA1);
            this.groupBox5.Location = new System.Drawing.Point(274, 36);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(110, 119);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Hash algorithm";
            // 
            // rbSHA256
            // 
            this.rbSHA256.AutoSize = true;
            this.rbSHA256.Location = new System.Drawing.Point(16, 81);
            this.rbSHA256.Name = "rbSHA256";
            this.rbSHA256.Size = new System.Drawing.Size(88, 22);
            this.rbSHA256.TabIndex = 1;
            this.rbSHA256.Text = "SHA-256";
            this.rbSHA256.UseVisualStyleBackColor = true;
            this.rbSHA256.CheckedChanged += new System.EventHandler(this.HashAlgo_CheckedChanged);
            // 
            // rbSHA1
            // 
            this.rbSHA1.AutoSize = true;
            this.rbSHA1.Checked = true;
            this.rbSHA1.Location = new System.Drawing.Point(16, 53);
            this.rbSHA1.Name = "rbSHA1";
            this.rbSHA1.Size = new System.Drawing.Size(72, 22);
            this.rbSHA1.TabIndex = 0;
            this.rbSHA1.TabStop = true;
            this.rbSHA1.Text = "SHA-1";
            this.rbSHA1.UseVisualStyleBackColor = true;
            this.rbSHA1.CheckedChanged += new System.EventHandler(this.HashAlgo_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rb256);
            this.groupBox4.Controls.Add(this.rb4096);
            this.groupBox4.Controls.Add(this.rb2048);
            this.groupBox4.Controls.Add(this.rb1024);
            this.groupBox4.Location = new System.Drawing.Point(143, 23);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(110, 138);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Key size";
            // 
            // rb256
            // 
            this.rb256.AutoSize = true;
            this.rb256.Location = new System.Drawing.Point(7, 110);
            this.rb256.Name = "rb256";
            this.rb256.Size = new System.Drawing.Size(97, 22);
            this.rb256.TabIndex = 3;
            this.rb256.Text = "256 bit EC";
            this.rb256.UseVisualStyleBackColor = true;
            this.rb256.CheckedChanged += new System.EventHandler(this.KeySize_CheckedChanged);
            // 
            // rb4096
            // 
            this.rb4096.AutoSize = true;
            this.rb4096.Location = new System.Drawing.Point(7, 81);
            this.rb4096.Name = "rb4096";
            this.rb4096.Size = new System.Drawing.Size(80, 22);
            this.rb4096.TabIndex = 2;
            this.rb4096.Text = "4096 bit";
            this.rb4096.UseVisualStyleBackColor = true;
            this.rb4096.CheckedChanged += new System.EventHandler(this.KeySize_CheckedChanged);
            // 
            // rb2048
            // 
            this.rb2048.AutoSize = true;
            this.rb2048.Checked = true;
            this.rb2048.Location = new System.Drawing.Point(7, 53);
            this.rb2048.Name = "rb2048";
            this.rb2048.Size = new System.Drawing.Size(80, 22);
            this.rb2048.TabIndex = 1;
            this.rb2048.TabStop = true;
            this.rb2048.Text = "2048 bit";
            this.rb2048.UseVisualStyleBackColor = true;
            this.rb2048.CheckedChanged += new System.EventHandler(this.KeySize_CheckedChanged);
            // 
            // rb1024
            // 
            this.rb1024.AutoSize = true;
            this.rb1024.Location = new System.Drawing.Point(7, 24);
            this.rb1024.Name = "rb1024";
            this.rb1024.Size = new System.Drawing.Size(80, 22);
            this.rb1024.TabIndex = 0;
            this.rb1024.Text = "1024 bit";
            this.rb1024.UseVisualStyleBackColor = true;
            this.rb1024.CheckedChanged += new System.EventHandler(this.KeySize_CheckedChanged);
            // 
            // cbFIPS
            // 
            this.cbFIPS.AutoSize = true;
            this.cbFIPS.Location = new System.Drawing.Point(163, 167);
            this.cbFIPS.Name = "cbFIPS";
            this.cbFIPS.Size = new System.Drawing.Size(104, 22);
            this.cbFIPS.TabIndex = 7;
            this.cbFIPS.Text = "FIPS mode";
            this.cbFIPS.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "CA name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 18);
            this.label5.TabIndex = 10;
            this.label5.Text = "CA DN:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(113, 111);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(310, 24);
            this.tbName.TabIndex = 11;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            this.tbName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbName_TextChanged);
            this.tbName.Leave += new System.EventHandler(this.tbName_TextChanged);
            // 
            // tbDN
            // 
            this.tbDN.Location = new System.Drawing.Point(113, 144);
            this.tbDN.Name = "tbDN";
            this.tbDN.Size = new System.Drawing.Size(310, 24);
            this.tbDN.TabIndex = 12;
            // 
            // gpCertType
            // 
            this.gpCertType.Controls.Add(this.rbV3);
            this.gpCertType.Controls.Add(this.rbV1);
            this.gpCertType.Location = new System.Drawing.Point(17, 505);
            this.gpCertType.Name = "gpCertType";
            this.gpCertType.Size = new System.Drawing.Size(141, 93);
            this.gpCertType.TabIndex = 13;
            this.gpCertType.TabStop = false;
            this.gpCertType.Text = "Certificate type";
            // 
            // rbV3
            // 
            this.rbV3.AutoSize = true;
            this.rbV3.Checked = true;
            this.rbV3.Location = new System.Drawing.Point(22, 64);
            this.rbV3.Name = "rbV3";
            this.rbV3.Size = new System.Drawing.Size(82, 22);
            this.rbV3.TabIndex = 1;
            this.rbV3.TabStop = true;
            this.rbV3.Text = "X509 v3";
            this.rbV3.UseVisualStyleBackColor = true;
            this.rbV3.CheckedChanged += new System.EventHandler(this.certType_CheckedChanged);
            // 
            // rbV1
            // 
            this.rbV1.AutoSize = true;
            this.rbV1.Location = new System.Drawing.Point(22, 36);
            this.rbV1.Name = "rbV1";
            this.rbV1.Size = new System.Drawing.Size(82, 22);
            this.rbV1.TabIndex = 0;
            this.rbV1.Text = "X509 v1";
            this.rbV1.UseVisualStyleBackColor = true;
            this.rbV1.CheckedChanged += new System.EventHandler(this.certType_CheckedChanged);
            // 
            // gpCertValidity
            // 
            this.gpCertValidity.Controls.Add(this.lbCertUnits);
            this.gpCertValidity.Controls.Add(this.label6);
            this.gpCertValidity.Controls.Add(this.tbCertValid);
            this.gpCertValidity.Location = new System.Drawing.Point(179, 505);
            this.gpCertValidity.Name = "gpCertValidity";
            this.gpCertValidity.Size = new System.Drawing.Size(272, 93);
            this.gpCertValidity.TabIndex = 14;
            this.gpCertValidity.TabStop = false;
            this.gpCertValidity.Text = "Certificate validity period";
            // 
            // lbCertUnits
            // 
            this.lbCertUnits.FormattingEnabled = true;
            this.lbCertUnits.Items.AddRange(new object[] {
            "Years",
            "Months",
            "Days"});
            this.lbCertUnits.Location = new System.Drawing.Point(111, 36);
            this.lbCertUnits.Name = "lbCertUnits";
            this.lbCertUnits.Size = new System.Drawing.Size(121, 26);
            this.lbCertUnits.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 18);
            this.label6.TabIndex = 2;
            this.label6.Text = "Period:";
            // 
            // tbCertValid
            // 
            this.tbCertValid.Location = new System.Drawing.Point(73, 36);
            this.tbCertValid.Name = "tbCertValid";
            this.tbCertValid.Size = new System.Drawing.Size(32, 24);
            this.tbCertValid.TabIndex = 0;
            this.tbCertValid.Text = "10";
            // 
            // crlPubInt
            // 
            this.crlPubInt.AutoSize = true;
            this.crlPubInt.Location = new System.Drawing.Point(38, 619);
            this.crlPubInt.Name = "crlPubInt";
            this.crlPubInt.Size = new System.Drawing.Size(161, 18);
            this.crlPubInt.TabIndex = 15;
            this.crlPubInt.Text = "CRL publishing interval:";
            // 
            // tbCRLInterval
            // 
            this.tbCRLInterval.Location = new System.Drawing.Point(205, 619);
            this.tbCRLInterval.Name = "tbCRLInterval";
            this.tbCRLInterval.Size = new System.Drawing.Size(32, 24);
            this.tbCRLInterval.TabIndex = 16;
            this.tbCRLInterval.Text = "1";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyDocuments;
            // 
            // lblIssCA
            // 
            this.lblIssCA.AutoSize = true;
            this.lblIssCA.Location = new System.Drawing.Point(55, 27);
            this.lblIssCA.Name = "lblIssCA";
            this.lblIssCA.Size = new System.Drawing.Size(32, 18);
            this.lblIssCA.TabIndex = 18;
            this.lblIssCA.Text = "CA:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(243, 622);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 18);
            this.label2.TabIndex = 20;
            this.label2.Text = "Days";
            // 
            // gpIssuingCA
            // 
            this.gpIssuingCA.Controls.Add(this.lbProfile);
            this.gpIssuingCA.Controls.Add(this.lbIssuingCA);
            this.gpIssuingCA.Controls.Add(this.label3);
            this.gpIssuingCA.Controls.Add(this.lblIssCA);
            this.gpIssuingCA.Enabled = false;
            this.gpIssuingCA.Location = new System.Drawing.Point(17, 187);
            this.gpIssuingCA.Name = "gpIssuingCA";
            this.gpIssuingCA.Size = new System.Drawing.Size(435, 90);
            this.gpIssuingCA.TabIndex = 21;
            this.gpIssuingCA.TabStop = false;
            this.gpIssuingCA.Text = "Issuing CA";
            // 
            // lbProfile
            // 
            this.lbProfile.FormattingEnabled = true;
            this.lbProfile.Location = new System.Drawing.Point(93, 54);
            this.lbProfile.Name = "lbProfile";
            this.lbProfile.Size = new System.Drawing.Size(312, 26);
            this.lbProfile.TabIndex = 23;
            // 
            // lbIssuingCA
            // 
            this.lbIssuingCA.FormattingEnabled = true;
            this.lbIssuingCA.Location = new System.Drawing.Point(93, 23);
            this.lbIssuingCA.Name = "lbIssuingCA";
            this.lbIssuingCA.Size = new System.Drawing.Size(310, 26);
            this.lbIssuingCA.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 18);
            this.label3.TabIndex = 20;
            this.label3.Text = "Profile:";
            // 
            // rbTA
            // 
            this.rbTA.AutoSize = true;
            this.rbTA.Location = new System.Drawing.Point(327, 22);
            this.rbTA.Name = "rbTA";
            this.rbTA.Size = new System.Drawing.Size(132, 22);
            this.rbTA.TabIndex = 22;
            this.rbTA.TabStop = true;
            this.rbTA.Text = "TA (self-signed)";
            this.rbTA.UseVisualStyleBackColor = true;
            this.rbTA.CheckedChanged += new System.EventHandler(this.CAtype_CheckedChanged);
            // 
            // CreateCA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(459, 705);
            this.Controls.Add(this.rbTA);
            this.Controls.Add(this.gpIssuingCA);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbCRLInterval);
            this.Controls.Add(this.crlPubInt);
            this.Controls.Add(this.gpCertValidity);
            this.Controls.Add(this.gpCertType);
            this.Controls.Add(this.tbDN);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.rbSubordinate);
            this.Controls.Add(this.rbRoot);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butBrowse);
            this.Controls.Add(this.butCreate);
            this.Controls.Add(this.tbFolder);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateCA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create new CA";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.gpCertType.ResumeLayout(false);
            this.gpCertType.PerformLayout();
            this.gpCertValidity.ResumeLayout(false);
            this.gpCertValidity.PerformLayout();
            this.gpIssuingCA.ResumeLayout(false);
            this.gpIssuingCA.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butCreate;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butBrowse;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbECDSA;
        private System.Windows.Forms.RadioButton rbRSA;
        private System.Windows.Forms.RadioButton rbDSA;
        private System.Windows.Forms.RadioButton rbRoot;
        private System.Windows.Forms.RadioButton rbSubordinate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbFIPS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbDN;
        private System.Windows.Forms.GroupBox gpCertType;
        private System.Windows.Forms.RadioButton rbV3;
        private System.Windows.Forms.RadioButton rbV1;
        private System.Windows.Forms.GroupBox gpCertValidity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbCertValid;
        private System.Windows.Forms.Label crlPubInt;
        private System.Windows.Forms.TextBox tbCRLInterval;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rb256;
        private System.Windows.Forms.RadioButton rb4096;
        private System.Windows.Forms.RadioButton rb2048;
        private System.Windows.Forms.RadioButton rb1024;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rbSHA256;
        private System.Windows.Forms.RadioButton rbSHA1;
        private System.Windows.Forms.Label lblIssCA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gpIssuingCA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox lbCertUnits;
        private System.Windows.Forms.ComboBox lbIssuingCA;
        private System.Windows.Forms.ComboBox lbProfile;
        private System.Windows.Forms.RadioButton rbTA;
    }
}