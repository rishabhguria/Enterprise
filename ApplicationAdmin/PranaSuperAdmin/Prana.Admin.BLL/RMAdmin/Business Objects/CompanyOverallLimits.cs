using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyOverallLimits.
    /// </summary>
    public class CompanyOverallLimits : IList
    {
        ArrayList _companyOverallLimits = new ArrayList();

        public CompanyOverallLimits()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companyOverallLimits.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CompanyOverallLimits.this getter implementation
                return _companyOverallLimits[index];
            }
            set
            {
                //Add CompanyOverallLimits.this setter implementation
                _companyOverallLimits[index] = value;
            }
        }
        public void RemoveAt(int index)
        {
            //Add CompanyOverallLimits.RemoveAt implementation
            _companyOverallLimits.RemoveAt(index);
        }

        public void Insert(int index, Object companyOverallLimits)
        {
            //Add CompanyOverallLimits.Insert implementation
            _companyOverallLimits.Insert(index, (CompanyOverallLimits)companyOverallLimits);
        }

        public void Remove(Object companyOverallLimits)
        {
            //Add CompanyOverallLimits.Remove implementation
            _companyOverallLimits.Remove((CompanyOverallLimits)companyOverallLimits);
        }

        public bool Contains(object companyOverallLimits)
        {
            //Add CompanyOverallLimits.Contains implementation
            return _companyOverallLimits.Contains((CompanyOverallLimits)companyOverallLimits);
        }

        public void Clear()
        {
            //Add CompanyOverallLimits.Clear implementation
            _companyOverallLimits.Clear();
        }

        public int IndexOf(object companyOverallLimits)
        {
            //Add CompanyOverallLimits.IndexOf implementation
            return _companyOverallLimits.IndexOf((CompanyOverallLimits)companyOverallLimits);
        }

        public int Add(object companyOverallLimits)
        {
            //Add CompanyOverallLimits.Add implementation
            return _companyOverallLimits.Add((CompanyOverallLimit)companyOverallLimits);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CompanyOverallLimits.IsFixedSize getter implementation
                return _companyOverallLimits.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CompanyOverallLimits.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _companyOverallLimits.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companyOverallLimits.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companyOverallLimits.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CompanyOverallLimitsEnumerator(this));
        }

        #endregion

        #region CompanyOverallLimitsEnumerator Class

        public class CompanyOverallLimitsEnumerator : IEnumerator
        {
            CompanyOverallLimits _companyOverallLimits;
            int _location;

            public CompanyOverallLimitsEnumerator(CompanyOverallLimits companyOverallLimits)
            {
                _companyOverallLimits = companyOverallLimits;
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
                    if ((_location < 0) || (_location >= _companyOverallLimits.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companyOverallLimits[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companyOverallLimits.Count)
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
