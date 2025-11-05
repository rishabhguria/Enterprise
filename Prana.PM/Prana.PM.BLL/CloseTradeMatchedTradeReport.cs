//
using Prana.BusinessObjects;
using System;

namespace Prana.PM.BLL
{
    public class CloseTradeMatchedTradeReport
    {
        private DateTime _reportDate;

        public DateTime ReportDate
        {
            get { return _reportDate; }
            set { _reportDate = value; }
        }

        private SortableSearchableList<CloseTradeMatchedTradeReportItem> _reportItems;

        public SortableSearchableList<CloseTradeMatchedTradeReportItem> ReportItems
        {
            get
            {
                if (_reportItems == null)
                {
                    _reportItems = new SortableSearchableList<CloseTradeMatchedTradeReportItem>();
                }
                return _reportItems;
            }
            set { _reportItems = value; }
        }


    }
}
