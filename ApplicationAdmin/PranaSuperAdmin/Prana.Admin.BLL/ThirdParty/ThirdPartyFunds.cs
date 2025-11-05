using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ThirdPartyAccounts.
    /// </summary>
    public class ThirdPartyAccounts : IList
    {
        ArrayList _thirdPartyAccounts = new ArrayList();
        public ThirdPartyAccounts()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _thirdPartyAccounts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ThirdPartyAccounts.this getter implementation
                return _thirdPartyAccounts[index];
            }
            set
            {
                //Add ThirdPartyAccounts.this setter implementation
                _thirdPartyAccounts[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ThirdPartyAccounts.RemoveAt implementation
            _thirdPartyAccounts.RemoveAt(index);
        }

        public void Insert(int index, Object thirdPartyAccount)
        {
            //Add ThirdPartyAccounts.Insert implementation
            _thirdPartyAccounts.Insert(index, (ThirdPartyAccount)thirdPartyAccount);
        }

        public void Remove(Object thirdPartyAccount)
        {
            //Add ThirdPartyAccounts.Remove implementation
            _thirdPartyAccounts.Remove((ThirdPartyAccount)thirdPartyAccount);
        }

        public bool Contains(object thirdPartyAccount)
        {
            //Add ThirdPartyAccounts.Contains implementation
            return _thirdPartyAccounts.Contains((ThirdPartyAccount)thirdPartyAccount);
        }

        public void Clear()
        {
            //Add ThirdPartyAccounts.Clear implementation
            _thirdPartyAccounts.Clear();
        }

        public int IndexOf(object thirdPartyAccount)
        {
            //Add ThirdPartyAccounts.IndexOf implementation
            return _thirdPartyAccounts.IndexOf((ThirdPartyAccount)thirdPartyAccount);
        }

        public int Add(object thirdPartyAccount)
        {
            //Add ThirdPartyAccounts.Add implementation
            return _thirdPartyAccounts.Add((ThirdPartyAccount)thirdPartyAccount);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ThirdPartyAccounts.IsFixedSize getter implementation
                return _thirdPartyAccounts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ThirdPartyAccounts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _thirdPartyAccounts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _thirdPartyAccounts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _thirdPartyAccounts.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ThirdPartyaccountEnumerator(this));
        }

        #endregion

        #region ThirdPartyaccountEnumerator Class

        public class ThirdPartyaccountEnumerator : IEnumerator
        {
            ThirdPartyAccounts _thirdPartyAccounts;
            int _location;

            public ThirdPartyaccountEnumerator(ThirdPartyAccounts thirdPartyAccounts)
            {
                _thirdPartyAccounts = thirdPartyAccounts;
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
                    if ((_location < 0) || (_location >= _thirdPartyAccounts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _thirdPartyAccounts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _thirdPartyAccounts.Count)
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
