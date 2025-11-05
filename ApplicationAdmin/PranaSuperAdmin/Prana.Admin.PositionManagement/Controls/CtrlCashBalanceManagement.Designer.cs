namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlCashBalanceManagement
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
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
            this.cmbDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblDate = new Infragistics.Win.Misc.UltraLabel();
            this.grdTrades = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnRefresh = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.grpDataSourceDetails = new Infragistics.Win.Misc.UltraGroupBox();
            this.ctrlSourceName1 = new Nirvana.Admin.PositionManagement.Controls.CtrlSourceName();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTrades)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpDataSourceDetails)).BeginInit();
            this.grpDataSourceDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbDate
            // 
            this.cmbDate.AutoSize = false;
            this.cmbDate.BackColor = System.Drawing.SystemColors.Window;
            this.cmbDate.DateButtons.Add(dateButton1);
            this.cmbDate.Location = new System.Drawing.Point(46, 27);
            this.cmbDate.Name = "cmbDate";
            this.cmbDate.NonAutoSizeHeight = 21;
            this.cmbDate.Size = new System.Drawing.Size(120, 20);
            this.cmbDate.TabIndex = 44;
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(6, 30);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(34, 15);
            this.lblDate.TabIndex = 43;
            this.lblDate.Text = "Date";
            // 
            // grdTrades
            // 
            this.grdTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdTrades.DisplayLayout.Appearance = appearance1;
            this.grdTrades.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdTrades.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdTrades.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdTrades.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdTrades.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdTrades.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdTrades.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdTrades.DisplayLayout.MaxColScrollRegions = 1;
            this.grdTrades.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdTrades.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdTrades.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdTrades.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdTrades.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdTrades.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdTrades.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdTrades.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdTrades.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdTrades.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.grdTrades.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdTrades.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdTrades.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdTrades.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdTrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdTrades.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdTrades.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdTrades.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdTrades.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdTrades.Location = new System.Drawing.Point(3, 73);
            this.grdTrades.Name = "grdTrades";
            this.grdTrades.Size = new System.Drawing.Size(628, 312);
            this.grdTrades.TabIndex = 46;
            this.grdTrades.Text = "ultraGrid1";
            this.grdTrades.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdTrades_InitializeLayout);
            this.grdTrades.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(grdTrades_ClickCellButton);
            // 
            // btnRefresh
            // 
            appearance13.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.refresh;
            this.btnRefresh.Appearance = appearance13;
            this.btnRefresh.ImageSize = new System.Drawing.Size(75, 23);
            this.btnRefresh.Location = new System.Drawing.Point(546, 30);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.ShowFocusRect = false;
            this.btnRefresh.ShowOutline = false;
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 47;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance14.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_view;
            this.btnView.Appearance = appearance14;
            this.btnView.ImageSize = new System.Drawing.Size(75, 23);
            this.btnView.Location = new System.Drawing.Point(274, 401);
            this.btnView.Margin = new System.Windows.Forms.Padding(4);
            this.btnView.Name = "btnView";
            this.btnView.ShowFocusRect = false;
            this.btnView.ShowOutline = false;
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 48;
            this.btnView.Text = "View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // grpDataSourceDetails
            // 
            this.grpDataSourceDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDataSourceDetails.Controls.Add(this.lblDate);
            this.grpDataSourceDetails.Controls.Add(this.cmbDate);
            this.grpDataSourceDetails.Controls.Add(this.btnRefresh);
            this.grpDataSourceDetails.Controls.Add(this.ctrlSourceName1);
            this.grpDataSourceDetails.Location = new System.Drawing.Point(3, 3);
            this.grpDataSourceDetails.Name = "grpDataSourceDetails";
            this.grpDataSourceDetails.Size = new System.Drawing.Size(628, 64);
            this.grpDataSourceDetails.SupportThemes = false;
            this.grpDataSourceDetails.TabIndex = 49;
            this.grpDataSourceDetails.Text = "Data Source Details";
            // 
            // ctrlSourceName1
            // 
            this.ctrlSourceName1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlSourceName1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlSourceName1.Location = new System.Drawing.Point(297, 27);
            this.ctrlSourceName1.Name = "ctrlSourceName1";
            this.ctrlSourceName1.Size = new System.Drawing.Size(243, 26);
            this.ctrlSourceName1.TabIndex = 42;
            this.ctrlSourceName1.SelectionChanged += new System.EventHandler(this.ctrlSourceName1_SelectionChanged);
            // 
            // CtrlCashBalanceManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpDataSourceDetails);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.grdTrades);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlCashBalanceManagement";
            this.Size = new System.Drawing.Size(634, 440);
            ((System.ComponentModel.ISupportInitialize)(this.cmbDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTrades)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpDataSourceDetails)).EndInit();
            this.grpDataSourceDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        

        #endregion

        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo cmbDate;
        private Infragistics.Win.Misc.UltraLabel lblDate;
        private CtrlSourceName ctrlSourceName1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdTrades;
        private Infragistics.Win.Misc.UltraButton btnRefresh;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraGroupBox grpDataSourceDetails;
    }
}
