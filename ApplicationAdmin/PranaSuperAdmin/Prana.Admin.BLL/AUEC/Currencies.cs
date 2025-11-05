using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Currencies is a collection class for <see cref="Currency"/> class.
    /// </summary>
    public class Currencies : IList
    {
        ArrayList _currencies = new ArrayList();
        Hashtable _IDMap = new Hashtable();

        public Currencies()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _currencies.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Currencies.this getter implementation
                return _currencies[index];
            }
            set
            {
                //Add Currencies.this setter implementation
                _currencies[index] = value;
                if (!_IDMap.ContainsKey(((Currency)value).CurencyID))
                {
                    _IDMap.Add(((Currency)value).CurencyID, value);
                }
            }
        }

        public void RemoveAt(int index)
        {
            //Add Currencies.RemoveAt implementation
            _currencies.RemoveAt(index);
        }

        public void Insert(int index, Object currency)
        {
            //Add Currencies.Insert implementation
            _currencies.Insert(index, (Currency)currency);
        }

        public void Remove(Object currency)
        {
            //Add Currencies.Remove implementation
            _currencies.Remove((Currency)currency);
        }

        public bool Contains(object currency)
        {
            //Add Currencies.Contains implementation
            //return _currencies.Contains((Currency)currency);
            return _IDMap.ContainsKey(((Currency)currency).CurencyID);
        }

        public void Clear()
        {
            //Add Currencies.Clear implementation
            _currencies.Clear();
        }

        public int IndexOf(object currency)
        {
            //Add Currencies.IndexOf implementation
            return _currencies.IndexOf((Currency)currency);
        }

        public int Add(object currency)
        {
            //Add Currencies.Add implementation
            if (!_IDMap.ContainsKey(((Currency)currency).CurencyID))
            {
                _IDMap.Add(((Currency)currency).CurencyID, currency);
            }
            return _currencies.Add((Currency)currency);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Currencies.IsFixedSize getter implementation
                return _currencies.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Currencies.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _currencies.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _currencies.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _currencies.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CurrencyEnumerator(this));
        }

        #endregion

        #region CurrencyEnumerator Class

        public class CurrencyEnumerator : IEnumerator
        {
            Currencies _currencies;
            int _location;

            public CurrencyEnumerator(Currencies currencies)
            {
                _currencies = currencies;
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
                    if ((_location < 0) || (_location >= _currencies.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _currencies[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _currencies.Count)
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
