using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.BusinessObjects.Classes.ThirdParty.Tables
{
    public class ThirdPartyAllocationLevelDetails
    {
        private int _allocationDetailId;
        private string _account;
        private string _commission;
        private string _quantity;
        private string _miscFees;
        private string _averagePx;
        private string _netMoney;
        private string _matchStatus;
        private string _accountAllocationId;
        private int _blockDetailID;
        private List<ThirdPartyAllocationDetailComparison> _allocationComparisons;
        private string _messageType;

        [Browsable(false)]
        public int AllocationDetailId
        {
            get { return _allocationDetailId; }
            set { _allocationDetailId = value; }
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

        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public string MiscFees
        {
            get { return _miscFees; }
            set { _miscFees = value; }
        }

        public string AveragePX
        {
            get { return _averagePx; }
            set { _averagePx = value; }
        }

        public string NetMoney
        {
            get { return _netMoney; }
            set { _netMoney = value; }
        }

        public string MatchStatus
        {
            get { return _matchStatus; }
            set { _matchStatus = value; }
        }

        public string AccountAllocationID
        {
            get { return _accountAllocationId; }
            set { _accountAllocationId = value; }
        }

        [Browsable(false)]
        public int BlockDetailID
        {
            get { return _blockDetailID; }
            set { _blockDetailID = value; }
        }

        [Browsable(false)]
        public List<ThirdPartyAllocationDetailComparison> AllocationComparisons
        {
            get { return _allocationComparisons; }
            set { _allocationComparisons = value; }
        }

        [Browsable(false)]
        public string MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }
    }
}
