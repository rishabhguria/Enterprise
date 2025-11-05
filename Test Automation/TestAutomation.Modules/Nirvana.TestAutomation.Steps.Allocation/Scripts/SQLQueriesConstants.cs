using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Steps.Allocation.Scripts
{
    /// <summary>
    /// SQLQueriesConstants
    /// </summary>
    public static class SQLQueriesConstants
    {
        /// <summary>
        /// The delete cv account query
        /// </summary>
        public static string deleteCVAccountQuery;
        /// <summary>
        /// The delete cv auec query
        /// </summary>
        public static string deleteCVAuecQuery;
        
        public static void deleteCVAccount(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string fundName = dr["FundName"].ToString();
                string auecName = dr["AUECName"].ToString();
                string counterPartyVenueName = dr["CounterPartyName"].ToString();
                deleteCVAccountQuery = "Declare @Fundid int,@AUECid int,@CPVid int" + Environment.NewLine +
                                       "Select @Fundid = CompanyFundID from T_CompanyFunds where FundName= \'" + fundName + "\'" + Environment.NewLine +
                                       "Select @AUECid = AUECID from T_AUEC where DisplayName= \'" + auecName + "\' " + Environment.NewLine +
                                       "Select @CPVid = CounterPartyVenueID from T_CounterPartyVenue where DisplayName= \'" + counterPartyVenueName + "\'" + Environment.NewLine +
                                       "Delete from T_CVACCOUNT where (AccountID) NOT IN (@Fundid)" + Environment.NewLine +
                                       "Delete from T_CVACCOUNT where (CounterPartyVenueID) NOT IN (@CPVid)" + Environment.NewLine +
                                       "Delete from T_CVAccount where AccountID = @Fundid and CounterPartyVenueID = @CPVid";
                SqlUtilities.ExecuteQuery(deleteCVAccountQuery);
            }
        }

        public static void deleteCVAUEC(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string fundName = dr["FundName"].ToString();
                string auecName = dr["AUECName"].ToString();
                string counterPartyVenueName = dr["CounterPartyName"].ToString();
                deleteCVAuecQuery = "Declare @Fundid int,@AUECid int,@CPVid int" + Environment.NewLine +
                                       "Select @Fundid = CompanyFundID from T_CompanyFunds where FundName= \'" + fundName + "\'" + Environment.NewLine +
                                       "Select @AUECid = AUECID from T_AUEC where DisplayName= \'" + auecName + "\' " + Environment.NewLine +
                                       "Select @CPVid = CounterPartyVenueID from T_CounterPartyVenue where DisplayName= \'" + counterPartyVenueName + "\'" + Environment.NewLine +
                                       "Delete from T_CVAUEC where AUECID = @AUECid and CounterPartyVenueID = @CPVid";
                SqlUtilities.ExecuteQuery(deleteCVAuecQuery);
            }
        }
        /// <summary>
        /// The insert broker account auec values query
        /// </summary>
        /// 
        public static string insertBrokerAccountAuecValuesQuery;
        /// <summary>
        /// The update check side value query
        /// </summary>
        public static string updateCheckSideValueQuery, updateAssetCheckSideValueQuery, updateAccountCheckSideValueQuery, updateCounterPartyCheckSideValueQuery, updateAllocationPreferencesQuery;

        /// <summary>
        /// AutoGrouping Preferences CheckBox
        /// </summary>
        public static string updateAutoGroupPreferencesValueQuery;
        /// <summary>
        /// Set AutoGrouping Account 
        /// </summary>
        public static string updateAutoGroupingAccountValueQuery, updateAutoGroupingAccountUnallocatedValueQuery;

        /// <summary>
        /// Set AvgPriceRounding CheckBox
        /// </summary>
        public static string updateAvgPriceRoundingValueQuery, updateRoundingDigitQuery;

        /// <summary>
        /// Sets the check side value.
        /// </summary>
        /// <param name="checkSideValue">The check side value.</param>
        public static void setCheckSideValue(string checkSideValue)
        {
            updateCheckSideValueQuery = "UPDATE T_AL_AllocationDefaultRule SET T_AL_AllocationDefaultRule.CheckSidePreference='{\"DisableCheckSidePref\":{},\"DoCheckSideSystem\":" + checkSideValue.ToLower() + "}'";
        }

        /// <summary>
        /// Sets the check master fund as client.
        /// </summary>
        /// <param name="CheckMasterFundAsClient">The check master fund as client.</param>
        public static void setCheckMasterFundAsClient(string CheckMasterFundAsClient)
        {
            int prefValue = CheckMasterFundAsClient.ToUpper().Equals("TRUE") ? 1 : 0;
            updateClientCheckBoxValue = "update T_PranaKeyValuePreferences set PreferenceValue = " + prefValue + " where PreferenceKey = 'IsShowmasterFundAsClient'";
        }

        /// <summary>
        /// Inserts the broker account auec values.
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static void insertBrokerAccountAuecValues(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string fundName = dr["FundName"].ToString();
                string auecName = dr["AUECName"].ToString();
                string counterPartyVenueName = dr["CounterPartyName"].ToString();

                insertBrokerAccountAuecValuesQuery = "Declare @Fundid int,@AUECid int,@CPVid int" + Environment.NewLine +
                                                     "Select @Fundid = CompanyFundID from T_CompanyFunds where FundName= \'"+ fundName +"\'" + Environment.NewLine +
                                                     "Select @AUECid = AUECID from T_AUEC where DisplayName= \'"+ auecName +"\' " + Environment.NewLine +
                                                     "Select @CPVid = CounterPartyVenueID from T_CounterPartyVenue where DisplayName= \'"+ counterPartyVenueName +"\'" + Environment.NewLine +
                                                     "Insert into T_CVACCOUNT (AccountID,CounterPartyVenueID) values(@Fundid,@CPVid)" + Environment.NewLine +
                                                     "Insert into T_CVAUEC(AUECID,CounterPartyVenueID) values(@AUECid,@CPVid)";
                SqlUtilities.ExecuteQuery(insertBrokerAccountAuecValuesQuery);
            }                                        
        }
        /// <summary>
        /// Sets the asset check side value.
        /// </summary>
        /// <param name="assetID">The asset identifier.</param>
        public static void setAssetCheckSideValue(string assetID)
        {
            updateAssetCheckSideValueQuery = "UPDATE T_AL_AllocationDefaultRule SET T_AL_AllocationDefaultRule.CheckSidePreference='{\"DisableCheckSidePref\":{\"Asset\":[" + assetID + "]},\"DoCheckSideSystem\":true}'";
        }

        /// <summary>
        /// Sets the account check side value.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        public static void setAccountCheckSideValue(string accountID)
        {
            updateAccountCheckSideValueQuery = "UPDATE T_AL_AllocationDefaultRule SET T_AL_AllocationDefaultRule.CheckSidePreference='{\"DisableCheckSidePref\":{\"Account\":[" + accountID + "]},\"DoCheckSideSystem\":true}'";
        }

        /// <summary>
        /// Sets the counter party check side value.
        /// </summary>
        /// <param name="counterPartyID">The counter party identifier.</param>
        public static void setCounterPartyCheckSideValue(string counterPartyID)
        {
            updateCounterPartyCheckSideValueQuery = "UPDATE T_AL_AllocationDefaultRule SET T_AL_AllocationDefaultRule.CheckSidePreference='{\"DisableCheckSidePref\":{\"CounterParty\":[" + counterPartyID + "]},\"DoCheckSideSystem\":true}'";
        }

        /// <summary>
        /// Sets the allocation preferences.
        /// </summary>
        /// <param name="assetID">The asset identifier.</param>
        /// <param name="accountID">The account identifier.</param>
        /// <param name="counterPartyID">The counter party identifier.</param>
        /// <param name="checkSideValue">The check side value.</param>
        public static void setAllocationPreferences(string assetID, string accountID, string counterPartyID, string checkSideValue)
        {
            updateAllocationPreferencesQuery = "UPDATE T_AL_AllocationDefaultRule SET T_AL_AllocationDefaultRule.CheckSidePreference='{\"DisableCheckSidePref\":{";
            if (checkSideValue != "")
            {
                if (Convert.ToBoolean(checkSideValue))
                {
                    if (assetID != "")
                    {
                        updateAllocationPreferencesQuery += "\"Asset\":[" + assetID + "],";
                    }
                    if (accountID != "")
                    {
                        updateAllocationPreferencesQuery += "\"Account\":[" + accountID + "],";
                    }
                    if (counterPartyID != "")
                    {
                        updateAllocationPreferencesQuery += "\"CounterParty\":[" + counterPartyID + "]";
                    }
                    updateAllocationPreferencesQuery += "},\"DoCheckSideSystem\":true}'";
                }
                else
                {
                    updateAllocationPreferencesQuery += "},\"DoCheckSideSystem\":false}'";
                }
            }
            else
            {
                updateAllocationPreferencesQuery += "},\"DoCheckSideSystem\":false}'";
            }
        }
        /// <summary>
        /// The update CheckBox value query
        /// </summary>
        public static string updateCheckBoxValueQuery;
        /// <summary>
        /// The update client CheckBox value
        /// </summary>
        public static string updateClientCheckBoxValue;

        /// <summary>
        /// Sets the check button master fundon tt value.
        /// </summary>
        /// <param name="CheckButtonMasterFundonTTValue">The check button master fundon tt value.</param>
        public static void setCheckButtonMasterFundonTTValue(string CheckButtonMasterFundonTTValue)
        {
            int prefValue = CheckButtonMasterFundonTTValue.ToUpper().Equals("TRUE")?1:0;
            updateCheckBoxValueQuery = "update T_PranaKeyValuePreferences set PreferenceValue = "+prefValue+" where PreferenceKey = 'IsShowMasterFundonTT'";
        }

        /// <summary>
        /// Sets the autogrouping checkbox
        /// </summary>
        public static void setAutoGroupPreferencesValueQuery(DataTable dt)
        {
            foreach(DataRow dr in dt.Rows)
             {
                string autoGroup = dr["AutoGroup"].ToString();
                string tradeAttribute1 = dr["TradeAttribute1"].ToString();
                string tradeAttribute2 = dr["TradeAttribute2"].ToString();
                string tradeAttribute3 = dr["TradeAttribute3"].ToString();
                string tradeAttribute4 = dr["TradeAttribute4"].ToString();
                string tradeAttribute5 = dr["TradeAttribute5"].ToString();
                string tradeAttribute6 = dr["TradeAttribute6"].ToString();
                string broker = dr["Broker"].ToString();
                string venue =  dr["Venue"].ToString();
                string tradingAC =  dr["TradingAC"].ToString();
                string tradeDate =  dr["TradeDate"].ToString();
                string processDate = dr["ProcessDate"].ToString();
                int prefValue = autoGroup.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue1 = tradeAttribute1.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue2 = tradeAttribute2.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue3 = tradeAttribute3.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue4 = tradeAttribute4.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue5 = tradeAttribute5.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue6 = tradeAttribute6.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue7 = broker.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue8 = venue.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue9 = tradingAC.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue10 = tradeDate.ToUpper().Equals("TRUE") ? 1 : 0;
                int prefValue11 = processDate.ToUpper().Equals("TRUE") ? 1 : 0;
                updateAutoGroupPreferencesValueQuery = "Update T_AutoGroupingPref set AutoGroup = " + prefValue + ", TradeAttribute1 = " + prefValue1 + ", TradeAttribute2 = " + prefValue2 + ", TradeAttribute3 = " + prefValue3 +", TradeAttribute4 = " + prefValue4 + ", TradeAttribute5 = " + prefValue5 +", TradeAttribute6 = "+ prefValue6 +", Broker = "+ prefValue7 +", Venue = " + prefValue8 +", TradingAC = " + prefValue9 +", TradeDate = "+ prefValue10 +", ProcessDate = "+ prefValue11 +"  where CompanyId > 0  ";
                                                        
             }        
        }

        /// <summary>
        /// Sets the autogrouping account
        /// </summary
        public static void setAutoGroupingAccountValueQuery(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string fundName = dr["FundName"].ToString();
                if (fundName == "Unallocated")
                {
                    updateAutoGroupingAccountUnallocatedValueQuery = "Update T_AutoGroupingFunds set AutoGroup = 'TRUE' where FundId = 0";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateAutoGroupingAccountUnallocatedValueQuery);
                }
                else
                {
                    updateAutoGroupingAccountValueQuery = "Declare @Fundid int" + Environment.NewLine +
                                                          "Select @Fundid = CompanyFundID from T_CompanyFunds where FundName= \'" + fundName + "\'" + Environment.NewLine +
                                                          "Update T_AutoGroupingFunds set AutoGroup = 'TRUE' where FundId = @Fundid";
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateAutoGroupingAccountValueQuery);
                }
            }

        }
     
        /// <summary>
        /// Sets the avg price rounding checkbox
        /// </summary
        public static void setAvgPriceRoundingValueQuery(string AvgPriceRoundingCheckBox)
        {
            int prefValue = AvgPriceRoundingCheckBox.ToUpper().Equals("TRUE") ? 0 : -1;
            updateAvgPriceRoundingValueQuery = "update T_PranaKeyValuePreferences set PreferenceValue = " + prefValue + " where PreferenceKey = 'AvgPriceRounding' ";

        }

         /// <summary>
        /// Sets the avg price rounding value
        /// </summary

        public static void setRoundingDigitQuery(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                 string roundingDigit = dr["RoundingDigit"].ToString();
                 //string avgPriceRounding = dr["AvgPricRounding"].ToString();
                 if (roundingDigit != null && roundingDigit != string.Empty)
                 {
                     updateRoundingDigitQuery = "Update T_PranaKeyValuePreferences set PreferenceValue =  " + roundingDigit + " where PreferenceKey = 'AvgPriceRounding'"; updateRoundingDigitQuery = "Update T_PranaKeyValuePreferences set PreferenceValue =  " + roundingDigit + " where PreferenceKey = 'AvgPriceRounding'";
                 }
                 else
                 {
                     updateRoundingDigitQuery = null;
                 }

            }
        }   
            
    }
}


