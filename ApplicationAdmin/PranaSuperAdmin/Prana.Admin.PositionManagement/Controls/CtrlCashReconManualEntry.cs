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
    public partial class CtrlCashReconManualEntry : UserControl
    {
        #region Grid Column Names

        const string COL_Symbol = "Symbol";
        const string COL_TransactionType = "CorporateActionNameID";
        const string COL_Difference ="Difference";
        const string COL_ManualAmount = "ManualAmount";
        const string COL_ApplicationAmount = "ApplicationAmount";                            

        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        private CashReconManualEntry _cashReconManualEntry = new CashReconManualEntry();

        public CtrlCashReconManualEntry()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Forms.ManualEntryPassword frmManualEntryPassword = new Forms.ManualEntryPassword();
            frmManualEntryPassword.ShowDialog();
         }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
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
            this.ActiveControl = txtManualEntry;
            this.txtManualEntry.Focus();
        }

        #endregion

        private void SetupBinding()
        {
            _formBindingSource.DataSource = RetrieveManualEntry();

            

            BindTextBoxes();

            BindGridComboBox();

            grdTransactions.DataMember = "CashTransactionEntries";
            grdTransactions.DataSource = _formBindingSource;

            
        }

        private void BindTextBoxes()
        {
            //create a binding object
            Binding userNameBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "User");
            //add new binding
            txtUserName.DataBindings.Add(userNameBinding);

            //Binding symbolBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "ExceptionReportEntryItem.Symbol");
            //txtSymbol.DataBindings.Add(symbolBinding);

            Binding sourceDataBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "CashReconItemValue.BalanceBF");
            txtSourceData.DataBindings.Add(sourceDataBinding);

            Binding applicationDataBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "CashReconItemValue.EstimatedClosingBalance");
            txtApplicationData.DataBindings.Add(applicationDataBinding);

            Binding manualEntryBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "CashReconItemValue.ManualEntry");
            txtManualEntry.DataBindings.Add(manualEntryBinding);

            Binding commentsBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "SummaryComments");
            txtSummaryComments.DataBindings.Add(commentsBinding);
        }

        private void BindGridComboBox()
        {
            cmbTransactionType.DisplayMember = "DisplayText";
            cmbTransactionType.ValueMember = "Value";
            cmbTransactionType.DataSource = GetTransactionTypes();
            Utils.UltraDropDownFilter(cmbTransactionType, "DisplayText");

        }
        private CashReconManualEntry RetrieveManualEntry()
        {
            _cashReconManualEntry.CashReconItemValue.BalanceBF = 1200;
            _cashReconManualEntry.CashReconItemValue.EstimatedClosingBalance = 1000;
            _cashReconManualEntry.CashReconItemValue.ManualEntry = 1100;
            _cashReconManualEntry.User.ID = "1";
            _cashReconManualEntry.User.UserName = "Shams";
            _cashReconManualEntry.SummaryComments = "This is a test comment!";

            _cashReconManualEntry.CashTransactionEntries = GetManualTransactionEntries();
            return _cashReconManualEntry;
        }

        private SortableSearchableList<CashTransactionDetailReconItem> GetManualTransactionEntries()
        {
            SortableSearchableList<CashTransactionDetailReconItem> Entry = new SortableSearchableList<CashTransactionDetailReconItem>();

            CashTransactionDetailReconItem item = new CashTransactionDetailReconItem();

            item.ManualAmount = 150;
            item.Symbol.Name = "MSFT";
            item.Symbol.ID = 1;
            item.CorporateActionNameID.ID = 1;
            item.CorporateActionNameID.Name = "Dividend";
            item.ApplicationAmount = 100;
            item.Difference = item.ManualAmount - item.ApplicationAmount;

            Entry.Add(item);

            item = new CashTransactionDetailReconItem();
            item.ManualAmount = 50;
            item.CorporateActionNameID.ID = 1;
            item.CorporateActionNameID.Name = "Intrest Paid";
            item.ApplicationAmount = 75;
            item.Difference = item.ManualAmount - item.ApplicationAmount;
            Entry.Add(item);

            return Entry;
        }

        private List<EnumerationValue> GetTransactionTypes()
        {
            List<EnumerationValue> transactionTypes = new List<EnumerationValue>();

            transactionTypes.Add(new EnumerationValue("<Add New>", 0));
            transactionTypes.Add(new EnumerationValue("Intrest Paid", 1));
            transactionTypes.Add(new EnumerationValue("Intrest Earned", 2));
            transactionTypes.Add(new EnumerationValue("Fees Paid", 3));
            transactionTypes.Add(new EnumerationValue("Dividend", 4));

            return transactionTypes;
        }

        private void grdTransactions_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            grdTransactions.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdTransactions.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdTransactions.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdTransactions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdTransactions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdTransactions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdTransactions.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdTransactions.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdTransactions.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colTransactionName = band.Columns[COL_TransactionType];
            colTransactionName.Header.Caption = "Transaction Name";
            colTransactionName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colTransactionName.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colTransactionName.ValueList = cmbTransactionType;
            colTransactionName.Header.VisiblePosition = 1;

            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 2;


            UltraGridColumn colApplicationAmount = band.Columns[COL_ApplicationAmount];
            colApplicationAmount.Header.Caption = "Application Amount";
            colApplicationAmount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
            colApplicationAmount.Header.VisiblePosition = 3;

            UltraGridColumn colManualAmount = band.Columns[COL_ManualAmount];
            colManualAmount.Header.Caption = "Manual Amount";
            colManualAmount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
            colManualAmount.Header.VisiblePosition = 4;

            UltraGridColumn colDifference = band.Columns[COL_Difference];
            colDifference.Header.Caption = "Difference";
            colDifference.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
            colDifference.Header.VisiblePosition = 5;
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            //Add row in Grid here
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}
