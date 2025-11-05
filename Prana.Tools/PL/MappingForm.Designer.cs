using Prana.Utilities.UI.UIUtilities;

namespace Prana.Tools
{
    partial class MappingForm
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
                if (ds != null)
                {
                    ds.Dispose();
                }
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
            this.grdData = new PranaUltraGrid();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnAddRow = new Infragistics.Win.Misc.UltraButton();
            this.cmbbxMapping = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbbxPrimeBroker = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label2 = new Infragistics.Win.Misc.UltraLabel();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.MappingForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.upnlbody = new Infragistics.Win.Misc.UltraPanel();
            this.upnlDashboard = new Infragistics.Win.Misc.UltraPanel();
            this.upnlButtons = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._MappingForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MappingForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MappingForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MappingForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.btnMappingUIExport = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxMapping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxPrimeBroker)).BeginInit();
            this.MappingForm_Fill_Panel.ClientArea.SuspendLayout();
            this.MappingForm_Fill_Panel.SuspendLayout();
            this.upnlbody.ClientArea.SuspendLayout();
            this.upnlbody.SuspendLayout();
            this.upnlDashboard.ClientArea.SuspendLayout();
            this.upnlDashboard.SuspendLayout();
            this.upnlButtons.ClientArea.SuspendLayout();
            this.upnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // grdData
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "Tahoma";
            appearance1.FontData.SizeInPoints = 8.25F;
            this.grdData.DisplayLayout.Appearance = appearance1;
            this.grdData.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance2.BackColor = System.Drawing.Color.White;
            this.grdData.DisplayLayout.CaptionAppearance = appearance2;
            this.grdData.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.GroupByBox.Hidden = true;
            this.grdData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdData.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdData.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdData.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdData.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdData.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.AllowMultiCellOperations = ((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation)((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Copy | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Paste)));
            this.grdData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this.grdData.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.FontData.BoldAsString = "False";
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8.25F;
            this.grdData.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdData.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.OncePerGroupedRowIsland;
            this.grdData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.grdData.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            this.grdData.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdData.Location = new System.Drawing.Point(0, 0);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(626, 286);
            this.grdData.TabIndex = 20;
            this.grdData.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdData.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdData_AfterCellUpdate);
            this.grdData.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdData_InitializeRow);
            this.grdData.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.grdData_BeforeExitEditMode);
            this.grdData.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdData_BeforeCustomRowFilterDialog);
            this.grdData.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdData_BeforeColumnChooserDisplayed);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(358, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(73, 22);
            this.btnSave.TabIndex = 121;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddRow
            // 
            this.btnAddRow.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddRow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRow.Location = new System.Drawing.Point(196, 9);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(73, 22);
            this.btnAddRow.TabIndex = 122;
            this.btnAddRow.Text = "AddRow";
            this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // cmbbxMapping
            // 
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxMapping.DisplayLayout.Appearance = appearance5;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbbxMapping.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbbxMapping.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxMapping.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cmbbxMapping.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
            appearance6.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxMapping.DisplayLayout.GroupByBox.Appearance = appearance6;
            appearance7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxMapping.DisplayLayout.GroupByBox.BandLabelAppearance = appearance7;
            this.cmbbxMapping.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance8.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance8.BackColor2 = System.Drawing.SystemColors.Control;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance8.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxMapping.DisplayLayout.GroupByBox.PromptAppearance = appearance8;
            this.cmbbxMapping.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxMapping.DisplayLayout.MaxRowScrollRegions = 1;
            appearance9.BackColor = System.Drawing.SystemColors.Window;
            appearance9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxMapping.DisplayLayout.Override.ActiveCellAppearance = appearance9;
            appearance10.BackColor = System.Drawing.SystemColors.Highlight;
            appearance10.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxMapping.DisplayLayout.Override.ActiveRowAppearance = appearance10;
            this.cmbbxMapping.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxMapping.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxMapping.DisplayLayout.Override.CardAreaAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            appearance12.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxMapping.DisplayLayout.Override.CellAppearance = appearance12;
            this.cmbbxMapping.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxMapping.DisplayLayout.Override.CellPadding = 0;
            appearance13.BackColor = System.Drawing.SystemColors.Control;
            appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance13.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance13.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxMapping.DisplayLayout.Override.GroupByRowAppearance = appearance13;
            appearance14.TextHAlignAsString = "Left";
            this.cmbbxMapping.DisplayLayout.Override.HeaderAppearance = appearance14;
            this.cmbbxMapping.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxMapping.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance15.BackColor = System.Drawing.SystemColors.Window;
            appearance15.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxMapping.DisplayLayout.Override.RowAppearance = appearance15;
            this.cmbbxMapping.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxMapping.DisplayLayout.Override.TemplateAddRowAppearance = appearance16;
            this.cmbbxMapping.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxMapping.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxMapping.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxMapping.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxMapping.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbbxMapping.Location = new System.Drawing.Point(148, 8);
            this.cmbbxMapping.Name = "cmbbxMapping";
            this.cmbbxMapping.Size = new System.Drawing.Size(80, 23);
            this.cmbbxMapping.TabIndex = 123;
            this.cmbbxMapping.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxMapping.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.cmbbxMapping_InitializeRow);
            this.cmbbxMapping.ValueChanged += new System.EventHandler(this.cmbbxMapping_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 18);
            this.label1.TabIndex = 124;
            this.label1.Text = "Select Mapping";
            // 
            // cmbbxPrimeBroker
            // 
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxPrimeBroker.DisplayLayout.Appearance = appearance17;
            this.cmbbxPrimeBroker.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxPrimeBroker.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance18.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance18.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxPrimeBroker.DisplayLayout.GroupByBox.Appearance = appearance18;
            appearance19.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxPrimeBroker.DisplayLayout.GroupByBox.BandLabelAppearance = appearance19;
            this.cmbbxPrimeBroker.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance20.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance20.BackColor2 = System.Drawing.SystemColors.Control;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance20.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxPrimeBroker.DisplayLayout.GroupByBox.PromptAppearance = appearance20;
            this.cmbbxPrimeBroker.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxPrimeBroker.DisplayLayout.MaxRowScrollRegions = 1;
            appearance21.BackColor = System.Drawing.SystemColors.Window;
            appearance21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxPrimeBroker.DisplayLayout.Override.ActiveCellAppearance = appearance21;
            appearance22.BackColor = System.Drawing.SystemColors.Highlight;
            appearance22.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxPrimeBroker.DisplayLayout.Override.ActiveRowAppearance = appearance22;
            this.cmbbxPrimeBroker.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxPrimeBroker.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxPrimeBroker.DisplayLayout.Override.CardAreaAppearance = appearance23;
            appearance24.BorderColor = System.Drawing.Color.Silver;
            appearance24.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxPrimeBroker.DisplayLayout.Override.CellAppearance = appearance24;
            this.cmbbxPrimeBroker.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxPrimeBroker.DisplayLayout.Override.CellPadding = 0;
            appearance25.BackColor = System.Drawing.SystemColors.Control;
            appearance25.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance25.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance25.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxPrimeBroker.DisplayLayout.Override.GroupByRowAppearance = appearance25;
            appearance26.TextHAlignAsString = "Left";
            this.cmbbxPrimeBroker.DisplayLayout.Override.HeaderAppearance = appearance26;
            this.cmbbxPrimeBroker.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxPrimeBroker.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance27.BackColor = System.Drawing.SystemColors.Window;
            appearance27.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxPrimeBroker.DisplayLayout.Override.RowAppearance = appearance27;
            this.cmbbxPrimeBroker.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxPrimeBroker.DisplayLayout.Override.TemplateAddRowAppearance = appearance28;
            this.cmbbxPrimeBroker.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxPrimeBroker.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxPrimeBroker.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxPrimeBroker.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxPrimeBroker.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbbxPrimeBroker.Location = new System.Drawing.Point(376, 7);
            this.cmbbxPrimeBroker.Name = "cmbbxPrimeBroker";
            this.cmbbxPrimeBroker.Size = new System.Drawing.Size(75, 25);
            this.cmbbxPrimeBroker.TabIndex = 125;
            this.cmbbxPrimeBroker.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.cmbbxPrimeBroker_InitializeRow);
            this.cmbbxPrimeBroker.ValueChanged += new System.EventHandler(this.cmbbxPrimeBroker_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(267, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 18);
            this.label2.TabIndex = 126;
            this.label2.Text = "Prime Broker";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(277, 9);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(73, 22);
            this.btnDelete.TabIndex = 127;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // MappingForm_Fill_Panel
            // 
            // 
            // MappingForm_Fill_Panel.ClientArea
            // 
            this.MappingForm_Fill_Panel.ClientArea.Controls.Add(this.upnlbody);
            this.MappingForm_Fill_Panel.ClientArea.Controls.Add(this.upnlDashboard);
            this.MappingForm_Fill_Panel.ClientArea.Controls.Add(this.upnlButtons);
            this.MappingForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.MappingForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MappingForm_Fill_Panel.Location = new System.Drawing.Point(8, 31);
            this.MappingForm_Fill_Panel.Name = "MappingForm_Fill_Panel";
            this.MappingForm_Fill_Panel.Size = new System.Drawing.Size(626, 365);
            this.MappingForm_Fill_Panel.TabIndex = 0;
            // 
            // upnlbody
            // 
            // 
            // upnlbody.ClientArea
            // 
            this.upnlbody.ClientArea.Controls.Add(this.grdData);
            this.upnlbody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upnlbody.Location = new System.Drawing.Point(0, 38);
            this.upnlbody.Name = "upnlbody";
            this.upnlbody.Size = new System.Drawing.Size(626, 286);
            this.upnlbody.TabIndex = 128;
            // 
            // upnlDashboard
            // 
            // 
            // upnlDashboard.ClientArea
            // 
            this.upnlDashboard.ClientArea.Controls.Add(this.btnMappingUIExport);
            this.upnlDashboard.ClientArea.Controls.Add(this.label1);
            this.upnlDashboard.ClientArea.Controls.Add(this.cmbbxMapping);
            this.upnlDashboard.ClientArea.Controls.Add(this.cmbbxPrimeBroker);
            this.upnlDashboard.ClientArea.Controls.Add(this.label2);
            this.upnlDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.upnlDashboard.Location = new System.Drawing.Point(0, 0);
            this.upnlDashboard.Name = "upnlDashboard";
            this.upnlDashboard.Size = new System.Drawing.Size(626, 38);
            this.upnlDashboard.TabIndex = 130;
            // 
            // upnlButtons
            // 
            // 
            // upnlButtons.ClientArea
            // 
            this.upnlButtons.ClientArea.Controls.Add(this.btnDelete);
            this.upnlButtons.ClientArea.Controls.Add(this.btnSave);
            this.upnlButtons.ClientArea.Controls.Add(this.btnAddRow);
            this.upnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.upnlButtons.Location = new System.Drawing.Point(0, 324);
            this.upnlButtons.Name = "upnlButtons";
            this.upnlButtons.Size = new System.Drawing.Size(626, 41);
            this.upnlButtons.TabIndex = 129;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _MappingForm_UltraFormManager_Dock_Area_Left
            // 
            this._MappingForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MappingForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MappingForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._MappingForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MappingForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._MappingForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._MappingForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._MappingForm_UltraFormManager_Dock_Area_Left.Name = "_MappingForm_UltraFormManager_Dock_Area_Left";
            this._MappingForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 365);
            // 
            // _MappingForm_UltraFormManager_Dock_Area_Right
            // 
            this._MappingForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MappingForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MappingForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._MappingForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MappingForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._MappingForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._MappingForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(634, 31);
            this._MappingForm_UltraFormManager_Dock_Area_Right.Name = "_MappingForm_UltraFormManager_Dock_Area_Right";
            this._MappingForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 365);
            // 
            // _MappingForm_UltraFormManager_Dock_Area_Top
            // 
            this._MappingForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MappingForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MappingForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._MappingForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MappingForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._MappingForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._MappingForm_UltraFormManager_Dock_Area_Top.Name = "_MappingForm_UltraFormManager_Dock_Area_Top";
            this._MappingForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(642, 31);
            // 
            // _MappingForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._MappingForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MappingForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MappingForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._MappingForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MappingForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._MappingForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._MappingForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 396);
            this._MappingForm_UltraFormManager_Dock_Area_Bottom.Name = "_MappingForm_UltraFormManager_Dock_Area_Bottom";
            this._MappingForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(642, 8);
            // 
            // btnMappingUIExport
            // 
            this.btnMappingUIExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMappingUIExport.Location = new System.Drawing.Point(501, 7);
            this.btnMappingUIExport.Name = "btnMappingUIExport";
            this.btnMappingUIExport.Size = new System.Drawing.Size(100, 24);
            this.btnMappingUIExport.TabIndex = 127;
            this.btnMappingUIExport.Text = "Export to Excel";
            this.btnMappingUIExport.Click += new System.EventHandler(this.btnMappingUIExport_Click);
            this.btnMappingUIExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
            this.btnMappingUIExport.ForeColor = System.Drawing.Color.White;
            this.btnMappingUIExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.btnMappingUIExport.UseAppStyling = false;
            this.btnMappingUIExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // MappingForm
            // 
            this.ClientSize = new System.Drawing.Size(642, 404);
            this.Controls.Add(this.MappingForm_Fill_Panel);
            this.Controls.Add(this._MappingForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._MappingForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._MappingForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._MappingForm_UltraFormManager_Dock_Area_Bottom);
            this.Name = "MappingForm";
            this.ShowIcon = false;
            this.Text = "Mapping Form";
            this.Load += new System.EventHandler(this.MappingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxMapping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxPrimeBroker)).EndInit();
            this.MappingForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.MappingForm_Fill_Panel.ResumeLayout(false);
            this.upnlbody.ClientArea.ResumeLayout(false);
            this.upnlbody.ResumeLayout(false);
            this.upnlDashboard.ClientArea.ResumeLayout(false);
            this.upnlDashboard.ClientArea.PerformLayout();
            this.upnlDashboard.ResumeLayout(false);
            this.upnlButtons.ClientArea.ResumeLayout(false);
            this.upnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PranaUltraGrid grdData;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnAddRow;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxMapping;
        private Infragistics.Win.Misc.UltraLabel label1;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxPrimeBroker;
        private Infragistics.Win.Misc.UltraLabel label2;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraPanel MappingForm_Fill_Panel;
        private Infragistics.Win.Misc.UltraPanel upnlbody;
        private Infragistics.Win.Misc.UltraPanel upnlDashboard;
        private Infragistics.Win.Misc.UltraPanel upnlButtons;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MappingForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MappingForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MappingForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MappingForm_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraButton btnMappingUIExport;
    }
}