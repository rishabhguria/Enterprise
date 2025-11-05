namespace Prana.CashManagement
{
    partial class ctrlCashManagementPreferences
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkbxRevalPublish = new System.Windows.Forms.CheckBox();
            this.ugbxSubChoice = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkbxBond = new System.Windows.Forms.CheckBox();
            this.chkbxDividend = new System.Windows.Forms.CheckBox();
            this.chkbxPnL = new System.Windows.Forms.CheckBox();
            this.chkbx_MainOption = new System.Windows.Forms.CheckBox();
            this.ugbMarginPercent = new Infragistics.Win.Misc.UltraGroupBox();
            this.txtMarginPercentage = new System.Windows.Forms.TextBox();
            this.gbxCashManagementStartDate = new Infragistics.Win.Misc.UltraGroupBox();
            this.uDtCashManagementStartDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxSubChoice)).BeginInit();
            this.ugbxSubChoice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbMarginPercent)).BeginInit();
            this.ugbMarginPercent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxCashManagementStartDate)).BeginInit();
            this.gbxCashManagementStartDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uDtCashManagementStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ultraGroupBox1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(484, 268);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox1.Controls.Add(this.ultraGroupBox2);
            this.ultraGroupBox1.Controls.Add(this.ugbMarginPercent);
            this.ultraGroupBox1.Controls.Add(this.gbxCashManagementStartDate);
            this.ultraGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(458, 253);
            this.ultraGroupBox1.TabIndex = 0;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.chkbxRevalPublish);
            this.ultraGroupBox2.Controls.Add(this.ugbxSubChoice);
            this.ultraGroupBox2.Controls.Add(this.chkbx_MainOption);
            this.ultraGroupBox2.Location = new System.Drawing.Point(20, 76);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(438, 177);
            this.ultraGroupBox2.TabIndex = 3;
            this.ultraGroupBox2.Text = "Options";
            // 
            // chkbxRevalPublish
            // 
            this.chkbxRevalPublish.AutoSize = true;
            this.chkbxRevalPublish.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.chkbxRevalPublish.Location = new System.Drawing.Point(44, 138);
            this.chkbxRevalPublish.Name = "chkbxRevalPublish";
            this.chkbxRevalPublish.Size = new System.Drawing.Size(207, 17);
            this.chkbxRevalPublish.TabIndex = 5;
            this.chkbxRevalPublish.Text = "Enable Publishing of Revaluation Data";
            this.chkbxRevalPublish.UseVisualStyleBackColor = true;
            this.chkbxRevalPublish.CheckStateChanged += new System.EventHandler(this.chkbxRevalPublish_CheckStateChanged);
            // 
            // ugbxSubChoice
            // 
            this.ugbxSubChoice.Controls.Add(this.chkbxBond);
            this.ugbxSubChoice.Controls.Add(this.chkbxDividend);
            this.ugbxSubChoice.Controls.Add(this.chkbxPnL);
            this.ugbxSubChoice.Location = new System.Drawing.Point(32, 45);
            this.ugbxSubChoice.Name = "ugbxSubChoice";
            this.ugbxSubChoice.Size = new System.Drawing.Size(369, 123);
            this.ugbxSubChoice.TabIndex = 4;
            // 
            // chkbxBond
            // 
            this.chkbxBond.AutoSize = true;
            this.chkbxBond.Checked = true;
            this.chkbxBond.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxBond.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.chkbxBond.Location = new System.Drawing.Point(12, 65);
            this.chkbxBond.Name = "chkbxBond";
            this.chkbxBond.Size = new System.Drawing.Size(231, 17);
            this.chkbxBond.TabIndex = 2;
            this.chkbxBond.Text = "Enable Bond Accurals Transaction Creation";
            this.chkbxBond.UseVisualStyleBackColor = true;
            this.chkbxBond.CheckedChanged += new System.EventHandler(this.chkbxBond_CheckedChanged);
            // 
            // chkbxDividend
            // 
            this.chkbxDividend.AutoSize = true;
            this.chkbxDividend.Checked = true;
            this.chkbxDividend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxDividend.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.chkbxDividend.Location = new System.Drawing.Point(12, 37);
            this.chkbxDividend.Name = "chkbxDividend";
            this.chkbxDividend.Size = new System.Drawing.Size(205, 17);
            this.chkbxDividend.TabIndex = 1;
            this.chkbxDividend.Text = "Enable Dividend Transaction Creation";
            this.chkbxDividend.UseVisualStyleBackColor = true;
            this.chkbxDividend.CheckedChanged += new System.EventHandler(this.chkbxDividend_CheckedChanged);
            // 
            // chkbxPnL
            // 
            this.chkbxPnL.AutoSize = true;
            this.chkbxPnL.Checked = true;
            this.chkbxPnL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxPnL.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.chkbxPnL.Location = new System.Drawing.Point(12, 8);
            this.chkbxPnL.Name = "chkbxPnL";
            this.chkbxPnL.Size = new System.Drawing.Size(292, 17);
            this.chkbxPnL.TabIndex = 0;
            this.chkbxPnL.Text = "Enable M2M PNL Transaction Creation For Future Trade";
            this.chkbxPnL.UseVisualStyleBackColor = true;
            this.chkbxPnL.CheckedChanged += new System.EventHandler(this.chkbxPnL_CheckedChanged);
            // 
            // chkbx_MainOption
            // 
            this.chkbx_MainOption.AllowDrop = true;
            this.chkbx_MainOption.AutoSize = true;
            this.chkbx_MainOption.Checked = true;
            this.chkbx_MainOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbx_MainOption.Location = new System.Drawing.Point(13, 25);
            this.chkbx_MainOption.Name = "chkbx_MainOption";
            this.chkbx_MainOption.Size = new System.Drawing.Size(334, 17);
            this.chkbx_MainOption.TabIndex = 0;
            this.chkbx_MainOption.Text = "Use Nirvana Cash Management To Calculate Daily Cash";
            this.chkbx_MainOption.UseVisualStyleBackColor = true;
            this.chkbx_MainOption.CheckedChanged += new System.EventHandler(this.chkbx_MainOption_CheckedChanged);
            // 
            // ugbMarginPercent
            // 
            this.ugbMarginPercent.Controls.Add(this.txtMarginPercentage);
            this.ugbMarginPercent.Location = new System.Drawing.Point(235, 12);
            this.ugbMarginPercent.Name = "ugbMarginPercent";
            this.ugbMarginPercent.Size = new System.Drawing.Size(214, 58);
            this.ugbMarginPercent.TabIndex = 2;
            this.ugbMarginPercent.Text = "Margin Percent For Future";
            // 
            // txtMarginPercentage
            // 
            this.txtMarginPercentage.Location = new System.Drawing.Point(50, 27);
            this.txtMarginPercentage.Name = "txtMarginPercentage";
            this.txtMarginPercentage.Size = new System.Drawing.Size(100, 21);
            this.txtMarginPercentage.TabIndex = 1;
            // 
            // gbxCashManagementStartDate
            // 
            this.gbxCashManagementStartDate.Controls.Add(this.uDtCashManagementStartDate);
            this.gbxCashManagementStartDate.Location = new System.Drawing.Point(11, 12);
            this.gbxCashManagementStartDate.Name = "gbxCashManagementStartDate";
            this.gbxCashManagementStartDate.Size = new System.Drawing.Size(214, 58);
            this.gbxCashManagementStartDate.TabIndex = 1;
            this.gbxCashManagementStartDate.Text = "Cash Management Start Date";
            // 
            // uDtCashManagementStartDate
            // 
            this.uDtCashManagementStartDate.Location = new System.Drawing.Point(44, 28);
            this.uDtCashManagementStartDate.Name = "uDtCashManagementStartDate";
            this.uDtCashManagementStartDate.Size = new System.Drawing.Size(100, 22);
            this.uDtCashManagementStartDate.TabIndex = 0;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(484, 268);
            // 
            // ultraTabControl1
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance1.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ultraTabControl1.ActiveTabAppearance = appearance1;
            appearance2.BackColor2 = System.Drawing.Color.White;
            appearance2.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.ultraTabControl1.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.ultraTabControl1.ClientAreaAppearance = appearance3;
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(486, 289);
            this.ultraTabControl1.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.ultraTabControl1.TabIndex = 0;
            ultraTab1.Key = "tbGeneral";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "General";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            // 
            // ctrlCashManagementPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraTabControl1);
            this.Name = "ctrlCashManagementPreferences";
            this.Size = new System.Drawing.Size(486, 289);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxSubChoice)).EndInit();
            this.ugbxSubChoice.ResumeLayout(false);
            this.ugbxSubChoice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbMarginPercent)).EndInit();
            this.ugbMarginPercent.ResumeLayout(false);
            this.ugbMarginPercent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxCashManagementStartDate)).EndInit();
            this.gbxCashManagementStartDate.ResumeLayout(false);
            this.gbxCashManagementStartDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uDtCashManagementStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.Misc.UltraGroupBox ugbxSubChoice;
        private System.Windows.Forms.CheckBox chkbxBond;
        private System.Windows.Forms.CheckBox chkbxDividend;
        private System.Windows.Forms.CheckBox chkbxPnL;
        private System.Windows.Forms.CheckBox chkbx_MainOption;
        private Infragistics.Win.Misc.UltraGroupBox ugbMarginPercent;
        private System.Windows.Forms.TextBox txtMarginPercentage;
        private Infragistics.Win.Misc.UltraGroupBox gbxCashManagementStartDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor uDtCashManagementStartDate;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private System.Windows.Forms.CheckBox chkbxRevalPublish;

    }
}
