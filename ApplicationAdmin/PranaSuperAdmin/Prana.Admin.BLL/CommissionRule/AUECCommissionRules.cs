using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AUECCommissionRules.
    /// </summary>
    public class AUECCommissionRules : IList
    {
        ArrayList _aueccommissionrules = new ArrayList();
        public AUECCommissionRules()
        {
        }


        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _aueccommissionrules.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add AUECCommissionRules.this getter implementation
                return _aueccommissionrules[index];
            }
            set
            {
                //Add AUECCommissionRules.this setter implementation
                _aueccommissionrules[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add AUECCommissionRules.RemoveAt implementation
            _aueccommissionrules.RemoveAt(index);
        }

        public void Insert(int index, Object aueccommissionrule)
        {
            //Add AUECCommissionRules.Insert implementation
            _aueccommissionrules.Insert(index, (AUECCommissionRule)aueccommissionrule);
        }

        public void Remove(Object aueccommissionrule)
        {
            //Add AUECCommissionRules.Remove implementation
            _aueccommissionrules.Remove((AUECCommissionRule)aueccommissionrule);
        }

        public bool Contains(object aueccommissionrule)
        {
            //Add AUECCommissionRules.Contains implementation
            return _aueccommissionrules.Contains((AUECCommissionRule)aueccommissionrule);
        }

        public void Clear()
        {
            //Add AUECCommissionRules.Clear implementation
            _aueccommissionrules.Clear();
        }

        public int IndexOf(object aueccommissionrule)
        {
            //Add AUECCommissionRules.IndexOf implementation
            return _aueccommissionrules.IndexOf((AUECCommissionRule)aueccommissionrule);
        }

        public int Add(object aueccommissionrule)
        {
            //Add AUECCommissionRules.Add implementation
            return _aueccommissionrules.Add((AUECCommissionRule)aueccommissionrule);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add AUECCommissionRules.IsFixedSize getter implementation
                return _aueccommissionrules.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add AUECCommissionRules.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _aueccommissionrules.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _aueccommissionrules.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _aueccommissionrules.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new AUECCommissionRuleEnumerator(this));
        }

        #endregion

        #region AUECCommissionRuleEnumerator Class

        public class AUECCommissionRuleEnumerator : IEnumerator
        {
            AUECCommissionRules _aueccommissionrules;
            int _location;

            public AUECCommissionRuleEnumerator(AUECCommissionRules aueccommissionrules)
            {
                _aueccommissionrules = aueccommissionrules;
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
                    if ((_location < 0) || (_location >= _aueccommissionrules.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _aueccommissionrules[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _aueccommissionrules.Count)
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
