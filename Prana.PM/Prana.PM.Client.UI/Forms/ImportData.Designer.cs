namespace Prana.PM.Client.UI.Forms
{
    partial class ImportData
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
            DisposeProxies();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
			if (ctrlRunDownload1 != null)
            {
                ctrlRunDownload1 = null;
            }
            if (ctrlImportPrefs1 != null)
            {
                ctrlImportPrefs1 = null;
            }
            if (ctrlReImport1 != null)
            {
                ctrlReImport1 = null;
            }
            if (ctrlProgress1 != null)
            {
                ctrlProgress1 = null;
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportData));
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlRunDownload1 = new Prana.PM.Client.UI.CtrlRunDownload();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlReImport1 = new Prana.PM.Client.UI.ctrlReImport();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlImportPrefs1 = new Prana.PM.Client.UI.CtrlImportPreferences();
            this.ctrlProgress1 = new Prana.PM.Client.UI.ctrlProgress();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tcCreateandImportPositions = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ImportData_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ImportData_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ImportData_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ImportData_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcCreateandImportPositions)).BeginInit();
            this.tcCreateandImportPositions.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ctrlRunDownload1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(895, 280);
            // 
            // ctrlRunDownload1
            // 
            this.ctrlRunDownload1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlRunDownload1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlRunDownload1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlRunDownload1.IsGroupSaved = false;
            this.ctrlRunDownload1.Location = new System.Drawing.Point(0, 0);
            this.ctrlRunDownload1.Name = "ctrlRunDownload1";
            this.ctrlRunDownload1.Size = new System.Drawing.Size(895, 280);
            this.ctrlRunDownload1.TabIndex = 0;
            this.ctrlRunDownload1.Load += new System.EventHandler(this.ctrlRunDownload1_Load);
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ctrlReImport1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(895, 280);
            // 
            // ctrlReImport1
            // 
            this.ctrlReImport1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlReImport1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlReImport1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlReImport1.Location = new System.Drawing.Point(0, 0);
            this.ctrlReImport1.Name = "ctrlReImport1";
            this.ctrlReImport1.Size = new System.Drawing.Size(895, 280);
            this.ctrlReImport1.TabIndex = 0;
            this.ctrlReImport1.Load += new System.EventHandler(this.ctrlReImport1_Load);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.ctrlImportPrefs1);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(1, 22);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(895, 280);
            // 
            // ctrlImportPrefs1
            // 
            this.ctrlImportPrefs1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlImportPrefs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlImportPrefs1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlImportPrefs1.ImportPrefs = null;
            this.ctrlImportPrefs1.Location = new System.Drawing.Point(0, 0);
            this.ctrlImportPrefs1.Name = "ctrlImportPrefs1";
            this.ctrlImportPrefs1.Size = new System.Drawing.Size(895, 280);
            this.ctrlImportPrefs1.TabIndex = 0;
            this.ctrlImportPrefs1.Load += new System.EventHandler(this.ctrlImportPrefs1_Load);
            // 
            // ctrlProgress1
            // 
            this.ctrlProgress1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlProgress1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlProgress1.ImportingText = null;
            this.ctrlProgress1.Location = new System.Drawing.Point(0, 314);
            this.ctrlProgress1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ctrlProgress1.Name = "ctrlProgress1";
            this.ctrlProgress1.ProgressingText = null;
            this.ctrlProgress1.ProgressValue = 0;
            this.ctrlProgress1.Size = new System.Drawing.Size(895, 35);
            this.ctrlProgress1.TabIndex = 2;
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "Nirvana Help.chm";
            //
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(895, 280);
            // 
            // tcCreateandImportPositions
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance1.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tcCreateandImportPositions.ActiveTabAppearance = appearance1;
            this.tcCreateandImportPositions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance2.FontData.SizeInPoints = 10F;
            this.tcCreateandImportPositions.Appearance = appearance2;
            this.tcCreateandImportPositions.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.tcCreateandImportPositions.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tcCreateandImportPositions.Controls.Add(this.ultraTabPageControl3);
            this.tcCreateandImportPositions.Controls.Add(this.ultraTabPageControl2);
            this.tcCreateandImportPositions.Controls.Add(this.ultraTabPageControl1);
            this.tcCreateandImportPositions.Location = new System.Drawing.Point(0, 0);
            this.tcCreateandImportPositions.Name = "tcCreateandImportPositions";
            this.tcCreateandImportPositions.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tcCreateandImportPositions.Size = new System.Drawing.Size(897, 303);
            this.tcCreateandImportPositions.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tcCreateandImportPositions.TabIndex = 1;
            ultraTab2.Key = "CustomFilters";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Import Data";
            ultraTab1.Key = "ReImportData";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Re-Import Data";
            ultraTab3.Key = "Preferences";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Preferences";
            this.tcCreateandImportPositions.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab2,
            ultraTab1,
            ultraTab3});
            this.tcCreateandImportPositions.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            this.tcCreateandImportPositions.ActiveTabChanging += new Infragistics.Win.UltraWinTabControl.ActiveTabChangingEventHandler(this.tcCreateandImportPositions_ActiveTabChanging);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.tcCreateandImportPositions);
            this.ultraPanel1.ClientArea.Controls.Add(this.ctrlProgress1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraPanel1.Location = new System.Drawing.Point(4, 27);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(897, 354);
            this.ultraPanel1.TabIndex = 1;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _ImportData_UltraFormManager_Dock_Area_Left
            // 
            this._ImportData_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportData_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportData_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ImportData_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportData_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ImportData_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._ImportData_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._ImportData_UltraFormManager_Dock_Area_Left.Name = "_ImportData_UltraFormManager_Dock_Area_Left";
            this._ImportData_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 354);
            // 
            // _ImportData_UltraFormManager_Dock_Area_Right
            // 
            this._ImportData_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportData_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportData_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ImportData_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportData_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ImportData_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._ImportData_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(901, 27);
            this._ImportData_UltraFormManager_Dock_Area_Right.Name = "_ImportData_UltraFormManager_Dock_Area_Right";
            this._ImportData_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 354);
            // 
            // _ImportData_UltraFormManager_Dock_Area_Top
            // 
            this._ImportData_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportData_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportData_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ImportData_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportData_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ImportData_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ImportData_UltraFormManager_Dock_Area_Top.Name = "_ImportData_UltraFormManager_Dock_Area_Top";
            this._ImportData_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(905, 27);
            // 
            // _ImportData_UltraFormManager_Dock_Area_Bottom
            // 
            this._ImportData_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportData_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportData_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ImportData_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportData_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ImportData_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._ImportData_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 381);
            this._ImportData_UltraFormManager_Dock_Area_Bottom.Name = "_ImportData_UltraFormManager_Dock_Area_Bottom";
            this._ImportData_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(905, 4);
            // 
            // ImportData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(905, 385);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this._ImportData_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ImportData_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ImportData_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ImportData_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.helpProvider1.SetHelpKeyword(this, "ImportData.html");
            this.helpProvider1.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(905, 385);
            this.Name = "ImportData";
            this.helpProvider1.SetShowHelp(this, true);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Import Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportData_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImportData_FormClosed);
            this.Load += new System.EventHandler(this.ImportData_Load);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tcCreateandImportPositions)).EndInit();
            this.tcCreateandImportPositions.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }      

       
        #endregion
        private System.Windows.Forms.HelpProvider helpProvider1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tcCreateandImportPositions;
        private Prana.PM.Client.UI.CtrlRunDownload ctrlRunDownload1;
        private Prana.PM.Client.UI.CtrlImportPreferences ctrlImportPrefs1;
        private Prana.PM.Client.UI.ctrlReImport ctrlReImport1;
        private Prana.PM.Client.UI.ctrlProgress ctrlProgress1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportData_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportData_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportData_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportData_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
    }
}