using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class FeederAccountManager
    {
        /// <summary>
        /// Check if any of the new feeder names already exist in the list
        /// </summary>
        /// <param name="feederNames">list of feeder names</param>
        /// <returns>true if the feeder name is already there</returns>
        public static bool HasDuplicateFeeders(List<string> feederNames)
        {
            if (feederNames == null)
                return false;
            try
            {
                List<string> dbFeederList = FeederAccountDAL.LoadFeederNamesList();
                foreach (string feederName in dbFeederList)
                {
                    if (feederNames.Contains(feederName.ToLower()))
                        return true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Get only selected details of the accounts from the database  
        /// </summary>
        /// <returns>New datatable with selected columns</returns>
        public static DataTable GetSelectedFeederDetails(int companyID)
        {
            try
            {
                if (companyID > 0)
                {
                    DataTable dtFeederAccounts = FeederAccountDAL.LoadFeederAccountsFromDb(companyID);
                    DataTable dtCustomizedAccounts = dtFeederAccounts.Copy();
                    dtCustomizedAccounts.Columns.Remove("CompanyID");
                    return dtCustomizedAccounts;
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
        public static Dictionary<int, string> GetCurrencies()
        {
            try
            {
                Dictionary<int, string> currencyCollection = FeederAccountDAL.LoadCurrencies();
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
        /// Delete the list of selected feeders from List
        /// </summary>
        /// <param name="feederAccounts">Number of Feeders selected for deletion</param>
        /// <returns></returns>
        public static int DeleteSelectedFeeder(List<int> feederAccounts)
        {
            int i = 0;
            try
            {
                List<int> MappedFeeders = FeederAccountDAL.GetMappedAccountIDs();
                foreach (int feederID in MappedFeeders)
                {
                    if (feederAccounts.Contains(feederID))
                        feederAccounts.Remove(feederID);
                }
                i = feederAccounts.Count;
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
            try
            {
                DataSet dsFeederAccounts = new DataSet("dsFeederAccounts");
                DataTable dtFeederAccounts = new DataTable("dtFeederAccounts");
                dtFeederAccounts.Columns.Add("FeederFundID", typeof(int));
                foreach (int feederAccountID in feederAccounts)
                {
                    dtFeederAccounts.Rows.Add(feederAccountID);
                }
                dsFeederAccounts.Tables.Add(dtFeederAccounts);
                bool isPermanentDeletion = CachedDataManager.GetInstance.IsPermanentDeletionEnabled();
                FeederAccountDAL.DeleteFeederFromDB(ConvertDataSetToXml(dsFeederAccounts), isPermanentDeletion);
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
        /// Create dataset of feeder accounts to save them in the DB
        /// </summary>
        /// <param name="feederAccounts">List of feeder accounts to save </param>
        /// <returns>The dataset of feeder accounts</returns>
        public static DataSet CreateFeederAccountDataSet(List<FeederAccountItem> feederAccounts)
        {
            DataSet dsFeederAccounts = new DataSet("dsFeederAccounts");
            DataTable dtFeederAccounts = new DataTable("dtFeederAccounts");
            dtFeederAccounts.Columns.Add("FeederFundName", typeof(string));
            dtFeederAccounts.Columns.Add("FeederFundShortName", typeof(string));
            dtFeederAccounts.Columns.Add("Amount", typeof(decimal));
            dtFeederAccounts.Columns.Add("CompanyID", typeof(int));
            dtFeederAccounts.Columns.Add("Currency", typeof(int));
            //dtFeederAccounts.Columns.Add("RemainingAmount", typeof(decimal));

            try
            {
                foreach (FeederAccountItem feederAccount in feederAccounts)
                {
                    //dtFeederAccounts.Rows.Add(feederAccount.FeederAccountName, feederAccount.FeederShortName, feederAccount.FeederAmount, feederAccount.FeederCompanyId, feederAccount.FeederCurrency);   // feederAccount.FeederRemainingAmount);
                    dtFeederAccounts.Rows.Add(feederAccount.FeederAccountName, feederAccount.FeederShortName, feederAccount.FeederAmount, feederAccount.FeederCompanyId, 1);
                }
                dsFeederAccounts.Tables.Add(dtFeederAccounts);
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
            return dsFeederAccounts;
        }

        /// <summary>
        /// Create the dataset of the updated details of the feeder
        /// </summary>
        /// <param name="feederAccounts">List of feeder account items</param>
        /// <returns></returns>
        public static DataSet CreateFeederUpdateDataSet(List<FeederAccountItem> feederAccounts)
        {
            DataSet dsFeederAccounts = new DataSet("dsFeederAccounts");
            DataTable dtFeederAccounts = new DataTable("dtFeederAccounts");
            dtFeederAccounts.Columns.Add("FeederFundID", typeof(int));
            dtFeederAccounts.Columns.Add("FeederFundName", typeof(string));
            dtFeederAccounts.Columns.Add("FeederFundShortName", typeof(string));
            dtFeederAccounts.Columns.Add("Amount", typeof(decimal));
            dtFeederAccounts.Columns.Add("CompanyID", typeof(int));
            //dtFeederAccounts.Columns.Add("Currency", typeof(string));
            dtFeederAccounts.Columns.Add("RemainingAmount", typeof(decimal));

            try
            {
                foreach (FeederAccountItem feederAccount in feederAccounts)
                {
                    //dtFeederAccounts.Rows.Add(feederAccount.FeederAccountID, feederAccount.FeederAccountName, feederAccount.FeederShortName, feederAccount.FeederAmount, feederAccount.FeederCompanyId, feederAccount.FeederCurrency, feederAccount.FeederRemainingAmount);
                    dtFeederAccounts.Rows.Add(feederAccount.FeederAccountID, feederAccount.FeederAccountName, feederAccount.FeederShortName, feederAccount.FeederAmount, feederAccount.FeederCompanyId, feederAccount.FeederRemainingAmount);
                }
                dsFeederAccounts.Tables.Add(dtFeederAccounts);
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
            return dsFeederAccounts;
        }

        /// <summary>
        /// Convert data set to Xml Document
        /// </summary>
        /// <param name="ds">Data set holding the table of feeder accounts</param>
        /// <returns>xml documents</returns>
        private static string ConvertDataSetToXml(DataSet dsFeederAccounts)
        {
            string xmlFeederAccounts = null;
            try
            {
                xmlFeederAccounts = dsFeederAccounts.GetXml();
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
            return xmlFeederAccounts;
        }

        /// <summary>
        /// Sav the new feeder account in database
        /// </summary>
        /// <param name="feederAccount">objcet of feeder account type</param>
        /// <returns></returns>
        public static int SaveFeederAccounts(List<FeederAccountItem> feederAccounts)
        {
            int count = -1;
            try
            {
                DataSet dsFeederAccounts = CreateFeederAccountDataSet(feederAccounts);
                String xmlDoc = ConvertDataSetToXml(dsFeederAccounts);
                count = FeederAccountDAL.SaveFeederAccountsinDB(xmlDoc);
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
        /// Update the details of the existing feeder account
        /// </summary>
        /// <param name="feederAccounts">List of feeder accounts </param>
        public static void UpdateFeederAccounts(List<FeederAccountItem> feederAccounts)
        {
            try
            {
                DataSet dsFeederAccounts = CreateFeederUpdateDataSet(feederAccounts);
                String xmlDoc = ConvertDataSetToXml(dsFeederAccounts);
                FeederAccountDAL.UpdateFeederAccountsinDB(xmlDoc);
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
