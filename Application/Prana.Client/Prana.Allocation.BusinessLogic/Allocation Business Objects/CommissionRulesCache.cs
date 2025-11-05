using System;
using System.Collections.Generic;
using System.Text;

using Prana.BusinessObjects;

namespace Prana.Allocation.BLL
{

    /// <summary>
    /// Keeps all the commission rules and their associations with CV - AUEC
    /// Singleton class
    /// </summary>
    internal class CommissionRulesCache
    {
        #region Singleton Instance
        private CommissionRulesCache()
        {
        }

        static CommissionRulesCache _commissionRulesCache = null;
        public static CommissionRulesCache GetInstance()
        {
            if (_commissionRulesCache == null)
            {
                _commissionRulesCache = new CommissionRulesCache();
            }
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

        private Dictionary<Guid, CommissionRule> _commissionRuleDict = new Dictionary<Guid,CommissionRule>();

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

        private List<CommissionRule> _modifiedCommissionRules= new List<CommissionRule>();
        /// <summary>
        /// This list is used for saving only the modified rules while setting up rules in Admin
        /// </summary>
        public List<CommissionRule> ModifiedCommissionRules
        {
            get { return _modifiedCommissionRules; }
            set { _modifiedCommissionRules = value; }
        }

        private List<CVAUECFundCommissionRule> _cvAUECFundCommissionRules = new List<CVAUECFundCommissionRule>();

        public List<CVAUECFundCommissionRule> CVAUECFundCommissionRules
        {
            get { return _cvAUECFundCommissionRules; }
            set { _cvAUECFundCommissionRules = value; }
        }

        private Dictionary<int, CVAUECFundCommissionRule> _cVAUECFundCommissionRuleDict = new Dictionary<int, CVAUECFundCommissionRule>();
        /// <summary>
        /// this dictionary is used for searching purpose, it also contains all the CVAUECFundCommissionRule Collection
        /// that is in the main collection i.e. List of CVAUECFundCommissionRule named '_cvAUECFundCommissionRules'
        /// </summary>
        public Dictionary<int, CVAUECFundCommissionRule> CVAUECFundCommissionRulesDict
        {
            get { return _cVAUECFundCommissionRuleDict; }
            set { _cVAUECFundCommissionRuleDict = value; }
        }

	

        
       
    }
}
