using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Symbols.
    /// </summary>
    public class Symbols : IList
    {
        ArrayList _symbols = new ArrayList();

        public Symbols()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _symbols.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Symbols.this getter implementation
                return _symbols[index];
            }
            set
            {
                //Add Symbols.this setter implementation
                _symbols[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Symbols.RemoveAt implementation
            _symbols.RemoveAt(index);
        }

        public void Insert(int index, Object symbol)
        {
            //Add Symbols.Insert implementation
            _symbols.Insert(index, (Symbol)symbol);
        }

        public void Remove(Object symbol)
        {
            //Add Symbols.Remove implementation
            _symbols.Remove((Symbol)symbol);
        }

        public bool Contains(object symbol)
        {
            //Add Symbols.Contains implementation
            return _symbols.Contains((Symbol)symbol);
        }

        public void Clear()
        {
            //Add Symbols.Clear implementation
            _symbols.Clear();
        }

        public int IndexOf(object symbol)
        {
            //Add Symbols.IndexOf implementation
            return _symbols.IndexOf((Symbol)symbol);
        }

        public int Add(object symbol)
        {
            //Add Symbols.Add implementation
            return _symbols.Add((Symbol)symbol);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Symbols.IsFixedSize getter implementation
                return _symbols.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Symbols.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _symbols.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _symbols.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _symbols.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new SymbolEnumerator(this));
        }

        #endregion

        #region SymbolEnumerator Class

        public class SymbolEnumerator : IEnumerator
        {
            Symbols _symbols;
            int _location;

            public SymbolEnumerator(Symbols symbols)
            {
                _symbols = symbols;
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
                    if ((_location < 0) || (_location >= _symbols.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _symbols[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _symbols.Count)
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
