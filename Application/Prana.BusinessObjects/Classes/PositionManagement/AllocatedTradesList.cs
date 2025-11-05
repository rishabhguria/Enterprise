using Csla;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.PositionManagement
{
    [Serializable()]
    [System.Runtime.InteropServices.ComVisible(false)]
    public class AllocatedTradesList : BusinessListBase<AllocatedTradesList, AllocatedTrade>
    {
        private List<AllocatedTrade> listOftaxlots = new List<AllocatedTrade>();
        /// <summary>
        /// returns the positional taxlots contained in this instance of Allocated trades List.
        /// Used iterator for the first time.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AllocatedTrade> PositionalTaxLots()
        {
            for (int counter = 0; counter < this.Count; counter++)
            {
                if (this[counter].IsPosition)
                    yield return this[counter];
            }
        }

        /// <summary>
        /// Gets the taxlot for positionID passed
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public AllocatedTrade GetPositionalTaxLotForPositionID(Guid guid)
        {
            foreach (AllocatedTrade taxlot in PositionalTaxLots())
            {
                if (taxlot.IsPosition && taxlot.PositionTaxlotID.Equals(guid))
                {
                    return taxlot;
                }
            }
            return null;
        }

        /// <summary>
        /// Same method as given above as IsPosition is not correctly used.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public AllocatedTrade GetPositionalTaxLotForPositionIDNew(Guid guid)
        {
            foreach (AllocatedTrade taxlot in this)
            {
                if (taxlot.PositionTaxlotID.Equals(guid))
                {
                    return taxlot;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the positional tax lot for taxlotID passed
        /// </summary>
        /// <param name="taxLotID">The tax lot ID.</param>
        /// <returns></returns>
        public AllocatedTrade GetPositionalTaxLotForTaxLotID(string taxLotID)
        {
            foreach (AllocatedTrade taxlot in PositionalTaxLots())
            {
                if (taxlot.ID.Equals(taxLotID))
                {
                    return taxlot;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the tax lot for ID.
        /// </summary>
        /// <param name="taxlotID">The taxlot ID.</param>
        /// <returns></returns>
        public AllocatedTrade GetTaxLotForID(string taxlotID)
        {
            foreach (AllocatedTrade taxlot in this)
            {
                if (string.Equals(taxlot.ID, taxlotID))
                {
                    return taxlot;
                }

            }
            return null;
        }


        /// <summary>
        /// Removes zero open quantity taxlots from the list
        /// </summary>
        /// <param name="taxlotID"></param>
        /// <returns></returns>
        public AllocatedTrade RemoveZeroOpenQtyTaxlots()
        {
            AllocatedTrade[] arrayAllocatedTrades = new AllocatedTrade[this.Count];
            this.CopyTo(arrayAllocatedTrades, 0); ;

            foreach (AllocatedTrade taxlot in arrayAllocatedTrades)
            {
                if (taxlot.OpenQty == 0)
                {
                    this.Remove(taxlot);
                }

            }
            return null;
        }



        ///// <summary>
        ///// Gets or sets the <see cref="Prana.PM.BLL.AllocatedTrade"/> with the specified allocatedTrade id to find.
        ///// </summary>
        ///// <value></value>        
        //public AllocatedTrade this[long allocatedTradeIDToFind]
        //{
        //    get
        //    {
        //        AllocatedTrade foundAllocatedTrade = null;

        //        foreach (AllocatedTrade allocatedTrade in this)
        //        {
        //            if (allocatedTrade.ID.Equals(allocatedTradeIDToFind))
        //            {
        //                foundAllocatedTrade = allocatedTrade;
        //                break;
        //            }
        //        }

        //        return foundAllocatedTrade;

        //    }
        //    set
        //    {
        //        for (int i = 0; i < this.Count; i++)
        //        {
        //            if (this[i].ID.Equals(allocatedTradeIDToFind))
        //            {
        //                this[i] = value;
        //                break;
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the <see cref="Prana.PM.BLL.AllocatedTrade"/> with the specified allocatedTrade id to find.
        ///// </summary>
        ///// <value></value>        
        //public new AllocatedTrade this[int counter]
        //{
        //     get
        //    {
        //        AllocatedTrade foundAllocatedTrade = null;

        //        foundAllocatedTrade = this[counter];
        //        foreach (AllocatedTrade allocatedTrade in this)
        //        {
        //            if(int.Equals(counter, this.IndexOf(allocatedTrade)))
        //            {
        //                foundAllocatedTrade = allocatedTrade;
        //                break;
        //            }

        //        }
        //        return foundAllocatedTrade;
        //    }            
        //}

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        //public new AllocatedTradesList Clone()
        //{
        //    AllocatedTradesList allocatedTradesList = new AllocatedTradesList();

        //    foreach (AllocatedTrade allocatedTrade in allocatedTradesList)
        //    {
        //        AllocatedTrade clone = allocatedTrade.Clone();                
        //        allocatedTradesList.Add(clone);
        //    }
        //    return allocatedTradesList;

        //}

        public new AllocatedTradesList Clone()
        {

            return (AllocatedTradesList)GetClone();
        }

        public new void Add(AllocatedTrade allocatedTrade)
        {
            base.Add(allocatedTrade);

            //if (!_sortedList.ContainsKey(taxlot.TradeDate))
            //{
            //    //_sortedList.Add(taxlot.TradeDate, taxlot);
            //}
            //else
            //{

            //}
        }
        //public bool RemoveFromList(AllocatedTrade allocatedTrade)
        //{
        //    if (allocatedTrade.OpenQty > 0)
        //    {
        //        listOftaxlots.Remove(allocatedTrade);
        //    }
        //}
        public void RemoveZeroOpenQtyFromList()
        {
            if (this.listOftaxlots.Count > 0)
            {
                AllocatedTrade[] arrayAllocatedTrades = new AllocatedTrade[this.listOftaxlots.Count];

                this.CopyTo(arrayAllocatedTrades, 0);

                foreach (AllocatedTrade taxlot in arrayAllocatedTrades)
                {
                    if (taxlot.OpenQty == 0)
                    {
                        this.listOftaxlots.Remove(taxlot);
                    }

                }
            }
        }

        public void CreateBucket()
        {
            foreach (AllocatedTrade taxlot in this.Items)
            {
                listOftaxlots.Add(taxlot);
            }
            listOftaxlots.Sort(delegate (AllocatedTrade t1, AllocatedTrade t2) { return t1.TradeDate.CompareTo(t2.TradeDate); });

        }
        //  SortedList<DateTime, List<AllocatedTrade>> _sortedList = new SortedList<DateTime, List<AllocatedTrade>>();
        //public void Add(AllocatedTrade taxlot)
        //{
        //    if (!_sortedList.ContainsKey(taxlot.TradeDate))
        //    {
        //        //_sortedList.Add(taxlot.TradeDate, taxlot);
        //    }
        //    else
        //    {

        //    }

        //}
        //public void Remove(DateTime date)
        //{
        //    if (_sortedList.ContainsKey(date))
        //    {
        //        List<AllocatedTrade> taxlots = _sortedList[date];
        //        List<AllocatedTrade> deletedList = new List<AllocatedTrade>();
        //        foreach (AllocatedTrade taxlot in taxlots)
        //        {
        //            if (taxlot.OpenQty == 0)
        //            {
        //                base.Remove(taxlot);
        //                deletedList.Add(taxlot);

        //            }

        //        }
        //        foreach (AllocatedTrade taxlot in deletedList)
        //        {
        //            taxlots.Remove(taxlot);
        //        }
        //        if (taxlots.Count == 0)
        //        {
        //            _sortedList.Remove(date);
        //        }


        //    }
        //    else
        //    {
        //        return null;
        //    }


        //}

        //public List<AllocatedTrade> GetTaxLotsByDate(DateTime date)
        //{
        //    List<AllocatedTrade> taxlotList = new List<AllocatedTrade>();

        //    foreach (AllocatedTrade sortedItem in listOftaxlots)
        //    {
        //        if (sortedItem.TradeDate.Date <= date)
        //        {
        //            taxlotList.Add(sortedItem);
        //         } 
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    return taxlotList;
        //}

        public List<DateTime> GetSortedDates()
        {
            List<DateTime> SortedDates = new List<DateTime>();
            foreach (AllocatedTrade sorteditem in listOftaxlots)
            {
                if (!SortedDates.Contains(sorteditem.TradeDate.Date))
                {
                    SortedDates.Add(sorteditem.TradeDate.Date);
                }
            }

            return SortedDates;
        }

        public List<AllocatedTrade> GetTaxlotList()
        {
            return listOftaxlots;
        }
    }
}
