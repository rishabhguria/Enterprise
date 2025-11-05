using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.Allocation.Common.Definitions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.AllocationNew
{
    class AllocationObjColl
    {
        List<AllocationCtrlObject> _collection = new List<AllocationCtrlObject>();
        
        public void SetValues(AllocationLevelList  list,bool isLong,PostTradeConstants.ORDERSTATE_ALLOCATION status)
        {

            if (status == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
            {
                _shouldClearPercentage = false;
            }
            else
            {
                _shouldClearPercentage = true;
            }
            foreach (Account account in AllocationStaticCollection.Accounts)
            {
                AllocationCtrlObject obj = new AllocationCtrlObject();
                obj.ISLong = isLong;
                AllocationLevelClass allocationAccount = list.GetAllocationLevel(account.AccountID); 

                if (allocationAccount != null)
                {
                    obj.SetValues(allocationAccount.LevelnID,allocationAccount.Percentage, allocationAccount.AllocatedQty, isLong);
                }
                _collection.Add(obj);

            }
        }
        public void SetPercentageValues(AllocationLevelList list, bool isLong, PostTradeConstants.ORDERSTATE_ALLOCATION status)
        {

            if (status == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
            {
                _shouldClearPercentage = false;
            }
            else
            {
                _shouldClearPercentage = true;
            }
            foreach (Account account in AllocationStaticCollection.Accounts)
            {
                AllocationCtrlObject obj = new AllocationCtrlObject();
                obj.ISLong = isLong;
                AllocationLevelClass allocationAccount = list.GetAllocationLevel(account.AccountID);

                if (allocationAccount != null)
                {
                    obj.SetValues(allocationAccount.LevelnID, allocationAccount.Percentage, double.MinValue, isLong);
                }
                _collection.Add(obj);

            }
        }
        public List<AllocationCtrlObject> Collection
        {
            get { return _collection; }
        }
        public AllocationLevelList GetAllocationLevelList(string groupID)
        {
            AllocationLevelList list = new AllocationLevelList();
            foreach (AllocationCtrlObject obj in _collection)
            {
                if (obj.Percentage > 0 )
                {
                    AllocationLevelClass account = new AllocationLevelClass(groupID);
                    account.AllocatedQty = obj.Qty;
                    account.LevelnID = obj.ID;
                    account.Percentage = obj.Percentage;
                    list.Add(account);
                }
            }
            return list;
        }
        public void  MergerLevel2(AllocationLevelList allocationList, AllocationObjColl level2Coll)
        {
           int i=0;
            foreach (AllocationCtrlObject obj in _collection)
            {
                if (obj.Percentage > 0)
                {
                    if (level2Coll.Collection[i].Percentage > 0)
                    {
                        AllocationLevelClass account= allocationList.GetAllocationLevel(_collection[i].ID);
                        AllocationLevelClass strategy = new AllocationLevelClass(account.GroupID);
                        AllocationCtrlObject stragyCtrl=   level2Coll.Collection[i];
                        strategy.AllocatedQty = stragyCtrl.Qty;
                        strategy.LevelnID = level2Coll.StrategyID;
                        strategy.Percentage = stragyCtrl.Percentage;
                        account.AddChilds(strategy);
                    }
                }
                i++;
            }
        }
        private bool _shouldClearPercentage;

        public bool ShouldClearPercentage
        {
            get { return _shouldClearPercentage; }
            set { _shouldClearPercentage = value; }
        }
        private int _strategyID;

        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }
	
        #region AllocationOperationPreference
	
        /// <summary>
        /// Set percetage value in Allocation control.
        /// </summary>
        /// <param name="targetDictionary"></param>
        /// <param name="isLong"></param>
        /// <param name="status"></param>
        /// <param name="level"></param>
        /// <param name="id"></param>
        internal void SetValues(SerializableDictionary<int, AccountValue> targetDictionary, bool isLong, PostTradeConstants.ORDERSTATE_ALLOCATION status,int level,int id)
        {
            try
            {
                if (status == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                {
                    _shouldClearPercentage = false;
                }
                else
                {
                    _shouldClearPercentage = true;
                }
                if (level == 1)
                {
                    foreach (Account account in AllocationStaticCollection.Accounts)
                    {
                        AllocationCtrlObject obj = new AllocationCtrlObject();
                        obj.ISLong = isLong;
                        //AllocationLevelClass allocationAccount = list.GetAllocationLevel(account.AccountID);
                        AccountValue value;
                        if (targetDictionary.ContainsKey(account.AccountID))
                            value = targetDictionary[account.AccountID];
                        else
                            value = new AccountValue(account.AccountID, 0);
                        if (value != null)
                        {
                            obj.SetValues(value.AccountId, (float)value.Value, double.MinValue, isLong);
                        }
                        _collection.Add(obj);

                    }
                }
                else if (level == 2)
                {
                    foreach (Account account in AllocationStaticCollection.Accounts)
                    {
                        AllocationCtrlObject obj = new AllocationCtrlObject();
                        obj.ISLong = isLong;
                        //AllocationLevelClass allocationAccount = list.GetAllocationLevel(account.AccountID);
                        AccountValue value;
                        if (targetDictionary.ContainsKey(account.AccountID))
                            value = targetDictionary[account.AccountID];
                        else
                            value = new AccountValue(account.AccountID, 0, new List<StrategyValue>());
                        if (value != null)
                        {
                            if (value.StrategyValueList.Count > 0)
                            {
                                foreach (StrategyValue strategyValue in value.StrategyValueList)
                                {
                                    if (strategyValue.StrategyId == id)
                                    {
                                        obj.SetValues(account.AccountID, (float)strategyValue.Value, double.MinValue, isLong);
                                        break;
                                    }
                                    else
                                    {
                                        obj.SetValues(account.AccountID, 0, double.MinValue, isLong);
                                    }
                                }
                            }
                            else
                            {
                                obj.SetValues(account.AccountID, 0, double.MinValue, isLong);
                            }
                        }
                        _collection.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            
        }
        #endregion
    }
}
