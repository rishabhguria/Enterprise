using System;
using System.Data;
using System.IO;
using System.Configuration;
using System.Runtime.Caching;

namespace Nirvana.TestAutomation.TestExecutor
{
    public class CacheManager
    {
        private static CacheManager instance;
        private MemoryCache cache;

        private CacheManager()
        {
            
            cache = MemoryCache.Default;
        }

        public static CacheManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CacheManager();
                }
                return instance;
            }
        }

        public MemoryCache GetCache()
        {
            return cache;
        }
    }

}
