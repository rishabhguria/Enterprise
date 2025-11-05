using Prana.BusinessObjects;
using Prana.ExpnlService.Grouping_Components.Views_and_SummaryCalculators.Compressors;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Prana.ExpnlService.Grouping_Components.Views_and_SummaryCalculators
{
    public class Account_Symbol_View : IGroupingComponent, IDisposable
    {
        private ICompressor _compressor = new Account_Symbol_View_Compressor();

        private Dictionary<int, ExposureAndPnlOrderCollection> _unCompressedData;
        private CompressedDataDictionaries _outputCompressedData;
        private ExposureAndPnlOrderCollection _markedcollection;
        private OutputCompressedSummaries _inputCalculatedSummaries;

        public event EventHandler DataCompressed;
        private EventWaitHandle wh = new AutoResetEvent(false);
        private Thread worker;
        private object locker = new object();
        private bool _isDataChanged = false;

        public Account_Symbol_View()
        {
            try
            {
                GroupingComponentName = CompressionViewFactory.ACCOUNTSYMBOL_COMPRESSION;
                worker = new Thread(CompressData);
                worker.IsBackground = true;
                worker.Start();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region ICompressionComponent Members
        public CompressedDataDictionaries GetCompressedData()
        {
            return _outputCompressedData;
        }

        public OutputCompressedSummaries GetCalculatedSummaries()
        {
            return _inputCalculatedSummaries;
        }

        public void SetInputData(Dictionary<int, ExposureAndPnlOrderCollection> unCompressedData, Dictionary<int, ExposureAndPnlOrderSummary> compressedAccountSummaries, ExposureAndPnlOrderCollection markedCollection, Dictionary<int, DistinctAccountSetWiseSummaryCollection> distinctAccountSetWiseSummaryCollection)
        {
            try
            {
                _inputCalculatedSummaries = new OutputCompressedSummaries();
                _unCompressedData = unCompressedData;
                _markedcollection = markedCollection;
                _inputCalculatedSummaries.OutputCompressedAccountSummaries = compressedAccountSummaries;
                _inputCalculatedSummaries.OutputAccountSetWiseConsolidatedSummary = distinctAccountSetWiseSummaryCollection;
                _isDataChanged = true;

                wh.Set();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public ExposurePnlCacheItemList GetContainingTaxLots(string compressedRowID, int accountID, int distinctAccountPermissionKey)
        {
            ExposurePnlCacheItemList listToReturn = null;
            try
            {
                lock (locker)
                {
                    DistinctAccountSetWiseSummaryCollection outputAccountSetWiseConsolidatedSummary = null;
                    if (_inputCalculatedSummaries.OutputAccountSetWiseConsolidatedSummary.ContainsKey(distinctAccountPermissionKey))
                    {
                        outputAccountSetWiseConsolidatedSummary = _inputCalculatedSummaries.OutputAccountSetWiseConsolidatedSummary[distinctAccountPermissionKey];
                    }
                    listToReturn = _compressor.GetContainingTaxlots(compressedRowID, accountID, outputAccountSetWiseConsolidatedSummary);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return listToReturn;
        }

        private void CompressData()
        {
            try
            {
                while (true)
                {
                    if (_isDataChanged)
                    {
                        lock (locker)
                        {
                            //TODO: all logic of compressing data goes here
                            _outputCompressedData = _compressor.GetData(_unCompressedData, _markedcollection, _inputCalculatedSummaries.OutputAccountSetWiseConsolidatedSummary, _inputCalculatedSummaries.OutputCompressedAccountSummaries);
                            if (DataCompressed != null && _isDataChanged)
                            {
                                DataCompressed(this, null);
                                _isDataChanged = false;
                            }
                        }
                    }
                    else
                        wh.WaitOne();         // No more tasks - wait for a signal
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                worker.Abort();
                wh.Close();             // Release any OS resources.
            }
        }
        #endregion


        public string GroupingComponentName
        {
            get;
            private set;
        }
    }
}