using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    internal class PricingRuleDAL
    {
        /// <summary>
        /// COnnection string for the database
        /// </summary>
        static string connStr = "PranaConnectionString";

        /// <summary>
        /// Get the assets from database
        /// </summary>
        /// <returns>The dictionary holding the asset ID and the asset name</returns>
        internal static Dictionary<int, string> GetAssetsFromDB()
        {
            Dictionary<int, string> dicAsset = new Dictionary<int, string>();

            try
            {
                //modified by: Bharat Raturi, 04 jun 2014
                //purpose: change the stored procedure that ignores the asset 'cash' while defining pricing rule
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllAssetsForPricingRules";

                using (IDataReader drAsset = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drAsset.Read())
                    {
                        dicAsset.Add(drAsset.GetInt32(0), drAsset.GetString(1));
                    }
                }
                return dicAsset;
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
        /// Get the details of exchanges from database
        /// </summary>
        /// <returns> the dictionary of exchange IDs and names</returns>
        internal static Dictionary<int, string> GetExchangesFromDB()
        {
            Dictionary<int, string> dicExchange = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllExchanges";

                using (IDataReader drExchange = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drExchange.Read())
                    {
                        dicExchange.Add(drExchange.GetInt32(0), drExchange.GetString(2));
                    }
                }
                return dicExchange;
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
        /// Get the details of exchanges from database
        /// </summary>
        /// <returns> the datatable holding the exchanges assetwise</returns>
        internal static DataTable GetExchangesAssetWiseFromDB()
        {
            DataTable dtExchanges = new DataTable();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetExchangesAssetWise";

                dtExchanges = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, connStr).Tables[0];
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
        /// Get Pricing Policies from db
        /// </summary>
        /// <returns></returns>
        internal static DataSet GetPricingPoliciesFromDB()
        {
            DataSet fillPricingPolicy = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetPricingPolicy";

                fillPricingPolicy = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, connStr);
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
            return fillPricingPolicy;
        }
        /// <summary>
        /// Get the pricing types from database
        /// </summary>
        /// <returns>The dictionary of priving type IDs and names</returns>
        internal static Dictionary<int, string> GetPricingTypesFromDB()
        {
            Dictionary<int, string> dicPricingType = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllPricingTypes";

                using (IDataReader drPricingType = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drPricingType.Read())
                    {
                        dicPricingType.Add(drPricingType.GetInt32(0), drPricingType.GetString(1));
                    }
                }
                return dicPricingType;
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
        /// Get the sources from database
        /// </summary>
        /// <returns>Dictionary of source IDs and names</returns>
        internal static Dictionary<int, string> GetPricingSourcesFromDB()
        {
            Dictionary<int, string> dicSource = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllPricingSources";

                using (IDataReader drSource = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drSource.Read())
                    {
                        dicSource.Add(drSource.GetInt32(0), drSource.GetString(1));
                    }
                }
                return dicSource;
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
        /// Get the Secondary sources from database
        /// omshiv, june 2014
        /// </summary>
        /// <returns>Dictionary of source IDs and names</returns>
        internal static Dictionary<int, string> GetSecondaryPricingSourcesFromDB()
        {
            Dictionary<int, string> dicSource = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllSecondaryPricingSources";

                using (IDataReader drSource = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drSource.Read())
                    {
                        dicSource.Add(drSource.GetInt32(0), drSource.GetString(1));
                    }
                }
                return dicSource;
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
        /// Get the details of the pricing rules from database
        /// </summary>
        /// <param name="companyID">Integer ID of the company</param>
        /// <returns>THe dictionary holding the pricing details</returns>
        internal static Dictionary<int, PricingRuleItem> GetPricingDetailsFromDB(int companyID)
        {
            Dictionary<int, PricingRuleItem> dicPricing = new Dictionary<int, PricingRuleItem>();
            try
            {
                object[] param = { companyID };
                string sProc = "P_GetAllPricingDetails";
                using (IDataReader drPricing = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param, connStr))
                {
                    while (drPricing.Read())
                    {

                        int ruleID = drPricing.GetInt32(0);
                        bool isPricingPolicy = drPricing.GetBoolean(10);
                        if (!isPricingPolicy)
                        {
                            if (dicPricing.ContainsKey(ruleID))
                            {
                                if (!dicPricing[ruleID].accountID.Contains(drPricing.GetInt32(1)))
                                {
                                    dicPricing[ruleID].accountID.Add(drPricing.GetInt32(1));
                                }
                                if (!dicPricing[ruleID].assetID.Contains(drPricing.GetInt32(2)))
                                {
                                    dicPricing[ruleID].assetID.Add(drPricing.GetInt32(2));
                                }
                                if (!dicPricing[ruleID].exchangeID.Contains(drPricing.GetInt32(3)))
                                {
                                    dicPricing[ruleID].exchangeID.Add(drPricing.GetInt32(3));
                                }
                                dicPricing[ruleID].PricingDataType = drPricing.GetInt32(4);
                                dicPricing[ruleID].SourceID = drPricing.GetInt32(5);
                                dicPricing[ruleID].SecondarySourceID = drPricing.GetInt32(6);
                                dicPricing[ruleID].RuleType = drPricing.GetInt32(8);
                                dicPricing[ruleID].RuleTypeByTime = drPricing.GetInt32(9);
                                dicPricing[ruleID].IsPricingPolicy = drPricing.GetBoolean(10);
                                dicPricing[ruleID].PricingPolicyID = drPricing.GetInt32(11);

                            }
                            else
                            {
                                PricingRuleItem pRule = new PricingRuleItem();
                                pRule.accountID.Add(drPricing.GetInt32(1));
                                pRule.assetID.Add(drPricing.GetInt32(2));
                                pRule.exchangeID.Add(drPricing.GetInt32(3));
                                pRule.PricingDataType = drPricing.GetInt32(4);
                                pRule.SourceID = drPricing.GetInt32(5);
                                pRule.SecondarySourceID = drPricing.GetInt32(6);
                                pRule.RuleType = drPricing.GetInt32(8);
                                pRule.RuleTypeByTime = drPricing.GetInt32(9);
                                pRule.IsPricingPolicy = drPricing.GetBoolean(10);
                                pRule.PricingPolicyID = drPricing.GetInt32(11);
                                dicPricing.Add(drPricing.GetInt32(0), pRule);
                            }
                        }
                        else
                        {
                            if (dicPricing.ContainsKey(ruleID))
                            {
                                if (!dicPricing[ruleID].accountID.Contains(drPricing.GetInt32(1)))
                                {
                                    dicPricing[ruleID].accountID.Add(drPricing.GetInt32(1));
                                }
                                if (!dicPricing[ruleID].assetID.Contains(drPricing.GetInt32(2)))
                                {
                                    dicPricing[ruleID].assetID.Add(drPricing.GetInt32(2));
                                }
                                if (!dicPricing[ruleID].exchangeID.Contains(drPricing.GetInt32(3)))
                                {
                                    dicPricing[ruleID].exchangeID.Add(drPricing.GetInt32(3));
                                }
                                dicPricing[ruleID].PricingDataType = drPricing.GetInt32(4);
                                dicPricing[ruleID].SourceID = drPricing.GetInt32(5);
                                dicPricing[ruleID].SecondarySourceID = drPricing.GetInt32(6);
                                dicPricing[ruleID].RuleType = drPricing.GetInt32(8);
                                dicPricing[ruleID].RuleTypeByTime = drPricing.GetInt32(9);
                                dicPricing[ruleID].IsPricingPolicy = drPricing.GetBoolean(10);
                                dicPricing[ruleID].PricingPolicyID = drPricing.GetInt32(11);
                            }
                            else
                            {
                                PricingRuleItem pRule = new PricingRuleItem();
                                pRule.accountID.Add(drPricing.GetInt32(1));
                                pRule.assetID.Add(drPricing.GetInt32(2));
                                pRule.exchangeID.Add(drPricing.GetInt32(3));
                                pRule.PricingDataType = drPricing.GetInt32(4);
                                pRule.SourceID = drPricing.GetInt32(5);
                                pRule.SecondarySourceID = drPricing.GetInt32(6);
                                pRule.RuleType = drPricing.GetInt32(8);
                                pRule.RuleTypeByTime = drPricing.GetInt32(9);
                                pRule.IsPricingPolicy = drPricing.GetBoolean(10);
                                pRule.PricingPolicyID = drPricing.GetInt32(11);
                                dicPricing.Add(drPricing.GetInt32(0), pRule);
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
            return dicPricing;
        }

        /// <summary>
        /// Save the pricing details in the database
        /// </summary>
        /// <param name="xmlPricingDetails">XML representation of the details</param>
        /// <param name="companyID">ID of the company</param>
        public static void SavePricingDetailsInDB(string xmlPricingDetails, int companyID, String deletedRuleIds)
        {
            object[] parameter = { xmlPricingDetails, companyID, deletedRuleIds };

            string sProcSaveData = "P_SaveCompanyPricingDetails";
            try
            {
                DatabaseManager.DatabaseManager.ExecuteScalar(sProcSaveData, parameter, connStr);
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
        /// Save Pricing Policy In DB
        /// </summary>
        /// <param name="xmlPricingDetails"></param>
        /// <param name="deletedIds"></param>
        public static int SavePricingPolicyInDB(string xmlPricingDetails, String deletedIds)
        {
            object[] parameter = { xmlPricingDetails, deletedIds };
            int result = 0;
            string sProcSaveData = "P_SavePricingPolicy";
            try
            {
                var res = DatabaseManager.DatabaseManager.ExecuteScalar(sProcSaveData, parameter, connStr);
                int.TryParse(res.ToString(), out result);
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

        internal static Dictionary<int, string> GetPricingPolicyListFromDB()
        {
            Dictionary<int, string> dicPricingpPolicyList = new Dictionary<int, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetPricingPolicyList";

                using (IDataReader drPricingPolicy = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drPricingPolicy.Read())
                    {
                        int ID = drPricingPolicy.GetInt32(0);
                        String policy = drPricingPolicy.GetString(1);
                        if (!dicPricingpPolicyList.ContainsKey(ID))
                        {
                            dicPricingpPolicyList.Add(ID, policy);
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
            return dicPricingpPolicyList;
        }
    }
}
