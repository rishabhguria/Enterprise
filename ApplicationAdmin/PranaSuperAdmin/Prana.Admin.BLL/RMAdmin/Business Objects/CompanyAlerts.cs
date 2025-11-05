using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyAlerts.
    /// </summary>
    public class CompanyAlerts : IList
    {
        ArrayList _companyAlerts = new ArrayList();

        public CompanyAlerts()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companyAlerts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CompanyAlerts.this getter implementation
                return _companyAlerts[index];
            }
            set
            {
                //Add CompanyAlerts.this setter implementation
                _companyAlerts[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CompanyAlerts.RemoveAt implementation
            _companyAlerts.RemoveAt(index);
        }

        public void Insert(int index, Object companyAlert)
        {
            //Add CompanyAlerts.Insert implementation
            _companyAlerts.Insert(index, (CompanyAlert)companyAlert);
        }

        public void Remove(Object companyAlert)
        {
            //Add CompanyAlerts.Remove implementation
            _companyAlerts.Remove((CompanyAlert)companyAlert);
        }

        public bool Contains(object companyAlert)
        {
            //Add CompanyAlerts.Contains implementation
            return _companyAlerts.Contains((CompanyAlert)companyAlert);
        }

        public void Clear()
        {
            //Add CompanyAlerts.Clear implementation
            _companyAlerts.Clear();
        }

        public int IndexOf(object companyAlert)
        {
            //Add CompanyAlerts.IndexOf implementation
            return _companyAlerts.IndexOf((CompanyAlert)companyAlert);
        }

        public int Add(object companyAlert)
        {
            //Add CompanyAlerts.Add implementation
            return _companyAlerts.Add((CompanyAlert)companyAlert);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CompanyAlerts.IsFixedSize getter implementation
                return _companyAlerts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CompanyAlerts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _companyAlerts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companyAlerts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companyAlerts.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CompanyAlertEnumerator(this));
        }

        #endregion

        #region CompanyAlertEnumerator Class

        public class CompanyAlertEnumerator : IEnumerator
        {
            CompanyAlerts _companyAlerts;
            int _location;

            public CompanyAlertEnumerator(CompanyAlerts companyAlerts)
            {
                _companyAlerts = companyAlerts;
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
                    if ((_location < 0) || (_location >= _companyAlerts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companyAlerts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companyAlerts.Count)
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
