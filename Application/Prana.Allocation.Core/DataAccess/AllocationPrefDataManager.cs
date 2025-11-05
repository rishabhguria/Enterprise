// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-09-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-09-2014
// ***********************************************************************
// <copyright file="AllocationPrefDataManager.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Core.Enums;
using Prana.Allocation.Core.Helper;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.DatabaseManager;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The DataAccess namespace.
/// </summary>
namespace Prana.Allocation.Core.DataAccess
{
    /// <summary>
    /// Database interaction point for allocation
    /// </summary>
    internal static class AllocationPrefDataManager
    {
        #region Names of the stored procedures which will return data

        /// <summary>
        /// the auto grouping preference sp
        /// </summary>
        private static readonly string _getAutoGroupingRulePref = "P_GetAutoGroupingRulePref";
        /// <summary>
        /// The _allocation preference sp
        /// </summary>
        private static readonly string _allocationPreferenceSP = "P_AL_GetAllocationPreference";
        /// <summary>
        /// The _delete allocation preference sp
        /// </summary>
        private static readonly string _deleteAllocationPreferenceSP = "P_AL_DeleteAllocationPreference";
        /// <summary>
        /// The _add allocation preference sp
        /// </summary>
        private static readonly string _addAllocationPreferenceSP = "P_AL_AddAllocationPreference";
        /// <summary>
        /// The _get allocation master fund preference sp
        /// </summary>
        private static readonly string _getAllocationMasterFundPrefSP = "P_AL_GetAllocationMasterFundPref";
        /// <summary>
        /// The _add allocation preference sp
        /// </summary>
        private static readonly string _addAllocationMasterFundPrefSP = "P_AL_AddAllocationMasterFundPref";
        /// <summary>
        /// The _delete allocation master fund preference sp
        /// </summary>
        private static readonly string _deleteAllocationMasterFundPrefSP = "P_AL_DeleteAllocationMasterFundPref";
        /// <summary>
        /// The _rename allocation master fund preference sp
        /// </summary>
        private static readonly string _renameAllocationMasterFundPrefSP = "P_AL_RenameAllocationMasterFundPref";
        /// <summary>
        /// The _save master fund preference
        /// </summary>
        private static readonly string _updateMasterFundPreference = "P_AL_UpdateMasterFundPreference";
        /// <summary>
        /// The _copy allocation preference sp
        /// </summary>
        private static readonly string _copyAllocationPreferenceSP = "P_AL_CopyAllocationPreference";

        /// <summary>
        /// The _update allocation preference sp
        /// </summary>
        private static readonly string _updateAllocationPreferenceSP = "P_AL_UpdateAllocationPreference";
        /// <summary>
        /// The _rename allocation preference sp
        /// </summary>
        private static readonly string _renameAllocationPreferenceSP = "P_AL_RenameAllocationPreference";
        /// <summary>
        /// The _state sp
        /// </summary>
        private static readonly string _stateSP = "P_AL_GetAllocationState";
        /// <summary>
        /// The _save default rule sp
        /// </summary>
        private static readonly string _saveDefaultRuleSP = "P_AL_SaveDefaultRule";
        /// <summary>
        /// _getAllocationDefaultRuleSP
        /// </summary>
        private static readonly string _getAllocationDefaultRuleSP = "P_AL_GetAllocationDefaultRule";

        /// <summary>
        /// SP name for getting symbol state
        /// </summary>
        private static readonly string _symbolStateSP = "P_AL_GetAllocationStateBySymbol";

        /// <summary>
        /// SP name for getting Account_strategy state
        /// </summary>
        private static readonly string _AllocationStateSP = "P_AL_GetAllocationStateNew";



        /// <summary>
        /// The _import allocation preference sp
        /// </summary>
        private static readonly string _importAllocationPreferenceSP = "P_AL_ImportAllocationPreference";

        private static readonly int _miscellanousTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["MiscellanousTimeout"]);

        #endregion

        /// <summary>
        /// This funtion get the values of auto grouping pref and make a auto grouping rule object.
        /// </summary>
        /// <returns>AutoGroupingRules</returns>
        internal static AutoGroupingRules GetAutoGroupingPreference(out string funds)
        {
            AutoGroupingRules autoGroupingRule = null;

            funds = string.Empty;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = _getAutoGroupingRulePref;

                DataTable dsAutoGroupingPrefs = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData).Tables[0];
                if (dsAutoGroupingPrefs != null && dsAutoGroupingPrefs.Rows.Count > 0)
                {
                    autoGroupingRule = new AutoGroupingRules();
                    autoGroupingRule.AutoGroup = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["AutoGroup"]);
                    autoGroupingRule.TradeAttributes1 = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["TradeAttribute1"]);
                    autoGroupingRule.TradeAttributes2 = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["TradeAttribute2"]);
                    autoGroupingRule.TradeAttributes3 = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["TradeAttribute3"]);
                    autoGroupingRule.TradeAttributes4 = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["TradeAttribute4"]);
                    autoGroupingRule.TradeAttributes5 = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["TradeAttribute5"]);
                    autoGroupingRule.TradeAttributes6 = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["TradeAttribute6"]);
                    autoGroupingRule.Broker = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["Broker"]);
                    autoGroupingRule.Venue = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["Venue"]);
                    autoGroupingRule.TradingAccount = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["TradingAC"]);
                    autoGroupingRule.TradeDate = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["TradeDate"]);
                    autoGroupingRule.ProcessDate = Convert.ToBoolean(dsAutoGroupingPrefs.Rows[0]["ProcessDate"]);
                    funds = dsAutoGroupingPrefs.Rows[0]["FundList"].ToString();
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
            return autoGroupingRule;
        }

        /// <summary>
        /// Returns all allocationOperationPreference for all companies
        /// </summary>
        /// <returns>Dictionary: Key - preferenceId, Value - AllocationOperationPreference</returns>
        internal static SerializableDictionary<int, AllocationOperationPreference> GetCompanyWisePreference()
        {
            try
            {
                // TODO: Method has become heavy needs to re factor and shrink to make it more readable
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = _allocationPreferenceSP;
                DataSet dsAllocationPreference = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                object resultLockerObject = new object();
                var result = new SerializableDictionary<int, AllocationOperationPreference>();

                Parallel.ForEach(dsAllocationPreference.Tables[0].AsEnumerable(), dr =>
                {

                    AllocationOperationPreference pref = GetPreferenceFromDataRow(dr);

                    lock (resultLockerObject)
                    {
                        // Adding preference to result
                        result.Add(pref.OperationPreferenceId, pref);
                    }
                });

                return result;
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
                return null;
            }
        }

        /// <summary>
        /// Returns the AllocationOperationPreference instance for given data row
        /// </summary>
        /// <param name="dr">DataRow which contains the instance information for required preference</param>
        /// <returns>AllocationOperationInstance of the given object</returns>
        private static AllocationOperationPreference GetPreferenceFromDataRow(DataRow dr)
        {
            try
            {
                #region Default preference section

                int preferenceId = Convert.ToInt32(dr["Id"]);
                int companyId = Convert.ToInt32(dr["CompanyId"]);
                int positionPrefId = Convert.ToInt32(dr["PositionPriority"]);
                string preferenceName = dr["Name"].ToString();
                DateTime updateDateTime = Convert.ToDateTime(dr["UpdateDateTime"].ToString());
                bool isPrefVisible = Convert.ToBoolean(dr["IsPrefVisible"].ToString());
                AllocationRule defaultRule = GetDefaultRuleFromDataRow(dr);

                AllocationOperationPreference allocationOperationPreference = new AllocationOperationPreference(preferenceId, companyId, positionPrefId, preferenceName, defaultRule, updateDateTime, isPrefVisible);

                #endregion

                #region Adding percentage

                List<AccountValue> accountValueList = GetAccountValueList(dr);
                foreach (AccountValue percentageAccountValue in accountValueList)
                {
                    allocationOperationPreference.TryUpdateTargetPercentage(percentageAccountValue);
                }

                #endregion

                #region Adding checklist

                List<CheckListWisePreference> checkListPreferences = GetCheckListFromDataRow(dr);
                foreach (CheckListWisePreference checkList in checkListPreferences)
                {
                    allocationOperationPreference.TryUpdateCheckList(checkList);
                }

                #endregion

                return allocationOperationPreference;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the default rule from data row.
        /// </summary>
        /// <param name="dr">The datarow.</param>
        /// <returns></returns>
        private static AllocationRule GetDefaultRuleFromDataRow(DataRow dr)
        {
            AllocationRule defaultRule = new AllocationRule();
            try
            {
                AllocationBaseType baseTypeDefault = (AllocationBaseType)Enum.Parse(typeof(AllocationBaseType), dr["AllocationBase"].ToString());
                MatchingRuleType matchingRuleTypeDefault = (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), dr["MatchingRule"].ToString());
                int preferenceAccountIdDefault = dr["PreferencedFundId"] == DBNull.Value ? -1 : Convert.ToInt32(dr["PreferencedFundId"].ToString());
                MatchClosingTransactionType matchPortfolioPostionDefault = (MatchClosingTransactionType)Enum.Parse(typeof(MatchClosingTransactionType), dr["MatchPortfolioPosition"].ToString());
                int prorataDaysBack = dr["ProrataDaysBack"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ProrataDaysBack"].ToString());

                defaultRule = new AllocationRule()
                {
                    BaseType = baseTypeDefault,
                    MatchClosingTransaction = matchPortfolioPostionDefault,
                    PreferenceAccountId = preferenceAccountIdDefault,
                    RuleType = matchingRuleTypeDefault,
                    ProrataAccountList = null,
                    ProrataDaysBack = prorataDaysBack
                };

                if (dr.Table.Columns.Contains("ProrataFundList") && !String.IsNullOrWhiteSpace(dr["ProrataFundList"].ToString()))
                {
                    DataSet accountListDataSet = GetDataSetFromXML(dr["ProrataFundList"].ToString());
                    foreach (DataRow drAccount in accountListDataSet.Tables[0].Rows)
                    {
                        int accountId = Convert.ToInt32(drAccount["FundId"]);
                        defaultRule.TryAddProrataAccount(accountId);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return defaultRule;
        }

        /// <summary>
        /// Gets the account value list from data row.
        /// </summary>
        /// <param name="dr">The datarow.</param>
        /// <returns></returns>
        private static List<AccountValue> GetAccountValueList(DataRow dr)
        {
            List<AccountValue> accountValueList = new List<AccountValue>();
            try
            {
                if (dr.Table.Columns.Contains("PercentageDataList") && !String.IsNullOrWhiteSpace(dr["PercentageDataList"].ToString()))
                {
                    DataSet percentageDataSet = GetDataSetFromXML(dr["PercentageDataList"].ToString());
                    foreach (DataRow drPercentage in percentageDataSet.Tables[0].Rows)
                    {
                        int accountId = Convert.ToInt32(drPercentage["FundId"]);
                        decimal percentage = Convert.ToDecimal(drPercentage["Value"]);
                        AccountValue percentageAccountValue = new AccountValue(accountId, percentage);

                        #region Adding Strategy

                        if (drPercentage.Table.Columns.Contains("StrategyList") && !String.IsNullOrWhiteSpace(drPercentage["StrategyList"].ToString()))
                        {
                            DataSet assetListDataSet = GetDataSetFromXML(drPercentage["StrategyList"].ToString());
                            foreach (DataRow drStrategy in assetListDataSet.Tables[0].Rows)
                            {
                                int strategyId = drStrategy["StrategyId"] == DBNull.Value ? -1 : Convert.ToInt32(drStrategy["StrategyId"]);
                                decimal strategyValue = Convert.ToDecimal(drStrategy["Value"]);
                                // Kuldeep: Added Strategy Quantity 0 as we do use only percentage in this case.
                                percentageAccountValue.StrategyValueList.Add(new StrategyValue(strategyId, strategyValue, 0));
                            }
                        }

                        #endregion
                        accountValueList.Add(percentageAccountValue);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountValueList;
        }

        /// <summary>
        /// Gets the check list target percentage from data row.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <returns></returns>
        private static SerializableDictionary<int, AccountValue> GetCheckListTargetPercentage(DataRow dr)
        {
            SerializableDictionary<int, AccountValue> targetPercentage = new SerializableDictionary<int, AccountValue>();
            try
            {

                if (dr.Table.Columns.Contains("TargetPercentageCheckListData") && !String.IsNullOrWhiteSpace(dr["TargetPercentageCheckListData"].ToString()))
                {
                    DataSet percentageDataSet = GetDataSetFromXML(dr["TargetPercentageCheckListData"].ToString());
                    foreach (DataRow drPercentage in percentageDataSet.Tables[0].Rows)
                    {
                        int accountId = Convert.ToInt32(drPercentage["AccountId"]);
                        decimal percentage = Convert.ToDecimal(drPercentage["Value"]);
                        AccountValue percentageAccountValue = new AccountValue(accountId, percentage);

                        #region Adding Strategy

                        if (drPercentage.Table.Columns.Contains("StrategyCheckListValue") && !String.IsNullOrWhiteSpace(drPercentage["StrategyCheckListValue"].ToString()))
                        {
                            DataSet assetDataSet = GetDataSetFromXML(drPercentage["StrategyCheckListValue"].ToString());
                            foreach (DataRow drStrategy in assetDataSet.Tables[0].Rows)
                            {
                                int strategyId = drStrategy["StrategyId"] == DBNull.Value ? -1 : Convert.ToInt32(drStrategy["StrategyId"]);
                                decimal strategyValue = Convert.ToDecimal(drStrategy["Value"]);
                                percentageAccountValue.StrategyValueList.Add(new StrategyValue(strategyId, strategyValue, 0));
                            }
                        }

                        #endregion
                        targetPercentage.Add(percentageAccountValue.AccountId, percentageAccountValue);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return targetPercentage;
        }

        /// <summary>
        /// Gets the check list from data row.
        /// </summary>
        /// <param name="dr">The datarow.</param>
        /// <returns></returns>
        private static List<CheckListWisePreference> GetCheckListFromDataRow(DataRow dr)
        {
            List<CheckListWisePreference> checkListPreferences = new List<CheckListWisePreference>();
            try
            {
                if (dr.Table.Columns.Contains("CheckListCollection") && !String.IsNullOrWhiteSpace(dr["CheckListCollection"].ToString()))
                {
                    DataSet checkListDataSet = GetDataSetFromXML(dr["CheckListCollection"].ToString());
                    foreach (DataRow drCheckList in checkListDataSet.Tables[0].Rows)
                    {
                        int checkListId = Convert.ToInt32(drCheckList["CheckListId"]);
                        AllocationBaseType baseTypeCheckList = (AllocationBaseType)Enum.Parse(typeof(AllocationBaseType), drCheckList["AllocationBase"].ToString());
                        MatchingRuleType matchingRuleTypeCheckList = (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), drCheckList["MatchingRule"].ToString());
                        int preferenceAccountIdCheckList = drCheckList["PreferencedFundId"] == DBNull.Value ? -1 : Convert.ToInt32(drCheckList["PreferencedFundId"].ToString());
                        MatchClosingTransactionType matchPortfolioPostionCheckList = (MatchClosingTransactionType)Enum.Parse(typeof(MatchClosingTransactionType), drCheckList["MatchPortfolioPosition"].ToString());
                        int prorataDaysBackCheckList = drCheckList["ProrataDaysBack"] == DBNull.Value ? 0 : Convert.ToInt32(drCheckList["ProrataDaysBack"].ToString());
                        SerializableDictionary<int, AccountValue> targetPercentage = GetCheckListTargetPercentage(drCheckList);

                        CheckListWisePreference checkList =
                            new CheckListWisePreference(
                                checkListId, baseTypeCheckList, matchingRuleTypeCheckList, preferenceAccountIdCheckList, matchPortfolioPostionCheckList, null, prorataDaysBackCheckList, targetPercentage);

                        #region Adding asset

                        // Adding asset
                        CustomOperator operatorAsset = (CustomOperator)Enum.Parse(typeof(CustomOperator), drCheckList["AssetOperator"].ToString());
                        checkList.TryUpdateAssetCheck(operatorAsset, new List<int>());
                        if (drCheckList.Table.Columns.Contains("AssetList") && !String.IsNullOrWhiteSpace(drCheckList["AssetList"].ToString()))
                        {
                            DataSet assetListDataSet = GetDataSetFromXML(drCheckList["AssetList"].ToString());
                            foreach (DataRow drAsset in assetListDataSet.Tables[0].Rows)
                            {
                                int assetId = Convert.ToInt32(drAsset["AssetId"]);
                                checkList.TryAddAsset(assetId);
                            }
                        }

                        #endregion

                        #region Adding exchange
                        // Adding exchange
                        CustomOperator operatorExchange = (CustomOperator)Enum.Parse(typeof(CustomOperator), drCheckList["ExchangeOperator"].ToString());
                        checkList.TryUpdateExchangeCheck(operatorExchange, new List<int>());
                        if (drCheckList.Table.Columns.Contains("ExchangeList") && !String.IsNullOrWhiteSpace(drCheckList["ExchangeList"].ToString()))
                        {
                            DataSet exchangeListDataSet = GetDataSetFromXML(drCheckList["ExchangeList"].ToString());
                            foreach (DataRow drExchange in exchangeListDataSet.Tables[0].Rows)
                            {
                                int exchangeId = Convert.ToInt32(drExchange["ExchangeId"]);
                                checkList.TryAddExchange(exchangeId);
                            }
                        }
                        #endregion

                        #region Adding order side
                        // Adding order side
                        CustomOperator operatorOrderSide = (CustomOperator)Enum.Parse(typeof(CustomOperator), drCheckList["OrderSideOperator"].ToString());
                        checkList.TryUpdateOrderSide(operatorOrderSide, new List<string>());
                        if (drCheckList.Table.Columns.Contains("OrderSideList") && !String.IsNullOrWhiteSpace(drCheckList["OrderSideList"].ToString()))
                        {
                            DataSet orderSideListDataSet = GetDataSetFromXML(drCheckList["OrderSideList"].ToString());
                            foreach (DataRow drExchange in orderSideListDataSet.Tables[0].Rows)
                            {
                                string orderSideId = Convert.ToString(drExchange["OrderSideId"]);
                                checkList.TryAddOrderSide(orderSideId);
                            }
                        }
                        #endregion

                        #region Adding PR
                        // Adding PR
                        CustomOperator operatorPR = (CustomOperator)Enum.Parse(typeof(CustomOperator), drCheckList["PROperator"].ToString());
                        checkList.TryUpdatePRCheck(operatorPR, new List<string>());
                        if (drCheckList.Table.Columns.Contains("PRList") && !String.IsNullOrWhiteSpace(drCheckList["PRList"].ToString()))
                        {
                            DataSet prListDataSet = GetDataSetFromXML(drCheckList["PRList"].ToString());
                            foreach (DataRow drPR in prListDataSet.Tables[0].Rows)
                            {
                                string pr = drPR["PRId"].ToString();
                                checkList.TryAddPR(pr);
                            }
                        }
                        #endregion

                        if (drCheckList.Table.Columns.Contains("ProrataFundList") && !String.IsNullOrWhiteSpace(drCheckList["ProrataFundList"].ToString()))
                        {
                            DataSet accountListDataSet = GetDataSetFromXML(drCheckList["ProrataFundList"].ToString());
                            foreach (DataRow drAccount in accountListDataSet.Tables[0].Rows)
                            {
                                int accountId = Convert.ToInt32(drAccount["FundId"]);
                                checkList.TryAddProrataAccount(accountId);
                            }
                        }

                        checkListPreferences.Add(checkList);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return checkListPreferences;
        }

        /// <summary>
        /// Convert XML string to dataset
        /// </summary>
        /// <param name="xmlString">XML string to be converted into dataset</param>
        /// <returns>Dataset created from XML string</returns>
        private static DataSet GetDataSetFromXML(string xmlString)
        {
            try
            {
                xmlString = xmlString.Replace("http://schemas.microsoft.com/sqlserver/2004/sqltypes/sqltypes.xsd", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\SqlTypes.xsd");

                StringReader xmlReader = new StringReader(xmlString);
                DataSet xmlDataSet = new DataSet();
                xmlDataSet.ReadXml(xmlReader);
                return xmlDataSet;
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
                return null;
            }
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <param name="updateDateTime">The update date time.</param>
        /// <param name="stateNotional">The state notional.</param>
        /// <param name="stateCumQuantity">The state cum quantity.</param>
        internal static void GetState(DateTime updateDateTime, out SerializableDictionary<string, List<AccountValue>> stateNotional, out SerializableDictionary<string, List<AccountValue>> stateCumQuantity)
        {
            stateNotional = new SerializableDictionary<string, List<AccountValue>>();
            stateCumQuantity = new SerializableDictionary<string, List<AccountValue>>();

            try
            {
                DataTable dt = new DataTable();
                //DataSet ds = db.ExecuteDataSet(_stateSP, new object[] { updateDateTime });

                //https://jira.nirvanasolutions.com:8443/browse/PRANA-18279
                //When the number of records in database is significantly large, the time taken to execute this stored procedure is greater than
                //the default timeout, therefore, adding the timeout explicitly.

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = _stateSP;
                queryData.CommandTimeout = 300;
                queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FromDate",
                    ParameterType = DbType.String,
                    ParameterValue = updateDateTime
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    dt.Load(reader);
                }

                foreach (DataRow dr in dt.Rows)
                {
                    string symbol = dr["Symbol"].ToString();
                    if (!stateNotional.ContainsKey(symbol))
                        stateNotional.Add(symbol, new List<AccountValue>());
                    if (!stateCumQuantity.ContainsKey(symbol))
                        stateCumQuantity.Add(symbol, new List<AccountValue>());

                    if (string.IsNullOrEmpty(dr["FundID"].ToString()) || string.IsNullOrEmpty(dr["NetNotional"].ToString()) || string.IsNullOrEmpty(dr["CumQuantity"].ToString()))
                    {
                        Exception ex = new Exception("There is data corruption error regarding the symbol:" + symbol);
                        throw ex;
                    }

                    int accountId = Convert.ToInt32(dr["FundID"]);

                    decimal notional = Convert.ToDecimal(dr["NetNotional"]);
                    decimal cumQuantity = Convert.ToDecimal(dr["CumQuantity"]);

                    AccountValue cumQuantityAccountValue = new AccountValue(accountId, cumQuantity);
                    AccountValue NotionalAccountValue = new AccountValue(accountId, notional);

                    stateCumQuantity[symbol].Add(cumQuantityAccountValue);
                    stateNotional[symbol].Add(NotionalAccountValue);

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
        /// Gets the state.
        /// </summary>
        /// <param name="updateDateTime">The update date time.</param>
        /// <param name="stateNotional">The state notional.</param>
        /// <param name="stateCumQuantity">The state cum quantity.</param>
        internal static void GetAllocationStateWithAccountStrategy(DateTime updateDateTime, out SerializableDictionary<string, List<AllocationState>> stateNotional, out SerializableDictionary<string, List<AllocationState>> stateCumQuantity)
        {
            stateCumQuantity = new SerializableDictionary<string, List<AllocationState>>();
            stateNotional = new SerializableDictionary<string, List<AllocationState>>();

            try
            {

                DataTable dt = new DataTable();
                //DataSet ds = db.ExecuteDataSet(_stateSP, new object[] { updateDateTime });

                //https://jira.nirvanasolutions.com:8443/browse/PRANA-18279
                //When the number of records in database is significantly large, the time taken to execute this stored procedure is greater than
                //the default timeout, therefore, adding the timeout explicitly.

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = _AllocationStateSP;
                queryData.CommandTimeout = 300;
                queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FromDate",
                    ParameterType = DbType.String,
                    ParameterValue = updateDateTime
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    dt.Load(reader);
                }


                foreach (DataRow dr in dt.Rows)
                {
                    string symbol = dr["Symbol"].ToString();


                    if (string.IsNullOrEmpty(dr["FundID"].ToString()) || string.IsNullOrEmpty(dr["Level2ID"].ToString()) || string.IsNullOrEmpty(dr["CumQuantity"].ToString()))
                    {
                        Exception ex = new Exception("There is data corruption error regarding the symbol:" + symbol);
                        throw ex;
                    }

                    int accountId = Convert.ToInt32(dr["FundID"]);

                    int level2Id = Convert.ToInt32(dr["Level2ID"]);
                    string orderSideTagValue = dr["OrderSideTagValue"].ToString();

                    decimal notional = Convert.ToDecimal(dr["NetNotional"]);
                    decimal cumQuantity = Convert.ToDecimal(dr["CumQuantity"]);

                    AllocationState cumQuantityAccountValue = new AllocationState()
                    {
                        AccountId = accountId,
                        Level2ID = level2Id,
                        OrderSideTagValue = orderSideTagValue,
                        cumQuantity = cumQuantity,
                        Notional = notional
                    };

                    if (CheckSideHelper.GetPositionKey(orderSideTagValue).Equals(GroupPositionType.LongClosing) ||
                      CheckSideHelper.GetPositionKey(orderSideTagValue).Equals(GroupPositionType.ShortClosing))
                    {
                        orderSideTagValue = CheckSideHelper.GetOpeningOrderSideTagValue(orderSideTagValue);
                        cumQuantityAccountValue.OrderSideTagValue = orderSideTagValue;
                    }



                    if (stateCumQuantity.ContainsKey(symbol))
                    {
                        AllocationState val = stateCumQuantity[symbol].Find(f => f.AccountId == accountId && f.Level2ID == level2Id && f.OrderSideTagValue == orderSideTagValue);
                        if (val == null)
                        {
                            stateCumQuantity[symbol].Add(cumQuantityAccountValue.Clone());
                        }
                        else
                        {
                            val.AddValueCumQuantity(cumQuantity);
                        }
                    }
                    else
                    {

                        stateCumQuantity.Add(symbol, new List<AllocationState>() { cumQuantityAccountValue });
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
        /// Updates the current preference in database and return result object
        /// </summary>
        /// <param name="preference">Preference to be updated</param>
        /// <returns>Update result object</returns>
        internal static PreferenceUpdateResult UpdatePreference(AllocationOperationPreference preference)
        {
            try
            {
                CustomXmlSerializer ser = new CustomXmlSerializer();
                String res = ser.WriteString(preference);
                res = res.Replace("KeyValuePair`2", "KeyValuePair");

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(_updateAllocationPreferenceSP, new object[] { res }, string.Empty, _miscellanousTimeout);

                if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    AllocationOperationPreference pref = GetPreferenceFromDataRow(ds.Tables[0].Rows[0]);
                    PreferenceUpdateResult result = new PreferenceUpdateResult() { Error = null, Preference = pref };

                    return result;
                }
                else
                    return null;
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
                return null;
            }
        }

        /// <summary>
        /// Import the preference in database and return result object
        /// </summary>
        /// <param name="preference">Preference to be imported</param>
        /// <returns>imported result object</returns>
        internal static PreferenceUpdateResult ImportPreference(AllocationOperationPreference preference)
        {
            try
            {
                CustomXmlSerializer ser = new CustomXmlSerializer();
                String res = ser.WriteString(preference);
                res = res.Replace("KeyValuePair`2", "KeyValuePair");

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(_importAllocationPreferenceSP, new object[] { res });

                if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    AllocationOperationPreference pref = GetPreferenceFromDataRow(ds.Tables[0].Rows[0]);
                    PreferenceUpdateResult result = new PreferenceUpdateResult() { Error = null, Preference = pref };

                    return result;
                }
                else
                    return null;
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
                return null;
            }
        }


        /// Copy preference from given preferenceId to new name
        /// </summary>
        /// <param name="idToCopyFrom">Id of the source to copy</param>
        /// <param name="newName">Name which will be of new preference</param>
        /// <returns>Update result object</returns>
        internal static PreferenceUpdateResult CopyPreference(int idToCopyFrom, string newName)
        {
            PreferenceUpdateResult res = new PreferenceUpdateResult();
            try
            {
                DataSet dsallocationpreference = DatabaseManager.DatabaseManager.ExecuteDataSet(_copyAllocationPreferenceSP, new object[] { idToCopyFrom, newName });
                if (dsallocationpreference.Tables != null && dsallocationpreference.Tables.Count > 0 && dsallocationpreference.Tables[0].Rows.Count > 0)
                {
                    AllocationOperationPreference pref = GetPreferenceFromDataRow(dsallocationpreference.Tables[0].Rows[0]);
                    PreferenceUpdateResult result = new PreferenceUpdateResult() { Error = null, Preference = pref };
                    res = result;
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
                return null;
            }
            return res;
        }

        /// <summary>
        /// A new preference will be added with given value
        /// </summary>
        /// <param name="companyId">CompanyId for which preference will be saved</param>
        /// <param name="name">Name of he new preference</param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns>Update result object</returns>
        internal static PreferenceUpdateResult AddPreference(int companyId, string name, AllocationPreferencesType allocationPrefType, bool isPrefVisible, string rebalancerFileName = "")
        {
            try
            {
                PreferenceUpdateResult result = null;

                switch (allocationPrefType)
                {
                    case AllocationPreferencesType.CalculatedAllocationPreference:
                        DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(_addAllocationPreferenceSP, new object[] { companyId, name, isPrefVisible, rebalancerFileName });

                        if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            AllocationOperationPreference pref = GetPreferenceFromDataRow(ds.Tables[0].Rows[0]);
                            result = new PreferenceUpdateResult() { Error = null, Preference = pref };
                        }
                        else
                            return new PreferenceUpdateResult() { Error = "Error while fetching calculated preference from DB" }; ;
                        break;
                    case AllocationPreferencesType.MasterFundAllocationPreference:
                        DataSet dataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(_addAllocationMasterFundPrefSP, new object[] { companyId, name });

                        if (dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            AllocationMasterFundPreference mfPref = GetMasterFundPrefFromDataRow(dataSet.Tables[0].Rows[0]);
                            result = new PreferenceUpdateResult() { Error = null, MasterFundPreference = mfPref };
                        }
                        else
                            return new PreferenceUpdateResult() { Error = "Error while fetching master fund preference from DB" }; ;
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Delete the preference with given preferenceId
        /// </summary>
        /// <param name="preferenceId">Id of the preference which will be deleted</param>
        /// <returns>Update result object</returns>
        internal static PreferenceUpdateResult DeletePreference(int preferenceId)
        {
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery(_deleteAllocationPreferenceSP, new object[] { preferenceId });

                PreferenceUpdateResult updateResult = new PreferenceUpdateResult() { Error = null, Preference = null };
                // TODO: Delete from database and return null
                return updateResult;
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
                return null;
            }
        }

        /// <summary>
        /// Rename the preference with given preferenceId
        /// </summary>
        /// <param name="preferenceId">Id of the preference which will be renamed</param>
        /// <param name="name">New Name of the preference</param>
        /// <returns>Update result object</returns>
        internal static PreferenceUpdateResult RenamePreference(int preferenceId, string name)
        {
            try
            {
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(_renameAllocationPreferenceSP, new object[] { preferenceId, name });

                if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    AllocationOperationPreference pref = GetPreferenceFromDataRow(ds.Tables[0].Rows[0]);
                    PreferenceUpdateResult result = new PreferenceUpdateResult() { Error = null, Preference = pref };

                    return result;
                }
                else
                    return null;
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
                return null;
            }
        }

        /// <summary>
        /// Saves Default rule for company.
        /// </summary>
        /// <param name="defaultRule">Allocation rule</param>
        /// <param name="companyId">Company id</param>
        /// <returns></returns>
        internal static bool SaveCompanyWisePreference(AllocationCompanyWisePref defaultPref)
        {
            try
            {

                string checkSidePref = JsonHelper.SerializeObject(defaultPref.AllocationCheckSidePref);

                int rows = DatabaseManager.DatabaseManager.ExecuteNonQuery(_saveDefaultRuleSP, new object[]
                        {
                            defaultPref.CompanyId,
                            (int)defaultPref.DefaultRule.BaseType,
                            (int)defaultPref.DefaultRule.RuleType,
                            defaultPref.DefaultRule.MatchClosingTransaction,
                            defaultPref.DefaultRule.PreferenceAccountId,
                            defaultPref.AllowEditPreferences,
                            GeneralUtilities.GetStringFromList(defaultPref.DefaultRule.ProrataAccountList.Select(s=> s.ToString()).ToList(),','),
                            defaultPref.DefaultRule.ProrataDaysBack,
                            defaultPref.PrecisionDigit,
                            GeneralUtilities.GetStringFromList(defaultPref.AssetsWithCommissionInNetAmount.Select(s=> s.ToString()).ToList(),','),
                            defaultPref.MsgOnBrokerChange,
                            defaultPref.RecalculateOnBrokerChange,
                            defaultPref.MsgOnAllocation,
                            defaultPref.RecalculateOnAllocation,
                            defaultPref.EnableMasterFundAllocation,
                            defaultPref.IsOneSymbolOneMasterFundAllocation,
                            defaultPref.SelectedProrataSchemeName,
                            (int)(Enum.Parse(typeof(AllocationSchemeKey), defaultPref.SelectedProrataSchemeBasis)),
                            defaultPref.SetSchemeGenerationAttributesProrata,
                            checkSidePref
                        });
                return true;
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
                return false;
            }
        }

        /// <summary>
        /// Gets company wise default rule from DB
        /// </summary>
        /// <returns>dictionary of rules with company id</returns>
        internal static Dictionary<int, AllocationCompanyWisePref> GetAllDefaultRule()
        {
            try
            {
                Dictionary<int, AllocationCompanyWisePref> ruleCache = new Dictionary<int, AllocationCompanyWisePref>();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = _getAllocationDefaultRuleSP;

                DataSet dsAllocationRule = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                foreach (DataRow row in dsAllocationRule.Tables[0].Rows)
                {
                    int companyId = Convert.ToInt32(row["CompanyId"]);
                    bool doCheckSide = row["DoCheckSide"] == DBNull.Value ? false : Convert.ToBoolean(row["DoCheckSide"]);
                    bool allowEditPreferences = row["AllowEditPreferences"] == DBNull.Value ? false : Convert.ToBoolean(row["AllowEditPreferences"]);
                    AllocationRule defaultRule = GetDefaultRule(row);
                    //Added to get precision digit and checkside list, PRANA-12154, PRANA-12148
                    int precisionDigit = row["PrecisionDigit"] == DBNull.Value ? 0 : Convert.ToInt32(row["PrecisionDigit"]);
                    // List<int> disableCheckSideForAssets = GeneralUtilities.GetListFromString(row["DisableCheckSideForAssets"].ToString(), ',').Select(s => int.Parse(s)).ToList();
                    AllocationCheckSidePref disableCheckSidePref = JsonHelper.DeserializeToObject<AllocationCheckSidePref>(row["CheckSidePreference"].ToString());

                    List<int> assetsWithCommissionInNetAmount = GeneralUtilities.GetListFromString(row["AssetsWithCommissionInNetAmount"].ToString(), ',').Select(s => int.Parse(s)).ToList();
                    bool msgOnBrokerChange = row["MsgOnBrokerChange"] == DBNull.Value ? false : Convert.ToBoolean(row["MsgOnBrokerChange"]);
                    bool recalculateOnBrokerChange = row["RecalculateOnBrokerChange"] == DBNull.Value ? false : Convert.ToBoolean(row["RecalculateOnBrokerChange"]);
                    bool msgOnAllocation = row["MsgOnAllocation"] == DBNull.Value ? false : Convert.ToBoolean(row["MsgOnAllocation"]);
                    bool recalculateOnAllocation = row["RecalculateOnAllocation"] == DBNull.Value ? false : Convert.ToBoolean(row["RecalculateOnAllocation"]);
                    bool enableMasterFundAllocation = row["EnableMasterFundAllocation"] == DBNull.Value ? false : Convert.ToBoolean(row["EnableMasterFundAllocation"]);
                    bool isOneSymbolOneMasterFundAllocation = row["IsOneSymbolOneMasterFundAllocation"] == DBNull.Value ? false : Convert.ToBoolean(row["IsOneSymbolOneMasterFundAllocation"]);
                    string selectedProrataSchemeName = row["ProrataSchemeName"].ToString();
                    int selectedProrataSchemeBasis = Convert.ToInt32(row["AllocationSchemeKey"]);
                    bool setSchemeGenerationAttributesProrata = row["SetSchemeFromUI"] == DBNull.Value ? false : Convert.ToBoolean(row["SetSchemeFromUI"]);
                    AllocationCompanyWisePref defaultPref = new AllocationCompanyWisePref()
                    {
                        CompanyId = companyId,
                        DefaultRule = defaultRule,
                        AllowEditPreferences = allowEditPreferences,
                        // DisableCheckSideForAssets = disableCheckSideForAssets,
                        PrecisionDigit = precisionDigit,
                        AssetsWithCommissionInNetAmount = assetsWithCommissionInNetAmount,
                        MsgOnBrokerChange = msgOnBrokerChange,
                        RecalculateOnBrokerChange = recalculateOnBrokerChange,
                        MsgOnAllocation = msgOnAllocation,
                        RecalculateOnAllocation = recalculateOnAllocation,
                        EnableMasterFundAllocation = enableMasterFundAllocation,
                        IsOneSymbolOneMasterFundAllocation = isOneSymbolOneMasterFundAllocation,
                        SelectedProrataSchemeName = selectedProrataSchemeName,
                        SelectedProrataSchemeBasis = ((AllocationSchemeKey)selectedProrataSchemeBasis).ToString(),
                        SetSchemeGenerationAttributesProrata = setSchemeGenerationAttributesProrata,
                        AllocationCheckSidePref = disableCheckSidePref
                    };
                    if (ruleCache.ContainsKey(companyId))
                        ruleCache[companyId] = defaultPref;
                    else
                        ruleCache.Add(defaultPref.CompanyId, defaultPref);
                }
                return ruleCache;
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
                return null;
            }
        }

        /// <summary>
        /// Returns default rule for data row
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>AllocationRule</returns>
        private static AllocationRule GetDefaultRule(DataRow dr)
        {
            try
            {
                #region Default preference section

                AllocationBaseType baseTypeDefault = (AllocationBaseType)Enum.Parse(typeof(AllocationBaseType), dr["AllocationBase"].ToString());
                MatchingRuleType matchingRuleTypeDefault = (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), dr["MatchingRule"].ToString());
                int preferenceAccountIdDefault = dr["PreferencedFundId"] == DBNull.Value ? -1 : Convert.ToInt32(dr["PreferencedFundId"].ToString());
                MatchClosingTransactionType matchPortfolioPostionDefault = (MatchClosingTransactionType)Enum.Parse(typeof(MatchClosingTransactionType), dr["MatchPortfolioPosition"].ToString());
                List<int> prorataAccountList = GeneralUtilities.GetListFromString(dr["ProrataFundList"].ToString(), ',').Select(s => int.Parse(s)).ToList();
                int prorataDaysBack = dr["ProrataDaysBack"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ProrataDaysBack"].ToString());

                AllocationRule defaultRule = new AllocationRule()
                {
                    BaseType = baseTypeDefault,
                    MatchClosingTransaction = matchPortfolioPostionDefault,
                    PreferenceAccountId = preferenceAccountIdDefault,
                    RuleType = matchingRuleTypeDefault,
                    ProrataAccountList = prorataAccountList,
                    ProrataDaysBack = prorataDaysBack
                };
                return defaultRule;
                #endregion
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
                return null;
            }
        }

        /// <summary>
        /// Gets the trade attrbs lists.
        /// </summary>
        /// <returns></returns>
        public static List<string>[] GetTradeAttrbsLists()
        {
            List<string>[] attribLists = new List<string>[45];
            for (int i = 0; i < 45; i++)
            {
                attribLists[i] = new List<string>();
            }

            for (int i = 1; i <= 6; i++)
            {
                List<string> attribList = attribLists[i - 1];
                //DataSet ds = db.ExecuteDataSet(CommandType.Text, "Select distinct TradeAttribute" + i + " from T_Group");

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetTradeAttribute" + i + "List";

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string attrb = dr[0].ToString();
                    if (attrb != null && attrb.Length > 0)
                    {
                        attribList.Add(attrb);
                    }
                }
            }

            #region Populate Additional Trade Attributes

            QueryData query = new QueryData
            {
                StoredProcedureName = "P_GetAdditionalTradeAttributesList"
            };

            DataSet tradeAttributesDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(query);
            foreach (DataRow dr in tradeAttributesDataSet.Tables[0].Rows)
            {
                if (dr.IsNull(0) || dr.IsNull(1))
                    continue;

                // Column 0: AttributeNumber (expected INT, typically between 7 and 45)
                // Column 1: DataItem (expected STRING representing a trade attribute value)
                if (int.TryParse(dr[0].ToString(), out int attrb) && attrb >= 1 && attrb <= attribLists.Length)
                {
                    attribLists[attrb - 1].Add(dr[1].ToString().Trim());
                }              
            }

            #endregion

            return attribLists;
        }

        /// <summary>
        /// Gets symbol state from DB
        /// </summary>
        /// <param name="updateDateTime">To date</param>
        /// <param name="stateNotional">Notional state</param>
        /// <param name="stateCumQuantity">Quantity state</param>
        /// <param name="symbol"></param>
        internal static void GetStateForSymbol(DateTime updateDateTime, out SerializableDictionary<int, AccountValue> stateNotional, out SerializableDictionary<int, AccountValue> stateCumQuantity, string symbol, StringBuilder groupIds)
        {
            stateNotional = new SerializableDictionary<int, AccountValue>();
            stateCumQuantity = new SerializableDictionary<int, AccountValue>();

            if (groupIds.Length != 0)
                groupIds.Length--;

            try
            {
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(_symbolStateSP, new object[] { updateDateTime, symbol, groupIds.ToString() });

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //string symbol = dr["Symbol"].ToString();
                    //if (!stateNotional.ContainsKey(symbol))
                    // stateNotional.Add(symbol, new List<AccountValue>());
                    //if (!stateCumQuantity.ContainsKey(symbol))
                    //stateCumQuantity.Add(symbol, new List<AccountValue>());

                    int accountId = Convert.ToInt32(dr["FundID"]);

                    decimal notional = Convert.ToDecimal(dr["NetNotional"]);
                    decimal cumQuantity = Convert.ToDecimal(dr["CumQuantity"]);

                    AccountValue cumQuantityAccountValue = new AccountValue(accountId, cumQuantity);
                    AccountValue NotionalAccountValue = new AccountValue(accountId, notional);

                    stateCumQuantity.Add(accountId, cumQuantityAccountValue);
                    stateNotional.Add(accountId, NotionalAccountValue);

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
        /// Getting master funds
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllMasterFunds()
        {
            DataSet ds = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMasterFundTargetRatio";
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ds;
        }

        /// <summary>
        /// Saves the master fund target ratio.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        internal static bool SaveMasterFundTargetRatio(DataSet ds)
        {
            bool isSaved = false;
            try
            {
                int rowsAffected = 0;
                int errorNumber = 0;
                string errorMessage = string.Empty;
                ds.Tables[0].TableName = "MasterFund";
                string generatedXml = string.Empty;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveMasterFundTargetRatio";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref errorMessage, ref errorNumber, queryData.DictionaryDatabaseParameter);

                isSaved = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSaved;
        }

        /// <summary>
        /// Saves the attribute names.
        /// </summary>
        /// <param name="ds">The ds.</param>
        internal static void SaveAttributeNames(DataSet ds)
        {
            try
            {
                int rowsAffected = 0;
                int errorNumber = 0;
                string errorMessage = string.Empty;
                ds.Tables[0].TableName = "Attributes";
                string generatedXml = string.Empty;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveAttributeNames";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref errorMessage, ref errorNumber, queryData.DictionaryDatabaseParameter);
                if (rowsAffected > 0)
                    CommonDataCache.CachedDataManager.RefreshAttibutesCache(ds);
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
        /// Gets the attribute names.
        /// </summary>
        /// <returns></returns>
        internal static DataSet GetAttributeNames()
        {
            try
            {
                return CommonDataCache.CachedDataManager.GetInstance.GetAttributeNames();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataSet();
        }

        /// <summary>
        /// Gets the allocation master fund preferences.
        /// </summary>
        /// <returns></returns>
        internal static SerializableDictionary<int, AllocationMasterFundPreference> GetAllocationMasterFundPreferences()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = _getAllocationMasterFundPrefSP;

                DataSet dsAllocationMasterFundPref = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                object resultLockerObject = new object();
                var result = new SerializableDictionary<int, AllocationMasterFundPreference>();

                Parallel.ForEach(dsAllocationMasterFundPref.Tables[0].AsEnumerable(), dr =>
                {

                    AllocationMasterFundPreference mfPref = GetMasterFundPrefFromDataRow(dr);

                    lock (resultLockerObject)
                    {
                        result.Add(mfPref.MasterFundPreferenceId, mfPref);
                    }
                });

                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the master fund preference from data row.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <returns></returns>
        private static AllocationMasterFundPreference GetMasterFundPrefFromDataRow(DataRow dr)
        {
            try
            {
                int mfPreferenceId = Convert.ToInt32(dr["MFPreferenceId"]);
                int companyId = Convert.ToInt32(dr["CompanyId"]);
                string mfPreferenceName = dr["MFPreferenceName"].ToString();
                DateTime updateDateTime = Convert.ToDateTime(dr["UpdateDateTime"].ToString());
                SerializableDictionary<int, int> masterFundPref = new SerializableDictionary<int, int>();
                SerializableDictionary<int, decimal> masterFundTargetPercentage = new SerializableDictionary<int, decimal>();
                AllocationRule defaultRule = GetDefaultRuleFromDataRow(dr);

                if (dr.Table.Columns.Contains("MasterFundCalculatedPref") && !String.IsNullOrWhiteSpace(dr["MasterFundCalculatedPref"].ToString()))
                {
                    DataSet accountListDataSet = GetDataSetFromXML(dr["MasterFundCalculatedPref"].ToString());
                    foreach (DataRow drAccount in accountListDataSet.Tables[0].Rows)
                    {
                        int masterFundId = Convert.ToInt32(drAccount["MFId"]);
                        if (!masterFundPref.ContainsKey(masterFundId))
                        {
                            int calculatedPrefId = Convert.ToInt32(drAccount["CalculatedPrefId"]);
                            masterFundPref.Add(masterFundId, calculatedPrefId);
                        }
                    }
                }

                if (dr.Table.Columns.Contains("MasterFundTargetPercentage") && !String.IsNullOrWhiteSpace(dr["MasterFundTargetPercentage"].ToString()))
                {
                    DataSet accountListDataSet = GetDataSetFromXML(dr["MasterFundTargetPercentage"].ToString());
                    foreach (DataRow drAccount in accountListDataSet.Tables[0].Rows)
                    {
                        int masterFundId = Convert.ToInt32(drAccount["MFId"]);
                        if (!masterFundTargetPercentage.ContainsKey(masterFundId))
                        {
                            decimal value = Convert.ToDecimal(drAccount["Value"]);
                            masterFundTargetPercentage.Add(masterFundId, value);
                        }
                    }
                }

                AllocationMasterFundPreference masterFundPreference = new AllocationMasterFundPreference(mfPreferenceId, companyId, mfPreferenceName, updateDateTime, defaultRule);
                masterFundPreference.UpdateTargetPercentage(masterFundTargetPercentage);
                masterFundPreference.UpdateMasterFundPreference(masterFundPref);
                return masterFundPreference;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Deletes the master fund preference.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns></returns>
        internal static PreferenceUpdateResult DeleteMasterFundPreference(int preferenceId)
        {
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery(_deleteAllocationMasterFundPrefSP, new object[] { preferenceId });

                PreferenceUpdateResult updateResult = new PreferenceUpdateResult() { Error = null, MasterFundPreference = null };
                // TODO: Delete from database and return null
                return updateResult;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Renames the master fund preference.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        internal static PreferenceUpdateResult RenameMasterFundPreference(int preferenceId, string name)
        {
            try
            {
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(_renameAllocationMasterFundPrefSP, new object[] { preferenceId, name });

                if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    AllocationMasterFundPreference allocationMfPref = GetMasterFundPrefFromDataRow(ds.Tables[0].Rows[0]);
                    PreferenceUpdateResult result = new PreferenceUpdateResult() { Error = null, MasterFundPreference = allocationMfPref };

                    return result;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Saves the master fund preference.
        /// </summary>
        /// <param name="mfPreference">The mf preference.</param>
        /// <returns></returns>
        internal static PreferenceUpdateResult SaveMasterFundPreference(AllocationMasterFundPreference mfPreference, List<AllocationOperationPreference> mfCalculatedPrefs)
        {
            try
            {
                CustomXmlSerializer ser = new CustomXmlSerializer();
                String res = ser.WriteString(mfPreference);
                res = res.Replace("KeyValuePair`2", "KeyValuePair");

                CustomXmlSerializer serCalculatedPrefs = new CustomXmlSerializer();
                String resultCalPrefs = serCalculatedPrefs.WriteString(mfCalculatedPrefs);
                resultCalPrefs = resultCalPrefs.Replace("KeyValuePair`2", "KeyValuePair");
                resultCalPrefs = resultCalPrefs.Replace("List`1", "List");

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(_updateMasterFundPreference, new object[] { res, resultCalPrefs });

                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    List<AllocationOperationPreference> mfCalPrefList = new List<AllocationOperationPreference>();
                    AllocationMasterFundPreference mfPref = null;
                    if (ds.Tables[ds.Tables.Count - 1].Rows.Count > 0)
                        mfPref = GetMasterFundPrefFromDataRow(ds.Tables[ds.Tables.Count - 1].Rows[0]);

                    for (int i = 0; i <= ds.Tables.Count - 2; i++)
                    {
                        AllocationOperationPreference mfCalPref = GetPreferenceFromDataRow(ds.Tables[i].Rows[0]);
                        mfCalPrefList.Add(mfCalPref);
                    }

                    PreferenceUpdateResult result = new PreferenceUpdateResult() { Error = null, MasterFundPreference = mfPref, MasterFundCalculatedPreferences = mfCalPrefList };
                    return result;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Deletes the allocation scheme.
        /// </summary>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <param name="SchemeName">Name of the scheme.</param>
        /// <returns></returns>
        internal static int DeleteAllocationScheme(int schemeID, string SchemeName)
        {
            int affectedRow = 0;
            try
            {
                object[] parameter = new object[2];

                parameter[0] = schemeID;
                parameter[1] = SchemeName;

                string spName = "P_DeleteAllocationScheme";
                affectedRow = DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return affectedRow;
        }

        /// <summary>
        /// Gets the name of the allocation scheme by.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <returns></returns>
        internal static AllocationFixedPreference GetAllocationSchemeByName(string allocationSchemeName)
        {
            AllocationFixedPreference allocationScheme = null;
            DataSet dsAllocationScheme = new DataSet();
            try
            {
                object[] parameter = new object[1];

                parameter[0] = allocationSchemeName;
                string spName = "P_GetAllocationSchemeByName";
                dsAllocationScheme = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameter);
                DataRow dr = dsAllocationScheme.Tables[0].Rows[0];
                string scheme = dr["AllocationScheme"].ToString();
                allocationScheme = new AllocationFixedPreference(dr, scheme);


            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationScheme;
        }

        /// <summary>
        /// Gets the allocation schemes.
        /// </summary>
        /// <returns>Dictionary containing allocation Schemes with scheme name as key</returns>
        internal static Dictionary<string, AllocationFixedPreference> GetAllocationSchemes()
        {
            Dictionary<string, AllocationFixedPreference> allocationSchemes = new Dictionary<string, AllocationFixedPreference>();
            DataSet dsAllocationSchemes = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllocationSchemes";

                dsAllocationSchemes = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                foreach (DataRow dr in dsAllocationSchemes.Tables[0].Rows)
                {
                    allocationSchemes.Add(dr["AllocationSchemeName"].ToString(), new AllocationFixedPreference(dr, string.Empty));

                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationSchemes;
        }

        /// <summary>
        /// Saves the allocation scheme.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="allocationSchemeDate">The allocation scheme date.</param>
        /// <param name="allocationSchemeXML">The allocation scheme XML.</param>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns></returns>
        public static int SaveAllocationScheme(AllocationFixedPreference fixedPref)
        {
            int affectedRow = 0;
            try
            {
                object[] parameter = new object[6];

                parameter[0] = fixedPref.SchemeName;
                parameter[1] = fixedPref.Date;
                parameter[2] = fixedPref.Scheme;
                parameter[3] = fixedPref.SchemeID;
                parameter[4] = fixedPref.IsPrefVisible ? 1 : 0;
                parameter[5] = (int)fixedPref.CreationSource;

                string spName = "P_SaveAllocationScheme";
                affectedRow = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar(spName, parameter).ToString());
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
            return affectedRow;
        }

    }
}

