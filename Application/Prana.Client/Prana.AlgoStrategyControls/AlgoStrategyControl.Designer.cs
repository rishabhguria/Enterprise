namespace Prana.AlgoStrategyControls
{
    partial class AlgoStrategyControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSend = new Infragistics.Win.Misc.UltraButton();
            //this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
         
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(377, 120);
            this.panel1.MinimumSize = new System.Drawing.Size(377, 120);
            this.panel1.TabIndex = 0;
            this.panel1.AutoSize = true;
            // 
            // shapeContainer1
            // 
            //this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            //this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            //this.shapeContainer1.Name = "shapeContainer1";
            //this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            //this.lineShape1});
            //this.shapeContainer1.Size = new System.Drawing.Size(284, 261);
            //this.shapeContainer1.TabIndex = 0;
            //this.shapeContainer1.TabStop = false;
             
          
            // btnSend
            // 
            this.btnSend.BackColorInternal = System.Drawing.Color.Transparent;
            this.btnSend.Location = new System.Drawing.Point(panel1.Size.Width - 119, panel1.Size.Height - 29);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(119, 29);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "&Send";
            this.btnSend.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // AlgoStrategyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.panel1);
            this.Name = "AlgoStrategyControl";
            this.Size = new System.Drawing.Size(377, 152);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraButton btnSend;
        //private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
       
    }
}
