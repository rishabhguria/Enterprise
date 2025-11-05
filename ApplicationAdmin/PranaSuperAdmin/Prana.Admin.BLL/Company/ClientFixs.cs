using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ClientFixs.
    /// </summary>
    public class ClientFixs : IList
    {
        public ClientFixs()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        ArrayList _clientFixs = new ArrayList();


        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _clientFixs.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Clients.this getter implementation
                return _clientFixs[index];
            }
            set
            {
                //Add Clients.this setter implementation
                _clientFixs[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Clients.RemoveAt implementation
            _clientFixs.RemoveAt(index);
        }

        public void Insert(int index, Object clientFix)
        {
            //Add Clients.Insert implementation
            _clientFixs.Insert(index, (ClientFix)clientFix);
        }

        public void Remove(Object clientFix)
        {
            //Add Clients.Remove implementation
            _clientFixs.Remove((ClientFix)clientFix);
        }

        public bool Contains(object clientFix)
        {
            //Add Clients.Contains implementation
            return _clientFixs.Contains((ClientFix)clientFix);
        }

        public void Clear()
        {
            //Add Clients.Clear implementation
            _clientFixs.Clear();
        }

        public int IndexOf(object clientFix)
        {
            //Add Clients.IndexOf implementation
            return _clientFixs.IndexOf((ClientFix)clientFix);
        }

        public int Add(object clientFix)
        {
            //Add Clients.Add implementation
            return _clientFixs.Add((ClientFix)clientFix);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Clients.IsFixedSize getter implementation
                return _clientFixs.IsFixedSize;
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
                return _clientFixs.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _clientFixs.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _clientFixs.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ClientFixEnumerator(this));
        }

        #endregion

        #region ClientEnumerator Class

        public class ClientFixEnumerator : IEnumerator
        {
            ClientFixs _clientfixs;
            int _location;

            public ClientFixEnumerator(ClientFixs clientFixs)
            {
                _clientfixs = clientFixs;
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
                    if ((_location < 0) || (_location >= _clientfixs.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _clientfixs[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _clientfixs.Count)
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
