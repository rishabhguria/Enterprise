namespace Prana.Tools.PL
{
    partial class DynamicUDAForm
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
            UnwireEvents();
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
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.dynamicUserControl1 = new Prana.Tools.PL.Controls.DynamicUserControl();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.dynamicUserControl1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(534, 512);
            this.ultraPanel1.TabIndex = 0;
            // 
            // dynamicUserControl1
            // 
            this.dynamicUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicUserControl1.Location = new System.Drawing.Point(0, 0);
            this.dynamicUserControl1.Name = "dynamicUserControl1";
            this.dynamicUserControl1.Size = new System.Drawing.Size(534, 512);
            this.dynamicUserControl1.TabIndex = 0;
            // 
            // DynamicUDAForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(534, 512);
            this.Controls.Add(this.ultraPanel1);
            this.Name = "DynamicUDAForm";
            this.ShowIcon = false;
            this.Text = "Dynamic User Defined Attributes (UDA)";
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Controls.DynamicUserControl dynamicUserControl1;

    }
}