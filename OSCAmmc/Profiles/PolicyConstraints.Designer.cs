namespace OSCASnapin.Profiles
{
    partial class PolicyConstraints
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
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCritical = new System.Windows.Forms.CheckBox();
            this.tbInPolMapSkip = new System.Windows.Forms.TextBox();
            this.cbInhibPolMap = new System.Windows.Forms.CheckBox();
            this.cbReqExpPol = new System.Windows.Forms.CheckBox();
            this.tbReqExpPolSkip = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOK.Location = new System.Drawing.Point(76, 142);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(82, 31);
            this.butOK.TabIndex = 0;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(278, 142);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(82, 31);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Skip certificates:";
            // 
            // cbCritical
            // 
            this.cbCritical.AutoSize = true;
            this.cbCritical.Location = new System.Drawing.Point(152, 102);
            this.cbCritical.Name = "cbCritical";
            this.cbCritical.Size = new System.Drawing.Size(75, 22);
            this.cbCritical.TabIndex = 5;
            this.cbCritical.Text = "Critical";
            this.cbCritical.UseVisualStyleBackColor = true;
            this.cbCritical.CheckedChanged += new System.EventHandler(this.cbCritical_CheckedChanged);
            // 
            // tbInPolMapSkip
            // 
            this.tbInPolMapSkip.Location = new System.Drawing.Point(366, 59);
            this.tbInPolMapSkip.Name = "tbInPolMapSkip";
            this.tbInPolMapSkip.Size = new System.Drawing.Size(28, 24);
            this.tbInPolMapSkip.TabIndex = 6;
            this.tbInPolMapSkip.Text = "0";
            this.tbInPolMapSkip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbInPolMapSkip.WordWrap = false;
            // 
            // cbInhibPolMap
            // 
            this.cbInhibPolMap.AutoSize = true;
            this.cbInhibPolMap.Location = new System.Drawing.Point(56, 64);
            this.cbInhibPolMap.Name = "cbInhibPolMap";
            this.cbInhibPolMap.Size = new System.Drawing.Size(171, 22);
            this.cbInhibPolMap.TabIndex = 7;
            this.cbInhibPolMap.Text = "Inhibit Policy Mapping";
            this.cbInhibPolMap.UseVisualStyleBackColor = true;
            // 
            // cbReqExpPol
            // 
            this.cbReqExpPol.AutoSize = true;
            this.cbReqExpPol.Location = new System.Drawing.Point(56, 28);
            this.cbReqExpPol.Name = "cbReqExpPol";
            this.cbReqExpPol.Size = new System.Drawing.Size(175, 22);
            this.cbReqExpPol.TabIndex = 8;
            this.cbReqExpPol.Text = "Require Explicit Policy";
            this.cbReqExpPol.UseVisualStyleBackColor = true;
            // 
            // tbReqExpPolSkip
            // 
            this.tbReqExpPolSkip.Location = new System.Drawing.Point(366, 28);
            this.tbReqExpPolSkip.Name = "tbReqExpPolSkip";
            this.tbReqExpPolSkip.Size = new System.Drawing.Size(28, 24);
            this.tbReqExpPolSkip.TabIndex = 9;
            this.tbReqExpPolSkip.Text = "0";
            this.tbReqExpPolSkip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "Skip certificates:";
            // 
            // PolicyConstraints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 198);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbReqExpPolSkip);
            this.Controls.Add(this.cbReqExpPol);
            this.Controls.Add(this.cbInhibPolMap);
            this.Controls.Add(this.tbInPolMapSkip);
            this.Controls.Add(this.cbCritical);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PolicyConstraints";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Policy Constraints";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbCritical;
        private System.Windows.Forms.TextBox tbInPolMapSkip;
        private System.Windows.Forms.CheckBox cbInhibPolMap;
        private System.Windows.Forms.CheckBox cbReqExpPol;
        private System.Windows.Forms.TextBox tbReqExpPolSkip;
        private System.Windows.Forms.Label label2;
    }
}