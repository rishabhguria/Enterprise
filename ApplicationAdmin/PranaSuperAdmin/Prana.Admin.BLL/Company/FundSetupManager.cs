using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using System.Linq;

namespace Prana.Admin.BLL
{
    public class AccountSetupManager
    {
        /// <summary>
        /// Dictionary to hold the accountID-account details pair
        /// </summary>
        public static Dictionary<int, AccountDetails> dictAccounts = new Dictionary<int, AccountDetails>();

        /// <summary>
        /// Flag variable to check whether any change has been done in the account information
        /// </summary>
        public static bool _isAccountChanged = false;

        /// <summary>
        /// Flag variable to check whether any change has been done in the account mapping
        /// </summary>
        public static bool _isMappingChanged = false;

        /// <summary>
        /// Dictionary of all feeder accounts 
        /// </summary>
        public static Dictionary<int, FeederAccountItem> dictFeederAccounts = new Dictionary<int, FeederAccountItem>();

        /// <summary>
        /// Dictionary to hold the mapping of the account with feeder account
        /// </summary>
        static Dictionary<int, List<AccountFeederMapItem>> mappingDictionary;

        /// <summary>
        /// IDs of mapped accounts
        /// </summary>
        public static List<int> mappedAccount;

        /// <summary>
        /// Dictionary to hold the accountID associated with batches
        /// </summary>
        public static Dictionary<int, string> dictBatchAssociatedAccounts = new Dictionary<int, string>();

        //Added By Faisal Shah
        //Dated 07/07/14
        //Assign this newID as AccountID till we get a Max ID from DB
        public static int newID = 0;

        /// <summary>
        /// Get all the accounts from the Database
        /// </summary>
        /// <param name="companyID">ID of the current company</param>
        /// <returns>Datatable holding the details of the accounts</returns>
        public static DataTable InitializeAccountDetails(int companyID)
        {
            DataTable dtAccounts = new DataTable();
            dtAccounts.Columns.Add("FundId", typeof(int));
            dtAccounts.Columns.Add("FundName", typeof(string));
            dtAccounts.Columns.Add("FundShortName", typeof(string));
            dtAccounts.Columns.Add("IsActive", typeof(bool));
            try
            {
                dictAccounts = AccountSetupDAL.GetAccountsFromDB(companyID);
                UpdateAccountLockDates();
                foreach (int accountID in dictAccounts.Keys)
                {
                    AccountDetails account = dictAccounts[accountID];
                    dtAccounts.Rows.Add(accountID, account.AccountName, account.AccountShortName, account.IsActive);
                }
                mappingDictionary = AccountSetupDAL.LoadAccountFeederAccountMappingFromDB(companyID);
                dictFeederAccounts = AccountSetupDAL.LoadFeederAccountsFromDb(companyID);
                mappedAccount = AccountSetupDAL.LoadMappedAccountsFromDB();
                dictBatchAssociatedAccounts = AccountSetupDAL.LoadAccountsAssociatedWithBatch();
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
            return dtAccounts;
        }

        /// <summary>
        /// Get the Details of the selected account
        /// </summary>
        /// <param name="accountID">ID of the selected account</param>
        /// <returns>Object holding the details of the account</returns>
        public static AccountDetails GetCurrentAccount(int accountID)
        {
            try
            {
                if (dictAccounts.ContainsKey(accountID))
                {
                    return dictAccounts[accountID];
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

        /// <summary>
        /// get the currency details from the database
        /// </summary>
        /// <returns>THe datatable of the currency details</returns>
        public static DataTable GetCurrencyDetails()
        {
            DataTable dtCurrencies = new DataTable();
            dtCurrencies.Columns.Add("CurrencyID", typeof(int));
            dtCurrencies.Columns.Add("Name", typeof(string));
            try
            {
                Dictionary<int, string> dictCurrency = FeederAccountDAL.LoadCurrencies();
                dtCurrencies.Rows.Add(-1, "-Select-");
                foreach (int currencyID in dictCurrency.Keys)
                {
                    dtCurrencies.Rows.Add(currencyID, dictCurrency[currencyID]);
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
            return dtCurrencies;
        }

        /// <summary>
        /// Get the closing methods from the database
        /// </summary>
        /// <returns>The datatable holding the closing methodology details</returns>
        public static DataTable GetClosingMethods()
        {
            DataTable dtClosingMethod = new DataTable();
            dtClosingMethod.Columns.Add("MethodID", typeof(int));
            dtClosingMethod.Columns.Add("MethodName", typeof(string));
            try
            {
                Dictionary<int, string> dictClosingMethod = AccountSetupDAL.GetClosingMethodsFromDB();
                dtClosingMethod.Rows.Add(-1, "-Select-");
                foreach (int methodID in dictClosingMethod.Keys)
                {
                    dtClosingMethod.Rows.Add(methodID, dictClosingMethod[methodID]);
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
            return dtClosingMethod;
        }

        /// <summary>
        /// Get the table of posting schedules from DB
        /// </summary>
        /// <returns>The datatable holding the details</returns>
        public static DataTable GetScheduleTypes()
        {
            DataTable dtPostSchedule = new DataTable();
            dtPostSchedule.Columns.Add("ScheduleID", typeof(int));
            dtPostSchedule.Columns.Add("ScheduleName", typeof(string));
            try
            {
                dtPostSchedule.Rows.Add(-1, "-Select-");
                Dictionary<int, string> dictPostSchedule = AccountSetupDAL.GetPostingSchedulesFromDB();
                foreach (int scheduleID in dictPostSchedule.Keys)
                {
                    dtPostSchedule.Rows.Add(scheduleID, dictPostSchedule[scheduleID]);
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
            return dtPostSchedule;
        }

        /// <summary>
        /// get the details of the secondary sort types from Database
        /// </summary>
        /// <returns>datatable holding the secondary sort details</returns>
        public static DataTable GetSecondarySort()
        {
            DataTable dtSecondarySort = new DataTable();
            dtSecondarySort.Columns.Add("SecondarySortID", typeof(int));
            dtSecondarySort.Columns.Add("SecondarySortName", typeof(string));
            try
            {
                dtSecondarySort.Rows.Add(-1, "-Select-");
                Dictionary<int, string> dictSecondarySort = AccountSetupDAL.LoadSecondarySortTypes();
                foreach (int secondarySortID in dictSecondarySort.Keys)
                {
                    dtSecondarySort.Rows.Add(secondarySortID, dictSecondarySort[secondarySortID]);
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
            return dtSecondarySort;
        }

        /// <summary>
        /// get the third party details from the database
        /// </summary>
        /// <param name="companyID">ID of the company</param>
        /// <returns>The datatable holding the third party details</returns>
        public static DataTable GetThirdParty()
        {
            DataTable dtThirdParty = new DataTable();
            dtThirdParty.Columns.Add("ThirdPartyID", typeof(int));
            dtThirdParty.Columns.Add("ThirdPartyName", typeof(string));
            try
            {
                dtThirdParty.Rows.Add(-1, "-Select-");
                Dictionary<int, string> dictThirdParty = AccountSetupDAL.GetThirdPartyFromDB();//companyID);
                //dtThirdParty.Rows.Add(0,"-Select-");
                foreach (int thirdPartyID in dictThirdParty.Keys)
                {
                    dtThirdParty.Rows.Add(thirdPartyID, dictThirdParty[thirdPartyID]);
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
            return dtThirdParty;
        }

        /// <summary>
        /// Save the Account Setup details in the Database
        /// </summary>
        /// <param name="companyID">ID of the company</param>
        /// <returns>True if the data is saved</returns>
        public static string SaveAccountSetup(int companyID)
        {
            try
            {
                //Modified By Faisal Shah
                //Dated 03/07/14
                //Save new Account ID in place of temporary AccountID
                int _maxAccountID = GetIDforNewAccount();
                int tempAccountID = 0;
                for (int newKey = newID + 1; newKey <= 0; newKey++)
                {
                    tempAccountID = _maxAccountID;
                    if (dictAccounts.Keys.Contains(newKey))
                    {
                        if (!dictAccounts.ContainsKey(_maxAccountID))
                        {
                            dictAccounts.Add(_maxAccountID, dictAccounts[newKey]);

                        }
                        else
                            dictAccounts[_maxAccountID] = dictAccounts[newKey];

                        dictAccounts[newKey].AccountID = _maxAccountID;
                        dictAccounts.Remove(newKey);
                        _maxAccountID++;
                    }
                    if (mappingDictionary.Keys.Contains(newKey))
                    {
                        if (!mappingDictionary.ContainsKey(tempAccountID))
                        {
                            mappingDictionary.Add(tempAccountID, mappingDictionary[newKey]);
                        }
                        else
                            mappingDictionary[tempAccountID] = mappingDictionary[newKey];
                        mappingDictionary.Remove(newKey);


                    }
                }

                DataSet dsAccount = CreateAccountDataSet();
                DataSet dsMapping = CreateMappingDataSet();
                DataSet dsAmount = CreateAmountDataSet(dsMapping.Tables[0]);
                String xmlAccount = dsAccount.GetXml();
                String xmlMapping = dsMapping.GetXml();
                String xmlAmount = dsAmount.GetXml();
                bool isPermanentDeletion = CachedDataManager.GetInstance.IsPermanentDeletionEnabled();
                string duplicateAccount = AccountSetupDAL.SaveAccountSetup(xmlAccount, xmlMapping, xmlAmount, companyID, isPermanentDeletion);
                AccountSetupDAL.SaveAccountSetupRevalPreference(xmlAccount);
                if (duplicateAccount == "Failure")
                {
                    return duplicateAccount;
                }
                else if (duplicateAccount == "NotFound")
                {
                    _isAccountChanged = false;
                }
                return duplicateAccount;

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
        /// create the dataset of the amount details
        /// </summary>
        /// <param name="dtMapping"></param>
        /// <returns></returns>
        private static DataSet CreateAmountDataSet(DataTable dtMapping)
        {
            DataSet dsAmount = new DataSet("dsAmount");
            DataTable dtAmount = new DataTable("dtAmount");
            dtAmount.Columns.Add("FeederFundID", typeof(int));
            dtAmount.Columns.Add("Amount", typeof(decimal));
            dtAmount.Columns.Add("CurrencyID", typeof(int));
            List<FeederAccountItem> feederList = new List<FeederAccountItem>();
            try
            {
                foreach (int accountID in dictFeederAccounts.Keys)
                {
                    dictFeederAccounts[accountID].FeederAmount = 0;
                    feederList.Add(dictFeederAccounts[accountID]);
                }
                foreach (DataRow dr in dtMapping.Rows)
                {
                    int feederID = (int)dr["CompanyFeederFundID"];
                    //List<AccountFeederMapItem> mappedFeederList = mappingDictionary[accountID];
                    foreach (FeederAccountItem feederItem in feederList)
                    {
                        if (feederID == feederItem.FeederAccountID)
                        {
                            feederItem.FeederAmount += (decimal)dr["AllocatedAmount"];
                            feederItem.FeederCurrency = (int)dr["CurrencyID"];
                            break;
                        }
                        //foreach (AccountFeederMapItem mappedFeeder in mappedFeederList)
                        //{
                        //    if (mappedFeeder.FeederAccountID == feederItem.FeederAccountID)
                        //    {
                        //        feederItem.FeederAmount += mappedFeeder.AllocatedAmount;
                        //    }
                        //}
                    }
                }
                foreach (FeederAccountItem feeder in feederList)
                {
                    dtAmount.Rows.Add(feeder.FeederAccountID, feeder.FeederAmount, feeder.FeederCurrency);
                }
                dsAmount.Tables.Add(dtAmount);
                return dsAmount;
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
        /// Create the Dataset of the account details
        /// </summary>
        /// <returns>Dataset holding the account details</returns>
        public static DataSet CreateAccountDataSet()
        {
            DataSet dsAccount = new DataSet("dsFund");
            DataTable dtAccount = new DataTable("dtFund");
            dtAccount.Columns.Add("FundID", typeof(string));
            dtAccount.Columns.Add("FundName", typeof(string));
            dtAccount.Columns.Add("FundShortName", typeof(string));
            dtAccount.Columns.Add("CompanyID", typeof(int));
            dtAccount.Columns.Add("LocalCurrency", typeof(int));
            dtAccount.Columns.Add("FundInceptionDate", typeof(string));
            dtAccount.Columns.Add("FundOnBoardDate", typeof(string));
            dtAccount.Columns.Add("ClosingMethodology", typeof(int));
            dtAccount.Columns.Add("LockDate", typeof(string));
            dtAccount.Columns.Add("CompanyThirdPartyID", typeof(int));
            dtAccount.Columns.Add("SecSortCriteriaID", typeof(int));
            dtAccount.Columns.Add("PostingLockScheduleID", typeof(int));
            dtAccount.Columns.Add("ModifiedType", typeof(int));
            dtAccount.Columns.Add("IsSwapAccount", typeof(bool));
            dtAccount.Columns.Add("IsActive", typeof(bool));
            try
            {
                foreach (int accountID in dictAccounts.Keys)
                {
                    AccountDetails account = dictAccounts[accountID];
                    if (account.modifiedType != 0)
                    {
                        if (account.LockDate.Equals("N/A"))
                        {
                            account.LockDate = DateTimeConstants.MinValue.ToString();
                        }
                        dtAccount.Rows.Add(account.AccountID, account.AccountName, account.AccountShortName, account.CompanyID, account.Currency,
                            account.InceptionDate.ToShortDateString(), account.OnBoardDate.ToShortDateString(),
                            account.ClosingMethodology, account.LockDate,//.ToShortDateString(),
                        account.CompanyPrimeBrokerClearerID, account.SecondarySortCriteria, account.LockSchedule, account.modifiedType, account.IsSwapAccount, account.IsActive);
                        if (account.modifiedType == AccountDetails.AccountModifiedType.Deleted && CachedDataManager.GetInstance.GetAccounts().ContainsKey(account.AccountID))
                        {
                            CachedDataManager.GetInstance.GetAccounts().Remove(account.AccountID);
                            if (CachedDataManager.GetInstance.GetAccountsWithFullName().ContainsKey(account.AccountID))
                                CachedDataManager.GetInstance.GetAccountsWithFullName().Remove(account.AccountID);
                        }
                        else if (account.modifiedType != AccountDetails.AccountModifiedType.Deleted && !CachedDataManager.GetInstance.GetAccounts().ContainsKey(account.AccountID))
                        {
                            CachedDataManager.GetInstance.GetAccounts().Add(account.AccountID, account.AccountShortName);
                            if (!CachedDataManager.GetInstance.GetAccountsWithFullName().ContainsKey(account.AccountID))
                                CachedDataManager.GetInstance.GetAccountsWithFullName().Add(account.AccountID, account.AccountShortName);
                        }
                        else if (account.modifiedType != AccountDetails.AccountModifiedType.Deleted)
                        {
                            CachedDataManager.GetInstance.GetAccounts()[account.AccountID] = account.AccountShortName;
                            CachedDataManager.GetInstance.GetAccountsWithFullName()[account.AccountID] = account.AccountShortName;
                        }
                    }

                }
                dsAccount.Tables.Add(dtAccount);
                return dsAccount;
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
        /// Create the Dataset of the account details
        /// </summary>
        /// <returns>Dataset holding the account details</returns>
        public static DataSet CreateDataSetForSelectedAccount(int selectedAccountID)
        {
            DataSet dsAccount = new DataSet("dsAccount");
            DataTable dtAccount = new DataTable("dtAccount");
            dtAccount.Columns.Add("FundID", typeof(string));
            dtAccount.Columns.Add("FundName", typeof(string));
            dtAccount.Columns.Add("FundShortName", typeof(string));
            dtAccount.Columns.Add("CompanyID", typeof(int));
            dtAccount.Columns.Add("LocalCurrency", typeof(int));
            dtAccount.Columns.Add("AccountInceptionDate", typeof(string));
            dtAccount.Columns.Add("AccountOnBoardDate", typeof(string));
            dtAccount.Columns.Add("ClosingMethodology", typeof(int));
            dtAccount.Columns.Add("LockDate", typeof(string));
            dtAccount.Columns.Add("CompanyThirdPartyID", typeof(int));
            dtAccount.Columns.Add("SecSortCriteriaID", typeof(int));
            dtAccount.Columns.Add("PostingLockScheduleID", typeof(int));
            dtAccount.Columns.Add("ModifiedType", typeof(int));
            dtAccount.Columns.Add("IsSwapAccount", typeof(bool));
            dtAccount.Columns.Add("IsActive", typeof(bool));
            try
            {
                foreach (int accountID in dictAccounts.Keys)
                {
                    AccountDetails account = dictAccounts[accountID];
                    if (account.AccountID == selectedAccountID)
                    {
                        dtAccount.Rows.Add(account.AccountID, account.AccountName, account.AccountShortName, account.CompanyID, account.Currency,
                            account.InceptionDate.ToShortDateString(), account.OnBoardDate.ToShortDateString(),
                            account.ClosingMethodology, account.LockDate,//.ToShortDateString(),
                        account.CompanyPrimeBrokerClearerID, account.SecondarySortCriteria, account.LockSchedule, account.modifiedType, account.IsSwapAccount, account.IsActive);
                    }
                }
                dsAccount.Tables.Add(dtAccount);
                return dsAccount;
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
        /// Create dataset of Account-Feeder Account mapping
        /// </summary>
        /// <returns>The dataset holding the mapping details</returns>
        private static DataSet CreateMappingDataSet()
        {
            DataSet dsMapping = new DataSet("dsMapping");
            DataTable dtMapping = new DataTable("dtMapping");
            dtMapping.Columns.Add("FundID", typeof(int));
            dtMapping.Columns.Add("CompanyFeederFundID", typeof(int));
            dtMapping.Columns.Add("AllocatedAmount", typeof(decimal));
            dtMapping.Columns.Add("CurrencyID", typeof(int));
            try
            {
                foreach (int accountID in mappingDictionary.Keys)
                {
                    // To check accountID in dictionary dictAccounts
                    if (dictAccounts.ContainsKey(accountID))
                    {
                        if (dictAccounts[accountID].modifiedType == AccountDetails.AccountModifiedType.Deleted)
                        {
                            continue;
                        }
                    }
                    List<AccountFeederMapItem> MappedAccounts = mappingDictionary[accountID];
                    foreach (AccountFeederMapItem mappedFeeder in MappedAccounts)
                    {
                        if (mappedFeeder.AllocatedAmount != 0.0M)
                        {
                            //Assign AccountID to Mapped Feeder in Place of Temp AccountID
                            mappedFeeder.AccountID = accountID;
                            dtMapping.Rows.Add(mappedFeeder.AccountID, mappedFeeder.FeederAccountID, mappedFeeder.AllocatedAmount, mappedFeeder.CurrencyID);
                        }
                    }
                }
                dsMapping.Tables.Add(dtMapping);
                return dsMapping;
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
        /// Get the mapped feeders for the current account
        /// </summary>
        /// <param name="accountID">ID of the account</param>
        /// <returns>Datatable holding the mapping details</returns>
        public static DataTable GetFeedersForCurrentAccount(int accountID)
        {
            DataTable dtFeeders = new DataTable();
            dtFeeders.Columns.Add("FeederAccountID", typeof(int));
            dtFeeders.Columns.Add("FeederAccountShortName", typeof(string));
            dtFeeders.Columns.Add("AllocatedAmount", typeof(decimal));
            dtFeeders.Columns.Add("CurrencyID", typeof(int));
            try
            {
                //check if the master fund is available in mapping
                if (mappingDictionary.ContainsKey(accountID))
                {
                    List<AccountFeederMapItem> tempFeederCollection = mappingDictionary[accountID];
                    foreach (AccountFeederMapItem mappedFeeder in tempFeederCollection)
                    {
                        FeederAccountItem feeder = GetSingleFeeder(mappedFeeder.FeederAccountID);
                        dtFeeders.Rows.Add(mappedFeeder.FeederAccountID, feeder.FeederShortName, mappedFeeder.AllocatedAmount, mappedFeeder.CurrencyID);
                    }
                }

                if (dtFeeders.Rows.Count > 0)
                {
                    EnumerableRowCollection<DataRow> dr1 = (from row in dtFeeders.AsEnumerable()
                                                            orderby row["FeederAccountShortName"] ascending
                                                            select row);

                    // Dataview's ToTable returns the sorted table
                    dtFeeders = dr1.AsDataView().ToTable();
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

            return dtFeeders;
        }

        /// <summary>
        /// Get the List of all the feeder accounts for the selected account from database
        /// </summary>
        /// <param name="companyID">company ID for which the Feeders are to be loaded</param>
        /// <returns>The collection of accounts</returns>
        public static void GetFeederAccounts(int companyID)
        {
            try
            {
                dictFeederAccounts = AccountSetupDAL.LoadFeederAccountsFromDb(companyID);
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
        /// Get the details of single feeder account
        /// </summary>
        /// <param name="feederId">ID of the feeder account</param>
        /// <returns>One Feeder account Object</returns>
        public static FeederAccountItem GetSingleFeeder(int feederId)
        {
            try
            {
                if (dictFeederAccounts.ContainsKey(feederId))
                {
                    return dictFeederAccounts[feederId];
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

        /// <summary>
        /// Get the dictionary containing the IDs and symbols of currencies
        /// </summary>
        /// <returns>the dictionary of currencies</returns>
        public static Dictionary<int, string> GetFeederIDNames()
        {
            Dictionary<int, string> dictFeederIDName = new Dictionary<int, string>();
            try
            {
                foreach (int feederID in dictFeederAccounts.Keys)
                {
                    dictFeederIDName.Add(feederID, dictFeederAccounts[feederID].FeederAccountName);
                }
                return dictFeederIDName;
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
        /// Save the mapping of account-feeder accounts on the front-end
        /// </summary>
        /// <param name="accountId">ID of the account</param>
        /// <param name="feeders">List of feeders accounts</param>
        public static void SetMappedFeeder(int accountId, DataTable dtFeeder)
        {
            try
            {
                //Code commented by Faisal Shah as We are adding Temp Account Ids as O or Negative.
                #region commented code
                //if (accountId <= 0)
                //{
                //    return;
                //}
                #endregion
                List<AccountFeederMapItem> feeders = new List<AccountFeederMapItem>();
                foreach (DataRow dRow in dtFeeder.Rows)
                {
                    if (!string.IsNullOrEmpty(dRow[0].ToString()) && !string.IsNullOrEmpty(dRow[1].ToString()) && !string.IsNullOrEmpty(dRow[2].ToString()) && !string.IsNullOrEmpty(dRow[3].ToString()))
                    {
                        AccountFeederMapItem mapFeeder = new AccountFeederMapItem();
                        mapFeeder.AccountID = accountId;
                        mapFeeder.FeederAccountID = (int)dRow[0];
                        mapFeeder.AllocatedAmount = (decimal)dRow[2];
                        mapFeeder.CurrencyID = (int)dRow[3];
                        feeders.Add(mapFeeder);
                    }
                }
                if (mappingDictionary.ContainsKey(accountId))
                {
                    _isMappingChanged = CompareOldMapping(feeders, mappingDictionary[accountId]);
                    mappingDictionary[accountId].Clear();
                    mappingDictionary[accountId].AddRange(feeders);
                }
                else
                {
                    if (feeders.Count > 0)
                    {
                        mappingDictionary.Add(accountId, feeders);
                        _isMappingChanged = true;
                    }
                    else
                        _isMappingChanged = false;
                }
                //if (mappingDictionary.ContainsKey(accountId))
                //{
                //    foreach (AccountFeederMapItem mappedFeeder in mappingDictionary[accountId])
                //    {
                //        foreach (FeederAccountItem unMappedFeeder in feederAccounts)
                //        {
                //            if (mappedFeeder.FeederAccountID == unMappedFeeder.FeederAccountID)
                //            {
                //                FeederAccountItem feeder = GetSingleFeeder(mappedFeeder.FeederAccountID);
                //                feeder.FeederRemainingAmount -= mappedFeeder.AllocatedAmount;
                //                feeder.FeederAllocatedAmount += mappedFeeder.AllocatedAmount;
                //            }
                //        }
                //    }
                //}
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
        /// check for the changes in the mapping
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="newMap"></param>
        /// <param name="oldMap"></param>
        /// <returns></returns>
        private static bool CompareOldMapping(List<AccountFeederMapItem> newMap, List<AccountFeederMapItem> oldMap)
        {
            if (oldMap.Count != newMap.Count || _isAccountChanged)
            {
                return true;
            }
            List<int> oldMapIdCollection = new List<int>();
            List<int> newMapIdCollection = new List<int>();

            foreach (AccountFeederMapItem newMapItem in newMap)
            {
                if (!newMapIdCollection.Contains(newMapItem.FeederAccountID))
                {
                    newMapIdCollection.Add(newMapItem.FeederAccountID);
                }
            }

            foreach (AccountFeederMapItem oldMapItem in oldMap)
            {
                if (!oldMapIdCollection.Contains(oldMapItem.FeederAccountID))
                {
                    oldMapIdCollection.Add(oldMapItem.FeederAccountID);
                }
            }

            var a = newMapIdCollection.SequenceEqual(oldMapIdCollection);
            if (!a)
            {
                return true;
            }

            foreach (AccountFeederMapItem newMapItem in newMap)
            {
                foreach (AccountFeederMapItem oldMapItem in oldMap)
                {
                    if (newMapItem.FeederAccountID == oldMapItem.FeederAccountID)
                    {
                        if (newMapItem.AllocatedAmount != oldMapItem.AllocatedAmount)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Create XML representation of the mapping data
        /// </summary>
        /// <param name="dsMapping">Datatset that holds the mapping</param>
        /// <returns>XML representation of mapping</returns>
        public static String CreateMappingXML(DataSet dsMapping)
        {
            String xmlDoc = null;
            try
            {
                xmlDoc = dsMapping.GetXml();
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
            return xmlDoc;
        }

        /// <summary>
        /// Update the details of the current account
        /// </summary>
        /// <param name="account">Account Object</param>
        public static bool UpdateAccount(AccountDetails account, DataTable dtFeeder)
        {
            try
            {
                if (dictAccounts.ContainsKey(account.AccountID))
                {
                    if (dictAccounts[account.AccountID].AccountName != account.AccountName || dictAccounts[account.AccountID].AccountShortName != account.AccountShortName
                        || dictAccounts[account.AccountID].InceptionDate != account.InceptionDate || dictAccounts[account.AccountID].Currency != account.Currency
                        || dictAccounts[account.AccountID].OnBoardDate != account.OnBoardDate || dictAccounts[account.AccountID].SecondarySortCriteria != account.SecondarySortCriteria
                        || dictAccounts[account.AccountID].LockSchedule != account.LockSchedule || dictAccounts[account.AccountID].CompanyPrimeBrokerClearerID != account.CompanyPrimeBrokerClearerID
                        || dictAccounts[account.AccountID].ClosingMethodology != account.ClosingMethodology || dictAccounts[account.AccountID].IsSwapAccount != account.IsSwapAccount || dictAccounts[account.AccountID].IsActive != account.IsActive)
                    {
                        if (!_isAccountChanged)
                        {
                            _isAccountChanged = true;
                        }
                        if (dictAccounts[account.AccountID].modifiedType != AccountDetails.AccountModifiedType.Added)
                        {
                            account.modifiedType = AccountDetails.AccountModifiedType.Updated;
                        }
                        else
                        {
                            account.modifiedType = AccountDetails.AccountModifiedType.Added;
                        }
                        dictAccounts[account.AccountID] = account;
                    }
                    // Added by Ankit Gupta on 8th Oct, 2014
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1550
                    // if no changes were made, set the variable to False, otherwise, it prompt every time, before closing the UI.
                    else
                    {
                        _isAccountChanged = false;
                    }
                }
                else
                {
                    if (!_isAccountChanged)
                    {
                        _isAccountChanged = true;
                    }
                    account.modifiedType = AccountDetails.AccountModifiedType.Added;
                    //Add a new Temp Id for Feeder as well as Account Id when we add new ones.
                    //Modified by Faisal Shah
                    dictAccounts.Add(newID, account);
                    SetMappedFeeder(newID, dtFeeder);
                    newID--;


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
            return _isAccountChanged;
        }

        /// <summary>
        /// Get the ID for the new accounts that are added at the front end
        /// </summary>
        /// <returns>AccountID for the new account</returns>
        public static int GetIDforNewAccount()
        {
            int accountID = 1;

            try
            {
                accountID = AccountSetupDAL.GetNewAccountID();
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
            return accountID;
        }

        /// <summary>
        /// Update the NAV Lock Schedules
        /// </summary>
        public static void UpdateNAVLockSchedules()
        {
            try
            {
                Dictionary<int, int> dictSchedules = new Dictionary<int, int>();
                foreach (int accountid in dictAccounts.Keys)
                {
                    dictSchedules.Add(accountid, dictAccounts[accountid].LockSchedule);
                }
                NAVLockManager.UpdateAccountLockSchedule(dictSchedules);
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
        /// update the lock dates for the accounts based on the lock dates in the nav lock setup
        /// </summary>
        public static void UpdateAccountLockDates()
        {
            try
            {
                Dictionary<int, string> dictLocks = NAVLockManager.GetLockDatesForAccounts();
                foreach (int accountid in dictLocks.Keys)
                {
                    if (dictAccounts.ContainsKey(accountid))
                    {
                        dictAccounts[accountid].LockDate = dictLocks[accountid];
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
        /// Get lock date for the account That is selected in the grid Now
        /// </summary>
        /// <param name="currAccountID"></param>
        /// <returns>Lock date for the current account</returns>
        public static string GetCurrentAccountLockDate(int currAccountID)
        {
            string lockDate = string.Empty;
            try
            {
                if (dictAccounts.ContainsKey(currAccountID))
                {
                    lockDate = dictAccounts[currAccountID].LockDate;
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
            return lockDate;
        }

        /// <summary>
        /// Load all accounts from Data base associated with Batch
        /// </summary>
        /// <returns>true if account associated with Batch</returns>
        public static bool isAccountAssociatedWithBatch(int accountID)
        {
            try
            {
                if (dictBatchAssociatedAccounts.ContainsKey(accountID))
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

        /// <summary>
        /// returns account association with other modules
        /// </summary>
        /// <param name="accountID">accountID</param>
        /// <returns>association</returns>
        public static string GetAccountAssociation(int accountID)
        {
            string result = string.Empty;
            try
            {
                result = AccountSetupDAL.GetCompanyAccountAssociation(accountID);
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

        /// <summary>
        /// Gets the account list.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetAccountList()
        {
            Dictionary<int, string> accountList = new Dictionary<int, string>();
            try
            {
                foreach (int accountId in dictAccounts.Keys)
                {
                    accountList.Add(accountId, dictAccounts[accountId].AccountName);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountList;
        }

        /// <summary>
        /// Gets the swap accounts.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetSwapAccounts()
        {
            Dictionary<int, string> swapAccountList = new Dictionary<int, string>();
            try
            {
                foreach (KeyValuePair<int, AccountDetails> account in dictAccounts.Where(x => x.Value.IsSwapAccount))
                {
                    swapAccountList.Add(account.Key, account.Value.AccountName);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return swapAccountList;
        }
    }
}
