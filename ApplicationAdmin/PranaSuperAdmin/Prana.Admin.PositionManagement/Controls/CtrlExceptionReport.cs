using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;

using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlExceptionReport : UserControl
    {
        #region Grid Column Names
        
        const string COL_IsSelected = "IsSelected";
        const string COL_Symbol = "Symbol";
        const string COL_Fund = "Fund";
        const string COL_ColumnName = "ColumnName";
        const string COL_ApplicationData = "ApplicationData";
        const string COL_SourceData = "SourceData";
        const string COL_ManualEntry = "ManualEntry";
        const string COL_Status = "Status";
        const string COL_DetailsButton = "DetailsButton";
        const string COL_ManualEntryButton = "ManualEntryButton";
        
        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        ExceptionReportSummary _exceptionReportSummary = null;

        UltraGridBand _exceptionGridBand = null;
        UltraGridBand _unknownGridBand = null;

        public CtrlExceptionReport()
        {
            InitializeComponent();
        }

        private DataSourceNameID _dataSourceNameIDValue;

        public DataSourceNameID DataSourceNameIDValue
        {
            get { return _dataSourceNameIDValue; }
            set { _dataSourceNameIDValue = value; }
        }

        #region Initialize the control
        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }


        /// <summary>
        /// Initialize the control.
        /// </summary>
        public void InitControl(DataSourceNameID dataSource)
        {
            if (!_isInitialized)
            {
                SetupBinding(dataSource);
                _dataSourceNameIDValue = dataSource;
                _isInitialized = true;
            }
        }

        #endregion

        private void SetupBinding(DataSourceNameID dataSource)
        {
            _formBindingSource.DataSource = RetrieveExceptionReportSummary(dataSource); // _exceptionReportSummary; 

            BindGridComboBox();

            lblDataSource.DataBindings.Add("Text", _formBindingSource, "DataSource");

            grdException.DataMember = "ExceptionReportEntryList";
            grdException.DataSource = _formBindingSource;

            grdUnknown.DataMember = "UnknownRecordList";
            grdUnknown.DataSource = _formBindingSource;
        }

        private void BindGridComboBox()
        {
            cmbEntryStatus.DisplayMember = "DisplayText";
            cmbEntryStatus.ValueMember = "Value";
            cmbEntryStatus.DataSource = EnumHelper.ConvertEnumForBinding(typeof(ReconStatus));
            Utils.UltraDropDownFilter(cmbEntryStatus, "DisplayText");
        }


        #region Dummy data
        private ExceptionReportSummary RetrieveExceptionReportSummary(DataSourceNameID dataSource)
        {
            ExceptionReportSummary summary = null;

            if (dataSource.ID == 1)
            {
                summary = RetrieveDataSourceId1(dataSource);
            }
            return summary;
        }


        private ExceptionReportSummary RetrieveDataSourceId1(DataSourceNameID dataSource)
        {
            _exceptionReportSummary = new ExceptionReportSummary();
            _exceptionReportSummary.DataSource = dataSource;
            _exceptionReportSummary.ExceptionReportEntryList = RetrieveExceptionReportEntryList();
            _exceptionReportSummary.UnknownRecordList = RetrieveUnknownRecordList();

            return _exceptionReportSummary;
        }

        private BindingList<ExceptionReportEntry> RetrieveExceptionReportEntryList()
        {
            BindingList<ExceptionReportEntry> exceptionReportEntryList = new BindingList<ExceptionReportEntry>();

            ExceptionReportEntry exceptionReportEntry = new ExceptionReportEntry();
            exceptionReportEntry.IsSelected = false;
            exceptionReportEntry.Symbol = "Dell";
            exceptionReportEntry.Fund = "Long";
            exceptionReportEntry.ColumnName = "Position";
            exceptionReportEntry.ApplicationData = "1000";
            exceptionReportEntry.SourceData = "1200";
            exceptionReportEntry.ManualEntry = "";
            exceptionReportEntry.Status = ReconStatus.Open;
            exceptionReportEntryList.Add(exceptionReportEntry);

            exceptionReportEntry = new ExceptionReportEntry();
            exceptionReportEntry.IsSelected = false;
            exceptionReportEntry.Symbol = "Msft";
            exceptionReportEntry.Fund = "Short";
            exceptionReportEntry.ColumnName = "Trade Date";
            exceptionReportEntry.ApplicationData = "1-Jan";
            exceptionReportEntry.SourceData = "2-Jan";
            exceptionReportEntry.ManualEntry = "";
            exceptionReportEntry.Status = ReconStatus.Closed;
            exceptionReportEntryList.Add(exceptionReportEntry);

            exceptionReportEntry = new ExceptionReportEntry();
            exceptionReportEntry.IsSelected = false;
            exceptionReportEntry.Symbol = "1.Hk";
            exceptionReportEntry.Fund = "Arb";
            exceptionReportEntry.ColumnName = "Average Price";
            exceptionReportEntry.ApplicationData = "122.55";
            exceptionReportEntry.SourceData = "122.58";
            exceptionReportEntry.ManualEntry = "";
            exceptionReportEntry.Status = ReconStatus.Closed;
            exceptionReportEntryList.Add(exceptionReportEntry);

            exceptionReportEntry = new ExceptionReportEntry();
            exceptionReportEntry.IsSelected = false;
            exceptionReportEntry.Symbol = "6501.TSE";
            exceptionReportEntry.Fund = "Arb";
            exceptionReportEntry.ColumnName = "Position";
            exceptionReportEntry.ApplicationData = "1000";
            exceptionReportEntry.SourceData = "1200";
            exceptionReportEntry.ManualEntry = "";
            exceptionReportEntry.Status = ReconStatus.Closed;
            exceptionReportEntryList.Add(exceptionReportEntry);

            return exceptionReportEntryList;
        }

        private BindingList<ExceptionReportEntry> RetrieveUnknownRecordList()
        {
            BindingList<ExceptionReportEntry> exceptionReportEntryList = new BindingList<ExceptionReportEntry>();

            ExceptionReportEntry exceptionReportEntry = new ExceptionReportEntry();
            exceptionReportEntry.Symbol = "AABB";
            exceptionReportEntry.Fund = "Unknown";
            exceptionReportEntry.ColumnName = "Split";
            exceptionReportEntry.ApplicationData = "Unknown";
            exceptionReportEntry.SourceData = "200";

            exceptionReportEntryList.Add(exceptionReportEntry);
            return exceptionReportEntryList;
        }


        #endregion

        /// <summary>
        /// Added for now to open the Transaction report and manual entry form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (e.Cell.Column.Key.Equals(COL_DetailsButton))
            {
                OpenTransactionReport();
            }

            if (e.Cell.Column.Key.Equals(COL_ManualEntryButton))
            {
                OpenManualEntry();
            }
        }

        private void OpenTransactionReport()
        {
            TransactionReport transactionReport = new TransactionReport(_dataSourceNameIDValue);
            transactionReport.ShowDialog();
            //if (OnOpeningTransactionReport != null)
            //{
            //    OnOpeningTransactionReport(this, EventArgs.Empty);
            //}
        }

        private void OpenManualEntry()
        {
            ManualEntry manualEntry = new ManualEntry();
            manualEntry.ShowDialog();
            //if (OnOpeningManualEntryReport != null) 
            //{
            //    OnOpeningManualEntryReport(this, EventArgs.Empty);
            //}
        }

        private void grdException_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            grdException.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdException.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdException.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdException.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdException.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdException.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdException.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdException.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            _exceptionGridBand = grdException.DisplayLayout.Bands[0];
            _exceptionGridBand.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colIsSelected = _exceptionGridBand.Columns[COL_IsSelected];
            colIsSelected.Header.Caption = "";
            colIsSelected.Header.VisiblePosition = 1;

            UltraGridColumn colSymbol = _exceptionGridBand.Columns[COL_Symbol];
            colSymbol.Header.VisiblePosition = 2;

            UltraGridColumn colFund = _exceptionGridBand.Columns[COL_Fund];
            colFund.Header.VisiblePosition = 3;

            UltraGridColumn colColumnName = _exceptionGridBand.Columns[COL_ColumnName];
            colColumnName.Header.Caption = "Column Name";
            colColumnName.Header.VisiblePosition = 4;

            UltraGridColumn colApplicationData = _exceptionGridBand.Columns[COL_ApplicationData];
            colApplicationData.Header.Caption = "Application Data";
            colApplicationData.Header.VisiblePosition = 5;

            UltraGridColumn colSourceData = _exceptionGridBand.Columns[COL_SourceData];
            colSourceData.Header.Caption = "Source Data";
            colSourceData.Header.VisiblePosition = 6;

            UltraGridColumn colManualEntry = _exceptionGridBand.Columns[COL_ManualEntry];
            colManualEntry.Header.Caption = "Manual Entry";
            colManualEntry.Header.VisiblePosition = 7;

            UltraGridColumn colStatus = _exceptionGridBand.Columns[COL_Status];
            colStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colStatus.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colStatus.ValueList = cmbEntryStatus;
            colStatus.Header.Caption = "Status";
            colStatus.Header.VisiblePosition = 8;

            UltraGridColumn colDetailsButton = _exceptionGridBand.Columns.Add(COL_DetailsButton);
            colDetailsButton.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            colDetailsButton.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colDetailsButton.Header.VisiblePosition = 9;
            colDetailsButton.Header.Caption = "Details";

            UltraGridColumn colManualEntryButton = _exceptionGridBand.Columns.Add(COL_ManualEntryButton);
            colManualEntryButton.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            colManualEntryButton.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colManualEntryButton.Header.VisiblePosition = 10;
            colManualEntryButton.Header.Caption = "Manual Entry Button";
        }

        private void grdUnknown_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            grdUnknown.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdUnknown.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdUnknown.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdUnknown.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdUnknown.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdUnknown.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdUnknown.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdUnknown.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdUnknown.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            _unknownGridBand = grdUnknown.DisplayLayout.Bands[0];
            _unknownGridBand.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colIsSelected = _unknownGridBand.Columns[COL_IsSelected];
            colIsSelected.Hidden = true;

            UltraGridColumn colSymbol = _unknownGridBand.Columns[COL_Symbol];
            colSymbol.Header.VisiblePosition = 1;

            UltraGridColumn colFund = _unknownGridBand.Columns[COL_Fund];
            colFund.Header.VisiblePosition = 2;


            UltraGridColumn colColumnName = _unknownGridBand.Columns[COL_ColumnName];
            colColumnName.Header.Caption = "Column Name";
            colColumnName.Header.VisiblePosition = 3;

            UltraGridColumn colApplicationData = _unknownGridBand.Columns[COL_ApplicationData];
            colApplicationData.Header.Caption = "Application Data";
            colApplicationData.Header.VisiblePosition = 4;

            UltraGridColumn colSourceData = _unknownGridBand.Columns[COL_SourceData];
            colSourceData.Header.Caption = "Source Data";
            colSourceData.Header.VisiblePosition = 5;

            UltraGridColumn colManualEntry = _unknownGridBand.Columns[COL_ManualEntry];
            colManualEntry.Hidden = true;

            UltraGridColumn colStatus = _unknownGridBand.Columns[COL_Status];
            colStatus.Hidden = true;
        }

    }
}
