namespace Prana.Utilities.UI
{
    partial class FileOpenDialogue
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileOpenDialogue));
            this.grpFileFormat = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblFileName = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grpFileFormat)).BeginInit();
            this.grpFileFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpFileFormat
            // 
            this.grpFileFormat.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rectangular3D;
            this.grpFileFormat.CaptionAlignment = Infragistics.Win.Misc.GroupBoxCaptionAlignment.Center;
            this.grpFileFormat.Controls.Add(this.btnClose);
            this.grpFileFormat.Controls.Add(this.btnView);
            this.grpFileFormat.Controls.Add(this.lblFileName);
            this.grpFileFormat.Location = new System.Drawing.Point(2, 2);
            this.grpFileFormat.Name = "grpFileFormat";
            this.grpFileFormat.Size = new System.Drawing.Size(455, 82);
            this.grpFileFormat.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(212, 48);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 64;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnView.Location = new System.Drawing.Point(131, 48);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 63;
            this.btnView.Text = "&View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblFileName
            // 
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.lblFileName.Appearance = appearance1;
            this.lblFileName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileName.Location = new System.Drawing.Point(6, 9);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(441, 33);
            this.lblFileName.TabIndex = 7;
            // 
            // ThirdPartyFlatFileOpenDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 83);
            this.Controls.Add(this.grpFileFormat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ThirdPartyFlatFileOpenDialogue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Open Dialogue";
            ((System.ComponentModel.ISupportInitialize)(this.grpFileFormat)).EndInit();
            this.grpFileFormat.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox grpFileFormat;
        private Infragistics.Win.Misc.UltraLabel lblFileName;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnView;
    }
}