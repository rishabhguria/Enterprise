using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities
{
    public class SamsaraSQLQueryManager
    {
        public static string updateCompliancePermission;
        public static string updateTTUIPrefQueryHide;
        public static string updateTTUIPrefQuery;

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



                if (Alert_P_T.ToUpper() == "SOFT")
                {
                    AlertRules.Add(Rule_Name, 1); //also bydefault value is 1
                }
                if (Alert_P_T.ToUpper() == "REQUIRES APPROVAL")
                {
                    AlertRules.Add(Rule_Name, 2);
                }
                if (Alert_P_T.ToUpper() == "HARD")
                {
                    AlertRules.Add(Rule_Name, 3);
                }

                if (Alert_P_T.ToUpper() == "SOFT WITH NOTES")
                {
                    AlertRules.Add(Rule_Name, 4);
                }
            }



            foreach (var x in AlertRules)
            {
                updateCompliancePermission = "Update RUP  set RUP.ruleOverrideType = " + x.Value + " from T_CA_RuleUserPermissions RUP inner join T_CA_RulesUserDefined RUD on RUP.RuleID = RUD.RuleID where RuleName = '" + x.Key + "' AND UserId = '" + User_ID + "' ";
                SqlUtilities.ExecuteQuery(updateCompliancePermission);
            }
        }

          public static void SetTTUIPrefQuery(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string broker = dr["Broker"].ToString();
                string venue = String.Empty;
                string tif = TestDataConstants.COL_DEFAULTIF;
                string trader = TestDataConstants.COL_DEFAULTTRADER;
                string HideTradingQuantity = String.Empty;
                if (dr.Table.Columns.Contains(TestDataConstants.COL_HIDETARGETQUANTITY))
                {
                    HideTradingQuantity = dr["ShowHideTargetQuantity"].ToString();
                }
                int prefValueHide = HideTradingQuantity.ToUpper().Equals("TRUE") ? 1 : 0;

                updateTTUIPrefQueryHide = "Update T_CompanyTTGeneralPreferences set IsShowTargetQTY = " + prefValueHide + " where CompanyId>0";
                SqlUtilities.ExecuteQuery(updateTTUIPrefQueryHide);

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


                if (broker != String.Empty && broker != "NULL")
                {
                    updateTTUIPrefQuery = "Declare @CounterPartyId int" + Environment.NewLine +
                                          "Set @CounterPartyId= (Select CounterPartyId from T_CounterParty where ShortName = \'" + broker + "\')" + Environment.NewLine +
                                          "Update T_CompanyTTGeneralPreferences Set CounterPartyID = @CounterPartyId where CompanyID>0";
                    SqlUtilities.ExecuteQuery(updateTTUIPrefQuery);
                }
                else
                {
                    updateTTUIPrefQuery = "Update T_CompanyTTGeneralPreferences Set CounterPartyID = NULL where CompanyID>0";
                    SqlUtilities.ExecuteQuery(updateTTUIPrefQuery);
                }
                if (venue != String.Empty && venue != "NULL")
                {
                    updateTTUIPrefQuery = "Declare @VenueId int" + Environment.NewLine +
                                          "Set @VenueId = (Select VenueID from T_Venue where VenueName = \'" + venue + "\')" + Environment.NewLine +
                                          "Update T_CompanyTTGeneralPreferences Set VenueID = @VenueId where CompanyID>0";
                    SqlUtilities.ExecuteQuery(updateTTUIPrefQuery);
                }
                else
                {
                    updateTTUIPrefQuery = "Update T_CompanyTTGeneralPreferences Set VenueID = NULL where CompanyID>0";
                    SqlUtilities.ExecuteQuery(updateTTUIPrefQuery);
                }
                if (tif != String.Empty && tif != "NULL")
                {
                    updateTTUIPrefQuery = "Declare @TimeInForceID int" + Environment.NewLine +
                                          "Set @TimeInForceID= (Select TimeInForceID from T_TimeInForce where TimeInForce = \'" + tif + "\')" + Environment.NewLine +
                                          "Update T_CompanyTTGeneralPreferences Set TimeInForceID = @TimeInForceID where CompanyID>0";
                    SqlUtilities.ExecuteQuery(updateTTUIPrefQuery);
                }
                else
                {
                    updateTTUIPrefQuery = "Update T_CompanyTTGeneralPreferences Set TimeInForceID = NULL where CompanyID>0";
                    SqlUtilities.ExecuteQuery(updateTTUIPrefQuery);
                }
                if (trader != String.Empty && trader != "NULL")
                {
                    updateTTUIPrefQuery = "Declare @TradingAccountID int" + Environment.NewLine +
                                          "Set @TradingAccountID = (Select CompanyTradingAccountsID from T_CompanyTradingAccounts where TradingShortName = \'" + trader + "\')" + Environment.NewLine +
                                          "Update T_CompanyTTGeneralPreferences Set TradingAccountID = @TradingAccountID where CompanyID>0";
                    SqlUtilities.ExecuteQuery(updateTTUIPrefQuery);
                }
                else
                {
                    updateTTUIPrefQuery = "Update T_CompanyTTGeneralPreferences Set TimeInForceID = NULL where CompanyID>0";
                    SqlUtilities.ExecuteQuery(updateTTUIPrefQuery);
                }
                if (!string.IsNullOrEmpty(HideTradingQuantity))
                {
                    SqlUtilities.ExecuteQuery(updateTTUIPrefQuery);
                }
            }


            }
    }
}
