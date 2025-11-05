using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
namespace Prana.BusinessObjects
{
    [Serializable]

    public class OrderFillCollection : CollectionBase, IBindingList
    {
        //OrderFill order = null;

        private Dictionary<string, OrderFill> _dictOrderCollection = new Dictionary<string, OrderFill>();
        public OrderFillCollection()
        {
        }
        #region " Class Declarations"

        [field: NonSerializedAttribute()]
        private ListChangedEventArgs resetEvent = null;

        #endregion

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
            OrderFill order = new OrderFill();
            //	List.Add(order);
            return order;
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

        public OrderFill this[int index]
        {
            get
            {
                return ((OrderFill)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public OrderFill this[string internalID]
        {
            get
            {
                OrderFill order = null;
                foreach (object obj in List)
                {
                    if (((OrderFill)obj).ClOrderID.ToString() == internalID)
                    {
                        order = (OrderFill)obj;
                    }
                }
                return order;
            }

        }
        #endregion

        #region " Public Methods"

        public int Add(OrderFill value)
        {
            try
            {
                if (!_dictOrderCollection.ContainsKey(value.ExecutionID))
                {
                    _dictOrderCollection.Add(value.ExecutionID, value);
                }
                return List.Add(value);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return -1;
            }

        }
        public bool Contains(OrderFill value)
        {
            return List.Contains(value);
        }
        public bool Contains(string ID)
        {
            return _dictOrderCollection.ContainsKey(ID);
        }
        public OrderFill AddNew2()
        {
            return ((OrderFill)(((IBindingList)this).AddNew()));
        }

        public void Remove(OrderFill value)
        {
            try
            {
                _dictOrderCollection.Remove(value.ExecutionID);
                List.Remove(value);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (ListChangedEvent != null)
                ListChangedEvent(this, ev);
        }

        protected override void OnClear()
        {
        }

        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        #endregion

        public OrderFill GetOrder(string executionID)
        {
            return _dictOrderCollection[executionID];
        }
    }


}
