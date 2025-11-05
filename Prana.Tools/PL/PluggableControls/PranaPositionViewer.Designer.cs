using Prana.Utilities.UI.UIUtilities;
namespace Prana.Tools
{
    partial class PranaPositionViewer
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
                if (_bgGetData != null)
                {
                    _bgGetData.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
            this.grdData = new PranaUltraGrid();
            this.cmbbxReconTemplates = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.dtToDatePicker = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.ultraExpandableGroupBox2 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.cmbbxClient = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.dtFromDatePicker = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.cmbbxReconType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxReconTemplates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDatePicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).BeginInit();
            this.ultraExpandableGroupBox2.SuspendLayout();
            this.ultraExpandableGroupBoxPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDatePicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxReconType)).BeginInit();
            this.SuspendLayout();
            // 
            // grdData
            // 
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "Tahoma";
            appearance1.FontData.SizeInPoints = 8.25F;
            this.grdData.DisplayLayout.Appearance = appearance1;
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
            this.grdData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdData.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.FontData.BoldAsString = "False";
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8.25F;
            this.grdData.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdData.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.OncePerGroupedRowIsland;
            this.grdData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.grdData.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            this.grdData.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdData.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdData.Location = new System.Drawing.Point(-1, -3);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(690, 361);
            this.grdData.TabIndex = 19;
            this.grdData.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdData.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdData_InitializeLayout);
            this.grdData.AfterRowFilterChanged += new Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventHandler(this.grdData_AfterRowFilterChanged);
            this.grdData.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdData_BeforeCustomRowFilterDialog);
            this.grdData.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdData_BeforeColumnChooserDisplayed);
            // 
            // cmbbxReconTemplates
            // 
            this.cmbbxReconTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance5.FontData.SizeInPoints = 10F;
            this.cmbbxReconTemplates.Appearance = appearance5;
            this.cmbbxReconTemplates.AutoSize = false;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxReconTemplates.DisplayLayout.Appearance = appearance6;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbbxReconTemplates.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbbxReconTemplates.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxReconTemplates.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cmbbxReconTemplates.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
            appearance7.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance7.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxReconTemplates.DisplayLayout.GroupByBox.Appearance = appearance7;
            appearance8.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxReconTemplates.DisplayLayout.GroupByBox.BandLabelAppearance = appearance8;
            this.cmbbxReconTemplates.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance9.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance9.BackColor2 = System.Drawing.SystemColors.Control;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxReconTemplates.DisplayLayout.GroupByBox.PromptAppearance = appearance9;
            this.cmbbxReconTemplates.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxReconTemplates.DisplayLayout.MaxRowScrollRegions = 1;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxReconTemplates.DisplayLayout.Override.ActiveCellAppearance = appearance10;
            appearance11.BackColor = System.Drawing.SystemColors.Highlight;
            appearance11.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxReconTemplates.DisplayLayout.Override.ActiveRowAppearance = appearance11;
            this.cmbbxReconTemplates.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxReconTemplates.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxReconTemplates.DisplayLayout.Override.CardAreaAppearance = appearance12;
            appearance13.BorderColor = System.Drawing.Color.Silver;
            appearance13.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxReconTemplates.DisplayLayout.Override.CellAppearance = appearance13;
            this.cmbbxReconTemplates.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxReconTemplates.DisplayLayout.Override.CellPadding = 0;
            appearance14.BackColor = System.Drawing.SystemColors.Control;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxReconTemplates.DisplayLayout.Override.GroupByRowAppearance = appearance14;
            appearance15.TextHAlignAsString = "Left";
            this.cmbbxReconTemplates.DisplayLayout.Override.HeaderAppearance = appearance15;
            this.cmbbxReconTemplates.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxReconTemplates.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            appearance16.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxReconTemplates.DisplayLayout.Override.RowAppearance = appearance16;
            this.cmbbxReconTemplates.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance17.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxReconTemplates.DisplayLayout.Override.TemplateAddRowAppearance = appearance17;
            this.cmbbxReconTemplates.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxReconTemplates.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxReconTemplates.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxReconTemplates.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxReconTemplates.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbbxReconTemplates.Location = new System.Drawing.Point(453, 362);
            this.cmbbxReconTemplates.Name = "cmbbxReconTemplates";
            this.cmbbxReconTemplates.Size = new System.Drawing.Size(141, 21);
            this.cmbbxReconTemplates.TabIndex = 113;
            this.cmbbxReconTemplates.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxReconTemplates.BeforeDropDown += new System.ComponentModel.CancelEventHandler(this.cmbbxReconTemplates_BeforeDropDown);
            this.cmbbxReconTemplates.ValueChanged += new System.EventHandler(this.cmbbxReconTemplates_ValueChanged);
            // 
            // dtToDatePicker
            // 
            this.dtToDatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance18.FontData.SizeInPoints = 10F;
            this.dtToDatePicker.Appearance = appearance18;
            this.dtToDatePicker.AutoSize = false;
            this.dtToDatePicker.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtToDatePicker.Location = new System.Drawing.Point(122, 362);
            this.dtToDatePicker.Name = "dtToDatePicker";
            this.dtToDatePicker.Size = new System.Drawing.Size(109, 21);
            this.dtToDatePicker.TabIndex = 115;
            this.dtToDatePicker.ValueChanged += new System.EventHandler(this.dtDatePicker_ValueChanged);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance19.FontData.SizeInPoints = 10F;
            this.btnView.Appearance = appearance19;
            this.btnView.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnView.Location = new System.Drawing.Point(607, 362);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(76, 21);
            this.btnView.TabIndex = 20;
            this.btnView.Text = "View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // ultraExpandableGroupBox2
            // 
            appearance20.FontData.SizeInPoints = 10F;
            this.ultraExpandableGroupBox2.Appearance = appearance20;
            this.ultraExpandableGroupBox2.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.ultraExpandableGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox2.ExpandedSize = new System.Drawing.Size(713, 392);
            this.ultraExpandableGroupBox2.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            this.ultraExpandableGroupBox2.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.RightOnBorder;
            this.ultraExpandableGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox2.Name = "ultraExpandableGroupBox2";
            this.ultraExpandableGroupBox2.Size = new System.Drawing.Size(713, 392);
            this.ultraExpandableGroupBox2.TabIndex = 120;
            this.ultraExpandableGroupBox2.Text = "Application Data";
            this.ultraExpandableGroupBox2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.cmbbxClient);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.dtFromDatePicker);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.dtToDatePicker);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.cmbbxReconType);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.grdData);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.btnView);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.cmbbxReconTemplates);
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(3, 3);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(689, 386);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // cmbbxClient
            // 
            this.cmbbxClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance21.FontData.SizeInPoints = 10F;
            this.cmbbxClient.Appearance = appearance21;
            this.cmbbxClient.AutoSize = false;
            appearance22.BackColor = System.Drawing.SystemColors.Window;
            appearance22.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxClient.DisplayLayout.Appearance = appearance22;
            this.cmbbxClient.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxClient.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance23.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance23.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxClient.DisplayLayout.GroupByBox.Appearance = appearance23;
            appearance24.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxClient.DisplayLayout.GroupByBox.BandLabelAppearance = appearance24;
            this.cmbbxClient.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance25.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance25.BackColor2 = System.Drawing.SystemColors.Control;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance25.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxClient.DisplayLayout.GroupByBox.PromptAppearance = appearance25;
            this.cmbbxClient.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxClient.DisplayLayout.MaxRowScrollRegions = 1;
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            appearance26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxClient.DisplayLayout.Override.ActiveCellAppearance = appearance26;
            appearance27.BackColor = System.Drawing.SystemColors.Highlight;
            appearance27.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxClient.DisplayLayout.Override.ActiveRowAppearance = appearance27;
            this.cmbbxClient.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxClient.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance28.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxClient.DisplayLayout.Override.CardAreaAppearance = appearance28;
            appearance29.BorderColor = System.Drawing.Color.Silver;
            appearance29.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxClient.DisplayLayout.Override.CellAppearance = appearance29;
            this.cmbbxClient.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxClient.DisplayLayout.Override.CellPadding = 0;
            appearance30.BackColor = System.Drawing.SystemColors.Control;
            appearance30.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance30.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance30.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxClient.DisplayLayout.Override.GroupByRowAppearance = appearance30;
            appearance31.TextHAlignAsString = "Left";
            this.cmbbxClient.DisplayLayout.Override.HeaderAppearance = appearance31;
            this.cmbbxClient.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxClient.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxClient.DisplayLayout.Override.RowAppearance = appearance32;
            this.cmbbxClient.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance33.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxClient.DisplayLayout.Override.TemplateAddRowAppearance = appearance33;
            this.cmbbxClient.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxClient.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxClient.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxClient.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxClient.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbbxClient.Location = new System.Drawing.Point(239, 362);
            this.cmbbxClient.Name = "cmbbxClient";
            this.cmbbxClient.Size = new System.Drawing.Size(99, 21);
            this.cmbbxClient.TabIndex = 118;
            this.cmbbxClient.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxClient.ValueChanged += new System.EventHandler(this.cmbbxClient_ValueChanged);
            // 
            // dtFromDatePicker
            // 
            this.dtFromDatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance34.FontData.SizeInPoints = 10F;
            this.dtFromDatePicker.Appearance = appearance34;
            this.dtFromDatePicker.AutoSize = false;
            this.dtFromDatePicker.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFromDatePicker.Location = new System.Drawing.Point(5, 362);
            this.dtFromDatePicker.Name = "dtFromDatePicker";
            this.dtFromDatePicker.Size = new System.Drawing.Size(109, 21);
            this.dtFromDatePicker.TabIndex = 117;
            this.dtFromDatePicker.ValueChanged += new System.EventHandler(this.dtFromDatePicker_ValueChanged);
            // 
            // cmbbxReconType
            // 
            this.cmbbxReconType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance35.FontData.SizeInPoints = 10F;
            this.cmbbxReconType.Appearance = appearance35;
            this.cmbbxReconType.AutoSize = false;
            appearance36.BackColor = System.Drawing.SystemColors.Window;
            appearance36.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxReconType.DisplayLayout.Appearance = appearance36;
            ultraGridBand2.ColHeadersVisible = false;
            this.cmbbxReconType.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbbxReconType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxReconType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cmbbxReconType.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
            appearance37.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance37.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance37.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxReconType.DisplayLayout.GroupByBox.Appearance = appearance37;
            appearance38.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxReconType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance38;
            this.cmbbxReconType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance39.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance39.BackColor2 = System.Drawing.SystemColors.Control;
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxReconType.DisplayLayout.GroupByBox.PromptAppearance = appearance39;
            this.cmbbxReconType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxReconType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance40.BackColor = System.Drawing.SystemColors.Window;
            appearance40.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxReconType.DisplayLayout.Override.ActiveCellAppearance = appearance40;
            appearance41.BackColor = System.Drawing.SystemColors.Highlight;
            appearance41.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxReconType.DisplayLayout.Override.ActiveRowAppearance = appearance41;
            this.cmbbxReconType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxReconType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance42.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxReconType.DisplayLayout.Override.CardAreaAppearance = appearance42;
            appearance43.BorderColor = System.Drawing.Color.Silver;
            appearance43.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxReconType.DisplayLayout.Override.CellAppearance = appearance43;
            this.cmbbxReconType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxReconType.DisplayLayout.Override.CellPadding = 0;
            appearance44.BackColor = System.Drawing.SystemColors.Control;
            appearance44.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance44.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance44.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxReconType.DisplayLayout.Override.GroupByRowAppearance = appearance44;
            appearance45.TextHAlignAsString = "Left";
            this.cmbbxReconType.DisplayLayout.Override.HeaderAppearance = appearance45;
            this.cmbbxReconType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxReconType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance46.BackColor = System.Drawing.SystemColors.Window;
            appearance46.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxReconType.DisplayLayout.Override.RowAppearance = appearance46;
            this.cmbbxReconType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance47.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxReconType.DisplayLayout.Override.TemplateAddRowAppearance = appearance47;
            this.cmbbxReconType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxReconType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxReconType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxReconType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxReconType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbbxReconType.Location = new System.Drawing.Point(346, 362);
            this.cmbbxReconType.Name = "cmbbxReconType";
            this.cmbbxReconType.Size = new System.Drawing.Size(99, 21);
            this.cmbbxReconType.TabIndex = 116;
            this.cmbbxReconType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxReconType.ValueChanged += new System.EventHandler(this.cmbbxReconType_ValueChanged);
            // 
            // PranaPositionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraExpandableGroupBox2);
            this.Name = "PranaPositionViewer";
            this.Size = new System.Drawing.Size(713, 392);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxReconTemplates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDatePicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).EndInit();
            this.ultraExpandableGroupBox2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDatePicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxReconType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PranaUltraGrid grdData;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxReconTemplates;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDatePicker;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox2;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxReconType;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDatePicker;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxClient;
    }
}