using Nirvana.Admin.PositionManagement.Controls;
namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class MapSymbol
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSymbol));
            this.ctrlMapSymbol1 = new Nirvana.Admin.PositionManagement.Controls.CtrlMapSymbol();
            this.SuspendLayout();
            // 
            // ctrlMapSymbol1
            // 
            this.ctrlMapSymbol1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlMapSymbol1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlMapSymbol1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlMapSymbol1.Location = new System.Drawing.Point(0, 0);
            this.ctrlMapSymbol1.Name = "ctrlMapSymbol1";
            this.ctrlMapSymbol1.Size = new System.Drawing.Size(340, 254);
            this.ctrlMapSymbol1.TabIndex = 0;
            // 
            // MapSymbol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(340, 254);
            this.Controls.Add(this.ctrlMapSymbol1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MapSymbol";
            this.Text = "MapSymbol";
            this.ResumeLayout(false);

        }

        #endregion

        private CtrlMapSymbol ctrlMapSymbol1;
    }
}