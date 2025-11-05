namespace Prana.AllocationNew.Allocation.UI.CostAdjustment
{
    partial class CostAdjustmentForm
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
                UnBindEvents();
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
            this.costAdjustmentControlMain1 = new Prana.AllocationNew.Allocation.UI.CostAdjustment.CostAdjustmentControlMain();
            this.ultraStatusBar1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // costAdjustmentControlMain1
            // 
            this.costAdjustmentControlMain1.AutoSize = true;
            this.costAdjustmentControlMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.costAdjustmentControlMain1.Location = new System.Drawing.Point(0, 0);
            this.costAdjustmentControlMain1.Name = "costAdjustmentControlMain1";
            this.costAdjustmentControlMain1.Size = new System.Drawing.Size(802, 430);
            this.costAdjustmentControlMain1.TabIndex = 0;
            // 
            // ultraStatusBar1
            // 
            this.ultraStatusBar1.Location = new System.Drawing.Point(0, 407);
            this.ultraStatusBar1.Name = "ultraStatusBar1";
            this.ultraStatusBar1.Size = new System.Drawing.Size(802, 23);
            this.ultraStatusBar1.TabIndex = 1;
            // 
            // CostAdjustmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 430);
            this.Controls.Add(this.ultraStatusBar1);
            this.Controls.Add(this.costAdjustmentControlMain1);
            this.Name = "CostAdjustmentForm";
            this.Text = "Cost Adjustment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CostAdjustmentForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CostAdjustmentControlMain costAdjustmentControlMain1;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar1;


    }
}