using ExportGridsData;
using Infragistics.Win.Misc;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Rebalancer.PercentTradingTool.Preferences;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace Prana.Rebalancer.PercentTradingTool.ViewModel
{
    /// <summary>
    /// The view model for View Allocation details window
    /// </summary>
    public class ViewAllocationDetailsViewModel : BindableBase, IExportGridData, IDisposable
    {
        /// <summary>
        /// The PTT request binding list collection view
        /// </summary>
        private BindingListCollectionView _pttRequestBindingListCollectionView;
        /// <summary>
        /// The PTT response binding list collection view
        /// </summary>
        private BindingListCollectionView _pttResponseBindingListCollectionView;
        /// <summary>
        /// The PTT unique identifier key
        /// </summary>
        private int _allocationPrefID;
        /// <summary>
        /// The _decimal precision
        /// </summary>
        private int _decimalPrecision;
        /// <summary>
        /// The _status message
        /// </summary>
        private string _statusMessage;

        /// <summary>
        /// The order side identifier
        /// </summary>
        private string _orderSideId;

        private string _symbol = string.Empty;

        private string _percentageOrBasisPointChangeText;

        private bool _isMasterFundOrAccountHidden;

        public string PercentageOrBasisPointChangeText
        {
            get { return _percentageOrBasisPointChangeText; }
            set { SetProperty(ref _percentageOrBasisPointChangeText, value); }
        }

        public string OrderSideId
        {
            get { return _orderSideId; }
            set { _orderSideId = value; }
        }

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        /// <summary>
        /// Gets or sets the PTT request binding list collection view.
        /// </summary>
        /// <value>
        /// The PTT request binding list collection view.
        /// </value>
        public BindingListCollectionView PTTRequestBindingListCollectionView
        {
            get { return _pttRequestBindingListCollectionView; }
            set { SetProperty(ref _pttRequestBindingListCollectionView, value); }
        }

        /// <summary>
        /// Gets or sets the PTT response binding list collection view.
        /// </summary>
        /// <value>
        /// The PTT response binding list collection view.
        /// </value>
        public BindingListCollectionView PTTResponseBindingListCollectionView
        {
            get { return _pttResponseBindingListCollectionView; }
            set { SetProperty(ref _pttResponseBindingListCollectionView, value); }
        }

        /// <summary>
        /// Gets or sets the PTT unique identifier key.
        /// </summary>
        /// <value>
        /// The PTT unique identifier key.
        /// </value>
        public int AllocationPrefID
        {
            get { return _allocationPrefID; }
            set { SetProperty(ref _allocationPrefID, value); }
        }

        /// <summary>
        /// Gets or sets the decimal precision.
        /// </summary>
        /// <value>
        /// The decimal precision.
        /// </value>
        public int DecimalPrecision
        {
            get { return _decimalPrecision; }
            set { SetProperty(ref _decimalPrecision, value); }
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>
        /// The status message.
        /// </value>
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        /// <summary>
        /// Gets or sets the allocation details UI loaded.
        /// </summary>
        /// <value>
        /// The allocation details UI loaded.
        /// </value>
        public ICommand AllocationDetailsUILoaded { get; set; }

        /// <summary>
        /// Gets or sets the Hidden property for MasterFundOrAccount
        /// </summary>
        /// <value>
        /// IsHidden value
        /// </value>
        public bool IsMasterFundOrAccountHidden
        {
            get { return _isMasterFundOrAccountHidden; }
            set { SetProperty(ref _isMasterFundOrAccountHidden, value); }
        }

        /// <summary>
        /// property to Export data for automation 
        /// </summary>
        private bool _exportDataGridForAutomation;
        public bool ExportDataGridForAutomation
        {
            get { return _exportDataGridForAutomation; }
            set { _exportDataGridForAutomation = value; OnPropertyChanged("ExportDataGridForAutomation"); }
        }

        /// <summary>
        /// property to store the file path for export data
        /// </summary>
        private string _exportFilePathForAutomation;
        public string ExportFilePathForAutomation
        {
            get { return _exportFilePathForAutomation; }
            set { _exportFilePathForAutomation = value; OnPropertyChanged("ExportFilePathForAutomation"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewAllocationDetailsViewModel"/> class.
        /// </summary>

        public ViewAllocationDetailsViewModel()
        {
            try
            {
                AllocationDetailsUILoaded = new DelegateCommand<object>(parameter =>
                   {
                       PTTPreferences pstPreferences = PTTPrefDataManager.GetInstance.GetPTTPreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                       DecimalPrecision = pstPreferences.IncreaseDecimalPrecision;
                       PTTAllocDetailsRequest pttRequestObject = new PTTAllocDetailsRequest();
                       List<PTTResponseObject> pttResponseObjects = new List<PTTResponseObject>();
                       StringBuilder errorMessage = new StringBuilder();
                       PTTManager.PTTGetAllocationDetails(pttRequestObject, pttResponseObjects, AllocationPrefID, Symbol, OrderSideId, errorMessage);

                       List<PTTAllocDetailsRequest> pttRequestObjects = new List<PTTAllocDetailsRequest> { pttRequestObject };
                       PTTRequestBindingListCollectionView = new BindingListCollectionView(new BindingList<PTTAllocDetailsRequest>(pttRequestObjects));
                       PTTResponseBindingListCollectionView = new BindingListCollectionView(new BindingList<PTTResponseObject>(pttResponseObjects));
                       StatusMessage = errorMessage.ToString();
                       PercentageOrBasisPointChangeText = pttRequestObjects[0].Type;
                       IsMasterFundOrAccountHidden = !string.IsNullOrEmpty(pttRequestObject.MasterFundOrAccount) ? true : false;
                   });
                InstanceManager.RegisterInstance(this);
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
        /// To Export data for Automation.
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            if (ExportDataGridForAutomation == true)
                ExportDataGridForAutomation = false;
            ExportFilePathForAutomation = filePath;
            ExportDataGridForAutomation = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    try
                    {
                        InstanceManager.ReleaseInstance(typeof(ViewAllocationDetailsViewModel));
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
    }
}