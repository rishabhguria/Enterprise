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


namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlEnterCashTransactions : UserControl
    {
        #region Grid Column Names

        const string COL_IsSelected = "IsSelected";
        const string COL_Source = "DataSourceNameID";
        const string COL_Fund = "Fund";
        const string COL_Symbol = "Symbol";
        const string COL_PositionQuantity = "PositionQuantity";
        const string COL_Currency = "Currency";
        const string COL_Amount = "Amount";
        const string COL_TransactionType = "CorporateActionType";
        const string COL_TransactionDate = "TransactionDate";
        const string COL_PaymentDate = "PaymentDate";
        const string COL_DividendXDate = "DividendXDate";
        const string COL_ExpectedDividend = "ExpectedDividend";
        const string COL_ImpactOnCash = "ImpactOnCash";

        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        private CashTransaction _cashTransaction = new CashTransaction();
        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();


        public CtrlEnterCashTransactions()
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
            _cashTransaction.CorporateActionEntryList = RetrieveCorporateActionEntries();
            _formBindingSource.DataSource = _cashTransaction;

            BindGridComboBox();

            grdEnterTransactions.DataMember = "CorporateActionEntryList";
            grdEnterTransactions.DataSource = _formBindingSource;

        }

        private void BindGridComboBox()
        {
            cmbImpactOnCash.DisplayMember = "DisplayText";
            cmbImpactOnCash.ValueMember = "Value";
            cmbImpactOnCash.DataSource = EnumHelper.ConvertEnumForBinding(typeof(ImpactOnCash));
            Utils.UltraDropDownFilter(cmbImpactOnCash, "DisplayText");

            cmbCurrency.DisplayMember = "DisplayText";
            cmbCurrency.ValueMember = "Value";
            cmbCurrency.DataSource = GetCurrencies();
            Utils.UltraDropDownFilter(cmbCurrency, "DisplayText");

            cmbFunds.DisplayMember = "DisplayText";
            cmbFunds.ValueMember = "Value";
            cmbFunds.DataSource = GetFunds();
            Utils.UltraDropDownFilter(cmbFunds, "DisplayText");

            cmbDataSources.DisplayMember = "DisplayText";
            cmbDataSources.ValueMember = "Value";
            cmbDataSources.DataSource = GetDataSources();
            Utils.UltraDropDownFilter(cmbDataSources, "DisplayText");

            cmbTransactionType.DisplayMember = "DisplayText";
            cmbTransactionType.ValueMember = "Value";
            cmbTransactionType.DataSource = GetTransactionTypes();
            Utils.UltraDropDownFilter(cmbTransactionType, "DisplayText");

        }

        private List<EnumerationValue> GetDataSources()
        {
            List<EnumerationValue> datasources = new List<EnumerationValue>();

            datasources.Add(new EnumerationValue("GS", 1));
            datasources.Add(new EnumerationValue("ML", 30));
            datasources.Add(new EnumerationValue("MS", 35));
            
            return datasources;
        }

        private List<EnumerationValue> GetTransactionTypes()
        {
            List<EnumerationValue> transactionTypes = new List<EnumerationValue>();

            transactionTypes.Add(new EnumerationValue("<Add New>", 0));
            transactionTypes.Add(new EnumerationValue("Intrest Paid", 1));
            transactionTypes.Add(new EnumerationValue("Intrest Earned", 2));
            transactionTypes.Add(new EnumerationValue("Fees Paid", 3));

            return transactionTypes;
        }

        private List<EnumerationValue> GetCurrencies()
        {
            List<EnumerationValue> currency = new List<EnumerationValue>();

            currency.Add(new EnumerationValue("USD", 0));
            currency.Add(new EnumerationValue("Yen", 1));
            currency.Add(new EnumerationValue("GBP", 2));

            return currency;
        }

        private List<EnumerationValue> GetFunds()
        {
            List<EnumerationValue> funds = new List<EnumerationValue>();

            funds.Add(new EnumerationValue("Fund X", 0));
            funds.Add(new EnumerationValue("Fund Y", 1));
            funds.Add(new EnumerationValue("Fund Z", 2));

            return funds;
        }

        public static SortableSearchableList<CorporateActionEntry> RetrieveCorporateActionEntries()
        {
            SortableSearchableList<CorporateActionEntry> _corporateActionEntryList = new SortableSearchableList<CorporateActionEntry>();
            TimeSpan ts = new TimeSpan(1, 0, 0, 0, 0);
            DateTime previousdate = DateTime.Today.Subtract(ts);

                CorporateActionEntry item = new CorporateActionEntry();
                item.CorporateActionNameID.ID = 1;
                item.CorporateActionNameID.Name = "First Quarter Intrest";
                item.CorporateActionType.ID = 1;
                item.CorporateActionType.Name = "Intrest Earned";
                item.Amount = 1000;
                item.Fund.ID = 1;
                item.Fund.Name = "Fund X";
                item.DataSourceNameID.ID = 1;
                item.DataSourceNameID.ShortName = "GS";
                item.DataSourceNameID.FullName = "Goldman Sachs";
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
                item.CorporateActionType.Name = "Intrest Paid";
                item.Amount = 1000;
                item.Fund.ID = 1;
                item.Fund.Name = "Fund Y";
                item.DataSourceNameID.ID = 2;
                item.DataSourceNameID.ShortName = "MS";
                item.DataSourceNameID.FullName = "Morgan Stanley";
                item.ImpactOnCash = ImpactOnCash.Negative;
                item.TransactionDate = new DateTime(2006, 1, 1);
                item.PaymentDate = new DateTime(2006, 1, 10);
                item.Currency.ID = 1;
                item.Currency.Name = "USD";
                _corporateActionEntryList.Add(item);

            return _corporateActionEntryList;
        }

        private void grdEnterTransactions_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            grdEnterTransactions.CreationFilter = headerCheckBox;
            grdEnterTransactions.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdEnterTransactions.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdEnterTransactions.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdEnterTransactions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdEnterTransactions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdEnterTransactions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdEnterTransactions.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdEnterTransactions.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdEnterTransactions.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colIsSelected = band.Columns[COL_IsSelected];
            colIsSelected.Header.Caption = "";
            colIsSelected.Header.VisiblePosition = 1;

            UltraGridColumn colSource = band.Columns[COL_Source];
            colSource.Header.Caption = "Source";
            colSource.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colSource.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colSource.ValueList = cmbDataSources;
            colSource.Header.VisiblePosition = 2;

            UltraGridColumn colFund = band.Columns[COL_Fund];
            colFund.Header.Caption = "Fund";
            colFund.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colFund.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colFund.ValueList = cmbFunds;
            colFund.Header.VisiblePosition = 3;

            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 4;

            UltraGridColumn colTransactionDate = band.Columns[COL_TransactionDate];
            colTransactionDate.Header.Caption = "Transaction Date";
            colTransactionDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
            colTransactionDate.Header.VisiblePosition = 5;

            UltraGridColumn colTransactionName = band.Columns[COL_TransactionType];
            colTransactionName.Header.Caption = "Transaction Name";
            colTransactionName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colTransactionName.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colTransactionName.ValueList = cmbTransactionType;
            colTransactionName.Header.VisiblePosition = 6;

            UltraGridColumn colPaymentDate = band.Columns[COL_PaymentDate];
            colPaymentDate.Header.Caption = "Payment Date";
            colPaymentDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
            colPaymentDate.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colPaymentDate.Header.VisiblePosition = 7;

            UltraGridColumn colAmount = band.Columns[COL_Amount];
            colAmount.Header.Caption = "Amount";
            colAmount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
            colAmount.Header.VisiblePosition = 8;

            UltraGridColumn colCurrency = band.Columns[COL_Currency];
            colCurrency.Header.Caption = "Currency";
            colCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colCurrency.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colCurrency.ValueList = cmbCurrency;
            colCurrency.Header.VisiblePosition = 9;

            UltraGridColumn colImpactOnCash = band.Columns[COL_ImpactOnCash];
            colImpactOnCash.Header.Caption = "Impact On Cash";
            colImpactOnCash.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colImpactOnCash.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colImpactOnCash.ValueList = cmbImpactOnCash;
            colImpactOnCash.Header.VisiblePosition = 10;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Forms.ManualEntryPassword frmManualEntryPassword = new Forms.ManualEntryPassword();
            frmManualEntryPassword.ShowDialog();
        }

    }
}
