using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for TicketCollection.
    /// </summary>		
    public class TicketCollection : IList
    {
        ArrayList _ticketCollection = new ArrayList();

        public TicketCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _ticketCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add TicketCollection.this getter implementation
                return _ticketCollection[index];
            }
            set
            {
                //Add TicketCollection.this setter implementation
                _ticketCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add TicketCollection.RemoveAt implementation
            _ticketCollection.RemoveAt(index);
        }

        public void Insert(int index, Object ticket)
        {
            //Add TicketCollection.Insert implementation
            _ticketCollection.Insert(index, (Ticket)ticket);
        }

        public void Remove(Object ticket)
        {
            //Add TicketCollection.Remove implementation
            _ticketCollection.Remove((Ticket)ticket);
        }

        public bool Contains(object ticket)
        {
            //Add TicketCollection.Contains implementation
            return _ticketCollection.Contains((Ticket)ticket);
        }

        public void Clear()
        {
            //Add TicketCollection.Clear implementation
            _ticketCollection.Clear();
        }

        public int IndexOf(object ticket)
        {
            //Add TicketCollection.IndexOf implementation
            return _ticketCollection.IndexOf((Ticket)ticket);
        }

        public int Add(object ticket)
        {
            //Add TicketCollection.Add implementation
            return _ticketCollection.Add((Ticket)ticket);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add TicketCollection.IsFixedSize getter implementation
                return _ticketCollection.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add TicketCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _ticketCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _ticketCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _ticketCollection.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new TicketEnumerator(this));
        }

        #endregion

        #region TicketEnumerator Class

        public class TicketEnumerator : IEnumerator
        {
            TicketCollection _ticketCollection;
            int _location;

            public TicketEnumerator(TicketCollection ticketCollection)
            {
                _ticketCollection = ticketCollection;
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
                    if ((_location < 0) || (_location >= _ticketCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _ticketCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _ticketCollection.Count)
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
