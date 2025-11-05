namespace Prana.PM.Client.UI.Controls
{
    partial class CtrlCloseTradeMatchedTradeReport
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
                if (_formBindingSource != null)
                {
                    _formBindingSource.Dispose();
                }
                _isInitialized = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlCloseTradeMatchedTradeReport));
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
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
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblMatchedTradeDate = new Infragistics.Win.Misc.UltraLabel();
            this.cmbDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.grdMatchedTradeReport = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMatchedTradeReport)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance1.Image = global::Prana.PM.Client.UI.Properties.Resources.btn_cancel;
            this.btnCancel.Appearance = appearance1;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.ImageSize = new System.Drawing.Size(75, 23);
            this.btnCancel.Location = new System.Drawing.Point(308, 380);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 59;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnSave.Appearance = appearance2;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(164, 380);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 58;
            // 
            // lblMatchedTradeDate
            // 
            this.lblMatchedTradeDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMatchedTradeDate.Location = new System.Drawing.Point(132, 21);
            this.lblMatchedTradeDate.Name = "lblMatchedTradeDate";
            this.lblMatchedTradeDate.Size = new System.Drawing.Size(143, 15);
            this.lblMatchedTradeDate.TabIndex = 60;
            this.lblMatchedTradeDate.Text = "Matched Trade Report as of";
            // 
            // cmbDate
            // 
            this.cmbDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbDate.BackColor = System.Drawing.SystemColors.Window;
            this.cmbDate.DateButtons.Add(dateButton1);
            this.cmbDate.Location = new System.Drawing.Point(281, 18);
            this.cmbDate.Name = "cmbDate";
            this.cmbDate.NonAutoSizeHeight = 21;
            this.cmbDate.Size = new System.Drawing.Size(121, 21);
            this.cmbDate.TabIndex = 61;
            // 
            // grdMatchedTradeReport
            // 
            this.grdMatchedTradeReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.SystemColors.Window;
            appearance3.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdMatchedTradeReport.DisplayLayout.Appearance = appearance3;
            this.grdMatchedTradeReport.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMatchedTradeReport.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance4.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance4.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.BorderColor = System.Drawing.SystemColors.Window;
            this.grdMatchedTradeReport.DisplayLayout.GroupByBox.Appearance = appearance4;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdMatchedTradeReport.DisplayLayout.GroupByBox.BandLabelAppearance = appearance5;
            this.grdMatchedTradeReport.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMatchedTradeReport.DisplayLayout.GroupByBox.Hidden = true;
            appearance6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance6.BackColor2 = System.Drawing.SystemColors.Control;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdMatchedTradeReport.DisplayLayout.GroupByBox.PromptAppearance = appearance6;
            this.grdMatchedTradeReport.DisplayLayout.MaxColScrollRegions = 1;
            this.grdMatchedTradeReport.DisplayLayout.MaxRowScrollRegions = 1;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            appearance7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdMatchedTradeReport.DisplayLayout.Override.ActiveCellAppearance = appearance7;
            appearance8.BackColor = System.Drawing.SystemColors.Highlight;
            appearance8.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdMatchedTradeReport.DisplayLayout.Override.ActiveRowAppearance = appearance8;
            this.grdMatchedTradeReport.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdMatchedTradeReport.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance9.BackColor = System.Drawing.SystemColors.Window;
            this.grdMatchedTradeReport.DisplayLayout.Override.CardAreaAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.Silver;
            appearance10.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdMatchedTradeReport.DisplayLayout.Override.CellAppearance = appearance10;
            this.grdMatchedTradeReport.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdMatchedTradeReport.DisplayLayout.Override.CellPadding = 0;
            appearance11.BackColor = System.Drawing.SystemColors.Control;
            appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance11.BorderColor = System.Drawing.SystemColors.Window;
            this.grdMatchedTradeReport.DisplayLayout.Override.GroupByRowAppearance = appearance11;
            appearance12.TextHAlign = Infragistics.Win.HAlign.Left;
            this.grdMatchedTradeReport.DisplayLayout.Override.HeaderAppearance = appearance12;
            this.grdMatchedTradeReport.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdMatchedTradeReport.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.Color.Silver;
            this.grdMatchedTradeReport.DisplayLayout.Override.RowAppearance = appearance13;
            this.grdMatchedTradeReport.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdMatchedTradeReport.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
            this.grdMatchedTradeReport.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdMatchedTradeReport.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdMatchedTradeReport.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdMatchedTradeReport.Location = new System.Drawing.Point(0, 59);
            this.grdMatchedTradeReport.Name = "grdMatchedTradeReport";
            this.grdMatchedTradeReport.Size = new System.Drawing.Size(547, 301);
            this.grdMatchedTradeReport.TabIndex = 62;
            this.grdMatchedTradeReport.Text = "ultraGrid1";
            this.grdMatchedTradeReport.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdMatchedTradeReport_InitializeLayout);
            // 
            // CtrlCloseTradeMatchedTradeReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grdMatchedTradeReport);
            this.Controls.Add(this.cmbDate);
            this.Controls.Add(this.lblMatchedTradeDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlCloseTradeMatchedTradeReport";
            this.Size = new System.Drawing.Size(547, 416);
            ((System.ComponentModel.ISupportInitialize)(this.cmbDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMatchedTradeReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraLabel lblMatchedTradeDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo cmbDate;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdMatchedTradeReport;
    }
}
