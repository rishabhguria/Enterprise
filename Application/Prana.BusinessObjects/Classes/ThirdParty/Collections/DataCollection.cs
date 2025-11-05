using System;
using System.Collections;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{

    /// <summary>
    /// Generic Data Collection
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class DataCollection<T> : IList, ICollection, IEnumerable where T : class, new()
    {
        /// <summary>
        /// Data Store
        /// </summary>
        readonly ArrayList _data = new ArrayList();

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</returns>
        /// <remarks></remarks>
        public int Count { get { return _data.Count; } set { } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.</returns>
        /// <remarks></remarks>
        public bool IsFixedSize { get { return _data.IsFixedSize; } set { } }
        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.IList"/> is read-only; otherwise, false.</returns>
        /// <remarks></remarks>
        public bool IsReadOnly { get { return _data.IsReadOnly; } set { } }
        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
        /// <remarks></remarks>
        public bool IsSynchronized { get { return _data.IsSynchronized; } set { } }
        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
        /// <remarks></remarks>
        public object SyncRoot { get { return _data.SyncRoot; } set { } }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>The element at the specified index.</returns>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList"/> is read-only. </exception>
        /// <remarks></remarks>
        public object this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        /// <summary>
        /// Adds the specified third party type.
        /// </summary>
        /// <param name="item">Type of the third party.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Add(object item)
        {
            return _data.Add(item);
        }

        public void AddRange(List<T> items)
        {
            _data.AddRange(items);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only. </exception>
        /// <remarks></remarks>
        public void Clear()
        {
            _data.Clear();
        }
        /// <summary>
        /// Determines whether [contains] [the specified third party type].
        /// </summary>
        /// <param name="item">Type of the third party.</param>
        /// <returns><c>true</c> if [contains] [the specified third party type]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public bool Contains(object item)
        {
            return _data.Contains(item);
        }
        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array"/> is null. </exception>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is less than zero. </exception>
        ///   
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
        ///   
        /// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
        /// <remarks></remarks>
        public void CopyTo(Array array, int index)
        {
            _data.CopyTo(array, index);
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        /// <remarks></remarks>
        public IEnumerator GetEnumerator()
        {
            return _data.GetEnumerator();
        }
        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">Type of the third party.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int IndexOf(object item)
        {
            return _data.IndexOf(item);
        }
        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">Type of the third party.</param>
        /// <remarks></remarks>
        public void Insert(int index, object item)
        {
            _data.Insert(index, item);
        }
        /// <summary>
        /// Removes the specified third party type.
        /// </summary>
        /// <param name="item">Type of the third party.</param>
        /// <remarks></remarks>
        public void Remove(object item)
        {
            _data.Remove(item);
        }
        /// <summary>
        /// Removes the <see cref="T:System.Collections.IList"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        /// <remarks></remarks>
        public void RemoveAt(int index)
        {
            _data.RemoveAt(index);
        }


        /// <summary>
        /// Row Enumerator
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Rows<U> : IEnumerator where U : IList, new()
        {
            /// <summary>
            /// Data Store
            /// </summary>
            //readonly T _items;
            readonly U _items;
            /// <summary>
            /// 
            /// </summary>
            int _index;
            /// <summary>
            /// Initializes a new instance of the <see cref="ThirdPartyBatchEnumerator"/> class.
            /// </summary>
            /// <param name="symbols">The symbols.</param>
            /// <remarks></remarks>
            public Rows(U items)
            {
                _items = items;
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <returns>The current element in the collection.</returns>
            ///   
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
            /// <remarks></remarks>
            public object Current
            {
                get
                {
                    if ((_index < 0) || (_index >= _items.Count))
                    {
                        return null;
                        //throw (new IndexOutOfRangeException());
                    }
                    else
                    {
                        return (T)_items[_index];
                    }
                }
            }
            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            /// <remarks></remarks>
            public bool MoveNext()
            {
                _index++;
                if (_index >= _items.Count)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            /// <remarks></remarks>
            public void Reset()
            {
                _index = -1;
            }
        }
    }
}
