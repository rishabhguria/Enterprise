using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Rules.
    /// </summary>
    public class Rules : IList
    {
        ArrayList _rules = new ArrayList();
        public Rules()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _rules.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Rules.this getter implementation
                return _rules[index];
            }
            set
            {
                //Add Rules.this setter implementation
                _rules[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Rules.RemoveAt implementation
            _rules.RemoveAt(index);
        }

        public void Insert(int index, Object rule)
        {
            //Add Rules.Insert implementation
            _rules.Insert(index, (Rule)rule);
        }

        public void Remove(Object rule)
        {
            //Add Rules.Remove implementation
            _rules.Remove((Rule)rule);
        }

        public bool Contains(object rule)
        {
            //Add Rules.Contains implementation
            return _rules.Contains((Rule)rule);
        }

        public void Clear()
        {
            //Add Rules.Clear implementation
            _rules.Clear();
        }

        public int IndexOf(object rule)
        {
            //Add Rules.IndexOf implementation
            return _rules.IndexOf((Rule)rule);
        }

        public int Add(object rule)
        {
            //Add Rules.Add implementation
            return _rules.Add((Rule)rule);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Rules.IsFixedSize getter implementation
                return _rules.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Rules.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _rules.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _rules.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _rules.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new RuleEnumerator(this));
        }

        #endregion

        #region RuleEnumerator Class

        public class RuleEnumerator : IEnumerator
        {
            Rules _rules;
            int _location;

            public RuleEnumerator(Rules rules)
            {
                _rules = rules;
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
                    if ((_location < 0) || (_location >= _rules.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _rules[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _rules.Count)
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
