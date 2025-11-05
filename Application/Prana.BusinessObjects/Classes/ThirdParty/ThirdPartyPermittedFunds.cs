using Prana.BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class ThirdPartyPermittedAccounts : IList
    {
        ArrayList _thirdPartyPermittedAccounts = new ArrayList();
        public ThirdPartyPermittedAccounts()
        {

        }


        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _thirdPartyPermittedAccounts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ThirdPartyPermittedAccounts.this getter implementation
                return _thirdPartyPermittedAccounts[index];
            }
            set
            {
                //Add ThirdPartyPermittedAccounts.this setter implementation
                _thirdPartyPermittedAccounts[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ThirdPartyPermittedAccounts.RemoveAt implementation
            _thirdPartyPermittedAccounts.RemoveAt(index);
        }

        public void Insert(int index, Object thirdPartyPermittedAccount)
        {
            //Add ThirdPartyPermittedAccounts.Insert implementation
            _thirdPartyPermittedAccounts.Insert(index, (ThirdPartyPermittedAccount)thirdPartyPermittedAccount);
        }

        public void Remove(Object thirdPartyPermittedAccount)
        {
            //Add ThirdPartyPermittedAccounts.Remove implementation
            _thirdPartyPermittedAccounts.Remove((ThirdPartyPermittedAccount)thirdPartyPermittedAccount);
        }

        public bool Contains(object thirdPartyPermittedAccount)
        {
            //Add ThirdPartyPermittedAccounts.Contains implementation
            return _thirdPartyPermittedAccounts.Contains((ThirdPartyPermittedAccount)thirdPartyPermittedAccount);
        }

        public void Clear()
        {
            //Add ThirdPartyPermittedAccounts.Clear implementation
            _thirdPartyPermittedAccounts.Clear();
        }

        public int IndexOf(object thirdPartyPermittedAccount)
        {
            //Add ThirdPartyPermittedAccounts.IndexOf implementation
            return _thirdPartyPermittedAccounts.IndexOf((ThirdPartyPermittedAccount)thirdPartyPermittedAccount);
        }

        public int Add(object thirdPartyPermittedAccount)
        {
            //Add ThirdPartyPermittedAccounts.Add implementation
            return _thirdPartyPermittedAccounts.Add((ThirdPartyPermittedAccount)thirdPartyPermittedAccount);
        }

        public void AddRange(List<ThirdPartyPermittedAccount> thirdPartypermittedAccount)
        {
            _thirdPartyPermittedAccounts.AddRange(thirdPartypermittedAccount);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ThirdPartyPermittedAccounts.IsFixedSize getter implementation
                return _thirdPartyPermittedAccounts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ThirdPartyPermittedAccounts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _thirdPartyPermittedAccounts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _thirdPartyPermittedAccounts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _thirdPartyPermittedAccounts.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ThirdPartyPermittedAccountEnumerator(this));
        }

        #endregion

        #region ThirdPartyPermittedAccountEnumerator Class

        public class ThirdPartyPermittedAccountEnumerator : IEnumerator
        {
            ThirdPartyPermittedAccounts _ThirdPartyPermittedAccounts;
            int _location;

            public ThirdPartyPermittedAccountEnumerator(ThirdPartyPermittedAccounts ThirdPartyPermittedAccounts)
            {
                _ThirdPartyPermittedAccounts = ThirdPartyPermittedAccounts;
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
                    if ((_location < 0) || (_location >= _ThirdPartyPermittedAccounts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _ThirdPartyPermittedAccounts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _ThirdPartyPermittedAccounts.Count)
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
