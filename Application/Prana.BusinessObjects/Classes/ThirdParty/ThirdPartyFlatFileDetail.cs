using Prana.BusinessObjects.AppConstants;
using System;
using System.Xml;
using System.Xml.Serialization;


namespace Prana.BusinessObjects
{
    [Serializable]
    public class ThirdPartyFlatFileDetail
    {
        #region Private Members

        private string _tradeRefID = string.Empty;
        private int _companyID = int.MinValue;
        private int _thirdPartyID = int.MinValue;
        private string _thirdPartyName = string.Empty;
        private string _sideTag = string.Empty;
        private string _side = string.Empty;
        private string _symbol = string.Empty;
        private double _executedQty = double.MinValue;
        private double _averagePrice = double.MinValue;
        private int _companyAccountID = int.MinValue;
        private string _companyAccountName = string.Empty;
        private string _accountMappedName = string.Empty;
        private string _accountNo = string.Empty;
        private int _orderTypeTag = int.MinValue;
        private string _orderType = string.Empty;
        private int _assetID = Int32.MinValue;
        private string _assetName = string.Empty;
        private int _underlyingID = Int32.MinValue;
        private string _underlyingName = string.Empty;
        private int _exchangeID = Int32.MinValue;
        private string _exchangeName = string.Empty;
        private int _currencyID = Int32.MinValue;
        private string _currencyName = string.Empty;
        private string _currencySymbol = string.Empty;
        private int _auecID = Int32.MinValue;
        private double _secFees = double.MinValue;
        private double _commissionCharged = double.MinValue;
        private double _SoftCommissionCharged = double.MinValue;
        private double _commission = double.MinValue;
        private double _softCommission = double.MinValue;
        private double _grossAmt = double.MinValue;
        private double _netAmt = double.MinValue;
        private string _securityIDType = "Ticker";
        private int _commissionRateTypeID = int.MinValue;
        private string _commissionRateType = string.Empty;
        private string _tradeDate = string.Empty;
        private string _tradeDateTime = string.Empty;
        private string _savePath = string.Empty;
        private string _namingConvention = string.Empty;
        private int _venueID = int.MinValue;
        private string _venueName = string.Empty;
        private string _cVIdentifier = string.Empty;
        private int _companyAccountTypeID = int.MinValue;
        private string _companyAccountType = string.Empty;
        private int _thirdPartyTypeID = int.MinValue;
        private string _thirdPartyType = string.Empty;
        private string _companyIdentifier = string.Empty;
        private double _percentage = double.MinValue;
        private double _orderQty = double.MinValue;
        private string _entityID = string.Empty;
        //private Int64 _orderID = Int64.MinValue;
        private int _counterpartyID = int.MinValue;
        private string _counterparty = string.Empty;
        //private int _tradeAccntID = int.MinValue;
        private double _allocationQty = double.MinValue;
        private double _totalQty = double.MinValue;
        //private string _tradAccntName = string.Empty;

        private int _thirdPartyFFRunID = int.MinValue;
        private int _fFUserID = int.MinValue;
        private int _statusID = int.MinValue;
        private string _status = string.Empty;
        private string _filePathName = string.Empty;
        private string _Prana2ClientXSLTPath = string.Empty;
        private string _client2PranaXSLTPath = string.Empty;
        private int _companyCVID = int.MinValue;
        private string _cvName = string.Empty;
        private string _listID = string.Empty;
        private double _otherBrokerFee = 0;
        private double _clearingBrokerFee = 0;
        private string _putOrCall = string.Empty;
        private string _vWAP = string.Empty;

        #endregion Private Members

        #region Constructor

        public ThirdPartyFlatFileDetail()
        {

        }

        #endregion Constructor

        #region Properties

        [XmlElement("ListID")]
        public string ListID
        {
            get { return _listID; }
            set { _listID = value; }
        }

        [XmlElement("CVName")]
        public string CVName
        {
            get { return _cvName; }
            set { _cvName = value; }
        }

        [XmlElement("CompanyCVID")]
        public int CompanyCVID
        {
            get { return _companyCVID; }
            set { _companyCVID = value; }
        }

        [XmlElement("ThirdPartyFFRunID")]
        public int ThirdPartyFFRunID
        {
            get { return _thirdPartyFFRunID; }
            set { _thirdPartyFFRunID = value; }
        }

        [XmlElement("FFUserID")]
        public int FFUserID
        {
            get { return _fFUserID; }
            set { _fFUserID = value; }
        }

        [XmlElement("StatusID")]
        public int StatusID
        {
            get { return _statusID; }
            set { _statusID = value; }
        }

        [XmlElement("Status")]
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        [XmlElement("FilePathName")]
        public string FilePathName
        {
            get { return _filePathName; }
            set { _filePathName = value; }
        }

        [XmlElement("Prana2ClientXSLTPath")]
        public string Prana2ClientXSLTPath
        {
            get { return _Prana2ClientXSLTPath; }
            set { _Prana2ClientXSLTPath = value; }
        }

        [XmlElement("Client2PranaXSLTPath")]
        public string Client2PranaXSLTPath
        {
            get { return _client2PranaXSLTPath; }
            set { _client2PranaXSLTPath = value; }
        }

        [XmlElement("TotalQty")]
        public double TotalQty
        {
            get { return _totalQty; }
            set { _totalQty = value; }
        }

        [XmlElement("AllocatedQty")]
        public double AllocatedQty
        {
            get { return _allocationQty; }
            set { _allocationQty = value; }
        }

        //[XmlElement("TradAccntID")]
        //public int TradAccntID
        //{
        //    get { return _tradeAccntID; }
        //    set { _tradeAccntID = value; }
        //}

        [XmlElement("CounterParty")]
        public string CounterParty
        {
            get { return _counterparty; }
            set { _counterparty = value; }
        }

        [XmlElement("CounterPartyID")]
        public int CounterPartyID
        {
            get { return _counterpartyID; }
            set { _counterpartyID = value; }
        }

        //[XmlElement("OrderID")]
        //public Int64 OrderID
        //{
        //    get 
        //    {
        //        return _orderID; 
        //    }
        //    set 
        //    {
        //        _orderID = value;
        //    }
        //}

        [XmlElement("EntityID")]
        public string EntityID
        {
            get
            {
                return _entityID;
            }
            set
            {
                _entityID = value;
            }
        }

        [XmlElement("OrderQty")]
        public double OrderQty
        {
            get
            {
                return _orderQty;
            }
            set
            {
                _orderQty = value;
            }
        }

        [XmlElement("TradeRefID")]
        public string TradeRefID
        {
            get
            {
                return _tradeRefID;
            }
            set
            {
                _tradeRefID = value;
            }
        }

        [XmlElement("CompanyID")]
        public int CompanyID
        {
            get
            {
                return _companyID;
            }
            set
            {
                _companyID = value;
            }
        }

        [XmlElement("ThirdPartyID")]
        public int ThirdPartyID
        {
            get
            {
                return _thirdPartyID;
            }
            set
            {
                _thirdPartyID = value;
            }
        }

        [XmlElement("ThirdParty")]
        public string ThirdParty
        {
            get
            {
                return _thirdPartyName;
            }
            set
            {
                _thirdPartyName = value;
            }
        }

        [XmlElement("SideTag")]
        public string SideTag
        {
            get
            {
                return _sideTag;
            }
            set
            {
                _sideTag = value;
            }
        }

        [XmlElement("Side")]
        public string Side
        {
            get
            {
                return _side;
            }
            set
            {
                _side = value;
            }
        }

        [XmlElement("Symbol")]
        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
            }
        }

        [XmlElement("ExecutedQty")]
        public double ExecutedQty
        {
            get
            {
                return _executedQty;
            }
            set
            {
                _executedQty = value;
            }
        }

        [XmlElement("AveragePrice")]
        public double AveragePrice
        {
            get
            {
                return _averagePrice;
            }
            set
            {
                _averagePrice = value;
            }
        }

        [XmlElement("CompanyAccountID")]
        public int CompanyAccountID
        {
            get
            {
                return _companyAccountID;
            }
            set
            {
                _companyAccountID = value;
            }
        }

        [XmlElement("AccountName")]
        public string AccountName
        {
            get
            {
                return _companyAccountName;
            }
            set
            {
                _companyAccountName = value;
            }
        }

        [XmlElement("AccountMappedName")]
        public string AccountMappedName
        {
            get
            {
                return _accountMappedName;
            }
            set
            {
                _accountMappedName = value;
            }
        }

        [XmlElement("AccountNo")]
        public string AccountNo
        {
            get
            {
                return _accountNo;
            }
            set
            {
                _accountNo = value;
            }
        }

        [XmlElement("OrderTypeTag")]
        public int OrderTypeTag
        {
            get
            {
                return _orderTypeTag;
            }
            set
            {
                _orderTypeTag = value;
            }
        }


        [XmlElement("OrderType")]
        public string OrderType
        {
            get
            {
                return _orderType;
            }
            set
            {
                _orderType = value;
            }
        }

        [XmlElement("AssetID")]
        public int AssetID
        {
            get
            {
                return _assetID;
            }
            set
            {
                _assetID = value;
            }
        }

        [XmlElement("Asset")]
        public string Asset
        {
            get
            {
                return _assetName;
            }
            set
            {
                _assetName = value;
            }
        }

        [XmlElement("UnderLyingID")]
        public int UnderLyingID
        {
            get
            {
                return _underlyingID;
            }
            set
            {
                _underlyingID = value;
            }
        }

        [XmlElement("UnderLying")]
        public string UnderLying
        {
            get
            {
                return _underlyingName;
            }
            set
            {
                _underlyingName = value;
            }
        }

        [XmlElement("ExchangeID")]
        public int ExchangeID
        {
            get
            {
                return _exchangeID;
            }
            set
            {
                _exchangeID = value;
            }
        }

        [XmlElement("Exchange")]
        public string Exchange
        {
            get
            {
                return _exchangeName;
            }
            set
            {
                _exchangeName = value;
            }
        }

        [XmlElement("CurrencyID")]
        public int CurrencyID
        {
            get
            {
                return _currencyID;
            }
            set
            {
                _currencyID = value;
            }
        }

        [XmlElement("CurrencyName")]
        public string CurrencyName
        {
            get
            {
                return _currencyName;
            }
            set
            {
                _currencyName = value;
            }
        }
        [XmlElement("CurrencySymbol")]
        public string CurrencySymbol
        {
            get
            {
                return _currencySymbol;
            }
            set
            {
                _currencySymbol = value;
            }
        }


        [XmlElement("AUECID")]
        public int AUECID
        {
            get
            {
                return _auecID;
            }
            set
            {
                _auecID = value;
            }
        }

        [XmlElement("SecFees")]
        public double SecFees
        {
            get
            {
                return _secFees;
            }
            set
            {
                _secFees = value;
            }
        }

        [XmlElement("Commission")]
        public double Commission
        {
            get
            {
                //if(_commissionCharged != double.MinValue)
                //{
                ////write the formula for calculating the commision.
                //}
                return _commission;
            }
            set
            {
                _commission = value;
            }
        }

        [XmlElement("SoftCommission")]
        public double SoftCommission
        {
            get
            {
                return _softCommission;
            }
            set
            {
                _softCommission = value;
            }
        }
        [XmlElement("CommissionCharged")]
        public double CommissionCharged
        {
            get
            {
                return _commissionCharged;
            }
            set
            {
                _commissionCharged = value;
            }
        }
        [XmlElement("SoftCommissionCharged")]
        public double SoftCommissionCharged
        {
            get
            {
                return _SoftCommissionCharged;
            }
            set
            {
                _SoftCommissionCharged = value;
            }
        }

        [XmlElement("GrossAmount")]
        public double GrossAmount
        {
            get
            {
                //if (_executedQty != int.MinValue && _averagePrice != double.MinValue)
                //{ 
                //    _grossAmt = (_executedQty * _averagePrice);
                //}
                return _grossAmt;
            }
            set
            {
                _grossAmt = value;
            }
        }

        [XmlElement("NetAmount")]
        public double NetAmount
        {
            get
            {
                //if (_grossAmt != double.MinValue && _commission != double.MinValue && _secFees != double.MinValue)
                //{ 
                //    _netAmt = (_grossAmt-_commission- _secFees);
                //}
                return _netAmt;
            }
            set
            {
                _netAmt = value;
            }
        }

        [XmlElement("SecurityIDType")]
        public string SecurityIDType
        {
            get
            {
                return _securityIDType;
            }
            set
            {
                _securityIDType = value;
            }
        }

        [XmlElement("CommissionRateTypeID")]
        public int CommissionRateTypeID
        {
            get
            {
                return _commissionRateTypeID;
            }
            set
            {
                _commissionRateTypeID = value;
            }
        }

        [XmlElement("CommissionRateType")]
        public string CommissionRateType
        {
            get
            {
                return _commissionRateType;
            }
            set
            {
                _commissionRateType = value;
            }
        }

        [XmlElement("TradeDate")]
        public string TradeDate
        {
            get
            {
                return _tradeDate;
            }
            set
            {
                _tradeDate = value;
            }
        }

        [XmlElement("TradeDateTime")]
        public string TradeDateTime
        {
            get
            {
                return _tradeDateTime;
            }
            set
            {
                _tradeDateTime = value;
            }
        }

        [XmlElement("SavePath")]
        public string SavePath
        {
            get
            {
                return _savePath;
            }
            set
            {
                _savePath = value;
            }
        }

        [XmlElement("NamingConvention")]
        public string NamingConvention
        {
            get
            {
                return _namingConvention;
            }
            set
            {
                _namingConvention = value;
            }
        }

        [XmlElement("VenueID")]
        public int VenueID
        {
            get
            {
                return _venueID;
            }
            set
            {
                _venueID = value;
            }
        }

        [XmlElement("VenueName")]
        public string VenueName
        {
            get
            {
                return _venueName;
            }
            set
            {
                _venueName = value;
            }
        }

        [XmlElement("CVIdentifier")]
        public string CVIdentifier
        {
            get
            {
                return _cVIdentifier;
            }
            set
            {
                _cVIdentifier = value;
            }
        }

        [XmlElement("CompanyAccountTypeID")]
        public int CompanyAccountTypeID
        {
            get
            {
                return _companyAccountTypeID;
            }
            set
            {
                _companyAccountTypeID = value;
            }
        }

        [XmlElement("CompanyAccountType")]
        public string CompanyAccountType
        {
            get
            {
                return _companyAccountType;
            }
            set
            {
                _companyAccountType = value;
            }
        }

        [XmlElement("ThirdPartyTypeID")]
        public int ThirdPartyTypeID
        {
            get
            {
                return _thirdPartyTypeID;
            }
            set
            {
                _thirdPartyTypeID = value;
            }
        }

        [XmlElement("ThirdPartyType")]
        public string ThirdPartyType
        {
            get
            {
                return _thirdPartyType;
            }
            set
            {
                _thirdPartyType = value;
            }
        }

        [XmlElement("CompanyIdentifier")]
        public string CompanyIdentifier
        {
            get
            {
                return _companyIdentifier;
            }
            set
            {
                _companyIdentifier = value;
            }
        }

        [XmlElement("Percentage")]
        public double Percentage
        {
            get
            {
                return _percentage;
            }
            set
            {
                _percentage = value;
            }

        }

        [XmlElement("OtherBrokerFee")]
        public double OtherBrokerFee
        {
            get
            {
                return _otherBrokerFee;
            }
            set
            {
                _otherBrokerFee = value;
            }

        }

        [XmlElement("VWAP")]
        public string VWAP
        {
            get
            {
                return _vWAP;
            }
            set
            {
                _vWAP = value;
            }

        }

        [XmlElement("ClearingBrokerFee")]
        public double ClearingBrokerFee
        {
            get
            {
                return _clearingBrokerFee;
            }
            set
            {
                _clearingBrokerFee = value;
            }

        }

        // added by sandeep on 28-sept-2007
        [XmlElement("PutOrCall")]
        public string PutOrCall
        {
            get
            {
                return _putOrCall;
            }
            set
            {
                _putOrCall = value;
            }

        }
        // added by sandeep on 04-oct-2007
        double _strikePrice = 0;
        [XmlElement("StrikePrice")]
        public double StrikePrice
        {
            get
            {
                return _strikePrice;
            }
            set
            {
                _strikePrice = value;
            }

        }
        // added by sandeep on 04-oct-2007
        string _expirationDate = string.Empty;
        [XmlElement("ExpirationDate")]
        public string ExpirationDate
        {
            get
            {
                return _expirationDate;
            }
            set
            {
                _expirationDate = value;
            }

        }
        // added by sandeep on 04-oct-2007
        string _settlementDate = string.Empty;
        [XmlElement("SettlementDate")]
        public string SettlementDate
        {
            get
            {
                return _settlementDate;
            }
            set
            {
                _settlementDate = value;
            }

        }

        // added by sandeep on 09-April-2008
        string _cUSIP = string.Empty;
        [XmlElement("CUSIP")]
        public string CUSIP
        {
            get
            {
                return _cUSIP;
            }
            set
            {
                _cUSIP = value;
            }

        }

        string _iSIN = string.Empty;
        [XmlElement("ISIN")]
        public string ISIN
        {
            get
            {
                return _iSIN;
            }
            set
            {
                _iSIN = value;
            }

        }

        string _sEDOL = string.Empty;
        [XmlElement("SEDOL")]
        public string SEDOL
        {
            get
            {
                return _sEDOL;
            }
            set
            {
                _sEDOL = value;
            }

        }

        string _rIC = string.Empty;
        [XmlElement("RIC")]
        public string RIC
        {
            get
            {
                return _rIC;
            }
            set
            {
                _rIC = value;
            }

        }
        /// <summary>
        /// Bloomberg Code
        /// </summary>
        string _bBCode = string.Empty;
        [XmlElement("BBCode")]
        public string BBCode
        {
            get
            {
                return _bBCode;
            }
            set
            {
                _bBCode = value;
            }

        }
        /// <summary>
        /// OSI 21
        /// </summary>
        string _osiOptionSymbol = string.Empty;
        [XmlElement("OSIOptionSymbol")]
        public string OSIOptionSymbol
        {
            get { return _osiOptionSymbol; }
            set { _osiOptionSymbol = value; }
        }

        /// <summary>
        /// IDCO 22
        /// </summary>
        string _idcoOptionSymbol = string.Empty;
        [XmlElement("IDCOOptionSymbol")]
        public string IDCOOptionSymbol
        {
            get { return _idcoOptionSymbol; }
            set { _idcoOptionSymbol = value; }
        }

        /// <summary>
        /// Opra Code
        /// </summary>
        private string _opraOptionSymbol = string.Empty;
        [XmlElement("OpraOptionSymbol")]
        public string OpraOptionSymbol
        {
            get { return _opraOptionSymbol; }
            set { _opraOptionSymbol = value; }
        }

        /// <summary>
        /// Security Full Name
        /// </summary>
        string _fullSecurityName = string.Empty;
        [XmlElement("FullSecurityName")]
        public string FullSecurityName
        {
            get { return _fullSecurityName; }
            set { _fullSecurityName = value; }
        }
        /// <summary>
        /// Prime Broker Unique ID
        /// </summary>
        Int64 _pbUniqueID = 0;
        [XmlElement("PBUniqueID")]
        public Int64 PBUniqueID
        {
            get { return _pbUniqueID; }
            set { _pbUniqueID = value; }
        }

        /// <summary>
        /// Client Allocation Sequence number
        /// </summary>
        int _allocationSeqNo = 0;
        [XmlElement("AllocationSeqNo")]
        public int AllocationSeqNo
        {
            get { return _allocationSeqNo; }
            set { _allocationSeqNo = value; }
        }

        /// <summary>
        /// Underlying Symbol
        /// </summary>
        string _underlyingSymbol = string.Empty;
        [XmlElement("UnderlyingSymbol")]
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        string _groupEnds = string.Empty;
        public string GroupEnds
        {
            get { return _groupEnds; }
            set { _groupEnds = value; }
        }

        string _groupAllocationReq = string.Empty;
        public string GroupAllocationReq
        {
            get { return _groupAllocationReq; }
            set { _groupAllocationReq = value; }
        }

        string _fileHeader = string.Empty;
        public string FileHeader
        {
            get { return _fileHeader; }
            set { _fileHeader = value; }
        }
        string _fileFooter = string.Empty;
        public string FileFooter
        {
            get { return _fileFooter; }
            set { _fileFooter = value; }
        }

        private int _taxLotStateID;
        [XmlElement("TaxLotStateID")]
        public int TaxLotStateID
        {
            get { return _taxLotStateID; }
            set { _taxLotStateID = value; }
        }

        private PranaTaxLotState _taxLotState;

        public PranaTaxLotState TaxLotState
        {
            get { return _taxLotState; }
            set { _taxLotState = value; }
        }

        private int _vsCurrencyID = 0;
        [XmlElement("VsCurrencyID")]
        public int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        private string _vsCurrencyName = string.Empty;
        [XmlElement("VsCurrencyName")]
        public string VsCurrencyName
        {
            get { return _vsCurrencyName; }
            set { _vsCurrencyName = value; }
        }

        private int _leadCurrencyID = 0;
        [XmlElement("LeadCurrencyID")]
        public int LeadCurrencyID
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }

        private string _leadCurrencyName = string.Empty;
        [XmlElement("LeadCurrencyName")]
        public string LeadCurrencyName
        {
            get { return _leadCurrencyName; }
            set { _leadCurrencyName = value; }
        }

        private double _stampDuty = 0;
        [XmlElement("StampDuty")]
        public double StampDuty
        {
            get { return _stampDuty; }
            set { _stampDuty = value; }
        }
        private double _transactionLevy = 0;
        [XmlElement("TransactionLevy")]
        public double TransactionLevy
        {
            get { return _transactionLevy; }
            set { _transactionLevy = value; }
        }

        private double _clearingFee = 0;
        [XmlElement("ClearingFee")]
        public double ClearingFee
        {
            get { return _clearingFee; }
            set { _clearingFee = value; }
        }
        private double _taxOnCommissions = 0;
        [XmlElement("TaxOnCommissions")]
        public double TaxOnCommissions
        {
            get { return _taxOnCommissions; }
            set { _taxOnCommissions = value; }
        }
        private double _miscFees = 0;
        [XmlElement("MiscFees")]
        public double MiscFees
        {
            get { return _miscFees; }
            set { _miscFees = value; }
        }

        private double _secFee = 0;
        [XmlElement("SecFee")]
        public double SecFee
        {
            get { return _secFee; }
            set { _secFee = value; }
        }
        private double _occFee = 0;
        [XmlElement("OccFee")]
        public double OccFee
        {
            get { return _occFee; }
            set { _occFee = value; }
        }
        private double _orfFee = 0;
        [XmlElement("OrfFee")]
        public double OrfFee
        {
            get { return _orfFee; }
            set { _orfFee = value; }
        }

        private double _forexRate = 0;
        [XmlElement("ForexRate")]
        public double ForexRate
        {
            get { return _forexRate; }
            set { _forexRate = value; }
        }

        private double _assetMultiplier = 0;
        [XmlElement("AssetMultiplier")]
        public double AssetMultiplier
        {
            get { return _assetMultiplier; }
            set { _assetMultiplier = value; }
        }

        private int _strategyID = 0;
        [XmlElement("StrategyID")]
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private string _strategy = string.Empty;
        [XmlElement("Strategy")]
        public string Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }
        private string _settlCurrency = string.Empty;
        [XmlElement("SettlCurrency")]
        public string SettlCurrency
        {
            get { return _settlCurrency; }
            set { _settlCurrency = value; }
        }
        private double _forexRate_Trade = 0;
        [XmlElement("ForexRate_Trade")]
        public double ForexRate_Trade
        {
            get { return _forexRate_Trade; }
            set { _forexRate_Trade = value; }
        }

        private string _fxConversionMethodOperator_Trade = string.Empty;
        [XmlElement("FXConversionMethodOperator_Trade")]
        public string FXConversionMethodOperator_Trade
        {
            get { return _fxConversionMethodOperator_Trade; }
            set { _fxConversionMethodOperator_Trade = value; }
        }

        private string _fromDeleted;
        [XmlElement("FromDeleted")]
        public string FromDeleted
        {
            get { return _fromDeleted; }
            set { _fromDeleted = value; }
        }

        // added by sandeep on 08-Feb-2012
        private string _processDate = string.Empty;
        [XmlElement("ProcessDate")]
        public string ProcessDate
        {
            get { return _processDate; }
            set { _processDate = value; }
        }

        // added by sandeep on 08-Feb-2012
        private string _originalPurchaseDate = string.Empty;
        [XmlElement("OriginalPurchaseDate")]
        public string OriginalPurchaseDate
        {
            get { return _originalPurchaseDate; }
            set { _originalPurchaseDate = value; }
        }

        // added by sandeep on 08-Feb-2012
        private double _accruedInterest = 0;
        [XmlElement("AccruedInterest")]
        public double AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }

        // added by sandeep on 08-Feb-2012
        private string _comment1 = string.Empty;
        [XmlElement("Comment1")]
        public string Comment1
        {
            get { return _comment1; }
            set { _comment1 = value; }
        }

        // added by sandeep on 08-Feb-2012
        private string _comment2 = string.Empty;
        [XmlElement("Comment2")]
        public string Comment2
        {
            get { return _comment2; }
            set { _comment2 = value; }
        }

        //Added 7 columns for FixedIncome: [RG 07252012] 
        #region FixedIncome columns

        private double _coupon;
        [XmlElement("Coupon")]
        public double Coupon
        {
            get { return _coupon; }
            set { _coupon = value; }
        }

        private int _accrualBasisID;
        public int AccrualBasisID
        {
            get { return _accrualBasisID; }
            set { _accrualBasisID = value; }
        }

        private AccrualBasis _accrualBasis;
        [XmlElement("AccrualBasis")]
        public AccrualBasis AccrualBasis
        {
            get { return _accrualBasis; }
            set { _accrualBasis = value; }
        }

        private int _couponFrequencyID;
        public int CouponFrequencyID
        {
            get { return _couponFrequencyID; }
            set { _couponFrequencyID = value; }
        }

        private CouponFrequency _frequency;
        [XmlElement("Frequency")]
        public CouponFrequency Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        private string _dateIssue = string.Empty;
        [XmlElement("IssueDate")]
        public string IssueDate
        {
            get { return _dateIssue; }
            set { _dateIssue = value; }
        }

        private string _firstCouponDate = string.Empty;
        [XmlElement("FirstCouponDate")]
        public string FirstCouponDate
        {
            get { return _firstCouponDate; }
            set { _firstCouponDate = value; }
        }
        #endregion


        //[RG : 07312012 Rerate date = one or two business days adjusted before maturity date]

        private string _rerateDateBusDayAdj1 = string.Empty;
        [XmlElement("RerateDateBusDayAdjusted1")]
        public string RerateDateBusDayAdjusted1
        {
            get { return _rerateDateBusDayAdj1; }
            set { _rerateDateBusDayAdj1 = value; }
        }

        private string _rerateDateBusDayAdj2 = string.Empty;
        [XmlElement("RerateDateBusDayAdjusted2")]
        public string RerateDateBusDayAdjusted2
        {
            get { return _rerateDateBusDayAdj2; }
            set { _rerateDateBusDayAdj2 = value; }
        }


        //Added7 columns for Swap Handling: [RG 07252012] 
        #region Swap Parameters

        private int _dayCount;
        [XmlElement("DayCount")]
        public int DayCount
        {
            get { return _dayCount; }
            set { _dayCount = value; }
        }

        private double _benchMarkRate = 0;
        [XmlElement("BenchMarkRate")]
        public double BenchMarkRate
        {
            get { return _benchMarkRate; }
            set { _benchMarkRate = value; }
        }

        private double _differential = 0;
        [XmlElement("Differential")]
        public double Differential
        {
            get { return _differential; }
            set { _differential = value; }
        }

        private string _firstResetDate = string.Empty;
        [XmlElement("FirstResetDate")]
        public string FirstResetDate
        {
            get { return _firstResetDate; }
            set { _firstResetDate = value; }
        }

        private string _swapDescription = string.Empty;
        [XmlElement("SwapDescription")]
        public string SwapDescription
        {
            get { return _swapDescription; }
            set { _swapDescription = value; }
        }

        private bool _isSwapped = false;
        [XmlElement("IsSwapped")]
        public bool IsSwapped
        {
            get { return _isSwapped; }
            set { _isSwapped = value; }
        }




        #endregion

        private string _country;
        [XmlElement("Country")]
        public string CountryName
        {
            get { return _country; }
            set { _country = value; }
        }

        //FX Rate at Taxlot Level
        private double _fxRate_Taxlot = 0;
        [XmlElement("FXRate_Taxlot")]
        public double FXRate_Taxlot
        {
            get { return _fxRate_Taxlot; }
            set { _fxRate_Taxlot = value; }
        }

        private string _fxConversionMethodOperator_Taxlot = string.Empty;
        [XmlElement("FXConversionMethodOperator_Taxlot")]
        public string FXConversionMethodOperator_Taxlot
        {
            get { return _fxConversionMethodOperator_Taxlot; }
            set { _fxConversionMethodOperator_Taxlot = value; }
        }

        private string _lotId = string.Empty;
        [XmlElement("LotId")]
        public string LotId
        {
            get { return _lotId; }
            set { _lotId = value; }
        }

        private string _externalTransId = string.Empty;
        [XmlElement("ExternalTransId")]
        public string ExternalTransId
        {
            get { return _externalTransId; }
            set { _externalTransId = value; }
        }





        private string _tradeAttribute1 = string.Empty;
        [XmlElement("TradeAttribute1")]
        public string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = value; }
        }

        private string _tradeAttribute2 = string.Empty;
        [XmlElement("TradeAttribute2")]
        public string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = value; }
        }

        private string _tradeAttribute3 = string.Empty;
        [XmlElement("TradeAttribute3")]
        public string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = value; }
        }


        private string _tradeAttribute4 = string.Empty;
        [XmlElement("TradeAttribute4")]
        public string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = value; }
        }

        private string _tradeAttribute5 = string.Empty;
        [XmlElement("TradeAttribute5")]
        public string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = value; }
        }

        private string _tradeAttribute6 = string.Empty;
        [XmlElement("TradeAttribute6")]
        public virtual string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = value; }
        }


        private string _UDAAssetName = string.Empty;
        [XmlElement("UDAAssetName")]
        public virtual string UDAAssetName
        {
            get { return _UDAAssetName; }
            set { _UDAAssetName = value; }
        }

        private string _UDASecurityTypeName = string.Empty;
        [XmlElement("UDASecurityTypeName")]
        public virtual string UDASecurityTypeName
        {
            get { return _UDASecurityTypeName; }
            set { _UDASecurityTypeName = value; }
        }

        private string _UDASectorName = string.Empty;
        [XmlElement("UDASectorName")]
        public virtual string UDASectorName
        {
            get { return _UDASectorName; }
            set { _UDASectorName = value; }
        }

        private string _UDASubSectorName = string.Empty;
        [XmlElement("UDASubSectorName")]
        public virtual string UDASubSectorName
        {
            get { return _UDASubSectorName; }
            set { _UDASubSectorName = value; }
        }

        private string _UDACountryName = string.Empty;
        [XmlElement("UDACountryName")]
        public virtual string UDACountryName
        {
            get { return _UDACountryName; }
            set { _UDACountryName = value; }
        }

        private string _Description = string.Empty;
        [XmlElement("Description")]
        public virtual string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private string _DeliveryDate = string.Empty;
        [XmlElement("DeliveryDate")]
        public virtual string DeliveryDate
        {
            get { return _DeliveryDate; }
            set { _DeliveryDate = value; }
        }

        private string _transactionType = string.Empty;
        [XmlElement("TransactionType")]
        public virtual string TransactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }
        [XmlElement("OptionPremiumAdjustment")]
        private double _optionPremiumAdjustment;
        public double OptionPremiumAdjustment
        {
            get { return _optionPremiumAdjustment; }
            set { _optionPremiumAdjustment = value; }
        }


        private ChangeType _changeType;
        [XmlElement("ChangeType")]
        public ChangeType ChangeType
        {
            get { return _changeType; }
            set { _changeType = value; }
        }

        private string _analyst = string.Empty;
        [XmlElement("Analyst")]
        public string Analyst
        {
            get { return _analyst; }
            set { _analyst = value; }
        }

        private string _countryOfRisk = string.Empty;
        [XmlElement("CountryOfRisk")]
        public string CountryOfRisk
        {
            get { return _countryOfRisk; }
            set { _countryOfRisk = value; }
        }

        private string _issuer = string.Empty;
        [XmlElement("Issuer")]
        public string Issuer
        {
            get { return _issuer; }
            set { _issuer = value; }
        }

        private string _liquidTag = string.Empty;
        [XmlElement("LiquidTag")]
        public string LiquidTag
        {
            get { return _liquidTag; }
            set { _liquidTag = value; }
        }

        private string _marketCap = string.Empty;
        [XmlElement("MarketCap")]
        public string MarketCap
        {
            get { return _marketCap; }
            set { _marketCap = value; }
        }

        private string _region = string.Empty;
        [XmlElement("Region")]
        public string Region
        {
            get { return _region; }
            set { _region = value; }
        }

        private string _riskCurrency = string.Empty;
        [XmlElement("RiskCurrency")]
        public string RiskCurrency
        {
            get { return _riskCurrency; }
            set { _riskCurrency = value; }
        }

        private string _uCITSEligibleTag = string.Empty;
        [XmlElement("UCITSEligibleTag")]
        public string UCITSEligibleTag
        {
            get { return _uCITSEligibleTag; }
            set { _uCITSEligibleTag = value; }
        }

        private string _customUDA1 = string.Empty;
        [XmlElement("CustomUDA1")]
        public string CustomUDA1
        {
            get { return _customUDA1; }
            set { _customUDA1 = value; }
        }

        private string _customUDA2 = string.Empty;
        [XmlElement("CustomUDA2")]
        public string CustomUDA2
        {
            get { return _customUDA2; }
            set { _customUDA2 = value; }
        }

        private string _customUDA3 = string.Empty;
        [XmlElement("CustomUDA3")]
        public string CustomUDA3
        {
            get { return _customUDA3; }
            set { _customUDA3 = value; }
        }

        private string _customUDA4 = string.Empty;
        [XmlElement("CustomUDA4")]
        public string CustomUDA4
        {
            get { return _customUDA4; }
            set { _customUDA4 = value; }
        }

        private string _customUDA5 = string.Empty;
        [XmlElement("CustomUDA5")]
        public string CustomUDA5
        {
            get { return _customUDA5; }
            set { _customUDA5 = value; }
        }

        private string _customUDA6 = string.Empty;
        [XmlElement("CustomUDA6")]
        public string CustomUDA6
        {
            get { return _customUDA6; }
            set { _customUDA6 = value; }
        }

        private string _customUDA7 = string.Empty;
        [XmlElement("CustomUDA7")]
        public string CustomUDA7
        {
            get { return _customUDA7; }
            set { _customUDA7 = value; }
        }

        #endregion Properties


    }
}
