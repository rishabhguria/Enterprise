namespace Prana.PM.BLL
{
    public class ExceptionReportEntry
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _account;

        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        private string _columnName;

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private string _applicationData;

        public string ApplicationData
        {
            get { return _applicationData; }
            set { _applicationData = value; }
        }

        private string _sourceData;

        public string SourceData
        {
            get { return _sourceData; }
            set { _sourceData = value; }
        }

        private string _manualEntry;

        public string ManualEntry
        {
            get { return _manualEntry; }
            set { _manualEntry = value; }
        }

        private ReconStatus _status;

        public ReconStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

    }
}
