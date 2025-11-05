using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Operators.
    /// </summary>
    public class Operaters : IList
    {
        ArrayList _operaters = new ArrayList();
        public Operaters()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _operaters.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Operaters.this getter implementation
                return _operaters[index];
            }
            set
            {
                //Add Operaters.this setter implementation
                _operaters[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Operaters.RemoveAt implementation
            _operaters.RemoveAt(index);
        }

        public void Insert(int index, Object operater)
        {
            //Add Operaters.Insert implementation
            _operaters.Insert(index, (Operater)operater);
        }

        public void Remove(Object operater)
        {
            //Add Operaters.Remove implementation
            _operaters.Remove((Operater)operater);
        }

        public bool Contains(object operater)
        {
            //Add Operaters.Contains implementation
            return _operaters.Contains((Operater)operater);
        }

        public void Clear()
        {
            //Add Operaters.Clear implementation
            _operaters.Clear();
        }

        public int IndexOf(object operater)
        {
            //Add Operaters.IndexOf implementation
            return _operaters.IndexOf((Operater)operater);
        }

        public int Add(object operater)
        {
            //Add Operaters.Add implementation
            return _operaters.Add((Operater)operater);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Operaters.IsFixedSize getter implementation
                return _operaters.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Operaters.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _operaters.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _operaters.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _operaters.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new OperaterEnumerator(this));
        }

        #endregion

        #region OperaterEnumerator Class

        public class OperaterEnumerator : IEnumerator
        {
            Operaters _operaters;
            int _location;

            public OperaterEnumerator(Operaters operaters)
            {
                _operaters = operaters;
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
                    if ((_location < 0) || (_location >= _operaters.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _operaters[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _operaters.Count)
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
