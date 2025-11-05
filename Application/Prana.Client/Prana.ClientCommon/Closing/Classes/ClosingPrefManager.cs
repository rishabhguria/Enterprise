using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.ClientCommon
{
    public class ClosingPrefManager
    {
        static int _userID = int.MinValue;
        static string _startPath = string.Empty;
        static string _closingLayoutFilePath = string.Empty;
        static string _closingTemplatePath = string.Empty;
        static string _closingTemplateDirectoryPath = string.Empty;
        static string _closingLayoutDirectoryPath = string.Empty;
        private static ProxyBase<IClosingServices> _closingServices = null;

        private static void CreateClosingServicesProxy()
        {
            _closingServices = new ProxyBase<IClosingServices>("TradeClosingServiceEndpointAddress");
        }

        static ClosingLayout _closingLayout = null;
        public static ClosingLayout ClosingLayout
        {
            get
            {
                if (_closingLayout == null)
                {
                    _closingLayout = GetClosingLayout();
                }
                return _closingLayout;
            }
        }

        private static ClosingLayout GetClosingLayout()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            _closingLayoutDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID.ToString();
            _closingLayoutFilePath = _closingLayoutDirectoryPath + @"\ClosingLayout.xml";

            ClosingLayout closingLayout = new ClosingLayout();
            try
            {
                if (!Directory.Exists(_closingLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_closingLayoutDirectoryPath);
                }
                if (File.Exists(_closingLayoutFilePath))
                {
                    using (FileStream fs = File.OpenRead(_closingLayoutFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ClosingLayout));
                        closingLayout = (ClosingLayout)serializer.Deserialize(fs);
                    }
                }

                _closingLayout = closingLayout;
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

            return closingLayout;
        }

        public static void SaveClosingLayout()
        {
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(_closingLayoutFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(ClosingLayout));
                    serializer.Serialize(writer, _closingLayout);

                    writer.Flush();
                    // writer.Close();
                }
            }
            #region catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }
        public static void Dispose()
        {
            _closingLayout = null;
            _dictClosingTemplates = null;
        }

        public static List<ColumnData> GetGridColumnLayout(UltraGrid grid)
        {
            List<ColumnData> listGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            try
            {
                foreach (UltraGridColumn gridCol in band.Columns)
                {
                    ColumnData colData = new ColumnData();
                    colData.Key = gridCol.Key;
                    colData.Caption = gridCol.Header.Caption;
                    colData.Format = gridCol.Format;
                    colData.Hidden = gridCol.Hidden;
                    colData.VisiblePosition = gridCol.Header.VisiblePosition;
                    colData.Width = gridCol.Width;
                    colData.ExcludeFromColumnChooser = gridCol.ExcludeFromColumnChooser;
                    colData.IsGroupByColumn = gridCol.IsGroupByColumn;
                    colData.Fixed = gridCol.Header.Fixed;
                    colData.CellActivation = gridCol.CellActivation;

                    // Sorted Columns
                    colData.SortIndicator = gridCol.SortIndicator;

                    //// Summary Settings
                    //if (band.Summaries.Exists(gridCol.Key))
                    //{
                    //    string colSummKey = band.Summaries[gridCol.Key].CustomSummaryCalculator.ToString();
                    //    colData.ColSummaryKey = (colSummKey.Contains(".")) ? colSummKey.Split('.')[2] : String.Empty;
                    //    colData.ColSummaryFormat = band.Summaries[gridCol.Key].DisplayFormat;
                    //}

                    //Filter Settings
                    foreach (FilterCondition fCond in band.ColumnFilters[gridCol.Key].FilterConditions)
                    {
                        FilterCondition filterCondClone = new FilterCondition(fCond.ComparisionOperator, fCond.CompareValue);
                        if (((gridCol.Key.Equals(ApplicationConstants.CONST_COLTradeDate) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing))) || (gridCol.Key.Equals(ApplicationConstants.CONST_PROCESSDATE) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing))) || (gridCol.Key.Equals(ApplicationConstants.CONST_COLClosingTradeDate) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing)))) && filterCondClone.ComparisionOperator == FilterComparisionOperator.StartsWith)
                        {
                            filterCondClone.CompareValue = "(Today)";
                        }
                        colData.FilterConditionList.Add(filterCondClone);
                    }
                    colData.FilterLogicalOperator = band.ColumnFilters[gridCol.Key].LogicalOperator;

                    listGridCols.Add(colData);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return listGridCols;
        }

        public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        {
            List<ColumnData> listSortedGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            ColumnsCollection gridColumns = band.Columns;// Just for readability ;)
            listColData.Sort();

            try
            {
                // Hide All
                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    gridCol.Hidden = true;
                }

                //Set Columns Properties
                foreach (ColumnData colData in listColData)
                {
                    if (gridColumns.Exists(colData.Key))
                    {
                        UltraGridColumn gridCol = gridColumns[colData.Key];
                        gridCol.Width = colData.Width;
                        //gridCol.Format = colData.Format;
                        //gridCol.Header.Caption = colData.Caption;
                        gridCol.Header.VisiblePosition = colData.VisiblePosition;
                        gridCol.Hidden = colData.Hidden;
                        gridCol.ExcludeFromColumnChooser = colData.ExcludeFromColumnChooser;
                        gridCol.Header.Fixed = colData.Fixed;
                        gridCol.SortIndicator = colData.SortIndicator;
                        gridCol.CellActivation = Activation.NoEdit;

                        // Sorted Columns
                        if (colData.SortIndicator == SortIndicator.Descending || colData.SortIndicator == SortIndicator.Ascending)
                        {
                            listSortedGridCols.Add(colData);
                        }

                        //Summary Settings
                        //if (colData.ColSummaryKey != String.Empty)
                        //{
                        //    SummarySettings summary = band.Summaries.Add(gridCol.Key, SummaryType.Custom, riskSummFactory.GetSummaryCalculator(colData.ColSummaryKey), gridCol, SummaryPosition.UseSummaryPositionColumn, gridCol);
                        //    summary.DisplayFormat = colData.ColSummaryFormat;
                        //}

                        // Filter Settings
                        if (colData.FilterConditionList.Count > 0)
                        {
                            band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                            foreach (FilterCondition fCond in colData.FilterConditionList)
                            {
                                if ((colData.Key.Equals(ApplicationConstants.CONST_COLTradeDate) || colData.Key.Equals(ApplicationConstants.CONST_PROCESSDATE) || colData.Key.Equals(ApplicationConstants.CONST_COLClosingTradeDate)) && colData.FilterConditionList.Count == 1 && colData.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.StartsWith && colData.FilterConditionList[0].CompareValue.Equals("(Today)"))
                                {
                                    band.ColumnFilters[colData.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                                }
                                else
                                {
                                    band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                                }
                            }
                        }
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            // Sorted Columns are returned as they need to be handled after data is binded.
            //  return listSortedGridCols;
        }



        public static void SetUp(int userId, string startUpPath)
        {
            _userID = userId;
            _startPath = startUpPath;
        }

        private static DateTime _closeTradeDate;

        /// <summary>
        /// Gets or sets the date time value to Todate.
        /// </summary>
        /// <value>The date time value.</value>
        public static DateTime CloseTradeDate
        {
            get { return _closeTradeDate; }
            set
            {
                _closeTradeDate = value;
            }
        }

        //private static int _id;

        private static DateTime _fromDate;

        /// <summary>
        /// Gets or sets the date time value.
        /// </summary>
        /// <value>The date time value.</value>
        public static DateTime FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
            }
        }
        private static PostTradeEnums.CloseTradeMethodology _defaultMethodology;

        /// <summary>
        /// Gets or sets the default methodology.
        /// </summary>
        /// <value>The default methodology.</value>
        public static PostTradeEnums.CloseTradeMethodology DefaultMethodology
        {
            get { return _defaultMethodology; }
            set
            {
                _defaultMethodology = value;

            }
        }

        private static PostTradeEnums.CloseTradeAlogrithm _algorithm;

        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>The algorithm.</value>
        public static PostTradeEnums.CloseTradeAlogrithm Algorithm
        {
            get { return _algorithm; }
            set
            {
                _algorithm = value;

            }
        }


        private static PostTradeEnums.SecondarySortCriteria _secondarySort;

        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>The algorithm.</value>
        public static PostTradeEnums.SecondarySortCriteria SecondarySort
        {
            get { return _secondarySort; }
            set
            {
                _secondarySort = value;

            }
        }

        private static bool _isShortWithBuyAndBuyToCover = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is short with buy and buy to cover.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is short with buy and buy to cover; otherwise, <c>false</c>.
        /// </value>
        public static bool IsShortWithBuyAndBuyToCover
        {
            get { return _isShortWithBuyAndBuyToCover; }
            set { _isShortWithBuyAndBuyToCover = value; }
        }

        private static bool _isSellWithBuyToClose = false;


        public static bool IsSellWithBuyToClose
        {
            get { return _isSellWithBuyToClose; }
            set { _isSellWithBuyToClose = value; }
        }
        private static bool _isCurrentDateClosing = true;

        public static bool IsCurrentDateClosing
        {
            get { return _isCurrentDateClosing; }
            set { _isCurrentDateClosing = value; }
        }

        private static PostTradeEnums.ClosingField _closingField;

        /// <summary>
        /// Gets or sets the closing field.
        /// </summary>
        /// <value>closing field.</value>
        public static PostTradeEnums.ClosingField ClosingField
        {
            get { return _closingField; }
            set { _closingField = value; }
        }


        //protected static override object GetIdValue()
        //{
        //    return _id;
        //}

        private static string _comments;

        /// <summary>
        /// Gets or sets the Comments value.
        /// </summary>
        /// <value>The Comments value.</value>
        public static string Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;

            }
        }

        public static ClosingPreferences GetPreferences()
        {
            ClosingPreferences preferences = null;
            try
            {
                CreateClosingServicesProxy();
                //AccountingMethods accountingMethods = new AccountingMethods(CachedDataManager.GetInstance.GetAccounts(), CachedDataManager.GetInstance.GetAllClosingAssets());
                preferences = _closingServices.InnerChannel.GetPreferences();
                //accountingMethods.Update(preferences.AccountingMethodsTable.Tables[0]);
                _closingServices.Dispose();
                //if (preferences == null)
                //{
                //    preferences = GetDefualtPreferences();
                //}
                // _closingPreferencesMain = preferences;

                return preferences;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }
                return null;
            }
            #endregion

        }


        //private static ClosingPreferences GetPreferences()
        //{
        //    ClosingPreferences preferences = null;
        //    try
        //    {
        //        CreateClosingServicesProxy();
        //        AccountingMethods accountingMethods = new AccountingMethods(CachedDataManager.GetInstance.GetAccounts(), CachedDataManager.GetInstance.GetAllClosingAssets());
        //         preferences   = _closingServices.InnerChannel.GetPreferences();
        //        accountingMethods.Update(preferences.AccountingMethods.AccountingMethodsTable.Tables[0]);
        //        _closingServices.Dispose();

        //    }

        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        //if(fs!=null)
        //        //    fs.Close();
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return accountingMethods;
        //}

        public static ClosingPreferences GetDefualtPreferences()
        {
            ClosingPreferences closingPreferences = new ClosingPreferences();
            try
            {
                closingPreferences.IsFetchDataAutomatically = true;
                closingPreferences.ClosingMethodology = GetDefaultClosingMethodology();

                //AccountingMethods accountingMethods = new AccountingMethods(CachedDataManager.GetInstance.GetAccounts(), CachedDataManager.GetInstance.GetAllClosingAssets());
                //closingPreferences.AccountingMethods = accountingMethods;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return closingPreferences;
        }

        public static ClosingMethodology GetDefaultClosingMethodology()
        {
            ClosingMethodology defaultMehtodology = new ClosingMethodology();
            try
            {
                defaultMehtodology.IsShortWithBuyandBTC = false;
                defaultMehtodology.IsSellWithBTC = false;
                defaultMehtodology.OverrideGlobal = false;
                defaultMehtodology.ClosingAlgo = PostTradeEnums.CloseTradeAlogrithm.FIFO;
                defaultMehtodology.ClosingField = PostTradeEnums.ClosingField.Default;
                DataSet ds = new DataSet();
                AccountingMethods.SetDefaultTableAndSchema(CachedDataManager.GetInstance.GetAccounts(), CachedDataManager.GetInstance.GetAllClosingAssets(), ds);
                defaultMehtodology.AccountingMethodsTable = ds;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return defaultMehtodology;
        }


        public static void SavePreferences(ClosingPreferences closingPreferences, bool isApplyOnly)
        {
            try
            {
                CreateClosingServicesProxy();
                // _closingPreferencesMain = closingPreferences;
                //_Xml.WriteFile(closingPreferences, _closingPreferencesFilePath, true);
                if (isApplyOnly)
                {
                    //_closingServices.InnerChannel.UpdatePreferences(closingPreferences);
                }
                else
                {
                    _closingServices.InnerChannel.SavePreferences(closingPreferences);
                }

                _closingServices.Dispose();

            }

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                //if(fs!=null)
                //    fs.Close();
                if (rethrow)
                {
                    throw;
                }
            }

        }
        //public static ClosingPreferences ClosingPreferences
        //{
        //    get
        //    {
        //        if (_closingPreferencesMain == null)
        //        {
        //            _closingPreferencesMain = GetPreferences();
        //        }
        //        return _closingPreferencesMain;
        //    }
        //}


        public static void SaveClosingTemplates()
        {
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(_closingTemplatePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(SerializableDictionary<string, List<ClosingTemplate>>));
                    serializer.Serialize(writer, _dictClosingTemplates);

                    writer.Flush();
                    //writer.Close();
                }
            }
            #region catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

        }




        static SerializableDictionary<string, List<ClosingTemplate>> _dictClosingTemplates = null;

        static public SerializableDictionary<string, List<ClosingTemplate>> DictClosingTemplates
        {
            get
            {

                if (_dictClosingTemplates == null || _dictClosingTemplates.Count == 0)
                {
                    GetExistingTemplates();
                }
                return _dictClosingTemplates;

            }
            set { _dictClosingTemplates = value; }
        }

        static public List<string> getRootTemplates()
        {
            List<string> listRootTemplates = new List<string>();
            try
            {
                listRootTemplates.Add(ClosingType.Closing.ToString());
                listRootTemplates.Add(ClosingType.UnWinding.ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return listRootTemplates;
        }



        public static void GetExistingTemplates()
        {
            try
            {
                _closingTemplateDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME;
                _closingTemplatePath = _closingTemplateDirectoryPath + @"\ClosingTemplates.xml";

                if (!Directory.Exists(_closingTemplateDirectoryPath))
                {
                    Directory.CreateDirectory(_closingTemplateDirectoryPath);
                }
                if (File.Exists(_closingTemplatePath))
                {
                    using (FileStream fs = File.OpenRead(_closingTemplatePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, List<ClosingTemplate>>));
                        _dictClosingTemplates = (SerializableDictionary<string, List<ClosingTemplate>>)serializer.Deserialize(fs);
                    }
                }
                if (_dictClosingTemplates == null)
                {
                    _dictClosingTemplates = new SerializableDictionary<string, List<ClosingTemplate>>();
                }

                //_closingLayout = closingLayout;
                //}
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }




        public static List<ClosingTemplate> GetClosingTemplates(string closingType, string templateName)
        {

            List<ClosingTemplate> templates = new List<ClosingTemplate>();
            try
            {
                if (_dictClosingTemplates == null || _dictClosingTemplates.Count == 0)
                {
                    GetExistingTemplates();
                }

                if (_dictClosingTemplates != null)
                {
                    if (_dictClosingTemplates.ContainsKey(closingType))
                    {
                        //fetch template for given template name...
                        if (!string.IsNullOrEmpty(templateName))
                        {
                            foreach (ClosingTemplate closingTemplate in _dictClosingTemplates[closingType])
                            {
                                if (closingTemplate.TemplateName.Equals(templateName))
                                {
                                    templates.Add(closingTemplate);
                                    break;
                                }

                            }
                        }
                        else
                        {
                            //fetch all templates for given root template type...
                            templates.AddRange(_dictClosingTemplates[closingType]);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                //if(fs!=null)
                //    fs.Close();
                if (rethrow)
                {
                    throw;
                }
            }

            return templates;
        }

        //Add default template when no preference are saved
        public static void AddTemplate(string closingType, string templateName)
        {
            ClosingTemplate template = null;
            List<ClosingTemplate> closingTemplates = new List<ClosingTemplate>();
            try
            {
                if (_dictClosingTemplates != null)
                {
                    if (_dictClosingTemplates.ContainsKey(closingType))
                    {
                        template = new ClosingTemplate();
                        template.TemplateName = templateName;
                        template.ClosingType = (ClosingType)(closingType == "Closing" ? 0 : 1);
                        closingTemplates = _dictClosingTemplates[closingType];
                        foreach (ClosingTemplate closingTemplate in closingTemplates)
                        {
                            if (string.Equals(templateName, closingTemplate.TemplateName))
                            {
                                return;
                            }

                        }
                        _dictClosingTemplates[closingType].Add(template);
                    }
                    else
                    {
                        template = new ClosingTemplate();

                        template.ClosingType = (ClosingType)Enum.Parse(typeof(ClosingType), closingType);
                        template.TemplateName = templateName;
                        closingTemplates.Add(template);

                        _dictClosingTemplates.Add(closingType, closingTemplates);
                    }
                    LoadDefaultDataForTemplate(template);
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

        public static void RemoveTemplate(string closingType, string templateName)
        {
            try
            {
                if (_dictClosingTemplates.ContainsKey(closingType))
                {
                    List<ClosingTemplate> listTemplates = _dictClosingTemplates[closingType];
                    ClosingTemplate templateToRemove = null;

                    foreach (ClosingTemplate template in listTemplates)
                    {
                        if (template.TemplateName.Equals(templateName))
                        {

                            templateToRemove = template;
                            break;
                        }
                    }

                    if (templateToRemove != null)
                    {
                        listTemplates.Remove(templateToRemove);
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

        public static void LoadDefaultDataForTemplate(ClosingTemplate template)
        {
            try
            {
                //FillStandardFilterData(template);
                template.ClosingMeth = GetDefaultClosingMethodology();
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

        //private static void FillStandardFilterData(ClosingTemplate template)
        //{
        //    //List<int> listAsset = new List<int>();
        //    //List<int> listAccount = new List<int>();
        //    //List<int> listMasterFund = new List<int>();
        //    //Dictionary<int, string> dictAssets = CommonDataCache.CachedDataManager.GetInstance.GetAllAssets();
        //    //Dictionary<int, string> dictAccounts = CommonDataCache.CachedDataManager.GetInstance.GetAccountsWithFullName();
        //    //Dictionary<int, string> dictMasterFunds = CachedDataManager.GetInstance.GetAllMasterFunds();
        //    //foreach (int key in dictAssets.Keys)
        //    //{
        //    //    listAsset.Add(key);
        //    //}
        //    //template.ListAssetFilters = listAsset;
        //    //foreach (int key in dictAccounts.Keys)
        //    //{
        //    //    listAccount.Add(key);
        //    //}
        //    //template.ListAccountFliters = listAccount;
        //    //foreach (int key in dictMasterFunds.Keys)
        //    //{
        //    //    listMasterFund.Add(key);
        //    //}
        //    //template.ListMasterFundFilters = listMasterFund;
        //}


        public static void DeleteTemplates(string closingType, string templateName)
        {
            try
            {
                if (_dictClosingTemplates.ContainsKey(closingType))
                {
                    List<ClosingTemplate> list = _dictClosingTemplates[closingType];
                    foreach (ClosingTemplate template in list)
                    {
                        if (template.TemplateName.Equals(templateName))
                            _dictClosingTemplates[closingType].Remove(template);
                    }
                }
            }
            #region catch
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
            #endregion

        }

        public static void UpdateTemplateName(string closingType, string prevTemplateName, string templateName)
        {
            try
            {
                if (_dictClosingTemplates != null && _dictClosingTemplates.ContainsKey(closingType))
                {
                    List<ClosingTemplate> list = _dictClosingTemplates[closingType];
                    foreach (ClosingTemplate template in list)
                    {
                        if (template.TemplateName.Equals(prevTemplateName))
                        {
                            template.TemplateName = templateName;
                            break;
                        }
                    }
                }
            }
            #region catch
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
            #endregion

        }

        public static bool CheckTemplateExists(string closingType, string templateName)
        {
            List<ClosingTemplate> closingTemplates = new List<ClosingTemplate>();
            try
            {
                if (_dictClosingTemplates != null && _dictClosingTemplates.ContainsKey(closingType))
                {
                    closingTemplates = _dictClosingTemplates[closingType];
                    foreach (ClosingTemplate closingTemplate in closingTemplates)
                        if (string.Equals(templateName, closingTemplate.TemplateName))
                            return true;
                }
                return false;
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
            return false;
        }

        public static void UpdateTemplates(ClosingTemplate Template)
        {
            try
            {
                string key = Template.TemplateName;
                List<ClosingTemplate> listTemplates = new List<ClosingTemplate>();
                string closingType = Template.ClosingType.ToString();
                if (_dictClosingTemplates.ContainsKey(closingType))
                {
                    listTemplates = _dictClosingTemplates[closingType];
                    foreach (ClosingTemplate template in listTemplates)
                    {
                        if (template.TemplateName == key)
                        {
                            listTemplates.Remove(template);
                            listTemplates.Add(Template);
                            break;
                        }
                    }
                }
                else
                {
                    listTemplates.Add(Template);
                    _dictClosingTemplates.Add(closingType, listTemplates);
                }
            }
            #region catch
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
            #endregion

        }

        // Added By : Manvendra Prajapati
        // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8746
        /// <summary>
        /// Get caption based on the closing date type server preferences
        /// </summary>
        /// <returns></returns>
        public static string GetCaptionBasedonClosingDateType()
        {
            string caption = string.Empty;
            try
            {
                if (_closingServices == null)
                {
                    CreateClosingServicesProxy();
                }
                PostTradeEnums.DateType dateType = _closingServices.InnerChannel.GetClosingDateType();
                switch (dateType)
                {
                    case PostTradeEnums.DateType.AuecLocalDate:
                        caption = "Trade Date";
                        break;
                    case PostTradeEnums.DateType.ProcessDate:
                        caption = "Process Date";
                        break;
                    case PostTradeEnums.DateType.OriginalPurchaseDate:
                        caption = "Original Purchase Date";
                        break;
                    default:
                        caption = "Original Purchase Date";
                        break;
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
            return caption;
        }
    }

}

