using Infragistics.Win;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LiveFeedProvider;
using Prana.LogManager;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.WatchList.Controls;
using Prana.WatchListData;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace Prana.WatchList.Classes
{
    public class WatchListManager : IDisposable
    {
        // this member contains the current userid.
        int CompanyUserID = -1;

        /// <summary>
        /// Occurs when [send symbol for tt to main].
        /// </summary>
        public event EventHandler<EventArgs<string, string>> SendSymbolForTTToMain;

        public event EventHandler<EventArgs<string>> SendSymbolForPTTToMain;

        /// <summary>
        /// Occurs when [send symbol to MTT].
        /// </summary>
        public event EventHandler<EventArgs<OrderBindingList>> SendSymbolForMTTToMain;

        /// <summary>
        /// Occures when Optio Chain Window is opened from TabCtrl
        /// </summary>
        public event EventHandler<EventArgs<int, string>> OptionChainModuleOpened;

        private ISecurityMasterServices _securityMaster;

        // these member cantains the current tab index
        private string _currentTabName = string.Empty;

        /// <summary>
        /// The linked tab index
        /// </summary>
        private string _linkedTabName = string.Empty;


        // this constant member is contains how much rows one grid contains.
        int _totalRowInGrid = Convert.ToInt32(ConfigurationManager.AppSettings["TotalRowInGrid_WatchList"]);

        // these members are contains the start due time and interval for a periodic method.
        int _level1TimerStartDueTime = Convert.ToInt32(ConfigurationManager.AppSettings["Level1TimerStartDueTime"]);
        int _timerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]);


        // this member is the object of Timer which is use to triger a method periodicly.
        System.Threading.Timer _timer = null;

        /// <summary>
        /// The sec master data
        /// </summary>
        public ConcurrentDictionary<string, SecMasterBaseObj> secMasterDataDict = new ConcurrentDictionary<string, SecMasterBaseObj>();

        private Dictionary<string, List<string>> _fxSymbolMapping = new Dictionary<string, List<string>>();

        /// <summary>
        /// The linked symbol selected
        /// </summary>
        public EventHandler<EventArgs<string>> LinkedSymbolSelected;

        // this object is use for store and fetch all the infomation related to symbol.
        private SymbolsAddressBook _symbolsAddressBook = null;

        // store the object of ultraDockManager of watchListMain Form.
        public UltraDockManager ultraDockManager = null;

        // cache for store the live response. 
        private Dictionary<string, SymbolData> _responseQueueCache = new Dictionary<string, SymbolData>();

        // this object is use to send request for live feed and wire the method for take response. 
        private MarketDataHelper _marketDataHelperInstance = MarketDataHelper.GetInstance();

        private DataBaseOperationHelper _dataBaseOperationHelper = DataBaseOperationHelper.Instance();

        /// <summary>
        /// Add total Tab firstlly.
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="LoginUser"></param>
        public WatchListManager(UltraDockManager ultraDockManager, ISecurityMasterServices securityMaster, Prana.BusinessObjects.CompanyUser LoginUser)
        {
            try
            {
                CompanyUserID = LoginUser == null ? -1 : LoginUser.CompanyUserID;
                _securityMaster = securityMaster;
                _securityMaster.SecMstrDataResponse += _securityMaster_SecMstrDataResponse;
                this.ultraDockManager = ultraDockManager;
                _marketDataHelperInstance.OnResponse += new EventHandler<EventArgs<SymbolData>>(SymbolLiveFeed_OnResponse);
                _symbolsAddressBook = new SymbolsAddressBook(_dataBaseOperationHelper.GetAllTabsDataForUser(CompanyUserID));
                if (_symbolsAddressBook.TabNamesDict.Count > 0)
                {
                    _currentTabName = _symbolsAddressBook.TabNamesDict.FirstOrDefault(x => x.Value == 0).Key;
                    string tabName = _dataBaseOperationHelper.GetLinkedTab(CompanyUserID);
                    if (!string.IsNullOrEmpty(tabName) && _symbolsAddressBook.TabNamesDict.ContainsKey(tabName))
                        _linkedTabName = tabName;
                }
                List<string> symbols = _symbolsAddressBook.AllSymbols;
                for (int i = 0; i < _symbolsAddressBook.TabNames.Count; i++)
                {
                    AddNewTabWithGrid(_symbolsAddressBook.TabNames[i], securityMaster);
                }
                if (symbols != null && symbols.Count > 0)
                {
                    GetAllSymbolsSecMaster(symbols);
                }
                _timer = new System.Threading.Timer(new TimerCallback(PeriodicTask), null, _level1TimerStartDueTime, _timerInterval);
                //_maxTabInd = tabControl.TabCount - 1;
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

        private void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                secMasterDataDict.TryAdd(e.Value.TickerSymbol, e.Value);
                RequestMarketDataSymbol(e.Value.TickerSymbol, e.Value);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <param name="errMessage">The error message.</param>
        public void GetAllSymbolsSecMaster(List<string> symbols)
        {
            try
            {
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    SecMasterRequestObj reqObj = new SecMasterRequestObj();
                    foreach (string symbol in symbols)
                    {
                        reqObj.AddData(symbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                    }
                    reqObj.HashCode = GetHashCode();
                    _securityMaster.SendRequest(reqObj);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates the dock area pane.
        /// </summary>
        /// <returns></returns>
        private DockAreaPane CreateDockAreaPane()
        {
            DockAreaPane pane = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedTop);
            //
            //dockAreaPane1
            //
            pane.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            pane.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            pane.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            pane.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            pane.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            pane.Settings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            pane.Settings.AllowPin = Infragistics.Win.DefaultableBoolean.False;
            pane.Settings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            pane.Settings.DoubleClickAction = PaneDoubleClickAction.None;
            pane.ChildPaneStyle = ChildPaneStyle.TabGroup;
            pane.Settings.Appearance.BackColor = Color.Gainsboro;
            AppearanceBase tabAppearance = new Infragistics.Win.Appearance();
            tabAppearance.BackColor = Color.Gainsboro;
            tabAppearance.ForeColor = Color.Black;
            AppearanceBase activeTabAppearance = new Infragistics.Win.Appearance();
            activeTabAppearance.BackColor = Color.White;
            activeTabAppearance.ForeColor = Color.Black;
            pane.DefaultPaneSettings.TabAppearance = tabAppearance;
            pane.DefaultPaneSettings.SelectedTabAppearance = activeTabAppearance;
            pane.DefaultPaneSettings.TabAppearance.FontData.SizeInPoints = 10;
            pane.DefaultPaneSettings.CaptionAppearance.TextHAlign = HAlign.Center;
            pane.DefaultPaneSettings.TabAppearance.BorderColor = Color.Black;
            return pane;
        }

        internal bool IsTabPresent(int tabNumber)
        {
            try
            {
                string tabName = _symbolsAddressBook.TabNamesDict.Where(x => x.Value == tabNumber).Select(x => x.Key).FirstOrDefault();

                if (tabName != null && !string.IsNullOrEmpty(tabName))
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// This method gets the tab number from tab name
        /// </summary>
        internal int GetTabNumberFromTabName(string tabName)
        {
            try
            {
                if (_symbolsAddressBook.TabNamesDict.ContainsKey(tabName))
                    return _symbolsAddressBook.TabNamesDict[tabName];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// Adds the new tab with grid.
        /// </summary>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="securityMasterServices">The security master services.</param>
        private void AddNewTabWithGrid(string tabName, ISecurityMasterServices securityMasterServices)
        {
            try
            {
                CtrlWatchListTab tabCtrl = new CtrlWatchListTab(this, tabName, CompanyUserID);
                DockAreaPane areaPane = null;
                if (ultraDockManager.DockAreas.Count > 0)
                    areaPane = ultraDockManager.DockAreas[0];
                else
                {
                    areaPane = CreateDockAreaPane();
                    ultraDockManager.DockAreas.Add(areaPane);
                }
                DockableControlPane dockableControlPane = new DockableControlPane();
                UserControl ctrl = new UserControl();
                dockableControlPane.Control = tabCtrl;
                dockableControlPane.Key = tabName;
                dockableControlPane.Text = "   " + tabName + "   ";
                tabCtrl.SendSymbolToPTT += SendSymbolToPTT;
                tabCtrl.SendSymbolToTT += SendTradeToTT;
                tabCtrl.SendSymbolToMTT += SendTradeToMTT;
                tabCtrl.SetThemeForUserControl();
                tabCtrl.GridLayoutLoad(CompanyUserID);
                if (securityMasterServices != null)
                    tabCtrl.SecurityMaster = securityMasterServices;
                areaPane.Panes.Add(dockableControlPane);
                if (_symbolsAddressBook.TabWiseSymbols.ContainsKey(tabName))
                    _symbolsAddressBook.TabWiseSymbols[tabName].ToList().ForEach(symbol => tabCtrl.AddNewBlankRowIntoGrid(symbol, true));
                tabCtrl.OptionChainModuleOpened += tabCtrl_OptionChainModuleOpened;
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

        void SendSymbolToPTT(object sender, EventArgs<string> e)
        {
            try
            {
                if (SendSymbolForPTTToMain != null)
                    SendSymbolForPTTToMain(null, e);
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

        /// <summary>
        /// This method add the Symbol in tab and send the request for live feed. 
        /// </summary>
        /// <param name="symbol">string</param>
        internal void AddSymbolToTab(string symbol, string tabName, SecMasterBaseObj secMasterObj = null)
        {
            try
            {
                _symbolsAddressBook.AddSymbolToTab(symbol, tabName);
                _dataBaseOperationHelper.AddSymbolIntoDatabase(symbol, tabName, CompanyUserID);
                RequestMarketDataSymbol(symbol, secMasterObj);
                AddNewBlankRow(symbol, _symbolsAddressBook.TabNamesDict[tabName]);
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

        /// <summary>
        /// This method add the Symbol in tab and send the request for live feed. 
        /// </summary>
        /// <param name="symbol">Symbol to add</param>
        /// <param name="tabNumber">Tab number where shymbol will be added</param>
        internal bool AddSymbolToTab(string symbol, int tabNumber)
        {
            try
            {
                CtrlWatchListTab ctrlTab = null;
                int maxRowInGrid = Convert.ToInt32(ConfigurationManager.AppSettings[WatchListConstants.TOTALROWINGRID_WATCHLIST]);
                string tabName = _symbolsAddressBook.TabNamesDict.Where(x => x.Value == tabNumber).Select(x => x.Key).FirstOrDefault();

                if (!string.IsNullOrEmpty(tabName) && ultraDockManager.DockAreas != null)
                {
                    foreach (DockableControlPane pane in ultraDockManager.DockAreas[0].Panes)
                    {
                        if (pane.Control != null && ((CtrlWatchListTab)pane.Control).TabName == tabName)
                            ctrlTab = (CtrlWatchListTab)pane.Control;
                    }

                    if (string.IsNullOrEmpty(symbol))
                    {
                        ctrlTab.SetLabelMessage(WatchListConstants.MSG_SYMBOL_EMPTY);
                        return false;
                    }

                    if (GetTabSymbolsCount(tabName) >= maxRowInGrid)
                    {
                        ctrlTab.SetLabelMessage("Cannot add symbol as row count has reached the limit of " + maxRowInGrid + ".");
                        return false;
                    }

                    if (!_symbolsAddressBook.IsSymbolInTab(tabName, symbol))
                    {
                        AddSymbolToTab(symbol, tabName);
                        return true;
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

            return false;
        }

        /// <summary>
        /// Requests the market data symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="secMasterObj">The sec master object.</param>
        private void RequestMarketDataSymbol(string symbol, SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (secMasterObj != null)
                {
                    secMasterDataDict.TryAdd(secMasterObj.TickerSymbol, secMasterObj);

                    string tickerSymbol = symbol;
                    ConversionRate conversionRate = null;
                    if (secMasterObj.AssetID == (int)AssetCategory.FX || secMasterObj.AssetID == (int)AssetCategory.Forex)
                    {
                        SecMasterFxObj fxObj = (SecMasterFxObj)secMasterObj;
                        conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromCurrencies(fxObj.LeadCurrencyID, fxObj.VsCurrencyID, 0);
                    }
                    else if (secMasterObj.AssetID == (int)AssetCategory.FXForward)
                    {
                        SecMasterFXForwardObj fxFrowardObj = (SecMasterFXForwardObj)secMasterObj;
                        conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromCurrencies(fxFrowardObj.LeadCurrencyID, fxFrowardObj.VsCurrencyID, 0);
                    }
                    if (conversionRate != null)
                    {
                        if (!String.IsNullOrEmpty(conversionRate.FXeSignalSymbol))
                        {
                            tickerSymbol = conversionRate.FXeSignalSymbol;
                            lock (_fxSymbolMapping)
                            {
                                if (_fxSymbolMapping.ContainsKey(tickerSymbol))
                                {
                                    if (!_fxSymbolMapping[tickerSymbol].Contains(symbol))
                                        _fxSymbolMapping[tickerSymbol].Add(symbol);
                                }
                                else
                                    _fxSymbolMapping.Add(tickerSymbol, new List<string> { symbol });
                            }
                        }
                    }
                    _marketDataHelperInstance.RequestSingleSymbol(tickerSymbol, false);
                }
                else
                {
                    if (!secMasterDataDict.ContainsKey(symbol))
                        GetAllSymbolsSecMaster(new List<string>() { symbol });
                    _marketDataHelperInstance.RequestSingleSymbol(symbol, false);
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

        /// <summary>
        /// This method is use to add a blank row on grid.
        /// </summary>
        /// <param name="symbol">string</param>
        /// <param name="tabNumber">int</param>
        private void AddNewBlankRow(string symbol, int tabNumber)
        {
            try
            {
                if (ultraDockManager.ControlPanes.Count > 0)
                {
                    var currentGrid = (CtrlWatchListTab)ultraDockManager.ControlPanes[tabNumber].Control;
                    currentGrid.AddNewBlankRowIntoGrid(symbol, false);
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

        /// <summary>
        /// This method is use to delete all the complete data 
        /// </summary>
        internal void DeleteSymbolFromTab(string symbol, string tabName)
        {
            try
            {
                _symbolsAddressBook.RemoveSymbolFromTab(symbol, tabName);
                _dataBaseOperationHelper.RemoveSymbolFromDatabase(symbol, tabName, CompanyUserID);
                if (_symbolsAddressBook.SymbolsCountDict[symbol] <= 0)
                    _marketDataHelperInstance.RemoveSingleSymbol(symbol);

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

        /// <summary>
        /// This method is used to check whether the symbol is already added or not.
        /// </summary>
        /// <param name="symbol">string</param>
        /// <returns>bool</returns>
        internal bool IsSymbolInCurrentTab(string symbol, string tabName)
        {
            if (_symbolsAddressBook.TabWiseSymbols.ContainsKey(tabName))
                return _symbolsAddressBook.TabWiseSymbols[tabName].Contains(symbol) ? true : false;
            else
                return false;
        }

        /// <summary>
        /// This method is use to take the response and add it into cache.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SymbolLiveFeed_OnResponse(object sender, EventArgs<SymbolData> args)
        {
            try
            {
                SymbolData symbolData = args.Value;
                Dictionary<string, SymbolData> symbolDataDict = new Dictionary<string, SymbolData>();
                lock (_fxSymbolMapping)
                {
                    if (_fxSymbolMapping.ContainsKey(symbolData.Symbol))
                    {
                        foreach (string symbol in _fxSymbolMapping[symbolData.Symbol])
                        {
                            SymbolData clonedData = DeepCopyHelper.Clone(symbolData);
                            clonedData.Symbol = symbol;
                            symbolDataDict.Add(symbol, clonedData);
                        }
                    }
                    else
                    {
                        symbolDataDict.Add(symbolData.Symbol, symbolData);
                    }
                }
                foreach (string symbol in symbolDataDict.Keys)
                {
                    SymbolData data = symbolDataDict[symbol];
                    if (secMasterDataDict.ContainsKey(symbol))
                    {
                        SecMasterBaseObj secData = secMasterDataDict[symbol];
                        data.AUECID = secData.AUECID;
                        if (!string.IsNullOrEmpty(secData.BloombergSymbol))
                            data.BloombergSymbol = secData.BloombergSymbol;
                        if (!string.IsNullOrEmpty(secData.ActivSymbol))
                            data.ActivSymbol = secData.ActivSymbol;
                        if (!string.IsNullOrEmpty(secData.FactSetSymbol))
                            data.FactSetSymbol = secData.FactSetSymbol;
                        if (!string.IsNullOrEmpty(secData.ISINSymbol))
                            data.ISIN = secData.ISINSymbol;
                        if (!string.IsNullOrEmpty(secData.CusipSymbol))
                            data.CusipNo = secData.CusipSymbol;
                        if (!string.IsNullOrEmpty(secData.LongName))
                            data.FullCompanyName = secData.LongName;
                        if (!string.IsNullOrEmpty(secData.OSIOptionSymbol))
                            data.OSIOptionSymbol = secData.OSIOptionSymbol;
                        if (data.SharesOutstanding <= 0 && secData.SharesOutstanding >= 0)
                            data.SharesOutstanding = (long)secData.SharesOutstanding;
                        if (!string.IsNullOrEmpty(secData.BloombergSymbolWithExchangeCode))
                            data.BloombergSymbolWithExchangeCode = secData.BloombergSymbolWithExchangeCode;
                    }
                    if (_symbolsAddressBook.AllSymbols.Contains(symbol))
                    {
                        lock (_responseQueueCache)
                        {
                            _responseQueueCache[symbol] = data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method takes serially form live feed cache and update or add on the grid.
        /// </summary>
        /// <param name="state">object</param>
        private void PeriodicTask(object state)
        {
            try
            {
                Dictionary<string, SymbolData> tempResponseQueue = new Dictionary<string, SymbolData>(); ;
                lock (_responseQueueCache)
                {
                    if (_responseQueueCache.Count > 0)
                    {
                        tempResponseQueue = _responseQueueCache;
                        _responseQueueCache = new Dictionary<string, SymbolData>();
                    }
                    else
                        return;
                }

                foreach (SymbolData data in tempResponseQueue.Values)
                {
                    foreach (string key in _symbolsAddressBook.TabWiseSymbols.Keys)
                    {
                        if (_symbolsAddressBook.TabWiseSymbols[key].Contains(data.Symbol))
                        {
                            ((CtrlWatchListTab)ultraDockManager.ControlPanes[key].Control).UpdateRowInGridWithSymbolData(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is use to remove the previous tab symbols from pricing server and add current tab symbols.
        /// </summary>
        /// <param name="currentTab">int</param>
        public void TabChangeProcess(string currentTab)
        {
            try
            {
                _currentTabName = currentTab;
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

        /// <summary>
        /// This method is use to add new tab details.
        /// </summary>
        /// <param name="tabName"></param>
        public void AddNewTab(string tabName)
        {
            try
            {
                if (_symbolsAddressBook.TabNames.Any(tName => tabName.Equals(tName, StringComparison.CurrentCultureIgnoreCase)))
                    MessageBox.Show("This tab name is already taken.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    _symbolsAddressBook.AddNewTab(tabName);
                    _dataBaseOperationHelper.AddNewTabIntoDatabase(tabName, CompanyUserID);
                    AddNewTabWithGrid(tabName, _securityMaster);

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

        public void RenameTab(string NewName)
        {
            try
            {
                if (_symbolsAddressBook.TabNames.Any(tName => NewName.Equals(tName, StringComparison.CurrentCultureIgnoreCase)))
                    MessageBox.Show("This tab name is already taken.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    string oldName = _currentTabName;
                    _symbolsAddressBook.RenameTab(NewName, oldName);
                    _dataBaseOperationHelper.RenameTab(NewName, oldName, CompanyUserID);

                    var pane = ultraDockManager.ControlPanes[oldName];
                    pane.Key = NewName;
                    pane.Text = "   " + NewName + "   ";
                    ((CtrlWatchListTab)pane.Control).TabName = NewName;
                    string oldXmlPath = Application.StartupPath + "\\Prana Preferences\\" + CompanyUserID + "\\WatchList_GridLayout_" + oldName + ".xml";
                    string newXmlPath = Application.StartupPath + "\\Prana Preferences\\" + CompanyUserID + "\\WatchList_GridLayout_" + NewName + ".xml";
                    if (File.Exists(oldXmlPath))
                    {
                        System.IO.File.Move(oldXmlPath, newXmlPath);
                    }

                    if (_symbolsAddressBook.PermanentTabName == oldName)
                        _symbolsAddressBook.PermanentTabName = NewName;
                    _currentTabName = NewName;
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

        /// <summary>
        /// Gets the data table from different file formats.
        /// </summary>
        /// <param name="dropFilePath">The drop file path.</param>
        /// <returns></returns>
        private DataTable GetDataTableFromDifferentFileFormats(string dropFilePath)
        {
            DataTable dTable = null;
            try
            {
                string fileFormat = dropFilePath.Substring(dropFilePath.LastIndexOf(".") + 1);

                switch (fileFormat.ToUpperInvariant())
                {
                    case "CSV":
                        dTable = GeneralUtilities.GetDataTableFromUploadedDataFileBulkRead(dropFilePath);
                        break;
                    case "XLS":
                        dTable = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(dropFilePath);
                        break;
                }
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("File in use! Please close the file and retry.");
            }
            return dTable;
        }

        /// <summary>
        /// This method is use to count the current grid symbols.
        /// </summary>
        /// <returns></returns>
        internal int GetTabSymbolsCount(string tab = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tab))
                    tab = _currentTabName;
                if (!string.IsNullOrWhiteSpace(tab) && _symbolsAddressBook.TabWiseSymbols.ContainsKey(tab))
                    return _symbolsAddressBook.TabWiseSymbols[tab].Count;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// Creates the order single.
        /// </summary>
        /// <param name="pttResponseObjectList"></param>
        /// <param name="pttRequestObject"></param>
        /// <returns></returns>
        public OrderSingle CreateOrderSingleForMTT(string symbol, bool isBuyTrade)
        {
            var order = new OrderSingle();
            try
            {
                if (!string.IsNullOrEmpty(symbol))
                {
                    int auecID = int.MinValue;
                    SecMasterBaseObj sec = null;
                    if (secMasterDataDict.TryGetValue(symbol, out sec))
                    {
                        auecID = sec.AUECID;
                    }
                    TradingTicketUIPrefs userTradingTicketUiPrefs = TradingTktPrefs.UserTradingTicketUiPrefs;
                    TradingTicketUIPrefs companyTradingTicketUiPrefs = TradingTktPrefs.CompanyTradingTicketUiPrefs;
                    CounterPartyWiseCommissionBasis CommisionUserTTUiPrefs = TradingTktPrefs.CpwiseCommissionBasis;

                    int counterPartyID = 0;

                    if (userTradingTicketUiPrefs != null && companyTradingTicketUiPrefs != null)
                    {
                        if (userTradingTicketUiPrefs.Broker.HasValue)
                        {
                            counterPartyID = userTradingTicketUiPrefs.Broker.Value;
                        }
                        else if (companyTradingTicketUiPrefs.Broker.HasValue)
                        {
                            counterPartyID = companyTradingTicketUiPrefs.Broker.Value;
                        }

                        if (userTradingTicketUiPrefs != null && userTradingTicketUiPrefs.Broker.HasValue && CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue.ContainsKey(userTradingTicketUiPrefs.Broker.Value))
                        {
                            order.VenueID = CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue[userTradingTicketUiPrefs.Broker.Value];
                        }
                        // Venue shouldn't be empty if Broker is assigned and a corresponding Broker-Venue exists
                        else if (counterPartyID != 0 && CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue.ContainsKey(counterPartyID))
                        {
                            order.VenueID = CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue[counterPartyID];
                        }
                        else if (companyTradingTicketUiPrefs.Venue.HasValue)
                        {
                            order.VenueID = companyTradingTicketUiPrefs.Venue.Value;
                        }

                        if (userTradingTicketUiPrefs.TimeInForce.HasValue)
                        {
                            order.TIF = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(userTradingTicketUiPrefs.TimeInForce.Value.ToString());
                        }
                        else if (companyTradingTicketUiPrefs.TimeInForce.HasValue)
                        {
                            order.TIF = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(companyTradingTicketUiPrefs.TimeInForce.Value.ToString());
                        }

                        if (userTradingTicketUiPrefs.HandlingInstruction.HasValue)
                        {
                            order.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(userTradingTicketUiPrefs.HandlingInstruction.Value.ToString());
                        }
                        else if (companyTradingTicketUiPrefs.HandlingInstruction.HasValue)
                        {
                            order.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(companyTradingTicketUiPrefs.HandlingInstruction.Value.ToString());
                        }

                        if (userTradingTicketUiPrefs.TradingAccount.HasValue)
                        {
                            order.TradingAccountID = userTradingTicketUiPrefs.TradingAccount.Value;
                        }
                        else if (companyTradingTicketUiPrefs.TradingAccount.HasValue)
                        {
                            order.TradingAccountID = companyTradingTicketUiPrefs.TradingAccount.Value;
                        }

                        if (userTradingTicketUiPrefs.Broker.HasValue)
                        {
                            if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(Convert.ToInt32(userTradingTicketUiPrefs.Broker)))
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(userTradingTicketUiPrefs.Broker)].ToString());
                            }
                            else
                            {
                                if (userTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(userTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }
                                else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(companyTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }

                            }
                        }
                        else if (companyTradingTicketUiPrefs.Broker.HasValue)
                        {
                            if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(Convert.ToInt32(userTradingTicketUiPrefs.Broker)))
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(userTradingTicketUiPrefs.Broker)].ToString());
                            }
                            else
                            {
                                if (userTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(userTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }
                                else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(companyTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }
                            }
                        }
                        else
                        {
                            if (userTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(userTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                            }
                            else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(companyTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                            }
                        }

                        if (userTradingTicketUiPrefs.Strategy.HasValue)
                        {
                            order.Level2ID = int.Parse(userTradingTicketUiPrefs.Strategy.ToString());
                            order.Strategy = userTradingTicketUiPrefs.Strategy.ToString();
                        }
                        else if (companyTradingTicketUiPrefs.Strategy.HasValue)
                        {
                            order.Level2ID = int.Parse(companyTradingTicketUiPrefs.Strategy.ToString());
                            order.Strategy = companyTradingTicketUiPrefs.Strategy.ToString();
                        }

                        if (userTradingTicketUiPrefs.OrderType.HasValue)
                        {
                            order.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValueBasedOnID(userTradingTicketUiPrefs.OrderType.Value.ToString());
                        }
                        else if (companyTradingTicketUiPrefs.OrderType.HasValue)
                        {
                            order.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValueBasedOnID(companyTradingTicketUiPrefs.OrderType.Value.ToString());
                        }

                        if (userTradingTicketUiPrefs.IsSettlementCurrencyBase.HasValue)
                        {
                            if (userTradingTicketUiPrefs.IsSettlementCurrencyBase.Value)
                            {
                                order.SettlementCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                            }
                        }
                        else if (companyTradingTicketUiPrefs.IsSettlementCurrencyBase.HasValue)
                        {
                            if (companyTradingTicketUiPrefs.IsSettlementCurrencyBase.Value)
                            {
                                order.SettlementCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                            }
                        }
                        int accountID;
                        if (int.TryParse(userTradingTicketUiPrefs.Account.ToString(), out accountID))
                        {
                            order.Account = accountID.ToString();
                            order.Level1ID = accountID;
                        }
                        else if (int.TryParse(companyTradingTicketUiPrefs.Account.ToString(), out accountID))
                        {
                            order.Account = accountID.ToString();
                            order.Level1ID = accountID;
                        }
                    }
                    order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(counterPartyID);
                    order.Venue = CachedDataManager.GetInstance.GetVenueText(order.VenueID);
                    order.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(order.TradingAccountID);
                    order.CounterPartyID = counterPartyID;
                    order.Price = 0;
                    order.Quantity = 0;
                    order.ShortRebate = 0;
                    order.Symbol = symbol;
                    order.BloombergSymbol = sec.BloombergSymbol;
                    order.FactSetSymbol = sec.FactSetSymbol;
                    order.ActivSymbol = sec.ActivSymbol;
                    order.AUECID = auecID;
                    order.ContractMultiplier = sec.Multiplier;
                    int assetID = Convert.ToInt32(CachedDataManager.GetInstance.GetAssetIdByAUECId(order.AUECID));
                    int underlyingID = Convert.ToInt32(CachedDataManager.GetInstance.GetUnderlyingID(order.AUECID));
                    int exchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(order.AUECID);
                    int currencyID = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(order.AUECID);
                    order.AssetID = assetID;
                    string orderSideId = GetOrderSideTagValue(isBuyTrade, assetID); //GetPreferredOrderSide(userTradingTicketUiPrefs, companyTradingTicketUiPrefs, assetID);
                    order.OrderSideTagValue = orderSideId;
                    order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(orderSideId);
                    order.UnderlyingID = underlyingID;
                    order.ExchangeID = exchangeID;
                    order.CurrencyID = currencyID;
                    order.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(currencyID);
                     if (order.TransactionSource == TransactionSource.None)
                    {
                        order.TransactionSource = TransactionSource.TradingTicket;
                        order.TransactionSourceTag = (int)TransactionSource.TradingTicket;
                    }
                    switch (order.CurrencyName)
                    {
                        case "EUR":
                        case "GBP":
                        case "NZD":
                        case "AUD":
                            order.FXConversionMethodOperator = "M";
                            break;

                        default:
                            order.FXConversionMethodOperator = "D";
                            break;
                    }
                    if (order.CurrencyID == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                        order.FXConversionMethodOperator = "M";

                    order.BloombergSymbolWithExchangeCode = sec.BloombergSymbolWithExchangeCode;
                    return order;
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
            return new OrderSingle();
        }

        /// <summary>
        /// Get Order Side Tag Value
        /// </summary>
        /// <param name="isBuyTrade"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        private static string GetOrderSideTagValue(bool isBuyTrade, int assetId)
        {
            AssetCategory asset = Mapper.GetBaseAssetCategory((AssetCategory)assetId);

            string orderSideId = isBuyTrade ? FIXConstants.SIDE_Buy : FIXConstants.SIDE_SellShort;
            switch (asset)
            {
                case AssetCategory.Option:
                    orderSideId = isBuyTrade ? FIXConstants.SIDE_Buy_Open : FIXConstants.SIDE_Sell_Open;
                    break;
                case AssetCategory.Future:
                    orderSideId = isBuyTrade ? FIXConstants.SIDE_Buy : FIXConstants.SIDE_Sell;
                    break;
            }

            return orderSideId;
        }

        /// <summary>
        /// Sets the preferred order side.
        /// </summary>
        /// <param name="userTradingTicketuserUiPrefs">The user trading ticketuser UI prefs.</param>
        /// <param name="companyTradingTicketUiPrefs">The company trading ticket UI prefs.</param>
        private string GetPreferredOrderSide(TradingTicketUIPrefs userTradingTicketuserUiPrefs, TradingTicketUIPrefs companyTradingTicketUiPrefs, int assetID)
        {
            string tagValue = string.Empty;
            try
            {
                if (userTradingTicketuserUiPrefs.DefAssetSides.Count > 0)
                {
                    DefAssetSide userOrderSide = userTradingTicketuserUiPrefs.DefAssetSides.First(DefAssetSide => DefAssetSide.Asset == assetID);
                    if (userOrderSide.OrderSide.HasValue)
                    {
                        tagValue = TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(userOrderSide.OrderSide.Value.ToString());
                    }
                    else
                    {
                        DefAssetSide companyOrderSide = companyTradingTicketUiPrefs.DefAssetSides.First(DefAssetSide => DefAssetSide.Asset == assetID);
                        if (companyOrderSide.OrderSide.HasValue)
                        {
                            tagValue = TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(companyOrderSide.OrderSide.Value.ToString());
                        }
                        else
                        {
                            tagValue = FIXConstants.SIDE_Buy;
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
            return tagValue;
        }

        /// <summary>
        /// Links the current tab.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void LinkCurrentTab(string tabName)
        {
            try
            {
                _linkedTabName = tabName;
                _dataBaseOperationHelper.SaveLinkedTab(_linkedTabName, CompanyUserID);
                foreach (DockableControlPane pane in ultraDockManager.ControlPanes)
                {
                    CtrlWatchListTab ctrl = (CtrlWatchListTab)pane.Control;
                    ctrl.UnlinkTab();
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

        /// <summary>
        /// Determines whether [is current tab linked].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is current tab linked]; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsCurrentTabLinked(string tabName)
        {
            return tabName.Equals(_linkedTabName);
        }

        /// <summary>
        /// Links the current tab.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void UnlinkCurrentTab()
        {
            try
            {
                _linkedTabName = string.Empty;
                _dataBaseOperationHelper.SaveLinkedTab(_linkedTabName, CompanyUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This Method is use to delete all the symbols which is on current tab and delete its tab too.
        /// </summary>
        internal void DeleteTab()
        {
            try
            {
                if (ultraDockManager.ControlPanes.Count <= 0 || string.IsNullOrWhiteSpace(_currentTabName))
                {
                    return;
                }
                string gridLayoutPath = Application.StartupPath + "\\Prana Preferences\\" + CompanyUserID + "\\WatchList_GridLayout_" + _currentTabName + ".xml";
                if (File.Exists(gridLayoutPath))
                {
                    File.Delete(gridLayoutPath);
                }

                List<string> symbolsToRemove = _symbolsAddressBook.RemoveTabWithSymbols(_currentTabName);
                foreach (string symbol in symbolsToRemove)
                {
                    _marketDataHelperInstance.RemoveSingleSymbol(symbol);
                }

                _dataBaseOperationHelper.RemoveTabFromDatabase(_currentTabName, CompanyUserID);
                int index = ultraDockManager.ControlPanes.IndexOf(_currentTabName);
                if (index >= 0)
                {
                    ultraDockManager.ControlPanes[_currentTabName].Control.Dispose();
                    if (_currentTabName.Equals(_linkedTabName))
                    {
                        UnlinkCurrentTab();
                    }
                    int tabCount = ultraDockManager.ControlPanes.Count;
                    if (tabCount <= 0)
                        _currentTabName = string.Empty;
                    else if (tabCount <= index)
                    {
                        _currentTabName = ultraDockManager.ControlPanes[tabCount - 1].Key;
                        ultraDockManager.ControlPanes[tabCount - 1].Activate();
                    }
                    else
                    {
                        _currentTabName = ultraDockManager.ControlPanes[index].Key;
                        ultraDockManager.ControlPanes[index].Activate();
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

        internal bool IsTabPermanent(string tabName)
        {
            try
            {
                if (tabName == _symbolsAddressBook.PermanentTabName)
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (_timer != null)
                    _timer.Dispose();
                if (_securityMaster != null)
                    this._securityMaster.SecMstrDataResponse -= _securityMaster_SecMstrDataResponse;
                if (_marketDataHelperInstance != null)
                {
                    _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(SymbolLiveFeed_OnResponse);
                    _marketDataHelperInstance.RemoveMultipleSymbols(_symbolsAddressBook.AllSymbols);
                    _marketDataHelperInstance.RemoveMultipleSymbols(_fxSymbolMapping.Keys.ToList());
                    _marketDataHelperInstance = null;
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

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Sends the trade to tt.HighlightSymbolFromWatchlist
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs{System.String, System.String}"/> instance containing the event data.</param>
        internal void SendTradeToTT(object sender, EventArgs<string, string> e)
        {
            try
            {
                if (SendSymbolForTTToMain != null)
                    SendSymbolForTTToMain(null, e);
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

        internal void SendTradeToMTT(object sender, EventArgs<OrderBindingList> e)
        {
            try
            {
                if (SendSymbolForMTTToMain != null)
                    SendSymbolForMTTToMain(null, e);
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

        private void tabCtrl_OptionChainModuleOpened(object sender, EventArgs<int, string> e)
        {
            if (OptionChainModuleOpened != null)
                OptionChainModuleOpened(sender, e);
        }

        /// <summary>
        /// used to Export Data for automation
        /// </summary>
        /// <param name="exportFilePath"></param>
        /// <param name="tabName"></param>
        public void ExportGridData(string exportFilePath, string tabName)
        {
            try
            {
                if (ultraDockManager.ControlPanes.Count > 0)
                {
                    var currentGrid = (CtrlWatchListTab)ultraDockManager.ControlPanes[tabName].Control;
                    currentGrid.ExportGridData(exportFilePath);
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
