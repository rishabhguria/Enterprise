using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Prana.Global;
using Prana.Interfaces;
using Prana.BusinessObjects.LiveFeed;
using Prana.LiveFeedProvider;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Collections;

namespace Prana.LiveFeedEngine
{
    class LiveFeedSubscriber
    {


        #region Private members

        eSignalManager _instESignalManager = eSignalManager.GetInstance();
        List<string> _requestedSnapshotSymbolList = new List<string>();
        System.Threading.Timer _timer = null;
        Dictionary<string, Level1Data> _level1DataDict = new Dictionary<string, Level1Data>();
        object _lockerObj = new object();

        public event EventHandler DataManagerConnected;
        public event EventHandler DataManagerDisConnected;

        #endregion

        #region Singleton instance
        private LiveFeedSubscriber()
        {
            try
            {
                _instESignalManager.DataManagerConnected += new EventHandler(_instESignalManager_DataManagerConnected);
                _instESignalManager.DataManagerDisconnected += new EventHandler(_instESignalManager_DataManagerDisconnected);
                TimerCallback tmrCallBack = new TimerCallback(ThrowSnapshotResponse);
                _timer = new System.Threading.Timer(tmrCallBack, null, Timeout.Infinite, 100);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }



        static LiveFeedSubscriber _liveFeedSubscriber = null;
        public static LiveFeedSubscriber GetInstance()
        {
            if (_liveFeedSubscriber == null)
                _liveFeedSubscriber = new LiveFeedSubscriber();

            return _liveFeedSubscriber;
        }

        public void Stop()
        {
            _instESignalManager.Level1SnapshotResponse -= new EventHandler(_instESignalManager_Level1SnapshotResponse);
            _instESignalManager.OnResponse -= new EventHandler(_instESignalManager_OnResponse);
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Start()
        {
            _instESignalManager.Level1SnapshotResponse += new EventHandler(_instESignalManager_Level1SnapshotResponse);
            _instESignalManager.OnResponse += new EventHandler(_instESignalManager_OnResponse);
        }

        #endregion

        #region Livefeed connection methods
        public bool IsDataManagerConnected()
        {
            return _instESignalManager.IsDataManagerConnected();
        }

        public bool RestartLiveFeeds()
        {
            return _instESignalManager.ConnectDataManager();

        }
        #endregion

        #region Livefeed request methods

        /// <summary>
        /// This don't cache the data.
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public void GetLiveFeedSnapshotForSymbols(List<string> symbols)
        {
            if (_requestedSnapshotSymbolList != null)
            {
                foreach (string symbolStr in symbols)
                {
                    string symbol = symbolStr.ToUpper().Trim();
                    if (!_requestedSnapshotSymbolList.Contains(symbol))
                    {
                        _requestedSnapshotSymbolList.Add(symbol);
                    }

                    _instESignalManager.SnapshotSymbol(symbol,ApplicationConstants.SymbologyCodes.TickerSymbol);
                }

                _timer.Change(0, 100);
            }
        }

        void _instESignalManager_OnResponse(object sender, EventArgs e)
        {
            ArrayList ar = new ArrayList((ICollection)sender);

            string symbol;
            int rowToUpdate = int.MinValue;

            for (int i = 0; i < ar.Count; i++)
            {
                rowToUpdate = int.MinValue;
                Level1Data currentL1Data = (Level1Data)ar[i];
                symbol = currentL1Data.Symbol.ToString();

                // Check if it is a indices symbol
                //if (!symbol.StartsWith("$"))
                //{
                //    continue;
                //}

                if (_requestedSnapshotSymbolList.Contains(symbol))
                {
                    lock (_lockerObj)
                    {
                        if (!_level1DataDict.ContainsKey(symbol))
                        {
                            _level1DataDict.Add(symbol, currentL1Data);
                            _requestedSnapshotSymbolList.Remove(symbol);
                        }
                        else
                        {
                            _level1DataDict[symbol].UpdateContinuousData(currentL1Data);
                        }
                    }
                }

            }

        }

        #endregion

        #region Livefeed response events

        void _instESignalManager_DataManagerDisconnected(object sender, EventArgs e)
        {
            if (DataManagerDisConnected != null)
            {
                DataManagerDisConnected(this, EventArgs.Empty);
            }
        }

        void _instESignalManager_DataManagerConnected(object sender, EventArgs e)
        {
            if (DataManagerConnected != null)
            {
                DataManagerConnected(this, EventArgs.Empty);
            }
        }

        void _instESignalManager_Level1SnapshotResponse(object sender, EventArgs e)
        {
            LiveFeedEventArgs eventArgs = e as LiveFeedEventArgs;
            Level1Data currentL1Data = null;
            if (eventArgs != null)
            {
                currentL1Data = eventArgs.SnapshotLevel1Data;
                if (currentL1Data == null)
                {
                    return;
                }
                string symbol = currentL1Data.Symbol.ToUpper().Trim();

                if (_requestedSnapshotSymbolList.Contains(symbol))
                {
                    lock (_lockerObj)
                    {
                        if (!_level1DataDict.ContainsKey(symbol))
                        {
                            _level1DataDict.Add(symbol, currentL1Data);
                            _requestedSnapshotSymbolList.Remove(symbol);
                        }
                    }
                }
            }
        }


        private void ThrowSnapshotResponse(object state)
        {
            if (SendLevel1DataList != null && _level1DataDict.Count > 0)
            {
                List<Level1Data> listToSend = null;
                lock (_lockerObj)
                {
                     listToSend = new List<Level1Data>((IEnumerable<Level1Data>)_level1DataDict.Values);
                     _level1DataDict.Clear();
                }

                SendLevel1DataList(listToSend);
            }
        }

        #endregion

        public event Level1DataListHandler SendLevel1DataList;
    }

    public delegate void Level1DataListHandler(List<Level1Data> l1DataList);

}
