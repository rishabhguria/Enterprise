using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Identifiers.
    /// </summary>
    public class Identifiers : IList
    {
        ArrayList _identifiers = new ArrayList();
        public Identifiers()
        {
        }
        public Identifier GetIdentifier(string PrimaryKey)
        {
            Identifier temp = new Identifier();
            foreach (Identifier identifier in _identifiers)
            {
                if (identifier.PrimaryKey.Equals(PrimaryKey))
                {
                    temp = identifier;
                    break;
                }
            }
            return temp;

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _identifiers.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Identifiers.this getter implementation
                return _identifiers[index];
            }
            set
            {
                //Add Identifiers.this setter implementation
                _identifiers[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Identifiers.RemoveAt implementation
            _identifiers.RemoveAt(index);
        }

        public void Insert(int index, Object identifier)
        {
            //Add Identifiers.Insert implementation
            _identifiers.Insert(index, (Identifier)identifier);
        }

        public void Remove(Object identifier)
        {
            //Add Identifiers.Remove implementation
            _identifiers.Remove((Identifier)identifier);
        }

        public bool Contains(object identifier)
        {
            //Add Identifiers.Contains implementation
            return _identifiers.Contains((Identifier)identifier);
        }
        public bool ContainsSameName(Identifier identifier)
        {
            bool bMatch = false;
            foreach (Identifier temp in _identifiers)
            {
                if (string.Compare(identifier.ClientIdentifierName, temp.ClientIdentifierName, true) == 0)
                {
                    bMatch = true;
                    break;
                }
            }
            return bMatch;
            //else
            //	return false;
        }

        public void Clear()
        {
            //Add Identifiers.Clear implementation
            _identifiers.Clear();
        }

        public int IndexOf(object identifier)
        {
            //Add Identifiers.IndexOf implementation
            return _identifiers.IndexOf((Identifier)identifier);
        }

        public int Add(object identifier)
        {
            //Add Identifiers.Add implementation
            return _identifiers.Add((Identifier)identifier);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Identifiers.IsFixedSize getter implementation
                return _identifiers.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Identifiers.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _identifiers.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _identifiers.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _identifiers.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new IdentifierEnumerator(this));
        }

        #endregion

        #region IdentifierEnumerator Class

        public class IdentifierEnumerator : IEnumerator
        {
            Identifiers _identifiers;
            int _location;

            public IdentifierEnumerator(Identifiers identifiers)
            {
                _identifiers = identifiers;
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
                    if ((_location < 0) || (_location >= _identifiers.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _identifiers[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _identifiers.Count)
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
