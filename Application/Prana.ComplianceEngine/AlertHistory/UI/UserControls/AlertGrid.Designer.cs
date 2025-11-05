using Prana.Utilities.UI.UIUtilities;

namespace Prana.ComplianceEngine.AlertHistory.UI.UserControls
{
    partial class AlertGrid
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_alertHistory != null)
                {
                    _alertHistory.Dispose();
                }
                if(_vlAlertPopResponse!=null)
                {
                    _vlAlertPopResponse.Dispose();
                }
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
            this.components = new System.ComponentModel.Container();
            this.ultraPnlGridMain = new Infragistics.Win.Misc.UltraPanel();
            this.ultraAlertGrid = new PranaUltraGrid();
            this.ultraGridAlertExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.ultraToolTipManagerAlert = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.cntxtMnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraPnlGridMain.ClientArea.SuspendLayout();
            this.ultraPnlGridMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraAlertGrid)).BeginInit();
            this.cntxtMnu.SuspendLayout();
            this.SuspendLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlertGrid));
            // 
            // ultraPnlGridMain
            // 
            // 
            // ultraPnlGridMain.ClientArea
            // 
            this.ultraPnlGridMain.ClientArea.Controls.Add(this.ultraAlertGrid);
            this.ultraPnlGridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPnlGridMain.Location = new System.Drawing.Point(0, 0);
            this.ultraPnlGridMain.Name = "ultraPnlGridMain";
            this.ultraPnlGridMain.Size = new System.Drawing.Size(150, 150);
            this.ultraPnlGridMain.TabIndex = 0;
            // 
            // ultraAlertGrid
            // 
            this.ultraAlertGrid.ContextMenuStrip = this.cntxtMnu;
            this.ultraAlertGrid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.ultraAlertGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraAlertGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraAlertGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraAlertGrid.DisplayLayout.InterBandSpacing = 10;
            this.ultraAlertGrid.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraAlertGrid.DisplayLayout.MaxRowScrollRegions = 1;
            this.ultraAlertGrid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraAlertGrid.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.ultraAlertGrid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraAlertGrid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraAlertGrid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.ultraAlertGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraAlertGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.InsetSoft;

            this.ultraAlertGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraAlertGrid.DisplayLayout.Override.CellPadding = 0;
            this.ultraAlertGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraAlertGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.ultraAlertGrid.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.ultraAlertGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.ultraAlertGrid.DisplayLayout.Override.RowSelectorWidth = 20;
            this.ultraAlertGrid.DisplayLayout.Override.RowSpacingAfter = 1;
            this.ultraAlertGrid.DisplayLayout.Override.RowSpacingBefore = 3;
            this.ultraAlertGrid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraAlertGrid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraAlertGrid.DisplayLayout.PriorityScrolling = true;
            this.ultraAlertGrid.DisplayLayout.RowConnectorColor = System.Drawing.Color.Gray;
            this.ultraAlertGrid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Dashed;
            this.ultraAlertGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraAlertGrid.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this.ultraAlertGrid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ultraAlertGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraAlertGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraAlertGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraAlertGrid.Location = new System.Drawing.Point(0, 0);
            this.ultraAlertGrid.Name = "ultraAlertGrid";
            this.ultraAlertGrid.Size = new System.Drawing.Size(150, 150);
            this.ultraAlertGrid.TabIndex = 0;
            this.ultraAlertGrid.Text = "ultraGrid1";
            this.ultraAlertGrid.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.UltraAlertGrid_AfterCellUpdate);
            this.ultraAlertGrid.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.UltraAlertGrid_ClickCell);
            this.ultraAlertGrid.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ultraAlertGrid_MouseEnterElement);
            this.ultraAlertGrid.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.ultraAlertGrid_BeforeColumnChooserDisplayed);
            this.ultraAlertGrid.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.ultraAlertGrid_BeforeCustomRowFilterDialog);
            // 
            // ultraToolTipManagerAlert
            // 
            this.ultraToolTipManagerAlert.ContainingControl = this;
            // 
            // cntxtMnu
            // 
            this.cntxtMnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.cntxtMnu.Name = "cntxtMnu";
            this.cntxtMnu.Size = new System.Drawing.Size(153, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "Save Layout";
            this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("saveLayoutToolStripMenuItem.Image")));
            this.toolStripMenuItem1.Click += new System.EventHandler(this.SaveLayout_Click);
            // 
            // AlertGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.ultraPnlGridMain);
            this.Name = "AlertGrid";
            this.Load += new System.EventHandler(this.AlertGrid_Load);
            this.ultraPnlGridMain.ClientArea.ResumeLayout(false);
            this.ultraPnlGridMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraAlertGrid)).EndInit();
            this.cntxtMnu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPnlGridMain;
        private PranaUltraGrid ultraAlertGrid;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridAlertExporter;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManagerAlert;
        private System.Windows.Forms.ContextMenuStrip cntxtMnu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}
