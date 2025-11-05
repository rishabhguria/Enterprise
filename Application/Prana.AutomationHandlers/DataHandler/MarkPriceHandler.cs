using System;
using System.Collections.Generic;
using System.Text;
using Prana.Interfaces;
using System.Collections;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.CommonDataCache;
using Prana.BusinessObjects.AppConstants;

namespace Prana.AutomationHandlers
{
    class MarkPriceHandler : IAutomationDataHandler
    {        
        const string DATEFORMAT = "MM/dd/yyyy";

        private IPranaPositionServices _positionServices;
        public IPranaPositionServices PositionServices
        {
            set
            {
                _positionServices = value;
            }
        }
        private ISecMasterServices _secMasterServices;
        public ISecMasterServices SecMasterServices
        {
            set 
            {
                _secMasterServices = value;
            }
        }
        private IRiskServices _riskServices = null;
        public IRiskServices RiskServices
        {
            set
            {
                _riskServices = value;
            }
        }

        private IAllocationServices _allocationServices;
        public IAllocationServices AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
        }

        public IList RetrieveData(ClientSettings clientSetting)
        {
            List<TaxLot> taxlots = _positionServices.GetOpenPositionsFromAnyDB(DateTime.Now, clientSetting.InputDataLocationPath, null);
            return taxlots;
        }

        public void ProcessData(ClientSettings clientSetting, IList data)
        {
            List<MarkPriceImport> markPriceList = ValidateFromSecMaster(clientSetting, data);

            AutomationHandlerDataManager.SaveRunUploadFileDataForMarkPrice(markPriceList, clientSetting.DataBaseSettings.ClientDB);
        }

        public List<MarkPriceImport> ValidateFromSecMaster(ClientSettings clientSetting, IList data)
        {
            List<MarkPriceImport> markPriceList = new List<MarkPriceImport>();
            foreach (Object markPriceData in data)
            {
                markPriceList.Add((MarkPriceImport)markPriceData);
            }
            GetSMDataForMarkPriceImport(markPriceList, clientSetting);
            return markPriceList;
        }

        private void GetSMDataForMarkPriceImport(List<MarkPriceImport> markPriceList, ClientSettings clientSetting)
        {
            try
            {
                Dictionary<int, Dictionary<string, List<MarkPriceImport>>> _markPriceSymbologyWiseDict = CreateMarkPriceDictionary(markPriceList, clientSetting);
              
                if (_markPriceSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<MarkPriceImport>>> kvp in _markPriceSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<MarkPriceImport>> symbolDict = _markPriceSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<MarkPriceImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                                secMasterRequestObj.AddNewRow();
                            }
                        }
                    }

                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = _secMasterServices.GetSecMasterDataForListSync(secMasterRequestObj, secMasterRequestObj.HashCode);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                            string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                            string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                            string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                            string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                            string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                            string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                            string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                            int requestedSymbologyID = secMasterObj.RequestedSymbology;

                            if (_markPriceSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<MarkPriceImport>> dictSymbols = _markPriceSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<MarkPriceImport> listMarkPrice = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (MarkPriceImport markPriceImport in listMarkPrice)
                                    {
                                        markPriceImport.Symbol = pranaSymbol;
                                        markPriceImport.CUSIP = cuspiSymbol;
                                        markPriceImport.ISIN = isinSymbol;
                                        markPriceImport.SEDOL = sedolSymbol;
                                        markPriceImport.Bloomberg = bloombergSymbol;
                                        markPriceImport.RIC = reutersSymbol;
                                        markPriceImport.OSIOptionSymbol = osiOptionSymbol;
                                        markPriceImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        markPriceImport.AUECID = secMasterObj.AUECID;
                                        markPriceImport.OSIOptionSymbol = osiOptionSymbol;
                                        markPriceImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        markPriceImport.OpraOptionSymbol = opraOptionSymbol;

                                        if (markPriceImport.IsForexRequired.Trim().ToUpper().Equals("TRUE"))
                                            UpdateMarkPriceObj(markPriceImport, secMasterObj, clientSetting);

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private Dictionary<int, Dictionary<string, List<MarkPriceImport>>> CreateMarkPriceDictionary(List<MarkPriceImport> markPriceList, ClientSettings clientSetting)
        {
            Dictionary<int, Dictionary<string, List<MarkPriceImport>>> _markPriceSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<MarkPriceImport>>>();

            try
            {
                List<MarkPriceImport> _markPriceValueCollection = new List<MarkPriceImport>();
                List<string> uniqueIdsList = new List<string>();
                foreach (MarkPriceImport markPriceValue in markPriceList)
                {
                    if (!markPriceValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(markPriceValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(markPriceValue.Date));
                            markPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(markPriceValue.Date);
                            markPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }


                    string uniqueID = string.Empty;

                    if (!String.IsNullOrEmpty(markPriceValue.Symbol))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.Symbol;

                        if (_markPriceSymbologyWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[0];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.Symbol))
                            {
                                List<MarkPriceImport> markPriceSymbolWiseList = markPriceSameSymbologyDict[markPriceValue.Symbol];
                                markPriceSymbolWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.Symbol] = markPriceSymbolWiseList;
                                _markPriceSymbologyWiseDict[0] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[0].Add(markPriceValue.Symbol, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbolDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameSymbolDict.Add(markPriceValue.Symbol, markPricelist);
                            _markPriceSymbologyWiseDict.Add(0, markPriceSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.RIC))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.RIC;

                        if (_markPriceSymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[1];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.RIC))
                            {
                                List<MarkPriceImport> markPriceRICWiseList = markPriceSameSymbologyDict[markPriceValue.RIC];
                                markPriceRICWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.RIC] = markPriceRICWiseList;
                                _markPriceSymbologyWiseDict[1] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPirceList = new List<MarkPriceImport>();
                                markPirceList.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[1].Add(markPriceValue.RIC, markPirceList);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPirceList = new List<MarkPriceImport>();
                            markPirceList.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameRICDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameRICDict.Add(markPriceValue.RIC, markPirceList);
                            _markPriceSymbologyWiseDict.Add(1, markPriceSameRICDict);
                        }

                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.ISIN))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.ISIN;

                        if (_markPriceSymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[2];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.ISIN))
                            {
                                List<MarkPriceImport> markPriceISINWiseList = markPriceSameSymbologyDict[markPriceValue.ISIN];
                                markPriceISINWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.ISIN] = markPriceISINWiseList;
                                _markPriceSymbologyWiseDict[2] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[2].Add(markPriceValue.ISIN, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameISINDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameISINDict.Add(markPriceValue.ISIN, markPricelist);
                            _markPriceSymbologyWiseDict.Add(2, markPriceSameISINDict);
                        }

                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.SEDOL))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.SEDOL;
                        if (_markPriceSymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[3];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.SEDOL))
                            {
                                List<MarkPriceImport> markPriceSEDOLWiseList = markPriceSameSymbologyDict[markPriceValue.SEDOL];
                                markPriceSEDOLWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.SEDOL] = markPriceSEDOLWiseList;
                                _markPriceSymbologyWiseDict[3] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[3].Add(markPriceValue.SEDOL, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSEDOLDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSEDOLDict.Add(markPriceValue.SEDOL, markPricelist);
                            _markPriceSymbologyWiseDict.Add(3, markPriceSEDOLDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.CUSIP))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.CUSIP;
                        if (_markPriceSymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[4];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.CUSIP))
                            {
                                List<MarkPriceImport> markPriceCUSIPWiseList = markPriceSameSymbologyDict[markPriceValue.CUSIP];
                                markPriceCUSIPWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.CUSIP] = markPriceCUSIPWiseList;
                                _markPriceSymbologyWiseDict[4] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[4].Add(markPriceValue.CUSIP, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameCUSIPDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameCUSIPDict.Add(markPriceValue.CUSIP, markPricelist);
                            _markPriceSymbologyWiseDict.Add(4, markPriceSameCUSIPDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.Bloomberg))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.Bloomberg;

                        if (_markPriceSymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[5];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.Bloomberg))
                            {
                                List<MarkPriceImport> markPriceBloombergWiseList = markPriceSameSymbologyDict[markPriceValue.Bloomberg];
                                markPriceBloombergWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.Bloomberg] = markPriceBloombergWiseList;
                                _markPriceSymbologyWiseDict[5] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[5].Add(markPriceValue.Bloomberg, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameBloombergDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameBloombergDict.Add(markPriceValue.Bloomberg, markPricelist);
                            _markPriceSymbologyWiseDict.Add(5, markPriceSameBloombergDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.OSIOptionSymbol))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.OSIOptionSymbol;

                        if (_markPriceSymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[6];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.OSIOptionSymbol))
                            {
                                List<MarkPriceImport> markPriceOSIWiseList = markPriceSameSymbologyDict[markPriceValue.OSIOptionSymbol];
                                markPriceOSIWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.OSIOptionSymbol] = markPriceOSIWiseList;
                                _markPriceSymbologyWiseDict[6] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[6].Add(markPriceValue.OSIOptionSymbol, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameOSIDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameOSIDict.Add(markPriceValue.OSIOptionSymbol, markPricelist);
                            _markPriceSymbologyWiseDict.Add(6, markPriceSameOSIDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.IDCOOptionSymbol))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.IDCOOptionSymbol;

                        if (_markPriceSymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[7];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.IDCOOptionSymbol))
                            {
                                List<MarkPriceImport> markPriceIDCOWiseList = markPriceSameSymbologyDict[markPriceValue.IDCOOptionSymbol];
                                markPriceIDCOWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.IDCOOptionSymbol] = markPriceIDCOWiseList;
                                _markPriceSymbologyWiseDict[7] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[7].Add(markPriceValue.IDCOOptionSymbol, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameIDCODict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameIDCODict.Add(markPriceValue.IDCOOptionSymbol, markPricelist);
                            _markPriceSymbologyWiseDict.Add(7, markPriceSameIDCODict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.OpraOptionSymbol))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.OpraOptionSymbol;

                        if (_markPriceSymbologyWiseDict.ContainsKey(8))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[8];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.OpraOptionSymbol))
                            {
                                List<MarkPriceImport> markPriceOpraWiseList = markPriceSameSymbologyDict[markPriceValue.OpraOptionSymbol];
                                markPriceOpraWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.OpraOptionSymbol] = markPriceOpraWiseList;
                                _markPriceSymbologyWiseDict[8] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[8].Add(markPriceValue.OpraOptionSymbol, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameOpraDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameOpraDict.Add(markPriceValue.OpraOptionSymbol, markPricelist);
                            _markPriceSymbologyWiseDict.Add(7, markPriceSameOpraDict);
                        }
                    }


                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _markPriceValueCollection.Add(markPriceValue);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _markPriceSymbologyWiseDict;
        }

        private void UpdateMarkPriceObj(MarkPriceImport markPriceImport, SecMasterBaseObj secMasterObj, ClientSettings clientSetting)
        {
            if (!string.IsNullOrEmpty(markPriceImport.Date))
            {
                if (ForexConverter.CompanyBaseCurrency != secMasterObj.CurrencyID)
                {
                    ForexConverter.GetInstance(clientSetting.ClientInformation.CompanyID, Convert.ToDateTime(markPriceImport.Date)).GetForexData(Convert.ToDateTime(markPriceImport.Date));
                    ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(clientSetting.ClientInformation.CompanyID).GetConversionRateFromCurrenciesForGivenDate(ForexConverter.CompanyBaseCurrency, secMasterObj.CurrencyID, Convert.ToDateTime(markPriceImport.Date));
                    if (conversionRate != null)
                    {
                        if (conversionRate.ConversionMethod == Operator.D)
                        {
                            if (conversionRate.RateValue != 0)
                                markPriceImport.MarkPrice = markPriceImport.MarkPrice / conversionRate.RateValue;
                        }
                        else
                        {
                            markPriceImport.MarkPrice = markPriceImport.MarkPrice * conversionRate.RateValue;
                        }
                    }
                }
            }
        }

        
        
    }
}
