using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for RMCurrencyRates.
    /// </summary>
    public class RMCurrencyRates : IList
    {
        ArrayList _rMCurrencyRates = new ArrayList();

        public RMCurrencyRates()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _rMCurrencyRates.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add RMCurrencyRates.this getter implementation
                return _rMCurrencyRates[index];
            }
            set
            {
                //Add RMCurrencyRates.this setter implementation
                _rMCurrencyRates[index] = value;
            }
        }
        public void RemoveAt(int index)
        {
            //Add RMCurrencyRates.RemoveAt implementation
            _rMCurrencyRates.RemoveAt(index);
        }

        public void Insert(int index, Object rMCurrencyRates)
        {
            //Add RMCurrencyRates.Insert implementation
            _rMCurrencyRates.Insert(index, (RMCurrencyRates)rMCurrencyRates);
        }

        public void Remove(Object rMCurrencyRates)
        {
            //Add RMCurrencyRates.Remove implementation
            _rMCurrencyRates.Remove((RMCurrencyRates)rMCurrencyRates);
        }

        public bool Contains(object rMCurrencyRates)
        {
            //Add RMCurrencyRates.Contains implementation
            return _rMCurrencyRates.Contains((RMCurrencyRates)rMCurrencyRates);
        }

        public void Clear()
        {
            //Add RMCurrencyRates.Clear implementation
            _rMCurrencyRates.Clear();
        }

        public int IndexOf(object rMCurrencyRates)
        {
            //Add RMCurrencyRates.IndexOf implementation
            return _rMCurrencyRates.IndexOf((RMCurrencyRates)rMCurrencyRates);
        }

        public int Add(object rMCurrencyRate)
        {
            //Add RMCurrencyRates.Add implementation
            return _rMCurrencyRates.Add((RMCurrencyRate)rMCurrencyRate);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add RMCurrencyRates.IsFixedSize getter implementation
                return _rMCurrencyRates.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add RMCurrencyRates.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _rMCurrencyRates.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _rMCurrencyRates.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _rMCurrencyRates.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new RMCurrencyRatesEnumerator(this));
        }

        #endregion

        #region RMCurrencyRatesEnumerator Class

        public class RMCurrencyRatesEnumerator : IEnumerator
        {
            RMCurrencyRates _rMCurrencyRates;
            int _location;

            public RMCurrencyRatesEnumerator(RMCurrencyRates rMCurrencyRates)
            {
                _rMCurrencyRates = rMCurrencyRates;
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
                    if ((_location < 0) || (_location >= _rMCurrencyRates.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _rMCurrencyRates[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _rMCurrencyRates.Count)
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

        #region IList Members

        void System.Collections.IList.Insert(int index, object value)
        {
            // TODO:  Add RMCurrencyRates.System.Collections.IList.Insert implementation
        }

        void System.Collections.IList.Remove(object value)
        {
            // TODO:  Add RMCurrencyRates.System.Collections.IList.Remove implementation
        }

        #endregion
    }
}
