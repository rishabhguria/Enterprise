namespace Prana.DropCopyClient
{
    partial class OutBox
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutBox));
            this.grdBasket = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pnlUpper = new System.Windows.Forms.Panel();
            this.txtBoxSymbol = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            this.cmbbxSide = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblSide = new System.Windows.Forms.Label();
            this.lblSymbol = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdBasket)).BeginInit();
            this.pnlUpper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxSide)).BeginInit();
            this.SuspendLayout();
            // 
            // grdBasket
            // 
            this.grdBasket.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.Black;
            appearance4.BackColor2 = System.Drawing.Color.Black;
            appearance4.BorderColor = System.Drawing.Color.Black;
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Tahoma";
            appearance4.FontData.SizeInPoints = 8.25F;
            this.grdBasket.DisplayLayout.Appearance = appearance4;
            this.grdBasket.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdBasket.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance5.BackColor = System.Drawing.Color.White;
            this.grdBasket.DisplayLayout.CaptionAppearance = appearance5;
            this.grdBasket.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdBasket.DisplayLayout.GroupByBox.Hidden = true;
            this.grdBasket.DisplayLayout.MaxColScrollRegions = 1;
            this.grdBasket.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdBasket.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdBasket.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdBasket.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdBasket.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdBasket.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdBasket.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdBasket.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.CellsOnly;
            appearance6.BackColor = System.Drawing.Color.DimGray;
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.Name = "Tahoma";
            appearance6.FontData.SizeInPoints = 8.25F;
            this.grdBasket.DisplayLayout.Override.HeaderAppearance = appearance6;
            this.grdBasket.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdBasket.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdBasket.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdBasket.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdBasket.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdBasket.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdBasket.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdBasket.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdBasket.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdBasket.Location = new System.Drawing.Point(0, 40);
            this.grdBasket.Name = "grdBasket";
            this.grdBasket.Size = new System.Drawing.Size(714, 292);
            this.grdBasket.TabIndex = 16;
            this.grdBasket.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
            // 
            // pnlUpper
            // 
            this.pnlUpper.BackColor = System.Drawing.Color.DimGray;
            this.pnlUpper.Controls.Add(this.txtBoxSymbol);
            this.pnlUpper.Controls.Add(this.btnClear);
            this.pnlUpper.Controls.Add(this.btnFilter);
            this.pnlUpper.Controls.Add(this.cmbbxSide);
            this.pnlUpper.Controls.Add(this.lblSide);
            this.pnlUpper.Controls.Add(this.lblSymbol);
            this.pnlUpper.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUpper.Location = new System.Drawing.Point(0, 0);
            this.pnlUpper.Name = "pnlUpper";
            this.pnlUpper.Size = new System.Drawing.Size(714, 43);
            this.pnlUpper.TabIndex = 17;
            // 
            // txtBoxSymbol
            // 
            this.txtBoxSymbol.Location = new System.Drawing.Point(58, 13);
            this.txtBoxSymbol.Name = "txtBoxSymbol";
            this.txtBoxSymbol.Size = new System.Drawing.Size(91, 20);
            this.txtBoxSymbol.TabIndex = 55;
            // 
            // btnClear
            // 
            this.btnClear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClear.BackgroundImage")));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(368, 14);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(63, 18);
            this.btnClear.TabIndex = 54;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFilter.BackgroundImage")));
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Location = new System.Drawing.Point(299, 14);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(63, 18);
            this.btnFilter.TabIndex = 53;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // cmbbxSide
            // 
            this.cmbbxSide.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbbxSide.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.cmbbxSide.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.cmbbxSide.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.cmbbxSide.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.cmbbxSide.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            this.cmbbxSide.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxSide.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.cmbbxSide.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.cmbbxSide.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.cmbbxSide.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxSide.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxSide.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this.cmbbxSide.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.cmbbxSide.DisplayMember = "";
            this.cmbbxSide.Location = new System.Drawing.Point(195, 12);
            this.cmbbxSide.Name = "cmbbxSide";
            this.cmbbxSide.Size = new System.Drawing.Size(86, 22);
            this.cmbbxSide.TabIndex = 5;
            this.cmbbxSide.Text = "Select";
            this.cmbbxSide.ValueMember = "";
            // 
            // lblSide
            // 
            this.lblSide.AutoSize = true;
            this.lblSide.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblSide.Location = new System.Drawing.Point(164, 16);
            this.lblSide.Name = "lblSide";
            this.lblSide.Size = new System.Drawing.Size(28, 13);
            this.lblSide.TabIndex = 4;
            this.lblSide.Text = "Side";
            // 
            // lblSymbol
            // 
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblSymbol.Location = new System.Drawing.Point(12, 16);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(41, 13);
            this.lblSymbol.TabIndex = 3;
            this.lblSymbol.Text = "Symbol";
            // 
            // OutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(714, 332);
            this.Controls.Add(this.pnlUpper);
            this.Controls.Add(this.grdBasket);
            this.MaximizeBox = false;
            this.Name = "OutBox";
            this.Text = "OutBox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OutBox_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.grdBasket)).EndInit();
            this.pnlUpper.ResumeLayout(false);
            this.pnlUpper.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxSide)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdBasket;
        private System.Windows.Forms.Panel pnlUpper;
        private System.Windows.Forms.Label lblSide;
        private System.Windows.Forms.Label lblSymbol;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxSide;
        private System.Windows.Forms.TextBox txtBoxSymbol;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnFilter;
    }
}