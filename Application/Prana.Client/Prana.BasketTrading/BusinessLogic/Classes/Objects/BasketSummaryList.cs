using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using Prana.BusinessObjects;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.BasketTrading
{
    class BasketSummaryList : CollectionBase, IBindingList
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
            BasketSummaryView basketSummaryView = new BasketSummaryView();
            //List.Add(basketSummaryView);
            return basketSummaryView;
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

        public BasketSummaryView this[int index]
        {
            get
            {
                return ((BasketSummaryView)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        //public BasketSummaryView this[string internalID]
        //{
        //    get
        //    {
        //        BasketSummaryView basketSummaryView = null;
        //        foreach (object obj in List)
        //        {
        //            if (((BasketSummaryView)obj).ClOrderID.ToString() == internalID)
        //            {
        //                basketSummaryView = (BasketSummaryView)obj;
        //            }
        //        }
        //        return basketSummaryView;
        //    }

        //}
        #endregion

       

        public int Add(BasketSummaryView value)
        {
            try
            {
                return List.Add(value);
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
            //BasketSummaryView basketSummaryView;
            //foreach (BasketSummaryView tempLoopVar_c in List)
            //{
            //    basketSummaryView = tempLoopVar_c;
            //}
        }

        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            //BasketSummaryView basketSummaryView = ((BasketSummaryView)value);
            //mpid.parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            //BasketSummaryView basketSummaryView = ((BasketSummaryView)value);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                //BasketSummaryView oldorder = ((BasketSummaryView)oldValue);
                //BasketSummaryView neworder = ((BasketSummaryView)newValue);

                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }
       
    }
}
