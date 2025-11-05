using System;
using System.Collections;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ThirdParties.
    /// </summary>
    [Serializable]
    public class ThirdParties : IList
    {
        ArrayList _thirdParties = new ArrayList();

        public ThirdParties()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _thirdParties.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ThirdParties.this getter implementation
                return _thirdParties[index];
            }
            set
            {
                //Add ThirdParties.this setter implementation
                _thirdParties[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ThirdParties.RemoveAt implementation
            _thirdParties.RemoveAt(index);
        }

        public void Insert(int index, Object thirdParty)
        {
            //Add ThirdParties.Insert implementation
            _thirdParties.Insert(index, (ThirdParty)thirdParty);
        }

        public void Remove(Object thirdParty)
        {
            //Add ThirdParties.Remove implementation
            _thirdParties.Remove((ThirdParty)thirdParty);
        }

        public bool Contains(object thirdParty)
        {
            //Add ThirdParties.Contains implementation
            return _thirdParties.Contains((ThirdParty)thirdParty);
        }

        public void Clear()
        {
            //Add ThirdParties.Clear implementation
            _thirdParties.Clear();
        }

        public int IndexOf(object thirdParty)
        {
            //Add ThirdParties.IndexOf implementation
            return _thirdParties.IndexOf((ThirdParty)thirdParty);
        }

        public int Add(object thirdParty)
        {
            //Add ThirdParties.Add implementation
            return _thirdParties.Add((ThirdParty)thirdParty);
        }

        public void AddRange(List<ThirdParty> thirdParties)
        {
            _thirdParties.AddRange(thirdParties);   
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ThirdParties.IsFixedSize getter implementation
                return _thirdParties.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ThirdParties.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _thirdParties.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _thirdParties.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _thirdParties.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ThirdPartyEnumerator(this));
        }

        #endregion

        #region ThirdPartyEnumerator Class

        [Serializable]
        public class ThirdPartyEnumerator : IEnumerator
        {
            ThirdParties _thirdParties;
            int _location;

            public ThirdPartyEnumerator(ThirdParties thirdParties)
            {
                _thirdParties = thirdParties;
                _location = -1;
            }

            public ThirdPartyEnumerator() { }

            #region IEnumerator Members
            public void Reset()
            {
                _location = -1;
            }
            public object Current
            {
                get
                {
                    if ((_location < 0) || (_location >= _thirdParties.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _thirdParties[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _thirdParties.Count)
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
