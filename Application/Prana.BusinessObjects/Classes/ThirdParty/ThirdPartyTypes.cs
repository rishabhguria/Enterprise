using System;
using System.Collections;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ThirdPartyTypes.
    /// </summary>
    [Serializable]
    public class ThirdPartyTypes : IList
    {
        ArrayList _thirdPartyTypes = new ArrayList();
        public ThirdPartyTypes()
        {
        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _thirdPartyTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ThirdPartyTypes.this getter implementation
                return _thirdPartyTypes[index];
            }
            set
            {
                //Add ThirdPartyTypes.this setter implementation
                _thirdPartyTypes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ThirdPartyTypes.RemoveAt implementation
            _thirdPartyTypes.RemoveAt(index);
        }

        public void Insert(int index, Object thirdPartyType)
        {
            //Add ThirdPartyTypes.Insert implementation
            _thirdPartyTypes.Insert(index, (ThirdPartyType)thirdPartyType);
        }

        public void Remove(Object thirdPartyType)
        {
            //Add ThirdPartyTypes.Remove implementation
            _thirdPartyTypes.Remove((ThirdPartyType)thirdPartyType);
        }

        public bool Contains(object thirdPartyType)
        {
            //Add ThirdPartyTypes.Contains implementation
            return _thirdPartyTypes.Contains((ThirdPartyType)thirdPartyType);
        }

        public void Clear()
        {
            //Add ThirdPartyTypes.Clear implementation
            _thirdPartyTypes.Clear();
        }

        public int IndexOf(object thirdPartyType)
        {
            //Add ThirdPartyTypes.IndexOf implementation
            return _thirdPartyTypes.IndexOf((ThirdPartyType)thirdPartyType);
        }

        public int Add(object thirdPartyType)
        {
            //Add ThirdPartyTypes.Add implementation
            return _thirdPartyTypes.Add((ThirdPartyType)thirdPartyType);
        }

        public void AddAllThirdParties(List<ThirdPartyType> thirdPartyTypeList)
        {
            _thirdPartyTypes.AddRange(thirdPartyTypeList);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ThirdPartyTypes.IsFixedSize getter implementation
                return _thirdPartyTypes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ThirdPartyTypes.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _thirdPartyTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _thirdPartyTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _thirdPartyTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ThirdPartyTypeEnumerator(this));
        }

        #endregion

        #region ThirdPartyTypeEnumerator Class

        [Serializable]
        public class ThirdPartyTypeEnumerator : IEnumerator
        {
            ThirdPartyTypes _thirdPartyTypes;
            int _location;

            public ThirdPartyTypeEnumerator(ThirdPartyTypes symbols)
            {
                _thirdPartyTypes = symbols;
                _location = -1;
            }

            public ThirdPartyTypeEnumerator() { }

            #region IEnumerator Members
            public void Reset()
            {
                _location = -1;
            }
            public object Current
            {
                get
                {
                    if ((_location < 0) || (_location >= _thirdPartyTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _thirdPartyTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _thirdPartyTypes.Count)
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
