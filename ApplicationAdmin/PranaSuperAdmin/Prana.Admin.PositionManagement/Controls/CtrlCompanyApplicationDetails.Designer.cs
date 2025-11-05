namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlCompanyApplicationDetails
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlCompanyApplicationDetails));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.rdbModels = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.lblMinimumRefreshRate = new Infragistics.Win.Misc.UltraLabel();
            this.lblUploadDataSources = new Infragistics.Win.Misc.UltraLabel();
            this.lblAllowDataMapping = new Infragistics.Win.Misc.UltraLabel();
            this.lblAllowDailyImport = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.spnMaximumRefreshRate = new System.Windows.Forms.NumericUpDown();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.checkedListBoxUploadDataSources = new System.Windows.Forms.CheckedListBox();
            this.chkBoxAllowDailyImport = new System.Windows.Forms.CheckBox();
            this.chkBoxAllowDataMapping = new System.Windows.Forms.CheckBox();
            this.rdoButtonWinDaleDummy = new System.Windows.Forms.RadioButton();
            this.bindingSourceApplicationDetails = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceUploadDataSources = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdbModels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnMaximumRefreshRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceApplicationDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUploadDataSources)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(200, 110);
            this.ultraGroupBox1.SupportThemes = false;
            this.ultraGroupBox1.TabIndex = 0;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ultraGroupBox2.Controls.Add(this.rdoButtonWinDaleDummy);
            this.ultraGroupBox2.Controls.Add(this.rdbModels);
            this.ultraGroupBox2.Location = new System.Drawing.Point(69, 3);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ultraGroupBox2.Size = new System.Drawing.Size(160, 60);
            this.ultraGroupBox2.SupportThemes = false;
            this.ultraGroupBox2.TabIndex = 0;
            this.ultraGroupBox2.Text = "Models";
            // 
            // rdbModels
            // 
            this.rdbModels.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.rdbModels.FlatMode = true;
            this.rdbModels.ItemAppearance = appearance1;
            valueListItem1.DataValue = "WinDale";
            valueListItem1.DisplayText = "WinDale";
            this.rdbModels.Items.Add(valueListItem1);
            this.rdbModels.Location = new System.Drawing.Point(6, 20);
            this.rdbModels.Name = "rdbModels";
            this.rdbModels.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rdbModels.Size = new System.Drawing.Size(96, 22);
            this.rdbModels.TabIndex = 0;
            this.rdbModels.Visible = false;
            // 
            // lblMinimumRefreshRate
            // 
            this.lblMinimumRefreshRate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMinimumRefreshRate.Location = new System.Drawing.Point(8, 73);
            this.lblMinimumRefreshRate.Name = "lblMinimumRefreshRate";
            this.lblMinimumRefreshRate.Size = new System.Drawing.Size(130, 23);
            this.lblMinimumRefreshRate.TabIndex = 1;
            this.lblMinimumRefreshRate.Text = "Minimum Refresh Rate";
            // 
            // lblUploadDataSources
            // 
            this.lblUploadDataSources.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblUploadDataSources.Location = new System.Drawing.Point(8, 166);
            this.lblUploadDataSources.Name = "lblUploadDataSources";
            this.lblUploadDataSources.Size = new System.Drawing.Size(130, 23);
            this.lblUploadDataSources.TabIndex = 2;
            this.lblUploadDataSources.Text = "Upload Data Sources";
            // 
            // lblAllowDataMapping
            // 
            this.lblAllowDataMapping.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblAllowDataMapping.Location = new System.Drawing.Point(93, 249);
            this.lblAllowDataMapping.Name = "lblAllowDataMapping";
            this.lblAllowDataMapping.Size = new System.Drawing.Size(130, 23);
            this.lblAllowDataMapping.TabIndex = 3;
            this.lblAllowDataMapping.Text = "Allow Data Mapping";
            // 
            // lblAllowDailyImport
            // 
            this.lblAllowDailyImport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblAllowDailyImport.Location = new System.Drawing.Point(93, 274);
            this.lblAllowDailyImport.Name = "lblAllowDailyImport";
            this.lblAllowDailyImport.Size = new System.Drawing.Size(130, 23);
            this.lblAllowDailyImport.TabIndex = 4;
            this.lblAllowDailyImport.Text = "Allow Daily Import";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnClose.Appearance = appearance2;
            this.btnClose.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClose.Location = new System.Drawing.Point(198, 297);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowFocusRect = false;
            this.btnClose.ShowOutline = false;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnSave.Appearance = appearance3;
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(26, 297);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spnMaximumRefreshRate
            // 
            this.spnMaximumRefreshRate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.spnMaximumRefreshRate.Location = new System.Drawing.Point(141, 74);
            this.spnMaximumRefreshRate.Name = "spnMaximumRefreshRate";
            this.spnMaximumRefreshRate.Size = new System.Drawing.Size(150, 21);
            this.spnMaximumRefreshRate.TabIndex = 18;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance4.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_clear;
            this.btnClear.Appearance = appearance4;
            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClear.Location = new System.Drawing.Point(112, 297);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.ShowFocusRect = false;
            this.btnClear.ShowOutline = false;
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 39;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // checkedListBoxUploadDataSources
            // 
            this.checkedListBoxUploadDataSources.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.checkedListBoxUploadDataSources.FormattingEnabled = true;
            this.checkedListBoxUploadDataSources.Location = new System.Drawing.Point(141, 111);
            this.checkedListBoxUploadDataSources.Name = "checkedListBoxUploadDataSources";
            this.checkedListBoxUploadDataSources.Size = new System.Drawing.Size(150, 132);
            this.checkedListBoxUploadDataSources.TabIndex = 40;
            // 
            // chkBoxAllowDailyImport
            // 
            this.chkBoxAllowDailyImport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkBoxAllowDailyImport.AutoSize = true;
            this.chkBoxAllowDailyImport.Location = new System.Drawing.Point(75, 274);
            this.chkBoxAllowDailyImport.Name = "chkBoxAllowDailyImport";
            this.chkBoxAllowDailyImport.Size = new System.Drawing.Size(15, 14);
            this.chkBoxAllowDailyImport.TabIndex = 42;
            this.chkBoxAllowDailyImport.UseVisualStyleBackColor = true;
            // 
            // chkBoxAllowDataMapping
            // 
            this.chkBoxAllowDataMapping.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkBoxAllowDataMapping.AutoSize = true;
            this.chkBoxAllowDataMapping.Location = new System.Drawing.Point(75, 249);
            this.chkBoxAllowDataMapping.Name = "chkBoxAllowDataMapping";
            this.chkBoxAllowDataMapping.Size = new System.Drawing.Size(15, 14);
            this.chkBoxAllowDataMapping.TabIndex = 43;
            this.chkBoxAllowDataMapping.UseVisualStyleBackColor = true;
            // 
            // rdoButtonWinDaleDummy
            // 
            this.rdoButtonWinDaleDummy.AutoSize = true;
            this.rdoButtonWinDaleDummy.Checked = true;
            this.rdoButtonWinDaleDummy.Location = new System.Drawing.Point(7, 37);
            this.rdoButtonWinDaleDummy.Name = "rdoButtonWinDaleDummy";
            this.rdoButtonWinDaleDummy.Size = new System.Drawing.Size(63, 17);
            this.rdoButtonWinDaleDummy.TabIndex = 1;
            this.rdoButtonWinDaleDummy.TabStop = true;
            this.rdoButtonWinDaleDummy.Text = "Windale";
            this.rdoButtonWinDaleDummy.UseVisualStyleBackColor = true;
            // 
            // bindingSourceApplicationDetails
            // 
            this.bindingSourceApplicationDetails.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.CompanyApplicationDetails);
            // 
            // bindingSourceUploadDataSources
            // 
            this.bindingSourceUploadDataSources.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.DataSourceNameIDList);
            // 
            // CtrlCompanyApplicationDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.chkBoxAllowDataMapping);
            this.Controls.Add(this.chkBoxAllowDailyImport);
            this.Controls.Add(this.checkedListBoxUploadDataSources);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.spnMaximumRefreshRate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblAllowDailyImport);
            this.Controls.Add(this.lblAllowDataMapping);
            this.Controls.Add(this.lblUploadDataSources);
            this.Controls.Add(this.lblMinimumRefreshRate);
            this.Controls.Add(this.ultraGroupBox2);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlCompanyApplicationDetails";
            this.Size = new System.Drawing.Size(298, 331);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdbModels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnMaximumRefreshRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceApplicationDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUploadDataSources)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet rdbModels;
        private Infragistics.Win.Misc.UltraLabel lblMinimumRefreshRate;
        private Infragistics.Win.Misc.UltraLabel lblUploadDataSources;
        private Infragistics.Win.Misc.UltraLabel lblAllowDataMapping;
        private Infragistics.Win.Misc.UltraLabel lblAllowDailyImport;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.NumericUpDown spnMaximumRefreshRate;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private System.Windows.Forms.BindingSource bindingSourceApplicationDetails;
        private System.Windows.Forms.CheckedListBox checkedListBoxUploadDataSources;
        private System.Windows.Forms.BindingSource bindingSourceUploadDataSources;        
        private System.Windows.Forms.CheckBox chkBoxAllowDailyImport;
        private System.Windows.Forms.CheckBox chkBoxAllowDataMapping;
        private System.Windows.Forms.RadioButton rdoButtonWinDaleDummy;
    }
}
