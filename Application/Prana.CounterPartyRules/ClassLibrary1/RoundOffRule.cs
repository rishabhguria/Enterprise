using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.CounterPartyRules
{
    class RoundOffRule : DllBaseRule
    {
        string _tag = string.Empty;
        int _roundOffUpto = int.MinValue;
        public override void ApplyRule(Prana.BusinessObjects.FIX.PranaMessage msg)
        {
            try
            {
                if (msg.FIXMessage.ExternalInformation.ContainsKey(_tag))
                {
                    decimal tagValue = Convert.ToDecimal(msg.FIXMessage.ExternalInformation[_tag].Value);
                    msg.FIXMessage.ExternalInformation[_tag].Value = (Math.Round(tagValue, _roundOffUpto)).ToString();
                }
                else if (msg.FIXMessage.InternalInformation.ContainsKey(_tag))
                {
                    decimal tagValue = Convert.ToDecimal(msg.FIXMessage.InternalInformation[_tag].Value);
                    msg.FIXMessage.InternalInformation[_tag].Value = (Math.Round(tagValue, _roundOffUpto)).ToString();
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

        public override void ApplyRule(PranaMessage msg1, List<RepeatingMessageFieldCollection> msg, string repeatingTag)
        {
            try
            {
                foreach (RepeatingMessageFieldCollection msgFields in msg)
                {
                    if (msgFields.ContainsKey(_tag))
                    {
                        decimal tagValue = Convert.ToDecimal(msgFields[_tag].Value);
                        msgFields[_tag].Value = (Math.Round(tagValue, _roundOffUpto)).ToString();
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
                if (xmlNodeItem.Attributes["Tag"] != null)
                    _tag = xmlNodeItem.Attributes["Tag"].Value;
                if (xmlNodeItem.Attributes["RoundOffUpto"] != null)
                    _roundOffUpto = Convert.ToInt16(xmlNodeItem.Attributes["RoundOffUpto"].Value);
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
    }
}
