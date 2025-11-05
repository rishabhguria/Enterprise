using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class VenueCollection : IList
    {
        ArrayList _venueCollection = new ArrayList();

        public VenueCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _venueCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add VenueCollection.this getter implementation
                return _venueCollection[index];
            }
            set
            {
                //Add VenueCollection.this setter implementation
                _venueCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add VenueCollection.RemoveAt implementation
            _venueCollection.RemoveAt(index);
        }

        public void Insert(int index, Object venue)
        {
            //Add VenueCollection.Insert implementation
            _venueCollection.Insert(index, (Venue)venue);
        }

        public void Remove(Object venue)
        {
            //Add VenueCollection.Remove implementation
            _venueCollection.Remove((Venue)venue);
        }

        public bool Contains(object venue)
        {
            //Add VenueCollection.Contains implementation
            return _venueCollection.Contains((Venue)venue);
        }

        public void Clear()
        {
            //Add VenueCollection.Clear implementation
            _venueCollection.Clear();
        }

        public int IndexOf(object venue)
        {
            //Add VenueCollection.IndexOf implementation
            return _venueCollection.IndexOf((Venue)venue);
        }

        public int Add(object venue)
        {
            //Add VenueCollection.Add implementation
            return _venueCollection.Add((Venue)venue);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add VenueCollection.IsFixedSize getter implementation
                return _venueCollection.IsFixedSize;
            }
        }

        #endregion
        public bool Contains(int venueID)
        {
            //Add CounterPartyCollection.Contains implementation
            foreach (Venue venue in _venueCollection)
            {
                if (venue.VenueID == venueID)
                {
                    return true;
                }
            }
            return false;
        }
        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add VenueCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _venueCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _venueCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _venueCollection.SyncRoot;
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

        #region AssetEnumerator Class

        public class VenueEnumerator : IEnumerator
        {
            VenueCollection _venueCollection;
            int _location;

            public VenueEnumerator(VenueCollection venueCollection)
            {
                _venueCollection = venueCollection;
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
                    if ((_location < 0) || (_location >= _venueCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _venueCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _venueCollection.Count)
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
