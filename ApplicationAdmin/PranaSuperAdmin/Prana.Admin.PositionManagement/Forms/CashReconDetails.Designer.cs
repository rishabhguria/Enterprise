namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class CashReconDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CashReconDetails));
            this.ctrlCashReconDetails1 = new Nirvana.Admin.PositionManagement.Controls.CtrlCashReconDetails();
            this.SuspendLayout();
            // 
            // ctrlCashReconDetails1
            // 
            this.ctrlCashReconDetails1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlCashReconDetails1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCashReconDetails1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlCashReconDetails1.IsInitialized = false;
            this.ctrlCashReconDetails1.Location = new System.Drawing.Point(0, 0);
            this.ctrlCashReconDetails1.Name = "ctrlCashReconDetails1";
            this.ctrlCashReconDetails1.Size = new System.Drawing.Size(743, 591);
            this.ctrlCashReconDetails1.TabIndex = 0;
            this.ctrlCashReconDetails1.Load += new System.EventHandler(this.CashReconDetails_Load);
            // 
            // CashReconDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(743, 591);
            this.Controls.Add(this.ctrlCashReconDetails1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CashReconDetails";
            this.Text = "Cash Reconciliation Details";
            this.Load += new System.EventHandler(this.CashReconDetails_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Nirvana.Admin.PositionManagement.Controls.CtrlCashReconDetails ctrlCashReconDetails1;
    }
}