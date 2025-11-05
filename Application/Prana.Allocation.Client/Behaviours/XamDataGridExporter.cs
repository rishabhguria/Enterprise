using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.ExcelExporter;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Helper;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;

namespace Prana.Allocation.Client.Behaviours
{
    public class XamDataGridExporter : Behavior<XamDataGrid>
    {
        #region Members
        public static readonly DependencyProperty ExportGridDataProperty = DependencyProperty.Register("ExportGridData", typeof(bool), typeof(XamDataGridExporter), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnExportGridDataChanged));

        public static readonly DependencyProperty ExportFilePathProperty = DependencyProperty.Register("ExportFilePath", typeof(string), typeof(XamDataGridExporter), new PropertyMetadata(string.Empty));

        /// <summary>
        /// The export grid property
        /// </summary>
        public static readonly DependencyProperty ExportGridProperty = DependencyProperty.Register("ExportGrid", typeof(bool), typeof(XamDataGridExporter), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnExportGridValueChanged));

        /// <summary>
        /// The export taxlots property
        /// </summary>
        public static readonly DependencyProperty ExportTaxlotsProperty = DependencyProperty.Register("ExportTaxlots", typeof(bool), typeof(XamDataGridExporter), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnExportTaxlotsValueChanged));

        /// <summary>
        /// The export groups property
        /// </summary>
        public static readonly DependencyProperty ExportGroupsProperty = DependencyProperty.Register("ExportGroups", typeof(bool), typeof(XamDataGridExporter), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnExportGroupsValueChanged));

        /// <summary>
        /// The exporter resource property
        /// </summary>
        public static readonly DependencyProperty ExporterResourceProperty = DependencyProperty.Register("ExporterResource", typeof(DataPresenterExcelExporter), typeof(XamDataGridExporter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// The load layout allocated grid
        /// </summary>
        public static readonly DependencyProperty AssetsWithCommissionInNetAmount = DependencyProperty.Register("AssetsWithCommissionInNetAmountList", typeof(List<int>), typeof(XamDataGridExporter), new FrameworkPropertyMetadata(new List<int>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AssetListChanged));

        /// <summary>
        /// The _file name
        /// </summary>
        private static string _fileName;

        /// <summary>
        /// The _grid name
        /// </summary>
        private static string _gridName;

        /// <summary>
        /// The exporter
        /// </summary>
        private static DataPresenterExcelExporter _exporter = null;

        /// <summary>
        /// The _assets with commission in net amount list
        /// </summary>
        private static List<int> _assetsWithCommissionInNetAmountList = new List<int>();

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

        public bool ExportGridData
        {
            get { return (bool)GetValue(ExportGridDataProperty); }
            set { SetValue(ExportGridDataProperty, value); }
        }

        public string ExportFilePath
        {
            get { return (string)GetValue(ExportFilePathProperty); }
            set { SetValue(ExportFilePathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [export taxlots].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export taxlots]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportTaxlots
        {
            get { return (bool)GetValue(ExportTaxlotsProperty); }
            set { SetValue(ExportTaxlotsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [export groups].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export groups]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportGroups
        {
            get { return (bool)GetValue(ExportGroupsProperty); }
            set { SetValue(ExportGroupsProperty, value); }
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
                _assetsWithCommissionInNetAmountList = ((XamDataGridExporter)d).AssetsWithCommissionInNetAmountList;
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
                XamDataGridExporter xamGridExporter = (XamDataGridExporter)d;
                ExportData(xamGridExporter, ExportHelper.ExportLevel.AllTrades);
                xamGridExporter.ExportGrid = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private static void OnExportGridDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                XamDataGridExporter xamGridExporter = (XamDataGridExporter)d;
                var folderPath = Path.GetDirectoryName(xamGridExporter.ExportFilePath);
                if (!System.IO.Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                XamDataGrid xamDataGrid = xamGridExporter.AssociatedObject;
                _exporter = xamGridExporter.ExporterResource;
                _exporter.CellExporting += exporter_CellExporting;
                WorkbookFormat format = ExportHelper.SetWorkBookFormat(Path.GetExtension(xamGridExporter.ExportFilePath));
                ExportOptions options = new ExportOptions();
                _exporter.Export(xamGridExporter.AssociatedObject, xamGridExporter.ExportFilePath, format, options);
                xamGridExporter.ExportGridData = false;
                _exporter.CellExporting -= exporter_CellExporting;
                _exporter = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [export taxlots value changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnExportTaxlotsValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                XamDataGridExporter xamGridExporter = (XamDataGridExporter)d;
                ExportData(xamGridExporter, ExportHelper.ExportLevel.Taxlots);
                xamGridExporter.ExportTaxlots = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [export groups value changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnExportGroupsValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                XamDataGridExporter xamGridExporter = (XamDataGridExporter)d;
                ExportData(xamGridExporter, ExportHelper.ExportLevel.Groups);
                xamGridExporter.ExportGroups = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Exports the data.
        /// </summary>
        /// <param name="xamGridExporter">The xam grid exporter.</param>
        /// <param name="exportLevel">The export level.</param>
        private static void ExportData(XamDataGridExporter xamGridExporter, ExportHelper.ExportLevel exportLevel)
        {
            try
            {

                if (ExportHelper.IsExportData(xamGridExporter, xamGridExporter.AssociatedObject, _exporter, out _gridName, out _fileName, exportLevel))
                {
                    _exporter = xamGridExporter.ExporterResource;
                    WorkbookFormat format = ExportHelper.SetWorkBookFormat(Path.GetExtension(_fileName));
                    ExportOptions options = new ExportOptions();
                    switch (exportLevel)
                    {
                        case ExportHelper.ExportLevel.AllTrades:
                            _exporter.CellExporting += exporter_CellExporting;
                            _exporter.HeaderLabelExporting += _exporter_HeaderLabelExporting;
                            _exporter.ExportEnding += _exporter_ExportEnding;
                            break;
                        case ExportHelper.ExportLevel.Groups:
                            _exporter.InitializeRecord += _exporter_InitializeRecord;
                            _exporter.CellExporting += exporter_CellExporting;
                            _exporter.HeaderLabelExporting += _exporter_HeaderLabelExporting;
                            break;
                        case ExportHelper.ExportLevel.Taxlots:
                            _exporter.CellExporting += _exporter_TaxlotCellExporting;
                            _exporter.ExportEnding += _exporter_ExportEnding;
                            _exporter.HeaderLabelExporting += _exporter_TaxlotHeaderLabelExporting;
                            options.ChildRecordCollectionSpacing = ChildRecordCollectionSpacing.None;
                            options.ExcludeExpandedState = true;
                            break;
                    }
                    _exporter.ExportEnded += exporter_ExportEnded;
                    _exporter.ExportAsync(xamGridExporter.AssociatedObject, _fileName, format, options);
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
        static void _exporter_InitializeRecord(object sender, InitializeRecordEventArgs e)
        {
            try
            {
                if (_gridName.Equals(AllocationClientConstants.CONST_GIRD_UNALLOCATED) || _gridName.Equals(AllocationClientConstants.CONST_GIRD_ALLOCATED))
                {
                    DataRecord record = e.Record as DataRecord;
                    if (record != null && record.ParentRecord == null && record.HasChildren)
                        e.SkipDescendants = true;
                }
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
        static void _exporter_HeaderLabelExporting(object sender, HeaderLabelExportingEventArgs e)
        {
            try
            {
                e.FormatSettings.FontWeight = FontWeights.Bold;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
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
        /// Handles the ExportEnding event of the _exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExportEndingEventArgs"/> instance containing the event data.</param>
        static void _exporter_ExportEnding(object sender, ExportEndingEventArgs e)
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

        /// <summary>
        /// Handles the TaxlotHeaderLabelExporting event of the _exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="HeaderLabelExportingEventArgs"/> instance containing the event data.</param>
        static void _exporter_TaxlotHeaderLabelExporting(object sender, HeaderLabelExportingEventArgs e)
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
        /// Handles the TaxlotCellExporting event of the _exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellExportingEventArgs"/> instance containing the event data.</param>
        private static void _exporter_TaxlotCellExporting(object sender, CellExportingEventArgs e)
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
        /// Handles the ExportEnded event of the exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Windows.DataPresenter.ExcelExporter.ExportEndedEventArgs"/> instance containing the event data.</param>
        static void exporter_ExportEnded(object sender, ExportEndedEventArgs e)
        {
            try
            {
                ExportHelper.OpenExportedFile(_gridName, _fileName);
                _exporter.InitializeRecord -= _exporter_InitializeRecord;
                _exporter.ExportEnded -= exporter_ExportEnded;
                _exporter.CellExporting -= exporter_CellExporting;
                _exporter.HeaderLabelExporting -= _exporter_HeaderLabelExporting;
                _exporter.HeaderLabelExporting -= _exporter_TaxlotHeaderLabelExporting;
                _exporter.ExportEnding -= _exporter_ExportEnding;
                _exporter.CellExporting -= _exporter_TaxlotCellExporting;
                _exporter = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Methods
    }
}
