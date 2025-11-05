using System;
using System.Collections;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Assets.
    /// </summary>
    public class Assets : IList
    {
        ArrayList _assets = new ArrayList();

        public Assets()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _assets.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Assets.this getter implementation
                return _assets[index];
            }
            set
            {
                //Add Assets.this setter implementation
                _assets[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Assets.RemoveAt implementation
            _assets.RemoveAt(index);
        }

        public void Insert(int index, Object asset)
        {
            //Add Assets.Insert implementation
            _assets.Insert(index, (Asset)asset);
        }

        public void Remove(Object asset)
        {
            //Add Assets.Remove implementation
            _assets.Remove((Asset)asset);
        }

        public bool Contains(object asset)
        {
            //Add Assets.Contains implementation
            return _assets.Contains((Asset)asset);
        }

        public void Clear()
        {
            //Add Assets.Clear implementation
            _assets.Clear();
        }

        public int IndexOf(object asset)
        {
            //Add Assets.IndexOf implementation
            return _assets.IndexOf((Asset)asset);
        }

        public int Add(object asset)
        {
            //Add Assets.Add implementation
            return _assets.Add((Asset)asset);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Assets.IsFixedSize getter implementation
                return _assets.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Assets.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _assets.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _assets.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _assets.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new AssetEnumerator(this));
        }

        #endregion

        #region AssetEnumerator Class

        public class AssetEnumerator : IEnumerator
        {
            Assets _assets;
            int _location;

            public AssetEnumerator(Assets assets)
            {
                _assets = assets;
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
                    if ((_location < 0) || (_location >= _assets.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _assets[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _assets.Count)
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
