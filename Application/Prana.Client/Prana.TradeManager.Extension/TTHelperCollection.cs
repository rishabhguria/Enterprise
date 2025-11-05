using System;
using System.Collections;
using System.ComponentModel;

namespace Prana.TradeManager.Extension
{
    /// <summary>
    /// Summary description for TTHelperCollection.
    /// </summary>
    public sealed class TTHelperCollection : CollectionBase, IBindingList
    {
        public TTHelperCollection()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        public TTHelper this[int index]
        {
            get
            {
                return (TTHelper)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(TTHelper value)
        {
            return List.Add(value);
        }

        public TTHelper AddNew()
        {
            return (TTHelper)((IBindingList)this).AddNew();
        }

        public void Remove(TTHelper value)
        {
            List.Remove(value);
        }


        private void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        protected override void OnClear()
        {
            //			foreach (PNLLevel1Data c in List) 
            //			{
            //				c.Parent = null;
            //			}
        }

        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            //TTHelper c = (TTHelper)value;
            //c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            //TTHelper c = (TTHelper)value;
            //c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                //TTHelper oldcust = (TTHelper)oldValue;
                //TTHelper newcust = (TTHelper)newValue;

                //oldcust.Parent = null;
                //newcust.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        // Called by TTHelper when it changes.
        //internal void TTHelperChanged(TTHelper cust) 
        //{

        //    int index = List.IndexOf(cust);

        //    OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        //}


        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return true; }
        }

        bool IBindingList.AllowRemove
        {
            get { return true; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }


        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }

        // Methods.
        object IBindingList.AddNew()
        {
            TTHelper c = new TTHelper();
            List.Add(c);
            return c;
        }


        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }


        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }
}
