using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for TradingAccounts.
    /// </summary>
    public class TradingAccounts : IList
    {
        ArrayList _tradingAccounts = new ArrayList();
        public TradingAccounts()
        {
        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _tradingAccounts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add TradingAccounts.this getter implementation
                if (index >= _tradingAccounts.Count || index < 0)
                {
                    return new TradingAccounts();
                }
                else
                {
                    return _tradingAccounts[index];
                }

            }
            set
            {
                //Add TradingAccounts.this setter implementation
                _tradingAccounts[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add TradingAccounts.RemoveAt implementation
            _tradingAccounts.RemoveAt(index);
        }

        public void Insert(int index, Object tradingAccounts)
        {
            //Add TradingAccounts.Insert implementation
            _tradingAccounts.Insert(index, (TradingAccount)tradingAccounts);
        }

        public void Remove(Object tradingAccounts)
        {
            //Add TradingAccounts.Remove implementation
            _tradingAccounts.Remove((TradingAccount)tradingAccounts);
        }

        public bool Contains(object tradingAccounts)
        {
            //Add TradingAccounts.Contains implementation
            return _tradingAccounts.Contains((TradingAccount)tradingAccounts);
        }

        public void Clear()
        {
            //Add TradingAccounts.Clear implementation
            _tradingAccounts.Clear();
        }

        public int IndexOf(object tradingAccounts)
        {
            //Add TradingAccounts.IndexOf implementation
            //return _tradingAccounts.IndexOf((TradingAccount)tradingAccount);

            TradingAccount tempTradingAccount = (TradingAccount)tradingAccounts;
            int counter = 0;
            int result = int.MinValue;
            foreach (TradingAccount _tradingAccount in _tradingAccounts)
            {
                if (_tradingAccount.TradingAccountsID == tempTradingAccount.TradingAccountsID
                    && _tradingAccount.CompanyID == tempTradingAccount.CompanyID
                    && _tradingAccount.TradingAccountName == tempTradingAccount.TradingAccountName
                    && _tradingAccount.TradingShortName == tempTradingAccount.TradingShortName
                    )
                {
                    result = counter;
                    break;
                }
                else
                {
                    counter++;
                }
            }
            return result;
        }

        public int Add(object tradingAccounts)
        {
            //Add TradingAccounts.Add implementation
            return _tradingAccounts.Add((TradingAccount)tradingAccounts);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add TradingAccounts.IsFixedSize getter implementation
                return _tradingAccounts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add TradingAccounts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _tradingAccounts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _tradingAccounts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _tradingAccounts.SyncRoot;
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

        #region TradingAccountEnumerator Class

        public class TradingAccountEnumerator : IEnumerator
        {
            TradingAccounts _tradingAccounts;
            int _location;

            public TradingAccountEnumerator(TradingAccounts clients)
            {
                _tradingAccounts = clients;
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
                    if ((_location < 0) || (_location >= _tradingAccounts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _tradingAccounts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _tradingAccounts.Count)
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
