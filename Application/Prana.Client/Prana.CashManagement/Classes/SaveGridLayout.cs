using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.CashManagement.Classes
{
    public class SaveGridLayout : IDisposable
    {
        #region Members

        /// <summary>
        /// The cash management layout
        /// </summary>
        private CashManagementLayout _cashManagementLayout = null;

        #endregion Members

        #region Methods

        /// <summary>
        /// Gets the layout.
        /// </summary>
        /// <param name="grd">The GRD.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public CashManagementLayout GetLayout(UltraGrid grd, string key)
        {
            try
            {
                _cashManagementLayout = new CashManagementLayout();

                if (_cashManagementLayout != null)
                {
                    _cashManagementLayout.TabName = key;
                    List<CashGridColumn> colsCollection = new List<CashGridColumn>();

                    foreach (UltraGridColumn column in grd.DisplayLayout.Bands[0].Columns)
                    {

                        if (!column.Hidden || grd.DisplayLayout.Bands[0].ColumnFilters.Exists(column.Key))
                        {

                            List<FilterCondition> filterConditionList = new List<FilterCondition>();
                            FilterLogicalOperator logicalOperator = FilterLogicalOperator.And;
                            FilterConditionsCollection filterConditionsColl = grd.DisplayLayout.Bands[0].ColumnFilters[column].FilterConditions;
                            logicalOperator = grd.DisplayLayout.Bands[0].ColumnFilters[column].LogicalOperator;

                            foreach (FilterCondition filterCond in filterConditionsColl)
                            {
                                FilterCondition filterCondClone = new FilterCondition(filterCond.ComparisionOperator, filterCond.CompareValue);
                                if (filterCondClone.CompareValue != null)
                                {
                                    Type type = filterCondClone.CompareValue.GetType();
                                    if (type.BaseType.Equals(typeof(System.Enum)))
                                    {
                                        filterCondClone.CompareValue = filterCondClone.CompareValue.ToString();
                                    }
                                    if (((column.Key.Equals(CashManagementConstants.COLUMN_TRADEDATE) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing))) || (column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing))) || (column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing)))
                                        || (column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing))) || (column.Key.Equals(CashManagementConstants.COLUMN_DATE) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing)))) && filterCondClone.ComparisionOperator == FilterComparisionOperator.StartsWith)
                                    {
                                        filterCondClone.CompareValue = "(Today)";
                                    }
                                    filterConditionList.Add(filterCondClone);
                                }
                            }

                            SortIndicator sort = column.SortIndicator;
                            CashGridColumn prefCol = new CashGridColumn(column.Key, column.Header.VisiblePosition, column.Width, column.Header.Fixed, filterConditionList, column.DataType.ToString(), logicalOperator, sort, column.Hidden);
                            colsCollection.Add(prefCol);

                        }

                        colsCollection.Sort();

                        _cashManagementLayout.SelectedColumnsCollection = colsCollection;

                    }

                    //Set GroupBy Columns in Grid
                    for (int count = 0; count < grd.DisplayLayout.Bands[0].SortedColumns.Count; count++)
                    {
                        if (grd.DisplayLayout.Bands[0].SortedColumns[count].IsGroupByColumn)
                        {
                            string[] groupByColName = grd.DisplayLayout.Bands[0].SortedColumns[count].ToString().Split(' ');
                            _cashManagementLayout.GroupByColumnsCollection.Add(groupByColName[0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _cashManagementLayout.TabName = key;
            }
            return _cashManagementLayout;
        }

        #endregion Methods

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
                _cashManagementLayout.Dispose();
            }
        }
        #endregion IDisposable Members
    }
}
