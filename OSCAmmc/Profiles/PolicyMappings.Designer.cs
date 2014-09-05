namespace OSCASnapin.Profiles
{
    partial class PolicyMappings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PolicyMappings));
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.lbIssuerPolicies = new System.Windows.Forms.ListBox();
            this.lbMappings = new System.Windows.Forms.ListBox();
            this.butMap = new System.Windows.Forms.Button();
            this.butUnmap = new System.Windows.Forms.Button();
            this.cbCritical = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOK.Location = new System.Drawing.Point(276, 409);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(82, 31);
            this.butOK.TabIndex = 0;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(415, 409);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(82, 31);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // lbIssuerPolicies
            // 
            this.lbIssuerPolicies.FormattingEnabled = true;
            this.lbIssuerPolicies.ItemHeight = 18;
            this.lbIssuerPolicies.Location = new System.Drawing.Point(15, 39);
            this.lbIssuerPolicies.Name = "lbIssuerPolicies";
            this.lbIssuerPolicies.Size = new System.Drawing.Size(277, 310);
            this.lbIssuerPolicies.TabIndex = 2;
            // 
            // lbMappings
            // 
            this.lbMappings.FormattingEnabled = true;
            this.lbMappings.ItemHeight = 18;
            this.lbMappings.Location = new System.Drawing.Point(465, 39);
            this.lbMappings.Name = "lbMappings";
            this.lbMappings.Size = new System.Drawing.Size(277, 310);
            this.lbMappings.TabIndex = 3;
            // 
            // butMap
            // 
            this.butMap.Location = new System.Drawing.Point(340, 137);
            this.butMap.Name = "butMap";
            this.butMap.Size = new System.Drawing.Size(82, 31);
            this.butMap.TabIndex = 4;
            this.butMap.Text = "Map ->";
            this.butMap.UseVisualStyleBackColor = true;
            this.butMap.Click += new System.EventHandler(this.butMap_Click);
            // 
            // butUnmap
            // 
            this.butUnmap.Location = new System.Drawing.Point(340, 197);
            this.butUnmap.Name = "butUnmap";
            this.butUnmap.Size = new System.Drawing.Size(82, 31);
            this.butUnmap.TabIndex = 5;
            this.butUnmap.Text = "<-Unmap";
            this.butUnmap.UseVisualStyleBackColor = true;
            this.butUnmap.Click += new System.EventHandler(this.butUnmap_Click);
            // 
            // cbCritical
            // 
            this.cbCritical.AutoSize = true;
            this.cbCritical.Location = new System.Drawing.Point(356, 362);
            this.cbCritical.Name = "cbCritical";
            this.cbCritical.Size = new System.Drawing.Size(75, 22);
            this.cbCritical.TabIndex = 6;
            this.cbCritical.Text = "Critical";
            this.cbCritical.UseVisualStyleBackColor = true;
            this.cbCritical.CheckedChanged += new System.EventHandler(this.cbCritical_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Issuer Policies:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(462, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "Mappings:";
            // 
            // PolicyMappings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 451);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCritical);
            this.Controls.Add(this.butUnmap);
            this.Controls.Add(this.butMap);
            this.Controls.Add(this.lbMappings);
            this.Controls.Add(this.lbIssuerPolicies);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PolicyMappings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Policy Mappings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.ListBox lbIssuerPolicies;
        private System.Windows.Forms.ListBox lbMappings;
        private System.Windows.Forms.Button butMap;
        private System.Windows.Forms.Button butUnmap;
        private System.Windows.Forms.CheckBox cbCritical;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}