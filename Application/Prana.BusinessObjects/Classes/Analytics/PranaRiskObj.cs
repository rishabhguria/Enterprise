using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class PranaRiskObj : TaxLot
    {
        public PranaRiskObj()
        {
        }

        private string _pssymbol;
        public string PSSymbol
        {
            get { return _pssymbol; }
            set { _pssymbol = value; }
        }

        [Browsable(false)]
        private string _underlyingPSSymbol;
        public string UnderlyingPSSymbol
        {
            get { return _underlyingPSSymbol; }
            set { _underlyingPSSymbol = value; }
        }

        private double _risk;
        public double Risk
        {
            get { return _risk; }
            set { _risk = value; }
        }

        private int _sideMultiplier = 1;
        public override int SideMultiplier
        {
            get { return _sideMultiplier; }
            set { _sideMultiplier = value; }
        }

        private double _unitRisk;
        public double UnitStdDev
        {
            get { return _unitRisk; }
            set { _unitRisk = value; }
        }

        private double _diffrisk;
        public double DifferentialRisk
        {
            get { return _diffrisk; }
            set { _diffrisk = value; }
        }

        private double _marginalRisk;
        public double MarginalRisk
        {
            get { return _marginalRisk; }
            set { _marginalRisk = value; }
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

        private bool _isChecked = true;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        private double _lastPrice;
        public double LastPrice
        {
            get { return _lastPrice; }
            set { _lastPrice = value; }
        }

        private double _underlyingLastPrice;
        public double UnderlyingLastPrice
        {
            get { return _underlyingLastPrice; }
            set { _underlyingLastPrice = value; }
        }

        private double _deltaAdjPosition;
        public double DeltaAdjPosition
        {
            get { return _deltaAdjPosition; }
            set { _deltaAdjPosition = value; }
        }

        private double _volatility;
        public double Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }

        private double _currentValue;
        public double CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; }
        }

        private double _projectedValue;
        public double ProjectedValue
        {
            get { return _projectedValue; }
            set { _projectedValue = value; }
        }

        private double _beta;
        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        private double _percentChange;
        public double PercentChange
        {
            get { return _percentChange; }
            set { _percentChange = value; }
        }

        private string _pnlImpact = "0";
        public string PnlImpact
        {
            get { return _pnlImpact; }
            set { _pnlImpact = value; }
        }

        public void SetRiskData(PranaRiskResult riskResult, RiskConstants.RiskCalculationBasedOn riskCalculationBasedOn)
        {
            if (riskResult.Quantity != 0)
            {
                double absRiskQty = Math.Abs(riskResult.Quantity);
                double absQty = 0.0;

                if (riskCalculationBasedOn == RiskConstants.RiskCalculationBasedOn.Quantity)
                    absQty = Math.Abs(_quantity);
                else if (riskCalculationBasedOn == RiskConstants.RiskCalculationBasedOn.DeltaAdjPosition)
                    absQty = Math.Abs(_deltaAdjPosition);

                _stdDev = Math.Round((riskResult.StandardDeviation * absQty) / absRiskQty);
                _risk = Math.Round((riskResult.Risk * absQty) / absRiskQty);
                _marginalRisk = Math.Round((riskResult.MarginalRisk * absQty) / absRiskQty);
                _componentRisk = Math.Round((riskResult.ComponentRisk * absQty) / absRiskQty);
            }
            _diffrisk = riskResult.DifferentialRisk;
            _correlation = riskResult.Correlation;
        }

        public void SetStressTestData(PranaRiskResult riskResult, double percentChange, double currentValue, double projectedValue)
        {
            _beta = riskResult.Beta;
            _correlation = riskResult.Correlation;
            _percentChange = percentChange;
            _currentValue = currentValue;
            _projectedValue = projectedValue;
            if (riskResult.PNLImpact == 0)
            {
                _pnlImpact = "N/A";
            }
            else
            {
                _pnlImpact = riskResult.PNLImpact.ToString();
            }
        }
    }
}
