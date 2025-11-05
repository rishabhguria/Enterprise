using Infragistics.Win.UltraWinGrid;
using Prana.ClientCommon;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Tools
{
    class PricingLayoutManager
    {
        #region private variable
        private static int _userID = int.MinValue;
        private static string _startPath = string.Empty;
        private static string _pricingLayoutFilePath = string.Empty;
        private static string _pricingLayoutDirectoryPath = string.Empty;
        private static PricingLayout _pricingLayout = null;
        #endregion

        public static void Dispose()
        {
            _pricingLayout = null;
        }

        public static void SetUp(string startUpPath)
        {
            _startPath = startUpPath;
        }

        public static PricingLayout PricingLayout
        {
            get
            {
                if (_pricingLayout == null)
                {
                    _pricingLayout = GetPricingInputsLayout();
                }
                return _pricingLayout;
            }
        }

        private static PricingLayout GetPricingInputsLayout()
        {
            PricingLayoutManager.SetUp(System.Windows.Forms.Application.StartupPath);
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            _pricingLayoutDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID.ToString();
            _pricingLayoutFilePath = _pricingLayoutDirectoryPath + @"\PricingInputsGridLayout.xml";

            PricingLayout pricingLayout = new PricingLayout();
            try
            {
                if (!Directory.Exists(_pricingLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_pricingLayoutDirectoryPath);
                }
                if (File.Exists(_pricingLayoutFilePath))
                {
                    using (FileStream fs = File.OpenRead(_pricingLayoutFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(PricingLayout));
                        pricingLayout = (PricingLayout)serializer.Deserialize(fs);
                    }
                }

                _pricingLayout = pricingLayout;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return pricingLayout;
        }

        public static void SavePricingInputsLayout()
        {
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(_pricingLayoutFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(PricingLayout));
                    serializer.Serialize(writer, _pricingLayout);
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

        public static List<ColumnData> GetGridColumnLayout(UltraGrid grid)
        {
            List<ColumnData> listGridCols = new List<ColumnData>();

            try
            {
                foreach (UltraGridColumn gridCol in grid.DisplayLayout.Bands[0].Columns)
                {
                    ColumnData colData = new ColumnData();
                    colData.Key = gridCol.Key;
                    colData.Hidden = gridCol.Hidden;
                    colData.VisiblePosition = gridCol.Header.VisiblePosition;
                    colData.Width = gridCol.Width;
                    colData.IsGroupByColumn = gridCol.IsGroupByColumn;
                    colData.Fixed = gridCol.Header.Fixed;
                    colData.SortIndicator = gridCol.SortIndicator;

                    //Filter Settings
                    foreach (FilterCondition fCond in grid.DisplayLayout.Bands[0].ColumnFilters[gridCol.Key].FilterConditions)
                    {
                        colData.FilterConditionList.Add(fCond);
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

        public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listPrefColData, List<string> listDisplayableColData)
        {
            try
            {
                ColumnsCollection gridColumns = grid.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    if (!listDisplayableColData.Contains(gridCol.Key))
                    {
                        gridCol.Hidden = true;
                        gridCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                }

                //Set Columns Properties
                foreach (ColumnData colData in listPrefColData)
                {
                    if (gridColumns.Exists(colData.Key))
                    {
                        UltraGridColumn gridCol = gridColumns[colData.Key];
                        gridCol.Width = colData.Width;
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
                                grid.DisplayLayout.Bands[0].ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                            }
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
    }
}