using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.Enumerators;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PostTrade;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.CashManagement
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class CashmgmtServices : ICashManagementService, IPublishing, IDisposable
    {
        #region Services Ref
        IFixedIncomeAdapter _fixedIncomeAdapter = null;
        public IFixedIncomeAdapter FixedIncomeAdapter
        {
            set
            {
                _fixedIncomeAdapter = value;
            }
        }

        IPranaPositionServices _pranaPositionServices = null;
        public IPranaPositionServices PranaPositionServices
        {
            set { _pranaPositionServices = value; }
        }

        private IActivityServices _activityServices;
        //public IActivityServices ActivityServices
        //{
        //    set { _activityServices = value; }
        //}
        #endregion

        #region Members
        /// <summary>
        /// The locker object
        /// </summary>
        private readonly object _lockerObj = new object();

        /// <summary>
        /// The locker daily credit limit
        /// </summary>
        private readonly object _lockerDailyCreditLimit = new object();

        /// <summary>
        /// The reval running lock
        /// </summary>
        private readonly object _revalRunningLock = new object();

        /// <summary>
        /// The daily credit limit values
        /// </summary>
        private DataTable _dailyCreditLimitValues;

        /// <summary>
        /// The dictionary cash preferences
        /// </summary>
        Dictionary<int, CashPreferences> _dictCashPreferences;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CashmgmtServices"/> class.
        /// </summary>
        public CashmgmtServices()
        {
            CreatePublishingProxy();
        }
        #endregion

        #region ICashManagementService Members

        /// <summary>
        /// Gets the cash impact.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="AccountIDs">The account i ds.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public async Task<List<Transaction>> GetCashImpact(DateTime fromDate, DateTime toDate, String AccountIDs, int userID)
        {
            LogMessage(userID, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_DAY_END_CASH, fromDate.ToString(), toDate.ToString(), AccountIDs);
            System.Threading.Tasks.Task<int> rslt = System.Threading.Tasks.Task<int>.Factory.StartNew(() => SaveCashActionsInAudit(userID, AccountIDs, fromDate, toDate, CashAuditManager.Com_GetDayEndCash));
            List<Transaction> listTransactions = await System.Threading.Tasks.Task.Run(() => { return GetCashImpactAsync(fromDate, toDate, AccountIDs); });
            LogMessage(userID, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_DAY_END_CASH, string.Empty, string.Empty, string.Empty);
            return listTransactions;
        }

        /// <summary>
        /// Gets the day end cash.
        /// </summary>
        /// <param name="givenDate">The given date.</param>
        /// <param name="dtEndDate">The dt end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public async Task<List<CompanyAccountCashCurrencyValue>> GetDayEndCash(DateTime givenDate, DateTime dtEndDate, string accountIDs)
        {
            List<CompanyAccountCashCurrencyValue> listDayEndCash = await System.Threading.Tasks.Task.Run(() => { return GetDayEndCashAsync(givenDate, dtEndDate, accountIDs); });
            return listDayEndCash;
        }

        /// <summary>
        /// IsAutoUpdateOptionsUDAWithUnderlyingUpdate
        /// </summary>
        /// <returns></returns>
        public bool IsAutoUpdateOptionsUDAWithUnderlyingUpdate()
        {
            bool isAutoUpdateOptionsUDAWithUnderlyingUpdate = true;
            try
            {
                isAutoUpdateOptionsUDAWithUnderlyingUpdate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAutoUpdateOptionsUDAWithUnderlyingUpdate"]);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return isAutoUpdateOptionsUDAWithUnderlyingUpdate;
        }
        /// <summary>
        /// Gets the day end data in base currency.
        /// </summary>
        /// <param name="givenDate">The given date.</param>
        /// <param name="IsAccrualsNeeded">if set to <c>true</c> [is accruals needed].</param>
        /// <returns></returns>
        public GenericDayEndData GetDayEndDataInBaseCurrency(DateTime givenDate, bool IsAccrualsNeeded, bool isIncludeTradingDayAccruals = true)
        {
            GenericDayEndData genericDayEndDataValue = new GenericDayEndData();
            try
            {
                DataSet ds = CashDataManager.GetDayEndDataInBaseCurrency(givenDate, IsAccrualsNeeded, isIncludeTradingDayAccruals);
                if (ds != null)
                {
                    //Table 0 have Account wise start of day cash values
                    if (ds.Tables["Table"] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (!genericDayEndDataValue.StartOfDayAccountWiseCash.ContainsKey(Int32.Parse(row["FundID"].ToString())))
                                genericDayEndDataValue.StartOfDayAccountWiseCash.Add(Int32.Parse(row["FundID"].ToString()), double.Parse(row["Cash"].ToString()));
                        }
                    }

                    //Table 1 have Account wise day cash values
                    if (ds.Tables["Table1"] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            if (!genericDayEndDataValue.DayAccountWiseCash.ContainsKey(Int32.Parse(row["FundID"].ToString())))
                                genericDayEndDataValue.DayAccountWiseCash.Add(Int32.Parse(row["FundID"].ToString()), new Tuple<double, double>(double.Parse(row["DR"].ToString()), double.Parse(row["CR"].ToString())));
                        }
                    }

                    //Table 2 have Account wise start of day accrual values
                    if (IsAccrualsNeeded && ds.Tables["Table2"] != null && ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[2].Rows)
                        {
                            int fundid = Int32.Parse(row["FundID"].ToString());
                            int currencyid = Int32.Parse(row["CurrencyID"].ToString());
                            if (!genericDayEndDataValue.StartOfDayAccountWiseAccruals.ContainsKey(fundid))
                            {
                                Dictionary<int, double> currencyCash = new Dictionary<int, double>();
                                currencyCash.Add(currencyid, double.Parse(row["Cash"].ToString()));
                                genericDayEndDataValue.StartOfDayAccountWiseAccruals.Add(fundid, currencyCash);
                            }
                            else if (!genericDayEndDataValue.StartOfDayAccountWiseAccruals[fundid].ContainsKey(currencyid))
                            {
                                genericDayEndDataValue.StartOfDayAccountWiseAccruals[fundid].Add(currencyid, double.Parse(row["Cash"].ToString()));
                            }
                        }
                    }

                    //Table 3 have Account wise day accrual values
                    if (IsAccrualsNeeded && ds.Tables["Table3"] != null && ds.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[3].Rows)
                        {
                            int fundid = Int32.Parse(row["FundID"].ToString());
                            int currencyid = Int32.Parse(row["CurrencyID"].ToString());
                            if (!genericDayEndDataValue.DayAccountWiseAccruals.ContainsKey(fundid))
                            {
                                Dictionary<int, double> currencyCash = new Dictionary<int, double>();
                                currencyCash.Add(currencyid, double.Parse(row["Cash"].ToString()));
                                genericDayEndDataValue.DayAccountWiseAccruals.Add(fundid, currencyCash);
                            }
                            else if (!genericDayEndDataValue.DayAccountWiseAccruals[fundid].ContainsKey(currencyid))
                            {
                                genericDayEndDataValue.DayAccountWiseAccruals[fundid].Add(currencyid, double.Parse(row["Cash"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return genericDayEndDataValue;
        }

        /// <summary>
        /// Gets the transactions beetween two dates.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="activitySource">The activity source.</param>
        /// <returns></returns>
        public List<Transaction> GetTransactionsBeetweenTwoDates(DateTime startDate, DateTime endDate, String accountIDs, int activitySource, int userId)
        {
            List<Transaction> lsTransactions = new List<Transaction>();
            try
            {
                LogMessage(userId, LoggingConstants.REQUEST_RECEIVED, "Get " + ((CashManagementEnums.ActivitySource)activitySource).ToString(), startDate.ToString(), endDate.ToString(), accountIDs);
                List<TransactionEntry> lsTransactionsEntries = CashDataManager.GetValueBeetweenTwoDates<TransactionEntry>(startDate, endDate, "TransactionEntry", accountIDs, activitySource);
                lsTransactions = CashRulesHelper.getTransactionsFromTransactionEntries(lsTransactionsEntries);
                lsTransactions = CashRulesHelper.SetCashDataNameValues(lsTransactions);
                LogMessage(userId, LoggingConstants.RESPONSE_SENT, "Get " + ((CashManagementEnums.ActivitySource)activitySource).ToString(), string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsTransactions;
        }

        /// <summary>
        /// Gets the exceptional trading data.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public List<TaxLot> GetExceptionalTradingData(DateTime startDate, DateTime endDate, string accountIDs)
        {
            List<TaxLot> lsExceptions = new List<TaxLot>();
            try
            {
                lsExceptions = _pranaPositionServices.GetCashJournalExceptionalTransactions(startDate, endDate, accountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsExceptions;
        }

        /// <summary>
        /// Gets the transactions for date range.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public List<TaxLot> GetTransactionsForDateRange(DateTime fromDate, DateTime toDate)
        {
            List<TaxLot> lsTaxlots = new List<TaxLot>();
            try
            {
                if (_pranaPositionServices != null)
                {
                    lsTaxlots = _pranaPositionServices.GetTransactionsForDateRange(fromDate, toDate);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsTaxlots;
        }

        /// <summary>
        /// Gets the open positions date wise.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public List<TaxLot> GetOpenPositionsDateWise(DateTime date)
        {
            List<TaxLot> lsTaxlots = new List<TaxLot>();
            try
            {
                if (_pranaPositionServices != null)
                {
                    //'PMGetAccountOpenPositionsForDateBase_New' get OpenPositions upto one day before
                    lsTaxlots = _pranaPositionServices.GetOpenPositions(date);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsTaxlots;
        }

        /// <summary>
        /// Gets the open positions for daily calculation.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="AccountIDs">The account i ds.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<TaxLot>>> GetOpenPositionsForDailyCalculation(DateTime fromDate, DateTime toDate, String AccountIDs, int userID)
        {
            LogMessage(userID, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_DAILY_CALCULATION, fromDate.ToString(), toDate.ToString(), AccountIDs);
            System.Threading.Tasks.Task<int> rslt = System.Threading.Tasks.Task<int>.Factory.StartNew(() => SaveCashActionsInAudit(userID, AccountIDs, fromDate, toDate, CashAuditManager.Com_GetMTMData));
            Dictionary<string, List<TaxLot>> dicPositionsForDailyCalculation = await System.Threading.Tasks.Task.Run(() => { return GetOpenPositionsForDailyCalculationAsync(fromDate, toDate, AccountIDs); });
            LogMessage(userID, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_DAILY_CALCULATION, string.Empty, string.Empty, string.Empty);
            return dicPositionsForDailyCalculation;
        }

        /// <summary>
        /// Gets the cash preferences.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, CashPreferences> GetCashPreferences()
        {
            try
            {
                lock (_lockerObj)
                {
                    if (_dictCashPreferences == null)
                        _dictCashPreferences = CashDataManager.GetCashPreferences();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return _dictCashPreferences;
        }

        /// <summary>
        /// Calculates the daily cash impact.
        /// </summary>
        /// <param name="dicOpenPositionsAndTransactionsDateWise">The dic open positions and transactions date wise.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public List<TaxLot> CalculateDailyCashImpact(Dictionary<string, List<TaxLot>> dicOpenPositionsAndTransactionsDateWise, int userID)
        {
            try
            {
                LogMessage(userID, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.CALCULATE_DAILY_CASH, string.Empty, string.Empty, string.Empty);
                Task<int>.Factory.StartNew(() => SaveCashActionsInAudit(userID, string.Empty, null, null, CashAuditManager.Com_CalculateMTMData));
                List<TaxLot> lsTaxlot = null; List<PranaBasicMessage> lsTaxLotWithAccruedInterest;
                DateTime dateOfCalculatedData; decimal markFromDb, diffInMarkPrice = 0.0m;
                List<TaxLot> lsCalculatedTaxlots = new List<TaxLot>();
                foreach (string givenDate in dicOpenPositionsAndTransactionsDateWise.Keys)
                {
                    dateOfCalculatedData = Convert.ToDateTime(givenDate);
                    lsTaxlot = dicOpenPositionsAndTransactionsDateWise[givenDate];
                    lsTaxLotWithAccruedInterest = new List<PranaBasicMessage>();
                    foreach (TaxLot taxlotToCalculate in lsTaxlot)
                    {
                        //Transaction is being Created For AUECLocalDate 
                        taxlotToCalculate.AUECLocalDate = dateOfCalculatedData;

                        //Saving All the taxlot whose accrued interest to be calculated in a separate list 
                        //so that it can be sent to the function:-- CalculateAccruedInterest(List<PranaBasicMessage> Taxlots)                    
                        switch (taxlotToCalculate.AssetID)
                        {
                            case (int)AssetCategory.FixedIncome:
                            case (int)AssetCategory.ConvertibleBond:
                                if (taxlotToCalculate.ClosingStatus == ClosingStatus.Closed || taxlotToCalculate.ClosingStatus == ClosingStatus.PartiallyClosed)
                                {
                                    taxlotToCalculate.CumQty = taxlotToCalculate.ClosedQuantity;
                                }
                                if (dateOfCalculatedData > taxlotToCalculate.SettlementDate)
                                {
                                    lsTaxLotWithAccruedInterest.Add(taxlotToCalculate);
                                }
                                break;

                            case (int)AssetCategory.Future:

                                #region Changed by: Bharat raturi, 10 jul 2014,
                                if (!CashDataManager.GetCashPreferences()[taxlotToCalculate.Level1ID].IsCalculatePnL)
                                    continue;
                                #endregion
                                if (taxlotToCalculate.AUECModifiedDate.Date == dateOfCalculatedData.Date)
                                {
                                    markFromDb = CashDataManager.GetMarkPriceForDayAndSymbol(taxlotToCalculate.Symbol, dateOfCalculatedData, taxlotToCalculate.Level1ID);
                                    diffInMarkPrice = markFromDb - Convert.ToDecimal(taxlotToCalculate.AvgPrice);
                                }
                                else
                                    diffInMarkPrice = CashDataManager.calculateDiffInMarkPrice(taxlotToCalculate.Symbol, taxlotToCalculate.AUECLocalDate.Date, taxlotToCalculate.Level1ID);

                                taxlotToCalculate.M2MProfitLoss = diffInMarkPrice * Convert.ToDecimal((taxlotToCalculate.TaxLotQty) * taxlotToCalculate.ContractMultiplier * Calculations.GetSideMultilpier(taxlotToCalculate.OrderSideTagValue));
                                break;
                        }
                    }

                    if (lsTaxLotWithAccruedInterest.Count > 0)
                    {
                        lsTaxLotWithAccruedInterest = CalculateAccruedInterestforEachDay(lsTaxLotWithAccruedInterest, dateOfCalculatedData);
                    }

                    //removing closed and partially closed taxlots, before summation of accrued interest, PRA
                    lsTaxlot.RemoveAll(item => item.ClosingStatus == ClosingStatus.Closed);
                    lsTaxlot.RemoveAll(item => item.ClosingStatus == ClosingStatus.PartiallyClosed);
                    foreach (PranaBasicMessage msg in lsTaxLotWithAccruedInterest)
                    {
                        if (((msg as TaxLot).ClosingStatus == ClosingStatus.Closed) || ((msg as TaxLot).ClosingStatus == ClosingStatus.PartiallyClosed))
                        {
                            foreach (TaxLot taxlot in lsTaxlot)
                            {
                                if (taxlot.TaxLotID == (msg as TaxLot).TaxLotID && (GetCashPreferences()[taxlot.GetAccountID()].IsAccruedTillSettlement))
                                {
                                    taxlot.AccruedInterest += msg.AccruedInterest;
                                    //Setting ClosingSettlementDate in the case where taxlot having ClosingStatus is closed 
                                    //We will use it to create entry for the case where next coupon date comes before the closing settlement date
                                    //https://jira.nirvanasolutions.com:8443/browse/PRANA-35245
                                    if (taxlot.ClosingSettlementDate == DateTimeConstants.MinValue)
                                        taxlot.ClosingSettlementDate = (msg as TaxLot).ClosingSettlementDate;
                                    if (taxlot.ClosingTradeDate == DateTimeConstants.MinValue)
                                        taxlot.ClosingTradeDate = (msg as TaxLot).ClosingTradeDate;
                                }
                            }
                        }
                    }
                    // need to improve this code.

                    lsCalculatedTaxlots.AddRange(lsTaxlot);
                }
                LogMessage(userID, LoggingConstants.RESPONSE_SENT, LoggingConstants.CALCULATE_DAILY_CASH, string.Empty, string.Empty, string.Empty);
                return lsCalculatedTaxlots;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Updates the day end base cash by forex rate.
        /// </summary>
        /// <param name="dtForexRate">The dt forex rate.</param>
        public void UpdateDayEndBaseCashByForexRate(DataTable dtForexRate)
        {
            try
            {
                int companyBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                //To Fetch Data From PM_CompanyAccountCashCurrencyValue
                Dictionary<string, List<int>> dicDateWiseCurrencyID = new Dictionary<string, List<int>>();

                //To Update Fetched Data 
                Dictionary<string, Dictionary<int, double>> dicDateWiseCurrencyIDWithFxRate = new Dictionary<string, Dictionary<int, double>>();
                Dictionary<int, double> dicLocalCurrencyID_FxRate;
                string dateOfFxChange; int localCurrencyId; double fxRte; int fromCurrencyID; int toCurrencyID;
                bool isDataRequireUpdation = false;
                foreach (DataRow dr in dtForexRate.Rows)
                {
                    fromCurrencyID = Convert.ToInt32(dr["FromCurrencyID"]); toCurrencyID = Convert.ToInt32(dr["ToCurrencyID"]);
                    if (fromCurrencyID == companyBaseCurrencyID || toCurrencyID == companyBaseCurrencyID)
                    {
                        if (isDataRequireUpdation == false)
                            isDataRequireUpdation = true;

                        dateOfFxChange = Convert.ToDateTime(dr["Date"]).ToShortDateString();
                        fxRte = Convert.ToDouble(dr["ConversionFactor"]);
                        if (fromCurrencyID == companyBaseCurrencyID)
                        {
                            localCurrencyId = toCurrencyID;
                            fxRte = 1 / fxRte;
                        }
                        else
                            localCurrencyId = fromCurrencyID;

                        if (!dicDateWiseCurrencyID.ContainsKey(dateOfFxChange))
                            dicDateWiseCurrencyID.Add(dateOfFxChange, new List<int>(new int[] { localCurrencyId }));
                        else
                            dicDateWiseCurrencyID[dateOfFxChange].Add(localCurrencyId);

                        if (!dicDateWiseCurrencyIDWithFxRate.ContainsKey(dateOfFxChange))
                        {
                            dicLocalCurrencyID_FxRate = new Dictionary<int, double>();
                            dicLocalCurrencyID_FxRate.Add(localCurrencyId, fxRte);
                            dicDateWiseCurrencyIDWithFxRate.Add(dateOfFxChange, dicLocalCurrencyID_FxRate);
                        }
                        else
                        {
                            ///Added the check as the data was already available for localCurrencyId key, so updated in that case.
                            Dictionary<int, double> dictCurrencyFXRate = dicDateWiseCurrencyIDWithFxRate[dateOfFxChange];
                            if (dictCurrencyFXRate.ContainsKey(localCurrencyId))
                            {
                                dictCurrencyFXRate[localCurrencyId] = fxRte;
                            }
                            else
                            {
                                dictCurrencyFXRate.Add(localCurrencyId, fxRte);
                            }
                        }
                    }
                }
                if (isDataRequireUpdation)
                {
                    List<CompanyAccountCashCurrencyValue> lsDayEndToChange = CashDataManager.getDayEndCashForDatewiseCurrencyID(dicDateWiseCurrencyID);
                    if (lsDayEndToChange.Count > 0)
                    {
                        Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> dateWiseDayEndDictionary = new Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>>();
                        GenericBindingList<CompanyAccountCashCurrencyValue> newList;
                        foreach (CompanyAccountCashCurrencyValue dayEndData in lsDayEndToChange)
                        {
                            if (dicDateWiseCurrencyIDWithFxRate[dayEndData.Date.ToShortDateString()].ContainsKey(dayEndData.LocalCurrencyID))
                            {
                                dayEndData.CashValueBase = dayEndData.CashValueLocal * Convert.ToDecimal(dicDateWiseCurrencyIDWithFxRate[dayEndData.Date.ToShortDateString()][dayEndData.LocalCurrencyID]);
                            }
                            if (!dateWiseDayEndDictionary.ContainsKey(dayEndData.Date.ToShortDateString()))
                            {
                                newList = new GenericBindingList<CompanyAccountCashCurrencyValue>();
                                newList.Add(dayEndData);
                                dateWiseDayEndDictionary.Add(dayEndData.Date.ToShortDateString(), newList);
                            }
                            else
                                dateWiseDayEndDictionary[dayEndData.Date.ToShortDateString()].Add(dayEndData);
                        }
                        SaveDayEndData(dateWiseDayEndDictionary, null, int.MinValue);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Generates the identifier.
        /// </summary>
        /// <returns></returns>
        public string GenerateID()
        {
            string strID = String.Empty;
            try
            {
                strID = uIDGenerator.GenerateID();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return strID;
        }

        /// <summary>
        /// Calculates the and save balances.
        /// </summary>
        /// <param name="endDate">The end date.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="accountIDs">The account i ds.</param>
        public void CalculateAndSaveBalances(DateTime endDate, int userID, string accountIDs)
        {
            try
            {
                CashDataManager.CalculateAndSaveBalances(endDate, userID, accountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        public void AddSymbolLevelAccrualsData(Nullable<DateTime> startDate, DateTime endDate, int userID, string accountIDs, bool isManualReval)
        {
            try
            {
                CashDataManager.AddSymbolLevelAccrualsData(startDate, endDate, userID, accountIDs, isManualReval);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the account balances as on date.
        /// </summary>
        /// <param name="balDate">The bal date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public DataSet GetAccountBalancesAsOnDate(DateTime balDate, string accountIDs, int userId)
        {
            DataSet accBalances = null;
            try
            {
                LogMessage(userId, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_ACCOUNT_BALANCES, string.Empty, balDate.ToString(), accountIDs);
                accBalances = CashDataManager.GetAccountBalancesAsOnDate(balDate, accountIDs);
                LogMessage(userId, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_ACCOUNT_BALANCES, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return accBalances;
        }

        /// <summary>
        /// Gets the sub account transaction entries for date range.
        /// </summary>
        /// <param name="subAccountID">The sub account identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public DataSet GetSubAccountTransactionEntriesForDateRange(int subAccountID, DateTime fromDate, DateTime toDate)
        {
            DataSet transactionEntries = null;
            try
            {
                LogMessage(-1, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_SUB_ACCOUNT_DETAILS, fromDate.ToString(), toDate.ToString(), subAccountID.ToString());
                transactionEntries = CashDataManager.GetSubAccountTransactionEntriesForDateRange(subAccountID, fromDate, toDate);
                LogMessage(-1, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_SUB_ACCOUNT_DETAILS, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return transactionEntries;
        }

        /// <summary>
        /// Updates the cash accounts tables in database.
        /// </summary>
        /// <param name="updatedDataSet">The updated data set.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public int UpdateCashAccountsTablesInDB(DataSet updatedDataSet, int userID)
        {
            int rowsAffected = 0;
            try
            {
                CashDataManager.UpdateCashAccountsTablesInDB(updatedDataSet);
                CachedDataManager.GetInstance.RefreshAccountData();
                RefreshSubAccountAlert(userID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Updates the cash activity tables in database.
        /// </summary>
        /// <param name="updatedDataSet">The updated data set.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public int UpdateCashActivityTablesInDB(DataSet updatedDataSet, int userID)
        {
            int rowsAffected = 0;
            try
            {
                CashDataManager.UpdateCashActivityTablesInDB(updatedDataSet);
                CachedDataManager.GetInstance.RefreshAccountData();
                RefreshSubAccountAlert(userID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Gets the cash dividend from activities.
        /// </summary>
        /// <param name="auecDateString">The auec date string.</param>
        /// <returns></returns>
        public DataSet GetCashDividendFromActivities(string auecDateString)
        {
            try
            {
                return CashDataManager.GetCashDividendFromActivities(auecDateString);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Saves the manual cash dividend.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DataSet SaveManualCashDividend(DataSet ds)
        {
            DataSet updatedData = null;
            try
            {
                LogMessage(-1, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.SAVE_CASH_TRANSACTIONS, string.Empty, string.Empty, string.Empty);
                updatedData = CashDataManager.UpdateCashDividendsInDB(ds);
                _activityServices.GenerateCashActivity(updatedData, CashTransactionType.CashTransaction);
                LogMessage(-1, LoggingConstants.RESPONSE_SENT, LoggingConstants.SAVE_CASH_TRANSACTIONS, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return updatedData;
        }

        /// <summary>
        /// Gets the cash dividends.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="commaSeparatedAccountIds">The comma separated account ids.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public DataSet GetCashDividends(string symbol, string commaSeparatedAccountIds, string dateType, DateTime dateFrom, DateTime dateTo, int userId)
        {
            try
            {
                if (userId != -1)
                    LogMessage(userId, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_CASH_TRANSACTIONS, dateFrom.ToString(), dateTo.ToString(), commaSeparatedAccountIds);
                DataSet dividends = CashDataManager.GetCashDividends(symbol, commaSeparatedAccountIds, dateType, dateFrom, dateTo);
                if (userId != -1)
                    LogMessage(userId, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_CASH_TRANSACTIONS, string.Empty, string.Empty, string.Empty);
                return dividends;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the transactions for overriding.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public List<Transaction> GetTransactionsForOverriding(DateTime fromDate, DateTime toDate, String accountIDs)
        {
            List<Transaction> lsTransaction = new List<Transaction>();
            try
            {
                LogMessage(-1, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_OVERRIDING_JOURNALS, fromDate.ToString(), toDate.ToString(), accountIDs);
                #region Trading Activity Overriding Section

                //Getting all positions(open and closed)
                if (_activityServices != null)
                {
                    //userId passed as -1 to indicate server call
                    List<CashActivity> lsCashActivity = _activityServices.GetActivity(fromDate, toDate, accountIDs, -1, "Trade Date", false).Where(x => x.TransactionSource != CashTransactionType.ManualJournalEntry && x.TransactionSource != CashTransactionType.ImportedEditableData && x.TransactionSource != CashTransactionType.OpeningBalance).ToList();
                    lsTransaction = CashHelperClass.CreateJournalEntry(lsCashActivity).Where(t => t != null && t.TransactionEntries.Count > 1 && t.TransactionEntries[0].TaxLotState != ApplicationConstants.TaxLotState.Deleted).ToList();
                }
                #endregion
                CashRulesHelper.SetCashDataNameValues(lsTransaction);
                CashHelperClass.SetTransactionsTaxlotState(lsTransaction, ApplicationConstants.TaxLotState.Updated);
                LogMessage(-1, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_OVERRIDING_JOURNALS, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsTransaction;
        }

        /// <summary>
        /// Generates the journal entry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public List<Transaction> GenerateJournalEntry<T>(T data, int userId, bool isSymbolLevelAccruals)
        {
            try
            {
                List<Transaction> lsTransactions = CashHelperClass.CreateJournalEntry(data);
                SaveTransactions(lsTransactions, null, string.Empty, userId, isSymbolLevelAccruals);
                return CashRulesHelper.SetCashDataNameValues(lsTransactions);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the journal exceptions.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public List<Transaction> GetJournalExceptions(DateTime startDate, DateTime endDate, string accountIDs)
        {
            List<Transaction> lsExceptions = new List<Transaction>();
            try
            {
                LogMessage(-1, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_JOURNAL_EXCEPTIONS, startDate.ToString(), endDate.ToString(), accountIDs);
                List<CashActivity> lsCashActivity = CashDataManager.GetJournalExceptions<CashActivity>(startDate, endDate, accountIDs);
                lsCashActivity = CashRulesHelper.SetActivityDataNameValues(lsCashActivity);
                if (lsCashActivity.Count > 0)
                {
                    lsExceptions = CashHelperClass.CreateJournalEntry(lsCashActivity).Where(t => t != null && t.TransactionEntries.Count > 1 && t.TransactionEntries[0].TaxLotState != ApplicationConstants.TaxLotState.Deleted).ToList();
                    lsExceptions = CashRulesHelper.SetCashDataNameValues(lsExceptions);
                    CashHelperClass.SetTransactionsTaxlotState(lsExceptions, ApplicationConstants.TaxLotState.New);
                }
                LogMessage(-1, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_JOURNAL_EXCEPTIONS, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsExceptions;
        }

        /// <summary>
        /// This method will run revaluation batch till given date
        /// </summary>
        /// <param name="endDate"></param>
        /// <param name="userID">ID of the currently logged in user</param>
        /// <param name="accountIDs">String of comma separated fund IDs</param>
        public async Task<bool> RunRevaluationProcess(Nullable<DateTime> fromDate, DateTime toDate, int userID, string accountIDs, bool isManualRevaluation, bool _isGetAccountBalancesClicked)
        {
            bool result = false;
            string[] accountIDsArray = accountIDs.Split(',');
            try
            {
                if (isManualRevaluation)
                    if (_isGetAccountBalancesClicked)
                        LogMessage(userID, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.RUN_MANUAL_REVAL + " (" + LoggingConstants.GET_ACCOUNT_BALANCES + ")", fromDate.ToString(), toDate.ToString(), accountIDs);
                    else
                        LogMessage(userID, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.RUN_MANUAL_REVAL, fromDate.ToString(), toDate.ToString(), accountIDs);
                else
                    if (_isGetAccountBalancesClicked)
                    LogMessage(userID, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.RUN_REVAL + " (" + LoggingConstants.GET_ACCOUNT_BALANCES + ")", fromDate.ToString(), toDate.ToString(), accountIDs);
                else
                    LogMessage(userID, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.RUN_REVAL, fromDate.ToString(), toDate.ToString(), accountIDs);
                if (isManualRevaluation)
                {
                    System.Threading.Tasks.Task<int> rslt = System.Threading.Tasks.Task<int>.Factory.StartNew(() => SaveCashActionsInAudit(userID, accountIDs, fromDate, toDate, CashAuditManager.Com_Man_Reval));
                }
                else
                {
                    if (_isGetAccountBalancesClicked)
                    {
                        System.Threading.Tasks.Task<int> rslt = System.Threading.Tasks.Task<int>.Factory.StartNew(() => SaveCashActionsInAudit(userID, accountIDs, toDate, toDate, CashAuditManager.Com_RevalGetAccountBalances));
                    }
                    else
                    {
                        System.Threading.Tasks.Task<int> rslt = System.Threading.Tasks.Task<int>.Factory.StartNew(() => SaveCashActionsInAudit(userID, accountIDs, toDate, toDate, CashAuditManager.Com_RevalwithoutGetAccountBalances));
                    }
                }

                lock (_revalRunningLock)
                {
                    _revalRunningForFundIDs.AddRange(accountIDsArray);
                    _activityServices.AddToRevalRunningCache(accountIDsArray);
                    CashManagementRevalState._revalRunningForFundIDsMSMQ.AddRange(accountIDsArray);
                }
                result = await System.Threading.Tasks.Task.Run(() => { return RunRevaluationProcessAsync(fromDate, toDate, userID, accountIDs, isManualRevaluation); });
                if (result)
                {
                    CashDataManager.PublishRevaluationProgressMessage("Revaluation process completed", userID);
                }
                else
                {
                    CashDataManager.PublishRevaluationProgressMessage("Revaluation process failed", userID);
                }
            }
            catch (Exception ex)
            {
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-7360
                Logger.LoggerWrite("Revaluation process failed [Server Side]... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            finally
            {
                lock (_revalRunningLock)
                {
                    _revalRunningForFundIDs = _revalRunningForFundIDs.Except(accountIDsArray).ToList();
                    _activityServices.DeleteFromRevalRunningCache(accountIDsArray);
                    CashManagementRevalState._revalRunningForFundIDsMSMQ = CashManagementRevalState._revalRunningForFundIDsMSMQ.Except(accountIDsArray).ToList();
                }
                _activityServices.ProcessCashActivityQueue(accountIDs);
                if (isManualRevaluation)
                    if (_isGetAccountBalancesClicked)
                        LogMessage(userID, LoggingConstants.RESPONSE_SENT, LoggingConstants.RUN_MANUAL_REVAL + " (" + LoggingConstants.GET_ACCOUNT_BALANCES + ")", string.Empty, string.Empty, string.Empty);
                    else
                        LogMessage(userID, LoggingConstants.RESPONSE_SENT, LoggingConstants.RUN_MANUAL_REVAL, string.Empty, string.Empty, string.Empty);
                else
                    if (_isGetAccountBalancesClicked)
                    LogMessage(userID, LoggingConstants.RESPONSE_SENT, LoggingConstants.RUN_REVAL + " (" + LoggingConstants.GET_ACCOUNT_BALANCES + ")", string.Empty, string.Empty, string.Empty);
                else
                    LogMessage(userID, LoggingConstants.RESPONSE_SENT, LoggingConstants.RUN_REVAL, string.Empty, string.Empty, string.Empty);

            }
            return result;
        }

        /// <summary>
        /// Saves the cash actionsfor MTM.
        /// for Saving Daily Calculation data entry 
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public int SaveCashActionsforMTM(int userID)
        {
            SaveCashActionsInAudit(userID, string.Empty, null, null, CashAuditManager.Com_SaveMTMData);
            return 0;
        }

        /// <summary>
        /// Adds the transactions to update settlement fx rate audit entry.
        /// </summary>
        /// <param name="lsCashActivity">The ls cash activity.</param>
        /// <param name="action">The action.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="currentUserID">The current user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">The Data Set to add in audit dictionary is null</exception>
        public bool AddTransactionsToUpdateSettlementFxRateAuditEntry(List<CashActivity> lsCashActivity, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            try
            {
                if (lsCashActivity != null && lsCashActivity.Count > 0)
                {
                    List<TradeAuditEntry> _tradeAuditCollection_SettlementFxRate = new List<TradeAuditEntry>();
                    foreach (CashActivity trail in lsCashActivity)
                    {
                        TradeAuditEntry newEntry = new TradeAuditEntry();
                        newEntry.Action = action;
                        newEntry.AUECLocalDate = DateTime.Now;
                        newEntry.OriginalDate = trail.Date;
                        newEntry.Comment = comment;
                        newEntry.CompanyUserId = currentUserID;
                        newEntry.TaxLotID = int.MinValue.ToString();
                        newEntry.GroupID = int.MinValue.ToString();
                        newEntry.TaxLotClosingId = "";
                        newEntry.OrderSideTagValue = "";
                        newEntry.OriginalValue = "";
                        newEntry.Symbol = trail.Symbol;
                        newEntry.Level1ID = trail.AccountID;
                        newEntry.Source = TradeAuditActionType.ActionSource.Cash;
                        _tradeAuditCollection_SettlementFxRate.Add(newEntry);
                    }
                    AuditTrailDataManager.Instance.SaveAuditList(_tradeAuditCollection_SettlementFxRate);
                }
                else
                    throw new NullReferenceException("The Data Set to add in audit dictionary is null");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return true;
        }

        /// <summary>
        /// Saves the transactions.
        /// </summary>
        /// <param name="lsTransactions">The ls transactions.</param>
        /// <param name="lsTransactionEntries">The ls transaction entries.</param>
        /// <param name="source">The source.</param>
        public void SaveTransactions(List<Transaction> lsTransactions, Dictionary<string, TransactionEntry> lsTransactionEntries, string source, int userId, bool isSymbolLevelAccruals)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(source))
                    LogMessage(userId, LoggingConstants.REQUEST_RECEIVED, "Save " + source, string.Empty, string.Empty, string.Empty);
                if (lsTransactions.Count > 0)
                {
                    CashDataManager.SaveOrUpdateDataInDataBase(lsTransactions, lsTransactionEntries, isSymbolLevelAccruals);
                }
                if (!isSymbolLevelAccruals)
                {
                    lsTransactions = CashRulesHelper.SetCashDataNameValues(lsTransactions);
                    PublishCashManagementData(lsTransactions);
                }
                if (!string.IsNullOrWhiteSpace(source))
                    LogMessage(userId, LoggingConstants.RESPONSE_SENT, "Save " + source, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the day end data.
        /// </summary>
        /// <param name="dateWiseDayEndDictionary">The date wise day end dictionary.</param>
        /// <param name="lstDeletedData">The LST deleted data.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="accountIDs">The account i ds.</param>
        public async void SaveDayEndData(Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> dateWiseDayEndDictionary, List<CompanyAccountCashCurrencyValue> lstDeletedData, int userID, string accountIDs = null)
        {
            try
            {
                LogMessage(userID, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.SAVE_DAY_END_CASH, string.Empty, string.Empty, string.Empty);
                if (userID != int.MinValue)
                {
                    System.Threading.Tasks.Task<int> rslt = System.Threading.Tasks.Task<int>.Factory.StartNew(() => SaveCashActionsInAudit(userID, string.Empty, null, null, CashAuditManager.Com_DayEndCash));
                }
                List<CompanyAccountCashCurrencyValue> lsToPublish = await System.Threading.Tasks.Task.Run(() => CashDataManager.SaveCashCurrencyValueInDataBase(dateWiseDayEndDictionary, lstDeletedData, accountIDs));
                if (lsToPublish != null && lsToPublish.Count > 0)
                    PublishDayEndCash(lsToPublish);
                LogMessage(userID, LoggingConstants.RESPONSE_SENT, LoggingConstants.SAVE_DAY_END_CASH, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the CI data.
        /// </summary>
        /// <param name="dateWiseDayEndDictionary">The date wise CI dictionary.</param>
        /// <param name="lstDeletedData">The LST deleted data.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="accountIDs">The account i ds.</param>
        public async void SaveCIData(Dictionary<string, GenericBindingList<CollateralInterestValue>> dateWiseCIDictionary, List<CollateralInterestValue> lstDeletedData, int userID, string accountIDs = null)
        {
            try
            {
                LogMessage(userID, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.SAVE_CI, string.Empty, string.Empty, string.Empty);
                if (userID != int.MinValue)
                {
                    System.Threading.Tasks.Task<int> rslt = System.Threading.Tasks.Task<int>.Factory.StartNew(() => SaveCashActionsInAudit(userID, string.Empty, null, null, CashAuditManager.Com_DayEndCash));
                }
                List<CollateralInterestValue> lsToPublish = await System.Threading.Tasks.Task.Run(() => CashDataManager.SaveCIValueInDataBase(dateWiseCIDictionary, lstDeletedData, accountIDs));
                if (lsToPublish != null && lsToPublish.Count > 0)
                    PublishCI(lsToPublish);
                LogMessage(userID, LoggingConstants.RESPONSE_SENT, LoggingConstants.SAVE_CI, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Creates the journal entries.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="transactionSource">The transaction source.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public int CreateJournalEntries(DataSet dataSet, CashTransactionType transactionSource, int userId, bool IsMultileg)
        {
            try
            {
                List<Transaction> transactionList;
                if (!IsMultileg)
                    transactionList = CashHelperClass.GetTransactionsFromDS(dataSet, transactionSource, userId);
                else
                    transactionList = CashHelperClass.GetTransactionsFromDSForMultileg(dataSet, transactionSource, userId);
                SaveTransactions(transactionList, null, transactionSource.ToString(), userId, false);
                return transactionList.Count;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return 0;
        }

        /// <summary>
        /// Saves the daily credit limit values.
        /// </summary>
        /// <param name="dtDailyCreditLimitValue">The dt daily credit limit value.</param>
        /// <param name="isSavingFromImport">if set to <c>true</c> [is saving from import].</param>
        /// <returns></returns>
        public int SaveDailyCreditLimitValues(DataTable dtDailyCreditLimitValue, bool isSavingFromImport)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = CashDataManager.SaveDailyCreditLimitValues(dtDailyCreditLimitValue, isSavingFromImport);

                #region Daily Credit Limit Cache
                lock (_lockerDailyCreditLimit)
                {
                    if (_dailyCreditLimitValues == null || _dailyCreditLimitValues.Rows.Count == 0 || !isSavingFromImport)
                    {
                        _dailyCreditLimitValues = dtDailyCreditLimitValue;
                    }
                    else
                    {
                        for (int counter1 = 0; counter1 < dtDailyCreditLimitValue.Rows.Count; counter1++)
                        {
                            bool rowFound = false;
                            for (int counter2 = 0; counter2 < _dailyCreditLimitValues.Rows.Count; counter2++)
                            {
                                if (Convert.ToInt32(dtDailyCreditLimitValue.Rows[counter1]["FundID"]) == Convert.ToInt32(_dailyCreditLimitValues.Rows[counter2]["FundID"]))
                                {
                                    _dailyCreditLimitValues.Rows[counter2]["LongDebitBalance"] = Convert.ToDouble(dtDailyCreditLimitValue.Rows[counter1]["LongDebitBalance"]);
                                    _dailyCreditLimitValues.Rows[counter2]["ShortCreditBalance"] = Convert.ToDouble(dtDailyCreditLimitValue.Rows[counter1]["ShortCreditBalance"]);
                                    rowFound = true;
                                    break;
                                }
                            }
                            if (!rowFound)
                            {
                                DataRow drNew = _dailyCreditLimitValues.NewRow();
                                drNew["FundID"] = Convert.ToInt32(dtDailyCreditLimitValue.Rows[counter1]["FundID"]);
                                drNew["LongDebitLimit"] = Convert.ToDouble(dtDailyCreditLimitValue.Rows[counter1]["LongDebitLimit"]);
                                drNew["ShortCreditLimit"] = Convert.ToDouble(dtDailyCreditLimitValue.Rows[counter1]["ShortCreditLimit"]);
                                drNew["LongDebitBalance"] = Convert.ToDouble(dtDailyCreditLimitValue.Rows[counter1]["LongDebitBalance"]);
                                drNew["ShortCreditBalance"] = Convert.ToDouble(dtDailyCreditLimitValue.Rows[counter1]["ShortCreditBalance"]);

                                _dailyCreditLimitValues.Rows.Add(drNew);
                            }
                        }
                    }
                    _dailyCreditLimitValues.AcceptChanges();
                    #endregion
                    if (rowsAffected != 0)
                    {
                        PublishDailyCreditLimit(_dailyCreditLimitValues);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Gets the daily credit limit values.
        /// </summary>
        /// <returns></returns>
        public DataTable GetDailyCreditLimitValues()
        {
            try
            {
                lock (_lockerDailyCreditLimit)
                {
                    if (_dailyCreditLimitValues == null)
                    {
                        _dailyCreditLimitValues = CashDataManager.GetDailyCreditLimitValues();
                    }
                }
                return _dailyCreditLimitValues;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Updates the last revaluation calculated date to given date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="isUpdated">if set to <c>true</c> [is updated].</param>
        public void UpdateLastRevaluationCalculatedDateToGivenDate(DateTime date, string accountIDs, bool isUpdated)
        {
            try
            {
                CashDataManager.UpdateLastRevaluationCalculatedDateToGivenDate(DateTime.MinValue, date, accountIDs, isUpdated, false, false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the last calculate revalution detail.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, RevaluationUpdateDetail> GetLastCalcRevalutionDetail()
        {
            try
            {
                return CachedDataManager.GetLastRevaluationCalculationDate();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the last calculate revalution detail.
        /// </summary>
        /// <returns></returns>
        public string GetInvalidFundsForSymbolLevel(string fundIds, Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            try
            {
                return CachedDataManager.GetInstance.GetInvalidFundsForSymbolLevel(fundIds, startDate, endDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the last calculate balance details.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, BalanceUpdateDetail> GetLastCalcBalanceDetails()
        {
            try
            {
                return CashDataManager.GetLastCalculatedBalanceDetails();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the transactions by transaction identifier.
        /// </summary>
        /// <param name="transactionID">The transaction identifier.</param>
        /// <returns></returns>
        public List<Transaction> GetTransactionsByTransactionID(string transactionID)
        {
            List<Transaction> lsTransactions = new List<Transaction>();
            try
            {
                List<TransactionEntry> lsTransactionsEntries = CashDataManager.GetTransactionsByTransactionID<TransactionEntry>(transactionID);
                lsTransactions = CashRulesHelper.getTransactionsFromTransactionEntries(lsTransactionsEntries);
                lsTransactions = CashRulesHelper.SetCashDataNameValues(lsTransactions);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsTransactions;
        }

        /// <summary>
        /// Gets the transactions by Corporate Action ID.
        /// </summary>
        /// <param name="transactionID">The transaction identifier.</param>
        /// <returns></returns>
        public List<Transaction> GetTransactionsByCAID(string CAID)
        {
            List<Transaction> lsTransactions = new List<Transaction>();
            try
            {
                List<TransactionEntry> lsTransactionsEntries = CashDataManager.GetTransactionsBytaxlotID<TransactionEntry>(CAID);
                lsTransactions = CashRulesHelper.getTransactionsFromTransactionEntries(lsTransactionsEntries);
                lsTransactions = CashRulesHelper.SetCashDataNameValues(lsTransactions);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsTransactions;
        }

        /// <summary>
        /// The reval running for fund i ds
        /// </summary>
        public static List<string> _revalRunningForFundIDs = new List<string>();

        /// <summary>
        /// Gets the is revaluation in progress.
        /// </summary>
        /// <param name="accountIds">The account ids.</param>
        /// <returns></returns>
        public bool GetIsRevaluationInProgress(string accountIds)
        {
            bool isRevaluationInProgress = false;
            try
            {
                string[] accountIDsArray = accountIds.Split(',');
                lock (_revalRunningLock)
                {
                    if ((_revalRunningForFundIDs.Intersect(accountIDsArray.ToList()).ToList().Count) > 0)
                    {
                        isRevaluationInProgress = true;
                    }
                    else
                    {
                        isRevaluationInProgress = false;
                    }
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
            return isRevaluationInProgress;
        }

        /// <summary>
        /// Get the Opertaion Mode of each account
        /// </summary>
        /// <returns>Dictionary with Operation Mode as key and List of Funds as value</returns>
        public Dictionary<CashManagementEnums.OperationMode, List<int>> GetAccountIdByRevaluationOperationMode()
        {
            Dictionary<CashManagementEnums.OperationMode, List<int>> dictAccountIdByRevaluationOperationMode = new Dictionary<CashManagementEnums.OperationMode, List<int>>();
            try
            {
                DataTable dtAccountIdByRevaluationOperationMode = CashDataManager.GetAccountIdByRevaluationOperationMode();

                if (dtAccountIdByRevaluationOperationMode != null && dtAccountIdByRevaluationOperationMode.Rows.Count > 0)
                {
                    foreach (DataRow row in dtAccountIdByRevaluationOperationMode.Rows)
                    {
                        int fundId = int.Parse(row["FundID"].ToString());
                        int operationModeInt = int.Parse(row["OperationMode"].ToString());

                        CashManagementEnums.OperationMode operationMode = (CashManagementEnums.OperationMode)operationModeInt;
                        if (dictAccountIdByRevaluationOperationMode.ContainsKey(operationMode))
                        {
                            dictAccountIdByRevaluationOperationMode[operationMode].Add(fundId);
                        }
                        else
                        {
                            List<int> lstFundId = new List<int>();
                            lstFundId.Add(fundId);
                            dictAccountIdByRevaluationOperationMode.Add(operationMode, lstFundId);
                        }
                    }
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
            return dictAccountIdByRevaluationOperationMode;
        }

        #endregion

        #region IPublishing Members
        /// <summary>
        /// The publish lock
        /// </summary>
        private static readonly object _publishLock = new object();

        /// <summary>
        /// Publishes the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="topicName">Name of the topic.</param>
        public void Publish(MessageData e, string topicName)
        {
            System.Object[] dataList = null;
            switch (topicName)
            {
                case Topics.Topic_MarkPrice:
                    dataList = (System.Object[])e.EventData;
                    List<string> listMark = new List<string>(Array.ConvertAll<object, string>(dataList, new Converter<object, string>(Convert.ToString)));
                    DataTable dtMark = DataTableToListConverter.GetDataTableFromList(listMark);
                    Dictionary<int, DateTime> accountsTobeUpdated = new Dictionary<int, DateTime>();
                    Dictionary<int, RevaluationUpdateDetail> revalDates = GetLastCalcRevalutionDetail();
                    if (dtMark == null)
                    {
                        return;
                    }
                    lock (_lockerObj)
                    {
                        foreach (DataRow dr in dtMark.Rows)
                        {
                            DateTime date = Convert.ToDateTime(dr["Date"]);
                            int drAccountId = Convert.ToInt32(dr["AccountId"]);

                            foreach (int accountID in GetCashPreferences().Keys)
                            {
                                if ((DateTime.Compare(_dictCashPreferences[accountID].CashMgmtStartDate, date) <= 0 && DateTime.Compare(date, revalDates[accountID].LastRevaluationDate) <= 0) && (drAccountId.Equals(accountID) || drAccountId.Equals(0)))
                                {
                                    revalDates[accountID].LastRevaluationDate = date;
                                    if (!accountsTobeUpdated.ContainsKey(accountID))
                                    {
                                        accountsTobeUpdated.Add(accountID, date);
                                    }
                                    else
                                    {
                                        accountsTobeUpdated[accountID] = date;
                                    }
                                }
                            }
                        }
                        if (accountsTobeUpdated != null && accountsTobeUpdated.Count > 0)
                        {
                            foreach (KeyValuePair<int, DateTime> kvp in accountsTobeUpdated)
                            {
                                CashDataManager.UpdateLastRevaluationCalculatedDateToGivenDate(DateTime.MinValue, kvp.Value, kvp.Key.ToString(), false, false, false);
                            }
                        }
                    }
                    break;

                case Topics.Topic_ForexRate:
                    dataList = (System.Object[])e.EventData;
                    Dictionary<int, DateTime> accountsTobeUpdate = new Dictionary<int, DateTime>();
                    List<string> listForexRate = new List<string>(Array.ConvertAll<object, string>(dataList, new Converter<object, string>(Convert.ToString)));
                    DataTable dtForexRate = DataTableToListConverter.GetDataTableFromList(listForexRate);
                    Dictionary<int, RevaluationUpdateDetail> revalDate = GetLastCalcRevalutionDetail();

                    if (dtForexRate == null)
                    {
                        return;
                    }
                    lock (_lockerObj)
                    {
                        foreach (DataRow dr in dtForexRate.Rows)
                        {
                            DateTime date = Convert.ToDateTime(dr["Date"]);
                            int drAccountId = Convert.ToInt32(dr["AccountId"]);
                            foreach (int accountID in GetCashPreferences().Keys)
                            {
                                if ((DateTime.Compare(_dictCashPreferences[accountID].CashMgmtStartDate, date) <= 0 && DateTime.Compare(date, revalDate[accountID].LastRevaluationDate) <= 0) && (drAccountId.Equals(accountID) || drAccountId.Equals(0)))
                                {
                                    revalDate[accountID].LastRevaluationDate = date;
                                    if (!accountsTobeUpdate.ContainsKey(accountID))
                                    {
                                        accountsTobeUpdate.Add(accountID, date);
                                    }
                                    else
                                    {
                                        accountsTobeUpdate[accountID] = date;
                                    }
                                }
                            }
                        }
                        if (accountsTobeUpdate != null && accountsTobeUpdate.Count > 0)
                        {
                            foreach (KeyValuePair<int, DateTime> kvp in accountsTobeUpdate)
                            {
                                CashDataManager.UpdateLastRevaluationCalculatedDateToGivenDate(DateTime.MinValue, kvp.Value, kvp.Key.ToString(), false, false, false);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Publishes the cash management data.
        /// </summary>
        /// <param name="cashDataList">The cash data list.</param>
        public void PublishCashManagementData(List<Transaction> cashDataList)
        {
            try
            {
                if (cashDataList != null && cashDataList.Count > 0)
                {
                    var cashData = cashDataList.Where(tr => tr.TransactionEntries.Count > 0).ToList();
                    if (cashData.Count > 0)
                    {
                        if (cashData[0].TransactionEntries[0].TransactionSource.ToString().Equals(CashTransactionType.Revaluation.ToString()))
                        {
                            List<object> data = new List<object>();
                            var publishList = cashData.Where(tran => GetCashPreferences().ContainsKey(tran.TransactionEntries[0].AccountID) && GetCashPreferences()[tran.TransactionEntries[0].AccountID] != null && (GetCashPreferences()[tran.TransactionEntries[0].AccountID].IsPublishRevaluationData)).ToList();
                            if (publishList != null)
                            {
                                data.AddRange(publishList);
                            }
                            if (publishList == null || publishList.Count != cashData.Count)
                            {
                                data.Add("Revaluation is done but some or all of the data is not published, click on Get Transactions to fetch data.");
                            }
                            MessageData e = new MessageData();
                            e.EventData = data;
                            e.TopicName = Topics.Topic_RevaluationJournal;
                            CentralizePublish(e);
                        }
                        else
                        {
                            MessageData e = new MessageData();
                            e.EventData = cashData;
                            e.TopicName = Topics.Topic_CashData;
                            CentralizePublish(e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Centralizes the publish.
        /// </summary>
        /// <param name="msgData">The MSG data.</param>
        private static void CentralizePublish(MessageData msgData)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            _proxyPublishing.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            if (rethrow)
                                throw;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Publishes the type of the activity.
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static void PublishActivityType(DataTable dt)
        {
            MessageData e = new MessageData();
            e.EventData = Prana.Utilities.MiscUtilities.DataTableToListConverter.GetListFromDataTable(dt);
            e.TopicName = Topics.Topic_ActivityType;
            CentralizePublish(e);

        }

        /// <summary>
        /// Publishes the activity type mapping.
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static void PublishActivityTypeMapping(DataTable dt)
        {
            MessageData e = new MessageData();
            e.EventData = Prana.Utilities.MiscUtilities.DataTableToListConverter.GetListFromDataTable(dt);
            e.TopicName = Topics.Topic_ActivityJournalMapping;
            CentralizePublish(e);
        }

        /// <summary>
        /// Publishes the cash activity.
        /// </summary>
        /// <param name="lsCashActivity">The ls cash activity.</param>
        public static void PublishCashActivity(List<CashActivity> lsCashActivity)
        {
            MessageData e = new MessageData();
            e.EventData = lsCashActivity;
            e.TopicName = Topics.Topic_ManualJournalActivity;
            CentralizePublish(e);
        }

        /// <summary>
        /// Publishes the day end cash.
        /// </summary>
        /// <param name="dayEndCash">The day end cash.</param>
        public void PublishDayEndCash(List<CompanyAccountCashCurrencyValue> dayEndCash)
        {
            MessageData e = new MessageData();
            e.EventData = dayEndCash;
            e.TopicName = Topics.Topic_DayEndCash;
            CentralizePublish(e);
        }

        /// <summary>
        /// Publishes the CI.
        /// </summary>
        /// <param name="dayEndCash">The day end cash.</param>
        public void PublishCI(List<CollateralInterestValue> CI)
        {
            MessageData e = new MessageData();
            e.EventData = CI;
            e.TopicName = Topics.Topic_CI;
            CentralizePublish(e);
        }

        /// <summary>
        /// Publishes the daily credit limit.
        /// </summary>
        /// <param name="dtDailyCreditLimit">The dt daily credit limit.</param>
        public void PublishDailyCreditLimit(DataTable dtDailyCreditLimit)
        {
            MessageData e = new MessageData();
            e.EventData = DataTableToListConverter.GetListFromDataTable(dtDailyCreditLimit);
            e.TopicName = Topics.Topic_DailyCreditLimit;
            CentralizePublish(e);
        }

        /// <summary>
        /// 'Last Modified Date' should be auto publish
        /// </summary>
        /// <param name="?"></param>        
        public static void PublishRevaluationDate(DateTime date, string accountIds)
        {
            List<object> eventData = new List<object>();
            eventData.Add(date);
            eventData.Add(accountIds);
            MessageData e = new MessageData();
            e.EventData = eventData;
            e.TopicName = Topics.Topic_RevaluationDate;
            CentralizePublish(e);
        }

        /// <summary>
        /// The proxy publishing
        /// </summary>
        static ProxyBase<IPublishing> _proxyPublishing;

        /// <summary>
        /// The proxy pricing
        /// </summary>
        DuplexProxyBase<ISubscription> _proxyPricing;

        /// <summary>
        /// Creates the publishing proxy.
        /// </summary>
        private void CreatePublishingProxy()
        {
            _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
        }

        /// <summary>
        /// Creates the subscription services proxy pricing.
        /// </summary>
        /// <param name="flag">if set to <c>true</c> [flag].</param>
        public void CreateSubscriptionServicesProxyPricing(bool flag)
        {
            try
            {
                if (_proxyPricing == null && flag)
                {
                    _proxyPricing = new DuplexProxyBase<ISubscription>("PricingSubscriptionEndpointAddress", this);
                    _proxyPricing.Subscribe(Topics.Topic_MarkPrice, null);
                    _proxyPricing.Subscribe(Topics.Topic_ForexRate, null);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the name of the receiver unique.
        /// </summary>
        /// <returns></returns>
        public string getReceiverUniqueName()
        {
            return "CashManagementServices";
        }

        /// <summary>
        /// Calculates the accrued interest.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        /// <returns></returns>
        public List<PranaBasicMessage> CalculateAccruedInterest(List<PranaBasicMessage> taxlots)
        {
            try
            {
                foreach (PranaBasicMessage obj in taxlots)
                {
                    obj.AccruedInterest = _fixedIncomeAdapter.CalculateAccruedInterest(obj);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return taxlots;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (_proxyPricing != null)
                    {
                        _proxyPricing.InnerChannel.UnSubscribe(Topics.Topic_MarkPrice);
                        _proxyPricing.InnerChannel.UnSubscribe(Topics.Topic_ForexRate);
                        _proxyPricing.Dispose();
                    }
                    if (_dailyCreditLimitValues != null)
                        _dailyCreditLimitValues.Dispose();
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
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="action">The action.</param>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        private void LogMessage(int userId, string action, string tabName, string fromDate, string toDate, String accountIDs)
        {
            try
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsGeneralLedgerLoggingRequired"]))
                {
                    string user = (userId == -1) ? string.Empty : CachedDataManager.GetInstance.GetUserText(userId);
                    LogExtensions.LogMessage(PranaModules.GENERAL_LEDGER_MODULE, user, action, tabName, fromDate, toDate, accountIDs);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the cash impact asynchronous.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        private List<Transaction> GetCashImpactAsync(DateTime fromDate, DateTime toDate, String accountIDs)
        {
            List<Transaction> lsTransactions = new List<Transaction>();
            try
            {
                List<TransactionEntry> data = CashDataManager.GetTransactionEntriesForGivenDate<TransactionEntry>(accountIDs, fromDate, toDate);
                lsTransactions = CashRulesHelper.getTransactionsFromTransactionEntries(data);
                if (lsTransactions != null && lsTransactions.Count > 0)
                    lsTransactions = CashRulesHelper.SetCashDataNameValues(lsTransactions);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsTransactions;
        }

        /// <summary>
        /// Gets the day end cash asynchronous.
        /// </summary>
        /// <param name="givenDate">The given date.</param>
        /// <param name="dtEndDate">The dt end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        private List<CompanyAccountCashCurrencyValue> GetDayEndCashAsync(DateTime givenDate, DateTime dtEndDate, string accountIDs)
        {
            List<CompanyAccountCashCurrencyValue> lsDayEndCash = new List<CompanyAccountCashCurrencyValue>();
            try
            {
                lsDayEndCash = CashDataManager.GetDayEndCashFromDb<CompanyAccountCashCurrencyValue>(accountIDs, givenDate, dtEndDate);
                foreach (CompanyAccountCashCurrencyValue data in lsDayEndCash)
                {
                    data.AccountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(data.AccountID);
                    data.LocalCurrencyName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(data.LocalCurrencyID);
                    data.BaseCurrencyName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(data.BaseCurrencyID);
                    data.Date = data.Date.Date;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsDayEndCash;
        }

        /// <summary>
        /// Calculates the accrued interestfor each day.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        /// <param name="calculationDate">The calculation date.</param>
        /// <returns></returns>
        private List<PranaBasicMessage> CalculateAccruedInterestforEachDay(List<PranaBasicMessage> taxlots, DateTime calculationDate)
        {
            try
            {
                foreach (PranaBasicMessage obj in taxlots)
                {
                    if (CashDataManager.GetCashPreferences().ContainsKey((obj as TaxLot).Level1ID) && CashDataManager.GetCashPreferences()[(obj as TaxLot).Level1ID].IsCalculateBondAccurals)
                    {
                        DateTime originalSettlementDate = obj.SettlementDate;

                        obj.SettlementDate = calculationDate;
                        DateTime settlementDate = obj.SettlementDate;
                        DateTime previousDate = settlementDate.AddDays(-1);

                        DateTime lastCouponPayDate = _fixedIncomeAdapter.GetLastCouponPayDate(obj, settlementDate);
                        DateTime nextCouponPayDate = _fixedIncomeAdapter.GetNextCouponPayDate(obj, settlementDate);
                        if (lastCouponPayDate.Date.Equals(previousDate.Date))
                        {
                            obj.AccruedInterest = _fixedIncomeAdapter.CalculateAccruedInterest(obj);
                        }
                        else
                        {
                            if ((obj as TaxLot).ClosingStatus == ClosingStatus.Open || !(GetCashPreferences()[(obj as TaxLot).GetAccountID()].IsAccruedTillSettlement))
                            {
                                obj.SettlementDate = previousDate;
                                double cumulativeAI_PrviousDate = _fixedIncomeAdapter.CalculateAccruedInterest(obj);

                                obj.SettlementDate = settlementDate;
                                double cumulativeAI_CurrentDate = _fixedIncomeAdapter.CalculateAccruedInterest(obj);

                                obj.AccruedInterest = (cumulativeAI_CurrentDate - cumulativeAI_PrviousDate);
                            }
                            else
                            {
                                DateTime closingTradeDate = (obj as TaxLot).ClosingTradeDate;
                                obj.SettlementDate = closingTradeDate;
                                double cumulativeAI_PrviousDate = _fixedIncomeAdapter.CalculateAccruedInterest(obj);

                                DateTime closingSettlementDate = (obj as TaxLot).ClosingSettlementDate; ;
                                obj.SettlementDate = closingSettlementDate;
                                double cumulativeAI_CurrentDate = 0;
                                if (nextCouponPayDate > closingTradeDate && nextCouponPayDate < closingSettlementDate)
                                {
                                    obj.SettlementDate = nextCouponPayDate;
                                    double cumulativeAI_OnCouponDate = _fixedIncomeAdapter.CalculateAccruedInterest(obj);
                                    obj.SettlementDate = (obj as TaxLot).ClosingSettlementDate;
                                    cumulativeAI_CurrentDate = cumulativeAI_OnCouponDate + _fixedIncomeAdapter.CalculateAccruedInterest(obj);
                                }
                                else
                                    cumulativeAI_CurrentDate = _fixedIncomeAdapter.CalculateAccruedInterest(obj);

                                obj.AccruedInterest = (cumulativeAI_CurrentDate - cumulativeAI_PrviousDate);
                            }
                        }
                        obj.SettlementDate = originalSettlementDate;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return taxlots;
        }

        /// <summary>
        /// Gets the open positions for daily calculation asynchronous.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="AccountIDs">The account i ds.</param>
        /// <returns></returns>
        private Dictionary<string, List<TaxLot>> GetOpenPositionsForDailyCalculationAsync(DateTime fromDate, DateTime toDate, String AccountIDs)
        {
            int[] accounts = Array.ConvertAll(AccountIDs.Split(','), int.Parse);
            StringBuilder customconditions = new StringBuilder();
            Dictionary<string, List<TaxLot>> dicPositionsForDailyCalculation = new Dictionary<string, List<TaxLot>>();
            try
            {
                string assetIdsForDailyValuation = CashHelperClass.AssetIDsForDailyCalculation(accounts, out customconditions);
                if (_pranaPositionServices != null && !String.IsNullOrEmpty(assetIdsForDailyValuation) && assetIdsForDailyValuation.Length > 0)
                {
                    dicPositionsForDailyCalculation = _pranaPositionServices.GetOpenPositionsAndTransactions(fromDate, toDate, assetIdsForDailyValuation, AccountIDs, customconditions.ToString());
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return dicPositionsForDailyCalculation;
        }

        /// <summary>
        /// Refreshes the sub account alert.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        private void RefreshSubAccountAlert(int userID)
        {
            try
            {
                MessageData e = new MessageData();
                List<int> data = new List<int>();
                data.Add(userID);
                e.EventData = data;
                e.TopicName = Topics.Topic_SubAccounts;
                CentralizePublish(e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the cash actions in audit.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        private int SaveCashActionsInAudit(int userID, string accountIDs, Nullable<DateTime> fromDate, Nullable<DateTime> toDate, string comment)
        {
            try
            {
                CashAuditManager.SaveCashActionsInAudit(userID, accountIDs, fromDate, toDate, comment);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return 0;
        }

        /// <summary>
        /// Runs the revaluation process asynchronous.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="isManualReval">if set to <c>true</c> [is manual reval].</param>
        /// <returns></returns>
        private bool RunRevaluationProcessAsync(Nullable<DateTime> fromDate, DateTime toDate, int userID, string accountIDs, bool isManualReval)
        {
            try
            {
                Logger.LoggerWrite("Run Revaluation Status: RunRevaluationProcessAsync started, Accounts: " + accountIDs + ", fromDate: " + fromDate + ", toDate: " + toDate + ", isManualReval:" + isManualReval + "..... Time: " + DateTime.Now, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                #region generate activities to update missing settlement fx rate
                List<TaxLot> lstTaxlots = _pranaPositionServices.GetTransactionsToUpdateSettlementFxRate(toDate, accountIDs);
                if (lstTaxlots != null && lstTaxlots.Count > 0)
                {
                    List<CashActivity> lsCashActivity = _activityServices.GenerateCashActivity(lstTaxlots, CashTransactionType.Trading, CashTransactionType.Revaluation);
                    AddTransactionsToUpdateSettlementFxRateAuditEntry(lsCashActivity, TradeAuditActionType.ActionType.MissingSettlementFxRate_Activity, "Activities to update missing settlement fx rate", userID);

                }
                #endregion

                DataSet dsAssetRevaluation = CashDataManager.GetAssetRevaluationData(fromDate, toDate, accountIDs, userID, isManualReval);

                Logger.LoggerWrite("Run Revaluation Status: GetAssetRevaluationData Completed.... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                if (dsAssetRevaluation != null && dsAssetRevaluation.Tables.Count > 0 && dsAssetRevaluation.Tables[0].Rows.Count > 0)
                {
                    //PRANA-9776 UserId
                    _activityServices.CreateActivity(dsAssetRevaluation, CashTransactionType.Revaluation, userID, false);
                }

                Logger.LoggerWrite("Run Revaluation Status: AddSymbolLevelAccrualsData Completed.... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                #region Add Symbol level accruals data in accrual tables.
                string fundIds = string.Empty;
                //Funds who has SymbolWiseAccrual date less than the reval end date are the eligible Funds For SymbolWiseAccrual 
                fundIds = CashDataManager.GetEligibleFundsForSymbolWiseAccrual(accountIDs, toDate);
                fundIds = fundIds.Trim(',');
                if (!string.IsNullOrWhiteSpace(fundIds))
                    AddSymbolLevelAccrualsData(fromDate, toDate, userID, fundIds, isManualReval);
                #endregion

                CalculateAndSaveBalances(toDate, userID, accountIDs);
                Logger.LoggerWrite("Run Revaluation Status: CalculateAndSaveBalances Completed.... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                //Cash and accruals revaluation is done after calculating sub-account balances for the given date range
                DataSet dsCashAndAccruals = CashDataManager.GetCashAccrualsRevaluationData(fromDate, toDate, accountIDs, userID, isManualReval);

                Logger.LoggerWrite("Run Revaluation Status: GetCashAccrualsRevaluationData Completed.... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                if (dsCashAndAccruals != null && dsCashAndAccruals.Tables.Count > 0 && dsCashAndAccruals.Tables[0].Rows.Count > 0)
                {
                    //PRANA-9776 UserId
                    _activityServices.CreateActivity(dsCashAndAccruals, CashTransactionType.Revaluation, userID, false);
                    if (dsCashAndAccruals.Tables.Count > 1 && dsCashAndAccruals.Tables[1].Rows.Count > 0)
                        _activityServices.CreateActivity(dsCashAndAccruals, CashTransactionType.Revaluation, userID, true);
                }
                CalculateAndSaveBalances(toDate, userID, accountIDs);
                Logger.LoggerWrite("Run Revaluation Status: CalculateAndSaveBalances2 Completed.... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                if (isManualReval)
                {
                    //Incase of Manual Revaluation fromDate == null case will never occur as we always pass fromDate for that.
                    if (fromDate == null)
                    {
                        CashDataManager.UpdateLastRevaluationCalculatedDateToGivenDate(DateTime.MinValue, DateTime.MinValue, accountIDs, true, true, isManualReval);
                    }
                    else
                    {
                        CashDataManager.UpdateLastRevaluationCalculatedDateToGivenDate(fromDate.Value, toDate, accountIDs, true, true, isManualReval);
                    }
                }
                else
                {
                    CashDataManager.UpdateLastRevaluationCalculatedDateToGivenDate(DateTime.MinValue, toDate, accountIDs, true, true, isManualReval);
                }
                Logger.LoggerWrite("Run Revaluation Status: RunRevaluationProcessAsync Completed... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                return true;
            }
            catch (Exception ex)
            {
                string[] accountIDsArray = accountIDs.Split(',');
                lock (_revalRunningLock)
                {
                    _revalRunningForFundIDs = _revalRunningForFundIDs.Except(accountIDsArray).ToList();
                    _activityServices.DeleteFromRevalRunningCache(accountIDsArray);
                    CashManagementRevalState._revalRunningForFundIDsMSMQ = CashManagementRevalState._revalRunningForFundIDsMSMQ.Except(accountIDsArray).ToList();
                }
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Set Acticity Services
        /// </summary>
        /// <param name="activityServices"></param>
        public void SetActicityServices(IActivityServices activityServices)
        {
            _activityServices = activityServices;
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion
    }
}
