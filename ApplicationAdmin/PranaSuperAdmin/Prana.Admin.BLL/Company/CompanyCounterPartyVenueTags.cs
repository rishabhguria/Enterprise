using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyCounterPartyVenueTags.
    /// </summary>
    public class CompanyCounterPartyVenueTags : IList
    {
        ArrayList _companyCounterPartyVenueTags = new ArrayList();
        public CompanyCounterPartyVenueTags()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companyCounterPartyVenueTags.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CompanyCounterPartyVenueTags.this getter implementation
                return _companyCounterPartyVenueTags[index];
            }
            set
            {
                //Add CompanyCounterPartyVenueTags.this setter implementation
                _companyCounterPartyVenueTags[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CompanyCounterPartyVenueTags.RemoveAt implementation
            _companyCounterPartyVenueTags.RemoveAt(index);
        }

        public void Insert(int index, Object companyCounterPartyVenueTag)
        {
            //Add CompanyCounterPartyVenueTags.Insert implementation
            _companyCounterPartyVenueTags.Insert(index, (CompanyCounterPartyVenueTag)companyCounterPartyVenueTag);
        }

        public void Remove(Object companyCounterPartyVenueTag)
        {
            //Add CompanyCounterPartyVenueTags.Remove implementation
            _companyCounterPartyVenueTags.Remove((CompanyCounterPartyVenueTag)companyCounterPartyVenueTag);
        }

        public bool Contains(object companyCounterPartyVenueTag)
        {
            //Add CompanyCounterPartyVenueTags.Contains implementation
            return _companyCounterPartyVenueTags.Contains((CompanyCounterPartyVenueTag)companyCounterPartyVenueTag);
        }

        public void Clear()
        {
            //Add CompanyCounterPartyVenueTags.Clear implementation
            _companyCounterPartyVenueTags.Clear();
        }

        public int IndexOf(object companyCounterPartyVenueTag)
        {
            //Add CompanyCounterPartyVenueTags.IndexOf implementation
            return _companyCounterPartyVenueTags.IndexOf((CompanyCounterPartyVenueTag)companyCounterPartyVenueTag);
        }

        public int Add(object companyCounterPartyVenueTag)
        {
            //Add CompanyCounterPartyVenueTags.Add implementation
            return _companyCounterPartyVenueTags.Add((CompanyCounterPartyVenueTag)companyCounterPartyVenueTag);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CompanyCounterPartyVenueTags.IsFixedSize getter implementation
                return _companyCounterPartyVenueTags.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CompanyCounterPartyVenueTags.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _companyCounterPartyVenueTags.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companyCounterPartyVenueTags.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companyCounterPartyVenueTags.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CompanyCounterPartyVenueTagEnumerator(this));
        }

        #endregion

        #region CompanyCounterPartyVenueTagEnumerator Class

        public class CompanyCounterPartyVenueTagEnumerator : IEnumerator
        {
            CompanyCounterPartyVenueTags _companyCounterPartyVenueTags;
            int _location;

            public CompanyCounterPartyVenueTagEnumerator(CompanyCounterPartyVenueTags companyCounterPartyVenueTags)
            {
                _companyCounterPartyVenueTags = companyCounterPartyVenueTags;
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
                    if ((_location < 0) || (_location >= _companyCounterPartyVenueTags.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companyCounterPartyVenueTags[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companyCounterPartyVenueTags.Count)
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
