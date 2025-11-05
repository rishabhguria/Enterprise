using System;
using System.Collections.Generic;
using static Prana.Global.ApplicationConstants;

namespace Prana.ThirdPartyManager
{
    public static class ThirdPartyCache
    {
        private static Dictionary<string, Dictionary<int, JobMatchDetails>> _dateWiseAllocationBlockDetails = new Dictionary<string, Dictionary<int, JobMatchDetails>>();
        private static Dictionary<string, HashSet<int>> _dateWiseFileStatusDetails =  new Dictionary<string, HashSet<int>>();

        public static Dictionary<string, Dictionary<int, JobMatchDetails>> DateWiseAllocationBlockDetails
        {
            get { return _dateWiseAllocationBlockDetails; }
            set { _dateWiseAllocationBlockDetails = value; }
        }

        public static Dictionary<string, HashSet<int>> DateWiseFileStatusDetails
        {
            get { return _dateWiseFileStatusDetails; }
            set { _dateWiseFileStatusDetails = value; }
        }

        public static Dictionary<int, string> AutomatedBatchStatus { get; set; } = new Dictionary<int, string>();
    }

    public class JobMatchDetails
    {
        public AllocationMatchStatus AllocationMatchStatus { get; set; } = AllocationMatchStatus.Pending;
        public Dictionary<string, BlockMatchDetails> AllocIdWiseDetails { get; set; } = new Dictionary<string, BlockMatchDetails>();
        public string ThirdPartyJobName { get; set; }
        public int ThirdPartyBatchId { get; set; }
        public DateTime BatchRunDate { get; set; }
    }

    public class BlockMatchDetails
    {
        public BlockMatchStatus BlockMatchStatus { get; set; }
        public int JMsgBlockId { get; set; }
    }
}
