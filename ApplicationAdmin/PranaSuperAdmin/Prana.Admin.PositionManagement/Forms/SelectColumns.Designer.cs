using Nirvana.Admin.PositionManagement.Controls;
namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class SelectColumns
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectColumns));
            this.ctrlSetupColumns1 = new Nirvana.Admin.PositionManagement.Controls.CtrlSelectColumns();
            this.SuspendLayout();
            // 
            // ctrlSetupColumns1
            // 
            this.ctrlSetupColumns1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlSetupColumns1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSetupColumns1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlSetupColumns1.IsInitialized = false;
            this.ctrlSetupColumns1.Location = new System.Drawing.Point(0, 0);
            this.ctrlSetupColumns1.Name = "ctrlSetupColumns1";
            this.ctrlSetupColumns1.Size = new System.Drawing.Size(436, 257);
            this.ctrlSetupColumns1.TabIndex = 0;
            // 
            // SelectColumns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(436, 257);
            this.Controls.Add(this.ctrlSetupColumns1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectColumns";
            this.Text = "SetupColumns";
            this.ResumeLayout(false);

        }

        #endregion

        private CtrlSelectColumns ctrlSetupColumns1;
    }
}