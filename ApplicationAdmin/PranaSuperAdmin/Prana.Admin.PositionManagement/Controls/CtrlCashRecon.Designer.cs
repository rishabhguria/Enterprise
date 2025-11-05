namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlCashRecon
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
            _isInitialized = false;
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlCashRecon));
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
            this.btnRunReconciliation = new Infragistics.Win.Misc.UltraButton();
            this.cmbDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblDate = new Infragistics.Win.Misc.UltraLabel();
            this.ctrlSourceName1 = new Nirvana.Admin.PositionManagement.Controls.CtrlSourceName();
            this.grdCashRecon = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnAcceptSource = new Infragistics.Win.Misc.UltraButton();
            this.btnAcceptApplication = new Infragistics.Win.Misc.UltraButton();
            this.btnAcceptManual = new Infragistics.Win.Misc.UltraButton();
            this.cmbReconStatus = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.grpDataSourceDetails = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCashRecon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReconStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpDataSourceDetails)).BeginInit();
            this.grpDataSourceDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRunReconciliation
            // 
            this.btnRunReconciliation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance1.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_run_recon;
            this.btnRunReconciliation.Appearance = appearance1;
            this.btnRunReconciliation.ImageSize = new System.Drawing.Size(160, 23);
            this.btnRunReconciliation.Location = new System.Drawing.Point(562, 23);
            this.btnRunReconciliation.Margin = new System.Windows.Forms.Padding(4);
            this.btnRunReconciliation.Name = "btnRunReconciliation";
            this.btnRunReconciliation.ShowFocusRect = false;
            this.btnRunReconciliation.ShowOutline = false;
            this.btnRunReconciliation.Size = new System.Drawing.Size(160, 23);
            this.btnRunReconciliation.TabIndex = 51;
            this.btnRunReconciliation.Text = "Run Reconciliation";
            this.btnRunReconciliation.Click += new System.EventHandler(this.btnRunReconciliation_Click);
            // 
            // cmbDate
            // 
            this.cmbDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbDate.AutoSize = false;
            this.cmbDate.BackColor = System.Drawing.SystemColors.Window;
            this.cmbDate.DateButtons.Add(dateButton1);
            this.cmbDate.Location = new System.Drawing.Point(149, 20);
            this.cmbDate.Name = "cmbDate";
            this.cmbDate.NonAutoSizeHeight = 21;
            this.cmbDate.Size = new System.Drawing.Size(120, 20);
            this.cmbDate.TabIndex = 50;
            this.cmbDate.ValueChanged += new System.EventHandler(this.cmbDate_ValueChanged);
            // 
            // lblDate
            // 
            this.lblDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDate.Location = new System.Drawing.Point(109, 22);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(34, 15);
            this.lblDate.TabIndex = 49;
            this.lblDate.Text = "Date";
            // 
            // ctrlSourceName1
            // 
            this.ctrlSourceName1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ctrlSourceName1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlSourceName1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlSourceName1.Location = new System.Drawing.Point(312, 20);
            this.ctrlSourceName1.Name = "ctrlSourceName1";
            this.ctrlSourceName1.Size = new System.Drawing.Size(243, 26);
            this.ctrlSourceName1.TabIndex = 48;
            this.ctrlSourceName1.SelectionChanged += new System.EventHandler(this.ctrlSourceName1_SelectionChanged);
            // 
            // grdCashRecon
            // 
            this.grdCashRecon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdCashRecon.DisplayLayout.Appearance = appearance2;
            this.grdCashRecon.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdCashRecon.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.grdCashRecon.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdCashRecon.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.grdCashRecon.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdCashRecon.DisplayLayout.GroupByBox.Hidden = true;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdCashRecon.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.grdCashRecon.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCashRecon.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdCashRecon.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdCashRecon.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.grdCashRecon.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdCashRecon.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.grdCashRecon.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdCashRecon.DisplayLayout.Override.CellAppearance = appearance9;
            this.grdCashRecon.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdCashRecon.DisplayLayout.Override.CellPadding = 0;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.BorderColor = System.Drawing.SystemColors.Window;
            this.grdCashRecon.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.TextHAlign = Infragistics.Win.HAlign.Left;
            this.grdCashRecon.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.grdCashRecon.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCashRecon.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.grdCashRecon.DisplayLayout.Override.RowAppearance = appearance12;
            this.grdCashRecon.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdCashRecon.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.grdCashRecon.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCashRecon.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCashRecon.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCashRecon.Location = new System.Drawing.Point(0, 81);
            this.grdCashRecon.Name = "grdCashRecon";
            this.grdCashRecon.Size = new System.Drawing.Size(825, 277);
            this.grdCashRecon.TabIndex = 52;
            this.grdCashRecon.Text = "ultraGrid1";
            this.grdCashRecon.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCashRecon_InitializeLayout);
            this.grdCashRecon.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCashRecon_ClickCellButton);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance14.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_clear;
            this.btnClear.Appearance = appearance14;
            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClear.Location = new System.Drawing.Point(461, 378);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.ShowFocusRect = false;
            this.btnClear.ShowOutline = false;
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 54;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
            this.btnSave.Appearance = appearance15;
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(643, 378);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 53;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance16.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_cancel;
            this.btnCancel.Appearance = appearance16;
            this.btnCancel.ImageSize = new System.Drawing.Size(75, 23);
            this.btnCancel.Location = new System.Drawing.Point(552, 378);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 55;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAcceptSource
            // 
            this.btnAcceptSource.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance17.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_accept_source;
            this.btnAcceptSource.Appearance = appearance17;
            this.btnAcceptSource.ImageSize = new System.Drawing.Size(100, 23);
            this.btnAcceptSource.Location = new System.Drawing.Point(113, 378);
            this.btnAcceptSource.Name = "btnAcceptSource";
            this.btnAcceptSource.ShowFocusRect = false;
            this.btnAcceptSource.ShowOutline = false;
            this.btnAcceptSource.Size = new System.Drawing.Size(100, 23);
            this.btnAcceptSource.TabIndex = 56;
            this.btnAcceptSource.Text = "Accept Source";
            // 
            // btnAcceptApplication
            // 
            this.btnAcceptApplication.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance18.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_accept_application;
            this.btnAcceptApplication.Appearance = appearance18;
            this.btnAcceptApplication.ImageSize = new System.Drawing.Size(100, 23);
            this.btnAcceptApplication.Location = new System.Drawing.Point(229, 378);
            this.btnAcceptApplication.Name = "btnAcceptApplication";
            this.btnAcceptApplication.ShowFocusRect = false;
            this.btnAcceptApplication.ShowOutline = false;
            this.btnAcceptApplication.Size = new System.Drawing.Size(100, 23);
            this.btnAcceptApplication.TabIndex = 57;
            this.btnAcceptApplication.Text = "Accept Application";
            // 
            // btnAcceptManual
            // 
            this.btnAcceptManual.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance19.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_accept_manual;
            this.btnAcceptManual.Appearance = appearance19;
            this.btnAcceptManual.ImageSize = new System.Drawing.Size(100, 23);
            this.btnAcceptManual.Location = new System.Drawing.Point(345, 378);
            this.btnAcceptManual.Name = "btnAcceptManual";
            this.btnAcceptManual.ShowFocusRect = false;
            this.btnAcceptManual.ShowOutline = false;
            this.btnAcceptManual.Size = new System.Drawing.Size(100, 23);
            this.btnAcceptManual.TabIndex = 58;
            this.btnAcceptManual.Text = "Accept Manual";
            // 
            // cmbReconStatus
            // 
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            appearance20.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbReconStatus.DisplayLayout.Appearance = appearance20;
            this.cmbReconStatus.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbReconStatus.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance21.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbReconStatus.DisplayLayout.GroupByBox.Appearance = appearance21;
            appearance22.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbReconStatus.DisplayLayout.GroupByBox.BandLabelAppearance = appearance22;
            this.cmbReconStatus.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance23.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance23.BackColor2 = System.Drawing.SystemColors.Control;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance23.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbReconStatus.DisplayLayout.GroupByBox.PromptAppearance = appearance23;
            this.cmbReconStatus.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbReconStatus.DisplayLayout.MaxRowScrollRegions = 1;
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            appearance24.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbReconStatus.DisplayLayout.Override.ActiveCellAppearance = appearance24;
            appearance25.BackColor = System.Drawing.SystemColors.Highlight;
            appearance25.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbReconStatus.DisplayLayout.Override.ActiveRowAppearance = appearance25;
            this.cmbReconStatus.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbReconStatus.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            this.cmbReconStatus.DisplayLayout.Override.CardAreaAppearance = appearance26;
            appearance27.BorderColor = System.Drawing.Color.Silver;
            appearance27.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbReconStatus.DisplayLayout.Override.CellAppearance = appearance27;
            this.cmbReconStatus.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbReconStatus.DisplayLayout.Override.CellPadding = 0;
            appearance28.BackColor = System.Drawing.SystemColors.Control;
            appearance28.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance28.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbReconStatus.DisplayLayout.Override.GroupByRowAppearance = appearance28;
            appearance29.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbReconStatus.DisplayLayout.Override.HeaderAppearance = appearance29;
            this.cmbReconStatus.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbReconStatus.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance30.BackColor = System.Drawing.SystemColors.Window;
            appearance30.BorderColor = System.Drawing.Color.Silver;
            this.cmbReconStatus.DisplayLayout.Override.RowAppearance = appearance30;
            this.cmbReconStatus.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance31.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbReconStatus.DisplayLayout.Override.TemplateAddRowAppearance = appearance31;
            this.cmbReconStatus.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbReconStatus.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbReconStatus.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbReconStatus.DisplayMember = "";
            this.cmbReconStatus.Location = new System.Drawing.Point(724, 364);
            this.cmbReconStatus.Name = "cmbReconStatus";
            this.cmbReconStatus.Size = new System.Drawing.Size(101, 37);
            this.cmbReconStatus.TabIndex = 59;
            this.cmbReconStatus.Text = "ultraDropDown1";
            this.cmbReconStatus.ValueMember = "";
            this.cmbReconStatus.Visible = false;
            // 
            // grpDataSourceDetails
            // 
            this.grpDataSourceDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDataSourceDetails.Controls.Add(this.cmbDate);
            this.grpDataSourceDetails.Controls.Add(this.ctrlSourceName1);
            this.grpDataSourceDetails.Controls.Add(this.lblDate);
            this.grpDataSourceDetails.Controls.Add(this.btnRunReconciliation);
            this.grpDataSourceDetails.Location = new System.Drawing.Point(0, 12);
            this.grpDataSourceDetails.Name = "grpDataSourceDetails";
            this.grpDataSourceDetails.Size = new System.Drawing.Size(825, 53);
            this.grpDataSourceDetails.SupportThemes = false;
            this.grpDataSourceDetails.TabIndex = 60;
            this.grpDataSourceDetails.Text = "Data Source Details";
            // 
            // CtrlCashRecon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpDataSourceDetails);
            this.Controls.Add(this.cmbReconStatus);
            this.Controls.Add(this.btnAcceptManual);
            this.Controls.Add(this.btnAcceptApplication);
            this.Controls.Add(this.btnAcceptSource);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdCashRecon);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlCashRecon";
            this.Size = new System.Drawing.Size(825, 419);
            ((System.ComponentModel.ISupportInitialize)(this.cmbDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCashRecon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReconStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpDataSourceDetails)).EndInit();
            this.grpDataSourceDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnRunReconciliation;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo cmbDate;
        private Infragistics.Win.Misc.UltraLabel lblDate;
        private CtrlSourceName ctrlSourceName1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCashRecon;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnAcceptSource;
        private Infragistics.Win.Misc.UltraButton btnAcceptApplication;
        private Infragistics.Win.Misc.UltraButton btnAcceptManual;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbReconStatus;
        private Infragistics.Win.Misc.UltraGroupBox grpDataSourceDetails;
    }
}
