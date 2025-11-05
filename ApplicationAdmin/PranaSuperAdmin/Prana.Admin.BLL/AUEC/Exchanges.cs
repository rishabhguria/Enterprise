using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Exchanges.
    /// </summary>
    public class Exchanges : IList
    {
        ArrayList _exchanges = new ArrayList();

        public Exchanges()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _exchanges.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Exchanges.this getter implementation
                return _exchanges[index];
            }
            set
            {
                //Add Exchanges.this setter implementation
                _exchanges[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Exchanges.RemoveAt implementation
            _exchanges.RemoveAt(index);
        }

        public void Insert(int index, Object exchange)
        {
            //Add Exchanges.Insert implementation
            _exchanges.Insert(index, (Exchange)exchange);
        }

        public void Remove(Object exchange)
        {
            //Add Exchanges.Remove implementation
            _exchanges.Remove((Exchange)exchange);
        }

        public bool Contains(object exchange)
        {
            //Add Exchanges.Contains implementation
            return _exchanges.Contains((Exchange)exchange);
        }

        public void Clear()
        {
            //Add Exchanges.Clear implementation
            _exchanges.Clear();
        }

        public int IndexOf(object exchange)
        {
            //Add Exchanges.IndexOf implementation
            return _exchanges.IndexOf((Exchange)exchange);
        }

        public int Add(object exchange)
        {
            //Add Exchanges.Add implementation
            return _exchanges.Add((Exchange)exchange);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Exchanges.IsFixedSize getter implementation
                return _exchanges.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Exchanges.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _exchanges.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _exchanges.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _exchanges.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ExchangeEnumerator(this));
        }

        #endregion

        #region ExchangeEnumerator Class

        public class ExchangeEnumerator : IEnumerator
        {
            Exchanges _exchanges;
            int _location;

            public ExchangeEnumerator(Exchanges assets)
            {
                _exchanges = assets;
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
                    if ((_location < 0) || (_location >= _exchanges.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _exchanges[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _exchanges.Count)
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
