using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for SymbolMappings.
    /// </summary>
    public class SymbolMappings : IList
    {
        ArrayList _symbolMappings = new ArrayList();
        public SymbolMappings()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _symbolMappings.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add SymbolMappings.this getter implementation

                if (!(index < 0))
                {
                    return _symbolMappings[index];
                }
                return null;
            }
            set
            {
                //Add SymbolMappings.this setter implementation
                _symbolMappings[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add SymbolMappings.RemoveAt implementation
            _symbolMappings.RemoveAt(index);
        }

        public void Insert(int index, Object symbolMapping)
        {
            //Add SymbolMappings.Insert implementation
            _symbolMappings.Insert(index, (SymbolMapping)symbolMapping);
        }

        public void Remove(Object symbolMapping)
        {
            //Add SymbolMappings.Remove implementation
            _symbolMappings.Remove((SymbolMapping)symbolMapping);
        }

        public bool Contains(object symbolMapping)
        {
            //Add SymbolMappings.Contains implementation
            return _symbolMappings.Contains((SymbolMapping)symbolMapping);
        }

        public void Clear()
        {
            //Add SymbolMappings.Clear implementation
            _symbolMappings.Clear();
        }

        public int IndexOf(object symbolMapping)
        {
            //Add SymbolMappings.IndexOf implementation
            //			return _symbolMappings.IndexOf((SymbolMapping)symbolMapping);
            SymbolMapping tempSymbolMapping = (SymbolMapping)symbolMapping;
            int counter = 0;
            int result = int.MinValue;
            foreach (SymbolMapping _symbolMapping in _symbolMappings)
            {
                if (_symbolMapping.CVAUECID == tempSymbolMapping.CVAUECID
                    && _symbolMapping.CounterPartyVenueID == tempSymbolMapping.CounterPartyVenueID
                    && _symbolMapping.MappedSymbol == tempSymbolMapping.MappedSymbol
                    && _symbolMapping.CVSymboMappingID == tempSymbolMapping.CVSymboMappingID
                    && _symbolMapping.Symbol == tempSymbolMapping.Symbol
                    //&& _symbolMapping.SymbolName == tempSymbolMapping.SymbolName
                    && _symbolMapping.AUEC == tempSymbolMapping.AUEC
                    && _symbolMapping.AUECID == tempSymbolMapping.AUECID)
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

        public int Add(object symbolMapping)
        {
            //Add SymbolMappings.Add implementation
            return _symbolMappings.Add((SymbolMapping)symbolMapping);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add SymbolMappings.IsFixedSize getter implementation
                return _symbolMappings.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add SymbolMappings.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _symbolMappings.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _symbolMappings.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _symbolMappings.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new SymbolMappingEnumerator(this));
        }

        #endregion

        #region AssetEnumerator Class

        public class SymbolMappingEnumerator : IEnumerator
        {
            SymbolMappings _symbolMappings;
            int _location;

            public SymbolMappingEnumerator(SymbolMappings symbolMappings)
            {
                _symbolMappings = symbolMappings;
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
                    if ((_location < 0) || (_location >= _symbolMappings.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _symbolMappings[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _symbolMappings.Count)
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
