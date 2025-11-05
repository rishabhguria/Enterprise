using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Strategies.
    /// </summary>
    public class Strategies : IList
    {
        ArrayList _strategies = new ArrayList();
        public Strategies()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _strategies.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Strategies.this getter implementation
                if (index >= _strategies.Count || index < 0)
                {
                    return new Strategies();
                }
                else
                {
                    return _strategies[index];
                }
            }
            set
            {
                //Add Strategies.this setter implementation
                _strategies[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Strategies.RemoveAt implementation
            _strategies.RemoveAt(index);
        }

        public void Insert(int index, Object strategy)
        {
            //Add Strategies.Insert implementation
            _strategies.Insert(index, (Strategy)strategy);
        }

        public void Remove(Object strategy)
        {
            //Add Strategies.Remove implementation
            _strategies.Remove((Strategy)strategy);
        }

        public bool Contains(object strategy)
        {
            //Add Strategies.Contains implementation
            return _strategies.Contains((Strategy)strategy);
        }

        public void Clear()
        {
            //Add Strategies.Clear implementation
            _strategies.Clear();
        }

        public int IndexOf(object strategy)
        {
            //Add Strategies.IndexOf implementation
            //return _strategies.IndexOf((Strategy)strategy);

            Strategy tempStrategy = (Strategy)strategy;
            int counter = 0;
            int result = int.MinValue;
            foreach (Strategy _strategy in _strategies)
            {
                if (_strategy.StrategyID == tempStrategy.StrategyID
                    && _strategy.CompanyID == tempStrategy.CompanyID
                    && _strategy.StrategyName == tempStrategy.StrategyName
                    && _strategy.StrategyShortName == tempStrategy.StrategyShortName
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

        public int Add(object strategy)
        {
            //Add Strategies.Add implementation
            return _strategies.Add((Strategy)strategy);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Strategies.IsFixedSize getter implementation
                return _strategies.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Strategies.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _strategies.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _strategies.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _strategies.SyncRoot;
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
            Strategies _strategies;
            int _location;

            public StrategyEnumerator(Strategies strategies)
            {
                _strategies = strategies;
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
                    if ((_location < 0) || (_location >= _strategies.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _strategies[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _strategies.Count)
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
