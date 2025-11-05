using ExportGridsData;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
//using Prana.ClientCommon;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Forms
{
    public partial class MarkPriceAndForexConversion : Form, IMarkPriceUI, IExportGridData
    {
        IPricingAnalysis _pricingAnalysis = null;
        //Timer _timer = new Timer();
        public MarkPriceAndForexConversion()
        {
            InitializeComponent();

            if (!CustomThemeHelper.IsDesignMode())
            {
                bool isPerformanceNumberColumnsEnabled = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_IsPerformanceNumberColumnsEnabled));
                if (!isPerformanceNumberColumnsEnabled)
                {
                    this.tabControlDailyValuation.Tabs[TabName_PerformanceNumbers].Visible = false;
                }

                toolStripStatusLabel1.Text = string.Empty;
                this.tabControlDailyValuation.Tabs[TabName_CollateralPrice].Visible = CachedDataManager.GetInstance.IsCollateralMarkPriceValidation();
                //ctrlMarkPriceAndForexConversion.ConfirmationSaveClicked += new EventHandler(ctrlMarkPriceAndForexConversion_ConfirmationSaveClicked);
                //ctrlMarkPriceAndForexConversion.ConnectServer += new EventHandler(ctrlMarkPriceAndForexConversion_ConnectServer);

                //_liveFeedEngineCommManagerInstance = new ClientTradeCommManager();
                //ctrlMarkPriceAndForexConversion.ExPNLCommMgrInstance = _liveFeedEngineCommManagerInstance;
                //ConnectToLiveFeedEngine();
                InstanceManager.RegisterInstance(this);

            }

        }



        //void ctrlMarkPriceAndForexConversion_ConfirmationSaveClicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //Depending upon the tab selection saving is done.
        //        if (tabMarkPriceForexConvertor.Tabs[TabName_MarkPrice].Selected.Equals(true))
        //        {
        //            //Saving mark price data.
        //            SaveMarkPriceData();
        //        }
        //        else if (tabMarkPriceForexConvertor.Tabs[TabName_ForexConversion].Selected.Equals(true))
        //        {
        //            //Saving forex conversion data.
        //            SaveForexConversionData();
        //        }
        //        else if (tabMarkPriceForexConvertor.Tabs[TabName_NAV].Selected.Equals(true))
        //        {
        //            //Saving NAV values
        //SaveNAVValues();
        //        }
        //        else if (tabMarkPriceForexConvertor.Tabs[TabName_DailyCash].Selected.Equals(true))
        //        {
        //            //Saving daily cash.
        //            //SaveNAVValues();
        //        }
        //        else if (tabMarkPriceForexConvertor.Tabs[TabName_DailyBeta].Selected.Equals(true))
        //        {
        //            SaveDailyBetaValues();
        //        }
        //        else if (tabMarkPriceForexConvertor.Tabs[TabName_DailyTradingVol].Selected.Equals(true))
        //        {
        //            SaveDailyTradingVolValues();
        //        }
        //        else if (tabMarkPriceForexConvertor.Tabs[TabName_DailyDelta].Selected.Equals(true))
        //        {
        //            SaveDailyDeltaValues();
        //        }
        //        else if (tabMarkPriceForexConvertor.Tabs[TabName_DailyDelta].Selected.Equals(true))
        //        {
        //            SaveDailyDeltaValues();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        private const string TabName_MarkPrice = "tabPageMarkPrice";
        private const string TabName_FXMarkPrice = "tabPagefxmarkPrices";//Forward points
        private const string TabName_ForexConversion = "tabPageForexConversion";
        private const string TabName_NAV = "tabPageNAV";
        private const string TabName_DailyCash = "tabPageDailyCash";
        private const string TabName_CollateralInterest = "tabPageCollateralInterest";
        private const string TabName_DailyBeta = "tabPageDailyBeta";
        private const string TabName_VWAP = "tabPageVWAP";
        private const string TabName_DailyTradingVol = "tabPageDailyTradingVol";
        private const string TabName_DailyDelta = "tabPageDailyDelta";
        private const string TabName_Outstanding = "tabPageDailyOutstandings";
        private const string TabName_PerformanceNumbers = "tabPagePerformanceNumbers";
        private const int KEY_CTRL_F = 131142;
        private const string TabName_StartOfMonthCapitalAccount = "tabPageStartOfMonthCapitalAccount";
        private const string TabName_UserDefinedMTDPnL = "tabPageUserDefinedMTDPnL";
        private const string TabName_DailyCreditLimit = "tabPageDailyCreditLimit";
        private const string TabName_DailyVolatility = "tabPageDailyVolatility";
        private const string TabName_DailyDividendYield = "tabPageDailyDividendYield";
        private const string TabName_CollateralPrice = "tabPageCollateralPrice";
        private void tabControlDailyValuation_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "Getting Data...";
                if (tabControlDailyValuation.Tabs[TabName_MarkPrice].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_MarkPrice);
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                if (tabControlDailyValuation.Tabs[TabName_FXMarkPrice].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_FXMarkPrice);
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                else if (tabControlDailyValuation.Tabs[TabName_ForexConversion].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_ForexConversion);
                    btnAdd.Visible = true;
                    btnRemove.Visible = false;

                    if (!String.IsNullOrEmpty(ctrlMarkPriceAndForexConversion._output))
                    {
                        toolStripStatusLabel1.Text = ("The following Currency Pairs does not exist- " + ctrlMarkPriceAndForexConversion._output + ".Please Add them");
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = String.Empty;
                    }
                }
                else if (tabControlDailyValuation.Tabs[TabName_NAV].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_NAV);
                    btnAdd.Visible = true;
                    btnRemove.Visible = true;
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyCash].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_DailyCash);
                    btnAdd.Visible = true;
                    btnRemove.Visible = true;
                }
                else if (tabControlDailyValuation.Tabs[TabName_CollateralInterest].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_CollateralInterest);
                    btnAdd.Visible = true;
                    btnRemove.Visible = true;
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyBeta].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_DailyBeta);
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                else if (tabControlDailyValuation.Tabs[TabName_VWAP].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_VWAP);
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                else if (tabControlDailyValuation.Tabs[TabName_CollateralPrice].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_CollateralPrice);
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyTradingVol].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_DailyTradingVol);
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyDelta].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.PricingAnalysis = _pricingAnalysis;
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_DailyDelta);
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                else if (tabControlDailyValuation.Tabs[TabName_Outstanding].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_Outstanding);
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                else if (tabControlDailyValuation.Tabs[TabName_PerformanceNumbers].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_PerformanceNumbers);
                    btnAdd.Visible = true;
                    btnRemove.Visible = true;
                }
                else if (tabControlDailyValuation.Tabs[TabName_StartOfMonthCapitalAccount].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_StartOfMonthCapitalAccount);
                    btnAdd.Visible = true;
                    btnRemove.Visible = true;
                }
                else if (tabControlDailyValuation.Tabs[TabName_UserDefinedMTDPnL].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_UserDefinedMTDPnL);
                    btnAdd.Visible = true;
                    btnRemove.Visible = true;
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyCreditLimit].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_DailyCreditLimit);
                    btnAdd.Visible = true;
                    btnRemove.Visible = true;
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyVolatility].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_DailyVolatility);
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyDividendYield].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.SetupControl(TabName_DailyDividendYield);
                    btnAdd.Visible = false;
                    btnRemove.Visible = false;
                }
                if (!tabControlDailyValuation.Tabs[TabName_ForexConversion].Selected.Equals(true))
                    toolStripStatusLabel1.Text = String.Empty;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                toolStripStatusLabel1.Text = string.Empty;
                if (rethrow)
                {
                    throw;
                }
            }
        }

        BackgroundWorker _bgSaveData = null;
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _bgSaveData = new BackgroundWorker();
                toolStripStatusLabel1.Text = "Saving...";
                btnSave.Enabled = false;
                DataTable dtDatToSave = new DataTable();
                dtDatToSave = ctrlMarkPriceAndForexConversion.GetDataToSave();
                _bgSaveData.DoWork += new DoWorkEventHandler(_bgSaveData_DoWork);
                _bgSaveData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgSaveData_RunWorkerCompleted);
                _bgSaveData.RunWorkerAsync(dtDatToSave);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                toolStripStatusLabel1.Text = string.Empty;
                btnSave.Enabled = true;
                if (rethrow)
                {
                    throw;
                }
            }
        }

        bool _isDataSaving = false;
        void _bgSaveData_DoWork(object sender, DoWorkEventArgs e)
        {
            _isDataSaving = true;
            int rowsAffected = 0;
            try
            {
                //DataTable dtDatToSave = new DataTable();
                DataTable dtDataToSave = (DataTable)e.Argument;
                if (tabControlDailyValuation.Tabs["tabPageMarkPrice"].Selected.Equals(true) || tabControlDailyValuation.Tabs["tabPagefxmarkPrices"].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveMarkPriceData(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs["tabPageForexConversion"].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveForexConversionData(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs["tabPageNAV"].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveNAVValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyCash].Selected.Equals(true))
                {
                    ctrlMarkPriceAndForexConversion.DeleteDailyCashEntries();
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveDailyCashValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_CollateralInterest].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveCollateralInterestValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyBeta].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveDailyBetaValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyTradingVol].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveDailyTradingVolValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyDelta].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveDailyDeltaValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_Outstanding].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveDailyOutStandingValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_PerformanceNumbers].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SavePerformanceNumberValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_StartOfMonthCapitalAccount].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveStartOfMonthCapitalAccountValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_UserDefinedMTDPnL].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveUserDefinedMTDPnLValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyCreditLimit].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveDailyCreditLimitValues(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyVolatility].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveDailyVolatility(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_DailyDividendYield].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveDailyDividendYield(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_VWAP].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveDailyVWAP(dtDataToSave);
                }
                else if (tabControlDailyValuation.Tabs[TabName_CollateralPrice].Selected.Equals(true))
                {
                    rowsAffected = ctrlMarkPriceAndForexConversion.SaveCollateralPriceValues(dtDataToSave);
                }
                e.Result = rowsAffected;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        void _bgSaveData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                int rowsAffected = 0;
                btnSave.Enabled = true;
                _isDataSaving = false;
                if (e.Result != null)
                {
                    rowsAffected = (int)e.Result;
                }
                if (rowsAffected > 0)
                {
                    toolStripStatusLabel1.Text = "Data Saved at " + DateTime.Now;
                }
                else
                {
                    toolStripStatusLabel1.Text = "Nothing To Save";
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                toolStripStatusLabel1.Text = string.Empty;
                btnSave.Enabled = true;
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnAddToCloseTrade_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = string.Empty;
            ctrlMarkPriceAndForexConversion.AddNewRow();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = string.Empty;
            ctrlMarkPriceAndForexConversion.RemoveRow();
        }

        private void MarkPriceAndForexConversion_Load(object sender, EventArgs e)
        {
            try
            {
                this.KeyPreview = true;
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_DAILY_PM_CLIENTUI);
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.Font = new Font("Century Gothic", 9F);
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRemove.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnRemove.ForeColor = System.Drawing.Color.White;
                btnRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRemove.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRemove.UseAppStyling = false;
                btnRemove.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClear.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClear.ForeColor = System.Drawing.Color.White;
                btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClear.UseAppStyling = false;
                btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAdd.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAdd.ForeColor = System.Drawing.Color.White;
                btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAdd.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAdd.UseAppStyling = false;
                btnAdd.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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


        void MarkPriceAndForexConversion_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (_isDataSaving == false)
            {
                ctrlMarkPriceAndForexConversion.ResetGrid();
                bool isUnSavedChanges = false;
                DataTable dtDataToSave = new DataTable();
                if (ctrlMarkPriceAndForexConversion.IsDataSourceChanged() == true)
                {
                    dtDataToSave = ctrlMarkPriceAndForexConversion.GetDataToSave();
                    isUnSavedChanges = true;
                    DialogResult dlgResult = DialogResult.Yes;
                    dlgResult = MessageBox.Show("Do you want to save the changes done in the data?", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dlgResult.Equals(DialogResult.Yes))
                    {
                        e.Cancel = true;
                        try
                        {
                            switch (tabControlDailyValuation.SelectedTab.Key.ToString())
                            {
                                case TabName_MarkPrice:
                                    ctrlMarkPriceAndForexConversion.SaveMarkPriceData(dtDataToSave);
                                    break;
                                case TabName_ForexConversion:
                                    ctrlMarkPriceAndForexConversion.SaveForexConversionData(dtDataToSave);
                                    break;
                                case TabName_NAV:
                                    ctrlMarkPriceAndForexConversion.SaveNAVValues(dtDataToSave);
                                    break;
                                case TabName_DailyCash:
                                    ctrlMarkPriceAndForexConversion.SaveDailyCashValues(dtDataToSave);
                                    break;
                                case TabName_CollateralInterest:
                                    ctrlMarkPriceAndForexConversion.SaveCollateralInterestValues(dtDataToSave);
                                    break;
                                case TabName_DailyBeta:
                                    ctrlMarkPriceAndForexConversion.SaveDailyBetaValues(dtDataToSave);
                                    break;
                                case TabName_DailyTradingVol:
                                    ctrlMarkPriceAndForexConversion.SaveDailyTradingVolValues(dtDataToSave);
                                    break;
                                case TabName_DailyDelta:
                                    ctrlMarkPriceAndForexConversion.SaveDailyDeltaValues(dtDataToSave);
                                    break;
                                case TabName_Outstanding:
                                    ctrlMarkPriceAndForexConversion.SaveDailyOutStandingValues(dtDataToSave);
                                    break;
                                case TabName_PerformanceNumbers:
                                    ctrlMarkPriceAndForexConversion.SavePerformanceNumberValues(dtDataToSave);
                                    break;
                                case TabName_DailyVolatility:
                                    ctrlMarkPriceAndForexConversion.SaveDailyVolatility(dtDataToSave);
                                    break;
                                case TabName_DailyDividendYield:
                                    ctrlMarkPriceAndForexConversion.SaveDailyDividendYield(dtDataToSave);
                                    break;
                                case TabName_VWAP:
                                    ctrlMarkPriceAndForexConversion.SaveDailyVWAP(dtDataToSave);
                                    break;
                                case TabName_CollateralPrice:
                                    ctrlMarkPriceAndForexConversion.SaveCollateralPriceValues(dtDataToSave);
                                    break;

                                default:
                                    break;
                            }
                            isUnSavedChanges = false;
                            InstanceManager.ReleaseInstance(typeof(MarkPriceAndForexConversion));
                        }
                        catch (Exception ex)
                        {

                            // Invoke our policy that is responsible for making sure no secure information
                            // gets out of our layer.
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                            if (rethrow)
                            {
                                throw;
                            }
                        }
                    }

                    else if (dlgResult.Equals(DialogResult.Cancel))
                    {
                        e.Cancel = true;
                    }
                }

                if (tabControlDailyValuation.SelectedTab.Key.Equals(TabName_FXMarkPrice) && !isUnSavedChanges)
                {
                    ctrlMarkPriceAndForexConversion.SaveMarkPriceData(dtDataToSave);
                }

                //DisconnectLiveFeedEngine();
            }
            else
            {
                toolStripStatusLabel1.Text = "Data is being saved...";
                e.Cancel = true;
            }
        }

        private void MarkPriceAndForexConversion_KeyUp(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyData == KEY_CTRL_F)
            {
                ctrlMarkPriceAndForexConversion.txtSymbolFilteration.Focus();
            }
        }

        void MarkPriceAndForexConversion_Disposed(object sender, System.EventArgs e)
        {
            try
            {
                if (FormClosed != null)
                {
                    FormClosed(this, EventArgs.Empty);
                    if (_bgSaveData != null)
                    {
                        _bgSaveData.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_bgSaveData_RunWorkerCompleted);
                        _bgSaveData.DoWork -= new DoWorkEventHandler(_bgSaveData_DoWork);
                    }
                }
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

        private void tabControlDailyValuation_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            var selectedTab = tabControlDailyValuation.SelectedTab;
            if (ctrlMarkPriceAndForexConversion.AllowTabChange.Equals(false))
            {
                tabControlDailyValuation.Tabs[selectedTab.Key].Selected = true;
                tabControlDailyValuation.Tabs[selectedTab.Key].Active = true;
                e.Cancel = true;
            }
        }

        #region IMarkPriceUI Members

        public Form Reference()
        {
            return this;
        }

        public new event EventHandler FormClosed;
        private Prana.BusinessObjects.CompanyUser _loginUser = null;
        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            get
            {
                return _loginUser;
            }
            set
            {
                _loginUser = value;
            }
        }

        #endregion


        public IPricingAnalysis PricingAnalysis
        {
            set { _pricingAnalysis = value; }
        }

        /// <summary>
        /// used to Export Data for automation
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            this.ctrlMarkPriceAndForexConversion.ExportGridData(filePath);
        }

    }
}