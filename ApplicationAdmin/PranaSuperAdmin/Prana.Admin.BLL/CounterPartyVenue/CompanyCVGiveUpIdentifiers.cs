using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyCVGiveUpIdentifiers.
    /// </summary>
    public class CompanyCVGiveUpIdentifiers : IList
    {
        ArrayList _companyCVGiveUpIdentifiers = new ArrayList();
        public CompanyCVGiveUpIdentifiers()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companyCVGiveUpIdentifiers.IsReadOnly;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CommissionCriterias.this getter implementation
                return _companyCVGiveUpIdentifiers[index];
            }
            set
            {
                //Add CommissionCriterias.this setter implementation
                _companyCVGiveUpIdentifiers[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CommissionCriterias.RemoveAt implementation
            _companyCVGiveUpIdentifiers.RemoveAt(index);
        }

        public void Insert(int index, Object companyCVGiveUpIdentifier)
        {
            //Add CommissionCriterias.Insert implementation
            _companyCVGiveUpIdentifiers.Insert(index, (CompanyCVGiveUpIdentifier)companyCVGiveUpIdentifier);
        }

        public void Remove(Object companyCVGiveUpIdentifier)
        {
            //Add CommissionCriterias.Remove implementation
            _companyCVGiveUpIdentifiers.Remove((CompanyCVGiveUpIdentifier)companyCVGiveUpIdentifier);
        }

        public bool Contains(object companyCVGiveUpIdentifier)
        {
            //Add CommissionCriterias.Contains implementation
            return _companyCVGiveUpIdentifiers.Contains((CompanyCVGiveUpIdentifier)companyCVGiveUpIdentifier);
        }

        public void Clear()
        {
            //Add CommissionCriterias.Clear implementation
            _companyCVGiveUpIdentifiers.Clear();
        }

        public int IndexOf(object companyCVGiveUpIdentifier)
        {
            //Add CommissionCriterias.IndexOf implementation
            return _companyCVGiveUpIdentifiers.IndexOf((CompanyCVGiveUpIdentifier)companyCVGiveUpIdentifier);
        }

        public int Add(object companyCVGiveUpIdentifier)
        {
            //Add CommissionCriterias.Add implementation
            return _companyCVGiveUpIdentifiers.Add((CompanyCVGiveUpIdentifier)companyCVGiveUpIdentifier);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CommissionCriterias.IsFixedSize getter implementation
                return _companyCVGiveUpIdentifiers.IsFixedSize;
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
                return _companyCVGiveUpIdentifiers.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companyCVGiveUpIdentifiers.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companyCVGiveUpIdentifiers.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CompanyCVGiveUpIdentifiersEnumerator(this));
        }

        #endregion

        #region CompanyCVGiveUpIdentifiersEnumerator Class

        public class CompanyCVGiveUpIdentifiersEnumerator : IEnumerator
        {
            CompanyCVGiveUpIdentifiers _companyCVGiveUpIdentifiers;
            int _location;

            public CompanyCVGiveUpIdentifiersEnumerator(CompanyCVGiveUpIdentifiers companyCVGiveUpIdentifiers)
            {
                _companyCVGiveUpIdentifiers = companyCVGiveUpIdentifiers;
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
                    if ((_location < 0) || (_location >= _companyCVGiveUpIdentifiers.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companyCVGiveUpIdentifiers[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companyCVGiveUpIdentifiers.Count)
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
