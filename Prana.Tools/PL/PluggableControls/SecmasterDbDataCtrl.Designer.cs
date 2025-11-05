namespace Prana.Tools
{
    partial class SecMasterDbDataCtrl
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
                if (dtDataBaseMessages != null)
                {
                    dtDataBaseMessages.Dispose();
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
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbbxCountry = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.grdDataBase = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnGetDataFromDB = new System.Windows.Forms.Button();
            this.ultraExpandableGroupBox2 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.btnRemoveDuplicate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxCountry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDataBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).BeginInit();
            this.ultraExpandableGroupBox2.SuspendLayout();
            this.ultraExpandableGroupBoxPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(54, 241);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 20);
            this.label8.TabIndex = 134;
            this.label8.Text = "Country";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbbxCountry
            // 
            this.cmbbxCountry.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance24.BorderColor = System.Drawing.Color.Black;
            this.cmbbxCountry.Appearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance25.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbbxCountry.ButtonAppearance = appearance25;
            this.cmbbxCountry.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            appearance26.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxCountry.DisplayLayout.Appearance = appearance26;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbbxCountry.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbbxCountry.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxCountry.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance27.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance27.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxCountry.DisplayLayout.GroupByBox.Appearance = appearance27;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxCountry.DisplayLayout.GroupByBox.BandLabelAppearance = appearance28;
            this.cmbbxCountry.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance29.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance29.BackColor2 = System.Drawing.SystemColors.Control;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance29.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxCountry.DisplayLayout.GroupByBox.PromptAppearance = appearance29;
            this.cmbbxCountry.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxCountry.DisplayLayout.MaxRowScrollRegions = 1;
            appearance30.BackColor = System.Drawing.SystemColors.Window;
            appearance30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxCountry.DisplayLayout.Override.ActiveCellAppearance = appearance30;
            appearance31.BackColor = System.Drawing.SystemColors.Highlight;
            appearance31.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxCountry.DisplayLayout.Override.ActiveRowAppearance = appearance31;
            this.cmbbxCountry.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxCountry.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxCountry.DisplayLayout.Override.CardAreaAppearance = appearance32;
            appearance33.BorderColor = System.Drawing.Color.Silver;
            appearance33.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxCountry.DisplayLayout.Override.CellAppearance = appearance33;
            this.cmbbxCountry.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxCountry.DisplayLayout.Override.CellPadding = 0;
            appearance34.BackColor = System.Drawing.SystemColors.Control;
            appearance34.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance34.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance34.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxCountry.DisplayLayout.Override.GroupByRowAppearance = appearance34;
            appearance35.TextHAlignAsString = "Left";
            this.cmbbxCountry.DisplayLayout.Override.HeaderAppearance = appearance35;
            this.cmbbxCountry.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxCountry.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance36.BackColor = System.Drawing.SystemColors.Window;
            appearance36.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxCountry.DisplayLayout.Override.RowAppearance = appearance36;
            this.cmbbxCountry.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance37.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxCountry.DisplayLayout.Override.TemplateAddRowAppearance = appearance37;
            this.cmbbxCountry.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxCountry.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxCountry.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxCountry.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbbxCountry.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxCountry.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbbxCountry.Location = new System.Drawing.Point(131, 242);
            this.cmbbxCountry.Name = "cmbbxCountry";
            this.cmbbxCountry.Size = new System.Drawing.Size(57, 19);
            this.cmbbxCountry.TabIndex = 135;
            this.cmbbxCountry.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // grdDataBase
            // 
            this.grdDataBase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance97.BackColor = System.Drawing.Color.Black;
            appearance97.BackColor2 = System.Drawing.Color.Black;
            appearance97.BorderColor = System.Drawing.Color.Black;
            appearance97.FontData.BoldAsString = "True";
            appearance97.FontData.Name = "Tahoma";
            appearance97.FontData.SizeInPoints = 8.25F;
            appearance97.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.grdDataBase.DisplayLayout.Appearance = appearance97;
            this.grdDataBase.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdDataBase.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance8.BackColor = System.Drawing.Color.White;
            this.grdDataBase.DisplayLayout.CaptionAppearance = appearance8;
            this.grdDataBase.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdDataBase.DisplayLayout.GroupByBox.Hidden = true;
            this.grdDataBase.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDataBase.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdDataBase.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinGroup;
            this.grdDataBase.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdDataBase.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDataBase.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDataBase.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdDataBase.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdDataBase.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance9.FontData.BoldAsString = "True";
            appearance9.FontData.Name = "Tahoma";
            appearance9.FontData.SizeInPoints = 8.25F;
            this.grdDataBase.DisplayLayout.Override.HeaderAppearance = appearance9;
            this.grdDataBase.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdDataBase.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdDataBase.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdDataBase.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdDataBase.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdDataBase.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdDataBase.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdDataBase.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdDataBase.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdDataBase.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grdDataBase.Location = new System.Drawing.Point(3, 3);
            this.grdDataBase.Name = "grdDataBase";
            this.grdDataBase.Size = new System.Drawing.Size(447, 233);
            this.grdDataBase.TabIndex = 133;
            this.grdDataBase.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdDataBase.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdDataBase_BeforeColumnChooserDisplayed);
            this.grdDataBase.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdDataBase_BeforeCustomRowFilterDialog);
            // 
            // btnGetDataFromDB
            // 
            this.btnGetDataFromDB.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGetDataFromDB.BackColor = System.Drawing.Color.SeaGreen;
            this.btnGetDataFromDB.Location = new System.Drawing.Point(205, 241);
            this.btnGetDataFromDB.Name = "btnGetDataFromDB";
            this.btnGetDataFromDB.Size = new System.Drawing.Size(86, 20);
            this.btnGetDataFromDB.TabIndex = 132;
            this.btnGetDataFromDB.Text = "GetFrom DB";
            this.btnGetDataFromDB.UseVisualStyleBackColor = false;
            this.btnGetDataFromDB.Click += new System.EventHandler(this.btnGetDataFromDB_Click);
            // 
            // ultraExpandableGroupBox2
            // 
            this.ultraExpandableGroupBox2.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.ultraExpandableGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox2.ExpandedSize = new System.Drawing.Size(485, 270);
            this.ultraExpandableGroupBox2.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            this.ultraExpandableGroupBox2.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.RightOnBorder;
            this.ultraExpandableGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox2.Name = "ultraExpandableGroupBox2";
            this.ultraExpandableGroupBox2.Size = new System.Drawing.Size(485, 270);
            this.ultraExpandableGroupBox2.TabIndex = 136;
            this.ultraExpandableGroupBox2.Text = "Application Data";
            this.ultraExpandableGroupBox2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.btnRemoveDuplicate);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.grdDataBase);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label8);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.btnGetDataFromDB);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.cmbbxCountry);
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(3, 3);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(461, 264);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // btnRemoveDuplicate
            // 
            this.btnRemoveDuplicate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRemoveDuplicate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnRemoveDuplicate.Location = new System.Drawing.Point(297, 241);
            this.btnRemoveDuplicate.Name = "btnRemoveDuplicate";
            this.btnRemoveDuplicate.Size = new System.Drawing.Size(110, 20);
            this.btnRemoveDuplicate.TabIndex = 136;
            this.btnRemoveDuplicate.Text = "Remove Duplicate";
            this.btnRemoveDuplicate.UseVisualStyleBackColor = false;
            this.btnRemoveDuplicate.Click += new System.EventHandler(this.btnRemoveDuplicate_Click);
            // 
            // SecmasterDbDataCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraExpandableGroupBox2);
            this.Name = "SecmasterDbDataCtrl";
            this.Size = new System.Drawing.Size(485, 270);
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxCountry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDataBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).EndInit();
            this.ultraExpandableGroupBox2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxCountry;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDataBase;
        private System.Windows.Forms.Button btnGetDataFromDB;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox2;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private System.Windows.Forms.Button btnRemoveDuplicate;
    }
}
