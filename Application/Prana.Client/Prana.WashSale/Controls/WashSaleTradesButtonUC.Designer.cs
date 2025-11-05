namespace Prana.WashSale.Controls
{
    partial class WashSaleTradesButtonUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WashSaleTradesButtonUC));
            this.exportToExcel = new Infragistics.Win.Misc.UltraButton();
            this.uploadImage = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.SaveImage = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblUpload = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraSparkline1 = new Infragistics.Win.DataVisualization.UltraSparkline();
            this.ultraSparkline2 = new Infragistics.Win.DataVisualization.UltraSparkline();
            this.ultraSparkline3 = new Infragistics.Win.DataVisualization.UltraSparkline();
            this.SuspendLayout();
            // 
            // exportToExcel
            // 
            this.exportToExcel.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(156)))), ((int)(((byte)(46)))));
            this.exportToExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportToExcel.ForeColor = System.Drawing.Color.White;
            this.exportToExcel.Location = new System.Drawing.Point(32, 22);
            this.exportToExcel.Name = "exportToExcel";
            this.exportToExcel.Size = new System.Drawing.Size(111, 30);
            this.exportToExcel.TabIndex = 0;
            this.exportToExcel.Text = "Export to Excel";
            this.exportToExcel.UseAppStyling = false;
            this.exportToExcel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.exportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // uploadImage
            // 
            this.uploadImage.BorderShadowColor = System.Drawing.Color.Empty;
            this.uploadImage.Image = ((object)(resources.GetObject("uploadImage.Image")));
            this.uploadImage.Location = new System.Drawing.Point(203, 22);
            this.uploadImage.Name = "uploadImage";
            this.uploadImage.Size = new System.Drawing.Size(40, 32);
            this.uploadImage.TabIndex = 1;
            this.uploadImage.Click += new System.EventHandler(this.UploadDataFromExcel);
            // 
            // SaveImage
            // 
            this.SaveImage.BorderShadowColor = System.Drawing.Color.Empty;
            this.SaveImage.Image = ((object)(resources.GetObject("SaveImage.Image")));
            this.SaveImage.Location = new System.Drawing.Point(268, 22);
            this.SaveImage.Name = "SaveImage";
            this.SaveImage.Size = new System.Drawing.Size(40, 32);
            this.SaveImage.TabIndex = 2;
            this.SaveImage.Click += new System.EventHandler(this.SaveWashSaleData);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(68, 85);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(49, 18);
            this.ultraLabel1.TabIndex = 3;
            this.ultraLabel1.Text = "Export";
            // 
            // lblUpload
            // 
            this.lblUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblUpload.Location = new System.Drawing.Point(203, 60);
            this.lblUpload.Name = "lblUpload";
            this.lblUpload.Size = new System.Drawing.Size(49, 16);
            this.lblUpload.TabIndex = 4;
            this.lblUpload.Text = "Upload";
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(232, 85);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(52, 23);
            this.ultraLabel3.TabIndex = 5;
            this.ultraLabel3.Text = "Actions";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ultraLabel4.Location = new System.Drawing.Point(274, 60);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(37, 16);
            this.ultraLabel4.TabIndex = 6;
            this.ultraLabel4.Text = "Save";
            // 
            // ultraSparkline1
            // 
            this.ultraSparkline1.BackColor = System.Drawing.Color.Black;
            this.ultraSparkline1.LineThickness = 2D;
            this.ultraSparkline1.Location = new System.Drawing.Point(173, 0);
            this.ultraSparkline1.Name = "ultraSparkline1";
            this.ultraSparkline1.Size = new System.Drawing.Size(2, 106);
            this.ultraSparkline1.TabIndex = 7;
            // 
            // ultraSparkline2
            // 
            this.ultraSparkline2.BackColor = System.Drawing.Color.Black;
            this.ultraSparkline2.LineThickness = 2D;
            this.ultraSparkline2.Location = new System.Drawing.Point(341, 0);
            this.ultraSparkline2.Name = "ultraSparkline2";
            this.ultraSparkline2.Size = new System.Drawing.Size(2, 106);
            this.ultraSparkline2.TabIndex = 8;
            // 
            // ultraSparkline3
            // 
            this.ultraSparkline3.BackColor = System.Drawing.Color.Black;
            this.ultraSparkline3.LineThickness = 2D;
            this.ultraSparkline3.Location = new System.Drawing.Point(2, 0);
            this.ultraSparkline3.Name = "ultraSparkline3";
            this.ultraSparkline3.Size = new System.Drawing.Size(2, 106);
            this.ultraSparkline3.TabIndex = 9;
            // 
            // WashSaleTradesButtonUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraSparkline3);
            this.Controls.Add(this.ultraSparkline2);
            this.Controls.Add(this.ultraSparkline1);
            this.Controls.Add(this.ultraLabel4);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.lblUpload);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.SaveImage);
            this.Controls.Add(this.uploadImage);
            this.Controls.Add(this.exportToExcel);
            this.Name = "WashSaleTradesButtonUC";
            this.Size = new System.Drawing.Size(424, 106);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton exportToExcel;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox uploadImage;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox SaveImage;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblUpload;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.DataVisualization.UltraSparkline ultraSparkline1;
        private Infragistics.Win.DataVisualization.UltraSparkline ultraSparkline2;
        private Infragistics.Win.DataVisualization.UltraSparkline ultraSparkline3;
    }
}
