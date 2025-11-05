namespace Prana.Blotter
{
    partial class CtrlTradingInstruction
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
            if (_sideList != null)
            {
                _sideList.Dispose();
                _sideList = null;
            }
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            this.grdDeskTrades = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnReject = new Infragistics.Win.Misc.UltraButton();
            this.btnAccept = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdDeskTrades)).BeginInit();
            this.SuspendLayout();
            // 
            // grdDeskTrades
            // 
            this.grdDeskTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdDeskTrades.DisplayLayout.Appearance = appearance1;
            this.grdDeskTrades.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdDeskTrades.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdDeskTrades.DisplayLayout.GroupByBox.Hidden = true;
            this.grdDeskTrades.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDeskTrades.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.Gold;
            appearance2.BorderColor = System.Drawing.Color.Black;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.grdDeskTrades.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdDeskTrades.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdDeskTrades.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdDeskTrades.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDeskTrades.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDeskTrades.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdDeskTrades.DisplayLayout.Override.CellPadding = 0;
            this.grdDeskTrades.DisplayLayout.Override.DefaultColWidth = 50;
            this.grdDeskTrades.DisplayLayout.Override.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Like;
            this.grdDeskTrades.DisplayLayout.Override.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.Hidden;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.grdDeskTrades.DisplayLayout.Override.GroupByRowAppearance = appearance3;
            this.grdDeskTrades.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value]";
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdDeskTrades.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.grdDeskTrades.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdDeskTrades.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdDeskTrades.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.Lime;
            this.grdDeskTrades.DisplayLayout.Override.RowAlternateAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            appearance6.ForeColor = System.Drawing.Color.Lime;
            this.grdDeskTrades.DisplayLayout.Override.RowAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdDeskTrades.DisplayLayout.Override.RowSelectorAppearance = appearance7;
            this.grdDeskTrades.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.grdDeskTrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdDeskTrades.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.grdDeskTrades.DisplayLayout.Override.SelectedRowAppearance = appearance8;
            this.grdDeskTrades.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdDeskTrades.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDeskTrades.DisplayLayout.Override.SpecialRowSeparator = ((Infragistics.Win.UltraWinGrid.SpecialRowSeparator)(((Infragistics.Win.UltraWinGrid.SpecialRowSeparator.TemplateAddRow | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FixedRows) 
            | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.SummaryRow)));
            appearance9.BackColor = System.Drawing.SystemColors.Info;
            this.grdDeskTrades.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance9;
            this.grdDeskTrades.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdDeskTrades.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook1.Appearance = appearance10;
            this.grdDeskTrades.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDeskTrades.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdDeskTrades.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdDeskTrades.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdDeskTrades.Location = new System.Drawing.Point(0, 3);
            this.grdDeskTrades.Name = "grdDeskTrades";
            this.grdDeskTrades.Size = new System.Drawing.Size(522, 307);
            this.grdDeskTrades.TabIndex = 5;
            this.grdDeskTrades.Text = "ultraGrid1";
            this.grdDeskTrades.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdDeskTrades_InitializeLayout);
            this.grdDeskTrades.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDeskTrades_CellChange);
            this.grdDeskTrades.Click += new System.EventHandler(this.grdDeskTrades_Click);
            // 
            // btnReject
            // 
            this.btnReject.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReject.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnReject.ImageSize = new System.Drawing.Size(75, 23);
            this.btnReject.Location = new System.Drawing.Point(280, 317);
            this.btnReject.Margin = new System.Windows.Forms.Padding(4);
            this.btnReject.Name = "btnReject";
            this.btnReject.ShowFocusRect = false;
            this.btnReject.ShowOutline = false;
            this.btnReject.Size = new System.Drawing.Size(77, 25);
            this.btnReject.TabIndex = 98;
            this.btnReject.Text = "Reject";
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAccept.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnAccept.ImageSize = new System.Drawing.Size(75, 23);
            this.btnAccept.Location = new System.Drawing.Point(168, 317);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.ShowFocusRect = false;
            this.btnAccept.ShowOutline = false;
            this.btnAccept.Size = new System.Drawing.Size(74, 25);
            this.btnAccept.TabIndex = 97;
            this.btnAccept.Text = "Accept";
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // CtrlTradingInstruction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.grdDeskTrades);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "CtrlTradingInstruction";
            this.Size = new System.Drawing.Size(525, 355);
            this.Load += new System.EventHandler(this.CtrlTradingInstruction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdDeskTrades)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdDeskTrades;
        private Infragistics.Win.Misc.UltraButton btnReject;
        private Infragistics.Win.Misc.UltraButton btnAccept;
    }
}
