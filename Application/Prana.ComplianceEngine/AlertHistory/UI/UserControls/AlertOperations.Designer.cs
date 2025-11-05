namespace Prana.ComplianceEngine.AlertHistory.UI.UserControls
{
    partial class AlertOperations
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.alertHistoryPaging1 = new Prana.ComplianceEngine.AlertHistory.UI.UserControls.AlertHistoryPaging();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.ultraBtnArchive = new Infragistics.Win.Misc.UltraButton();
            this.ultraBtnGetData = new Infragistics.Win.Misc.UltraButton();
            this.ultraBtnExport = new Infragistics.Win.Misc.UltraButton();
            this.ultraLblTo = new Infragistics.Win.Misc.UltraLabel();
            this.ultraClndrTo = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLblFrom = new Infragistics.Win.Misc.UltraLabel();
            this.ultraClndrFrom = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraOptHistory = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraClndrTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraClndrFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraOptHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.alertHistoryPaging1);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraButton1);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraBtnArchive);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraBtnGetData);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraBtnExport);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLblTo);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraClndrTo);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLblFrom);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraClndrFrom);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraOptHistory);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1131, 40);
            this.ultraPanel1.TabIndex = 0;
            // 
            // alertHistoryPaging1
            // 
            this.alertHistoryPaging1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.alertHistoryPaging1.Location = new System.Drawing.Point(822, 12);
            this.alertHistoryPaging1.Name = "alertHistoryPaging1";
            this.alertHistoryPaging1.Size = new System.Drawing.Size(306, 28);
            this.alertHistoryPaging1.TabIndex = 17;
            // 
            // ultraButton1
            // 
            this.ultraButton1.Location = new System.Drawing.Point(725, 13);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(75, 21);
            this.ultraButton1.TabIndex = 16;
            this.ultraButton1.Text = "Delete";
            this.ultraButton1.Click += new System.EventHandler(this.UltraButtonDelete_Click);
            // 
            // ultraBtnArchive
            // 
            this.ultraBtnArchive.Location = new System.Drawing.Point(657, 12);
            this.ultraBtnArchive.Name = "ultraBtnArchive";
            this.ultraBtnArchive.Size = new System.Drawing.Size(60, 23);
            this.ultraBtnArchive.TabIndex = 15;
            this.ultraBtnArchive.Text = "Archive";
            this.ultraBtnArchive.Click += new System.EventHandler(this.ultraBtnArchive_Click);
            // 
            // ultraBtnGetData
            // 
            this.ultraBtnGetData.Location = new System.Drawing.Point(518, 12);
            this.ultraBtnGetData.Name = "ultraBtnGetData";
            this.ultraBtnGetData.Size = new System.Drawing.Size(64, 23);
            this.ultraBtnGetData.TabIndex = 14;
            this.ultraBtnGetData.Text = "Get Data";
            this.ultraBtnGetData.Click += new System.EventHandler(this.ultraBtnGetData_Click);
            // 
            // ultraBtnExport
            // 
            this.ultraBtnExport.Location = new System.Drawing.Point(590, 12);
            this.ultraBtnExport.Name = "ultraBtnExport";
            this.ultraBtnExport.Size = new System.Drawing.Size(59, 23);
            this.ultraBtnExport.TabIndex = 13;
            this.ultraBtnExport.Text = "Export";
            this.ultraBtnExport.Click += new System.EventHandler(this.ultraBtnExport_Click);
            // 
            // ultraLblTo
            // 
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLblTo.Appearance = appearance1;
            this.ultraLblTo.Location = new System.Drawing.Point(345, 14);
            this.ultraLblTo.Name = "ultraLblTo";
            this.ultraLblTo.Size = new System.Drawing.Size(24, 18);
            this.ultraLblTo.TabIndex = 12;
            this.ultraLblTo.Text = "To: ";
            // 
            // ultraClndrTo
            // 
            this.ultraClndrTo.DateButtons.Add(dateButton1);
            this.ultraClndrTo.Enabled = false;
            this.ultraClndrTo.Format = "MM/dd/yyyy";
            this.ultraClndrTo.Location = new System.Drawing.Point(377, 13);
            this.ultraClndrTo.Name = "ultraClndrTo";
            this.ultraClndrTo.NonAutoSizeHeight = 21;
            this.ultraClndrTo.Size = new System.Drawing.Size(133, 21);
            this.ultraClndrTo.TabIndex = 11;
            // 
            // ultraLblFrom
            // 
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLblFrom.Appearance = appearance2;
            this.ultraLblFrom.Location = new System.Drawing.Point(158, 14);
            this.ultraLblFrom.Name = "ultraLblFrom";
            this.ultraLblFrom.Size = new System.Drawing.Size(39, 18);
            this.ultraLblFrom.TabIndex = 10;
            this.ultraLblFrom.Text = "From: ";
            // 
            // ultraClndrFrom
            // 
            this.ultraClndrFrom.AutoSelectionUpdate = true;
            this.ultraClndrFrom.DateButtons.Add(dateButton2);
            this.ultraClndrFrom.Enabled = false;
            this.ultraClndrFrom.Format = "MM/dd/yyyy";
            this.ultraClndrFrom.Location = new System.Drawing.Point(205, 13);
            this.ultraClndrFrom.Name = "ultraClndrFrom";
            this.ultraClndrFrom.NonAutoSizeHeight = 21;
            this.ultraClndrFrom.Size = new System.Drawing.Size(132, 21);
            this.ultraClndrFrom.TabIndex = 9;
            this.ultraClndrFrom.YearScrollButtonsVisible = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraOptHistory
            // 
            this.ultraOptHistory.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraOptHistory.CheckedIndex = 0;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = "Current";
            valueListItem1.DisplayText = "Current";
            valueListItem1.Tag = "Current";
            valueListItem2.DataValue = "Historical";
            valueListItem2.DisplayText = "Historical";
            valueListItem2.Tag = "Historical";
            this.ultraOptHistory.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.ultraOptHistory.ItemSpacingHorizontal = 5;
            this.ultraOptHistory.Location = new System.Drawing.Point(12, 15);
            this.ultraOptHistory.Name = "ultraOptHistory";
            this.ultraOptHistory.Size = new System.Drawing.Size(140, 16);
            this.ultraOptHistory.TabIndex = 8;
            this.ultraOptHistory.Text = "Current";
            this.ultraOptHistory.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraOptHistory.ValueChanged += new System.EventHandler(this.ultraOptHistory_ValueChanged);
            // 
            // AlertOperations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "AlertOperations";
            this.Size = new System.Drawing.Size(1131, 40);
            this.Load += new System.EventHandler(this.AlertOperations_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraClndrTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraClndrFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraOptHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton ultraBtnArchive;
        private Infragistics.Win.Misc.UltraButton ultraBtnGetData;
        private Infragistics.Win.Misc.UltraButton ultraBtnExport;
        private Infragistics.Win.Misc.UltraLabel ultraLblTo;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo ultraClndrTo;
        private Infragistics.Win.Misc.UltraLabel ultraLblFrom;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo ultraClndrFrom;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet ultraOptHistory;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private AlertHistoryPaging alertHistoryPaging1;

    }
}
