using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for States.
    /// </summary>
    public class States : IList
    {
        ArrayList _states = new ArrayList();
        public States()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _states.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add States.this getter implementation
                return _states[index];
            }
            set
            {
                //Add States.this setter implementation
                _states[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add States.RemoveAt implementation
            _states.RemoveAt(index);
        }

        public void Insert(int index, Object state)
        {
            //Add States.Insert implementation
            _states.Insert(index, (State)state);
        }

        public void Remove(Object state)
        {
            //Add States.Remove implementation
            _states.Remove((State)state);
        }

        public bool Contains(object state)
        {
            //Add States.Contains implementation
            return _states.Contains((State)state);
        }

        public void Clear()
        {
            //Add States.Clear implementation
            _states.Clear();
        }

        public int IndexOf(object state)
        {
            //Add States.IndexOf implementation
            return _states.IndexOf((State)state);
        }

        public int Add(object state)
        {
            //Add States.Add implementation
            return _states.Add((State)state);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add States.IsFixedSize getter implementation
                return _states.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add States.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _states.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _states.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _states.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new StateEnumerator(this));
        }

        #endregion

        #region StateEnumerator Class

        public class StateEnumerator : IEnumerator
        {
            States _states;
            int _location;

            public StateEnumerator(States states)
            {
                _states = states;
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
                    if ((_location < 0) || (_location >= _states.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _states[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _states.Count)
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
