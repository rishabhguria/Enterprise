using System;
using System.Collections;
using System.ComponentModel;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for MPIDCollection.
    /// </summary>
    public class MPIDCollection : CollectionBase, IBindingList
    {
        public MPIDCollection()
        {
        }

        #region " Class Declarations"

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
            MPID mpid = new MPID(int.MinValue, int.MinValue, "");
            List.Add(mpid);
            return mpid;
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

        public MPID this[int index]
        {
            get
            {
                return ((MPID)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        #endregion

        #region " Public Methods"

        public int Add(MPID value)
        {
            return List.Add(value);
        }

        public MPID AddNew2()
        {
            return ((MPID)(((IBindingList)this).AddNew()));
        }

        public void Remove(MPID value)
        {
            List.Remove(value);
        }

        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (ListChangedEvent != null)
                ListChangedEvent(this, ev);
        }

        protected override void OnClear()
        {
            //MPID mpid;
            //foreach (MPID tempLoopVar_c in List)
            //{
            //    mpid = tempLoopVar_c;
            //    //mpid.parent = null;
            //}
        }

        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            //MPID mpid = ((MPID) value);
            //mpid.parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            //MPID mpid = ((MPID) value);
            //mpid.parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                //MPID oldmpid = ((MPID) oldValue);
                //MPID newmpid = ((MPID) newValue);

                //oldmpid.parent = null;
                //newmpid.parent = this;

                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        // Called by Customer when it changes.
        //internal void CustomerChanged (MPID mpid)
        //{
        //    int index = List.IndexOf(mpid);
        //    OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        //}
        #endregion
    }
}
