using System;
using System.Collections;


namespace Prana.Admin.BLL
{
    /// <summary>
    /// CounterParties is a collection class for <see cref="CounterParty"/> class.
    /// </summary>
    public class CounterParties : IList
    {
        ArrayList _counterparties = new ArrayList();
        Hashtable _idMap = new Hashtable();

        public CounterParties()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _counterparties.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CounterParties.this getter implementation
                return _counterparties[index];
            }
            set
            {
                //Add CounterParties.this setter implementation
                _counterparties[index] = value;
                if (!_idMap.ContainsKey(((CounterParty)value).CounterPartyID))
                {
                    _idMap.Add(((CounterParty)value).CounterPartyID, value);
                }
            }
        }

        public void RemoveAt(int index)
        {
            //Add CounterParties.RemoveAt implementation
            _counterparties.RemoveAt(index);
        }

        public void Insert(int index, Object counterparty)
        {
            //Add CounterParties.Insert implementation
            _counterparties.Insert(index, (CounterParty)counterparty);
        }

        public void Remove(Object counterparty)
        {
            //Add CounterParties.Remove implementation
            _counterparties.Remove((CounterParty)counterparty);
        }

        public bool Contains(object counterparty)
        {
            //Add CounterParties.Contains implementation
            return _idMap.ContainsKey(((CounterParty)counterparty).CounterPartyID);
            //return _counterparties.Contains((CounterParty)counterparty);
        }

        public void Clear()
        {
            //Add CounterParties.Clear implementation
            _counterparties.Clear();
        }

        public int IndexOf(object counterparty)
        {
            //Add CounterParties.IndexOf implementation
            return _counterparties.IndexOf((CounterParty)counterparty);
        }

        public int Add(object counterparty)
        {
            //Add CounterParties.Add implementation
            if (!_idMap.ContainsKey(((CounterParty)counterparty).CounterPartyID))
            {
                _idMap.Add(((CounterParty)counterparty).CounterPartyID, counterparty);
            }
            return _counterparties.Add((CounterParty)counterparty);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CounterParties.IsFixedSize getter implementation
                return _counterparties.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CounterParties.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _counterparties.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _counterparties.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _counterparties.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CounterPartyEnumerator(this));
        }

        #endregion

        #region CounterPartyEnumerator Class

        public class CounterPartyEnumerator : IEnumerator
        {
            CounterParties _counterparties;
            int _location;

            public CounterPartyEnumerator(CounterParties counterparties)
            {
                _counterparties = counterparties;
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
                    if ((_location < 0) || (_location >= _counterparties.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _counterparties[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _counterparties.Count)
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