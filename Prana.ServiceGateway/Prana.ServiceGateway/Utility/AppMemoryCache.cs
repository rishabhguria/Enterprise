using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Prana.ServiceGateway.Utility
{
    /// <summary>  
    /// Provides a wrapper around the IMemoryCache to simplify caching operations.  
    /// </summary>  
    public class AppMemoryCache
    {
        private readonly IMemoryCache _cache;

        /// <summary>  
        /// Initializes a new instance of the <see cref="AppMemoryCache"/> class.  
        /// </summary>  
        /// <param name="memoryCache">The memory cache instance to use.</param>  
        public AppMemoryCache(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        /// <summary>  
        /// Retrieves data from the cache using the specified key.  
        /// </summary>  
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>  
        /// <param name="key">The cache key.</param>  
        /// <returns>The cached data if found; otherwise, the default value of <typeparamref name="T"/>.</returns>  
        public T GetData<T>(string key)
        {
            _cache.TryGetValue(key, out T value);
            return value;
        }

        /// <summary>  
        /// Stores data in the cache with an optional expiration time.  
        /// </summary>  
        /// <typeparam name="T">The type of the data to store.</typeparam>  
        /// <param name="key">The cache key.</param>  
        /// <param name="value">The data to cache.</param>  
        /// <param name="absoluteExpirationRelativeToNow">The optional expiration time relative to now.</param>  
        public void SetData<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            var options = new MemoryCacheEntryOptions();
            if (absoluteExpirationRelativeToNow.HasValue)
                options.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;

            _cache.Set(key, value, options);
        }
    }
}