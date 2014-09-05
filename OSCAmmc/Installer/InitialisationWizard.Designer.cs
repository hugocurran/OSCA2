namespace OSCASnapin
{
    partial class InitialisationWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitialisationWizard));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbConfigFolder = new System.Windows.Forms.TextBox();
            this.butBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.butInstall = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.cbOverwrite = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbAdminPassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(316, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a folder to store the OSCA configuration";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Folder:";
            // 
            // tbConfigFolder
            // 
            this.tbConfigFolder.Location = new System.Drawing.Point(87, 56);
            this.tbConfigFolder.Name = "tbConfigFolder";
            this.tbConfigFolder.Size = new System.Drawing.Size(252, 24);
            this.tbConfigFolder.TabIndex = 2;
            // 
            // butBrowse
            // 
            this.butBrowse.Location = new System.Drawing.Point(345, 53);
            this.butBrowse.Name = "butBrowse";
            this.butBrowse.Size = new System.Drawing.Size(82, 31);
            this.butBrowse.TabIndex = 3;
            this.butBrowse.Text = "Browse";
            this.butBrowse.UseVisualStyleBackColor = true;
            this.butBrowse.Click += new System.EventHandler(this.butBrowse_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(94, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(264, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Cancel button will abort the initialisation";
            // 
            // butInstall
            // 
            this.butInstall.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butInstall.Location = new System.Drawing.Point(97, 242);
            this.butInstall.Name = "butInstall";
            this.butInstall.Size = new System.Drawing.Size(82, 31);
            this.butInstall.TabIndex = 5;
            this.butInstall.Text = "Install";
            this.butInstall.UseVisualStyleBackColor = true;
            this.butInstall.Click += new System.EventHandler(this.butInstall_Click);
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Location = new System.Drawing.Point(276, 242);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 31);
            this.button4.TabIndex = 7;
            this.button4.Text = "Cancel";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.Personal;
            // 
            // cbOverwrite
            // 
            this.cbOverwrite.AutoSize = true;
            this.cbOverwrite.Enabled = false;
            this.cbOverwrite.Location = new System.Drawing.Point(87, 96);
            this.cbOverwrite.Name = "cbOverwrite";
            this.cbOverwrite.Size = new System.Drawing.Size(190, 22);
            this.cbOverwrite.TabIndex = 8;
            this.cbOverwrite.Text = "Overwrite existing config";
            this.cbOverwrite.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "Admin password:";
            // 
            // tbAdminPassword
            // 
            this.tbAdminPassword.Location = new System.Drawing.Point(158, 145);
            this.tbAdminPassword.Name = "tbAdminPassword";
            this.tbAdminPassword.Size = new System.Drawing.Size(181, 24);
            this.tbAdminPassword.TabIndex = 10;
            this.tbAdminPassword.UseSystemPasswordChar = true;
            // 
            // InitialisationWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 292);
            this.Controls.Add(this.tbAdminPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbOverwrite);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.butInstall);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.butBrowse);
            this.Controls.Add(this.tbConfigFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitialisationWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OSCA initialisation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbConfigFolder;
        private System.Windows.Forms.Button butBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button butInstall;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox cbOverwrite;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbAdminPassword;
    }
}