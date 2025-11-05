using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.Admin.BLL
{
    public class PricingRuleManager
    {
        /// <summary>
        /// ID of the company
        /// </summary>
        public static int _companyID = int.MinValue;

        /// <summary>
        /// added by: Bharat raturi, 03 jun 2014
        /// purpose: store the exchanges assetwise in the cache
        /// </summary>
        private static Dictionary<int, Dictionary<int, string>> _dictAssetExchanges = new Dictionary<int, Dictionary<int, string>>();

        /// <summary>
        /// Create the datasource for the ultracombo 
        /// </summary>
        /// <returns>The Datatable of accounts</returns>
        public static DataTable GetAccounts()
        {
            DataTable dtAccounts = new DataTable();
            dtAccounts.Columns.Add("FundID", typeof(int));
            dtAccounts.Columns.Add("FundName", typeof(string));

            try
            {
                Dictionary<int, string> dicAccount = MasterFundMappingDAL.LoadAccountsFromDb(_companyID);
                foreach (int accountID in dicAccount.Keys)
                {
                    dtAccounts.Rows.Add(accountID, dicAccount[accountID]);
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
            return dtAccounts;
        }

        /// <summary>
        /// Create the datasource for the Ultragrid 
        /// </summary>
        /// <returns>Datatable holding the data</returns>
        public static DataTable GetPricingDetails()
        {
            DataTable dtPricingDetail = new DataTable();
            dtPricingDetail.Columns.Add("RuleID", typeof(int));
            dtPricingDetail.Columns.Add("Account", typeof(object));
            dtPricingDetail.Columns.Add("IsPricingPolicy", typeof(bool));
            dtPricingDetail.Columns.Add("PricingPolicy", typeof(int));
            dtPricingDetail.Columns.Add("AssetClass", typeof(object));
            dtPricingDetail.Columns.Add("Exchange", typeof(object));
            dtPricingDetail.Columns.Add("Pricing", typeof(int));
            dtPricingDetail.Columns.Add("Source", typeof(int));
            dtPricingDetail.Columns.Add("SecondarySource", typeof(int));
            dtPricingDetail.Columns.Add("RuleType", typeof(int));
            dtPricingDetail.Columns.Add("RuleTypeByTime", typeof(int));

            try
            {
                Dictionary<int, PricingRuleItem> dicPricing = PricingRuleDAL.GetPricingDetailsFromDB(_companyID);
                foreach (int ruleID in dicPricing.Keys)
                {
                    dtPricingDetail.Rows.Add(ruleID, dicPricing[ruleID].accountID,
                    dicPricing[ruleID].IsPricingPolicy, dicPricing[ruleID].PricingPolicyID,
                        dicPricing[ruleID].assetID, dicPricing[ruleID].exchangeID,
                        dicPricing[ruleID].PricingDataType, dicPricing[ruleID].SourceID,
                    dicPricing[ruleID].SecondarySourceID, dicPricing[ruleID].RuleType, dicPricing[ruleID].RuleTypeByTime);
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
            return dtPricingDetail;
        }

        /// <summary>
        /// Get the assets for the company
        /// </summary>
        /// <returns>Dictionary holding the Asset IDs and Asset names</returns>
        public static DataTable GetAssets()
        {
            DataTable dtAssets = new DataTable();
            dtAssets.Columns.Add("AssetID", typeof(int));
            dtAssets.Columns.Add("AssetName", typeof(string));

            try
            {
                Dictionary<int, string> dicAsset = PricingRuleDAL.GetAssetsFromDB();
                foreach (int assetID in dicAsset.Keys)
                {
                    dtAssets.Rows.Add(assetID, dicAsset[assetID]);
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
            return dtAssets;
        }

        /// <summary>
        /// Get the Exchanges for the company
        /// </summary>
        /// <returns>Dictionary of exchange IDs and exchange names</returns>
        public static DataTable GetExchanges()
        {
            DataTable dtExchanges = new DataTable();
            dtExchanges.Columns.Add("ExchangeID", typeof(int));
            dtExchanges.Columns.Add("DisplayName", typeof(string));

            try
            {
                Dictionary<int, string> dicExchange = PricingRuleDAL.GetExchangesFromDB();
                foreach (int assetID in dicExchange.Keys)
                {
                    dtExchanges.Rows.Add(assetID, dicExchange[assetID]);
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
            return dtExchanges;
        }

        /// <summary>
        /// Get the Pricing Data Types for the company
        /// </summary>
        /// <returns>The dictionary holding the pricing datattpe ID and the pricing data type name</returns>
        public static Dictionary<int, string> GetPricingDataTypes()
        {
            try
            {
                Dictionary<int, string> dicPricingTypes = PricingRuleDAL.GetPricingTypesFromDB();
                return dicPricingTypes;
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
        /// Get Pricing Policies
        /// </summary>
        /// <returns>The dictionary holding the pricing datattpe ID and the pricing data type name</returns>
        public static Dictionary<int, string> GetPricingPolicyList()
        {
            try
            {
                Dictionary<int, string> dicPricingTypes = PricingRuleDAL.GetPricingPolicyListFromDB();
                return dicPricingTypes;
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
        /// Get the sources for the company
        /// </summary>
        /// <returns>Dictionary of Source IDs and source names</returns>
        public static Dictionary<int, string> GetSources()
        {
            try
            {
                Dictionary<int, string> dicPricingSource = PricingRuleDAL.GetPricingSourcesFromDB();
                return dicPricingSource;
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
        /// Get the secondary sources for the company
        /// </summary>
        /// <returns>The dictionary holding the secondary source IDs and names </returns>
        public static Dictionary<int, string> GetSecondarySource()
        {
            try
            {
                //modified by omshiv, get secondary pricing source from DB
                Dictionary<int, string> dicSecondarySource = PricingRuleDAL.GetSecondaryPricingSourcesFromDB();
                return dicSecondarySource;
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
        /// Create the dataset of the pricing details
        /// </summary>
        /// <param name="listPricing"></param>
        /// <returns>The dataset holding the pricing details</returns>
        public static DataSet CreatePricingDataSet(List<List<int>> listPricing)
        {
            DataSet dsPricing = new DataSet("dsPricing");
            List<string> ruleKeys = new List<string>();
            DataTable dtPricing = new DataTable("dtPricing");
            dtPricing.Columns.Add("PricingRuleID", typeof(int));
            dtPricing.Columns.Add("CompanyAccountID", typeof(int));
            dtPricing.Columns.Add("AssetClassID", typeof(int));
            dtPricing.Columns.Add("ExchangeID", typeof(int));
            dtPricing.Columns.Add("PricingDataType", typeof(int));
            dtPricing.Columns.Add("SourceID", typeof(int));
            dtPricing.Columns.Add("SecondarySourceID", typeof(int));
            dtPricing.Columns.Add("CompanyID", typeof(int));
            dtPricing.Columns.Add("RuleType", typeof(int));
            dtPricing.Columns.Add("RuleTypeByTime", typeof(int));
            dtPricing.Columns.Add("IsPricingPolicy", typeof(bool));
            dtPricing.Columns.Add("PricingPolicyID", typeof(int));

            try
            {
                foreach (List<int> priceRecord in listPricing)
                {

                    string pKey = priceRecord[9].ToString() + ":" + priceRecord[10].ToString() + ":" + priceRecord[1].ToString() + ":" + priceRecord[4].ToString() + ":" + priceRecord[5].ToString();

                    if (ruleKeys.Contains(pKey))
                    {
                        dsPricing.DataSetName = "Duplicate";
                        return dsPricing;
                    }
                    else
                    {
                        ruleKeys.Add(pKey);
                    }

                    dtPricing.Rows.Add(priceRecord[0], priceRecord[1], priceRecord[4],
                                  priceRecord[5], priceRecord[6], priceRecord[7], priceRecord[8], _companyID, priceRecord[9], priceRecord[10], priceRecord[2], priceRecord[3]);

                }

                dsPricing.Tables.Add(dtPricing);
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
            return dsPricing;
        }

        /// <summary>
        /// Create the XML representation of the dataset holding the details
        /// </summary>
        /// <param name="dsPricing">Dataset holding the details </param>
        /// <returns>XML representation of the data</returns>
        public static string CreatePricingXML(DataSet dsPricing)
        {
            string pricingXML = null;
            try
            {
                pricingXML = dsPricing.GetXml();
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
            return pricingXML;
        }

        /// <summary>
        /// Save the pricing details to the database
        /// </summary>
        /// <param name="pricingDetailsList">List holding the lists of rules</param>
        /// <returns></returns>
        public static bool SavePricingRules(List<List<int>> pricingDetailsList, List<int> deletedRuleIds)
        {
            try
            {
                DataSet dsPricing = CreatePricingDataSet(pricingDetailsList);
                if (dsPricing.DataSetName == "Duplicate")
                {
                    return false;
                }

                String pricingXML = CreatePricingXML(dsPricing);
                String deletedRulesXml = String.Join(",", deletedRuleIds);

                PricingRuleDAL.SavePricingDetailsInDB(pricingXML, _companyID, deletedRulesXml);
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
            return true;
        }

        /// <summary>
        /// returns company wise list of pricing id
        /// </summary>
        /// <param name="pricingDetailsList">List holding the lists of rules</param>
        /// <returns></returns>
        public static List<int> GetCompanywisePricingRuleID(List<List<int>> pricingDetailsList, int companyID)
        {
            List<int> pricingRuleIdList = new List<int>();
            try
            {
                DataSet dsPricing = CreatePricingDataSet(pricingDetailsList);
                DataTable dtPricing = dsPricing.Tables[0];

                pricingRuleIdList = (from row in dtPricing.AsEnumerable()
                                     where row.Field<int>("CompanyID") == companyID
                                     select row.Field<int>("PricingRuleID")).Distinct().ToList();

                return pricingRuleIdList;
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
            return pricingRuleIdList;
        }

        /// <summary>
        /// added by: Bharat Raturi, 03 jun 2014
        /// </summary>
        /// <param name="assets">List of Asset IDs</param>
        /// <returns>Datatable holding the exchange id, names</returns>
        public static DataTable GetCurrentAssetExchanges(List<object> assets)
        {
            DataTable dtExchanges = new DataTable();
            dtExchanges.Columns.Add("ExchangeID", typeof(int));
            dtExchanges.Columns.Add("ExchangeName", typeof(string));
            dtExchanges.PrimaryKey = new DataColumn[] { dtExchanges.Columns["ExchangeID"] };
            try
            {
                foreach (int assetID in assets)
                {
                    if (_dictAssetExchanges.ContainsKey(assetID))
                    {
                        foreach (int exchangeID in _dictAssetExchanges[assetID].Keys)
                        {
                            if (!string.IsNullOrWhiteSpace(_dictAssetExchanges[assetID][exchangeID]) && dtExchanges.Rows.Find(exchangeID) == null)
                            {
                                dtExchanges.Rows.Add(exchangeID, _dictAssetExchanges[assetID][exchangeID]);
                            }
                        }
                    }
                }
                dtExchanges.DefaultView.Sort = "ExchangeName";
                return dtExchanges;
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
        /// added by: Bharat Raturi, 03 jun 2014
        ///create the dictionary of exchanges asset wise
        /// </summary>
        public static void GetAssetExchanges()
        {
            try
            {
                DataTable dt = PricingRuleDAL.GetExchangesAssetWiseFromDB();
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrWhiteSpace(dr[0].ToString()) && !string.IsNullOrWhiteSpace(dr[1].ToString()) && !string.IsNullOrWhiteSpace(dr[2].ToString()))
                        {
                            int assetID = Convert.ToInt32(dr[0]);
                            int exchangeID = Convert.ToInt32(dr[1]);
                            string exchangeName = Convert.ToString(dr[2]);

                            if (_dictAssetExchanges.ContainsKey(assetID))
                            {
                                if (!_dictAssetExchanges[assetID].ContainsKey(exchangeID))
                                {
                                    _dictAssetExchanges[assetID].Add(exchangeID, exchangeName);
                                }
                            }
                            else
                            {
                                Dictionary<int, string> dictExchange = new Dictionary<int, string>();
                                dictExchange.Add(exchangeID, exchangeName);
                                _dictAssetExchanges.Add(assetID, dictExchange);
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



        public static DataSet GetPricingPolicies()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = PricingRuleDAL.GetPricingPoliciesFromDB();
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
            return ds;
        }
        /// <summary>
        /// Save Pricing Policy
        /// </summary>
        /// <param name="pricingDetailsList"></param>
        /// <param name="deletedRuleIds"></param>
        /// <returns></returns>
        public static int SavePricingPolicy(DataTable dt, List<int> deletedRuleIds)
        {
            int result = 1;
            try
            {
                DataSet dsPricing = CreatePricingPolicyDataSet(dt);
                if (dsPricing.DataSetName != "dsPricing")
                {
                    result = -1;
                }
                String pricingXML = CreatePricingXML(dsPricing);
                String deletedRulesXml = String.Join(",", deletedRuleIds);

                result = PricingRuleDAL.SavePricingPolicyInDB(pricingXML, deletedRulesXml);
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
        /// Create Pricing Policy Data Set
        /// </summary>
        /// <param name="listPricing"></param>
        /// <returns></returns>
        public static DataSet CreatePricingPolicyDataSet(DataTable dt)
        {
            DataSet dsPricing = new DataSet("dsPricing");
            //List<string> ruleKeys = new List<string>();
            DataTable dtPricing = new DataTable("dtPricing");
            dtPricing.Columns.Add("Id", typeof(int));
            dtPricing.Columns.Add("IsActive", typeof(bool));
            dtPricing.Columns.Add("PolicyName", typeof(string));
            dtPricing.Columns.Add("SPName", typeof(string));
            dtPricing.Columns.Add("IsFileAvailable", typeof(bool));
            dtPricing.Columns.Add("FilePath", typeof(string));
            dtPricing.Columns.Add("FolderPath", typeof(string));
            dtPricing.Columns.Add("IsModified", typeof(bool));

            try
            {
                foreach (DataRow dtrow in dt.Rows)
                {

                    if (dtrow.RowState == DataRowState.Modified || dtrow.RowState == DataRowState.Added)
                    {
                        bool isModified = true;
                        if (dtrow.RowState == DataRowState.Modified) { isModified = true; }
                        else { isModified = false; }
                        dtPricing.Rows.Add(dtrow[0], dtrow[1], dtrow[2], dtrow[3], dtrow[4], dtrow[5], dtrow[6], isModified);
                    }

                }

                dsPricing.Tables.Add(dtPricing);
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
            return dsPricing;
        }
    }
}
