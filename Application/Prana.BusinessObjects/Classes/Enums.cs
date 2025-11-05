using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Prana.BusinessObjects.AppConstants
{
    /// <summary>
    /// TODO : Use this Enum at all places to differentiate among assetcategory
    /// Author : Rajat.
    /// Always add any value in the last so that it don't affect already saved values.
    /// Forex category is only used to handle pricing for fxRates.
    /// </summary>
    public enum AssetCategory
    {
        [XmlEnumAttribute("0")]
        None,
        [XmlEnumAttribute("1")]
        Equity,
        [XmlEnumAttribute("2")]
        EquityOption,
        [XmlEnumAttribute("3")]
        Future,
        [XmlEnumAttribute("4")]
        FutureOption,
        [XmlEnumAttribute("5")]
        FX,
        [XmlEnumAttribute("6")]
        Cash,
        [XmlEnumAttribute("7")]
        Indices,
        [XmlEnumAttribute("8")]
        FixedIncome,
        [XmlEnumAttribute("9")]
        PrivateEquity,
        [XmlEnumAttribute("10")]
        FXOption,
        [XmlEnumAttribute("11")]
        FXForward,
        [XmlEnumAttribute("12")]
        Forex,
        [XmlEnumAttribute("13")]
        ConvertibleBond,                        //Works similar as Options - Bond type which have maturity of greater than 10 years (http://jira.nirvanasolutions.com:8080/browse/PRANA-3666)
        [XmlEnumAttribute("14")]
        CreditDefaultSwap,                       //Works similar as Equities - Agreement that the seller of the CDS will compensate the buyer in the event of a loan default or other credit event (http://jira.nirvanasolutions.com:8080/browse/PRANA-3949)
        [XmlEnumAttribute("101")]
        Option
    }

    public enum DerivedAssetCategory
    {

        [XmlEnumAttribute("0")]
        None,
        [XmlEnumAttribute("1")]
        EquitySwap
    }

    public enum DataSourceFileLayout : ushort
    {
        None = 0,
        SummaryHeaderData = 1,
        HeaderData = 2,
        NoHeaderData = 3,
        HeaderBlankData = 4
    }

    /// <summary>
    /// Rajat Added - Corrupted - 25 Nov 2006
    /// </summary>
    public enum RunUploadStatus
    {
        [XmlEnumAttribute("0")]
        Awaiting,
        [XmlEnumAttribute("1")]
        Failed,
        [XmlEnumAttribute("2")]
        Corrupted,
        [XmlEnumAttribute("3")]
        Successful
    }

    /// <summary>
    /// Author : Rajat 
    /// 09/10/2006
    /// </summary>
    public enum Underlying
    {
        None = 0,
        US = 1,
        EURO = 2,
        JAPAN = 3,
        ASIAXJAPAN = 4,
        Brazil = 5,
        //Spot, // no use
        //Forwards,// no use
        SA = 8, // South Africa
        Australia = 9,
        EmergingDebt = 10,
        OTCDerivatives = 11,
        Multiple = 12,
        LatinAmerica = 13,
        MiddleEast = 14
    }

    public enum PositionCategory
    {
        Account,
        Strategy
    }

    public enum SortingOrder
    {
        Ascending,
        Descending
    }

    public enum AccountSide
    {
        DR,
        CR
    }

    public enum SubAccounts
    {
        Invt_Equity_Long,
        Invt_Equity_Short,
        Invt_Future_Long,
        Invt_Future_Short,
        Invt_Option_Long,
        Invt_Option_Short,
        Invt_Bond_Long,
        Invt_Bond_Short,
        M2M_Income,
        M2M_Expense,
        Interest_Income,
        Interest_Expense,
        Interest_Receivable,
        Dividend_Receivable,
        Dividend_Income,
        Dividend_Payable,
        Dividend_Expense,
        Cash,
        Total_Commission,
        Total_Fee,
        Margin,
        Margin_Liability,
        M2M_Margin,
        Account_Payble,
        Account_Recievable,
        Stamp_Duty,
        Clearing_Fee,
        Misc_Fees,
        Tax_On_Commissions,
        Transaction_Levy,
        Other_Broker_Fees,
        Commission,
        Invt_EquitySwap_Long,
        Invt_EquitySwap_Short,
        FX_Gain,
        FX_Loss,
        Invt_FX_Long,
        Invt_FX_Short,
        trManualCashPCR,
        Owner_Equity,
        AccrualsUnRealizedPNL,
        Sec_Fee,
        Occ_Fee,
        Orf_Fee,
        Soft_Commission,
        Clearing_Broker_Fee,
        OptionPremiumAdjustment
    }

    public enum Activities
    {
        EquityL,
        EquityS,
        FutureL,
        FutureS,
        EquityOptionL,
        EquityOptionS,
        BondL,
        BondS,
        FXL,
        FXS,
        Bond_Interest_Received,
        Bond_Interest_Paid,
        Bond_Interest_Receivable,
        Bond_Interest_Payable,
        Bond_Interest_Expense,
        Dividend_Accrued_Long,
        Dividend_Accrued_Short,
        Dividend_Settled_Long,
        Dividend_Settled_Short,
        EquitySwapL,
        EquitySwapS,
        FX_Settled,
        FXL_CurrencySettled,
        FXS_CurrencySettled,
        ManualCash_In,
        ManualCash_Out,
        Misc_Fees_Short,
        Misc_Fees_Long,
        Future_Settled,
        FuturesProfitAndLoss,
        FXProfitAndLoss,
        FutureOptionL,
        FutureOptionS,
        FXOptionL,
        FXOptionS,
        CashTransfer,
        AccrualsRevaluation,
        CDSLong,
        CDSShort,
        ConvertibleBondL,
        ConvertibleBondS,
        PrivateEquityL,
        PrivateEquityS,
        Select,
        FXForwardL,
        FXForwardS,
        DividendIncome,
        DividendExpense,
        WithholdingTax,
        M2MProfit,
        M2MLoss,
        EquityLongAddition,
        EquityLongWithdrawal,
        EquityShortAddition,
        EquityShortWithdrawal,
        EquityOptionLongAddition,
        EquityOptionLongWithdrawal,
        EquityOptionShortAddition,
        EquityOptionShortWithdrawal,
        FutureLongAddition,
        FutureLongWithdrawal,
        FutureShortAddition,
        FutureShortWithdrawal,
        FutureOptionLongAddition,
        FutureOptionLongWithdrawal,
        FutureOptionShortAddition,
        FutureOptionShortWithdrawal,
        FXLongAddition,
        FXLongWithdrawal,
        FXShortAddition,
        FXShortWithdrawal,
        BondLongAddition,
        BondLongWithdrawal,
        BondShortAddition,
        BondShortWithdrawal,
        PrivateEquityLongAddition,
        PrivateEquityLongWithdrawal,
        PrivateEquityShortAddition,
        PrivateEquityShortWithdrawal,
        FXOptionLongAddition,
        FXOptionLongWithdrawal,
        FXOptionShortAddition,
        FXOptionShortWithdrawal,
        FXForwardLongAddition,
        FXForwardLongWithdrawal,
        FXForwardShortAddition,
        FXForwardShortWithdrawal,
        ConvertibleBondLongAddition,
        ConvertibleBondLongWithdrawal,
        ConvertibleBondShortAddition,
        ConvertibleBondShortWithdrawal,
        CDSLongAddition,
        CDSLongWithdrawal,
        CDSShortAddition,
        CDSShortWithdrawal,
        EquitySwapLongAddition,
        EquitySwapLongWithdrawal,
        EquitySwapShortAddition,
        EquitySwapShortWithdrawal,
        OptionPremiumAdjustment_Short,
        OptionPremiumAdjustment_Long,
        Bond_Trading_Interest,
        Bond_Trading_Payable,
        FxForwardL_CurrencySettled,
        FxForwardS_CurrencySettled,
        FxForward_Settled,
        FxForwardLongLT_Settled,
        FxForwardLongST_Settled,
        FxForwardShortST_Settled
    }

    public enum CashTransactionType
    {
        [EnumDescription("Trading")]
        Trading = 1,
        [EnumDescription("Manual Journal Entry")]
        ManualJournalEntry = 2,
        [EnumDescription("Daily Calculation")]
        DailyCalculation = 3,
        [EnumDescription("Corporate Action")]
        CorpAction = 4,
        [EnumDescription("Cash Transaction")]
        CashTransaction = 5,
        [EnumDescription("Importable & Editable Data")]
        ImportedEditableData = 6,
        [EnumDescription("Closing")]
        Closing = 7,
        [EnumDescription("Opening Balance")]
        OpeningBalance = 8,
        [EnumDescription("Revaluation")]
        Revaluation = 9,
        [EnumDescription("Unwinding")]
        Unwinding = 10,
        [EnumDescription("Settlement Transaction")]
        SettlementTransaction = 11,
        [EnumDescription("Trade Import")]
        TradeImport = 12
    }

    public enum ActivitySource
    {
        NonTrading = 0,
        Trading = 1,
        Dividend = 2,
        Revaluation = 3,
        OpeningBalance = 4
    }

    public enum BalanceType
    {
        Cash = 1,
        Accrual = 2,
    }

    public enum ActivityDateType
    {
        ExDate = 1,
        PayoutDate = 2
    }

    public enum TransactionType
    {
        NTRA = 1,
        ACB = 2,
        TRA = 3,
        ICT = 4,
        AccountTransfer = 5,
        CashAccountTransfer = 6,
        Cash = 7
    }

    // As transaction type already exists and used in cash management, so a new enum is added
    public enum TradingTransactionType
    {
        Trade,
        Expire,
        Exercise,
        Assignment,
        CSCost,
        DLCost,
        DLCostAndPNL,
        CSClosingPx,
        CSZero,
        CSSwpRl,
        CSSwp,
        LongAddition,
        LongWithdrawal,
        ShortAddition,
        ShortWithdrawal,
        LongCostAdj,
        ShortCostAdj,
        LongWithdrawalCashInLieu,
        ShortWithdrawalCashInLieu,
        Buy,
        Sell,
        SellShort,
        BuytoCover,
        BuytoClose,
        BuytoOpen,
        SelltoOpen,
        SelltoClose
    }

    public enum TransactionSource
    {
        [EnumDescription("None")]
        None,
        [EnumDescription("FIX")]
        FIX,
        [EnumDescription("TradingTicket")]
        TradingTicket,
        [EnumDescription("TradeImport")]
        TradeImport,
        [EnumDescription("CreateTransactionUI")]
        CreateTransactionUI,
        [EnumDescription("Closing")]
        Closing,
        [EnumDescription("CAStockDividend")]
        CAStockDividend, //Stock Dividend
        [EnumDescription("CAStockMerger")]
        CAStockMerger, //Stock Merger
        [EnumDescription("CAStockNameChange")]
        CAStockNameChange, //Stock Name Change
        [EnumDescription("CAStockSpinoff")]
        CAStockSpinoff, //Stock Spin-off
        [EnumDescription("CAStockSplit")]
        CAStockSplit, //Stock Split
        [EnumDescription("CAStockExchange")]
        CAStockExchange, //Stock Echange       
        [EnumDescription("CostAdjustment")]
        CostAdjustment,  //Cost Adjustment
        [EnumDescription("PST")]
        PST,
        [EnumDescription("Rebalancer")]
        Rebalancer,
        [EnumDescription("ShortLocate")]
        ShortLocate,
        [EnumDescription("Multi Trading Ticket")]
        MultiTradingTicket,
        [EnumDescription("Single Trade")]
        SingleTrade,
        [EnumDescription("PM")]
        PM,
        [EnumDescription("Blotter")]
        Blotter,
        [EnumDescription("Hot Button")]
        HotButton,
    }

    public enum PMType
    {
        None = 0,
        Increase = 1,
        Close = 2
    }

    public enum CashValueType
    {
        Positive = 0,
        Negative = 1,
    }

    /// <summary>
    /// Selected Feed Price is used to tell on which basis exposure and Pnl are going to be calculated
    /// i.e, Ask, Bid, Last or Mid
    /// </summary>
    public enum SelectedFeedPrice
    {
        Ask,
        Bid,
        Last,
        Previous,
        Mid,
        iMid,
        None,
        AskOrBid
    }

    public enum PositionTag
    {
        [XmlEnumAttribute("0")]
        Long,
        [XmlEnumAttribute("1")]
        Short,
        [XmlEnumAttribute("2")]
        None,
        [XmlEnumAttribute("3")]
        LongAddition,
        [XmlEnumAttribute("4")]
        LongWithdrawal,
        [XmlEnumAttribute("5")]
        ShortAddition,
        [XmlEnumAttribute("6")]
        ShortWithdrawal,
        [XmlEnumAttribute("7")]
        //LongNotionalChange,
        LongCostAdj,
        [XmlEnumAttribute("8")]
        //ShortNotionalChange,
        ShortCostAdj,
        [XmlEnumAttribute("9")]
        //LongWithdrawalCashInLieu,
        LongWithdrawalCashInLieu,
        [XmlEnumAttribute("10")]
        //ShortWithdrawalCashInLieu
        ShortWithdrawalCashInLieu
    }

    public enum PositionType
    {
        [XmlEnumAttribute("0")]
        Long,
        [XmlEnumAttribute("1")]
        Short,
        [XmlEnumAttribute("2")]
        Multiple,
        [XmlEnumAttribute("3")]
        Boxed,
        [XmlEnumAttribute("4")]
        FX,
    }

    public enum FeedPriceChecks
    {
        [XmlEnumAttribute("0")]
        LastWhenMidZero,
        [XmlEnumAttribute("1")]
        LastWhenAskOrBidOrMidZero,
        [XmlEnumAttribute("2")]
        AskIfLastIsZero,
        [XmlEnumAttribute("3")]
        BidIfLastIsZero,
        [XmlEnumAttribute("4")]
        MidIfLastIsZero,
        [XmlEnumAttribute("5")]
        iMidIfLastIsZero,
        [XmlEnumAttribute("6")]
        Never
    }

    public enum PostionStatus
    {
        [XmlEnumAttribute("0")]
        Open,
        [XmlEnumAttribute("1")]
        Closed
    }

    public enum ExPNLSubscriptionType
    {
        None,
        //TradesOnly,
        TradesTaxlotsAndPositions
    }

    public enum ExPNLData
    {
        None,
        Account,
        MasterFund,
        Strategy
    }

    public enum ExPNLPreferenceMsgType
    {
        NewPreferences,
        SelectedColumnAdded,
        SelectedColumnDeleted,
        GroupByColumnAdded,
        GroupByColumnDeleted,
        SelectedViewChanged,
        CustomViewDeleted,
        CustomViewAdded,
        SelectedViewCopied,
        FilterValueChanged
    }

    public enum SymbolConventionenum
    {
        None,
        PranaSymbol,
        RIC,
        CUSIP,
        SEDOL,
        ISIN,
        OSIOption,
        IDCOOption
    }

    public enum TradeType
    {
        [XmlEnumAttribute("0")]
        [EnumDescription("Single Trade")]
        SingleTrade,
        [XmlEnumAttribute("1")]
        [EnumDescription("Basket Trade")]
        BasketTrade,
        [XmlEnumAttribute("2")]
        [EnumDescription("Single Trade and Basket Trade")]
        Both
    }

    public enum CommisionSource
    {
        [XmlEnumAttribute("0")]
        Manual,
        [XmlEnumAttribute("1")]
        Auto
    }

    public enum CommissionType
    {
        [XmlEnumAttribute("0")]
        Commission,                     //Treat as HardCommission
        [XmlEnumAttribute("1")]
        SoftCommission
    }

    public enum ClearingFeeType
    {
        [XmlEnumAttribute("0")]
        Broker,
        [XmlEnumAttribute("1")]
        Other
    }

    public enum CalculationBasis
    {
        [XmlEnumAttribute("0")]
        [EnumDescription("Shares/Contracts")]
        Shares,
        [XmlEnumAttribute("1")]
        [EnumDescription("Notional BPS")]
        Notional,
        [XmlEnumAttribute("2")]
        [EnumDescription("Contracts")]
        Contracts,
        [XmlEnumAttribute("3")]
        [EnumDescription("Avg Price")]
        AvgPrice,
        [XmlEnumAttribute("4")]
        [EnumDescription("Commission")]
        Commission,
        [XmlEnumAttribute("5")]
        [EnumDescription("Notional Plus Commission")]
        NotionalPlusCommission,
        [XmlEnumAttribute("6")]
        [EnumDescription("Flat Amount")]
        FlatAmount,
        [XmlEnumAttribute("7")]
        [EnumDescription("Flat Amount ProRata")]
        FlatRateProrata,
        [XmlEnumAttribute("8")]
        [EnumDescription("Auto")]
        Auto,
        [XmlEnumAttribute("9")]
        [EnumDescription("Soft Commission")]
        SoftCommission
    }
    public enum CalculationFeeBasis
    {
        [XmlEnumAttribute("0")]
        [EnumDescription("Shares/Contracts")]
        Shares,
        [XmlEnumAttribute("1")]
        [EnumDescription("Notional BPS")]
        Notional,
        [XmlEnumAttribute("2")]
        [EnumDescription("Contracts")]
        Contracts,
        [XmlEnumAttribute("3")]
        [EnumDescription("Avg Price")]
        AvgPrice,
        [XmlEnumAttribute("4")]
        [EnumDescription("Commission")]
        Commission,
        [XmlEnumAttribute("5")]
        [EnumDescription("Notional Plus Commission")]
        NotionalPlusCommission,
        [XmlEnumAttribute("6")]
        [EnumDescription("Flat Amount")]
        FlatAmount,
        [XmlEnumAttribute("7")]
        [EnumDescription("Flat Amount ProRata")]
        FlatRateProrata,
        [XmlEnumAttribute("8")]
        [EnumDescription("Auto")]
        Auto,
        [XmlEnumAttribute("9")]
        [EnumDescription("Soft Commission")]
        SoftCommission,
        [XmlEnumAttribute("10")]
        [EnumDescription("Other Broker Fee")]
        OtherBrokerFee,
        [XmlEnumAttribute("11")]
        [EnumDescription("Clearing Broker Fee")]
        ClearingBrokerFee,
        [XmlEnumAttribute("12")]
        [EnumDescription("Stamp Duty")]
        StampDuty,
        [XmlEnumAttribute("13")]
        [EnumDescription("Transaction Levy")]
        TransactionLevy,
        [XmlEnumAttribute("14")]
        [EnumDescription("Clearing Fee")]
        ClearingFee,
        [XmlEnumAttribute("15")]
        [EnumDescription("Tax On Commissions")]
        TaxOnCommissions,
        [XmlEnumAttribute("16")]
        [EnumDescription("Misc Fees")]
        MiscFees,
        [XmlEnumAttribute("17")]
        [EnumDescription("Sec Fee")]
        SecFee,
        [XmlEnumAttribute("18")]
        [EnumDescription("Occ Fee")]
        OccFee,
        [XmlEnumAttribute("19")]
        [EnumDescription("Orf Fee")]
        OrfFee
    }

    /// <summary>
    /// 'M' stands for multiply and D stands for divide in FIX
    /// </summary>

    public enum Operator
    {
        M,
        D,
        Multiple
    }

    public enum ReconType
    {
        Transaction = 0,
        Position = 1,
        PNL = 2,
        TaxLot = 3
    }

    public enum ReconDateType
    {
        TradeDate = 0,
        ProcessDate = 1,
        NirvanaProcessDate = 2
    }

    public enum ClosingType
    {
        Closing = 0,
        UnWinding = 1
    }

    public enum Summary
    {
        None = 0,
        Sum = 1,
        Max = 2,
        Min = 3,
        WeightedAvg = 4
    }

    //Any change in the enums below will have repurcussions on reports. So any one changing here would need
    //to tell the reporting guys about the change.
    public enum ClosingMode
    {
        [XmlEnumAttribute("0")]
        Offset,
        [XmlEnumAttribute("1")]
        Cash,
        [XmlEnumAttribute("2")]
        Exercise,
        [XmlEnumAttribute("3")]
        Expire,
        [XmlEnumAttribute("4")]
        Physical,
        [XmlEnumAttribute("5")]
        SwapExpireAndRollover,
        [XmlEnumAttribute("6")]
        SwapExpire,
        [XmlEnumAttribute("7")]
        CorporateAction,
        [XmlEnumAttribute("8")]
        None,
        [XmlEnumAttribute("9")]
        CashSettleinBaseCurrency, // for NDF fxForwards
        [XmlEnumAttribute("10")]
        CostBasisAdjustment			//for CostAdjustment
    }

    public enum ClosingStatus
    {
        [XmlEnumAttribute("0")]
        Open,
        [XmlEnumAttribute("1")]
        PartiallyClosed,
        [XmlEnumAttribute("2")]
        Closed
    }

    public enum CorporateActionType
    {
        [XmlEnumAttribute("0")]
        [EnumDescription("All")]
        All,
        [XmlEnumAttribute("1")]
        [EnumDescription("Exchange")]
        Exchange,
        [XmlEnumAttribute("2")]
        [EnumDescription("Merger")]
        Merger,
        [XmlEnumAttribute("3")]
        [EnumDescription("SpinOff")]
        SpinOff,
        [XmlEnumAttribute("4")]
        [EnumDescription("Name Change")]
        NameChange,
        [XmlEnumAttribute("5")]
        [EnumDescription("Stock Dividend")]
        StockDividend,
        [XmlEnumAttribute("6")]
        [EnumDescription("Split")]
        Split,
        [XmlEnumAttribute("7")]
        [EnumDescription("Cash Dividend")]
        CashDividend
    }

    public enum OtherFeeType
    {
        [XmlEnumAttribute("0")]
        StampDuty,
        [XmlEnumAttribute("1")]
        TransactionLevy,
        [XmlEnumAttribute("2")]
        ClearingFee,
        [XmlEnumAttribute("3")]
        TaxOnCommissions,
        [XmlEnumAttribute("4")]
        MiscFees,
        [XmlEnumAttribute("5")]
        SecFee,
        [XmlEnumAttribute("6")]
        OccFee,
        [XmlEnumAttribute("7")]
        OrfFee
    }

    public enum BrokerLevelFeeType
    {
        [XmlEnumAttribute("0")]
        OtherBrokerFee,
        [XmlEnumAttribute("1")]
        ClearingBrokerFee
    }

    public enum FeePrecisionType
    {
        /// <summary>
        /// RoundOff means numeric rounding up to nearest number
        /// </summary>
        [XmlEnumAttribute("0")]
        RoundOff,
        [XmlEnumAttribute("1")]
        RoundUp,
        [XmlEnumAttribute("2")]
        RoundDown
    }

    /// <summary>
    /// The following IDs are assigned on instanciation of a class in EPNL and used to check
    /// which type of class it is so that appropriate action is taken
    /// </summary>
    public enum EPnLClassID
    {
        EPnlOrder = 0,
        EPnLOrderEquity = 1,
        EPnLOrderOption = 2,
        EPnLOrderFuture = 3,
        EPnLOrderFX = 4,
        EPnLOrderEquitySwap = 5,
        EPnLOrderFXForward = 6,
        EPnLOrderFixedIncome = 7
    }
    /// <summary>
    /// used in Third Party Taxlot State
    /// </summary>
    public enum PranaTaxLotState
    {
        Allocated = 0,
        Sent = 1,
        Amended = 2,
        Deleted = 3,
        Ignore = 4
    }

    public enum SwapValidate
    {
        Trade,
        Allocate,
        Expire,
        ExpireAndRollover
    }

    public enum OptionType
    {
        [XmlEnumAttribute("0")]
        PUT,
        [XmlEnumAttribute("1")]
        CALL,
        [XmlEnumAttribute("2")]
        NONE
    }

    public enum OptionTypeFilter
    {
        [XmlEnumAttribute("0")]
        CALL_PUT,
        [XmlEnumAttribute("1")]
        CALL,
        [XmlEnumAttribute("2")]
        PUT
    }

    public enum SymbolType
    {
        [XmlEnumAttribute("0")]
        New,
        [XmlEnumAttribute("1")]
        Updated,
        [XmlEnumAttribute("2")]
        Unchanged

    }

    /// <summary>
    /// 'L' stands for live feed i.e. from Mark Price UI and P stands for Prime Broker i.e Import Data UI
    /// </summary>
    public enum MarkPriceImportType
    {
        L,
        P
    }

    public enum ConnectedEntityTypes
    {
        Users,
        FixSessions,
        MiscConnection
    }

    public enum ControlType
    {
        Apply,
        Undo,
        Redo
    }

    public enum Duration
    {
        Day,
        Month,
        Quarter,
        Year
    }

    public enum PriceUsedType
    {
        Mid,
        Last,
        iMid
    }

    public enum PayReceiveChanges
    {
        AdjustAvgPrice = 1,
        AdjustQty = 2
    }

    [Serializable]
    public enum CollateralType
    {
        Undefined = 0,
        General_Collateral = 1,
        Hard_to_Borrow = 2
    }

    [Serializable]
    public enum AccrualBasis
    {
        [XmlEnumAttribute("0")]
        Actual_365,
        [XmlEnumAttribute("1")]
        Actual_360,
        [XmlEnumAttribute("2")]
        Accrual_30_360,
        [XmlEnumAttribute("3")]
        Accrual_30E_360,
        [XmlEnumAttribute("4")]
        Actual_Actual
    }

    [Serializable]
    public enum SecurityType
    {

        [XmlEnumAttribute("0")]
        Treasury,
        [XmlEnumAttribute("1")]
        Municipal,
        [XmlEnumAttribute("2")]
        Agency,
        [XmlEnumAttribute("3")]
        Corporate,
        [XmlEnumAttribute("4")]
        Sovereign,
        [XmlEnumAttribute("5")]
        CommercialPaper,
        [XmlEnumAttribute("6")]
        Credit,
        [XmlEnumAttribute("7")]
        BankDebt

    }

    [Serializable]
    public enum CouponFrequency
    {
        Monthly = 0,
        Quarterly = 1,
        SemiAnnually = 2,
        Annually = 3,
        None = 4
    }

    public enum CollateralCouponFrequency
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly
    }

    public enum SymbologyCodesForRecon
    {
        [XmlEnumAttribute("0")]
        Ticker,
        [XmlEnumAttribute("1")]
        OSIOption,
        [XmlEnumAttribute("2")]
        Bloomberg,
        [XmlEnumAttribute("3")]
        IDCOOption,
        [XmlEnumAttribute("4")]
        SEDOL,
        [XmlEnumAttribute("5")]
        CUSIP
    }

    public enum CalculateRiskBy
    {
        Individual,
        Group,
        Both
    }

    // used in risk Analysis for Non parallel shifts 
    [Serializable]
    public enum GroupShockBasis
    {
        PositionSide = 0,
        PositionSidePortfolio = 1,
        PositionSideExpPortfolio = 2,
        UDAAssets = 3,
        UDASectors = 4,
        UDASubSectors = 5,
        UDASecurityTypes = 6,
        UDACountries = 7
    }

    public enum ReconFilterType
    {
        PrimeBroker,
        MasterFund,
        Account,
        Asset,
        AUEC,
        CounterParty
    }

    public enum AllocationScheme
    {
        Import = 0,
        Edit = 1,
        Recon = 2
    }

    public enum AllocationSchemeKey
    {
        Symbol,
        SymbolSide,
        PBSymbolSide
    }

    public enum GroupAllocationStatus
    {
        UnAllocated,
        Allocated,
        ReAllocated,
        Hold
    }

    /// <summary>
    /// Used for recording Progress of Import.
    /// </summary>
    public enum Progress
    {
        Start,
        Running,
        End
    }

    /// <summary>
    /// The moneyness of an option contract
    /// </summary>
    public enum OptionMoneyness
    {
        [EnumDescription("NA")]
        NA,
        [EnumDescription("In the money")]
        ITM,
        [EnumDescription("At the money")]
        ATM,
        [EnumDescription("Out of the Money")]
        OTM
    }

    //modified by omshiv, 26 sep,14,added bloombergDLWS in Pricing sources 
    public enum PricingSource
    {
        [EnumDescription("New Order")]
        NewOrder,
        [EnumDescription("None")]
        None,
        [EnumDescription("Live Feed")]
        LiveFeed,
        [EnumDescription("Closing Mark")]
        ClosingMark,
        [EnumDescription("Stale Closing Mark")]
        StaleClosingMark,
        [EnumDescription("OMI")]
        OMI,
        [EnumDescription("User Defined")]
        UserDefined,
        [EnumDescription("Theoretical Price")]
        TheoreticalPrice,
        [EnumDescription("Live Feed Proxy Symbol")]
        LiveFeedProxySymbol,
        [EnumDescription("Closing Mark(PI)")]
        ClosingMarkPI,
        [EnumDescription("Stale Closing Mark(PI)")]
        StaleClosingMarkPI,
        [EnumDescription("Bloomberg DLWS")]
        BloombergDLWS,
        [EnumDescription("EOD T-1 Snapshot")]
        EODT_1Snapshot,
        [EnumDescription("Gateway")]
        Gateway,
        [EnumDescription("Import File (Bloomberg)")]
        ImportFileBloomberg,
        [EnumDescription("Import File (Custom)")]
        ImportFileCustom
    }

    // This enum provides source of Delta which is :-
    // in case of Non-Options : Undefined,
    // In case of options : Default (1 for Call, -1 for Put and 0 for Expired Options) or calculated.
    public enum DeltaSource
    {
        Default,
        Calculated,
        UserDefined
    }

    public enum PranaPricingSource
    {
        Esignal = 0,
        Bloomberg = 1,
        FactSet = 2,
        ACTIV = 3
    }

    public enum PricingStatus
    {
        [EnumDescription("None")]
        None = 0,
        [EnumDescription("Real-Time")]
        RealTime = 1,
        [EnumDescription("Delayed")]
        Delayed = 2
    }

    public enum MailType
    {
        DataFile,
        LogFile,
    }

    /// <summary>
    /// List (enum) of available verbose levels (NoVerbose, Verbose, VeryVerbose)
    /// </summary>
    public enum VerboseLevel
    {
        /// <summary>
        /// Reset verbose level to 0 (no information shown during processing)
        /// </summary>
        NoVerbose,
        /// <summary>
        /// Give more information during processing.
        /// </summary>
        Verbose,
        /// <summary>
        /// Give full information during processing (the input data is listed in detail).
        /// </summary>
        VeryVerbose
    };

    /// <summary>
    /// List (enum) of available commands (sign, encrypt, sign and encrypt, etc...)
    /// </summary>
    public enum Commands
    {
        /// <summary>
        /// Make a signature
        /// </summary>
        Sign,
        /// <summary>
        /// Encrypt  data
        /// </summary>
        Encrypt,
        /// <summary>
        /// Sign and encrypt data
        /// </summary>
        SignAndEncrypt,
        /// <summary>
        /// Decrypt data
        /// </summary>
        Decrypt,
        /// <summary>
        /// Assume that input is a signature and verify it without generating any output
        /// </summary>
        Verify
    };

    [Serializable]
    public enum MarketDataProvider
    {
        None = -1,
        Esignal,
        SAPI,
        BPIPE,
        Google,
        Yahoo,
        BlpDLWS,
        File,
        API,
        FactSet,
        ACTIV
    }

    public enum SecondaryMarketDataProvider
    {
        None,
        [EnumDescription("Bloomberg DLWS")]
        BloombergDLWS
    }

    public enum FactSetContractType
    {
        ChannelPartner = 0,
        Reseller = 1
    }

    public enum ImportType
    {
        Cash,
        Transaction,
        NetPosition,
        MarkPrice,
        ForexPrice,
        DailyBeta,
        Activities,
        SecMasterInsert,
        SecMasterUpdate,
        OMI,
        AllocationScheme,
        AllocationScheme_AppPositions,
        StagedOrder,
        GenericImport,
        CreditLimit,
        DoubleEntry,
        SMBatch,
        SettlementDateCash,
        DailyVolatility,
        DialyDividendYield,
        DailyVWAP,
        DailyCollateralPrice,
        CollateralInterest,
        ShortLocate,
        MultilegJournalImport
    }

    public enum ImportDataSource
    {
        File,
        Function
    }

    //to set the import type of the import in file seting setup
    public enum FormatType
    {
        Import,
        Recon,
        SMBatch
    }

    public enum ImportSource
    {
        Manual,
        Automatic
    }

    public enum EnmImportType
    {
        PositionImport,
        TransactionImport,
        CashImport,
        DividendImport,
        MarkPriceImport,
        BetaImport,
        OMIImport,
        SecMasterUpdateData,
        SecMasterInsertData,
        AllocationScheme,
        AllocationScheme_AppPositions,
        GenericImport,
        VolatilityImport,
        DividendYieldImport
    }

    /// <summary>
    /// added by: Bharat raturi
    /// date: 02-may-2013
    /// purpose: to get the types for the batch
    /// </summary>
    public enum SMBatchType
    {
        Beta = 0,
        //   ClosingMark = 1,
        Close = 2,
        DailyVolume = 3,
        //  IndexPrice = 4,
        MarketCap = 5
    }

    /// <summary>
    /// Enum to show Import status
    /// </summary>
    public enum ImportStatus
    {
        [XmlEnumAttribute("Success")]
        Success = 0,
        [XmlEnumAttribute("Partial Success")]
        PartialSuccess = 1,
        [XmlEnumAttribute("Failure")]
        Failure = 2,
        [XmlEnumAttribute("Imported")]
        Imported = 3,
        [XmlEnumAttribute("None")]
        None = 4,
        [XmlEnumAttribute("Not Imported")]
        NotImported = 5,
        [XmlEnumAttribute("Import Error")]
        ImportError = 6
    }

    //Modified By Faisal Shah 09/07/14
    //added By: Bharat raturi
    //15-may-2014
    //purpose: batch run time types for SM batches
    public enum SMBatchRunTime
    {
        ExchangeOpen = 0,
        ExchangeClose = 1,
        UserDefined = 2
    }

    #region enums used in recon

    public enum DataSourceType
    {
        Nirvana,
        PrimeBroker,
        Both
    }
    public enum ColumnGroupType
    {
        Nirvana = 0,
        PrimeBroker = 1,
        Common = 2,
        Both = 3,
        Diff = 4
    }

    public enum ComparisionType
    {
        Exact,
        Partial,
        Numeric
    }
    //this tax lot status is used in new Reconciliation/cancel Amend Module
    public enum AmendedTaxLotStatus
    {
        ValueChanged,
        Deleted
    }
    public enum ReconUIView
    {
        Prana = 0,
        CH = 1,
        Both = 2
    }

    public enum ReconStatus
    {
        ExactlyMatched,
        MatchedWithInTolerance,
        MisMatch,
        PBDataMissing,
        NirvanaDataMissing
    }

    public enum ToleranceType
    {
        None,
        RoundOff,
        Percentage,
        Integral,
        Absolute
    }

    #endregion

    /// <summary>
    /// Task status of a task
    /// </summary>
    public enum NirvanaTaskStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Running = 2,

        /// <summary>
        /// There were some errors and task has been terminated
        /// </summary>
        Failure = 3,

        /// <summary>
        /// Task has been canceled by user and terminated
        /// </summary>
        Canceled = 4,

        /// <summary>
        /// Task has been completed successfully
        /// </summary>
        Completed = 5,
        /// <summary>
        /// Batch is being imported into application
        /// </summary>
        Importing = 6,

        /// <summary>
        /// If Task not started yet
        /// </summary>
        Pending = 7,

        /// <summary>
        ///  yet to be completed workflow
        /// </summary>
        PendingCompleted = 8,

        /// <summary>
        ///  Success status of event
        /// </summary>
        Success = 9,

        PartialSuccess = 10,

        NotApplicable = 11
    }

    /// <summary>
    /// Task status of a task
    /// </summary>
    public enum NirvanaWorkFlows
    {
        None = 0,
        FileUpload = 1,
        SMValidation = 2,
        ImportIntoAPP = 3,
        Recon = 4,
        CnclAmnd = 5,
        MarkPrice = 6,
        Closing = 7,
        Reporting = 8,
        Import = 9,
        SMBatch = 10
    }

    /// <summary>
    /// Task status of a task
    /// </summary>
    public enum NirvanaWorkFlowsStats
    {
        None = 0,
        Imported = 1,
        Reconciled1 = 2,
        ReconPendingApproval = 3,
        ReconApproved = 4,
        FailedReconciliation = 5,
        Closed = 6,
        Deleted = 7,
        Sent = 8,

    }

    //Enum for approved and unapproved status
    public enum ApprovalStatus
    {

        Approved = 0,
        UnApproved = 1,
    }

    public enum OMIPublishType
    {
        OMIData = 1,
        OMIPreferences = 2
    }

    /// <summary>
    /// Enum for specifying secondary sources
    /// </summary>
    public enum SecondarySource
    {
        [XmlEnumAttribute("NY Composite")]
        CMPN = 0,
        [XmlEnumAttribute("London Composite")]
        CMPL = 1,
        [XmlEnumAttribute("Tokyo Composite")]
        CMPT = 2,
        [XmlEnumAttribute("Bloomberg")]
        BGN = 3,
        [XmlEnumAttribute("BVAL")]
        BVAL = 4
    }

    /// <summary>
    /// Provides a description for an enumerated type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        /// <value>The description stored in the attribute.</value>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref=”EnumDescriptionAttribute”/> class.
        /// </summary>
        /// <param name=”description”>The description to store in this attribute.</param>
        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }

        //Mukul: 2013/02/08
        // Summary:
        //     Enum for specifying the comparision operator Custom OperatorCondition.
        public enum ConditionOperator
        {
            // Summary:
            //     Tests for two values being equal.
            Equals = 0,
            //
            // Summary:
            //     Tests for two values being not equal.
            NotEquals = 1,
            //
            // Summary:
            //     Will do a wildcard comparision of the condition's value to the comparision
            //     value taking comparision value as the string with wild cards.
            Like = 2,
            //
            //
            // Summary:
            //     Complement of Like.
            NotLike = 3,
            //
            // Summary:
            //     Tests to see if the condition value starts with the operand.
            StartsWith = 4,
            //
            // Summary:
            //     Complement of StartsWith.
            DoesNotStartWith = 5,
            //
            // Summary:
            //     Tests to see if the condition value ends with the operand.
            EndsWith = 6,
            //
            // Summary:
            //     Complement of EndsWith.
            DoesNotEndWith = 7,
            //
            // Summary:
            //     Tests to see if the condition value contains the operand.
            Contains = 8,
            //
            // Summary:
            //     Complement of Contains.
            DoesNotContain = 9,

        }

        //Ankit: 2013/02/08
        //Summary: Enum for specifying the different condition types for customConditions.
        public enum ConditionType
        {
            Default = 1,
            ConditionGroup = 2,
            Formula = 4,
            Operator = 8,
            True = 16,
            All = 32767,
        }
    }

    // Aman : 2015/06/10
    // Summary:
    // Enum for specifying the different Change types for Work Area.
    public enum ChangeType
    {
        Transfer = 1,
        Trade = 2,
        NoTrade = 3
    }

    // Aman : 2015/06/10
    // Summary:
    // Enum for specifying weather the secutity is to be included or not in nav Work Area.
    public enum ExcludeType
    {
        Include = 0,
        Exclude = 1,
        ExcludeTrade = 2
    }

    // Aman : 2015/06/10
    // Summary:
    // Enum for specifying which field will be calculated by other two fields.
    public enum SettlementAutoCalculateField
    {
        SettlementPrice = 0,
        FXRate = 1,
        AveragePrice = 2
    }

    public enum PTTType
    {
        [EnumDescription("Percentage")]
        Percentage = 0,
        [EnumDescription("Basis Point")]
        BasisPoints = 1,
        [EnumDescription("$ Amount")]
        DollarAmount = 2
    }

    public enum PTTChangeType
    {
        [EnumDescription("Increase")]
        Increase = 0,
        [EnumDescription("Decrease")]
        Decrease = 1,
        [EnumDescription("Set")]
        Set = 2,
        [EnumDescription("Buy")]
        Buy = 3,
        [EnumDescription("Sell Short")]
        SellShort = 4
    }

    public enum PTTMasterFundOrAccount
    {
        [EnumDescription("Account")]
        Account = 0,
        [EnumDescription("MasterFund")]
        MasterFund = 1,
    }

    /// <summary>
    /// these below enums are used for PST edit operation
    /// </summary>
    public enum PSTType
    {
        [EnumDescription("%")]
        Percentage = 0,
        [EnumDescription("BPS")]
        BasisPoints = 1,
        [EnumDescription("Amt")]
        DollarAmount = 2
    }

    public enum PSTChangeType
    {
        [EnumDescription("increase")]
        Increase = 0,
        [EnumDescription("decrease")]
        Decrease = 1,
        [EnumDescription("set-to")]
        Set = 2,
    }

    public enum PSTMasterFundOrAccount
    {
        [EnumDescription("Account")]
        Account = 0,
        [EnumDescription("MasterFund")]
        MasterFund = 1,
        [EnumDescription("CustomGroup")]
        CustomGroup = 2,
        [EnumDescription("CalculatedPreference")]
        CalculatedPreference = 3,
    }

    /// <summary>
    /// Used for Share outstanding trigger on these levels in the Trading rule.
    /// </summary>
    public enum FundSelectionType
    {
        [EnumDescription("Account")]
        Account = 0,
        [EnumDescription("MasterFund")]
        MasterFund = 1,
        [EnumDescription("Portfolio")]
        Portfolio = 2,
    }
    /// <summary>
    /// Used to store absolute amount and define Percent
    /// </summary>
    public enum AbsoluteAmountOrDefinePercent
    {
        [EnumDescription("DefinePercent")]
        DefinePercent = 0,
        [EnumDescription("AbsoluteAmount")]
        AbsoluteAmount = 1
    }

    public enum PTTValueType
    {
        Summary = 0,
        Field = 1,
        FieldPercentage = 2,
        FixedField = 3
    }

    public enum PTTOrderSide
    {
        [EnumDescription("Buy")]
        Buy = 0,
        [EnumDescription("Sell")]
        Sell = 1,
        [EnumDescription("Sell Short")]
        SellShort = 2,
        [EnumDescription("Buy to Close")]
        BuyToClose = 3
    }

    public enum PTTCombineAccountTotalValue
    {
        [EnumDescription("No")]
        No = 0,
        [EnumDescription("Yes")]
        Yes = 1
    }

    public enum PTTRoundLotPreferenceValue
    {
        [EnumDescription("No")]
        No = 0,
        [EnumDescription("Yes")]
        Yes = 1
    }
    public enum PTTCustodianBrokerPreferenceValue
    {
        [EnumDescription("No")]
        No = 0,
        [EnumDescription("Yes")]
        Yes = 1
    }
    public enum PTTPreferenceType
    {
        [EnumDescription("Global")]
        Global = 0,
        [EnumDescription("Long")]
        Long = 1,
        [EnumDescription("Short")]
        Short = 2
    }

    /// <summary>
    /// Atul (01/June/2016)
    /// Allocation Preference Types 
    /// </summary>
    public enum AllocationPreferenceType
    {
        [EnumDescription("AllocationByAccount")]
        AllocationByAccount = 0,
        [EnumDescription("AllocationBySymbol")]
        AllocationBySymbol = 1,
    }

    public enum TradingTicketPreferenceType
    {
        [EnumDescription("Company")]
        Company = 0,
        [EnumDescription("User")]
        User = 1,
    }

    /// <summary>
    /// Used in trading ticket to reset order details or save order details for manual,live or stage 
    /// </summary>
    public enum TradingTicketType
    {
        [EnumDescription("None")]
        None = 0,
        [EnumDescription("Manual")]
        Manual = 1,
        [EnumDescription("Live")]
        Live = 2,
        [EnumDescription("Stage")]
        Stage = 3,
    }

    /// <summary>
    /// Keep track of Parent module from which TT is opened.
    /// </summary>
    public enum TradingTicketParent
    {
        [EnumDescription("None")]
        None = 0,
        [EnumDescription("PTT")]
        PTT = 1,
        [EnumDescription("Blotter")]
        Blotter = 2,
        [EnumDescription("PM")]
        PM = 3,
        [EnumDescription("SymbolLookup")]
        SymbolLookup = 4,
        [EnumDescription("LiveFeed")]
        LiveFeed = 5,
        [EnumDescription("Watchlist")]
        WatchList = 6,
        [EnumDescription("ShortLocate")]
        ShortLocate = 7,
        [EnumDescription("OptionChain")]
        OptionChain = 8
    }

    /// <summary>
    /// Quantity field on TT should be used to enter trade quantity or Notional (#Amount)
    /// </summary>
    public enum QuantityTypeOnTT
    {
        [EnumDescription("Quantity")]
        Quantity = 0,
        [EnumDescription("$Amount")]
        Amount = 1
    }

    /// <summary>
    /// Enum to store user action
    /// </summary>
    public enum UserAction
    {
        [EnumDescription("None")]
        None = 0,
        [EnumDescription("Yes")]
        Yes = 1,
        [EnumDescription("No")]
        No = 2
    }

    /// <summary>
    /// Enum to store user action
    /// </summary>
    public enum PTTEditedColumn
    {
        [EnumDescription("None")]
        None = 0,
        [EnumDescription("PercentageType")]
        PercentageType = 1,
        [EnumDescription("EndingPercentage")]
        EndingPercentage = 2,
        [EnumDescription("TradeQuantity")]
        TradeQuantity = 3

    }


    public enum NumericConditionOperator
    {
        [XmlEnumAttribute("0")]
        Equal,
        [XmlEnumAttribute("1")]
        LessThan,
        [XmlEnumAttribute("2")]
        GreaterThan,
        [XmlEnumAttribute("3")]
        LessThanOrEqual,
        [XmlEnumAttribute("4")]
        GreaterThanOrEqual
    }

    public enum CashManagementAccountSetup
    {
        CashAccounts,
        ActivityTypes,
        ActivityJournalMapping,
        SubAccountType
    }

    public enum OrderFilterLevels
    {
        [XmlEnumAttribute("0")]
        Asset,
        [XmlEnumAttribute("1")]
        Account,
        [XmlEnumAttribute("2")]
        CounterParty,
        [XmlEnumAttribute("3")]
        MasterFund,
        [XmlEnumAttribute("4")]
        PrimeBroker,
        [XmlEnumAttribute("5")]
        AUEC
    }

    public enum ShortLocateYTD
    {
        [EnumDescription("YTD")]
        YTD = 0,
        [EnumDescription("1 Day")]
        OneDay = 1
    }

    public enum ShortLocateRebateFee
    {
        [EnumDescription("BPS")]
        BPS = 0,
        [EnumDescription("%")]
        Percentage = 1
    }

    public enum BloombergCouponFrequency
    {
        NoCoupon = 0,
        Annual = 1,
        SemiAnnual = 2,
        TriAnnual = 3,
        Quarterly = 4,
        BiMonthly = 6,
        Monthly = 12,
        Weekly = 52,
        Daily = 365,
    }

    public enum BloombergAccrualBasis
    {
        [Description("ACT/360")]
        Actual360,
        [Description("ACT/365")]
        Actual365,
        [Description("ACT/ACT")]
        ActualActual,
        [Description("30/360")]
        Thirty360,
        [Description("30E/360")]
        ThirtyE360,
        [Description("30/365")]
        Thirty365,
        [Description("ACT/365F")]
        Actual365F,
        [Description("ACT/364")]
        Actual364,
        [Description("NL/365")]
        NL365,
        [Description("BUS/365")]
        BUS365
    }

    public enum BloombergSecurityType
    {
        CORP,
        GOVT,
        MUNI,
        SOV,
        AGY
    }

    /// <summary>
    /// Enum to store Trade Application Source
    /// </summary>
    public enum TradeApplicationSource
    {
        Enterprise = 0,
        NirvanaOne = 1
    }
}