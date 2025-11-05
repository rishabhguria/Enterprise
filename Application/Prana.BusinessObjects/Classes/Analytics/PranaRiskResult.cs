using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class PranaRiskResult
    {
        public PranaRiskResult(PranaRiskObj riskObj, RiskConstants.RiskCalculationBasedOn riskCalculationBasedOn, bool isBetaRequest)
        {
            if (isBetaRequest)
            {
                _symbol = riskObj.UnderlyingSymbol;
                _underlyingSymbol = riskObj.UnderlyingSymbol;
                _quantity = riskObj.DeltaAdjPosition;
                _psSymbol = riskObj.UnderlyingPSSymbol;
                _pxSelectedFeed = riskObj.UnderlyingLastPrice;
                _underlyingPrice = riskObj.UnderlyingLastPrice;
            }
            else
            {
                _symbol = riskObj.Symbol;
                _underlyingSymbol = riskObj.UnderlyingSymbol;
                if (riskCalculationBasedOn == RiskConstants.RiskCalculationBasedOn.DeltaAdjPosition)
                {
                    _quantity = riskObj.DeltaAdjPosition;
                }
                else
                {
                    _quantity = riskObj.Quantity;
                }
                _psSymbol = riskObj.PSSymbol;
                _pxSelectedFeed = riskObj.LastPrice;
                _underlyingPrice = riskObj.UnderlyingLastPrice;
            }
        }

        public PranaRiskResult()
        {
        }

        public void Add(PranaRiskObj riskObj, RiskConstants.RiskCalculationBasedOn riskCalculationBasedOn, bool isBetaRequest)
        {
            if (isBetaRequest || riskCalculationBasedOn == RiskConstants.RiskCalculationBasedOn.DeltaAdjPosition)
            {
                _quantity += riskObj.DeltaAdjPosition;
            }
            else
            {
                _quantity += riskObj.Quantity;
            }
        }

        private double _risk;
        public double Risk
        {
            get { return _risk; }
            set { _risk = value; }
        }

        private double _diffrisk;
        public double DifferentialRisk
        {
            get { return _diffrisk; }
            set { _diffrisk = value; }
        }

        private double _marginalrisk;
        public double MarginalRisk
        {
            get { return _marginalrisk; }
            set { _marginalrisk = value; }
        }

        private double _componentRisk;
        public double ComponentRisk
        {
            get { return _componentRisk; }
            set { _componentRisk = value; }
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

        private double _psVolatility;
        public double PSVolatility
        {
            get { return _psVolatility; }
            set { _psVolatility = value; }
        }

        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _underlyingSymbol;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        private string _psSymbol;
        public string PSSymbol
        {
            get { return _psSymbol; }
            set { _psSymbol = value; }
        }

        private double _quantity;
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private double _pxSelectedFeed;
        public double PxSelectedFeed
        {
            get { return _pxSelectedFeed; }
            set { _pxSelectedFeed = value; }
        }

        private double _underlyingPrice;
        public double UnderlyingPrice
        {
            get { return _underlyingPrice; }
            set { _underlyingPrice = value; }
        }

        private double _beta;
        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        private double _pnlImpact;
        public double PNLImpact
        {
            get { return _pnlImpact; }
            set { _pnlImpact = value; }
        }
    }
}
