using Prana.Global;
namespace Prana.UDATool
{
    partial class UDAForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.usrControlUDA1 = new Prana.UDATool.UsrControlUDA();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.usrCtrlSymbolUDAData1 = new Prana.UDATool.UsrCtrlSymbolUDAData();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.usrControlUDA1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(828, 503);
            // 
            // usrControlUDA1
            // 
            this.usrControlUDA1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usrControlUDA1.Location = new System.Drawing.Point(0, 0);
            this.usrControlUDA1.Name = "usrControlUDA1";
            this.usrControlUDA1.Size = new System.Drawing.Size(828, 503);
            this.usrControlUDA1.TabIndex = 0;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.usrCtrlSymbolUDAData1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(828, 503);
            // 
            // usrCtrlSymbolUDAData1
            // 
            this.usrCtrlSymbolUDAData1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usrCtrlSymbolUDAData1.IsChanged = false;
            this.usrCtrlSymbolUDAData1.Location = new System.Drawing.Point(0, 0);
            this.usrCtrlSymbolUDAData1.Name = "usrCtrlSymbolUDAData1";
            this.usrCtrlSymbolUDAData1.Size = new System.Drawing.Size(828, 503);
            this.usrCtrlSymbolUDAData1.TabIndex = 0;
           
            // 
            // ultraTabControl1
            // 
            appearance1.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance1.BackColor2 = System.Drawing.Color.White;
            this.ultraTabControl1.Appearance = appearance1;
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(832, 529);
            this.ultraTabControl1.TabIndex = 6;
            ultraTab1.Key = "UDA";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "UDA";
            ultraTab2.Key = "UDASymbolData";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "UDA Symbol Data";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.ultraTabControl1.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(828, 503);
            // 
            // UDAForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(832, 529);
            this.Controls.Add(this.ultraTabControl1);
            this.Name = "UDAForm";
            this.Text = "User Defined Attributes (UDA)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UDAForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        
        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private UsrControlUDA usrControlUDA1;
        private UsrCtrlSymbolUDAData usrCtrlSymbolUDAData1;
    }
}

