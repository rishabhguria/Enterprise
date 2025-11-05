using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Logos.
    /// </summary>
    public class Logos : IList
    {
        ArrayList _logos = new ArrayList();
        public Logos()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _logos.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Logos.this getter implementation
                return _logos[index];
            }
            set
            {
                //Add Logos.this setter implementation
                _logos[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Logos.RemoveAt implementation
            _logos.RemoveAt(index);
        }

        public void Insert(int index, Object logo)
        {
            //Add Logos.Insert implementation
            _logos.Insert(index, (Logo)logo);
        }

        public void Remove(Object logo)
        {
            //Add Logos.Remove implementation
            _logos.Remove((Logo)logo);
        }

        public bool Contains(object logo)
        {
            //Add Logos.Contains implementation
            return _logos.Contains((Logo)logo);
        }

        public void Clear()
        {
            //Add Logos.Clear implementation
            _logos.Clear();
        }

        public int IndexOf(object logo)
        {
            //Add Logos.IndexOf implementation
            return _logos.IndexOf((Logo)logo);
        }

        public int Add(object logo)
        {
            //Add Logos.Add implementation
            return _logos.Add((Logo)logo);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Logos.IsFixedSize getter implementation
                return _logos.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Logos.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _logos.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _logos.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _logos.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new LogoEnumerator(this));
        }

        #endregion

        #region LogoEnumerator Class

        public class LogoEnumerator : IEnumerator
        {
            Logos _logos;
            int _location;

            public LogoEnumerator(Logos logos)
            {
                _logos = logos;
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
                    if ((_location < 0) || (_location >= _logos.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _logos[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _logos.Count)
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
