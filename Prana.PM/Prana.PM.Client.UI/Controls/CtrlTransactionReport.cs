//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Text;
//using System.Windows.Forms;

//using Infragistics.Win.UltraWinGrid;
//using Prana.PM.BLL;
//using Prana.BusinessObjects.PositionManagement;

//namespace Prana.PM.Client.UI.Controls
//{
//    public partial class CtrlTransactionReport : UserControl
//    {
//        #region Grid Column Names

//        #region Record Grid
//        const string CAPTION_IsSelected = "IsSelected";
//        const string CAPTION_Symbol = "Symbol";
//        const string CAPTION_Account = "Account";
//        const string CAPTION_ColumnName = "ColumnName";
//        const string CAPTION_ApplicationData = "ApplicationData";
//        const string CAPTION_SourceData = "SourceData";
//        const string CAPTION_ManualEntry = "ManualEntry";
//        const string CAPTION_Status = "Status";
//        const string CAPTION_ManualEntryButton = "ManualEntryButton"; 
//        #endregion

//        #region Trade Transaction Column
//        const string CAPTION_Side = "Side";
//        const string CAPTION_ExecQty = "ExecQty";
//        const string CAPTION_AvgPrice = "AvgPrice";
//        const string CAPTION_CV = "CV";
//        const string CAPTION_Notional = "Notional";
//        const string CAPTION_Commissions = "Commissions";
//        const string CAPTION_Fees = "Fees";
//        const string CAPTION_NetAmount = "NetAmount";
//        const string CAPTION_Fills = "Fills";
//        #endregion

//        #region Corporate Action Entry Column
//        const string CAPTION_Description = "Description";
//        const string CAPTION_Details = "Details";
//        const string CAPTION_ImpactOnCash = "ImpactOnCash";
//        const string CAPTION_ImpactOnPosition = "ImpactOnPosition"; 
//        #endregion


//        #endregion Grid Column Names

//        BindingSource _formBindingSource = new BindingSource();
//        TransactionReportSummary _transactionReportSummary = new TransactionReportSummary();

//        public CtrlTransactionReport()
//        {
//            InitializeComponent();
//        }

//        #region Initialize the control
//        private bool _isInitialized = false;

//        /// <summary>
//        /// Gets or sets a value indicating whether this instance is initialized.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
//        /// </value>
//        public bool IsInitialized
//        {
//            get { return _isInitialized; }
//            set { _isInitialized = value; }
//        }


//        /// <summary>
//        /// Initialize the control.
//        /// </summary>
//        public void InitControl(DataSourceNameID dataSource)
//        {
//            if (!_isInitialized)
//            {
//                SetupBinding(dataSource);
//                _isInitialized = true;
//            }
//        }

//        #endregion

//        private void SetupBinding(DataSourceNameID dataSource)
//        {
//            _formBindingSource.DataSource = RetrieveTransactionReportSummary(dataSource);

//            lblDataSource.DataBindings.Add("Text", _formBindingSource, "DataSourceNameIDValue");

//            grdRecord.DataMember = "ExceptionReportEntry";
//            grdRecord.DataSource = _formBindingSource;

//            clrTransactionDate.DataBindings.Add("Value", _formBindingSource, "TransactionDate");

//            grdTransaction.DataMember = "TradeTransactionList";
//            grdTransaction.DataSource = null;
//            grdTransaction.DataSource = _formBindingSource;

//            clrCorporateActions.DataBindings.Add("Value", _formBindingSource, "CorporateActionDate");

//            grdCorporateActions.DataMember = "CorporateActionEntryList";
//            grdCorporateActions.DataSource = null;
//            grdCorporateActions.DataSource = _formBindingSource;

//        }

//        private TransactionReportSummary RetrieveTransactionReportSummary(DataSourceNameID dataSource)
//        {
//            _transactionReportSummary.DataSourceNameIDValue = dataSource;
//            _transactionReportSummary.ExceptionReportEntry = RetrieveExceptionReportEntry();
//            _transactionReportSummary.TransactionDate = DateTime.UtcNow;
//            _transactionReportSummary.TradeTransactionList = RetrieveTradeTransactionList();
//            _transactionReportSummary.CorporateActionDate = DateTime.UtcNow;
//            _transactionReportSummary.CorporateActionEntryList = RetrieveCorporateActionEntryList();

//            return _transactionReportSummary;
//        }

//        private ExceptionReportEntry RetrieveExceptionReportEntry()
//        {
//            ExceptionReportEntry exceptionReportEntry = new ExceptionReportEntry();
//            exceptionReportEntry.Symbol = "Dell";
//            exceptionReportEntry.Account = "Long";
//            exceptionReportEntry.ColumnName = "Position";
//            exceptionReportEntry.ApplicationData = "1000";
//            exceptionReportEntry.SourceData = "1200";

//            return exceptionReportEntry;
//        }

//        private BindingList<TradeTransaction> RetrieveTradeTransactionList()
//        {
//            BindingList<TradeTransaction> tradeTransactionList = new BindingList<TradeTransaction>();

//            TradeTransaction tradeTransaction1 = new TradeTransaction();
//            tradeTransaction1.Side = "B";
//            tradeTransaction1.ExecQty = 1000;
//            tradeTransaction1.AvgPrice = 25.00F;
//            tradeTransaction1.CV = "Morgan-Arca";
//            tradeTransaction1.Account = "Long";
//            tradeTransaction1.Notional = 2500.00;
//            tradeTransaction1.Commissions = 20.00F;
//            tradeTransaction1.Fees = 1.00F;
//            tradeTransaction1.NetAmount = 2521.00;
//            tradeTransactionList.Add(tradeTransaction1);

//            tradeTransaction1 = new TradeTransaction();
//            tradeTransaction1.Side = "S";
//            tradeTransaction1.ExecQty = 1000;
//            tradeTransaction1.AvgPrice = 24.00F;
//            tradeTransaction1.CV = "GS-Desk";
//            tradeTransaction1.Account = "Long";
//            tradeTransaction1.Notional = 2400.00;
//            tradeTransaction1.Commissions = 19.00F;
//            tradeTransaction1.NetAmount = 2381.00;
//            tradeTransactionList.Add(tradeTransaction1);

//            return tradeTransactionList;
//        }


//        private BindingList<CorporateAction> RetrieveCorporateActionEntryList()
//        {
//            BindingList<CorporateAction> corporateActionEntryList = new BindingList<CorporateAction>();

//            CorporateAction corporateActionEntry1 = new CorporateAction();
//            corporateActionEntry1.Description = "Dividend";
//            corporateActionEntry1.Details = "3 cents a share";
//            corporateActionEntry1.ImpactOnCash = "30";
//            corporateActionEntry1.ImpactOnPosition = "None";
//            corporateActionEntryList.Add(corporateActionEntry1);

//            corporateActionEntry1 = new CorporateAction();
//            corporateActionEntry1.Description = "Share Div";
//            corporateActionEntry1.Details = "1 for 100";
//            corporateActionEntry1.ImpactOnCash = "None";
//            corporateActionEntry1.ImpactOnPosition = "10";
//            corporateActionEntryList.Add(corporateActionEntry1);

//            return corporateActionEntryList;
//        }

//        private void grdRecord_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
//        {
//            UltraGridBand band = e.Layout.Bands[0];
//            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

//            grdRecord.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
//            grdRecord.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
//            grdRecord.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
//            grdRecord.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
//            grdRecord.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
//            grdRecord.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
//            grdRecord.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
//            grdRecord.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
//            grdRecord.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

//            UltraGridColumn colIsSelected = band.Columns[CAPTION_IsSelected];
//            colIsSelected.Hidden = true;

//            UltraGridColumn colManualEntry = band.Columns[CAPTION_ManualEntry];
//            colManualEntry.Hidden = true;

//            UltraGridColumn colStatus = band.Columns[CAPTION_Status];
//            colStatus.Hidden = true;


//            UltraGridColumn colSymbol = band.Columns[CAPTION_Symbol];
//            colSymbol.Header.VisiblePosition = 1;

//            UltraGridColumn colAccount = band.Columns[CAPTION_Account];
//            colAccount.Header.VisiblePosition = 2;

//            UltraGridColumn colColumnName = band.Columns[CAPTION_ColumnName];
//            colColumnName.Header.Caption = "Column Name";
//            colColumnName.Header.VisiblePosition = 3;

//            UltraGridColumn colApplicationData = band.Columns[CAPTION_ApplicationData];
//            colApplicationData.Header.Caption = "Application Data";
//            colApplicationData.Header.VisiblePosition = 4;

//            UltraGridColumn colSourceData = band.Columns[CAPTION_SourceData];
//            colSourceData.Header.Caption = "Source Data";
//            colSourceData.Header.VisiblePosition = 5;

//            UltraGridColumn colManualEntryButton = band.Columns.Add(CAPTION_ManualEntryButton);
//            colManualEntryButton.Header.Caption = "Manual Entry";
//            colManualEntryButton.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
//            colManualEntryButton.ButtonDisplayStyle = ButtonDisplayStyle.Always;
//            colManualEntryButton.Header.VisiblePosition = 6;

//        }

//        private void grdTransaction_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
//        {
//            UltraGridBand band = e.Layout.Bands[0];
//            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

//            grdTransaction.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
//            grdTransaction.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
//            grdTransaction.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
//            grdTransaction.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
//            grdTransaction.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
//            grdTransaction.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
//            grdTransaction.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
//            grdTransaction.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
//            grdTransaction.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

//            UltraGridColumn colSide = band.Columns[CAPTION_Side];
//            colSide.Header.VisiblePosition = 1;

//            UltraGridColumn colExecQty = band.Columns[CAPTION_ExecQty];
//            colExecQty.Header.Caption = "Executed Qty";
//            colExecQty.Header.VisiblePosition = 2;

//            UltraGridColumn colAvgPrice = band.Columns[CAPTION_AvgPrice];
//            colAvgPrice.Header.Caption = "Average Price";
//            colAvgPrice.Header.VisiblePosition = 3;

//            UltraGridColumn colCV = band.Columns[CAPTION_CV];
//            colCV.Header.VisiblePosition = 4;

//            UltraGridColumn colAccount = band.Columns[CAPTION_Account];
//            colAccount.Header.VisiblePosition = 5;

//            UltraGridColumn colNotional = band.Columns[CAPTION_Notional];
//            colNotional.Header.Caption = "Notional Value";
//            colNotional.Header.VisiblePosition = 6;

//            UltraGridColumn colCommissions = band.Columns[CAPTION_Commissions];
//            colCommissions.Header.VisiblePosition = 7;

//            UltraGridColumn colFees = band.Columns[CAPTION_Fees];
//            colFees.Header.VisiblePosition = 8;

//            UltraGridColumn colNetAmount = band.Columns[CAPTION_NetAmount];
//            colNetAmount.Header.Caption = "Net Amt";
//            colNetAmount.Header.VisiblePosition = 9;

//            UltraGridColumn colFills = band.Columns.Add(CAPTION_Fills);
//            colFills.Header.Caption = "Fills";
//            colFills.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
//            colFills.ButtonDisplayStyle = ButtonDisplayStyle.Always;
//            colFills.Header.VisiblePosition = 10;

//        }

//        private void grdCorporateActions_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
//        {
//            UltraGridBand band = e.Layout.Bands[0];
//            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

//            grdCorporateActions.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
//            grdCorporateActions.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
//            grdCorporateActions.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
//            grdCorporateActions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
//            grdCorporateActions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
//            grdCorporateActions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
//            grdCorporateActions.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
//            grdCorporateActions.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
//            grdCorporateActions.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

//            UltraGridColumn colDescription = band.Columns[CAPTION_Description];
//            colDescription.Header.VisiblePosition = 1;

//            UltraGridColumn colDetails = band.Columns[CAPTION_Details];
//            colDetails.Header.VisiblePosition = 2;

//            UltraGridColumn colImpactOnCash = band.Columns[CAPTION_ImpactOnCash];
//            colImpactOnCash.Header.Caption = "Impact on cash";
//            colImpactOnCash.Header.VisiblePosition = 3;

//            UltraGridColumn colImpactOnPosition = band.Columns[CAPTION_ImpactOnPosition];
//            colImpactOnPosition.Header.Caption = "Impact on position";
//            colImpactOnPosition.Header.VisiblePosition = 4;
//        }





//    }


//}
