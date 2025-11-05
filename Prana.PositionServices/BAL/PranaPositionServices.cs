using Prana.BusinessLogic.Symbol;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Blotter;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Prana.PositionServices
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class PranaPositionServices : IPranaPositionServices
    {
        #region IPranaPositionServices Members
        TaxlotGroupingManager _taxlotGroupingManager = new TaxlotGroupingManager();
        static ProxyBase<IPublishing> _proxyPublishing = null;
        static readonly object _locker = new object();

        private static void CreatePublishingProxy()
        {
            try
            {
                lock (_locker)
                {
                    if (_proxyPublishing == null)
                    {
                        _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy", ex), LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public List<TaxLot> GetOpenPositions(DateTime date, string commaSapratedAccountIds = "")
        {
            List<TaxLot> _lsTaxlots = new List<TaxLot>();
            try
            {
                _lsTaxlots = PranaPositionDataManager.GetOpenPositions(date, false, commaSapratedAccountIds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _lsTaxlots;
        }

        public List<TaxLot> GetOpenPositionsFromAnyDB(DateTime date, string connectionString, List<string> listGrouping)
        {
            List<TaxLot> _groupedTaxlots = new List<TaxLot>();
            try
            {
                List<TaxLot> _lsTaxlots = PranaPositionDataManager.GetOpenPositions(date, connectionString);
                _groupedTaxlots = CreateGroupedData(listGrouping, _lsTaxlots);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _groupedTaxlots;
        }

        public async Task<List<TaxLot>> GetGroupedPositions(List<string> listGrouping, DateTime date, bool isUnallocatedTradesIncludedInPositions, string commaSapratedAccountIds)
        {
            List<TaxLot> coll = new List<TaxLot>();
            try
            {
                coll = await System.Threading.Tasks.Task.Run(() => { return GetGroupedPositionsAsync(listGrouping, date, isUnallocatedTradesIncludedInPositions, commaSapratedAccountIds); });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return coll;
        }

        private List<TaxLot> GetGroupedPositionsAsync(List<string> listGrouping, DateTime date, bool isUnallocatedTradesIncludedInPositions, string commaSapratedAccountIds)
        {
            List<TaxLot> taxlots = GetOpenPositions(date, commaSapratedAccountIds);
            if (isUnallocatedTradesIncludedInPositions)
            {
                //We are using all AUEC local date because in GetOpenPosition method it is hardcode.
                List<TaxLot> unallocatedTrades = GetUnallocatedTradesForDateString(TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(date));
                if (unallocatedTrades != null && unallocatedTrades.Count > 0)
                    taxlots.AddRange(unallocatedTrades);
            }
            List<TaxLot> coll = CreateGroupedData(listGrouping, taxlots);
            return coll;
        }

        public async Task<GenericPositionData> GetGroupedPositionsAndTransactions(List<string> listGrouping, DateTime date, string commaSapratedAssetIDs, string commaSapratedAccountIds, string symbols, string customConditions, bool isSameDateInAllAUEC, bool isTransactionsIncludedInPositions, bool isAccrualsNeeded, bool isUnallocatedTradesIncludedInPositions, bool isBetaNeeded, bool isFxRateNeeded, bool isUnallocatedTradesPermitted)
        {
            GenericPositionData genericpositionData = new GenericPositionData();
            try
            {
                genericpositionData = await System.Threading.Tasks.Task.Run(() => { return GetGroupedPositionsAndTransactionsAsync(listGrouping, date, commaSapratedAssetIDs, commaSapratedAccountIds, symbols, customConditions, isSameDateInAllAUEC, isTransactionsIncludedInPositions, isAccrualsNeeded, isUnallocatedTradesIncludedInPositions, isBetaNeeded, isFxRateNeeded, isUnallocatedTradesPermitted); });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return genericpositionData;
        }

        private GenericPositionData GetGroupedPositionsAndTransactionsAsync(List<string> listGrouping, DateTime date, string commaSapratedAssetIDs, string commaSapratedAccountIds, string symbols, string customConditions, bool isSameDateInAllAUEC, bool isTransactionsIncludedInPositions, bool isAccrualsNeeded, bool isUnallocatedTradesIncludedInPositions, bool isBetaNeeded, bool isFxRateNeeded, bool isUnallocatedTradesPermitted)
        {
            GenericPositionData genericPositionData = new GenericPositionData();
            try
            {
                genericPositionData.GivenDate = date;
                genericPositionData.CommaSapratedAssetIDs = commaSapratedAssetIDs;
                genericPositionData.CommaSapratedAccountIds = commaSapratedAccountIds;
                genericPositionData.Symbols = symbols;
                genericPositionData.CustomConditions = customConditions;
                genericPositionData.IsSameDateInAllAUEC = isSameDateInAllAUEC;
                genericPositionData.IsTransactionsIncludedInPositions = isTransactionsIncludedInPositions;
                genericPositionData.IsAccrualsNeeded = isAccrualsNeeded;
                genericPositionData.IsBetaNeeded = isBetaNeeded;
                genericPositionData.IsFxRateNeeded = isFxRateNeeded;

                GenericPositionDataManager.GetData(ref genericPositionData);

                if (isUnallocatedTradesIncludedInPositions && genericPositionData.UnallocatedTrades != null && genericPositionData.UnallocatedTrades.Count > 0 && isUnallocatedTradesPermitted)
                    genericPositionData.Positions.AddRange(genericPositionData.UnallocatedTrades);

                //Apply Grouping on Taxlots
                genericPositionData.Positions = CreateGroupedData(listGrouping, genericPositionData.Positions);

                if (genericPositionData.Transactions != null && genericPositionData.Transactions.Count > 0)
                {
                    List<string> listAccountSymbolGrouping = new List<string>();
                    listAccountSymbolGrouping.Add("Symbol");
                    listAccountSymbolGrouping.Add("Account");
                    genericPositionData.Transactions = CreateGroupedData(listGrouping, genericPositionData.Transactions);
                    genericPositionData.GenericDayEndDataValue.StartOfDayAccountWiseCash = GenericPositionDataManager.AddCashAndCashImpact(genericPositionData.GenericDayEndDataValue.StartOfDayAccountWiseCash, GenericPositionDataManager.CalculateAccountWiseCashImpact(genericPositionData.Transactions));
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
            return genericPositionData;
        }

        /// <summary>
        /// Fetch positions for AUECWise dates
        /// </summary>
        /// <param name="datesForAUECs"></param>
        /// <returns></returns>
        // Datetype is one among Trade/Process/OriginalPurchase Date.
        public List<TaxLot> GetOpenPositionsOrTransactions(string auecDatesString, bool isTodaysTransactions, string CommaSeparatedAccountIds, int dateType)
        {
            List<TaxLot> _lsTaxlots = new List<TaxLot>();
            try
            {
                _lsTaxlots = PranaPositionDataManager.GetOpenPositionsOrTransactions(auecDatesString, isTodaysTransactions, CommaSeparatedAccountIds, string.Empty, string.Empty, string.Empty, dateType);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _lsTaxlots;
        }

        public List<TaxLot> GetOpenPositionsOrTransactions(string auecDatesString, bool isTodaysTransactions, string CommaSeparatedAccountIds, string CommaSeparatedAssetIDs, string commaSeparatedSymbols, string customConditions, int dateType)
        {
            List<TaxLot> _lsTaxlots = new List<TaxLot>();
            try
            {
                _lsTaxlots = PranaPositionDataManager.GetOpenPositionsOrTransactions(auecDatesString, isTodaysTransactions, CommaSeparatedAccountIds, CommaSeparatedAssetIDs, commaSeparatedSymbols, customConditions, dateType);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return _lsTaxlots;
        }

        /// <summary>
        /// Fetch positions for AUECWise dates
        /// </summary>
        /// <param name="datesForAUECs"></param>
        /// <returns></returns>
        public List<TaxLot> GetOpenPositionsOrTransactionsForASymbol(string symbol, string auecDatesString, string CommaSeparatedAccountIds, string orderSideTagValue)
        {
            List<TaxLot> _lsTaxlots = new List<TaxLot>();
            try
            {
                _lsTaxlots = PranaPositionDataManager.GetOpenPositionsOrTransactionsForASymbol(symbol, auecDatesString, CommaSeparatedAccountIds, orderSideTagValue);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _lsTaxlots;
        }

        //Retrive open position of all the taxlots from PM_Taxlot table
        public List<TaxLot> GetOpenPositionsFromDB(DateTime GivenDate, bool IsTodaysTransactions, string CommaSapratedAssetIDs, string CommaSapratedAccountIds)
        {
            List<TaxLot> _lsTaxlots = new List<TaxLot>();
            try
            {
                DataSet ds = PranaPositionDataManager.GetOpenPositionsFromDB(GivenDate, IsTodaysTransactions, CommaSapratedAssetIDs, CommaSapratedAccountIds, false);
                return PranaPositionDataManager.GetTaxlotsFromDataset(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new List<TaxLot>();
        }

        //method arguments modified to get trades between two dates, new field FromDate added
        public DataSet FetchDataForGivenSpName(ReconParameters reconParameters, string commaSeparatedAssetIDs, string commaSeparatedAccountIDs)
        {
            try
            {
                //Modified by omshiv, isConsiderNirvanaProcessDate, get data based on pranProcessDate or AUEClocalDate
                return PranaPositionDataManager.FetchDataForGivenSpName(reconParameters, commaSeparatedAssetIDs, commaSeparatedAccountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataSet();
        }

        public List<TaxLot> GetUnallocatedTradesForDateString(string auecDatesString)
        {
            List<TaxLot> _lsTaxlots = new List<TaxLot>();
            try
            {
                _lsTaxlots = PranaPositionDataManager.GetOpenUnallocatedTradesForDateString(auecDatesString);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _lsTaxlots;
        }

        public List<TaxLot> GetPostDatedTransactions(string auecDatesString, int dateType)
        {
            List<TaxLot> _lsTaxlots = new List<TaxLot>();
            try
            {
                _lsTaxlots = PranaPositionDataManager.GetPostDatedTransactions(auecDatesString, dateType);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _lsTaxlots;
        }

        public List<TaxLot> GetTransactions(DateTime date)
        {
            List<TaxLot> _lsTaxlots = new List<TaxLot>();
            try
            {
                _lsTaxlots = PranaPositionDataManager.GetOpenPositions(date, true);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _lsTaxlots;
        }

        public List<TaxLot> GetTransactionsForDateRange(DateTime FromDate, DateTime ToDate)
        {
            List<TaxLot> _lsTaxlots = new List<TaxLot>();
            try
            {
                _lsTaxlots = PranaPositionDataManager.GetTransactions(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _lsTaxlots;
        }

        public bool ReProcessSnapShot()
        {
            bool isSuccess = false;
            try
            {
                int rowsEffected = PranaPositionDataManager.SaveSnapShotData();
                if (rowsEffected > 0)
                    isSuccess = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSuccess;
        }

        public bool CheckIfReprocessingRequired(List<TaxLot> taxLots)
        {
            bool isReq = false;
            try
            {
                if (taxLots.Count == 0)
                {
                    return false;
                }
                taxLots.Sort(delegate (TaxLot t1, TaxLot t2) { return t1.AUECLocalDate.CompareTo(t2.AUECLocalDate); });
                DateTime snapShotDate = GetSnapShotDate();
                if (snapShotDate > taxLots[0].AUECLocalDate)
                    isReq = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isReq;
        }

        public DateTime GetSnapShotDate()
        {
            DateTime snapShotDate = new DateTime();
            try
            {
                snapShotDate = PranaPositionDataManager.GetSnapShotDate();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return snapShotDate;
        }

        public bool SaveSnapShotDate(DateTime givenDate)
        {
            bool isSuccess = false;
            try
            {
                int rowsEffected = PranaPositionDataManager.SaveSnapShotDate(givenDate);
                if (rowsEffected > 0)
                    isSuccess = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSuccess;
        }

        #region Getting All PS Symbols

        public async Task<Dictionary<string, string>> GetAllPSSymbolsforRisk(List<PSSymbolRequestObject> PSReqObjectListforRisk)
        {
            Dictionary<string, string> psSymbolResponse = new Dictionary<string, string>();
            try
            {
                psSymbolResponse = await System.Threading.Tasks.Task.Run(() => { return GetAllPSSymbols(PSReqObjectListforRisk); });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return psSymbolResponse;
        }

        public Dictionary<string, string> GetAllPSSymbols(List<PSSymbolRequestObject> PSSymbolRequestObjList)
        {
            Dictionary<string, string> dictPSSymbols = new Dictionary<string, string>();
            try
            {
                foreach (PSSymbolRequestObject psReqObj in PSSymbolRequestObjList)
                {
                    if (!dictPSSymbols.ContainsKey(psReqObj.Symbol))
                    {
                        //Getting PS Symbol from Manual Mapping
                        string psSymbol = RiskPreferenceManager.GetPSSymbol(psReqObj.Symbol);
                        if (psSymbol.Equals(psReqObj.Symbol) && psReqObj.AssetID != (int)AssetCategory.Indices)
                        {
                            InformationReporter.GetInstance.Write("Invalid PS mapping for Symbol : " + psReqObj.Symbol + "\t AUECID :" + psReqObj.AUECID + "\t Incorrect PS Due to 'RiskPreferenceManager.GetPSSymbol'");
                            Logger.LoggerWrite("Invalid PS mapping for Symbol : " + psReqObj.Symbol + "\t AUECID :" + psReqObj.AUECID + "\t Incorrect PS Due to 'RiskPreferenceManager.GetPSSymbol'", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        //If we don't get the symbol in the manual mapping, then apply the logic to generating ps symbol
                        if (String.IsNullOrEmpty(psSymbol))
                        {
                            psSymbol = PSSymbolGenerator.GetPSSymbol(psReqObj, _secMasterServices);
                            if (psSymbol.Equals(psReqObj.Symbol) && psReqObj.AssetID != (int)AssetCategory.Indices)
                            {
                                InformationReporter.GetInstance.Write("Invalid PS mapping for Symbol : " + psReqObj.Symbol + "\t AUECID :" + psReqObj.AUECID + "\t Incorrect PS Due to 'PSSymbolGenerator.GetPSSymbol'");
                                Logger.LoggerWrite("Invalid PS mapping for Symbol : " + psReqObj.Symbol + "\t AUECID :" + psReqObj.AUECID + "\t Incorrect PS Due to 'PSSymbolGenerator.GetPSSymbol'", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            }
                            if (String.IsNullOrEmpty(psSymbol) && psReqObj.AssetID != (int)AssetCategory.Indices)
                            {
                                InformationReporter.GetInstance.Write("Invalid PS mapping for Symbol : " + psReqObj.Symbol + "\t AUECID :" + psReqObj.AUECID + "\t PS Symbol Returned is Empty'");
                                Logger.LoggerWrite("Invalid PS mapping for Symbol : " + psReqObj.Symbol + "\t AUECID :" + psReqObj.AUECID + "\t PS Symbol Returned is Empty'", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            }
                        }
                        dictPSSymbols.Add(psReqObj.Symbol, psSymbol);
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
            return dictPSSymbols;
        }
        #endregion

        //Used only in Cash Management
        public Dictionary<string, List<TaxLot>> GetOpenPositionsAndTransactions(DateTime FromDate, DateTime ToDate, string CommaSapratedAssetIDs, string CommaSapratedAccountIds, string CommaSeparatedCustomConditions)
        {
            Dictionary<string, List<TaxLot>> dicDateWisePositions = new Dictionary<string, List<TaxLot>>();
            try
            {
                dicDateWisePositions = PranaPositionDataManager.GetOpenPositionsAndTransactions(FromDate, ToDate, CommaSapratedAccountIds, CommaSapratedAssetIDs, CommaSeparatedCustomConditions);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dicDateWisePositions;
        }

        /// <summary>
        /// This method returns list of taxlot for which we have to update cash activity and journals for the day end fx rate
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="CommaSapratedAssetIDs"></param>
        /// <param name="CommaSapratedAccountIds"></param>
        /// <param name="CommaSeparatedCustomConditions"></param>
        /// <returns></returns>
        public List<TaxLot> GetTransactionsToUpdateSettlementFxRate(DateTime ToDate, string CommaSapratedAccountIds)
        {
            List<TaxLot> lstTransaction = new List<TaxLot>();
            try
            {
                lstTransaction = PranaPositionDataManager.GetTransactionsToUpdateSettlementFxRate(ToDate.ToString(), CommaSapratedAccountIds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return lstTransaction;
        }

        private void FilterDataToAvoidNameChangeCA(List<TaxLot> data)
        {
            try
            {
                for (int i = data.Count - 1; i >= 0; i--)
                {
                    TaxLot _currentTaxLot = data[i];
                    if (_currentTaxLot.PositionTag == PositionTag.LongWithdrawal || _currentTaxLot.PositionTag == PositionTag.ShortAddition
                        || _currentTaxLot.PositionTag == PositionTag.ShortWithdrawal || _currentTaxLot.PositionTag == PositionTag.LongAddition)
                    {
                        data.Remove(_currentTaxLot);
                    }
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

        //public List<TaxLot> GetCashJournalExceptionalTransactions(DateTime fromDate,DateTime toDate)
        /// <summary>
        /// Modified by: Bharat raturi, 1 aug 2014
        /// purpose: send the account ids to pick only the data after the cash management start date of the accounts
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="accountIDs"></param>
        /// <returns></returns>
        public List<TaxLot> GetCashJournalExceptionalTransactions(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            try
            {
                return PranaPositionDataManager.GetTaxlotsFromDataset(PranaPositionDataManager.GetCashJournalExceptionalTransactions(fromDate, toDate, accountIDs));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Get the account locking dictionary from cache
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetAccountsLockStatus()
        {
            try
            {
                return CachedDataManager.GetInstance.GetAccountUserLockDetail();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return null;
        }

        /// <summary>
        /// Added By Faisal Shah 19/09/14
        /// Added because of the reason that 
        /// we can find how many accounts are available to be locked by a user
        /// </summary>
        /// <param name="accountList"></param>
        /// <returns></returns>
        public List<int> GetListOfUnlockedAccounts(List<int> accountList)
        {
            List<int> unLockedAccounts = new List<int>();
            try
            {
                Dictionary<int, int> accountUserLockDetail = CachedDataManager.GetInstance.GetAccountUserLockDetail();
                foreach (int accountID in accountList)
                {
                    if (!accountUserLockDetail.ContainsKey(accountID))
                    {
                        unLockedAccounts.Add(accountID);
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
            return unLockedAccounts;
        }

        /// <summary>
        /// set the account locking dictionary
        /// if account are already acquired by user then it returns true
        /// if requested account are acquired by some other user then false is return and no changes are made
        /// </summary>
        /// <param name="userID"></param>
        /// User ID f the user to modify their no. of accounts locked
        /// <param name="accountsToBeLocked"></param>
        /// Accounts that user want ot get lock of
        /// <returns></returns>
        public bool SetAccountsLockStatus(int userID, List<int> accountsToBeLocked)
        {
            try
            {
                Dictionary<int, int> accountUserLockDetail = CachedDataManager.GetInstance.GetAccountUserLockDetail();
                Dictionary<int, int> newaccountUserLockDetail = new Dictionary<int, int>();
                //get accounts lock
                foreach (int account in accountsToBeLocked)
                {
                    //if account is free or user holds the account
                    if (accountUserLockDetail.ContainsKey(account) && accountUserLockDetail[account] == userID)
                    {
                        newaccountUserLockDetail.Add(account, userID);
                    }
                    //if account is locked b some other user
                    else if (accountUserLockDetail.ContainsKey(account) && accountUserLockDetail[account] != userID)
                    {
                        return false;
                    }
                    //if account is new to server then it adds to cache
                    else if (!accountUserLockDetail.ContainsKey(account))
                    {
                        newaccountUserLockDetail.Add(account, userID);
                    }
                }
                //release Accounts lock
                foreach (KeyValuePair<int, int> kvp in accountUserLockDetail)
                {
                    if (!newaccountUserLockDetail.ContainsKey(kvp.Key))
                    {
                        //if account is previously locked but user released it then dont add it
                        if (!accountsToBeLocked.Contains(kvp.Key) && accountUserLockDetail[kvp.Key] != userID)
                        {
                            newaccountUserLockDetail.Add(kvp.Key, kvp.Value);
                        }
                    }
                }
                CachedDataManager.GetInstance.SetAccountUserLockDetail(newaccountUserLockDetail);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return true;
        }
        #endregion

        private ISecMasterServices _secMasterServices;
        public ISecMasterServices SecMasterServices
        {
            set { _secMasterServices = value; }
        }

        private Dictionary<string, List<TaxLot>> CreateSymbolWiseDictionary(List<TaxLot> positionList)
        {
            Dictionary<string, List<TaxLot>> dictSymbolWisePositions = new Dictionary<string, List<TaxLot>>();
            try
            {
                foreach (TaxLot pos in positionList)
                {
                    if (!dictSymbolWisePositions.ContainsKey(pos.Symbol))
                    {
                        List<TaxLot> listPos = new List<TaxLot>();
                        listPos.Add(pos);
                        dictSymbolWisePositions.Add(pos.Symbol, listPos);
                    }
                    else
                    {
                        dictSymbolWisePositions[pos.Symbol].Add(pos);
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
            return dictSymbolWisePositions;
        }

        private List<TaxLot> CreateGroupedData(List<string> listGrouping, List<TaxLot> taxlotstogroup)
        {
            List<TaxLot> groupedTaxlots = new List<TaxLot>();
            try
            {
                if (listGrouping == null)
                {
                    return taxlotstogroup;
                }
                Dictionary<string, TaxLot> dictFinalGroupPos = new Dictionary<string, TaxLot>();
                //create dictionary based on symbols 
                Dictionary<string, List<TaxLot>> dictSymbolWisePositions = CreateSymbolWiseDictionary(taxlotstogroup);

                foreach (KeyValuePair<string, List<TaxLot>> symbolListPos in dictSymbolWisePositions)
                {
                    List<TaxLot> listPos = symbolListPos.Value;
                    if (listPos.Count == 1)
                    {
                        groupedTaxlots.Add((TaxLot)listPos[0].Clone());
                        continue;
                    }
                    foreach (TaxLot pos in listPos)
                    {
                        string compareKey = GetCompareKey(pos, listGrouping);
                        if (!dictFinalGroupPos.ContainsKey(compareKey))
                        {
                            dictFinalGroupPos.Add(compareKey, (TaxLot)pos.Clone());
                        }
                        else
                        {
                            TaxLot targetPos = dictFinalGroupPos[compareKey];
                            _taxlotGroupingManager.GroupPositionDetails(targetPos, pos);
                        }
                    }
                }
                // for which grouping is tried
                foreach (KeyValuePair<string, TaxLot> groupPos in dictFinalGroupPos)
                {
                    groupedTaxlots.Add(groupPos.Value);
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
            return groupedTaxlots;
        }

        private string GetCompareKey(TaxLot pos, List<string> listGrouping)
        {
            StringBuilder compareKey = new StringBuilder();
            compareKey.Append(pos.Symbol);

            if (listGrouping.Contains("Side"))
            {
                compareKey.Append(pos.OrderSideTagValue);
            }
            if (listGrouping.Contains("Account"))
            {
                compareKey.Append(pos.Level1ID);
            }
            if (listGrouping.Contains("Strategy"))
            {
                compareKey.Append(pos.Level2ID);
            }
            return compareKey.ToString();
        }

        public void RefershPSSymbolMappingCache(DataSet dsPSSymbolMapping)
        {
            try
            {
                RiskPreferenceManager.RefreshPSSymbolMapping(dsPSSymbolMapping);
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

        public void ReconPreferenceSaved(int userID)
        {
            try
            {
                if (_proxyPublishing == null)
                {
                    CreatePublishingProxy();
                }
                MessageData e = new MessageData();
                // Publishing data after updating from db
                List<int> lstUserID = new List<int>();
                lstUserID.Add(userID);
                e.EventData = lstUserID;
                e.TopicName = Topics.Topic_ReconPreferenceUpdated;
                _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_ReconPreferenceUpdated);
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

        /// <summary>
        /// Gets data for cost adjustment taxlots
        /// </summary>
        /// <returns>List of data for cost adjustment taxlots</returns>
        public List<CostAdjustmentTaxlotsForSave> GetCostAdjustmentData()
        {
            List<CostAdjustmentTaxlotsForSave> costAdjustmentTaxlots = new List<CostAdjustmentTaxlotsForSave>();
            try
            {
                costAdjustmentTaxlots = AllocationDataManager.GetCostAdjustmentData();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return costAdjustmentTaxlots;
        }

        /// <summary>
        /// Hides the order from blotter.
        /// </summary>
        /// <param name="parentClOrderIds">The parent cl order ids.</param>
        public int HideOrderFromBlotter(string parentClOrderIds, bool isAllSubOrdersRemovable, int companyUserID, List<int> uniqueTradingAccounts)
        {
            try
            {
                int rowsAffected = PranaPositionDataManager.HideOrderFromBlotter(parentClOrderIds);
                if (rowsAffected > 0)
                {
                    if (_proxyPublishing == null)
                    {
                        CreatePublishingProxy();
                    }
                    StageOrderRemovalData stageOrderRemovalData = new StageOrderRemovalData();
                    stageOrderRemovalData.ParentClOrderIds = parentClOrderIds;
                    stageOrderRemovalData.IsAllSubOrdersRemovable = isAllSubOrdersRemovable;
                    stageOrderRemovalData.CompanyUserID = companyUserID;
                    stageOrderRemovalData.UniqueTradingAccounts = uniqueTradingAccounts;

                    List<StageOrderRemovalData> lstStageOrderRemovalData = new List<StageOrderRemovalData>();
                    lstStageOrderRemovalData.Add(stageOrderRemovalData);

                    MessageData e = new MessageData();
                    e.EventData = lstStageOrderRemovalData;
                    e.TopicName = Topics.Topic_StageOrderRemovalFromBlotter;
                    _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_StageOrderRemovalFromBlotter);
                }
                return rowsAffected;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return -1;
        }

        /// <summary>
        /// Hides the sub orders from blotter.
        /// </summary>
        /// <param name="subOrderClOrderID">The sub order cl order i ds.</param>
        /// <param name="companyUserID">The company user identifier.</param>
        /// <param name="uniqueTradingAccounts">The unique trading accounts.</param>
        /// <returns></returns>
        public int HideSubOrderFromBlotter(string subOrderClOrderID, int companyUserID, List<int> uniqueTradingAccounts)
        {
            try
            {
                int rowsAffected = PranaPositionDataManager.HideSubOrdersFromBlotter(subOrderClOrderID);
                if (rowsAffected > 0)
                {
                    if (_proxyPublishing == null)
                    {
                        CreatePublishingProxy();
                    }
                    SubOrderRemovalData subOrderRemovalData = new SubOrderRemovalData();
                    subOrderRemovalData.SubOrdersClOrderIds = subOrderClOrderID;
                    subOrderRemovalData.CompanyUserID = companyUserID;
                    subOrderRemovalData.UniqueTradingAccounts = uniqueTradingAccounts;

                    List<SubOrderRemovalData> lstSubOrderRemovalData = new List<SubOrderRemovalData>();
                    lstSubOrderRemovalData.Add(subOrderRemovalData);

                    MessageData e = new MessageData();
                    e.EventData = lstSubOrderRemovalData;
                    e.TopicName = Topics.Topic_SubOrderRemovalFromBlotter;
                    _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_SubOrderRemovalFromBlotter);
                }
                return rowsAffected;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return -1;
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
