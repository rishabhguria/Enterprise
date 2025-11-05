using Prana.BusinessObjects.PositionManagement;
using System.ComponentModel;

namespace Prana.PM.BLL
{
    public class ExceptionReportSummary
    {
        private ThirdPartyNameID _dataSource;

        public ThirdPartyNameID DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        private BindingList<ExceptionReportEntry> _exceptionReportEntryList;

        public BindingList<ExceptionReportEntry> ExceptionReportEntryList
        {
            get { return _exceptionReportEntryList; }
            set { _exceptionReportEntryList = value; }
        }

        private BindingList<ExceptionReportEntry> _unknownRecordList;

        public BindingList<ExceptionReportEntry> UnknownRecordList
        {
            get { return _unknownRecordList; }
            set { _unknownRecordList = value; }
        }

    }
}
