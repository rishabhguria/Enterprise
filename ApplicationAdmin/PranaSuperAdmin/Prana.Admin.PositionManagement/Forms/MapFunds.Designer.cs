namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class MapFunds
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapFunds));
            this.ctrlMapFunds1 = new Nirvana.Admin.PositionManagement.Controls.CtrlMapFunds();
            this.SuspendLayout();
            // 
            // ctrlMapFunds1
            // 
            this.ctrlMapFunds1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlMapFunds1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlMapFunds1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlMapFunds1.IsInitialized = false;
            this.ctrlMapFunds1.Location = new System.Drawing.Point(0, 0);
            this.ctrlMapFunds1.Name = "ctrlMapFunds1";
            this.ctrlMapFunds1.Size = new System.Drawing.Size(331, 301);
            this.ctrlMapFunds1.TabIndex = 0;
            // 
            // MapFunds
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 301);
            this.Controls.Add(this.ctrlMapFunds1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MapFunds";
            this.Text = "MapFunds";
            this.ResumeLayout(false);

        }

        #endregion

        private Nirvana.Admin.PositionManagement.Controls.CtrlMapFunds ctrlMapFunds1;
    }
}