using System;
using System.Collections;
using System.Linq;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for TimeInForces.
    /// </summary>
    public class TimeInForces : IList
    {
        ArrayList _timeInForces = new ArrayList();

        public TimeInForces()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _timeInForces.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add TimeInForces.this getter implementation
                return _timeInForces[index];
            }
            set
            {
                //Add TimeInForces.this setter implementation
                _timeInForces[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add TimeInForces.RemoveAt implementation
            _timeInForces.RemoveAt(index);
        }

        public void Insert(int index, Object timeInForce)
        {
            //Add TimeInForces.Insert implementation
            _timeInForces.Insert(index, (TimeInForce)timeInForce);
        }

        public void Remove(Object timeInForce)
        {
            //Add TimeInForces.Remove implementation
            _timeInForces.Remove((TimeInForce)timeInForce);
        }

        public bool Contains(object timeInForce)
        {
            //Add TimeInForces.Contains implementation
            return _timeInForces.Contains((TimeInForce)timeInForce);
        }

        public bool Contains(int timeInForceID)
        {
            return _timeInForces.Cast<TimeInForce>().Any(timeInForce => timeInForce.TimeInForceID == timeInForceID);
        }

        public void Clear()
        {
            //Add TimeInForces.Clear implementation
            _timeInForces.Clear();
        }

        public int IndexOf(object timeInForce)
        {
            //Add TimeInForces.IndexOf implementation
            return _timeInForces.IndexOf((TimeInForce)timeInForce);
        }

        public int Add(object timeInForce)
        {
            //Add TimeInForces.Add implementation
            return _timeInForces.Add((TimeInForce)timeInForce);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add TimeInForces.IsFixedSize getter implementation
                return _timeInForces.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add TimeInForces.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _timeInForces.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _timeInForces.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _timeInForces.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new TimeInForceEnumerator(this));
        }

        #endregion

        #region AssetEnumerator Class

        public class TimeInForceEnumerator : IEnumerator
        {
            TimeInForces _timeInForces;
            int _location;

            public TimeInForceEnumerator(TimeInForces timeInForces)
            {
                _timeInForces = timeInForces;
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
                    if ((_location < 0) || (_location >= _timeInForces.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _timeInForces[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _timeInForces.Count)
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
