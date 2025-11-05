using System;
using System.Collections;
using System.Linq;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for OrderTypes.
    /// </summary>
    public class OrderTypes : IList
    {
        ArrayList _orderTypes = new ArrayList();

        public OrderTypes()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _orderTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add OrderTypes.this getter implementation
                return _orderTypes[index];
            }
            set
            {
                //Add OrderTypes.this setter implementation
                _orderTypes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add OrderTypes.RemoveAt implementation
            _orderTypes.RemoveAt(index);
        }

        public void Insert(int index, Object orderType)
        {
            //Add OrderTypes.Insert implementation
            _orderTypes.Insert(index, (OrderType)orderType);
        }

        public void Remove(Object orderType)
        {
            //Add OrderTypes.Remove implementation
            _orderTypes.Remove((OrderType)orderType);
        }

        public bool Contains(object orderType)
        {
            //Add OrderTypes.Contains implementation
            return _orderTypes.Contains((OrderType)orderType);
        }

        public bool Contains(int orderTypesID)
        {
            return _orderTypes.Cast<OrderType>().Any(orderType => orderType.OrderTypesID == orderTypesID);
        }

        public void Clear()
        {
            //Add OrderTypes.Clear implementation
            _orderTypes.Clear();
        }

        public int IndexOf(object orderType)
        {
            //Add OrderTypes.IndexOf implementation
            return _orderTypes.IndexOf((OrderType)orderType);
        }

        public int Add(object orderType)
        {
            //Add OrderTypes.Add implementation
            return _orderTypes.Add((OrderType)orderType);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add OrderTypes.IsFixedSize getter implementation
                return _orderTypes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add OrderTypes.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _orderTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _orderTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _orderTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new OrderTypeEnumerator(this));
        }

        #endregion

        #region OrderTypeEnumerator Class

        public class OrderTypeEnumerator : IEnumerator
        {
            OrderTypes _orderTypes;
            int _location;

            public OrderTypeEnumerator(OrderTypes orderTypes)
            {
                _orderTypes = orderTypes;
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
                    if ((_location < 0) || (_location >= _orderTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _orderTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _orderTypes.Count)
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
