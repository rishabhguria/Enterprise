namespace Prana.Admin.Controls
{
    partial class CAPrefsCtrl
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
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
            this.chkUseNetNotional = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkAdjustShares = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.grpBoxClosing = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblClosingAlgo = new Infragistics.Win.Misc.UltraLabel();
            this.lblSecondarySort = new Infragistics.Win.Misc.UltraLabel();
            this.cmbClosingAlgo = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbSecondarySort = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseNetNotional)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAdjustShares)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxClosing)).BeginInit();
            this.grpBoxClosing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClosingAlgo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSecondarySort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkUseNetNotional
            // 
            this.chkUseNetNotional.Location = new System.Drawing.Point(20, 22);
            this.chkUseNetNotional.Name = "chkUseNetNotional";
            this.chkUseNetNotional.Size = new System.Drawing.Size(120, 20);
            this.chkUseNetNotional.TabIndex = 0;
            this.chkUseNetNotional.Text = "Use Net Notional";
            // 
            // chkAdjustShares
            // 
            this.chkAdjustShares.Location = new System.Drawing.Point(20, 42);
            this.chkAdjustShares.Name = "chkAdjustShares";
            this.chkAdjustShares.Size = new System.Drawing.Size(300, 23);
            this.chkAdjustShares.TabIndex = 1;
            this.chkAdjustShares.Text = "Adjust Fractional Shares in Cash in Lieu at Account Level";
            this.chkAdjustShares.CheckedChanged += new System.EventHandler(this.chkAdjustShares_CheckedChanged);
            // 
            // grpBoxClosing
            // 
            this.grpBoxClosing.Controls.Add(this.cmbSecondarySort);
            this.grpBoxClosing.Controls.Add(this.cmbClosingAlgo);
            this.grpBoxClosing.Controls.Add(this.lblSecondarySort);
            this.grpBoxClosing.Controls.Add(this.lblClosingAlgo);
            this.grpBoxClosing.Enabled = false;
            this.grpBoxClosing.Location = new System.Drawing.Point(20, 71);
            this.grpBoxClosing.Name = "grpBoxClosing";
            this.grpBoxClosing.Size = new System.Drawing.Size(205, 145);
            this.grpBoxClosing.TabIndex = 2;
            this.grpBoxClosing.Text = "Closing Criteria for Fractional Shares";
            // 
            // lblClosingAlgo
            // 
            this.lblClosingAlgo.AutoSize = true;
            this.lblClosingAlgo.Location = new System.Drawing.Point(17, 25);
            this.lblClosingAlgo.Name = "lblClosingAlgo";
            this.lblClosingAlgo.Size = new System.Drawing.Size(93, 22);
            this.lblClosingAlgo.TabIndex = 0;
            this.lblClosingAlgo.Text = "Closing Algorithm";
            // 
            // lblSecondarySort
            // 
            this.lblSecondarySort.AutoSize = true;
            this.lblSecondarySort.Location = new System.Drawing.Point(17, 77);
            this.lblSecondarySort.Name = "lblSecondarySort";
            this.lblSecondarySort.Size = new System.Drawing.Size(82, 22);
            this.lblSecondarySort.TabIndex = 2;
            this.lblSecondarySort.Text = "Secondary Sort";
            // 
            // cmbClosingAlgo
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbClosingAlgo.DisplayLayout.Appearance = appearance13;
            this.cmbClosingAlgo.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClosingAlgo.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClosingAlgo.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClosingAlgo.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbClosingAlgo.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClosingAlgo.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbClosingAlgo.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClosingAlgo.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClosingAlgo.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClosingAlgo.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbClosingAlgo.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbClosingAlgo.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbClosingAlgo.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClosingAlgo.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbClosingAlgo.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClosingAlgo.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClosingAlgo.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.cmbClosingAlgo.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbClosingAlgo.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClosingAlgo.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbClosingAlgo.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbClosingAlgo.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbClosingAlgo.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbClosingAlgo.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClosingAlgo.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClosingAlgo.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClosingAlgo.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClosingAlgo.Location = new System.Drawing.Point(17, 49);
            this.cmbClosingAlgo.Name = "cmbClosingAlgo";
            this.cmbClosingAlgo.Size = new System.Drawing.Size(119, 22);
            this.cmbClosingAlgo.TabIndex = 1;
            // 
            // cmbSecondarySort
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSecondarySort.DisplayLayout.Appearance = appearance1;
            this.cmbSecondarySort.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSecondarySort.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSecondarySort.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSecondarySort.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbSecondarySort.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSecondarySort.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbSecondarySort.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSecondarySort.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSecondarySort.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSecondarySort.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbSecondarySort.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSecondarySort.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSecondarySort.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSecondarySort.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbSecondarySort.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSecondarySort.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSecondarySort.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbSecondarySort.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbSecondarySort.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSecondarySort.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbSecondarySort.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbSecondarySort.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSecondarySort.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbSecondarySort.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSecondarySort.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSecondarySort.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSecondarySort.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSecondarySort.Location = new System.Drawing.Point(17, 101);
            this.cmbSecondarySort.Name = "cmbSecondarySort";
            this.cmbSecondarySort.Size = new System.Drawing.Size(119, 22);
            this.cmbSecondarySort.TabIndex = 3;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.chkAdjustShares);
            this.ultraGroupBox1.Controls.Add(this.grpBoxClosing);
            this.ultraGroupBox1.Controls.Add(this.chkUseNetNotional);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(536, 308);
            this.ultraGroupBox1.TabIndex = 0;
            // 
            // CAPrefsCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "CAPrefsCtrl";
            this.Size = new System.Drawing.Size(536, 308);
            this.Load += new System.EventHandler(this.CAPrefsCtrl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkUseNetNotional)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAdjustShares)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxClosing)).EndInit();
            this.grpBoxClosing.ResumeLayout(false);
            this.grpBoxClosing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClosingAlgo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSecondarySort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkUseNetNotional;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkAdjustShares;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxClosing;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbClosingAlgo;
        private Infragistics.Win.Misc.UltraLabel lblSecondarySort;
        private Infragistics.Win.Misc.UltraLabel lblClosingAlgo;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbSecondarySort;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
    }
}
