namespace Prana.HeatMapControls
{
    partial class HeatMapControl
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraPanel = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFlowLayoutManager1 = new Infragistics.Win.Misc.UltraFlowLayoutManager(this.components);
            this.ultraPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            this.ultraPanel.Appearance = appearance1;
            this.ultraPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel.Name = "ultraPanel";
            this.ultraPanel.Size = new System.Drawing.Size(150, 150);
            this.ultraPanel.TabIndex = 0;
            this.ultraPanel.ClickClient += new System.EventHandler(this.tile_Click);
            // 
            // ultraFlowLayoutManager1
            // 
            this.ultraFlowLayoutManager1.ContainerControl = this.ultraPanel.ClientArea;
            this.ultraFlowLayoutManager1.HorizontalGap = 0;
            this.ultraFlowLayoutManager1.VerticalGap = 0;
            // 
            // HeatMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel);
            this.Name = "HeatMapControl";
            this.ultraPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel;
        private Infragistics.Win.Misc.UltraFlowLayoutManager ultraFlowLayoutManager1;
    }
}
