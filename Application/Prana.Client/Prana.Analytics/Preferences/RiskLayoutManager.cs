using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.ClientCommon;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Analytics
{
    class RiskLayoutManager
    {
        static int _userID = int.MinValue;
        static string _startPath = string.Empty;
        //static string _riskLayoutPath = string.Empty;
        static string _riskLayoutFilePath = string.Empty;
        // static string _riskDefaultLayoutFilePath = string.Empty;
        static string _riskLayoutDirectoryPath = string.Empty;
        static RiskLayout _riskLayout = null;

        public static void Dispose()
        {
            _riskLayout = null;
        }

        public static void SetUp(string startUpPath)
        {
            _startPath = startUpPath;
        }

        public static RiskLayout RiskLayout
        {
            get
            {
                if (_riskLayout == null)
                {
                    _riskLayout = GetRiskLayout();
                }
                return _riskLayout;
            }
        }

        private static RiskLayout GetRiskLayout()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            _riskLayoutDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID.ToString();
            _riskLayoutFilePath = _riskLayoutDirectoryPath + @"\RiskLayout.xml";

            RiskLayout riskLayout = new RiskLayout();
            try
            {
                if (!Directory.Exists(_riskLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_riskLayoutDirectoryPath);
                }
                if (File.Exists(_riskLayoutFilePath))
                {
                    using (FileStream fs = File.OpenRead(_riskLayoutFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(RiskLayout));
                        riskLayout = (RiskLayout)serializer.Deserialize(fs);
                    }
                }

                _riskLayout = riskLayout;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return riskLayout;
        }

        public static void SaveRiskLayout()
        {
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(_riskLayoutFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(RiskLayout));
                    serializer.Serialize(writer, _riskLayout);

                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static void SaveDefaultStepAnalysisLayout(StepAnalLayout stepAnalLayout)
        {
            try
            {
                RiskLayout.SaveDefaultStepAnalysisLayout(stepAnalLayout, _userID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static List<ColumnData> GetGridColumnLayout(UltraGrid grid)
        {
            List<ColumnData> listGridCols = new List<ColumnData>();

            try
            {
                bool isGroupByColExist = false;
                foreach (UltraGridColumn sortedCol in grid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (sortedCol.IsGroupByColumn)
                    {
                        isGroupByColExist = true;
                        break;
                    }
                }

                foreach (UltraGridColumn gridCol in grid.DisplayLayout.Bands[0].Columns)
                {
                    ColumnData colData = new ColumnData();
                    colData.Key = gridCol.Key;
                    colData.Hidden = gridCol.Hidden;
                    colData.VisiblePosition = gridCol.Header.VisiblePosition;
                    colData.Width = gridCol.Width;
                    colData.IsGroupByColumn = gridCol.IsGroupByColumn;
                    colData.Fixed = gridCol.Header.Fixed;

                    //Remove sorting for other columns if group by column exists
                    if (isGroupByColExist && !gridCol.IsGroupByColumn)
                    {
                        colData.SortIndicator = SortIndicator.None;
                    }
                    else
                    {
                        colData.SortIndicator = gridCol.SortIndicator;
                    }

                    //Filter Settings
                    foreach (FilterCondition fCond in grid.DisplayLayout.Bands[0].ColumnFilters[gridCol.Key].FilterConditions)
                    {
                        FilterCondition filterCondClone = new FilterCondition(fCond.ComparisionOperator, fCond.CompareValue);
                        if (((gridCol.Key.Equals(ApplicationConstants.CONST_COLTradeDate) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing))) || (gridCol.Key.Equals(ApplicationConstants.CONST_EXPIRATIONDATE) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing)))) && filterCondClone.ComparisionOperator == FilterComparisionOperator.StartsWith)
                        {
                            filterCondClone.CompareValue = "(Today)";
                        }
                        colData.FilterConditionList.Add(filterCondClone);
                    }
                    colData.FilterLogicalOperator = grid.DisplayLayout.Bands[0].ColumnFilters[gridCol.Key].LogicalOperator;

                    listGridCols.Add(colData);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            listGridCols.Sort();
            return listGridCols;
        }

        public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listPrefColData, List<SortedColumnData> listGroupByColumnsCollection, List<string> listDisplayableColData)
        {
            try
            {
                ColumnsCollection gridColumns = grid.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    gridCol.Hidden = true;

                    if (!listDisplayableColData.Contains(gridCol.Key))
                    {
                        gridCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                }

                //Disabling header checkbox sorting
                grid.DisplayLayout.Bands[0].Columns["IsChecked"].SortIndicator = SortIndicator.Disabled;

                //Set Columns Properties
                foreach (ColumnData colData in listPrefColData)
                {
                    if (gridColumns.Exists(colData.Key))
                    {
                        UltraGridColumn gridCol = gridColumns[colData.Key];
                        gridCol.Width = colData.Width;
                        gridCol.Header.VisiblePosition = colData.VisiblePosition;
                        gridCol.Hidden = colData.Hidden;
                        gridCol.Header.Fixed = colData.Fixed;
                        if (colData.SortIndicator == SortIndicator.Ascending || colData.SortIndicator == SortIndicator.Descending)
                        {
                            grid.DisplayLayout.Bands[0].Columns[colData.Key].SortIndicator = colData.SortIndicator;
                        }

                        //Filter Settings
                        if (colData.FilterConditionList.Count > 0)
                        {
                            grid.DisplayLayout.Bands[0].ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                            foreach (FilterCondition fCond in colData.FilterConditionList)
                            {
                                if ((colData.Key.Equals(ApplicationConstants.CONST_COLTradeDate) || colData.Key.Equals(ApplicationConstants.CONST_EXPIRATIONDATE)) && colData.FilterConditionList.Count == 1 && colData.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.StartsWith && colData.FilterConditionList[0].CompareValue.Equals("(Today)"))
                                {
                                    grid.DisplayLayout.Bands[0].ColumnFilters[colData.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                                }
                                else
                                {
                                    grid.DisplayLayout.Bands[0].ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                                }
                            }
                        }
                    }
                }

                foreach (SortedColumnData sortedColData in listGroupByColumnsCollection)
                {
                    if (gridColumns.Exists(sortedColData.Key))
                    {
                        if (sortedColData.SortIndicator == SortIndicator.Ascending)
                        {
                            grid.DisplayLayout.Bands[0].SortedColumns.Add(sortedColData.Key, false, true);
                        }
                        else if (sortedColData.SortIndicator == SortIndicator.Descending)
                        {
                            grid.DisplayLayout.Bands[0].SortedColumns.Add(sortedColData.Key, true, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static void LoadColumnsWidthFromXML(UltraGrid grid, List<ColumnData> listPrefColData)
        {
            try
            {
                ColumnsCollection gridColumns = grid.DisplayLayout.Bands[0].Columns;
                foreach (ColumnData colData in listPrefColData)
                {
                    if (gridColumns.Exists(colData.Key))
                    {
                        UltraGridColumn gridCol = gridColumns[colData.Key];
                        gridCol.Width = colData.Width;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        public static List<SortedColumnData> GetGridGroupByColumnLayout(UltraGrid grdPositions)
        {
            List<SortedColumnData> columnGroubByList = new List<SortedColumnData>();
            try
            {
                foreach (UltraGridColumn sortedCol in grdPositions.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (sortedCol.IsGroupByColumn)
                    {
                        SortedColumnData sortedColumnData = new SortedColumnData();
                        sortedColumnData.Key = sortedCol.Key;
                        sortedColumnData.SortIndicator = sortedCol.SortIndicator;
                        columnGroubByList.Add(sortedColumnData);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return columnGroubByList;
        }
    }
}
