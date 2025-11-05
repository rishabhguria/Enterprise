using System;
using System.Collections;
using System.Linq;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for HandlingInstructions.
    /// </summary>
    public class HandlingInstructions : IList
    {
        ArrayList _handlingInstructions = new ArrayList();

        public HandlingInstructions()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _handlingInstructions.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add HandlingInstructions.this getter implementation
                return _handlingInstructions[index];
            }
            set
            {
                //Add HandlingInstructions.this setter implementation
                _handlingInstructions[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add HandlingInstructions.RemoveAt implementation
            _handlingInstructions.RemoveAt(index);
        }

        public void Insert(int index, Object user)
        {
            //Add HandlingInstructions.Insert implementation
            _handlingInstructions.Insert(index, (HandlingInstruction)user);
        }

        public void Remove(Object user)
        {
            //Add HandlingInstructions.Remove implementation
            _handlingInstructions.Remove((HandlingInstruction)user);
        }

        public bool Contains(object user)
        {
            //Add HandlingInstructions.Contains implementation
            return _handlingInstructions.Contains((HandlingInstruction)user);
        }

        public bool Contains(int handlingInstructionID)
        {
            return _handlingInstructions.Cast<HandlingInstruction>().Any(handlingInstruction => handlingInstruction.HandlingInstructionID == handlingInstructionID);
        }

        public void Clear()
        {
            //Add HandlingInstructions.Clear implementation
            _handlingInstructions.Clear();
        }

        public int IndexOf(object user)
        {
            //Add HandlingInstructions.IndexOf implementation
            return _handlingInstructions.IndexOf((HandlingInstruction)user);
        }

        public int Add(object user)
        {
            //Add HandlingInstructions.Add implementation
            return _handlingInstructions.Add((HandlingInstruction)user);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add HandlingInstructions.IsFixedSize getter implementation
                return _handlingInstructions.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add HandlingInstructions.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _handlingInstructions.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _handlingInstructions.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _handlingInstructions.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new HandlingInstructionEnumerator(this));
        }

        #endregion

        #region AssetEnumerator Class

        public class HandlingInstructionEnumerator : IEnumerator
        {
            HandlingInstructions _handlingInstructions;
            int _location;

            public HandlingInstructionEnumerator(HandlingInstructions users)
            {
                _handlingInstructions = users;
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
                    if ((_location < 0) || (_location >= _handlingInstructions.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _handlingInstructions[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _handlingInstructions.Count)
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
