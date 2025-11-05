using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for UnderLyings.
    /// </summary>
    public class UnderLyings : IList
    {
        ArrayList _underlyings = new ArrayList();

        public UnderLyings()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _underlyings.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add UnderLyings.this getter implementation
                return _underlyings[index];
            }
            set
            {
                //Add UnderLyings.this setter implementation
                _underlyings[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add UnderLyings.RemoveAt implementation
            _underlyings.RemoveAt(index);
        }

        public void Insert(int index, Object underlying)
        {
            //Add UnderLyings.Insert implementation
            _underlyings.Insert(index, (UnderLying)underlying);
        }

        public void Remove(Object underlying)
        {
            //Add UnderLyings.Remove implementation
            _underlyings.Remove((UnderLying)underlying);
        }

        public bool Contains(object underlying)
        {
            //Add UnderLyings.Contains implementation
            return _underlyings.Contains((UnderLying)underlying);
        }

        public void Clear()
        {
            //Add UnderLyings.Clear implementation
            _underlyings.Clear();
        }

        public int IndexOf(object underlying)
        {
            //Add UnderLyings.IndexOf implementation
            return _underlyings.IndexOf((UnderLying)underlying);
        }

        public int Add(object underlying)
        {
            //Add UnderLyings.Add implementation
            return _underlyings.Add((UnderLying)underlying);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add UnderLyings.IsFixedSize getter implementation
                return _underlyings.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add UnderLyings.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _underlyings.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _underlyings.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _underlyings.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new AssetEnumerator(this));
        }

        #endregion

        #region AssetEnumerator Class

        public class AssetEnumerator : IEnumerator
        {
            UnderLyings _underlyings;
            int _location;

            public AssetEnumerator(UnderLyings underlyings)
            {
                _underlyings = underlyings;
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
                    if ((_location < 0) || (_location >= _underlyings.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _underlyings[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _underlyings.Count)
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
