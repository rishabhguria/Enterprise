namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class AddUploadClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddUploadClient));
            this.ctrlAddUploadClient1 = new Nirvana.Admin.PositionManagement.Controls.CtrlAddUploadClient();
            this.SuspendLayout();
            // 
            // ctrlAddUploadClient1
            // 
            this.ctrlAddUploadClient1.Location = new System.Drawing.Point(11, 1);
            this.ctrlAddUploadClient1.Name = "ctrlAddUploadClient1";
            this.ctrlAddUploadClient1.Size = new System.Drawing.Size(271, 91);
            this.ctrlAddUploadClient1.TabIndex = 0;
            // 
            // AddUploadClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 94);
            this.Controls.Add(this.ctrlAddUploadClient1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddUploadClient";
            this.Text = "Add Upload Client";
            this.ResumeLayout(false);

        }

        #endregion

        private Nirvana.Admin.PositionManagement.Controls.CtrlAddUploadClient ctrlAddUploadClient1;

        
    }
}