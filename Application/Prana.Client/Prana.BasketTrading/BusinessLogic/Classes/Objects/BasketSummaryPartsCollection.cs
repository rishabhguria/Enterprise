using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
namespace Prana.BasketTrading
{
    class BasketSummaryPartsCollection : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = null;

        #region " Implements IBindingList"

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
        private ListChangedEventHandler ListChangedEvent;
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                ListChangedEvent = (ListChangedEventHandler)System.Delegate.Combine(ListChangedEvent, value);
            }
            remove
            {
                ListChangedEvent = (ListChangedEventHandler)System.Delegate.Remove(ListChangedEvent, value);
            }
        }


        // Methods.
        public object AddNew()
        {
            BasketSummaryParts basketSummaryParts = new BasketSummaryParts();
            //List.Add(basketSummaryParts);
            return basketSummaryParts;
        } //IBindingList.AddNew

        // Unsupported properties.
        public bool IsSorted
        {
            get
            {
                throw (new NotSupportedException());
            }
        }

        public ListSortDirection SortDirection
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
        public void AddIndex(PropertyDescriptor prop)
        {
            throw (new NotSupportedException());
        } //IBindingList.AddIndex

        public void ApplySort(PropertyDescriptor prop, ListSortDirection direction)
        {
            throw (new NotSupportedException());
        } //IBindingList.ApplySort

        public int Find(PropertyDescriptor prop, object key)
        {
            throw (new NotSupportedException());
        } //IBindingList.Find

        public void RemoveIndex(PropertyDescriptor prop)
        {
            throw (new NotSupportedException());
        } //IBindingList.RemoveIndex

        public void RemoveSort()
        {
            throw (new NotSupportedException());
        } //IBindingList.RemoveSort

        #endregion

        #region " Public Properties"

        public BasketSummaryParts this[int index]
        {
            get
            {
                return ((BasketSummaryParts)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        //public BasketSummaryParts this[string internalID]
        //{
        //    get
        //    {
        //        BasketSummaryParts basketSummaryParts = null;
        //        foreach (object obj in List)
        //        {
        //            if (((BasketSummaryParts)obj).ClOrderID.ToString() == internalID)
        //            {
        //                basketSummaryParts = (BasketSummaryParts)obj;
        //            }
        //        }
        //        return basketSummaryParts;
        //    }

        //}
        #endregion



        public int Add(BasketSummaryParts value)
        {
            try
            {
                return List.Add(value);
                //if (value.Side == "Long")
                //{

                //}
                //else if (value.Side == "Short")
                //{ 

                //}
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return -1;
            }

        }

        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (ListChangedEvent != null)
                ListChangedEvent(this, ev);
        }

        protected override void OnClear()
        {
            //BasketSummaryParts basketSummaryParts;
            //foreach (BasketSummaryParts tempLoopVar_c in List)
            //{
            //    basketSummaryParts = tempLoopVar_c;
            //}
        }

        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            //BasketSummaryParts basketSummaryParts = ((BasketSummaryParts)value);
            //mpid.parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            //BasketSummaryParts basketSummaryParts = ((BasketSummaryParts)value);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                //BasketSummaryParts oldorder = ((BasketSummaryParts)oldValue);
                //BasketSummaryParts neworder = ((BasketSummaryParts)newValue);

                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }       
    }
}
