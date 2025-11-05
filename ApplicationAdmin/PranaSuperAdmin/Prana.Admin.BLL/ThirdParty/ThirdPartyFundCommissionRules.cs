using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ThirdPartyAccountCommissionRules.
    /// </summary>
    public class ThirdPartyAccountCommissionRules : IList
    {
        ArrayList _thirdPartyAccountCommissionRules = new ArrayList();
        public ThirdPartyAccountCommissionRules()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _thirdPartyAccountCommissionRules.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ThirdPartyAccountCommissionRules.this getter implementation
                return _thirdPartyAccountCommissionRules[index];
            }
            set
            {
                //Add ThirdPartyAccountCommissionRules.this setter implementation
                _thirdPartyAccountCommissionRules[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ThirdPartyAccountCommissionRules.RemoveAt implementation
            _thirdPartyAccountCommissionRules.RemoveAt(index);
        }

        public void Insert(int index, Object thirdPartyAccountCommissionRule)
        {
            //Add ThirdPartyAccountCommissionRules.Insert implementation
            _thirdPartyAccountCommissionRules.Insert(index, (ThirdPartyAccountCommissionRule)thirdPartyAccountCommissionRule);
        }

        public void Remove(Object thirdPartyAccountCommissionRule)
        {
            //Add ThirdPartyAccountCommissionRules.Remove implementation
            _thirdPartyAccountCommissionRules.Remove((ThirdPartyAccountCommissionRule)thirdPartyAccountCommissionRule);
        }

        public bool Contains(object thirdPartyAccountCommissionRule)
        {
            //Add ThirdPartyAccountCommissionRules.Contains implementation
            return _thirdPartyAccountCommissionRules.Contains((ThirdPartyAccountCommissionRule)thirdPartyAccountCommissionRule);
        }

        public void Clear()
        {
            //Add ThirdPartyAccountCommissionRules.Clear implementation
            _thirdPartyAccountCommissionRules.Clear();
        }

        public int IndexOf(object thirdPartyAccountCommissionRule)
        {
            //Add ThirdPartyAccountCommissionRules.IndexOf implementation
            return _thirdPartyAccountCommissionRules.IndexOf((ThirdPartyAccountCommissionRule)thirdPartyAccountCommissionRule);
        }

        public int Add(object thirdPartyAccountCommissionRule)
        {
            //Add ThirdPartyAccountCommissionRules.Add implementation
            return _thirdPartyAccountCommissionRules.Add((ThirdPartyAccountCommissionRule)thirdPartyAccountCommissionRule);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ThirdPartyAccountCommissionRules.IsFixedSize getter implementation
                return _thirdPartyAccountCommissionRules.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ThirdPartyAccountCommissionRules.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _thirdPartyAccountCommissionRules.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _thirdPartyAccountCommissionRules.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _thirdPartyAccountCommissionRules.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ThirdPartyAccountCommissionRuleEnumerator(this));
        }

        #endregion

        #region ThirdPartyAccountCommissionRuleEnumerator Class

        public class ThirdPartyAccountCommissionRuleEnumerator : IEnumerator
        {
            ThirdPartyAccountCommissionRules _thirdPartyAccountCommissionRules;
            int _location;

            public ThirdPartyAccountCommissionRuleEnumerator(ThirdPartyAccountCommissionRules thirdPartyAccountCommissionRules)
            {
                _thirdPartyAccountCommissionRules = thirdPartyAccountCommissionRules;
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
                    if ((_location < 0) || (_location >= _thirdPartyAccountCommissionRules.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _thirdPartyAccountCommissionRules[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _thirdPartyAccountCommissionRules.Count)
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
