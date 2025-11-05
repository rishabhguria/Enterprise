using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for FileFormats.
    /// </summary>
    public class FileFormats : IList
    {
        ArrayList _fileFormats = new ArrayList();
        public FileFormats()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _fileFormats.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                return _fileFormats[index];
            }
            set
            {
                //Add Users.this setter implementation
                _fileFormats[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _fileFormats.RemoveAt(index);
        }

        public void Insert(int index, Object fileformat)
        {
            //Add Users.Insert implementation
            _fileFormats.Insert(index, (FileFormat)fileformat);
        }

        public void Remove(Object fileformat)
        {
            //Add Users.Remove implementation
            _fileFormats.Remove((FileFormat)fileformat);
        }

        public bool Contains(object fileformat)
        {
            //Add Users.Contains implementation
            return _fileFormats.Contains((FileFormat)fileformat);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _fileFormats.Clear();
        }

        public int IndexOf(object fileformat)
        {
            //Add Users.IndexOf implementation
            return _fileFormats.IndexOf((FileFormat)fileformat);
        }

        public int Add(object fileformat)
        {
            //Add Users.Add implementation
            return _fileFormats.Add((FileFormat)fileformat);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _fileFormats.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Fileformats.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _fileFormats.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _fileFormats.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _fileFormats.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new FileFormatEnumerator(this));
        }

        #endregion

        #region FileFormatEnumerator Class

        public class FileFormatEnumerator : IEnumerator
        {
            FileFormats _fileFormats;
            int _location;

            public FileFormatEnumerator(FileFormats fileformats)
            {
                _fileFormats = fileformats;
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
                    if ((_location < 0) || (_location >= _fileFormats.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _fileFormats[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _fileFormats.Count)
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
