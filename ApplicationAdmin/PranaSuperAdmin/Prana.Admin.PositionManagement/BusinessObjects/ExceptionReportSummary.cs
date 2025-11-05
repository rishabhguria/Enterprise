using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class ExceptionReportSummary
    {
        private DataSourceNameID _dataSource;

        public DataSourceNameID DataSource
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
