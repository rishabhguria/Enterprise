
using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AllCommissionRules.
    /// </summary>
    public class AllCommissionRules : IList
    {
        ArrayList _allCommissionRules = new ArrayList();
        public AllCommissionRules()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _allCommissionRules.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add AllCommissionRules.this getter implementation
                return _allCommissionRules[index];
            }
            set
            {
                //Add AllCommissionRules.this setter implementation
                _allCommissionRules[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add AllCommissionRules.RemoveAt implementation
            _allCommissionRules.RemoveAt(index);
        }

        public void Insert(int index, Object AllCommissionRule)
        {
            //Add AllCommissionRules.Insert implementation
            _allCommissionRules.Insert(index, (AllCommissionRule)AllCommissionRule);
        }

        public void Remove(Object AllCommissionRule)
        {
            //Add AllCommissionRules.Remove implementation
            _allCommissionRules.Remove((AllCommissionRule)AllCommissionRule);
        }

        public bool Contains(object AllCommissionRule)
        {
            //Add AllCommissionRules.Contains implementation
            return _allCommissionRules.Contains((AllCommissionRule)AllCommissionRule);
        }

        public void Clear()
        {
            //Add AllCommissionRules.Clear implementation
            _allCommissionRules.Clear();
        }

        public int IndexOf(object AllCommissionRule)
        {
            //Add AllCommissionRules.IndexOf implementation
            return _allCommissionRules.IndexOf((AllCommissionRule)AllCommissionRule);
        }

        public int Add(object AllCommissionRule)
        {
            //Add AllCommissionRules.Add implementation
            return _allCommissionRules.Add((AllCommissionRule)AllCommissionRule);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add AllCommissionRules.IsFixedSize getter implementation
                return _allCommissionRules.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add AllCommissionRules.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _allCommissionRules.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _allCommissionRules.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _allCommissionRules.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new AllCommissionRuleEnumerator(this));
        }

        #endregion

        #region AllCommissionRuleEnumerator Class

        public class AllCommissionRuleEnumerator : IEnumerator
        {
            AllCommissionRules _allCommissionRules;
            int _location;

            public AllCommissionRuleEnumerator(AllCommissionRules AllCommissionRules)
            {
                _allCommissionRules = AllCommissionRules;
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
                    if ((_location < 0) || (_location >= _allCommissionRules.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _allCommissionRules[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _allCommissionRules.Count)
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
