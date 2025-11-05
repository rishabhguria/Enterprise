using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using Prana.PM.Admin.UI.Controls;
using Prana.PM.BLL;
//using Prana.PM.Common;


namespace Prana.PM.Admin.UI.Forms
{
    partial class PMMain
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
            if (disposing )
            {
                if(frmAddUploadClient != null)
                { 
                    frmAddUploadClient.Dispose();
                }
                if (components != null)
                {
                    components.Dispose();
                }
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab12 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab13 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab14 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab15 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab16 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab17 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PMMain));
            this.ultraTabPageControl14 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl15 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl16 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl12 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlRunUploadSetup1 = new Prana.PM.Admin.UI.Controls.CtrlRunDownloadSetup();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl17 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabDataSource = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabCompanyDetails = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage3 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabSetUpRunUpload = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage4 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabReconciliation = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage5 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl13 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabCashManagement = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage6 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.treePMMain = new Infragistics.Win.UltraWinTree.UltraTree();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.tabMainPMAdmin = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl8.SuspendLayout();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabDataSource)).BeginInit();
            this.tabDataSource.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabCompanyDetails)).BeginInit();
            this.ultraTabCompanyDetails.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabSetUpRunUpload)).BeginInit();
            this.tabSetUpRunUpload.SuspendLayout();
            this.ultraTabPageControl10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabReconciliation)).BeginInit();
            this.tabReconciliation.SuspendLayout();
            this.ultraTabPageControl13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCashManagement)).BeginInit();
            this.tabCashManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treePMMain)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabMainPMAdmin)).BeginInit();
            this.tabMainPMAdmin.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl14
            // 
            this.ultraTabPageControl14.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl14.Name = "ultraTabPageControl14";
            this.ultraTabPageControl14.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl15
            // 
            this.ultraTabPageControl15.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl15.Name = "ultraTabPageControl15";
            this.ultraTabPageControl15.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl16
            // 
            this.ultraTabPageControl16.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl16.Name = "ultraTabPageControl16";
            this.ultraTabPageControl16.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl11
            // 
            this.ultraTabPageControl11.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl11.Name = "ultraTabPageControl11";
            this.ultraTabPageControl11.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl12
            // 
            this.ultraTabPageControl12.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl12.Name = "ultraTabPageControl12";
            this.ultraTabPageControl12.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl8
            // 
            this.ultraTabPageControl8.Controls.Add(this.ctrlRunUploadSetup1);
            this.ultraTabPageControl8.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl8.Name = "ultraTabPageControl8";
            this.ultraTabPageControl8.Size = new System.Drawing.Size(651, 524);
            // 
            // ctrlRunUploadSetup1
            // 
            this.ctrlRunUploadSetup1.AutoScroll = true;
            this.ctrlRunUploadSetup1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlRunUploadSetup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlRunUploadSetup1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlRunUploadSetup1.Location = new System.Drawing.Point(0, 0);
            this.ctrlRunUploadSetup1.Name = "ctrlRunUploadSetup1";
            this.ctrlRunUploadSetup1.Size = new System.Drawing.Size(651, 524);
            this.ctrlRunUploadSetup1.TabIndex = 0;
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl7
            // 
            this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl7.Name = "ultraTabPageControl7";
            this.ultraTabPageControl7.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl17
            // 
            this.ultraTabPageControl17.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl17.Name = "ultraTabPageControl17";
            this.ultraTabPageControl17.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.tabDataSource);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(653, 545);
            // 
            // tabDataSource
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabDataSource.ActiveTabAppearance = appearance1;
            this.tabDataSource.Controls.Add(this.ultraTabSharedControlsPage2);
            this.tabDataSource.Controls.Add(this.ultraTabPageControl4);
            this.tabDataSource.Controls.Add(this.ultraTabPageControl5);
            this.tabDataSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDataSource.Location = new System.Drawing.Point(0, 0);
            this.tabDataSource.Name = "tabDataSource";
            appearance2.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            this.tabDataSource.SelectedTabAppearance = appearance2;
            this.tabDataSource.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.tabDataSource.Size = new System.Drawing.Size(653, 545);
            this.tabDataSource.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabDataSource.TabIndex = 0;
            ultraTab1.Key = "DataSourceDetails";
            ultraTab1.TabPage = this.ultraTabPageControl4;
            ultraTab1.Text = "Data Source Details";
            ultraTab2.Key = "ImportSetUp";
            ultraTab2.TabPage = this.ultraTabPageControl5;
            ultraTab2.Text = "Import Set Up";
            this.tabDataSource.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.tabDataSource.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabDataSource_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ultraTabCompanyDetails);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(653, 545);
            // 
            // ultraTabCompanyDetails
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ultraTabCompanyDetails.ActiveTabAppearance = appearance3;
            this.ultraTabCompanyDetails.Controls.Add(this.ultraTabSharedControlsPage3);
            this.ultraTabCompanyDetails.Controls.Add(this.ultraTabPageControl6);
            this.ultraTabCompanyDetails.Controls.Add(this.ultraTabPageControl7);
            this.ultraTabCompanyDetails.Controls.Add(this.ultraTabPageControl17);
            this.ultraTabCompanyDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabCompanyDetails.Location = new System.Drawing.Point(0, 0);
            this.ultraTabCompanyDetails.Name = "ultraTabCompanyDetails";
            appearance4.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            this.ultraTabCompanyDetails.SelectedTabAppearance = appearance4;
            this.ultraTabCompanyDetails.SharedControlsPage = this.ultraTabSharedControlsPage3;
            this.ultraTabCompanyDetails.Size = new System.Drawing.Size(653, 545);
            this.ultraTabCompanyDetails.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.ultraTabCompanyDetails.TabIndex = 1;
            ultraTab3.Key = "CompanyDetails";
            ultraTab3.TabPage = this.ultraTabPageControl6;
            ultraTab3.Text = "Details";
            ultraTab4.Key = "ApplicationDetails";
            ultraTab4.TabPage = this.ultraTabPageControl7;
            ultraTab4.Text = "Application Details";
            ultraTab5.Key = "MapAccounts";
            ultraTab5.TabPage = this.ultraTabPageControl17;
            ultraTab5.Text = "Map Accounts";
            this.ultraTabCompanyDetails.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab3,
            ultraTab4,
            ultraTab5});
            // 
            // ultraTabSharedControlsPage3
            // 
            this.ultraTabSharedControlsPage3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage3.Name = "ultraTabSharedControlsPage3";
            this.ultraTabSharedControlsPage3.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.tabSetUpRunUpload);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(653, 545);
            // 
            // tabSetUpRunUpload
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance5.BackColor2 = System.Drawing.Color.White;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabSetUpRunUpload.ActiveTabAppearance = appearance5;
            this.tabSetUpRunUpload.Controls.Add(this.ultraTabSharedControlsPage4);
            this.tabSetUpRunUpload.Controls.Add(this.ultraTabPageControl8);
            this.tabSetUpRunUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSetUpRunUpload.Location = new System.Drawing.Point(0, 0);
            this.tabSetUpRunUpload.Name = "tabSetUpRunUpload";
            appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            this.tabSetUpRunUpload.SelectedTabAppearance = appearance6;
            this.tabSetUpRunUpload.SharedControlsPage = this.ultraTabSharedControlsPage4;
            this.tabSetUpRunUpload.Size = new System.Drawing.Size(653, 545);
            this.tabSetUpRunUpload.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabSetUpRunUpload.TabIndex = 1;
            ultraTab6.Key = "SetUpRunUpload";
            ultraTab6.TabPage = this.ultraTabPageControl8;
            ultraTab6.Text = "Set Up";
            this.tabSetUpRunUpload.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab6});
            this.tabSetUpRunUpload.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabSetUpRunUpload_MouseClick);
            this.tabSetUpRunUpload.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabSetUpRunUpload_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage4
            // 
            this.ultraTabSharedControlsPage4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage4.Name = "ultraTabSharedControlsPage4";
            this.ultraTabSharedControlsPage4.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl10
            // 
            this.ultraTabPageControl10.Controls.Add(this.tabReconciliation);
            this.ultraTabPageControl10.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl10.Name = "ultraTabPageControl10";
            this.ultraTabPageControl10.Size = new System.Drawing.Size(653, 545);
            // 
            // tabReconciliation
            // 
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabReconciliation.ActiveTabAppearance = appearance7;
            this.tabReconciliation.Controls.Add(this.ultraTabSharedControlsPage5);
            this.tabReconciliation.Controls.Add(this.ultraTabPageControl11);
            this.tabReconciliation.Controls.Add(this.ultraTabPageControl12);
            this.tabReconciliation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReconciliation.Location = new System.Drawing.Point(0, 0);
            this.tabReconciliation.Name = "tabReconciliation";
            appearance8.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            this.tabReconciliation.SelectedTabAppearance = appearance8;
            this.tabReconciliation.SharedControlsPage = this.ultraTabSharedControlsPage5;
            this.tabReconciliation.Size = new System.Drawing.Size(653, 545);
            this.tabReconciliation.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabReconciliation.TabIndex = 1;
            ultraTab8.Key = "SetUp Recon";
            ultraTab8.TabPage = this.ultraTabPageControl11;
            ultraTab8.Text = "SetUp Recon";
            ultraTab9.Key = "Run Re-con";
            ultraTab9.TabPage = this.ultraTabPageControl12;
            ultraTab9.Text = "Run Re-con";
            this.tabReconciliation.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab8,
            ultraTab9});
            this.tabReconciliation.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabReconciliation_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage5
            // 
            this.ultraTabSharedControlsPage5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage5.Name = "ultraTabSharedControlsPage5";
            this.ultraTabSharedControlsPage5.Size = new System.Drawing.Size(651, 524);
            // 
            // ultraTabPageControl13
            // 
            this.ultraTabPageControl13.Controls.Add(this.tabCashManagement);
            this.ultraTabPageControl13.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl13.Name = "ultraTabPageControl13";
            this.ultraTabPageControl13.Size = new System.Drawing.Size(653, 545);
            // 
            // tabCashManagement
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance9.BackColor2 = System.Drawing.Color.White;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabCashManagement.ActiveTabAppearance = appearance9;
            this.tabCashManagement.Controls.Add(this.ultraTabSharedControlsPage6);
            this.tabCashManagement.Controls.Add(this.ultraTabPageControl14);
            this.tabCashManagement.Controls.Add(this.ultraTabPageControl15);
            this.tabCashManagement.Controls.Add(this.ultraTabPageControl16);
            this.tabCashManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCashManagement.Location = new System.Drawing.Point(0, 0);
            this.tabCashManagement.Name = "tabCashManagement";
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            this.tabCashManagement.SelectedTabAppearance = appearance10;
            this.tabCashManagement.SharedControlsPage = this.ultraTabSharedControlsPage6;
            this.tabCashManagement.Size = new System.Drawing.Size(653, 545);
            this.tabCashManagement.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabCashManagement.TabIndex = 0;
            ultraTab10.Key = "TransactionManagement";
            ultraTab10.TabPage = this.ultraTabPageControl14;
            ultraTab10.Text = "Transaction Management";
            ultraTab11.Key = "BalanceManagement";
            ultraTab11.TabPage = this.ultraTabPageControl15;
            ultraTab11.Text = "Balance Management";
            ultraTab12.Key = "Reconciliation";
            ultraTab12.TabPage = this.ultraTabPageControl16;
            ultraTab12.Text = "Reconciliation";
            this.tabCashManagement.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab10,
            ultraTab11,
            ultraTab12});
            this.tabCashManagement.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabCashManagement_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage6
            // 
            this.ultraTabSharedControlsPage6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage6.Name = "ultraTabSharedControlsPage6";
            this.ultraTabSharedControlsPage6.Size = new System.Drawing.Size(651, 524);
            // 
            // treePMMain
            // 
            this.treePMMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treePMMain.FullRowSelect = true;
            this.treePMMain.HideSelection = false;
            this.treePMMain.Location = new System.Drawing.Point(4, 3);
            this.treePMMain.Name = "treePMMain";
            this.treePMMain.NodeConnectorStyle = Infragistics.Win.UltraWinTree.NodeConnectorStyle.Solid;
            this.treePMMain.Size = new System.Drawing.Size(141, 528);
            this.treePMMain.TabIndex = 0;
            this.treePMMain.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.treePMMain_AfterSelect);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.treePMMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabMainPMAdmin);
            this.splitContainer1.Size = new System.Drawing.Size(806, 566);
            this.splitContainer1.SplitterDistance = 147;
            this.splitContainer1.TabIndex = 1;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Location = new System.Drawing.Point(3, 534);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(141, 29);
            this.panel1.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance11.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.btnAdd.Appearance = appearance11;
            this.btnAdd.BackColorInternal = System.Drawing.Color.White;
            this.btnAdd.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnAdd.ImageSize = new System.Drawing.Size(75, 23);
            this.btnAdd.Location = new System.Drawing.Point(32, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ShowFocusRect = false;
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tabMainPMAdmin
            // 
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabMainPMAdmin.ActiveTabAppearance = appearance12;
            this.tabMainPMAdmin.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabMainPMAdmin.Controls.Add(this.ultraTabPageControl1);
            this.tabMainPMAdmin.Controls.Add(this.ultraTabPageControl2);
            this.tabMainPMAdmin.Controls.Add(this.ultraTabPageControl3);
            this.tabMainPMAdmin.Controls.Add(this.ultraTabPageControl10);
            this.tabMainPMAdmin.Controls.Add(this.ultraTabPageControl13);
            this.tabMainPMAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMainPMAdmin.Location = new System.Drawing.Point(0, 0);
            this.tabMainPMAdmin.Name = "tabMainPMAdmin";
            appearance13.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            this.tabMainPMAdmin.SelectedTabAppearance = appearance13;
            this.tabMainPMAdmin.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMainPMAdmin.Size = new System.Drawing.Size(655, 566);
            this.tabMainPMAdmin.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabMainPMAdmin.TabIndex = 0;
            ultraTab13.Key = "ImportAdmin";
            ultraTab13.TabPage = this.ultraTabPageControl1;
            ultraTab13.Text = "Import Admin";
            ultraTab13.Visible = false;
            ultraTab14.Key = "SetUpCompany";
            ultraTab14.TabPage = this.ultraTabPageControl2;
            ultraTab14.Text = "Set Up Company";
            ultraTab14.Visible = false;
            ultraTab15.Key = "RunUpload";
            ultraTab15.TabPage = this.ultraTabPageControl3;
            ultraTab15.Text = "Run Upload";
            ultraTab16.TabPage = this.ultraTabPageControl10;
            ultraTab16.Text = "Reconciliation";
            ultraTab16.Visible = false;
            ultraTab17.Key = "CashManagement";
            ultraTab17.TabPage = this.ultraTabPageControl13;
            ultraTab17.Text = "Cash Management";
            ultraTab17.Visible = false;
            this.tabMainPMAdmin.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab13,
            ultraTab14,
            ultraTab15,
            ultraTab16,
            ultraTab17});
            this.tabMainPMAdmin.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabMainPMAdmin_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(653, 545);
            // 
            // PMMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(806, 566);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "PMMain";
            this.Text = "Import Setup";
            this.Load += new System.EventHandler(this.PMMain_Load);
            this.ultraTabPageControl8.ResumeLayout(false);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabDataSource)).EndInit();
            this.tabDataSource.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabCompanyDetails)).EndInit();
            this.ultraTabCompanyDetails.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabSetUpRunUpload)).EndInit();
            this.tabSetUpRunUpload.ResumeLayout(false);
            this.ultraTabPageControl10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabReconciliation)).EndInit();
            this.tabReconciliation.ResumeLayout(false);
            this.ultraTabPageControl13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabCashManagement)).EndInit();
            this.tabCashManagement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treePMMain)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabMainPMAdmin)).EndInit();
            this.tabMainPMAdmin.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTree.UltraTree treePMMain;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraButton btnAdd;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMainPMAdmin;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabDataSource;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabCompanyDetails;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl17;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabSetUpRunUpload;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
        private CtrlRunDownloadSetup ctrlRunUploadSetup1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabReconciliation;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage5;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl12;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl13;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCashManagement;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage6;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl14;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl15;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl16;
        //private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl9;
        //private CtrlRunDownload ctrlRunUpload1;
        //private CtrlSetupTradeRecon ctrlSetupTradeRecon1;
        //private CtrlRunTradeRecon ctrlRunTradeRecon1;
        //private CtrlCashTransactionManagement ctrlTransactionManagement1;
        //private CtrlCashBalanceManagement ctrlCashBalanceManagement1;
        //private CtrlCashRecon ctrlCashRecon1;
    }
}
