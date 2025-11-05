namespace Prana.PM.Client.UI.Controls.Preferences
{
    partial class PMPreferenceControl
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
           // this.chkClosingMark = new System.Windows.Forms.CheckBox();
            this.spnPercentAvgVol = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblShowPMToolbar = new Infragistics.Win.Misc.UltraLabel();
            this.chbIsShowPMToolbar = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkClosingMark
            // 
            //this.chkClosingMark.AutoSize = true;
            //this.chkClosingMark.Location = new System.Drawing.Point(26, 17);
            //this.chkClosingMark.Name = "chkClosingMark";
            //this.chkClosingMark.Size = new System.Drawing.Size(360, 17);
            //this.chkClosingMark.TabIndex = 0;
            //this.chkClosingMark.Text = "Use closing marks to calculate Day P/L, Market Values and Exposures";
            //this.chkClosingMark.UseVisualStyleBackColor = true;
            //this.chkClosingMark.Click += new System.EventHandler(this.chkClosingMark_click);
            // 
            // spnPercentAvgVol
            // 
            this.spnPercentAvgVol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnPercentAvgVol.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.Numeric;
            this.spnPercentAvgVol.DecimalPoints = 2147483647;
            this.spnPercentAvgVol.Increment = 0.1;
            this.spnPercentAvgVol.Location = new System.Drawing.Point(26, 59);
            this.spnPercentAvgVol.MaxValue = int.MaxValue;
            this.spnPercentAvgVol.MinValue = 0;
            this.spnPercentAvgVol.Name = "spnPercentAvgVol";
            this.spnPercentAvgVol.Size = new System.Drawing.Size(66, 20);
            this.spnPercentAvgVol.TabIndex = 1;
            this.spnPercentAvgVol.Tag = "";
            this.spnPercentAvgVol.Value = 1;
            this.spnPercentAvgVol.ValueChanged += new System.EventHandler(this.spnPercentAvgVol_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(112, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Value of X for the calculation % Exposure over ‘X’ percent of average volume ";
            // 
            // label2
            // 
            this.lblShowPMToolbar.Location = new System.Drawing.Point(112, 105);
            this.lblShowPMToolbar.Name = "label2";
            this.lblShowPMToolbar.Size = new System.Drawing.Size(100, 23);
            this.lblShowPMToolbar.TabIndex = 4;
            this.lblShowPMToolbar.Text = "Show PM toolbar";
            // 
            // checkBox1
            // 
            this.chbIsShowPMToolbar.AutoSize = true;
            this.chbIsShowPMToolbar.Location = new System.Drawing.Point(77, 105);
            this.chbIsShowPMToolbar.Name = "checkBox1";
            this.chbIsShowPMToolbar.Size = new System.Drawing.Size(15, 14);
            this.chbIsShowPMToolbar.TabIndex = 8;
            this.chbIsShowPMToolbar.UseVisualStyleBackColor = true;
            this.chbIsShowPMToolbar.Checked = false;
            this.chbIsShowPMToolbar.CheckedChanged += new System.EventHandler(this.chbIsShowPMToolbar_CheckedChanged);
            // 
            // PMPreferenceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chbIsShowPMToolbar);
            this.Controls.Add(this.lblShowPMToolbar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spnPercentAvgVol);
            //this.Controls.Add(this.chkClosingMark);
            this.Name = "PMPreferenceControl";
            this.Size = new System.Drawing.Size(557, 273);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        //private Infra.CheckBox chkClosingMark;
        // private System.Windows.Forms.CheckBox chkClosingMark;
        private Prana.Utilities.UI.UIUtilities.Spinner spnPercentAvgVol;
        private Infragistics.Win.Misc.UltraLabel label1;
        private Infragistics.Win.Misc.UltraLabel lblShowPMToolbar;
        private System.Windows.Forms.CheckBox chbIsShowPMToolbar;
    }
}
