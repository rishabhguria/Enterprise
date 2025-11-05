using Prana.BusinessObjects;

namespace Prana.Rebalancer
{
    class RebalancerConstants
    {
        #region compliance fields, case sensitive
        public const string CONST_Symbol = "symbol";
        public const string CONST_Side = "defaultPositionSide";
        public const string CONST_AccountId = "accountId";
        public const string CONST_AssetId = "assetId";
        public const string COL_MasterFund = "MasterFund";
        public const string COL_MasterFundID = "masterFundId";
        public const string COL_MasterFundName = "masterFundName";
        public const string CONST_YesterdayMarketValueBase = "yesterdayMarketValueBase";
        public const string CONST_DayPnLBase = "dayPnLBase";
        public const string CONST_Quantity = "quantity";
        #endregion


        public const string CONST_Total = "Total";
        public const string CONST_AlertTitle = "Work Area Alert";
        public const string CONST_AddedTransaction = "Added Transaction";
        public const string CONST_DataRowSortingConstant = "0";
        public const string CONST_AddedTransactionSortingConstant = "1";
        public const string CONST_SummaryRowSortingConstant = "2";
        public const string GRP_SuggestedChanges = "SuggestedChanges";
        public const string GRP_FinalizeChanges = "FinalizeChanges";
        public const string CAP_SuggestedChanges = "Suggested Changes";
        public const string CAP_FinalizeChanges = "Finalize Changes";


        public const string CompressorKey_MasterFund = "MasterFund";
        public const string CompressorKey_Account = "Account";
        public const string COL_ID = "ID";
        public const string COL_ExcludeType = "ExcludeType";
        public const string COL_Key = "Key";
        //public const string COL_SortSymbol = "SortSymbol";
        public const string COL_SortKey = "SortKey";
        public const string COL_Symbol = "Symbol";
        public const string COL_IsChecked = "IsChecked";
        public const string COL_Caption = "Caption";
        public const string COL_Quantity = "Quantity";
        public const string COL_OriginalNAV = "OriginalNAV";

        public const string COL_CurrentQuantity = "CurrentQuantity";
        public const string COL_NewQuantity = "NewQuantity";
        public const string COL_MasterFundNAV = "startOfDayNavMasterFund";
        public const string COL_NewNAV = "NewNAV";
        public const string COL_AccountID = "AccountID";
        public const string COL_AccountNAV = "startOfDayNavAccount";
        public const string COL_AccountName = "AccountName";
        public const string COL_ThirdPartyID = "ThirdPartyID";
        public const string COL_ThirdPartyName = "ThirdPartyName";


        public const string COL_Side = "Side";
        public const string COL_NetChange = "NetChange";
        public const string COL_ChangeType = "ChangeType";
        public const string COL_CurrencyID = "CurrencyID";
        public const string COL_CurrencySymbol = "CurrencySymbol";
        public const string COL_Price = "Price";
        public const string COL_FXRate = "FxRate";
        public const string COL_FXConversionMethodOperator = "FXConversionMethodOperator";
        public const string COL_MarkPrice = "MarkPrice";
        public const string COL_TradeCurrency = "TradeCurrency";
        public const string COL_IsCompliancePass = "IsCompliancePass";
        public const string COL_IsAddedTransaction = "IsAddedTransaction";
        public const string COL_IsSwapTrade = "IsSwapTrade";
        public const string COL_IsSwapped = "IsSwapped";
        public const string COL_TransferThirdPartyID = "TransferThirdPartyID";
        public const string COL_TransferThirdPartyName = "TransferThirdPartyName";
        public const string COL_OverrideChange = "OverrideChange";
        public const string COL_ChangeOrTrade = "ChangeOrTrade";
        public const string COL_SummaryRowIndex = "SummaryRowIndex ";
        public const string COL_DataRowIndexes = "DataRowIndexes ";
        public const string COL_RowID = "RowID";
        public const string COL_OrderKey = "OrderKey";
        public const string COL_OriginalPercentageContribution = "OriginalPercentageContribution";
        public const string COL_NewCashFlow = "NewCashFlow";
        public const string COL_NewPercentageContribution = "NewPercentageContribution";
        public const string COL_TotalOriginalNAV = "TotalOriginalNAV";
        public const string COL_TotalNewNAV = "TotalNewNAV";
        public const string COL_RemainingCash = "RemainingCash";
        public const string COL_AUECID = "AUECID";
        public const string COL_TransactionType = "TransactionType";
        public const string COL_DollarAmountOfTrades = "DollarAmountOfTrades";
        public const string COL_CashFlow = "CashFlow";
        public const string COL_TotalDollarAmountOfTrades = "TotalDollarAmountOfTrades";
        public const string COL_CashRemainingFromCashFlow = "CashRemainingFromCashFlow";
        public const string COL_Security = "Security";
        public const string COL_TradeDate = "TradeDate";
        public const string COL_ContractMultiplier = "ContractMultiplier";
        public const string COL_AssetID = "AssetID";
        public const string COL_TotalOriginalQuantity = "TotalOriginalQuantity";
        public const string COL_TotalNewQuantity = "TotalNewQuantity";
        public const string COL_SideMultiplier = "SideMultiplier";
        public const string COL_OtherAssetsMarketValue = "OtherAssetsMarketValue";
        public const string COL_CashInBaseCurrency = "CashInBaseCurrency";
        public const string COL_AccrualsInBaseCurrency = "AccrualsInBaseCurrency";
        public const string COL_SwapNavAdjustment = "SwapNavAdjustment";
        public const string COL_UnRealizedPnlOfSwaps = "UnRealizedPnlOfSwaps";

        public const string CAP_IsExcluded = "Is Excluded";
        public const string CAP_Currency = "Currency";
        public const string CAP_PositionSide = "Position Side";
        public const string CAP_Security = "Security";
        public const string CAP_ThirdParty = "Third Party";
        public const string CAP_OriginalNAV = "Original NAV";
        public const string CAP_OriginalPercentageContribution = "Original % Contribution";
        public const string CAP_NewCashFlow = "New Cash Flow (+/-)";
        public const string CAP_NewNAV = "New NAV";
        public const string CAP_NewPercentageContribution = "New  % Contribution";
        public const string CAP_TotalOriginalNAV = "Total Original NAV";
        public const string CAP_TotalNewNAV = "Total New NAV";
        public const string CAP_ChangeOrTrade = "Change or Trade";
        public const string CAP_RemainingCash = "Remaining Cash";
        public const string CAP_TransactionType = "Transaction Type";
        public const string CAP_Quantity = "Quantity";
        public const string CAP_DollarAmountOfTrades = "Dollar Amount Of Trades";
        public const string CAP_Price = "Price";
        public const string CAP_MasterFund = "Master Fund";
        public const string CAP_CashFlow = "Cash Flow";
        public const string CAP_TotalDollarAmountOfTrades = "Total Dollar Amount Of Trades";
        public const string CAP_CashRemainingFromCashFlow = "Cash Remaining From CashFlow";
        public const string CAP_ChangeType = "Change Type";
        public const string CAP_TradeDate = "Trade Date";
        public const string CAP_TotalOriginalQuantiy = "Total Original Quantity";
        public const string CAP_TotalNewQuantiy = "Total New Quantity";
        public const string CAP_VIEWALLOCATIONCAPTION = "View Allocation Details";
        public const string CAP_NIRVANACAPTION = "Nirvana";
        public const string CAP_NIRVANA_ALERTCAPTION = "Nirvana Alert";
        public const string CAP_PERCENTTRADINGTOOL = "% Trading Tool";

        public const string CONST_WORK_AREA_PREFERENCE_ERROR = "Work Area Preference Error";
        public const string CONST_SIDE_LONG = "LONG";
        public const string CONST_SIDE_SHORT = "SHORT";
        public const string CONST_COMMA_AND_PRECISION_FORMATER_NAV = "#,0.##";
        public const string CONST_COMMA_AND_PRECISION_FORMATER_PRICE = "#,0.####";
        public const string CONST_COMMA_AND_PRECISION_FORMATER_QUANTITY = "#,##0";
        public const string CONST_IsLock = "IsLock";
        public const string CONST_IsSymbolValid = "IsSymbolValid";
        public const string CONST_Locked = "Locked";
        public const string CONST_Valid = "Valid";
        public const string CONST_InValid = "InValid";
        public const string CONST_UnLocked = "Unlocked";
        public const string CONST_RebalPricingFeld = "RebalPricingField";
        public const string CONST_RebalAccountGroupVisibilityPref = "RebalAccountGroupVisibilityPref";
        public const string CONST_AssetClass = "AssetClass";
        public const string CONST_OtherItemsImpactingNAV = "OtherItemsImpactingNAV";
        public const string COL_Target = "Target";
        public const string COL_AccountId = "AccountId";
        public const string COL_TargetPercentage = "TargetPercentage";
        public const string COL_ToleranceBPS = "ToleranceBPS";
        public const string COL_TolerancePercentage = "TolerancePercentage";
        public const string Grid_ModelPortfolioGrid = "ModelPortfolioGrid";
        public const string CONST_None = "None";
        public const string CONST_RebalRASPrefrence = "RASPrefrence";
        public const string CONST_RebalUseRoundLot = "RebalUseRoundLot";
        public const string CONST_RebalTradingRulesPref = "RebalTradingRulesPref";
        public const string CONST_RebalExpandAcrossSecurities = "RebalExpandAcrossSecurities";
        public const string CONST_RebalShowSaveTradePopup = "RebalShowSaveTradePopup";
        public const string CONST_RebalRoundingType = "RebalRoundingType";
        public const string CONST_RebalImportSymbologyPreference = "RebalImportSymbologyPreference";

        public const string MSG_LIVE_FEED_DISCONNECTED = "Unable to fetch price from Live Feed as live feed is not connected.";
        public const string MSG_WAITING_FOR_VALIDATION = "Waiting for symbol validation";
        public const string MSG_FETCHINGPOSITIONS = "Fetching positions..";
        public const string MSG_NOPOSITIONSTOSHOW = "No positions to show.";
        public const string MSG_CalculatingData = "Calculating data..";

        public const string ERR_NON_PERMITTED_ASSET = "Asset class {0} not permitted.";

        #region Error strings for securities in RB import module.
        public const string ERR_SECURITY_PRICE_LESS_OR_EQUALS_ZERO = "Price should be greater than zero.";
        public const string ERR_SECURITY_FX_LESS_OR_EQUALS_ZERO = "FX should be greater than zero.";
        public const string ERR_SECURITY_TARGET_LESS_OR_EQUALS_ZERO = "Target should be greater than zero.";
        public const string ERR_SECURITY_TARGET_EXCEEDS_100 = "Target should be less than 100%.";
        public const string ERR_SECURITY_TICKER_NOT_PRESENT = "Ticker Symbol can not be blank.";
        public const string ERR_SECURITY_ACCOUNT_NOT_EXISTS = "Account does not exist.";
        public const string ERR_SECURITY_ACCOUNT_NOT_EXISTS_SELECTED_ACCOUNTORGROUP = "Account does not exist in selected AccountOrGroup.";
        public const string ERR_SECURITY_ACCOUNT_SUM_EXCEEDS_100 = "Account's target sum exceeds 100.";
        #endregion

        public const string CONST_CASH = "Cash";
        public const string CONST_ACCRUALS = "Accruals";
        public const string CONST_OTHER_ASSETS_MARKET_VALUE = "Other Assets Market Value";
        public const string CONST_SWAP_NAV_ADJUSTMENT = "Swap NAV adjustment";
        public const string CONST_NAV_ADJUSTEMENT_Factor = "NAV Adjustment Factor";
        public const string CONST_SWAP_UNREALIZED_PNL = "Unrealized P&L of Swaps";
        public const string CONST_AlphaNumericRegex = "^(?![0-9]*$)[a-zA-Z0-9]+$";
        public const string CONST_CustomGroupsTab = "CustomGroupsTab";
        public const string CONST_RebalancerTab = "RebalancerTab";
        public const string CONST_ModelPortfolioTab = "ModelPortfolioTab";
        public const string CONST_DataPreferenceTab = "DataPreferenceTab";
        public const string CONST_SET = "Set";
        public const string CONST_CASHSymbol = "CASH";
        public const string CONST_NotApplicable = "N.A.";

        public const string CONST_ProRata = "Pro-Rata";
        public const string MSG_CANNOT_VALIDATE_COMPLIANCE_RULES = "Could not validate compliance rules";
        public const string MSG_NO_ITEM_CHECKED = "Please select at least one record to check compliance";
        public const string MSG_NOTHING_TO_CHECK = "Nothing to \"Compliance Check.\"";
        public const string MSG_NO_RULES_VIOLATED = "No compliance rules were violated";
        public const string MSG_PREF_NOT_DEFINED = "Trading Ticket preferences not set, Please set trading ticket preferences";
        public const string MSG_COMPLIANCE_VALIDATED = "Compliance validated successfully for {0} orders.";
        public const string MSG_COMPLIANCE_NOT_VALIDATED = "Compliance could not be validated for {0} orders.";
        public const string MSG_TRADING_RULES_VALIDATION = "Allow negative cash and Set cash target/Sell to raise cash cannot be selected together";
        public const string MSG_CASH_TARGET_PERCENT_AND_RAS_VALIDATION = "Set cash target % + Rebalance across securities target % cannot be greater than 100";
        public const string MSG_CASH_TARGET_PERCENT_VALUE_VALIDATION = "Set Cash Target is Blank. Please input and retry";
        public const string MSG_CASH_TARGET_PERCENT_AND_MODEL_VALIDATION = "Set cash target% cannot be used along with Model portfolio";
        public const string MSG_CASH_TARGET_PERCENT_VALIDATION = "This requires additional cash. Thus sell to raise cash should be on.";
        public const string MSG_CASH_LOCK_VALIDATION =  "Cannot rebalance the portfolio because Cash is locked.";
        public const string MSG_TARGET_PERCENT_SUM_VALIDATION = "Cannot Rebalance because sum of target % doesn't sum up to 100%";
        //TODO: This method should not be here
        internal static string GetWorkAreaTradeKeyFromItem(string workAreaItemKey)
        {
            return workAreaItemKey.Split(Seperators.SEPERATOR_6)[0] + Seperators.SEPERATOR_6 + workAreaItemKey.Split(Seperators.SEPERATOR_6)[1];
        }
    }
}
