using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessObjects.Compliance.Alerting
{
    [Serializable]
    public class Alert : IEquatable<Alert>
    {
        #region Properties
        public String AlertId { get; set; }
        public String RuleName { get; set; }
        public String Description { get; set; }
        public DateTime ValidationTime { get; set; }
        public String ConstraintFields { get; set; }
        public String Threshold { get; set; }
        public String ActualResult { get; set; }
        public String OrderId { get; set; }
        public Boolean IsViolated { get; set; }
        public Boolean IsEOM { get; set; }
        public String CompressionLevel { get; set; }
        public String Summary { get; set; }
        public int UserId { get; set; }
        public RulePackage PackageName { get; set; }
        public String Parameters { get; set; }
        public String Dimension { get; set; }
        public Boolean Blocked { get; set; }
        public String RuleId { get; set; }
        public String Status { get; set; }
        public String GroupId { get; set; }
        public PreTradeType PreTradeType { get; set; }
        public PreTradeActionType PreTradeActionType { get; set; }
        public AlertType AlertType { get; set; }
        public int ActionUser { get; set; }
        public String ActionUserName { get; set; }
        public String OverrideUserId { get; set; }
        public String UserName { get; set; }
        public String TradeDetails { get; set; }
        public string ComplianceOfficerNotes { get; set; }
        public string UserNotes { get; set; }
        public AlertPopUpResponse AlertPopUpResponse { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Alert()
        {
            this.AlertId = String.Empty;
            this.Summary = String.Empty;
            this.Description = String.Empty;
            this.OrderId = String.Empty;
            this.IsViolated = true;
            this.IsEOM = true;
            this.CompressionLevel = String.Empty;
            this.UserId = -1;
            this.RuleName = String.Empty;
            this.ValidationTime = DateTime.Now;
            this.PackageName = RulePackage.None;
            this.Parameters = String.Empty;
            this.Dimension = String.Empty;
            this.Blocked = true;
            this.RuleId = String.Empty;
            this.Status = String.Empty;
            this.GroupId = "-1";
            this.PreTradeType = PreTradeType.Trade;
            this.ActionUser = -1;
            this.ActionUserName = String.Empty;
            this.PreTradeActionType = PreTradeActionType.NoAction;
            this.OverrideUserId = String.Empty;
            this.ConstraintFields = String.Empty;
            this.Threshold = String.Empty;
            this.ActualResult = String.Empty;
            this.AlertType = AlertType.SoftAlert;
            this.ComplianceOfficerNotes = String.Empty;
            this.UserNotes = String.Empty;
            this.TradeDetails = String.Empty;
            this.AlertPopUpResponse = AlertPopUpResponse.None;
        }

        /// <summary>
        /// Returns alert for data set.
        /// used for rabbit mq received data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Alert GetAlertObject(DataSet data)
        {
            try
            {
                Alert alert = new Alert();

                if (data.Tables[0].Columns.Contains("ValidationTime"))
                {
                    DateTime valTime;
                    DateTime.TryParse(data.Tables[0].Rows[0]["ValidationTime"].ToString(), out valTime);
                    alert.ValidationTime = valTime;
                }
                if (data.Tables[0].Columns.Contains("RuleId"))
                    alert.RuleId = data.Tables[0].Rows[0]["RuleId"].ToString();
                if (data.Tables[0].Columns.Contains("Name"))
                    alert.RuleName = data.Tables[0].Rows[0]["Name"].ToString();
                alert.AlertId =
                    (alert.RuleName.Contains("not started completely") ||
                    alert.RuleName.Contains("Calculation engine algorithm unable to complete successfully"))
                    ? PreTradeConstants.CONST_FAILED_ALERT_ID : uIDGenerator.GenerateID();
                if (data.Tables[0].Columns.Contains("UserId"))
                    alert.UserId = Convert.ToInt32(data.Tables[0].Rows[0]["UserId"]);
                if (data.Tables[0].Columns.Contains("RuleType") && !String.IsNullOrEmpty(data.Tables[0].Rows[0]["RuleType"].ToString()))
                {
                    alert.PackageName = (RulePackage)Enum.Parse(typeof(RulePackage), data.Tables[0].Rows[0]["RuleType"].ToString());
                    if (alert.PackageName == RulePackage.Basket)
                        alert.PackageName = RulePackage.PreTrade;
                }
                if (data.Tables[0].Columns.Contains("Summary"))
                    alert.Summary = data.Tables[0].Rows[0]["Summary"].ToString();
                if (data.Tables[0].Columns.Contains("CompressionLevel"))
                    alert.CompressionLevel = data.Tables[0].Rows[0]["CompressionLevel"].ToString();
                if (data.Tables[0].Columns.Contains("Parameters"))
                    alert.Parameters = data.Tables[0].Rows[0]["Parameters"].ToString();
                if (data.Tables[0].Columns.Contains("OrderId"))
                    alert.OrderId = data.Tables[0].Rows[0]["OrderId"].ToString();
                if (data.Tables[0].Columns.Contains("Status"))
                    alert.Status = data.Tables[0].Rows[0]["Status"].ToString();
                if (data.Tables[0].Columns.Contains("Description"))
                    alert.Description = data.Tables[0].Rows[0]["Description"].ToString();
                if (data.Tables[0].Columns.Contains("Dimension"))
                    alert.Dimension = data.Tables[0].Rows[0]["Dimension"].ToString();
                if (data.Tables[0].Columns.Contains("violated"))
                    alert.IsViolated = Convert.ToBoolean(data.Tables[0].Rows[0]["violated"].ToString());
                if (data.Tables[0].Columns.Contains("blocked"))
                {
                    bool isBlocked = Convert.ToBoolean(data.Tables[0].Rows[0]["blocked"].ToString());
                    alert.Blocked = isBlocked;
                    alert.AlertType = isBlocked ? AlertType.HardAlert : AlertType.SoftAlert;
                }
                if (data.Tables[0].Columns.Contains("isEOM") && !String.IsNullOrEmpty(data.Tables[0].Rows[0]["isEOM"].ToString()))
                    alert.IsEOM = Convert.ToBoolean(data.Tables[0].Rows[0]["isEOM"].ToString());
                if (data.Tables[0].Columns.Contains("PreTradeType"))
                    alert.PreTradeType = (PreTradeType)Enum.Parse(typeof(PreTradeType), data.Tables[0].Rows[0]["PreTradeType"].ToString());
                if (data.Tables[0].Columns.Contains("ComplianceOfficerNotes"))
                    alert.ComplianceOfficerNotes = data.Tables[0].Rows[0]["ComplianceOfficerNotes"].ToString();
                if (data.Tables[0].Columns.Contains("TradeDetails"))
                    alert.TradeDetails = data.Tables[0].Rows[0]["TradeDetails"].ToString();

                if (data.Tables[0].Columns.Contains("ActionUser"))
                    alert.ActionUser = Convert.ToInt32(data.Tables[0].Rows[0]["ActionUser"].ToString());
                if (data.Tables[0].Columns.Contains("PreTradeActionType"))
                    alert.PreTradeActionType = (PreTradeActionType)Enum.Parse(typeof(PreTradeActionType), data.Tables[0].Rows[0]["PreTradeActionType"].ToString());
                if (data.Tables[0].Columns.Contains("OverrideUserId"))
                    alert.OverrideUserId = data.Tables[0].Rows[0]["OverrideUserId"].ToString();
                if (data.Tables[0].Columns.Contains("ConstraintFields"))
                    alert.ConstraintFields = data.Tables[0].Rows[0]["ConstraintFields"].ToString();
                if (data.Tables[0].Columns.Contains("Threshold"))
                    alert.Threshold = data.Tables[0].Rows[0]["Threshold"].ToString();
                if (data.Tables[0].Columns.Contains("ActualResult"))
                    alert.ActualResult = data.Tables[0].Rows[0]["ActualResult"].ToString();
                if (data.Tables[0].Columns.Contains("UserNotes"))
                    alert.UserNotes = data.Tables[0].Rows[0]["UserNotes"].ToString();
                if (data.Tables[0].Columns.Contains("AlertPopUpResponse"))
                    alert.AlertPopUpResponse = (AlertPopUpResponse)Enum.Parse(typeof(AlertPopUpResponse), data.Tables[0].Rows[0]["AlertPopUpResponse"].ToString());

                return alert;
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
        /// Returns data set for alert object for
        /// sending alert to client.
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static DataSet GetAlertDataSet(Alert alert)
        {
            try
            {
                DataTable dtTemp = new DataTable("Alert");
                dtTemp.Columns.Add("Name");
                dtTemp.Columns.Add("UserId");
                dtTemp.Columns.Add("CompressionLevel");
                dtTemp.Columns.Add("ValidationTime");
                dtTemp.Columns.Add("RuleType");
                dtTemp.Columns.Add("Parameters");
                dtTemp.Columns.Add("Summary");
                dtTemp.Columns.Add("OrderId");
                dtTemp.Columns.Add("Status");
                dtTemp.Columns.Add("RuleId");
                dtTemp.Columns.Add("Description");
                dtTemp.Columns.Add("Dimension");
                dtTemp.Columns.Add("blocked");
                dtTemp.Columns.Add("PreTradeType");
                dtTemp.Columns.Add("ComplianceOfficerNotes");
                dtTemp.Columns.Add("ActionUser");
                dtTemp.Columns.Add("PreTradeActionType");
                dtTemp.Columns.Add("OverrideUserId");
                dtTemp.Columns.Add("ConstraintFields");
                dtTemp.Columns.Add("Threshold");
                dtTemp.Columns.Add("ActualResult");
                dtTemp.Columns.Add("AlertType");
                dtTemp.Columns.Add("UserNotes");
                dtTemp.Columns.Add("TradeDetails");
                dtTemp.Columns.Add("AlertPopUpResponse");


                dtTemp.Rows.Add(new Object[] {
                alert.RuleName,
                alert.UserId,
                alert.CompressionLevel,
                alert.ValidationTime,
                alert.PackageName,
                alert.Parameters,
                alert.Summary,
                alert.OrderId,
                alert.Status,
                alert.RuleId,
                alert.Description,
                alert.Dimension,
                alert.Blocked,
                alert.PreTradeType,
                alert.ComplianceOfficerNotes,
                alert.ActionUser,
                alert.PreTradeActionType,
                alert.OverrideUserId,
                alert.ConstraintFields,
                alert.Threshold,
                alert.ActualResult,
                alert.AlertType,
                alert.UserNotes,
                alert.TradeDetails,
                alert.AlertPopUpResponse
            });
                //dtTemp.Rows.Add(dr);
                DataSet dsAlert = new DataSet("Alert");
                dsAlert.Tables.Add(dtTemp);
                return dsAlert;
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
        /// Returns data set for alert object for
        /// sending alert to client.
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static DataSet RowToDataset(DataRow row)
        {
            try
            {
                DataTable dtTemp = new DataTable("Alert");
                dtTemp.Columns.Add("Name");
                dtTemp.Columns.Add("UserId");
                dtTemp.Columns.Add("CompressionLevel");
                dtTemp.Columns.Add("ValidationTime");
                dtTemp.Columns.Add("RuleType");
                dtTemp.Columns.Add("Parameters");
                dtTemp.Columns.Add("Summary");
                dtTemp.Columns.Add("OrderId");
                dtTemp.Columns.Add("Status");
                dtTemp.Columns.Add("RuleId");
                dtTemp.Columns.Add("Description");
                dtTemp.Columns.Add("Dimension");
                dtTemp.Columns.Add("blocked");
                dtTemp.Columns.Add("PreTradeType");
                dtTemp.Columns.Add("ComplianceOfficerNotes");
                dtTemp.Columns.Add("ActionUser");
                dtTemp.Columns.Add("PreTradeActionType");
                dtTemp.Columns.Add("OverrideUserId");
                dtTemp.Columns.Add("ConstraintFields");
                dtTemp.Columns.Add("Threshold");
                dtTemp.Columns.Add("ActualResult");
                dtTemp.Columns.Add("AlertType");
                dtTemp.Columns.Add("UserNotes");
                dtTemp.Columns.Add("TradeDetails");


                dtTemp.Rows.Add(new Object[] {
                row["Name"],
                row["UserId"],
                row["CompressionLevel"],
                row["ValidationTime"],
                row["RuleType"],
                row["Parameters"],
                row["Summary"],
                row["OrderId"],
                row["Status"],
                row["RuleId"],
                row["Description"],
                row["Dimension"],
                row["blocked"],
                row["PreTradeType"],
                row["ComplianceOfficerNotes"],
                row["ActionUser"],
                row["PreTradeActionType"],
                row["OverrideUserId"],
                row["ConstraintFields"],
                row["Threshold"],
                row["ActualResult"],
                row["AlertType"],
                row["UserNotes"],
                row["TradeDetails"]
            }); ;
                //dtTemp.Rows.Add(dr);
                DataSet dsAlert = new DataSet("Alert");
                dsAlert.Tables.Add(dtTemp);
                return dsAlert;
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

        // <summary>
        /// Returns data set for total alerts object for
        /// sending alert to client.
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static DataSet GetCombinedAlertsDataSet(List<Alert> alerts)
        {
            try
            {
                DataTable dtTemp = new DataTable("Alert");
                dtTemp.Columns.Add("Name");
                dtTemp.Columns.Add("UserId");
                dtTemp.Columns.Add("CompressionLevel");
                dtTemp.Columns.Add("ValidationTime");
                dtTemp.Columns.Add("RuleType");
                dtTemp.Columns.Add("Parameters");
                dtTemp.Columns.Add("Summary");
                dtTemp.Columns.Add("OrderId");
                dtTemp.Columns.Add("Status");
                dtTemp.Columns.Add("RuleId");
                dtTemp.Columns.Add("Description");
                dtTemp.Columns.Add("Dimension");
                dtTemp.Columns.Add("blocked");
                dtTemp.Columns.Add("PreTradeType");
                dtTemp.Columns.Add("ComplianceOfficerNotes");
                dtTemp.Columns.Add("ActionUser");
                dtTemp.Columns.Add("PreTradeActionType");
                dtTemp.Columns.Add("OverrideUserId");
                dtTemp.Columns.Add("ConstraintFields");
                dtTemp.Columns.Add("Threshold");
                dtTemp.Columns.Add("ActualResult");
                dtTemp.Columns.Add("AlertType");
                dtTemp.Columns.Add("UserNotes");
                dtTemp.Columns.Add("TradeDetails");
                dtTemp.Columns.Add("AlertPopUpResponse");

                foreach (Alert alert in alerts)
                {
                    dtTemp.Rows.Add(new Object[] {
                        alert.RuleName,
                        alert.UserId,
                        alert.CompressionLevel,
                        alert.ValidationTime,
                        alert.PackageName,
                        alert.Parameters,
                        alert.Summary,
                        alert.OrderId,
                        alert.Status,
                        alert.RuleId,
                        alert.Description,
                        alert.Dimension,
                        alert.Blocked,
                        alert.PreTradeType,
                        alert.ComplianceOfficerNotes,
                        alert.ActionUser,
                        alert.PreTradeActionType,
                        alert.OverrideUserId,
                        alert.ConstraintFields,
                        alert.Threshold,
                        alert.ActualResult,
                        alert.AlertType,
                        alert.UserNotes,
                        alert.TradeDetails,
                        alert.AlertPopUpResponse
                    });
                }
                //dtTemp.Rows.Add(dr);
                DataSet dsAlert = new DataSet("Alert");
                dsAlert.Tables.Add(dtTemp);
                return dsAlert;
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
        /// Returns a default error alert
        /// </summary>
        /// <returns></returns>
        public static Alert GetComplianceFailureAlert(bool blockTradeOnComplianceFaliure)
        {
            Alert alert = new Alert();
            alert.RuleName = PreTradeConstants.ConstComplianceFailed;
            alert.IsViolated = true;
            alert.IsEOM = false;
            alert.PackageName = RulePackage.PreTrade;
            alert.Description = "Compliance failed to process ";
            alert.Parameters = "N/A";
            alert.Blocked = true;
            alert.CompressionLevel = "None";
            alert.Dimension = "N/A";
            alert.Threshold = "N/A";
            alert.ActualResult = "N/A";
            alert.AlertId = PreTradeConstants.CONST_FAILED_ALERT_ID;

            if (blockTradeOnComplianceFaliure)
            {
                alert.Summary = "The compliance engine could not process the trade that was just entered and has been blocked. Please retry to enter the trade, or contact your client representative if this issue persists.";
            }
            else
            {
                alert.Summary = "The compliance engine could not process the trade that was just entered. Please retry to enter the trade or contact your client representative. If you override this alert (by clicking yes) and allow the trade to continue, you are acknowledging that compliance was not actually checked before the trade generated.";
            }

            return alert;
        }

        /// <summary>
        /// Returns a default error alert for basket compliance
        /// </summary>
        /// <returns></returns>
        public static Alert GetBasketComplianceFailureAlert(bool blockTradeOnComplianceFaliure)
        {
            Alert alert = new Alert();
            alert.RuleName = PreTradeConstants.ConstBasketComplianceFailed;
            alert.IsViolated = true;
            alert.IsEOM = false;
            alert.PackageName = RulePackage.PreTrade;
            alert.Description = "Basket Compliance failed to process ";
            alert.Parameters = "N/A";
            alert.Blocked = true;
            alert.CompressionLevel = "None";
            alert.Dimension = "N/A";
            alert.Threshold = "N/A";
            alert.ActualResult = "N/A";
            alert.AlertId = PreTradeConstants.CONST_FAILED_ALERT_ID;

            if (blockTradeOnComplianceFaliure)
            {
                alert.Summary = "The basket compliance engine could not process the trade that was just entered and has been blocked. Please retry to enter the trade, or contact your client representative if this issue persists.";
            }
            else
            {
                alert.Summary = "The basket compliance engine could not process the trade that was just entered. Please retry to enter the trade or contact your client representative. If you override this alert (by clicking yes) and allow the trade to continue, you are acknowledging that compliance was not actually checked before the trade generated.";
            }

            return alert;
        }

        /// <summary>
        /// Returns alert for data set.
        /// used for rabbit mq received data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Alert> GetAlertObjectFromDataTable(DataTable dataTable)
        {
            try
            {
                List<Alert> alerts = new List<Alert>();
                foreach (DataRow row in dataTable.Rows)
                {
                    Alert alert = new Alert();
                    if (dataTable.Columns.Contains("AlertId"))
                        alert.AlertId = row["AlertId"].ToString();
                    if (dataTable.Columns.Contains("ValidationTime"))
                    {
                        DateTime valTime;
                        DateTime.TryParse(row["ValidationTime"].ToString(), out valTime);
                        alert.ValidationTime = valTime;
                    }
                    if (dataTable.Columns.Contains("ActionUser"))
                        alert.ActionUser = Convert.ToInt32(row["ActionUser"].ToString());
                    if (dataTable.Columns.Contains("PreTradeActionType"))
                        alert.PreTradeActionType = (PreTradeActionType)Enum.Parse(typeof(PreTradeActionType), row["PreTradeActionType"].ToString());
                    if (dataTable.Columns.Contains("OverrideUserId"))
                        alert.OverrideUserId = row["OverrideUserId"].ToString();

                    if (dataTable.Columns.Contains("RuleId"))
                        alert.RuleId = row["RuleId"].ToString();
                    if (dataTable.Columns.Contains("RuleName"))
                        alert.RuleName = row["RuleName"].ToString();
                    if (dataTable.Columns.Contains("UserId"))
                        alert.UserId = Convert.ToInt32(row["UserId"]);
                    if (dataTable.Columns.Contains("PackageName") && !String.IsNullOrEmpty(row["PackageName"].ToString()))
                        alert.PackageName = (RulePackage)Enum.Parse(typeof(RulePackage), row["PackageName"].ToString());
                    if (dataTable.Columns.Contains("RuleType") && !String.IsNullOrEmpty(row["RuleType"].ToString()))
                        alert.PackageName = (RulePackage)Enum.Parse(typeof(RulePackage), row["RuleType"].ToString());
                    if (dataTable.Columns.Contains("Summary"))
                        alert.Summary = row["Summary"].ToString();
                    if (dataTable.Columns.Contains("CompressionLevel"))
                        alert.CompressionLevel = row["CompressionLevel"].ToString();
                    if (dataTable.Columns.Contains("Parameters"))
                        alert.Parameters = row["Parameters"].ToString();
                    if (dataTable.Columns.Contains("OrderId"))
                        alert.OrderId = row["OrderId"].ToString();
                    if (dataTable.Columns.Contains("Status"))
                        alert.Status = row["Status"].ToString();
                    if (dataTable.Columns.Contains("Description"))
                        alert.Description = row["Description"].ToString();
                    if (dataTable.Columns.Contains("Dimension"))
                        alert.Dimension = row["Dimension"].ToString();
                    if (dataTable.Columns.Contains("IsViolated"))
                        alert.IsViolated = Convert.ToBoolean(row["IsViolated"].ToString());
                    if (dataTable.Columns.Contains("Blocked"))
                    {
                        bool isBlocked = Convert.ToBoolean(row["Blocked"].ToString());
                        alert.Blocked = isBlocked;
                        //alert.AlertType = isBlocked ? AlertType.HardAlert : AlertType.SoftAlert;
                    }
                    if (dataTable.Columns.Contains("AlertType"))
                    {
                        AlertType alertType = (AlertType)Enum.Parse(typeof(AlertType), (row["AlertType"].ToString()));
                        alert.AlertType = alertType;
                    }
                    if (dataTable.Columns.Contains("IsEOM") && !String.IsNullOrEmpty(row["IsEOM"].ToString()))
                        alert.IsEOM = Convert.ToBoolean(row["IsEOM"].ToString());
                    if (dataTable.Columns.Contains("PreTradeType"))
                        alert.PreTradeType = (PreTradeType)Enum.Parse(typeof(PreTradeType), row["PreTradeType"].ToString());
                    if (dataTable.Columns.Contains("ConstraintFields"))
                        alert.ConstraintFields = row["ConstraintFields"].ToString();
                    if (dataTable.Columns.Contains("Threshold"))
                        alert.Threshold = row["Threshold"].ToString();
                    if (dataTable.Columns.Contains("ActualResult"))
                        alert.ActualResult = row["ActualResult"].ToString();
                    if (dataTable.Columns.Contains("ComplianceOfficerNotes"))
                        alert.ComplianceOfficerNotes = row["ComplianceOfficerNotes"].ToString();
                    if (dataTable.Columns.Contains("UserNotes"))
                        alert.UserNotes = row["UserNotes"].ToString();
                    if (dataTable.Columns.Contains("TradeDetails"))
                        alert.TradeDetails = row["TradeDetails"].ToString();
                    //if (!alerts.Contains(alert))
                    alerts.Add(alert);
                }
                return alerts;
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


        #region IEquatable<Alert> Members

        public bool Equals(Alert alert)
        {
            if (this.RuleName.Equals(alert.RuleName) && this.PackageName.Equals(alert.PackageName) && this.Dimension.Equals(alert.Dimension))
                return true;
            return false;
        }

        #endregion
    }
}
