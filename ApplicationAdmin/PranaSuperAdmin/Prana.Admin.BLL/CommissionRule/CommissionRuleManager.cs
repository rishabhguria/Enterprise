#region Using namespaces

using Prana.BusinessLogic;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;

#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionRuleManager.
    /// </summary>
    public class CommissionRuleManager
    {
        public CommissionRuleManager()
        {
        }

        public static AUECCommissionRule FillAUECCommissionRule(object[] row, int offSet)
        {
            int RuleID = 0 + offSet;
            int AUECID_FK = 1 + offSet;
            int RuleName = 2 + offSet;
            int ApplyRuletoID_FK = 3 + offSet;
            int RuleDescription = 4 + offSet;
            int CalculationID_FK = 5 + offSet;
            int CurrencyID_FK = 6 + offSet;
            int CommissionRateID_FK = 7 + offSet;
            int Commission = 8 + offSet;
            int ApplyCriteria = 9 + offSet;
            int ApplyClrFee = 10 + offSet;


            AUECCommissionRule auecCommissionRule = new AUECCommissionRule();
            try
            {
                if (row[RuleID] != null)
                {
                    auecCommissionRule.RuleID = int.Parse(row[RuleID].ToString());
                }
                if (row[AUECID_FK] != null)
                {
                    auecCommissionRule.AUECID = int.Parse(row[AUECID_FK].ToString());
                }
                if (row[RuleName] != null)
                {
                    auecCommissionRule.RuleName = row[RuleName].ToString();
                }
                if (row[ApplyRuletoID_FK] != null)
                {
                    auecCommissionRule.ApplyRuleID = int.Parse(row[ApplyRuletoID_FK].ToString());
                }
                if (row[RuleDescription] != null)
                {
                    auecCommissionRule.RuleDescription = row[RuleDescription].ToString();
                }
                if (row[CalculationID_FK] != null)
                {
                    auecCommissionRule.CalculationID = int.Parse(row[CalculationID_FK].ToString());
                }
                if (row[CurrencyID_FK] != null)
                {
                    auecCommissionRule.CurrencyID = int.Parse(row[CurrencyID_FK].ToString());
                }
                if (row[CommissionRateID_FK] != null)
                {
                    auecCommissionRule.CommissionRateID = int.Parse(row[CommissionRateID_FK].ToString());
                }
                if (row[Commission] != null)
                {
                    auecCommissionRule.Commission = float.Parse(row[Commission].ToString());
                }
                if (row[ApplyCriteria] != null)
                {
                    auecCommissionRule.ApplyCriteria = int.Parse(row[ApplyCriteria].ToString());
                }
                if (row[ApplyClrFee] != null)
                {
                    auecCommissionRule.ApplyClearingFee = int.Parse(row[ApplyClrFee].ToString());
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
            return auecCommissionRule;
        }

        public static AUECCommissionRules GetAUECCommissionRules()
        {
            AUECCommissionRules auecCommissionRules = new AUECCommissionRules();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllAUECCommissionRules";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        auecCommissionRules.Add(FillAUECCommissionRule(row, 0));
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
            return auecCommissionRules;
        }

        public static AUECCommissionRules GetCVAUECCommissionRules(int cvAUECID)
        {
            AUECCommissionRules cvAUECCommissionRules = new AUECCommissionRules();

            Object[] parameter = new object[1];
            parameter[0] = cvAUECID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUECCommissionRulesByCVAUECID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        cvAUECCommissionRules.Add(FillAUECCommissionRule(row, 0));
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
            return cvAUECCommissionRules;
        }

        public static int SaveAUECCommissionRule(AUECCommissionRule auecCommissionRule, int ruleID)
        {
            int result = int.MinValue;
            try
            {
                Object[] parameter = new object[11];
                parameter[0] = ruleID;
                parameter[1] = auecCommissionRule.AUECID;
                parameter[2] = auecCommissionRule.RuleName;
                parameter[3] = auecCommissionRule.ApplyRuleID;
                parameter[4] = auecCommissionRule.RuleDescription;
                parameter[5] = auecCommissionRule.CalculationID;
                parameter[6] = auecCommissionRule.CurrencyID;
                parameter[7] = auecCommissionRule.CommissionRateID;
                parameter[8] = auecCommissionRule.Commission;
                parameter[9] = auecCommissionRule.ApplyCriteria;
                parameter[10] = auecCommissionRule.ApplyClearingFee;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveAUECCommissionRule", parameter).ToString());
                auecCommissionRule.RuleID = result;
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
        public static Rules GetRules()
        {
            Rules rules = new Rules();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetRules";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        rules.Add(FillRule(row, 0));
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
            return rules;
        }
        private static Rule FillRule(object[] row, int offSet)
        {
            int ApplyRuletoId = 0 + offSet;
            int TypeofTrade = 1 + offSet;
            Rule rule = new Rule();
            try
            {
                if (row[ApplyRuletoId] != null)
                {
                    rule.ApplyRuleID = int.Parse(row[ApplyRuletoId].ToString());
                }
                if (row[TypeofTrade] != null)
                {
                    rule.TradeType = row[TypeofTrade].ToString();
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
            return rule;
        }
        public static Rules GetRules(int applyruleID)
        {
            Rules rules = new Rules();

            Object[] parameter = new object[1];
            parameter[0] = applyruleID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRulebyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        rules.Add(FillRule(row, 0));
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
            return rules;
        }
        public static CommissionCalculations GetCalculation()
        {
            CommissionCalculations commissioncalculations = new CommissionCalculations();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCalculation";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissioncalculations.Add(FillCommissionCalculation(row, 0));
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
            return commissioncalculations;
        }

        public static CommissionCalculations GetCalculation(int calculationID)
        {
            CommissionCalculations commissioncalculations = new CommissionCalculations();

            Object[] parameter = new object[1];
            parameter[0] = calculationID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCalculationbyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissioncalculations.Add(FillCommissionCalculation(row, 0));
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
            return commissioncalculations;
        }

        /// <summary>
        /// created by : <Kanupriya>
        /// Date : <10/27/2006>
        /// purpose : <To fetch an object of CommissionCalculation for a particular ID.>
        //TODO : Method above it GetCalculation also works similarly. 
        /// </summary>
        /// <param name="calculationID"></param>
        /// <returns></returns>
        public static CommissionCalculation GetCalculationType(int calculationID)
        {
            CommissionCalculation commissioncalculation = new CommissionCalculation();

            Object[] parameter = new object[1];
            parameter[0] = calculationID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCalculationbyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissioncalculation = FillCommissionCalculation(row, 0);
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
            return commissioncalculation;
        }

        private static CommissionCalculation FillCommissionCalculation(object[] row, int offSet)
        {
            int CommissionCalculationID = 0 + offSet;
            int CalculationType = 1 + offSet;
            CommissionCalculation commissioncalculation = new CommissionCalculation();
            try
            {
                if (row[CommissionCalculationID] != null)
                {
                    commissioncalculation.CommissionCalculationID = int.Parse(row[CommissionCalculationID].ToString());
                }
                if (row[CalculationType] != null)
                {
                    commissioncalculation.CalculationType = row[CalculationType].ToString();
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
            return commissioncalculation;
        }

        public static CommissionRateTypes GetCommisionRate()
        {
            CommissionRateTypes commissionratetypes = new CommissionRateTypes();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCommissionRates";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionratetypes.Add(FillCommissionRateType(row, 0));
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
            return commissionratetypes;
        }
        private static CommissionRateType FillCommissionRateType(object[] row, int offSet)
        {
            int CommissionRateID = 0 + offSet;
            int CommissionRateType = 1 + offSet;
            CommissionRateType commissionratetype = new CommissionRateType();
            try
            {
                if (row[CommissionRateID] != null)
                {
                    commissionratetype.CommissionRateID = int.Parse(row[CommissionRateID].ToString());
                }
                if (row[CommissionRateType] != null)
                {
                    commissionratetype.CommissionRateTypeName = row[CommissionRateType].ToString();
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
            return commissionratetype;
        }
        public static CommissionRateTypes GetCommisionRate(int commissionRateID)
        {
            CommissionRateTypes commissionratetypes = new CommissionRateTypes();

            Object[] parameter = new object[1];
            parameter[0] = commissionRateID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCommissionRatesbyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionratetypes.Add(FillCommissionRateType(row, 0));
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
            return commissionratetypes;
        }

        /// <summary>
        /// Created by : <Kanupriya>
        /// Date : <10/28/2006>
        /// Purpose : <To fetch a single row of data against a selected commissionRateType.>
        // TODO : Seems same as the above function GetCommisionRate
        /// </summary>
        /// <param name="commissionRateID"></param>
        /// <returns></returns>
        public static CommissionRateType GetCommisionRateType(int commissionRateID)
        {
            CommissionRateType commissionratetype = new CommissionRateType();

            Object[] parameter = new object[1];
            parameter[0] = commissionRateID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCommissionRatesbyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionratetype = FillCommissionRateType(row, 0);
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
            return commissionratetype;
        }

        public static Operaters GetOperaters()
        {
            Operaters operaters = new Operaters();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetOperater";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        operaters.Add(FillOperater(row, 0));
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
            return operaters;
        }
        private static Operater FillOperater(object[] row, int offSet)
        {
            int OperatorID = 0 + offSet;
            int Name = 1 + offSet;
            Operater operater = new Operater();
            try
            {
                if (row[OperatorID] != null)
                {
                    operater.OperatorID = int.Parse(row[OperatorID].ToString());
                }
                if (row[Name] != null)
                {
                    operater.Name = row[Name].ToString();
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
            return operater;
        }
        public static Operaters GetOperaters(int operatorID)
        {
            Operaters operaters = new Operaters();

            Object[] parameter = new object[1];
            parameter[0] = operatorID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOperaterbyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        operaters.Add(FillOperater(row, 0));
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
            return operaters;
        }
        public static CommissionCriterias GetCommissionCriterias()
        {
            CommissionCriterias commissionCriterias = new CommissionCriterias();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCommissionCriteria";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        commissionCriterias.Add(FillCommissionCriteria(row, 0));
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
            return commissionCriterias;
        }
        public static CommissionCriteria FillCommissionCriteria(object[] row, int offSet)
        {
            int CommissionCriteriaID = 0 + offSet;
            int RuleID_FK = 1 + offSet;
            int CommissionCalculationID_FK = 2 + offSet;
            int MinimumCommissionRate = 3 + offSet;
            CommissionCriteria commissionCriteria = new CommissionCriteria();
            try
            {
                if (row[CommissionCriteriaID] != null)
                {
                    commissionCriteria.CommissionCriteriaID = int.Parse(row[CommissionCriteriaID].ToString());
                }
                if (row[RuleID_FK] != null)
                {
                    commissionCriteria.RuleID_FK = int.Parse(row[RuleID_FK].ToString());
                }

                if (row[CommissionCalculationID_FK] != null)
                {
                    commissionCriteria.CommissionCalculationID_FK = int.Parse(row[CommissionCalculationID_FK].ToString());
                }

                if (row[MinimumCommissionRate] != null)
                {
                    commissionCriteria.MinimumCommissionRate = float.Parse(row[MinimumCommissionRate].ToString());
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
            return commissionCriteria;
        }

        public static int SaveCommissionCriteria(CommissionCriteria commissionCriteria, int RuleID)
        {
            int result = int.MinValue;
            try
            {
                Object[] parameter = new object[4];
                parameter[0] = commissionCriteria.CommissionCriteriaID;
                parameter[1] = RuleID;
                parameter[2] = commissionCriteria.CommissionCalculationID_FK;
                parameter[3] = commissionCriteria.MinimumCommissionRate;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCommissionCriteria", parameter).ToString());
                commissionCriteria.CommissionCriteriaID = result;
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
        public static CommissionRuleCriterias GetCommissionRuleCriterias()
        {
            CommissionRuleCriterias commissionRuleCriterias = new CommissionRuleCriterias();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCommissionRuleCriteria";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionRuleCriterias.Add(FillCommissionRuleCriteria(row, 0));
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
            return commissionRuleCriterias;
        }

        public static CommissionRuleCriteriaold FillCommissionRuleCriteria(object[] row, int offSet)
        {
            int CommissionRuleCriteriaID = 0 + offSet;
            int CommissionCriteriaID_FK = 1 + offSet;
            int OperatorID_FK1 = 2 + offSet;
            int Value1 = 3 + offSet;
            int CommissionRateID_FK1 = 4 + offSet;
            int CommisionRate1 = 5 + offSet;
            int OperatorID_FK2 = 6 + offSet;
            int Value2 = 7 + offSet;
            int CommissionRateID_FK2 = 8 + offSet;
            int CommisionRate2 = 9 + offSet;
            int OperatorID_FK3 = 10 + offSet;
            int Value3 = 11 + offSet;
            int CommissionRateID_FK3 = 12 + offSet;
            int CommisionRate3 = 13 + offSet;

            CommissionRuleCriteriaold commissionRuleCriteria = new CommissionRuleCriteriaold();
            try
            {
                if (row[CommissionRuleCriteriaID] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionRuleCriteriaID = int.Parse(row[CommissionRuleCriteriaID].ToString());
                }
                if (row[CommissionCriteriaID_FK] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionCriteriaID_FK = int.Parse(row[CommissionCriteriaID_FK].ToString());
                }

                if (row[OperatorID_FK1] != System.DBNull.Value)
                {
                    commissionRuleCriteria.OperatorID_FK1 = int.Parse(row[OperatorID_FK1].ToString());
                }

                if (row[Value1] != System.DBNull.Value)
                {
                    commissionRuleCriteria.Value1 = int.Parse(row[Value1].ToString());
                }
                if (row[CommissionRateID_FK1] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionRateID_FK1 = int.Parse(row[CommissionRateID_FK1].ToString());
                }
                if (row[CommisionRate1] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommisionRate1 = double.Parse(row[CommisionRate1].ToString());
                }
                if (row[OperatorID_FK2] != System.DBNull.Value)
                {
                    commissionRuleCriteria.OperatorID_FK2 = int.Parse(row[OperatorID_FK2].ToString());
                }

                if (row[Value2] != System.DBNull.Value)
                {
                    commissionRuleCriteria.Value2 = int.Parse(row[Value2].ToString());
                }
                if (row[CommissionRateID_FK2] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionRateID_FK2 = int.Parse(row[CommissionRateID_FK2].ToString());
                }
                if (row[CommisionRate2] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommisionRate2 = float.Parse(row[CommisionRate2].ToString());
                }
                if (row[OperatorID_FK3] != System.DBNull.Value)
                {
                    commissionRuleCriteria.OperatorID_FK3 = int.Parse(row[OperatorID_FK3].ToString());
                }

                if (row[Value3] != System.DBNull.Value)
                {
                    commissionRuleCriteria.Value3 = int.Parse(row[Value3].ToString());
                }
                if (row[CommissionRateID_FK3] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommissionRateID_FK3 = int.Parse(row[CommissionRateID_FK3].ToString());
                }
                if (row[CommisionRate3] != System.DBNull.Value)
                {
                    commissionRuleCriteria.CommisionRate3 = float.Parse(row[CommisionRate3].ToString());
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



        public static int SaveCommissionRuleCriteriasUp(CommissionRuleCriteriasUp commiRulesCriasUp, int criteriaRuleID)
        {
            int result = int.MinValue;

            object[] parameter = new object[5];

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCommissionRuleCriteriaUp", new object[] { criteriaRuleID });

                foreach (CommissionRuleCriteriaUp commRulecritriaUp in commiRulesCriasUp)
                {
                    parameter[0] = criteriaRuleID;
                    parameter[1] = commRulecritriaUp.ValueFrom;
                    parameter[2] = commRulecritriaUp.ValueTo;
                    parameter[3] = commRulecritriaUp.CommissionRateID_FK;
                    parameter[4] = commRulecritriaUp.CommisionRate;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCommissionRuleCriteriaUp", parameter).ToString());
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
        public static int SaveCommissionRuleClearingFee(CommissionRuleClearingFee commiRulesClrFee, int RuleID)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];

            try
            {
                //db.ExecuteNonQuery("P_DeleteCommissionRuleCriteriaUp", criteriaRuleID);

                parameter[0] = RuleID;
                parameter[1] = commiRulesClrFee.CalculationId;
                parameter[2] = commiRulesClrFee.CurrencyId;
                parameter[3] = commiRulesClrFee.CommissionRate;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCommissionRuleClearingFee", parameter).ToString());

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




        public static int SaveCommissionRuleCriteria(CommissionRuleCriteriaold commissionRuleCriteria, int CommissionCriteriaID)
        {
            int result = int.MinValue;
            try
            {
                Object[] parameter = new object[14];
                parameter[0] = commissionRuleCriteria.CommissionRuleCriteriaID;
                parameter[1] = CommissionCriteriaID;
                parameter[2] = commissionRuleCriteria.OperatorID_FK1;
                parameter[3] = commissionRuleCriteria.Value1;
                parameter[4] = commissionRuleCriteria.CommissionRateID_FK1;
                parameter[5] = commissionRuleCriteria.CommisionRate1;
                parameter[6] = commissionRuleCriteria.OperatorID_FK2;
                parameter[7] = commissionRuleCriteria.Value2;
                parameter[8] = commissionRuleCriteria.CommissionRateID_FK2;
                parameter[9] = commissionRuleCriteria.CommisionRate2;
                parameter[10] = commissionRuleCriteria.OperatorID_FK3;
                parameter[11] = commissionRuleCriteria.Value3;
                parameter[12] = commissionRuleCriteria.CommissionRateID_FK3;
                parameter[13] = commissionRuleCriteria.CommisionRate3;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCommissionRuleCriteria", parameter).ToString());
                commissionRuleCriteria.CommissionRuleCriteriaID = result;
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
        public static CommissionCriteria GetCommissionCriteriaByRuleID(int ruleID)
        {
            CommissionCriteria commissionCriteria = null;
            //CommissionCriteria  commissionCriteria = new CommissionCriteria();
            Object[] parameter = new object[1];
            parameter[0] = ruleID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCommissionCriteriabyRuleID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        commissionCriteria = FillCommissionCriteria(row, 0);
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
            return commissionCriteria;
        }


        public static CommissionRuleClearingFee FillCommissionRuleClrFee(object[] row, int offSet)
        {
            int ClearingFeeId = 0 + offSet;
            int RuleId = 1 + offSet;
            int CalculationId = 2 + offSet;
            int CurrencyId = 3 + offSet;
            int CommissionRate = 4 + offSet;
            int CalculationType = 5 + offSet;
            CommissionRuleClearingFee commissionClrFee = new CommissionRuleClearingFee();
            try
            {
                if (row[ClearingFeeId] != null)
                {
                    commissionClrFee.ClearingFeeId = int.Parse(row[ClearingFeeId].ToString());
                }
                if (row[RuleId] != null)
                {
                    commissionClrFee.RuleId = int.Parse(row[RuleId].ToString());
                }

                if (row[CalculationId] != null)
                {
                    commissionClrFee.CalculationId = int.Parse(row[CalculationId].ToString());
                }

                if (row[CurrencyId] != null)
                {
                    commissionClrFee.CurrencyId = int.Parse(row[CurrencyId].ToString());
                }
                if (row[CommissionRate] != null)
                {
                    commissionClrFee.CommissionRate = float.Parse(row[CommissionRate].ToString());
                }
                if (row[CalculationType] != null)
                {
                    commissionClrFee.CalculationType = row[CalculationType].ToString();
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
            return commissionClrFee;
        }



        public static CommissionRuleClearingFee GetCommissionRuleClrFeeByRuleID(int ruleID)
        {
            CommissionRuleClearingFee commissionRuleClrFee = null;

            Object[] parameter = new object[1];
            parameter[0] = ruleID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCommissionRuleClearingFeeByRuleId", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        commissionRuleClrFee = FillCommissionRuleClrFee(row, 0);
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
            return commissionRuleClrFee;
        }



        public static bool DeleteCommissionRule(int ruleID, bool deleteForceFully)
        {
            bool result = false;
            int ForceFully = 0;
            try
            {
                if (deleteForceFully)
                {
                    ForceFully = 1;
                }
                else
                {
                    ForceFully = 0;
                }

                object[] parameter = new object[2];
                parameter[0] = ruleID;
                parameter[1] = ForceFully;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCommissionRule", parameter) > 0)
                {
                    result = true;
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

        public static int DeleteSelectedCommissionRule(int AccountId, int CVId, int AUECId)
        {
            int result = 0;

            try
            {
                object[] parameter = new object[3];
                parameter[0] = AccountId;
                parameter[1] = CVId;
                parameter[2] = AUECId;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteSelectedCommissionRule", parameter).ToString());

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

        public static AUECCommissionRule GetAUECCommissionRule(int RuleID)
        {
            AUECCommissionRule auecCommissionRule = null;

            Object[] parameter = new object[1];
            parameter[0] = RuleID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllAUECCommissionRulesbyRuleID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        auecCommissionRule = FillAUECCommissionRule(row, 0);
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
            return auecCommissionRule;

        }

        public static CommissionRuleCriteriaold GetCommissionRuleCriteria(int commissionCriteriaID)
        {
            CommissionRuleCriteriaold commissionRuleCriteria = null;
            Object[] parameter = new object[1];
            parameter[0] = commissionCriteriaID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllCommissionRuleCriteriabyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        commissionRuleCriteria = FillCommissionRuleCriteria(row, 0);
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
            return commissionRuleCriteria;
        }


        public static CommissionRuleCriteriaUp FillCommissionRuleCriteriaUp(object[] row, int offSet)
        {
            int CommissionRuleCriteriaID = 0 + offSet;
            int CommissionCriteriaID_FK = 1 + offSet;
            Int64 ValueFrom = 2 + offSet;
            Int64 ValueTo = 3 + offSet;
            int CommissionRateID_FK = 4 + offSet;
            int CommisionRate = 5 + offSet;

            CommissionRuleCriteriaUp commissionRuleCriteriaUp = new CommissionRuleCriteriaUp();
            try
            {
                if (row[CommissionRuleCriteriaID] != System.DBNull.Value)
                {
                    commissionRuleCriteriaUp.CommissionRuleCriteriaID = int.Parse(row[CommissionRuleCriteriaID].ToString());
                }
                if (row[CommissionCriteriaID_FK] != System.DBNull.Value)
                {
                    commissionRuleCriteriaUp.CommissionCriteriaID_FK = int.Parse(row[CommissionCriteriaID_FK].ToString());
                }

                if (row[ValueFrom] != System.DBNull.Value)
                {
                    commissionRuleCriteriaUp.ValueFrom = Int64.Parse(row[ValueFrom].ToString());
                }

                if (row[ValueTo] != System.DBNull.Value)
                {
                    commissionRuleCriteriaUp.ValueTo = Int64.Parse(row[ValueTo].ToString());
                }
                if (row[CommissionRateID_FK] != System.DBNull.Value)
                {
                    commissionRuleCriteriaUp.CommissionRateID_FK = int.Parse(row[CommissionRateID_FK].ToString());
                }
                if (row[CommisionRate] != System.DBNull.Value)
                {
                    commissionRuleCriteriaUp.CommisionRate = double.Parse(row[CommisionRate].ToString());
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
            return commissionRuleCriteriaUp;
        }


        //Updated Commission Rule Criteria


        public static CommissionRuleCriteriasUp GetCommissionRuleCriteriasUp(int commissionCriteriaID)
        {
            CommissionRuleCriteriasUp commissionRuleCriteriasUp = new CommissionRuleCriteriasUp();
            Object[] parameter = new object[1];
            parameter[0] = commissionCriteriaID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllCommissionRuleCriteriabyIDUpdated", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        commissionRuleCriteriasUp.Add(FillCommissionRuleCriteriaUp(row, 0));
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
            return commissionRuleCriteriasUp;
        }




        public static AllCommissionRules GetAllCommissionRules()
        {
            AllCommissionRules allCommissionRules = new AllCommissionRules();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCommissionRules";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                //using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader(CommandType.StoredProcedure, "P_GetAllCommissionRulesbyID"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        allCommissionRules.Add(FillAllCommissionRule(row, 0));
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
            return allCommissionRules;
        }

        public static AllCommissionRule FillAllCommissionRule(object[] row, int offSet)
        {
            int RuleID = 0 + offSet;
            int AUECID_FK = 1 + offSet;
            int RuleName = 2 + offSet;
            int ApplyRuletoID_FK = 3 + offSet;
            int RuleDescription = 4 + offSet;
            int CalculationID_FK = 5 + offSet;
            int CurrencyID_FK = 6 + offSet;
            int CommissionRateID_FK = 7 + offSet;
            int Commission = 8 + offSet;
            int ApplyCriteria = 9 + offSet;
            int CommissionCriteriaID = 10 + offSet;
            int RuleID_FK = 11 + offSet;
            int CommissionCalculationID_FK = 12 + offSet;
            int MinimumCommissionRate = 13 + offSet;

            //			int CommissionRuleCriteriaID = 14 + offSet;
            //			int CommissionCriteriaID_FK= 15 + offSet;

            //			int OperatorID_FK1= 16 + offSet;
            //			int Value1 = 17 + offSet;
            //			int CommissionRateID_FK1= 18 + offSet;
            //			int CommisionRate1 =19+ offSet;

            //			int OperatorID_FK2= 20+ offSet;
            //			int Value2 = 21 + offSet;
            //			int CommissionRateID_FK2= 22 + offSet;
            //			int CommisionRate2 =23+ offSet;
            //			
            //			int OperatorID_FK3= 24 + offSet;
            //			int Value3 = 25 + offSet;
            //			int CommissionRateID_FK3=26 + offSet;
            //			int CommisionRate3 =27+ offSet;



            AllCommissionRule allCommissionRule = new AllCommissionRule();
            try
            {
                if (row[RuleID] != System.DBNull.Value)
                {
                    allCommissionRule.RuleID = int.Parse(row[RuleID].ToString());
                }
                if (row[AUECID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.AUECID = int.Parse(row[AUECID_FK].ToString());
                }
                if (row[RuleName] != System.DBNull.Value)
                {
                    allCommissionRule.RuleName = row[RuleName].ToString();
                }
                if (row[ApplyRuletoID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.ApplyRuleID = int.Parse(row[ApplyRuletoID_FK].ToString());
                }
                if (row[RuleDescription] != System.DBNull.Value)
                {
                    allCommissionRule.RuleDescription = row[RuleDescription].ToString();
                }
                if (row[CalculationID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.CalculationID = int.Parse(row[CalculationID_FK].ToString());
                }
                if (row[CurrencyID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.CurrencyID = int.Parse(row[CurrencyID_FK].ToString());
                }
                if (row[CommissionRateID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionRateID = int.Parse(row[CommissionRateID_FK].ToString());
                }
                if (row[Commission] != System.DBNull.Value)
                {
                    allCommissionRule.Commission = float.Parse(row[Commission].ToString());
                }
                if (row[ApplyCriteria] != System.DBNull.Value)
                {
                    allCommissionRule.ApplyCriteria = int.Parse(row[ApplyCriteria].ToString());
                }

                if (row[CommissionCriteriaID] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionCriteriaID = int.Parse(row[CommissionCriteriaID].ToString());
                }
                if (row[RuleID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.RuleID_FK = int.Parse(row[RuleID_FK].ToString());
                }

                if (row[CommissionCalculationID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionCalculationID_FK = int.Parse(row[CommissionCalculationID_FK].ToString());
                }

                if (row[MinimumCommissionRate] != System.DBNull.Value)
                {
                    allCommissionRule.MinimumCommissionRate = float.Parse(row[MinimumCommissionRate].ToString());
                }

                //				if(row[CommissionRuleCriteriaID] != System.DBNull.Value)
                //				{
                //					allCommissionRule.CommissionRuleCriteriaID= int.Parse(row[CommissionRuleCriteriaID].ToString());
                //				}
                //				if(row[CommissionCriteriaID_FK] != System.DBNull.Value)
                //				{
                //					allCommissionRule.CommissionCriteriaID_FK= int.Parse(row[CommissionCriteriaID_FK].ToString());
                //				}
                //				
                //				if(row[OperatorID_FK1] != System.DBNull.Value)
                //				{
                //					allCommissionRule.OperatorID_FK1= int.Parse(row[OperatorID_FK1].ToString());
                //				}
                //				
                //				if(row[Value1] != System.DBNull.Value)
                //				{
                //					allCommissionRule.Value1= int.Parse(row[Value1].ToString());
                //				}
                //				if(row[CommissionRateID_FK1] != System.DBNull.Value)
                //				{
                //					allCommissionRule.CommissionRateID_FK1= int.Parse(row[CommissionRateID_FK1].ToString());
                //				}
                //				if(row[CommisionRate1] != System.DBNull.Value)
                //				{
                //					allCommissionRule.CommisionRate1 = int.Parse(row[CommisionRate1].ToString());
                //				}

                //				if(row[OperatorID_FK2] != System.DBNull.Value)
                //				{
                //					allCommissionRule.OperatorID_FK2= int.Parse(row[OperatorID_FK2].ToString());
                //				}
                //				
                //				if(row[Value2] != System.DBNull.Value)
                //				{
                //					allCommissionRule.Value2= int.Parse(row[Value2].ToString());
                //				}
                //				if(row[CommissionRateID_FK2] != System.DBNull.Value)
                //				{
                //					allCommissionRule.CommissionRateID_FK2= int.Parse(row[CommissionRateID_FK2].ToString());
                //				}
                //				if(row[CommisionRate2] != System.DBNull.Value)
                //				{
                //					allCommissionRule.CommisionRate2 = int.Parse(row[CommisionRate2].ToString());
                //				}
                //				if(row[OperatorID_FK3] != System.DBNull.Value)
                //				{
                //					allCommissionRule.OperatorID_FK3= int.Parse(row[OperatorID_FK3].ToString());
                //				}
                //				
                //				if(row[Value3] != System.DBNull.Value)
                //				{
                //					allCommissionRule.Value3= int.Parse(row[Value3].ToString());
                //				}
                //				if(row[CommissionRateID_FK3] != System.DBNull.Value)
                //				{
                //					allCommissionRule.CommissionRateID_FK3= int.Parse(row[CommissionRateID_FK3].ToString());
                //				}
                //				if(row[CommisionRate3] != System.DBNull.Value)
                //				{
                //					allCommissionRule.CommisionRate3 = int.Parse(row[CommisionRate3].ToString());
                //				}

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
            return allCommissionRule;
        }
        public static AllCommissionRule GetAllCommissionRule(int RuleID)
        {
            AllCommissionRule allCommissionRule = null;

            Object[] parameter = new object[1];
            parameter[0] = RuleID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllCommissionRulesbyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        allCommissionRule = FillAllCommissionRule(row, 0);
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
            return allCommissionRule;

        }

        #region CommissionRuleByIDForEdit
        public static AllCommissionRule FillCommissionRuleByID(object[] row, int offSet)
        {
            int RuleID = 0 + offSet;
            int AUECID_FK = 1 + offSet;
            int RuleName = 2 + offSet;
            int ApplyRuletoID_FK = 3 + offSet;
            int RuleDescription = 4 + offSet;
            int CalculationID_FK = 5 + offSet;
            int CurrencyID_FK = 6 + offSet;
            int CommissionRateID_FK = 7 + offSet;
            int Commission = 8 + offSet;
            int ApplyCriteria = 9 + offSet;
            int CommissionCriteriaID = 10 + offSet;
            int RuleID_FK = 11 + offSet;
            int CommissionCalculationID_FK = 12 + offSet;
            int MinimumCommissionRate = 13 + offSet;

            int CommissionRuleCriteriaID = 14 + offSet;
            int CommissionCriteriaID_FK = 15 + offSet;

            int OperatorID_FK1 = 16 + offSet;
            int Value1 = 17 + offSet;
            int CommissionRateID_FK1 = 18 + offSet;
            int CommisionRate1 = 19 + offSet;

            int OperatorID_FK2 = 20 + offSet;
            int Value2 = 21 + offSet;
            int CommissionRateID_FK2 = 22 + offSet;
            int CommisionRate2 = 23 + offSet;

            int OperatorID_FK3 = 24 + offSet;
            int Value3 = 25 + offSet;
            int CommissionRateID_FK3 = 26 + offSet;
            int CommisionRate3 = 27 + offSet;



            AllCommissionRule allCommissionRule = new AllCommissionRule();
            try
            {
                if (row[RuleID] != System.DBNull.Value)
                {
                    allCommissionRule.RuleID = int.Parse(row[RuleID].ToString());
                }
                if (row[AUECID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.AUECID = int.Parse(row[AUECID_FK].ToString());
                }
                if (row[RuleName] != System.DBNull.Value)
                {
                    allCommissionRule.RuleName = row[RuleName].ToString();
                }
                if (row[ApplyRuletoID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.ApplyRuleID = int.Parse(row[ApplyRuletoID_FK].ToString());
                }
                if (row[RuleDescription] != System.DBNull.Value)
                {
                    allCommissionRule.RuleDescription = row[RuleDescription].ToString();
                }
                if (row[CalculationID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.CalculationID = int.Parse(row[CalculationID_FK].ToString());
                }
                if (row[CurrencyID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.CurrencyID = int.Parse(row[CurrencyID_FK].ToString());
                }
                if (row[CommissionRateID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionRateID = int.Parse(row[CommissionRateID_FK].ToString());
                }
                if (row[Commission] != System.DBNull.Value)
                {
                    allCommissionRule.Commission = float.Parse(row[Commission].ToString());
                }
                if (row[ApplyCriteria] != System.DBNull.Value)
                {
                    allCommissionRule.ApplyCriteria = int.Parse(row[ApplyCriteria].ToString());
                }

                if (row[CommissionCriteriaID] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionCriteriaID = int.Parse(row[CommissionCriteriaID].ToString());
                }
                if (row[RuleID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.RuleID_FK = int.Parse(row[RuleID_FK].ToString());
                }

                if (row[CommissionCalculationID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionCalculationID_FK = int.Parse(row[CommissionCalculationID_FK].ToString());
                }

                if (row[MinimumCommissionRate] != System.DBNull.Value)
                {
                    allCommissionRule.MinimumCommissionRate = float.Parse(row[MinimumCommissionRate].ToString());
                }

                if (row[CommissionRuleCriteriaID] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionRuleCriteriaID = int.Parse(row[CommissionRuleCriteriaID].ToString());
                }
                if (row[CommissionCriteriaID_FK] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionCriteriaID_FK = int.Parse(row[CommissionCriteriaID_FK].ToString());
                }

                if (row[OperatorID_FK1] != System.DBNull.Value)
                {
                    allCommissionRule.OperatorID_FK1 = int.Parse(row[OperatorID_FK1].ToString());
                }

                if (row[Value1] != System.DBNull.Value)
                {
                    allCommissionRule.Value1 = int.Parse(row[Value1].ToString());
                }
                if (row[CommissionRateID_FK1] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionRateID_FK1 = int.Parse(row[CommissionRateID_FK1].ToString());
                }
                if (row[CommisionRate1] != System.DBNull.Value)
                {
                    allCommissionRule.CommisionRate1 = float.Parse(row[CommisionRate1].ToString());
                }

                if (row[OperatorID_FK2] != System.DBNull.Value)
                {
                    allCommissionRule.OperatorID_FK2 = int.Parse(row[OperatorID_FK2].ToString());
                }

                if (row[Value2] != System.DBNull.Value)
                {
                    allCommissionRule.Value2 = int.Parse(row[Value2].ToString());
                }
                if (row[CommissionRateID_FK2] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionRateID_FK2 = int.Parse(row[CommissionRateID_FK2].ToString());
                }
                if (row[CommisionRate2] != System.DBNull.Value)
                {
                    allCommissionRule.CommisionRate2 = float.Parse(row[CommisionRate2].ToString());
                }
                if (row[OperatorID_FK3] != System.DBNull.Value)
                {
                    allCommissionRule.OperatorID_FK3 = int.Parse(row[OperatorID_FK3].ToString());
                }

                if (row[Value3] != System.DBNull.Value)
                {
                    allCommissionRule.Value3 = int.Parse(row[Value3].ToString());
                }
                if (row[CommissionRateID_FK3] != System.DBNull.Value)
                {
                    allCommissionRule.CommissionRateID_FK3 = int.Parse(row[CommissionRateID_FK3].ToString());
                }
                if (row[CommisionRate3] != System.DBNull.Value)
                {
                    allCommissionRule.CommisionRate3 = float.Parse(row[CommisionRate3].ToString());
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
            return allCommissionRule;
        }

        public static AllCommissionRule GetCommissionRuleByID(int ruleID)
        {
            AllCommissionRule allCommissionRule = new AllCommissionRule();

            Object[] parameter = new object[1];
            parameter[0] = ruleID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllCommissionRulesbyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        allCommissionRule = FillCommissionRuleByID(row, 0);
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
            return allCommissionRule;

        }
        #endregion

        #region CommissionRuleDescriptionByRuleID
        public static AllCommissionRule FillCommissionRuleForDescription(object[] row, int offSet)
        {
            int RuleID = 0 + offSet;
            int ApplyCriteria = 1 + offSet;
            int OperatorName = 2 + offSet;
            int Value = 3 + offSet;
            int CommissionRateID_FK = 4 + offSet;
            int CommisionRate = 5 + offSet;
            int CommisionRateType = 6 + offSet;
            int CalculationType = 7 + offSet;
            int CurencySymbol = 8 + offSet;
            int Commission = 9 + offSet;

            AllCommissionRule commissionRuleDescription = new AllCommissionRule();
            try
            {
                if (row[RuleID] != System.DBNull.Value)
                {
                    commissionRuleDescription.RuleID = int.Parse(row[RuleID].ToString());
                }
                if (row[ApplyCriteria] != System.DBNull.Value)
                {
                    commissionRuleDescription.ApplyCriteria = int.Parse(row[ApplyCriteria].ToString());
                }
                if (row[OperatorName] != System.DBNull.Value)
                {
                    commissionRuleDescription.OperatorName = row[OperatorName].ToString();
                }
                if (row[Value] != System.DBNull.Value)
                {
                    commissionRuleDescription.Value1 = int.Parse(row[Value].ToString());
                }
                if (row[CommissionRateID_FK] != System.DBNull.Value)
                {
                    commissionRuleDescription.CommissionRateID_FK1 = int.Parse(row[CommissionRateID_FK].ToString());
                }
                if (row[CommisionRate] != System.DBNull.Value)
                {
                    commissionRuleDescription.CommisionRate1 = float.Parse(row[CommisionRate].ToString());
                }
                if (row[CommisionRateType] != System.DBNull.Value)
                {
                    commissionRuleDescription.CommissionRateType = row[CommisionRateType].ToString();
                }
                if (row[CalculationType] != System.DBNull.Value)
                {
                    commissionRuleDescription.CaluculationType = row[CalculationType].ToString();
                }
                if (row[CurencySymbol] != System.DBNull.Value)
                {
                    commissionRuleDescription.CurrencySymbol = row[CurencySymbol].ToString();
                }
                if (row[Commission] != System.DBNull.Value)
                {
                    commissionRuleDescription.Commission = float.Parse(row[Commission].ToString());
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
            return commissionRuleDescription;
        }

        /// <summary>
        /// This method gets the specific description of the ruleID supplied.
        /// </summary>
        /// <param name="RuleID">The parameter supplied to get the required description.</param>
        /// <returns>Returns the <see cref="AllCommissionRules"/> object.</returns>
        public static AllCommissionRules GetCommissionRuleDescription(int ruleID)
        {
            AllCommissionRules commissionRuleDescriptions = new AllCommissionRules();

            Object[] parameter = new object[1];
            parameter[0] = ruleID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRuleDescription", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        commissionRuleDescriptions.Add(FillCommissionRuleForDescription(row, 0));
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
            return commissionRuleDescriptions;

        }
        #endregion

        /// <summary>
        /// updated
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>

        public static AllCommissionRule FillCommissionRuleForDescriptionUpdated(object[] row, int offSet)
        {
            int RuleID = 0 + offSet;
            int ApplyCriteria = 1 + offSet;
            int ValueFrom = 2 + offSet;
            int ValueTo = 3 + offSet;
            int CommissionRateID_FK = 4 + offSet;
            int CommisionRate = 5 + offSet;
            int CommisionRateType = 6 + offSet;
            int CalculationType = 7 + offSet;
            int CurencySymbol = 8 + offSet;
            int Commission = 9 + offSet;

            AllCommissionRule commissionRuleDescription = new AllCommissionRule();
            try
            {
                if (row[RuleID] != System.DBNull.Value)
                {
                    commissionRuleDescription.RuleID = int.Parse(row[RuleID].ToString());
                }
                if (row[ApplyCriteria] != System.DBNull.Value)
                {
                    commissionRuleDescription.ApplyCriteria = int.Parse(row[ApplyCriteria].ToString());
                }
                if (row[ValueFrom] != System.DBNull.Value)
                {
                    commissionRuleDescription.ValueFrom = Int64.Parse(row[ValueFrom].ToString());
                }
                if (row[ValueTo] != System.DBNull.Value)
                {
                    commissionRuleDescription.ValueTo = Int64.Parse(row[ValueTo].ToString());
                }
                if (row[CommissionRateID_FK] != System.DBNull.Value)
                {
                    commissionRuleDescription.CommissionRateID_FK1 = int.Parse(row[CommissionRateID_FK].ToString());
                }
                if (row[CommisionRate] != System.DBNull.Value)
                {
                    commissionRuleDescription.CommisionRate1 = float.Parse(row[CommisionRate].ToString());
                }
                if (row[CommisionRateType] != System.DBNull.Value)
                {
                    commissionRuleDescription.CommissionRateType = row[CommisionRateType].ToString();
                }
                if (row[CalculationType] != System.DBNull.Value)
                {
                    commissionRuleDescription.CaluculationType = row[CalculationType].ToString();
                }
                if (row[CurencySymbol] != System.DBNull.Value)
                {
                    commissionRuleDescription.CurrencySymbol = row[CurencySymbol].ToString();
                }
                if (row[Commission] != System.DBNull.Value)
                {
                    commissionRuleDescription.Commission = float.Parse(row[Commission].ToString());
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
            return commissionRuleDescription;
        }

        // Updated Method
        public static AllCommissionRules GetCommissionRuleDescriptionUpdated(int ruleID)
        {
            AllCommissionRules commissionRuleDescriptions = new AllCommissionRules();

            Object[] parameter = new object[1];
            parameter[0] = ruleID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRuleDescriptionUpdated", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        commissionRuleDescriptions.Add(FillCommissionRuleForDescriptionUpdated(row, 0));
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
            return commissionRuleDescriptions;
        }

        public static bool DeleteCommissionCrietariaDetails(int ruleID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = ruleID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("DeleteCommissionCrietariaDetails", parameter) > 0)
                {
                    result = true;
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


        public static bool DeleteCommissionRuleClearingFee(int ruleID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = ruleID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("DeleteCommissionRuleClearingFee", parameter) > 0)
                {
                    result = true;
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


        #region New Commission Rule Region

        private static int _errorNumber = 0;

        private static string _errorMessage = string.Empty;

        public static int SaveAndUpdateCommissionRule(List<CommissionRule> commissionRuleColection)
        {
            int rowsAffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "SaveAndUpdateCommissionRule";
                queryData.DictionaryDatabaseParameter.Add("@commissionRuleColection", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@commissionRuleColection",
                    ParameterType = DbType.String,
                    ParameterValue = commissionRuleColection
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

        public static bool DeleteCommissionRule(string ruleID)//second parameter  , bool deleteForceFully
        {
            bool deleteForceFully = false;
            bool result = false;
            int ForceFully = 0;

            try
            {
                if (deleteForceFully)
                {
                    ForceFully = 1;
                }
                else
                {
                    ForceFully = 0;
                }

                object[] parameter = new object[2];
                parameter[0] = ruleID;
                parameter[1] = ForceFully;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCommissionRule", parameter) > 0)
                {
                    result = true;
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

        public static CommissionRule FillCommissionRule(object[] row, int offSet)
        {
            int RuleID = 0 + offSet;
            int AUECID_FK = 1 + offSet;
            int RuleName = 2 + offSet;
            int ApplyRuletoID_FK = 3 + offSet;
            int RuleDescription = 4 + offSet;
            int CalculationID_FK = 5 + offSet;
            int CurrencyID_FK = 6 + offSet;
            int CommissionRateID_FK = 7 + offSet;
            int Commission = 8 + offSet;
            int ApplyCriteria = 9 + offSet;
            int ApplyClrFee = 10 + offSet;


            CommissionRule commissionRule = new CommissionRule();
            try
            {
                if (row[RuleID] != null)
                {
                    // commissionRule.RuleID = int.Parse(row[RuleID].ToString());
                }
                if (row[AUECID_FK] != null)
                {
                    //commissionRule.AUECID = int.Parse(row[AUECID_FK].ToString());
                }
                if (row[RuleName] != null)
                {
                    commissionRule.RuleName = row[RuleName].ToString();
                }
                if (row[ApplyRuletoID_FK] != null)
                {
                    //  commissionRule.ApplyRuleID = int.Parse(row[ApplyRuletoID_FK].ToString());
                }
                if (row[RuleDescription] != null)
                {
                    commissionRule.RuleDescription = row[RuleDescription].ToString();
                }
                if (row[CalculationID_FK] != null)
                {
                    // commissionRule.CalculationID = int.Parse(row[CalculationID_FK].ToString());
                }
                if (row[CurrencyID_FK] != null)
                {
                    // commissionRule.CurrencyID = int.Parse(row[CurrencyID_FK].ToString());
                }
                if (row[CommissionRateID_FK] != null)
                {
                    //commissionRule.CommissionRateID = int.Parse(row[CommissionRateID_FK].ToString());
                }
                if (row[Commission] != null)
                {
                    // commissionRule.Commission = float.Parse(row[Commission].ToString());
                }
                if (row[ApplyCriteria] != null)
                {
                    // commissionRule.ApplyCriteria = int.Parse(row[ApplyCriteria].ToString());
                }
                if (row[ApplyClrFee] != null)
                {
                    //commissionRule.ApplyClearingFee = int.Parse(row[ApplyClrFee].ToString());
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

        public static List<CommissionRule> GetCommissionRules()
        {
            CommissionRules CommissionRules = new CommissionRules();
            List<CommissionRule> commissionRuleList = new List<CommissionRule>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCommissionRules";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        commissionRuleList = CommissionRules.AddCommissionRule(FillCommissionRule(row, 0));
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

        #endregion New Commission Rule Region
    }
}
