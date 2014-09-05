namespace OSCASnapin
{
    partial class OSCAPropertiesControl
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.clbAdminPriv = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clbUserPriv = new System.Windows.Forms.CheckedListBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.clbAdminPriv);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(248, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(225, 443);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Admin privileges";
            // 
            // clbAdminPriv
            // 
            this.clbAdminPriv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbAdminPriv.FormattingEnabled = true;
            this.clbAdminPriv.Location = new System.Drawing.Point(3, 18);
            this.clbAdminPriv.Name = "clbAdminPriv";
            this.clbAdminPriv.Size = new System.Drawing.Size(219, 422);
            this.clbAdminPriv.TabIndex = 0;
            this.clbAdminPriv.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ItemCheck);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clbUserPriv);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 443);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User privileges";
            // 
            // clbUserPriv
            // 
            this.clbUserPriv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbUserPriv.FormattingEnabled = true;
            this.clbUserPriv.Location = new System.Drawing.Point(3, 18);
            this.clbUserPriv.Name = "clbUserPriv";
            this.clbUserPriv.Size = new System.Drawing.Size(233, 422);
            this.clbUserPriv.TabIndex = 1;
            this.clbUserPriv.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ItemCheck);
            // 
            // OSCAPropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "OSCAPropertiesControl";
            this.Size = new System.Drawing.Size(473, 443);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox clbAdminPriv;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox clbUserPriv;
    }
}
