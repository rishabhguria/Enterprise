namespace PranaAlgoStrategyControlsNoPost
{
    partial class UCNoPost
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
            this.lblName = new Infragistics.Win.Misc.UltraLabel();
            this.spinner = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.chkbxNoPost = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxNoPost)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 30);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(64, 14);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "DisplaySize";
            // 
            // spinner
            // 
            this.spinner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spinner.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.Numeric;
            this.spinner.DecimalEntered = false;
            this.spinner.DecimalPoints = 2147483647;
            this.spinner.Increment = 1D;
            this.spinner.Location = new System.Drawing.Point(92, 30);
            this.spinner.MaxValue = 99999D;
            this.spinner.MinValue = 1D;
            this.spinner.Name = "spinner";
            this.spinner.Size = new System.Drawing.Size(66, 20);
            this.spinner.TabIndex = 5;
            this.spinner.Value = 1D;
            // 
            // chkbxNoPost
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.chkbxNoPost.AutoSize = true;
            this.chkbxNoPost.Location = new System.Drawing.Point(3, 10);
            this.chkbxNoPost.Name = "chkbxNoPost";
            this.chkbxNoPost.Size = new System.Drawing.Size(58, 17);
            this.chkbxNoPost.TabIndex = 6;
            this.chkbxNoPost.Text = "NoPost";
            this.chkbxNoPost.CheckStateChanged += new System.EventHandler(this.chkbxNoPost_CheckStateChanged);
            // 
            // UCNoPost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkbxNoPost);
            this.Controls.Add(this.spinner);
            this.Controls.Add(this.lblName);
            this.Name = "UCNoPost";
            this.Size = new System.Drawing.Size(163, 56);
            ((System.ComponentModel.ISupportInitialize)(this.chkbxNoPost)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblName;
        private Prana.Utilities.UI.UIUtilities.Spinner spinner;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxNoPost;
    }
}
