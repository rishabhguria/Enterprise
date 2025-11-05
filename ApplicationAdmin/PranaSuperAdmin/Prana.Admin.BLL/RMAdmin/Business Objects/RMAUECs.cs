using System;
using System.Collections;


namespace Prana.Admin.BLL
{
    public class RMAUECs : IList
    {
        ArrayList _rMAUECs = new ArrayList();

        public RMAUECs()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _rMAUECs.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add RMAUECs.this getter implementation
                return _rMAUECs[index];
            }
            set
            {
                //Add RMAUECs.this setter implementation
                _rMAUECs[index] = value;
            }
        }
        public void RemoveAt(int index)
        {
            //Add RMAUECs.RemoveAt implementation
            _rMAUECs.RemoveAt(index);
        }

        public void Insert(int index, Object rMAUECs)
        {
            //Add RMAUECs.Insert implementation
            _rMAUECs.Insert(index, (RMAUECs)rMAUECs);
        }

        public void Remove(Object rMAUECs)
        {
            //Add RMAUECs.Remove implementation
            _rMAUECs.Remove((RMAUECs)rMAUECs);
        }

        public bool Contains(object rMAUECs)
        {
            //Add RMAUECs.Contains implementation
            return _rMAUECs.Contains((RMAUECs)rMAUECs);
        }

        public void Clear()
        {
            //Add RMAUECs.Clear implementation
            _rMAUECs.Clear();
        }

        public int IndexOf(object rMAUECs)
        {
            //Add RMAUECs.IndexOf implementation
            return _rMAUECs.IndexOf((RMAUECs)rMAUECs);
        }

        public int Add(object rMAUEC)
        {
            //Add RMAUECs.Add implementation
            return _rMAUECs.Add((RMAUEC)rMAUEC);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add RMAUECs.IsFixedSize getter implementation
                return _rMAUECs.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add RMAUECs.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _rMAUECs.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _rMAUECs.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _rMAUECs.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new RMAUECsEnumerator(this));
        }

        #endregion

        #region RMAUECsEnumerator Class

        public class RMAUECsEnumerator : IEnumerator
        {
            RMAUECs _rMAUECs;
            int _location;

            public RMAUECsEnumerator(RMAUECs rMAUECs)
            {
                _rMAUECs = rMAUECs;
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
                    if ((_location < 0) || (_location >= _rMAUECs.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _rMAUECs[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _rMAUECs.Count)
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

        #region IList Members

        void System.Collections.IList.Insert(int index, object value)
        {
            // TODO:  Add RMAUECs.System.Collections.IList.Insert implementation
        }

        void System.Collections.IList.Remove(object value)
        {
            // TODO:  Add RMAUECs.System.Collections.IList.Remove implementation
        }

        #endregion

    }
}
