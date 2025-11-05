using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    public class SetUpContracts : IList
    {
        ArrayList _setUpContracts = new ArrayList();
        public SetUpContracts()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _setUpContracts.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add SetUpContracts.this getter implementation
                return _setUpContracts[index];
            }
            set
            {
                //Add SetUpContracts.this setter implementation
                _setUpContracts[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add SetUpContracts.RemoveAt implementation
            _setUpContracts.RemoveAt(index);
        }

        public void Insert(int index, Object setUpContract)
        {
            //Add SetUpContracts.Insert implementation
            _setUpContracts.Insert(index, (SetUpContract)setUpContract);
        }

        public void Remove(Object setUpContract)
        {
            //Add SetUpContracts.Remove implementation
            _setUpContracts.Remove((SetUpContract)setUpContract);
        }

        public bool Contains(object setUpContract)
        {
            //Add SetUpContracts.Contains implementation
            return _setUpContracts.Contains((SetUpContract)setUpContract);
        }

        public void Clear()
        {
            //Add SetUpContracts.Clear implementation
            _setUpContracts.Clear();
        }

        public int IndexOf(object setUpContract)
        {
            //Add SetUpContracts.IndexOf implementation
            return _setUpContracts.IndexOf((SetUpContract)setUpContract);
        }

        public int Add(object setUpContract)
        {
            //Add SetUpContracts.Add implementation
            return _setUpContracts.Add((SetUpContract)setUpContract);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add SetUpContracts.IsFixedSize getter implementation
                return _setUpContracts.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add SetUpContracts.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _setUpContracts.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _setUpContracts.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _setUpContracts.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new SetUpContractEnumerator(this));
        }

        #endregion

        #region ClientEnumerator Class

        public class SetUpContractEnumerator : IEnumerator
        {
            SetUpContracts _setUpContracts;
            int _location;

            public SetUpContractEnumerator(SetUpContracts setUpContracts)
            {
                _setUpContracts = setUpContracts;
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
                    if ((_location < 0) || (_location >= _setUpContracts.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _setUpContracts[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _setUpContracts.Count)
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
