#region Using namespaces
using Infragistics.Windows.DataPresenter.Events;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LiveFeedProvider;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.BussinessLogic;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.ServiceConnector;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
#endregion

namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class RASImportViewModel : RebalancerBase, ILaunchForm
    {
        /// <summary>
        /// Used for checking if the call to Closing event of
        /// RAS Import is made from clicking Continue button or not.
        /// </summary>
        public bool IsContinueClicked { get; set; }

        public delegate void SetSedolSymbolColumnVisibilityDelegate(bool show);
        public SetSedolSymbolColumnVisibilityDelegate SetSedolSymbolColumnVisibility;

        public static Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseDict = new Dictionary<int, Dictionary<string, SecurityDataGridModel>>();

        #region Private attributes
        private MarketDataHelper _marketDataHelperInstance;
        private ObservableCollection<SecurityDataGridModel> _importedSecuritiesList;
        private ObservableCollection<SecurityDataGridModel> _validSecuritiesList;
        private ObservableCollection<SecurityDataGridModel> _invalidSecuritiesList;
        private ObservableCollection<string> _accountsList;
        private ApplicationConstants.SymbologyCodes _symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;

        private Dictionary<string, List<SecurityDataGridModel>> importedSecuritiesDict;
        #endregion

        #region Attributes of Old account i.e. previously assigned account of row
        public static int OldAccountId = int.MinValue;
        public static string OldAccountName = string.Empty;
        #endregion

        #region Commands
        public DelegateCommand ContinueCommand { get; set; }
        public DelegateCommand AbortCommand { get; set; }
        public DelegateCommand SecurityMasterCommand { get; set; }
        public DelegateCommand<object> RevalidateEditableCellCommand { get; set; }
        #endregion

        #region Datasources for Import view

        private bool _exportValidSecuritiesGridForAutomation;
        public bool ExportValidSecuritiesGridForAutomation
        {
            get { return _exportValidSecuritiesGridForAutomation; }
            set
            {
                _exportValidSecuritiesGridForAutomation = value;
                OnPropertyChanged("ExportValidSecuritiesGridForAutomation");
            }
        }
        private bool _exportInvalidSecuritiesGridForAutomation;
        public bool ExportInvalidSecuritiesGridForAutomation
        {
            get { return _exportInvalidSecuritiesGridForAutomation; }
            set
            {
                _exportInvalidSecuritiesGridForAutomation = value;
                OnPropertyChanged("ExportInvalidSecuritiesGridForAutomation");
            }
        }

        private string _exportFilePathForAutomation;
        public string ExportFilePathForAutomation
        {
            get { return _exportFilePathForAutomation; }
            set { _exportFilePathForAutomation = value; OnPropertyChanged("ExportFilePathForAutomation"); }
        }

        public ObservableCollection<SecurityDataGridModel> ValidSecuritiesList
        {
            get
            {
                return _validSecuritiesList;
            }
            set
            {
                _validSecuritiesList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<SecurityDataGridModel> InvalidSecuritiesList
        {
            get
            {
                return _invalidSecuritiesList;
            }
            set
            {
                _invalidSecuritiesList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> AccountsList
        {
            get
            {
                return _accountsList;
            }
            set
            {
                _accountsList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Events
        public event EventHandler LaunchForm;
        public event EventHandler UpdatedListFromImport;
        #endregion

        #region Constructor
        public RASImportViewModel(ISecurityMasterServices securityMasterInstance, IRebalancerHelper rebalancerHelperInstance)
            : base(securityMasterInstance, rebalancerHelperInstance)
        {
            ContinueCommand = new DelegateCommand(() => ContinueCommandAction());
            AbortCommand = new DelegateCommand(() => AbortCommandAction());
            SecurityMasterCommand = new DelegateCommand(() => SecurityMasterCommandAction());
            RevalidateEditableCellCommand = new DelegateCommand<object>(RevalidateEditableCellAction);
            SecurityMaster.SecMstrDataResponse += SecMasterClientSecMstrDataResponse;
            _marketDataHelperInstance = MarketDataHelper.GetInstance();
            _marketDataHelperInstance.OnResponse += new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
        }
        #endregion

        /// <summary>
        /// Sets up the Import UI with data.
        /// </summary>
        /// <param name="importedSecuritiesList">List of imported securities from file.</param>
        /// <param name="symbology">Symbology in use.</param>
        public void SetUp(ObservableCollection<SecurityDataGridModel> importedSecuritiesList, ApplicationConstants.SymbologyCodes symbology)
        {
            try
            {
                #region Initializing collections.
                ValidSecuritiesList = new ObservableCollection<SecurityDataGridModel>();
                InvalidSecuritiesList = DeepCopyHelper.Clone(importedSecuritiesList);
                importedSecuritiesDict = new Dictionary<string, List<SecurityDataGridModel>>();
                AccountsList = new ObservableCollection<string>(RebalancerCache.Instance.AccountsOrGroupsList.Values.ToList());
                _importedSecuritiesList = importedSecuritiesList;
                _symbology = symbology;
                SetSedolSymbolColumnVisibility(_symbology == ApplicationConstants.SymbologyCodes.SEDOLSymbol);

                foreach (var securityObj in InvalidSecuritiesList)
                {
                    securityObj.Symbol = securityObj.Symbol.ToUpper();
                    if(_symbology == ApplicationConstants.SymbologyCodes.SEDOLSymbol)
                    {
                        securityObj.SEDOLSymbol = securityObj.Symbol;
                    }
                    securityObj.AccountOrGroupId = securityObj.AccountOrGroupName.Equals(RebalancerConstants.CONST_ProRata) ? 0 : CachedDataManager.GetInstance.GetAccountID(securityObj.AccountOrGroupName);
                    securityObj.Remove = "Invalid Symbol";
                    if (!importedSecuritiesDict.ContainsKey(securityObj.Symbol))
                    {
                        importedSecuritiesDict.Add(securityObj.Symbol, new List<SecurityDataGridModel>());
                    }
                    importedSecuritiesDict[securityObj.Symbol].Add(securityObj);
                    if (securityObj.AccountOrGroupId != int.MinValue && RebalancerCache.Instance.AccountsOrGroupsList.ContainsKey(securityObj.AccountOrGroupId))
                    {
                        AccountWiseSecurityDataGridModel.Instance.AddModel(securityObj, AccountWiseDict);
                    }
                    if (securityObj.IncreaseDecreaseOrSet != RebalancerEnums.RASIncreaseDecreaseOrSet.Set.ToString())
                    {
                        securityObj.Target = Math.Abs(securityObj.Target); 
                    }
                }
                #endregion

                #region Sending imported symbols for validation.
                if (!_marketDataHelperInstance.IsDataManagerConnected())
                {
                    ShowErrorAlert(RebalancerConstants.MSG_LIVE_FEED_DISCONNECTED);
                }
                SendRequestForValidation(_symbology);
                #endregion
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

        /// <summary>
        /// Initialize RAS grid with Valid securities from file.
        /// </summary>
        private async void ContinueCommandAction()
        {
            try
            {
                var itemsForRemoval = new ObservableCollection<SecurityDataGridModel>();
                foreach (var item in ValidSecuritiesList)
                {
                    if (item.Remove.Equals(string.Empty))
                    {
                        itemsForRemoval.Add(item);
                        AccountWiseSecurityDataGridModel.Instance.AddModel(DeepCopyHelper.Clone(item), RebalancerViewModel.AccountWiseDict);
                        AccountWiseSecurityDataGridModel.Instance.RemoveData(item, AccountWiseDict);
                    }
                }
                if (itemsForRemoval.Count > 0)
                {
                    IsContinueClicked = true;
                }
                foreach (var item in itemsForRemoval)
                {
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ValidSecuritiesList.Remove(item);
                    }));
                }
                if (ValidSecuritiesList.Count == 0 && InvalidSecuritiesList.Count == 0)
                {
                    Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
                }
                UpdatedListFromImport(this, EventArgs.Empty);
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

        /// <summary>
        /// Closes Import UI and goes back to upload page.
        /// </summary>
        private void AbortCommandAction()
        {
            try
            {
                System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
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

        /// <summary>
        /// Opens Security Master.
        /// </summary>
        public void SecurityMasterCommandAction()
        {
            try
            {
                LaunchForm(this, EventArgs.Empty);
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

        /// <summary>
        /// Validates the attributes of the security.
        /// </summary>
        /// <param name="importedSecurity">The security to validate.</param>
        /// <returns>True if security's attributes are valid, else False.</returns>
        private void ValidateRebalanceAcrossSecurityDataFromFile(SecurityDataGridModel importedSecurity)
        {
            try
            {
                string errorString = string.Empty;
                if (importedSecurity.AccountOrGroupId == int.MinValue)
                {
                    errorString += RebalancerConstants.ERR_SECURITY_ACCOUNT_NOT_EXISTS + "\n";
                }
                else if (importedSecurity.AccountOrGroupId != int.MinValue && !RebalancerCache.Instance.AccountsOrGroupsList.ContainsKey(importedSecurity.AccountOrGroupId))
                {
                    errorString += RebalancerConstants.ERR_SECURITY_ACCOUNT_NOT_EXISTS_SELECTED_ACCOUNTORGROUP + "\n";
                }
                else
                {
                    if (AccountWiseDict.ContainsKey(importedSecurity.AccountOrGroupId))
                    {
                        decimal targetPercentageSum = 0M;
                        foreach (var securityObj in AccountWiseDict[importedSecurity.AccountOrGroupId])
                        {
                            targetPercentageSum += securityObj.Value.TargetPercentage;
                        }
                        if (targetPercentageSum > 100)
                        {
                            errorString += RebalancerConstants.ERR_SECURITY_ACCOUNT_SUM_EXCEEDS_100 + "\n";
                        }
                    }
                }
                if (!importedSecurity.IncreaseDecreaseOrSet.Equals(RebalancerConstants.CONST_SET) && importedSecurity.TargetPercentage <= 0)
                {
                    errorString += RebalancerConstants.ERR_SECURITY_TARGET_LESS_OR_EQUALS_ZERO + "\n";
                }
                if (importedSecurity.TargetPercentage > 100)
                {
                    errorString += RebalancerConstants.ERR_SECURITY_TARGET_EXCEEDS_100 + "\n";
                }
                if (string.IsNullOrWhiteSpace(importedSecurity.Symbol))
                {
                    errorString += RebalancerConstants.ERR_SECURITY_TICKER_NOT_PRESENT + "\n";
                }
                if (importedSecurity.Price == 0)
                {
                    errorString += RebalancerConstants.ERR_SECURITY_PRICE_LESS_OR_EQUALS_ZERO + "\n";
                }
                if (importedSecurity.FXRate == 0)
                {
                    errorString += RebalancerConstants.ERR_SECURITY_FX_LESS_OR_EQUALS_ZERO + "\n";
                }
                importedSecurity.Remove = errorString;
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

        /// <summary>
        /// Validates the row's attributes after any attribute has been changed.
        /// </summary>
        /// <param name="parameter">Object having the row with new data.</param>
        private void RevalidateEditableCellAction(object parameter)
        {
            try
            {
                EditModeEndedEventArgs editModeEndedEventArgs = (EditModeEndedEventArgs)parameter;
                var updatedRow = (editModeEndedEventArgs.Cell.Record.DataItem as SecurityDataGridModel);
                updatedRow.AccountOrGroupId = updatedRow.AccountOrGroupName.Equals(RebalancerConstants.CONST_ProRata) ? 0 : CachedDataManager.GetInstance.GetAccountID(updatedRow.AccountOrGroupName);
                if (updatedRow.AccountOrGroupId != int.MinValue)
                {
                    if (updatedRow.Remove.Contains(RebalancerConstants.ERR_SECURITY_ACCOUNT_NOT_EXISTS))
                    {
                        updatedRow.Remove = updatedRow.Remove.Replace(RebalancerConstants.ERR_SECURITY_ACCOUNT_NOT_EXISTS + "\n", string.Empty);
                    }

                    bool hasAccountReverted = false;
                    if (!AccountWiseDict.ContainsKey(updatedRow.AccountOrGroupId))
                    {
                        AccountWiseSecurityDataGridModel.Instance.AddModel(updatedRow, AccountWiseDict);
                    }
                    else
                    {
                        if (!AccountWiseDict[updatedRow.AccountOrGroupId].ContainsKey(updatedRow.Symbol))
                        {
                            AccountWiseDict[updatedRow.AccountOrGroupId].Add(updatedRow.Symbol, updatedRow);
                        }
                        else
                        {
                            if (updatedRow != AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol])
                            {
                                if (updatedRow.IncreaseDecreaseOrSet.Equals(AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].IncreaseDecreaseOrSet.ToString()))
                                {
                                    if (AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].TargetPercentage + updatedRow.TargetPercentage <= 100)
                                    {
                                        AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].Target += updatedRow.Target;
                                        AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].Price = RebalancerFormulas.GetWeightedPrice(AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].TargetPercentage,
                                                                                                              AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].Price, updatedRow.TargetPercentage, updatedRow.Price);
                                        ValidSecuritiesList.Remove(updatedRow);
                                        updatedRow = AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol];
                                    }
                                    else
                                    {
                                        updatedRow.AccountOrGroupId = OldAccountId;
                                        updatedRow.AccountOrGroupName = OldAccountName;
                                        hasAccountReverted = true;
                                        if (updatedRow.AccountOrGroupId == int.MinValue && !updatedRow.Remove.Contains(RebalancerConstants.ERR_SECURITY_ACCOUNT_NOT_EXISTS))
                                        {
                                            updatedRow.Remove += RebalancerConstants.ERR_SECURITY_ACCOUNT_NOT_EXISTS + "\n";
                                        }
                                    }
                                }
                                else
                                {
                                    if (updatedRow.Target > AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].Target)
                                    {
                                        AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].Target = updatedRow.Target - AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].Target;
                                        AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].IncreaseDecreaseOrSet = updatedRow.IncreaseDecreaseOrSet;
                                    }
                                    else
                                    {
                                        AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].Target = AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].Target - updatedRow.Target;
                                    }

                                    AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].Price = RebalancerFormulas.GetWeightedPrice(AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].TargetPercentage,
                                                                                                          AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol].Price, updatedRow.TargetPercentage, updatedRow.Price);
                                    ValidSecuritiesList.Remove(updatedRow);
                                    updatedRow = AccountWiseDict[updatedRow.AccountOrGroupId][updatedRow.Symbol];
                                }
                            }
                        }
                    }
                    if (!hasAccountReverted && OldAccountId != int.MinValue && AccountWiseDict.ContainsKey(OldAccountId) && AccountWiseDict[OldAccountId].ContainsKey(updatedRow.Symbol))
                    {
                        AccountWiseDict[OldAccountId].Remove(updatedRow.Symbol);
                    }
                    foreach (var accountWithSecurityObj in AccountWiseDict)
                    {
                        decimal targetPercentageSum = 0M;
                        foreach (var securityObj in accountWithSecurityObj.Value)
                        {
                            targetPercentageSum += securityObj.Value.TargetPercentage * securityObj.Value.MultiplierFactor;
                        }
                        if (targetPercentageSum > 100)
                        {
                            foreach (var obj in accountWithSecurityObj.Value)
                            {
                                if (obj.Value.AccountOrGroupId == accountWithSecurityObj.Key && !obj.Value.Remove.Contains(RebalancerConstants.ERR_SECURITY_ACCOUNT_SUM_EXCEEDS_100))
                                {
                                    obj.Value.Remove += RebalancerConstants.ERR_SECURITY_ACCOUNT_SUM_EXCEEDS_100 + "\n";
                                }
                            }
                        }
                        else
                        {
                            foreach (var obj in accountWithSecurityObj.Value)
                            {
                                if (obj.Value.AccountOrGroupId == accountWithSecurityObj.Key && obj.Value.Remove.Contains(RebalancerConstants.ERR_SECURITY_ACCOUNT_SUM_EXCEEDS_100))
                                {
                                    obj.Value.Remove = obj.Value.Remove.Replace(RebalancerConstants.ERR_SECURITY_ACCOUNT_SUM_EXCEEDS_100 + "\n", string.Empty);
                                }
                            }
                        }
                    }
                }
                if (updatedRow.Remove.Contains(RebalancerConstants.ERR_SECURITY_PRICE_LESS_OR_EQUALS_ZERO) && updatedRow.Price > 0)
                {
                    updatedRow.Remove = updatedRow.Remove.Replace(RebalancerConstants.ERR_SECURITY_PRICE_LESS_OR_EQUALS_ZERO + "\n", string.Empty);
                }
                if (updatedRow.Remove.Contains(RebalancerConstants.ERR_SECURITY_FX_LESS_OR_EQUALS_ZERO) && updatedRow.FXRate > 0)
                {
                    updatedRow.Remove = updatedRow.Remove.Replace(RebalancerConstants.ERR_SECURITY_FX_LESS_OR_EQUALS_ZERO + "\n", string.Empty);
                }
                else if (!updatedRow.Remove.Contains(RebalancerConstants.ERR_SECURITY_FX_LESS_OR_EQUALS_ZERO) && updatedRow.FXRate <= 0)
                {
                    updatedRow.Remove += RebalancerConstants.ERR_SECURITY_FX_LESS_OR_EQUALS_ZERO + "\n";
                }
                if (updatedRow.Remove.Contains(RebalancerConstants.ERR_SECURITY_TARGET_LESS_OR_EQUALS_ZERO) && !updatedRow.IncreaseDecreaseOrSet.Equals(RebalancerConstants.CONST_SET) && updatedRow.TargetPercentage > 0)
                {
                    updatedRow.Remove = updatedRow.Remove.Replace(RebalancerConstants.ERR_SECURITY_TARGET_LESS_OR_EQUALS_ZERO + "\n", string.Empty);
                }
                else if (!updatedRow.Remove.Contains(RebalancerConstants.ERR_SECURITY_TARGET_LESS_OR_EQUALS_ZERO) && !updatedRow.IncreaseDecreaseOrSet.Equals(RebalancerConstants.CONST_SET) && updatedRow.TargetPercentage <= 0)
                {
                    updatedRow.Remove += RebalancerConstants.ERR_SECURITY_TARGET_LESS_OR_EQUALS_ZERO + "\n";
                }
                if (updatedRow.Remove.Contains(RebalancerConstants.ERR_SECURITY_TARGET_EXCEEDS_100) && updatedRow.TargetPercentage <= 100)
                {
                    updatedRow.Remove = updatedRow.Remove.Replace(RebalancerConstants.ERR_SECURITY_TARGET_EXCEEDS_100 + "\n", string.Empty);
                }
                if (updatedRow.Remove.Contains(RebalancerConstants.ERR_SECURITY_ACCOUNT_NOT_EXISTS_SELECTED_ACCOUNTORGROUP) && RebalancerCache.Instance.AccountsOrGroupsList.ContainsKey(updatedRow.AccountOrGroupId))
                {
                    updatedRow.Remove = updatedRow.Remove.Replace(RebalancerConstants.ERR_SECURITY_ACCOUNT_NOT_EXISTS_SELECTED_ACCOUNTORGROUP + "\n", string.Empty);
                }
                if (ValidSecuritiesList.Count > 1)
                {
                    ValidSecuritiesList = new ObservableCollection<SecurityDataGridModel>(ValidSecuritiesList.OrderByDescending(x => x.Remove.Length));
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

        /// <summary>
        /// Sends Request for validation of symbols
        /// </summary>
        /// <param name="importedSecuritiesList">List of symbols present in file.</param>
        /// <param name="symbology">Symbology used by client.</param>
        private void SendRequestForValidation(ApplicationConstants.SymbologyCodes symbology)
        {
            try
            {
                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                foreach (var importedSecurity in importedSecuritiesDict.Keys.ToList())
                {
                    reqObj.AddData(importedSecurity.ToUpper(), symbology);
                }
                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                reqObj.HashCode = GetHashCode();
                SecurityMaster?.SendRequest(reqObj);
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

        /// <summary>
        /// Data response of symbol validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SecMasterClientSecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    SecMasterBaseObj iSecMasterBaseObj = e.Value;
                    if (iSecMasterBaseObj != null)
                    {
                        var price = RebalancerCache.Instance.GetSymbolPrice(iSecMasterBaseObj.TickerSymbol);
                        var fxRate = RebalancerCache.Instance.GetSymbolFx(iSecMasterBaseObj.TickerSymbol);
                        var symbol = _symbology == ApplicationConstants.SymbologyCodes.TickerSymbol ? iSecMasterBaseObj.TickerSymbol : iSecMasterBaseObj.SedolSymbol;
                        foreach (var item in importedSecuritiesDict[symbol])
                        {
                            item.Price = price;
                            item.FXRate = fxRate;
                            item.AUECID = iSecMasterBaseObj.AUECID;
                            item.Symbol = iSecMasterBaseObj.TickerSymbol;
                            item.FactSetSymbol = iSecMasterBaseObj.FactSetSymbol;
                            item.ActivSymbol = iSecMasterBaseObj.ActivSymbol;
                            item.BloombergSymbol = iSecMasterBaseObj.BloombergSymbol;
                            item.Asset = iSecMasterBaseObj.AssetCategory.ToString();
                            item.RoundLot = iSecMasterBaseObj.RoundLot;

                            //Assuming base currency USD
                            item.Sector = iSecMasterBaseObj.Sector;
                            item.Multiplier = (decimal)iSecMasterBaseObj.Multiplier;
                            item.Delta = (decimal)iSecMasterBaseObj.Delta;

                            item.Remove = string.Empty;

                            //TODO: Harcoded 1 for now
                            item.LeveragedFactor = 1;
                            ValidateRebalanceAcrossSecurityDataFromFile(item);
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                ValidSecuritiesList.Add(item);
                                if (ValidSecuritiesList.Count > 1)
                                {
                                    ValidSecuritiesList = new ObservableCollection<SecurityDataGridModel>(ValidSecuritiesList.OrderByDescending(x => x.Remove.Length));
                                }
                                InvalidSecuritiesList.Remove(item);
                            }));
                        }
                        if (price == 0 || fxRate == 0)
                        {
                            if (_marketDataHelperInstance != null && CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                            {
                                _marketDataHelperInstance.RequestSingleSymbol(iSecMasterBaseObj.TickerSymbol, true);
                            }
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
            });
        }

        /// <summary>
        /// Handles the OnResponse event of the LOne control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arg">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LOne_OnResponse(object sender, EventArgs<SymbolData> args)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    if (Dispatcher.CurrentDispatcher.CheckAccess())
                    {
                        if (args != null)
                        {
                            onL1Response(args.Value);
                        }
                    }
                    else
                    {
                        Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                        {
                            LOne_OnResponse(sender, args);
                        }));
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    if (rethrow)
                    {
                        throw;
                    }
                }
            });
        }

        /// <summary>
        /// Ons the l1 response.
        /// </summary>
        /// <param name="l1Data">The l1 data.</param>
        private async void onL1Response(SymbolData l1Data)
        {
            await System.Threading.Tasks.Task.Run(() => {
                try
                {
                    if (_marketDataHelperInstance != null)
                    {
                        decimal priceValue;
                        decimal fxValue;
                        decimal realTimePrice = 0;
                        string prefValue = RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalPricingFeld, 0);

                        SelectedFeedPrice enumName;
                        if (Enum.TryParse(prefValue, out enumName))
                        {
                            switch (enumName)
                            {
                                case SelectedFeedPrice.Ask:
                                    decimal.TryParse(l1Data.Ask.ToString(), out realTimePrice);
                                    break;
                                case SelectedFeedPrice.Bid:
                                    decimal.TryParse(l1Data.Bid.ToString(), out realTimePrice);
                                    break;
                                case SelectedFeedPrice.Last:
                                    decimal.TryParse(l1Data.LastPrice.ToString(), out realTimePrice);
                                    break;
                                case SelectedFeedPrice.Mid:
                                    decimal.TryParse(l1Data.Mid.ToString(), out realTimePrice);
                                    break;
                                case SelectedFeedPrice.iMid:
                                    decimal.TryParse(l1Data.iMid.ToString(), out realTimePrice);
                                    break;
                                default:
                                    decimal.TryParse(l1Data.LastPrice.ToString(), out realTimePrice);
                                    break;
                            }
                        }
                        if (_importedSecuritiesList != null)
                        {
                            priceValue = realTimePrice;
                            StringBuilder errorMessage = new StringBuilder();
                            Dictionary<int, decimal> currentAccountFxRateValue = ExpnlServiceConnector.GetInstance().GetFxRateForSymbolAndAccounts(l1Data.Symbol, new List<int> { -1 }, l1Data.AUECID, CachedDataManager.GetInstance.GetCurrencyID(l1Data.CurencyCode), ref errorMessage);
                            fxValue = currentAccountFxRateValue.Count > 0 ? currentAccountFxRateValue.FirstOrDefault().Value : 1;
                            RebalancerCache.Instance.AddOrUpdateSymbolWisePriceAndFx(l1Data.Symbol, priceValue, fxValue);
                            if (importedSecuritiesDict.ContainsKey(l1Data.Symbol))
                            {
                                foreach (var securityObj in importedSecuritiesDict[l1Data.Symbol])
                                {
                                    securityObj.Price = priceValue;
                                    securityObj.FXRate = fxValue;
                                    ValidateRebalanceAcrossSecurityDataFromFile(securityObj);
                                }
                            }
                            if (ValidSecuritiesList.Count > 1)
                            {
                                ValidSecuritiesList = new ObservableCollection<SecurityDataGridModel>(ValidSecuritiesList.OrderByDescending(x => x.Remove.Length));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    if (rethrow)
                    {
                        throw;
                    }
                }
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (SecurityMaster != null)
                {
                    SecurityMaster.SecMstrDataResponse -= SecMasterClientSecMstrDataResponse;
                    SecurityMaster = null;
                }
                if (_marketDataHelperInstance != null)
                {
                    _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                    _marketDataHelperInstance = null;
                }
                if (ValidSecuritiesList != null)
                {
                    ValidSecuritiesList = null;
                }
                if (InvalidSecuritiesList != null)
                {
                    InvalidSecuritiesList = null;
                }
                if (AccountsList != null)
                {
                    AccountsList = null;
                }
            }
        }

        /// <summary>
        /// Disposes data of Import UI on closing Rebalancer.
        /// </summary>
        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}