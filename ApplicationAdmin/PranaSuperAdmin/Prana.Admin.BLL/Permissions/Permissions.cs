using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Permissions.
    /// </summary>
    public class Permissions : IList
    {
        ArrayList _permissions = new ArrayList();
        private int _userID = int.MinValue;

        public Permissions()
        {
        }

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _permissions.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Permissions.this getter implementation
                return _permissions[index];
            }
            set
            {
                //Add Permissions.this setter implementation
                _permissions[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Permissions.RemoveAt implementation
            _permissions.RemoveAt(index);
        }

        public void Insert(int index, Object permission)
        {
            //Add Permissions.Insert implementation
            _permissions.Insert(index, (Permission)permission);
        }

        public void Remove(Object permission)
        {
            //Add Permissions.Remove implementation
            _permissions.Remove((Permission)permission);
        }

        public bool Contains(object permission)
        {
            //Add Permissions.Contains implementation
            return _permissions.Contains((Permission)permission);
        }

        public void Clear()
        {
            //Add Permissions.Clear implementation
            _permissions.Clear();
        }

        public int IndexOf(object permission)
        {
            //Add Permissions.IndexOf implementation
            return _permissions.IndexOf((Permission)permission);
        }

        public int Add(object permission)
        {
            //Add Permissions.Add implementation
            return _permissions.Add((Permission)permission);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Permissions.IsFixedSize getter implementation
                return _permissions.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Permissions.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _permissions.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _permissions.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _permissions.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new PermissionEnumerator(this));
        }

        #endregion

        #region PermissionEnumerator Class

        public class PermissionEnumerator : IEnumerator
        {
            Permissions _permissions;
            int _location;

            public PermissionEnumerator(Permissions permissions)
            {
                _permissions = permissions;
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
                    if ((_location < 0) || (_location >= _permissions.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _permissions[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _permissions.Count)
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
