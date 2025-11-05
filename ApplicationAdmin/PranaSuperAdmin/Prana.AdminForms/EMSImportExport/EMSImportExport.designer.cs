namespace Prana.Admin
{
    partial class EMSImportExport
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
            this.ctrlSaveImportXSLT = new Prana.Admin.Controls.ctrlSaveImportXSLT();
            this.SuspendLayout();
            // 
            // ctrlSaveImportXSLT
            // 
            this.ctrlSaveImportXSLT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlSaveImportXSLT.Location = new System.Drawing.Point(12, 12);
            this.ctrlSaveImportXSLT.Name = "ctrlSaveImportXSLT";
            this.ctrlSaveImportXSLT.Size = new System.Drawing.Size(521, 252);
            this.ctrlSaveImportXSLT.TabIndex = 0;
            // 
            // EMSImportExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 270);
            this.Controls.Add(this.ctrlSaveImportXSLT);
            this.Name = "EMSImportExport";
            this.Text = "EMS Import XSLT Setup";
            this.Load += new System.EventHandler(this.EMSImportExport_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Prana.Admin.Controls.ctrlSaveImportXSLT ctrlSaveImportXSLT;

    }
}

