namespace Prana.BusinessObjects.SMObjects
{
    public class SMBatch
    {
        public enum BatchType
        {
            HistoricalBatch,
            CurrentBatch
        }

        #region Private members

        private int _smBatchID = int.MinValue;
        private string _systemLevelName = string.Empty;
        private string _userDefinedName = string.Empty;
        private string _accountIDs = string.Empty;
        private bool _isHistoric = false;
        private int _historicDaysRequired = int.MinValue;
        private string _fields = string.Empty;
        private string _indices = string.Empty;
        private int _runTime = int.MinValue;
        private string _cronExpression = string.Empty;
        private string _filterClause = string.Empty;
        private BatchType _batType = BatchType.HistoricalBatch;

        #endregion

        #region Constructors
        public SMBatch()
        {
        }

        public SMBatch(int smBatchID)
        {
            _smBatchID = smBatchID;
        }
        #endregion

        #region Properties
        public int SMBatchID
        {
            get { return _smBatchID; }
            set { _smBatchID = value; }
        }

        public string SystemLevelName
        {
            get { return _systemLevelName; }
            set { _systemLevelName = value; }
        }

        public string UserDefinedName
        {
            get { return _userDefinedName; }
            set { _userDefinedName = value; }
        }

        public string AccountIDs
        {
            get { return _accountIDs; }
            set { _accountIDs = value; }
        }

        public bool IsHistoric
        {
            get { return _isHistoric; }
            set { _isHistoric = value; }
        }

        public int HistoricDaysRequired
        {
            get { return _historicDaysRequired; }
            set { _historicDaysRequired = value; }
        }

        public string Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        public string Indices
        {
            get { return _indices; }
            set { _indices = value; }
        }

        public int RunTime
        {
            get { return _runTime; }
            set { _runTime = value; }
        }

        public string CronExpression
        {
            get { return _cronExpression; }
            set { _cronExpression = value; }
        }

        public string FilterClause
        {
            get { return _filterClause; }
            set { _filterClause = value; }
        }

        public BatchType BatType
        {
            get { return _batType; }
            set { _batType = value; }
        }
        #endregion
    }
}
