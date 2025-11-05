using Prana.LogManager;
using System;
using System.Collections;
using System.ComponentModel;
namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for AllocationGroups.
    /// </summary>

    public class AllocationGroupCollection : CollectionBase, IBindingList
    {

        private ListChangedEventArgs resetEvent = null;

        public AllocationGroupCollection()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public double SumOfExeQuantity()
        {
            double sumOfExeQty = 0;
            foreach (AllocationGroup group in List)
            {
                sumOfExeQty = group.CumQty + sumOfExeQty;
            }
            return sumOfExeQty;
        }

        public double SumOfTotalQuantity()
        {
            double sumOfTotalQty = 0;
            foreach (AllocationGroup group in List)
            {
                sumOfTotalQty = group.Quantity + sumOfTotalQty;
            }
            return sumOfTotalQty;
        }


        #region " Implements IBindingList"

        public bool AllowEdit
        {
            get
            {

                //return false;
                return true;
            }
        }

        public bool AllowNew
        {
            get
            {
                return false;
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
            AllocationGroup group = new AllocationGroup();
            //List.Add(group);
            return group;
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

        #region ClientEnumerator Class

        public class ClientEnumerator : IEnumerator
        {
            AllocationGroupCollection _groups;
            int _location;

            public ClientEnumerator(AllocationGroupCollection groups)
            {
                _groups = groups;
                _location = -1;
            }

            #region IEnumerator Members
            public void Reset()
            {
                _location = -1;
            }
            public object Current
            {
                get
                {
                    if ((_location < 0) || (_location >= _groups.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _groups[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _groups.Count)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            #endregion
        }

        #endregion

        #region " Public Properties"

        public AllocationGroup this[int index]
        {
            get
            {
                return ((AllocationGroup)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        #endregion

        #region " Public Methods"

        public int Add(AllocationGroup value)
        {

            return List.Add(value);

        }

        public AllocationGroup AddNew2()
        {
            return ((AllocationGroup)(((IBindingList)this).AddNew()));
        }

        public void Remove(AllocationGroup value)
        {
            if (List.Contains(value))
            {
                List.Remove(value);
            }
            else
            {
                AllocationGroup grpToDelete = null;
                foreach (AllocationGroup grp in List)
                {
                    if (grp.GroupID == value.GroupID)
                    {
                        grpToDelete = grp;
                        break;
                    }

                }
                if (grpToDelete != null)
                {
                    List.Remove(grpToDelete);
                }

            }
        }


        public new void Clear()
        {

            base.Clear();

        }

        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (ListChangedEvent != null) //&& bAllowEvent)
                ListChangedEvent(this, ev);
        }

        protected override void OnClear()
        {
            try
            {
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

        protected override void OnClearComplete()
        {
            resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
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
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
            }
        }

        #endregion

        public bool Contains(AllocationGroup group)
        {
            return List.Contains(group);
        }


        public void ClearCollection()
        {

        }

    }
}
