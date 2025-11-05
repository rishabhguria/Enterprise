using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ClientOverallLimits.
    /// </summary>
    public class ClientOverallLimits : IList
    {
        ArrayList _clientOverallLimits = new ArrayList();

        public ClientOverallLimits()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _clientOverallLimits.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                return _clientOverallLimits[index];
            }
            set
            {
                //Add Clients.this setter implementation
                _clientOverallLimits[index] = value;
            }
        }
        public void RemoveAt(int index)
        {
            //Add Clientss.RemoveAt implementation
            _clientOverallLimits.RemoveAt(index);
        }

        public void Insert(int index, Object clientOverallLimits)
        {
            //Add Clients.Insert implementation
            _clientOverallLimits.Insert(index, (ClientOverallLimits)clientOverallLimits);
        }

        public void Remove(Object clientOverallLimits)
        {
            //Add Clients.Remove implementation
            _clientOverallLimits.Remove((ClientOverallLimits)clientOverallLimits);
        }

        public bool Contains(object clientOverallLimits)
        {
            //Add Clients.Contains implementation
            return _clientOverallLimits.Contains((ClientOverallLimits)clientOverallLimits);
        }

        public void Clear()
        {
            //Add Clients.Clear implementation
            _clientOverallLimits.Clear();
        }

        public int IndexOf(object clientOverallLimits)
        {
            //Add Clients.IndexOf implementation
            return _clientOverallLimits.IndexOf((ClientOverallLimits)clientOverallLimits);
        }

        public int Add(object clientOverallLimit)
        {
            //Add Clients.Add implementation
            return _clientOverallLimits.Add((ClientOverallLimit)clientOverallLimit);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Clients.IsFixedSize getter implementation
                return _clientOverallLimits.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Clients.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _clientOverallLimits.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _clientOverallLimits.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _clientOverallLimits.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ClientOverallLimitsEnumerator(this));
        }

        #endregion

        #region ClientOverallLimitsEnumerator Class

        public class ClientOverallLimitsEnumerator : IEnumerator
        {
            ClientOverallLimits _clientOverallLimits;
            int _location;

            public ClientOverallLimitsEnumerator(ClientOverallLimits clientOverallLimits)
            {
                _clientOverallLimits = clientOverallLimits;
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
                    if ((_location < 0) || (_location >= _clientOverallLimits.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _clientOverallLimits[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _clientOverallLimits.Count)
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
