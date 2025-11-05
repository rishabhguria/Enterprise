namespace Prana.LiveFeed.UI
{
    partial class DataReceiveIndicator
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
            this.lblmassage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblmassage
            // 
            this.lblmassage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblmassage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmassage.Location = new System.Drawing.Point(0, 0);
            this.lblmassage.Name = "lblmassage";
            this.lblmassage.Size = new System.Drawing.Size(66, 19);
            this.lblmassage.TabIndex = 0;
            this.lblmassage.Text = "OK";
            this.lblmassage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataReceiveIndicator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lime;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.lblmassage);
            this.Name = "DataReceiveIndicator";
            this.Size = new System.Drawing.Size(66, 19);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lblmassage;

    }
}
