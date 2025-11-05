using Prana.Allocation.Client.Forms.ViewModels;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows;

namespace Prana.Allocation.Client.Forms.Views
{
    /// <summary>
    /// Interaction logic for AllocationClient.xaml
    /// </summary>
    public partial class AllocationClient : Window
    {
        public AllocationClient()
        {
            try
            {
                InitializeComponent();
                AddInfragisticsSourceDictionary();
                AllocationClientViewModel.IsLoadLayout = true;
                AllocationClientViewModel.LoadSymbolLookupEvent += AllocationClient_LoadSymbolLookupEvent;
                AllocationClientViewModel.LoadAuditUIEvent += AllocationClient_LoadAuditUIEvent;
                AllocationClientViewModel.LoadCloseTradeUIEvent += AllocationClient_LoadCloseTradeUIEvent;
                AllocationClientViewModel.LoadCashTransactionUIEvent += AllocationClient_LoadCashTransactionUIEvent;
                AllocationClientViewModel.CloseAllocationClient += AllocationClientViewModel_CloseAllocationClient;
                AllocationClientViewModel.AllocationDataChange += AllocationClientViewModel_AllocationDataChange;
                this.Closing += AllocationClient_Closing;

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            }
        }

        /// <summary>
        /// Occurs when [get audit click].
        /// </summary>
        public event EventHandler GetAuditClick;

        /// <summary>
        /// Occurs when [genrate cash transaction].
        /// </summary>
        public event EventHandler GenrateCashTransaction;

        /// <summary>
        /// Occurs when [load close trade UI from allocation].
        /// </summary>
        public event EventHandler<EventArgs<BusinessObjects.AllocationGroup>> LoadCloseTradeUIFromAllocation;

        /// <summary>
        /// Occurs when [load symbol look up UI from allocation].
        /// </summary>
        public event EventHandler<EventArgs<string>> LoadSymbolLookUpUIFromAllocation;

        /// <summary>
        /// Occurs when [allocation data change].
        /// </summary>
        public event EventHandler<EventArgs<bool>> AllocationDataChange;

        /// <summary>
        /// Allocation Client View Model
        /// </summary>
        public AllocationClientViewModel AllocationClientViewModel
        {
            set { this.DataContext = value; }
            get { return (AllocationClientViewModel)this.DataContext; }
        }

        /// <summary>
        /// Handles the AllocationDataChange event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.Boolean}"/> instance containing the event data.</param>
        void AllocationClientViewModel_AllocationDataChange(object sender, EventArgs<bool> e)
        {
            try
            {
                if (AllocationDataChange != null)
                    AllocationDataChange(this, new EventArgs<bool>(e.Value));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            }
        }

        /// <summary>
        /// Handles the LoadCashTransactionUIEvent event of the AllocationClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void AllocationClient_LoadCashTransactionUIEvent(object sender, EventArgs e)
        {
            try
            {
                if (GenrateCashTransaction != null)
                    GenrateCashTransaction(this, e);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the LoadCloseTradeUIEvent event of the AllocationClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AllocationGroup"/> instance containing the event data.</param>
        void AllocationClient_LoadCloseTradeUIEvent(object sender, EventArgs<BusinessObjects.AllocationGroup> e)
        {
            try
            {
                if (LoadCloseTradeUIFromAllocation != null)
                    LoadCloseTradeUIFromAllocation(this, new EventArgs<AllocationGroup>(e.Value));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the LoadAuditUIEvent event of the AllocationClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void AllocationClient_LoadAuditUIEvent(object sender, EventArgs e)
        {
            try
            {
                if (GetAuditClick != null)
                    GetAuditClick(this, e);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the LoadSymbolLookupEvent event of the AllocationClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="string"/> instance containing the event data.</param>
        void AllocationClient_LoadSymbolLookupEvent(object sender, EventArgs<string> e)
        {
            try
            {
                if (LoadSymbolLookUpUIFromAllocation != null)
                    LoadSymbolLookUpUIFromAllocation(this, new EventArgs<string>(e.Value));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the Closing event of the AllocationClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        void AllocationClient_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                AllocationClientViewModel.AllocationWindowClosing(e);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the CloseAllocationClient event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AllocationClientViewModel_CloseAllocationClient(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }



        private void AddInfragisticsSourceDictionary()
        {
            try
            {
                if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    ResourceDictionary rd = new ResourceDictionary();
                    rd.Source = new Uri(@"/Prana.Allocation.Client;component/Themes/IG/IG.xamDataPresenter.xaml", UriKind.Relative);
                    this.Resources.MergedDictionaries.Add(rd);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (AllocationClientViewModel != null)
                    {
                        AllocationClientViewModel.Dispose();
                        AllocationClientViewModel.LoadSymbolLookupEvent -= AllocationClient_LoadSymbolLookupEvent;
                        AllocationClientViewModel.LoadAuditUIEvent -= AllocationClient_LoadAuditUIEvent;
                        AllocationClientViewModel.LoadCloseTradeUIEvent -= AllocationClient_LoadCloseTradeUIEvent;
                        AllocationClientViewModel.LoadCashTransactionUIEvent -= AllocationClient_LoadCashTransactionUIEvent;
                        AllocationClientViewModel.CloseAllocationClient -= AllocationClientViewModel_CloseAllocationClient;
                        AllocationClientViewModel.AllocationDataChange -= AllocationClientViewModel_AllocationDataChange;
                        this.Closing -= AllocationClient_Closing;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Applies the filter and get data.
        /// </summary>
        /// <param name="groupIds">The group ids.</param>
        /// <param name="tradeDates">The trade dates.</param>
        /// <param name="loadData"></param>
        public void ApplyFilterAndGetData(string groupIds, DateTime fromDate, DateTime toDate, bool loadData = true)
        {
            try
            {
                AllocationClientViewModel.ApplyFilterAndGetData(groupIds, fromDate, toDate, loadData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
