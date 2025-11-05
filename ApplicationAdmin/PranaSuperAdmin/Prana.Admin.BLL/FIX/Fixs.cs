using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Fixs is a collection class for <see cref="Fix"/> class.
    /// </summary>
    public class Fixs : IList
    {
        ArrayList _fixs = new ArrayList();

        public Fixs()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _fixs.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Fixs.this getter implementation
                return _fixs[index];
            }
            set
            {
                //Add Fixs.this setter implementation
                _fixs[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Fixs.RemoveAt implementation
            _fixs.RemoveAt(index);
        }

        public void Insert(int index, Object fix)
        {
            //Add Fixs.Insert implementation
            _fixs.Insert(index, (Fix)fix);
        }

        public void Remove(Object fix)
        {
            //Add Fixs.Remove implementation
            _fixs.Remove((Fix)fix);
        }

        public bool Contains(object fix)
        {
            //Add Fixs.Contains implementation
            return _fixs.Contains((Fix)fix);
        }

        public void Clear()
        {
            //Add Fixs.Clear implementation
            _fixs.Clear();
        }

        public int IndexOf(object fix)
        {
            //Add Fixs.IndexOf implementation
            return _fixs.IndexOf((Fixs)fix);
        }

        public int Add(object fix)
        {
            //Add Fixs.Add implementation
            return _fixs.Add((Fix)fix);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Fixs.IsFixedSize getter implementation
                return _fixs.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Fixs.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _fixs.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _fixs.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _fixs.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new FixEnumerator(this));
        }

        #endregion

        #region FixEnumerator Class

        public class FixEnumerator : IEnumerator
        {
            Fixs _fixs;
            int _location;

            public FixEnumerator(Fixs fixs)
            {
                _fixs = fixs;
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
                    if ((_location < 0) || (_location >= _fixs.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _fixs[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _fixs.Count)
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
