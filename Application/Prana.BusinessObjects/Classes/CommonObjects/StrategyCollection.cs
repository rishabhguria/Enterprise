using System;
using System.Collections;
using System.Linq;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for StrategyCollection.
    /// </summary>
    [Serializable]
    public class StrategyCollection : IList
    {
        ArrayList _strategyCollection = new ArrayList();

        public StrategyCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _strategyCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add StrategyCollection.this getter implementation
                return _strategyCollection[index];
            }
            set
            {
                //Add StrategyCollection.this setter implementation
                _strategyCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add StrategyCollection.RemoveAt implementation
            _strategyCollection.RemoveAt(index);
        }

        public void Insert(int index, Object strategy)
        {
            //Add StrategyCollection.Insert implementation
            _strategyCollection.Insert(index, (Strategy)strategy);
        }

        public void Remove(Object strategy)
        {
            //Add StrategyCollection.Remove implementation
            _strategyCollection.Remove((Strategy)strategy);
        }

        public bool Contains(object strategy)
        {
            //Add StrategyCollection.Contains implementation
            return _strategyCollection.Contains((Strategy)strategy);
        }

        public bool Contains(int strategyID)
        {
            return _strategyCollection.Cast<Strategy>().Any(strategy => strategy.StrategyID == strategyID);
        }

        public void Clear()
        {
            //Add StrategyCollection.Clear implementation
            _strategyCollection.Clear();
        }

        public int IndexOf(object strategy)
        {
            //Add StrategyCollection.IndexOf implementation
            return _strategyCollection.IndexOf((Strategy)strategy);
        }
        public int IndexOf(int strategyID)
        {

            for (int i = 0; i < _strategyCollection.Count; i++)
            {
                if (strategyID == ((Strategy)_strategyCollection[i]).StrategyID)
                {
                    return i;
                }
            }
            return int.MinValue;
        }
        public int Add(object strategy)
        {
            //Add StrategyCollection.Add implementation
            return _strategyCollection.Add((Strategy)strategy);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add StrategyCollection.IsFixedSize getter implementation
                return _strategyCollection.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add StrategyCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _strategyCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _strategyCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _strategyCollection.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new StrategyEnumerator(this));
        }

        #endregion

        #region StrategyEnumerator Class

        public class StrategyEnumerator : IEnumerator
        {
            StrategyCollection _strategyCollection;
            int _location;

            public StrategyEnumerator(StrategyCollection strategyCollection)
            {
                _strategyCollection = strategyCollection;
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
                    if ((_location < 0) || (_location >= _strategyCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _strategyCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _strategyCollection.Count)
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
