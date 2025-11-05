using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    public class RMTradingAccounts : IList
    {
        ArrayList _rMTradingAccounts = new ArrayList();

        public RMTradingAccounts()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _rMTradingAccounts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add RMTradingAccounts.this getter implementation
                return _rMTradingAccounts[index];
            }
            set
            {
                //Add RMTradingAccounts.this setter implementation
                _rMTradingAccounts[index] = value;
            }
        }
        public void RemoveAt(int index)
        {
            //Add RMTradingAccounts.RemoveAt implementation
            _rMTradingAccounts.RemoveAt(index);
        }

        public void Insert(int index, Object rMTradingAccounts)
        {
            //Add RMTradingAccounts.Insert implementation
            _rMTradingAccounts.Insert(index, (RMTradingAccounts)rMTradingAccounts);
        }

        public void Remove(Object rMTradingAccounts)
        {
            //Add RMTradingAccounts.Remove implementation
            _rMTradingAccounts.Remove((RMTradingAccounts)rMTradingAccounts);
        }

        public bool Contains(object rMTradingAccounts)
        {
            //Add RMTradingAccounts.Contains implementation
            return _rMTradingAccounts.Contains((RMTradingAccounts)rMTradingAccounts);
        }

        public void Clear()
        {
            //Add RMTradingAccounts.Clear implementation
            _rMTradingAccounts.Clear();
        }

        public int IndexOf(object rMTradingAccounts)
        {
            //Add RMTradingAccounts.IndexOf implementation
            return _rMTradingAccounts.IndexOf((RMTradingAccounts)rMTradingAccounts);
        }

        public int Add(object rMTradingAccount)
        {
            //Add RMTradingAccounts.Add implementation
            return _rMTradingAccounts.Add((RMTradingAccount)rMTradingAccount);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add RMTradingAccounts.IsFixedSize getter implementation
                return _rMTradingAccounts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add RMTradingAccounts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _rMTradingAccounts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _rMTradingAccounts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _rMTradingAccounts.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new RMTradingAccountsEnumerator(this));
        }

        #endregion

        #region RMTradingAccountsEnumerator Class

        public class RMTradingAccountsEnumerator : IEnumerator
        {
            RMTradingAccounts _rMTradingAccounts;
            int _location;

            public RMTradingAccountsEnumerator(RMTradingAccounts rMTradingAccounts)
            {
                _rMTradingAccounts = rMTradingAccounts;
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
                    if ((_location < 0) || (_location >= _rMTradingAccounts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _rMTradingAccounts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _rMTradingAccounts.Count)
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
            // TODO:  Add RMTradingAccounts.System.Collections.IList.Insert implementation
        }

        void System.Collections.IList.Remove(object value)
        {
            // TODO:  Add RMTradingAccounts.System.Collections.IList.Remove implementation
        }

        #endregion
    }
}
