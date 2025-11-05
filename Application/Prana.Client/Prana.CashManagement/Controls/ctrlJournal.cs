using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.TradeAudit;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.Enumerators;
using Prana.CashManagement.Classes;
using Prana.ClientCommon;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.CashManagement
{
    public partial class ctrlJournal : UserControl, IPublishing
    {
        #region Classes

        public class GroupSortComparer : IComparer
        {
            #region Members

            /// <summary>
            /// The multiplier
            /// </summary>
            private int _multiplier = -1;

            /// <summary>
            /// The column name
            /// </summary>
            private string _columnName;

            /// <summary>
            /// The sort indicator
            /// </summary>
            private SortIndicator _sortIndicator = SortIndicator.Descending;

            #endregion Members

            #region Properties

            /// <summary>
            /// Gets or sets the column.
            /// </summary>
            /// <value>
            /// The column.
            /// </value>
            public string Column
            {
                get { return _columnName; }
                set { _columnName = value; }
            }

            /// <summary>
            /// Gets or sets the sort indicator.
            /// </summary>
            /// <value>
            /// The sort indicator.
            /// </value>
            public SortIndicator SortIndicator
            {
                get { return _sortIndicator; }
                set
                {
                    _sortIndicator = value;
                    switch (_sortIndicator)
                    {
                        case SortIndicator.Ascending:
                            _multiplier = 1;
                            break;
                        case SortIndicator.Descending:
                        case SortIndicator.Disabled:
                        case SortIndicator.None:
                            _multiplier = -1;
                            break;
                        default:
                            break;
                    }
                }
            }

            #endregion Properties

            #region Methods

            /// <summary>
            /// Compares the specified x object.
            /// </summary>
            /// <param name="xObj">The x object.</param>
            /// <param name="yObj">The y object.</param>
            /// <returns></returns>
            public int Compare(object xObj, object yObj)
            {
                try
                {
                    UltraGridGroupByRow x = (UltraGridGroupByRow)xObj;
                    UltraGridGroupByRow y = (UltraGridGroupByRow)yObj;
                    IComparable xValue;
                    IComparable yValue;

                    if (Equals(xObj, yObj))
                    {
                        return 0;
                    }
                    if (!(string.IsNullOrEmpty(_columnName)))
                    {
                        if (x.Rows.SummaryValues[_columnName].Value == null)
                        {
                            return _multiplier;
                        }
                        if (y.Rows.SummaryValues[_columnName].Value == null)
                        {
                            return (-(_multiplier));
                        }
                        if (!x.Rows.SummaryValues[_columnName].Value.GetType().Equals(y.Rows.SummaryValues[_columnName].Value.GetType()))
                        {
                            if (x.Rows.SummaryValues[_columnName].Value is IComparable && y.Rows.SummaryValues[_columnName].Value is IComparable)
                            {
                                xValue = (IComparable)x.Rows.SummaryValues[_columnName].Value;
                                yValue = (IComparable)y.Rows.SummaryValues[_columnName].Value;
                                return xValue.ToString().CompareTo(yValue.ToString()) * _multiplier;
                            }
                            if (x.Rows.SummaryValues[_columnName].Value is IComparable)
                            {
                                return _multiplier;
                            }
                            if (y.Rows.SummaryValues[_columnName].Value is IComparable)
                            {
                                return (-(_multiplier));
                            }
                        }
                        else
                        {
                            xValue = (IComparable)x.Rows.SummaryValues[_columnName].Value;
                            yValue = (IComparable)y.Rows.SummaryValues[_columnName].Value;
                            return xValue.CompareTo(yValue) * _multiplier;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return 0;
            }

            #endregion Methods
        }

        #endregion Classes

        #region Members

        /// <summary>
        /// The group sort comparer
        /// </summary>
        GroupSortComparer _groupSortComparer = new GroupSortComparer();

        /// <summary>
        /// The ls modified transactions
        /// </summary>
        private List<Transaction> _lsModifiedTransactions = new List<Transaction>();

        /// <summary>
        /// The ls trading transactions
        /// </summary>
        private GenericBindingList<Transaction> _lsTradingTransactions = new GenericBindingList<Transaction>();

        /// <summary>
        /// The ls trading transaction entries
        /// </summary>
        private GenericBindingList<TransactionEntry> _lsTradingTransactionEntries = new GenericBindingList<TransactionEntry>();

        /// <summary>
        /// The ls dividend transactions
        /// </summary>
        private GenericBindingList<Transaction> _lsDividendTransactions = new GenericBindingList<Transaction>();

        /// <summary>
        /// The ls dividend transaction entries
        /// </summary>
        private GenericBindingList<TransactionEntry> _lsDividendTransactionEntries = new GenericBindingList<TransactionEntry>();

        /// <summary>
        /// The ls revaluation transactions
        /// </summary>
        private GenericBindingList<Transaction> _lsRevaluationTransactions = new GenericBindingList<Transaction>();

        /// <summary>
        /// The ls revaluation transaction entries
        /// </summary>
        private GenericBindingList<TransactionEntry> _lsRevaluationTransactionEntries = new GenericBindingList<TransactionEntry>();

        /// <summary>
        /// The ls other journal transactions
        /// </summary>
        private GenericBindingList<Transaction> _lsOtherJournalTransactions = new GenericBindingList<Transaction>();

        /// <summary>
        /// The ls other journal transaction entries
        /// </summary>
        private GenericBindingList<TransactionEntry> _lsOtherJournalTransactionEntries = new GenericBindingList<TransactionEntry>();

        /// <summary>
        /// The ls non trading transactions
        /// </summary>
        private GenericBindingList<Transaction> _lsNonTradingTransactions = new GenericBindingList<Transaction>();

        /// <summary>
        /// The ls non trading transaction entries
        /// </summary>
        private GenericBindingList<TransactionEntry> _lsNonTradingTransactionEntries = new GenericBindingList<TransactionEntry>();

        /// <summary>
        /// The ls modified transaction entries
        /// </summary>
        private Dictionary<string, TransactionEntry> _lsModifiedTransactionEntries = new Dictionary<string, TransactionEntry>();

        /// <summary>
        /// The sub account
        /// </summary>
        static ValueList[] _subAccount = new ValueList[4];

        /// <summary>
        /// The account value list
        /// </summary>
        ValueList[] _accountValList = new ValueList[4];

        /// <summary>
        /// The currency value list
        /// </summary>
        ValueList[] _currencyValList = new ValueList[4];

        /// <summary>
        /// The account side value list
        /// </summary>
        ValueList[] _AccountSideValList = new ValueList[2];

        /// <summary>
        /// The fx conversion method operator value list
        /// </summary>
        ValueList[] fxConversionMethodOperatorValList = new ValueList[2];

        /// <summary>
        /// The vl transaction source
        /// </summary>
        ValueList[] _vlTransactionSource = new ValueList[5];

        /// <summary>
        /// The dictionary sub accounts
        /// </summary>
        Dictionary<string, int> dictionarySubAccounts = new Dictionary<string, int>();

        /// <summary>
        /// The dictionary accounts
        /// </summary>
        Dictionary<string, int> dictionaryAccounts = new Dictionary<string, int>();

        /// <summary>
        /// The dictionary currency
        /// </summary>
        Dictionary<string, int> dictionaryCurrency = new Dictionary<string, int>();

        /// <summary>
        /// The newcash activity
        /// </summary>
        List<CashActivity> newcashActivity = new List<CashActivity>();

        /// <summary>
        /// The LST for visible all legs
        /// </summary>
        List<string> lstForVisibleAllLegs = new List<string>();

        /// <summary>
        /// The locker
        /// </summary>
        private object _locker = new object();

        /// <summary>
        /// The ls transactions from database
        /// </summary>
        List<Transaction> lsTransactionsFromDB;

        /// <summary>
        /// The ls deleted transaction entry
        /// </summary>
        List<TransactionEntry> lsDeletedTransactionEntry = new List<TransactionEntry>();

        /// <summary>
        /// The is new row added
        /// </summary>
        bool isNewRowAdded = false;

        /// <summary>
        /// The proxy
        /// </summary>
        DuplexProxyBase<ISubscription> _proxy;

        /// <summary>
        /// The group by columns collection
        /// </summary>
        List<string> GroupByColumnsCollection = new List<string>();

        /// <summary>
        /// The column sorted
        /// </summary>
        UltraGridColumn _columnSorted = null;

        /// <summary>
        /// The trading
        /// </summary>
        private const string Trading = "CashJournal_TradingTransaction";

        /// <summary>
        /// The non trading
        /// </summary>
        private const string NonTrading = "CashJournal_NonTradingTransaction";

        /// <summary>
        /// The dividend
        /// </summary>
        private const string Dividend = "CashJournal_Dividend";

        /// <summary>
        /// The revaluation
        /// </summary>
        private const string Revaluation = "CashJournal_Revaluation";

        /// <summary>
        /// The opening balance
        /// </summary>
        private const string OpeningBalance = "CashJournal_OpeningBalance";

        /// <summary>
        /// The cash journal layout
        /// </summary>
        private Dictionary<string, CashManagementLayout> cashJournalLayout = new Dictionary<string, CashManagementLayout>();

        /// <summary>
        /// The original transactions
        /// </summary>
        private Dictionary<string, TransactionEntry> _originalTransactions = new Dictionary<string, TransactionEntry>();

        /// <summary>
        /// The newly added transactions
        /// </summary>
        private List<string> _newlyAddedTransactions = new List<string>();
        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the dividend transaction entries.
        /// </summary>
        /// <value>
        /// The dividend transaction entries.
        /// </value>
        public GenericBindingList<TransactionEntry> DividendTransactionEntries
        {
            get { return _lsDividendTransactionEntries; }
            set { _lsDividendTransactionEntries = value; }
        }

        /// <summary>
        /// Gets or sets the dividend transactions.
        /// </summary>
        /// <value>
        /// The dividend transactions.
        /// </value>
        public GenericBindingList<Transaction> DividendTransactions
        {
            get { return _lsDividendTransactions; }
            set { _lsDividendTransactions = value; }
        }

        /// <summary>
        /// Gets or sets the ls modified transactions.
        /// </summary>
        /// <value>
        /// The ls modified transactions.
        /// </value>
        public List<Transaction> lsModifiedTransactions
        {
            get { return _lsModifiedTransactions; }
            set { _lsModifiedTransactions = value; }
        }

        /// <summary>
        /// Gets or sets the modified transaction entries.
        /// </summary>
        /// <value>
        /// The modified transaction entries.
        /// </value>
        public Dictionary<string, TransactionEntry> ModifiedTransactionEntries
        {
            get { return _lsModifiedTransactionEntries; }
            set { _lsModifiedTransactionEntries = value; }
        }

        /// <summary>
        /// Gets or sets the revaluation transaction entries.
        /// </summary>
        /// <value>
        /// The revaluation transaction entries.
        /// </value>
        public GenericBindingList<TransactionEntry> RevaluationTransactionEntries
        {
            get { return _lsRevaluationTransactionEntries; }
            set { _lsRevaluationTransactionEntries = value; }
        }

        /// <summary>
        /// Gets or sets the revaluation transactions.
        /// </summary>
        /// <value>
        /// The revaluation transactions.
        /// </value>
        public GenericBindingList<Transaction> RevaluationTransactions
        {
            get { return _lsRevaluationTransactions; }
            set { _lsRevaluationTransactions = value; }
        }

        /// <summary>
        /// Gets or sets the trading transaction entries.
        /// </summary>
        /// <value>
        /// The trading transaction entries.
        /// </value>
        public GenericBindingList<TransactionEntry> TradingTransactionEntries
        {
            get { return _lsTradingTransactionEntries; }
            set { _lsTradingTransactionEntries = value; }
        }

        /// <summary>
        /// Gets or sets the trading transactions.
        /// </summary>
        /// <value>
        /// The trading transactions.
        /// </value>
        public GenericBindingList<Transaction> TradingTransactions
        {
            get { return _lsTradingTransactions; }
            set { _lsTradingTransactions = value; }
        }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ctrlJournal"/> class.
        /// </summary>
        public ctrlJournal()
        {
            try
            {
                InitializeComponent();
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                CreateSubscriptionServicesProxy();
                BindGrid();
                CashAccountsUI.SubAccountUpdated += new EventHandler(CashAccountsUI_SubAccountUpdated);
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

        #endregion Constructors

        #region Trading Transactions Section

        /// <summary>
        /// Handles the InitializeGroupByRow event of the grdTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeGroupByRowEventArgs"/> instance containing the event data.</param>
        private void grdTradingTransactions_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            try
            {
                HelperClass.GroupByRowSetting(e);
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

        /// <summary>
        /// Handles the InitializeLayout event of the grdTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdTradingTransactions_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                HelperClass.SummarySettings(e);
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

        /// <summary>
        /// Handles the InitializeRow event of the grdTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdTradingTransactions_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                CommonInitializeRowWork(e);
                #region Hide local currency cash journal transaction entries
                //CHMW-3114 [Foreign Positions Settling in Base Currency] [Implementation] [Cash Management] Hide local currency cash journal transaction entries
                TransactionEntry trEntry = (TransactionEntry)e.Row.ListObject;
                if (trEntry != null
                    && trEntry.TransactionSource == CashTransactionType.SettlementTransaction
                    && CashDataManager.GetInstance().GetCashPreferences(trEntry.AccountID) != null
                    && !CashDataManager.GetInstance().GetCashPreferences(trEntry.AccountID).IsCashSettlementEntriesVisible)
                {
                    e.Row.Hidden = true;
                }
                else
                {
                    e.Row.Hidden = false;
                }

                #endregion
            }
            // rowColRegionIntersectionUIElement =(RowColRegionIntersectionUIElement)grdTradingTransactions.DisplayLayout.RowScrollRegions[0].GetUIElement;
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the BeforeColumnChooserDisplayed event of the grdTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeColumnChooserDisplayedEventArgs"/> instance containing the event data.</param>
        private void grdTradingTransactions_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdTradingTransactions);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseClick event of the grdTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdTradingTransactions_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                MouseClickCommanForSort(sender, e);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the grdTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdTradingTransactions_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MouseDownCommanForSort(e, grdTradingTransactions);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the AfterSortChange event of the grdTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BandEventArgs"/> instance containing the event data.</param>
        private void grdTradingTransactions_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                if (_columnSorted != null)
                {
                    string[] lsColumnToSort = new string[] { "TransactionDate", "AccountName", "Symbol", "CurrencyName" };
                    var lsColumnSortQuery = from column in lsColumnToSort where column == _columnSorted.Key select column;
                    if (lsColumnSortQuery.Count() > 0)
                        SortGridByColumnName(grdTradingTransactions);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the BeforeCustomRowFilterDialog event of the grdTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeCustomRowFilterDialogEventArgs"/> instance containing the event data.</param>
        private void grdTradingTransactions_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
        #endregion      

        void grd_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {
            try
            {
                if (e.Column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE))
                {
                    e.ValueList.ValueListItems.Insert(4, "(Today)", "(Today)");
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

        void grdTradingTransactions_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdTradingTransactions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdTradingTransactions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Non Trading Transactions Section

        /// <summary>
        /// Handles the AfterCellUpdate event of the grdNonTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdNonTradingTransactions_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                AfterCellUpdateAction(e);
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

        /// <summary>
        /// Handles the CellChange event of the grdNonTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdNonTradingTransactions_CellChange(object sender, CellEventArgs e)
        {
            try
            {                
                AddOriginalTransaction((TransactionEntry)e.Cell.Row.ListObject);
                UpdateErrorsOnCellChange(e);
                //Added by: Bharat Raturi, 18 Sep 2014
                if (e.Cell.Column.Key == "DR" || e.Cell.Column.Key == "CR" || e.Cell.Column.Key == "EntryAccountSide")
                {
                    SetColumnReadOnly(e);
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

        /// <summary>
        /// added by: Bharat Raturi, 18 Sep 2014
        /// Set the cell to read only if entry side is DR and current cell is CR and vice-versa
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-4915
        /// </summary>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void SetColumnReadOnly(CellEventArgs e)
        {
            if (e.Cell.Row.Cells["EntryAccountSide"].Text == "DR")
            {
                e.Cell.Row.Cells["CR"].Value = 0.0;
                e.Cell.Row.Cells["CR"].Activation = Activation.NoEdit;
                e.Cell.Row.Cells["DR"].Activation = Activation.AllowEdit;
            }
            else if (e.Cell.Row.Cells["EntryAccountSide"].Text == "CR")
            {
                e.Cell.Row.Cells["DR"].Value = 0.0;
                e.Cell.Row.Cells["DR"].Activation = Activation.NoEdit;
                e.Cell.Row.Cells["CR"].Activation = Activation.AllowEdit;
            }
        }

        /// <summary>
        /// Handles the InitializeGroupByRow event of the grdNonTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeGroupByRowEventArgs"/> instance containing the event data.</param>
        private void grdNonTradingTransactions_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            try
            {
                HelperClass.GroupByRowSetting(e);
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

        /// <summary>
        /// Handles the InitializeLayout event of the grdNonTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdNonTradingTransactions_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                HelperClass.SummarySettings(e);
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

        /// <summary>
        /// Handles the InitializeRow event of the grdNonTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdNonTradingTransactions_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                CommonInitializeRowWork(e);
                var transactionEntry = (TransactionEntry)e.Row.ListObject;
                if (!((transactionEntry.TransactionSource == CashTransactionType.ManualJournalEntry) || (transactionEntry.TransactionSource == CashTransactionType.ImportedEditableData)))
                {
                    e.Row.Activation = Activation.NoEdit;
                }
                if ((transactionEntry.TransactionSource == CashTransactionType.ManualJournalEntry) && (transactionEntry.Description == "Cash In Lieu"))
                {
                    e.Row.Activation = Activation.NoEdit;
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

        /// <summary>
        /// Handles the MouseDown event of the grdNonTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdNonTradingTransactions_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ActivateRow(sender, e);
                MouseDownCommanForSort(e, grdNonTradingTransactions);
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

        /// <summary>
        /// Handles the BeforeColumnChooserDisplayed event of the grdNonTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs"/> instance containing the event data.</param>
        private void grdNonTradingTransactions_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdNonTradingTransactions);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the AfterSortChange event of the grdNonTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BandEventArgs"/> instance containing the event data.</param>
        private void grdNonTradingTransactions_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                if (_columnSorted != null)
                {
                    string[] lsColumnToSort = new string[] { "TransactionDate", "AccountName", "Symbol", "CurrencyName" };
                    var lsColumnSortQuery = from column in lsColumnToSort where column == _columnSorted.Key select column;
                    if (lsColumnSortQuery.Count() > 0)
                        SortGridByColumnName(grdNonTradingTransactions);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseClick event of the grdNonTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdNonTradingTransactions_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                MouseClickCommanForSort(sender, e);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the BeforeCustomRowFilterDialog event of the grdNonTradingTransactions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeCustomRowFilterDialogEventArgs"/> instance containing the event data.</param>
        private void grdNonTradingTransactions_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        void grdNonTradingTransactions_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdNonTradingTransactions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdNonTradingTransactions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Dividend Section

        /// <summary>
        /// Handles the AfterCellUpdate event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdDividend_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                AfterCellUpdateAction(e);
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

        /// <summary>
        /// Handles the AfterRowActivate event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void grdDividend_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                if (grdDividend.ActiveRow != null)
                {
                    //UltraGridRow row = ((UltraGrid)sender).ActiveRow;
                    //Infragistics_SetAllColumnsEditState(row);
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

        /// <summary>
        /// Handles the AfterRowUpdate event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        private void grdDividend_AfterRowUpdate(object sender, RowEventArgs e)
        {
            try
            {
                UpdateListOfModifiedTransactions();
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

        /// <summary>
        /// Handles the CellChange event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdDividend_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                UpdateErrorsOnCellChange(e);

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

        /// <summary>
        /// Handles the InitializeGroupByRow event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeGroupByRowEventArgs"/> instance containing the event data.</param>
        private void grdDividend_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            try
            {
                HelperClass.GroupByRowSetting(e);
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

        /// <summary>
        /// Handles the InitializeLayout event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdDividend_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                HelperClass.SummarySettings(e);
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

        /// <summary>
        /// Handles the InitializeRow event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdDividend_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                CommonInitializeRowWork(e);
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

        /// <summary>
        /// Handles the MouseDown event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdDividend_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MouseDownCommanForSort(e, grdDividend);
                ActivateRow(sender, e);
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

        /// <summary>
        /// Handles the BeforeColumnChooserDisplayed event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs"/> instance containing the event data.</param>
        private void grdDividend_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdDividend);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the AfterSortChange event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BandEventArgs"/> instance containing the event data.</param>
        private void grdDividend_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                if (_columnSorted != null)
                {
                    string[] lsColumnToSort = new string[] { "TransactionDate", "AccountName", "Symbol", "CurrencyName" };
                    var lsColumnSortQuery = from column in lsColumnToSort where column == _columnSorted.Key select column;
                    if (lsColumnSortQuery.Count() > 0)
                        SortGridByColumnName(grdDividend);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseClick event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdDividend_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                MouseClickCommanForSort(sender, e);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the BeforeCustomRowFilterDialog event of the grdDividend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeCustomRowFilterDialogEventArgs"/> instance containing the event data.</param>
        private void grdDividend_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        void grdDividend_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdDividend.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdDividend.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Revaluation Section

        /// <summary>
        /// Handles the InitializeGroupByRow event of the grdRevaluation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeGroupByRowEventArgs"/> instance containing the event data.</param>
        private void grdRevaluation_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            try
            {
                HelperClass.GroupByRowSetting(e);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the InitializeLayout event of the grdRevaluation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdRevaluation_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                HelperClass.SummarySettings(e);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the InitializeRow event of the grdRevaluation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdRevaluation_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                CommonInitializeRowWork(e);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the BeforeColumnChooserDisplayed event of the grdRevaluation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs"/> instance containing the event data.</param>
        private void grdRevaluation_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdRevaluation);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the AfterSortChange event of the grdRevaluation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BandEventArgs"/> instance containing the event data.</param>
        private void grdRevaluation_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                if (_columnSorted != null)
                {
                    string[] lsColumnToSort = new string[] { "TransactionDate", "AccountName", "Symbol", "CurrencyName" };
                    var lsColumnSortQuery = from column in lsColumnToSort where column == _columnSorted.Key select column;
                    if (lsColumnSortQuery.Count() > 0)
                        SortGridByColumnName(grdRevaluation);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseClick event of the grdRevaluation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdRevaluation_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                MouseClickCommanForSort(sender, e);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the grdRevaluation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdRevaluation_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MouseDownCommanForSort(e, grdRevaluation);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the BeforeCustomRowFilterDialog event of the grdRevaluation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeCustomRowFilterDialogEventArgs"/> instance containing the event data.</param>
        private void grdRevaluation_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();

        }

        void grdRevaluation_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdRevaluation.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdRevaluation.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Opening Balance Section

        /// <summary>
        /// Handles the AfterCellUpdate event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                AfterCellUpdateAction(e);
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

        /// <summary>
        /// Handles the AfterRowUpdate event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_AfterRowUpdate(object sender, RowEventArgs e)
        {
            try
            {
                UpdateListOfModifiedTransactions();
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

        /// <summary>
        /// Handles the CellChange event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                AddOriginalTransaction((TransactionEntry)e.Cell.Row.ListObject);
                UpdateErrorsOnCellChange(e);
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

        /// <summary>
        /// Handles the InitializeGroupByRow event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeGroupByRowEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            try
            {
                HelperClass.GroupByRowSetting(e);
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

        /// <summary>
        /// Handles the InitializeLayout event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                HelperClass.SummarySettings(e);
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

        /// <summary>
        /// Handles the InitializeRow event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                CommonInitializeRowWork(e);
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

        /// <summary>
        /// Handles the MouseDown event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MouseDownCommanForSort(e, grdOpeningBalance);
                ActivateRow(sender, e);
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

        /// <summary>
        /// Handles the BeforeColumnChooserDisplayed event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {

            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdOpeningBalance);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the AfterSortChange event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BandEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                if (_columnSorted != null)
                {
                    string[] lsColumnToSort = new string[] { "TransactionDate", "AccountName", "Symbol", "CurrencyName" };
                    var lsColumnSortQuery = from column in lsColumnToSort where column == _columnSorted.Key select column;
                    if (lsColumnSortQuery.Count() > 0)
                        SortGridByColumnName(grdOpeningBalance);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseClick event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                MouseClickCommanForSort(sender, e);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the BeforeCustomRowFilterDialog event of the grdOpeningBalance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeCustomRowFilterDialogEventArgs"/> instance containing the event data.</param>
        private void grdOpeningBalance_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        void grdOpeningBalance_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdOpeningBalance.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdOpeningBalance.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Common Grid Section
        /// <summary>
        /// Accounts the side column setting.
        /// </summary>
        /// <param name="ultraGridRow">The ultra grid row.</param>
        private void AccountSideColumnSetting(UltraGridRow ultraGridRow)
        {
            try
            {
                AccountSide entryAcSide = (AccountSide)Enum.Parse(typeof(AccountSide), ultraGridRow.Cells["EntryAccountSide"].Text.ToString());
                if (!CustomThemeHelper.ApplyTheme)
                {
                    if (entryAcSide == AccountSide.DR)
                    {
                        ultraGridRow.Appearance.BackColor = Color.Black;
                        ultraGridRow.Cells["CR"].Appearance.ForeColor = ultraGridRow.Appearance.BackColor;
                        ultraGridRow.Cells["DR"].Appearance.ForeColor = Color.White;
                    }
                    else if (entryAcSide == AccountSide.CR)
                    {
                        ultraGridRow.Appearance.BackColor = Color.Gray;
                        ultraGridRow.Cells["DR"].Appearance.ForeColor = ultraGridRow.Appearance.BackColor;
                        ultraGridRow.Cells["CR"].Appearance.ForeColor = Color.White;
                    }
                }
                else
                {
                    if (entryAcSide == AccountSide.DR)
                    {
                        foreach (UltraGridCell cell in ultraGridRow.Cells)
                        {
                            if (!(cell.Column.Key.Equals(CashManagementConstants.COLUMN_SYMBOL) ||
                                cell.Column.Key.Equals(CashManagementConstants.COLUMN_ACCOUNT) ||
                                cell.Column.Key.Equals(CashManagementConstants.COLUMN_CURRENCYNAME) ||
                                cell.Column.Key.Equals("TransactionDate") ||
                                cell.Column.Key.Equals("TransactionID") ||
                                cell.Column.Key.Equals(CashManagementConstants.COLUMN_DESCRIPTION)))
                            {
                                cell.Appearance.BackColor = Color.FromArgb(231, 232, 233);
                            }
                        }
                        ultraGridRow.Cells["CR"].Appearance.ForeColor = Color.FromArgb(231, 232, 233);
                        ultraGridRow.Cells["DR"].Appearance.ForeColor = Color.Black;
                    }
                    else if (entryAcSide == AccountSide.CR)
                    {
                        foreach (UltraGridCell cell in ultraGridRow.Cells)
                        {
                            if (!(cell.Column.Key.Equals(CashManagementConstants.COLUMN_SYMBOL) ||
                                cell.Column.Key.Equals(CashManagementConstants.COLUMN_ACCOUNT) ||
                                cell.Column.Key.Equals(CashManagementConstants.COLUMN_CURRENCYNAME) ||
                                cell.Column.Key.Equals("TransactionDate") ||
                                cell.Column.Key.Equals("TransactionID") ||
                                cell.Column.Key.Equals(CashManagementConstants.COLUMN_DESCRIPTION)))
                            {
                                cell.Appearance.BackColor = Color.FromArgb(134, 134, 134);
                            }
                        }
                        ultraGridRow.Cells["DR"].Appearance.ForeColor = Color.FromArgb(134, 134, 134);
                        ultraGridRow.Cells["CR"].Appearance.ForeColor = Color.Black;
                    }
                }

                if (ultraTabControlCashMainValue.SelectedTab != null && ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                {
                    if (entryAcSide == AccountSide.CR)
                    {
                        ultraGridRow.Cells["DR"].Value = 0;
                    }
                    else if (entryAcSide == AccountSide.DR)
                    {
                        ultraGridRow.Cells["CR"].Value = 0;
                    }
                }

                if (ultraTabControlCashMainValue.SelectedTab != null && ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                {
                    if (entryAcSide == AccountSide.CR)
                    {
                        ultraGridRow.Cells["DR"].Value = 0;
                        ultraGridRow.Cells["DR"].Activation = Activation.NoEdit;
                        ultraGridRow.Cells["CR"].Activation = Activation.AllowEdit;
                    }
                    else if (entryAcSide == AccountSide.DR)
                    {
                        ultraGridRow.Cells["CR"].Value = 0;
                        ultraGridRow.Cells["CR"].Activation = Activation.NoEdit;
                        ultraGridRow.Cells["DR"].Activation = Activation.AllowEdit;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Activates the row.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void ActivateRow(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Commons the initialize row work.
        /// </summary>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        private void CommonInitializeRowWork(InitializeRowEventArgs e)
        {
            try
            {
                if (!e.ReInitialize && !e.Row.IsGroupByRow && !e.Row.IsSummaryRow)
                {
                    if (isNewRowAdded && ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans && e.Row.ListObject is TransactionEntry) || (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance && e.Row.ListObject is TransactionEntry)))
                    {
                        //Narendra Kumar Jangir, Dec 30 2013
                        //For new transaction entry, default currency should be base currency
                        e.Row.Cells["CurrencyName"].Value = CashDataManager.BaseCurrencyID;
                        e.Row.Cells["FxRate"].Value = 1;
                        e.Row.Cells["FXConversionMethodOperator"].Value = Operator.M.ToString();
                        setErrorsForNewTransactionEntry(e);
                    }
                    else
                        e.Row.ExpandAll();

                    if (e.Row.ListObject is TransactionEntry)
                    {
                        AccountSideColumnSetting(e.Row);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the errors on cell change.
        /// </summary>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void UpdateErrorsOnCellChange(CellEventArgs e)
        {
            try
            {
                if (e.Cell.Row.ListObject is TransactionEntry)
                {
                    TransactionEntry trEntry = e.Cell.Row.ListObject as TransactionEntry;
                    trEntry.properityChanged(e.Cell.Column.Key, e.Cell.Text);

                    if (e.Cell.Column.Key == "DR" || e.Cell.Column.Key == "CR")
                    {
                        bool isValid = false;
                        decimal value;
                        isValid = decimal.TryParse(e.Cell.Text, out value);
                        if ((isValid == false) || value < 0)
                        {
                            e.Cell.CancelUpdate();
                        }
                        isValid = false;
                    }
                    if (e.Cell.Column.Key == "EntryAccountSide")
                    {
                        AccountSideColumnSetting(e.Cell.Row);
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

        /// <summary>
        /// Afters the cell update action.
        /// </summary>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void AfterCellUpdateAction(CellEventArgs e)
        {
            try
            {
                if (e.Cell.Row.ListObject is TransactionEntry)
                {
                    TransactionEntry objEntry = e.Cell.Row.ListObject as TransactionEntry;
                    Transaction selectedTranscation = null;
                    if (objEntry != null)
                    {
                        selectedTranscation = GetSelectedTransaction();

                        if (e.Cell.Column.Header.Caption == "Cash Sub-Account")
                        {
                            int value;
                            if (!int.TryParse(Convert.ToString(e.Cell.Value), out value) || !dictionarySubAccounts.ContainsValue(Convert.ToInt32(e.Cell.Value)))
                            {
                                if (!string.IsNullOrEmpty(e.Cell.Value.ToString()))
                                {
                                    e.Cell.SetValue(string.Empty, false);
                                    objEntry.properityChanged(e.Cell.Column.Key, e.Cell.Text);
                                }
                                return;
                            }
                        }

                        //PRANA-9777
                        if (selectedTranscation != null)
                            foreach (TransactionEntry trEntry in selectedTranscation.TransactionEntries)
                            {

                                AddOriginalTransaction(trEntry);
                                trEntry.ModifyDate = DateTime.Now;
                                //PRANA-9776
                                trEntry.UserId = Convert.ToInt32(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                trEntry.UserName = CachedDataManager.GetInstance.LoggedInUser.ShortName;
                            }

                        if (e.Cell.Column.Header.Caption == "Symbol")
                        {
                            if (selectedTranscation != null)
                                foreach (TransactionEntry trEntry in selectedTranscation.TransactionEntries)
                                {
                                    trEntry.Symbol = e.Cell.Text;
                                    trEntry.properityChanged(e.Cell.Column.Key, e.Cell.Text);

                                }

                            CashDataManager.GetInstance().ValidateSymbol(objEntry);
                        }
                        else if ((e.Cell.Column.Header.Caption == "DR" || e.Cell.Column.Header.Caption == "CR") && !e.Cell.Text.Equals(string.Empty))
                        {
                            //selectedTranscation = GetSelectedTransaction();
                            if (selectedTranscation != null)
                            {
                                selectedTranscation.Modify(objEntry, Convert.ToDecimal(e.Cell.OriginalValue), e.Cell.Column.Header.Caption);
                                selectedTranscation.Validate();
                            }

                        }
                        else if (e.Cell.Column.Key == "FxRate" || e.Cell.Column.Key == "FXConversionMethodOperator")
                        {
                            if (selectedTranscation != null)
                                selectedTranscation.Validate();
                        }
                        else if ((e.Cell.Column.Header.Caption == "Currency"))
                        {
                            objEntry.CurrencyID = Convert.ToInt32(e.Cell.Value);
                            objEntry.CurrencyName = e.Cell.Text;
                            //whenever currency is changed to base currency then change fxrate to 1
                            if (objEntry.CurrencyID == CashDataManager.BaseCurrencyID)
                            {
                                objEntry.FXConversionMethodOperator = Operator.M.ToString();
                                objEntry.FxRate = 1;
                            }
                            //Narendra Kumar Jangir, Dec 23, 2013
                            //Whenever SubAccount/Account/Currency is changed for a transactionentry
                            //Then update cache of LastCalculatedBalanceDate of previous journal entry so that subaccount balances can be calculated properly
                            if (!string.IsNullOrEmpty(e.Cell.OriginalValue.ToString()) && (!(e.Cell.OriginalValue.ToString()).Equals(e.Cell.Value.ToString())))
                            {
                                string key = objEntry.AccountID.ToString() + "_" + dictionaryCurrency[(e.Cell.OriginalValue.ToString())].ToString() + "_" + objEntry.SubAcID.ToString();
                                if (!ModifiedTransactionEntries.ContainsKey(key))
                                {
                                    TransactionEntry newTranEntry = new TransactionEntry();
                                    newTranEntry.AccountID = objEntry.AccountID;
                                    newTranEntry.AccountName = objEntry.AccountName;
                                    newTranEntry.CurrencyID = dictionaryCurrency[(e.Cell.OriginalValue.ToString())];
                                    newTranEntry.CurrencyName = e.Cell.OriginalValue.ToString();
                                    newTranEntry.SubAcID = objEntry.SubAcID;
                                    newTranEntry.SubAcName = objEntry.SubAcName;
                                    newTranEntry.TransactionDate = objEntry.TransactionDate;
                                    newTranEntry.TransactionSource = objEntry.TransactionSource;
                                    newTranEntry.TransactionID = objEntry.TransactionID;
                                    ModifiedTransactionEntries.Add(key, newTranEntry);
                                }
                            }

                            if (selectedTranscation != null)
                                selectedTranscation.Validate();
                            //Allow user to enter transaction having multiple currencies
                            //Narendra Kumar Jangir Dec 20,2013
                            //if (selectedTranscation != null)
                            //    foreach (TransactionEntry trEntry in selectedTranscation.TransactionEntries)
                            //    {
                            //        trEntry.CurrencyID = objEntry.CurrencyID;
                            //        trEntry.CurrencyName = objEntry.CurrencyName;
                            //        trEntry.properityChanged(e.Cell.Column.Key, e.Cell.Text);
                            //    }
                        }
                        else if (e.Cell.Column.Header.Caption == "Account")
                        {
                            //modified by amit on 18.03.2015
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-3095
                            if (e.Cell.Value.ToString() != String.Empty)
                            {
                                objEntry.AccountID = Convert.ToInt32(e.Cell.Value);
                                objEntry.AccountName = e.Cell.Text;
                                if (selectedTranscation != null)
                                {
                                    foreach (TransactionEntry trEntry in selectedTranscation.TransactionEntries)
                                    {
                                        trEntry.AccountID = objEntry.AccountID;
                                        trEntry.AccountName = objEntry.AccountName;
                                        trEntry.properityChanged(e.Cell.Column.Key, e.Cell.Text);

                                        if (!string.IsNullOrEmpty(e.Cell.OriginalValue.ToString()) && (!(e.Cell.OriginalValue.ToString()).Equals(e.Cell.Value.ToString())))
                                        {
                                            string key = dictionaryAccounts[(e.Cell.OriginalValue.ToString())].ToString() + "_" + trEntry.CurrencyID.ToString() + "_" + trEntry.SubAcID.ToString();
                                            if (!ModifiedTransactionEntries.ContainsKey(key))
                                            {
                                                TransactionEntry newTranEntry = new TransactionEntry();
                                                newTranEntry.AccountID = dictionaryAccounts[(e.Cell.OriginalValue.ToString())];
                                                newTranEntry.AccountName = e.Cell.OriginalValue.ToString();
                                                newTranEntry.CurrencyID = trEntry.CurrencyID;
                                                newTranEntry.CurrencyName = trEntry.CurrencyName;
                                                newTranEntry.SubAcID = trEntry.SubAcID;
                                                newTranEntry.SubAcName = trEntry.SubAcName;
                                                newTranEntry.TransactionDate = trEntry.TransactionDate;
                                                newTranEntry.TransactionSource = objEntry.TransactionSource;
                                                newTranEntry.TransactionID = trEntry.TransactionID;
                                                ModifiedTransactionEntries.Add(key, newTranEntry);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                UpdateErrorsOnCellChange(e);
                                return;
                            }
                        }
                        else if (e.Cell.Column.Header.Caption == "Cash Sub-Account")
                        {
                            objEntry.SubAcID = Convert.ToInt32(e.Cell.Value);
                            objEntry.SubAcName = e.Cell.Text;
                            if (!string.IsNullOrEmpty(e.Cell.OriginalValue.ToString()) && (!(e.Cell.OriginalValue.ToString()).Equals(e.Cell.Value.ToString())))
                            {
                                string key = objEntry.AccountID.ToString() + "_" + objEntry.CurrencyID.ToString() + "_" + dictionarySubAccounts[(e.Cell.OriginalValue.ToString())].ToString();
                                if (!ModifiedTransactionEntries.ContainsKey(key))
                                {
                                    TransactionEntry newTranEntry = new TransactionEntry();
                                    newTranEntry.AccountID = objEntry.AccountID;
                                    newTranEntry.AccountName = objEntry.AccountName;
                                    newTranEntry.CurrencyID = objEntry.CurrencyID;
                                    newTranEntry.CurrencyName = objEntry.CurrencyName;
                                    newTranEntry.SubAcID = dictionarySubAccounts[(e.Cell.OriginalValue.ToString())];
                                    newTranEntry.SubAcName = e.Cell.OriginalValue.ToString();
                                    newTranEntry.TransactionDate = objEntry.TransactionDate;
                                    newTranEntry.TransactionSource = objEntry.TransactionSource;
                                    newTranEntry.TransactionID = objEntry.TransactionID;
                                    ModifiedTransactionEntries.Add(key, newTranEntry);
                                }
                            }
                        }
                        else if ((e.Cell.Column.Key == "TransactionDate"))
                        {
                            if (!string.IsNullOrEmpty(e.Cell.OriginalValue.ToString()) && (!(e.Cell.OriginalValue.ToString()).Equals(e.Cell.Value.ToString())))
                            {
                                foreach (TransactionEntry transaction in selectedTranscation.TransactionEntries)
                                {
                                    bool isExist = false;
                                    string key = transaction.AccountID.ToString() + "_" + transaction.CurrencyID.ToString() + "_" + transaction.SubAcID;
                                    if (!ModifiedTransactionEntries.ContainsKey(key) || (isExist = ModifiedTransactionEntries[key].TransactionDate > Convert.ToDateTime(e.Cell.Value)))
                                    {
                                        TransactionEntry newTranEntry = new TransactionEntry();
                                        newTranEntry.AccountID = transaction.AccountID;
                                        newTranEntry.AccountName = transaction.AccountName;
                                        newTranEntry.CurrencyID = transaction.CurrencyID;
                                        newTranEntry.CurrencyName = transaction.CurrencyName;
                                        newTranEntry.SubAcID = transaction.SubAcID;
                                        newTranEntry.SubAcName = transaction.SubAcName;
                                        newTranEntry.TransactionSource = transaction.TransactionSource;
                                        newTranEntry.TransactionID = transaction.TransactionID;
                                        if (!isExist)
                                        {
                                            newTranEntry.TransactionDate = Convert.ToDateTime(e.Cell.OriginalValue);
                                            ModifiedTransactionEntries.Add(key, newTranEntry);
                                        }
                                        else
                                        {
                                            newTranEntry.TransactionDate = Convert.ToDateTime(e.Cell.Value);
                                            ModifiedTransactionEntries[key] = newTranEntry;
                                        }
                                        newTranEntry = null;
                                    }
                                }
                            }

                            if (selectedTranscation != null)
                            {
                                selectedTranscation.Date = Convert.ToDateTime(e.Cell.Value);
                                //Update the sub account last calculated balance date properly 
                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3157
                                List<TransactionEntry> tr = _lsModifiedTransactionEntries.Values.AsEnumerable().Where(r => r.TransactionID == selectedTranscation.TransactionID).ToList();
                                foreach (TransactionEntry t in tr)
                                {
                                    if (t.TransactionDate > selectedTranscation.Date)
                                        t.TransactionDate = selectedTranscation.Date;
                                }
                            }
                        }
                        if (e.Cell.OriginalValue != null && !(Convert.ToString(e.Cell.OriginalValue).Equals(Convert.ToString(e.Cell.Value))))
                        {
                            UpdateListOfModifiedTransactions();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }

        }

        /// <summary>
        /// Infragisticses the state of the set all columns edit.
        /// </summary>
        /// <param name="row">The row.</param>
        public void Infragistics_SetAllColumnsEditState(UltraGridRow row)
        {
            try
            {
                bool isTradingDividend = false;
                UltraGridBand band = row.Band;
                if (row.ListObject != null)
                {
                    if (row.ListObject is TransactionEntry)
                    {
                        if (((TransactionEntry)(row.ListObject)).TransactionSource == CashTransactionType.CashTransaction)
                            isTradingDividend = true;
                        else
                            isTradingDividend = false;
                    }
                    else if (row.ListObject is Transaction)
                    {
                        isTradingDividend = ((Transaction)(row.ListObject)).IsDividendTransaction();
                    }
                    for (int i = 0; i < band.Columns.Count; i++)  // columns belong to a band, not a row
                    {
                        if (!band.Columns[i].Hidden) // don't mess with hidden columns!
                        {
                            if (isTradingDividend)
                            {
                                band.Columns[i].TabStop = false;
                                band.Columns[i].CellActivation = Activation.NoEdit;
                            }
                            else
                            {
                                band.Columns[i].TabStop = true;
                                band.Columns[i].CellActivation = Activation.AllowEdit;
                            }
                        }
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

        /// <summary>
        /// Initializes the grid layout.
        /// </summary>
        /// <param name="cashManagementLayout">The cash management layout.</param>
        /// <param name="grd">The GRD.</param>
        private void InitializeGridLayout(CashManagementLayout cashManagementLayout, UltraGrid grd)
        {
            try
            {
                UnWireEvents();
                if (cashManagementLayout != null)
                {
                    if (cashManagementLayout.GroupByColumnsCollection.Count > 0)
                    {
                        //Set GroupBy Columns
                        bool flag = true;
                        int GroupedColumns = grd.DisplayLayout.Bands[0].SortedColumns.Count;
                        for (int i = 0; i < GroupedColumns - 1; i++)
                        {
                            grd.DisplayLayout.Bands[0].SortedColumns.Remove(GroupedColumns - 1 - i);
                        }
                        foreach (string item in cashManagementLayout.GroupByColumnsCollection)
                        {
                            if (grd.DisplayLayout.Bands[0].Columns.Exists(item) && !grd.DisplayLayout.Bands[0].SortedColumns.Contains(item))
                            {
                                grd.DisplayLayout.Bands[0].SortedColumns.Add(item, true, true);
                                if (grd.DisplayLayout.Bands[0].SortedColumns.Count == 1)
                                {
                                    flag = false;
                                }
                                if (flag)
                                {
                                    grd.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                                    flag = false;
                                }
                            }
                        }

                        grd.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                        grd.DisplayLayout.RefreshSummaries();
                    }

                    ColumnFiltersCollection columnFilters = grd.DisplayLayout.Bands[0].ColumnFilters;
                    ///TODO : When we apply the custom filters we need to change the code so the filters won't be on a common field
                    columnFilters.ClearAllFilters();

                    foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
                    {
                        col.Hidden = true;
                    }

                    foreach (CashGridColumn selectedCols in cashManagementLayout.SelectedColumnsCollection)
                    {
                        UltraGridColumn col = null;
                        if (grd.DisplayLayout.Bands[0].Columns.Exists(selectedCols.Name))
                        {
                            col = grd.DisplayLayout.Bands[0].Columns[selectedCols.Name];
                            if (col != null)
                            {
                                col.HiddenWhenGroupBy = DefaultableBoolean.False;
                                col.Hidden = selectedCols.Hidden;
                                col.Header.Fixed = selectedCols.IsHeaderFixed;


                                if (!grd.DisplayLayout.Bands[0].SortedColumns.Contains(col))
                                {
                                    col.SortIndicator = selectedCols.SortIndicator;
                                    if (col.SortIndicator.Equals(SortIndicator.Ascending))

                                        grd.DisplayLayout.Bands[0].SortedColumns.Add(col, false);
                                    if (col.SortIndicator.Equals(SortIndicator.Descending))
                                        grd.DisplayLayout.Bands[0].SortedColumns.Add(col, true);

                                }

                                grd.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                                if (selectedCols.Width == 0)
                                {
                                    col.AutoSizeMode = ColumnAutoSizeMode.Default;
                                }
                                else
                                {
                                    col.Width = selectedCols.Width;
                                }
                                col.Header.VisiblePosition = selectedCols.Position;
                                if (selectedCols.FilterConditionList.Count > 0)
                                {
                                    foreach (FilterCondition filCond in selectedCols.FilterConditionList)
                                    {
                                        if ((selectedCols.Name.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_ENTRYDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_MODIFYDATE)) && selectedCols.FilterConditionList.Count == 1 && selectedCols.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.StartsWith && selectedCols.FilterConditionList[0].CompareValue.Equals("(Today)"))
                                        {
                                            grd.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                                        }
                                        else
                                        {
                                            grd.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(filCond);
                                        }
                                    }
                                    grd.DisplayLayout.Bands[0].ColumnFilters[col].LogicalOperator = selectedCols.FilterLogicalOperator;
                                }
                            }
                        }
                    }

                }
                WireEvents();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the selected grid.
        /// </summary>
        /// <returns></returns>
        private UltraGrid GetSelectedGrid()
        {
            UltraGrid selectedGrid = null;
            try
            {
                if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                    selectedGrid = grdNonTradingTransactions;
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans)
                    selectedGrid = grdDividend;
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageTradingTrans)
                    selectedGrid = grdTradingTransactions;
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageRevaluation)
                    selectedGrid = grdRevaluation;
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                    selectedGrid = grdOpeningBalance;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return selectedGrid;
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        /// <param name="setForAll">if set to <c>true</c> [set for all].</param>
        private void BindGrid(bool setForAll = true)
        {
            try
            {
                lock (_locker)
                {
                    if (!this.IsDisposed)
                    {
                        InitializeValueList();

                        #region Trading Transactions

                        if (setForAll || ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageTradingTrans)
                        {
                            grdTradingTransactions.DataSource = TradingTransactionEntries;

                            HelperClass.SetColumnDisplayNames(grdTradingTransactions, null);
                            HelperClass.GridSettingForJournalLook(grdTradingTransactions);
                            SetTransactionSourceCaption(grdTradingTransactions, 0);
                        }
                        #endregion

                        #region Revaluation Transactions

                        if (setForAll || ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageRevaluation)
                        {
                            grdRevaluation.DataSource = RevaluationTransactionEntries;
                            HelperClass.SetColumnDisplayNames(grdRevaluation, null);
                            HelperClass.GridSettingForJournalLook(grdRevaluation);
                            SetTransactionSourceCaption(grdRevaluation, 1);
                        }
                        #endregion

                        #region Non Trading Transactions

                        List<string> lsColumnsToDisplay = new List<string>(new string[] { "TransactionDate", "AccountName", "Symbol", "CurrencyName", "SubAcName", "EntryAccountSide", "DR", "CR", "Description" });
                        if (setForAll || ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                        {
                            grdNonTradingTransactions.DataSource = _lsNonTradingTransactionEntries;
                            HelperClass.SetColumnDisplayNames(grdNonTradingTransactions, lsColumnsToDisplay);
                            HelperClass.GridSettingForJournalLook(grdNonTradingTransactions);
                            BindValueListToGrid(grdNonTradingTransactions, 1);
                            SetTransactionSourceCaption(grdNonTradingTransactions, 2);
                        }
                        #endregion

                        #region Opening Balance Transactions

                        if (setForAll || ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                        {
                            grdOpeningBalance.DataSource = _lsOtherJournalTransactionEntries;
                            HelperClass.SetColumnDisplayNames(grdOpeningBalance, lsColumnsToDisplay);
                            HelperClass.GridSettingForJournalLook(grdOpeningBalance);
                            BindValueListToGrid(grdOpeningBalance, 2);
                            SetTransactionSourceCaption(grdOpeningBalance, 3);
                        }
                        #endregion

                        #region Dividend Transactions

                        if (setForAll || ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans)
                        {
                            grdDividend.DataSource = DividendTransactionEntries;
                            HelperClass.SetColumnDisplayNames(grdDividend, null);
                            HelperClass.GridSettingForJournalLook(grdDividend);
                            SetTransactionSourceCaption(grdDividend, 4);
                        }
                        #endregion

                        clearData();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is used to bind value lists to the grids
        /// </summary>
        /// <param name="reportGrid">The report grid.</param>
        /// <param name="gridNo">The grid no.</param>
        private void BindValueListToGrid(UltraGrid reportGrid, int gridNo)
        {
            try
            {
                _subAccount[gridNo].SortStyle = ValueListSortStyle.Ascending;
                _accountValList[gridNo].SortStyle = ValueListSortStyle.Ascending;
                if (reportGrid.DataSource != null)
                {
                    UltraGridBand band;
                    if (reportGrid.DisplayLayout.Bands.Exists("TransactionEntries"))
                    {
                        band = reportGrid.DisplayLayout.Bands["TransactionEntries"];
                    }
                    else
                    {
                        band = reportGrid.DisplayLayout.Bands[0];
                    }
                    band.Columns["AccountName"].ValueList = _accountValList[gridNo];
                    band.Columns["SubAcName"].ValueList = _subAccount[gridNo];
                    band.Columns["CurrencyName"].ValueList = _currencyValList[gridNo];
                    //FXConversionMethodOperator value list is needed only for opening balance UI and non trade transaction UI
                    if (gridNo == 1 || gridNo == 2)
                    {
                        band.Columns["EntryAccountSide"].ValueList = _AccountSideValList[gridNo - 1];
                        band.Columns["FXConversionMethodOperator"].ValueList = fxConversionMethodOperatorValList[gridNo - 1];
                    }
                    band.Columns["AccountName"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    band.Columns["SubAcName"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    band.Columns["SubAcName"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                    band.Columns["SubAcName"].AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                    band.Columns["CurrencyName"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    band.Columns["EntryAccountSide"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    band.Columns["FXConversionMethodOperator"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Mouses the click comman for sort.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void MouseClickCommanForSort(object sender, MouseEventArgs e)
        {
            try
            {
                UltraGrid grid = sender as UltraGrid;
                UIElement controlElement = grid.DisplayLayout.UIElement;
                UIElement elementAtPoint = controlElement != null ? controlElement.ElementFromPoint(e.Location) : null;
                while (elementAtPoint != null)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridUIElement uiElement = elementAtPoint.ControlElement as Infragistics.Win.UltraWinGrid.UltraGridUIElement;
                    HeaderUIElement headerElement = uiElement.ElementWithMouseCapture as HeaderUIElement;
                    if (headerElement != null && headerElement.Header is Infragistics.Win.UltraWinGrid.ColumnHeader)
                    {
                        _columnSorted = headerElement.GetContext(typeof(UltraGridColumn)) as UltraGridColumn;
                        break;
                    }
                    elementAtPoint = elementAtPoint.Parent;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Mouses down comman for sort.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <param name="grd">The GRD.</param>
        private void MouseDownCommanForSort(MouseEventArgs e, UltraGrid grd)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Get a reference to the UIElement at the current mouse position
                    UIElement thisElem = grd.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
                    // Exit the event handler if no UIElement is found
                    if (thisElem == null)
                        return;
                    //See if the UIElement at the current mouse position is a GroupByBoxUIElement,
                    // or if it is contained as a child of a GroupByBoxUIElement
                    if (thisElem is GroupByBoxUIElement ||
                        thisElem.GetAncestor(typeof(GroupByBoxUIElement)) is GroupByBoxUIElement)
                    {
                        string columnSortedName = string.Empty;
                        if (thisElem is Infragistics.Win.TextUIElement)
                        {
                            columnSortedName = ((Infragistics.Win.TextUIElement)(thisElem)).Text;
                        }
                        else if (thisElem is Infragistics.Win.UltraWinGrid.SortIndicatorUIElement)
                        {
                            columnSortedName = ((Infragistics.Win.UltraWinGrid.SortIndicatorUIElement)(thisElem)).ToString();
                        }
                        else if (thisElem is GroupByButtonUIElement)
                        {
                            foreach (object thisElemChild in thisElem.ChildElements)
                            {
                                if (thisElemChild is Infragistics.Win.TextUIElement)
                                {
                                    columnSortedName = ((Infragistics.Win.TextUIElement)(thisElemChild)).Text;
                                    break;
                                }
                            }
                        }

                        for (int counter = 0; counter < grd.DisplayLayout.Bands[0].SortedColumns.Count; counter++)
                        {
                            if (columnSortedName.Equals(grd.DisplayLayout.Bands[0].SortedColumns[counter].Header.Caption))
                            {
                                _columnSorted = grd.DisplayLayout.Bands[0].SortedColumns[grd.DisplayLayout.Bands[0].SortedColumns[counter].Key];
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Rows the summary settings.
        /// </summary>
        /// <param name="grd">The GRD.</param>
        private void RowSummarySettings(UltraGrid grd)
        {
            try
            {
                bool groupedBySomeColumn = false;
                foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (col.IsGroupByColumn)
                    {
                        groupedBySomeColumn = true;
                        break;
                    }
                }
                if (!groupedBySomeColumn)
                {
                    grd.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the layout for all cash journal grids.
        /// </summary>
        /// <param name="setLayoutForAll">if set to <c>true</c> [set layout for all].</param>
        public void SetLayoutForAllCashJournalGrids(bool setLayoutForAll = true)
        {
            if (setLayoutForAll.Equals(true))
            {
                if (cashJournalLayout.ContainsKey(Trading))
                {
                    InitializeGridLayout(cashJournalLayout[Trading], grdTradingTransactions);
                }
                if (cashJournalLayout.ContainsKey(NonTrading))
                {
                    InitializeGridLayout(cashJournalLayout[NonTrading], grdNonTradingTransactions);
                }
                if (cashJournalLayout.ContainsKey(Dividend))
                {
                    InitializeGridLayout(cashJournalLayout[Dividend], grdDividend);
                }
                if (cashJournalLayout.ContainsKey(Revaluation))
                {
                    InitializeGridLayout(cashJournalLayout[Revaluation], grdRevaluation);
                }
                if (cashJournalLayout.ContainsKey(OpeningBalance))
                {
                    InitializeGridLayout(cashJournalLayout[OpeningBalance], grdOpeningBalance);
                }
            }
            else
            {
                if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageTradingTrans && cashJournalLayout.ContainsKey(Trading))
                {
                    InitializeGridLayout(cashJournalLayout[Trading], grdTradingTransactions);
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans && cashJournalLayout.ContainsKey(NonTrading))
                {
                    InitializeGridLayout(cashJournalLayout[NonTrading], grdNonTradingTransactions);
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans && cashJournalLayout.ContainsKey(Dividend))
                {
                    InitializeGridLayout(cashJournalLayout[Dividend], grdDividend);
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageRevaluation && cashJournalLayout.ContainsKey(Revaluation))
                {
                    InitializeGridLayout(cashJournalLayout[Revaluation], grdRevaluation);
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance && cashJournalLayout.ContainsKey(OpeningBalance))
                {
                    InitializeGridLayout(cashJournalLayout[OpeningBalance], grdOpeningBalance);
                }
            }
        }

        /// <summary>
        /// Sorts the name of the grid by column.
        /// </summary>
        /// <param name="grd">The GRD.</param>
        private void SortGridByColumnName(UltraGrid grd)
        {
            try
            {
                int sortCount = grd.DisplayLayout.Bands[0].SortedColumns.Count;
                if (sortCount > 0)
                {
                    //Correction made as it was not returning the column that has been sorted.
                    //Now the sorted column will be detected by mouse click event on column header.
                    UltraGridColumn sortColumn;
                    if (grd.DisplayLayout.Bands[0].SortedColumns.Contains(_columnSorted))
                    {
                        sortColumn = grd.DisplayLayout.Bands[0].SortedColumns[_columnSorted.Key];
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grd.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                        //Set the Group by row summary display settings 
                        RowSummarySettings(grd);
                        return;
                    }
                    if (sortColumn.Formula != null && !(sortColumn.DataType.Equals(typeof(System.Double))))
                    {
                        //Set the Group by row summary display settings 
                        RowSummarySettings(grd);
                        return;
                    }

                    if (!sortColumn.IsGroupByColumn && !GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _groupSortComparer.Column = sortColumn.Key;
                        _groupSortComparer.SortIndicator = sortColumn.SortIndicator;
                        foreach (UltraGridColumn var in grd.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                            if (var.IsGroupByColumn)
                            {
                                if (var.SortIndicator == SortIndicator.Descending)
                                {
                                    if (_groupSortComparer.SortIndicator == SortIndicator.Ascending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Descending;
                                    }
                                    else if (_groupSortComparer.SortIndicator == SortIndicator.Descending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Ascending;
                                    }
                                }
                                var.GroupByComparer = _groupSortComparer;
                            }
                        }
                        sortColumn.GroupByComparer = _groupSortComparer;
                    }
                    else if (sortColumn.IsGroupByColumn && GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _groupSortComparer.Column = sortColumn.Key;
                        _groupSortComparer.SortIndicator = sortColumn.SortIndicator;
                        foreach (UltraGridColumn var in grd.DisplayLayout.Bands[0].SortedColumns)
                        {
                            if (var.IsGroupByColumn)
                            {
                                if (var.SortIndicator == SortIndicator.Descending)
                                {
                                    if (_groupSortComparer.SortIndicator == SortIndicator.Ascending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Descending;
                                    }
                                    else if (_groupSortComparer.SortIndicator == SortIndicator.Descending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Ascending;
                                    }
                                }
                                var.GroupByComparer = _groupSortComparer;
                            }
                        }
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grd.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                    }
                    grd.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                }
                RowSummarySettings(grd);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the underlying object with un commited changes.
        /// </summary>
        private void UpdateUnderlyingObjWithUnCommitedChanges()
        {
            if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans
                && grdNonTradingTransactions.ActiveCell != null
                && grdNonTradingTransactions.ActiveCell.IsInEditMode)
            {
                grdNonTradingTransactions.ActiveCell.Row.Update();
            }
            else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance
                             && grdOpeningBalance.ActiveCell != null
                             && grdOpeningBalance.ActiveCell.IsInEditMode)
            {
                grdOpeningBalance.ActiveCell.Row.Update();
            }
            else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans
                && grdDividend.ActiveCell != null
                && grdDividend.ActiveCell.IsInEditMode)
            {
                grdDividend.ActiveCell.Row.Update();
            }
        }
        #endregion

        #region Transaction Changes

        /// <summary>
        /// Adds the original transaction.
        /// </summary>
        /// <param name="tranEntry">The tran entry.</param>
        private void AddOriginalTransaction(TransactionEntry tranEntry)
        {
            try
            {
                if (!_newlyAddedTransactions.Contains(tranEntry.TransactionEntryID) && !_originalTransactions.ContainsKey(tranEntry.TransactionEntryID))
                    _originalTransactions.Add(tranEntry.TransactionEntryID, tranEntry.Clone());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the audit trail list.
        /// </summary>
        /// <param name="modifiedTransactionEntries">The modified transaction entries.</param>
        /// <param name="comments">The comments.</param>
        /// <returns></returns>
        private List<CashJournalAuditEntry> GetAuditTrailList(List<TransactionEntry> modifiedTransactionEntries, string comments)
        {
            List<CashJournalAuditEntry> lstCashJournalAudit = new List<CashJournalAuditEntry>();
            try
            {
                int userId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                modifiedTransactionEntries.ForEach(newEntry =>
                    {
                        string tranId = newEntry.TransactionEntryID;
                        CashJournalAuditEntry newAuditEntry = new CashJournalAuditEntry(newEntry.TransactionDate, newEntry.CurrencyName, newEntry.AccountID, comments, userId, newEntry.SubAcID, newEntry.Symbol, newEntry.CR, newEntry.DR, newEntry.FxRate);
                        if (_originalTransactions.ContainsKey(tranId))
                        {
                            TransactionEntry origEntry = _originalTransactions[tranId];
                            CashJournalAuditEntry origAuditEntry = new CashJournalAuditEntry(origEntry.TransactionDate, origEntry.CurrencyName, origEntry.AccountID, comments, userId, origEntry.SubAcID, origEntry.Symbol, origEntry.CR, origEntry.DR, origEntry.FxRate);
                            if (newEntry.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                            {
                                origAuditEntry.Comments += ": Transaction deleted";
                                lstCashJournalAudit.Add(origAuditEntry);
                            }
                            else
                            {
                                origAuditEntry.Comments += ": Before transaction modified";
                                lstCashJournalAudit.Add(origAuditEntry);
                                newAuditEntry.Comments += ": After transaction modified";
                                lstCashJournalAudit.Add(newAuditEntry);
                            }
                        }
                        else
                        {
                            newAuditEntry.Comments += ": Transaction added";
                            lstCashJournalAudit.Add(newAuditEntry);
                        }
                    });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lstCashJournalAudit;
        }

        /// <summary>
        /// Adds the new transcation.
        /// </summary>
        private void AddNewTranscation()
        {
            try
            {
                isNewRowAdded = true;
                Transaction newTranscation = new Transaction();
                TransactionEntry firstTransactionEntry = CreateNewTransactionEntry();
                firstTransactionEntry.EntryAccountSide = AccountSide.DR;

                firstTransactionEntry.TransactionID = firstTransactionEntry.TransactionEntryID;

                TransactionEntry SecondTransactionEntry = CreateNewTransactionEntry();
                SecondTransactionEntry.EntryAccountSide = AccountSide.CR;

                if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                {
                    firstTransactionEntry.TransactionSource = CashTransactionType.ManualJournalEntry;
                    SecondTransactionEntry.TransactionSource = CashTransactionType.ManualJournalEntry;
                }

                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                {
                    firstTransactionEntry.TransactionSource = CashTransactionType.OpeningBalance;
                    SecondTransactionEntry.TransactionSource = CashTransactionType.OpeningBalance;
                }
                newTranscation.TransactionID = firstTransactionEntry.TransactionEntryID;
                newTranscation.Add(firstTransactionEntry);
                newTranscation.Add(SecondTransactionEntry);

                //Only opening balance will be added from cash journal UI
                if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                {
                    _lsOtherJournalTransactions.Add(newTranscation);
                    AddTransactionEntries(newTranscation, _lsOtherJournalTransactionEntries);
                    grdOpeningBalance.Rows[0].ExpandAll();
                    grdOpeningBalance.ActiveRow = grdOpeningBalance.Rows[0];
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                {
                    _lsNonTradingTransactions.Add(newTranscation);
                    AddTransactionEntries(newTranscation, _lsNonTradingTransactionEntries);
                    grdNonTradingTransactions.Rows[0].ExpandAll();
                    grdNonTradingTransactions.ActiveRow = grdNonTradingTransactions.Rows[0];

                }
                lsModifiedTransactions.Add(newTranscation);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Adds the new transcaton entry.
        /// </summary>
        private void AddNewTranscatonEntry()
        {
            try
            {

                isNewRowAdded = true;
                Transaction objSelectedTranscation = GetSelectedTransaction();

                if (objSelectedTranscation != null && objSelectedTranscation.TransactionID != "")
                {
                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(objSelectedTranscation.Date))
                    {
                        MessageBox.Show("The date  of the some or all entries you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                            + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    TransactionEntry trEntry = CreateNewTransactionEntry();
                    //for all transaction entries of a transaction account and transaction date will be same.
                    if (objSelectedTranscation.TransactionEntries.Count > 0)
                    {
                        trEntry.AccountID = objSelectedTranscation.TransactionEntries[0].AccountID;
                        trEntry.AccountName = objSelectedTranscation.TransactionEntries[0].AccountName;
                        trEntry.TransactionDate = objSelectedTranscation.TransactionEntries[0].TransactionDate;

                        //PRANA-9777
                        trEntry.EntryDate = objSelectedTranscation.TransactionEntries[0].EntryDate;

                        //PRANA-9776
                        foreach (TransactionEntry tr in objSelectedTranscation.TransactionEntries)
                        {
                            tr.ModifyDate = DateTime.Now;
                            trEntry.UserId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                            trEntry.UserName = CachedDataManager.GetInstance.LoggedInUser.ShortName;
                        }

                        #region added by Bharat raturi
                        //added by: Bharat Raturi, 12-sep-14
                        if (!string.IsNullOrWhiteSpace(objSelectedTranscation.TransactionEntries[0].ActivityId_FK))
                        {
                            trEntry.ActivityId_FK = objSelectedTranscation.TransactionEntries[0].ActivityId_FK;
                        }
                        if (!string.IsNullOrWhiteSpace(objSelectedTranscation.TransactionEntries[0].Symbol))
                        {
                            trEntry.Symbol = objSelectedTranscation.TransactionEntries[0].Symbol;
                        }
                        #endregion

                        if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                            trEntry.TransactionSource = CashTransactionType.ManualJournalEntry;
                        else
                            trEntry.TransactionSource = CashTransactionType.OpeningBalance;
                    }
                    objSelectedTranscation.Add(trEntry);
                    if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance && !_lsOtherJournalTransactionEntries.Contains(trEntry))
                        _lsOtherJournalTransactionEntries.Add(trEntry);
                    else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans && !_lsNonTradingTransactionEntries.Contains(trEntry))
                        _lsNonTradingTransactionEntries.Add(trEntry);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Adds the transaction entries.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destinationList">The destination list.</param>
        private void AddTransactionEntries(Transaction source, GenericBindingList<TransactionEntry> destinationList)
        {
            try
            {
                if (source != null)
                {
                    //Narendra Kumar Jangir May 10 2013
                    //Get Grouped Transaction entries on the basis of TransactionId and SubAccountId
                    Dictionary<string, TransactionEntry> dictGroupedTransactionEntry = GroupTransactionEntries(source);

                    //Code to bind Transaction Entries to ui instead of Transactions
                    foreach (KeyValuePair<string, TransactionEntry> kvpTransactionEntry in dictGroupedTransactionEntry)
                    {
                        //PRANA-27448
                        if (kvpTransactionEntry.Value.TransactionSource == CashTransactionType.ImportedEditableData)
                        {
                            kvpTransactionEntry.Value.TaxLotState = ApplicationConstants.TaxLotState.NotChanged;
                        }

                        if (!destinationList.Contains(kvpTransactionEntry.Value))
                            destinationList.Add(kvpTransactionEntry.Value);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Assigns the database transactions to respective lists.
        /// </summary>
        private void AssignDBTransactionsToRespectiveLists()
        {
            lock (_locker)
            {
                clearData();
                if (lsTransactionsFromDB != null)
                {
                    foreach (Transaction t in lsTransactionsFromDB)
                    {
                        if ((t.GetActivitySource().Equals((byte)ActivitySource.Trading)) && t.TransactionID != null)
                        {
                            TradingTransactions.Add(t);
                            AddTransactionEntries(t, TradingTransactionEntries);
                        }
                        else if ((t.GetActivitySource().Equals((byte)ActivitySource.Dividend)) && t.TransactionID != null)
                        {
                            DividendTransactions.Add(t);
                            AddTransactionEntries(t, DividendTransactionEntries);
                        }
                        else if ((t.GetActivitySource().Equals((byte)ActivitySource.NonTrading) && t.TransactionID != null))
                        {
                            _lsNonTradingTransactions.Add(t);
                            AddTransactionEntries(t, _lsNonTradingTransactionEntries);
                        }
                        else if ((t.GetActivitySource().Equals((byte)ActivitySource.Revaluation)) && t.TransactionID != null)
                        {
                            RevaluationTransactions.Add(t);
                            AddTransactionEntries(t, RevaluationTransactionEntries);
                        }
                        else if ((t.GetActivitySource().Equals((byte)ActivitySource.OpeningBalance) && t.TransactionID != null))
                        {
                            _lsOtherJournalTransactions.Add(t);
                            AddTransactionEntries(t, _lsOtherJournalTransactionEntries);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates the new transaction entry.
        /// </summary>
        /// <returns></returns>
        private TransactionEntry CreateNewTransactionEntry()
        {
            TransactionEntry newTransactionEntry = new TransactionEntry();
            try
            {
                newTransactionEntry.TransactionEntryID = CashDataManager.GetInstance().GenerateTransactionEntryID();

                //For non trading transactions unique key will be transactionEntryid
                //newTransactionEntry.UniqueKey = newTransactionEntry.TransactionEntryID;
                newTransactionEntry.TaxLotState = ApplicationConstants.TaxLotState.New;
                newTransactionEntry.EntryAccountSide = AccountSide.DR;
                newTransactionEntry.FXConversionMethodOperator = Operator.M.ToString();
                //Non trade transaction and dividend will not be added from cash journal UI

                //if (ultraTabControlCashMainValue.SelectedTab.TabPage == ulTabPageConlNonTradingTrans)
                //    newTransactionEntry.TransactionSource = TransactionSource.ManualEntry;
                //else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ulTabPageConlDividendTrans)
                //    newTransactionEntry.TransactionSource = TransactionSource.CashTransaction;

                if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                {
                    newTransactionEntry.TransactionSource = CashTransactionType.OpeningBalance;
                    newTransactionEntry.ActivitySource = ActivitySource.OpeningBalance;
                }
                _newlyAddedTransactions.Add(newTransactionEntry.TransactionEntryID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return newTransactionEntry;
        }

        /// <summary>
        /// Deletes the transaction.
        /// </summary>
        private void DeleteTransaction()
        {
            try
            {
                //Added by: Bharat Raturi, 09-Sep-2014
                //Keep the state of the transaction taxlot in a variable so that the fresh transactions can be checked for while deletion  
                bool isInvalid = false;
                ApplicationConstants.TaxLotState trTaxlotState = ApplicationConstants.TaxLotState.NotChanged;

                Transaction selectedTransaction = GetSelectedTransaction();
                if (selectedTransaction != null)
                {
                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(selectedTransaction.Date))
                    {
                        MessageBox.Show("The date  of the some or all entries you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                            + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //Added by: Bharat Raturi, 09-Sep-2014
                    //Get the state of the transaction taxlot
                    trTaxlotState = (ApplicationConstants.TaxLotState)Enum.Parse(typeof(ApplicationConstants.TaxLotState), selectedTransaction.GetTaxlotState());

                    if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                    {
                        _lsOtherJournalTransactions.Remove(selectedTransaction);
                        RemoveTransactionEntries(selectedTransaction, _lsOtherJournalTransactionEntries);
                    }
                    if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                    {
                        _lsNonTradingTransactions.Remove(selectedTransaction);
                        RemoveTransactionEntries(selectedTransaction, _lsNonTradingTransactionEntries);
                    }

                    foreach (TransactionEntry transactionEntryToRemove in selectedTransaction.TransactionEntries)
                    {
                        transactionEntryToRemove.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                        AddOriginalTransaction(transactionEntryToRemove);
                    }

                    //Added by: Bharat Raturi, 09-Sep-2014
                    //Delete the fresh transaction from the list of the modified transactions
                    if (selectedTransaction.GetTaxlotState() == ApplicationConstants.TaxLotState.Deleted.ToString() && trTaxlotState == ApplicationConstants.TaxLotState.New)
                    {
                        if (lsModifiedTransactions.Exists(delegate (Transaction tr) { return tr.TransactionID == selectedTransaction.TransactionID; }))
                        {
                            lsModifiedTransactions.Remove(selectedTransaction);
                            isInvalid = true;
                        }
                    }
                    if (!lsModifiedTransactions.Exists(delegate (Transaction tr) { return tr.TransactionID == selectedTransaction.TransactionID; }) && !isInvalid)
                    {
                        lsModifiedTransactions.Add(selectedTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Deletes the transcation entry.
        /// </summary>
        private void DeleteTranscationEntry()
        {
            try
            {
                Transaction selectedTransaction = GetSelectedTransaction();
                if (selectedTransaction != null && selectedTransaction.TransactionID != "")
                {
                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(selectedTransaction.Date))
                    {
                        MessageBox.Show("The date  of the some or all entries you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                            + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    TransactionEntry selectedTranscationEntry = GetSelectedTransactionEntry();
                    selectedTransaction.Remove(selectedTranscationEntry);
                    if (_lsOtherJournalTransactionEntries.Contains(selectedTranscationEntry))
                        _lsOtherJournalTransactionEntries.Remove(selectedTranscationEntry);

                    else if (_lsNonTradingTransactionEntries.Contains(selectedTranscationEntry))
                        _lsNonTradingTransactionEntries.Remove(selectedTranscationEntry);

                    if (!lsDeletedTransactionEntry.Contains(selectedTranscationEntry))
                    {
                        selectedTranscationEntry.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                        selectedTranscationEntry.Errors.Clear();
                        lsDeletedTransactionEntry.Add(selectedTranscationEntry);
                    }
                    if (!lsModifiedTransactions.Exists(delegate (Transaction tr) { return tr.TransactionID == selectedTransaction.TransactionID; }))
                    {
                        lsModifiedTransactions.Add(selectedTransaction);
                    }
                    AddOriginalTransaction(selectedTranscationEntry);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Gets the selected transaction.
        /// </summary>
        /// <returns></returns>
        private Transaction GetSelectedTransaction()
        {
            Transaction selectedTransaction = null;
            try
            {
                UltraGrid selectedGrid = null;
                GenericBindingList<Transaction> selectedList = null;

                if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans) && (grdNonTradingTransactions.ActiveRow != null))
                {
                    selectedGrid = grdNonTradingTransactions;
                    selectedList = _lsNonTradingTransactions;
                }
                else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans) && (grdDividend.ActiveRow != null))
                {
                    selectedGrid = grdDividend;
                    selectedList = DividendTransactions;
                }
                else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance) && (grdOpeningBalance.ActiveRow != null))
                {
                    selectedGrid = grdOpeningBalance;
                    selectedList = _lsOtherJournalTransactions;
                }

                if (selectedGrid != null && selectedList != null)
                {
                    if (selectedGrid.ActiveRow.ListObject is TransactionEntry)
                    {
                        //TODO: SUrendra: Analyze the logic
                        string key = string.Empty;
                        String key2 = string.Empty;
                        if ((selectedGrid.ActiveRow.ListObject as TransactionEntry).ActivityId_FK != null)
                        {
                            key = (selectedGrid.ActiveRow.ListObject as TransactionEntry).ActivityId_FK + (selectedGrid.ActiveRow.ListObject as TransactionEntry).TransactionNumber.ToString();
                            key2 = (selectedGrid.ActiveRow.ListObject as TransactionEntry).TransactionID;
                        }
                        else
                            key = (selectedGrid.ActiveRow.ListObject as TransactionEntry).TransactionID;

                        if (selectedList.GetItem(key) != null)
                            selectedTransaction = selectedList.GetItem(key);
                        else if (!String.IsNullOrEmpty(key2))
                            selectedTransaction = selectedList.GetItem(key2);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return selectedTransaction;
        }

        /// <summary>
        /// Gets the selected transaction entry.
        /// </summary>
        /// <returns></returns>
        private TransactionEntry GetSelectedTransactionEntry()
        {
            TransactionEntry selectedTransactionEntry = new TransactionEntry();
            try
            {
                UltraGrid selectedGrid = null;
                if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans) && (grdNonTradingTransactions.ActiveRow != null))
                {
                    selectedGrid = grdNonTradingTransactions;
                }
                else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans) && (grdDividend.ActiveRow != null))
                {
                    selectedGrid = grdDividend;
                }
                else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance) && (grdOpeningBalance.ActiveRow != null))
                {
                    selectedGrid = grdOpeningBalance;
                }
                if (selectedGrid != null)
                {
                    if (selectedGrid.ActiveRow.ListObject is TransactionEntry)
                    {
                        selectedTransactionEntry = selectedGrid.ActiveRow.ListObject as TransactionEntry;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return selectedTransactionEntry;
        }

        /// <summary>
        /// Narendra Kumar Jangir May 10 2013
        /// Group Grouped Transaction Entries on the basis of TransactionId and SubAccountId
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        private Dictionary<string, TransactionEntry> GroupTransactionEntries(Transaction source)
        {
            Dictionary<string, TransactionEntry> dictGroupedTransactionEntry = new Dictionary<string, TransactionEntry>();
            try
            {
                foreach (TransactionEntry trEntry in source.TransactionEntries)
                {
                    //modified by: Bharat raturi
                    //Ignore grouping because it is creating issue while editing transactions. Moreover, it is not required. 
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-3618
                    //group by transactionid, subaccountid, currency
                    //string keyToGroup = trEntry.TransactionID + trEntry.SubAcID + trEntry.CurrencyID+trEntry.EntryAccountSide;
                    string keyToGroup = trEntry.TransactionEntryID.ToString();
                    if (dictGroupedTransactionEntry.ContainsKey(keyToGroup))
                    {
                        dictGroupedTransactionEntry[keyToGroup].CR += trEntry.CR;
                        dictGroupedTransactionEntry[keyToGroup].DR += trEntry.DR;
                    }
                    else
                    {
                        dictGroupedTransactionEntry.Add(keyToGroup, trEntry);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dictGroupedTransactionEntry;
        }

        /// <summary>
        /// Removes the transaction entries.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destinationList">The destination list.</param>
        private void RemoveTransactionEntries(Transaction source, GenericBindingList<TransactionEntry> destinationList)
        {
            try
            {
                if (source != null)
                {
                    List<TransactionEntry> lsTrEntryToDelete;
                    if (source.ActivityId_FK == null)
                        lsTrEntryToDelete = destinationList.GetList().FindAll(delegate (TransactionEntry trEntry) { return trEntry.TransactionID == source.TransactionID; });
                    else
                        lsTrEntryToDelete = destinationList.GetList().FindAll(delegate (TransactionEntry trEntry) { return (trEntry.ActivityId_FK == source.ActivityId_FK && trEntry.TransactionNumber == source.TransactionNumber); });
                    //Code to bind Transaction Entries to ui instead of Transactions
                    foreach (TransactionEntry trEntry in lsTrEntryToDelete)
                        if (destinationList.Contains(trEntry))
                            destinationList.Remove(trEntry);


                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the errors for new transaction entry.
        /// </summary>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        private void setErrorsForNewTransactionEntry(InitializeRowEventArgs e)
        {
            try
            {
                TransactionEntry newTransactionEntry = e.Row.ListObject as TransactionEntry;
                foreach (UltraGridCell currentCell in e.Row.Cells)
                {
                    if (currentCell.Column.Key != "DR" && currentCell.Column.Key != "CR" && currentCell.Column.Key != "Symbol")
                        newTransactionEntry.properityChanged(currentCell.Column.Key, currentCell.Text);
                    if (currentCell.Column.Key == "Symbol" && ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans)
                        newTransactionEntry.properityChanged(currentCell.Column.Key, currentCell.Text);
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

        /// <summary>
        /// Updates the list of modified transactions.
        /// </summary>
        private void UpdateListOfModifiedTransactions()
        {
            try
            {
                Transaction objTransaction = GetSelectedTransaction();
                if (objTransaction != null)
                    if (!lsModifiedTransactions.Exists(delegate (Transaction tr) { return tr.TransactionID == objTransaction.TransactionID; }))
                        lsModifiedTransactions.Add(objTransaction);
                    else
                    {
                        //Remove Transaction if it is not updated
                        int count = 0;
                        if (_originalTransactions.Count > 0)
                        {
                            objTransaction.TransactionEntries.ToList().ForEach(entry =>
                                {
                                    if (_originalTransactions.ContainsKey(entry.TransactionEntryID))
                                    {
                                        TransactionEntry orig = _originalTransactions[entry.TransactionEntryID];
                                        if (orig != null)
                                        {
                                            if (orig.TransactionDate == entry.TransactionDate && orig.AccountID == entry.AccountID && orig.Symbol == entry.Symbol && orig.CurrencyID == entry.CurrencyID && orig.SubAcID == entry.SubAcID && orig.CR == entry.CR && orig.DR == entry.DR && orig.Description == entry.Description && orig.FxRate == entry.FxRate && orig.FXConversionMethodOperator == entry.FXConversionMethodOperator)
                                            {
                                                count++;
                                            }
                                        }
                                    }
                                });
                            if (count == objTransaction.TransactionEntries.Count)
                                lsModifiedTransactions.Remove(objTransaction);
                        }
                    }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
        }
        #endregion

        #region UI Actions Section

        /// <summary>
        /// Handles the Click event of the addRowToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewTranscatonEntry();
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

        /// <summary>
        /// Handles the Click event of the addTranscationToolStripMenuitem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void addTranscationToolStripMenuitem_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewTranscation();
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

        /// <summary>
        /// Handles the DoWork event of the bgwrkr control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        void bgwrkr_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];
                getData((string)arguments[2], (string)arguments[3]);
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

        /// <summary>
        /// Handles the RunWorkerCompleted event of the bgwrkr control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        void bgwrkr_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                changeStatusMsg(true);
                AssignDBTransactionsToRespectiveLists();
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

        /// <summary>
        /// Handles the Click event of the btnExport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExportToExcel())
                {
                    MessageBox.Show("Report Successfully saved.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Handles the Click event of the btnGetCash control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnGetCash_Click(object sender, EventArgs e)
        {
            try
            {
                string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                {
                    if (DateTime.Compare(Convert.ToDateTime(dtPickerUpper.DateTime), Convert.ToDateTime(dtPickerlower.DateTime)) >= 0)
                    {
                        if (ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetNoOfCheckedItems() == 0)
                        {
                            MessageBox.Show("Select at least one account to proceed.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        GetDataInBackGround();
                        BindGrid(false);
                        SetLayoutForAllCashJournalGrids(false);
                    }
                    else
                        MessageBox.Show("To Date is before From Date", "Cash Journal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            #region added by Bharat raturi
            StringBuilder _ErrorDetails = new StringBuilder(string.Empty);
            #endregion

            bool isErrorsExists = false;
            isNewRowAdded = false;
            bool isValidEntry = true; //PRANA-6321
            try
            {
                UpdateUnderlyingObjWithUnCommitedChanges();
                List<string> navLockedAccounts = new List<string>();
                foreach (var tran in lsModifiedTransactions)
                {
                    if (tran.TransactionEntries.Count > 0)
                    {
                        if (CachedDataManager.GetInstance.NAVLockDate.HasValue)
                        {
                            if ((_originalTransactions.ContainsKey(tran.TransactionEntries[0].TransactionEntryID) 
                                && !CachedDataManager.GetInstance.ValidateNAVLockDate(_originalTransactions[tran.TransactionEntries[0].TransactionEntryID].TransactionDate))
                                || !CachedDataManager.GetInstance.ValidateNAVLockDate(tran.TransactionEntries[0].TransactionDate))
                            {
                                MessageBox.Show("The date of the some or all entries you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                    + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        NAVLockItem accountNAVlockDetail = NAVLockManager.GetInstance.getNAVLockItemDetails(tran.TransactionEntries[0].AccountID);
                        //Modified by: Bharat raturi,
                        //Applied the null check on accountNAVlockDetail. it would be null if there is a blank transaction or no account is selected for the transaction
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6691
                        if (accountNAVlockDetail != null && Enum.IsDefined(typeof(Utilities.UI.CronUtility.ScheduleType), accountNAVlockDetail.LockSchedule))
                        {
                            if (accountNAVlockDetail.LastLockDate.Date != DateTime.MinValue.Date)
                            {
                                if (!navLockedAccounts.Contains(tran.TransactionEntries[0].AccountName))
                                {
                                    navLockedAccounts.Add(tran.TransactionEntries[0].AccountName);
                                }
                            }
                        }
                    }
                }
                if (navLockedAccounts.Count > 0)
                {
                    MessageBox.Show("NAV is locked for the following accounts :" + string.Join(", ", navLockedAccounts) + ".", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    navLockedAccounts.Clear();
                }
                else
                {
                    foreach (Transaction tran in lsModifiedTransactions)
                    {
                        if (tran.TransactionEntries.Count > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(tran.TransactionEntries[0].AccountID.ToString())
                                && !string.IsNullOrWhiteSpace(tran.TransactionEntries[0].TransactionDate.ToString()))
                            {
                                if (CashDataManager.GetInstance().GetCashPreferences(tran.TransactionEntries[0].AccountID) != null)
                                {
                                    DateTime accountCashMgmtStartDate = CashDataManager.GetInstance().GetCashPreferences(tran.TransactionEntries[0].AccountID).CashMgmtStartDate;

                                    //Changed by Nishant Jain [2015-02-23]
                                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-6321 
                                    if (tran.TransactionEntries[0].TransactionSource.Equals(CashTransactionType.OpeningBalance))
                                    {
                                        if (accountCashMgmtStartDate.Date > tran.TransactionEntries[0].TransactionDate.Date)
                                            isValidEntry = false;
                                    }
                                    else
                                    {
                                        if (accountCashMgmtStartDate.Date >= tran.TransactionEntries[0].TransactionDate.Date)
                                            isValidEntry = false;
                                    }
                                    if (isValidEntry.Equals(false))
                                    {
                                        MessageBox.Show("Some of the transactions do not have a valid transaction date. Details could not be saved", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                                else if (tran.TransactionEntries[0].AccountID > 0)
                                {
                                    MessageBox.Show("Cash preferences are not set for some of the accounts. Details could not be saved", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }
                    if (lsDeletedTransactionEntry.Count > 0)
                    {
                        //this list is used to add first deleted transactions than modified transactions
                        List<Transaction> lsUpdatedModifiedTransactions = new List<Transaction>();
                        lsUpdatedModifiedTransactions.AddRange(lsModifiedTransactions);
                        lsModifiedTransactions.Clear();
                        Transaction trDeletedTransactionEntry = new Transaction();
                        foreach (TransactionEntry entryToDelete in lsDeletedTransactionEntry)
                            trDeletedTransactionEntry.TransactionEntries.Add(entryToDelete);

                        //Both of the transactions have same activityid_fk 
                        //Transactions are first deleted and then added based on activityid_fk in persistence manager

                        //first add deleted transactions // If an entry is deleted then here is no need to have it's modified entries.
                        lsModifiedTransactions.Add(trDeletedTransactionEntry);

                        //then add modified transactions
                        lsModifiedTransactions.AddRange(lsUpdatedModifiedTransactions);
                        lsDeletedTransactionEntry.Clear();
                    }
                    if (lsModifiedTransactions.Count > 0)
                    {
                        HashSet<int> finalAccountIDs = new HashSet<int>();
                        foreach (Transaction transaction in lsModifiedTransactions)
                        {
                            foreach (TransactionEntry trEntry in transaction.TransactionEntries)
                            {
                                finalAccountIDs.Add(trEntry.AccountID);
                                //Access only the transaction Entries that are not deleted
                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-4740
                                if (trEntry.Error != string.Empty && trEntry.TaxLotState != ApplicationConstants.TaxLotState.Deleted)
                                {
                                    _ErrorDetails.Append("Tran. ID " + trEntry.TransactionID + " :\n" + trEntry.Error);
                                    isErrorsExists = true;
                                    break;
                                }
                            }
                            if (isErrorsExists)
                            {
                                break;
                            }
                        }
                        if (!isErrorsExists)
                        {
                            string commaSeparedIDs = string.Empty;
                            foreach (int accountID in finalAccountIDs)
                            {
                                commaSeparedIDs = commaSeparedIDs + accountID.ToString() + Seperators.SEPERATOR_8;
                            }
                            if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(commaSeparedIDs.TrimEnd(',')))
                            {
                                CashDataManager.GetInstance().Save(lsModifiedTransactions, ModifiedTransactionEntries, string.Empty, ultraTabControlCashMainValue.SelectedTab.Text);

                                List<TransactionEntry> modifiedTranEntries = lsModifiedTransactions.SelectMany(x => x.TransactionEntries).ToList();
                                List<CashJournalAuditEntry> lstCashJournalAudit = GetAuditTrailList(modifiedTranEntries, ultraTabControlCashMainValue.SelectedTab.Text);
                                AuditManager.Instance.SaveAuditListForCashJournal(lstCashJournalAudit);
                                lstCashJournalAudit.Clear();
                                lsModifiedTransactions.Clear();
                                ModifiedTransactionEntries.Clear();
                                _originalTransactions.Clear();
                                _newlyAddedTransactions.Clear();
                                if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                                {
                                    toolStripStatusLabel3.Text = "Data Saved";
                                    if (!CustomThemeHelper.ApplyTheme)
                                    {
                                        toolStripStatusLabel3.ForeColor = System.Drawing.Color.Blue;
                                    }
                                }
                                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                                {
                                    toolStripStatusLabel4.Text = "Data Saved";
                                    if (!CustomThemeHelper.ApplyTheme)
                                    {
                                        toolStripStatusLabel4.ForeColor = System.Drawing.Color.Blue;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please correct following error(s).\n-----------------------------------\n" + _ErrorDetails, "Information");
                        }
                    }
                    else
                    {
                        if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                        {
                            toolStripStatusLabel3.Text = "Nothing to Save";
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                toolStripStatusLabel3.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                        {
                            toolStripStatusLabel4.Text = "Nothing to Save";
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                toolStripStatusLabel4.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                btnSave.Text = "Save";
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the deleteRowtoolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void deleteRowtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Transaction selectedTransaction = GetSelectedTransaction();
                if (selectedTransaction != null)
                {
                    if (selectedTransaction.TransactionEntries.Count != 1)
                        DeleteTranscationEntry();
                    else
                        DeleteTransaction();
                }


                #region oldCode
                //if (ultraTabControlCashMainValue.SelectedTab.TabPage == ulTabPageConlNonTradingTrans)
                //{
                //    if (grdNonTradingTransactions.ActiveRow != null && grdNonTradingTransactions.ActiveRow.ListObject != null)
                //    {
                //        DataSet dsJournal = (DataSet)grdNonTradingTransactions.DataSource;
                //        DataRow row = ((System.Data.DataRowView)(grdNonTradingTransactions.ActiveRow.ListObject)).Row;
                //        //((DataSet)executionReportGrid.DataSource).Tables[0].Rows.Remove(row);
                //        if (!row[0].Equals(DBNull.Value) && !Convert.ToBoolean(row["IsAutomatic"]))
                //        {
                //            if (lsDeletedCash == null)
                //                lsDeletedCash = new List<string>();
                //            if (!lsDeletedCash.Contains(row["CashID"].ToString()))
                //                lsDeletedCash.Add(row["CashID"].ToString());

                //            dsJournal.Tables[0].Rows.Remove(row);
                //            //row.Delete();
                //        }
                //        else
                //            ((DataSet)grdNonTradingTransactions.DataSource).Tables[0].Rows.Remove(row);
                //    }
                //}
                //if (ultraTabControlCashMainValue.SelectedTab.TabPage == ulTabPageConlDividendTrans)
                //{
                //    if (grdDividend.ActiveRow != null && grdDividend.ActiveRow.ListObject != null)
                //    {
                //        DataSet dsDiv = (DataSet)grdDividend.DataSource;
                //        DataRow row = ((System.Data.DataRowView)(grdDividend.ActiveRow.ListObject)).Row;
                //        //((DataSet)executionReportGrid.DataSource).Tables[0].Rows.Remove(row);
                //        if (!row[0].Equals(DBNull.Value) && !Convert.ToBoolean(row["IsAutomatic"]))
                //        {
                //            if (lsDeletedDivCash == null)
                //                lsDeletedDivCash = new List<string>();
                //            if (!lsDeletedDivCash.Contains(row["CashID"].ToString()))
                //                lsDeletedDivCash.Add(row["CashID"].ToString());

                //            dsDiv.Tables[0].Rows.Remove(row);
                //            //row.Delete();
                //        }
                //        else
                //            ((DataSet)grdDividend.DataSource).Tables[0].Rows.Remove(row);
                //    }
                //}
                #endregion

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

        /// <summary>
        /// Handles the Click event of the deleteTranscationToolStripMenuitem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void deleteTranscationToolStripMenuitem_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteTransaction();

                #region oldcode
                //if (ultraTabControlCashMainValue.SelectedTab.TabPage == ulTabPageConlNonTradingTrans)
                //{
                //    if (grdNonTradingTransactions.ActiveRow != null && grdNonTradingTransactions.ActiveRow.ListObject != null)
                //    {
                //        //DataSet dsJournel = (DataSet)grdNonTradingTransactions.DataSource;
                //        //string strTranscationID = grdNonTradingTransactions.ActiveRow.Cells["TaxLot"].Value.ToString();
                //        //DataRow row;
                //        //for (int intRow = 0; intRow < grdNonTradingTransactions.Rows.Count; intRow++)
                //        //{
                //        //    if (strTranscationID == grdNonTradingTransactions.Rows[intRow].Cells["TaxLot"].Value.ToString())
                //        //    {
                //        //        row = ((System.Data.DataRowView)grdNonTradingTransactions.Rows[intRow].ListObject).Row;
                //        //        if (!row[0].Equals(DBNull.Value))
                //        //        {

                //        //            if (lsDeletedCash == null)
                //        //                lsDeletedCash = new List<string>();
                //        //            if (!lsDeletedCash.Contains(row["CashID"].ToString()))
                //        //                lsDeletedCash.Add(row["CashID"].ToString());

                //        //            dsJournel.Tables[0].Rows.Remove(row);
                //        //        }
                //        //    }
                //        //}

                //        if(grdNonTradingTransactions.ActiveRow.ListObject  is Transaction)
                //        {
                //            Transaction newTranscation = grdNonTradingTransactions.ActiveRow.ListObject as Transaction;                            
                //        }
                //        else if (grdNonTradingTransactions.ActiveRow.ListObject is TransactionEntry)
                //        {
                //            TransactionEntry newTransction = grdNonTradingTransactions.ActiveRow.ListObject as TransactionEntry;

                //        }


                //    }
                //}
                //else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ulTabPageConlDividendTrans)
                //{ 

                //}
                #endregion
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

        /// <summary>
        /// Handles the ValueChanged event of the dtPickerlower control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void dtPickerlower_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //SubscribeForCashTransactions();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the dtPickerUpper control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void dtPickerUpper_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //SubscribeForCashTransactions();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Exports to excel.
        /// </summary>
        /// <returns></returns>
        private bool ExportToExcel()
        {
            bool result = false;
            try
            {
                Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                string pathName = null;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                else
                {
                    return result;
                }
                string workbookName = "Report" + DateTime.Now.Date.ToString("yyyyMMdd");
                workBook.Worksheets.Add(workbookName);
                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];
                //UltraGrid selectedGrid = GetSelectedGrid();
                workBook = this.ultraGridExcelExporter1.Export(GetSelectedGrid(), workBook.Worksheets[workbookName]);
                workBook.Save(pathName);
                result = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="activitySource">The activity source.</param>
        private void getData(String accountIDs, String activitySource)
        {
            try
            {
                lsTransactionsFromDB = CashDataManager.GetInstance().GetTransactionsBeetweenTwoDates(dtPickerlower.DateTime, dtPickerUpper.DateTime, accountIDs, EnumHelper.GetValueFromEnumDescription<CashManagementEnums.ActivitySource>(activitySource));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the data in back ground.
        /// </summary>
        private void GetDataInBackGround()
        {
            try
            {
                SubscribeForCashTransactions();  // Filtering published data should be always according to the Date Range of gotten transactions.
                BackgroundWorker bgwrkr = new BackgroundWorker();
                bgwrkr.DoWork += new DoWorkEventHandler(bgwrkr_DoWork);
                bgwrkr.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwrkr_RunWorkerCompleted);
                changeStatusMsg(false);
                bgwrkr.RunWorkerAsync(new object[] { dtPickerlower.DateTime, dtPickerUpper.DateTime, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(','), ultraTabControlCashMainValue.SelectedTab.Text });

                lsModifiedTransactions.Clear();
                lsDeletedTransactionEntry.Clear();
                ModifiedTransactionEntries.Clear();
                isNewRowAdded = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the SelectedTabChanged event of the ultraTabControlCashMainValue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs"/> instance containing the event data.</param>
        private void ultraTabControlCashMainValue_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (ultraTabControlCashMainValue.SelectedTab != null && (ultraTabControlCashMainValue.SelectedTab.Key == "tbOtherJournals" || ultraTabControlCashMainValue.SelectedTab.Key == "tbNonTradingTran"))
                {
                    btnSave.Visible = true;
                }
                else
                    btnSave.Visible = false;
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

        /// <summary>
        /// Handles the MouseEnter event of the menuStripCashValue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuStripCashValue_MouseEnter(object sender, EventArgs e)
        {

            try
            {
                //if (grdNonTradingTransactions.ActiveRow == null && grdNonTradingTransactions.ActiveRow.ListObject == null)
                //{
                //    menuStripCashValue.Items["deleteTranscationItemtoolStripMenuItem"].Enabled = false;
                //    menuStripCashValue.Items["addTranscationItemToolStripMenuItem"].Enabled = false;
                //    menuStripCashValue.Items["deleteTranscationToolStripMenuitem"].Enabled = false;
                //}
                //else
                //{
                //    menuStripCashValue.Items["deleteTranscationItemtoolStripMenuItem"].Enabled = true;
                //    menuStripCashValue.Items["addTranscationItemToolStripMenuItem"].Enabled = true;
                //    menuStripCashValue.Items["deleteTranscationToolStripMenuitem"].Enabled = true;
                //}
                if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans && (grdNonTradingTransactions.ActiveRow != null && grdNonTradingTransactions.ActiveRow.ListObject == null))
                {
                    if ((grdNonTradingTransactions.ActiveRow.ListObject is TransactionEntry))
                    {
                        menuStripCashValue.Items["deleteTranscationItemtoolStripMenuItem"].Enabled = true;
                    }
                    else if (grdNonTradingTransactions.ActiveRow.ListObject is Transaction)
                    {
                        menuStripCashValue.Items["deleteTranscationItemtoolStripMenuItem"].Enabled = true;
                    }
                }


                else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans) && ((grdDividend.ActiveRow != null && grdDividend.ActiveRow.ListObject != null)))
                {
                    if ((grdDividend.ActiveRow.ListObject is Transaction))
                    {
                        menuStripCashValue.Items["deleteTranscationItemtoolStripMenuItem"].Enabled = false;
                    }
                    else
                    {
                        menuStripCashValue.Items["deleteTranscationItemtoolStripMenuItem"].Enabled = true;
                    }
                }

                else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance) && ((grdOpeningBalance.ActiveRow != null && grdOpeningBalance.ActiveRow.ListObject != null)))
                {
                    if ((grdOpeningBalance.ActiveRow.ListObject is Transaction))
                    {
                        menuStripCashValue.Items["deleteTranscationItemtoolStripMenuItem"].Enabled = false;
                        menuStripCashValue.Items["deleteTranscationToolStripMenuitem"].Enabled = true;
                    }
                    else
                    {
                        menuStripCashValue.Items["deleteTranscationItemtoolStripMenuItem"].Enabled = true;
                        menuStripCashValue.Items["deleteTranscationToolStripMenuitem"].Enabled = true;
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

        /// <summary>
        /// Handles the Opening event of the menuStripCashValue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void menuStripCashValue_Opening(object sender, CancelEventArgs e)
        {
            try
            {

                if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageTradingTrans
                    || ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans
                    || ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageRevaluation)
                {
                    deleteTranscationToolStripMenuitem.Visible = false;
                    deleteTranscationItemtoolStripMenuItem.Visible = false;
                    addTranscationToolStripMenuitem.Visible = false;
                    addTranscationItemToolStripMenuItem.Visible = false;
                    ShowAllLegstoolStripMenuItem.Visible = false;
                    saveLayoutToolStripMenuItem.Visible = true;

                    if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageRevaluation && grdRevaluation.Rows.Count > 0 && !string.IsNullOrEmpty(grdRevaluation.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ToString()))
                        ShowAllLegstoolStripMenuItem.Visible = true;
                    else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageTradingTrans && grdTradingTransactions.Rows.Count > 0 && !string.IsNullOrEmpty(grdTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ToString()))
                        ShowAllLegstoolStripMenuItem.Visible = true;
                    else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans && grdDividend.Rows.Count > 0 && !string.IsNullOrEmpty(grdDividend.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ToString()))
                        ShowAllLegstoolStripMenuItem.Visible = true;
                    else
                        ShowAllLegstoolStripMenuItem.Visible = false;

                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans
                    || ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                {

                    deleteTranscationToolStripMenuitem.Visible = true;
                    deleteTranscationItemtoolStripMenuItem.Visible = true;
                    addTranscationToolStripMenuitem.Visible = true;
                    addTranscationItemToolStripMenuItem.Visible = true;
                    saveLayoutToolStripMenuItem.Visible = true;

                    if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans && grdNonTradingTransactions.Rows.Count > 0 && !string.IsNullOrEmpty(grdNonTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ToString()))
                        ShowAllLegstoolStripMenuItem.Visible = true;
                    else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance && grdOpeningBalance.Rows.Count > 0 && !string.IsNullOrEmpty(grdOpeningBalance.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ToString()))
                        ShowAllLegstoolStripMenuItem.Visible = true;
                    else
                        ShowAllLegstoolStripMenuItem.Visible = false;

                    if (grdNonTradingTransactions.ContainsFocus && grdNonTradingTransactions.ActiveRow != null && grdNonTradingTransactions.Rows.Count > 0)
                    {
                        deleteTranscationToolStripMenuitem.Visible = true;
                        deleteTranscationItemtoolStripMenuItem.Visible = true;
                        if (grdNonTradingTransactions.ActiveRow.ListObject != null && (((TransactionEntry)grdNonTradingTransactions.ActiveRow.ListObject).TransactionSource == CashTransactionType.ManualJournalEntry || ((TransactionEntry)grdNonTradingTransactions.ActiveRow.ListObject).TransactionSource == CashTransactionType.ImportedEditableData) && (((TransactionEntry)grdNonTradingTransactions.ActiveRow.ListObject).Description != "Cash In Lieu"))
                        {
                            deleteTranscationToolStripMenuitem.Enabled = true;
                            deleteTranscationItemtoolStripMenuItem.Enabled = true;
                        }
                        else
                        {
                            deleteTranscationToolStripMenuitem.Enabled = false;
                            deleteTranscationItemtoolStripMenuItem.Enabled = false;
                        }
                    }
                    else
                    {
                        deleteTranscationToolStripMenuitem.Enabled = false;
                        deleteTranscationItemtoolStripMenuItem.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the saveLayoutAllToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void saveLayoutAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGridLayout saveGridLayout = new SaveGridLayout();
                CashManagementLayout cashManagementLayout = null;

                cashManagementLayout = saveGridLayout.GetLayout(grdTradingTransactions, "CashJournal");
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, Trading);
                Set(Trading, cashManagementLayout);
                cashManagementLayout = null;

                cashManagementLayout = saveGridLayout.GetLayout(grdNonTradingTransactions, "CashJournal");
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, NonTrading);
                Set(NonTrading, cashManagementLayout);
                cashManagementLayout = null;

                cashManagementLayout = saveGridLayout.GetLayout(grdDividend, "CashJournal");
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, Dividend);
                Set(Dividend, cashManagementLayout);
                cashManagementLayout = null;

                cashManagementLayout = saveGridLayout.GetLayout(grdRevaluation, "CashJournal");
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, Revaluation);
                Set(Revaluation, cashManagementLayout);
                cashManagementLayout = null;

                cashManagementLayout = saveGridLayout.GetLayout(grdOpeningBalance, "CashJournal");
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, OpeningBalance);
                Set(OpeningBalance, cashManagementLayout);
                cashManagementLayout = null;

                saveGridLayout = null;
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

        /// <summary>
        /// Handles the Click event of the saveLayoutCurrentToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void saveLayoutCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGridLayout saveGridLayout = new SaveGridLayout();
                if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageTradingTrans)
                {
                    CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(grdTradingTransactions, "CashJournal");
                    CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, Trading);
                    Set(Trading, cashManagementLayout);
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                {
                    CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(grdNonTradingTransactions, "CashJournal");
                    CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, NonTrading);
                    Set(NonTrading, cashManagementLayout);
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans)
                {
                    CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(grdDividend, "CashJournal");
                    CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, Dividend);
                    Set(Dividend, cashManagementLayout);
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageRevaluation)
                {
                    CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(grdRevaluation, "CashJournal");
                    CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, Revaluation);
                    Set(Revaluation, cashManagementLayout);
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance)
                {
                    CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(grdOpeningBalance, "CashJournal");
                    CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, OpeningBalance);
                    Set(OpeningBalance, cashManagementLayout);
                }
                saveGridLayout = null;
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

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                btnSave_Click(null, null);
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

        /// <summary>
        /// Handles the Click event of the ShowAllLegstoolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ShowAllLegstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                lstForVisibleAllLegs.Clear();
                if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans && grdNonTradingTransactions.Rows.Count > 0 && !string.IsNullOrEmpty(grdNonTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ToString()))
                {
                    foreach (UltraGridRow r in grdNonTradingTransactions.Rows.GetFilteredInNonGroupByRows())
                        lstForVisibleAllLegs.Add(Convert.ToString(r.GetCellValue("TransactionID")));

                    grdNonTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ClearFilterConditions();
                    grdNonTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].ClearFilterConditions();
                    grdNonTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].LogicalOperator = FilterLogicalOperator.Or;

                    foreach (string str in lstForVisibleAllLegs)
                        grdNonTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].FilterConditions.Add(FilterComparisionOperator.Equals, str);

                    if (grdNonTradingTransactions.DisplayLayout.Bands[0].Columns.Exists("TransactionID"))
                        grdNonTradingTransactions.DisplayLayout.Bands[0].Columns["TransactionID"].Hidden = false;
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageTradingTrans && grdTradingTransactions.Rows.Count > 0 && !string.IsNullOrEmpty(grdTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ToString()))
                {
                    foreach (UltraGridRow r in grdTradingTransactions.Rows.GetFilteredInNonGroupByRows())
                        lstForVisibleAllLegs.Add(Convert.ToString(r.GetCellValue("TransactionID")));

                    grdTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ClearFilterConditions();
                    grdTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].ClearFilterConditions();
                    grdTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].LogicalOperator = FilterLogicalOperator.Or;

                    foreach (string str in lstForVisibleAllLegs)
                        grdTradingTransactions.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].FilterConditions.Add(FilterComparisionOperator.Equals, str);

                    if (grdTradingTransactions.DisplayLayout.Bands[0].Columns.Exists("TransactionID"))
                        grdTradingTransactions.DisplayLayout.Bands[0].Columns["TransactionID"].Hidden = false;
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans && grdDividend.Rows.Count > 0 && !string.IsNullOrEmpty(grdDividend.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ToString()))
                {
                    foreach (UltraGridRow r in grdDividend.Rows.GetFilteredInNonGroupByRows())
                        lstForVisibleAllLegs.Add(Convert.ToString(r.GetCellValue("TransactionID")));

                    grdDividend.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ClearFilterConditions();
                    grdDividend.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].ClearFilterConditions();
                    grdDividend.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].LogicalOperator = FilterLogicalOperator.Or;

                    foreach (string str in lstForVisibleAllLegs)
                        grdDividend.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].FilterConditions.Add(FilterComparisionOperator.Equals, str);

                    if (grdDividend.DisplayLayout.Bands[0].Columns.Exists("TransactionID"))
                        grdDividend.DisplayLayout.Bands[0].Columns["TransactionID"].Hidden = false;
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageRevaluation && grdRevaluation.Rows.Count > 0 && !string.IsNullOrEmpty(grdRevaluation.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ToString()))
                {
                    foreach (UltraGridRow r in grdRevaluation.Rows.GetFilteredInNonGroupByRows())
                        lstForVisibleAllLegs.Add(Convert.ToString(r.GetCellValue("TransactionID")));

                    grdRevaluation.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ClearFilterConditions();
                    grdRevaluation.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].ClearFilterConditions();
                    grdRevaluation.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].LogicalOperator = FilterLogicalOperator.Or;

                    foreach (string str in lstForVisibleAllLegs)
                        grdRevaluation.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].FilterConditions.Add(FilterComparisionOperator.Equals, str);

                    if (grdRevaluation.DisplayLayout.Bands[0].Columns.Exists("TransactionID"))
                        grdRevaluation.DisplayLayout.Bands[0].Columns["TransactionID"].Hidden = false;
                }
                else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance && grdOpeningBalance.Rows.Count > 0 && !string.IsNullOrEmpty(grdOpeningBalance.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ToString()))
                {
                    foreach (UltraGridRow r in grdOpeningBalance.Rows.GetFilteredInNonGroupByRows())
                        lstForVisibleAllLegs.Add(Convert.ToString(r.GetCellValue("TransactionID")));

                    grdOpeningBalance.DisplayLayout.Bands[0].ColumnFilters["SubAcName"].ClearFilterConditions();
                    grdOpeningBalance.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].ClearFilterConditions();
                    grdOpeningBalance.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].LogicalOperator = FilterLogicalOperator.Or;

                    foreach (string str in lstForVisibleAllLegs)
                        grdOpeningBalance.DisplayLayout.Bands[0].ColumnFilters["TransactionID"].FilterConditions.Add(FilterComparisionOperator.Equals, str);

                    if (grdOpeningBalance.DisplayLayout.Bands[0].Columns.Exists("TransactionID"))
                        grdOpeningBalance.DisplayLayout.Bands[0].Columns["TransactionID"].Hidden = false;
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Other Methods

        /// <summary>
        /// Handles the SubAccountUpdated event of the CashAccountsUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void CashAccountsUI_SubAccountUpdated(object sender, EventArgs e)
        {
            try
            {
                InitializeSubAccountValueList();
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

        /// <summary>
        /// Initializes the sub account value list.
        /// </summary>
        public void InitializeSubAccountValueList()
        {
            try
            {
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-6293
                Dictionary<int, string> dicSubAccounts = WindsorContainerManager.getSubAccountsWithMasterCategoryName();
                _subAccount[0] = new ValueList();
                _subAccount[1] = new ValueList();
                _subAccount[2] = new ValueList();
                _subAccount[3] = new ValueList();
                foreach (int key in dicSubAccounts.Keys)
                {
                    _subAccount[0].ValueListItems.Add(key, dicSubAccounts[key]);
                    _subAccount[1].ValueListItems.Add(key, dicSubAccounts[key]);
                    _subAccount[2].ValueListItems.Add(key, dicSubAccounts[key]);
                    _subAccount[3].ValueListItems.Add(key, dicSubAccounts[key]);
                    if (!dictionarySubAccounts.ContainsKey(dicSubAccounts[key]))
                    {
                        dictionarySubAccounts.Add(dicSubAccounts[key], key);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Initializes the value list.
        /// </summary>
        private void InitializeValueList()
        {
            try
            {
                AccountCollection accountCollection = CachedDataManager.GetInstance.GetUserAccounts();

                _accountValList[0] = new ValueList();
                _accountValList[1] = new ValueList();
                _accountValList[2] = new ValueList();
                _accountValList[3] = new ValueList();
                foreach (Account account in accountCollection)
                {
                    if (account.AccountID != int.MinValue)
                    {
                        _accountValList[0].ValueListItems.Add(account.AccountID, account.Name);
                        _accountValList[1].ValueListItems.Add(account.AccountID, account.Name);
                        _accountValList[2].ValueListItems.Add(account.AccountID, account.Name);
                        _accountValList[3].ValueListItems.Add(account.AccountID, account.Name);
                        if (!dictionaryAccounts.ContainsKey(account.Name))
                        {
                            dictionaryAccounts.Add(account.Name, account.AccountID);
                        }
                    }
                }
                Dictionary<int, String> dictCurrency = new Dictionary<int, string>();
                dictCurrency = CachedDataManager.GetInstance.GetAllCurrencies();
                _currencyValList[0] = new ValueList();
                _currencyValList[1] = new ValueList();
                _currencyValList[2] = new ValueList();
                _currencyValList[3] = new ValueList();

                _AccountSideValList[0] = new ValueList();
                _AccountSideValList[1] = new ValueList();
                _AccountSideValList[0].ValueListItems.Add(AccountSide.DR);
                _AccountSideValList[0].ValueListItems.Add(AccountSide.CR);
                _AccountSideValList[1].ValueListItems.Add(AccountSide.DR);
                _AccountSideValList[1].ValueListItems.Add(AccountSide.CR);

                foreach (int key in dictCurrency.Keys)
                {
                    _currencyValList[0].ValueListItems.Add(key, dictCurrency[key]);
                    _currencyValList[1].ValueListItems.Add(key, dictCurrency[key]);
                    _currencyValList[2].ValueListItems.Add(key, dictCurrency[key]);
                    _currencyValList[3].ValueListItems.Add(key, dictCurrency[key]);
                    if (!dictionaryCurrency.ContainsKey(dictCurrency[key]))
                    {
                        dictionaryCurrency.Add(dictCurrency[key], key);
                    }
                }
                fxConversionMethodOperatorValList[0] = new ValueList();
                fxConversionMethodOperatorValList[1] = new ValueList();
                List<EnumerationValue> fxConversionMethodOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                foreach (EnumerationValue var in fxConversionMethodOperator)
                {
                    if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                    {
                        fxConversionMethodOperatorValList[0].ValueListItems.Add(var.DisplayText, var.DisplayText);
                        fxConversionMethodOperatorValList[1].ValueListItems.Add(var.DisplayText, var.DisplayText);
                    }
                }

                _vlTransactionSource[0] = new ValueList();
                _vlTransactionSource[1] = new ValueList();
                _vlTransactionSource[2] = new ValueList();
                _vlTransactionSource[3] = new ValueList();
                _vlTransactionSource[4] = new ValueList();

                //create the transaction source value lists
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5219
                foreach (CashTransactionType cashType in Enum.GetValues(typeof(CashTransactionType)))
                {
                    _vlTransactionSource[0].ValueListItems.Add(cashType.ToString(), EnumHelper.GetDescription(cashType));
                    _vlTransactionSource[1].ValueListItems.Add(cashType.ToString(), EnumHelper.GetDescription(cashType));
                    _vlTransactionSource[2].ValueListItems.Add(cashType.ToString(), EnumHelper.GetDescription(cashType));
                    _vlTransactionSource[3].ValueListItems.Add(cashType.ToString(), EnumHelper.GetDescription(cashType));
                    _vlTransactionSource[4].ValueListItems.Add(cashType.ToString(), EnumHelper.GetDescription(cashType));
                }
                InitializeSubAccountValueList();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Changes the status MSG.
        /// </summary>
        /// <param name="isWorkCompleted">if set to <c>true</c> [is work completed].</param>
        private void changeStatusMsg(bool isWorkCompleted)
        {
            try
            {
                if (isWorkCompleted)
                {
                    if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                    {
                        toolStripStatusLabel3.Text = String.Empty;
                        grdNonTradingTransactions.Enabled = true;
                        grdNonTradingTransactions.ResumeLayout();
                    }
                    else if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageTradingTrans)
                    {
                        grdTradingTransactions.Enabled = true;
                        grdTradingTransactions.ResumeLayout();
                    }
                    else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance))
                    {
                        grdOpeningBalance.Enabled = true;
                        grdOpeningBalance.ResumeLayout();
                    }
                    else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans))
                    {
                        grdDividend.Enabled = true;
                        grdDividend.ResumeLayout();
                    }
                    else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageRevaluation))
                    {
                        grdRevaluation.Enabled = true;
                        grdRevaluation.ResumeLayout();
                    }
                    ugbxJournalParams.Enabled = true;
                    toolStripStatusLabel1.Text = string.Empty;
                    toolStripRevaluation.Text = string.Empty;
                    toolStripStatusLabel3.Text = string.Empty;
                    toolStripStatusLabel4.Text = string.Empty;
                    statusStripLabelDividend.Text = string.Empty;
                }
                else
                {
                    if (ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageNonTradingTrans)
                    {
                        toolStripStatusLabel3.Text = "Getting Non-Trading Transactions....";
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            toolStripStatusLabel3.ForeColor = System.Drawing.Color.Green;
                        }
                        grdNonTradingTransactions.Enabled = false;
                        grdNonTradingTransactions.SuspendLayout();
                    }

                    else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageOpeningBalance))
                    {
                        toolStripStatusLabel4.Text = "Getting Opening Balances....";
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            toolStripStatusLabel4.ForeColor = System.Drawing.Color.Green;
                        }
                        grdOpeningBalance.Enabled = false;
                        grdOpeningBalance.SuspendLayout();
                    }
                    else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageDividendTrans))
                    {
                        statusStripLabelDividend.Text = "Getting Dividends....";
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            statusStripLabelDividend.ForeColor = System.Drawing.Color.Green;
                        }
                        grdDividend.Enabled = false;
                        grdDividend.SuspendLayout();
                    }
                    else if ((ultraTabControlCashMainValue.SelectedTab.TabPage == ultraTabPageTradingTrans))
                    {
                        toolStripStatusLabel1.Text = "Getting Trading Transactions....";
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            toolStripStatusLabel1.ForeColor = System.Drawing.Color.Green;
                        }
                        grdTradingTransactions.Enabled = false;
                        grdTradingTransactions.SuspendLayout();
                    }
                    else
                    {
                        toolStripRevaluation.Text = "Getting Revaluation Transactions....";
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            toolStripRevaluation.ForeColor = System.Drawing.Color.Green;
                        }
                        grdRevaluation.Enabled = false;
                        grdRevaluation.SuspendLayout();
                    }
                    ugbxJournalParams.Enabled = false;
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

        /// <summary>
        /// Clears the data.
        /// </summary>
        private void clearData()
        {
            if (ultraTabControlCashMainValue.SelectedTab != null)
            {
                if (ultraTabControlCashMainValue.SelectedTab.Key.Equals("tbTradingTrans"))
                {
                    if (TradingTransactions.Count > 0 || TradingTransactionEntries.Count > 0)
                    {
                        TradingTransactions.Clear();
                        TradingTransactionEntries.Clear();
                    }
                }
                else if (ultraTabControlCashMainValue.SelectedTab.Key.Equals("tbNonTradingTran"))
                {
                    if (_lsNonTradingTransactions.Count > 0 || _lsNonTradingTransactionEntries.Count > 0)
                    {
                        _lsNonTradingTransactions.Clear();
                        _lsNonTradingTransactionEntries.Clear();
                    }
                }
                else if (ultraTabControlCashMainValue.SelectedTab.Key.Equals("tbDividend"))
                {
                    if (DividendTransactions.Count > 0 || DividendTransactionEntries.Count > 0)
                    {
                        DividendTransactions.Clear();
                        DividendTransactionEntries.Clear();
                    }
                }
                else if (ultraTabControlCashMainValue.SelectedTab.Key.Equals("tbRevaluation"))
                {
                    if (RevaluationTransactions.Count > 0 || RevaluationTransactionEntries.Count > 0)
                    {
                        RevaluationTransactions.Clear();
                        RevaluationTransactionEntries.Clear();
                    }
                }
                else if (ultraTabControlCashMainValue.SelectedTab.Key.Equals("tbOtherJournals"))
                {
                    if (_lsOtherJournalTransactions.Count > 0 || _lsOtherJournalTransactionEntries.Count > 0)
                    {
                        _lsOtherJournalTransactions.Clear();
                        _lsOtherJournalTransactionEntries.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Creates the subscription services proxy.
        /// </summary>
        public void CreateSubscriptionServicesProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                //SubscribeForCashTransactions();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the CtrlSubAccountCashImport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CtrlSubAccountCashImport_Load(object sender, EventArgs e)
        {
            try
            {
                SubscribeForCashTransactions();
                ctrlMasterFundAndAccountsDropdown1.Setup();
                ugbxJournalParams.Appearance.BackColor = System.Drawing.Color.LightGray;
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

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(string key, CashManagementLayout value)
        {
            try
            {
                if (cashJournalLayout.ContainsKey(key))
                {
                    cashJournalLayout[key] = value;
                }
                else
                {
                    cashJournalLayout.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
        }

        /// <summary>
        /// Sets the color of the buttons.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnGetCash.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetCash.ForeColor = System.Drawing.Color.White;
                btnGetCash.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetCash.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetCash.UseAppStyling = false;
                btnGetCash.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExport.ForeColor = System.Drawing.Color.White;
                btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExport.UseAppStyling = false;
                btnExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Added by: Bharat raturi
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-5219
        /// Set the transaction source value list for the column
        /// </summary>
        /// <param name="grid">ultragrid that the column is in</param>
        /// <param name="gridNumber">number to pick up the right valuelist from the collection</param>
        private void SetTransactionSourceCaption(UltraGrid grid, int gridNumber)
        {
            try
            {
                if (grid != null && grid.DisplayLayout.Bands.Count > 0 && grid.DisplayLayout.Bands[0].Columns.Exists("TransactionSource"))
                {
                    grid.DisplayLayout.Bands[0].Columns["TransactionSource"].ValueList = _vlTransactionSource[gridNumber];
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Subscribes for cash transactions.
        /// </summary>
        private void SubscribeForCashTransactions()
        {
            try
            {
                if (_proxy != null)
                {
                    _proxy.UnSubscribe();
                    FilterDataByDateRange filterdata = new FilterDataByDateRange();
                    filterdata.ToDate = dtPickerUpper.DateTime.Date;
                    filterdata.FromDate = dtPickerlower.DateTime.Date;
                    List<FilterData> filters = new List<FilterData>();
                    filters.Add(filterdata);
                    _proxy.Subscribe(Topics.Topic_CashData, filters);
                    _proxy.Subscribe(Topics.Topic_RevaluationJournal, null);
                    _proxy.Subscribe(Topics.Topic_ManualJournalActivity, null);

                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-6746
                    _proxy.Subscribe(Topics.Topic_SubAccounts, null);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Uns the wire events.
        /// </summary>
        private void UnWireEvents()
        {
            this.grdTradingTransactions.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdTradingTransactions_AfterSortChange);
            this.grdNonTradingTransactions.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdNonTradingTransactions_AfterSortChange);
            this.grdDividend.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdDividend_AfterSortChange);
            this.grdOpeningBalance.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdOpeningBalance_AfterSortChange);
            this.grdRevaluation.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdRevaluation_AfterSortChange);
        }

        /// <summary>
        /// Wires the events.
        /// </summary>
        private void WireEvents()
        {
            this.grdTradingTransactions.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdTradingTransactions_AfterSortChange);
            this.grdNonTradingTransactions.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdNonTradingTransactions_AfterSortChange);
            this.grdDividend.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdDividend_AfterSortChange);
            this.grdOpeningBalance.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdOpeningBalance_AfterSortChange);
            this.grdRevaluation.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdRevaluation_AfterSortChange);
        }

        #endregion Methods

        #region IPublishing Members

        /// <summary>
        /// Gets the name of the receiver unique.
        /// </summary>
        /// <returns></returns>
        public string getReceiverUniqueName()
        {
            return "CtrlSubAccountCashImport";
        }

        /// <summary>
        /// Publishes the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="topicName">Name of the topic.</param>
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker del =
                            delegate
                            {
                                Publish(e, topicName);
                            };
                        this.BeginInvoke(del);
                        return;
                    }

                    System.Object[] publishDataList = null;
                    lock (_locker)
                    {
                        switch (topicName)
                        {
                            case Topics.Topic_CashData:
                                publishDataList = (System.Object[])e.EventData;

                                foreach (Object obj in publishDataList)
                                {
                                    if (obj is string)
                                    {
                                        toolStripRevaluation.Text = obj as string;
                                        continue;
                                    }
                                    Transaction transaction = (Transaction)obj;

                                    switch (transaction.GetActivitySource())
                                    {
                                        case (byte)ActivitySource.Trading:
                                            UpdateEntries(transaction, TradingTransactions, TradingTransactionEntries);
                                            break;
                                        case (byte)ActivitySource.Dividend:
                                            UpdateEntries(transaction, DividendTransactions, DividendTransactionEntries);
                                            break;
                                        case (byte)ActivitySource.NonTrading:
                                            UpdateEntries(transaction, _lsNonTradingTransactions, _lsNonTradingTransactionEntries);
                                            break;
                                        case (byte)ActivitySource.Revaluation:
                                            UpdateEntries(transaction, RevaluationTransactions, RevaluationTransactionEntries);
                                            break;
                                        case (byte)ActivitySource.OpeningBalance:
                                            UpdateEntries(transaction, _lsOtherJournalTransactions, _lsOtherJournalTransactionEntries);
                                            break;
                                    }

                                    #region Old Code


                                    ////From Daily Calculation TaxlotState Come with only new state
                                    //if (transaction.IsDailyCalculationTransaction())
                                    //{
                                    //    if (!string.IsNullOrEmpty(transaction.TransactionID) && TradingTransactions.Contains(transaction))
                                    //        TradingTransactions.Update(transaction);
                                    //    else
                                    //    {
                                    //        TradingTransactions.Add(transaction);
                                    //        //Code to bind Transaction Entries to ui instead of Transactions
                                    //        AddTransactionEntries(transaction, TradingTransactionEntries);
                                    //    }

                                    //}
                                    //else
                                    //{
                                    //    if (transaction.GetTaxlotState() == ApplicationConstants.TaxLotState.New.ToString())
                                    //    {
                                    //        if (transaction.IsTradingTransaction() || transaction.GetTransactionSource() == (int)TransactionSource.Closing)
                                    //        {
                                    //            TradingTransactions.Add(transaction);

                                    //            //Code to bind Transaction Entries to ui instead of Transactions
                                    //            AddTransactionEntries(transaction, TradingTransactionEntries);
                                    //        }
                                    //        //here is no need to handle Manual Dividend/ Manual transactions  as these will be already on ui
                                    //        else if (transaction.GetTransactionSource() == (int)TransactionSource.TradingDividend)
                                    //        {
                                    //            DividendTransactions.Add(transaction);

                                    //            //Code to bind Transaction Entries to ui instead of Transactions
                                    //            AddTransactionEntries(transaction, DividendTransactionEntries);
                                    //        }
                                    //        else if (transaction.GetTransactionSource() == (int)TransactionSource.ImportedEditableData)
                                    //        {
                                    //            _lsNonTradingTransactions.Add(transaction);

                                    //            //Code to bind Transaction Entries to ui instead of Transactions
                                    //            AddTransactionEntries(transaction, _lsNonTradingTransactionEntriess);
                                    //        }
                                    //    }
                                    //    else if (transaction.GetTaxlotState() == ApplicationConstants.TaxLotState.Deleted.ToString())
                                    //    {
                                    //        if (transaction.IsTradingTransaction() && !string.IsNullOrEmpty(transaction.TransactionID) && TradingTransactions.Contains(transaction))
                                    //        {
                                    //            TradingTransactions.Remove(transaction);

                                    //            //Code to bind Transaction Entries to ui instead of Transactions
                                    //            RemoveTransactionEntries(transaction, TradingTransactionEntries);
                                    //        }
                                    //        else if (transaction.GetTransactionSource() == (int)TransactionSource.TradingDividend && !string.IsNullOrEmpty(transaction.TransactionID) && DividendTransactions.Contains(transaction))
                                    //        {
                                    //            DividendTransactions.Remove(transaction);

                                    //            RemoveTransactionEntries(transaction, DividendTransactionEntries);
                                    //        }

                                    //    }
                                    //    else if (transaction.GetTaxlotState() == ApplicationConstants.TaxLotState.Updated.ToString())
                                    //    {
                                    //        if (transaction.IsTradingTransaction() && !string.IsNullOrEmpty(transaction.TransactionID) && TradingTransactions.Contains(transaction))
                                    //        {
                                    //            TradingTransactions.Update(transaction);
                                    //            RemoveTransactionEntries(transaction, TradingTransactionEntries);
                                    //            AddTransactionEntries(transaction, TradingTransactionEntries);
                                    //        }
                                    //        else if (transaction.GetTransactionSource() == (int)TransactionSource.TradingDividend && !string.IsNullOrEmpty(transaction.TransactionID) && DividendTransactions.Contains(transaction))
                                    //        {
                                    //            DividendTransactions.Update(transaction);
                                    //            RemoveTransactionEntries(transaction, DividendTransactionEntries);
                                    //            AddTransactionEntries(transaction, DividendTransactionEntries);
                                    //        }
                                    //    }
                                    //}
                                    #endregion

                                }
                                break;

                            case Topics.Topic_RevaluationJournal:
                                publishDataList = (System.Object[])e.EventData;
                                foreach (Object obj in publishDataList)
                                {
                                    toolStripRevaluation.Text = obj as string;
                                }
                                break;

                            case Topics.Topic_ManualJournalActivity:
                                publishDataList = (System.Object[])e.EventData;
                                foreach (Object obj in publishDataList)
                                {
                                    newcashActivity.Add(obj as CashActivity);
                                }

                                foreach (CashActivity c in newcashActivity)
                                {
                                    //Modified by: Surendra Bisht, 1oct 2014
                                    //Publish the transaction details after save for both of the non-trading and opening balance transactions
                                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-4395
                                    GenericBindingList<TransactionEntry> transactionEntryToModify = new GenericBindingList<TransactionEntry>();
                                    if (c.TransactionSource == CashTransactionType.ManualJournalEntry)
                                        transactionEntryToModify = _lsNonTradingTransactionEntries;
                                    else
                                        transactionEntryToModify = _lsOtherJournalTransactionEntries;

                                    for (int i = 0; i < transactionEntryToModify.Count; i++)
                                    {
                                        TransactionEntry t = transactionEntryToModify[i];

                                        if (t.TransactionID == c.FKID && t.ActivityId_FK == null)
                                        {
                                            t.ActivityId_FK = c.ActivityId;
                                            t.TaxLotState = ApplicationConstants.TaxLotState.Updated;
                                        }
                                    }
                                }
                                break;
                            case Topics.Topic_SubAccounts:
                                publishDataList = (System.Object[])e.EventData;
                                if (publishDataList.Count() > 0)
                                {
                                    Dictionary<int, string> dicSubAccounts = WindsorContainerManager.getSubAccountsWithMasterCategoryName(true);
                                    _subAccount[0].ValueListItems.Clear();
                                    _subAccount[1].ValueListItems.Clear();
                                    _subAccount[2].ValueListItems.Clear();
                                    _subAccount[3].ValueListItems.Clear();
                                    dictionarySubAccounts.Clear();
                                    foreach (int key in dicSubAccounts.Keys)
                                    {
                                        _subAccount[0].ValueListItems.Add(key, dicSubAccounts[key]);
                                        _subAccount[1].ValueListItems.Add(key, dicSubAccounts[key]);
                                        _subAccount[2].ValueListItems.Add(key, dicSubAccounts[key]);
                                        _subAccount[3].ValueListItems.Add(key, dicSubAccounts[key]);

                                        if (!dictionarySubAccounts.ContainsKey(dicSubAccounts[key]))
                                            dictionarySubAccounts.Add(dicSubAccounts[key], key);
                                    }
                                    grdNonTradingTransactions.DisplayLayout.Bands[0].Columns["SubAcName"].ValueList = null;
                                    grdOpeningBalance.DisplayLayout.Bands[0].Columns["SubAcName"].ValueList = null;
                                    grdNonTradingTransactions.DisplayLayout.Bands[0].Columns["SubAcName"].ValueList = _subAccount[1];
                                    grdOpeningBalance.DisplayLayout.Bands[0].Columns["SubAcName"].ValueList = _subAccount[2];


                                    //These foreach loops are used to update Sub Account on Journal UI.
                                    foreach (TransactionEntry transNon in _lsNonTradingTransactionEntries)
                                    {
                                        if (dicSubAccounts.ContainsKey(transNon.SubAcID))
                                            transNon.SubAcName = Convert.ToString(dicSubAccounts[transNon.SubAcID]);
                                    }

                                    foreach (TransactionEntry transOther in _lsOtherJournalTransactionEntries)
                                    {
                                        if (dicSubAccounts.ContainsKey(transOther.SubAcID))
                                            transOther.SubAcName = Convert.ToString(dicSubAccounts[transOther.SubAcID]);
                                    }

                                    foreach (TransactionEntry transTrade in _lsTradingTransactionEntries)
                                    {
                                        if (dicSubAccounts.ContainsKey(transTrade.SubAcID))
                                            transTrade.SubAcName = Convert.ToString(dicSubAccounts[transTrade.SubAcID]);
                                    }

                                    foreach (TransactionEntry transDiv in _lsDividendTransactionEntries)
                                    {
                                        if (dicSubAccounts.ContainsKey(transDiv.SubAcID))
                                            transDiv.SubAcName = Convert.ToString(dicSubAccounts[transDiv.SubAcID]);
                                    }

                                    foreach (TransactionEntry transRev in _lsRevaluationTransactionEntries)
                                    {
                                        if (dicSubAccounts.ContainsKey(transRev.SubAcID))
                                            transRev.SubAcName = Convert.ToString(dicSubAccounts[transRev.SubAcID]);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Updates the entries.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="lsTransactions">The ls transactions.</param>
        /// <param name="lsTransactionEntries">The ls transaction entries.</param>
        private void UpdateEntries(Transaction transaction, GenericBindingList<Transaction> lsTransactions, GenericBindingList<TransactionEntry> lsTransactionEntries)
        {
            try
            {
                // Kuldeep: Here we faced the issue in which sequence of Transaction entries was:- Delete, Not changed and New.
                // So first delete comes and it deletes the transaction from collection and for next Transaction, it gets status as NOT CHANGED because in method GetTaxlotState()
                // we are getting status of first entry which is wrong and thus it never adds it back to transaction.
                List<Transaction> lsTransactionsToRemove = lsTransactions.GetList().FindAll(delegate (Transaction o) { return ((o.ActivityId_FK == transaction.ActivityId_FK) && (o.TransactionNumber == transaction.TransactionNumber)); });
                foreach (Transaction trToRemove in lsTransactionsToRemove)
                {
                    lsTransactions.Remove(trToRemove);
                    RemoveTransactionEntries(trToRemove, lsTransactionEntries);
                }

                if (transaction.GetTaxlotState() != ApplicationConstants.TaxLotState.Deleted.ToString())
                {
                    lsTransactions.Add(transaction);
                    AddTransactionEntries(transaction, lsTransactionEntries);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
