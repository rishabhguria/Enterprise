namespace Prana.PM.Client.UI.Forms
{
    partial class PMTaxLotsDisplayForm
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
                if (_pmAppearances != null)
                {
                    _pmAppearances.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PMTaxLotsDisplayForm));
            this.ultraGrid1 = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGrid1
            // 
            this.ultraGrid1.ContextMenuStrip = this.contextMenuStrip;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.ultraGrid1.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.Indentation = 0;
            appearance2.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance2.BackColor2 = System.Drawing.Color.DarkGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            ultraGridBand1.Override.HeaderAppearance = appearance2;
            ultraGridBand1.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance3.TextVAlignAsString = "Middle";
            ultraGridBand1.Override.RowAlternateAppearance = appearance3;
            appearance4.TextVAlignAsString = "Middle";
            ultraGridBand1.Override.RowAppearance = appearance4;
            this.ultraGrid1.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ultraGrid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            appearance5.BackColor = System.Drawing.Color.Silver;
            appearance5.BackColor2 = System.Drawing.Color.DimGray;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            this.ultraGrid1.DisplayLayout.GroupByBox.Appearance = appearance5;
            this.ultraGrid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid1.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.Black;
            this.ultraGrid1.DisplayLayout.GroupByBox.ButtonConnectorStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid1.DisplayLayout.GroupByBox.Hidden = true;
            this.ultraGrid1.DisplayLayout.MaxBandDepth = 4;
            this.ultraGrid1.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGrid1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.Color.LightSlateGray;
            appearance6.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance6.BorderColor = System.Drawing.Color.DimGray;
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            this.ultraGrid1.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ultraGrid1.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ultraGrid1.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            this.ultraGrid1.DisplayLayout.Override.CellPadding = 0;
            this.ultraGrid1.DisplayLayout.Override.CellSpacing = 0;
            this.ultraGrid1.DisplayLayout.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.CheckOnDisplay;
            appearance7.BackColor = System.Drawing.Color.Gray;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance7.BorderColor = System.Drawing.Color.Black;
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.White;
            this.ultraGrid1.DisplayLayout.Override.GroupByRowAppearance = appearance7;
            this.ultraGrid1.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
            this.ultraGrid1.DisplayLayout.Override.GroupByRowPadding = 0;
            this.ultraGrid1.DisplayLayout.Override.GroupByRowSpacingAfter = 0;
            this.ultraGrid1.DisplayLayout.Override.GroupByRowSpacingBefore = 0;
            this.ultraGrid1.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            appearance8.TextHAlignAsString = "Right";
            appearance8.TextVAlignAsString = "Middle";
            this.ultraGrid1.DisplayLayout.Override.GroupBySummaryValueAppearance = appearance8;
            appearance9.FontData.BoldAsString = "False";
            appearance9.FontData.SizeInPoints = 8F;
            appearance9.TextHAlignAsString = "Center";
            appearance9.TextVAlignAsString = "Middle";
            this.ultraGrid1.DisplayLayout.Override.HeaderAppearance = appearance9;
            this.ultraGrid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.ultraGrid1.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.ultraGrid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance10.TextHAlignAsString = "Right";
            appearance10.TextVAlignAsString = "Middle";
            this.ultraGrid1.DisplayLayout.Override.RowAlternateAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.Black;
            appearance11.TextHAlignAsString = "Right";
            appearance11.TextVAlignAsString = "Middle";
            this.ultraGrid1.DisplayLayout.Override.RowAppearance = appearance11;
            this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.ultraGrid1.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            appearance12.FontData.BoldAsString = "True";
            this.ultraGrid1.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.ultraGrid1.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ultraGrid1.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ultraGrid1.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGrid1.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ultraGrid1.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.ultraGrid1.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.ultraGrid1.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            appearance13.BorderColor = System.Drawing.Color.Transparent;
            this.ultraGrid1.DisplayLayout.Override.SummaryFooterAppearance = appearance13;
            this.ultraGrid1.DisplayLayout.Override.SummaryFooterSpacingAfter = 0;
            this.ultraGrid1.DisplayLayout.Override.SummaryFooterSpacingBefore = 0;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            appearance14.BorderColor = System.Drawing.Color.Transparent;
            appearance14.FontData.BoldAsString = "False";
            appearance14.ForeColor = System.Drawing.Color.White;
            this.ultraGrid1.DisplayLayout.Override.SummaryValueAppearance = appearance14;
            this.ultraGrid1.DisplayLayout.PriorityScrolling = true;
            this.ultraGrid1.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this.ultraGrid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGrid1.DisplayLayout.UseFixedHeaders = true;
            this.ultraGrid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGrid1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(177)));
            this.ultraGrid1.Location = new System.Drawing.Point(4, 27);
            this.ultraGrid1.Name = "ultraGrid1";
            this.ultraGrid1.Size = new System.Drawing.Size(782, 298);
            this.ultraGrid1.TabIndex = 0;
            this.ultraGrid1.Text = "ultraGrid1";
            this.ultraGrid1.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
            this.ultraGrid1.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraGrid1_InitializeRow);
            this.ultraGrid1.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.ultraGrid1_BeforeCustomRowFilterDialog);
            this.ultraGrid1.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.ultraGrid1_BeforeColumnChooserDisplayed);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip2";
            this.contextMenuStrip.Size = new System.Drawing.Size(138, 26);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveLayoutToolStripMenuItem.Image")));
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click_1);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left
            // 
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left.Name = "_PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left";
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 298);
            // 
            // _PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right
            // 
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(786, 27);
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right.Name = "_PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right";
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 298);
            // 
            // _PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top
            // 
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top.Name = "_PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top";
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(790, 27);
            // 
            // _PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 325);
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom.Name = "_PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom";
            this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(790, 4);
            // 
            // PMTaxLotsDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 329);
            this.Controls.Add(this.ultraGrid1);
            this.Controls.Add(this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom);
            this.Name = "PMTaxLotsDisplayForm";
            this.Text = "Taxlot Details";
            this.Load += new System.EventHandler(this.PMTaxLotsDisplayForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Bottom;
    }
}