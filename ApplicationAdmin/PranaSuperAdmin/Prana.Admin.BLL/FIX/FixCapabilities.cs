using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// FixCapabilities is a collection class for <see cref="FixCapability"/> class.
    /// </summary>
    public class FixCapabilities : IList
    {
        ArrayList _fixCapabilities = new ArrayList();

        public FixCapabilities()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _fixCapabilities.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add FixCapabilities.this getter implementation
                return _fixCapabilities[index];
            }
            set
            {
                //Add FixCapabilities.this setter implementation
                _fixCapabilities[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add FixCapabilities.RemoveAt implementation
            _fixCapabilities.RemoveAt(index);
        }

        public void Insert(int index, Object fixCapability)
        {
            //Add FixCapabilities.Insert implementation
            _fixCapabilities.Insert(index, (FixCapability)fixCapability);
        }

        public void Remove(Object fixCapability)
        {
            //Add FixCapabilities.Remove implementation
            _fixCapabilities.Remove((FixCapability)fixCapability);
        }

        public bool Contains(object fixCapability)
        {
            //Add FixCapabilities.Contains implementation
            return _fixCapabilities.Contains((FixCapability)fixCapability);
        }

        public void Clear()
        {
            //Add FixCapabilities.Clear implementation
            _fixCapabilities.Clear();
        }

        public int IndexOf(object fixCapability)
        {
            //Add FixCapabilities.IndexOf implementation
            return _fixCapabilities.IndexOf((FixCapability)fixCapability);
        }

        public int Add(object fixCapability)
        {
            //Add FixCapabilities.Add implementation
            return _fixCapabilities.Add((FixCapability)fixCapability);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add FixCapabilities.IsFixedSize getter implementation
                return _fixCapabilities.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add FixCapabilities.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _fixCapabilities.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _fixCapabilities.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _fixCapabilities.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new FixCapabilityEnumerator(this));
        }

        #endregion

        #region FixCapabilityEnumerator Class

        public class FixCapabilityEnumerator : IEnumerator
        {
            FixCapabilities _fixCapabilities;
            int _location;

            public FixCapabilityEnumerator(FixCapabilities fixCapabilities)
            {
                _fixCapabilities = fixCapabilities;
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
                    if ((_location < 0) || (_location >= _fixCapabilities.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _fixCapabilities[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _fixCapabilities.Count)
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
