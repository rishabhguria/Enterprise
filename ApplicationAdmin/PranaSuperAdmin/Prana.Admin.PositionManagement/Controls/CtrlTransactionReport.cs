using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;
using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlTransactionReport : UserControl
    {
        #region Grid Column Names

        #region Record Grid
        const string COL_IsSelected = "IsSelected";
        const string COL_Symbol = "Symbol";
        const string COL_Fund = "Fund";
        const string COL_ColumnName = "ColumnName";
        const string COL_ApplicationData = "ApplicationData";
        const string COL_SourceData = "SourceData";
        const string COL_ManualEntry = "ManualEntry";
        const string COL_Status = "Status";
        const string COL_ManualEntryButton = "ManualEntryButton"; 
        #endregion

        #region Trade Transaction Column
        const string COL_Side = "Side";
        const string COL_ExecQty = "ExecQty";
        const string COL_AvgPrice = "AvgPrice";
        const string COL_CV = "CV";
        const string COL_Notional = "Notional";
        const string COL_Commissions = "Commissions";
        const string COL_Fees = "Fees";
        const string COL_NetAmount = "NetAmount";
        const string COL_Fills = "Fills";
        #endregion

        #region Corporate Action Entry Column
        const string COL_Description = "Description";
        const string COL_Details = "Details";
        const string COL_ImpactOnCash = "ImpactOnCash";
        const string COL_ImpactOnPosition = "ImpactOnPosition"; 
        #endregion


        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        TransactionReportSummary _transactionReportSummary = new TransactionReportSummary();

        public CtrlTransactionReport()
        {
            InitializeComponent();
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
                _isInitialized = true;
            }
        }

        #endregion

        private void SetupBinding(DataSourceNameID dataSource)
        {
            _formBindingSource.DataSource = RetrieveTransactionReportSummary(dataSource);

            lblDataSource.DataBindings.Add("Text", _formBindingSource, "DataSourceNameIDValue");

            grdRecord.DataMember = "ExceptionReportEntry";
            grdRecord.DataSource = _formBindingSource;

            clrTransactionDate.DataBindings.Add("Value", _formBindingSource, "TransactionDate");

            grdTransaction.DataMember = "TradeTransactionList";
            grdTransaction.DataSource = _formBindingSource;

            clrCorporateActions.DataBindings.Add("Value", _formBindingSource, "CorporateActionDate");

            grdCorporateActions.DataMember = "CorporateActionEntryList";
            grdCorporateActions.DataSource = _formBindingSource;

        }

        private TransactionReportSummary RetrieveTransactionReportSummary(DataSourceNameID dataSource)
        {
            _transactionReportSummary.DataSourceNameIDValue = dataSource;
            _transactionReportSummary.ExceptionReportEntry = RetrieveExceptionReportEntry();
            _transactionReportSummary.TransactionDate = DateTime.Now;
            _transactionReportSummary.TradeTransactionList = RetrieveTradeTransactionList();
            _transactionReportSummary.CorporateActionDate = DateTime.Now;
            _transactionReportSummary.CorporateActionEntryList = RetrieveCorporateActionEntryList();

            return _transactionReportSummary;
        }

        private ExceptionReportEntry RetrieveExceptionReportEntry()
        {
            ExceptionReportEntry exceptionReportEntry = new ExceptionReportEntry();
            exceptionReportEntry.Symbol = "Dell";
            exceptionReportEntry.Fund = "Long";
            exceptionReportEntry.ColumnName = "Position";
            exceptionReportEntry.ApplicationData = "1000";
            exceptionReportEntry.SourceData = "1200";

            return exceptionReportEntry;
        }

        private BindingList<TradeTransaction> RetrieveTradeTransactionList()
        {
            BindingList<TradeTransaction> tradeTransactionList = new BindingList<TradeTransaction>();

            TradeTransaction tradeTransaction1 = new TradeTransaction();
            tradeTransaction1.Side = "B";
            tradeTransaction1.ExecQty = 1000;
            tradeTransaction1.AvgPrice = 25.00F;
            tradeTransaction1.CV = "Morgan-Arca";
            tradeTransaction1.Fund = "Long";
            tradeTransaction1.Notional = 2500.00;
            tradeTransaction1.Commissions = 20.00F;
            tradeTransaction1.Fees = 1.00F;
            tradeTransaction1.NetAmount = 2521.00;
            tradeTransactionList.Add(tradeTransaction1);

            tradeTransaction1 = new TradeTransaction();
            tradeTransaction1.Side = "S";
            tradeTransaction1.ExecQty = 1000;
            tradeTransaction1.AvgPrice = 24.00F;
            tradeTransaction1.CV = "GS-Desk";
            tradeTransaction1.Fund = "Long";
            tradeTransaction1.Notional = 2400.00;
            tradeTransaction1.Commissions = 19.00F;
            tradeTransaction1.NetAmount = 2381.00;
            tradeTransactionList.Add(tradeTransaction1);

            return tradeTransactionList;
        }


        private BindingList<CorporateAction> RetrieveCorporateActionEntryList()
        {
            BindingList<CorporateAction> corporateActionEntryList = new BindingList<CorporateAction>();

            CorporateAction corporateActionEntry1 = new CorporateAction();
            corporateActionEntry1.Description = "Dividend";
            corporateActionEntry1.Details = "3 cents a share";
            corporateActionEntry1.ImpactOnCash = "30";
            corporateActionEntry1.ImpactOnPosition = "None";
            corporateActionEntryList.Add(corporateActionEntry1);

            corporateActionEntry1 = new CorporateAction();
            corporateActionEntry1.Description = "Share Div";
            corporateActionEntry1.Details = "1 for 100";
            corporateActionEntry1.ImpactOnCash = "None";
            corporateActionEntry1.ImpactOnPosition = "10";
            corporateActionEntryList.Add(corporateActionEntry1);

            return corporateActionEntryList;
        }

        private void grdRecord_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdRecord.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdRecord.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdRecord.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdRecord.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdRecord.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdRecord.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdRecord.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdRecord.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdRecord.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colIsSelected = band.Columns[COL_IsSelected];
            colIsSelected.Hidden = true;

            UltraGridColumn colManualEntry = band.Columns[COL_ManualEntry];
            colManualEntry.Hidden = true;

            UltraGridColumn colStatus = band.Columns[COL_Status];
            colStatus.Hidden = true;


            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.VisiblePosition = 1;

            UltraGridColumn colFund = band.Columns[COL_Fund];
            colFund.Header.VisiblePosition = 2;

            UltraGridColumn colColumnName = band.Columns[COL_ColumnName];
            colColumnName.Header.Caption = "Column Name";
            colColumnName.Header.VisiblePosition = 3;

            UltraGridColumn colApplicationData = band.Columns[COL_ApplicationData];
            colApplicationData.Header.Caption = "Application Data";
            colApplicationData.Header.VisiblePosition = 4;

            UltraGridColumn colSourceData = band.Columns[COL_SourceData];
            colSourceData.Header.Caption = "Source Data";
            colSourceData.Header.VisiblePosition = 5;

            UltraGridColumn colManualEntryButton = band.Columns.Add(COL_ManualEntryButton);
            colManualEntryButton.Header.Caption = "Manual Entry";
            colManualEntryButton.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            colManualEntryButton.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colManualEntryButton.Header.VisiblePosition = 6;

        }

        private void grdTransaction_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdTransaction.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdTransaction.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdTransaction.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdTransaction.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdTransaction.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdTransaction.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdTransaction.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdTransaction.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdTransaction.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colSide = band.Columns[COL_Side];
            colSide.Header.VisiblePosition = 1;

            UltraGridColumn colExecQty = band.Columns[COL_ExecQty];
            colExecQty.Header.Caption = "Executed Qty";
            colExecQty.Header.VisiblePosition = 2;

            UltraGridColumn colAvgPrice = band.Columns[COL_AvgPrice];
            colAvgPrice.Header.Caption = "Average Price";
            colAvgPrice.Header.VisiblePosition = 3;

            UltraGridColumn colCV = band.Columns[COL_CV];
            colCV.Header.VisiblePosition = 4;

            UltraGridColumn colFund = band.Columns[COL_Fund];
            colFund.Header.VisiblePosition = 5;

            UltraGridColumn colNotional = band.Columns[COL_Notional];
            colNotional.Header.Caption = "Notional Value";
            colNotional.Header.VisiblePosition = 6;

            UltraGridColumn colCommissions = band.Columns[COL_Commissions];
            colCommissions.Header.VisiblePosition = 7;

            UltraGridColumn colFees = band.Columns[COL_Fees];
            colFees.Header.VisiblePosition = 8;

            UltraGridColumn colNetAmount = band.Columns[COL_NetAmount];
            colNetAmount.Header.Caption = "Net Amt";
            colNetAmount.Header.VisiblePosition = 9;

            UltraGridColumn colFills = band.Columns.Add(COL_Fills);
            colFills.Header.Caption = "Fills";
            colFills.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            colFills.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colFills.Header.VisiblePosition = 10;

        }

        private void grdCorporateActions_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdCorporateActions.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdCorporateActions.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdCorporateActions.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdCorporateActions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdCorporateActions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdCorporateActions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdCorporateActions.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdCorporateActions.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdCorporateActions.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colDescription = band.Columns[COL_Description];
            colDescription.Header.VisiblePosition = 1;

            UltraGridColumn colDetails = band.Columns[COL_Details];
            colDetails.Header.VisiblePosition = 2;

            UltraGridColumn colImpactOnCash = band.Columns[COL_ImpactOnCash];
            colImpactOnCash.Header.Caption = "Impact on cash";
            colImpactOnCash.Header.VisiblePosition = 3;

            UltraGridColumn colImpactOnPosition = band.Columns[COL_ImpactOnPosition];
            colImpactOnPosition.Header.Caption = "Impact on position";
            colImpactOnPosition.Header.VisiblePosition = 4;
        }

        

       

    }


}
