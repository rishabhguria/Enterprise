using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.BusinessObjects.Classes.ThirdParty.Tables
{
    public class ThirdPartyBlockLevelDetails
    {
        private int _blockDetailId;
        private string _lastUpdated;
        private string _currency;
        private string _symbol;
        private string _isinSedolCusip;
        private string _isin;
        private string _sedol;
        private string _cusip;
        private string _grossAmount;
        private string _side;
        private string _quantity;
        private string _tradeDate;
        private string _settlementDate;
        private string _averagePx;
        private string _commission;
        private string _netAmount;
        private string _matchStatus;
        private string _subStatus;
        private string _allocationId;
        private List<ThirdPartyAllocationLevelDetails> _allocationLevelDetails;
        private string _transactionTime;
        private string _allocationTransactionType;
        private string _text;
        private string _allocReportId;
        private string _msgType;

        [Browsable(false)]
        public int BlockDetailId
        {
            get { return _blockDetailId; }
            set { _blockDetailId = value; }
        }

        public string LastUpdated
        {
            get { return _lastUpdated; }
            set { _lastUpdated = value; }
        }

        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public string ISINSedolCUSIP
        {
            get
            {
                if (!string.IsNullOrEmpty(ISIN))
                {
                    _isinSedolCusip = ISIN;
                }
                if (!string.IsNullOrEmpty(Sedol))
                {
                    _isinSedolCusip += "/" + Sedol;
                }
                if (!string.IsNullOrEmpty(CUSIP))
                {
                    _isinSedolCusip += "/" + CUSIP;
                }
                return _isinSedolCusip;
            }
        }

        [Browsable(false)]
        public string ISIN
        {
            get { return _isin; }
            set { _isin = value; }
        }

        [Browsable(false)]
        public string Sedol
        {
            get { return _sedol; }
            set { _sedol = value; }
        }

        [Browsable(false)]
        public string CUSIP
        {
            get { return _cusip; }
            set { _cusip = value; }
        }

        public string GrossAmount
        {
            get { return _grossAmount; }
            set { _grossAmount = value; }
        }

        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public string TradeDate
        {
            get { return _tradeDate; }
            set { _tradeDate = value; }
        }

        public string SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }

        public string AveragePX
        {
            get { return _averagePx; }
            set { _averagePx = value; }
        }

        public string Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        public string NetAmount
        {
            get { return _netAmount; }
            set { _netAmount = value; }
        }

        public string MatchStatus
        {
            get { return _matchStatus; }
            set { _matchStatus = value; }
        }

        public string SubStatus
        {
            get { return _subStatus; }
            set { _subStatus = value; }
        }

        public string AllocationID
        {
            get { return _allocationId; }
            set { _allocationId = value; }
        }

        [Browsable(false)]
        public List<ThirdPartyAllocationLevelDetails> AllocationLevelDetails
        {
            get { return _allocationLevelDetails; }
            set { _allocationLevelDetails = value; }
        }

        [Browsable(false)]
        public string TransactionTime
        {
            get { return _transactionTime; }
            set { _transactionTime = value; }
        }

        [Browsable(false)]
        public string AllocationTransactionType
        {
            get { return _allocationTransactionType; }
            set { _allocationTransactionType = value; }
        }

        [Browsable(false)]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        [Browsable(false)]
        public string AllocReportId
        {
            get { return _allocReportId; }
            set { _allocReportId = value; }
        }

        [Browsable(false)]
        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }
    }
}
