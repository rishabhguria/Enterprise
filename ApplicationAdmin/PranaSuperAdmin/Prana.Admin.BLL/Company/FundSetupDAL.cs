using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class AccountSetupDAL
    {
        /// <summary>
        /// Initialize Connection string for data base 
        /// </summary>
        static string connStr = ApplicationConstants.PranaConnectionString;

        /// <summary>
        /// Get the dictionary holding the account details from Database
        /// </summary>
        /// <param name="companyID">ID of the company</param>
        /// <returns>The dictionary of account details </returns>
        internal static Dictionary<int, AccountDetails> GetAccountsFromDB(int companyID)
        {
            Dictionary<int, AccountDetails> dictAccounts = new Dictionary<int, AccountDetails>();
            try
            {
                string sProc = "P_GetCompanyFundsForSetup";
                object[] param = { companyID };
                using (IDataReader drGetAccounts = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param, connStr))
                {
                    while (drGetAccounts.Read())
                    {
                        AccountDetails account = new AccountDetails();
                        if (drGetAccounts.GetValue(0) != DBNull.Value)
                        {
                            account.AccountID = drGetAccounts.GetInt32(0);
                        }
                        if (drGetAccounts.GetValue(1) != DBNull.Value)
                        {
                            account.AccountName = drGetAccounts.GetString(1);
                        }
                        if (drGetAccounts.GetValue(2) != DBNull.Value)
                        {
                            account.AccountShortName = drGetAccounts.GetString(2);
                        }
                        if (drGetAccounts.GetValue(3) != DBNull.Value)
                        {
                            account.Currency = drGetAccounts.GetInt32(3);
                        }
                        if (drGetAccounts.GetValue(4) != DBNull.Value)
                        {
                            account.InceptionDate = drGetAccounts.GetDateTime(4);
                        }
                        if (drGetAccounts.GetValue(5) != DBNull.Value)
                        {
                            account.OnBoardDate = drGetAccounts.GetDateTime(5);
                        }
                        if (drGetAccounts.GetValue(6) != DBNull.Value)
                        {
                            account.ClosingMethodology = drGetAccounts.GetInt32(6);
                        }
                        if (drGetAccounts.GetValue(7) != DBNull.Value)
                        {
                            account.LockDate = drGetAccounts.GetDateTime(7).ToShortDateString();
                        }
                        if (drGetAccounts.GetValue(8) != DBNull.Value)
                        {
                            account.CompanyPrimeBrokerClearerID = drGetAccounts.GetInt32(8);
                        }
                        if (drGetAccounts.GetValue(9) != DBNull.Value)
                        {
                            account.SecondarySortCriteria = drGetAccounts.GetInt32(9);
                        }
                        if (drGetAccounts.GetValue(10) != DBNull.Value)
                        {
                            account.LockSchedule = drGetAccounts.GetInt32(10);
                        }
                        if (drGetAccounts.GetValue(11) != DBNull.Value)
                        {
                            account.CompanyID = drGetAccounts.GetInt32(11);
                        }
                        if (drGetAccounts.GetValue(12) != DBNull.Value)
                        {
                            account.IsSwapAccount = drGetAccounts.GetBoolean(12);
                        }
                        if (drGetAccounts.GetValue(13) != DBNull.Value)
                        {
                            account.IsActive = drGetAccounts.GetBoolean(13);
                        }
                        if (!dictAccounts.ContainsKey(account.AccountID))
                        {
                            dictAccounts.Add(account.AccountID, account);
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
            return dictAccounts;
        }

        /// <summary>
        /// Get the value (maximum Account ID+1) from the database
        /// </summary>
        /// <returns>The accountid that will be used for the new created account</returns>
        internal static int GetNewAccountID()
        {
            int accountID = 1;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetMaxFundID";

            try
            {
                object idValue = DatabaseManager.DatabaseManager.ExecuteScalar(queryData, connStr);
                if (idValue != DBNull.Value)
                {
                    accountID = Convert.ToInt32(idValue);
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
            return accountID;
        }
        /// <summary>
        /// Get the dictionary of closing methodologies from Database 
        /// </summary>
        /// <returns>The dictionary holding the closing methodology details</returns>
        internal static Dictionary<int, string> GetClosingMethodsFromDB()
        {
            Dictionary<int, string> dictClosingMethod = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetClosingMethodsFundSetup";

                using (IDataReader drClosingMethod = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drClosingMethod.Read())
                    {
                        // Purpose : Excluding "None" closing methodology
                        if (drClosingMethod.GetInt32(0) != 0)
                            dictClosingMethod.Add(drClosingMethod.GetInt32(0), drClosingMethod.GetString(1));
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
            return dictClosingMethod;
        }

        /// <summary>
        /// Get the posting schedules from DB
        /// </summary>
        /// <returns>The dictionary of PostingScheduleID-PostingSchedule name</returns>
        internal static Dictionary<int, string> GetPostingSchedulesFromDB()
        {
            Dictionary<int, string> dictPostSchedule = new Dictionary<int, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllScheduleTypes";

            try
            {
                using (IDataReader drPostSchedule = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drPostSchedule.Read())
                    {
                        dictPostSchedule.Add(drPostSchedule.GetInt32(0), drPostSchedule.GetString(1));
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
            return dictPostSchedule;
        }

        /// <summary>
        /// Get the dictionary of third parties from the Database
        /// </summary>
        /// <param name="companyID">ID of the company</param>
        /// <returns>The dictionary holding the third parties</returns>
        internal static Dictionary<int, string> GetThirdPartyFromDB()//int companyID)
        {
            Dictionary<int, string> dictThirdParty = new Dictionary<int, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetThirdPartyForFundSetup";

            //object[] param = { companyID };
            try
            {
                using (IDataReader drThirdParty = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr)) //, param))
                {
                    while (drThirdParty.Read())
                    {
                        dictThirdParty.Add(drThirdParty.GetInt32(0), drThirdParty.GetString(1));
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
            return dictThirdParty;
        }

        /// <summary>
        /// Load the mapping of the account and feeder accounts
        /// </summary>
        /// <param name="companyID">ID of the company</param>
        /// <returns>Dictionary containg the account IDs as keys and respective collection of feeder IDs as values</returns>
        internal static Dictionary<int, List<AccountFeederMapItem>> LoadAccountFeederAccountMappingFromDB(int companyID)
        {
            Dictionary<int, List<AccountFeederMapItem>> accountFeederAccountMapping = new Dictionary<int, List<AccountFeederMapItem>>();
            string spMapping = "P_GetFundFeederFundAssociationForCompany";
            object[] param = { companyID };
            try
            {
                using (IDataReader drMapping = DatabaseManager.DatabaseManager.ExecuteReader(spMapping, param, connStr))
                {
                    //if (drMapping.HasRows)
                    //{
                    while (drMapping.Read())
                    {
                        int accountId = drMapping.GetInt32(1);
                        AccountFeederMapItem mappedFeeder = new AccountFeederMapItem();
                        mappedFeeder.RecordID = drMapping.GetInt32(0);
                        mappedFeeder.AccountID = drMapping.GetInt32(1);
                        mappedFeeder.FeederAccountID = drMapping.GetInt32(2);
                        mappedFeeder.AllocatedAmount = drMapping.GetDecimal(3);
                        mappedFeeder.CompanyID = drMapping.GetInt32(5);
                        mappedFeeder.CurrencyID = drMapping.GetInt32(8);

                        if (accountFeederAccountMapping.ContainsKey(accountId))
                        {
                            accountFeederAccountMapping[accountId].Add(mappedFeeder);
                        }
                        else
                        {
                            List<AccountFeederMapItem> mapCollection = new List<AccountFeederMapItem>();
                            mapCollection.Add(mappedFeeder);
                            accountFeederAccountMapping.Add(accountId, mapCollection);
                        }
                    }
                    //}
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
            return accountFeederAccountMapping;
        }

        /// <summary>
        /// Load all the feeder accounts for this company
        /// </summary>
        /// <param name="companyID">ID of the company</param>
        /// <returns>colection of feeder accounts</returns>
        public static Dictionary<int, FeederAccountItem> LoadFeederAccountsFromDb(int companyID)
        {
            Dictionary<int, FeederAccountItem> dictFeederAccount = new Dictionary<int, FeederAccountItem>();
            string spFeedersForAccount = "P_GetAllFeederFunds";
            try
            {
                using (IDataReader drFeeders = DatabaseManager.DatabaseManager.ExecuteReader(spFeedersForAccount, new object[] { companyID }, connStr))
                {
                    while (drFeeders.Read())
                    {
                        FeederAccountItem objFeeder = new FeederAccountItem();
                        objFeeder.FeederAccountID = drFeeders.GetInt32(0);
                        objFeeder.FeederAccountName = drFeeders.GetString(1);
                        objFeeder.FeederShortName = drFeeders.GetString(2);
                        objFeeder.FeederAmount = drFeeders.GetDecimal(4);
                        objFeeder.FeederCurrency = drFeeders.GetInt32(5);
                        objFeeder.FeederRemainingAmount = 0.0M;
                        objFeeder.FeederAllocatedAmount = drFeeders.GetDecimal(6);
                        dictFeederAccount.Add(objFeeder.FeederAccountID, objFeeder);
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
            return dictFeederAccount;
        }

        /// <summary>
        /// Save the account details into the database
        /// </summary>
        /// <param name="xmlAccount"></param>
        /// <param name="xmlMapping"></param>
        /// <param name="xmlAmount"></param>
        /// <param name="companyID"></param>
        internal static string SaveAccountSetup(String xmlAccount, String xmlMapping, String xmlAmount, int companyID, bool isDeletedForceFully)
        {
            string message = String.Empty;
            string sProc = "P_SaveFundSetup";
            object[] param = { xmlAccount, xmlMapping, xmlAmount, companyID, isDeletedForceFully };

            try
            {
                var i = DatabaseManager.DatabaseManager.ExecuteScalar(sProc, param, connStr);
                if (i != null)
                    message = i.ToString();
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
            return message;
        }

        /// <summary>
        /// Get the list opf mapped accounts from DB
        /// </summary>
        /// <returns>THe list of Mapped account IDs</returns>
        internal static List<int> LoadMappedAccountsFromDB()
        {
            List<int> mappedAccounts = new List<int>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllMappedFunds";

                using (IDataReader drMappedAccount = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drMappedAccount.Read())
                    {
                        mappedAccounts.Add(drMappedAccount.GetInt32(0));
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
            return mappedAccounts;
        }

        internal static Dictionary<int, string> LoadSecondarySortTypes()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetAllSecondarySortTypes";
            Dictionary<int, string> dictSort = new Dictionary<int, string>();

            try
            {
                using (IDataReader drSceondarySort = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (drSceondarySort.Read())
                    {
                        if (drSceondarySort.GetValue(0) != DBNull.Value && drSceondarySort.GetValue(1) != DBNull.Value)
                        {
                            int sortID = drSceondarySort.GetInt32(0);
                            string sortName = drSceondarySort.GetString(1);
                            if (!dictSort.ContainsKey(sortID))
                            {
                                dictSort.Add(sortID, sortName);
                            }
                        }
                    }
                    return dictSort;
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
        /// Load all accounts from Data base associated with Batch
        /// </summary>
        internal static Dictionary<int, string> LoadAccountsAssociatedWithBatch()
        {
            Dictionary<int, string> accountCollection = new Dictionary<int, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetFundsForSchedule";

            try
            {
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (dr.Read())
                    {
                        if (!accountCollection.ContainsKey(dr.GetInt32(1)))
                        {
                            accountCollection.Add(dr.GetInt32(1), dr.GetString(2));
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
            return accountCollection;
        }

        /// <summary>
        /// To check for account association
        /// </summary>
        /// <param name="thirdPartyID"></param>
        /// <returns></returns>
        public static string GetCompanyAccountAssociation(int accountID)
        {
            string result = string.Empty;
            Object[] parameter = new object[1];
            parameter[0] = accountID;

            try
            {
                result = DatabaseManager.DatabaseManager.ExecuteScalar("P_GetCompanyFundAssociation", parameter).ToString();
            }
            #region Catch
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
            #endregion
            return result;
        }

        /// <summary>
        /// Save the account details in the Revaluation Preference into the database
        /// </summary>
        /// <param name="xmlAccount"></param>
        internal static void SaveAccountSetupRevalPreference(String xmlAccount)
        {
            try
            {
                string sProc = "P_SaveRevaluationPreference";
                object[] param = { xmlAccount, 1 };
                DatabaseManager.DatabaseManager.ExecuteScalar(sProc, param, connStr);
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
    }
}
