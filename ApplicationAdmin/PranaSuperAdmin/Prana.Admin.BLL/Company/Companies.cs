using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Companies.
    /// </summary>
    public class Companies : IList
    {
        ArrayList _companies = new ArrayList();

        public Companies()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companies.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                //return _companies[index];
                if (index >= _companies.Count || index < 0)
                {
                    return new Companies();
                }
                else
                {
                    return _companies[index];
                }
            }
            set
            {
                //Add Users.this setter implementation
                _companies[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _companies.RemoveAt(index);
        }

        public void Insert(int index, Object company)
        {
            //Add Users.Insert implementation
            _companies.Insert(index, (Company)company);
        }

        public void Remove(Object company)
        {
            //Add Users.Remove implementation
            _companies.Remove((Company)company);
        }

        public bool Contains(object company)
        {
            //Add Users.Contains implementation
            return _companies.Contains((Company)company);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _companies.Clear();
        }

        public int IndexOf(object company)
        {
            //Add Users.IndexOf implementation
            //return _companies.IndexOf((Company)company);
            Company tempCompany = (Company)company;
            int counter = 0;
            int result = int.MinValue;
            foreach (Company _company in _companies)
            {
                if (_company.CompanyID == tempCompany.CompanyID
                    && _company.BorrowerName == tempCompany.BorrowerName
                    && _company.BorrowerShortName == tempCompany.BorrowerShortName
                    && _company.FirmID == tempCompany.FirmID
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

        public int Add(object company)
        {
            //Add Users.Add implementation
            return _companies.Add((Company)company);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _companies.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Users.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _companies.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companies.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companies.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CompanyEnumerator(this));
        }

        #endregion

        #region CompanyEnumerator Class

        public class CompanyEnumerator : IEnumerator
        {
            Companies _companies;
            int _location;

            public CompanyEnumerator(Companies companies)
            {
                _companies = companies;
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
                    if ((_location < 0) || (_location >= _companies.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companies[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companies.Count)
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
