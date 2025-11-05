using System;
using System.Collections;
using System.Linq;


namespace Prana.BusinessObjects
{
    [Serializable]
    public class TradingAccountCollection : IList
    {
        ArrayList _tradingAccountCollection = new ArrayList();

        public TradingAccountCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _tradingAccountCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add TradingAccountCollection.this getter implementation
                return _tradingAccountCollection[index];
            }
            set
            {
                //Add TradingAccountCollection.this setter implementation
                _tradingAccountCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add TradingAccountCollection.RemoveAt implementation
            _tradingAccountCollection.RemoveAt(index);
        }

        public void Insert(int index, Object tradingAccount)
        {
            //Add TradingAccountCollection.Insert implementation
            _tradingAccountCollection.Insert(index, (TradingAccount)tradingAccount);
        }

        public void Remove(Object tradingAccount)
        {
            //Add TradingAccountCollection.Remove implementation
            _tradingAccountCollection.Remove((TradingAccount)tradingAccount);
        }

        public bool Contains(object tradingAccount)
        {
            //Add TradingAccountCollection.Contains implementation
            return _tradingAccountCollection.Contains((TradingAccount)tradingAccount);
        }

        public bool Contains(int tradingAccountID)
        {
            return _tradingAccountCollection.Cast<TradingAccount>().Any(tradingAccount => tradingAccount.TradingAccountID == tradingAccountID);
        }

        public void Clear()
        {
            //Add TradingAccountCollection.Clear implementation
            _tradingAccountCollection.Clear();
        }

        public int IndexOf(object tradingAccount)
        {
            //Add TradingAccountCollection.IndexOf implementation
            return _tradingAccountCollection.IndexOf((TradingAccount)tradingAccount);
        }

        public int Add(object tradingAccount)
        {
            //Add TradingAccountCollection.Add implementation
            return _tradingAccountCollection.Add((TradingAccount)tradingAccount);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add TradingAccountCollection.IsFixedSize getter implementation
                return _tradingAccountCollection.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add TradingAccountCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _tradingAccountCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _tradingAccountCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _tradingAccountCollection.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new TradingAccountEnumerator(this));
        }

        #endregion

        #region AssetEnumerator Class

        public class TradingAccountEnumerator : IEnumerator
        {
            TradingAccountCollection _tradingAccountCollection;
            int _location;

            public TradingAccountEnumerator(TradingAccountCollection tradingAccountCollection)
            {
                _tradingAccountCollection = tradingAccountCollection;
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
                    if ((_location < 0) || (_location >= _tradingAccountCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _tradingAccountCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _tradingAccountCollection.Count)
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
