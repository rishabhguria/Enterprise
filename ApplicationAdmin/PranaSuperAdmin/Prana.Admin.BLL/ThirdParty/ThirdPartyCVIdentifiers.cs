using System;
using System.Collections;


namespace Prana.Admin.BLL
{
    public class ThirdPartyCVIdentifiers : IList
    {

        ArrayList _thirdPartyCVIdentifiers = new ArrayList();

        public ThirdPartyCVIdentifiers()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _thirdPartyCVIdentifiers.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ThirdPartyCVIdentifiers.this getter implementation
                return _thirdPartyCVIdentifiers[index];
            }
            set
            {
                //Add ThirdPartyCVIdentifiers.this setter implementation
                _thirdPartyCVIdentifiers[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ThirdPartyCVIdentifiers.RemoveAt implementation
            _thirdPartyCVIdentifiers.RemoveAt(index);
        }

        public void Insert(int index, Object thirdPartyCVIdentifier)
        {
            //Add ThirdPartyCVIdentifiers.Insert implementation
            _thirdPartyCVIdentifiers.Insert(index, (ThirdPartyCVIdentifier)thirdPartyCVIdentifier);
        }

        public void Remove(Object thirdPartyCVIdentifier)
        {
            //Add ThirdPartyCVIdentifiers.Remove implementation
            _thirdPartyCVIdentifiers.Remove((ThirdPartyCVIdentifier)thirdPartyCVIdentifier);
        }

        public bool Contains(object thirdPartyCVIdentifier)
        {
            //Add ThirdPartyCVIdentifiers.Contains implementation
            return _thirdPartyCVIdentifiers.Contains((ThirdPartyCVIdentifier)thirdPartyCVIdentifier);
        }

        public void Clear()
        {
            //Add ThirdPartyCVIdentifiers.Clear implementation
            _thirdPartyCVIdentifiers.Clear();
        }

        public int IndexOf(object thirdPartyCVIdentifier)
        {
            //Add ThirdPartyCVIdentifiers.IndexOf implementation
            return _thirdPartyCVIdentifiers.IndexOf((ThirdPartyCVIdentifier)thirdPartyCVIdentifier);
        }

        public int Add(object thirdPartyCVIdentifier)
        {
            //Add ThirdPartyCVIdentifiers.Add implementation
            return _thirdPartyCVIdentifiers.Add((ThirdPartyCVIdentifier)thirdPartyCVIdentifier);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ThirdPartyCVIdentifiers.IsFixedSize getter implementation
                return _thirdPartyCVIdentifiers.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ThirdPartyCVIdentifiers.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _thirdPartyCVIdentifiers.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _thirdPartyCVIdentifiers.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _thirdPartyCVIdentifiers.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ThirdPartyCVIdentifierEnumerator(this));
        }

        #endregion

        #region ThirdPartyCVIdentifierEnumerator Class

        public class ThirdPartyCVIdentifierEnumerator : IEnumerator
        {
            ThirdPartyCVIdentifiers _thirdPartyCVIdentifiers;
            int _location;

            public ThirdPartyCVIdentifierEnumerator(ThirdPartyCVIdentifiers thirdPartyCVIdentifiers)
            {
                _thirdPartyCVIdentifiers = thirdPartyCVIdentifiers;
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
                    if ((_location < 0) || (_location >= _thirdPartyCVIdentifiers.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _thirdPartyCVIdentifiers[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _thirdPartyCVIdentifiers.Count)
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
