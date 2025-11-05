using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.CounterPartyRules
{
    public class CacheFillerRule : DllBaseRule
    {
        string _tag = "";

        public override void ApplyRule(Prana.BusinessObjects.FIX.PranaMessage msg)
        {
            try
            {
                switch (_tag)
                {
                    case CustomFIXConstants.CUST_TAG_SettlementCurrencyName:
                        if (msg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_SettlementCurrencyName) && !string.IsNullOrWhiteSpace(msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_SettlementCurrencyName].Value))
                        {
                            int SettleCurrencyID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyID(msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_SettlementCurrencyName].Value);
                            if (SettleCurrencyID > 0)
                                msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_SettlementCurrencyID, SettleCurrencyID.ToString());
                        }
                        break;
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
            _tag = xmlNodeItem.Attributes["Tag"].Value;
            return true;
        }

        public override void ApplyRule(PranaMessage msg, List<RepeatingMessageFieldCollection> repeatingMessageFields, string repeatingTag)
        {
            return;
        }
    }
}
