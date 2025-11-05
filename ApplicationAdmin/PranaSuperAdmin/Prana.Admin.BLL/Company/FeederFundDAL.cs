//created By: Bharat Raturi, date: 02/21/2014
//Purpose: Provide the data access facility for the feeder accounts 
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class FeederAccountDAL
    {
        /// <summary>
        /// Initialize Connection string for data base 
        /// </summary>
        static string connStr = "PranaConnectionString";

        /// <summary>
        /// Load all the feeder accounts details from the database
        /// </summary>
        /// <param> The ID of the company for which the feeder accounts are to be loaded</param>
        /// <returns>Datatable holding the account details</returns>
        internal static DataTable LoadFeederAccountsFromDb(int companyID)
        {
            DataTable dtFeederAccounts = new DataTable();
            object[] parameter = { companyID };
            string spFeederAccounts = "P_GetAllFeederFunds";
            try
            {
                dtFeederAccounts = DatabaseManager.DatabaseManager.ExecuteDataSet(spFeederAccounts, parameter, connStr).Tables[0];
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
            return dtFeederAccounts;
        }

        /// <summary>
        /// Load all the feeder accounts names from the database
        /// </summary>
        /// <returns>list of all feeder account names</returns>
        internal static List<string> LoadFeederNamesList()
        {
            List<string> feederNames = new List<string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFeederNames";

            try
            {
                using (IDataReader drFeederNames = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drFeederNames.Read())
                    {
                        feederNames.Add(drFeederNames.GetString(0).ToLower());
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
            return feederNames;
        }

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
        /// Get the list of feeder ids that are mapped to the accounts
        /// </summary>
        /// <returns>the list collection of feeder IDs</returns>
        internal static List<int> GetMappedAccountIDs()
        {
            List<int> accountIdColletion = new List<int>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetMappedFeederFunds";

            try
            {
                using (IDataReader drMappedAccounts = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drMappedAccounts.Read())
                    {
                        accountIdColletion.Add(drMappedAccounts.GetInt32(0));
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
            return accountIdColletion;
        }

        /// <summary>
        /// Delete the Feeder account from the database
        /// </summary>
        /// <param name="feederAccountID"></param>
        /// <returns>Count of deleted record</returns>
        internal static void DeleteFeederFromDB(string xmlFeedersAccounts, bool isDeletedForceFully)
        {
            Object[] prm = new object[2];
            prm[0] = xmlFeedersAccounts;
            prm[1] = (isDeletedForceFully == true ? 1 : 0);
            //object[] prm ={ xmlFeedersFunds };
            string spDeleteFeeder = "P_DeleteFeederFunds";
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery(spDeleteFeeder, prm, connStr);
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
        /// Save the feeder account details in the database
        /// </summary>
        /// <param name="xmlFeedersAccounts">XMl document for inserting the record</param>
        /// <returns>the count of inserted records</returns>
        internal static int SaveFeederAccountsinDB(string xmlFeedersAccounts)
        {
            int count = 0;
            object[] parameter = { xmlFeedersAccounts };

            string sProcSaveData = "P_SaveFeederAccounts";
            try
            {
                count = DatabaseManager.DatabaseManager.ExecuteNonQuery(sProcSaveData, parameter, connStr);
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
            return count;
        }

        /// <summary>
        /// Update the details of exisiting accounts
        /// </summary>
        /// <param name="xmlFeedersAccounts">xml Representation of information</param>
        /// <returns></returns>
        internal static int UpdateFeederAccountsinDB(string xmlFeedersAccounts)
        {
            int count = 0;
            object[] parameter = { xmlFeedersAccounts };

            string sProcSaveData = "P_UpdateFeederAccounts";
            try
            {
                count = DatabaseManager.DatabaseManager.ExecuteNonQuery(sProcSaveData, parameter, connStr);
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
            return count;
        }
    }
}
