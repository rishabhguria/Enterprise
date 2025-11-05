using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Prana.CommonDataCache
{

    public class CommissionRulesCacheManager
    {
        static CommissionRulesCache _commissionRulesCache = null;
        static CommissionRulesCacheManager _commissionRulesCacheManager = null;

        #region Singleton Instance of CommissionRulesCacheManager
        //Harsh: these are thread safe as CLR ensures static constructor runs before any other access
        static CommissionRulesCacheManager()
        {
            _commissionRulesCache = CommissionRulesCache.GetInstance();
            if (_commissionRulesCacheManager == null)
            {
                _commissionRulesCacheManager = new CommissionRulesCacheManager();
            }
        }

        public static CommissionRulesCacheManager GetInstance()
        {
            return _commissionRulesCacheManager;
        }
        #endregion

        public List<OtherFeeRule> GetOtherFeeRuleAuecDict(int AUECID)
        {
            List<OtherFeeRule> list = new List<OtherFeeRule>();
            lock (_commissionRulesCache.OtherFeeRuleAuecDict)
            {
                if (_commissionRulesCache.OtherFeeRuleAuecDict.ContainsKey(AUECID))
                {
                    list = _commissionRulesCache.OtherFeeRuleAuecDict[AUECID];
                }
            }
            return list;
        }

        #region Commission Rules 
        public CommissionRule GetCommissionRuleByRuleId(Guid ruleId)
        {
            CommissionRule commissionRuleobj = null;
            lock (_commissionRulesCache.CommissionRuleDict)
            {
                if (_commissionRulesCache.CommissionRuleDict.ContainsKey(ruleId))
                {
                    commissionRuleobj = _commissionRulesCache.CommissionRuleDict[ruleId];
                }
            }
            return commissionRuleobj;
        }

        public void AddCommissionRule(CommissionRule commissionRule)
        {
            lock (_commissionRulesCache.CommissionRuleDict)
            {
                // add commission rule into the main collection, that is type of List<CommissionRule>
                if (!_commissionRulesCache.CommissionRuleDict.ContainsKey(commissionRule.RuleID))
                {
                    _commissionRulesCache.AllCommissionRules.Add(commissionRule);
                    commissionRule.CommissionRuleChanged += new CommissionRuleChangeHandler(commissionRule_CommissionRuleChanged);

                    // add commission rule into the collection which is require for searching purpose
                    // that is type of Dictionary<RuleId,CommissionRule>
                    _commissionRulesCache.CommissionRuleDict.Add(commissionRule.RuleID, commissionRule);

                }
            }
        }

        public void DeleteCommissionRuleFromCollections(CommissionRule commissionRule)
        {
            lock (_commissionRulesCache.CommissionRuleDict)
            {
                if (_commissionRulesCache.CommissionRuleDict.ContainsKey(commissionRule.RuleID))
                {
                    // delete commission rule from AllCommissionRules list collection           
                    _commissionRulesCache.AllCommissionRules.Remove(commissionRule);
                    // delete commission rule from CommissionRuleDict dictionary collection 
                    _commissionRulesCache.CommissionRuleDict.Remove(commissionRule.RuleID);
                    //remove from the modified rule ModifiedCommissionRules list collections
                    _commissionRulesCache.ModifiedCommissionRules.Remove(commissionRule);
                }
            }
        }

        public void ClearAllCommissionRuleCollections()
        {
            // clear Commission Rule dictionary
            _commissionRulesCache.CommissionRuleDict.Clear();
            // clear All commission Rule List
            _commissionRulesCache.AllCommissionRules.Clear();
            // clear All modified List
            _commissionRulesCache.ModifiedCommissionRules.Clear();
        }

        public List<CommissionRule> GetAllCommissionRules()
        {
            lock (_commissionRulesCache.AllCommissionRules)
            {
                return _commissionRulesCache.AllCommissionRules;
            }
        }

        public List<CommissionRule> GetAllModifiedCommissionRules()
        {
            lock (_commissionRulesCache.ModifiedCommissionRules)
            {
                return _commissionRulesCache.ModifiedCommissionRules;
            }
        }

        void commissionRule_CommissionRuleChanged(object sender, EventArgs<CommissionRule> e)
        {
            lock (_commissionRulesCache.ModifiedCommissionRules)
            {
                if (!_commissionRulesCache.ModifiedCommissionRules.Contains(e.Value))
                {
                    _commissionRulesCache.ModifiedCommissionRules.Add(e.Value);
                }
            }
        }

        public void SetAllocatedCalculationProperty(bool setCommissionCalculationTime)
        {
            _commissionRulesCache.IsPostAllocatedCalculation = setCommissionCalculationTime;
        }

        public bool GetAllocatedCalculationProperty()
        {
            return _commissionRulesCache.IsPostAllocatedCalculation;
        }


        #endregion

        #region CV-AUEC association

        public void AddCVAUECRule(CVAUECAccountCommissionRule cvAUECAccountCommissionRule)
        {
            if (!_commissionRulesCache.CVAUECAccountCommissionRulesDict.ContainsKey(cvAUECAccountCommissionRule.CVAUECRuleID))
            {
                // object in the CVAUECAccountCommissionRules List
                _commissionRulesCache.CVAUECAccountCommissionRules.Add(cvAUECAccountCommissionRule);
                // add object in the CVAUECAccountCommissionRulesDict
                _commissionRulesCache.CVAUECAccountCommissionRulesDict.Add(cvAUECAccountCommissionRule.CVAUECRuleID, cvAUECAccountCommissionRule);
            }
        }
        public void AddAUECOtherFeeRule(OtherFeeRule otherFeeRule)
        {
            List<OtherFeeRule> otherFeeRuleList = new List<OtherFeeRule>();
            if (!_commissionRulesCache.OtherFeeRuleAuecDict.ContainsKey(otherFeeRule.AUECID))
            {
                otherFeeRuleList.Add(otherFeeRule);
                _commissionRulesCache.OtherFeeRuleAuecDict.Add(otherFeeRule.AUECID, otherFeeRuleList);
            }
            else
            {
                otherFeeRuleList = _commissionRulesCache.OtherFeeRuleAuecDict[otherFeeRule.AUECID];
                otherFeeRuleList.Add(otherFeeRule);
            }
        }
        public void ClearCVAUECCommissionRulesDictionary()
        {
            _commissionRulesCache.CVAUECAccountCommissionRulesDict.Clear();
        }
        public void ClearAUECOtherFeesRulesDictionary()
        {
            _commissionRulesCache.AllOtherFeeRules.Clear();
            _commissionRulesCache.OtherFeeRuleAuecDict.Clear();
        }
        public List<CVAUECAccountCommissionRule> GetAllCVAUECAccountCommissionRules()
        {
            //_commissionRulesCache.CVAUECAccountCommissionRules.Clear();
            //List<CVAUECAccountCommissionRule> cvAUECAccountCommissionRuleList = _commissionRulesCache.CVAUECAccountCommissionRules;
            //foreach (CVAUECAccountCommissionRule cvAUECAccountCommissionRule in _commissionRulesCache.CVAUECAccountCommissionRulesDict)
            //{
            //    cvAUECAccountCommissionRuleList.Add(cvAUECAccountCommissionRule);    
            //}
            //_commissionRulesCache.CVAUECAccountCommissionRules = cvAUECAccountCommissionRuleList;
            return _commissionRulesCache.CVAUECAccountCommissionRules;
        }


        /// <summary>
        /// this method is used for PreAllocation Commission Calculation
        /// </summary>
        /// <param name="auecID"></param>
        /// <returns></returns>
        //public CommissionRule GetCommissionRuleByCVAUECAccountId(int cvID, int auecID, bool blntradeType,int accountID) Commented on 29th Oct, 07.
        public CommissionRule GetCommissionRuleByCVAUECAccountId(int counterPartyID, int venueID, int auecID, string listId, int accountID, ref string commissionText)
        {
            // listId to check for Trade Type i.e. for Fingle or Basket, in case of Single listId will be Empty
            TradeType tradeType = (listId == string.Empty) ? TradeType.SingleTrade : TradeType.BasketTrade;

            foreach (CVAUECAccountCommissionRule cvAUECAccountCommissionRule in _commissionRulesCache.CVAUECAccountCommissionRules)
            {
                //if (cvAUECAccountCommissionRule.CVID == cvID) Commented on 29th Oct, 07.
                if (cvAUECAccountCommissionRule.CounterPartyId == counterPartyID && cvAUECAccountCommissionRule.VenueId == venueID)
                {
                    if (cvAUECAccountCommissionRule.AUECID == auecID)
                    {
                        if (cvAUECAccountCommissionRule.AccountID == accountID)
                        {
                            switch (tradeType)
                            {
                                case TradeType.SingleTrade:
                                    return cvAUECAccountCommissionRule.SingleRule;

                                case TradeType.BasketTrade:
                                    return cvAUECAccountCommissionRule.BasketRule;

                                default:
                                    {
                                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Trade type not set. It should be either Single Trade or Basket Trade.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                        return null;
                                    }
                            }
                        }
                        else
                        {
                            commissionText = "Account is not specified";
                        }
                    }
                }
                else
                {
                    commissionText = ApplicationConstants.CONST_BROKER + " or venue is not specified";
                }
            }

            return null;
        }

        /// <summary>
        /// this method is used for PostAllocation Commission Calculation
        /// </summary>
        /// <param name="auecID"></param>
        /// <returns></returns>
        //public CommissionRule GetCommissionRuleByCVAUEC(int cvID, int auecID, bool blntradeType) :Commented on 30th Oct.

        public CommissionRule GetCommissionRuleByCVAUEC(int counterPartyID, int venueID, int auecID, string listId, ref string commissionText)
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

            foreach (CVAUECAccountCommissionRule cvAUECAccountCommissionRule in _commissionRulesCache.CVAUECAccountCommissionRules)
            {
                //if (cvAUECAccountCommissionRule.CVID == cvID) :Commented on 30th Oct.
                if (cvAUECAccountCommissionRule.CounterPartyId == counterPartyID && cvAUECAccountCommissionRule.VenueId == venueID)
                {
                    if (cvAUECAccountCommissionRule.AUECID == auecID)
                    {
                        switch (tradeType)
                        {
                            case TradeType.SingleTrade:
                                return cvAUECAccountCommissionRule.SingleRule;

                            case TradeType.BasketTrade:
                                return cvAUECAccountCommissionRule.BasketRule;

                            default:
                                {
                                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Trade type not set. It should be either Single Trade or Basket Trade.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                    return null;
                                }
                        }
                    }
                    else
                    {
                        commissionText = "Not Calculated,Rule not defined";

                    }

                }
                else
                {
                    commissionText = "Not Calculated,Rule not defined";

                }

            }

            return null;
        }

        #endregion

        #region AUEC Rule Association

        #endregion

        #region Get_Single_Basket_Rules_List

        private List<CommissionRule> _allSingleCommissionRules = new List<CommissionRule>();
        //Returns all the Single and Both type of commission Rules.
        public List<CommissionRule> GetAllSingleCommissionRules()
        {
            _allSingleCommissionRules.Clear();
            foreach (CommissionRule commissionRule in _commissionRulesCache.AllCommissionRules)
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
            foreach (CommissionRule commissionRule in _commissionRulesCache.AllCommissionRules)
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
            //CommissionDBManager.SaveCompanyCommissionRulesForCVAUEC(_commissionRulesCache.CVAUECAccountCommissionRules);
            //_commissionRulesCache.CVAUECAccountCommissionRule = fasfdlkj
            ///Save into db
            //CommissionDBManager.SaveAndUpdateCommissionRule();



        }
        #endregion

        #region GetCommissionRulesForCVAUEC
        //public List<CVAUECAccountCommissionRule> GetCommissionRulesForCVAUEC(int companyID)
        //{
        //    //_commissionRulesCache.CVAUECAccountCommissionRules = CommissionDBManager.GetAllCommissionRulesForCVAUEC(companyID);
        //    //return _commissionRulesCache.CVAUECAccountCommissionRules;
        //}
        #endregion
    }
}
