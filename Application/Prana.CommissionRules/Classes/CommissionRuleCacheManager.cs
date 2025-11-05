using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;

namespace Prana.CommissionRules
{
    public class CommissionRuleCacheManager
    {
        CommissionRuleCache _commissionRuleCache = null;

        #region Singleton Instance of CommissionRuleCacheManager

        private CommissionRuleCacheManager()
        {
            _commissionRuleCache = CommissionRuleCache.GetInstance();
        }

        static CommissionRuleCacheManager _commissionRuleCacheManager = null;
        public static CommissionRuleCacheManager GetInstance()
        {
            if (_commissionRuleCacheManager == null)
            {
                _commissionRuleCacheManager = new CommissionRuleCacheManager();
            }
            return _commissionRuleCacheManager;
        }
        #endregion

        #region Commission Rules

        public CommissionRule GetCommissionRuleByRuleId(Guid ruleId)
        {
            CommissionRule commissionRuleobj = null;
            lock (_commissionRuleCache.CommissionRuleDict)
            {
                if (_commissionRuleCache.CommissionRuleDict.ContainsKey(ruleId))
                {
                    commissionRuleobj = _commissionRuleCache.CommissionRuleDict[ruleId];
                }
            }
            return commissionRuleobj;
        }

        public void AddCommissionRule(CommissionRule commissionRule)
        {
            lock (_commissionRuleCache.CommissionRuleDict)
            {
                // add commission rule into the main collection, that is type of List<CommissionRule>
                if (!_commissionRuleCache.CommissionRuleDict.ContainsKey(commissionRule.RuleID))
                {
                    _commissionRuleCache.AllCommissionRules.Add(commissionRule);
                    commissionRule.CommissionRuleChanged += new CommissionRuleChangeHandler(commissionRule_CommissionRuleChanged);

                    // add commission rule into the collection which is require for searching purpose
                    // that is type of Dictionary<RuleId,CommissionRule>
                    _commissionRuleCache.CommissionRuleDict.Add(commissionRule.RuleID, commissionRule);

                }
            }
        }

        public void DeleteCommissionRuleFromCollections(CommissionRule commissionRule)
        {
            lock (_commissionRuleCache.CommissionRuleDict)
            {
                if (_commissionRuleCache.CommissionRuleDict.ContainsKey(commissionRule.RuleID))
                {
                    // delete commission rule from AllCommissionRules list collection           
                    _commissionRuleCache.AllCommissionRules.Remove(commissionRule);
                    // delete commission rule from CommissionRuleDict dictionary collection 
                    _commissionRuleCache.CommissionRuleDict.Remove(commissionRule.RuleID);
                    //remove from the modified rule ModifiedCommissionRules list collections
                    _commissionRuleCache.ModifiedCommissionRules.Remove(commissionRule);
                }
            }
        }

        public void ClearAllCommissionRuleCollections()
        {
            // clear Commission Rule dictionary
            _commissionRuleCache.CommissionRuleDict.Clear();
            // clear All commission Rule List
            _commissionRuleCache.AllCommissionRules.Clear();
            // clear All modified List
            _commissionRuleCache.ModifiedCommissionRules.Clear();
        }

        public List<CommissionRule> GetAllCommissionRules()
        {
            lock (_commissionRuleCache.AllCommissionRules)
            {
                return _commissionRuleCache.AllCommissionRules;
            }
        }

        public List<CommissionRule> GetAllModifiedCommissionRules()
        {
            lock (_commissionRuleCache.ModifiedCommissionRules)
            {
                return _commissionRuleCache.ModifiedCommissionRules;
            }
        }

        void commissionRule_CommissionRuleChanged(CommissionRule changedRule)
        {
            lock (_commissionRuleCache.ModifiedCommissionRules)
            {
                if (!_commissionRuleCache.ModifiedCommissionRules.Contains(changedRule))
                {
                    _commissionRuleCache.ModifiedCommissionRules.Add(changedRule);
                }
            }
        }

        public void SetAllocatedCalculationProperty(bool setCommissionCalculationTime)
        {
            _commissionRuleCache.IsPostAllocatedCalculation = setCommissionCalculationTime;
        }

        public bool GetAllocatedCalculationProperty()
        {
            return _commissionRuleCache.IsPostAllocatedCalculation;
        }


        #endregion

        #region CV-AUEC association

        public void AddCVAUECRule(CVAUECFundCommissionRule cvAUECFundCommissionRule)
        {
            if (!_commissionRuleCache.CVAUECFundCommissionRulesDict.ContainsKey(cvAUECFundCommissionRule.CVAUECRuleID))
            {
                // object in the CVAUECFundCommissionRules List
                _commissionRuleCache.CVAUECFundCommissionRules.Add(cvAUECFundCommissionRule);
                // add object in the CVAUECFundCommissionRulesDict
                _commissionRuleCache.CVAUECFundCommissionRulesDict.Add(cvAUECFundCommissionRule.CVAUECRuleID, cvAUECFundCommissionRule);
            }
        }

        public void ClearCVAUECCommissionRulesDictionary()
        {
            _commissionRuleCache.CVAUECFundCommissionRulesDict.Clear();
        }

        public List<CVAUECFundCommissionRule> GetAllCVAUECFundCommissionRules()
        {
            //_commissionRulesCache.CVAUECFundCommissionRules.Clear();
            //List<CVAUECFundCommissionRule> cvAUECFundCommissionRuleList = _commissionRulesCache.CVAUECFundCommissionRules;
            //foreach (CVAUECFundCommissionRule cvAUECFundCommissionRule in _commissionRulesCache.CVAUECFundCommissionRulesDict)
            //{
            //    cvAUECFundCommissionRuleList.Add(cvAUECFundCommissionRule);    
            //}
            //_commissionRulesCache.CVAUECFundCommissionRules = cvAUECFundCommissionRuleList;
            return _commissionRuleCache.CVAUECFundCommissionRules;
        }


        /// <summary>
        /// this method is used for PreAllocation Commission Calculation
        /// </summary>
        /// <param name="cvID"></param>
        /// <param name="auecID"></param>
        /// <param name="blntradeType"></param>
        /// <returns></returns>
        //public CommissionRule GetCommissionRuleByCVAUECFundId(int cvID, int auecID, bool blntradeType,int fundID) Commented on 29th Oct, 07.
        public CommissionRule GetCommissionRuleByCVAUECFundId(int counterPartyID, int venueID, int auecID, string listId, int fundID, ref string commissionText)
        {
            foreach (CVAUECFundCommissionRule cvAUECFundCommissionRule in _commissionRuleCache.CVAUECFundCommissionRules)
            {
                // listId to check for Trade Type i.e. for Fingle or Basket, in case of Single listId will be Empty
                TradeType tradeType;
                if (listId == string.Empty)
                {
                    tradeType = TradeType.SingleTrade;
                }
                else
                {
                    tradeType = TradeType.BasketTrade;
                }
                //if (cvAUECFundCommissionRule.CVID == cvID) Commented on 29th Oct, 07.
                if (cvAUECFundCommissionRule.CounterPartyId == counterPartyID && cvAUECFundCommissionRule.VenueId == venueID)
                {
                    if (cvAUECFundCommissionRule.AUECID == auecID)
                    {
                        if (cvAUECFundCommissionRule.FundID == fundID)
                        {
                            #region Commented IsPostAllocatedCalculation check
                            ///This condition takes care whether the commissions are calculated at pre-allocated stage or 
                            ///post-allocated stage. 
                            //if (_commissionRulesCache.IsPostAllocatedCalculation)
                            //{
                            //if (cvAUECFundCommissionRule.FundID != fundID)
                            //{
                            ///If commisions needs to be calculated post allocation, but fundID of current Order does
                            ///not match with the current rule , it means the current rule is not associated with the order's fund,
                            ///So continue
                            // continue;
                            //}
                            //}
                            #endregion

                            switch (tradeType)
                            {
                                case TradeType.SingleTrade:
                                    return cvAUECFundCommissionRule.SingleRule;
                                    break;
                                case TradeType.BasketTrade:
                                    return cvAUECFundCommissionRule.BasketRule;
                                    break;
                                default:
                                    return null;
                                    throw new Exception("Trade type not set. It should be either Single Trade or Basket Trade.");
                            }
                        }
                        else
                        {
                            commissionText = "Fund is not specified";
                        }

                    }
                    else
                    {
                        //commissionText = "AUEC is not specified";
                    }

                }
                else
                {
                    commissionText = "counter party or venue is not specified";
                }

            }

            return null;
        }


        /// <summary>
        /// this method is used for PostAllocation Commission Calculation
        /// </summary>
        /// <param name="cvID"></param>
        /// <param name="auecID"></param>
        /// <param name="blntradeType"></param>
        /// <returns></returns>
        //public CommissionRule GetCommissionRuleByCVAUEC(int cvID, int auecID, bool blntradeType) :Commented on 30th Oct.

        public CommissionRule GetCommissionRuleByCVAUEC(int counterPartyID, int venueID, int auecID, string listId, ref string commissionText)
        {
            foreach (CVAUECFundCommissionRule cvAUECFundCommissionRule in _commissionRuleCache.CVAUECFundCommissionRules)
            {
                TradeType tradeType;
                if (listId == string.Empty)
                {
                    tradeType = TradeType.SingleTrade;
                }
                else
                {
                    tradeType = TradeType.BasketTrade;
                }


                //if (cvAUECFundCommissionRule.CVID == cvID) :Commented on 30th Oct.
                if (cvAUECFundCommissionRule.CounterPartyId == counterPartyID && cvAUECFundCommissionRule.VenueId == venueID)
                {
                    if (cvAUECFundCommissionRule.AUECID == auecID)
                    {
                        switch (tradeType)
                        {
                            case TradeType.SingleTrade:
                                return cvAUECFundCommissionRule.SingleRule;
                                break;
                            case TradeType.BasketTrade:
                                return cvAUECFundCommissionRule.BasketRule;
                                break;
                            default:
                                return null;
                                throw new Exception("Trade type not set. It should be either Single Trade or Basket Trade.");
                        }
                    }
                    else
                    {
                        commissionText = "AUEC is not specified";
                    }

                }
                else
                {
                    commissionText = "counter party or venue is not specified";
                }

            }

            return null;
        }

        #endregion

        #region Get_Single_Basket_Rules_List

        private List<CommissionRule> _allSingleCommissionRules = new List<CommissionRule>();
        //Returns all the Single and Both type of commission Rules.
        public List<CommissionRule> GetAllSingleCommissionRules()
        {
            _allSingleCommissionRules.Clear();
            foreach (CommissionRule commissionRule in _commissionRuleCache.AllCommissionRules)
            {
                if (commissionRule.ApplyRuleForTrade == Prana.BusinessObjects.AppConstants.TradeType.SingleTrade || commissionRule.ApplyRuleForTrade == Prana.BusinessObjects.AppConstants.TradeType.Both)
                {
                    _allSingleCommissionRules.Add(commissionRule);
                }
            }
            return _allSingleCommissionRules;
        }

        private List<CommissionRule> _allBasketCommissionRules = new List<CommissionRule>();
        //Returns all the Basket and Both type of commission Rules.
        public List<CommissionRule> GetAllBasketCommissionRules()
        {
            _allBasketCommissionRules.Clear();
            foreach (CommissionRule commissionRule in _commissionRuleCache.AllCommissionRules)
            {
                if (commissionRule.ApplyRuleForTrade == Prana.BusinessObjects.AppConstants.TradeType.BasketTrade || commissionRule.ApplyRuleForTrade == Prana.BusinessObjects.AppConstants.TradeType.Both)
                {
                    _allBasketCommissionRules.Add(commissionRule);
                }
            }
            return _allBasketCommissionRules;
        }
        #endregion

        #region SaveCommissionRulesForCVAUEC
        public void SaveCommissionRulesForCVAUEC()
        {
            // Save into cache i.e. 
            //CommissionDBManager.SaveCompanyCommissionRulesForCVAUEC(_commissionRulesCache.CVAUECFundCommissionRules);
            //_commissionRulesCache.CVAUECFundCommissionRule = fasfdlkj
            ///Save into db
            //CommissionDBManager.SaveAndUpdateCommissionRule();



        }
        #endregion

        #region GetCommissionRulesForCVAUEC
        //public List<CVAUECFundCommissionRule> GetCommissionRulesForCVAUEC(int companyID)
        //{
        //    //_commissionRulesCache.CVAUECFundCommissionRules = CommissionDBManager.GetAllCommissionRulesForCVAUEC(companyID);
        //    //return _commissionRulesCache.CVAUECFundCommissionRules;
        //}
        #endregion



    }
}
