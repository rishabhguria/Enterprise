using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class GenericPositionData
    {
        private List<TaxLot> _positions = new List<TaxLot>();
        private List<TaxLot> _transactions = new List<TaxLot>();
        private GenericDayEndData _genericDayEndDataValue = new GenericDayEndData();
        private string _commaSapratedAssetIDs = null;
        private string _commaSapratedAccountIds = null;
        private string _symbols = string.Empty;
        private string _customConditions = string.Empty;
        private bool _isSameDateInAllAUEC = false;
        private bool _isTransactionsIncludedInPositions = false;
        private bool _isAccrualsNeeded = false;
        private DateTime _giventDate = DateTime.MinValue;
        private DateTime _mostLeadingTimeZoneDate = DateTime.MinValue;
        private List<TaxLot> _unallocatedTrades = new List<TaxLot>();
        private bool _isBetaNeeded = false;
        private Dictionary<string, double> _symbolWiseBeta = new Dictionary<string, double>();
        private bool _isFxRateNeeded = false;
        private Dictionary<int, DateTime> _currentOffsetAdjustedAUECDates = new Dictionary<int, DateTime>();

        public List<TaxLot> Positions
        {
            get { return _positions; }
            set { _positions = value; }
        }
        public List<TaxLot> Transactions
        {
            get { return _transactions; }
            set { _transactions = value; }
        }
        public GenericDayEndData GenericDayEndDataValue
        {
            get { return _genericDayEndDataValue; }
            set { _genericDayEndDataValue = value; }
        }
        public string CommaSapratedAssetIDs
        {
            get { return _commaSapratedAssetIDs; }
            set { _commaSapratedAssetIDs = value; }
        }
        public string CommaSapratedAccountIds
        {
            get { return _commaSapratedAccountIds; }
            set { _commaSapratedAccountIds = value; }
        }
        public string Symbols
        {
            get { return _symbols; }
            set { _symbols = value; }
        }
        public string CustomConditions
        {
            get { return _customConditions; }
            set { _customConditions = value; }
        }
        public bool IsSameDateInAllAUEC
        {
            get { return _isSameDateInAllAUEC; }
            set { _isSameDateInAllAUEC = value; }
        }
        public bool IsTransactionsIncludedInPositions
        {
            get { return _isTransactionsIncludedInPositions; }
            set { _isTransactionsIncludedInPositions = value; }
        }
        public bool IsAccrualsNeeded
        {
            get { return _isAccrualsNeeded; }
            set { _isAccrualsNeeded = value; }
        }
        public DateTime GivenDate
        {
            get { return _giventDate; }
            set { _giventDate = value; }
        }
        public DateTime MostLeadingTimeZoneDate
        {
            get { return _mostLeadingTimeZoneDate; }
            set { _mostLeadingTimeZoneDate = value; }
        }
        public List<TaxLot> UnallocatedTrades
        {
            get { return _unallocatedTrades; }
            set { _unallocatedTrades = value; }
        }
        public bool IsBetaNeeded
        {
            get { return _isBetaNeeded; }
            set { _isBetaNeeded = value; }
        }
        public Dictionary<string, double> SymbolWiseBeta
        {
            get { return _symbolWiseBeta; }
            set { _symbolWiseBeta = value; }
        }

        public bool IsFxRateNeeded
        {
            get { return _isFxRateNeeded; }
            set { _isFxRateNeeded = value; }
        }

        public Dictionary<int, DateTime> CurrentOffsetAdjustedAUECDates
        {
            get { return _currentOffsetAdjustedAUECDates; }
            set { _currentOffsetAdjustedAUECDates = value; }
        }
    }
}