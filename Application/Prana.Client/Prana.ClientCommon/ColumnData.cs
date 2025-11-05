using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.ClientCommon
{
    [XmlInclude(typeof(Prana.BusinessObjects.PositionManagement.Account))]
    [Serializable]
    public class ColumnData : IComparable<ColumnData>
    {
        public ColumnData()
        {
        }
        public ColumnData(string key, int visiblePosition, int width, bool isVisible, string caption, string format)
        {
            _key = key;
            _visiblePosition = visiblePosition;
            _width = width;
            _hidden = isVisible;
            _caption = caption;
            _format = format;

        }

        private string _key = String.Empty;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private int _visiblePosition = 1;
        public int VisiblePosition
        {
            get { return _visiblePosition; }
            set { _visiblePosition = value; }
        }

        private int _width = 70;
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private bool _hidden = true;
        public bool Hidden
        {
            get { return _hidden; }
            set { _hidden = value; }
        }

        private string _caption = String.Empty;
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        private string _format = String.Empty;
        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        private ExcludeFromColumnChooser _excludeFromColumnChooser;
        public ExcludeFromColumnChooser ExcludeFromColumnChooser
        {
            get { return _excludeFromColumnChooser; }
            set { _excludeFromColumnChooser = value; }
        }

        private SortIndicator _sortIndicator;
        public SortIndicator SortIndicator
        {
            get { return _sortIndicator; }
            set { _sortIndicator = value; }
        }

        private bool _isGroupByColumn;
        public bool IsGroupByColumn
        {
            get { return _isGroupByColumn; }
            set { _isGroupByColumn = value; }
        }

        private string _groupheaderName = string.Empty;
        public string GroupHeaderName
        {
            get { return _groupheaderName; }
            set { _groupheaderName = value; }
        }


        private bool _fixed;
        public bool Fixed
        {
            get { return _fixed; }
            set { _fixed = value; }
        }

        private List<FilterCondition> _filterConditionList = new List<FilterCondition>();
        public List<FilterCondition> FilterConditionList
        {
            get { return _filterConditionList; }
            set { _filterConditionList = value; }
        }

        private FilterLogicalOperator _filterLogicalOperator;
        public FilterLogicalOperator FilterLogicalOperator
        {
            get { return _filterLogicalOperator; }
            set { _filterLogicalOperator = value; }
        }

        private string _colSummaryKey = String.Empty;
        public string ColSummaryKey
        {
            get { return _colSummaryKey; }
            set { _colSummaryKey = value; }
        }

        private string _colSummaaryFromat = String.Empty;
        public string ColSummaryFormat
        {
            get { return _colSummaaryFromat; }
            set { _colSummaaryFromat = value; }
        }

        private Activation _cellActivation;
        public Activation CellActivation
        {
            get { return _cellActivation; }
            set { _cellActivation = value; }
        }

        private String _nullText;
        public String NullText
        {
            get { return _nullText; }
            set { _nullText = value; }
        }


        private ColumnStyle _ColumnStyle;
        public ColumnStyle ColumnStyle
        {
            get { return _ColumnStyle; }
            set { _ColumnStyle = value; }
        }

        private ButtonDisplayStyle _ButtonDisplayStyle;
        public ButtonDisplayStyle ButtonDisplayStyle
        {
            get { return _ButtonDisplayStyle; }
            set { _ButtonDisplayStyle = value; }
        }
        //modified by amit.changes done for http://jira.nirvanasolutions.com:8080/browse/CHMW-3569
        private string _maskInput;
        public string MaskInput
        {
            get { return _maskInput; }
            set { _maskInput = value; }
        }

        public int CompareTo(ColumnData colData)
        {
            return _visiblePosition.CompareTo(colData.VisiblePosition);
        }
    }
}
