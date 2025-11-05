using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prana.BusinessObjects
{
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class ExposureAndPnlOrderCollection : KeyedCollection<string, EPnlOrder>
    {
        // This parameterless constructor calls the base class constructor
        // that specifies a dictionary threshold of 0, so that the internal
        // dictionary is created as soon as an item is added to the 
        // collection.
        //
        public ExposureAndPnlOrderCollection() : base(null, 0) { }

        // This is the only method that absolutely must be overridden,
        // because without it the KeyedCollection cannot extract the
        // keys from the items. 
        //

        private DateTime _lastDBPickTime;

        public DateTime LastDBPickTime
        {
            get { return _lastDBPickTime; }
            set { _lastDBPickTime = value; }
        }

        private List<string> _keys = null;

        public List<string> Keys
        {
            get
            {
                if (base.Dictionary != null && base.Dictionary.Keys.Count > 0)
                {
                    _keys = new List<string>((IEnumerable<string>)base.Dictionary.Keys);
                }
                return _keys;
            }
        }


        //this is the indexer (readonly)
        //public virtual EPnlOrder this[string id]
        //{

        //    get
        //    {
        //        //return the EPnlOrder for the corresponding key
        //        return (EPnlOrder)this[id];
        //    }
        //}

        public void UpdateOrder(EPnlOrder order)
        {
            this.Remove(order.ID);
            this.Add(order);
        }

        protected override string GetKeyForItem(EPnlOrder item)
        {
            string key = string.Empty;

            key = item.ID;

            //switch (item.OrderType)
            //{
            //    case Prana.BusinessObjects.AppConstants.ConsolidationInfoType.Trade:
            //        key = item.ClOrderID;
            //        break;
            //    case Prana.BusinessObjects.AppConstants.ConsolidationInfoType.TaxLot:
            //        key = item.TaxLotID.ToString();
            //        break;
            //    case Prana.BusinessObjects.AppConstants.ConsolidationInfoType.Position:
            //        key = item.PositionID;
            //        break;
            //}

            return key;
        }

        /// <summary>
        /// It sorts the present collection
        /// </summary>
        /// <returns></returns>
        public void Sort()
        {
            List<EPnlOrder> list = new List<EPnlOrder>();
            foreach (EPnlOrder order in this)
            {
                list.Add(order);
            }
            list = list.OrderBy(i => i.AUECLocalDate).ToList();

            //EpnlOrderComparer comparer = new EpnlOrderComparer(AppConstants.SortingOrder.Ascending);
            //list.Sort(comparer);

            this.ClearItems();

            foreach (EPnlOrder order in list)
            {
                this.Add(order);
            }
        }

        public void AddRange(ExposureAndPnlOrderCollection epnlCollection)
        {
            foreach (EPnlOrder order in epnlCollection)
            {
                this.Add(order);
            }
        }


        private bool _isUpdated = false;

        public bool IsUpdated
        {
            get { return _isUpdated; }
            set { _isUpdated = value; }
        }

        public void RemoveOrders(string symbol, int accountID, int splittedTaxlotsCacheBasis, out Dictionary<String, EPnlOrder> ordersToBeRemoved)
        {
            List<string> removableIDs = new List<string>();
            ordersToBeRemoved = new Dictionary<String, EPnlOrder>();
            foreach (EPnlOrder order in this)
            {
                if (((splittedTaxlotsCacheBasis == 1 && order.MasterFundID == accountID) || order.Level1ID == accountID) && order.Symbol.Equals(symbol))
                {
                    removableIDs.Add(order.ID);
                    order.EpnlOrderState = Global.ApplicationConstants.TaxLotState.Deleted;
                    ordersToBeRemoved.Add(order.ID, order.Clone());
                }
            }

            foreach (string id in removableIDs)
            {
                this.Remove(id);
            }
        }
    }
}
