using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities.StringUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace Prana.CounterPartyRules
{
    public class DllOperatorRule : DllBaseRule
    {
        string[] Tags = null;
        string totag = "";
        List<string> operators = new List<string>();
        public override void ApplyRule(Prana.BusinessObjects.FIX.PranaMessage msg)
        {
            try
            {
                string result = string.Empty;
                List<string> Values = new List<string>();
                foreach (string tag in Tags)
                {
                    if (tag.Contains("'"))
                    {
                        Values.Add(tag.Substring(1, tag.Length - 2));
                    }
                    else
                    {
                        FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagValue(tag);
                        if (fixfield != null)
                        {
                            if (fixfield.IsExternal)
                            {
                                if (msg.FIXMessage.ExternalInformation.ContainsKey(tag))
                                    Values.Add(msg.FIXMessage.ExternalInformation[tag].Value);
                            }
                            else
                            {
                                if (msg.FIXMessage.InternalInformation.ContainsKey(tag))
                                    Values.Add(msg.FIXMessage.InternalInformation[tag].Value);
                            }
                        }
                    }
                }
                if (Values.Count > 0)
                {
                    result = Values[0];
                }
                else
                {
                    Logger.HandleException(new Exception("DLL Rule could not get applied for FIX Message : " + msg.ToString() + " Value Of Tags: " + String.Join(",", Tags) + " Value of ToTag is: " + totag), LoggingConstants.POLICY_LOGONLY);
                }
                for (int count = 0; count < Values.Count - 1; count++)
                {
                    result = ApplyOperator(operators[count], result, Values[count + 1]).ToString();
                }

                if (result != string.Empty)
                {
                    FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagValue(totag);
                    if (fixfield.IsExternal)
                    {
                        msg.FIXMessage.ExternalInformation.AddField(totag, result);
                    }
                    else
                    {
                        msg.FIXMessage.InternalInformation.AddField(totag, result);
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

        public override void ApplyRule(PranaMessage msg, List<RepeatingMessageFieldCollection> repeatingFieldList, string repeatingTag)
        {
            try
            {
                foreach (RepeatingMessageFieldCollection msgFields in repeatingFieldList)
                {
                    string result = string.Empty;
                    List<string> Values = new List<string>();
                    foreach (string tag in Tags)
                    {
                        if (tag.Contains("'"))
                        {
                            Values.Add(tag.Substring(1, tag.Length - 2));
                        }
                        else
                        {
                            FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagValue(tag);
                            if (fixfield != null)
                            {
                                if (msgFields.ContainsKey(tag))
                                {
                                    Values.Add(msgFields[tag].Value);
                                }
                                if (fixfield.IsExternal)
                                {
                                    if (msg.FIXMessage.ExternalInformation.ContainsKey(tag))
                                        Values.Add(msg.FIXMessage.ExternalInformation[tag].Value);
                                }
                                else
                                {
                                    if (msg.FIXMessage.InternalInformation.ContainsKey(tag))
                                        Values.Add(msg.FIXMessage.InternalInformation[tag].Value);
                                }
                            }
                        }
                    }
                    if (Values.Count > 0)
                        result = Values[0];
                    else
                        Logger.HandleException(new Exception("DLL Rule could not get applied for FIX Message : " + msg.ToString() + " Value Of Tags: " + String.Join(",", Tags) + " Value of ToTag is: " + totag), LoggingConstants.POLICY_LOGONLY);

                    for (int count = 0; count < Values.Count - 1; count++)
                    {
                        result = ApplyOperator(operators[count], result, Values[count + 1]).ToString();
                    }

                    if (result != string.Empty)
                    {
                        RepeatingFixField orderdedRepeatingFields = FixMessageValidator.GetOrderedRepeatingGroupFields(msg.MessageType, repeatingTag, totag);

                        FixRepeatingFieldHelper.UpdateGroup(msgFields, orderdedRepeatingFields, result, totag);
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

        public override bool CreateRules(System.Xml.XmlNode xmlNodeItem)
        {
            try
            {
                string RuleToApply = string.Empty;
                if (xmlNodeItem.Attributes["ConditionToApply"] != null)
                {
                    RuleToApply = xmlNodeItem.Attributes["ConditionToApply"].Value.Trim();
                    string[] data = RuleToApply.Split('=');
                    totag = data[0];
                    var operatorsRegex = new Regex("((')[^']*('))|([^'+/*-]+)");
                    Tags = operatorsRegex.Matches(data[1]).Cast<Match>().Select(m => m.Value).ToArray();
                    string remainingData = data[1];
                    int count = 0;
                    foreach (string tag in Tags)
                    {
                        if (count < Tags.Length - 1)
                        {
                            string operator1 = remainingData.Substring(tag.Length, 1);
                            remainingData = remainingData.Substring(tag.Length + 1);
                            if (operator1 != string.Empty)
                            {
                                operators.Add(operator1);
                            }
                        }
                        count++;
                    }
                }
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
            return true;
        }

        private object ApplyOperator(string operatorToApply, string value1, string value2)
        {
            try
            {
                if (RegularExpressionValidation.IsNumber(value1) && RegularExpressionValidation.IsNumber(value2))
                {
                    double numvalue1 = double.Parse(value1);
                    double numvalue2 = double.Parse(value2);
                    switch (operatorToApply)
                    {
                        case "+":
                            return numvalue1 + numvalue2;

                        case "-":
                            return numvalue1 - numvalue2;

                        case "*":
                            return numvalue1 * numvalue2;

                        case "/":
                            return numvalue1 / numvalue2;

                        default:
                            return value1 + value2;
                    }
                }
                else
                {
                    return value1 + value2;
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
            return value1 + value2;
        }
    }
}
