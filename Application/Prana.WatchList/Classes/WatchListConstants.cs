using Prana.LogManager;
using System;

namespace Prana.WatchList.Classes
{
    class WatchListConstants
    {

        public const string TOTALROWINGRID_WATCHLIST = "TotalRowInGrid_WatchList";

        #region Messages
        public const string MSG_BBGID_IS_INVALID = "BBGID is invalid. Length of BBGID should be 12.";
        public const string MSG_TRADE_SERVER_CONNECTED = "Trade Server : Connected";
        public const string MSG_SYMBOL_ADDED = "Symbol Added.";
        public const string MSG_SYMBOL_ALREADY_ADDED = "Symbol Already Added.";
        public const string MSG_SYMBOL_EMPTY = "Symbol not provided.";
        public const string MSG_ADD_SYMBOL_ALERT = "The symbol count has exceeded the tab limit. You have to add manually new tab first.";
        public const string MSG_SELECT_ROW_FOR_DEL = "Please select row before deleting.";
        public const string MSG_NO_TAB = "There is no tab";
        public const string MSG_NO_ROW = "There is no row";
        public const string MSG_LAYOUT_SAVED = "Layout Saved.";
        public const string MSG_FILTER_REMOVED = "Filter Removed.";
        #endregion

        #region Captions
        public const string CAP_AnnualDividend = "Annual Dividend";
        public const string CAP_AskExchange = "Ask Exchange";
        public const string CAP_AskSize = "Ask Size";
        public const string CAP_AverageVolume20Day = "Average Volume 20 Day";
        public const string CAP_AvgVolume = "Average Volume";
        public const string CAP_Beta_5yrMonthly = "Beta 5 Year Monthly";
        public const string CAP_BidExchange = "Bid Exchange";
        public const string CAP_BidSize = "Bid Size";
        public const string CAP_BloombergSymbol = "Bloomberg";
        public const string CAP_BloombergSymbolWithExchangeCode = "Bloomberg Symbol(With Exchange Code)";
        public const string CAP_FactSetSymbol = "FactSet Symbol";
        public const string CAP_ActivSymbol = "ACTIV Symbol";
        public const string CAP_CategoryCode = "Category Code";
        public const string CAP_CFICode = "CFI Code";
        public const string CAP_Change = "Day Change";
        public const string CAP_ConversionMethod = "Conversion Method";
        public const string CAP_CurencyCode = "Curency Code";
        public const string CAP_CusipNo = "Cusip No";
        public const string CAP_DaysToExpiration = "Days To Expiration";
        public const string CAP_DeltaSource = "Delta Source";
        public const string CAP_DivDistributionDate = "Div Distribution Date";
        public const string CAP_DividendAmtRate = "Dividend Amount Rate";
        public const string CAP_DividendInterval = "Dividend Interval";
        public const string CAP_DividendYield = "Dividend Yield";
        public const string CAP_ExchangeID = "Exchange ID";
        public const string CAP_ExpirationDate = "Expiration Date";
        public const string CAP_FinalDividendYield = "Final Dividend Yield";
        public const string CAP_FinalImpliedVol = "Final Implied Volume";
        public const string CAP_FinalInterestRate = "Final Interest Rate";
        public const string CAP_ForwardPoints = "Forward Points";
        public const string CAP_FullCompanyName = "Full Company Name";
        public const string CAP_GapOpen = "Gap Open";
        public const string CAP_IDCOOptionSymbol = "IDCO Option Symbol";
        public const string CAP_ImpliedVol = "Implied Volume";
        public const string CAP_InterestRate = "Interest Rate";
        public const string CAP_IsChangedToHigherCurrency = "Change To Higher Currency";
        public const string CAP_LastPrice = "Last Price";
        public const string CAP_LastTick = "Last Tick";
        public const string CAP_ListedExchange = "Listed Exchange";
        public const string CAP_MarketCapitalization = "Market Capitalization";
        public const string CAP_MarkPrice = "Mark Price";
        public const string CAP_MarkPriceStr = "Mark Price String";
        public const string CAP_OpenInterest = "Open Interest";
        public const string CAP_OpraSymbol = "Opra Symbol";
        public const string CAP_OSIOptionSymbol = "OSI Option Symbol";
        public const string CAP_PctChange = "% Day Change";
        public const string CAP_PreferencedPrice = "Preferenced Price";
        public const string CAP_PricingSource = "Pricing Source";
        public const string CAP_PricingProvider = "Market Data Provider";
        public const string CAP_PutOrCall = "Put Or Call";
        public const string CAP_RequestedSymbology = "Requested Symbology";
        public const string CAP_ReuterSymbol = "Reuter Symbol";
        public const string CAP_SedolSymbol = "Sedol Symbol";
        public const string CAP_SelectedFeedPrice = "Selected Feed Price";
        public const string CAP_SharesOutstanding = "Shares Outstanding (MM)";
        public const string CAP_StrikePrice = "Strike Price";
        public const string CAP_TheoreticalPrice = "Theoretical Price";
        public const string CAP_TotalVolume = "Total Volume";
        public const string CAP_TradeVolume = "Trade Volume";
        public const string CAP_UnderlyingCategory = "Underlying Category";
        public const string CAP_UnderlyingData = "Underlying Data";
        public const string CAP_UnderlyingSymbol = "Underlying Symbol";
        public const string CAP_UpdateTime = "Update Time";
        public const string CAP_Volume10DAvg = "Volume 10 Day Average";
        public const string CAP_XDividendDate = "XDividend Date";
        #endregion

        #region Column Name
        public const string COL_AnnualDividend = "AnnualDividend";
        public const string COL_AskExchange = "AskExchange";
        public const string COL_AskSize = "AskSize";
        public const string COL_AverageVolume20Day = "AverageVolume20Day";
        public const string COL_AvgVolume = "AvgVolume";
        public const string COL_Beta_5yrMonthly = "Beta_5yrMonthly";
        public const string COL_BidExchange = "BidExchange";
        public const string COL_BidSize = "BidSize";
        public const string COL_BloombergSymbol = "BloombergSymbol";
        public const string COL_BloombergSymbolWithExchangeCode = "BloombergSymbolWithExchangeCode";
        public const string COL_FactSetSymbol = "FactSetSymbol";
        public const string COL_ActivSymbol = "ActivSymbol";
        public const string COL_CategoryCode = "CategoryCode";
        public const string COL_CFICode = "CFICode";
        public const string COL_ConversionMethod = "ConversionMethod";
        public const string COL_CurencyCode = "CurencyCode";
        public const string COL_CusipNo = "CusipNo";
        public const string COL_DaysToExpiration = "DaysToExpiration";
        public const string COL_DeltaSource = "DeltaSource";
        public const string COL_DivDistributionDate = "DivDistributionDate";
        public const string COL_DividendAmtRate = "DividendAmtRate";
        public const string COL_DividendInterval = "DividendInterval";
        public const string COL_DividendYield = "DividendYield";
        public const string COL_ExchangeID = "ExchangeID";
        public const string COL_ExpirationDate = "ExpirationDate";
        public const string COL_FinalDividendYield = "FinalDividendYield";
        public const string COL_FinalImpliedVol = "FinalImpliedVol";
        public const string COL_FinalInterestRate = "FinalInterestRate";
        public const string COL_ForwardPoints = "ForwardPoints";
        public const string COL_FullCompanyName = "FullCompanyName";
        public const string COL_GapOpen = "GapOpen";
        public const string COL_IDCOOptionSymbol = "IDCOOptionSymbol";
        public const string COL_ImpliedVol = "ImpliedVol";
        public const string COL_InterestRate = "InterestRate";
        public const string COL_IsChangedToHigherCurrency = "IsChangedToHigherCurrency";
        public const string COL_LastPrice = "LastPrice";
        public const string COL_LastTick = "LastTick";
        public const string COL_ListedExchange = "ListedExchange";
        public const string COL_MarketCapitalization = "MarketCapitalization";
        public const string COL_MarkPrice = "MarkPrice";
        public const string COL_MarkPriceStr = "MarkPriceStr";
        public const string COL_OpenInterest = "OpenInterest";
        public const string COL_OpraSymbol = "OpraSymbol";
        public const string COL_OSIOptionSymbol = "OSIOptionSymbol";
        public const string COL_PctChange = "PctChange";
        public const string COL_PreferencedPrice = "PreferencedPrice";
        public const string COL_PricingSource = "PricingSource";
        public const string COL_PricingProvider = "MarketDataProvider";
        public const string COL_PutOrCall = "PutOrCall";
        public const string COL_RequestedSymbology = "RequestedSymbology";
        public const string COL_Symbol = "Symbol";
        public const string COL_ReuterSymbol = "ReuterSymbol";
        public const string COL_SedolSymbol = "SedolSymbol";
        public const string COL_SelectedFeedPrice = "SelectedFeedPrice";
        public const string COL_SharesOutstanding = "SharesOutstanding";
        public const string COL_StrikePrice = "StrikePrice";
        public const string COL_TheoreticalPrice = "TheoreticalPrice";
        public const string COL_TotalVolume = "TotalVolume";
        public const string COL_TradeVolume = "TradeVolume";
        public const string COL_UnderlyingCategory = "UnderlyingCategory";
        public const string COL_UnderlyingData = "UnderlyingData";
        public const string COL_UnderlyingSymbol = "UnderlyingSymbol";
        public const string COL_UpdateTime = "UpdateTime";
        public const string COL_Volume10DAvg = "Volume10DAvg";
        public const string COL_XDividendDate = "XDividendDate";
        public const string COL_Bid = "Bid";
        public const string COL_Ask = "Ask";
        public const string COL_Change = "Change";
        public const string COL_Theta = "Theta";
        public const string COL_Vega = "Vega";
        public const string COL_Rho = "Rho";
        public const string COL_Gamma = "Gamma";
        public const string COL_Delta = "Delta";
        public const string COL_VWAP = "VWAP";
        public const string COL_Previous = "Previous";
        public const string COL_Open = "Open";
        public const string COL_High = "High";
        public const string COL_Low = "Low";
        public const string COL_Spread = "Spread";
        public const string COL_Dividend = "Dividend";
        public const string COL_Mid = "Mid";
        public const string COL_Imid = "iMid";
        public const string COL_StockBorrowCost = "StockBorrowCost";
        public const string COL_High52W = "High52W";
        public const string COL_Low52W = "Low52W";
        public const string COL_DelayInterval = "DelayInterval";

        #endregion    

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

        public const string FORMAT_TWO_DECIMAL_DIGITS_MILLIONS = "#,##0,,.00";
        #endregion 

        public static string GetDefaultCloumnWiseDecimalDigits(string columnName)
        {
            try
            {
                switch (columnName)
                {


                    case COL_ImpliedVol:
                    case COL_FinalImpliedVol:
                    case COL_InterestRate:
                    case COL_FinalInterestRate:
                    case COL_OpenInterest:
                    case COL_ForwardPoints:
                    case COL_Theta:
                    case COL_Vega:
                    case COL_Rho:
                    case COL_Gamma:
                    case COL_VWAP:
                    case COL_Delta:
                    case COL_Beta_5yrMonthly:
                    case COL_Bid:
                    case COL_Ask:
                    case COL_StrikePrice:
                    case COL_TheoreticalPrice:
                    case COL_LastPrice:
                    case COL_Change:
                    case COL_PctChange:
                    case COL_MarkPrice:
                    case COL_Previous:
                    case COL_Open:
                    case COL_High:
                    case COL_Low:
                    case COL_AvgVolume:
                    case COL_PreferencedPrice:
                    case COL_AverageVolume20Day:
                    case COL_Volume10DAvg:
                    case COL_Spread:
                    case COL_GapOpen:
                    case COL_SelectedFeedPrice:
                    case COL_MarketCapitalization:
                    case COL_Dividend:
                    case COL_DividendAmtRate:
                    case COL_AnnualDividend:
                    case COL_DividendYield:
                    case COL_FinalDividendYield:
                    case COL_Mid:
                    case COL_Imid:
                    case COL_StockBorrowCost:
                    case COL_High52W:
                    case COL_Low52W:

                        return FORMAT_TWO_DECIMAL_DIGITS;

                    case COL_SharesOutstanding:
                        return FORMAT_TWO_DECIMAL_DIGITS_MILLIONS;

                    default:
                        return FORMAT_ZERO_DECIMAL_DIGITS;
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
                return string.Empty;
            }
        }

    }
}










