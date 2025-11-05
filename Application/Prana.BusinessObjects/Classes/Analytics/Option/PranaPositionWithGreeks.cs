using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class PranaPositionWithGreeks : TaxLot
    {
        public void UpdatePranaPositionWithGreeks(OptionGreeks greeks, double fxRate)
        {
            _delta = (float)greeks.Delta;
            _gamma = greeks.Gamma;
            _volatility = greeks.Volatility * 100;
            _strikePrice = greeks.StrikePrice;
            _theta = greeks.Theta;
            _vega = greeks.Vega;
            _simulatedPrice = greeks.SimulatedPrice;
            _simulatedUnderlyingStockPrice = greeks.SimulatedUnderlyingStockPrice;
            _interestRate = greeks.InterestRate * 100;
            _daysToExpiration = greeks.DaysToExpiration;
            _rho = greeks.Rho;
            //_putOrCalls = greeks.PutOrCalls;
            if (greeks.PutOrCalls.Equals('P'))
            {
                _putOrCall = 0;
            }
            else if (greeks.PutOrCalls.Equals('C'))
            {
                _putOrCall = 1;
            }
            _selectedFeedPrice = greeks.SelectedFeedPrice;
            _dividendYield = greeks.DividendYield * 100;
            _proxySymbol = greeks.ProxySymbol;
            _selectedFeedPriceInBaseCurrency = greeks.SelectedFeedPrice * fxRate;
            _simulatedPriceInBaseCurrency = greeks.SimulatedPrice * fxRate;
            _simulatedUnderlyingStockPriceInBaseCurrency = greeks.SimulatedUnderlyingStockPrice * fxRate;
        }

        public InputParametersForGreeks GetAllInputParams()
        {
            InputParametersForGreeks inputParams = new InputParametersForGreeks();
            inputParams.Symbol = _symbol;
            inputParams.Volatility = _volatility;
            inputParams.InterestRate = _interestRate;
            inputParams.SimulatedPrice = _simulatedPrice;

            //inputParams.PutOrCalls = _putOrCalls;
            if (_putOrCall == 0)
            {
                inputParams.PutOrCalls = 'P';
            }
            else if (_putOrCall == 1)
            {
                inputParams.PutOrCalls = 'C';
            }
            inputParams.SimulatedUnderlyingStockPrice = _simulatedUnderlyingStockPrice;
            inputParams.StrikePrice = _strikePrice;
            inputParams.UnderlyingSymbol = _underLyingsymbol;
            inputParams.ExpirationDate = _expirationDate;
            return inputParams;
        }

        public InputParametersForGreeks GetBasicInputParams()
        {
            InputParametersForGreeks inputParams = new InputParametersForGreeks();
            inputParams.Symbol = _symbol;
            inputParams.UnderlyingSymbol = _underLyingsymbol;
            return inputParams;
        }

        private double _deltaAdjustedExposure;
        public double DeltaAdjExposure
        {
            get { return _deltaAdjustedExposure; }
            set { _deltaAdjustedExposure = value; }
        }

        private double _simulatedPnl;
        public double SimulatedPnl
        {
            get { return _simulatedPnl; }
            set { _simulatedPnl = value; }
        }

        private double _costBasisUnrealizedPnL;
        public double CostBasisUnrealizedPnL
        {
            get { return _costBasisUnrealizedPnL; }
            set { _costBasisUnrealizedPnL = value; }
        }

        private double _deltaAdjustedPosition;
        public double DeltaAdjPosition
        {
            get { return _deltaAdjustedPosition; }
            set { _deltaAdjustedPosition = value; }
        }

        private double _gamma;
        public double Gamma
        {
            get { return _gamma; }
            set { _gamma = value; }
        }

        private double _theta;
        public double Theta
        {
            get { return _theta; }
            set { _theta = value; }
        }

        private double _vega;
        public double Vega
        {
            get { return _vega; }
            set { _vega = value; }
        }

        private double _rho;
        public double Rho
        {
            get { return _rho; }
            set { _rho = value; }
        }

        private double _volatility;
        public double Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }

        private int _daysToExpiration;
        public int DaysToExpiration
        {
            get { return _daysToExpiration; }
            set { _daysToExpiration = value; }
        }

        private double _interestRate;
        public double InterestRate
        {
            get { return _interestRate; }
            set { _interestRate = value; }
        }

        private double _simulatedUnderlyingStockPrice = 0.0;
        public double SimulatedUnderlyingStockPrice
        {
            get { return _simulatedUnderlyingStockPrice; }
            set { _simulatedUnderlyingStockPrice = value; }
        }

        private double _simulatedPrice;
        public double SimulatedPrice
        {
            get { return _simulatedPrice; }
            set { _simulatedPrice = value; }
        }

        private bool _isChecked = true;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        private double _selectedFeedPrice;
        public double SelectedFeedPrice
        {
            get { return _selectedFeedPrice; }
            set { _selectedFeedPrice = value; }
        }

        private double _marketValue;
        public double MarketValue
        {
            get { return _marketValue; }
            set { _marketValue = value; }
        }

        private double _dividendYield;
        public double DividendYield
        {
            get { return _dividendYield; }
            set { _dividendYield = value; }
        }

        private double _dollarDelta;
        public double DollarDelta
        {
            get { return _dollarDelta; }
            set { _dollarDelta = value; }
        }

        private double _dollarGamma;
        public double DollarGamma
        {
            get { return _dollarGamma; }
            set { _dollarGamma = value; }
        }

        private double _dollarTheta;
        public double DollarTheta
        {
            get { return _dollarTheta; }
            set { _dollarTheta = value; }
        }

        private double _dollarVega;
        public double DollarVega
        {
            get { return _dollarVega; }
            set { _dollarVega = value; }
        }

        private double _dollarRho;
        public double DollarRho
        {
            get { return _dollarRho; }
            set { _dollarRho = value; }
        }

        private string _positionSideMVInPortfolio = string.Empty;
        public string PositionSideMVInPortfolio
        {
            get { return _positionSideMVInPortfolio; }
            set { _positionSideMVInPortfolio = value; }
        }

        private string _positionSideExposureInPortfolio = string.Empty;
        public string PositionSideExposureInPortfolio
        {
            get { return _positionSideExposureInPortfolio; }
            set { _positionSideExposureInPortfolio = value; }
        }

        private string _pricingSource = "None";
        public string PricingSource
        {
            get { return _pricingSource; }
            set { _pricingSource = value; }
        }

        private double _deltaAdjustedPositionLME;
        public double DeltaAdjPositionLME
        {
            get { return _deltaAdjustedPositionLME; }
            set { _deltaAdjustedPositionLME = value; }
        }

        private DateTime _expirationMonth;
        public DateTime ExpirationMonth
        {
            get { return _expirationMonth; }
            set { _expirationMonth = value; }
        }

        private double _gammaAdjustedPosition;
        public double GammaAdjPosition
        {
            get { return _gammaAdjustedPosition; }
            set { _gammaAdjustedPosition = value; }
        }

        private double _beta;
        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        private double _betaAdjustedExposure;
        public double BetaAdjExposure
        {
            get { return _betaAdjustedExposure; }
            set { _betaAdjustedExposure = value; }
        }

        private double _selectedFeedPriceInBaseCurrency;
        public double SelectedFeedPriceInBaseCurrency
        {
            get { return _selectedFeedPriceInBaseCurrency; }
            set { _selectedFeedPriceInBaseCurrency = value; }
        }

        private double _betaAdjExposureInBaseCurrency;
        public double BetaAdjExposureInBaseCurrency
        {
            get { return _betaAdjExposureInBaseCurrency; }
            set { _betaAdjExposureInBaseCurrency = value; }
        }

        private double _costBasisUnrealizedPnlInCompanyBaseCurrency;
        public double CostBasisUnrealizedPnLInBaseCurrency
        {
            get { return _costBasisUnrealizedPnlInCompanyBaseCurrency; }
            set { _costBasisUnrealizedPnlInCompanyBaseCurrency = value; }
        }

        private double _deltaAdjExposureInBaseCurrency;
        public double DeltaAdjExposureInBaseCurrency
        {
            get { return _deltaAdjExposureInBaseCurrency; }
            set { _deltaAdjExposureInBaseCurrency = value; }
        }

        private double _dollarDeltaInBaseCurrency;
        public double DollarDeltaInBaseCurrency
        {
            get { return _dollarDeltaInBaseCurrency; }
            set { _dollarDeltaInBaseCurrency = value; }
        }

        private double _dollarGammaInBaseCurrency;
        public double DollarGammaInBaseCurrency
        {
            get { return _dollarGammaInBaseCurrency; }
            set { _dollarGammaInBaseCurrency = value; }
        }

        private double _dollarRhoInBaseCurrency;
        public double DollarRhoInBaseCurrency
        {
            get { return _dollarRhoInBaseCurrency; }
            set { _dollarRhoInBaseCurrency = value; }
        }

        private double _dollarThetaInBaseCurrency;
        public double DollarThetaInBaseCurrency
        {
            get { return _dollarThetaInBaseCurrency; }
            set { _dollarThetaInBaseCurrency = value; }
        }

        private double _dollarVegaInBaseCurrency;
        public double DollarVegaInBaseCurrency
        {
            get { return _dollarVegaInBaseCurrency; }
            set { _dollarVegaInBaseCurrency = value; }
        }

        private double _marketValueInBaseCurrency;
        public double MarketValueInBaseCurrency
        {
            get { return _marketValueInBaseCurrency; }
            set { _marketValueInBaseCurrency = value; }
        }

        private double _simulatedPnlInBaseCurrency;
        public double SimulatedPnlInBaseCurrency
        {
            get { return _simulatedPnlInBaseCurrency; }
            set { _simulatedPnlInBaseCurrency = value; }
        }

        private double _simulatedPriceInBaseCurrency;
        public double SimulatedPriceInBaseCurrency
        {
            get { return _simulatedPriceInBaseCurrency; }
            set { _simulatedPriceInBaseCurrency = value; }
        }

        private double _simulatedUnderlyingStockPriceInBaseCurrency;
        public double SimulatedUnderlyingStockPriceInBaseCurrency
        {
            get { return _simulatedUnderlyingStockPriceInBaseCurrency; }
            set { _simulatedUnderlyingStockPriceInBaseCurrency = value; }
        }

        private double _avgPriceInBaseCurrency;
        public double AvgPriceInBaseCurrency
        {
            get { return _avgPriceInBaseCurrency; }
            set { _avgPriceInBaseCurrency = value; }
        }
    }
}
