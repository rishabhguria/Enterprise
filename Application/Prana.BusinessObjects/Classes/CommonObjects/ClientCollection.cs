using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ClientCollection.
    /// </summary>
    [Serializable]
    public class ClientCollection : IList
    {
        ArrayList _clientCollection = new ArrayList();

        public ClientCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _clientCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ClientCollection.this getter implementation
                return _clientCollection[index];
            }
            set
            {
                //Add ClientCollection.this setter implementation
                _clientCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ClientCollection.RemoveAt implementation
            _clientCollection.RemoveAt(index);
        }

        public void Insert(int index, Object client)
        {
            //Add ClientCollection.Insert implementation
            _clientCollection.Insert(index, (Client)client);
        }

        public void Remove(Object client)
        {
            //Add ClientCollection.Remove implementation
            _clientCollection.Remove((Client)client);
        }

        public bool Contains(object client)
        {
            //Add ClientCollection.Contains implementation
            return _clientCollection.Contains((Client)client);
        }

        public void Clear()
        {
            //Add ClientCollection.Clear implementation
            _clientCollection.Clear();
        }

        public int IndexOf(object client)
        {
            //Add ClientCollection.IndexOf implementation
            return _clientCollection.IndexOf((Client)client);
        }

        public int Add(object client)
        {
            //Add ClientCollection.Add implementation
            return _clientCollection.Add((Client)client);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ClientCollection.IsFixedSize getter implementation
                return _clientCollection.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ClientCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _clientCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _clientCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _clientCollection.SyncRoot;
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
            ClientCollection _clientCollection;
            int _location;

            public ClientEnumerator(ClientCollection clientCollection)
            {
                _clientCollection = clientCollection;
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
                    if ((_location < 0) || (_location >= _clientCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _clientCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _clientCollection.Count)
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
