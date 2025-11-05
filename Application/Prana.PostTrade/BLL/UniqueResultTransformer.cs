using NHibernate.Transform;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prana.PostTrade.BLL
{
    /// <summary>
    /// http://stackoverflow.com/questions/11950502/nhibernate-collection-of-collections-fetching-duplicate-data
    /// nHibernate fetches data from the db and apply mapping on its own. Afterwards it fills the data in respective object. If object graph is complex
    /// e.g. - in case of AllocationGroup; we might need to assign data in objects in specific manner. UniqueResultTransformer implments IResultTransformer
    /// to extend the functionality of nHibernate.
    /// </summary>
    internal class UniqueResultTransformer : IResultTransformer
    {
        /// <summary>
        /// This method removes the duplicates records from oldlist and form a new list without any duplicates.
        /// </summary>
        /// <param name="oldList">List that will be transformed</param>
        /// <returns>Merged list</returns>
        public List<AllocationGroup> MergeList(List<AllocationGroup> oldList)
        {
            if (!(oldList.Count > 0))
            {
                return oldList;
            }
            List<AllocationGroup> groups = new List<AllocationGroup>();
            // used extension method for getting distinct groups on basis of ID, PRANA-9780
            groups = (List<AllocationGroup>)oldList.DistinctBy(x => x.GroupID).ToList();
            Parallel.ForEach(groups, group => RemoveDuplicates(group));

            #region Legacy Code
            //List<String> idList = new List<string>();

            //for (int i = 0; i < oldList.Count; i++)
            //{
            //    if (!idList.Contains(oldList[i].GroupID))
            //    {
            //        idList.Add(oldList[i].GroupID);
            //        RemoveDuplicates(oldList[i]);
            //        groups.Add(oldList[i]);        

            //    }                
            //}
            ////clearing data
            //idList.Clear();
            #endregion

            oldList.Clear();
            return groups;
        }

        /// <summary>
        /// Remove duplicates from collections Order, Swap, Level1Allocation, Taxlots based on their Unique 
        /// ID as defined in corresponding hbm.xml file
        /// </summary>
        /// <param name="allocationGroup">A single Allocation Group from which duplicates needs to be removed</param>
        private void RemoveDuplicates(AllocationGroup allocationGroup)
        {
            List<int> removalList = new List<int>();

            // used extension method for getting distinct orders, swap parameters, taxlots in allocation group, PRANA-9780
            // Order removal
            allocationGroup.OrdersH = allocationGroup.OrdersH.DistinctBy(x => x.ClOrderID).ToList();

            // Swap removal
            allocationGroup.SwapParametersH = allocationGroup.SwapParametersH.DistinctBy(x => x.SwapPK).ToList();

            // Level1Allocation removal
            allocationGroup.Level1AllocationList = allocationGroup.Level1AllocationList.DistinctBy(x => x.AllocationId).ToList();

            // Taxlots removal
            Parallel.ForEach(allocationGroup.Level1AllocationList, lvl => { lvl.TaxLotsH = lvl.TaxLotsH.DistinctBy(x => x.TaxLotID).ToList(); });

            #region Legacy Code
            //#region Order removal
            //int orderCount = allocationGroup.OrdersH.Count;
            //for (int i = 0; i < orderCount; i++)
            //{
            //    for (int j = i + 1; j < orderCount; j++)
            //    {
            //        if (allocationGroup.OrdersH[i].ClOrderID == allocationGroup.OrdersH[j].ClOrderID)
            //        {
            //            if (!removalList.Contains(j))
            //                removalList.Add(j);
            //        }
            //    }
            //}

            ///// Sorts and reverses collection because index of a list is self adjustable 
            ///// so removing a lower index will automatically adjustes other higher indices.
            ///// So avoiding index out of range exception removal operation is performed from highest index.
            //removalList.Sort();
            //removalList.Reverse();
            //foreach (int k in removalList)
            //{
            //    allocationGroup.OrdersH.RemoveAt(k);
            //}
            //removalList.Clear();
            //#endregion

            //#region Swap removal
            //int swapCount = allocationGroup.SwapParametersH.Count;
            //for (int i = 0; i < swapCount; i++)
            //{
            //    for (int j = i + 1; j < swapCount; j++)
            //    {
            //        if (allocationGroup.SwapParametersH[i].SwapPK == allocationGroup.SwapParametersH[j].SwapPK)
            //        {
            //            if (!removalList.Contains(j))
            //                removalList.Add(j);
            //        }
            //    }
            //}
            //removalList.Sort();
            //removalList.Reverse();
            //foreach (int k in removalList)
            //{
            //    allocationGroup.SwapParametersH.RemoveAt(k);
            //}
            //removalList.Clear();
            //#endregion

            //#region Level1Allocation removal
            //int level1Count = allocationGroup.Level1AllocationList.Count;
            //for (int i = 0; i < level1Count; i++)
            //{
            //    for (int j = i + 1; j < level1Count; j++)
            //    {
            //        if (allocationGroup.Level1AllocationList[i].AllocationId == allocationGroup.Level1AllocationList[j].AllocationId)
            //        {
            //            if (!removalList.Contains(j))
            //                removalList.Add(j);
            //        }
            //    }
            //}
            //removalList.Sort();
            //removalList.Reverse();
            //foreach (int k in removalList)
            //{
            //    allocationGroup.Level1AllocationList.RemoveAt(k);
            //}
            //removalList.Clear();
            //#endregion

            //#region Taxlots removal
            //foreach (Level1Allocation lv1 in allocationGroup.Level1AllocationList)
            //{
            //    int taxlotCount = lv1.TaxLotsH.Count;
            //    for (int i = 0; i < taxlotCount; i++)
            //    {
            //        for (int j = i + 1; j < taxlotCount; j++)
            //        {
            //            if (lv1.TaxLotsH[i].TaxLotID== lv1.TaxLotsH[j].TaxLotID)
            //            {
            //                if (!removalList.Contains(j))
            //                    removalList.Add(j);
            //            }
            //        }
            //    }
            //    removalList.Sort();
            //    removalList.Reverse();
            //    foreach (int k in removalList)
            //    {
            //        lv1.TaxLotsH.RemoveAt(k);
            //    }
            //    removalList.Clear();
            //}
            //#endregion
            #endregion
        }

        #region IResultTransformer Members

        /// <summary>
        /// Transforms the list.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>System.Collections.IList.</returns>
        public System.Collections.IList TransformList(System.Collections.IList collection)
        {
            if (collection.Count == 0)
                return collection;

            List<String> idList = new List<String>();
            List<AllocationGroup> groupsOld = new List<AllocationGroup>();
            foreach (object[] ob in collection)
            {
                foreach (object obin in ob)
                {
                    groupsOld.Add(obin as AllocationGroup);
                }
            }

            List<AllocationGroup> groups = MergeList(groupsOld);

            return groups;
        }

        /// <summary>
        /// Transforms the tuple.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <param name="aliases">The aliases.</param>
        /// <returns>System.Object.</returns>
        public object TransformTuple(object[] tuple, string[] aliases)
        {
            //throw new Exception("The method or operation is not implemented.");
            return tuple;
        }

        #endregion
    }
}
