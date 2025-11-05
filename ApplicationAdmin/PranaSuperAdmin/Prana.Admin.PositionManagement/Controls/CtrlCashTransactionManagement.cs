using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;

using Infragistics.Win.UltraWinGrid;
using Nirvana.Admin.PositionManagement.Forms;


namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlCashTransactionManagement : UserControl
    {
        #region Grid Column Names

        const string COL_IsSelected = "IsSelected";
        const string COL_Source = "DataSourceNameID";
        const string COL_Fund = "Fund";
        const string COL_Symbol = "Symbol";
        const string COL_PositionQuantity = "PositionQuantity";
        const string COL_Currency = "Currency";
        const string COL_Amount = "Amount";
        const string COL_TransactionName = "CorporateActionType";
        const string COL_DividendXDate = "DividendXDate";
        const string COL_ExpectedDividend = "ExpectedDividend";
        const string COL_PaymentDate = "PaymentDate";
        const string COL_TransactionDate = "TransactionDate";
        const string COL_ImpactOnCash = "ImpactOnCash";
        
        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        private CashTransactionManagement _cashTransactionManagement = new CashTransactionManagement();
        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();
        
        public CtrlCashTransactionManagement()
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
        public void InitControl()
        {
            if (!_isInitialized)
            {
                ctrlSourceName1.IsSelectItemRequired = false;
                ctrlSourceName1.IsAllDataSourceAvailable = true;
                ctrlSourceName1.InitControl();

                SetupBinding();
                _isInitialized = true;
            }
        }

        #endregion

        /// <summary>
        /// Setups the binding.
        /// </summary>
        /// <param name="dataSourceNameID">The data source name ID.</param>
        private void SetupBinding()
        {
            //_formBindingSource.DataSource = RetrieveCashTransactionManagement(dataSourceNameID, DateTime.Now);
            _cashTransactionManagement.Date = DateTime.Today;
            _formBindingSource.DataSource = _cashTransactionManagement;
            cmbDate.DataBindings.Add("Value", _formBindingSource, "Date");

            grdDividends.DataMember = "DividendEntryListItems";
            grdDividends.DataSource = _formBindingSource;

            grdOtherTransactions.DataMember = "CorporateActionEntryListItems";
            grdOtherTransactions.DataSource = _formBindingSource;

        }

        public static SortableSearchableList<DividendEntry> RetrieveDividendEntries(DataSourceNameID dataSourceNameID, DateTime date)
        {
            SortableSearchableList<DividendEntry> _dividendEntryList = new SortableSearchableList<DividendEntry>();
            TimeSpan ts = new TimeSpan(1, 0, 0, 0, 0);            
            DateTime previousdate = DateTime.Today.Subtract(ts);
            
            if (date.Equals(DateTime.Today))
            {

                if (int.Equals(dataSourceNameID.ID, 0) || int.Equals(dataSourceNameID.ID, 1))
                {
                    DividendEntry item = new DividendEntry();
                    item.CorporateActionNameID.ID = 1;
                    item.CorporateActionNameID.Name = "First Quarter Dividend";
                    item.CorporateActionType.ID = 1;
                    item.CorporateActionType.Name = "Dividend";
                    item.Amount = 1000;
                    item.DividendXDate = new DateTime(2005, 10, 30);
                    item.ExpectedDividend = 2000;
                    item.Fund.ID = 1;
                    item.Fund.Name = "Fund X";
                    item.DataSourceNameID = dataSourceNameID;
                    item.ImpactOnCash = ImpactOnCash.Positive;
                    //item.Symbol.ID = 1;
                    item.Symbol = "MSFT";
                    item.TransactionDate = new DateTime(2006, 1, 1);

                    _dividendEntryList.Add(item);

                    item = new DividendEntry();
                    item.CorporateActionNameID.ID = 2;
                    item.CorporateActionNameID.Name = "Second Quarter Dividend";
                    item.CorporateActionType.ID = 1;
                    item.CorporateActionType.Name = "Dividend";
                    item.Amount = 1000;
                    item.DividendXDate = new DateTime(2006, 1, 30);
                    item.ExpectedDividend = 2000;
                    item.Fund.ID = 1;
                    item.Fund.Name = "Fund X";
                    item.DataSourceNameID = dataSourceNameID;
                    item.ImpactOnCash = ImpactOnCash.Positive;
                    //item.Symbol.ID = 1;
                    item.Symbol = "MSFT";
                    item.TransactionDate = new DateTime(2006, 1, 1);

                    _dividendEntryList.Add(item);

                } 
            }
            if (date.Equals(previousdate))
            {

                if (int.Equals(dataSourceNameID.ID, 0) || int.Equals(dataSourceNameID.ID, 1))
                {
                    DividendEntry item = new DividendEntry();
                    item.CorporateActionNameID.ID = 1;
                    item.CorporateActionNameID.Name = "Half Yearly Dividend";
                    item.CorporateActionType.ID = 1;
                    item.CorporateActionType.Name = "Dividend";
                    item.Amount = 13000;
                    item.DividendXDate = new DateTime(2005, 10, 30);
                    item.ExpectedDividend = 30000;
                    item.Fund.ID = 1;
                    item.Fund.Name = "Fund Y";
                    item.DataSourceNameID = dataSourceNameID;
                    item.ImpactOnCash = ImpactOnCash.Positive;
                    //item.Symbol.ID = 2;
                    item.Symbol = "GOOG";
                    item.TransactionDate = new DateTime(2006, 1, 1);

                    _dividendEntryList.Add(item);

                }
            }
            return _dividendEntryList;
        }

        public static SortableSearchableList<CorporateActionEntry> RetrieveCorporateActionEntries(DataSourceNameID dataSourceNameID, DateTime date)
        {
            SortableSearchableList<CorporateActionEntry> _corporateActionEntryList = new SortableSearchableList<CorporateActionEntry>();
            TimeSpan ts = new TimeSpan(1, 0, 0, 0, 0);            
            DateTime previousdate = DateTime.Today.Subtract(ts);
           
            if (date.Equals(DateTime.Today))
            {
                //for All or GS
                if (int.Equals(dataSourceNameID.ID, 0) || int.Equals(dataSourceNameID.ID, 1))
                {
                    CorporateActionEntry item = new CorporateActionEntry();
                    item.CorporateActionNameID.ID = 1;
                    item.CorporateActionNameID.Name = "First Quarter Intrest";
                    item.CorporateActionType.ID = 1;
                    item.CorporateActionType.Name = "Intrest";
                    item.Amount = 1000;
                    item.Fund.ID = 1;
                    item.Fund.Name = "Fund X";
                    item.DataSourceNameID = dataSourceNameID;
                    item.ImpactOnCash = ImpactOnCash.Positive;
                    item.TransactionDate = new DateTime(2006, 1, 1);
                    item.PaymentDate = new DateTime(2006, 1, 10);
                    item.Currency.ID = 1;
                    item.Currency.Name = "USD";

                    _corporateActionEntryList.Add(item);

                    item = new DividendEntry();
                    item.CorporateActionNameID.ID = 2;
                    item.CorporateActionNameID.Name = "Second Quarter Intrest";
                    item.CorporateActionType.ID = 1;
                    item.CorporateActionType.Name = "Intrest";
                    item.Amount = 1000;
                    item.Fund.ID = 1;
                    item.Fund.Name = "Fund X";
                    item.DataSourceNameID = dataSourceNameID;
                    item.ImpactOnCash = ImpactOnCash.Positive;
                    item.TransactionDate = new DateTime(2006, 1, 1);
                    item.PaymentDate = new DateTime(2006, 1, 10);
                    item.Currency.ID = 1;
                    item.Currency.Name = "USD";


                    _corporateActionEntryList.Add(item);

                }
                
            }

            if (date.Equals(previousdate))
            {
                //for All or GS
                if (int.Equals(dataSourceNameID.ID, 0) || int.Equals(dataSourceNameID.ID, 1))
                {
                    CorporateActionEntry item = new CorporateActionEntry();
                    item.CorporateActionNameID.ID = 1;
                    item.CorporateActionNameID.Name = "Half Yearly Intrest";
                    item.CorporateActionType.ID = 1;
                    item.CorporateActionType.Name = "Intrest";
                    item.Amount = 1000;
                    item.Fund.ID = 1;
                    item.Fund.Name = "Fund Y";
                    item.DataSourceNameID = dataSourceNameID;
                    item.ImpactOnCash = ImpactOnCash.Positive ;
                    item.TransactionDate = new DateTime(2006, 1, 1);
                    item.PaymentDate = new DateTime(2006, 1, 10);
                    item.Currency.ID = 1;
                    item.Currency.Name = "USD";

                    _corporateActionEntryList.Add(item);

                }
            }
            return _corporateActionEntryList;
        }

        void ctrlSourceName1_SelectionChanged(object sender, EventArgs e)
        {
            _cashTransactionManagement.DataSourceNameID = ((DataSourceEventArgs)e).DataSourceNameID;
        }

        private void btnEnterTransactions_Click(object sender, EventArgs e)
        {
            Forms.EnterTransactions frmEnterTransactions = new EnterTransactions();
            frmEnterTransactions.ShowDialog();

        }

        private void btnEnterDividends_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This will open an interface of Security Master to let the user to enter Dividends","Notification");
        }

        private void grdDividends_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdDividends.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdDividends.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdDividends.CreationFilter = headerCheckBox;
            grdDividends.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdDividends.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdDividends.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdDividends.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdDividends.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdDividends.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdDividends.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
            ///of the property.

            UltraGridColumn colIsSelected = band.Columns[COL_IsSelected];
            colIsSelected.Header.Caption = "";
            colIsSelected.Header.VisiblePosition = 1;

            UltraGridColumn colSource = band.Columns[COL_Source];
            colSource.Header.Caption = "Source";
            colSource.Header.VisiblePosition = 2;

            UltraGridColumn colFund = band.Columns[COL_Fund];
            colFund.Header.Caption = "Fund";
            colFund.Header.VisiblePosition = 3;

            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 4;

            UltraGridColumn colPositionQuantity = band.Columns[COL_PositionQuantity];
            colPositionQuantity.Header.Caption = "Position Quantity";
            colPositionQuantity.Header.VisiblePosition = 5;

            UltraGridColumn colAmount = band.Columns[COL_Amount];
            colAmount.Header.Caption = "Amount";
            colAmount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
            colAmount.Header.VisiblePosition = 6;

            UltraGridColumn colCurrency = band.Columns[COL_Currency];
            colCurrency.Header.Caption = "Currency";
            colCurrency.Header.VisiblePosition = 7;

            UltraGridColumn colDividendXDate = band.Columns[COL_DividendXDate];
            colDividendXDate.Header.Caption = "Dividend X Date";
            colDividendXDate.Header.VisiblePosition = 8;

            UltraGridColumn colExpectedDividend = band.Columns[COL_ExpectedDividend];
            colExpectedDividend.Header.Caption = "Expected Dividend";
            colExpectedDividend.Header.VisiblePosition = 9;

            UltraGridColumn colTransactionDate = band.Columns[COL_TransactionDate];
            colTransactionDate.Hidden = true;

            UltraGridColumn colPaymentDate = band.Columns[COL_PaymentDate];
            colPaymentDate.Hidden = true;

            UltraGridColumn colImpactOnCash = band.Columns[COL_ImpactOnCash];
            colImpactOnCash.Hidden = true;

            //UltraGridColumn colApplicationItemName = band.Columns[COL_ApplicationItemName];
            //colApplicationItemName.Header.Caption = "Applicatin Funds";
            //colApplicationItemName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            //colApplicationItemName.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            //colApplicationItemName.ValueList = cmbApplicationColumn;
            //colApplicationItemName.Header.VisiblePosition = 2;

            //UltraGridColumn colLock = band.Columns[COL_Lock];
            //colLock.Hidden = true;
        }

        private void grdOtherTransactions_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdOtherTransactions.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdOtherTransactions.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdOtherTransactions.CreationFilter = headerCheckBox;
            grdOtherTransactions.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdOtherTransactions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdOtherTransactions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdOtherTransactions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdOtherTransactions.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdOtherTransactions.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdOtherTransactions.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colIsSelected = band.Columns[COL_IsSelected];
            colIsSelected.Header.Caption = "";
            colIsSelected.Header.VisiblePosition = 1;

            UltraGridColumn colSource = band.Columns[COL_Source];
            colSource.Header.Caption = "Source";
            colSource.Header.VisiblePosition = 2;

            UltraGridColumn colFund = band.Columns[COL_Fund];
            colFund.Header.Caption = "Fund";
            colFund.Header.VisiblePosition = 3;

            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 4;

            UltraGridColumn colTransactionName = band.Columns[COL_TransactionName];
            colTransactionName.Header.Caption = "Transaction Name";
            colTransactionName.Header.VisiblePosition = 5;

            UltraGridColumn colAmount = band.Columns[COL_Amount];
            colAmount.Header.Caption = "Amount";
            colAmount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
            colAmount.Header.VisiblePosition = 6;

            UltraGridColumn colCurrency = band.Columns[COL_Currency];
            colCurrency.Header.Caption = "Currency";
            colCurrency.Header.VisiblePosition = 7;

            UltraGridColumn colTransactionDate = band.Columns[COL_TransactionDate];
            colTransactionDate.Hidden = true;

            UltraGridColumn colPaymentDate = band.Columns[COL_PaymentDate];
            colPaymentDate.Hidden = true;

            UltraGridColumn colImpactOnCash = band.Columns[COL_ImpactOnCash];
            colImpactOnCash.Hidden = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            OnUpdate();
        }
        private void OnUpdate()
        {
            if (!_cashTransactionManagement.DataSourceNameID.FullName.Equals(Constants.C_COMBO_SELECT))
            {
                _cashTransactionManagement.DividendEntryListItems = RetrieveDividendEntries(_cashTransactionManagement.DataSourceNameID, _cashTransactionManagement.Date);
                _cashTransactionManagement.CorporateActionEntryListItems = RetrieveCorporateActionEntries(_cashTransactionManagement.DataSourceNameID, _cashTransactionManagement.Date);
                _formBindingSource.ResetBindings(false);
            }
        }

        private void cmbDate_ValueChanged(object sender, EventArgs e)
        {
            _cashTransactionManagement.Date = Convert.ToDateTime(cmbDate.Value);

            TimeSpan ts = new TimeSpan(1, 0, 0, 0, 0);
            DateTime previousdate = DateTime.Today.Subtract(ts);

            if (DateTime.Today.Equals(previousdate))
            {
                OnUpdate();
            }
        }
    }
}
