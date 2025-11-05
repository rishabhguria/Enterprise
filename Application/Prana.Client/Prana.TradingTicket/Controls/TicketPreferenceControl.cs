using ExportGridsData;
using Infragistics.Win.UltraWinTabControl;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Windows.Forms;

namespace Prana.TradingTicket
{
    /// <summary> 
    /// Summary description for TicketPreferences.
    /// </summary>
    public sealed class TicketPreferenceControl : System.Windows.Forms.UserControl, IPreferences, IExportGridData
    {
        #region Windows Members
        private const string FORM_NAME = "TicketPreferences : ";
        private Prana.BusinessObjects.CompanyUser _LoginUser;
        //private Prana.TradingTicket.CustomSettingsGrid customSettingsGrid2;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion

        TTPrefSettingsNew _currentPrefs = new TTPrefSettingsNew();
        TTPrefSettingsNew _dbPrefs = new TTPrefSettingsNew();
        TranferTradeRules transfertraderules = CachedDataManager.GetInstance.GetTransferTradeRules();

        private UltraTabPageControl ultraTabPageControl1;
        private PriceSymbolValidate riskValidateCtrl;
        private ConfirmationPopUpUserControl confirmationPopUpUserControl;
        private UltraTabPageControl ultraTabPageControl2;
        private CustomSettingsGridNew customSettingsGrid1;
        private UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private UltraTabControl tabCtrlTradingPrefs;
        private UltraTabPageControl tabDefaultCPwiseCommissions;
        private UltraTabPageControl tabGeneralPrefs;
        private UltraTabPageControl tabRestrictedAllowedPrefs;
        private Prana.TradingTicket.Controls.CounterPartyWiseDefaultCommissions counterPartyWiseDefaultCommissions1;
        private Prana.TradingTicket.Controls.TTGeneralPreferencesControl generalPrefsControl1;
        Prana.BusinessObjects.PriceSymbolValidation _riskValidateSetting = new Prana.BusinessObjects.PriceSymbolValidation();
        private UltraTabPageControl ultraTabPageControl3;
        private UltraTabPageControl tabQuickTTPrefs;
        private Admin.CommonControls.PL.ttPreferenceCtrl ttPreferenceCtrl1;
        private Prana.TradingTicket.Controls.QuickTTPreference quickTTPrefsCtrl;
        private Prana.TradingTicket.Controls.RestrictedAllowedSecuritiesPreference restrictedAllowedPrefControl1;

        public TicketPreferenceControl()
        {
            InitializeComponent();
            InstanceManager.RegisterInstance(this);
        }

        /// <summary>
        /// The _allocation proxy for getting Account prefernces
        /// </summary>
        ProxyBase<IAllocationManager> _allocationProxy = null;

        public void SetUp(Prana.BusinessObjects.CompanyUser user)
        {
            try
            {
                _LoginUser = user;

                //Setting allocation proxy if null.
                if (_allocationProxy == null)
                {
                    _allocationProxy = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                }

                GetStartUpValues();
                SetUserControls(false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void GetStartUpValues()
        {
            _currentPrefs.ConfirmationPopUp = TradingTktPrefs.ConfirmationPopUpPrefs;

            TradingTicketSettingsCollection ttsettlocal = TradingTicketManager.GetInstance().GetTradingTicketSettings(_LoginUser.CompanyUserID);
            for (int itemIndex = 0; itemIndex < ttsettlocal.Count; itemIndex++)
            {
                _currentPrefs.TTSettingsCollection.Add(ttsettlocal[itemIndex]);
            }

            //added for 2.0.1.4 by harsh
            _riskValidateSetting = TradingTktPrefs.PriceSymbolValidationData;
        }

        private void SetUserControls(bool isDefaultClick)
        {
            try
            {
                BindTTSettingNConfirmation();
                generalPrefsControl1.Setup(isDefaultClick);
                ttPreferenceCtrl1.TTPreferenceType = TradingTicketPreferenceType.User;
                ttPreferenceCtrl1.SetupControl(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, CachedDataManager.GetInstance.LoggedInUser.CompanyID, CachedDataManager.GetInstance.IsEquityOptionManualValidation());
                counterPartyWiseDefaultCommissions1.Setup();
                quickTTPrefsCtrl.Setup(CachedDataManager.GetInstance.PermissibleQuickTTInstances);
                restrictedAllowedPrefControl1.SetupControl(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                if (TradingTktPrefs.TTGeneralPrefs != null)
                {
                    ttPreferenceCtrl1.EnableDisableBroker(!TradingTktPrefs.TTGeneralPrefs.IsUseCustodianAsExecutingBroker);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindTTSettingNConfirmation()
        {
            //Tab Custom Setting Grid
            customSettingsGrid1.SetUp(_LoginUser, _currentPrefs.TTSettingsCollection);
            //Tab Confirmation PopUP
            confirmationPopUpUserControl.SetConfirmation(_currentPrefs.ConfirmationPopUp);
            //added for 2.0.1.4 by harsh
            riskValidateCtrl.SetValues(_riskValidateSetting);
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
                if (ultraTabPageControl1 != null)
                {
                    ultraTabPageControl1.Dispose();
                }
                if (riskValidateCtrl != null)
                {
                    riskValidateCtrl.Dispose();
                }
                if (confirmationPopUpUserControl != null)
                {
                    confirmationPopUpUserControl.Dispose();
                }
                if (ultraTabPageControl2 != null)
                {
                    ultraTabPageControl2.Dispose();
                }
                if (customSettingsGrid1 != null)
                {
                    customSettingsGrid1.Dispose();
                }
                if (ultraTabSharedControlsPage1 != null)
                {
                    ultraTabSharedControlsPage1.Dispose();
                }
                if (tabCtrlTradingPrefs != null)
                {
                    tabCtrlTradingPrefs.Dispose();
                }
                if (tabDefaultCPwiseCommissions != null)
                {
                    tabDefaultCPwiseCommissions.Dispose();
                }
                if (tabGeneralPrefs != null)
                {
                    tabGeneralPrefs.Dispose();
                }
                if (tabRestrictedAllowedPrefs != null)
                {
                    tabRestrictedAllowedPrefs.Dispose();
                }
                if (counterPartyWiseDefaultCommissions1 != null)
                {
                    counterPartyWiseDefaultCommissions1.Dispose();
                }
                if (generalPrefsControl1 != null)
                {
                    generalPrefsControl1.Dispose();
                }
                if (ultraTabPageControl3 != null)
                {
                    ultraTabPageControl3.Dispose();
                }
                if (tabQuickTTPrefs != null)
                {
                    tabQuickTTPrefs.Dispose();
                }
                if (ttPreferenceCtrl1 != null)
                {
                    ttPreferenceCtrl1.Dispose();
                }
                if (quickTTPrefsCtrl != null)
                {
                    quickTTPrefsCtrl.Dispose();
                }
                if (restrictedAllowedPrefControl1 != null)
                {
                    restrictedAllowedPrefControl1.Dispose();
                }
                if (_allocationProxy != null)
                {
                    _allocationProxy.Dispose();
                    _allocationProxy = null;
                }
            }
            InstanceManager.ReleaseInstance(typeof(TicketPreferenceControl));
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabQuickTTPrefs = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ttPreferenceCtrl1 = new Prana.Admin.CommonControls.PL.ttPreferenceCtrl();
            this.quickTTPrefsCtrl = new Controls.QuickTTPreference();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabDefaultCPwiseCommissions = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabGeneralPrefs = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabRestrictedAllowedPrefs = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tabCtrlTradingPrefs = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.customSettingsGrid1 = new Prana.TradingTicket.CustomSettingsGridNew();
            this.riskValidateCtrl = new Prana.TradingTicket.PriceSymbolValidate();
            this.confirmationPopUpUserControl = new Prana.TradingTicket.ConfirmationPopUpUserControl();
            this.counterPartyWiseDefaultCommissions1 = new Prana.TradingTicket.Controls.CounterPartyWiseDefaultCommissions();
            this.generalPrefsControl1 = new Prana.TradingTicket.Controls.TTGeneralPreferencesControl();
            this.restrictedAllowedPrefControl1 = new Prana.TradingTicket.Controls.RestrictedAllowedSecuritiesPreference();
            this.restrictedAllowedPrefControl1.SecurityMaster = Preferences.PreferencesMain.GetSecurityMaster();
            this.tabQuickTTPrefs.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl1.SuspendLayout();
            this.tabDefaultCPwiseCommissions.SuspendLayout();
            this.tabGeneralPrefs.SuspendLayout();
            this.tabRestrictedAllowedPrefs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlTradingPrefs)).BeginInit();
            this.tabCtrlTradingPrefs.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl4
            // 
            this.tabQuickTTPrefs.Controls.Add(this.quickTTPrefsCtrl);
            this.tabQuickTTPrefs.Location = new System.Drawing.Point(-10000, -10000);
            this.tabQuickTTPrefs.Name = "ultraTabPageControl4";
            this.tabQuickTTPrefs.Size = new System.Drawing.Size(692, 442);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.ttPreferenceCtrl1);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(692, 442);
            // 
            // quickTTPrefsCtrl
            // 
            this.quickTTPrefsCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickTTPrefsCtrl.Location = new System.Drawing.Point(0, 0);
            this.quickTTPrefsCtrl.Name = "quickTTPrefsCtrl";
            this.quickTTPrefsCtrl.Size = new System.Drawing.Size(692, 442);
            this.quickTTPrefsCtrl.TabIndex = 0;
            // 
            // ttPreferenceCtrl1
            // 
            this.ttPreferenceCtrl1.CompanyID = -2147483648;
            this.ttPreferenceCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ttPreferenceCtrl1.Location = new System.Drawing.Point(0, 0);
            this.ttPreferenceCtrl1.Name = "ttPreferenceCtrl1";
            this.ttPreferenceCtrl1.Size = new System.Drawing.Size(692, 442);
            this.ttPreferenceCtrl1.TabIndex = 0;
            this.ttPreferenceCtrl1.TTPreferenceType = Prana.BusinessObjects.AppConstants.TradingTicketPreferenceType.Company;
            this.ttPreferenceCtrl1.UserID = -2147483648;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.customSettingsGrid1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(692, 442);
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.riskValidateCtrl);
            this.ultraTabPageControl1.Controls.Add(this.confirmationPopUpUserControl);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(692, 442);
            // 
            // tabDefaultCPwiseCommissions
            // 
            this.tabDefaultCPwiseCommissions.Controls.Add(this.counterPartyWiseDefaultCommissions1);
            this.tabDefaultCPwiseCommissions.Location = new System.Drawing.Point(-10000, -10000);
            this.tabDefaultCPwiseCommissions.Name = "tabDefaultCPwiseCommissions";
            this.tabDefaultCPwiseCommissions.Size = new System.Drawing.Size(692, 442);
            // 
            // tabRestrictedAllowedPrefs
            // 
            this.tabRestrictedAllowedPrefs.Controls.Add(this.restrictedAllowedPrefControl1);
            this.tabRestrictedAllowedPrefs.Location = new System.Drawing.Point(-10000, -10000);
            this.tabRestrictedAllowedPrefs.Name = "tabRestrictedAllowedPrefs";
            this.tabRestrictedAllowedPrefs.Size = new System.Drawing.Size(692, 442);
            // 
            // tabGeneralPrefs
            // 
            this.tabGeneralPrefs.Controls.Add(this.generalPrefsControl1);
            this.tabGeneralPrefs.Location = new System.Drawing.Point(-10000, -10000);
            this.tabGeneralPrefs.Name = "tabGeneralPrefs";
            this.tabGeneralPrefs.Size = new System.Drawing.Size(692, 442);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(692, 442);
            // 
            // tabCtrlTradingPrefs
            // 
            this.tabCtrlTradingPrefs.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabCtrlTradingPrefs.Controls.Add(this.ultraTabPageControl2);
            this.tabCtrlTradingPrefs.Controls.Add(this.ultraTabPageControl1);
            this.tabCtrlTradingPrefs.Controls.Add(this.tabDefaultCPwiseCommissions);
            this.tabCtrlTradingPrefs.Controls.Add(this.tabRestrictedAllowedPrefs);
            this.tabCtrlTradingPrefs.Controls.Add(this.tabGeneralPrefs);
            this.tabCtrlTradingPrefs.Controls.Add(this.ultraTabPageControl3);
            this.tabCtrlTradingPrefs.Controls.Add(this.tabQuickTTPrefs);
            this.tabCtrlTradingPrefs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlTradingPrefs.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.tabCtrlTradingPrefs.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlTradingPrefs.Name = "tabCtrlTradingPrefs";
            this.tabCtrlTradingPrefs.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabCtrlTradingPrefs.Size = new System.Drawing.Size(694, 463);
            this.tabCtrlTradingPrefs.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabCtrlTradingPrefs.TabIndex = 0;
            ultraTab1.Key = "QuickTT";
            ultraTab1.TabPage = this.tabQuickTTPrefs;
            ultraTab1.Text = "Quick TT";
            ultraTab4.Key = "UiPrefs";
            ultraTab4.TabPage = this.ultraTabPageControl3;
            ultraTab4.Text = "UI Preferences";
            ultraTab2.Key = "TicketSetting";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Action Buttons";
            ultraTab2.ToolTipText = "Ticket Setting";
            ultraTab2.Visible = false;
            ultraTab3.Key = "Compliance";
            ultraTab3.TabPage = this.ultraTabPageControl1;
            ultraTab3.Text = "Compliance";
            ultraTab5.Key = "BrokerWisePreferences";
            ultraTab5.TabPage = this.tabDefaultCPwiseCommissions;
            ultraTab5.Text = "Broker Wise Preferences";
            ultraTab6.Key = "TTGeneralPrefs";
            ultraTab6.TabPage = this.tabGeneralPrefs;
            ultraTab6.Text = "General Preferences";
            ultraTab7.Key = "RestrictedAllowedSecurities";
            ultraTab7.TabPage = this.tabRestrictedAllowedPrefs;
            ultraTab7.Text = "Restricted/Allowed Securities";
            this.tabCtrlTradingPrefs.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab4,
            ultraTab2,
            ultraTab3,
            ultraTab5,
            ultraTab6,
            ultraTab7});
            this.tabCtrlTradingPrefs.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;


            // Visibility of Restricted/Allowed Securities List
            if (!transfertraderules.IsAllowRestrictedSecuritiesList && !transfertraderules.IsAllowAllowedSecuritiesList)
            {
                ultraTab7.Visible = false;
            }

            // Display Quick TT according to permission in Admin
            if (!ModuleManager.CheckModulePermissioning(PranaModules.QUICK_TRADING_TICKET_MODULE, PranaModules.QUICK_TRADING_TICKET_MODULE))
            {
                ultraTab1.Visible = false;
            }


            // 
            // customSettingsGrid1
            // 
            this.customSettingsGrid1.AUECDefinedButtons = null;
            this.customSettingsGrid1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customSettingsGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customSettingsGrid1.Location = new System.Drawing.Point(0, 0);
            this.customSettingsGrid1.Name = "customSettingsGrid1";
            this.customSettingsGrid1.Size = new System.Drawing.Size(692, 442);
            this.customSettingsGrid1.TabIndex = 4;
            // 
            // riskValidateCtrl
            // 
            this.riskValidateCtrl.Location = new System.Drawing.Point(3, 175);
            this.riskValidateCtrl.Name = "riskValidateCtrl";
            this.riskValidateCtrl.Size = new System.Drawing.Size(619, 181);
            this.riskValidateCtrl.TabIndex = 1;
            // 
            // confirmationPopUpUserControl
            // 
            this.confirmationPopUpUserControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.confirmationPopUpUserControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.confirmationPopUpUserControl.Location = new System.Drawing.Point(0, 0);
            this.confirmationPopUpUserControl.Name = "confirmationPopUpUserControl";
            this.confirmationPopUpUserControl.Size = new System.Drawing.Size(692, 170);
            this.confirmationPopUpUserControl.TabIndex = 0;
            // 
            // counterPartyWiseDefaultCommissions1
            // 
            this.counterPartyWiseDefaultCommissions1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.counterPartyWiseDefaultCommissions1.Location = new System.Drawing.Point(0, 0);
            this.counterPartyWiseDefaultCommissions1.Name = "counterPartyWiseDefaultCommissions1";
            this.counterPartyWiseDefaultCommissions1.Size = new System.Drawing.Size(692, 442);
            this.counterPartyWiseDefaultCommissions1.TabIndex = 0;
            // 
            // generalPrefsControl1
            // 
            this.generalPrefsControl1.Location = new System.Drawing.Point(0, 0);
            this.generalPrefsControl1.Name = "generalPrefsControl1";
            this.generalPrefsControl1.Size = new System.Drawing.Size(680, 439);
            this.generalPrefsControl1.TabIndex = 0;
            // 
            // restrictedAllowedPrefControl1
            // 
            this.restrictedAllowedPrefControl1.Location = new System.Drawing.Point(0, 0);
            this.restrictedAllowedPrefControl1.Name = "restrictedAllowedPrefControl1";
            this.restrictedAllowedPrefControl1.Size = new System.Drawing.Size(680, 439);
            this.restrictedAllowedPrefControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restrictedAllowedPrefControl1.TabIndex = 0;
            // 
            // TicketPreferenceControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.tabCtrlTradingPrefs);
            this.Name = "TicketPreferenceControl";
            this.Size = new System.Drawing.Size(694, 463);
            this.tabQuickTTPrefs.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.tabDefaultCPwiseCommissions.ResumeLayout(false);
            this.tabGeneralPrefs.ResumeLayout(false);
            this.tabRestrictedAllowedPrefs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlTradingPrefs)).EndInit();
            this.tabCtrlTradingPrefs.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Events
        public void Save()
        {
            try
            {
                counterPartyWiseDefaultCommissions1.Save(_LoginUser.CompanyUserID);
                generalPrefsControl1.Save();
                ttPreferenceCtrl1.SetBrokerFieldOnSave(!TradingTktPrefs.TTGeneralPrefs.IsUseCustodianAsExecutingBroker);
                quickTTPrefsCtrl.Save();
                TradingTicketManager.GetInstance().SaveConfirmationPopUpPreferences(_currentPrefs.ConfirmationPopUp);
                TradingTicketManager.GetInstance().DeleteAllTradingTickets(_LoginUser.CompanyUserID);
                foreach (TradingTicketSettings tt in _currentPrefs.TTSettingsCollection)
                {
                    TradingTicketManager.GetInstance().SaveTradingTicketSettings(tt);
                }

                riskValidateCtrl.SavePriceSymbolValidation(_currentPrefs.RiskValidateSettings);
                TradingTktPrefs.UserTradingTicketUiPrefs = ttPreferenceCtrl1.SaveTradePreferences();
                restrictedAllowedPrefControl1.SaveSecuritiesList(_LoginUser.CompanyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {

            }
        }

        private void GetCurrentlySelectedValues()
        {
            _currentPrefs.ConfirmationPopUp = confirmationPopUpUserControl.GetConfirmation();
            _currentPrefs.ConfirmationPopUp.CompanyUserID = _LoginUser.CompanyUserID;
            TradingTicketSettingsCollection tradingtickets = customSettingsGrid1.GetTicketPreferences();
            foreach (TradingTicketSettings ttsetting in tradingtickets)
            {
                if (!_currentPrefs.TTSettingsCollection.Contains((TradingTicketSettings)ttsetting))
                {
                    _currentPrefs.TTSettingsCollection.Add(ttsetting);
                }
            }
            //added for 2.0.1.4 by harsh
            _currentPrefs.RiskValidateSettings = riskValidateCtrl.GetValues();
            _currentPrefs.RiskValidateSettings.CompanyUserID = _LoginUser.CompanyUserID;
            TradingTktPrefs.UpdateCacheTradingPrefs(_currentPrefs.RiskValidateSettings, _currentPrefs.ConfirmationPopUp);
        }

        #endregion

        #region IPreferences Members

        UserControl IPreferences.Reference()
        {
            _dbPrefs = _currentPrefs.Clone();
            return this;
        }

        bool IPreferences.Save()
        {
            GetCurrentlySelectedValues();
            this.Save();
            return true;
        }

        void IPreferences.RestoreDefault()
        {
            _currentPrefs = _dbPrefs.Clone();
            SetUserControls(true);
        }

        IPreferenceData IPreferences.GetPrefs()
        {
            //SetStartUpValues();
            return _currentPrefs;
        }

        // we have more than one tab in the LiveFeed preferences, so need to select on select of a particular module
        // so declare a property in the IPreference interface
        private string _modulename = string.Empty;
        public string SetModuleActive
        {
            set
            {
                _modulename = value;
            }
        }
        //public event EventHandler SaveClicked;
        #endregion

        /// <summary>
        /// To export data for automation
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            if (gridName == "grdBrokerWiseSettings" && this.counterPartyWiseDefaultCommissions1 != null)
            {
                this.counterPartyWiseDefaultCommissions1.ExportGridData(filePath);
            }
            else if (gridName == "grdAssetSide" && ttPreferenceCtrl1 != null)
            {
                this.ttPreferenceCtrl1.ExportGridData(filePath);
            }
        }
    }
}
