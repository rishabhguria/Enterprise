namespace Prana.PM.Admin.UI.Controls
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlCompanyApplicationDetails));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.rdbModels = new System.Windows.Forms.RadioButton();
            this.lblAllowDailyImport = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.chkBoxAllowDailyImport = new System.Windows.Forms.CheckBox();
            this.bindingSourceApplicationDetails = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceUploadDataSources = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
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
            this.ultraGroupBox2.Controls.Add(this.rdbModels);
            this.ultraGroupBox2.Location = new System.Drawing.Point(83, 3);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ultraGroupBox2.Size = new System.Drawing.Size(132, 50);
            this.ultraGroupBox2.SupportThemes = false;
            this.ultraGroupBox2.TabIndex = 0;
            this.ultraGroupBox2.Text = "Models";
            // 
            // rdbModels
            // 
            this.rdbModels.AutoSize = true;
            this.rdbModels.Checked = true;
            this.rdbModels.Location = new System.Drawing.Point(10, 20);
            this.rdbModels.Name = "rdbModels";
            this.rdbModels.Size = new System.Drawing.Size(63, 17);
            this.rdbModels.TabIndex = 1;
            this.rdbModels.TabStop = true;
            this.rdbModels.Text = "Windale";
            this.rdbModels.UseVisualStyleBackColor = true;
            // 
            // lblAllowDailyImport
            // 
            this.lblAllowDailyImport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblAllowDailyImport.Location = new System.Drawing.Point(108, 61);
            this.lblAllowDailyImport.Name = "lblAllowDailyImport";
            this.lblAllowDailyImport.Size = new System.Drawing.Size(103, 23);
            this.lblAllowDailyImport.TabIndex = 4;
            this.lblAllowDailyImport.Text = "Allow Daily Import";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.btnClose.Appearance = appearance1;
            this.btnClose.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClose.Location = new System.Drawing.Point(214, 80);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowFocusRect = false;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.btnSave.Appearance = appearance2;
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(10, 80);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance3.Image = global::Prana.PM.Admin.UI.Properties.Resources.btn_clear;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.btnClear.Appearance = appearance3;
            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClear.Location = new System.Drawing.Point(112, 80);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.ShowFocusRect = false;
            this.btnClear.ShowOutline = false;
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 39;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // chkBoxAllowDailyImport
            // 
            this.chkBoxAllowDailyImport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkBoxAllowDailyImport.AutoSize = true;
            this.chkBoxAllowDailyImport.Location = new System.Drawing.Point(87, 61);
            this.chkBoxAllowDailyImport.Name = "chkBoxAllowDailyImport";
            this.chkBoxAllowDailyImport.Size = new System.Drawing.Size(15, 14);
            this.chkBoxAllowDailyImport.TabIndex = 42;
            this.chkBoxAllowDailyImport.UseVisualStyleBackColor = true;
            // 
            // bindingSourceApplicationDetails
            // 
            this.bindingSourceApplicationDetails.DataSource = typeof(Prana.PM.BLL.CompanyApplicationDetails);
            // 
            // bindingSourceUploadDataSources
            // 
            this.bindingSourceUploadDataSources.DataSource = typeof(Prana.BusinessObjects.PositionManagement.DataSourceNameIDList);
            // 
            // CtrlCompanyApplicationDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.chkBoxAllowDailyImport);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblAllowDailyImport);
            this.Controls.Add(this.ultraGroupBox2);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlCompanyApplicationDetails";
            this.Size = new System.Drawing.Size(298, 112);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceApplicationDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUploadDataSources)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.Misc.UltraLabel lblAllowDailyImport;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private System.Windows.Forms.BindingSource bindingSourceApplicationDetails;
        private System.Windows.Forms.BindingSource bindingSourceUploadDataSources;
        private System.Windows.Forms.CheckBox chkBoxAllowDailyImport;
        private System.Windows.Forms.RadioButton rdbModels;
    }
}
