namespace OSCASnapin.Profiles
{
    partial class BasicConstraints
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BasicConstraints));
            this.butCreate = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.cbCA = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPathLen = new System.Windows.Forms.TextBox();
            this.cbCritical = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // butCreate
            // 
            this.butCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butCreate.Location = new System.Drawing.Point(80, 117);
            this.butCreate.Name = "butCreate";
            this.butCreate.Size = new System.Drawing.Size(82, 31);
            this.butCreate.TabIndex = 0;
            this.butCreate.Text = "OK";
            this.butCreate.UseVisualStyleBackColor = true;
            this.butCreate.Click += new System.EventHandler(this.butCreate_Click);
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
            // cbCA
            // 
            this.cbCA.AutoSize = true;
            this.cbCA.Checked = true;
            this.cbCA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCA.Location = new System.Drawing.Point(112, 12);
            this.cbCA.Name = "cbCA";
            this.cbCA.Size = new System.Drawing.Size(50, 22);
            this.cbCA.TabIndex = 2;
            this.cbCA.Text = "CA";
            this.cbCA.UseVisualStyleBackColor = true;
            this.cbCA.CheckedChanged += new System.EventHandler(this.ca_Changed);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Path length constraint:";
            // 
            // tbPathLen
            // 
            this.tbPathLen.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.tbPathLen.Location = new System.Drawing.Point(269, 40);
            this.tbPathLen.Name = "tbPathLen";
            this.tbPathLen.Size = new System.Drawing.Size(60, 24);
            this.tbPathLen.TabIndex = 4;
            this.tbPathLen.Text = "none";
            // 
            // cbCritical
            // 
            this.cbCritical.AutoSize = true;
            this.cbCritical.Checked = true;
            this.cbCritical.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCritical.Location = new System.Drawing.Point(154, 77);
            this.cbCritical.Name = "cbCritical";
            this.cbCritical.Size = new System.Drawing.Size(75, 22);
            this.cbCritical.TabIndex = 5;
            this.cbCritical.Text = "Critical";
            this.cbCritical.UseVisualStyleBackColor = true;
            this.cbCritical.CheckedChanged += new System.EventHandler(this.cbCritical_CheckedChanged);
            // 
            // BasicConstraints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 160);
            this.Controls.Add(this.cbCritical);
            this.Controls.Add(this.tbPathLen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCA);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butCreate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BasicConstraints";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Basic Constraints";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butCreate;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox cbCA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPathLen;
        private System.Windows.Forms.CheckBox cbCritical;
    }
}