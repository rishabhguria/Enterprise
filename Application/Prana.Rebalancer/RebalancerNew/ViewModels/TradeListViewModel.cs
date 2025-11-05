using Newtonsoft.Json;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.Rebalancer.Classes;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.Rebalancer.RebalancerNew.Views;
using Prana.ServiceConnector;
using Prana.TradeManager.Extension;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;

namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class TradeListViewModel : BindableBase, IDisposable
    {
        BackgroundWorker _bgSendToStaging = null;
        BackgroundWorker _bgSave = null;
        private SynchronizationContext _uiSyncContext;
        private AccountsModel _selectedAccountOrGroup;
        // private ObservableCollection<AccountsModel> _accountsAndGroupsList;
        private List<int> _accountList;

        public EventHandler<EventArgs<bool>> CheckUncheckFilteredRecords;
        public delegate List<TradeListModel> GetFilteredTradeListModel();
        public event GetFilteredTradeListModel GetFilteredTradeListModelEvent;
        public delegate Window GetParentWindow();
        public event GetParentWindow GetParentWindowEvent;       
        #region Commands

        /// <summary>
        /// SendToStaging Command
        /// </summary>
        public DelegateCommand SendToStagingCommand { get; set; }

        /// <summary>
        /// CheckCompliance Command
        /// </summary>
        public ICommand CheckComplianceButtonClicked { get; set; }

        /// <summary>
        /// Save command
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// ExportTradeList Command
        /// </summary>
        public DelegateCommand ExportTradeListCommand { get; set; }
        #endregion

        #region Properties

        private bool _isCheckComplianceAllowed;
        public bool IsCheckComplianceAllowed
        {
            get { return _isCheckComplianceAllowed; }
            set { SetProperty(ref _isCheckComplianceAllowed, value); }
        }

        private bool _isUseCustodianBrokerPreference;
        public bool IsUseCustodianBrokerPreference
        {
            get { return _isUseCustodianBrokerPreference; }
            set { SetProperty(ref _isUseCustodianBrokerPreference, value); }
        }

        private bool _isUseCustodianBroker;
        public bool IsUseCustodianBroker
        {
            get { return _isUseCustodianBroker; }
            set { SetProperty(ref _isUseCustodianBroker, value); }
        }

        private CompanyUser _loginUser;
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { SetProperty(ref _loginUser, value); }
        }

        private bool _enableButtons;
        public bool EnableButtons
        {
            get { return _enableButtons; }
            set { SetProperty(ref _enableButtons, value); }
        }

        private bool _enableGrid;
        public bool EnableGrid
        {
            get { return _enableGrid; }
            set { SetProperty(ref _enableGrid, value); }
        }

        private bool _inProgress;
        public bool InProgress
        {
            get { return _inProgress; }
            set { SetProperty(ref _inProgress, value); }
        }

        private string _inProgressMessage;
        public string InProgressMessage
        {
            get { return _inProgressMessage; }
            set { SetProperty(ref _inProgressMessage, value); }
        }

        private bool isAllTradesChecked = true;
        public bool IsAllTradesChecked
        {
            get { return isAllTradesChecked; }
            set
            {
                isAllTradesChecked = value;
                CheckUncheckFilteredRecords(this, new EventArgs<bool>(isAllTradesChecked));
                OnPropertyChanged();
            }
        }

        public bool IsCurrListSaved { get; set; }

        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        private string statusMessage;

        public string StatusMessage
        {
            get { return statusMessage; }
            set
            {
                statusMessage = value;
                OnPropertyChanged();
            }
        }

        Dictionary<string, int> DictTradeListNamesandId { get; set; }


        private KeyValuePair<string, ObservableCollection<TradeListModel>> currTradeList;
        public KeyValuePair<string, ObservableCollection<TradeListModel>> CurrTradeList
        {

            get { return currTradeList; }
            set
            {
                currTradeList = value;
            }
        }

        private ObservableCollection<TradeListModel> tradeList;
        public ObservableCollection<TradeListModel> TradeList
        {

            get { return tradeList; }
            set
            {
                tradeList = value;
                OnPropertyChanged();
            }
        }


        private DateTime selectedDate = DateTime.Now.Date;

        public DateTime SelectedDate
        {

            get { return selectedDate; }
            set
            {
                selectedDate = value;
                OnPropertyChanged();
                GetTradeListNames();
            }
        }

        private ObservableCollection<string> tradeListNames;
        public ObservableCollection<string> TradeListNames
        {

            get { return tradeListNames; }
            set
            {
                tradeListNames = value;
                OnPropertyChanged();
            }
        }

        private string smartName;
        public string SmartName
        {
            get { return smartName; }
            set
            {
                smartName = value;
                OnPropertyChanged();
            }
        }

        private string selectedTradeListName;
        public string SelectedTradeListName
        {

            get { return selectedTradeListName; }
            set
            {
                selectedTradeListName = value;
                OnPropertyChanged();
                if (SelectedTradeListName != null)
                {
                    if (!IsCurrListSaved && SelectedTradeListName.Equals(CurrTradeList.Key))
                    {
                        string str = JsonConvert.SerializeObject(CurrTradeList.Value);
                        TradeList = JsonConvert.DeserializeObject<ObservableCollection<TradeListModel>>(str);
                    }
                    else
                    {
                        GetTradeList();
                    }
                    SmartName = SelectedTradeListName;
                }
                AddUpdateStatusAndMessage(string.Empty, string.Empty);
            }
        }

        private bool _exportTradeList;
        //private readonly string RebalancerConstantsMSG_COMPLIANCE_VALIDATED;

        public bool ExportTradeList
        {
            get { return _exportTradeList; }
            set
            {
                _exportTradeList = value;
                OnPropertyChanged("ExportTradeList");
            }
        }

        private bool _exportTradeListGridForAutomation;
        //private readonly string RebalancerConstantsMSG_COMPLIANCE_VALIDATED;

        public bool ExportTradeListGridForAutomation
        {
            get { return _exportTradeListGridForAutomation; }
            set
            {
                _exportTradeListGridForAutomation = value;
                OnPropertyChanged("ExportTradeListGridForAutomation");
            }
        }

        private string _exportFilePathForAutomation;
        public string ExportFilePathForAutomation
        {
            get { return _exportFilePathForAutomation; }
            set { _exportFilePathForAutomation = value; OnPropertyChanged("ExportFilePathForAutomation"); }
        }

        private bool _enableExport;
        public bool EnableExport
        {
            get { return _enableExport; }
            set
            {
                _enableExport = value;
                OnPropertyChanged(nameof(EnableExport));
            }

        }

        /// <summary>
        /// _checkComplianceConnector_FeedbackMessageReceived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _checkComplianceConnector_FeedbackMessageReceived(object sender, EventArgs<DataSet> e)
        {
            try
            {
                InProgressMessage = HelperFunctionsForCompliance.CheckComplianceConnector_FeedbackMessageReceived(e, _loginUser.CompanyUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region Constructor
        public TradeListViewModel()
        {
            LoginUser = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser;
            if (ComplianceCacheManager.CheckIsBasketComplianceEnabled(_loginUser.CompanyUserID))
            {
                IsCheckComplianceAllowed = true;
                CheckComplianceFeedback checkComplianceFeedback = new CheckComplianceFeedback();
                checkComplianceFeedback.FeedbackMessageReceived += _checkComplianceConnector_FeedbackMessageReceived;
            }
            if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
            {
                EnableExport = false;
            }
            else
                EnableExport = true;
            _uiSyncContext = SynchronizationContext.Current;
            TradeListNames = new ObservableCollection<string>();
            SendToStagingCommand = new DelegateCommand(() => SendToStagingCommandAction());
            CheckComplianceButtonClicked = new Prism.Commands.DelegateCommand<object>(CheckComplianceAction, canExecute => IsCheckComplianceAllowed);
            SaveCommand = new DelegateCommand(() => SaveCommandAction());
            ExportTradeListCommand = new DelegateCommand(() => { ExportTradeList = true; });
            _bgSendToStaging = new BackgroundWorker();
            _bgSendToStaging.DoWork += _bgSendToStaging_DoWork;
            _bgSave = new BackgroundWorker();
            _bgSave.DoWork += _bgSave_DoWork;
            _bgSave.RunWorkerCompleted += _bgSave_RunWorkerCompleted;
        }

        #endregion

        #region Methods

        internal void SetUp(ObservableCollection<TradeListModel> tradeList, AccountsModel selectedAccountOrGroup, List<int> accountList)
        {
            IsCurrListSaved = false;
            CurrTradeList = new KeyValuePair<string, ObservableCollection<TradeListModel>>(RebalancerServiceApi.GetInstance().GetSmartName(), tradeList);
            SelectedDate = DateTime.Now.Date;
            _selectedAccountOrGroup = selectedAccountOrGroup;
            _accountList = accountList;
            EnableButtonsAndGrid(true);
            IsUseCustodianBrokerPreference = TradingTktPrefs.TTGeneralPrefs.IsUseCustodianAsExecutingBroker;
            IsUseCustodianBroker = IsUseCustodianBrokerPreference;
        }

        private void AddUpdateStatusAndMessage(string statusMessage, string errorMessage)
        {
            StatusMessage = statusMessage;
            ErrorMessage = errorMessage;
        }

        private void _bgSendToStaging_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread thread = new Thread((ThreadStart)(() =>
            {
                List<object> arguments = e.Argument as List<object>;
                if (arguments != null && arguments.Count > 0)
                {
                    List<TradeListModel> lstTrades = arguments[0] as List<TradeListModel>;
                    SendTradesToBlotter(lstTrades);
                }
                EnableButtonsAndGrid(true);
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void _bgSave_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> arguments = e.Argument as List<object>;
            if (arguments != null && arguments.Count > 0)
            {
                DateTime date = Convert.ToDateTime(arguments[0]);// as DateTime;
                string name = arguments[1] as string;
                string strTradeList = arguments[2] as string;
                int id = Convert.ToInt32(arguments[3]);// as int;
                bool isSaved = RebalancerServiceApi.GetInstance().SaveRebalancerTradeList(date, name, id, strTradeList);
                List<object> argument = new List<object>();
                argument.Add(isSaved);
                argument.Add(name);
                argument.Add(id);
                e.Result = argument;
            }
        }

        private void _bgSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // check error, check cancel, then use result
                if (e.Error != null)
                {
                    AddUpdateStatusAndMessage(string.Empty, e.Error.Message.ToString());
                }
                else if (e.Cancelled)
                {
                    // handle cancellation
                }

                List<object> arguments = e.Result as List<object>;
                if (arguments != null && arguments.Count > 0)
                {
                    bool isSaved = Convert.ToBoolean(arguments[0]);// as bool;
                    string name = arguments[1] as string;
                    int id = Convert.ToInt32(arguments[2]);
                    if (id == int.MinValue)
                    {
                        IsCurrListSaved = true;
                    }
                    if (!String.IsNullOrEmpty(ErrorMessage))
                    {
                        AddUpdateStatusAndMessage(StatusMessage, string.Empty);
                        return;
                    }
                    GetTradeListNames();
                    SelectedTradeListName = name;
                    if (!isSaved)
                        AddUpdateStatusAndMessage(string.Empty, "Data not Saved.");
                    else
                        AddUpdateStatusAndMessage("Saved Successfully.", string.Empty);
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
            //finally
            //{
            //    EnableButtonsAndGrid(false);
            //}
        }

        private TradeListSummaryView _tradeListSummaryViewInstance;
        private TradeListSummaryViewModel _tradeListSummaryViewModelInstance;
        public void BindTradeListSummaryView()
        {
            _tradeListSummaryViewModelInstance = new TradeListSummaryViewModel();
            _tradeListSummaryViewInstance = new TradeListSummaryView();
            _tradeListSummaryViewInstance.DataContext = _tradeListSummaryViewModelInstance;
            _tradeListSummaryViewInstance.Owner = GetParentWindowEvent();
            ElementHost.EnableModelessKeyboardInterop(_tradeListSummaryViewInstance);
            _tradeListSummaryViewInstance.ShowInTaskbar = true;
        }

        /// <summary>
        /// SendToStaging Command Action
        /// </summary>
        /// <returns></returns>
        private void SendToStagingCommandAction()
        {
            try
            {
                InProgressMessage = ComplainceConstants.Feedback_Initial_Msg;
                List<TradeListModel> lstTrades = GetFilteredTradeListModelEvent();
                lstTrades = lstTrades.Where(d => d.IsChecked == true).ToList();
                List<object> arguments = new List<object>();
                arguments.Add(lstTrades);
                if (lstTrades.Count > 0)
                {
                    BindTradeListSummaryView();
                    _tradeListSummaryViewModelInstance.SetUp(lstTrades);
                    bool _showSaveTradePopup;
                    bool.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalShowSaveTradePopup, 0), out _showSaveTradePopup);
                    if (!IsCurrListSaved && SelectedTradeListName.Equals(CurrTradeList.Key))
                    {
                        if (!_showSaveTradePopup)
                        {
                            SaveCommandAction(true);
                            _tradeListSummaryViewModelInstance.CaptionToDisplay = "Send the Trades to Staging?";
                            _tradeListSummaryViewInstance.SetUpForSaveTradeList(false);
                            _tradeListSummaryViewInstance.ShowDialog();
                            if (_tradeListSummaryViewModelInstance.Yes)
                            {
                                EnableButtonsAndGrid(false);
                                _bgSendToStaging.RunWorkerAsync(arguments);
                            }
                        }
                        else
                        {
                            _tradeListSummaryViewModelInstance.CaptionToDisplay = "Save the Trades before Send to Staging?";
                            _tradeListSummaryViewInstance.SetUpForSaveTradeList(true);
                            _tradeListSummaryViewInstance.ShowDialog();
                            if (_tradeListSummaryViewModelInstance.Yes)
                            {
                                EnableButtonsAndGrid(false);
                                SaveCommandAction(true);
                                _bgSendToStaging.RunWorkerAsync(arguments);

                            }
                            else if (_tradeListSummaryViewModelInstance.No)
                            {
                                EnableButtonsAndGrid(false);
                                _bgSendToStaging.RunWorkerAsync(arguments);
                            }
                            else if (_tradeListSummaryViewModelInstance.Cancel)
                            {
                                EnableButtonsAndGrid(true);
                            }
                        }
                    }
                    else
                    {
                        _tradeListSummaryViewModelInstance.CaptionToDisplay = "Send the Trades to Staging?";
                        _tradeListSummaryViewInstance.SetUpForSaveTradeList(false);
                        _tradeListSummaryViewInstance.ShowDialog();
                        if (_tradeListSummaryViewModelInstance.Yes)
                        {
                            EnableButtonsAndGrid(false);
                            _bgSendToStaging.RunWorkerAsync(arguments);
                        }
                    }
                }
                else
                {
                    AddUpdateStatusAndMessage("No trade is selected", string.Empty);
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
        /// Checks the compliance action.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        private void CheckComplianceAction(object parentWindow)
        {
            try
            {
                InProgressMessage = ComplainceConstants.Feedback_Initial_Msg;
                System.Threading.Tasks.Task.Run(() =>
                {
                    EnableButtonsAndGrid(false);
                    List<TradeListModel> lstTrades = TradeList.Where(d => d.IsChecked == true).ToList();
                    StringBuilder errorMsg = new StringBuilder();
                    Logger.LoggerWrite("Sending Orders to create preference for Check Compliance from Trade List.", LoggingConstants.CATEGORY_GENERAL_COMPLIANCE);
                    List<OrderSingle> listOfOrders = RebalancerCommon.Instance.GroupStagedOrders(lstTrades, ref errorMsg, LoginUser.CompanyUserID, SmartName);
                    Logger.LoggerWrite("Preference created for Check Compliance from Trade List.", LoggingConstants.CATEGORY_GENERAL_COMPLIANCE);
                    AddUpdateStatusAndMessage(String.Format("Sending {0} orders for compliance validation.", listOfOrders.Count), string.Empty);
                    UpdateOrdersBasedOnCustodianBrokerPreference(listOfOrders);
                    if (listOfOrders.Count > 0 && string.IsNullOrEmpty(errorMsg.ToString()))
                    {
                        //StringBuilder prefMessage = new StringBuilder(string.Empty);
                        //foreach (var singleOrder in listOfOrders)
                        //{
                        //    StringBuilder preferenceErrorMessage = ValidateAndCreatOrderForCompliance(singleOrder);
                        //    if (!string.IsNullOrEmpty(preferenceErrorMessage.ToString()) && string.IsNullOrEmpty(prefMessage.ToString()))
                        //    {
                        //        prefMessage.Append(preferenceErrorMessage);
                        //    }
                        //}
                        SendOrdersForComplianceCheck(listOfOrders, errorMsg);
                    }
                    else
                    {
                        DialogService.DialogServiceInstance.ShowMessageBox(this, RebalancerConstants.MSG_NO_ITEM_CHECKED,
                                   RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, true);
                    }
                    //AddUpdateStatusAndMessage(String.Format("Compliance validated successfully for {0} orders.", listOfOrders.Count), string.Empty);
                }).ContinueWith(o =>
                {
                    EnableButtonsAndGrid(true);
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            finally
            {
                EnableButtonsAndGrid(true);
            }
        }

        /// <summary>
        /// Validates the order and create complete order for compliance.
        /// </summary>
        /// <param name="singleOrder">The single order.</param>
        /// <returns></returns>
        private static StringBuilder ValidateAndCreatOrderForCompliance(OrderSingle singleOrder)
        {
            StringBuilder preferenceErrorMessage = new StringBuilder();
            try
            {
                HelperFunctionsForCompliance.CreateCompleteOrderForStageAndCompliance(singleOrder, ref preferenceErrorMessage);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceErrorMessage;
        }

        /// <summary>
        /// Sends the List of orders for compliance check.
        /// </summary>
        /// <param name="orderList">The order list.</param>
        /// <param name="preferenceErrorMessage">The preference error message.</param>
        private void SendOrdersForComplianceCheck(List<OrderSingle> orderList, StringBuilder preferenceErrorMessage)
        {
            try
            {
                if (preferenceErrorMessage.Length == 0)
                {
                    int mfID = RebalancerCommon.Instance.GetMasterFundIDfromAccounts(_selectedAccountOrGroup.Type, _accountList);
                    int tradingAccountID = CachedDataManager.GetTradingAccountForMasterFund(mfID);
                    string masterFundName = CachedDataManager.GetInstance.GetMasterFund(mfID); ;
                    if (mfID > 0)
                    {
                        orderList.ForEach((OrderSingle order) =>
                        {
                            order.MasterFund = masterFundName;
                            if (tradingAccountID > 0)
                                order.TradingAccountID = tradingAccountID;
                        });
                    }
                    bool isRealTimePositions = RebalancerCommon.Instance.GetIsRealTimePositionsPreference();
                    ComplianceCommon.SendCashAmountForAccountsFromRebalancer(RebalancerCommon.Instance.cashFlow);
                    Logger.LoggerWrite(string.Format("Sending {0} orders for compliance validation.", orderList.Count), LoggingConstants.CATEGORY_GENERAL_COMPLIANCE);
                    var complianceCheckResult = ComplianceCommon.CheckCompliance(orderList, LoginUser.CompanyUserID, isRealTimePositions, true);
                    if (complianceCheckResult == null)
                    {
                        AddUpdateStatusAndMessage(String.Format(RebalancerConstants.MSG_COMPLIANCE_NOT_VALIDATED, orderList.Count), string.Empty);
                        StatusMessage = preferenceErrorMessage.ToString();
                    }
                    else if (complianceCheckResult.Equals(false))
                    {
                        DialogService.DialogServiceInstance.ShowMessageBox(this, RebalancerConstants.MSG_NO_RULES_VIOLATED,
                                    RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, true);
                        AddUpdateStatusAndMessage(RebalancerConstants.MSG_NO_RULES_VIOLATED, string.Empty);
                    }
                    else
                    {
                        string statusBarMsg = complianceCheckResult.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID)
                            ? PreTradeConstants.ConstBasketComplianceFailed
                            : string.Format(RebalancerConstants.MSG_COMPLIANCE_VALIDATED, orderList.Count);
                        AddUpdateStatusAndMessage(statusBarMsg, string.Empty);
                        Logger.LoggerWrite(string.Format(RebalancerConstants.MSG_COMPLIANCE_VALIDATED, orderList.Count), LoggingConstants.CATEGORY_GENERAL_COMPLIANCE);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// SendTradesToBlotter
        /// </summary>
        /// <param name="lstTrades"></param>
        private void SendTradesToBlotter(List<TradeListModel> lstTrades)
        {
            try
            {
                int tradesStagingCount = 0;

                AddUpdateStatusAndMessage(String.Format("Sending {0} trades to staging.", lstTrades.Count), string.Empty);
                StringBuilder errorMsg = new StringBuilder();
                Logger.LoggerWrite(string.Format("Sending Orders to create preference for Send to stage from Trade List."), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                List<OrderSingle> lstOrders = RebalancerCommon.Instance.GroupStagedOrders(lstTrades, ref errorMsg, LoginUser.CompanyUserID, SmartName, IsUseCustodianBroker);
                Logger.LoggerWrite(string.Format("Preference created for Send to stage from Trade List."), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                HashSet<Guid> tradeHashSet = new HashSet<Guid>();

                if (errorMsg.Length == 0)
                {
                    var listAfterGrouping = lstTrades.GroupBy(x => new { x.Symbol, x.Side }).Select(group => new { Name = group.Key, Count = group.Count() }).OrderByDescending(x => x.Count);

                    foreach (var item in listAfterGrouping)
                    {
                        if (item.Count > 1)
                        {
                            string key = item.Name.Symbol.ToString() + Seperators.SEPERATOR_6 + item.Name.Side.ToString();
                            foreach (TradeListModel tradeLisItem in TradeList)
                            {
                                if (key.Equals(tradeLisItem.Symbol + Seperators.SEPERATOR_6 + tradeLisItem.Side))
                                {
                                    Guid guidAfterGrouping = lstOrders.Where(x => (x.Symbol == tradeLisItem.Symbol) && (x.OrderSide == tradeLisItem.Side.ToString())).Select(x => x.OrderSingleGuid).FirstOrDefault();
                                    tradeLisItem.TradeGuid = guidAfterGrouping;
                                }
                            }
                        }
                    }

                    UpdateOrdersBasedOnCustodianBrokerPreference(lstOrders);

                    bool isComplianceValid = true;
                    if (ComplianceCacheManager.CheckIsBasketComplianceEnabled(_loginUser.CompanyUserID))
                    {
                        bool isRealTimePositions = RebalancerCommon.Instance.GetIsRealTimePositionsPreference();
                        ComplianceCommon.SendCashAmountForAccountsFromRebalancer(RebalancerCommon.Instance.cashFlow);
                        AddUpdateStatusAndMessage(String.Format("Sending {0} orders for compliance validation.", lstOrders.Count), string.Empty);
                        Logger.LoggerWrite(string.Format("Sending {0} orders for compliance validation.", lstOrders.Count), LoggingConstants.CATEGORY_GENERAL_COMPLIANCE);
                        isComplianceValid = ComplianceCommon.ValidateOrderInCompliance_New(lstOrders, null, LoginUser.CompanyUserID, false, isRealTimePositions, true);
                        AddUpdateStatusAndMessage(String.Format("Compliance validated successfully for {0} orders.", lstOrders.Count), string.Empty);
                        Logger.LoggerWrite(string.Format("Compliance validated successfully for {0} orders.", lstOrders.Count), LoggingConstants.CATEGORY_GENERAL_COMPLIANCE);
                    }

                    if (isComplianceValid)
                    {
                        foreach (OrderSingle orderSingle in lstOrders)
                        {
                            bool isTradeStagedSuccessfully = false;                          
                            isTradeStagedSuccessfully = TradeManager.TradeManager.GetInstance().SendBlotterTrades(orderSingle, 0);

                            if (isTradeStagedSuccessfully)
                            {
                                tradeHashSet.Add(orderSingle.OrderSingleGuid);
                                tradesStagingCount++;
                                AddUpdateStatusAndMessage(String.Format("{0} trades staged successfully.", tradesStagingCount), string.Empty);
                            }
                        }
                        if (tradesStagingCount == lstOrders.Count)
                        {
                            AddUpdateStatusAndMessage(String.Format("{0} selected trades staged successfully.", tradesStagingCount), string.Empty);
                        }
                        else
                        {
                            AddUpdateStatusAndMessage(tradesStagingCount + " trades staged successfully.", lstOrders.Count - tradesStagingCount + " trades failed to send to staging.");
                        }

                        foreach (TradeListModel tradeLisItem in TradeList)
                        {
                            if (tradeHashSet.Contains(tradeLisItem.TradeGuid))
                            {
                                tradeLisItem.IsStaged = true;
                                tradeLisItem.IsChecked = false;
                            }
                        }
                    }
                }
                else
                {
                    AddUpdateStatusAndMessage(errorMsg.ToString(), errorMsg.ToString());
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SaveCommandAction(bool frmSendToStaging = false)
        {
            try
            {
                if (TradeList.Count > 0)
                {
                    if (!IsSmartNameDuplicate(SmartName) || SelectedTradeListName.Equals(SmartName))
                    {

                        bool userInput = false;
                        if (!frmSendToStaging)
                        {
                            MessageBoxResult rsltMessageBox = MessageBox.Show("Do you want to Save the data?", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                                            MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                            userInput = rsltMessageBox == MessageBoxResult.Yes;
                        }
                        if (frmSendToStaging || userInput)
                        {
                            string strTradeList = JsonConvert.SerializeObject(TradeList);
                            string smartName = SmartName;
                            List<object> arguments = new List<object>();
                            arguments.Add(SelectedDate);
                            arguments.Add(SmartName);
                            arguments.Add(strTradeList);

                            if (DictTradeListNamesandId.ContainsKey(SelectedTradeListName))
                            {
                                arguments.Add(DictTradeListNamesandId[SelectedTradeListName]);
                                _bgSave.RunWorkerAsync(arguments);
                                //isSaved = RebalancerServiceApi.GetInstance().SaveRebalancerTradeList(SelectedDate, SmartName, DictTradeListNamesandId[SelectedTradeListName], strTradeList);
                            }
                            else
                            {
                                arguments.Add(int.MinValue);
                                _bgSave.RunWorkerAsync(arguments);
                                //isSaved = RebalancerServiceApi.GetInstance().SaveRebalancerTradeList(SelectedDate, SmartName, int.MinValue, strTradeList);

                            }

                        }

                    }
                    else
                    {
                        AddUpdateStatusAndMessage(string.Empty, "SmartName is already present");
                    }
                }
                else
                {
                    AddUpdateStatusAndMessage(string.Empty, "Can not Save Empty List");
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
        /// 
        /// </summary>
        /// <param name="smartName"></param>
        /// <returns></returns>
        private bool IsSmartNameDuplicate(string smartName)
        {
            if (DictTradeListNamesandId.ContainsKey(smartName))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// GetTradeListNames
        /// </summary>
        private void GetTradeListNames()
        {
            AddUpdateStatusAndMessage("Fetching Data...", string.Empty);
            DictTradeListNamesandId = RebalancerServiceApi.GetInstance().GetRebalancerTradeListNames(SelectedDate);
            TradeListNames.Clear();
            TradeListNames.AddRange(DictTradeListNamesandId.Keys);
            if (!IsCurrListSaved && SelectedDate == DateTime.Now.Date)
            {
                TradeListNames.Add(CurrTradeList.Key);
                SelectedTradeListName = CurrTradeList.Key;
            }
            else if (TradeListNames.Count > 0)
            {
                SelectedTradeListName = TradeListNames[0];
            }
            else
            {
                SmartName = string.Empty;
                TradeList.Clear();
                AddUpdateStatusAndMessage("No Data to show.", string.Empty);
            }
        }

        /// <summary>
        /// Update Orders Based On Custodian Broker Preference
        /// </summary>
        private void UpdateOrdersBasedOnCustodianBrokerPreference(List<OrderSingle> listOfOrders)
        {
            try
            {
                foreach (OrderSingle orderSingle in listOfOrders)
                {
                    if (IsUseCustodianBroker)
                    {
                        orderSingle.IsUseCustodianBroker = true;
                        var allocationPreference = RebalancerCommon.Instance.GetAllocationOperationPreference(orderSingle.Level1ID);
                        List<int> accountIds = new List<int>();
                        if (allocationPreference != null)
                        {
                            accountIds = allocationPreference.TargetPercentage.Keys.ToList();
                        }
                        else
                        {
                            accountIds.Add(orderSingle.Level1ID);
                        }
                        orderSingle.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, orderSingle.CounterPartyID));
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

        /// <summary>
        /// 
        /// </summary>
        private void GetTradeList()
        {
            if (DictTradeListNamesandId.ContainsKey(SelectedTradeListName))
            {
                string xlmTradeListData = RebalancerServiceApi.GetInstance().GetTradeList(DictTradeListNamesandId[SelectedTradeListName]);
                TradeList = JsonConvert.DeserializeObject<ObservableCollection<TradeListModel>>(xlmTradeListData);
            }
            AddUpdateStatusAndMessage(string.Empty, string.Empty);
        }

        private void EnableButtonsAndGrid(bool isEnabled)
        {
            try
            {
                InProgress = !isEnabled;
                EnableGrid = isEnabled;
                EnableButtons = isEnabled;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region IDisposable

        public void DisposeData()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_bgSendToStaging != null)
                    {
                        _bgSendToStaging.DoWork -= _bgSendToStaging_DoWork;
                        _bgSendToStaging.Dispose();
                    }
                    if (_bgSave != null)
                    {
                        _bgSave.DoWork -= _bgSave_DoWork;
                        _bgSave.RunWorkerCompleted -= _bgSave_RunWorkerCompleted;
                        _bgSave.Dispose();
                    }
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
        }

        #endregion
    }
}
