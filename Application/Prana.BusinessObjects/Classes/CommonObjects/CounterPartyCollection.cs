using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class CounterPartyCollection : IList
    {
        ArrayList _counterPartyCollection = new ArrayList();

        public CounterPartyCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _counterPartyCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CounterPartyCollection.this getter implementation
                return _counterPartyCollection[index];
            }
            set
            {
                //Add CounterPartyCollection.this setter implementation
                _counterPartyCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CounterPartyCollection.RemoveAt implementation
            _counterPartyCollection.RemoveAt(index);
        }

        public void Insert(int index, Object counterParty)
        {
            //Add CounterPartyCollection.Insert implementation
            _counterPartyCollection.Insert(index, (CounterParty)counterParty);
        }

        public void Remove(Object counterParty)
        {
            //Add CounterPartyCollection.Remove implementation
            _counterPartyCollection.Remove((CounterParty)counterParty);
        }

        public void Sort()
        {
            //Sort CounterPartyCollection based on name
            _counterPartyCollection.Sort((IComparer)new counterPartySorter());
        }

        public bool Contains(object counterParty)
        {
            //Add CounterPartyCollection.Contains implementation
            return _counterPartyCollection.Contains((CounterParty)counterParty);
        }
        public bool Contains(int counterPartyID)
        {
            //Add CounterPartyCollection.Contains implementation
            foreach (CounterParty counterParty in _counterPartyCollection)
            {
                if (counterParty.CounterPartyID == counterPartyID)
                {
                    return true;
                }
            }
            return false;
        }
        public void Clear()
        {
            //Add CounterPartyCollection.Clear implementation
            _counterPartyCollection.Clear();
        }

        public int IndexOf(object counterParty)
        {
            //Add CounterPartyCollection.IndexOf implementation
            return _counterPartyCollection.IndexOf((CounterParty)counterParty);
        }

        public int Add(object counterParty)
        {
            //Add CounterPartyCollection.Add implementation
            return _counterPartyCollection.Add((CounterParty)counterParty);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CounterPartyCollection.IsFixedSize getter implementation
                return _counterPartyCollection.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CounterPartyCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _counterPartyCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _counterPartyCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _counterPartyCollection.SyncRoot;
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

        #region AssetEnumerator Class

        public class CounterPartyEnumerator : IEnumerator
        {
            CounterPartyCollection _counterPartyCollection;
            int _location;

            public CounterPartyEnumerator(CounterPartyCollection counterPartyCollection)
            {
                _counterPartyCollection = counterPartyCollection;
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
                    if ((_location < 0) || (_location >= _counterPartyCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _counterPartyCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _counterPartyCollection.Count)
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
        private class counterPartySorter : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                CounterParty cp1 = (CounterParty)a;
                CounterParty cp2 = (CounterParty)b;
                int comparison = String.Compare(cp1.Name, cp2.Name, comparisonType: StringComparison.OrdinalIgnoreCase);
                if (comparison > 1)
                    return 1;
                if (comparison < 1)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
