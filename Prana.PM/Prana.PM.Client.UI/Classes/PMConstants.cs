using Prana.CommonDataCache;
using System;
using System.Collections.Generic;

namespace Prana.PM.Client.UI
{
    public static class PMConstants
    {
        public static string doubleMinValString = double.MinValue.ToString();
        public static string doubleEpsilonValString = Double.Epsilon.ToString();
        public static string intMinValString = int.MinValue.ToString();
        public const string SUMMARY_MULTIPLE = "Multiple";
        public const string SUMMARY_UNDEFINED = "Undefined";
        public const string SUMMARY_DASH = "-";

        #region Formatting of columns
        public const string FORMAT_PNL_EXPOSURE_CASH_NEW = "#,##,##0";
        public const string FORMAT_ZERO_DECIMAL_DIGITS = "#,##,###0";
        public const string FORMAT_ONE_DECIMAL_DIGITS = "#,##,###0.0";
        public const string FORMAT_TWO_DECIMAL_DIGITS = "#,##,###0.00";
        public const string FORMAT_THREE_DECIMAL_DIGITS = "#,##,###0.000";
        public const string FORMAT_FOUR_DECIMAL_DIGITS = "#,##,###0.0000";
        public const string FORMAT_FIVE_DECIMAL_DIGITS = "#,##,###0.00000";
        public const string FORMAT_SIX_DECIMAL_DIGITS = "#,##,###0.000000";
        public const string FORMAT_SEVEN_DECIMAL_DIGITS = "#,##,###0.0000000";
        public const string FORMAT_EIGHT_DECIMAL_DIGITS = "#,##,###0.00000000";
        public const string FORMAT_NINE_DECIMAL_DIGITS = "#,##,###0.000000000";
        public const string FORMAT_TEN_DECIMAL_DIGITS = "#,##,###0.0000000000";
        public const string FORMAT_ELEVEN_DECIMAL_DIGITS = "#,##,###0.00000000000";
        public const string FORMAT_TWELVE_DECIMAL_DIGITS = "#,##,###0.000000000000";

        public const string FORMAT_ZERO_DECIMAL_DIGITS_MILLIONS = "#,##0,,";
        public const string FORMAT_ONE_DECIMAL_DIGITS_MILLIONS = "#,##0,,.0";
        public const string FORMAT_TWO_DECIMAL_DIGITS_MILLIONS = "#,##0,,.00";
        public const string FORMAT_THREE_DECIMAL_DIGITS_MILLIONS = "#,##0,,.000";
        public const string FORMAT_FOUR_DECIMAL_DIGITS_MILLIONS = "#,##0,,.0000";
        public const string FORMAT_FIVE_DECIMAL_DIGITS_MILLIONS = "#,##0,,.00000";
        public const string FORMAT_SIX_DECIMAL_DIGITS_MILLIONS = "#,##0,,.000000";
        public const string FORMAT_SEVEN_DECIMAL_DIGITS_MILLIONS = "#,##0,,.0000000";
        public const string FORMAT_EIGHT_DECIMAL_DIGITS_MILLIONS = "#,##0,,.00000000";
        public const string FORMAT_NINE_DECIMAL_DIGITS_MILLIONS = "#,##0,,.000000000";
        public const string FORMAT_TEN_DECIMAL_DIGITS_MILLIONS = "#,##0,,.0000000000";
        public const string FORMAT_ELEVEN_DECIMAL_DIGITS_MILLIONS = "#,##0,,.0000000000";
        public const string FORMAT_TWELVE_DECIMAL_DIGITS_MILLIONS = "#,##0,,.000000000000";

        public const string FORMAT_ZERO_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0;(#,##,###0);#,##,###0";
        public const string FORMAT_ONE_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.0;(#,##,###0.0);#,##,###0.0";
        public const string FORMAT_TWO_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.00;(#,##,###0.00);#,##,###0.00";
        public const string FORMAT_THREE_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.000;(#,##,###0.000);#,##,###0.000";
        public const string FORMAT_FOUR_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.0000;(#,##,###0.0000);#,##,###0.0000";
        public const string FORMAT_FIVE_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.00000;(#,##,###0.00000);#,##,###0.00000";
        public const string FORMAT_SIX_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.000000;(#,##,###0.000000);#,##,###0.000000";
        public const string FORMAT_SEVEN_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.0000000;(#,##,###0.0000000);#,##,###0.0000000";
        public const string FORMAT_EIGHT_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.00000000;(#,##,###0.00000000);#,##,###0.00000000";
        public const string FORMAT_NINE_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.000000000;(#,##,###0.000000000);#,##,###0.000000000";
        public const string FORMAT_TEN_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.0000000000;(#,##,###0.0000000000);#,##,###0.0000000000";
        public const string FORMAT_ELEVEN_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.00000000000;(#,##,###0.00000000000);#,##,###0.00000000000";
        public const string FORMAT_TWELVE_DECIMAL_DIGITS_WITH_BRACKETS = "#,##,###0.000000000000;(#,##,###0.000000000000);#,##,###0.000000000000";
        #endregion Formatting of columns

        #region Grid Column Names
        public const string COL_Analyst = "Analyst";
        public const string COL_AskPrice = "AskPrice";
        public const string COL_Asset = "Asset";
        public const string COL_AverageVolume20Day = "AverageVolume20Day";
        public const string COL_AverageVolume20DayUnderlyingSymbol = "AverageVolume20DayUnderlyingSymbol";
        public const string COL_AvgPrice = "AvgPrice";
        public const string COL_Beta = "Beta";
        public const string COL_BetaAdjExposure = "BetaAdjExposure";
        public const string COL_BetaAdjExposureInBaseCurrency = "BetaAdjExposureInBaseCurrency";
        public const string COL_BetaAdjGrossExposure = "BetaAdjGrossExposure";
        public const string COL_BetaAdjGrossExposureUnderlying = "BetaAdjGrossExposureUnderlying";
        public const string COL_BetaAdjGrossExposureUnderlyingInBaseCurrency = "BetaAdjGrossExposureUnderlyingInBaseCurrency";
        public const string COL_BidPrice = "BidPrice";
        public const string COL_BloombergSymbol = "BloombergSymbol";
        public const string COL_BloombergSymbolWithExchangeCode = "BloombergSymbolWithExchangeCode";
        public const string COL_FactSetSymbol = "FactSetSymbol";
        public const string COL_ActivSymbol = "ActivSymbol";
        public const string COL_CashImpact = "CashImpact";
        public const string COL_CashImpactInBaseCurrency = "CashImpactInBaseCurrency";
        public const string COL_ChangeInUnderlyingPrice = "ChangeInUnderlyingPrice";
        public const string COL_ClosingPrice = "ClosingPrice";
        public const string COL_ContractType = "ContractType";
        public const string COL_CostBasisBreakEven = "CostBasisBreakEven";
        public const string COL_CostBasisUnRealizedPNL = "CostBasisUnrealizedPnL";
        public const string COL_CostBasisUnrealizedPnLInBaseCurrency = "CostBasisUnrealizedPnLInBaseCurrency";
        public const string COL_CounterPartyName = "CounterPartyName";
        public const string COL_CountryOfRisk = "CountryOfRisk";
        public const string COL_CurrencySymbol = "CurrencySymbol";
        public const string COL_CUSIPSymbol = "CusipSymbol";
        public const string COL_CustomUDA1 = "CustomUDA1";
        public const string COL_CustomUDA2 = "CustomUDA2";
        public const string COL_CustomUDA3 = "CustomUDA3";
        public const string COL_CustomUDA4 = "CustomUDA4";
        public const string COL_CustomUDA5 = "CustomUDA5";
        public const string COL_CustomUDA6 = "CustomUDA6";
        public const string COL_CustomUDA7 = "CustomUDA7";
        public const string COL_CustomUDA8 = "CustomUDA8";
        public const string COL_CustomUDA9 = "CustomUDA9";
        public const string COL_CustomUDA10 = "CustomUDA10";
        public const string COL_CustomUDA11 = "CustomUDA11";
        public const string COL_CustomUDA12 = "CustomUDA12";
        public const string COL_DataSourceNameIDValue = "DataSourceNameIDValue";
        public const string COL_DayInterest = "DayInterest";
        public const string COL_DayPnL = "DayPnL";
        public const string COL_DayPnLInBaseCurrency = "DayPnLInBaseCurrency";
        public const string COL_DayReturn = "DayReturn";
        public const string COL_Delta = "Delta";
        public const string COL_DeltaAdjPosition = "DeltaAdjPosition";
        public const string COL_DeltaAdjPositionLME = "DeltaAdjPositionLME";
        public const string COL_DeltaSource = "DeltaSource";
        public const string COL_DividendYield = "DividendYield";
        public const string COL_EarnedDividendBase = "EarnedDividendBase";
        public const string COL_EarnedDividendLocal = "EarnedDividendLocal";
        public const string COL_Exchange = "Exchange";
        public const string COL_ExDividendDate = "ExDividendDate";
        public const string COL_ExpirationDate = "ExpirationDate";
        public const string COL_ExpirationMonth = "ExpirationMonth";
        public const string COL_Exposure = "Exposure";
        public const string COL_ExposureBPInBaseCurrency = "ExposureBPInBaseCurrency";
        public const string COL_ExposureInBaseCurrency = "ExposureInBaseCurrency";
        public const string COL_ForwardPoints = "ForwardPoints";
        public const string COL_FullSecurityName = "FullSecurityName";
        public const string COL_FxCostBasisPnl = "FxCostBasisPnl";
        public const string COL_FxDayPnl = "FxDayPnl";
        public const string COL_FXRate = "FxRate";
        public const string COL_FXRateDisplay = "FxRateDisplay";
        public const string COL_FXRateOnTradeDateStr = "FXRateOnTradeDateStr";
        public const string COL_GrossExposure = "GrossExposure";
        public const string COL_GrossExposureLocal = "GrossExposureLocal";
        public const string COL_GrossMarketValue = "GrossMarketValue";
        public const string COL_IDCOSymbol = "IdcoSymbol";
        public const string COL_InternalComments = "InternalComments";
        public const string COL_ISINSymbol = "IsinSymbol";
        public const string COL_IsStaleData = "IsStaleData";
        public const string COL_Issuer = "Issuer";
        public const string COL_LastPrice = "LastPrice";
        public const string COL_LastUpdatedUTC = "LastUpdatedUTC";
        public const string COL_LeadCurrencySymbol = "LeadCurrencySymbol";
        public const string COL_LeveragedFactor = "LeveragedFactor";
        public const string COL_LiquidTag = "LiquidTag";
        public const string COL_MarketCap = "MarketCap";
        public const string COL_MarketCapitalization = "MarketCapitalization";
        public const string COL_MarketValue = "MarketValue";
        public const string COL_MarketValueInBaseCurrency = "MarketValueInBaseCurrency";
        public const string COL_MasterFund = "MasterFund";
        public const string COL_MasterStrategy = "MasterStrategy";
        public const string COL_MidPrice = "MidPrice";
        public const string COL_Multiplier = "Multiplier";
        public const string COL_NAV = "NAV";
        public const string COL_NavTouch = "NavTouch";
        public const string COL_NetExposure = "NetExposure";
        public const string COL_NetExposureInBaseCurrency = "NetExposureInBaseCurrency";
        public const string COL_NetNotionalForCostBasisBreakEven = "NetNotionalForCostBasisBreakEven";
        public const string COL_OrderSideTagValue = "OrderSideTagValue";
        public const string COL_OSISymbol = "OsiSymbol";
        public const string COL_PercentageAverageVolume = "PercentageAverageVolume";
        public const string COL_PercentageAverageVolumeDeltaAdjusted = "PercentageAverageVolumeDeltaAdjusted";
        public const string COL_PercentageChange = "PercentageChange";
        public const string COL_PercentageGainLoss = "PercentageGainLoss";
        public const string COL_PercentageGainLossCostBasis = "PercentageGainLossCostBasis";
        public const string COL_PercentagePNLContribution = "PercentagePNLContribution";
        public const string COL_PercentageUnderlyingChange = "PercentageUnderlyingChange";
        public const string COL_PercentBetaAdjGrossExposureInBaseCurrency = "PercentBetaAdjGrossExposureInBaseCurrency";
        public const string COL_PercentDayPnLGrossMV = "PercentDayPnLGrossMV";
        public const string COL_PercentDayPnLNetMV = "PercentDayPnLNetMV";
        public const string COL_PercentExposureInBaseCurrency = "PercentExposureInBaseCurrency";
        public const string COL_PercentGrossExposureInBaseCurrency = "PercentGrossExposureInBaseCurrency";
        public const string COL_PercentGrossMarketValueInBaseCurrency = "PercentGrossMarketValueInBaseCurrency";
        public const string COL_PercentNetExposureInBaseCurrency = "PercentNetExposureInBaseCurrency";
        public const string COL_PercentNetMarketValueInBaseCurrency = "PercentNetMarketValueInBaseCurrency";
        public const string COL_PercentUnderlyingGrossExposureInBaseCurrency = "PercentUnderlyingGrossExposureInBaseCurrency";
        public const string COL_PositionSideExposure = "PositionSideExposure";
        public const string COL_PositionSideExposureBoxed = "PositionSideExposureBoxed";
        public const string COL_PositionSideExposureUnderlying = "PositionSideExposureUnderlying";
        public const string COL_PositionSideMV = "PositionSideMV";
        public const string COL_Premium = "Premium";
        public const string COL_PremiumDollar = "PremiumDollar";
        public const string COL_PricingSource = "PricingSource";
        public const string COL_ProxySymbol = "ProxySymbol";
        public const string COL_Quantity = "Quantity";
        public const string COL_Region = "Region";
        public const string COL_ReutersSymbol = "ReutersSymbol";
        public const string COL_RiskCurrency = "RiskCurrency";
        public const string COL_SEDOLSymbol = "SedolSymbol";
        public const string COL_SelectedFeedPrice = "SelectedFeedPrice";
        public const string COL_SelectedFeedPriceInBaseCurrency = "SelectedFeedPriceInBaseCurrency";
        public const string COL_SettlementDate = "SettlementDate";
        public const string COL_SharesOutstanding = "SharesOutstanding";
        public const string COL_SideMultiplier = "SideMultiplier";
        public const string COL_SideName = "SideName";
        public const string COL_StartOfDayNAV = "StartOfDayNAV";
        public const string COL_StartTradeDate = "StartTradeDate";
        public const string COL_StrikeGapExposure = "StrikeGapExposure";
        public const string COL_StrikeGapRisk = "StrikeGapRisk";
        public const string COL_StrikePrice = "StrikePrice";
        public const string COL_Symbol = "Symbol";
        public const string COL_TotalInterest = "TotalInterest";
        public const string COL_TradeAttribute1 = "TradeAttribute1";
        public const string COL_TradeAttribute2 = "TradeAttribute2";
        public const string COL_TradeAttribute3 = "TradeAttribute3";
        public const string COL_TradeAttribute4 = "TradeAttribute4";
        public const string COL_TradeAttribute5 = "TradeAttribute5";
        public const string COL_TradeAttribute6 = "TradeAttribute6";
        public const string COL_TradeAttribute = "TradeAttribute";
        public const string COL_TradeCostBasisPnl = "TradeCostBasisPnl";
        public const string COL_TradeDate = "TradeDate";
        public const string COL_TradeDayPnl = "TradeDayPnl";
        public const string COL_TransactionSide = "TransactionSide";
        public const string COL_TransactionType = "TransactionType";
        public const string COL_UcitsEligibleTag = "UcitsEligibleTag";
        public const string COL_UDAAsset = "UDAAsset";
        public const string COL_UDACountry = "UDACountry";
        public const string COL_UDASector = "UDASector";
        public const string COL_UDASecurityType = "UDASecurityType";
        public const string COL_UDASubSector = "UDASubSector";
        public const string COL_Underlying = "Underlying";
        public const string COL_UnderlyingGrossExposure = "UnderlyingGrossExposure";
        public const string COL_UnderlyingGrossExposureInBaseCurrency = "UnderlyingGrossExposureInBaseCurrency";
        public const string COL_UnderlyingStockPrice = "UnderlyingStockPrice";
        public const string COL_UnderlyingSymbol = "UnderlyingSymbol";
        public const string COL_UnderlyingValueForOptions = "UnderlyingValueForOptions";
        public const string COL_UserName = "UserName";
        public const string COL_Volatility = "Volatility";
        public const string COL_VsCurrencySymbol = "VsCurrencySymbol";
        public const string COL_YesterdayFXRate = "YesterdayFXRate";
        public const string COL_YesterdayMarketValue = "YesterdayMarketValue";
        public const string COL_YesterdayMarketValueInBaseCurrency = "YesterdayMarketValueInBaseCurrency";
        public const string COL_YesterdayMarkPriceStr = "YesterdayMarkPriceStr";
        public const string COL_YesterdayUnderlyingMarkPriceStr = "YesterdayUnderlyingMarkPriceStr";
        public const string COL_NetNotionalValue = "NetNotionalValue";
        public const string COL_NetNotionalValueBase = "NetNotionalValueInBaseCurrency";
        public const string COL_CounterCurrencySymbol = "CounterCurrencySymbol";
        public const string COL_CounterCurrencyID = "CounterCurrencyID";
        public const string COL_CounterCurrencyAmount = "CounterCurrencyAmount";
        public const string COL_CounterCurrencyCostBasisPnL = "CounterCurrencyCostBasisPnL";
        public const string COL_CounterCurrencyDayPnL = "CounterCurrencyDayPnL";
        public const string COL_ItmOtm = "ItmOtm";
        public const string COL_PercentOfITMOTM = "PercentOfITMOTM";
        public const string COL_IntrinsicValue = "IntrinsicValue";
        public const string COL_DaysToExpiry = "DaysToExpiry";
        public const string COL_GainLossIfExerciseAssign = "GainLossIfExerciseAssign";
        public const string COL_DayTradedPosition = "DayTradedPosition";
        public const string COL_TradeVolume = "TradeVolume";
        public const string COL_PricingStatus = "PricingStatus";
        #endregion Grid Column Names

        #region Captions
        public const string CAP_Analyst = "Analyst";
        public const string CAP_AskPrice = "Px Ask";
        public const string CAP_Asset = "Asset Class";
        public const string CAP_AverageVolume20Day = "Avg Volume 20 Day";
        public const string CAP_AverageVolume20DayUnderlyingSymbol = "Avg Volume 20 Day (Underlying Symbol)";
        public const string CAP_AvgPrice = "Cost Basis";
        public const string CAP_Beta = "Beta";
        public const string CAP_BetaAdjExposure = "Beta Adj. Exposure (Local)";
        public const string CAP_BetaAdjExposureInBaseCurrency = "Beta Adj. Exposure (Base)";
        public const string CAP_BetaAdjGrossExposure = "Beta Adj. Gross Exposure (Base)";
        public const string CAP_BetaAdjGrossExposureUnderlying = "Beta Adj. Gross Exposure (Underlying) (Local)";
        public const string CAP_BetaAdjGrossExposureUnderlyingInBaseCurrency = "Beta Adj. Gross Exposure (Underlying) (Base)";
        public const string CAP_BidPrice = "Px Bid";
        public const string CAP_BloombergSymbol = "Bloomberg";
        public const string CAP_BloombergSymbolWithExchangeCode = "Bloomberg Symbol(with Exchange Code)";
        public const string CAP_FactSetSymbol = "FactSet Symbol";
        public const string CAP_ActivSymbol = "ACTIV Symbol";
        public const string CAP_CashImpact = "Cash Impact (Local)";
        public const string CAP_CashImpactInBaseCurrency = "Cash Impact (Base)";
        public const string CAP_ChangeInUnderlyingPrice = "Change In Underlying Price";
        public const string CAP_ClosingPrice = "Closing Price";
        public const string CAP_ContractType = "Put/Call";
        public const string CAP_CostBasisBreakEven = "Cost Basis (Break Even)";
        public const string CAP_CostBasisUnrealizedPnL = "Cost Basis P&L (Local)";
        public const string CAP_CostBasisUnrealizedPnLInBaseCurrency = "Cost Basis P&L (Base)";
        public const string CAP_CounterPartyName = "Broker";
        public const string CAP_CountryOfRisk = "Country Of Risk";
        public const string CAP_CurrencySymbol = "Trade Currency";
        public const string CAP_CUSIPSymbol = "CUSIP";
        public const string CAP_CustomUDA1 = "Custom UDA1";
        public const string CAP_CustomUDA2 = "Custom UDA2";
        public const string CAP_CustomUDA3 = "Custom UDA3";
        public const string CAP_CustomUDA4 = "Custom UDA4";
        public const string CAP_CustomUDA5 = "Custom UDA5";
        public const string CAP_CustomUDA6 = "Custom UDA6";
        public const string CAP_CustomUDA7 = "Custom UDA7";
        public const string CAP_CustomUDA8 = "Custom UDA8";
        public const string CAP_CustomUDA9 = "Custom UDA9";
        public const string CAP_CustomUDA10 = "Custom UDA10";
        public const string CAP_CustomUDA11 = "Custom UDA11";
        public const string CAP_CustomUDA12 = "Custom UDA12";
        public const string CAP_DataSourceNameIDValue = "Prime Broker";
        public const string CAP_DayInterest = "Day Interest";
        public const string CAP_DayPnL = "Day P&L (Local)";
        public const string CAP_DayPnLInBaseCurrency = "Day P&L (Base)";
        public const string CAP_DayReturn = "Day Return";
        public const string CAP_Delta = "Delta";
        public const string CAP_DeltaAdjPosition = "Delta Adj. Position";
        public const string CAP_DeltaAdjPositionLME = "Delta Adj. Position (LME)";
        public const string CAP_DeltaSource = "Delta Source";
        public const string CAP_DividendYield = "Dividend Yield (%)";
        public const string CAP_EarnedDividendBase = "Earned Dividend (Base)";
        public const string CAP_EarnedDividendLocal = "Earned Dividend (Local)";
        public const string CAP_Exchange = "Exchange";
        public const string CAP_ExDividendDate = "Ex Dividend Date";
        public const string CAP_ExpirationDate = "Expiration Date";
        public const string CAP_ExpirationMonth = "Expiration Month";
        public const string CAP_Exposure = "Exposure (Local)";
        public const string CAP_ExposureBPInBaseCurrency = "Exposure (BP)(Base)";
        public const string CAP_ExposureInBaseCurrency = "Exposure (Base)";
        public const string CAP_ForwardPoints = "Forward Points";
        public const string CAP_FullSecurityName = "Security Name";
        public const string CAP_FxCostBasisPnl = "Cost Basis P&L (Base)(FX Gain)";
        public const string CAP_FxDayPnl = "Day P&L (Base)(FX Gain)";
        public const string CAP_FXRate = "FX Rate";
        public const string CAP_FXRateDisplay = "FX Rate (Display)";
        public const string CAP_FXRateOnTradeDateStr = "FX Rate On Trade Date";
        public const string CAP_GrossExposure = "Gross Exposure (Base)";
        public const string CAP_GrossExposureLocal = "Gross Exposure (Local)";
        public const string CAP_GrossMarketValue = "Gross Market Value (Base)";
        public const string CAP_IDCOSymbol = "IDCO";
        public const string CAP_InternalComments = "Internal Comments";
        public const string CAP_ISINSymbol = "ISIN";
        public const string CAP_IsStaleData = "Is Stale Data";
        public const string CAP_Issuer = "Issuer";
        public const string CAP_LastPrice = "Px Last";
        public const string CAP_LastUpdatedUTC = "Update Time (Last Price)";
        public const string CAP_LeadCurrencySymbol = "Lead Currency";
        public const string CAP_LeveragedFactor = "Leveraged Factor";
        public const string CAP_LiquidTag = "Liquid Tag";
        public const string CAP_MarketCap = "Market Cap UDA";
        public const string CAP_MarketCapitalization = "Market Cap";
        public const string CAP_MarketValue = "Market Value (Local)";
        public const string CAP_MarketValueInBaseCurrency = "Market Value (Base)";
        public const string CAP_MasterFund = "MasterFund";
        public const string CAP_CLIENT = "Client";
        public const string CAP_MasterStrategy = "Master Strategy";
        public const string CAP_MidPrice = "Px Mid";
        public const string CAP_Multiplier = "Multiplier";
        public const string CAP_NAV = "NAV";
        public const string CAP_NavTouch = "NAV (Touch)";
        public const string CAP_NetExposure = "Net Exposure (Local)";
        public const string CAP_NetExposureInBaseCurrency = "Net Exposure (Base)";
        public const string CAP_NetNotionalForCostBasisBreakEven = "Notional For Cost Basis (Break Even)";
        public const string CAP_OrderSideTagValue = "Order Side Tag Value";
        public const string CAP_OSISymbol = "OSI";
        public const string CAP_PercentageAverageVolume = "% Avg Volume";
        public const string CAP_PercentageAverageVolumeDeltaAdjusted = "% Avg Volume (Delta Adj.)";
        public const string CAP_PercentageChange = "% Change";
        public const string CAP_PercentageGainLoss = "% Gain/Loss Day";
        public const string CAP_PercentageGainLossCostBasis = "% Gain/Loss Cost";
        public const string CAP_PercentagePNLContribution = "% P&L Contribution";
        public const string CAP_PercentageUnderlyingChange = "% Change (Underlying Price)";
        public const string CAP_PercentBetaAdjGrossExposureInBaseCurrency = "% Beta Adj. Gross Exposure (Base)";
        public const string CAP_PercentDayPnLGrossMV = "% Day P&L (Gross MV)";
        public const string CAP_PercentDayPnLNetMV = "% Day P&L (Net MV)";
        public const string CAP_PercentExposureInBaseCurrency = "% Exposure (Base)";
        public const string CAP_PercentGrossExposureInBaseCurrency = "% Gross Exposure (Base)";
        public const string CAP_PercentGrossMarketValueInBaseCurrency = "% Gross Market Value (Base)";
        public const string CAP_PercentNetExposureInBaseCurrency = "% Net Exposure (Base)";
        public const string CAP_PercentNetMarketValueInBaseCurrency = "% Net Market Value (Base)";
        public const string CAP_PercentUnderlyingGrossExposureInBaseCurrency = "% Gross Exposure (Underlying) (Base)";
        public const string CAP_PositionSideExposure = "Position Side Exposure";
        public const string CAP_PositionSideExposureBoxed = "Position Side Exposure (Boxed)";
        public const string CAP_PositionSideExposureUnderlying = "Position Side Exposure (Underlying)";
        public const string CAP_PositionSideMV = "Position Side";
        public const string CAP_Premium = "Premium";
        public const string CAP_PremiumDollar = "Premium $";
        public const string CAP_PricingSource = "Pricing Source";
        public const string CAP_ProxySymbol = "Proxy Symbol";
        public const string CAP_Quantity = "Position";
        public const string CAP_Region = "Region";
        public const string CAP_ReutersSymbol = "RIC";
        public const string CAP_RiskCurrency = "Risk Currency";
        public const string CAP_SEDOLSymbol = "SEDOL";
        public const string CAP_SelectedFeedPrice = "Px Selected Feed (Local)";
        public const string CAP_SelectedFeedPriceInBaseCurrency = "Px Selected Feed (Base)";
        public const string CAP_SettlementDate = "Settlement Date";
        public const string CAP_SharesOutstanding = "Shares Outstanding (MM)";
        public const string CAP_SideMultiplier = "Side Multiplier";
        public const string CAP_SideName = "Order Side";
        public const string CAP_StartOfDayNAV = "Start of Day NAV";
        public const string CAP_StartTradeDate = "Start Trade Date";
        public const string CAP_StrikePrice = "Strike Price";
        public const string CAP_Symbol = "Symbol";
        public const string CAP_TotalInterest = "Total Interest";
        public const string CAP_TradeAttribute1 = "Trade Attribute 1";
        public const string CAP_TradeAttribute2 = "Trade Attribute 2";
        public const string CAP_TradeAttribute3 = "Trade Attribute 3";
        public const string CAP_TradeAttribute4 = "Trade Attribute 4";
        public const string CAP_TradeAttribute5 = "Trade Attribute 5";
        public const string CAP_TradeAttribute6 = "Trade Attribute 6";
        public const string CAP_TradeAttribute = "Trade Attribute ";
        public const string CAP_TradeCostBasisPnl = "Cost Basis P&L (Base)(Price Gain)";
        public const string CAP_TradeDate = "Trade Date";
        public const string CAP_TradeDayPnl = "Day P&L (Base)(Price Gain)";
        public const string CAP_TransactionSide = "Transaction Side";
        public const string CAP_TransactionType = "Transaction Type";
        public const string CAP_UcitsEligibleTag = "UCITS Eligible Tag";
        public const string CAP_UDAAsset = "User Asset";
        public const string CAP_UDACountry = "Country";
        public const string CAP_UDASector = "Sector";
        public const string CAP_UDASecurityType = "Security Type";
        public const string CAP_UDASubSector = "Sub Sector";
        public const string CAP_Underlying = "Underlying";
        public const string CAP_UnderlyingGrossExposure = "Gross Exposure (Underlying)";
        public const string CAP_UnderlyingGrossExposureInBaseCurrency = "Gross Exposure (Underlying) (Base)";
        public const string CAP_UnderlyingStockPrice = "Underlying Price";
        public const string CAP_UnderlyingSymbol = "Underlying Symbol";
        public const string CAP_UnderlyingValueForOptions = "Underlying Value (Options)";
        public const string CAP_UserName = "User";
        public const string CAP_Volatility = "Volatility (%)";
        public const string CAP_VsCurrencySymbol = "Vs Currency";
        public const string CAP_YesterdayFXRate = "Closing FX Rate";
        public const string CAP_YesterdayMarketValue = "Closing Market Value (Local)";
        public const string CAP_YesterdayMarketValueInBaseCurrency = "Closing Market Value (Base)";
        public const string CAP_YesterdayMarkPriceStr = "Closing Mark";
        public const string CAP_YesterdayUnderlyingMarkPriceStr = "Closing Mark (Underlying)";
        public const string CAP_NetNotionalValue = "Notional (Base)";
        public const string CAP_NetNotionalValueBase = "Notional (Local)";
        public const string CAP_CounterCurrencySymbol = "Counter Currency";
        public const string CAP_CounterCurrencyAmount = "Counter Currency Amount";
        public const string CAP_CounterCurrencyCostBasisPnL = "Cost Basis P&L (Counter Currency)";
        public const string CAP_CounterCurrencyDayPNL = "Day P&L (Counter Currency)";
        public const string CAP_ItmOtm = "ITM/OTM";
        public const string CAP_PercentOfITMOTM = "% of ITM/OTM";
        public const string CAP_IntrinsicValue = "Intrinsic value";
        public const string CAP_DaysToExpiry = "Days to Expiry";
        public const string CAP_GainLossIfExerciseAssign = "Gain/Loss if Ex./Assign";
        public const string CAP_DayTradedPosition = "Day Traded Position";
        public const string CAP_TradeVolume = "Total Volume";
        public const string CAP_PricingStatus = "Pricing Status";
        #endregion

        #region Dictionary column name from caption
        public static Dictionary<string, string> columnFromCaptionMappingDictionary = CreateColumnFromCaptionMappingDictionary();

        private static Dictionary<string, string> CreateColumnFromCaptionMappingDictionary()
        {
            Dictionary<string, string> columnFromCaptionMappingDictionary = new Dictionary<string, string>()
            {
                      {CAP_Analyst, COL_Analyst}
                     ,{CAP_AskPrice, COL_AskPrice}
                     ,{CAP_Asset, COL_Asset}
                     ,{CAP_AverageVolume20Day, COL_AverageVolume20Day}
                     ,{CAP_AverageVolume20DayUnderlyingSymbol, COL_AverageVolume20DayUnderlyingSymbol}
                     ,{CAP_AvgPrice, COL_AvgPrice}
                     ,{CAP_Beta, COL_Beta}
                     ,{CAP_BetaAdjExposure, COL_BetaAdjExposure}
                     ,{CAP_BetaAdjExposureInBaseCurrency, COL_BetaAdjExposureInBaseCurrency}
                     ,{CAP_BetaAdjGrossExposure, COL_BetaAdjGrossExposure}
                     ,{CAP_BetaAdjGrossExposureUnderlying, COL_BetaAdjGrossExposureUnderlying}
                     ,{CAP_BetaAdjGrossExposureUnderlyingInBaseCurrency, COL_BetaAdjGrossExposureUnderlyingInBaseCurrency}
                     ,{CAP_BidPrice, COL_BidPrice}
                     ,{CAP_BloombergSymbol, COL_BloombergSymbol}
                     ,{CAP_FactSetSymbol, COL_FactSetSymbol}
                     ,{CAP_ActivSymbol, COL_ActivSymbol}
                     ,{CAP_CashImpact, COL_CashImpact}
                     ,{CAP_CashImpactInBaseCurrency, COL_CashImpactInBaseCurrency}
                     ,{CAP_ChangeInUnderlyingPrice, COL_ChangeInUnderlyingPrice}
                     ,{CAP_ClosingPrice, COL_ClosingPrice}
                     ,{CAP_ContractType, COL_ContractType}
                     ,{CAP_CostBasisBreakEven, COL_CostBasisBreakEven}
                     ,{CAP_CostBasisUnrealizedPnL, COL_CostBasisUnRealizedPNL}
                     ,{CAP_CostBasisUnrealizedPnLInBaseCurrency, COL_CostBasisUnrealizedPnLInBaseCurrency}
                     ,{CAP_CounterPartyName, COL_CounterPartyName}
                     ,{CAP_CountryOfRisk, COL_CountryOfRisk}
                     ,{CAP_CurrencySymbol, COL_CurrencySymbol}
                     ,{CAP_CUSIPSymbol, COL_CUSIPSymbol}
                     ,{CAP_CustomUDA1, COL_CustomUDA1}
                     ,{CAP_CustomUDA2, COL_CustomUDA2}
                     ,{CAP_CustomUDA3, COL_CustomUDA3}
                     ,{CAP_CustomUDA4, COL_CustomUDA4}
                     ,{CAP_CustomUDA5, COL_CustomUDA5}
                     ,{CAP_CustomUDA6, COL_CustomUDA6}
                     ,{CAP_CustomUDA7, COL_CustomUDA7}
                     ,{CAP_CustomUDA8, COL_CustomUDA8}
                     ,{CAP_CustomUDA9, COL_CustomUDA9}
                     ,{CAP_CustomUDA10, COL_CustomUDA10}
                     ,{CAP_CustomUDA11, COL_CustomUDA11}
                     ,{CAP_CustomUDA12, COL_CustomUDA12}
                     ,{CAP_DayInterest, COL_DayInterest}
                     ,{CAP_DataSourceNameIDValue, COL_DataSourceNameIDValue}
                     ,{CAP_DayPnL, COL_DayPnL}
                     ,{CAP_DayPnLInBaseCurrency, COL_DayPnLInBaseCurrency}
                     ,{CAP_DayReturn, COL_DayReturn}
                     ,{CAP_Delta, COL_Delta}
                     ,{CAP_DeltaAdjPosition, COL_DeltaAdjPosition}
                     ,{CAP_DeltaAdjPositionLME, COL_DeltaAdjPositionLME}
                     ,{CAP_DeltaSource, COL_DeltaSource}
                     ,{CAP_DividendYield, COL_DividendYield}
                     ,{CAP_EarnedDividendBase, COL_EarnedDividendBase}
                     ,{CAP_EarnedDividendLocal, COL_EarnedDividendLocal}
                     ,{CAP_Exchange, COL_Exchange}
                     ,{CAP_ExDividendDate, COL_ExDividendDate}
                     ,{CAP_ExpirationDate, COL_ExpirationDate}
                     ,{CAP_ExpirationMonth, COL_ExpirationMonth}
                     ,{CAP_Exposure, COL_Exposure}
                     ,{CAP_ExposureBPInBaseCurrency, COL_ExposureBPInBaseCurrency}
                     ,{CAP_ExposureInBaseCurrency, COL_ExposureInBaseCurrency}
                     ,{CAP_ForwardPoints, COL_ForwardPoints}
                     ,{CAP_FullSecurityName, COL_FullSecurityName}
                     ,{CAP_FxCostBasisPnl, COL_FxCostBasisPnl}
                     ,{CAP_FxDayPnl, COL_FxDayPnl}
                     ,{CAP_FXRate, COL_FXRate}
                     ,{CAP_FXRateDisplay, COL_FXRateDisplay}
                     ,{CAP_FXRateOnTradeDateStr, COL_FXRateOnTradeDateStr}
                     ,{CAP_GrossExposure, COL_GrossExposure}
                     ,{CAP_GrossMarketValue, COL_GrossMarketValue}
                     ,{CAP_IDCOSymbol, COL_IDCOSymbol}
                     ,{CAP_InternalComments, COL_InternalComments}
                     ,{CAP_ISINSymbol, COL_ISINSymbol}
                     ,{CAP_Issuer, COL_Issuer}
                     ,{CAP_LastPrice, COL_LastPrice}
                     ,{CAP_LastUpdatedUTC, COL_LastUpdatedUTC}
                     ,{CAP_LeadCurrencySymbol, COL_LeadCurrencySymbol}
                     ,{CAP_LeveragedFactor, COL_LeveragedFactor}
                     ,{CAP_LiquidTag, COL_LiquidTag}
                     ,{CAP_MarketCap, COL_MarketCap}
                     ,{CAP_MarketCapitalization, COL_MarketCapitalization}
                     ,{CAP_MarketValue, COL_MarketValue}
                     ,{CAP_MarketValueInBaseCurrency, COL_MarketValueInBaseCurrency}
                     ,{ CachedDataManager.GetInstance.IsShowmasterFundAsClient() ? CAP_CLIENT :CAP_MasterFund, COL_MasterFund}
                     ,{CAP_MasterStrategy, COL_MasterStrategy}
                     ,{CAP_MidPrice, COL_MidPrice}
                     ,{CAP_Multiplier, COL_Multiplier}
                     ,{CAP_NAV, COL_NAV}
                     ,{CAP_NavTouch, COL_NavTouch}
                     ,{CAP_NetExposure, COL_NetExposure}
                     ,{CAP_NetExposureInBaseCurrency, COL_NetExposureInBaseCurrency}
                     ,{CAP_NetNotionalForCostBasisBreakEven, COL_NetNotionalForCostBasisBreakEven}
                     ,{CAP_NetNotionalValue, COL_NetNotionalValue}
                     ,{CAP_NetNotionalValueBase, COL_NetNotionalValueBase}
                     ,{CAP_OrderSideTagValue, COL_OrderSideTagValue}
                     ,{CAP_OSISymbol, COL_OSISymbol}
                     ,{CAP_PercentageAverageVolume, COL_PercentageAverageVolume}
                     ,{CAP_PercentageAverageVolumeDeltaAdjusted, COL_PercentageAverageVolumeDeltaAdjusted}
                     ,{CAP_PercentageChange, COL_PercentageChange}
                     ,{CAP_PercentageGainLoss, COL_PercentageGainLoss}
                     ,{CAP_PercentageGainLossCostBasis, COL_PercentageGainLossCostBasis}
                     ,{CAP_PercentagePNLContribution, COL_PercentagePNLContribution}
                     ,{CAP_PercentageUnderlyingChange, COL_PercentageUnderlyingChange}
                     ,{CAP_PercentBetaAdjGrossExposureInBaseCurrency, COL_PercentBetaAdjGrossExposureInBaseCurrency}
                     ,{CAP_PercentDayPnLGrossMV, COL_PercentDayPnLGrossMV}
                     ,{CAP_PercentDayPnLNetMV, COL_PercentDayPnLNetMV}
                     ,{CAP_PercentExposureInBaseCurrency, COL_PercentExposureInBaseCurrency}
                     ,{CAP_PercentGrossExposureInBaseCurrency, COL_PercentGrossExposureInBaseCurrency}
                     ,{CAP_PercentGrossMarketValueInBaseCurrency, COL_PercentGrossMarketValueInBaseCurrency}
                     ,{CAP_PercentNetExposureInBaseCurrency, COL_PercentNetExposureInBaseCurrency}
                     ,{CAP_PercentNetMarketValueInBaseCurrency, COL_PercentNetMarketValueInBaseCurrency}
                     ,{CAP_PercentUnderlyingGrossExposureInBaseCurrency, COL_PercentUnderlyingGrossExposureInBaseCurrency}
                     ,{CAP_PositionSideExposure, COL_PositionSideExposure}
                     ,{CAP_PositionSideExposureBoxed, COL_PositionSideExposureBoxed}
                     ,{CAP_PositionSideExposureUnderlying, COL_PositionSideExposureUnderlying}
                     ,{CAP_PositionSideMV, COL_PositionSideMV}
                     ,{CAP_Premium, COL_Premium}
                     ,{CAP_PremiumDollar, COL_PremiumDollar}
                     ,{CAP_PricingSource , COL_PricingSource}
                     ,{CAP_Quantity, COL_Quantity}
                     ,{CAP_Region, COL_Region}
                     ,{CAP_ReutersSymbol, COL_ReutersSymbol}
                     ,{CAP_RiskCurrency, COL_RiskCurrency}
                     ,{CAP_SEDOLSymbol, COL_SEDOLSymbol}
                     ,{CAP_SelectedFeedPrice, COL_SelectedFeedPrice}
                     ,{CAP_SelectedFeedPriceInBaseCurrency, COL_SelectedFeedPriceInBaseCurrency}
                     ,{CAP_SettlementDate, COL_SettlementDate}
                     ,{CAP_SharesOutstanding, COL_SharesOutstanding}
                     ,{CAP_SideMultiplier, COL_SideMultiplier}
                     ,{CAP_SideName, COL_SideName}
                     ,{CAP_StartOfDayNAV, COL_StartOfDayNAV}
                     ,{CAP_StartTradeDate, COL_StartTradeDate}
                     ,{CAP_StrikePrice, COL_StrikePrice}
                     ,{CAP_Symbol , COL_Symbol}
                     ,{CAP_TotalInterest, COL_TotalInterest}
                     ,{CAP_TradeAttribute1, COL_TradeAttribute1}
                     ,{CAP_TradeAttribute2, COL_TradeAttribute2}
                     ,{CAP_TradeAttribute3, COL_TradeAttribute3}
                     ,{CAP_TradeAttribute4, COL_TradeAttribute4}
                     ,{CAP_TradeAttribute5, COL_TradeAttribute5}
                     ,{CAP_TradeAttribute6, COL_TradeAttribute6}
                     ,{CAP_TradeCostBasisPnl, COL_TradeCostBasisPnl}
                     ,{CAP_TradeDate, COL_TradeDate}
                     ,{CAP_TradeDayPnl, COL_TradeDayPnl}
                     ,{CAP_TransactionSide, COL_TransactionSide}
                     ,{CAP_TransactionType, COL_TransactionType}
                     ,{CAP_UcitsEligibleTag, COL_UcitsEligibleTag}
                     ,{CAP_UDAAsset, COL_UDAAsset}
                     ,{CAP_UDACountry, COL_UDACountry}
                     ,{CAP_UDASector, COL_UDASector}
                     ,{CAP_UDASecurityType, COL_UDASecurityType}
                     ,{CAP_UDASubSector, COL_UDASubSector}
                     ,{CAP_Underlying, COL_Underlying}
                     ,{CAP_UnderlyingGrossExposure, COL_UnderlyingGrossExposure}
                     ,{CAP_UnderlyingGrossExposureInBaseCurrency, COL_UnderlyingGrossExposureInBaseCurrency}
                     ,{CAP_UnderlyingStockPrice, COL_UnderlyingStockPrice}
                     ,{CAP_UnderlyingSymbol, COL_UnderlyingSymbol}
                     ,{CAP_UnderlyingValueForOptions, COL_UnderlyingValueForOptions}
                     ,{CAP_UserName, COL_UserName}
                     ,{CAP_Volatility, COL_Volatility}
                     ,{CAP_VsCurrencySymbol, COL_VsCurrencySymbol}
                     ,{CAP_YesterdayFXRate, COL_YesterdayFXRate}
                     ,{CAP_YesterdayMarketValue, COL_YesterdayMarketValue}
                     ,{CAP_YesterdayMarketValueInBaseCurrency, COL_YesterdayMarketValueInBaseCurrency }
                     ,{CAP_YesterdayMarkPriceStr, COL_YesterdayMarkPriceStr}
                     ,{CAP_YesterdayUnderlyingMarkPriceStr, COL_YesterdayUnderlyingMarkPriceStr}
                     ,{Prana.Global.OrderFields.CAPTION_LEVEL1NAME,Prana.Global.OrderFields.PROPERTY_LEVEL1NAME}
                     ,{Prana.Global.OrderFields.CAPTION_LEVEL2NAME,Prana.Global.OrderFields.PROPERTY_LEVEL2NAME}
                     ,{CAP_CounterCurrencySymbol, COL_CounterCurrencySymbol}
                     ,{CAP_CounterCurrencyAmount, COL_CounterCurrencyAmount}
                     ,{CAP_CounterCurrencyCostBasisPnL, COL_CounterCurrencyCostBasisPnL}
                     ,{CAP_CounterCurrencyDayPNL, COL_CounterCurrencyDayPnL}
                     ,{CAP_ItmOtm, COL_ItmOtm}
                     ,{CAP_PercentOfITMOTM, COL_PercentOfITMOTM}
                     ,{CAP_IntrinsicValue, COL_IntrinsicValue}
                     ,{CAP_DaysToExpiry, COL_DaysToExpiry}
                     ,{CAP_GainLossIfExerciseAssign, COL_GainLossIfExerciseAssign}
                     ,{CAP_DayTradedPosition,COL_DayTradedPosition}
                     ,{CAP_TradeVolume,COL_TradeVolume}
                     ,{CAP_BloombergSymbolWithExchangeCode, COL_BloombergSymbolWithExchangeCode }
                     ,{CAP_PricingStatus, COL_PricingStatus }
            };
            for (int i = 7; i <= 45; i++)
            {
                columnFromCaptionMappingDictionary.Add(CAP_TradeAttribute + i, COL_TradeAttribute + i);
            }
            return columnFromCaptionMappingDictionary;
        }
        #endregion

        #region Dictionary caption from column name
        public static Dictionary<string, string> captionFromColumnMappingDictionary = CreateCaptionFromColumnMappingDictionary();

        private static Dictionary<string, string> CreateCaptionFromColumnMappingDictionary()
        {
            Dictionary<string, string> columnFromCaptionMappingDictionary = new Dictionary<string, string>()
        {
                      {COL_Analyst, CAP_Analyst}
                     ,{COL_AskPrice, CAP_AskPrice}
                     ,{COL_Asset, CAP_Asset}
                     ,{COL_AverageVolume20Day, CAP_AverageVolume20Day}
                     ,{COL_AverageVolume20DayUnderlyingSymbol, CAP_AverageVolume20DayUnderlyingSymbol}
                     ,{COL_AvgPrice, CAP_AvgPrice}
                     ,{COL_Beta, CAP_Beta}
                     ,{COL_BetaAdjExposure, CAP_BetaAdjExposure}
                     ,{COL_BetaAdjExposureInBaseCurrency, CAP_BetaAdjExposureInBaseCurrency}
                     ,{COL_BetaAdjGrossExposure, CAP_BetaAdjGrossExposure}
                     ,{COL_BetaAdjGrossExposureUnderlying, CAP_BetaAdjGrossExposureUnderlying}
                     ,{COL_BetaAdjGrossExposureUnderlyingInBaseCurrency, CAP_BetaAdjGrossExposureUnderlyingInBaseCurrency}
                     ,{COL_BidPrice, CAP_BidPrice}
                     ,{COL_BloombergSymbol, CAP_BloombergSymbol}
                     ,{COL_FactSetSymbol, CAP_FactSetSymbol}
                     ,{COL_ActivSymbol, CAP_ActivSymbol}
                     ,{COL_CashImpact, CAP_CashImpact}
                     ,{COL_CashImpactInBaseCurrency, CAP_CashImpactInBaseCurrency}
                     ,{COL_ChangeInUnderlyingPrice, CAP_ChangeInUnderlyingPrice}
                     ,{COL_ClosingPrice, CAP_ClosingPrice}
                     ,{COL_ContractType, CAP_ContractType}
                     ,{COL_CostBasisBreakEven, CAP_CostBasisBreakEven}
                     ,{COL_CostBasisUnRealizedPNL, CAP_CostBasisUnrealizedPnL}
                     ,{COL_CostBasisUnrealizedPnLInBaseCurrency, CAP_CostBasisUnrealizedPnLInBaseCurrency}
                     ,{COL_CounterPartyName, CAP_CounterPartyName}
                     ,{COL_CountryOfRisk, CAP_CountryOfRisk}
                     ,{COL_CurrencySymbol, CAP_CurrencySymbol}
                     ,{COL_CUSIPSymbol, CAP_CUSIPSymbol}
                     ,{COL_CustomUDA1, CAP_CustomUDA1}
                     ,{COL_CustomUDA2, CAP_CustomUDA2}
                     ,{COL_CustomUDA3, CAP_CustomUDA3}
                     ,{COL_CustomUDA4, CAP_CustomUDA4}
                     ,{COL_CustomUDA5, CAP_CustomUDA5}
                     ,{COL_CustomUDA6, CAP_CustomUDA6}
                     ,{COL_CustomUDA7, CAP_CustomUDA7}
                     ,{COL_CustomUDA8, CAP_CustomUDA8}
                     ,{COL_CustomUDA9, CAP_CustomUDA9}
                     ,{COL_CustomUDA10, CAP_CustomUDA10}
                     ,{COL_CustomUDA11, CAP_CustomUDA11}
                     ,{COL_CustomUDA12, CAP_CustomUDA12}
                     ,{COL_DataSourceNameIDValue, CAP_DataSourceNameIDValue}
                     ,{COL_DayInterest, CAP_DayInterest}
                     ,{COL_DayPnL, CAP_DayPnL}
                     ,{COL_DayPnLInBaseCurrency, CAP_DayPnLInBaseCurrency}
                     ,{COL_DayReturn, CAP_DayReturn}
                     ,{COL_Delta, CAP_Delta}
                     ,{COL_DeltaAdjPosition, CAP_DeltaAdjPosition}
                     ,{COL_DeltaAdjPositionLME, CAP_DeltaAdjPositionLME}
                     ,{COL_DeltaSource, CAP_DeltaSource}
                     ,{COL_DividendYield, CAP_DividendYield}
                     ,{COL_EarnedDividendBase, CAP_EarnedDividendBase}
                     ,{COL_EarnedDividendLocal, CAP_EarnedDividendLocal}
                     ,{COL_Exchange, CAP_Exchange}
                     ,{COL_ExDividendDate, CAP_ExDividendDate}
                     ,{COL_ExpirationDate, CAP_ExpirationDate}
                     ,{COL_ExpirationMonth, CAP_ExpirationMonth}
                     ,{COL_Exposure, CAP_Exposure}
                     ,{COL_ExposureBPInBaseCurrency, CAP_ExposureBPInBaseCurrency}
                     ,{COL_ExposureInBaseCurrency, CAP_ExposureInBaseCurrency}
                     ,{COL_ForwardPoints, CAP_ForwardPoints}
                     ,{COL_FullSecurityName, CAP_FullSecurityName}
                     ,{COL_FxCostBasisPnl, CAP_FxCostBasisPnl}
                     ,{COL_FxDayPnl, CAP_FxDayPnl}
                     ,{COL_FXRate, CAP_FXRate}
                     ,{COL_FXRateDisplay, CAP_FXRateDisplay}
                     ,{COL_FXRateOnTradeDateStr, CAP_FXRateOnTradeDateStr}
                     ,{COL_GrossExposure, CAP_GrossExposure}
                     ,{COL_GrossMarketValue, CAP_GrossMarketValue}
                     ,{COL_IDCOSymbol, CAP_IDCOSymbol}
                     ,{COL_InternalComments, CAP_InternalComments}
                     ,{COL_ISINSymbol, CAP_ISINSymbol}
                     ,{COL_Issuer, CAP_Issuer}
                     ,{COL_LastPrice, CAP_LastPrice}
                     ,{COL_LastUpdatedUTC, CAP_LastUpdatedUTC}
                     ,{COL_LeadCurrencySymbol, CAP_LeadCurrencySymbol}
                     ,{COL_LeveragedFactor, CAP_LeveragedFactor}
                     ,{COL_LiquidTag, CAP_LiquidTag}
                     ,{COL_MarketCap, CAP_MarketCap}
                     ,{COL_MarketCapitalization, CAP_MarketCapitalization}
                     ,{COL_MarketValue, CAP_MarketValue}
                     ,{COL_MarketValueInBaseCurrency, CAP_MarketValueInBaseCurrency}
                     ,{COL_MasterFund,  CachedDataManager.GetInstance.IsShowmasterFundAsClient() ? CAP_CLIENT :CAP_MasterFund}
                     ,{COL_MasterStrategy, CAP_MasterStrategy}
                     ,{COL_MidPrice, CAP_MidPrice}
                     ,{COL_Multiplier, CAP_Multiplier}
                     ,{COL_NAV, CAP_NAV}
                     ,{COL_NavTouch, CAP_NavTouch}
                     ,{COL_NetExposure, CAP_NetExposure}
                     ,{COL_NetExposureInBaseCurrency, CAP_NetExposureInBaseCurrency}
                     ,{COL_NetNotionalForCostBasisBreakEven, CAP_NetNotionalForCostBasisBreakEven}
                     ,{COL_NetNotionalValue, CAP_NetNotionalValue}
                     ,{COL_NetNotionalValueBase, CAP_NetNotionalValueBase}
                     ,{COL_OrderSideTagValue, CAP_OrderSideTagValue}
                     ,{COL_OSISymbol, CAP_OSISymbol}
                     ,{COL_PercentageAverageVolume, CAP_PercentageAverageVolume}
                     ,{COL_PercentageAverageVolumeDeltaAdjusted, CAP_PercentageAverageVolumeDeltaAdjusted}
                     ,{COL_PercentageChange, CAP_PercentageChange}
                     ,{COL_PercentageGainLoss, CAP_PercentageGainLoss}
                     ,{COL_PercentageGainLossCostBasis, CAP_PercentageGainLossCostBasis}
                     ,{COL_PercentagePNLContribution, CAP_PercentagePNLContribution}
                     ,{COL_PercentageUnderlyingChange, CAP_PercentageUnderlyingChange}
                     ,{COL_PercentBetaAdjGrossExposureInBaseCurrency, CAP_PercentBetaAdjGrossExposureInBaseCurrency}
                     ,{COL_PercentDayPnLGrossMV, CAP_PercentDayPnLGrossMV}
                     ,{COL_PercentDayPnLNetMV, CAP_PercentDayPnLNetMV}
                     ,{COL_PercentExposureInBaseCurrency, CAP_PercentExposureInBaseCurrency}
                     ,{COL_PercentGrossExposureInBaseCurrency, CAP_PercentGrossExposureInBaseCurrency}
                     ,{COL_PercentGrossMarketValueInBaseCurrency, CAP_PercentGrossMarketValueInBaseCurrency}
                     ,{COL_PercentNetExposureInBaseCurrency, CAP_PercentNetExposureInBaseCurrency}
                     ,{COL_PercentNetMarketValueInBaseCurrency, CAP_PercentNetMarketValueInBaseCurrency}
                     ,{COL_PercentUnderlyingGrossExposureInBaseCurrency, CAP_PercentUnderlyingGrossExposureInBaseCurrency}
                     ,{COL_PositionSideExposure, CAP_PositionSideExposure}
                     ,{COL_PositionSideExposureBoxed, CAP_PositionSideExposureBoxed}
                     ,{COL_PositionSideExposureUnderlying, CAP_PositionSideExposureUnderlying}
                     ,{COL_PositionSideMV, CAP_PositionSideMV}
                     ,{COL_Premium, CAP_Premium}
                     ,{COL_PremiumDollar, CAP_PremiumDollar}
                     ,{COL_PricingSource , CAP_PricingSource}
                     ,{COL_Quantity, CAP_Quantity}
                     ,{COL_Region, CAP_Region}
                     ,{COL_ReutersSymbol, CAP_ReutersSymbol}
                     ,{COL_RiskCurrency, CAP_RiskCurrency}
                     ,{COL_SEDOLSymbol, CAP_SEDOLSymbol}
                     ,{COL_SelectedFeedPrice, CAP_SelectedFeedPrice}
                     ,{COL_SelectedFeedPriceInBaseCurrency, CAP_SelectedFeedPriceInBaseCurrency}
                     ,{COL_SettlementDate, CAP_SettlementDate}
                     ,{COL_SharesOutstanding, CAP_SharesOutstanding}
                     ,{COL_SideMultiplier, CAP_SideMultiplier}
                     ,{COL_SideName, CAP_SideName}
                     ,{COL_StartOfDayNAV, CAP_StartOfDayNAV}
                     ,{COL_StartTradeDate, CAP_StartTradeDate}
                     ,{COL_StrikePrice, CAP_StrikePrice}
                     ,{COL_Symbol , CAP_Symbol}
                     ,{COL_TotalInterest, CAP_TotalInterest}
                     ,{COL_TradeAttribute1, CAP_TradeAttribute1}
                     ,{COL_TradeAttribute2, CAP_TradeAttribute2}
                     ,{COL_TradeAttribute3, CAP_TradeAttribute3}
                     ,{COL_TradeAttribute4, CAP_TradeAttribute4}
                     ,{COL_TradeAttribute5, CAP_TradeAttribute5}
                     ,{COL_TradeAttribute6, CAP_TradeAttribute6}
                     ,{COL_TradeCostBasisPnl, CAP_TradeCostBasisPnl}
                     ,{COL_TradeDate, CAP_TradeDate}
                     ,{COL_TradeDayPnl, CAP_TradeDayPnl}
                     ,{COL_TransactionSide, CAP_TransactionSide}
                     ,{COL_TransactionType, CAP_TransactionType}
                     ,{COL_UcitsEligibleTag, CAP_UcitsEligibleTag}
                     ,{COL_UDAAsset, CAP_UDAAsset}
                     ,{COL_UDACountry, CAP_UDACountry}
                     ,{COL_UDASector, CAP_UDASector}
                     ,{COL_UDASecurityType, CAP_UDASecurityType}
                     ,{COL_UDASubSector, CAP_UDASubSector}
                     ,{COL_Underlying, CAP_Underlying}
                     ,{COL_UnderlyingGrossExposure, CAP_UnderlyingGrossExposure}
                     ,{COL_UnderlyingGrossExposureInBaseCurrency, CAP_UnderlyingGrossExposureInBaseCurrency}
                     ,{COL_UnderlyingStockPrice, CAP_UnderlyingStockPrice}
                     ,{COL_UnderlyingSymbol, CAP_UnderlyingSymbol}
                     ,{COL_UnderlyingValueForOptions, CAP_UnderlyingValueForOptions}
                     ,{COL_UserName, CAP_UserName}
                     ,{COL_Volatility, CAP_Volatility}
                     ,{COL_VsCurrencySymbol, CAP_VsCurrencySymbol}
                     ,{COL_YesterdayFXRate, CAP_YesterdayFXRate}
                     ,{COL_YesterdayMarketValue, CAP_YesterdayMarketValue}
                     ,{COL_YesterdayMarketValueInBaseCurrency, CAP_YesterdayMarketValueInBaseCurrency }
                     ,{COL_YesterdayMarkPriceStr, CAP_YesterdayMarkPriceStr}
                     ,{COL_YesterdayUnderlyingMarkPriceStr, CAP_YesterdayUnderlyingMarkPriceStr}
                     ,{COL_CounterCurrencySymbol, CAP_CounterCurrencySymbol}
                     ,{COL_CounterCurrencyAmount, CAP_CounterCurrencyAmount}
                     ,{COL_CounterCurrencyCostBasisPnL, CAP_CounterCurrencyCostBasisPnL}
                     ,{COL_CounterCurrencyDayPnL, CAP_CounterCurrencyDayPNL}
                     ,{COL_ItmOtm, CAP_ItmOtm}
                     ,{COL_PercentOfITMOTM, CAP_PercentOfITMOTM}
                     ,{COL_IntrinsicValue, CAP_IntrinsicValue}
                     ,{COL_DaysToExpiry, CAP_DaysToExpiry}
                     ,{COL_GainLossIfExerciseAssign, CAP_GainLossIfExerciseAssign}
                     ,{COL_DayTradedPosition,CAP_DayTradedPosition}
                     ,{COL_TradeVolume,CAP_TradeVolume}
                     ,{COL_BloombergSymbolWithExchangeCode, CAP_BloombergSymbolWithExchangeCode}
                     ,{COL_PricingStatus, CAP_PricingStatus }
    };
            for (int i = 7; i <= 45; i++)
            {
                columnFromCaptionMappingDictionary.Add(COL_TradeAttribute + i, CAP_TradeAttribute + i);
            }
            return columnFromCaptionMappingDictionary;
        }
        #endregion
    }
}