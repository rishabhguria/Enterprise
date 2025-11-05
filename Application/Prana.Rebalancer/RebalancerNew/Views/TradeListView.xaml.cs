using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Editors;
using Prana.Global;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.Rebalancer.RebalancerNew.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.Views
{
    /// <summary>
    /// Interaction logic for TradeListView.xaml
    /// </summary>
    public partial class TradeListView : Window
    {
        public TradeListView(TradeListViewModel tradeListViewModelInstance)
        {
            InitializeComponent();
            DataContext = tradeListViewModelInstance;
            this.Closing += TradeListView_Closing;
            tradeListViewModelInstance.CheckUncheckFilteredRecords += new EventHandler<EventArgs<bool>>(CheckUncheckFilteredRecords);
            tradeListViewModelInstance.GetFilteredTradeListModelEvent += GetFilteredTradeList;
            tradeListViewModelInstance.GetParentWindowEvent += GetWindow;
        }

        private void TradeListView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (this.TradeListGrid != null)
                {
                    XamDataGrid Grid = this.TradeListGrid;
                    if (Grid != null)
                    {
                        Grid.ClearCustomizations(CustomizationType.RecordFilters);
                        Grid.ClearCustomizations(CustomizationType.GroupingAndSorting);
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

        private void XamNumericEditor_EditModeStarted(object sender, Infragistics.Windows.Editors.Events.EditModeStartedEventArgs e)
        {
            ((XamNumericEditor)sender).SelectAll();
        }

        private void DataPresenterExcelExporter_ExportStarted(object sender, Infragistics.Windows.DataPresenter.ExcelExporter.ExportStartedEventArgs e)
        {
            e.DataPresenter.FieldSettings.CellHeight = 20;
        }

        private void CheckUncheckFilteredRecords(object sender, EventArgs<bool> args)
        {
            try
            {
                foreach (var row in this.TradeListGrid.RecordManager.GetFilteredInDataRecords())
                {
                    ((TradeListModel)row.DataItem).IsChecked = args.Value && !((TradeListModel)row.DataItem).IsStaged;
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

        private List<TradeListModel> GetFilteredTradeList()
        {
            var filteredList = new List<TradeListModel>();
            try
            {
                foreach (var row in this.TradeListGrid.RecordManager.GetFilteredInDataRecords())
                {
                    filteredList.Add((TradeListModel)row.DataItem);
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
            return filteredList;
        }

        private Window GetWindow()
        {
            return this;
        }        
    }
}
