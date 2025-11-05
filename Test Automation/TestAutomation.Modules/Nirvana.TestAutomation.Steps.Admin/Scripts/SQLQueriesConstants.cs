using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.Admin.Scripts
{
    /// <summary>
    /// SQL Queries for Admin module.
    /// </summary>
    public static class SQLQueriesConstants
    {
        public static string clearanceTimeUPdateSQLQuery;
        public static string UpdateCashPreferences;
        public static string updateTradingRulesQuery;
        public static string updateTTGeneralPrefQuery;
        public static string updateMasterUserQuery;
        public static int count = 0;
        public static string isEnableModule;
        public static string updateDollarAmountPermissionQuery;
        public static string ShortLocateStructures;
        public static string updateComplianceAlert;
        public static string importOverrideOnShortLocate;
        public static string updateExpirationDate;
        public static string renameMasterStrategyQuery;
        public static string updateSecFee;
        public static string updateTTUIPrefQuery;
        public static string updateTTUIPrefQueryHide;
        public static string updateThirdPartyFileFormat;
        public static string updateSMSharesOutValueQuery;
        public static string deleteSymbolFromSM;
        public static string updateRoundLot;
        public static string updateCompliancePermission;
        public static string updateMasterFundQuery;
        public static string updateAllocationPrefFromAdmin;
        public static string updateMarketDataProvider;
        public static string SetCommissionRuleQuery;
        public static string SetPreTradeCompliancePermissionQuery;
        public static string UpdateTransferTradeRules;
        public static readonly Dictionary<int, string> DefaultValues = new Dictionary<int, string>()
         {
        { 0, "0" },
        { 1, "1" },
        { 2, "2" },
        { 3, "3" },
        { 4, "4" },
        { 5, "5" }
         };
        /// <summary>
        /// Defining function for passing values.
        /// </summary>
        /// <param name="clearanceTime">Clearance Time as mention in Set Rollover schema</param>
        /// <param name="exchangeSymbolName">Exchange Symbol Name as mention in Set Rollover schema</param>
        /// <param name="underlyingName">underlyingName as mention in Set Rollover schema</param>
        /// <param name="assetName">Asset Name as mention in Set Rollover schema</param>
        public static void passingSetRolloverSchemaData(string clearanceTime, string exchangeSymbolName, string underlyingName, string assetName)
        {
            clearanceTimeUPdateSQLQuery = "UPDATE T_CompanyAUECClearanceTimeBlotter SET PermitRollover='1', T_CompanyAUECClearanceTimeBlotter.ClearanceTime='" + clearanceTime + "' FROM T_CompanyAUECClearanceTimeBlotter INNER JOIN T_CompanyAUEC ON T_CompanyAUECClearanceTimeBlotter.CompanyAUECID=T_CompanyAUEC.CompanyAUECID INNER JOIN T_AUEC ON T_CompanyAUEC.AUECID=T_AUEC.AUECID INNER JOIN T_Exchange ON T_AUEC.ExchangeID=T_Exchange.ExchangeID INNER JOIN T_UnderLying ON T_AUEC.UnderLyingID=T_UnderLying.UnderLyingID INNER JOIN T_Asset ON T_AUEC.AssetID=T_Asset.AssetID WHERE T_Exchange.DisplayName='" + exchangeSymbolName + "' AND T_UnderLying.UnderLyingName='" + underlyingName + "' AND T_Asset.AssetName='" + assetName + "' AND T_CompanyAUEC.CompanyID>0;";
        }

        public static void EnableBreakdownRealizedPNl(string IsBreakRealizedPNLSubAccount)
        {
            int prefValue = IsBreakRealizedPNLSubAccount.ToUpper().Equals("TRUE") ? 1 : 0;
            UpdateCashPreferences = "update T_CashPreferences set IsBreakRealizedPnlSubaccount=" + prefValue +"";
        }


        /// <summary>
        /// Set Trading Compliance Rule
        /// </summary>
        /// <param name="dt"></param>
        public static void SetTradingRulesQuery(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string overSellTradigRule = dr["OverSellTradingRule"].ToString();
                string overBuyTradingRule = dr["OverBuyTradingRule"].ToString();
                string unallocatedTradeAlert = dr["UnallocatedTradeAlert"].ToString();
                string fatFingerTradingRule = dr["FatFingerTradingRule"].ToString(); 
                string duplicateTradeAlert = dr["DuplicateTradeAlert"].ToString();
                string pendingNewOrderAlert = dr["PendingNewOrderAlert"].ToString();
                string fatFingerPercent = dr["FatFingerPercent"].ToString();
                if (fatFingerPercent.Equals(String.Empty))
                    fatFingerPercent = "0";
                string duplicateTradeAlertTime = dr["DuplicateTradeAlertTime"].ToString();
                if (duplicateTradeAlertTime.Equals(String.Empty))
                    duplicateTradeAlertTime = "0";
                string pendingNewOrderAlertTime = dr["PendingNewOrderAlertTime"].ToString();
                if (pendingNewOrderAlertTime.Equals(String.Empty))
                    pendingNewOrderAlertTime = "0";
                string fatFingerAccountOrMasterFund = dr["FatFingerAccountOrMasterFund"].ToString();
                int prefValue1 = overSellTradigRule.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue2 = overBuyTradingRule.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue3 = unallocatedTradeAlert.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue4 = fatFingerTradingRule.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue5 = duplicateTradeAlert.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue6 = pendingNewOrderAlert.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue7 = fatFingerAccountOrMasterFund.Equals("MasterFund") ? 1 : 0;
                if (dr.Table.Columns.Contains(TestDataConstants.Col_IsAbsoluteAmountOrDefinePercent))
                {
                    string isAbsoluteAmountOrDefinePercent = dr["IsAbsoluteAmountOrDefinePercent"].ToString();
                    int prefValue8 = isAbsoluteAmountOrDefinePercent.Equals("AbsoluteAmount") ? 1 : 0;
                    updateTradingRulesQuery = "Update T_TradingRulesPreferences set IsOverSellTradingRule = " + prefValue1 + ", IsOverBuyTradingRule = " + prefValue2 + ", IsUnallocatedTradeAlert = " + prefValue3 + ", IsFatFingerTradingRule = " + prefValue4 + ", IsDuplicateTradeAlert = " + prefValue5 + ", IsPendingNewTradeAlert = " + prefValue6 + ", DefineFatFingerValue =" + fatFingerPercent + ", DuplicateTradeAlertTime = " + duplicateTradeAlertTime + ", PendingNewOrderAlertTime = " + pendingNewOrderAlertTime + ", FatFingerAccountOrMasterFund = " + prefValue7 + ",IsAbsoluteAmountOrDefinePercent = " + prefValue8 + "where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTradingRulesQuery);

                    if (dr.Table.Columns.Contains(TestDataConstants.Col_SharesOutstandingPercent) && dr.Table.Columns.Contains(TestDataConstants.Col_SharesOutstandingAccountOrMF) && dr.Table.Columns.Contains(TestDataConstants.Col_IsInMarketIncluded) && dr.Table.Columns.Contains(TestDataConstants.Col_IsSharesOutstandingRule))
                
                        {
                          string isInMarketIncluded = dr["IsInMarketIncluded"].ToString();
                          string isSharesOutstandingRule = dr["IsSharesOutstandingRule"].ToString();
                          string sharesOutstandingPercent = dr["SharesOutstandingPercent"].ToString();
                          string sharesOutstandingAccountOrMF = dr["SharesOutstandingAccountOrMF"].ToString();
                          int prefValue9 = isInMarketIncluded.ToUpper().Equals("TRUE") ? 1 : 0;
                          int prefValue10 = isSharesOutstandingRule.ToUpper().Equals("TRUE") ? 1 : 0;
                          if(sharesOutstandingPercent.Equals(String.Empty))
                          {
                              sharesOutstandingPercent = "0";
                          }
                        int prefValue11 = sharesOutstandingAccountOrMF.Equals("Account") ? 0: sharesOutstandingAccountOrMF.Equals("MasterFund") ?  1 : 2;
                          updateTradingRulesQuery = "Update T_TradingRulesPreferences set IsInMarketIncluded = " + prefValue9 + ", IsSharesOutstandingRule = " + prefValue10 + ",SharesOutstandingPercent = " + sharesOutstandingPercent + ", SharesOutstandingAccountOrMF = " + prefValue11 + "where CompanyID>0";
                          SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTradingRulesQuery);          
                        }
                }
                          
                else
                {
                    updateTradingRulesQuery = "Update T_TradingRulesPreferences set IsOverSellTradingRule = " + prefValue1 + ", IsOverBuyTradingRule = " + prefValue2 + ", IsUnallocatedTradeAlert = " + prefValue3 + ", IsFatFingerTradingRule = " + prefValue4 + ", IsDuplicateTradeAlert = " + prefValue5 + ", IsPendingNewTradeAlert = " + prefValue6 + ", DefineFatFingerValue =" + fatFingerPercent + ", DuplicateTradeAlertTime = " + duplicateTradeAlertTime + ", PendingNewOrderAlertTime = " + pendingNewOrderAlertTime + ", FatFingerAccountOrMasterFund = " + prefValue7 + "where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTradingRulesQuery);
                }
               
            }
        }

        public static void EnableMasterFund(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string ShowMasterFundonTT = dr["IsShowMasterFundonTT"].ToString();
                string ShowmasterFundAsClient = dr["IsShowmasterFundAsClient"].ToString();
                int prefValue1 = ShowMasterFundonTT.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue2 = ShowmasterFundAsClient.ToUpper().Equals("TRUE") ? 1 : 0;
                updateMasterFundQuery = "update T_PranaKeyValuePreferences set PreferenceValue = " + prefValue1 + " where PreferenceKey = 'IsShowMasterFundonTT' or PreferenceKey =  'IsShowmasterFundAsClient'";
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateMasterFundQuery);
            }
        }

        public static void RemoveMasterUser()
        {
            UpdateTransferTradeRules = "Update T_TransferTradeRules set MasterUsersIDs = NULL";
            SqlUtilities.ExecuteQuery(SQLQueriesConstants.UpdateTransferTradeRules);
        }


        public static void SetTTGeneralPrefQuery(DataTable dt)
        {
            string defaultOrderType = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                string restrictedSecuritiesList = dr["RestrictedSecuritiesList"].ToString();
                string allowedSecuritiesList = dr["AllowedSecuritiesList"].ToString();
                string allowAllUserToCancelReplaceRemove = dr["AllowAllUserToCancelReplaceRemove"].ToString();
                string allowAllUserToTransferTrade = dr["AllowAllUserToTransferTrade"].ToString();
                string masterUserPermission = dr["MasterUserPermission"].ToString();
               
                if (dr.Table.Columns.Contains("DefaultOrderType"))
                     defaultOrderType = dr["DefaultOrderType"].ToString();

                if (dr.Table.Columns.Contains("LimitPriceRulesForStageOrdersOnly"))
                {
                    string limitPriceRulesForStageOrdersOnly = dr["LimitPriceRulesForStageOrdersOnly"].ToString();
                    string limitRulesForSubOrders = dr["LimitRulesForSubOrders"].ToString();
                    string limitRulesForOrdersExceptStagedAndSubOrders = dr["LimitRulesForOrdersExceptStagedAndSubOrders"].ToString();
                    string limitPriceRules = dr["LimitPriceRules"].ToString();

                    int prefValue6 = limitPriceRulesForStageOrdersOnly.ToUpper().Equals("TRUE") ? 1 : 0;
                    int prefValue7 = limitRulesForSubOrders.ToUpper().Equals("TRUE") ? 1 : 0;
                    int prefValue8 = limitRulesForOrdersExceptStagedAndSubOrders.ToUpper().Equals("TRUE") ? 1 : 0;
                    int prefValue9 = limitPriceRules.ToUpper().Equals("TRUE") ? 0 : 1;

                    updateTTGeneralPrefQuery = "Update T_TransferTradeRules set IsApplyLimitRulesForReplacingStagedOrders = " + prefValue6 + ", IsApplyLimitRulesForReplacingSubOrders = " + prefValue7 + " , IsApplyLimitRulesForReplacingOtherOrders = " + prefValue8 + ", IsAllowAllUserToChangeOrderType = " + prefValue9 + "";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTGeneralPrefQuery);
                }

                int prefValue1 = restrictedSecuritiesList.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue2 = allowedSecuritiesList.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue3 = allowAllUserToCancelReplaceRemove.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue4 = allowAllUserToTransferTrade.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue5 = defaultOrderType.ToUpper().Equals("") ? 1 : 0;
             
                updateTTGeneralPrefQuery = "Update T_TransferTradeRules set IsAllowRestrictedSecuritiesList = " + prefValue1 + ", IsAllowAllowedSecuritiesList = " + prefValue2 + " , IsAllowAllUserToCancelReplaceRemove = " + prefValue3 + ", IsAllowAllUserToTransferTrade = " + prefValue4 + ", IsDefaultOrderTypeLimitForMultiDay = " + prefValue5 + " where CompanyId>0";
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTGeneralPrefQuery);


                if (masterUserPermission == String.Empty)
                {
                    updateMasterUserQuery = "Update  T_TransferTradeRules set MasterUsersIDs = NULL where CompanyId>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateMasterUserQuery);
                }
                else
                {
                    string[] values = masterUserPermission.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        if (count == 0)
                        {
                            updateMasterUserQuery = "Declare @MasterId int"+ Environment.NewLine +
                                                    "Select @MasterId = UserID from T_CompanyUser where Login = \'" + values[i] + "\'" + Environment.NewLine +
                                                    "Update  T_TransferTradeRules set MasterUsersIDs = @MasterId where CompanyId >0";
                              count++;
                            SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateMasterUserQuery);
                          
                        }
                        else
                        {
                            updateMasterUserQuery =  "Declare @MasterId int" + Environment.NewLine +
                                                     "Select @MasterId = UserID from T_CompanyUser where Login = \'" + values[i] + "\'" + Environment.NewLine +
                                                     "Update T_TransferTradeRules set MasterUsersIDs =  MasterUsersIDs+', '+ Convert(varchar(Max), @MasterId ) where CompanyId >0";
                            SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateMasterUserQuery);
                        }
                    }
                  
                }
            }       
             

        }

        /// <summary>
        /// Enable module on user level
        /// </summary>     
        public static void EnableModule(string moduleName, string userName)
        {
            isEnableModule = "DECLARE @User_ID int set @User_ID=(select UserID from T_CompanyUser where ShortName='" + userName + "')" + Environment.NewLine +
                "insert into T_CompanyUserModule (CompanyModuleID,CompanyUserID,Read_WriteID) select CM.CompanyModuleID,@User_ID,CM.Read_WriteID from  T_Module M inner join T_CompanyModule CM  on M.ModuleID = CM.ModuleID  WHERE UPPER(ModuleName) = UPPER('" + moduleName + "')";
        }
        /// <summary>
        /// Disable module on user level
        /// </summary> 
        public static void DisableModule(string moduleName, string userName)
        {
            isEnableModule = "DECLARE @User_ID int set @User_ID=(select UserID from T_CompanyUser where ShortName='" + userName + "')" + Environment.NewLine +
                "delete  from T_CompanyUserModule where CompanyModuleID IN(select CM.CompanyModuleID from T_Module M inner join T_CompanyModule CM on M.ModuleID = CM.ModuleID  Where UPPER(ModuleName) = UPPER('" + moduleName + "')) AND CompanyUserID=@User_ID";
        }

        /// Import override on shortlocate
        /// </summary>
        public static void isImportOverrideOnShortLocate(string CheckImportOverrideOnShortLocate)
        {
            int prefValue = CheckImportOverrideOnShortLocate.ToUpper().Equals("TRUE") ? 1 : 0;
            importOverrideOnShortLocate = "update T_PranaKeyValuePreferences set PreferenceValue = " + prefValue + " where PreferenceKey = 'IsImportOverrideOnShortLocate'";
        }

        /// <summary>
        /// Set ShortLocate Strutures
        /// </summary>
        public static void IsShowmasterFundOnShortLocate(string IsShowmasterFundOnShortLocate)
        {
            int prefValue = IsShowmasterFundOnShortLocate.ToUpper().Equals("TRUE") ? 1 : 0;
            ShortLocateStructures = "update T_PranaKeyValuePreferences set PreferenceValue = " + prefValue + " where PreferenceKey = 'IsShowmasterFundOnShortLocate'";
        }

        public static void IsImportOverrideOnShortLocate(string IsImportOverrideOnShortLocate)
        {
            int prefValue = IsImportOverrideOnShortLocate.ToUpper().Equals("TRUE") ? 1 : 0;
            ShortLocateStructures = "update T_PranaKeyValuePreferences set PreferenceValue = " + prefValue + " where PreferenceKey = 'IsImportOverrideOnShortLocate'";
        }

        /// <summary>
        /// Dollar Amount Permission  in admin
        /// </summary>
        public static void SetDolloarAmountPermissionQuery(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string permissionTT = dr[TestDataConstants.Col_TT].ToString();
                string permissioPTT = dr[TestDataConstants.Col_PTT].ToString();
                int prefValue1 = permissionTT.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue2 = permissioPTT.ToUpper().Equals("TRUE") ? 1 : 0;
 
                updateDollarAmountPermissionQuery = "Update T_DollarAmountPermission set TT = "+ prefValue1 + ", PTT = "+ prefValue2 +"";
            }
         }

        public static void SetRoundLot(DataTable dt)
        {
            foreach(DataRow dr in dt.Rows)
            {
                string symbolUpdate = dr[TestDataConstants.COL_SYM].ToString();
                double roundLot = Convert.ToDouble(dr[TestDataConstants.COL_ROUNDLOTSM]);

                updateRoundLot = "Update T_SMSymbolLookUpTable set RoundLot = "+ roundLot +" where TickerSymbol = '"+ symbolUpdate +"'";
                SqlUtilities.ExecuteQuerySM(updateRoundLot);
            }
        }

        public static void UpdateExpirationDate(DateTime expirationDate, string symbol, string tableAsPerAsset)
        {
            updateExpirationDate = "Update " + tableAsPerAsset + " set ExpirationDate='" + expirationDate + "' where Symbol_PK =(select Symbol_PK from t_smsymbollookuptable where TickerSymbol='" + symbol + "')";
        }
        public static void RenameMasterStrategy(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string oldMasterStrategy = dr[TestDataConstants.COL_Old_MasterStrategy].ToString();
                string newMasterStrategy = dr[TestDataConstants.COL_New_MasterStrategy].ToString();

                renameMasterStrategyQuery = "Update T_CompanyMasterStrategy Set MasterStrategyName = '" + newMasterStrategy + "' where MasterStrategyName = '" + oldMasterStrategy + "' "; 
            }
  
        }

        public static void UpdateFee(decimal fee, FeeType feetype, string displayName)
        {
            //updateSecFee = "update T_OtherFeeRules set LongFeeRate ='"+fee+"', ShortFeeRate ='"+fee+ "' where FeeTypeID='"+(int)feetype+"'";
            updateSecFee = "update T_OtherFeeRules set LongFeeRate ='" + fee + "', ShortFeeRate ='" + fee + "' where FeeTypeID='" + (int)feetype + "'and AUECID = (select AUECID from T_AUEC where DisplayName='"+displayName+"' )";
        }

        public static void SetTTUIPrefQuery(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string broker = dr["Broker"].ToString();
                string venue = String.Empty;
                string tif = TestDataConstants.COL_DEFAULTIF;
                string orderType = TestDataConstants.COL_DEFAULORDERTYPE;
                string trader = TestDataConstants.COL_DEFAULTTRADER;
                string HideTradingQuantity = String.Empty;
                string SecurityValidationForEquityOption = String.Empty;
                string account = TestDataConstants.COL_DEFAULT_ACCOUNT;
                if (dr.Table.Columns.Contains("SecurityValidationForEquityOption"))
                {
                    SecurityValidationForEquityOption = dr["SecurityValidationForEquityOption"].ToString();
                    if (!String.IsNullOrEmpty(SecurityValidationForEquityOption)) {
                        if (SecurityValidationForEquityOption.ToUpper().Equals("TRUE"))
                        {
                            updateTTUIPrefQueryHide = "update T_PranaKeyValuePreferences set PreferenceValue = 1 where PreferenceKey = 'IsEquityOptionManualValidation'";
                        }
                        else {
                            updateTTUIPrefQueryHide = "update T_PranaKeyValuePreferences set PreferenceValue = 0 where PreferenceKey = 'IsEquityOptionManualValidation'";
                        }
                        SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQueryHide);
                        updateTTUIPrefQueryHide = string.Empty;
                    }

                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_HIDETARGETQUANTITY))
                {
                    HideTradingQuantity = dr["ShowHideTargetQuantity"].ToString();
                }
                int prefValueHide = HideTradingQuantity.ToUpper().Equals("TRUE") ? 1 : 0;

                updateTTUIPrefQueryHide = "Update T_CompanyTTGeneralPreferences set IsShowTargetQTY = " + prefValueHide + " where CompanyId>0";
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQueryHide);

                if (dr.Table.Columns.Contains(TestDataConstants.COL_VENUE))
                {
                    venue = dr["Venue"].ToString();
                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_TIF) && !String.IsNullOrEmpty(dr[TestDataConstants.COL_TIF].ToString()))
                {
                    tif = dr["TIF"].ToString();
                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_TRADER) && !String.IsNullOrEmpty(dr[TestDataConstants.COL_TRADER].ToString()))
                {
                    trader = dr["Trader"].ToString();
                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_ORDER_TYPE) && !String.IsNullOrEmpty(dr[TestDataConstants.COL_ORDER_TYPE].ToString()))
                {
                    orderType = dr[TestDataConstants.COL_ORDER_TYPE].ToString();
                }

                if (dr.Table.Columns.Contains(TestDataConstants.COL_ACCOUNT))
                {
                    account = dr["Account"].ToString();
                }
                if (account != String.Empty && account != "NULL")
                {

                    updateTTUIPrefQuery = "Declare @accountid int" + Environment.NewLine +
                                          "Set @accountid = (Select CompanyFundID from T_CompanyFunds where FundName = \'" + account + "\')" + Environment.NewLine +
                                          "Update T_CompanyTTGeneralPreferences Set AccountID = @accountid where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                else if (account.ToUpper() == "NULL")
                {
                    updateTTUIPrefQuery = "Update T_CompanyTTGeneralPreferences Set AccountID = NULL where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }

                if (orderType != String.Empty && orderType != "NULL")
                {

                    updateTTUIPrefQuery = "Declare @OrderTypeId int" + Environment.NewLine +
                                          "Set @OrderTypeId = (Select OrderTypesID from T_OrderType where OrderTypes = \'" + orderType + "\')" + Environment.NewLine +
                                          "Update T_CompanyTTGeneralPreferences Set OrderTypeID = @OrderTypeId where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                else
                {
                    updateTTUIPrefQuery = "Update T_CompanyTTGeneralPreferences Set OrderTypeID = NULL where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                if (broker != String.Empty && broker != "NULL")
                {
                    updateTTUIPrefQuery = "Declare @CounterPartyId int" + Environment.NewLine +
                                          "Set @CounterPartyId= (Select CounterPartyId from T_CounterParty where ShortName = \'" + broker + "\')" + Environment.NewLine +
                                          "Update T_CompanyTTGeneralPreferences Set CounterPartyID = @CounterPartyId where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                else
                {
                    updateTTUIPrefQuery = "Update T_CompanyTTGeneralPreferences Set CounterPartyID = NULL where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                // update venue for not null exchange id
                if (venue != String.Empty && venue != "NULL")
                {
                    updateTTUIPrefQuery = "Declare @VenueId int" + Environment.NewLine +
                                          "Set @VenueId = (Select VenueID from T_Venue where VenueName = \'" + venue + "\' AND ExchangeID IS NOT NULL)" + Environment.NewLine +
                                          "Update T_CompanyTTGeneralPreferences Set VenueID = @VenueId where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                else
                {
                    updateTTUIPrefQuery = "Update T_CompanyTTGeneralPreferences Set VenueID = NULL where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                if (tif != String.Empty && tif != "NULL")
                {
                    updateTTUIPrefQuery = "Declare @TimeInForceID int" + Environment.NewLine +
                                          "Set @TimeInForceID= (Select TimeInForceID from T_TimeInForce where TimeInForce = \'" + tif + "\')" + Environment.NewLine +
                                          "Update T_CompanyTTGeneralPreferences Set TimeInForceID = @TimeInForceID where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                else
                {
                    updateTTUIPrefQuery = "Update T_CompanyTTGeneralPreferences Set TimeInForceID = NULL where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                if (trader != String.Empty && trader != "NULL")
                {
                    updateTTUIPrefQuery = "Declare @TradingAccountID int" + Environment.NewLine +
                                          "Set @TradingAccountID = (Select CompanyTradingAccountsID from T_CompanyTradingAccounts where TradingShortName = \'" + trader + "\')" + Environment.NewLine +
                                          "Update T_CompanyTTGeneralPreferences Set TradingAccountID = @TradingAccountID where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                else
                {
                    updateTTUIPrefQuery = "Update T_CompanyTTGeneralPreferences Set TimeInForceID = NULL where CompanyID>0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
                if (!string.IsNullOrEmpty(HideTradingQuantity))
                {
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateTTUIPrefQuery);
                }
               
               
            }
                
            
        }
        /// <summary>
        /// Set Compliance Alert Preferences
        /// </summary>
        /// <param name="dt"></param>
        public static void SetComplianceAlert(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string ImportExportPath = dr["ImportExportPath"].ToString();
                string PrePostCrossImport = dr["PrePostCrossImport"].ToString();
                string InMarket = dr["InMarket"].ToString();
                string InStage = dr["InStage"].ToString();
                string PostInMarket = dr["PostInMarket"].ToString();
                string PostInStage = dr["PostInStage"].ToString();
                string BlockTradeOnComplianceFaliure = dr["BlockTradeOnComplianceFaliure"].ToString();
                string StageValueFromField = dr["StageValueFromField"].ToString();
                string StageValueFromFieldString = dr["StageValueFromFieldString"].ToString();
                int prefValue1 = PrePostCrossImport.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue2 = InMarket.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue3 = InStage.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue4 = PostInMarket.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue5 = PostInStage.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue6 = BlockTradeOnComplianceFaliure.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue7 = StageValueFromField.ToUpper().Equals("TRUE") ? 1 : 0;
                updateComplianceAlert = "Update T_CA_CompliancePreferences set ImportExportPath = '" + ImportExportPath + "', PrePostCrossImport = " + prefValue1 + ", InMarket = " + prefValue2 + ", InStage = " + prefValue3 + ", PostInMarket = " + prefValue4 + ", PostInStage = " + prefValue5 + ", BlockTradeOnComplianceFaliure =" + prefValue6 + ", StageValueFromField = " + prefValue7 + ", StageValueFromFieldString = '" + StageValueFromFieldString + "' where CompanyID>0";

            }
        }

        /// <summary>
        /// Set Alert Type Permission in compliance permission
        /// </summary>  

        public static void SetCompliancePermission(DataTable dt)
        {
            IDictionary<string, int> AlertRules = new Dictionary<string, int>();
            string User_ID = "17";
            if (dt.Columns.Contains(TestDataConstants.USERID) && dt.Rows[0][TestDataConstants.USERID].ToString() != String.Empty)
            {
                User_ID = dt.Rows[0][TestDataConstants.USERID].ToString();
            }
            foreach (DataRow dr in dt.Rows)
            {
                string Rule_Name = dr[TestDataConstants.COL_RULENAME].ToString();
                string Alert_P_T = dr[TestDataConstants.COL_ALERT_PERMISSION_TYPE].ToString();
                
            
             
                    if(Alert_P_T.ToUpper() == "SOFT"  )
                    {
                        AlertRules.Add(Rule_Name,1); //also bydefault value is 1
                    }
                    if (Alert_P_T.ToUpper() == "REQUIRES APPROVAL")
                    {
                         AlertRules.Add(Rule_Name,2);
                    }
                     if(Alert_P_T.ToUpper() == "HARD" )
                    {
                         AlertRules.Add(Rule_Name,3);
                    }
                    
                     if(Alert_P_T.ToUpper() == "SOFT WITH NOTES" )
                    {
                        AlertRules.Add(Rule_Name,4);
                    }
                }



            foreach (var x in AlertRules)
            {
                updateCompliancePermission = "Update RUP  set RUP.ruleOverrideType = " + x.Value + " from T_CA_RuleUserPermissions RUP inner join T_CA_RulesUserDefined RUD on RUP.RuleID = RUD.RuleID where RuleName = '" + x.Key + "' AND UserId = '" + User_ID + "' ";
                SqlUtilities.ExecuteQuery(updateCompliancePermission);
            }
         }
        

        /// Set ThirdParty File Format
        /// </summary>
        public static void UpdateThirdPartyFileFormatQuery(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string exportOnly = dr["ExportOnly"].ToString();
                int prefValue1 = exportOnly.ToUpper().Equals("TRUE") ? 1 : 0;
                updateThirdPartyFileFormat = "Update T_ThirdPartyFileFormat set ExportOnly = " + prefValue1 + " where FileFormatId=43";
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateThirdPartyFileFormat);
            }
        }

        public static void UpdatingSharesOutValueQuery(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string symbol = dr[TestDataConstants.COL_SYMBOL].ToString();
                updateSMSharesOutValueQuery = "Update T_SMSymbolLookUpTable set SharesOutstanding = 0 where TickerSymbol = '" + symbol + "'";
                SqlUtilities.ExecuteQuerySM(SQLQueriesConstants.updateSMSharesOutValueQuery);
            }

        }

        public static void  DeleteSymbol(String symbol)
        {
            deleteSymbolFromSM = "DECLARE @Symbol_PK bigint" + Environment.NewLine +
            "set @Symbol_PK= (select Symbol_PK From T_SMSymbolLookUpTable Where TickerSymbol ='" + symbol + "')" + Environment.NewLine +
            "DELETE From T_SMCorporateActions where Symbol= '" + symbol + "'" + Environment.NewLine +
            "DELETE From T_SMEquityNonHistoryData where Symbol_PK= @Symbol_PK" + Environment.NewLine +
            "DELETE From T_SMFixedIncomeData where Symbol_PK= @Symbol_PK" + Environment.NewLine +
            "DELETE From T_SMFutureData where Symbol_PK= @Symbol_PK" + Environment.NewLine +
            "DELETE From T_SMFxData where Symbol_PK = @Symbol_PK" + Environment.NewLine +
            "DELETE From T_SMFXForwardData where Symbol_PK= @Symbol_PK" + Environment.NewLine +
            "DELETE From T_SMOptionData where Symbol_PK = @Symbol_PK" + Environment.NewLine +
            "DELETE From T_SMReuters where Symbol_PK = @Symbol_PK" + Environment.NewLine +
            "DELETE From T_UDA_DynamicUDAData where Symbol_PK = @Symbol_PK" +Environment.NewLine +
            "DELETE From T_SMSymbolLookUpTable where Symbol_PK = @Symbol_PK";
            
        }


        public static void SetAllocationPref(DataTable dt)
        {
         
            
            foreach (DataRow dr in dt.Rows)
            {
                string ColCompanyID = string.IsNullOrEmpty(dr["CompanyId"].ToString()) ? DefaultValues[5] : dr["CompanyId"].ToString();
                string ColAutoGroup = string.IsNullOrEmpty(dr["AutoGroup"].ToString()) ? DefaultValues[0] : dr["AutoGroup"].ToString();
                string ColTradeAttribute1 = string.IsNullOrEmpty(dr["TradeAttribute1"].ToString()) ? DefaultValues[0] : dr["TradeAttribute1"].ToString();
                string ColTradeAttribute2 = string.IsNullOrEmpty(dr["TradeAttribute2"].ToString()) ? DefaultValues[0] : dr["TradeAttribute2"].ToString();
                string ColTradeAttribute3 = string.IsNullOrEmpty(dr["TradeAttribute3"].ToString()) ? DefaultValues[0] : dr["TradeAttribute3"].ToString();
                string ColTradeAttribute4 = string.IsNullOrEmpty(dr["TradeAttribute4"].ToString()) ? DefaultValues[0] : dr["TradeAttribute4"].ToString();
                string ColTradeAttribute5 = string.IsNullOrEmpty(dr["TradeAttribute5"].ToString()) ? DefaultValues[0] : dr["TradeAttribute5"].ToString();
                string ColTradeAttribute6 = string.IsNullOrEmpty(dr["TradeAttribute6"].ToString()) ? DefaultValues[0] : dr["TradeAttribute6"].ToString();
                string ColBroker = string.IsNullOrEmpty(dr["Broker"].ToString()) ? DefaultValues[0] : dr["Broker"].ToString();
                string ColVenue = string.IsNullOrEmpty(dr["Venue"].ToString()) ? DefaultValues[0] : dr["Venue"].ToString();
                string ColTradingAC = string.IsNullOrEmpty(dr["TradingAC"].ToString()) ? DefaultValues[1] : dr["TradingAC"].ToString();
                string ColTradeDate = string.IsNullOrEmpty(dr["TradeDate"].ToString()) ? DefaultValues[1] : dr["TradeDate"].ToString();
                string ColProcessDate = string.IsNullOrEmpty(dr["ProcessDate"].ToString()) ? DefaultValues[0] : dr["ProcessDate"].ToString();

                updateAllocationPrefFromAdmin = "Update T_AutoGroupingPref SET AutoGroup = "+ ColAutoGroup +" ,TradeAttribute1 = "+ ColTradeAttribute1 + " ,TradeAttribute2 = " +ColTradeAttribute2 +",TradeAttribute3 = "+ ColTradeAttribute3 +" ,TradeAttribute4 = " + ColTradeAttribute4 + " ,TradeAttribute5 = " +ColTradeAttribute5 + " ,TradeAttribute6 = " +ColTradeAttribute6 + " ,Broker = " + ColBroker + " ,Venue = " + ColVenue +" ,TradingAC = " +ColTradingAC + " ,TradeDate = " +ColTradeDate +" ,ProcessDate =" +ColProcessDate +" WHERE CompanyId = "+ ColCompanyID ;
                SqlUtilities.ExecuteQuery(updateAllocationPrefFromAdmin);
            }

            //updating autogrouping for all the accounts by defaul
            SqlUtilities.ExecuteQuery("Update T_AutoGroupingFunds SET AutoGroup = "+ DefaultValues[1]);
 
        }


        public static void UpdateMarketType(int val, int B_data)
        {

            updateMarketDataProvider = "Update T_CompanyMarketDataProvider SET MarketDataProvider = '" + val + "', IsMarketDataBlocked = " + B_data + " where CompanyID>0";
            SqlUtilities.ExecuteQuery(updateMarketDataProvider);

        }

        public static void SetCommissionRule(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string CommissionCalculation = dr[TestDataConstants.COL_COMMISSIONCALCULATION].ToString();
                int CommissionCalculationValue = 1;
                if (CommissionCalculation.ToUpper() == "PREALLOCATION")
                {
                    CommissionCalculationValue = 0;   
                }
                SetCommissionRuleQuery = "Update T_CommissionCalculationTime SET IsPostAllocatedCalculation = " + CommissionCalculationValue + "";
                SqlUtilities.ExecuteQuery(SetCommissionRuleQuery);
            }
        }
        public static void SetPreTradeCompliancePermission(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string Trader = dr["Trading"].ToString();
                string ManualTrading = dr["Manual Trading"].ToString();
                string Staging = dr["Staging"].ToString();
                if (Trader.ToUpper() == "FALSE")
                {
                    SetPreTradeCompliancePermissionQuery = "Update T_CA_OtherCompliancePermission SET Trading = 0";
                    SqlUtilities.ExecuteQuery(SetPreTradeCompliancePermissionQuery);
                }
                if (ManualTrading.ToUpper() == "FALSE")
                {
                    SetPreTradeCompliancePermissionQuery = "Update T_CA_OtherCompliancePermission SET IsApplyToManual = 0";
                    SqlUtilities.ExecuteQuery(SetPreTradeCompliancePermissionQuery);
                }
                if (Staging.ToUpper() == "FALSE")
                {
                    SetPreTradeCompliancePermissionQuery = "Update T_CA_OtherCompliancePermission SET Staging = 0";
                    SqlUtilities.ExecuteQuery(SetPreTradeCompliancePermissionQuery);
                }

            }
        }
        public static void SetAuecExecutingBrokerQuery(DataTable dt)
        {
            string DeleteQuery = "Delete from T_FundWiseExecutingBroker";
            SqlUtilities.ExecuteQuery(DeleteQuery);
            foreach (DataRow dr in dt.Rows)
            {
                string Account = dr["Account"].ToString();
                string Broker = dr["Broker"].ToString();
                string SetAuecExecutingBrokerQuery = string.Empty;
                SetAuecExecutingBrokerQuery = "Declare @AccountID int " + Environment.NewLine +
                                              "Declare @BrokerID int " + Environment.NewLine +
                                              "Set @AccountID = (SELECT CompanyFundID FROM T_CompanyFunds WHERE FundName = \'" + Account + "\') " + Environment.NewLine +
                                              "Set @BrokerID = (SELECT CounterPartyID FROM T_Counterparty WHERE ShortName = \'" + Broker + "\') " + Environment.NewLine +
                                              "Insert into T_FundWiseExecutingBroker (Fundid,Brokerid,Companyid)values (@AccountID,@BrokerID,'5')";
                SqlUtilities.ExecuteQuery(SetAuecExecutingBrokerQuery);

            }
        }

        public static void IsAlogoBroker(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string broker = dr["Broker"].ToString();
                int isAlgo = dr["IsAlgoBroker"].ToString().ToUpper().Equals("TRUE") ? 1:0;
                string query = "update T_CounterParty set IsAlgoBroker = '" + isAlgo + "' where ShortName = '" + broker + "'";
                SqlUtilities.ExecuteQuery(query);
            }
        
        }

        public static void ClearRTPNLtabs()
        {
            string query = "delete from T_Samsara_CompanyUserLayouts";
            SqlUtilities.ExecuteQuery(query);
            query = "delete from T_Samsara_OpenfinWorkspaceInfo";
            SqlUtilities.ExecuteQuery(query);
            query = "delete from T_Samsara_OpenfinPageInfo";
            SqlUtilities.ExecuteQuery(query);
            query = "delete from T_RTPNL_UserWidgetConfigDetails";
            SqlUtilities.ExecuteQuery(query);

        }

        public static void EnableAutoGroupingAllocationQuery(DataTable dt)
        {
            
            foreach (DataRow dr in dt.Rows)
            {
                string updateEnableAutoGroupingAllocationQuery;
                string updateEnableAutoGroupingAllocationFundsQuery;
                string permissionEnable = dr["Enable"].ToString();
                string permissionEnableFunds = dr["EnableAllFunds"].ToString();
                int prefValue1 = permissionEnable.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue2 = permissionEnableFunds.ToUpper().Equals("TRUE") ? 1 : 0;

                updateEnableAutoGroupingAllocationQuery = "Update T_AutoGroupingPref set AutoGroup = " + prefValue1 + " where CompanyId = 5";
                updateEnableAutoGroupingAllocationFundsQuery = "Update T_AutoGroupingFunds set AutoGroup = " + prefValue2 + "";
                SqlUtilities.ExecuteQuery(updateEnableAutoGroupingAllocationQuery);
                SqlUtilities.ExecuteQuery(updateEnableAutoGroupingAllocationFundsQuery);
            }
        }

        public static void DeselectAccount(String[] Accounts, String UserID)
        {
            foreach (string Account in Accounts)
            {
                //string query = " delete from T_CompanyUserFunds where CompanyFundID = " + Account + "and CompanyUserID = " + UserID + "";
                string query = "Declare @FundId int" + Environment.NewLine +
                                "SELECT @FundId = CompanyFundID FROM T_CompanyFunds WHERE FundName =\'" + Account + "\'" + Environment.NewLine +
                                "delete from T_CompanyUserFunds where CompanyFundID = @FundId and CompanyUserID = \'" + UserID + "\'";
                SqlUtilities.ExecuteQuery(query);
            }

        }

        public static void AccountPermission(String[] Accounts, String UserID, String Permission)
        {
            if (Permission.ToUpper() == "ALLOCATE")
            {
                foreach (string Account in Accounts)
                {                    
                    string query = @"DECLARE @FundName NVARCHAR(100), @FundId INT, @CompUserFundID INT;
                                    
                                    SET @FundName = @Account;
                                    
                                    SELECT @FundId = CompanyFundID 
                                    FROM T_CompanyFunds 
                                    WHERE FundName = @FundName;
                                    
                                    SELECT TOP 1 @CompUserFundID = CompanyUserFundID 
                                    FROM T_CompanyUserFunds 
                                    ORDER BY CompanyUserFundID DESC;
                                    
                                    SET IDENTITY_INSERT T_CompanyUserFunds ON;

                                    INSERT INTO T_CompanyUserFunds (CompanyUserFundID, CompanyFundID, CompanyUserID)
                                    VALUES (@CompUserFundID + 1, @FundId, @UserID);

                                    SET IDENTITY_INSERT T_CompanyUserFunds OFF;
                                    ";
                    SqlUtilities.ExecuteQueryParameter(query, Account, UserID);

                }               
            }
            else if (Permission.ToUpper() == "UNALLOCATE")
            {
                foreach (string Account in Accounts)
                {
                    string query =  "Declare @FundId int" + Environment.NewLine +
                                    "SELECT @FundId = CompanyFundID FROM T_CompanyFunds WHERE FundName =\'" + Account + "\'" + Environment.NewLine +
                                    "delete from T_CompanyUserFunds where CompanyFundID = @FundId and CompanyUserID = \'" + UserID + "\'";
                    SqlUtilities.ExecuteQuery(query);
                }

            
            }

        }
        public static void DisableBCPermission(string userID, string companyWise, DataTable companyIDUserIDList, string statetoSet,string isBasketComplianceEnabledCompany)
        {
            try
            {
                if (!string.IsNullOrEmpty(isBasketComplianceEnabledCompany))
                {
                    string query = "UPDATE T_CA_CompliancePreferences " +
                                "SET isBasketComplianceEnabledCompany = " + isBasketComplianceEnabledCompany + " " +
                                "WHERE CompanyId = '" + companyWise + "'";
                    SqlUtilities.ExecuteQuery(query);
                }
                else
                {
                    if (string.IsNullOrEmpty(statetoSet))
                    {
                        statetoSet = "0";
                    }

                    string query = "";

                    if (!string.IsNullOrEmpty(userID))
                    {
                        query = "UPDATE T_CA_OtherCompliancePermission " +
                                "SET EnableBasketComplianceCheck = " + statetoSet + " " +
                                "WHERE UserId = '" + userID + "' AND CompanyId = '" + companyWise + "'";
                    }
                    else
                    {
                        query = "UPDATE T_CA_OtherCompliancePermission " +
                                "SET EnableBasketComplianceCheck = " + statetoSet + " " +
                                "WHERE CompanyId = '" + companyWise + "'";
                    }

                    SqlUtilities.ExecuteQuery(query);

                    Console.WriteLine("Successfully updated Basket Compliance Check for UserID: " + userID + ", CompanyID: " + companyWise);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while updating BC permission for UserID: " + userID + ", CompanyID: " + companyWise + " - " + ex.Message);
                throw;
            }
        }

    }

}
    