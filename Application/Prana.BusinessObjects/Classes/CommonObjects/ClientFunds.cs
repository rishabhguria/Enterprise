using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ClientAccounts.
    /// </summary>
    public class ClientAccounts : IList
    {
        ArrayList _clientAccounts = new ArrayList();

        public ClientAccounts()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _clientAccounts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ClientAccounts.this getter implementation
                if (index >= _clientAccounts.Count || index < 0)
                {
                    return new ClientAccounts();
                }
                else
                {
                    return _clientAccounts[index];
                }
            }
            set
            {
                //Add ClientAccounts.this setter implementation
                _clientAccounts[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ClientAccounts.RemoveAt implementation
            _clientAccounts.RemoveAt(index);
        }

        public void Insert(int index, Object clientAccount)
        {
            //Add ClientAccounts.Insert implementation
            _clientAccounts.Insert(index, (ClientAccount)clientAccount);
        }

        public void Remove(Object clientAccount)
        {
            //Add ClientAccounts.Remove implementation
            _clientAccounts.Remove((ClientAccount)clientAccount);
        }

        public bool Contains(object clientAccount)
        {
            //Add ClientAccounts.Contains implementation
            return _clientAccounts.Contains((ClientAccount)clientAccount);
        }

        public void Clear()
        {
            //Add ClientAccounts.Clear implementation
            _clientAccounts.Clear();
        }

        public int IndexOf(object clientAccount)
        {
            //Add ClientAccounts.IndexOf implementation
            //return _clientAccounts.IndexOf((ClientAccount)clientAccount);

            ClientAccount tempClientAccount = (ClientAccount)clientAccount;
            int counter = 0;
            int result = int.MinValue;
            foreach (ClientAccount _clientAccount in _clientAccounts)
            {
                if (_clientAccount.CompanyClientAccountID == tempClientAccount.CompanyClientAccountID
                    && _clientAccount.CompanyClientID == tempClientAccount.CompanyClientID
                    && _clientAccount.CompanyClientAccountName == tempClientAccount.CompanyClientAccountName
                    && _clientAccount.CompanyClientAccountShortName == tempClientAccount.CompanyClientAccountShortName
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

        public int Add(object clientAccount)
        {
            //Add ClientAccounts.Add implementation
            return _clientAccounts.Add((ClientAccount)clientAccount);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ClientAccounts.IsFixedSize getter implementation
                return _clientAccounts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ClientAccounts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _clientAccounts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _clientAccounts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _clientAccounts.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ClientAccountEnumerator(this));
        }

        #endregion

        #region ClientAccountEnumerator Class

        public class ClientAccountEnumerator : IEnumerator
        {
            ClientAccounts _clientAccounts;
            int _location;

            public ClientAccountEnumerator(ClientAccounts clientAccounts)
            {
                _clientAccounts = clientAccounts;
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
                    if ((_location < 0) || (_location >= _clientAccounts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _clientAccounts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _clientAccounts.Count)
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
