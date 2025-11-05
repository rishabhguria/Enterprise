using System;
using System.Collections;
using System.Linq;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ExecutionInstructions.
    /// </summary>
    public class ExecutionInstructions : IList
    {
        ArrayList _executionInstructions = new ArrayList();

        public ExecutionInstructions()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _executionInstructions.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ExecutionInstructions.this getter implementation
                return _executionInstructions[index];
            }
            set
            {
                //Add ExecutionInstructions.this setter implementation
                _executionInstructions[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ExecutionInstructions.RemoveAt implementation
            _executionInstructions.RemoveAt(index);
        }

        public void Insert(int index, Object executionInstruction)
        {
            //Add ExecutionInstructions.Insert implementation
            _executionInstructions.Insert(index, (ExecutionInstruction)executionInstruction);
        }

        public void Remove(Object executionInstruction)
        {
            //Add ExecutionInstructions.Remove implementation
            _executionInstructions.Remove((ExecutionInstruction)executionInstruction);
        }

        public bool Contains(object executionInstruction)
        {
            //Add ExecutionInstructions.Contains implementation
            return _executionInstructions.Contains((ExecutionInstruction)executionInstruction);
        }

        public bool Contains(int excecutionInstructionID)
        {
            return _executionInstructions.Cast<ExecutionInstruction>().Any(executionInstruction => executionInstruction.ExecutionInstructionsID == excecutionInstructionID);
        }

        public void Clear()
        {
            //Add ExecutionInstructions.Clear implementation
            _executionInstructions.Clear();
        }

        public int IndexOf(object executionInstruction)
        {
            //Add ExecutionInstructions.IndexOf implementation
            return _executionInstructions.IndexOf((ExecutionInstruction)executionInstruction);
        }

        public int Add(object executionInstruction)
        {
            //Add ExecutionInstructions.Add implementation
            return _executionInstructions.Add((ExecutionInstruction)executionInstruction);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ExecutionInstructions.IsFixedSize getter implementation
                return _executionInstructions.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ExecutionInstructions.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _executionInstructions.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _executionInstructions.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _executionInstructions.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ExecutionInstructionEnumerator(this));
        }

        #endregion

        #region AssetEnumerator Class

        public class ExecutionInstructionEnumerator : IEnumerator
        {
            ExecutionInstructions _executionInstructions;
            int _location;

            public ExecutionInstructionEnumerator(ExecutionInstructions executionInstructions)
            {
                _executionInstructions = executionInstructions;
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
                    if ((_location < 0) || (_location >= _executionInstructions.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _executionInstructions[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _executionInstructions.Count)
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
