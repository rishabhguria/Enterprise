namespace Prana.AllocationNew.Allocation.UI
{
    partial class FilterGrid
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
            if (disposing)
            {
                foreach (AutoCompleteTextBox at in _liAutoCompleteTextBox)
                {
                    at.Dispose();
                }
                _filterGrid.AfterColPosChanged -= new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(_filterGrid_AfterColPosChanged);
                _filterGrid.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(_filterGrid_ClickCellButton);
            }
            UnWireEvents();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this._filterGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._filterGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // _filterGrid
            // 
            ultraGridBand1.CardView = true;
            this._filterGrid.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            appearance1.BackColor = System.Drawing.Color.Bisque;
            appearance1.ForeColor = System.Drawing.Color.Gray;
            this._filterGrid.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance1;
            this._filterGrid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this._filterGrid.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            appearance2.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.BorderColor = System.Drawing.Color.DarkGray;
            appearance2.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this._filterGrid.DisplayLayout.Override.CellAppearance = appearance2;
            this._filterGrid.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.HeadersOnly;
            this._filterGrid.DisplayLayout.Override.FilterClearButtonLocation = Infragistics.Win.UltraWinGrid.FilterClearButtonLocation.RowAndCell;
            this._filterGrid.DisplayLayout.Override.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Contains;
            this._filterGrid.DisplayLayout.Override.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.Hidden;
            this._filterGrid.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            this._filterGrid.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            appearance3.ForeColor = System.Drawing.Color.Red;
            this._filterGrid.DisplayLayout.Override.SelectedCellAppearance = appearance3;
            this._filterGrid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._filterGrid.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            this._filterGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._filterGrid.DisplayLayout.UseFixedHeaders = true;
            this._filterGrid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._filterGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filterGrid.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._filterGrid.Location = new System.Drawing.Point(0, 0);
            this._filterGrid.Name = "_filterGrid";
            this._filterGrid.Size = new System.Drawing.Size(500, 60);
            this._filterGrid.TabIndex = 0;
            this._filterGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._filterGrid_InitializeLayout);
            this._filterGrid.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this._filterGrid_BeforeCustomRowFilterDialog);
            this._filterGrid.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this._filterGrid_BeforeColumnChooserDisplayed);
            // 
            // FilterGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this._filterGrid);
            this.MinimumSize = new System.Drawing.Size(500, 60);
            this.Name = "FilterGrid";
            this.Size = new System.Drawing.Size(500, 60);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            ((System.ComponentModel.ISupportInitialize)(this._filterGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid _filterGrid;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
    }
}
