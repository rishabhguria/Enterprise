using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;

namespace Prana.Analytics.Classes
{
    class GroupSortComparer : System.Collections.IComparer
    {
        private int _multiplier = -1;
        private string _columnName;
        public string Column
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private SortIndicator _sortIndicator = SortIndicator.Descending;
        public SortIndicator SortIndicator
        {
            get { return _sortIndicator; }
            set
            {
                _sortIndicator = value;
                switch (_sortIndicator)
                {
                    case SortIndicator.Ascending:
                        _multiplier = 1;
                        break;

                    case SortIndicator.Descending:
                    case SortIndicator.Disabled:
                    case SortIndicator.None:
                        _multiplier = -1;
                        break;

                    default:
                        break;
                }
            }
        }

        public int Compare(object xObj, object yObj)
        {
            try
            {
                UltraGridGroupByRow x = (UltraGridGroupByRow)xObj;
                UltraGridGroupByRow y = (UltraGridGroupByRow)yObj;
                IComparable xValue;
                IComparable yValue;

                if (Equals(xObj, yObj))
                {
                    return 0;
                }
                if (!(string.IsNullOrEmpty(_columnName)))
                {
                    if (x.Rows.SummaryValues[_columnName].Value == null)
                    {
                        return _multiplier;
                    }
                    if (y.Rows.SummaryValues[_columnName].Value == null)
                    {
                        return (-(_multiplier));
                    }
                    if (!x.Rows.SummaryValues[_columnName].Value.GetType().Equals(y.Rows.SummaryValues[_columnName].Value.GetType()))
                    {
                        if (x.Rows.SummaryValues[_columnName].Value is IComparable && y.Rows.SummaryValues[_columnName].Value is IComparable)
                        {
                            xValue = (IComparable)x.Rows.SummaryValues[_columnName].Value;
                            yValue = (IComparable)y.Rows.SummaryValues[_columnName].Value;
                            return xValue.ToString().CompareTo(yValue.ToString()) * _multiplier;
                        }
                        if (x.Rows.SummaryValues[_columnName].Value is IComparable)
                        {
                            return _multiplier;
                        }
                        if (y.Rows.SummaryValues[_columnName].Value is IComparable)
                        {
                            return (-(_multiplier));
                        }
                    }
                    else
                    {
                        xValue = (IComparable)x.Rows.SummaryValues[_columnName].Value;
                        yValue = (IComparable)y.Rows.SummaryValues[_columnName].Value;
                        return xValue.CompareTo(yValue) * _multiplier;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }
    }
}
