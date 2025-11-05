namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    partial class BulkChangesForm
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
            this.bulkChangeControl1 = new Prana.AllocationNew.Allocation.UI.UserControls.BulkChangeControl();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bulkChangeControl1
            // 
            this.bulkChangeControl1.AutoScroll = true;
            this.bulkChangeControl1.AutoSize = true;
            this.bulkChangeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bulkChangeControl1.Location = new System.Drawing.Point(0, 0);
            this.bulkChangeControl1.Name = "bulkChangeControl1";
            this.bulkChangeControl1.Size = new System.Drawing.Size(365, 384);
            this.bulkChangeControl1.TabIndex = 0;
            this.bulkChangeControl1.Load += new System.EventHandler(this.bulkChangeControl1_Load);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.bulkChangeControl1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(365, 384);
            this.ultraPanel1.TabIndex = 1;
            // 
            // BulkChangesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 384);
            this.Controls.Add(this.ultraPanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(381, 422);
            this.MinimumSize = new System.Drawing.Size(381, 422);
            this.Name = "BulkChangesForm";
            this.Text = "BulkChangesForm";
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal BulkChangeControl bulkChangeControl1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;

    }
}