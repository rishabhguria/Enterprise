using System;
using System.Collections;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ThirdParties.
    /// </summary>
    public class ThirdPartyFileFormats : IList
    {
        ArrayList _thirdPartyFileFormats = new ArrayList();

        public ThirdPartyFileFormats()
        {

        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _thirdPartyFileFormats.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ThirdParties.this getter implementation
                return _thirdPartyFileFormats[index];
            }
            set
            {
                //Add ThirdParties.this setter implementation
                _thirdPartyFileFormats[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ThirdParties.RemoveAt implementation
            _thirdPartyFileFormats.RemoveAt(index);
        }

        public void Insert(int index, Object thirdPartyFileFormat)
        {
            //Add ThirdParties.Insert implementation
            _thirdPartyFileFormats.Insert(index, (ThirdPartyFileFormat)thirdPartyFileFormat);
        }

        public void Remove(Object thirdPartyFileFormat)
        {
            //Add ThirdParties.Remove implementation
            _thirdPartyFileFormats.Remove((ThirdPartyFileFormat)thirdPartyFileFormat);
        }

        public bool Contains(object thirdPartyFileFormat)
        {
            //Add ThirdParties.Contains implementation
            return _thirdPartyFileFormats.Contains((ThirdPartyFileFormat)thirdPartyFileFormat);
        }

        public void Clear()
        {
            //Add ThirdParties.Clear implementation
            _thirdPartyFileFormats.Clear();
        }

        public int IndexOf(object thirdPartyFileFormat)
        {
            //Add ThirdParties.IndexOf implementation
            return _thirdPartyFileFormats.IndexOf((ThirdPartyFileFormat)thirdPartyFileFormat);
        }

        public int Add(object thirdPartyFileFormat)
        {
            //Add ThirdParties.Add implementation
            return _thirdPartyFileFormats.Add((ThirdPartyFileFormat)thirdPartyFileFormat);
        }

        public void AddRange(List<ThirdPartyFileFormat> thirdPartyFileFormats)
        {
            _thirdPartyFileFormats.AddRange(thirdPartyFileFormats);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ThirdParties.IsFixedSize getter implementation
                return _thirdPartyFileFormats.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ThirdParties.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _thirdPartyFileFormats.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _thirdPartyFileFormats.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _thirdPartyFileFormats.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ThirdPartyFileFormatEnumerator(this));
        }

        #endregion

        #region ThirdPartyEnumerator Class

        public class ThirdPartyFileFormatEnumerator : IEnumerator
        {
            ThirdPartyFileFormats _thirdPartyFileFormats;
            int _location;

            public ThirdPartyFileFormatEnumerator(ThirdPartyFileFormats thirdPartyFileFormats)
            {
                _thirdPartyFileFormats = thirdPartyFileFormats;
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
                    if ((_location < 0) || (_location >= _thirdPartyFileFormats.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _thirdPartyFileFormats[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _thirdPartyFileFormats.Count)
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
