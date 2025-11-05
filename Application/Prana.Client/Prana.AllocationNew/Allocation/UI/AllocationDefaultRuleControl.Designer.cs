namespace Prana.AllocationNew.Allocation.UI
{
    partial class AllocationDefaultRuleControl
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
            if (ultraCmbMatchRule != null)
            {
                this.ultraCmbMatchRule.ValueChanged -= new System.EventHandler(this.ultraCmbMatchRule_ValueChanged);
               // this.ultraCmbMatchRule.ValueChanged -= new System.EventHandler(this.ultraCmbMatchRule_ValueChanged);
                ultraCmbMatchRule.Dispose();
                ultraCmbMatchRule = null;
            }
            if (ultraCmbPrefAccount != null)
            {
            this.ultraCmbPrefAccount.ValueChanged -= new System.EventHandler(this.ultraCmbPrefAccount_ValueChanged);
                ultraCmbPrefAccount.Dispose();
                ultraCmbPrefAccount = null;
            }
            if (ultraCmbAllocationBase != null)
            {
                this.ultraCmbAllocationBase.ValueChanged -= new System.EventHandler(this.ultraCmbAllocationBase_ValueChanged);
                ultraCmbAllocationBase.Dispose();
                ultraCmbAllocationBase = null;
            }

            if (ultraChckMatchPosition != null)
            {
                this.ultraChckMatchPosition.CheckedChanged -= new System.EventHandler(this.ultraChckMatchPosition_CheckedChanged);
                ultraChckMatchPosition.Dispose();
                ultraChckMatchPosition = null;
            }
            this.Load -= new System.EventHandler(this.AllocationDefaultRuleControl_Load);
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            this.ultraCmbMatchRule = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraCmbPrefAccount = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraCmbAllocationBase = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLblAllocationBase = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLblPrefAccount = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLblMatchRule = new Infragistics.Win.Misc.UltraLabel();
            this.ultraChckMatchPosition = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultralblAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLblDateUpTo = new Infragistics.Win.Misc.UltraLabel();
            this.ultraCmbAccounts = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraNumEditorDate = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraClndrDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbMatchRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbPrefAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbAllocationBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckMatchPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumEditorDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraClndrDate)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraCmbMatchRule
            // 
            this.ultraCmbMatchRule.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbMatchRule.Location = new System.Drawing.Point(188, 94);
            this.ultraCmbMatchRule.Margin = new System.Windows.Forms.Padding(4);
            this.ultraCmbMatchRule.Name = "ultraCmbMatchRule";
            this.ultraCmbMatchRule.Size = new System.Drawing.Size(168, 25);
            this.ultraCmbMatchRule.TabIndex = 11;
            this.ultraCmbMatchRule.ValueChanged += new System.EventHandler(this.ultraCmbMatchRule_ValueChanged);
            // 
            // ultraCmbPrefAccount
            // 
            this.ultraCmbPrefAccount.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbPrefAccount.Location = new System.Drawing.Point(188, 61);
            this.ultraCmbPrefAccount.Margin = new System.Windows.Forms.Padding(4);
            this.ultraCmbPrefAccount.Name = "ultraCmbPrefAccount";
            this.ultraCmbPrefAccount.Size = new System.Drawing.Size(168, 25);
            this.ultraCmbPrefAccount.TabIndex = 10;
            this.ultraCmbPrefAccount.ValueChanged += new System.EventHandler(this.ultraCmbPrefAccount_ValueChanged);
            // 
            // ultraCmbAllocationBase
            // 
            this.ultraCmbAllocationBase.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbAllocationBase.Location = new System.Drawing.Point(188, 28);
            this.ultraCmbAllocationBase.Margin = new System.Windows.Forms.Padding(4);
            this.ultraCmbAllocationBase.Name = "ultraCmbAllocationBase";
            this.ultraCmbAllocationBase.Size = new System.Drawing.Size(168, 25);
            this.ultraCmbAllocationBase.TabIndex = 9;
            this.ultraCmbAllocationBase.ValueChanged += new System.EventHandler(this.ultraCmbAllocationBase_ValueChanged);
            // 
            // ultraLblAllocationBase
            // 
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLblAllocationBase.Appearance = appearance1;
            this.ultraLblAllocationBase.Location = new System.Drawing.Point(28, 27);
            this.ultraLblAllocationBase.Margin = new System.Windows.Forms.Padding(4);
            this.ultraLblAllocationBase.Name = "ultraLblAllocationBase";
            this.ultraLblAllocationBase.Size = new System.Drawing.Size(140, 26);
            this.ultraLblAllocationBase.TabIndex = 12;
            this.ultraLblAllocationBase.Text = "Allocation Method";
            // 
            // ultraLblPrefAccount
            // 
            appearance2.TextHAlignAsString = "Left";
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLblPrefAccount.Appearance = appearance2;
            this.ultraLblPrefAccount.Location = new System.Drawing.Point(28, 60);
            this.ultraLblPrefAccount.Margin = new System.Windows.Forms.Padding(4);
            this.ultraLblPrefAccount.Name = "ultraLblPrefAccount";
            this.ultraLblPrefAccount.Size = new System.Drawing.Size(150, 26);
            this.ultraLblPrefAccount.TabIndex = 13;
            this.ultraLblPrefAccount.Text = "Remainder allocation to";
            // 
            // ultraLblMatchRule
            // 
            appearance3.TextHAlignAsString = "Left";
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLblMatchRule.Appearance = appearance3;
            this.ultraLblMatchRule.Location = new System.Drawing.Point(28, 93);
            this.ultraLblMatchRule.Margin = new System.Windows.Forms.Padding(4);
            this.ultraLblMatchRule.Name = "ultraLblMatchRule";
            this.ultraLblMatchRule.Size = new System.Drawing.Size(116, 26);
            this.ultraLblMatchRule.TabIndex = 14;
            this.ultraLblMatchRule.Text = "Target % as of";
            // 
            // ultraChckMatchPosition
            // 
            this.ultraChckMatchPosition.Location = new System.Drawing.Point(28, 189);
            this.ultraChckMatchPosition.Margin = new System.Windows.Forms.Padding(4);
            this.ultraChckMatchPosition.Name = "ultraChckMatchPosition";
            this.ultraChckMatchPosition.Size = new System.Drawing.Size(248, 23);
            this.ultraChckMatchPosition.TabIndex = 15;
            this.ultraChckMatchPosition.Text = "Match closing transactions exactly";
            this.ultraChckMatchPosition.CheckedChanged += new System.EventHandler(this.ultraChckMatchPosition_CheckedChanged);
            // 
            // ultralblAccounts
            // 
            this.ultralblAccounts.Location = new System.Drawing.Point(28, 126);
            this.ultralblAccounts.Name = "ultralblAccounts";
            this.ultralblAccounts.Size = new System.Drawing.Size(140, 23);
            this.ultralblAccounts.TabIndex = 16;
            this.ultralblAccounts.Text = "Accounts for Prorata";
            // 
            // ultraLblDateUpTo
            // 
            this.ultraLblDateUpTo.Location = new System.Drawing.Point(28, 159);
            this.ultraLblDateUpTo.Name = "ultraLblDateUpTo";
            this.ultraLblDateUpTo.Size = new System.Drawing.Size(140, 23);
            this.ultraLblDateUpTo.TabIndex = 18;
            this.ultraLblDateUpTo.Text = "Date Up to Days";
            // 
            // ultraCmbAccounts
            // 
            this.ultraCmbAccounts.CheckedListSettings.CheckBoxStyle = Infragistics.Win.CheckStyle.CheckBox;
            this.ultraCmbAccounts.CheckedListSettings.EditorValueSource = Infragistics.Win.EditorWithComboValueSource.CheckedItems;
            this.ultraCmbAccounts.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
            this.ultraCmbAccounts.CheckedListSettings.ListSeparator = ",";
            this.ultraCmbAccounts.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbAccounts.Location = new System.Drawing.Point(188, 126);
            this.ultraCmbAccounts.Name = "ultraCmbAccounts";
            this.ultraCmbAccounts.Size = new System.Drawing.Size(168, 25);
            this.ultraCmbAccounts.TabIndex = 20;
            // 
            // ultraNumEditorDate
            // 
            this.ultraNumEditorDate.Location = new System.Drawing.Point(188, 159);
            this.ultraNumEditorDate.MaskInput = "nnnnnnnnn";
            this.ultraNumEditorDate.Name = "ultraNumEditorDate";
            this.ultraNumEditorDate.PromptChar = ' ';
            this.ultraNumEditorDate.Size = new System.Drawing.Size(168, 25);
            this.ultraNumEditorDate.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.ultraNumEditorDate.TabIndex = 21;
            // 
            // ultraClndrDate
            // 
            this.ultraClndrDate.AllowNull = false;
            this.ultraClndrDate.DateButtons.Add(dateButton1);
            this.ultraClndrDate.Location = new System.Drawing.Point(188, 160);
            this.ultraClndrDate.Name = "ultraClndrDate";
            this.ultraClndrDate.NonAutoSizeHeight = 22;
            this.ultraClndrDate.Size = new System.Drawing.Size(121, 22);
            this.ultraClndrDate.TabIndex = 22;
            // 
            // AllocationDefaultRuleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.ultraClndrDate);
            this.Controls.Add(this.ultraNumEditorDate);
            this.Controls.Add(this.ultraCmbAccounts);
            this.Controls.Add(this.ultraLblDateUpTo);
            this.Controls.Add(this.ultralblAccounts);
            this.Controls.Add(this.ultraChckMatchPosition);
            this.Controls.Add(this.ultraLblMatchRule);
            this.Controls.Add(this.ultraLblPrefAccount);
            this.Controls.Add(this.ultraLblAllocationBase);
            this.Controls.Add(this.ultraCmbMatchRule);
            this.Controls.Add(this.ultraCmbPrefAccount);
            this.Controls.Add(this.ultraCmbAllocationBase);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AllocationDefaultRuleControl";
            this.Size = new System.Drawing.Size(360, 216);
            this.Load += new System.EventHandler(this.AllocationDefaultRuleControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbMatchRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbPrefAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbAllocationBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckMatchPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumEditorDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraClndrDate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbMatchRule;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbPrefAccount;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbAllocationBase;
        private Infragistics.Win.Misc.UltraLabel ultraLblAllocationBase;
        private Infragistics.Win.Misc.UltraLabel ultraLblPrefAccount;
        private Infragistics.Win.Misc.UltraLabel ultraLblMatchRule;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraChckMatchPosition;
        private Infragistics.Win.Misc.UltraLabel ultralblAccounts;
        private Infragistics.Win.Misc.UltraLabel ultraLblDateUpTo;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbAccounts;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor ultraNumEditorDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo ultraClndrDate;

    }
}
