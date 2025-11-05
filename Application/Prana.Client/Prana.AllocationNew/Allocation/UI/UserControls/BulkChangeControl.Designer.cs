namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    partial class BulkChangeControl
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
            if(this.ultraCmbAllocationBase!=null)
            {
                this.ultraCmbAllocationBase.Dispose();
                this.ultraCmbAllocationBase = null;
            }
            if (this.ultraCmbPrefAccount != null)
            {
                this.ultraCmbPrefAccount.Dispose();
                this.ultraCmbPrefAccount = null;
            }
            if (this.ultraCmbMatchRule != null)
            {
                this.ultraCmbMatchRule.Dispose();
                this.ultraCmbMatchRule = null;
            }
            if (this.ultraCmbMatchPosition != null)
            {
                this.ultraCmbMatchPosition.Dispose();
                this.ultraCmbMatchPosition = null;
            }
            if (this.ultraCmbSelectPreferences != null)
            {
                this.ultraCmbSelectPreferences.Dispose();
                this.ultraCmbSelectPreferences = null;
            }

            if (this.ultraChckAllocationBase != null)
            {
                this.ultraChckAllocationBase.CheckedChanged -= new System.EventHandler(this.ultraChckAllocationBase_CheckedChanged);
                this.ultraChckAllocationBase.Dispose();
                this.ultraChckAllocationBase = null;
            }
            if (this.ultraChckPrefAccount != null)
            {
                this.ultraChckPrefAccount.CheckedChanged -= new System.EventHandler(this.ultraChckPrefAccount_CheckedChanged);
                this.ultraChckPrefAccount.Dispose();
                this.ultraChckPrefAccount=null;
            }

            if (ultraChckMatchRule != null)
            {
                this.ultraChckMatchRule.CheckedChanged += new System.EventHandler(this.ultraChckMatchRule_CheckedChanged);
                ultraChckMatchRule.Dispose();
                ultraChckMatchRule = null;
            }

            if (ultraChckBoxPosition != null)
            {
                this.ultraChckBoxPosition.CheckedChanged -= new System.EventHandler(this.ultraChckBoxPosition_CheckedChanged);
                ultraChckBoxPosition.Dispose();
                ultraChckBoxPosition = null;
            }
            if (ultraBtnApply != null)
            {
                this.ultraBtnApply.Click -= new System.EventHandler(this.ultraBtnApply_Click);
                ultraBtnApply.Dispose();
                ultraBtnApply = null;
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
            this.ultraPnlMain = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraNumEditorDate = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraCmbAccounts = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLblDateUpTo = new Infragistics.Win.Misc.UltraLabel();
            this.ultralblAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.ultraChckAllocationBase = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraChckPrefAccount = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraChckMatchRule = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraChckBoxPosition = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraCmbAllocationBase = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraCmbPrefAccount = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraCmbMatchRule = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraBtnApply = new Infragistics.Win.Misc.UltraButton();
            this.ultraCmbMatchPosition = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraCmbSelectPreferences = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraChckSelectPreference = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraChckApplyDefaultRule = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraPnlMain.ClientArea.SuspendLayout();
            this.ultraPnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumEditorDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckAllocationBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckPrefAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckMatchRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckBoxPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbAllocationBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbPrefAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbMatchRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbMatchPosition)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbSelectPreferences)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckSelectPreference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckApplyDefaultRule)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPnlMain
            // 
            this.ultraPnlMain.AutoScroll = true;
            // 
            // ultraPnlMain.ClientArea
            // 
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.ultraPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPnlMain.Location = new System.Drawing.Point(0, 0);
            this.ultraPnlMain.Name = "ultraPnlMain";
            this.ultraPnlMain.Size = new System.Drawing.Size(355, 391);
            this.ultraPnlMain.TabIndex = 0;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.ultraPanel2);
            this.ultraGroupBox1.Controls.Add(this.ultraSplitter1);
            this.ultraGroupBox1.Controls.Add(this.ultraPanel1);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(355, 391);
            this.ultraGroupBox1.TabIndex = 0;
            this.ultraGroupBox1.Text = "Bulk Changes on Allocation Rule";
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraNumEditorDate);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraCmbAccounts);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraLblDateUpTo);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultralblAccounts);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraChckAllocationBase);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraChckPrefAccount);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraChckMatchRule);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraChckBoxPosition);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraCmbAllocationBase);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraCmbPrefAccount);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraCmbMatchRule);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraBtnApply);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraCmbMatchPosition);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel2.Location = new System.Drawing.Point(3, 121);
            this.ultraPanel2.MinimumSize = new System.Drawing.Size(349, 198);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(349, 267);
            this.ultraPanel2.TabIndex = 11;
            // 
            // ultraNumEditorDate
            // 
            this.ultraNumEditorDate.Location = new System.Drawing.Point(180, 204);
            this.ultraNumEditorDate.MaskInput = "nnnnnnnnn";
            this.ultraNumEditorDate.Name = "ultraNumEditorDate";
            this.ultraNumEditorDate.PromptChar = ' ';
            this.ultraNumEditorDate.Size = new System.Drawing.Size(144, 21);
            this.ultraNumEditorDate.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.ultraNumEditorDate.TabIndex = 25;
            // 
            // ultraCmbAccounts
            // 
            this.ultraCmbAccounts.CheckedListSettings.CheckBoxStyle = Infragistics.Win.CheckStyle.CheckBox;
            this.ultraCmbAccounts.CheckedListSettings.EditorValueSource = Infragistics.Win.EditorWithComboValueSource.CheckedItems;
            this.ultraCmbAccounts.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
            this.ultraCmbAccounts.CheckedListSettings.ListSeparator = ",";
            this.ultraCmbAccounts.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbAccounts.Location = new System.Drawing.Point(180, 171);
            this.ultraCmbAccounts.Name = "ultraCmbAccounts";
            this.ultraCmbAccounts.Size = new System.Drawing.Size(144, 21);
            this.ultraCmbAccounts.TabIndex = 24;
            // 
            // ultraLblDateUpTo
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BackColor2 = System.Drawing.Color.White;
            this.ultraLblDateUpTo.Appearance = appearance1;
            this.ultraLblDateUpTo.Location = new System.Drawing.Point(20, 204);
            this.ultraLblDateUpTo.Name = "ultraLblDateUpTo";
            this.ultraLblDateUpTo.Size = new System.Drawing.Size(116, 23);
            this.ultraLblDateUpTo.TabIndex = 23;
            this.ultraLblDateUpTo.Text = "Date Up to Days";
            this.ultraLblDateUpTo.UseAppStyling = false;
            // 
            // ultralblAccounts
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.ultralblAccounts.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.ultralblAccounts.HotTrackAppearance = appearance3;
            this.ultralblAccounts.Location = new System.Drawing.Point(20, 171);
            this.ultralblAccounts.Name = "ultralblAccounts";
            this.ultralblAccounts.Size = new System.Drawing.Size(116, 23);
            this.ultralblAccounts.TabIndex = 22;
            this.ultralblAccounts.Text = "Accounts for Prorata";
            this.ultralblAccounts.UseAppStyling = false;
            // 
            // ultraChckAllocationBase
            // 
            this.ultraChckAllocationBase.BackColor = System.Drawing.Color.Transparent;
            this.ultraChckAllocationBase.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraChckAllocationBase.Location = new System.Drawing.Point(15, 20);
            this.ultraChckAllocationBase.Name = "ultraChckAllocationBase";
            this.ultraChckAllocationBase.Size = new System.Drawing.Size(120, 30);
            this.ultraChckAllocationBase.TabIndex = 0;
            this.ultraChckAllocationBase.Text = "Allocation Method";
            this.ultraChckAllocationBase.CheckedChanged += new System.EventHandler(this.ultraChckAllocationBase_CheckedChanged);
            // 
            // ultraChckPrefAccount
            // 
            this.ultraChckPrefAccount.BackColor = System.Drawing.Color.Transparent;
            this.ultraChckPrefAccount.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraChckPrefAccount.Location = new System.Drawing.Point(15, 56);
            this.ultraChckPrefAccount.Name = "ultraChckPrefAccount";
            this.ultraChckPrefAccount.Size = new System.Drawing.Size(150, 30);
            this.ultraChckPrefAccount.TabIndex = 1;
            this.ultraChckPrefAccount.Text = "Remainder allocation to";
            this.ultraChckPrefAccount.CheckedChanged += new System.EventHandler(this.ultraChckPrefAccount_CheckedChanged);
            // 
            // ultraChckMatchRule
            // 
            this.ultraChckMatchRule.BackColor = System.Drawing.Color.Transparent;
            this.ultraChckMatchRule.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraChckMatchRule.Location = new System.Drawing.Point(15, 91);
            this.ultraChckMatchRule.Name = "ultraChckMatchRule";
            this.ultraChckMatchRule.Size = new System.Drawing.Size(120, 20);
            this.ultraChckMatchRule.TabIndex = 2;
            this.ultraChckMatchRule.Text = "Target % as of";
            this.ultraChckMatchRule.CheckedChanged += new System.EventHandler(this.ultraChckMatchRule_CheckedChanged);
            // 
            // ultraChckBoxPosition
            // 
            this.ultraChckBoxPosition.BackColor = System.Drawing.Color.Transparent;
            this.ultraChckBoxPosition.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraChckBoxPosition.Location = new System.Drawing.Point(15, 126);
            this.ultraChckBoxPosition.Name = "ultraChckBoxPosition";
            this.ultraChckBoxPosition.Size = new System.Drawing.Size(150, 31);
            this.ultraChckBoxPosition.TabIndex = 3;
            this.ultraChckBoxPosition.Text = "Match closing transactions exactly";
            this.ultraChckBoxPosition.CheckedChanged += new System.EventHandler(this.ultraChckBoxPosition_CheckedChanged);
            // 
            // ultraCmbAllocationBase
            // 
            this.ultraCmbAllocationBase.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbAllocationBase.Location = new System.Drawing.Point(180, 19);
            this.ultraCmbAllocationBase.Name = "ultraCmbAllocationBase";
            this.ultraCmbAllocationBase.Size = new System.Drawing.Size(144, 21);
            this.ultraCmbAllocationBase.TabIndex = 4;
            // 
            // ultraCmbPrefAccount
            // 
            this.ultraCmbPrefAccount.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbPrefAccount.Location = new System.Drawing.Point(180, 55);
            this.ultraCmbPrefAccount.Name = "ultraCmbPrefAccount";
            this.ultraCmbPrefAccount.Size = new System.Drawing.Size(144, 21);
            this.ultraCmbPrefAccount.TabIndex = 5;
            // 
            // ultraCmbMatchRule
            // 
            this.ultraCmbMatchRule.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbMatchRule.Location = new System.Drawing.Point(180, 90);
            this.ultraCmbMatchRule.Name = "ultraCmbMatchRule";
            this.ultraCmbMatchRule.Size = new System.Drawing.Size(144, 21);
            this.ultraCmbMatchRule.TabIndex = 6;
            this.ultraCmbMatchRule.ValueChanged += new System.EventHandler(this.ultraCmbMatchRule_ValueChanged);
            // 
            // ultraBtnApply
            // 
            this.ultraBtnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraBtnApply.Location = new System.Drawing.Point(121, 233);
            this.ultraBtnApply.Name = "ultraBtnApply";
            this.ultraBtnApply.Size = new System.Drawing.Size(75, 23);
            this.ultraBtnApply.TabIndex = 7;
            this.ultraBtnApply.Text = "Apply";
            this.ultraBtnApply.Click += new System.EventHandler(this.ultraBtnApply_Click);
            // 
            // ultraCmbMatchPosition
            // 
            this.ultraCmbMatchPosition.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbMatchPosition.Location = new System.Drawing.Point(180, 136);
            this.ultraCmbMatchPosition.Name = "ultraCmbMatchPosition";
            this.ultraCmbMatchPosition.Size = new System.Drawing.Size(144, 21);
            this.ultraCmbMatchPosition.TabIndex = 8;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(3, 115);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 103;
            this.ultraSplitter1.Size = new System.Drawing.Size(349, 6);
            this.ultraSplitter1.TabIndex = 10;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraCmbSelectPreferences);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraChckSelectPreference);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraChckApplyDefaultRule);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(3, 16);
            this.ultraPanel1.MaximumSize = new System.Drawing.Size(349, 99);
            this.ultraPanel1.MinimumSize = new System.Drawing.Size(349, 99);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(349, 99);
            this.ultraPanel1.TabIndex = 9;
            // 
            // ultraCmbSelectPreferences
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            this.ultraCmbSelectPreferences.Appearance = appearance4;
            this.ultraCmbSelectPreferences.BackColor = System.Drawing.Color.White;
            this.ultraCmbSelectPreferences.CheckedListSettings.CheckBoxStyle = Infragistics.Win.CheckStyle.CheckBox;
            this.ultraCmbSelectPreferences.CheckedListSettings.EditorValueSource = Infragistics.Win.EditorWithComboValueSource.CheckedItems;
            this.ultraCmbSelectPreferences.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
            this.ultraCmbSelectPreferences.CheckedListSettings.ListSeparator = ",";
            this.ultraCmbSelectPreferences.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbSelectPreferences.Location = new System.Drawing.Point(202, 56);
            this.ultraCmbSelectPreferences.Name = "ultraCmbSelectPreferences";
            this.ultraCmbSelectPreferences.Size = new System.Drawing.Size(144, 21);
            this.ultraCmbSelectPreferences.TabIndex = 5;
            this.ultraCmbSelectPreferences.UseAppStyling = false;
            // 
            // ultraChckSelectPreference
            // 
            this.ultraChckSelectPreference.BackColor = System.Drawing.Color.Transparent;
            this.ultraChckSelectPreference.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraChckSelectPreference.Location = new System.Drawing.Point(15, 56);
            this.ultraChckSelectPreference.Name = "ultraChckSelectPreference";
            this.ultraChckSelectPreference.Size = new System.Drawing.Size(181, 30);
            this.ultraChckSelectPreference.TabIndex = 1;
            this.ultraChckSelectPreference.Text = "Apply on Selected Preferences";
            this.ultraChckSelectPreference.CheckedChanged += new System.EventHandler(this.ultraChckSelectPreference_CheckedChanged);
            // 
            // ultraChckApplyDefaultRule
            // 
            this.ultraChckApplyDefaultRule.BackColor = System.Drawing.Color.Transparent;
            this.ultraChckApplyDefaultRule.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraChckApplyDefaultRule.Location = new System.Drawing.Point(15, 20);
            this.ultraChckApplyDefaultRule.Name = "ultraChckApplyDefaultRule";
            this.ultraChckApplyDefaultRule.Size = new System.Drawing.Size(162, 20);
            this.ultraChckApplyDefaultRule.TabIndex = 0;
            this.ultraChckApplyDefaultRule.Text = "Apply on Default rule";
            // 
            // BulkChangeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.ultraPnlMain);
            this.Name = "BulkChangeControl";
            this.Size = new System.Drawing.Size(355, 391);
            this.Load += new System.EventHandler(this.BulkChangeControl_Load);
            this.ultraPnlMain.ClientArea.ResumeLayout(false);
            this.ultraPnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ClientArea.PerformLayout();
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumEditorDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckAllocationBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckPrefAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckMatchRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckBoxPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbAllocationBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbPrefAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbMatchRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbMatchPosition)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbSelectPreferences)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckSelectPreference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChckApplyDefaultRule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPnlMain;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraButton ultraBtnApply;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbMatchRule;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbPrefAccount;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbAllocationBase;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraChckBoxPosition;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraChckMatchRule;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraChckPrefAccount;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbMatchPosition;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraChckAllocationBase;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbSelectPreferences;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraChckSelectPreference;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraChckApplyDefaultRule;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor ultraNumEditorDate;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbAccounts;
        private Infragistics.Win.Misc.UltraLabel ultraLblDateUpTo;
        private Infragistics.Win.Misc.UltraLabel ultralblAccounts;

    }
}
