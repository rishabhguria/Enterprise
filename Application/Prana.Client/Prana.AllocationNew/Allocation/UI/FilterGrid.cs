using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.CommonDataCache;
using Infragistics.Win;
using System.IO;
using System.Xml.Serialization;
using Infragistics.Win.UltraWinEditors;
using Prana.BusinessObjects.AppConstants;
using Prana.Utilities.UIUtilities;
using Prana.Utilities;

namespace Prana.AllocationNew.Allocation.UI
{
    /// <summary>
    /// Filter grid functionality to improve prefecth performance
    /// </summary>
    public partial class FilterGrid : UserControl
    {
        #region Private Enums

        /// <summary>
        /// Default Column in application used for normalization. We use commondatacache to map the id with text. This is done for the these columns
        /// Here only those columns are used which contains mapping of name to ID
        /// </summary>
        private enum ColumnListForNormalization
        {
            [XmlEnumAttribute("Account Name")]
            AccountName,
            [XmlEnumAttribute("Strategy Name")]
            StrategyName,
            [XmlEnumAttribute("Side")]
            Side,
            [XmlEnumAttribute("Venue")]
            Venue,
            [XmlEnumAttribute("Counter Party")]
            CounterParty,
            [XmlEnumAttribute("Trading Account")]
            TradingAccount,
            [XmlEnumAttribute("Asset")]
            AssetCategory,
            [XmlEnumAttribute("Currency")]
            Currency,
            [XmlEnumAttribute("Exchange")]
            Exchange,
            [XmlEnumAttribute("Underlying")]
            Underlying,
        }

        #endregion

        #region Private members
        /// <summary>
        /// Internally maintained list of columns
        /// </summary>
        Dictionary<String, List<String>> _columnList = new Dictionary<string, List<string>>();
        /// <summary>
        /// Used while binding column names to filter grid. first band name is not shown on filter grid
        /// </summary>
        String _firstBandName = String.Empty;//
        /// <summary>
        /// Used to set name of filter grid
        /// </summary>
        String _filterTitle = String.Empty;
        /// <summary>
        /// Maintains status of Filter. Used while rendering controls.
        /// </summary>
        bool _iscardview = true;
        //TODO : _isCardView = False NOT HANDLED
        /// <summary>
        /// DataSource for _filterGrid
        /// </summary>
        DataTable _dtSourceFilter = new DataTable("TableFilter");//Datasource for grid.
        /// <summary>
        /// No of columns in a row in _filterGrid
        /// </summary>
        const int NoOfCols = 7;
        /// <summary>
        /// Tells whether the particular instance is a Combined filter or not.
        /// </summary>
        bool _isCombined = false;
        /// <summary>
        /// File path for saving of xmls.
        /// </summary>
        String _settingFilePath;//TODO : Could not load settings into grid. Saving Done
        /// <summary>
        /// Used to initialize default grid columns for first time.
        /// </summary>
        bool _isInitilizedFirstTime = true;

        List<AutoCompleteTextBox> _liAutoCompleteTextBox = new List<AutoCompleteTextBox>();

        UltraGridColumn _btColumnChooser;

        UltraGridColumn _btClearFilter;

        List<String> _liDefault= new List<string>();

        #endregion

        #region Properties

        /// <summary>
        /// Filter title
        /// </summary>
        public String FilterTitle
        {
            get { return _filterTitle; }
            set { _filterTitle = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default condtructor made private. So that no filter grid can be created without filter columns
        /// </summary>
        private FilterGrid()
        {
            try
            {

                InitializeComponent();
                _liDefault.Add("Symbol");//Default columns to be loaded in grid at starting
                _liDefault.Add("Side");
                _liDefault.Add("Account Name");
                _liDefault.Add("Asset");
                _liDefault.Add("Currency");
                _filterGrid.AfterColPosChanged += new AfterColPosChangedEventHandler(_filterGrid_AfterColPosChanged);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Initializes filters on the basis of list of columns passed to it. All columns will be assigned to band 0.
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="columnList">List of columns which will be assigned to Band0</param>
        public FilterGrid(String key, List<String> columnList)
            : this()
        {
            try
            {

                _firstBandName = key;
                _columnList.Add(key, columnList);
                LoadColumnsInGrid();



                _settingFilePath = Application.StartupPath + "\\GridSettings\\" + key + ".xml";

                if (Directory.Exists(_settingFilePath.Substring(0, _settingFilePath.LastIndexOf("\\"))))
                {
                    if (File.Exists(_settingFilePath))
                        _filterGrid.DisplayLayout.LoadFromXml(_settingFilePath, PropertyCategories.All);
                }
                else
                    Directory.CreateDirectory(_settingFilePath.Substring(0, _settingFilePath.LastIndexOf("\\")));


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Event handlers


        /// <summary>
        /// Event for redrawing filterGrid when a new column is added or removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _filterGrid_AfterColPosChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                if (e.PosChanged == PosChanged.HiddenStateChanged)
                {
                    RowLayout defRowLayout = _filterGrid.DisplayLayout.Bands[0].RowLayouts["LayOut"];
                    SetDefaultColumnPositions(defRowLayout.ColumnInfos);
                    defRowLayout.RowLayoutLabelStyle = RowLayoutLabelStyle.WithCellData;
                    defRowLayout.CardViewStyle = CardStyle.StandardLabels;
                    defRowLayout.RowLayoutLabelPosition = LabelPosition.Left;
                    defRowLayout.Apply();

                    ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterGrid));
                    _btColumnChooser.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _btColumnChooser.Header.VisiblePosition = 0;
                    _btColumnChooser.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    _btColumnChooser.Width = 18;
                    _btColumnChooser.RowLayoutColumnInfo.LabelPosition = LabelPosition.Left;
                    _btColumnChooser.TipStyleCell = TipStyle.Show;
                    _btColumnChooser.CellAppearance.Image = ((System.Drawing.Image)resources.GetObject("btnColumnChooser.Image"));
                    _btColumnChooser.CellButtonAppearance.Image = ((System.Drawing.Image)resources.GetObject("btnColumnChooser.Image"));

                    _btClearFilter.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _btClearFilter.Header.VisiblePosition = 1;
                    _btClearFilter.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    _btClearFilter.Width = 18;
                    _btClearFilter.RowLayoutColumnInfo.LabelPosition = LabelPosition.None;
                    _btClearFilter.TipStyleCell = TipStyle.Show;
                    _btClearFilter.CellAppearance.Image = ((System.Drawing.Image)resources.GetObject("Clear"));
                    _btClearFilter.CellButtonAppearance.Image = ((System.Drawing.Image)resources.GetObject("Clear"));
                    _filterGrid.Refresh();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Event handler for Clearbutton and choose button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _filterGrid_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "ClearButton")
                {
                    _filterGrid.BeginUpdate();
                    string[] ndtrow = new string[_dtSourceFilter.Columns.Count];
                    _dtSourceFilter.Rows.Clear();
                    _dtSourceFilter.Rows.Add(ndtrow);
                    _filterGrid.DataSource = _dtSourceFilter;
                    _filterGrid.EndUpdate();
                }
                if (e.Cell.Column.Key == _filterTitle)
                {
                    //Modified By Faisal Shah 08/07/14
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-4438
                    _filterGrid.ShowColumnChooser("Choose columns for " + _filterTitle, false);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void UnWireEvents()
        {
            foreach (AutoCompleteTextBox at in _liAutoCompleteTextBox)
            {
                at.Dispose();
            }
            _filterGrid.AfterColPosChanged -= new AfterColPosChangedEventHandler(_filterGrid_AfterColPosChanged);
            _filterGrid.ClickCellButton -= new CellEventHandler(_filterGrid_ClickCellButton);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Load autocomplete textboxes for each cell
        /// </summary>
        /// <param name="container">Containing form</param>
        private void LoadAutoCompleteTextBoxes(Form container)
        {
            try
            {
                foreach (UltraGridColumn dc in _filterGrid.DisplayLayout.Bands[0].Columns)
                {
                    AutoCompleteTextBox auto = new AutoCompleteTextBox(container);
                    auto.Values = GetSuggestionValueFromCache(dc.Key);

                    if (auto.Values != null && auto.Values.Length > 0)
                    {
                        _liAutoCompleteTextBox.Add(auto);
                        UltraControlContainerEditor ultraControlContainerEditor1 = new UltraControlContainerEditor();
                        ultraControlContainerEditor1.EditingControl = auto;
                        dc.EditorComponent = ultraControlContainerEditor1;
                        dc.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;

                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get data from Cache manager and tagdatabase manager
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string[] GetSuggestionValueFromCache(string p)
        {
            List<String> liReturn = new List<string>();
            try
            {

                switch (p.ToLower())
                {
                    case "account name":
                        // Removed CHMV Release Mode Check Because getting user accounts only for the prana release mode also
                        //  So same task was performed in the both release mode
                        foreach (String s in CachedDataManager.GetInstance.GetUserAccountsAsDict().Values)
                            liReturn.Add(s);
                        break;
                    case "strategy name":
                        foreach (String s in CachedDataManager.GetInstance.GetAllStrategies().Values)
                            liReturn.Add(s);
                        break;
                    case "side":
                        foreach (String s in TagDatabaseManager.GetInstance.GetAllOrderSides().Values)
                            liReturn.Add(s);
                        break;
                    case "venue":
                        foreach (String s in CachedDataManager.GetInstance.GetAllVenues().Values)
                            liReturn.Add(s);
                        break;
                    case "counter party":
                        foreach (String s in CachedDataManager.GetInstance.GetAllCounterParties().Values)
                            liReturn.Add(s);
                        break;
                    case "trading account":
                        foreach (String s in CachedDataManager.GetInstance.GetAllTradingAccount().Values)
                            liReturn.Add(s);
                        break;
                    case "asset":
                        foreach (String s in CachedDataManager.GetInstance.GetAllAssets().Values)
                            liReturn.Add(s);
                        break;
                    case "currency":
                        foreach (String s in CachedDataManager.GetInstance.GetAllCurrencies().Values)
                            liReturn.Add(s);
                        break;
                    case "exchange":
                        foreach (String s in CachedDataManager.GetInstance.GetAllExchanges().Values)
                            liReturn.Add(s);
                        break;
                    case "underlying":
                        foreach (String s in CachedDataManager.GetInstance.GetAllUnderlyings().Values)
                            liReturn.Add(s);
                        break;
                    case "preallocated":
                    case "manual group":
                        liReturn.Add("True");
                        liReturn.Add("False");
                        break;

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            if (liReturn.Count > 0)
                return liReturn.ToArray();

            return null;
        }


        /// <summary>
        /// Loading list of columns into grid. Called internally by constructors
        /// </summary>
        private void LoadColumnsInGrid()
        {
            try
            {

                foreach (String key in _columnList.Keys)
                {
                    foreach (String colName in _columnList[key])
                    {

                        if (key == _firstBandName)//first band name will be omitted.
                            //_filterGrid.DisplayLayout.Bands[0].Columns.Add(colName);
                            _dtSourceFilter.Columns.Add(colName, typeof(string));
                        else//Columns from other band is added as BandName.ColumnName to avoid ambiguity
                            //_filterGrid.DisplayLayout.Bands[0].Columns.Add(key + "." + colName);
                            _dtSourceFilter.Columns.Add(colName).Namespace = key;
                    }
                }



            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This function normalizes filters as required by query generator and returns
        /// </summary>
        /// <param name="dictFilter">Filters to be normalized</param>
        /// <returns>Normalized Filters</returns>
        private Dictionary<string, string> NormalizeFilter(Dictionary<string, string> dictFilter)
        {
            Dictionary<String, String> newDictFilter = new Dictionary<string, string>();
            try
            {
                foreach (String key in dictFilter.Keys)
                {
                    String val = dictFilter[key].Trim().ToLower();
                    if (!string.IsNullOrEmpty(val))
                    {
                        switch (key)
                        {
                            case "Account Name":
                                String accountIDCSV = ConvertCSVNameToCSVID(ColumnListForNormalization.AccountName, val);
                                //int accountID = CachedDataManager.GetInstance.GetAccountID(val);
                                if (!String.IsNullOrEmpty(accountIDCSV))
                                    newDictFilter.Add("AccountID", accountIDCSV);
                                continue;
                            case "Strategy Name":
                                String straCSV = ConvertCSVNameToCSVID(ColumnListForNormalization.StrategyName, val);
                                //int strategyID = CachedDataManager.GetInstance.GetStrategyID(val);
                                if (!String.IsNullOrEmpty(straCSV))
                                    newDictFilter.Add("StrategyID", straCSV);
                                continue;
                            case "Counter Party":
                                String cpCSV = ConvertCSVNameToCSVID(ColumnListForNormalization.CounterParty, val);
                                //int counterParty = CachedDataManager.GetInstance.GetCounterPartyID(val);
                                if (!String.IsNullOrEmpty(cpCSV))
                                    newDictFilter.Add("CounterPartyID", cpCSV);
                                continue;
                            case "Venue":
                                String venueCSV = ConvertCSVNameToCSVID(ColumnListForNormalization.Venue, val);
                                if (!String.IsNullOrEmpty(venueCSV))
                                    newDictFilter.Add("VenueID", venueCSV);
                                continue;
                            case "Side":
                                String sideCSV = ConvertCSVNameToCSVID(ColumnListForNormalization.Side, val);
                                if (!String.IsNullOrEmpty(sideCSV))
                                    newDictFilter.Add("OrderSideTagValue", sideCSV);
                                continue;
                            case "Trading Account":
                                String taCSV = ConvertCSVNameToCSVID(ColumnListForNormalization.TradingAccount, val);
                                //int tradingAccountID = CachedDataManager.GetInstance.GetTradingAccountID(val);
                                if (!String.IsNullOrEmpty(taCSV))
                                    newDictFilter.Add("TradingAccountID", taCSV);
                                continue;
                            case "Asset":
                                String acCSV = ConvertCSVNameToCSVID(ColumnListForNormalization.AssetCategory, val);
                                //int assetID = CachedDataManager.GetInstance.GetAssetID(val);
                                if (!String.IsNullOrEmpty(acCSV))
                                    newDictFilter.Add("AssetID", acCSV);
                                continue;
                            case "Currency":
                                String cCSV = ConvertCSVNameToCSVID(ColumnListForNormalization.Currency, val);
                                //int currencyID = CachedDataManager.GetInstance.GetCurrencyID(val);
                                if (!String.IsNullOrEmpty(cCSV))
                                    newDictFilter.Add("CurrencyID", cCSV);
                                continue;
                            case "Exchange":
                                String exCSV = ConvertCSVNameToCSVID(ColumnListForNormalization.Exchange, val);
                                //int exchangeID = CachedDataManager.GetInstance.GetExchangeID(val);
                                if (!String.IsNullOrEmpty(exCSV))
                                    newDictFilter.Add("ExchangeID", exCSV);
                                continue;
                            case "PreAllocated":
                                if (!String.IsNullOrEmpty(val))
                                {
                                    int isPreAllocatedInt = 1;//Default value is true
                                    if (String.Compare(val, "false", true) == 0)
                                    {
                                        isPreAllocatedInt = 0;
                                    }
                                    if (GetListFromCSV(val).Count > 0)
                                        newDictFilter.Add("IsPreAllocated", isPreAllocatedInt.ToString());
                                }
                                continue;
                            case "Manual Group":
                                if (!String.IsNullOrEmpty(val))
                                {
                                    int isManualGroupInt = 1;//Default value is true
                                    if (String.Compare(val, "false", true) == 0)
                                    {
                                        isManualGroupInt = 0;
                                    }
                                    if (GetListFromCSV(val).Count > 0)
                                        newDictFilter.Add("IsManualGroup", isManualGroupInt.ToString());
                                }
                                continue;
                            //case "PutOrCall":
                            //    bool isPreallocated = CachedDataManager.GetInstance.GetExchangeID(dictFilter[key]);
                            //    newDictFilter.Add("PutOrCall", isPreallocated.ToString());
                            //    continue;
                            case "Underlying":
                                String unCSV = ConvertCSVNameToCSVID(ColumnListForNormalization.Underlying, val);
                                //int underlyingID = CachedDataManager.GetInstance.GetUnderlyingID(val);
                                if (!String.IsNullOrEmpty(unCSV))
                                    newDictFilter.Add("UnderlyingID", unCSV);
                                continue;
                            default:
                                if (GetListFromCSV(val).Count > 0)
                                    newDictFilter.Add(key, val);
                                continue;
                        }
                    }
                }
                dictFilter.Clear();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return newDictFilter;
        }

        /// <summary>
        /// Using CacheManager and tagDatabasemanager converts csv value of names to csv value of IDs
        /// </summary>
        /// <param name="key">ColumnName</param>
        /// <param name="name">CSV value</param>
        /// <returns>CSV ID</returns>
        private String ConvertCSVNameToCSVID(ColumnListForNormalization key, String name)
        {
            StringBuilder sbNewCSV = new StringBuilder();
            try
            {
                List<String> nameList = GetListFromCSV(name);
                for (int i = 0; i < nameList.Count; i++)
                {
                    if (i != 0)
                    {
                        sbNewCSV.Append(',');
                    }
                    switch (key)
                    {
                        case ColumnListForNormalization.AssetCategory:

                            sbNewCSV.Append(CachedDataManager.GetInstance.GetAssetID(nameList[i]));
                            break;
                        case ColumnListForNormalization.CounterParty:
                            sbNewCSV.Append(CachedDataManager.GetInstance.GetCounterPartyID(nameList[i]));
                            break;
                        case ColumnListForNormalization.Currency:
                            sbNewCSV.Append(CachedDataManager.GetInstance.GetCurrencyID(nameList[i]));
                            break;
                        case ColumnListForNormalization.Exchange:
                            sbNewCSV.Append(CachedDataManager.GetInstance.GetExchangeID(nameList[i]));
                            break;
                        case ColumnListForNormalization.AccountName:
                            sbNewCSV.Append(CachedDataManager.GetInstance.GetAccountID(nameList[i]));
                            break;
                        case ColumnListForNormalization.Side:
                            String sideValue = TagDatabaseManager.GetInstance.GetOrderSideValue(nameList[i]);
                            if (String.IsNullOrEmpty(sideValue))
                                sbNewCSV.Append("NotValid");
                            else
                                sbNewCSV.Append(sideValue);
                            break;
                        case ColumnListForNormalization.StrategyName:
                            sbNewCSV.Append(CachedDataManager.GetInstance.GetStrategyID(nameList[i]));
                            break;
                        case ColumnListForNormalization.TradingAccount:
                            sbNewCSV.Append(CachedDataManager.GetInstance.GetTradingAccountID(nameList[i]));
                            break;
                        case ColumnListForNormalization.Underlying:
                            sbNewCSV.Append(CachedDataManager.GetInstance.GetUnderlyingID(nameList[i]));
                            break;
                        case ColumnListForNormalization.Venue:
                            sbNewCSV.Append(CachedDataManager.GetInstance.GetVenueID(nameList[i]));
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return sbNewCSV.ToString();
        }

        /// <summary>
        /// Convert CSV string to list value operates on String
        /// </summary>
        /// <param name="value">Values in csv format</param>
        /// <returns>List of values</returns>
        private static List<string> GetListFromCSV(string value)
        {
            List<String> ls = new List<String>();
            try
            {
                String[] lsArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lsArray.Length; i++)
                {
                    ls.Add(lsArray[i].Trim());
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return ls;
        }

        /// <summary>
        /// Changes Appearance for passed instance
        /// </summary>
        /// <param name="_currentFilter">Instance of FilterGrid which appearance has to be changed.</param>
        private static void DesignFilterColor(FilterGrid _currentFilter)
        {
            try
            {
                switch (_currentFilter._filterTitle)
                {
                    case "Allocated":
                        _currentFilter._filterGrid.DisplayLayout.Appearance.BackColor = Color.DarkGray;
                        break;
                    case "UnAllocated":
                        _currentFilter._filterGrid.DisplayLayout.Appearance.BackColor = Color.LightGray;
                        break;
                    case "All":
                        _currentFilter._filterGrid.DisplayLayout.Appearance.BackColor = Color.LemonChiffon;
                        //_currentFilter._filterGrid.DisplayLayout.Override.CellAppearance.BackColor = Color.LemonChiffon;
                        //_currentFilter._filterGrid.DisplayLayout.Override.ActiveCellColumnHeaderAppearance.BackColor = Color.LemonChiffon;
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Internal method for setting positions of columns. NO_OF_COLS determines the number of columns in each row.
        /// </summary>
        /// <param name="colInfos"></param>
        private void SetDefaultColumnPositions(RowLayoutColumnInfosCollection colInfos)
        {
            try
            {
                int totalColWidth = 0;
                int visibleColumnCount = 0;
                foreach (RowLayoutColumnInfo rlColInfo in colInfos)
                {
                    if (rlColInfo.Column.Hidden == false)
                    {
                        visibleColumnCount++;
                        totalColWidth += rlColInfo.Column.CellSizeResolved.Width;
                        if ((rlColInfo.Column.Key == _filterTitle) || (rlColInfo.Column.Key == "ClearButton"))
                            continue;
                        rlColInfo.WeightX = 2.0F;//does not let the buttons expand due to autofit
                    }

                }
                visibleColumnCount = visibleColumnCount - 2;//to handle the 2 buttons

                int NO_OF_ROWS = Convert.ToInt32(System.Math.Ceiling(visibleColumnCount * 1.0 / NoOfCols));
                int columnCount = 0;
                _filterGrid.Height = 22 * (NO_OF_ROWS);

                if (columnCount == colInfos.Count)
                {
                    return;
                }

                for (int i = 0; i < NO_OF_ROWS; i++)
                {
                    for (int j = 0; j < NoOfCols; j++)
                    {
                        if (columnCount >= colInfos.Count)
                        {
                            return;
                        }

                        while (colInfos[columnCount].Column.Hidden || colInfos[columnCount].Column.Key == _filterTitle || colInfos[columnCount].Column.Key == "ClearButton")
                        {
                            ++columnCount;

                            if (columnCount >= colInfos.Count)
                            {
                                return;
                            }
                        }

                        if (columnCount < colInfos.Count)
                        {
                            //if (i == 0)
                            //{
                            colInfos[columnCount].Initialize((j + 2) * 2, i * 2);
                            //}
                            //else
                            //{
                            //    colInfos[columnCount].Initialize(j * 2, i * 2);
                            //}
                            ++columnCount;
                            //continue;
                        }

                        if (columnCount >= colInfos.Count)
                        {
                            break;
                        }
                    }

                    if (columnCount >= colInfos.Count)
                    {
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Publicly exposed Methods

        /// <summary>
        /// Not completely implemented
        /// </summary>
        public void SaveLayout()
        {

            try
            {
                if (!Directory.Exists(_settingFilePath.Substring(0, _settingFilePath.LastIndexOf("\\"))))
                    Directory.CreateDirectory(_settingFilePath.Substring(0, _settingFilePath.LastIndexOf("\\")));
                _filterGrid.DisplayLayout.SaveAsXml(_settingFilePath, PropertyCategories.All);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Gets filter conditions only for combined filter instances.
        /// </summary>
        /// <returns>combined filter conditions as dictionary of dictionary.</returns>
        public Dictionary<String, Dictionary<String, String>> GetCombinedFilterCondition()
        {
            Dictionary<String, Dictionary<String, String>> finalFilter = new Dictionary<string, Dictionary<string, string>>();
            try
            {
                this._filterGrid.PerformAction(UltraGridAction.ExitEditMode);

                //TODO : to make it generic, need to remove the allocation specific logic
                Dictionary<String, String> allocated = new Dictionary<string, string>();
                Dictionary<String, String> unAllocated = new Dictionary<string, string>();
                if (_isCombined)
                {
                    DataRow dr = _dtSourceFilter.Rows[0];
                    foreach (DataColumn dc in this._dtSourceFilter.Columns)
                    {
                        String s = dc.ColumnName;
                        string val = dr[s].ToString().Trim();
                        if (String.IsNullOrEmpty(val))
                        {
                            continue;
                        }
                        foreach (String k in this._columnList.Keys)
                        {
                            if (this._columnList[k].Contains(s))
                            {
                                switch (k)
                                {
                                    case "Allocated":
                                        allocated.Add(s, val);
                                        break;
                                    case "UnAllocated":
                                        unAllocated.Add(s, val);
                                        break;
                                    case "All":
                                        allocated.Add(s, val);
                                        unAllocated.Add(s, val);
                                        break;
                                }
                            }
                        }
                    }
                }
                finalFilter.Add("Allocated", NormalizeFilter(allocated));
                finalFilter.Add("UnAllocated", NormalizeFilter(unAllocated));

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return finalFilter;
        }

        /// <summary>
        /// Gets filter condtion entered by user only for filters not of combined type.
        /// </summary>
        /// <returns>Condition as key value pair in string format</returns>
        public Dictionary<String, String> GetFilterCondition()
        {
            Dictionary<String, String> filterCondition = new Dictionary<string, string>();
            try
            {
                this._filterGrid.PerformAction(UltraGridAction.ExitEditMode);
                if (!_iscardview)
                {
                    //As all columns in filerGrid is band0 so geeting columnFilter from "_filterGrid.DisplayLayout.Bands[0]"
                    foreach (ColumnFilter columnFilter in _filterGrid.DisplayLayout.Bands[0].ColumnFilters)
                    {
                        if (columnFilter.FilterConditions != null && columnFilter.FilterConditions.Count > 0)
                        {
                            //Only one condition per column is allowed so using "FilterConditions[0]"
                            //Only one operator either contains or equals operator is used so only extracting value of operand
                            String sym = columnFilter.FilterConditions[0].CompareValue as String;
                            if (!String.IsNullOrEmpty(sym))
                                filterCondition.Add(columnFilter.Column.Key, sym);
                        }
                    }
                }

                else
                {
                    DataRow dr = _dtSourceFilter.Rows[0];
                    filterCondition.Clear();
                    foreach (DataColumn dc in _dtSourceFilter.Columns)
                    {

                        if (!String.IsNullOrEmpty(dr[dc.ColumnName].ToString().Trim()))
                            filterCondition.Add(dc.ColumnName, dr[dc.ColumnName].ToString());

                    }
                }
                return NormalizeFilter(filterCondition);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }


            return filterCondition;
        }

        /// <summary>
        /// Attaches filter to the given ultratoolbar manager instance.
        /// </summary>
        /// <param name="utm">UltraToolbarManager instance in which the filter is to be attached.</param>
        /// <returns>true if successful else false.</returns>
        public bool AttachToToolbar(UltraToolbarsManager utm)
        {
            try
            {
                this.SuspendLayout();

                #region FilterGrid Design
                if (_isInitilizedFirstTime)
                {
                    _filterGrid.DisplayLayout.Bands[0].Columns.Add("firstcol");
                }
                string[] ndtrow = new string[_dtSourceFilter.Columns.Count];
                _dtSourceFilter.Rows.Add(ndtrow);
                _filterGrid.DataSource = _dtSourceFilter;

                //Initializing default columns if Filter is initialiing for the first time
                if (_isInitilizedFirstTime)
                {
                    ColumnsCollection colCollection = _filterGrid.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn dcDefault in colCollection)
                    {

                        //XmlEnumAttribute.IsDefined(typeof(DefaultColumnsList), dcDefault.Key);
                        //if (!Enum.IsDefined(typeof(DefaultColumnsList), dcDefault.Key))
                        if (!_liDefault.Contains(dcDefault.Key))
                        {
                            _filterGrid.DisplayLayout.Bands[0].Columns[dcDefault.Key].Hidden = true;
                        }
                    }

                    _btColumnChooser = _filterGrid.DisplayLayout.Bands[0].Columns.Add(_filterTitle);

                    _btClearFilter = _filterGrid.DisplayLayout.Bands[0].Columns.Add("ClearButton");

                    _isInitilizedFirstTime = false;
                }


                _filterGrid.DisplayLayout.Bands[0].CardView = true;
                _filterGrid.DisplayLayout.Bands[0].RowLayoutStyle = RowLayoutStyle.ColumnLayout;
                _filterGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
                _filterGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                _filterGrid.DisplayLayout.Bands[0].Override.BorderStyleCell = UIElementBorderStyle.None;
                _filterGrid.DisplayLayout.Bands[0].CardSettings.AutoFit = true;
                _filterGrid.DisplayLayout.Scrollbars = Scrollbars.Automatic;
                _filterGrid.Dock = System.Windows.Forms.DockStyle.Fill;
                _filterGrid.DisplayLayout.Bands[0].CardSettings.MaxCardAreaCols = 7;
                _filterGrid.DisplayLayout.Bands[0].CardSettings.MaxCardAreaRows = 10;
                _filterGrid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _filterGrid.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.SeparateElement;
                _filterGrid.DisplayLayout.Override.AllowColSizing = AllowColSizing.None;
                //_filterGrid.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
                FilterGrid.DesignFilterColor(this);
                _filterGrid.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;


                //Adding tooltip
                System.Windows.Forms.ToolTip tpGrid = new System.Windows.Forms.ToolTip();
                tpGrid.IsBalloon = false;
                tpGrid.AutoPopDelay = 0;
                tpGrid.InitialDelay = 0;
                tpGrid.ForeColor = Color.Gray;
                tpGrid.BackColor = Color.WhiteSmoke;
                tpGrid.SetToolTip(_filterGrid, _filterTitle);

                _filterGrid.DisplayLayout.Bands[0].RowLayouts.Clear();

                RowLayout defaultRowLayout = _filterGrid.DisplayLayout.Bands[0].RowLayouts.Add("LayOut");
                SetDefaultColumnPositions(defaultRowLayout.ColumnInfos);
                defaultRowLayout.RowLayoutLabelStyle = RowLayoutLabelStyle.WithCellData;
                defaultRowLayout.CardViewStyle = CardStyle.StandardLabels;
                defaultRowLayout.RowLayoutLabelPosition = LabelPosition.Left;
                defaultRowLayout.Apply();

                //added to display the first 2 hidden columns
                ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterGrid));
                _btColumnChooser.Header.VisiblePosition = 0;
                _btColumnChooser.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                _btColumnChooser.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                _btColumnChooser.Width = 18;
                _btColumnChooser.RowLayoutColumnInfo.LabelPosition = LabelPosition.Left;
                _btColumnChooser.TipStyleCell = TipStyle.Show;
                _btColumnChooser.CellAppearance.Image = ((System.Drawing.Image)resources.GetObject("btnColumnChooser.Image"));
                _btColumnChooser.CellButtonAppearance.Image = ((System.Drawing.Image)resources.GetObject("btnColumnChooser.Image"));

                _btClearFilter.Header.VisiblePosition = 1;
                _btClearFilter.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                _btClearFilter.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                _btClearFilter.Width = 18;
                _btClearFilter.RowLayoutColumnInfo.LabelPosition = LabelPosition.None;
                _btClearFilter.CellAppearance.Image = ((System.Drawing.Image)resources.GetObject("Clear"));
                _btClearFilter.CellButtonAppearance.Image = ((System.Drawing.Image)resources.GetObject("Clear"));
                _filterGrid.ClickCellButton += new CellEventHandler(_filterGrid_ClickCellButton);

                //ultraComboDropdown();
                LoadAutoCompleteTextBoxes(utm.DockWithinContainer.FindForm());
                _filterGrid.Refresh();


                #endregion

                #region Toolbar Design

                UltraToolbar tb = new UltraToolbar(this._filterTitle);
                tb.DockedPosition = DockedPosition.Floating;
                tb.FloatingSize = new Size(900, 100);
                tb.Settings.FillEntireRow = DefaultableBoolean.True;

                #endregion

                #region Container tool designing

                Infragistics.Win.UltraWinToolbars.ControlContainerTool cntool = new ControlContainerTool(this._filterTitle);
                cntool.SharedProps.Caption = "Filtertool";
                cntool.Control = _filterGrid;
                cntool.SharedProps.DisplayStyle = ToolDisplayStyle.DefaultForToolType;
                cntool.InstanceProps.DisplayStyle = ToolDisplayStyle.DefaultForToolType;
                cntool.InstanceProps.Spring = DefaultableBoolean.True;
                cntool.SharedProps.Spring = true;
                cntool.SharedProps.ToolTipText = "Filter Toolbar for " + _filterTitle;
                cntool.CanSetWidth = true;

                #endregion

                //Adding tools and toolbar to ToolbarsManager
                utm.Tools.Add(cntool);
                utm.ToolbarSettings.FillEntireRow = DefaultableBoolean.True;
                utm.Toolbars.AddToolbar(this._filterTitle).Tools.AddToolRange(new string[] { this._filterTitle });

                this.ResumeLayout(true);

                //LoadAutoCompleteTextBoxes(utm.DockWithinContainer as Form);

                return true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Combines instances of filters and generates a new instance.
        /// </summary>
        /// <param name="filterGridList">list of filters to be combined.</param>
        /// <returns>Newly created instance of combined filter</returns>
        public static FilterGrid CombineFilters(params FilterGrid[] filterGridList)
        {
            //creating new filterGrid instance and assigning values to it
            FilterGrid newFilterGrid = new FilterGrid();
            try
            {
                if (!(filterGridList[0]._isCombined) && !(filterGridList[1]._isCombined))
                {
                    newFilterGrid._isCombined = true;
                    newFilterGrid._filterTitle = "All";
                    newFilterGrid._firstBandName = "All";
                    newFilterGrid._settingFilePath = Application.StartupPath + "\\GridSettings\\" + newFilterGrid._filterTitle + ".xml";

                    //TODO : Designed only for Allocated needs to be refactored again as generic
                    String key1 = String.Empty;
                    String key2 = String.Empty;

                    foreach (String k in filterGridList[0]._columnList.Keys)
                    {
                        key1 = k;
                        break;
                    }

                    foreach (String k in filterGridList[1]._columnList.Keys)
                    {
                        key2 = k;
                        break;
                    }

                    //Adding new Key "All"for common columns
                    newFilterGrid._columnList.Add("All", new List<String>());

                    List<String> newList1 = new List<string>();
                    foreach (String s in filterGridList[0]._columnList[key1])
                    {
                        newList1.Add(s);
                    }

                    List<String> newList2 = new List<string>();
                    foreach (String s in filterGridList[1]._columnList[key2])
                    {
                        newList2.Add(s);
                    }

                    newFilterGrid._columnList.Add(key1, newList1);
                    newFilterGrid._columnList.Add(key2, newList2);

                    #region Refactoring column list for all keys

                    for (int i = 0; i < filterGridList[0]._columnList[key1].Count; i++)
                    {
                        string s = filterGridList[0]._columnList[key1][i];
                        if (filterGridList[1]._columnList[key2].Contains(s))
                        {
                            newFilterGrid._columnList["All"].Add(s);
                        }
                    }

                    foreach (String s in newFilterGrid._columnList["All"])
                    {
                        newFilterGrid._columnList[key1].Remove(s);
                        newFilterGrid._columnList[key2].Remove(s);
                    }

                    #endregion

                }

                newFilterGrid.LoadColumnsInGrid();

                #region Loading settings from xml file
                newFilterGrid._settingFilePath = Application.StartupPath + "\\GridSettings\\" + newFilterGrid._filterTitle + ".xml";

                if (Directory.Exists(newFilterGrid._settingFilePath.Substring(0, newFilterGrid._settingFilePath.LastIndexOf("\\"))))
                {
                    if (File.Exists(newFilterGrid._settingFilePath))
                        newFilterGrid._filterGrid.DisplayLayout.LoadFromXml(newFilterGrid._settingFilePath, PropertyCategories.All);
                    newFilterGrid.Refresh();
                }
                else
                    Directory.CreateDirectory(newFilterGrid._settingFilePath.Substring(0, newFilterGrid._settingFilePath.LastIndexOf("\\")));
                #endregion

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return newFilterGrid;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Unwire events. Disopose control. Currently tried to call it from AllocationMain UI but did not work
        /// </summary>
        //void Dispose(bool isDisposing)
        //{
        //    if (isDisposing)
        //    {
        //        foreach (AutoCompleteTextBox at in _liAutoCompleteTextBox)
        //        {
        //            at.Dispose();
        //        }
        //        _filterGrid.AfterColPosChanged -= new AfterColPosChangedEventHandler(_filterGrid_AfterColPosChanged);
        //        _filterGrid.ClickCellButton -= new CellEventHandler(_filterGrid_ClickCellButton); 
        //    }
        //}

        #endregion

        private void _filterGrid_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {

            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this._filterGrid);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void _filterGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    this._filterGrid.UseAppStyling = false;
                    this._filterGrid.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
                    Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
                    appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(89)))), ((int)(((byte)(90)))));
                    appearance3.ForeColor = System.Drawing.Color.White;
                    this._filterGrid.DisplayLayout.Override.FixedHeaderAppearance = appearance3;

                    Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
                    appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(89)))), ((int)(((byte)(90)))));
                    appearance1.ForeColor = System.Drawing.Color.White;
                    this._filterGrid.DisplayLayout.Override.HeaderAppearance = appearance1;

                    Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
                    appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(232)))));
                    appearance2.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
                    appearance2.BorderColor = System.Drawing.Color.DarkGray;
                    appearance2.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
                    appearance2.ForeColor = System.Drawing.Color.Black;
                    this._filterGrid.DisplayLayout.Override.CellAppearance = appearance2;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void _filterGrid_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}
