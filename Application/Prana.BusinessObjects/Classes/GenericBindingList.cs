using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class GenericBindingList<T> : BindingList<T> where T : class, IKeyable, INotifyPropertyChangedCustom
    {
        Dictionary<string, T> _dict = new Dictionary<string, T>();
        List<T> _list = new List<T>();
        public void AddList(List<T> list)
        {
            if (list != null)
            {
                foreach (T item in list)
                {
                    if (item != null)
                    {
                        Add(item);
                    }
                }
            }
        }
        public List<T> GetList()
        {
            return _list = new List<T>(_dict.Values);
        }

        // addding new keyword as we have custom def
        new public void Add(T item)
        {
            if (item != null)
            {
                String key = item.GetKey();
                if (!_dict.ContainsKey(key))
                {
                    base.Add(item);
                    _dict.Add(key, item);
                }
            }
        }
        public T GetItem(string key)
        {

            if (_dict.ContainsKey(key))
            {
                return _dict[key];
            }
            else
            {
                return null;
            }
        }
        // addding new keyword as we have custom def
        new public void Remove(T item)
        {
            if (item != null)
            {
                if (_dict.ContainsKey(item.GetKey()))
                {
                    T olditem = GetItem(item.GetKey());
                    base.Remove(olditem);
                    _dict.Remove(item.GetKey());

                }
            }
        }

        public void Remove(string key)
        {
            if (_dict.ContainsKey(key))
            {
                T olditem = GetItem(key);
                // int index = _list.BinarySearch(olditem);
                // base.RemoveAt(index);
                //_list.RemoveAt(index);
                base.Remove(olditem);
                _dict.Remove(key);
            }
        }
        public void Update(T newItem)
        {
            if (newItem != null)
            {
                String key = newItem.GetKey();
                if (_dict.ContainsKey(key))
                {
                    T oldItem = _dict[key];
                    oldItem.Update(newItem);
                    RaisepropertyChanged(oldItem);
                }
            }
        }
        // addding new keyword as we have custom def
        new public bool Contains(T item)
        {
            if (item != null)
            {
                if (_dict.ContainsKey(item.GetKey()))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public bool ContainsKey(string key)
        {
            if (_dict.ContainsKey(key))
            {
                return true;
            }
            else
                return false;
        }

        // addding new keyword as we have custom def
        new public void Clear()
        {
            base.Clear();
            _dict.Clear();
        }
        public void UpdateItem(T newItem)
        {
            if (newItem != null)
            {
                String key = newItem.GetKey();
                if (_dict.ContainsKey(key))
                {
                    T oldItem = _dict[key];
                    int index = base.IndexOf(oldItem);
                    base.Remove(oldItem);
                    base.InsertItem(index, newItem);
                    _dict[key] = newItem;

                }
            }
        }

        private void RaisepropertyChanged(T Item)
        {
            if (Item != null)
            {
                Item.PropertyHasChanged();
            }
        }
    }
    //public interface IKeyable
    //{
    //    String GetKey();
    //    void Update(IKeyable item);
    //}
}
