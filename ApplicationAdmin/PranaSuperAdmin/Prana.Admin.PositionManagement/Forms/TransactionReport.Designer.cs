namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class TransactionReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransactionReport));
            this.ctrlTransactionReport1 = new Nirvana.Admin.PositionManagement.Controls.CtrlTransactionReport();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // ctrlTransactionReport1
            // 
            this.ctrlTransactionReport1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlTransactionReport1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlTransactionReport1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlTransactionReport1.IsInitialized = false;
            this.ctrlTransactionReport1.Location = new System.Drawing.Point(0, 0);
            this.ctrlTransactionReport1.Name = "ctrlTransactionReport1";
            this.ctrlTransactionReport1.Size = new System.Drawing.Size(723, 563);
            this.ctrlTransactionReport1.TabIndex = 0;
            this.ctrlTransactionReport1.Load += new System.EventHandler(this.ctrlTransactionReport1_Load);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.ultraButton1.Appearance = appearance1;
            this.ultraButton1.ImageSize = new System.Drawing.Size(75, 23);
            this.ultraButton1.Location = new System.Drawing.Point(311, 569);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.ShowFocusRect = false;
            this.ultraButton1.ShowOutline = false;
            this.ultraButton1.Size = new System.Drawing.Size(78, 28);
            this.ultraButton1.TabIndex = 20;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // TransactionReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 600);
            this.Controls.Add(this.ultraButton1);
            this.Controls.Add(this.ctrlTransactionReport1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TransactionReport";
            this.Text = "Transaction Report";
            this.ResumeLayout(false);

        }

        #endregion

        private Nirvana.Admin.PositionManagement.Controls.CtrlTransactionReport ctrlTransactionReport1;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
    }
}