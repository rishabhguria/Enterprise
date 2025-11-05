using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ContractListingTypes.
    /// </summary>
    public class ContractListingTypes : IList
    {
        ArrayList _contractListingTypes = new ArrayList();
        public ContractListingTypes()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _contractListingTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ContractListingTypes.this getter implementation
                return _contractListingTypes[index];
            }
            set
            {
                //Add ContractListingTypes.this setter implementation
                _contractListingTypes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ContractListingTypes.RemoveAt implementation
            _contractListingTypes.RemoveAt(index);
        }

        public void Insert(int index, Object contractListingType)
        {
            //Add ContractListingTypes.Insert implementation
            _contractListingTypes.Insert(index, (ContractListingType)contractListingType);
        }

        public void Remove(Object contractListingType)
        {
            //Add ContractListingTypes.Remove implementation
            _contractListingTypes.Remove((ContractListingType)contractListingType);
        }

        public bool Contains(object contractListingType)
        {
            //Add ContractListingTypes.Contains implementation
            return _contractListingTypes.Contains((ContractListingType)contractListingType);
        }

        public void Clear()
        {
            //Add ContractListingTypes.Clear implementation
            _contractListingTypes.Clear();
        }

        public int IndexOf(object contractListingType)
        {
            //Add ContractListingTypes.IndexOf implementation
            return _contractListingTypes.IndexOf((ContractListingType)contractListingType);
        }

        public int Add(object contractListingType)
        {
            //Add ContractListingTypes.Add implementation
            return _contractListingTypes.Add((ContractListingType)contractListingType);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ContractListingTypes.IsFixedSize getter implementation
                return _contractListingTypes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ContractListingTypes.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _contractListingTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _contractListingTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _contractListingTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ContractListingTypeEnumerator(this));
        }

        #endregion

        #region ContractListingTypeEnumerator Class

        public class ContractListingTypeEnumerator : IEnumerator
        {
            ContractListingTypes _contractListingTypes;
            int _location;

            public ContractListingTypeEnumerator(ContractListingTypes contractListingTypes)
            {
                _contractListingTypes = contractListingTypes;
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
                    if ((_location < 0) || (_location >= _contractListingTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _contractListingTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _contractListingTypes.Count)
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
