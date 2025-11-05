using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Collections;
using Prana.Interfaces;
using Prana.Utilities.MiscUtilities;
using Prana.CommonDataCache;

namespace Prana.AutomationHandlers
{
    class RiskHandler:IAutomationDataHandler
    {
        private IPranaPositionServices _positionServices;
        private ISecMasterServices _secMasterServices;
        public ISecMasterServices SecMasterServices
        {
            set { _secMasterServices = value; }

        }
        public IPranaPositionServices PositionServices
        {
            set
            {
                _positionServices = value;
            }
        }
        private string getUniqueID()
        {
            Int64 uniqueID = Int64.Parse(DateTime.Now.ToString("yyMMddHHmmss"));
            return uniqueID.ToString();
        }
        private IAllocationServices _allocationServices;

        public IAllocationServices AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
        }
        IRiskServices _riskServices = null;
        public IRiskServices RiskServices
        {
            set
            {
                _riskServices = value;

            }

        }
        public void ProcessData(ClientSettings clientSetting, IList data)
        {
            
                // risk processing
                //call to risk
                TaxlotGroupingManager taxlotGrpManager = new TaxlotGroupingManager();
                ClientRiskPref clientRiskPref = clientSetting.RiskPreferences;
                string[] filenamedata=clientSetting.ReportFileName.Split('.');
                if (filenamedata.Length > 1)
                {
                    string filename = filenamedata[0];
                    string extension = filenamedata[1];
                    clientSetting.ReportFileName = filename + clientSetting.ClientSettingName + "." + extension;
                }
                List<PositionMaster> positionList = new List<PositionMaster>();


                #region Get Data
                List<TaxLot> taxlots = new List<TaxLot>();

                List<string> groupCreterias = Prana.Utilities.MiscUtilities.GeneralUtilities.GetListFromString(clientRiskPref.GroupingCriteria, ',');

                if (clientSetting.InputDataLocationType == AutomationEnum.InputOutputType.FileSystem)
                {
                    TaxLotHandler taxlotHandler = new TaxLotHandler();
                    taxlotHandler.AllocationServices = _allocationServices;
                    taxlotHandler.PositionServices = _positionServices;
                    taxlotHandler.SecMasterServices = _secMasterServices;


                    positionList = taxlotHandler.ValidateFromSecMaster(clientSetting, data);

                    taxlots = _allocationServices.CreateTaxlotsFromPositions(positionList);







                }
                else
                {
                    foreach (Object positiondata in data)
                    {
                        TaxLot taxlot = positiondata as TaxLot;
                        taxlots.Add(taxlot);
                    }
                }

                if (taxlots.Count == 0)
                {
                    throw new Exception("NO data receveied for processing so no file will be generated");
                }

                foreach (TaxLot taxlot in taxlots)
                {
                    if (taxlot != null)
                    {
                        string fundName = ClientFunds.GetFundName(clientSetting.ClientID, taxlot.Level1ID);
                        if (fundName != null && fundName != string.Empty)
                        {
                            taxlot.Level1Name = fundName;
                        }
                        taxlot.AssetName = CommonDataCache.CachedDataManager.GetInstance.GetAssetText(taxlot.AssetID);
                        taxlot.UnderlyingName = CommonDataCache.CachedDataManager.GetInstance.GetUnderLyingText(taxlot.UnderlyingID);
                    }
                }
                taxlots = taxlotGrpManager.CreateGroupedData(groupCreterias, taxlots);

                #endregion

                // if report type is stress test
                if (clientSetting.ReportType == Prana.BusinessObjects.AutomationEnum.ReprotTypeEnum.StressTest)
                {
                    if (clientRiskPref.ViewCriteria == "")
                    {
                        PranaRiskObjColl riskObjColl = new PranaRiskObjColl();

                        PranaRequestCarrier riskCarrier = CreateRequestCarrier(taxlots, clientRiskPref, ref riskObjColl);
                        double percentageChangeInBenchMark = clientRiskPref.PercentageBenchMarkMove;


                        riskCarrier.ForGrouped = true;
                        //TODO Uncomment
                        PranaRequestCarrier updatedriskCarrier = new PranaRequestCarrier();
                        updatedriskCarrier = _riskServices.CalculateCorrelationAndBetaForGroup(riskCarrier);

                        double percentageChangesInPortfolio = updatedriskCarrier.Beta * percentageChangeInBenchMark;
                        List<RiskStressTest> stressTestList = new List<RiskStressTest>();
                        RiskStressTest riskStressTest = new RiskStressTest();
                        
                        riskStressTest.Beta = updatedriskCarrier.Beta;
                        riskStressTest.Correlation = updatedriskCarrier.Correlation;
                        riskStressTest.OldPortfolioValue = updatedriskCarrier.PortFolioValue;
                        riskStressTest.OldBenchmarkValue = updatedriskCarrier.BenchMarkValue;
                        riskStressTest.PercentageChangeinPortfolioValue = percentageChangesInPortfolio;
                        riskStressTest.NewPortfolioValue = updatedriskCarrier.PortFolioValue * (1 + (percentageChangesInPortfolio / 100));
                        riskStressTest.NewBenchmarkValue = updatedriskCarrier.BenchMarkValue * (1 + (percentageChangeInBenchMark / 100.0));
                        stressTestList.Add(riskStressTest);
                        FileFormatterFactory.GetInstance().GetFormatterClass(clientSetting.FileFormatter).CreateFile(riskObjColl, stressTestList, clientSetting.ReportFileName, clientSetting.DicColumns);
                    }
                    else
                    {
                        
                        List<RiskStressTest> stressTestList = new List<RiskStressTest>();
                        Dictionary<string, List<TaxLot>> _dictGroup = taxlotGrpManager.CreateGroups(clientRiskPref.ViewCriteria, taxlots);
                        foreach (KeyValuePair<string, List<TaxLot>> taxxlotstobeconverted in _dictGroup)
                        {
                            try
                            {
                                PranaRiskObjColl riskObjColl = new PranaRiskObjColl();
                                PranaRequestCarrier riskCarrier = CreateRequestCarrier(taxxlotstobeconverted.Value, clientRiskPref, ref riskObjColl);
                                double percentageChangeInBenchMark = clientRiskPref.PercentageBenchMarkMove;


                                riskCarrier.ForGrouped = true;
                                //TODO Uncomment
                                PranaRequestCarrier updatedriskCarrier = new PranaRequestCarrier();
                                updatedriskCarrier = _riskServices.CalculateCorrelationAndBetaForGroup(riskCarrier);
                                double percentageChangesInPortfolio = updatedriskCarrier.Beta * percentageChangeInBenchMark;
                                RiskStressTest riskStressTest = new RiskStressTest();
                                riskStressTest.GroupingName = taxxlotstobeconverted.Key;
                                riskStressTest.Beta = updatedriskCarrier.Beta;
                                riskStressTest.Correlation = updatedriskCarrier.Correlation;
                                riskStressTest.OldPortfolioValue = updatedriskCarrier.PortFolioValue;
                                riskStressTest.OldBenchmarkValue = updatedriskCarrier.BenchMarkValue;
                                riskStressTest.PercentageChangeinPortfolioValue = percentageChangesInPortfolio;
                                riskStressTest.NewPortfolioValue = updatedriskCarrier.PortFolioValue * (1 + (percentageChangesInPortfolio / 100));
                                riskStressTest.NewBenchmarkValue = updatedriskCarrier.BenchMarkValue * (1 + (percentageChangeInBenchMark / 100.0));
                                stressTestList.Add(riskStressTest);

                            }
                            catch (Exception ex)
                            {
                                throw ex;

                            }
                        }

                        FileFormatterFactory.GetInstance().GetFormatterClass(clientSetting.FileFormatter).CreateFile(stressTestList, null, clientSetting.ReportFileName, null);
                    }

                    

                }
                else
                {

                    if (clientRiskPref.ViewCriteria == "")
                    {
                        PranaRiskObjColl riskObjColl = new PranaRiskObjColl();
                        List<RiskGrouping> summaryList = new List<RiskGrouping>();
                        PranaRequestCarrier riskCarrier = CreateRequestCarrier(taxlots, clientRiskPref, ref riskObjColl);
                        try
                        {
                            //TODO Uncomment 
                            PranaRequestCarrier updatedRiskCarrier = _riskServices.CalculateRisk(riskCarrier);
                             PranaReportingServicesHelpter.GetInstance().UpdateRiskObjFromPranaRequestCarrier(updatedRiskCarrier, riskObjColl);

                            RiskGrouping riskGrouping = new RiskGrouping();
                            riskGrouping.GroupName = "Portfolio";
                            riskGrouping.Risk = updatedRiskCarrier.PortfolioRisk;
                            riskGrouping.Correlation = updatedRiskCarrier.Correlation;
                            summaryList.Add(riskGrouping);
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }

                        FileFormatterFactory.GetInstance().GetFormatterClass(clientSetting.FileFormatter).CreateFile(riskObjColl, summaryList, clientSetting.ReportFileName, clientSetting.DicColumns);

                    }
                    else
                    {
                        List<RiskGrouping> listOfGroupedReq = new List<RiskGrouping>();
                        Dictionary<string, List<TaxLot>> _dictGroup = taxlotGrpManager.CreateGroups(clientRiskPref.ViewCriteria, taxlots);
                        foreach (KeyValuePair<string, List<TaxLot>> taxxlotstobeconverted in _dictGroup)
                        {


                            RiskGrouping riskGrouping = new RiskGrouping();
                            riskGrouping.GroupName = taxxlotstobeconverted.Key;
                            listOfGroupedReq.Add(riskGrouping);
                            try
                            {
                                PranaRiskObjColl coll = new PranaRiskObjColl();
                                PranaRequestCarrier riskCarrier = CreateRequestCarrier(taxxlotstobeconverted.Value, clientRiskPref, ref coll);
                                // TODO Uncomment
                                PranaRequestCarrier updatedRiskCarrier = _riskServices.CalculateRiskAndCorrelationForGroup(riskCarrier);
                                riskGrouping.Risk = updatedRiskCarrier.PortfolioRisk;
                                riskGrouping.Correlation = updatedRiskCarrier.Correlation;
                            }
                            catch (Exception ex)
                            {
                                 throw ex;

                            }
                            

                        }
                        if (clientSetting.RiskPreferences.ViewCriteria == "")
                        {
                            FileFormatterFactory.GetInstance().GetFormatterClass(clientSetting.FileFormatter).CreateFile(listOfGroupedReq, null, clientSetting.ReportFileName, clientSetting.DicColumns);
                        }
                        else
                        {
                            FileFormatterFactory.GetInstance().GetFormatterClass(clientSetting.FileFormatter).CreateFile(listOfGroupedReq, null, clientSetting.ReportFileName, null);
                        }
                    }
                }
                //FileFormatterFactory.GetInstance().GetFormatterClass(clientSetting.FileFormatter).CreateFile(riskObjColl, clientSetting.ReportFileName, null);


           
            
            

           
           
           
        }
        private PranaRequestCarrier CreateRequestCarrier(List<TaxLot> taxlots, ClientRiskPref clientRiskPref,ref PranaRiskObjColl riskObjColl)
        {
            List<PSSymbolRequestObject> PSReqObjectList = new List<PSSymbolRequestObject>();
            foreach (TaxLot taxlot in taxlots)
            {
                PSReqObjectList.Add(new PSSymbolRequestObject(taxlot));
            }

            Dictionary<string, string> pssymbolresponse = _positionServices.GetAllPSSymbols(PSReqObjectList);

            riskObjColl = PranaReportingServicesHelpter.GetInstance().ConvertRiskObjFromTaxlot(taxlots);
            foreach (PranaRiskObj riskObj in riskObjColl)
            {
                if (pssymbolresponse.ContainsKey(riskObj.Symbol))
                {
                    riskObj.PSSymbol = pssymbolresponse[riskObj.Symbol];
                }
            }
            PranaRequestCarrier riskCarrier = new PranaRequestCarrier(riskObjColl, clientRiskPref.StartDate, clientRiskPref.EndDate, clientRiskPref.BenchMarkSymbol, "1");
            return riskCarrier;
        }
      

        public IList RetrieveData(ClientSettings clientSetting)
        {
            return null;
        }
    }
}
