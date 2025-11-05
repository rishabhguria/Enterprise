#region IList Version

//using System;
//using System.Collections;
//
//namespace Prana.LiveFeedProvider
//{
//	/// <summary>
//	/// Summary description for FullOptionDataCollection.
//	/// </summary>
//	public class FullOptionDataCollection : IList
//	{
//		ArrayList _fullOptionDataCollection = new ArrayList();
//
//		public FullOptionDataCollection()
//		{
//		}
//
//		#region IList Members
//
//		public bool IsReadOnly
//		{
//			get
//			{
//				return _fullOptionDataCollection.IsReadOnly;
//			}
//		}
//
//		public object this[int index]
//		{
//			get
//			{
//				return _fullOptionDataCollection[index];
//			}
//			set
//			{
//				_fullOptionDataCollection[index] = (FullOptionData) value; 
//			}
//		}
//
//		public void RemoveAt(int index)
//		{
//			_fullOptionDataCollection.RemoveAt(index);
//		}
//
//		public void Insert(int index, object value)
//		{
//			_fullOptionDataCollection.Insert(index,(FullOptionData)value);
//		}
//
//		public void Remove(object value)
//		{
//			_fullOptionDataCollection.Remove((FullOptionData)value);
//		}
//
//		public bool Contains(object value)
//		{
//			return _fullOptionDataCollection.Contains((FullOptionData)value);
//		}
//
//		public void Clear()
//		{
//			_fullOptionDataCollection.Clear();
//		}
//
//		public int IndexOf(object value)
//		{
//			return _fullOptionDataCollection.IndexOf((FullOptionData)value);
//		}
//
//		public int Add(object value)
//		{
//			return _fullOptionDataCollection.Add((FullOptionData)value);
//		}
//
//		public bool IsFixedSize
//		{
//			get
//			{
//				return _fullOptionDataCollection.IsFixedSize;
//			}
//		}
//
//		#endregion
//
//		#region ICollection Members
//
//		public bool IsSynchronized
//		{
//			get
//			{
//				return _fullOptionDataCollection.IsSynchronized;
//			}
//		}
//
//		public int Count
//		{
//			get
//			{
//				return _fullOptionDataCollection.Count;
//			}
//		}
//
//		public void CopyTo(Array array, int index)
//		{
//			_fullOptionDataCollection.CopyTo(array,index);
//		}
//
//		public object SyncRoot
//		{
//			get
//			{
//				return _fullOptionDataCollection.SyncRoot;
//			}
//		}
//
//		#endregion
//
//		#region IEnumerable Members
//
//		public IEnumerator GetEnumerator()
//		{
//			return (new FullOptionDataEnumerator(this));
//		}
//
//		#endregion
//
//		#region FullOptionDataEnumerator Class
//
//		public class FullOptionDataEnumerator: IEnumerator
//		{
//			FullOptionDataCollection  _fullOptionDataCollection;
//			int _location;
//
//			public FullOptionDataEnumerator (FullOptionDataCollection fullOptionDataCollection)
//			{
//				_fullOptionDataCollection = fullOptionDataCollection;
//				_location = -1;
//			}
//
//			#region IEnumerator Members
//
//			public void Reset()
//			{
//				_location = -1;	
//			}
//			public object Current
//			{
//				get
//				{
//					if ((_location < 0) || (_location >= _fullOptionDataCollection.Count))
//					{
//						throw (new InvalidOperationException());
//					}
//					else
//					{
//						return _fullOptionDataCollection[_location];
//					}
//				}
//			}
//
//			public bool MoveNext()
//			{
//				_location++;
//
//				if (_location >= _fullOptionDataCollection.Count)
//				{
//					return false;
//				}
//				else
//				{
//					return true;
//				}
//			}            
//			#endregion
//		}
//
//		#endregion
//	}
//}

#endregion

#region IBindingList version

using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;


namespace Prana.BusinessObjects.LiveFeed
{
    /// <summary>
    /// This class contains the full option data collection. We have created this
    /// class in order to directly bind it from the Infragistics DataGrid by implementing 
    /// the interface IBindingList. Deriving from the CollectionBase class makes it strongly typed collection
    /// TODO : Need to change the sorting - properly and also updates should be directly shown on the grid.
    /// </summary>
    [Serializable]
    public class FullOptionDataCollection : CollectionBase, IBindingList
    {
        public FullOptionDataCollection()
        {
        }

        [field: NonSerializedAttribute()]
        private ListChangedEventArgs resetEvent = null;
        private ArrayList sortedList;
        private ArrayList unsortedItems;

        #region IBindingList Members

        public void AddIndex(PropertyDescriptor property)
        {
            throw (new NotSupportedException());
        }

        public bool AllowNew
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Sorts the items if overridden in a derived class; otherwise, throws a <see cref="T:System.NotSupportedException"></see>.
        /// </summary>
        /// <param name="prop">A <see cref="T:System.ComponentModel.PropertyDescriptor"></see> that specifies the property to sort on.</param>
        /// <param name="direction">One of the <see cref="T:System.ComponentModel.ListSortDirection"></see>  values.</param>
        public void ApplySort(PropertyDescriptor prop, ListSortDirection direction)
        {
            sortedList = new ArrayList();

            // Check to see if the property type we are sorting by implements
            // the IComparable interface.
            Type interfaceType = prop.PropertyType.GetInterface("IComparable");

            if (interfaceType != null)
            {
                // If so, set the SortPropertyValue and SortDirectionValue.
                sortProperty = prop;
                sortDirection = direction;

                unsortedItems = new ArrayList(this.Count);

                // Loop through each item, adding it the the sortedItems ArrayList.
                foreach (Object item in this)
                {
                    sortedList.Add(prop.GetValue(item));
                    unsortedItems.Add(item);
                }
                // Call Sort on the ArrayList.
                sortedList.Sort();
                FullOptionData temp;

                // Check the sort direction and then copy the sorted items
                // back into the list.
                if (direction == ListSortDirection.Descending)
                    sortedList.Reverse();

                for (int i = 0; i < this.Count; i++)
                {
                    // Check the properties for a property with the specified name.
                    PropertyDescriptorCollection properties =
                        TypeDescriptor.GetProperties(typeof(FullOptionData));
                    PropertyDescriptor property = properties.Find(prop.Name, true);

                    int position = Find(property, sortedList[i]);
                    if (position != i)
                    {
                        temp = this[i];
                        this[i] = this[position];
                        this[position] = temp;
                    }
                }

                isSorted = true;

                // Raise the ListChanged event so bound controls refresh their
                // values.
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
            else
                // If the property type does not implement IComparable, let the user
                // know.
                throw new NotSupportedException("Cannot sort by " + prop.Name +
                    ". This" + prop.PropertyType.ToString() +
                    " does not implement IComparable");
        }

        [field: NonSerializedAttribute()]
        PropertyDescriptor sortProperty;
        /// <summary>
        /// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns null.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor"></see> used for sorting the list.</returns>
        public PropertyDescriptor SortProperty
        {
            get { return sortProperty; }
        }


        /// <summary>
        /// Implementation to search the list.
        /// </summary>
        /// <param name="prop">The prop.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public int Find(PropertyDescriptor prop, object key)
        {
            // Get the property info for the specified property.
            PropertyInfo propInfo = typeof(FullOptionData).GetProperty(prop.Name);
            FullOptionData item;

            if (key != null)
            {
                // Loop through the items to see if the key
                // value matches the property value.
                for (int i = 0; i < Count; ++i)
                {
                    item = (FullOptionData)this[i];
                    if (propInfo.GetValue(item, null).Equals(key))
                        return i;
                }
            }
            return -1;
        }

        public bool SupportsSorting
        {
            get
            {
                return true;
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

        bool isSorted;

        public bool IsSorted
        {
            get
            {
                return isSorted;
            }
        }

        public bool AllowRemove
        {
            get
            {
                return false;
            }
        }

        public bool SupportsSearching
        {
            get
            {
                return false;
            }
        }

        ListSortDirection sortDirection;
        public ListSortDirection SortDirection
        {
            get
            {
                return sortDirection;
            }
        }

        public bool SupportsChangeNotification
        {
            get
            {
                return true;
            }
        }

        public object AddNew()
        {
            FullOptionData c = new FullOptionData(); //this.Count.ToString());
            List.Add(c);
            return c;
        }

        public bool AllowEdit
        {
            get
            {
                return true;
            }
        }

        public void RemoveIndex(PropertyDescriptor property)
        {
            // TODO:  Add FullOptionDataCollection.RemoveIndex implementation
        }

        #endregion

        #region IList Members

        public FullOptionData this[int index]
        {
            get
            {
                return (FullOptionData)List[index];
            }
            set
            {
                List[index] = (FullOptionData)value;
            }
        }

        public new void RemoveAt(int index)
        {
            List.RemoveAt(index);
        }

        public void Insert(int index, FullOptionData fullOptionData)
        {
            List.Insert(index, fullOptionData);
        }

        public void Remove(FullOptionData fullOptionData)
        {
            List.Remove(fullOptionData);
        }

        //		public bool Contains(FullOptionData fullOptionData)
        //		{
        //			return List.Contains(fullOptionData);
        //		}

        //		public void Clear()
        //		{
        //			if(List.Count >0)
        //				List.Clear();
        //		}

        //		public int IndexOf(FullOptionData fullOptionData)
        //		{
        //			return List.IndexOf(fullOptionData);
        //		}

        public int Add(FullOptionData fullOptionData)
        {
            return List.Add(fullOptionData);
        }

        public void CustomListChanged()
        {
            if (this != null && this.Count > 0)
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0)); // .ItemAdded | ListChangedType.ItemChanged , 0));
            }

        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is FullOptionData))
            {
                throw new ArgumentException("Collection only supports FullOptionData objects.");
            }
        }


        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (ListChangedEvent != null)
                ListChangedEvent(this, ev);
        }

        /// <summary>
        /// Was fully commmented
        /// </summary>
        protected override void OnClear()
        {
            //foreach (FullOptionData tempLoopVar_c in List)
            //{
            //    tempLoopVar_c.Parents = null;
            //    OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, index));
            //}
        }

        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            //FullOptionData c = ((FullOptionData)value);
            //c.Parents = this;

            base.OnInsertComplete(index, value);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
            //FullOptionData c = ((FullOptionData)value);
            //c.Parents = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {
                //FullOptionData oldcust = ((FullOptionData)oldValue);
                //FullOptionData newcust = ((FullOptionData)newValue);

                //oldcust.Parents = null;
                //newcust.Parents = this;

                base.OnSetComplete(index, oldValue, newValue);
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        //public int Count
        //{
        //    get
        //    {
        //        return List.Count;
        //    }
        //}

        public void CopyTo(Array array, int index)
        {
            List.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return List.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        public new IEnumerator GetEnumerator()
        {
            return (new FullOptionDataEnumerator(this));
        }

        #endregion

        #region FullOptionDataEnumerator Class

        public class FullOptionDataEnumerator : IEnumerator
        {
            FullOptionDataCollection fullOptionDataCollection;
            int _location;

            public FullOptionDataEnumerator(FullOptionDataCollection recdFullOptionDataCollection)
            {
                fullOptionDataCollection = recdFullOptionDataCollection;
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
                    if ((_location < 0) || (_location >= fullOptionDataCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return fullOptionDataCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= fullOptionDataCollection.Count)
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

        #region Sorting Capabilities

        /// <summary>
        /// Gets a value indicating whether the list supports sorting.
        /// </summary>
        /// <value></value>
        /// <returns>true if the list supports sorting; otherwise, false. The default is false.</returns>
        protected bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        /// Sorts the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns></returns>
        public int Sort(string property, ListSortDirection sortDirection)
        {
            // Check the properties for a property with the specified name.
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(FullOptionData));
            PropertyDescriptor prop = properties.Find(property, true);

            // If there is not a match, return -1 otherwise pass search to
            // ApplySort method.
            if (prop == null)
                return -1;
            else
                ApplySort(prop, sortDirection);
            return 1;
        }

        /// <summary>
        /// Removes any sort applied with 
        /// <see cref="M:System.ComponentModel.BindingList`1.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"></see> 
        /// if sorting is implemented in a derived class; otherwise, raises <see cref="T:System.NotSupportedException"></see>.
        /// </summary>
        protected void RemoveSortCore()
        {
            int position;
            object temp;
            // Ensure the list has been sorted.
            if (unsortedItems != null)
            {
                // Loop through the unsorted items and reorder the
                // list per the unsorted list.
                for (int i = 0; i < unsortedItems.Count;)
                {
                    // Check the properties for a property with the specified name.
                    PropertyDescriptorCollection properties =
                        TypeDescriptor.GetProperties(typeof(FullOptionData));
                    PropertyDescriptor property = properties.Find("SortingKey", true);

                    position = this.Find(property, unsortedItems[i].GetType().
                        GetProperty("SortingKey").GetValue(unsortedItems[i], null));
                    if (position > 0 && position != i)
                    {
                        temp = this[i];
                        this[i] = this[position];
                        this[position] = (FullOptionData)temp;
                        i++;
                    }
                    else if (position == i)
                        i++;
                    else
                        // If an item in the unsorted list no longer exists,
                        // delete it.
                        unsortedItems.RemoveAt(i);
                }
                isSorted = false;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        /// <summary>
        /// Removes any sort applied using 
        /// <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"></see> is false. </exception>
        public void RemoveSort()
        {
            RemoveSortCore();
        }

        /// <summary>
        /// Commits a pending new item to the collection.
        /// </summary>
        /// <param name="itemIndex">The index of the new item to be added.</param>
        public void EndNew()
        {
            // Check to see if the item is added to the end of the list,
            // and if so, re-sort the list.
            //if (sortProperty!= null && itemIndex == this.Count - 1)
            //    ApplySort(this.sortProperty, this.sortDirection);

            //base.EndNew(itemIndex);
        }

        #endregion

    }
}

#endregion
