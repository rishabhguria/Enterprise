using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for MPIDs.
    /// </summary>
    public class MPIDs : IList
    {
        ArrayList _mPIDs = new ArrayList();
        public MPIDs()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _mPIDs.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add MPIDs.this getter implementation
                if (index >= _mPIDs.Count || index < 0)
                {
                    return new MPIDs();
                }
                else
                {
                    return _mPIDs[index];
                }
            }
            set
            {
                //Add MPIDs.this setter implementation
                _mPIDs[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add MPIDs.RemoveAt implementation
            _mPIDs.RemoveAt(index);
        }

        public void Insert(int index, Object mPID)
        {
            //Add MPIDs.Insert implementation
            _mPIDs.Insert(index, (MPID)mPID);
        }

        public void Remove(Object mPID)
        {
            //Add MPIDs.Remove implementation
            _mPIDs.Remove((MPID)mPID);
        }

        public bool Contains(object mPID)
        {
            //Add MPIDs.Contains implementation
            return _mPIDs.Contains((MPID)mPID);
        }

        public void Clear()
        {
            //Add MPIDs.Clear implementation
            _mPIDs.Clear();
        }

        public int IndexOf(object mPID)
        {
            //Add MPIDs.IndexOf implementation
            //return _mPIDs.IndexOf((MPID)mPID);

            MPID tempAccount = (MPID)mPID;
            int counter = 0;
            int result = int.MinValue;
            foreach (MPID _mPID in _mPIDs)
            {
                if (_mPID.CompanyMPID == tempAccount.CompanyMPID
                    && _mPID.CompanyID == tempAccount.CompanyID
                    && _mPID.MPIDName == tempAccount.MPIDName
                    )
                {
                    result = counter;
                    break;
                }
                else
                {
                    counter++;
                }
            }
            return result;
        }

        public int Add(object mPID)
        {
            //Add MPIDs.Add implementation
            return _mPIDs.Add((MPID)mPID);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add MPIDs.IsFixedSize getter implementation
                return _mPIDs.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add MPIDs.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _mPIDs.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _mPIDs.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _mPIDs.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new MPIDEnumerator(this));
        }

        #endregion

        #region MPIDEnumerator Class

        public class MPIDEnumerator : IEnumerator
        {
            MPIDs _mPIDs;
            int _location;

            public MPIDEnumerator(MPIDs accounts)
            {
                _mPIDs = accounts;
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
                    if ((_location < 0) || (_location >= _mPIDs.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _mPIDs[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _mPIDs.Count)
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
