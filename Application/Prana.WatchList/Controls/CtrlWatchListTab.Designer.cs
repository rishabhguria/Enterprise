using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WatchList.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.WatchList.Controls
{
    partial class CtrlWatchListTab
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
                if (_securityMaster != null)
                {
                    _securityMaster.SecMstrDataResponse -= _securityMaster_SecMstrDataResponse;
                    _securityMaster.SecMstrDataSymbolSearcResponse -= _securityMaster_SecMstrDataSymbolSearcResponse;
                    _securityMaster = null;
                }
                if (dataGrid != null)
                {
                    dataGrid.BeforeColumnChooserDisplayed -= new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(Grid_BeforeColumnChooserDisplayed);
                    dataGrid.DoubleClickCell -= dataGrid_DoubleClickCell;
                    dataGrid.ClickCell -= dataGrid_ClickCell;
                    dataGrid = null;
                }
                if (symbolValue != null)
                {
                    symbolValue.SymbolSearch -= SymbolSearch;
                    symbolValue.Dispose();
                    symbolValue = null;
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.OptionTable = new System.Windows.Forms.TableLayoutPanel();
            this.linkTabBtn = new Infragistics.Win.Misc.UltraButton();
            this.importSymbolBtn = new Infragistics.Win.Misc.UltraButton();
            this.addSymbolBtn = new Infragistics.Win.Misc.UltraButton();
            this.symbolValue = new Prana.Utilities.UI.UIUtilities.PranaSymbolCtrl();
            this.mainTblLayout = new System.Windows.Forms.TableLayoutPanel();
            this.dataGrid = new PranaUltraGrid();
            this.mnuSymbolLookup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblErrorMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.removeFilterToolStripMenuItemGrid = new System.Windows.Forms.ToolStripMenuItem(); ;
            this.buyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AdjustPositionStripMenuItem = new ToolStripMenuItem();
            this.sellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionChainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionTable.SuspendLayout();
            this.mainTblLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OptionTable
            // 
            this.OptionTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.OptionTable.ColumnCount = 6;
            this.OptionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.OptionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.OptionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.OptionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.OptionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.OptionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.OptionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.OptionTable.Controls.Add(this.linkTabBtn, 5, 0);
            this.OptionTable.Controls.Add(this.importSymbolBtn, 4, 0);
            this.OptionTable.Controls.Add(this.addSymbolBtn, 1, 0);
            this.OptionTable.Controls.Add(this.symbolValue, 0, 0);
            this.OptionTable.Location = new System.Drawing.Point(0, 0);
            this.OptionTable.Margin = new System.Windows.Forms.Padding(0);
            this.OptionTable.Name = "OptionTable";
            this.OptionTable.RowCount = 1;
            this.OptionTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.OptionTable.Size = new System.Drawing.Size(1003, 31);
            // 
            // linkTabBtn
            // 
            this.linkTabBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linkTabBtn.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(67)))), ((int)(((byte)(85)))));
            this.linkTabBtn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.linkTabBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkTabBtn.ForeColor = System.Drawing.Color.White;
            this.linkTabBtn.Location = new System.Drawing.Point(923, 3);
            this.linkTabBtn.Name = "linkTabBtn";
            this.linkTabBtn.Size = new System.Drawing.Size(77, 25);
            this.linkTabBtn.TabIndex = 4;
            this.linkTabBtn.Text = "Link tab";
            this.linkTabBtn.UseAppStyling = false;
            this.linkTabBtn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.linkTabBtn.Click += new System.EventHandler(this.LinkTabBtn_Click);
            // 
            // importSymbolBtn
            // 
            this.importSymbolBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.importSymbolBtn.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(67)))), ((int)(((byte)(85)))));
            this.importSymbolBtn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.importSymbolBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importSymbolBtn.ForeColor = System.Drawing.Color.White;
            this.importSymbolBtn.Location = new System.Drawing.Point(823, 3);
            this.importSymbolBtn.Name = "importSymbolBtn";
            this.importSymbolBtn.Size = new System.Drawing.Size(94, 25);
            this.importSymbolBtn.TabIndex = 3;
            this.importSymbolBtn.Text = "Upload File";
            this.importSymbolBtn.UseAppStyling = false;
            this.importSymbolBtn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.importSymbolBtn.Click += new System.EventHandler(this.importSymbolBtn_Click);
            // 
            // addSymbolBtn
            // 
            this.addSymbolBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.addSymbolBtn.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(67)))), ((int)(((byte)(85)))));
            this.addSymbolBtn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.addSymbolBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addSymbolBtn.ForeColor = System.Drawing.Color.White;
            this.addSymbolBtn.Location = new System.Drawing.Point(220, 3);
            this.addSymbolBtn.Name = "addSymbolBtn";
            this.addSymbolBtn.Size = new System.Drawing.Size(94, 25);
            this.addSymbolBtn.TabIndex = 2;
            this.addSymbolBtn.Text = "Add Symbol";
            this.addSymbolBtn.UseAppStyling = false;
            this.addSymbolBtn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.addSymbolBtn.Click += new System.EventHandler(this.addSymbolBtn_Click);
            // 
            // symbolValue
            // 
            this.symbolValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.symbolValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this.symbolValue.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.symbolValue.Location = new System.Drawing.Point(20, 2);
            this.symbolValue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.symbolValue.MaxLength = 32767;
            this.symbolValue.Name = "symbolValue";
            this.symbolValue.PrevSymbolEntered = "";
            this.symbolValue.Size = new System.Drawing.Size(194, 24);
            this.symbolValue.TabIndex = 1;
            // 
            // mainTblLayout
            // 
            this.mainTblLayout.ColumnCount = 1;
            this.mainTblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTblLayout.Controls.Add(this.OptionTable, 0, 0);
            this.mainTblLayout.Controls.Add(this.dataGrid, 0, 1);
            this.mainTblLayout.Controls.Add(this.statusStrip1, 0, 2);
            this.mainTblLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTblLayout.Location = new System.Drawing.Point(0, 0);
            this.mainTblLayout.Name = "mainTblLayout";
            this.mainTblLayout.RowCount = 3;
            this.mainTblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.mainTblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTblLayout.Size = new System.Drawing.Size(1003, 500);
            // 
            // mnuSymbolLookup
            // 
            this.mnuSymbolLookup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buyToolStripMenuItem,
            this.sellToolStripMenuItem,
            this.AdjustPositionStripMenuItem,
            this.deleteSymbolToolStripMenuItem,
            this.optionChainToolStripMenuItem,
            this.removeFilterToolStripMenuItemGrid,
            this.saveLayoutToolStripMenuItem});
            this.mnuSymbolLookup.Name = "contextMenuStrip1";
            this.mnuSymbolLookup.Size = new System.Drawing.Size(151, 114);
            this.mnuSymbolLookup.Opening += EnableDsiableStripOptions;
            // 
            // deleteSymbolToolStripMenuItem
            // 
            this.deleteSymbolToolStripMenuItem.Name = "deleteSymbolToolStripMenuItem";
            this.deleteSymbolToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.deleteSymbolToolStripMenuItem.Text = "Delete Symbol";
            this.deleteSymbolToolStripMenuItem.Click += new System.EventHandler(this.DeleteSymbolBtn_Click);
            // 
            // removeFilterToolStripMenuItemGrid
            // 
            this.removeFilterToolStripMenuItemGrid.Name = "removeFilterToolStripMenuItemGrid";
            this.removeFilterToolStripMenuItemGrid.Size = new System.Drawing.Size(150, 22);
            this.removeFilterToolStripMenuItemGrid.Text = "Remove Filter";
            this.removeFilterToolStripMenuItemGrid.Click += new System.EventHandler(this.RemoveFilterToolStripMenuItemForFileGrid_Click);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.SaveLayoutToolStripMenuItem_Click);
            // 
            // buyToolStripMenuItem
            // 
            this.buyToolStripMenuItem.Name = "buyToolStripMenuItem";
            this.buyToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.buyToolStripMenuItem.Text = "Buy";
            this.buyToolStripMenuItem.Click += new System.EventHandler(this.TradeSymbolToolStripMenuItem_Click);
            // 
            // sellToolStripMenuItem
            // 
            this.sellToolStripMenuItem.Name = "sellToolStripMenuItem";
            this.sellToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.sellToolStripMenuItem.Text = "Sell short";
            this.sellToolStripMenuItem.Click += new System.EventHandler(this.TradeSymbolToolStripMenuItem_Click);
            // 
            // AdjustPositionStripMenuItem
            // 
            this.AdjustPositionStripMenuItem.Name = "AdjustPositionStripMenuItem";
            this.AdjustPositionStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.AdjustPositionStripMenuItem.Text = "Adjust Position";
            this.AdjustPositionStripMenuItem.Click += new System.EventHandler(this.PTTTradeSymbolToolStripMenuItem_Click);
            // 
            // optionChainToolStripMenuItem
            // 
            this.optionChainToolStripMenuItem.Name = "optionChainToolStripMenuItem";
            this.optionChainToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.optionChainToolStripMenuItem.Text = "Option Chain";
            this.optionChainToolStripMenuItem.Click += new System.EventHandler(this.optionChainToolStripMenuItem_Click);
            // 
            // dataGrid
            // 
            dataGrid.ContextMenuStrip = mnuSymbolLookup;
            dataGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            dataGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            dataGrid.DisplayLayout.GroupByBox.Hidden = true;
            dataGrid.DisplayLayout.MaxColScrollRegions = 1;
            dataGrid.DisplayLayout.MaxRowScrollRegions = 1;
            dataGrid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            dataGrid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            dataGrid.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            dataGrid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            dataGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            dataGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            dataGrid.DisplayLayout.Override.DefaultRowHeight = 26;
            dataGrid.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            dataGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            dataGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            dataGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            dataGrid.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            dataGrid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            dataGrid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            dataGrid.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            dataGrid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            dataGrid.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            dataGrid.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            dataGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGrid.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            dataGrid.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            dataGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            dataGrid.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            dataGrid.DisplayLayout.Override.FilterUIType = FilterUIType.HeaderIcons;
            
            dataGrid.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            dataGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            dataGrid.DisplayLayout.UseFixedHeaders = true;
            dataGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGrid.Location = new System.Drawing.Point(3, 3);
            dataGrid.Size = new System.Drawing.Size(983, 347);
            dataGrid.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            dataGrid.InitializeLayout += dataGrid_InitializeLayout;
            dataGrid.InitializeRow += dataGrid_InitializeRow;
            dataGrid.MouseDown += dataGrid_MouseDown;
            dataGrid.Name = "WatchlistDataGrid";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblErrorMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 469);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1003, 20);
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.lblErrorMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblErrorMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lblErrorMessage.Margin = new System.Windows.Forms.Padding(0);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(0, 0);
            // 
            // TabCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTblLayout);
            this.Name = "TabCtrl";
            this.Size = new System.Drawing.Size(1003, 500);
            this.Dock = DockStyle.Fill;
            this.OptionTable.ResumeLayout(false);
            this.mainTblLayout.ResumeLayout(false);
            this.mainTblLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private TableLayoutPanel OptionTable;
        private System.Windows.Forms.ContextMenuStrip mnuSymbolLookup;
        private ToolStripMenuItem removeFilterToolStripMenuItemGrid;
        private System.Windows.Forms.ToolStripMenuItem buyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AdjustPositionStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private ToolStripMenuItem deleteSymbolToolStripMenuItem;
        private ToolStripMenuItem optionChainToolStripMenuItem;
        private Infragistics.Win.Misc.UltraButton linkTabBtn;
        private Infragistics.Win.Misc.UltraButton importSymbolBtn;
        private Infragistics.Win.Misc.UltraButton addSymbolBtn;
        private Prana.Utilities.UI.UIUtilities.PranaSymbolCtrl symbolValue;
        private TableLayoutPanel mainTblLayout;
        private PranaUltraGrid dataGrid;
        private ToolStripStatusLabel lblErrorMessage;
        private StatusStrip statusStrip1;

    }
}
