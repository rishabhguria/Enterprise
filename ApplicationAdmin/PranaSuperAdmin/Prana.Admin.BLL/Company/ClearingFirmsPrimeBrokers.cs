using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ClearingFirmsPrimeBrokers.
    /// </summary>
    public class ClearingFirmsPrimeBrokers : IList
    {
        ArrayList _clearingFirmsPrimeBrokers = new ArrayList();
        public ClearingFirmsPrimeBrokers()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _clearingFirmsPrimeBrokers.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ClearingFirmsPrimeBrokers.this getter implementation
                if (index >= _clearingFirmsPrimeBrokers.Count || index < 0)
                {
                    return new ClearingFirmsPrimeBrokers();
                }
                else
                {
                    return _clearingFirmsPrimeBrokers[index];
                }
            }
            set
            {
                //Add ClearingFirmsPrimeBrokers.this setter implementation
                _clearingFirmsPrimeBrokers[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ClearingFirmsPrimeBrokers.RemoveAt implementation
            _clearingFirmsPrimeBrokers.RemoveAt(index);
        }

        public void Insert(int index, Object clearingFirmPrimeBroker)
        {
            //Add ClearingFirmsPrimeBrokers.Insert implementation
            _clearingFirmsPrimeBrokers.Insert(index, (ClearingFirmPrimeBroker)clearingFirmPrimeBroker);
        }

        public void Remove(Object clearingFirmPrimeBroker)
        {
            //Add ClearingFirmsPrimeBrokers.Remove implementation
            _clearingFirmsPrimeBrokers.Remove((ClearingFirmPrimeBroker)clearingFirmPrimeBroker);
        }

        public bool Contains(object clearingFirmPrimeBroker)
        {
            //Add ClearingFirmsPrimeBrokers.Contains implementation
            return _clearingFirmsPrimeBrokers.Contains((ClearingFirmPrimeBroker)clearingFirmPrimeBroker);
        }

        public void Clear()
        {
            //Add ClearingFirmsPrimeBrokers.Clear implementation
            _clearingFirmsPrimeBrokers.Clear();
        }

        public int IndexOf(object clearingFirmPrimeBroker)
        {
            //Add ClearingFirmsPrimeBrokers.IndexOf implementation
            //return _clearingFirmsPrimeBrokers.IndexOf((ClearingFirmPrimeBroker)clearingFirmPrimeBroker);

            ClearingFirmPrimeBroker tempClearingFirmPrimeBroker = (ClearingFirmPrimeBroker)clearingFirmPrimeBroker;
            int counter = 0;
            int result = int.MinValue;
            foreach (ClearingFirmPrimeBroker _clearingFirmPrimeBroker in _clearingFirmsPrimeBrokers)
            {
                if (_clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersID == tempClearingFirmPrimeBroker.ClearingFirmsPrimeBrokersID
                    && _clearingFirmPrimeBroker.CompanyID == tempClearingFirmPrimeBroker.CompanyID
                    && _clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName == tempClearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName
                    && _clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName == tempClearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName
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

        public int Add(object clearingFirmPrimeBroker)
        {
            //Add ClearingFirmsPrimeBrokers.Add implementation
            return _clearingFirmsPrimeBrokers.Add((ClearingFirmPrimeBroker)clearingFirmPrimeBroker);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ClearingFirmsPrimeBrokers.IsFixedSize getter implementation
                return _clearingFirmsPrimeBrokers.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ClearingFirmsPrimeBrokers.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _clearingFirmsPrimeBrokers.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _clearingFirmsPrimeBrokers.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _clearingFirmsPrimeBrokers.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ClearingFirmPrimeBrokerEnumerator(this));
        }

        #endregion

        #region ClearingFirmPrimeBrokerEnumerator Class

        public class ClearingFirmPrimeBrokerEnumerator : IEnumerator
        {
            ClearingFirmsPrimeBrokers _clearingFirmsPrimeBrokers;
            int _location;

            public ClearingFirmPrimeBrokerEnumerator(ClearingFirmsPrimeBrokers clients)
            {
                _clearingFirmsPrimeBrokers = clients;
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
                    if ((_location < 0) || (_location >= _clearingFirmsPrimeBrokers.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _clearingFirmsPrimeBrokers[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _clearingFirmsPrimeBrokers.Count)
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
