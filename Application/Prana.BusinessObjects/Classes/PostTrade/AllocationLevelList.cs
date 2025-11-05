using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class AllocationLevelList
    {
        private List<AllocationLevelClass> _collection = new List<AllocationLevelClass>();

        public AllocationLevelList()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void Add(AllocationLevelClass allocationLevelBase)
        {
            _collection.Add(allocationLevelBase);
        }
        public void Add(AllocationLevelList list)
        {
            _collection = new List<AllocationLevelClass>();
            foreach (AllocationLevelClass allocation in list.Collection)
            {

                _collection.Add(allocation);
            }
        }
        [XmlArray("Collection"), XmlArrayItem("AllocationLevelClass", typeof(AllocationLevelClass))]
        public List<AllocationLevelClass> Collection
        {
            get
            {
                return _collection;
            }
        }
        public AllocationLevelClass GetAllocationLevel(int id)
        {
            AllocationLevelClass temp = null;
            foreach (AllocationLevelClass allocationLevelClass in _collection)
            {
                if (allocationLevelClass.LevelnID == id)
                {
                    temp = allocationLevelClass;
                    break;
                }
            }
            return temp;
        }
        public AllocationLevelList GetSecondLevelAccounts(int level2ID)
        {
            AllocationLevelList strategyAccounts = new AllocationLevelList();
            foreach (AllocationLevelClass account in _collection)
            {
                if (account.Childs != null)
                {
                    foreach (AllocationLevelClass strategy in account.Childs.Collection)
                    {
                        if (strategy.LevelnID == level2ID)
                        {
                            AllocationLevelClass newaccount = (AllocationLevelClass)strategy.Clone();
                            newaccount.LevelnID = account.LevelnID;
                            strategyAccounts.Add(newaccount);
                        }
                    }
                }
            }
            return strategyAccounts;
        }

        public float GetSumOfPercentageLevel1()
        {
            float percentage = 0;
            foreach (AllocationLevelClass account in _collection)
            {
                percentage += account.Percentage;

            }
            return (Single)Math.Round(percentage, 4);
        }
        public int CheckSumOfPercentageLevel2()
        {

            foreach (AllocationLevelClass level1 in _collection)
            {
                float percentage = 0;
                if (level1.Childs != null)
                {
                    foreach (AllocationLevelClass level2 in level1.Childs.Collection)
                    {
                        percentage += level2.Percentage;
                    }
                    if (!(percentage == 0 || percentage == 100.0))
                    {
                        return level1.LevelnID;
                    }
                }
            }
            return 0;
        }

        public bool Contains(int AccountID)
        {
            foreach (AllocationLevelClass account in _collection)
            {
                if (account.LevelnID == AccountID)
                {
                    return true;
                }
            }

            return false;
        }

        public void Remove(int AccountID)
        {
            AllocationLevelClass accountToRemove = null;

            foreach (AllocationLevelClass account in _collection)
            {
                if (account.LevelnID == AccountID)
                {
                    accountToRemove = account;
                    break;
                }
            }

            if (accountToRemove != null)
            {
                _collection.Remove(accountToRemove);
            }

        }

        /// <summary>
        /// Merges the specified allocation level list.
        /// </summary>
        /// <param name="allocationLevelList">The allocation level list.</param>
        public void Merge(AllocationLevelList allocationLevelList)
        {
            try
            {
                if (allocationLevelList != null)
                {
                    foreach (AllocationLevelClass allocations in allocationLevelList.Collection)
                    {
                        AllocationLevelClass listItem = _collection.FirstOrDefault(x => x.LevelnID == allocations.LevelnID);
                        if (listItem != null)
                        {
                            allocations.AllocatedQty += listItem.AllocatedQty;

                            if (listItem.Childs != null && listItem.Childs.Collection.Count > 0)
                                allocations.Childs.Merge(listItem.Childs);

                            _collection.Remove(listItem);
                        }
                        _collection.Add(allocations);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
