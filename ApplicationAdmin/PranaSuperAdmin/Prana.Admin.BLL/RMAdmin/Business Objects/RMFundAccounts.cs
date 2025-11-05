using System;
using System.Collections;


namespace Prana.Admin.BLL
{
    public class RMFundAccounts : IList
    {
        ArrayList _rMFundAccounts = new ArrayList();

        public RMFundAccounts()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _rMFundAccounts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add RMFundAccounts.this getter implementation
                return _rMFundAccounts[index];
            }
            set
            {
                //Add RMFundAccounts.this setter implementation
                _rMFundAccounts[index] = value;
            }
        }
        public void RemoveAt(int index)
        {
            //Add RMFundAccounts.RemoveAt implementation
            _rMFundAccounts.RemoveAt(index);
        }

        public void Insert(int index, Object rMFundAccounts)
        {
            //Add RMFundAccounts.Insert implementation
            _rMFundAccounts.Insert(index, (RMFundAccounts)rMFundAccounts);
        }

        public void Remove(Object rMFundAccounts)
        {
            //Add RMFundAccounts.Remove implementation
            _rMFundAccounts.Remove((RMFundAccounts)rMFundAccounts);
        }

        public bool Contains(object rMFundAccounts)
        {
            //Add RMFundAccounts.Contains implementation
            return _rMFundAccounts.Contains((RMFundAccounts)rMFundAccounts);
        }

        public void Clear()
        {
            //Add RMFundAccounts.Clear implementation
            _rMFundAccounts.Clear();
        }

        public int IndexOf(object rMFundAccounts)
        {
            //Add RMFundAccounts.IndexOf implementation
            return _rMFundAccounts.IndexOf((RMFundAccounts)rMFundAccounts);
        }

        public int Add(object rMFundAccount)
        {
            //Add RMFundAccounts.Add implementation
            return _rMFundAccounts.Add((RMFundAccount)rMFundAccount);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add RMFundAccounts.IsFixedSize getter implementation
                return _rMFundAccounts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add RMFundAccounts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _rMFundAccounts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _rMFundAccounts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _rMFundAccounts.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new RMFundAccountsEnumerator(this));
        }

        #endregion

        #region RMFundAccountsEnumerator Class

        public class RMFundAccountsEnumerator : IEnumerator
        {
            RMFundAccounts _rMFundAccounts;
            int _location;

            public RMFundAccountsEnumerator(RMFundAccounts rMFundAccounts)
            {
                _rMFundAccounts = rMFundAccounts;
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
                    if ((_location < 0) || (_location >= _rMFundAccounts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _rMFundAccounts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _rMFundAccounts.Count)
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
            // TODO:  Add RMFundAccounts.System.Collections.IList.Insert implementation
        }

        void System.Collections.IList.Remove(object value)
        {
            // TODO:  Add RMFundAccounts.System.Collections.IList.Remove implementation
        }

        #endregion

    }
}
