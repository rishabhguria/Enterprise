//using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.PositionManagement;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.GreenFieldModels;
//using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.CommonDataCache
{
    public class CachedDataManager : IDisposable
    {
        //
        // TODO: Add constructor logic here
        //
        private static CachedDataManager _cachedDataManager = null;
        private UDAData _udaDataNotKnown = new UDAData();
        static readonly object _locker = new object();
        private UserCachedData _userCachedData;
        DataSet _dsActivities = new DataSet();
        private IClientsCommonDataManager _clientsCommonDataManager;
        private IAllocationPrefDataManager _allocationPrefDataManager;

        private CachedDataManager()
        {
            try
            {
                _clientsCommonDataManager = WindsorContainerManager.Container.Resolve<IClientsCommonDataManager>();
                _allocationPrefDataManager = WindsorContainerManager.Container.Resolve<IAllocationPrefDataManager>();
                SetUnknownUDAData();
                //FillValueLists();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //This property is added for web application where we need to create user wise instance
        public UserCachedData UserCachedData
        {
            get { return _userCachedData; }
        }

        public static SecondaryMarketDataProvider SecondaryCompanyMarketDataProvider
        {
            get { return CachedData.SecondaryCompanyMarketDataProvider; }
        }
        public Dictionary<string, int> ExchangeIdentifiers
        {
            get { return CachedData.GetInstance().ExchangeIdentifiers; }
        }
        public static void UpdateMasterFunds(Dictionary<int, string> _masterFundCollection)
        {
            try
            {
                CachedData.UpdateMasterFunds(_masterFundCollection);
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
        /// UpdateAttributeLabels to update the trade attributes when published through trade service via common data service
        /// </summary>
        public static void UpdateAttributeLabels()
        {
            try
            {
                CachedData.UpdateAttributeLabels();
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

        public bool IsMarketDataPermissionEnabled
        {
            get { return CachedData.IsMarketDataPermissionEnabled; }
            set { CachedData.IsMarketDataPermissionEnabled = value; }
        }

        public void SetOTCWorkflowPreference(bool isOTCWorkflow)
        {
            try
            {
                CachedData.GetInstance().SetOTCWorkflowPreference(isOTCWorkflow);
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

        public bool IsSecurityValidationLoggingEnabled
        {
            get { return CachedData.IsSecurityValidationLoggingEnabled; }
        }
        public Dictionary<int, double> AuecMultipliers
        {
            get { return CachedData.GetInstance().AuecMultipliers; }
        }

        public Dictionary<int, decimal> AuecRoundLot
        {
            get { return CachedData.GetInstance().AuecRoundLot; }
        }

        public bool IsImportOverrideOnShortLocate
        {
            get { return CachedData.GetInstance().IsImportOverrideOnShortLocate; }
            set { CachedData.GetInstance().IsImportOverrideOnShortLocate = value; }
        }

        public Dictionary<string, string> DictTransactionType
        {
            get { return CachedData.GetInstance().DictTransactionType; }
        }

        public Dictionary<int, List<Account>> CompanyAccountsMapping
        {
            get { return CachedData.GetInstance().CompanyAccountsMapping; }
        }
        public static int GetTradingAccountForMasterFund(int masterFundId)
        {
            int TradingAccount = -1;
            try
            {
                TradingAccount = CachedData.GetTradingAccountForMasterFund(masterFundId);
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
            return TradingAccount;

        }
        public bool IsShowMasterFundonShortLocate
        {
            get { return CachedData.GetInstance().IsShowMasterFundonShortLocate; }
            set { CachedData.GetInstance().IsShowMasterFundonShortLocate = value; }
        }

        public Dictionary<int, List<int>> ReleaseWiseAccount
        {
            get { return CachedData.GetInstance().ReleaseWiseAccount; }
        }

        public DataTable GetAccountsForCompany()
        {
            DataTable dtAccount = new DataTable();
            dtAccount.Columns.Add("FundID", typeof(int));
            dtAccount.Columns.Add("FundName", typeof(string));
            // Added By : Manvendra Prajapati
            // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3741
            AccountCollection userAccounts = _userCachedData.UserAccounts;
            try
            {
                if (userAccounts != null)
                {
                    foreach (Prana.BusinessObjects.Account account in userAccounts)
                    {
                        if (account.AccountID != int.MinValue)
                        {
                            dtAccount.Rows.Add(account.AccountID, account.Name.ToString());
                        }
                    }
                }
                return dtAccount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int PermissibleQuickTTInstances
        {
            get { return CachedData.GetInstance().PermissibleQuickTTInstances; }
        }
        public string GetSymbolConvertionForCounterPartyVenues(string counterpartyid, string vernueID)
        {
            string symbolConvertion = null;
            try
            {
                symbolConvertion = CachedData.GetInstance().getSymbolConvertionForCounterPartyVenues(counterpartyid, vernueID);
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
            return symbolConvertion;
        }
        public static MarketDataProvider CompanyMarketDataProvider
        {
            get { return CachedData.CompanyMarketDataProvider; }
        }

        public static bool IsMarketDataBlocked
        {
            get { return CachedData.IsMarketDataBlocked; }
        }

        public static FactSetContractType CompanyFactSetContractType
        {
            get { return CachedData.CompanyFactSetContractType; }
        }

        public string GetInvalidFundsForSymbolLevel(string fundIds, DateTime? startDate, DateTime? endDate)
        {
            string invalidFundNames = "";
            try
            {
                invalidFundNames = CachedData.GetInstance().GetInvalidFundsForSymbolLevel(fundIds, startDate, endDate);
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
            return invalidFundNames;
        }
        public static void FetchCompanyMarketDataProvider()
        {
            try
            {
                CachedData.FetchCompanyMarketDataProvider();
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
        public static void FetchMarketDataBlockedInformation()
        {
            try
            {
                CachedData.FetchMarketDataBlockedInformation();
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

        public static void FetchFactSetContractType()
        {
            try
            {
                CachedData.FetchFactSetContractType();
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
        public static void FetchSecondaryCompanyMarketDataProvider()
        {
            try
            {
                CachedData.FetchSecondaryCompanyMarketDataProvider();
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
        public int EmailIntervalForStuckTrades
        {
            get { return CachedData.GetInstance().EmailIntervalForStuckTrades; }
            set { CachedData.GetInstance().EmailIntervalForStuckTrades = value; }
        }

        public Dictionary<int, string> CompanyModules
        {
            get { return CachedData.GetInstance().CompanyModules; }
        }

        public int WaitTimeToGetStuckTrade
        {
            get { return CachedData.GetInstance().WaitTimeToGetStuckTrade; }
            set { CachedData.GetInstance().WaitTimeToGetStuckTrade = value; }
        }

        private void SetUnknownUDAData()
        {
            try
            {
                _udaDataNotKnown.UDAAsset = "Undefined";
                _udaDataNotKnown.UDACountry = "Undefined";
                _udaDataNotKnown.UDASector = "Undefined";
                _udaDataNotKnown.UDASecurityType = "Undefined";
                _udaDataNotKnown.UDASubSector = "Undefined";
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static CachedDataManager GetInstance
        {
            get
            {
                if (_cachedDataManager == null)
                {
                    lock (_locker)
                    {
                        if (_cachedDataManager == null)
                        {
                            _cachedDataManager = new CachedDataManager();
                        }
                    }
                }
                return _cachedDataManager;
            }
        }

        /// <summary>
        /// Returns assetID based on assetText
        /// </summary>
        /// <param name="assetText">Text corresponding to which Id needs to be found</param>
        /// <returns>ID if text found, otherwise int.minvalue</returns>
        public int GetAssetID(String assetText)
        {
            Dictionary<int, string> dt = CachedData.GetInstance().Asset;

            try
            {
                foreach (KeyValuePair<int, string> de in dt)
                {
                    if (string.Compare(de.Value.Trim(), assetText, true) == 0)
                    {
                        return de.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return int.MinValue;
        }

        public string GetAssetText(int assetID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Asset;
                if (dt.ContainsKey(assetID))
                {
                    return dt[assetID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public string GetUnderLyingText(int ID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Underlying;
                if (dt.ContainsKey(ID))
                {
                    return dt[ID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public int GetlastPreferencedAccountID()
        {
            return CachedData.PreferencedAccountID;
        }

        public void SetPreferencedAccountID(int accountID)
        {
            CachedData.PreferencedAccountID = accountID;
        }


        /// <summary>
        /// Returns ID based on text
        /// </summary>
        /// <param name="underlyingText">Text corresponding to which Id need to be found</param>
        /// <returns>ID if text matches, otherwise int.minvalue</returns>
        public int GetUnderlyingID(string underlyingText)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Underlying;

                foreach (KeyValuePair<int, string> de in dt)
                {
                    if (string.Compare(de.Value.Trim(), underlyingText, true) == 0)
                    {
                        return de.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        public string GetExchangeText(int ID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Exchange;
                if (dt.ContainsKey(ID))
                {
                    return dt[ID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public int GetExchangeID(string exchangeName)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Exchange;

                foreach (KeyValuePair<int, string> de in dt)
                {
                    if (string.Compare(de.Value, exchangeName, true) == 0)
                    {
                        return de.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// Returns account name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetAccountText(int ID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().AccountsWithFullName;
                //Modified by: Bharat Raturi, 17 Feb 2014
                //purpose: get the short name of the account instead of the full name
                if (dt.ContainsKey(ID) && CachedData.GetInstance().Accounts.ContainsKey(ID))
                {
                    //return dt[ID];
                    return CachedData.GetInstance().Accounts[ID];
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
            return string.Empty;
        }
        /// <summary>
        /// sends account id form the account name(Short/Full)
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public int GetAccountID(string accountName)
        {
            try
            {
                Dictionary<int, string> dt = new Dictionary<int, string>();
                if (CachedData.GetInstance().Accounts.Values.Contains(accountName, StringComparer.InvariantCultureIgnoreCase))
                {
                    dt = CachedData.GetInstance().Accounts;
                }
                else if (CachedData.GetInstance().AccountsWithFullName.Values.Contains(accountName, StringComparer.InvariantCultureIgnoreCase))
                {
                    dt = CachedData.GetInstance().AccountsWithFullName;
                }
                else
                {
                    return int.MinValue;
                }
                foreach (KeyValuePair<int, string> kvp in dt)
                {
                    if (string.Compare(kvp.Value, accountName, true) == 0)
                    {
                        return kvp.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }
        public string GetStrategyText(int ID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Strategies;
                if (dt.ContainsKey(ID))
                {
                    return dt[ID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public Dictionary<int, string> GetAllStrategies()
        {
            return CachedData.GetInstance().Strategies;
        }

        /// <summary>
        /// Returns the Dictionary of all the accounts acquired by users and free
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetAccountUserLockDetail()
        {
            return CachedData.GetInstance().AccountUserLockDetail;
        }

        /// <summary>
        /// Set the Dictionary of all the accounts acquired by users and free
        /// </summary>
        /// <returns></returns>
        public void SetAccountUserLockDetail(Dictionary<int, int> accountUserLockDetail)
        {
            CachedData.GetInstance().AccountUserLockDetail = accountUserLockDetail;
        }


        /// <summary>
        /// Get the Locked Accounts for the user currently Loged in
        /// </summary>
        /// <param name="accountUserLockDetail"></param>
        public List<int> GetLockedAccounts()
        {
            return CachedData.GetInstance().AccountsLocked;
        }


        /// <summary>
        /// check if the Locked is locked and updates its timer to zero
        /// </summary>
        /// <param name="accountUserLockDetail"></param>
        public bool isAccountLocked(int accountID)
        {
            try
            {
                if (CachedData.GetInstance().IsAccountLockingEnabled)
                {
                    if (CachedData.GetInstance().AccountsLocked.Contains(accountID) && GetAccountsLockDuration().ContainsKey(accountID))
                    {
                        ResetAccountsLockTimer(accountID, 0);
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return false;
        }
        /// <summary>
        /// Release the account in account duration dictionary
        /// </summary>
        /// <param name="p"></param>
        public bool ReleaseAccount(int accountID)
        {
            try
            {
                if (CachedData.GetInstance().AccountsLocked.Contains(accountID))
                {
                    ResetAccountsLockTimer(accountID, int.MinValue);
                    return true;
                }

            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }
        /// <summary>

        /// Set the Locked Accounts for the user currently Logged in
        /// </summary>
        /// <param name="accountUserLockDetail"></param>
        public void SetLockedAccounts(List<int> accountsLockedbyUser)
        {
            CachedData.GetInstance().SetLockedAccounts(accountsLockedbyUser);
        }

        /// <summary>
        /// fetch the account dictionary with rgeir duration of last usage in minutes
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<int, int> GetAccountsLockDuration()
        {
            return CachedData.GetInstance().AccountsLockDuration;
        }

        /// <summary>
        /// Gets or sets the nav lock date.
        /// </summary>
        /// <value>
        /// The nav lock date.
        /// </value>
        public DateTime? NAVLockDate
        {
            get
            {
                return CachedData.GetInstance().NAVLockDate;
            }
            set
            {
                CachedData.GetInstance().NAVLockDate = value;
            }
        }

        public bool ValidateNAVLockDate(DateTime date)
        {
            DateTime? navLockDate = CachedData.GetInstance().NAVLockDate;
            if (navLockDate.HasValue && date.Date <= navLockDate.Value)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// fetch the account dictionary with rgeir duration of last usage in minutes
        /// </summary>
        /// <returns></returns>
        public void ResetAccountsLockTimer(int accountID, int value)
        {
            CachedData.GetInstance().ResetAccountsLockTimer(accountID, value);
        }


        public int GetStrategyID(string strategyName)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Strategies;

                foreach (KeyValuePair<int, string> kvp in dt)
                {
                    //start and end is trimed as strategy might contains multiple words
                    if (string.Compare(kvp.Value.Trim(), strategyName, true) == 0)
                    {
                        return kvp.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// Added by: Bharat raturi, 29 apr 2014
        /// Purpose: get the list of account IDs for the third party 
        /// </summary>
        /// <param name="thirdpartyID">ID of the third party</param>
        /// <returns>List of account IDs</returns>
        public List<int> GetThirdPartyAccounts(int thirdpartyID)
        {
            List<int> accountList = new List<int>();
            try
            {
                if (CachedData.GetInstance().ThirdPartyAccounts.ContainsKey(thirdpartyID))
                {
                    accountList = CachedData.GetInstance().ThirdPartyAccounts[thirdpartyID];
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
            return accountList;
        }

        /// <summary>
        /// Purpose: get the list of account IDs for the third parties 
        /// </summary>
        /// <param name="thirdpartyID">List of third party IDs</param>
        /// <returns>List of account IDs</returns>
        public List<int> GetThirdPartiesAccountsList(List<int> thirdpartyIDs)
        {
            List<int> accountList = new List<int>();
            try
            {
                foreach (int thirdPartyID in thirdpartyIDs)
                {
                    if (CachedData.GetInstance().ThirdPartyAccounts.ContainsKey(thirdPartyID))
                    {
                        foreach (int accountID in CachedData.GetInstance().ThirdPartyAccounts[thirdPartyID])
                        {
                            if (!accountList.Contains(accountID) && accountID > 0)
                                accountList.Add(accountID);
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
            return accountList;
        }

        /// <summary>
        /// GetThirdPartyID by AccountID
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public int GetThirdPartyIDOfAccount(int AccountID)
        {
            int thirdParty = 0;
            try
            {
                return CachedData.GetInstance().GetThirdPartyIDOfAccount(AccountID);
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

        /// <summary>
        /// GetCompanyID by AccountID
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public int GetCompanyIDOfAccountID(int AccountID)
        {
            int companyID = 0;
            try
            {
                Dictionary<int, List<Account>> companyAccountmapping = CachedData.GetInstance().CompanyAccountsMapping;

                foreach (int key in companyAccountmapping.Keys)
                {
                    foreach (Account account in companyAccountmapping[key])
                    {
                        if (account.AccountID == AccountID)
                        {
                            return key;
                        }
                    }
                }
                //need to check why not working below query
                // companyID =   companyAccountmapping.FirstOrDefault(x => x.Value.Select(y=>y.AccountID == AccountID).First()).Key;

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
            return companyID;
        }

        // Duplicate Property
        ///// <summary>
        ///// Added By: Bharat raturi, 29 apr 2014
        ///// purpose: get the dictionary of third partyID-third party names
        ///// </summary>
        ///// <returns>dictionary of third party</returns>
        //public Dictionary<int, string> GetThirdParties()
        //{
        //    return CachedData.GetInstance().ThirdParties;
        //}

        /// <summary>
        /// Author : Rajat
        /// 06 Oct 2006
        /// Gets the currency code by supplying currencyID.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <returns></returns>
        public string GetCurrencyText(int ID)
        {
            try
            {
                Dictionary<int, string> dictCurrency = CachedData.GetInstance().Currency;
                if (dictCurrency.ContainsKey(ID))
                {
                    return dictCurrency[ID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Author : Rajat 06 Oct 2006
        /// Gets the currency ID.
        /// </summary>
        /// <param name="currencyCode">The currency code.</param>
        /// <returns></returns>
        public int GetCurrencyID(string currencyCode)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Currency;
                foreach (KeyValuePair<int, string> de in dt)
                {
                    if (string.Compare(de.Value, currencyCode, true) == 0)
                    {
                        return de.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// GetCountryID by FactsetCode
        /// </summary>
        /// <param name="FactsetCountryCode"></param>
        /// <returns></returns>
        public int GetCountryIDFromFactsetCountryCode(string factsetCountryCode)
        {
            try
            {
                Dictionary<string, int> dictCountryID = CachedData.GetInstance().dictCountryWiseFactsetCode;
                {
                    if (dictCountryID.ContainsKey(factsetCountryCode))
                    {
                        return dictCountryID[factsetCountryCode];
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
            return -1;
        }

        /// <summary>
        /// GetCountryID by BloombergCode
        /// </summary>
        /// <param name="BloombergCountryCode"></param>
        /// <returns></returns>
        public int GetCountryIDFromBloombergCountryCode(string bloombergCountryCode)
        {
            try
            {
                Dictionary<string, int> dictCountryID = CachedData.GetInstance().dictCountryWiseBloombergCode;
                {
                    if (dictCountryID.ContainsKey(bloombergCountryCode))
                    {
                        return dictCountryID[bloombergCountryCode];
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
            return -1;
        }

        public string GetUserText(int ID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Users;
                if (dt.ContainsKey(ID))
                {
                    return dt[ID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public int GetUserID(String userText)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Users;
                foreach (KeyValuePair<int, string> de in dt)
                {
                    if (string.Compare(de.Value.Trim(), userText, true) == 0)
                    {
                        return de.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        public Dictionary<int, string> GetAllTradingAccount()
        {
            return CachedData.GetInstance().TradingAccounts;
        }

        public bool IsNewOTCWorkflow
        {
            get { return CachedData.GetInstance().IsNewOTCWorkflow; }
            set { CachedData.GetInstance().IsNewOTCWorkflow = value; }
        }

        public string GetTradingAccountText(int ID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().TradingAccounts;
                if (dt.ContainsKey(ID))
                {
                    return dt[ID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public int GetTradingAccountID(String tradingAccountText)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().TradingAccounts;
                foreach (KeyValuePair<int, string> de in dt)
                {
                    if (string.Compare(de.Value.Trim(), tradingAccountText, true) == 0)
                    {
                        return de.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        public int GetCounterPartyID(string counterPartyName)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().CounterParty;

                foreach (KeyValuePair<int, string> de in dt)
                {
                    if (string.Compare(de.Value, counterPartyName, true) == 0)
                    {
                        return de.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return int.MinValue;
        }
        public string GetCounterPartyText(int counterPartyID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().CounterParty;
                if (dt.ContainsKey(counterPartyID))
                {
                    return dt[counterPartyID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public string GetVenueText(int ID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Venues;
                if (dt.ContainsKey(ID))
                {
                    return dt[ID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public Dictionary<int, string> GetAllVenues()
        {
            return CachedData.GetInstance().Venues;
        }

        public string GetApplicationVersion()
        {
            return _userCachedData.ApplicationVersion;
        }

        public IList<MasterFundAccountDetails> GetUserCustomGroups()
        {
            return CachedData.GetInstance().CustomGroups;
        }

        public int GetVenueID(string venueName)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Venues;
                foreach (KeyValuePair<int, string> de in dt)
                {
                    if (String.Compare(de.Value.Trim(), venueName, true) == 0)
                    {
                        return de.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }
        public TranferTradeRules GetTransferTradeRules()
        {
            TranferTradeRules transferTradeRules = null;
            try
            {
                transferTradeRules = _userCachedData.TranferTradeRules;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return transferTradeRules;
        }

        public static void RefreshAttibutesCache(DataSet dsAttributes)
        {
            try
            {
                CachedData.GetInstance().SetAttributeNames(dsAttributes);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public DataSet GetAttributeNames()
        {
            try
            {
                return CachedData.GetInstance().AttributeNames;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataSet();
        }

        /// <summary>
        /// Gets the attribute keep records.
        /// </summary>
        /// <returns></returns>
        public bool[] GetAttributeKeepRecords()
        {
            bool[] keepRecords = new bool[45];
            try
            {
                DataSet dsAttributes = CachedData.GetInstance().AttributeNames;
                string attributName = string.Empty;
                DataTable dt = null;
                if (dsAttributes.Tables.Count > 0)
                {
                    dt = dsAttributes.Tables[0];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        keepRecords[i] = Convert.ToBoolean(dr[2].ToString());
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return keepRecords;
        }
       
        public string GetAttributeNameForValue(string attributeValue)
        {
            try
            {
                DataSet dsAttributes = CachedData.GetInstance().AttributeNames;
                string attributName = string.Empty;
                DataTable dt = null;
                if (dsAttributes.Tables.Count > 0)
                {
                    dt = dsAttributes.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[0].ToString().Equals(attributeValue))
                        {
                            attributName = dr[1].ToString();
                        }
                    }
                }
                if (attributName.Equals(string.Empty))
                {
                    return attributeValue;
                }
                else
                {
                    return attributName;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return attributeValue;
        }

        /// <summary>
        /// Gets the cash preference funds list.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Tuple<DateTime, bool>> GetCashPreferenceFundsDict()
        {
            return CachedData.GetInstance().CashPreferenceFunds;
        }

        public Dictionary<string, string> DictTransactionType_Acronym
        {
            get { return CachedData.GetInstance().dictTransactionType_Acronym; }
        }

        public string GetTransactionTypeAcronymByOrderSideTagValue(string orderSideTagValue)
        {
            try
            {
                string side = TagDatabaseManager.GetInstance.GetOrderSideText(orderSideTagValue);

                Dictionary<string, string> dictTranType = CachedData.GetInstance().dictTransactionType_Acronym;
                if (dictTranType.ContainsKey(side))
                {
                    return dictTranType[side];
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
            return string.Empty;
        }

        public static int GetSecurityValidationTimeOut()
        {
            return CachedData.GetInstance().SecurityValidationTimeOut;
        }

        public string GetPranaPreferenceByKey(string key)
        {
            try
            {
                if (CachedData.GetInstance().PranaPreferences.ContainsKey(key))
                {
                    return CachedData.GetInstance().PranaPreferences[key];
                }
                else
                    return string.Empty;
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

                return string.Empty;
            }
        }
        public string GetTransactionTypeNameByAcronym(string acronym)
        {
            try
            {
                Dictionary<string, string> dictTransactionType = CachedData.GetInstance().DictTransactionType;
                if (!String.IsNullOrEmpty(acronym) && dictTransactionType.ContainsKey(acronym))
                {
                    return dictTransactionType[acronym];
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
            return string.Empty;
        }

        /// <summary>
        /// Get Is NAV Locking Enabled
        /// Created By : omshiv , March 2014
        /// </summary>
        /// <returns></returns>
        public bool IsNAVLockingEnabled()
        {
            return CachedData.GetInstance().IsNAVLockingEnabled;
        }

        /// <summary>
        /// Get Is Account Locking Enabled
        /// Created By : omshiv , March 2014
        /// </summary>
        /// <returns></returns>
        public bool IsAccountLockingEnabled()
        {
            return CachedData.GetInstance().IsAccountLockingEnabled;
        }

        /// <summary>
        /// Gets the average price rounding.
        /// </summary>
        /// <returns></returns>
        public int GetAvgPriceRounding()
        {
            return CachedData.GetInstance().AvgPriceRounding;
        }

        public bool IsFeederAccountEnabled()
        {
            return CachedData.GetInstance().IsFeederAccountEnabled;
        }

        /// <summary>
        /// Is Window User Required
        /// </summary>
        /// <returns></returns>
        public bool IsWindowUserRequired()
        {
            return CachedData.GetInstance().IsWindowUserReq;
        }

        //added By: Bharat raturi, 23 apr 2014
        //purpose: check whether the many to many mapping is allowed
        /// <summary>
        /// Get whether the many to many master fund-account mapping is allowed
        /// </summary>
        /// <returns>true if many to many mapping is allowed</returns>
        public bool IsMasterFundManyToManyMappingAllowed()
        {
            return CachedData.GetInstance().IsAccountManyToManyMappingAllowed;
        }

        /// Get whether the  Is chkBox Show Master Fund on TT is allowed
        /// </summary>
        /// <returns>true if ShowMaster Fundon TT is allowed</returns>
        public bool IsShowMasterFundonTT()
        {
            return CachedData.GetInstance().IsShowMasterFundonTT;
        }

        public bool IsEquityOptionManualValidation()
        {
            return CachedData.GetInstance().IsEquityOptionManualValidation;
        }
        /// <summary>
        /// Determines whether [is collateral mark price validation].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is collateral mark price validation]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCollateralMarkPriceValidation()
        {
            return CachedData.GetInstance().IsCollateralMarkPriceValidation;
        }

        public bool IsShowTillSettlementDate()
        {
            return CachedData.GetInstance().IsShowTillSettlementDate;
        }
        /// <summary>
        /// Determines whether [is file pricing for touch].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is file pricing for touch]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFilePricingForTouch()
        {
            return CachedData.GetInstance().IsFilePricingForTouch;
        }

        /// <summary>
        /// Determine whether this instance will break orders for PTT and RB.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance will break orders for PTT and RB; otherwise, <c>false</c>.
        /// </returns>
        public bool IsBreakOrderPreference()
        {
            return CachedData.GetInstance().IsBreakOrderPreference;
        }

        /// Get whether the IsChkBox Show master Fund As Client is allowed
        /// </summary>
        /// <returns>true if many to Show master Fund is allowed</returns>
        public bool IsShowmasterFundAsClient()
        {
            return CachedData.GetInstance().IsShowmasterFundAsClient;
        }


        public bool IsPermanentDeletionEnabled()
        {
            return CachedData.GetInstance().IsPermanentDeletionEnabled;
        }

        //added By: Bharat raturi, 23 apr 2014
        //purpose: check whether the many to many mapping is allowed
        /// <summary>
        /// Get whether the many to many master strategy-strategy mapping is allowed
        /// </summary>
        /// <returns>true if many to many mapping is allowed</returns>
        public bool IsMasterStrategyManyToManyMappingAllowed()
        {
            return CachedData.GetInstance().IsStrategyManyToManyMappingAllowed;
        }

        /// <summary>
        /// Set the new settings in the cache
        /// </summary>
        /// <param name="isMasterFundmanyToMany">true, if many to many mapping for master fund-account is enabled</param>
        /// <param name="isStrategyManyToMany">true, if many to many mapping for master strategy-strategy is enabled</param>
        /// <param name="isNavLockEnabled">true, if Nav lock setup is enabled</param>
        public void UpdateandSavePranaPreference(bool? isMasterFundmanyToMany, bool? isStrategyManyToMany, bool? isNavLockEnabled, bool? isFeederAccountEnabled, int? pricingSource, bool? isAccountLockEnabled, bool? isPermanentDeletion, int? SettlementAutoCalculateProperty, bool? isZeroCommissionForSwaps, int? avgPriceRounding, bool? isShowmasterFundAsClient, bool? isShowMasterFundonTT, bool? isEquityOptionManualValidation, bool? isCollateralMarkPriceValidation, bool? isShowTillSettlementDate)
        {
            try
            {
                if (isNavLockEnabled != null)
                    CachedData.GetInstance().IsNAVLockingEnabled = isNavLockEnabled.Value;
                if (isAccountLockEnabled != null)
                    CachedData.GetInstance().IsAccountLockingEnabled = isAccountLockEnabled.Value;
                if (isMasterFundmanyToMany != null)
                    CachedData.GetInstance().IsAccountManyToManyMappingAllowed = isMasterFundmanyToMany.Value;
                if (isStrategyManyToMany != null)
                    CachedData.GetInstance().IsStrategyManyToManyMappingAllowed = isStrategyManyToMany.Value;
                //CachedData.GetInstance().IsStrategyManyToManyMappingAllowed = isStrategyManyToMany;
                if (isFeederAccountEnabled != null)
                    CachedData.GetInstance().IsFeederAccountEnabled = isFeederAccountEnabled.Value;
                if (pricingSource != null)
                {
                    CachedData.GetInstance().PricingSource = pricingSource.Value;
                    if (CachedData.GetInstance().PranaPreferences.ContainsKey(ApplicationConstants.CONST_PRICINGSOURCE))
                    {
                        CachedData.GetInstance().PranaPreferences[ApplicationConstants.CONST_PRICINGSOURCE] = pricingSource.Value.ToString();
                    }
                }
                if (isPermanentDeletion != null)
                    CachedData.GetInstance().IsPermanentDeletionEnabled = isPermanentDeletion.Value;
                if (isShowMasterFundonTT != null)
                    CachedData.GetInstance().IsShowMasterFundonTT = isShowMasterFundonTT.Value;
                if (isEquityOptionManualValidation != null)
                    CachedData.GetInstance().IsEquityOptionManualValidation = isEquityOptionManualValidation.Value;
                if (isCollateralMarkPriceValidation != null)
                    CachedData.GetInstance().IsCollateralMarkPriceValidation = isCollateralMarkPriceValidation.Value;
                if (isShowTillSettlementDate != null)
                    CachedData.GetInstance().IsShowTillSettlementDate = isShowTillSettlementDate.Value;
                if (isShowmasterFundAsClient != null)
                    CachedData.GetInstance().IsShowmasterFundAsClient = isShowmasterFundAsClient.Value;
                if (SettlementAutoCalculateProperty != null)
                {
                    CachedData.GetInstance().SettlementAutoCalculateField = SettlementAutoCalculateProperty.Value;
                    if (CachedData.GetInstance().PranaPreferences.ContainsKey(ApplicationConstants.CONST_SettlementAutoCalculateField))
                    {
                        CachedData.GetInstance().PranaPreferences[ApplicationConstants.CONST_SettlementAutoCalculateField] = SettlementAutoCalculateProperty.Value.ToString();
                    }
                }
                if (isZeroCommissionForSwaps != null)
                {
                    if (CachedData.GetInstance().PranaPreferences.ContainsKey(ApplicationConstants.CONST_ZEROCOMMISSIONFORSWAPS))
                    {
                        CachedData.GetInstance().PranaPreferences[ApplicationConstants.CONST_ZEROCOMMISSIONFORSWAPS] = (Convert.ToInt32(isZeroCommissionForSwaps)).ToString();
                    }
                }
                if (avgPriceRounding != null)
                    CachedData.GetInstance().AvgPriceRounding = avgPriceRounding.Value;
                _clientsCommonDataManager.SavePranaPreferencesinDB(isMasterFundmanyToMany, isStrategyManyToMany, isNavLockEnabled, isFeederAccountEnabled, pricingSource, isPermanentDeletion, SettlementAutoCalculateProperty, isZeroCommissionForSwaps, avgPriceRounding, isShowmasterFundAsClient, isShowMasterFundonTT, isEquityOptionManualValidation, isCollateralMarkPriceValidation, isShowTillSettlementDate);
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
        /// returns a dictionary which contains a master fund id and the corresponding sub account i.e account ids
        /// Author : Divya Bansal
        /// Dated : 23 May 2012
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<int>> GetMasterFundSubAccountAssociation()
        {
            try
            {
                return CachedData.GetInstance().MasterFundSubAccountAssociation;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new Dictionary<int, List<int>>();
        }

        /// <summary>
        /// returns a dictionary which contains a master fund id and the corresponding third party id
        /// Author : Aman Seth 
        /// Dated : 22 6 2015
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<int>> GetMasterFundsThridPartyAssociation()
        {
            try
            {
                return CachedData.GetInstance().DictMasterFundsThridParty;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new Dictionary<int, List<int>>();
        }
        /// <summary>
        /// returns a dictionary which contains a third party id and the corresponding master fund id
        /// Author : Aman Seth 
        /// Dated : 22 6 2015
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<int>> GetThridPartyMasterFundsAssociation()
        {
            try
            {
                return CachedData.GetInstance().DictThridPartyMasterFunds;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new Dictionary<int, List<int>>();
        }

        public int GetMasterFundIDFromAccountID(int accountID)
        {
            try
            {
                foreach (KeyValuePair<int, List<int>> kvp in GetMasterFundSubAccountAssociation())
                {
                    if (kvp.Value.Contains(accountID))
                    {
                        return kvp.Key;
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
            return -1;
        }
        /// <summary>
        /// Commented Rajat 09 Oct 2006, No Text for auec saved in keyvalue cache
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetAUECText(int ID)
        {
            try
            {
                Dictionary<string, int> dt = CachedData.GetInstance().ExchangeIdentifiers;
                foreach (KeyValuePair<string, int> de in dt)
                {
                    if (de.Value.Equals(ID))
                    {
                        return de.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;

        }
        /// <summary>
        /// Gets the AUECID.
        /// </summary>
        /// <param name="AssetID">The asset ID.</param>
        /// <param name="UnderlyingID">The underlying ID.</param>
        /// <param name="ExchangeID">The exchange ID.</param>
        /// <param name="CurrencyID">The currency ID.</param>
        /// <returns>AUEC Id</returns>
        public int GetAUECID(int AssetID, int UnderlyingID, int ExchangeID, int CurrencyID)
        {
            try
            {
                Dictionary<int, string> auecDict = CachedData.GetInstance().AUECs;
                foreach (KeyValuePair<int, string> de in auecDict)
                {
                    string[] auecInfo = de.Value.Split(',');
                    int dictAssetId = Convert.ToInt32(auecInfo[0]);
                    int dictUnderlyingId = Convert.ToInt32(auecInfo[1]);
                    int dictExchangeId = Convert.ToInt32(auecInfo[2]);

                    if (AssetID == dictAssetId && UnderlyingID == dictUnderlyingId && ExchangeID == dictExchangeId)
                    {
                        for (int i = 3; i < auecInfo.Length; i++)
                        {
                            if (CurrencyID == Convert.ToInt32(auecInfo[i]))
                            {
                                return de.Key;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// Gets the AUECID.
        /// </summary>
        /// <param name="AssetID">The asset ID.</param>
        /// <param name="UnderlyingID">The underlying ID.</param>
        /// <param name="ExchangeID">The exchange ID.</param>
        /// <returns>AUEC Id</returns>
        public int GetAUECID(int AssetID, int UnderlyingID, int ExchangeID)
        {
            try
            {
                Dictionary<int, string> auecDict = CachedData.GetInstance().AUECs;
                foreach (KeyValuePair<int, string> de in auecDict)
                {
                    string[] auecInfo = de.Value.Split(',');
                    int dictAssetId = Convert.ToInt32(auecInfo[0]);
                    int dictUnderlyingId = Convert.ToInt32(auecInfo[1]);
                    int dictExchangeId = Convert.ToInt32(auecInfo[2]);

                    if (AssetID == dictAssetId && UnderlyingID == dictUnderlyingId && ExchangeID == dictExchangeId)
                    {
                        return de.Key;
                    }
                }

            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        public double GetMultiplierByAUECID(int auecID)
        {
            try
            {
                Dictionary<int, double> dt = CachedData.GetInstance().AUECMultiplier;
                if (dt.ContainsKey(auecID))
                {
                    return dt[auecID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 1;
        }
        public DataSet GetLatestAvailableFxRatesLessThanToday()
        {
            try
            {
                return CachedData.GetInstance().GetLatestAvailableFxRatesLessThanToday();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataSet();
        }

        public int GetAssetIdByAUECId(int auecID)
        {
            try
            {
                Dictionary<int, int> AUECIdToAssetIdDict = CachedData.GetInstance().AUECIdToAssetDict;
                if (AUECIdToAssetIdDict.ContainsKey(auecID))
                {
                    return AUECIdToAssetIdDict[auecID];
                }

            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 1;
        }

        /// <summary>
        /// Gets the exchange id from AUEC id.
        /// Not needed now ... use GetUnderlyingExchangeCurrencyFromAUECID which will return Underlying, Exchange and Currency
        /// </summary>
        /// <param name="auecID">The auec ID.</param>
        /// <returns></returns>
        public int GetExchangeIdFromAUECId(int auecID)
        {
            try
            {
                Dictionary<int, string> auecDict = CachedData.GetInstance().AUECs;
                foreach (KeyValuePair<int, string> de in auecDict)
                {
                    string[] auecInfo = de.Value.Split(',');
                    int tempExchangeId = Convert.ToInt32(auecInfo[2]);
                    if (auecID == de.Key)
                    {
                        return tempExchangeId;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        public int GetCurrencyIdByAUECID(int auecID)
        {
            try
            {
                Dictionary<int, string> auecDict = CachedData.GetInstance().AUECs;
                foreach (KeyValuePair<int, string> de in auecDict)
                {
                    string[] auecInfo = de.Value.Split(',');
                    int tempCurrencyID = Convert.ToInt32(auecInfo[3]);
                    if (auecID == de.Key)
                    {
                        return tempCurrencyID;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }



        public int GetAUECIdByExchangeIdentifier(string exchangeIdentifier)
        {
            try
            {
                Dictionary<string, int> dt = CachedData.GetInstance().ExchangeIdentifiers;
                if (dt.ContainsKey(exchangeIdentifier))
                {
                    return dt[exchangeIdentifier];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// Use ForexConverter for fx related work
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        //public ConversionRate GetFXConversionRateBySymbolAndDate(string symbol, DateTime date)
        //{
        //    ConversionRate conversionRate = null;
        //Dictionary<string, SortedDictionary<DateTime, ConversionRate>> dt = CachedData.GetInstance().SymbolAndDateWiseFXRates;
        //    if (dt.ContainsKey(symbol))
        //    {
        //        SortedDictionary<DateTime, ConversionRate> dateWiseFXRates = dt[symbol];
        //        if (dateWiseFXRates.ContainsKey(date))
        //        {
        //            conversionRate = dateWiseFXRates[date];
        //        }
        //    }

        //    return conversionRate;
        //}

        //public int GetExchangeIdentifierByAUECID(int auecID)
        //{
        //    Dictionary<string, int> dt = CachedData.GetInstance().ExchangeIdentifiers;
        //    foreach (KeyValuePair<int, string> de in dt)
        //    {
        //        if (de.Value.ToUpper().Equals(counterPartyName.ToUpper()))
        //        {
        //            return de.Key;
        //        }
        //    }

        //    return int.MinValue;
        //}


        /// <summary>
        /// Returns Underlying and Exchange ID from AUEC ID 
        /// can be modified to return Exchange and Currency as well 
        /// </summary>
        /// <param name="AUECID"></param>
        /// <param name="UnderlyingId"></param>
        /// <param name="ExchangeID"></param>
        /// <param name="CurrencyID"></param>
        public void GetUnderlyingExchangeIDFromAUECID(int AUECID, ref Underlying UnderlyingId, ref int ExchangeID)
        {
            try
            {
                Dictionary<int, string> auecDict = CachedData.GetInstance().AUECs;
                if (auecDict.ContainsKey(AUECID))
                {
                    string[] auecInfo = auecDict[AUECID].Split(',');
                    UnderlyingId = (Underlying)Convert.ToInt32(auecInfo[1]);
                    ExchangeID = Convert.ToInt32(auecInfo[2]);
                    //CurrencyID = Convert.ToInt32(auecInfo[3]);
                }
                else
                {
                    UnderlyingId = Underlying.None;// (Underlying)Convert.ToInt32(auecInfo[1]);
                    ExchangeID = int.MinValue;
                    //CurrencyID = int.MinValue;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public int GetRoundDigitsFromAUECID(int AUECID)
        {
            try
            {
                Dictionary<int, int> auecDict = CachedData.GetInstance().RoundOffRules;

                if (auecDict.ContainsKey(AUECID))
                {
                    return auecDict[AUECID];
                }

            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        public void GetAssetIDFromAUECID(int AUECID, ref AssetCategory assetCategory)
        {
            try
            {
                Dictionary<int, string> auecDict = CachedData.GetInstance().AUECs;
                if (auecDict.ContainsKey(AUECID))
                {
                    string[] auecInfo = auecDict[AUECID].Split(',');
                    assetCategory = (AssetCategory)Convert.ToInt32(auecInfo[0]);
                }
                else
                {
                    assetCategory = AssetCategory.None;// (Underlying)Convert.ToInt32(auecInfo[1]);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Author : Rajat - 09 Oct 2006
        /// Assumption : For passed AssetID, ExchangeID, CurrencyID ==> UnderlyingId will be unique.
        /// Gets the underlying id from exchange id.
        /// Not needed now ... use GetUnderlyingExchangeCurrencyFromAUECID which will return Underlying, Exchange and Currency
        /// </summary>
        /// <param name="AssetID">The asset ID.</param>
        /// <param name="ExchangeID">The exchange ID.</param>
        /// <param name="CurrencyID">The currency ID.</param>
        /// <returns>Underlying Id</returns>
        public void GetUnderlyingAUECIdFromAssetExchangeCurrency(ref int AUECID, AssetCategory AssetID, ref Underlying UnderlyingId, int ExchangeID, int CurrencyID)
        {
            try
            {
                Dictionary<int, string> auecDict = CachedData.GetInstance().AUECs;
                foreach (KeyValuePair<int, string> de in auecDict)
                {
                    string[] auecInfo = de.Value.Split(',');
                    AssetCategory tempAssetId = (AssetCategory)Convert.ToInt32(auecInfo[0]);
                    Underlying tempUnderlyingId = (Underlying)Convert.ToInt32(auecInfo[1]);
                    int tempExchangeId = Convert.ToInt32(auecInfo[2]);

                    if (AssetID == tempAssetId && ExchangeID == tempExchangeId)
                    {
                        for (int i = 3; i < auecInfo.Length; i++)
                        {
                            if (CurrencyID == Convert.ToInt32(auecInfo[i]))
                            {
                                UnderlyingId = tempUnderlyingId;
                                AUECID = de.Key;
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public int GetUnderlyingID(int AUECID)
        {
            try
            {
                Dictionary<int, string> auecDict = CachedData.GetInstance().AUECs;
                foreach (KeyValuePair<int, string> de in auecDict)
                {
                    string[] auecInfo = de.Value.Split(',');
                    int tempUnderlyingId = Convert.ToInt32(auecInfo[1]);
                    if (AUECID == de.Key)
                    {
                        return tempUnderlyingId;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }
        public string GetCountryISOCode(int AUECID)
        {
            try
            {
                Dictionary<int, string> auecDict = CachedData.GetInstance().AUECs;
                foreach (KeyValuePair<int, string> de in auecDict)
                {
                    string[] auecInfo = de.Value.Split(',');
                    var tempCode = Convert.ToString(auecInfo[4]);
                    if (AUECID == de.Key)
                    {
                        return tempCode;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Check if the user is allowed to trade on the given AUEC and the selected CV is allowed to trade on the same AUEC
        /// </summary>
        /// <param name="AUECID">The AUEC ID on which the user wants to trade</param>
        /// <param name="counterPartyID">the counterparty to which the trade is to be sent</param>
        /// <param name="VenueID">they venue linked to this Counterparty where the trade is being sent</param>
        /// <param name="userID">user id of the user who wants to trade</param>
        /// <returns></returns>
        public bool CheckTradePermissionByCVandAUECID(int AUECID, int counterPartyID, int VenueID)
        {
            return _userCachedData.IsTradingPermitted(AUECID, counterPartyID, VenueID);
        }

        public byte[] GetFlagImage(int ID)
        {
            try
            {
                Dictionary<int, byte[]> dt = CachedData.GetInstance().Flags;
                if (dt.ContainsKey(ID))
                {
                    return dt[ID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
        public void StartCaching(Prana.BusinessObjects.CompanyUser loginUser)
        {
            try
            {
                _userCachedData = new UserCachedData(loginUser.CompanyUserID);
                _userCachedData.StartCaching(loginUser.CompanyID);
                //commnted by: OMshiv , NOv 2103, Removed UDA symbol data cache
                // RefershUDAData();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public DataTable GetOrderSides(int assetID)
        {
            try
            {
                if (_userCachedData.PermissionSideBasedOnAsset.ContainsKey(assetID))
                {
                    return _userCachedData.PermissionSideBasedOnAsset[assetID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return GetBasicOrderSide();
        }
        public DataTable GetOrderSides()
        {
            try
            {
                return _userCachedData.PermissionOrderSide;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataTable();
        }
        public DataTable GetOrderTypes()
        {
            try
            {
                return _userCachedData.PermissionOrderType;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return GetBasicOrderType();
        }

        public DataTable GetExecutionInstruction()
        {
            try
            {
                return _userCachedData.PermissionExecutionInst;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataTable();
        }

        public DataTable GetHandlingInstruction()
        {
            try
            {
                return _userCachedData.PermissionHandlingInst;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataTable();
        }

        public DataTable GetTIFS()
        {
            try
            {
                return _userCachedData.PermissionTIF;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataTable();
        }

        public void GetListedExchange(int AssetID, ref string listedExchange)
        {
            try
            {
                foreach (DataRow existingRow in CachedData.GetInstance().AssetsExchangeIdentifiers.Rows)
                {
                    if (existingRow["AssetID"].ToString() == AssetID.ToString())
                    {
                        listedExchange = existingRow["ExchangeIdentifier"].ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static DataTable GetBasicOrderSide()
        {
            try
            {
                return TagDatabase.GetInstance().BasicOrderSide;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataTable();
        }

        public DataTable GetCompany()
        {
            try
            {
                return CachedData.GetInstance().Company;
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
            return new DataTable();
        }

        public Dictionary<int, List<int>> GetAllCompanyAccounts()
        {
            return _userCachedData.CompanyAccounts;
        }

        /// <summary>
        /// Added By: Bharat Raturi, 27 may 2014
        /// purpose: Get the More frequently used details refreshed in the cache
        /// </summary>
        public void RefreshFrequentlyUsedData()
        {
            try
            {
                CachedData.GetInstance().RefreshFrequentlyUsedData();
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
        /// Get User Permitted Company List
        /// Omshiv, March 2014
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, String> GetUserPermittedCompanyList()
        {
            try
            {
                return CachedData.GetInstance().Companies;
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
        /// <summary>
        /// Narendra Jangir, 2014 Mar 31.
        /// Get Company text for companyID
        /// TODO: Need to refactor metrhods GetCompany(), GetCompanyBaseCurrencyID() for multiple companies
        /// </summary>
        /// <returns></returns>
        public static string GetCompanyText(int CompanyID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Companies;
                if (dt.ContainsKey(CompanyID))
                {
                    return dt[CompanyID];
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
            return string.Empty;
        }

        public string GetPranaOrderSideTagValue(string OrderSideTagValue, string OpenClose)
        {
            string retval = OrderSideTagValue;
            try
            {
                switch (OrderSideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                        if (OpenClose.Equals(FIXConstants.Open))
                        {
                            retval = FIXConstants.SIDE_Buy_Open;
                        }
                        else if (OpenClose.Equals(FIXConstants.Close))
                        {
                            retval = FIXConstants.SIDE_Buy_Closed;
                        }
                        break;

                    case FIXConstants.SIDE_Sell:
                        if (OpenClose.Equals(FIXConstants.Open))
                        {
                            retval = FIXConstants.SIDE_Sell_Open;
                        }
                        else if (OpenClose.Equals(FIXConstants.Close))
                        {
                            retval = FIXConstants.SIDE_Sell_Closed;
                        }
                        break;
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
            return retval;
        }

        public static DataTable GetBasicOrderType()
        {
            DataTable ordersideList = new DataTable();
            try
            {
                ordersideList.Columns.Add(Prana.Global.OrderFields.PROPERTY_ORDER_TYPETAGVALUE);
                ordersideList.Columns.Add(Prana.Global.OrderFields.PROPERTY_ORDER_TYPE);
                ordersideList.Rows.Add(new object[] { FIXConstants.ORDTYPE_Market, TagDatabaseManager.GetInstance.GetOrderTypeText(FIXConstants.ORDTYPE_Market) });
                ordersideList.Rows.Add(new object[] { FIXConstants.ORDTYPE_Limit, TagDatabaseManager.GetInstance.GetOrderTypeText(FIXConstants.ORDTYPE_Limit) });
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
            return ordersideList;
        }

        public static DataTable GetBasicHandlingInst()
        {
            DataTable handlingInst = new DataTable();
            try
            {
                handlingInst.Columns.Add(Prana.Global.OrderFields.PROPERTY_HANDLING_INST_TagValue);
                handlingInst.Columns.Add(Prana.Global.OrderFields.PROPERTY_HANDLING_INST);

                handlingInst.Rows.Add(new object[] { Prana.Global.ApplicationConstants.C_COMBO_SELECT, Prana.Global.ApplicationConstants.C_COMBO_SELECT });
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
            return handlingInst;
        }
        public static DataTable GetBasicExecutionInst()
        {
            DataTable executioninst = new DataTable();
            try
            {
                executioninst.Columns.Add(Prana.Global.OrderFields.PROPERTY_EXECUTION_INST);
                executioninst.Columns.Add(Prana.Global.OrderFields.CAPTION_EXECUTION_INST);

                executioninst.Rows.Add(new object[] { Prana.Global.ApplicationConstants.C_COMBO_SELECT, Prana.Global.ApplicationConstants.C_COMBO_SELECT });
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
            return executioninst;
        }

        public static DataTable GetBasicTIFs()
        {
            DataTable tifs = new DataTable();
            try
            {
                tifs.Columns.Add(Prana.Global.OrderFields.PROPERTY_TIF_TAGVALUE);
                tifs.Columns.Add(Prana.Global.OrderFields.PROPERTY_TIF);

                tifs.Rows.Add(new object[] { Prana.Global.ApplicationConstants.C_COMBO_SELECT, Prana.Global.ApplicationConstants.C_COMBO_SELECT });
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
            return tifs;
        }

        /// <summary>
        /// Gets all user account list.
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllUserAccountList()
        {
            List<int> accountIds = new List<int>();
            try
            {
                foreach (Account userAccount in _userCachedData.UserAccounts)
                {
                    accountIds.Add(userAccount.AccountID);
                }
                //removing unallocated id because in expnl its id is -1
                accountIds.Remove(Int32.MinValue);
                accountIds.Add(-1);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return accountIds;
        }

        public AccountCollection GetUserAccounts()
        {
            try
            {
                return _userCachedData.UserAccounts;
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
            return new AccountCollection();
        }

        public Dictionary<int, List<int>> CompanyThirdPartyAccounts()
        {
            try
            {
                return CachedData.GetInstance().CompanyThirdPartyAccounts;
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
            return new Dictionary<int, List<int>>();
        }

        public Dictionary<int, int> GetAccountWiseExecutingBrokerMapping()
        {
            try
            {
                return CachedData.GetInstance().AccountWiseExecutingBrokerMapping;
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
            return new Dictionary<int, int>();
        }

        /// <summary>
        /// This Method is to get mapped broker id with account id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public int GetCounterPartyByAccountId(int accountId)
        {
            try
            {
                if (GetAccountWiseExecutingBrokerMapping().ContainsKey(accountId))
                {
                    return GetAccountWiseExecutingBrokerMapping()[accountId];
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
            return int.MinValue;
        }

        public StrategyCollection GetUserStrategies()
        {
            try
            {
                return _userCachedData.UserStrategies;
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
            return new StrategyCollection();
        }
        public StrategyCollection GetUserPermittedStrategies()
        {
            try
            {
                Dictionary<int, string> StartegyList = CachedDataManager.GetInstance.GetUserStrategiesDictionary();
                if (StartegyList != null && StartegyList.Count > 0 && StartegyList.Keys.Max() <= 0)
                {
                    return new StrategyCollection();
                }
                else
                {
                    return _userCachedData.UserStrategies;
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
            return new StrategyCollection();
        }
        public TradingAccountCollection GetUserTradingAccounts()
        {
            try
            {
                if (_userCachedData != null)
                    return _userCachedData.UserTradingAccounts;
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
            return new TradingAccountCollection();
        }
        public DataTable GetUserComplianceBorrowers()
        {
            try
            {
                return _userCachedData.UserComplianceBorrowers;
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
            return new DataTable();
        }


        public void SetCurrentTimeZone()
        {
            try
            {
                CachedData.GetInstance().GetTimeZoneByUserID();
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
        public Prana.BusinessObjects.TimeZone GetAUECTimeZone(int auecID)
        {
            try
            {
                Dictionary<int, Prana.BusinessObjects.TimeZone> dt = CachedData.GetInstance().AUECIDTimeZones;
                if (dt.ContainsKey(auecID))
                {
                    return dt[auecID];
                }
                else
                {
                    return CachedData.GetInstance().CurrentTimeZone;
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
            return CachedData.GetInstance().CurrentTimeZone;
        }

        public static bool CheckPermissionAllAccountsAndTradingAccountsFromCache()
        {
            bool resultBool = false;
            try
            {
                if (GetInstance.GetAllAccountsCount() == GetInstance.GetAllAccountIDsForUser().Count && GetInstance.GetAllTradingAccount().Count == GetInstance.GetUserTradingAccounts().Count)
                {
                    resultBool = true;
                }
                else
                {
                    resultBool = false;
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
            return resultBool;
        }

        public Dictionary<int, Prana.BusinessObjects.TimeZone> GetAllAUECTimeZones()
        {
            return CachedData.GetInstance().AUECIDTimeZones;
        }

        public bool IsLongSide(string orderSideTagValue)
        {
            try
            {
                if (orderSideTagValue == FIXConstants.SIDE_Buy ||
                        orderSideTagValue == FIXConstants.SIDE_Buy_Closed ||
                        orderSideTagValue == FIXConstants.SIDE_Buy_Open ||
                        orderSideTagValue == FIXConstants.SIDE_BuyMinus ||
                        orderSideTagValue == FIXConstants.SIDE_Cross ||
                        orderSideTagValue == FIXConstants.SIDE_Buy_Cover ||
                        orderSideTagValue == FIXConstants.SIDE_CrossShort)
                //orderSideTagValue == FIXConstants.SIDE_CrossShortExempt ||
                //orderSideTagValue == FIXConstants.SIDE_Opposite)
                {
                    return true;
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
            return false;
        }

        public int GetAUECSettlementPeriod(int auecID, string sideTagValue)
        {
            try
            {
                StructSettlementPeriodSidewise structSettlementPeriodSidewise;
                Dictionary<int, StructSettlementPeriodSidewise> dt = CachedData.GetInstance().AUECIDSettlementPeriods;
                if (dt.ContainsKey(auecID))
                {
                    structSettlementPeriodSidewise = dt[auecID];

                    if (IsLongSide(sideTagValue))
                    {
                        return structSettlementPeriodSidewise.Long;
                    }
                    else
                    {
                        return structSettlementPeriodSidewise.Short;

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
            return Prana.Global.ApplicationConstants.NO_OF_SETTLEMENTDAYS;
        }


        #region GetAll_Assets_UnderLyings_Exchanges_Currencies
        public Dictionary<int, string> GetAllAssets()
        {
            Dictionary<int, string> dictAssets = null;
            try
            {
                dictAssets = CachedData.GetInstance().Asset;

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
            return dictAssets;
        }
        public Dictionary<int, string> GetAllUnderlyings()
        {
            Dictionary<int, string> dictUnderlyings = null;
            try
            {
                dictUnderlyings = CachedData.GetInstance().Underlying;
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
            return dictUnderlyings;
        }
        public Dictionary<int, string> GetAllExchanges()
        {
            Dictionary<int, string> dictExchanges = null;
            try
            {
                dictExchanges = CachedData.GetInstance().Exchange;
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
            return dictExchanges;
        }
        public Dictionary<int, string> GetAllCurrencies()
        {
            Dictionary<int, string> dictCurrencies = null;
            try
            {
                dictCurrencies = CachedData.GetInstance().Currency;
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
            return dictCurrencies;
        }
        public Dictionary<int, string> GetAllCounterParties()
        {
            Dictionary<int, string> dictCps = null;
            try
            {
                dictCps = CachedData.GetInstance().CounterParty;
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
            return dictCps;
        }
        #endregion

        //commnted by: OMshiv , NOv 2103, Removed UDA  data cache
        //#region GetAll_UDAAssets_UDASectors_UDA SubSectors_UDASecurityTypes_UDACountries
        //public Dictionary<int, string> GetAllUDAAssets()
        //{
        //    Dictionary<int, string> dictUDAAssets = CachedData.GetInstance().UDAAssets;
        //    return dictUDAAssets;
        //}
        //public Dictionary<int, string> GetAllUDASectors()
        //{
        //    Dictionary<int, string> dictUDASectors = CachedData.GetInstance().UDASectors;
        //    return dictUDASectors;
        //}
        //public Dictionary<int, string> GetAllUDASubSectors()
        //{
        //    Dictionary<int, string> dictUDASubSectors = CachedData.GetInstance().UDASubSectors;
        //    return dictUDASubSectors;
        //}
        //public Dictionary<int, string> GetAllUDASecurityTypes()
        //{
        //    Dictionary<int, string> dictUDASecurityTypes = CachedData.GetInstance().UDASecurityTypes;
        //    return dictUDASecurityTypes;
        //}

        //public Dictionary<int, string> GetAllUDACountries()
        //{
        //    Dictionary<int, string> dictUDACountries = CachedData.GetInstance().UDACountries;
        //    return dictUDACountries;
        //}

        //#endregion


        public Dictionary<int, string> GetAllClosingAssets()
        {
            try
            {
                return CachedData.GetInstance().ClosingAssets;
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
            return new Dictionary<int, string>();
        }

        public Dictionary<int, string> GetAccounts()
        {
            try
            {
                return CachedData.GetInstance().Accounts;
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
            return new Dictionary<int, string>();
        }

        public Dictionary<string, string> GetPranaImportTags()
        {
            try
            {
                return CachedData.GetInstance().PranaImportTags;
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
            return new Dictionary<string, string>();
        }

        public int GetAllAccountsCount()
        {
            return CachedData.GetInstance().Accounts.Count;
        }

        public Dictionary<int, string> GetAccountsWithFullName()
        {
            try
            {
                return CachedData.GetInstance().AccountsWithFullName;
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
            return new Dictionary<int, string>();
        }
        public Dictionary<int, string> GetAllUsersName()
        {
            try
            {
                return CachedData.GetInstance().Users;
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
            return new Dictionary<int, string>();
        }
        public string GetAccount(int accountID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().AccountsWithFullName;
                if (dt.ContainsKey(accountID))
                {
                    return dt[accountID];
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
            return string.Empty;
        }
        public string GetMasterFund(int masterFundID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().MasterFunds;
                if (dt.ContainsKey(masterFundID))
                {
                    return dt[masterFundID];
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
            return string.Empty;
        }

        public int GetMasterFundID(string masterFundName)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().MasterFunds;

                if (dt.Values.Contains(masterFundName, StringComparer.InvariantCultureIgnoreCase))
                {
                    foreach (KeyValuePair<int, string> kvp in dt)
                    {
                        if (dt[kvp.Key].Equals(masterFundName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return kvp.Key;
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
            return int.MinValue;
        }

        public Dictionary<int, string> GetAllMasterFunds()
        {
            try
            {
                return CachedData.GetInstance().MasterFunds;
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
            return new Dictionary<int, string>();
        }

        /// <summary>
        /// Gets the user master funds.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetUserMasterFunds()
        {
            try
            {
                return _userCachedData.UserMasterFunds;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return new Dictionary<int, string>();
        }

        /// <summary>
        /// returns Account group value of accountGroupID
        /// </summary>
        /// <param name="accountGroupID"></param>
        /// <returns></returns>
        public string GetAccountGroups(int accountGroupID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().AccountGroups;
                if (dt.ContainsKey(accountGroupID))
                {
                    return dt[accountGroupID];
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
            return string.Empty;

        }

        /// <summary>
        /// returns dictionary of all account Groups
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllAccountGroups()
        {
            Dictionary<int, string> dictAccountGroups = null;
            try
            {
                dictAccountGroups = CachedData.GetInstance().AccountGroups;

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
            return dictAccountGroups;
        }

        public List<string> GetAllMastersFundsWithFunds()
        {
            return CachedData.GetInstance().ListMasterFundsAndFunds;
        }

        /// <summary>
        /// returns counter party venue name
        /// </summary>
        /// <param name="counterPartyVenueID"></param>
        /// <returns></returns>
        public string GetCounterPartyVenueText(int counterPartyVenueID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().CounterPartyVenue;
                if (dt.ContainsKey(counterPartyVenueID))
                {
                    return dt[counterPartyVenueID];
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
            return string.Empty;
        }

        /// <summary>
        /// Gets the exercise assign venue.
        /// </summary>
        /// <returns></returns>
        public Venue GetExerciseAssignVenue()
        {
            try
            {
                return CachedData.GetInstance().ExerciseAssignVenue;
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


        public List<GenericNameID> GetAllPrimeBrokers()
        {
            return CachedData.GetInstance().PrimeBrokers;
        }

        public Dictionary<int, string> GetAllThirdParties()
        {
            return CachedData.GetInstance().ThirdParties;
        }

        public Dictionary<int, string> GetAllThirdPartiesWithShortName()
        {
            return CachedData.GetInstance().ThirdPartiesWithShortName;
        }

        public string GetMasterStrategy(int masterStrategyID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().MasterStrategy;
                if (dt.ContainsKey(masterStrategyID))
                {
                    return dt[masterStrategyID];
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
            return string.Empty;
        }

        public static bool HasDataSource(int AccountID)
        {
            try
            {
                if (CachedData.GetInstance().DataSources.ContainsKey(AccountID))
                {
                    return true;
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
            return false;
        }


        public static Prana.BusinessObjects.PositionManagement.ThirdPartyNameID GetDatasource(int AccountID)
        {
            try
            {
                Dictionary<int, Prana.BusinessObjects.PositionManagement.ThirdPartyNameID> dataSources = CachedData.GetInstance().DataSources;
                if (dataSources.ContainsKey(AccountID))
                {
                    return dataSources[AccountID];
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

        public void SetCompanyUser(CompanyUser companyUser)
        {
            try
            {
                if (companyUser != null)
                {
                    if (_loggedInUser == null)
                    {
                        _loggedInUser = new CompanyUser();
                    }
                    _loggedInUser.CompanyID = companyUser.CompanyID;
                    _loggedInUser.CompanyName = companyUser.CompanyName;
                    _loggedInUser.CompanyUserID = companyUser.CompanyUserID;
                    _loggedInUser.EMail = companyUser.EMail;
                    _loggedInUser.Fax = companyUser.Fax;
                    _loggedInUser.FirstName = companyUser.FirstName;
                    _loggedInUser.LastName = companyUser.LastName;
                    _loggedInUser.LoginID = companyUser.LoginID;
                    _loggedInUser.MailingAddress = companyUser.MailingAddress;
                    _loggedInUser.ShortName = companyUser.ShortName;
                    _loggedInUser.TelephoneHome = companyUser.TelephoneHome;
                    _loggedInUser.TelephoneMobile = companyUser.TelephoneMobile;
                    _loggedInUser.TelephoneWork = companyUser.TelephoneWork;
                    _loggedInUser.Title = companyUser.Title;
                    _loggedInUser.TradingAccounts = companyUser.TradingAccounts;
                    _loggedInUser.MarketDataTypes = companyUser.MarketDataTypes;
                    _loggedInUser.FactSetUsernameAndSerialNumber = companyUser.FactSetUsernameAndSerialNumber;
                    _loggedInUser.IsFactSetSupportUser = companyUser.IsFactSetSupportUser;
                    _loggedInUser.MarketDataAccessIPAddresses = companyUser.MarketDataAccessIPAddresses;
                    _loggedInUser.ActivUsername = companyUser.ActivUsername;
                    _loggedInUser.ActivPassword = companyUser.ActivPassword;
                    _loggedInUser.SapiUsername = companyUser.SapiUsername;

                    StartCaching(_loggedInUser);
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

        private CompanyUser _loggedInUser;

        public CompanyUser LoggedInUser
        {
            get { return _loggedInUser; }
        }

        public List<AllocationDefault> GetAllocationDefaults()
        {
            try
            {
                return _userCachedData.AllocationDefaultCollection;
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
            return new List<AllocationDefault>();
        }

        public void SetAllocationDefaults(AllocationDefaultCollection allocationDefaults)
        {
            try
            {
                _allocationPrefDataManager.DeleteDefaults(allocationDefaults.GetRemovedDefaults());
                List<AllocationDefault> listDefaults = allocationDefaults.GetDefaults();
                _userCachedData.SetDefaults(listDefaults);
                _allocationPrefDataManager.SaveDefaults(listDefaults);
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
        /// refreshes allocation default cache. syncs it with DB
        /// </summary>
        public void RefreshAllocationDefaults()
        {
            try
            {
                List<AllocationDefault> lstDefault = _allocationPrefDataManager.GetAccountDefaults();
                _userCachedData.SetDefaults(lstDefault);
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

        public DataTable GetAccountsAndAllocationRules()
        {
            try
            {
                return _userCachedData.GetAccountsAndAllocationRules();
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
            return new DataTable();
        }

        public AllocationDefault GetAllocationDefault(int id)
        {
            try
            {
                return CachedData.GetInstance().GetAllocationDefault(id);
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

        public AllocationDefault GetAllocationDefaultForAllocation(int id)
        {
            try
            {
                AllocationDefault allocdefault = CachedData.GetInstance().GetAllocationDefault(id);
                if (allocdefault != null)
                {
                    return (AllocationDefault)allocdefault.Clone();
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


        //public DataTable GetAccountingMethodsTable()
        //{
        //    return ((AccountingMethods)CachedData.GetInstance().AccountingMethods.Clone()).AccountingMethodsTable;
        //}

        //public void SetAccountingMethods(AccountingMethods accountingMethods)
        //{
        //    //CachedData.GetInstance().AccountingMethods = accountingMethods;
        //    ClosingPrefDataManager.SaveAccountingMethods(accountingMethods.AccountingMethodsTable);
        //    CachedData.GetInstance().AccountingMethods.Update(accountingMethods.AccountingMethodsTable);
        //}

        public int GetAUECCount()
        {
            try
            {
                return CachedData.AUECCount;
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
            return 0;
        }

        #region Account SubAccount
        public void RefreshAccountData()
        {
            try
            {
                CachedData.SetAccountsAndActivityData();
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

        public static string GetCashAccountName(int accountID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().Accounts;
                if (dt.ContainsKey(accountID))
                {
                    return dt[accountID];
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
            return string.Empty;
        }
        public static string GetSubAccountName(int subAccountID)
        {
            try
            {
                Dictionary<int, string> dt = CachedData.GetInstance().SubCashAccounts;
                if (dt.ContainsKey(subAccountID))
                {
                    return dt[subAccountID];
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
            return string.Empty;
        }
        public static int GetSubAccountIDByAcronym(string subAccountAcronym)
        {
            try
            {
                Dictionary<string, int> dt = CachedData.GetInstance().SubCashAccounts_Acronym;
                if (dt.ContainsKey(subAccountAcronym))
                {
                    return dt[subAccountAcronym];
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
            return int.MinValue;
        }
        public static int GetActivityTypeID(string activity)
        {
            try
            {
                Dictionary<string, int> dt = CachedData.GetInstance().ActivityType;

                if (dt.ContainsKey(activity))
                {
                    return dt[activity];
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
            return int.MinValue;
        }

        #region commented
        //public static int GetCashTransactionTypeIDForName(string cashTransactionType)
        //{
        //    try
        //    {
        //        Dictionary<string, int> dt = CachedData.GetInstance().CashTransactionTypeWithName;

        //        if (dt.ContainsKey(cashTransactionType))
        //        {
        //            return dt[cashTransactionType];
        //        }
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
        //    return int.MinValue;
        //}

        //public static int GetCashTransactionTypeID(string cashTransactionType)
        //{
        //    try
        //    {
        //        Dictionary<string, int> dt = CachedData.GetInstance().CashTransactionTypeWithAcronym;

        //        if (dt.ContainsKey(cashTransactionType))
        //        {
        //            return dt[cashTransactionType];
        //        }
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
        //    return int.MinValue;
        //}
        #endregion

        public static int GetBalanceTypeID(string activity)
        {
            try
            {
                Dictionary<string, int> dt = CachedData.GetInstance().BalanceTypeID;

                if (dt.ContainsKey(activity))
                {
                    return dt[activity];
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
            return int.MinValue;
        }
        public static string GetActivityText(int activityID)
        {
            try
            {
                Dictionary<string, int> dt = CachedData.GetInstance().ActivityType;
                string activity = GeneralUtilities.FindKeyByValue(dt, activityID);
                if (!string.IsNullOrEmpty(activity))
                {
                    //Activities activityAcr = (Activities)(Enum.Parse(typeof(Activities), activity));
                    //return activityAcr;
                    return activity;
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
            //return (Activities)Enum.Parse(typeof(Activities), string.Empty);
            return string.Empty;
        }



        //public static Dictionary<string, int> GetAllActivity()
        //{
        //    try
        //    {
        //        return CachedData.GetInstance().ActivityType;
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
        //    return null;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetActivityJournalMapping()
        {
            try
            {
                return CachedData.GetInstance().ActivityJournalMapping;
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

        #region commented
        //public static DataTable GetCashTRNActivityMapping()
        //{
        //    try
        //    {
        //        return CachedData.GetInstance().CashTRNActivityMapping;
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
        //    return null;
        //}
        #endregion

        public static string GetAccountTypeAcronymBySubAccountID(int subAccountID)
        {
            try
            {
                if (CachedData.GetInstance().SubAccounts_AccountType.ContainsKey(subAccountID))
                    return CachedData.GetInstance().SubAccounts_AccountType[subAccountID];
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
        public static Dictionary<string, int> GetTransactionEntrySideMultiplierBySubAccountID(int subAccountID)
        {
            try
            {
                if (CachedData.GetInstance().SubAccounts_Side_Multipier.ContainsKey(subAccountID))
                    return CachedData.GetInstance().SubAccounts_Side_Multipier[subAccountID];
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

        //public  DataSet GetCashAccountTablesFromDB()
        //{
        //    try
        //    {
        //        return CachedData.GetInstance().GetCashAccountTablesFromDB();
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
        //    return null;
        //}

        public DataSet GetMasterCategorySubCategoryTables()
        {
            try
            {
                return CachedData.GetInstance().GetMasterCategorySubCategoryTables();
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

        public DataSet GetAllActivityTables()
        {
            try
            {
                return CachedData.GetInstance().GetAllActivityTables();
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
        public SecMasterRequestObj GetAlltradedSymbols()
        {
            try
            {
                return CachedData.GetInstance().GetAlltradedSymbols();
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
            return new SecMasterRequestObj();
        }

        public bool IsSymbolTraded(long symbol_pk)
        {
            try
            {
                return CachedData.GetInstance().IsSymbolTraded(symbol_pk);
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
            return false;
        }


        public Dictionary<int, string> GetUserCounterParties()
        {
            return _userCachedData.GetUserCounterParties();
        }

        public int GetCompanyID()
        {
            try
            {
                return CachedData.CompanyID;
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
            return int.MinValue;
        }

        public string GetCompanyName()
        {
            try
            {
                return CachedData.CompanyName;
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
            return string.Empty;
        }

        public int GetCompanyBaseCurrencyID()
        {
            try
            {
                return CachedData.CompanyBaseCurrencyID;
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
            return int.MinValue;
        }

        public bool IsSendRealtimeManualOrderViaFix()
        {
            try
            {
                return CachedData.IsSendRealtimeManualOrderViaFix;
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
            return false;
        }

        public bool GetIsMarketDataPermissionEnabledForTradingRules()
        {
            try
            {
                return CachedData.IsInMarketDataPermissionEnabledForTradingRules;
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
            return false;
        }

        public ConcurrentDictionary<int, int> GetAccountWiseBaseCurrencyID()
        {
            try
            {
                return CachedData.GetInstance().DictAccountWiseBaseCurrency;
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
            return new ConcurrentDictionary<int, int>();
        }

        public int GetBaseCurrencyIDForAccount(int accountID)
        {
            try
            {
                if (CachedData.GetInstance().DictAccountWiseBaseCurrency.ContainsKey(accountID))
                {
                    return CachedData.GetInstance().DictAccountWiseBaseCurrency[accountID];
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
            return int.MinValue;
        }

        public Dictionary<int, string> GetAllAuecs()
        {
            Dictionary<int, string> dictAuecs = null;
            try
            {
                dictAuecs = CachedData.GetInstance().AUECs;
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
            return dictAuecs;
        }

        //public int GetMethodologybyAccountID(int accountID, int assetID)
        //{
        //    try
        //    {
        //        string assetName = GetAssetText(assetID);

        //        DataTable accountingMethods = CachedData.GetInstance().AccountingMethods.AccountingMethodsTable;
        //        if (assetName != String.Empty && accountingMethods.Columns.Contains(assetName))
        //        {

        //            foreach (DataRow row in accountingMethods.Rows)
        //            {
        //                if (row["AccountID"].ToString() == accountID.ToString())
        //                {

        //                    return System.Convert.ToInt32(row[assetName].ToString());

        //                    //if (!(bool.Parse(row[assetName].ToString())))
        //                    //{
        //                    //    return int.MinValue;
        //                    //}
        //                    //else
        //                    //{
        //                    //    return System.Convert.ToInt32(row["Algorithm"].ToString());
        //                    //}
        //                }
        //            }
        //        }
        //        return int.MinValue;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetActivityType()
        {
            try
            {
                return CachedData.GetInstance().ActivityType;
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
            return new Dictionary<string, int>();
        }

        public static string GetActivityTypeWithAcronym(string acronym)
        {
            try
            {
                if (CachedData.GetInstance().ActivityTypeWithAcronym.ContainsKey(acronym))
                {
                    return CachedData.GetInstance().ActivityTypeWithAcronym[acronym];
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
            return String.Empty;
        }

        /// <summary>
        /// Gets the activity type from acronym.
        /// </summary>
        /// <param name="activityAcronym">The activity acronym.</param>
        /// <returns></returns>
        public static Activities GetActivityTypeFromAcronym(string activityAcronym)
        {

            try
            {
                if (CachedData.GetInstance().ActivityTypeWithAcronym.ContainsValue(activityAcronym))
                {
                    Activities activityType = Activities.Select;
                    string activity = CachedData.GetInstance().ActivityTypeWithAcronym.FirstOrDefault(x => x.Value.Equals(activityAcronym)).Key;
                    if (Enum.TryParse<Activities>(activity, out activityType))
                        return activityType;
                    else
                        return Activities.Select;
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
            return Activities.Select;
        }

        #region commented
        //public static Dictionary<string, int> GetCashTransactionTypeWithAcronym()
        //{
        //    Dictionary<string, int> dictCashTransactionType = new Dictionary<string, int>();
        //    try
        //    {
        //        return CachedData.GetInstance().CashTransactionTypeWithAcronym;
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
        //    return dictCashTransactionType;
        //}

        //public static Dictionary<string, int> GetCashTransactionTypeWithName()
        //{
        //    Dictionary<string, int> dictCashTransactionType = new Dictionary<string, int>();
        //    try
        //    {
        //        return CachedData.GetInstance().CashTransactionTypeWithName;
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
        //    return dictCashTransactionType;
        //}
        #endregion

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetActivityAmountType()
        {
            try
            {
                return CachedData.GetInstance().ActivityAmountType;
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
            return new Dictionary<int, string>();
        }

        /// <summary>
        /// Get the Last calculated revaluation dates dictionary
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, RevaluationUpdateDetail> GetLastRevaluationCalculationDate()
        {
            try
            {
                return CachedData.GetInstance().LastRevaluationCalcDate;
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

        /// <summary>
        /// Set the last revaluation date account-wise
        /// </summary>
        /// <param name="date">Date to be set</param>
        /// <param name="accountIDs">Comma separated string of account ids</param>
        public static void SetLastRevaluationCalculationDate(DateTime date, string accountIDs, bool isUpdated)
        {
            try
            {
                int[] accounts;
                if (!string.IsNullOrWhiteSpace(accountIDs))
                {
                    accounts = accountIDs.Split(',').Select(account => Convert.ToInt32(account)).ToArray();
                }
                else
                {
                    accounts = CachedData.GetInstance().LastRevaluationCalcDate.Keys.ToArray();
                }
                foreach (int account in accounts)
                {
                    if (accounts.Contains(account))
                    {
                        if (CachedData.GetInstance().LastRevaluationCalcDate.ContainsKey(account))
                        {
                            CachedData.GetInstance().LastRevaluationCalcDate[account].LastRevaluationDate = date;
                            CachedData.GetInstance().LastRevaluationCalcDate[account].isUpdatedReval = isUpdated;
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

        //Commmented by OMshiv, moved to UDA cache Manager in secmasterNew Project
        ///// <summary>
        ///// Get UDA text by ID of particular UDA type like UDASector, UDA Subsector 
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <param name="UDAType"></param>
        ///// <returns></returns>
        //public string GetUDATextFromID(int ID, String UDAType)
        //{
        //    String attributeValue = "Undefined";
        //    try
        //    {

        //        Dictionary<int, string> dt = new Dictionary<int, string>();
        //        switch (UDAType)
        //        {
        //            case SecMasterConstants.CONST_UDAAsset:
        //                dt = CachedData.GetInstance().UDAAssets;
        //                break;

        //            case SecMasterConstants.CONST_UDASector:
        //                dt = CachedData.GetInstance().UDASectors;
        //                break;

        //            case SecMasterConstants.CONST_UDASubSector:
        //                dt = CachedData.GetInstance().UDASubSectors;
        //                break;

        //            case SecMasterConstants.CONST_UDASecurityType:
        //                dt = CachedData.GetInstance().UDASecurityTypes;
        //                break;

        //            case SecMasterConstants.CONST_UDACountry:
        //                dt = CachedData.GetInstance().UDACountries;
        //                break;


        //        }

        //        if (dt.ContainsKey(ID))
        //        {
        //            attributeValue = dt[ID];
        //        }
        //        else
        //        {
        //            attributeValue = "Undefined"; 
        //        }
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
        //    return attributeValue;
        //}

        public static ActivitySource GetActivitySource(int activityTypeId)
        {
            try
            {
                Dictionary<int, byte> dictActivityTypeActivitySource = CachedData.GetInstance().ActivityTypeActivitySource;
                if (dictActivityTypeActivitySource.ContainsKey(activityTypeId))
                {
                    return ((ActivitySource)dictActivityTypeActivitySource[activityTypeId]);
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
            return ActivitySource.Trading;
        }

        public void RefreshPranaPreferences()
        {
            try
            {
                CachedData.GetPranaPreferences();
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
        /// Added BY: Bharat Raturi
        /// date: 16 may 2014
        /// purpose: get the companyID from the company name
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns>ID of the company</returns>
        public static int GetCompanyID(string companyName)
        {
            Dictionary<int, string> dt = CachedData.GetInstance().Companies;
            try
            {
                foreach (int key in dt.Keys)
                {
                    if (dt[key].Equals(companyName))
                    {
                        return key;
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
            return -1;
        }
        public Dictionary<Tuple<int, string>, int> GetDefaultAUECMapping()
        {
            try
            {
                return CachedData.GetInstance().dictDefaultAUECMapping;
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
                return null;
            }
        }

        /// <summary>
        /// added By: Bharat raturi, 03 jun 2014
        /// purpose: refresh the client information when the new client is added
        /// </summary>
        public void RefreshClientCache()
        {
            try
            {
                CachedData.GetInstance().RefreshClientCache();
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
        /// returns thirdParty name of thirdPartyID
        /// </summary>
        /// <param name="thirdPartyID"></param>
        /// <returns></returns>
        public string GetThirdPartyNameByID(int thirdPartyID)
        {
            Dictionary<int, string> dt = CachedData.GetInstance().ThirdParties;
            try
            {
                if (dt.ContainsKey(thirdPartyID))
                {
                    return dt[thirdPartyID];
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
            return string.Empty;
        }

        /// <summary>
        /// returns thirdParty short name of thirdPartyID
        /// </summary>
        /// <param name="thirdPartyID"></param>
        /// <returns></returns>
        public string GetThirdPartyShortNameByID(int thirdPartyID)
        {
            Dictionary<int, string> dt = CachedData.GetInstance().ThirdPartiesWithShortName;
            try
            {
                if (dt.ContainsKey(thirdPartyID))
                {
                    return dt[thirdPartyID];
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
            return string.Empty;
        }

        /// <summary>
        /// returns thirdPartyID of thirdParty Long name
        /// </summary>
        /// <param name="thirdPartyID"></param>
        /// <returns></returns>
        public int GetThirdPartyIDByFullName(string thirdPartyName)
        {
            Dictionary<int, string> dt = CachedData.GetInstance().ThirdParties;
            try
            {
                if (dt.Values.Contains(thirdPartyName, StringComparer.InvariantCultureIgnoreCase))
                {
                    foreach (KeyValuePair<int, string> kvp in dt)
                    {
                        if (dt[kvp.Key].Equals(thirdPartyName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return kvp.Key;
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
            return int.MinValue;
        }

        /// <summary>
        /// returns thirdPartyID of thirdParty Short name
        /// </summary>
        /// <param name="thirdPartyID"></param>
        /// <returns></returns>
        public int GetThirdPartyIDByShortName(string thirdPartyName)
        {
            Dictionary<int, string> dt = CachedData.GetInstance().ThirdPartiesWithShortName;
            try
            {
                if (dt.Values.Contains(thirdPartyName, StringComparer.InvariantCultureIgnoreCase))
                {
                    foreach (KeyValuePair<int, string> kvp in dt)
                    {
                        if (dt[kvp.Key].Equals(thirdPartyName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return kvp.Key;
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
            return int.MinValue;
        }

        public void UpdateAccountLockData(List<int> accountsToBeLocked)
        {
            try
            {
                foreach (KeyValuePair<int, string> account in GetAccountsWithFullName())
                {
                    //new Account Locked	
                    if (accountsToBeLocked.Contains(account.Key) && !GetLockedAccounts().Contains(account.Key))
                    {
                        ResetAccountsLockTimer(account.Key, 0);
                    }
                    //Account Released
                    if (!accountsToBeLocked.Contains(account.Key) && GetLockedAccounts().Contains(account.Key))
                    {
                        ResetAccountsLockTimer(account.Key, int.MinValue);
                    }
                }
                SetLockedAccounts(accountsToBeLocked);
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
        /// Added By Faisal Shah
        /// 29/10/14 Needed accounts as Dictionary at multiple places
        /// So had to move the function here
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetUserAccountsAsDict()
        {
            return _userCachedData.GetUserAccountsAsDict();
        }

        public Dictionary<int, string> GetUserTradingAccountsAsDict()
        {
            TradingAccountCollection userAccounts = _userCachedData.UserTradingAccounts;
            Dictionary<int, string> convertedAccounts = new Dictionary<int, string>();
            try
            {
                if (userAccounts != null)
                {
                    foreach (Prana.BusinessObjects.TradingAccount account in userAccounts)
                    {

                        if (!convertedAccounts.ContainsKey(account.TradingAccountID) && account.TradingAccountID != int.MinValue)
                        {
                            convertedAccounts.Add(account.TradingAccountID, account.Name);
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
            return convertedAccounts;
        }


        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dsActivities.Dispose();

                if (_cachedDataManager != null)
                    _cachedDataManager = null;
                if (_userCachedData != null)
                {
                    _userCachedData.Dispose();
                }
            }
        }

        #endregion


        public List<string> GetAllAccountIDsForUser()
        {
            try
            {

                List<string> userAccounts = new List<string>();
                foreach (Account account in _userCachedData.UserAccounts)
                {
                    if (account.AccountID > 0)
                        userAccounts.Add(account.AccountID.ToString());
                }
                return userAccounts;

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
            return new List<string>();
        }

        public List<string> GetAllAccountNamesForUser()
        {
            try
            {

                List<string> userAccounts = new List<string>();
                foreach (Account account in _userCachedData.UserAccounts)
                {
                    if (account.AccountID > 0)
                        userAccounts.Add(account.Name);
                }
                return userAccounts;

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
            return new List<string>();
        }

        /// <summary>
        /// Get roundlot value for given AUEC id, PRANA-12686
        /// </summary>
        /// <param name="auecID"> AUEC ID</param>
        /// <returns></returns>
        public decimal GetRoundLotByAUECID(int auecID)
        {
            try
            {
                Dictionary<int, decimal> dt = CachedData.GetInstance().AuecRoundLot;
                if (dt.ContainsKey(auecID))
                {
                    return dt[auecID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 1;
        }

        public Dictionary<int, string> GetUserStrategiesDictionary()
        {
            Dictionary<int, string> userStrategies = new Dictionary<int, string>();
            try
            {
                StrategyCollection strategyCollection = _userCachedData.UserStrategies;
                foreach (Strategy strategy in strategyCollection)
                {
                    userStrategies.Add(strategy.StrategyID, strategy.Name.Trim());
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return userStrategies;
        }



        /// <summary>
        /// Gets the key from value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameter">The source dictionary identifier.</param>
        /// <returns>The corresponding key</returns>
        public object GetDictionaryKeyFromValue(object value, object source)
        {
            try
            {
                switch (source.ToString())
                {
                    case "TransactionType":
                        if (CachedData.GetInstance().DictTransactionType.ContainsValue(value.ToString()))
                        {
                            foreach (string key in CachedData.GetInstance().DictTransactionType.Keys)
                            {
                                if (CachedData.GetInstance().DictTransactionType[key].Equals(value.ToString()))
                                    return key;
                            }
                        }
                        break;

                    case "Calculation Basis":
                        Dictionary<int, string> calculationBasisDict = CommissionEnumHelper.GetOldListForCalculationBasisAsDic();
                        if (calculationBasisDict.ContainsValue(value.ToString()))
                        {
                            foreach (int key in calculationBasisDict.Keys)
                            {
                                if (calculationBasisDict[key].Equals(value.ToString()))
                                    return key;
                            }
                        }
                        else
                            return null;
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return value;
        }

        /// <summary>
        /// Gets the dictionary value from key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="source">The source dictionary identifier.</param>
        /// <returns>The corresponding value</returns>
        public object GetDictionaryValueFromKey(object key, object source)
        {
            try
            {
                switch (source.ToString())
                {
                    case "TransactionType":
                        foreach (KeyValuePair<string, string> transactionType in CachedData.GetInstance().DictTransactionType)
                        {
                            if (transactionType.Key.ToLower().Equals(key.ToString().ToLower()))
                                return transactionType.Value;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return key;
        }

        public bool IsAlgoBrokerFromID(int brokerID)
        {
            return CachedData.AlgoBrokersWithFullName.ContainsKey(brokerID);
        }

        public bool IsAlgoBrokerFromShortName(string brokerShortName)
        {
            return CachedData.AlgoBrokersWithShortName.ContainsValue(brokerShortName);
        }

        public bool IsAlgoBrokerFromFullName(string brokerFullName)
        {
            return CachedData.AlgoBrokersWithFullName.ContainsValue(brokerFullName);
        }

        /// <summary>
        /// Gets the send allocations via fix.
        /// </summary>
        /// <returns></returns>
        public static bool GetSendAllocationsViaFix()
        {
            try
            {
                return CachedData.SendAllocationsViaFix;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Gets the swap accounts.
        /// </summary>
        /// <returns></returns>
        public static List<int> GetSwapAccounts()
        {
            List<int> swapAccounts = new List<int>();
            try
            {
                Dictionary<int, List<Account>> companyAccountmapping = CachedData.GetInstance().CompanyAccountsMapping;
                if (companyAccountmapping != null && companyAccountmapping.ContainsKey(CachedData.CompanyID))
                {
                    List<Account> companyAccounts = companyAccountmapping[CachedData.CompanyID];
                    swapAccounts.AddRange(companyAccounts.Where(x => x.IsSwapAccount).Select(y => y.AccountID).ToList());
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return swapAccounts;

        }
        /// <summary>
        /// Update cache and Save in the DB for Prana Preference of RevaluationDailyProcessDays
        /// </summary>
        public void UpdateandSavePranaPreferenceRevaluationPref(int dailyProcessDays)
        {
            try
            {
                if (CachedData.GetInstance().PranaPreferences.ContainsKey(ApplicationConstants.CONST_REVALUATION_DAILY_PROCESS_DAYS))
                {
                    CachedData.GetInstance().PranaPreferences[ApplicationConstants.CONST_REVALUATION_DAILY_PROCESS_DAYS] = dailyProcessDays.ToString();
                }

                _clientsCommonDataManager.SavePranaPreferencesRevaluationPrefinDB(dailyProcessDays);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Add pending approval frozen alerts' order id to cache
        /// </summary>
        /// <param name="orderId"></param>
        public void AddPendingApprovalFrozenAlerts(string orderId)
        {
            try
            {
                if (!CachedData.LstPendingApprovalFrozenAlerts.Contains(orderId))
                    CachedData.LstPendingApprovalFrozenAlerts.Add(orderId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Remove pending approval frozen alerts' order id from cache
        /// </summary>
        /// <param name="orderId"></param>
        public void RemovePendingApprovalFrozenAlerts(string orderId)
        {
            try
            {
                if (CachedData.LstPendingApprovalFrozenAlerts.Contains(orderId))
                    CachedData.LstPendingApprovalFrozenAlerts.Remove(orderId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets pending approval frozen alerts' order id
        /// </summary>
        /// <returns></returns>
        public List<string> GetPendingApprovalFrozenAlerts()
        {
            List<string> listPendingApprovalFrozenAlerts = new List<string>();
            try
            {
                listPendingApprovalFrozenAlerts = CachedData.LstPendingApprovalFrozenAlerts;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return listPendingApprovalFrozenAlerts;
        }

        /// <summary>
        /// Retrieves the default values for each of the 45 trade attributes from the cached DataSet.
        /// Each attribute's default values (comma-separated string) are split and returned as a List of strings.
        /// </summary>
        public List<string>[] GetAttributeDefaultValues()
        {
            // Initialize an array of 45 lists to hold the default values per attribute
            List<string>[] defaultValuesArray = new List<string>[45];
            DataSet dsAttributes = CachedData.GetInstance().AttributeNames;

            if (dsAttributes.Tables.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr in dsAttributes.Tables[0].Rows)
                {
                    if (i >= defaultValuesArray.Length)
                        break;

                    // Read and split default values by comma, remove empty entries and trim spaces
                    string rawDefaults = dr[3]?.ToString() ?? string.Empty;
                    defaultValuesArray[i] = rawDefaults.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(value => value.Trim()).Distinct().ToList();
                    i++;
                }
            }
            return defaultValuesArray;
        }


    }
}
