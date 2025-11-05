using Prana.BusinessObjects;
using System;

namespace Prana.PM.Client.UI
{
    [Serializable]
    public class PMAppearances : IDisposable
    {

        private int _rowBgcolor = -1578775; //Grid background color *Black*
        public int RowBgColor
        {
            get { return _rowBgcolor; }
            set { _rowBgcolor = value; }
        }


        private int _orderSideBuyColor = -15228834; //Row Color : OrderSide *Green*
        public int OrderSideBuyColor
        {
            get { return _orderSideBuyColor; }
            set { _orderSideBuyColor = value; }
        }

        private int _orderSideSellColor = -3647189; //Row Color : OrderSide *Red*
        public int OrderSideSellColor
        {
            get { return _orderSideSellColor; }
            set { _orderSideSellColor = value; }
        }

        private int _dayPnlPositiveColor = -5121984; //Row Color : DayPnl *Green*
        public int DayPnlPositiveColor
        {
            get { return _dayPnlPositiveColor; }
            set { _dayPnlPositiveColor = value; }
        }

        private int _dayPnlNegativeColor = -42169; //Row Color : DayPnl *Red*
        public int DayPnlNegativeColor
        {
            get { return _dayPnlNegativeColor; }
            set { _dayPnlNegativeColor = value; }
        }

        private string _rowColorbasis = "0"; //tag for orderside 
        public string RowColorbasis
        {
            get { return _rowColorbasis; }
            set { _rowColorbasis = value; }
        }

        private bool _showGridlines = true;
        public bool ShowGridLines
        {
            get { return _showGridlines; }
            set { _showGridlines = value; }
        }

        private bool _wrapHeader = false;
        public bool WrapHeader
        {
            get { return _wrapHeader; }
            set { _wrapHeader = value; }
        }

        private decimal _fontSizeGrid = 13.00M;
        public decimal FontSizeGrid
        {
            get { return _fontSizeGrid; }
            set { _fontSizeGrid = value; }
        }

        private decimal _fontSizeDashboard = 11.00M;
        public decimal FontSizeDashboard
        {
            get { return _fontSizeDashboard; }
            set { _fontSizeDashboard = value; }
        }

        private int _foreColor1 = -16777216; //Top level summary header color : White
        public int ForeColor1
        {
            get { return _foreColor1; }
            set { _foreColor1 = value; }
        }

        private int _foreColor2 = -16777216; //2nd level summary header color : Orange
        public int ForeColor2
        {
            get { return _foreColor2; }
            set { _foreColor2 = value; }
        }

        private int _foreColor3 = -16777216; //3rd level summary header color : Golden Rod
        public int ForeColor3
        {
            get { return _foreColor3; }
            set { _foreColor3 = value; }
        }
        private int _foreColor4 = -16777216; //3rd level summary header color : Golden Rod
        public int ForeColor4
        {
            get { return _foreColor4; }
            set { _foreColor4 = value; }
        }
        private int _backColor1 = -1578775; // 1st Level Summary Back Color : Gray
        public int BackColor1
        {
            get { return _backColor1; }
            set { _backColor1 = value; }
        }

        private int _backColor2 = -1578775;  // 2nd Level Summary Back Color : Purple
        public int BackColor2
        {
            get { return _backColor2; }
            set { _backColor2 = value; }
        }

        private int _backColor3 = -1578775;  // 3rd Level Summary Back Color : Gray
        public int BackColor3
        {
            get { return _backColor3; }
            set { _backColor3 = value; }
        }

        private int _backColor4 = -1578775;  // 4nd Level Summary Back Color : Purple
        public int BackColor4
        {
            get { return _backColor4; }
            set { _backColor4 = value; }
        }

        private int _summaryColor = -16777216;  //Summay Fore Color : White
        public int SummaryColor
        {
            get { return _summaryColor; }
            set { _summaryColor = value; }
        }

        private int _alternateColor = -3418400; //Alternate row background color : light Gray
        public int AlternateColor
        {
            get { return _alternateColor; }
            set { _alternateColor = value; }
        }

        private int _rowSelectorBackcolor = -1578775; //Row Selector Back Color **
        public int RowSelectorBackColor
        {
            get { return _rowSelectorBackcolor; }
            set { _rowSelectorBackcolor = value; }
        }

        private int _rowSelectorForcolor = -16777216; //Row Selector Fore Color **
        public int RowSelectorForColor
        {
            get { return _rowSelectorForcolor; }
            set { _rowSelectorForcolor = value; }
        }

        private bool _showGridlinesbyGroup = true;// Enable Grid by Group Line
        public bool ShowGridLinesbyGroup
        {
            get { return _showGridlinesbyGroup; }
            set { _showGridlinesbyGroup = value; }
        }

        private bool _showNegativeValuesWithBrackets = false; //Show negative values with brackets like -19.3 = (19.3)
        public bool ShowNegativeValuesWithBrackets
        {
            get { return _showNegativeValuesWithBrackets; }
            set { _showNegativeValuesWithBrackets = value; }
        }

        private string _defaultselectedView = null;
        public string DefaultSelectedView
        {
            get { return _defaultselectedView; }
            set { _defaultselectedView = value; }
        }

        private SerializableDictionary<string, int> _decimalPlaceLimitsForColumns = new SerializableDictionary<string, int>();
        public SerializableDictionary<string, int> DecimalPlaceLimitsForColumns
        {
            get { return _decimalPlaceLimitsForColumns; }

            set { _decimalPlaceLimitsForColumns = value; }
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
                if (_decimalPlaceLimitsForColumns != null)
                    _decimalPlaceLimitsForColumns.Clear();
                _decimalPlaceLimitsForColumns = null;
            }
        }
        #endregion



        public bool IsDefaultRowBackColor { get; set; }

        public bool IsDefaultAlternateRowColor { get; set; }

        public bool IsDefaultDashboardFontSize { get; set; }

        public bool IsDefaultGridFontSize { get; set; }

        public bool IsDefaultGroupingColor { get; set; }
    }


}
