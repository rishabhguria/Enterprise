using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class DistinctAccountSetWiseSummaryCollection
    {

        private Dictionary<string, ExposureAndPnlOrderSummary> _symbolWiseGroupSummary = new Dictionary<string, ExposureAndPnlOrderSummary>();
        public Dictionary<string, ExposureAndPnlOrderSummary> SymbolWiseGroupSummary
        {
            get { return _symbolWiseGroupSummary; }
            set { _symbolWiseGroupSummary = value; }
        }

        private Dictionary<string, ExposureAndPnlOrderSummary> _underlyingSymbolWiseGroupSummary = new Dictionary<string, ExposureAndPnlOrderSummary>();
        public Dictionary<string, ExposureAndPnlOrderSummary> UnderlyingSymbolWiseGroupSummary
        {
            get { return _underlyingSymbolWiseGroupSummary; }
            set { _underlyingSymbolWiseGroupSummary = value; }
        }

        private ExposureAndPnlOrderSummary _consolidationDashBoardSummary = new ExposureAndPnlOrderSummary();
        public ExposureAndPnlOrderSummary ConsolidationDashBoardSummary
        {
            get { return _consolidationDashBoardSummary; }
            set { _consolidationDashBoardSummary = value; }
        }

        private string _tabKey = string.Empty;
        public string TabKey
        {
            get { return _tabKey; }
            set { _tabKey = value; }
        }
    }
}
