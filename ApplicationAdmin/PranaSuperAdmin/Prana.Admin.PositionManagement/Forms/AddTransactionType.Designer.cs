namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class AddTransactionType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTransactionType));
            this.ctrlAddTransactionType1 = new Nirvana.Admin.PositionManagement.Controls.CtrlAddTransactionType();
            this.SuspendLayout();
            // 
            // ctrlAddTransactionType1
            // 
            this.ctrlAddTransactionType1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlAddTransactionType1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlAddTransactionType1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlAddTransactionType1.Location = new System.Drawing.Point(0, 0);
            this.ctrlAddTransactionType1.Name = "ctrlAddTransactionType1";
            this.ctrlAddTransactionType1.Size = new System.Drawing.Size(260, 83);
            this.ctrlAddTransactionType1.TabIndex = 0;
            // 
            // AddTransactionType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(260, 83);
            this.Controls.Add(this.ctrlAddTransactionType1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddTransactionType";
            this.Text = "Add Transaction Type";
            this.ResumeLayout(false);

        }

        #endregion

        private Nirvana.Admin.PositionManagement.Controls.CtrlAddTransactionType ctrlAddTransactionType1;

    }
}