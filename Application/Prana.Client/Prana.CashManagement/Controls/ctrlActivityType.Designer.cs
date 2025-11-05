namespace Prana.CashManagement
{
    partial class ctrlActivityType
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
                if (_dsActivities != null)
                {
                    _dsActivities.Dispose();
                }
                if (_vlActivitySourceType != null)
                {
                    _vlActivitySourceType.Dispose();
                    _vlActivitySourceType = null;
                }
                if (_vlBalanceType != null)
                {
                    _vlBalanceType.Dispose();
                    _vlBalanceType = null;
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
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.grdActivityTypeDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lblActivityJournalMapping = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdActivityTypeDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(0, 30);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(882, 24);
            this.ultraLabel2.TabIndex = 16;
            this.ultraLabel2.Text = "Activity Type Details";
            // 
            // grdActivityTypeDetails
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdActivityTypeDetails.DisplayLayout.Appearance = appearance1;
            this.grdActivityTypeDetails.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdActivityTypeDetails.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdActivityTypeDetails.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdActivityTypeDetails.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdActivityTypeDetails.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdActivityTypeDetails.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdActivityTypeDetails.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdActivityTypeDetails.DisplayLayout.GroupByBox.ShowBandLabels = Infragistics.Win.UltraWinGrid.ShowBandLabels.None;
            this.grdActivityTypeDetails.DisplayLayout.MaxColScrollRegions = 1;
            this.grdActivityTypeDetails.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdActivityTypeDetails.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdActivityTypeDetails.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdActivityTypeDetails.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            this.grdActivityTypeDetails.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivityTypeDetails.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivityTypeDetails.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivityTypeDetails.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdActivityTypeDetails.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdActivityTypeDetails.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdActivityTypeDetails.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdActivityTypeDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdActivityTypeDetails.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdActivityTypeDetails.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.FontData.BoldAsString = "True";
            appearance10.TextHAlignAsString = "Center";
            this.grdActivityTypeDetails.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdActivityTypeDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdActivityTypeDetails.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdActivityTypeDetails.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdActivityTypeDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivityTypeDetails.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdActivityTypeDetails.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdActivityTypeDetails.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdActivityTypeDetails.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdActivityTypeDetails.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdActivityTypeDetails.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdActivityTypeDetails.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdActivityTypeDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdActivityTypeDetails.Location = new System.Drawing.Point(0, 54);
            this.grdActivityTypeDetails.Name = "grdActivityTypeDetails";
            this.grdActivityTypeDetails.Size = new System.Drawing.Size(882, 317);
            this.grdActivityTypeDetails.TabIndex = 15;
            this.grdActivityTypeDetails.Text = "ultraGrid1";
            this.grdActivityTypeDetails.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdActivityTypeDetails_AfterCellUpdate);
            this.grdActivityTypeDetails.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdActivityTypeDetails_InitializeLayout);
            this.grdActivityTypeDetails.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdActivityTypeDetails_CellChange);
            // 
            // lblActivityJournalMapping
            // 
            this.lblActivityJournalMapping.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblActivityJournalMapping.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblActivityJournalMapping.Location = new System.Drawing.Point(0, 0);
            this.lblActivityJournalMapping.Name = "lblActivityJournalMapping";
            this.lblActivityJournalMapping.Size = new System.Drawing.Size(882, 30);
            this.lblActivityJournalMapping.TabIndex = 18;
            this.lblActivityJournalMapping.Text = "Activity Types";
            // 
            // ctrlActivityType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdActivityTypeDetails);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.lblActivityJournalMapping);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "ctrlActivityType";
            this.Size = new System.Drawing.Size(882, 371);
            this.Load += new System.EventHandler(this.ctrlActivityType_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdActivityTypeDetails)).EndInit();
            this.ResumeLayout(false);

        }

        

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdActivityTypeDetails;
        private Infragistics.Win.Misc.UltraLabel lblActivityJournalMapping;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
    }
}
