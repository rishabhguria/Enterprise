using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Traders.
    /// </summary>
    public class Traders : IList
    {
        ArrayList _traders = new ArrayList();

        public Traders()
        {
        }
        public Trader GetTrader(string shortName)
        {
            foreach (Trader trader in _traders)
            {
                if (trader.ShortName == shortName)
                {
                    return trader;
                }
            }
            return null;

        }
        public bool ContainsShortName(string stName)
        {
            foreach (Trader trader in _traders)
            {
                if (trader.ShortName.Equals(stName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _traders.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Traders.this getter implementation
                if (index >= _traders.Count || index < 0)
                {
                    return new Traders();
                }
                else
                {
                    return _traders[index];
                }
            }
            set
            {
                //Add Traders.this setter implementation
                _traders[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Traders.RemoveAt implementation
            _traders.RemoveAt(index);
        }

        public void Insert(int index, Object trader)
        {
            //Add Traders.Insert implementation
            _traders.Insert(index, (Trader)trader);
        }

        public void Remove(Object trader)
        {
            //Add Traders.Remove implementation
            _traders.Remove((Trader)trader);
        }

        public bool Contains(object trader)
        {
            //Add Traders.Contains implementation
            return _traders.Contains((Trader)trader);
        }


        public void Clear()
        {
            //Add Traders.Clear implementation
            _traders.Clear();
        }

        public int IndexOf(object trader)
        {
            //Add Traders.IndexOf implementation
            //return _traders.IndexOf((Trader)trader);

            Trader tempTrader = (Trader)trader;
            int counter = 0;
            int result = int.MinValue;
            foreach (Trader _trader in _traders)
            {
                if (_trader.CompanyID == tempTrader.CompanyID
                    && _trader.EMail == tempTrader.EMail
                    && _trader.Fax == tempTrader.Fax
                    && _trader.FirstName == tempTrader.FirstName
                    && _trader.LastName == tempTrader.LastName
                    && _trader.Pager == tempTrader.Pager
                    && _trader.ShortName == tempTrader.ShortName
                    && _trader.TelephoneCell == tempTrader.TelephoneCell
                    && _trader.TelephoneHome == tempTrader.TelephoneHome
                    && _trader.TelephoneWork == tempTrader.TelephoneWork
                    && _trader.Title == tempTrader.Title
                    && _trader.TraderID == tempTrader.TraderID
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

        public int Add(object trader)
        {
            //Add Traders.Add implementation
            return _traders.Add((Trader)trader);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Traders.IsFixedSize getter implementation
                return _traders.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Traders.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _traders.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _traders.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _traders.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new TraderEnumerator(this));
        }

        #endregion

        #region TraderEnumerator Class

        public class TraderEnumerator : IEnumerator
        {
            Traders _traders;
            int _location;

            public TraderEnumerator(Traders traders)
            {
                _traders = traders;
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
                    if ((_location < 0) || (_location >= _traders.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _traders[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _traders.Count)
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
