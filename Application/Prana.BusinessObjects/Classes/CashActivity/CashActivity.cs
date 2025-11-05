using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class CashActivity : IKeyable, IFilterable, INotifyPropertyChangedCustom, IDataErrorInfo
    {
        #region property

        private string _activityId;
        [Browsable(false)]
        public virtual string ActivityId
        {
            get { return _activityId; }
            set { _activityId = value; }
        }

        private int _activityTypeId;
        [Browsable(false)]
        public virtual int ActivityTypeId
        {
            get { return _activityTypeId; }
            set { _activityTypeId = value; }
        }

        private string _activityType = "Select";
        public virtual string ActivityType
        {
            get { return _activityType; }
            set { _activityType = value; }
        }

        private string _fKID = string.Empty;
        [Browsable(false)]
        public virtual string FKID
        {
            get { return _fKID; }
            set { _fKID = value; }
        }

        private BalanceType _balanceType = BalanceType.Cash;
        public virtual BalanceType BalanceType
        {
            get { return _balanceType; }
            set { _balanceType = value; }
        }

        private string _symbol;
        public virtual string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private int _accountID = 0;
        [Browsable(false)]
        public virtual int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private string _accountName = "Select";
        public virtual string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }

        private DateTime _date = DateTime.Now;
        public virtual DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private Nullable<DateTime> _settlementDate = DateTime.Now;
        public virtual Nullable<DateTime> SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }

        private int _currencyID = 0;
        [Browsable(false)]
        public virtual int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        private int _sideMultiplier = 1;
        [Browsable(false)]
        public virtual int SideMultiplier
        {
            get { return _sideMultiplier; }
            set { _sideMultiplier = value; }
        }

        private string _currencyName = "Select";
        public virtual string CurrencyName
        {
            get { return _currencyName; }
            set { _currencyName = value; }
        }

        private int _leadCurrencyID;
        [Browsable(false)]
        public virtual int LeadCurrencyID
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }

        private string _leadCurrencyName;
        public virtual string LeadCurrencyName
        {
            get { return _leadCurrencyName; }
            set { _leadCurrencyName = value; }
        }

        private int _vsCurrencyID;
        [Browsable(false)]
        public virtual int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        private string _vsCurrencyName;
        public virtual string VsCurrencyName
        {
            get { return _vsCurrencyName; }
            set { _vsCurrencyName = value; }
        }

        private decimal _closedQty;
        public virtual decimal ClosedQty
        {
            get { return _closedQty; }
            set { _closedQty = value; }
        }

        #region taxlot id commented
        //since Taxlotid and cashTransactionId both make fkid for T_AllActivity and they can be separated using isTradeActivity field.

        //private string _taxlotID;

        //public virtual string TaxlotID
        //{
        //    get { return _taxlotID; }
        //    set { _taxlotID = value; }
        //}
        #endregion

        private decimal _amount;
        public virtual decimal Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        /// <summary>
        /// PnL will store total pnl of the activity during settlement
        /// </summary>
        private decimal _pnl = 0;
        public virtual decimal PnL
        {
            get { return _pnl; }
            set { _pnl = value; }
        }

        /// <summary>
        /// FXPnL will store pnl generated by the difference of fxrate and forward rate  
        /// </summary>
 		private decimal _fxPnl = 0;
        public virtual decimal FXPnL
        {
            get { return _fxPnl; }
            set { _fxPnl = value; }
        }

        private decimal _commission;
        public virtual decimal Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        private decimal _softCommission;
        public virtual decimal SoftCommission
        {
            get { return _softCommission; }
            set { _softCommission = value; }
        }

        private decimal _otherBrokerFees;
        public virtual decimal OtherBrokerFees
        {
            get { return _otherBrokerFees; }
            set { _otherBrokerFees = value; }
        }

        private decimal _clearingBrokerFee;
        public virtual decimal ClearingBrokerFee
        {
            get { return _clearingBrokerFee; }
            set { _clearingBrokerFee = value; }
        }

        private decimal _stampDuty;
        public virtual decimal StampDuty
        {
            get { return _stampDuty; }
            set { _stampDuty = value; }
        }

        private decimal _transactionLevy;
        public virtual decimal TransactionLevy
        {
            get { return _transactionLevy; }
            set { _transactionLevy = value; }
        }

        private decimal _clearingFee;
        public virtual decimal ClearingFee
        {
            get { return _clearingFee; }
            set { _clearingFee = value; }
        }

        private decimal _taxOnCommissions;
        public virtual decimal TaxOnCommissions
        {
            get { return _taxOnCommissions; }
            set { _taxOnCommissions = value; }
        }

        private decimal _miscFees;
        public virtual decimal MiscFees
        {
            get { return _miscFees; }
            set { _miscFees = value; }
        }

        private decimal _secFee;
        public virtual decimal SecFee
        {
            get { return _secFee; }
            set { _secFee = value; }
        }

        private decimal _occFee;
        public virtual decimal OccFee
        {
            get { return _occFee; }
            set { _occFee = value; }
        }

        private decimal _orfFee;
        public virtual decimal OrfFee
        {
            get { return _orfFee; }
            set { _orfFee = value; }
        }

        public virtual decimal TotalCommission
        {
            get { return _commission + _softCommission + _otherBrokerFees + _clearingBrokerFee + _stampDuty + _transactionLevy + _taxOnCommissions + _miscFees + _clearingFee + _secFee + _occFee + _orfFee + _optionPremiumAdjustment; }
        }

        private double _fXRate;
        public virtual double FXRate
        {
            get { return _fXRate; }
            set { _fXRate = value; }
        }

        //PRANA-9035
        private double _avgPrice;
        [Browsable(false)]
        public virtual double avgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }

        //PRANA-9777
        private DateTime _modifyDate = DateTime.Now;
        public virtual DateTime ModifyDate
        {
            get { return _modifyDate; }
            set { _modifyDate = value; }
        }

        //PRANA-9777
        private DateTime _entryDate = DateTime.Now;
        public virtual DateTime EntryDate
        {
            get { return _entryDate; }
            set { _entryDate = value; }
        }

        //PRANA-9776
        private int _userId;
        public virtual int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        //PRANA-9776
        private string _userName;
        public virtual string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _FXConversionMethodOperator = string.Empty;
        public virtual string FXConversionMethodOperator
        {
            get { return _FXConversionMethodOperator; }
            set { _FXConversionMethodOperator = value; }
        }

        private CashTransactionType _transactionSource = CashTransactionType.Trading;
        public virtual CashTransactionType TransactionSource
        {
            get { return _transactionSource; }
            set { _transactionSource = value; }
        }

        private int _activityNumber = 1;
        [Browsable(false)]
        public virtual int ActivityNumber
        {
            get { return _activityNumber; }
            set { _activityNumber = value; }
        }

        private string _description;
        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private ApplicationConstants.TaxLotState _activityState;
        [Browsable(false)]
        public virtual ApplicationConstants.TaxLotState ActivityState
        {
            get { return _activityState; }
            set { _activityState = value; }
        }

        private string _uniqueKey;
        [Browsable(false)]
        public virtual string UniqueKey
        {
            get { return _uniqueKey; }
            set { _uniqueKey = value; }
        }

        private string _subActivity = string.Empty;
        [Browsable(false)]
        public virtual string SubActivity
        {
            get { return _subActivity; }
            set { _subActivity = value; }
        }

        //Narendra Kumar Jangir, Nov 07 2013
        //This column is added to differentiate trade cash and dividend.
        //value 0 = cash activity, 1=trade activity, 2=dividend
        //Modified By: Ishan Gandhi(06/10/2013)
        //Removed the [Browsable(false)] so that the column is visible on grid
        //Jira Link: Activity Source Should be included in Activity Tab(http://jira.nirvanasolutions.com:8080/browse/PRANA-5050)
        private ActivitySource _activitySource;
        public virtual ActivitySource ActivitySource
        {
            get { return _activitySource; }
            set { _activitySource = value; }
        }

        public virtual bool IsEqual(CashActivity itemToComare)
        {
            if (this._activityTypeId == itemToComare.ActivityTypeId && this._currencyID == itemToComare.CurrencyID
                && this._description == itemToComare.Description && this._accountID == itemToComare.AccountID
                && this._symbol == itemToComare.Symbol && this._transactionSource == itemToComare.TransactionSource && this.Amount == itemToComare.Amount)
                return true;
            else
                return false;
        }

        private int _subAccountID;
        [Browsable(false)]
        public virtual int SubAccountID
        {
            get { return _subAccountID; }
            set { _subAccountID = value; }
        }

        //Bharat Jangir (08 Nov 2014)
        //Adding Taxlot Id in Cash Activity for handing published data on PM
        //Jira Link http://jira.nirvanasolutions.com:8080/browse/PRANA-5034
        //Jira Link http://jira.nirvanasolutions.com:8080/browse/PRANA-4805
        private string _taxlotId = string.Empty;
        [Browsable(false)]
        public virtual string TaxLotId
        {
            get { return _taxlotId; }
            set { _taxlotId = value; }
        }

        //PRANA-12569
        private decimal _optionPremiumAdjustment;
        public virtual decimal OptionPremiumAdjustment
        {
            get { return _optionPremiumAdjustment; }
            set { _optionPremiumAdjustment = value; }
        }
        #endregion

        #region IKeyable Members

        public virtual string GetKey()
        {
            if (string.IsNullOrEmpty(_fKID))
                _fKID = uIDGenerator.GenerateID();
            return _fKID.ToString() + _date.ToShortDateString() + ((int)_transactionSource).ToString() + _activityNumber.ToString();
        }

        public virtual void Update(IKeyable item)
        {
            //this = item as CashActivity;
        }

        #endregion

        #region IFilterable Members

        public virtual DateTime GetDate()
        {
            return Date;
        }

        public virtual DateTime GetDateModified()
        {
            return Date;
        }

        public virtual string GetSymbol()
        {
            return Symbol;
        }

        public virtual int GetAccountID()
        {
            return AccountID;
        }
        #endregion

        #region INotifyPropertyChangedCustom Members

        [field: NonSerialized]
        public virtual event PropertyChangedEventHandler PropertyChanged;

        [Browsable(false)]
        public virtual void PropertyHasChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, null);
            }
        }
        #endregion

        #region IDataErrorInfo Members

        public virtual void properityChanged(string ColumnName, string Value)
        {
            switch (ColumnName)
            {
                case "Activity":
                    if (string.IsNullOrEmpty(Value) || Value.Equals("Select"))
                    {
                        if (!_errors.ContainsKey(ColumnName))
                            _errors.Add("Activity", "Activity required.");
                    }
                    else
                    {
                        if (_errors.ContainsKey(ColumnName))
                            _errors.Remove(ColumnName);
                    }

                    break;

                case "FundName":
                    if (string.IsNullOrEmpty(Value) || Value.Equals("Select"))
                    {
                        if (!_errors.ContainsKey(ColumnName))
                            _errors.Add("FundName", "Account Required.");
                    }
                    else
                    {
                        if (_errors.ContainsKey(ColumnName))
                            _errors.Remove(ColumnName);
                    }
                    break;

                case "CurrencyName":
                    if (string.IsNullOrEmpty(Value) || Value.Equals("Select"))
                    {
                        if (!_errors.ContainsKey(ColumnName))
                            _errors.Add("CurrencyName", "Currency Required.");
                    }
                    else
                    {
                        if (_errors.ContainsKey(ColumnName))
                            _errors.Remove(ColumnName);
                    }
                    break;

                case "Symbol":
                    if (string.IsNullOrEmpty(Value) || string.IsNullOrEmpty(_symbol))
                    {
                        if (_errors.ContainsKey(ColumnName))
                            _errors.Remove(ColumnName);
                    }
                    else if (Value == "Symbol Validated !")
                    {

                        if (_errors.ContainsKey(ColumnName))
                            _errors.Remove(ColumnName);
                    }
                    else if (Value == "Symbol Not Validated !" || Value == "Symbol is in validation process !")
                    {
                        if (_errors.ContainsKey(ColumnName))
                            _errors[ColumnName] = Value;
                        else
                            _errors.Add(ColumnName, Value);
                    }
                    PropertyChanged(this, new PropertyChangedEventArgs("Symbol"));
                    break;

                case "Date":
                    if (string.IsNullOrEmpty(Value))
                    {
                        if (!_errors.ContainsKey(ColumnName))
                            _errors.Add("Date", "Date Required.");
                    }
                    else
                    {
                        if (_errors.ContainsKey(ColumnName))
                            _errors.Remove(ColumnName);
                    }
                    break;

                default:
                    break;
            }
        }

        private Dictionary<string, string> _errors = new Dictionary<string, string>();
        [Browsable(false)]
        public virtual Dictionary<string, string> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        StringBuilder _rowLevelError = new StringBuilder();
        [Browsable(false)]
        public virtual string Error
        {
            get
            {
                if (_errors.Count > 0)
                {
                    _rowLevelError = new StringBuilder();
                    foreach (string value in _errors.Values)
                        _rowLevelError.AppendLine(value);
                    return _rowLevelError.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public virtual string this[string columnName]
        {
            get
            {
                if (_errors.ContainsKey(columnName))
                    return _errors[columnName];
                return string.Empty;
            }
        }

        #endregion

        protected string _settlCurrency;
        public virtual string SettlCurrency
        {
            get { return _settlCurrency; }
            set { _settlCurrency = value; }
        }

        protected int _settlCurrencyID;
        public virtual int SettlCurrencyID
        {
            get { return _settlCurrencyID; }
            set { _settlCurrencyID = value; }
        }
    }
}
