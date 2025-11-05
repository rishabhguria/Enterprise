using Prana.BusinessObjects;
using System;
using System.Collections;


namespace Prana.Admin.BLL
{
    public class ThirdPartyFlatFileSaveDetails : IList
    {
        ArrayList _thirdPartyFlatFileSaveDetails = new ArrayList();

        public ThirdPartyFlatFileSaveDetails()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _thirdPartyFlatFileSaveDetails.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add ThirdPartyFlatFileSaveDetails.this getter implementation
                return _thirdPartyFlatFileSaveDetails[index];
            }
            set
            {
                //Add ThirdPartyFlatFileSaveDetails.this setter implementation
                _thirdPartyFlatFileSaveDetails[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add ThirdPartyFlatFileSaveDetails.RemoveAt implementation
            _thirdPartyFlatFileSaveDetails.RemoveAt(index);
        }

        public void Insert(int index, Object thirdPartyFlatFileSaveDetail)
        {
            //Add ThirdPartyFlatFileSaveDetails.Insert implementation
            _thirdPartyFlatFileSaveDetails.Insert(index, (ThirdPartyFlatFileSaveDetail)thirdPartyFlatFileSaveDetail);
        }

        public void Remove(Object thirdPartyFlatFileSaveDetail)
        {
            //Add ThirdPartyFlatFileSaveDetails.Remove implementation
            _thirdPartyFlatFileSaveDetails.Remove((ThirdPartyFlatFileSaveDetail)thirdPartyFlatFileSaveDetail);
        }

        public bool Contains(object thirdPartyFlatFileSaveDetail)
        {
            //Add ThirdPartyFlatFileSaveDetails.Contains implementation
            return _thirdPartyFlatFileSaveDetails.Contains((ThirdPartyFlatFileSaveDetail)thirdPartyFlatFileSaveDetail);
        }

        public void Clear()
        {
            //Add ThirdPartyFlatFileSaveDetails.Clear implementation
            _thirdPartyFlatFileSaveDetails.Clear();
        }

        public int IndexOf(object thirdPartyFlatFileSaveDetail)
        {
            //Add ThirdPartyFlatFileSaveDetails.IndexOf implementation
            return _thirdPartyFlatFileSaveDetails.IndexOf((ThirdPartyFlatFileSaveDetail)thirdPartyFlatFileSaveDetail);
        }

        public int Add(object thirdPartyFlatFileSaveDetail)
        {
            //Add ThirdPartyFlatFileSaveDetails.Add implementation
            return _thirdPartyFlatFileSaveDetails.Add((ThirdPartyFlatFileSaveDetail)thirdPartyFlatFileSaveDetail);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add ThirdPartyFlatFileSaveDetails.IsFixedSize getter implementation
                return _thirdPartyFlatFileSaveDetails.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ThirdPartyFlatFileSaveDetails.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _thirdPartyFlatFileSaveDetails.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _thirdPartyFlatFileSaveDetails.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _thirdPartyFlatFileSaveDetails.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ThirdPartyFlatFileSaveDetailEnumerator(this));
        }

        #endregion

        #region ThirdPartyFlatFileSaveDetailEnumerator Class

        public class ThirdPartyFlatFileSaveDetailEnumerator : IEnumerator
        {
            ThirdPartyFlatFileSaveDetails _thirdPartyFlatFileSaveDetails;
            int _location;

            public ThirdPartyFlatFileSaveDetailEnumerator(ThirdPartyFlatFileSaveDetails thirdPartyFlatFileSaveDetails)
            {
                _thirdPartyFlatFileSaveDetails = thirdPartyFlatFileSaveDetails;
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
                    if ((_location < 0) || (_location >= _thirdPartyFlatFileSaveDetails.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _thirdPartyFlatFileSaveDetails[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _thirdPartyFlatFileSaveDetails.Count)
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
