using Infragistics.Controls.Editors.Primitives;
using Infragistics.Windows.Controls;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using Infragistics.Windows.Editors;
using Microsoft.Xaml.Behaviors;
using Prana.Admin.BLL;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Prana.Allocation.Client.Controls.Allocation.Behaviours
{
    public class AllocatedGridBehavior : Behavior<XamDataGrid>
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
                AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
                AssociatedObject.InitializeRecord += AssociatedObject_InitializeRecord;
                AssociatedObject.EditModeEnded += AssociatedObject_EditModeEnded;
                AssociatedObject.EditModeEnding += AssociatedObject_EditModeEnding;
                AssociatedObject.CellChanged += AssociatedObject_CellChanged;
                AssociatedObject.CellUpdated += AssociatedObject_CellUpdated;
                AssociatedObject.ContextMenuOpening += AssociatedObject_ContextMenuOpening;
                AssociatedObject.Loaded += AssociatedObject_Loaded;
                AssociatedObject.EditModeValidationError += AssociatedObject_EditModeValidationError;
                AssociatedObject.RecordFilterChanged += AssociatedObject_RecordFilterChanged;
                AssociatedObject.RecordActivated += AssociatedObject_RecordActivatedDeleted;
                AssociatedObject.RecordsDeleted += AssociatedObject_RecordActivatedDeleted;
                AssociatedObject.FieldChooserOpening += AssociatedObject_FieldChooserOpening;
                AssociatedObject.FieldLayoutInitialized += AssociatedObject_FieldLayoutInitialized;
                AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                try
                {
                    (AssociatedObject.SelectedDataItem as AllocationGroup).IsSelected = !(AssociatedObject.SelectedDataItem as AllocationGroup).IsSelected;
                    SelectedNoOfTradesAllocatedGrid = AssociatedObject.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(AssociatedObject.RecordManager.GetFilteredInDataRecords()).Count();
                }
                catch
                {
                }
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// The save layout allocated grid
        /// </summary>
        public static readonly DependencyProperty SaveLayoutAllocatedGrid = DependencyProperty.Register("IsSaveLayoutAllocatedGrid", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SaveAllocatedGridLayout));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save layout allocated grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save layout allocated grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveLayoutAllocatedGrid
        {
            get { return (bool)GetValue(SaveLayoutAllocatedGrid); }
            set { SetValue(SaveLayoutAllocatedGrid, value); }
        }

        /// <summary>
        /// The clear un allocated selected items
        /// </summary>
        public static readonly DependencyProperty ClearUnAllocatedSelectedItems = DependencyProperty.Register("IsClearUnAllocatedSelectedItems", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });
        /// <summary>
        /// Gets or sets a value indicating whether this instance is clear allocated selected items.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is clear allocated selected items; otherwise, <c>false</c>.
        /// </value>
        public bool IsClearUnAllocatedSelectedItems
        {
            get { return (bool)GetValue(ClearUnAllocatedSelectedItems); }
            set { SetValue(ClearUnAllocatedSelectedItems, value); }
        }

        /// <summary>
        /// The expand collapse all
        /// </summary>
        public static readonly DependencyProperty ExpandCollapseAll = DependencyProperty.Register("IsExpandCollapseAllocatedGrid", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ExpandCollapse));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expand collapse all.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expand collapse all; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpandCollapseAllocatedGrid
        {
            get { return (bool)GetValue(ExpandCollapseAll); }
            set { SetValue(ExpandCollapseAll, value); }
        }

        /// <summary>
        /// The load layout allocated grid
        /// </summary>
        public static readonly DependencyProperty AssetsWithCommissionInNetAmount = DependencyProperty.Register("AssetsWithCommissionInNetAmountList", typeof(List<int>), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

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
        /// The end edit mode and commit changes property
        /// </summary>
        public static readonly DependencyProperty EndEditModeAndCommitChangesProperty = DependencyProperty.Register("EndEditModeAndCommitChanges", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EndEditMode));

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
        public static readonly DependencyProperty AllowEditGridProperty = DependencyProperty.Register("AllowEditGrid", typeof(object), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsEditMode));

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
        /// The is enable disable menus
        /// </summary>
        public static readonly DependencyProperty UnallocateMenuVisibilityProperty = DependencyProperty.Register("UnallocateMenuVisibility", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether [enable disable menus].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable disable menus]; otherwise, <c>false</c>.
        /// </value>
        public bool UnallocateMenuVisibility
        {
            get { return (bool)GetValue(UnallocateMenuVisibilityProperty); }
            set { SetValue(UnallocateMenuVisibilityProperty, value); }
        }

        /// <summary>
        /// The un group menu enabled for Allocated grid
        /// </summary>
        public static readonly DependencyProperty IsAllocatedUnGroupMenuEnabledProperty = DependencyProperty.Register("IsAllocatedUnGroupMenuEnabled", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether this instance is un group menu enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is un group menu enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocatedUnGroupMenuEnabled
        {
            get { return (bool)GetValue(IsAllocatedUnGroupMenuEnabledProperty); }
            set { SetValue(IsAllocatedUnGroupMenuEnabledProperty, value); }
        }

        /// <summary>
        /// The allocated security master enabled
        /// </summary>
        public static readonly DependencyProperty AllocatedSecurityMasterEnabled = DependencyProperty.Register("IsAllocatedSecurityMasterEnabled", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocated security master enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocated security master enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocatedSecurityMasterEnabled
        {
            get { return (bool)GetValue(AllocatedSecurityMasterEnabled); }
            set { SetValue(AllocatedSecurityMasterEnabled, value); }
        }

        /// <summary>
        /// The allocated audit trail enabled
        /// </summary>
        public static readonly DependencyProperty AllocatedAuditTrailEnabled = DependencyProperty.Register("IsAllocatedAuditTrailEnabled", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocated audit trail enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocated audit trail enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocatedAuditTrailEnabled
        {
            get { return (bool)GetValue(AllocatedAuditTrailEnabled); }
            set { SetValue(AllocatedAuditTrailEnabled, value); }
        }

        /// <summary>
        /// The allocated close order enabled
        /// </summary>
        public static readonly DependencyProperty AllocatedCloseOrderEnabled = DependencyProperty.Register("IsAllocatedCloseOrderEnabled", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocated close order enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocated close order enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocatedCloseOrderEnabled
        {
            get { return (bool)GetValue(AllocatedCloseOrderEnabled); }
            set { SetValue(AllocatedCloseOrderEnabled, value); }
        }

        /// <summary>
        /// The selected no of trades allocated grid property
        /// </summary>
        public static readonly DependencyProperty SelectedNoOfTradesAllocatedGridProperty = DependencyProperty.Register("SelectedNoOfTradesAllocatedGrid", typeof(int), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets the selected no of trades allocated grid.
        /// </summary>
        /// <value>
        /// The selected no of trades allocated grid.
        /// </value>
        public int SelectedNoOfTradesAllocatedGrid
        {
            get { return (int)GetValue(SelectedNoOfTradesAllocatedGridProperty); }
            set { SetValue(SelectedNoOfTradesAllocatedGridProperty, value); }
        }

        /// <summary>
        /// The total no of trades allocated grid property
        /// </summary>
        public static readonly DependencyProperty TotalNoOfTradesAllocatedGridProperty = DependencyProperty.Register("TotalNoOfTradesAllocatedGrid", typeof(int), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Gets or sets the total no of trades allocated grid.
        /// </summary>
        /// <value>
        /// The total no of trades allocated grid.
        /// </value>
        public int TotalNoOfTradesAllocatedGrid
        {
            get { return (int)GetValue(TotalNoOfTradesAllocatedGridProperty); }
            set { SetValue(TotalNoOfTradesAllocatedGridProperty, value); }
        }

        /// <summary>
        /// The is allocated header checked property
        /// </summary>
        public static readonly DependencyProperty IsAllocatedHeaderCheckedProperty = DependencyProperty.Register("IsAllocatedHeaderChecked", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AllocatedHeaderCheckChanged));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocated header checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocated header checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocatedHeaderChecked
        {
            get { return (bool)GetValue(IsAllocatedHeaderCheckedProperty); }
            set { SetValue(IsAllocatedHeaderCheckedProperty, value); }
        }


        /// <summary>
        /// The is refresh after get data property
        /// </summary>
        public static readonly DependencyProperty IsRefreshAfterGetDataProperty = DependencyProperty.Register("IsRefreshAfterGetData", typeof(bool), typeof(AllocatedGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RefreshAfterGetData));

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

        private bool _IsBloombergEXCodeAvailable=false;
        private const string _const_BloombergEXCode = "BloombergSymbolWithExchangeCode";
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

                AllocatedGridBehavior gridExtender = (AllocatedGridBehavior)d;
                gridExtender.AssociatedObject.ExecuteCommand(DataPresenterCommands.EndEditModeAndCommitRecord);
                gridExtender.EndEditModeAndCommitChanges = false;
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
                    SelectedNoOfTradesAllocatedGrid = AssociatedObject.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(AssociatedObject.RecordManager.GetFilteredInDataRecords()).Count();
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
                e.ToolWindow.Title = "Allocated Grid Field Chooser";
                e.ToolWindow.Width = 260;
                e.ToolWindow.Height = 300;
                e.ToolWindow.Loaded += ToolWindow_Loaded;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Tool Window Loaded of Field Chooser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var comboSelector = Infragistics.Windows.Utilities.GetDescendantFromName((sender as ToolWindow), AllocationUIConstants.CONST_FIELD_GROUP_SELECTOR) as XamComboEditor;
                comboSelector.DropDownOpened += comboSelector_DropDownOpened;
                AutomationProperties.SetAutomationId(sender as ToolWindow, "AllocatedGridFieldChooser");
                AutomationProperties.SetName(sender as ToolWindow, "Allocated Grid Field Chooser");
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Combo Drop Down Opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comboSelector_DropDownOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                var popup = Infragistics.Windows.Utilities.GetDescendantFromType(sender as XamComboEditor, typeof(Popup), true) as Popup;
                var dropdown = popup.Child;

                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var itemsPresenter = Infragistics.Windows.Utilities.GetDescendantFromType(dropdown, typeof(ItemsPresenter), true);
                    var items = FindVisualChildren<ComboBoxItem>(itemsPresenter);
                    int i = 0;
                    foreach (ComboBoxItem item in items)
                    {
                        i++;
                        if (i != 1 && i != 2 || String.IsNullOrEmpty(item.Content.ToString()))
                            item.Visibility = Visibility.Collapsed;
                    }
                }));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }


        // Find visual children of an element
        //For more info about Yield: http://www.dotnetperls.com/yield
        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        /// <summary>
        /// Handles the RecordActivated event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RecordActivatedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_RecordActivatedDeleted(object sender, RoutedEventArgs e)
        {
            try
            {
                //Updating Trade Counter
                TotalNoOfTradesAllocatedGrid = AssociatedObject.RecordManager.GetFilteredInDataRecords().Count();
                SelectedNoOfTradesAllocatedGrid = AssociatedObject.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(AssociatedObject.RecordManager.GetFilteredInDataRecords()).Count();
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
                    Type type = e.Cell.Record.DataItem.GetType();
                    AllocationGroup allocationGroup = type.Name.Equals(AllocationUIConstants.TAXLOT_FIELD_LAYOUT_NAME) ? (AllocationGroup)e.Cell.Record.ParentDataRecord.DataItem : (AllocationGroup)e.Cell.Record.DataItem;
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
        /// Handles the RecordFilterChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RecordFilterChangedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_RecordFilterChanged(object sender, RecordFilterChangedEventArgs e)
        {
            try
            {
                TotalNoOfTradesAllocatedGrid = AssociatedObject.RecordManager.GetFilteredInDataRecords().Count();
                SelectedNoOfTradesAllocatedGrid = AssociatedObject.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(AssociatedObject.RecordManager.GetFilteredInDataRecords()).Count();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        ///Loaded 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadLayout();
                AddStringColumns();
                XamDataGrid dataGrid = (sender as XamDataGrid);

                //Set label with split camel case
                for (int j = 0; j < dataGrid.FieldLayouts[0].Fields.Count; j++)
                    dataGrid.FieldLayouts[0].Fields[j].Label = CommonAllocationMethods.SplitCamelCase(dataGrid.FieldLayouts[0].Fields[j].Label.ToString());

                //Column Customization For Group
                if (dataGrid.FieldLayouts.Any(x => x.Description.ToString().Equals(AllocationUIConstants.ALLOCATION_GROUP_FIELD_LAYOUT_NAME)))
                {
                    FieldLayout allocationGroupFieldLayout = dataGrid.FieldLayouts.FirstOrDefault(x => x.Description.ToString().Equals(AllocationUIConstants.ALLOCATION_GROUP_FIELD_LAYOUT_NAME));
                    
                    if(!_IsBloombergEXCodeAvailable)
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
        void AssociatedObject_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            try
            {
                bool isSelected = AssociatedObject.Records.Any(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected);
                bool isAllSwappedGroups = !AssociatedObject.Records.Any(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected && (((x as DataRecord).DataItem) as AllocationGroup).IsSwapped == false);
                UnallocateMenuVisibility = isSelected;
                IsAllocatedUnGroupMenuEnabled = isSelected && !isAllSwappedGroups;
                IsAllocatedSecurityMasterEnabled = ModuleManager.CheckModulePermissioning(PranaModules.SECURITY_MASTER_MODULE, PranaModules.SECURITY_MASTER_MODULE);
                IsAllocatedAuditTrailEnabled = ModuleManager.CheckModulePermissioning(PranaModules.AUDIT_TRAIL_MODULE, PranaModules.AUDIT_TRAIL_MODULE);
                bool isClosingPermitted = ModuleManager.CheckModulePermissioning(PranaModules.CLOSE_POSITIONS_MODULE, PranaModules.CLOSE_POSITIONS_MODULE);
                bool isNAVLocked = false;
                if (CachedDataManager.GetInstance.NAVLockDate.HasValue)
                {
                    if (isSelected)
                    {
                        isNAVLocked = AssociatedObject.Records.Any(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected
                                    && (((x as DataRecord).DataItem) as AllocationGroup).AUECLocalDate.Date <= CachedDataManager.GetInstance.NAVLockDate.Value);
                    }
                    else if(AssociatedObject.ActiveDataItem != null)
                    {
                        if(AssociatedObject.ActiveDataItem is AllocationGroup)
                        {
                            isNAVLocked = !CachedDataManager.GetInstance.ValidateNAVLockDate((AssociatedObject.ActiveDataItem as AllocationGroup).AUECLocalDate.Date);
                        }
                        else if(AssociatedObject.ActiveDataItem is TaxLot)
                        {
                            var taxlot = (TaxLot)AssociatedObject.ActiveDataItem;
                            var group = AssociatedObject.Records.FirstOrDefault(x => x is DataRecord && ((x as DataRecord).DataItem as AllocationGroup).GroupID == taxlot.GroupID);
                            if(group != null)
                            {
                                isNAVLocked = !CachedDataManager.GetInstance.ValidateNAVLockDate(((group as DataRecord).DataItem as AllocationGroup).AUECLocalDate.Date);
                            }
                        }
                    }
                    
                }
                IsAllocatedCloseOrderEnabled = isClosingPermitted && !isNAVLocked;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Add String Columns
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
                AllocationGroup allocationGroup = e.Cell.Record.DataItem.GetType().Name.Equals(AllocationUIConstants.TAXLOT_FIELD_LAYOUT_NAME) ? (AllocationGroup)e.Cell.Record.ParentDataRecord.DataItem : (AllocationGroup)e.Cell.Record.DataItem;
                Dictionary<string, PostTradeEnums.Status> statusDictionary = AllocationClientManager.GetInstance().GetGroupStatus(new List<AllocationGroup> { allocationGroup });
                PostTradeEnums.Status groupStatus = statusDictionary[allocationGroup.GroupID];

                //Update Settlement Currency
                if (e.Cell.Field.Name.Equals(AllocationUIConstants.SETTLEMENT_CURRENCY))
                    ((AllocationGroup)e.Cell.Record.DataItem).SettlementCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(e.Editor.Text);

                string errormessage = string.Empty;
                bool result = EditTradeValidation.CheckGroupStatus(groupStatus, e.Cell.Field.Name, allocationGroup.TransactionType, ref errormessage);
                result = !result ? result : EditTradeValidation.ValidationOnField(e.Cell.Field.Name, allocationGroup, groupStatus, ref errormessage);

                if (!result)
                {
                    AssociatedObject.CellChanged -= AssociatedObject_CellChanged;
                    e.Cell.Record.CancelUpdate();
                    AssociatedObject.CellChanged += AssociatedObject_CellChanged;
                    MessageBoxImage img = errormessage.Equals(AllocationUIConstants.MSG_AUTO_CALCULATE_FIELD) ? MessageBoxImage.Information : MessageBoxImage.Warning;
                    MessageBox.Show(errormessage, AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, img);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }



        /// <summary>
        /// Handles the CellDeactivating event of the AssociatedObject control.
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

                    Type type = e.Cell.Record.DataItem.GetType();
                    AllocationGroup group = type.Name.Equals(AllocationUIConstants.TAXLOT_FIELD_LAYOUT_NAME) ? (AllocationGroup)e.Cell.Record.ParentDataRecord.DataItem : (AllocationGroup)e.Cell.Record.DataItem;
                    group.IsRecalculateCommission = false;
                    group.IsModified = true;

                    // Update group level fields
                    if (type.Name.Equals(AllocationUIConstants.ALLOCATION_GROUP_FIELD_LAYOUT_NAME))
                    {
                        EditTradeHelper.UpdateGroupFields(e.Cell.Field.Name, e.Cell.Value, group);
                        EditTradeHelper.UpdateSecurityFields(e.Cell.Field.Name, group);
                        EditTradeHelper.UpdateTradeAttributes(e.Cell.Field.Name, group, null);
                        EditTradeHelper.UpdateCommissionAndFees(e.Cell.Field.Name, group, null, e.Cell.Value);

                        // Update taxlot and order level
                        EditTradeHelper.UpdateTaxlotandOrders(e.Cell.Field.Name, e.Cell.Value.ToString(), group);
                    }
                    // Update taxlot level fields
                    else if (type.Name.Equals(AllocationUIConstants.TAXLOT_FIELD_LAYOUT_NAME))
                    {
                        TaxLot taxlot = (TaxLot)e.Cell.Record.DataItem;
                        EditTradeHelper.UpdateTaxlotFields(e.Cell.Field.Name, group, taxlot);
                        EditTradeHelper.UpdateCommissionAndFees(e.Cell.Field.Name, group, taxlot, e.Cell.Value);
                        EditTradeHelper.UpdateTradeAttributes(e.Cell.Field.Name, group, taxlot);

                        // Update order level
                        EditTradeHelper.UpdateGroupOrder(group);
                    }


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

                    if (((dr.DataItem is TaxLot && dr.FieldLayout.Description.Equals(AllocationUIConstants.TAXLOT_FIELD_LAYOUT_NAME)) || dr.DataItem is AllocationGroup) && dr.DataPresenter.Name.Equals(AllocationClientConstants.CONST_GIRD_ALLOCATED))
                    {
                        if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.SETTLEMENT_CURRENCY) != -1 && dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.SETTLEMENT_CURRENCYID) != -1)
                            dr.Cells[AllocationUIConstants.SETTLEMENT_CURRENCY].Value = FieldCalculator.GetSettlementCurrency(Convert.ToInt32(dr.Cells[AllocationUIConstants.SETTLEMENT_CURRENCYID].Value));

                        if (dr.DataItem is AllocationGroup)
                        {
                            if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.COUNTERPARTY_NAME) != -1 && dr.FieldLayout.Fields.IndexOf(OrderFields.PROPERTY_COUNTERPARTY_NAME) != -1 && dr.Cells[AllocationUIConstants.COUNTERPARTY_NAME].Value.Equals("Undefined"))
                                dr.Cells[AllocationUIConstants.COUNTERPARTY_NAME].Value = ApplicationConstants.C_COMBO_SELECT;
                            if (dr.Cells[AllocationUIConstants.BorrowerBroker].Value != null)
                            {
                                if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.BorrowerBroker) != -1 && dr.FieldLayout.Fields.IndexOf(OrderFields.PROPERTY_BORROWERBROKER) != -1 && dr.Cells[AllocationUIConstants.BorrowerBroker].Value.Equals("Undefined"))
                                    dr.Cells[AllocationUIConstants.BorrowerBroker].Value = ApplicationConstants.C_COMBO_SELECT;
                            }
                            if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.VENUE) != -1 && dr.FieldLayout.Fields.IndexOf(OrderFields.PROPERTY_VENUE) != -1 && (int)dr.Cells[AllocationUIConstants.VENUE_ID].Value == 0)
                                dr.Cells[AllocationUIConstants.VENUE].Value = ApplicationConstants.C_COMBO_SELECT;

                            if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ACCOUNT_NAME) != -1)
                                dr.Cells[AllocationUIConstants.ACCOUNT_NAME].Value = FieldCalculator.GetAccountName(((AllocationGroup)dr.DataItem).TaxLots);

                            if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.MASTER_FUND_NAME) != -1)
                                dr.Cells[AllocationUIConstants.MASTER_FUND_NAME].Value = FieldCalculator.GetMasterFundName((AllocationGroup)dr.DataItem);

                            if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.STRATEGY_NAME) != -1)
                                dr.Cells[AllocationUIConstants.STRATEGY_NAME].Value = FieldCalculator.GetStrategyName(((AllocationGroup)dr.DataItem).TaxLots);

                            if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.IMPORT_FILE_NAME) != -1)
                                dr.Cells[AllocationUIConstants.IMPORT_FILE_NAME].Value = FieldCalculator.GetImportFileName(((AllocationGroup)dr.DataItem).Orders);

                            if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ASSET_CATEGORY) != -1 && dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.IS_SWAPPED) != -1 && dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ASSET_ID) != -1 && dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ASSET_NAME) != -1)
                                dr.Cells[AllocationUIConstants.ASSET_CATEGORY].Value = FieldCalculator.GetAssetCategory(Convert.ToBoolean(dr.Cells[AllocationUIConstants.IS_SWAPPED].Value), Convert.ToInt32(dr.Cells[AllocationUIConstants.ASSET_ID].Value), dr.Cells[AllocationUIConstants.ASSET_NAME].Value.ToString());

                            //disable fields if base currency and local currencies are same
                            bool _groupCellState = !CachedDataManager.GetInstance.GetCompanyBaseCurrencyID().Equals(((AllocationGroup)dr.DataItem).CurrencyID);
                            if (!_groupCellState)
                            {
                                dr.Cells[AllocationUIConstants.FX_CONVERSION_METHOD_OPERATOR].IsEnabled = _groupCellState;
                                dr.Cells[AllocationUIConstants.FXRATE].IsEnabled = _groupCellState;
                            }
                        }
                        else if (dr.DataItem is TaxLot)
                        {
                            //disable fields if base currency and local currencies are same
                            bool _taxlotCellState = !CachedDataManager.GetInstance.GetCompanyBaseCurrencyID().Equals(((TaxLot)dr.DataItem).CurrencyID);
                            if (!_taxlotCellState)
                            {
                                dr.Cells[AllocationUIConstants.FX_CONVERSION_METHOD_OPERATOR].IsEnabled = _taxlotCellState;
                                dr.Cells[AllocationUIConstants.FXRATE].IsEnabled = _taxlotCellState;
                            }
                        }
                        if (dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ASSET_CATEGORY) != -1 && dr.FieldLayout.Fields.IndexOf(AllocationUIConstants.ACCRUED_INTEREST) != -1)
                        {
                            string assetCategory = (string)dr.Cells[AllocationUIConstants.ASSET_CATEGORY].Value;
                            if (!(assetCategory == "FixedIncome" || assetCategory == "ConvertibleBond"))
                                dr.Cells[AllocationUIConstants.ACCRUED_INTEREST].Field.AllowEdit = false;
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
        /// Saves the allocated grid layout.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SaveAllocatedGridLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                    return;

                AllocatedGridBehavior gridExtender = (AllocatedGridBehavior)d;
                File.WriteAllText(CommonAllocationMethods.GetDirectoryPath() + @"\" + CommonAllocationMethods.GetFileName(gridExtender.AssociatedObject.Name) + ".xml", AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID);
                FileStream fs = new FileStream(CommonAllocationMethods.GetDirectoryPath() + @"\" + CommonAllocationMethods.GetFileName(gridExtender.AssociatedObject.Name) + ".xml", FileMode.Append, FileAccess.Write, FileShare.None);
                gridExtender.AssociatedObject.SaveCustomizations(fs);
                fs.Close();
                gridExtender.IsSaveLayoutAllocatedGrid = false;
                _defaultLayout = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
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
                            if (xmlString.Contains(_const_BloombergEXCode))
                            {
                                _IsBloombergEXCodeAvailable = true;
                            }
                            if (xmlString.Contains("Version"))
                            {
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

                AllocatedGridBehavior gridExtender = (AllocatedGridBehavior)d;
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
                gridExtender.IsExpandCollapseAllocatedGrid = false;
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
                AllocatedGridBehavior gridExtender = (AllocatedGridBehavior)d;
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
        /// Allocateds the header check changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void AllocatedHeaderCheckChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AllocatedGridBehavior gridExtender = (AllocatedGridBehavior)d;
                gridExtender.AssociatedObject.CellUpdated -= gridExtender.AssociatedObject_CellUpdated;
                gridExtender.AssociatedObject.InitializeRecord -= gridExtender.AssociatedObject_InitializeRecord;
                if (gridExtender.IsAllocatedHeaderChecked)
                {
                    gridExtender.AssociatedObject.RecordManager.GetFilteredInDataRecords().ToList().ForEach(record =>
                        {
                            (record.DataItem as AllocationGroup).IsSelected = true;
                        }
                    );
                }
                else
                {
                    gridExtender.AssociatedObject.RecordManager.GetFilteredInDataRecords().ToList().ForEach(record =>
                        {
                            (record.DataItem as AllocationGroup).IsSelected = false;
                        }
                    );
                }
                gridExtender.SelectedNoOfTradesAllocatedGrid = gridExtender.AssociatedObject.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(gridExtender.AssociatedObject.RecordManager.GetFilteredInDataRecords()).Count();
                ((AllocatedGridBehavior)d).AssociatedObject.InitializeRecord += ((AllocatedGridBehavior)d).AssociatedObject_InitializeRecord;
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

                AllocatedGridBehavior gridExtender = (AllocatedGridBehavior)d;
                XamDataGrid dataGrid = gridExtender.AssociatedObject;
                gridExtender.TotalNoOfTradesAllocatedGrid = dataGrid.RecordManager.GetFilteredInDataRecords().Count();
                gridExtender.SelectedNoOfTradesAllocatedGrid = dataGrid.Records.Where(x => x is DataRecord && (((x as DataRecord).DataItem) as AllocationGroup).IsSelected).ToList().Intersect(dataGrid.RecordManager.GetFilteredInDataRecords()).Count();
                gridExtender.IsRefreshAfterGetData = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the FieldLayoutInitialized event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FieldLayoutInitializedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_FieldLayoutInitialized(object sender, FieldLayoutInitializedEventArgs e)
        {
            try
            {
                if (!_isCustomizationCompleted)
                {
                    XamDataGrid dataGrid = (sender as XamDataGrid);
                    GridCustomizationHelper.AddUnboundColumns(dataGrid);
                    //To Load layout of Taxlot and customization of Taxlot fields
                    if (dataGrid.Records.Count > 0)
                    {
                        if (dataGrid.FieldLayouts.Any(x => x.Description.ToString().Equals(AllocationUIConstants.TAXLOT_FIELD_LAYOUT_NAME)))
                        {
                            FieldLayout taxlotFieldLayout = dataGrid.FieldLayouts.FirstOrDefault(x => x.Description.ToString().Equals(AllocationUIConstants.TAXLOT_FIELD_LAYOUT_NAME));
                            GridCustomizationHelper.ColumnsCustomizationForTaxlots(taxlotFieldLayout, _defaultLayout, dataGrid.Resources);
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
                AssociatedObject.CellChanged -= AssociatedObject_CellChanged;
                AssociatedObject.ContextMenuOpening -= AssociatedObject_ContextMenuOpening;
                AssociatedObject.Loaded -= AssociatedObject_Loaded;
                AssociatedObject.RecordFilterChanged -= AssociatedObject_RecordFilterChanged;
                AssociatedObject.RecordActivated -= AssociatedObject_RecordActivatedDeleted;
                AssociatedObject.RecordsDeleted -= AssociatedObject_RecordActivatedDeleted;
                AssociatedObject.FieldChooserOpening -= AssociatedObject_FieldChooserOpening;
                AssociatedObject.CellUpdated -= AssociatedObject_CellUpdated;
                AssociatedObject.FieldLayoutInitialized -= AssociatedObject_FieldLayoutInitialized;
                AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
                AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
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
