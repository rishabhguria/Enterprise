using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace Prana.ExpnlService.DataDumper
{
    /// <summary>
    /// This is main processor class for all operation of data dumper
    /// It will be a singleton class and will be running on a thread
    /// </summary>
    internal sealed class DataDumpProcessor : IDisposable
    {
        #region Singleton
        private static readonly Lazy<DataDumpProcessor> lazy =
        new Lazy<DataDumpProcessor>(() => new DataDumpProcessor());
        internal static DataDumpProcessor Instance { get { return lazy.Value; } }

        private DataDumpProcessor()
        {
        }
        #endregion

        bool _isStopRequested = false;

        /// <summary>
        /// Start Data dumper
        /// </summary>
        internal void Start()
        {
            try
            {
                _isStopRequested = false;
                ServiceManager.GetInstance().AddAccountSymbolCompression();
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
                InformationReporter.GetInstance.Write(DateTime.UtcNow + ": Data dumper started.");
            }
            catch (Exception ex)
            {
                InformationReporter.GetInstance.Write(DateTime.UtcNow + ": Problem in starting the data dump Process.");
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handle if any error occurs in data dump process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //Worker stopped. If has some error log and restart again.
                //throw new NotImplementedException();
                if (e.Cancelled || !_isStopRequested)
                {
                    this.Start();
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Problem in Data dumper. Restarted again. " + e.Error, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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

        /// <summary>
        /// Save Real-time data in background thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            while (!_isStopRequested)
            {
                try
                {
                    Dictionary<int, ExposurePnlCacheItemList> outgoing = AccountSymbolCacheManager.Instance.GetOutgoingData();

                    if (outgoing != null && outgoing.Count > 0)
                    {
                        DataTable dtIndicesReturnCleaned = new DataTable();
                        DataTable dtIndicesReturn = AccountSymbolCacheManager.Instance.GetIndicesReturnData();
                        dtIndicesReturnCleaned = DataCleaner.GetCleanTable(dtIndicesReturn);

                        ExposurePnlCacheItemList cleanedData = DataCleaner.GetCleanedData(outgoing);
                        DBConnector.SaveRealTimeData(cleanedData, dtIndicesReturnCleaned);
                        outgoing = null;
                        cleanedData = null;
                        dtIndicesReturnCleaned.Clear();
                        if (dtIndicesReturn != null && dtIndicesReturn.Rows.Count > 0)
                        {
                            dtIndicesReturn.Clear();
                        }
                    }
                    else
                    {
                        AccountSymbolCacheManager.Instance.CacheWait.WaitOne();
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
        }

        /// <summary>
        /// Stop Data dump Process
        /// </summary>
        internal void Stop()
        {
            try
            {
                _isStopRequested = true;
                ServiceManager.GetInstance().RemoveAccountSymbolCompression();
                InformationReporter.GetInstance.Write(DateTime.UtcNow + ": Data dumper stopped.");
                AccountSymbolCacheManager.Instance.CacheWait.Set();
                AccountSymbolCacheManager.Instance.ClearCache();
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


        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose();
        }

        #endregion
    }
}
