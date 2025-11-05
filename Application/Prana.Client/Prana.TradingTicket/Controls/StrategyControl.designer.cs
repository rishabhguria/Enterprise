namespace Prana.TradingTicket
{
    partial class StrategyControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrategyControl));
            this.cmbbxStrategy = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbbxStrategy
            // 
            this.cmbbxStrategy.Location = new System.Drawing.Point(5, 5);
            this.cmbbxStrategy.Name = "cmbbxStrategy";
            this.cmbbxStrategy.NullText = "Algo Type";
            this.cmbbxStrategy.Size = new System.Drawing.Size(180, 22);
            this.cmbbxStrategy.TabIndex = 8;
            this.cmbbxStrategy.ValueChanged += new System.EventHandler(this.cmbbxStrategy_ValueChanged);
            // 
            // pictureBox1
            // 
            //this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            //this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            //this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(5, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(180, 40);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = true;
            // 
            // StrategyControl
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.cmbbxStrategy);
            this.Controls.Add(this.pictureBox1);
            this.Name = "StrategyControl";
            this.Size = new System.Drawing.Size(200, 152);
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxStrategy;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
