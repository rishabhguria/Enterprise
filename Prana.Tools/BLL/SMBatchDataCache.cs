using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Prana.Tools
{
    public class SMBatchDataCache
    {
        private static SMBatchDataCache _SMBatchInstance = null;

        public SMBatchDataCache() { }

        /// <summary>
        /// Singleton instance of SMBatchData Class
        /// </summary>
        /// <returns></returns>
        public static SMBatchDataCache GetInstance()
        {
            if (_SMBatchInstance == null)
            {
                _SMBatchInstance = new SMBatchDataCache();
            }
            return _SMBatchInstance;
        }


        /// <summary>
        /// maintains the cache for all the requested SM batches
        /// </summary>
        ConcurrentDictionary<string, HashSet<int>> requestsBatchId = new ConcurrentDictionary<string, HashSet<int>>();
        public ConcurrentDictionary<string, HashSet<int>> RequestsBatchId
        {
            get { return requestsBatchId; }
            set { requestsBatchId = value; }
        }
    }
}
