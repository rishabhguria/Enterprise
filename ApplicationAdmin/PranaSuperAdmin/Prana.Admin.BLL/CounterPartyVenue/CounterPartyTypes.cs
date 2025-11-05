using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CounterPartyTypes.
    /// </summary>
    public class CounterPartyTypes : IList
    {
        ArrayList _counterPartyTypes = new ArrayList();

        public CounterPartyTypes()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _counterPartyTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                return _counterPartyTypes[index];
            }
            set
            {
                //Add Users.this setter implementation
                _counterPartyTypes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _counterPartyTypes.RemoveAt(index);
        }

        public void Insert(int index, Object counterPartyType)
        {
            //Add Users.Insert implementation
            _counterPartyTypes.Insert(index, (CounterPartyType)counterPartyType);
        }

        public void Remove(Object counterPartyType)
        {
            //Add Users.Remove implementation
            _counterPartyTypes.Remove((CounterPartyType)counterPartyType);
        }

        public bool Contains(object counterPartyType)
        {
            //Add Users.Contains implementation
            return _counterPartyTypes.Contains((CounterPartyType)counterPartyType);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _counterPartyTypes.Clear();
        }

        public int IndexOf(object counterPartyType)
        {
            //Add Users.IndexOf implementation
            return _counterPartyTypes.IndexOf((CounterPartyType)counterPartyType);
        }

        public int Add(object counterPartyType)
        {
            //Add Users.Add implementation
            return _counterPartyTypes.Add((CounterPartyType)counterPartyType);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _counterPartyTypes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Users.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _counterPartyTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _counterPartyTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _counterPartyTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CounterPartyTypeEnumerator(this));
        }

        #endregion

        #region CounterPartyTypeEnumerator Class

        public class CounterPartyTypeEnumerator : IEnumerator
        {
            CounterPartyTypes _counterPartyTypes;
            int _location;

            public CounterPartyTypeEnumerator(CounterPartyTypes counterPartyTypes)
            {
                _counterPartyTypes = counterPartyTypes;
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
                    if ((_location < 0) || (_location >= _counterPartyTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _counterPartyTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _counterPartyTypes.Count)
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
