using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.LiveFeed;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.OptionCalculator.CalculationComponent;
using Prana.OptionCalculator.Common;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Prana.RiskServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession, UseSynchronizationContext = false)]
    public class GreekAnalysisManager : IGreekAnalysisServices, ILiveFeedCallback, IDisposable
    {
        #region private-variables

        // NOTE: The variables for storing callbacks and beer inventory are static. This is
        //       necessary since the service is using PerCall instancing. An instance of the service
        //       will be created each time a service method is invoked by a client. Consequently,
        //       the state must be persisted somewhere in between calls.
        private OperationContextPreservingSynchronizationContext _greekAnalysisCallbackSyncronisationContext = null;
        private int _snapshotCallCounter = 0;
        private int _stepAnalysisCallCounter = 0;
        private GreeksCalculator _greeksCalculator = null;
        private int _greeksCount = 0;
        private object _lockerCalculateGreeks = new object();
        private object _lockerSendDataToClient = new object();
        private IPricingService _pricingService = null;
        private Timer _timer = null;
        private delegate void OptionResponseHandler(string symbol, List<OptionStaticData> dictOfOptionData);
        private delegate void SymbolDataHandler(SymbolData data);

        #region snapshot pipeline variables
        private Timer _timerToSendSnapshotData;
        private Timer _timerTriggerSnapshotPipeline;
        private BatchBlock<object> _snapshotBufferBatchBlock;
        private BatchBlock<object> _snapshotBatchBlock;
        private TransformBlock<object[], object> _timerBlockSnapshot;
        private ActionBlock<object[]> _snapshotActionBlock;
        #endregion

        #region stepanalysis pipeline variables
        private Timer _timerTrigerStepAnalysisPipeline;
        private Timer _timerToSendStepAnalysisData;
        private BatchBlock<object> _stepAnalysisBatchBlock;
        private TransformBlock<object[], object> _timerBlockStepAnalysis;
        private BatchBlock<object> _stepAnalysisBufferBatchBlock;
        private ActionBlock<object[]> _stepAnalysisActionBlock;
        #endregion

        #endregion private-variables

        #region constructor

        public GreekAnalysisManager()
        {
            try
            {
                _greeksCalculator = new GreeksCalculator();
                _timer = new Timer(new TimerCallback(Calculate), null, Timeout.Infinite, 5000);
                SetupDataFlowPipelines();
                SetupPricingSevice();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetupDataFlowPipelines()
        {
            try
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = cancellationTokenSource.Token;

                GroupingDataflowBlockOptions groupingDataflowBlockOptions = new GroupingDataflowBlockOptions();
                groupingDataflowBlockOptions.Greedy = true;
                groupingDataflowBlockOptions.BoundedCapacity = GroupingDataflowBlockOptions.Unbounded;
                groupingDataflowBlockOptions.CancellationToken = cancellationToken;

                int batchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BatchSizeForPricingGreekAnalysisService"].ToString());
                int timeIntervalBetweenTwoBatches = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TimeIntervalBetweenSendingOfBatchesForPricingGreekAnalysisService"].ToString());

                SetupSnapshotPipeline(batchSize, timeIntervalBetweenTwoBatches, groupingDataflowBlockOptions, cancellationToken);
                SetupStepAnalysisPipeline(batchSize, timeIntervalBetweenTwoBatches, groupingDataflowBlockOptions, cancellationToken);
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

        private void SetupSnapshotPipeline(int batchSize, int timeIntervalBetweenTwoBatches, GroupingDataflowBlockOptions groupingDataflowBlockOptions, CancellationToken cancellationToken)
        {
            try
            {
                _timerToSendSnapshotData = new Timer(_ => _snapshotBatchBlock.TriggerBatch());
                _timerTriggerSnapshotPipeline = new Timer(_ => _snapshotBufferBatchBlock.TriggerBatch());
                _snapshotBatchBlock = new BatchBlock<object>(batchSize, groupingDataflowBlockOptions);
                _snapshotBufferBatchBlock = new BatchBlock<object>(1, groupingDataflowBlockOptions);
                _timerBlockSnapshot = new TransformBlock<object[], object>((value) =>
                {
                    _timerTriggerSnapshotPipeline.Change(500, Timeout.Infinite);
                    _timerToSendSnapshotData.Change(timeIntervalBetweenTwoBatches, Timeout.Infinite);
                    return value[0];
                }
                , new ExecutionDataflowBlockOptions
                {
                    CancellationToken = cancellationToken
                });
                _snapshotActionBlock = new ActionBlock<object[]>(async (snapshotBatch) =>
                {
                    if (Prana.BusinessObjects.UserSettingConstants.IsRiskLoggingEnabled)
                    {
                        InformationReporter.GetInstance.Write(++_snapshotCallCounter + "Snapshot Batch deployed whose count is: " + snapshotBatch.Count());
                    }
                    SendOrPostCallback greekAnalysisCallback = new SendOrPostCallback(SendSnapshotToClientUsingSynchronisationContext);
                    _greekAnalysisCallbackSyncronisationContext.Send(greekAnalysisCallback, snapshotBatch.Cast<object>().ToList());
                    await System.Threading.Tasks.Task.Delay(TimeSpan.FromMilliseconds(timeIntervalBetweenTwoBatches));
                });

                _snapshotBatchBlock.LinkTo(_snapshotActionBlock);
                _timerBlockSnapshot.LinkTo(_snapshotBatchBlock);
                _snapshotBufferBatchBlock.LinkTo(_timerBlockSnapshot);
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

        private void SetupStepAnalysisPipeline(int batchSize, int timeIntervalBetweenTwoBatches, GroupingDataflowBlockOptions groupingDataflowBlockOptions, CancellationToken cancellationToken)
        {
            try
            {
                _timerToSendStepAnalysisData = new Timer(_ => _stepAnalysisBatchBlock.TriggerBatch());
                _timerTrigerStepAnalysisPipeline = new Timer(_ => _stepAnalysisBufferBatchBlock.TriggerBatch());
                _stepAnalysisBatchBlock = new BatchBlock<object>(batchSize, groupingDataflowBlockOptions);
                _stepAnalysisBufferBatchBlock = new BatchBlock<object>(1, groupingDataflowBlockOptions);
                _timerBlockStepAnalysis = new TransformBlock<object[], object>((value) =>
                {
                    _timerTrigerStepAnalysisPipeline.Change(500, Timeout.Infinite);
                    _timerToSendStepAnalysisData.Change(timeIntervalBetweenTwoBatches, Timeout.Infinite);
                    return value[0];
                }
                , new ExecutionDataflowBlockOptions
                {
                    CancellationToken = cancellationToken
                });
                _stepAnalysisActionBlock = new ActionBlock<object[]>(async (stepAnalysisBatch) =>
                {
                    if (Prana.BusinessObjects.UserSettingConstants.IsRiskLoggingEnabled)
                    {
                        InformationReporter.GetInstance.Write(++_stepAnalysisCallCounter + "Step Analysis Batch deployed whose count is: " + stepAnalysisBatch.Count());
                    }
                    SendOrPostCallback greekAnalysisCallback = new SendOrPostCallback(SendStepAnalysisDataToClientUsingSynchronisationContext);
                    _greekAnalysisCallbackSyncronisationContext.Send(greekAnalysisCallback, stepAnalysisBatch.Cast<object>().ToList());
                    await System.Threading.Tasks.Task.Delay(TimeSpan.FromMilliseconds(timeIntervalBetweenTwoBatches));
                });

                _stepAnalysisBatchBlock.LinkTo(_stepAnalysisActionBlock);
                _timerBlockStepAnalysis.LinkTo(_stepAnalysisBatchBlock);
                _stepAnalysisBufferBatchBlock.LinkTo(_timerBlockStepAnalysis);
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

        #endregion constructor

        #region private-members

        private void _pricingService_OptionChainResponse(object sender, EventArgs<string, List<OptionStaticData>> e)
        {
            try
            {
                OptionResponseHandler optionChainResponseHandler = new OptionResponseHandler(OptionChainResponseOnNewThread);
                optionChainResponseHandler.BeginInvoke(e.Value, e.Value2, null, null);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void Calculate(object param)
        {
            try
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                lock (_lockerCalculateGreeks)
                {
                    foreach (KeyValuePair<string, SubscriberData> subscriberDataKeyValue in SubscriberCollection.SubscriberCollectionData)
                    {
                        SubscriberData subscriberData = subscriberDataKeyValue.Value;
                        foreach (SubscriberViewData subscriberView in subscriberData.DictSubscriberViews.Values)
                        {
                            if (subscriberView.IsVolSkewRequest)
                            {
                                if (subscriberView.CheckIFProcessingRequired())
                                {
                                    List<ProxyMappedData> listMappedData = subscriberView.FetchProxySymbolMappedData();
                                    if (listMappedData.Count > 0)
                                    {
                                        foreach (ProxyMappedData proxyMappedData in listMappedData)
                                        {
                                            bool isVolatililtyCalculated = false;
                                            List<VolSkewObject> volSkewReqUpdated = subscriberView.GetVolSkewData(proxyMappedData.ProxySymbolData.Symbol, true);

                                            // if VolSkewReqUpdated is null then this means that the
                                            // data corresponds to the actual option Symbol and not
                                            // proxy symbol and so we don't need to do anything.
                                            if (volSkewReqUpdated.Count > 0)
                                            {
                                                foreach (VolSkewObject obj in volSkewReqUpdated)
                                                {
                                                    Dictionary<string, SymbolData> dictParentSymbols = proxyMappedData.DictParentSymbols;
                                                    SymbolData parentSymbolData = null;
                                                    if (dictParentSymbols.ContainsKey(obj.Symbol))
                                                    {
                                                        parentSymbolData = dictParentSymbols[obj.Symbol];
                                                    }
                                                    if (parentSymbolData != null)
                                                    {
                                                        isVolatililtyCalculated = SendRequestForImpliedVolCalculation(proxyMappedData.ProxySymbolData, obj, parentSymbolData, subscriberView);
                                                        subscriberView.RemovefromProxySymbolDict(proxyMappedData.ProxySymbolData.Symbol, obj);

                                                        if (isVolatililtyCalculated)
                                                        {
                                                            if (subscriberView.IsStressTestRequest)
                                                            {
                                                                PricingModelData pricingData = CreatePricingModelDataObjectFromSymbolData(parentSymbolData, proxyMappedData.UnderlyingSymbolData);

                                                                ResponseObj optionRes = SendSnapShotResponseForGreeksCalculation(subscriberView, pricingData);
                                                                if (optionRes.CalculatedGreeks.Count > 0)
                                                                {
                                                                    if (optionRes.CalculatedGreeks.ContainsKey(parentSymbolData.Symbol))
                                                                    {
                                                                        OptionGreeks greeks = optionRes.CalculatedGreeks[parentSymbolData.Symbol];
                                                                        greeks.ProxySymbol = proxyMappedData.ProxySymbolData.Symbol;
                                                                        SendSnapshotDataToClient(optionRes, subscriberDataKeyValue.Key, subscriberView.SubscriberHashCode);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Dictionary<string, StepAnalysisResponse> dictstepAnal = subscriberView.GetStepAnalysisData(parentSymbolData.Symbol);
                                                                if (dictstepAnal != null)
                                                                {
                                                                    List<StepAnalysisResponse> stepAnalList = new List<StepAnalysisResponse>();
                                                                    foreach (StepAnalysisResponse stepAnal in dictstepAnal.Values)
                                                                    {
                                                                        stepAnal.Greeks = _greeksCalculator.CalculateGreeksForStaticDataNew(stepAnal.InputParameters);
                                                                        stepAnalList.Add(stepAnal);
                                                                        _greeksCount++;
                                                                    }

                                                                    SendStepAnalysisDataToClient(stepAnalList, subscriberDataKeyValue.Key, subscriberView.SubscriberHashCode);
                                                                }
                                                            }
                                                            subscriberView.RemoveSymbol(proxyMappedData.ProxySymbolData.Symbol, proxyMappedData.UnderlyingSymbolData.Symbol, proxyMappedData.ProxySymbolData.CategoryCode);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                _timer.Change(1000, 1000);
            }
        }

        private PricingModelData CreatePricingModelDataObjectFromStressInputData(StepAnalysisResponse stepAnalReqObj)
        {
            PricingModelData pricingModelData = new PricingModelData();

            try
            {
                pricingModelData.ListedExchange = stepAnalReqObj.InputParameters.ListedExchange;
                pricingModelData.CategoryCode = stepAnalReqObj.InputParameters.CategoryCode;
                pricingModelData.DividendYield = stepAnalReqObj.InputParameters.DividendYield;
                pricingModelData.Volatility = stepAnalReqObj.InputParameters.Volatility;
                pricingModelData.OptionPrice = stepAnalReqObj.InputParameters.SimulatedPrice;
                pricingModelData.OptSymbol = stepAnalReqObj.InputParameters.Symbol;
                pricingModelData.AUECID = stepAnalReqObj.InputParameters.AUECID;

                char putOrCall;
                char.TryParse(stepAnalReqObj.InputParameters.PutOrCalls.ToString().Substring(0, 1), out putOrCall);
                pricingModelData.PutOrCall = putOrCall;
                pricingModelData.StockPrice = stepAnalReqObj.InputParameters.SimulatedUnderlyingStockPrice;
                pricingModelData.StrikePrice = stepAnalReqObj.InputParameters.StrikePrice;
                pricingModelData.UnderlyingSymbol = stepAnalReqObj.InputParameters.UnderlyingSymbol;
                pricingModelData.BloombergSymbol = stepAnalReqObj.InputParameters.BloombergSymbol;
                pricingModelData.ExpirationDate = stepAnalReqObj.InputParameters.ExpirationDate;
                /*setting the interest rate value to double.minVal as interest rate needs to be recalculated later based on simulated days to exp if the value has not been
                provided by user ie if its value is double min value.
                same case for delta as the final delta value needs to be overridden by the user value.*/

                UserOptModelInput UserOMIData = OptionModelUserInputCache.GetUserOMIData(stepAnalReqObj.InputParameters.Symbol);
                if (UserOMIData != null)
                {
                    pricingModelData.InterestRate = (UserOMIData.IntRateUsed) ? UserOMIData.IntRate : pricingModelData.InterestRate;
                    pricingModelData.Delta = (UserOMIData.DeltaUsed) ? UserOMIData.Delta : pricingModelData.Delta;

                    if ((!UserOMIData.LastPriceUsed && !FeedPriceChooser.UseClosingMark) && UserOMIData.TheoreticalPriceUsed && pricingModelData.StockPrice != 0)
                    {
                        DateTime auecMarketEndTime = PricingUtilityCache.GetMarketEndTimeForAUEC(pricingModelData.AUECID);
                        DateTime auecLocalTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(pricingModelData.AUECID));
                        auecLocalTime = auecLocalTime <= DateTimeConstants.MinValue ? DateTime.UtcNow : auecLocalTime;
                        TimeSpan ts2 = auecMarketEndTime.Subtract(auecLocalTime.Date);
                        DateTime expirationDate = pricingModelData.ExpirationDate.AddDays(ts2.TotalDays);

                        TimeSpan ts = expirationDate.Subtract(auecLocalTime);
                        double daysToExpiration = ts.TotalDays;
                        daysToExpiration = daysToExpiration < 0 ? 0 : daysToExpiration;
                        pricingModelData.OptionPrice = _greeksCalculator.GetTheoreticalPrice(pricingModelData, RiskPreferenceManager.GetInterestRate(daysToExpiration));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return pricingModelData;
        }

        private PricingModelData CreatePricingModelDataObjectFromSymbolData(SymbolData data, SymbolData underlyingData)
        {
            OptionSymbolData optData = data as OptionSymbolData;

            PricingModelData pricingModelData = new PricingModelData();

            pricingModelData.ListedExchange = optData.ListedExchange;
            pricingModelData.CategoryCode = optData.CategoryCode;
            pricingModelData.DividendYield = underlyingData.FinalDividendYield - underlyingData.StockBorrowCost;
            pricingModelData.Volatility = optData.FinalImpliedVol;
            pricingModelData.OptionPrice = optData.SelectedFeedPrice;
            pricingModelData.OptSymbol = optData.Symbol;
            pricingModelData.AUECID = optData.AUECID;

            char putOrCall;
            char.TryParse(optData.PutOrCall.ToString().Substring(0, 1), out putOrCall);
            pricingModelData.PutOrCall = putOrCall;
            pricingModelData.StockPrice = underlyingData.SelectedFeedPrice;
            pricingModelData.StrikePrice = optData.StrikePrice;
            pricingModelData.UnderlyingSymbol = optData.UnderlyingSymbol;
            pricingModelData.BloombergSymbol = optData.BloombergSymbol;
            pricingModelData.ExpirationDate = optData.ExpirationDate;
            /*setting the interest rate value to double.minVal as interest rate needs to be recalculated later based on simulated days to exp if the value has not been
            provided by user ie if its value is double min value.
            same case for delta as the final delta value needs to be overridden by the user value.*/

            UserOptModelInput UserOMIData = OptionModelUserInputCache.GetUserOMIData(data.Symbol);
            if (UserOMIData != null)
            {
                pricingModelData.InterestRate = (UserOMIData.IntRateUsed) ? UserOMIData.IntRate : pricingModelData.InterestRate;
                pricingModelData.Delta = (UserOMIData.DeltaUsed) ? UserOMIData.Delta : pricingModelData.Delta;

                if ((!UserOMIData.LastPriceUsed && !FeedPriceChooser.UseClosingMark) && UserOMIData.TheoreticalPriceUsed && pricingModelData.StockPrice != 0)
                {
                    DateTime auecMarketEndTime = PricingUtilityCache.GetMarketEndTimeForAUEC(pricingModelData.AUECID);
                    DateTime auecLocalTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(pricingModelData.AUECID));
                    auecLocalTime = auecLocalTime <= DateTimeConstants.MinValue ? DateTime.UtcNow : auecLocalTime;
                    TimeSpan ts2 = auecMarketEndTime.Subtract(auecLocalTime.Date);
                    DateTime expirationDate = pricingModelData.ExpirationDate.AddDays(ts2.TotalDays);
                    TimeSpan ts = expirationDate.Subtract(auecLocalTime) + ts2;
                    double daysToExpiration = ts.TotalDays;
                    daysToExpiration = daysToExpiration < 0 ? 0 : daysToExpiration;

                    pricingModelData.OptionPrice = _greeksCalculator.GetTheoreticalPrice(pricingModelData, RiskPreferenceManager.GetInterestRate(daysToExpiration));
                }
            }
            return pricingModelData;
        }

        private void OptionChainResponseOnNewThread(string symbol, List<OptionStaticData> dictOfOptionData)
        {
            try
            {
                //string requestID = string.Empty;
                //string underlyingSymbol = string.Empty;
                //string userID = string.Empty;
                //string subscriberViewID = string.Empty;
                //List<string> Symbols = new List<string>();
                //// List<string> listAlreadyRequested = new List<string>();
                //List<VolSkewObject> volSkewReqlist = null;

                //if (dictOfOptionData.Count > 0)
                //{
                //    foreach (KeyValuePair<string, List<OptionStaticData>> kp in dictOfOptionData)
                //    {
                //        requestID = kp.Key;
                //        //break;
                //        // here 6 is fixed as the UID we are generating is of 4 digits and subscriberViewID is fixed to be 2 digits
                //        userID = requestID.Substring(0, (requestID.Length - 6));

                //        //subscriberViewID is fixed to be 2 digits
                //        subscriberViewID = requestID.Substring((requestID.Length - 2), 2);

                //        SubscriberViewData subscriberView = SubscriberCollection.GetSubscriberView(userID, subscriberViewID);
                //        if (subscriberView != null)
                //        {
                //            volSkewReqlist = subscriberView.GetVolSkewObjectlist(requestID);

                //            if (volSkewReqlist != null && volSkewReqlist.Count > 0)
                //            {
                //                underlyingSymbol = volSkewReqlist[0].UnderlyingSymbol;
                //                List<OptionStaticData> listOptData = dictOfOptionData[requestID];
                //                foreach (VolSkewObject ReqObj in volSkewReqlist)
                //                {
                //                    List<string> listProxyOptions = VolSkewManager.GetProxyOptions(listOptData, ReqObj, subscriberView, requestID);
                //                    if (listProxyOptions.Count > 0)
                //                    {
                //                        Symbols = new List<string>();
                //                        foreach (string proxySymbol in listProxyOptions)
                //                        {
                //                            bool isSnapShotRequested = subscriberView.CheckIfSnapshotAlreadyRequested(proxySymbol);

                //                            if (!isSnapShotRequested)
                //                            {
                //                                Symbols.Add(proxySymbol);
                //                            }
                //                        }
                //                        if (Symbols.Count > 0)
                //                        {
                //                            _pricingService.RequestSnapshot(Symbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, this, true);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ProcessVolSkewReq(List<VolSkewObject> lstVolSkewReq, SymbolData underlyingData, SubscriberViewData subscriberViewData, string userID)
        {
            try
            {
                Dictionary<string, VolSkewStrikeRange> dictUniqueExpYearMonths = new Dictionary<string, VolSkewStrikeRange>();
                string subscriberViewID = subscriberViewData.SubscriberViewID;

                double toleranceValue = 0.05;
                List<OptionSimulationInputs> OptInputs = new List<OptionSimulationInputs>();
                string RequestID = string.Empty;
                foreach (VolSkewObject obj in lstVolSkewReq)
                {
                    OptInputs = subscriberViewData.GetSimulationInputs(obj.Symbol);
                    VolSkewManager.SetToleranceAdjustedProxyValues(underlyingData, obj, toleranceValue, OptInputs);
                    List<DateTime> listProxyExpirationDates = obj.GetProxyExpirationDatesForUniqueMonths();
                    bool isAdded = false;

                    foreach (DateTime date in listProxyExpirationDates)
                    {
                        int month = date.Date.Month;
                        string monthConcat = month.ToString();
                        if (month < 10)
                        {
                            monthConcat = string.Concat("0", month);
                        }
                        string yearMonth = date.Year.ToString() + monthConcat;

                        if (dictUniqueExpYearMonths.ContainsKey(yearMonth))
                        {
                            VolSkewStrikeRange Range = dictUniqueExpYearMonths[yearMonth];
                            if (Range.StrikeMax < obj.ProxyStrikeMax)
                            {
                                Range.StrikeMax = obj.ProxyStrikeMax;
                            }
                            if (Range.StrikeMin > obj.ProxyStrikeMin)
                            {
                                Range.StrikeMin = obj.ProxyStrikeMin;
                            }
                            if (!isAdded)
                            {
                                Range.ListVolSkew.Add(obj);
                                isAdded = true;
                            }
                        }
                        else
                        {
                            VolSkewStrikeRange strikeRange = new VolSkewStrikeRange();
                            strikeRange.StrikeMin = obj.ProxyStrikeMin;
                            strikeRange.StrikeMax = obj.ProxyStrikeMax;
                            strikeRange.ListVolSkew.Add(obj);
                            dictUniqueExpYearMonths.Add(yearMonth, strikeRange);
                        }
                    }
                }
                foreach (KeyValuePair<string, VolSkewStrikeRange> kp in dictUniqueExpYearMonths)
                {
                    string yearMonth = kp.Key;
                    int expirationMonth = int.Parse(yearMonth.Substring(yearMonth.Length - 2));
                    VolSkewStrikeRange range = kp.Value;
                    List<VolSkewObject> VolSkewReqList = range.ListVolSkew;
                    AssetCategory CategoryCode = AssetCategory.EquityOption;

                    if (VolSkewReqList.Count > 0)
                    {
                        CategoryCode = VolSkewReqList[0].CategoryCode;
                    }

                    string uID = uIDGenerator.GenerateIDLong();
                    if (uID.Length < 4)
                    {
                        int length = uID.Length;
                        int diff = (4 - length);
                        for (int i = 0; i < diff; i++)
                        {
                            uID = string.Format("0{0}", uID);
                        }
                    }
                    RequestID = userID + uID + subscriberViewID;
                    subscriberViewData.SubscribeProxySearchRequest(RequestID, VolSkewReqList);
                    subscriberViewData.UpdateProxyExpirationMonthDictionary(RequestID, expirationMonth);

                    // Option chain symbols are requested here.
                    double strikeMin = Math.Round(range.StrikeMin);
                    double strikeMax = Math.Round(range.StrikeMax);

                    OptionChainFilter optionChainFilter = new BusinessObjects.OptionChainFilter();
                    optionChainFilter.ExpirationDate = new DateTime(int.Parse(yearMonth.Substring(0, yearMonth.Length - 2)), expirationMonth, 01);
                    optionChainFilter.LowerStrike = strikeMin;
                    optionChainFilter.UpperStrike = strikeMax;
                    optionChainFilter.TurnAroundID = RequestID;
                    optionChainFilter.CategoryCode = CategoryCode;

                    SendStaticOptionChainRequest(underlyingData.Symbol, optionChainFilter);
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

        private void SendGreeksCalculatedDataToClient(Dictionary<string, StepAnalysisResponse> dictstepAnal, int hashCode, string viewId, string userId)
        {
            try
            {
                if (dictstepAnal != null)
                {
                    List<StepAnalysisResponse> stepAnalList = new List<StepAnalysisResponse>();
                    foreach (StepAnalysisResponse stepAnal in dictstepAnal.Values)
                    {
                        stepAnal.Greeks = _greeksCalculator.CalculateGreeksForStaticDataNew(stepAnal.InputParameters);
                        stepAnalList.Add(stepAnal);
                    }
                    SendStepAnalysisDataToClient(stepAnalList, userId, hashCode);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool SendRequestForImpliedVolCalculation(SymbolData ProxyOptData, VolSkewObject VolSkewObj, SymbolData parentSymbolData, SubscriberViewData subscriberViewData)
        {
            bool isVolatilityUpdated = false;

            Dictionary<double, double> dictImpliedVols = new Dictionary<double, double>();
            try
            {
                double underlyingPrice = VolSkewObj.UnderlyingPrice;
                double proxyImpliedVolatility = 0;
                OptionSymbolData optData = ProxyOptData as OptionSymbolData;
                OptionSymbolData optData_original = parentSymbolData as OptionSymbolData;
                DateTime auecMarketEndTime = PricingUtilityCache.GetMarketEndTimeForAUEC(ProxyOptData.AUECID);
                DateTime auecLocalTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(ProxyOptData.AUECID));
                auecLocalTime = auecLocalTime <= DateTimeConstants.MinValue ? DateTime.UtcNow : auecLocalTime;
                TimeSpan ts2 = auecMarketEndTime.Subtract(auecLocalTime.Date);
                DateTime expirationDate = ProxyOptData.ExpirationDate.AddDays(ts2.TotalDays);

                TimeSpan ts = expirationDate.Subtract(auecLocalTime);
                double DaysToExpiration = ts.TotalDays;
                DaysToExpiration = DaysToExpiration < 0 ? 0 : DaysToExpiration;
                OptionType PutOrCall = optData.PutOrCall;

                string exchangeIdentifier = string.Empty;
                double timeForExpirationinYrs = DaysToExpiration * 1.0 / 365.0;
                proxyImpliedVolatility = _greeksCalculator.CalculateImpliedVol(underlyingPrice, optData.FinalInterestRate, timeForExpirationinYrs, optData.SelectedFeedPrice, VolSkewObj.UnderlyingSymbol, optData.BloombergSymbol, optData.StrikePrice, PutOrCall, optData.AUECID);
                if (subscriberViewData.IsStressTestRequest)
                {
                    VolSkewObj.ProxyDaysToExpiration = optData.DaysToExpiration;
                    VolSkewObj.ProxyExpirationDate = optData.ExpirationDate;
                    VolSkewObj.OptionPrice = ProxyOptData.SelectedFeedPrice;
                    VolSkewObj.ProxyVolatility = proxyImpliedVolatility;
                    if (optData_original.FinalImpliedVol == optData_original.ImpliedVol)
                    {
                        optData_original.ImpliedVol = VolSkewObj.ProxyVolatility;
                        optData_original.FinalImpliedVol = VolSkewObj.ProxyVolatility;
                    }
                    isVolatilityUpdated = true;
                }
                else
                {
                    List<string> listSteps = VolSkewObj.GetStepkeyValuesForProxySymbol(optData.Symbol);

                    subscriberViewData.UpdateStepAnalysisResponseWithProxyVol(listSteps, proxyImpliedVolatility, parentSymbolData.Symbol);
                    if (VolSkewObj.IsProxyVolCalculatedForAllSymbols())
                    {
                        isVolatilityUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isVolatilityUpdated;
        }

        private void SendSnapshotDataToClient(ResponseObj responseObj, string userID, int hashCode)
        {
            try
            {

                if (responseObj != null)
                {
                    try
                    {

                        List<object> objectState = new List<object>();
                        objectState.Add(responseObj);
                        objectState.Add(userID);
                        objectState.Add(hashCode);
                        _snapshotBufferBatchBlock.Post(objectState);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SendSnapshotToClientUsingSynchronisationContext(object state)
        {
            try
            {
                lock (_lockerSendDataToClient)
                {
                    OperationContext.Current.GetCallbackChannel<IGreekAnalysisCallback>().ProcessSnapshotData(state);
                }
            }
            catch (ObjectDisposedException)
            {

            }
            // To avoid channel communication exceptions
            //http://jira.nirvanasolutions.com:8080/browse/PRANA-3020
            catch (CommunicationException)
            {

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void SendStepAnalysisDataToClientUsingSynchronisationContext(object state)
        {
            try
            {
                lock (_lockerSendDataToClient)
                {
                    OperationContext.Current.GetCallbackChannel<IGreekAnalysisCallback>().ProcessStepAnalysisData(state);
                }
            }
            catch (ObjectDisposedException)
            {

            }
            // To avoid channel communication exceptions
            //http://jira.nirvanasolutions.com:8080/browse/PRANA-3020
            catch (CommunicationException)
            {

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private ResponseObj SendSnapShotResponseForGreeksCalculation(SubscriberViewData subscriberData, PricingModelData snapShotResponse)
        {
            ResponseObj optionRes = new ResponseObj();
            Dictionary<string, OptionGreeks> calculatedGreeks = optionRes.CalculatedGreeks;

            try
            {
                OptionSimulationInputs simInputs = null;
                List<OptionSimulationInputs> listSimInputs = subscriberData.GetSimulationInputs(snapShotResponse.OptSymbol);

                if (listSimInputs.Count > 0)
                {
                    simInputs = listSimInputs[0];
                }

                OptionGreeks optionGreeks = new OptionGreeks();
                PricingModelData OMIUpdatedLiveData = snapShotResponse;

                InputParametersForGreeks inputForGreeks = new InputParametersForGreeks();
                inputForGreeks.ListedExchange = OMIUpdatedLiveData.ListedExchange;
                inputForGreeks.CategoryCode = OMIUpdatedLiveData.CategoryCode;
                inputForGreeks.StrikePrice = OMIUpdatedLiveData.StrikePrice;
                inputForGreeks.PutOrCalls = OMIUpdatedLiveData.PutOrCall;
                inputForGreeks.Symbol = OMIUpdatedLiveData.OptSymbol;
                inputForGreeks.UnderlyingSymbol = OMIUpdatedLiveData.UnderlyingSymbol;
                inputForGreeks.InterestRate = OMIUpdatedLiveData.InterestRate;
                inputForGreeks.SimulatedUnderlyingStockPrice = OMIUpdatedLiveData.StockPrice;
                inputForGreeks.SimulatedPrice = OMIUpdatedLiveData.OptionPrice;
                inputForGreeks.DividendYield = OMIUpdatedLiveData.DividendYield;
                inputForGreeks.Volatility = OMIUpdatedLiveData.Volatility;
                inputForGreeks.BloombergSymbol = OMIUpdatedLiveData.BloombergSymbol;
                inputForGreeks.AUECID = OMIUpdatedLiveData.AUECID;
                inputForGreeks.ExpirationDate = OMIUpdatedLiveData.ExpirationDate;

                inputForGreeks.ExpirationDate = inputForGreeks.ExpirationDate.AddDays(simInputs.ChangeDaysToExpiration * (-1));

                if (inputForGreeks.InterestRate == double.MinValue)
                {
                    DateTime auecMarketEndTime = PricingUtilityCache.GetMarketEndTimeForAUEC(inputForGreeks.AUECID);
                    DateTime auecLocalTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(inputForGreeks.AUECID));
                    auecLocalTime = auecLocalTime <= DateTimeConstants.MinValue ? DateTime.UtcNow : auecLocalTime;
                    TimeSpan ts2 = auecMarketEndTime.Subtract(auecLocalTime.Date);
                    DateTime expirationDate = inputForGreeks.ExpirationDate.AddDays(ts2.TotalDays);

                    TimeSpan ts = expirationDate.Subtract(auecLocalTime);
                    double daysToExpiration = ts.TotalDays;
                    daysToExpiration = daysToExpiration < 0 ? 0 : daysToExpiration;
                    inputForGreeks.InterestRate = RiskPreferenceManager.GetInterestRate(daysToExpiration);
                }

                if (simInputs.UnderlyingPriceAsAbosluteValue)
                {
                    inputForGreeks.SimulatedUnderlyingStockPrice += simInputs.ChangeUnderlyingPrice;
                    if (inputForGreeks.SimulatedUnderlyingStockPrice < 0)
                    {
                        inputForGreeks.SimulatedUnderlyingStockPrice = 0;
                    }
                }
                else
                {
                    inputForGreeks.SimulatedUnderlyingStockPrice *= simInputs.ChangeUnderlyingPrice;
                }
                inputForGreeks.InterestRate *= simInputs.ChangeInterestRate;
                inputForGreeks.Volatility *= simInputs.ChangeVolatility;

                optionGreeks = _greeksCalculator.CalculateGreeksForStaticDataNew(inputForGreeks);

                if (simInputs.ChangeDaysToExpiration == 0 && simInputs.ChangeInterestRate == 1 && simInputs.ChangeUnderlyingPrice == 1 && simInputs.ChangeVolatility == 1)
                {
                    optionGreeks.SimulatedPrice = optionGreeks.SelectedFeedPrice;
                }
                else if (simInputs.ChangeDaysToExpiration == 0 && simInputs.ChangeInterestRate == 1 && simInputs.ChangeUnderlyingPrice == 0 && simInputs.UnderlyingPriceAsAbosluteValue && simInputs.ChangeVolatility == 1)
                {
                    optionGreeks.SimulatedPrice = optionGreeks.SelectedFeedPrice;
                }

                if (OMIUpdatedLiveData.Delta != double.MinValue)
                {
                    optionGreeks.Delta = OMIUpdatedLiveData.Delta;
                }

                calculatedGreeks.Add(optionGreeks.Symbol, optionGreeks);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return optionRes;
        }

        private void SendStaticOptionChainRequest(string symbol, OptionChainFilter optionChainFilter)
        {
            try
            {
                _pricingService.SubscribeStaticOptionChain(symbol, optionChainFilter);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SendStepAnalysisDataToClient(List<StepAnalysisResponse> stepAnalysisList, string userID, int hashCode)
        {
            try
            {
                if (stepAnalysisList != null && stepAnalysisList.Count > 0)
                {
                    List<object> objectState = new List<object>();
                    objectState.Add(stepAnalysisList);
                    objectState.Add(userID);
                    objectState.Add(hashCode);
                    _stepAnalysisBufferBatchBlock.Post(objectState);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SnapshotResponseOnNewThread(SymbolData data)
        {
            try
            {
                Thread.Sleep(300);
                if (data == null)
                    return;

                if (data is OptionSymbolData)
                {
                    UserOptModelInput optInputs = OptionModelUserInputCache.GetUserOMIData(data.Symbol);

                    if (optInputs != null)
                    {
                        if (!optInputs.LastPriceUsed && !FeedPriceChooser.UseClosingMark)
                        {
                            if (optInputs.TheoreticalPriceUsed)
                            {
                                FeedPriceChooser.SetSelectedFeedPrice(ref data);
                            }
                        }
                    }
                }

                PricingModelData pricingData = null;
                List<SymbolData> lstOptions = new List<SymbolData>();
                string underlyingSymbol = string.Empty;
                SymbolData underlyingData = null;
                List<VolSkewObject> volSkewReqUpdated = new List<VolSkewObject>();

                foreach (KeyValuePair<string, SubscriberData> subscriberDataKeyValue in SubscriberCollection.SubscriberCollectionData)
                {
                    //list views contains all those views for which this particular symbol is requested.
                    List<SubscriberViewData> listViews = subscriberDataKeyValue.Value.GetVeiwsToBeProcessed(data);
                    if (listViews.Count == 0)
                    {
                        continue;
                    }
                    foreach (SubscriberViewData subscriberView in listViews)
                    {
                        if (data.CategoryCode == AssetCategory.EquityOption || data.CategoryCode == AssetCategory.FutureOption)
                        {
                            underlyingSymbol = data.UnderlyingSymbol;
                            List<SymbolData> lstUnderlyingData = subscriberView.GetSymbolData(data.Symbol, underlyingSymbol, data.CategoryCode, true);

                            if (lstUnderlyingData.Count > 0)
                            {
                                underlyingData = lstUnderlyingData[0];

                                // TODO : Need to make sure that symbol is unique List of options is
                                // maintained so that we can wait for underlying symbol
                                lstOptions.Add(data);
                            }
                        }
                        else
                        {
                            //sending the Response for the non option Data
                            if (subscriberView.checkifSymbolRequested(data.Symbol, data.Symbol, true))
                            {
                                subscriberView.RemoveSymbol(data.Symbol, data.Symbol, data.CategoryCode);

                                ResponseObj Level1Response = new ResponseObj();
                                Level1Response.Data = data;

                                SendSnapshotDataToClient(Level1Response, subscriberDataKeyValue.Key, subscriberView.SubscriberHashCode);
                            }

                            if (subscriberView.IsVolSkewRequest)
                            {
                                List<VolSkewObject> volSkewReqlist = subscriberView.GetVolSkewData(data.Symbol, false);
                                if (volSkewReqlist != null)
                                {
                                    ProcessVolSkewReq(volSkewReqlist, data, subscriberView, subscriberDataKeyValue.Key);
                                }
                            }
                            subscriberView.RemoveSymbol(data.Symbol, data.Symbol, data.CategoryCode);
                            lstOptions = subscriberView.GetSymbolData(data.Symbol, data.Symbol, data.CategoryCode, false);
                            underlyingData = data;
                        }

                        if (underlyingData != null && lstOptions.Count > 0)
                        {
                            foreach (SymbolData optiondata in lstOptions)
                            {
                                if (subscriberView.checkifSymbolRequested(optiondata.Symbol, optiondata.UnderlyingSymbol, false))
                                {
                                    if (subscriberView.IsStressTestRequest)
                                    {
                                        if (!subscriberView.IsVolSkewRequest)
                                        {
                                            pricingData = CreatePricingModelDataObjectFromSymbolData(optiondata, underlyingData);
                                            ResponseObj optionRes = SendSnapShotResponseForGreeksCalculation(subscriberView, pricingData);
                                            if (optionRes.CalculatedGreeks.Count > 0)
                                            {
                                                SendSnapshotDataToClient(optionRes, subscriberDataKeyValue.Key, subscriberView.SubscriberHashCode);
                                            }
                                            subscriberView.RemoveSymbol(optiondata.Symbol, optiondata.UnderlyingSymbol, optiondata.CategoryCode);
                                        }
                                    }
                                    else
                                    {
                                        pricingData = CreatePricingModelDataObjectFromSymbolData(optiondata, underlyingData);
                                        subscriberView.SetStepAnalysisData(pricingData);
                                        if (!subscriberView.IsVolSkewRequest)
                                        {
                                            Dictionary<string, StepAnalysisResponse> dictstepAnal = subscriberView.GetStepAnalysisData(optiondata.Symbol);
                                            SendGreeksCalculatedDataToClient(dictstepAnal, subscriberView.SubscriberHashCode, subscriberView.SubscriberViewID, subscriberDataKeyValue.Key);
                                            subscriberView.RemoveSymbol(optiondata.Symbol, optiondata.UnderlyingSymbol, optiondata.CategoryCode);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion private-members

        #region IGreekAnalysisServices Memebers

        public void RemoveCallbackDetails()
        {
            try
            {
                _snapshotCallCounter = 0;
                _stepAnalysisCallCounter = 0;
                //_pricingService.OptionChainResponse -= new EventHandler<EventArgs<string, List<OptionStaticData>>>(_pricingService_OptionChainResponse);
                _pricingService.UnSubscribe(this);
                // Unsubscribe the control to stop receiving data
                OperationContext.Current.InstanceContext.ReleaseServiceInstance();
                _greekAnalysisCallbackSyncronisationContext = null;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void RequestSnapshotData(InputParametersCollection inputParams)
        {
            try
            {
                SubscriberCollection.RegisterSymbolsForSnapShot(inputParams);
                foreach (SubscriberViewInputs inputs in inputParams.DictSubscriberInputs.Values)
                {
                    if (inputs.IsVolSkewRequest)
                    {
                        _timer.Change(1000, 1000);
                        break;
                    }
                }

                _pricingService.RequestSnapshot(inputParams.ListUniqueSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, this, true);

                if (inputParams.DictFXSymbols.Count > 0)
                {
                    List<fxInfo> listFxSymbols = new List<fxInfo>(inputParams.DictFXSymbols.Values);
                    _pricingService.RequestSnapshot(listFxSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, this, true);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void RequestStepAnalysisData(InputParametersCollection inputParamsForStepAnalysis)
        {
            try
            {
                SubscriberCollection.RegisterSymbolsForSnapShot(inputParamsForStepAnalysis);

                //Bharat Kumar Jangir (11 December 2013)
                //handling two cases
                //1. Step Analysis using LiveFeed Data
                //2. Step Analysis using Stress Test Data
                bool isStepAnalysisUsingLiveFeedData = false;

                foreach (KeyValuePair<string, SubscriberData> subscriberDataKeyValue in SubscriberCollection.SubscriberCollectionData)
                {
                    //list-views contains all those views for which this particular symbol is requested.
                    List<SubscriberViewData> listViews = subscriberDataKeyValue.Value.GetViewsToBeProcessed();
                    if (listViews.Count == 0)
                    {
                        continue;
                    }
                    foreach (SubscriberViewData subscriberView in listViews)
                    {
                        if (!subscriberView.StepAnalysisUsingStressData)
                        {
                            isStepAnalysisUsingLiveFeedData = true;
                            break;
                        }

                        PricingModelData pricingData = null;
                        foreach (StepAnalysisResponse stepAnalReqObj in subscriberView.ListStepAnalysisInputs)
                        {
                            pricingData = CreatePricingModelDataObjectFromStressInputData(stepAnalReqObj);
                            subscriberView.SetStepAnalysisData(pricingData);
                            Dictionary<string, StepAnalysisResponse> dictstepAnal = subscriberView.GetStepAnalysisData(stepAnalReqObj.Symbol);
                            SendGreeksCalculatedDataToClient(dictstepAnal, subscriberView.SubscriberHashCode, subscriberView.SubscriberViewID, subscriberDataKeyValue.Key);
                        }
                    }
                    if (isStepAnalysisUsingLiveFeedData)
                    {
                        break;
                    }
                }

                if (isStepAnalysisUsingLiveFeedData)
                {
                    _pricingService.RequestSnapshot(inputParamsForStepAnalysis.ListUniqueSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, this, true);

                    if (inputParamsForStepAnalysis.DictFXSymbols.Count > 0)
                    {
                        List<fxInfo> listFxSymbols = new List<fxInfo>(inputParamsForStepAnalysis.DictFXSymbols.Values);
                        _pricingService.RequestSnapshot(listFxSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, this, true);
                    }

                    foreach (SubscriberViewInputs inputs in inputParamsForStepAnalysis.DictSubscriberInputs.Values)
                    {
                        if (inputs.IsVolSkewRequest)
                        {
                            _timer.Change(2000, 1000);
                            break;
                        }
                    }
                    _greeksCount = 0;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SendCallbackDetails()
        {
            try
            {

                SynchronizationContext.SetSynchronizationContext(new OperationContextPreservingSynchronizationContext(OperationContext.Current));
                _greekAnalysisCallbackSyncronisationContext = SynchronizationContext.Current as OperationContextPreservingSynchronizationContext;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SetupPricingSevice()
        {
            try
            {
                this._pricingService = PricingInstanceHandler.GetInstance.PricingService;
                //_pricingService.OptionChainResponse += new EventHandler<EventArgs<string, List<OptionStaticData>>>(_pricingService_OptionChainResponse);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion IGreekAnalysisServices Memebers

        #region ILiveFeedCallback Members
        public void LiveFeedConnected()
        {
            //Do nothing
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedDisConnected()
        {
            //Do nothing
        }

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                SymbolDataHandler symbolDataHandler = new SymbolDataHandler(SnapshotResponseOnNewThread);
                symbolDataHandler.BeginInvoke(data, null, null);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion ILiveFeedCallback Members

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion

        #region IDisposable Menmbers

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Dispose();
                _timerToSendSnapshotData.Dispose();
                _timerToSendStepAnalysisData.Dispose();
                _timerTriggerSnapshotPipeline.Dispose();
                _timerTrigerStepAnalysisPipeline.Dispose();
            }
        }

        #endregion IDisposable Menmbers
    }
}