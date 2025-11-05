using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.Interfaces;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Infragistics.Win;
using Prana.CommonDataCache;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using System.Timers;
using System.Configuration;
using Prana.Utilities.UIUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.BusinessObjects.AppConstants;
using Prana.Utilities;
using Prana.BusinessLogic;

namespace Prana.CashManagement
{
    public partial class CashTransactions : Form, ICashTransactions
    {
        DataSet ds = null;
        BackgroundWorker bgwSaveAsync;
        BackgroundWorker bgwGetAsync;
        private List<string> _lsColumnsToDisplay = null;
        List<TradeAuditEntry> _tradeAuditCollection_CashTransaction = new List<TradeAuditEntry>();

        public List<string> LsColumnsToDisplay
        {
            get
            {
                if (_lsColumnsToDisplay == null)
                {
                    //PRANA-9777
                    return new List<string>(new string[] { CashManagementConstants.COLUMN_TAXLOTID, CashManagementConstants.COLUMN_CASHTRANSACTIONID, CashManagementConstants.COLUMN_ACTIVITYTYPEID, CashManagementConstants.COLUMN_SYMBOL, CashManagementConstants.COLUMN_STRATEGYID, CashManagementConstants.COLUMN_FUNDID, CashManagementConstants.COLUMN_CURRENCYID, CashManagementConstants.COLUMN_AMOUNT, CashManagementConstants.COLUMN_EXDATE, CashManagementConstants.COLUMN_PAYOUTDATE, CashManagementConstants.COLUMN_RECORDDATE, CashManagementConstants.COLUMN_DECLARATIONDATE, CashManagementConstants.COLUMN_DESCRIPTION, CashManagementConstants.COLUMN_OTHERCURRENCYID, CashManagementConstants.COLUMN_FXRATE, CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR, CashManagementConstants.COLUMN_TRANSACTIONSOURCE, CashManagementConstants.COLUMN_ENTRYDATE, CashManagementConstants.COLUMN_MODIFYDATE });
                }
                else
                {
                    return _lsColumnsToDisplay;
                }
            }
        }

        public CashTransactions()
        {
            try
            {
                InitializeComponent();
                this.Disposed += new EventHandler(CashDividends_Disposed);
                _dictRowsToValidate = new Dictionary<string, List<DataRow>>();
                FillAccountCombo();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void CashDividends_Disposed(object sender, EventArgs e)
        {
            try
            {
                if (FormClosedHandler != null)
                {
                    FormClosedHandler(this, e);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region ICashDividends Members

        public Form Reference()
        {
            return this;
        }

        public event EventHandler FormClosedHandler;

        #endregion

        // Bharat Kumar Jangir (02/07/2013)
        // Filling AccountNames in ComboBox for Filter
        private void FillAccountCombo()
        {
            try
            {
                Dictionary<int, string> _dictAccounts = new Dictionary<int, string>();
                _dictAccounts = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict();
                MultiSelectDropDown1.SetManualTheme(false);
                //add accounts to the check list default value will be unchecked
                MultiSelectDropDown1.AddItemsToTheCheckList(_dictAccounts, CheckState.Checked);

                //adjust checklistbox width according to the longest accountname
                MultiSelectDropDown1.AdjustCheckListBoxWidth();
                MultiSelectDropDown1.TitleText = "Account";
                MultiSelectDropDown1.SetTextEditorText("All Account(s) Selected");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime(dtTo.DateTime), Convert.ToDateTime(dtFrom.DateTime)) >= 0)
                {
                    toolStripStatusLabel1.Text = "Getting data...";
                    ChangeStatus(false);

                    bgwGetAsync = new BackgroundWorker();
                    bgwGetAsync.DoWork += new DoWorkEventHandler(bgwGetAsync_DoWork);
                    bgwGetAsync.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwGetAsync_RunWorkerCompleted);
                    string commaSeparatedAccountIds = MultiSelectDropDown1.GetCommaSeperatedAccountIds();
                    if (!string.IsNullOrEmpty(commaSeparatedAccountIds))
                    {
                        // removing last comma
                        commaSeparatedAccountIds = commaSeparatedAccountIds.Substring(0, commaSeparatedAccountIds.Length - 1);
                        string inputSymbol = GetCompleteText(txtBoxSymbol.Text);
                        List<object> bgwArguments = new List<object>();
                        bgwArguments.Add(commaSeparatedAccountIds);
                        bgwArguments.Add(inputSymbol);
                        bgwGetAsync.RunWorkerAsync(bgwArguments);
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Please select atleast one account.";
                    }
                }
                else
                    MessageBox.Show("To Date is before From Date", "Cash Transactions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                ClearStatus();
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ChangeStatus(bool isCompleted)
        {
            try
            {
                if (isCompleted)
                {
                    btnGet.Enabled = true;
                    btnSave.Enabled = true;
                    btnExport.Enabled = true;
                    grdCashDividends.Enabled = true;
                    toolStripStatusLabel1.Text = "Success.";
                }
                else
                {
                    btnGet.Enabled = false;
                    btnSave.Enabled = false;
                    btnExport.Enabled = false;
                    grdCashDividends.Enabled = false;
                }
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

        void bgwGetAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<object> genericList = e.Argument as List<object>;
                if (genericList != null && genericList.Count > 0)
                {
                    ds = CashDataManager.GetInstance().GetCashDividendsForGivenDates(genericList[1].ToString(), genericList[0].ToString(), dtDateType.Text, dtFrom.DateTime, dtTo.DateTime);
                }
                e.Result = ds;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void bgwGetAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                UpdateGridUI(e.Result as DataSet);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateGridUI(DataSet ds)
        {
            try
            {
                if (ds != null)
                {
                    BindGridData(ds);
                }
                ChangeStatus(true);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindGridData(DataSet ds)
        {
            try
            {
                grdCashDividends.SuspendLayout();
                grdCashDividends.DisplayLayout.Grid.BeginUpdate();
                grdCashDividends.DataSource = ds;
                UltraGridBand band = grdCashDividends.DisplayLayout.Bands[0];
                UltraWinGridUtils.SetColumns(LsColumnsToDisplay, grdCashDividends);
                ValueList fxConversionMethodOperatorValList = new ValueList();
                List<EnumerationValue> fxConversionMethodOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                foreach (EnumerationValue var in fxConversionMethodOperator)
                {
                    if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                    {
                        fxConversionMethodOperatorValList.ValueListItems.Add(var.DisplayText, var.DisplayText);
                    }
                }

                band.Columns[CashManagementConstants.COLUMN_CASHTRANSACTIONID].Hidden = true;
                band.Columns[CashManagementConstants.COLUMN_TAXLOTID].Hidden = false;

                band.Columns[CashManagementConstants.COLUMN_FUNDID].ValueList = GetAccountsValueList();
                band.Columns[CashManagementConstants.COLUMN_FUNDID].Header.Caption = CashManagementConstants.CAPTION_FUND;
                band.Columns[CashManagementConstants.COLUMN_FUNDID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

                band.Columns[CashManagementConstants.COLUMN_STRATEGYID].ValueList = GetStrategyValueList();
                band.Columns[CashManagementConstants.COLUMN_STRATEGYID].Header.Caption = CashManagementConstants.CAPTION_STRATEGY;
                band.Columns[CashManagementConstants.COLUMN_STRATEGYID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                band.Columns[CashManagementConstants.COLUMN_STRATEGYID].Width = 80;

                band.Columns[CashManagementConstants.COLUMN_EXDATE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                band.Columns[CashManagementConstants.COLUMN_EXDATE].Header.Caption = CashManagementConstants.CAPTION_EXDATE;
                band.Columns[CashManagementConstants.COLUMN_EXDATE].Width = 100;

                band.Columns[CashManagementConstants.COLUMN_PAYOUTDATE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                band.Columns[CashManagementConstants.COLUMN_PAYOUTDATE].Header.Caption = CashManagementConstants.CAPTION_PAYOUTDATE;
                band.Columns[CashManagementConstants.COLUMN_PAYOUTDATE].Width = 100;

                band.Columns[CashManagementConstants.COLUMN_SYMBOL].CharacterCasing = CharacterCasing.Upper;
                band.Columns[CashManagementConstants.COLUMN_AMOUNT].Header.Caption = CashManagementConstants.CAPTION_AMOUNT;
                band.Columns[CashManagementConstants.COLUMN_FXRATE].Width = 100;

                band.Columns[CashManagementConstants.COLUMN_CURRENCYID].Header.Caption = CashManagementConstants.CAPTION_CURRENCY;
                band.Columns[CashManagementConstants.COLUMN_CURRENCYID].ValueList = GetCurrencyValueList();
                band.Columns[CashManagementConstants.COLUMN_CURRENCYID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                band.Columns[CashManagementConstants.COLUMN_CURRENCYID].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                band.Columns[CashManagementConstants.COLUMN_CURRENCYID].AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;

                band.Columns[CashManagementConstants.COLUMN_OTHERCURRENCYID].Header.Caption = CashManagementConstants.CAPTION_OTHERCURRENCY;
                band.Columns[CashManagementConstants.COLUMN_OTHERCURRENCYID].ValueList = GetCurrencyValueList();
                band.Columns[CashManagementConstants.COLUMN_OTHERCURRENCYID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;


                band.Columns[CashManagementConstants.COLUMN_RECORDDATE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                band.Columns[CashManagementConstants.COLUMN_RECORDDATE].Header.Caption = CashManagementConstants.CAPTION_RECORDDATE;
                band.Columns[CashManagementConstants.COLUMN_RECORDDATE].Width = 100;

                band.Columns[CashManagementConstants.COLUMN_DECLARATIONDATE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                band.Columns[CashManagementConstants.COLUMN_DECLARATIONDATE].Header.Caption = CashManagementConstants.CAPTION_DECLARATIONDATE;
                band.Columns[CashManagementConstants.COLUMN_DECLARATIONDATE].Width = 110;

                band.Columns[CashManagementConstants.COLUMN_DESCRIPTION].Header.Caption = CashManagementConstants.COLUMN_DESCRIPTION;
                band.Columns[CashManagementConstants.COLUMN_DESCRIPTION].Width = 110;

                band.Columns[CashManagementConstants.COLUMN_ACTIVITYTYPEID].Header.Caption = CashManagementConstants.CAPTION_ACTIVITYTYPE;
                band.Columns[CashManagementConstants.COLUMN_ACTIVITYTYPEID].Width = 150;
                band.Columns[CashManagementConstants.COLUMN_ACTIVITYTYPEID].ValueList = CachedDataManager.GetInstance.GetCashTransactionTypeValueList().Clone();
                band.Columns[CashManagementConstants.COLUMN_ACTIVITYTYPEID].CellActivation = Activation.AllowEdit;
                band.Columns[CashManagementConstants.COLUMN_ACTIVITYTYPEID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                band.Columns[CashManagementConstants.COLUMN_ACTIVITYTYPEID].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                band.Columns[CashManagementConstants.COLUMN_ACTIVITYTYPEID].AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;

                band.Columns[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].ValueList = fxConversionMethodOperatorValList;
                band.Columns[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                band.Columns[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].Width = 100;

                band.Columns[CashManagementConstants.COLUMN_TRANSACTIONSOURCE].Header.Caption = CashManagementConstants.CAPTION_TRANSACTIONSOURCE;
                band.Columns[CashManagementConstants.COLUMN_TRANSACTIONSOURCE].ValueList = GetTransactionSourceValueList();
                band.Columns[CashManagementConstants.COLUMN_TRANSACTIONSOURCE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

                //PRANA-9777
                band.Columns[CashManagementConstants.COLUMN_ENTRYDATE].Header.Caption = CashManagementConstants.CAPTION_ENTRYDATE;
                band.Columns[CashManagementConstants.COLUMN_ENTRYDATE].CellActivation = Activation.NoEdit;
                band.Columns[CashManagementConstants.COLUMN_ENTRYDATE].Hidden = true;

                //PRANA-9777
                band.Columns[CashManagementConstants.COLUMN_MODIFYDATE].Header.Caption = CashManagementConstants.CAPTION_MODIFYDATE;
                band.Columns[CashManagementConstants.COLUMN_MODIFYDATE].CellActivation = Activation.NoEdit;
                band.Columns[CashManagementConstants.COLUMN_MODIFYDATE].Hidden = true;

                SetGridCustomisations();
                grdCashDividends.DisplayLayout.Grid.EndUpdate();
                grdCashDividends.ResumeLayout();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        // completing symbol string on the basis of match on
        private string GetCompleteText(string text)
        {
            if (cbMatchContains.Checked)
            {
                text = "%" + text + "%";
            }
            else if (cbMatchExact.Checked)
            {

            }
            else if (cbMatchStartsWith.Checked)
            {
                text = text + "%";
            }
            return text;
        }

        private ValueList GetCurrencyValueList()
        {
            try
            {
                Dictionary<int, String> dictCurrency = CachedDataManager.GetInstance.GetAllCurrencies();
                ValueList currencyValList = new ValueList();
                foreach (int key in dictCurrency.Keys)
                {
                    currencyValList.ValueListItems.Add(key, dictCurrency[key]);
                }
                return currencyValList;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        private void SetGridCustomisations()
        {
            // Make Rows came through CA uneditble
            try
            {
                foreach (UltraGridRow row in grdCashDividends.Rows)
                {
                    row.Cells[CashManagementConstants.COLUMN_TAXLOTID].Activation = Activation.NoEdit;
                    row.Cells[CashManagementConstants.COLUMN_TRANSACTIONSOURCE].Activation = Activation.NoEdit;
                    if (row.Cells[CashManagementConstants.COLUMN_TAXLOTID].Value != DBNull.Value)
                    {
                        row.Cells[CashManagementConstants.COLUMN_FUNDID].Activation = Activation.NoEdit;
                        row.Cells[CashManagementConstants.COLUMN_CURRENCYID].Activation = Activation.NoEdit;
                        row.Cells[CashManagementConstants.COLUMN_SYMBOL].Activation = Activation.NoEdit;
                        if (Convert.ToInt32(row.Cells[CashManagementConstants.COLUMN_TRANSACTIONSOURCE].Value) == Convert.ToInt32(CashTransactionType.CorpAction))
                        {
                            row.Activation = Activation.NoEdit;
                            row.Appearance.BackColor = Color.Gray;
                            row.Appearance.ForeColor = Color.Orange;
                        }
                    }
                }
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

        private ValueList GetAccountsValueList()
        {
            ValueList accountValList = new ValueList();
            try
            {
                AccountCollection accountCollection = CachedDataManager.GetInstance.GetUserAccounts();
                //To Remove --Select-- From teh List
                if (accountCollection.Count > 0)
                {
                    if (string.IsNullOrEmpty(accountCollection[0].ToString())) //Code change to remove at 0th position only if it doesnt contain any accountname
                        accountCollection.RemoveAt(0);
                }//ValueList accountValList = new ValueList();
                foreach (Account account in accountCollection)
                {
                    accountValList.ValueListItems.Add(account.AccountID, account.Name);
                }

            }

            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            return accountValList;
        }

        private ValueList GetStrategyValueList()
        {
            ValueList strategyValList = new ValueList();
            try
            {
                StrategyCollection strategyCollection = CachedDataManager.GetInstance.GetUserStrategies();
                //To Remove --Select-- From teh List
                if (strategyCollection.Count > 0)
                {
                    if (string.IsNullOrEmpty(strategyCollection[0].ToString())) //Code change to remove at 0th position only if it doesnt contain any accountname
                        strategyCollection.RemoveAt(0);
                }

                strategyValList.ValueListItems.Add(0, "-Select-");
                foreach (Strategy strategy in strategyCollection)
                {
                    strategyValList.ValueListItems.Add(strategy.StrategyID, strategy.Name);
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
            return strategyValList;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClearStatus();
                SetStatus();
                DataSet dataSet = (DataSet)grdCashDividends.DataSource;
                if (dataSet != null && dataSet.GetChanges() != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    if (dataSet.HasErrors)
                    {
                        toolStripStatusLabel1.Text = "Please correct errors.";
                        return;
                    }
                    else
                    {
                        ChangeStatus(false);
                        //since Cash transaction ExDate and Payoutdate plays role to make primary key
                        //so whenever these fields are modified we need to delete exiting row and add a new row.
                        string CashTransactionTypeOrig = string.Empty;
                        string CashTransactionTypeNew = string.Empty;
                        string ExDateOrig = string.Empty;
                        string ExDateNew = string.Empty;
                        string PayOutDateOrig = string.Empty;
                        string PayOutDateNew = string.Empty;
                        string AccountOrig = string.Empty;
                        string AccountNew = string.Empty;
                        string CurrencyIdOrig = string.Empty;
                        string CurrencyIdNew = string.Empty;
                        string AmountOrig = string.Empty;
                        string AmountNew = string.Empty;
                        string FXRateOrig = string.Empty;
                        string FXRateNew = string.Empty;
                        string FXConvMethodOpOrig = string.Empty;
                        string FXConvMethodOpNew = string.Empty;

                        int noOfChangedRows = dataSet.Tables[0].Rows.Count;

                        for (int i = 0; i < noOfChangedRows; i++)
                        {
                            bool isCashTransactionTypeChanged = false;
                            bool isExDateChanged = false;
                            bool isPayoutDateChanged = false;
                            bool isAccountChanged = false;
                            bool isCurrencyIdChanged = false;
                            bool isAmountChanged = false;
                            bool isFxRateChanged = false;
                            bool isFxConvMethodOpChanged = false;
                            if (dataSet.Tables[0].Rows[i].RowState.Equals(DataRowState.Modified))
                            {
                                CashTransactionTypeOrig = dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_ACTIVITYTYPEID, DataRowVersion.Original].ToString();
                                CashTransactionTypeNew = dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_ACTIVITYTYPEID].ToString();
                                ExDateOrig = dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_EXDATE, DataRowVersion.Original].ToString();
                                ExDateNew = dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_EXDATE].ToString();
                                PayOutDateOrig = dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_PAYOUTDATE, DataRowVersion.Original].ToString();
                                PayOutDateNew = dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_PAYOUTDATE].ToString();

                                AccountOrig = Convert.ToString(dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_FUNDID, DataRowVersion.Original]);
                                AccountNew = Convert.ToString(dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_FUNDID]);

                                AmountOrig = Convert.ToString(dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_AMOUNT, DataRowVersion.Original]);
                                AmountNew = Convert.ToString(dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_AMOUNT]);

                                CurrencyIdOrig = Convert.ToString(dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_CURRENCYID, DataRowVersion.Original]);
                                CurrencyIdNew = Convert.ToString(dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_CURRENCYID]);

                                FXRateOrig = Convert.ToString(dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_FXRATE, DataRowVersion.Original]);
                                FXRateNew = Convert.ToString(dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_FXRATE]);

                                FXConvMethodOpOrig = Convert.ToString(dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR, DataRowVersion.Original]);
                                FXConvMethodOpNew = Convert.ToString(dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR]);

                                DataRow newRow = dataSet.Tables[0].NewRow();
                                newRow.ItemArray = dataSet.Tables[0].Rows[i].ItemArray;
                                if (!CashTransactionTypeOrig.Equals(CashTransactionTypeNew))
                                {
                                    isCashTransactionTypeChanged = true;
                                    dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_ACTIVITYTYPEID] = CashTransactionTypeOrig;
                                }
                                if (!ExDateOrig.Equals(ExDateNew))
                                {
                                    isExDateChanged = true;
                                    dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_EXDATE] = ExDateOrig;
                                }
                                if (!PayOutDateOrig.Equals(PayOutDateNew))
                                {
                                    isPayoutDateChanged = true;
                                    dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_PAYOUTDATE] = PayOutDateOrig;
                                }
                                if (!AccountOrig.Equals(AccountNew))
                                {
                                    isAccountChanged = true;
                                    dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_FUNDID] = AccountOrig;
                                }
                                if (!AmountOrig.Equals(AmountNew))
                                {
                                    isAmountChanged = true;
                                    dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_AMOUNT] = AmountOrig;
                                }
                                if (!CurrencyIdOrig.Equals(CurrencyIdNew))
                                {
                                    isCurrencyIdChanged = true;
                                    dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_CURRENCYID] = CurrencyIdOrig;
                                }
                                if (string.IsNullOrEmpty(FXRateOrig))
                                {
                                    FXRateOrig = "0";
                                }
                                if (string.IsNullOrEmpty(FXRateNew))
                                {
                                    FXRateNew = "0";
                                }

                                if (!FXRateOrig.Equals(FXRateNew))
                                {
                                    isFxRateChanged = true;
                                    dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_FXRATE] = FXRateOrig;
                                }
                                if (!string.IsNullOrEmpty(FXConvMethodOpOrig) && !string.IsNullOrEmpty(FXConvMethodOpNew) && !FXConvMethodOpOrig.Equals(FXConvMethodOpNew))
                                {
                                    isFxConvMethodOpChanged = true;
                                    dataSet.Tables[0].Rows[i][CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR] = FXConvMethodOpOrig;
                                }

                                if (isCashTransactionTypeChanged)
                                {
                                    if (dataSet.Tables[0].Rows.Count > 0)
                                    {
                                        AddCashTransactionDividendAuditEntry(dataSet.Tables[0].Rows[i], TradeAuditActionType.ActionType.CashTransaction_Type_Changed, "Cash Transaction Type Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                    }
                                }
                                if (isExDateChanged)
                                {
                                    if (dataSet.Tables[0].Rows.Count > 0)
                                    {
                                        AddCashTransactionDividendAuditEntry(dataSet.Tables[0].Rows[i], TradeAuditActionType.ActionType.CashTransaction_ExDate_Changed, "Cash Transaction Ex Date Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                    }
                                }
                                if (isPayoutDateChanged)
                                {
                                    if (dataSet.Tables[0].Rows.Count > 0)
                                    {
                                        AddCashTransactionDividendAuditEntry(dataSet.Tables[0].Rows[i], TradeAuditActionType.ActionType.CashTransaction_PayoutDate_Changed, "Cash Transaction Payout Date Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                    }
                                }
                                if (isAmountChanged)
                                {
                                    if (dataSet.Tables[0].Rows.Count > 0)
                                    {
                                        AddCashTransactionDividendAuditEntry(dataSet.Tables[0].Rows[i], TradeAuditActionType.ActionType.CashTransaction_Amount_Changed, "Cash Transaction Amount Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                    }
                                }
                                if (isFxRateChanged)
                                {
                                    if (dataSet.Tables[0].Rows.Count > 0)
                                    {
                                        AddCashTransactionDividendAuditEntry(dataSet.Tables[0].Rows[i], TradeAuditActionType.ActionType.CashTransaction_Amount_Changed, "Cash Transaction FX Rate Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                    }
                                }
                                if (isFxConvMethodOpChanged)
                                {
                                    if (dataSet.Tables[0].Rows.Count > 0)
                                    {
                                        AddCashTransactionDividendAuditEntry(dataSet.Tables[0].Rows[i], TradeAuditActionType.ActionType.CashTransaction_Amount_Changed, "Cash Transaction FX Conversion Method Operator Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                    }
                                }
                                if ((isCashTransactionTypeChanged || isExDateChanged || isPayoutDateChanged || isAccountChanged) || isCurrencyIdChanged || isAmountChanged || isFxRateChanged || isFxConvMethodOpChanged)
                                {
                                    dataSet.Tables[0].Rows[i].Delete();
                                    dataSet.Tables[0].Rows.Add(newRow);
                                }
                            }
                            if (dataSet.Tables[0].Rows[i].RowState.Equals(DataRowState.Added))
                            {
                                AddCashTransactionDividendAuditEntry(dataSet.Tables[0].Rows[i], TradeAuditActionType.ActionType.Dividend_Applied_CashTransaction, "Cash Transaction Dividend Applied", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                            }
                        }
                        if (_tradeAuditCollection_CashTransaction != null && _tradeAuditCollection_CashTransaction.Count >= 1)
                        {
                            AuditManager.Instance.SaveAuditList(_tradeAuditCollection_CashTransaction);
                            _tradeAuditCollection_CashTransaction.Clear();
                        }
                    }

                    if (_currentRowInValidationProcess == null)
                    {
                        DataSet dsChanges = dataSet.GetChanges();
                        bgwSaveAsync = new BackgroundWorker();
                        bgwSaveAsync.DoWork += new DoWorkEventHandler(bgwSaveAsync_DoWork);
                        bgwSaveAsync.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwSaveAsync_RunWorkerCompleted);
                        bgwSaveAsync.RunWorkerAsync(dsChanges);
                    }
                    else
                    {
                        ChangeStatus(true);
                        toolStripStatusLabel1.Text = "Please wait, symbol is in validation process.";
                    }
                }
                else
                    toolStripStatusLabel1.Text = "Nothing to save.";
            }
            catch (Exception ex)
            {
                ChangeStatus(true);
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds entry to the Audit List for the Cash Dividend applied from Cash Transaction UI
        /// </summary>
        /// <param name="modifiedTaxlot">Not Null, the Data collection from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the action by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        public bool AddCashTransactionDividendAuditEntry(DataRow row, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            try
            {
                if (row != null && comment != null)
                {
                    if (!row.RowState.Equals(DataRowState.Deleted))
                    {
                        if (row["TaxlotId"] == null)
                        {
                            return false;
                        }
                        TradeAuditEntry newEntry = new TradeAuditEntry();
                        newEntry.AuditId = AuditManager.Instance.GetAuditId();
                        newEntry.Action = action;
                        newEntry.AUECLocalDate = DateTime.Now;
                        if (row["ExDate"] != null)
                        {
                            newEntry.OriginalDate = DateTime.Parse(row["ExDate"].ToString());
                        }
                        newEntry.Comment = comment;
                        newEntry.CompanyUserId = currentUserID;
                        newEntry.Symbol = row["Symbol"].ToString();
                        newEntry.Level1ID = Int32.Parse(row["FundID"].ToString());
                        newEntry.TaxLotClosingId = "";
                        newEntry.TaxLotID = row["TaxlotId"].ToString();
                        if (String.IsNullOrEmpty(newEntry.TaxLotID))
                            newEntry.GroupID = int.MinValue.ToString();
                        else
                            newEntry.GroupID = newEntry.TaxLotID.Substring(0, 13);
                        newEntry.OrderSideTagValue = "";
                        if (action.ToString() == "Dividend_Applied_CashTransaction")
                        {
                            newEntry.OriginalValue = "0";
                        }
                        else if (action.ToString() == "CashTransaction_Type_Changed")
                        {
                            newEntry.OriginalValue = row["ActivityTypeId"].ToString();
                        }
                        else if (action.ToString() == "CashTransaction_ExDate_Changed")
                        {
                            newEntry.OriginalValue = row["ExDate"].ToString();
                        }
                        else if (action.ToString() == "CashTransaction_PayoutDate_Changed")
                        {
                            newEntry.OriginalValue = row["PayoutDate"].ToString();
                        }
                        else if (action.ToString() == "CashTransaction_Amount_Changed")
                        {
                            if (comment == "Cash Transaction FX Rate Changed")
                            {
                                newEntry.OriginalValue = row["FxRate"].ToString();
                            }
                            else if (comment == "Cash Transaction FX Conversion Method Operator Changed")
                            {
                                newEntry.OriginalValue = row["FXConversionMethodOperator"].ToString();
                            }
                            else
                            {
                                newEntry.OriginalValue = row["Amount"].ToString();
                            }
                        }
                        if (action.ToString() == "Dividend_UnApplied_CashTransaction")
                        {
                            newEntry.OriginalValue = row["Amount"].ToString();
                        }
                        _tradeAuditCollection_CashTransaction.Add(newEntry);
                    }
                }
                else
                    throw new NullReferenceException("The Data Set to add in audit dictionary is null");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        private void SetStatus()
        {
            toolStripStatusLabel1.Text = "Saving Data...";
        }

        void bgwSaveAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataSet updatedDT = CashDataManager.GetInstance().SaveManualCashDividend(e.Argument as DataSet);
                e.Result = updatedDT;

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void bgwSaveAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                DataSet updatedDT = e.Result as DataSet;
                ds.Clear();
                foreach (DataRow dr in updatedDT.Tables[0].Rows)
                    ds.Tables[0].ImportRow(dr);
                ds.AcceptChanges();
                UpdateGridUI(ds);
                toolStripStatusLabel1.Text = "Data saved.";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

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
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

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
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
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

                workBook = this.ultraGridExcelExporter1.Export(this.grdCashDividends, workBook.Worksheets[workbookName]);
                workBook.Save(pathName);
                result = true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Adds new cash Transaction Type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ds == null)
                {
                    SetGridColumns();
                }
                if (ds != null)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    //when adding new cash transaction, add default cash transactiontype to DivL
                    if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_ACTIVITYTYPEID))
                    {
                        if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TRANSACTIONSOURCE))
                            row[CashManagementConstants.COLUMN_TRANSACTIONSOURCE] = Convert.ToInt32(CashTransactionType.CashTransaction);

                        //PRANA-9777
                        if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_ENTRYDATE))
                            row[CashManagementConstants.COLUMN_ENTRYDATE] = DateTime.Now;

                        ds.Tables[0].Rows.Add(row);
                    }
                }
                BindGridData(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetGridColumns()
        {
            try
            {
                DataSet dsNew = new DataSet();
                dsNew.Tables.Add("dtCashTransactions");
                foreach (string col in LsColumnsToDisplay)
                {
                    dsNew.Tables[0].Columns.Add(col);
                }
                ds = dsNew;
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

        private void menuDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdCashDividends.ActiveRow != null)
                {
                    DataRow row = ((System.Data.DataRowView)(grdCashDividends.ActiveRow.ListObject)).Row;
                    if (!string.IsNullOrEmpty(row["ExDate"].ToString()))
                    {
                        //DataSet updatedDT = CreateDataSetFromDataRow(row);
                        AddCashTransactionDividendAuditEntry(row, TradeAuditActionType.ActionType.Dividend_UnApplied_CashTransaction, "Cash Transaction Dividend UnApplied", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                        AuditManager.Instance.SaveAuditList(_tradeAuditCollection_CashTransaction);
                        _tradeAuditCollection_CashTransaction.Clear();
                    }
                    row.Delete();
                    grdCashDividends.ActiveRow = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //private DataSet CreateDataSetFromDataRow(DataRow row)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        DataTable dTCashDividend = new DataTable();
        //        dTCashDividend.TableName = "CashTransactionDividend";
        //        dTCashDividend.Columns.Add(new DataColumn("ExDate"));
        //        dTCashDividend.Columns.Add(new DataColumn("Symbol"));
        //        dTCashDividend.Columns.Add(new DataColumn("FundID"));
        //        dTCashDividend.Columns.Add(new DataColumn("TaxlotId"));
        //        dTCashDividend.Columns.Add(new DataColumn("Amount"));

        //        DataRow drNew = dTCashDividend.NewRow();
        //        drNew["ExDate"] = DateTime.Parse(row["ExDate"].ToString());
        //        drNew["Symbol"] = row["Symbol"].ToString().ToUpper();
        //        drNew["FundID"] = row["FundID"].ToString();
        //        drNew["TaxlotId"] = row["TaxlotId"].ToString();
        //        drNew["Amount"] = row["Amount"].ToString();
        //        dTCashDividend.Rows.Add(drNew);
        //        dTCashDividend.AcceptChanges();

        //        ds.Tables.Add(dTCashDividend);                
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return ds;
        //}

        private void grdCashDividends_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                //UltraGridRow row = grdCashDividends.ActiveRow;
                menuAdd.Enabled = true;
                menuDeleteRow.Enabled = true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCashDividends_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (!e.ReInitialize)
                {
                    if (String.IsNullOrEmpty(e.Row.Cells[CashManagementConstants.COLUMN_SYMBOL].Text))
                    {
                        e.Row.Cells[CashManagementConstants.COLUMN_RECORDDATE].Activation = Activation.NoEdit;
                        e.Row.Cells[CashManagementConstants.COLUMN_DECLARATIONDATE].Activation = Activation.NoEdit;
                    }
                    else
                    {
                        e.Row.Cells[CashManagementConstants.COLUMN_RECORDDATE].Activation = Activation.AllowEdit;
                        e.Row.Cells[CashManagementConstants.COLUMN_DECLARATIONDATE].Activation = Activation.AllowEdit;
                    }

                    DataRow row = ((System.Data.DataRowView)(e.Row.ListObject)).Row;

                    if (row[CashManagementConstants.COLUMN_FUNDID].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError(CashManagementConstants.COLUMN_FUNDID, "Select  Account!");
                    }
                    if (row[CashManagementConstants.COLUMN_AMOUNT].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError(CashManagementConstants.COLUMN_AMOUNT, "Dividend Value can't be null!");
                    }
                    if (row[CashManagementConstants.COLUMN_STRATEGYID].ToString().Equals(String.Empty))
                    {
                        row[CashManagementConstants.COLUMN_STRATEGYID] = 0;
                    }
                    if (row[CashManagementConstants.COLUMN_CURRENCYID].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError(CashManagementConstants.COLUMN_CURRENCYID, "Select Currency !");
                    }
                    if (row[CashManagementConstants.COLUMN_PAYOUTDATE].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError(CashManagementConstants.COLUMN_PAYOUTDATE, "Select Payout Date !");
                    }
                    if (row[CashManagementConstants.COLUMN_EXDATE].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError(CashManagementConstants.COLUMN_EXDATE, "Select  ExDate !");
                    }
                    if (row[CashManagementConstants.COLUMN_ACTIVITYTYPEID].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYTYPEID, "Select valid ActivityType !");
                    }

                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetError(string caption, DataRow row, CellEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.Cell.Text))
                {
                    row.SetColumnError(caption, ApplicationConstants.C_COMBO_SELECT + caption + "!");
                }
                else
                {
                    row.SetColumnError(caption, "");
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCashDividends_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                string column = e.Cell.Column.Key;
                ClearStatus();
                switch (column)
                {
                    case CashManagementConstants.COLUMN_FUNDID:
                        SetError(column, row, e);
                        break;

                    case CashManagementConstants.COLUMN_AMOUNT:
                        SetError(column, row, e);
                        break;

                    case CashManagementConstants.COLUMN_CURRENCYID:
                        SetError(column, row, e);
                        if (CachedData.CompanyBaseCurrencyID == CachedDataManager.GetInstance.GetCurrencyID(e.Cell.Text))
                        {
                            row[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR] = 'M';
                            row[CashManagementConstants.COLUMN_FXRATE] = 1;
                        }
                        break;

                    case CashManagementConstants.COLUMN_EXDATE:
                        SetError(column, row, e);
                        break;

                    case CashManagementConstants.COLUMN_PAYOUTDATE:
                        SetError(column, row, e);
                        break;

                    case CashManagementConstants.COLUMN_SYMBOL:
                        //symbol can be null in case if there are non dividend cash transactions
                        if (string.IsNullOrEmpty(e.Cell.Text))
                        {
                            row.SetColumnError(column, string.Empty);
                        }
                        else
                            SetError(column, row, e);
                        break;

                    case CashManagementConstants.COLUMN_ACTIVITYTYPEID:
                        SetError(column, row, e);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ClearStatus()
        {
            toolStripStatusLabel1.Text = string.Empty;
        }

        #region Symbol Validation Section
        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                _securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                _securityMaster.ResponseCompleted += new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
            }
        }

        void _securityMaster_ResponseCompleted(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                toolStripStatusLabel1.Text += ": " + e.Value.Message.ToString();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        object _validationLocker = new object();
        DataRow _currentRowInValidationProcess = null;
        void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                lock (_validationLocker)
                {
                    SecMasterBaseObj secMasterObj = e.Value;
                    if (_dictRowsToValidate.ContainsKey(secMasterObj.TickerSymbol))
                    {
                        foreach (DataRow dr in _dictRowsToValidate[secMasterObj.TickerSymbol])
                        {
                            dr.SetColumnError(CashManagementConstants.COLUMN_SYMBOL, null);
                        }
                        _dictRowsToValidate.Remove(secMasterObj.TickerSymbol);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        Dictionary<string, List<DataRow>> _dictRowsToValidate;
        public void ValidateSymbol(string text, DataRow row)
        {
            try
            {
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    SecMasterRequestObj reqObj = new SecMasterRequestObj();
                    reqObj.AddData(text, ApplicationConstants.PranaSymbology);
                    reqObj.HashCode = this.GetHashCode();

                    if (row != null)
                        row.SetColumnError(CashManagementConstants.COLUMN_SYMBOL, "Symbol Not Validated !");
                    lock (_validationLocker)
                    {
                        if (_dictRowsToValidate.ContainsKey(text))
                            _dictRowsToValidate[text].Add(row);
                        else
                        {
                            _dictRowsToValidate.Add(text, new List<DataRow>());
                            _dictRowsToValidate[text].Add(row);
                        }
                    }
                    _securityMaster.SendRequest(reqObj);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        void grdCashDividends_CellDataError(object sender, Infragistics.Win.UltraWinGrid.CellDataErrorEventArgs e)
        {
            try
            {
                e.RestoreOriginalValue = true;
                e.RaiseErrorEvent = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void grdCashDividends_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                ClearStatus();
                DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                if (string.IsNullOrEmpty(e.Cell.Row.Cells[CashManagementConstants.COLUMN_AMOUNT].Text))
                {
                    row.SetColumnError(CashManagementConstants.COLUMN_AMOUNT, "Amount can be only numeric");
                }

                if (e.Cell.Column.Header.Caption == CashManagementConstants.COLUMN_SYMBOL)
                {
                    if (String.IsNullOrEmpty(e.Cell.Text))
                    {
                        e.Cell.Row.Cells[CashManagementConstants.COLUMN_RECORDDATE].Value = DBNull.Value;
                        e.Cell.Row.Cells[CashManagementConstants.COLUMN_DECLARATIONDATE].Value = DBNull.Value;

                        e.Cell.Row.Cells[CashManagementConstants.COLUMN_RECORDDATE].Activation = Activation.NoEdit;
                        e.Cell.Row.Cells[CashManagementConstants.COLUMN_DECLARATIONDATE].Activation = Activation.NoEdit;
                        //row[CashManagementConstants.COLUMN_COMPANYNAME] = string.Empty;
                    }
                    else
                    {
                        e.Cell.Row.Cells[CashManagementConstants.COLUMN_RECORDDATE].Activation = Activation.AllowEdit;
                        e.Cell.Row.Cells[CashManagementConstants.COLUMN_DECLARATIONDATE].Activation = Activation.AllowEdit;
                        //row[CashManagementConstants.COLUMN_COMPANYNAME] = string.Empty;
                        string enteredSymbol = e.Cell.Row.Cells[CashManagementConstants.COLUMN_SYMBOL].Value.ToString();
                        ValidateSymbol(enteredSymbol, row);
                    }
                }
                //Raturi: select default fxconversionmethodoperator
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-7520
                else if (e.Cell.Column.Header.Caption == CashManagementConstants.COLUMN_CURRENCY)
                {
                    if (!string.IsNullOrWhiteSpace(e.Cell.Text))
                    {
                        if (!CachedDataManager.GetInstance.GetAllCurrencies().ContainsValue(e.Cell.Text))
                        {
                            e.Cell.Value = string.Empty;
                        }
                        else
                        {
                            e.Cell.Row.Cells[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].Value = 'M';
                        }
                    }
                    else
                    {
                        row.SetColumnError(CashManagementConstants.COLUMN_CURRENCYID, "Select Valid Currency !");
                    }
                }
                else if (e.Cell.Column.Key.Equals(CashManagementConstants.COLUMN_AMOUNT))
                {
                    if (!string.IsNullOrEmpty(e.Cell.Text))
                    {
                        double amount;
                        if (!double.TryParse(e.Cell.Text, out amount))
                        {
                            e.Cell.Value = string.Empty;
                        }
                    }
                    else
                    {
                        row.SetColumnError(CashManagementConstants.COLUMN_AMOUNT, "Amount can be only numeric");
                    }
                }


                #region Handling for CashTransfer Activity

                else if (e.Cell.Column.Key.Equals(CashManagementConstants.COLUMN_ACTIVITYTYPEID))
                {
                    if (!string.IsNullOrEmpty(e.Cell.Text))
                    {
                        bool isExist = false;
                        foreach (ValueListItem item in CachedDataManager.GetInstance.GetCashTransactionTypeValueList().ValueListItems)
                        {
                            if (e.Cell.Text.Equals(item.DisplayText))
                            {
                                isExist = true;
                                break;
                            }
                        }

                        if (!isExist)
                        {
                            e.Cell.Value = string.Empty;
                        }
                        else
                        {
                            if (e.Cell.Text.ToString().Equals(Activities.CashTransfer.ToString()))
                            {
                                if (string.IsNullOrEmpty((e.Cell.Row.Cells[CashManagementConstants.COLUMN_OTHERCURRENCYID].Value.ToString())))
                                    row.SetColumnError(CashManagementConstants.COLUMN_OTHERCURRENCYID, "VsCurrency is Compulsory.");
                                if (string.IsNullOrEmpty((e.Cell.Row.Cells[CashManagementConstants.COLUMN_FXRATE].Value.ToString())))
                                    row.SetColumnError(CashManagementConstants.COLUMN_FXRATE, "FxRate is Compulsory.");
                                if (string.IsNullOrEmpty((e.Cell.Row.Cells[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].Value.ToString())))
                                    row.SetColumnError(CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR, "FX Conversion Method Operator  is Compulsory.");
                            }
                            else
                            {
                                row.SetColumnError(CashManagementConstants.COLUMN_OTHERCURRENCYID, "");
                                if (string.IsNullOrEmpty((e.Cell.Row.Cells[CashManagementConstants.COLUMN_FXRATE].Value.ToString())))
                                    row.SetColumnError(CashManagementConstants.COLUMN_FXRATE, "");
                                row.SetColumnError(CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR, "");
                            }
                        }
                    }
                    else
                    {
                        row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYTYPEID, "Select valid ActivityType !");
                    }
                }

                #endregion

                else if (e.Cell.Column.Key.Equals(CashManagementConstants.COLUMN_OTHERCURRENCYID) && !string.IsNullOrEmpty(e.Cell.Text.ToString()))
                    row.SetColumnError(CashManagementConstants.COLUMN_OTHERCURRENCYID, "");

                else if (e.Cell.Column.Key.Equals(CashManagementConstants.COLUMN_FXRATE, StringComparison.OrdinalIgnoreCase))
                {
                    double fxrate;
                    if (!String.IsNullOrEmpty(e.Cell.Text.ToString()))
                    {
                        if (double.TryParse(e.Cell.Text, out fxrate))
                            row.SetColumnError(CashManagementConstants.COLUMN_FXRATE, "");
                        else
                            row.SetColumnError(CashManagementConstants.COLUMN_FXRATE, "FXRate can be only numeric.");
                    }
                    else if (String.IsNullOrEmpty(e.Cell.Text.ToString()) && !e.Cell.Row.Cells[CashManagementConstants.COLUMN_ACTIVITYTYPEID].Value.ToString().Equals(Activities.CashTransfer.ToString()))
                        row.SetColumnError(CashManagementConstants.COLUMN_FXRATE, "");
                }

                else if (e.Cell.Column.Key.Equals(CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR) && !string.IsNullOrEmpty(e.Cell.Text.ToString()))
                    row.SetColumnError(CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR, "");
            }

            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void contextMenuCashDividends_Popup(object sender, EventArgs e)
        {
            try
            {
                UltraGridRow row = grdCashDividends.ActiveRow;
                if (row != null)
                {
                    if (grdCashDividends.DataSource == null || grdCashDividends.Rows.Count == 0 || (row.Cells[CashManagementConstants.COLUMN_TAXLOTID].Value != DBNull.Value && Convert.ToInt32(row.Cells[CashManagementConstants.COLUMN_TRANSACTIONSOURCE].Value) == Convert.ToInt32(CashTransactionType.CorpAction)))
                    {
                        menuDeleteRow.Enabled = false;
                        return;
                    }
                    if (grdCashDividends.ActiveRow == null || grdCashDividends.ActiveRow.Band.Index != 0)
                    {
                        menuDeleteRow.Enabled = false;
                        return;
                    }

                    menuAdd.Enabled = true;
                }
                else
                {
                    menuDeleteRow.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCashDividends_MouseDown(object sender, MouseEventArgs e)
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
            catch (Exception)
            {
                //Do Nothing as user can try again
            }
        }

        //added by: Bharat raturi, 30 sep 2014
        //refresh the activity data whenever the form activates
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-4928
        private void CashTransactions_Activated(object sender, EventArgs e)
        {
            try
            {
                //CachedDataManager.GetInstance.RefreshAccountData();
                if (grdCashDividends.DisplayLayout != null && grdCashDividends.DisplayLayout.Bands.Count > 0 && grdCashDividends.DisplayLayout.Bands[0].Columns.Exists(CashManagementConstants.COLUMN_ACTIVITYTYPEID))
                {
                    grdCashDividends.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYTYPEID].ValueList = CachedDataManager.GetInstance.GetCashTransactionTypeValueList().Clone();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CashTransactions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bgwSaveAsync != null)
            {
                if (bgwSaveAsync.IsBusy)
                {
                    if (bgwSaveAsync.WorkerSupportsCancellation)
                    {
                        bgwSaveAsync.CancelAsync();
                    }
                }
                bgwSaveAsync.DoWork -= new DoWorkEventHandler(bgwSaveAsync_DoWork);
                bgwSaveAsync.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(bgwSaveAsync_RunWorkerCompleted);
            }
            if (bgwGetAsync != null)
            {
                if (bgwGetAsync.IsBusy)
                {
                    if (bgwGetAsync.WorkerSupportsCancellation)
                    {
                        bgwGetAsync.CancelAsync();
                    }
                }
                bgwGetAsync.DoWork -= new DoWorkEventHandler(bgwGetAsync_DoWork);
                bgwGetAsync.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(bgwGetAsync_RunWorkerCompleted);
            }
        }

        private void CashTransactions_Load(object sender, EventArgs e)
        {
            try
            {
                if (CachedDataManager.GetInstance.GetPranaReleaseViewType() != PranaReleaseViewType.CHMiddleWare && CustomThemeHelper.ApplyTheme)
                {
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CASH_MANAGEMENT);
                }
                else
                {
                    //Purpose:- Applying theme on the form   Modified by : sachin mishra 24 feb 15                 
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                }
                if (CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnGet.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGet.ForeColor = System.Drawing.Color.White;
                btnGet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGet.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGet.UseAppStyling = false;
                btnGet.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CashTransactions_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.Height = Screen.PrimaryScreen.Bounds.Height;
                    this.Width = Screen.PrimaryScreen.Bounds.Width;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCashDividends_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdCashDividends);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Added by Nishant Jain
        /// To add new line when called this UI from Allocation
        /// </summary>
        /// <param name="taxlotToAddCash"></param>
        public void AddCashTransaction(TaxLot taxlotToAddCash)
        {
            try
            {
                this.BringToFront();
                //isRowAdded = true;
                if (ds == null)
                {
                    SetGridColumns();
                }
                if (ds != null)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    //when adding new cash transaction, add default cash transactiontype to DivL
                    if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_ACTIVITYTYPEID))
                    {
                        if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TAXLOTID))
                            row[CashManagementConstants.COLUMN_TAXLOTID] = taxlotToAddCash.TaxLotID;

                        if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TRANSACTIONSOURCE))
                            row[CashManagementConstants.COLUMN_TRANSACTIONSOURCE] = Convert.ToInt32(CashTransactionType.CashTransaction);

                        if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_FUNDID))
                            row[CashManagementConstants.COLUMN_FUNDID] = taxlotToAddCash.Level1ID;

                        if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_SYMBOL))
                            row[CashManagementConstants.COLUMN_SYMBOL] = taxlotToAddCash.Symbol;

                        if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_CURRENCYID))
                            row[CashManagementConstants.COLUMN_CURRENCYID] = taxlotToAddCash.CurrencyID;

                        //PRANA-9777
                        if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_ENTRYDATE))
                            row[CashManagementConstants.COLUMN_ENTRYDATE] = DateTime.Now;

                        ds.Tables[0].Rows.Add(row);
                    }
                }
                BindGridData(ds);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private ValueList GetTransactionSourceValueList()
        {
            try
            {
                ValueList currencyValList = new ValueList();
                foreach (CashTransactionType cashType in Enum.GetValues(typeof(CashTransactionType)))
                    currencyValList.ValueListItems.Add(Convert.ToInt32(cashType), EnumHelper.GetDescription(cashType));

                return currencyValList;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        private void grdCashDividends_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

    }
}