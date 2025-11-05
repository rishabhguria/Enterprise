using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for AllocationOrderCollection.
    /// </summary>

    [Serializable]
    public class AllocationOrderCollection
    {
        List<AllocationOrder> list = new List<AllocationOrder>();

        public AllocationOrderCollection()
        {
        }

        public double ExeQty
        {
            get
            {
                double sumOfExeQty = 0;
                foreach (AllocationOrder order in list)
                {
                    sumOfExeQty = order.CumQty + sumOfExeQty;
                }
                return sumOfExeQty;
            }
        }

        public double Quantity
        {
            get
            {
                double sumOfTotalQty = 0;
                foreach (AllocationOrder order in list)
                {
                    sumOfTotalQty = order.Quantity + sumOfTotalQty;
                }
                return sumOfTotalQty;
            }
        }

        #region " Implements IBindinglist"

        public bool AllowEdit
        {
            get
            {
                return true;
            }
        }

        public bool AllowNew
        {
            get
            {
                return true;
            }
        }

        public bool AllowRemove
        {
            get
            {
                return true;
            }
        }

        public bool SupportsChangeNotification
        {
            get
            {
                return true;
            }
        }

        public bool SupportsSearching
        {
            get
            {
                return false;
            }
        }

        public bool SupportsSorting
        {
            get
            {
                return false;
            }
        }

        // Events.



        // Methods.
        public object AddNew()
        {
            AllocationOrder order = new AllocationOrder();
            //SK20071207: Need to check why it is called and automatically.
            // LH defect #120 and 118 were coming because of this so commented for now.

            //list.Add(order);
            return order;
        } //IBindinglist.AddNew

        // Unsupported properties.
        public bool IsSorted
        {
            get
            {
                throw (new NotSupportedException());
            }
        }

        public PropertyDescriptor SortProperty
        {
            get
            {
                throw (new NotSupportedException());
            }
        }

        // Unsupported Methods.
        public void AddIndex()
        {
            throw (new NotSupportedException());
        } //IBindinglist.AddIndex



        public int Find()
        {
            throw (new NotSupportedException());
        } //IBindinglist.Find

        public void RemoveIndex()
        {
            throw (new NotSupportedException());
        } //IBindinglist.RemoveIndex

        public void RemoveSort()
        {
            throw (new NotSupportedException());
        } //IBindinglist.RemoveSort

        #endregion



        #region " Public Properties"

        public AllocationOrder this[int index]
        {
            get
            {
                return ((AllocationOrder)list[index]);
            }
            set
            {
                list[index] = value;
            }
        }

        #endregion

        #region " Public Methods"

        public void Add(AllocationOrder value)
        {
            try
            {
                list.Add(value);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Remove(AllocationOrder value)
        {
            list.Remove(value);
        }
        #endregion

        public void Clear()
        {
            list.Clear();
        }
    }
}