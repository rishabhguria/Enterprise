using Prana.BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Utilities
{
    public static class PranaExtensionHelper
    {
        public static void AddThreadSafely<T>(this ICollection<T> collection, T item)
        {
            if (collection != null)
            {
                bool locked = false;
                try
                {
                    System.Threading.Monitor.Enter(collection, ref locked);
                    collection.Add(item);
                }
                finally
                {
                    if (locked)
                        System.Threading.Monitor.Exit(collection);
                }
            }
        }

        public static void AddRangeThreadSafely<T>(this ICollection<T> collection, IEnumerable<T> enumerableItem)
        {
            if (collection != null)
            {
                bool locked = false;
                try
                {
                    System.Threading.Monitor.Enter(collection, ref locked);
                    foreach (var item in enumerableItem)
                    {
                        collection.Add(item);
                    }
                }
                finally
                {
                    if (locked)
                        System.Threading.Monitor.Exit(collection);
                }
            }
        }

        public static void ClearThreadSafely<T>(this ICollection<T> collection)
        {
            if (collection != null)
            {
                bool locked = false;
                try
                {
                    System.Threading.Monitor.Enter(collection, ref locked);
                    collection.Clear();
                }
                finally
                {
                    if (locked)
                        System.Threading.Monitor.Exit(collection);
                }
            }
        }

        public static List<TaxLot> Clone(this IList<TaxLot> listToClone)
        {
            return listToClone.Select(item => item.Clone()).Cast<TaxLot>().ToList();
        }
    }
}
