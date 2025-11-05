using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CashAccounts.
    /// </summary>
    public class Accounts : IList
    {
        ArrayList _accounts = new ArrayList();
        public Accounts()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _accounts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Accounts.this getter implementation
                if (index >= _accounts.Count || index < 0)
                {
                    return new Accounts();
                }
                else
                {
                    return _accounts[index];
                }
            }
            set
            {
                //Add Accounts.this setter implementation
                _accounts[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Accounts.RemoveAt implementation
            _accounts.RemoveAt(index);
        }

        public void Insert(int index, Object account)
        {
            //Add Accounts.Insert implementation
            _accounts.Insert(index, (Account)account);
        }

        public void Remove(Object account)
        {
            //Add Accounts.Remove implementation
            _accounts.Remove((Account)account);
        }

        public bool Contains(object account)
        {
            //Add Accounts.Contains implementation
            return _accounts.Contains((Account)account);
        }

        public void Clear()
        {
            //Add Accounts.Clear implementation
            _accounts.Clear();
        }

        public int IndexOf(object account)
        {
            //Add Accounts.IndexOf implementation
            //return _Accounts.IndexOf((Account)account);

            Account tempAccount = (Account)account;
            int counter = 0;
            int result = int.MinValue;
            foreach (Account _account in _accounts)
            {
                if (_account.AccountID == tempAccount.AccountID
                    && _account.CompanyID == tempAccount.CompanyID
                    && _account.AccountName == tempAccount.AccountName
                    && _account.AccountShortName == tempAccount.AccountShortName
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

        public int Add(object account)
        {
            //Add Accounts.Add implementation
            return _accounts.Add((Account)account);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Accounts.IsFixedSize getter implementation
                return _accounts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Accounts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _accounts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _accounts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _accounts.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new AccountEnumerator(this));
        }

        #endregion

        #region AccountEnumerator Class

        public class AccountEnumerator : IEnumerator
        {
            Accounts _accounts;
            int _location;

            public AccountEnumerator(Accounts accounts)
            {
                _accounts = accounts;
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
                    if ((_location < 0) || (_location >= _accounts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _accounts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _accounts.Count)
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
