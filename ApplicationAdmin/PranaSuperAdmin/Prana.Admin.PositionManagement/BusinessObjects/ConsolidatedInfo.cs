using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class ConsolidatedInfo
    {
        private DataSourceNameID _dataSourceNameIDValue;

        public DataSourceNameID DataSourceNameIDValue
        {
            get { return _dataSourceNameIDValue; }
            set { _dataSourceNameIDValue = value; }
        }

        private Fund _fundValue;

        public Fund FundValue
        {
            get 
            {
                if (_fundValue == null)
                {
                    _fundValue = new Fund();
                }
                return _fundValue; 
            }
            set { _fundValue = value; }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private int _qty;

        public int Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }

        private double _cost;

        public double Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        private int _multiplier;

        public int Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }

        private Currency _currencyValue;

        public Currency CurrencyValue
        {
            get
            {
                if (_currencyValue == null)
                {
                    _currencyValue = new Currency();
                }
                return _currencyValue; 
            }
            set { _currencyValue = value; }
        }

        private double _fxRate;

        public double FXRate
        {
            get { return _fxRate; }
            set { _fxRate = value; }
        }

        private int _daysTradedQty;

        public int DaysTradedQty
        {
            get { return _daysTradedQty; }
            set { _daysTradedQty = value; }
        }

        private double _dayAveragePrice;

        public double DayAveragePrice
        {
            get { return _dayAveragePrice; }
            set { _dayAveragePrice = value; }
        }

        private int _netPosition;

        public int NetPosition
        {
            get { return _netPosition; }
            set { _netPosition = value; }
        }

        private double  _lastPrice;

        public double  LastPrice
        {
            get { return _lastPrice; }
            set { _lastPrice = value; }
        }

        private double _netExposure;

        public double NetExposure
        {
            get { return _netExposure; }
            set { _netExposure = value; }
        }


        private double _weightedLongPrice;

        public double WeightedLongPrice
        {
            get { return _weightedLongPrice; }
            set { _weightedLongPrice = value; }
        }

        private double _weightedShortPrice;

        public double WeightedShortPrice
        {
            get { return _weightedShortPrice; }
            set { _weightedShortPrice = value; }
        }

        private double _netLong;

        public double NetLong
        {
            get { return _netLong; }
            set { _netLong = value; }
        }

        private double _netShort;

        public double NetShort
        {
            get { return _netShort; }
            set { _netShort = value; }
        }

        private double _netPNL;

        public double NetPNL
        {
            get { return _netPNL; }
            set { _netPNL = value; }
        }

        private double _realisedPNL;

        public double RealisedPNL
        {
            get { return _realisedPNL; }
            set { _realisedPNL = value; }
        }

        private double _unrealisedPNL;

        public double UnrealisedPNL
        {
            get { return _unrealisedPNL; }
            set { _unrealisedPNL = value; }
        }

        private double _netLongExposure;

        public double NetLongExposure
        {
            get { return _netLongExposure; }
            set { _netLongExposure = value; }
        }

        private double _netShortExposure;

        public double NetShortExposure
        {
            get { return _netShortExposure; }
            set { _netShortExposure = value; }
        }

        private double _securityWeightLong;

        public double SecurityWeightLong
        {
            get { return _securityWeightLong; }
            set { _securityWeightLong = value; }
        }

        private double _securityWeightShort;

        public double SecurityWeightShort
        {
            get { return _securityWeightShort; }
            set { _securityWeightShort = value; }
        }

        private double _pnlLong;

        public double PNLLong
        {
            get { return _pnlLong; }
            set { _pnlLong = value; }
        }

        private double _pnlShort;

        public double PNLShort
        {
            get { return _pnlShort; }
            set { _pnlShort = value; }
        }

        private double _longExposure;

        public double LongExposure
        {
            get { return _longExposure; }
            set { _longExposure = value; }
        }

        private double _shortExposure;

        public double ShortExposure
        {
            get { return _shortExposure; }
            set { _shortExposure = value; }
        }

    }
}
