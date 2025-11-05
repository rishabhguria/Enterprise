using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for FutureMonthCodes.
    /// </summary>
    public class FutureMonthCodes : IList
    {
        ArrayList _futureMonthCodes = new ArrayList();
        public FutureMonthCodes()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _futureMonthCodes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add FutureMonthCodes.this getter implementation
                return _futureMonthCodes[index];
            }
            set
            {
                //Add FutureMonthCodes.this setter implementation
                _futureMonthCodes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add FutureMonthCodes.RemoveAt implementation
            _futureMonthCodes.RemoveAt(index);
        }

        public void Insert(int index, Object futureMonthCode)
        {
            //Add FutureMonthCodes.Insert implementation
            _futureMonthCodes.Insert(index, (FutureMonthCode)futureMonthCode);
        }

        public void Remove(Object futureMonthCode)
        {
            //Add FutureMonthCodes.Remove implementation
            _futureMonthCodes.Remove((FutureMonthCode)futureMonthCode);
        }

        public bool Contains(object futureMonthCode)
        {
            //Add FutureMonthCodes.Contains implementation
            return _futureMonthCodes.Contains((FutureMonthCode)futureMonthCode);
        }

        public void Clear()
        {
            //Add FutureMonthCodes.Clear implementation
            _futureMonthCodes.Clear();
        }

        public int IndexOf(object futureMonthCode)
        {
            //Add FutureMonthCodes.IndexOf implementation
            return _futureMonthCodes.IndexOf((FutureMonthCode)futureMonthCode);
        }

        public int Add(object futureMonthCode)
        {
            //Add FutureMonthCodes.Add implementation
            return _futureMonthCodes.Add((FutureMonthCode)futureMonthCode);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add FutureMonthCodes.IsFixedSize getter implementation
                return _futureMonthCodes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add FutureMonthCodes.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _futureMonthCodes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _futureMonthCodes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _futureMonthCodes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new FutureMonthCodeEnumerator(this));
        }

        #endregion

        #region FutureMonthCodeEnumerator Class

        public class FutureMonthCodeEnumerator : IEnumerator
        {
            FutureMonthCodes _futureMonthCodes;
            int _location;

            public FutureMonthCodeEnumerator(FutureMonthCodes futureMonthCodes)
            {
                _futureMonthCodes = futureMonthCodes;
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
                    if ((_location < 0) || (_location >= _futureMonthCodes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _futureMonthCodes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _futureMonthCodes.Count)
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
