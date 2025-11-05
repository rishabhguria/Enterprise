using System;
using System.Collections;

namespace Prana.PM.Client
{
    /// <summary>
    /// Accurals Parameters Collection
    /// </summary>    

    public class clsAccruals : IList
    {
        ArrayList _accruals = new ArrayList();

        public clsAccruals()
        {

        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _accruals.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {

                return _accruals[index];
            }
            set
            {

                _accruals[index] = value;
            }
        }

        public void RemoveAt(int index)
        {

            _accruals.RemoveAt(index);
        }

        public void Insert(int index, Object accrual)
        {

            _accruals.Insert(index, (clsAccrual)accrual);
        }

        public void Remove(Object accrual)
        {

            _accruals.Remove((clsAccrual)accrual);
        }

        public bool Contains(object accrual)
        {

            return _accruals.Contains((clsAccrual)accrual);
        }

        public void Clear()
        {
            //Add ThirdParties.Clear implementation
            _accruals.Clear();
        }

        public int IndexOf(object accrual)
        {
            //Add ThirdParties.IndexOf implementation
            return _accruals.IndexOf((clsAccrual)accrual);
        }

        public int Add(object accrual)
        {
            return _accruals.Add((clsAccrual)accrual);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add clsAccrual.IsFixedSize getter implementation
                return _accruals.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Accruals.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _accruals.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _accruals.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _accruals.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new clsAccrualEnumerator(this));
        }

        #endregion

        #region AccrualsEnumerator Class

        public class clsAccrualEnumerator : IEnumerator
        {
            clsAccruals _accruals;
            int _location;

            public clsAccrualEnumerator(clsAccruals accruals)
            {
                _accruals = accruals;
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
                    if ((_location < 0) || (_location >= _accruals.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _accruals[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _accruals.Count)
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
