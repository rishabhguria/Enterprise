using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.ComponentModel;
using System.Data;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class AllocationOrder : PranaBasicMessage, INotifyPropertyChanged
    {
        #region private Variable

        private string _clOrderID = string.Empty;
        private string _parentClOrderID = string.Empty;
        private string _listID = string.Empty;
        //private bool _updated                   =false;
        //private double _allocatedQty = 0;
        private string _groupID = string.Empty;
        private string _multiTradeName = string.Empty;
        private ImportFileLog _importFileLog = null;


        //private string _openClose = string.Empty;
        //private DateTime _auecLocalDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;
        //private DateTime _settlementDate = DateTimeConstants.MinValue;
        //private DateTime _expirationDate = DateTimeConstants.MinValue;
        //private string _transactionTime = string.Empty;

        #endregion

        public AllocationOrder()
        {

        }

        public AllocationOrder(DataRow dr)
        {
            _groupID = dr["GroupID"].ToString();
            _parentClOrderID = dr["ParentClOrderID"].ToString();
            _clOrderID = dr["CLOrderID"].ToString();
            if (!string.IsNullOrEmpty(dr["OCumQty"].ToString()))
                _cumQty = Convert.ToDouble(dr["OCumQty"]);
            if (!string.IsNullOrEmpty(dr["OCumQty"].ToString()))
                _originalCumQty = Convert.ToDouble(dr["OCumQty"]);
            if (!string.IsNullOrEmpty(dr["OQuantity"].ToString()))
                _quantity = Convert.ToDouble(dr["OQuantity"]);
            if (!string.IsNullOrEmpty(dr["OAvgPrice"].ToString()))
                _avgPrice = Convert.ToDouble(dr["OAvgPrice"]);
            if (!string.IsNullOrEmpty(dr["NirvanaMsgType"].ToString()))
                _pranaMsgType = (OrderFields.PranaMsgTypes)(Convert.ToInt32(dr["NirvanaMsgType"]));
            if (!string.IsNullOrEmpty(dr["OFXRate"].ToString()))
                _avgFXRateForTrade = Convert.ToDouble(dr["OFXRate"]);
            _AccountID = string.IsNullOrEmpty(dr["OFundID"].ToString()) ? int.MinValue : Convert.ToInt32(dr["OFundID"]);
            _StrategyID = string.IsNullOrEmpty(dr["OStrategyID"].ToString()) ? int.MinValue : Convert.ToInt32(dr["OStrategyID"]);
            if (!string.IsNullOrEmpty(dr["OOriginalPurchaseDate"].ToString()))
                _originalPurchaseDate = Convert.ToDateTime(dr["OOriginalPurchaseDate"]);
            if (!string.IsNullOrEmpty(dr["OProcessDate"].ToString()))
                _processDate = Convert.ToDateTime(dr["OProcessDate"]);
            if (!string.IsNullOrEmpty(dr["OAUECLocalDate"].ToString()))
                _aUECLocalDate = Convert.ToDateTime(dr["OAUECLocalDate"]);
            if (!string.IsNullOrEmpty(dr["OIsModified"].ToString()))
                _isModified = Convert.ToBoolean(dr["OIsModified"]);
            if (!string.IsNullOrEmpty(dr["OOrderSideTagValue"].ToString()))
                _orderSideTagValue = dr["OOrderSideTagValue"].ToString();
            if (!string.IsNullOrEmpty(dr["OVenueID"].ToString()))
                _venueID = Convert.ToInt32(dr["OVenueID"]);
            if (!string.IsNullOrEmpty(dr["OCounterPartyID"].ToString()))
                _counterPartyID = Convert.ToInt32(dr["OCounterPartyID"]);
            if (!string.IsNullOrEmpty(dr["OTradingAccountID"].ToString()))
                _tradingAccountID = Convert.ToInt32(dr["OTradingAccountID"]);
            if (!string.IsNullOrEmpty(dr["OSettlementDate"].ToString()))
                _settlementDate = Convert.ToDateTime(dr["OSettlementDate"]);
            if (!string.IsNullOrEmpty(dr["OFXConversionMethodOperator"].ToString()))
                _FXConversionMethodOperator = dr["OFXConversionMethodOperator"].ToString().Trim();
            _multiTradeName = string.IsNullOrEmpty(dr["MultiTradeName"].ToString()) ? string.Empty : dr["MultiTradeName"].ToString();
            _importFileLog = new ImportFileLog(dr);
            if (!string.IsNullOrEmpty(dr["OuserID"].ToString()))
                _userID = Convert.ToInt32(dr["OuserID"]);
            if (!string.IsNullOrEmpty(dr["OTradeAttribute1"].ToString()))
                _tradeAttribute1 = dr["OTradeAttribute1"].ToString();
            if (!string.IsNullOrEmpty(dr["OTradeAttribute2"].ToString()))
                _tradeAttribute2 = dr["OTradeAttribute2"].ToString();
            if (!string.IsNullOrEmpty(dr["OTradeAttribute3"].ToString()))
                _tradeAttribute3 = dr["OTradeAttribute3"].ToString();
            if (!string.IsNullOrEmpty(dr["OTradeAttribute4"].ToString()))
                _tradeAttribute4 = dr["OTradeAttribute4"].ToString();
            if (!string.IsNullOrEmpty(dr["OTradeAttribute5"].ToString()))
                _tradeAttribute5 = dr["OTradeAttribute5"].ToString();
            if (!string.IsNullOrEmpty(dr["OTradeAttribute6"].ToString()))
                _tradeAttribute6 = dr["OTradeAttribute6"].ToString();
            _internalComments = string.IsNullOrEmpty(dr["OInternalComments"].ToString()) ? string.Empty : dr["OInternalComments"].ToString();
            _settlementCurrencyID = string.IsNullOrEmpty(dr["OSettlCurrency"].ToString()) ? int.MinValue : Convert.ToInt32(dr["OSettlCurrency"]);
            if (!string.IsNullOrEmpty(dr["OTransactionSource"].ToString()))
            {
                _transactionSource = (TransactionSource)Enum.Parse(typeof(TransactionSource), dr["OTransactionSource"].ToString());
                _transactionSourceTag = (int)_transactionSource;
            }
            if (!string.IsNullOrEmpty(dr["text"].ToString()))
                _internalComments = dr["text"].ToString();
            if (!string.IsNullOrEmpty(dr["OOriginalAllocationPreferenceID"].ToString()))
                _originalAllocationPreferenceID = Convert.ToInt32(dr["OOriginalAllocationPreferenceID"]);

            if (!string.IsNullOrEmpty(dr["BorrowerID"].ToString()))
                _borrowerID = dr["BorrowerID"].ToString();
            if (!string.IsNullOrEmpty(dr["BorrowBroker"].ToString()))
                _borrowerBroker = dr["BorrowBroker"].ToString();
            if (!string.IsNullOrEmpty(dr["ShortRebate"].ToString()))
                _shortRebate = Convert.ToDouble(dr["ShortRebate"]);
			if (!string.IsNullOrEmpty(dr["OAdditionalTradeAttributes"].ToString()))
            {
                base.SetTradeAttribute(dr["OAdditionalTradeAttributes"].ToString());
            }
        }

        #region Properties
        public virtual ImportFileLog ImportFileLogObj
        {
            get { return _importFileLog; }
            set { _importFileLog = value; }
        }

        public virtual string ClOrderID
        {
            get { return _clOrderID; }
            set { _clOrderID = value; }
        }

        public virtual string ParentClOrderID
        {
            get { return _parentClOrderID; }
            set { _parentClOrderID = value; }
        }

        public virtual string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public virtual string ListID
        {
            get { return _listID; }
            set { _listID = value; }
        }

        private OrderFields.PranaMsgTypes _pranaMsgType;
        [XmlIgnore]
        public virtual OrderFields.PranaMsgTypes PranaMsgType
        {
            get { return _pranaMsgType; }
            set
            {
                _pranaMsgType = value;
                _intPranaMsgType = (int)_pranaMsgType;
            }
        }

        private int _intPranaMsgType = int.MinValue;
        [Browsable(false)]
        public virtual int IntPranaMsgType
        {
            get { return _intPranaMsgType; }
            set
            {
                _intPranaMsgType = value;
                _pranaMsgType = (OrderFields.PranaMsgTypes)_intPranaMsgType;
            }
        }

        public virtual string MultiTradeName
        {
            get { return _multiTradeName; }
            set { _multiTradeName = value; }
        }
        #endregion

        private int _AccountID;
        public virtual int AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }

        private int _StrategyID;
        public virtual int StrategyID
        {
            get { return _StrategyID; }
            set { _StrategyID = value; }
        }

        #region INotifyPropertyChangedCustom Members

        //Implemented INotifyPropertyChangedCustom to raise change event while working with WPF, PRANA-15997
        [field: NonSerialized]
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public virtual void PropertyHasChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
            }
        }

        #endregion INotifyPropertyChangedCustom Members
    }
}