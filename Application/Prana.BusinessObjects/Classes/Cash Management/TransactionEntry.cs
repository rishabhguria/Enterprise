using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class TransactionEntry : IKeyable, IFilterable, INotifyPropertyChangedCustom, IDataErrorInfo
    {
        #region Private Fields
        private Dictionary<string, string> _errors = new Dictionary<string, string>();
        private string _taxlotID = string.Empty;
        private int _AccountID;
        private string _AccountName = string.Empty;
        private int _SubAcID;
        private string _SubAcName = string.Empty;
        private int _CurrencyID;
        private int _baseCurrencyID;
        private string _currencyName;
        private string _baseCurrencyName;
        #endregion

        private DateTime _transactionDate = DateTime.Now;
        public virtual DateTime TransactionDate
        {
            get { return _transactionDate; }
            set { _transactionDate = value; }
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

        public TransactionEntry()
        {
        }

        private decimal _dr;
        public virtual decimal DR
        {
            get { return _dr; }
            set { _dr = value; }
        }

        private decimal _cr;
        public virtual decimal CR
        {
            get { return _cr; }
            set { _cr = value; }
        }

        [Browsable(false)]
        public virtual Dictionary<string, string> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        private double _fxRate;
        public virtual double FxRate
        {
            get { return _fxRate; }
            set { _fxRate = value; }
        }

        private string _FXConversionMethodOperator = string.Empty;
        public virtual string FXConversionMethodOperator
        {
            get { return _FXConversionMethodOperator; }
            set { _FXConversionMethodOperator = value; }
        }

        private AccountSide _entryAccountSide = AccountSide.DR;
        public virtual AccountSide EntryAccountSide
        {
            get { return _entryAccountSide; }
            set { _entryAccountSide = value; }
        }

        private string _transactionID;
        public virtual string TransactionID
        {
            get { return _transactionID; }
            set { _transactionID = value; }
        }

        private CashTransactionType _transactionSource;
        public virtual CashTransactionType TransactionSource
        {
            get { return _transactionSource; }
            set { _transactionSource = value; }
        }

        public virtual bool IsEqual(TransactionEntry itemToComare)
        {
            if (this._cr == itemToComare.CR && this._CurrencyID == itemToComare.CurrencyID
                && this._description == itemToComare.Description && this._dr == itemToComare.DR && this._AccountID == itemToComare.AccountID
                && this._SubAcID == itemToComare.SubAcID && this._symbol == itemToComare.Symbol && this._taxlotID == itemToComare.TaxLotId
                && this._transactionDate == itemToComare.TransactionDate && this._transactionSource == itemToComare.TransactionSource)
                return true;
            else
                return false;
        }

        #region GetSetSection

        private string _transactionEntryID = string.Empty;
        [Browsable(false)]
        public virtual string TransactionEntryID
        {
            get { return _transactionEntryID; }
            set { _transactionEntryID = value; }
        }

        public virtual string TaxLotId
        {
            get { return _taxlotID; }
            set { _taxlotID = value; }
        }

        public virtual int AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }

        public virtual string AccountName
        {
            get { return _AccountName; }
            set { _AccountName = value; }
        }

        public virtual int SubAcID
        {
            get { return _SubAcID; }
            set { _SubAcID = value; }
        }

        public virtual string SubAcName
        {
            get { return _SubAcName; }
            set { _SubAcName = value; }
        }

        public virtual int CurrencyID
        {
            get { return _CurrencyID; }
            set { _CurrencyID = value; }
        }

        public virtual int BaseCurrencyID
        {
            get { return _baseCurrencyID; }
            set { _baseCurrencyID = value; }
        }

        public virtual string CurrencyName
        {
            get { return _currencyName; }
            set { _currencyName = value; }
        }

        public virtual string BaseCurrencyName
        {
            get { return _baseCurrencyName; }
            set { _baseCurrencyName = value; }
        }

        private ApplicationConstants.TaxLotState _TaxLotState;
        [Browsable(false)]
        public virtual ApplicationConstants.TaxLotState TaxLotState
        {
            get { return _TaxLotState; }
            set { _TaxLotState = value; }
        }

        private string _symbol = string.Empty;
        public virtual string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _description = string.Empty;
        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private int _transactionNumber = 1;
        [Browsable(false)]
        public virtual int TransactionNumber
        {
            get { return _transactionNumber; }
            set { _transactionNumber = value; }
        }

        private string _activityId_FK;
        [Browsable(false)]
        public virtual string ActivityId_FK
        {
            get { return _activityId_FK; }
            set { _activityId_FK = value; }
        }

        public virtual string GetUniqueSubAcBalKey()
        {
            return AccountID.ToString() + "_" + CurrencyID.ToString() + "_" + SubAcID.ToString();
        }
        #endregion

        public virtual TransactionEntry Clone()
        {
            return Prana.Global.Utilities.DeepCopyHelper.Clone(this);
        }

        #region IKeyable Members

        public virtual string GetKey()
        {
            return _transactionEntryID;
        }

        public virtual void Update(IKeyable item)
        {
            TransactionEntry newdata = item as TransactionEntry;
            string uniqueID = this._transactionEntryID;
            this.CR = newdata.CR;
            this.DR = newdata.DR;
            this._transactionEntryID = uniqueID;
        }

        public virtual void properityChanged(string ColumnName, string Value)
        {
            switch (ColumnName)
            {
                case "SubAcName":
                    if (string.IsNullOrEmpty(Value))
                    {
                        if (!_errors.ContainsKey(ColumnName))
                            _errors.Add("SubAcName", "Sub account name required.");
                    }
                    else
                    {
                        if (_errors.ContainsKey(ColumnName))
                            _errors.Remove(ColumnName);
                    }
                    break;

                case "AccountName":
                    if (string.IsNullOrEmpty(Value))
                    {
                        if (!_errors.ContainsKey(ColumnName))
                            _errors.Add("AccountName", "Account Required.");
                    }
                    else
                    {
                        if (_errors.ContainsKey(ColumnName))
                            _errors.Remove(ColumnName);
                    }
                    break;

                case "CurrencyName":
                    if (string.IsNullOrEmpty(Value))
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

                case "TradedDate":
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
        #endregion

        #region IFilterable Members
        public virtual DateTime GetDate()
        {
            return _transactionDate;
        }

        public virtual DateTime GetDateModified()
        {
            return TransactionDate;
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

        #region Column Headers Actual Names

        public const string PROPERTY_CashID = "CashID";
        public const string PROPERTY_TaxLotID = "TaxLot";
        public const string PROPERTY_AccountID = "AccountID";
        public const string PROPERTY_AccountName = "AccountName";
        public const string PROPERTY_Symbol = "Symbol";
        public const string PROPERTY_AcName = "AcName";
        public const string PROPERTY_SubAcName = "SubAcName";
        public const string PROPERTY_Amount = "Amount";
        public const string PROPERTY_TradedDate = "TradedDate";
        public const string PROPERTY_PayOutDate = "PayOutDate";
        public const string PROPERTY_TaxLotState = "TaxLotState";
        public const string PROPERTY_Description = "Description";
        public const string PROPERTY_IsAutomatic = "IsAutomatic";

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

        private bool _isDrTotalEqualsToCrTotal = true;
        [Browsable(false)]
        public virtual bool IsDrTotalEqualsToCrTotal
        {
            get { return _isDrTotalEqualsToCrTotal; }
            set { _isDrTotalEqualsToCrTotal = value; }
        }

        private bool _isDrBaseEqualsToCrBase = true;
        [Browsable(false)]
        public virtual bool IsDrBaseEqualsToCrBase
        {
            get { return _isDrBaseEqualsToCrBase; }
            set { _isDrBaseEqualsToCrBase = value; }
        }

        private ActivitySource _activitySource;
        [Browsable(false)]
        public virtual ActivitySource ActivitySource
        {
            get { return _activitySource; }
            set { _activitySource = value; }
        }

        public virtual decimal DrBase
        {
            get
            {
                if (_fxRate != 0)
                    return ((_FXConversionMethodOperator.Equals(Operator.M.ToString())) ? (_dr * (decimal)_fxRate) : (_dr / (decimal)_fxRate));
                else
                    return 0;
            }
        }

        public virtual decimal CrBase
        {
            get
            {
                if (_fxRate != 0)
                    return ((_FXConversionMethodOperator.Equals(Operator.M.ToString())) ? (_cr * (decimal)_fxRate) : (_cr / (decimal)_fxRate));
                else
                    return 0;
            }
        }

        StringBuilder _rowLevelError = new StringBuilder();
        [Browsable(false)]
        public virtual string Error
        {
            get
            {
                if (_errors.Count > 0 || !_isDrTotalEqualsToCrTotal || !_isDrBaseEqualsToCrBase)
                {
                    _rowLevelError = new StringBuilder();
                    foreach (string value in _errors.Values)
                        _rowLevelError.AppendLine(value);
                    if (!_isDrTotalEqualsToCrTotal)
                        _rowLevelError.AppendLine("DR total is not equal to CR total");
                    if (!_isDrBaseEqualsToCrBase)
                        _rowLevelError.AppendLine("DR base is not equal to CR base");
                    return _rowLevelError.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        [Browsable(false)]
        public virtual string this[string columnName]
        {
            get
            {
                if (_errors.ContainsKey(columnName))
                    return _errors[columnName];
                return string.Empty;
            }
        }
    }
}
