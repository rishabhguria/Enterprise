namespace Prana.BusinessObjects.Classes.TTPrefs
{
    public class TradingTicketRulesPrefs
    {

        /// <summary>
        /// The is oversell trading rule
        /// </summary>
        private bool? _isOversellTradingRule;
        /// <summary>
        /// The is overbuy trading rule
        /// </summary>
        private bool? _isOverbuyTradingRule;
        /// <summary>
        /// The is unallocated trade alert
        /// </summary>
        private bool? _isUnallocatedTradeAlert;
        /// <summary>
        /// The is fat finger trading rule
        /// </summary>
        private bool? _isFatFingerTradingRule;
        /// <summary>
        /// The is duplicate trade alert
        /// </summary>
        private bool? _isDuplicateTradeAlert;
        /// <summary>
        /// The is pending new trade alert
        /// </summary>
        private bool? _isPendingNewTradeAlert;
        /// <summary>
        /// The define fat finger percent
        /// </summary>
        private double? _defineFatFingerValue = 0;
        /// <summary>
        /// The duplicate trade alert time
        /// </summary>
        private int? _duplicateTradeAlertTime = 0;
        /// <summary>
        /// The pending new order alert time
        /// </summary>
        private int? _pendingNewOrderAlertTime = 0;
        /// <summary>
        /// The fat finger account or masterfund
        /// </summary>
        private int? _fatFingerAccountOrMasterFund;
        /// <summary>
        /// The fat finger AbsoluteAmount or DefinePercent
        /// </summary>
        private int? _isAbsoluteAmountOrDefinePercent;
        /// <summary>
        /// The shares Outstanding account or masterfund
        /// </summary>
        private int? _sharesOutstandingAccOrMF;
        /// <summary>
        /// The define shares Outstanding percent
        /// </summary>
        private double? _sharesOutstandingValue = 0;
        /// <summary>
        /// The is In-Market included
        /// </summary>
        private bool? _isInMarketIncluded;
        /// <summary>
        /// The is duplicate trade alert
        /// </summary>
        private bool? _isSharesOutstandingRule;

        /// <summary>
        /// Gets or sets value of IsOversellTradingRule
        /// </summary>
        public bool? IsOversellTradingRule
        {
            get { return _isOversellTradingRule; }
            set { _isOversellTradingRule = value; }
        }

        /// <summary>
        /// Gets or sets value of IsOverbuyTradingRule
        /// </summary>
        public bool? IsOverbuyTradingRule
        {
            get { return _isOverbuyTradingRule; }
            set { _isOverbuyTradingRule = value; }
        }

        /// <summary>
        /// Gets or sets value of IsUnallocatedTradeAlert
        /// </summary>
        public bool? IsUnallocatedTradeAlert
        {
            get { return _isUnallocatedTradeAlert; }
            set { _isUnallocatedTradeAlert = value; }
        }

        /// <summary>
        /// Gets or sets value of IsFatFingerTradingRule
        /// </summary>
        public bool? IsFatFingerTradingRule
        {
            get { return _isFatFingerTradingRule; }
            set { _isFatFingerTradingRule = value; }
        }

        /// <summary>
        /// Gets or sets value of IsDuplicateTradeAlert
        /// </summary>
        public bool? IsDuplicateTradeAlert
        {
            get { return _isDuplicateTradeAlert; }
            set { _isDuplicateTradeAlert = value; }
        }

        /// <summary>
        /// Gets or sets value of IsPendingNewTradeAlert
        /// </summary>
        public bool? IsPendingNewTradeAlert
        {
            get { return _isPendingNewTradeAlert; }
            set { _isPendingNewTradeAlert = value; }
        }

        /// <summary>
        /// Gets or sets value of IsInMarketIncluded
        /// </summary>
        public bool? IsInMarketIncluded
        {
            get { return _isInMarketIncluded; }
            set { _isInMarketIncluded = value; }
        }

        /// <summary>
        /// Gets or sets value of IsSharesOutstandingRule
        /// </summary>
        public bool? IsSharesOutstandingRule
        {
            get { return _isSharesOutstandingRule; }
            set { _isSharesOutstandingRule = value; }
        }

        /// <summary>
        /// Gets or sets value of DefineFatFingerValue
        /// </summary>
        public double? DefineFatFingerValue
        {
            get { return _defineFatFingerValue; }
            set { _defineFatFingerValue = value; }
        }

        /// <summary>
        /// Gets or sets value of DuplicateTradeAlertTime
        /// </summary>
        public int? DuplicateTradeAlertTime
        {
            get { return _duplicateTradeAlertTime; }
            set { _duplicateTradeAlertTime = value; }
        }

        /// <summary>
        /// Gets or sets value of PendingNewOrderAlertTime
        /// </summary>
        public int? PendingNewOrderAlertTime
        {
            get { return _pendingNewOrderAlertTime; }
            set { _pendingNewOrderAlertTime = value; }
        }

        /// <summary>
        /// Gets or sets value of FatFingerAccountOrMasterFund
        /// </summary>
        public int? FatFingerAccountOrMasterFund
        {
            get { return _fatFingerAccountOrMasterFund; }
            set { _fatFingerAccountOrMasterFund = value; }
        }
        /// <summary>
        /// Gets or sets value of IsAbsoluteAmountOrDefinePercent
        /// </summary>
        public int? IsAbsoluteAmountOrDefinePercent
        {
            get { return _isAbsoluteAmountOrDefinePercent; }
            set { _isAbsoluteAmountOrDefinePercent = value; }
        }

        /// <summary>
        /// Gets or sets value of SharesOutstandingAccOrMF
        /// </summary>
        public int? SharesOutstandingAccOrMF
        {
            get { return _sharesOutstandingAccOrMF; }
            set { _sharesOutstandingAccOrMF = value; }
        }

        /// <summary>
        /// Gets or sets value of SharesOutstandingValue
        /// </summary>
        public double? SharesOutstandingValue
        {
            get { return _sharesOutstandingValue; }
            set { _sharesOutstandingValue = value; }
        }
    }
}
