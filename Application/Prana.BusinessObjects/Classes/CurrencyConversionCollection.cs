using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for CurrencyConversionCollection.
    /// </summary>
    [Serializable]
    public class CurrencyConversionCollection : IList
    {
        ArrayList _currencyConversionCollection = new ArrayList();

        public CurrencyConversionCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _currencyConversionCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CurrencyConversionCollection.this getter implementation
                return _currencyConversionCollection[index];
            }
            set
            {
                //Add CurrencyConversionCollection.this setter implementation
                _currencyConversionCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CurrencyConversionCollection.RemoveAt implementation
            _currencyConversionCollection.RemoveAt(index);
        }

        public void Insert(int index, Object currencyConversion)
        {
            //Add CurrencyConversionCollection.Insert implementation
            _currencyConversionCollection.Insert(index, (CurrencyConversion)currencyConversion);
        }

        public void Remove(Object currencyConversion)
        {
            //Add CurrencyConversionCollection.Remove implementation
            _currencyConversionCollection.Remove((CurrencyConversion)currencyConversion);
        }

        public bool Contains(object currencyConversion)
        {
            //Add CurrencyConversionCollection.Contains implementation
            return _currencyConversionCollection.Contains((CurrencyConversion)currencyConversion);
        }

        public void Clear()
        {
            //Add CurrencyConversionCollection.Clear implementation
            _currencyConversionCollection.Clear();
        }

        public int IndexOf(object currencyConversion)
        {
            //Add CurrencyConversionCollection.IndexOf implementation
            return _currencyConversionCollection.IndexOf((CurrencyConversion)currencyConversion);
        }

        public int Add(object currencyConversion)
        {
            //Add CurrencyConversionCollection.Add implementation
            return _currencyConversionCollection.Add((CurrencyConversion)currencyConversion);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CurrencyConversionCollection.IsFixedSize getter implementation
                return _currencyConversionCollection.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CurrencyConversionCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _currencyConversionCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _currencyConversionCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _currencyConversionCollection.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CurrencyConversionEnumerator(this));
        }

        #endregion

        #region CurrencyConversionEnumerator Class

        public class CurrencyConversionEnumerator : IEnumerator
        {
            CurrencyConversionCollection _currencyConversionCollection;
            int _location;

            public CurrencyConversionEnumerator(CurrencyConversionCollection currencyConversionCollection)
            {
                _currencyConversionCollection = currencyConversionCollection;
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
                    if ((_location < 0) || (_location >= _currencyConversionCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _currencyConversionCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _currencyConversionCollection.Count)
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
