namespace OSCASnapin.Profiles
{
    partial class CreateProfile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateProfile));
            this.butCreate = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.lbMaster = new System.Windows.Forms.ListBox();
            this.lbProfile = new System.Windows.Forms.ListBox();
            this.butAdd = new System.Windows.Forms.Button();
            this.butDelete = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbLifetime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbVersion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.cbLifetimeUnits = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbOverlap = new System.Windows.Forms.TextBox();
            this.cbOverlapUnits = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // butCreate
            // 
            this.butCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butCreate.Location = new System.Drawing.Point(183, 431);
            this.butCreate.Name = "butCreate";
            this.butCreate.Size = new System.Drawing.Size(82, 31);
            this.butCreate.TabIndex = 0;
            this.butCreate.Text = "Create";
            this.butCreate.UseVisualStyleBackColor = true;
            this.butCreate.Click += new System.EventHandler(this.butCreate_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(436, 431);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(82, 31);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // lbMaster
            // 
            this.lbMaster.FormattingEnabled = true;
            this.lbMaster.ItemHeight = 18;
            this.lbMaster.Location = new System.Drawing.Point(15, 124);
            this.lbMaster.Name = "lbMaster";
            this.lbMaster.Size = new System.Drawing.Size(250, 274);
            this.lbMaster.TabIndex = 2;
            // 
            // lbProfile
            // 
            this.lbProfile.FormattingEnabled = true;
            this.lbProfile.ItemHeight = 18;
            this.lbProfile.Location = new System.Drawing.Point(436, 124);
            this.lbProfile.Name = "lbProfile";
            this.lbProfile.Size = new System.Drawing.Size(250, 274);
            this.lbProfile.TabIndex = 3;
            this.lbProfile.DoubleClick += new System.EventHandler(this.extension_DoubleClick);
            // 
            // butAdd
            // 
            this.butAdd.Location = new System.Drawing.Point(299, 207);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(90, 31);
            this.butAdd.TabIndex = 4;
            this.butAdd.Text = "Add ->";
            this.butAdd.UseVisualStyleBackColor = true;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // butDelete
            // 
            this.butDelete.Location = new System.Drawing.Point(299, 292);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(90, 31);
            this.butDelete.TabIndex = 5;
            this.butDelete.Text = "<- Remove";
            this.butDelete.UseVisualStyleBackColor = true;
            this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Profile name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "Certificate lifetime:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(113, 17);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(224, 24);
            this.tbName.TabIndex = 8;
            // 
            // tbLifetime
            // 
            this.tbLifetime.Location = new System.Drawing.Point(147, 47);
            this.tbLifetime.Name = "tbLifetime";
            this.tbLifetime.Size = new System.Drawing.Size(34, 24);
            this.tbLifetime.TabIndex = 9;
            this.tbLifetime.Text = "1";
            this.tbLifetime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(369, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 18);
            this.label3.TabIndex = 11;
            this.label3.Text = "Version:";
            // 
            // tbVersion
            // 
            this.tbVersion.Location = new System.Drawing.Point(438, 18);
            this.tbVersion.Name = "tbVersion";
            this.tbVersion.Size = new System.Drawing.Size(57, 24);
            this.tbVersion.TabIndex = 12;
            this.tbVersion.Text = "1.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "Description:";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(96, 87);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(581, 24);
            this.tbDescription.TabIndex = 14;
            // 
            // cbLifetimeUnits
            // 
            this.cbLifetimeUnits.FormattingEnabled = true;
            this.cbLifetimeUnits.Items.AddRange(new object[] {
            "Years",
            "Months",
            "Days",
            "Hours"});
            this.cbLifetimeUnits.Location = new System.Drawing.Point(187, 47);
            this.cbLifetimeUnits.Name = "cbLifetimeUnits";
            this.cbLifetimeUnits.Size = new System.Drawing.Size(99, 26);
            this.cbLifetimeUnits.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(348, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 18);
            this.label5.TabIndex = 16;
            this.label5.Text = "Renewal overlap:";
            // 
            // tbOverlap
            // 
            this.tbOverlap.Location = new System.Drawing.Point(475, 49);
            this.tbOverlap.Name = "tbOverlap";
            this.tbOverlap.Size = new System.Drawing.Size(34, 24);
            this.tbOverlap.TabIndex = 17;
            this.tbOverlap.Text = "0";
            // 
            // cbOverlapUnits
            // 
            this.cbOverlapUnits.FormattingEnabled = true;
            this.cbOverlapUnits.Items.AddRange(new object[] {
            "Years",
            "Months",
            "Days",
            "Hours"});
            this.cbOverlapUnits.Location = new System.Drawing.Point(515, 47);
            this.cbOverlapUnits.Name = "cbOverlapUnits";
            this.cbOverlapUnits.Size = new System.Drawing.Size(99, 26);
            this.cbOverlapUnits.TabIndex = 18;
            // 
            // CreateProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 476);
            this.Controls.Add(this.cbOverlapUnits);
            this.Controls.Add(this.tbOverlap);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbLifetimeUnits);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbLifetime);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butDelete);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.lbProfile);
            this.Controls.Add(this.lbMaster);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butCreate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CreateProfile";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butCreate;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.ListBox lbMaster;
        private System.Windows.Forms.ListBox lbProfile;
        private System.Windows.Forms.Button butAdd;
        private System.Windows.Forms.Button butDelete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbLifetime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.ComboBox cbLifetimeUnits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbOverlap;
        private System.Windows.Forms.ComboBox cbOverlapUnits;
    }
}