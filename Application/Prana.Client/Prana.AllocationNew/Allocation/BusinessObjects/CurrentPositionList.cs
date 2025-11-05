using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.Global;
namespace Prana.AllocationNew
{
    class CurrentPositionList
    {
        static Dictionary<string, List<CurrentPosition>> _coll = new Dictionary<string, List<CurrentPosition>>();
        public static void AddPosition(CurrentPosition position)
        {
            if (!_coll.ContainsKey(position.Symbol))
            {
                List<CurrentPosition> accountList = new List<CurrentPosition>();
                accountList.Add(position);
                _coll.Add(position.Symbol, accountList);
            }
            else
            {
                _coll[position.Symbol].Add(position);
            }
        }
        public static AllocationGroup GetAllocationGroup(string symbol)
        {
            AllocationGroup group = new AllocationGroup();

            try
            {
                if (_coll.ContainsKey(symbol))
                {
                    double totalQty = 0;
                    foreach (CurrentPosition position in _coll[symbol])
                    {
                        totalQty +=Math.Abs(position.Qty);
                    }
                    group.CumQty = totalQty;
                    group.State = PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED;
                    AllocationLevelList allocationLevelList = new AllocationLevelList();
                    foreach (CurrentPosition position in _coll[symbol])
                    {
                        AllocationLevelClass allocationLevel = new AllocationLevelClass(group.GroupID);
                        allocationLevel.LevelnID = position.AccountID;
                        allocationLevel.AllocatedQty = position.Qty;
                        allocationLevel.Percentage = 0;
                        if (totalQty != 0)
                        {
                            allocationLevel.Percentage = Math.Abs(Convert.ToSingle((position.Qty * 100) / totalQty));
                        }
                        if(allocationLevel.Percentage !=0)
                        allocationLevelList.Add(allocationLevel);
                    }
                    group.AddAccounts(allocationLevelList);
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
            return group;
            
        }
        public static void Clear()
        {
            _coll.Clear();
        }
    }
}
