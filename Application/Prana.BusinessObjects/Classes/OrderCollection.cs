using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    [KnownType(typeof(Order))]
    public class OrderCollection : CollectionBase, IBindingList
    {
        private Dictionary<string, Order> _dictOrderCollection = new Dictionary<string, Order>();
        public OrderCollection()
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
            Order order = new Order(true);
            //List.Add(order);
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

        public Order this[int index]
        {
            get
            {
                return ((Order)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public Order this[string internalID]
        {
            get
            {
                Order order = null;
                foreach (object obj in List)
                {
                    if (((Order)obj).ClOrderID.ToString() == internalID)
                    {
                        order = (Order)obj;
                    }
                }
                return order;
            }

        }
        #endregion

        #region " Public Methods"

        public int Add(Order value)
        {
            try
            {
                if (!_dictOrderCollection.ContainsKey(value.ClientOrderID))
                {
                    _dictOrderCollection.Add(value.ClientOrderID, value);
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
        public bool Contains(Order value)
        {
            return List.Contains(value);
        }
        public bool Contains(string ID)
        {
            return _dictOrderCollection.ContainsKey(ID);
        }
        public Order AddNew2()
        {
            return ((Order)(((IBindingList)this).AddNew()));
        }

        public void Remove(Order value)
        {
            try
            {
                _dictOrderCollection.Remove(value.ClientOrderID);
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

        public Order GetOrder(string clientOrderID)
        {
            if (_dictOrderCollection.ContainsKey(clientOrderID))
            {
                return _dictOrderCollection[clientOrderID];
            }
            else
            {
                return null;
            }
        }
        public OrderCollection Clone()
        {
            OrderCollection clonedOrders = new OrderCollection();
            foreach (Order order in List)
            {
                clonedOrders.Add((Order)order.Clone());
            }
            return clonedOrders;
        }
        public double CumQty
        {
            get
            {
                double cumQty = 0;
                foreach (Order order in List)
                {
                    cumQty = cumQty + order.CumQty;
                }
                return cumQty;
            }

        }
        public double Quantity
        {
            get
            {
                double quantity = 0;
                foreach (Order order in List)
                {
                    quantity = quantity + order.Quantity;
                }
                return quantity;
            }

        }
        public double ExeValue
        {
            get
            {
                double exeValue = 0;
                foreach (Order order in List)
                {
                    exeValue = exeValue + order.CumQty * order.AvgPrice;
                }
                return exeValue;
            }

        }
        public List<string> SymbolList
        {
            get
            {
                List<string> symbolList = new List<string>();
                foreach (Order order in List)
                {
                    string upperSymbol = order.Symbol.ToUpper().Trim();
                    if (upperSymbol != string.Empty)
                    {
                        if (!symbolList.Contains(upperSymbol))
                        {
                            symbolList.Add(upperSymbol);
                        }
                    }

                }
                return symbolList;
            }
        }

    }


}
