using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for SymbolIdentifiers.
    /// </summary>
    public class SymbolIdentifiers : IList
    {
        ArrayList _symbolIdentifiers = new ArrayList();

        public SymbolIdentifiers()
        {
        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _symbolIdentifiers.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add SymbolIdentifiers.this getter implementation
                return _symbolIdentifiers[index];
            }
            set
            {
                //Add SymbolIdentifiers.this setter implementation
                _symbolIdentifiers[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add SymbolIdentifiers.RemoveAt implementation
            _symbolIdentifiers.RemoveAt(index);
        }

        public void Insert(int index, Object symbolIdentifier)
        {
            //Add SymbolIdentifiers.Insert implementation
            _symbolIdentifiers.Insert(index, (SymbolIdentifier)symbolIdentifier);
        }

        public void Remove(Object symbolIdentifier)
        {
            //Add SymbolIdentifiers.Remove implementation
            _symbolIdentifiers.Remove((SymbolIdentifier)symbolIdentifier);
        }

        public bool Contains(object symbolIdentifier)
        {
            //Add SymbolIdentifiers.Contains implementation
            return _symbolIdentifiers.Contains((SymbolIdentifier)symbolIdentifier);
        }

        public void Clear()
        {
            //Add SymbolIdentifiers.Clear implementation
            _symbolIdentifiers.Clear();
        }

        public int IndexOf(object symbolIdentifier)
        {
            //Add SymbolIdentifiers.IndexOf implementation
            return _symbolIdentifiers.IndexOf((SymbolIdentifier)symbolIdentifier);
        }

        public int Add(object symbolIdentifier)
        {
            //Add SymbolIdentifiers.Add implementation
            return _symbolIdentifiers.Add((SymbolIdentifier)symbolIdentifier);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add SymbolIdentifiers.IsFixedSize getter implementation
                return _symbolIdentifiers.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add SymbolIdentifiers.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _symbolIdentifiers.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _symbolIdentifiers.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _symbolIdentifiers.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new SymbolIdentifierEnumerator(this));
        }

        #endregion

        #region SymbolIdentifierEnumerator Class

        public class SymbolIdentifierEnumerator : IEnumerator
        {
            SymbolIdentifiers _symbolIdentifiers;
            int _location;

            public SymbolIdentifierEnumerator(SymbolIdentifiers symbolIdentifiers)
            {
                _symbolIdentifiers = symbolIdentifiers;
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
                    if ((_location < 0) || (_location >= _symbolIdentifiers.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _symbolIdentifiers[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _symbolIdentifiers.Count)
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
