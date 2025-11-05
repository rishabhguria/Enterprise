using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class GenericRepository<T> : List<T> where T : class, IKeyable
    {
        Dictionary<string, T> _dict = new Dictionary<string, T>();
        // addding new keyword as we have custom def
        new public virtual void Add(T item)
        {
            _dict.Add(item.GetKey(), item);
            base.Add(item);
        }
        public virtual T GetItem(string key)
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
        new public virtual void Remove(T item)
        {
            _dict.Remove(item.GetKey());
        }
        public virtual void Update(T newItem)
        {
            String key = newItem.GetKey();
            if (_dict.ContainsKey(key))
            {
                T oldItem = _dict[key];
                oldItem.Update(newItem);
            }
        }

    }
    public interface IKeyable
    {
        String GetKey();
        void Update(IKeyable item);

    }
}
