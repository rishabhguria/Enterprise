namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class CreateCustomViews
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
            this.ctrlCustomView1 = new Nirvana.Admin.PositionManagement.Controls.CtrlCustomView();
            this.SuspendLayout();
            // 
            // ctrlCustomView1
            // 
            this.ctrlCustomView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCustomView1.Location = new System.Drawing.Point(0, 0);
            this.ctrlCustomView1.Name = "ctrlCustomView1";
            this.ctrlCustomView1.Size = new System.Drawing.Size(423, 206);
            this.ctrlCustomView1.TabIndex = 0;
            // 
            // CreateCustomViews
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 206);
            this.Controls.Add(this.ctrlCustomView1);
            this.Name = "CreateCustomViews";
            this.Text = "Custom View";
            this.ResumeLayout(false);

        }

        #endregion

        private Nirvana.Admin.PositionManagement.Controls.CtrlCustomView ctrlCustomView1;
    }
}