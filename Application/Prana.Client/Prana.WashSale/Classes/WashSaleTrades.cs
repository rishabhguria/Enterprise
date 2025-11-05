using System;

namespace Prana.WashSale.Classes
{
    public class WashSaleTrades
    {
        #region Properties
        public string TaxlotID { get; set; }
        public string TypeOfTransaction { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime OriginalPurchaseDate { get; set; }
        public string Account { get; set; }
        public string Side { get; set; }
        public string Asset { get; set; }
        public string Currency { get; set; }
        public string Broker { get; set; }
        public string Symbol { get; set; }
        public string BloombergSymbol { get; set; }
        public string CUSIP { get; set; }
        public string Issuer { get; set; }
        public string UnderlyingSymbol { get; set; }
        public double Quantity { get; set; }
        public double UnitCostLocal { get; set; }
        public double TotalCostLocal { get; set; }
        public double TotalCost { get; set; }
        public decimal? WashSaleAdjustedRealizedLoss { get; set; }
        public int? WashSaleAdjustedHoldingsPeriod { get; set; }
        public decimal? WashSaleAdjustedCostBasis { get; set; }
        public DateTime? WashSaleAdjustedHoldingsStartDate { get; set; }


        #endregion

    }
}
