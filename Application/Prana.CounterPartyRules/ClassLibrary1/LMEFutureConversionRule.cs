using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.CounterPartyRules
{
    class LMEFutureConversionRule : DllBaseRule
    {
        string _conditionString = string.Empty;

        public override void ApplyRule(Prana.BusinessObjects.FIX.PranaMessage msg)
        {
            string monthYear = string.Empty;
            string monthStr = string.Empty;
            string firstHalf = string.Empty;
            string secondHalf = string.Empty;

            int day = int.MinValue;
            int month = int.MinValue;
            int year = int.MinValue;

            //if invalid maturity date is recieved from FIX
            bool isValidExpiryDate = false;
            DateTime expiryDate = DateTimeConstants.MinValue;

            try
            {
                if (!msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSecurityType))
                {
                    //Its definitely not an Future trade so get out of the rule
                    return;
                }

                if (!string.IsNullOrEmpty(_conditionString))
                {
                    string[] str = _conditionString.Split(new char[] { '=' });
                    if (str[1].Contains("LME") && str[2] == "FUT")
                    {
                        if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagMaturityMonthYear))
                        {
                            monthYear = msg.FIXMessage.ExternalInformation[FIXConstants.TagMaturityMonthYear].Value;
                            monthStr = monthYear.Trim().Substring(4, 2);
                            month = int.Parse(monthStr);
                            year = int.Parse(monthYear.Trim().Substring(0, 4));
                            if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagMaturityDay))
                            {
                                day = int.Parse(msg.FIXMessage.ExternalInformation[FIXConstants.TagMaturityDay].Value.Trim().Substring(6, 2));
                            }
                            else
                            {
                                string[] splittedSymbol = msg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value.Split(new char[] { ' ' });
                                day = int.Parse(splittedSymbol[1].Trim().Substring(2, 2));
                            }
                        }

                        if (year >= DateTime.MinValue.Year && year <= DateTime.MaxValue.Year && month >= DateTime.MinValue.Month && month <= DateTime.MaxValue.Month && day >= DateTime.MinValue.Day && day <= DateTime.MaxValue.Day)
                        {
                            isValidExpiryDate = true;
                        }
                        if (isValidExpiryDate)
                        {
                            expiryDate = new DateTime(year, month, day);
                        }
                        else
                        {
                            monthYear = "180001"; //set to minimum value
                            monthStr = "01";
                            month = 1;
                            day = 1;  //set to minimum value
                        }

                        DateTime standardExpiry = Prana.Utilities.DateTimeUtilities.DateTimeHelper.GetNthWeekDay(3, DayOfWeek.Wednesday, year, month);
                        if (expiryDate == standardExpiry)
                        {
                            string[] splittedSymbol = msg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value.Split(new char[] { ' ' });
                            firstHalf = splittedSymbol[0];
                            secondHalf = splittedSymbol[1].Remove(2, 2);
                            msg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value = firstHalf + " " + secondHalf;
                        }
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
                if (xmlNodeItem.Attributes["ConditionString"] != null)
                    _conditionString = xmlNodeItem.Attributes["ConditionString"].Value;
            }
            catch (Exception ex)
            {
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
}

