using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.CommonDataCache
{
    public class PostTradeDataManager
    {
        static object _locker = new object();

        #region Commission Rules Cache

        static CommissionRulesCacheManager _commissionRulesCacheManager = CommissionRulesCacheManager.GetInstance();

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

        public static void GetAllCommissionRulesForCVAUEC()
        {
            _commissionRulesCacheManager.GetAllCVAUECAccountCommissionRules().Clear();
            _commissionRulesCacheManager.ClearCVAUECCommissionRulesDictionary();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCommissionRulesForCVAUEC";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
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
            _tempCommRuleSelect.RuleName = ApplicationConstants.C_COMBO_SELECT;

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
                    //As there is no need of set _tempCommRuleSelect:Am
                    //if (commissionRule == null)
                    //{
                    //    cvAUECAccountCommissionRule.BasketRule = _tempCommRuleSelect;
                    //}
                    //else
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

        public static void GetAllOtherFeesRulesForAUEC()
        {
            _commissionRulesCacheManager.ClearAUECOtherFeesRulesDictionary();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAuecOtherFeesRules";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        OtherFeeRule otherFeeRule = FillAuecRule(row, 0);
                        if (otherFeeRule.IsCriteriaApplied)
                        {
                            List<OtherFeesCriteria> criteriaList = GetAUECOtherFeeRulesCriteria(otherFeeRule.RuleID);
                            otherFeeRule.LongFeeRuleCriteriaList = criteriaList;
                            otherFeeRule.ShortFeeRuleCriteriaList = new List<OtherFeesCriteria>(criteriaList);
                        }
                        _commissionRulesCacheManager.AddAUECOtherFeeRule(otherFeeRule);
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

        /// <summary>
        /// Gets the auec other fee rules criteria.
        /// </summary>
        /// <param name="ruleID">The rule identifier.</param>
        /// <returns></returns>
        public static List<OtherFeesCriteria> GetAUECOtherFeeRulesCriteria(Guid ruleID)
        {
            List<OtherFeesCriteria> otherFeeRulesCriteria = new List<OtherFeesCriteria>();

            Object[] parameter = new object[1];
            parameter[0] = ruleID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUECOtherFeeCriteria", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        otherFeeRulesCriteria.Add(FillAUECOtherFeeRulesCriteria(row, 0));
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
            return otherFeeRulesCriteria;
        }

        /// <summary>
        /// Fills the auec other fee rules criteria.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offSet">The off set.</param>
        /// <returns></returns>
        public static OtherFeesCriteria FillAUECOtherFeeRulesCriteria(object[] row, int offSet)
        {
            int otherFeesCriteriaId = 0 + offSet;
            int longValueGreaterThan = 1 + offSet;
            int longValueLessThanOrEqual = 2 + offSet;
            int longFeeRate = 3 + offSet;
            int longCalculationBasis = 4 + offSet;
            int shortValueGreaterThan = 6 + offSet;
            int shortValueLessThanOrEqual = 7 + offSet;
            int shortFeeRate = 8 + offSet;
            int shortCalculationBasis = 9 + offSet;

            OtherFeesCriteria otherFeeRule = new OtherFeesCriteria();
            try
            {
                if (row[otherFeesCriteriaId] != null)
                {
                    otherFeeRule.OtherFeesCriteriaId = Convert.ToInt32(row[otherFeesCriteriaId].ToString());
                }
                if (row[longValueGreaterThan] != null)
                {
                    otherFeeRule.LongValueGreaterThan = double.Parse(row[longValueGreaterThan].ToString());
                }
                if (row[longValueLessThanOrEqual] != null)
                {
                    otherFeeRule.LongValueLessThanOrEqual = double.Parse(row[longValueLessThanOrEqual].ToString());
                }
                if (row[longFeeRate] != null)
                {
                    otherFeeRule.LongFeeRate = double.Parse(row[longFeeRate].ToString());
                }
                if (row[longCalculationBasis] != null)
                {
                    otherFeeRule.LongCalculationBasis = Convert.ToInt32(row[longCalculationBasis].ToString());
                    otherFeeRule.LongFeeCriteriaUnit = GetRateUnitByValue(otherFeeRule.LongCalculationBasis);
                }
                if (row[shortValueGreaterThan] != null)
                {
                    otherFeeRule.ShortValueGreaterThan = double.Parse(row[shortValueGreaterThan].ToString());
                }
                if (row[shortValueLessThanOrEqual] != null)
                {
                    otherFeeRule.ShortValueLessThanOrEqual = double.Parse(row[shortValueLessThanOrEqual].ToString());
                }
                if (row[shortFeeRate] != null)
                {
                    otherFeeRule.ShortFeeRate = double.Parse(row[shortFeeRate].ToString());
                }
                if (row[shortCalculationBasis] != null)
                {
                    otherFeeRule.ShortCalculationBasis = Convert.ToInt32(row[shortCalculationBasis].ToString());
                    otherFeeRule.ShortFeeCriteriaUnit = GetRateUnitByValue(otherFeeRule.ShortCalculationBasis);
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
            return otherFeeRule;
        }

        private static OtherFeeRule FillAuecRule(object[] row, int offSet)
        {
            int AUECRuleId = 0 + offSet;
            int LongFeeRate = 1 + offSet;
            int ShortFeeRate = 2 + offSet;
            int LongCalculationBasis = 3 + offSet;
            int ShortCalculationBasis = 4 + offSet;
            int RoundOffPrecision = 5 + offSet;
            int MaxValue = 6 + offSet;
            int MinValue = 7 + offSet;
            int AUECID = 8 + offSet;
            int FeeTypeID = 9 + offSet;
            int roundUpPrecision = 10 + offSet;
            int roundDownPrecision = 11 + offSet;
            int feePrecisionType = 12 + offSet;
            int IsCriteriaApplied = 13 + offSet;

            OtherFeeRule otherFeeRule = new OtherFeeRule();

            try
            {
                if (row[AUECRuleId] != System.DBNull.Value)
                {
                    otherFeeRule.RuleID = (Guid)(row[AUECRuleId]);
                }

                if (row[LongFeeRate] != System.DBNull.Value)
                {
                    otherFeeRule.LongRate = double.Parse(row[LongFeeRate].ToString());
                }
                if (row[ShortFeeRate] != System.DBNull.Value)
                {
                    otherFeeRule.ShortRate = double.Parse(row[ShortFeeRate].ToString());
                }

                if (row[LongCalculationBasis] != System.DBNull.Value)
                {

                    otherFeeRule.LongCalculationBasis = (CalculationFeeBasis)int.Parse(row[LongCalculationBasis].ToString());
                }
                if (row[ShortCalculationBasis] != System.DBNull.Value)
                {

                    otherFeeRule.ShortCalculationBasis = (CalculationFeeBasis)int.Parse(row[ShortCalculationBasis].ToString());
                }

                if (row[RoundOffPrecision] != System.DBNull.Value)
                {
                    otherFeeRule.RoundOffPrecision = Convert.ToInt16((row[RoundOffPrecision].ToString()));
                }
                if (row[MaxValue] != System.DBNull.Value)
                {
                    otherFeeRule.MaxValue = double.Parse(row[MaxValue].ToString());
                }
                if (row[MinValue] != System.DBNull.Value)
                {
                    otherFeeRule.MinValue = double.Parse(row[MinValue].ToString());
                }
                if (row[AUECID] != System.DBNull.Value)
                {
                    otherFeeRule.AUECID = int.Parse(row[AUECID].ToString());
                }
                if (row[FeeTypeID] != System.DBNull.Value)
                {
                    otherFeeRule.OtherFeeType = (OtherFeeType)int.Parse(row[FeeTypeID].ToString());
                }
                if (row[roundUpPrecision] != null)
                {
                    otherFeeRule.RoundUpPrecision = Convert.ToInt32(row[roundUpPrecision].ToString());
                }
                if (row[roundDownPrecision] != null)
                {
                    otherFeeRule.RoundDownPrecision = Convert.ToInt32(row[roundDownPrecision].ToString());
                }
                if (row[feePrecisionType] != null)
                {
                    otherFeeRule.FeePrecisionType = (FeePrecisionType)Enum.Parse(typeof(FeePrecisionType), row[feePrecisionType].ToString());
                }
                if (row[IsCriteriaApplied] != null)
                {
                    otherFeeRule.IsCriteriaApplied = bool.Parse(row[IsCriteriaApplied].ToString());
                }
                return otherFeeRule;

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
                return null;
            }
            #endregion
        }

        /// <summary>
        /// Gets the rate unit by value.
        /// </summary>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns></returns>
        private static string GetRateUnitByValue(int selectedValue)
        {
            try
            {
                CalculationBasis criteria = (CalculationBasis)selectedValue;
                switch (criteria)
                {
                    case CalculationBasis.Shares:
                        return "Per Share";
                    case CalculationBasis.Notional:
                        return "Basis Points";
                    case CalculationBasis.Contracts:
                        return "Per Contract";
                    case CalculationBasis.AvgPrice:
                        return "Per Share/Contract";
                    case CalculationBasis.FlatAmount:
                        return "Per Trade/Taxlot";
                    default:
                        return string.Empty;
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
                return null;
            }
            #endregion
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

        #endregion Commission Rules Cache

        public static Dictionary<string, List<string>> GetAllGroupIDsAndParentClOrderID()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetAllGroupIDSFromParentClOrderID";

            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    string groupID = string.Empty;
                    string parentclOrderid = string.Empty;
                    string cumQty = string.Empty;

                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        if (row[0] != DBNull.Value)
                        {
                            groupID = row[0].ToString();
                        }
                        if (row[1] != DBNull.Value)
                        {
                            parentclOrderid = row[1].ToString();
                        }
                        if (row[2] != DBNull.Value)
                        {
                            cumQty = row[2].ToString();
                        }
                        List<string> list = new List<string>();
                        list.Add(groupID);
                        list.Add(cumQty);
                        if (!dict.ContainsKey(parentclOrderid))
                        {
                            dict.Add(parentclOrderid, list);
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
            return dict;
        }

        /// <summary>
        /// returns the cloOrderId mapped to given parentCLorderId.
        /// </summary>
        /// <param name="clOrderID"></param>
        /// <returns></returns>
        public static string GetClOrderIDFromParentClOrderID(string clOrderID)
        {
            string origClOrdId = string.Empty;
            object[] parematers = new object[1];
            parematers[0] = clOrderID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("GetClOrdIDFromParentClOrderID", parematers))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value)
                        {
                            origClOrdId = row[0].ToString();
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
            return origClOrdId;
        }

        public static string GetGroupIDFromParentClOrderID(string clOrderID)
        {
            string groupID = string.Empty;
            object[] parematers = new object[1];
            parematers[0] = clOrderID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("GetGroupIDFromParentClOrderID", parematers))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value)
                        {
                            groupID = row[0].ToString();
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
            return groupID;
        }

        public static DataSet GetAllocationSchemeReconReport(string allocationSchemeName, DateTime fromAllocationDate, DateTime toAllocationDate)
        {
            DataSet dsAllocationScheme = new DataSet();
            try
            {
                object[] parameter = new object[3];

                parameter[0] = allocationSchemeName;
                parameter[1] = fromAllocationDate;
                parameter[2] = toAllocationDate;
                string spName = "P_GetAllocationSchemeReconReport";
                dsAllocationScheme = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameter);
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
            return dsAllocationScheme;
        }

        public static Dictionary<int, string> GetAccountPBDetails()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetFundPBDetails";
            Dictionary<int, string> accountPBDetails = new Dictionary<int, string>();
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            int FUNDID = 0;
                            int PBNAME = 1;
                            int accountID = int.MinValue;
                            string pbName = string.Empty;

                            if (row[FUNDID] != System.DBNull.Value)
                            {
                                accountID = int.Parse(row[FUNDID].ToString());
                            }
                            if (row[PBNAME] != System.DBNull.Value)
                            {
                                pbName = row[PBNAME].ToString();
                            }
                            if (!accountID.Equals(int.MinValue) && !string.IsNullOrEmpty(pbName))
                            {
                                if (!accountPBDetails.ContainsKey(accountID))
                                    accountPBDetails.Add(accountID, pbName);
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
            return accountPBDetails;
        }

        public static DataSet GetTaxlotDetailsToUpdateExternalTransactionID(string taxlotID)
        {
            DataSet dsTaxlotDetails = new DataSet();
            try
            {
                object[] parameter = new object[1];

                parameter[0] = taxlotID;
                string spName = "P_GetTaxlotDetailsToUpdateExternalTransactionID";
                dsTaxlotDetails = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameter);
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
            return dsTaxlotDetails;
        }

        public static Dictionary<string, string> GetPendingReplacedOrderClOrderID()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetPendingReplacedClOrderId";

            Dictionary<string, string> dict = new Dictionary<string, string>();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    string parentclOrderid = string.Empty;
                    string clOrdId = string.Empty;

                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        if (row[0] != DBNull.Value)
                        {
                            parentclOrderid = row[0].ToString();
                        }
                        if (row[1] != DBNull.Value)
                        {
                            clOrdId = row[1].ToString();
                        }
                        if (!dict.ContainsKey(parentclOrderid))
                        {
                            dict.Add(parentclOrderid, clOrdId);
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
            return dict;
        }

    }
}
