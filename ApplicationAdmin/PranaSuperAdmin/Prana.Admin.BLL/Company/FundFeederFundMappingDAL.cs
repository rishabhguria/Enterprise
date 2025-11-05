using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class AccountFeederAccountMappingDAL
    {
        /// <summary>
        /// Connection String
        /// </summary>
        static string connStr = "PranaConnectionString";

        /// <summary>
        /// Load currency details
        /// </summary>
        /// <returns>dictionary holding the currency id and the currency symbol</returns>
        internal static Dictionary<int, string> LoadCurrencies()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCurrencies";

            Dictionary<int, string> currencyCollection = new Dictionary<int, string>();
            try
            {
                using (IDataReader drCurrencies = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drCurrencies.Read())
                    {
                        currencyCollection.Add(drCurrencies.GetInt32(0), drCurrencies.GetString(2));
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
            return currencyCollection;
        }

        /// <summary>
        /// Load all the feeder accounts for this company
        /// </summary>
        /// <param name="companyID">ID of the company</param>
        /// <returns>colection of feeder accounts</returns>
        public static List<FeederAccountItem> LoadFeederAccountsFromDb(int companyID)
        {
            List<FeederAccountItem> feederAccountsCollection = new List<FeederAccountItem>();

            try
            {
                using (IDataReader drFeeders = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllFeederFunds", new object[] { companyID }, connStr))
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
                        feederAccountsCollection.Add(objFeeder);
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
            return feederAccountsCollection;
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
                    while (drMapping.Read())
                    {
                        int accountId = drMapping.GetInt32(1);
                        AccountFeederMapItem mappedFeeder = new AccountFeederMapItem();
                        mappedFeeder.RecordID = drMapping.GetInt32(0);
                        mappedFeeder.AccountID = drMapping.GetInt32(1);
                        mappedFeeder.FeederAccountID = drMapping.GetInt32(2);
                        mappedFeeder.AllocatedAmount = drMapping.GetDecimal(3);
                        mappedFeeder.CompanyID = drMapping.GetInt32(5);

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

        public static int SaveMappingInDB(string xmlMap, string xmlAmount, int companyID)
        {
            int i = 0;
            string spSaveMapping = "P_SaveFundFeederMapping";
            object[] param = { xmlMap, xmlAmount, companyID };
            try
            {
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery(spSaveMapping, param, connStr);
            }
            catch (Exception ex)
            {
                //invoke the policy for exception handling
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return i;
        }
    }
}
