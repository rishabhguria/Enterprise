using Prana.Utilities;
namespace Prana.CashManagement
{
    partial class ctrlActivityJournalMapping
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
                if (_vlActivityType != null)
                {
                    _vlActivityType.Dispose();
                    _vlActivityType = null;
                }
                if (_vlAmountType != null)
                {
                    _vlAmountType.Dispose();
                    _vlAmountType = null;
                }
                if (_vlCashActivityType != null)
                {
                    _vlCashActivityType.Dispose();
                    _vlCashActivityType = null;
                }
                if (_vlCashTransactionDateType != null)
                {
                    _vlCashTransactionDateType.Dispose();
                    _vlCashTransactionDateType = null;
                }
                if (_vlCashValueType != null)
                {
                    _vlCashValueType.Dispose();
                    _vlCashValueType = null;
                }
                if (_vlSubAccount != null)
                {
                    _vlSubAccount.Dispose();
                    _vlSubAccount = null;
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.grdJournalData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraTreeActivityType = new Infragistics.Win.UltraWinTree.UltraTree();
            this.txtFilterActivity = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblActivityJournalMapping = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel4 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            ((System.ComponentModel.ISupportInitialize)(this.grdJournalData)).BeginInit();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTreeActivityType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFilterActivity)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            this.ultraPanel4.ClientArea.SuspendLayout();
            this.ultraPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(674, 24);
            this.ultraLabel2.TabIndex = 16;
            this.ultraLabel2.Text = "Activity Journal Mapping";
            // 
            // grdJournalData
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdJournalData.DisplayLayout.Appearance = appearance1;
            this.grdJournalData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdJournalData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdJournalData.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdJournalData.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdJournalData.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdJournalData.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdJournalData.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdJournalData.DisplayLayout.GroupByBox.ShowBandLabels = Infragistics.Win.UltraWinGrid.ShowBandLabels.None;
            this.grdJournalData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdJournalData.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdJournalData.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdJournalData.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdJournalData.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            this.grdJournalData.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdJournalData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdJournalData.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdJournalData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdJournalData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdJournalData.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdJournalData.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdJournalData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdJournalData.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdJournalData.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.FontData.BoldAsString = "True";
            appearance10.TextHAlignAsString = "Center";
            this.grdJournalData.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdJournalData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdJournalData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdJournalData.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdJournalData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdJournalData.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdJournalData.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdJournalData.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdJournalData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdJournalData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdJournalData.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdJournalData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdJournalData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdJournalData.Location = new System.Drawing.Point(0, 24);
            this.grdJournalData.Name = "grdJournalData";
            this.grdJournalData.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdJournalData.Size = new System.Drawing.Size(674, 322);
            this.grdJournalData.TabIndex = 15;
            this.grdJournalData.Text = "ultraGrid1";
            this.grdJournalData.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdJournalData_AfterCellUpdate);
            this.grdJournalData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdJournalData_InitializeLayout);
            this.grdJournalData.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdJournalData_InitializeRow);
            this.grdJournalData.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdJournalData_CellChange);
            this.grdJournalData.CellDataError += new Infragistics.Win.UltraWinGrid.CellDataErrorEventHandler(this.grdJournalData_CellDataError);
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraTreeActivityType);
            this.ultraPanel2.ClientArea.Controls.Add(this.txtFilterActivity);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(202, 346);
            this.ultraPanel2.TabIndex = 17;
            // 
            // ultraTreeActivityType
            // 
            appearance13.BackColor = System.Drawing.Color.Black;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.ForeColor = System.Drawing.Color.White;
            this.ultraTreeActivityType.Appearance = appearance13;
            this.ultraTreeActivityType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTreeActivityType.Location = new System.Drawing.Point(0, 22);
            this.ultraTreeActivityType.Name = "ultraTreeActivityType";
            this.ultraTreeActivityType.Size = new System.Drawing.Size(202, 324);
            this.ultraTreeActivityType.TabIndex = 24;
            this.ultraTreeActivityType.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.ultraTreeActivityType_AfterSelect);
            // 
            // txtFilterActivity
            // 
            appearance14.ForeColor = System.Drawing.Color.DimGray;
            this.txtFilterActivity.Appearance = appearance14;
            this.txtFilterActivity.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFilterActivity.Location = new System.Drawing.Point(0, 0);
            this.txtFilterActivity.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txtFilterActivity.Name = "txtFilterActivity";
            this.txtFilterActivity.NullText = "Filter Activity Type";
            appearance15.ForeColor = System.Drawing.Color.DimGray;
            this.txtFilterActivity.NullTextAppearance = appearance15;
            this.txtFilterActivity.Size = new System.Drawing.Size(202, 22);
            this.txtFilterActivity.TabIndex = 26;
            this.txtFilterActivity.ValueChanged += new System.EventHandler(this.txtFilterActivity_ValueChanged);
            // 
            // lblActivityJournalMapping
            // 
            this.lblActivityJournalMapping.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblActivityJournalMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblActivityJournalMapping.Location = new System.Drawing.Point(0, 0);
            this.lblActivityJournalMapping.Name = "lblActivityJournalMapping";
            this.lblActivityJournalMapping.Size = new System.Drawing.Size(882, 25);
            this.lblActivityJournalMapping.TabIndex = 18;
            this.lblActivityJournalMapping.Text = "Activity Journal Mapping";
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.grdJournalData);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabel2);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(208, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(674, 346);
            this.ultraPanel1.TabIndex = 19;
            // 
            // ultraPanel3
            // 
            // 
            // ultraPanel3.ClientArea
            // 
            this.ultraPanel3.ClientArea.Controls.Add(this.lblActivityJournalMapping);
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(882, 25);
            this.ultraPanel3.TabIndex = 20;
            // 
            // ultraPanel4
            // 
            // 
            // ultraPanel4.ClientArea
            // 
            this.ultraPanel4.ClientArea.Controls.Add(this.ultraPanel1);
            this.ultraPanel4.ClientArea.Controls.Add(this.ultraSplitter1);
            this.ultraPanel4.ClientArea.Controls.Add(this.ultraPanel2);
            this.ultraPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel4.Location = new System.Drawing.Point(0, 25);
            this.ultraPanel4.Name = "ultraPanel4";
            this.ultraPanel4.Size = new System.Drawing.Size(882, 346);
            this.ultraPanel4.TabIndex = 21;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.ultraSplitter1.Enabled = false;
            this.ultraSplitter1.Location = new System.Drawing.Point(202, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 0;
            this.ultraSplitter1.Size = new System.Drawing.Size(6, 346);
            this.ultraSplitter1.TabIndex = 20;
            // 
            // ctrlActivityJournalMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel4);
            this.Controls.Add(this.ultraPanel3);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "ctrlActivityJournalMapping";
            this.Size = new System.Drawing.Size(882, 371);
            this.Load += new System.EventHandler(this.ctrlActivityJournalMapping_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdJournalData)).EndInit();
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ClientArea.PerformLayout();
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTreeActivityType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFilterActivity)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            this.ultraPanel4.ClientArea.ResumeLayout(false);
            this.ultraPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdJournalData;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraLabel lblActivityJournalMapping;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraPanel ultraPanel4;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.UltraWinTree.UltraTree ultraTreeActivityType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFilterActivity;
    }
}
