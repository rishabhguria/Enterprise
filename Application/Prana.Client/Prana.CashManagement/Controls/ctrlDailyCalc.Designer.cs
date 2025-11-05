using Prana.LogManager;
using Prana.CashManagement.Classes;
using Prana.Global;
using System;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.CashManagement.Controls
{
    partial class ctrlDailyCalc
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
            try
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                    if (tbctrlAllOpenPositions != null)
                    {
                        tbctrlAllOpenPositions.Tabs.Clear();
                        tbctrlAllOpenPositions.Dispose();
                    }

                    if (ctrlMasterFundAndAccountsDropdown1 != null)
                        ctrlMasterFundAndAccountsDropdown1.Dispose();

                    if (_cashManagementLayoutForAllOpenPositions != null)
                    {
                        _cashManagementLayoutForAllOpenPositions.Dispose();
                    }
                    if (_cashManagementLayoutForCalculatedTransactions != null)
                    {
                        _cashManagementLayoutForCalculatedTransactions.Dispose();
                    }
                    if (noOfOpenPositionTab != null && noOfOpenPositionTab.Count > 0)
                    {
                        for (int counter = 0; counter < noOfOpenPositionTab.Count; counter++)
                        {
                            noOfOpenPositionTab[counter].CashLayout -= new EventHandler<EventArgs<CashManagementLayout>>(SetLayoutForAllOpenPositionsGrid);
                            noOfOpenPositionTab[counter].Dispose();
                            noOfOpenPositionTab[counter] = null;
                        }
                        noOfOpenPositionTab = null;
                    }
                }
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Windows Form Designer generated code

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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ugAllOpenPosition = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.egbAllTransaction = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel3 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.tbctrlAllOpenPositions = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.egbCalculatedTransactions = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ugCalculatedTransactions = new PranaUltraGrid();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.miniToolStrip = new System.Windows.Forms.StatusStrip();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnCalculate = new Infragistics.Win.Misc.UltraButton();
            this.btnGetData = new Infragistics.Win.Misc.UltraButton();
            this.ulblToDate = new Infragistics.Win.Misc.UltraLabel();
            this.udtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblFromDate = new Infragistics.Win.Misc.UltraLabel();
            this.udtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.grdGetLots = new Infragistics.Win.UltraWinGrid.UltraGrid();
            //this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ugbxDailyCalc = new Infragistics.Win.Misc.UltraGroupBox();
            this.ctrlMasterFundAndAccountsDropdown1 = new Prana.CashManagement.Controls.ctrlMasterFundAndAccountsDropdown();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugAllOpenPosition)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.egbAllTransaction)).BeginInit();
            this.egbAllTransaction.SuspendLayout();
            this.ultraExpandableGroupBoxPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbctrlAllOpenPositions)).BeginInit();
            this.tbctrlAllOpenPositions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.egbCalculatedTransactions)).BeginInit();
            this.egbCalculatedTransactions.SuspendLayout();
            this.ultraExpandableGroupBoxPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugCalculatedTransactions)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udtFromDate)).BeginInit();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGetLots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxDailyCalc)).BeginInit();
            this.ugbxDailyCalc.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ugAllOpenPosition);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 25);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1044, 187);
            // 
            // ugAllOpenPosition
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            this.ugAllOpenPosition.DisplayLayout.Appearance = appearance1;
            this.ugAllOpenPosition.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugAllOpenPosition.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ugAllOpenPosition.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.ugAllOpenPosition.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.HideRowSelector;
            this.ugAllOpenPosition.DisplayLayout.MaxColScrollRegions = 1;
            this.ugAllOpenPosition.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.LightSlateGray;
            appearance2.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance2.BorderColor = System.Drawing.Color.DimGray;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ugAllOpenPosition.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.ugAllOpenPosition.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ugAllOpenPosition.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.ugAllOpenPosition.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.ugAllOpenPosition.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ugAllOpenPosition.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.ugAllOpenPosition.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ugAllOpenPosition.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.ugAllOpenPosition.DisplayLayout.Override.BorderStyleRowSelector = Infragistics.Win.UIElementBorderStyle.None;
            this.ugAllOpenPosition.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ugAllOpenPosition.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            this.ugAllOpenPosition.DisplayLayout.Override.CellPadding = 0;
            this.ugAllOpenPosition.DisplayLayout.Override.CellSpacing = 0;
            this.ugAllOpenPosition.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.Color.Gray;
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance3.BorderColor = System.Drawing.Color.Black;
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ugAllOpenPosition.DisplayLayout.Override.GroupByRowAppearance = appearance3;
            this.ugAllOpenPosition.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
            this.ugAllOpenPosition.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.ugAllOpenPosition.DisplayLayout.Override.GroupBySummaryValueAppearance = appearance4;
            appearance5.FontData.BoldAsString = "False";
            appearance5.FontData.Name = "Segoe UI";
            appearance5.FontData.SizeInPoints = 9F;
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.ugAllOpenPosition.DisplayLayout.Override.HeaderAppearance = appearance5;
            this.ugAllOpenPosition.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.ugAllOpenPosition.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.ugAllOpenPosition.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance6.TextHAlignAsString = "Right";
            appearance6.TextVAlignAsString = "Middle";
            this.ugAllOpenPosition.DisplayLayout.Override.RowAlternateAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.Black;
            appearance7.TextHAlignAsString = "Right";
            appearance7.TextVAlignAsString = "Middle";
            this.ugAllOpenPosition.DisplayLayout.Override.RowAppearance = appearance7;
            this.ugAllOpenPosition.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.ugAllOpenPosition.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            appearance8.FontData.BoldAsString = "True";
            this.ugAllOpenPosition.DisplayLayout.Override.SelectedRowAppearance = appearance8;
            this.ugAllOpenPosition.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ugAllOpenPosition.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ugAllOpenPosition.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ugAllOpenPosition.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ugAllOpenPosition.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.ugAllOpenPosition.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.ugAllOpenPosition.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            this.ugAllOpenPosition.DisplayLayout.Override.SummaryFooterAppearance = appearance9;
            this.ugAllOpenPosition.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            appearance10.FontData.BoldAsString = "False";
            appearance10.ForeColor = System.Drawing.Color.White;
            this.ugAllOpenPosition.DisplayLayout.Override.SummaryValueAppearance = appearance10;
            this.ugAllOpenPosition.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance11.TextHAlignAsString = "Center";
            this.ugAllOpenPosition.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
            this.ugAllOpenPosition.DisplayLayout.PriorityScrolling = true;
            this.ugAllOpenPosition.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this.ugAllOpenPosition.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugAllOpenPosition.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugAllOpenPosition.DisplayLayout.UseFixedHeaders = true;
            this.ugAllOpenPosition.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ugAllOpenPosition.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugAllOpenPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugAllOpenPosition.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ugAllOpenPosition.Location = new System.Drawing.Point(0, 0);
            this.ugAllOpenPosition.Name = "ugAllOpenPosition";
            this.ugAllOpenPosition.Size = new System.Drawing.Size(1044, 187);
            this.ugAllOpenPosition.TabIndex = 9;
            this.ugAllOpenPosition.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 48);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 47);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.egbAllTransaction);
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1.Panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.egbCalculatedTransactions);
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1.Panel2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer1.Size = new System.Drawing.Size(1054, 533);
            this.splitContainer1.SplitterDistance = 238;
            this.splitContainer1.SplitterWidth = 1;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer1.TabIndex = 1;
            // 
            // egbAllTransaction
            // 
            appearance12.FontData.SizeInPoints = 9F;
            this.egbAllTransaction.Appearance = appearance12;
            this.egbAllTransaction.Controls.Add(this.ultraExpandableGroupBoxPanel3);
            this.egbAllTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.egbAllTransaction.ExpandedSize = new System.Drawing.Size(1054, 238);
            this.egbAllTransaction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.egbAllTransaction.Location = new System.Drawing.Point(0, 0);
            this.egbAllTransaction.Name = "egbAllTransaction";
            this.egbAllTransaction.Size = new System.Drawing.Size(1054, 238);
            this.egbAllTransaction.TabIndex = 1;
            this.egbAllTransaction.Text = "All Open Positions";
            this.egbAllTransaction.ExpandedStateChanged += new System.EventHandler(this.egbAllTransaction_ExpandedStateChanged);
            // 
            // ultraExpandableGroupBoxPanel3
            // 
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.tbctrlAllOpenPositions);
            this.ultraExpandableGroupBoxPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel3.Location = new System.Drawing.Point(3, 20);
            this.ultraExpandableGroupBoxPanel3.Name = "ultraExpandableGroupBoxPanel3";
            this.ultraExpandableGroupBoxPanel3.Size = new System.Drawing.Size(1048, 215);
            this.ultraExpandableGroupBoxPanel3.TabIndex = 0;
            // 
            // tbctrlAllOpenPositions
            // 
            appearance13.FontData.SizeInPoints = 9F;
            appearance13.ForeColor = System.Drawing.Color.Black;
            this.tbctrlAllOpenPositions.Appearance = appearance13;
            this.tbctrlAllOpenPositions.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tbctrlAllOpenPositions.Controls.Add(this.ultraTabPageControl1);
            this.tbctrlAllOpenPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbctrlAllOpenPositions.Location = new System.Drawing.Point(0, 0);
            this.tbctrlAllOpenPositions.Name = "tbctrlAllOpenPositions";
            this.tbctrlAllOpenPositions.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tbctrlAllOpenPositions.Size = new System.Drawing.Size(1048, 215);
            this.tbctrlAllOpenPositions.TabIndex = 0;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Positions";
            this.tbctrlAllOpenPositions.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1044, 187);
            // 
            // egbCalculatedTransactions
            // 
            appearance14.FontData.SizeInPoints = 9F;
            this.egbCalculatedTransactions.Appearance = appearance14;
            this.egbCalculatedTransactions.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.egbCalculatedTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.egbCalculatedTransactions.ExpandedSize = new System.Drawing.Size(1054, 294);
            this.egbCalculatedTransactions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.egbCalculatedTransactions.Location = new System.Drawing.Point(0, 0);
            this.egbCalculatedTransactions.Name = "egbCalculatedTransactions";
            this.egbCalculatedTransactions.Size = new System.Drawing.Size(1054, 294);
            this.egbCalculatedTransactions.TabIndex = 3;
            this.egbCalculatedTransactions.Text = "Calculated Transactions";
            this.egbCalculatedTransactions.ExpandedStateChanged += new System.EventHandler(this.egbCalculatedTransactions_ExpandedStateChanged);
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.ugCalculatedTransactions);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.statusStrip1);
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(3, 20);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(1048, 271);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // ugCalculatedTransactions
            // 
            this.ugCalculatedTransactions.ContextMenuStrip = this.contextMenuStrip1;
            appearance15.BackColor = System.Drawing.Color.Black;
            this.ugCalculatedTransactions.DisplayLayout.Appearance = appearance15;
            this.ugCalculatedTransactions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugCalculatedTransactions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ugCalculatedTransactions.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.ugCalculatedTransactions.DisplayLayout.MaxColScrollRegions = 1;
            this.ugCalculatedTransactions.DisplayLayout.MaxRowScrollRegions = 1;
            appearance16.BackColor = System.Drawing.Color.LightSlateGray;
            appearance16.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance16.BorderColor = System.Drawing.Color.DimGray;
            appearance16.FontData.BoldAsString = "True";
            appearance16.ForeColor = System.Drawing.Color.White;
            this.ugCalculatedTransactions.DisplayLayout.Override.ActiveRowAppearance = appearance16;
            this.ugCalculatedTransactions.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ugCalculatedTransactions.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.ugCalculatedTransactions.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.ugCalculatedTransactions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ugCalculatedTransactions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.ugCalculatedTransactions.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ugCalculatedTransactions.DisplayLayout.Override.BorderStyleRowSelector = Infragistics.Win.UIElementBorderStyle.None;
            this.ugCalculatedTransactions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ugCalculatedTransactions.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            this.ugCalculatedTransactions.DisplayLayout.Override.CellPadding = 0;
            this.ugCalculatedTransactions.DisplayLayout.Override.CellSpacing = 0;
            this.ugCalculatedTransactions.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance17.BackColor = System.Drawing.Color.Gray;
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance17.BorderColor = System.Drawing.Color.Black;
            appearance17.FontData.BoldAsString = "True";
            appearance17.ForeColor = System.Drawing.Color.White;
            this.ugCalculatedTransactions.DisplayLayout.Override.GroupByRowAppearance = appearance17;
            this.ugCalculatedTransactions.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
            this.ugCalculatedTransactions.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;
            appearance18.BorderColor = System.Drawing.Color.Transparent;
            appearance18.TextHAlignAsString = "Right";
            appearance18.TextVAlignAsString = "Middle";
            this.ugCalculatedTransactions.DisplayLayout.Override.GroupBySummaryValueAppearance = appearance18;
            appearance19.FontData.BoldAsString = "False";
            appearance19.FontData.Name = "Segoe UI";
            appearance19.FontData.SizeInPoints = 9F;
            appearance19.TextHAlignAsString = "Center";
            appearance19.TextVAlignAsString = "Middle";
            this.ugCalculatedTransactions.DisplayLayout.Override.HeaderAppearance = appearance19;
            this.ugCalculatedTransactions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.ugCalculatedTransactions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance20.TextHAlignAsString = "Right";
            appearance20.TextVAlignAsString = "Middle";
            this.ugCalculatedTransactions.DisplayLayout.Override.RowAlternateAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.Black;
            appearance21.ForeColor = System.Drawing.Color.White;
            appearance21.TextHAlignAsString = "Right";
            appearance21.TextVAlignAsString = "Middle";
            this.ugCalculatedTransactions.DisplayLayout.Override.RowAppearance = appearance21;
            this.ugCalculatedTransactions.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.ugCalculatedTransactions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.ugCalculatedTransactions.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance22.BackColor = System.Drawing.Color.Transparent;
            appearance22.BorderColor = System.Drawing.Color.Transparent;
            appearance22.FontData.BoldAsString = "True";
            this.ugCalculatedTransactions.DisplayLayout.Override.SelectedRowAppearance = appearance22;
            this.ugCalculatedTransactions.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ugCalculatedTransactions.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ugCalculatedTransactions.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ugCalculatedTransactions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ugCalculatedTransactions.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.ugCalculatedTransactions.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.ugCalculatedTransactions.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            appearance23.BorderColor = System.Drawing.Color.Transparent;
            this.ugCalculatedTransactions.DisplayLayout.Override.SummaryFooterAppearance = appearance23;
            this.ugCalculatedTransactions.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.Color.Transparent;
            appearance24.BorderColor = System.Drawing.Color.Transparent;
            appearance24.FontData.BoldAsString = "False";
            appearance24.ForeColor = System.Drawing.Color.White;
            this.ugCalculatedTransactions.DisplayLayout.Override.SummaryValueAppearance = appearance24;
            this.ugCalculatedTransactions.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance25.TextHAlignAsString = "Center";
            this.ugCalculatedTransactions.DisplayLayout.Override.TemplateAddRowAppearance = appearance25;
            this.ugCalculatedTransactions.DisplayLayout.PriorityScrolling = true;
            this.ugCalculatedTransactions.DisplayLayout.RowConnectorColor = System.Drawing.Color.White;
            this.ugCalculatedTransactions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugCalculatedTransactions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugCalculatedTransactions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugCalculatedTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugCalculatedTransactions.ExitEditModeOnLeave = false;
            this.ugCalculatedTransactions.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ugCalculatedTransactions.Location = new System.Drawing.Point(0, 0);
            this.ugCalculatedTransactions.Name = "ugCalculatedTransactions";
            this.ugCalculatedTransactions.Size = new System.Drawing.Size(1048, 249);
            this.ugCalculatedTransactions.TabIndex = 8;
            this.ugCalculatedTransactions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ugCalculatedTransactions.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugCalculatedTransactions_InitializeLayout);
            this.ugCalculatedTransactions.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugCalculatedTransactions_InitializeRow);
            this.ugCalculatedTransactions.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.ugCalculatedTransactions_InitializeGroupByRow);
            this.ugCalculatedTransactions.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugCalculatedTransactions_AfterSortChange);
            this.ugCalculatedTransactions.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.ugCalculatedTransactions_BeforeCustomRowFilterDialog);
            this.ugCalculatedTransactions.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.ugCalculatedTransactions_BeforeColumnChooserDisplayed);
            this.ugCalculatedTransactions.BeforeRowFilterDropDown += ugCalculatedTransactions_BeforeRowFilterDropDown;
            this.ugCalculatedTransactions.AfterRowFilterChanged += ugCalculatedTransactions_AfterRowFilterChanged;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 249);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1048, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.Location = new System.Drawing.Point(1, 1);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(771, 22);
            this.inboxControlStyler1.SetStyleSettings(this.miniToolStrip, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.miniToolStrip.TabIndex = 9;
            // 
            // btnSave
            // 
            appearance26.FontData.SizeInPoints = 9F;
            this.btnSave.Appearance = appearance26;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(973, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCalculate
            // 
            appearance27.FontData.SizeInPoints = 9F;
            this.btnCalculate.Appearance = appearance27;
            this.btnCalculate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalculate.Location = new System.Drawing.Point(881, 12);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(84, 23);
            this.btnCalculate.TabIndex = 5;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnGetData
            // 
            appearance28.FontData.SizeInPoints = 9F;
            this.btnGetData.Appearance = appearance28;
            this.btnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetData.Location = new System.Drawing.Point(789, 12);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(84, 23);
            this.btnGetData.TabIndex = 4;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // ulblToDate
            // 
            appearance29.FontData.SizeInPoints = 9F;
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.TextHAlignAsString = "Left";
            appearance29.TextVAlignAsString = "Middle";
            this.ulblToDate.Appearance = appearance29;
            this.ulblToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ulblToDate.Location = new System.Drawing.Point(646, 12);
            this.ulblToDate.Name = "ulblToDate";
            this.ulblToDate.Size = new System.Drawing.Size(23, 23);
            this.ulblToDate.TabIndex = 3;
            this.ulblToDate.Text = "To";
            // 
            // udtToDate
            // 
            this.udtToDate.AutoSize = false;
            this.udtToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udtToDate.Location = new System.Drawing.Point(671, 12);
            this.udtToDate.Name = "udtToDate";
            this.udtToDate.Size = new System.Drawing.Size(109, 23);
            this.udtToDate.TabIndex = 2;
            // 
            // lblFromDate
            // 
            appearance30.FontData.SizeInPoints = 9F;
            appearance30.ForeColor = System.Drawing.Color.Black;
            appearance30.TextHAlignAsString = "Left";
            appearance30.TextVAlignAsString = "Middle";
            this.lblFromDate.Appearance = appearance30;
            this.lblFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(490, 12);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(34, 23);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "From";
            // 
            // udtFromDate
            // 
            this.udtFromDate.AutoSize = false;
            this.udtFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udtFromDate.Location = new System.Drawing.Point(531, 12);
            this.udtFromDate.Name = "udtFromDate";
            this.udtFromDate.Size = new System.Drawing.Size(109, 23);
            this.udtFromDate.TabIndex = 1;
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.grdGetLots);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(707, 187);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // grdGetLots
            // 
            appearance31.BackColor = System.Drawing.Color.Black;
            this.grdGetLots.DisplayLayout.Appearance = appearance31;
            this.grdGetLots.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdGetLots.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdGetLots.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.EmptyRowSettings.ShowEmptyRows = true;
            this.grdGetLots.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.HideRowSelector;
            this.grdGetLots.DisplayLayout.MaxColScrollRegions = 1;
            this.grdGetLots.DisplayLayout.MaxRowScrollRegions = 1;
            appearance32.BackColor = System.Drawing.Color.LightSlateGray;
            appearance32.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance32.BorderColor = System.Drawing.Color.DimGray;
            appearance32.FontData.BoldAsString = "True";
            appearance32.ForeColor = System.Drawing.Color.White;
            this.grdGetLots.DisplayLayout.Override.ActiveRowAppearance = appearance32;
            this.grdGetLots.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdGetLots.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdGetLots.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdGetLots.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdGetLots.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            this.grdGetLots.DisplayLayout.Override.CellPadding = 0;
            this.grdGetLots.DisplayLayout.Override.CellSpacing = 0;
            this.grdGetLots.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance33.BorderColor = System.Drawing.Color.Transparent;
            appearance33.ForeColor = System.Drawing.Color.White;
            this.grdGetLots.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.FontData.Name = "Segoe UI";
            appearance34.FontData.SizeInPoints = 9F;
            appearance34.TextHAlignAsString = "Center";
            this.grdGetLots.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.grdGetLots.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdGetLots.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance35.ForeColor = System.Drawing.Color.White;
            appearance35.TextHAlignAsString = "Right";
            appearance35.TextVAlignAsString = "Middle";
            this.grdGetLots.DisplayLayout.Override.RowAlternateAppearance = appearance35;
            appearance36.BackColor = System.Drawing.Color.Black;
            appearance36.ForeColor = System.Drawing.Color.White;
            appearance36.TextHAlignAsString = "Right";
            appearance36.TextVAlignAsString = "Middle";
            this.grdGetLots.DisplayLayout.Override.RowAppearance = appearance36;
            this.grdGetLots.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdGetLots.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance37.BackColor = System.Drawing.Color.Transparent;
            appearance37.BorderColor = System.Drawing.Color.Transparent;
            appearance37.FontData.BoldAsString = "True";
            this.grdGetLots.DisplayLayout.Override.SelectedRowAppearance = appearance37;
            this.grdGetLots.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGetLots.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGetLots.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdGetLots.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGetLots.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdGetLots.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdGetLots.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            this.grdGetLots.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance38.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdGetLots.DisplayLayout.Override.TemplateAddRowAppearance = appearance38;
            this.grdGetLots.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdGetLots.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdGetLots.DisplayLayout.UseFixedHeaders = true;
            this.grdGetLots.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdGetLots.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdGetLots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdGetLots.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdGetLots.Location = new System.Drawing.Point(0, 0);
            this.grdGetLots.Name = "grdGetLots";
            this.grdGetLots.Size = new System.Drawing.Size(707, 187);
            this.grdGetLots.TabIndex = 107;
            this.grdGetLots.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ugbxDailyCalc
            // 
            this.ugbxDailyCalc.Controls.Add(this.ctrlMasterFundAndAccountsDropdown1);
            this.ugbxDailyCalc.Controls.Add(this.udtFromDate);
            this.ugbxDailyCalc.Controls.Add(this.ulblToDate);
            this.ugbxDailyCalc.Controls.Add(this.btnGetData);
            this.ugbxDailyCalc.Controls.Add(this.btnSave);
            this.ugbxDailyCalc.Controls.Add(this.udtToDate);
            this.ugbxDailyCalc.Controls.Add(this.lblFromDate);
            this.ugbxDailyCalc.Controls.Add(this.btnCalculate);
            this.ugbxDailyCalc.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxDailyCalc.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ugbxDailyCalc.Location = new System.Drawing.Point(0, 0);
            this.ugbxDailyCalc.Name = "ugbxDailyCalc";
            this.ugbxDailyCalc.Size = new System.Drawing.Size(1054, 47);
            this.ugbxDailyCalc.TabIndex = 2;
            // 
            // ctrlMasterFundAndAccountsDropdown1
            // 
            this.ctrlMasterFundAndAccountsDropdown1.Location = new System.Drawing.Point(6, 0);
            this.ctrlMasterFundAndAccountsDropdown1.Name = "ctrlMasterFundAndAccountsDropdown1";
            this.ctrlMasterFundAndAccountsDropdown1.Size = new System.Drawing.Size(485, 36);
            this.ctrlMasterFundAndAccountsDropdown1.TabIndex = 7;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer1);
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxDailyCalc);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1054, 580);
            this.ultraPanel1.TabIndex = 103;
            // 
            // ctrlDailyCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ultraPanel1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Name = "ctrlDailyCalc";
            this.Size = new System.Drawing.Size(1054, 580);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.ctrlDailyCalc_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugAllOpenPosition)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.egbAllTransaction)).EndInit();
            this.egbAllTransaction.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbctrlAllOpenPositions)).EndInit();
            this.tbctrlAllOpenPositions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.egbCalculatedTransactions)).EndInit();
            this.egbCalculatedTransactions.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugCalculatedTransactions)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udtFromDate)).EndInit();
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdGetLots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxDailyCalc)).EndInit();
            this.ugbxDailyCalc.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdGetLots;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox egbAllTransaction;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel3;
        private Infragistics.Win.Misc.UltraLabel lblFromDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor udtFromDate;
        //private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor udtToDate;
        private Infragistics.Win.Misc.UltraLabel ulblToDate;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbctrlAllOpenPositions;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnCalculate;
        private Infragistics.Win.Misc.UltraButton btnGetData;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugAllOpenPosition;
        private Infragistics.Win.Misc.UltraExpandableGroupBox egbCalculatedTransactions;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private PranaUltraGrid ugCalculatedTransactions;
        private System.Windows.Forms.StatusStrip miniToolStrip;
        private Infragistics.Win.Misc.UltraGroupBox ugbxDailyCalc;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private ctrlMasterFundAndAccountsDropdown ctrlMasterFundAndAccountsDropdown1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
    }
}
