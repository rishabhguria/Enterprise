using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for BrokerCollection.
    /// </summary>
    [Serializable]
    public class BrokerCollection : IList
    {
        ArrayList _brokerCollection = new ArrayList();

        public BrokerCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _brokerCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add BrokerCollection.this getter implementation
                return _brokerCollection[index];
            }
            set
            {
                //Add BrokerCollection.this setter implementation
                _brokerCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add BrokerCollection.RemoveAt implementation
            _brokerCollection.RemoveAt(index);
        }

        public void Insert(int index, Object broker)
        {
            //Add BrokerCollection.Insert implementation
            _brokerCollection.Insert(index, (Broker)broker);
        }

        public void Remove(Object broker)
        {
            //Add BrokerCollection.Remove implementation
            _brokerCollection.Remove((Broker)broker);
        }

        public bool Contains(object broker)
        {
            //Add BrokerCollection.Contains implementation
            return _brokerCollection.Contains((Broker)broker);
        }

        public void Clear()
        {
            //Add BrokerCollection.Clear implementation
            _brokerCollection.Clear();
        }

        public int IndexOf(object broker)
        {
            //Add BrokerCollection.IndexOf implementation
            return _brokerCollection.IndexOf((Broker)broker);
        }

        public int Add(object broker)
        {
            //Add BrokerCollection.Add implementation
            return _brokerCollection.Add((Broker)broker);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add BrokerCollection.IsFixedSize getter implementation
                return _brokerCollection.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add BrokerCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _brokerCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _brokerCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _brokerCollection.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new BrokerEnumerator(this));
        }

        #endregion

        #region BrokerEnumerator Class

        public class BrokerEnumerator : IEnumerator
        {
            BrokerCollection _brokerCollection;
            int _location;

            public BrokerEnumerator(BrokerCollection brokerCollection)
            {
                _brokerCollection = brokerCollection;
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
                    if ((_location < 0) || (_location >= _brokerCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _brokerCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _brokerCollection.Count)
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
