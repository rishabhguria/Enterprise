using System.Collections.Generic;

namespace Prana.BusinessObjects.Classes.RebalancerNew
{
    public class RebalancerData
    {
        private List<RebalancerDto> rebalancerDtos = new List<RebalancerDto>();

        public List<RebalancerDto> RebalancerDtos
        {
            get { return rebalancerDtos; }
            set { rebalancerDtos = value; }
        }

        private List<AccountLevelNAV> accountWiseNAV = new List<AccountLevelNAV>();

        public List<AccountLevelNAV> AccountWiseNAV
        {
            get { return accountWiseNAV; }
            set { accountWiseNAV = value; }
        }

        private Dictionary<string, PriceAndFx> symbolWisePriceAndFx = new Dictionary<string, PriceAndFx>();

        public Dictionary<string, PriceAndFx> SymbolWisePriceAndFx
        {
            get { return symbolWisePriceAndFx; }
            set { symbolWisePriceAndFx = value; }
        }

        private Dictionary<string, decimal> symbolWiseYesterDayPrice = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> SymbolWiseYesterDayPrice
        {
            get { return symbolWiseYesterDayPrice; }
            set { symbolWiseYesterDayPrice = value; }
        }
    }
}
