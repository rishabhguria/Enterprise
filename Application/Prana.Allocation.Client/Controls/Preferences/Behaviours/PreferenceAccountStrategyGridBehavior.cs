using Infragistics.Controls.Grids;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Editors;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Controls.Allocation.Behaviours;
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

namespace Prana.Allocation.Client.Controls.Preferences.Behaviours
{
    class PreferenceAccountStrategyGridBehavior : Behavior<XamGrid>
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
                AssociatedObject.CellEnteredEditMode += AssociatedObject_CellEnteredEditMode;
                AssociatedObject.CellExitedEditMode += OnCellExitedEditMode;
                AssociatedObject.CellEditingValidationFailed += AssociatedObject_CellEditingValidationFailed;
                AssociatedObject.KeyDown += AssociatedObject_KeyDown;
                AssociatedObject.DataObjectRequested += AssociatedObject_DataObjectRequested;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Members

        /// <summary>
        /// The account value dictionary values
        /// </summary>
        public static readonly DependencyProperty AccountValueDictionaryValues = DependencyProperty.Register("AccountValueDictionary", typeof(SerializableDictionary<int, AccountValue>), typeof(PreferenceAccountStrategyGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        /// <summary>
        /// The _is cell changing
        /// </summary>
        private bool _isCellChanging = false;

        /// <summary>
        /// The refresh summary
        /// </summary>
        public static readonly DependencyProperty RefreshSummary = DependencyProperty.Register("IsRefreshSummary", typeof(bool), typeof(PreferenceAccountStrategyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RefreshGridSummary));

        /// <summary>
        /// The end edit mode property
        /// </summary>
        public static readonly DependencyProperty EndEditModeProperty = DependencyProperty.Register("EndEditMode", typeof(bool), typeof(PreferenceAccountStrategyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EndEditModeGrid));

        /// <summary>
        /// to export data for automation Team
        /// </summary>
        public static readonly DependencyProperty IsExportTriggeredProperty = DependencyProperty.Register("IsAccountStrategyGridExport", typeof(bool), typeof(PreferenceAccountStrategyGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsExportTriggeredChanged));

        public static readonly DependencyProperty ExportDataPathProperty = DependencyProperty.Register("ExportDataPathAutomation", typeof(string), typeof(PreferenceAccountStrategyGridBehavior), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the account value dictionary.
        /// </summary>
        /// <value>
        /// The account value dictionary.
        /// </value>
        public SerializableDictionary<int, AccountValue> AccountValueDictionary
        {
            get { return GetValue(AccountValueDictionaryValues) as SerializableDictionary<int, AccountValue>; }
            set { SetValue(AccountValueDictionaryValues, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [end edit mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [end edit mode]; otherwise, <c>false</c>.
        /// </value>
        public bool EndEditMode
        {
            get { return (bool)GetValue(EndEditModeProperty); }
            set { SetValue(EndEditModeProperty, value); }
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
        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the CellEditingValidationFailed event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellValidationErrorEventArgs"/> instance containing the event data.</param>
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
                // the check is added which checks that if column selected is NOT account column than make _iscellchanging true 
                if (!AssociatedObject.ActiveCell.Column.Key.ToString().Equals(AllocationUIConstants.ACCOUNT.ToString()))
                    _isCellChanging = true;

                XamNumericEditor editor = e.Editor as XamNumericEditor;
                if (editor != null)
                {
                    editor.StartEditMode();
                    editor.SelectAll();
                }
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
                PreferenceAccountStrategyGridBehavior gridExtender = (PreferenceAccountStrategyGridBehavior)d;
                XamGrid dataGrid = gridExtender.AssociatedObject;
                if (dataGrid != null)
                {
                    dataGrid.Rows.ToList().ForEach(row => row.Manager.RefreshSummaries());
                }
                gridExtender.IsRefreshSummary = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Ends the edit mode grid.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void EndEditModeGrid(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                    return;

                PreferenceAccountStrategyGridBehavior gridBehavior = (PreferenceAccountStrategyGridBehavior)d;
                gridBehavior.AssociatedObject.ExitEditMode(false);
                gridBehavior.EndEditMode = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [cell exiting edit mode].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExitEditingCellEventArgs"/> instance containing the event data.</param>
        private void OnCellExitedEditMode(object sender, CellExitedEditingEventArgs e)
        {
            try
            {
                XamGrid dataGrid = (sender as XamGrid);
                if (AccountValueDictionary == null)
                    AccountValueDictionary = new SerializableDictionary<int, AccountValue>();
                if (dataGrid != null)
                {

                    if (dataGrid.ActiveCell.Column.Key.Equals(AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE) || dataGrid.ActiveCell.Column.Key.Equals(AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY))
                    {
                        AssociatedObject.SortingSettings.SortedColumns.Clear();
                        AssociatedObject.SortingSettings.SortedColumns.Add(AssociatedObject.Columns.DataColumns[AllocationUIConstants.ACCOUNT + AllocationUIConstants.GROUP].AllColumns[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE]);
                        AssociatedObject.SortingSettings.FirstSortDirection = SortDirection.Descending;
                    }
                    DataRow row = ((DataRowView)(dataGrid.ActiveItem)).Row;

                    // the check [e.Cell.Row.RowType.ToString()!="FilterRow"] is added so that below operation doesnot happen for filter row, PRANA-13408                 
                    if (e.Cell.Value == null && e.Cell.Row.RowType.ToString() != "FilterRow")
                    {
                        DataTable table = ((DataView)AssociatedObject.ItemsSource).Table;
                        int index = table.Rows.IndexOf(row);
                        table.Rows[index][e.Cell.Column.Key] = 0;
                    }

                    if (Convert.ToDouble(row[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE]) != 0)
                    {
                        AccountValue accountValue = CommonAllocationMethods.GetAccountValue(row, false);

                        if (AccountValueDictionary.ContainsKey(accountValue.AccountId))
                            AccountValueDictionary[accountValue.AccountId] = accountValue;
                        else
                            AccountValueDictionary.Add(accountValue.AccountId, accountValue);
                    }
                    else
                    {
                        if (row[AllocationUIConstants.ACCOUNT_ID] != DBNull.Value)
                        {
                            int accountId = Convert.ToInt32(row[AllocationUIConstants.ACCOUNT_ID]);
                            if (AccountValueDictionary.ContainsKey(accountId))
                                AccountValueDictionary.Remove(accountId);
                        }
                    }

                    if (e.Cell.Row != null)
                        e.Cell.Row.Manager.RefreshSummaries();
                }

                _isCellChanging = false;
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
                StringOperand totalStringSummary = new StringOperand(AllocationClientConstants.SUMMARY_TOTAL);
                TotalPercentageOperand totalPercentageSummary = new TotalPercentageOperand();

                XamGrid dataGrid = (sender as XamGrid);

                GroupColumn accountGroup = new GroupColumn();
                accountGroup.Key = AllocationUIConstants.ACCOUNT + AllocationUIConstants.GROUP;
                accountGroup.HeaderText = AllocationUIConstants.ACCOUNT;

                TextColumn accountIdColumn = new TextColumn();
                accountIdColumn.Key = AllocationUIConstants.ACCOUNT_ID;
                accountIdColumn.Visibility = System.Windows.Visibility.Hidden;
                accountIdColumn.MaximumWidth = 0.0;
                accountGroup.Columns.Add(accountIdColumn);

                TextColumn accountNameColumn = new TextColumn();
                accountNameColumn.Key = AllocationUIConstants.ACCOUNT;
                accountNameColumn.HeaderText = "Account Name";
                accountNameColumn.IsReadOnly = true;
                accountNameColumn.SummaryColumnSettings.SummaryOperands.Add(totalStringSummary);
                accountNameColumn.MinimumWidth = (CommonAllocationMethods.GetMaxAccountLength() * 4) + 110;
                accountGroup.Columns.Add(accountNameColumn);
                accountNameColumn.AllowToolTips = AllowToolTips.Always;
                accountNameColumn.FilterColumnSettings.FilteringOperand = new ContainsOperand();

                TemplateColumn accountPercentageColumn = CommonAllocationMethods.GetTemplateColumn(AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE, AllocationUIConstants.PERCENTAGE_HEADER, false, new List<object> { totalPercentageSummary }, null);
                accountPercentageColumn.IsSorted = SortDirection.Descending;
                accountGroup.Columns.Add(accountPercentageColumn);

                dataGrid.Columns.Add(accountGroup);

                StrategyCollection strategyCollection = CommonDataCache.CachedDataManager.GetInstance.GetUserPermittedStrategies();
                if (strategyCollection != null)
                {

                    Style cellStyle = AssociatedObject.Resources["DisableEnableCellStyle"] as Style;
                    foreach (Strategy strategy in strategyCollection)
                    {
                        if (strategy.StrategyID != int.MinValue)
                        {
                            GroupColumn strategyGroup = new GroupColumn();
                            strategyGroup.Key = strategy.StrategyID.ToString();
                            strategyGroup.HeaderText = strategy.Name;

                            TemplateColumn strategyPercentageColumn = CommonAllocationMethods.GetTemplateColumn(AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE, AllocationUIConstants.PERCENTAGE_HEADER, false, new List<object> { totalPercentageSummary }, cellStyle);
                            strategyGroup.Columns.Add(strategyPercentageColumn);

                            dataGrid.Columns.Add(strategyGroup);
                        }
                    }
                }

                //Deatching event as this needs to be called once only, currents this is being fired twice
                AssociatedObject.Loaded -= OnGridIntitialized;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
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
                AssociatedObject.CellExitedEditMode -= OnCellExitedEditMode;
                AssociatedObject.CellEditingValidationFailed -= AssociatedObject_CellEditingValidationFailed;
                AssociatedObject.CellEnteredEditMode -= AssociatedObject_CellEnteredEditMode;
                AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
                AssociatedObject.DataObjectRequested -= AssociatedObject_DataObjectRequested;
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

        private static void OnIsExportTriggeredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (PreferenceAccountStrategyGridBehavior)d;
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
    }
}
