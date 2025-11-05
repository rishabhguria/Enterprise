namespace Prana.Admin.Controls.CommissionRules
{
    partial class CtrlCommission
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ValueGreaterThan", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ValueLessThanOrEqual", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRate", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeleteButton", 3);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCriteriaId", 4);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit Name", 1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCalculationID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CalculationType", 1);
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
            this.rdbtnParameters = new System.Windows.Forms.RadioButton();
            this.rdbtnCriteria = new System.Windows.Forms.RadioButton();
            this.grpCriteria = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.grdCommissionRules = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.nudMiniCommCriteria = new System.Windows.Forms.NumericUpDown();
            this.cmbBasedOnCriteria = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblMiniCommCriteria = new System.Windows.Forms.Label();
            this.lblBasedOnCriteria = new System.Windows.Forms.Label();
            this.grpParameters = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.RoundOff = new System.Windows.Forms.NumericUpDown();
            this.RoundOffCheckBox = new System.Windows.Forms.CheckBox();
            this.lblMaxCommParameter = new System.Windows.Forms.Label();
            this.nudMaxCommParameter = new System.Windows.Forms.NumericUpDown();
            this.lblCommissionRateParameter = new System.Windows.Forms.Label();
            this.nudMiniCommParameter = new System.Windows.Forms.NumericUpDown();
            this.lbldisplayParameter = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudCommissionRateParameter = new System.Windows.Forms.NumericUpDown();
            this.cmbBasedOnParameter = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblBasedOnParameter = new System.Windows.Forms.Label();
            this.grpCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCommissionRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMiniCommCriteria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBasedOnCriteria)).BeginInit();
            this.grpParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RoundOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxCommParameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMiniCommParameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRateParameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBasedOnParameter)).BeginInit();
            this.SuspendLayout();
            // 
            // rdbtnParameters
            // 
            this.rdbtnParameters.AutoSize = true;
            this.rdbtnParameters.Location = new System.Drawing.Point(5, 3);
            this.rdbtnParameters.Name = "rdbtnParameters";
            this.rdbtnParameters.Size = new System.Drawing.Size(73, 17);
            this.rdbtnParameters.TabIndex = 0;
            this.rdbtnParameters.TabStop = true;
            this.rdbtnParameters.Text = "Parameter";
            this.rdbtnParameters.UseVisualStyleBackColor = true;
            this.rdbtnParameters.CheckedChanged += new System.EventHandler(this.rdbtnParameters_CheckedChanged);
            // 
            // rdbtnCriteria
            // 
            this.rdbtnCriteria.AutoSize = true;
            this.rdbtnCriteria.Location = new System.Drawing.Point(5, 98);
            this.rdbtnCriteria.Name = "rdbtnCriteria";
            this.rdbtnCriteria.Size = new System.Drawing.Size(57, 17);
            this.rdbtnCriteria.TabIndex = 0;
            this.rdbtnCriteria.TabStop = true;
            this.rdbtnCriteria.Text = "Criteria";
            this.rdbtnCriteria.UseVisualStyleBackColor = true;
            this.rdbtnCriteria.CheckedChanged += new System.EventHandler(this.rdbtnCriteria_CheckedChanged);
            // 
            // grpCriteria
            // 
            this.grpCriteria.Controls.Add(this.label6);
            this.grpCriteria.Controls.Add(this.grdCommissionRules);
            this.grpCriteria.Controls.Add(this.nudMiniCommCriteria);
            this.grpCriteria.Controls.Add(this.cmbBasedOnCriteria);
            this.grpCriteria.Controls.Add(this.lblMiniCommCriteria);
            this.grpCriteria.Controls.Add(this.lblBasedOnCriteria);
            this.grpCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCriteria.Location = new System.Drawing.Point(-1, 95);
            this.grpCriteria.Name = "grpCriteria";
            this.grpCriteria.Size = new System.Drawing.Size(515, 218);
            this.grpCriteria.TabIndex = 6;
            this.grpCriteria.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(81, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "*";
            // 
            // grdCommissionRules
            // 
            ultraGridColumn3.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn3.ColSpan = ((short)(0));
            ultraGridColumn3.DataType = typeof(double);
            ultraGridColumn3.DefaultCellValue = 0D;
            ultraGridColumn3.Format = "";
            ultraGridColumn3.Header.Caption = "Value From (>)";
            ultraGridColumn3.Header.ToolTipText = "Initial Value will always be greater that 0";
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn3.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RevertValue;
            ultraGridColumn3.MaxLength = 14;
            ultraGridColumn3.MinValue = ((short)(0));
            ultraGridColumn3.NullText = "0";
            ultraGridColumn3.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn3.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
            ultraGridColumn3.Width = 111;
            ultraGridColumn4.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn4.DataType = typeof(double);
            ultraGridColumn4.DefaultCellValue = 0D;
            ultraGridColumn4.Format = "";
            ultraGridColumn4.Header.Caption = "Value To (<=)";
            ultraGridColumn4.Header.ToolTipText = "Enter Range To (numeric)";
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridColumn4.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RevertValue;
            ultraGridColumn4.MaxLength = 14;
            ultraGridColumn4.MinValue = ((short)(0));
            ultraGridColumn4.NullText = "Infinity";
            ultraGridColumn4.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn4.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
            ultraGridColumn4.Width = 111;
            ultraGridColumn5.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn5.DataType = typeof(double);
            ultraGridColumn5.Format = "##,###.0000";
            ultraGridColumn5.Header.Caption = "Commission Rate";
            ultraGridColumn5.Header.ToolTipText = "Enter Commission Rate";
            ultraGridColumn5.Header.VisiblePosition = 2;
            ultraGridColumn5.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RevertValue;
            ultraGridColumn5.MaxLength = 10;
            ultraGridColumn5.MinValue = ((short)(0));
            ultraGridColumn5.NullText = "0";
            ultraGridColumn5.PromptChar = ' ';
            ultraGridColumn5.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn5.Width = 111;
            ultraGridColumn6.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            appearance1.Image = global::Prana.Admin.Controls.Properties.Resources.delete;
            appearance1.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            ultraGridColumn6.CellButtonAppearance = appearance1;
            ultraGridColumn6.Header.Caption = "Delete";
            ultraGridColumn6.Header.ToolTipText = "Delete";
            ultraGridColumn6.Header.VisiblePosition = 3;
            ultraGridColumn6.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn6.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn6.Width = 101;
            ultraGridColumn7.Header.VisiblePosition = 4;
            ultraGridColumn7.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7});
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this.grdCommissionRules.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCommissionRules.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCommissionRules.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCommissionRules.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCommissionRules.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCommissionRules.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCommissionRules.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCommissionRules.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCommissionRules.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCommissionRules.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCommissionRules.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCommissionRules.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdCommissionRules.Location = new System.Drawing.Point(32, 50);
            this.grdCommissionRules.Name = "grdCommissionRules";
            this.grdCommissionRules.Size = new System.Drawing.Size(455, 162);
            this.grdCommissionRules.TabIndex = 3;
            this.grdCommissionRules.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCommissionRules.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCommissionRules_AfterCellUpdate);
            this.grdCommissionRules.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCommissionRules_CellChange);
            this.grdCommissionRules.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCommissionRules_ClickCellButton);
            this.grdCommissionRules.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdCommissionRules_Error);
            // 
            // nudMiniCommCriteria
            // 
            this.nudMiniCommCriteria.BackColor = System.Drawing.Color.White;
            this.nudMiniCommCriteria.DecimalPlaces = 4;
            this.nudMiniCommCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudMiniCommCriteria.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudMiniCommCriteria.Location = new System.Drawing.Point(372, 20);
            this.nudMiniCommCriteria.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudMiniCommCriteria.Name = "nudMiniCommCriteria";
            this.nudMiniCommCriteria.Size = new System.Drawing.Size(95, 21);
            this.nudMiniCommCriteria.TabIndex = 2;
            // 
            // cmbBasedOnCriteria
            // 
            this.cmbBasedOnCriteria.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn8.Header.VisiblePosition = 0;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn9.Header.VisiblePosition = 1;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn8,
            ultraGridColumn9});
            this.cmbBasedOnCriteria.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbBasedOnCriteria.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBasedOnCriteria.DropDownWidth = 0;
            this.cmbBasedOnCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbBasedOnCriteria.Location = new System.Drawing.Point(96, 20);
            this.cmbBasedOnCriteria.Name = "cmbBasedOnCriteria";
            this.cmbBasedOnCriteria.Size = new System.Drawing.Size(104, 21);
            this.cmbBasedOnCriteria.TabIndex = 1;
            this.cmbBasedOnCriteria.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbBasedOnCriteria.ValueChanged += new System.EventHandler(this.cmbBasedOnCriteria_ValueChanged);
            // 
            // lblMiniCommCriteria
            // 
            this.lblMiniCommCriteria.AutoSize = true;
            this.lblMiniCommCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMiniCommCriteria.Location = new System.Drawing.Point(257, 22);
            this.lblMiniCommCriteria.Name = "lblMiniCommCriteria";
            this.lblMiniCommCriteria.Size = new System.Drawing.Size(108, 13);
            this.lblMiniCommCriteria.TabIndex = 10;
            this.lblMiniCommCriteria.Text = "Minimum Commission ";
            // 
            // lblBasedOnCriteria
            // 
            this.lblBasedOnCriteria.AutoSize = true;
            this.lblBasedOnCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblBasedOnCriteria.Location = new System.Drawing.Point(34, 22);
            this.lblBasedOnCriteria.Name = "lblBasedOnCriteria";
            this.lblBasedOnCriteria.Size = new System.Drawing.Size(53, 13);
            this.lblBasedOnCriteria.TabIndex = 3;
            this.lblBasedOnCriteria.Text = "Based On";
            // 
            // grpParameters
            // 
            this.grpParameters.Controls.Add(this.label5);
            this.grpParameters.Controls.Add(this.RoundOff);
            this.grpParameters.Controls.Add(this.RoundOffCheckBox);
            this.grpParameters.Controls.Add(this.lblMaxCommParameter);
            this.grpParameters.Controls.Add(this.nudMaxCommParameter);
            this.grpParameters.Controls.Add(this.lblCommissionRateParameter);
            this.grpParameters.Controls.Add(this.nudMiniCommParameter);
            this.grpParameters.Controls.Add(this.lbldisplayParameter);
            this.grpParameters.Controls.Add(this.label4);
            this.grpParameters.Controls.Add(this.nudCommissionRateParameter);
            this.grpParameters.Controls.Add(this.cmbBasedOnParameter);
            this.grpParameters.Controls.Add(this.lblBasedOnParameter);
            this.grpParameters.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpParameters.Location = new System.Drawing.Point(1, 4);
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new System.Drawing.Size(513, 88);
            this.grpParameters.TabIndex = 3;
            this.grpParameters.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(9, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 46;
            this.label5.Text = "Min Commission";
            // 
            // RoundOff
            // 
            this.RoundOff.BackColor = System.Drawing.Color.White;
            this.RoundOff.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RoundOff.Location = new System.Drawing.Point(442, 58);
            this.RoundOff.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.RoundOff.Name = "RoundOff";
            this.RoundOff.Size = new System.Drawing.Size(62, 21);
            this.RoundOff.TabIndex = 5;
            this.RoundOff.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.RoundOff.ValueChanged += new System.EventHandler(this.RoundOff_ValueChanged);
            // 
            // RoundOffCheckBox
            // 
            this.RoundOffCheckBox.AutoSize = true;
            this.RoundOffCheckBox.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RoundOffCheckBox.Location = new System.Drawing.Point(362, 58);
            this.RoundOffCheckBox.Name = "RoundOffCheckBox";
            this.RoundOffCheckBox.Size = new System.Drawing.Size(76, 17);
            this.RoundOffCheckBox.TabIndex = 4;
            this.RoundOffCheckBox.Text = "Round Off";
            this.RoundOffCheckBox.UseVisualStyleBackColor = true;
            this.RoundOffCheckBox.CheckedChanged += new System.EventHandler(this.RoundOffCheckBox_CheckedChanged);
            // 
            // lblMaxCommParameter
            // 
            this.lblMaxCommParameter.AutoSize = true;
            this.lblMaxCommParameter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMaxCommParameter.Location = new System.Drawing.Point(182, 62);
            this.lblMaxCommParameter.Name = "lblMaxCommParameter";
            this.lblMaxCommParameter.Size = new System.Drawing.Size(85, 13);
            this.lblMaxCommParameter.TabIndex = 43;
            this.lblMaxCommParameter.Text = "Max Commission";
            // 
            // nudMaxCommParameter
            // 
            this.nudMaxCommParameter.BackColor = System.Drawing.Color.White;
            this.nudMaxCommParameter.DecimalPlaces = 4;
            this.nudMaxCommParameter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudMaxCommParameter.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudMaxCommParameter.Location = new System.Drawing.Point(275, 56);
            this.nudMaxCommParameter.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudMaxCommParameter.Name = "nudMaxCommParameter";
            this.nudMaxCommParameter.Size = new System.Drawing.Size(78, 21);
            this.nudMaxCommParameter.TabIndex = 3;
            // 
            // lblCommissionRateParameter
            // 
            this.lblCommissionRateParameter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCommissionRateParameter.Location = new System.Drawing.Point(182, 29);
            this.lblCommissionRateParameter.Name = "lblCommissionRateParameter";
            this.lblCommissionRateParameter.Size = new System.Drawing.Size(91, 18);
            this.lblCommissionRateParameter.TabIndex = 41;
            this.lblCommissionRateParameter.Text = "Commission Rate";
            // 
            // nudMiniCommParameter
            // 
            this.nudMiniCommParameter.BackColor = System.Drawing.Color.White;
            this.nudMiniCommParameter.DecimalPlaces = 4;
            this.nudMiniCommParameter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudMiniCommParameter.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudMiniCommParameter.Location = new System.Drawing.Point(98, 58);
            this.nudMiniCommParameter.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudMiniCommParameter.Name = "nudMiniCommParameter";
            this.nudMiniCommParameter.Size = new System.Drawing.Size(78, 21);
            this.nudMiniCommParameter.TabIndex = 2;
            // 
            // lbldisplayParameter
            // 
            this.lbldisplayParameter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldisplayParameter.Location = new System.Drawing.Point(360, 29);
            this.lbldisplayParameter.Name = "lbldisplayParameter";
            this.lbldisplayParameter.Size = new System.Drawing.Size(140, 16);
            this.lbldisplayParameter.TabIndex = 38;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(57, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "*";
            // 
            // nudCommissionRateParameter
            // 
            this.nudCommissionRateParameter.BackColor = System.Drawing.Color.White;
            this.nudCommissionRateParameter.DecimalPlaces = 4;
            this.nudCommissionRateParameter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudCommissionRateParameter.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudCommissionRateParameter.Location = new System.Drawing.Point(275, 27);
            this.nudCommissionRateParameter.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudCommissionRateParameter.Name = "nudCommissionRateParameter";
            this.nudCommissionRateParameter.Size = new System.Drawing.Size(78, 21);
            this.nudCommissionRateParameter.TabIndex = 1;
            // 
            // cmbBasedOnParameter
            // 
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBasedOnParameter.DisplayLayout.Appearance = appearance2;
            this.cmbBasedOnParameter.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn10.Header.VisiblePosition = 0;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn11.Header.VisiblePosition = 1;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn10,
            ultraGridColumn11});
            this.cmbBasedOnParameter.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbBasedOnParameter.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBasedOnParameter.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBasedOnParameter.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBasedOnParameter.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.cmbBasedOnParameter.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBasedOnParameter.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.cmbBasedOnParameter.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBasedOnParameter.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBasedOnParameter.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBasedOnParameter.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.cmbBasedOnParameter.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBasedOnParameter.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBasedOnParameter.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBasedOnParameter.DisplayLayout.Override.CellAppearance = appearance9;
            this.cmbBasedOnParameter.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBasedOnParameter.DisplayLayout.Override.CellPadding = 0;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBasedOnParameter.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.TextHAlignAsString = "Left";
            this.cmbBasedOnParameter.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.cmbBasedOnParameter.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBasedOnParameter.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.cmbBasedOnParameter.DisplayLayout.Override.RowAppearance = appearance12;
            this.cmbBasedOnParameter.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBasedOnParameter.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.cmbBasedOnParameter.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBasedOnParameter.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBasedOnParameter.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBasedOnParameter.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBasedOnParameter.DropDownWidth = 0;
            this.cmbBasedOnParameter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbBasedOnParameter.Location = new System.Drawing.Point(98, 27);
            this.cmbBasedOnParameter.Name = "cmbBasedOnParameter";
            this.cmbBasedOnParameter.Size = new System.Drawing.Size(78, 21);
            this.cmbBasedOnParameter.TabIndex = 0;
            this.cmbBasedOnParameter.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbBasedOnParameter.ValueChanged += new System.EventHandler(this.cmbBasedOnParameter_ValueChanged);
            // 
            // lblBasedOnParameter
            // 
            this.lblBasedOnParameter.AutoSize = true;
            this.lblBasedOnParameter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblBasedOnParameter.Location = new System.Drawing.Point(9, 29);
            this.lblBasedOnParameter.Name = "lblBasedOnParameter";
            this.lblBasedOnParameter.Size = new System.Drawing.Size(53, 13);
            this.lblBasedOnParameter.TabIndex = 0;
            this.lblBasedOnParameter.Text = "Based On";
            // 
            // CtrlCommission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdbtnParameters);
            this.Controls.Add(this.rdbtnCriteria);
            this.Controls.Add(this.grpCriteria);
            this.Controls.Add(this.grpParameters);
            this.Name = "CtrlCommission";
            this.Size = new System.Drawing.Size(518, 319);
            this.grpCriteria.ResumeLayout(false);
            this.grpCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCommissionRules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMiniCommCriteria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBasedOnCriteria)).EndInit();
            this.grpParameters.ResumeLayout(false);
            this.grpParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RoundOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxCommParameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMiniCommParameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRateParameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBasedOnParameter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbtnParameters;
        private System.Windows.Forms.RadioButton rdbtnCriteria;
        private System.Windows.Forms.GroupBox grpCriteria;
        private System.Windows.Forms.Label label6;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCommissionRules;
        private System.Windows.Forms.NumericUpDown nudMiniCommCriteria;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbBasedOnCriteria;
        private System.Windows.Forms.Label lblMiniCommCriteria;
        private System.Windows.Forms.Label lblBasedOnCriteria;
        private System.Windows.Forms.GroupBox grpParameters;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown RoundOff;
        private System.Windows.Forms.CheckBox RoundOffCheckBox;
        private System.Windows.Forms.Label lblMaxCommParameter;
        private System.Windows.Forms.NumericUpDown nudMaxCommParameter;
        private System.Windows.Forms.Label lblCommissionRateParameter;
        private System.Windows.Forms.NumericUpDown nudMiniCommParameter;
        private System.Windows.Forms.Label lbldisplayParameter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudCommissionRateParameter;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbBasedOnParameter;
        private System.Windows.Forms.Label lblBasedOnParameter;

    }
}
