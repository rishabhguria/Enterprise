using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    public class PermissionTypes : IList
    {
        ArrayList _permissionTypes = new ArrayList();

        public PermissionTypes()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _permissionTypes.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add PermissionTypes.this getter implementation
                return _permissionTypes[index];
            }
            set
            {
                //Add PermissionTypes.this setter implementation
                _permissionTypes[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add PermissionTypes.RemoveAt implementation
            _permissionTypes.RemoveAt(index);
        }

        public void Insert(int index, Object permissionType)
        {
            //Add PermissionTypes.Insert implementation
            _permissionTypes.Insert(index, (PermissionType)permissionType);
        }

        public void Remove(Object permissionType)
        {
            //Add PermissionTypes.Remove implementation
            _permissionTypes.Remove((PermissionType)permissionType);
        }

        public bool Contains(object permissionType)
        {
            //Add PermissionTypes.Contains implementation
            return _permissionTypes.Contains((PermissionType)permissionType);
        }

        public void Clear()
        {
            //Add PermissionTypes.Clear implementation
            _permissionTypes.Clear();
        }

        public int IndexOf(object permissionType)
        {
            //Add PermissionTypes.IndexOf implementation
            return _permissionTypes.IndexOf((PermissionType)permissionType);
        }

        public int Add(object permissionType)
        {
            //Add PermissionTypes.Add implementation
            return _permissionTypes.Add((PermissionType)permissionType);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add PermissionTypes.IsFixedSize getter implementation
                return _permissionTypes.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add PermissionTypes.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _permissionTypes.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _permissionTypes.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _permissionTypes.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new PermissionTypeEnumerator(this));
        }

        #endregion

        #region PermissionTypeEnumerator Class

        public class PermissionTypeEnumerator : IEnumerator
        {
            PermissionTypes _permissionTypes;
            int _location;

            public PermissionTypeEnumerator(PermissionTypes permissionTypes)
            {
                _permissionTypes = permissionTypes;
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
                    if ((_location < 0) || (_location >= _permissionTypes.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _permissionTypes[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _permissionTypes.Count)
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
