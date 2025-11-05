using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    public class EnumerationValueList : IList, ICloneable
    {
        ArrayList _enumerationValues = new ArrayList();

        public EnumerationValueList()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _enumerationValues.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add enumerationValues.this getter implementation
                return _enumerationValues[index];
            }
            set
            {
                //Add enumerationValues.this setter implementation
                _enumerationValues[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add enumerationValues.RemoveAt implementation
            _enumerationValues.RemoveAt(index);
        }

        public void Insert(int index, Object enumerationValue)
        {
            //Add enumerationValues.Insert implementation
            _enumerationValues.Insert(index, (EnumerationValue)enumerationValue);
        }

        public void Remove(Object enumerationValue)
        {
            //Add enumerationValues.Remove implementation
            _enumerationValues.Remove((EnumerationValue)enumerationValue);
        }

        public bool Contains(object enumerationValue)
        {
            //Add enumerationValues.Contains implementation
            return _enumerationValues.Contains((EnumerationValue)enumerationValue);
        }

        public void Clear()
        {
            //Add enumerationValues.Clear implementation
            _enumerationValues.Clear();
        }

        public int IndexOf(object enumerationValue)
        {
            //Add enumerationValues.IndexOf implementation
            return _enumerationValues.IndexOf((EnumerationValue)enumerationValue);
        }

        public int Add(object enumerationValue)
        {
            //Add enumerationValues.Add implementation
            return _enumerationValues.Add((EnumerationValue)enumerationValue);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add enumerationValues.IsFixedSize getter implementation
                return _enumerationValues.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add enumerationValues.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _enumerationValues.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _enumerationValues.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _enumerationValues.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new EnumerationValueEnumerator(this));
        }

        #endregion

        #region ClientEnumerator Class

        public class EnumerationValueEnumerator : IEnumerator
        {
            EnumerationValueList _enumerationValues;
            int _location;

            public EnumerationValueEnumerator(EnumerationValueList enumerationValues)
            {
                _enumerationValues = enumerationValues;
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
                    if ((_location < 0) || (_location >= _enumerationValues.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _enumerationValues[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _enumerationValues.Count)
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

        public object Clone()
        {
            EnumerationValueList enumValList = new EnumerationValueList();
            foreach (Object obj in _enumerationValues)
            {
                enumValList.Add((EnumerationValue)obj);
            }
            return enumValList;
        }
    }
}
