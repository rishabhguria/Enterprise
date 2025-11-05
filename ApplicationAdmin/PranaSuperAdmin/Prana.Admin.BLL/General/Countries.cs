using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Countries.
    /// </summary>
    public class Countries : IList
    {
        ArrayList _countries = new ArrayList();

        public Countries()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _countries.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Countries.this getter implementation
                return _countries[index];
            }
            set
            {
                //Add Countries.this setter implementation
                _countries[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Countries.RemoveAt implementation
            _countries.RemoveAt(index);
        }

        public void Insert(int index, Object country)
        {
            //Add Countries.Insert implementation
            _countries.Insert(index, (Country)country);
        }

        public void Remove(Object country)
        {
            //Add Countries.Remove implementation
            _countries.Remove((Country)country);
        }

        public bool Contains(object country)
        {
            //Add Countries.Contains implementation
            return _countries.Contains((Country)country);
        }

        public void Clear()
        {
            //Add Countries.Clear implementation
            _countries.Clear();
        }

        public int IndexOf(object country)
        {
            //Add Countries.IndexOf implementation
            return _countries.IndexOf((Country)country);
        }

        public int Add(object country)
        {
            //Add Countries.Add implementation
            return _countries.Add((Country)country);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Countries.IsFixedSize getter implementation
                return _countries.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Countries.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _countries.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _countries.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _countries.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CountryEnumerator(this));
        }

        #endregion

        #region CountryEnumerator Class

        public class CountryEnumerator : IEnumerator
        {
            Countries _countries;
            int _location;

            public CountryEnumerator(Countries countries)
            {
                _countries = countries;
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
                    if ((_location < 0) || (_location >= _countries.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _countries[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _countries.Count)
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
