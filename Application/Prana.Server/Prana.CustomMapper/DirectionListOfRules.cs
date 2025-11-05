using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using System.Collections.Generic;

namespace Prana.CustomMapper
{
    public class DirectionListOfRules
    {
        Dictionary<int, List<RuleParameters>> _directionList = new Dictionary<int, List<RuleParameters>>();
        public DirectionListOfRules()
        {
            List<RuleParameters> listIn = new List<RuleParameters>();
            _directionList.Add((int)Direction.In, listIn);
            List<RuleParameters> listOut = new List<RuleParameters>();
            _directionList.Add((int)Direction.Out, listOut);
        }
        public void Add(RuleParameters ruleParameters)
        {
            _directionList[(int)ruleParameters.Direction].Add(ruleParameters);
        }
        public void ApplyRule(Direction direction, PranaMessage pranaMsg)
        {
            bool isReProcessed = pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsProcessed) ? true : false;
            List<RuleParameters> ruleParameters = _directionList[(int)direction];
            foreach (RuleParameters ruleParameter in ruleParameters)
            {
                if (!isReProcessed || (isReProcessed && ruleParameter.ExecuteOnReloadRule == 1))
                    ruleParameter.SetValues(pranaMsg);
            }
        }

        public void ApplyRule(Direction direction, List<PranaMessage> pranaMsgList)
        {
            List<RuleParameters> ruleParameters = _directionList[(int)direction];
            foreach (RuleParameters ruleParameter in ruleParameters)
            {
                ruleParameter.SetValues(pranaMsgList);
            }
        }
    }
}
