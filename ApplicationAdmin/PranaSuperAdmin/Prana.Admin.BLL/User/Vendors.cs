using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Vendors.
    /// </summary>
    public class Vendors : IList
    {
        ArrayList _vendors = new ArrayList();

        public Vendors()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _vendors.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Vendor.this getter implementation
                return _vendors[index];
            }
            set
            {
                //Add Vendor.this setter implementation
                _vendors[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Vendor.RemoveAt implementation
            _vendors.RemoveAt(index);
        }

        public void Insert(int index, Object vendor)
        {
            //Add Vendor.Insert implementation
            _vendors.Insert(index, (Vendor)vendor);
        }

        public void Remove(Object vendor)
        {
            //Add Vendor.Remove implementation
            _vendors.Remove((Vendor)vendor);
        }

        public bool Contains(object vendor)
        {
            //Add Vendor.Contains implementation
            return _vendors.Contains((Vendor)vendor);
        }

        public void Clear()
        {
            //Add Vendor.Clear implementation
            _vendors.Clear();
        }

        public int IndexOf(object vendor)
        {
            //Add Vendor.IndexOf implementation
            return _vendors.IndexOf((Vendor)vendor);
        }

        public int Add(object vendor)
        {
            //Add Vendor.Add implementation
            return _vendors.Add((Vendor)vendor);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Vendor.IsFixedSize getter implementation
                return _vendors.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Vendor.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _vendors.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _vendors.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _vendors.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new VendorEnumerator(this));
        }

        #endregion

        #region VendorEnumerator Class

        public class VendorEnumerator : IEnumerator
        {
            Vendors _vendors;
            int _location;

            public VendorEnumerator(Vendors vendors)
            {
                _vendors = vendors;
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
                    if ((_location < 0) || (_location >= _vendors.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _vendors[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _vendors.Count)
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
