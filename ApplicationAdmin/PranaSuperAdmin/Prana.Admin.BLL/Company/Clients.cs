using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Clients.
    /// </summary>
    public class Clients : IList
    {
        ArrayList _clients = new ArrayList();

        public Clients()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _clients.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Clients.this getter implementation
                return _clients[index];
            }
            set
            {
                //Add Clients.this setter implementation
                _clients[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Clients.RemoveAt implementation
            _clients.RemoveAt(index);
        }

        public void Insert(int index, Object client)
        {
            //Add Clients.Insert implementation
            _clients.Insert(index, (Client)client);
        }

        public void Remove(Object client)
        {
            //Add Clients.Remove implementation
            _clients.Remove((Client)client);
        }

        public bool Contains(object client)
        {
            //Add Clients.Contains implementation
            return _clients.Contains((Client)client);
        }

        public void Clear()
        {
            //Add Clients.Clear implementation
            _clients.Clear();
        }

        public int IndexOf(object client)
        {
            //Add Clients.IndexOf implementation
            return _clients.IndexOf((Client)client);
        }

        public int Add(object client)
        {
            //Add Clients.Add implementation
            return _clients.Add((Client)client);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Clients.IsFixedSize getter implementation
                return _clients.IsFixedSize;
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
                return _clients.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _clients.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _clients.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ClientEnumerator(this));
        }

        #endregion

        #region ClientEnumerator Class

        public class ClientEnumerator : IEnumerator
        {
            Clients _clients;
            int _location;

            public ClientEnumerator(Clients clients)
            {
                _clients = clients;
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
                    if ((_location < 0) || (_location >= _clients.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _clients[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _clients.Count)
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
