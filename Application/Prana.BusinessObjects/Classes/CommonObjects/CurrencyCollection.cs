using System;
using System.Collections;
using System.Linq;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for CurrencyCollection.
    /// </summary>
    [Serializable]
    public class CurrencyCollection : IList
    {
        ArrayList _currencyCollection = new ArrayList();

        public CurrencyCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _currencyCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CurrencyCollection.this getter implementation
                return _currencyCollection[index];
            }
            set
            {
                //Add CurrencyCollection.this setter implementation
                _currencyCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CurrencyCollection.RemoveAt implementation
            _currencyCollection.RemoveAt(index);
        }

        public void Insert(int index, Object currency)
        {
            //Add CurrencyCollection.Insert implementation
            _currencyCollection.Insert(index, (Currency)currency);
        }

        public void Remove(Object currency)
        {
            //Add CurrencyCollection.Remove implementation
            _currencyCollection.Remove((Currency)currency);
        }

        public bool Contains(object currency)
        {
            //Add CurrencyCollection.Contains implementation
            return _currencyCollection.Contains((Currency)currency);
        }

        public bool Contains(int currencyID)
        {
            return _currencyCollection.Cast<Currency>().Any(currency => currency.CurrencyID == currencyID);
        }

        public void Clear()
        {
            //Add CurrencyCollection.Clear implementation
            _currencyCollection.Clear();
        }

        public int IndexOf(object currency)
        {
            //Add CurrencyCollection.IndexOf implementation
            return _currencyCollection.IndexOf((Currency)currency);
        }

        public int Add(object currency)
        {
            //Add CurrencyCollection.Add implementation
            return _currencyCollection.Add((Currency)currency);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CurrencyCollection.IsFixedSize getter implementation
                return _currencyCollection.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CurrencyCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _currencyCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _currencyCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _currencyCollection.SyncRoot;
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
            CurrencyCollection _currencyCollection;
            int _location;

            public CurrencyEnumerator(CurrencyCollection currencyCollection)
            {
                _currencyCollection = currencyCollection;
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
                    if ((_location < 0) || (_location >= _currencyCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _currencyCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _currencyCollection.Count)
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
