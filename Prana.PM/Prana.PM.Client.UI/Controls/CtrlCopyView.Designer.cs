namespace Prana.PM.Client.UI.Controls
{
    partial class CtrlCopyView
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
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.labelStatus = new Infragistics.Win.Misc.UltraLabel();
            this.comboCopyFrom = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.comboDefaultView = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.labelCopyTo = new Infragistics.Win.Misc.UltraLabel();
            this.labelCopyFrom = new Infragistics.Win.Misc.UltraLabel();
            this.labelDefaultView = new Infragistics.Win.Misc.UltraLabel();
            this.checkboxIncludeFilers = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkboxIncludeGrouping = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.comboCopyTo = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboCopyFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxIncludeFilers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxIncludeGrouping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboDefaultView)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.labelStatus);
            this.ultraGroupBox1.Controls.Add(this.comboCopyFrom);
            this.ultraGroupBox1.Controls.Add(this.labelCopyTo);
            this.ultraGroupBox1.Controls.Add(this.labelCopyFrom);
            this.ultraGroupBox1.Controls.Add(this.checkboxIncludeFilers);
            this.ultraGroupBox1.Controls.Add(this.checkboxIncludeGrouping);
            this.ultraGroupBox1.Controls.Add(this.comboCopyTo);
            this.ultraGroupBox1.Location = new System.Drawing.Point(25, 14);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(318, 212);
            this.ultraGroupBox1.TabIndex = 1;
            this.ultraGroupBox1.Text = "Copy Views";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(14, 182);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(252, 23);
            this.labelStatus.TabIndex = 21;
            this.labelStatus.UseAppStyling = false;
            // 
            // comboCopyFrom
            // 
            this.comboCopyFrom.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.comboCopyFrom.Location = new System.Drawing.Point(14, 39);
            this.comboCopyFrom.Name = "comboCopyFrom";
            this.comboCopyFrom.NullText = "Select Tab Name";
            appearance1.ForeColor = System.Drawing.Color.Gray;
            this.comboCopyFrom.NullTextAppearance = appearance1;
            this.comboCopyFrom.Size = new System.Drawing.Size(252, 21);
            this.comboCopyFrom.TabIndex = 20;
            this.comboCopyFrom.Leave += new System.EventHandler(this.comboCopyFrom_Leave);
            // 
            // labelCopyTo
            // 
            this.labelCopyTo.AutoSize = true;
            this.labelCopyTo.Location = new System.Drawing.Point(14, 66);
            this.labelCopyTo.Name = "labelCopyTo";
            this.labelCopyTo.Size = new System.Drawing.Size(47, 14);
            this.labelCopyTo.TabIndex = 19;
            this.labelCopyTo.Text = "Copy To";
            // 
            // labelCopyFrom
            // 
            this.labelCopyFrom.AutoSize = true;
            this.labelCopyFrom.Location = new System.Drawing.Point(14, 19);
            this.labelCopyFrom.Name = "labelCopyFrom";
            this.labelCopyFrom.Size = new System.Drawing.Size(60, 14);
            this.labelCopyFrom.TabIndex = 18;
            this.labelCopyFrom.Text = "Copy From";
            // 
            // checkboxIncludeFilers
            // 
            this.checkboxIncludeFilers.Location = new System.Drawing.Point(14, 148);
            this.checkboxIncludeFilers.Name = "checkboxIncludeFilers";
            this.checkboxIncludeFilers.Size = new System.Drawing.Size(120, 20);
            this.checkboxIncludeFilers.TabIndex = 3;
            this.checkboxIncludeFilers.Text = "Include Fillters";
            // 
            // checkboxIncludeGrouping
            // 
            this.checkboxIncludeGrouping.Checked = true;
            this.checkboxIncludeGrouping.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxIncludeGrouping.Location = new System.Drawing.Point(14, 122);
            this.checkboxIncludeGrouping.Name = "checkboxIncludeGrouping";
            this.checkboxIncludeGrouping.Size = new System.Drawing.Size(120, 20);
            this.checkboxIncludeGrouping.TabIndex = 2;
            this.checkboxIncludeGrouping.Text = "Include Grouping";
            // 
            // comboCopyTo
            // 
            this.comboCopyTo.Location = new System.Drawing.Point(14, 86);
            this.comboCopyTo.Name = "comboCopyTo";
            this.comboCopyTo.Size = new System.Drawing.Size(252, 21);
            this.comboCopyTo.TabIndex = 0;
            this.comboCopyTo.TitleText = "View";
            this.comboCopyTo.Leave += new System.EventHandler(this.comboCopyTo_Leave);
            // 
            // labelDefaultView
            // 
            this.labelDefaultView.AutoSize = true;
            this.labelDefaultView.Location = new System.Drawing.Point(25, 230);
            this.labelDefaultView.Name = "labelDefaultView";
            this.labelDefaultView.Size = new System.Drawing.Size(47, 14);
            this.labelDefaultView.TabIndex = 19;
            this.labelDefaultView.Text = "Choose Default Selected View";
            // 
            // comboDefaultView
            // 
            this.comboDefaultView.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.comboDefaultView.Location = new System.Drawing.Point(25, 250);
            this.comboDefaultView.Name = "comboDefaultView";
            appearance1.ForeColor = System.Drawing.Color.Gray;
            this.comboDefaultView.Size = new System.Drawing.Size(252, 21);
            this.comboDefaultView.TabIndex = 20;
            // 
            // CtrlCopyView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this.comboDefaultView);
            this.Controls.Add(this.labelDefaultView);
            this.Name = "CtrlCopyView";
            this.Size = new System.Drawing.Size(362, 300);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboCopyFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboDefaultView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxIncludeFilers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxIncludeGrouping)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraLabel labelCopyTo;
        private Infragistics.Win.Misc.UltraLabel labelCopyFrom;
        public Utilities.UI.UIUtilities.MultiSelectDropDown comboCopyTo;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor checkboxIncludeFilers;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor checkboxIncludeGrouping;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor comboCopyFrom;
        public Infragistics.Win.Misc.UltraLabel labelStatus;
        private Infragistics.Win.Misc.UltraLabel labelDefaultView;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor comboDefaultView;
    }
}
