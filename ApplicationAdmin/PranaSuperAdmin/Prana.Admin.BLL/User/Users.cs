using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Users is a collection class for <see cref="User"/> class.
    /// </summary>
    public class Users : IList
    {
        ArrayList _users = new ArrayList();

        public Users()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _users.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                return _users[index];
            }
            set
            {
                //Add Users.this setter implementation
                _users[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _users.RemoveAt(index);
        }

        public void Insert(int index, Object user)
        {
            //Add Users.Insert implementation
            _users.Insert(index, (User)user);
        }

        public void Remove(Object user)
        {
            //Add Users.Remove implementation
            _users.Remove((User)user);
        }

        public bool Contains(object user)
        {
            //Add Users.Contains implementation
            return _users.Contains((User)user);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _users.Clear();
        }

        public int IndexOf(object user)
        {
            //Add Users.IndexOf implementation
            return _users.IndexOf((User)user);
        }

        public int Add(object user)
        {
            //Add Users.Add implementation
            return _users.Add((User)user);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _users.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Users.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _users.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _users.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _users.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new UserEnumerator(this));
        }

        #endregion

        #region AssetEnumerator Class

        public class UserEnumerator : IEnumerator
        {
            Users _users;
            int _location;

            public UserEnumerator(Users users)
            {
                _users = users;
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
                    if ((_location < 0) || (_location >= _users.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _users[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _users.Count)
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
