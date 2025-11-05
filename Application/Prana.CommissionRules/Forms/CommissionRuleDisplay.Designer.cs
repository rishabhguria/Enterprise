namespace Nirvana.CommissionRules
{
    partial class CommissionRuleDisplay
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
            this.lblAssets = new Infragistics.Win.Misc.UltraLabel();
            this.lblDescription = new Infragistics.Win.Misc.UltraLabel();
            this.lblAppliedRule = new Infragistics.Win.Misc.UltraLabel();
            this.lblFeesRate = new Infragistics.Win.Misc.UltraLabel();
            this.lblCommissionCriteria = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // lblAssets
            // 
            this.lblAssets.Location = new System.Drawing.Point(17, 15);
            this.lblAssets.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblAssets.Name = "lblAssets";
            this.lblAssets.Size = new System.Drawing.Size(356, 107);
            this.lblAssets.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(16, 192);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(356, 25);
            this.lblDescription.TabIndex = 1;
            // 
            // lblAppliedRule
            // 
            this.lblAppliedRule.Location = new System.Drawing.Point(17, 133);
            this.lblAppliedRule.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblAppliedRule.Name = "lblAppliedRule";
            this.lblAppliedRule.Size = new System.Drawing.Size(356, 34);
            this.lblAppliedRule.TabIndex = 2;
            // 
            // lblFeesRate
            // 
            this.lblFeesRate.Location = new System.Drawing.Point(13, 281);
            this.lblFeesRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblFeesRate.Name = "lblFeesRate";
            this.lblFeesRate.Size = new System.Drawing.Size(356, 40);
            this.lblFeesRate.TabIndex = 6;
            // 
            // lblCommissionCriteria
            // 
            this.lblCommissionCriteria.Location = new System.Drawing.Point(16, 192);
            this.lblCommissionCriteria.Margin = new System.Windows.Forms.Padding(4);
            this.lblCommissionCriteria.Name = "lblCommissionCriteria";
            this.lblCommissionCriteria.Size = new System.Drawing.Size(356, 78);
            this.lblCommissionCriteria.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(200, 329);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // CommissionRuleDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(452, 370);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblFeesRate);
            this.Controls.Add(this.lblCommissionCriteria);
            this.Controls.Add(this.lblAppliedRule);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblAssets);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CommissionRuleDisplay";
            this.Load += new System.EventHandler(this.CommissionRuleDisplay_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblAssets;
        private Infragistics.Win.Misc.UltraLabel lblDescription;
        private Infragistics.Win.Misc.UltraLabel lblAppliedRule;
        private Infragistics.Win.Misc.UltraLabel lblFeesRate;
        private Infragistics.Win.Misc.UltraLabel lblCommissionCriteria;
        private Infragistics.Win.Misc.UltraButton btnClose;

    }
}

