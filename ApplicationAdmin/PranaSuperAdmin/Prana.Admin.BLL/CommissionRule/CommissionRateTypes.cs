using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionRateTypes.
    /// </summary>
    public class CommissionRateTypes : IList
    {
        ArrayList _commissionRateTypes = new ArrayList();
        public CommissionRateTypes()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _commissionRateTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CommissionRateTypes.this getter implementation
                return _commissionRateTypes[index];
            }
            set
            {
                //Add CommissionRateTypes.this setter implementation
                _commissionRateTypes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CommissionRateTypes.RemoveAt implementation
            _commissionRateTypes.RemoveAt(index);
        }

        public void Insert(int index, Object commissionRateType)
        {
            //Add CommissionRateTypes.Insert implementation
            _commissionRateTypes.Insert(index, (CommissionRateType)commissionRateType);
        }

        public void Remove(Object commissionRateType)
        {
            //Add CommissionRateTypes.Remove implementation
            _commissionRateTypes.Remove((CommissionRateType)commissionRateType);
        }

        public bool Contains(object commissionRateType)
        {
            //Add CommissionRateTypes.Contains implementation
            return _commissionRateTypes.Contains((CommissionRateType)commissionRateType);
        }

        public void Clear()
        {
            //Add CommissionRateTypes.Clear implementation
            _commissionRateTypes.Clear();
        }

        public int IndexOf(object commissionRateType)
        {
            //Add CommissionRateTypes.IndexOf implementation
            return _commissionRateTypes.IndexOf((CommissionRateType)commissionRateType);
        }

        public int Add(object commissionRateType)
        {
            //Add CommissionRateTypes.Add implementation
            return _commissionRateTypes.Add((CommissionRateType)commissionRateType);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CommissionRateTypes.IsFixedSize getter implementation
                return _commissionRateTypes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CommissionRateTypes.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _commissionRateTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _commissionRateTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _commissionRateTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CommissionRateTypeEnumerator(this));
        }

        #endregion

        #region CommissionRateTypeEnumerator Class

        public class CommissionRateTypeEnumerator : IEnumerator
        {
            CommissionRateTypes _commissionRateTypes;
            int _location;

            public CommissionRateTypeEnumerator(CommissionRateTypes commissionRateTypes)
            {
                _commissionRateTypes = commissionRateTypes;
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
                    if ((_location < 0) || (_location >= _commissionRateTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _commissionRateTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _commissionRateTypes.Count)
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
