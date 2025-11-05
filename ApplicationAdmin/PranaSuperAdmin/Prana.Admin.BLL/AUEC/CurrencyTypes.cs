using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CurrencyTypes.
    /// </summary>
    public class CurrencyTypes : IList
    {
        ArrayList _currencyTypes = new ArrayList();
        Hashtable _IDMap = new Hashtable();

        public CurrencyTypes()
        {
        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _currencyTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CurrencyTypes.this getter implementation
                return _currencyTypes[index];
            }
            set
            {
                //Add CurrencyTypes.this setter implementation
                _currencyTypes[index] = value;
                if (!_IDMap.ContainsKey(((CurrencyType)value).CurrencyTypeID))
                {
                    _IDMap.Add(((CurrencyType)value).CurrencyTypeID, value);
                }
            }
        }

        public void RemoveAt(int index)
        {
            //Add CurrencyTypes.RemoveAt implementation
            _currencyTypes.RemoveAt(index);
        }

        public void Insert(int index, Object currencyType)
        {
            //Add CurrencyTypes.Insert implementation
            _currencyTypes.Insert(index, (CurrencyType)currencyType);
        }

        public void Remove(Object currencyType)
        {
            //Add CurrencyTypes.Remove implementation
            _currencyTypes.Remove((CurrencyType)currencyType);
        }

        public bool Contains(object currencyType)
        {
            //Add CurrencyTypes.Contains implementation
            //return _currencyTypes.Contains((CurrencyType)currencyType);
            return _IDMap.ContainsKey(((CurrencyType)currencyType).CurrencyTypeID);
        }

        public void Clear()
        {
            //Add CurrencyTypes.Clear implementation
            _currencyTypes.Clear();
        }

        public int IndexOf(object currencyType)
        {
            //Add CurrencyTypes.IndexOf implementation
            return _currencyTypes.IndexOf((CurrencyType)currencyType);
        }

        public int Add(object currencyType)
        {
            //Add CurrencyTypes.Add implementation
            if (!_IDMap.ContainsKey(((CurrencyType)currencyType).CurrencyTypeID))
            {
                _IDMap.Add(((CurrencyType)currencyType).CurrencyTypeID, currencyType);
            }
            return _currencyTypes.Add((CurrencyType)currencyType);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CurrencyTypes.IsFixedSize getter implementation
                return _currencyTypes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CurrencyTypes.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _currencyTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _currencyTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _currencyTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CurrencyTypeEnumerator(this));
        }

        #endregion

        #region CurrencyTypeEnumerator Class

        public class CurrencyTypeEnumerator : IEnumerator
        {
            CurrencyTypes _currencyTypes;
            int _location;

            public CurrencyTypeEnumerator(CurrencyTypes currencyTypes)
            {
                _currencyTypes = currencyTypes;
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
                    if ((_location < 0) || (_location >= _currencyTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _currencyTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _currencyTypes.Count)
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
