namespace Prana.PM.Client.UI.Controls
{
	partial class CtrlSplit
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
            _buysides.Dispose();
            _sellsides.Dispose();
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
            this.btnSave = new System.Windows.Forms.Button();
            this.grdSplit = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.grdSplit)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(273, 166);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grdSplit
            // 
            this.grdSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdSplit.DisplayLayout.Appearance = appearance1;
            this.grdSplit.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSplit.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdSplit.DisplayLayout.GroupByBox.Hidden = true;
            this.grdSplit.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSplit.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.LightSlateGray;
            appearance2.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance2.BorderColor = System.Drawing.Color.DimGray;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.grdSplit.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdSplit.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSplit.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSplit.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.grdSplit.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdSplit.DisplayLayout.Override.CellPadding = 0;
            this.grdSplit.DisplayLayout.Override.CellSpacing = 0;
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8F;
            appearance3.TextHAlignAsString = "Center";
            this.grdSplit.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdSplit.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdSplit.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.grdSplit.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Middle";
            this.grdSplit.DisplayLayout.Override.RowAppearance = appearance5;
            this.grdSplit.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdSplit.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            appearance6.FontData.BoldAsString = "True";
            this.grdSplit.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.grdSplit.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdSplit.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdSplit.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSplit.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdSplit.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdSplit.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdSplit.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdSplit.DisplayLayout.Override.TemplateAddRowAppearance = appearance7;
            this.grdSplit.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdSplit.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdSplit.DisplayLayout.UseFixedHeaders = true;
            this.grdSplit.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdSplit.Location = new System.Drawing.Point(-3, 3);
            this.grdSplit.Name = "grdSplit";
            this.grdSplit.Size = new System.Drawing.Size(643, 160);
            this.grdSplit.TabIndex = 3;
            this.grdSplit.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSplit_InitializeLayout);
            this.grdSplit.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSplit_InitializeRow);
            this.grdSplit.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSplit_CellChange);
            // 
            // CtrlSplit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdSplit);
            this.Controls.Add(this.btnSave);
            this.Name = "CtrlSplit";
            this.Size = new System.Drawing.Size(643, 192);
            ((System.ComponentModel.ISupportInitialize)(this.grdSplit)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Button btnSave;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSplit;
	}
}
