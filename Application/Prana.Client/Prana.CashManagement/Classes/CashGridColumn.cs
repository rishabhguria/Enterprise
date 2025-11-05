using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;

namespace Prana.CashManagement.Classes
{
    [Serializable]
    public class CashGridColumn : IComparable<CashGridColumn>, IDisposable
    {
        #region Members

        /// <summary>
        /// The hidden
        /// </summary>
        private bool _hidden;

        /// <summary>
        /// The name
        /// </summary>
        private string _name;

        /// <summary>
        /// The position
        /// </summary>
        private int _position;

        /// <summary>
        /// The width
        /// </summary>
        private int _width;

        /// <summary>
        /// The is header fixed
        /// </summary>
        private bool _isHeaderFixed;

        /// <summary>
        /// The filter condition list
        /// </summary>
        List<FilterCondition> _filterConditionList = new List<FilterCondition>();

        /// <summary>
        /// The sort indicator
        /// </summary>
        SortIndicator _sortIndicator = SortIndicator.None;

        /// <summary>
        /// The filter logical operator
        /// </summary>
        FilterLogicalOperator _filterLogicalOperator = FilterLogicalOperator.And;

        /// <summary>
        /// The column type
        /// </summary>
        private string _columnType;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the type of the column.
        /// </summary>
        /// <value>
        /// The type of the column.
        /// </value>
        public string ColumnType
        {
            get { return _columnType; }
            set { _columnType = value; }
        }

        /// <summary>
        /// Gets or sets the filter condition list.
        /// </summary>
        /// <value>
        /// The filter condition list.
        /// </value>
        public List<FilterCondition> FilterConditionList
        {
            get
            {
                return _filterConditionList;
            }
            set
            {

                _filterConditionList = value;
            }
        }

        /// <summary>
        /// Gets or sets the filter logical operator.
        /// </summary>
        /// <value>
        /// The filter logical operator.
        /// </value>
        public FilterLogicalOperator FilterLogicalOperator
        {
            get
            {
                return _filterLogicalOperator;
            }
            set
            {

                _filterLogicalOperator = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CashGridColumn"/> is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if hidden; otherwise, <c>false</c>.
        /// </value>
        public bool Hidden
        {
            get { return _hidden; }
            set { _hidden = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is header fixed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is header fixed; otherwise, <c>false</c>.
        /// </value>
        public bool IsHeaderFixed
        {
            get { return _isHeaderFixed; }
            set { _isHeaderFixed = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Gets or sets the sort indicator.
        /// </summary>
        /// <value>
        /// The sort indicator.
        /// </value>
        public SortIndicator SortIndicator
        {
            get
            {
                return _sortIndicator;
            }
            set
            {

                _sortIndicator = value;
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CashGridColumn"/> class.
        /// </summary>
        public CashGridColumn()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CashGridColumn"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public CashGridColumn(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CashGridColumn"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="position">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="isHeaderFixed">if set to <c>true</c> [is header fixed].</param>
        /// <param name="filterconditionList">The filtercondition list.</param>
        /// <param name="columnType">Type of the column.</param>
        /// <param name="filterLogicalOperator">The filter logical operator.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="hidden">if set to <c>true</c> [hidden].</param>
        public CashGridColumn(string name, int position, int width, bool isHeaderFixed, List<FilterCondition> filterconditionList, string columnType, FilterLogicalOperator filterLogicalOperator, SortIndicator sort, bool hidden)
        {
            _name = name;
            _position = position;
            _width = width;
            _isHeaderFixed = isHeaderFixed;
            _filterConditionList = filterconditionList;
            _columnType = columnType;
            _filterLogicalOperator = filterLogicalOperator;
            _sortIndicator = sort;
            _hidden = hidden;
        }

        #endregion Constructors

        #region IComparable Members

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public int CompareTo(CashGridColumn obj)
        {
            return _position.CompareTo(obj._position);
        }

        #endregion IComparable Members

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _name = null;
                _filterConditionList = null;
                _columnType = null;
            }
        }

        #endregion IDisposable Members
    }
}
