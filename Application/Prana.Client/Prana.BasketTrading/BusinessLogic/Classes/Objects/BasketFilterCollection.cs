using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;


namespace Prana.BasketTrading
{
    public class BasketFilterCollection:IList<BasketFilter>
    {

        List<BasketFilter> _basketFilterCollection = new List<BasketFilter>(); 
        #region IList<BasketFilter> Members

        public int IndexOf(BasketFilter basketfilter)
        {
            return _basketFilterCollection.IndexOf(basketfilter);
        }

        public void Insert(int index, BasketFilter basketfilter)
        {
            _basketFilterCollection.Insert(index, basketfilter);
        }

        public void RemoveAt(int index)
        {
            _basketFilterCollection.RemoveAt(index);
        }

        public BasketFilter this[int index]
        {
            get
            {
                return _basketFilterCollection[index];
            }
            set
            {
                _basketFilterCollection[index] = value;
            }
        }

        #endregion

        #region ICollection<BasketFilter> Members

        public void Add(BasketFilter basketfilter)
        {
            _basketFilterCollection.Add(basketfilter); 
        }

        public void Clear()
        {
            _basketFilterCollection.Clear();
        }

        public bool Contains(BasketFilter basketfilter)
        {
            return _basketFilterCollection.Remove(basketfilter);
        }

        public void CopyTo(BasketFilter[] array, int arrayIndex)
        {
            _basketFilterCollection.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _basketFilterCollection.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(BasketFilter basketfilter)
        {
            return _basketFilterCollection.Remove(basketfilter);

        }

        #endregion

        #region IEnumerable<BasketFilter> Members

        public IEnumerator<BasketFilter> GetEnumerator()
        {
            return (new BasketFilterEnumerator1(this));

        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (new BasketFilterEnumerator(this));
        }

        #endregion

        public BasketFilter GetBasketFilterByID(string BasketFilterID)
        {
            BasketFilter BasketFilter = null;
            foreach (BasketFilter temp in _basketFilterCollection)
            {
                if (temp.FilterID.Equals(BasketFilterID))
                {
                    BasketFilter = temp;
                    break;
                }
            }
            return BasketFilter;
        }

        public class BasketFilterEnumerator : IEnumerator
        {
            BasketFilterCollection _BasketFilter;
            int _location;

            public BasketFilterEnumerator(BasketFilterCollection itemValuesCollection)
            {
                _BasketFilter = itemValuesCollection;
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
                    if ((_location < 0) || (_location >= _BasketFilter.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _BasketFilter[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _BasketFilter.Count)
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
        public sealed class BasketFilterEnumerator1 : IEnumerator<BasketFilter>
        {
            BasketFilterCollection _BasketFiltercollection;
            int _location;
            public BasketFilterEnumerator1(BasketFilterCollection itemValuesCollection)
            {
                _BasketFiltercollection = itemValuesCollection;
                _location = -1;
            }
            public void Reset()
            {
                _location = -1;
            }
            public bool MoveNext()
            {
                _location++;

                if (_location >= _BasketFiltercollection.Count)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
            object IEnumerator.Current
            {
                get
                {
                    if ((_location < 0) || (_location >= _BasketFiltercollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _BasketFiltercollection[_location];
                    }
                }
            }
            public BasketFilter Current
            {
                get
                {
                    if ((_location < 0) || (_location >= _BasketFiltercollection.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _BasketFiltercollection[_location];
                    }
                }
            }
			public void Dispose()
            {
                try
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
            }

            private void Dispose(bool disposing)
            {
			if(disposing){}
            }
        }
    }
}
