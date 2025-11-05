using Prana.BusinessObjects.FIX;
using System.Collections.Generic;

namespace Prana.CounterPartyRules
{
    public abstract class DllBaseRule
    {
        public abstract void ApplyRule(PranaMessage msg);

        public abstract void ApplyRule(PranaMessage msg, List<RepeatingMessageFieldCollection> repeatingFieldList, string repeatingTag);

        public virtual void ApplyRule(List<PranaMessage> msgList)
        {
            return;
        }
        public abstract bool CreateRules(System.Xml.XmlNode xmlNodeItem);
    }
}
