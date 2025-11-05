using Prana.PM.Admin.UI.Controls;
using Prana.PM.BLL;
//using Prana.PM.Common;

namespace Prana.PM.Admin.UI.Forms
{
    partial class MapAUEC
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

            this.ctrlMapAUEC1 = new Prana.PM.Admin.UI.Controls.CtrlMapAUEC();
            this.SuspendLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapAUEC));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            // 
            // ctrlMapAUEC1
            // 
            //this.ctrlMapAUEC1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            //this.ctrlMapAUEC1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ctrlMapAUEC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlMapAUEC1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlMapAUEC1.IsInitialized = false;
            this.ctrlMapAUEC1.Location = new System.Drawing.Point(2, 2);
            this.ctrlMapAUEC1.Name = "ctrlMapAUEC1";
            //this.ctrlMapAUEC1.Size = new System.Drawing.Size(394, 352);
            this.ctrlMapAUEC1.Size = new System.Drawing.Size(418, 380);
            this.ctrlMapAUEC1.TabIndex = 0;
            this.ctrlMapAUEC1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // MapAUEC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(420, 380);
            this.Controls.Add(this.ctrlMapAUEC1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MinimumSize = new System.Drawing.Size(420, 410);
            this.Name = "MapAUEC";
            this.Text = "Map AUEC";
            this.ResumeLayout(false);

        }

        #endregion

        private CtrlMapAUEC ctrlMapAUEC1;





    }
}