using Infragistics;
using Infragistics.Controls.Grids;
using Infragistics.Controls.Grids.Primitives;
using Infragistics.Documents.Excel;
using Infragistics.Windows.Editors;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Controls.Preferences.Behaviours;
using Prana.Allocation.Client.Helper;
using Prana.Allocation.Client.Summary;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Prana.Allocation.Client.Controls.Allocation.Behaviours
{
    /// <summary>
    /// </summary>
    /// <seealso cref="System.Windows.Interactivity.Behavior{Infragistics.Controls.Grids.XamGrid}" />
    public class AccountAndStartegyGridBehavior : Behavior<XamGrid>
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
                AssociatedObject.Loaded += OnGridIntitialized;
                AssociatedObject.CellExitedEditMode += CellExitedAfterEdit;
                AssociatedObject.CellEditingValidationFailed += AssociatedObject_CellEditingValidationFailed;
                AssociatedObject.DataObjectRequested += AssociatedObject_DataObjectRequested;
                AssociatedObject.CellEnteredEditMode += AssociatedObject_CellEnteredEditMode;
                AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Members

        /// <summary>
        /// The selected strategy
        /// </summary>
        public static readonly DependencyProperty SelectedStrategy = DependencyProperty.Register("SelectedStrategyKeyValue", typeof(int), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, BringStrategyColumnInView));

        /// <summary>
        /// The _is cell changing
        /// </summary>
        private bool _isCellChanging = false;

        /// <summary>
        /// The _is target dic updated
        /// </summary>
        private static bool _isTargetDicUpdated = false;

        /// <summary>
        /// The refresh summary
        /// </summary>
        public static readonly DependencyProperty RefreshSummary = DependencyProperty.Register("IsRefreshSummary", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RefreshGridSummary));

        /// <summary>
        /// The grid enable property
        /// </summary>
        public static readonly DependencyProperty GridEnableProperty = DependencyProperty.Register("IsGridEnable", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, GridEditable));

        /// <summary>
        /// The is sort by percentage property
        /// </summary>
        public static readonly DependencyProperty IsSortByPercentageProperty = DependencyProperty.Register("IsSortByPercentage", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SortByPercentage));


        /// <summary>
        /// The is qty column first property
        /// </summary>
        public static readonly DependencyProperty IsQtyColumnFirstProperty = DependencyProperty.Register("IsQtyColumnFirst", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// The set active cell property
        /// </summary>
        public static readonly DependencyProperty SetActiveCellProperty = DependencyProperty.Register("SetActiveCell", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SetActiveCellInGrid));

        /// <summary>
        /// The cell old value
        /// </summary>
        string cellOldValue;

        /// <summary>
        /// The target dic property
        /// </summary>
        public readonly static DependencyProperty TargetDicProperty = DependencyProperty.Register("TargetDic", typeof(SerializableDictionary<int, AccountValue>), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// The Original Allocation dic property
        /// </summary>
        public readonly static DependencyProperty OriginalAllocationDicProperty = DependencyProperty.Register("OriginalAllocationDic", typeof(SerializableDictionary<int, AccountValue>), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(new SerializableDictionary<int, AccountValue>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnOriginalDictionaryChanged));

        /// <summary>
        /// The Account strategy table property
        /// </summary>
        public readonly static DependencyProperty AccountStrategyTableProperty = DependencyProperty.Register("AccountStrategyTable", typeof(DataTable), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// The updated accounts property
        /// </summary>
        public readonly static DependencyProperty UpdatedAccountsProperty = DependencyProperty.Register("UpdatedAccounts", typeof(HashSet<int>), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// The cum quantity property
        /// </summary>
        public readonly static DependencyProperty CumQuantityProperty = DependencyProperty.Register("CumQuantity", typeof(double), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCumQtyChanged));

        /// <summary>
        /// The precision digit property
        /// </summary>
        public readonly static DependencyProperty PrecisionDigitProperty = DependencyProperty.Register("PrecisionDigit", typeof(int), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPrecisionDigitsChanged));

        /// <summary>
        /// The selected trades property
        /// </summary>
        public readonly static DependencyProperty SelectedTradesProperty = DependencyProperty.Register("SelectedTrades", typeof(int), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SetNoOfTradesSummary));

        /// <summary>
        /// The total trades property
        /// </summary>
        public readonly static DependencyProperty TotalTradesProperty = DependencyProperty.Register("TotalTrades", typeof(int), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SetNoOfTradesSummary));

        /// <summary>
        /// The is preference CMB enabled property
        /// </summary>
        public readonly static DependencyProperty IsPrefCmbEnabledProperty = DependencyProperty.Register("IsPrefCmbEnabled", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// The is preference selected property
        /// </summary>
        public readonly static DependencyProperty ISPrefSelectedProperty = DependencyProperty.Register("ISPrefSelected", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// The is preference changed property
        /// </summary>
        public readonly static DependencyProperty IsPrefChangedProperty = DependencyProperty.Register("IsPrefChanged", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RemoveFilters));

        /// <summary>
        /// The summary precision format
        /// </summary>
        private string SummaryPrecisionFormat;

        /// <summary>
        /// The end edit mode property
        /// </summary>
        public static readonly DependencyProperty EndEditModeProperty = DependencyProperty.Register("EndEditModeGrid", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EndEditMode));

        /// <summary>
        /// The save layout account and startegy grid property
        /// </summary>
        public static readonly DependencyProperty SaveLayoutAccountAndStartegyGridProperty = DependencyProperty.Register("IsSaveLayoutAccountAndStartegyGrid", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SaveAccountAndStartegyGridLayout));

        public static readonly DependencyProperty IsExportTriggeredProperty = DependencyProperty.Register("IsAccountStrategyGridExport", typeof(bool), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsExportTriggeredChanged));

        public static readonly DependencyProperty ExportDataPathProperty = DependencyProperty.Register("ExportDataPathAutomation", typeof(string), typeof(AccountAndStartegyGridBehavior), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the Account strategy table.
        /// </summary>
        /// <value>
        /// The Account strategy table.
        /// </value>
        public DataTable AccountStrategyTable
        {
            get { return GetValue(AccountStrategyTableProperty) as DataTable; }
            set { SetValue(AccountStrategyTableProperty, value); }
        }

        /// <summary>
        /// Gets or sets the updated accounts.
        /// </summary>
        /// <value>
        /// The updated accounts.
        /// </value>
        public HashSet<int> UpdatedAccounts
        {
            get { return GetValue(UpdatedAccountsProperty) as HashSet<int>; }
            set { SetValue(UpdatedAccountsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cum quantity.
        /// </summary>
        /// <value>
        /// The cum quantity.
        /// </value>
        public double CumQuantity
        {
            get { return (double)GetValue(CumQuantityProperty); }
            set { SetValue(CumQuantityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is refresh summary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is refresh summary; otherwise, <c>false</c>.
        /// </value>
        public bool IsRefreshSummary
        {
            get { return (bool)GetValue(RefreshSummary); }
            set { SetValue(RefreshSummary, value); }
        }

        public bool IsAccountStrategyGridExport
        {
            get { return (bool)GetValue(IsExportTriggeredProperty); }
            set { SetValue(IsExportTriggeredProperty, value); }
        }

        public string ExportDataPathAutomation
        {
            get { return (string)GetValue(ExportDataPathProperty); }
            set { SetValue(ExportDataPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is grid enable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is grid enable; otherwise, <c>false</c>.
        /// </value>
        public bool IsGridEnable
        {
            get { return (bool)GetValue(GridEnableProperty); }
            set { SetValue(GridEnableProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preference changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is preference changed; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrefChanged
        {
            get { return (bool)GetValue(IsPrefChangedProperty); }
            set { SetValue(IsPrefChangedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preference CMB enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is preference CMB enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrefCmbEnabled
        {
            get { return (bool)GetValue(IsPrefCmbEnabledProperty); }
            set { SetValue(IsPrefCmbEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preference selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is preference selected; otherwise, <c>false</c>.
        /// </value>
        public bool ISPrefSelected
        {
            get { return (bool)GetValue(ISPrefSelectedProperty); }
            set { SetValue(ISPrefSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is sort by percentage.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is sort by percentage; otherwise, <c>false</c>.
        /// </value>
        public bool IsSortByPercentage
        {
            get { return (bool)GetValue(IsSortByPercentageProperty); }
            set { SetValue(IsSortByPercentageProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is qty column first.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is qty column first; otherwise, <c>false</c>.
        /// </value>
        public bool IsQtyColumnFirst
        {
            get { return (bool)GetValue(IsQtyColumnFirstProperty); }
            set { SetValue(IsQtyColumnFirstProperty, value); }
        }

        /// <summary>
        /// Gets or sets the precision digit.
        /// </summary>
        /// <value>
        /// The precision digit.
        /// </value>
        public int PrecisionDigit
        {
            get { return (int)GetValue(PrecisionDigitProperty); }
            set { SetValue(PrecisionDigitProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the selected strategy.
        /// </summary>
        /// <value>
        /// The name of the selected strategy.
        /// </value>
        public int SelectedStrategyKeyValue
        {
            get { return (int)GetValue(SelectedStrategy); }
            set { SetValue(SelectedStrategy, value); }
        }

        public bool IsSaveLayoutAccountAndStartegyGrid
        {
            get { return (bool)GetValue(SaveLayoutAccountAndStartegyGridProperty); }
            set { SetValue(SaveLayoutAccountAndStartegyGridProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected trades.
        /// </summary>
        /// <value>
        /// The selected trades.
        /// </value>
        public int SelectedTrades
        {
            get { return (int)GetValue(SelectedTradesProperty); }
            set { SetValue(SelectedTradesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [set active cell].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [set active cell]; otherwise, <c>false</c>.
        /// </value>
        public bool SetActiveCell
        {
            get { return (bool)GetValue(SetActiveCellProperty); }
            set { SetValue(SetActiveCellProperty, value); }
        }

        /// <summary>
        /// Gets or sets the target dic.
        /// </summary>
        /// <value>
        /// The target dic.
        /// </value>
        public SerializableDictionary<int, AccountValue> TargetDic
        {
            get { return GetValue(TargetDicProperty) as SerializableDictionary<int, AccountValue>; }
            set { SetValue(TargetDicProperty, value); }
        }

        /// <summary>
        /// Gets or sets the OriginalAllocation dic. Created to handle PRANA-18845
        /// </summary>
        /// <value>
        /// The OriginalAllocation dic.
        /// </value>
        public SerializableDictionary<int, AccountValue> OriginalAllocationDic
        {
            get { return GetValue(OriginalAllocationDicProperty) as SerializableDictionary<int, AccountValue>; }
            set { SetValue(OriginalAllocationDicProperty, value); }
        }

        /// <summary>
        /// Gets or sets the total trades.
        /// </summary>
        /// <value>
        /// The total trades.
        /// </value>
        public int TotalTrades
        {
            get { return (int)GetValue(TotalTradesProperty); }
            set { SetValue(TotalTradesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [end edit mode grid].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [end edit mode grid]; otherwise, <c>false</c>.
        /// </value>
        public bool EndEditModeGrid
        {
            get { return (bool)GetValue(EndEditModeProperty); }
            set { SetValue(EndEditModeProperty, value); }
        }
        #endregion Properties

        #region Methods


        /// <summary>
        /// Loads the column layout.
        /// </summary>
        /// <returns></returns>
        private string[] LoadColumnLayout()
        {
            try
            {
                if (AssociatedObject != null)
                {
                    if (Directory.Exists(CommonAllocationMethods.GetDirectoryPath()))
                    {
                        string path = CommonAllocationMethods.GetDirectoryPath() + @"\AccountStrategyGrid.xml";
                        if (File.Exists(path))
                        {
                            return File.ReadAllLines(path);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return new string[0];
        }

        /// <summary>
        /// Saves the allocated grid layout.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SaveAccountAndStartegyGridLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                    return;

                AccountAndStartegyGridBehavior gridExtender = (AccountAndStartegyGridBehavior)d;
                List<string> columnHeaders = new List<string>();
                var columns = gridExtender.AssociatedObject.Columns.AllColumns;
                foreach (ColumnBase col in columns)
                {
                    columnHeaders.Add(col.Key);
                }
                File.WriteAllLines(CommonAllocationMethods.GetDirectoryPath() + @"\AccountStrategyGrid.xml", columnHeaders);

                gridExtender.IsSaveLayoutAccountAndStartegyGrid = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Cell Editing Validation Failed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_CellEditingValidationFailed(object sender, CellValidationErrorEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the CellEnteredEditMode event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EditingCellEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_CellEnteredEditMode(object sender, EditingCellEventArgs e)
        {
            try
            {
                if (!AssociatedObject.ActiveCell.Column.Key.ToString().Equals(AllocationUIConstants.ACCOUNT.ToString()))
                    _isCellChanging = true;

                if (e.Editor is XamNumericEditor)
                {
                    //Select Cell Value If we click in Cell 
                    (e.Editor as XamNumericEditor).StartEditMode();
                    (e.Editor as XamNumericEditor).SelectAll();
                }

                //JIRA: http://jira.nirvanasolutions.com:8080/browse/PRANA-15305
                //Resolving Object Reference
                if (e.Cell.Value != null)
                    cellOldValue = e.Cell.Value.ToString();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the DataObjectRequested event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataObjectCreationEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_DataObjectRequested(object sender, DataObjectCreationEventArgs e)
        {
            try
            {
                if (AssociatedObject.ItemsSource != null && e.ObjectType == typeof(DataRowView))
                {
                    DataTable table = ((DataView)AssociatedObject.ItemsSource).Table;
                    e.NewObject = table.DefaultView[0];
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (_isCellChanging)
                {
                    if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
                    {
                        e.Handled = false;
                        return;
                    }
                    e.Handled = ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)) ? false : true;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Automatics the calculate quantity percentage.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <param name="row">The row.</param>
        private void AutoCalculateQuantityPercentage(XamGrid dataGrid, DataRow row)
        {
            try
            {
                int index = AccountStrategyTable.Rows.IndexOf(row);
                string groupKey = dataGrid.ActiveCell.Column.Key;


                if (groupKey.StartsWith(AllocationUIConstants.ACCOUNT))
                {
                    if (groupKey.Equals(AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE))
                        AccountStrategyTable.Rows[index][AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY] = (Convert.ToDouble(row[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE]) * CumQuantity) / 100;
                    else if (groupKey.Equals(AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY))
                    {
                        if (CumQuantity != 0)
                            AccountStrategyTable.Rows[index][AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE] = (Convert.ToDouble(row[AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY]) * 100) / CumQuantity;
                        else
                            AccountStrategyTable.Rows[index][AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE] = 0;
                    }
                    UpdateStrategyQuantityPercentage(AccountStrategyTable.Rows[index]);
                }
                else
                {
                    //TODO: Improve Logic to get subString
                    int startIndex = groupKey.LastIndexOf(AllocationUIConstants.STRATEGY_PREFIX) + AllocationUIConstants.STRATEGY_PREFIX.Length;
                    int endIndex = (groupKey.EndsWith(AllocationUIConstants.PERCENTAGE)) ? groupKey.IndexOf(AllocationUIConstants.PERCENTAGE) : groupKey.IndexOf(AllocationUIConstants.QUANTITY); ;
                    int length = endIndex - startIndex;
                    string strategyName = groupKey.Substring(startIndex, length);
                    double accountValue = Convert.ToDouble(AccountStrategyTable.Rows[index][AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY]);
                    if (groupKey.Equals(AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.QUANTITY))
                    {
                        if (accountValue != 0)
                            AccountStrategyTable.Rows[index][AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.PERCENTAGE] = (Convert.ToDouble(row[groupKey]) * 100) / accountValue;
                        else
                            AccountStrategyTable.Rows[index][AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.PERCENTAGE] = 0;
                    }
                    else if (groupKey.Equals(AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.PERCENTAGE))
                        AccountStrategyTable.Rows[index][AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.QUANTITY] = (Convert.ToDouble(row[groupKey]) * accountValue) / 100;
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
        /// Brings the strategy column in view.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void BringStrategyColumnInView(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AccountAndStartegyGridBehavior gridExtender = (AccountAndStartegyGridBehavior)d;
                if (gridExtender.AssociatedObject != null && gridExtender.AssociatedObject.Rows.Count > 0)
                {
                    Column associatedColumn = gridExtender.AssociatedObject.Rows[0].Columns.DataColumns.FirstOrDefault(x => x.Key.Equals(gridExtender.SelectedStrategyKeyValue.ToString()));
                    if (associatedColumn != null)
                    {
                        CellBase bringIntoViewCell = gridExtender.AssociatedObject.Rows[0].Cells[associatedColumn.Key];
                        gridExtender.AssociatedObject.ScrollCellIntoView(bringIntoViewCell);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Cells the exited after edit.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CellExitedAfterEdit(object sender, CellExitedEditingEventArgs e)
        {
            try
            {
                XamGrid dataGrid = (sender as XamGrid);
                DataRow row = ((DataRowView)(dataGrid.ActiveItem)).Row;
                if (dataGrid.ActiveCell.GetType() != typeof(FilterRowCell))
                {
                    if (e.Cell.Value == null)
                    {
                        int index = AccountStrategyTable.Rows.IndexOf(row);
                        AccountStrategyTable.Rows[index][e.Cell.Column.Key] = 0;
                    }
                    if (dataGrid != null)
                    {
                        if (IsSortByPercentage &&
                            (dataGrid.ActiveCell.Column.Key.Equals(AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE) || dataGrid.ActiveCell.Column.Key.Equals(AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY)))
                        {
                            AssociatedObject.SortingSettings.SortedColumns.Clear();
                            AssociatedObject.SortingSettings.SortedColumns.Add(AssociatedObject.Columns.DataColumns[AllocationUIConstants.ACCOUNT + AllocationUIConstants.GROUP].AllColumns[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE]);
                            AssociatedObject.SortingSettings.FirstSortDirection = SortDirection.Descending;
                        }

                        if (TargetDic == null)
                            TargetDic = new SerializableDictionary<int, AccountValue>();

                        if (OriginalAllocationDic == null)
                            OriginalAllocationDic = new SerializableDictionary<int, AccountValue>();

                        if (!_isTargetDicUpdated)
                        {
                            TargetDic.Clear();
                            foreach (KeyValuePair<int, AccountValue> entry in OriginalAllocationDic)
                            {
                                TargetDic.Add(entry.Key, entry.Value.Clone());
                            }
                            _isTargetDicUpdated = true;
                        }

                        AutoCalculateQuantityPercentage(dataGrid, row);
                        if (Convert.ToDouble(row[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE]) != 0)
                        {
                            AccountValue accountValue = CommonAllocationMethods.GetAccountValue(row, true);

                            if (TargetDic.ContainsKey(accountValue.AccountId))
                                TargetDic[accountValue.AccountId] = accountValue;
                            else
                                TargetDic.Add(accountValue.AccountId, accountValue);

                            if (OriginalAllocationDic.ContainsKey(accountValue.AccountId))
                                OriginalAllocationDic[accountValue.AccountId] = accountValue;
                            else
                                OriginalAllocationDic.Add(accountValue.AccountId, accountValue);
                        }
                        else
                        {
                            if (row[AllocationUIConstants.ACCOUNT_ID] != DBNull.Value)
                            {
                                int accountId = Convert.ToInt32(row[AllocationUIConstants.ACCOUNT_ID]);
                                if (TargetDic.ContainsKey(accountId))
                                    TargetDic.Remove(accountId);
                                if (OriginalAllocationDic.ContainsKey(accountId))
                                    OriginalAllocationDic.Remove(accountId);
                            }
                        }
                        // Check used to undo the selected pref in pref combo if the Quantity is changed in the Grid by the user, PRANA-15179
                        if (ISPrefSelected && !dataGrid.ActiveCell.Value.ToString().Equals(cellOldValue))
                            IsPrefCmbEnabled = true;

                        //To refresh summary
                        if (e.Cell.Row != null)
                            e.Cell.Row.Manager.RefreshSummaries();
                    }
                }
                _isCellChanging = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Refreshes the grid summary.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void RefreshGridSummary(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                AccountAndStartegyGridBehavior gridExtender = (AccountAndStartegyGridBehavior)d;
                XamGrid dataGrid = gridExtender.AssociatedObject;
                if (dataGrid != null)
                {
                    foreach (Row row in (IList<Row>)dataGrid.Rows)
                    {
                        var rowView = row.Data as DataRowView;
                        if (gridExtender.UpdatedAccounts.Contains((int)rowView[AllocationUIConstants.ACCOUNT_ID]))
                            row.Manager.RefreshSummaries();
                    }
                }
                gridExtender.IsRefreshSummary = false;
                gridExtender.UpdatedAccounts.Clear();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Grids the editable.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void GridEditable(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AccountAndStartegyGridBehavior gridExtender = (AccountAndStartegyGridBehavior)d;
                XamGrid dataGrid = gridExtender.AssociatedObject;

                if (dataGrid != null)
                {
                    if (gridExtender.IsGridEnable)
                        dataGrid.EditingSettings.AllowEditing = EditingType.Cell;
                    else
                        dataGrid.EditingSettings.AllowEditing = EditingType.None;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [cum qty changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCumQtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AccountAndStartegyGridBehavior gridExtender = (AccountAndStartegyGridBehavior)d;
                if (gridExtender.AssociatedObject != null)
                {
                    XamGrid dataGrid = gridExtender.AssociatedObject;
                    StrategyCollection strategyCollection = CommonDataCache.CachedDataManager.GetInstance.GetUserPermittedStrategies();
                    if (dataGrid != null && strategyCollection != null)
                    {
                        Column accountQtyColumn = (Column)dataGrid.Columns.AllColumns[AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY];
                        SetQuantitySummaryFormat(accountQtyColumn, Convert.ToDouble(gridExtender.CumQuantity), gridExtender.SummaryPrecisionFormat);

                        foreach (Strategy strategy in strategyCollection)
                        {
                            if (strategy.StrategyID != int.MinValue)
                            {
                                Column strategyQtyColumn = (Column)dataGrid.Columns.AllColumns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY];
                                SetQuantitySummaryFormat(strategyQtyColumn, Convert.ToDouble(gridExtender.CumQuantity), gridExtender.SummaryPrecisionFormat);

                                Column strategyPercentageColumn = (Column)dataGrid.Columns.AllColumns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE];
                                UpdateStrategyPercentageSummary(strategyPercentageColumn, Convert.ToDouble(gridExtender.CumQuantity));
                            }
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
        /// Called when [grid intitialized].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void OnGridIntitialized(object sender, EventArgs e)
        {
            try
            {
                XamGrid dataGrid = (sender as XamGrid);
                SummaryPrecisionFormat = CommonAllocationMethods.SetPrecisionStringFormatXamGrid(PrecisionDigit);
                TotalQuantityOperand TotalQuantitySummary = new TotalQuantityOperand();
                TotalPercentageOperand TotalPercentageSummary = new TotalPercentageOperand();
                RemainingQuantityOperand RemainingQuantitySummary = new RemainingQuantityOperand();
                RemainingPercentageOperand RemainingPercentageSummary = new RemainingPercentageOperand();
                StringOperand TotalStringSummary = new StringOperand(AllocationClientConstants.SUMMARY_TOTAL);
                StringOperand RemainingStringSummary = new StringOperand(AllocationClientConstants.SUMMARY_REMAINING);
                StringOperand TotalTradesStringSummary = new StringOperand(AllocationClientConstants.SUMMARY_TOTALTRADES);

                TotalQuantitySummary.FormatString = SummaryPrecisionFormat + "/" + CumQuantity;
                TotalPercentageSummary.FormatString = SummaryPrecisionFormat + "/100";
                RemainingQuantitySummary.FormatString = SummaryPrecisionFormat + "/" + CumQuantity;
                RemainingPercentageSummary.FormatString = SummaryPrecisionFormat + "/100";

                List<string> columnLayout = LoadColumnLayout().ToList();

                GroupColumn accountGroup = new GroupColumn();
                accountGroup.Key = AllocationUIConstants.ACCOUNT + AllocationUIConstants.GROUP;
                accountGroup.HeaderText = AllocationUIConstants.ACCOUNT;
                accountGroup.IsFixed = FixedState.Left;

                TextColumn accountIdColumn = new TextColumn();
                accountIdColumn.Key = AllocationUIConstants.ACCOUNT_ID;
                accountIdColumn.Visibility = Visibility.Collapsed;
                accountIdColumn.MaximumWidth = 0.0;
                accountGroup.Columns.Add(accountIdColumn);

                TextColumn accountNameColumn = new TextColumn();
                accountNameColumn.Key = AllocationUIConstants.ACCOUNT;
                accountNameColumn.HeaderText = "Account Name";
                accountNameColumn.IsReadOnly = true;
                accountNameColumn.MinimumWidth = (CommonAllocationMethods.GetMaxAccountLength() * 4) + 110;
                accountNameColumn.SummaryColumnSettings.SummaryOperands.Add(TotalStringSummary);
                accountNameColumn.SummaryColumnSettings.SummaryOperands.Add(RemainingStringSummary);
                accountNameColumn.SummaryColumnSettings.SummaryOperands.Add(TotalTradesStringSummary);
                accountNameColumn.AllowToolTips = AllowToolTips.Always;
                accountNameColumn.FilterColumnSettings.FilteringOperand = new ContainsOperand();
                accountGroup.Columns.Add(accountNameColumn);

                TemplateColumn accountPercentageColumn = CommonAllocationMethods.GetTemplateColumn(AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE, AllocationUIConstants.PERCENTAGE_HEADER, false, new List<object> { TotalPercentageSummary, RemainingPercentageSummary }, null);

                TemplateColumn accountQuantityColumn = CommonAllocationMethods.GetTemplateColumn(AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY, AllocationUIConstants.QUANTITY_HEADER, false, new List<object> { TotalQuantitySummary, RemainingQuantitySummary }, null);

                if (columnLayout.IndexOf(accountQuantityColumn.Key) < columnLayout.IndexOf(accountPercentageColumn.Key))
                {
                    accountGroup.Columns.Add(accountQuantityColumn);
                    accountGroup.Columns.Add(accountPercentageColumn);
                }
                else
                {
                    accountGroup.Columns.Add(accountPercentageColumn);
                    accountGroup.Columns.Add(accountQuantityColumn);
                }
                dataGrid.Columns.Add(accountGroup);

                List<GroupColumn> strategyGroups = new List<GroupColumn>();
                StrategyCollection strategyCollection = CommonDataCache.CachedDataManager.GetInstance.GetUserPermittedStrategies();
                if (strategyCollection != null)
                {
                    Style cellStyle = AssociatedObject.Resources["DisableEnableCellStyle"] as Style;

                    foreach (Strategy strategy in strategyCollection)
                    {
                        if (strategy.StrategyID != int.MinValue)
                        {

                            TotalQuantityOperand TotalQuantityStrategySummary = new TotalQuantityOperand();
                            StrategyPercentageOperand TotalPercentageStrategySummary = new StrategyPercentageOperand(false);
                            RemainingQuantityOperand RemainingQuantityStrategySummary = new RemainingQuantityOperand();
                            StrategyPercentageOperand RemainingPercentageStrategySummary = new StrategyPercentageOperand(true);

                            TotalQuantityStrategySummary.FormatString = SummaryPrecisionFormat + "/" + CumQuantity;
                            TotalPercentageStrategySummary.FormatString = SummaryPrecisionFormat + "/100";
                            RemainingQuantityStrategySummary.FormatString = SummaryPrecisionFormat + "/" + CumQuantity;
                            RemainingPercentageStrategySummary.FormatString = SummaryPrecisionFormat + "/100";

                            GroupColumn strategyGroup = new GroupColumn();
                            strategyGroup.Key = strategy.StrategyID.ToString();
                            strategyGroup.HeaderText = strategy.Name;

                            TemplateColumn strategyPercentageColumn = CommonAllocationMethods.GetTemplateColumn(AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE, AllocationUIConstants.PERCENTAGE_HEADER, false, new List<object> { TotalPercentageStrategySummary, RemainingPercentageStrategySummary }, cellStyle);

                            TemplateColumn strategyQuantityColumn = CommonAllocationMethods.GetTemplateColumn(AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY, AllocationUIConstants.QUANTITY_HEADER, false, new List<object> { TotalQuantityStrategySummary, RemainingQuantityStrategySummary }, cellStyle);

                            if (columnLayout.IndexOf(strategyQuantityColumn.Key) < columnLayout.IndexOf(strategyPercentageColumn.Key))
                            {
                                strategyGroup.Columns.Add(strategyQuantityColumn);
                                strategyGroup.Columns.Add(strategyPercentageColumn);
                            }
                            else
                            {
                                strategyGroup.Columns.Add(strategyPercentageColumn);
                                strategyGroup.Columns.Add(strategyQuantityColumn);
                            }
                            strategyGroups.Add(strategyGroup);
                        }
                    }
                    if (columnLayout.Count > 0)
                        strategyGroups = strategyGroups.OrderBy(group => columnLayout.Contains(group.Key)).ThenBy(group => columnLayout.IndexOf(group.Key)).ToList();
                    foreach (var group in strategyGroups)
                    {
                        dataGrid.Columns.Add(group);
                    }
                }
                if (IsSortByPercentage)
                {
                    AssociatedObject.SortingSettings.SortedColumns.Clear();
                    AssociatedObject.SortingSettings.SortedColumns.Add(AssociatedObject.Columns.DataColumns[AllocationUIConstants.ACCOUNT + AllocationUIConstants.GROUP].AllColumns[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE]);
                    AssociatedObject.SortingSettings.FirstSortDirection = SortDirection.Descending;
                }

                //Deatching event as this needs to be called once only, currents this is being fired twice
                AssociatedObject.Loaded -= OnGridIntitialized;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Called when [precision digits changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnPrecisionDigitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AccountAndStartegyGridBehavior gridExtender = (AccountAndStartegyGridBehavior)d;
                gridExtender.SummaryPrecisionFormat = CommonAllocationMethods.SetPrecisionStringFormatXamGrid(gridExtender.PrecisionDigit);
                if (gridExtender.AssociatedObject != null)
                {
                    XamGrid dataGrid = gridExtender.AssociatedObject;
                    StrategyCollection strategyCollection = CommonDataCache.CachedDataManager.GetInstance.GetUserPermittedStrategies();
                    if (dataGrid != null && strategyCollection != null)
                    {
                        Column accountQtyColumn = (Column)dataGrid.Columns.AllColumns[AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY];
                        if (accountQtyColumn != null)
                            SetQuantitySummaryFormat(accountQtyColumn, Convert.ToDouble(gridExtender.CumQuantity), gridExtender.SummaryPrecisionFormat);

                        Column accountPercentageColumn = (Column)dataGrid.Columns.AllColumns[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE];
                        if (accountPercentageColumn != null)
                            SetPercentageSummaryFormat(accountPercentageColumn, gridExtender.SummaryPrecisionFormat);

                        foreach (Strategy strategy in strategyCollection)
                        {
                            if (strategy.StrategyID != int.MinValue)
                            {
                                Column strategyQtyColumn = (Column)dataGrid.Columns.AllColumns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY];
                                if (strategyQtyColumn != null)
                                    SetQuantitySummaryFormat(strategyQtyColumn, Convert.ToDouble(gridExtender.CumQuantity), gridExtender.SummaryPrecisionFormat);

                                Column strategyPercentageColumn = (Column)dataGrid.Columns.AllColumns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE];
                                if (strategyPercentageColumn != null)
                                    SetPercentageSummaryFormat(strategyPercentageColumn, gridExtender.SummaryPrecisionFormat);
                            }
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
        /// Removes the filters.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void RemoveFilters(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                    return;

                AccountAndStartegyGridBehavior gridExtender = (AccountAndStartegyGridBehavior)d;
                XamGrid dataGrid = gridExtender.AssociatedObject;

                if (dataGrid != null)
                {
                    RowFiltersCollection collection = dataGrid.FilteringSettings.RowFiltersCollection;
                    foreach (RowsFilter rowFilter in collection)
                        rowFilter.Column.FilterColumnSettings.FilterCellValue = null;

                    dataGrid.FilteringSettings.RowFiltersCollection.Clear();
                }
                gridExtender.IsPrefChanged = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sets the active cell in grid.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SetActiveCellInGrid(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                AccountAndStartegyGridBehavior gridBehavior = (AccountAndStartegyGridBehavior)d;
                gridBehavior.AssociatedObject.ActiveCell = null;
                gridBehavior.SetActiveCell = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [original dictionary changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnOriginalDictionaryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                _isTargetDicUpdated = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sets the no of trades summary.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SetNoOfTradesSummary(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AccountAndStartegyGridBehavior gridExtender = (AccountAndStartegyGridBehavior)d;
                if (gridExtender.AssociatedObject != null)
                {
                    XamGrid dataGrid = gridExtender.AssociatedObject;
                    Column accountColumn = (Column)dataGrid.Columns.AllColumns[AllocationUIConstants.ACCOUNT];
                    StringOperand summary = (StringOperand)accountColumn.SummaryColumnSettings.SummaryOperands.FirstOrDefault(x => ((StringOperand)x).StringValue.StartsWith(AllocationClientConstants.SUMMARY_TOTALTRADES));
                    summary.StringValue = AllocationClientConstants.SUMMARY_TOTALTRADES + gridExtender.SelectedTrades + "/" + gridExtender.TotalTrades;
                    gridExtender.AssociatedObject.ExitEditMode(false);

                    //To refresh summary
                    var row = gridExtender.AssociatedObject.Rows.FirstOrDefault();
                    if (row != null)
                        row.Manager.RefreshSummaries();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sets the percentage summary format.
        /// </summary>
        /// <param name="accountPercentageColumn">The Account percentage column.</param>
        /// <param name="summaryPrecisionFormat">The summary precision format.</param>
        private static void SetPercentageSummaryFormat(Column accountPercentageColumn, string summaryPrecisionFormat)
        {
            try
            {
                foreach (SummaryOperandBase summary in accountPercentageColumn.SummaryColumnSettings.SummaryOperands)
                {
                    summary.FormatString = summaryPrecisionFormat + "/100";
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
        /// Sets the quantity summary format.
        /// </summary>
        /// <param name="quantityColumn">The quantity column.</param>
        /// <param name="cumQty">The cum qty.</param>
        /// <param name="summaryPrecisionFormat">The summary precision format.</param>
        private static void SetQuantitySummaryFormat(Column quantityColumn, double cumQty, string summaryPrecisionFormat)
        {
            try
            {
                foreach (SummaryOperandBase summary in quantityColumn.SummaryColumnSettings.SummaryOperands)
                {
                    summary.FormatString = summaryPrecisionFormat + "/" + cumQty;

                    if (summary.SummaryCalculator.GetType().Equals(typeof(RemainingValueCalc)))
                    {
                        RemainingQuantityOperand remainingQtySummary = (RemainingQuantityOperand)summary;
                        remainingQtySummary.TotalQuantity = cumQty;
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
        /// Sorts the by percentage.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SortByPercentage(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                AccountAndStartegyGridBehavior gridExtender = (AccountAndStartegyGridBehavior)d;
                if (gridExtender.AssociatedObject != null)
                    gridExtender.AssociatedObject.SortingSettings.SortedColumns.Clear();
                //gridExtender.IsSortByPercentage = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Updates the strategy percentage summary.
        /// </summary>
        /// <param name="strategyPercentageColumn">The strategy percentage column.</param>
        /// <param name="cumQty">The cum qty.</param>
        private static void UpdateStrategyPercentageSummary(Column strategyPercentageColumn, double cumQty)
        {
            try
            {
                foreach (SummaryOperandBase summary in strategyPercentageColumn.SummaryColumnSettings.SummaryOperands.Where(x => x.SummaryCalculator.GetType().Equals(typeof(StrategyPercentageValueCalc))))
                {
                    StrategyPercentageOperand percentageSummary = (StrategyPercentageOperand)summary;
                    percentageSummary.TotalQuantity = cumQty;
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
        /// Updates the strategy quantity percentage.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        private void UpdateStrategyQuantityPercentage(DataRow dataRow)
        {
            try
            {
                StrategyCollection strategyCollection = CommonDataCache.CachedDataManager.GetInstance.GetUserPermittedStrategies();
                if (strategyCollection != null)
                {
                    foreach (Strategy strategy in strategyCollection)
                    {
                        if (strategy.StrategyID != int.MinValue)
                        {
                            dataRow[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY] = (Convert.ToDouble(dataRow[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE]) * Convert.ToDouble(dataRow[AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY])) / 100;
                            if (Convert.ToDouble(dataRow[AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY]) == 0.0)
                                dataRow[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE] = 0;
                        }
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

                AccountAndStartegyGridBehavior gridExtender = (AccountAndStartegyGridBehavior)d;
                gridExtender.AssociatedObject.ExitEditMode(false);
                gridExtender.EndEditModeGrid = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private static void OnIsExportTriggeredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (AccountAndStartegyGridBehavior)d;
            if ((bool)e.NewValue == true)
            {
                behavior.ExportGrid(behavior.ExportDataPathAutomation);
                behavior.IsAccountStrategyGridExport = false; // Reset trigger
            }
        }

        private void ExportGrid(string filePath)
        {
            var grid = AssociatedObject;
            var folderPath = Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            if (grid == null || grid.ItemsSource == null || string.IsNullOrWhiteSpace(folderPath))
                return;
            var workbook = new Workbook();
            var worksheet = workbook.Worksheets.Add(grid.Name);

            int row = 0;
            int col = 0;

            // Header row
            foreach (var column in grid.Columns)
            {
                worksheet.Rows[row].Cells[col++].Value = column.HeaderText;
            }

            row++;

            var items = grid.ItemsSource as IEnumerable<object>;
            if (items == null) return;

            foreach (var item in items)
            {
                col = 0;
                foreach (var column in grid.Columns)
                {
                    // Use reflection to get the property value
                    var property = item.GetType().GetProperty(column.Key);
                    var value = property?.GetValue(item, null);
                    worksheet.Rows[row].Cells[col++].Value = value?.ToString();
                }
                row++;
            }

            // Save to specified file
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Save(fs);
            }

        }

        #endregion Methods

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
                AssociatedObject.Loaded -= OnGridIntitialized;
                AssociatedObject.CellExitedEditMode -= CellExitedAfterEdit;
                AssociatedObject.CellEditingValidationFailed -= AssociatedObject_CellEditingValidationFailed;
                AssociatedObject.DataObjectRequested -= AssociatedObject_DataObjectRequested;
                AssociatedObject.CellEnteredEditMode -= AssociatedObject_CellEnteredEditMode;
                AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
                base.OnDetaching();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion OnDetach Events
    }
}
