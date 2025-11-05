using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyClients.
    /// </summary>
    public class CompanyClients : IList
    {
        ArrayList _companyClients = new ArrayList();
        public CompanyClients()
        {
        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companyClients.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CompanyClients.this getter implementation
                return _companyClients[index];
            }
            set
            {
                //Add CompanyClients.this setter implementation
                _companyClients[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CompanyClients.RemoveAt implementation
            _companyClients.RemoveAt(index);
        }

        public void Insert(int index, Object companyClient)
        {
            //Add CompanyClients.Insert implementation
            _companyClients.Insert(index, (CompanyClient)companyClient);
        }

        public void Remove(Object companyClient)
        {
            //Add CompanyClients.Remove implementation
            _companyClients.Remove((CompanyClient)companyClient);
        }

        public bool Contains(object companyClient)
        {
            //Add CompanyClients.Contains implementation
            return _companyClients.Contains((CompanyClient)companyClient);
        }

        public void Clear()
        {
            //Add CompanyClients.Clear implementation
            _companyClients.Clear();
        }

        public int IndexOf(object companyClient)
        {
            //Add CompanyClients.IndexOf implementation
            return _companyClients.IndexOf((CompanyClient)companyClient);
        }

        public int Add(object companyClient)
        {
            //Add CompanyClients.Add implementation
            return _companyClients.Add((CompanyClient)companyClient);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CompanyClients.IsFixedSize getter implementation
                return _companyClients.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CompanyClients.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _companyClients.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companyClients.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companyClients.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CompanyClientEnumerator(this));
        }

        #endregion

        #region AssetEnumerator Class

        public class CompanyClientEnumerator : IEnumerator
        {
            CompanyClients _companyClients;
            int _location;

            public CompanyClientEnumerator(CompanyClients companyClients)
            {
                _companyClients = companyClients;
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
                    if ((_location < 0) || (_location >= _companyClients.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companyClients[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companyClients.Count)
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
