namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class EnterTransactions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnterTransactions));
            this.ctrlEnterTransactions1 = new Nirvana.Admin.PositionManagement.Controls.CtrlEnterCashTransactions();
            this.SuspendLayout();
            // 
            // ctrlEnterTransactions1
            // 
            this.ctrlEnterTransactions1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlEnterTransactions1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlEnterTransactions1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlEnterTransactions1.IsInitialized = false;
            this.ctrlEnterTransactions1.Location = new System.Drawing.Point(0, 0);
            this.ctrlEnterTransactions1.Name = "ctrlEnterTransactions1";
            this.ctrlEnterTransactions1.Size = new System.Drawing.Size(941, 320);
            this.ctrlEnterTransactions1.TabIndex = 0;
            // 
            // EnterTransactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(941, 320);
            this.Controls.Add(this.ctrlEnterTransactions1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnterTransactions";
            this.Text = "Enter Transactions";
            this.ResumeLayout(false);

        }

        #endregion

        private Nirvana.Admin.PositionManagement.Controls.CtrlEnterCashTransactions ctrlEnterTransactions1;

    }
}