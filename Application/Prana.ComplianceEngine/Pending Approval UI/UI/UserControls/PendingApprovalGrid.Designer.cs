using Infragistics.Win.UltraWinGrid;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.ComplianceEngine.Pending_Approval_UI
{
    partial class PendingApprovalGrid
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
                if (_currentBand != null)
                {
                    _currentBand.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PendingApprovalGrid));
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.ultraPendingApprovalGrid = new PranaUltraGrid();
            this.cntxtMnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraPendingApprovalGrid)).BeginInit();
            this.cntxtMnu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraPendingApprovalGrid);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(150, 150);
            this.ultraPanel1.TabIndex = 0;
            // 
            // ultraPendingApprovalGrid
            // 
            this.ultraPendingApprovalGrid.ContextMenuStrip = this.cntxtMnu;
            this.ultraPendingApprovalGrid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.ultraPendingApprovalGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraPendingApprovalGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraPendingApprovalGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraPendingApprovalGrid.DisplayLayout.InterBandSpacing = 10;
            this.ultraPendingApprovalGrid.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraPendingApprovalGrid.DisplayLayout.MaxRowScrollRegions = 1;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.InsetSoft;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.CellPadding = 0;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.RowSelectorWidth = 20;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.RowSpacingAfter = 1;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.RowSpacingBefore = 3;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraPendingApprovalGrid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraPendingApprovalGrid.DisplayLayout.PriorityScrolling = true;
            this.ultraPendingApprovalGrid.DisplayLayout.RowConnectorColor = System.Drawing.Color.Gray;
            this.ultraPendingApprovalGrid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Dashed;
            this.ultraPendingApprovalGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraPendingApprovalGrid.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this.ultraPendingApprovalGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraPendingApprovalGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPendingApprovalGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraPendingApprovalGrid.Location = new System.Drawing.Point(0, 0);
            this.ultraPendingApprovalGrid.Name = "ultraPendingApprovalGrid";
            this.ultraPendingApprovalGrid.Size = new System.Drawing.Size(150, 150);
            this.ultraPendingApprovalGrid.TabIndex = 1;
            this.ultraPendingApprovalGrid.Text = "ultraPendingApprovalGrid";
            this.ultraPendingApprovalGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraPendingApprovalGrid_InitializeLayout);
            this.ultraPendingApprovalGrid.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraPendingApprovalGrid_InitializeRow);
            this.ultraPendingApprovalGrid.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraPendingApprovalGrid_ClickCellButton);
            this.ultraPendingApprovalGrid.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.ultraPendingApprovalGrid_BeforeCustomRowFilterDialog);
            this.ultraPendingApprovalGrid.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.ultraPendingApprovalGrid_BeforeColumnChooserDisplayed);
            this.ultraPendingApprovalGrid.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ultraPendingApprovalGrid_MouseEnterElement);
            this.ultraPendingApprovalGrid.AfterHeaderCheckStateChanged += new AfterHeaderCheckStateChangedEventHandler(this.ultraPendingApprovalGrid_AfterHeaderCheckStateChanged);
            this.ultraPendingApprovalGrid.AfterColPosChanged += new AfterColPosChangedEventHandler(this.ultraPendingApprovalGrid_AfterColPosChanged);
            this.ultraPendingApprovalGrid.ClickCell += new ClickCellEventHandler(this.ultraPendingApprovalGrid_CellClicked);
            // 
            // cntxtMnu
            // 
            this.cntxtMnu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cntxtMnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.exportToExcelToolStripMenuItem});
            this.cntxtMnu.Name = "cntxtMnu";
            this.cntxtMnu.Size = new System.Drawing.Size(138, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.toolStripMenuItem1.Text = "Save Layout";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // exportToExcelToolStripMenuItem
            // 
            this.exportToExcelToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportDataToolStripMenuItem.Image")));
            this.exportToExcelToolStripMenuItem.Name = "exportToExcelToolStripMenuItem";
            this.exportToExcelToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.exportToExcelToolStripMenuItem.Text = "Export";
            this.exportToExcelToolStripMenuItem.Click += new System.EventHandler(this.exportToExcelToolStripMenuItem_Click);
            // 
            // PendingApprovalGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "PendingApprovalGrid";
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraPendingApprovalGrid)).EndInit();
            this.cntxtMnu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private PranaUltraGrid ultraPendingApprovalGrid;
        private System.Windows.Forms.ContextMenuStrip cntxtMnu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exportToExcelToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
    }
}
