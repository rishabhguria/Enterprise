using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class OutputCompressedSummaries
    {
        //Account wise summaries
        private Dictionary<int, ExposureAndPnlOrderSummary> _outputCompressedAccountSummaries = new Dictionary<int, ExposureAndPnlOrderSummary>();
        public Dictionary<int, ExposureAndPnlOrderSummary> OutputCompressedAccountSummaries
        {
            get { return _outputCompressedAccountSummaries; }
            set { _outputCompressedAccountSummaries = value; }
        }

        Dictionary<int, DistinctAccountSetWiseSummaryCollection> _outputAccountSetWiseConsolidatedSummary = new Dictionary<int, DistinctAccountSetWiseSummaryCollection>();
        public Dictionary<int, DistinctAccountSetWiseSummaryCollection> OutputAccountSetWiseConsolidatedSummary
        {
            get { return _outputAccountSetWiseConsolidatedSummary; }
            set { _outputAccountSetWiseConsolidatedSummary = value; }
        }
    }
}
