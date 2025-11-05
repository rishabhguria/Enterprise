using Prana.BusinessObjects.PositionManagement;
using System;
using System.ComponentModel;

namespace Prana.PM.BLL
{
    public class TransactionReportSummary
    {
        private ThirdPartyNameID _dataSourceNameIDValue;

        public ThirdPartyNameID DataSourceNameIDValue
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
