using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Prana.BusinessObjects
{
    [KnownType(typeof(TransactionEntry))]
    [Serializable]
    public partial class Transaction : IKeyable, IFilterable, INotifyPropertyChangedCustom, IDataErrorInfo
    {
        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        private GenericBindingList<TransactionEntry> _transactionEntries;
        public GenericBindingList<TransactionEntry> TransactionEntries
        {
            get { return _transactionEntries; }
            set { _transactionEntries = value; }
        }

        /// <summary>
        /// Deleted Transaction Entries or with 0 value for updating last calculation date in T_LastCalculatedBalanceDate
        /// </summary>
        private List<TransactionEntry> _deletedTransactionEntries;
        public List<TransactionEntry> DeletedTransactionEntries
        {
            get { return _deletedTransactionEntries; }
            set { _deletedTransactionEntries = value; }
        }

        public Transaction(DateTime givenDate)
        {
            _date = givenDate;
            _transactionEntries = new GenericBindingList<TransactionEntry>();
            _deletedTransactionEntries = new List<TransactionEntry>();
        }

        public Transaction()
        {
            _transactionEntries = new GenericBindingList<TransactionEntry>();
        }

        [Browsable(false)]
        public string ActivityId_FK
        {
            get
            {
                if (_transactionEntries != null && _transactionEntries.Count > 0)
                    return _transactionEntries[0].ActivityId_FK;
                return string.Empty;
            }
        }

        [Browsable(false)]
        public int TransactionNumber
        {
            get
            {
                if (_transactionEntries != null && _transactionEntries.Count > 0)
                    return _transactionEntries[0].TransactionNumber;
                return 0;
            }
        }

        private string _transactionID = string.Empty;
        public string TransactionID
        {
            get { return _transactionID; }
            set { _transactionID = value; }
        }

        private DateTime _date = DateTime.Now.Date;
        public DateTime Date
        {
            get { return _date.Date; }
            set
            {
                _date = value.Date;
                if (value != DateTime.MinValue && _transactionEntries != null)
                    foreach (TransactionEntry trEntry in _transactionEntries)
                        trEntry.TransactionDate = _date;
            }
        }

        /// <summary>
        /// This method is used to identify that all transaction entries for a transaction are in single currency or have multiple currencies.
        /// true:- All transaction entries are in same currency
        /// false:- All transaction entries are not in same currency
        /// </summary>
        public bool isAllJournalEntriesInLocalCurrency
        {
            get
            {
                if (_transactionEntries != null && _transactionEntries.Count > 0)
                {
                    foreach (TransactionEntry trEntry in _transactionEntries)
                    {
                        if (trEntry.CurrencyID != _transactionEntries[0].CurrencyID)
                            return false;
                    }
                }
                return true;
            }
        }

        public string GetTaxlotID()
        {
            try
            {
                if (_transactionEntries.Count > 0)
                    return _transactionEntries[0].TaxLotId;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /*
        Narendra Kumar jangir 2013 Sept 21
        This method is used to identify the transaction source 
        Trading = 1,
        ManualEntry = 2,
        DailyCalculation = 3,
        CorpAction = 4,
        CashTransaction = 5,
        ImportedEditableData=6,
        Closing=7,
        Unwinding = 8
        */
        public int GetTransactionSource()
        {
            try
            {
                if (_transactionEntries.Count > 0)
                    return (int)_transactionEntries[0].TransactionSource;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        //Narendra Kumar Jangir 2013 Nov 2013
        //This method is used to identify that activity is cash=0, trade=1 or dividend=2
        public byte GetActivitySource()
        {
            try
            {
                if (_transactionEntries.Count > 0)
                    return (byte)_transactionEntries[0].ActivitySource;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return byte.MinValue;
        }

        public string GetTaxlotState()
        {
            try
            {
                if (_transactionEntries.Count > 0)
                    return _transactionEntries[0].TaxLotState.ToString();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public void Add(TransactionEntry item)
        {
            try
            {
                if (string.IsNullOrEmpty(this._transactionID) && !string.IsNullOrEmpty(item.TransactionID))
                    this._transactionID = item.TransactionID;
                if (!string.IsNullOrEmpty(this._transactionID))
                    item.TransactionID = _transactionID;
                //Setting default TransactionEntry date to TransactionDate 
                if (item.TransactionDate.Date == DateTime.Now.Date && this._date != DateTime.MinValue)
                    item.TransactionDate = _date.Date;
                _transactionEntries.Add(item);
                _drTotal += item.DR;
                _crTotal += item.CR;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Remove(TransactionEntry item)
        {
            try
            {
                if (_transactionEntries.Contains(item))
                {
                    _transactionEntries.Remove(item);
                    _drTotal -= item.DR;
                    _crTotal -= item.CR;
                    Validate();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Modify(TransactionEntry _item, decimal OriginalValue, string ColunmName)
        {
            if (ColunmName == "DR")
            {
                if (_item.DR > OriginalValue)
                    _drTotal += _item.DR - OriginalValue;
                else
                    _drTotal -= OriginalValue - _item.DR;
            }
            if (ColunmName == "CR")
            {
                if (_item.CR > OriginalValue)
                    _crTotal += _item.CR - OriginalValue;
                else
                    _crTotal -= OriginalValue - _item.CR;
            }
            Validate();
        }

        private decimal _drTotal;
        public decimal DRTotal
        {
            get { return _drTotal; }
            set { _drTotal = value; }
        }

        private decimal _crTotal;
        public decimal CRTotal
        {
            get { return _crTotal; }
            set { _crTotal = value; }
        }

        private decimal _drBaseTotal = 0;
        public decimal DRBaseTotal
        {
            get
            {
                _drBaseTotal = 0;
                if (_transactionEntries != null && _transactionEntries.Count > 0)
                {
                    foreach (TransactionEntry trEntry in _transactionEntries)
                    {
                        _drBaseTotal += trEntry.DrBase;
                    }
                }
                return _drBaseTotal;
            }
        }

        private decimal _crBaseTotal = 0;
        public decimal CRBaseTotal
        {
            get
            {
                _crBaseTotal = 0;
                if (_transactionEntries != null && _transactionEntries.Count > 0)
                {
                    foreach (TransactionEntry trEntry in _transactionEntries)
                    {
                        _crBaseTotal += trEntry.CrBase;
                    }
                }
                return _crBaseTotal;
            }
        }

        public void Validate()
        {
            foreach (TransactionEntry trEntry in _transactionEntries)
            {
                trEntry.IsDrTotalEqualsToCrTotal = true;
                trEntry.IsDrBaseEqualsToCrBase = true;
                if (isAllJournalEntriesInLocalCurrency)
                {
                    if (_drTotal != _crTotal)
                    {
                        trEntry.IsDrTotalEqualsToCrTotal = false;
                    }
                }
                else
                {
                    // In PRANA-33344, an improvement was raised to compare the DR and CR base upto two decimal places
                    // I have added hardcode value for decimal comparison to 2 because as per discussion with product team, no configuration is required for this
                    if (Math.Round(DRBaseTotal, 2) != Math.Round(CRBaseTotal, 2))
                    {
                        trEntry.IsDrBaseEqualsToCrBase = false;
                    }
                }
            }
        }

        public bool IsTradingTransaction()
        {
            try
            {
                if (GetTransactionSource() == (int)CashTransactionType.Trading)
                    return true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        public bool IsDividendTransaction()
        {
            try
            {
                if (_transactionEntries.Count > 0 && _transactionEntries[0].ActivitySource.Equals(ActivitySource.Dividend))
                    return true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        public bool IsDailyCalculationTransaction()
        {
            try
            {
                if (GetTransactionSource() == (int)CashTransactionType.DailyCalculation)
                    return true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        #region IKeyable Members

        public string GetKey()
        {
            if (ActivityId_FK != null)
                return ActivityId_FK + TransactionNumber.ToString();
            else
                return TransactionID;
        }

        public void Update(IKeyable item)
        {
            Transaction newdata = item as Transaction;
            this.TransactionEntries = newdata.TransactionEntries;
            this.CRTotal = newdata.CRTotal;
            this.DRTotal = newdata.DRTotal;
        }
        #endregion

        #region IFilterable Members

        public DateTime GetDate()
        {
            return _date;
        }

        public DateTime GetDateModified()
        {
            return _date;
        }

        public virtual string GetSymbol()
        {
            return string.Empty;
        }

        public virtual int GetAccountID()
        {
            return int.MinValue;
        }
        #endregion

        public void SetErrorState(string ColumnName, string Value)
        {
            switch (ColumnName)
            {
                case "Date":
                    if ((DateTime.MinValue.ToString()) == Value)
                    {
                        if (!_errors.ContainsKey(ColumnName))
                            _errors.Add("Date", "Date is required.");
                    }
                    else
                    {
                        if (_errors.ContainsKey(ColumnName))
                            _errors.Remove(ColumnName);
                    }
                    break;

                case "DRCR":
                    if (Value == "NotEqual")
                    {
                        if (!_errors.ContainsKey("DRTotal"))
                            _errors.Add("DRTotal", "DRTotal is not equal to CRTotal");
                        if (!_errors.ContainsKey("CRTotal"))
                            _errors.Add("CRTotal", "CRTotal is not equal to DRTotal");
                    }
                    else
                    {
                        if (_errors.ContainsKey("DRTotal"))
                            _errors.Remove("DRTotal");

                        if (_errors.ContainsKey("CRTotal"))
                            _errors.Remove("CRTotal");
                    }
                    break;

                default:
                    break;
            }
        }

        #region IDataErrorInfo Members
        [Browsable(false)]
        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                if (_errors.ContainsKey(columnName))
                    return _errors[columnName];
                return string.Empty;
            }
        }
        #endregion

        #region INotifyPropertyChangedCustom Members

        public void PropertyHasChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, null);
            }
        }
        #endregion

        #region INotifyPropertyChanged Members
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
