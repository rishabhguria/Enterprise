using Prana.BusinessObjects;
using System.Collections.Generic;

namespace Prana.ExpnlService
{
    public class AccountInfo
    {
        private Dictionary<int, ExposureAndPnlOrderSummary> _accountWiseSummary;
        public Dictionary<int, ExposureAndPnlOrderSummary> AccountWiseSummary
        {
            get { return _accountWiseSummary; }
            set { _accountWiseSummary = value; }
        }

        private Dictionary<int, ExposureAndPnlOrderCollection> _accountWiseOrderCollection;
        public Dictionary<int, ExposureAndPnlOrderCollection> AccountWiseOrderCollection
        {
            get { return _accountWiseOrderCollection; }
            set { _accountWiseOrderCollection = value; }
        }
    }
}
