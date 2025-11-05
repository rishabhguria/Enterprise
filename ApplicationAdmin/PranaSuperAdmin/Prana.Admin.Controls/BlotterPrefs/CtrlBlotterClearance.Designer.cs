namespace Prana.Admin.Controls
{
    partial class CtrlBlotterClearance
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
                if (clearanceDataSource != null)
                {
                    clearanceDataSource.Dispose();
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            this.cmbTimeZone = new Infragistics.Win.UltraWinEditors.UltraTimeZoneEditor();
            this.gridClearanceTable = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.clearanceGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.timeToNotify = new System.Windows.Forms.DateTimePicker();
            this.checkBoxNotifyGTCGTDOrders = new System.Windows.Forms.CheckBox();
            this.ultraLabelRolloverPermittedUser = new Infragistics.Win.Misc.UltraLabel();
            this.ultraComboRolloverPermittedUser = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.uoAutoClearing = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.checkBoxLiveManualOrderSend = new System.Windows.Forms.CheckBox();
            this.lblTimeZone = new Infragistics.Win.Misc.UltraLabel();
            this.dtBaseTime = new System.Windows.Forms.DateTimePicker();
            this.lblAutoClearing = new Infragistics.Win.Misc.UltraLabel();
            this.lblBaseTime = new Infragistics.Win.Misc.UltraLabel();
            this.activeGtcGtdGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTimeZone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridClearanceTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearanceGroupBox)).BeginInit();
            this.clearanceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboRolloverPermittedUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoAutoClearing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeGtcGtdGroupBox)).BeginInit();
            this.activeGtcGtdGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbTimeZone
            // 
            this.cmbTimeZone.Location = new System.Drawing.Point(19, 51);
            this.cmbTimeZone.Name = "cmbTimeZone";
            this.cmbTimeZone.Size = new System.Drawing.Size(308, 22);
            this.cmbTimeZone.TabIndex = 33;
            this.cmbTimeZone.Text = "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi";
            this.cmbTimeZone.ValueChanged += new System.EventHandler(this.cmbTimeZone_ValueChanged);
            // 
            // clearanceTable
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            appearance10.TextHAlignAsString = "Left";
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;

            this.gridClearanceTable.DisplayLayout.Appearance = appearance1;
            this.gridClearanceTable.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.gridClearanceTable.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridClearanceTable.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.gridClearanceTable.DisplayLayout.GroupByBox.Appearance = appearance2;
            this.gridClearanceTable.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.gridClearanceTable.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridClearanceTable.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.gridClearanceTable.DisplayLayout.MaxColScrollRegions = 1;
            this.gridClearanceTable.DisplayLayout.MaxRowScrollRegions = 1;
            this.gridClearanceTable.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            this.gridClearanceTable.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.gridClearanceTable.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.gridClearanceTable.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridClearanceTable.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridClearanceTable.DisplayLayout.Override.CardAreaAppearance = appearance7;
            this.gridClearanceTable.DisplayLayout.Override.CellAppearance = appearance8;
            this.gridClearanceTable.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridClearanceTable.DisplayLayout.Override.CellPadding = 0;
            this.gridClearanceTable.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.HeadersOnly;
            this.gridClearanceTable.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            this.gridClearanceTable.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.gridClearanceTable.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.gridClearanceTable.DisplayLayout.Override.RowAppearance = appearance11;
            this.gridClearanceTable.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.gridClearanceTable.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.gridClearanceTable.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.gridClearanceTable.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.gridClearanceTable.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.gridClearanceTable.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.gridClearanceTable.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.gridClearanceTable.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridClearanceTable.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridClearanceTable.Location = new System.Drawing.Point(17, 128);
            this.gridClearanceTable.Name = "clearanceTable";
            this.gridClearanceTable.Size = new System.Drawing.Size(659, 222);
            this.gridClearanceTable.TabIndex = 0;
            this.gridClearanceTable.Text = "Blotter Clearance";
            this.gridClearanceTable.CellChange += this.clearanceTable_CellChange;
            // 
            // clearanceGroupBox
            // 
            this.clearanceGroupBox.Controls.Add(this.ultraLabelRolloverPermittedUser);
            this.clearanceGroupBox.Controls.Add(this.ultraComboRolloverPermittedUser);
            this.clearanceGroupBox.Controls.Add(this.uoAutoClearing);
            this.clearanceGroupBox.Controls.Add(this.lblMessage);
            this.clearanceGroupBox.Controls.Add(this.cmbTimeZone);
            this.clearanceGroupBox.Controls.Add(this.gridClearanceTable);
            this.clearanceGroupBox.Controls.Add(this.lblTimeZone);
            this.clearanceGroupBox.Controls.Add(this.dtBaseTime);
            this.clearanceGroupBox.Controls.Add(this.lblAutoClearing);
            this.clearanceGroupBox.Controls.Add(this.lblBaseTime);
            this.clearanceGroupBox.Controls.Add(this.checkBoxLiveManualOrderSend);
            this.clearanceGroupBox.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.clearanceGroupBox.Location = new System.Drawing.Point(0, 0);
            this.clearanceGroupBox.Name = "clearanceGroupBox";
            this.clearanceGroupBox.Size = new System.Drawing.Size(694, 379);
            this.clearanceGroupBox.TabIndex = 4;
            this.clearanceGroupBox.Text = "Blotter Clearance";
            // 
            // timeToNotify
            // 
            this.timeToNotify.Enabled = false;
            this.timeToNotify.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timeToNotify.Location = new System.Drawing.Point(196, 15);
            this.timeToNotify.Name = "timeToNotify";
            this.timeToNotify.ShowUpDown = true;
            this.timeToNotify.Size = new System.Drawing.Size(102, 20);
            this.timeToNotify.TabIndex = 42;
            this.timeToNotify.Value = new System.DateTime(2023, 12, 1, 9, 0, 0, 0);
            // 
            // checkBoxNotifyGTCGTDOrders
            // 
            this.checkBoxNotifyGTCGTDOrders.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.checkBoxNotifyGTCGTDOrders.Location = new System.Drawing.Point(19, 15);
            this.checkBoxNotifyGTCGTDOrders.Name = "checkBoxNotifyGTCGTDOrders";
            this.checkBoxNotifyGTCGTDOrders.Size = new System.Drawing.Size(180, 26);
            this.checkBoxNotifyGTCGTDOrders.TabIndex = 41;
            this.checkBoxNotifyGTCGTDOrders.Text = "Notify Active GTC/GTD orders";
            this.checkBoxNotifyGTCGTDOrders.CheckedChanged += new System.EventHandler(this.CheckBoxNotifyGTCGTDOrders_CheckedChanged);
            // 
            // ultraLabelRolloverPermittedUser
            // 
            this.ultraLabelRolloverPermittedUser.Location = new System.Drawing.Point(542, 32);
            this.ultraLabelRolloverPermittedUser.Name = "ultraLabelRolloverPermittedUser";
            this.ultraLabelRolloverPermittedUser.Size = new System.Drawing.Size(134, 13);
            this.ultraLabelRolloverPermittedUser.TabIndex = 39;
            this.ultraLabelRolloverPermittedUser.Text = "Rollover Permitted User";
            // 
            // ultraComboRolloverPermittedUser
            // 
            this.ultraComboRolloverPermittedUser.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraComboRolloverPermittedUser.Location = new System.Drawing.Point(446, 51);
            this.ultraComboRolloverPermittedUser.Name = "ultraComboRolloverPermittedUser";
            this.ultraComboRolloverPermittedUser.Size = new System.Drawing.Size(230, 22);
            this.ultraComboRolloverPermittedUser.TabIndex = 38;
            // 
            // uoAutoClearing
            // 
            this.uoAutoClearing.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.uoAutoClearing.CheckedIndex = 0;
            valueListItem1.DataValue = "Yes";
            valueListItem1.DisplayText = "Yes";
            valueListItem2.DataValue = "No";
            valueListItem2.DisplayText = "No";
            this.uoAutoClearing.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.uoAutoClearing.Location = new System.Drawing.Point(252, 101);
            this.uoAutoClearing.Name = "uoAutoClearing";
            this.uoAutoClearing.Size = new System.Drawing.Size(75, 14);
            this.uoAutoClearing.TabIndex = 36;
            this.uoAutoClearing.Text = "Yes";
            // 
            // lblMessage
            // 
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(19, 356);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(264, 15);
            this.lblMessage.TabIndex = 35;
            this.lblMessage.Text = "* AUEC times are their respective Exchange Times";
            // 
            // checkBoxLiveManualOrderSend
            //
            this.checkBoxLiveManualOrderSend.Location = new System.Drawing.Point(446, 101);
            this.checkBoxLiveManualOrderSend.Name = "checkBoxLiveManualOrderSend";
            this.checkBoxLiveManualOrderSend.Size = new System.Drawing.Size(230, 15);
            this.checkBoxLiveManualOrderSend.TabIndex = 40;
            this.checkBoxLiveManualOrderSend.Text = "Send Manual Orders Via FIX RealTime";



            // 
            // lblTimeZone
            // 
            this.lblTimeZone.Location = new System.Drawing.Point(19, 32);
            this.lblTimeZone.Name = "lblTimeZone";
            this.lblTimeZone.Size = new System.Drawing.Size(58, 13);
            this.lblTimeZone.TabIndex = 34;
            this.lblTimeZone.Text = "Time Zone";
            // 
            // dtBaseTime
            // 
            this.dtBaseTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtBaseTime.Location = new System.Drawing.Point(19, 101);
            this.dtBaseTime.Name = "dtBaseTime";
            this.dtBaseTime.ShowUpDown = true;
            this.dtBaseTime.Size = new System.Drawing.Size(102, 21);
            this.dtBaseTime.TabIndex = 6;
            this.dtBaseTime.ValueChanged += new System.EventHandler(this.dtBaseTime_ValueChanged);
            // 
            // lblAutoClearing
            // 
            this.lblAutoClearing.Location = new System.Drawing.Point(186, 101);
            this.lblAutoClearing.Name = "lblAutoClearing";
            this.lblAutoClearing.Size = new System.Drawing.Size(56, 13);
            this.lblAutoClearing.TabIndex = 29;
            this.lblAutoClearing.Text = "Auto Clear";
            // 
            // lblBaseTime
            // 
            this.lblBaseTime.Location = new System.Drawing.Point(19, 82);
            this.lblBaseTime.Name = "lblBaseTime";
            this.lblBaseTime.Size = new System.Drawing.Size(58, 13);
            this.lblBaseTime.TabIndex = 5;
            this.lblBaseTime.Text = "Base Time";
            //
            // activeGtcGtdGroupBox
            // 
            this.activeGtcGtdGroupBox.Controls.Add(this.timeToNotify);
            this.activeGtcGtdGroupBox.Controls.Add(this.checkBoxNotifyGTCGTDOrders);
            this.activeGtcGtdGroupBox.Location = new System.Drawing.Point(0, 388);
            this.activeGtcGtdGroupBox.Name = "activeGtcGtdGroupBox";
            this.activeGtcGtdGroupBox.Size = new System.Drawing.Size(694, 48);
            this.activeGtcGtdGroupBox.TabIndex = 5;
            // 
            // CtrlBlotterClearance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.activeGtcGtdGroupBox);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.clearanceGroupBox);
            this.Name = "CtrlBlotterClearance";
            this.Size = new System.Drawing.Size(694, 461);
            ((System.ComponentModel.ISupportInitialize)(this.cmbTimeZone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridClearanceTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearanceGroupBox)).EndInit();
            this.clearanceGroupBox.ResumeLayout(false);
            this.clearanceGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboRolloverPermittedUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoAutoClearing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeGtcGtdGroupBox)).EndInit();
            this.activeGtcGtdGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

   

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraTimeZoneEditor cmbTimeZone;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridClearanceTable;
        private Infragistics.Win.Misc.UltraGroupBox clearanceGroupBox;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet uoAutoClearing;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private Infragistics.Win.Misc.UltraLabel lblTimeZone;
        private System.Windows.Forms.DateTimePicker dtBaseTime;
        private Infragistics.Win.Misc.UltraLabel lblAutoClearing;
        private Infragistics.Win.Misc.UltraLabel lblBaseTime;
        private Infragistics.Win.Misc.UltraLabel ultraLabelRolloverPermittedUser;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraComboRolloverPermittedUser;
        private System.Windows.Forms.CheckBox checkBoxLiveManualOrderSend;
        private System.Windows.Forms.CheckBox checkBoxNotifyGTCGTDOrders;
        private System.Windows.Forms.DateTimePicker timeToNotify;
        private Infragistics.Win.Misc.UltraGroupBox activeGtcGtdGroupBox;
    }
}
