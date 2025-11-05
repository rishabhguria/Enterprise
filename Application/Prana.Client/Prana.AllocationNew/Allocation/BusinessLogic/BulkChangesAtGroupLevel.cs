using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.AllocationNew
{
    /// <summary>
    /// This object is used for updating group collection for the following fields in one go.
    /// </summary>
    class BulkChangesAtGroupLevel
    {
        private decimal _avgPrice = 0;
        public decimal AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }
        /// <summary>
        /// set and get avgPxUpto given by user
        /// </summary>
        private int _avgPxUpto = 0;
        public int AvgPxUpto
        {
            get { return _avgPxUpto; }
            set { _avgPxUpto = value; }
        }

        private decimal _FXRate = 0;
        public decimal FXRate
        {
            get { return _FXRate; }
            set { _FXRate = value; }
        }

        private decimal _accruedInterest;
        public decimal AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _internalComments = null;
        public String InternalComments
        {
            get { return _internalComments; }
            set { _internalComments = value; }
        }


        private int _counterPartyID = int.MinValue;
        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        private string _fxConversionOperator = string.Empty;
        public string FXConversionOperator
        {
            get { return _fxConversionOperator; }
            set { _fxConversionOperator = value; }
        }

        #region settlement currency fields
        // CHMW-3035 [Foreign Positions Settling in Base Currency] Add settlement currency fields in application business objects
        private int _settlCurrency;
        public virtual int SettlCurrency
        {
            get { return _settlCurrency; }
            set { _settlCurrency = value; }
        }

        private double _settlCurrFxRate;
        public virtual double SettlCurrFxRate
        {
            get { return _settlCurrFxRate; }
            set { _settlCurrFxRate = value; }
        }

        private string _SettlCurrFxRateCalc = string.Empty;
        public string SettlCurrFxRateCalc
        {
            get { return _SettlCurrFxRateCalc; }
            set
            {
                _SettlCurrFxRateCalc = value;
            }
        }

        private double _settlCurrAmt;
        public virtual double SettlCurrAmt
        {
            get { return _settlCurrAmt; }
            set { _settlCurrAmt = value; }
        }
        #endregion

        private bool _groupWise = true;
        public bool GroupWise
        {
            get { return _groupWise; }
            set { _groupWise= value; }
        }

        private int _fxCounterPartyID = int.MinValue;
        public int FXCounterPartyID
        {
            get { return _fxCounterPartyID; }
            set { _fxCounterPartyID = value; }
        }

        private List<int> _accountIDs = null;
        public List<int> AccountIDs
        {
            get { return _accountIDs; }
            set { _accountIDs = value; }
        }

        

    }
}
