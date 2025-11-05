using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.PM.Client.UI.Controls
{
    partial class CtrlCreateAndImportPosition
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
                if (_allocationProxy != null)
                {
                    _allocationProxy.Dispose();
                }
                if (_templates != null)
                {
                    _templates.Dispose();
                }
                if (_commissionSources != null)
                {
                    _commissionSources.Dispose();
                }
                if (_defaultSelectValListItem != null)
                {
                    _defaultSelectValListItem.Dispose();
                }
                if (_fxConversionMethodOperator != null)
                {
                    _fxConversionMethodOperator.Dispose();
                }
                if (_importType != null)
                {
                    _importType.Dispose();
                }
                if (_positionType != null)
                {
                    _positionType.Dispose();
                }
                if (_softCommissionSources != null)
                {
                    _softCommissionSources.Dispose();
                }
                if (_ifPayReceiveChanges != null)
                {
                    _ifPayReceiveChanges.Dispose();
                }
                if (_fileType != null)
                {
                    _fileType.Dispose();
                }
                if (_closingServices != null)
                {
                    _closingServices.Dispose();
                }
                if (_allocationServices != null)
                {
                    _allocationServices.Dispose();
                }
                if (_gridBandOTCPositions != null)
                {
                    _gridBandOTCPositions.Dispose();
                }
                if (_accounts != null)
                {
                    _accounts.Dispose();
                }
                if (_cashManagementServices != null)
                {
                    _cashManagementServices.Dispose();
                }
                if (_securityMaster != null)
                    _securityMaster.SecMstrDataResponse -= _securityMaster_SecMstrDataResponse;
            }
            //_allSides.Dispose();
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
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            this.grdCreatePosition = new PranaUltraGrid();
            this.mnuExerciseAssign = new System.Windows.Forms.ContextMenuStrip();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmbSide = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cmbCounterParty = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cmbAccounts = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cmbStrategy = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cmbSymbolConvention = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cmbInstrumentType = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cmbVenue = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraTextEditor1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.cmbOptionType = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            ((System.ComponentModel.ISupportInitialize)(this.grdCreatePosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSymbolConvention)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbInstrumentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOptionType)).BeginInit();
            this.SuspendLayout();
            // 
            // grdCreatePosition
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdCreatePosition.DisplayLayout.Appearance = appearance1;
            this.grdCreatePosition.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdCreatePosition.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdCreatePosition.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdCreatePosition.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdCreatePosition.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdCreatePosition.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdCreatePosition.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCreatePosition.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.BackColor2 = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.White;
            this.grdCreatePosition.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.LightSlateGray;
            appearance6.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance6.BorderColor = System.Drawing.Color.DimGray;
            appearance6.ForeColor = System.Drawing.Color.White;
            this.grdCreatePosition.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdCreatePosition.DisplayLayout.Override.AllowMultiCellOperations = ((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation)((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Copy | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Paste)));
            this.grdCreatePosition.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdCreatePosition.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdCreatePosition.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdCreatePosition.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdCreatePosition.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdCreatePosition.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdCreatePosition.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdCreatePosition.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdCreatePosition.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCreatePosition.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdCreatePosition.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdCreatePosition.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdCreatePosition.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            this.grdCreatePosition.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCreatePosition.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCreatePosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCreatePosition.Location = new System.Drawing.Point(0, 0);
            this.grdCreatePosition.Name = "grdCreatePosition";
            this.grdCreatePosition.Size = new System.Drawing.Size(921, 398);
            this.grdCreatePosition.TabIndex = 6;
            this.grdCreatePosition.Text = "grdCreatePosition";
            this.grdCreatePosition.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCreatePosition_AfterCellUpdate);
            this.grdCreatePosition.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCreatePosition_InitializeLayout);
            this.grdCreatePosition.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCreatePosition_CellChange);
            this.grdCreatePosition.CellListSelect += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCreatePosition_CellListSelect);
            this.grdCreatePosition.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdCreatePosition_BeforeCellUpdate);
            this.grdCreatePosition.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdCreatePosition_Error);
            this.grdCreatePosition.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdCreatePosition_BeforeCustomRowFilterDialog);
            this.grdCreatePosition.CellDataError += new Infragistics.Win.UltraWinGrid.CellDataErrorEventHandler(this.grdCreatePosition_CellDataError);
            this.grdCreatePosition.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdCreatePosition_BeforeColumnChooserDisplayed);
            this.grdCreatePosition.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdCreatePosition_BeforeRowsDeleted);
            // 
            // mnuExerciseAssign
            // 
            this.mnuExerciseAssign.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duplicateToolStripMenuItem});
            this.mnuExerciseAssign.Name = "mnuExerciseAssign";
            this.mnuExerciseAssign.Size = new System.Drawing.Size(151, 114);
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.duplicateToolStripMenuItem.Text = "Add duplicate row";
            this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.DuplicateRowBtn_Click);
            // 
            // cmbSide
            // 
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSide.DisplayLayout.Appearance = appearance12;
            this.cmbSide.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSide.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.GroupByBox.Appearance = appearance13;
            appearance14.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSide.DisplayLayout.GroupByBox.BandLabelAppearance = appearance14;
            this.cmbSide.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance15.BackColor2 = System.Drawing.SystemColors.Control;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSide.DisplayLayout.GroupByBox.PromptAppearance = appearance15;
            this.cmbSide.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSide.DisplayLayout.MaxRowScrollRegions = 1;
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            appearance16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSide.DisplayLayout.Override.ActiveCellAppearance = appearance16;
            appearance17.BackColor = System.Drawing.SystemColors.Highlight;
            appearance17.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSide.DisplayLayout.Override.ActiveRowAppearance = appearance17;
            this.cmbSide.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSide.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.Override.CardAreaAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.Silver;
            appearance19.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSide.DisplayLayout.Override.CellAppearance = appearance19;
            this.cmbSide.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSide.DisplayLayout.Override.CellPadding = 0;
            appearance20.BackColor = System.Drawing.SystemColors.Control;
            appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance20.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance20.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.Override.GroupByRowAppearance = appearance20;
            appearance21.TextHAlignAsString = "Left";
            this.cmbSide.DisplayLayout.Override.HeaderAppearance = appearance21;
            this.cmbSide.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSide.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance22.BackColor = System.Drawing.SystemColors.Window;
            appearance22.BorderColor = System.Drawing.Color.Silver;
            this.cmbSide.DisplayLayout.Override.RowAppearance = appearance22;
            this.cmbSide.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance23.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSide.DisplayLayout.Override.TemplateAddRowAppearance = appearance23;
            this.cmbSide.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSide.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSide.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSide.Location = new System.Drawing.Point(627, 353);
            this.cmbSide.Name = "cmbSide";
            this.cmbSide.Size = new System.Drawing.Size(96, 30);
            this.cmbSide.TabIndex = 111;
            this.cmbSide.Text = "Side Combo";
            this.cmbSide.Visible = false;
            // 
            // cmbCounterParty
            // 
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            appearance24.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCounterParty.DisplayLayout.Appearance = appearance24;
            this.cmbCounterParty.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterParty.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance25.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance25.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance25.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.GroupByBox.Appearance = appearance25;
            appearance26.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BandLabelAppearance = appearance26;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance27.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance27.BackColor2 = System.Drawing.SystemColors.Control;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.PromptAppearance = appearance27;
            this.cmbCounterParty.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCounterParty.DisplayLayout.MaxRowScrollRegions = 1;
            appearance28.BackColor = System.Drawing.SystemColors.Window;
            appearance28.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveCellAppearance = appearance28;
            appearance29.BackColor = System.Drawing.SystemColors.Highlight;
            appearance29.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveRowAppearance = appearance29;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance30.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.CardAreaAppearance = appearance30;
            appearance31.BorderColor = System.Drawing.Color.Silver;
            appearance31.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCounterParty.DisplayLayout.Override.CellAppearance = appearance31;
            this.cmbCounterParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCounterParty.DisplayLayout.Override.CellPadding = 0;
            appearance32.BackColor = System.Drawing.SystemColors.Control;
            appearance32.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance32.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance32.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.GroupByRowAppearance = appearance32;
            appearance33.TextHAlignAsString = "Left";
            this.cmbCounterParty.DisplayLayout.Override.HeaderAppearance = appearance33;
            this.cmbCounterParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCounterParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance34.BackColor = System.Drawing.SystemColors.Window;
            appearance34.BorderColor = System.Drawing.Color.Silver;
            this.cmbCounterParty.DisplayLayout.Override.RowAppearance = appearance34;
            this.cmbCounterParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance35.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCounterParty.DisplayLayout.Override.TemplateAddRowAppearance = appearance35;
            this.cmbCounterParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCounterParty.Location = new System.Drawing.Point(729, 353);
            this.cmbCounterParty.Name = "cmbCounterParty";
            this.cmbCounterParty.Size = new System.Drawing.Size(96, 30);
            this.cmbCounterParty.TabIndex = 112;
            this.cmbCounterParty.Text = "CounterParty";
            this.cmbCounterParty.Visible = false;
            // 
            // cmbAccounts
            // 
            appearance36.BackColor = System.Drawing.SystemColors.Window;
            appearance36.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAccounts.DisplayLayout.Appearance = appearance36;
            this.cmbAccounts.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAccounts.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance37.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance37.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance37.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAccounts.DisplayLayout.GroupByBox.Appearance = appearance37;
            appearance38.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAccounts.DisplayLayout.GroupByBox.BandLabelAppearance = appearance38;
            this.cmbAccounts.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance39.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance39.BackColor2 = System.Drawing.SystemColors.Control;
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAccounts.DisplayLayout.GroupByBox.PromptAppearance = appearance39;
            this.cmbAccounts.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAccounts.DisplayLayout.MaxRowScrollRegions = 1;
            appearance40.BackColor = System.Drawing.SystemColors.Window;
            appearance40.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAccounts.DisplayLayout.Override.ActiveCellAppearance = appearance40;
            appearance41.BackColor = System.Drawing.SystemColors.Highlight;
            appearance41.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAccounts.DisplayLayout.Override.ActiveRowAppearance = appearance41;
            this.cmbAccounts.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAccounts.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance42.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAccounts.DisplayLayout.Override.CardAreaAppearance = appearance42;
            appearance43.BorderColor = System.Drawing.Color.Silver;
            appearance43.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAccounts.DisplayLayout.Override.CellAppearance = appearance43;
            this.cmbAccounts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAccounts.DisplayLayout.Override.CellPadding = 0;
            appearance44.BackColor = System.Drawing.SystemColors.Control;
            appearance44.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance44.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance44.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAccounts.DisplayLayout.Override.GroupByRowAppearance = appearance44;
            appearance45.TextHAlignAsString = "Left";
            this.cmbAccounts.DisplayLayout.Override.HeaderAppearance = appearance45;
            this.cmbAccounts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAccounts.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance46.BackColor = System.Drawing.SystemColors.Window;
            appearance46.BorderColor = System.Drawing.Color.Silver;
            this.cmbAccounts.DisplayLayout.Override.RowAppearance = appearance46;
            this.cmbAccounts.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance47.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAccounts.DisplayLayout.Override.TemplateAddRowAppearance = appearance47;
            this.cmbAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAccounts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAccounts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAccounts.Location = new System.Drawing.Point(3, 353);
            this.cmbAccounts.Name = "cmbAccounts";
            this.cmbAccounts.Size = new System.Drawing.Size(96, 30);
            this.cmbAccounts.TabIndex = 113;
            this.cmbAccounts.Text = "combo accounts";
            this.cmbAccounts.Visible = false;
            // 
            // cmbStrategy
            // 
            appearance48.BackColor = System.Drawing.SystemColors.Window;
            appearance48.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbStrategy.DisplayLayout.Appearance = appearance48;
            this.cmbStrategy.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbStrategy.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance49.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance49.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance49.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStrategy.DisplayLayout.GroupByBox.Appearance = appearance49;
            appearance50.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStrategy.DisplayLayout.GroupByBox.BandLabelAppearance = appearance50;
            this.cmbStrategy.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance51.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance51.BackColor2 = System.Drawing.SystemColors.Control;
            appearance51.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance51.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStrategy.DisplayLayout.GroupByBox.PromptAppearance = appearance51;
            this.cmbStrategy.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbStrategy.DisplayLayout.MaxRowScrollRegions = 1;
            appearance52.BackColor = System.Drawing.SystemColors.Window;
            appearance52.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbStrategy.DisplayLayout.Override.ActiveCellAppearance = appearance52;
            appearance53.BackColor = System.Drawing.SystemColors.Highlight;
            appearance53.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbStrategy.DisplayLayout.Override.ActiveRowAppearance = appearance53;
            this.cmbStrategy.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbStrategy.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance54.BackColor = System.Drawing.SystemColors.Window;
            this.cmbStrategy.DisplayLayout.Override.CardAreaAppearance = appearance54;
            appearance55.BorderColor = System.Drawing.Color.Silver;
            appearance55.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbStrategy.DisplayLayout.Override.CellAppearance = appearance55;
            this.cmbStrategy.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbStrategy.DisplayLayout.Override.CellPadding = 0;
            appearance56.BackColor = System.Drawing.SystemColors.Control;
            appearance56.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance56.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance56.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance56.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStrategy.DisplayLayout.Override.GroupByRowAppearance = appearance56;
            appearance57.TextHAlignAsString = "Left";
            this.cmbStrategy.DisplayLayout.Override.HeaderAppearance = appearance57;
            this.cmbStrategy.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbStrategy.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance58.BackColor = System.Drawing.SystemColors.Window;
            appearance58.BorderColor = System.Drawing.Color.Silver;
            this.cmbStrategy.DisplayLayout.Override.RowAppearance = appearance58;
            this.cmbStrategy.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance59.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbStrategy.DisplayLayout.Override.TemplateAddRowAppearance = appearance59;
            this.cmbStrategy.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbStrategy.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbStrategy.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbStrategy.Location = new System.Drawing.Point(105, 353);
            this.cmbStrategy.Name = "cmbStrategy";
            this.cmbStrategy.Size = new System.Drawing.Size(96, 30);
            this.cmbStrategy.TabIndex = 114;
            this.cmbStrategy.Text = "combo strategy";
            this.cmbStrategy.Visible = false;
            // 
            // cmbSymbolConvention
            // 
            appearance60.BackColor = System.Drawing.SystemColors.Window;
            appearance60.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSymbolConvention.DisplayLayout.Appearance = appearance60;
            this.cmbSymbolConvention.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSymbolConvention.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance61.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance61.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance61.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance61.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSymbolConvention.DisplayLayout.GroupByBox.Appearance = appearance61;
            appearance62.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSymbolConvention.DisplayLayout.GroupByBox.BandLabelAppearance = appearance62;
            this.cmbSymbolConvention.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance63.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance63.BackColor2 = System.Drawing.SystemColors.Control;
            appearance63.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance63.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSymbolConvention.DisplayLayout.GroupByBox.PromptAppearance = appearance63;
            this.cmbSymbolConvention.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSymbolConvention.DisplayLayout.MaxRowScrollRegions = 1;
            appearance64.BackColor = System.Drawing.SystemColors.Window;
            appearance64.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSymbolConvention.DisplayLayout.Override.ActiveCellAppearance = appearance64;
            appearance65.BackColor = System.Drawing.SystemColors.Highlight;
            appearance65.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSymbolConvention.DisplayLayout.Override.ActiveRowAppearance = appearance65;
            this.cmbSymbolConvention.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSymbolConvention.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance66.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSymbolConvention.DisplayLayout.Override.CardAreaAppearance = appearance66;
            appearance67.BorderColor = System.Drawing.Color.Silver;
            appearance67.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSymbolConvention.DisplayLayout.Override.CellAppearance = appearance67;
            this.cmbSymbolConvention.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSymbolConvention.DisplayLayout.Override.CellPadding = 0;
            appearance68.BackColor = System.Drawing.SystemColors.Control;
            appearance68.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance68.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance68.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance68.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSymbolConvention.DisplayLayout.Override.GroupByRowAppearance = appearance68;
            appearance69.TextHAlignAsString = "Left";
            this.cmbSymbolConvention.DisplayLayout.Override.HeaderAppearance = appearance69;
            this.cmbSymbolConvention.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSymbolConvention.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance70.BackColor = System.Drawing.SystemColors.Window;
            appearance70.BorderColor = System.Drawing.Color.Silver;
            this.cmbSymbolConvention.DisplayLayout.Override.RowAppearance = appearance70;
            this.cmbSymbolConvention.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance71.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSymbolConvention.DisplayLayout.Override.TemplateAddRowAppearance = appearance71;
            this.cmbSymbolConvention.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSymbolConvention.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSymbolConvention.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSymbolConvention.Location = new System.Drawing.Point(167, 268);
            this.cmbSymbolConvention.Name = "cmbSymbolConvention";
            this.cmbSymbolConvention.Size = new System.Drawing.Size(96, 30);
            this.cmbSymbolConvention.TabIndex = 115;
            this.cmbSymbolConvention.Text = "combo SymbolConvention";
            this.cmbSymbolConvention.Visible = false;
            // 
            // cmbInstrumentType
            // 
            appearance72.BackColor = System.Drawing.SystemColors.Window;
            appearance72.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbInstrumentType.DisplayLayout.Appearance = appearance72;
            this.cmbInstrumentType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbInstrumentType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance73.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance73.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance73.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance73.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbInstrumentType.DisplayLayout.GroupByBox.Appearance = appearance73;
            appearance74.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbInstrumentType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance74;
            this.cmbInstrumentType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance75.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance75.BackColor2 = System.Drawing.SystemColors.Control;
            appearance75.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance75.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbInstrumentType.DisplayLayout.GroupByBox.PromptAppearance = appearance75;
            this.cmbInstrumentType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbInstrumentType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance76.BackColor = System.Drawing.SystemColors.Window;
            appearance76.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbInstrumentType.DisplayLayout.Override.ActiveCellAppearance = appearance76;
            appearance77.BackColor = System.Drawing.SystemColors.Highlight;
            appearance77.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbInstrumentType.DisplayLayout.Override.ActiveRowAppearance = appearance77;
            this.cmbInstrumentType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbInstrumentType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance78.BackColor = System.Drawing.SystemColors.Window;
            this.cmbInstrumentType.DisplayLayout.Override.CardAreaAppearance = appearance78;
            appearance79.BorderColor = System.Drawing.Color.Silver;
            appearance79.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbInstrumentType.DisplayLayout.Override.CellAppearance = appearance79;
            this.cmbInstrumentType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbInstrumentType.DisplayLayout.Override.CellPadding = 0;
            appearance80.BackColor = System.Drawing.SystemColors.Control;
            appearance80.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance80.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance80.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance80.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbInstrumentType.DisplayLayout.Override.GroupByRowAppearance = appearance80;
            appearance81.TextHAlignAsString = "Left";
            this.cmbInstrumentType.DisplayLayout.Override.HeaderAppearance = appearance81;
            this.cmbInstrumentType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbInstrumentType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance82.BackColor = System.Drawing.SystemColors.Window;
            appearance82.BorderColor = System.Drawing.Color.Silver;
            this.cmbInstrumentType.DisplayLayout.Override.RowAppearance = appearance82;
            this.cmbInstrumentType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance83.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbInstrumentType.DisplayLayout.Override.TemplateAddRowAppearance = appearance83;
            this.cmbInstrumentType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbInstrumentType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbInstrumentType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbInstrumentType.Location = new System.Drawing.Point(372, 184);
            this.cmbInstrumentType.Name = "cmbInstrumentType";
            this.cmbInstrumentType.Size = new System.Drawing.Size(96, 30);
            this.cmbInstrumentType.TabIndex = 116;
            this.cmbInstrumentType.Text = "InstrumentType";
            this.cmbInstrumentType.Visible = false;
            // 
            // cmbVenue
            // 
            appearance84.BackColor = System.Drawing.SystemColors.Window;
            appearance84.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbVenue.DisplayLayout.Appearance = appearance84;
            this.cmbVenue.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbVenue.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance85.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance85.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance85.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance85.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.GroupByBox.Appearance = appearance85;
            appearance86.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.BandLabelAppearance = appearance86;
            this.cmbVenue.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance87.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance87.BackColor2 = System.Drawing.SystemColors.Control;
            appearance87.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance87.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.PromptAppearance = appearance87;
            this.cmbVenue.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbVenue.DisplayLayout.MaxRowScrollRegions = 1;
            appearance88.BackColor = System.Drawing.SystemColors.Window;
            appearance88.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbVenue.DisplayLayout.Override.ActiveCellAppearance = appearance88;
            appearance89.BackColor = System.Drawing.SystemColors.Highlight;
            appearance89.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbVenue.DisplayLayout.Override.ActiveRowAppearance = appearance89;
            this.cmbVenue.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbVenue.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance90.BackColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.CardAreaAppearance = appearance90;
            appearance91.BorderColor = System.Drawing.Color.Silver;
            appearance91.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbVenue.DisplayLayout.Override.CellAppearance = appearance91;
            this.cmbVenue.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbVenue.DisplayLayout.Override.CellPadding = 0;
            appearance92.BackColor = System.Drawing.SystemColors.Control;
            appearance92.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance92.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance92.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance92.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.GroupByRowAppearance = appearance92;
            appearance93.TextHAlignAsString = "Left";
            this.cmbVenue.DisplayLayout.Override.HeaderAppearance = appearance93;
            this.cmbVenue.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbVenue.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance94.BackColor = System.Drawing.SystemColors.Window;
            appearance94.BorderColor = System.Drawing.Color.Silver;
            this.cmbVenue.DisplayLayout.Override.RowAppearance = appearance94;
            this.cmbVenue.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance95.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbVenue.DisplayLayout.Override.TemplateAddRowAppearance = appearance95;
            this.cmbVenue.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbVenue.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbVenue.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbVenue.Location = new System.Drawing.Point(372, 268);
            this.cmbVenue.Name = "cmbVenue";
            this.cmbVenue.Size = new System.Drawing.Size(96, 30);
            this.cmbVenue.TabIndex = 117;
            this.cmbVenue.Text = "Venue";
            this.cmbVenue.Visible = false;
            // 
            // ultraTextEditor1
            // 
            editorButton1.Text = "CP";
            this.ultraTextEditor1.ButtonsRight.Add(editorButton1);
            this.ultraTextEditor1.Location = new System.Drawing.Point(50, 236);
            this.ultraTextEditor1.Name = "ultraTextEditor1";
            this.ultraTextEditor1.Size = new System.Drawing.Size(100, 22);
            this.ultraTextEditor1.TabIndex = 119;
            this.ultraTextEditor1.Text = "ultraTextEditor1";
            this.ultraTextEditor1.Visible = false;
            this.ultraTextEditor1.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.ultraTextEditor1_EditorButtonClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "Nirvana Help.chm";
            // 
            // cmbOptionType
            // 
            appearance96.BackColor = System.Drawing.SystemColors.Window;
            appearance96.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbOptionType.DisplayLayout.Appearance = appearance96;
            this.cmbOptionType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOptionType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance97.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance97.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance97.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance97.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOptionType.DisplayLayout.GroupByBox.Appearance = appearance97;
            appearance98.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOptionType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance98;
            this.cmbOptionType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance99.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance99.BackColor2 = System.Drawing.SystemColors.Control;
            appearance99.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance99.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOptionType.DisplayLayout.GroupByBox.PromptAppearance = appearance99;
            this.cmbOptionType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbOptionType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance100.BackColor = System.Drawing.SystemColors.Window;
            appearance100.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbOptionType.DisplayLayout.Override.ActiveCellAppearance = appearance100;
            appearance101.BackColor = System.Drawing.SystemColors.Highlight;
            appearance101.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbOptionType.DisplayLayout.Override.ActiveRowAppearance = appearance101;
            this.cmbOptionType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbOptionType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance102.BackColor = System.Drawing.SystemColors.Window;
            this.cmbOptionType.DisplayLayout.Override.CardAreaAppearance = appearance102;
            appearance103.BorderColor = System.Drawing.Color.Silver;
            appearance103.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbOptionType.DisplayLayout.Override.CellAppearance = appearance103;
            this.cmbOptionType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbOptionType.DisplayLayout.Override.CellPadding = 0;
            appearance104.BackColor = System.Drawing.SystemColors.Control;
            appearance104.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance104.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance104.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance104.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOptionType.DisplayLayout.Override.GroupByRowAppearance = appearance104;
            appearance105.TextHAlignAsString = "Left";
            this.cmbOptionType.DisplayLayout.Override.HeaderAppearance = appearance105;
            this.cmbOptionType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbOptionType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance106.BackColor = System.Drawing.SystemColors.Window;
            appearance106.BorderColor = System.Drawing.Color.Silver;
            this.cmbOptionType.DisplayLayout.Override.RowAppearance = appearance106;
            this.cmbOptionType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance107.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbOptionType.DisplayLayout.Override.TemplateAddRowAppearance = appearance107;
            this.cmbOptionType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbOptionType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbOptionType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbOptionType.Location = new System.Drawing.Point(224, 163);
            this.cmbOptionType.Name = "cmbOptionType";
            this.cmbOptionType.Size = new System.Drawing.Size(96, 30);
            this.cmbOptionType.TabIndex = 121;
            this.cmbOptionType.Text = "combo optiontype";
            this.cmbOptionType.Visible = false;
            // 
            // CtrlCreateAndImportPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbOptionType);
            this.Controls.Add(this.ultraTextEditor1);
            this.Controls.Add(this.cmbVenue);
            this.Controls.Add(this.cmbInstrumentType);
            this.Controls.Add(this.cmbSymbolConvention);
            this.Controls.Add(this.cmbStrategy);
            this.Controls.Add(this.cmbAccounts);
            this.Controls.Add(this.cmbCounterParty);
            this.Controls.Add(this.cmbSide);
            this.Controls.Add(this.grdCreatePosition);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.helpProvider1.SetHelpKeyword(this, "ImportingPositions.html");
            this.helpProvider1.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "CtrlCreateAndImportPosition";
            this.helpProvider1.SetShowHelp(this, true);
            this.Size = new System.Drawing.Size(921, 398);
            ((System.ComponentModel.ISupportInitialize)(this.grdCreatePosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSymbolConvention)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbInstrumentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOptionType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PranaUltraGrid grdCreatePosition;
        private System.Windows.Forms.ContextMenuStrip mnuExerciseAssign;
        private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbSide;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbCounterParty;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbAccounts;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbStrategy;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbSymbolConvention;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbInstrumentType;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbVenue;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditor1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbOptionType;
    }
}
