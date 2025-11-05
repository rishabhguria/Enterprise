using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class PranaRequestCarrier
    {
        private void Initialize(DateTime startDate, DateTime endDate, string benchMarkSymbols, string benchMarkQty, double indexStress, RiskParamameter riskParameter)
        {
            _startDate = startDate;
            _endDate = endDate;
            _benchMarkSymbols = benchMarkSymbols;
            _benchMarkQty = benchMarkQty;
            _indexStress = indexStress;
            _riskParams = riskParameter;
        }

        public PranaRequestCarrier()
        {
        }

        public PranaRequestCarrier(PranaRiskObjColl riskObjcollection, DateTime startDate, DateTime endDate, string benchMarkSymbols, string benchMarkQty, double benchMarkMove, RiskConstants.RiskCalculationBasedOn riskCalculationBasedOn, bool isBetaRequest, RiskParamameter riskParameter)
        {
            CreateRequest(riskObjcollection, riskCalculationBasedOn, isBetaRequest);
            Initialize(startDate, endDate, benchMarkSymbols, benchMarkQty, benchMarkMove, riskParameter);
        }

        public void CreateRequest(PranaRiskObjColl riskObjcollection, RiskConstants.RiskCalculationBasedOn riskCalculationBasedOn, bool isBetaRequest)
        {
            if (isBetaRequest)
            {
                foreach (PranaRiskObj riskObj in riskObjcollection)
                {
                    if (_individualRiskRequest.ContainsKey(riskObj.UnderlyingSymbol))
                    {
                        _individualRiskRequest[riskObj.UnderlyingSymbol].Add(riskObj, riskCalculationBasedOn, isBetaRequest);
                    }
                    else
                    {
                        PranaRiskResult riskResult = new PranaRiskResult(riskObj, riskCalculationBasedOn, isBetaRequest);
                        _individualRiskRequest.Add(riskObj.UnderlyingSymbol, riskResult);
                    }

                    if (_groupSymbolList.ContainsKey(riskObj.UnderlyingSymbol))
                    {
                        _groupSymbolList[riskObj.UnderlyingSymbol].Add(riskObj, riskCalculationBasedOn, isBetaRequest);
                    }
                    else
                    {
                        PranaRiskResult riskResultGroup = new PranaRiskResult(riskObj, riskCalculationBasedOn, isBetaRequest);
                        _groupSymbolList.Add(riskObj.UnderlyingSymbol, riskResultGroup);
                    }
                }
            }
            else
            {
                foreach (PranaRiskObj riskObj in riskObjcollection)
                {
                    string keySymbolPosition = riskObj.Symbol + riskObj.PositionType;

                    if (_individualRiskRequest.ContainsKey(keySymbolPosition))
                    {
                        _individualRiskRequest[keySymbolPosition].Add(riskObj, riskCalculationBasedOn, isBetaRequest);
                    }
                    else
                    {
                        PranaRiskResult riskResult = new PranaRiskResult(riskObj, riskCalculationBasedOn, isBetaRequest);
                        _individualRiskRequest.Add(keySymbolPosition, riskResult);
                    }

                    if (_groupSymbolList.ContainsKey(riskObj.Symbol))
                    {
                        _groupSymbolList[riskObj.Symbol].Add(riskObj, riskCalculationBasedOn, isBetaRequest);
                    }
                    else
                    {
                        PranaRiskResult riskResultGroup = new PranaRiskResult(riskObj, riskCalculationBasedOn, isBetaRequest);
                        _groupSymbolList.Add(riskObj.Symbol, riskResultGroup);
                    }
                }
            }
        }

        private Dictionary<string, PranaRiskResult> _individualRiskRequest = new Dictionary<string, PranaRiskResult>();
        public Dictionary<string, PranaRiskResult> IndividualSymbolList
        {
            get { return _individualRiskRequest; }
            set { _individualRiskRequest = value; }
        }

        private Dictionary<string, PranaRiskResult> _groupSymbolList = new Dictionary<string, PranaRiskResult>();
        public Dictionary<string, PranaRiskResult> GroupSymbolList
        {
            get { return _groupSymbolList; }
            set { _groupSymbolList = value; }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        private string _benchMarkSymbols;
        public string BenchMarkSymbols
        {
            get { return _benchMarkSymbols; }
            set { _benchMarkSymbols = value; }
        }

        private string _benchMarkQty;
        public string BenchMarkQty
        {
            get { return _benchMarkQty; }
            set { _benchMarkQty = value; }
        }

        private double _indexStress;
        public double IndexStress
        {
            get { return _indexStress; }
            set { _indexStress = value; }
        }

        private double _portfoliovalue;
        public double PortFolioValue
        {
            get { return _portfoliovalue; }
            set { _portfoliovalue = value; }
        }

        private double _benchMarkValue;
        public double BenchMarkValue
        {
            get { return _benchMarkValue; }
            set { _benchMarkValue = value; }
        }

        private double _diffrisk;
        public double DifferentialRisk
        {
            get { return _diffrisk; }
            set { _diffrisk = value; }
        }

        private double _stdDev;
        public double StandardDeviation
        {
            get { return _stdDev; }
            set { _stdDev = value; }
        }

        private double _correlation;
        public double Correlation
        {
            get { return _correlation; }
            set { _correlation = value; }
        }

        private double _beta;
        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        private string _groupingName = string.Empty;
        public string GroupingName
        {
            get { return _groupingName; }
            set { _groupingName = value; }
        }

        private int _volType = 3;
        public int VolatilityType
        {
            get { return _volType; }
            set { _volType = value; }
        }

        private double _portfolioRisk;
        public double PortfolioRisk
        {
            get { return _portfolioRisk; }
            set { _portfolioRisk = value; }
        }

        private double _pnlImpact;
        public double PNLImpact
        {
            get { return _pnlImpact; }
            set { _pnlImpact = value; }
        }

        private string msgType;
        public string MsgType
        {
            get { return msgType; }
            set { msgType = value; }
        }

        private bool _forNew = false;
        public bool ForNew
        {
            get { return _forNew; }
            set { _forNew = value; }
        }

        private RiskParamameter _riskParams = new RiskParamameter();
        public RiskParamameter RiskParam
        {
            get { return _riskParams; }
            set { _riskParams = value; }
        }

        public PranaRequestCarrier Clone()
        {
            PranaRequestCarrier pranaRequestCarrier = new PranaRequestCarrier();
            pranaRequestCarrier.StartDate = _startDate;
            pranaRequestCarrier.EndDate = _endDate;
            pranaRequestCarrier.BenchMarkQty = _benchMarkQty;
            pranaRequestCarrier.BenchMarkSymbols = _benchMarkSymbols;
            pranaRequestCarrier.MsgType = msgType;
            pranaRequestCarrier.GroupingName = _groupingName;
            pranaRequestCarrier.VolatilityType = _volType;
            pranaRequestCarrier.IndexStress = _indexStress;
            pranaRequestCarrier.ForNew = _forNew;

            return pranaRequestCarrier;
        }

        public PranaRequestCarrierContainer GetIndividualPranaRequestCarrierContainer()
        {
            PranaRequestCarrierContainer pranaRequestCarrierContainer = new PranaRequestCarrierContainer();

            foreach (KeyValuePair<string, PranaRiskResult> item in _individualRiskRequest)
            {
                pranaRequestCarrierContainer.PxSelectedFeeds.Add(item.Value.PxSelectedFeed.ToString());
                pranaRequestCarrierContainer.PranaSymbols.Add(item.Key);
                pranaRequestCarrierContainer.UnderlyingPrices.Add(item.Value.UnderlyingPrice.ToString());
                pranaRequestCarrierContainer.Quantities.Add(item.Value.Quantity.ToString());
                pranaRequestCarrierContainer.PSSymbols.Add(item.Value.PSSymbol);
            }
            return pranaRequestCarrierContainer;
        }

        public PranaRequestCarrierContainer GetGroupPranaRequestCarrierContainer()
        {
            PranaRequestCarrierContainer pranaRequestCarrierContainer = new PranaRequestCarrierContainer();

            foreach (KeyValuePair<string, PranaRiskResult> item in _groupSymbolList)
            {
                pranaRequestCarrierContainer.PxSelectedFeeds.Add(item.Value.PxSelectedFeed.ToString());
                pranaRequestCarrierContainer.PranaSymbols.Add(item.Key);
                pranaRequestCarrierContainer.UnderlyingPrices.Add(item.Value.UnderlyingPrice.ToString());
                pranaRequestCarrierContainer.Quantities.Add(item.Value.Quantity.ToString());
                pranaRequestCarrierContainer.PSSymbols.Add(item.Value.PSSymbol);
            }
            return pranaRequestCarrierContainer;
        }
    }

    public class PranaRequestCarrierContainer
    {
        public List<string> PxSelectedFeeds = new List<string>();
        public List<string> PranaSymbols = new List<string>();
        public List<string> UnderlyingPrices = new List<string>();
        public List<string> Quantities = new List<string>();
        public List<string> PSSymbols = new List<string>();
    }
}
