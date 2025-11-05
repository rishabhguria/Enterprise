using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Infragistics.Win;
using Prana.BusinessObjects.AppConstants;
using Prana.Interfaces;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.CommonDataCache;

namespace Prana.CashManagement.Controls
{
    public partial class ctrlCashExceptions : UserControl
    {
        public ctrlCashExceptions()
        {
            InitializeComponent();
            BindGrid();
        }

        GenericBindingList<TransactionEntry> _lsTransactionEntryToBind = new GenericBindingList<TransactionEntry>();
        List<Transaction> _transactionsFromDB = new List<Transaction>();  

        private void BindGrid()
        {
            try
            {
                if (!this.IsDisposed)
                {
                    
                    TransactionEntry trEntryToInitiallizeGrid = new TransactionEntry();
                    if (_lsTransactionEntryToBind.Count == 0)
                        _lsTransactionEntryToBind.Add(trEntryToInitiallizeGrid);

                    grdCashExceptions.DataSource = _lsTransactionEntryToBind;

                    HelperClass.SetColumnDisplayNames(grdCashExceptions, null);
                    HelperClass.GridSettingForJournalLook(grdCashExceptions);
                    _lsTransactionEntryToBind.Clear();
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }

        }

        private void ClearData()
        {
            try
            {
                if (_lsTransactionEntryToBind != null)
                    _lsTransactionEntryToBind.Clear();
                if (_transactionsFromDB != null)
                    _transactionsFromDB.Clear();
                if (_dtDiv != null)
                    _dtDiv.Clear();
                if (_lsTRATransactions != null)
                    _lsTRATransactions.Clear();

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }

        }

        private void ChangeStatus(bool workCompleted, bool isGetExceptions, bool isGetOverridingData)
        {
            if (workCompleted)
            {
                btnGetEx.Text = "Get Exceptions";
                btnSave.Text = "Save";
                btnGetEx.Enabled = true;
                btnSave.Enabled = true;
                btnOverriding.Enabled = true;
                uEGboxExceptions.Text = " ";
                btnOverriding.Text = "Get Overriding Data";

            }
            else
            {
                if (isGetExceptions)
                {
                    btnGetEx.Text = "Getting...";
                    uEGboxExceptions.Text = "Getting Exceptions...";
                }
                else if (isGetOverridingData)
                {
                    btnOverriding.Text = "Getting...";                    
                    uEGboxExceptions.Text = "Getting OverridingData...";
                }
                else
                {
                    btnSave.Text = "Saving...";
                    uEGboxExceptions.Text = "Saving Data...";
                }
                btnGetEx.Enabled = false;
                btnOverriding.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        #region Getting Exceptions Section

        private void btnGetEx_Click(object sender, EventArgs e)
        {
            try
            {
                ClearData();           
                GetExceptionsAsync();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void GetExceptionsAsync()
        {
            try
            {
                BackgroundWorker bgwrkrGetData = new BackgroundWorker();
                bgwrkrGetData.DoWork += new DoWorkEventHandler(bgwrkrGetData_DoWork);
                bgwrkrGetData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwrkrGetData_RunWorkerCompleted);
                ChangeStatus(false, true,false);
                bgwrkrGetData.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
            }
        }
        DataTable _dtDiv;
        List<Transaction> _lsTRATransactions;
        void bgwrkrGetData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                DateTime CashmgmtStartDate = CashDataManager.GetInstance().GetCashPreferences().CashMgmtStartDate;
                if (CashmgmtStartDate <= dtFromDate.DateTime)
                {

                    #region Trading Activity Overriding Section

                    _lsTRATransactions = CashDataManager.GetInstance().GetCashExceptions(dtFromDate.DateTime, dtToDate.DateTime);
                    if(_lsTRATransactions!=null && _lsTRATransactions.Count>0)
                        _transactionsFromDB.AddRange(_lsTRATransactions); 


                    #endregion

                    #region Dividend Overriding Section

                    _dtDiv = CashDataManager.GetInstance().GetCashDividendsExceptions(string.Empty, dtFromDate.DateTime, dtToDate.DateTime).Tables[0];
                    if (_dtDiv != null && _dtDiv.Rows.Count > 0)
                    {
                        TaxlotBaseCollection modifiedTaxlots = CashDataManager.GetInstance().GetTaxlotBaseCollection(_dtDiv);
                        List<Transaction> lsDividendTransactions = CashDataManager.GetInstance().GetDividendEntriesFromTaxlotBase(modifiedTaxlots);


                        _transactionsFromDB.AddRange(lsDividendTransactions);
                    }

                    #endregion
                    
                }
                else
                    MessageBox.Show("Please Select Date Greater then Cash Management Start Date " + CashmgmtStartDate);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
            }
        }

        void bgwrkrGetData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {   
                ChangeStatus(true, true,true);
                if (_transactionsFromDB != null)
                {

                    //Code to bind Transaction Entries to ui instead of Transactions
                    foreach (Transaction tr in _transactionsFromDB)
                    {
                        foreach (TransactionEntry trEntry in tr.TransactionEntries)
                        {
                            if (!_lsTransactionEntryToBind.Contains(trEntry))
                                _lsTransactionEntryToBind.Add(trEntry);
                        }
                    }
                }
             
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
            }
        }


        #endregion 

        #region overriding Data Section

        private void btnOverriding_Click(object sender, EventArgs e)
        {
            try
            {
                ClearData();
                BackgroundWorker bgWorkerGetOverridingDataAsyn = new BackgroundWorker();
                bgWorkerGetOverridingDataAsyn.DoWork += new DoWorkEventHandler(bgWorkerGetOverridingDataAsyn_DoWork);
                bgWorkerGetOverridingDataAsyn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwrkrGetData_RunWorkerCompleted);
                ChangeStatus(false, false, true);
                bgWorkerGetOverridingDataAsyn.RunWorkerAsync();
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void bgWorkerGetOverridingDataAsyn_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DateTime CashmgmtStartDate = CashDataManager.GetInstance().GetCashPreferences().CashMgmtStartDate;

                if (CashmgmtStartDate <= dtFromDate.DateTime)
                {
                    

                    #region Trading Activity Overriding Section

                    _lsTRATransactions = CashDataManager.GetInstance().GetTransactionsForOverriding(dtFromDate.DateTime, dtToDate.DateTime);
                    if (_lsTRATransactions != null && _lsTRATransactions.Count > 0)
                        _transactionsFromDB.AddRange(_lsTRATransactions);

                    #endregion

                    #region Dividend Overriding Section

                    _dtDiv = CashDataManager.GetInstance().GetCashDividendsForGivenDates(string.Empty, "-1", "ExDate", dtFromDate.DateTime, dtToDate.DateTime).Tables[0];
                    if (_dtDiv != null && _dtDiv.Rows.Count > 0)
                    {
                        TaxlotBaseCollection modifiedTaxlots = CashDataManager.GetInstance().GetTaxlotBaseCollection(_dtDiv);
                        List<Transaction> lsDividendTransactions = CashDataManager.GetInstance().GetDividendEntriesFromTaxlotBase(modifiedTaxlots);

                        _transactionsFromDB.AddRange(lsDividendTransactions);

                    }
                    #endregion
                }
                else
                    MessageBox.Show("Please Select Date Greater then Cash Management Start Date " + CashmgmtStartDate);

            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Persistance Section

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_transactionsFromDB != null && _transactionsFromDB.Count > 0)
                    SaveDataAsync();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }
        

        private void SaveDataAsync()
        {
            try
            {
                BackgroundWorker bgwrkrSaveData = new BackgroundWorker();
                bgwrkrSaveData.DoWork += new DoWorkEventHandler(bgwrkrSaveData_DoWork);
                bgwrkrSaveData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwrkrSaveData_RunWorkerCompleted);
                ChangeStatus(false, false,false);
                bgwrkrSaveData.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
            }
        }

        void bgwrkrSaveData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                #region Trading Activity Section

                if (_lsTRATransactions!=null && _lsTRATransactions.Count > 0)
                    CashDataManager.GetInstance().SaveTransactions(_lsTRATransactions);


                #endregion

                #region Dividend Section

                if (_dtDiv != null && _dtDiv.Rows.Count > 0)
                {
                    foreach (DataRow dr in _dtDiv.Rows)
                        dr.SetModified();
                    CashDataManager.GetInstance().CreateAndSaveJournalEntriesForCashDividend(_dtDiv);
                }

                #endregion

               
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
            }
        }

        void bgwrkrSaveData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                ClearData();
                ChangeStatus(true, false,false);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
            }
        }        

        #endregion

        private void grdCashExceptions_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {

                //double Amount = Convert.ToDouble(e.Row.Cells["Amount"].Value);

                //if (Amount >= 0)
                //{
                //    //e.Row.Appearance.ForeColor = Color.PaleGreen;
                //    e.Row.Appearance.ForeColor = Color.FromArgb(177, 216, 64);
                //}
                //else
                //{

                //    //e.Row.Appearance.ForeColor = Color.IndianRed;
                //    e.Row.Appearance.ForeColor = Color.FromArgb(255, 91, 71);
                //}
                //e.Row.Cells["Amount"].SetValue(Amount.ToString("#0.00"), true);

                if (e.Row.ListObject is TransactionEntry)
                {
                    AccountSideColumnSetting(e.Row);
                }
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

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
                    //if (!isCreditRowSpacingDone)
                    //{

                    //    e.Row.RowSpacingBefore = 10;
                    //    isCreditRowSpacingDone = true;
                    //    //UIElement ele = e.Row.Cells["CR"].Column.Layout.UIElement;

                    //    //Rectangle rec =  e.Row.Cells["CR"].GetUIElement().Rect;
                    //}

                    //e.Row.Appearance.TextHAlign = HAlign.Right;

                }
                //if (ultraTabControlCashMainValue.SelectedTab != null && ultraTabControlCashMainValue.SelectedTab.TabPage == ulTabPageConlNonTradingTrans)
                //{
                //    if (entryAcSide == AccountSide.CR)
                //    {
                //        ultraGridRow.Cells["DR"].Value = 0;
                //        ultraGridRow.Cells["DR"].Activation = Activation.NoEdit;

                //        ultraGridRow.Cells["CR"].Activation = Activation.AllowEdit;
                //    }
                //    else if (entryAcSide == AccountSide.DR)
                //    {
                //        ultraGridRow.Cells["CR"].Value = 0;
                //        ultraGridRow.Cells["CR"].Activation = Activation.NoEdit;

                //        ultraGridRow.Cells["DR"].Activation = Activation.AllowEdit;
                //    }
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCashExceptions_AfterSortChange(object sender, Infragistics.Win.UltraWinGrid.BandEventArgs e)
        {
            if (grdCashExceptions.Rows.IsGroupByRows)
            {
                grdCashExceptions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
            }
            else
            {
                grdCashExceptions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
            }
        }

        private void grdCashExceptions_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            HelperClass.SummarySettings(e);            
        }

        private void grdCashExceptions_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            HelperClass.GroupByRowSetting(e);
        }


        
    }    
}
