using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.CashManagement.Forms
{
    //Added by Nishant Jain, 04-08-2015 
    public partial class frmAccountTransactionDetails : Form
    {
        GenericBindingList<TransactionEntry> lstTrasactionTobind = new GenericBindingList<TransactionEntry>();
        public frmAccountTransactionDetails()
        {
            InitializeComponent();
        }

        public void SetUp(string transactionId)
        {
            try
            {
                grdActivities.DataSource = null;
                grdJournals.DataSource = null;
                lstTrasactionTobind.Clear();
                List<Transaction> lsTransactionsFromDB = CashDataManager.GetInstance().GetTransactionsByTransactionID(transactionId);
                getTransactionEntry(lsTransactionsFromDB);
                grdJournals.DataSource = lstTrasactionTobind;
                HelperClass.SetColumnDisplayNames(grdJournals, null);
                HelperClass.GridSettingForJournalLook(grdJournals);

                if (lsTransactionsFromDB != null && lsTransactionsFromDB.Count > 0)
                {
                    GenericBindingList<CashActivity> lsCashActivityToBind = new GenericBindingList<CashActivity>();
                    List<CashActivity> lstCashActivity = new List<CashActivity>();
                    if (lsTransactionsFromDB[0].GetActivitySource() == (byte)(ActivitySource.NonTrading) || lsTransactionsFromDB[0].GetActivitySource() == (byte)(ActivitySource.OpeningBalance))
                    {
                        string transactionID = string.Join(",", lsTransactionsFromDB.Select(x => x.TransactionID).ToArray());
                        lstCashActivity = CashDataManager.GetInstance().GetActivitiesByTransactionId(transactionID);
                    }
                    else
                    {
                        string activitiesId = string.Join(",", lsTransactionsFromDB.Select(x => x.ActivityId_FK).ToArray());
                        lstCashActivity = CashDataManager.GetInstance().GetActivitiesByActivityId(activitiesId);
                    }

                    lsCashActivityToBind.AddList(lstCashActivity);
                    foreach (CashActivity ca in lsCashActivityToBind)
                    {
                        ca.Amount = ca.Amount - ca.TotalCommission;
                    }
                    grdActivities.DataSource = lsCashActivityToBind;

                    List<string> lsColumnsToDisplay = new List<string>(new string[] { "Date", "ActivityType", "AccountName", "Symbol", "CurrencyName", "Amount", "BalanceType", "TransactionSource" });
                    HelperClass.SetColumnDisplayNames(grdActivities, lsColumnsToDisplay);
                }
                CustomThemeHelper.AddUltraFormManagerToDynamicForm(this.FindForm());
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CASH_MANAGEMENT);
                CustomThemeHelper.SetThemeProperties(grdJournals, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_JOURNAL_PANAL);
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

        private void getTransactionEntry(List<Transaction> lsTransactionsFromDB)
        {
            try
            {
                if (lsTransactionsFromDB != null)
                {
                    foreach (Transaction t in lsTransactionsFromDB)
                    {
                        if (t.TransactionID != null)
                        {
                            AddTransactionEntries(t, lstTrasactionTobind);
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

        private void AddTransactionEntries(Transaction source, GenericBindingList<TransactionEntry> destinationList)
        {
            try
            {
                if (source != null)
                {
                    Dictionary<string, TransactionEntry> dictGroupedTransactionEntry = GroupTransactionEntries(source);

                    foreach (KeyValuePair<string, TransactionEntry> kvpTransactionEntry in dictGroupedTransactionEntry)
                    {
                        if (!destinationList.Contains(kvpTransactionEntry.Value))
                            destinationList.Add(kvpTransactionEntry.Value);
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

        private Dictionary<string, TransactionEntry> GroupTransactionEntries(Transaction source)
        {
            Dictionary<string, TransactionEntry> dictGroupedTransactionEntry = new Dictionary<string, TransactionEntry>();
            try
            {
                foreach (TransactionEntry trEntry in source.TransactionEntries)
                {
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dictGroupedTransactionEntry;
        }

        private void grdActivities_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdActivities);
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

        private void grdJournals_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdJournals);
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

        private void grdActivities_InitializeGroupByRow(object sender, Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventArgs e)
        {
            HelperClass.GroupByRowSetting(e);
        }

        private void grdActivities_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            HelperClass.ActivitySummarySettings(e);
            grdActivities.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
        }

        private void grdJournals_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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

        private void grdJournals_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
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

        private void grdActivities_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        //private void grdJournals_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        //{
        //    (e.CustomRowFiltersDialog as Form).PaintDynamicForm();            
        //}

        private void grdJournals_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.ListObject is TransactionEntry)
            {
                AccountSideColumnSetting(e.Row);
            }
        }
    }
}
