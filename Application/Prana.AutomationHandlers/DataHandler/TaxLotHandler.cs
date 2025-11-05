using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Collections;
using Prana.Interfaces;
using Prana.CommonDataCache;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects.AppConstants;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.MiscUtilities;

namespace Prana.AutomationHandlers
{
    class TaxLotHandler:IAutomationDataHandler
    {
        const string DATEFORMAT = "MM/dd/yyyy";
        const string VALID = "Validated";

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
            set { _secMasterServices = value; }

        }
        private IAllocationServices _allocationServices;
        IRiskServices _riskServices = null;
        public IRiskServices RiskServices
        {
            set
            {
                _riskServices = value;

            }

        }
        public IAllocationServices AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
        }
       
        public void ProcessData(ClientSettings clientSetting, IList data)
        {
            List<PositionMaster> positionList = ValidateFromSecMaster(clientSetting, data);
            foreach (PositionMaster taxlot in positionList)
            {

                //if (taxlot != null)
                //{
                //    if (taxlot.FundID != int.MinValue && taxlot.FundID != 0)
                //    {
                //        string fundName = ClientFunds.GetFundName(clientSetting.ClientID, taxlot.FundID);
                //        if (fundName != null && fundName != string.Empty)
                //        {
                //            taxlot.FundName = fundName;
                //        }
                //        else
                //        {
                //            //throw new Exception("Fund Setting not set correctly in xslt , Please Check");
                //        }
                //    }

                //}

                if (taxlot != null)
                {
                    if (taxlot.FundID != 0 && taxlot.FundID !=int.MinValue)
                    {
                        string fundName = ClientFunds.GetFundName(clientSetting.ClientID, taxlot.FundID);
                        if (fundName != null && fundName != string.Empty)
                        {
                            taxlot.FundName = fundName;
                        }
                    }
                    else if (taxlot.FundName != "")
                    {
                        int fundID = ClientFunds.GetFundID(clientSetting.ClientID, taxlot.FundName);
                        if (fundID != 0 || fundID != int.MinValue)
                        {
                            taxlot.FundID = fundID;
                        }

                    }
                    else
                    {
                        throw new Exception("Fund Setting not set correctly in xslt , Please Check");
                    }
                }
            }
            List<PositionMaster> validatedpositionList = new List<PositionMaster>();
            List<string> inValidateSymbolList = new List<string>();
            //check is there any invalid symbol
            foreach (PositionMaster position in positionList)
            {
                if (position.Validated.Equals(VALID))
                {
                    validatedpositionList.Add(position);
                }
                else
                {
                    if (!inValidateSymbolList.Contains(position.Symbol))
                    inValidateSymbolList.Add(position.Symbol);
                }
            }

            if (inValidateSymbolList.Count > 0)
            {
                throw new Exception("Symbols not validated -- " + GeneralUtilities.GetStringFromList(inValidateSymbolList, ','));
            }
            _allocationServices.SavePositions(validatedpositionList, clientSetting.DataBaseSettings.ClientDB);
        }
        public List<PositionMaster> ValidateFromSecMaster(ClientSettings clientSetting, IList data)
        {
            List<PositionMaster> positionList = new List<PositionMaster>();           
            try
            {
                foreach (Object positiondata in data)
                {
                    positionList.Add((PositionMaster)positiondata);
                }
                // validate data by Security Master
                GetSMDataForTaxlotImport(positionList, clientSetting);               
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

            return positionList;
        }

        public IList RetrieveData(ClientSettings clientSetting)
        {
            //clientSetting.RiskPreferences.GroupingCriteria
            List<TaxLot> taxlots = _positionServices.GetOpenPositionsFromAnyDB(DateTime.Now, clientSetting.InputDataLocationPath, null);
            return taxlots;
        }

        #region Private
        private  Dictionary<int, Dictionary<string, List<PositionMaster>>> CreatePositionDictionary(List<PositionMaster> listOfData,ClientSettings clientSetting)
        {

            Dictionary<int, Dictionary<string, List<PositionMaster>>> _positionMasterSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<PositionMaster>>>();
            try
            {
                

                
                foreach (PositionMaster positionMaster in listOfData)
                {
                    
                    positionMaster.AUECID = 0;
                    positionMaster.UnderlyingID = 0;
                    positionMaster.ExchangeID = 0;
                   // positionMaster.FundName = string.Empty;
                    positionMaster.UserID = clientSetting.ClientInformation.UserId ;
                    positionMaster.CompanyID = clientSetting.ClientInformation.CompanyID;
                    positionMaster.TradingAccountID = clientSetting.ClientInformation.TradingAccountID;
                    positionMaster.PranaMsgType = OrderFields.PranaMsgTypes.ImportPosition;
                    positionMaster.ExternalOrderID = OrderIDGenerator.GenerateExternalOrderID();

                    
                    if (!String.IsNullOrEmpty(positionMaster.SideTagValue))
                    {
                        positionMaster.Side = TagDatabaseManager.GetInstance.GetOrderSideText(positionMaster.SideTagValue);
                    }

                    
                    if (!String.IsNullOrEmpty(positionMaster.FundName))
                    {
                        positionMaster.FundID = CachedDataManager.GetInstance.GetFundID(positionMaster.FundName.Trim());
                    }
                    if (!String.IsNullOrEmpty(positionMaster.Strategy))
                    {
                        positionMaster.StrategyID = CachedDataManager.GetInstance.GetStrategyID(positionMaster.Strategy.Trim());
                    }
                    if (positionMaster.CounterPartyID > 0)
                    {
                        positionMaster.ExecutingBroker = CachedDataManager.GetInstance.GetCounterPartyText(positionMaster.CounterPartyID);
                    }

                    if (!String.IsNullOrEmpty(positionMaster.Symbol))
                    {
                        string upperSymbol = positionMaster.Symbol.Trim().ToUpper();
                        if (_positionMasterSymbologyWiseDict.ContainsKey(0))
                        {
                           
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[0];
                            if (positionMasterSameSymbologyDict.ContainsKey(upperSymbol))
                            {
                                List<PositionMaster> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[upperSymbol];
                                positionMasterSymbolWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[upperSymbol] = positionMasterSymbolWiseList;
                                _positionMasterSymbologyWiseDict[0] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[0].Add(upperSymbol, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbolDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameSymbolDict.Add(upperSymbol, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(0, positionMasterSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.RIC))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[1];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.RIC))
                            {
                                List<PositionMaster> positionMasterRICWiseList = positionMasterSameSymbologyDict[positionMaster.RIC];
                                positionMasterRICWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.RIC] = positionMasterRICWiseList;
                                _positionMasterSymbologyWiseDict[1] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[1].Add(positionMaster.RIC, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameRICDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameRICDict.Add(positionMaster.RIC, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(1, positionMasterSameRICDict);
                        }

                    }
                    else if (!String.IsNullOrEmpty(positionMaster.ISIN))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[2];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.ISIN))
                            {
                                List<PositionMaster> positionMasterISINWiseList = positionMasterSameSymbologyDict[positionMaster.ISIN];
                                positionMasterISINWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.ISIN] = positionMasterISINWiseList;
                                _positionMasterSymbologyWiseDict[2] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[2].Add(positionMaster.ISIN, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameISINDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameISINDict.Add(positionMaster.ISIN, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(2, positionMasterSameISINDict);
                        }

                    }
                    else if (!String.IsNullOrEmpty(positionMaster.SEDOL))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[3];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.SEDOL))
                            {
                                List<PositionMaster> positionMasterSEDOLWiseList = positionMasterSameSymbologyDict[positionMaster.SEDOL];
                                positionMasterSEDOLWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.SEDOL] = positionMasterSEDOLWiseList;
                                _positionMasterSymbologyWiseDict[3] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[3].Add(positionMaster.SEDOL, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameSEDOLDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameSEDOLDict.Add(positionMaster.SEDOL, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(3, positionMasterSameSEDOLDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.CUSIP))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[4];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.CUSIP))
                            {
                                List<PositionMaster> positionMasterCUSIPWiseList = positionMasterSameSymbologyDict[positionMaster.CUSIP];
                                positionMasterCUSIPWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.CUSIP] = positionMasterCUSIPWiseList;
                                _positionMasterSymbologyWiseDict[4] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[4].Add(positionMaster.CUSIP, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameCUSIPDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameCUSIPDict.Add(positionMaster.CUSIP, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(4, positionMasterSameCUSIPDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.Bloomberg))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[5];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.Bloomberg))
                            {
                                List<PositionMaster> positionMasterBloombergWiseList = positionMasterSameSymbologyDict[positionMaster.Bloomberg];
                                positionMasterBloombergWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.Bloomberg] = positionMasterBloombergWiseList;
                                _positionMasterSymbologyWiseDict[5] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[5].Add(positionMaster.Bloomberg, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameBloombergDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameBloombergDict.Add(positionMaster.Bloomberg, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(5, positionMasterSameBloombergDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.OSIOptionSymbol))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[6];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.OSIOptionSymbol))
                            {
                                List<PositionMaster> positionMasterOSIWiseList = positionMasterSameSymbologyDict[positionMaster.OSIOptionSymbol];
                                positionMasterOSIWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.OSIOptionSymbol] = positionMasterOSIWiseList;
                                _positionMasterSymbologyWiseDict[6] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[6].Add(positionMaster.OSIOptionSymbol, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameOSIDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameOSIDict.Add(positionMaster.OSIOptionSymbol, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(6, positionMasterSameOSIDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.IDCOOptionSymbol))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[7];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.IDCOOptionSymbol))
                            {
                                List<PositionMaster> positionMasterIDCOWiseList = positionMasterSameSymbologyDict[positionMaster.IDCOOptionSymbol];
                                positionMasterIDCOWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.IDCOOptionSymbol] = positionMasterIDCOWiseList;
                                _positionMasterSymbologyWiseDict[7] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[7].Add(positionMaster.IDCOOptionSymbol, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameIDCODict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameIDCODict.Add(positionMaster.IDCOOptionSymbol, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(7, positionMasterSameIDCODict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.OpraOptionSymbol))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(8))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[8];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.OpraOptionSymbol))
                            {
                                List<PositionMaster> positionMasterOpraWiseList = positionMasterSameSymbologyDict[positionMaster.OpraOptionSymbol];
                                positionMasterOpraWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.OpraOptionSymbol] = positionMasterOpraWiseList;
                                _positionMasterSymbologyWiseDict[8] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[8].Add(positionMaster.OpraOptionSymbol, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameOpraDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameOpraDict.Add(positionMaster.OpraOptionSymbol, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(7, positionMasterSameOpraDict);
                        }
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
            return _positionMasterSymbologyWiseDict;
        }

        private  void GetSMDataForTaxlotImport(List<PositionMaster> positionList,ClientSettings clientSetting)
        {
            try
            {

                Dictionary<int, Dictionary<string, List<PositionMaster>>> _positionMasterSymbologyWiseDict = CreatePositionDictionary(positionList, clientSetting);
               
                if (_positionMasterSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<PositionMaster>>> kvp in _positionMasterSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<PositionMaster>> symbolDict = _positionMasterSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<PositionMaster>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                                secMasterRequestObj.AddNewRow();
                            }
                        }
                    }

                    if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                    {
                        secMasterRequestObj.HashCode = this.GetHashCode();
                        
                        List<SecMasterBaseObj> secMasterCollection = _secMasterServices.GetSecMasterDataForListSync(secMasterRequestObj, secMasterRequestObj.HashCode);

                        if (secMasterCollection != null && secMasterCollection.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                            {
                                string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                                string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                                string cUSIPSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                                string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                                string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                                string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                                string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                                string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                                string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();


                                int requestedSymbologyID = secMasterObj.RequestedSymbology;

                                if (_positionMasterSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                                {
                                    Dictionary<string, List<PositionMaster>> dictSymbols = _positionMasterSymbologyWiseDict[requestedSymbologyID];
                                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                    {
                                        List<PositionMaster> listPosMaster = dictSymbols[secMasterObj.RequestedSymbol];
                                        foreach (PositionMaster positionMaster in listPosMaster)
                                        {
                                            positionMaster.Symbol = pranaSymbol;
                                            positionMaster.CUSIP = cUSIPSymbol;
                                            positionMaster.ISIN = isinSymbol;
                                            positionMaster.SEDOL = sedolSymbol;
                                            positionMaster.Bloomberg = bloombergSymbol;
                                            positionMaster.RIC = reutersSymbol;
                                            positionMaster.OSIOptionSymbol = osiOptionSymbol;
                                            positionMaster.IDCOOptionSymbol = idcoOptionSymbol;
                                            positionMaster.OpraOptionSymbol = opraOptionSymbol;
                                            positionMaster.UnderlyingSymbol = secMasterObj.UnderLyingSymbol;
                                            UpdatePositionMasterObj(positionMaster, secMasterObj);

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
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private  PositionMaster UpdatePositionMasterObj(PositionMaster positionMaster, SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (positionMaster.AUECID == int.MinValue || positionMaster.AUECID == 0)
                {
                    positionMaster.AUECID = secMasterObj.AUECID;
                }

                positionMaster.AssetID = secMasterObj.AssetID;
                positionMaster.AssetType = (AssetCategory)secMasterObj.AssetID;

                if (positionMaster.UnderlyingID == int.MinValue || positionMaster.UnderlyingID == 0)
                {
                    positionMaster.UnderlyingID = secMasterObj.UnderLyingID;
                }
                if (positionMaster.ExchangeID == int.MinValue || positionMaster.ExchangeID == 0)
                {
                    positionMaster.ExchangeID = secMasterObj.ExchangeID;
                }
                // TODO : Currency - pickup from securitymaster
                if (positionMaster.CurrencyID == int.MinValue || positionMaster.CurrencyID == 0)
                {
                    positionMaster.CurrencyID = secMasterObj.CurrencyID;
                }

                if (!string.IsNullOrEmpty(positionMaster.PositionStartDate))
                {
                    string[] splitDateFieldSlash = positionMaster.PositionStartDate.Split('/');
                    if (splitDateFieldSlash.Length == 1)
                    {
                        string[] splitDateFieldWithDash = positionMaster.PositionStartDate.Split('-');
                        if (splitDateFieldWithDash.Length == 1)
                        {
                            bool blnIsTrue;
                            double result;
                            blnIsTrue = double.TryParse(positionMaster.PositionStartDate, out result);
                            if (blnIsTrue)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.PositionStartDate));//.ParseExact(positionMaster.PositionStartDate, "yyyyMMdd", null);
                                positionMaster.PositionStartDate = dtn.ToShortDateString();

                                UpdatePositionMasterAUECandSettlementDate(positionMaster);
                            }
                        }
                        else
                        {
                            UpdatePositionMasterAUECandSettlementDate(positionMaster);
                        }
                    }
                    else
                    {
                        UpdatePositionMasterAUECandSettlementDate(positionMaster);
                    }
                }
                double result1;
                bool blnIsTrue1 = double.TryParse(positionMaster.PositionStartDate, out result1);
                if (blnIsTrue1)
                {
                    DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.PositionStartDate));//.ParseExact(positionMaster.PositionStartDate, "yyyyMMdd", null);
                    positionMaster.PositionStartDate = dtn.ToShortDateString();


                }
                int auecSettlementPeriod = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAUECSettlementPeriod(positionMaster.AUECID, positionMaster.SideTagValue);
                DateTime positionSettlementDate = DateTimeConstants.MinValue;
                if (auecSettlementPeriod == 0)
                {
                    positionSettlementDate = Convert.ToDateTime(positionMaster.PositionStartDate);
                }
                else
                {
                    positionSettlementDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(Convert.ToDateTime(positionMaster.PositionStartDate), auecSettlementPeriod, positionMaster.AUECID); ;
                }
                positionMaster.PositionSettlementDate = positionSettlementDate.ToShortDateString();
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

            return positionMaster;
        }
        private  void UpdatePositionMasterAUECandSettlementDate(PositionMaster positionMaster)
        {
            //DateTime dt = TimeZoneHelper.GetAUECLocalDateFromUTC(positionMaster.AUECID, Convert.ToDateTime(positionMaster.PositionStartDate));
            DateTime dt = Convert.ToDateTime(positionMaster.PositionStartDate);
            positionMaster.AUECLocalDate = dt.ToString(DATEFORMAT);
            int auecSettlementPeriod = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAUECSettlementPeriod(positionMaster.AUECID, positionMaster.SideTagValue);
            DateTime positionSettlementDate = DateTimeConstants.MinValue;
            if (auecSettlementPeriod == 0)
            {
                positionSettlementDate = Convert.ToDateTime(positionMaster.PositionStartDate);
            }
            else
            {
                positionSettlementDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(Convert.ToDateTime(positionMaster.PositionStartDate), auecSettlementPeriod, positionMaster.AUECID); ;
            }
            positionMaster.PositionSettlementDate = positionSettlementDate.ToShortDateString();
        }
        #endregion
    }
}
