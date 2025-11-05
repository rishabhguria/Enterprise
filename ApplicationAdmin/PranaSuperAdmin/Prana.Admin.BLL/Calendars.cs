using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    public class Calendars : IList
    {

        ArrayList _calendars = new ArrayList();

        public Calendars()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _calendars.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Holidays.this getter implementation
                return _calendars[index];
            }
            set
            {
                //Add Holidays.this setter implementation
                _calendars[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Holidays.RemoveAt implementation
            _calendars.RemoveAt(index);
        }

        public void Insert(int index, Object calendar)
        {
            //Add Holidays.Insert implementation
            _calendars.Insert(index, (Calendar)calendar);
        }

        public void Remove(Object calendar)
        {
            //Add Holidays.Remove implementation
            _calendars.Remove((Calendar)calendar);
        }

        public bool Contains(object calendar)
        {
            //Add Holidays.Contains implementation
            return _calendars.Contains((Calendar)calendar);
        }

        public void Clear()
        {
            //Add Holidays.Clear implementation
            _calendars.Clear();
        }

        public int IndexOf(object calendar)
        {
            //Add Holidays.IndexOf implementation
            return _calendars.IndexOf((Calendar)calendar);
        }

        public int Add(object calendar)
        {
            //Add Holidays.Add implementation
            return _calendars.Add((Calendar)calendar);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Holidays.IsFixedSize getter implementation
                return _calendars.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Holidays.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _calendars.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _calendars.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _calendars.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CalendarEnumerator(this));
        }

        #endregion

        #region CalendarEnumerator Class

        public class CalendarEnumerator : IEnumerator
        {
            Calendars _calendars;
            int _location;

            public CalendarEnumerator(Calendars calendars)
            {
                _calendars = calendars;
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
                    if ((_location < 0) || (_location >= _calendars.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _calendars[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _calendars.Count)
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
