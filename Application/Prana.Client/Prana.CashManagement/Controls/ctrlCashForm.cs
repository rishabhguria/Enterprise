using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.CashManagement.Classes;
using Prana.CashManagement.Controls;
using Prana.CashManagement.Forms;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prana.CashManagement
{
    public partial class ctrlCashForm : UserControl, IPublishing
    {
        private string _previousDate;
        CashSavePreference _cashSavePreference = new CashSavePreference();
        private List<ctrlDataGrid> noOfDayEndCashTab;
        frmAccountTransactionDetails transactionDetails = null;
        public ctrlCashForm(CompanyUser LoginUserArg)
        {
            try
            {
                noOfDayEndCashTab = new List<ctrlDataGrid>();
                LoginUser = LoginUserArg;
                InitializeComponent();
                CreateSubscriptionServicesProxy();
                CashDataManager.GetInstance();
                CashDataManager.CashManagementServices.ConnectedEvent += new Proxy<ICashManagementService>.ConnectionEventHandler(CashManagementServices_ConnectedEvent);
                CashDataManager.CashManagementServices.DisconnectedEvent += new Proxy<ICashManagementService>.ConnectionEventHandler(CashManagementServices_DisconnectedEvent);
                _todayenddata = Application.StartupPath.ToString() + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + LoginUser.CompanyUserID + "\\" + "todayenddata";
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

        private static object _lockerCashTransaction = new object();
        GenericBindingList<TransactionEntry> _cashImpactToBind = new GenericBindingList<TransactionEntry>();
        public GenericBindingList<TransactionEntry> CashImpactToBind
        {
            get { lock (_lockerCashTransaction) { return _cashImpactToBind; } }
            set { lock (_lockerCashTransaction) { _cashImpactToBind = value; } }
        }

        private static object _lockerTransactions = new object();
        GenericBindingList<Transaction> _getedTransactions = new GenericBindingList<Transaction>();
        public GenericBindingList<Transaction> GetedTransactions
        {
            get { lock (_lockerTransactions) { return _getedTransactions; } }
            set { lock (_lockerTransactions) { _getedTransactions = value; } }
        }

        private static object _lockerDayEnd = new object();
        GenericBindingList<CompanyAccountCashCurrencyValue> _dayEndDataToBind = new GenericBindingList<CompanyAccountCashCurrencyValue>();
        public GenericBindingList<CompanyAccountCashCurrencyValue> DayEndDataToBind
        {
            get { lock (_lockerDayEnd) { return _dayEndDataToBind; } }
            set { lock (_lockerDayEnd) { _dayEndDataToBind = value; } }
        }

        private CompanyUser _companyUser;
        public CompanyUser LoginUser
        {
            get { return _companyUser; }
            set { _companyUser = value; }
        }

        #region Private Functions
        //update
        //To get datewise cash Impact dictionary
        private Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> getDateWiseDayEndDataFromCashData()
        {
            Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> dateWiseData = new Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>>();
            try
            {
                GenericBindingList<CompanyAccountCashCurrencyValue> cashDataList;
                string dateKey;
                if (CashImpactToBind != null)
                {
                    foreach (TransactionEntry cashTransactionEntry in CashImpactToBind)
                    {
                        CompanyAccountCashCurrencyValue dataToAdd = new CompanyAccountCashCurrencyValue();
                        dataToAdd.Date = cashTransactionEntry.TransactionDate;
                        dataToAdd.CashValueLocal = CashRulesHelper.getCashImpact(cashTransactionEntry);
                        dataToAdd.AccountID = cashTransactionEntry.AccountID;
                        dataToAdd.LocalCurrencyID = cashTransactionEntry.CurrencyID;
                        dataToAdd.BaseCurrencyID = CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(dataToAdd.AccountID);
                        dataToAdd.AccountName = cashTransactionEntry.AccountName;
                        dataToAdd.LocalCurrencyName = cashTransactionEntry.CurrencyName;
                        dataToAdd.BaseCurrencyName = CachedDataManager.GetInstance.GetCurrencyText(dataToAdd.BaseCurrencyID);
                        //Cutting The Time part of the date time
                        dateKey = cashTransactionEntry.TransactionDate.ToShortDateString();
                        if (!dateWiseData.ContainsKey(dateKey))
                        {
                            cashDataList = new GenericBindingList<CompanyAccountCashCurrencyValue>();
                            cashDataList.Add(dataToAdd);
                            dateWiseData.Add(dateKey, cashDataList);
                        }
                        else
                        {
                            if (!dateWiseData[dateKey].Contains(dataToAdd))
                                dateWiseData[dateKey].Add(dataToAdd);
                            else
                                dateWiseData[dateKey].GetItem(dataToAdd.GetKey()).CashValueLocal += dataToAdd.CashValueLocal;
                        }
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
            return dateWiseData;
        }

        //Assigning Date Wise Day End Data in Global Variable _DateWiseDayEndDataDictionary
        private Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> _dateWiseDayEndDataDictionary;
        Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>> _dictforexGivenDateRange = null;

        private bool CalculateDateWiseDayEndData(GenericBindingList<CompanyAccountCashCurrencyValue> yesterdayDataList, List<CompanyAccountCashCurrencyValue> backupList)
        {
            bool result = true;
            try
            {
                _dictforexGivenDateRange = null;
                //Getting DateWiseCashDataDictionary
                Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> dateWiseCashDataDictionary = getDateWiseDayEndDataFromCashData();
                _dateWiseDayEndDataDictionary = new Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>>();
                CompanyAccountCashCurrencyValue TodayDayEndData;
                GenericBindingList<CompanyAccountCashCurrencyValue> todayDayEndDataList;
                string keyOfTodayData;
                DateTime dayEndFromDate = dtFromDate.DateTime;
                DateTime dayEndtoDate = dtToDate.DateTime;

                while (dayEndFromDate <= dayEndtoDate)
                {
                    keyOfTodayData = dayEndFromDate.ToShortDateString();
                    todayDayEndDataList = new GenericBindingList<CompanyAccountCashCurrencyValue>();
                    AddInitialDayEndCach(yesterdayDataList, backupList, dayEndFromDate);

                    #region Adding particular dates Transactions in todaydatalist
                    if (dateWiseCashDataDictionary.ContainsKey(keyOfTodayData))
                    {
                        GenericBindingList<CompanyAccountCashCurrencyValue> CashDataListOfPerticularDate = dateWiseCashDataDictionary[keyOfTodayData];
                        foreach (CompanyAccountCashCurrencyValue cashdata in CashDataListOfPerticularDate)
                        {
                            if (dayEndFromDate < CashDataManager.GetInstance().GetCashPreferences(cashdata.AccountID).CashMgmtStartDate)
                            {
                                continue;
                            }
                            TodayDayEndData = new CompanyAccountCashCurrencyValue();
                            TodayDayEndData.Date = dayEndFromDate;
                            TodayDayEndData.CashValueLocal = cashdata.CashValueLocal;

                            TodayDayEndData.AccountID = cashdata.AccountID;
                            TodayDayEndData.LocalCurrencyID = cashdata.LocalCurrencyID;
                            TodayDayEndData.BaseCurrencyID = CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(TodayDayEndData.AccountID);// _cashdata.LocalCurrencyID;
                            TodayDayEndData.AccountName = cashdata.AccountName;
                            TodayDayEndData.LocalCurrencyName = cashdata.LocalCurrencyName;
                            TodayDayEndData.BaseCurrencyName = cashdata.BaseCurrencyName;

                            if (yesterdayDataList.ContainsKey(cashdata.GetKey()))
                            {
                                //HaveToApply Check here of CashManagementStartDate on account
                                //Account+CUrrency
                                CompanyAccountCashCurrencyValue item = yesterdayDataList.GetItem(cashdata.GetKey());
                                if (Convert.ToDateTime(keyOfTodayData) >= CashDataManager.GetInstance().GetCashPreferences(item.AccountID).CashMgmtStartDate)
                                {
                                    TodayDayEndData.CashValueLocal += yesterdayDataList.GetItem(cashdata.GetKey()).CashValueLocal;
                                }
                            }
                            //CHMW-3132	Account wise fx rate handling for expiration settlement
                            TodayDayEndData.CashValueBase = CalculateBaseCurrencyValue(TodayDayEndData.BaseCurrencyID, TodayDayEndData.LocalCurrencyID, TodayDayEndData.CashValueLocal, dayEndFromDate, TodayDayEndData.AccountID);
                            todayDayEndDataList.Add(TodayDayEndData);
                        }
                    }
                    #endregion
                    #region Adding Remaining Previous day end data into todayDayEndData
                    foreach (CompanyAccountCashCurrencyValue previousDayEndData in yesterdayDataList)
                    {
                        if (!todayDayEndDataList.Contains(previousDayEndData) && CashDataManager.GetInstance().GetCashPreferences(previousDayEndData.AccountID) != null && Convert.ToDateTime(keyOfTodayData) >= CashDataManager.GetInstance().GetCashPreferences(previousDayEndData.AccountID).CashMgmtStartDate)
                        {
                            TodayDayEndData = new CompanyAccountCashCurrencyValue();
                            TodayDayEndData.Date = dayEndFromDate;
                            TodayDayEndData.CashValueLocal = previousDayEndData.CashValueLocal;
                            TodayDayEndData.LocalCurrencyID = previousDayEndData.LocalCurrencyID;
                            TodayDayEndData.AccountID = previousDayEndData.AccountID;
                            TodayDayEndData.BaseCurrencyID = CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(TodayDayEndData.AccountID);// CashDataManager.BaseCurrencyID;
                            //CHMW-3132	Account wise fx rate handling for expiration settlement
                            TodayDayEndData.CashValueBase = CalculateBaseCurrencyValue(TodayDayEndData.BaseCurrencyID, TodayDayEndData.LocalCurrencyID, TodayDayEndData.CashValueLocal, dayEndFromDate, TodayDayEndData.AccountID);
                            TodayDayEndData.AccountName = previousDayEndData.AccountName;
                            TodayDayEndData.LocalCurrencyName = previousDayEndData.LocalCurrencyName;
                            TodayDayEndData.BaseCurrencyName = previousDayEndData.BaseCurrencyName;
                            todayDayEndDataList.Add(TodayDayEndData);
                        }
                    }
                    #endregion
                    if (_dateWiseDayEndDataDictionary != null)
                    {
                        if (todayDayEndDataList.Count > 0)
                        {
                            _dateWiseDayEndDataDictionary.Add(keyOfTodayData, todayDayEndDataList);
                            //For Next Iteration Today Data DayEnd Will Be Previous DayEnd Data
                            yesterdayDataList = todayDayEndDataList;
                        }
                    }

                    dayEndFromDate = dayEndFromDate.AddDays(1);
                }
                if (dayEndCashLayout.ContainsKey(LastDayEndCash))
                {
                    InitializeGridLayout(dayEndCashLayout[LastDayEndCash], grdYesterdayEnd);
                }
                if (dayEndCashLayout.ContainsKey(TransactionDetails))
                {
                    InitializeGridLayout(dayEndCashLayout[TransactionDetails], grdGetLots);
                }

                _dictforexGivenDateRange = null;
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
            return result;
        }

        void grd_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {
            try
            {
                if (e.Column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_DATE))
                {
                    e.ValueList.ValueListItems.Insert(4, "(Today)", "(Today)");
                }
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

        void grdGetLots_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdGetLots.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdGetLots.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
                }
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

        void grdYesterdayEnd_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_DATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdYesterdayEnd.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdYesterdayEnd.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
                }
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
        public decimal CalculateBaseCurrencyValue(int baseCurrencyID, int localCurrencyID, decimal cashValueLocal, DateTime givenDate, int accountID)
        {
            decimal cashValueBase = 0;
            try
            {
                if (!baseCurrencyID.Equals(localCurrencyID))
                {
                    int companyID = int.Parse(CommonDataCache.CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"].ToString());
                    if (cashValueLocal != 0)
                    {
                        if (_dictforexGivenDateRange == null)
                            _dictforexGivenDateRange = ForexConverter.GetInstance(companyID, dtFromDate.DateTime).GetForexData(dtFromDate.DateTime, dtToDate.DateTime);
                        ConversionRate conversionRate = GetConversionRateFromCurrenciesForGivenDate(Convert.ToInt32(localCurrencyID), Convert.ToInt32(baseCurrencyID), givenDate, accountID);
                        if (conversionRate != null)
                        {
                            decimal rateVale = Convert.ToDecimal(conversionRate.RateValue);
                            if (conversionRate.ConversionMethod.Equals(Operator.D))
                            {
                                if (conversionRate.RateValue != 0)
                                    cashValueBase = cashValueLocal / rateVale;
                            }
                            else
                            {
                                cashValueBase = cashValueLocal * rateVale;
                            }
                        }
                    }
                }
                else
                {
                    cashValueBase = cashValueLocal;
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
            return cashValueBase;
        }

        public ConversionRate GetConversionRateFromCurrenciesForGivenDate(int fromCurrencyID, int toCurrencyID, DateTime date, int accountID)
        {
            ConversionRate objConversionRate = GetDefaultConversionrateObject();
            try
            {
                string key = Convert.ToString(fromCurrencyID + Seperators.SEPERATOR_7 + toCurrencyID).ToString() + Seperators.SEPERATOR_7 + Convert.ToDateTime(date.ToString()).Date;
                if (!_dictforexGivenDateRange.ContainsKey(accountID))
                {
                    accountID = 0;
                }
                if (_dictforexGivenDateRange.ContainsKey(accountID))
                {
                    if (_dictforexGivenDateRange[accountID].ContainsKey(key))
                    {
                        SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate = _dictforexGivenDateRange[accountID][key];
                        ConversionRate objavailabeConversionRate = GetAvailabeConversionRate(dateWiseConversionRate, date);
                        if (objavailabeConversionRate != null)
                        {
                            return objavailabeConversionRate;
                        }
                        else
                        {
                            return objConversionRate;
                        }
                    }
                    else
                    {
                        accountID = 0;
                        if (_dictforexGivenDateRange.ContainsKey(accountID))
                        {
                            if (_dictforexGivenDateRange[accountID].ContainsKey(key))
                            {
                                SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate = _dictforexGivenDateRange[accountID][key];
                                ConversionRate objavailabeConversionRate = GetAvailabeConversionRate(dateWiseConversionRate, date);
                                if (objavailabeConversionRate != null)
                                {
                                    return objavailabeConversionRate;
                                }
                                else
                                {
                                    return objConversionRate;
                                }
                            }
                        }
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
            return objConversionRate;
        }

        private ConversionRate GetDefaultConversionrateObject()
        {
            ConversionRate objConversionRate = new ConversionRate();
            objConversionRate.RateValue = 0.0;
            objConversionRate.ConversionMethod = Operator.M;
            return objConversionRate;
        }

        private ConversionRate GetAvailabeConversionRate(SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate, DateTime date)
        {
            ConversionRate objConversionRate = null;
            try
            {
                if (dateWiseConversionRate.ContainsKey(date.Date))
                {
                    objConversionRate = dateWiseConversionRate[date.Date];
                }
                else
                {
                    foreach (KeyValuePair<DateTime, ConversionRate> keyVal in dateWiseConversionRate)
                    {
                        if (keyVal.Key.Date < date.Date)
                        {
                            if (dateWiseConversionRate.ContainsKey(keyVal.Key.Date))
                            {
                                objConversionRate = dateWiseConversionRate[keyVal.Key.Date];
                                break;
                            }
                        }
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
            return objConversionRate;
        }

        /// <summary>
        /// Added by: Bharat Raturi, 24 jul 2014
        /// Copy the end day cash from the backup list to day end cash data list
        /// </summary>
        /// <param name="yesterdayDataList"></param>
        /// <param name="backupList"></param>
        /// <param name="todayDate"></param>
        private void AddInitialDayEndCach(GenericBindingList<CompanyAccountCashCurrencyValue> yesterdayDataList, List<CompanyAccountCashCurrencyValue> backupList, DateTime todayDate)
        {
            try
            {
                foreach (CompanyAccountCashCurrencyValue val in backupList)
                {
                    if (!yesterdayDataList.ContainsKey(val.AccountID.ToString() + ":" + val.LocalCurrencyID.ToString()) && val.Date < todayDate)
                    {
                        yesterdayDataList.Add(val);
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

        ctrlDataGrid ctrlgrdTodayDayEnd;
        string _todayenddata;

        private Dictionary<string, CashManagementLayout> dayEndCashLayout = new Dictionary<string, CashManagementLayout>();
        public void AddLayout(string key, CashManagementLayout value)
        {
            try
            {
                if (dayEndCashLayout.ContainsKey(key))
                {
                    dayEndCashLayout[key] = value;
                }
                else
                {
                    dayEndCashLayout.Add(key, value);
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


        bool _isNewGrid = true;
        private void CreateTabsAccordingToDate()
        {
            try
            {
                if (noOfDayEndCashTab != null && noOfDayEndCashTab.Count > 0)
                {
                    for (int counter = 0; counter < noOfDayEndCashTab.Count; counter++)
                    {
                        noOfDayEndCashTab[counter].CashLayout -= new EventHandler<EventArgs<CashManagementLayout>>(SetLayoutForDayEndCashGrid);
                        noOfDayEndCashTab[counter].Dispose();
                        noOfDayEndCashTab[counter] = null;
                    }
                }
                noOfDayEndCashTab.Clear();
                tabCntlDayEndData.Tabs.Clear();
                UltraGrid grdTodayDayEnd;
                if (_dateWiseDayEndDataDictionary != null && _dateWiseDayEndDataDictionary.Count > 0)
                {
                    _isNewGrid = true;
                    foreach (string Date in _dateWiseDayEndDataDictionary.Keys)
                    {
                        ctrlgrdTodayDayEnd = new ctrlDataGrid();
                        ctrlgrdTodayDayEnd.KeyForXML = DayEndCash;
                        ctrlgrdTodayDayEnd.CashLayout += new EventHandler<EventArgs<CashManagementLayout>>(SetLayoutForDayEndCashGrid);
                        CustomThemeHelper.SetThemeProperties(ctrlgrdTodayDayEnd, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CASH_MANAGEMENT);
                        grdTodayDayEnd = (UltraGrid)ctrlgrdTodayDayEnd.Controls["ultraGrid"];
                        grdTodayDayEnd.AfterColPosChanged += new AfterColPosChangedEventHandler(grdTodayDayEnd_AfterColPosChanged);
                        tabCntlDayEndData.Tabs.Add(Date, Date);
                        tabCntlDayEndData.Tabs[Date].TabPage.Controls.Add(grdTodayDayEnd);
                        grdTodayDayEnd.Dock = DockStyle.Fill;
                        List<string> visibleColumsYesterdayEnd = Prana.Utilities.MiscUtilities.GeneralUtilities.GetListFromString(_cashSavePreference.YesterdayEndColumn, ',');
                        if (visibleColumsYesterdayEnd.Count > 0)
                            HelperClass.BindGridTodayDayEndData(grdTodayDayEnd, _dateWiseDayEndDataDictionary[Date], visibleColumsYesterdayEnd);
                        else
                            HelperClass.BindGridTodayDayEndData(grdTodayDayEnd, _dateWiseDayEndDataDictionary[Date], null);
                        grdTodayDayEnd.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
                        if (_isdisplaychanged.Equals(true))
                            grdTodayDayEnd.DisplayLayout.Load(_todayenddata, PropertyCategories.All);

                        if (dayEndCashLayout.ContainsKey(DayEndCash))
                        {
                            InitializeGridLayout(dayEndCashLayout[DayEndCash], grdTodayDayEnd);
                        }
                        noOfDayEndCashTab.Add(ctrlgrdTodayDayEnd);
                    }
                    _isNewGrid = false;
                }
                else
                {
                    RefreshCalculatedDayEndGrid();
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

        bool _isdisplaychanged = false;
        void grdTodayDayEnd_AfterColPosChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                if (_isNewGrid.Equals(false))
                {
                    UltraGrid grd = sender as UltraGrid;
                    if (!(grd.Equals(null)))
                    {
                        _isdisplaychanged = true;
                        grd.DisplayLayout.Save(_todayenddata, Infragistics.Win.UltraWinGrid.PropertyCategories.All);
                        foreach (UltraTab tb in tabCntlDayEndData.Tabs)
                        {
                            grd = tb.TabPage.Controls[0] as UltraGrid;
                            if (grd != null)
                                grd.DisplayLayout.Load(_todayenddata, PropertyCategories.All);
                        }
                    }
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

        private void RefreshCalculatedDayEndGrid()
        {
            try
            {
                tabCntlDayEndData.Tabs.Clear();
                ctrlgrdTodayDayEnd = new ctrlDataGrid();
                CustomThemeHelper.SetThemeProperties(ctrlgrdTodayDayEnd, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CASH_MANAGEMENT);
                if (tabCntlDayEndData != null && tabCntlDayEndData.Parent != null)
                {
                    tabCntlDayEndData.Tabs.Add("0", "Day End Cash");
                    tabCntlDayEndData.Tabs[0].TabPage.Controls.Add(ctrlgrdTodayDayEnd);
                }
                ctrlgrdTodayDayEnd.Dock = DockStyle.Fill;
                noOfDayEndCashTab.Add(ctrlgrdTodayDayEnd);
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

        void CashManagementServices_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker(() => this.CashManagementServices_ConnectedEvent(sender, e)));
                        return;
                    }
                    GetDataAsync();
                }
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

        DateTime _previousDayDate;

        //private bool isgetLotsInitialized = false;
        private bool isyesterdayEndinitialized = false;
        private void InitializeGrids()
        {
            try
            {
                CashManagementPreferences objCash = new CashManagementPreferences();
                _cashSavePreference = objCash.GetPreferences();
                {
                    #region grdGetLots Binding Section

                    CashImpactToBind = new GenericBindingList<TransactionEntry>();
                    TransactionEntry entryToInitialezeGrid = new TransactionEntry();

                    CashImpactToBind.Add(entryToInitialezeGrid);
                    grdGetLots.DataSource = CashImpactToBind;
                    grdGetLots.DisplayLayout.Bands[0].AddButtonCaption = "TransactionEntries";
                    CashImpactToBind.Clear();
                    if (_cashSavePreference.TranscationDetailColumn != "")
                    {
                        GetPreferenceTranscation();
                    }
                    else
                    {
                        HelperClass.SetColumnDisplayNames(grdGetLots, null);
                    }
                    #endregion

                    #region grdYesterdayEnd Binding Section

                    CompanyAccountCashCurrencyValue dayEndToInitializeGrid = new CompanyAccountCashCurrencyValue();
                    DayEndDataToBind.Add(dayEndToInitializeGrid);
                    grdYesterdayEnd.DataSource = DayEndDataToBind;
                    DayEndDataToBind.Clear();

                    if (_cashSavePreference.YesterdayEndColumn != "")
                    {
                        GetPreferenceYesterday();
                    }
                    else
                    {
                        if (!isyesterdayEndinitialized)
                        {
                            HelperClass.SetDayEndCashDisplayNames(grdYesterdayEnd, null);
                            isyesterdayEndinitialized = true;
                        }
                    }
                    #endregion
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

        void CashManagementServices_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker(() => this.CashManagementServices_DisconnectedEvent(sender, e)));
                        return;
                    }
                    ClearData();
                }
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

        public void ClearData()
        {
            try
            {
                if (CashImpactToBind != null)
                    CashImpactToBind.Clear();
                if (GetedTransactions != null)
                    GetedTransactions.Clear();
                DayEndDataToBind.Clear();
                _dateWiseDayEndDataDictionary = null;
                CreateTabsAccordingToDate();
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

        public async void GetDataAsync()
        {
            try
            {
                //Raturi: Subscribe for cash journals and day end cash
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5967
                _proxy.UnSubscribe();
                SubscribeForCashTransactions();
                SubscribeForDayEndCash();
                changeStatus(false);
                ClearData();
                List<object> dtDates = new List<object>();
                dtDates.Add(dtFromDate.DateTime);
                dtDates.Add(dtToDate.DateTime);
                dtDates.Add(ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(','));

                bool result = await getData(dtDates);

                if (!this.IsDisposed)
                {
                    System.Threading.Tasks.Task<GenericBindingList<Transaction>> GetedTransactionsTask = System.Threading.Tasks.Task<GenericBindingList<Transaction>>.Factory.StartNew(() => DeepCopyHelper.Clone(CashDataManager.GetInstance().CashImpact));
                    GetedTransactions = await GetedTransactionsTask;
                    CashImpactToBind.Clear();
                    HelperClass.GlobalGridSetting(grdGetLots.DisplayLayout.Bands[0]);
                    HelperClass.GlobalGridSetting(grdYesterdayEnd.DisplayLayout.Bands[0]);

                    //PRANA-24172. Parallel for each was used here. Now I have reverted that as this was creating problem.
                    foreach (Transaction t in _getedTransactions)
                    {
                        List<TransactionEntry> lsCashTransactionEntries = CashRulesHelper.GetCashTransactionEntries(t);
                        foreach (TransactionEntry transactionEntry in lsCashTransactionEntries)
                        {
                            if (transactionEntry != null)
                                CashImpactToBind.Add(transactionEntry);
                        }
                    }

                    DayEndDataToBind.AddList(DeepCopyHelper.Clone(CashDataManager.GetInstance().DayEndData));
                    _previousDate = _previousDayDate.ToShortDateString();
                    changeStatus(true);
                    if (dayEndCashLayout.ContainsKey(LastDayEndCash))
                    {
                        InitializeGridLayout(dayEndCashLayout[LastDayEndCash], grdYesterdayEnd);
                    }
                    if (dayEndCashLayout.ContainsKey(TransactionDetails))
                    {
                        InitializeGridLayout(dayEndCashLayout[TransactionDetails], grdGetLots);
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

        private void changeStatus(bool isWorkCompleted)
        {
            try
            {
                if (isWorkCompleted)
                {
                    btnGet.Text = "Get Data";
                    ultraExpnGrpBxAllTransactions.Text = "Transaction Details";
                    ugbxDayEndParams.Enabled = true;

                    toolStripStatusLabel1.Text = "";

                }
                else
                {
                    btnGet.Text = "Getting.....";
                    toolStripStatusLabel1.Text = "Getting.....";
                    ultraExpnGrpBxAllTransactions.Text = "Getting Transaction Details .....";
                    ugbxDayEndParams.Enabled = false;

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

        private void changeStatusOnCalculation(bool isWorkCompleted)
        {
            try
            {
                if (isWorkCompleted)
                {
                    btnRunBatch.Text = "Calculate";
                    toolStripStatusLabel1.Text = "";
                    ugbxDayEndParams.Enabled = true;
                }
                else
                {
                    btnRunBatch.Text = "Calculating.....";
                    toolStripStatusLabel1.Text = "Calculating.....";
                    ugbxDayEndParams.Enabled = false;

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

        /// <summary>
        /// Change Status On Save
        /// </summary>
        /// <param name="isWorkCompleted"></param>
        private void ChangeStatusOnSave(bool isWorkCompleted)
        {
            try
            {
                if (isWorkCompleted)
                {
                    btnSave.Text = "Save";
                    toolStripStatusLabel1.Text = "Day End Cash Saved.";
                    ugbxDayEndParams.Enabled = true;
                }
                else
                {
                    btnSave.Text = "Saving.....";
                    toolStripStatusLabel1.Text = "Saving.....";
                    ugbxDayEndParams.Enabled = false;

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

        private async Task<bool> getData(List<object> dtDates)
        {
            try
            {
                List<Transaction> listCashImpact = new List<Transaction>();
                List<CompanyAccountCashCurrencyValue> listdayEndData = new List<CompanyAccountCashCurrencyValue>();
                listCashImpact = await CashDataManager.GetInstance().GetCashImpact((DateTime)dtDates[0], (DateTime)dtDates[1], (string)dtDates[2]);
                _previousDayDate = ((DateTime)dtDates[0]).AddDays(-1);
                listdayEndData = await CashDataManager.GetInstance().GetDayEndData(_previousDayDate, Convert.ToDateTime(dtDates[1]), (string)dtDates[2]);
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
            return true;
        }
        #endregion

        #region UI Functionality

        private async void btnRunBatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CachedDataManager.GetInstance.ValidateNAVLockDate(dtFromDate.DateTime))
                {
                    MessageBox.Show("The start date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                        + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                {
                    changeStatusOnCalculation(false);

                    //Getting DateWiseYesterdayDataDictionary
                    GenericBindingList<CompanyAccountCashCurrencyValue> yesterdayDataList = new GenericBindingList<CompanyAccountCashCurrencyValue>();
                    List<CompanyAccountCashCurrencyValue> backupList = new List<CompanyAccountCashCurrencyValue>();

                    foreach (CompanyAccountCashCurrencyValue yesterdayData in DayEndDataToBind)
                    {
                        if (!yesterdayDataList.Contains(yesterdayData))
                        {
                            yesterdayDataList.Add(yesterdayData);
                            backupList.Add(yesterdayData);
                        }
                    }

                    //True if Any PriviousdayEndDataExist Or Any Selected dates Trasactions Exist
                    DialogResult dlgResult = new DialogResult();
                    if (yesterdayDataList.Count == 0)
                        dlgResult = ConfirmationMessageBox.Display(_previousDate + " DayEndCash not present. would you like to continue with previous day cash ?", "DayEndCash");
                    else
                        dlgResult = DialogResult.Yes;

                    bool result = false;
                    if (dlgResult == DialogResult.Yes)
                        result = await System.Threading.Tasks.Task.Run(() => { return CalculateDateWiseDayEndData(yesterdayDataList, backupList); });
                    else
                        MessageBox.Show(_previousDate + " Day End Cash Not Present ! So Can't Create DayEnd Data for Future Dates !");

                    CreateTabsAccordingToDate();
                }
                else
                {
                    MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            finally
            {
                changeStatusOnCalculation(true);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dateWiseDayEndDataDictionary != null && _dateWiseDayEndDataDictionary.Count > 0)
                {
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                    {
                        ChangeStatusOnSave(false);
                        CashDataManager.GetInstance().SaveDayEndData(_dateWiseDayEndDataDictionary, null, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(','));
                        _dateWiseDayEndDataDictionary = null;
                        ChangeStatusOnSave(true);

                    }
                    else
                    {
                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "No Day Cash Data Available To Be Saved.";
                }
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

        private void CashForm_Load(object sender, EventArgs e)
        {

            try
            {
                SubscribeForCashTransactions();
                InitializeGrids();

                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                ctrlMasterFundAndAccountsDropdown1.Setup();
                ctrlMasterFundAndAccountsDropdown1.CheckStateChanged += ctrlMasterFundAndAccountsDropdown1_CheckStateChanged;

                if (dayEndCashLayout.ContainsKey(LastDayEndCash))
                {
                    InitializeGridLayout(dayEndCashLayout[LastDayEndCash], grdYesterdayEnd);
                }
                if (dayEndCashLayout.ContainsKey(TransactionDetails))
                {
                    InitializeGridLayout(dayEndCashLayout[TransactionDetails], grdGetLots);
                }
                ctrlDayEndDataGrid1.HideContextMenu();
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

        private void ctrlMasterFundAndAccountsDropdown1_CheckStateChanged(object sender, EventArgs e)
        {
            ClearData();
        }

        private void SetButtonsColor()
        {
            try
            {
                btnRunBatch.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRunBatch.ForeColor = System.Drawing.Color.White;
                btnRunBatch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRunBatch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRunBatch.UseAppStyling = false;
                btnRunBatch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGet.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGet.ForeColor = System.Drawing.Color.White;
                btnGet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGet.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGet.UseAppStyling = false;
                btnGet.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void btnGetFx_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime(dtToDate.DateTime), Convert.ToDateTime(dtFromDate.DateTime)) >= 0)
                {
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                    {
                        if (ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetNoOfCheckedItems() < 1)
                        {
                            MessageBox.Show("Please select at least one account to proceed", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        GetDataAsync();
                    }
                    else
                    {
                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("To Date is before From Date", "Day End Cash", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ClearData();
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
        #endregion

        #region Subscribe Section

        DuplexProxyBase<ISubscription> _proxy;
        public void CreateSubscriptionServicesProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
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

        private void SubscribeForDayEndCash()
        {
            try
            {
                if (_proxy != null)
                {
                    FilterDataByExactDate filterdata = new FilterDataByExactDate();
                    filterdata.GivenDate = dtFromDate.DateTime.AddDays(-1).Date;
                    List<FilterData> filters = new List<FilterData>();
                    filters.Add(filterdata);

                    try
                    {
                        _proxy.Subscribe(Topics.Topic_DayEndCash, filters);
                    }
                    catch
                    {
                        throw new Exception("TradeService not connected");
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

        private void SubscribeForCashTransactions()
        {
            try
            {
                if (_proxy != null)
                {
                    FilterDataByFromDate filterdata = new FilterDataByFromDate();
                    filterdata.FromDate = dtFromDate.DateTime.Date;
                    List<FilterData> filters = new List<FilterData>();
                    filters.Add(filterdata);
                    _proxy.Subscribe(Topics.Topic_CashData, filters);
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
        #endregion

        #region IPublishing Members

        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker del =
                            delegate
                            {
                                Publish(e, topicName);
                            };
                        this.BeginInvoke(del);
                        return;
                    }
                    System.Object[] publishDataList = null;
                    switch (topicName)
                    {
                        case Topics.Topic_CashData:
                            publishDataList = (System.Object[])e.EventData;
                            foreach (Object obj in publishDataList)
                            {
                                Transaction transaction = (Transaction)obj;
                                if (!transaction.GetActivitySource().Equals((byte)ActivitySource.Trading))
                                {
                                    List<TransactionEntry> lsTRansactionEntry = CashRulesHelper.GetCashTransactionEntries(transaction);
                                    foreach (TransactionEntry item in lsTRansactionEntry)
                                    {
                                        if (item.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                                        {
                                            if (CashImpactToBind.Contains(item))
                                            {
                                                CashImpactToBind.Remove(item);
                                            }
                                        }
                                        else
                                        {
                                            if (!CashImpactToBind.Contains(item))
                                            {
                                                //Have to check for cash transction Entry
                                                CashImpactToBind.Add(item);
                                            }
                                            else
                                            {
                                                CashImpactToBind.Update(item);
                                            }
                                        }
                                    }
                                }
                                //From Daily Calculation TaxlotState Come with only "New state"
                                else if (transaction.IsDailyCalculationTransaction())
                                {

                                    if (GetedTransactions.Contains(transaction))
                                    {
                                        GetedTransactions.Update(transaction);
                                    }
                                    else
                                    {
                                        GetedTransactions.Add(transaction);
                                        List<TransactionEntry> lsTRansactionEntry = CashRulesHelper.GetCashTransactionEntries(transaction);
                                        CashImpactToBind.AddList(lsTRansactionEntry);
                                    }
                                }
                                else
                                {
                                    if (transaction.GetTaxlotState() == ApplicationConstants.TaxLotState.New.ToString())
                                    {
                                        GetedTransactions.Add(transaction);
                                        List<TransactionEntry> lsTRansactionEntry = CashRulesHelper.GetCashTransactionEntries(transaction);
                                        foreach (TransactionEntry transactionEntry in lsTRansactionEntry)
                                        {
                                            CashImpactToBind.Add(transactionEntry);
                                        }
                                    }
                                    else if (transaction.GetTaxlotState() == ApplicationConstants.TaxLotState.Deleted.ToString())
                                    {
                                        if (!string.IsNullOrEmpty(transaction.TransactionID) && GetedTransactions.Contains(transaction))
                                        {
                                            GetedTransactions.Remove(transaction);
                                            List<TransactionEntry> lsTRansactionEntry = CashRulesHelper.GetCashTransactionEntries(transaction);
                                            if (CashImpactToBind != null)
                                            {
                                                foreach (TransactionEntry transactionEntry in lsTRansactionEntry)
                                                {
                                                    TransactionEntry trEntry = CashRulesHelper.GetTransactionEntryToRemove(CashImpactToBind, transactionEntry);
                                                    if (trEntry != null)
                                                    {
                                                        CashImpactToBind.Remove(trEntry);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (transaction.GetTaxlotState() == ApplicationConstants.TaxLotState.Updated.ToString())
                                    {
                                        if (!string.IsNullOrEmpty(transaction.TransactionID) && GetedTransactions.Contains(transaction))
                                        {
                                            GetedTransactions.Update(transaction);
                                            List<TransactionEntry> lsTRansactionEntry = CashRulesHelper.GetCashTransactionEntries(transaction);
                                            if (CashImpactToBind != null)
                                            {
                                                foreach (TransactionEntry transactionEntry in lsTRansactionEntry)
                                                {
                                                    TransactionEntry trEntry = CashRulesHelper.GetTransactionEntryToRemove(CashImpactToBind, transactionEntry);
                                                    if (trEntry != null)
                                                    {
                                                        CashImpactToBind.Remove(trEntry);
                                                        CashImpactToBind.Add(transactionEntry);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        case Topics.Topic_DayEndCash:
                            publishDataList = (System.Object[])e.EventData;
                            DayEndDataToBind.Clear();
                            CompanyAccountCashCurrencyValue dayEndCash;
                            foreach (Object obj in publishDataList)
                            {
                                dayEndCash = obj as CompanyAccountCashCurrencyValue;
                                if (dayEndCash != null)
                                    DayEndDataToBind.Add(dayEndCash);
                            }
                            CashRulesHelper.SetDayEndCash_NameValues(DayEndDataToBind);
                            break;

                        case Topics.Topic_RevaluationJournal:
                            publishDataList = (System.Object[])e.EventData;
                            foreach (Object obj in publishDataList)
                            {
                                toolStripStatusLabel1.Text = obj as string;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public string getReceiverUniqueName()
        {
            try
            {
                return "CashForm";
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
            return null;
        }
        #endregion

        private void grdGetLots_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e == null || e.Row == null)
                {
                    return;
                }
                else
                {
                    #region Hide local currency cash journal transaction entries
                    //CHMW-3114 [Foreign Positions Settling in Base Currency] [Implementation] [Cash Management] Hide local currency cash journal transaction entries
                    TransactionEntry trEntry = (TransactionEntry)e.Row.ListObject;
                    if (trEntry != null
                        && trEntry.TransactionSource == CashTransactionType.SettlementTransaction
                        && CashDataManager.GetInstance().GetCashPreferences(trEntry.AccountID) != null
                        && !CashDataManager.GetInstance().GetCashPreferences(trEntry.AccountID).IsCashSettlementEntriesVisible)
                    {
                        e.Row.Hidden = true;
                    }
                    else
                    {
                        e.Row.Hidden = false;
                    }
                    #endregion
                }
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

        private void grdYesterdayEnd_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                HelperClass.RowColorSettings(e);
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

        private void grdGetLots_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                HelperClass.SummarySettings(e);
                grdGetLots.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                //Added as not to group by  Transactionid on this form which is implicitly grouped by Tranasactionid in SummarySettings
                e.Layout.Bands[0].SortedColumns.Remove("TransactionID");
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

        private void grdGetLots_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                if (grdGetLots.Rows.IsGroupByRows)
                {
                    grdGetLots.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
                }
                else
                {
                    grdGetLots.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                }
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

        private void grdYesterdayEnd_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                if (grdYesterdayEnd.Rows.IsGroupByRows)
                {
                    grdYesterdayEnd.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
                }
                else
                {
                    grdYesterdayEnd.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                }
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

        private void GetPreferenceYesterday()
        {
            try
            {
                List<string> visibleColumsYesterdayEnd = Prana.Utilities.MiscUtilities.GeneralUtilities.GetListFromString(_cashSavePreference.YesterdayEndColumn, ',');
                HelperClass.SetDayEndCashDisplayNames(grdYesterdayEnd, visibleColumsYesterdayEnd);
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

        private void GetPreferenceTranscation()
        {
            try
            {
                List<string> visibleColumsTranscationDetail = Prana.Utilities.MiscUtilities.GeneralUtilities.GetListFromString(_cashSavePreference.TranscationDetailColumn, ',');
                HelperClass.SetColumnDisplayNames(grdGetLots, visibleColumsTranscationDetail);
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

        private void ultraDateTimeEditor1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ClearData();
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

        private void ultraExpnGrpBxAllTransactions_ExpandedStateChanged(object sender, EventArgs e)
        {
            if (ultraExpnGrpBxAllTransactions.Expanded)
            {
                if (ultraExpnGrpBxAllTransactions.Expanded)
                    splitContainer2.SplitterDistance = this.FindForm().Height / 2 - 50;
                else
                    splitContainer2.SplitterDistance = 25;
            }
            else
            {
                splitContainer2.SplitterDistance = this.FindForm().Height - 25;
            }
        }

        private void grdYesterdayEnd_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdYesterdayEnd);
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

        private void grdGetLots_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            HelperClass.GroupByRowSetting(e);
        }

        private void grdGetLots_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdGetLots);
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

        #region Export to Excel

        private const string LastDayEndCash = "DayEndCash_LastDayEndCash";
        private const string TransactionDetails = "DayEndCash_TransactionDetails";
        private const string DayEndCash = "DayEndCash_DayEndCash";

        private string _keyForXML;
        public string KeyForXML
        {
            get { return _keyForXML; }
            set { _keyForXML = value; }
        }

        private UltraGrid _activeGrid;
        public UltraGrid ActiveGrid
        {
            get { return _activeGrid; }
            set { _activeGrid = value; }
        }

        private void grdGetLots_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    ActiveGrid = sender as UltraGrid;
                    KeyForXML = TransactionDetails;
                }
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

        private void grdYesterdayEnd_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    ActiveGrid = sender as UltraGrid;
                    KeyForXML = LastDayEndCash;
                }
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

        private void exportCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExportToExcelHelper.ExportToExcel(ActiveGrid))
                {
                    toolStripStatusLabel1.Text = "Report Successfully Saved.";
                }
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
        #endregion

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGridLayout saveGridLayout = new SaveGridLayout();
                CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(ActiveGrid, "DayEndCash");
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, KeyForXML);
                AddLayout(KeyForXML, cashManagementLayout);
            }
            catch (Exception Ex)
            {
                bool rethrow = Logger.HandleException(Ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void InitializeGridLayout(CashManagementLayout cashManagementLayout, UltraGrid grd)
        {
            try
            {
                if (cashManagementLayout != null)
                {
                    if (cashManagementLayout.GroupByColumnsCollection.Count > 0)
                    {
                        //Set GroupBy Columns
                        bool flag = true;
                        int GroupedColumns = grd.DisplayLayout.Bands[0].SortedColumns.Count;
                        for (int i = 0; i < GroupedColumns - 1; i++)
                        {
                            grd.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                        }
                        foreach (string item in cashManagementLayout.GroupByColumnsCollection)
                        {
                            if (grd.DisplayLayout.Bands[0].Columns.Exists(item) && !grd.DisplayLayout.Bands[0].SortedColumns.Contains(item))
                            {
                                grd.DisplayLayout.Bands[0].SortedColumns.Add(item, true, true);
                                if (grd.DisplayLayout.Bands[0].SortedColumns.Count == 1)
                                {
                                    flag = false;
                                }
                                if (flag)
                                {
                                    grd.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                                    flag = false;
                                }
                            }
                        }
                        grd.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                        grd.DisplayLayout.RefreshSummaries();
                    }

                    ColumnFiltersCollection columnFilters = grd.DisplayLayout.Bands[0].ColumnFilters;
                    ///TODO : When we apply the custom filters we need to change the code so the filters won't be on a common field
                    columnFilters.ClearAllFilters();

                    foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
                    {
                        col.Hidden = true;
                    }

                    foreach (CashGridColumn selectedCols in cashManagementLayout.SelectedColumnsCollection)
                    {
                        UltraGridColumn col = null;
                        if (grd.DisplayLayout.Bands[0].Columns.Exists(selectedCols.Name))
                        {
                            col = grd.DisplayLayout.Bands[0].Columns[selectedCols.Name];
                            if (col != null)
                            {
                                col.HiddenWhenGroupBy = DefaultableBoolean.False;
                                col.Hidden = selectedCols.Hidden;
                                col.Header.Fixed = selectedCols.IsHeaderFixed;


                                if (!grd.DisplayLayout.Bands[0].SortedColumns.Contains(col))
                                {
                                    col.SortIndicator = selectedCols.SortIndicator;
                                    if (col.SortIndicator.Equals(SortIndicator.Ascending))
                                        grd.DisplayLayout.Bands[0].SortedColumns.Add(col, false);
                                    if (col.SortIndicator.Equals(SortIndicator.Descending))
                                        grd.DisplayLayout.Bands[0].SortedColumns.Add(col, true);
                                }
                                grd.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                                if (selectedCols.Width == 0)
                                {
                                    col.AutoSizeMode = ColumnAutoSizeMode.Default;
                                }
                                else
                                {
                                    col.Width = selectedCols.Width;
                                }
                                col.Header.VisiblePosition = selectedCols.Position;
                                if (selectedCols.FilterConditionList.Count > 0)
                                {
                                    foreach (FilterCondition filCond in selectedCols.FilterConditionList)
                                    {
                                        if ((selectedCols.Name.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_ENTRYDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_DATE)) && selectedCols.FilterConditionList.Count == 1 && selectedCols.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.StartsWith && selectedCols.FilterConditionList[0].CompareValue.Equals("(Today)"))
                                        {
                                            grd.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                                        }
                                        else
                                        {
                                            grd.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(filCond);
                                        }
                                    }
                                    grd.DisplayLayout.Bands[0].ColumnFilters[col].LogicalOperator = selectedCols.FilterLogicalOperator;
                                }
                            }
                        }
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

        private void SetLayoutForDayEndCashGrid(object sender, EventArgs<CashManagementLayout> e)
        {
            try
            {
                AddLayout(DayEndCash, e.Value);
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

        private void grdGetLots_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdYesterdayEnd_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdGetLots_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            try
            {
                if (e.Row.ListObject == null)
                {
                    return;
                }

                if (transactionDetails != null)
                {
                    transactionDetails.Close();
                }
                transactionDetails = new frmAccountTransactionDetails();
                transactionDetails.FormClosing += new FormClosingEventHandler(transactionDetails_FormClosing);
                transactionDetails.SetUp(Convert.ToString(e.Row.Cells["TransactionID"].Value));
                transactionDetails.Show();
                transactionDetails.BringToFront();
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
        void transactionDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                transactionDetails.FormClosing -= new FormClosingEventHandler(transactionDetails_FormClosing);
                transactionDetails = null;
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

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
