using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.OptionCalculator.Common
{
    public class SubscriberViewData
    {
        #region private-variables

        private Dictionary<string, OptionSimulationInputs> _dictOptSimChanges = new Dictionary<string, OptionSimulationInputs>();
        private Dictionary<string, List<VolSkewObject>> _dictProxySymbolWise = new Dictionary<string, List<VolSkewObject>>();
        private Dictionary<string, List<string>> _dictRequestedSymbols = new Dictionary<string, List<string>>();
        private Dictionary<string, int> _dictRequestWiseProxyExpMonths = new Dictionary<string, int>();
        private Dictionary<string, SymbolData> _dictResponseSymbols = new Dictionary<string, SymbolData>();
        private Dictionary<string, List<VolSkewObject>> _dictVolSkewRequest = new Dictionary<string, List<VolSkewObject>>();
        private bool _isStressTestRequest;
        private bool _isVolSkewRequest;
        private List<string> _listNonOptions = new List<string>();
        private List<StepAnalysisResponse> _listStepAnalysisInputs = new List<StepAnalysisResponse>();
        private object _locker = new object();
        private Dictionary<string, Dictionary<string, StepAnalysisResponse>> _stepAnalResDict = new Dictionary<string, Dictionary<string, StepAnalysisResponse>>();
        private bool _stepAnalysisUsingStressData = false;
        private int _subscriberHashCode;
        private string _subscriberViewID = string.Empty;
        private double _toleranceValue;
        private Dictionary<string, List<VolSkewObject>> _volSkewReqDictUnderlying = new Dictionary<string, List<VolSkewObject>>();

        #endregion private-variables

        #region properties

        public bool IsStressTestRequest
        {
            get { return _isStressTestRequest; }
            set { _isStressTestRequest = value; }
        }

        public bool IsVolSkewRequest
        {
            get { return _isVolSkewRequest; }
            set { _isVolSkewRequest = value; }
        }

        public List<string> ListNonOptions
        {
            get { return _listNonOptions; }
            set { _listNonOptions = value; }
        }

        public List<StepAnalysisResponse> ListStepAnalysisInputs
        {
            get { return _listStepAnalysisInputs; }
            set { _listStepAnalysisInputs = value; }
        }

        public bool StepAnalysisUsingStressData
        {
            get { return _stepAnalysisUsingStressData; }
            set { _stepAnalysisUsingStressData = value; }
        }

        public int SubscriberHashCode
        {
            get { return _subscriberHashCode; }
            set { _subscriberHashCode = value; }
        }


        public string SubscriberViewID
        {
            get { return _subscriberViewID; }
            set { _subscriberViewID = value; }
        }

        public double ToleranceValue
        {
            get { return _toleranceValue; }
            set { _toleranceValue = value; }
        }

        #endregion properties

        #region public-members

        public void AddSubscriberInputParameters(SubscriberViewInputs inputs)
        {
            ClearCache();
            _dictOptSimChanges = inputs.DictOptSimInputs;
            _dictOptSimChanges = inputs.DictOptSimInputs;
            _dictRequestedSymbols = inputs.DictSymbolOptions;
            _isStressTestRequest = inputs.IsStressTestRequest;
            _isVolSkewRequest = inputs.IsVolSkewRequest;
            _listNonOptions = inputs.ListNonOptions;
            _listStepAnalysisInputs = inputs.ListStepAnalysisInputs;
            _stepAnalResDict = inputs.StepAnalResDict;
            _stepAnalysisUsingStressData = inputs.StepAnalysisUsingStressData;
            _subscriberHashCode = inputs.HashCode;
            _subscriberViewID = inputs.ID;
            _toleranceValue = inputs.ToleranceValue;
            _volSkewReqDictUnderlying = inputs.DictVolSkewReq;
        }

        public bool CheckIFProcessingRequired()
        {
            lock (_locker)
            {
                if (_dictRequestedSymbols.Count > 0)
                {
                    lock (_dictProxySymbolWise)
                    {
                        if (_dictProxySymbolWise.Count > 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return false;
            }
        }

        public bool CheckIfSnapshotAlreadyRequested(string symbol)
        {
            lock (_locker)
            {
                if (_dictResponseSymbols.ContainsKey(symbol))
                {
                    return true;
                }
                return false;
            }
        }

        public bool checkifSymbolRequested(string symbol, string UnderlyingSymbol, bool isNonOption)
        {
            if (!isNonOption)
            {
                lock (_locker)
                {
                    if (_dictRequestedSymbols.ContainsKey(UnderlyingSymbol))
                    {
                        if (_dictRequestedSymbols[UnderlyingSymbol].Contains(symbol))
                        {
                            return true;
                        }
                    }
                }
                lock (_dictProxySymbolWise)
                {
                    if (_dictProxySymbolWise.ContainsKey(symbol))
                    {
                        return true;
                    }
                }
            }
            else
            {
                lock (_locker)
                {
                    if (_listNonOptions.Contains(symbol))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<ProxyMappedData> FetchProxySymbolMappedData()
        {
            List<ProxyMappedData> listSymbolMapping = new List<ProxyMappedData>();
            try
            {
                lock (_dictProxySymbolWise)
                {
                    foreach (KeyValuePair<string, List<VolSkewObject>> kp in _dictProxySymbolWise)
                    {
                        string proxySymbol = kp.Key;
                        List<VolSkewObject> listVolSkewReq = kp.Value;
                        if (listVolSkewReq.Count > 0)
                        {
                            foreach (VolSkewObject obj in listVolSkewReq)
                            {
                                lock (_locker)
                                {
                                    if (_dictResponseSymbols.ContainsKey(obj.Symbol) && _dictResponseSymbols.ContainsKey(proxySymbol) && _dictResponseSymbols.ContainsKey(obj.UnderlyingSymbol))
                                    {
                                        ProxyMappedData mappedData = new ProxyMappedData();
                                        mappedData.ProxySymbolData = _dictResponseSymbols[proxySymbol];
                                        SymbolData parentSymbolData = _dictResponseSymbols[obj.Symbol];
                                        mappedData.UnderlyingSymbolData = _dictResponseSymbols[obj.UnderlyingSymbol];
                                        mappedData.DictParentSymbols.Add(parentSymbolData.Symbol, parentSymbolData);
                                        listSymbolMapping.Add(mappedData);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return listSymbolMapping;
        }

        public int GetProxyExpirationMonthForGivenRequestID(string requestID)
        {
            int proxyExpMonth = 0;
            lock (_dictRequestWiseProxyExpMonths)
            {
                if (_dictRequestWiseProxyExpMonths.ContainsKey(requestID))
                {
                    proxyExpMonth = _dictRequestWiseProxyExpMonths[requestID];
                    _dictRequestWiseProxyExpMonths.Remove(requestID);
                }
            }
            return proxyExpMonth;
        }

        public List<OptionSimulationInputs> GetSimulationInputs(string optionSymbol)
        {
            List<OptionSimulationInputs> OptionInputs = new List<OptionSimulationInputs>();
            try
            {
                if (IsStressTestRequest)
                {
                    if (_dictOptSimChanges.ContainsKey(optionSymbol))
                    {
                        OptionInputs.Add(_dictOptSimChanges[optionSymbol]);
                    }
                    return OptionInputs;
                }
                else
                {
                    Dictionary<string, StepAnalysisResponse> dictOfstepAnal = null;
                    lock (_stepAnalResDict)
                    {
                        if (_stepAnalResDict.ContainsKey(optionSymbol))
                        {
                            dictOfstepAnal = _stepAnalResDict[optionSymbol];
                        }
                        if (dictOfstepAnal != null)
                        {
                            foreach (KeyValuePair<string, StepAnalysisResponse> kp in dictOfstepAnal)
                            {
                                StepAnalysisResponse stepAnal = kp.Value;
                                int steps = 0;
                                for (double value1 = stepAnal.StepParameter_X.Low; ;)
                                {
                                    OptionSimulationInputs simInput = new OptionSimulationInputs();
                                    if (stepAnal.StepParameter_X.ParameterCode == StepAnalParameterCode.DaysToExpiration)
                                    {
                                        simInput.ChangeDaysToExpiration = Convert.ToInt32(value1);
                                    }
                                    else if (stepAnal.StepParameter_X.ParameterCode == StepAnalParameterCode.UnderlyingPrice)
                                    {
                                        simInput.ChangeUnderlyingPrice = (1 + value1 / 100);
                                    }
                                    OptionInputs.Add(simInput);
                                    value1 = value1 + stepAnal.StepParameter_X.StepDifference;
                                    if (steps == stepAnal.StepParameter_X.Steps)
                                    {
                                        break;
                                    }
                                    steps++;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return OptionInputs;
        }

        public Dictionary<string, StepAnalysisResponse> GetStepAnalysisData(string optSymbol)
        {
            Dictionary<string, StepAnalysisResponse> dictStepAnal = null;
            lock (_stepAnalResDict)
            {
                if (_stepAnalResDict.ContainsKey(optSymbol))
                {
                    dictStepAnal = _stepAnalResDict[optSymbol];
                    _stepAnalResDict.Remove(optSymbol);
                }
            }
            return dictStepAnal;
        }

        public List<SymbolData> GetSymbolData(string symbol, string underlyingSymbol, AssetCategory categoryCode, bool isUnderlyingDataRequired)
        {
            List<SymbolData> lstSymbol = new List<SymbolData>();
            lock (_locker)
            {
                if (categoryCode == AssetCategory.EquityOption || categoryCode == AssetCategory.FutureOption)
                {
                    if (isUnderlyingDataRequired)
                    {
                        if (_dictResponseSymbols.ContainsKey(underlyingSymbol))
                        {
                            lstSymbol.Add(_dictResponseSymbols[underlyingSymbol]);
                        }
                        return lstSymbol;
                    }
                    else
                    {
                        if (_dictResponseSymbols.ContainsKey(symbol))
                        {
                            lstSymbol.Add(_dictResponseSymbols[symbol]);
                        }
                        return lstSymbol;
                    }
                }
                else
                {
                    string underlyingSymbol_new = string.Empty;
                    List<string> lstOptions = new List<string>();
                    if (_dictRequestedSymbols.ContainsKey(symbol))
                    {
                        lstOptions = _dictRequestedSymbols[symbol];
                    }
                    foreach (string optSymbol in lstOptions)
                    {
                        SymbolData optData = null;
                        if (_dictResponseSymbols.ContainsKey(optSymbol))
                        {
                            optData = _dictResponseSymbols[optSymbol];
                        }
                        if (optData != null)
                        {
                            underlyingSymbol_new = optData.UnderlyingSymbol;
                            if ((optData.CategoryCode == AssetCategory.FutureOption || optData.CategoryCode == AssetCategory.EquityOption) && symbol == underlyingSymbol_new)
                            {
                                lstSymbol.Add(optData);
                            }
                        }
                    }
                    return lstSymbol;
                }
            }
        }

        public List<VolSkewObject> GetVolSkewData(string Symbol, bool isProxySymbol)
        {
            List<VolSkewObject> listVolSkew = new List<VolSkewObject>();
            if (!isProxySymbol)
            {
                lock (_volSkewReqDictUnderlying)
                {
                    if (_volSkewReqDictUnderlying.Count == 0)
                        return listVolSkew;
                    if (_volSkewReqDictUnderlying.ContainsKey(Symbol))
                    {
                        listVolSkew.AddRange(_volSkewReqDictUnderlying[Symbol]);
                        _volSkewReqDictUnderlying.Remove(Symbol);
                    }
                    return listVolSkew;
                }
            }
            else
            {
                lock (_dictProxySymbolWise)
                {
                    if (_dictProxySymbolWise.Count == 0)
                        return listVolSkew;
                    if (_dictProxySymbolWise.ContainsKey(Symbol))
                    {
                        listVolSkew.AddRange(_dictProxySymbolWise[Symbol]);
                    }
                }
                return listVolSkew;
            }
        }

        public List<VolSkewObject> GetVolSkewObjectlist(string requestID)
        {
            List<VolSkewObject> volSkewobjlist = null;
            lock (_dictVolSkewRequest)
            {
                if (_dictVolSkewRequest.ContainsKey(requestID))
                {
                    volSkewobjlist = _dictVolSkewRequest[requestID];
                    _dictVolSkewRequest.Remove(requestID);
                }
                return volSkewobjlist;
            }
        }

        public void RemovefromProxySymbolDict(string ProxySymbol, VolSkewObject obj)
        {
            lock (_dictProxySymbolWise)
            {
                if (_dictProxySymbolWise.ContainsKey(ProxySymbol))
                {
                    List<VolSkewObject> listVolSkew = _dictProxySymbolWise[ProxySymbol];
                    if (listVolSkew.Contains(obj))
                    {
                        listVolSkew.Remove(obj);
                    }
                    if (_dictProxySymbolWise[ProxySymbol].Count == 0)
                    {
                        _dictProxySymbolWise.Remove(ProxySymbol);
                    }
                }
            }
        }

        public void RemoveSymbol(string symbol, string underlyingSymbol, AssetCategory categoryCode)
        {
            lock (_locker)
            {
                if (categoryCode == AssetCategory.EquityOption || categoryCode == AssetCategory.FutureOption)
                {
                    if (_dictRequestedSymbols.ContainsKey(underlyingSymbol))
                    {
                        _dictRequestedSymbols[underlyingSymbol].Remove(symbol);
                        if (_dictRequestedSymbols[underlyingSymbol].Count == 0)
                        {
                            _dictRequestedSymbols.Remove(underlyingSymbol);
                        }
                    }
                }
                else
                {
                    if (_listNonOptions.Contains(symbol))
                    {
                        _listNonOptions.Remove(symbol);
                    }
                }
            }
        }

        public void SetStepAnalysisData(PricingModelData pricingData)
        {
            lock (_stepAnalResDict)
            {
                if (_stepAnalResDict.ContainsKey(pricingData.OptSymbol))
                {
                    bool isOMIInterestRateProvided = false;
                    Dictionary<string, StepAnalysisResponse> dictOfStepResponse = new Dictionary<string, StepAnalysisResponse>();
                    Dictionary<string, StepAnalysisResponse> dictOfstepAnal = _stepAnalResDict[pricingData.OptSymbol]; ;
                    if (dictOfstepAnal.Count == 1)
                    {
                        foreach (KeyValuePair<string, StepAnalysisResponse> kp in dictOfstepAnal)
                        {
                            if (pricingData.InterestRate == double.MinValue)
                            {
                                DateTime auecMarketEndTime = PricingUtilityCache.GetMarketEndTimeForAUEC(pricingData.AUECID);
                                DateTime auecLocalTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(pricingData.AUECID));

                                auecLocalTime = auecLocalTime <= DateTimeConstants.MinValue ? DateTime.UtcNow : auecLocalTime;
                                TimeSpan ts2 = auecMarketEndTime.Subtract(auecLocalTime.Date);
                                DateTime expirationDate = pricingData.ExpirationDate.AddDays(ts2.TotalDays);
                                TimeSpan ts = expirationDate.Subtract(auecLocalTime) + ts2;
                                double daysToExpiration = ts.TotalDays;
                                daysToExpiration = daysToExpiration < 0 ? 0 : daysToExpiration;
                                pricingData.InterestRate = RiskPreferenceManager.GetInterestRate(daysToExpiration);
                            }
                            else
                            {
                                isOMIInterestRateProvided = true;
                            }
                            StepAnalysisResponse stepAnal = kp.Value;
                            stepAnal.InputParameters.SetValues(pricingData);
                            int steps = 0;
                            for (double value1 = stepAnal.StepParameter_X.Low; ;)
                            {
                                StepAnalysisResponse stepres = (StepAnalysisResponse)stepAnal.Clone();
                                stepres.Symbol = pricingData.OptSymbol;
                                stepres.InputParameters.Key = value1.ToString();
                                stepres.SetXParameters(value1);
                                if (!isOMIInterestRateProvided)
                                {
                                    if (stepres.StepParameter_X.ParameterCode.Equals(StepAnalParameterCode.DaysToExpiration))
                                    {
                                        DateTime auecMarketEndTime = PricingUtilityCache.GetMarketEndTimeForAUEC(stepres.InputParameters.AUECID);
                                        DateTime auecLocalTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime
                                            (DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(stepres.InputParameters.AUECID));
                                        auecLocalTime = auecLocalTime <= DateTimeConstants.MinValue ? DateTime.UtcNow : auecLocalTime;
                                        TimeSpan ts2 = auecMarketEndTime.Subtract(auecLocalTime.Date);
                                        DateTime expirationDate = stepres.InputParameters.ExpirationDate.AddDays(ts2.TotalDays);
                                        TimeSpan ts = expirationDate.Subtract(auecLocalTime);
                                        double daysToExpiration = ts.TotalDays;
                                        daysToExpiration = daysToExpiration < 0 ? 0 : daysToExpiration;
                                        stepres.InputParameters.InterestRate = RiskPreferenceManager.GetInterestRate(daysToExpiration);
                                    }
                                }
                                if (!dictOfStepResponse.ContainsKey(value1.ToString()))
                                {
                                    dictOfStepResponse.Add(value1.ToString(), stepres);
                                }
                                value1 = value1 + stepAnal.StepParameter_X.StepDifference;
                                if (steps == stepAnal.StepParameter_X.Steps)
                                {
                                    break;
                                }
                                steps++;
                            }
                        }
                        _stepAnalResDict[pricingData.OptSymbol] = dictOfStepResponse;
                    }
                }
            }
        }

        public void SubscribeProxySearchRequest(string requestID, List<VolSkewObject> VolSkewObjlist)
        {
            lock (_dictVolSkewRequest)
            {
                if (!_dictVolSkewRequest.ContainsKey(requestID))
                {
                    _dictVolSkewRequest.Add(requestID, VolSkewObjlist);
                }
            }
        }

        public void UpdateProxyExpirationMonthDictionary(string requestID, int proxyMonth)
        {
            lock (_dictRequestWiseProxyExpMonths)
            {
                if (!_dictRequestWiseProxyExpMonths.ContainsKey(requestID))
                {
                    _dictRequestWiseProxyExpMonths.Add(requestID, proxyMonth);
                }
            }
        }

        public void UpdateProxySymbolDict(string proxyOptSymbol, VolSkewObject Reqobj)
        {
            lock (_dictProxySymbolWise)
            {
                if (_dictProxySymbolWise.ContainsKey(proxyOptSymbol))
                {
                    List<VolSkewObject> listVolSkew = _dictProxySymbolWise[proxyOptSymbol];
                    if (!listVolSkew.Contains(Reqobj))
                    {
                        listVolSkew.Add(Reqobj);
                    }
                }
                else
                {
                    List<VolSkewObject> listVolSkew = new List<VolSkewObject>();
                    listVolSkew.Add(Reqobj);
                    _dictProxySymbolWise.Add(proxyOptSymbol, listVolSkew);
                }
            }
        }

        public bool UpdateSnapShotResponseifProcessingRequired(SymbolData data)
        {
            lock (_locker)
            {
                if (_dictResponseSymbols.ContainsKey(data.Symbol))
                {
                    return false;
                }
            }
            if (data.CategoryCode == AssetCategory.EquityOption || data.CategoryCode == AssetCategory.FutureOption)
            {
                bool isProxySnapShotRequested = false;
                lock (_dictProxySymbolWise)
                {
                    if (_dictProxySymbolWise.ContainsKey(data.Symbol))
                    {
                        isProxySnapShotRequested = true;
                    }
                }
                lock (_locker)
                {
                    if (isProxySnapShotRequested)
                    {
                        _dictResponseSymbols.Add(data.Symbol, data);
                        return true;
                    }
                    if (_dictRequestedSymbols.ContainsKey(data.UnderlyingSymbol))
                    {
                        if (_dictRequestedSymbols[data.UnderlyingSymbol].Contains(data.Symbol))
                        {
                            _dictResponseSymbols.Add(data.Symbol, data);
                        }
                    }
                }
            }
            else
            {
                lock (_locker)
                {
                    if (_listNonOptions.Contains(data.Symbol) || _dictRequestedSymbols.ContainsKey(data.Symbol))
                    {
                        _dictResponseSymbols.Add(data.Symbol, data);
                    }
                }
            }
            return true;
        }

        public void UpdateStepAnalysisResponseWithProxyVol(List<string> stepKeyValues, double proxyImpliedVol, string optionSymbol)
        {
            lock (_stepAnalResDict)
            {
                foreach (string stepKey in stepKeyValues)
                {
                    if (_stepAnalResDict.ContainsKey(optionSymbol))
                    {
                        Dictionary<string, StepAnalysisResponse> dictofStepResponse = _stepAnalResDict[optionSymbol];
                        if (dictofStepResponse.ContainsKey(stepKey))
                        {
                            StepAnalysisResponse stepAnal = dictofStepResponse[stepKey];
                            stepAnal.InputParameters.Volatility = proxyImpliedVol;
                        }
                    }
                }
            }
        }

        #endregion public-members

        #region private-members

        private void ClearCache()
        {
            _dictRequestedSymbols.Clear();
            _listNonOptions.Clear();
            _dictResponseSymbols.Clear();
            _volSkewReqDictUnderlying.Clear();
            _stepAnalResDict.Clear();
            _dictVolSkewRequest.Clear();
            _dictProxySymbolWise.Clear();
            _dictRequestWiseProxyExpMonths.Clear();
            _listStepAnalysisInputs.Clear();
        }

        #endregion private-members
    }
}