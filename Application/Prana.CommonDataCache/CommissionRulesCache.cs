using Prana.BusinessObjects;
using System;
using System.Collections.Generic;

namespace Prana.CommonDataCache
{

    /// <summary>
    /// Keeps all the commission rules and their associations with CV - AUEC
    /// Singleton class
    /// </summary>
    internal class CommissionRulesCache
    {
        #region Singleton Instance
        //Harsh: these are thread safe as CLR ensures static constructor runs before any other access
        static CommissionRulesCache()
        {

            _commissionRulesCache = new CommissionRulesCache();
        }

        static CommissionRulesCache _commissionRulesCache = null;
        public static CommissionRulesCache GetInstance()
        {
            return _commissionRulesCache;
        }
        #endregion

        private bool _isPostAllocatedCalculation = false;
        /// <summary>
        /// This property stores the value for checking whether the calculations/fees are calculated pre allocation or
        /// post allocation.
        /// </summary>
        public bool IsPostAllocatedCalculation
        {
            get { return _isPostAllocatedCalculation; }
            set { _isPostAllocatedCalculation = value; }
        }

        private Dictionary<Guid, CommissionRule> _commissionRuleDict = new Dictionary<Guid, CommissionRule>();

        public Dictionary<Guid, CommissionRule> CommissionRuleDict
        {
            get { return _commissionRuleDict; }
            set { _commissionRuleDict = value; }
        }

        private List<CommissionRule> _allCommissionRules = new List<CommissionRule>();

        public List<CommissionRule> AllCommissionRules
        {
            get { return _allCommissionRules; }
            set { _allCommissionRules = value; }
        }

        private List<CommissionRule> _modifiedCommissionRules = new List<CommissionRule>();
        /// <summary>
        /// This list is used for saving only the modified rules while setting up rules in Admin
        /// </summary>
        public List<CommissionRule> ModifiedCommissionRules
        {
            get { return _modifiedCommissionRules; }
            set { _modifiedCommissionRules = value; }
        }

        private List<CVAUECAccountCommissionRule> _cvAUECAccountCommissionRules = new List<CVAUECAccountCommissionRule>();

        public List<CVAUECAccountCommissionRule> CVAUECAccountCommissionRules
        {
            get { return _cvAUECAccountCommissionRules; }
            set { _cvAUECAccountCommissionRules = value; }
        }

        private Dictionary<int, CVAUECAccountCommissionRule> _cVAUECAccountCommissionRuleDict = new Dictionary<int, CVAUECAccountCommissionRule>();
        /// <summary>
        /// this dictionary is used for searching purpose, it also contains all the CVAUECAccountCommissionRule Collection
        /// that is in the main collection i.e. List of CVAUECAccountCommissionRule named '_cvAUECAccountCommissionRules'
        /// </summary>
        public Dictionary<int, CVAUECAccountCommissionRule> CVAUECAccountCommissionRulesDict
        {
            get { return _cVAUECAccountCommissionRuleDict; }
            set { _cVAUECAccountCommissionRuleDict = value; }
        }


        #region Other Fees
        private Dictionary<Guid, OtherFeeRule> _otherFeeRuleDict = new Dictionary<Guid, OtherFeeRule>();

        public Dictionary<Guid, OtherFeeRule> OtherFeeRuleDict
        {
            get { return _otherFeeRuleDict; }
            set { _otherFeeRuleDict = value; }
        }
        private Dictionary<int, List<OtherFeeRule>> _otherFeeRuleAuecDict = new Dictionary<int, List<OtherFeeRule>>();

        //dictionary rule based on AUECID 
        public Dictionary<int, List<OtherFeeRule>> OtherFeeRuleAuecDict
        {
            get { return _otherFeeRuleAuecDict; }
            set { _otherFeeRuleAuecDict = value; }
        }
        private List<OtherFeeRule> _allOtherFeeRules = new List<OtherFeeRule>();

        public List<OtherFeeRule> AllOtherFeeRules
        {
            get { return _allOtherFeeRules; }
            set { _allOtherFeeRules = value; }
        }
        #endregion

    }
}
