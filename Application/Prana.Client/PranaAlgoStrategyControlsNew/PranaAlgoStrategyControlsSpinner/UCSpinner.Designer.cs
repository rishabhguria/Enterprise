using System.Windows.Forms;
namespace PranaAlgoStrategyControlsSpinner
{
    partial class UCSpinner
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
            this.lblName = new Label();
            this.spinner = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.SuspendLayout();
            // 
            // lblName
            // 
            //this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(90, 23);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Spinner";
            // 
            // spinner
            // 
            //this.spinner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.spinner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spinner.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.Numeric;
            this.spinner.DecimalEntered = false;
            this.spinner.DecimalPoints = 2147483647;
            this.spinner.ForeColor = System.Drawing.Color.Black;
            this.spinner.Increment = 1D;
            this.spinner.Location = new System.Drawing.Point(0, 23);
            this.spinner.MaxValue = 99999D;
            this.spinner.MinValue = 1D;
            this.spinner.Name = "spinner";
            this.spinner.Size = new System.Drawing.Size(65, 23);
            this.spinner.TabIndex = 0;
            this.spinner.Value = 1D;
            // 
            // UCSpinner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.spinner);
            this.Name = "UCSpinner";
            this.Size = new System.Drawing.Size(163, 33);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Prana.Utilities.UI.UIUtilities.Spinner spinner;
        private Label lblName;
    }
}
