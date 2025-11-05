namespace Prana.PM.Client.UI.Forms
{
    partial class Split
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
            this.ctrlSplit1 = new Prana.PM.Client.UI.Controls.CtrlSplit();
            this.SuspendLayout();
            // 
            // ctrlSplit1
            // 
            this.ctrlSplit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSplit1.Location = new System.Drawing.Point(0, 0);
            this.ctrlSplit1.Name = "ctrlSplit1";
            this.ctrlSplit1.Size = new System.Drawing.Size(479, 131);
            this.ctrlSplit1.TabIndex = 0;
            // 
            // Split
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 131);
            this.Controls.Add(this.ctrlSplit1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(487, 165);
            this.MinimizeBox = false;
            this.Name = "Split";
            this.Text = "Split";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Split_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private Prana.PM.Client.UI.Controls.CtrlSplit ctrlSplit1;

    }
}