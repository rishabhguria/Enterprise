using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Prana.WashSale.Controls
{
    partial class WashSaleTradesGridUC
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
            this.washSaleGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraGridBagLayoutManager1 = new Infragistics.Win.Misc.UltraGridBagLayoutManager(this.components);
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.washSaleGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // washSaleGrid
            // 
            this.washSaleGrid.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.washSaleGrid.DisplayLayout.Appearance.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.washSaleGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.washSaleGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.washSaleGrid.DisplayLayout.GroupByBox.Appearance.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.washSaleGrid.DisplayLayout.GroupByBox.Appearance.BackColor2 = System.Drawing.SystemColors.ControlDark;
            this.washSaleGrid.DisplayLayout.GroupByBox.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.washSaleGrid.DisplayLayout.GroupByBox.BandLabelAppearance.ForeColor = System.Drawing.SystemColors.GrayText;
            this.washSaleGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.washSaleGrid.DisplayLayout.GroupByBox.PromptAppearance.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.washSaleGrid.DisplayLayout.GroupByBox.PromptAppearance.BackColor2 = System.Drawing.SystemColors.Control;
            this.washSaleGrid.DisplayLayout.GroupByBox.PromptAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            this.washSaleGrid.DisplayLayout.GroupByBox.PromptAppearance.ForeColor = System.Drawing.SystemColors.GrayText;
            this.washSaleGrid.DisplayLayout.MaxColScrollRegions = 1;
            this.washSaleGrid.DisplayLayout.MaxRowScrollRegions = 1;
            this.washSaleGrid.DisplayLayout.Override.ActiveCellAppearance.ForeColor = System.Drawing.SystemColors.ControlText;  
            this.washSaleGrid.DisplayLayout.Override.ActiveRowAppearance.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.washSaleGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.washSaleGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Default;
            this.washSaleGrid.DisplayLayout.Override.CellAppearance.BorderColor = System.Drawing.Color.Silver;
            this.washSaleGrid.DisplayLayout.Override.CellAppearance.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.washSaleGrid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;     
            this.washSaleGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;   
            this.washSaleGrid.DisplayLayout.Override.CellPadding = 0;
            this.washSaleGrid.DisplayLayout.Override.BorderStyleCell=UIElementBorderStyle.None;
            this.washSaleGrid.DisplayLayout.Override.GroupByRowAppearance.BackColor = System.Drawing.SystemColors.Control;
            this.washSaleGrid.DisplayLayout.Override.GroupByRowAppearance.BackColor2 = System.Drawing.SystemColors.ControlDark;
            this.washSaleGrid.DisplayLayout.Override.GroupByRowAppearance.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            this.washSaleGrid.DisplayLayout.Override.GroupByRowAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            this.washSaleGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            this.washSaleGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.washSaleGrid.DisplayLayout.Override.SelectTypeRow = SelectType.None;    
            this.washSaleGrid.DisplayLayout.Override.SelectTypeCell = SelectType.SingleAutoDrag;  
            this.washSaleGrid.DisplayLayout.Override.SelectTypeCol = SelectType.None;
            this.washSaleGrid.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.True; 
            this.washSaleGrid.DisplayLayout.Override.RowAppearance.BorderColor = System.Drawing.Color.Gray;
            this.washSaleGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.washSaleGrid.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            this.washSaleGrid.DisplayLayout.Override.DataErrorCellAppearance.ImageHAlign = HAlign.Right;
            this.washSaleGrid.SyncWithCurrencyManager = false;
            this.washSaleGrid.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
            this.washSaleGrid.DisplayLayout.Override.TemplateAddRowAppearance.BackColor = System.Drawing.SystemColors.ControlLight;
            this.washSaleGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.washSaleGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.washSaleGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.washSaleGrid.Location = new System.Drawing.Point(0, 0);
            this.washSaleGrid.Name = "washSaleGrid";
            this.washSaleGrid.Size = new System.Drawing.Size(552, 153);
            this.washSaleGrid.TabIndex = 0;
            this.washSaleGrid.Text = "washSaleGrid";
            this.washSaleGrid.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
            this.washSaleGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.WashSaleGrid_InitializeLayout);
            this.washSaleGrid.AfterExitEditMode += washSaleGrid_AfterExitEditMode;
            this.washSaleGrid.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.GridDataChanged);
            this.washSaleGrid.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.DisableGrid);
            this.washSaleGrid.InitializeRow += new InitializeRowEventHandler(this.WashSaleGrid_InitializeRow);
            this.washSaleGrid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WashSaleGrid_KeyPress);
            // 
            // WashSaleTradesGridUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.washSaleGrid);
            this.Name = "WashSaleTradesGridUC";
            this.Size = new System.Drawing.Size(552, 153);
            ((System.ComponentModel.ISupportInitialize)(this.washSaleGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid washSaleGrid;
        private Infragistics.Win.Misc.UltraGridBagLayoutManager ultraGridBagLayoutManager1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;

    }
}
