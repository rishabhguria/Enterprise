using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyTypes.
    /// </summary>
    public class CompanyTypes : IList
    {
        ArrayList _companyTypes = new ArrayList();

        public CompanyTypes()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companyTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                return _companyTypes[index];
            }
            set
            {
                //Add Users.this setter implementation
                _companyTypes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _companyTypes.RemoveAt(index);
        }

        public void Insert(int index, Object companyType)
        {
            //Add Users.Insert implementation
            _companyTypes.Insert(index, (CompanyType)companyType);
        }

        public void Remove(Object companyType)
        {
            //Add Users.Remove implementation
            _companyTypes.Remove((CompanyType)companyType);
        }

        public bool Contains(object companyType)
        {
            //Add Users.Contains implementation
            return _companyTypes.Contains((CompanyType)companyType);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _companyTypes.Clear();
        }

        public int IndexOf(object companyType)
        {
            //Add Users.IndexOf implementation
            return _companyTypes.IndexOf((CompanyType)companyType);
        }

        public int Add(object companyType)
        {
            //Add Users.Add implementation
            return _companyTypes.Add((CompanyType)companyType);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _companyTypes.IsFixedSize;
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
                return _companyTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companyTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companyTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CompanyTypeEnumerator(this));
        }

        #endregion

        #region CompanyTypeEnumerator Class

        public class CompanyTypeEnumerator : IEnumerator
        {
            CompanyTypes _companyTypes;
            int _location;

            public CompanyTypeEnumerator(CompanyTypes companyTypes)
            {
                _companyTypes = companyTypes;
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
                    if ((_location < 0) || (_location >= _companyTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companyTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companyTypes.Count)
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
