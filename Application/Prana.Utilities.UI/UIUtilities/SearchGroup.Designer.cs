namespace Prana.Utilities.UI.UIUtilities
{
    partial class SearchGroup
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
            this.AdvSearchFilertUI_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.btnDeleteGroup = new Infragistics.Win.Misc.UltraButton();
            this.cmbBoxAndOr = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.panelConditions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddCondition = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.SuspendLayout();
            this.AdvSearchFilertUI_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBoxAndOr)).BeginInit();
            this.SuspendLayout();
            // 
            // AdvSearchFilertUI_Fill_Panel
            // 
            this.AdvSearchFilertUI_Fill_Panel.BorderStyle = Infragistics.Win.UIElementBorderStyle.Rounded1;
            // 
            // AdvSearchFilertUI_Fill_Panel.ClientArea
            // 
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.Controls.Add(this.btnDeleteGroup);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.Controls.Add(this.cmbBoxAndOr);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.Controls.Add(this.panelConditions);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.Controls.Add(this.btnAddCondition);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.Controls.Add(this.ultraLabel1);
            this.AdvSearchFilertUI_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AdvSearchFilertUI_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdvSearchFilertUI_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.AdvSearchFilertUI_Fill_Panel.Margin = new System.Windows.Forms.Padding(4, 12, 4, 12);
            this.AdvSearchFilertUI_Fill_Panel.Name = "AdvSearchFilertUI_Fill_Panel";
            appearance13.FontData.SizeInPoints = 10F;
            scrollBarLook1.Appearance = appearance13;
            this.AdvSearchFilertUI_Fill_Panel.ScrollBarLook = scrollBarLook1;
            this.AdvSearchFilertUI_Fill_Panel.Size = new System.Drawing.Size(776, 155);
            this.AdvSearchFilertUI_Fill_Panel.TabIndex = 1;
            // 
            // btnDeleteGroup
            // 
            this.btnDeleteGroup.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDeleteGroup.AutoSize = true;
            this.btnDeleteGroup.Location = new System.Drawing.Point(657, 1);
            this.btnDeleteGroup.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteGroup.Name = "btnDeleteGroup";
            this.btnDeleteGroup.Size = new System.Drawing.Size(113, 28);
            this.btnDeleteGroup.TabIndex = 8;
            this.btnDeleteGroup.Text = "Remove group";
            this.btnDeleteGroup.Click += new System.EventHandler(this.btnDeleteGroup_Click);
            // 
            // cmbBoxAndOr
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBoxAndOr.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbBoxAndOr.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbBoxAndOr.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBoxAndOr.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBoxAndOr.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBoxAndOr.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbBoxAndOr.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBoxAndOr.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbBoxAndOr.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBoxAndOr.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBoxAndOr.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBoxAndOr.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbBoxAndOr.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBoxAndOr.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBoxAndOr.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBoxAndOr.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbBoxAndOr.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBoxAndOr.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBoxAndOr.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbBoxAndOr.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbBoxAndOr.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBoxAndOr.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbBoxAndOr.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbBoxAndOr.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBoxAndOr.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbBoxAndOr.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBoxAndOr.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBoxAndOr.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBoxAndOr.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBoxAndOr.Location = new System.Drawing.Point(404, 5);
            this.cmbBoxAndOr.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxAndOr.Name = "cmbBoxAndOr";
            this.cmbBoxAndOr.Size = new System.Drawing.Size(249, 24);
            this.cmbBoxAndOr.TabIndex = 7;
            this.cmbBoxAndOr.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // panelConditions
            // 
            this.panelConditions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelConditions.AutoScroll = true;
            this.panelConditions.AutoSize = true;
            this.panelConditions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelConditions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelConditions.Location = new System.Drawing.Point(8, 37);
            this.panelConditions.Margin = new System.Windows.Forms.Padding(4);
            this.panelConditions.MaximumSize = new System.Drawing.Size(0, 120);
            this.panelConditions.MinimumSize = new System.Drawing.Size(750, 0);
            this.panelConditions.Name = "panelConditions";
            this.panelConditions.Size = new System.Drawing.Size(750, 0);
            this.panelConditions.TabIndex = 2;
            this.panelConditions.WrapContents = false;
            // 
            // btnAddCondition
            // 
            this.btnAddCondition.AutoSize = true;
            this.btnAddCondition.Location = new System.Drawing.Point(221, 0);
            this.btnAddCondition.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddCondition.Name = "btnAddCondition";
            this.btnAddCondition.Size = new System.Drawing.Size(175, 28);
            this.btnAddCondition.TabIndex = 1;
            this.btnAddCondition.Text = "Add items that match...";
            this.btnAddCondition.Click += new System.EventHandler(this.btnAddCondition_Click);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(-2, 2);
            this.ultraLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(286, 28);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Please add search parameters:";
            // 
            // SearchGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AdvSearchFilertUI_Fill_Panel);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(776, 148);
            this.Name = "SearchGroup";
            this.Size = new System.Drawing.Size(776, 155);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AdvSearchFilertUI_Fill_Panel.ClientArea.PerformLayout();
            this.AdvSearchFilertUI_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbBoxAndOr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel AdvSearchFilertUI_Fill_Panel;
        private System.Windows.Forms.FlowLayoutPanel panelConditions;
        private Infragistics.Win.Misc.UltraButton btnAddCondition;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbBoxAndOr;
        private Infragistics.Win.Misc.UltraButton btnDeleteGroup;

    }
}
