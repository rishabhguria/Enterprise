using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Sides.
    /// </summary>
    public class Sides : IList
    {
        ArrayList _sides = new ArrayList();

        public Sides()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _sides.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Sides.this getter implementation
                return _sides[index];
            }
            set
            {
                //Add Sides.this setter implementation
                _sides[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Sides.RemoveAt implementation
            _sides.RemoveAt(index);
        }

        public void Insert(int index, Object side)
        {
            //Add Sides.Insert implementation
            _sides.Insert(index, (Side)side);
        }

        public void Remove(Object side)
        {
            //Add Sides.Remove implementation
            _sides.Remove((Side)side);
        }

        public bool Contains(object side)
        {
            //Add Sides.Contains implementation
            return _sides.Contains((Side)side);
        }

        public void Clear()
        {
            //Add Sides.Clear implementation
            _sides.Clear();
        }

        public int IndexOf(object side)
        {
            //Add Sides.IndexOf implementation
            return _sides.IndexOf((Side)side);
        }

        public int Add(object side)
        {
            //Add Sides.Add implementation
            return _sides.Add((Side)side);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Sides.IsFixedSize getter implementation
                return _sides.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Sides.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _sides.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _sides.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _sides.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new SideEnumerator(this));
        }

        #endregion

        #region SideEnumerator Class

        public class SideEnumerator : IEnumerator
        {
            Sides _sides;
            int _location;

            public SideEnumerator(Sides sides)
            {
                _sides = sides;
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
                    if ((_location < 0) || (_location >= _sides.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _sides[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _sides.Count)
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
