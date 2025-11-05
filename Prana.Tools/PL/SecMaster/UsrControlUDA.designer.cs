using Prana.Global;
namespace Prana.Tools
{
    partial class UserControlUDA
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
            //Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            //Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlUDA));
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnExcelExport = new Infragistics.Win.Misc.UltraButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.udaAssetClass = new Prana.Tools.UDAControl();
            this.udaSecurityType = new Prana.Tools.UDAControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.udaSector = new Prana.Tools.UDAControl();
            this.udaSubSector = new Prana.Tools.UDAControl();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.udaCountry = new Prana.Tools.UDAControl();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(305, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // btnExcelExport
            // 
            this.btnExcelExport.Anchor = System.Windows.Forms.AnchorStyles.Top;
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
            //this.udaAssetClass.AddedUDACollection = ((Prana.BusinessObjects.SecurityMasterBusinessObjects.UDACollection)(resources.GetObject("udaAssetClass.AddedUDACollection")));
            this.udaAssetClass.AutoScroll = true;
            this.udaAssetClass.Collection = ((Prana.BusinessObjects.SecurityMasterBusinessObjects.UDACollection)(resources.GetObject("udaAssetClass.Collection")));
            this.udaAssetClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udaAssetClass.IsChanged = false;
            this.udaAssetClass.Location = new System.Drawing.Point(0, 0);
            this.udaAssetClass.Name = "udaAssetClass";
            this.udaAssetClass.Size = new System.Drawing.Size(151, 469);
            this.udaAssetClass.SP_DeleteName = "";
            this.udaAssetClass.SP_InsertName = "";
            this.udaAssetClass.TabIndex = 0;
            this.udaAssetClass.UDAsInUse = ((System.Collections.Generic.List<int>)(resources.GetObject("udaAssetClass.UDAsInUse")));
            this.udaAssetClass.UDAType = "";
            // 
            // udaSecurityType
            // 
            //this.udaSecurityType.AddedUDACollection = ((Prana.BusinessObjects.SecurityMasterBusinessObjects.UDACollection)(resources.GetObject("udaSecurityType.AddedUDACollection")));
            this.udaSecurityType.AutoScroll = true;
            this.udaSecurityType.Collection = ((Prana.BusinessObjects.SecurityMasterBusinessObjects.UDACollection)(resources.GetObject("udaSecurityType.Collection")));
            this.udaSecurityType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udaSecurityType.IsChanged = false;
            this.udaSecurityType.Location = new System.Drawing.Point(0, 0);
            this.udaSecurityType.Name = "udaSecurityType";
            this.udaSecurityType.Size = new System.Drawing.Size(147, 469);
            this.udaSecurityType.SP_DeleteName = "";
            this.udaSecurityType.SP_InsertName = "";
            this.udaSecurityType.TabIndex = 1;
            this.udaSecurityType.UDAsInUse = ((System.Collections.Generic.List<int>)(resources.GetObject("udaSecurityType.UDAsInUse")));
            this.udaSecurityType.UDAType = "";
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
            //this.udaSector.AddedUDACollection = ((Prana.BusinessObjects.SecurityMasterBusinessObjects.UDACollection)(resources.GetObject("udaSector.AddedUDACollection")));
            this.udaSector.AutoScroll = true;
            this.udaSector.Collection = ((Prana.BusinessObjects.SecurityMasterBusinessObjects.UDACollection)(resources.GetObject("udaSector.Collection")));
            this.udaSector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udaSector.IsChanged = false;
            this.udaSector.Location = new System.Drawing.Point(0, 0);
            this.udaSector.Name = "udaSector";
            this.udaSector.Size = new System.Drawing.Size(146, 469);
            this.udaSector.SP_DeleteName = "";
            this.udaSector.SP_InsertName = "";
            this.udaSector.TabIndex = 2;
            this.udaSector.UDAsInUse = ((System.Collections.Generic.List<int>)(resources.GetObject("udaSector.UDAsInUse")));
            this.udaSector.UDAType = "";
            // 
            // udaSubSector
            // 
            //this.udaSubSector.AddedUDACollection = ((Prana.BusinessObjects.SecurityMasterBusinessObjects.UDACollection)(resources.GetObject("udaSubSector.AddedUDACollection")));
            this.udaSubSector.AutoScroll = true;
            this.udaSubSector.Collection = ((Prana.BusinessObjects.SecurityMasterBusinessObjects.UDACollection)(resources.GetObject("udaSubSector.Collection")));
            this.udaSubSector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udaSubSector.IsChanged = false;
            this.udaSubSector.Location = new System.Drawing.Point(0, 0);
            this.udaSubSector.Name = "udaSubSector";
            this.udaSubSector.Size = new System.Drawing.Size(133, 469);
            this.udaSubSector.SP_DeleteName = "";
            this.udaSubSector.SP_InsertName = "";
            this.udaSubSector.TabIndex = 3;
            this.udaSubSector.UDAsInUse = ((System.Collections.Generic.List<int>)(resources.GetObject("udaSubSector.UDAsInUse")));
            this.udaSubSector.UDAType = "";
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
            //this.udaCountry.AddedUDACollection = ((Prana.BusinessObjects.SecurityMasterBusinessObjects.UDACollection)(resources.GetObject("udaCountry.AddedUDACollection")));
            this.udaCountry.AutoScroll = true;
            this.udaCountry.Collection = ((Prana.BusinessObjects.SecurityMasterBusinessObjects.UDACollection)(resources.GetObject("udaCountry.Collection")));
            this.udaCountry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udaCountry.IsChanged = false;
            this.udaCountry.Location = new System.Drawing.Point(0, 0);
            this.udaCountry.Name = "udaCountry";
            this.udaCountry.Size = new System.Drawing.Size(151, 469);
            this.udaCountry.SP_DeleteName = "";
            this.udaCountry.SP_InsertName = "";
            this.udaCountry.TabIndex = 4;
            this.udaCountry.UDAsInUse = ((System.Collections.Generic.List<int>)(resources.GetObject("udaCountry.UDAsInUse")));
            this.udaCountry.UDAType = "";
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
            this.Load += new System.EventHandler(this.UsrControlUDA_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
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
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}
