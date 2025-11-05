//Created By: Bharat Raturi, Date: 26/02/14
//Purpose: Providing business logic for the account-feeder account mapping

using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class AccountFeederAccountMappingManager
    {
        /// <summary>
        /// ID of the company for which the mapping is loaded
        /// </summary>
        public static int companyID = 0;

        /// <summary>
        /// Dictionary to hold the mapping of the account with feeder account
        /// </summary>
        static Dictionary<int, List<AccountFeederMapItem>> mappingDictionary;

        /// <summary>
        /// Collection of all accounts for the company 
        /// </summary>
        static List<FeederAccountItem> feederAccounts;

        /// <summary>
        /// Get the dictionary containing the IDs and symbols of currencies
        /// </summary>
        /// <returns>the dictionary of currencies</returns>
        public static Dictionary<int, string> GetCurrencies()
        {
            Dictionary<int, string> currencyCollection;
            try
            {
                currencyCollection = AccountFeederAccountMappingDAL.LoadCurrencies();
                return currencyCollection;
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
            Dictionary<int, string> feederDictionary = new Dictionary<int, string>();
            try
            {
                foreach (FeederAccountItem feeder in feederAccounts)
                {
                    feederDictionary.Add(feeder.FeederAccountID, feeder.FeederAccountName);
                }
                return feederDictionary;
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
        /// Get the List of all the feeder accounts for the selected account from database
        /// </summary>
        /// <param name="companyID">company ID for which the Feeders are to be loaded</param>
        /// <returns>The collection of accounts</returns>
        public static List<FeederAccountItem> GetFeederAccounts(int companyID)
        {
            try
            {
                List<FeederAccountItem> listFeederAccounts = AccountFeederAccountMappingDAL.LoadFeederAccountsFromDb(companyID);
                return listFeederAccounts;
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
        /// Get the Dictionary holding the accounts and their IDs
        /// </summary>
        /// <param name="companyID">company ID for which the accounts are to be loaded</param>
        /// <returns>Dictionary of master funds</returns>
        public static Dictionary<int, string> GetAllAccounts(int companyID)
        {
            try
            {
                Dictionary<int, string> mFeederDictionary = MasterFundMappingDAL.LoadAccountsFromDb(companyID);
                mappingDictionary = AccountFeederAccountMappingDAL.LoadAccountFeederAccountMappingFromDB(companyID);
                feederAccounts = AccountFeederAccountMappingDAL.LoadFeederAccountsFromDb(companyID);
                return mFeederDictionary;
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
        /// Get the table of feeder accounts that are associated with the specified master fund
        /// </summary>
        /// <param name="accountID">account id</param>
        /// <returns>The Datatable of feeder accounts</returns>
        public static DataTable GetFeedersForCurrentAccount(int accountID)
        {
            DataTable dtFeeders = new DataTable();
            dtFeeders.Columns.Add("FeederAccountID", typeof(int));
            dtFeeders.Columns.Add("FeederAccountShortName", typeof(string));
            dtFeeders.Columns.Add("Currency", typeof(int));
            dtFeeders.Columns.Add("Amount", typeof(decimal));
            dtFeeders.Columns.Add("RemainingAmount", typeof(decimal));
            dtFeeders.Columns.Add("AllocatedAmount", typeof(decimal));

            try
            {
                //check if the master fund is available in mapping
                if (mappingDictionary.ContainsKey(accountID))
                {
                    List<AccountFeederMapItem> tempFeederCollection = mappingDictionary[accountID];
                    foreach (AccountFeederMapItem mappedFeeder in tempFeederCollection)
                    {
                        FeederAccountItem feeder = GetSingleFeeder(mappedFeeder.FeederAccountID);
                        dtFeeders.Rows.Add(mappedFeeder.FeederAccountID, feeder.FeederShortName, feeder.FeederCurrency, feeder.FeederAmount, feeder.FeederRemainingAmount, mappedFeeder.AllocatedAmount);
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
            return dtFeeders;
        }

        /// <summary>
        /// Save the mapping of account-feeder accounts on the front-end
        /// </summary>
        /// <param name="accountId">ID of the account</param>
        /// <param name="feeders">List of feeders accounts</param>
        public void SetNewMapping(int accountId, List<AccountFeederMapItem> feeders)
        {
            try
            {
                if (mappingDictionary.ContainsKey(accountId))
                {
                    foreach (AccountFeederMapItem mappedFeeder in mappingDictionary[accountId])
                    {
                        foreach (FeederAccountItem unMappedFeeder in feederAccounts)
                        {
                            if (mappedFeeder.FeederAccountID == unMappedFeeder.FeederAccountID)
                            {
                                FeederAccountItem feeder = GetSingleFeeder(mappedFeeder.FeederAccountID);
                                feeder.FeederRemainingAmount += mappedFeeder.AllocatedAmount;
                                feeder.FeederAllocatedAmount -= mappedFeeder.AllocatedAmount;
                                mappedFeeder.AllocatedAmount = 0.0M;
                            }
                        }
                    }
                    mappingDictionary[accountId].Clear();
                    mappingDictionary[accountId].AddRange(feeders);
                }
                else
                {
                    mappingDictionary.Add(accountId, feeders);
                }
                if (mappingDictionary.ContainsKey(accountId))
                {
                    foreach (AccountFeederMapItem mappedFeeder in mappingDictionary[accountId])
                    {
                        foreach (FeederAccountItem unMappedFeeder in feederAccounts)
                        {
                            if (mappedFeeder.FeederAccountID == unMappedFeeder.FeederAccountID)
                            {
                                FeederAccountItem feeder = GetSingleFeeder(mappedFeeder.FeederAccountID);
                                feeder.FeederRemainingAmount -= mappedFeeder.AllocatedAmount;
                                feeder.FeederAllocatedAmount += mappedFeeder.AllocatedAmount;
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

        /// <summary>
        /// Load the grid with the new mapping 
        /// This mapping is is not yet saved in the database
        /// </summary>
        public static DataTable LoadNewMapping(int accountID)
        {
            DataTable dtNewMapping = new DataTable();
            dtNewMapping.Columns.Add("FeederAccountID", typeof(int));
            //dtNewMapping.Columns.Add("FeederAccountName", typeof(string));
            dtNewMapping.Columns.Add("FeederAccountShortName", typeof(string));
            dtNewMapping.Columns.Add("Amount", typeof(decimal));
            dtNewMapping.Columns.Add("Currency", typeof(int));
            dtNewMapping.Columns.Add("RemainingAmount", typeof(decimal));
            dtNewMapping.Columns.Add("AllocatedAmount", typeof(decimal));

            try
            {
                if (mappingDictionary.ContainsKey(accountID))
                {
                    foreach (AccountFeederMapItem mappedFeeder in mappingDictionary[accountID])
                    {
                        FeederAccountItem feeder = GetSingleFeeder(mappedFeeder.FeederAccountID);
                        dtNewMapping.Rows.Add(mappedFeeder.FeederAccountID, feeder.FeederShortName, feeder.FeederAmount, feeder.FeederCurrency, feeder.FeederRemainingAmount, mappedFeeder.AllocatedAmount);
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
            return dtNewMapping;
        }

        /// <summary>
        /// Get the detaiuls of single feeder account
        /// </summary>
        /// <param name="feederId">ID of the feeder account</param>
        /// <returns></returns>
        public static FeederAccountItem GetSingleFeeder(int feederId)
        {
            try
            {
                //List<FeederAccountItem> FeederAccounts = GetFeederAccounts(companyID);
                foreach (FeederAccountItem feeder in feederAccounts)
                {
                    if (feeder.FeederAccountID == feederId)
                        return feeder;
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
        /// Save the mapping in the database
        /// </summary>
        /// <returns> integer value indicating the success of the operation</returns>
        public static int SaveMapping()
        {
            DataSet dsMap = new DataSet();
            String xmlMap = null;
            DataSet dsAmount = new DataSet();
            String xmlAmount = null;

            int i = 0;
            try
            {
                dsMap = CreateMappingDataSet();
                xmlMap = CreateMappingXML(dsMap);
                dsAmount = CreateFeederAmountDataSet();
                xmlAmount = CreateFeederAmountXML(dsAmount);
                i = AccountFeederAccountMappingDAL.SaveMappingInDB(xmlMap, xmlAmount, companyID);
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
            return i;
        }

        /// <summary>
        /// Create the dataset of the mapping details
        /// </summary>
        /// <returns>the dataset holding the new mapping</returns>
        public static DataSet CreateMappingDataSet()
        {
            DataSet dsMapping = new DataSet("dsMapping");
            DataTable dtMapping = new DataTable("dtMapping");
            dtMapping.Columns.Add("FundID", typeof(int));
            dtMapping.Columns.Add("CompanyFeederAccountID", typeof(int));
            dtMapping.Columns.Add("AllocatedAmount", typeof(decimal));
            try
            {
                foreach (int accountID in mappingDictionary.Keys)
                {
                    List<AccountFeederMapItem> MappedAccounts = mappingDictionary[accountID];
                    foreach (AccountFeederMapItem mappedFeeder in MappedAccounts)
                    {
                        dtMapping.Rows.Add(mappedFeeder.AccountID, mappedFeeder.FeederAccountID, mappedFeeder.AllocatedAmount);
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
        /// Create dataset for feeder amounts details
        /// </summary>
        /// <returns>The dataset holding the feeder amount details</returns>
        public static DataSet CreateFeederAmountDataSet()
        {
            DataSet dsAmount = new DataSet("dsAmount");
            DataTable dtAmount = new DataTable("dtAmount");
            try
            {
                List<FeederAccountItem> backUpList = AccountFeederAccountMappingDAL.LoadFeederAccountsFromDb(companyID);

                dtAmount.Columns.Add("FeederAccountID", typeof(int));
                dtAmount.Columns.Add("AllocatedAmount", typeof(decimal));
                dtAmount.Columns.Add("RemainingAmount", typeof(decimal));

                try
                {
                    foreach (FeederAccountItem feeder in feederAccounts)
                    {
                        foreach (FeederAccountItem origfeeder in backUpList)
                        {
                            if (feeder.FeederAccountID == origfeeder.FeederAccountID)
                            {
                                if (feeder.FeederRemainingAmount != origfeeder.FeederRemainingAmount && feeder.FeederAllocatedAmount != origfeeder.FeederAllocatedAmount)
                                {
                                    dtAmount.Rows.Add(feeder.FeederAccountID, feeder.FeederAllocatedAmount, feeder.FeederRemainingAmount);
                                }
                            }
                        }
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
        /// Create XML representation of the mapping data
        /// </summary>
        /// <param name="dsMapping">Datatset that holds the mapping</param>
        /// <returns>XML representation of mapping</returns>
        public static String CreateFeederAmountXML(DataSet dsAmount)
        {
            String xmlAmountDoc = null;
            try
            {
                xmlAmountDoc = dsAmount.GetXml();
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
            return xmlAmountDoc;
        }
    }
}
