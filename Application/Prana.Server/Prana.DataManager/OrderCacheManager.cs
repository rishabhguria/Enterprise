using Prana.BusinessObjects;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.DataManager
{
    internal class OrderCacheManager
    {
        private static readonly OrderCacheManager instance = new OrderCacheManager();

        internal static OrderCacheManager Instance
        {
            get { return instance; }
        }

        private readonly object locker = new object();
        private Queue<Order> orderFillCache = new Queue<Order>();

        internal void Add(Order order)
        {
            try
            {
                lock (locker)
                {
                    orderFillCache.Enqueue(order);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal Queue<Order> GetOrders()
        {
            Queue<Order> tempColl = null;
            try
            {
                lock (locker)
                {
                    tempColl = DeepCopyHelper.Clone(orderFillCache);

                    orderFillCache.Clear();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return tempColl;
        }
    }
}
