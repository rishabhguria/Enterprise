using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionRuleCriterias.
    /// </summary>
    public class CommissionRuleCriterias : IList
    {
        ArrayList _commissionRuleCriterias = new ArrayList();
        public CommissionRuleCriterias()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _commissionRuleCriterias.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CommissionCriterias.this getter implementation
                return _commissionRuleCriterias[index];
            }
            set
            {
                //Add CommissionCriterias.this setter implementation
                _commissionRuleCriterias[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CommissionCriterias.RemoveAt implementation
            _commissionRuleCriterias.RemoveAt(index);
        }

        public void Insert(int index, Object commissionRuleCriteria)
        {
            //Add CommissionCriterias.Insert implementation
            _commissionRuleCriterias.Insert(index, (CommissionRuleCriteriaold)commissionRuleCriteria);
        }

        public void Remove(Object commissionRuleCriteria)
        {
            //Add CommissionCriterias.Remove implementation
            _commissionRuleCriterias.Remove((CommissionRuleCriteriaold)commissionRuleCriteria);
        }

        public bool Contains(object commissionRuleCriteria)
        {
            //Add CommissionCriterias.Contains implementation
            return _commissionRuleCriterias.Contains((CommissionRuleCriteriaold)commissionRuleCriteria);
        }

        public void Clear()
        {
            //Add CommissionCriterias.Clear implementation
            _commissionRuleCriterias.Clear();
        }

        public int IndexOf(object commissionRuleCriteria)
        {
            //Add CommissionCriterias.IndexOf implementation
            return _commissionRuleCriterias.IndexOf((CommissionRuleCriteriaold)commissionRuleCriteria);
        }

        public int Add(object commissionRuleCriteria)
        {
            //Add CommissionCriterias.Add implementation
            return _commissionRuleCriterias.Add((CommissionRuleCriteriaold)commissionRuleCriteria);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CommissionCriterias.IsFixedSize getter implementation
                return _commissionRuleCriterias.IsFixedSize;
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
                return _commissionRuleCriterias.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _commissionRuleCriterias.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _commissionRuleCriterias.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CommissionRuleCriteriaEnumerator(this));
        }

        #endregion

        #region CommissionRuleCriteriaEnumerator Class

        public class CommissionRuleCriteriaEnumerator : IEnumerator
        {
            CommissionRuleCriterias _commissionRuleCriterias;
            int _location;

            public CommissionRuleCriteriaEnumerator(CommissionRuleCriterias commissionRuleCriterias)
            {
                _commissionRuleCriterias = commissionRuleCriterias;
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
                    if ((_location < 0) || (_location >= _commissionRuleCriterias.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _commissionRuleCriterias[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _commissionRuleCriterias.Count)
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
