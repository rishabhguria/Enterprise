using System;
using System.ComponentModel;

namespace Prana.BusinessObjects.Classes.ThirdParty.Tables
{
    public class ThirdPartyForceConfirm
    {
        private int _userId;
        private string _userName;
        private string _broker;
        private string _symbol;
        private string _side;
        private string _quantity;
        private string _allocationId;
        private int _thirdPartyBatchId;
        private string _comment = string.Empty;
        private string _averagePx;
        private string _tradeDate;
        private string _matchStatus;
        private DateTime _confirmationDateTime;
        private string _account;
        private string _commission;
        private string _miscFees;
        private string _netMoney;
        private string _msgType;
        private string _allocReportId;

        public int UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public DateTime ConfirmationDateTime
        {
            get { return _confirmationDateTime; }
            set { _confirmationDateTime = value; }
        }

        public string Broker
        {
            get { return _broker; }
            set { _broker = value; }
        }

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
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

        public string AllocationID
        {
            get { return _allocationId; }
            set { _allocationId = value; }
        }

        public int ThirdPartyBatchID
        {
            get { return _thirdPartyBatchId; }
            set { _thirdPartyBatchId = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string AveragePX
        {
            get { return _averagePx; }
            set { _averagePx = value; }
        }

        public string TradeDate
        {
            get { return _tradeDate; }
            set { _tradeDate = value; }
        }

        public string MatchStatus
        {
            get { return _matchStatus; }
            set { _matchStatus = value; }
        }

        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        public string Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        public string MiscFees
        {
            get { return _miscFees; }
            set { _miscFees = value; }
        }

        public string NetMoney
        {
            get { return _netMoney; }
            set { _netMoney = value; }
        }

        [Browsable(false)]
        public int BlockID
        {
            get;
            set;
        }

        [Browsable(false)]
        public int ConfirmStatus
        {
            get;
            set;
        }
        [Browsable(false)]
        public string AllocId
        {
            get;
            set;
        }

        [Browsable(false)]
        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        [Browsable(false)]
        public string AllocReportId
        {
            get { return _allocReportId; }
            set { _allocReportId = value; }
        }
    }
}
