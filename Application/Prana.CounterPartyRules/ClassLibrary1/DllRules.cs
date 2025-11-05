using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.CounterPartyRules
{
    public class DllRule1 : DllBaseRule
    {
        string fromSuffix = string.Empty;
        string fromSeperator = "-";
        static Int64 id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssff"));
        public override void ApplyRule(PranaMessage msg)
        {
            try
            {
                string tickerSymbol = msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TickerSymbol].Value;
                tickerSymbol = tickerSymbol.Replace('/', '.');
                if (tickerSymbol != string.Empty && !tickerSymbol.Contains(fromSuffix))
                {
                    tickerSymbol = tickerSymbol + fromSeperator + fromSuffix;
                }

                msg.FIXMessage.InternalInformation.AddField("100048", id.ToString());// Symbol_PK
                id++;
                msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TickerSymbol].Value = tickerSymbol;
                msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagUnderlyingSymbol, tickerSymbol);
            }
            catch (Exception ex)
            {
                Logger.HandleException(new Exception("DLL1 rule could not be get applied for FIX message: " + msg.ToString()), LoggingConstants.POLICY_LOGANDTHROW);
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
                if (xmlNodeItem.Attributes["fromSuffix"] != null)
                    fromSuffix = xmlNodeItem.Attributes["fromSuffix"].Value;
                if (xmlNodeItem.Attributes["fromSeperator"] != null)
                    fromSeperator = xmlNodeItem.Attributes["fromSeperator"].Value;
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
            return true;
        }

        public override void ApplyRule(PranaMessage msg, List<RepeatingMessageFieldCollection> repeatingMessageFields, string repeatingTag)
        {
            return;
        }
    }
    class DllUpdateRule : DllBaseRule
    {
        List<string> toSuffixs = new List<string>();
        List<string> fromSuffixs = new List<string>();
        string[] fromSeperators;
        string[] toSeperators;
        string fromTag;
        string toTag;
        bool StartFromNonZero = true;
        public override void ApplyRule(PranaMessage msg)
        {
            try
            {
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagValue(fromTag);
                string fromSymbol = string.Empty;
                if (fixfield != null)
                {
                    if (fixfield.IsExternal)
                    {
                        fromSymbol = msg.FIXMessage.ExternalInformation[fromTag].Value;
                    }
                    else
                    {
                        fromSymbol = msg.FIXMessage.InternalInformation[fromTag].Value;
                    }

                    if (fromSymbol == string.Empty)
                        return;

                    string toSymbol = GetToSymbol(fromSymbol);

                    if (!string.IsNullOrEmpty(toSymbol))
                    {
                        FixFields fixfieldtoTag = FixDictionaryHelper.GetTagFieldByTagValue(toTag);
                        if (fixfieldtoTag != null)
                        {
                            if (fixfieldtoTag.IsExternal)
                            {
                                msg.FIXMessage.ExternalInformation.AddField(toTag, toSymbol);
                            }
                            else
                            {
                                msg.FIXMessage.InternalInformation.AddField(toTag, toSymbol);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(new Exception("DLL Update rule could not be get applied for FIX message: " + msg.ToString()), LoggingConstants.POLICY_LOGANDTHROW);
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
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagValue(fromTag);
                string fromSymbol = string.Empty;

                if (fixfield != null)
                {
                    if (fixfield.IsExternal)
                    {
                        fromSymbol = msg.FIXMessage.ExternalInformation[fromTag].Value;
                    }
                    else
                    {
                        fromSymbol = msg.FIXMessage.InternalInformation[fromTag].Value;
                    }

                    foreach (RepeatingMessageFieldCollection msgFields in repeatingFieldList)
                    {
                        if (msgFields.ContainsKey(fromTag))
                        {
                            fromSymbol = msgFields[fromTag].Value;
                        }
                        if (fromSymbol == string.Empty)
                            continue;

                        string toSymbol = GetToSymbol(fromSymbol);
                        if (string.IsNullOrEmpty(toSymbol))
                        {
                            RepeatingFixField orderdedRepeatingFields = FixMessageValidator.GetOrderedRepeatingGroupFields(msg.MessageType, repeatingTag, toTag);

                            FixRepeatingFieldHelper.UpdateGroup(msgFields, orderdedRepeatingFields, toSymbol, toTag);
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
                Logger.HandleException(new Exception("DLL Update rule could not be get applied for FIX message: " + msg.ToString()), LoggingConstants.POLICY_LOGANDTHROW);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private string GetToSymbol(string fromSymbol)
        {
            try
            {
                string basicCode = fromSymbol;
                string fromSeperator = GetfromSeperator(fromSymbol);

                int fromIndex = 0;
                fromIndex = fromSymbol.LastIndexOf(fromSeperator);
                if (fromSeperator != string.Empty)
                    basicCode = fromSymbol.Substring(0, fromIndex);

                string fromSuffix = fromSymbol.Substring(fromIndex + 1, fromSymbol.Length - fromIndex - 1);
                string counterSuffix = GettoSuffix(fromSuffix);
                if (counterSuffix != "N/A")
                {
                    string toSymbol = basicCode + counterSuffix;
                    toSymbol = toSymbol.Trim();
                    if (StartFromNonZero)
                        toSymbol = RemoveInitalZeros(toSymbol);
                    return toSymbol;
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
            return string.Empty;
        }

        private string GetfromSeperator(string symbol)
        {
            int index = 0;
            foreach (string fromSep in fromSeperators)
            {
                if (symbol.Contains(fromSep))
                {
                    if (symbol.Contains(fromSep + fromSuffixs[index]))
                        return fromSep;
                }
                index++;
            }
            return string.Empty;
        }

        private string RemoveInitalZeros(string value)
        {
            while (true)
            {
                int indexOfZero = value.IndexOf("0");
                if (indexOfZero == 0)
                {
                    value = value.Substring(indexOfZero + 1, value.Length - indexOfZero - 1);
                }
                else
                {
                    break;
                }
            }
            return value;
        }

        private string GettoSuffix(string fromSuffix)
        {
            int index = fromSuffixs.IndexOf(fromSuffix);
            if (index == -1)
                return "N/A";
            return toSeperators[index] + toSuffixs[index];
        }

        public override bool CreateRules(System.Xml.XmlNode xmlNodeItem)
        {
            try
            {
                if (xmlNodeItem.Attributes["fromSuffixs"] != null)
                {
                    string[] fromSuffixColl = xmlNodeItem.Attributes["fromSuffixs"].Value.Split(',');
                    foreach (string fromSuff in fromSuffixColl)
                    {
                        fromSuffixs.Add(fromSuff);
                    }
                }
                if (xmlNodeItem.Attributes["toSuffixs"] != null)
                {
                    string[] toSuffixColl = xmlNodeItem.Attributes["toSuffixs"].Value.Split(',');
                    foreach (string countrySuffix in toSuffixColl)
                    {
                        toSuffixs.Add(countrySuffix);
                    }
                }
                fromSeperators = xmlNodeItem.Attributes["fromSeperators"].Value.Split(',');
                toSeperators = xmlNodeItem.Attributes["toSeperators"].Value.Split(',');
                fromTag = xmlNodeItem.Attributes["FromTag"].Value;
                toTag = xmlNodeItem.Attributes["ToTag"].Value;
                if (xmlNodeItem.Attributes["StartFromNonZero"] != null)
                    StartFromNonZero = Convert.ToBoolean(xmlNodeItem.Attributes["StartFromNonZero"].Value);
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
            return true;
        }
    }
    internal enum Direction
    {
        In = 0,
        Out = 1
    }
}
