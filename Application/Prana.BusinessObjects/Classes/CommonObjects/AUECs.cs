using System;
using System.Collections;
namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for AUECs.
    /// </summary>
    public class AUECs : IList
    {
        ArrayList _auecs = new ArrayList();

        public AUECs()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _auecs.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add AUECs.this getter implementation
                return _auecs[index];
            }
            set
            {
                //Add AUECs.this setter implementation
                _auecs[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add AUECs.RemoveAt implementation
            _auecs.RemoveAt(index);
        }

        public void Insert(int index, Object auec)
        {
            //Add AUECs.Insert implementation
            _auecs.Insert(index, (AUEC)auec);
        }

        public void Remove(Object auec)
        {
            //Add AUECs.Remove implementation
            _auecs.Remove((AUEC)auec);
        }

        public bool Contains(object auec)
        {
            //Add AUECs.Contains implementation
            return _auecs.Contains((AUEC)auec);
        }

        public void Clear()
        {
            //Add AUECs.Clear implementation
            _auecs.Clear();
        }

        public int IndexOf(object auec)
        {
            //Add AUECs.IndexOf implementation
            return _auecs.IndexOf((AUEC)auec);
        }

        public int Add(object auec)
        {
            //Add AUECs.Add implementation
            return _auecs.Add((AUEC)auec);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add AUECs.IsFixedSize getter implementation
                return _auecs.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add AUECs.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _auecs.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _auecs.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _auecs.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new AUECEnumerator(this));
        }

        #endregion

        #region AUECEnumerator Class

        public class AUECEnumerator : IEnumerator
        {
            AUECs _auecs;
            int _location;

            public AUECEnumerator(AUECs auecs)
            {
                _auecs = auecs;
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
                    if ((_location < 0) || (_location >= _auecs.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _auecs[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _auecs.Count)
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
