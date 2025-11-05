namespace Prana.Utilities.UI.UIUtilities
{
    partial class ActivityIndicatorUI
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
            this.ActivityIndicator = new Prana.Utilities.UI.ActivityIndicator();
            this.SuspendLayout();
            // 
            // ActivityIndicator
            // 
            this.ActivityIndicator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActivityIndicator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivityIndicator.ForeColor = System.Drawing.Color.Black;
            this.ActivityIndicator.Location = new System.Drawing.Point(0, 0);
            this.ActivityIndicator.Name = "ActivityIndicator";
            this.ActivityIndicator.Size = new System.Drawing.Size(429, 63);
            this.ActivityIndicator.TabIndex = 0;
            // 
            // ActivityIndicatorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 63);
            this.ControlBox = false;
            this.Controls.Add(this.ActivityIndicator);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(447, 95);
            this.Name = "ActivityIndicatorUI";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Task Progress";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.White;
            this.ResumeLayout(false);

        }

        #endregion

        private ActivityIndicator ActivityIndicator;
    }
}