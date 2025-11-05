using Infragistics.Controls.Editors;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.Helper;
using Prana.Allocation.Common.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Prana.Allocation.Client.ViewModelBase" />
    public class AllocationGridControlViewModel : ViewModelBase, IDisposable
    {
        #region Events

        /// <summary>
        /// Occurs when [active allocated group event].
        /// </summary>
        public event EventHandler<EventArgs<AllocationGroup>> ActiveAllocatedGroupEvent;

        /// <summary>
        /// Occurs when [active un allocated group event].
        /// </summary>
        public event EventHandler<EventArgs<AllocationGroup>> ActiveUnAllocatedGroupEvent;

        /// <summary>
        /// Occurs when [header checkbox changed un allocated group event].
        /// </summary>
        public event EventHandler<EventArgs<List<AllocationGroup>>> HeaderCheckboxChangeUnAllocatedGroupEvent;

        /// <summary>
        /// Occurs when [delete allocation group event].
        /// </summary>
        public event EventHandler<EventArgs<List<AllocationGroup>>> DeleteAllocationGroupEvent;

        /// <summary>
        /// Occurs when [group ungroup event].
        /// </summary>
        public event EventHandler<EventArgs<List<AllocationGroup>, string>> GroupUngroupEvent;

        /// <summary>
        /// Occurs when [load audit trail UI].
        /// </summary>
        public event EventHandler LoadAuditTrailUI;

        /// <summary>
        /// Occurs when [load cash transaction UI].
        /// </summary>
        public event EventHandler LoadCashTransactionUI;

        /// <summary>
        /// Occurs when [load close trade UI].
        /// </summary>
        public event EventHandler<EventArgs<AllocationGroup>> LoadCloseTradeUI;

        /// <summary>
        /// Occurs when [load symbol look up UI].
        /// </summary>
        public event EventHandler<EventArgs<string>> LoadSymbolLookUpUI;

        /// <summary>
        /// Occurs when [open external transaction UI].
        /// </summary>
        internal event EventHandler OpenExternalTransactionUI;

        /// <summary>
        /// Occurs when [save layout event].
        /// </summary>
        public event EventHandler<EventArgs<bool>> SaveLayoutEvent;

        /// <summary>
        /// Occurs when [unallocate groups event].
        /// </summary>
        public event EventHandler<EventArgs<List<AllocationGroup>>> UnallocateGroupsEvent;

        /// <summary>
        /// Occurs when [update total no of trades].
        /// </summary>
        public event EventHandler<EventArgs<int, int>> UpdateTotalNoOfTrades;

        /// <summary>
        /// Occurs when [set account strategy control event].
        /// </summary>
        public event EventHandler<EventArgs<bool>> SetAccountStrategyControlEvent;
        #endregion Events

        #region Members

        /// <summary>
        /// The _is filters allocated cleared
        /// </summary>
        private bool _isFiltersAllocatedCleared;

        /// <summary>
        /// The _allocated groups collection
        /// </summary>
        private BindingListCollectionView _allocatedGroupsCollection;

        /// <summary>
        /// The _un allocated groups collection
        /// </summary>
        private BindingListCollectionView _unAllocatedGroupsCollection;

        /// <summary>
        /// The export unallocated grid
        /// </summary>
        private bool _exportUnallocatedGrid;

        /// <summary>
        /// The export allocated grid
        /// </summary>
        private bool _exportAllocatedGrid;

        /// <summary>
        /// The export taxlots
        /// </summary>
        private bool _exportTaxlots;

        /// <summary>
        /// The export groups
        /// </summary>
        private bool _exportGroups;

        /// <summary>
        /// The export both grids
        /// </summary>
        private bool _exportBothGrids;

        /// <summary>
        /// The _expand collapse all
        /// </summary>
        private bool _isExpandCollapseAll;

        /// <summary>
        /// The _is expand collapse allocated grid
        /// </summary>
        private bool _isExpandCollapseAllocatedGrid;

        /// <summary>
        /// The is allocated un group menu enabled
        /// </summary>
        private bool _isAllocatedUnGroupMenuEnabled;

        /// <summary>
        /// The _menu item visibility
        /// </summary>
        private Visibility _taxlotMenuItemVisibility;

        /// <summary>
        /// The _enable disable menus
        /// </summary>
        private bool _unallocateMenuVisibility;

        /// <summary>
        /// The _is group menu enabled
        /// </summary>
        private bool _isGroupMenuEnabled;

        /// <summary>
        /// The is un allocated security master enabled
        /// </summary>
        private bool _isUnAllocatedSecurityMasterEnabled;

        /// <summary>
        /// The is allocated security master enabled
        /// </summary>
        private bool _isAllocatedSecurityMasterEnabled;

        /// <summary>
        /// The is un allocated audit trail enabled
        /// </summary>
        private bool _isUnAllocatedAuditTrailEnabled;

        /// <summary>
        /// The is allocated close order enabled
        /// </summary>
        private bool _isAllocatedCloseOrderEnabled;

        /// <summary>
        /// The is un allocated audit trail enabled
        /// </summary>
        private bool _isAllocatedAuditTrailEnabled;

        /// <summary>
        /// The _is un group menu enabled
        /// </summary>
        private bool _isUnGroupMenuEnabled;

        /// <summary>
        /// The _is save layout allocated grid
        /// </summary>
        private bool _isSaveLayoutAllocatedGrid;

        /// <summary>
        /// The _is save layout unallocated grid
        /// </summary>
        private bool _isSaveLayoutUnallocatedGrid;

        /// <summary>
        /// The _is clear allocated selected items
        /// </summary>
        private bool _isClearAllocatedSelectedItems;

        /// <summary>
        /// The _assets with commission in net amount list
        /// </summary>
        private List<int> _assetsWithCommissionInNetAmountList;

        /// <summary>
        /// The _is delete menu enabled
        /// </summary>
        private Visibility _deleteMenuVisibility;

        /// <summary>
        /// The trade attributes keep records
        /// </summary>
        private CustomValueEnteredActions[] _tradeAttributesKeepRecords;

        /// <summary>
        /// The _selected single item allocated
        /// </summary>
        private object _activeAllocatedDataItem;

        /// <summary>
        /// The _is clear un allocated selected items
        /// </summary>
        private bool _isClearUnAllocatedSelectedItems;

        /// <summary>
        /// The _active unallocated data item
        /// </summary>
        private object _activeUnallocatedDataItem;

        /// <summary>
        /// The _counter party
        /// </summary>
        private ObservableDictionary<int, string> _counterParty;

        /// <summary>
        /// The _third party
        /// </summary>
        private ObservableDictionary<int, string> _thirdParty;

        /// <summary>
        /// The _venues
        /// </summary>
        private ObservableDictionary<int, string> _venues;

        /// <summary>
        /// The _sides
        /// </summary>
        private ObservableDictionary<string, string> _sides;

        /// <summary>
        /// The TransactionType
        /// </summary>
        private ObservableDictionary<string, string> _transactionType;

        /// <summary>
        /// The _FX conversion operator
        /// </summary>
        private ObservableCollection<EnumerationValue> _fxConversionOperator;

        /// <summary>
        /// The _settl conversion operator
        /// </summary>
        private ObservableCollection<EnumerationValue> _settlConversionOperator;

        /// <summary>
        /// The _precision format
        /// </summary>
        private string _precisionFormat;

        /// <summary>
        /// The _qty precision format
        /// </summary>
        private string _qtyPrecisionFormat;

        /// <summary>
        /// The _total no of trades un allocated grid
        /// </summary>
        private int _totalNoOfTradesUnAllocatedGrid;

        /// <summary>
        /// The _total no of trades allocated grid
        /// </summary>
        private int _totalNoOfTradesAllocatedGrid;

        /// <summary>
        /// The _selected no of trades Allocated Grid
        /// </summary>
        private int _selectedNoOfTradesAllocatedGrid;

        /// <summary>
        /// The _selected no of trades UnAllocated Grid
        /// </summary>
        private int _selectedNoOfTradesUnAllocatedGrid;

        /// <summary>
        /// The _end edit mode and commit changes
        /// </summary>
        private bool _endEditModeAndCommitChanges;

        /// <summary>
        /// The _trade attributes collection
        /// </summary>
        private ObservableCollection<string>[] _tradeAttributesCollection;

        /// <summary>
        /// The _allow edit grid
        /// </summary>
        private bool _allowEditGrid;

        /// <summary>
        /// The _is allocated header checked
        /// </summary>
        private bool _isAllocatedHeaderChecked;

        /// <summary>
        /// The _is un allocated header checked
        /// </summary>
        private bool _isUnAllocatedHeaderChecked;

        /// <summary>
        /// The _is refresh after get data
        /// </summary>
        private bool _isRefreshAfterGetData;

        /// <summary>
        /// The _is filters cleared for un allocated grid
        /// </summary>
        private bool _isFiltersClearedForUnAllocatedGrid;

        /// <summary>
        /// The _is open add and update extenal transaction UI
        /// </summary>
        private bool _isOpenAddAndUpdateExtenalTransactionUi;

        /// <summary>
        /// The _trade attribute caption1
        /// </summary>
        private string _tradeAttributeCaption1;

        /// <summary>
        /// The _trade attribute caption2
        /// </summary>
        private string _tradeAttributeCaption2;

        /// <summary>
        /// The _trade attribute caption3
        /// </summary>
        private string _tradeAttributeCaption3;

        /// <summary>
        /// The _trade attribute caption4
        /// </summary>
        private string _tradeAttributeCaption4;

        /// <summary>
        /// The _trade attribute caption5
        /// </summary>
        private string _tradeAttributeCaption5;

        /// <summary>
        /// The _trade attribute caption6
        /// </summary>
        private string _tradeAttributeCaption6;

        /// <summary>
        /// The trade attribute 7 to 45 captions dictionary
        /// </summary>
        private ObservableDictionary<int, string> _additionalTradeAttributesCaptions;
        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the selected single item allocated.
        /// </summary>
        /// <value>
        /// The selected single item allocated.
        /// </value>
        public object ActiveAllocatedDataItem
        {
            get { return _activeAllocatedDataItem; }
            set
            {
                // added this check so event is raised only once for same value of activated data item
                if (!(_activeAllocatedDataItem != null && value != null && _activeAllocatedDataItem.Equals(value)))
                {
                    _activeAllocatedDataItem = value;
                    if (_activeAllocatedDataItem != null)
                    {
                        // clear Unallocated active/selected items
                        IsClearUnAllocatedSelectedItems = true;

                        if (_activeAllocatedDataItem is TaxLot)
                            TaxlotMenuItemVisibility = Visibility.Visible;
                        else
                            TaxlotMenuItemVisibility = Visibility.Collapsed;
                    }
                    RaisePropertyChangedEvent("ActiveAllocatedDataItem");
                }
            }
        }

        /// <summary>
        /// Gets or sets the active unallocated data item.
        /// </summary>
        /// <value>
        /// The active unallocated data item.
        /// </value>
        public object ActiveUnallocatedDataItem
        {
            get { return _activeUnallocatedDataItem; }
            set
            {
                // added this check so event is raised only once for same value of activated data item
                if (!(_activeUnallocatedDataItem != null && value != null && _activeUnallocatedDataItem.Equals(value)))
                {
                    _activeUnallocatedDataItem = value;
                    if (_activeUnallocatedDataItem != null)
                    {
                        // clear allocated active/selected items
                        IsClearAllocatedSelectedItems = true;
                    }
                    RaisePropertyChangedEvent("ActiveUnallocatedDataItem");
                }
            }
        }

        /// <summary>
        /// Gets or sets the allocated groups collection.
        /// </summary>
        /// <value>
        /// The allocated groups collection.
        /// </value>
        public BindingListCollectionView AllocatedGroupsCollection
        {
            get { return _allocatedGroupsCollection; }
            set
            {
                _allocatedGroupsCollection = value;
                RaisePropertyChangedEvent("AllocatedGroupsCollection");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow edit grid].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow edit grid]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowEditGrid
        {
            get { return _allowEditGrid; }
            set
            {
                _allowEditGrid = value;
                RaisePropertyChangedEvent("AllowEditGrid");
            }
        }

        /// <summary>
        /// Gets or sets the assets with commission in net amount list.
        /// </summary>
        /// <value>
        /// The assets with commission in net amount list.
        /// </value>
        public List<int> AssetsWithCommissionInNetAmountList
        {
            get { return _assetsWithCommissionInNetAmountList; }
            set
            {
                _assetsWithCommissionInNetAmountList = value;
                RaisePropertyChangedEvent("AssetsWithCommissionInNetAmountList");
            }
        }


        /// <summary>
        /// Gets or sets the counter party.
        /// </summary>
        /// <value>
        /// The counter party.
        /// </value>
        public ObservableDictionary<int, string> CounterParty
        {
            get { return _counterParty; }
            set
            {
                _counterParty = value;
                RaisePropertyChangedEvent("CounterParty");
            }
        }

        /// <summary>
        /// Gets or sets the third party.
        /// </summary>
        /// <value>
        /// The third party.
        /// </value>
        public ObservableDictionary<int, string> ThirdParty
        {
            get { return _thirdParty; }
            set
            {
                _thirdParty = value;
                RaisePropertyChangedEvent("ThirdParty");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is delete menu enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is delete menu enabled; otherwise, <c>false</c>.
        /// </value>
        public Visibility DeleteMenuVisibility
        {
            get { return _deleteMenuVisibility; }
            set
            {
                _deleteMenuVisibility = value;
                RaisePropertyChangedEvent("DeleteMenuVisibility");
            }
        }

        /// <summary>
        /// Gets or sets the trade attributes keep records.
        /// </summary>
        /// <value>
        /// The trade attributes keep records.
        /// </value>
        public CustomValueEnteredActions[] TradeAttributesKeepRecords
        {
            get { return _tradeAttributesKeepRecords; }
            set
            {
                _tradeAttributesKeepRecords = value;
                RaisePropertyChangedEvent("TradeAttributesKeepRecords");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [end edit mode and commit changes].
        /// </summary>
        /// <value>
        /// <c>true</c> if [end edit mode and commit changes]; otherwise, <c>false</c>.
        /// </value>
        public bool EndEditModeAndCommitChanges
        {
            get { return _endEditModeAndCommitChanges; }
            set
            {
                _endEditModeAndCommitChanges = value;
                RaisePropertyChangedEvent("EndEditModeAndCommitChanges");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [export grid allocated grid].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export grid allocated grid]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportGridAllocatedGrid
        {
            get { return _exportAllocatedGrid; }
            set
            {
                _exportAllocatedGrid = value;
                RaisePropertyChangedEvent("ExportGridAllocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [export grid taxlots].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export grid taxlots]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportGridTaxlots
        {
            get { return _exportTaxlots; }
            set
            {
                _exportTaxlots = value;
                RaisePropertyChangedEvent("ExportGridTaxlots");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [export grid groups].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export grid groups]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportGridGroups
        {
            get { return _exportGroups; }
            set
            {
                _exportGroups = value;
                RaisePropertyChangedEvent("ExportGridGroups");
            }
        }

        private bool _exportDataGridUnallocated;
        public bool ExportDataGridUnallocated
        {
            get { return _exportDataGridUnallocated; }
            set
            {
                _exportDataGridUnallocated = value;
                RaisePropertyChangedEvent("ExportDataGridUnallocated");
            }
        }
        private bool _exportDataGridAllocated;
        public bool ExportDataGridAllocated
        {
            get { return _exportDataGridAllocated; }
            set
            {
                _exportDataGridAllocated = value;
                RaisePropertyChangedEvent("ExportDataGridAllocated");
            }
        }

        private string _exportFilePathForAutomation;
        public string ExportFilePathForAutomation
        {
            get { return _exportFilePathForAutomation; }
            set { _exportFilePathForAutomation = value; RaisePropertyChangedEvent("ExportFilePathForAutomation"); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [export grid].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export grid]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportGridUnAllocatedGrid
        {
            get { return _exportUnallocatedGrid; }
            set
            {
                _exportUnallocatedGrid = value;
                RaisePropertyChangedEvent("ExportGridUnAllocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [export both grids].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export both grids]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportBothGrids
        {
            get { return _exportBothGrids; }
            set
            {
                _exportBothGrids = value;
                RaisePropertyChangedEvent("ExportBothGrids");
            }
        }

        /// <summary>
        /// Gets or sets the fx conversion operator.
        /// </summary>
        /// <value>
        /// The fx conversion operator.
        /// </value>
        public ObservableCollection<EnumerationValue> FxConversionOperator
        {
            get { return _fxConversionOperator; }
            set
            {
                _fxConversionOperator = value;
                RaisePropertyChangedEvent("FxConversionOperator");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocated header checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocated header checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocatedHeaderChecked
        {
            get { return _isAllocatedHeaderChecked; }
            set
            {
                _isAllocatedHeaderChecked = value;
                if (_isAllocatedHeaderChecked)
                {
                    IsClearUnAllocatedSelectedItems = true;
                }
                RaisePropertyChangedEvent("IsAllocatedHeaderChecked");

                if (SetAccountStrategyControlEvent != null)
                    SetAccountStrategyControlEvent(this, new EventArgs<bool>(false));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is clear allocated selected items.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is clear allocated selected items; otherwise, <c>false</c>.
        /// </value>
        public bool IsClearAllocatedSelectedItems
        {
            get { return _isClearAllocatedSelectedItems; }
            set
            {
                _isClearAllocatedSelectedItems = value;
                RaisePropertyChangedEvent("IsClearAllocatedSelectedItems");

                if (_isClearAllocatedSelectedItems)
                {
                    ActiveAllocatedDataItem = null;
                    ClearSelectedGroups(AllocatedGroupsCollection);
                    IsClearAllocatedSelectedItems = false;
                    IsAllocatedHeaderChecked = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is clear un allocated selected items.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is clear un allocated selected items; otherwise, <c>false</c>.
        /// </value>
        public bool IsClearUnAllocatedSelectedItems
        {
            get { return _isClearUnAllocatedSelectedItems; }
            set
            {
                _isClearUnAllocatedSelectedItems = value;
                RaisePropertyChangedEvent("IsClearUnAllocatedSelectedItems");

                if (_isClearUnAllocatedSelectedItems)
                {
                    ActiveUnallocatedDataItem = null;
                    ClearSelectedGroups(UnAllocatedGroupsCollection);
                    IsClearUnAllocatedSelectedItems = false;
                    IsUnAllocatedHeaderChecked = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expand collapse all.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expand collapse all; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpandCollapseAll
        {
            get { return _isExpandCollapseAll; }
            set
            {
                _isExpandCollapseAll = value;
                RaisePropertyChangedEvent("IsExpandCollapseAll");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expand collapse allocated grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expand collapse allocated grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpandCollapseAllocatedGrid
        {
            get { return _isExpandCollapseAllocatedGrid; }
            set
            {
                _isExpandCollapseAllocatedGrid = value;
                RaisePropertyChangedEvent("IsExpandCollapseAllocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocated un group menu enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocated un group menu enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocatedUnGroupMenuEnabled
        {
            get { return _isAllocatedUnGroupMenuEnabled; }
            set
            {
                _isAllocatedUnGroupMenuEnabled = value;
                RaisePropertyChangedEvent("IsAllocatedUnGroupMenuEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is filters cleared for allocated grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is filters cleared for allocated grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsFiltersClearedForAllocatedGrid
        {
            get { return _isFiltersAllocatedCleared; }
            set
            {
                _isFiltersAllocatedCleared = value;
                RaisePropertyChangedEvent("IsFiltersClearedForAllocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is filters cleared for un allocated grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is filters cleared for un allocated grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsFiltersClearedForUnAllocatedGrid
        {
            get { return _isFiltersClearedForUnAllocatedGrid; }
            set
            {
                _isFiltersClearedForUnAllocatedGrid = value;
                RaisePropertyChangedEvent("IsFiltersClearedForUnAllocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is group menu enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is group menu enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsGroupMenuEnabled
        {
            get { return _isGroupMenuEnabled; }
            set
            {
                _isGroupMenuEnabled = value;
                RaisePropertyChangedEvent("IsGroupMenuEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is un allocated security master enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is un allocated security master enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnAllocatedSecurityMasterEnabled
        {
            get { return _isUnAllocatedSecurityMasterEnabled; }
            set
            {
                _isUnAllocatedSecurityMasterEnabled = value;
                RaisePropertyChangedEvent("IsUnAllocatedSecurityMasterEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocated security master enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocated security master enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocatedSecurityMasterEnabled
        {
            get { return _isAllocatedSecurityMasterEnabled; }
            set
            {
                _isAllocatedSecurityMasterEnabled = value;
                RaisePropertyChangedEvent("IsAllocatedSecurityMasterEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is un allocated audit trail enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is un allocated audit trail enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnAllocatedAuditTrailEnabled
        {
            get { return _isUnAllocatedAuditTrailEnabled; }
            set
            {
                _isUnAllocatedAuditTrailEnabled = value;
                RaisePropertyChangedEvent("IsUnAllocatedAuditTrailEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocated audit trail enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocated audit trail enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocatedAuditTrailEnabled
        {
            get { return _isAllocatedAuditTrailEnabled; }
            set
            {
                _isAllocatedAuditTrailEnabled = value;
                RaisePropertyChangedEvent("IsAllocatedAuditTrailEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocated close order enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocated close order enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocatedCloseOrderEnabled
        {
            get { return _isAllocatedCloseOrderEnabled; }
            set
            {
                _isAllocatedCloseOrderEnabled = value;
                RaisePropertyChangedEvent("IsAllocatedCloseOrderEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open add and update extenal transaction UI.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is open add and update extenal transaction UI; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpenAddAndUpdateExtenalTransactionUi
        {
            get { return _isOpenAddAndUpdateExtenalTransactionUi; }
            set
            {
                _isOpenAddAndUpdateExtenalTransactionUi = value;
                if (_isOpenAddAndUpdateExtenalTransactionUi)
                    if (OpenExternalTransactionUI != null)
                        OpenExternalTransactionUI(this, EventArgs.Empty);
                RaisePropertyChangedEvent("IsOpenAddAndUpdateExtenalTransactionUi");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is refresh after get data.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is refresh after get data; otherwise, <c>false</c>.
        /// </value>
        public bool IsRefreshAfterGetData
        {
            get { return _isRefreshAfterGetData; }
            set
            {
                _isRefreshAfterGetData = value;
                RaisePropertyChangedEvent("IsRefreshAfterGetData");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save layout allocated grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save layout allocated grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveLayoutAllocatedGrid
        {
            get { return _isSaveLayoutAllocatedGrid; }
            set
            {
                _isSaveLayoutAllocatedGrid = value;
                RaisePropertyChangedEvent("IsSaveLayoutAllocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save layout unallocated grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save layout unallocated grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveLayoutUnallocatedGrid
        {
            get { return _isSaveLayoutUnallocatedGrid; }
            set
            {
                _isSaveLayoutUnallocatedGrid = value;
                RaisePropertyChangedEvent("IsSaveLayoutUnallocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is un allocated header checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is un allocated header checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnAllocatedHeaderChecked
        {
            get { return _isUnAllocatedHeaderChecked; }
            set
            {
                _isUnAllocatedHeaderChecked = value;
                if (_isUnAllocatedHeaderChecked)
                {
                    IsClearAllocatedSelectedItems = true;
                }
                RaisePropertyChangedEvent("IsUnAllocatedHeaderChecked");

                if (SetAccountStrategyControlEvent != null)
                    SetAccountStrategyControlEvent(this, new EventArgs<bool>(true));
                if (HeaderCheckboxChangeUnAllocatedGroupEvent != null)
                    HeaderCheckboxChangeUnAllocatedGroupEvent(this, new EventArgs<List<AllocationGroup>>(GetSelectedUnAllocatedGroups()));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is un group menu enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is un group menu enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnGroupMenuEnabled
        {
            get { return _isUnGroupMenuEnabled; }
            set
            {
                _isUnGroupMenuEnabled = value;
                RaisePropertyChangedEvent("IsUnGroupMenuEnabled");
            }
        }

        /// <summary>
        /// Gets or sets the precision format.
        /// </summary>
        /// <value>
        /// The precision format.
        /// </value>
        public string PrecisionFormat
        {
            get { return _precisionFormat; }
            set
            {
                _precisionFormat = value;
                RaisePropertyChangedEvent("PrecisionFormat");
            }
        }

        /// <summary>
        /// Gets or sets the qty precision format.
        /// </summary>
        /// <value>
        /// The qty precision format.
        /// </value>
        public string QtyPrecisionFormat
        {
            get { return _qtyPrecisionFormat; }
            set
            {
                _qtyPrecisionFormat = value;
                RaisePropertyChangedEvent("QtyPrecisionFormat");
            }
        }

        /// <summary>
        /// Gets or sets the selected no of trades Allocated Grid.
        /// </summary>
        /// <value>
        /// The selected no of trades Allocated Grid.
        /// </value>
        public int SelectedNoOfTradesAllocatedGrid
        {
            get { return _selectedNoOfTradesAllocatedGrid; }
            set
            {
                _selectedNoOfTradesAllocatedGrid = value;

                if (UpdateTotalNoOfTrades != null)
                    UpdateTotalNoOfTrades(this, new EventArgs<int, int>(_selectedNoOfTradesAllocatedGrid, _totalNoOfTradesAllocatedGrid));
                //   RaisePropertyChangedEvent("SelectedNoOfTradesAllocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets the selected no of trades UnAllocated Grid.
        /// </summary>
        /// <value>
        /// The selected no of trades UnAllocated Grid.
        /// </value>
        public int SelectedNoOfTradesUnAllocatedGrid
        {
            get { return _selectedNoOfTradesUnAllocatedGrid; }
            set
            {
                _selectedNoOfTradesUnAllocatedGrid = value;

                if (UpdateTotalNoOfTrades != null)
                    UpdateTotalNoOfTrades(this, new EventArgs<int, int>(_selectedNoOfTradesUnAllocatedGrid, _totalNoOfTradesUnAllocatedGrid));
                // RaisePropertyChangedEvent("SelectedNoOfTradesUnAllocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets the settl conversion operator.
        /// </summary>
        /// <value>
        /// The settl conversion operator.
        /// </value>
        public ObservableCollection<EnumerationValue> SettlConversionOperator
        {
            get { return _settlConversionOperator; }
            set
            {
                _settlConversionOperator = value;
                RaisePropertyChangedEvent("SettlConversionOperator");
            }
        }


        /// <summary>
        /// Gets or sets the sides.
        /// </summary>
        /// <value>
        /// The sides.
        /// </value>
        public ObservableDictionary<string, string> Sides
        {
            get { return _sides; }
            set
            {
                _sides = value;
                RaisePropertyChangedEvent("Sides");
            }
        }

        /// <summary>
        /// Gets or sets the menu item visibility.
        /// </summary>
        /// <value>
        /// The menu item visibility.
        /// </value>
        public Visibility TaxlotMenuItemVisibility
        {
            get { return _taxlotMenuItemVisibility; }
            set
            {
                _taxlotMenuItemVisibility = value;
                RaisePropertyChangedEvent("TaxlotMenuItemVisibility");
            }
        }

        /// <summary>
        /// Gets or sets the total no of trades allocated grid.
        /// </summary>
        /// <value>
        /// The total no of trades allocated grid.
        /// </value>
        public int TotalNoOfTradesAllocatedGrid
        {
            get { return _totalNoOfTradesAllocatedGrid; }
            set
            {
                _totalNoOfTradesAllocatedGrid = value;
                //  RaisePropertyChangedEvent("TotalNoOfTradesAllocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets the total no of trades un allocated grid.
        /// </summary>
        /// <value>
        /// The total no of trades un allocated grid.
        /// </value>
        public int TotalNoOfTradesUnAllocatedGrid
        {
            get { return _totalNoOfTradesUnAllocatedGrid; }
            set
            {
                _totalNoOfTradesUnAllocatedGrid = value;
                //RaisePropertyChangedEvent("TotalNoOfTradesUnAllocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets the trade attributes collection.
        /// </summary>
        /// <value>
        /// The trade attributes collection.
        /// </value>
        public ObservableCollection<string>[] TradeAttributesCollection
        {
            get { return _tradeAttributesCollection; }
            set
            {
                _tradeAttributesCollection = value;
                RaisePropertyChangedEvent("TradeAttributesCollection");
            }
        }

        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>
        /// The type of the transaction.
        /// </value>
        public ObservableDictionary<string, string> TransactionType
        {
            get { return _transactionType; }
            set
            {
                _transactionType = value;
                RaisePropertyChangedEvent("TransactionType");
            }
        }

        /// <summary>
        /// Gets or sets the un allocated groups collection.
        /// </summary>
        /// <value>
        /// The un allocated groups collection.
        /// </value>
        public BindingListCollectionView UnAllocatedGroupsCollection
        {
            get { return _unAllocatedGroupsCollection; }
            set
            {
                _unAllocatedGroupsCollection = value;
                RaisePropertyChangedEvent("UnAllocatedGroupsCollection");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable disable menus].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable disable menus]; otherwise, <c>false</c>.
        /// </value>
        public bool UnallocateMenuVisibility
        {
            get { return _unallocateMenuVisibility; }
            set
            {
                _unallocateMenuVisibility = value;
                RaisePropertyChangedEvent("UnallocateMenuVisibility");
            }
        }

        /// <summary>
        /// Gets or sets the venues.
        /// </summary>
        /// <value>
        /// The venues.
        /// </value>
        public ObservableDictionary<int, string> Venues
        {
            get { return _venues; }
            set
            {
                _venues = value;
                RaisePropertyChangedEvent("Venues");
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute caption1.
        /// </summary>
        /// <value>
        /// The trade attribute caption1.
        /// </value>
        public string TradeAttributeCaption1
        {
            get { return _tradeAttributeCaption1; }
            set
            {
                _tradeAttributeCaption1 = value;
                RaisePropertyChangedEvent("TradeAttributeCaption1");
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute caption2.
        /// </summary>
        /// <value>
        /// The trade attribute caption2.
        /// </value>
        public string TradeAttributeCaption2
        {
            get { return _tradeAttributeCaption2; }
            set
            {
                _tradeAttributeCaption2 = value;
                RaisePropertyChangedEvent("TradeAttributeCaption2");
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute caption3.
        /// </summary>
        /// <value>
        /// The trade attribute caption3.
        /// </value>
        public string TradeAttributeCaption3
        {
            get { return _tradeAttributeCaption3; }
            set
            {
                _tradeAttributeCaption3 = value;
                RaisePropertyChangedEvent("TradeAttributeCaption3");
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute caption4.
        /// </summary>
        /// <value>
        /// The trade attribute caption4.
        /// </value>
        public string TradeAttributeCaption4
        {
            get { return _tradeAttributeCaption4; }
            set
            {
                _tradeAttributeCaption4 = value;
                RaisePropertyChangedEvent("TradeAttributeCaption4");
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute caption5.
        /// </summary>
        /// <value>
        /// The trade attribute caption5.
        /// </value>
        public string TradeAttributeCaption5
        {
            get { return _tradeAttributeCaption5; }
            set
            {
                _tradeAttributeCaption5 = value;
                RaisePropertyChangedEvent("TradeAttributeCaption5");
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute caption6.
        /// </summary>
        /// <value>
        /// The trade attribute caption6.
        /// </value>
        public string TradeAttributeCaption6
        {
            get { return _tradeAttributeCaption6; }
            set
            {
                _tradeAttributeCaption6 = value;
                RaisePropertyChangedEvent("TradeAttributeCaption6");
            }
        }

        /// <summary>
        /// The trade attribute 7 to 45 captions dictionary
        /// </summary>
        public ObservableDictionary<int, string> AdditionalTradeAttributesCaptions
        {
            get { return _additionalTradeAttributesCaptions; }
            set
            {
                _additionalTradeAttributesCaptions = value;
                RaisePropertyChangedEvent("AdditionalTradeAttributesCaptions");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the allocation group clicked.
        /// </summary>
        /// <value>
        /// The allocation group clicked.
        /// </value>
        public RelayCommand<object> AllocationGroupClicked { get; set; }

        /// <summary>
        /// Gets or sets the context menu clicked.
        /// </summary>
        /// <value>
        /// The context menu clicked.
        /// </value>
        public RelayCommand<object> ContextMenuClicked { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationGridControlViewModel"/> class.
        /// </summary>
        public AllocationGridControlViewModel()
        {
            try
            {
                ContextMenuClicked = new RelayCommand<object>((parameter) => ContextMenuChanged(parameter));
                AllocationGroupClicked = new RelayCommand<object>((parameter) => OnAllocationGroupClicked(parameter));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Clears the selected groups.
        /// </summary>
        private void ClearSelectedGroups(BindingListCollectionView groupCollection)
        {
            try
            {
                if (groupCollection.SourceCollection != null)
                {
                    List<AllocationGroup> groupList = ((GenericBindingList<AllocationGroup>)groupCollection.SourceCollection).Where(x => x.IsSelected).ToList();
                    if (groupList.Count > 0)
                        groupList.ForEach(x => x.IsSelected = false);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Closes the trade.
        /// </summary>
        public void CloseTrade()
        {
            try
            {
                AllocationGroup group = null;
                TaxLot taxlot = null;
                if (ActiveAllocatedDataItem != null)
                {
                    if (ActiveAllocatedDataItem is AllocationGroup)
                    {
                        group = (AllocationGroup)ActiveAllocatedDataItem;
                        group.AccountID = 0;
                        group.StrategyID = 0;
                    }
                    else
                    {
                        if (ActiveAllocatedDataItem is TaxLot)
                        {
                            taxlot = (TaxLot)ActiveAllocatedDataItem;
                            group = ((GenericBindingList<AllocationGroup>)AllocatedGroupsCollection.SourceCollection).FirstOrDefault(x => x.GroupID == taxlot.GroupID);
                            group.AccountID = taxlot.Level1ID;
                            group.StrategyID = taxlot.Level2ID;
                        }
                    }

                    if (group != null && group.PersistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
                    {
                        if (group.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || group.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed) || group.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed))
                        {
                            if (LoadCloseTradeUI != null)
                            {
                                LoadCloseTradeUI(this, new EventArgs<AllocationGroup>(group));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select a valid trade.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                        MessageBox.Show("Please select a valid trade. This trade cannot be closed as there are unsaved changes", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                    MessageBox.Show("Please Select a trade.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
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
        /// Contexts the menu changed.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>        
        private object ContextMenuChanged(object parameter)
        {
            try
            {
                List<object> parameterValues = (List<object>)parameter;
                string selectedMenu = parameterValues[0].ToString();
                string gridName = string.Empty;
                List<AllocationGroup> allocationGroups = new List<AllocationGroup>();
                switch (selectedMenu)
                {
                    case AllocationClientConstants.MENU_UNALLOCATE:
                        allocationGroups = GetSelectedAllocatedGroups();
                        if (allocationGroups.Count > 0)
                        {
                            if (UnallocateGroupsEvent != null)
                                UnallocateGroupsEvent(this, new EventArgs<List<AllocationGroup>>(allocationGroups));
                        }
                        break;

                    case AllocationClientConstants.MENU_GROUP:
                        allocationGroups = GetSelectedUnAllocatedGroups();
                        if (allocationGroups.Count > 0)
                        {
                            if (GroupUngroupEvent != null)
                                GroupUngroupEvent(this, new EventArgs<List<AllocationGroup>, string>(allocationGroups, AllocationClientConstants.MENU_GROUP));
                        }
                        break;

                    case AllocationClientConstants.MENU_UNGROUP:
                        allocationGroups = GetSelectedUnAllocatedGroups();
                        if (allocationGroups.Count > 0)
                        {
                            if (GroupUngroupEvent != null)
                                GroupUngroupEvent(this, new EventArgs<List<AllocationGroup>, string>(allocationGroups, AllocationClientConstants.MENU_UNGROUP));
                        }
                        break;

                    case AllocationClientConstants.MENU_UNGROUP_ALLOCATED:
                        allocationGroups = GetSelectedAllocatedGroups();
                        if (allocationGroups.Count > 0)
                        {
                            if (GroupUngroupEvent != null)
                                GroupUngroupEvent(this, new EventArgs<List<AllocationGroup>, string>(allocationGroups, AllocationClientConstants.MENU_UNGROUP_ALLOCATED));
                        }
                        break;

                    case AllocationClientConstants.MENU_SAVE_LAYOUT:
                        gridName = parameterValues[1].ToString();
                        SaveGridLayout(gridName);
                        break;

                    case AllocationClientConstants.MENU_EXPAND_COLLAPSE:
                        gridName = parameterValues[1].ToString();
                        ExpandCollapseAll(gridName);
                        break;

                    case AllocationClientConstants.EXPORTDATA_UNALLOCATED:
                        ExportGridUnAllocatedGrid = true;
                        break;

                    case AllocationClientConstants.EXPORTDATA_ALLOCATED:
                        ExportGridAllocatedGrid = true;
                        break;

                    case AllocationClientConstants.EXPORTDATA_TAXLOTS:
                        ExportGridTaxlots = true;
                        break;

                    case AllocationClientConstants.EXPORTDATA_GROUPS:
                        ExportGridGroups = true;
                        break;

                    case AllocationClientConstants.MENU_SYMBOL_LOOKUP:
                        gridName = parameterValues[1].ToString();
                        LoadSymbolLookupUI(gridName);
                        break;

                    case AllocationClientConstants.MENU_AUDIT_TRAIL:
                        gridName = parameterValues[1].ToString();
                        LoadAuditUI(gridName);
                        break;

                    case AllocationClientConstants.MENU_CLOSE_ORDER:
                        CloseTrade();
                        break;

                    case AllocationClientConstants.MENU_GENERATE_CASH_TRANSACTION:
                        GenerateCashTransaction();
                        break;

                    case AllocationClientConstants.MENU_DELETE:
                        allocationGroups = GetSelectedUnAllocatedGroups();
                        DeleteGroups(allocationGroups);
                        break;

                    case AllocationClientConstants.CLEAR_FILTERS:
                        gridName = parameterValues[1].ToString();
                        ClearGridFilters(gridName);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Expands the collapse all.
        /// </summary>
        /// <param name="gridName">Name of the grid.</param>
        private void ExpandCollapseAll(string gridName)
        {
            try
            {
                if (gridName.Equals(AllocationClientConstants.UNALLOCATED_GRID))
                    IsExpandCollapseAll = true;
                else if (gridName.Equals(AllocationClientConstants.ALLOCATED_GRID))
                    IsExpandCollapseAllocatedGrid = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the grid filters.
        /// </summary>
        /// <param name="gridName">Name of the grid.</param>
        internal void ClearGridFilters(string gridName)
        {
            try
            {
                if (gridName.Equals(AllocationClientConstants.ALLOCATED_GRID))
                    IsFiltersClearedForAllocatedGrid = true;
                else if (gridName.Equals(AllocationClientConstants.UNALLOCATED_GRID))
                    IsFiltersClearedForUnAllocatedGrid = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the grid layout.
        /// </summary>
        /// <param name="gridName">Name of the grid.</param>
        private void SaveGridLayout(string gridName)
        {
            try
            {
                if (gridName.Equals(AllocationClientConstants.ALLOCATED_GRID))
                    IsSaveLayoutAllocatedGrid = true;
                else if (gridName.Equals(AllocationClientConstants.UNALLOCATED_GRID))
                    IsSaveLayoutUnallocatedGrid = true;

                if (SaveLayoutEvent != null)
                    SaveLayoutEvent(this, new EventArgs<bool>(true));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Deletes the groups.
        /// </summary>
        /// <param name="allocationGroups">The allocation groups.</param>
        private void DeleteGroups(List<AllocationGroup> allocationGroups)
        {
            try
            {
                if (allocationGroups != null && allocationGroups.Count > 0)
                {
                    MessageBoxResult userChoice = MessageBoxResult.Yes;
                    bool isAnyTradePartiallyExecuted = allocationGroups.Any(group => !group.IsManualGroup && group.NotAllExecuted);

                    if (isAnyTradePartiallyExecuted)
                        userChoice = MessageBox.Show("Some Fix Trades are partially  executed, Do you want to delete?", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (userChoice == MessageBoxResult.Yes)
                    {
                        // in case of deleting trades, we are setting active items to null so as to avoid other operations like preview data fro selected item as they are not required, PRANA-25315
                        ActiveAllocatedDataItem = null;
                        ActiveUnallocatedDataItem = null;
                        if (DeleteAllocationGroupEvent != null)
                            DeleteAllocationGroupEvent(this, new EventArgs<List<AllocationGroup>>(allocationGroups));
                    }
                }
                else
                {
                    MessageBox.Show("Please select an unallocated group.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Loads the symbol lookup UI.
        /// </summary>
        /// <param name="gridName">Name of the grid.</param>
        private void LoadSymbolLookupUI(string gridName)
        {
            try
            {
                if (gridName.ToString().Equals(AllocationClientConstants.UNALLOCATED_GRID) && ActiveUnallocatedDataItem != null)
                {
                    if (ActiveUnallocatedDataItem is AllocationGroup)
                    {
                        if (LoadSymbolLookUpUI != null)
                            LoadSymbolLookUpUI(this, new EventArgs<string>(((AllocationGroup)ActiveUnallocatedDataItem).Symbol));
                    }
                    else if (ActiveUnallocatedDataItem is AllocationOrder)
                    {
                        if (LoadSymbolLookUpUI != null)
                            LoadSymbolLookUpUI(this, new EventArgs<string>(((AllocationOrder)ActiveUnallocatedDataItem).Symbol));
                    }
                }
                else if (gridName.ToString().Equals(AllocationClientConstants.ALLOCATED_GRID) && ActiveAllocatedDataItem != null)
                {
                    if (ActiveAllocatedDataItem is AllocationGroup)
                    {
                        if (LoadSymbolLookUpUI != null)
                            LoadSymbolLookUpUI(this, new EventArgs<string>(((AllocationGroup)ActiveAllocatedDataItem).Symbol));
                    }
                    else if (ActiveAllocatedDataItem is TaxLot)
                    {
                        if (LoadSymbolLookUpUI != null)
                            LoadSymbolLookUpUI(this, new EventArgs<string>(((TaxLot)ActiveAllocatedDataItem).Symbol));
                    }
                }
                else
                    MessageBox.Show("Please Select a trade.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_counterParty != null)
                        _counterParty = null;
                    if (_thirdParty != null)
                        _thirdParty = null;
                    if (_venues != null)
                        _venues = null;
                    if (_sides != null)
                        _sides = null;
                    if (_transactionType != null)
                        _transactionType = null;
                    if (_fxConversionOperator != null)
                        _fxConversionOperator = null;
                    if (_settlConversionOperator != null)
                        _settlConversionOperator = null;
                    if (AllocatedGroupsCollection != null)
                    {
                        AllocatedGroupsCollection.DetachFromSourceCollection();
                        AllocatedGroupsCollection = null;
                    }
                    if (UnAllocatedGroupsCollection != null)
                    {
                        UnAllocatedGroupsCollection.DetachFromSourceCollection();
                        UnAllocatedGroupsCollection = null;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Generates the cash transaction.
        /// </summary>
        internal void GenerateCashTransaction()
        {
            try
            {
                if (ActiveAllocatedDataItem != null && ActiveAllocatedDataItem is TaxLot)
                {
                    TaxLot taxLot = (TaxLot)ActiveAllocatedDataItem;
                    if (LoadCashTransactionUI != null)
                    {
                        CashDataEventArgs taxlotToSend = new CashDataEventArgs(taxLot);
                        LoadCashTransactionUI(this, taxlotToSend);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Gets all unallocated groups.
        /// </summary>
        /// <returns></returns>
        internal List<AllocationGroup> GetAllUnallocatedGroups()
        {
            List<AllocationGroup> unallocatedGroupList = new List<AllocationGroup>();
            try
            {
                if (UnAllocatedGroupsCollection.SourceCollection != null)
                    unallocatedGroupList = ((GenericBindingList<AllocationGroup>)UnAllocatedGroupsCollection.SourceCollection).ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return unallocatedGroupList;
        }

        /// <summary>
        /// Gets the current active group.
        /// </summary>
        /// <returns></returns>
        internal AllocationGroup GetCurrentActiveGroup()
        {
            try
            {
                if (ActiveAllocatedDataItem != null && ActiveAllocatedDataItem is AllocationGroup)
                    return (ActiveAllocatedDataItem as AllocationGroup);
                if (ActiveUnallocatedDataItem != null && ActiveUnallocatedDataItem is AllocationGroup)
                    return (ActiveUnallocatedDataItem as AllocationGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the selected allocated groups.
        /// </summary>
        /// <returns></returns>
        internal List<AllocationGroup> GetSelectedAllocatedGroups()
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                GenericBindingList<AllocationGroup> allocatedGroupList = new GenericBindingList<AllocationGroup>();
                if (AllocatedGroupsCollection.SourceCollection != null)
                    allocatedGroupList = ((GenericBindingList<AllocationGroup>)AllocatedGroupsCollection.SourceCollection);
                groups = allocatedGroupList.Where(x => x.IsSelected).ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return groups;
        }

        /// <summary>
        /// Gets the selected group ids.
        /// </summary>
        /// <param name="SelectedAllocatedDataItems">The selected allocated data items.</param>
        /// <returns></returns>
        private List<string> GetSelectedGroupIds(IEnumerable SelectedAllocatedDataItems)
        {
            List<String> allocationGroups = new List<String>();
            try
            {
                foreach (AllocationGroup group in SelectedAllocatedDataItems)
                {
                    allocationGroups.Add(group.GroupID);
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
            return allocationGroups;
        }

        /// <summary>
        /// Gets the selected un allocated groups.
        /// </summary>
        /// <returns></returns>
        internal List<AllocationGroup> GetSelectedUnAllocatedGroups()
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                GenericBindingList<AllocationGroup> unallocatedGroupList = new GenericBindingList<AllocationGroup>();
                if (UnAllocatedGroupsCollection.SourceCollection != null)
                    unallocatedGroupList = ((GenericBindingList<AllocationGroup>)UnAllocatedGroupsCollection.SourceCollection);
                groups = unallocatedGroupList.Where(x => x.IsSelected).ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return groups;
        }

        /// <summary>
        /// Gets the unique selected symbols.
        /// </summary>
        /// <returns></returns>
        internal List<string> GetUniqueSelectedSymbols()
        {
            List<string> uniqueSymbols = new List<string>();
            try
            {
                foreach (AllocationGroup group in GetSelectedAllocatedGroups())
                {
                    string symbol = CommonHelper.GetSwapSymbol(group.Symbol, group.IsSwapped);
                    if (!uniqueSymbols.Contains(symbol))
                        uniqueSymbols.Add(symbol);
                }
                foreach (AllocationGroup group in GetSelectedUnAllocatedGroups())
                {
                    string symbol = CommonHelper.GetSwapSymbol(group.Symbol, group.IsSwapped);
                    if (!uniqueSymbols.Contains(symbol))
                        uniqueSymbols.Add(symbol);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return uniqueSymbols;
        }

        /// <summary>
        /// Loads the audit UI.
        /// </summary>
        /// <param name="gridName">The values.</param>
        private void LoadAuditUI(string gridName)
        {
            try
            {
                if (gridName.Equals(AllocationClientConstants.UNALLOCATED_GRID))
                {
                    List<AllocationGroup> groups = new List<AllocationGroup>();
                    groups = GetSelectedUnAllocatedGroups();
                    List<string> groupIds = GetSelectedGroupIds(groups);
                    if (groupIds.Count > 0)
                    {
                        LoadAuditTrailUI(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                    }
                    else
                    {
                        if (ActiveUnallocatedDataItem != null && ActiveUnallocatedDataItem is AllocationGroup)
                        {
                            groupIds.Add(((AllocationGroup)ActiveUnallocatedDataItem).GroupID);
                            LoadAuditTrailUI(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                        }
                        else if (ActiveUnallocatedDataItem != null && ActiveUnallocatedDataItem is AllocationOrder)
                        {
                            groupIds.Add(((AllocationOrder)ActiveUnallocatedDataItem).GroupID);
                            LoadAuditTrailUI(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                        }
                        else
                            MessageBox.Show("Please Select a trade.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else if (gridName.Equals(AllocationClientConstants.ALLOCATED_GRID))
                {
                    List<AllocationGroup> groups = GetSelectedAllocatedGroups();
                    List<string> groupIds = GetSelectedGroupIds(groups);
                    if (groupIds.Count > 0)
                    {
                        LoadAuditTrailUI(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                    }
                    else
                    {
                        if (ActiveAllocatedDataItem != null && ActiveAllocatedDataItem is AllocationGroup)
                        {
                            groupIds.Add(((AllocationGroup)ActiveAllocatedDataItem).GroupID);
                            LoadAuditTrailUI(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                        }
                        else if (ActiveAllocatedDataItem != null && ActiveAllocatedDataItem is TaxLot)
                        {
                            groupIds.Add(((TaxLot)ActiveAllocatedDataItem).GroupID);
                            LoadAuditTrailUI(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                        }
                        else
                            MessageBox.Show("Please Select a trade.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
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
        /// Called when [allocation group clicked].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnAllocationGroupClicked(object parameter)
        {
            try
            {
                if (parameter != null && parameter is AllocationGroup)
                {
                    if (((parameter as AllocationGroup).State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED) && ActiveAllocatedGroupEvent != null)
                        ActiveAllocatedGroupEvent(this, new EventArgs<AllocationGroup>((AllocationGroup)parameter));
                    else if (((parameter as AllocationGroup).State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED) && ActiveUnAllocatedGroupEvent != null)
                        ActiveUnAllocatedGroupEvent(this, new EventArgs<AllocationGroup>((AllocationGroup)parameter));
                }

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Loads the grid layouts.
        /// </summary>
        internal void OnLoadGrid(ObservableCollection<string>[] tradeAttributes, GenericBindingList<AllocationGroup> allocatedGroups, GenericBindingList<AllocationGroup> unallocatedGroups, CustomValueEnteredActions[] tradeAttributeKeepRecords)
        {
            try
            {
                if (!AllocationPermissions.EditTradeModulePermission)
                    DeleteMenuVisibility = Visibility.Collapsed;

                if (!AllocationPermissions.AllocationModulePermission)
                    UnallocateMenuVisibility = false;

                TradeAttributesCollection = tradeAttributes;
                TradeAttributesKeepRecords = tradeAttributeKeepRecords;
                List<EnumerationValue> conversionOperator = new List<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(Operator)));
                conversionOperator = new List<EnumerationValue>(conversionOperator.Where(x => x.DisplayText != Operator.Multiple.ToString()));

                Dictionary<int, string> brokerDict = new Dictionary<int, string>();
                brokerDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                brokerDict.AddRangeThreadSafely(CachedDataManager.GetInstance.GetUserCounterParties());

                Dictionary<int, string> thirdpartyDict = new Dictionary<int, string>();
                thirdpartyDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                thirdpartyDict.AddRangeThreadSafely(CachedDataManager.GetInstance.GetAllThirdPartiesWithShortName());

                Dictionary<int, string> venuesDict = new Dictionary<int, string>();
                venuesDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                venuesDict.AddRangeThreadSafely(CachedDataManager.GetInstance.GetAllVenues());

                Sides = new ObservableDictionary<string, string>(CommonAllocationMethods.GetOrderSides());
                CounterParty = new ObservableDictionary<int, string>(brokerDict);
                ThirdParty = new ObservableDictionary<int, string>(thirdpartyDict);
                Venues = new ObservableDictionary<int, string>(venuesDict);
                FxConversionOperator = new ObservableCollection<EnumerationValue>(conversionOperator);
                SettlConversionOperator = new ObservableCollection<EnumerationValue>(conversionOperator);
                TransactionType = new ObservableDictionary<string, string>(CachedDataManager.GetInstance.DictTransactionType);
                AllocatedGroupsCollection = new BindingListCollectionView(allocatedGroups);
                UnAllocatedGroupsCollection = new BindingListCollectionView(unallocatedGroups);

                SetTradeAttributesNames();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the preferences.
        /// </summary>
        /// <param name="allocationCompanyWisePref">The _allocation company wise preference.</param>
        internal void SetPreferences(Prana.BusinessObjects.Classes.Allocation.AllocationCompanyWisePref allocationCompanyWisePref)
        {
            try
            {
                AssetsWithCommissionInNetAmountList = allocationCompanyWisePref.AssetsWithCommissionInNetAmount;
                SetTradeAttributesNames();

                //set precision format
                PrecisionFormat = CommonAllocationMethods.SetPrecisionStringFormat(allocationCompanyWisePref.PrecisionDigit);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the trade attributes names.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void SetTradeAttributesNames()
        {
            try
            {
                TradeAttributeCaption1 = CachedDataManager.GetInstance.GetAttributeNameForValue(AllocationUIConstants.CAPTION_TradeAttribute1);
                TradeAttributeCaption2 = CachedDataManager.GetInstance.GetAttributeNameForValue(AllocationUIConstants.CAPTION_TradeAttribute2);
                TradeAttributeCaption3 = CachedDataManager.GetInstance.GetAttributeNameForValue(AllocationUIConstants.CAPTION_TradeAttribute3);
                TradeAttributeCaption4 = CachedDataManager.GetInstance.GetAttributeNameForValue(AllocationUIConstants.CAPTION_TradeAttribute4);
                TradeAttributeCaption5 = CachedDataManager.GetInstance.GetAttributeNameForValue(AllocationUIConstants.CAPTION_TradeAttribute5);
                TradeAttributeCaption6 = CachedDataManager.GetInstance.GetAttributeNameForValue(AllocationUIConstants.CAPTION_TradeAttribute6);
                Dictionary<int, string> tradeAttributesCaptionsDict = new Dictionary<int, string>();
                for (int i = 7; i <= 45; i++)
                {
                    tradeAttributesCaptionsDict.Add(i, CachedDataManager.GetInstance.GetAttributeNameForValue(AllocationUIConstants.CAPTION_TradeAttribute + i));
                }
                AdditionalTradeAttributesCaptions = new ObservableDictionary<int, string>(tradeAttributesCaptionsDict);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the selected groups allocated in accounts.
        /// </summary>
        /// <param name="accountIds">The account ids.</param>
        /// <returns></returns>
        internal List<AllocationGroup> GetSelectedGroupsForAccounts(List<int> accountIds)
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                List<AllocationGroup> allocatedGroups = GetSelectedAllocatedGroups();
                if (allocatedGroups.Count > 0 && accountIds != null && accountIds.Count > 0)
                {
                    allocatedGroups.ForEach(group =>
                    {
                        foreach (TaxLot taxlot in group.TaxLots)
                        {
                            if (accountIds.Contains(taxlot.Level1ID))
                            {
                                groups.Add(group);
                                break;
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return groups;
        }

        /// <summary>
        /// Gets the selected groups.
        /// </summary>
        /// <returns></returns>
        internal List<AllocationGroup> GetSelectedGroups()
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                groups.AddRange(GetSelectedUnAllocatedGroups());
                groups.AddRange(GetSelectedAllocatedGroups());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return groups;
        }

        #endregion Methods
    }
}
