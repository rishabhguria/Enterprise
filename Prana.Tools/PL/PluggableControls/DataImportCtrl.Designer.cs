namespace Prana.Tools
{
    partial class DataImportCtrl
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
                if (_dt != null)
                {
                    _dt.Dispose();
                }
                if (timerSMRequest != null)
                {
                    timerSMRequest.Dispose();
                }
                if (_bgImportData != null)
                {
                    _bgImportData.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            this.grdData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnImport = new Infragistics.Win.Misc.UltraButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cmbbxXslts = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraExpandableGroupBox2 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxXslts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).BeginInit();
            this.ultraExpandableGroupBox2.SuspendLayout();
            this.ultraExpandableGroupBoxPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdData
            // 
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "Tahoma";
            appearance1.FontData.SizeInPoints = 8.25F;
            this.grdData.DisplayLayout.Appearance = appearance1;
            this.grdData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance2.BackColor = System.Drawing.Color.White;
            this.grdData.DisplayLayout.CaptionAppearance = appearance2;
            this.grdData.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.GroupByBox.Hidden = true;
            this.grdData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdData.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdData.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdData.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdData.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdData.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdData.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.FontData.BoldAsString = "False";
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8.25F;
            this.grdData.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdData.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.OncePerGroupedRowIsland;
            this.grdData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.grdData.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            this.grdData.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdData.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdData.Location = new System.Drawing.Point(3, 3);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(502, 239);
            this.grdData.TabIndex = 17;
            this.grdData.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdData.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdData_InitializeLayout);
            this.grdData.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdData_InitializeRow);
            this.grdData.AfterRowFilterChanged += new Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventHandler(this.grdData_AfterRowFilterChanged);
            this.grdData.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdData_BeforeColumnChooserDisplayed);
            this.grdData.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdData_BeforeCustomRowFilterDialog);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.Appearance = appearance5;
            this.btnImport.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(267, 245);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(74, 22);
            this.btnImport.TabIndex = 18;
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cmbbxXslts
            // 
            this.cmbbxXslts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxXslts.DisplayLayout.Appearance = appearance6;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbbxXslts.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbbxXslts.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxXslts.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cmbbxXslts.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
            appearance7.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance7.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxXslts.DisplayLayout.GroupByBox.Appearance = appearance7;
            appearance8.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxXslts.DisplayLayout.GroupByBox.BandLabelAppearance = appearance8;
            this.cmbbxXslts.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance9.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance9.BackColor2 = System.Drawing.SystemColors.Control;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxXslts.DisplayLayout.GroupByBox.PromptAppearance = appearance9;
            this.cmbbxXslts.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxXslts.DisplayLayout.MaxRowScrollRegions = 1;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxXslts.DisplayLayout.Override.ActiveCellAppearance = appearance10;
            appearance11.BackColor = System.Drawing.SystemColors.Highlight;
            appearance11.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxXslts.DisplayLayout.Override.ActiveRowAppearance = appearance11;
            this.cmbbxXslts.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxXslts.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxXslts.DisplayLayout.Override.CardAreaAppearance = appearance12;
            appearance13.BorderColor = System.Drawing.Color.Silver;
            appearance13.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxXslts.DisplayLayout.Override.CellAppearance = appearance13;
            this.cmbbxXslts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxXslts.DisplayLayout.Override.CellPadding = 0;
            appearance14.BackColor = System.Drawing.SystemColors.Control;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxXslts.DisplayLayout.Override.GroupByRowAppearance = appearance14;
            appearance15.TextHAlignAsString = "Left";
            this.cmbbxXslts.DisplayLayout.Override.HeaderAppearance = appearance15;
            this.cmbbxXslts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxXslts.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            appearance16.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxXslts.DisplayLayout.Override.RowAppearance = appearance16;
            this.cmbbxXslts.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance17.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxXslts.DisplayLayout.Override.TemplateAddRowAppearance = appearance17;
            this.cmbbxXslts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxXslts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxXslts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxXslts.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxXslts.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbbxXslts.Location = new System.Drawing.Point(53, 246);
            this.cmbbxXslts.Name = "cmbbxXslts";
            this.cmbbxXslts.Size = new System.Drawing.Size(105, 24);
            this.cmbbxXslts.TabIndex = 117;
            this.cmbbxXslts.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxXslts.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbbxXslts_InitializeLayout);
            // 
            // ultraExpandableGroupBox2
            // 
            this.ultraExpandableGroupBox2.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.ultraExpandableGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox2.ExpandedSize = new System.Drawing.Size(528, 276);
            this.ultraExpandableGroupBox2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraExpandableGroupBox2.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            this.ultraExpandableGroupBox2.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.RightOnBorder;
            this.ultraExpandableGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox2.Name = "ultraExpandableGroupBox2";
            this.ultraExpandableGroupBox2.Size = new System.Drawing.Size(528, 276);
            this.ultraExpandableGroupBox2.TabIndex = 119;
            this.ultraExpandableGroupBox2.Text = "Application Data";
            this.ultraExpandableGroupBox2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.grdData);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.cmbbxXslts);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.btnImport);
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(3, 3);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(502, 270);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // DataImportCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraExpandableGroupBox2);
            this.Name = "DataImportCtrl";
            this.Size = new System.Drawing.Size(528, 276);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxXslts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).EndInit();
            this.ultraExpandableGroupBox2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdData;
        private Infragistics.Win.Misc.UltraButton btnImport;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxXslts;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox2;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
    }
}
