using System;
using System.Collections;
namespace Prana.TradingTicket
{
    /// <summary>
    /// Summary description for TradingTickets.
    /// </summary>
    public class TradingTicketSettingsCollection : IList
    {
        ArrayList _tradingTickets = new ArrayList();
        public TradingTicketSettingsCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _tradingTickets.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add TradingTickets.this getter implementation
                return _tradingTickets[index];
            }
            set
            {
                //Add TradingTickets.this setter implementation
                _tradingTickets[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add TradingTickets.RemoveAt implementation
            _tradingTickets.RemoveAt(index);
        }

        public void Insert(int index, Object tradingTicket)
        {
            //Add TradingTickets.Insert implementation
            _tradingTickets.Insert(index, (TradingTicketSettings)tradingTicket);
        }

        public void Remove(Object tradingTicket)
        {
            //Add TradingTickets.Remove implementation
            _tradingTickets.Remove((TradingTicketSettings)tradingTicket);
        }

        public bool Contains(object tradingTicket)
        {
            //Add TradingTickets.Contains implementation
            return _tradingTickets.Contains((TradingTicketSettings)tradingTicket);
        }

        public void Clear()
        {
            //Add TradingTickets.Clear implementation
            _tradingTickets.Clear();
        }

        public int IndexOf(object tradingTicket)
        {
            //Add TradingTickets.IndexOf implementation
            return _tradingTickets.IndexOf((TradingTicketSettings)tradingTicket);
        }

        public int Add(object tradingTicket)
        {
            //Add TradingTickets.Add implementation
            return _tradingTickets.Add((TradingTicketSettings)tradingTicket);
        }
        public TradingTicketSettings GetTradingTicketByID(string ID)
        {
            foreach (TradingTicketSettings tt in _tradingTickets)
            {
                if (tt.TicketSettingsID.Equals(ID))
                {
                    return tt;
                }
            }
            return null;

        }
        public bool IsFixedSize
        {
            get
            {
                //Add TradingTickets.IsFixedSize getter implementation
                return _tradingTickets.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add TradingTickets.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _tradingTickets.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _tradingTickets.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _tradingTickets.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new TradingTicketEnumerator(this));
        }

        #endregion

        #region TradingTicketEnumerator Class

        public class TradingTicketEnumerator : IEnumerator
        {
            TradingTicketSettingsCollection _tradingTickets;
            int _location;

            public TradingTicketEnumerator(TradingTicketSettingsCollection tradingTickets)
            {
                _tradingTickets = tradingTickets;
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
                    if ((_location < 0) || (_location >= _tradingTickets.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _tradingTickets[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _tradingTickets.Count)
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
