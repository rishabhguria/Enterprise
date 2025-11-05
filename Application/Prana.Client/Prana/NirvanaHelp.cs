using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana
{
    /// <summary>
    /// Summary description for PranaHelp.
    /// </summary>
    public class AboutPrana : System.Windows.Forms.Form
    {
        private Infragistics.Win.Misc.UltraButton button1;
        private Label lblpricingServer;
        private Label lblExpnlServer;
        private Label label9;
        private Label label7;
        private Label lblTradeServer;
        private Label label12;
        private Label lblLicensee;
        private Label label5;
        private Label label3;
        private Label label2;

        private Prana.Utilities.XMLUtilities.Config _config = new Prana.Utilities.XMLUtilities.Config();
        private Label lblAppver;
        private Label lblAppverText;
        private IContainer components;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Panel AboutPrana_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Bottom;
        private string _pranaUserName = string.Empty;
        private System.Windows.Forms.Label ultraUserAccounts;
        private System.Windows.Forms.Label ultraUserTradingAccounts;
        private System.Windows.Forms.Label accountsLabel;
        private System.Windows.Forms.Label tradingAccountsLabel;
        private ToolTip toolTip1;
        private int _userID;
        private string _accountsList = string.Empty;
        private string _accounts = string.Empty;

        public AboutPrana(string PranaUserName, int UserID)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            _pranaUserName = PranaUserName;
            _userID = UserID;
            LoadAboutBoxInformation();
        }

        private void LoadAboutBoxInformation()
        {
            _config.cfgFile = Application.StartupPath + "\\Prana.exe.config";

            lblTradeServer.Text = _config.GetValue("//appSettings//add[@key='TradeServer']");
            lblExpnlServer.Text = _config.GetValue("//appSettings//add[@key='ExpnlServer']");
            lblpricingServer.Text = _config.GetValue("//appSettings//add[@key='PricingServer']");
            //lblAppverText.Text = Prana.CommonDataCache.CachedDataManager.GetInstance.GetApplicationVersion();
            // Application version is now fetched from the file
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            lblAppverText.Text = fvi.FileMajorPart + "." + fvi.FileMinorPart + "." + fvi.FileBuildPart;

            string appCopyRight = ConfigurationHelper.Instance.GetAppSettingValueByKey("Copyright");

            //if value of config is like "® Copyright © 2009 to #currentyear#. All rights Reserved.", then remove year between # and replace it with corrent year.
            string currentYear = "#currentyear#";
            if (appCopyRight.ToLower().Contains(currentYear))
            {
                appCopyRight = Regex.Replace(appCopyRight, currentYear, DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
            }

            label2.Text = WhiteLabelTheme.AppTitle + appCopyRight;
            lblLicensee.Text = _pranaUserName;
            // Funtions called to show the list of CashAccounts & Trading CashAccounts of the login user on About Nirvana form,PRANA-11986
            FillTradingAccountCombo();
            FillAccountsCombo();

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
                if (button1 != null)
                {
                    button1.Dispose();
                }
                if (lblpricingServer != null)
                {
                    lblpricingServer.Dispose();
                }
                if (lblExpnlServer != null)
                {
                    lblExpnlServer.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (lblTradeServer != null)
                {
                    lblTradeServer.Dispose();
                }
                if (label12 != null)
                {
                    label12.Dispose();
                }
                if (lblLicensee != null)
                {
                    lblLicensee.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (lblAppver != null)
                {
                    lblAppver.Dispose();
                }
                if (lblAppverText != null)
                {
                    lblAppverText.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (AboutPrana_Fill_Panel != null)
                {
                    AboutPrana_Fill_Panel.Dispose();
                }
                if (_AboutPrana_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _AboutPrana_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
                if (_AboutPrana_UltraFormManager_Dock_Area_Left != null)
                {
                    _AboutPrana_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_AboutPrana_UltraFormManager_Dock_Area_Right != null)
                {
                    _AboutPrana_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_AboutPrana_UltraFormManager_Dock_Area_Top != null)
                {
                    _AboutPrana_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (ultraUserAccounts != null)
                {
                    ultraUserAccounts.Dispose();
                }
                if (ultraUserTradingAccounts != null)
                {
                    ultraUserTradingAccounts.Dispose();
                }
                if (accountsLabel != null)
                {
                    accountsLabel.Dispose();
                }
                if (tradingAccountsLabel != null)
                {
                    tradingAccountsLabel.Dispose();
                }
                if (toolTip1 != null)
                {
                    toolTip1.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutPrana));
            this.button1 = new Infragistics.Win.Misc.UltraButton();
            this.lblpricingServer = new System.Windows.Forms.Label();
            this.lblExpnlServer = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTradeServer = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblLicensee = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAppver = new System.Windows.Forms.Label();
            this.lblAppverText = new System.Windows.Forms.Label();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.AboutPrana_Fill_Panel = new System.Windows.Forms.Panel();
            this.accountsLabel = new System.Windows.Forms.Label();
            this.tradingAccountsLabel = new System.Windows.Forms.Label();
            this.ultraUserTradingAccounts = new System.Windows.Forms.Label();
            this.ultraUserAccounts = new System.Windows.Forms.Label();
            this._AboutPrana_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.AboutPrana_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(456, 331);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 21);
            this.button1.TabIndex = 11;
            this.button1.Text = "EXIT";
            this.button1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblpricingServer
            // 
            this.lblpricingServer.BackColor = System.Drawing.Color.Transparent;
            this.lblpricingServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblpricingServer.Location = new System.Drawing.Point(201, 194);
            this.lblpricingServer.Name = "lblpricingServer";
            this.lblpricingServer.Size = new System.Drawing.Size(140, 16);
            this.lblpricingServer.TabIndex = 37;
            this.lblpricingServer.Text = "Licensee";
            // 
            // lblExpnlServer
            // 
            this.lblExpnlServer.BackColor = System.Drawing.Color.Transparent;
            this.lblExpnlServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExpnlServer.Location = new System.Drawing.Point(201, 175);
            this.lblExpnlServer.Name = "lblExpnlServer";
            this.lblExpnlServer.Size = new System.Drawing.Size(140, 16);
            this.lblExpnlServer.TabIndex = 36;
            this.lblExpnlServer.Text = "Licensee";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(43, 175);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(154, 15);
            this.label9.TabIndex = 35;
            this.label9.Text = "Expnl Server :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(43, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(154, 15);
            this.label7.TabIndex = 34;
            this.label7.Text = "Pricing Server :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTradeServer
            // 
            this.lblTradeServer.BackColor = System.Drawing.Color.Transparent;
            this.lblTradeServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTradeServer.Location = new System.Drawing.Point(201, 156);
            this.lblTradeServer.Name = "lblTradeServer";
            this.lblTradeServer.Size = new System.Drawing.Size(140, 16);
            this.lblTradeServer.TabIndex = 33;
            this.lblTradeServer.Text = "Licensee";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(43, 156);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(154, 15);
            this.label12.TabIndex = 32;
            this.label12.Text = "Trade Server :";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblLicensee
            // 
            this.lblLicensee.BackColor = System.Drawing.Color.Transparent;
            this.lblLicensee.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicensee.Location = new System.Drawing.Point(201, 116);
            this.lblLicensee.Name = "lblLicensee";
            this.lblLicensee.Size = new System.Drawing.Size(140, 16);
            this.lblLicensee.TabIndex = 31;
            this.lblLicensee.Text = "Licensee";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 274);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(519, 69);
            this.label5.TabIndex = 30;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(43, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 15);
            this.label3.TabIndex = 29;
            this.label3.Text = "This product is licensed to: ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(41, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(489, 16);
            this.label2.TabIndex = 28;
            this.label2.Text = "Nirvana® Copyright © 2008 Nirvana Solutions. All rights Reserved.";
            // 
            // lblAppver
            // 
            this.lblAppver.BackColor = System.Drawing.Color.Transparent;
            this.lblAppver.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppver.Location = new System.Drawing.Point(43, 137);
            this.lblAppver.Name = "lblAppver";
            this.lblAppver.Size = new System.Drawing.Size(154, 15);
            this.lblAppver.TabIndex = 34;
            this.lblAppver.Text = "Application Ver:";
            this.lblAppver.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblAppverText
            // 
            this.lblAppverText.BackColor = System.Drawing.Color.Transparent;
            this.lblAppverText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppverText.Location = new System.Drawing.Point(201, 137);
            this.lblAppverText.Name = "lblAppverText";
            this.lblAppverText.Size = new System.Drawing.Size(140, 16);
            this.lblAppverText.TabIndex = 37;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // AboutPrana_Fill_Panel
            // 
            this.AboutPrana_Fill_Panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AboutPrana_Fill_Panel.Controls.Add(this.accountsLabel);
            this.AboutPrana_Fill_Panel.Controls.Add(this.tradingAccountsLabel);
            this.AboutPrana_Fill_Panel.Controls.Add(this.ultraUserTradingAccounts);
            this.AboutPrana_Fill_Panel.Controls.Add(this.ultraUserAccounts);
            this.AboutPrana_Fill_Panel.Controls.Add(this.button1);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblAppverText);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblpricingServer);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblExpnlServer);
            this.AboutPrana_Fill_Panel.Controls.Add(this.label9);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblAppver);
            this.AboutPrana_Fill_Panel.Controls.Add(this.label7);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblTradeServer);
            this.AboutPrana_Fill_Panel.Controls.Add(this.label12);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblLicensee);
            this.AboutPrana_Fill_Panel.Controls.Add(this.label5);
            this.AboutPrana_Fill_Panel.Controls.Add(this.label3);
            this.AboutPrana_Fill_Panel.Controls.Add(this.label2);
            this.AboutPrana_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AboutPrana_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutPrana_Fill_Panel.Location = new System.Drawing.Point(8, 31);
            this.AboutPrana_Fill_Panel.Name = "AboutPrana_Fill_Panel";
            this.AboutPrana_Fill_Panel.Size = new System.Drawing.Size(536, 373);
            this.AboutPrana_Fill_Panel.TabIndex = 0;
            // 
            // accountsLabel
            // 
            this.accountsLabel.BackColor = System.Drawing.Color.Transparent;
            this.accountsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountsLabel.Location = new System.Drawing.Point(43, 241);
            this.accountsLabel.Name = "accountsLabel";
            this.accountsLabel.Size = new System.Drawing.Size(154, 15);
            this.accountsLabel.TabIndex = 42;
            this.accountsLabel.Text = "Accounts :";
            this.accountsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tradingAccountsLabel
            // 
            this.tradingAccountsLabel.BackColor = System.Drawing.Color.Transparent;
            this.tradingAccountsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tradingAccountsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tradingAccountsLabel.Location = new System.Drawing.Point(43, 223);
            this.tradingAccountsLabel.Name = "tradingAccountsLabel";
            this.tradingAccountsLabel.Size = new System.Drawing.Size(154, 15);
            this.tradingAccountsLabel.TabIndex = 41;
            this.tradingAccountsLabel.Text = "Trading Accounts :";
            this.tradingAccountsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ultraUserTradingAccounts
            // 
            this.ultraUserTradingAccounts.BackColor = System.Drawing.Color.Transparent;
            this.ultraUserTradingAccounts.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraUserTradingAccounts.Location = new System.Drawing.Point(201, 223);
            this.ultraUserTradingAccounts.Name = "ultraUserTradingAccounts";
            this.ultraUserTradingAccounts.Size = new System.Drawing.Size(100, 18);
            this.ultraUserTradingAccounts.TabIndex = 40;
            this.ultraUserTradingAccounts.Text = "ultraLabel1";
            this.ultraUserTradingAccounts.MouseHover += new System.EventHandler(this.ultraUserTradingAccounts_MouseHover);
            // 
            // ultraUserAccounts
            // 
            this.ultraUserAccounts.BackColor = System.Drawing.Color.Transparent;
            this.ultraUserAccounts.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraUserAccounts.Location = new System.Drawing.Point(201, 242);
            this.ultraUserAccounts.Name = "ultraUserAccounts";
            this.ultraUserAccounts.Size = new System.Drawing.Size(110, 18);
            this.ultraUserAccounts.TabIndex = 39;
            this.ultraUserAccounts.Text = "ultraLabel1";
            this.ultraUserAccounts.MouseHover += new System.EventHandler(this.ultraUserAccounts_MouseHover);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Left
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._AboutPrana_UltraFormManager_Dock_Area_Left.Name = "_AboutPrana_UltraFormManager_Dock_Area_Left";
            this._AboutPrana_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 373);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Right
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(544, 31);
            this._AboutPrana_UltraFormManager_Dock_Area_Right.Name = "_AboutPrana_UltraFormManager_Dock_Area_Right";
            this._AboutPrana_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 373);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Top
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AboutPrana_UltraFormManager_Dock_Area_Top.Name = "_AboutPrana_UltraFormManager_Dock_Area_Top";
            this._AboutPrana_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(552, 31);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Bottom
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 404);
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.Name = "_AboutPrana_UltraFormManager_Dock_Area_Bottom";
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(552, 8);
            // 
            // AboutPrana
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(552, 412);
            this.Controls.Add(this.AboutPrana_Fill_Panel);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Bottom);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(552, 412);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(552, 412);
            this.Name = "AboutPrana";
            this.ShowInTaskbar = false;
            this.Text = "Software Details";
            this.Load += new System.EventHandler(this.PranaHelp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.AboutPrana_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void PranaHelp_Load(object sender, System.EventArgs e)
        {
            this.AboutPrana_Fill_Panel.BackgroundImage = WhiteLabelTheme.AboutBackGroundImage;
            this.Text = WhiteLabelTheme.AppTitle;
            this.Icon = WhiteLabelTheme.AppIcon;

            SetButtonsColor();
            CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_MISSING_TRADES);
            if (CustomThemeHelper.ApplyTheme)
            {
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Software Details", CustomThemeHelper.UsedFont);
            }
            this.lblpricingServer.ForeColor = System.Drawing.Color.White;
            this.lblExpnlServer.ForeColor = System.Drawing.Color.White;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.lblTradeServer.ForeColor = System.Drawing.Color.White;
            this.label12.ForeColor = System.Drawing.Color.White;
            this.lblLicensee.ForeColor = System.Drawing.Color.White;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.lblAppver.ForeColor = System.Drawing.Color.White;
            this.lblAppverText.ForeColor = System.Drawing.Color.White;
            this.ultraUserTradingAccounts.ForeColor = System.Drawing.Color.White;
            this.ultraUserAccounts.ForeColor = System.Drawing.Color.White;
            this.accountsLabel.ForeColor = System.Drawing.Color.White;
            this.tradingAccountsLabel.ForeColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// Used for changing the color of buttons. The indices and their colors are as follows:       
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                this.button1.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                this.button1.ForeColor = System.Drawing.Color.White;
                this.button1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                this.button1.UseAppStyling = false;
                this.button1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LicenseAggrement frmlicenseAgreement = new LicenseAggrement();
            frmlicenseAgreement.ShowDialog();
        }

        /// <summary>
        /// Function used to Fill the Account Label of the login user to show on About Nirvana Form,PRANA-11986
        /// </summary>
        private void FillTradingAccountCombo()
        {
            try
            {
                TradingAccountCollection _userTradingAccounts = Prana.CommonDataCache.WindsorContainerManager.GetTradingAccounts(_userID);

                List<String> list = new List<string>();

                foreach (Prana.BusinessObjects.TradingAccount tradingAccount in _userTradingAccounts)
                {
                    list.Add(tradingAccount.Name);
                }
                _accounts = string.Join(",", list);
                ultraUserTradingAccounts.Width = GetWidthOfLabel(_accounts);
                ultraUserTradingAccounts.Text = _accounts;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        ///  Function used to Fill the trading CashAccounts Label of the login user to show on About Nirvana Form,PRANA-11986
        /// </summary>
        private void FillAccountsCombo()
        {
            try
            {
                AccountCollection _userAccounts = Prana.CommonDataCache.WindsorContainerManager.GetAccounts(_userID);
                List<String> list = new List<string>();
                foreach (Prana.BusinessObjects.Account _accounts in _userAccounts)
                {
                    list.Add(_accounts.Name);
                }

                _accountsList = string.Join(",", list);
                ultraUserAccounts.Width = GetWidthOfLabel(_accountsList);
                ultraUserAccounts.Text = _accountsList;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To get the size of the label upto 2 comma in string
        /// </summary>
        /// <param name="inputString">Input String</param>
        /// <returns>width</returns>
        private int GetWidthOfLabel(string inputString)
        {
            try
            {
                int width = 0;
                int index = inputString.IndexOf(',');
                index = inputString.IndexOf(',', index + 1);
                if (index == -1)
                    index = inputString.Length;
                string result = inputString.Substring(0, index);
                using (Graphics g = CreateGraphics())
                {
                    SizeF size = g.MeasureString(result, ultraUserAccounts.Font);
                    width = (int)Math.Ceiling(size.Width);
                }
                return width;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
                return 0;
            }
        }

        /// <summary>
        ///  Added a tool tip functionality on mouse hover to show complete list of accounts of the login user,PRANA-11986
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraUserAccounts_MouseHover(object sender, EventArgs e)
        {
            try
            {
                this.toolTip1.SetToolTip(ultraUserAccounts, _accountsList);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Added a tool tip functionality on mouse hover to show complete list of trading accounts of the login user,PRANA-11986
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraUserTradingAccounts_MouseHover(object sender, EventArgs e)
        {
            try
            {
                this.toolTip1.SetToolTip(ultraUserTradingAccounts, _accounts);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
