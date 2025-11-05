namespace Prana.Tools.PL.Controls
{
    partial class ctrlReconShowCAGeneratedTrades
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkShowCAGeneratedTrades = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowCAGeneratedTrades)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(308, 160);
            this.ultraPanel1.TabIndex = 0;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.chkShowCAGeneratedTrades);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(308, 160);
            this.ultraGroupBox1.TabIndex = 0;
            this.ultraGroupBox1.Text = "Show Corporate Actions Generated Trades";
            // 
            // chkShowCAGeneratedTrades
            // 
            appearance1.FontData.BoldAsString = "False";
            this.chkShowCAGeneratedTrades.Appearance = appearance1;
            this.chkShowCAGeneratedTrades.Location = new System.Drawing.Point(30, 55);
            this.chkShowCAGeneratedTrades.Name = "chkShowCAGeneratedTrades";
            this.chkShowCAGeneratedTrades.Size = new System.Drawing.Size(253, 50);
            this.chkShowCAGeneratedTrades.TabIndex = 0;
            this.chkShowCAGeneratedTrades.Text = "Show Corporate Actions Generated Trades";
            this.chkShowCAGeneratedTrades.CheckedChanged += new System.EventHandler(this.chkShowCAGeneratedTrades_CheckedChanged);
            // 
            // ctrlReconShowCAGeneratedTrades
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlReconShowCAGeneratedTrades";
            this.Size = new System.Drawing.Size(308, 160);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkShowCAGeneratedTrades)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkShowCAGeneratedTrades;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
    }
}
