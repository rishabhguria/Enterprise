using Infragistics.Windows.DataPresenter;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Constants;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.Rebalancer.RebalancerNew.ViewModels;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace Prana.Rebalancer.RebalancerNew.Views
{

    /// <summary>
    /// Interaction logic for RebalancerMainView.xaml
    /// </summary>
    public partial class RebalancerMainView : Window, IRebalancer, IPublishing, IDisposable, ILaunchForm
    {
        private BackgroundWorker _backgroundWorker;
        private readonly SynchronizationContext _syncContext;

        public event EventHandler LaunchForm;

        public RebalancerMainView()
        {
            EnsureApplicationResources();
            InitializeComponent();
            _syncContext = SynchronizationContext.Current;
            CreateSubscriptionServicesProxy();
        }

        /// <summary>
        /// ApplicationResources
        /// </summary>
        public void EnsureApplicationResources()
        {
            try
            {
                if (System.Windows.Application.Current == null)
                {
                    // Create the Application object
                    new System.Windows.Application()
                    {
                        ShutdownMode = ShutdownMode.OnExplicitShutdown
                    };

                    // Merge in your application resources
                    System.Windows.Application.Current.Resources.MergedDictionaries.Add(
                        System.Windows.Application.LoadComponent(
                            new Uri(@"/Prana.Rebalancer;component/AppResources.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        DuplexProxyBase<ISubscription> _proxy;
        private void CreateSubscriptionServicesProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);

                _proxy.Subscribe(Topics.Topic_Rebalancer_ModelPortfolio, null);
                _proxy.Subscribe(Topics.Topic_Rebalancer_CustomGroup, null);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="topicName"></param>
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                _syncContext.Post(o =>
                {
                    Object[] dataList = null;
                    switch (topicName)
                    {
                        case Topics.Topic_Rebalancer_ModelPortfolio:

                            dataList = (System.Object[])e.EventData;

                            foreach (Object obj in dataList)
                            {
                                if (obj is ModelPortfolioDto)
                                {
                                    ModelPortfolioDto modelPortfolioDto = (ModelPortfolioDto)obj;
                                    RebalancerCache.Instance.AddOrUpdateModelPortfolio(modelPortfolioDto);
                                    ((ModelPortfolioViewModel)ModelPortfolioView.DataContext)
                                        .AddOrUpdateModelPortfolio(
                                            modelPortfolioDto);
                                }
                                else if (obj is int)
                                {
                                    int modelPortfolioId = (int)obj;
                                    RebalancerCache.Instance.DeleteModelPortfolio(modelPortfolioId);
                                    ((ModelPortfolioViewModel)ModelPortfolioView.DataContext).DeleteModelPortfolio(
                                        modelPortfolioId);
                                }
                            }
                            break;
                        case Topics.Topic_Rebalancer_CustomGroup:

                            dataList = (System.Object[])e.EventData;

                            foreach (Object obj in dataList)
                            {
                                if (obj is CustomGroupDto)
                                {
                                    CustomGroupDto customGroupDto = (CustomGroupDto)obj;
                                    RebalancerCache.Instance.AddOrUpdateCustomGroupsDictionary(customGroupDto.GroupID, customGroupDto.GroupName);
                                    RebalancerCache.Instance.AddOrUpdateCustomGroupAssociatedAccounts(customGroupDto.GroupID, customGroupDto.FundGroupMapping);
                                    ((CustomGroupsViewModel)CustomGroupsView.DataContext).AddOrUpdateCustomGroups(customGroupDto);
                                }
                                else if (obj is int)
                                {
                                    int customGroupId = (int)obj;
                                    RebalancerCache.Instance.DeleteCustomGroup(customGroupId);
                                    ((CustomGroupsViewModel)CustomGroupsView.DataContext).DeleteCustomGroupUI(customGroupId);
                                }
                            }
                            break;
                    }
                }, null);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public string getReceiverUniqueName()
        {
            return "frmRebalancer";
        }

        public object Reference()
        {
            return this;
        }

        public void SetUp()
        {
            Indicator.IsBusy = true;
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            _backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                RebalancerCache.Instance.FillRebalancerCache();
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

        RebalancerViewModel rebalancerViewModelInstance;
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                RebalancerCache.Instance.AccountsOrGroupsList.Clear();
                ModelPortfolioViewModel modelPortfolioViewModelInstance = new ModelPortfolioViewModel(SecurityMaster, RebalancerCache.Instance.RebalancerHelperInstance);
                ModelPortfolioView.DataContext = modelPortfolioViewModelInstance;
                modelPortfolioViewModelInstance.BindFormatWindow(this);
                modelPortfolioViewModelInstance.CheckUncheckFilteredRecords += new EventHandler<EventArgs<bool>>(CheckUncheckFilteredRecords);
                CustomGroupsViewModel customGroupsViewModelInstance = new CustomGroupsViewModel(SecurityMaster, RebalancerCache.Instance.RebalancerHelperInstance);
                CustomGroupsView.DataContext = customGroupsViewModelInstance;
                customGroupsViewModelInstance.BindFormatWindow(this);
                rebalancerViewModelInstance = new RebalancerViewModel(SecurityMaster, RebalancerCache.Instance.RebalancerHelperInstance);
                RebalancerView.DataContext = rebalancerViewModelInstance;
                rebalancerViewModelInstance.BindTradeListView(this);
                rebalancerViewModelInstance.LaunchForm += btnSymbolLookup_Click;
                rebalancerViewModelInstance.GetParentWindowEvent += GetWindow;
                this.DataPreferenceView.DataContext = new DataPreferencesViewModel(SecurityMaster, RebalancerCache.Instance.RebalancerHelperInstance);
                if (RebalancerView != null)
                    StatusAndErrorsDock.DataContext = ((RebalancerViewModel)RebalancerView.DataContext).StatusAndErrorMessages;
                Indicator.IsBusy = false;
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

        private void btnSymbolLookup_Click(object sender, EventArgs e)
        {
            try
            {
                ListEventAargs args = new ListEventAargs();
                Dictionary<String, String> argDict = new Dictionary<string, string>();
                argDict.Add("Action", SecMasterConstants.SecurityActions.APPROVE.ToString());
                argDict.Add(ApplicationConstants.CONST_IS_SECURITY_APPROVED, "false");
                args.argsObject = argDict;
                args.listOfValues.Add(ApplicationConstants.CONST_SYMBOL_LOOKUP);
                if (LaunchForm != null)
                {
                    LaunchForm(this, args);
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

        public ISecurityMasterServices SecurityMaster { get; set; }

        private void RebalancerMainView_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                //StringsValuesSummaryCalculator stringsValuesSummary = new StringsValuesSummaryCalculator();
                //SummaryCalculator.Register(stringsValuesSummary);
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

		/// <summary>
        /// This function is an event handler for the `Window.ContentRendered` event
		/// It is executed when the content of the window has been rendered.
        /// </summary>
        public void RebalancerMainView_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                if(rebalancerViewModelInstance != null) 
                    rebalancerViewModelInstance.IsSetCashSpecificRules();
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

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_proxy != null)
                    {
                        _proxy.UnSubscribe();
                        _proxy.Dispose();
                        _proxy = null;
                    }
                    if (_backgroundWorker != null)
                    {
                        _backgroundWorker.DoWork -= backgroundWorker_DoWork;
                        _backgroundWorker.RunWorkerCompleted -= backgroundWorker_RunWorkerCompleted;
                        _backgroundWorker.Dispose();
                        this._backgroundWorker = null;
                    }
                    if(rebalancerViewModelInstance != null)
                    {
                        rebalancerViewModelInstance.Dispose();
                    }
                    StatusAndErrorsDock.DataContext = null;
                    this.CustomGroupsTab = null;
                    this.DataPreferenceTab = null;
                    this.ModelPortfolioTab = null;
                    this.RebalancerTab = null;
                    this.SecurityMaster = null;
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

        private void RebalancerTabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count > 0)
                {
                    TabItem selectedTab = e.AddedItems[0] as TabItem;  // Gets selected tab
                    if (selectedTab != null)
                    {
                        RebalancerCache.Instance.SelectedTab = selectedTab.Name;
                        if (selectedTab.Name == RebalancerConstants.CONST_CustomGroupsTab && CustomGroupsView != null)
                        {
                            ((CustomGroupsViewModel)CustomGroupsView.DataContext).InitializeViewModel();
                            StatusAndErrorsDock.DataContext = ((CustomGroupsViewModel)CustomGroupsView.DataContext).StatusAndErrorMessages;
                        }
                        else if (selectedTab.Name == RebalancerConstants.CONST_RebalancerTab && RebalancerView != null)
                        {
                            StatusAndErrorsDock.DataContext = ((RebalancerViewModel)RebalancerView.DataContext).StatusAndErrorMessages;
                        }
                        else if (selectedTab.Name == RebalancerConstants.CONST_ModelPortfolioTab && ModelPortfolioView != null)
                        {
                            StatusAndErrorsDock.DataContext = ((ModelPortfolioViewModel)ModelPortfolioView.DataContext).StatusAndErrorMessages;
                        }
                        else if (selectedTab.Name == RebalancerConstants.CONST_DataPreferenceTab && DataPreferenceView != null)
                        {
                            StatusAndErrorsDock.DataContext = ((DataPreferencesViewModel)DataPreferenceView.DataContext).StatusAndErrorMessages;
                        }

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

        private void RebalancerMainView_OnClosed(object sender, EventArgs e)
        {

            try
            {
                ModelPortfolioViewModel modelPortfolioViewModelInstance = ModelPortfolioView.DataContext as ModelPortfolioViewModel;
                if (modelPortfolioViewModelInstance != null)
                {
                    modelPortfolioViewModelInstance.Dispose();
                    modelPortfolioViewModelInstance = null;
                    ModelPortfolioView.DataContext = null;
                    this.ModelPortfolioView = null;

                }
                CustomGroupsViewModel customGroupsViewModelInstance = CustomGroupsView.DataContext as CustomGroupsViewModel;
                if (customGroupsViewModelInstance != null)
                {
                    customGroupsViewModelInstance.Dispose();
                    customGroupsViewModelInstance = null;
                    CustomGroupsView.DataContext = null;
                    this.CustomGroupsView = null;

                }
                RebalancerViewModel rebalancerViewModelInstance = RebalancerView.DataContext as RebalancerViewModel;
                if (rebalancerViewModelInstance != null)
                {
                    rebalancerViewModelInstance.LaunchForm -= btnSymbolLookup_Click;
                    rebalancerViewModelInstance.Dispose();
                    rebalancerViewModelInstance = null;
                    RebalancerView.Dispose();
                    //Need to uncomment when infragistics updated to latest version
                    //RebalancerView.DataContext = null;
                    this.RebalancerView = null;

                }
                DataPreferencesViewModel dataPreferencesViewModelInstance = this.DataPreferenceView.DataContext as DataPreferencesViewModel;
                if (dataPreferencesViewModelInstance != null)
                {
                    dataPreferencesViewModelInstance.Dispose();
                    dataPreferencesViewModelInstance = null;
                    DataPreferenceView.DataContext = null;
                    this.DataPreferenceView = null;

                }
                RebalancerCache.Instance.ClearCache();
                Dispose();
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

        private void CheckUncheckFilteredRecords(object sender, EventArgs<bool> args)
        {
            try
            {
                foreach (DataRecord datarow in ModelPortfolioView.ModelPortfolioGrid.RecordManager.GetFilteredInDataRecords())
                {
                    ((PortfolioDto)datarow.DataItem).IsChecked = args.Value;
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

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion

        private Window GetWindow()
        {
            return this;
        }
    }
}
