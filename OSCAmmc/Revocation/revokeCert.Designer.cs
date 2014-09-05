namespace OSCASnapin
{
    partial class revokeCert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(revokeCert));
            this.butRevoke = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbReason = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labSubject = new System.Windows.Forms.Label();
            this.labIssueDate = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // butRevoke
            // 
            this.butRevoke.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butRevoke.Location = new System.Drawing.Point(115, 150);
            this.butRevoke.Name = "butRevoke";
            this.butRevoke.Size = new System.Drawing.Size(82, 31);
            this.butRevoke.TabIndex = 0;
            this.butRevoke.Text = "Revoke";
            this.butRevoke.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(281, 150);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 31);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Revocation reason:";
            // 
            // cbReason
            // 
            this.cbReason.FormattingEnabled = true;
            this.cbReason.Items.AddRange(new object[] {
            "Unknown",
            "Key Compromise",
            "CA Compromised",
            "Affiliation Changed",
            "Superseded",
            "Cessation of Operation"});
            this.cbReason.Location = new System.Drawing.Point(183, 106);
            this.cbReason.Name = "cbReason";
            this.cbReason.Size = new System.Drawing.Size(212, 26);
            this.cbReason.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labIssueDate);
            this.groupBox1.Controls.Add(this.labSubject);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 87);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Certificate";
            // 
            // labSubject
            // 
            this.labSubject.AutoSize = true;
            this.labSubject.Location = new System.Drawing.Point(7, 24);
            this.labSubject.Name = "labSubject";
            this.labSubject.Size = new System.Drawing.Size(76, 18);
            this.labSubject.TabIndex = 0;
            this.labSubject.Text = "labSubject";
            // 
            // labIssueDate
            // 
            this.labIssueDate.AutoSize = true;
            this.labIssueDate.Location = new System.Drawing.Point(7, 53);
            this.labIssueDate.Name = "labIssueDate";
            this.labIssueDate.Size = new System.Drawing.Size(93, 18);
            this.labIssueDate.TabIndex = 1;
            this.labIssueDate.Text = "labIssueDate";
            // 
            // revokeCert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(488, 209);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbReason);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.butRevoke);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "revokeCert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Revoke a certificate";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butRevoke;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ComboBox cbReason;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labIssueDate;
        private System.Windows.Forms.Label labSubject;
    }
}