namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    partial class CtlTradeAttributes
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
            ckbTradeAttribute1.CheckedChanged -= ckbTradeAttribute_CheckedChanged;
            ckbTradeAttribute2.CheckedChanged -= ckbTradeAttribute_CheckedChanged;
            ckbTradeAttribute3.CheckedChanged -= ckbTradeAttribute_CheckedChanged;
            ckbTradeAttribute4.CheckedChanged -= ckbTradeAttribute_CheckedChanged;
            ckbTradeAttribute5.CheckedChanged -= ckbTradeAttribute_CheckedChanged;
            ckbTradeAttribute6.CheckedChanged -= ckbTradeAttribute_CheckedChanged;

            ckbTradeAttribute1.Click -= ckbTradeAttribute_Click;
            ckbTradeAttribute2.Click -= ckbTradeAttribute_Click;
            ckbTradeAttribute3.Click -= ckbTradeAttribute_Click;
            ckbTradeAttribute4.Click -= ckbTradeAttribute_Click;
            ckbTradeAttribute5.Click -= ckbTradeAttribute_Click;
            ckbTradeAttribute6.Click -= ckbTradeAttribute_Click;

            if (cmbMasterFund != null)
            {
                this.cmbMasterFund.ValueChanged -= new System.EventHandler(this.cmbMasterFund_ValueChanged);
                cmbMasterFund.Dispose();
                cmbMasterFund = null;
            }
            if (cmbFXPrimeBroker != null)
            {
                this.cmbFXPrimeBroker.ValueChanged -= new System.EventHandler(this.cmbFXPrimeBroker_ValueChanged);
                cmbFXPrimeBroker.Dispose();
                cmbFXPrimeBroker = null;
            }
            if (cboTradeAttribute1 != null)
            {
                cboTradeAttribute1.Dispose();
                cboTradeAttribute1 = null;
            }
            if (cboTradeAttribute2 != null)
            {
                cboTradeAttribute2.Dispose();
                cboTradeAttribute2 = null;
            }
            if (cboTradeAttribute3 != null)
            {
                cboTradeAttribute3.Dispose();
                cboTradeAttribute3 = null;
            }
            if (cboTradeAttribute4 != null)
            {
                cboTradeAttribute4.Dispose();
                cboTradeAttribute4 = null;
            }
            if (cboTradeAttribute5 != null)
            {
                cboTradeAttribute5.Dispose();
                cboTradeAttribute5 = null;
            }
            if (cboTradeAttribute6 != null)
            {
                cboTradeAttribute6.Dispose();
                cboTradeAttribute6 = null;
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCalculationID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CalculationType", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCalculationID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CalculationType", 1);
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
            this.cboTradeAttribute1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboTradeAttribute6 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboTradeAttribute5 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboTradeAttribute4 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboTradeAttribute3 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboTradeAttribute2 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ckbTradeAttribute1 = new System.Windows.Forms.CheckBox();
            this.ckbTradeAttribute6 = new System.Windows.Forms.CheckBox();
            this.ckbTradeAttribute5 = new System.Windows.Forms.CheckBox();
            this.ckbTradeAttribute4 = new System.Windows.Forms.CheckBox();
            this.ckbTradeAttribute3 = new System.Windows.Forms.CheckBox();
            this.ckbTradeAttribute2 = new System.Windows.Forms.CheckBox();
            this.ubApply = new Infragistics.Win.Misc.UltraButton();
            this.gbGroupLevel = new System.Windows.Forms.GroupBox();
            this.chkSelectAllBulkChanges = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbtnTaxlot = new System.Windows.Forms.RadioButton();
            this.rdbtnGroup = new System.Windows.Forms.RadioButton();
            this.grpTaxlot = new System.Windows.Forms.GroupBox();
            this.chkSelectAllAccounts = new System.Windows.Forms.CheckBox();
            this.cmbMasterFund = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblMasterFund = new Infragistics.Win.Misc.UltraLabel();
            this.chkLstPrimeBrokerAccounts = new System.Windows.Forms.CheckedListBox();
            this.lblPBAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.lblPrimeBroker = new Infragistics.Win.Misc.UltraLabel();
            this.cmbFXPrimeBroker = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.statusProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute2)).BeginInit();
            this.gbGroupLevel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpTaxlot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMasterFund)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFXPrimeBroker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // cboTradeAttribute1
            // 
            this.cboTradeAttribute1.Enabled = false;
            this.cboTradeAttribute1.Location = new System.Drawing.Point(211, 19);
            this.cboTradeAttribute1.Name = "cboTradeAttribute1";
            this.cboTradeAttribute1.Size = new System.Drawing.Size(144, 21);
            this.cboTradeAttribute1.TabIndex = 6;
            // 
            // cboTradeAttribute6
            // 
            this.cboTradeAttribute6.Enabled = false;
            this.cboTradeAttribute6.Location = new System.Drawing.Point(211, 163);
            this.cboTradeAttribute6.Name = "cboTradeAttribute6";
            this.cboTradeAttribute6.Size = new System.Drawing.Size(144, 21);
            this.cboTradeAttribute6.TabIndex = 8;
            // 
            // cboTradeAttribute5
            // 
            this.cboTradeAttribute5.Enabled = false;
            this.cboTradeAttribute5.Location = new System.Drawing.Point(211, 132);
            this.cboTradeAttribute5.Name = "cboTradeAttribute5";
            this.cboTradeAttribute5.Size = new System.Drawing.Size(144, 21);
            this.cboTradeAttribute5.TabIndex = 9;
            // 
            // cboTradeAttribute4
            // 
            this.cboTradeAttribute4.Enabled = false;
            this.cboTradeAttribute4.Location = new System.Drawing.Point(211, 105);
            this.cboTradeAttribute4.Name = "cboTradeAttribute4";
            this.cboTradeAttribute4.Size = new System.Drawing.Size(144, 21);
            this.cboTradeAttribute4.TabIndex = 10;
            // 
            // cboTradeAttribute3
            // 
            this.cboTradeAttribute3.Enabled = false;
            this.cboTradeAttribute3.Location = new System.Drawing.Point(211, 78);
            this.cboTradeAttribute3.Name = "cboTradeAttribute3";
            this.cboTradeAttribute3.Size = new System.Drawing.Size(144, 21);
            this.cboTradeAttribute3.TabIndex = 11;
            // 
            // cboTradeAttribute2
            // 
            this.cboTradeAttribute2.Enabled = false;
            this.cboTradeAttribute2.Location = new System.Drawing.Point(211, 49);
            this.cboTradeAttribute2.Name = "cboTradeAttribute2";
            this.cboTradeAttribute2.Size = new System.Drawing.Size(144, 21);
            this.cboTradeAttribute2.TabIndex = 12;
            // 
            // ckbTradeAttribute1
            // 
            this.ckbTradeAttribute1.AutoSize = true;
            this.ckbTradeAttribute1.Location = new System.Drawing.Point(11, 22);
            this.ckbTradeAttribute1.Name = "ckbTradeAttribute1";
            this.ckbTradeAttribute1.Size = new System.Drawing.Size(105, 17);
            this.ckbTradeAttribute1.TabIndex = 13;
            this.ckbTradeAttribute1.Text = "Trade Attribute 1";
            this.ckbTradeAttribute1.UseVisualStyleBackColor = true;
            // 
            // ckbTradeAttribute6
            // 
            this.ckbTradeAttribute6.AutoSize = true;
            this.ckbTradeAttribute6.Location = new System.Drawing.Point(11, 163);
            this.ckbTradeAttribute6.Name = "ckbTradeAttribute6";
            this.ckbTradeAttribute6.Size = new System.Drawing.Size(105, 17);
            this.ckbTradeAttribute6.TabIndex = 14;
            this.ckbTradeAttribute6.Text = "Trade Attribute 6";
            this.ckbTradeAttribute6.UseVisualStyleBackColor = true;
            // 
            // ckbTradeAttribute5
            // 
            this.ckbTradeAttribute5.AutoSize = true;
            this.ckbTradeAttribute5.Location = new System.Drawing.Point(11, 132);
            this.ckbTradeAttribute5.Name = "ckbTradeAttribute5";
            this.ckbTradeAttribute5.Size = new System.Drawing.Size(105, 17);
            this.ckbTradeAttribute5.TabIndex = 15;
            this.ckbTradeAttribute5.Text = "Trade Attribute 5";
            this.ckbTradeAttribute5.UseVisualStyleBackColor = true;
            // 
            // ckbTradeAttribute4
            // 
            this.ckbTradeAttribute4.AutoSize = true;
            this.ckbTradeAttribute4.Location = new System.Drawing.Point(11, 105);
            this.ckbTradeAttribute4.Name = "ckbTradeAttribute4";
            this.ckbTradeAttribute4.Size = new System.Drawing.Size(105, 17);
            this.ckbTradeAttribute4.TabIndex = 16;
            this.ckbTradeAttribute4.Text = "Trade Attribute 4";
            this.ckbTradeAttribute4.UseVisualStyleBackColor = true;
            // 
            // ckbTradeAttribute3
            // 
            this.ckbTradeAttribute3.AutoSize = true;
            this.ckbTradeAttribute3.Location = new System.Drawing.Point(11, 78);
            this.ckbTradeAttribute3.Name = "ckbTradeAttribute3";
            this.ckbTradeAttribute3.Size = new System.Drawing.Size(105, 17);
            this.ckbTradeAttribute3.TabIndex = 17;
            this.ckbTradeAttribute3.Text = "Trade Attribute 3";
            this.ckbTradeAttribute3.UseVisualStyleBackColor = true;
            // 
            // ckbTradeAttribute2
            // 
            this.ckbTradeAttribute2.AutoSize = true;
            this.ckbTradeAttribute2.Location = new System.Drawing.Point(11, 49);
            this.ckbTradeAttribute2.Name = "ckbTradeAttribute2";
            this.ckbTradeAttribute2.Size = new System.Drawing.Size(105, 17);
            this.ckbTradeAttribute2.TabIndex = 18;
            this.ckbTradeAttribute2.Text = "Trade Attribute 2";
            this.ckbTradeAttribute2.UseVisualStyleBackColor = true;
            // 
            // ubApply
            // 
            this.ubApply.Location = new System.Drawing.Point(266, 190);
            this.ubApply.Name = "ubApply";
            this.ubApply.Size = new System.Drawing.Size(75, 28);
            this.ubApply.TabIndex = 19;
            this.ubApply.Text = "Update";
            this.ubApply.Click += new System.EventHandler(this.ubApply_Click);
            // 
            // gbGroupLevel
            // 
            this.gbGroupLevel.Controls.Add(this.chkSelectAllBulkChanges);
            this.gbGroupLevel.Controls.Add(this.cboTradeAttribute1);
            this.gbGroupLevel.Controls.Add(this.cboTradeAttribute6);
            this.gbGroupLevel.Controls.Add(this.cboTradeAttribute5);
            this.gbGroupLevel.Controls.Add(this.ubApply);
            this.gbGroupLevel.Controls.Add(this.cboTradeAttribute4);
            this.gbGroupLevel.Controls.Add(this.ckbTradeAttribute2);
            this.gbGroupLevel.Controls.Add(this.cboTradeAttribute3);
            this.gbGroupLevel.Controls.Add(this.ckbTradeAttribute3);
            this.gbGroupLevel.Controls.Add(this.cboTradeAttribute2);
            this.gbGroupLevel.Controls.Add(this.ckbTradeAttribute4);
            this.gbGroupLevel.Controls.Add(this.ckbTradeAttribute1);
            this.gbGroupLevel.Controls.Add(this.ckbTradeAttribute5);
            this.gbGroupLevel.Controls.Add(this.ckbTradeAttribute6);
            this.gbGroupLevel.Location = new System.Drawing.Point(367, 66);
            this.gbGroupLevel.Name = "gbGroupLevel";
            this.gbGroupLevel.Size = new System.Drawing.Size(393, 243);
            this.gbGroupLevel.TabIndex = 22;
            this.gbGroupLevel.TabStop = false;
            this.gbGroupLevel.Text = "Apply Group Level";
            // 
            // chkSelectAllBulkChanges
            // 
            this.chkSelectAllBulkChanges.AutoSize = true;
            this.chkSelectAllBulkChanges.Location = new System.Drawing.Point(11, 196);
            this.chkSelectAllBulkChanges.Name = "chkSelectAllBulkChanges";
            this.chkSelectAllBulkChanges.Size = new System.Drawing.Size(117, 17);
            this.chkSelectAllBulkChanges.TabIndex = 20;
            this.chkSelectAllBulkChanges.Text = "Select/Unselect All";
            this.chkSelectAllBulkChanges.UseVisualStyleBackColor = true;
            this.chkSelectAllBulkChanges.Click += new System.EventHandler(this.chkSelectAllBulkChanges_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdbtnTaxlot);
            this.groupBox2.Controls.Add(this.rdbtnGroup);
            this.groupBox2.Location = new System.Drawing.Point(30, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(730, 43);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Apply Trade Attributes";
            // 
            // rdbtnTaxlot
            // 
            this.rdbtnTaxlot.AutoSize = true;
            this.rdbtnTaxlot.Location = new System.Drawing.Point(96, 18);
            this.rdbtnTaxlot.Name = "rdbtnTaxlot";
            this.rdbtnTaxlot.Size = new System.Drawing.Size(83, 17);
            this.rdbtnTaxlot.TabIndex = 1;
            this.rdbtnTaxlot.Text = "Taxlot Level";
            this.rdbtnTaxlot.UseVisualStyleBackColor = true;
            // 
            // rdbtnGroup
            // 
            this.rdbtnGroup.AutoSize = true;
            this.rdbtnGroup.Checked = true;
            this.rdbtnGroup.Location = new System.Drawing.Point(7, 18);
            this.rdbtnGroup.Name = "rdbtnGroup";
            this.rdbtnGroup.Size = new System.Drawing.Size(83, 17);
            this.rdbtnGroup.TabIndex = 0;
            this.rdbtnGroup.TabStop = true;
            this.rdbtnGroup.Text = "Group Level";
            this.rdbtnGroup.UseVisualStyleBackColor = true;
            this.rdbtnGroup.CheckedChanged += new System.EventHandler(this.rdbtnGroup_CheckedChanged);
            // 
            // grpTaxlot
            // 
            this.grpTaxlot.Controls.Add(this.chkSelectAllAccounts);
            this.grpTaxlot.Controls.Add(this.cmbMasterFund);
            this.grpTaxlot.Controls.Add(this.lblMasterFund);
            this.grpTaxlot.Controls.Add(this.chkLstPrimeBrokerAccounts);
            this.grpTaxlot.Controls.Add(this.lblPBAccounts);
            this.grpTaxlot.Controls.Add(this.lblPrimeBroker);
            this.grpTaxlot.Controls.Add(this.cmbFXPrimeBroker);
            this.grpTaxlot.Enabled = false;
            this.grpTaxlot.Location = new System.Drawing.Point(30, 66);
            this.grpTaxlot.Name = "grpTaxlot";
            this.grpTaxlot.Size = new System.Drawing.Size(313, 243);
            this.grpTaxlot.TabIndex = 24;
            this.grpTaxlot.TabStop = false;
            this.grpTaxlot.Text = "Filter Taxlot Level";
            // 
            // chkSelectAllAccounts
            // 
            this.chkSelectAllAccounts.AutoSize = true;
            this.chkSelectAllAccounts.Location = new System.Drawing.Point(7, 63);
            this.chkSelectAllAccounts.Name = "chkSelectAllAccounts";
            this.chkSelectAllAccounts.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAllAccounts.TabIndex = 5;
            this.chkSelectAllAccounts.Text = "Select All";
            this.chkSelectAllAccounts.UseVisualStyleBackColor = true;
            this.chkSelectAllAccounts.CheckedChanged += new System.EventHandler(this.chkSelectAllAccounts_CheckedChanged);
            // 
            // cmbMasterFund
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbMasterFund.DisplayLayout.Appearance = appearance1;
            this.cmbMasterFund.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn21.Header.VisiblePosition = 0;
            ultraGridColumn21.Hidden = true;
            ultraGridColumn22.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn21,
            ultraGridColumn22});
            this.cmbMasterFund.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbMasterFund.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbMasterFund.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbMasterFund.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMasterFund.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbMasterFund.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMasterFund.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbMasterFund.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbMasterFund.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbMasterFund.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbMasterFund.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbMasterFund.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbMasterFund.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbMasterFund.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbMasterFund.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbMasterFund.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbMasterFund.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbMasterFund.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbMasterFund.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbMasterFund.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbMasterFund.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbMasterFund.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbMasterFund.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbMasterFund.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbMasterFund.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbMasterFund.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbMasterFund.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbMasterFund.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbMasterFund.DropDownWidth = 0;
            this.cmbMasterFund.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbMasterFund.Location = new System.Drawing.Point(83, 17);
            this.cmbMasterFund.Name = "cmbMasterFund";
            this.cmbMasterFund.Size = new System.Drawing.Size(97, 21);
            this.cmbMasterFund.TabIndex = 1;
            this.cmbMasterFund.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbMasterFund.ValueChanged += new System.EventHandler(this.cmbMasterFund_ValueChanged);
            // 
            // lblMasterFund
            // 
            this.lblMasterFund.AutoSize = true;
            this.lblMasterFund.Location = new System.Drawing.Point(7, 21);
            this.lblMasterFund.Name = "lblMasterFund";
            this.lblMasterFund.Size = new System.Drawing.Size(68, 14);
            this.lblMasterFund.TabIndex = 0;
            this.lblMasterFund.Text = "Master Fund";
            // 
            // chkLstPrimeBrokerAccounts
            // 
            this.chkLstPrimeBrokerAccounts.CheckOnClick = true;
            this.chkLstPrimeBrokerAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkLstPrimeBrokerAccounts.Location = new System.Drawing.Point(83, 45);
            this.chkLstPrimeBrokerAccounts.Name = "chkLstPrimeBrokerAccounts";
            this.chkLstPrimeBrokerAccounts.Size = new System.Drawing.Size(224, 164);
            this.chkLstPrimeBrokerAccounts.Sorted = true;
            this.chkLstPrimeBrokerAccounts.TabIndex = 6;
            this.chkLstPrimeBrokerAccounts.ThreeDCheckBoxes = true;
            this.chkLstPrimeBrokerAccounts.SelectedIndexChanged += new System.EventHandler(this.chkLstPrimeBrokerAccounts_SelectedIndexChanged);
            // 
            // lblPBAccounts
            // 
            this.lblPBAccounts.AutoSize = true;
            this.lblPBAccounts.Location = new System.Drawing.Point(7, 45);
            this.lblPBAccounts.Name = "lblPBAccounts";
            this.lblPBAccounts.Size = new System.Drawing.Size(36, 14);
            this.lblPBAccounts.TabIndex = 4;
            this.lblPBAccounts.Text = "Accounts";
            // 
            // lblPrimeBroker
            // 
            this.lblPrimeBroker.AutoSize = true;
            this.lblPrimeBroker.Location = new System.Drawing.Point(186, 21);
            this.lblPrimeBroker.Name = "lblPrimeBroker";
            this.lblPrimeBroker.Size = new System.Drawing.Size(19, 14);
            this.lblPrimeBroker.TabIndex = 2;
            this.lblPrimeBroker.Text = "PB";
            // 
            // cmbFXPrimeBroker
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbFXPrimeBroker.DisplayLayout.Appearance = appearance13;
            this.cmbFXPrimeBroker.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn23.Header.VisiblePosition = 0;
            ultraGridColumn23.Hidden = true;
            ultraGridColumn24.Header.VisiblePosition = 1;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn23,
            ultraGridColumn24});
            this.cmbFXPrimeBroker.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbFXPrimeBroker.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbFXPrimeBroker.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFXPrimeBroker.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFXPrimeBroker.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbFXPrimeBroker.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFXPrimeBroker.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbFXPrimeBroker.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbFXPrimeBroker.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbFXPrimeBroker.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbFXPrimeBroker.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbFXPrimeBroker.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbFXPrimeBroker.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbFXPrimeBroker.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbFXPrimeBroker.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbFXPrimeBroker.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbFXPrimeBroker.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFXPrimeBroker.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.cmbFXPrimeBroker.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbFXPrimeBroker.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbFXPrimeBroker.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbFXPrimeBroker.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbFXPrimeBroker.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbFXPrimeBroker.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbFXPrimeBroker.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbFXPrimeBroker.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbFXPrimeBroker.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbFXPrimeBroker.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbFXPrimeBroker.DropDownWidth = 0;
            this.cmbFXPrimeBroker.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbFXPrimeBroker.Location = new System.Drawing.Point(215, 18);
            this.cmbFXPrimeBroker.Name = "cmbFXPrimeBroker";
            this.cmbFXPrimeBroker.Size = new System.Drawing.Size(90, 21);
            this.cmbFXPrimeBroker.TabIndex = 3;
            this.cmbFXPrimeBroker.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbFXPrimeBroker.ValueChanged += new System.EventHandler(this.cmbFXPrimeBroker_ValueChanged);
            // 
            // statusProvider
            // 
            this.statusProvider.ContainerControl = this;
            // 
            // CtlTradeAttributes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpTaxlot);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbGroupLevel);
            this.Name = "CtlTradeAttributes";
            this.Size = new System.Drawing.Size(850, 322);
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTradeAttribute2)).EndInit();
            this.gbGroupLevel.ResumeLayout(false);
            this.gbGroupLevel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpTaxlot.ResumeLayout(false);
            this.grpTaxlot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMasterFund)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFXPrimeBroker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTradeAttribute1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTradeAttribute6;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTradeAttribute5;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTradeAttribute4;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTradeAttribute3;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTradeAttribute2;
        private System.Windows.Forms.CheckBox ckbTradeAttribute1;
        private System.Windows.Forms.CheckBox ckbTradeAttribute6;
        private System.Windows.Forms.CheckBox ckbTradeAttribute5;
        private System.Windows.Forms.CheckBox ckbTradeAttribute4;
        private System.Windows.Forms.CheckBox ckbTradeAttribute3;
        private System.Windows.Forms.CheckBox ckbTradeAttribute2;
        private Infragistics.Win.Misc.UltraButton ubApply;
        private System.Windows.Forms.GroupBox gbGroupLevel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbtnTaxlot;
        private System.Windows.Forms.RadioButton rdbtnGroup;
        private System.Windows.Forms.GroupBox grpTaxlot;
        private System.Windows.Forms.CheckBox chkSelectAllAccounts;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbMasterFund;
        private Infragistics.Win.Misc.UltraLabel lblMasterFund;
        private System.Windows.Forms.CheckedListBox chkLstPrimeBrokerAccounts;
        private Infragistics.Win.Misc.UltraLabel lblPBAccounts;
        private Infragistics.Win.Misc.UltraLabel lblPrimeBroker;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbFXPrimeBroker;
        private System.Windows.Forms.CheckBox chkSelectAllBulkChanges;
        private System.Windows.Forms.ErrorProvider statusProvider;
    }
}
