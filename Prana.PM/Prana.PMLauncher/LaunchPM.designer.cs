namespace Prana.PMLauncher
{
    partial class LaunchPM 
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
            this.btnLaunchPM = new System.Windows.Forms.Button();
            this.btnTestGrid = new System.Windows.Forms.Button();
            this.btnNewPMMain = new Infragistics.Win.Misc.UltraButton();
            this.button1 = new System.Windows.Forms.Button();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // btnLaunchPM
            // 
            this.btnLaunchPM.Location = new System.Drawing.Point(74, 70);
            this.btnLaunchPM.Name = "btnLaunchPM";
            this.btnLaunchPM.Size = new System.Drawing.Size(223, 55);
            this.btnLaunchPM.TabIndex = 0;
            this.btnLaunchPM.Text = "Launch PM Admin";
            this.btnLaunchPM.UseVisualStyleBackColor = true;
            this.btnLaunchPM.Click += new System.EventHandler(this.btnLaunchPM_Click);
            // 
            // btnTestGrid
            // 
            this.btnTestGrid.Location = new System.Drawing.Point(138, 12);
            this.btnTestGrid.Name = "btnTestGrid";
            this.btnTestGrid.Size = new System.Drawing.Size(99, 23);
            this.btnTestGrid.TabIndex = 0;
            this.btnTestGrid.Text = "Launch GridTest ";
            this.btnTestGrid.UseVisualStyleBackColor = true;
            this.btnTestGrid.Click += new System.EventHandler(this.btnTestGrid_Click);
            // 
            // btnNewPMMain
            // 
            this.btnNewPMMain.Location = new System.Drawing.Point(74, 168);
            this.btnNewPMMain.Name = "btnNewPMMain";
            this.btnNewPMMain.Size = new System.Drawing.Size(223, 55);
            this.btnNewPMMain.TabIndex = 1;
            this.btnNewPMMain.Text = "Launch PM Client";
            this.btnNewPMMain.Click += new System.EventHandler(this.btnNewPMMain_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(121, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Launch DataError Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Location = new System.Drawing.Point(283, 12);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(75, 23);
            this.ultraButton1.TabIndex = 2;
            this.ultraButton1.Text = "Drag\'n\'Drop";
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // LaunchPM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 246);
            this.Controls.Add(this.ultraButton1);
            this.Controls.Add(this.btnNewPMMain);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnTestGrid);
            this.Controls.Add(this.btnLaunchPM);
            this.Name = "LaunchPM";
            this.Text = "LaunchPM";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLaunchPM;
        private System.Windows.Forms.Button btnTestGrid;
        private Infragistics.Win.Misc.UltraButton btnNewPMMain;
        private System.Windows.Forms.Button button1;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        
    }
}