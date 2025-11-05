namespace Prana.Tools
{
    partial class MatchingRuleForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.btnRefresh = new Infragistics.Win.Misc.UltraButton();
            this.pnlRules = new System.Windows.Forms.Panel();
            this.btnOpenXML = new Infragistics.Win.Misc.UltraButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lstbxRules = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance1.BackColor = System.Drawing.SystemColors.Desktop;
            this.btnRefresh.Appearance = appearance1;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnRefresh.Location = new System.Drawing.Point(12, 200);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(54, 22);
            this.btnRefresh.TabIndex = 118;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefersh_Click);
            // 
            // pnlRules
            // 
            this.pnlRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRules.AutoScroll = true;
            this.pnlRules.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlRules.Location = new System.Drawing.Point(90, 1);
            this.pnlRules.Name = "pnlRules";
            this.pnlRules.Size = new System.Drawing.Size(309, 197);
            this.pnlRules.TabIndex = 119;
            // 
            // btnOpenXML
            // 
            this.btnOpenXML.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance4.BackColor = System.Drawing.SystemColors.Desktop;
            this.btnOpenXML.Appearance = appearance4;
            this.btnOpenXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnOpenXML.Location = new System.Drawing.Point(185, 200);
            this.btnOpenXML.Name = "btnOpenXML";
            this.btnOpenXML.Size = new System.Drawing.Size(78, 22);
            this.btnOpenXML.TabIndex = 120;
            this.btnOpenXML.Text = "Open Rule File";
            this.btnOpenXML.Click += new System.EventHandler(this.btnOpenXML_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lstbxRules
            // 
            this.lstbxRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstbxRules.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstbxRules.FormattingEnabled = true;
            this.lstbxRules.Location = new System.Drawing.Point(1, 1);
            this.lstbxRules.Name = "lstbxRules";
            this.lstbxRules.Size = new System.Drawing.Size(83, 197);
            this.lstbxRules.TabIndex = 121;
            this.lstbxRules.Click += new System.EventHandler(this.lstbxRules_Click);
            // 
            // MatchingRuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 223);
            this.Controls.Add(this.lstbxRules);
            this.Controls.Add(this.btnOpenXML);
            this.Controls.Add(this.pnlRules);
            this.Controls.Add(this.btnRefresh);
            this.Name = "MatchingRuleForm";
            this.Text = "MatchingRuleForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnRefresh;
        private System.Windows.Forms.Panel pnlRules;
        private Infragistics.Win.Misc.UltraButton btnOpenXML;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox lstbxRules;

    }
}