namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlRunTradeRecon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlRunTradeRecon));
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.grdReconSummary = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lblAction = new Infragistics.Win.Misc.UltraLabel();
            this.btnRunRecon = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.cmbReconStatus = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ctrlSourceName1 = new Nirvana.Admin.PositionManagement.Controls.CtrlSourceName();
            this.grpDataSourceDetails = new Infragistics.Win.Misc.UltraGroupBox();
            this.cmbReconDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblReconSummaryRecord = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grdReconSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReconStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpDataSourceDetails)).BeginInit();
            this.grpDataSourceDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReconDate)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnCancel.Appearance = appearance1;
            this.btnCancel.ImageSize = new System.Drawing.Size(75, 23);
            this.btnCancel.Location = new System.Drawing.Point(387, 416);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grdReconSummary
            // 
            this.grdReconSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdReconSummary.DisplayLayout.Appearance = appearance2;
            this.grdReconSummary.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdReconSummary.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.grdReconSummary.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdReconSummary.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.grdReconSummary.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdReconSummary.DisplayLayout.GroupByBox.Hidden = true;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdReconSummary.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.grdReconSummary.DisplayLayout.MaxColScrollRegions = 1;
            this.grdReconSummary.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdReconSummary.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdReconSummary.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.grdReconSummary.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdReconSummary.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.grdReconSummary.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdReconSummary.DisplayLayout.Override.CellAppearance = appearance9;
            this.grdReconSummary.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdReconSummary.DisplayLayout.Override.CellPadding = 0;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.BorderColor = System.Drawing.SystemColors.Window;
            this.grdReconSummary.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.TextHAlign = Infragistics.Win.HAlign.Left;
            this.grdReconSummary.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.grdReconSummary.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdReconSummary.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.grdReconSummary.DisplayLayout.Override.RowAppearance = appearance12;
            this.grdReconSummary.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdReconSummary.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.grdReconSummary.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdReconSummary.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdReconSummary.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdReconSummary.Location = new System.Drawing.Point(8, 113);
            this.grdReconSummary.Name = "grdReconSummary";
            this.grdReconSummary.Size = new System.Drawing.Size(706, 281);
            this.grdReconSummary.TabIndex = 6;
            this.grdReconSummary.Text = "ultraGrid1";
            this.grdReconSummary.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdReconSummary_InitializeLayout);
            // 
            // lblAction
            // 
            this.lblAction.Location = new System.Drawing.Point(415, 26);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(47, 17);
            this.lblAction.TabIndex = 7;
            this.lblAction.Text = "Action";
            // 
            // btnRunRecon
            // 
            appearance14.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_run_recon;
            this.btnRunRecon.Appearance = appearance14;
            this.btnRunRecon.ImageSize = new System.Drawing.Size(145, 23);
            this.btnRunRecon.Location = new System.Drawing.Point(468, 24);
            this.btnRunRecon.Name = "btnRunRecon";
            this.btnRunRecon.ShowFocusRect = false;
            this.btnRunRecon.ShowOutline = false;
            this.btnRunRecon.Size = new System.Drawing.Size(145, 23);
            this.btnRunRecon.TabIndex = 9;
            this.btnRunRecon.Text = "Run Re-conciliation";
            // 
            // btnView
            // 
            this.btnView.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance15.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_view;
            this.btnView.Appearance = appearance15;
            this.btnView.ImageSize = new System.Drawing.Size(75, 23);
            this.btnView.Location = new System.Drawing.Point(260, 416);
            this.btnView.Name = "btnView";
            this.btnView.ShowFocusRect = false;
            this.btnView.ShowOutline = false;
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 9;
            this.btnView.Text = "View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // cmbReconStatus
            // 
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            appearance16.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbReconStatus.DisplayLayout.Appearance = appearance16;
            this.cmbReconStatus.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbReconStatus.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance17.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance17.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbReconStatus.DisplayLayout.GroupByBox.Appearance = appearance17;
            appearance18.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbReconStatus.DisplayLayout.GroupByBox.BandLabelAppearance = appearance18;
            this.cmbReconStatus.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance19.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance19.BackColor2 = System.Drawing.SystemColors.Control;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance19.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbReconStatus.DisplayLayout.GroupByBox.PromptAppearance = appearance19;
            this.cmbReconStatus.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbReconStatus.DisplayLayout.MaxRowScrollRegions = 1;
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            appearance20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbReconStatus.DisplayLayout.Override.ActiveCellAppearance = appearance20;
            appearance21.BackColor = System.Drawing.SystemColors.Highlight;
            appearance21.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbReconStatus.DisplayLayout.Override.ActiveRowAppearance = appearance21;
            this.cmbReconStatus.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbReconStatus.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance22.BackColor = System.Drawing.SystemColors.Window;
            this.cmbReconStatus.DisplayLayout.Override.CardAreaAppearance = appearance22;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            appearance23.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbReconStatus.DisplayLayout.Override.CellAppearance = appearance23;
            this.cmbReconStatus.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbReconStatus.DisplayLayout.Override.CellPadding = 0;
            appearance24.BackColor = System.Drawing.SystemColors.Control;
            appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance24.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance24.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbReconStatus.DisplayLayout.Override.GroupByRowAppearance = appearance24;
            appearance25.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbReconStatus.DisplayLayout.Override.HeaderAppearance = appearance25;
            this.cmbReconStatus.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbReconStatus.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            appearance26.BorderColor = System.Drawing.Color.Silver;
            this.cmbReconStatus.DisplayLayout.Override.RowAppearance = appearance26;
            this.cmbReconStatus.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance27.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbReconStatus.DisplayLayout.Override.TemplateAddRowAppearance = appearance27;
            this.cmbReconStatus.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbReconStatus.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbReconStatus.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbReconStatus.DisplayMember = "";
            this.cmbReconStatus.Location = new System.Drawing.Point(13, 400);
            this.cmbReconStatus.Name = "cmbReconStatus";
            this.cmbReconStatus.Size = new System.Drawing.Size(104, 34);
            this.cmbReconStatus.TabIndex = 13;
            this.cmbReconStatus.ValueMember = "";
            this.cmbReconStatus.Visible = false;
            // 
            // ctrlSourceName1
            // 
            this.ctrlSourceName1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlSourceName1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlSourceName1.Location = new System.Drawing.Point(23, 20);
            this.ctrlSourceName1.Name = "ctrlSourceName1";
            this.ctrlSourceName1.Size = new System.Drawing.Size(257, 27);
            this.ctrlSourceName1.TabIndex = 11;
            this.ctrlSourceName1.SelectionChanged += new System.EventHandler(this.ctrlSourceName1_SelectionChanged);
            // 
            // grpDataSourceDetails
            // 
            this.grpDataSourceDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDataSourceDetails.Controls.Add(this.cmbReconDate);
            this.grpDataSourceDetails.Controls.Add(this.lblReconSummaryRecord);
            this.grpDataSourceDetails.Controls.Add(this.ctrlSourceName1);
            this.grpDataSourceDetails.Controls.Add(this.btnRunRecon);
            this.grpDataSourceDetails.Controls.Add(this.lblAction);
            this.grpDataSourceDetails.Location = new System.Drawing.Point(8, 3);
            this.grpDataSourceDetails.Name = "grpDataSourceDetails";
            this.grpDataSourceDetails.Size = new System.Drawing.Size(706, 104);
            this.grpDataSourceDetails.SupportThemes = false;
            this.grpDataSourceDetails.TabIndex = 14;
            this.grpDataSourceDetails.Text = "Data Source Details";
            // 
            // cmbReconDate
            // 
            this.cmbReconDate.BackColor = System.Drawing.SystemColors.Window;
            this.cmbReconDate.DateButtons.Add(dateButton1);
            this.cmbReconDate.FlatMode = true;
            this.cmbReconDate.Format = "m";
            this.cmbReconDate.Location = new System.Drawing.Point(346, 61);
            this.cmbReconDate.Name = "cmbReconDate";
            this.cmbReconDate.NonAutoSizeHeight = 21;
            this.cmbReconDate.Size = new System.Drawing.Size(121, 21);
            this.cmbReconDate.TabIndex = 14;
            // 
            // lblReconSummaryRecord
            // 
            this.lblReconSummaryRecord.Location = new System.Drawing.Point(178, 63);
            this.lblReconSummaryRecord.Name = "lblReconSummaryRecord";
            this.lblReconSummaryRecord.Size = new System.Drawing.Size(162, 20);
            this.lblReconSummaryRecord.TabIndex = 13;
            this.lblReconSummaryRecord.Text = "Re-con Summary Record as of";
            // 
            // CtrlRunTradeRecon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpDataSourceDetails);
            this.Controls.Add(this.cmbReconStatus);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grdReconSummary);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlRunTradeRecon";
            this.Size = new System.Drawing.Size(722, 462);
            ((System.ComponentModel.ISupportInitialize)(this.grdReconSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReconStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpDataSourceDetails)).EndInit();
            this.grpDataSourceDetails.ResumeLayout(false);
            this.grpDataSourceDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReconDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdReconSummary;
        private Infragistics.Win.Misc.UltraLabel lblAction;
        private Infragistics.Win.Misc.UltraButton btnRunRecon;
        private Infragistics.Win.Misc.UltraButton btnView;
        private CtrlSourceName ctrlSourceName1;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbReconStatus;
        private Infragistics.Win.Misc.UltraGroupBox grpDataSourceDetails;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo cmbReconDate;
        private Infragistics.Win.Misc.UltraLabel lblReconSummaryRecord;
    }
}
