using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    public class UserTradingAccounts : IList
    {
        ArrayList _userTradingAccounts = new ArrayList();

        public UserTradingAccounts()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _userTradingAccounts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add UserTradingAccounts.this getter implementation
                return _userTradingAccounts[index];
            }
            set
            {
                //Add UserTradingAccounts.this setter implementation
                _userTradingAccounts[index] = value;
            }
        }
        public void RemoveAt(int index)
        {
            //Add UserTradingAccounts.RemoveAt implementation
            _userTradingAccounts.RemoveAt(index);
        }

        public void Insert(int index, Object userTradingAccounts)
        {
            //Add UserTradingAccounts.Insert implementation
            _userTradingAccounts.Insert(index, (UserTradingAccounts)userTradingAccounts);
        }

        public void Remove(Object userTradingAccounts)
        {
            //Add UserTradingAccounts.Remove implementation
            _userTradingAccounts.Remove((UserTradingAccounts)userTradingAccounts);
        }

        public bool Contains(object userTradingAccounts)
        {
            //Add UserTradingAccounts.Contains implementation
            return _userTradingAccounts.Contains((UserTradingAccounts)userTradingAccounts);
        }

        public void Clear()
        {
            //Add UserTradingAccounts.Clear implementation
            _userTradingAccounts.Clear();
        }

        public int IndexOf(object userTradingAccounts)
        {
            //Add UserTradingAccounts.IndexOf implementation
            return _userTradingAccounts.IndexOf((UserTradingAccounts)userTradingAccounts);
        }

        public int Add(object userTradingAccounts)
        {
            //Add UserTradingAccounts.Add implementation
            return _userTradingAccounts.Add((UserTradingAccounts)userTradingAccounts);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add UserTradingAccounts.IsFixedSize getter implementation
                return _userTradingAccounts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add UserTradingAccounts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _userTradingAccounts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _userTradingAccounts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _userTradingAccounts.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new UserTradingAccountsEnumerator(this));
        }

        #endregion

        #region UserTradingAccountsEnumerator Class

        public class UserTradingAccountsEnumerator : IEnumerator
        {
            UserTradingAccounts _userTradingAccounts;
            int _location;

            public UserTradingAccountsEnumerator(UserTradingAccounts userTradingAccounts)
            {
                _userTradingAccounts = userTradingAccounts;
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
                    if ((_location < 0) || (_location >= _userTradingAccounts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _userTradingAccounts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _userTradingAccounts.Count)
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
            // TODO:  Add UserTradingAccounts.System.Collections.IList.Insert implementation
        }

        void System.Collections.IList.Remove(object value)
        {
            // TODO:  Add UserTradingAccounts.System.Collections.IList.Remove implementation
        }

        #endregion
    }
}
