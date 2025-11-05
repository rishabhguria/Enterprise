using System.Collections.Generic;

namespace Prana.BasketProcessor
{
    /// <summary>
    /// It is used for managing cache.
    /// </summary>
    public class BasketCacheManager
    {
        static Dictionary<string, string> _basketData = new Dictionary<string, string>();
        private static BasketCacheManager _basketCacheMgr;
        static BasketCacheManager()
        {
            _basketCacheMgr = new BasketCacheManager();
        }
        /// <summary>
        /// Gets the instance of BasketCacheManager.
        /// </summary>        
        public static BasketCacheManager GetInstance()
        {
            return _basketCacheMgr;
        }
        /// <summary>
        /// Check whether the basket with specified BasketID exists or not.
        /// </summary>
        /// <param name="basketID"></param>        
        public static bool DoesBasketExist(string basketID)
        {
            if (_basketData.ContainsKey(basketID))

                return true;
            else
                return false;
        }
        /// <summary>
        /// Fetches traded basket id.
        /// </summary>
        /// <param name="basketID"></param>        
        public static string GetTradedBasketID(string basketID)
        {
            if (_basketData.ContainsKey(basketID))
            {
                return _basketData[basketID];
            }
            else
                return string.Empty;

        }
        /// <summary>
        /// Add Basket
        /// </summary>
        /// <param name="basketID"></param>
        /// <param name="tradedbasketID"></param>
        public static void AddBasket(string basketID, string tradedbasketID)
        {
            if (!_basketData.ContainsKey(basketID))
                _basketData.Add(basketID, tradedbasketID);
        }


    }
}
