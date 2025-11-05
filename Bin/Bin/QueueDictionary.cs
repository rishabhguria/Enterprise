using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Utilities
{
    public class QueueDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private Queue<TKey> innerQueue = new Queue<TKey>();
        private object padlock = new object();

        public QueueDictionary()
            : base()
        {
        }

        new public void Add(TKey key, TValue value)
        {
            lock (padlock)
            {
                base.Add(key, value);
                innerQueue.Enqueue(key);
            }
        }

        new public bool Remove(TKey key)
        {
            lock (padlock)
            {
                return base.Remove(key);
            }
        }

        public TValue Peek()
        {
            lock (padlock)
            {
                if (innerQueue.Count == 0)
                    return default(TValue);

                TKey key = innerQueue.Peek();
                while (innerQueue.Count > 0 && !base.ContainsKey(key))
                {
                    innerQueue.Dequeue();
                    key = innerQueue.Peek();
                }

                return base.ContainsKey(key) ? base[key] : default(TValue);
            }
        }

        public TValue Dequeue()
        {
            lock (padlock)
            {
                if (innerQueue.Count == 0)
                    return default(TValue);

                TKey key = innerQueue.Dequeue();
                while (innerQueue.Count > 0 && !base.ContainsKey(key))
                {
                    key = innerQueue.Dequeue();
                }

                if (base.ContainsKey(key))
                {
                    TValue ret = base[key];
                    base.Remove(key);
                    return ret;
                }
                else
                {
                    return default(TValue);
                }
            }
        }
    }

}
