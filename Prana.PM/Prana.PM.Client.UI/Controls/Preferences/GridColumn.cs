using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;

namespace Prana.PM.BLL
{
    [Serializable]
    public class PreferenceGridColumn : IComparable<PreferenceGridColumn>, IDisposable
    {
        public PreferenceGridColumn()
        { }

        public PreferenceGridColumn(string name)
        {
            _name = name;
        }

        public PreferenceGridColumn(string name, int position, int width, bool isHeaderFixed, List<FilterCondition> filterconditionList, FilterLogicalOperator filterLogicalOperator)
        {
            _name = name;
            _position = position;
            _width = width;
            _isHeaderFixed = isHeaderFixed;
            _filterConditionList = filterconditionList;
            _filterLogicalOperator = filterLogicalOperator;
        }
        public PreferenceGridColumn(string name, int position, int width, bool isHeaderFixed, List<FilterCondition> filterconditionList, string columnFormula, string columnType, FilterLogicalOperator filterLogicalOperator, SortIndicator sort, bool hidden)
        {
            _name = name;
            _position = position;
            _width = width;
            _isHeaderFixed = isHeaderFixed;
            _filterConditionList = filterconditionList;
            _columnFormula = columnFormula;
            _columnType = columnType;
            _filterLogicalOperator = filterLogicalOperator;
            _sortIndicator = sort;
            _hidden = hidden;
        }

        private bool _hidden;

        public bool Hidden
        {
            get { return _hidden; }
            set { _hidden = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _position;

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private int _width;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private bool _isHeaderFixed;

        public bool IsHeaderFixed
        {
            get { return _isHeaderFixed; }
            set { _isHeaderFixed = value; }
        }

        public int CompareTo(PreferenceGridColumn obj)
        {
            return _position.CompareTo(obj._position);
        }

        List<FilterCondition> _filterConditionList = new List<FilterCondition>();
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

        SortIndicator _sortIndicator = SortIndicator.None;
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


        FilterLogicalOperator _filterLogicalOperator = FilterLogicalOperator.And;
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

        private string _columnFormula;

        public string columnFormula
        {
            get { return _columnFormula; }
            set { _columnFormula = value; }
        }
        private string _columnType;

        public string ColumnType
        {
            get { return _columnType; }
            set { _columnType = value; }
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _name = null;
                _filterConditionList = null;
                _columnFormula = null;
                _columnType = null;
            }
        }
        #endregion
    }
}
