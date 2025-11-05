namespace Prana.Admin.Controls.WashSalePref
{
    partial class ctrlWashSalePreferences
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
            this.chkBoxMultiple = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.cmbAccounts = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbMultiAccounts = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.gbxWashSaleStartDate = new Infragistics.Win.Misc.UltraGroupBox();
            this.uDtWashSaleStartDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblValidationMsg = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxMultiple)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxWashSaleStartDate)).BeginInit();
            this.gbxWashSaleStartDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uDtWashSaleStartDate)).BeginInit();
            this.SuspendLayout();
            // 
            // chkBoxMultiple
            // 
            this.chkBoxMultiple.AllowDrop = true;
            this.chkBoxMultiple.AutoSize = true;
            this.chkBoxMultiple.Checked = true;
            this.chkBoxMultiple.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxMultiple.Location = new System.Drawing.Point(44, 36);
            this.chkBoxMultiple.Name = "chkBoxMultiple";
            this.chkBoxMultiple.Size = new System.Drawing.Size(130, 17);
            this.chkBoxMultiple.TabIndex = 17;
            this.chkBoxMultiple.Text = "Set Multiple Accounts";
            this.chkBoxMultiple.CheckedChanged += new System.EventHandler(this.chkBoxMultiple_CheckedChanged);
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.cmbAccounts);
            this.ultraGroupBox3.Controls.Add(this.cmbMultiAccounts);
            this.ultraGroupBox3.Location = new System.Drawing.Point(196, 20);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(190, 46);
            this.ultraGroupBox3.TabIndex = 18;
            this.ultraGroupBox3.Text = "Account";
            // 
            // cmbAccounts
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAccounts.DisplayLayout.Appearance = appearance1;
            this.cmbAccounts.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAccounts.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAccounts.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAccounts.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbAccounts.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAccounts.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbAccounts.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAccounts.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAccounts.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAccounts.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbAccounts.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAccounts.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAccounts.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAccounts.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbAccounts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAccounts.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAccounts.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbAccounts.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbAccounts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAccounts.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbAccounts.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbAccounts.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAccounts.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAccounts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAccounts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAccounts.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAccounts.Location = new System.Drawing.Point(3, 16);
            this.cmbAccounts.MaximumSize = new System.Drawing.Size(165, 22);
            this.cmbAccounts.MinimumSize = new System.Drawing.Size(165, 22);
            this.cmbAccounts.Name = "cmbAccounts";
            this.cmbAccounts.Size = new System.Drawing.Size(165, 22);
            this.cmbAccounts.TabIndex = 0;
            this.cmbAccounts.ValueChanged += new System.EventHandler(this.cmbAccounts_ValueChanged);
            // 
            // cmbMultiAccounts
            // 
            this.cmbMultiAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbMultiAccounts.Location = new System.Drawing.Point(3, 16);
            this.cmbMultiAccounts.Name = "cmbMultiAccounts";
            this.cmbMultiAccounts.Size = new System.Drawing.Size(184, 27);
            this.cmbMultiAccounts.TabIndex = 1;
            this.cmbMultiAccounts.TitleText = "";
            // 
            // gbxWashSaleStartDate
            // 
            this.gbxWashSaleStartDate.Controls.Add(this.uDtWashSaleStartDate);
            this.gbxWashSaleStartDate.Location = new System.Drawing.Point(392, 20);
            this.gbxWashSaleStartDate.Name = "gbxWashSaleStartDate";
            this.gbxWashSaleStartDate.Size = new System.Drawing.Size(172, 46);
            this.gbxWashSaleStartDate.TabIndex = 19;
            this.gbxWashSaleStartDate.Text = "Start Date";
            // 
            // uDtWashSaleStartDate
            // 
            this.uDtWashSaleStartDate.Location = new System.Drawing.Point(6, 16);
            this.uDtWashSaleStartDate.Name = "uDtWashSaleStartDate";
            this.uDtWashSaleStartDate.Size = new System.Drawing.Size(100, 21);
            this.uDtWashSaleStartDate.TabIndex = 0;
            // 
            // lblValidationMsg
            // 
            this.lblValidationMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValidationMsg.Location = new System.Drawing.Point(44, 80);
            this.lblValidationMsg.Name = "lblValidationMsg";
            this.lblValidationMsg.Size = new System.Drawing.Size(427, 24);
            this.lblValidationMsg.TabIndex = 20;
            this.lblValidationMsg.Text = "*Wash Sale start date cannot be before Cash Management start date*";
            // 
            // ctrlWashSalePreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblValidationMsg);
            this.Controls.Add(this.gbxWashSaleStartDate);
            this.Controls.Add(this.ultraGroupBox3);
            this.Controls.Add(this.chkBoxMultiple);
            this.Name = "ctrlWashSalePreferences";
            this.Size = new System.Drawing.Size(612, 538);
            this.Load += new System.EventHandler(this.ctrlWashSalePreferences_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxMultiple)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.ultraGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxWashSaleStartDate)).EndInit();
            this.gbxWashSaleStartDate.ResumeLayout(false);
            this.gbxWashSaleStartDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uDtWashSaleStartDate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxMultiple;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAccounts;
        private Utilities.UI.UIUtilities.MultiSelectDropDown cmbMultiAccounts;
        private Infragistics.Win.Misc.UltraGroupBox gbxWashSaleStartDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor uDtWashSaleStartDate;
        private Infragistics.Win.Misc.UltraLabel lblValidationMsg;
    }
}
