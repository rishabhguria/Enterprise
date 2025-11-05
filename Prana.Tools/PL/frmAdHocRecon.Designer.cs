namespace Prana.Tools
{
    partial class frmAdHocRecon
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
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ctrlAdHocRecon1 = new Prana.Tools.ctrlAdHocRecon();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ctrlAdHocRecon1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(977, 412);
            this.ultraPanel1.TabIndex = 0;
            // 
            // ctrlAdHocRecon1
            // 
            this.ctrlAdHocRecon1.AllowDrop = true;
            this.ctrlAdHocRecon1.BackColor = System.Drawing.Color.Transparent;
            this.ctrlAdHocRecon1.Location = new System.Drawing.Point(96, 12);
            this.ctrlAdHocRecon1.Name = "ctrlAdHocRecon1";
            this.ctrlAdHocRecon1.Size = new System.Drawing.Size(905, 441);
            this.ctrlAdHocRecon1.TabIndex = 0;
            this.ctrlAdHocRecon1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // frmAdHocRecon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.ClientSize = new System.Drawing.Size(977, 412);
            this.Controls.Add(this.ultraPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "frmAdHocRecon";
            this.Text = "Ad-Hoc Recon";
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private ctrlAdHocRecon ctrlAdHocRecon1;

    }
}