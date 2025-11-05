using Prana.BusinessObjects.LiveFeed;

namespace Prana.BusinessObjects
{
    public class SymbolPriceRequest
    {
        public int accountId { get; set; }

        public string Symbol { get; set; }

        public int AssetId { get; set; }

        public int ExchangeId { get; set; }

        public PricingDataType PriceFieldType { get; set; }

        public string secondaryPricingSource { get; set; }
    }
}
