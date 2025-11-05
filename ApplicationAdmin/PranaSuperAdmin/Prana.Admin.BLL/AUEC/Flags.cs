using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Flags.
    /// </summary>
    public class Flags : IList
    {
        ArrayList _flags = new ArrayList();
        public Flags()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _flags.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Flags.this getter implementation
                return _flags[index];
            }
            set
            {
                //Add Flags.this setter implementation
                _flags[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Flags.RemoveAt implementation
            _flags.RemoveAt(index);
        }

        public void Insert(int index, Object flag)
        {
            //Add Flags.Insert implementation
            _flags.Insert(index, (Flag)flag);
        }

        public void Remove(Object flag)
        {
            //Add Flags.Remove implementation
            _flags.Remove((Flag)flag);
        }

        public bool Contains(object flag)
        {
            //Add Flags.Contains implementation
            return _flags.Contains((Flag)flag);
        }

        public void Clear()
        {
            //Add Flags.Clear implementation
            _flags.Clear();
        }

        public int IndexOf(object flag)
        {
            //Add Flags.IndexOf implementation
            return _flags.IndexOf((Flag)flag);
        }

        public int Add(object flag)
        {
            //Add Flags.Add implementation
            return _flags.Add((Flag)flag);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Flags.IsFixedSize getter implementation
                return _flags.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Flags.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _flags.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _flags.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _flags.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new FlagEnumerator(this));
        }

        #endregion

        #region FlagEnumerator Class

        public class FlagEnumerator : IEnumerator
        {
            Flags _flags;
            int _location;

            public FlagEnumerator(Flags flags)
            {
                _flags = flags;
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
                    if ((_location < 0) || (_location >= _flags.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _flags[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _flags.Count)
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
