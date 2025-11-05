using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for SymbolConventions.
    /// </summary>
    public class SymbolConventions : IList
    {
        ArrayList _symbolConventions = new ArrayList();
        public SymbolConventions()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _symbolConventions.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                return _symbolConventions[index];
            }
            set
            {
                //Add Users.this setter implementation
                _symbolConventions[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _symbolConventions.RemoveAt(index);
        }

        public void Insert(int index, Object symbolConvention)
        {
            //Add Users.Insert implementation
            _symbolConventions.Insert(index, (SymbolConvention)symbolConvention);
        }

        public void Remove(Object symbolConvention)
        {
            //Add Users.Remove implementation
            _symbolConventions.Remove((SymbolConvention)symbolConvention);
        }

        public bool Contains(object symbolConvention)
        {
            //Add Users.Contains implementation
            return _symbolConventions.Contains((SymbolConvention)symbolConvention);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _symbolConventions.Clear();
        }

        public int IndexOf(object symbolConvention)
        {
            //Add Users.IndexOf implementation
            return _symbolConventions.IndexOf((SymbolConvention)symbolConvention);
        }

        public int Add(object symbolConvention)
        {
            //Add Users.Add implementation
            return _symbolConventions.Add((SymbolConvention)symbolConvention);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _symbolConventions.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add SymbolConventions.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _symbolConventions.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _symbolConventions.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _symbolConventions.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new SymbolConventionEnumerator(this));
        }

        #endregion

        #region SymbolConventionEnumerator Class

        public class SymbolConventionEnumerator : IEnumerator
        {
            SymbolConventions _symbolConventions;
            int _location;

            public SymbolConventionEnumerator(SymbolConventions venues)
            {
                _symbolConventions = venues;
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
                    if ((_location < 0) || (_location >= _symbolConventions.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _symbolConventions[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _symbolConventions.Count)
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
