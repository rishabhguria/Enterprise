namespace Prana.AllocationNew.Allocation.UI.CostAdjustment
{
    partial class CostAdjustmentControlMain
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.costAdjustmentGridControl1 = new Prana.AllocationNew.Allocation.UI.CostAdjustment.CostAdjustmentGridControl();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.costAdjustmentUndoGridControl1 = new Prana.AllocationNew.Allocation.UI.CostAdjustment.CostAdjustmentUndoGridControl();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.costAdjustmentControl1 = new Prana.AllocationNew.Allocation.UI.CostAdjustment.CostAdjustmentControl();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.costAdjustmentGridControl1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(717, 154);
            // 
            // costAdjustmentGridControl1
            // 
            this.costAdjustmentGridControl1.AutoSize = true;
            this.costAdjustmentGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.costAdjustmentGridControl1.Location = new System.Drawing.Point(0, 0);
            this.costAdjustmentGridControl1.Name = "costAdjustmentGridControl1";
            this.costAdjustmentGridControl1.Size = new System.Drawing.Size(717, 154);
            this.costAdjustmentGridControl1.TabIndex = 0;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.costAdjustmentUndoGridControl1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(717, 154);
            // 
            // costAdjustmentUndoGridControl1
            // 
            this.costAdjustmentUndoGridControl1.AutoSize = true;
            this.costAdjustmentUndoGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.costAdjustmentUndoGridControl1.Location = new System.Drawing.Point(0, 0);
            this.costAdjustmentUndoGridControl1.Name = "costAdjustmentUndoGridControl1";
            this.costAdjustmentUndoGridControl1.Size = new System.Drawing.Size(717, 154);
            this.costAdjustmentUndoGridControl1.TabIndex = 0;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.costAdjustmentControl1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(721, 144);
            this.ultraPanel1.TabIndex = 0;
            // 
            // costAdjustmentControl1
            // 
            this.costAdjustmentControl1.AutoScroll = true;
            this.costAdjustmentControl1.AutoSize = true;
            this.costAdjustmentControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.costAdjustmentControl1.Location = new System.Drawing.Point(0, 0);
            this.costAdjustmentControl1.Name = "costAdjustmentControl1";
            this.costAdjustmentControl1.Size = new System.Drawing.Size(721, 144);
            this.costAdjustmentControl1.TabIndex = 0;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 144);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 144;
            this.ultraSplitter1.Size = new System.Drawing.Size(721, 6);
            this.ultraSplitter1.TabIndex = 1;
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 150);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(721, 180);
            this.ultraTabControl1.TabIndex = 2;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Apply New";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Applied";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.ultraTabControl1.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(717, 154);
            // 
            // CostAdjustmentControlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.ultraTabControl1);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.ultraPanel1);
            this.Name = "CostAdjustmentControlMain";
            this.Size = new System.Drawing.Size(721, 330);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl2.PerformLayout();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private CostAdjustmentControl costAdjustmentControl1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private CostAdjustmentGridControl costAdjustmentGridControl1;
        private CostAdjustmentUndoGridControl costAdjustmentUndoGridControl1;

    }
}
