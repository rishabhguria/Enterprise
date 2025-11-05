using Prana.Utilities.UI.UIUtilities;
namespace Prana.Tools.PL.Controls
{
    partial class ctrlApproveChanges
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
                if (_bgApproveClickWorker != null)
                {
                    _bgApproveClickWorker.Dispose();
                }
                if (!CustomThemeHelper.IsDesignMode())
                {
                    _pricingServicesProxy.Dispose();
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
            this.grpApproveChanges = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnGetUnapprovedChanges = new Infragistics.Win.Misc.UltraButton();
            this.lblApproveType = new Infragistics.Win.Misc.UltraLabel();
            this.cmbReconType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbGroup = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.dtStartDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtEndDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblStartDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblEndDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblGroup = new Infragistics.Win.Misc.UltraLabel();
            this.grdApproveChange = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnApprove = new Infragistics.Win.Misc.UltraButton();
            this.btnRescindChanges = new Infragistics.Win.Misc.UltraButton();
            this.btnExportExcel = new Infragistics.Win.Misc.UltraButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ultraStatusBar1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.grpApproveChanges)).BeginInit();
            this.grpApproveChanges.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReconType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdApproveChange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpApproveChanges
            // 
            this.grpApproveChanges.Controls.Add(this.btnGetUnapprovedChanges);
            this.grpApproveChanges.Controls.Add(this.lblApproveType);
            this.grpApproveChanges.Controls.Add(this.cmbReconType);
            this.grpApproveChanges.Controls.Add(this.cmbGroup);
            this.grpApproveChanges.Controls.Add(this.dtStartDate);
            this.grpApproveChanges.Controls.Add(this.dtEndDate);
            this.grpApproveChanges.Controls.Add(this.btnView);
            this.grpApproveChanges.Controls.Add(this.lblStartDate);
            this.grpApproveChanges.Controls.Add(this.lblEndDate);
            this.grpApproveChanges.Controls.Add(this.lblGroup);
            this.grpApproveChanges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpApproveChanges.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpApproveChanges.Location = new System.Drawing.Point(0, 0);
            this.grpApproveChanges.Name = "grpApproveChanges";
            this.grpApproveChanges.Size = new System.Drawing.Size(972, 64);
            this.grpApproveChanges.TabIndex = 0;
            this.grpApproveChanges.Text = "Approve Changes";
            // 
            // btnGetUnapprovedChanges
            // 
            this.btnGetUnapprovedChanges.Location = new System.Drawing.Point(745, 26);
            this.btnGetUnapprovedChanges.Name = "btnGetUnapprovedChanges";
            this.btnGetUnapprovedChanges.Size = new System.Drawing.Size(169, 23);
            this.btnGetUnapprovedChanges.TabIndex = 7;
            this.btnGetUnapprovedChanges.Text = "Get UnApproved Changes";
            this.btnGetUnapprovedChanges.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblApproveType
            // 
            this.lblApproveType.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblApproveType.Location = new System.Drawing.Point(491, 26);
            this.lblApproveType.Name = "lblApproveType";
            this.lblApproveType.Size = new System.Drawing.Size(35, 22);
            this.lblApproveType.TabIndex = 6;
            this.lblApproveType.Text = "Type";
            // 
            // cmbReconType
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbReconType.DisplayLayout.Appearance = appearance1;
            this.cmbReconType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbReconType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbReconType.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbReconType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbReconType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbReconType.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbReconType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbReconType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbReconType.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbReconType.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbReconType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbReconType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbReconType.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbReconType.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbReconType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbReconType.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbReconType.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbReconType.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbReconType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbReconType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbReconType.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbReconType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbReconType.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbReconType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbReconType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbReconType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbReconType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbReconType.Location = new System.Drawing.Point(529, 26);
            this.cmbReconType.Name = "cmbReconType";
            this.cmbReconType.Size = new System.Drawing.Size(100, 23);
            this.cmbReconType.TabIndex = 5;
            // 
            // cmbGroup
            // 
            this.cmbGroup.AutoSize = false;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbGroup.DisplayLayout.Appearance = appearance13;
            this.cmbGroup.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbGroup.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbGroup.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbGroup.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbGroup.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbGroup.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbGroup.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbGroup.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbGroup.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbGroup.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbGroup.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbGroup.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbGroup.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbGroup.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbGroup.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbGroup.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbGroup.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.cmbGroup.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbGroup.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbGroup.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbGroup.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbGroup.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbGroup.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbGroup.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbGroup.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbGroup.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbGroup.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cmbGroup.Location = new System.Drawing.Point(385, 26);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(100, 22);
            this.cmbGroup.TabIndex = 2;
            this.cmbGroup.Text = "Group";
            // 
            // dtStartDate
            // 
            this.dtStartDate.AutoSize = false;
            this.dtStartDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.dtStartDate.Location = new System.Drawing.Point(71, 26);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(100, 21);
            this.dtStartDate.TabIndex = 2;
            // 
            // dtEndDate
            // 
            this.dtEndDate.AutoSize = false;
            this.dtEndDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.dtEndDate.Location = new System.Drawing.Point(234, 26);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(100, 21);
            this.dtEndDate.TabIndex = 3;
            // 
            // btnView
            // 
            this.btnView.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnView.Location = new System.Drawing.Point(639, 26);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(100, 23);
            this.btnView.TabIndex = 4;
            this.btnView.Text = "View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblStartDate
            // 
            this.lblStartDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblStartDate.Location = new System.Drawing.Point(6, 26);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(100, 23);
            this.lblStartDate.TabIndex = 1;
            this.lblStartDate.Text = "Start Date";
            // 
            // lblEndDate
            // 
            this.lblEndDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblEndDate.Location = new System.Drawing.Point(177, 26);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(100, 23);
            this.lblEndDate.TabIndex = 2;
            this.lblEndDate.Text = "End Date";
            // 
            // lblGroup
            // 
            this.lblGroup.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblGroup.Location = new System.Drawing.Point(344, 26);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(100, 22);
            this.lblGroup.TabIndex = 3;
            this.lblGroup.Text = "Group";
            // 
            // grdApproveChange
            // 
            appearance25.BackColor = System.Drawing.Color.Black;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdApproveChange.DisplayLayout.Appearance = appearance25;
            this.grdApproveChange.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdApproveChange.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.grdApproveChange.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdApproveChange.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.grdApproveChange.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdApproveChange.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.grdApproveChange.DisplayLayout.MaxColScrollRegions = 1;
            this.grdApproveChange.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdApproveChange.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdApproveChange.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.grdApproveChange.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdApproveChange.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.grdApproveChange.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdApproveChange.DisplayLayout.Override.CellAppearance = appearance32;
            this.grdApproveChange.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdApproveChange.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.grdApproveChange.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlignAsString = "Left";
            this.grdApproveChange.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.grdApproveChange.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdApproveChange.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.grdApproveChange.DisplayLayout.Override.RowAppearance = appearance35;
            this.grdApproveChange.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdApproveChange.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.grdApproveChange.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdApproveChange.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdApproveChange.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdApproveChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdApproveChange.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grdApproveChange.Location = new System.Drawing.Point(0, 0);
            this.grdApproveChange.Name = "grdApproveChange";
            this.grdApproveChange.Size = new System.Drawing.Size(972, 355);
            this.grdApproveChange.TabIndex = 1;
            this.grdApproveChange.Text = "ultraGrid1";
            this.grdApproveChange.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdApproveChange_InitializeLayout);
            this.grdApproveChange.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdApproveChange_InitializeRow);
            this.grdApproveChange.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdApproveChange_BeforeCellUpdate);
            this.grdApproveChange.FilterCellValueChanged += new Infragistics.Win.UltraWinGrid.FilterCellValueChangedEventHandler(this.grdApproveChange_FilterCellValueChanged);
            this.grdApproveChange.BeforeHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventHandler(this.grdApproveChange_BeforeHeaderCheckStateChanged);
            this.grdApproveChange.AfterHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventHandler(this.grdApproveChange_AfterHeaderCheckStateChanged);
            // 
            // btnApprove
            // 
            this.btnApprove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnApprove.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnApprove.Location = new System.Drawing.Point(414, -2);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 24);
            this.btnApprove.TabIndex = 2;
            this.btnApprove.Text = "Approve";
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnRescindChanges
            // 
            this.btnRescindChanges.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRescindChanges.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnRescindChanges.Location = new System.Drawing.Point(511, -2);
            this.btnRescindChanges.Name = "btnRescindChanges";
            this.btnRescindChanges.Size = new System.Drawing.Size(118, 24);
            this.btnRescindChanges.TabIndex = 3;
            this.btnRescindChanges.Text = "Rescind Changes";
            this.btnRescindChanges.Click += new System.EventHandler(this.btnRescindChanges_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExportExcel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnExportExcel.Location = new System.Drawing.Point(317, -2);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(75, 24);
            this.btnExportExcel.TabIndex = 4;
            this.btnExportExcel.Text = "Export";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ultraStatusBar1);
            this.splitContainer1.Panel2.Controls.Add(this.btnRescindChanges);
            this.splitContainer1.Panel2.Controls.Add(this.btnExportExcel);
            this.splitContainer1.Panel2.Controls.Add(this.btnApprove);
            this.splitContainer1.Size = new System.Drawing.Size(972, 476);
            this.splitContainer1.SplitterDistance = 423;
            this.splitContainer1.TabIndex = 7;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.grpApproveChanges);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.grdApproveChange);
            this.splitContainer2.Size = new System.Drawing.Size(972, 423);
            this.splitContainer2.SplitterDistance = 64;
            this.splitContainer2.TabIndex = 2;
            // 
            // ultraStatusBar1
            // 
            this.ultraStatusBar1.Location = new System.Drawing.Point(0, 26);
            this.ultraStatusBar1.Name = "ultraStatusBar1";
            this.ultraStatusBar1.Size = new System.Drawing.Size(972, 23);
            this.ultraStatusBar1.TabIndex = 5;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(972, 476);
            this.ultraPanel1.TabIndex = 8;
            // 
            // ctrlApproveChanges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlApproveChanges";
            this.Size = new System.Drawing.Size(972, 476);
            ((System.ComponentModel.ISupportInitialize)(this.grpApproveChanges)).EndInit();
            this.grpApproveChanges.ResumeLayout(false);
            this.grpApproveChanges.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReconType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdApproveChange)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox grpApproveChanges;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbGroup;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtStartDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtEndDate;
        protected Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraLabel lblStartDate;
        private Infragistics.Win.Misc.UltraLabel lblEndDate;
        private Infragistics.Win.Misc.UltraLabel lblGroup;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdApproveChange;
        public Infragistics.Win.Misc.UltraButton btnApprove;
        public Infragistics.Win.Misc.UltraButton btnRescindChanges;
        public Infragistics.Win.Misc.UltraButton btnExportExcel;
        private Infragistics.Win.Misc.UltraLabel lblApproveType;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbReconType;
        protected Infragistics.Win.Misc.UltraButton btnGetUnapprovedChanges;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar1;

    }
}
