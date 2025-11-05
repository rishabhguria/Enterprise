//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Text;

//namespace Prana.BasketTrading 
//{
//    public class BasketTemplates :CollectionBase,IList 
//    {
//        ArrayList _basketTemplates = new ArrayList();
//        #region IList Members

//        public bool IsReadOnly
//        {
//            get
//            {
//                return _basketTemplates.IsReadOnly;
//                //return false;
//            }
//        }

//        public object this[int index]
//        {
//            get
//            {
//                //Add CustomBlotterCollection.this getter implementation
//                return _basketTemplates[index];
//            }
//            set
//            {
//                //Add CustomBlotterCollection.this setter implementation
//                _basketTemplates[index] = value;
//            }
//        }

       

//        public void RemoveAt(int index)
//        {
//            //Add CustomBlotterCollection.RemoveAt implementation
//            _basketTemplates.RemoveAt(index);
//        }

//        public void Insert(int index, Object customBlotter)
//        {
//            //Add CustomBlotterCollection.Insert implementation
//            _basketTemplates.Insert(index, (BasketTemplate )customBlotter);
//        }

//        public void Remove(Object customBlotter)
//        {
//            //Add CustomBlotterCollection.Remove implementation
//            _basketTemplates.Remove((CustomBlotter)customBlotter);
//        }

//        public bool Contains(object customBlotter)
//        {
//            //Add CustomBlotterCollection.Contains implementation
//            return _basketTemplates.Contains((CustomBlotter)customBlotter);
//        }

//        public void Clear()
//        {
//            //Add CustomBlotterCollection.Clear implementation
//            _basketTemplates.Clear();
//        }

//        public int IndexOf(object customBlotter)
//        {
//            //Add CustomBlotterCollection.IndexOf implementation
//            return _basketTemplates.IndexOf((CustomBlotter)customBlotter);
//        }

//        public int Add(object customBlotter)
//        {
//            //Add CustomBlotterCollection.Add implementation
//            return _basketTemplates.Add((CustomBlotter)customBlotter);
//        }

//        public bool IsFixedSize
//        {
//            get
//            {
//                //Add CustomBlotterCollection.IsFixedSize getter implementation
//                return _basketTemplates.IsFixedSize;
//            }
//        }

//        #endregion

//        #region ICollection Members

//        public bool IsSynchronized
//        {
//            get
//            {
//                // TODO:  Add CustomBlotterCollection.IsSynchronized getter implementation
//                return false;
//            }
//        }

//        public int Count
//        {
//            get
//            {
//                return _basketTemplates.Count;
//            }
//        }

//        public void CopyTo(Array array, int index)
//        {
//            _basketTemplates.CopyTo(array, index);
//        }

//        public object SyncRoot
//        {
//            get
//            {
//                return _basketTemplates.SyncRoot;
//                //return null;
//            }
//        }

//        #endregion

//        #region IEnumerable Members

//        public IEnumerator GetEnumerator()
//        {
//            return (new CustomBlotterEnumerator(this));
//        }

//        #endregion

//        #region AssetEnumerator Class

//        public class CustomBlotterEnumerator : IEnumerator
//        {
//            CustomBlotterCollection _basketTemplates;
//            int _location;

//            public CustomBlotterEnumerator(CustomBlotterCollection customBlotterCollection)
//            {
//                _basketTemplates = customBlotterCollection;
//                _location = -1;
//            }

//            #region IEnumerator Members
//            public void Reset()
//            {
//                _location = -1;
//            }
//            public object Current
//            {
//                get
//                {
//                    if ((_location < 0) || (_location >= _basketTemplates.Count))
//                    {
//                        throw (new InvalidOperationException());
//                    }
//                    else
//                    {
//                        return _basketTemplates[_location];
//                    }
//                }
//            }

//            public bool MoveNext()
//            {
//                _location++;

//                if (_location >= _basketTemplates.Count)
//                {
//                    return false;
//                }
//                else
//                {
//                    return true;
//                }
//            }
//            #endregion
//        }

//        #endregion

//    }
//}
