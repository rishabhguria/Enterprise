using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CounterPartyVenues.
    /// </summary>
    public class CounterPartyVenues : IList
    {
        ArrayList _counterPartyVenues = new ArrayList();
        public CounterPartyVenues()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _counterPartyVenues.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                return _counterPartyVenues[index];
            }
            set
            {
                //Add Users.this setter implementation
                _counterPartyVenues[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _counterPartyVenues.RemoveAt(index);
        }

        public void Insert(int index, Object counterPartyVenue)
        {
            //Add Users.Insert implementation
            _counterPartyVenues.Insert(index, (CounterPartyVenue)counterPartyVenue);
        }

        public void Remove(Object counterPartyVenue)
        {
            //Add Users.Remove implementation
            _counterPartyVenues.Remove((CounterPartyVenue)counterPartyVenue);
        }

        public bool Contains(object counterPartyVenue)
        {
            //Add Users.Contains implementation
            return _counterPartyVenues.Contains((CounterPartyVenue)counterPartyVenue);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _counterPartyVenues.Clear();
        }

        public int IndexOf(object counterPartyVenue)
        {
            //Add Users.IndexOf implementation
            return _counterPartyVenues.IndexOf((CounterPartyVenue)counterPartyVenue);
        }

        public int Add(object counterPartyVenue)
        {
            //Add Users.Add implementation
            return _counterPartyVenues.Add((CounterPartyVenue)counterPartyVenue);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _counterPartyVenues.IsFixedSize;
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
                return _counterPartyVenues.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _counterPartyVenues.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _counterPartyVenues.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CounterPartyVenueEnumerator(this));
        }

        #endregion

        #region CounterPartyVenueEnumerator Class

        public class CounterPartyVenueEnumerator : IEnumerator
        {
            CounterPartyVenues _counterPartyVenues;
            int _location;

            public CounterPartyVenueEnumerator(CounterPartyVenues counterPartyVenues)
            {
                _counterPartyVenues = counterPartyVenues;
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
                    if ((_location < 0) || (_location >= _counterPartyVenues.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _counterPartyVenues[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _counterPartyVenues.Count)
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
