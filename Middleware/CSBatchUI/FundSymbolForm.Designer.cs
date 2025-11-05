namespace CSBatchUI
{
    partial class FundSymbolForm
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

        #region Windows Form Designer generated code

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
            this.ultraGridFundSymbol = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraControlContainerEditor1 = new Infragistics.Win.UltraWinEditors.UltraControlContainerEditor(this.components);
            this.labelAffectedDates = new System.Windows.Forms.Label();
            this.listBoxDates = new System.Windows.Forms.ListBox();
            this.checkBoxIgnoreAUECID = new System.Windows.Forms.CheckBox();
            this.numericRollBackDays = new System.Windows.Forms.NumericUpDown();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.labelRollBackDays = new System.Windows.Forms.Label();
            this.numericAdjustFrom = new System.Windows.Forms.NumericUpDown();
            this.labelAdjustFrom = new System.Windows.Forms.Label();
            this.labelAdjustTo = new System.Windows.Forms.Label();
            this.numericAdjustTo = new System.Windows.Forms.NumericUpDown();
            this.PanelFundSymbol = new System.Windows.Forms.Panel();
            this.LabelTo = new System.Windows.Forms.Label();
            this.LabelFundSymbol = new System.Windows.Forms.Label();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.labelFund = new System.Windows.Forms.Label();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.comboBoxFund = new System.Windows.Forms.ComboBox();
            this.LabelFrom = new System.Windows.Forms.Label();
            this.BindingSourceDates = new System.Windows.Forms.BindingSource(this.components);
            this.BindingSourceFundSymbols = new System.Windows.Forms.BindingSource(this.components);
            this.BindingSourceFunds = new System.Windows.Forms.BindingSource(this.components);
            this.numericCores = new System.Windows.Forms.NumericUpDown();
            this.labelCores = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.labelStep = new System.Windows.Forms.Label();
            this.txtBatch = new System.Windows.Forms.TextBox();
            this.numericStep = new System.Windows.Forms.NumericUpDown();
            this.bgwFetchSymbols = new System.ComponentModel.BackgroundWorker();
            this.labelFundsCount = new System.Windows.Forms.Label();
            this.checkBoxAllFunds = new System.Windows.Forms.CheckBox();
            this.checkBoxSaveEnabled = new System.Windows.Forms.CheckBox();
            this.labelSymbolsStatus = new System.Windows.Forms.Label();
            this.checkBoxTouchStep = new System.Windows.Forms.CheckBox();
            this.ctrlFundSymbol = new CSBatchUI.FundSymbolControl();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridFundSymbol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraControlContainerEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRollBackDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericAdjustFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericAdjustTo)).BeginInit();
            this.PanelFundSymbol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceDates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceFundSymbols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceFunds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericStep)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGridFundSymbol
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridFundSymbol.DisplayLayout.Appearance = appearance1;
            this.ultraGridFundSymbol.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.ultraGridFundSymbol.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridFundSymbol.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridFundSymbol.DisplayLayout.MaxRowScrollRegions = 1;
            this.ultraGridFundSymbol.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridFundSymbol.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridFundSymbol.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridFundSymbol.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridFundSymbol.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BorderColor = System.Drawing.Color.Silver;
            appearance3.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridFundSymbol.DisplayLayout.Override.CellAppearance = appearance3;
            this.ultraGridFundSymbol.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this.ultraGridFundSymbol.DisplayLayout.Override.CellPadding = 0;
            this.ultraGridFundSymbol.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance4.BackColor = System.Drawing.SystemColors.Control;
            appearance4.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance4.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridFundSymbol.DisplayLayout.Override.GroupByRowAppearance = appearance4;
            appearance5.TextHAlignAsString = "Left";
            this.ultraGridFundSymbol.DisplayLayout.Override.HeaderAppearance = appearance5;
            this.ultraGridFundSymbol.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridFundSymbol.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridFundSymbol.DisplayLayout.Override.RowAppearance = appearance6;
            this.ultraGridFundSymbol.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridFundSymbol.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ultraGridFundSymbol.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGridFundSymbol.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridFundSymbol.DisplayLayout.Override.TemplateAddRowAppearance = appearance7;
            this.ultraGridFundSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridFundSymbol.Location = new System.Drawing.Point(0, 0);
            this.ultraGridFundSymbol.Name = "ultraGridFundSymbol";
            this.ultraGridFundSymbol.Size = new System.Drawing.Size(800, 128);
            this.ultraGridFundSymbol.TabIndex = 0;
            this.ultraGridFundSymbol.Text = "ultraGrid1";
            this.ultraGridFundSymbol.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.ultraGridFundSymbol.AfterEnterEditMode += new System.EventHandler(this.ultraGridFundSymbol_AfterEnterEditMode);
            this.ultraGridFundSymbol.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ultraGridFundSymbol_BeforeExitEditMode);
            this.ultraGridFundSymbol.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ultraGridFundSymbol_BeforeRowsDeleted);
            // 
            // ultraControlContainerEditor1
            // 
            this.ultraControlContainerEditor1.ContainingControl = this;
            this.ultraControlContainerEditor1.Name = "ultraControlContainerEditor1";
            // 
            // labelAffectedDates
            // 
            this.labelAffectedDates.AutoSize = true;
            this.labelAffectedDates.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAffectedDates.ForeColor = System.Drawing.Color.Black;
            this.labelAffectedDates.Location = new System.Drawing.Point(603, 50);
            this.labelAffectedDates.Name = "labelAffectedDates";
            this.labelAffectedDates.Size = new System.Drawing.Size(232, 13);
            this.labelAffectedDates.TabIndex = 29;
            this.labelAffectedDates.Text = "Affected Dates Under Current Selection";
            // 
            // listBoxDates
            // 
            this.listBoxDates.FormattingEnabled = true;
            this.listBoxDates.Location = new System.Drawing.Point(603, 74);
            this.listBoxDates.Name = "listBoxDates";
            this.listBoxDates.Size = new System.Drawing.Size(232, 225);
            this.listBoxDates.TabIndex = 28;
            // 
            // checkBoxIgnoreAUECID
            // 
            this.checkBoxIgnoreAUECID.AutoSize = true;
            this.checkBoxIgnoreAUECID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.checkBoxIgnoreAUECID.Location = new System.Drawing.Point(35, 134);
            this.checkBoxIgnoreAUECID.Name = "checkBoxIgnoreAUECID";
            this.checkBoxIgnoreAUECID.Size = new System.Drawing.Size(181, 17);
            this.checkBoxIgnoreAUECID.TabIndex = 27;
            this.checkBoxIgnoreAUECID.Text = "Include Non-Business Days";
            this.checkBoxIgnoreAUECID.UseVisualStyleBackColor = true;
            // 
            // numericRollBackDays
            // 
            this.numericRollBackDays.Location = new System.Drawing.Point(485, 132);
            this.numericRollBackDays.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numericRollBackDays.Name = "numericRollBackDays";
            this.numericRollBackDays.Size = new System.Drawing.Size(72, 20);
            this.numericRollBackDays.TabIndex = 24;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(38, 497);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(136, 23);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // labelRollBackDays
            // 
            this.labelRollBackDays.AutoSize = true;
            this.labelRollBackDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRollBackDays.Location = new System.Drawing.Point(395, 136);
            this.labelRollBackDays.Name = "labelRollBackDays";
            this.labelRollBackDays.Size = new System.Drawing.Size(62, 13);
            this.labelRollBackDays.TabIndex = 21;
            this.labelRollBackDays.Text = "Roll Back";
            // 
            // numericAdjustFrom
            // 
            this.numericAdjustFrom.Location = new System.Drawing.Point(130, 89);
            this.numericAdjustFrom.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numericAdjustFrom.Minimum = new decimal(new int[] {
            365,
            0,
            0,
            -2147483648});
            this.numericAdjustFrom.Name = "numericAdjustFrom";
            this.numericAdjustFrom.Size = new System.Drawing.Size(72, 20);
            this.numericAdjustFrom.TabIndex = 20;
            // 
            // labelAdjustFrom
            // 
            this.labelAdjustFrom.AutoSize = true;
            this.labelAdjustFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdjustFrom.Location = new System.Drawing.Point(35, 93);
            this.labelAdjustFrom.Name = "labelAdjustFrom";
            this.labelAdjustFrom.Size = new System.Drawing.Size(73, 13);
            this.labelAdjustFrom.TabIndex = 17;
            this.labelAdjustFrom.Text = "Adjust From";
            // 
            // labelAdjustTo
            // 
            this.labelAdjustTo.AutoSize = true;
            this.labelAdjustTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdjustTo.Location = new System.Drawing.Point(396, 93);
            this.labelAdjustTo.Name = "labelAdjustTo";
            this.labelAdjustTo.Size = new System.Drawing.Size(61, 13);
            this.labelAdjustTo.TabIndex = 19;
            this.labelAdjustTo.Text = "Adjust To";
            // 
            // numericAdjustTo
            // 
            this.numericAdjustTo.Location = new System.Drawing.Point(485, 89);
            this.numericAdjustTo.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numericAdjustTo.Minimum = new decimal(new int[] {
            365,
            0,
            0,
            -2147483648});
            this.numericAdjustTo.Name = "numericAdjustTo";
            this.numericAdjustTo.Size = new System.Drawing.Size(72, 20);
            this.numericAdjustTo.TabIndex = 18;
            // 
            // PanelFundSymbol
            // 
            this.PanelFundSymbol.Controls.Add(this.ultraGridFundSymbol);
            this.PanelFundSymbol.Location = new System.Drawing.Point(35, 345);
            this.PanelFundSymbol.Name = "PanelFundSymbol";
            this.PanelFundSymbol.Size = new System.Drawing.Size(800, 128);
            this.PanelFundSymbol.TabIndex = 8;
            // 
            // LabelTo
            // 
            this.LabelTo.AutoSize = true;
            this.LabelTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTo.Location = new System.Drawing.Point(317, 50);
            this.LabelTo.Name = "LabelTo";
            this.LabelTo.Size = new System.Drawing.Size(22, 13);
            this.LabelTo.TabIndex = 3;
            this.LabelTo.Text = "To";
            // 
            // LabelFundSymbol
            // 
            this.LabelFundSymbol.AutoSize = true;
            this.LabelFundSymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelFundSymbol.ForeColor = System.Drawing.Color.Black;
            this.LabelFundSymbol.Location = new System.Drawing.Point(35, 318);
            this.LabelFundSymbol.Name = "LabelFundSymbol";
            this.LabelFundSymbol.Size = new System.Drawing.Size(79, 13);
            this.LabelFundSymbol.TabIndex = 7;
            this.LabelFundSymbol.Text = "Fund Symbol";
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Location = new System.Drawing.Point(357, 46);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.ShowCheckBox = true;
            this.dateTimePickerTo.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerTo.TabIndex = 1;
            // 
            // labelFund
            // 
            this.labelFund.AutoSize = true;
            this.labelFund.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFund.ForeColor = System.Drawing.Color.Black;
            this.labelFund.Location = new System.Drawing.Point(32, 249);
            this.labelFund.Name = "labelFund";
            this.labelFund.Size = new System.Drawing.Size(35, 13);
            this.labelFund.TabIndex = 5;
            this.labelFund.Text = "Fund";
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Location = new System.Drawing.Point(95, 46);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.ShowCheckBox = true;
            this.dateTimePickerFrom.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerFrom.TabIndex = 0;
            // 
            // comboBoxFund
            // 
            this.comboBoxFund.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxFund.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxFund.FormattingEnabled = true;
            this.comboBoxFund.Location = new System.Drawing.Point(33, 278);
            this.comboBoxFund.Name = "comboBoxFund";
            this.comboBoxFund.Size = new System.Drawing.Size(522, 21);
            this.comboBoxFund.TabIndex = 4;
                       // 
            // LabelFrom
            // 
            this.LabelFrom.AutoSize = true;
            this.LabelFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelFrom.ForeColor = System.Drawing.Color.Black;
            this.LabelFrom.Location = new System.Drawing.Point(35, 50);
            this.LabelFrom.Name = "LabelFrom";
            this.LabelFrom.Size = new System.Drawing.Size(34, 13);
            this.LabelFrom.TabIndex = 2;
            this.LabelFrom.Text = "From";
            // 
            // numericCores
            // 
            this.numericCores.Location = new System.Drawing.Point(130, 178);
            this.numericCores.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericCores.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericCores.Name = "numericCores";
            this.numericCores.Size = new System.Drawing.Size(72, 20);
            this.numericCores.TabIndex = 26;
            this.numericCores.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelCores
            // 
            this.labelCores.AutoSize = true;
            this.labelCores.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCores.Location = new System.Drawing.Point(35, 182);
            this.labelCores.Name = "labelCores";
            this.labelCores.Size = new System.Drawing.Size(68, 13);
            this.labelCores.TabIndex = 22;
            this.labelCores.Text = "# of CPU\'s";
            // 
            // btnRun
            // 
            this.btnRun.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.Location = new System.Drawing.Point(235, 496);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(136, 23);
            this.btnRun.TabIndex = 25;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.Location = new System.Drawing.Point(699, 496);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(136, 23);
            this.btnCreate.TabIndex = 27;
            this.btnCreate.Text = "Create Batch File";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStep.Location = new System.Drawing.Point(396, 182);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(33, 13);
            this.labelStep.TabIndex = 24;
            this.labelStep.Text = "Step";
            // 
            // txtBatch
            // 
            this.txtBatch.Location = new System.Drawing.Point(435, 497);
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Size = new System.Drawing.Size(200, 20);
            this.txtBatch.TabIndex = 28;
            // 
            // numericStep
            // 
            this.numericStep.Location = new System.Drawing.Point(483, 178);
            this.numericStep.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericStep.Name = "numericStep";
            this.numericStep.Size = new System.Drawing.Size(72, 20);
            this.numericStep.TabIndex = 23;
            this.numericStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // bgwFetchSymbols
            // 
            this.bgwFetchSymbols.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwFetchSymbols_DoWork);
            this.bgwFetchSymbols.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwFetchSymbols_RunWorkerCompleted);
            // 
            // labelFundsCount
            // 
            this.labelFundsCount.AutoSize = true;
            this.labelFundsCount.Location = new System.Drawing.Point(143, 318);
            this.labelFundsCount.Name = "labelFundsCount";
            this.labelFundsCount.Size = new System.Drawing.Size(0, 13);
            this.labelFundsCount.TabIndex = 30;
            // 
            // checkBoxAllFunds
            // 
            this.checkBoxAllFunds.AutoSize = true;
            this.checkBoxAllFunds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllFunds.Location = new System.Drawing.Point(477, 245);
            this.checkBoxAllFunds.Name = "checkBoxAllFunds";
            this.checkBoxAllFunds.Size = new System.Drawing.Size(78, 17);
            this.checkBoxAllFunds.TabIndex = 31;
            this.checkBoxAllFunds.Text = "All Funds";
            this.checkBoxAllFunds.UseVisualStyleBackColor = true;
            this.checkBoxAllFunds.CheckStateChanged += new System.EventHandler(this.checkBoxAllFunds_CheckStateChanged);
            // 
            // checkBoxSaveEnabled
            // 
            this.checkBoxSaveEnabled.AutoSize = true;
            this.checkBoxSaveEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.checkBoxSaveEnabled.Location = new System.Drawing.Point(423, 214);
            this.checkBoxSaveEnabled.Name = "checkBoxSaveEnabled";
            this.checkBoxSaveEnabled.Size = new System.Drawing.Size(132, 17);
            this.checkBoxSaveEnabled.TabIndex = 32;
            this.checkBoxSaveEnabled.Text = "Save Within Batch";
            this.checkBoxSaveEnabled.UseVisualStyleBackColor = true;
            // 
            // labelSymbolsStatus
            // 
            this.labelSymbolsStatus.AutoSize = true;
            this.labelSymbolsStatus.Location = new System.Drawing.Point(254, 318);
            this.labelSymbolsStatus.Name = "labelSymbolsStatus";
            this.labelSymbolsStatus.Size = new System.Drawing.Size(0, 13);
            this.labelSymbolsStatus.TabIndex = 33;
            // 
            // checkBoxTouchStep
            // 
            this.checkBoxTouchStep.AutoSize = true;
            this.checkBoxTouchStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.checkBoxTouchStep.Location = new System.Drawing.Point(38, 214);
            this.checkBoxTouchStep.Name = "checkBoxTouchStep";
            this.checkBoxTouchStep.Size = new System.Drawing.Size(92, 17);
            this.checkBoxTouchStep.TabIndex = 34;
            this.checkBoxTouchStep.Text = "Touch Step";
            this.checkBoxTouchStep.UseVisualStyleBackColor = true;
            // 
            // ctrlFundSymbol
            // 
            this.ctrlFundSymbol.AutoSize = true;
            this.ctrlFundSymbol.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ctrlFundSymbol.Fund = null;
            this.ctrlFundSymbol.Location = new System.Drawing.Point(183, 27);
            this.ctrlFundSymbol.Margin = new System.Windows.Forms.Padding(0);
            this.ctrlFundSymbol.Name = "ctrlFundSymbol";
            this.ctrlFundSymbol.Size = new System.Drawing.Size(0, 0);
            this.ctrlFundSymbol.TabIndex = 1;
            this.ctrlFundSymbol.Visible = false;
            // 
            // FundSymbolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 556);
            this.Controls.Add(this.checkBoxTouchStep);
            this.Controls.Add(this.labelSymbolsStatus);
            this.Controls.Add(this.checkBoxSaveEnabled);
            this.Controls.Add(this.checkBoxAllFunds);
            this.Controls.Add(this.labelFundsCount);
            this.Controls.Add(this.checkBoxIgnoreAUECID);
            this.Controls.Add(this.labelAffectedDates);
            this.Controls.Add(this.numericRollBackDays);
            this.Controls.Add(this.listBoxDates);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.numericCores);
            this.Controls.Add(this.labelRollBackDays);
            this.Controls.Add(this.labelCores);
            this.Controls.Add(this.numericAdjustFrom);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.labelAdjustFrom);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.labelAdjustTo);
            this.Controls.Add(this.labelStep);
            this.Controls.Add(this.numericAdjustTo);
            this.Controls.Add(this.txtBatch);
            this.Controls.Add(this.PanelFundSymbol);
            this.Controls.Add(this.numericStep);
            this.Controls.Add(this.LabelTo);
            this.Controls.Add(this.LabelFundSymbol);
            this.Controls.Add(this.ctrlFundSymbol);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.LabelFrom);
            this.Controls.Add(this.labelFund);
            this.Controls.Add(this.comboBoxFund);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Name = "FundSymbolForm";
            this.Text = "CS Batch UI";
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridFundSymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraControlContainerEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRollBackDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericAdjustFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericAdjustTo)).EndInit();
            this.PanelFundSymbol.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceDates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceFundSymbols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceFunds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericStep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridFundSymbol;
        private Infragistics.Win.UltraWinEditors.UltraControlContainerEditor ultraControlContainerEditor1;
        private FundSymbolControl ctrlFundSymbol;
        private System.Windows.Forms.Label LabelTo;
        private System.Windows.Forms.Label LabelFundSymbol;
        public System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Label labelFund;
        public System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.ComboBox comboBoxFund;
        private System.Windows.Forms.Label LabelFrom;
        private System.Windows.Forms.Panel PanelFundSymbol;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.NumericUpDown numericAdjustFrom;
        private System.Windows.Forms.Label labelAdjustFrom;
        private System.Windows.Forms.Label labelAdjustTo;
        private System.Windows.Forms.NumericUpDown numericAdjustTo;
        private System.Windows.Forms.NumericUpDown numericRollBackDays;
        private System.Windows.Forms.Label labelRollBackDays;
        private System.Windows.Forms.CheckBox checkBoxIgnoreAUECID;
        private System.Windows.Forms.ListBox listBoxDates;
        private System.Windows.Forms.BindingSource BindingSourceDates;
        private System.Windows.Forms.Label labelAffectedDates;
        private System.Windows.Forms.BindingSource BindingSourceFundSymbols;
        private System.Windows.Forms.BindingSource BindingSourceFunds;
        private System.Windows.Forms.NumericUpDown numericCores;
        private System.Windows.Forms.Label labelCores;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.TextBox txtBatch;
        private System.Windows.Forms.NumericUpDown numericStep;
        private System.ComponentModel.BackgroundWorker bgwFetchSymbols;
        private System.Windows.Forms.Label labelFundsCount;
        private System.Windows.Forms.CheckBox checkBoxAllFunds;
        private System.Windows.Forms.CheckBox checkBoxSaveEnabled;
        private System.Windows.Forms.Label labelSymbolsStatus;
        private System.Windows.Forms.CheckBox checkBoxTouchStep;


    }
}