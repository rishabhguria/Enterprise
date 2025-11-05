using System;

namespace Prana.BusinessObjects
{
    public class CorpActionParameterHelper
    {
        private string _originalSymbol = string.Empty;

        public string OriginalSymbol
        {
            get { return _originalSymbol; }
            set { _originalSymbol = value; }
        }

        private string _originalSymbolCompanyName = string.Empty;

        public string OriginalSymbolCompanyName
        {
            get { return _originalSymbolCompanyName; }
            set { _originalSymbolCompanyName = value; }
        }

        private string _newSymbol = string.Empty;

        public string NewSymbol
        {
            get { return _newSymbol; }
            set { _newSymbol = value; }
        }

        private string _newSymbolCompanyName = string.Empty;

        public string NewSymbolCompanyName
        {
            get { return _newSymbolCompanyName; }
            set { _newSymbolCompanyName = value; }
        }

        private string _caID = string.Empty;

        public string CAID
        {
            get { return _caID; }
            set { _caID = value; }
        }

        private DateTime _effectiveDate;

        public DateTime EffectiveDate
        {
            get { return _effectiveDate; }
            set { _effectiveDate = value; }
        }

        private double _newSharesReceivedRatio = 0;

        public double NewSharesReceivedRatio
        {
            get { return _newSharesReceivedRatio; }
            set { _newSharesReceivedRatio = value; }
        }

        private double _spinOffRatio = 0;

        public double SpinOffRatio
        {
            get { return _spinOffRatio; }
            set { _spinOffRatio = value; }
        }

        private int _cashInLieuAt = 0;

        public int CashInLieuAt
        {
            get { return _cashInLieuAt; }
            set { _cashInLieuAt = value; }
        }

        private bool _isCashLieuRequired = false;

        public bool IsCashInLieuRequired
        {
            get { return _isCashLieuRequired; }
            set { _isCashLieuRequired = value; }
        }

        private bool _adjustCashinLieuatAccountLevel = true;

        public bool AdjustCashinLieuatAccountLevel
        {
            get { return _adjustCashinLieuatAccountLevel; }
            set { _adjustCashinLieuatAccountLevel = value; }
        }

        private double _closingPrice = 0;

        public double ClosingPrice
        {
            get { return _closingPrice; }
            set { _closingPrice = value; }
        }

        private int _quantityToRoundoff = 0;

        public int QuantityToRoundoff
        {
            get { return _quantityToRoundoff; }
            set { _quantityToRoundoff = value; }
        }

        private bool _useNetNotional = true;
        public bool UseNetNotional
        {
            get { return _useNetNotional; }
            set { _useNetNotional = value; }
        }

        private int _counterPartyId = 0;

        public int CounterPartyId
        {
            get { return _counterPartyId; }
            set { _counterPartyId = value; }
        }

        private int _originalSymbolCurrencyID = 1;

        public int OriginalSymbolCurrencyID
        {
            get { return _originalSymbolCurrencyID; }
            set { _originalSymbolCurrencyID = value; }
        }

        private int _originalSymbolAUECID = 1;

        public int OriginalSymbolAUECID
        {
            get { return _originalSymbolAUECID; }
            set { _originalSymbolAUECID = value; }
        }

        private int _newSymbolCurrencyID = 1;

        public int NewSymbolCurrencyID
        {
            get { return _newSymbolCurrencyID; }
            set { _newSymbolCurrencyID = value; }
        }

        private int _newSymbolAUECID = 1;

        public int NewSymbolAUECID
        {
            get { return _newSymbolAUECID; }
            set { _newSymbolAUECID = value; }
        }

        private double _originalSymbolFXRate = 0;

        public double OriginalSymbolFXRate
        {
            get { return _originalSymbolFXRate; }
            set { _originalSymbolFXRate = value; }
        }

        private double _newSymbolFXRate = 0;

        public double NewSymbolFXRate
        {
            get { return _newSymbolFXRate; }
            set { _newSymbolFXRate = value; }
        }

    }
}
