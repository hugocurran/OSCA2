namespace OSCASnapin
{
    partial class AddCA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCA));
            this.label1 = new System.Windows.Forms.Label();
            this.tbConfigFile = new System.Windows.Forms.TextBox();
            this.butBrowse = new System.Windows.Forms.Button();
            this.butAdd = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cbImport = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "CA config file:";
            // 
            // tbConfigFile
            // 
            this.tbConfigFile.Location = new System.Drawing.Point(116, 22);
            this.tbConfigFile.Name = "tbConfigFile";
            this.tbConfigFile.Size = new System.Drawing.Size(325, 24);
            this.tbConfigFile.TabIndex = 1;
            // 
            // butBrowse
            // 
            this.butBrowse.Location = new System.Drawing.Point(447, 17);
            this.butBrowse.Name = "butBrowse";
            this.butBrowse.Size = new System.Drawing.Size(92, 35);
            this.butBrowse.TabIndex = 2;
            this.butBrowse.Text = "Browse";
            this.butBrowse.UseVisualStyleBackColor = true;
            this.butBrowse.Click += new System.EventHandler(this.butBrowse_Click);
            // 
            // butAdd
            // 
            this.butAdd.Location = new System.Drawing.Point(146, 119);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(92, 35);
            this.butAdd.TabIndex = 3;
            this.butAdd.Text = "Add";
            this.butAdd.UseVisualStyleBackColor = true;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(316, 119);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 35);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "CAConfig.xml";
            this.openFileDialog1.Filter = ".xml files|*.xml";
            this.openFileDialog1.Title = "OSCA - Add existing CA";
            // 
            // cbImport
            // 
            this.cbImport.AutoSize = true;
            this.cbImport.Location = new System.Drawing.Point(116, 70);
            this.cbImport.Name = "cbImport";
            this.cbImport.Size = new System.Drawing.Size(298, 22);
            this.cbImport.TabIndex = 5;
            this.cbImport.Text = "Import key material (FIPS Mode CA only)";
            this.cbImport.UseVisualStyleBackColor = true;
            this.cbImport.CheckedChanged += new System.EventHandler(this.cbImport_CheckedChanged);
            // 
            // AddCA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 175);
            this.Controls.Add(this.cbImport);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.butBrowse);
            this.Controls.Add(this.tbConfigFile);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OSCA - Add existing CA";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbConfigFile;
        private System.Windows.Forms.Button butBrowse;
        private System.Windows.Forms.Button butAdd;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox cbImport;
    }
}