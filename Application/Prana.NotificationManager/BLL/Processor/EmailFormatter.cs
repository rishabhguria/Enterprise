using Prana.BusinessObjects.Compliance;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.NotificationManager.BLL.Processor
{
    internal static class EmailFormatter
    {

        static String _companyName = String.Empty;
        static String _compliancePostTradeRuleViolationSummaries = String.Empty;
        static String _compliancePostTradeRuleType = String.Empty;
        //static String _hostName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        static EmailFormatter()
        {
            try
            {
                _companyName = CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyName"].ToString();
                _compliancePostTradeRuleViolationSummaries = Prana.Global.ConfigurationHelper.Instance.GetAppSettingValueByKey("CompliancePostTradeRuleViolationSummaries");
                _compliancePostTradeRuleType = Prana.Global.ConfigurationHelper.Instance.GetAppSettingValueByKey("CompliancePostTradeRuleType");
                //_hostName = Dns.GetHostName();
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
        /// Format mail message
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        internal static void FormatAlertMessage(Alert alert, ref String subject, out String message)
        {
            try
            {

                StringBuilder builder = new StringBuilder();
                //builder.AppendLine("Some rules have been violated.<br/><br/>");
                String ruleType = alert.PackageName.ToString();
                if (ruleType == "PostTrade")
                {
                    builder.AppendLine("<h4><b><i>" + _compliancePostTradeRuleViolationSummaries + ":</i></b></h4>");

                }
                else
                {
                    builder.AppendLine("<h4><b><i>Rule Violation Summaries:</i></b></h4>");
                }

                subject += alert.Summary;
                //if (ruleType == "PreTrade")
                builder.AppendLine("<br/><font color=\"black\" size=\"3\">--------------------------------------------------------------------------");
                //else
                //    builder.AppendLine("<br/><font color=\"red\" size=\"2\">--------------------------------------------------------------------------");
                builder.Append("<br/><b>Rule Type:</b> ");
                if (ruleType == "PostTrade")
                {
                    builder.AppendLine(_compliancePostTradeRuleType);
                }
                else
                {
                    builder.AppendLine(ruleType);
                }
                builder.Append("<br/><b>Rule Validated:</b> ");
                builder.AppendLine(alert.RuleName);

                //builder.Append("<br/><b>Description:</b> ");
                //builder.AppendLine(dr["Description"].ToString());


                builder.Append("<br/><b>Compliance Level:</b> ");
                builder.AppendLine(alert.CompressionLevel);

                builder.Append("<br/><b>Current Values:</b> ");
                builder.AppendLine(alert.Parameters);



                if (ruleType == "PreTrade")
                {
                    builder.Append("<br/><b>User:</b> ");
                    int userId = Convert.ToInt32(alert.UserId);
                    if (userId != 0)
                        builder.AppendLine(CachedDataManager.GetInstance.GetUserText(userId));
                    else
                        builder.AppendLine("Not Available");

                    builder.Append("<br/><b>Order ID:</b> ");
                    builder.AppendLine(alert.OrderId);
                    if (!String.IsNullOrEmpty(alert.Status))
                    {
                        builder.Append("<br/><b>Action Applied:</b> ");
                        builder.AppendLine(alert.Status);
                    }
                }

                //String userId = CachedDataManager.GetInstance.GetUserText(Convert.ToInt32(dr["UserId"].ToString()));



                builder.Append("<br/><b>Rule Validation Time:</b> ");
                builder.AppendLine(alert.ValidationTime.ToString());


                builder.Append("<br/><b>User Summary:</b> ");
                builder.AppendLine(alert.Summary);

                if (!String.IsNullOrEmpty(alert.UserNotes))
                {
                    builder.Append("<br/><b>User Notes:</b> ");
                    builder.AppendLine(alert.UserNotes);
                }
                if (!String.IsNullOrEmpty(alert.TradeDetails))
                {
                    builder.Append("<br/><b>Trade Details:</b> ");
                    builder.AppendLine(alert.TradeDetails);
                }
                if (!string.IsNullOrEmpty(alert.ActionUser.ToString()) && alert.ActionUser != -1)
                {
                    string actionUserName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(alert.ActionUser);
                    builder.Append("<br/><b>Compliance Officer:</b> ");
                    builder.AppendLine(actionUserName);
                }

                if (!string.IsNullOrEmpty(alert.ComplianceOfficerNotes))
                {
                    builder.Append("<br/><b>Compliance Officer's Notes:</b> ");
                    builder.AppendLine(alert.ComplianceOfficerNotes);
                }

                builder.AppendLine("<br/>--------------------------------------------------------------------------<br/></font>");

                /*
                message = "This is a " + alert.Rows[0]["RuleType"].ToString() +
                    " Alert\n\nCompression Level: " + alert.Rows[0]["CompressionLevel"].ToString() +
                    "\nRule Summary: " + alert.Rows[0]["Summary"].ToString() +
                    "\nCurrent Parameter: " + alert.Rows[0]["curParameter"].ToString() +
                    "\nValidation Time: " + alert.Rows[0]["ValidationTime"].ToString();
                */


                builder.AppendLine("<br/><br/><br/><br/><br/>");
                builder.AppendLine(@"<span style=");
                builder.Append('"');
                builder.Append("color:#444444;");
                builder.Append('"');
                builder.Append("><h6><i>This is an autogenerated mail sent by Nirvana compliance and alerting system");

                if (!String.IsNullOrEmpty(_companyName))
                {
                    builder.Append(" for ");
                    builder.Append(_companyName.ToUpper());//.rows
                }

                //if (!String.IsNullOrEmpty(_hostName))
                //{
                //    builder.Append(" from ");
                //    builder.Append(_hostName);
                //    builder.AppendLine(" ");
                //}


                builder.AppendLine("</i></h6></span>");
                builder.AppendLine("");



                message = builder.ToString();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                message = "";

            }
        }

        /// <summary>
        /// Formats the alerts in one message.
        /// </summary>
        /// <param name="alerts">The alerts.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        internal static void FormatAlertsInOneMessage(List<Alert> alerts, ref String subject, out String message)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                StringBuilder summaryBuilder = new StringBuilder();
                if (alerts[0].Summary != string.Empty)
                {
                    alerts.ForEach(x => summaryBuilder.Append(x.Summary + ", "));
                    subject += summaryBuilder.ToString(0, summaryBuilder.Length - 2);
                }
                builder.Append("<br/><b>Rule Type: ");
                if (alerts[0].PackageName.ToString() == "PostTrade")
                {
                    builder.AppendLine(_compliancePostTradeRuleType);
                }
                else
                {
                    builder.AppendLine(alerts[0].PackageName.ToString());
                }

                builder.Append("<br/><b>Rule Name: </b>");
                builder.AppendLine(alerts[0].RuleName);
                builder.Append("</b><br/><b>Rule Breach Date Time: </b>");
                builder.AppendLine(DateTime.Now.ToString());
                if (alerts[0].Summary != string.Empty)
                {
                    builder.Append("<br/><b>User Summary: </b> ");
                    builder.AppendLine(summaryBuilder.ToString(0, summaryBuilder.Length - 2));
                }
                builder.Append("</b><br/><b>Compliance Level:</b> ");
                builder.AppendLine(alerts[0].CompressionLevel);
                if (alerts[0].Parameters != string.Empty)
                {
                    builder.Append("</b><br/><b>Current Values:</b> ");
                    alerts.ForEach(alert => builder.AppendLine("<br/>" + alert.Parameters));
                }

                builder.AppendLine("<br/>--------------------------------------------------------------------------<br/></font>");
                builder.AppendLine("<br/><br/><br/><br/><br/>");
                builder.AppendLine(@"<span style=");
                builder.Append('"');
                builder.Append("color:#444444;");
                builder.Append('"');
                builder.Append("><h6><i>This is an autogenerated mail sent by Nirvana compliance and alerting system");

                if (!String.IsNullOrEmpty(_companyName))
                {
                    builder.Append(" for ");
                    builder.Append(_companyName.ToUpper());//.rows
                }
                builder.AppendLine("</i></h6></span>");
                builder.AppendLine("");

                message = builder.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                message = "";
            }
        }


        /// <summary>
        /// returns rule wise recipient list
        /// </summary>
        /// <param name="recipientsCSV"></param>
        /// <returns></returns>
        internal static List<string> GetRulewiseList(string recipientsCSV)
        {
            List<string> recipientsList = new List<string>();
            try
            {

                if (recipientsCSV == null)
                    return recipientsList;
                String[] list = recipientsCSV.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (list.Length > 0)
                {
                    foreach (String s in list)
                    {
                        if (!recipientsList.Contains(s.Trim()))
                            recipientsList.Add(s.Trim());

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

            return recipientsList;
        }

        /// <summary>
        /// Format Approval Message
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        internal static void FormatApprovalMessage(Alert alert, out string subject, out string message, string emailSub = null)
        {
            try
            {
                subject = "Nirvana: Compliance and Alerting: PENDING APPROVAL - ";

                StringBuilder builder = new StringBuilder();
                String ruleType = alert.PackageName.ToString();
                if (ruleType == "PostTrade")
                {
                    builder.AppendLine("<h4><b><i>" + _compliancePostTradeRuleViolationSummaries + ":</i></b></h4>");

                }
                else
                {
                    builder.AppendLine("<h4><b><i>Rule violation summaries:</i></b></h4>");
                }


                if (!string.IsNullOrWhiteSpace(emailSub))
                    subject += ruleType + ": " + emailSub;
                else
                    subject += ruleType + ": " + alert.Summary;
                builder.AppendLine("<br/><font color=\"black\" size=\"3\">--------------------------------------------------------------------------");
                builder.Append("<br/><b>Rule Type:</b> ");
                if (ruleType == "PostTrade")
                {
                    builder.AppendLine(_compliancePostTradeRuleType);
                }
                else
                {
                    builder.AppendLine(ruleType);
                }
                builder.Append("<br/><b>Rule Validated:</b> ");
                builder.AppendLine(alert.RuleName);
                builder.Append("<br/><b>Compliance Level:</b> ");
                builder.AppendLine(alert.CompressionLevel);
                builder.Append("<br/><b>Current Values:</b> ");
                builder.AppendLine(alert.Parameters);
                if (ruleType == "PreTrade")
                {
                    builder.Append("<br/><b>User:</b> ");
                    int userId = Convert.ToInt32(alert.UserId);
                    if (userId != 0)
                        builder.AppendLine(CachedDataManager.GetInstance.GetUserText(userId));
                    else
                        builder.AppendLine("Not Available");

                    builder.Append("<br/><b>Order ID:</b> ");
                    builder.AppendLine(alert.OrderId);
                    if (!String.IsNullOrEmpty(alert.Status))
                    {
                        builder.Append("<br/><b>Action Applied:</b> ");
                        builder.AppendLine(alert.Status);
                    }
                }

                builder.Append("<br/><b>Rule Validation Time:</b> ");
                builder.AppendLine(alert.ValidationTime.ToString());

                builder.Append("<br/><b>User Summary:</b> ");
                builder.AppendLine(alert.Summary);
                if (!String.IsNullOrEmpty(alert.UserNotes))
                {
                    builder.Append("<br/><b>User Notes:</b> ");
                    builder.AppendLine(alert.UserNotes);
                }
                if (!String.IsNullOrEmpty(alert.TradeDetails))
                {
                    builder.Append("<br/><b>Trade Details:</b> ");
                    builder.AppendLine(alert.TradeDetails);
                }
                builder.AppendLine("<br/>--------------------------------------------------------------------------<br/></font>");
                builder.AppendLine("<br/><br/><br/><br/><br/>");
                builder.AppendLine(@"<span style=");
                builder.Append('"');
                builder.Append("color:#444444;");
                builder.Append('"');
                builder.Append("><h6><i>This is an autogenerated mail sent by Nirvana compliance and alerting approval system");

                if (!String.IsNullOrEmpty(_companyName))
                {
                    builder.Append(" for ");
                    builder.Append(_companyName.ToUpper());
                }

                builder.AppendLine("</i></h6></span>");
                builder.AppendLine("");
                message = builder.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                subject = "";
                message = "";

            }
        }
    }
}
