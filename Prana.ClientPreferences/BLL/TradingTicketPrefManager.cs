using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.TTPrefs;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;

namespace Prana.ClientPreferences
{
    public class TradingTicketPrefManager : IDisposable
    {
        #region singleton instance
        private static TradingTicketPrefManager _tradingTicketPrefManager = null;
        static object _locker = new object();
        public static TradingTicketPrefManager GetInstance
        {
            get
            {
                if (_tradingTicketPrefManager == null)
                {
                    lock (_locker)
                    {
                        if (_tradingTicketPrefManager == null)
                        {
                            _tradingTicketPrefManager = new TradingTicketPrefManager();
                        }
                    }
                }
                return _tradingTicketPrefManager;
            }
        }

        private TradingTicketPrefManager()
        { }
        #endregion

        #region userWise instance
        private static Dictionary<int, TradingTicketPrefManager> _userWisetradingPrefManagers = new Dictionary<int, TradingTicketPrefManager>();
        public static TradingTicketPrefManager GetInstanceForUser(int userId, int companyID)
        {
            if (!_userWisetradingPrefManagers.ContainsKey(userId))
            {
                lock (_locker)
                {
                    if (!_userWisetradingPrefManagers.ContainsKey(userId))
                    {
                        TradingTicketPrefManager prefManager = new TradingTicketPrefManager();
                        prefManager.Initialise(TradingTicketPreferenceType.User, userId, companyID);
                        prefManager.GetPreferenceBindingData(true, true);                        
                        _userWisetradingPrefManagers.Add(userId, prefManager);
                    }
                }
            }
            return _userWisetradingPrefManagers[userId];
        }
        #endregion

        private Dictionary<Asset, Sides> _assetSides;
        private Sides _sides;
        private CounterPartyCollection _brokers;
        private VenueCollection _venues;
        private OrderTypes _orderTypes;
        private TimeInForces _timeInForces;
        private HandlingInstructions _handlingInstructions;
        private ExecutionInstructions _executionInstructions;
        private TradingAccountCollection _tradingAccounts;
        private StrategyCollection _strategies;
        private AccountCollection _accounts;
        private CurrencyCollection _settlementCurrencies;
        private TradingTicketUIPrefs _tradingTicketUiPrefs;
        private TradingTicketUIPrefs _companyTradingTicketUiPrefs;
        private TradingTicketRulesPrefs _tradingTicketRulesPrefs;
        ProxyBase<IAllocationManager> _allocationProxy = null;

        private TradingTicketPreferenceType _ttPreferenceType;
        private int _userID;
        private int _companyID;

        public Dictionary<Asset, Sides> AssetSides
        {
            get { return _assetSides; }
            set { _assetSides = value; }
        }

        public Sides Sides
        {
            get { return _sides; }
            set { _sides = value; }
        }

        public CounterPartyCollection Brokers
        {
            get { return _brokers; }
            set { _brokers = value; }
        }

        public VenueCollection Venues
        {
            get { return _venues; }
            set { _venues = value; }
        }

        public OrderTypes OrderTypes
        {
            get { return _orderTypes; }
            set { _orderTypes = value; }
        }

        public TimeInForces TimeInForces
        {
            get { return _timeInForces; }
            set { _timeInForces = value; }
        }

        public HandlingInstructions HandlingInstructions
        {
            get { return _handlingInstructions; }
            set { _handlingInstructions = value; }
        }

        public ExecutionInstructions ExecutionInstructions
        {
            get { return _executionInstructions; }
            set { _executionInstructions = value; }
        }

        public TradingAccountCollection TradingAccounts
        {
            get { return _tradingAccounts; }
            set { _tradingAccounts = value; }
        }

        public StrategyCollection Strategies
        {
            get { return _strategies; }
            set { _strategies = value; }
        }

        public AccountCollection Accounts
        {
            get { return _accounts; }
            set { _accounts = value; }
        }

        public CurrencyCollection SettlementCurrencies
        {
            get { return _settlementCurrencies; }
            set { _settlementCurrencies = value; }
        }

        public TradingTicketUIPrefs TradingTicketUiPrefs
        {
            get { return _tradingTicketUiPrefs; }
            set { _tradingTicketUiPrefs = value; }
        }

        /// <summary>
        /// Gets or sets the Trading rules preferences
        /// </summary>
        public TradingTicketRulesPrefs TradingTicketRulesPrefs
        {
            get { return _tradingTicketRulesPrefs; }
            set { _tradingTicketRulesPrefs = value; }
        }

        public TradingTicketUIPrefs CompanyTradingTicketUiPrefs
        {
            get { return _companyTradingTicketUiPrefs; }
            set { _companyTradingTicketUiPrefs = value; }
        }

        private static bool? _dollarAmountPermission = null;
        public static bool? DollarAmountPermission
        {
            get
            {
                return _dollarAmountPermission ?? (_dollarAmountPermission = GetDollarAmountPermission());
            }
        }
        private static bool? _dollarAmountPTTPermission = null;
        public static bool? DollarAmountPTTPermission
        {
            get
            {
                return _dollarAmountPTTPermission ?? (_dollarAmountPTTPermission = GetDollarAmountPTTPermission());
            }
        }

        /// <summary>
        /// to get the value of dollar amount permission from the database
        /// </summary>
        /// <returns></returns>
        public static bool GetDollarAmountPermission()
        {
            try
            {
                DataSet result = null;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetDollarAmountPermission";

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (result.Tables[0].Rows.Count != 0)
                    return Boolean.Parse(result.Tables[0].Rows[0]["TT"].ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }
        /// <summary>
        /// to get the value of dollar amount permission from the database
        /// </summary>
        /// <returns></returns>
        public static bool GetDollarAmountPTTPermission()
        {
            try
            {
                DataSet result = null;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetDollarAmountPermission";

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (result.Tables[0].Rows.Count != 0)
                    return Boolean.Parse(result.Tables[0].Rows[0]["PTT"].ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }
        private bool? _pstCompanyModuleForUserPermission = null;
        public bool? PSTCompanyModuleForUserPermission
        {
            get
            {
                return _pstCompanyModuleForUserPermission ?? (_pstCompanyModuleForUserPermission = GetPSTCompanyModuleForUserPermission(_userID));
            }
        }
        /// <summary>
        /// to get the value of company user module permission for pst from the database
        /// </summary>
        /// <returns></returns>
        public static bool GetPSTCompanyModuleForUserPermission(int companyUserID)
        {
            try
            {
                DataSet result = null;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCompanyModulesForUser";
                queryData.DictionaryDatabaseParameter.Add("@companyUserID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@companyUserID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyUserID
                });

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    // Iterate through the rows to check for the specific ModuleName
                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        if (row["ModuleName"] != null && row["ModuleName"].ToString() == "% Trading Tool")
                        {
                            return true; // Module found
                        }
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
            return false;
        }
        public void Initialise(TradingTicketPreferenceType ttPreferenceType, int userID, int companyID)
        {
            _ttPreferenceType = ttPreferenceType;
            _userID = userID;
            _companyID = companyID;
            TradingTicketPreferenceDataManager.SetupManager(ttPreferenceType);
            if (ttPreferenceType == TradingTicketPreferenceType.User)
            {
                if (_allocationProxy == null)
                {
                    _allocationProxy = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                }
            }
        }

        public void GetPreferenceBindingData(bool isLevelingPerferenceRequired, bool isProrataByNavPerferenceRequired)
        {
            int idToUse = _ttPreferenceType == TradingTicketPreferenceType.Company ? _companyID : _userID;

            _assetSides = TradingTicketPreferenceDataManager.GetCompanyAssets();
            _brokers = TradingTicketPreferenceDataManager.GetAllCompanyPermittedCounterparties(idToUse);
            _orderTypes = TradingTicketPreferenceDataManager.GetOrderTypes(_companyID);
            _timeInForces = TradingTicketPreferenceDataManager.GetTimeInForces(_companyID);
            _executionInstructions = TradingTicketPreferenceDataManager.GetExecutionInstructions(_companyID);
            _handlingInstructions = TradingTicketPreferenceDataManager.GetHandlingInstructions(_companyID);
            _accounts = TradingTicketPreferenceDataManager.GetAccounts(idToUse);
            _tradingAccounts = TradingTicketPreferenceDataManager.GetTradingAccounts(idToUse);
            _strategies = TradingTicketPreferenceDataManager.GetStrategies(idToUse);
            _settlementCurrencies = TradingTicketPreferenceDataManager.GetCurrencies();

            _tradingTicketUiPrefs = _ttPreferenceType == TradingTicketPreferenceType.Company ? TradingTicketPreferenceDataManager.GetTTCompanyUIPreferences(_companyID) : TradingTicketPreferenceDataManager.GetTTUserUIPreferences(_userID);
            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                _companyTradingTicketUiPrefs = TradingTicketPreferenceDataManager.GetTTCompanyUIPreferences(_companyID);
                if (_allocationProxy != null)
                {
                    //This method is returning preferences which are created from Edit Allocation preferences UI, PRANA-23524
                    Dictionary<int, string> preferences = _allocationProxy.InnerChannel.GetAllocationPreferences(_companyID, _userID, isLevelingPerferenceRequired, isProrataByNavPerferenceRequired);
                    if (preferences != null)
                    {
                        foreach (Account accountRow in preferences.Select(allocations => new Account
                        {
                            AccountID = allocations.Key,
                            Name = allocations.Value
                        }))
                        {
                            _accounts.Add(accountRow);
                        }
                    }
                }
            }
            _tradingTicketRulesPrefs = TradingTicketPreferenceDataManager.GetTradingRulesPreferences(_companyID);
        }

        public void GetVenuesBasedOnSelectedCounterparty(int brokerID)
        {
            int idToUse = _ttPreferenceType == TradingTicketPreferenceType.Company ? _companyID : _userID;
            _venues = TradingTicketPreferenceDataManager.GetVenues(idToUse, brokerID, _ttPreferenceType);
        }

        public void SaveTTPreference(TradingTicketUIPrefs preferences)
        {
            if (_ttPreferenceType == TradingTicketPreferenceType.Company)
            {
                TradingTicketPreferenceDataManager.SaveTTCompanyUIPreferences(_companyID, preferences);
            }
            else
            {
                TradingTicketPreferenceDataManager.SaveTTUserUIPreferences(_userID, preferences);
            }
            _tradingTicketUiPrefs = preferences;
        }

        /// <summary>
        /// Saves the trading rules preference.
        /// </summary>
        /// <param name="ttRulesPreferences">The tt rules preferences.</param>
        public void SaveTradingRulesPreference(TradingTicketRulesPrefs ttRulesPreferences)
        {
            try
            {
                TradingTicketPreferenceDataManager.SaveTTRulesPreferences(_companyID, ttRulesPreferences);
                _tradingTicketRulesPrefs = ttRulesPreferences;
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

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_allocationProxy != null)
                        _allocationProxy.Dispose();
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

        #endregion

        /// <summary>
        /// Saves the Security List based on restricted or allowed type
        /// </summary>
        public void SaveSecuritiesList(int companyID, string securitiesListType, string securitiesListToSave, bool isTickerSymbology)
        {
            try
            {
                TradingTicketPreferenceDataManager.SaveSecuritiesList(companyID, securitiesListType, securitiesListToSave, isTickerSymbology);
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
        /// Deletes the Security List based on restricted or allowed type
        /// </summary>
        public void DeleteSecuritiesList(string securitiesListType)
        {
            try
            {
                TradingTicketPreferenceDataManager.DeleteSecuritiesList(_companyID, securitiesListType);
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
        /// Gets the Security List based on restricted or allowed type
        /// </summary>
        public Tuple<string, bool> GetSecuritiesList(string securitiesListType)
        {
            return TradingTicketPreferenceDataManager.GetSecuritiesList(_companyID, securitiesListType);
        }
    }
}
