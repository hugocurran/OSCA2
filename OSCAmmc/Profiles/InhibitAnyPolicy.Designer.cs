namespace OSCASnapin.Profiles
{
    partial class InhibitAnyPolicy
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
            this.tbSkipCerts = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOK.Location = new System.Drawing.Point(80, 117);
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
            this.butCancel.Location = new System.Drawing.Point(232, 117);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(82, 31);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Skip certificates:";
            // 
            // cbCritical
            // 
            this.cbCritical.AutoSize = true;
            this.cbCritical.Location = new System.Drawing.Point(152, 66);
            this.cbCritical.Name = "cbCritical";
            this.cbCritical.Size = new System.Drawing.Size(75, 22);
            this.cbCritical.TabIndex = 5;
            this.cbCritical.Text = "Critical";
            this.cbCritical.UseVisualStyleBackColor = true;
            this.cbCritical.CheckedChanged += new System.EventHandler(this.cbCritical_CheckedChanged);
            // 
            // tbSkipCerts
            // 
            this.tbSkipCerts.Location = new System.Drawing.Point(199, 25);
            this.tbSkipCerts.Name = "tbSkipCerts";
            this.tbSkipCerts.Size = new System.Drawing.Size(28, 24);
            this.tbSkipCerts.TabIndex = 6;
            this.tbSkipCerts.Text = "0";
            this.tbSkipCerts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSkipCerts.WordWrap = false;
            // 
            // InhibitAnyPolicy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 160);
            this.Controls.Add(this.tbSkipCerts);
            this.Controls.Add(this.cbCritical);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InhibitAnyPolicy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inhibit AnyPolicy";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbCritical;
        private System.Windows.Forms.TextBox tbSkipCerts;
    }
}