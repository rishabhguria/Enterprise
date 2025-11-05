using System;
using System.Data;

namespace Act40OrderGeneratorTool
{
    [Serializable]
    class Rule
    {
        #region PrivateFields
        private BookSizeOperation _bookSizeOperation;
        private Double _bookSizeFactor;
        private Int32 _portfoiolLimit;
        private FilterFields _limitingField;
        private DataTable _filterConditions;
        private Boolean _limitDestination;
        private Double _destinationLimit;
        #endregion

        #region Properties
        public BookSizeOperation BookSizeOperation
        {
            get { return _bookSizeOperation; }
            set { _bookSizeOperation = value; }
        }



        public Double BookSizeFactor
        {
            get { return _bookSizeFactor; }
            set { _bookSizeFactor = value; }
        }


        public Int32 PortfolioLimit
        {
            get { return _portfoiolLimit; }
            set { _portfoiolLimit = value; }
        }



        public FilterFields LimitingField
        {
            get { return _limitingField; }
            set { _limitingField = value; }
        }


        public DataTable FilterConditions
        {
            get { return _filterConditions; }
            set { _filterConditions = value; }
        }



        public Boolean LimitDestination
        {
            get { return _limitDestination; }
            set { _limitDestination = value; }
        }


        public Double DestinationLimit
        {
            get { return _destinationLimit; }
            set { _destinationLimit = value; }
        }

        #endregion

        /// <summary>
        /// Creates a default Rule
        /// </summary>
        internal Rule()
        {
            _bookSizeOperation = BookSizeOperation.Percentage;
            _bookSizeFactor = 100;
            _portfoiolLimit = 0;
            _limitingField = FilterFields.MarketCap;
            _filterConditions = DefaultFilterConditions();
            _limitDestination = false;
            _destinationLimit = 0;
        }

        private DataTable DefaultFilterConditions()
        {
            DataTable dt = new DataTable("FilterCondition");
            dt.Columns.Add("Column");
            dt.Columns.Add("Condition");
            dt.Columns.Add("Value");
            return dt;
        }
    }
}
