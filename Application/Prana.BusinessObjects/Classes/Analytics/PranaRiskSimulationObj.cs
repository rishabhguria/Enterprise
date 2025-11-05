using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class PranaRiskSimulationObj
    {
        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private int _assetID;
        [Browsable(false)]
        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        private string _assetName;
        public string AssetName
        {
            get { return _assetName; }
            set { _assetName = value; }
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

        private string _underlyingSymbol;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        private double _oldquantity;
        public double OldQuantity
        {
            get { return _oldquantity; }
            set { _oldquantity = value; }
        }

        private double _newquantity;
        public double NewQuantity
        {
            get { return _newquantity; }
            set { _newquantity = value; }
        }

        private double _oldrisk;
        public double OldRisk
        {
            get { return _oldrisk; }
            set { _oldrisk = value; }
        }

        private double _newrisk;
        public double NewRisk
        {
            get { return _newrisk; }
            set { _newrisk = value; }
        }

        private double _oldStandardDeviation;
        public double OldStandardDeviation
        {
            get { return _oldStandardDeviation; }
            set { _oldStandardDeviation = value; }
        }

        private double _newStandardDeviation;
        public double NewStandardDeviation
        {
            get { return _newStandardDeviation; }
            set { _newStandardDeviation = value; }
        }

        private double _oldComponentRisk;
        public double OldComponentRisk
        {
            get { return _oldComponentRisk; }
            set { _oldComponentRisk = value; }
        }

        private double _newComponentRisk;
        public double NewComponentRisk
        {
            get { return _newComponentRisk; }
            set { _newComponentRisk = value; }
        }

        private float _percentageChange;
        public float PercentageChange
        {
            get { return _percentageChange; }
            set { _percentageChange = value; }
        }

        private string _securityName;
        public string SecurityName
        {
            get { return _securityName; }
            set { _securityName = value; }
        }

        private bool _isChecked = true;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        private string _account;
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        private string _strategy;
        public string Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        private string _sectorName;
        public string SectorName
        {
            get { return _sectorName; }
            set { _sectorName = value; }
        }

        private string _countryName;
        public string CountryName
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        private string _udaAsset;
        public string UDAAsset
        {
            get { return _udaAsset; }
            set { _udaAsset = value; }
        }

        private string _securityTypeName;
        public string SecurityTypeName
        {
            get { return _securityTypeName; }
            set { _securityTypeName = value; }
        }

        private int _sideMultiplier = 1;
        public int SideMultiplier
        {
            get { return _sideMultiplier; }
            set { _sideMultiplier = value; }
        }

        private string _masterFund;
        public string MasterFund
        {
            get { return _masterFund; }
            set { _masterFund = value; }
        }

        private double _contractMultiplier;
        public double ContractMultiplier
        {
            get { return _contractMultiplier; }
            set { _contractMultiplier = value; }
        }

        private string _tradeAttribute1 = string.Empty;
        public string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = value; }
        }

        private string _tradeAttribute2 = string.Empty;
        public string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = value; }
        }

        private string _tradeAttribute3 = string.Empty;
        public string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = value; }
        }


        private string _tradeAttribute4 = string.Empty;
        public string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = value; }
        }

        private string _tradeAttribute5 = string.Empty;
        public string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = value; }
        }

        private string _tradeAttribute6 = string.Empty;
        public string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = value; }
        }

        private string _bloombergSymbol = string.Empty;
        public virtual string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }
        private string _bloombergSymbolWithExchangeCode = string.Empty;
        public virtual string BloombergSymbolWithExchangeCode
        {
            get { return _bloombergSymbolWithExchangeCode; }
            set { _bloombergSymbolWithExchangeCode = value; }
        }

        private string _factSetSymbol = string.Empty;
        public virtual string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }
        }

        private string _activSymbol = string.Empty;
        public virtual string ActivSymbol
        {
            get { return _activSymbol; }
            set { _activSymbol = value; }
        }

        private string _idcoSymbol = string.Empty;
        public virtual string IDCOSymbol
        {
            get { return _idcoSymbol; }
            set { _idcoSymbol = value; }
        }

        private string _isinSymbol = string.Empty;
        public virtual string ISINSymbol
        {
            get { return _isinSymbol; }
            set { _isinSymbol = value; }
        }

        private string _sedolSymbol = string.Empty;
        public virtual string SedolSymbol
        {
            get { return _sedolSymbol; }
            set { _sedolSymbol = value; }
        }

        private string _osiSymbol = string.Empty;
        public virtual string OSISymbol
        {
            get { return _osiSymbol; }
            set { _osiSymbol = value; }
        }

        private string _cusipSymbol = string.Empty;
        public virtual string CusipSymbol
        {
            get { return _cusipSymbol; }
            set { _cusipSymbol = value; }
        }

        private double _delta;
        public double Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        private double _oldDeltaAdjPosition;
        public double OldDeltaAdjPosition
        {
            get { return _oldDeltaAdjPosition; }
            set { _oldDeltaAdjPosition = value; }
        }

        private double _newDeltaAdjPosition;
        public double NewDeltaAdjPosition
        {
            get { return _newDeltaAdjPosition; }
            set { _newDeltaAdjPosition = value; }
        }
    }
}
