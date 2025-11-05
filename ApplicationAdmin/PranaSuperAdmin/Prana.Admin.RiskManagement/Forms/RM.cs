#region using
using Prana.Admin.BLL;
using Prana.Admin.RiskManagement.Controls;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Prana.Admin.RiskManagement
{
    /// <summary>
    /// Summary description for RM.
    /// </summary>
    public class RM : System.Windows.Forms.Form
    {
        #region Wizard Stuff

        #region Constants definitions

        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "RMADMIN : ";

        //Tab Constants defined by user.
        const int C_TAB_COMPANY = 0;
        const int C_TAB_RMAUEC = 1;
        const int C_TAB_TRADINGACCOUNT = 2;
        const int C_TAB_USERLEVEL = 3;
        const int C_TAB_FUNDACCOUNT = 4;
        const int C_TAB_CLIENT = 5;

        const int C_TAB_COMPANYOVERALLLIMIT = 0;
        const int C_TAB_COMPANYALERTS = 1;

        const int C_TAB_USERLEVELOVERALLLIMIT = 0;
        const int C_TAB_USERTRADINGACCOUNT = 1;
        const int C_TAB_USERLEVELUICONTROLS = 2;

        const int C_TAB_CLIENTOVERALLLIMIT = 0;

        #endregion

        #region Private 

        private System.Windows.Forms.TreeView trvRM;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcRMAdmin;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbPageCompany;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbPageUserLevel;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbPageClients;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcRMCompany;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbPageCompanyOverallLimits;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbPageCompanyAlerts;
        //		private Prana.Admin.RiskManagement.Controls.uctCompanyOverallLimits uctCompanyOverallLimits;
        //		private Prana.Admin.RiskManagement.Controls.Company_Alerts company_Alerts;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage3;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcUserLevel;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbPageUserOverall;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbPageUserUI;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        //		private Prana.Admin.RiskManagement.Controls.uctUserLevelOverallLimits uctUserLevelOverallLimits1;
        private Prana.Admin.RiskManagement.Controls.uctUserLevelOverallLimits uctUserLevelOverallLimits;
        private Prana.Admin.RiskManagement.Controls.uctCompanyOverallLimits uctCompanyOverallLimits;
        private Prana.Admin.RiskManagement.Controls.Company_Alerts company_Alerts;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Prana.Admin.RiskManagement.Controls.uctClientOverallLimits uctClientOverallLimits;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcClient;
        private Prana.Admin.RiskManagement.Controls.uctUserLevelUIControls uctUserLevelUIControls;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCompanySave;
        private System.Windows.Forms.Button btnClientSave;
        private System.Windows.Forms.Button btnClientClose;
        private System.Windows.Forms.Button btnDelete;

        #endregion Private
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Prana.Admin.RiskManagement.Controls.RM_AUEC rM_AUEC;
        private FundAccount fundAccount;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar;
        private Button btnAUECSave;
        private Button btnAUECClose;
        private Button btnAccountAccntSave;
        private Button btnAccountAccntClose;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcTradingAccount;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage5;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        private Prana.Admin.RiskManagement.Controls.TradingAccount tradingAccount;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
        private Prana.Admin.RiskManagement.Controls.UserTradingAccount userTradingAccount;
        private Button btnTradAccntSave;
        private Button btnTradAccntClose;
        private Button btnUserTradSave;
        private Button btnClose5;
        private Button btnUserSave;
        private Button btnUserClose;
        private Button btnUserUISave;
        private Button btnClose7;
        private IContainer components;

        public RM()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (trvRM != null)
                {
                    trvRM.Dispose();
                }
                if (ultraTabSharedControlsPage1 != null)
                {
                    ultraTabSharedControlsPage1.Dispose();
                }
                if (ultraTabSharedControlsPage2 != null)
                {
                    ultraTabSharedControlsPage2.Dispose();
                }
                if (tbcRMAdmin != null)
                {
                    tbcRMAdmin.Dispose();
                }
                if (tbPageCompany != null)
                {
                    tbPageCompany.Dispose();
                }
                if (tbPageUserLevel != null)
                {
                    tbPageUserLevel.Dispose();
                }
                if (tbPageClients != null)
                {
                    tbPageClients.Dispose();
                }
                if (tbcRMCompany != null)
                {
                    tbcRMCompany.Dispose();
                }
                if (tbPageCompanyOverallLimits != null)
                {
                    tbPageCompanyOverallLimits.Dispose();
                }
                if (tbPageCompanyAlerts != null)
                {
                    tbPageCompanyAlerts.Dispose();
                }
                if (ultraTabSharedControlsPage3 != null)
                {
                    ultraTabSharedControlsPage3.Dispose();
                }
                if (tbcUserLevel != null)
                {
                    tbcUserLevel.Dispose();
                }
                if (tbPageUserOverall != null)
                {
                    tbPageUserOverall.Dispose();
                }
                if (tbPageUserUI != null)
                {
                    tbPageUserUI.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (uctUserLevelOverallLimits != null)
                {
                    uctUserLevelOverallLimits.Dispose();
                }
                if (uctCompanyOverallLimits != null)
                {
                    uctCompanyOverallLimits.Dispose();
                }
                if (company_Alerts != null)
                {
                    company_Alerts.Dispose();
                }
                if (ultraTabSharedControlsPage4 != null)
                {
                    ultraTabSharedControlsPage4.Dispose();
                }
                if (ultraTabPageControl1 != null)
                {
                    ultraTabPageControl1.Dispose();
                }
                if (uctClientOverallLimits != null)
                {
                    uctClientOverallLimits.Dispose();
                }
                if (tbcClient != null)
                {
                    tbcClient.Dispose();
                }
                if (uctUserLevelUIControls != null)
                {
                    uctUserLevelUIControls.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnCompanySave != null)
                {
                    btnCompanySave.Dispose();
                }
                if (btnClientSave != null)
                {
                    btnClientSave.Dispose();
                }
                if (btnClientClose != null)
                {
                    btnClientClose.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (ultraTabPageControl2 != null)
                {
                    ultraTabPageControl2.Dispose();
                }
                if (ultraTabPageControl3 != null)
                {
                    ultraTabPageControl3.Dispose();
                }
                if (ultraTabPageControl4 != null)
                {
                    ultraTabPageControl4.Dispose();
                }
                if (rM_AUEC != null)
                {
                    rM_AUEC.Dispose();
                }
                if (fundAccount != null)
                {
                    fundAccount.Dispose();
                }
                if (ultraStatusBar != null)
                {
                    ultraStatusBar.Dispose();
                }
                if (btnAUECSave != null)
                {
                    btnAUECSave.Dispose();
                }
                if (btnAUECClose != null)
                {
                    btnAUECClose.Dispose();
                }
                if (btnAccountAccntSave != null)
                {
                    btnAccountAccntSave.Dispose();
                }
                if (btnAccountAccntClose != null)
                {
                    btnAccountAccntClose.Dispose();
                }
                if (tbcTradingAccount != null)
                {
                    tbcTradingAccount.Dispose();
                }
                if (ultraTabSharedControlsPage5 != null)
                {
                    ultraTabSharedControlsPage5.Dispose();
                }
                if (ultraTabPageControl5 != null)
                {
                    ultraTabPageControl5.Dispose();
                }
                if (tradingAccount != null)
                {
                    tradingAccount.Dispose();
                }
                if (ultraTabPageControl6 != null)
                {
                    ultraTabPageControl6.Dispose();
                }
                if (userTradingAccount != null)
                {
                    userTradingAccount.Dispose();
                }
                if (btnTradAccntSave != null)
                {
                    btnTradAccntSave.Dispose();
                }
                if (btnTradAccntClose != null)
                {
                    btnTradAccntClose.Dispose();
                }
                if (btnUserTradSave != null)
                {
                    btnUserTradSave.Dispose();
                }
                if (btnClose5 != null)
                {
                    btnClose5.Dispose();
                }
                if (btnUserSave != null)
                {
                    btnUserSave.Dispose();
                }
                if (btnUserClose != null)
                {
                    btnUserClose.Dispose();
                }
                if (btnUserUISave != null)
                {
                    btnUserUISave.Dispose();
                }
                if (btnClose7 != null)
                {
                    btnClose7.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion Wizard Stuff

        #region Windows Form Designer generated code
        /// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RM));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab(true);
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab(true);
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab(true);
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab12 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab13 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.tbPageCompanyOverallLimits = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.uctCompanyOverallLimits = new Prana.Admin.RiskManagement.Controls.uctCompanyOverallLimits();
            this.tbPageCompanyAlerts = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.company_Alerts = new Prana.Admin.RiskManagement.Controls.Company_Alerts();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnTradAccntSave = new System.Windows.Forms.Button();
            this.btnTradAccntClose = new System.Windows.Forms.Button();
            this.tradingAccount = new Prana.Admin.RiskManagement.Controls.TradingAccount();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnUserTradSave = new System.Windows.Forms.Button();
            this.btnClose5 = new System.Windows.Forms.Button();
            this.userTradingAccount = new Prana.Admin.RiskManagement.Controls.UserTradingAccount();
            this.tbPageUserOverall = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnUserSave = new System.Windows.Forms.Button();
            this.btnUserClose = new System.Windows.Forms.Button();
            this.uctUserLevelOverallLimits = new Prana.Admin.RiskManagement.Controls.uctUserLevelOverallLimits();
            this.tbPageUserUI = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnUserUISave = new System.Windows.Forms.Button();
            this.btnClose7 = new System.Windows.Forms.Button();
            this.uctUserLevelUIControls = new Prana.Admin.RiskManagement.Controls.uctUserLevelUIControls();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.uctClientOverallLimits = new Prana.Admin.RiskManagement.Controls.uctClientOverallLimits();
            this.tbPageCompany = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnCompanySave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbcRMCompany = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnAUECSave = new System.Windows.Forms.Button();
            this.btnAUECClose = new System.Windows.Forms.Button();
            this.rM_AUEC = new Prana.Admin.RiskManagement.Controls.RM_AUEC();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tbcTradingAccount = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage5 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tbPageUserLevel = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tbcUserLevel = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage3 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnAccountAccntSave = new System.Windows.Forms.Button();
            this.btnAccountAccntClose = new System.Windows.Forms.Button();
            this.fundAccount = new Prana.Admin.RiskManagement.Controls.FundAccount();
            this.tbPageClients = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnClientSave = new System.Windows.Forms.Button();
            this.btnClientClose = new System.Windows.Forms.Button();
            this.tbcClient = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage4 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.trvRM = new System.Windows.Forms.TreeView();
            this.tbcRMAdmin = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.btnDelete = new System.Windows.Forms.Button();
            this.ultraStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.tbPageCompanyOverallLimits.SuspendLayout();
            this.tbPageCompanyAlerts.SuspendLayout();
            this.ultraTabPageControl5.SuspendLayout();
            this.ultraTabPageControl6.SuspendLayout();
            this.tbPageUserOverall.SuspendLayout();
            this.tbPageUserUI.SuspendLayout();
            this.ultraTabPageControl1.SuspendLayout();
            this.tbPageCompany.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbcRMCompany)).BeginInit();
            this.tbcRMCompany.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbcTradingAccount)).BeginInit();
            this.tbcTradingAccount.SuspendLayout();
            this.tbPageUserLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbcUserLevel)).BeginInit();
            this.tbcUserLevel.SuspendLayout();
            this.ultraTabPageControl4.SuspendLayout();
            this.tbPageClients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbcClient)).BeginInit();
            this.tbcClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcRMAdmin)).BeginInit();
            this.tbcRMAdmin.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPageCompanyOverallLimits
            // 
            this.tbPageCompanyOverallLimits.Controls.Add(this.uctCompanyOverallLimits);
            this.tbPageCompanyOverallLimits.Location = new System.Drawing.Point(1, 20);
            this.tbPageCompanyOverallLimits.Name = "tbPageCompanyOverallLimits";
            this.tbPageCompanyOverallLimits.Size = new System.Drawing.Size(703, 259);
            // 
            // uctCompanyOverallLimits
            // 
            this.uctCompanyOverallLimits.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctCompanyOverallLimits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctCompanyOverallLimits.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uctCompanyOverallLimits.Location = new System.Drawing.Point(0, 0);
            this.uctCompanyOverallLimits.Name = "uctCompanyOverallLimits";
            this.uctCompanyOverallLimits.Size = new System.Drawing.Size(703, 259);
            this.uctCompanyOverallLimits.TabIndex = 0;
            // 
            // tbPageCompanyAlerts
            // 
            this.tbPageCompanyAlerts.Controls.Add(this.company_Alerts);
            this.tbPageCompanyAlerts.Location = new System.Drawing.Point(-10000, -10000);
            this.tbPageCompanyAlerts.Name = "tbPageCompanyAlerts";
            this.tbPageCompanyAlerts.Size = new System.Drawing.Size(703, 259);
            // 
            // company_Alerts
            // 
            this.company_Alerts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.company_Alerts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.company_Alerts.Location = new System.Drawing.Point(9, 12);
            this.company_Alerts.Name = "company_Alerts";
            this.company_Alerts.Size = new System.Drawing.Size(684, 234);
            this.company_Alerts.TabIndex = 0;
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Controls.Add(this.btnTradAccntSave);
            this.ultraTabPageControl5.Controls.Add(this.btnTradAccntClose);
            this.ultraTabPageControl5.Controls.Add(this.tradingAccount);
            this.ultraTabPageControl5.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(702, 292);
            // 
            // btnTradAccntSave
            // 
            this.btnTradAccntSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTradAccntSave.BackgroundImage")));
            this.btnTradAccntSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTradAccntSave.Location = new System.Drawing.Point(263, 254);
            this.btnTradAccntSave.Name = "btnTradAccntSave";
            this.btnTradAccntSave.Size = new System.Drawing.Size(75, 23);
            this.btnTradAccntSave.TabIndex = 31;
            this.btnTradAccntSave.Click += new System.EventHandler(this.btnTradAccntSave_Click);
            // 
            // btnTradAccntClose
            // 
            this.btnTradAccntClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTradAccntClose.BackgroundImage")));
            this.btnTradAccntClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTradAccntClose.Location = new System.Drawing.Point(357, 254);
            this.btnTradAccntClose.Name = "btnTradAccntClose";
            this.btnTradAccntClose.Size = new System.Drawing.Size(75, 23);
            this.btnTradAccntClose.TabIndex = 30;
            this.btnTradAccntClose.Click += new System.EventHandler(this.btnTradAccntClose_Click);
            // 
            // tradingAccount
            // 
            this.tradingAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.tradingAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tradingAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tradingAccount.Location = new System.Drawing.Point(0, 0);
            this.tradingAccount.Name = "tradingAccount";
            this.tradingAccount.Size = new System.Drawing.Size(702, 292);
            this.tradingAccount.TabIndex = 0;
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Controls.Add(this.btnUserTradSave);
            this.ultraTabPageControl6.Controls.Add(this.btnClose5);
            this.ultraTabPageControl6.Controls.Add(this.userTradingAccount);
            this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(702, 292);
            // 
            // btnUserTradSave
            // 
            this.btnUserTradSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUserTradSave.BackgroundImage")));
            this.btnUserTradSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserTradSave.Location = new System.Drawing.Point(232, 255);
            this.btnUserTradSave.Name = "btnUserTradSave";
            this.btnUserTradSave.Size = new System.Drawing.Size(75, 23);
            this.btnUserTradSave.TabIndex = 31;
            this.btnUserTradSave.Click += new System.EventHandler(this.btnUserTradSave_Click);
            // 
            // btnClose5
            // 
            this.btnClose5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose5.BackgroundImage")));
            this.btnClose5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose5.Location = new System.Drawing.Point(326, 255);
            this.btnClose5.Name = "btnClose5";
            this.btnClose5.Size = new System.Drawing.Size(75, 23);
            this.btnClose5.TabIndex = 30;
            this.btnClose5.Click += new System.EventHandler(this.btnClose5_Click);
            // 
            // userTradingAccount
            // 
            this.userTradingAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.userTradingAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.userTradingAccount.Location = new System.Drawing.Point(17, 16);
            this.userTradingAccount.Name = "userTradingAccount";
            this.userTradingAccount.Size = new System.Drawing.Size(669, 224);
            this.userTradingAccount.TabIndex = 0;
            // 
            // tbPageUserOverall
            // 
            this.tbPageUserOverall.Controls.Add(this.btnUserSave);
            this.tbPageUserOverall.Controls.Add(this.btnUserClose);
            this.tbPageUserOverall.Controls.Add(this.uctUserLevelOverallLimits);
            this.tbPageUserOverall.Location = new System.Drawing.Point(1, 20);
            this.tbPageUserOverall.Name = "tbPageUserOverall";
            this.tbPageUserOverall.Size = new System.Drawing.Size(702, 292);
            // 
            // btnUserSave
            // 
            this.btnUserSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUserSave.BackgroundImage")));
            this.btnUserSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserSave.Location = new System.Drawing.Point(245, 264);
            this.btnUserSave.Name = "btnUserSave";
            this.btnUserSave.Size = new System.Drawing.Size(75, 23);
            this.btnUserSave.TabIndex = 31;
            this.btnUserSave.Click += new System.EventHandler(this.btnUserSave_Click);
            // 
            // btnUserClose
            // 
            this.btnUserClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUserClose.BackgroundImage")));
            this.btnUserClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserClose.Location = new System.Drawing.Point(339, 264);
            this.btnUserClose.Name = "btnUserClose";
            this.btnUserClose.Size = new System.Drawing.Size(75, 23);
            this.btnUserClose.TabIndex = 30;
            this.btnUserClose.Click += new System.EventHandler(this.btnUserClose_Click);
            // 
            // uctUserLevelOverallLimits
            // 
            this.uctUserLevelOverallLimits.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctUserLevelOverallLimits.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.uctUserLevelOverallLimits.Location = new System.Drawing.Point(0, 0);
            this.uctUserLevelOverallLimits.Name = "uctUserLevelOverallLimits";
            this.uctUserLevelOverallLimits.Size = new System.Drawing.Size(705, 261);
            this.uctUserLevelOverallLimits.TabIndex = 0;
            // 
            // tbPageUserUI
            // 
            this.tbPageUserUI.Controls.Add(this.btnUserUISave);
            this.tbPageUserUI.Controls.Add(this.btnClose7);
            this.tbPageUserUI.Controls.Add(this.uctUserLevelUIControls);
            this.tbPageUserUI.Location = new System.Drawing.Point(-10000, -10000);
            this.tbPageUserUI.Name = "tbPageUserUI";
            this.tbPageUserUI.Size = new System.Drawing.Size(702, 292);
            // 
            // btnUserUISave
            // 
            this.btnUserUISave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUserUISave.BackgroundImage")));
            this.btnUserUISave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserUISave.Location = new System.Drawing.Point(247, 264);
            this.btnUserUISave.Name = "btnUserUISave";
            this.btnUserUISave.Size = new System.Drawing.Size(75, 23);
            this.btnUserUISave.TabIndex = 31;
            this.btnUserUISave.Click += new System.EventHandler(this.btnUserUISave_Click);
            // 
            // btnClose7
            // 
            this.btnClose7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose7.BackgroundImage")));
            this.btnClose7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose7.Location = new System.Drawing.Point(341, 264);
            this.btnClose7.Name = "btnClose7";
            this.btnClose7.Size = new System.Drawing.Size(75, 23);
            this.btnClose7.TabIndex = 30;
            this.btnClose7.Click += new System.EventHandler(this.btnClose7_Click);
            // 
            // uctUserLevelUIControls
            // 
            this.uctUserLevelUIControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctUserLevelUIControls.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.uctUserLevelUIControls.Location = new System.Drawing.Point(17, 3);
            this.uctUserLevelUIControls.Name = "uctUserLevelUIControls";
            this.uctUserLevelUIControls.Size = new System.Drawing.Size(671, 252);
            this.uctUserLevelUIControls.TabIndex = 0;
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.uctClientOverallLimits);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(703, 253);
            // 
            // uctClientOverallLimits
            // 
            this.uctClientOverallLimits.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctClientOverallLimits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctClientOverallLimits.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.uctClientOverallLimits.Location = new System.Drawing.Point(0, 0);
            this.uctClientOverallLimits.Name = "uctClientOverallLimits";
            this.uctClientOverallLimits.Size = new System.Drawing.Size(703, 253);
            this.uctClientOverallLimits.TabIndex = 0;
            // 
            // tbPageCompany
            // 
            this.tbPageCompany.Controls.Add(this.btnCompanySave);
            this.tbPageCompany.Controls.Add(this.btnClose);
            this.tbPageCompany.Controls.Add(this.tbcRMCompany);
            this.tbPageCompany.Location = new System.Drawing.Point(1, 20);
            this.tbPageCompany.Name = "tbPageCompany";
            this.tbPageCompany.Size = new System.Drawing.Size(704, 313);
            // 
            // btnCompanySave
            // 
            this.btnCompanySave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCompanySave.BackgroundImage")));
            this.btnCompanySave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompanySave.Location = new System.Drawing.Point(268, 285);
            this.btnCompanySave.Name = "btnCompanySave";
            this.btnCompanySave.Size = new System.Drawing.Size(75, 23);
            this.btnCompanySave.TabIndex = 27;
            this.btnCompanySave.Click += new System.EventHandler(this.btnCompanySave_Click_1);
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(362, 285);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 26;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // tbcRMCompany
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.FontData.BoldAsString = "True";
            this.tbcRMCompany.ActiveTabAppearance = appearance1;
            this.tbcRMCompany.Controls.Add(this.ultraTabSharedControlsPage2);
            this.tbcRMCompany.Controls.Add(this.tbPageCompanyOverallLimits);
            this.tbcRMCompany.Controls.Add(this.tbPageCompanyAlerts);
            this.tbcRMCompany.Location = new System.Drawing.Point(0, 0);
            this.tbcRMCompany.Name = "tbcRMCompany";
            this.tbcRMCompany.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.tbcRMCompany.Size = new System.Drawing.Size(705, 280);
            this.tbcRMCompany.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tbcRMCompany.TabIndex = 0;
            ultraTab1.Key = "CompanyOverallLimits";
            ultraTab1.TabPage = this.tbPageCompanyOverallLimits;
            ultraTab1.Text = "Company Overall Limits";
            ultraTab2.Key = "CompanyAlerts";
            ultraTab2.TabPage = this.tbPageCompanyAlerts;
            ultraTab2.Text = "Company Alerts";
            this.tbcRMCompany.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(703, 259);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.btnAUECSave);
            this.ultraTabPageControl2.Controls.Add(this.btnAUECClose);
            this.ultraTabPageControl2.Controls.Add(this.rM_AUEC);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(704, 313);
            // 
            // btnAUECSave
            // 
            this.btnAUECSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAUECSave.BackgroundImage")));
            this.btnAUECSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAUECSave.Location = new System.Drawing.Point(268, 285);
            this.btnAUECSave.Name = "btnAUECSave";
            this.btnAUECSave.Size = new System.Drawing.Size(75, 23);
            this.btnAUECSave.TabIndex = 29;
            this.btnAUECSave.Click += new System.EventHandler(this.btnAUECSave_Click_1);
            // 
            // btnAUECClose
            // 
            this.btnAUECClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAUECClose.BackgroundImage")));
            this.btnAUECClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAUECClose.Location = new System.Drawing.Point(362, 285);
            this.btnAUECClose.Name = "btnAUECClose";
            this.btnAUECClose.Size = new System.Drawing.Size(75, 23);
            this.btnAUECClose.TabIndex = 28;
            this.btnAUECClose.Click += new System.EventHandler(this.btnAUECClose_Click_1);
            // 
            // rM_AUEC
            // 
            this.rM_AUEC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.rM_AUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.rM_AUEC.Location = new System.Drawing.Point(5, 3);
            this.rM_AUEC.Name = "rM_AUEC";
            this.rM_AUEC.Size = new System.Drawing.Size(695, 278);
            this.rM_AUEC.TabIndex = 0;
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.tbcTradingAccount);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(704, 313);
            // 
            // tbcTradingAccount
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance2.BackColor2 = System.Drawing.Color.White;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.FontData.BoldAsString = "True";
            this.tbcTradingAccount.ActiveTabAppearance = appearance2;
            this.tbcTradingAccount.Controls.Add(this.ultraTabSharedControlsPage5);
            this.tbcTradingAccount.Controls.Add(this.ultraTabPageControl5);
            this.tbcTradingAccount.Controls.Add(this.ultraTabPageControl6);
            this.tbcTradingAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcTradingAccount.Location = new System.Drawing.Point(0, 0);
            this.tbcTradingAccount.Name = "tbcTradingAccount";
            this.tbcTradingAccount.SharedControlsPage = this.ultraTabSharedControlsPage5;
            this.tbcTradingAccount.Size = new System.Drawing.Size(704, 313);
            this.tbcTradingAccount.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tbcTradingAccount.TabIndex = 30;
            ultraTab3.Key = "TradingAccountOverallLimits";
            ultraTab3.TabPage = this.ultraTabPageControl5;
            ultraTab3.Text = "Trading Account OverallLimits";
            ultraTab4.Key = "TradingAccountsUsers";
            ultraTab4.TabPage = this.ultraTabPageControl6;
            ultraTab4.Text = "Trading Accounts\' Users";
            this.tbcTradingAccount.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab3,
            ultraTab4});
            this.tbcTradingAccount.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tbcTradingAccount_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage5
            // 
            this.ultraTabSharedControlsPage5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage5.Name = "ultraTabSharedControlsPage5";
            this.ultraTabSharedControlsPage5.Size = new System.Drawing.Size(702, 292);
            // 
            // tbPageUserLevel
            // 
            this.tbPageUserLevel.Controls.Add(this.tbcUserLevel);
            this.tbPageUserLevel.Location = new System.Drawing.Point(-10000, -10000);
            this.tbPageUserLevel.Name = "tbPageUserLevel";
            this.tbPageUserLevel.Size = new System.Drawing.Size(704, 313);
            // 
            // tbcUserLevel
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.FontData.BoldAsString = "True";
            this.tbcUserLevel.ActiveTabAppearance = appearance3;
            this.tbcUserLevel.Controls.Add(this.ultraTabSharedControlsPage3);
            this.tbcUserLevel.Controls.Add(this.tbPageUserOverall);
            this.tbcUserLevel.Controls.Add(this.tbPageUserUI);
            this.tbcUserLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcUserLevel.Location = new System.Drawing.Point(0, 0);
            this.tbcUserLevel.Name = "tbcUserLevel";
            this.tbcUserLevel.SharedControlsPage = this.ultraTabSharedControlsPage3;
            this.tbcUserLevel.Size = new System.Drawing.Size(704, 313);
            this.tbcUserLevel.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tbcUserLevel.TabIndex = 0;
            ultraTab5.Key = "UserLevelOverallLimits";
            ultraTab5.TabPage = this.tbPageUserOverall;
            ultraTab5.Text = "UserLevel OverallLimits";
            ultraTab6.Key = "UserUIControls";
            ultraTab6.TabPage = this.tbPageUserUI;
            ultraTab6.Text = "User UI Controls";
            this.tbcUserLevel.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab5,
            ultraTab6});
            this.tbcUserLevel.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tbcUserLevel_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage3
            // 
            this.ultraTabSharedControlsPage3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage3.Name = "ultraTabSharedControlsPage3";
            this.ultraTabSharedControlsPage3.Size = new System.Drawing.Size(702, 292);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.btnAccountAccntSave);
            this.ultraTabPageControl4.Controls.Add(this.btnAccountAccntClose);
            this.ultraTabPageControl4.Controls.Add(this.fundAccount);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(704, 313);
            // 
            // btnAccountAccntSave
            // 
            this.btnAccountAccntSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAccountAccntSave.BackgroundImage")));
            this.btnAccountAccntSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccountAccntSave.Location = new System.Drawing.Point(268, 285);
            this.btnAccountAccntSave.Name = "btnAccountAccntSave";
            this.btnAccountAccntSave.Size = new System.Drawing.Size(75, 23);
            this.btnAccountAccntSave.TabIndex = 29;
            this.btnAccountAccntSave.Click += new System.EventHandler(this.btnAccountAccntSave_Click);
            // 
            // btnAccountAccntClose
            // 
            this.btnAccountAccntClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAccountAccntClose.BackgroundImage")));
            this.btnAccountAccntClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccountAccntClose.Location = new System.Drawing.Point(362, 285);
            this.btnAccountAccntClose.Name = "btnAccountAccntClose";
            this.btnAccountAccntClose.Size = new System.Drawing.Size(75, 23);
            this.btnAccountAccntClose.TabIndex = 28;
            this.btnAccountAccntClose.Click += new System.EventHandler(this.btnAccountAccntClose_Click);
            // 
            // fundAccount
            // 
            this.fundAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.fundAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.fundAccount.Location = new System.Drawing.Point(-1, 0);
            this.fundAccount.Name = "fundAccount";
            this.fundAccount.Size = new System.Drawing.Size(705, 282);
            this.fundAccount.TabIndex = 0;
            // 
            // tbPageClients
            // 
            this.tbPageClients.Controls.Add(this.btnClientSave);
            this.tbPageClients.Controls.Add(this.btnClientClose);
            this.tbPageClients.Controls.Add(this.tbcClient);
            this.tbPageClients.Location = new System.Drawing.Point(-10000, -10000);
            this.tbPageClients.Name = "tbPageClients";
            this.tbPageClients.Size = new System.Drawing.Size(704, 313);
            // 
            // btnClientSave
            // 
            this.btnClientSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClientSave.BackgroundImage")));
            this.btnClientSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClientSave.Location = new System.Drawing.Point(268, 285);
            this.btnClientSave.Name = "btnClientSave";
            this.btnClientSave.Size = new System.Drawing.Size(75, 23);
            this.btnClientSave.TabIndex = 31;
            this.btnClientSave.Click += new System.EventHandler(this.btnClientSave_Click_1);
            // 
            // btnClientClose
            // 
            this.btnClientClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClientClose.BackgroundImage")));
            this.btnClientClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClientClose.Location = new System.Drawing.Point(362, 285);
            this.btnClientClose.Name = "btnClientClose";
            this.btnClientClose.Size = new System.Drawing.Size(75, 23);
            this.btnClientClose.TabIndex = 30;
            this.btnClientClose.Click += new System.EventHandler(this.btnClose3_Click_1);
            // 
            // tbcClient
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.FontData.BoldAsString = "True";
            this.tbcClient.ActiveTabAppearance = appearance4;
            this.tbcClient.Controls.Add(this.ultraTabSharedControlsPage4);
            this.tbcClient.Controls.Add(this.ultraTabPageControl1);
            this.tbcClient.Location = new System.Drawing.Point(0, 0);
            this.tbcClient.Name = "tbcClient";
            this.tbcClient.SharedControlsPage = this.ultraTabSharedControlsPage4;
            this.tbcClient.Size = new System.Drawing.Size(705, 274);
            this.tbcClient.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tbcClient.TabIndex = 0;
            ultraTab7.Key = "ClientOverallLimits";
            ultraTab7.TabPage = this.ultraTabPageControl1;
            ultraTab7.Text = "Client Overall Limits";
            this.tbcClient.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab7});
            // 
            // ultraTabSharedControlsPage4
            // 
            this.ultraTabSharedControlsPage4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage4.Name = "ultraTabSharedControlsPage4";
            this.ultraTabSharedControlsPage4.Size = new System.Drawing.Size(703, 253);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // trvRM
            // 
            this.trvRM.BackColor = System.Drawing.Color.White;
            this.trvRM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.trvRM.Cursor = System.Windows.Forms.Cursors.Default;
            this.trvRM.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.trvRM.ForeColor = System.Drawing.Color.Black;
            this.trvRM.FullRowSelect = true;
            this.trvRM.HideSelection = false;
            this.trvRM.HotTracking = true;
            this.trvRM.Indent = 29;
            this.trvRM.Location = new System.Drawing.Point(4, 7);
            this.trvRM.Name = "trvRM";
            this.trvRM.ShowRootLines = false;
            this.trvRM.Size = new System.Drawing.Size(205, 304);
            this.trvRM.TabIndex = 0;
            this.trvRM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvRM_AfterSelect);
            // 
            // tbcRMAdmin
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.FontData.BoldAsString = "True";
            this.tbcRMAdmin.ActiveTabAppearance = appearance6;
            this.tbcRMAdmin.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tbcRMAdmin.Controls.Add(this.tbPageCompany);
            this.tbcRMAdmin.Controls.Add(this.tbPageUserLevel);
            this.tbcRMAdmin.Controls.Add(this.tbPageClients);
            this.tbcRMAdmin.Controls.Add(this.ultraTabPageControl2);
            this.tbcRMAdmin.Controls.Add(this.ultraTabPageControl3);
            this.tbcRMAdmin.Controls.Add(this.ultraTabPageControl4);
            this.tbcRMAdmin.Location = new System.Drawing.Point(213, 7);
            this.tbcRMAdmin.Name = "tbcRMAdmin";
            this.tbcRMAdmin.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tbcRMAdmin.Size = new System.Drawing.Size(706, 334);
            this.tbcRMAdmin.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tbcRMAdmin.TabIndex = 1;
            ultraTab8.Key = "Company";
            ultraTab8.TabPage = this.tbPageCompany;
            ultraTab8.Text = "Company";
            ultraTab9.Key = "RMAUEC";
            ultraTab9.TabPage = this.ultraTabPageControl2;
            ultraTab9.Text = "RM AUECs";
            ultraTab10.Key = "TradingAccount";
            ultraTab10.TabPage = this.ultraTabPageControl3;
            ultraTab10.Text = "Trading Account";
            ultraTab11.Key = "UserLevel";
            ultraTab11.TabPage = this.tbPageUserLevel;
            ultraTab11.Text = "User Level";
            ultraTab12.Key = "FundAccount";
            ultraTab12.TabPage = this.ultraTabPageControl4;
            ultraTab12.Text = "Account";
            ultraTab13.Key = "Client";
            ultraTab13.TabPage = this.tbPageClients;
            ultraTab13.Text = "Clients";
            this.tbcRMAdmin.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab8,
            ultraTab9,
            ultraTab10,
            ultraTab11,
            ultraTab12,
            ultraTab13});
            this.tbcRMAdmin.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tbcRMAdmin_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(704, 313);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(69, 315);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 28;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click_1);
            // 
            // ultraStatusBar
            // 
            appearance5.ForeColor = System.Drawing.Color.Red;
            appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance5.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraStatusBar.Appearance = appearance5;
            this.ultraStatusBar.Location = new System.Drawing.Point(0, 340);
            this.ultraStatusBar.MaximumSize = new System.Drawing.Size(885, 18);
            this.ultraStatusBar.Name = "ultraStatusBar";
            this.ultraStatusBar.Size = new System.Drawing.Size(885, 18);
            this.ultraStatusBar.TabIndex = 29;
            // 
            // RM
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(922, 358);
            this.Controls.Add(this.ultraStatusBar);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.tbcRMAdmin);
            this.Controls.Add(this.trvRM);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(930, 392);
            this.Name = "RM";
            this.Text = "RM Admin";
            this.Load += new System.EventHandler(this.RM_Load);
            this.tbPageCompanyOverallLimits.ResumeLayout(false);
            this.tbPageCompanyAlerts.ResumeLayout(false);
            this.ultraTabPageControl5.ResumeLayout(false);
            this.ultraTabPageControl6.ResumeLayout(false);
            this.tbPageUserOverall.ResumeLayout(false);
            this.tbPageUserUI.ResumeLayout(false);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.tbPageCompany.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbcRMCompany)).EndInit();
            this.tbcRMCompany.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbcTradingAccount)).EndInit();
            this.tbcTradingAccount.ResumeLayout(false);
            this.tbPageUserLevel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbcUserLevel)).EndInit();
            this.tbcUserLevel.ResumeLayout(false);
            this.ultraTabPageControl4.ResumeLayout(false);
            this.tbPageClients.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbcClient)).EndInit();
            this.tbcClient.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcRMAdmin)).EndInit();
            this.tbcRMAdmin.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region RiskTree Binding

        /// <summary>
        /// this method is to bind the tree nodes in the form
        /// </summary>
        private void BindRMTree()
        {
            //To clear the tree of any node before binding it afresh.
            trvRM.Nodes.Clear();

            //Create Company nodes and Add label Company.

            //Font font = new Font("Vedana", 8.25F , System.Drawing.FontStyle.Bold);
            Font font = new Font("Tahoma", 11, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

            TreeNode treeNodeCompanies = new TreeNode("Company");
            //Making the root node to bold by assigning it to the font object defined above. 
            treeNodeCompanies.NodeFont = font;
            NodeDetails companiesNode = new NodeDetails(NodeType.Company, int.MinValue, int.MinValue);
            treeNodeCompanies.Tag = companiesNode;

            //GetCompanies method is used to fetch the existing companies in the database.
            Companies companies = RMAdminBusinessLogic.GetCompanies();
            CompanyOverallLimits companyOverallLimits = RMAdminBusinessLogic.GetCompanyOverallLimits();
            try
            {
                //Loop through all the companies, users and clients, assigning each node an id 
                //corresponding to its unique value in the database.
                foreach (Company company in companies)
                {
                    //if (company.CompanyID == 14)
                    //{
                    TreeNode treeNodeCompany = new TreeNode(company.Name);
                    //Making the root node to bold by assigning it to the font object defined above. 
                    treeNodeCompany.NodeFont = font;
                    NodeDetails companyNode = new NodeDetails(NodeType.Company, company.CompanyID, company.CompanyID);
                    treeNodeCompany.Tag = companyNode;

                    foreach (CompanyOverallLimit companyOverall in companyOverallLimits)
                    {
                        if (company.CompanyID == companyOverall.CompanyID)
                        {
                            treeNodeCompany.ForeColor = System.Drawing.Color.Navy;
                            break;
                        }
                        //else
                        //{
                        //    treeNodeCompany.ForeColor = System.Drawing.Color.Black;
                        //}
                    }

                    //create RM AUECs nodes.
                    //--------------------------------------------------------------------------

                    //Add Label Company AUECs.
                    TreeNode treeNodeRMAUECs = new TreeNode("Company AUECs");
                    //Making the root node to bold by assigning it to the font object defined above. 
                    treeNodeRMAUECs.NodeFont = font;
                    NodeDetails auecsNode = new NodeDetails(NodeType.CompanyAUECs, int.MinValue, company.CompanyID);
                    treeNodeRMAUECs.Tag = auecsNode;

                    ////GetCompanyAUECs method is used to fetch the existing RM AUECS in the database for a given company.
                    AUECs auecs = RMAdminBusinessLogic.GetAllCompanyAUECs(company.CompanyID);
                    RMAUECs rmAUECs = RMAdminBusinessLogic.GetRM_AUECs(company.CompanyID);
                    if (auecs.Count > 0)
                    {
                        foreach (AUEC auec in auecs)
                        {
                            //SK 20061009 REmoved Compliance Class                                
                            // New object of currency class.
                            //Currency currency = new Currency();
                            //// Assign to this object the data for a particular currencyID. 
                            //currency = RMAdminBusinessLogic.GetCurrency(auec.Compliance.BaseCurrencyID);
                            //

                            // the string is assigned the asset name, underlying name , exchange name and currency symbol in concatenated form.
                            string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.DisplayName.ToString() + "/" + auec.Currency.CurrencyName.ToString();
                            //string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + currency.CurrencySymbol.ToString();
                            int Value = auec.AUECID;

                            TreeNode treeNodeAUEC = new TreeNode(Data);
                            NodeDetails auecNode = new NodeDetails(NodeType.CompanyAUECs, Value, company.CompanyID);
                            treeNodeAUEC.Tag = auecNode;
                            foreach (RMAUEC rMAUEC in rmAUECs)
                            {
                                if (auec.AUECID == rMAUEC.AUECID)
                                {
                                    treeNodeAUEC.ForeColor = System.Drawing.Color.Navy;
                                    break;
                                }
                            }
                            treeNodeRMAUECs.Nodes.Add(treeNodeAUEC);
                        }

                    }

                    treeNodeCompany.Nodes.Add(treeNodeRMAUECs);


                    // Create Trading Account Nodes.
                    //--------------------------------------------------------------------------

                    //Add label Trading Account.
                    TreeNode treeNodeTradAccnts = new TreeNode("Trading Account");
                    //Making the root node to bold by assigning it to the font object defined above. 
                    treeNodeTradAccnts.NodeFont = font;
                    NodeDetails tradAccntsNode = new NodeDetails(NodeType.TradingAccount, int.MinValue, company.CompanyID);
                    treeNodeTradAccnts.Tag = tradAccntsNode;

                    //GetRMTradingAccnts method is used to fetch the existing Trading Accounts in the database for a given RM company.
                    TradingAccounts tradingAccnts = RMAdminBusinessLogic.GetCompanyTradingAccnts(company.CompanyID);
                    RMTradingAccounts rMTradingAccounts = RMAdminBusinessLogic.GetRMTradingAccnts(company.CompanyID);
                    foreach (Prana.Admin.BLL.TradingAccount tradingAccnt in tradingAccnts)
                    {
                        TreeNode treeNodeTradAccnt = new TreeNode(tradingAccnt.TradingShortName);
                        NodeDetails tradAccntNode = new NodeDetails(NodeType.TradingAccount, tradingAccnt.TradingAccountsID, company.CompanyID);
                        treeNodeTradAccnt.Tag = tradAccntNode;
                        foreach (RMTradingAccount rMTA in rMTradingAccounts)
                        {
                            if (tradingAccnt.TradingAccountsID == rMTA.CompanyTradingAccountID)
                            {
                                treeNodeTradAccnt.ForeColor = System.Drawing.Color.Navy;
                                break;
                            }
                        }

                        //Create a level for tradingAccount users.
                        //======================================
                        //Add label Users of Trading Account.
                        TreeNode treeNodeTradAccntUsers = new TreeNode("Users");
                        //Making the root node to bold by assigning it to the font object defined above. 
                        treeNodeTradAccntUsers.NodeFont = font;
                        NodeDetails tradAccntUsersNode = new NodeDetails(NodeType.TradingAccountUser, int.MinValue, tradingAccnt.TradingAccountsID, company.CompanyID);
                        treeNodeTradAccntUsers.Tag = tradAccntUsersNode;

                        Users tradAccntUsers = RMAdminBusinessLogic.GetUsersforTradingAccount(tradingAccnt.TradingAccountsID);
                        Prana.Admin.BLL.UserTradingAccounts userTradAccnts = RMAdminBusinessLogic.GetUserTradingAccounts(company.CompanyID, tradingAccnt.TradingAccountsID);
                        foreach (User user in tradAccntUsers)
                        {
                            TreeNode treeNodeTradAccntUser = new TreeNode(user.ShortName);
                            NodeDetails tradAccntUserNode = new NodeDetails(NodeType.TradingAccountUser, user.UserID, tradingAccnt.TradingAccountsID, company.CompanyID);
                            treeNodeTradAccntUser.Tag = tradAccntUserNode;
                            foreach (Prana.Admin.BLL.UserTradingAccount userTradAccnt in userTradAccnts)
                            {
                                if (user.UserID == userTradAccnt.CompanyUserID)
                                {
                                    treeNodeTradAccntUser.ForeColor = System.Drawing.Color.Navy;
                                    break;
                                }

                            }
                            treeNodeTradAccntUsers.Nodes.Add(treeNodeTradAccntUser);
                        }
                        treeNodeTradAccnt.Nodes.Add(treeNodeTradAccntUsers);

                        treeNodeTradAccnts.Nodes.Add(treeNodeTradAccnt);
                    }

                    treeNodeCompany.Nodes.Add(treeNodeTradAccnts);

                    //Create company user nodes.
                    // --------------------------------------------------------------------------

                    //Add label USER.
                    TreeNode treeNodeUsers = new TreeNode("Users");
                    //Making the root node to bold by assigning it to the font object defined above. 
                    treeNodeUsers.NodeFont = font;
                    NodeDetails usersNode = new NodeDetails(NodeType.Users, int.MinValue, company.CompanyID);
                    treeNodeUsers.Tag = usersNode;

                    //GetUsers method is used to fetch the existing users in the database to a given company.
                    Users users = RMAdminBusinessLogic.GetCompanyUsers(company.CompanyID);
                    UserLevelOverallLimits userLevelOverallLimits = RMAdminBusinessLogic.GetUserLevelOverallLimits(company.CompanyID);
                    foreach (User user in users)
                    {
                        TreeNode treeNodeUser = new TreeNode(user.ShortName /*+ " " + user.FirstName*/);
                        NodeDetails userNode = new NodeDetails(NodeType.Users, user.UserID, company.CompanyID);
                        treeNodeUser.Tag = userNode;
                        foreach (UserLevelOverallLimit userLevelOverallLimit in userLevelOverallLimits)
                        {
                            if (user.UserID == userLevelOverallLimit.CompanyUserID)
                            {
                                treeNodeUser.ForeColor = System.Drawing.Color.Navy;
                                break;
                            }
                        }

                        //Create the UserAUEC level.
                        //====================================================================
                        TreeNode treeNodeUserAUECs = new TreeNode("UserAUECs");
                        //Making the root node to bold by assigning it to the font object defined above. 
                        treeNodeUserAUECs.NodeFont = font;
                        NodeDetails userAUECsNode = new NodeDetails(NodeType.UserAUEC, int.MinValue, user.UserID, company.CompanyID, int.MinValue);
                        treeNodeUserAUECs.Tag = userAUECsNode;

                        AUECs userAUECs = RMAdminBusinessLogic.GetUserAUECs(user.UserID);
                        UserUIControls userUIControls = RMAdminBusinessLogic.GetUserUIControls(company.CompanyID, user.UserID);
                        if (userAUECs.Count > 0)
                        {
                            foreach (AUEC userAUEC in userAUECs)
                            {
                                //SK 20061009 REmoved Compliance Class
                                // New object of currency class.
                                //Currency currency = new Currency();
                                //// Assign to this object the data for a particular currencyID. 
                                //currency = RMAdminBusinessLogic.GetCurrency(userAUEC.Compliance.BaseCurrencyID);
                                //

                                // the string is assigned the asset name, underlying name , exchange name and currency symbol in concatenated form.
                                string Data = userAUEC.Asset.Name.ToString() + "/" + userAUEC.UnderLying.Name.ToString() + "/" + userAUEC.DisplayName.ToString() + "/" + userAUEC.Currency.CurrencyName.ToString();
                                //string Data = userAUEC.Asset.Name.ToString() + "/" + userAUEC.UnderLying.Name.ToString() + "/" + userAUEC.Exchange.DisplayName.ToString() + "/" + currency.CurrencySymbol.ToString();
                                int Value = userAUEC.AUECID;

                                TreeNode treeNodeUserAUEC = new TreeNode(Data);
                                NodeDetails auecNode = new NodeDetails(NodeType.UserAUEC, Value, user.UserID, company.CompanyID, int.MinValue);
                                treeNodeUserAUEC.Tag = auecNode;
                                foreach (UserUIControl userUIControl in userUIControls)
                                {
                                    if (userAUEC.AUECID == userUIControl.CompanyUserAUECID)
                                    {
                                        treeNodeUserAUEC.ForeColor = System.Drawing.Color.Navy;
                                        break;
                                    }
                                }

                                treeNodeUserAUECs.Nodes.Add(treeNodeUserAUEC);

                            }
                            treeNodeUser.Nodes.Add(treeNodeUserAUECs);
                        }


                        treeNodeUsers.Nodes.Add(treeNodeUser);
                    }

                    treeNodeCompany.Nodes.Add(treeNodeUsers);


                    //Create Account Accounts nodes.
                    // --------------------------------------------------------------------------

                    // Add label Account Accounts
                    TreeNode treeNodeAccountAccnts = new TreeNode("Accounts");
                    //Making the root node to bold by assigning it to the font object defined above. 
                    treeNodeAccountAccnts.NodeFont = font;
                    NodeDetails accountAccntNode = new NodeDetails(NodeType.FundAccount, int.MinValue, company.CompanyID);
                    treeNodeAccountAccnts.Tag = accountAccntNode;

                    //GetRMTradingAccnts method is used to fetch the existing Trading Accounts in the database for a given RM company.
                    Accounts accounts = RMAdminBusinessLogic.GetCompanyaccounts(company.CompanyID);
                    RMFundAccounts rMFundAccounts = RMAdminBusinessLogic.GetRMFundAccounts(company.CompanyID);
                    foreach (Account account in accounts)
                    {
                        TreeNode treeNodeAccountAccnt = new TreeNode(account.AccountShortName);
                        NodeDetails rMaccountAccntNode = new NodeDetails(NodeType.FundAccount, account.AccountID, company.CompanyID);
                        treeNodeAccountAccnt.Tag = rMaccountAccntNode;

                        foreach (RMFundAccount rMFundAccount in rMFundAccounts)
                        {
                            if (account.AccountID == rMFundAccount.CompanyFundAccntID)
                            {
                                treeNodeAccountAccnt.ForeColor = System.Drawing.Color.Navy;
                                break;
                            }
                        }
                        treeNodeAccountAccnts.Nodes.Add(treeNodeAccountAccnt);
                    }

                    treeNodeCompany.Nodes.Add(treeNodeAccountAccnts);


                    //Create company clients' nodes.
                    // --------------------------------------------------------------------------

                    //Add label Clients
                    TreeNode treeNodeClients = new TreeNode("Clients");
                    //Making the root node to bold by assigning it to the font object defined above. 
                    treeNodeClients.NodeFont = font;
                    NodeDetails clientsNode = new NodeDetails(NodeType.Clients, int.MinValue, company.CompanyID);
                    treeNodeClients.Tag = clientsNode;

                    //GetClients method is used to fetch the existing Clients in the database to a given company.
                    Prana.Admin.BLL.CompanyClients companyClients = RMAdminBusinessLogic.GetCompanyClients(company.CompanyID);
                    ClientOverallLimits clientOverallLimits = RMAdminBusinessLogic.GetClientOverallLimits(company.CompanyID);

                    foreach (CompanyClient client in companyClients)
                    {
                        TreeNode treeNodeClient = new TreeNode(client.Name);
                        NodeDetails clientNode = new NodeDetails(NodeType.Clients, client.CompanyClientID, company.CompanyID);
                        treeNodeClient.Tag = clientNode;
                        foreach (ClientOverallLimit clientOverallLimit in clientOverallLimits)
                        {
                            if (client.CompanyClientID == clientOverallLimit.ClientID)
                            {
                                treeNodeClient.ForeColor = System.Drawing.Color.Navy;
                                break;
                            }
                        }
                        treeNodeClients.Nodes.Add(treeNodeClient);
                    }

                    treeNodeCompany.Nodes.Add(treeNodeClients);
                    // --------------------------------------------------------------------------

                    treeNodeCompanies.Nodes.Add(treeNodeCompany);
                }
                //}
                trvRM.Nodes.Add(treeNodeCompanies);

                trvRM.TopNode.Expand();

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void SetUp()
        {
            try
            {
                BindRMTree();
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        #endregion Binding tree

        #region NodeDetails

        /// <summary>
        /// Creating class NodeDetail to be used for the purpose of tree giving it some methods & properties.
        /// </summary>
        class NodeDetails
        {
            private NodeType _type = NodeType.Company;
            private int _nodeID = int.MinValue;
            private int _companyID = int.MinValue;
            private int _companyUserID = int.MinValue;
            private int _clientID = int.MinValue;
            private int _companyAUECID = int.MinValue;
            private int _tradingAccountID = int.MinValue;
            private int _fundAccountID = int.MinValue;
            private int _tradingAccountUserID = int.MinValue;
            private int _userAUECID = int.MinValue;

            public NodeDetails()
            {
            }

            public NodeDetails(NodeType type, int nodeID)
            {
                _type = type;
                _nodeID = nodeID;
            }

            public NodeDetails(NodeType type, int nodeID, int companyID)
            {
                _type = type;
                _nodeID = nodeID;
                _companyID = companyID;
            }
            public NodeDetails(NodeType type, int nodeID, int tradingAccountID, int companyID)
            {
                _type = type;
                _nodeID = nodeID;
                _tradingAccountID = tradingAccountID;
                _companyID = companyID;
            }
            public NodeDetails(NodeType type, int nodeID, int companyUserID, int companyID, int userauecID)
            {
                _type = type;
                _nodeID = nodeID;
                _companyUserID = companyUserID;
                _companyID = companyID;
                _userAUECID = userauecID;

            }
            public NodeType Type
            {
                get { return _type; }
                set { _type = value; }
            }
            public int NodeID
            {
                get { return _nodeID; }
                set { _nodeID = value; }
            }
            public int CompanyID
            {
                get { return _companyID; }
                set { _companyID = value; }
            }
            public int CompanyUserID
            {
                get { return _companyUserID; }
                set { _companyUserID = value; }
            }
            public int ClientID
            {
                get { return _clientID; }
                set { _clientID = value; }
            }
            public int CompanyAUECID
            {
                get { return _companyAUECID; }
                set { _companyAUECID = value; }
            }
            public int TradingAccountID
            {
                get { return _tradingAccountID; }
                set { _tradingAccountID = value; }
            }
            public int FundAccountID
            {
                get { return _fundAccountID; }
                set { _fundAccountID = value; }
            }
            public int TradingAccountUserID
            {
                get { return _tradingAccountUserID; }
                set { _tradingAccountUserID = value; }
            }
            public int UserAUECID
            {
                get { return _userAUECID; }
                set { _userAUECID = value; }
            }
        }

        //Creating enumeration to be used to distinguish tree nodetype on the basis of Company/Users/Clients
        enum NodeType
        {
            Company = 0,
            CompanyAUECs = 1,
            TradingAccount = 2,
            TradingAccountUser = 3,
            Users = 4,
            UserAUEC = 5,
            FundAccount = 6,
            Clients = 7

        }

        #endregion

        #region TabSelect Selected Changed event

        /// <summary>
        /// This is to allow selection of corresponding node in tree for each selected tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbcRMAdmin_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {

                // to check whether the some node is selected or not.
                if (trvRM.SelectedNode != null)
                {
                    // To assign the tg details of the selected node to the instance nodeDetails.
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;

                    // If the selected node is the root node of the tree, nothing happens, as the tabs are disabled. 
                    if (nodeDetails.NodeID == int.MinValue && nodeDetails.CompanyID == int.MinValue)
                    {
                        return;
                    }
                    else
                    {
                        // If the selected node is the parent node at any level other than the root node of the tree.
                        if (nodeDetails.NodeID == int.MinValue)
                        {
                            // To set the selected node as Company Node, when the company tab is selected.
                            if ((nodeDetails.Type != NodeType.Company) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_COMPANY])
                            {
                                if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                {
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent;
                                }
                                else
                                {
                                    //The company node will always be the parent node at any level.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent;
                                }

                            }
                            // To set the selected node as AUEC Node, when the RM AUEC tab is selected.
                            if ((nodeDetails.Type != NodeType.CompanyAUECs) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_RMAUEC])
                            {
                                if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                {
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Nodes[0]; ;
                                }
                                else
                                {
                                    // The RMAUEc node will be the first child node of Parent Node i.e Company Node.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Nodes[0];
                                }
                            }
                            //To set the selected node as Trading Account Node, when the Trading Account tab is selected.
                            if ((nodeDetails.Type != NodeType.TradingAccount) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_TRADINGACCOUNT])
                            {
                                if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                {
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Nodes[1]; ;
                                }
                                else
                                {
                                    // The Trading Account node is the 2nd child node of the Company Node.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Nodes[1];
                                }
                            }
                            //To set the selected node as Users Node, when the UserLevel tab is selected.
                            if ((nodeDetails.Type != NodeType.Users) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_USERLEVEL])
                            {
                                if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                {
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Nodes[2]; ;
                                }
                                else
                                {
                                    // Users is the 3rd Child node of the company node. 
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Nodes[2];
                                }
                            }
                            //To set the selected node as account Account Node, when the Account Account tab is selected.
                            if ((nodeDetails.Type != NodeType.FundAccount) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_FUNDACCOUNT])
                            {
                                if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                {
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Nodes[3]; ;
                                }
                                else
                                {

                                    // Account Account is the 4rth child node of company Node.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Nodes[3];
                                }
                            }
                            //To set the selected node as Clients Node, when the clients tab is selected.
                            if ((nodeDetails.Type != NodeType.Clients) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_CLIENT])
                            {
                                if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                {
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Nodes[4]; ;
                                }
                                else
                                {
                                    //Clients is the 5th child node of the company Node.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Nodes[4];
                                }
                            }
                        }
                        else
                        {
                            // If the selected node is Company Node.
                            if (nodeDetails.NodeID == nodeDetails.CompanyID)
                            {
                                // To set the Selected Node to RM AUEC , if the selected tab is RMAUEC.
                                if ((nodeDetails.Type == NodeType.Company) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_RMAUEC])
                                {
                                    // the selected node is set to the first child node of the selected company.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[0];
                                }
                                // To set the Selected Node to Trading Account , if the selected tab is Trading Account.
                                if ((nodeDetails.Type == NodeType.Company) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_TRADINGACCOUNT])
                                {
                                    // the selected node is set to the 2nd child node of the selected company.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[1];
                                }
                                // To set the Selected Node to Users , if the selected tab is UserLevel.
                                if ((nodeDetails.Type == NodeType.Company) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_USERLEVEL])
                                {
                                    // the selected node is set to the 3rd child node of the selected company.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[2];
                                }
                                // To set the Selected Node to account Account , if the selected tab is Account Account.
                                if ((nodeDetails.Type == NodeType.Company) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_FUNDACCOUNT])
                                {
                                    // the selected node is set to the 4rth child node of the selected company.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[3];
                                }
                                // To set the Selected Node to Clients , if the selected tab is Clients.
                                if ((nodeDetails.Type == NodeType.Company) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_CLIENT])
                                {
                                    // the selected node is set to the 5th child node of the selected company.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[4];
                                }

                            }
                            else
                            {
                                // If the selected tab is Company Tab
                                if ((nodeDetails.Type != NodeType.Company) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_COMPANY])
                                {
                                    if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                    {
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Parent;
                                    }
                                    else
                                    {
                                        // the selected node is set to selected company.
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent;
                                    }
                                }
                                // if the selected tab is RM AUEC tab.
                                if ((nodeDetails.Type != NodeType.CompanyAUECs) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_RMAUEC])
                                {
                                    if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                    {
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Parent.Nodes[0];
                                    }
                                    else
                                    {
                                        //the selected node is set to AUEC child node of selected company.
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Nodes[0];
                                    }
                                }
                                // if the selected tab is Trading Account.
                                if ((nodeDetails.Type != NodeType.TradingAccount) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_TRADINGACCOUNT])
                                {
                                    if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                    {
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Parent.Nodes[1];
                                    }
                                    else
                                    {
                                        // the selected node is set to trading account node if the selected company.
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Nodes[1];
                                    }
                                }
                                // if the selected tab is userlevel
                                if ((nodeDetails.Type != NodeType.Users) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_USERLEVEL])
                                {
                                    if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                    {
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Parent.Nodes[2];
                                    }
                                    else
                                    {
                                        // the node is set to users node of selected company.
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Nodes[2];
                                    }
                                }
                                // if the selected tab is Account Account.
                                if ((nodeDetails.Type != NodeType.FundAccount) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_FUNDACCOUNT])
                                {
                                    if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                    {
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Parent.Nodes[3];
                                    }
                                    else
                                    {
                                        // the node is set to account account node of selected company.
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Nodes[3];
                                    }
                                }
                                //If the selected tab is Clients
                                if ((nodeDetails.Type != NodeType.Clients) && tbcRMAdmin.SelectedTab == tbcRMAdmin.Tabs[C_TAB_CLIENT])
                                {
                                    if ((nodeDetails.Type == NodeType.TradingAccountUser) || (nodeDetails.Type == NodeType.UserAUEC))
                                    {
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Parent.Nodes[4];
                                    }
                                    else
                                    {
                                        // the node is set to clients node of the company.
                                        trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Nodes[4];
                                    }
                                }

                            }
                        }
                    }

                    //if((nodeDetails.Type == NodeType.Users) && tbcUserLevel.ActiveTab == tbcUserLevel.Tabs[1])
                    //{
                    //    int aUECChk = uctUserLevelUIControls.SetUp();
                    //    if(aUECChk >0)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("No AUECs are available for the selected user! ");
                    //    }
                    //}

                }
            }

            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

        }

        #endregion TabSelect Selected Changed event

        #region ChildTab Select Changed event

        private void tbcTradingAccount_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            userTradingAccount.Enabled = true;
            try
            {
                // to check whether the some node is selected or not.
                if (trvRM.SelectedNode != null)
                {
                    // To assign the tg details of the selected node to the instance nodeDetails.
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;

                    // If the selected node is the root node of the tree, nothing happens, as the tabs are disabled. 
                    if (nodeDetails.NodeID == int.MinValue && nodeDetails.CompanyID == int.MinValue)
                    {
                        return;
                    }
                    else
                    {
                        if ((nodeDetails.Type == NodeType.TradingAccountUser) && tbcTradingAccount.SelectedTab == tbcTradingAccount.Tabs[0])
                        {
                            // The RMAUEc node will be the first child node of Parent Node i.e Company Node.
                            trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Nodes[1];
                        }
                        if ((nodeDetails.Type == NodeType.TradingAccount) && tbcTradingAccount.SelectedTab == tbcTradingAccount.Tabs[1])
                        {
                            if (nodeDetails.NodeID == int.MinValue)
                            {
                                if (trvRM.SelectedNode.Nodes[0].Nodes.Count > 0)
                                {
                                    // The RMAUEc node will be the first child node of Parent Node i.e Company Node.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[0].Nodes[0];
                                }
                                else
                                {
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[0];
                                    userTradingAccount.Enabled = false;
                                    MessageBox.Show(" Since, there are no Users for Trading Accounts for the company. Therefore, the tab is disabled. !");
                                }
                            }
                            else
                            {
                                //if (trvRM.SelectedNode.Parent.Nodes[0].Nodes.Count > 0)
                                if (trvRM.SelectedNode.Nodes.Count > 0)
                                {
                                    // The RMAUEc node will be the first child node of Parent Node i.e Company Node.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[0];
                                }
                                else
                                {
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Nodes[0];
                                    userTradingAccount.Enabled = false;
                                    MessageBox.Show(" Since, there are no Users for Trading Accounts for the company. Therefore, the tab is disabled. !");
                                }
                            }
                        }


                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void tbcUserLevel_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            uctUserLevelUIControls.Enabled = true;
            try
            {
                // to check whether the some node is selected or not.
                if (trvRM.SelectedNode != null)
                {
                    // To assign the tg details of the selected node to the instance nodeDetails.
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;

                    // If the selected node is the root node of the tree, nothing happens, as the tabs are disabled. 
                    if (nodeDetails.NodeID == int.MinValue && nodeDetails.CompanyID == int.MinValue)
                    {
                        return;
                    }
                    else
                    {
                        if ((nodeDetails.Type == NodeType.UserAUEC) && tbcUserLevel.SelectedTab == tbcUserLevel.Tabs[0])
                        {
                            // The RMAUEc node will be the first child node of Parent Node i.e Company Node.
                            trvRM.SelectedNode = trvRM.SelectedNode.Parent.Parent.Parent.Nodes[2];
                        }
                        if ((nodeDetails.Type == NodeType.Users) && tbcUserLevel.SelectedTab == tbcUserLevel.Tabs[1])
                        {
                            if (nodeDetails.NodeID == int.MinValue)
                            {
                                if (trvRM.SelectedNode.Nodes[0].Nodes.Count > 0)
                                {
                                    // The RMAUEc node will be the first child node of Parent Node i.e Company Node.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[0].Nodes[0];
                                }
                                else
                                {
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[0];
                                    userTradingAccount.Enabled = false;
                                    MessageBox.Show(" Since, there are no Users for Trading Accounts for the company. Therefore, the tab is disabled. !");
                                }
                            }
                            else
                            {
                                //if (trvRM.SelectedNode.Parent.Nodes[0].Nodes.Count > 0)
                                if (trvRM.SelectedNode.Nodes.Count > 0)
                                {
                                    // The RMAUEc node will be the first child node of Parent Node i.e Company Node.
                                    trvRM.SelectedNode = trvRM.SelectedNode.Nodes[0];
                                }
                                else
                                {
                                    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Nodes[0];
                                    uctUserLevelUIControls.Enabled = false;
                                    MessageBox.Show(" Since, there are no AUECs for the User. Therefore, the tab is disabled. !");
                                }
                            }
                            //if (trvRM.SelectedNode.Parent.Nodes[0].Nodes.Count > 0)
                            //{
                            //    // The RMAUEc node will be the first child node of Parent Node i.e Company Node.
                            //    trvRM.SelectedNode = trvRM.SelectedNode.Parent.Nodes[0].Nodes[0];
                            //}
                            //else
                            //{
                            //    uctUserLevelUIControls.Enabled = false;
                            //    MessageBox.Show("Since, there are no Users for the company. Therefore, the tab is disabled !");
                            //}
                        }


                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }


        #endregion

        #region Tree AfterSelect

        /// <summary>
		/// This event is used to allow the selection of corresponding tab for each selected node .
		/// Also, this displayes the data for the selected node in the controls on the tabpages.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trvRM_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            /* the instance nodeDetails is assigned the tag for the selected node i.e, all the necessary 
             details of the selected node. */


            NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;

            try
            {
                // First it is checked, whether the selected node is the Root node of the tree.
                // If this is true, the Tabs are disabled until the user selects a valid company node from the tree.
                if (nodeDetails.NodeID == int.MinValue && nodeDetails.CompanyID == int.MinValue)
                {
                    // Sets the selected tab to the CompanyTab in disabled mode.
                    tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["Company"];
                    tbcRMAdmin.Enabled = false;
                    ultraStatusBar.Text = "Please select a Company from the tree !";
                }
                else
                {
                    // If a valid company node is selected , then the tabs are enabled for user.
                    tbcRMAdmin.Enabled = true;

                    ultraStatusBar.Text = "The fields marked by '*' are mandatory.";
                    // The CompanyID(Parent NodeID) of the selected node is assigned to the integer companyID.
                    int companyID = nodeDetails.CompanyID;

                    // Further depending upon the type of node selected , the functions are performed.
                    switch (nodeDetails.Type)
                    {
                        // In case of selected node is Company type( as defined in NodeDetails class).
                        case NodeType.Company:

                            // to set the Company Tab as selected tab. 
                            tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["Company"];

                            // Pass the selected companyID to the user controls "CompanyOverallLimits" and "CompanyAlerts".
                            uctCompanyOverallLimits.CompanyID = companyID;
                            company_Alerts.CompanyID = companyID;

                            // GetCompanyOverallLimit method calld from RMAdminBusinessLogic , which fetches the details of OverallLmits
                            // of that particular companyID.And this fetched data from database is displayed on the UI.
                            CompanyOverallLimit companyOverallLimit = RMAdminBusinessLogic.GetCompanyOverallLimit(companyID);
                            uctCompanyOverallLimits.SetCompanyOverallLimit = companyOverallLimit;

                            //CompanyAlerts companyAlerts = RMAdminBusinessLogic.GetCompanyAlert(companyID);
                            //company_Alerts.SetCompanyAlerts = companyAlerts;

                            //DefaultAlert defaultAlert = RMAdminBusinessLogic.GetDefaultAlert(companyID);
                            //company_Alerts.SetDefaultAlert = defaultAlert;
                            break;


                        case NodeType.CompanyAUECs:
                            // To set the CompanyAUECs as selected tab.
                            tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["RMAUEC"];

                            rM_AUEC.Enabled = true;
                            btnAUECSave.Enabled = true;
                            CompanyOverallLimit companyOverallLimit1 = RMAdminBusinessLogic.GetCompanyOverallLimit(companyID);
                            uctCompanyOverallLimits.SetCompanyOverallLimit = companyOverallLimit1;
                            if (companyOverallLimit1.RMCompanyOverallLimitID > 0)
                            {
                                AUECs aUECsNo = RMAdminBusinessLogic.GetAllCompanyAUECs(companyID);
                                if (aUECsNo.Count > 0)
                                {
                                    // Passing the companyID to the CompanyAUECs UserControl.
                                    rM_AUEC.CompanyID = companyID;

                                    int companyAUECID = nodeDetails.NodeID;

                                    rM_AUEC.CompanyAUECID = companyAUECID;

                                    RMAUEC rMAUEC = RMAdminBusinessLogic.GetRM_AUEC(companyID, companyAUECID);
                                    rM_AUEC.SetRMAUEC = rMAUEC;
                                }
                                else
                                {
                                    rM_AUEC.Enabled = false;
                                    btnAUECSave.Enabled = false;
                                    MessageBox.Show("There are no permitted  AUECs for the company!");

                                }

                            }
                            else
                            {
                                rM_AUEC.Enabled = false;
                                btnAUECSave.Enabled = false;
                                MessageBox.Show("You must enter the RM settings for Company before setting at RMAUEC level !");
                            }
                            break;

                        //In case of selected node is TradingAccount type( as defined in NodeDetails class).
                        case NodeType.TradingAccount:
                            // To set the TradingAccount as selected tab.
                            tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["TradingAccount"];
                            tbcTradingAccount.SelectedTab = tbcTradingAccount.Tabs[0];

                            tradingAccount.Enabled = true;
                            userTradingAccount.Enabled = true;
                            btnTradAccntSave.Enabled = true;
                            tbcTradingAccount.Enabled = true;
                            CompanyOverallLimit companyOverallLimit2 = RMAdminBusinessLogic.GetCompanyOverallLimit(companyID);
                            uctCompanyOverallLimits.SetCompanyOverallLimit = companyOverallLimit2;
                            if (companyOverallLimit2.RMCompanyOverallLimitID > 0)
                            {
                                TradingAccounts tradingAccountsNo = RMAdminBusinessLogic.GetCompanyTradingAccnts(companyID);
                                if (tradingAccountsNo.Count > 0)
                                {
                                    // The companyID is passed to the Trading Account User Control.
                                    tradingAccount.CompanyID = companyID;

                                    int companyTradingAccountID = nodeDetails.NodeID;

                                    tradingAccount.CompanyTradingAccountID = companyTradingAccountID;

                                    RMTradingAccount rMTradingAccount = RMAdminBusinessLogic.GetRMTradingAccnt(companyID, companyTradingAccountID);
                                    tradingAccount.SetRMTradingAccount = rMTradingAccount;
                                }
                                else
                                {
                                    tradingAccount.Enabled = false;
                                    userTradingAccount.Enabled = false;
                                    btnTradAccntSave.Enabled = false;
                                    tbcTradingAccount.Enabled = false;
                                    MessageBox.Show("There are no permitted trading accounts for the company.!");

                                }
                            }
                            else
                            {
                                tradingAccount.Enabled = false;
                                userTradingAccount.Enabled = false;
                                btnTradAccntSave.Enabled = false;
                                tbcTradingAccount.Enabled = false;
                                MessageBox.Show("You must enter the RM settings for Company before setting at Trading Account Level!");
                            }

                            break;

                        case NodeType.TradingAccountUser:
                            tbcRMAdmin.SelectedTabChanged -= new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(tbcRMAdmin_SelectedTabChanged);
                            tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["TradingAccount"];
                            tbcTradingAccount.SelectedTab = tbcTradingAccount.Tabs[1];

                            //tbcTradingAccount.Tabs[2].t .tab .SelectedTab = tbcTradingAccount.Tabs[1];

                            //add the event handler here.
                            userTradingAccount.Enabled = true;
                            btnTradAccntSave.Enabled = true;
                            CompanyOverallLimit companyOverallLimit3 = RMAdminBusinessLogic.GetCompanyOverallLimit(companyID);
                            uctCompanyOverallLimits.SetCompanyOverallLimit = companyOverallLimit3;
                            if (companyOverallLimit3.RMCompanyOverallLimitID > 0)
                            {
                                userTradingAccount.CompanyID = companyID;
                                int tradingAccountID = nodeDetails.TradingAccountID;
                                userTradingAccount.TradingAccountID = tradingAccountID;

                                Users tradAccntUsers = RMAdminBusinessLogic.GetUsersforTradingAccount(tradingAccountID);
                                if (tradAccntUsers.Count > 0)
                                {
                                    int tradAccntUserID = nodeDetails.NodeID;
                                    userTradingAccount.UserID = tradAccntUserID;

                                    Prana.Admin.BLL.UserTradingAccount userTradAccnt = RMAdminBusinessLogic.GetUserTradingAccount(tradingAccountID, tradAccntUserID);
                                    userTradingAccount.SetUserTradingAccount = userTradAccnt;

                                }
                                else
                                {
                                    userTradingAccount.Enabled = false;
                                    btnTradAccntSave.Enabled = false;
                                    MessageBox.Show("There are no available Users for this TradingAccount !");
                                }


                            }
                            else
                            {
                                userTradingAccount.Enabled = false;
                                btnTradAccntSave.Enabled = false;
                                MessageBox.Show("You must enter the Company level settings before entering settings at TradingAccount User's level !");

                            }
                            tbcRMAdmin.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(tbcRMAdmin_SelectedTabChanged);

                            break;


                        case NodeType.Users:
                            // To set the Users as selected tab.
                            tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["UserLevel"];
                            tbcUserLevel.SelectedTab = tbcUserLevel.Tabs[0];

                            uctUserLevelOverallLimits.Enabled = true;
                            uctUserLevelUIControls.Enabled = true;
                            btnUserSave.Enabled = true;
                            tbcUserLevel.Enabled = true;
                            //Passing the selected companyID to the UserLevel.
                            uctUserLevelOverallLimits.CompanyID = companyID;

                            CompanyOverallLimit companyOverallLimit4 = RMAdminBusinessLogic.GetCompanyOverallLimit(companyID);
                            uctCompanyOverallLimits.SetCompanyOverallLimit = companyOverallLimit4;
                            if (companyOverallLimit4.RMCompanyOverallLimitID != int.MinValue)
                            {
                                Users users = RMAdminBusinessLogic.GetCompanyUsers(companyID);
                                int userID = nodeDetails.NodeID;
                                if (users.Count > 0)
                                {
                                    //int userID = nodeDetails.NodeID;
                                    uctUserLevelOverallLimits.CompanyUserID = userID;
                                    UserLevelOverallLimit userLevelOverallLimit = RMAdminBusinessLogic.GetUserLevelOverallLimit(companyID, userID);
                                    uctUserLevelOverallLimits.UserLevelOverallLimit = userLevelOverallLimit;
                                }
                                else
                                {
                                    //UserLevelOverallLimit userLevelOverallLimit = RMAdminBusinessLogic.GetUserLevelOverallLimit(companyID, userID);
                                    //uctUserLevelOverallLimits.UserLevelOverallLimit = userLevelOverallLimit;
                                    uctUserLevelOverallLimits.Enabled = false;
                                    uctUserLevelUIControls.Enabled = false;
                                    btnUserSave.Enabled = false;
                                    tbcUserLevel.Enabled = false;
                                    MessageBox.Show("There are no users for the company!");

                                }

                            }
                            else
                            {
                                uctUserLevelOverallLimits.Enabled = false;
                                uctUserLevelUIControls.Enabled = false;
                                btnUserSave.Enabled = false;
                                tbcUserLevel.Enabled = false;
                                MessageBox.Show("You must enter the settings for company level before settings for  UserOverallLimits level .!");
                            }

                            break;

                        case NodeType.UserAUEC:
                            tbcRMAdmin.SelectedTabChanged -= new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(tbcRMAdmin_SelectedTabChanged);

                            tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["UserLevel"];
                            tbcUserLevel.SelectedTab = tbcUserLevel.Tabs[1];

                            uctUserLevelUIControls.Enabled = true;
                            btnUserSave.Enabled = true;
                            CompanyOverallLimit companyOverallLimit5 = RMAdminBusinessLogic.GetCompanyOverallLimit(companyID);
                            uctCompanyOverallLimits.SetCompanyOverallLimit = companyOverallLimit5;
                            if (companyOverallLimit5.RMCompanyOverallLimitID > 0)
                            {
                                uctUserLevelUIControls.CompanyID = companyID;
                                int selectedUserID = nodeDetails.CompanyUserID;
                                uctUserLevelUIControls.CompanyUserID = selectedUserID;
                                AUECs userAUECs = RMAdminBusinessLogic.GetUserAUECs(selectedUserID);
                                if (userAUECs.Count > 0)
                                {
                                    int selectedAUECID = nodeDetails.NodeID;
                                    uctUserLevelUIControls.UserAUECID = selectedAUECID;

                                    Prana.Admin.BLL.UserUIControl userUIControl = RMAdminBusinessLogic.GetUserUIControl(companyID, selectedUserID, selectedAUECID);
                                    uctUserLevelUIControls.SetUserUIControl = userUIControl;
                                }
                                else
                                {
                                    uctUserLevelUIControls.Enabled = false;
                                    btnUserSave.Enabled = false;
                                    MessageBox.Show(" There are no available AUECs for the selected User.!");
                                }
                            }
                            else
                            {
                                uctUserLevelUIControls.Enabled = false;
                                btnUserSave.Enabled = false;
                                MessageBox.Show("You must enter the settings for companyLevel before settings at UserUI level.!");
                            }
                            tbcRMAdmin.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(tbcRMAdmin_SelectedTabChanged);
                            break;


                        case NodeType.FundAccount:
                            // To set the FundAccount as selected tab.
                            tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["FundAccount"];

                            fundAccount.Enabled = true;
                            btnAccountAccntSave.Enabled = true;
                            CompanyOverallLimit companyOverallLimit6 = RMAdminBusinessLogic.GetCompanyOverallLimit(companyID);
                            uctCompanyOverallLimits.SetCompanyOverallLimit = companyOverallLimit6;
                            if (companyOverallLimit6.RMCompanyOverallLimitID > 0)
                            {
                                Accounts accountsNo = RMAdminBusinessLogic.GetCompanyaccounts(companyID);
                                if (accountsNo.Count > 0)
                                {
                                    // Passing the companyID to the CompanyAccountAccnts UserControl.
                                    fundAccount.CompanyID = companyID;

                                    int companyAccountAccntID = nodeDetails.NodeID;

                                    fundAccount.CompanyFundAccountID = companyAccountAccntID;

                                    RMFundAccount rMFundAccount = RMAdminBusinessLogic.GetRMFundAccount(companyID, companyAccountAccntID);
                                    fundAccount.SetRMFundAccount = rMFundAccount;
                                }
                                else
                                {
                                    fundAccount.Enabled = false;
                                    btnAccountAccntSave.Enabled = false;
                                    MessageBox.Show("There are no permitted FundAccounts for the company.!");

                                }

                            }
                            else
                            {
                                fundAccount.Enabled = false;
                                btnAccountAccntSave.Enabled = false;
                                MessageBox.Show("You must enter the RM settings for Company before setting at RMFundAccount level !");
                            }
                            break;


                        case NodeType.Clients:
                            // To set the Clients as selected tab.
                            tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["Client"];

                            uctCompanyOverallLimits.CurrencyChanged += new CurrencyChangedHandler(uctClientOverallLimits.UpdateCurrencyText);

                            //The usercontrol and save button are set to enabled by default.
                            uctClientOverallLimits.Enabled = true;
                            btnClientSave.Enabled = true;

                            // The check is performed to check whether the companydetails exist before entering this level details.
                            CompanyOverallLimit companyOverallLimit7 = RMAdminBusinessLogic.GetCompanyOverallLimit(companyID);
                            uctCompanyOverallLimits.SetCompanyOverallLimit = companyOverallLimit7;
                            if (companyOverallLimit7.RMCompanyOverallLimitID > 0)
                            {
                                CompanyClients companyClientsNo = RMAdminBusinessLogic.GetCompanyClients(companyID);
                                if (companyClientsNo.Count > 0)
                                {
                                    // Passing the selected companyID to Clients UserControl.
                                    uctClientOverallLimits.CompanyID = companyID;
                                    // fetching the details of the nodeID selected.
                                    int companyClientID = nodeDetails.NodeID;
                                    //Assigning the selected companyClientID from the tree to the usercontrol.
                                    uctClientOverallLimits.CompanyClientID = companyClientID;
                                    // Getting the data for the selected client from the database.
                                    ClientOverallLimit clientOverallLimit = RiskClientManager.GetClientOverallLimit(companyID, companyClientID);
                                    //Setting the get data into the controls on usercontrol.
                                    uctClientOverallLimits.SetClientOverallLimit = clientOverallLimit;
                                }
                                else
                                {
                                    uctClientOverallLimits.Enabled = false;
                                    btnClientSave.Enabled = false;
                                    MessageBox.Show(" There are no clients for the company!");

                                }

                            }
                            else
                            {
                                //Incase, the company details for RM do not exist , the current selected tab usercontrol is set to disable.
                                uctClientOverallLimits.Enabled = false;
                                btnClientSave.Enabled = false;
                                MessageBox.Show(" You must enter the companySettings before setting the RM details at Client Level!");
                            }

                            break;
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        #endregion Tree AfterSelect

        #region Company Save Method

        /// <summary>
        /// Save Method to save the data entered in the Company Tabs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCompanySave_Click_1(object sender, System.EventArgs e)
        {
            try
            {

                if (trvRM.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
                    //	int companyId = nodeDetails.CompanyID;

                    bool IsOverallLimitsValidated = uctCompanyOverallLimits.ValidateControl();
                    bool IsAlertsValidated = company_Alerts.ValidateControl();
                    bool IsDefaultValidated = company_Alerts.DefaultAlertsValidationControl();
                    if (IsOverallLimitsValidated)
                    {
                        if (IsAlertsValidated)
                        {
                            if (IsDefaultValidated)
                            {
                                int companyId = nodeDetails.CompanyID;



                                CompanyOverallLimit companyOverallLimit = new CompanyOverallLimit();
                                companyOverallLimit.CompanyID = nodeDetails.NodeID;
                                uctCompanyOverallLimits.SaveCompanyOverallLimit(companyOverallLimit, companyId);

                                CompanyAlert companyAlert = new CompanyAlert();
                                companyAlert.CompanyID = nodeDetails.NodeID;
                                company_Alerts.SaveCompanyAlerts(companyAlert, companyId);

                                DefaultAlert defaultAlert = new DefaultAlert();
                                defaultAlert.CompanyID = nodeDetails.NodeID;
                                company_Alerts.SaveDefaultAlert(defaultAlert, companyId);

                                BindRMTree();
                                int selCompanyID = int.Parse(nodeDetails.CompanyID.ToString());
                                NodeDetails selectNodeDetails = new NodeDetails(NodeType.Company, selCompanyID);
                                SelectTreeNode(selectNodeDetails);
                            }
                            else
                            {
                                tbcRMCompany.SelectedTab = tbcRMCompany.Tabs[1];
                            }

                        }
                        else
                        {
                            tbcRMCompany.SelectedTab = tbcRMCompany.Tabs[1];
                        }
                    }
                    else
                    {
                        tbcRMCompany.SelectedTab = tbcRMCompany.Tabs[0];
                    }
                }
            }

            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }


        #endregion Company Save Method

        #region SelectTreeNode Method
        /// <summary>
        /// This method selects the node in the tree based on the parameter passed to it in nodedetails. 
        /// </summary> 
        /// <param name="nodeDetails"></param>
        private void SelectTreeNode(NodeDetails nodeDetails)
        {
            try
            {

                this.trvRM.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.trvRM_AfterSelect);
                int compIndex = int.MinValue;
                //Selects the node based on the nodedetail type ie. company/user/client.
                switch (nodeDetails.Type)
                {
                    case NodeType.Company:
                        if (trvRM.Nodes[C_TAB_COMPANY].Nodes.Count > 0)
                        {
                            foreach (TreeNode node in trvRM.Nodes[C_TAB_COMPANY].Nodes)
                            {
                                if (((NodeDetails)node.Tag).NodeID == nodeDetails.NodeID)
                                {
                                    trvRM.SelectedNode = node;
                                    break;
                                }
                            }
                        }
                        break;

                    case NodeType.CompanyAUECs:
                        compIndex = -1;
                        foreach (TreeNode node in trvRM.Nodes[C_TAB_COMPANY].Nodes)
                        {
                            compIndex++;
                            if (((NodeDetails)node.Tag).CompanyID == nodeDetails.CompanyID)
                            {
                                if (trvRM.Nodes[C_TAB_COMPANYOVERALLLIMIT].Nodes[compIndex].Nodes[C_TAB_USERLEVELOVERALLLIMIT].Nodes.Count > 0)
                                {
                                    foreach (TreeNode rMAUECNode in trvRM.Nodes[C_TAB_COMPANY].Nodes[compIndex].Nodes[C_TAB_USERLEVELOVERALLLIMIT].Nodes)
                                    {
                                        if (((NodeDetails)rMAUECNode.Tag).NodeID == nodeDetails.NodeID)
                                        {
                                            trvRM.SelectedNode = rMAUECNode;
                                            break;
                                        }
                                        else if (nodeDetails.NodeID == int.MinValue)
                                        {
                                            trvRM.SelectedNode = node.FirstNode;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    trvRM.SelectedNode = node.FirstNode;
                                }
                            }
                        }
                        break;

                    case NodeType.TradingAccount:
                        compIndex = -1;
                        foreach (TreeNode node in trvRM.Nodes[C_TAB_COMPANY].Nodes)
                        {
                            compIndex++;
                            if (((NodeDetails)node.Tag).CompanyID == nodeDetails.CompanyID)
                            {
                                if (trvRM.Nodes[C_TAB_COMPANYOVERALLLIMIT].Nodes[compIndex].Nodes[1].Nodes.Count > 0)
                                {
                                    foreach (TreeNode tradAccntNode in trvRM.Nodes[C_TAB_COMPANY].Nodes[compIndex].Nodes[1].Nodes)
                                    {
                                        if (((NodeDetails)tradAccntNode.Tag).NodeID == nodeDetails.NodeID)
                                        {
                                            trvRM.SelectedNode = tradAccntNode;
                                            break;
                                        }
                                        else if (nodeDetails.NodeID == int.MinValue)
                                        {
                                            trvRM.SelectedNode = node.Nodes[1];
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    trvRM.SelectedNode = node.Nodes[1];
                                }
                            }
                        }
                        break;

                    case NodeType.TradingAccountUser:
                        compIndex = -1;
                        foreach (TreeNode node in trvRM.Nodes[C_TAB_COMPANY].Nodes)
                        {
                            compIndex++;
                            if (((NodeDetails)node.Tag).CompanyID == nodeDetails.CompanyID)
                            {
                                if (trvRM.Nodes[C_TAB_COMPANYOVERALLLIMIT].Nodes[compIndex].Nodes[1].Nodes.Count > 0)
                                {
                                    int nodeCount = -1;
                                    foreach (TreeNode tradAccntNode in trvRM.Nodes[C_TAB_COMPANY].Nodes[compIndex].Nodes[1].Nodes)
                                    {
                                        nodeCount++;
                                        if (((NodeDetails)tradAccntNode.Tag).TradingAccountID == nodeDetails.TradingAccountID)
                                        {
                                            if (trvRM.Nodes[0].Nodes[compIndex].Nodes[1].Nodes[nodeCount].Nodes[0].Nodes.Count > 0)
                                            {
                                                foreach (TreeNode tradAccntUserNode in trvRM.Nodes[0].Nodes[compIndex].Nodes[1].Nodes[nodeCount].Nodes[0].Nodes)
                                                {
                                                    if (((NodeDetails)tradAccntUserNode.Tag).NodeID == nodeDetails.NodeID)
                                                    {
                                                        trvRM.SelectedNode = tradAccntUserNode;
                                                        break;
                                                    }
                                                    else if (nodeDetails.NodeID == int.MinValue)
                                                    {
                                                        trvRM.SelectedNode = node.Nodes[1].Nodes[0].Nodes[0];
                                                        break;
                                                    }

                                                }
                                            }

                                        }

                                    }
                                }
                                else
                                {
                                    trvRM.SelectedNode = node.Nodes[1].Nodes[0].Nodes[0];
                                }
                            }
                        }
                        break;
                    case NodeType.Users:
                        compIndex = -1;
                        foreach (TreeNode node in trvRM.Nodes[C_TAB_COMPANY].Nodes)
                        {
                            compIndex++;
                            if (((NodeDetails)node.Tag).CompanyID == nodeDetails.CompanyID)
                            {
                                if (trvRM.Nodes[C_TAB_COMPANYOVERALLLIMIT].Nodes[compIndex].Nodes[2].Nodes.Count > 0)
                                {
                                    foreach (TreeNode userNode in trvRM.Nodes[C_TAB_COMPANY].Nodes[compIndex].Nodes[2].Nodes)
                                    {
                                        if (((NodeDetails)userNode.Tag).NodeID == nodeDetails.NodeID)
                                        {
                                            trvRM.SelectedNode = userNode;
                                            break;
                                        }
                                        else if (nodeDetails.NodeID == int.MinValue)
                                        {
                                            trvRM.SelectedNode = node.Nodes[2];
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    trvRM.SelectedNode = node.Nodes[2];
                                }

                            }
                            break;

                        }
                        break;

                    case NodeType.UserAUEC:
                        compIndex = -1;
                        foreach (TreeNode node in trvRM.Nodes[C_TAB_COMPANY].Nodes)
                        {
                            compIndex++;
                            if (((NodeDetails)node.Tag).CompanyID == nodeDetails.CompanyID)
                            {
                                if (trvRM.Nodes[C_TAB_COMPANYOVERALLLIMIT].Nodes[compIndex].Nodes[2].Nodes.Count > 0)
                                {
                                    int nodeCount = -1;
                                    foreach (TreeNode tradAccntNode in trvRM.Nodes[C_TAB_COMPANY].Nodes[compIndex].Nodes[2].Nodes)
                                    {
                                        nodeCount++;
                                        if (((NodeDetails)tradAccntNode.Tag).CompanyUserID == nodeDetails.CompanyUserID)
                                        {
                                            if (trvRM.Nodes[0].Nodes[compIndex].Nodes[2].Nodes[nodeCount].Nodes[0].Nodes.Count > 0)
                                            {
                                                foreach (TreeNode userAUECNode in trvRM.Nodes[0].Nodes[compIndex].Nodes[2].Nodes[nodeCount].Nodes[0].Nodes)
                                                {
                                                    if (((NodeDetails)userAUECNode.Tag).NodeID == nodeDetails.NodeID)
                                                    {
                                                        trvRM.SelectedNode = userAUECNode;
                                                        break;
                                                    }
                                                    else if (nodeDetails.NodeID == int.MinValue)
                                                    {
                                                        trvRM.SelectedNode = node.Nodes[2].Nodes[0].Nodes[0];
                                                        break;
                                                    }

                                                }
                                            }

                                        }


                                    }
                                }
                                else
                                {
                                    trvRM.SelectedNode = node.Nodes[2].Nodes[0].Nodes[0];
                                }
                            }

                        }
                        break;

                    case NodeType.FundAccount:
                        compIndex = -1;
                        foreach (TreeNode node in trvRM.Nodes[C_TAB_COMPANY].Nodes)
                        {
                            compIndex++;
                            if (((NodeDetails)node.Tag).CompanyID == nodeDetails.CompanyID)
                            {
                                if (trvRM.Nodes[C_TAB_COMPANYOVERALLLIMIT].Nodes[compIndex].Nodes[3].Nodes.Count > 0)
                                {
                                    foreach (TreeNode accountAccntNode in trvRM.Nodes[C_TAB_COMPANY].Nodes[compIndex].Nodes[3].Nodes)
                                    {
                                        if (((NodeDetails)accountAccntNode.Tag).NodeID == nodeDetails.NodeID)
                                        {
                                            trvRM.SelectedNode = accountAccntNode;
                                            break;
                                        }
                                        else if (nodeDetails.NodeID == int.MinValue)
                                        {
                                            trvRM.SelectedNode = node.Nodes[3];
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    trvRM.SelectedNode = node.Nodes[3];
                                }
                            }
                        }
                        break;

                    case NodeType.Clients:
                        compIndex = -1;
                        foreach (TreeNode node in trvRM.Nodes[C_TAB_COMPANY].Nodes)
                        {
                            compIndex++;
                            if (((NodeDetails)node.Tag).CompanyID == nodeDetails.CompanyID)
                            {
                                if (trvRM.Nodes[C_TAB_COMPANYOVERALLLIMIT].Nodes[compIndex].Nodes[4].Nodes.Count > 0)
                                {
                                    foreach (TreeNode clientsNode in trvRM.Nodes[C_TAB_COMPANY].Nodes[compIndex].Nodes[4].Nodes)
                                    {
                                        if (((NodeDetails)clientsNode.Tag).NodeID == nodeDetails.NodeID)
                                        {
                                            trvRM.SelectedNode = clientsNode;
                                            break;
                                        }
                                        else if (nodeDetails.NodeID == int.MinValue)
                                        {
                                            trvRM.SelectedNode = node.Nodes[4];
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    trvRM.SelectedNode = node.Nodes[4];
                                }
                            }
                        }
                        break;

                }
                this.trvRM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvRM_AfterSelect);
            }
            #region Catch
            catch (Exception)
            {
                //bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                //if (rethrow)
                //{
                throw;
                //}
            }
            #endregion

        }
        #endregion SelectTreeNode

        #region Form Load

        private void RM_Load(object sender, System.EventArgs e)
        {
            SetUp();

            //Attached event handler to Company Overalllimit for RMBaseCurrency Labels
            uctCompanyOverallLimits.CurrencyChanged += new CurrencyChangedHandler(uctUserLevelOverallLimits.UpdateCurrencyText);
            uctCompanyOverallLimits.CurrencyChanged += new CurrencyChangedHandler(uctUserLevelUIControls.UpdateCurrencyText);
            uctCompanyOverallLimits.CurrencyChanged += new CurrencyChangedHandler(uctClientOverallLimits.UpdateCurrencyText);
            uctCompanyOverallLimits.CurrencyChanged += new CurrencyChangedHandler(rM_AUEC.UpdateCurrencyText);
            uctCompanyOverallLimits.CurrencyChanged += new CurrencyChangedHandler(tradingAccount.UpdateCurrencyText);
            uctCompanyOverallLimits.CurrencyChanged += new CurrencyChangedHandler(userTradingAccount.UpdateCurrencyText);
            uctCompanyOverallLimits.CurrencyChanged += new CurrencyChangedHandler(fundAccount.UpdateCurrencyText);

            //the event handlers to select the corresponding nodes in tree.
            rM_AUEC.RMAUECChanged += new RMAUECChangedHandler(rM_AUEC_RMAUECChanged);
            tradingAccount.RMTAChanged += new RMTradAccntChangedHandler(tradingAccount_RMTAChanged);
            userTradingAccount.RMUTAChangedHandled += new RMUserTAChangedHandler(userTradingAccount_RMUTAChangedHandled);
            uctUserLevelOverallLimits.UserChanged += new UserChangedHandler(this.uctUserLevelOverallLimits_UserChanged);
            uctUserLevelUIControls.RMUserAUECChanged += new RMUserAUECChangedHandler(uctUserLevelUIControls_RMUserAUECChanged);
            fundAccount.RMFAChanged += new RMAccountAccntChangedHandler(fundAccount_RMFAChanged);
            uctClientOverallLimits.ClientChanged += new ClientChangedHandler(this.uctClientOverallLimits_ClientChanged);


        }

        #endregion Form Load

        #region UserLevel Save Method

        /// <summary>
        /// Save Click function for UserLevel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUserSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (trvRM.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;


                    bool IsDataEntered = uctUserLevelOverallLimits.CheckforInputData();
                    if (IsDataEntered)
                    {
                        int companyId = nodeDetails.CompanyID;
                        UserLevelOverallLimit userLevelOverallLimit = new UserLevelOverallLimit();
                        userLevelOverallLimit.CompanyID = nodeDetails.CompanyID;
                        int userID = uctUserLevelOverallLimits.SaveUserLevelOverallLimit(userLevelOverallLimit, companyId);
                        if (userID > 0)
                        {
                            BindRMTree();
                            //int selCompanyID = int.Parse(nodeDetails.CompanyID.ToString());
                            NodeDetails selectNodeDetails = new NodeDetails(NodeType.Users, userID, companyId);
                            SelectTreeNode(selectNodeDetails);

                        }
                        else
                        {
                            tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["UserLevel"];
                        }


                    }


                }

            }

            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void btnUserUISave_Click(object sender, EventArgs e)
        {
            try
            {
                if (trvRM.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;

                    bool IsDataInput = uctUserLevelUIControls.CheckforInputData();
                    if (IsDataInput)
                    {
                        int companyId = nodeDetails.CompanyID;
                        UserUIControl userUIControl = new UserUIControl();
                        userUIControl.CompanyID = nodeDetails.CompanyID;

                        int auecID = uctUserLevelUIControls.SaveUserUIControl(userUIControl, companyId);
                        if (auecID > 0)
                        {
                            BindRMTree();
                            int selCompanyUserID = int.Parse(nodeDetails.CompanyUserID.ToString());
                            NodeDetails selectNodeDetails = new NodeDetails(NodeType.UserAUEC, auecID, selCompanyUserID, companyId);
                            SelectTreeNode(selectNodeDetails);

                        }
                        else
                        {
                            tbcUserLevel.SelectedTab = tbcUserLevel.Tabs[1];
                        }


                    }

                }
            }

            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        #endregion UserLevel Save Method 

        #region Passing the value of user and client combo to selecttree

        /// <summary>
        /// event to pass the value of the selected user in combo for tree node selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uctUserLevelOverallLimits_UserChanged(System.Object sender, ValueEventArgs e)
        {
            NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
            int companyID = nodeDetails.CompanyID;
            int companyUserID = int.Parse(e.companyUserID.ToString());
            NodeDetails selectNodeDetal = new NodeDetails(NodeType.Users, companyUserID, companyID);
            SelectTreeNode(selectNodeDetal);
        }

        private void uctClientOverallLimits_ClientChanged(Object sender, ClientIDValueEventArgs e)
        {
            NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
            int companyID = nodeDetails.CompanyID;
            int companyClientID = int.Parse(e.companyClientID.ToString());
            NodeDetails selectNodeDetail = new NodeDetails(NodeType.Clients, companyClientID, companyID);
            SelectTreeNode(selectNodeDetail);
        }

        private void rM_AUEC_RMAUECChanged(object sender, AUECValueEventArgs e)
        {
            NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
            int companyID = nodeDetails.CompanyID;
            int companyAUECID = int.Parse(e.companyAUECID.ToString());
            NodeDetails selectNodeDetal = new NodeDetails(NodeType.CompanyAUECs, companyAUECID, companyID);
            SelectTreeNode(selectNodeDetal);
        }

        private void tradingAccount_RMTAChanged(object sender, TAValueEventArgs e)
        {
            NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
            int companyID = nodeDetails.CompanyID;
            int companyTAID = int.Parse(e.companyTradingAccntID.ToString());
            NodeDetails selectNodeDetal = new NodeDetails(NodeType.TradingAccount, companyTAID, companyID);
            SelectTreeNode(selectNodeDetal);
        }


        private void fundAccount_RMFAChanged(object sender, FAValueEventArgs e)
        {
            NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
            int companyID = nodeDetails.CompanyID;
            int companyFAID = int.Parse(e.companyFundAccntID.ToString());
            NodeDetails selectNodeDetal = new NodeDetails(NodeType.FundAccount, companyFAID, companyID);
            SelectTreeNode(selectNodeDetal);
        }

        private void uctUserLevelUIControls_RMUserAUECChanged(object sender, UserAUECValueEventArgs e)
        {
            NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
            int companyID = nodeDetails.CompanyID;
            int companyUserID = nodeDetails.CompanyUserID;
            int companyAUECID = int.Parse(e.companyUserAUECID.ToString());
            NodeDetails selectNodeDetal = new NodeDetails(NodeType.UserAUEC, companyAUECID, companyUserID, companyID, companyAUECID);
            SelectTreeNode(selectNodeDetal);
        }

        private void userTradingAccount_RMUTAChangedHandled(object sender, UTAValueEventArgs e)
        {
            NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
            int companyID = nodeDetails.CompanyID;
            int companyTAID = nodeDetails.TradingAccountID;
            int companyTAUserID = int.Parse(e.companyUserID.ToString());
            NodeDetails selectNodeDetal = new NodeDetails(NodeType.TradingAccountUser, companyTAUserID, companyTAID, companyID);
            SelectTreeNode(selectNodeDetal);
        }
        #endregion Passing the value of user combo to selecttree

        #region Client Save Method

        private void btnClientSave_Click_1(object sender, System.EventArgs e)
        {
            try
            {
                if (trvRM.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;

                    //bool IsClientOverallValidated = uctClientOverallLimits.ValidationControl();
                    //if(IsClientOverallValidated)
                    //{
                    int companyId = nodeDetails.CompanyID;
                    ClientOverallLimit clientOverallLimit = new ClientOverallLimit();
                    clientOverallLimit.CompanyID = nodeDetails.CompanyID;
                    int clientID = uctClientOverallLimits.SaveClientOverallLimit(clientOverallLimit, companyId);
                    if (clientID > 0)
                    {
                        BindRMTree();
                        int selCompanyID = int.Parse(nodeDetails.CompanyID.ToString());
                        NodeDetails selectNodeDetails = new NodeDetails(NodeType.Clients, clientID, selCompanyID);
                        SelectTreeNode(selectNodeDetails);

                        //uctClientOverallLimits.BindClientGrid();
                    }
                    else
                    {
                        tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["Client"];
                    }
                }


            }

            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        #endregion Client Save Method

        #region RMAUEC Save Method 

        private void btnAUECSave_Click_1(object sender, System.EventArgs e)
        {
            try
            {
                if (trvRM.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
                    int companyId = nodeDetails.CompanyID;

                    RMAUEC rMAUEC = new RMAUEC();
                    rMAUEC.CompanyID = nodeDetails.CompanyID;
                    int companyAUECID = rM_AUEC.SaveRMAUEC(rMAUEC, companyId);
                    if (companyAUECID != int.MinValue)
                    {
                        BindRMTree();
                        int selCompanyID = int.Parse(nodeDetails.CompanyID.ToString());
                        NodeDetails selectNodeDetails = new NodeDetails(NodeType.CompanyAUECs, companyAUECID, selCompanyID);
                        SelectTreeNode(selectNodeDetails);
                    }
                    else
                    {

                        tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["RMAUEC"];
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        #endregion RMAUEC Save Method 

        #region Trading Account Save Method

        private void btnTradAccntSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (trvRM.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
                    if (tradingAccount.Enabled == true)
                    {
                        bool IsDataInput = tradingAccount.CheckForDataEntered();
                        if (IsDataInput)
                        {

                            int companyId = nodeDetails.CompanyID;
                            RMTradingAccount rMTradingAccount = new RMTradingAccount();
                            rMTradingAccount.CompanyID = nodeDetails.CompanyID;

                            int companyTradingAccntID = tradingAccount.SaveTradingAccnt(rMTradingAccount, companyId);

                            if (companyTradingAccntID > 0)
                            {
                                //BindRMTree();
                                int selCompanyID = int.Parse(nodeDetails.CompanyID.ToString());
                                NodeDetails selectNodeDetails = new NodeDetails(NodeType.TradingAccount, companyTradingAccntID, selCompanyID);
                                SelectTreeNode(selectNodeDetails);
                            }
                            else
                            {
                                tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["TradingAccount"];
                            }
                        }

                    }

                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void btnUserTradSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (trvRM.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
                    if (userTradingAccount.Enabled == true)
                    {
                        bool IsDataEntered = userTradingAccount.CheckForDataEntered();
                        if (IsDataEntered)
                        {
                            int companyId = nodeDetails.CompanyID;

                            Prana.Admin.BLL.UserTradingAccount userTradAccnt = new Prana.Admin.BLL.UserTradingAccount();
                            userTradAccnt.CompanyID = nodeDetails.CompanyID;
                            int companyTradingAccntUserID = userTradingAccount.SaveUserTradingDetails(userTradAccnt, companyId);

                            if (companyTradingAccntUserID > 0)
                            {
                                BindRMTree();
                                int selTradAccntID = int.Parse(nodeDetails.TradingAccountID.ToString());
                                NodeDetails selectNodeDetails = new NodeDetails(NodeType.TradingAccountUser, companyTradingAccntUserID, selTradAccntID, companyId);
                                SelectTreeNode(selectNodeDetails);
                            }
                            else
                            {
                                tbcTradingAccount.SelectedTab = tbcTradingAccount.Tabs[1];
                            }


                        }
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

        }

        #endregion Trading Account Save Method

        #region Save FundAccount Method
        private void btnAccountAccntSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (trvRM.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
                    int companyId = nodeDetails.CompanyID;
                    //bool IsAccountAccntValid = fundAccount.Validation();
                    //if (IsAccountAccntValid)
                    //{
                    RMFundAccount rMFundAccount = new RMFundAccount();
                    rMFundAccount.CompanyID = nodeDetails.CompanyID;
                    int companyAccountAccntID = fundAccount.SaveFundAccount(rMFundAccount, companyId);

                    if (companyAccountAccntID > 0)
                    {
                        BindRMTree();
                        int selCompanyID = int.Parse(nodeDetails.CompanyID.ToString());
                        NodeDetails selectNodeDetails = new NodeDetails(NodeType.FundAccount, companyAccountAccntID, selCompanyID);
                        SelectTreeNode(selectNodeDetails);

                    }
                    else
                    {

                        tbcRMAdmin.SelectedTab = tbcRMAdmin.Tabs["FundAccount"];
                    }
                }

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        #endregion Save FundAccount Method

        #region Close Button

        /// <summary>
        /// this event is to close the form on close button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnClose_Click_1(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnClose3_Click_1(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnClose2_Click_1(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void btnAUECClose_Click_1(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void btnTradAccntClose_Click_1(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void btnAccountAccntClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnClose5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnUserClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnTradAccntClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion  Close Button

        #region Delete Method

        private void btnDelete_Click_1(object sender, System.EventArgs e)
        {
            try
            {
                if (trvRM.SelectedNode == null)
                {
                    //Nothing is selected in Node tree.
                    MessageBox.Show("Please select Client/ClientAUEC/Trading Account/User/Account Account that you want to delete!");
                }
                else
                {
                    //Getting the info about the previous node so as to select it after deleting the currently selected node.
                    NodeDetails parentNodeDetails = new NodeDetails();
                    if (trvRM.SelectedNode.Parent != null)
                    {
                        //    prevNodeDetails = (NodeDetails)trvRM.SelectedNode.PrevNode.Tag;
                        //}
                        //else
                        //{

                        parentNodeDetails = (NodeDetails)trvRM.SelectedNode.Parent.Tag;
                    }

                    //Getting the info about the currently selected node to be deleted.
                    NodeDetails nodeDetails = (NodeDetails)trvRM.SelectedNode.Tag;
                    switch (nodeDetails.Type)
                    {
                        case NodeType.Company:
                            int companyID = nodeDetails.NodeID;
                            if (companyID != int.MinValue)
                            {
                                if (trvRM.SelectedNode.ForeColor == System.Drawing.Color.Navy)
                                {
                                    if (MessageBox.Show(this, "Are you sure you want to delete selected Company RM Details?", "RM ADMIN Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        //Deleting the selected company before checking it for reference in other 
                                        //nodes or forms.
                                        bool chkVarraible = RMAdminBusinessLogic.DeleteRMCompanyDetails(companyID);
                                        //CompanyManager.DeleteCompany(companyID, false);
                                        if (!(chkVarraible))
                                        {
                                            MessageBox.Show(this, "Company RM Details could not be deleted due to some system error. Please contact the system adminstrator.", "RM ADMIN Alert");
                                        }
                                        else
                                        {
                                            //Binding the tree after deleting the selected node and selecting its
                                            //previous node.
                                            // Use a method to redo teh color of tree nodes.....
                                            BindRMTree();
                                            SelectTreeNode(parentNodeDetails);
                                            trvRM_AfterSelect(this, null);
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Already no data exists for the selected entity.!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Select a valid company for deletion.");
                            }
                            break;

                        case NodeType.CompanyAUECs:
                            int companyAUECID = nodeDetails.NodeID;
                            if (companyAUECID != int.MinValue)
                            {
                                if (trvRM.SelectedNode.ForeColor == System.Drawing.Color.Navy)
                                {
                                    if (MessageBox.Show(this, " Are you sure you want to delete RM details for the selected AUEC ?", "RM ADMIN Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        bool IsDeleted = RMAdminBusinessLogic.DeleteSelectedAUECfromRM(nodeDetails.CompanyID, companyAUECID);
                                        //RiskUserLevelManager.DeleteCompanyUserforRM(nodeDetails.CompanyID, companyUserID, true);
                                        if (!IsDeleted)
                                        {
                                            MessageBox.Show(this, "AUEC RM Details could not be deleted due to some system error. Please contact the system adminstrator.", "RM ADMIN Alert");
                                        }
                                        else
                                        {
                                            //Binding the tree after deleting the selected node and selecting its
                                            //parent node.
                                            //RMAUEC rMAUEC = RMAdminBusinessLogic.GetRM_AUEC(nodeDetails.CompanyID, int.MinValue);
                                            //rM_AUEC.SetRMAUEC = rMAUEC;
                                            BindRMTree();
                                            SelectTreeNode(parentNodeDetails);
                                            trvRM_AfterSelect(this, null);

                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Already no data exists for the selected entity.!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Select a valid Company AUEC for deletion.");
                            }

                            break;

                        case NodeType.TradingAccount:
                            int companyTradAccntID = nodeDetails.NodeID;
                            if (companyTradAccntID != int.MinValue)
                            {
                                if (trvRM.SelectedNode.ForeColor == System.Drawing.Color.Navy)
                                {
                                    if (MessageBox.Show(this, " Are you sure you want to go ahead with deletion of the RM details of selected Trading Account ?", "RM ADMIN Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        bool IsDeleted = RMAdminBusinessLogic.DeleteSelectedTradingAccountfromRM(nodeDetails.CompanyID);

                                        if (!IsDeleted)
                                        {
                                            MessageBox.Show(this, "TradingAccounts RM Details could not be deleted due to some system error. Please contact the system adminstrator.", "RM ADMIN Alert");
                                        }
                                        else
                                        {
                                            //Binding the tree after deleting the selected node and selecting its
                                            //parent node.
                                            //RMTradingAccount rMTradingAccount = RMAdminBusinessLogic.GetRMTradingAccnt(nodeDetails.CompanyID, int.MinValue);
                                            //tradingAccount.SetRMTradingAccount = rMTradingAccount;
                                            BindRMTree();
                                            SelectTreeNode(parentNodeDetails);
                                            trvRM_AfterSelect(this, null);

                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Already no data exists for the selected entity.!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Select a valid Trading Account for deletion.");
                            }

                            break;

                        case NodeType.TradingAccountUser:
                            int companyTradAccntUserID = nodeDetails.NodeID;
                            if (companyTradAccntUserID != int.MinValue)
                            {
                                if (trvRM.SelectedNode.ForeColor == System.Drawing.Color.Navy)
                                {
                                    if (MessageBox.Show(this, " Are you sure you want to go ahead with deletion of the RM details of selected Trading Account's User ?", "RM ADMIN Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        bool IsDeleted = RMAdminBusinessLogic.DeleteRMTradAccntUserDetails(nodeDetails.TradingAccountID, companyTradAccntUserID);

                                        if (!IsDeleted)
                                        {
                                            MessageBox.Show(this, "TradingAccount's User RM Details could not be deleted due to some system error. Please contact the system adminstrator.", "RM ADMIN Alert");
                                        }
                                        else
                                        {
                                            //Binding the tree after deleting the selected node and selecting its
                                            //parent node.
                                            //RMTradingAccount rMTradingAccount = RMAdminBusinessLogic.GetRMTradingAccnt(nodeDetails.CompanyID, int.MinValue);
                                            //tradingAccount.SetRMTradingAccount = rMTradingAccount;
                                            BindRMTree();
                                            SelectTreeNode(parentNodeDetails);
                                            trvRM_AfterSelect(this, null);

                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Already no data exists for the selected entity.!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Select a valid Trading Account's User for deletion.");
                            }

                            break;

                        case NodeType.Users:
                            int companyUserID = nodeDetails.NodeID;
                            if (companyUserID != int.MinValue)
                            {
                                if (trvRM.SelectedNode.ForeColor == System.Drawing.Color.Navy)
                                {
                                    if (MessageBox.Show(this, "Are you sure that want to delete selected User RM Details?", "RM ADMIN Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        bool IsDeleted = RMAdminBusinessLogic.DeleteSelectedUserfromRM(nodeDetails.CompanyID, companyUserID);
                                        //RiskUserLevelManager.DeleteCompanyUserforRM(nodeDetails.CompanyID, companyUserID, true);
                                        if (!(IsDeleted))
                                        {
                                            MessageBox.Show(this, "UserLevel RM Details could not be deleted due to some system error. Please contact the system adminstrator.", "RM ADMIN Alert");
                                        }
                                        else
                                        {
                                            //Binding the tree after deleting the selected node and selecting its
                                            //parent node.
                                            //UserLevelOverallLimit userLevelOverallLimit = RMAdminBusinessLogic.GetUserLevelOverallLimit(nodeDetails.CompanyID, int.MinValue);
                                            //uctUserLevelOverallLimits.UserLevelOverallLimit = userLevelOverallLimit;

                                            BindRMTree();
                                            SelectTreeNode(parentNodeDetails);
                                            trvRM_AfterSelect(this, null);

                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Already no data exists for the selected entity.!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Select a valid Company User for deletion.");
                            }
                            break;

                        case NodeType.UserAUEC:
                            int userAUECID = nodeDetails.NodeID;
                            if (userAUECID != int.MinValue)
                            {
                                if (trvRM.SelectedNode.ForeColor == System.Drawing.Color.Navy)
                                {
                                    if (MessageBox.Show(this, " Are you sure you want to go ahead with deletion of the RM details of selected UserAUEC ?", "RM ADMIN Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        bool IsDeleted = RMAdminBusinessLogic.DeleteRMUserAUECDetails(nodeDetails.CompanyUserID, userAUECID);

                                        if (!IsDeleted)
                                        {
                                            MessageBox.Show(this, "User's AUEC RM Details could not be deleted due to some system error. Please contact the system adminstrator.", "RM ADMIN Alert");
                                        }
                                        else
                                        {
                                            //Binding the tree after deleting the selected node and selecting its
                                            //parent node.
                                            //RMTradingAccount rMTradingAccount = RMAdminBusinessLogic.GetRMTradingAccnt(nodeDetails.CompanyID, int.MinValue);
                                            //tradingAccount.SetRMTradingAccount = rMTradingAccount;
                                            BindRMTree();
                                            SelectTreeNode(parentNodeDetails);
                                            trvRM_AfterSelect(this, null);

                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Already no data exists for the selected entity.!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Select a valid User's AUEC for deletion.");
                            }

                            break;

                        case NodeType.FundAccount:
                            int companyAccountAccntID = nodeDetails.NodeID;
                            if (trvRM.SelectedNode.ForeColor == System.Drawing.Color.Navy)
                            {
                                if (MessageBox.Show(this, " Are you sure that you want to delete the RM details for the selected Account Account?", "RM ADMIN Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    bool IsDeleted = RMAdminBusinessLogic.DeleteSelectedAccountAccntfromRM(nodeDetails.CompanyID, companyAccountAccntID);

                                    if (!IsDeleted)
                                    {
                                        MessageBox.Show(this, "Account Account RM Details could not be deleted due to some system error. Please contact the system adminstrator.", "RM ADMIN Alert");

                                    }
                                    else
                                    {
                                        //Binding the tree after deleting the selected node and selecting its
                                        //parent node.
                                        //RMFundAccount rMFundAccount = RMAdminBusinessLogic.GetRMFundAccount(nodeDetails.CompanyID, int.MinValue);
                                        //fundAccount.SetRMFundAccount = rMFundAccount;
                                        BindRMTree();
                                        SelectTreeNode(parentNodeDetails);
                                        trvRM_AfterSelect(this, null);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Already no data exists for the selected entity.!");
                            }

                            break;

                        case NodeType.Clients:
                            int clientID = nodeDetails.NodeID;
                            if (trvRM.SelectedNode.ForeColor == System.Drawing.Color.Navy)
                            {
                                if (MessageBox.Show(this, "Are you sure u want to delete RM Details for the selected Client?", "RM ADMIN Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    //Deleting the selected Client before checking it for reference in other 
                                    //nodes or forms.
                                    bool chkVarraible = RMAdminBusinessLogic.DeleteSelectedClient(nodeDetails.CompanyID, clientID);

                                    if (!(chkVarraible))
                                    {
                                        MessageBox.Show(this, "Client's RM Details could not be deleted due to some system error. Please contact the system adminstrator.", "RM ADMIN Alert");

                                    }
                                    else
                                    {
                                        //Binding the tree after deleting the selected node and selecting its
                                        //parent node.
                                        //uctClientOverallLimits.RefreshClientDetails();
                                        BindRMTree();
                                        SelectTreeNode(parentNodeDetails);
                                        trvRM_AfterSelect(this, null);
                                        //uctClientOverallLimits.BindClientGrid();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Already no data exists for the selected entity.!");
                            }
                            break;

                    }

                }

            }

            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

        }


        #endregion Delete Method














    }
}