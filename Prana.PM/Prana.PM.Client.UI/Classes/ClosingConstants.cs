namespace Prana.PM.Client.UI
{
    public class ClosingConstants
    {
        #region Caption Constants
        public const string CAP_AvgPriceBase = "AvgPriceBase";
        public const string CAP_TaxlotId = "Taxlot ID";
        public const string CAP_Account = "Account";
        public const string CAP_Strategy = "Strategy";
        public const string CAP_TradeDate = "Trade Date";
        public const string CAP_ProcessDate = "Process Date";
        public const string CAP_OriginalPurchaseDate = "Original Purchase Date";
        public const string CAP_PositionType = "Position Type";
        public const string CAP_ClosedPositionType = "Closing Position Type";
        public const string CAP_Symbol = "Symbol";
        public const string CAP_StartQty = "Start Quantity";
        public const string CAP_CloseQty = "Qty Closed";
        public const string CAP_OpenQty = "Open Qty";
        public const string CAP_AvgPrice = "Opening Price";
        public const string CAP_AvgClosingPrice = "Closing Price";
        public const string CAP_Commission = "Total Fees and Commission";
        public const string CAP_OtherFees = "Other Fees";
        public const string CAP_RealizedPNL = "Realized PNL(C.B.)";
        public const string CAP_AUECCloseDt = "AUEC Close Date";
        public const string CAP_CloseDt = "Closing Date";
        public const string CAP_Side = "Side";
        public const string CAP_OpeningSide = "Opening Side";
        public const string CAP_ClosingSide = "Closing Side";
        public const string CAP_NetNotional = "Net Notional";
        public const string CAP_SecurityFullName = "Security Name";
        public const string CAP_ClosingMode = "Closing Mode";
        public const string CAP_SettlementPrice = "Settlement Price";
        public const string CAP_AssetCategory = "Asset Class";
        public const string CAP_Exchange = "Exchange";
        public const string CAP_PositionCommission = "Opening Fees & Commission";
        public const string CAP_Currency = "Currency";
        public const string CAP_Underlying = "Underlying";
        public const string CAP_ClosingID = "ClosingID";
        public const string CAP_ClosingAlgo = "Closing Method";
        public const string CAP_IsSwapped = "IsSwapped";
        public const string CAPTION_LEVEL1NAME = "Account";
        public const string CAPTION_AUECLOCALDATE = "Trade Date";
        public const string CAPTION_TAXLOTQTY = "Quantity";
        public const string CAPTION_AVGPRICE = "Unit Cost";
        public const string CAPTION_LEVEL2NAME = "Strategy";
        public const string CAPTION_ORDERSIDE = "Side";
        public const string CAPTION_QUANTITYTOCLOSE = "Close Quantity";
        public const string CAP_Fees = "Other Broker Fees";
        public const string CAPTION_ORIGPURCHASEDATE = "Original Purchase Date";
        public const string CAPTION_ExecutedQty = "Executed Qty";
        public const string CAPTION_MarkPrice = "Closing Mark";
        public const string CAPTION_UnRealizedPNL = "Un-Realized PNL";
        public const string CAPTION_ClosingStatus = "Closing Status";
        public static string CAPTION_AssetDerivative = "Asset Derivative";
        public static string CAPTION_TRADEATTRIBUTE = "Trade Attribute ";
        #endregion

        #region Column Constants
        public const string COL_ID = "ID";
        public const string COL_StartDate = "StartDate";
        public const string COL_LastActivityDate = "LastActivityDate";
        public const string COL_PositionTag = "PositionalTag";
        public const string COL_ClosingTag = "ClosingPositionTag";
        public const string COL_AccountValue = "AccountValue";
        public const string COL_PNLPOSITION = "PNLWhenTaxLotsPopulated";
        public const string COL_PNL = "CostBasisRealizedPNL";
        public const string COL_StartTaxLotID = "StartTaxLotID";
        public const string COL_PositionStartQuantity = "PositionStartQty";
        public const string COL_AccountID = "AccountID";
        public const string COL_Multiplier = "Multiplier";
        public const string COL_AUECID = "AUECID";
        public const string COL_RealizedPNL = "CostBasisRealizedPNL";
        public const string COL_RecordType = "RecordType";
        public const string COL_Status = "Status";
        public const string COL_EndDate = "EndDate";
        public const string COL_Description = "Description";
        public const string COL_Strategy = "Strategy";
        public const string COL_StrategyID = "StrategyID";
        public const string COL_MarkPriceForMonth = "MarkPriceForMonth";
        public const string COL_MonthToDateRealizedProfit = "MonthToDateRealizedProfit";
        public const string COL_NotionalValue = "NotionalValue";
        public const string COL_AvgPriceRealizedPL = "AvgPriceRealizedPL";
        public const string COL_SymbolAveragePrice = "SymbolAveragePrice";
        public const string COL_AUECLocalCloseDate = "AUECLocalCloseDate";
        public const string COL_CloseDate = "TimeOfSaveUTC";
        public const string COL_GeneratedTaxlotSymbol = "GeneratedTaxlotSymbol";
        public const string COL_Exchange = "Exchange";
        public const string COL_OpenQty = "OpenQty";
        public const string COL_CurrencyID = "CurrencyID";
        public const string COL_Currency = "Currency";
        //public const string COL_UnderlyingName = "Underlying";
        public const string COL_ClosingID = "ClosingID";
        public const string COL_ClosingAlgo = "ClosingAlgo";
        public const string COL_ClosingAlgoChooser = "ClosingAlgoChooser";
        public const string PositionalSide_Long = "Long";
        public const string PositionalSide_Short = "Short";
        public const string COL_PositionSide = "PositionSide";
        public const string COL_TradeDatePosition = "TradeDate";
        public const string COL_AveragePrice = "AvgPrice";
        public const string COLUMN_LEVEL1NAME = "Level1Name";
        public const string COLUMN_AUECLOCALDATE = "AUECLocalDate";
        public const string COLUMN_ORIGPURCHASEDATE = "OriginalPurchaseDate";
        public const string COLUMN_TAXLOTQTY = "TaxLotQty";
        public const string COLUMN_AVGPRICE = "AvgPrice";
        public const string COLUMN_LEVEL2NAME = "Level2Name";
        public const string COLUMN_ORDERSIDE = "OrderSide";
        public const string COLUMN_QUANTITYTOCLOSE = "TaxLotQtyToClose";
        public const string COL_TradeDate = "AUECLocalDate";
        public const string COL_AllocationID = "TaxLotID";
        public const string COL_ProcessDate = "ProcessDate";
        public const string COL_ClosingTradeDate = "ClosingTradeDate";
        public const string COL_TradeDateUTC = "TradeDateUTC";
        public const string COL_ClosingSide = "ClosingSide";
        public const string COL_Symbol = "Symbol";
        public const string COL_SecurityFullName = "CompanyName";
        public const string COL_OpenQuantity = "TaxLotQty";
        public const string COL_SideMultiplier = "SideMultiplier";
        public const string COL_ClosedQty = "ClosedQty";
        public const string COL_OpenAveragePrice = "OpenAveragePrice";
        public const string COL_ClosedAveragePrice = "ClosedAveragePrice";
        public const string COL_Account = "Level1Name";
        public const string COL_SideID = "OrderSideTagValue";
        public const string COL_IsPosition = "IsPosition";
        public const string COL_PositionTaxlotID = "PositionTaxlotID";
        public const string COL_OpenCommission = "OpenTotalCommissionandFees";
        public const string COL_PositionCommission = "PositionTotalCommissionandFees";
        public const string COL_OpenFees = "OtherBrokerOpenFees";
        public const string COL_PositionFees = "PositionOtherBrokerFees";
        public const string COL_ClosedCommission = "ClosedTotalCommissionandFees";
        public const string COL_ClosingTotalCommissionandFees = "ClosingTotalCommissionandFees";
        public const string COL_NetNotionalValue = "NetNotionalValue";
        public const string COL_StrategyValue = "Level2Name";
        public const string COL_SettledQty = "SettledQty";
        public const string COL_CashSettledPrice = "CashSettledPrice";
        public const string COL_ClosingMode = "ClosingMode";
        public const string COL_IsExpired_Settled = "IsExpired_Settled";
        public const string COL_AssetCategoryValue = "AssetCategoryValue";
        public const string COL_AssetName = "AssetName";
        public const string COL_ExpiryDate = "ExpirationDate";
        public const string COL_Underlying = "UnderlyingName";
        public const string COL_UnitCost = "UnitCost";
        public const string COL_PositionTagValue = "PositionTag";
        public const string COL_IsSwap = "ISSwap";
        public const string COL_LotId = "LotId";
        public const string COL_ExternalTransId = "ExternalTransId";
        //public const string _currencyColumnFormat = "0.0000";
        public const string COL_Side = "OrderSide";
        public const string COL_OriginalPurchaseDate = "OriginalPurchaseDate";
        public const string COL_ExecutedQty = "ExecutedQty";
        public const string COL_MarkPrice = "MarkPrice";
        public const string COL_UnRealizedPNL = "UnRealizedPNL";
        public const string COL_NotionalChange = "NotionalChange";
        public const string COL_CostBasisGrossPNL = "CostBasisGrossPNL";
        public static string COL_ClosingStatus = "ClosingStatus";
        public static string COL_AssetDerivative = "AssetDerivative";
        public const string IS_STAGE_REQUIRED = "IsStageRequired";
        public const string IS_MANUAL_ORDER = "IsManualOrder";
        public const string COL_BloombergExCode = "BloombergSymbolWithExchangeCode";
        public const string Header_BloombergExCode = "BloombergSymbol(WithExchangeCode)";
        #endregion


        #region Column Constants for ApprovedChangesMapping
        public const string ApprovedChangesColumnQuantity = "Quantity";
        public const string ApprovedChangesColumnAvgPX = "AvgPX";
        #endregion


    }
}