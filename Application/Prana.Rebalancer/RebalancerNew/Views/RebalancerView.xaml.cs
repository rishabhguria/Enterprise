using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using Infragistics.Windows.Editors;
using Prana.Global;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Prana.Rebalancer.RebalancerNew.Views
{
    /// <summary>
    /// Interaction logic for RebalancerTabView.xaml
    /// </summary>
    public partial class RebalancerView : UserControl, IDisposable
    {
        StringsValuesSummaryCalculator stringsValuesSummaryCalculator;
        RebalancerGridSummaryCalculator rebalancerGridSummaryCalculator;

        public RebalancerView()
        {
            try
            {
                Infragistics.Windows.Utilities.EnableModelessKeyboardInterop();
                var c = Assembly.GetExecutingAssembly().CodeBase;
                stringsValuesSummaryCalculator = new StringsValuesSummaryCalculator();
                rebalancerGridSummaryCalculator = new RebalancerGridSummaryCalculator();
                SummaryCalculator.Register(stringsValuesSummaryCalculator);
                SummaryCalculator.Register(rebalancerGridSummaryCalculator);
                InitializeComponent();
                LoadRebalancerGridLayout();
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

        private void LoadRebalancerGridLayout()
        {
            try
            {
                string rebalancerGridPreferencesFilePath = GetRebalancerGridPreferencePath();

                if (File.Exists(rebalancerGridPreferencesFilePath))
                {
                    using (FileStream fs = new FileStream(rebalancerGridPreferencesFilePath, FileMode.Open, FileAccess.Read))
                    {
                        this.RebalacerDataGrid.LoadCustomizations(fs);
                    }
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

        private string GetRebalancerGridPreferencePath()
        {
            string startPath = System.Windows.Forms.Application.StartupPath;
            string rebalancerPreferencesDirectoryPath = startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
            return rebalancerPreferencesDirectoryPath + @"\RebalancerGridLayout.xml";
        }

        private void RebalacerDataGrid_Sorting(object sender, SortingEventArgs e)
        {
            if (e.SortDescription.FieldName.Equals("Symbol"))
            {
                e.Cancel = true;
                FieldSortDescription aSort = new FieldSortDescription();
                aSort.Direction = System.ComponentModel.ListSortDirection.Ascending;
                aSort.FieldName = "IsCustomModel";
                FieldSortDescription bSort = new FieldSortDescription();
                bSort.Direction = e.SortDescription.Direction;
                bSort.FieldName = e.SortDescription.FieldName;

                this.RebalacerDataGrid.FieldLayouts[0].SortedFields.Clear();
                this.RebalacerDataGrid.FieldLayouts[0].SortedFields.Add(aSort);
                this.RebalacerDataGrid.FieldLayouts[0].SortedFields.Add(bSort);
            }

        }

        private void XamNumericEditor_EditModeStarted(object sender, Infragistics.Windows.Editors.Events.EditModeStartedEventArgs e)
        {
            ((XamNumericEditor)sender).SelectAll();
        }

        private void RebalacerDataGrid_FieldChooserOpening(object sender, FieldChooserOpeningEventArgs e)
        {
            //var converter = new System.Windows.Media.BrushConverter();
            //var brush = (Brush)converter.ConvertFromString("#FF212C39");

            e.FieldChooser.FieldGroupSelectorVisibility = Visibility.Collapsed;
            e.ToolWindow.Title = "Nirvana";
            e.ToolWindow.MaxWidth = 260;
            e.ToolWindow.Height = 300;
            e.ToolWindow.FontFamily = new FontFamily("Tahoma");
            e.ToolWindow.FontSize = 12;
        }

        private void RebalacerDataGrid_InitializeRecord(object sender, InitializeRecordEventArgs e)
        {
            if (e.Record is GroupByRecord)
                if (e.Record.Description.Contains(" items)") || e.Record.Description.Contains(" item)"))
                    e.Record.Description = e.Record.Description.Substring(0, e.Record.Description.LastIndexOf("("));
        }

        private void DeleteLayout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rebalancerGridPreferencesFilePath = GetRebalancerGridPreferencePath();
                if (File.Exists(rebalancerGridPreferencesFilePath))
                    File.Delete(rebalancerGridPreferencesFilePath);
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

        private void NAVGrid_OnDataSourceChanged(object sender, RoutedPropertyChangedEventArgs<IEnumerable> e)
        {
            try
            {
                if (e.OldValue == null || e.NewValue == null || sender == null) return;
                XamDataGrid xamDataGrid = sender as XamDataGrid;
                bool cashColumnVisible = false,
                    otherAssetsMarketValueColumnVisible = false,
                    accrualsColumnVisible = false,
                    swapNavColumn = false,
                    unrealizedPNLColumn = false;
                foreach (var d in e.NewValue)
                {
                    AdjustedAccountLevelNAV adjustedAccounLeveleNAVInstance = d as AdjustedAccountLevelNAV;
                    if (adjustedAccounLeveleNAVInstance != null)
                    {
                        cashColumnVisible = cashColumnVisible ||
                                            adjustedAccounLeveleNAVInstance.IsIncludeCashInBaseCurrency;
                        otherAssetsMarketValueColumnVisible = otherAssetsMarketValueColumnVisible ||
                                                              adjustedAccounLeveleNAVInstance.IsIncludeOtherAssetsNAV;
                        accrualsColumnVisible = accrualsColumnVisible ||
                                                adjustedAccounLeveleNAVInstance.IsIncludeAccrualsInBaseCurrency;
                        swapNavColumn = swapNavColumn || adjustedAccounLeveleNAVInstance.IsIncludeSwapNavAdjustement;
                        unrealizedPNLColumn =
                            unrealizedPNLColumn || adjustedAccounLeveleNAVInstance.IsIncludeUnrealizedPNLOfSwaps;
                    }
                }
                xamDataGrid.FieldLayouts[0].Fields[RebalancerConstants.COL_OtherAssetsMarketValue].Visibility = otherAssetsMarketValueColumnVisible ? Visibility.Visible : Visibility.Collapsed;
                xamDataGrid.FieldLayouts[0].Fields[RebalancerConstants.COL_CashInBaseCurrency].Visibility = cashColumnVisible ? Visibility.Visible : Visibility.Collapsed;
                xamDataGrid.FieldLayouts[0].Fields[RebalancerConstants.COL_AccrualsInBaseCurrency].Visibility = accrualsColumnVisible ? Visibility.Visible : Visibility.Collapsed;
                xamDataGrid.FieldLayouts[0].Fields[RebalancerConstants.COL_SwapNavAdjustment].Visibility = swapNavColumn ? Visibility.Visible : Visibility.Collapsed;
                xamDataGrid.FieldLayouts[0].Fields[RebalancerConstants.COL_UnRealizedPnlOfSwaps].Visibility = unrealizedPNLColumn ? Visibility.Visible : Visibility.Collapsed;
                int count = xamDataGrid.FieldLayouts[0].Fields.Where(p => p.Visibility == Visibility.Visible).Count();
                xamDataGrid.Width = 150 * count;
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

        private void RebalacerDataGrid_OnGrouping(object sender, GroupingEventArgs e)
        {
            try
            {
                if (e.Groups.Length == 0)
                    RebalacerDataGrid.GroupByAreaMulti.IsExpanded = false;
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

        private void CmbAccountsAndGroups_DropDownClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Infragistics.Controls.Editors.XamComboEditor XamComboEditor = sender as Infragistics.Controls.Editors.XamComboEditor;
                if (XamComboEditor != null && XamComboEditor.SelectedItem == null)
                {
                    XamComboEditor.SelectedIndex = -1;
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
        /// docManager_ToolWindowLoaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void docManager_ToolWindowLoaded(object sender, Infragistics.Windows.DockManager.Events.PaneToolWindowEventArgs e)
        {
            e.Window.UseOSNonClientArea = false;
            e.Window.Style = Application.Current.Resources["toolWindowStyle"] as Style;
        }

        /// <summary>
        ///DockManager_SizeChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DockManager_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ComplianceSplitPane.Height = docManager.ActualHeight / 2;
        }

        /// <summary>
        /// ComplianceSplitPane_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComplianceSplitPane_Loaded(object sender, RoutedEventArgs e)
        {
            ComplianceSplitPane.Height = docManager.ActualHeight / 2;
        }

        /// <summary>
        /// Handles checked changed event to sleect unslect filtered rows in grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CheckChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                IEnumerable<DataRecord> rows = RebalacerDataGrid.RecordManager.GetFilteredInDataRecords();
                bool isLocked = (bool)(sender as CheckBox).IsChecked;
                foreach (var row in rows)
                {
                    RebalancerModel item = row.DataItem as RebalancerModel;
                    item.IsLock = isLocked;
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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RebalacerDataGrid.DataContext = null;
                RebalacerDataGrid = null;
                SummaryCalculator.UnRegister(stringsValuesSummaryCalculator);
                stringsValuesSummaryCalculator = null;
                SummaryCalculator.UnRegister(rebalancerGridSummaryCalculator);
                rebalancerGridSummaryCalculator = null;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}