namespace Prana.PM.Client.UI.Controls
{
    partial class PMUserControl
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
            if (_accountBindableView != null)
                _accountBindableView.Dispose();
            _accountBindableView = null;
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Splitter = new System.Windows.Forms.Splitter();
            this.pmGrid = new Prana.PM.Client.UI.Controls.CtrlMainConsolidationView();
            this.pmDashboard = new Prana.Utilities.UI.UIUtilities.CustomizableCardGrid();
            this.SuspendLayout();
            // 
            // Splitter
            // 
            this.Splitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.Splitter.Location = new System.Drawing.Point(0, 150);
            this.Splitter.Name = "Splitter";
            this.Splitter.Size = new System.Drawing.Size(919, 3);
            this.Splitter.TabIndex = 1;
            this.Splitter.TabStop = false;
            // 
            // pmGrid
            // 
            this.pmGrid.BackColor = System.Drawing.Color.Black;
            this.pmGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pmGrid.ExPnlBindableView = null;
            this.pmGrid.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.pmGrid.HideExpandCollapseButtonPanel = false;
            this.pmGrid.IsInitialized = false;
            this.pmGrid.Location = new System.Drawing.Point(0, 153);
            this.pmGrid.Margin = new System.Windows.Forms.Padding(0);
            this.pmGrid.Name = "pmGrid";
            this.pmGrid.ParentKey = null;
            this.pmGrid.PositionTypes = Prana.BusinessObjects.AppConstants.ExPNLData.None;
            this.pmGrid.ShowGroupByBox = false;
            this.pmGrid.Size = new System.Drawing.Size(919, 385);
            this.pmGrid.SplitterDistanceFromTop = -2147483648;
            this.pmGrid.TabIndex = 2;
            // 
            // pmDashboard
            // 
            this.pmDashboard.AutoScroll = true;
            this.pmDashboard.BackColor = System.Drawing.Color.Black;
            this.pmDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pmDashboard.ForeColor = System.Drawing.Color.White;
            this.pmDashboard.Location = new System.Drawing.Point(0, 0);
            this.pmDashboard.Name = "pmDashboard";
            this.pmDashboard.Size = new System.Drawing.Size(919, 150);
            this.pmDashboard.TabIndex = 0;
            // 
            // PMUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pmGrid);
            this.Controls.Add(this.Splitter);
            this.Controls.Add(this.pmDashboard);
            this.Name = "PMUserControl";
            this.Size = new System.Drawing.Size(919, 538);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Splitter Splitter;
        private Utilities.UI.UIUtilities.CustomizableCardGrid pmDashboard;
        private CtrlMainConsolidationView pmGrid;


    }
}
