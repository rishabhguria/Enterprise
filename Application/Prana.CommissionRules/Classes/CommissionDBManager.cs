#region Using namespaces

using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace Prana.CommissionRules
{
    /// <summary>
    /// Summary description for CommissionRuleManager.
    /// </summary>
    public class CommissionDBManager
    {
        private static int _errorNumber = 0;

        private static string _errorMessage = string.Empty;
        public CommissionDBManager()
        {
        }

        #region New Commission Rule Region

        #region Commission Rule Get, Save and Detele Region
        /// <summary>
        /// for save new and update existing  commission rules 
        /// </summary>
        /// <param name="commissionRuleCollection"></param>
        /// <returns></returns>
        public static int SaveAndUpdateCommissionRule(List<CommissionRule> commissionRuleCollection)
        {
            int rowsAffected = 0;
            string commissionRuleCollectionXML = XMLUtilities.SerializeToXML(commissionRuleCollection);
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveAndUpdateCommissionRules";

                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = commissionRuleCollectionXML
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;

        }

        private static CommissionRule FillCommissionRule(object[] row, int offSet)
        {
            int RuleID = 0 + offSet;
            int RuleName = 1 + offSet;
            int RuleDescription = 2 + offSet;
            int ApllyRuleForTrade = 3 + offSet;
            int CalculationBasedOn = 4 + offSet;
            int CommissionRate = 5 + offSet;
            int MinCommission = 6 + offSet;
            int IsCriteriaApplied = 7 + offSet;
            int IsClearingFeeApplied = 8 + offSet;
            int CalculationBasedOnClearing = 9 + offSet;
            int CommissionRateClearing = 10 + offSet;
            int MinimumCommissionClearing = 11 + offSet;
            int MaxCommission = 12 + offSet;
            int IsRoundOff = 13 + offSet;
            int RoundOffValue = 14 + offSet;
            int CalculationBasedOnForSoft = 15 + offSet;
            int CommissionRateForSoft = 16 + offSet;
            int MinCommissionForSoft = 17 + offSet;
            int MaxCommissionForSoft = 18 + offSet;
            int IsCriteriaAppliedForSoft = 19 + offSet;
            int IsRoundOffForSoft = 20 + offSet;
            int RoundOffValueForSoft = 21 + offSet;
            int IsClearingBrokerFeeApplied = 22 + offSet;
            int CalculationBasedOnClearingBrokerFee = 23 + offSet;
            int CommissionRateClearingBrokerFee = 24 + offSet;
            int MinimumCommissionClearingBrokerFee = 25 + offSet;

            CommissionRule commissionRule = new CommissionRule();
            try
            {
                if (row[RuleID] != null)
                {
                    commissionRule.RuleID = (Guid)row[RuleID];
                }
                if (row[RuleName] != null)
                {
                    commissionRule.RuleName = row[RuleName].ToString();
                }
                if (row[RuleDescription] != null && row[RuleDescription] != System.DBNull.Value)
                {
                    commissionRule.RuleDescription = row[RuleDescription].ToString();
                }
                if (row[ApllyRuleForTrade] != null)
                {
                    commissionRule.ApplyRuleForTrade = (TradeType)(row[ApllyRuleForTrade]);
                }
                if (row[CalculationBasedOn] != null)
                {
                    commissionRule.Commission.RuleAppliedOn = (CalculationBasis)row[CalculationBasedOn];
                }
                if (row[CommissionRate] != null)
                {
                    commissionRule.Commission.CommissionRate = double.Parse(row[CommissionRate].ToString());
                }
                if (row[MinCommission] != null)
                {
                    commissionRule.Commission.MinCommission = double.Parse(row[MinCommission].ToString());
                }
                if (row[IsCriteriaApplied] != null)
                {
                    commissionRule.Commission.IsCriteriaApplied = bool.Parse(row[IsCriteriaApplied].ToString());
                }
                if (row[IsClearingFeeApplied] != null)
                {
                    commissionRule.IsClearingFeeApplied = bool.Parse(row[IsClearingFeeApplied].ToString());
                }
                if (row[CalculationBasedOnClearing] != null && row[CalculationBasedOnClearing] != System.DBNull.Value)
                {
                    commissionRule.ClearingFeeObj.RuleAppliedOn = (CalculationBasis)row[CalculationBasedOnClearing];
                }
                if (row[CommissionRateClearing] != null && row[CommissionRateClearing] != System.DBNull.Value)
                {
                    commissionRule.ClearingFeeObj.ClearingFeeRate = double.Parse(row[CommissionRateClearing].ToString());
                }
                if (row[MinimumCommissionClearing] != null && row[MinimumCommissionClearing] != System.DBNull.Value)
                {
                    commissionRule.ClearingFeeObj.MinClearingFee = double.Parse(row[MinimumCommissionClearing].ToString());
                }
                if (row[MaxCommission] != null && row[MaxCommission] != System.DBNull.Value)
                {
                    commissionRule.Commission.MaxCommission = double.Parse(row[MaxCommission].ToString());
                }
                if (row[IsRoundOff] != null && row[IsRoundOff] != System.DBNull.Value)
                {
                    commissionRule.Commission.IsRoundOff = bool.Parse(row[IsRoundOff].ToString());
                }
                if (row[RoundOffValue] != null && row[RoundOffValue] != System.DBNull.Value)
                {
                    commissionRule.Commission.RoundOffValue = int.Parse(row[RoundOffValue].ToString());
                }

                //Fields for Soft Commission
                if (row[CalculationBasedOnForSoft] != null)
                {
                    commissionRule.SoftCommission.RuleAppliedOn = (CalculationBasis)row[CalculationBasedOnForSoft];
                }
                if (row[CommissionRateForSoft] != null)
                {
                    commissionRule.SoftCommission.CommissionRate = double.Parse(row[CommissionRateForSoft].ToString());
                }
                if (row[MinCommissionForSoft] != null)
                {
                    commissionRule.SoftCommission.MinCommission = double.Parse(row[MinCommissionForSoft].ToString());
                }
                if (row[IsCriteriaAppliedForSoft] != null)
                {
                    commissionRule.SoftCommission.IsCriteriaApplied = bool.Parse(row[IsCriteriaAppliedForSoft].ToString());
                }
                if (row[MaxCommissionForSoft] != null && row[MaxCommissionForSoft] != System.DBNull.Value)
                {
                    commissionRule.SoftCommission.MaxCommission = double.Parse(row[MaxCommissionForSoft].ToString());
                }
                if (row[IsRoundOffForSoft] != null && row[IsRoundOffForSoft] != System.DBNull.Value)
                {
                    commissionRule.SoftCommission.IsRoundOff = bool.Parse(row[IsRoundOffForSoft].ToString());
                }
                if (row[RoundOffValueForSoft] != null && row[RoundOffValueForSoft] != System.DBNull.Value)
                {
                    commissionRule.SoftCommission.RoundOffValue = int.Parse(row[RoundOffValueForSoft].ToString());
                }

                //Fields for Clearing Broker Fee
                if (row[IsClearingBrokerFeeApplied] != null)
                {
                    commissionRule.IsClearingBrokerFeeApplied = bool.Parse(row[IsClearingBrokerFeeApplied].ToString());
                }
                if (row[CalculationBasedOnClearingBrokerFee] != null && row[CalculationBasedOnClearingBrokerFee] != System.DBNull.Value)
                {
                    commissionRule.ClearingBrokerFeeObj.RuleAppliedOn = (CalculationBasis)row[CalculationBasedOnClearingBrokerFee];
                }
                if (row[CommissionRateClearingBrokerFee] != null && row[CommissionRateClearingBrokerFee] != System.DBNull.Value)
                {
                    commissionRule.ClearingBrokerFeeObj.ClearingFeeRate = double.Parse(row[CommissionRateClearingBrokerFee].ToString());
                }
                if (row[MinimumCommissionClearingBrokerFee] != null && row[MinimumCommissionClearingBrokerFee] != System.DBNull.Value)
                {
                    commissionRule.ClearingBrokerFeeObj.MinClearingFee = double.Parse(row[MinimumCommissionClearingBrokerFee].ToString());
                }
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
            return commissionRule;
        }
        /// <summary>
        /// for getting  All save commission rule to generate list for calculation of commission 
        /// </summary>
        /// <returns></returns>
        private static List<CommissionRule> GetCommissionRules()
        {
            List<CommissionRule> commissionRuleList = new List<CommissionRule>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCommissionRules";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionRuleList.Add(FillCommissionRule(row, 0));
                    }
                }
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
            return commissionRuleList;
        }

        private static AssetCategory FillAssetCategory(object[] row, int offSet)
        {
            int AssetId = 0 + offSet;

            AssetCategory assetId = new AssetCategory();
            try
            {
                if (row[AssetId] != null && row[AssetId] != System.DBNull.Value)
                {
                    assetId = (AssetCategory)row[AssetId];
                }
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
            return assetId;
        }
        /// <summary>
        /// for getting  assets permitted for particular rule
        /// </summary>
        /// <param name="commRuleId"></param>
        /// <returns></returns>
        private static List<AssetCategory> GetCommissionRuleAssets(Guid commRuleId)
        {
            List<AssetCategory> commissionRuleList = new List<AssetCategory>();

            Object[] parameter = new object[1];
            parameter[0] = commRuleId;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCommissionRuleAssets", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionRuleList.Add(FillAssetCategory(row, 0));
                    }
                }

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
            return commissionRuleList;

        }
        /// <summary>
        /// to get the specefied criteria for selected rule
        /// </summary>
        /// <param name="commissionRuleId"></param>
        /// <returns></returns>
        public static List<ClearingFeeCriteria> GetClearingCriterias(Guid commissionRuleId)
        {
            List<ClearingFeeCriteria> commissionRuleCriteriaColl = new List<ClearingFeeCriteria>();
            Object[] parameter = new object[1];
            parameter[0] = commissionRuleId;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetClearingFeeCriterias", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionRuleCriteriaColl.Add(FillClearingFeeCriteria(row, 0));
                    }
                }
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
            return commissionRuleCriteriaColl;
        }

        /// <summary>
        /// to get the specefied criteria for selected rule
        /// </summary>
        /// <param name="commissionRuleId"></param>
        /// <returns></returns>
        public static List<CommissionRuleCriteria> GetCommissionRuleCriterias(Guid commissionRuleId)
        {
            List<CommissionRuleCriteria> commissionRuleCriteriaColl = new List<CommissionRuleCriteria>();
            Object[] parameter = new object[1];
            parameter[0] = commissionRuleId;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCommissionRuleCriterias", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionRuleCriteriaColl.Add(FillCommissionRuleCriteria(row, 0));
                    }
                }
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
            return commissionRuleCriteriaColl;
        }

        private static CommissionRuleCriteria FillCommissionRuleCriteria(object[] row, int offSet)
        {
            int CommissionCriteriaId = 0 + offSet;
            int ValueGreaterThan = 1 + offSet;
            int ValueLessThanOrEqualTo = 2 + offSet;
            int CommissionRate = 3 + offSet;
            int CommissionType = 4 + offSet;

            CommissionRuleCriteria commissionRuleCriteria = new CommissionRuleCriteria();
            try
            {
                if (row[CommissionCriteriaId] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionCriteriaId = int.Parse(row[CommissionCriteriaId].ToString());
                }
                if (row[ValueGreaterThan] != System.DBNull.Value)
                {
                    commissionRuleCriteria.ValueGreaterThan = Convert.ToDouble(row[ValueGreaterThan].ToString());
                }

                if (row[ValueLessThanOrEqualTo] != System.DBNull.Value)
                {
                    commissionRuleCriteria.ValueLessThanOrEqual = Convert.ToDouble(row[ValueLessThanOrEqualTo].ToString());
                }
                if (row[CommissionRate] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionRate = Convert.ToDouble(row[CommissionRate].ToString());
                }
                if (row[CommissionType] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionType = (CommissionType)row[CommissionType];
                }
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
            return commissionRuleCriteria;
        }

        /// <summary>
        /// Fills the clearing fee criteria.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offSet">The off set.</param>
        /// <returns></returns>
        private static ClearingFeeCriteria FillClearingFeeCriteria(object[] row, int offSet)
        {
            int CommissionCriteriaId = 0 + offSet;
            int ValueGreaterThan = 1 + offSet;
            int ValueLessThanOrEqualTo = 2 + offSet;
            int CommissionRate = 3 + offSet;
            int CommissionType = 4 + offSet;

            ClearingFeeCriteria clearingFeeCriteria = new ClearingFeeCriteria();
            try
            {
                if (row[CommissionCriteriaId] != System.DBNull.Value)
                {
                    clearingFeeCriteria.ClearingFeeCriteriaId = int.Parse(row[CommissionCriteriaId].ToString());
                }
                if (row[ValueGreaterThan] != System.DBNull.Value)
                {
                    clearingFeeCriteria.ValueGreaterThan = Convert.ToDouble(row[ValueGreaterThan].ToString());
                }

                if (row[ValueLessThanOrEqualTo] != System.DBNull.Value)
                {
                    clearingFeeCriteria.ValueLessThanOrEqual = Convert.ToDouble(row[ValueLessThanOrEqualTo].ToString());
                }
                if (row[CommissionRate] != System.DBNull.Value)
                {
                    clearingFeeCriteria.ClearingFeeRate = Convert.ToDouble(row[CommissionRate].ToString());
                }
                if (row[CommissionType] != System.DBNull.Value)
                {
                    clearingFeeCriteria.ClearingFeeType = (ClearingFeeType)row[CommissionType];
                }
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
            return clearingFeeCriteria;
        }

        public static void GetAllSavedCommissionRules()
        {
            try
            {
                CommissionRulesCacheManager.GetInstance().ClearAllCommissionRuleCollections();
                DataSet commissionRuleDetails = GetCommissionRuleDetails();

                Dictionary<string, List<AssetCategory>> dictRuleAssets = new Dictionary<string, List<AssetCategory>>();
                Dictionary<string, List<CommissionRuleCriteria>> dictRuleCriterias = new Dictionary<string, List<CommissionRuleCriteria>>();
                Dictionary<string, List<CommissionRuleCriteria>> dictRuleCriteriasForSoft = new Dictionary<string, List<CommissionRuleCriteria>>();
                Dictionary<string, List<ClearingFeeCriteria>> dictRuleCriteriasForClearingFee = new Dictionary<string, List<ClearingFeeCriteria>>();
                Dictionary<string, List<ClearingFeeCriteria>> dictRuleCriteriasForClearingBrokerFee = new Dictionary<string, List<ClearingFeeCriteria>>();

                if (commissionRuleDetails != null && commissionRuleDetails.Tables.Count > 0)
                {
                    DataTable commissionRules = commissionRuleDetails.Tables[0];
                    DataTable commissionRuleAssets = commissionRuleDetails.Tables[1];
                    DataTable commissionRuleCriterias = commissionRuleDetails.Tables[2];
                    DataTable clearingFeeCriterias = commissionRuleDetails.Tables[3];

                    int RuleID = 3;
                    int AssetID = 2;
                    int CommissionType = 5;
                    int clearingFeeType = 5;
                    if (commissionRuleAssets != null && commissionRuleAssets.Rows.Count > 0)
                    {
                        foreach (DataRow drow in commissionRuleAssets.Rows)
                        {
                            try
                            {
                                if (!dictRuleAssets.ContainsKey(drow[RuleID].ToString()))
                                {
                                    dictRuleAssets.Add(drow[RuleID].ToString(), new List<AssetCategory> { (AssetCategory)Enum.Parse(typeof(AssetCategory), drow[AssetID].ToString(), true) });
                                }
                                else
                                {
                                    dictRuleAssets[drow[RuleID].ToString()].Add((AssetCategory)Enum.Parse(typeof(AssetCategory), drow[AssetID].ToString(), true));
                                }
                            }
                            catch (Exception ex)
                            {
                                // If there is any error in any rule then it won't bother the execution of other rules.
                                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                                if (rethrow)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    if (commissionRuleCriterias != null && commissionRuleCriterias.Rows.Count > 0)
                    {
                        foreach (DataRow drow in commissionRuleCriterias.Rows)
                        {
                            try
                            {
                                if (drow[CommissionType] != System.DBNull.Value && (CommissionType)drow[CommissionType] == Prana.BusinessObjects.AppConstants.CommissionType.Commission)
                                {
                                    if (!dictRuleCriterias.ContainsKey(drow[RuleID].ToString()))
                                    {
                                        dictRuleCriterias.Add(drow[RuleID].ToString(), new List<CommissionRuleCriteria> { FillCommissionRuleCriteria(drow, 0) });
                                    }
                                    else
                                    {
                                        dictRuleCriterias[drow[RuleID].ToString()].Add(FillCommissionRuleCriteria(drow, 0));
                                    }
                                }
                                else if (drow[CommissionType] != System.DBNull.Value && (CommissionType)drow[CommissionType] == Prana.BusinessObjects.AppConstants.CommissionType.SoftCommission)
                                {
                                    if (!dictRuleCriteriasForSoft.ContainsKey(drow[RuleID].ToString()))
                                    {
                                        dictRuleCriteriasForSoft.Add(drow[RuleID].ToString(), new List<CommissionRuleCriteria> { FillCommissionRuleCriteria(drow, 0) });
                                    }
                                    else
                                    {
                                        dictRuleCriteriasForSoft[drow[RuleID].ToString()].Add(FillCommissionRuleCriteria(drow, 0));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // If there is any error in any rule then it won't bother the execution of other rules.
                                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                                if (rethrow)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    if (clearingFeeCriterias != null && clearingFeeCriterias.Rows.Count > 0)
                    {
                        foreach (DataRow drow in clearingFeeCriterias.Rows)
                        {
                            try
                            {
                                if (drow[clearingFeeType] != System.DBNull.Value && (ClearingFeeType)drow[clearingFeeType] == Prana.BusinessObjects.AppConstants.ClearingFeeType.Broker)
                                {
                                    if (!dictRuleCriteriasForClearingBrokerFee.ContainsKey(drow[RuleID].ToString()))
                                    {
                                        dictRuleCriteriasForClearingBrokerFee.Add(drow[RuleID].ToString(), new List<ClearingFeeCriteria> { FillClearingFeeCriteria(drow, 0) });
                                    }
                                    else
                                    {
                                        dictRuleCriteriasForClearingBrokerFee[drow[RuleID].ToString()].Add(FillClearingFeeCriteria(drow, 0));
                                    }
                                }
                                else if (drow[clearingFeeType] != System.DBNull.Value && (ClearingFeeType)drow[clearingFeeType] == Prana.BusinessObjects.AppConstants.ClearingFeeType.Other)
                                {
                                    if (!dictRuleCriteriasForClearingFee.ContainsKey(drow[RuleID].ToString()))
                                    {
                                        dictRuleCriteriasForClearingFee.Add(drow[RuleID].ToString(), new List<ClearingFeeCriteria> { FillClearingFeeCriteria(drow, 0) });
                                    }
                                    else
                                    {
                                        dictRuleCriteriasForClearingFee[drow[RuleID].ToString()].Add(FillClearingFeeCriteria(drow, 0));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // If there is any error in any rule then it won't bother the execution of other rules.
                                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                                if (rethrow)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    if (commissionRules != null && commissionRules.Rows.Count > 0)
                    {
                        foreach (DataRow drow in commissionRules.Rows)
                        {
                            try
                            {
                                CommissionRule commissionRule = FillCommissionRule(drow, 0);
                                if (dictRuleAssets.ContainsKey(commissionRule.RuleID.ToString()))
                                {
                                    if (dictRuleAssets[commissionRule.RuleID.ToString()].Count > 0)
                                    {
                                        commissionRule.AssetIdList = dictRuleAssets[commissionRule.RuleID.ToString()];
                                    }
                                }
                                if (commissionRule.Commission.IsCriteriaApplied == true)
                                {
                                    if (dictRuleCriterias.ContainsKey(commissionRule.RuleID.ToString()))
                                    {
                                        if (dictRuleCriterias[commissionRule.RuleID.ToString()].Count > 0)
                                        {
                                            commissionRule.Commission.CommissionRuleCriteiaList = dictRuleCriterias[commissionRule.RuleID.ToString()];
                                        }
                                    }
                                }
                                if (commissionRule.SoftCommission.IsCriteriaApplied == true)
                                {
                                    if (dictRuleCriteriasForSoft.ContainsKey(commissionRule.RuleID.ToString()))
                                    {
                                        if (dictRuleCriteriasForSoft[commissionRule.RuleID.ToString()].Count > 0)
                                        {
                                            commissionRule.SoftCommission.CommissionRuleCriteiaList = dictRuleCriteriasForSoft[commissionRule.RuleID.ToString()];
                                        }
                                    }
                                }
                                if (commissionRule.IsClearingBrokerFeeApplied && commissionRule.ClearingBrokerFeeObj.IsCriteriaApplied == true)
                                {
                                    if (dictRuleCriteriasForClearingBrokerFee.ContainsKey(commissionRule.RuleID.ToString()))
                                    {
                                        if (dictRuleCriteriasForClearingBrokerFee[commissionRule.RuleID.ToString()].Count > 0)
                                        {
                                            commissionRule.ClearingBrokerFeeObj.ClearingFeeRuleCriteiaList = dictRuleCriteriasForClearingBrokerFee[commissionRule.RuleID.ToString()];
                                        }
                                    }
                                }
                                if (commissionRule.IsClearingFeeApplied && commissionRule.ClearingFeeObj.IsCriteriaApplied == true)
                                {
                                    if (dictRuleCriteriasForClearingFee.ContainsKey(commissionRule.RuleID.ToString()))
                                    {
                                        if (dictRuleCriteriasForClearingFee[commissionRule.RuleID.ToString()].Count > 0)
                                        {
                                            commissionRule.ClearingFeeObj.ClearingFeeRuleCriteiaList = dictRuleCriteriasForClearingFee[commissionRule.RuleID.ToString()];
                                        }
                                    }
                                }
                                CommissionRulesCacheManager.GetInstance().AddCommissionRule(commissionRule);
                            }
                            catch (Exception ex)
                            {
                                // If there is any error in any rule then it won't bother the execution of other rules.
                                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                                if (rethrow)
                                {
                                    throw;
                                }
                            }
                        }
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
        }

        private static DataSet GetCommissionRuleDetails()
        {
            DataSet commissionRulesDetails = new DataSet();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCommissionRulesDetails";

            try
            {
                commissionRulesDetails = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return commissionRulesDetails;
        }

        private static CommissionRuleCriteria FillCommissionRuleCriteria(DataRow row, int offSet)
        {
            int CommissionCriteriaId = 0 + offSet;
            int ValueGreaterThan = 1 + offSet;
            int ValueLessThanOrEqualTo = 2 + offSet;
            int CommissionRate = 4 + offSet;
            int CommissionType = 5 + offSet;

            CommissionRuleCriteria commissionRuleCriteria = new CommissionRuleCriteria();
            try
            {
                if (row[CommissionCriteriaId] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionCriteriaId = int.Parse(row[CommissionCriteriaId].ToString());
                }
                if (row[ValueGreaterThan] != System.DBNull.Value)
                {
                    commissionRuleCriteria.ValueGreaterThan = Convert.ToDouble(row[ValueGreaterThan].ToString());
                }

                if (row[ValueLessThanOrEqualTo] != System.DBNull.Value)
                {
                    commissionRuleCriteria.ValueLessThanOrEqual = Convert.ToDouble(row[ValueLessThanOrEqualTo].ToString());
                }
                if (row[CommissionRate] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionRate = Convert.ToDouble(row[CommissionRate].ToString());
                }
                if (row[CommissionType] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionType = (CommissionType)row[CommissionType];
                }
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
            return commissionRuleCriteria;
        }

        /// <summary>
        /// Fills the clearing fee criteria.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offSet">The off set.</param>
        /// <returns></returns>
        private static ClearingFeeCriteria FillClearingFeeCriteria(DataRow row, int offSet)
        {
            int clearingFeeCriteriaID = 0 + offSet;
            int valueGreaterThan = 1 + offSet;
            int valueLessThanOrEqualTo = 2 + offSet;
            int clearingFeeRate = 4 + offSet;
            int clearingFeeType = 5 + offSet;

            ClearingFeeCriteria clearingFeeCriteria = new ClearingFeeCriteria();
            try
            {
                if (row[clearingFeeCriteriaID] != System.DBNull.Value)
                {
                    clearingFeeCriteria.ClearingFeeCriteriaId = int.Parse(row[clearingFeeCriteriaID].ToString());
                }
                if (row[valueGreaterThan] != System.DBNull.Value)
                {
                    clearingFeeCriteria.ValueGreaterThan = Convert.ToDouble(row[valueGreaterThan].ToString());
                }

                if (row[valueLessThanOrEqualTo] != System.DBNull.Value)
                {
                    clearingFeeCriteria.ValueLessThanOrEqual = Convert.ToDouble(row[valueLessThanOrEqualTo].ToString());
                }
                if (row[clearingFeeRate] != System.DBNull.Value)
                {
                    clearingFeeCriteria.ClearingFeeRate = Convert.ToDouble(row[clearingFeeRate].ToString());
                }
                if (row[clearingFeeType] != System.DBNull.Value)
                {
                    clearingFeeCriteria.ClearingFeeType = (ClearingFeeType)row[clearingFeeType];
                }
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
            return clearingFeeCriteria;
        }

        private static CommissionRule FillCommissionRule(DataRow row, int offSet)
        {
            int RuleID = 0 + offSet;
            int RuleName = 1 + offSet;
            int RuleDescription = 2 + offSet;
            int ApllyRuleForTrade = 3 + offSet;
            int CalculationBasedOn = 4 + offSet;
            int CommissionRate = 5 + offSet;
            int MinCommission = 6 + offSet;
            int IsCriteriaApplied = 7 + offSet;
            int IsClearingFeeApplied = 8 + offSet;
            int CalculationBasedOnClearing = 9 + offSet;
            int CommissionRateClearing = 10 + offSet;
            int MinimumCommissionClearing = 11 + offSet;
            int MaxCommission = 12 + offSet;
            int IsRoundOff = 13 + offSet;
            int RoundOffValue = 14 + offSet;
            int CalculationBasedOnForSoft = 15 + offSet;
            int CommissionRateForSoft = 16 + offSet;
            int MinCommissionForSoft = 17 + offSet;
            int MaxCommissionForSoft = 18 + offSet;
            int IsCriteriaAppliedForSoft = 19 + offSet;
            int IsRoundOffForSoft = 20 + offSet;
            int RoundOffValueForSoft = 21 + offSet;
            int IsClearingBrokerFeeApplied = 22 + offSet;
            int CalculationBasedOnClearingBrokerFee = 23 + offSet;
            int CommissionRateClearingBrokerFee = 24 + offSet;
            int MinimumCommissionClearingBrokerFee = 25 + offSet;
            int IsCriteriaAppliedForClearingBrokerFee = 26 + offSet;
            int IsCriteriaAppliedForClearingFee = 27 + offSet;

            CommissionRule commissionRule = new CommissionRule();
            try
            {
                if (row[RuleID] != null)
                {
                    commissionRule.RuleID = (Guid)row[RuleID];
                }
                if (row[RuleName] != null)
                {
                    commissionRule.RuleName = row[RuleName].ToString();
                }
                if (row[RuleDescription] != null && row[RuleDescription] != System.DBNull.Value)
                {
                    commissionRule.RuleDescription = row[RuleDescription].ToString();
                }
                if (row[ApllyRuleForTrade] != null)
                {
                    commissionRule.ApplyRuleForTrade = (TradeType)(row[ApllyRuleForTrade]);
                }
                if (row[CalculationBasedOn] != null)
                {
                    commissionRule.Commission.RuleAppliedOn = (CalculationBasis)row[CalculationBasedOn];
                }
                if (row[CommissionRate] != null)
                {
                    commissionRule.Commission.CommissionRate = double.Parse(row[CommissionRate].ToString());
                }
                if (row[MinCommission] != null)
                {
                    commissionRule.Commission.MinCommission = double.Parse(row[MinCommission].ToString());
                }
                if (row[IsCriteriaApplied] != null)
                {
                    commissionRule.Commission.IsCriteriaApplied = bool.Parse(row[IsCriteriaApplied].ToString());
                }
                if (row[IsClearingFeeApplied] != null)
                {
                    commissionRule.IsClearingFeeApplied = bool.Parse(row[IsClearingFeeApplied].ToString());
                }
                if (row[CalculationBasedOnClearing] != null && row[CalculationBasedOnClearing] != System.DBNull.Value)
                {
                    commissionRule.ClearingFeeObj.RuleAppliedOn = (CalculationBasis)row[CalculationBasedOnClearing];
                }
                if (row[CommissionRateClearing] != null && row[CommissionRateClearing] != System.DBNull.Value)
                {
                    commissionRule.ClearingFeeObj.ClearingFeeRate = double.Parse(row[CommissionRateClearing].ToString());
                }
                if (row[MinimumCommissionClearing] != null && row[MinimumCommissionClearing] != System.DBNull.Value)
                {
                    commissionRule.ClearingFeeObj.MinClearingFee = double.Parse(row[MinimumCommissionClearing].ToString());
                }
                if (row[MaxCommission] != null && row[MaxCommission] != System.DBNull.Value)
                {
                    commissionRule.Commission.MaxCommission = double.Parse(row[MaxCommission].ToString());
                }
                if (row[IsRoundOff] != null && row[IsRoundOff] != System.DBNull.Value)
                {
                    commissionRule.Commission.IsRoundOff = bool.Parse(row[IsRoundOff].ToString());
                }
                if (row[RoundOffValue] != null && row[RoundOffValue] != System.DBNull.Value)
                {
                    commissionRule.Commission.RoundOffValue = int.Parse(row[RoundOffValue].ToString());
                }


                //Fields for Soft Commission
                if (row[CalculationBasedOnForSoft] != null)
                {
                    commissionRule.SoftCommission.RuleAppliedOn = (CalculationBasis)row[CalculationBasedOnForSoft];
                }
                if (row[CommissionRateForSoft] != null)
                {
                    commissionRule.SoftCommission.CommissionRate = double.Parse(row[CommissionRateForSoft].ToString());
                }
                if (row[MinCommissionForSoft] != null)
                {
                    commissionRule.SoftCommission.MinCommission = double.Parse(row[MinCommissionForSoft].ToString());
                }
                if (row[IsCriteriaAppliedForSoft] != null)
                {
                    commissionRule.SoftCommission.IsCriteriaApplied = bool.Parse(row[IsCriteriaAppliedForSoft].ToString());
                }
                if (row[MaxCommissionForSoft] != null && row[MaxCommissionForSoft] != System.DBNull.Value)
                {
                    commissionRule.SoftCommission.MaxCommission = double.Parse(row[MaxCommissionForSoft].ToString());
                }
                if (row[IsRoundOffForSoft] != null && row[IsRoundOffForSoft] != System.DBNull.Value)
                {
                    commissionRule.SoftCommission.IsRoundOff = bool.Parse(row[IsRoundOffForSoft].ToString());
                }
                if (row[RoundOffValueForSoft] != null && row[RoundOffValueForSoft] != System.DBNull.Value)
                {
                    commissionRule.SoftCommission.RoundOffValue = int.Parse(row[RoundOffValueForSoft].ToString());
                }

                //Fields for Clearing Broker Fee
                if (row[IsClearingBrokerFeeApplied] != null)
                {
                    commissionRule.IsClearingBrokerFeeApplied = bool.Parse(row[IsClearingBrokerFeeApplied].ToString());
                }
                if (row[CalculationBasedOnClearingBrokerFee] != null && row[CalculationBasedOnClearingBrokerFee] != System.DBNull.Value)
                {
                    commissionRule.ClearingBrokerFeeObj.RuleAppliedOn = (CalculationBasis)row[CalculationBasedOnClearingBrokerFee];
                }
                if (row[CommissionRateClearingBrokerFee] != null && row[CommissionRateClearingBrokerFee] != System.DBNull.Value)
                {
                    commissionRule.ClearingBrokerFeeObj.ClearingFeeRate = double.Parse(row[CommissionRateClearingBrokerFee].ToString());
                }
                if (row[MinimumCommissionClearingBrokerFee] != null && row[MinimumCommissionClearingBrokerFee] != System.DBNull.Value)
                {
                    commissionRule.ClearingBrokerFeeObj.MinClearingFee = double.Parse(row[MinimumCommissionClearingBrokerFee].ToString());
                }
                if (row[IsCriteriaAppliedForClearingBrokerFee] != null)
                {
                    commissionRule.ClearingBrokerFeeObj.IsCriteriaApplied = row[IsCriteriaAppliedForClearingBrokerFee].ToString() == "1" ? true : false;
                }
                if (row[IsCriteriaAppliedForClearingFee] != null)
                {
                    commissionRule.ClearingFeeObj.IsCriteriaApplied = row[IsCriteriaAppliedForClearingFee].ToString() == "1" ? true : false;
                }
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
            return commissionRule;
        }

        /// <summary>
        /// delete selected commission rule by ruleId
        /// </summary>
        /// <param name="commissionRuleId"></param>
        /// <returns></returns>
        public static int DeleteCommissionRule(Guid commissionRuleId)
        {
            int result = 0;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = commissionRuleId;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteCommRule", parameter).ToString());

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
        /// commission calculation methodlogy Pre or Post Allocation 
        /// </summary>
        /// <returns></returns>

        public static bool GetCommissionCalculationTime()
        {

            bool result = false;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCommissionCalculationTime";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    if (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int offSet = 0;
                        if (row[offSet] != System.DBNull.Value)
                        {
                            result = bool.Parse(row[offSet].ToString());
                        }
                    }
                }

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


        #endregion Commission Rule Get, Save and Detele Region

        #region CommissionRulesForCVAUEC

        /// <summary>
        /// get All the saved commission rules for Particaualr Counter party Venue
        /// </summary>
        static CommissionRulesCacheManager _commissionRulesCacheManager = CommissionRulesCacheManager.GetInstance();
        static List<CVAUECAccountCommissionRule> _cvUECAccountCommissionRuleList = new List<CVAUECAccountCommissionRule>();
        public static void GetAllCommissionRulesForCVAUEC(int companyID)
        {
            _commissionRulesCacheManager.GetAllCVAUECAccountCommissionRules().Clear();
            _commissionRulesCacheManager.ClearCVAUECCommissionRulesDictionary();

            List<CVAUECAccountCommissionRule> cvAUECAccountCommissionRulesList = new List<CVAUECAccountCommissionRule>();
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllCommissionRulesForCVAUECForCompany", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //cvAUECAccountCommissionRulesList.Add(FillCVAUECAccountCommissionRules(row, 0));
                        _commissionRulesCacheManager.AddCVAUECRule(FillCVAUECAccountCommissionRules(row, 0));
                    }
                }

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


            //return cvAUECAccountCommissionRulesList;
        }

        private static Prana.BusinessObjects.CommissionRule _tempCommRuleSelect = new Prana.BusinessObjects.CommissionRule();
        public static CVAUECAccountCommissionRule FillCVAUECAccountCommissionRules(object[] row, int offSet)
        {
            int CVAUECRuleId = 0 + offSet;
            int CVId = 1 + offSet;
            int AUECId = 2 + offSet;
            int AccountId = 3 + offSet;
            int SingleRuleId = 4 + offSet;
            int BasketRuleId = 5 + offSet;
            int CompanyID = 6 + offSet;
            int CounterPartyID = 7 + offSet;
            int VenueID = 8 + offSet;

            CVAUECAccountCommissionRule cvAUECAccountCommissionRule = new CVAUECAccountCommissionRule();
            CommissionRulesCacheManager commissionRulesCacheManager = CommissionRulesCacheManager.GetInstance();
            Guid ruleID = Guid.Empty;
            Prana.BusinessObjects.CommissionRule commissionRule = new Prana.BusinessObjects.CommissionRule();
            _tempCommRuleSelect.RuleID = Guid.Empty;
            _tempCommRuleSelect.RuleName = "-Select-";


            try
            {
                if (row[CVAUECRuleId] != System.DBNull.Value)
                {
                    cvAUECAccountCommissionRule.CVAUECRuleID = int.Parse(row[CVAUECRuleId].ToString());
                }

                if (row[CVId] != System.DBNull.Value)
                {
                    cvAUECAccountCommissionRule.CVID = int.Parse(row[CVId].ToString());
                }

                if (row[AUECId] != System.DBNull.Value)
                {
                    cvAUECAccountCommissionRule.AUECID = int.Parse(row[AUECId].ToString());
                }
                if (row[AccountId] != System.DBNull.Value)
                {
                    cvAUECAccountCommissionRule.AccountID = int.Parse(row[AccountId].ToString());
                }

                if (row[SingleRuleId] != System.DBNull.Value)
                {
                    ruleID = (Guid)row[SingleRuleId];
                    commissionRule = commissionRulesCacheManager.GetCommissionRuleByRuleId(ruleID);
                    //cvAUECAccountCommissionRule.SingleRule.RuleID = (Guid)row[SingleRuleId];
                    cvAUECAccountCommissionRule.SingleRule = commissionRule;
                }

                if (row[BasketRuleId] != System.DBNull.Value)
                {
                    ruleID = (Guid)row[BasketRuleId];
                    commissionRule = commissionRulesCacheManager.GetCommissionRuleByRuleId(ruleID);
                    //cvAUECAccountCommissionRule.BasketRule.RuleID = (Guid)(row[BasketRuleId]);
                    if (commissionRule == null)
                    {
                        cvAUECAccountCommissionRule.BasketRule = _tempCommRuleSelect;
                    }
                    else
                    {
                        cvAUECAccountCommissionRule.BasketRule = commissionRule;
                    }
                }
                if (row[CompanyID] != System.DBNull.Value)
                {
                    cvAUECAccountCommissionRule.CompanyID = int.Parse(row[CompanyID].ToString());
                }

                if (row[CounterPartyID] != System.DBNull.Value)
                {
                    cvAUECAccountCommissionRule.CounterPartyId = int.Parse(row[CounterPartyID].ToString());
                }

                if (row[VenueID] != System.DBNull.Value)
                {
                    cvAUECAccountCommissionRule.VenueId = int.Parse(row[VenueID].ToString());
                }

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
            return cvAUECAccountCommissionRule;
        }
        /// <summary>
        /// save the association of account with commission rules 
        /// </summary>
        /// <param name="cvAUECAccountCommissionRules"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveCompanyCommissionRulesForCVAUEC(List<CVAUECAccountCommissionRule> cvAUECAccountCommissionRules, int companyID)
        {
            int rowsAffected = 0;
            // string cvAUECAccountCommissionRulesXML = XMLUtilities.SerializeToXML(cvAUECAccountCommissionRules);
            //cvAUECAccountCommissionRulesXML= cvAUECAccountCommissionRulesXML.Replace("AccountID", "FundID");
            //cvAUECAccountCommissionRulesXML= cvAUECAccountCommissionRulesXML.Replace("CVAUECAccountCommissionRule", "CVAUECFundCommissionRule");
            //cvAUECAccountCommissionRulesXML= cvAUECAccountCommissionRulesXML.Replace("ArrayOfCVAUECAccountCommissionRule", "ArrayOfCVAUECFundCommissionRule");
            try
            {
                DataTable dsRule = CreateDatasetFromList(cvAUECAccountCommissionRules);
                #region Commented
                //Database db = DatabaseFactory.CreateDatabase();
                //DbCommand cmd = new SqlCommand();
                //cmd.CommandText = "P_SaveCompanyCVAUECFundCommissionRules";
                //cmd.CommandType = CommandType.StoredProcedure;
                //db.AddInParameter(cmd, "@Xml", DbType.String, cvAUECAccountCommissionRulesXML);
                //db.AddInParameter(cmd, "@companyID", DbType.String, companyID);

                //XMLSaveManager.AddOutErrorParameters(db, cmd);

                //rowsAffected = db.ExecuteNonQuery(cmd);

                //XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, cmd);
                #endregion

                using (SqlConnection conn = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                {
                    conn.Open();
                    using (SqlTransaction transaction = DatabaseManager.DatabaseManager.BeginTransaction(conn))
                    {
                        try
                        {
                            QueryData queryData = new QueryData();
                            queryData.StoredProcedureName = "P_DeletCompanyCVAUECFundCommissionRules";
                            queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
                            {
                                IsOutParameter = false,
                                ParameterName = "@companyID",
                                ParameterType = DbType.Int32,
                                ParameterValue = companyID
                            });

                            rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, string.Empty, transaction);

                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, transaction))
                            {

                                sqlBulkCopy.DestinationTableName = "T_CommissionRulesForCVAUEC";
                                sqlBulkCopy.ColumnMappings.Add("CVId_FK", "CVId_FK");
                                sqlBulkCopy.ColumnMappings.Add("AUECId_FK", "AUECId_FK");
                                sqlBulkCopy.ColumnMappings.Add("FundId_FK", "FundId_FK");
                                sqlBulkCopy.ColumnMappings.Add("SingleRuleId_FK", "SingleRuleId_FK");
                                sqlBulkCopy.ColumnMappings.Add("BasketRuleId_FK", "BasketRuleId_FK");
                                sqlBulkCopy.ColumnMappings.Add("CompanyID", "CompanyID");
                                sqlBulkCopy.BulkCopyTimeout = 172800;
                                sqlBulkCopy.WriteToServer(dsRule);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                            if (rethrow)
                            {
                                //throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    //throw;
                }
            }
            return rowsAffected;
        }


        public static DataTable CreateDatasetFromList(List<CVAUECAccountCommissionRule> cvAUECFundCommissionRules)
        {
            DataTable tblRule = new DataTable();
            try
            {
                DataColumn SingleRule = new DataColumn();
                SingleRule.DataType = System.Type.GetType("System.Guid");
                SingleRule.ColumnName = "SingleRuleId_FK";
                DataColumn BasketRule = new DataColumn();
                BasketRule.DataType = System.Type.GetType("System.Guid");
                BasketRule.ColumnName = "BasketRuleId_FK";

                tblRule.Columns.Add("CVId_FK", typeof(System.Int32));
                tblRule.Columns.Add("AUECId_FK", typeof(System.Int32));
                tblRule.Columns.Add("FundId_FK", typeof(System.Int32));
                tblRule.Columns.Add(SingleRule);
                tblRule.Columns.Add(BasketRule);
                tblRule.Columns.Add("CompanyID", typeof(System.Int32));

                foreach (CVAUECAccountCommissionRule rule in cvAUECFundCommissionRules)
                {
                    DataRow row = tblRule.NewRow();
                    row["CVId_FK"] = rule.CVID;
                    row["AUECId_FK"] = rule.AUECID;
                    row["FundId_FK"] = rule.AccountID;
                    row["SingleRuleId_FK"] = rule.SingleRule.RuleID;
                    row["BasketRuleId_FK"] = rule.BasketRule.RuleID;
                    row["CompanyID"] = rule.CompanyID;
                    tblRule.Rows.Add(row);
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
            return tblRule;
        }

        /// <summary>
        /// save the methodlogy for commission calculation Pre or post Allocation 
        /// </summary>
        /// <param name="commissionCalculationTime"></param>
        public static void SaveCommissionCalculationTime(bool commissionCalculationTime)
        {
            int result = int.MinValue;
            object[] parameter = new object[3];

            parameter[0] = commissionCalculationTime;
            parameter[1] = "";
            parameter[2] = 0;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("AL_CalculationMethodlogy", parameter).ToString());
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
        }

        #endregion



        #endregion New Commission Rule Region
    }
}
