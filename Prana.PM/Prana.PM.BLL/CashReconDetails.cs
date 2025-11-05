//
using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using System;

namespace Prana.PM.BLL
{
    public class CashReconDetails
    {
        private DateTime _date = DateTime.Today;

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private ThirdPartyNameID _dataSourceNameIDValue;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public ThirdPartyNameID DataSourceNameIDValue
        {
            get { return _dataSourceNameIDValue; }
            set { _dataSourceNameIDValue = value; }
        }

        private SortableSearchableList<DividendEntry> _dividendEntryListItems;

        /// <summary>
        /// Gets or sets the dividend entry list items.
        /// </summary>
        /// <value>The dividend entry list items.</value>
        public SortableSearchableList<DividendEntry> DividendEntryListItems
        {
            get { return _dividendEntryListItems; }
            set { _dividendEntryListItems = value; }
        }

        private CashTransactionList _tradingCashTransactionListValue;

        /// <summary>
        /// Gets or sets the trade transaction list.
        /// </summary>
        /// <value>The trade transaction list.</value>
        public CashTransactionList TradingCashTransactionListValue
        {
            get { return _tradingCashTransactionListValue; }
            set { _tradingCashTransactionListValue = value; }
        }

        private SortableSearchableList<CorporateActionEntry> _corporateActionsList;

        /// <summary>
        /// Gets or sets the corporate actions list.
        /// </summary>
        /// <value>The corporate actions list.</value>
        public SortableSearchableList<CorporateActionEntry> CorporateActionsList
        {
            get { return _corporateActionsList; }
            set { _corporateActionsList = value; }
        }

        private double _netCashFlow;

        public double NetCashFlow
        {
            get { return _netCashFlow; }
            set { _netCashFlow = value; }
        }




    }
}
