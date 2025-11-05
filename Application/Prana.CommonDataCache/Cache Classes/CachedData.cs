//using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.GreenFieldModels;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Prana.CommonDataCache
{
    internal class CachedData
    {
        private static CachedData _cachedData = null;
        static Dictionary<int, string> _assets = new Dictionary<int, string>();
        static Dictionary<int, string> _cashAccounts = new Dictionary<int, string>();
        static Dictionary<int, string> _subCashAccounts = new Dictionary<int, string>();
        static Dictionary<string, int> _subCashAccounts_Acronym = new Dictionary<string, int>();
        static Dictionary<int, List<int>> _releaseWiseAccount = new Dictionary<int, List<int>>();
        static Dictionary<string, int> _activityType = new Dictionary<string, int>();
        private static IClientsCommonDataManager _clientsCommonDataManager;
        private static IKeyValueDataManager _keyValueDataManager;
        public Dictionary<string, int> ActivityType
        {
            get { return _activityType; }
            set { _activityType = value; }
        }

        static Dictionary<int, string> _activityAmountType = new Dictionary<int, string>();
        public Dictionary<int, string> ActivityAmountType
        {
            get { return _activityAmountType; }
            set { _activityAmountType = value; }
        }

        static DataTable _activityJournalMapping = new DataTable();
        public DataTable ActivityJournalMapping
        {
            get { return _activityJournalMapping; }
            set { _activityJournalMapping = value; }
        }

        static DataTable _activityTypeDetails = new DataTable();
        public DataTable ActivityTypeDetails
        {
            get { return _activityTypeDetails; }
            set { _activityTypeDetails = value; }
        }

        static Dictionary<string, int> _balanceTypeID = new Dictionary<string, int>();
        public Dictionary<string, int> BalanceTypeID
        {
            get { return _balanceTypeID; }
            set { _balanceTypeID = value; }
        }

        static Dictionary<int, byte> _activityTypeActivitySource = new Dictionary<int, byte>();
        public Dictionary<int, byte> ActivityTypeActivitySource
        {
            get { return _activityTypeActivitySource; }
            set { _activityTypeActivitySource = value; }
        }

        static Dictionary<string, string> _activityTypeWithAcronym = new Dictionary<string, string>();
        public Dictionary<string, string> ActivityTypeWithAcronym
        {
            get { return _activityTypeWithAcronym; }
            set { _activityTypeWithAcronym = value; }
        }

        static Dictionary<int, Dictionary<string, int>> _subAccounts_Side_Multipier = new Dictionary<int, Dictionary<string, int>>();
        public Dictionary<int, Dictionary<string, int>> SubAccounts_Side_Multipier
        {
            get { return _subAccounts_Side_Multipier; }
            set { _subAccounts_Side_Multipier = value; }
        }


        private static Dictionary<int, string> _subAccounts_AccountType = new Dictionary<int, string>();
        public Dictionary<int, string> SubAccounts_AccountType
        {
            get { return _subAccounts_AccountType; }
            set { _subAccounts_AccountType = value; }
        }

        //modified by: Bharat raturi 9 oct 2014
        //Maintain the dictionary as there are multiple revaluation dates snow
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-5107
        //private static DateTime _lastRevaluationCalculationDate = DateTime.MinValue;
        private static Dictionary<int, RevaluationUpdateDetail> _lastRevaluationCalculationDate = new Dictionary<int, RevaluationUpdateDetail>();

        public Dictionary<int, RevaluationUpdateDetail> LastRevaluationCalcDate
        {
            get { return _lastRevaluationCalculationDate; }
            set { _lastRevaluationCalculationDate = value; }
        }

        static Dictionary<int, string> _closingAssets = null;

        //static Dictionary<int, string> _masterCategory = new Dictionary<int, string>();
        //static Dictionary<int, string> _subCategory = new Dictionary<int, string>();
        //static Dictionary<int, string> _Transaction = new Dictionary<int, string>();

        static Dictionary<int, string> _underLyings = new Dictionary<int, string>();
        static Dictionary<int, string> _exchanges = new Dictionary<int, string>();
        static Dictionary<string, int> _exchangeIdentifiers = new Dictionary<string, int>();
        /// <summary>
        /// Added Rajat. 06 Oct 2006
        /// </summary>
        static Dictionary<int, string> _currencies = new Dictionary<int, string>();
        static Dictionary<int, string> _counterParties = new Dictionary<int, string>();
        static Dictionary<int, string> _venues = new Dictionary<int, string>();
        static Dictionary<int, string> _users = new Dictionary<int, string>();
        static Dictionary<int, string> _tradingaccounts = new Dictionary<int, string>();
        static Dictionary<int, string> _auecs = new Dictionary<int, string>();
        static Dictionary<int, int> _roundOff = new Dictionary<int, int>();
        static Dictionary<int, byte[]> _flags = new Dictionary<int, byte[]>();
        static Dictionary<int, string> _accounts = new Dictionary<int, string>();
        static Dictionary<string, string> _pranaImportTags = new Dictionary<string, string>();
        static readonly Dictionary<string, Dictionary<string, string>> _counterPartyVenuesSymbolConventions = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Added By: Bharat raturi
        /// Create a dictionary of (companyID-AccountID list) pair
        /// </summary>
        static Dictionary<int, List<Account>> _companyWiseAccounts = new Dictionary<int, List<Account>>();
        public Dictionary<int, List<Account>> CompanyAccountsMapping
        {
            get { return _companyWiseAccounts; }
        }
        static Dictionary<int, string> _accountsWithFullName = new Dictionary<int, string>();
        static Dictionary<int, string> _strategies = new Dictionary<int, string>();
        static Dictionary<int, double> _auecMultipliers = new Dictionary<int, double>();
        static Dictionary<int, decimal> _auecRoundLot = new Dictionary<int, decimal>();
        // DataTable _tradingPermissions = new DataTable();

        static Dictionary<int, int> _auecIdToAssetDict = new Dictionary<int, int>();
        //static Dictionary<string, SortedDictionary<DateTime, ConversionRate>> _symbolAndDateWiseFXRates = new Dictionary<string, SortedDictionary<DateTime, ConversionRate>>();
        static Dictionary<int, List<int>> _masterFundSubAccountAssociation = new Dictionary<int, List<int>>();

        static Dictionary<int, List<int>> _dictMasterFundsThridParty = null;
        static Dictionary<int, List<int>> _dictThridPartyMasterFunds = null;
        static List<string> _lstMasterFundsAndAccounts = null;

        static DataTable _assetsExchangeIdentifiers = null;
        static Dictionary<int, string> _masterFunds = new Dictionary<int, string>();
        static Dictionary<int, string> _accountGroups = new Dictionary<int, string>();
        static IList<MasterFundAccountDetails> _customGroups;
        static Dictionary<int, string> _counterPartyVenue = new Dictionary<int, string>();
        static Venue _exAssignVenue = null;
        static List<GenericNameID> _primeBrokers = new List<GenericNameID>();
        static Dictionary<int, string> _masterStrategy = new Dictionary<int, string>();
        static Dictionary<int, Prana.BusinessObjects.PositionManagement.ThirdPartyNameID> _dataSourceLookup = new Dictionary<int, Prana.BusinessObjects.PositionManagement.ThirdPartyNameID>();
        static Dictionary<int, Prana.BusinessObjects.TimeZone> _auecIDTimeZones = new Dictionary<int, Prana.BusinessObjects.TimeZone>();
        static Dictionary<int, StructSettlementPeriodSidewise> _auecIDSettlementPeriods = new Dictionary<int, StructSettlementPeriodSidewise>();
        static DataTable _company = null;
        static Dictionary<int, String> _companies = new Dictionary<int, string>();

        //added by: Bharat raturi, 29 apr 2014
        //purpose: provide dictionaries for third party and third party-account mapping
        static Dictionary<int, string> _thirdParties = new Dictionary<int, string>();
        static Dictionary<int, string> _thirdPartiesWithShortName = new Dictionary<int, string>();
        static Dictionary<int, List<int>> _thirdPartyAccounts = new Dictionary<int, List<int>>();

        //static AccountingMethods _accountingMethods = null;
        //static DataSet _accountSubAccount = new DataSet();
        static DataSet _lastAvailablefxratesLessThanToday = new DataSet();

        static DataSet _dsCashAccountTablesWithRelation = new DataSet();
        static DataSet _dsCashAccountTables = new DataSet();
        static DataSet _dsCashActivityTables = new DataSet();
        static Dictionary<string, string> _dictTransactionType = new Dictionary<string, string>();
        static Dictionary<int, Tuple<DateTime, bool>> _cashPreferenceFunds = new Dictionary<int, Tuple<DateTime, bool>>();

        static Dictionary<string, string> _dictTransactionType_Acronym = new Dictionary<string, string>();

        //maintains the cache for all the accounts acquired by all the users 
        //Dictionary<AccountID,UserID>
        static Dictionary<int, int> _accountUserLockDetail = new Dictionary<int, int>();
        static Dictionary<int, string> _companyModules = new Dictionary<int, string>();

        static List<string> _lstPendingApprovalFrozenAlerts = new List<string>();
        public Dictionary<int, int> AccountUserLockDetail
        {
            get { return CachedData._accountUserLockDetail; }
            set { CachedData._accountUserLockDetail = value; }
        }

        /// <summary>
        /// maintains the account loggedIn user has locked to work on
        /// </summary>
        static List<int> _accountsLocked = new List<int>();
        public List<int> AccountsLocked
        {
            get { return CachedData._accountsLocked; }
        }


        /// <summary>
        /// Maintains accounts id and time before they were last used
        /// Concurent Dictionary is used for avoiding conflict while running multiple instances
        /// </summary>
        static ConcurrentDictionary<int, int> _accountsLockDuration = new ConcurrentDictionary<int, int>();
        public ConcurrentDictionary<int, int> AccountsLockDuration
        {
            get { return CachedData._accountsLockDuration; }
        }
        /// <summary>
        /// The NAV lock date
        /// </summary>
        static DateTime? _nAVLockDate = null;
        /// <summary>
        /// Gets or sets the NAV lock date.
        /// </summary>
        /// <value>
        /// The NAV lock date.
        /// </value>
        public DateTime? NAVLockDate
        {
            get { return CachedData._nAVLockDate; }
            set { CachedData._nAVLockDate = value; }
        }

        /// Reset timer to the value
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="value"></param>
        public void ResetAccountsLockTimer(int accountID, int value)
        {
            try
            {
                if (_accountsLockDuration.ContainsKey(accountID))
                {
                    _accountsLockDuration[accountID] = value;
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

        private static Dictionary<int, List<int>> _companyThirdPartyAccounts = new Dictionary<int, List<int>>();

        public Dictionary<int, List<int>> CompanyThirdPartyAccounts
        {
            get { return _companyThirdPartyAccounts; }
        }

        private static Dictionary<int, int> _accountWiseExecutingBrokerMapping = new Dictionary<int, int>();

        public Dictionary<int, int> AccountWiseExecutingBrokerMapping
        {
            get { return _accountWiseExecutingBrokerMapping; }
        }

        
        private static int _auecCount;

        public static int AUECCount
        {
            get { return _auecCount; }
        }

        private static int _preferencedAccountID;

        public static int PreferencedAccountID
        {
            get
            {
                return _preferencedAccountID;
            }
            set { _preferencedAccountID = value; }
        }
        private Prana.BusinessObjects.TimeZone _currentTimeZone;

        public Prana.BusinessObjects.TimeZone CurrentTimeZone
        {
            get
            {
                if (_currentTimeZone == null)
                    _currentTimeZone = Prana.BusinessObjects.TimeZoneInfo.FindTimeZoneByString(System.TimeZoneInfo.Local.DaylightName);
                return _currentTimeZone;
            }
            set { _currentTimeZone = value; }
        }

        public Prana.BusinessObjects.TimeZone GetTimeZoneByUserID()
        {
            _currentTimeZone = Prana.BusinessObjects.TimeZoneInfo.FindTimeZoneByString(System.TimeZoneInfo.Local.DaylightName);
            return _currentTimeZone;
        }

        static CachedData()
        {
            try
            {
                _cachedData = new CachedData();
                _clientsCommonDataManager = WindsorContainerManager.Container.Resolve<IClientsCommonDataManager>();
                _keyValueDataManager = WindsorContainerManager.Container.Resolve<IKeyValueDataManager>();
                SetCommonData();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public static CachedData GetInstance()
        {

            return _cachedData;
        }

        public Dictionary<int, string> CounterParty
        {
            get { return _counterParties; }
        }

        public Dictionary<int, string> Asset
        {
            get { return _assets; }
        }
        public Dictionary<int, string> Underlying
        {
            get { return _underLyings; }
        }
        public Dictionary<int, string> Exchange
        {
            get { return _exchanges; }
        }

        public Dictionary<string, int> ExchangeIdentifiers
        {
            get { return _exchangeIdentifiers; }
        }

        public Dictionary<int, decimal> AuecRoundLot
        {
            get { return _auecRoundLot; }
        }

        public Dictionary<int, double> AuecMultipliers
        {
            get { return _auecMultipliers; }
        }

        //commented by Omshiv, Nov 2013, Moved to securityMaster project
        //#region UDA DATA Dictiories,
        //static Dictionary<int, string> _UDAAssets = new Dictionary<int, string>();
        //static Dictionary<int, string> _UDASectors = new Dictionary<int, string>();
        //static Dictionary<int, string> _UDASubSectors = new Dictionary<int, string>();
        //static Dictionary<int, string> _UDASecurityTypes = new Dictionary<int, string>();
        //static Dictionary<int, string> _UDACountries = new Dictionary<int, string>();
        //public Dictionary<int, string> UDAAssets
        //{
        //    get { return _UDAAssets; }
        //}

        //public Dictionary<int, string> UDASectors
        //{
        //    get { return _UDASectors; }
        //}
        //public Dictionary<int, string> UDASubSectors
        //{
        //    get { return _UDASubSectors; }
        //}
        //public Dictionary<int, string> UDASecurityTypes
        //{
        //    get { return _UDASecurityTypes; }
        //}
        //public Dictionary<int, string> UDACountries
        //{
        //    get { return _UDACountries; }
        //}
        //#endregion



        public Dictionary<int, string> CashAccounts
        {
            get { return _cashAccounts; }
        }
        public Dictionary<int, string> SubCashAccounts
        {
            get { return _subCashAccounts; }
        }

        public Dictionary<string, int> SubCashAccounts_Acronym
        {
            get { return _subCashAccounts_Acronym; }
        }

        /// <summary>
        /// It contains the symbol wise and datewise sorted FX conversionrate objects
        /// </summary>
        //public Dictionary<string, SortedDictionary<DateTime, ConversionRate>> SymbolAndDateWiseFXRates
        //{
        //    get { return _symbolAndDateWiseFXRates; }
        //}


        /// <summary>
        /// Author : rajat 06-10-2006
        /// Gets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public Dictionary<int, string> Currency
        {
            get { return _currencies; }
        }

        public Dictionary<int, string> Venues
        {
            get { return _venues; }
        }

        public Dictionary<int, string> TradingAccounts
        {
            get { return _tradingaccounts; }
        }
        public Dictionary<int, string> Users
        {
            get { return _users; }
        }
        public Dictionary<int, string> AUECs
        {
            get { return _auecs; }
        }
        public Dictionary<int, int> RoundOffRules
        {
            get { return _roundOff; }
        }
        public Dictionary<int, byte[]> Flags
        {
            get { return _flags; }
        }
        public Dictionary<int, string> ClosingAssets
        {
            get { return _closingAssets; }
        }
        public Dictionary<int, string> Accounts
        {
            get { return _accounts; }
        }
        public Dictionary<string, string> PranaImportTags
        {
            get { return _pranaImportTags; }
        }
        public Dictionary<int, string> AccountsWithFullName
        {
            get { return _accountsWithFullName; }
        }
        public Dictionary<int, string> Strategies
        {
            get { return _strategies; }
        }
        public Dictionary<int, double> AUECMultiplier
        {
            get { return _auecMultipliers; }
        }

        public Dictionary<int, int> AUECIdToAssetDict
        {
            get { return _auecIdToAssetDict; }
        }

        public Dictionary<int, string> ThirdParties
        {
            get { return _thirdParties; }
        }

        public Dictionary<int, string> ThirdPartiesWithShortName
        {
            get { return _thirdPartiesWithShortName; }
        }

        public Dictionary<int, List<int>> ThirdPartyAccounts
        {
            get { return _thirdPartyAccounts; }
        }

        public int GetThirdPartyIDOfAccount(int AccountID)
        {
            int thirdParty = 0;
            try
            {
                thirdParty = _thirdPartyAccounts.FirstOrDefault(x => x.Value.Contains(AccountID)).Key;
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
            return thirdParty;
        }

        public DataTable AssetsExchangeIdentifiers
        {
            get { return _assetsExchangeIdentifiers; }
        }

        private static int _companyID;

        public static int CompanyID
        {
            get { return _companyID; }
        }

        private static string _companyName;
        public static string CompanyName
        {
            get { return _companyName; }
        }

        private static DataSet _attributeNames;

        public DataSet AttributeNames
        {
            get { return _attributeNames; }
        }

        /// <summary>
        /// waiting time to find out out trade is stuck, time is in seconds.
        /// </summary>
        private int _WaitTimeToGetStuckTrade;
        public int WaitTimeToGetStuckTrade
        {
            get { return _WaitTimeToGetStuckTrade; }
            set { _WaitTimeToGetStuckTrade = value; }
        }

        private int _emailIntervalForStuckTrades;
        public int EmailIntervalForStuckTrades
        {
            get { return _emailIntervalForStuckTrades; }
            set { _emailIntervalForStuckTrades = value; }
        }


        private static bool _isSendRealtimeManualOrderViaFix;
        public static bool IsSendRealtimeManualOrderViaFix
        {
            get { return _isSendRealtimeManualOrderViaFix; }
        }

        private static int _companyBaseCurrencyID;
        public static int CompanyBaseCurrencyID
        {
            get { return _companyBaseCurrencyID; }
        }

        private static MarketDataProvider _companyMarketDataProvider;
        public static MarketDataProvider CompanyMarketDataProvider
        {
            get { return _companyMarketDataProvider; }
        }

        private static bool _isMarketDataBlocked;
        public static bool IsMarketDataBlocked
        {
            get { return _isMarketDataBlocked; }
        }

        private static FactSetContractType _companyFactSetContractType;
        public static FactSetContractType CompanyFactSetContractType
        {
            get { return _companyFactSetContractType; }
        }

        public static SecondaryMarketDataProvider _secondaryMarketDataProvider;
        public static SecondaryMarketDataProvider SecondaryCompanyMarketDataProvider
        {
            get { return _secondaryMarketDataProvider; }
        }

        private static bool _isMarketDataPermissionEnabled;
        public static bool IsMarketDataPermissionEnabled
        {
            get { return _isMarketDataPermissionEnabled; }
            set { _isMarketDataPermissionEnabled = value; }
        }

        private static bool _isSecurityValidationLoggingEnabled;
        public static bool IsSecurityValidationLoggingEnabled
        {
            get
            {
                return _isSecurityValidationLoggingEnabled;
            }
        }

        private static bool _isInMarketDataPermissionEnabledForTradingRules;
        public static bool IsInMarketDataPermissionEnabledForTradingRules
        {
            get { return _isInMarketDataPermissionEnabledForTradingRules; }
            set { _isInMarketDataPermissionEnabledForTradingRules = value; }
        }

        public Dictionary<int, List<int>> ReleaseWiseAccount
        {
            get { return _releaseWiseAccount; }
        }

        private static ConcurrentDictionary<int, int> _dictAccountWiseBaseCurrency = new ConcurrentDictionary<int, int>();
        public ConcurrentDictionary<int, int> DictAccountWiseBaseCurrency
        {
            get { return _dictAccountWiseBaseCurrency; }
            set { _dictAccountWiseBaseCurrency = value; }
        }

        public void SetAttributeNames(DataSet attributes)
        {
            _attributeNames = attributes;
        }

        public Dictionary<int, Tuple<DateTime, bool>> CashPreferenceFunds
        {
            get { return _cashPreferenceFunds; }
        }

        public Dictionary<string, string> dictTransactionType_Acronym
        {
            get { return _dictTransactionType_Acronym; }
        }


        static Dictionary<string, string> _pranaPreferences;

        public Dictionary<string, string> PranaPreferences
        {
            get { return CachedData._pranaPreferences; }
        }

        static int _securityValidationTimeOut;
        public int SecurityValidationTimeOut
        {
            get { return _securityValidationTimeOut; }

        }

        static int _permissibleQuickTTInstances = 5;
        public int PermissibleQuickTTInstances
        {
            get { return _permissibleQuickTTInstances; }

        }

        //added by: Bharat raturi, 23 apr 2014
        //purpose: provide flag variables for storing the value for many to many mapping  for account master fund and strategy master strategy
        static bool _IsAccountManyToManyMappingAllowed = false;

        public bool IsAccountManyToManyMappingAllowed
        {
            get { return _IsAccountManyToManyMappingAllowed; }
            set { _IsAccountManyToManyMappingAllowed = value; }

        }

        static bool _IsStrategyManyToManyMappingAllowed = false;

        public bool IsStrategyManyToManyMappingAllowed
        {
            get { return _IsStrategyManyToManyMappingAllowed; }
            set { _IsStrategyManyToManyMappingAllowed = value; }
        }
        //add finished

        static bool _isNAVLockingEnabled = false;

        public bool IsNAVLockingEnabled
        {
            get { return _isNAVLockingEnabled; }
            set { _isNAVLockingEnabled = value; }
        }


        static bool _isAccountLockingEnabled = false;

        public bool IsAccountLockingEnabled
        {
            get { return _isAccountLockingEnabled; }
            set { _isAccountLockingEnabled = value; }
        }

        static bool _isFeederAccountEnabled = false;

        /// <summary>
        /// added by: Bharat Raturi, 06-may-2014
        /// purpose: variable to store the value whether the feeder accounts for the client are enabled 
        /// </summary>
        public bool IsFeederAccountEnabled
        {
            get { return _isFeederAccountEnabled; }
            set { _isFeederAccountEnabled = value; }
        }

        //TODO: Remove Unused Prana Preference Properties after verification
        static int _pricingSource = 0;

        public int PricingSource
        {
            get { return CachedData._pricingSource; }
            set { CachedData._pricingSource = value; }
        }

        /// <summary>
        /// The average price rounding
        /// </summary>
        static int _avgPriceRounding = 0;

        /// <summary>
        /// Gets or sets the average price rounding.
        /// </summary>
        /// <value>
        /// The average price rounding.
        /// </value>
        public int AvgPriceRounding
        {
            get { return _avgPriceRounding; }
            set { _avgPriceRounding = value; }
        }
        static int _settlementAutoCalculateField = 0;

        public int SettlementAutoCalculateField
        {
            get { return CachedData._settlementAutoCalculateField; }
            set { CachedData._settlementAutoCalculateField = value; }
        }


        public Dictionary<string, string> DictTransactionType
        {
            get { return _dictTransactionType; }
        }

        static bool _isShowMasterFundonTT = false;
        public bool IsShowMasterFundonTT
        {
            get { return _isShowMasterFundonTT; }
            set { _isShowMasterFundonTT = value; }
        }

        static bool _isShowMasterFundonShortLocate = false;
        public bool IsShowMasterFundonShortLocate
        {
            get { return _isShowMasterFundonShortLocate; }
            set { _isShowMasterFundonShortLocate = value; }
        }

        static bool _isImportOverrideOnShortLocate = false;
        public bool IsImportOverrideOnShortLocate
        {
            get { return _isImportOverrideOnShortLocate; }
            set { _isImportOverrideOnShortLocate = value; }
        }

        /// <summary>
        /// The is equity option manual validation
        /// </summary>
        static bool _isEquityOptionManualValidation;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is equity option manual validation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is equity option manual validation; otherwise, <c>false</c>.
        /// </value>
        public bool IsEquityOptionManualValidation
        {
            get { return _isEquityOptionManualValidation; }
            set { _isEquityOptionManualValidation = value; }
        }

        /// <summary>
        /// The is collateral mark price validation
        /// </summary>
        static bool _isCollateralMarkPriceValidation;

        static bool _isShowTillSettlementDate;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is collateral mark price.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is collateral mark price validation; otherwise, <c>false</c>.
        /// </value>
        public bool IsCollateralMarkPriceValidation
        {
            get { return _isCollateralMarkPriceValidation; }
            set { _isCollateralMarkPriceValidation = value; }
        }
        public bool IsShowTillSettlementDate
        {
            get { return _isShowTillSettlementDate; }
            set { _isShowTillSettlementDate = value; }
        }
        /// <summary>
        /// The is file pricing for touch
        /// </summary>
        static bool _isFilePricingForTouch;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is file pricing for touch.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is file pricing for touch; otherwise, <c>false</c>.
        /// </value>
        public bool IsFilePricingForTouch
        {
            get { return _isFilePricingForTouch; }
            set { _isFilePricingForTouch = value; }
        }

        static bool _isBreakOrderPreference;

        /// <summary>
        /// Gets or sets a value indicating whether this instance will break orders for PTT and RB.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance will break orders for PTT and RB; otherwise, <c>false</c>.
        /// </value>
        public bool IsBreakOrderPreference
        {
            get { return _isBreakOrderPreference; }
            set { _isBreakOrderPreference = value; }
        }

        static Dictionary<Tuple<int, string>, int> _mappedDefaultAUEC = new Dictionary<Tuple<int, string>, int>();
        internal Dictionary<Tuple<int, string>, int> dictDefaultAUECMapping
        {
            get { return _mappedDefaultAUEC; }
        }

        static Dictionary<string, int> _countryWiseFactsetCodes = new Dictionary<string, int>();
        internal Dictionary<string, int> dictCountryWiseFactsetCode
        {
            get { return _countryWiseFactsetCodes; }
        }

        static Dictionary<string, int> _countryWiseBloombergCodes = new Dictionary<string, int>();
        internal Dictionary<string, int> dictCountryWiseBloombergCode
        {
            get { return _countryWiseBloombergCodes; }
        }

        static Dictionary<int, int> _masterFundTradingAccountMapping = new Dictionary<int, int>();
        public Dictionary<int, int> MasterFundTradingAccountMapping
        {
            get { return _masterFundTradingAccountMapping; }
            set { _masterFundTradingAccountMapping = value; }
        }

        static bool _isShowmasterFundAsClient = false;
        public bool IsShowmasterFundAsClient
        {
            get { return _isShowmasterFundAsClient; }
            set { _isShowmasterFundAsClient = value; }
        }


        static bool _isPermanentDeletionEnabled = true;
        public bool IsPermanentDeletionEnabled
        {
            get { return _isPermanentDeletionEnabled; }
            set { _isPermanentDeletionEnabled = value; }
        }

        /// <summary>
        /// Is Window User Req
        /// </summary>
        static bool _isWindowUserReq = false;
        public bool IsWindowUserReq
        {
            get { return _isWindowUserReq; }
        }

        private static Dictionary<int, string> _algoBrokersWithFullName;
        private static Dictionary<int, string> _algoBrokersWithShortName;

        /// <summary>
        /// The send allocations via fix
        /// </summary>
        private static bool _sendAllocationsViaFix = false;

        /// <summary>
        /// Gets or sets a value indicating whether [send allocations via fix].
        /// </summary>
        /// <value>
        /// <c>true</c> if [send allocations via fix]; otherwise, <c>false</c>.
        /// </value>
        public static bool SendAllocationsViaFix
        {
            get { return _sendAllocationsViaFix; }
            set { _sendAllocationsViaFix = value; }
        }

        /// <summary>
        /// maintains alerts which needs to be frozen
        /// </summary>
        public static List<string> LstPendingApprovalFrozenAlerts
        {
            get { return _lstPendingApprovalFrozenAlerts; }
            set { _lstPendingApprovalFrozenAlerts = value; }
        }

        public static void SetAccountsAndActivityData()
        {
            try
            {
                #region old data fetching code
                //KeyValueDataManager.GetInstance().ResetAllAccountTable();
                //_dsCashAccountTablesWithRelation = ClientsCommonDataManager.GetAllAccountsWithRelation();
                //_cashAccounts = KeyValueDataManager.GetInstance().getAccounts();
                //_subCashAccounts = KeyValueDataManager.GetInstance().getSubAccounts();
                //_subCashAccounts_Acronym = KeyValueDataManager.GetInstance().getSubAccounts_Acronym();
                ////dictionary which contains activity type name and activity type name 
                //_activityType = KeyValueDataManager.GetInstance().GetActivityType();
                ////datatable containg journal mapping with the activity on the basis of Journal Code
                //_activityJournalMapping = KeyValueDataManager.GetInstance().GetActivityJournalMapping();
                ////ActivityTypeDetails is a datatable which contains ActivityTypeId, ActivityType, Description, BalanceType, AssetId, SideMultiplier, CustomRule columns
                //_activityTypeDetails = KeyValueDataManager.GetInstance().GetActivityTypeDetails();
                ////datatable containing cash transaction type and activity type.
                ////eg. there are two enteries for dividend div accured and div settled
                //_cashTRNActivityMapping = KeyValueDataManager.GetInstance().GetCashTRNActivityMapping();
                ////dictionary which contains CashTransactionTypeId and CashTransactionTypeName
                //_cashTransactionType = KeyValueDataManager.GetInstance().GetCashTransactionType();
                ////dictionary which contains AmountTypeId and AmountType
                //_activityAmountType = KeyValueDataManager.GetInstance().GetActivityAmountType();
                ////dictionary which contains activity type name and balance type
                //_balanceTypeID = KeyValueDataManager.GetInstance().GetActivityWithBalanceTypeID();
                //_subAccounts_Side_Multipier = KeyValueDataManager.GetInstance().getSubAccounts_Side_Multipier();
                //_subAccounts_TransactionType = KeyValueDataManager.GetInstance().getSubAccounts_TransactionType();
                #endregion
                //code is modified because there was unnecessary dbcalls for the same sp.
                _dsCashAccountTables = _clientsCommonDataManager.GetAllAccountTablesFromDB();
                //add relationship to dataset _dsCashAccountTables 
                _dsCashAccountTablesWithRelation = _clientsCommonDataManager.GetAllAccountsWithRelation(_dsCashAccountTables);
                _keyValueDataManager.ResetAllAccountTable(_dsCashAccountTables, _dsCashAccountTablesWithRelation);
                //_subCashAccounts is a dictionary having SubAccountId(int) and Name(string)       
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-6293
                _subCashAccounts = _keyValueDataManager.getSubAccountsWithMasterCategoryName();
                //_subCashAccounts_Acronym is a dictionary having Acronym(string) and SubAccountId(int) 
                _subCashAccounts_Acronym = _keyValueDataManager.getSubAccounts_Acronym();
                _dsCashActivityTables = _clientsCommonDataManager.GetAllActivitiesFromDB();
                //set all activity tables in cache
                _keyValueDataManager.SetAllActivityTables(_dsCashActivityTables);
                //dictionary which contains activity type name and activity type id
                _activityType = _keyValueDataManager.GetActivityType();
                //datatable containing journal mapping with the activity on the basis of Journal Code
                _activityJournalMapping = _dsCashActivityTables.Tables["ActivityJournalMapping"];
                //ActivityTypeDetails is a datatable which contains ActivityTypeId, ActivityType, Description, BalanceType, AssetId, SideMultiplier, CustomRule columns
                _activityTypeDetails = _dsCashActivityTables.Tables["ActivityType"];
                //dictionary which contains AmountTypeId and AmountType
                _activityAmountType = _keyValueDataManager.GetActivityAmountType();
                //dictionary which contains activity type name and balance type
                _balanceTypeID = _keyValueDataManager.GetActivityWithBalanceTypeID();
                _subAccounts_Side_Multipier = _keyValueDataManager.getSubAccounts_Side_Multipier();
                _subAccounts_AccountType = _keyValueDataManager.getSubAccounts_AccountType();
                _activityTypeActivitySource = _keyValueDataManager.GetActivityTypeActivitySource();
                _lastRevaluationCalculationDate = _keyValueDataManager.GetLastRevaluationCalcDate();
                _activityTypeWithAcronym = _keyValueDataManager.GetActivityTypeWithAcronym();

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

        private static void SetCommonData()
        {
            try
            {
                // Modified by Ankit Gupta on 10 Oct, 2014
                // For CH release type, call a different stored procedure, because in CH the concept of active & inactive exists whereas in Prana mode, this concept 
                // doesn't exists. Therefore, here release view type is used as a parameter.
                GetPranaPreferences();
                DataSet keyValuePairs = _keyValueDataManager.GetKeyValuePairs();
                _assets = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[0], 0);
                _counterParties = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[1], 0);
                _venues = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[2], 0);
                _users = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[3], 0);
                _underLyings = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[4], 0);
                _exchanges = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[5], 0);
                _auecs = _keyValueDataManager.FillAUEC(keyValuePairs.Tables[6]);
                _currencies = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[7], 0);
                _tradingaccounts = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[8], 0);
                _accounts = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[9], 0);
                _accountsWithFullName = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[9], 0); // 6281

                //Modified By: Bharat Raturi
                //store the accounts company-wise
                _companyWiseAccounts = _keyValueDataManager.FillAccountsCompanyWise(keyValuePairs.Tables[9]);

                _dictAccountWiseBaseCurrency = _keyValueDataManager.FillAccountWiseBaseCurrency(keyValuePairs.Tables[9], 3);

                _strategies = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[10], 0);
                if (!_strategies.ContainsKey(0))
                {
                    _strategies.Add(0, "Strategy Unallocated");
                }
                _thirdParties = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[11], 0);
                _thirdPartiesWithShortName = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[11], 1);

                _thirdPartyAccounts = _keyValueDataManager.FillThirdPartyAccounts(keyValuePairs.Tables[12], 0);

                _pranaImportTags = _keyValueDataManager.FillImportTag(keyValuePairs.Tables[13], 0);
                _closingAssets = _keyValueDataManager.GetClosingAssets();
                _releaseWiseAccount = _keyValueDataManager.FillReleaseWiseAccount(keyValuePairs.Tables[14]);

                SetAccountsAndActivityData();
                _lastAvailablefxratesLessThanToday = _clientsCommonDataManager.GetLatestAvailableFxRatesLessThanToday();
                _exchangeIdentifiers = _keyValueDataManager.GetExchangeIdentifiers();

                _roundOff = _keyValueDataManager.GetRoundOffRules();
                _auecCount = _auecs.Count;
                _flags = _keyValueDataManager.GetFlagsbyAUECs();

                _auecMultipliers = _keyValueDataManager.GetAUECMultipliers();
                _auecRoundLot = _keyValueDataManager.GetAUECRoundLots();
                // _contractMultipliers = _keyValueDataManager.GetContractMultipliers();
                _auecIdToAssetDict = _keyValueDataManager.GetAUECIdToAssetMapping();
                _assetsExchangeIdentifiers = _keyValueDataManager.GetAssetsExchangeIdentifiers();
                _auecIDTimeZones = _keyValueDataManager.GetAUECIDTimeZones();
                _auecIDSettlementPeriods = _keyValueDataManager.GetAUECIDSettlementPeriods();
                _company = _keyValueDataManager.GetCompany();
                _companyID = int.Parse(_company.Rows[0]["CompanyID"].ToString());
                _companyName = _company.Rows[0]["CompanyName"].ToString();
                _sendAllocationsViaFix = _keyValueDataManager.GetSendAllocationsViaFix();
                _companies = _keyValueDataManager.GetCompanies();
                _nAVLockDate = _keyValueDataManager.GetCurrentNavLockDate();
                //_symbolAndDateWiseFXRates = ClientsCommonDataManager.GetFXConversionRates();
                _masterFunds = _keyValueDataManager.GetAllMasterFunds();
                _accountGroups = _keyValueDataManager.GetAllAccountGroups();
                _customGroups = _keyValueDataManager.GetAllCustomGroups();
                _counterPartyVenue = _keyValueDataManager.GetAllCounterPartyVenues();
                _exAssignVenue = _keyValueDataManager.GetExAssignVenueID();
                _primeBrokers = _keyValueDataManager.GetAllPrimeBrokers();
                _masterStrategy = _keyValueDataManager.GetAllMasterStrategy();
                _dataSourceLookup = _keyValueDataManager.GetAllDataSources();
                _masterFundSubAccountAssociation = _keyValueDataManager.GetCompanyMasterFundSubAccountAssociation(_companyID);
                _companyModules = _keyValueDataManager.GetCompanyModulesPermissioning(_companyID);
                _countryWiseFactsetCodes = _keyValueDataManager.GetCountryWiseFactsetCodes();
                _countryWiseBloombergCodes = _keyValueDataManager.GetCountryWiseBloombergCodes();

                _companyBaseCurrencyID = int.Parse(_clientsCommonDataManager.GetCompanyBaseCurrency(_companyID).ToString());
                _isInMarketDataPermissionEnabledForTradingRules = _clientsCommonDataManager.GetIsInMarketIncludedForTradingRules(_companyID);
                _isSendRealtimeManualOrderViaFix = _clientsCommonDataManager.IsSendRealtimeManualOrderViaFix(_companyID);
                _dictTransactionType = _clientsCommonDataManager.GetTransactionTypeFromDB();
                _cashPreferenceFunds = _clientsCommonDataManager.GetCashPreferenceFundsFromDB();
                
                _preferencedAccountID = _keyValueDataManager.GetLastPreferencedAccountID();
                _attributeNames = _keyValueDataManager.GetAttributeNames();
                _algoBrokersWithFullName = _keyValueDataManager.GetAlgoBrokersWithFullName();
                _algoBrokersWithShortName = _keyValueDataManager.GetAlgoBrokersWithShortName();
                FillSymbolConvertionForCounterPartyVenues(_clientsCommonDataManager.GetCounterPartyVernuesSymbolConvertions());
                _masterFundTradingAccountMapping = _clientsCommonDataManager.GetAllMasterFundsTradingAccounts(_companyID);
                _mappedDefaultAUEC = _clientsCommonDataManager.GetDefaultAUECDict(_companyID);
                _isBreakOrderPreference = _clientsCommonDataManager.GetBreakOrderPreference(_companyID);
                FetchSecondaryCompanyMarketDataProvider();
                FetchCompanyMarketDataProvider();
                FetchMarketDataBlockedInformation();
                FetchFactSetContractType();
                CreateTransactionTypeAcronymsDictionary();
                
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("IsSecurityValidationLoggingEnabled")) && ConfigurationManager.AppSettings["IsSecurityValidationLoggingEnabled"] != null)
                {
                    bool.TryParse((ConfigurationManager.AppSettings["IsSecurityValidationLoggingEnabled"]), out _isSecurityValidationLoggingEnabled);
                }
                // fill account lock timer
                _companyThirdPartyAccounts = _thirdPartyAccounts;
                _pranaImportTags = _pranaImportTags = _keyValueDataManager.FillImportTag(_clientsCommonDataManager.GetAllImportTags(), 0);
                _accountsLockDuration.Clear();
                foreach (int accountID in CachedData.GetInstance().AccountsWithFullName.Keys)
                {
                    _accountsLockDuration.TryAdd(accountID, int.MinValue);
                }

                _accountWiseExecutingBrokerMapping = _clientsCommonDataManager.GetAccountWiseExecutingBrokerMappingFromDB(_companyID);
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

        /// <summary>
        /// This Method is for creating Acronym Dictionary for Transaction Type
        /// </summary>
        private static void CreateTransactionTypeAcronymsDictionary()
        {
            try
            {
                foreach (KeyValuePair<string, string> kvp in _dictTransactionType)
                {
                    // this dictionary is used to get transaction type acromyn from transaction type name
                    // key is transaction type name and vaule is Acronym
                    if (!_dictTransactionType_Acronym.ContainsKey(kvp.Value))
                    {
                        _dictTransactionType_Acronym.Add(kvp.Value, kvp.Key);
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

        /// <summary>
        /// Added By: Bharat Raturi, 27 may 2014
        /// purpose: Get the More frequently used details refreshed in the cache
        /// </summary>
        public void RefreshFrequentlyUsedData()
        {
            try
            {
                DataSet keyValuePairs = _keyValueDataManager.GetFrequentlyUsedData();
                _users = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[0], 0);
                _currencies = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[1], 0);
                _accounts = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[2], 0);
                _accountsWithFullName = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[2], 1);
                _companyWiseAccounts = _keyValueDataManager.FillAccountsCompanyWise(keyValuePairs.Tables[2]);
                _strategies = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[3], 0);
                _thirdParties = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[4], 0);
                _thirdPartiesWithShortName = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[4], 1);
                _thirdPartyAccounts = _keyValueDataManager.FillThirdPartyAccounts(keyValuePairs.Tables[5], 0);
                //Set this to null so that it can be updated from third parties next time it is used
                _dictMasterFundsThridParty = null; _dictThridPartyMasterFunds = null;
                _counterParties = _keyValueDataManager.FillKeyValuePairs(keyValuePairs.Tables[6], 0);
                _companies = _keyValueDataManager.GetCompanies();
                //_masterFunds = _keyValueDataManager.GetAllMasterFunds();
                _accountGroups = _keyValueDataManager.GetAllAccountGroups();
                _counterPartyVenue = _keyValueDataManager.GetAllCounterPartyVenues();
                _masterFunds = _keyValueDataManager.GetAllMasterFunds();
                _masterStrategy = _keyValueDataManager.GetAllMasterStrategy();


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

        /// <summary>
        /// get Prana pref 
        /// Omshiv, March 2014
        /// </summary>
        public static void GetPranaPreferences()
        {

            try
            {
                DataSet pranaPref = _keyValueDataManager.GetPranaPreference();

                foreach (DataRow row in pranaPref.Tables[0].Rows)
                {
                    string PreferenceKey = row["PreferenceKey"].ToString();
                    string PreferenceValue = row["PreferenceValue"].ToString();
                    if (_pranaPreferences == null)
                        _pranaPreferences = new Dictionary<string, string>();
                    if (!_pranaPreferences.ContainsKey(PreferenceKey))
                        _pranaPreferences.Add(PreferenceKey, PreferenceValue);

                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_NAVLOCK_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isNAVLockingEnabled = true;
                        }
                        else
                        {
                            _isNAVLockingEnabled = false;
                        }
                    }

                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_AccountLOCK_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isAccountLockingEnabled = true;
                        }
                        else
                        {
                            _isAccountLockingEnabled = false;
                        }
                    }

                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_PERMANENTDELETION_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isPermanentDeletionEnabled = true;
                        }
                        else
                        {
                            _isPermanentDeletionEnabled = false;
                        }
                    }


                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_SHOWMASTERFUNCONTT_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isShowMasterFundonTT = true;
                        }
                        else
                        {
                            _isShowMasterFundonTT = false;
                        }
                    }
                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_SHOWMASTERFUNDASCLIENT_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isShowmasterFundAsClient = true;
                        }
                        else
                        {
                            _isShowmasterFundAsClient = false;
                        }
                    }

                    //added by: Bharat Raturi, 23 apr 2014
                    //purpose: to set the value of flag variable for master-account mapping and master-strategy mapping
                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_FUND_MANYTOMANY_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _IsAccountManyToManyMappingAllowed = true;
                        }
                        else
                        {
                            _IsAccountManyToManyMappingAllowed = false;
                        }
                    }

                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_STRATEGY_MANYTOMANY_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _IsStrategyManyToManyMappingAllowed = true;
                        }
                        else
                        {
                            _IsStrategyManyToManyMappingAllowed = false;
                        }
                    }
                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_FEEDERFUND_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isFeederAccountEnabled = true;
                        }
                        else
                        {
                            _isFeederAccountEnabled = false;
                        }
                    }

                    if (PreferenceKey.Equals(ApplicationConstants.CONST_PRICINGSOURCE))
                    {
                        if (PreferenceValue != null)
                        {
                            _pricingSource = Convert.ToInt32(PreferenceValue);
                        }
                    }
                    if (PreferenceKey.Equals(ApplicationConstants.CONST_SettlementAutoCalculateField))
                    {
                        if (PreferenceValue != null)
                        {
                            _settlementAutoCalculateField = Convert.ToInt32(PreferenceValue);
                            SettlementCachePreferences.SettlementAutoCalculateField = (SettlementAutoCalculateField)_settlementAutoCalculateField;
                        }
                    }

                    if (PreferenceKey.Equals(ApplicationConstants.CONST_Is_Window_User_Req))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isWindowUserReq = true;
                        }
                        else
                        {
                            _isWindowUserReq = false;
                        }
                    }

                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_EquityOptionManualValidation_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isEquityOptionManualValidation = true;
                        }
                        else
                        {
                            _isEquityOptionManualValidation = false;
                        }
                    }

                    if (PreferenceKey.Equals(ApplicationConstants.CONST_AVGPRICEROUNDING))
                    {
                        if (PreferenceValue != null)
                        {
                            _avgPriceRounding = Convert.ToInt32(PreferenceValue);
                        }
                    }
                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_CollateralMarkPriceValidation_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isCollateralMarkPriceValidation = true;
                        }
                        else
                        {
                            _isCollateralMarkPriceValidation = false;
                        }
                    }
                    if (PreferenceKey.Equals(ApplicationConstants.Const_Is_ShowTillSettlementDate_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isShowTillSettlementDate = true;
                        }
                        else
                        {
                            _isShowTillSettlementDate = false;
                        }
                    }
                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_ShowmasterFundOnShortLocate_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isShowMasterFundonShortLocate = true;
                        }
                        else
                        {
                            _isShowMasterFundonShortLocate = false;
                        }
                    }
                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_ImportOverrideOnShortLocate_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isImportOverrideOnShortLocate = true;
                        }
                        else
                        {
                            _isImportOverrideOnShortLocate = false;
                        }
                    }
                    if (PreferenceKey.Equals(ApplicationConstants.CONST_IS_FILEPRICINGFORTOUCH_ENABLED))
                    {
                        if (PreferenceValue != null && Convert.ToInt16(PreferenceValue) == 1)
                        {
                            _isFilePricingForTouch = true;
                        }
                        else
                        {
                            _isFilePricingForTouch = false;
                        }
                    }
                    //add finished
                }

                //set symbol validation timeout from app config 
                //default timeout is 5 min
                int securityValidationTimeOut = 300000;
                if (ConfigurationManager.AppSettings["SecurityValidationTimeOut"] != null && int.TryParse((ConfigurationManager.AppSettings["SecurityValidationTimeOut"]), out securityValidationTimeOut))
                    _securityValidationTimeOut = securityValidationTimeOut;
                else
                    _securityValidationTimeOut = securityValidationTimeOut;

                //set permissible quick TT instances from app config
                int permissibleQuickTTInstances;
                if (ConfigurationManager.AppSettings[ConfigurationHelper.CONFIGKEY_PermissibleQuickTTInstances] != null
                    && int.TryParse((ConfigurationManager.AppSettings[ConfigurationHelper.CONFIGKEY_PermissibleQuickTTInstances]), out permissibleQuickTTInstances))
                {
                    if (permissibleQuickTTInstances <= 0)
                        _permissibleQuickTTInstances = 1;
                    else if (permissibleQuickTTInstances > 10)
                        _permissibleQuickTTInstances = 10;
                    else
                        _permissibleQuickTTInstances = permissibleQuickTTInstances;

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

        private void FillMasterFundThirdPartyDictionary()
        {
            try
            {
                _dictMasterFundsThridParty = new Dictionary<int, List<int>>();
                _dictThridPartyMasterFunds = new Dictionary<int, List<int>>();
                foreach (KeyValuePair<int, List<int>> kvpMasterFundAccounts in _masterFundSubAccountAssociation)
                {
                    foreach (int accountID in kvpMasterFundAccounts.Value)
                    {
                        int thirdPartyID = GetThirdPartyIDOfAccount(accountID);
                        if (thirdPartyID != 0)
                        {
                            if (_dictMasterFundsThridParty.ContainsKey(kvpMasterFundAccounts.Key))
                            {
                                _dictMasterFundsThridParty[kvpMasterFundAccounts.Key].Add(thirdPartyID);
                            }
                            else
                            {
                                _dictMasterFundsThridParty.Add(kvpMasterFundAccounts.Key, new List<int>() { thirdPartyID });
                            }


                            if (_dictThridPartyMasterFunds.ContainsKey(thirdPartyID))
                            {
                                _dictThridPartyMasterFunds[thirdPartyID].Add(kvpMasterFundAccounts.Key);
                            }
                            else
                            {
                                _dictThridPartyMasterFunds.Add(thirdPartyID, new List<int>() { kvpMasterFundAccounts.Key });
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

        ///// <summary>
        ///// This function is used to refresh the Cache Data as case might be that we add a new account
        ///// and we may want to see it immidiately on Trading Ticket or in the App.
        ///// So we call this function on reload settings from NirvanaMain.cs to get latest Data from DB 
        ///// Created by Faisal Shah on 06/25/2014
        ///// </summary>
        ///// <param name="userID"></param>
        //public  void RefreshUserPermittedAccountsForCH(int userID)
        //{
        //    try
        //    {
        //        _useraccounts.Clear();
        //        DataTable accountsDT = ClientsCommonDataManager.GetAllPermittedAccounts(userID);
        //        _useraccounts = new AccountCollection();
        //        foreach (DataRow dr in accountsDT.Rows)
        //        {

        //            if (dr["AccountID"] != System.DBNull.Value && dr["AccountName"] != System.DBNull.Value)
        //            {
        //                Account account = new Account();
        //                account.AccountID = int.Parse(dr["AccountID"].ToString());
        //                account.Name = account.FullName = dr["AccountName"].ToString();
        //                _useraccounts.Add(account);
        //            }
        //        }
        //        _useraccounts.Insert(0, new Prana.BusinessObjects.Account(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
        //        FillAccountAndAllocationDefaultsDataTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public DataTable Company
        {
            get
            {
                return _company;
            }
        }

        /// <summary>
        /// Copmanies list
        /// </summary>
        public Dictionary<int, String> Companies
        {
            get
            {
                return _companies;
            }
        }

        public Dictionary<int, Prana.BusinessObjects.TimeZone> AUECIDTimeZones
        {
            get { return _auecIDTimeZones; }
        }

        public Dictionary<int, StructSettlementPeriodSidewise> AUECIDSettlementPeriods
        {
            get { return _auecIDSettlementPeriods; }
        }
        public Dictionary<int, string> MasterFunds
        {
            get { return _masterFunds; }
        }
        public Dictionary<int, string> AccountGroups
        {
            get { return _accountGroups; }
        }
        public IList<MasterFundAccountDetails> CustomGroups
        {
            get { return _customGroups; }
        }
        public Dictionary<int, string> CounterPartyVenue
        {
            get { return _counterPartyVenue; }
        }
        public Venue ExerciseAssignVenue
        {
            get { return _exAssignVenue; }
        }

        public List<GenericNameID> PrimeBrokers
        {
            get { return _primeBrokers; }
        }
        public Dictionary<int, List<int>> MasterFundSubAccountAssociation
        {
            get { return _masterFundSubAccountAssociation; }
        }

        public Dictionary<int, List<int>> DictMasterFundsThridParty
        {
            get
            {
                if (_dictMasterFundsThridParty == null)
                {
                    FillMasterFundThirdPartyDictionary();
                }
                return _dictMasterFundsThridParty;
            }
        }

        public Dictionary<int, List<int>> DictThridPartyMasterFunds
        {
            get
            {
                if (_dictThridPartyMasterFunds == null)
                {
                    FillMasterFundThirdPartyDictionary();
                }
                return _dictThridPartyMasterFunds;
            }
        }

        public List<string> ListMasterFundsAndFunds
        {
            get
            {
                if (_lstMasterFundsAndAccounts == null)
                {
                    FillMasterFundsAndAccountsAssociationList();
                }
                return _lstMasterFundsAndAccounts;
            }
        }


        public Dictionary<int, string> MasterStrategy
        {
            get { return _masterStrategy; }
        }

        public Dictionary<int, Prana.BusinessObjects.PositionManagement.ThirdPartyNameID> DataSources
        {
            get { return _dataSourceLookup; }
        }

        public AllocationDefault GetAllocationDefault(int id)
        {
            try
            {
                //Commenting default region as allocation preference is defined at server side allocation service
                //if (_allocationDefaultCollection.DoesDefaultExist(id))
                //{
                //    AllocationDefault allocationDefaultRule = _allocationDefaultCollection.GetDefault(id);
                //    // allocationDefaultRule.SetGroupDetails(groupID, qty);
                //    return allocationDefaultRule;
                //}
                //else 
                if (_accounts.ContainsKey(id))
                {
                    AllocationDefault allocDefault = new AllocationDefault();
                    allocDefault.DefaultName = "";
                    //allocDefault.DefaultID=Allocation
                    AllocationLevelClass account = new AllocationLevelClass(string.Empty);
                    account.LevelnID = id;
                    account.Percentage = 100;
                    // account.AllocatedQty = qty;
                    allocDefault.DefaultAllocationLevelList = new AllocationLevelList();
                    allocDefault.DefaultAllocationLevelList.Add(account);
                    return allocDefault;
                }
                else
                {
                    return null;
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
            return null;
        }
 
        //public AccountingMethods AccountingMethods
        //{
        //    get { return _accountingMethods; }
        //    set { _accountingMethods = value; }
        //}
        //public DataSet GetCashAccountTablesFromDB()
        //{
        //    return _accountSubAccount;
        //}
        public DataSet GetLatestAvailableFxRatesLessThanToday()
        {
            return _lastAvailablefxratesLessThanToday;
        }

        public SecMasterRequestObj GetAlltradedSymbols()
        {
            return _clientsCommonDataManager.GetAllSymbols();
        }

        public bool IsSymbolTraded(long symbol_pk)
        {
            return _clientsCommonDataManager.IsSymbolTraded(symbol_pk);
        }

        public DataSet GetMasterCategorySubCategoryTables()
        {
            return _dsCashAccountTablesWithRelation;
        }

        public DataSet GetAllActivityTables()
        {
            return _dsCashActivityTables;
        }

        /// <summary>
        /// added by: Bharat Raturi, 03 jun 2014
        /// purpose: set only the client information when the new client is added
        /// </summary>
        internal void RefreshClientCache()
        {
            try
            {
                _companies = _keyValueDataManager.GetCompanies();
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

        public static void UpdateMasterFunds(Dictionary<int, string> _masterFundCollection)
        {
            _masterFunds = _masterFundCollection;
        }

        internal void SetLockedAccounts(List<int> accountsLockedbyUser)
        {
            try
            {
                _accountsLocked = accountsLockedbyUser;
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

        public string getSymbolConvertionForCounterPartyVenues(string counterpartyid, string vernueID)
        {
            string symbolConvertion = null;
            try
            {

                if (_counterPartyVenuesSymbolConventions.ContainsKey(counterpartyid))
                {
                    Dictionary<string, string> venuesSymbolConvertions = _counterPartyVenuesSymbolConventions[counterpartyid];
                    if (venuesSymbolConvertions != null && venuesSymbolConvertions.ContainsKey(vernueID))
                    {
                        symbolConvertion = venuesSymbolConvertions[vernueID];
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
            return symbolConvertion;
        }

        private static void FillSymbolConvertionForCounterPartyVenues(IList<string[]> lists)
        {
            foreach (string[] symbolConvertion in lists)
            {
                if (symbolConvertion[0] != null && symbolConvertion[0] != string.Empty && symbolConvertion[1] != null && symbolConvertion[1] != string.Empty && symbolConvertion[2] != null && symbolConvertion[2] != string.Empty)
                {
                    string counterPartyID = symbolConvertion[0];
                    Dictionary<string, string> venuesSymbolConvertions = null;
                    if (!_counterPartyVenuesSymbolConventions.ContainsKey(counterPartyID))
                    {
                        venuesSymbolConvertions = new Dictionary<string, string>();
                        _counterPartyVenuesSymbolConventions[counterPartyID] = venuesSymbolConvertions;
                    }
                    else
                    {
                        venuesSymbolConvertions = _counterPartyVenuesSymbolConventions[counterPartyID];
                    }

                    venuesSymbolConvertions[symbolConvertion[1]] = symbolConvertion[2];

                }
            }
        }

        //public static void UpdateCounterPartyCaches(int counterPartyID, string brokerName)
        //{
        //    try
        //    {
        //        // Updates Counterpartycache.
        //        if (_counterParties.ContainsKey(counterPartyID))
        //        {
        //            _counterParties[counterPartyID] = brokerName;
        //        }
        //        else
        //        {
        //            _counterParties.Add(counterPartyID, brokerName);
        //        }

        //        // Update counterpartyvenues.
        //       Dictionary<int, string> counterpartyVenues = KeyValueDataManager.GetInstance().UpdateCounterPartyVanueCacheForCounterParty(counterPartyID);
        //        foreach (int counterpartyVenueID in counterpartyVenues.Keys)
        //        {
        //            if (!_counterPartyVenue.ContainsKey(counterpartyVenueID))
        //            {
        //                _counterPartyVenue.Add(counterpartyVenueID, counterpartyVenues[counterpartyVenueID]);
        //            }
        //            else
        //            {
        //                _counterPartyVenue[counterpartyVenueID] = counterpartyVenues[counterpartyVenueID];
        //            }
        //        }

        //        //TODO: Update userwise Counterparty permission cache.
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public Dictionary<int, string> CompanyModules
        {
            get { return _companyModules; }
        }

        public static Dictionary<int, string> AlgoBrokersWithFullName
        {
            get { return _algoBrokersWithFullName; }
            set { _algoBrokersWithFullName = value; }
        }

        public static Dictionary<int, string> AlgoBrokersWithShortName
        {
            get { return _algoBrokersWithShortName; }
            set { _algoBrokersWithShortName = value; }
        }


        /// <summary>
        /// Get the Master Fund adn Account association and bind the values in list
        /// </summary>
        private void FillMasterFundsAndAccountsAssociationList()
        {
            try
            {
                _lstMasterFundsAndAccounts = new List<string>();
                foreach (KeyValuePair<int, List<int>> kvp in MasterFundSubAccountAssociation)
                {
                    string masterFund = MasterFunds.ContainsKey(kvp.Key) ? MasterFunds[kvp.Key] : String.Empty;
                    _lstMasterFundsAndAccounts.Add(masterFund);
                    foreach (int fundId in kvp.Value)
                    {
                        string acc = AccountsWithFullName.ContainsKey(fundId) ? AccountsWithFullName[fundId] : String.Empty;
                        _lstMasterFundsAndAccounts.Add("    " + acc);
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

        public static int GetTradingAccountForMasterFund(int masterFundId)
        {
            int tradingAccountID = -1;
            try
            {
                if (masterFundId > 0 && _masterFundTradingAccountMapping.ContainsKey(masterFundId))
                {
                    tradingAccountID = _masterFundTradingAccountMapping[masterFundId];
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
            return tradingAccountID;
        }


        public string GetInvalidFundsForSymbolLevel(string fundIds, Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            string invalidFundNames = "";
            try
            {
                invalidFundNames = _clientsCommonDataManager.GetInvalidFundsForSymbolLevel(fundIds, startDate, endDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return invalidFundNames;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _isNewOTCWorkflow;
        public bool IsNewOTCWorkflow
        {
            get { return _isNewOTCWorkflow; }
            set { _isNewOTCWorkflow = value; }
        }


        /// <summary>
        /// Set OTC Workflow Preference
        /// </summary>
        /// <param name="isNewOTCWorkflow"></param>
        public void SetOTCWorkflowPreference(bool isNewOTCWorkflow)
        {
            _isNewOTCWorkflow = isNewOTCWorkflow;

        }

        public static void FetchSecondaryCompanyMarketDataProvider()
        {
            _secondaryMarketDataProvider = _clientsCommonDataManager.GetSecondaryCompanyMarketDataProvider(_companyID);
        }
        public static void FetchCompanyMarketDataProvider()
        {
            _companyMarketDataProvider = _clientsCommonDataManager.GetCompanyMarketDataProvider(_companyID);
        }
        public static void FetchMarketDataBlockedInformation()
        {
            _isMarketDataBlocked = _clientsCommonDataManager.IsMarketDataBlocked(_companyID);
        }
        public static void FetchFactSetContractType()
        {
            _companyFactSetContractType = (FactSetContractType)_clientsCommonDataManager.GetFactSetContractType(_companyID);
        }

        /// <summary>
        /// Updates the list of attribute names by retrieving them from the key-value data manager.
        /// </summary>
        public static void UpdateAttributeLabels()
        {
            try
            {
                _attributeNames = _keyValueDataManager.GetAttributeNames();
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
