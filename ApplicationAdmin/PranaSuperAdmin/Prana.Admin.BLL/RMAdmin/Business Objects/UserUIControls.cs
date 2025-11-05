using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for UserUIControls.
    /// </summary>
    public class UserUIControls : IList
    {
        ArrayList _userUIControls = new ArrayList();

        public UserUIControls()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _userUIControls.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add UserUIControls.this getter implementation
                return _userUIControls[index];
            }
            set
            {
                //Add UserUIControls.this setter implementation
                _userUIControls[index] = value;
            }
        }
        public void RemoveAt(int index)
        {
            //Add UserUIControls.RemoveAt implementation
            _userUIControls.RemoveAt(index);
        }

        public void Insert(int index, Object userUIControls)
        {
            //Add UserUIControls.Insert implementation
            _userUIControls.Insert(index, (UserUIControls)userUIControls);
        }

        public void Remove(Object userUIControls)
        {
            //Add UserUIControls.Remove implementation
            _userUIControls.Remove((UserUIControls)userUIControls);
        }

        public bool Contains(object userUIControls)
        {
            //Add UserUIControls.Contains implementation
            return _userUIControls.Contains((UserUIControls)userUIControls);
        }

        public void Clear()
        {
            //Add UUserUIControls.Clear implementation
            _userUIControls.Clear();
        }

        public int IndexOf(object userUIControls)
        {
            //Add UserUIControls.IndexOf implementation
            return _userUIControls.IndexOf((UserUIControls)userUIControls);
        }

        public int Add(object userUIControl)
        {
            //Add UserUIControls.Add implementation
            return _userUIControls.Add((UserUIControl)userUIControl);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add UserUIControls.IsFixedSize getter implementation
                return _userUIControls.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add UserUIControls.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _userUIControls.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _userUIControls.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _userUIControls.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new UserUIControlsEnumerator(this));
        }

        #endregion

        #region UserUIControlsEnumerator Class

        public class UserUIControlsEnumerator : IEnumerator
        {
            UserUIControls _userUIControls;
            int _location;

            public UserUIControlsEnumerator(UserUIControls userUIControls)
            {
                _userUIControls = userUIControls;
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
                    if ((_location < 0) || (_location >= _userUIControls.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _userUIControls[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _userUIControls.Count)
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
