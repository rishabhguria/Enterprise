using System.Collections.Generic;

namespace Prana.Admin.BLL
{
    public class PricingRuleItem
    {
        public List<object> accountID;
        public List<object> assetID;
        public List<object> exchangeID;
        int pricingDataType;
        int sourceID;
        int secondarySourceID;
        //int companyID;

        public PricingRuleItem()
        {
            accountID = new List<object>();
            assetID = new List<object>();
            exchangeID = new List<object>();
            PricingDataType = 0;
            SourceID = 0;
            SecondarySourceID = 0;
        }


        public int PricingDataType
        {
            get { return pricingDataType; }
            set { pricingDataType = value; }
        }

        public int SourceID
        {
            get { return sourceID; }
            set { sourceID = value; }
        }

        public int SecondarySourceID
        {
            get { return secondarySourceID; }
            set { secondarySourceID = value; }
        }


        public int RuleType { get; set; }

        public int RuleTypeByTime { get; set; }

        public bool IsPricingPolicy { get; set; }

        public int PricingPolicyID { get; set; }
    }
}
