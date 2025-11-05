using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionRuleCriteriasUp.
    /// </summary>
    public class CommissionRuleCriteriasUp : IList
    {
        ArrayList _commissionRuleCriteriasUp = new ArrayList();
        public CommissionRuleCriteriasUp()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _commissionRuleCriteriasUp.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CommissionCriterias.this getter implementation
                return _commissionRuleCriteriasUp[index];
            }
            set
            {
                //Add CommissionCriterias.this setter implementation
                _commissionRuleCriteriasUp[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CommissionCriterias.RemoveAt implementation
            _commissionRuleCriteriasUp.RemoveAt(index);
        }

        public void Insert(int index, Object commissionRuleCriteriaUp)
        {
            //Add CommissionCriterias.Insert implementation
            _commissionRuleCriteriasUp.Insert(index, (CommissionRuleCriteriaUp)commissionRuleCriteriaUp);
        }

        public void Remove(Object commissionRuleCriteriaUp)
        {
            //Add CommissionCriterias.Remove implementation
            _commissionRuleCriteriasUp.Remove((CommissionRuleCriteriaUp)commissionRuleCriteriaUp);
        }

        public bool Contains(object commissionRuleCriteriaUp)
        {
            //Add CommissionCriterias.Contains implementation
            return _commissionRuleCriteriasUp.Contains((CommissionRuleCriteriaUp)commissionRuleCriteriaUp);
        }

        public void Clear()
        {
            //Add CommissionCriterias.Clear implementation
            _commissionRuleCriteriasUp.Clear();
        }

        public int IndexOf(object commissionRuleCriteriaUp)
        {
            //Add CommissionCriterias.IndexOf implementation
            return _commissionRuleCriteriasUp.IndexOf((CommissionRuleCriteriaUp)commissionRuleCriteriaUp);
        }

        public int Add(object commissionRuleCriteriaUp)
        {
            //Add CommissionCriterias.Add implementation
            return _commissionRuleCriteriasUp.Add((CommissionRuleCriteriaUp)commissionRuleCriteriaUp);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CommissionCriterias.IsFixedSize getter implementation
                return _commissionRuleCriteriasUp.IsFixedSize;
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
                return _commissionRuleCriteriasUp.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _commissionRuleCriteriasUp.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _commissionRuleCriteriasUp.SyncRoot;
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
            CommissionRuleCriteriasUp _commissionRuleCriteriasUp;
            int _location;

            public CommissionRuleCriteriaEnumerator(CommissionRuleCriteriasUp commissionRuleCriteriasUp)
            {
                _commissionRuleCriteriasUp = commissionRuleCriteriasUp;
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
                    if ((_location < 0) || (_location >= _commissionRuleCriteriasUp.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _commissionRuleCriteriasUp[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _commissionRuleCriteriasUp.Count)
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
