using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Prana.ExpnlService.DataDumper
{
    /// <summary>
    /// Singleton class to hold dual cache of data coming from data calculator
    /// Main thread will be putting data in incoming copy of objects
    /// Secondary thread will be switching cache, storing secondary cache to db and cleaning up secondary cache.
    /// This class is just an encapsulated store of data
    /// </summary>
    internal sealed class AccountSymbolCacheManager : IDisposable
    {
        #region Singleton

        private static readonly Lazy<AccountSymbolCacheManager> lazy =
        new Lazy<AccountSymbolCacheManager>(() => new AccountSymbolCacheManager());

        public static AccountSymbolCacheManager Instance { get { return lazy.Value; } }

        private AccountSymbolCacheManager()
        {
        }

        #endregion

        #region Cache objects

        Dictionary<int, ExposurePnlCacheItemList> _incomingCache;
        Dictionary<int, ExposurePnlCacheItemList> _outgoingCache;
        DataTable _incomingDTIndicesReturn;
        DataTable _outgoingDTIndicesReturn;
        object _lockerObject = new object();
        object _outgoingCacheLocker = new object();
        #endregion
        public EventWaitHandle CacheWait = new EventWaitHandle(true, EventResetMode.AutoReset);
        internal void SaveIncomingData(Dictionary<int, ExposurePnlCacheItemList> cache)
        {
            lock (_lockerObject)
            {
                _incomingCache = cache;
                _incomingDTIndicesReturn = ExPnlCache.Instance.DTIndicesReturn;
                CacheWait.Set();
            }

        }


        /// <summary>
        /// Get outgoing data
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, ExposurePnlCacheItemList> GetOutgoingData()
        {
            lock (_outgoingCacheLocker)
            {
                //Switching Cache
                lock (_lockerObject)
                {
                    if (_incomingCache != null)
                    {

                        _outgoingCache = _incomingCache;
                        _incomingCache = null;
                    }
                    else
                        return null;
                }

                return _outgoingCache;
            }
        }

        /// <summary>
        /// Get Indices Return Data
        /// </summary>
        /// <returns></returns>
        internal DataTable GetIndicesReturnData()
        {
            lock (_outgoingCacheLocker)
            {
                //Switching Cache
                lock (_lockerObject)
                {
                    if (_incomingDTIndicesReturn != null)
                    {
                        _outgoingDTIndicesReturn = _incomingDTIndicesReturn.Copy();
                        _incomingDTIndicesReturn.Clear();
                    }
                    else
                        return null;
                }

                return _outgoingDTIndicesReturn;
            }
        }

        /// <summary>
        /// Dispose 
        /// </summary>
        public void Dispose()
        {
            if (_outgoingDTIndicesReturn != null)
                _outgoingDTIndicesReturn.Dispose();
            CacheWait.Dispose();
        }

        /// <summary>
        /// Clean cache
        /// </summary>
        internal void ClearCache()
        {
            lock (_lockerObject)
            {
                _incomingCache = null;
                _outgoingCache = null;
                if (_incomingDTIndicesReturn != null)
                    _incomingDTIndicesReturn.Clear();
                if (_outgoingDTIndicesReturn != null)
                    _outgoingDTIndicesReturn.Clear();
            }
        }
    }
}
