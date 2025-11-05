//namespace Prana.PM.Client.UI.Controls
//{
//    partial class CtrlSourceName
//    {
//        /// <summary> 
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("DataSourceNameID", -1);
//            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FullName");
//            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName");
//            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
//            this.lblSourceName = new Infragistics.Win.Misc.UltraLabel();
//            this.cmbSourceName = new Infragistics.Win.UltraWinGrid.UltraCombo();
//            this.bindingSourceDataSourceNameIDList = new System.Windows.Forms.BindingSource(this.components);
//            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceName)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDataSourceNameIDList)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // lblSourceName
//            // 
//            this.lblSourceName.Location = new System.Drawing.Point(0, 5);
//            this.lblSourceName.Name = "lblSourceName";
//            this.lblSourceName.Size = new System.Drawing.Size(88, 20);
//            this.lblSourceName.TabIndex = 2;
//            this.lblSourceName.Text = "Name of Source";
//            // 
//            // cmbSourceName
//            // 
//            this.cmbSourceName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.cmbSourceName.AutoSize = false;
//            this.cmbSourceName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
//            this.cmbSourceName.DataSource = null;
//            this.cmbSourceName.DataSource = this.bindingSourceDataSourceNameIDList;
//            appearance1.BackColor = System.Drawing.SystemColors.Window;
//            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
//            this.cmbSourceName.DisplayLayout.Appearance = appearance1;
//            ultraGridColumn1.Header.VisiblePosition = 0;
//            ultraGridColumn2.Header.VisiblePosition = 1;
//            ultraGridColumn3.Header.VisiblePosition = 2;
//            ultraGridBand1.Columns.AddRange(new object[] {
//            ultraGridColumn1,
//            ultraGridColumn2,
//            ultraGridColumn3});
//            this.cmbSourceName.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
//            this.cmbSourceName.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
//            this.cmbSourceName.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
//            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
//            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
//            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            appearance2.BorderColor = System.Drawing.SystemColors.Window;
//            this.cmbSourceName.DisplayLayout.GroupByBox.Appearance = appearance2;
//            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
//            this.cmbSourceName.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
//            this.cmbSourceName.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
//            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
//            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
//            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
//            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
//            this.cmbSourceName.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
//            this.cmbSourceName.DisplayLayout.MaxColScrollRegions = 1;
//            this.cmbSourceName.DisplayLayout.MaxRowScrollRegions = 1;
//            appearance5.BackColor = System.Drawing.SystemColors.Window;
//            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
//            this.cmbSourceName.DisplayLayout.Override.ActiveCellAppearance = appearance5;
//            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
//            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
//            this.cmbSourceName.DisplayLayout.Override.ActiveRowAppearance = appearance6;
//            this.cmbSourceName.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
//            this.cmbSourceName.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
//            appearance7.BackColor = System.Drawing.SystemColors.Window;
//            this.cmbSourceName.DisplayLayout.Override.CardAreaAppearance = appearance7;
//            appearance8.BorderColor = System.Drawing.Color.Silver;
//            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
//            this.cmbSourceName.DisplayLayout.Override.CellAppearance = appearance8;
//            this.cmbSourceName.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
//            this.cmbSourceName.DisplayLayout.Override.CellPadding = 0;
//            appearance9.BackColor = System.Drawing.SystemColors.Control;
//            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
//            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
//            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
//            appearance9.BorderColor = System.Drawing.SystemColors.Window;
//            this.cmbSourceName.DisplayLayout.Override.GroupByRowAppearance = appearance9;
//            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
//            this.cmbSourceName.DisplayLayout.Override.HeaderAppearance = appearance10;
//            this.cmbSourceName.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
//            this.cmbSourceName.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
//            appearance11.BackColor = System.Drawing.SystemColors.Window;
//            appearance11.BorderColor = System.Drawing.Color.Silver;
//            this.cmbSourceName.DisplayLayout.Override.RowAppearance = appearance11;
//            this.cmbSourceName.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
//            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
//            this.cmbSourceName.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
//            this.cmbSourceName.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
//            this.cmbSourceName.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
//            this.cmbSourceName.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
//            this.cmbSourceName.DisplayMember = "FullName";
//            this.cmbSourceName.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
//            this.cmbSourceName.DropDownWidth = 0;
//            this.cmbSourceName.Location = new System.Drawing.Point(94, 4);
//            this.cmbSourceName.Margin = new System.Windows.Forms.Padding(4);
//            this.cmbSourceName.Name = "cmbSourceName";
//            this.cmbSourceName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
//            this.cmbSourceName.Size = new System.Drawing.Size(150, 20);
//            this.cmbSourceName.TabIndex = 3;
//            this.cmbSourceName.ValueMember = "ID";
//            this.cmbSourceName.ValueChanged += new System.EventHandler(this.cmbSourceName_ValueChanged);
//            // 
//            // bindingSourceDataSourceNameIDList
//            // 
//            this.bindingSourceDataSourceNameIDList.DataSource = typeof(Prana.BusinessObjects.PositionManagement.DataSourceNameIDList);
//            this.bindingSourceDataSourceNameIDList.CurrentChanged += new System.EventHandler(this.bindingSourceDataSourceNameIDList_CurrentChanged);
//            this.bindingSourceDataSourceNameIDList.BindingComplete += new System.Windows.Forms.BindingCompleteEventHandler(this.bindingSourceDataSourceNameIDList_BindingComplete);
//            // 
//            // CtrlSourceName
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
//            this.Controls.Add(this.cmbSourceName);
//            this.Controls.Add(this.lblSourceName);
//            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.Name = "CtrlSourceName";
//            this.Size = new System.Drawing.Size(245, 27);
//            this.Load += new System.EventHandler(this.CtrlSourceName_Load);
//            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceName)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDataSourceNameIDList)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private Infragistics.Win.Misc.UltraLabel lblSourceName;
//        private System.Windows.Forms.BindingSource bindingSourceDataSourceNameIDList;
//        private Infragistics.Win.UltraWinGrid.UltraCombo cmbSourceName;
//    }
//}
