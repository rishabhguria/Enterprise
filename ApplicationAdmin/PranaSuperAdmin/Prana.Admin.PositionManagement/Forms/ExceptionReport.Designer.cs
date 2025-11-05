namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class ExceptionReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionReport));
            this.ctrlExceptionReport1 = new Nirvana.Admin.PositionManagement.Controls.CtrlExceptionReport();
            this.SuspendLayout();
            // 
            // ctrlExceptionReport1
            // 
            this.ctrlExceptionReport1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlExceptionReport1.DataSourceNameIDValue = null;
            this.ctrlExceptionReport1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlExceptionReport1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlExceptionReport1.IsInitialized = false;
            this.ctrlExceptionReport1.Location = new System.Drawing.Point(0, 0);
            this.ctrlExceptionReport1.Name = "ctrlExceptionReport1";
            this.ctrlExceptionReport1.Size = new System.Drawing.Size(818, 523);
            this.ctrlExceptionReport1.TabIndex = 0;
            // 
            // ExceptionReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 523);
            this.Controls.Add(this.ctrlExceptionReport1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExceptionReport";
            this.Text = "Exception Report";
            this.ResumeLayout(false);

        }

        #endregion

        private Nirvana.Admin.PositionManagement.Controls.CtrlExceptionReport ctrlExceptionReport1;
    }
}