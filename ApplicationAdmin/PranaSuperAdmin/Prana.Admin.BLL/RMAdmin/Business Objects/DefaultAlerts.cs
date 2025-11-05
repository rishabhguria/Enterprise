using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for DefaultAlerts.
    /// </summary>
    public class DefaultAlerts : IList
    {
        ArrayList _defaultAlerts = new ArrayList();

        public DefaultAlerts()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _defaultAlerts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add DefaultAlerts.this getter implementation
                return _defaultAlerts[index];
            }
            set
            {
                //Add DefaultAlerts.this setter implementation
                _defaultAlerts[index] = value;
            }
        }
        public void RemoveAt(int index)
        {
            //Add DefaultAlerts.RemoveAt implementation
            _defaultAlerts.RemoveAt(index);
        }

        public void Insert(int index, Object defaultAlerts)
        {
            //Add DefaultAlerts.Insert implementation
            _defaultAlerts.Insert(index, (DefaultAlerts)defaultAlerts);
        }

        public void Remove(Object defaultAlerts)
        {
            //Add DefaultAlerts.Remove implementation
            _defaultAlerts.Remove((DefaultAlerts)defaultAlerts);
        }

        public bool Contains(object defaultAlerts)
        {
            //Add DefaultAlerts.Contains implementation
            return _defaultAlerts.Contains((DefaultAlerts)defaultAlerts);
        }

        public void Clear()
        {
            //Add DefaultAlerts.Clear implementation
            _defaultAlerts.Clear();
        }

        public int IndexOf(object defaultAlerts)
        {
            //Add DefaultAlerts.IndexOf implementation
            return _defaultAlerts.IndexOf((DefaultAlerts)defaultAlerts);
        }

        public int Add(object defaultAlerts)
        {
            //Add DefaultAlerts.Add implementation
            return _defaultAlerts.Add((DefaultAlerts)defaultAlerts);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add DefaultAlerts.IsFixedSize getter implementation
                return _defaultAlerts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add DefaultAlerts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _defaultAlerts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _defaultAlerts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _defaultAlerts.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new DefaultAlertsEnumerator(this));
        }

        #endregion

        #region DefaultAlertsEnumerator Class

        public class DefaultAlertsEnumerator : IEnumerator
        {
            DefaultAlerts _defaultAlerts;
            int _location;

            public DefaultAlertsEnumerator(DefaultAlerts defaultAlerts)
            {
                _defaultAlerts = defaultAlerts;
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
                    if ((_location < 0) || (_location >= _defaultAlerts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _defaultAlerts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _defaultAlerts.Count)
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
            // TODO:  Add DefaultAlerts.System.Collections.IList.Insert implementation
        }

        void System.Collections.IList.Remove(object value)
        {
            // TODO:  Add DefaultAlerts.System.Collections.IList.Remove implementation
        }

        #endregion
    }
}
