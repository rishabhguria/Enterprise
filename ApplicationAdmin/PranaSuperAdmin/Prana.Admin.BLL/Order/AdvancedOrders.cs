using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AdvancedOrders.
    /// </summary>
    public class AdvancedOrders : IList
    {
        ArrayList _advancedOrders = new ArrayList();

        public AdvancedOrders()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _advancedOrders.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add AdvancedOrders.this getter implementation
                return _advancedOrders[index];
            }
            set
            {
                //Add AdvancedOrders.this setter implementation
                _advancedOrders[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add AdvancedOrders.RemoveAt implementation
            _advancedOrders.RemoveAt(index);
        }

        public void Insert(int index, Object advancedOrder)
        {
            //Add AdvancedOrders.Insert implementation
            _advancedOrders.Insert(index, (AdvancedOrder)advancedOrder);
        }

        public void Remove(Object advancedOrder)
        {
            //Add AdvancedOrders.Remove implementation
            _advancedOrders.Remove((AdvancedOrder)advancedOrder);
        }

        public bool Contains(object advancedOrder)
        {
            //Add AdvancedOrders.Contains implementation
            return _advancedOrders.Contains((AdvancedOrder)advancedOrder);
        }

        public void Clear()
        {
            //Add AdvancedOrders.Clear implementation
            _advancedOrders.Clear();
        }

        public int IndexOf(object advancedOrder)
        {
            //Add AdvancedOrders.IndexOf implementation
            return _advancedOrders.IndexOf((AdvancedOrder)advancedOrder);
        }

        public int Add(object advancedOrder)
        {
            //Add AdvancedOrders.Add implementation
            return _advancedOrders.Add((AdvancedOrder)advancedOrder);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add AdvancedOrders.IsFixedSize getter implementation
                return _advancedOrders.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add AdvancedOrders.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _advancedOrders.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _advancedOrders.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _advancedOrders.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new AdvancedOrderEnumerator(this));
        }

        #endregion

        #region AssetEnumerator Class

        public class AdvancedOrderEnumerator : IEnumerator
        {
            AdvancedOrders _advancedOrders;
            int _location;

            public AdvancedOrderEnumerator(AdvancedOrders advancedOrders)
            {
                _advancedOrders = advancedOrders;
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
                    if ((_location < 0) || (_location >= _advancedOrders.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _advancedOrders[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _advancedOrders.Count)
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
