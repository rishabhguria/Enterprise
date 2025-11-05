using System;
using System.Collections;
using System.Linq;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for AccountCollection.
    /// </summary>
    [Serializable]
    public class AccountCollection : IList
    {
        ArrayList _accountCollection = new ArrayList();

        public AccountCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _accountCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add AccountCollection.this getter implementation
                return _accountCollection[index];
            }
            set
            {
                //Add AccountCollection.this setter implementation
                _accountCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add AccountCollection.RemoveAt implementation
            _accountCollection.RemoveAt(index);
        }

        public void Insert(int index, Object account)
        {
            //Add AccountCollection.Insert implementation
            _accountCollection.Insert(index, (Account)account);
        }

        public void Remove(Object account)
        {
            //Add AccountCollection.Remove implementation
            _accountCollection.Remove((Account)account);
        }

        public bool Contains(object account)
        {

            //Add AccountCollection.Contains implementation
            //return _accountCollection.Contains((Account)account);
            bool inList = false;
            for (int i = 0; i < Count; i++)
            {
                int accountID = int.MinValue;
                if ((int.TryParse(account.ToString(), out accountID) && ((Account)_accountCollection[i]).AccountID == accountID) ||
                (((Account)_accountCollection[i]).FullName.Equals(account.ToString(), StringComparison.InvariantCultureIgnoreCase)) ||
               (((Account)_accountCollection[i]).Name.Equals(account.ToString(), StringComparison.InvariantCultureIgnoreCase))
                   )

                {
                    inList = true;
                    break;
                }
            }
            return inList;
        }

        public bool Contains(int accountID)
        {
            return _accountCollection.Cast<Account>().Any(account => account.AccountID == accountID);
        }

        public void Clear()
        {
            //Add AccountCollection.Clear implementation
            _accountCollection.Clear();
        }

        public int IndexOf(object account)
        {
            //Add AccountCollection.IndexOf implementation
            return _accountCollection.IndexOf((Account)account);
        }
        public int IndexOf(int accountID)
        {

            for (int i = 0; i < _accountCollection.Count; i++)
            {
                if (accountID == ((Account)_accountCollection[i]).AccountID)
                {
                    return i;
                }
            }
            return int.MinValue;
        }
        public int Add(object account)
        {
            //Add AccountCollection.Add implementation
            return _accountCollection.Add((Account)account);
        }

        public void Sort()
        {
            //Sort AccountCollection based on name
            if(_accountCollection.Count > 1)  
                _accountCollection.Sort(1, _accountCollection.Count - 1, (IComparer)new accountCollectionSorter());
        }

        public bool IsFixedSize
        {
            get
            {
                //Add AccountCollection.IsFixedSize getter implementation
                return _accountCollection.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add AccountCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _accountCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _accountCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _accountCollection.SyncRoot;
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
            AccountCollection _accountCollection;
            int _location;

            public AccountEnumerator(AccountCollection accountCollection)
            {
                _accountCollection = accountCollection;
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
                    if ((_location < 0) || (_location >= _accountCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _accountCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _accountCollection.Count)
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

        private class accountCollectionSorter : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                Account cp1 = (Account)a;
                Account cp2 = (Account)b;
                int comparison = String.Compare(cp1.Name, cp2.Name, comparisonType: StringComparison.OrdinalIgnoreCase);
                if (comparison > 1)
                    return 1;
                if (comparison < 1)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
