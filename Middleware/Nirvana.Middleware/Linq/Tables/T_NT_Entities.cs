using System;

namespace Nirvana.Middleware.Linq
{
    [Serializable]
    public class T_NT_GenericPNL
    {
        public int AcctId { get; set; }
        public string AcctName { get; set; }
        public System.DateTime RunDate { get; set; }
        public string Symbol { get; set; }
        public string UnderlyingSymbol { get; set; }
        public System.DateTime TradeDate { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public string Open_CloseTag { get; set; }
        public string UdaSector { get; set; }
        public string UdaSubSector { get; set; }
        public string UdaCountry { get; set; }
        public string Strategy { get; set; }
        public string SymbolDescription { get; set; }
        public string UnderlyingSymbolDescription { get; set; }
        public string BloombergSymbol { get; set; }
        public string PutOrCall { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public double StrikePrice { get; set; }
        public double Delta { get; set; }
        public double Beta { get; set; }
        public Nullable<double> ImpliedVol { get; set; }
        public string Asset { get; set; }
        public string SetupAsset { get; set; }
        public bool CommissionAndFees { get; set; }
        public bool FXPNL { get; set; }
        public double PriceMultiplier { get; set; }
        public bool DeltaAdjPosMultiplier { get; set; }
        public int ZeroOrEndingMVOrUnrealized { get; set; }
        public bool CouponRate { get; set; }
        public bool BlackScholesOrBlack76 { get; set; }
        public string Side { get; set; }
        public string TradeCurrency { get; set; }
        public double OpeningFXRate { get; set; }
        public double BeginningFXRate { get; set; }
        public double EndingFXRate { get; set; }
        public double TotalCostLocal { get; set; }
        public double BeginningMarketValueLocal { get; set; }
        public double EndingMarketValueLocal { get; set; }
        public double TotalOpenCommissionAndFeesLocal { get; set; }
        public double TotalClosedCommissionAndFeesLocal { get; set; }
        public double DividendLocal { get; set; }
        public double BeginningQuantity { get; set; }
        public double EndingQuantity { get; set; }
        public double Quantity { get; set; }
        public double UnitCostLocal { get; set; }
        public double BeginningPriceLocal { get; set; }
        public double ClosingPriceLocal { get; set; }
        public double EndingPriceLocal { get; set; }
        public double UnderlyingSymbolPriceLocal { get; set; }
        public double SideMultiplier { get; set; }
        public double Multiplier { get; set; }
        public double UnderlyingDelta { get; set; }
        public string TaxlotID { get; set; }
        public Nullable<System.Guid> TaxlotClosingID { get; set; }
        public double DaySplitFactor { get; set; }
        public double TillSplitFactor { get; set; }
        public Nullable<double> K0 { get; set; }
        public Nullable<double> K1 { get; set; }
        public Nullable<double> K2 { get; set; }
        public Nullable<double> AveDaystoLiquidate { get; set; }
        public Nullable<double> DaysToLiquidate { get; set; }
        public Nullable<double> AveNDaysTradingVolume { get; set; }
        public Nullable<double> AveNDaysTradingValueLocal { get; set; }
        public double BeginningDelta { get; set; }
        public double BeginningUnderlyingSymbolPriceLocal { get; set; }
    }

    public class T_NT_GenericPNL_Insert : T_NT_GenericPNL
    {
        public byte[] CheckSumId { get; set; }
    }

    [Serializable]
    public class T_NT_Transaction
    {
        public int AcctId { get; set; }
        public string AcctName { get; set; }
        public System.DateTime RunDate { get; set; }
        public string Symbol { get; set; }
        public string UnderlyingSymbol { get; set; }
        public string Open_CloseTag { get; set; }
        public double AvgPrice { get; set; }
        public double Quantity { get; set; }
        public string Side { get; set; }
        public string UdaSector { get; set; }
        public string UdaSubSector { get; set; }
        public string UdaCountry { get; set; }
        public string Strategy { get; set; }
        public string SymbolDescription { get; set; }
        public string UnderlyingSymbolDescription { get; set; }
        public string Asset { get; set; }
        public string SetupAsset { get; set; }
        public bool CommissionAndFees { get; set; }
        public bool FXPNL { get; set; }
        public double PriceMultiplier { get; set; }
        public bool DeltaAdjPosMultiplier { get; set; }
        public int ZeroOrEndingMVOrUnrealized { get; set; }
        public string TradeCurrency { get; set; }
        public double NetAmountBase { get; set; }
        public double Dividend { get; set; }
        public string BloombergSymbol { get; set; }
        public string PutOrCall { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public bool CouponRate { get; set; }
        public bool BlackScholesOrBlack76 { get; set; }
        public string GroupID { get; set; }
        public string TransactionType { get; set; }
    }

    public class T_NT_Transaction_Insert : T_NT_Transaction
    {
        public byte[] CheckSumId { get; set; }
    }

    [Serializable]
    public class T_NT_CashAccruals
    {
        public int AcctId { get; set; }
        public string AcctName { get; set; }
        public System.DateTime RunDate { get; set; }
        public bool CashOrAccruals { get; set; }
        public string TradeCurrency { get; set; }
        public double BeginningMarketValueLocal { get; set; }
        public double EndingMarketValueLocal { get; set; }
        public Nullable<double> BeginningFXRate { get; set; }
        public Nullable<double> EndingFXRate { get; set; }
    }

    public class T_NT_CashAccruals_Insert : T_NT_CashAccruals
    {
        public byte[] CheckSumId { get; set; }
    }
}
