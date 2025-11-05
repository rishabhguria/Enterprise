using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class TransactionReportSummary
    {
        private DataSourceNameID _dataSourceNameIDValue;

        public DataSourceNameID DataSourceNameIDValue
        {
            get { return _dataSourceNameIDValue; }
            set { _dataSourceNameIDValue = value; }
        }

        private ExceptionReportEntry _exceptionReportEntry;

        public ExceptionReportEntry ExceptionReportEntry
        {
            get { return _exceptionReportEntry; }
            set { _exceptionReportEntry = value; }
        }

        private DateTime _transactionDate;

        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set { _transactionDate = value; }
        }

        private BindingList<TradeTransaction> _tradeTransactionList;

        public BindingList<TradeTransaction> TradeTransactionList
        {
            get { return _tradeTransactionList; }
            set { _tradeTransactionList = value; }
        }

        private DateTime _corporateActionDate;

        public DateTime CorporateActionDate
        {
            get { return _corporateActionDate; }
            set { _corporateActionDate = value; }
        }

        private BindingList<CorporateAction> _corporateActionEntryList;

        public BindingList<CorporateAction> CorporateActionEntryList
        {
            get { return _corporateActionEntryList; }
            set { _corporateActionEntryList = value; }
        }


    }
}
