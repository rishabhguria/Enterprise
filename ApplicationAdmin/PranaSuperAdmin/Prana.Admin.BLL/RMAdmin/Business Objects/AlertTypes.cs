using System;
using System.Collections;


namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AlertTypes.
    /// </summary>
    public class AlertTypes : IList
    {
        ArrayList _alertTypes = new ArrayList();

        public AlertTypes()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _alertTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Alertss.this getter implementation
                return _alertTypes[index];
            }
            set
            {
                //Add Alerts.this setter implementation
                _alertTypes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Alerts.RemoveAt implementation
            _alertTypes.RemoveAt(index);
        }

        public void Insert(int index, Object alertType)
        {
            //Add Alerts.Insert implementation
            _alertTypes.Insert(index, (AlertType)alertType);
        }

        public void Remove(Object alertType)
        {
            //Add Alerts.Remove implementation
            _alertTypes.Remove((AlertType)alertType);
        }

        public bool Contains(object alertType)
        {
            //Add Alerts.Contains implementation
            return _alertTypes.Contains((AlertType)alertType);
        }

        public void Clear()
        {
            //Add Alerts.Clear implementation
            _alertTypes.Clear();
        }

        public int IndexOf(object alertType)
        {
            //Add Alerts.IndexOf implementation
            return _alertTypes.IndexOf((AlertType)alertType);
        }

        public int Add(object alertType)
        {
            //Add Alerts.Add implementation
            return _alertTypes.Add((AlertType)alertType);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Alerts.IsFixedSize getter implementation
                return _alertTypes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Alerts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _alertTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _alertTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _alertTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new AlertTypeEnumerator(this));
        }

        #endregion

        #region AlertTypeEnumerator Class

        public class AlertTypeEnumerator : IEnumerator
        {
            AlertTypes _alertTypes;
            int _location;

            public AlertTypeEnumerator(AlertTypes alertTypes)
            {
                _alertTypes = alertTypes;
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
                    if ((_location < 0) || (_location >= _alertTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _alertTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _alertTypes.Count)
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
