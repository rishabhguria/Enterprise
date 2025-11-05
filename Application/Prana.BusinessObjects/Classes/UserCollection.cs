using System;
using System.Collections;


namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for IUsers.
    /// </summary>
    [Serializable]
    public class UserCollection : IEnumerable
    {
        ArrayList _users = new ArrayList();

        public UserCollection()
        {
        }

        public void Add(User user)
        {
            _users.Add(user);
        }

        public User this[int i]
        {
            get
            {

                if ((i < 0) || (i >= _users.Count))
                {
                    //throw (new IndexOutOfRangeException());
                    // exception should not be thrown in a property - MSMR
                    return null;
                }
                else
                {
                    return (User)_users[i];
                }
            }
        }

        public int Count
        {
            get
            {
                return _users.Count;
            }
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new UserEnumerator(this));
        }

        #endregion

        public class UserEnumerator : IEnumerator
        {
            UserCollection _users;
            int _location;

            public UserEnumerator(UserCollection users)
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
    }
}
