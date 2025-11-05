using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.ExcelExporter;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Helper;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Prana.Allocation.Client.Controls.Allocation.Behaviours
{
    class AllocationGridsExporter : Behavior<XamDataGrid>
    {
        #region Members

        /// <summary>
        /// The export grid property
        /// </summary>
        public static readonly DependencyProperty ExportGridProperty = DependencyProperty.Register("ExportGrid", typeof(bool), typeof(AllocationGridsExporter), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnExportGridValueChanged));

        /// <summary>
        /// The exporter resource property
        /// </summary>
        public static readonly DependencyProperty ExporterResourceProperty = DependencyProperty.Register("ExporterResource", typeof(DataPresenterExcelExporter), typeof(AllocationGridsExporter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// The load layout allocated grid
        /// </summary>
        public static readonly DependencyProperty AssetsWithCommissionInNetAmount = DependencyProperty.Register("AssetsWithCommissionInNetAmountList", typeof(List<int>), typeof(AllocationGridsExporter), new FrameworkPropertyMetadata(new List<int>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AssetListChanged));

        /// <summary>
        /// The _assets with commission in net amount list
        /// </summary>
        private static List<int> _assetsWithCommissionInNetAmountList = new List<int>();

        /// <summary>
        /// The _file name
        /// </summary>
        private static string _fileName;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the exporter resource.
        /// </summary>
        /// <value>
        /// The exporter resource.
        /// </value>
        public DataPresenterExcelExporter ExporterResource
        {
            get { return (DataPresenterExcelExporter)GetValue(ExporterResourceProperty); }
            set { SetValue(ExporterResourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [export grid].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export grid]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportGrid
        {
            get { return (bool)GetValue(ExportGridProperty); }
            set { SetValue(ExportGridProperty, value); }
        }

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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Assets the list changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void AssetListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                _assetsWithCommissionInNetAmountList = ((AllocationGridsExporter)d).AssetsWithCommissionInNetAmountList;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [export grid value changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnExportGridValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                AllocationGridsExporter xamGridExporter = (AllocationGridsExporter)d;
                XamDataGrid xamDataGrid = xamGridExporter.AssociatedObject;
                if (xamDataGrid.Records.Count > 0)
                {
                    List<ExportHelper.ExportLevel> exportLevelList = new List<ExportHelper.ExportLevel> { ExportHelper.ExportLevel.Groups };
                    if (xamDataGrid.Name.Equals(AllocationClientConstants.CONST_GIRD_ALLOCATED))
                    {
                        exportLevelList.Add(ExportHelper.ExportLevel.AllTrades);
                        exportLevelList.Add(ExportHelper.ExportLevel.Taxlots);
                    }
                    foreach (ExportHelper.ExportLevel exportLevel in exportLevelList)
                    {
                        SaveFileDialog saveFileDialog = ExportHelper.GetSaveFileDialog(xamDataGrid, exportLevel);
                        ExportGridDataAsync(xamGridExporter, xamDataGrid, saveFileDialog, exportLevel);
                    }
                }
                else
                    System.Windows.MessageBox.Show("Nothing to export for " + xamDataGrid.Name.Replace("Grid", String.Empty) + " grid.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                xamGridExporter.ExportGrid = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Exports the grid data asynchronous.
        /// </summary>
        /// <param name="xamGridExporter">The xam grid exporter.</param>
        /// <param name="xamDataGrid">The xam data grid.</param>
        /// <param name="saveFileDialog">The save file dialog.</param>
        /// <param name="exportLevel">The export level.</param>
        private static void ExportGridDataAsync(AllocationGridsExporter xamGridExporter, XamDataGrid xamDataGrid, SaveFileDialog saveFileDialog, ExportHelper.ExportLevel exportLevel)
        {
            try
            {
                if (saveFileDialog.ShowDialog().Equals(DialogResult.OK))
                {
                    if (!string.IsNullOrWhiteSpace(_fileName) && _fileName.Equals(saveFileDialog.FileName))
                    {
                        System.Windows.MessageBox.Show("Export to this file is already in progress. Please select a different file name.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                        ExportGridDataAsync(xamGridExporter, xamDataGrid, saveFileDialog, exportLevel);
                    }
                    else
                    {
                        _fileName = saveFileDialog.FileName;
                        DataPresenterExcelExporter exporter = xamGridExporter.ExporterResource;
                        WorkbookFormat format = ExportHelper.SetWorkBookFormat(Path.GetExtension(saveFileDialog.FileName));
                        ExportOptions options = new ExportOptions();
                        switch (exportLevel)
                        {

                            case ExportHelper.ExportLevel.AllTrades:
                                exporter.CellExporting += exporter_CellExporting;
                                exporter.HeaderLabelExporting += exporter_HeaderLabelExporting;
                                exporter.ExportEnding += exporter_ExportEnding;
                                break;

                            case ExportHelper.ExportLevel.Groups:
                                exporter.InitializeRecord += exporter_InitializeRecord;
                                exporter.CellExporting += exporter_CellExporting;
                                exporter.HeaderLabelExporting += exporter_HeaderLabelExporting;
                                break;

                            case ExportHelper.ExportLevel.Taxlots:
                                exporter.CellExporting += exporter_TaxlotCellExporting;
                                exporter.ExportEnding += exporter_ExportEnding;
                                exporter.HeaderLabelExporting += exporter_TaxlotHeaderLabelExporting;
                                options.ChildRecordCollectionSpacing = ChildRecordCollectionSpacing.None;
                                options.ExcludeExpandedState = true;
                                break;
                        }
                        exporter.ExportEnded += exporter_ExportEnded;
                        switch (xamDataGrid.Name)
                        {
                            case AllocationClientConstants.CONST_GIRD_ALLOCATED:
                                exporter.Export(xamDataGrid, saveFileDialog.FileName, format, options);
                                break;

                            case AllocationClientConstants.CONST_GIRD_UNALLOCATED:
                                exporter.ExportAsync(xamDataGrid, saveFileDialog.FileName, format, options);
                                break;
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
        /// Handles the InitializeRecord event of the _exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeRecordEventArgs"/> instance containing the event data.</param>
        static void exporter_InitializeRecord(object sender, InitializeRecordEventArgs e)
        {
            try
            {
                DataRecord record = e.Record as DataRecord;
                if (record != null && record.ParentRecord == null && record.HasChildren)
                    e.SkipDescendants = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the ExportEnded event of the exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Windows.DataPresenter.ExcelExporter.ExportEndedEventArgs"/> instance containing the event data.</param>
        static void exporter_ExportEnded(object sender, ExportEndedEventArgs e)
        {
            try
            {
                DataPresenterExcelExporter exporter = (sender as DataPresenterExcelExporter);
                exporter.ExportEnded -= exporter_ExportEnded;
                exporter.CellExporting -= exporter_CellExporting;
                exporter.HeaderLabelExporting -= exporter_HeaderLabelExporting;
                exporter.InitializeRecord -= exporter_InitializeRecord;
                exporter.HeaderLabelExporting -= exporter_TaxlotHeaderLabelExporting;
                exporter.ExportEnding -= exporter_ExportEnding;
                exporter.CellExporting -= exporter_TaxlotCellExporting;
                exporter = null;
                _fileName = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the HeaderLabelExporting event of the _exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="HeaderLabelExportingEventArgs"/> instance containing the event data.</param>
        static void exporter_HeaderLabelExporting(object sender, HeaderLabelExportingEventArgs e)
        {
            try
            {
                e.FormatSettings.FontWeight = FontWeights.Bold;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the CellExporting event of the exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellExportingEventArgs"/> instance containing the event data.</param>
        static void exporter_CellExporting(object sender, CellExportingEventArgs e)
        {
            try
            {
                ExportHelper.UpdateUnboundFieldValues(e, _assetsWithCommissionInNetAmountList);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the TaxlotHeaderLabelExporting event of the exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="HeaderLabelExportingEventArgs"/> instance containing the event data.</param>
        private static void exporter_TaxlotHeaderLabelExporting(object sender, HeaderLabelExportingEventArgs e)
        {
            try
            {
                if ((e.Record.IsDataRecord && (e.Record as DataRecord).DataItem.GetType().Name.Equals(AllocationUIConstants.ALLOCATION_GROUP_FIELD_LAYOUT_NAME)) || e.CurrentRowIndex > 2)
                {
                    e.Cancel = true;
                }
                e.FormatSettings.FontWeight = FontWeights.Bold;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the TaxlotCellExporting event of the exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellExportingEventArgs"/> instance containing the event data.</param>
        private static void exporter_TaxlotCellExporting(object sender, CellExportingEventArgs e)
        {
            try
            {
                Type type = e.Record.DataItem.GetType();
                if (type.Name.Equals(AllocationUIConstants.ALLOCATION_GROUP_FIELD_LAYOUT_NAME))
                {
                    e.Cancel = true;
                }
                else
                {
                    ExportHelper.UpdateUnboundFieldValues(e, _assetsWithCommissionInNetAmountList);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the ExportEnding event of the exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExportEndingEventArgs"/> instance containing the event data.</param>
        private static void exporter_ExportEnding(object sender, ExportEndingEventArgs e)
        {
            try
            {
                ExportHelper.RemoveEmptyRowsAndColumns(e.CurrentWorksheet);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Methods
    }
}
