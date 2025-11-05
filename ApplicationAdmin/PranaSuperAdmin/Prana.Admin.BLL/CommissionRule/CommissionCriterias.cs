using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionCriterias.
    /// </summary>
    public class CommissionCriterias : IList
    {
        ArrayList _commissionCriterias = new ArrayList();
        public CommissionCriterias()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _commissionCriterias.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CommissionCriterias.this getter implementation
                return _commissionCriterias[index];
            }
            set
            {
                //Add CommissionCriterias.this setter implementation
                _commissionCriterias[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CommissionCriterias.RemoveAt implementation
            _commissionCriterias.RemoveAt(index);
        }

        public void Insert(int index, Object commissionCriteria)
        {
            //Add CommissionCriterias.Insert implementation
            _commissionCriterias.Insert(index, (CommissionCriteria)commissionCriteria);
        }

        public void Remove(Object commissionCriteria)
        {
            //Add CommissionCriterias.Remove implementation
            _commissionCriterias.Remove((CommissionCriteria)commissionCriteria);
        }

        public bool Contains(object commissionCriteria)
        {
            //Add CommissionCriterias.Contains implementation
            return _commissionCriterias.Contains((CommissionCriteria)commissionCriteria);
        }

        public void Clear()
        {
            //Add CommissionCriterias.Clear implementation
            _commissionCriterias.Clear();
        }

        public int IndexOf(object commissionCriteria)
        {
            //Add CommissionCriterias.IndexOf implementation
            return _commissionCriterias.IndexOf((CommissionCriteria)commissionCriteria);
        }

        public int Add(object commissionCriteria)
        {
            //Add CommissionCriterias.Add implementation
            return _commissionCriterias.Add((CommissionCriteria)commissionCriteria);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CommissionCriterias.IsFixedSize getter implementation
                return _commissionCriterias.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CommissionCriterias.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _commissionCriterias.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _commissionCriterias.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _commissionCriterias.SyncRoot;
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
            CommissionCriterias _commissionCriterias;
            int _location;

            public UnitEnumerator(CommissionCriterias commissionCriterias)
            {
                _commissionCriterias = commissionCriterias;
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
                    if ((_location < 0) || (_location >= _commissionCriterias.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _commissionCriterias[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _commissionCriterias.Count)
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
