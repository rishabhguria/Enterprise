using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.CorporateActionNew.Controls
{
    partial class ctrlPositions
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.grdPositions = new PranaUltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuUndoCA = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRedoCA = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveLayoutApply = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveLayoutUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveLayoutRedo = new System.Windows.Forms.ToolStripMenuItem();

            this.mnuRemoveFiltersApply = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveFiltersUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveFiltersRedo = new System.Windows.Forms.ToolStripMenuItem();

            this.mnuExportToExcel = new System.Windows.Forms.ToolStripMenuItem();

            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdPositions)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdPositions
            // 
            this.grdPositions.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdPositions.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdPositions.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdPositions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPositions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdPositions.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdPositions.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPositions.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.LightSlateGray;
            appearance2.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance2.BorderColor = System.Drawing.Color.DimGray;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.grdPositions.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdPositions.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdPositions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdPositions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdPositions.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdPositions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdPositions.DisplayLayout.Override.CellPadding = 0;
            this.grdPositions.DisplayLayout.Override.CellSpacing = 0;
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8F;
            appearance3.TextHAlignAsString = "Center";
            this.grdPositions.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdPositions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdPositions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.grdPositions.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Middle";
            this.grdPositions.DisplayLayout.Override.RowAppearance = appearance5;
            this.grdPositions.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdPositions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdPositions.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            appearance6.FontData.BoldAsString = "True";
            this.grdPositions.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.grdPositions.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdPositions.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdPositions.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPositions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdPositions.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdPositions.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdPositions.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdPositions.DisplayLayout.Override.TemplateAddRowAppearance = appearance7;
            this.grdPositions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdPositions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdPositions.DisplayLayout.UseFixedHeaders = true;
            this.grdPositions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPositions.Location = new System.Drawing.Point(0, 0);
            this.grdPositions.Name = "grdPositions";
            this.grdPositions.Size = new System.Drawing.Size(778, 383);
            this.grdPositions.TabIndex = 1;
            this.grdPositions.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdPositions_InitializeRow);
            this.grdPositions.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdPositions_BeforeCustomRowFilterDialog);
            this.grdPositions.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdPositions_BeforeColumnChooserDisplayed);
            this.grdPositions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdPositions_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUndoCA,
            this.mnuRedoCA,
            this.mnuSaveLayoutApply,
            this.mnuSaveLayoutUndo,
            this.mnuSaveLayoutRedo,
            this.mnuRemoveFiltersApply,
            this.mnuRemoveFiltersUndo,
            this.mnuRemoveFiltersRedo,
            this.mnuExportToExcel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 114);
            // 
            // mnuUndoCA
            // 
            this.mnuUndoCA.Enabled = false;
            this.mnuUndoCA.Name = "mnuUndoCA";
            this.mnuUndoCA.Size = new System.Drawing.Size(137, 22);
            this.mnuUndoCA.Text = "Undo";
            this.mnuUndoCA.Click += new System.EventHandler(this.mnuUndoCA_Click);
            // 
            // mnuRedoCA
            // 
            this.mnuRedoCA.Enabled = false;
            this.mnuRedoCA.Name = "mnuRedoCA";
            this.mnuRedoCA.Size = new System.Drawing.Size(137, 22);
            this.mnuRedoCA.Text = "Apply";
            this.mnuRedoCA.Click += new System.EventHandler(this.mnuRedoCA_Click);
            // 
            // mnuSaveLayoutApply
            // 
            this.mnuSaveLayoutApply.Name = "mnuSaveLayoutApply";
            this.mnuSaveLayoutApply.Size = new System.Drawing.Size(137, 22);
            this.mnuSaveLayoutApply.Text = "Save Layout";
            this.mnuSaveLayoutApply.Click += new System.EventHandler(this.saveLayoutToolStripMenuItemApply_Click);
            // 
            // mnuSaveLayoutUndo
            // 
            this.mnuSaveLayoutUndo.Name = "mnuSaveLayoutUndo";
            this.mnuSaveLayoutUndo.Size = new System.Drawing.Size(137, 22);
            this.mnuSaveLayoutUndo.Text = "Save Layout";
            this.mnuSaveLayoutUndo.Click += new System.EventHandler(this.saveLayoutToolStripMenuItemUndo_Click);
            // 
            // mnuSaveLayoutRedo
            // 
            this.mnuSaveLayoutRedo.Name = "mnuSaveLayoutRedo";
            this.mnuSaveLayoutRedo.Size = new System.Drawing.Size(137, 22);
            this.mnuSaveLayoutRedo.Text = "Save Layout";
            this.mnuSaveLayoutRedo.Click += new System.EventHandler(this.saveLayoutToolStripMenuItemRedo_Click);
            // 
            // mnuRemoveFiltersApply
            // 
            this.mnuRemoveFiltersApply.Name = "mnuRemoveFiltersApply";
            this.mnuRemoveFiltersApply.Size = new System.Drawing.Size(137, 22);
            this.mnuRemoveFiltersApply.Text = "Remove Filter";
            this.mnuRemoveFiltersApply.Click += new System.EventHandler(this.removeFiltersToolStripMenuItemApply_Click);
            // 
            // mnuRemoveFiltersUndo
            // 
            this.mnuRemoveFiltersUndo.Name = "mnuRemoveFiltersUndo";
            this.mnuRemoveFiltersUndo.Size = new System.Drawing.Size(137, 22);
            this.mnuRemoveFiltersUndo.Text = "Remove Filter";
            this.mnuRemoveFiltersUndo.Click += new System.EventHandler(this.removeFiltersToolStripMenuItemUndo_Click);
            // 
            // mnuRemoveFiltersRedo
            // 
            this.mnuRemoveFiltersRedo.Name = "mnuRemoveFiltersRedo";
            this.mnuRemoveFiltersRedo.Size = new System.Drawing.Size(137, 22);
            this.mnuRemoveFiltersRedo.Text = "Remove Filter";
            this.mnuRemoveFiltersRedo.Click += new System.EventHandler(this.removeFiltersToolStripMenuItemRedo_Click);
            // 
            // mnuExportToExcel
            // 
            this.mnuExportToExcel.Name = "mnuExportToExcel";
            this.mnuExportToExcel.Size = new System.Drawing.Size(137, 22);
            this.mnuExportToExcel.Text = "Export To Excel";
            this.mnuExportToExcel.Click += new System.EventHandler(this.mnuExportToExcel_Click);
            // 
            // ctrlPositions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.grdPositions);
            this.Name = "ctrlPositions";
            this.Size = new System.Drawing.Size(778, 383);
            this.Load += new System.EventHandler(this.ctrlPositions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdPositions)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal PranaUltraGrid grdPositions;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuUndoCA;
        private System.Windows.Forms.ToolStripMenuItem mnuRedoCA;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveLayoutApply;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveLayoutUndo;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveLayoutRedo;

        private System.Windows.Forms.ToolStripMenuItem mnuRemoveFiltersApply;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveFiltersUndo;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveFiltersRedo;

        private System.Windows.Forms.ToolStripMenuItem mnuExportToExcel;

        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
    }
}
