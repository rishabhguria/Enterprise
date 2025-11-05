using Prana.BusinessObjects.PositionManagement;

namespace Prana.PM.BLL
{
    /// <summary>
    /// Business class used to display data on Run Recon Grid of PM
    /// </summary>
    public class TradeReconSummary
    {
        private bool _isSelectedforViewing;

        public bool IsSelectedforViewing
        {
            get { return _isSelectedforViewing; }
            set { _isSelectedforViewing = value; }
        }

        private ThirdPartyNameID _dataSourceName;

        public ThirdPartyNameID DataSourceName
        {
            get { return _dataSourceName; }
            set { _dataSourceName = value; }
        }

        private int _noOfDataSourceRecords;

        public int NoOfDataSourceRecords
        {
            get { return _noOfDataSourceRecords; }
            set { _noOfDataSourceRecords = value; }
        }

        private int _noOfApplicationRecords;

        public int NoOfApplicationRecords
        {
            get { return _noOfApplicationRecords; }
            set { _noOfApplicationRecords = value; }
        }

        private int _noOfReconRecords;

        public int NoOfReconRecords
        {
            get { return _noOfReconRecords; }
            set { _noOfReconRecords = value; }
        }

        private int _noOfMatchedRecords;

        public int NoOfMatchedRecords
        {
            get { return _noOfMatchedRecords; }
            set { _noOfMatchedRecords = value; }
        }

        private int _noOfMismatchedRecords;

        public int NoOfMismatchedRecords
        {
            get { return _noOfMismatchedRecords; }
            set { _noOfMismatchedRecords = value; }
        }

        private ReconStatus _reconStatus;

        public ReconStatus ReconStatus
        {
            get { return _reconStatus; }
            set { _reconStatus = value; }
        }


    }
}
