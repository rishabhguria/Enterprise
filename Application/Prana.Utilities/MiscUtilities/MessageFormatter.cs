using Prana.BusinessObjects.Compliance.Alerting;
using Prana.LogManager;
using System;
using System.Data;
using System.Text;

namespace Prana.Utilities.MiscUtilities
{
    public class MessageFormatter
    {
        //Fields passed as dataSet(object data)
        public static void FormatToOverrideMessage(object data, out String message)
        {
            String summary;
            try
            {
                DataSet dsTemp = data as DataSet;

                summary = "\nRule Summary: " + dsTemp.Tables[0].Rows[0]["Summary"].ToString();
                String validationTime = "\n\nValidation Time: " + dsTemp.Tables[0].Rows[0]["validationTime"].ToString();
                String compressionLevel = "\n\nCompression Level: " + dsTemp.Tables[0].Rows[0]["compressionLevel"].ToString();
                String currentParameter = "\n\nCurrent Parameter: " + dsTemp.Tables[0].Rows[0]["parameters"].ToString();
                String ruleName = "\n\nRule Violated: " + FormatRuleNameForDisplay(dsTemp.Tables[0].Rows[0]["name"].ToString());
                message = @"This trade has been blocked by pre trade compliance." +
                                    summary + validationTime + compressionLevel +
                                    currentParameter + ruleName + "\n\nDo you want to ALLOW this trade?";
            }
            catch (Exception ex)
            {
                message = "Some Error has occured while formatting message.";
                summary = "Not available";

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        public static DataSet FormatToOverrrideResponseMessage(object data, bool isAllowed)
        {
            DataSet dsTemp = data as DataSet;
            try
            {
                dsTemp.Tables[0].Columns.Add("IsAllowed", typeof(bool));
                dsTemp.Tables[0].Rows[0]["IsAllowed"] = isAllowed;
                /* DataTable sendingInformation = new DataTable("OverrideResponse");
                sendingInformation.Columns.Add("OrderId");
                sendingInformation.Columns.Add("IsAllowed");
                sendingInformation.Columns.Add("Summary");
                //sendingInformation.Rows.Add(new Object[] { orderId,"true"});

                sendingInformation.Rows.Add(new Object[] { orderId, isAllowed, message });

                    dsTemp.Tables.Add(sendingInformation);*/
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dsTemp;
        }

        /// <summary>
        /// Returns message to be shown on tray notification
        /// </summary>
        /// <param name="dsTemp"></param>
        /// <returns></returns>
        public static String FormatNotificationMessage(DataSet dsTemp)
        {


            StringBuilder builder = new StringBuilder();
            builder.Append("<br/><b>Rule validated:</b> ");
            builder.AppendLine(FormatRuleNameForDisplay(dsTemp.Tables[0].Rows[0]["name"].ToString()));

            builder.Append("<b>Compliance Level:</b> ");
            builder.AppendLine(dsTemp.Tables[0].Rows[0]["compressionLevel"].ToString());

            builder.Append("<b>Validation Time:</b> ");
            builder.AppendLine(dsTemp.Tables[0].Rows[0]["validationTime"].ToString());

            builder.Append("<b>Rule Description:</b> ");
            builder.AppendLine(dsTemp.Tables[0].Rows[0]["description"].ToString());

            builder.Append("<b>Current values:</b> ");
            builder.AppendLine(dsTemp.Tables[0].Rows[0]["parameters"].ToString());

            builder.Append("<b>Rule Summary:</b> ");
            builder.AppendLine(dsTemp.Tables[0].Rows[0]["Summary"].ToString());

            builder.Append("<b>Action applied:</b> ");
            builder.AppendLine(dsTemp.Tables[0].Rows[0]["Status"].ToString());

            return builder.ToString();
        }

        /// <summary>
        /// Description is formatted for alert pop up
        /// </summary>
        /// <param name="dsTemp"></param>
        /// <returns>Description to be shown on alert pop up</returns>
        public static String FormatAlertDescription(DataSet dsTemp)
        {


            try
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("Compliance Level: ");
                builder.AppendLine(dsTemp.Tables[0].Rows[0]["compressionLevel"].ToString());
                builder.AppendLine("");

                builder.Append("Last Validation Time: ");
                builder.AppendLine(dsTemp.Tables[0].Rows[0]["validationTime"].ToString());
                builder.AppendLine("");

                builder.Append("Rule Description: ");
                builder.AppendLine(dsTemp.Tables[0].Rows[0]["summary"].ToString());
                builder.AppendLine("");

                builder.Append("Current values: ");
                builder.AppendLine(dsTemp.Tables[0].Rows[0]["parameters"].ToString());

                if (dsTemp.Tables[0].Columns.Contains("RuleType") && dsTemp.Tables[0].Rows[0]["RuleType"].ToString().Equals("PreTrade"))
                {
                    builder.AppendLine("");
                    builder.Append("Action applied: ");
                    builder.AppendLine(dsTemp.Tables[0].Rows[0]["Status"].ToString());
                }

                if (dsTemp.Tables[0].Columns.Contains("complianceOfficerNotes") && !(String.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["complianceOfficerNotes"].ToString())))
                {
                    builder.AppendLine("");
                    builder.Append("Compliance Officer Notes: ");
                    builder.AppendLine(dsTemp.Tables[0].Rows[0]["complianceOfficerNotes"].ToString());
                }

                return builder.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return "";
            }

        }

        public static string FormatRuleNameForGuvnor(string ruleName)
        {
            try
            {
                string rule1 = ruleName;
                while (rule1.Contains(" ") || rule1.Contains("%"))
                {
                    if (rule1.Contains(" "))
                        rule1 = rule1.Replace(" ", "(20)");
                    else if (rule1.Contains("%"))
                        rule1 = rule1.Replace("%", "(25)");
                    else
                        rule1 = ruleName;
                }
                return rule1;
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
                return String.Empty;
            }
            //return ruleName;
        }

        public static string FormatRuleNameForDisplay(string ruleName)
        {
            try
            {
                string rule1 = ruleName;
                while (rule1.Contains("(20)") || rule1.Contains("(25)"))
                {
                    if (rule1.Contains("(20)"))
                        rule1 = rule1.Replace("(20)", " ");
                    else if (rule1.Contains("(25)"))
                        rule1 = rule1.Replace("(25)", "%");
                    else
                        rule1 = ruleName;
                }
                return rule1;
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
                return String.Empty;
            }
            //return ruleName;
        }

        public static string FormatRuleNameForAlert(string dimension)
        {
            return " [" + dimension + "] ";
        }

        /// <summary>
        /// Convert the data row into alert text
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String FormatToOverrideMessage(DataRow data)
        {
            String message;
            try
            {
                String summary = "\nRule Summary: " + data["Summary"].ToString();
                // String description = "\n\nRule Description: " + data["Description"].ToString();
                String validationTime = "\n\nValidation Time: " + data["validationTime"].ToString();
                String compressionLevel = "\n\nCompression Level: " + data["compressionLevel"].ToString();
                String currentParameter = "\n\nCurrent Parameter: " + data["parameters"].ToString();
                String ruleName = "\n\nRule Violated: " + FormatRuleNameForDisplay(data["RuleName"].ToString());

                message = summary + validationTime + compressionLevel +
                                    currentParameter + ruleName + "\n";

            }
            catch (Exception ex)
            {
                message = "Some Error has occured while formatting message.";

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return message;
        }

        /// <summary>
        /// Convert the alert object into alert text
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String FormatToOverrideMessage(Alert alert)
        {
            String message;
            try
            {
                String summary = "\nRule Summary: " + alert.Summary;
                String validationTime = "\n\nValidation Time: " + alert.ValidationTime;
                String compressionLevel = "\n\nCompression Level: " + alert.CompressionLevel;
                String currentParameter = "\n\nCurrent Parameter: " + alert.Parameters;
                String ruleName = "\n\nRule Violated: " + FormatRuleNameForDisplay(alert.RuleName);

                message = summary + validationTime + compressionLevel +
                                    currentParameter + ruleName + "\n";

            }
            catch (Exception ex)
            {
                message = "Some Error has occured while formatting message.";

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return message;
        }
    }
}