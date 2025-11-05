namespace Prana.PM.BLL
{
    public class TradeReconManualEntry
    {
        private User _user;

        public User User
        {
            get
            {
                if (_user == null)
                {
                    _user = new User();
                }
                return _user;
            }
            set { _user = value; }
        }

        private ExceptionReportEntry _exceptionReportEntryItem;

        /// <summary>
        /// Gets or sets the exception report entry item.
        /// </summary>
        /// <value>The exception report entry item.</value>
        public ExceptionReportEntry ExceptionReportEntryItem
        {
            get
            {
                if (_exceptionReportEntryItem == null)
                {
                    _exceptionReportEntryItem = new ExceptionReportEntry();
                }
                return _exceptionReportEntryItem;
            }
            set { _exceptionReportEntryItem = value; }
        }

        private string _comments;

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

    }
}
