using Prana.Global;
namespace Prana.UDATool
{
    partial class UsrControlUDA
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnScreenShot = SnapShotManager.GetInstance().ultraButton;
            this.btnExcelExport = new Infragistics.Win.Misc.UltraButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.udaAssetClass = new Prana.UDATool.UDAControl();
            this.udaSecurityType = new Prana.UDATool.UDAControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.udaSector = new Prana.UDATool.UDAControl();
            this.udaSubSector = new Prana.UDATool.UDAControl();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.udaCountry = new Prana.UDATool.UDAControl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance4.BackColor = System.Drawing.Color.AliceBlue;
            appearance4.BackColor2 = System.Drawing.Color.SteelBlue;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.btnSave.Appearance = appearance4;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(305, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.ultraButton1_Click);
            //
            // btnScreenShot
            //
            this.btnScreenShot.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnScreenShot.Appearance = appearance4;
            this.btnScreenShot.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScreenShot.Location = new System.Drawing.Point(450, 3);
            this.btnScreenShot.Name = "btnScreenShot";
            this.btnScreenShot.Size = new System.Drawing.Size(65, 23);
            this.btnScreenShot.Click += new System.EventHandler(this.btnScreenShot_Click);
            // 
            // btnExcelExport
            // 
            this.btnExcelExport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance3.BackColor = System.Drawing.Color.AliceBlue;
            appearance3.BackColor2 = System.Drawing.Color.SteelBlue;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.btnExcelExport.Appearance = appearance3;
            this.btnExcelExport.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcelExport.Location = new System.Drawing.Point(378, 3);
            this.btnExcelExport.Name = "btnExcelExport";
            this.btnExcelExport.Size = new System.Drawing.Size(65, 23);
            this.btnExcelExport.TabIndex = 5;
            this.btnExcelExport.Text = "Export";
            this.btnExcelExport.Click += new System.EventHandler(this.btnExcelExport_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(589, 469);
            this.splitContainer1.SplitterDistance = 302;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.udaAssetClass);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.udaSecurityType);
            this.splitContainer3.Size = new System.Drawing.Size(302, 469);
            this.splitContainer3.SplitterDistance = 151;
            this.splitContainer3.TabIndex = 8;
            // 
            // udaAssetClass
            // 
            this.udaAssetClass.AutoScroll = true;
            this.udaAssetClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udaAssetClass.IsChanged = false;
            this.udaAssetClass.Location = new System.Drawing.Point(0, 0);
            this.udaAssetClass.Name = "udaAssetClass";
            this.udaAssetClass.Size = new System.Drawing.Size(151, 469);
            this.udaAssetClass.TabIndex = 0;
            // 
            // udaSecurityType
            // 
            this.udaSecurityType.AutoScroll = true;
            this.udaSecurityType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udaSecurityType.IsChanged = false;
            this.udaSecurityType.Location = new System.Drawing.Point(0, 0);
            this.udaSecurityType.Name = "udaSecurityType";
            this.udaSecurityType.Size = new System.Drawing.Size(147, 469);
            this.udaSecurityType.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.udaSector);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.udaSubSector);
            this.splitContainer2.Size = new System.Drawing.Size(283, 469);
            this.splitContainer2.SplitterDistance = 146;
            this.splitContainer2.TabIndex = 7;
            // 
            // udaSector
            // 
            this.udaSector.AutoScroll = true;
            this.udaSector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udaSector.IsChanged = false;
            this.udaSector.Location = new System.Drawing.Point(0, 0);
            this.udaSector.Name = "udaSector";
            this.udaSector.Size = new System.Drawing.Size(146, 469);
            this.udaSector.TabIndex = 2;
            // 
            // udaSubSector
            // 
            this.udaSubSector.AutoScroll = true;
            this.udaSubSector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udaSubSector.IsChanged = false;
            this.udaSubSector.Location = new System.Drawing.Point(0, 0);
            this.udaSubSector.Name = "udaSubSector";
            this.udaSubSector.Size = new System.Drawing.Size(133, 469);
            this.udaSubSector.TabIndex = 3;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer4.Location = new System.Drawing.Point(0, 32);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.udaCountry);
            this.splitContainer4.Size = new System.Drawing.Size(748, 471);
            this.splitContainer4.SplitterDistance = 591;
            this.splitContainer4.TabIndex = 7;
            // 
            // udaCountry
            // 
            this.udaCountry.AutoScroll = true;
            this.udaCountry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udaCountry.IsChanged = false;
            this.udaCountry.Location = new System.Drawing.Point(0, 0);
            this.udaCountry.Name = "udaCountry";
            this.udaCountry.Size = new System.Drawing.Size(151, 469);
            this.udaCountry.TabIndex = 4;
            // 
            // UsrControlUDA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer4);
            this.Controls.Add(this.btnExcelExport);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnScreenShot);
            this.Name = "UsrControlUDA";
            this.Size = new System.Drawing.Size(748, 503);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UDAControl udaAssetClass;
        private UDAControl udaSecurityType;
        private UDAControl udaSector;
        private UDAControl udaSubSector;
        private UDAControl udaCountry;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnScreenShot;
        private Infragistics.Win.Misc.UltraButton btnExcelExport;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
    }
}
