using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyCounterPartyVenues.
    /// </summary>
    public class CompanyCounterPartyVenues : IList
    {
        ArrayList _companyCounterPartyVenues = new ArrayList();
        public CompanyCounterPartyVenues()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companyCounterPartyVenues.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CompanyCounterPartyVenues.this getter implementation
                return _companyCounterPartyVenues[index];
            }
            set
            {
                //Add CompanyCounterPartyVenues.this setter implementation
                _companyCounterPartyVenues[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CompanyCounterPartyVenues.RemoveAt implementation
            _companyCounterPartyVenues.RemoveAt(index);
        }

        public void Insert(int index, Object companyCounterPartyVenue)
        {
            //Add CompanyCounterPartyVenues.Insert implementation
            _companyCounterPartyVenues.Insert(index, (CompanyCounterPartyVenue)companyCounterPartyVenue);
        }

        public void Remove(Object companyCounterPartyVenue)
        {
            //Add CompanyCounterPartyVenues.Remove implementation
            _companyCounterPartyVenues.Remove((CompanyCounterPartyVenue)companyCounterPartyVenue);
        }

        public bool Contains(object companyCounterPartyVenue)
        {
            //Add CompanyCounterPartyVenues.Contains implementation
            return _companyCounterPartyVenues.Contains((CompanyCounterPartyVenue)companyCounterPartyVenue);
        }

        public void Clear()
        {
            //Add CompanyCounterPartyVenues.Clear implementation
            _companyCounterPartyVenues.Clear();
        }

        public int IndexOf(object companyCounterPartyVenue)
        {
            //Add CompanyCounterPartyVenues.IndexOf implementation
            return _companyCounterPartyVenues.IndexOf((CompanyCounterPartyVenue)companyCounterPartyVenue);
        }

        public int Add(object companyCounterPartyVenue)
        {
            //Add CompanyCounterPartyVenues.Add implementation
            return _companyCounterPartyVenues.Add((CompanyCounterPartyVenue)companyCounterPartyVenue);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CompanyCounterPartyVenues.IsFixedSize getter implementation
                return _companyCounterPartyVenues.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CompanyCounterPartyVenues.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _companyCounterPartyVenues.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companyCounterPartyVenues.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companyCounterPartyVenues.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CompanyCounterPartyVenueEnumerator(this));
        }

        #endregion

        #region CompanyCounterPartyVenueEnumerator Class

        public class CompanyCounterPartyVenueEnumerator : IEnumerator
        {
            CompanyCounterPartyVenues _companyCounterPartyVenues;
            int _location;

            public CompanyCounterPartyVenueEnumerator(CompanyCounterPartyVenues companyCounterPartyVenues)
            {
                _companyCounterPartyVenues = companyCounterPartyVenues;
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
                    if ((_location < 0) || (_location >= _companyCounterPartyVenues.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companyCounterPartyVenues[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companyCounterPartyVenues.Count)
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
