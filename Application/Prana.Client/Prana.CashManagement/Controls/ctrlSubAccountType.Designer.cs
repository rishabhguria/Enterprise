using Infragistics.Win.UltraWinGrid;
namespace Prana.CashManagement.Controls
{
    partial class ctrlSubAccountType
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
                if (_dataSetMasterCategory != null)
                {
                    _dataSetMasterCategory.Dispose();
                }
                if (_dtSubAccounts != null)
                {
                    _dtSubAccounts.Dispose();
                }
                if (_dtSubAccountType != null)
                {
                    _dtSubAccountType.Dispose();
                }
                if (_dtTransactionSource != null)
                {
                    _dtTransactionSource.Dispose();
                }
                if (_vlTransactionSource != null)
                {
                    _vlTransactionSource.Dispose();
                    _vlTransactionSource = null;
                }
            }
            base.Dispose(disposing);
        }
        public delegate void CancelableCellEventHandler(object sender, CancelableCellEventArgs e);
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
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.grdSubAccountDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.grdSubAccountDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // grdSubAccountDetails
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdSubAccountDetails.DisplayLayout.Appearance = appearance1;
            this.grdSubAccountDetails.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSubAccountDetails.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdSubAccountDetails.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdSubAccountDetails.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdSubAccountDetails.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSubAccountDetails.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdSubAccountDetails.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdSubAccountDetails.DisplayLayout.GroupByBox.ShowBandLabels = Infragistics.Win.UltraWinGrid.ShowBandLabels.None;
            this.grdSubAccountDetails.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSubAccountDetails.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdSubAccountDetails.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdSubAccountDetails.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdSubAccountDetails.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            this.grdSubAccountDetails.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdSubAccountDetails.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdSubAccountDetails.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdSubAccountDetails.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdSubAccountDetails.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdSubAccountDetails.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdSubAccountDetails.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdSubAccountDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdSubAccountDetails.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdSubAccountDetails.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.FontData.BoldAsString = "True";
            appearance10.TextHAlignAsString = "Center";
            this.grdSubAccountDetails.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdSubAccountDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdSubAccountDetails.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdSubAccountDetails.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdSubAccountDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdSubAccountDetails.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdSubAccountDetails.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdSubAccountDetails.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdSubAccountDetails.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdSubAccountDetails.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdSubAccountDetails.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdSubAccountDetails.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdSubAccountDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSubAccountDetails.Location = new System.Drawing.Point(0, 54);
            this.grdSubAccountDetails.Name = "grdSubAccountDetails";
            this.grdSubAccountDetails.Size = new System.Drawing.Size(246, 125);
            this.grdSubAccountDetails.TabIndex = 15;
            this.grdSubAccountDetails.Text = "ultraGrid1";
            this.grdSubAccountDetails.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSubAccountDetails_InitializeLayout);
            this.grdSubAccountDetails.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSubAccountDetails_CellValueChanged);
            
            // 
            // ctrlSubAccountType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdSubAccountDetails);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "ctrlSubAccountType";
            this.Size = new System.Drawing.Size(246, 179);
            this.Load += new System.EventHandler(this.CtrlSubAccountType_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdSubAccountDetails)).EndInit();
            this.ResumeLayout(false);

        }

        private Infragistics.Win.UltraWinGrid.UltraGrid grdSubAccountDetails;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        #endregion

    }
}
