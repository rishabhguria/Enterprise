using Newtonsoft.Json;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CalculationService.Constants;
using Prana.CalculationService.Models;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace Prana.CalculationService.CacheStore
{
    internal class CompressionsCache
    {
        #region Private variables

        /// <summary>
        /// Cache for Row Calculation
        /// </summary>
        private ConcurrentDictionary<string, RowCalculationBaseNav> _dictRowCalculation = new ConcurrentDictionary<string, RowCalculationBaseNav>();
        public ConcurrentDictionary<string, RowCalculationBaseNav> DictRowCalculation
        {
            get { return _dictRowCalculation; }
        }

        /// <summary>
        /// Cache for Row Calculation for Quantity Zero.
        /// </summary>
        private ConcurrentDictionary<string, RowCalculationBaseNav> _dictRowCalculationQuantityZero = new ConcurrentDictionary<string, RowCalculationBaseNav>();
        public ConcurrentDictionary<string, RowCalculationBaseNav> DictRowCalculationQuantityZero
        {
            get { return _dictRowCalculationQuantityZero; }
        }

        // <summary>
        /// Cache for Row Calculation incoming compression updates
        /// </summary>
        private ConcurrentDictionary<string, RowCalculationBaseNav> _rowCalculationIncomingUpdates = new ConcurrentDictionary<string, RowCalculationBaseNav>();

        /// <summary>
        /// Cache for Row Calculation outgoing compression updates
        /// </summary>
        private ConcurrentDictionary<string, RowCalculationBaseNav> _rowCalculationOutgoingUpdates = new ConcurrentDictionary<string, RowCalculationBaseNav>();

        /// <summary>
        /// SymbolsRequireLogging)
        /// </summary>
        private string _symbolsRequireLogging = ConfigurationHelper.Instance.GetAppSettingValueByKey(RtpnlConstants.CONFIG_APPSETTING_SymbolsRequireLogging);

        /// <summary>
        /// Action Block for conflating Row Calculation updates
        /// </summary>
        private ActionBlock<KeyValuePair<string, RowCalculationBaseNav>> conflateBlock;

        public List<string> SymbolsList = new List<string>();
        #endregion

        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly object _lockForInstance = new object();

        /// <summary>
        /// Locker object
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        /// Locker object for Outgoing Row Calculation
        /// </summary>
        private readonly object _lockForOutgoingRowCalculation = new object();
        public object LockForOutgoingRowCalculation
        {
            get { return _lockForOutgoingRowCalculation; }
        }

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static CompressionsCache _compressionsCache = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static CompressionsCache GetInstance()
        {
            lock (_lockForInstance)
            {
                if (_compressionsCache == null)
                    _compressionsCache = new CompressionsCache();
                return _compressionsCache;
            }
        }

        /// <summary>
        /// MarketDataProvider
        /// </summary>
        private MarketDataProvider _marketDataProvider = MarketDataProvider.None;
        public MarketDataProvider MarketDataProvider
        {
            get { return _marketDataProvider; }
            set { _marketDataProvider = value; }
        }
        #endregion

        #region ConflationBlockLoggingStatics

        private int _totalRowsReceivedForConflation = 0;
        private int _rowsLeftAfterConflation = 0;
        private static int duplicateUpdatesNotInLast5Minute = 0;
        private static int duplicateRowCalculationCount = 0;
        private int totalZeroQuantityRowCalculationUpdates = 0;
        private readonly object _lockForConflationStatistics = new object();

        public (int DuplicateEventsCount, int DuplicateInLast5Min, int TotalZeroEvents) GetDuplicateStatistics()
        {
            return (duplicateRowCalculationCount, duplicateRowCalculationCount-duplicateUpdatesNotInLast5Minute, totalZeroQuantityRowCalculationUpdates);
        }

        void IncrementTotalRowsReceived()
        {
            lock (_lockForConflationStatistics)
            {
                _totalRowsReceivedForConflation++;
            }
        }

        void IncrementRowsLeftAfterConflation(int conflatedUpdates)
        {
            lock (_lockForConflationStatistics)
            {
                _rowsLeftAfterConflation += conflatedUpdates;
            }
        }

        public (int TotalRowsReceived, int RowsLeftAfterConflation) GetStatistics()
        {
            lock (_lockForConflationStatistics)
            {
                return (_totalRowsReceivedForConflation, _rowsLeftAfterConflation);
            }
        }

        public void ResetStatistics()
        {
            lock (_lockForConflationStatistics)
            {
                _totalRowsReceivedForConflation = 0;
                _rowsLeftAfterConflation = 0;
                duplicateUpdatesNotInLast5Minute = duplicateRowCalculationCount;
            }
        }
        #endregion

        /// <summary>  
        /// Constructor  
        /// </summary>  
        private CompressionsCache()
        {
            try
            {
                if (_symbolsRequireLogging != string.Empty)
                {
                    string[] symbolsArray = _symbolsRequireLogging.Split(',');
                    SymbolsList = symbolsArray.ToList();
                }

                // Initialize the ActionBlock to handle incoming updates for RowCalculationBaseNav objects.
                // The ActionBlock processes each KeyValuePair<string, RowCalculationBaseNav> message asynchronously.
                // The key represents the unique identifier for the RowCalculationBaseNav object.
                // The value is the RowCalculationBaseNav object itself.
                // The processed message is added to the _rowCalculationIncomingUpdates dictionary.
                conflateBlock = new ActionBlock<KeyValuePair<string, RowCalculationBaseNav>>(msg =>
                {
                    _rowCalculationIncomingUpdates[msg.Key] = msg.Value;
                });
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

        /// <summary>
        /// RTPNL startup data receiver method
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        internal void StartupDataReceived(string routingKey, string data)
        {
            try
            {
                if (routingKey != null && !string.IsNullOrWhiteSpace(data))
                {
                    lock (_lockObject)
                    {
                        if (routingKey.Equals(RtpnlConstants.CONST_Underscore + RtpnlConstants.CONST_RowCalculationBaseWithNavStartupData))
                        {
                            RowCalculationBaseNav rowCalculationObj = RowCalculationBaseNav.GetRowCalculationObject(data, _marketDataProvider);
                            if (rowCalculationObj != null && rowCalculationObj.AssetId != int.MinValue && !string.IsNullOrEmpty(rowCalculationObj.Asset))
                            {
                                if (rowCalculationObj.Qty != 0)
                                {
                                    if (!_dictRowCalculation.ContainsKey(rowCalculationObj.UnqId))
                                        _dictRowCalculation.TryAdd(rowCalculationObj.UnqId, rowCalculationObj);
                                    else
                                        _dictRowCalculation[rowCalculationObj.UnqId] = rowCalculationObj;
                                }
                                else
                                {
                                    if (!_dictRowCalculationQuantityZero.ContainsKey(rowCalculationObj.UnqId))
                                        _dictRowCalculationQuantityZero.TryAdd(rowCalculationObj.UnqId, rowCalculationObj);
                                    else
                                        _dictRowCalculationQuantityZero[rowCalculationObj.UnqId] = rowCalculationObj;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Row Calculation runtime compressions calculated receiver method
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="data"></param>
        internal void RowCalculationBaseWithNavReceived(string routingKey, string data)
        {
            try
            {
                RowCalculationBaseNav rowCalculationInfo = RowCalculationBaseNav.GetRowCalculationObject(data, _marketDataProvider);
                if (rowCalculationInfo != null)
                {
                    if (!_dictRowCalculation.ContainsKey(rowCalculationInfo.UnqId) || !_dictRowCalculation[rowCalculationInfo.UnqId].PropertiesEqual(rowCalculationInfo))
                    {
                        if (rowCalculationInfo.Qty == 0)
                        {
                            totalZeroQuantityRowCalculationUpdates++;
                            if (_dictRowCalculation.ContainsKey(rowCalculationInfo.UnqId))
                            {
                                _dictRowCalculation.TryRemove(rowCalculationInfo.UnqId, out RowCalculationBaseNav value);
                                SendRemoveRowUpdates(rowCalculationInfo);
                            }
                            else if (rowCalculationInfo.Symbol.Equals(RtpnlConstants.CONST_NO_POSITION) && !RequiresRowRemoval(rowCalculationInfo))
                            {
                                IncrementTotalRowsReceived();
                                conflateBlock.Post(new KeyValuePair<string, RowCalculationBaseNav>(rowCalculationInfo.UnqId, rowCalculationInfo));
                                UpdateRowCalculationQuantityZeroCollection(rowCalculationInfo);
                            }
                            else if (RequiresRowRemoval(rowCalculationInfo))
                            {
                                SendRemoveRowUpdates(rowCalculationInfo);
                            }
                        }
                        else if (rowCalculationInfo.AssetId != int.MinValue && !string.IsNullOrEmpty(rowCalculationInfo.Asset))
                        {
                            IncrementTotalRowsReceived();
                            conflateBlock.Post(new KeyValuePair<string, RowCalculationBaseNav>(rowCalculationInfo.UnqId, rowCalculationInfo));

                            if (_dictRowCalculationQuantityZero.ContainsKey(rowCalculationInfo.UnqId))
                                _dictRowCalculationQuantityZero.TryRemove(rowCalculationInfo.UnqId, out RowCalculationBaseNav value);

                            if (SymbolsList.Count > 0 && SymbolsList.Contains(rowCalculationInfo.Symbol))
                                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - Row Calcualtion Method - SYM: " + rowCalculationInfo.Symbol + " QTY: " + rowCalculationInfo.Qty + " SFP: " + rowCalculationInfo.FeedPriceB + " UQID: " + rowCalculationInfo.UnqId);
                        }
                    }
                    else
                    {
                        duplicateRowCalculationCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Checks if Zero row needs update or not.
        /// </summary>
        /// <returns></returns>
        private bool RequiresRowRemoval(RowCalculationBaseNav rowCalculationInfo)
        {
            try
            {
                if (!_dictRowCalculationQuantityZero.ContainsKey(rowCalculationInfo.UnqId))
                    return false;

                var existing = _dictRowCalculationQuantityZero[rowCalculationInfo.UnqId];

                return (existing.AccNav != 0 && rowCalculationInfo.AccNav == 0) ||
                       (existing.MFNav != 0 && rowCalculationInfo.MFNav == 0) ||
                       (existing.Accrual != 0 && rowCalculationInfo.Accrual == 0) ||
                       (existing.CurCash != 0 && rowCalculationInfo.CurCash == 0);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return false;
        }

        /// <summary>
        /// Update Row Calculation Collection of Zero Quantity
        /// </summary>
        /// <returns></returns>
        internal void UpdateRowCalculationQuantityZeroCollection(RowCalculationBaseNav rowCalculationInfo)
        {
            try
            {
                if (rowCalculationInfo.AssetId != int.MinValue && !string.IsNullOrEmpty(rowCalculationInfo.Asset))
                {
                    if (!_dictRowCalculationQuantityZero.ContainsKey(rowCalculationInfo.UnqId))
                        _dictRowCalculationQuantityZero.TryAdd(rowCalculationInfo.UnqId, rowCalculationInfo);
                    else
                        _dictRowCalculationQuantityZero[rowCalculationInfo.UnqId] = rowCalculationInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Send update to frontend to remove the row 
        /// </summary>
        /// <returns></returns>
        internal void SendRemoveRowUpdates(RowCalculationBaseNav rowCalculationInfo)
        {
            try
            {
                KeyValuePair<string, RowCalculationBaseNav> rowCalculation = new KeyValuePair<string, RowCalculationBaseNav>(rowCalculationInfo.UnqId, rowCalculationInfo);
                RequestResponseModel response = new RequestResponseModel(0, RowCalculationBaseNav.GetCustomSerializedData(rowCalculation));
                _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RtpnlRowCalculationRemoved, response);

                UpdateRowCalculationQuantityZeroCollection(rowCalculationInfo);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Get Row Calculation outgoing data
        /// </summary>
        /// <returns></returns>
        internal ConcurrentDictionary<string, RowCalculationBaseNav> GetRowCalculationOutgoingUpdates()
        {
            lock (_lockForOutgoingRowCalculation)
            {
                if (_rowCalculationIncomingUpdates != null)
                {
                    _rowCalculationOutgoingUpdates = new ConcurrentDictionary<string, RowCalculationBaseNav>(_rowCalculationIncomingUpdates);
                    IncrementRowsLeftAfterConflation(_rowCalculationOutgoingUpdates.Count);
                    _rowCalculationIncomingUpdates.Clear();
                }
                else
                    return null;


                return _rowCalculationOutgoingUpdates;
            }
        }
    }
}
