using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class TranferTradeRules
    {
        private bool _isTIFChange;
        public bool IsTIFChange
        {
            get { return _isTIFChange; }
            set { _isTIFChange = value; }
        }

        private bool _isTradingAccChange;
        public bool IsTradingAccChange
        {
            get { return _isTradingAccChange; }
            set { _isTradingAccChange = value; }
        }

        private bool _isAccountChange;
        public bool IsAccountChange
        {
            get { return _isAccountChange; }
            set { _isAccountChange = value; }
        }

        private bool _isStrategyChange;
        public bool IsStrategyChange
        {
            get { return _isStrategyChange; }
            set { _isStrategyChange = value; }
        }

        private bool _isHandlingInstrChange;
        public bool IsHandlingInstrChange
        {
            get { return _isHandlingInstrChange; }
            set { _isHandlingInstrChange = value; }
        }

        private bool _isVenueCPChange;
        public bool IsVenueCPChange
        {
            get { return _isVenueCPChange; }
            set { _isVenueCPChange = value; }
        }

        private bool _isAllowAllUserToCancelReplaceRemove;
        public bool IsAllowAllUserToCancelReplaceRemove
        {
            get { return _isAllowAllUserToCancelReplaceRemove; }
            set { _isAllowAllUserToCancelReplaceRemove = value; }
        }

        private bool _isAllowUserToChangeOrderType;
        public bool IsAllowUserToChangeOrderType
        {
            get { return _isAllowUserToChangeOrderType; }
            set { _isAllowUserToChangeOrderType = value; }
        }
        private bool _isExecutionInstrChange;
        public bool IsExecutionInstrChange
        {
            get { return _isExecutionInstrChange; }
            set { _isExecutionInstrChange = value; }
        }

        private bool _isAllowUserToTansferTrade;
        public bool IsAllowUserToTansferTrade
        {
            get { return _isAllowUserToTansferTrade; }
            set { _isAllowUserToTansferTrade = value; }
        }

        private bool _isAllowUserToGenerateSub;
        public bool IsAllowUserToGenerateSub
        {
            get { return _isAllowUserToGenerateSub; }
            set { _isAllowUserToGenerateSub = value; }
        }

        private bool _isDefaultOrderTypeLimitMultiDay;
        public bool IsDefaultOrderTypeLimitForMultiDay
        {
            get { return _isDefaultOrderTypeLimitMultiDay; }
            set { _isDefaultOrderTypeLimitMultiDay = value; }
        }

        private bool _isApplyLimitRulesForReplacingStagedOrders;
        public bool IsApplyLimitRulesForReplacingStagedOrders
        {
            get { return _isApplyLimitRulesForReplacingStagedOrders; }
            set { _isApplyLimitRulesForReplacingStagedOrders = value; }
        }

        private bool _isApplyLimitRulesForReplacingOtherOrders;
        public bool IsApplyLimitRulesForReplacingOtherOrders
        {
            get { return _isApplyLimitRulesForReplacingOtherOrders; }
            set { _isApplyLimitRulesForReplacingOtherOrders = value; }
        }

        private bool _isApplyLimitRulesForReplacingSubOrders;
        public bool IsApplyLimitRulesForReplacingSubOrders
        {
            get { return _isApplyLimitRulesForReplacingSubOrders; }
            set { _isApplyLimitRulesForReplacingSubOrders = value; }
        }

        /// <summary>
        /// The is allow restricted securities list
        /// </summary>
        private bool _isAllowRestrictedSecuritiesList;
        /// <summary>
        /// Gets or sets IsAllowRestrictedSecuritiesList
        /// </summary>
        public bool IsAllowRestrictedSecuritiesList
        {
            get { return _isAllowRestrictedSecuritiesList; }
            set { _isAllowRestrictedSecuritiesList = value; }
        }

        /// <summary>
        /// The is allow allowed securities list
        /// </summary>
        private bool _isAllowAllowedSecuritiesList;
        /// <summary>
        /// Gets or sets IsAllowAllowedSecuritiesList
        /// </summary>
        public bool IsAllowAllowedSecuritiesList
        {
            get { return _isAllowAllowedSecuritiesList; }
            set { _isAllowAllowedSecuritiesList = value; }
        }

        /// <summary>
        /// The list of master users Ids
        /// </summary>
        private List<int> _masterUsersIDs;
        /// <summary>
        /// Gets or sets MasterUsersIDs
        /// </summary>
        public List<int> MasterUsersIDs
        {
            get { return _masterUsersIDs; }
            set { _masterUsersIDs = value; }
        }

    }
}
