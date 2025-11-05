namespace Prana.Tools.PL.Controls
{
    partial class ctrlReconGroupByColumns
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
                if (_reconTemplate != null)
                {
                    _reconTemplate.Dispose();
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
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.groupBoxgroupbyColumns = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkSelectAll = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkedGroup = new System.Windows.Forms.CheckedListBox();
            this.ultraGroupSymbology = new Infragistics.Win.Misc.UltraGroupBox();
            this.rbCUSIP = new System.Windows.Forms.RadioButton();
            this.rbSedolSymbol = new System.Windows.Forms.RadioButton();
            this.rbBloomberg = new System.Windows.Forms.RadioButton();
            this.rbIDCO = new System.Windows.Forms.RadioButton();
            this.rbOSI = new System.Windows.Forms.RadioButton();
            this.rbTicker = new System.Windows.Forms.RadioButton();
            this.lblAsset = new Infragistics.Win.Misc.UltraLabel();
            this.cmbAssets = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxgroupbyColumns)).BeginInit();
            this.groupBoxgroupbyColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupSymbology)).BeginInit();
            this.ultraGroupSymbology.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAssets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.groupBoxgroupbyColumns);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(304, 168);
            this.ultraPanel1.TabIndex = 26;
            // 
            // groupBoxgroupbyColumns
            // 
            this.groupBoxgroupbyColumns.BackColorInternal = System.Drawing.Color.Transparent;
            this.groupBoxgroupbyColumns.Controls.Add(this.ultraLabel1);
            this.groupBoxgroupbyColumns.Controls.Add(this.groupBox1);
            this.groupBoxgroupbyColumns.Controls.Add(this.ultraGroupSymbology);
            this.groupBoxgroupbyColumns.Controls.Add(this.lblAsset);
            this.groupBoxgroupbyColumns.Controls.Add(this.cmbAssets);
            this.groupBoxgroupbyColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxgroupbyColumns.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxgroupbyColumns.ForeColor = System.Drawing.Color.Black;
            this.groupBoxgroupbyColumns.Location = new System.Drawing.Point(0, 0);
            this.groupBoxgroupbyColumns.Name = "groupBoxgroupbyColumns";
            this.groupBoxgroupbyColumns.Size = new System.Drawing.Size(304, 168);
            this.groupBoxgroupbyColumns.TabIndex = 25;
            this.groupBoxgroupbyColumns.Text = "Group By Columns";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 11);
            this.ultraLabel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColorInternal = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.chkSelectAll);
            this.groupBox1.Controls.Add(this.checkedGroup);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(6, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(104, 139);
            this.groupBox1.TabIndex = 120;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkSelectAll.Location = new System.Drawing.Point(3, 3);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(98, 20);
            this.chkSelectAll.TabIndex = 2;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // checkedGroup
            // 
            this.checkedGroup.CheckOnClick = true;
            this.checkedGroup.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkedGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedGroup.ForeColor = System.Drawing.Color.Black;
            this.checkedGroup.FormattingEnabled = true;
            this.checkedGroup.Location = new System.Drawing.Point(3, 20);
            this.checkedGroup.Name = "checkedGroup";
            this.checkedGroup.Size = new System.Drawing.Size(98, 116);
            this.inboxControlStyler1.SetStyleSettings(this.checkedGroup, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.False));
            this.checkedGroup.TabIndex = 1;
            this.checkedGroup.Click += new System.EventHandler(this.checkedGroup_Click);
            this.checkedGroup.SelectedValueChanged += new System.EventHandler(this.checkedGroup_SelectedValueChanged);
            // 
            // ultraGroupSymbology
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.ultraGroupSymbology.Appearance = appearance1;
            this.ultraGroupSymbology.Controls.Add(this.rbCUSIP);
            this.ultraGroupSymbology.Controls.Add(this.rbSedolSymbol);
            this.ultraGroupSymbology.Controls.Add(this.rbBloomberg);
            this.ultraGroupSymbology.Controls.Add(this.rbIDCO);
            this.ultraGroupSymbology.Controls.Add(this.rbOSI);
            this.ultraGroupSymbology.Controls.Add(this.rbTicker);
            this.ultraGroupSymbology.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupSymbology.ForeColor = System.Drawing.Color.Black;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.ultraGroupSymbology.HeaderAppearance = appearance2;
            this.ultraGroupSymbology.Location = new System.Drawing.Point(113, 57);
            this.ultraGroupSymbology.Name = "ultraGroupSymbology";
            this.ultraGroupSymbology.Size = new System.Drawing.Size(191, 95);
            this.ultraGroupSymbology.TabIndex = 0;
            this.ultraGroupSymbology.Text = "Symbology ";
            // 
            // rbCUSIP
            // 
            this.rbCUSIP.Location = new System.Drawing.Point(98, 30);
            this.rbCUSIP.Name = "rbCUSIP";
            this.rbCUSIP.Size = new System.Drawing.Size(55, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbCUSIP, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbCUSIP.TabIndex = 5;
            this.rbCUSIP.TabStop = true;
            this.rbCUSIP.Text = "CUSIP";
            this.rbCUSIP.UseMnemonic = false;
            this.rbCUSIP.UseVisualStyleBackColor = true;
            this.rbCUSIP.CheckedChanged += new System.EventHandler(this.rbTicker_CheckedChanged);
            // 
            // rbSedolSymbol
            // 
            this.rbSedolSymbol.Location = new System.Drawing.Point(98, 15);
            this.rbSedolSymbol.Name = "rbSedolSymbol";
            this.rbSedolSymbol.Size = new System.Drawing.Size(91, 18);
            this.inboxControlStyler1.SetStyleSettings(this.rbSedolSymbol, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbSedolSymbol.TabIndex = 4;
            this.rbSedolSymbol.TabStop = true;
            this.rbSedolSymbol.Text = "Sedol  Symbol";
            this.rbSedolSymbol.UseMnemonic = false;
            this.rbSedolSymbol.UseVisualStyleBackColor = true;
            this.rbSedolSymbol.CheckedChanged += new System.EventHandler(this.rbTicker_CheckedChanged);
            // 
            // rbBloomberg
            // 
            this.rbBloomberg.Location = new System.Drawing.Point(7, 65);
            this.rbBloomberg.Name = "rbBloomberg";
            this.rbBloomberg.Size = new System.Drawing.Size(112, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbBloomberg, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbBloomberg.TabIndex = 3;
            this.rbBloomberg.TabStop = true;
            this.rbBloomberg.Text = "Bloomberg Symbol";
            this.rbBloomberg.UseVisualStyleBackColor = true;
            this.rbBloomberg.CheckedChanged += new System.EventHandler(this.rbTicker_CheckedChanged);
            // 
            // rbIDCO
            // 
            this.rbIDCO.Location = new System.Drawing.Point(7, 49);
            this.rbIDCO.Name = "rbIDCO";
            this.rbIDCO.Size = new System.Drawing.Size(88, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbIDCO, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbIDCO.TabIndex = 2;
            this.rbIDCO.TabStop = true;
            this.rbIDCO.Text = "IDCO Symbol";
            this.rbIDCO.UseVisualStyleBackColor = true;
            this.rbIDCO.CheckedChanged += new System.EventHandler(this.rbTicker_CheckedChanged);
            // 
            // rbOSI
            // 
            this.rbOSI.Location = new System.Drawing.Point(7, 32);
            this.rbOSI.Name = "rbOSI";
            this.rbOSI.Size = new System.Drawing.Size(80, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbOSI, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbOSI.TabIndex = 1;
            this.rbOSI.TabStop = true;
            this.rbOSI.Text = "OSI Symbol";
            this.rbOSI.UseVisualStyleBackColor = true;
            this.rbOSI.CheckedChanged += new System.EventHandler(this.rbTicker_CheckedChanged);
            // 
            // rbTicker
            // 
            this.rbTicker.Location = new System.Drawing.Point(7, 16);
            this.rbTicker.Name = "rbTicker";
            this.rbTicker.Size = new System.Drawing.Size(90, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbTicker, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbTicker.TabIndex = 0;
            this.rbTicker.TabStop = true;
            this.rbTicker.Text = "Ticker Symbol";
            this.rbTicker.UseVisualStyleBackColor = true;
            this.rbTicker.CheckedChanged += new System.EventHandler(this.rbTicker_CheckedChanged);
            // 
            // lblAsset
            // 
            this.lblAsset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsset.Location = new System.Drawing.Point(114, 20);
            this.lblAsset.Name = "lblAsset";
            this.lblAsset.Size = new System.Drawing.Size(100, 14);
            this.lblAsset.TabIndex = 10;
            this.lblAsset.Text = "Asset";
            // 
            // cmbAssets
            // 
            this.cmbAssets.AutoSize = false;
            appearance3.BackColor = System.Drawing.SystemColors.Window;
            appearance3.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAssets.DisplayLayout.Appearance = appearance3;
            this.cmbAssets.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAssets.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cmbAssets.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
            appearance4.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance4.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAssets.DisplayLayout.GroupByBox.Appearance = appearance4;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAssets.DisplayLayout.GroupByBox.BandLabelAppearance = appearance5;
            this.cmbAssets.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance6.BackColor2 = System.Drawing.SystemColors.Control;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAssets.DisplayLayout.GroupByBox.PromptAppearance = appearance6;
            this.cmbAssets.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAssets.DisplayLayout.MaxRowScrollRegions = 1;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            appearance7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAssets.DisplayLayout.Override.ActiveCellAppearance = appearance7;
            appearance8.BackColor = System.Drawing.SystemColors.Highlight;
            appearance8.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAssets.DisplayLayout.Override.ActiveRowAppearance = appearance8;
            this.cmbAssets.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAssets.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance9.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAssets.DisplayLayout.Override.CardAreaAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.Silver;
            appearance10.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAssets.DisplayLayout.Override.CellAppearance = appearance10;
            this.cmbAssets.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAssets.DisplayLayout.Override.CellPadding = 0;
            appearance11.BackColor = System.Drawing.SystemColors.Control;
            appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance11.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAssets.DisplayLayout.Override.GroupByRowAppearance = appearance11;
            appearance12.TextHAlignAsString = "Left";
            this.cmbAssets.DisplayLayout.Override.HeaderAppearance = appearance12;
            this.cmbAssets.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAssets.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.Color.Silver;
            this.cmbAssets.DisplayLayout.Override.RowAppearance = appearance13;
            this.cmbAssets.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAssets.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
            this.cmbAssets.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAssets.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAssets.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAssets.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAssets.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbAssets.Location = new System.Drawing.Point(115, 35);
            this.cmbAssets.Name = "cmbAssets";
            this.cmbAssets.Size = new System.Drawing.Size(124, 21);
            this.cmbAssets.TabIndex = 117;
            this.cmbAssets.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAssets.ValueChanged += new System.EventHandler(this.cmbAssets_ValueChanged);
            // 
            // ctrlReconGroupByColumns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlReconGroupByColumns";
            this.Size = new System.Drawing.Size(304, 168);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxgroupbyColumns)).EndInit();
            this.groupBoxgroupbyColumns.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupSymbology)).EndInit();
            this.ultraGroupSymbology.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbAssets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraGroupBox groupBoxgroupbyColumns;
        private Infragistics.Win.Misc.UltraGroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox checkedGroup;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupSymbology;
        private System.Windows.Forms.RadioButton rbCUSIP;
        private System.Windows.Forms.RadioButton rbSedolSymbol;
        private System.Windows.Forms.RadioButton rbBloomberg;
        private System.Windows.Forms.RadioButton rbIDCO;
        private System.Windows.Forms.RadioButton rbOSI;
        private System.Windows.Forms.RadioButton rbTicker;
        private Infragistics.Win.Misc.UltraLabel lblAsset;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAssets;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkSelectAll;
    }
}
