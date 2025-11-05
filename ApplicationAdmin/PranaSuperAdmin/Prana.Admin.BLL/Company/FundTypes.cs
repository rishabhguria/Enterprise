using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AccountTypes.
    /// </summary>
    public class AccountTypes : IList
    {
        ArrayList _accountTypes = new ArrayList();
        public AccountTypes()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _accountTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add AccountTypes.this getter implementation
                return _accountTypes[index];
            }
            set
            {
                //Add AccountTypes.this setter implementation
                _accountTypes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add AccountTypes.RemoveAt implementation
            _accountTypes.RemoveAt(index);
        }

        public void Insert(int index, Object accountType)
        {
            //Add AccountTypes.Insert implementation
            _accountTypes.Insert(index, (AccountType)accountType);
        }

        public void Remove(Object accountType)
        {
            //Add AccountTypes.Remove implementation
            _accountTypes.Remove((AccountType)accountType);
        }

        public bool Contains(object accountType)
        {
            //Add AccountTypes.Contains implementation
            return _accountTypes.Contains((AccountType)accountType);
        }

        public void Clear()
        {
            //Add AccountTypes.Clear implementation
            _accountTypes.Clear();
        }

        public int IndexOf(object accountType)
        {
            //Add AccountTypes.IndexOf implementation
            return _accountTypes.IndexOf((AccountType)accountType);
        }

        public int Add(object accountType)
        {
            //Add AccountTypes.Add implementation
            return _accountTypes.Add((AccountType)accountType);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add AccountTypes.IsFixedSize getter implementation
                return _accountTypes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add AccountTypes.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _accountTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _accountTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _accountTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new AccountTypeEnumerator(this));
        }

        #endregion

        #region AccountTypeEnumerator Class

        public class AccountTypeEnumerator : IEnumerator
        {
            AccountTypes _accountTypes;
            int _location;

            public AccountTypeEnumerator(AccountTypes accountTypes)
            {
                _accountTypes = accountTypes;
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
                    if ((_location < 0) || (_location >= _accountTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _accountTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _accountTypes.Count)
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
