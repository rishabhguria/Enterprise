using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Modules.
    /// </summary>
    public class Modules : IList
    {
        ArrayList _modules = new ArrayList();

        public Modules()
        {
        }
        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _modules.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Modules.this getter implementation
                return _modules[index];
            }
            set
            {
                //Add Modules.this setter implementation
                _modules[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Modules.RemoveAt implementation
            _modules.RemoveAt(index);
        }

        public void Insert(int index, Object module)
        {
            //Add Modules.Insert implementation
            _modules.Insert(index, (Module)module);
        }

        public void Remove(Object module)
        {
            //Add Modules.Remove implementation
            _modules.Remove((Module)module);
        }

        public bool Contains(object module)
        {
            //Add Modules.Contains implementation
            return _modules.Contains((Module)module);
        }

        public void Clear()
        {
            //Add Modules.Clear implementation
            _modules.Clear();
        }

        public int IndexOf(object module)
        {
            //Add Modules.IndexOf implementation
            return _modules.IndexOf((Module)module);
        }

        public int Add(object module)
        {
            //Add Modules.Add implementation
            return _modules.Add((Module)module);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Modules.IsFixedSize getter implementation
                return _modules.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Modules.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _modules.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _modules.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _modules.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new ModuleEnumerator(this));
        }

        #endregion

        #region ModuleEnumerator Class

        public class ModuleEnumerator : IEnumerator
        {
            Modules _modules;
            int _location;

            public ModuleEnumerator(Modules modules)
            {
                _modules = modules;
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
                    if ((_location < 0) || (_location >= _modules.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _modules[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _modules.Count)
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
