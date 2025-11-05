using Infragistics.Controls.Editors.Primitives;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using Microsoft.Xaml.Behaviors;
using Prana.Admin.BLL;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Data;
using System.Windows.Input;

namespace Prana.Allocation.Client.Controls.Allocation.Behaviours
{
    public class UnallocatedGridBehavior : Behavior<XamDataGrid>
    {
        #region OnAttach Events
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            try
            {
                base.OnAttached();
                AssociatedObject.InitializeRecord += AssociatedObject_InitializeRecord;
                AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
                AssociatedObject.EditModeEnded += AssociatedObject_EditModeEnded;
                AssociatedObject.EditModeEnding += AssociatedObject_EditModeEnding;
                AssociatedObject.ContextMenuOpening += AssociatedObject_ContextMenuOpening;
                AssociatedObject.CellChanged += AssociatedObject_CellChanged;
                AssociatedObject.CellUpdated += AssociatedObject_CellUpdated;
                AssociatedObject.EditModeValidationError += AssociatedObject_EditModeValidationError;
                AssociatedObject.Loaded += AssociatedObject_Loaded;
                AssociatedObject.RecordFilterChanged += AssociatedObject_RecordFilterChanged;
                AssociatedObject.RecordActivated += AssociatedObject_RecordActivatedDeleted;
                AssociatedObject.RecordsDeleted += AssociatedObject_RecordActivatedDeleted;
                AssociatedObject.FieldChooserOpening += AssociatedObject_FieldChooserOpening;
                AssociatedObject.FieldLayoutInitialized += AssociatedObject_FieldLayoutInitialized;
                AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                try
                {
                    (AssociatedObject.SelectedDataItem as AllocationGroup).IsSelected = !(AssociatedObject.SelectedDataItem as AllocationGroup).IsSelected;
                    SelectedNoOfTradesUnAllocatedGrid = AssociatedObject.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(AssociatedObject.RecordManager.GetFilteredInDataRecords()).Count();
                }
                catch
                {
                }
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// The save layout unallocated grid
        /// </summary>
        public static readonly DependencyProperty SaveLayoutUnallocatedGrid = DependencyProperty.Register("IsSaveLayoutUnallocatedGrid", typeof(bool), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SaveUnallocatedGridLayout));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save layout unallocated grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save layout unallocated grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveLayoutUnallocatedGrid
        {
            get { return (bool)GetValue(SaveLayoutUnallocatedGrid); }
            set { SetValue(SaveLayoutUnallocatedGrid, value); }
        }

        /// <summary>
        /// The clear allocated selected items
        /// </summary>
        public static readonly DependencyProperty ClearAllocatedSelectedItems = DependencyProperty.Register("IsClearAllocatedSelectedItems", typeof(bool), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether this instance is clear allocated selected items.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is clear allocated selected items; otherwise, <c>false</c>.
        /// </value>
        public bool IsClearAllocatedSelectedItems
        {
            get { return (bool)GetValue(ClearAllocatedSelectedItems); }
            set { SetValue(ClearAllocatedSelectedItems, value); }
        }

        /// <summary>
        /// The expand collapse all
        /// </summary>
        public static readonly DependencyProperty ExpandCollapseAll = DependencyProperty.Register("IsExpandCollapseAll", typeof(bool), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ExpandCollapse));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expand collapse all.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expand collapse all; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpandCollapseAll
        {
            get { return (bool)GetValue(ExpandCollapseAll); }
            set { SetValue(ExpandCollapseAll, value); }
        }

        /// <summary>
        /// The load layout allocated grid
        /// </summary>
        public static readonly DependencyProperty AssetsWithCommissionInNetAmount = DependencyProperty.Register("AssetsWithCommissionInNetAmountList", typeof(List<int>), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether this instance is load layout allocated grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is load layout allocated grid; otherwise, <c>false</c>.
        /// </value>
        public List<int> AssetsWithCommissionInNetAmountList
        {
            get { return (List<int>)GetValue(AssetsWithCommissionInNetAmount); }
            set { SetValue(AssetsWithCommissionInNetAmount, value); }
        }

        /// <summary>
        /// The un group menu enabled
        /// </summary>
        public static readonly DependencyProperty UnGroupMenuEnabled = DependencyProperty.Register("IsUnGroupMenuEnabled", typeof(bool), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether this instance is un group menu enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is un group menu enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnGroupMenuEnabled
        {
            get { return (bool)GetValue(UnGroupMenuEnabled); }
            set { SetValue(UnGroupMenuEnabled, value); }
        }

        /// <summary>
        /// The group menu enabled
        /// </summary>
        public static readonly DependencyProperty GroupMenuEnabled = DependencyProperty.Register("IsGroupMenuEnabled", typeof(bool), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save layout unallocated grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save layout unallocated grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsGroupMenuEnabled
        {
            get { return (bool)GetValue(GroupMenuEnabled); }
            set { SetValue(GroupMenuEnabled, value); }
        }

        /// <summary>
        /// The un allocated security master enabled
        /// </summary>
        public static readonly DependencyProperty UnAllocatedSecurityMasterEnabled = DependencyProperty.Register("IsUnAllocatedSecurityMasterEnabled", typeof(bool), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether this instance is un allocated security master enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is un allocated security master enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnAllocatedSecurityMasterEnabled
        {
            get { return (bool)GetValue(UnAllocatedSecurityMasterEnabled); }
            set { SetValue(UnAllocatedSecurityMasterEnabled, value); }
        }

        /// <summary>
        /// The un allocated audit trail enabled
        /// </summary>
        public static readonly DependencyProperty UnAllocatedAuditTrailEnabled = DependencyProperty.Register("IsUnAllocatedAuditTrailEnabled", typeof(bool), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether this instance is un allocated security master enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is un allocated security master enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnAllocatedAuditTrailEnabled
        {
            get { return (bool)GetValue(UnAllocatedAuditTrailEnabled); }
            set { SetValue(UnAllocatedAuditTrailEnabled, value); }
        }


        /// <summary>
        /// The selected no of trades un allocated grid property
        /// </summary>
        public static readonly DependencyProperty SelectedNoOfTradesUnAllocatedGridProperty = DependencyProperty.Register("SelectedNoOfTradesUnAllocatedGrid", typeof(int), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets the selected no of trades un allocated grid.
        /// </summary>
        /// <value>
        /// The selected no of trades un allocated grid.
        /// </value>
        public int SelectedNoOfTradesUnAllocatedGrid
        {
            get { return (int)GetValue(SelectedNoOfTradesUnAllocatedGridProperty); }
            set { SetValue(SelectedNoOfTradesUnAllocatedGridProperty, value); }
        }

        /// <summary>
        /// The total no of trades un allocated grid property
        /// </summary>
        public static readonly DependencyProperty TotalNoOfTradesUnAllocatedGridProperty = DependencyProperty.Register("TotalNoOfTradesUnAllocatedGrid", typeof(int), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets the total no of trades un allocated grid.
        /// </summary>
        /// <value>
        /// The total no of trades un allocated grid.
        /// </value>
        public int TotalNoOfTradesUnAllocatedGrid
        {
            get { return (int)GetValue(TotalNoOfTradesUnAllocatedGridProperty); }
            set { SetValue(TotalNoOfTradesUnAllocatedGridProperty, value); }
        }

        /// <summary>
        /// The end edit mode and commit changes property
        /// </summary>
        public static readonly DependencyProperty EndEditModeAndCommitChangesProperty = DependencyProperty.Register("EndEditModeAndCommitChanges", typeof(bool), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EndEditMode));

        /// <summary>
        /// Gets or sets a value indicating whether [end edit mode and commit changes].
        /// </summary>
        /// <value>
        /// <c>true</c> if [end edit mode and commit changes]; otherwise, <c>false</c>.
        /// </value>
        public bool EndEditModeAndCommitChanges
        {
            get { return (bool)GetValue(EndEditModeAndCommitChangesProperty); }
            set { SetValue(EndEditModeAndCommitChangesProperty, value); }
        }

        /// <summary>
        /// The allow edit grid property
        /// </summary>
        public static readonly DependencyProperty AllowEditGridProperty = DependencyProperty.Register("AllowEditGrid", typeof(object), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsEditMode));
        /// <summary>
        /// Gets or sets the allow edit grid.
        /// </summary>
        /// <value>
        /// The allow edit grid.
        /// </value>
        public object AllowEditGrid
        {
            get { return GetValue(AllowEditGridProperty); }
            set { SetValue(AllowEditGridProperty, value); }
        }

        /// <summary>
        /// The is un allocated header checked property
        /// </summary>
        public static readonly DependencyProperty IsUnAllocatedHeaderCheckedProperty = DependencyProperty.Register("IsUnAllocatedHeaderChecked", typeof(bool), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, UnAllocatedHeaderCheckChanged));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is un allocated header checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is un allocated header checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnAllocatedHeaderChecked
        {
            get { return (bool)GetValue(IsUnAllocatedHeaderCheckedProperty); }
            set { SetValue(IsUnAllocatedHeaderCheckedProperty, value); }
        }

        /// <summary>
        /// The is refresh after get data property
        /// </summary>
        public static readonly DependencyProperty IsRefreshAfterGetDataProperty = DependencyProperty.Register("IsRefreshAfterGetData", typeof(bool), typeof(UnallocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RefreshAfterGetData));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is refresh after get data.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is refresh after get data; otherwise, <c>false</c>.
        /// </value>
        public bool IsRefreshAfterGetData
        {
            get { return (bool)GetValue(IsRefreshAfterGetDataProperty); }
            set { SetValue(IsRefreshAfterGetDataProperty, value); }
        }

        #endregion

        #region Variables

        /// <summary>
        /// TradeStringFields
        /// </summary>
        private List<string> TradeStringFields = new List<string>();

        /// <summary>
        /// The _default layout
        /// </summary>
        static bool _defaultLayout = false;

        /// <summary>
        /// Is Customization Completed
        /// </summary>
        bool _isCustomizationCompleted = false;

        private bool _IsBloombergEXCodeAvailable = false;
        private const string _const_BloombergEXCode= "BloombergSymbolWithExchangeCode";
        #endregion

        #region Method and Events

        /// <summary>
        /// Ends the edit mode.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void EndEditMode(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                    return;

                UnallocatedGridBehavior gridExtender = (UnallocatedGridBehavior)d;
                gridExtender.AssociatedObject.ExecuteCommand(DataPresenterCommands.EndEditModeAndCommitRecord);
                gridExtender.EndEditModeAndCommitChanges = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the CellUpdated event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellUpdatedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_CellUpdated(object sender, CellUpdatedEventArgs e)
        {
            try
            {
                if (e.Field.Name.Equals(AllocationUIConstants.IS_SELECTED))
                    SelectedNoOfTradesUnAllocatedGrid = AssociatedObject.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(AssociatedObject.RecordManager.GetFilteredInDataRecords()).Count();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the FieldChooserOpening event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FieldChooserOpeningEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_FieldChooserOpening(object sender, FieldChooserOpeningEventArgs e)
        {
            try
            {
                e.FieldChooser.FieldGroupSelectorVisibility = Visibility.Collapsed;
                e.ToolWindow.Title = "UnAllocated Grid Field Chooser";
                e.ToolWindow.Width = 260;
                e.ToolWindow.Height = 300;
                AutomationProperties.SetAutomationId(e.ToolWindow, "UnAllocatedGridFieldChooser");
                AutomationProperties.SetName(e.ToolWindow, "UnAllocated Grid Field Chooser");
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the RecordActivated event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RecordActivatedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_RecordActivatedDeleted(object sender, RoutedEventArgs e)
        {
            try
            {
                //Updating Trade Counter
                TotalNoOfTradesUnAllocatedGrid = AssociatedObject.RecordManager.GetFilteredInDataRecords().Count();
                SelectedNoOfTradesUnAllocatedGrid = AssociatedObject.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(AssociatedObject.RecordManager.GetFilteredInDataRecords()).Count();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the Loaded event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadLayout();
                XamDataGrid dataGrid = (sender as XamDataGrid);

                //Set label with split camel case
                foreach (Field field in dataGrid.FieldLayouts[0].Fields)
                    field.Label = CommonAllocationMethods.SplitCamelCase(field.Label.ToString());

                //Column Customization For Group
                if (dataGrid.FieldLayouts.Any(x => x.Description.ToString().Equals(AllocationUIConstants.ALLOCATION_GROUP_FIELD_LAYOUT_NAME)))
                {
                    FieldLayout allocationGroupFieldLayout = dataGrid.FieldLayouts.FirstOrDefault(x => x.Description.ToString().Equals(AllocationUIConstants.ALLOCATION_GROUP_FIELD_LAYOUT_NAME));

                    if (!_IsBloombergEXCodeAvailable)
                        allocationGroupFieldLayout.Fields[_const_BloombergEXCode].Visibility = Visibility.Collapsed;

                    GridCustomizationHelper.ColumnsCustomizationForGroup(allocationGroupFieldLayout, _defaultLayout, dataGrid.Name, dataGrid.Resources);
                }

                //Set for grid Editable or non editable based on permission
                if (!AllocationPermissions.EditTradeModulePermission)
                    dataGrid.FieldSettings.AllowEdit = false;

                dataGrid.FieldChooserOpening += (ss, ee) =>
                {
                    dataGrid.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(ee.ToolWindow.Parent as Window);
                    }), System.Windows.Threading.DispatcherPriority.Background, null);
                };
                AddStringColumns();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the RecordFilterChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RecordFilterChangedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_RecordFilterChanged(object sender, RecordFilterChangedEventArgs e)
        {
            try
            {
                TotalNoOfTradesUnAllocatedGrid = AssociatedObject.RecordManager.GetFilteredInDataRecords().Count();
                SelectedNoOfTradesUnAllocatedGrid = AssociatedObject.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(AssociatedObject.RecordManager.GetFilteredInDataRecords()).Count();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the EditModeValidationError event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EditModeValidationErrorEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_EditModeValidationError(object sender, EditModeValidationErrorEventArgs e)
        {
            try
            {
                e.Editor.Text = "None";
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Adds the string columns.
        /// </summary>
        private void AddStringColumns()
        {
            try
            {
                TradeStringFields.Add(AllocationUIConstants.LOT_ID);
                TradeStringFields.Add(AllocationUIConstants.EXTERNAL_TRANS_ID);
                TradeStringFields.Add(AllocationUIConstants.TradeAttribute1);
                TradeStringFields.Add(AllocationUIConstants.TradeAttribute2);
                TradeStringFields.Add(AllocationUIConstants.TradeAttribute3);
                TradeStringFields.Add(AllocationUIConstants.TradeAttribute4);
                TradeStringFields.Add(AllocationUIConstants.TradeAttribute5);
                TradeStringFields.Add(AllocationUIConstants.TradeAttribute6);
                TradeStringFields.Add(AllocationUIConstants.CAPTION_DESCRIPTION);
                TradeStringFields.Add(AllocationUIConstants.INTERNAL_COMMENTS);
                TradeStringFields.Add(AllocationUIConstants.CHANGE_COMMENT);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Handles the CellChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellChangedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_CellChanged(object sender, CellChangedEventArgs e)
        {
            try
            {
                //Update Settlement Currency
                if (e.Cell.Field.Name.Equals(AllocationUIConstants.SETTLEMENT_CURRENCY))
                    ((AllocationGroup)e.Cell.Record.DataItem).SettlementCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(e.Editor.Text);

                AllocationGroup allocationGroup = (AllocationGroup)e.Cell.Record.DataItem;
                string errorMessage = string.Empty;
                bool result = EditTradeValidation.ValidationOnField(e.Cell.Field.Name, allocationGroup, PostTradeEnums.Status.None, ref errorMessage);

                if (!result)
                {
                    AssociatedObject.CellChanged -= AssociatedObject_CellChanged;
                    e.Cell.Record.CancelUpdate();
                    AssociatedObject.CellChanged += AssociatedObject_CellChanged;
                    MessageBoxImage img = errorMessage.Equals(AllocationUIConstants.MSG_AUTO_CALCULATE_FIELD) ? MessageBoxImage.Information : MessageBoxImage.Warning;
                    MessageBox.Show(errorMessage, AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, img);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the CellUpdated event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellUpdatedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_EditModeEnded(object sender, EditModeEndedEventArgs e)
        {
            try
            {
                if (e.ChangesAccepted)
                {
                    // Update default value if value is null
                    if (e.Cell.Value == null || e.Cell.Value.Equals(string.Empty))
                        e.Cell.Value = CommonAllocationMethods.GetDefaultValueForDataType(e.Cell.Field.DataType);

                    // Update dependednt fields
                    AllocationGroup group = (AllocationGroup)e.Cell.Record.DataItem;
                    group.IsRecalculateCommission = false;
                    group.IsModified = true;

                    // Update audit trail and depenedent fields
                    EditTradeHelper.UpdateGroupDates(e.Cell.Field.Name, group);
                    EditTradeHelper.UpdateCommissionAndFees(e.Cell.Field.Name, group, null, e.Cell.Value);
                    EditTradeHelper.UpdateGroupFields(e.Cell.Field.Name, e.Cell.Value, group);
                    EditTradeHelper.UpdateSecurityFields(e.Cell.Field.Name, group);
                    EditTradeHelper.UpdateTradeAttributes(e.Cell.Field.Name, group, null);

                    // Update fields for fixed Income and convertible bonds
                    if (e.Cell.Record.Cells[AllocationUIConstants.ASSET_NAME].Value.ToString().Equals(AssetCategory.FixedIncome.ToString()) || e.Cell.Record.Cells[AllocationUIConstants.ASSET_NAME].Value.ToString().Equals(AssetCategory.ConvertibleBond.ToString()))
                        EditTradeHelper.UpdateFieldsForFixedIncomeAndConvertibleBond(e.Cell.Field.Name, group);

                    // Update taxlot and order level
                    EditTradeHelper.UpdateTaxlotandOrders(e.Cell.Field.Name, e.Cell.Value.ToString(), group);

                    // Raise property changed event
                    group.PropertyHasChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the InitializeRecord event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Windows.DataPresenter.Events.InitializeRecordEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_InitializeRecord(object sender, InitializeRecordEventArgs e)
        {
            try
            {
                AssociatedObject.CellUpdated -= AssociatedObject_CellUpdated;
                if (e.Record is DataRecord)
                {
                    DataRecord dr = (DataRecord)e.Record;

                    if (dr.DataPresenter.Name.Equals(AllocationClientConstants.CONST_GIRD_UNALLOCATED) && dr.DataItem is AllocationGroup)
                    {
                        if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.SETTLEMENT_CURRENCY) != -1 && dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.SETTLEMENT_CURRENCYID) != -1)
                            dr.Cells[AllocationUIConstants.SETTLEMENT_CURRENCY].Value = FieldCalculator.GetSettlementCurrency(Convert.ToInt32(dr.Cells[AllocationUIConstants.SETTLEMENT_CURRENCYID].Value));
                        if (dr.Cells[AllocationUIConstants.BorrowerBroker].Value != null)
                        {
                            if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.BorrowerBroker) != -1 && dr.FieldLayout.Fields.IndexOf(OrderFields.PROPERTY_BORROWERBROKER) != -1 && dr.Cells[AllocationUIConstants.BorrowerBroker].Value.Equals("Undefined"))
                                dr.Cells[AllocationUIConstants.BorrowerBroker].Value = ApplicationConstants.C_COMBO_SELECT;
                        }
                        if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.COUNTERPARTY_NAME) != -1 && dr.FieldLayout.Fields.IndexOf(OrderFields.PROPERTY_COUNTERPARTY_NAME) != -1 && (dr.Cells[AllocationUIConstants.COUNTERPARTY_NAME].Value.Equals("Undefined") || (int)dr.Cells[AllocationUIConstants.COUNTERPARTY_ID].Value == 0))
                            dr.Cells[AllocationUIConstants.COUNTERPARTY_NAME].Value = ApplicationConstants.C_COMBO_SELECT;

                        if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.VENUE) != -1 && dr.FieldLayout.Fields.IndexOf(OrderFields.PROPERTY_VENUE) != -1 && (int)dr.Cells[AllocationUIConstants.VENUE_ID].Value == 0)
                            dr.Cells[AllocationUIConstants.VENUE].Value = ApplicationConstants.C_COMBO_SELECT;

                        if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.IMPORT_FILE_NAME) != -1)
                            dr.Cells[AllocationUIConstants.IMPORT_FILE_NAME].Value = FieldCalculator.GetImportFileName(((AllocationGroup)dr.DataItem).Orders);

                        if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ASSET_CATEGORY) != -1 && dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.IS_SWAPPED) != -1 && dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ASSET_ID) != -1 && dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ASSET_NAME) != -1)
                            dr.Cells[AllocationUIConstants.ASSET_CATEGORY].Value = FieldCalculator.GetAssetCategory(Convert.ToBoolean(dr.Cells[AllocationUIConstants.IS_SWAPPED].Value), Convert.ToInt32(dr.Cells[AllocationUIConstants.ASSET_ID].Value), dr.Cells[AllocationUIConstants.ASSET_NAME].Value.ToString());

                        if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ASSET_CATEGORY) != -1 && dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ACCRUED_INTEREST) != -1)
                        {
                            string assetCategory = (string)dr.Cells[AllocationUIConstants.ASSET_CATEGORY].Value;
                            if (!(assetCategory == "FixedIncome" || assetCategory == "ConvertibleBond"))
                                dr.Cells[AllocationUIConstants.ACCRUED_INTEREST].Field.AllowEdit = false;
                        }
                        //disable fields if base currency and local currencies are same
                        bool _groupCellState = !CachedDataManager.GetInstance.GetCompanyBaseCurrencyID().Equals(((AllocationGroup)dr.DataItem).CurrencyID);
                        if (!_groupCellState)
                        {
                            dr.Cells[AllocationUIConstants.FX_CONVERSION_METHOD_OPERATOR].Field.AllowEdit = _groupCellState;
                            dr.Cells[AllocationUIConstants.FXRATE].Field.AllowEdit = _groupCellState;
                        }

                        if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.MASTER_FUND_NAME) != -1)
                            dr.Cells[AllocationUIConstants.MASTER_FUND_NAME].Value = FieldCalculator.GetMasterFundName((AllocationGroup)dr.DataItem);

                    }
                    else if (dr.DataItem is TaxLot)
                    {
                        //disable fields if base currency and local currencies are same
                        bool _taxlotCellState = !CachedDataManager.GetInstance.GetCompanyBaseCurrencyID().Equals(((TaxLot)dr.DataItem).CurrencyID);
                        if (!_taxlotCellState)
                        {
                            dr.Cells[AllocationUIConstants.FX_CONVERSION_METHOD_OPERATOR].Field.AllowEdit = _taxlotCellState;
                            dr.Cells[AllocationUIConstants.FXRATE].Field.AllowEdit = _taxlotCellState;
                        }
                    }
                }
                AssociatedObject.CellUpdated += AssociatedObject_CellUpdated;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the EditModeEnding event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EditModeEndingEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_EditModeEnding(object sender, EditModeEndingEventArgs e)
        {
            try
            {
                if (e.Cell.Field.Name.StartsWith("AUECLocalDate"))
                {
                    AllocationGroup allocationGroup = (AllocationGroup)e.Cell.Record.DataItem;
                    if (allocationGroup.PersistenceStatus == ApplicationConstants.PersistenceStatus.ReAllocated)
                    {
                        MessageBox.Show("Please Save Status of Trade Before Changing Trade Date.", "Warning", System.Windows.MessageBoxButton.OK, MessageBoxImage.Warning);
                        e.AcceptChanges = false;
                        return;
                    }
                }

                //If Execution Qty more than Qty then no need to accept change. And cancel update
                if (e.Cell.Field.Name.StartsWith(AllocationUIConstants.CUMQTY))
                {
                    int cumQty = 0;
                    AllocationGroup allocationGroup = (AllocationGroup)e.Cell.Record.DataItem;
                    Int32.TryParse(e.Editor.Text, out cumQty);
                    if (cumQty > allocationGroup.Quantity)
                    {
                        MessageBox.Show("Executed Quantity should be less than or equal to the Quantity!", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                        e.AcceptChanges = false;
                        return;
                    }
                }

                if (e.Cell.Field.Name.StartsWith("TradeAttribute"))
                {
                    SpecializedTextBox textBox = Infragistics.Windows.Utilities.GetDescendantFromType(sender as DependencyObject, typeof(SpecializedTextBox), false) as SpecializedTextBox;
                    if (textBox != null)
                        e.Editor.Value = textBox.Text;
                }

                //To check If cell Data type is DateTime and value is less than the minimum value. Or invalid date then display a message
                if (e.Cell.Field.DataType == typeof(DateTime))
                {
                    DateTime dateValue;
                    bool result = DateTime.TryParse(e.Editor.Text, out dateValue);

                    if (result && dateValue.Date < DateTimeConstants.MinValue.Date)
                    {
                        MessageBox.Show("Entered Date cannot be less than 1/1/1800, it will be reverted to current Date", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Editor.Text = DateTime.Now.ToString();
                    }
                    else if (!result)
                    {
                        MessageBox.Show("You have entered incorrect date, it will be reverted to current Date", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Editor.Text = DateTime.Now.ToString();
                    }
                }

                //If original value and new value are same then no need to accept change. And cancel update
                if (e.Editor.OriginalValue != null && e.Editor.OriginalValue.Equals(e.Editor.Value))
                    e.AcceptChanges = false;
                else
                {
                    AllocationGroup allocationGroup = (AllocationGroup)e.Cell.Record.DataItem;
                    bool result = true;

                    if (e.Cell.Field.DataType == typeof(DateTime))
                        result = EditTradeValidation.ValidationOnDatesField(e.Cell.Field.Name, e.Editor.Text, allocationGroup);

                    if (!result)
                        e.Editor.Text = e.Cell.Value.ToString();
                    else
                    {
                        AllocationClientManager.GetInstance().DictUnsavedAdd(allocationGroup.GroupID, (AllocationGroup)allocationGroup.Clone());
                        allocationGroup.UpdateGroupPersistenceStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Saves the unallocated grid layout.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SaveUnallocatedGridLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                    return;

                UnallocatedGridBehavior gridExtender = (UnallocatedGridBehavior)d;
                File.WriteAllText(CommonAllocationMethods.GetDirectoryPath() + @"\" + CommonAllocationMethods.GetFileName(gridExtender.AssociatedObject.Name) + ".xml", AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID);
                FileStream fs = new FileStream(CommonAllocationMethods.GetDirectoryPath() + @"\" + CommonAllocationMethods.GetFileName(gridExtender.AssociatedObject.Name) + ".xml", FileMode.Append, FileAccess.Write, FileShare.None);
                gridExtender.AssociatedObject.SaveCustomizations(fs);
                fs.Close();
                gridExtender.IsSaveLayoutUnallocatedGrid = false;
                _defaultLayout = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Loads the layout.
        /// </summary>
        private void LoadLayout()
        {
            try
            {
                if (AssociatedObject != null)
                {
                    FileStream fs = null;
                    if (Directory.Exists(CommonAllocationMethods.GetDirectoryPath()))
                    {
                        string path = CommonAllocationMethods.GetDirectoryPath() + @"\" + CommonAllocationMethods.GetFileName(AssociatedObject.Name) + ".xml";
                        if (File.Exists(path))
                        {
                            fs = new FileStream(CommonAllocationMethods.GetDirectoryPath() + @"\" + CommonAllocationMethods.GetFileName(AssociatedObject.Name) + ".xml", FileMode.Open, FileAccess.Read, FileShare.Read);
                            _defaultLayout = false;
                        }
                        else
                            _defaultLayout = true;
                        if (fs != null)
                        {
                            string xmlString = File.ReadAllText(path);
                            if (xmlString.Contains("Version"))
                            {
                                if (xmlString.Contains(_const_BloombergEXCode))
                                {
                                    _IsBloombergEXCodeAvailable = true;
                                }
                                string layoutVersion = xmlString.Substring(xmlString.IndexOf("<Version>"), xmlString.LastIndexOf("</Version>") + "</Version>".Length);
                                fs.Position = (long)(layoutVersion.Length);
                            }
                            else
                                fs.Position = 0;
                            AssociatedObject.LoadCustomizations(fs);
                        }
                    }
                    else
                        _defaultLayout = true;
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
        /// Expands the collapse.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ExpandCollapse(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                    return;

                UnallocatedGridBehavior gridExtender = (UnallocatedGridBehavior)d;
                Record record = gridExtender.AssociatedObject.ActiveRecord;
                if (record == null && gridExtender.AssociatedObject.Records.Count > 0)
                    record = gridExtender.AssociatedObject.Records[0];
                if (record != null && record.ParentRecord != null)
                    record = gridExtender.AssociatedObject.ActiveRecord.ParentRecord;
                if (record != null)
                {
                    if (record.IsExpanded)
                        gridExtender.AssociatedObject.Records.CollapseAll(true);
                    else
                        gridExtender.AssociatedObject.Records.ExpandAll(true);
                }
                gridExtender.IsExpandCollapseAll = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the ContextMenuOpening event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.ContextMenuEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            try
            {
                bool isAllSwappedGroups = !AssociatedObject.Records.Any(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected && (((x as DataRecord).DataItem) as AllocationGroup).IsSwapped == false);
                if (isAllSwappedGroups)
                {
                    IsUnGroupMenuEnabled = false;
                    IsGroupMenuEnabled = false;
                }
                else
                {
                    IsUnGroupMenuEnabled = AssociatedObject.Records.Any(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected);
                    IsGroupMenuEnabled = AssociatedObject.Records.Count(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected) > 1;
                }
                IsUnAllocatedSecurityMasterEnabled = ModuleManager.CheckModulePermissioning(PranaModules.SECURITY_MASTER_MODULE, PranaModules.SECURITY_MASTER_MODULE);
                IsUnAllocatedAuditTrailEnabled = ModuleManager.CheckModulePermissioning(PranaModules.AUDIT_TRAIL_MODULE, PranaModules.AUDIT_TRAIL_MODULE);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Determines whether [is edit mode] [the specified d].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void IsEditMode(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                UnallocatedGridBehavior gridExtender = (UnallocatedGridBehavior)d;
                XamDataGrid dataGrid = gridExtender.AssociatedObject;
                if (dataGrid != null)
                    dataGrid.FieldSettings.AllowEdit = (bool)gridExtender.AllowEditGrid;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Uns the allocated header check changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void UnAllocatedHeaderCheckChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                UnallocatedGridBehavior gridExtender = (UnallocatedGridBehavior)d;
                gridExtender.AssociatedObject.CellUpdated -= gridExtender.AssociatedObject_CellUpdated;
                gridExtender.AssociatedObject.InitializeRecord -= gridExtender.AssociatedObject_InitializeRecord;
                if (gridExtender.IsUnAllocatedHeaderChecked)
                {
                    gridExtender.AssociatedObject.RecordManager.GetFilteredInDataRecords().ToList().ForEach(record =>
                        {
                            ((AllocationGroup)record.DataItem).IsSelected = true;
                        }
                    );
                }
                else
                {
                    gridExtender.AssociatedObject.RecordManager.GetFilteredInDataRecords().ToList().ForEach(record =>
                        {
                            ((AllocationGroup)record.DataItem).IsSelected = false;
                        }
                    );
                }
                gridExtender.SelectedNoOfTradesUnAllocatedGrid = gridExtender.AssociatedObject.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(gridExtender.AssociatedObject.RecordManager.GetFilteredInDataRecords()).Count();
                ((UnallocatedGridBehavior)d).AssociatedObject.InitializeRecord += ((UnallocatedGridBehavior)d).AssociatedObject_InitializeRecord;
                gridExtender.AssociatedObject.CellUpdated += gridExtender.AssociatedObject_CellUpdated;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Refreshes the after get data.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void RefreshAfterGetData(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                    return;

                UnallocatedGridBehavior gridExtender = (UnallocatedGridBehavior)d;
                XamDataGrid dataGrid = gridExtender.AssociatedObject;
                gridExtender.TotalNoOfTradesUnAllocatedGrid = dataGrid.RecordManager.GetFilteredInDataRecords().Count();
                gridExtender.SelectedNoOfTradesUnAllocatedGrid = dataGrid.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(dataGrid.RecordManager.GetFilteredInDataRecords()).Count();
                gridExtender.IsRefreshAfterGetData = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// AssociatedObject FieldLayoutInitialized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_FieldLayoutInitialized(object sender, FieldLayoutInitializedEventArgs e)
        {
            try
            {
                if (!_isCustomizationCompleted)
                {
                    XamDataGrid dataGrid = (sender as XamDataGrid);
                    GridCustomizationHelper.AddUnboundColumns(dataGrid);
                    //To Load layout of Order and customization of Order fields
                    if (dataGrid.Records.Count > 0)
                    {
                        if (dataGrid.FieldLayouts.Any(x => x.Description.ToString().Equals(AllocationUIConstants.ALLOCATION_ORDER_FIELD_LAYOUT_NAME)))
                        {
                            FieldLayout orderFieldLayout = dataGrid.FieldLayouts.FirstOrDefault(x => x.Description.ToString().Equals(AllocationUIConstants.ALLOCATION_ORDER_FIELD_LAYOUT_NAME));
                            GridCustomizationHelper.ColumnsCustomizationForOrders(orderFieldLayout, dataGrid.Resources);
                            _isCustomizationCompleted = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                XamDataGrid dataGrid = (sender as XamDataGrid);
                if (dataGrid != null && dataGrid.ActiveCell != null && dataGrid.ActiveCell.IsInEditMode)
                    EndEditModeAndCommitChanges = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }

        }

        #endregion

        #region OnDetach Events
        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            try
            {
                AssociatedObject.InitializeRecord -= AssociatedObject_InitializeRecord;
                AssociatedObject.EditModeEnded -= AssociatedObject_EditModeEnded;
                AssociatedObject.EditModeEnding -= AssociatedObject_EditModeEnding;
                AssociatedObject.ContextMenuOpening -= AssociatedObject_ContextMenuOpening;
                AssociatedObject.RecordFilterChanged -= AssociatedObject_RecordFilterChanged;
                AssociatedObject.Loaded -= AssociatedObject_Loaded;
                AssociatedObject.RecordsDeleted -= AssociatedObject_RecordActivatedDeleted;
                AssociatedObject.RecordActivated -= AssociatedObject_RecordActivatedDeleted;
                AssociatedObject.FieldChooserOpening -= AssociatedObject_FieldChooserOpening;
                AssociatedObject.CellUpdated -= AssociatedObject_CellUpdated;
                AssociatedObject.FieldLayoutInitialized -= AssociatedObject_FieldLayoutInitialized;
                AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
                base.OnDetaching();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion
    }
}
