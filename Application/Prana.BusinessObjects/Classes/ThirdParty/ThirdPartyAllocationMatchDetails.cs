using System;
using static Prana.Global.ApplicationConstants;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ThirdPartyAllocationMatchDetails
    {
        private int _thirdPartyBatchId;
        private AllocationMatchStatus _allocationMatchStatus;
        private DateTime _batchRunDate;

        public int ThirdPartyBatchId
        {
            get { return _thirdPartyBatchId; }
            set { _thirdPartyBatchId = value; }
        }

        public AllocationMatchStatus AllocationMatchStatus
        {
            get { return _allocationMatchStatus; }
            set { _allocationMatchStatus = value; }
        }

        public DateTime BatchRunDate
        {
            get { return _batchRunDate; }
            set { _batchRunDate = value; }
        }
    }
}
