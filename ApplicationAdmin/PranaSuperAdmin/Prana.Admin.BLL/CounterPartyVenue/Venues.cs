using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Venues.
    /// </summary>
    public class Venues : IList
    {
        ArrayList _venues = new ArrayList();
        public Venues()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _venues.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                return _venues[index];
            }
            set
            {
                //Add Users.this setter implementation
                _venues[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _venues.RemoveAt(index);
        }

        public void Insert(int index, Object venue)
        {
            //Add Users.Insert implementation
            _venues.Insert(index, (Venue)venue);
        }

        public void Remove(Object venue)
        {
            //Add Users.Remove implementation
            _venues.Remove((Venue)venue);
        }

        public bool Contains(object venue)
        {
            //Add Users.Contains implementation
            return _venues.Contains((Venue)venue);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _venues.Clear();
        }

        public int IndexOf(object venue)
        {
            //Add Users.IndexOf implementation
            return _venues.IndexOf((Venue)venue);
        }

        public int Add(object venue)
        {
            //Add Users.Add implementation
            return _venues.Add((Venue)venue);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _venues.IsFixedSize;
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
                return _venues.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _venues.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _venues.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new VenueEnumerator(this));
        }

        #endregion

        #region VenueEnumerator Class

        public class VenueEnumerator : IEnumerator
        {
            Venues _venues;
            int _location;

            public VenueEnumerator(Venues venues)
            {
                _venues = venues;
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
                    if ((_location < 0) || (_location >= _venues.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _venues[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _venues.Count)
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
