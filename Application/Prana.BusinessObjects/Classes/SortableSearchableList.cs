using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class SortableSearchableList<T> : BindingList<T>
    {

        private ArrayList sortedList;
        private ArrayList unsortedItems;
        /// <summary>
        /// Gets a value indicating whether [supports searching core].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [supports searching core]; otherwise, <c>false</c>.
        /// </value>
        protected override bool SupportsSearchingCore
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Implementation to search the list.
        /// </summary>
        /// <param name="prop">The prop.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            // Get the property info for the specified property.
            PropertyInfo propInfo = typeof(T).GetProperty(prop.Name);
            T item;

            if (key != null)
            {
                // Loop through the items to see if the key
                // value matches the property value.
                for (int i = 0; i < Count; ++i)
                {
                    item = (T)Items[i];
                    if (propInfo.GetValue(item, null).Equals(key))
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Expose the search functionality with a public method.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public int Find(string property, object key)
        {
            // Check the properties for a property with the specified name.
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = properties.Find(property, true);

            // If there is not a match, return -1 otherwise pass search to
            // FindCore method.
            if (prop == null)
                return -1;
            else
                return FindCore(prop, key);
        }

        /// <summary>
        /// Gets a value indicating whether the list supports sorting.
        /// </summary>
        /// <value></value>
        /// <returns>true if the list supports sorting; otherwise, false. The default is false.</returns>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        bool isSortedValue;
        /// <summary>
        /// Gets a value indicating whether the list is sorted.
        /// </summary>
        /// <value></value>
        /// <returns>true if the list is sorted; otherwise, false. The default is false.</returns>
        protected override bool IsSortedCore
        {
            get { return isSortedValue; }
        }



        [field: NonSerializedAttribute()]
        PropertyDescriptor sortPropertyValue;
        /// <summary>
        /// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns null.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor"></see> used for sorting the list.</returns>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortPropertyValue; }
        }

        ListSortDirection sortDirectionValue;
        /// <summary>
        /// Gets the direction the list is sorted.
        /// </summary>
        /// <value></value>
        /// <returns>One of the <see cref="T:System.ComponentModel.ListSortDirection"></see> values. The default is <see cref="F:System.ComponentModel.ListSortDirection.Ascending"></see>. </returns>
        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirectionValue; }
        }

        /// <summary>
        /// Sorts the items if overridden in a derived class; otherwise, throws a <see cref="T:System.NotSupportedException"></see>.
        /// </summary>
        /// <param name="prop">A <see cref="T:System.ComponentModel.PropertyDescriptor"></see> that specifies the property to sort on.</param>
        /// <param name="direction">One of the <see cref="T:System.ComponentModel.ListSortDirection"></see>  values.</param>
        protected override void ApplySortCore(PropertyDescriptor prop,
            ListSortDirection direction)
        {
            sortedList = new ArrayList();

            // Check to see if the property type we are sorting by implements
            // the IComparable interface.
            Type interfaceType = prop.PropertyType.GetInterface("IComparable");

            if (interfaceType != null)
            {
                // If so, set the SortPropertyValue and SortDirectionValue.
                sortPropertyValue = prop;
                sortDirectionValue = direction;

                unsortedItems = new ArrayList(this.Count);

                // Loop through each item, adding it the the sortedItems ArrayList.
                foreach (Object item in this.Items)
                {
                    sortedList.Add(prop.GetValue(item));
                    unsortedItems.Add(item);
                }
                // Call Sort on the ArrayList.
                sortedList.Sort();
                T temp;

                // Check the sort direction and then copy the sorted items
                // back into the list.
                if (direction == ListSortDirection.Descending)
                    sortedList.Reverse();

                for (int i = 0; i < this.Count; i++)
                {
                    int position = Find(prop.Name, sortedList[i]);
                    if (position != i)
                    {
                        temp = this[i];
                        this[i] = this[position];
                        this[position] = temp;
                    }
                }

                isSortedValue = true;

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
                TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = properties.Find(property, true);

            // If there is not a match, return -1 otherwise pass search to
            // ApplySortCore method.
            if (prop == null)
                return -1;
            else
                ApplySortCore(prop, sortDirection);
            return 1;
        }

        /// <summary>
        /// Removes any sort applied with 
        /// <see cref="M:System.ComponentModel.BindingList`1.ApplySortCore(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"></see> 
        /// if sorting is implemented in a derived class; otherwise, raises <see cref="T:System.NotSupportedException"></see>.
        /// </summary>
        protected override void RemoveSortCore()
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
                    position = this.Find("LastName",
                        unsortedItems[i].GetType().
                        GetProperty("LastName").GetValue(unsortedItems[i], null));
                    if (position > 0 && position != i)
                    {
                        temp = this[i];
                        this[i] = this[position];
                        this[position] = (T)temp;
                        i++;
                    }
                    else if (position == i)
                        i++;
                    else
                        // If an item in the unsorted list no longer exists,
                        // delete it.
                        unsortedItems.RemoveAt(i);
                }
                isSortedValue = false;
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
        public override void EndNew(int itemIndex)
        {
            // Check to see if the item is added to the end of the list,
            // and if so, re-sort the list.
            if (sortPropertyValue != null && itemIndex == this.Count - 1)
                ApplySortCore(this.sortPropertyValue, this.sortDirectionValue);

            base.EndNew(itemIndex);
        }


    }
}
