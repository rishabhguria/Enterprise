using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Venues.
    /// </summary>
    public class SymbolConversions : IList
    {
        ArrayList _symbolConversions = new ArrayList();
        public SymbolConversions()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _symbolConversions.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                return _symbolConversions[index];
            }
            set
            {
                //Add Users.this setter implementation
                _symbolConversions[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _symbolConversions.RemoveAt(index);
        }

        public void Insert(int index, Object symbol)
        {
            //Add Users.Insert implementation
            _symbolConversions.Insert(index, (SymbolConversion)symbol);
        }

        public void Remove(Object symbol)
        {
            //Add Users.Remove implementation
            _symbolConversions.Remove((SymbolConversion)symbol);
        }

        public bool Contains(object symbol)
        {
            //Add Users.Contains implementation
            return _symbolConversions.Contains((SymbolConversion)symbol);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _symbolConversions.Clear();
        }

        public int IndexOf(object symbol)
        {
            //Add Users.IndexOf implementation
            return _symbolConversions.IndexOf((SymbolConversion)symbol);
        }

        public int Add(object symbol)
        {
            //Add Users.Add implementation
            return _symbolConversions.Add((SymbolConversion)symbol);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _symbolConversions.IsFixedSize;
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
                return _symbolConversions.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _symbolConversions.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _symbolConversions.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new SymbolConversionEnumerator(this));
        }

        #endregion

        #region SymbolConversionEnumerator Class

        public class SymbolConversionEnumerator : IEnumerator
        {
            SymbolConversions _symbolConversions;
            int _location;

            public SymbolConversionEnumerator(SymbolConversions symbolConversions)
            {
                _symbolConversions = symbolConversions;
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
                    if ((_location < 0) || (_location >= _symbolConversions.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _symbolConversions[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _symbolConversions.Count)
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
