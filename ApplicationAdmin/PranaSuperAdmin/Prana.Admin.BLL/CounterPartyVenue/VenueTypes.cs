using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for VenueTypes.
    /// </summary>
    public class VenueTypes : IList
    {
        ArrayList _venueTypes = new ArrayList();
        public VenueTypes()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _venueTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                return _venueTypes[index];
            }
            set
            {
                //Add Users.this setter implementation
                _venueTypes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _venueTypes.RemoveAt(index);
        }

        public void Insert(int index, Object venueType)
        {
            //Add Users.Insert implementation
            _venueTypes.Insert(index, (VenueType)venueType);
        }

        public void Remove(Object venueType)
        {
            //Add Users.Remove implementation
            _venueTypes.Remove((VenueType)venueType);
        }

        public bool Contains(object venueType)
        {
            //Add Users.Contains implementation
            return _venueTypes.Contains((VenueType)venueType);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _venueTypes.Clear();
        }

        public int IndexOf(object venueType)
        {
            //Add Users.IndexOf implementation
            return _venueTypes.IndexOf((VenueType)venueType);
        }

        public int Add(object venueType)
        {
            //Add Users.Add implementation
            return _venueTypes.Add((VenueType)venueType);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _venueTypes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Venues.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _venueTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _venueTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _venueTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new VenueTypeEnumerator(this));
        }

        #endregion

        #region VenueTypeEnumerator Class

        public class VenueTypeEnumerator : IEnumerator
        {
            VenueTypes _venueTypes;
            int _location;

            public VenueTypeEnumerator(VenueTypes venueTypes)
            {
                _venueTypes = venueTypes;
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
                    if ((_location < 0) || (_location >= _venueTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _venueTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _venueTypes.Count)
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
