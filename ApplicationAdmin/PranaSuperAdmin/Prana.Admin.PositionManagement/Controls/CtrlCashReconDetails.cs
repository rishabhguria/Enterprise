using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;

using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlCashReconDetails : UserControl
    {

        #region Dividends, Trading Transaction and Other Transaction Grid

        const string COL_IsSelected = "IsSelected";
        const string COL_Fund = "Fund";
        const string COL_Symbol = "Symbol";
        const string COL_PositionQuantity = "PositionQuantity";
        const string COL_DividendAmount = "Amount";
        const string COL_Currency = "Currency";
        const string COL_DividendXDate = "DividendXDate";
        const string COL_ExpectedDividend = "ExpectedDividend"; 

        const string COL_TradingCashSpent = "TradingCashSpent";
        const string COL_TradingCashReceived = "TradingCashReceived";
        const string COL_NetAmount = "NetAmount";

        const string COL_CorporateActionType = "CorporateActionType";
        const string COL_Amount = "Amount";

        const string COL_TransactionDate = "TransactionDate";
        const string COL_CorporateActionNameID = "CorporateActionNameID";
        const string COL_DataSourceNameID = "DataSourceNameID";
        const string COL_Transactions = "Transactions";

        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        CashReconDetails _cashReconDetails = new CashReconDetails();
        //CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();

        public CtrlCashReconDetails()
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
        public void InitControl(DataSourceNameID dataSourceNameID)
        {
            if (!_isInitialized)
            {
                SetupBinding(dataSourceNameID);

                _isInitialized = true;
            }
        }

        private void SetupBinding(DataSourceNameID dataSourceNameID)
        {
            _formBindingSource.DataSource = RetrieveCashReconDetails(dataSourceNameID);

            cmbDate.DataBindings.Add("Value", _formBindingSource, "Date");
            lblDataSourceName.DataBindings.Add("Text", _formBindingSource, "DataSourceNameIDValue");

            //BindGridComboBox();
            grdDividends.DataMember = "DividendEntryListItems";
            grdDividends.DataSource = _formBindingSource;

            grdTradingTransactions.DataMember = "TradingCashTransactionListValue";
            grdTradingTransactions.DataSource = _formBindingSource;

            grdOtherTransactions.DataMember = "CorporateActionsList";
            grdOtherTransactions.DataSource = _formBindingSource;

            txtNetCashFlow.DataBindings.Add("Text", _formBindingSource, "NetCashFlow");
        }

        #endregion

        private CashReconDetails RetrieveCashReconDetails(DataSourceNameID dataSourceNameID)
        {
            _cashReconDetails.Date = Convert.ToDateTime(cmbDate.Value);
            _cashReconDetails.DataSourceNameIDValue = dataSourceNameID;
            _cashReconDetails.DividendEntryListItems = RetrieveDividendEntryListItems();
            _cashReconDetails.TradingCashTransactionListValue = RetrieveTradingCashTransactionListValue();
            _cashReconDetails.CorporateActionsList = RetrieveOtherTransactionList();
            _cashReconDetails.NetCashFlow = 1500000;

            return _cashReconDetails;
        }

        private SortableSearchableList<DividendEntry> RetrieveDividendEntryListItems()
        {
            SortableSearchableList<DividendEntry> dividendEntryList = new SortableSearchableList<DividendEntry>();
            DividendEntry dividendEntry = new DividendEntry();
            dividendEntry.IsSelected = true;
            dividendEntry.Fund.Name = "Long";
            dividendEntry.Symbol = "Dell";
            dividendEntry.PositionQuantity = 10000;
            dividendEntry.Amount = 0.02;
            dividendEntry.Currency.Name = "USD";
            dividendEntry.DividendXDate = new DateTime(2005, 12, 1);
            dividendEntry.ExpectedDividend = 200;

            dividendEntryList.Add(dividendEntry);
            return dividendEntryList;
        }

        private CashTransactionList RetrieveTradingCashTransactionListValue()
        {
            CashTransactionList cashTransactionList = new CashTransactionList();
            CashBalanceEntry cashBalanceEntry = new CashBalanceEntry();
            cashBalanceEntry.IsSelected = true; 
            cashBalanceEntry.Fund.Name = "Long";
            cashBalanceEntry.Symbol = "Dell";
            cashBalanceEntry.Currency.Name = "USD";
            cashBalanceEntry.TradingCashSpent = 100000;
            cashBalanceEntry.TradingCashReceived = 20000;
            cashBalanceEntry.NetAmount = cashBalanceEntry.TradingCashReceived - cashBalanceEntry.TradingCashSpent;

            cashTransactionList.Add(cashBalanceEntry);
            return cashTransactionList;
        }

        private SortableSearchableList<CorporateActionEntry> RetrieveOtherTransactionList()
        {
            SortableSearchableList<CorporateActionEntry> cashTransactionList = new SortableSearchableList<CorporateActionEntry>();
            CorporateActionEntry corporateActionEntry = new CorporateActionEntry();
            corporateActionEntry.IsSelected = true;
            corporateActionEntry.Fund.Name = "long";
            corporateActionEntry.Symbol = "Dell";
            corporateActionEntry.CorporateActionType.Name = "Interest Received";
            corporateActionEntry.Currency.Name = "USD";
            corporateActionEntry.Amount = 1200;

            cashTransactionList.Add(corporateActionEntry);
            return cashTransactionList;
        }

        private void grdDividends_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            grdDividends.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdDividends.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdDividends.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdDividends.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdDividends.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdDividends.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdDividends.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdDividends.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdDividends.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colTransactionDate = band.Columns[COL_TransactionDate];
            colTransactionDate.Hidden = true;

            UltraGridColumn colCorporateActionNameID = band.Columns[COL_CorporateActionNameID];
            colCorporateActionNameID.Hidden = true;

            UltraGridColumn colCorporateActionType = band.Columns[COL_CorporateActionType];
            colCorporateActionType.Hidden = true;

            UltraGridColumn colDataSourceNameID = band.Columns[COL_DataSourceNameID];
            colDataSourceNameID.Hidden = true;

            UltraGridColumn colIsSelected = band.Columns[COL_IsSelected];
            colIsSelected.Header.Caption = "";
            colIsSelected.Header.VisiblePosition = 1;

            UltraGridColumn colFund = band.Columns[COL_Fund];
            colIsSelected.Header.Caption = "Fund";
            colIsSelected.Header.VisiblePosition = 2;

            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 3;

            UltraGridColumn colPositionQuantity = band.Columns[COL_PositionQuantity];
            colPositionQuantity.Header.Caption = "Position";
            colPositionQuantity.Header.VisiblePosition = 4;

            UltraGridColumn colDividendAmount = band.Columns[COL_DividendAmount];
            colDividendAmount.Header.Caption = "Dividend Amt";
            colDividendAmount.Header.VisiblePosition = 5;

            UltraGridColumn colCurrency = band.Columns[COL_Currency];
            colCurrency.Header.Caption = "Dividend Currency";
            colCurrency.Header.VisiblePosition = 6;

            UltraGridColumn colDividendXDate = band.Columns[COL_DividendXDate];
            colDividendXDate.Header.Caption = "Dividend X Date";
            colDividendXDate.Header.VisiblePosition = 7;

            UltraGridColumn colExpectedDividend = band.Columns[COL_ExpectedDividend];
            colExpectedDividend.Header.Caption = "Expected Dividend";
            colExpectedDividend.Header.VisiblePosition = 8;
        }

        private void grdTradingTransactions_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            grdTradingTransactions.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdTradingTransactions.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdTradingTransactions.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdTradingTransactions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdTradingTransactions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdTradingTransactions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdTradingTransactions.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdTradingTransactions.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdTradingTransactions.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colTransactions = band.Columns[COL_Transactions];
            colTransactions.Hidden = true;

            UltraGridColumn colDataSourceNameID = band.Columns[COL_DataSourceNameID];
            colDataSourceNameID.Hidden = true;

            UltraGridColumn colIsSelected = band.Columns[COL_IsSelected];
            colIsSelected.Header.Caption = "";
            colIsSelected.Header.VisiblePosition = 1;

            UltraGridColumn colFund = band.Columns[COL_Fund];
            colIsSelected.Header.Caption = "Fund";
            colIsSelected.Header.VisiblePosition = 2;

            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 3;

            UltraGridColumn colCurrency = band.Columns[COL_Currency];
            colCurrency.Header.Caption = "Currency";
            colCurrency.Header.VisiblePosition = 4;

            UltraGridColumn colTradingCashSpent = band.Columns[COL_TradingCashSpent];
            colTradingCashSpent.Header.Caption = "Trading Cash Spent";
            colTradingCashSpent.Header.VisiblePosition = 5;

            UltraGridColumn colTradingCashReceived = band.Columns[COL_TradingCashReceived];
            colTradingCashReceived.Header.Caption = "Trading-Cash received";
            colTradingCashReceived.Header.VisiblePosition = 6;

            UltraGridColumn colNetAmount = band.Columns[COL_NetAmount];
            colNetAmount.Header.Caption = "Net Amt";
            colNetAmount.Header.VisiblePosition = 7;
        }

        private void grdOtherTransactions_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            grdOtherTransactions.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdOtherTransactions.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdOtherTransactions.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdOtherTransactions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdOtherTransactions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdOtherTransactions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdOtherTransactions.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdOtherTransactions.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdOtherTransactions.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colTransactionDate = band.Columns[COL_TransactionDate];
            colTransactionDate.Hidden = true;

            UltraGridColumn colCorporateActionNameID = band.Columns[COL_CorporateActionNameID];
            colCorporateActionNameID.Hidden = true;

            UltraGridColumn colDataSourceNameID = band.Columns[COL_DataSourceNameID];
            colDataSourceNameID.Hidden = true;

            UltraGridColumn colIsSelected = band.Columns[COL_IsSelected];
            colIsSelected.Header.Caption = "";
            colIsSelected.Header.VisiblePosition = 1;

            UltraGridColumn colFund = band.Columns[COL_Fund];
            colIsSelected.Header.Caption = "Fund";
            colIsSelected.Header.VisiblePosition = 2;

            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 3;

            UltraGridColumn colTransactionName = band.Columns[COL_CorporateActionType];
            colTransactionName.Header.Caption = "Transaction Name";
            colTransactionName.Header.VisiblePosition = 4;

            UltraGridColumn colCurrency = band.Columns[COL_Currency];
            colCurrency.Header.Caption = "Currency";
            colCurrency.Header.VisiblePosition = 5;

            UltraGridColumn colAmount = band.Columns[COL_Amount];
            colAmount.Header.Caption = "colAmount";
            colAmount.Header.VisiblePosition = 6;
        }

        private void cmbDate_ValueChanged(object sender, EventArgs e)
        {
            _cashReconDetails.Date = Convert.ToDateTime(cmbDate.Value);
        }

        private void btnMakeManualEntry_Click(object sender, EventArgs e)
        {
            Forms.CashReconManualEntry frmCashReconManualEntry = new Nirvana.Admin.PositionManagement.Forms.CashReconManualEntry();
            frmCashReconManualEntry.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the Container Form
            this.FindForm().Close();
        }

      

        
    }
}
