using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Units.
    /// </summary>
    public class Units : IList
    {
        ArrayList _units = new ArrayList();
        public Units()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _units.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Units.this getter implementation
                return _units[index];
            }
            set
            {
                //Add Units.this setter implementation
                _units[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Units.RemoveAt implementation
            _units.RemoveAt(index);
        }

        public void Insert(int index, Object unit)
        {
            //Add Units.Insert implementation
            _units.Insert(index, (Unit)unit);
        }

        public void Remove(Object unit)
        {
            //Add Units.Remove implementation
            _units.Remove((Unit)unit);
        }

        public bool Contains(object unit)
        {
            //Add Units.Contains implementation
            return _units.Contains((Unit)unit);
        }

        public void Clear()
        {
            //Add Units.Clear implementation
            _units.Clear();
        }

        public int IndexOf(object unit)
        {
            //Add Units.IndexOf implementation
            return _units.IndexOf((Unit)unit);
        }

        public int Add(object unit)
        {
            //Add Units.Add implementation
            return _units.Add((Unit)unit);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Units.IsFixedSize getter implementation
                return _units.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Units.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _units.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _units.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _units.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new UnitEnumerator(this));
        }

        #endregion

        #region UnitEnumerator Class

        public class UnitEnumerator : IEnumerator
        {
            Units _units;
            int _location;

            public UnitEnumerator(Units units)
            {
                _units = units;
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
                    if ((_location < 0) || (_location >= _units.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _units[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _units.Count)
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
