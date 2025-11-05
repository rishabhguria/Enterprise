using System;
using System.Collections;

namespace Prana.Admin.BLL

{
    /// <summary>
    /// Summary description for UserLevelOverallLimits.
    /// </summary>
    public class UserLevelOverallLimits : IList
    {
        ArrayList _userLevelOverallLimits = new ArrayList();

        public UserLevelOverallLimits()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _userLevelOverallLimits.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add UserLevelOverallLimits.this getter implementation
                return _userLevelOverallLimits[index];
            }
            set
            {
                //Add UserLevelOverallLimits.this setter implementation
                _userLevelOverallLimits[index] = value;
            }
        }
        public void RemoveAt(int index)
        {
            //Add UserLevelOverallLimits.RemoveAt implementation
            _userLevelOverallLimits.RemoveAt(index);
        }

        public void Insert(int index, Object userLevelOverallLimits)
        {
            //Add UserLevelOverallLimits.Insert implementation
            _userLevelOverallLimits.Insert(index, (UserLevelOverallLimits)userLevelOverallLimits);
        }

        public void Remove(Object userLevelOverallLimits)
        {
            //Add UserLevelOverallLimits.Remove implementation
            _userLevelOverallLimits.Remove((UserLevelOverallLimits)userLevelOverallLimits);
        }

        public bool Contains(object userLevelOverallLimits)
        {
            //Add UserLevelOverallLimits.Contains implementation
            return _userLevelOverallLimits.Contains((UserLevelOverallLimits)userLevelOverallLimits);
        }

        public void Clear()
        {
            //Add UserLevelOverallLimits.Clear implementation
            _userLevelOverallLimits.Clear();
        }

        public int IndexOf(object userLevelOverallLimits)
        {
            //Add UserLevelOverallLimits.IndexOf implementation
            return _userLevelOverallLimits.IndexOf((UserLevelOverallLimits)userLevelOverallLimits);
        }

        public int Add(object userLevelOverallLimit)
        {
            //Add UserLevelOverallLimits.Add implementation
            return _userLevelOverallLimits.Add((UserLevelOverallLimit)userLevelOverallLimit);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add UserLevelOverallLimits.IsFixedSize getter implementation
                return _userLevelOverallLimits.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add UserLevelOverallLimits.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _userLevelOverallLimits.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _userLevelOverallLimits.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _userLevelOverallLimits.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new UserLevelOverallLimitsEnumerator(this));
        }

        #endregion

        #region UserLevelOverallLimitsEnumerator Class

        public class UserLevelOverallLimitsEnumerator : IEnumerator
        {
            UserLevelOverallLimits _userLevelOverallLimits;
            int _location;

            public UserLevelOverallLimitsEnumerator(UserLevelOverallLimits userLevelOverallLimits)
            {
                _userLevelOverallLimits = userLevelOverallLimits;
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
                    if ((_location < 0) || (_location >= _userLevelOverallLimits.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _userLevelOverallLimits[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _userLevelOverallLimits.Count)
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
