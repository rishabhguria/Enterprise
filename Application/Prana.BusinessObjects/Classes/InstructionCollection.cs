using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for InstructionCollection.
    /// </summary>
    [Serializable]
    public class InstructionCollection : IList
    {
        ArrayList _instructionCollection = new ArrayList();

        public InstructionCollection()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _instructionCollection.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add InstructionCollection.this getter implementation
                return _instructionCollection[index];
            }
            set
            {
                //Add InstructionCollection.this setter implementation
                _instructionCollection[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add InstructionCollection.RemoveAt implementation
            _instructionCollection.RemoveAt(index);
        }

        public void Insert(int index, Object instruction)
        {
            //Add InstructionCollection.Insert implementation
            _instructionCollection.Insert(index, (Instruction)instruction);
        }

        public void Remove(Object instruction)
        {
            //Add InstructionCollection.Remove implementation
            _instructionCollection.Remove((Instruction)instruction);
        }

        public bool Contains(object instruction)
        {
            //Add InstructionCollection.Contains implementation
            return _instructionCollection.Contains((Instruction)instruction);
        }

        public void Clear()
        {
            //Add InstructionCollection.Clear implementation
            _instructionCollection.Clear();
        }

        public int IndexOf(object instruction)
        {
            //Add InstructionCollection.IndexOf implementation
            return _instructionCollection.IndexOf((Instruction)instruction);
        }

        public int Add(object instruction)
        {
            //Add InstructionCollection.Add implementation
            return _instructionCollection.Add((Instruction)instruction);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add InstructionCollection.IsFixedSize getter implementation
                return _instructionCollection.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add InstructionCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _instructionCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _instructionCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _instructionCollection.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new InstructionEnumerator(this));
        }

        #endregion

        #region InstructionEnumerator Class

        public class InstructionEnumerator : IEnumerator
        {
            InstructionCollection _instructionCollection;
            int _location;

            public InstructionEnumerator(InstructionCollection instructionCollection)
            {
                _instructionCollection = instructionCollection;
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
                    if ((_location < 0) || (_location >= _instructionCollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _instructionCollection[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _instructionCollection.Count)
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
