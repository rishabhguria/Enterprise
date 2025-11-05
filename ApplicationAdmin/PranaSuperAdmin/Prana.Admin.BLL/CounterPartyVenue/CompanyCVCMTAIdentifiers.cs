using System;
using System.Collections;


namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyCVCMTAIdentifiers.
    /// </summary>
    public class CompanyCVCMTAIdentifiers : IList
    {
        ArrayList _companyCVCMTAIdentifiers = new ArrayList();
        public CompanyCVCMTAIdentifiers()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companyCVCMTAIdentifiers.IsReadOnly;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CompanyCVCMTAIdentifiers.this getter implementation
                return _companyCVCMTAIdentifiers[index];
            }
            set
            {
                //Add CompanyCVCMTAIdentifiers.this setter implementation
                _companyCVCMTAIdentifiers[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CompanyCVCMTAIdentifiers.RemoveAt implementation
            _companyCVCMTAIdentifiers.RemoveAt(index);
        }

        public void Insert(int index, Object companyCVCMTAIdentifier)
        {
            //Add CompanyCVCMTAIdentifiers.Insert implementation
            _companyCVCMTAIdentifiers.Insert(index, (CompanyCVCMTAIdentifier)companyCVCMTAIdentifier);
        }

        public void Remove(Object companyCVCMTAIdentifier)
        {
            //Add CompanyCVCMTAIdentifiers.Remove implementation
            _companyCVCMTAIdentifiers.Remove((CompanyCVCMTAIdentifier)companyCVCMTAIdentifier);
        }

        public bool Contains(object companyCVCMTAIdentifier)
        {
            //Add CompanyCVCMTAIdentifiers.Contains implementation
            return _companyCVCMTAIdentifiers.Contains((CompanyCVCMTAIdentifier)companyCVCMTAIdentifier);
        }

        public void Clear()
        {
            //Add CompanyCVCMTAIdentifiers.Clear implementation
            _companyCVCMTAIdentifiers.Clear();
        }

        public int IndexOf(object companyCVCMTAIdentifier)
        {
            //Add CompanyCVCMTAIdentifiers.IndexOf implementation
            return _companyCVCMTAIdentifiers.IndexOf((CompanyCVCMTAIdentifier)companyCVCMTAIdentifier);
        }

        public int Add(object companyCVCMTAIdentifier)
        {
            //Add CompanyCVCMTAIdentifiers.Add implementation
            return _companyCVCMTAIdentifiers.Add((CompanyCVCMTAIdentifier)companyCVCMTAIdentifier);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CompanyCVCMTAIdentifiers.IsFixedSize getter implementation
                return _companyCVCMTAIdentifiers.IsFixedSize;
            }
        }


        #endregion
        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CompanyCVCMTAIdentifiers.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _companyCVCMTAIdentifiers.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companyCVCMTAIdentifiers.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companyCVCMTAIdentifiers.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CompanyCVCMTAIdentifiersEnumerator(this));
        }

        #endregion

        #region CompanyCVCMTAIdentifiersEnumerator Class

        public class CompanyCVCMTAIdentifiersEnumerator : IEnumerator
        {
            CompanyCVCMTAIdentifiers _companyCVCMTAIdentifiers;
            int _location;

            public CompanyCVCMTAIdentifiersEnumerator(CompanyCVCMTAIdentifiers companyCVCMTAIdentifiers)
            {
                _companyCVCMTAIdentifiers = companyCVCMTAIdentifiers;
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
                    if ((_location < 0) || (_location >= _companyCVCMTAIdentifiers.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companyCVCMTAIdentifiers[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companyCVCMTAIdentifiers.Count)
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
