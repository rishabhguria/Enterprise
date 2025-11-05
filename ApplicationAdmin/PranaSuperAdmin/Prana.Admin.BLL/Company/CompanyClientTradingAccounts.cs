using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyClientTradingAccounts.
    /// </summary>
    public class CompanyClientTradingAccounts : IList
    {
        #region Private members
        ArrayList _companyClientTradingAccounts = new ArrayList();
        #endregion



        public CompanyClientTradingAccounts()
        {

        }

        public CompanyClientTradingAccount GetCompanyClientTradingAccount(int CompanyClientTradingAccountID)
        {
            foreach (CompanyClientTradingAccount companyClientTradingAccount in _companyClientTradingAccounts)
            {
                if (companyClientTradingAccount.CompanyClientTradingAccountID == CompanyClientTradingAccountID)
                    return companyClientTradingAccount;

            }
            return null;
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companyClientTradingAccounts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ClientAccounts.this getter implementation
                if (index >= _companyClientTradingAccounts.Count || index < 0)
                {
                    return new CompanyClientTradingAccounts();
                }
                else
                {
                    return _companyClientTradingAccounts[index];
                }
            }
            set
            {
                //Add ClientAccounts.this setter implementation
                _companyClientTradingAccounts[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ClientAccounts.RemoveAt implementation
            _companyClientTradingAccounts.RemoveAt(index);
        }

        public void Insert(int index, Object companyClientTradingAccount)
        {
            //Add ClientAccounts.Insert implementation
            _companyClientTradingAccounts.Insert(index, (CompanyClientTradingAccount)companyClientTradingAccount);
        }

        public void Remove(Object companyClientTradingAccount)
        {
            //Add ClientAccounts.Remove implementation
            _companyClientTradingAccounts.Remove((CompanyClientTradingAccount)companyClientTradingAccount);
        }

        public bool Contains(object companyClientTradingAccount)
        {
            bool bMatch = false;
            CompanyClientTradingAccount temp1 = (CompanyClientTradingAccount)companyClientTradingAccount;

            foreach (CompanyClientTradingAccount temp in _companyClientTradingAccounts)
            {
                if (temp.CompClientTradingAccount.Equals(temp1.CompClientTradingAccount) &&
                    temp.ClientTraderShortName.Equals(temp1.ClientTraderShortName) &&
                    temp.ClientTraderID.Equals(temp1.ClientTraderID) &&
                    temp.CompanyClientID.Equals(temp1.CompanyClientID) &&
                    temp.CompanyClientTradingAccountID.Equals(temp1.CompanyClientTradingAccountID) &&
                    temp.CompanyTradingAccountID.Equals(temp1.CompanyTradingAccountID))
                {
                    bMatch = true;
                    break;
                }

            }
            return bMatch;
            //return _companyClientTradingAccounts.Contains((CompanyClientTradingAccount)companyClientTradingAccount);
        }

        public void Clear()
        {
            //Add ClientAccounts.Clear implementation
            _companyClientTradingAccounts.Clear();
        }

        public int IndexOf(object companyClientTradingAccount)
        {

            CompanyClientTradingAccount tempCompanyClientTradingAccount = (CompanyClientTradingAccount)companyClientTradingAccount;
            int counter = 0;
            int result = int.MinValue;
            foreach (CompanyClientTradingAccount _companyClientTradingAccount in _companyClientTradingAccounts)
            {
                if (_companyClientTradingAccount.Equal(tempCompanyClientTradingAccount))
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

        public int Add(object companyClientTradingAccount)
        {
            //Add ClientAccounts.Add implementation
            return _companyClientTradingAccounts.Add((CompanyClientTradingAccount)companyClientTradingAccount);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ClientAccounts.IsFixedSize getter implementation
                return _companyClientTradingAccounts.IsFixedSize;
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
                return _companyClientTradingAccounts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companyClientTradingAccounts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companyClientTradingAccounts.SyncRoot;
                //return null;
            }
        }
        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ClientTradingAccountEnumerator(this));
        }

        #endregion

        #region ClientTradingAccountEnumerator Class

        public class ClientTradingAccountEnumerator : IEnumerator
        {
            CompanyClientTradingAccounts _companyClientTradingAccounts;
            int _location;

            public ClientTradingAccountEnumerator(CompanyClientTradingAccounts companyClientTradingAccounts)
            {
                _companyClientTradingAccounts = companyClientTradingAccounts;
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
                    if ((_location < 0) || (_location >= _companyClientTradingAccounts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companyClientTradingAccounts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companyClientTradingAccounts.Count)
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
