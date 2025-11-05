using Nirvana.Admin.PositionManagement.Controls;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Properties;

namespace Nirvana.Admin.PositionManagement.Forms
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
            this.ctrlMapAUEC1 = new Nirvana.Admin.PositionManagement.Controls.CtrlMapAUEC();
            this.SuspendLayout();
            // 
            // ctrlMapAUEC1
            // 
            this.ctrlMapAUEC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlMapAUEC1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlMapAUEC1.IsInitialized = false;
            this.ctrlMapAUEC1.Location = new System.Drawing.Point(2, 2);
            this.ctrlMapAUEC1.Name = "ctrlMapAUEC1";
            this.ctrlMapAUEC1.Size = new System.Drawing.Size(394, 352);
            this.ctrlMapAUEC1.TabIndex = 0;
            // 
            // MapAUEC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(395, 353);
            this.Controls.Add(this.ctrlMapAUEC1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "MapAUEC";
            this.Text = "MapAUEC";
            this.ResumeLayout(false);

        }

        #endregion

        private CtrlMapAUEC ctrlMapAUEC1;





    }
}