using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    //[NonSerialized]
    public class ClearanceDataCollection : System.Collections.CollectionBase, IBindingList
    {
        // ArrayList _clearanceData = new ArrayList();


        /// <summary>
        /// ClearanceDataCollection
        /// </summary>
        public ClearanceDataCollection()
        {

        }
        public ClearanceData this[int index]
        {
            get
            {
                return ((ClearanceData)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

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


        public void Add(ClearanceData clearanceData)
        {
            List.Add(clearanceData);

        }

        // Methods.
        public object AddNew()
        {
            ClearanceData clearanceData = new ClearanceData();
            List.Add(clearanceData);
            return clearanceData;
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

        #region IEnumerable Members

        public new System.Collections.IEnumerator GetEnumerator()
        {
            return (new ClearanceDataEnumerator(this));
        }
        public class ClearanceDataEnumerator : System.Collections.IEnumerator
        {
            ClearanceDataCollection clearanceDataCollection;
            int _location;

            public ClearanceDataEnumerator(ClearanceDataCollection recdClearanceDataCollection)
            {
                clearanceDataCollection = recdClearanceDataCollection;
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
                    if ((_location < 0) || (_location >= clearanceDataCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return clearanceDataCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= clearanceDataCollection.Count)
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
    }
}
