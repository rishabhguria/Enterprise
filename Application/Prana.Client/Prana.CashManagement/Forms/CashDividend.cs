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

namespace Prana.CashManagement
{
    public partial class CashDividends : Form, ICashDividends
    {
        DataSet ds = null;
        bool isRowAdded = false;
        public CashDividends()
        {
            try
            {
            InitializeComponent();
            this.Disposed += new EventHandler(CashDividends_Disposed);
                _dictRowsToValidate = new Dictionary<string, List<DataRow>>();
                FillFundCombo();
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
	// Filling FundNames in ComboBox for Filter
        private void FillFundCombo()
        {
            Dictionary<int, string> _dictFunds = new Dictionary<int, string>();
            _dictFunds = CommonDataCache.CachedDataManager.GetInstance.GetFundsWithFullName();
            //add funds to the check list dafult value will be unchecked
            MultiSelectDropDown1.AddItemsToTheCheckList(_dictFunds, CheckState.Checked);
            
            //adjust checklistbox width according to the longest fundname
            MultiSelectDropDown1.AdjustCheckListBoxWidth();
            MultiSelectDropDown1.SetTextEditorText("All Fund(s) Selected");
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            //ds = CashAccountDataManager.GetCashDividendsForGivenDates(txtBoxSymbol.Text, dtFrom.DateTime, dtTo.DateTime);
            try
            {
                toolStripStatusLabel1.Text = string.Empty;

                string commaSeparatedFundIds = MultiSelectDropDown1.GetCommaSeperatedFundIds();
                //dont make db call if no fund selected
                if (!string.IsNullOrEmpty(commaSeparatedFundIds))
                {
                    string inputSymbol = GetCompleteText(txtBoxSymbol.Text);
                    // removing last comma
                    commaSeparatedFundIds = commaSeparatedFundIds.Substring(0, commaSeparatedFundIds.Length - 1);
                    ds = CashDataManager.GetInstance().GetCashDividendsForGivenDates(inputSymbol, commaSeparatedFundIds, dtDateType.Text, dtFrom.DateTime, dtTo.DateTime);
                    BindGridData(ds);
                }
                else
                {
                    toolStripStatusLabel1.Text = "Please select atleast one fund.";
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
        private void BindGridData(DataSet ds)
        {
            try
            {
                grdCashDividends.DataSource = ds;

                UltraGridBand band = grdCashDividends.DisplayLayout.Bands[0];
                List<string> lsColumnsToDisplay = null;
                if (lsColumnsToDisplay == null)
                    lsColumnsToDisplay = new List<string>(new string[] { "Symbol", "CompanyName", "FundID", "CurrencyID", "Dividend", "ExDate", "PayoutDate", "RecordDate", "DeclarationDate", "Description", "DivRate" });

                UltraWinGridUtils.SetColumns(lsColumnsToDisplay, grdCashDividends);
               

                band.Columns["DivPKId"].Hidden = true;
                band.Columns["TaxlotId"].Hidden = true;
                band.Columns["CorpActionId"].Hidden = true;
                band.Columns["ParentRow_PK"].Hidden = true;
                band.Columns["FundID"].ValueList = GetFundsValueList();
                band.Columns["FundID"].Header.Caption = "Fund";
                band.Columns["ExDate"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                band.Columns["ExDate"].Width = 100;
                band.Columns["FundID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                band.Columns["PayoutDate"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                band.Columns["PayoutDate"].Header.Caption = "Payout Date";
                band.Columns["PayoutDate"].Width = 100;
               
                band.Columns["Symbol"].CharacterCasing = CharacterCasing.Upper;
                band.Columns["CurrencyID"].Header.Caption = "Currency";
                band.Columns["CurrencyID"].ValueList = GetCurrencyValueList();
                band.Columns["CurrencyID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                band.Columns["Dividend"].Header.Caption = "Dividend";
                band.Columns["DivRate"].Header.Caption = "Div Rate";
                band.Columns["DivRate"].CellActivation = Activation.NoEdit;
                band.Columns["RecordDate"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                band.Columns["RecordDate"].Header.Caption = "Record Date";
                band.Columns["RecordDate"].Width = 100;
                band.Columns["DeclarationDate"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                band.Columns["DeclarationDate"].Header.Caption = "Declaration Date";
                band.Columns["DeclarationDate"].Width = 110;
                band.Columns["Description"].Header.Caption = "Description";
                band.Columns["Description"].Width = 110;
                band.Columns["CompanyName"].Header.Caption = "Company Name";
                band.Columns["CompanyName"].Width = 150;
                band.Columns["CompanyName"].CellActivation = Activation.NoEdit;
                SetGridCustomisations();
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
            // Make Rows came throgh CA uneditble
            try
            {
            foreach (UltraGridRow row in grdCashDividends.Rows)
            {
                if (row.Cells["CorpActionId"].Value != DBNull.Value)
                {
                    row.Activation = Activation.NoEdit;
                    row.Appearance.BackColor = Color.Gray;
                    row.Appearance.ForeColor = Color.Orange;
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

        private ValueList GetFundsValueList()
        {
            ValueList fundValList = new ValueList();
            try
            {
                FundCollection fundCollection = CachedDataManager.GetInstance.GetUserFunds();
                //To Remove --Select-- From teh List
                if (fundCollection.Count > 0)
                {
                    if(string.IsNullOrEmpty(fundCollection[0].ToString())) //Code change to remove at 0th position only if it doesnt contain any fundname
                    fundCollection.RemoveAt(0);
                }//ValueList fundValList = new ValueList();
                foreach (Fund fund in fundCollection)
                {
                    fundValList.ValueListItems.Add(fund.FundID, fund.Name);
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
            return fundValList;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dataSet = (DataSet)grdCashDividends.DataSource;

                if (dataSet != null && dataSet.GetChanges() != null && dataSet.GetChanges().Tables[0].Rows.Count > 0)
                {
                    if (_currentRowInValidationProcess == null)
                    {
                        if (!dataSet.HasErrors)
                        {
                            DataSet dsChanges=dataSet.GetChanges();
                            DataTable updatedDT = CashDataManager.GetInstance().SaveMannualCashDividend(dsChanges);
                            toolStripStatusLabel1.Text = "Data saved.";
                            if(dataSet.GetChanges(DataRowState.Added)!=null )
                                btnGet_Click(null, null);
                        }
                        else
                            toolStripStatusLabel1.Text = "Please correct errors.";
                    }
                    else
                        toolStripStatusLabel1.Text = "Please wait, symbol is in validation process.";

                }
                else
                    toolStripStatusLabel1.Text = "Nothing to save.";
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
            if (ExportToExcel())
            {
                MessageBox.Show("Report Succesfully saved.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void mnuAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                isRowAdded = true;
                if (ds == null)
                    btnGet_Click(null, null);
                if (ds != null)
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
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
            //else
            //{
            //    MessageBox.Show("Please Hit Get Button Once !", "Prana", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }


        private void mnuDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
            if (grdCashDividends.ActiveRow != null)
            {
                DataRow row = ((System.Data.DataRowView)(grdCashDividends.ActiveRow.ListObject)).Row;                
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
        private void grdCashDividends_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
            toolStripStatusLabel1.Text = string.Empty;
            UltraGridRow row = grdCashDividends.ActiveRow;

            if (row.Cells["CorpActionId"].Value != DBNull.Value)
            {
                mnuDeleteRow.Enabled = false;
            }
            else
            {
                mnuDeleteRow.Enabled = true;
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

        private void grdCashDividends_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (!e.ReInitialize)
                {
                    if (isRowAdded)
                    {
                        e.Row.Activate();
                        e.Row.Band.Columns["DivRate"].CellActivation = Activation.NoEdit;                        
                        e.Row.Band.Columns["CompanyName"].CellActivation = Activation.NoEdit;                        
                        isRowAdded = false;
                    }
                    DataRow row = ((System.Data.DataRowView)(e.Row.ListObject)).Row;

                    toolStripStatusLabel1.Text = string.Empty;
                    if (row["FundID"].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError("FundID", "Select  Fund!");
                    }

                    if (row["Dividend"].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError("Dividend", "Dividend Value can't be null!");
                    }

                    if (row["CurrencyID"].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError("CurrencyID", "Select  Currency !");
                    }
                    if (row["PayoutDate"].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError("PayoutDate", "Select Payout Date !");
                    }
                    if (row["ExDate"].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError("ExDate", "Select  ExDate !");
                    }
                    if (row["Symbol"].ToString().Equals(String.Empty))
                    {
                        row.SetColumnError("Symbol", "Symbol can't be null!");
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
                if (row[caption].ToString().Equals(String.Empty))
                {
                    if (e.Cell.Text == string.Empty)
                    {
                        row.SetColumnError(caption, ApplicationConstants.C_COMBO_SELECT + caption + "!");
                    }
                    else
                    {
                        row.SetColumnError(caption, "");
                    }
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
                toolStripStatusLabel1.Text = string.Empty;
                switch (column)
                {
                    case "FundID":
                        SetError(column, row, e);
                        break;
                    case "Dividend":
                        SetError(column, row, e);
                        break;
                    case "CurrencyID":
                        SetError(column, row, e);
                        break;
                    case "ExDate":
                        SetError(column, row, e);
                        break;
                    case "PayoutDate":
                        SetError(column, row, e);
                        break;
                    case "Symbol":
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

        #region Symbol Validation Section
        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                _securityMaster.SecMstrDataResponse += new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);
                _securityMaster.ResponseCompleted += new CompletedReceivedDelegate(_securityMaster_ResponseCompleted);
            }
        }

        void _securityMaster_ResponseCompleted(QueueMessage qMsg)
        {
            try
            {
                toolStripStatusLabel1.Text += ": " + qMsg.Message.ToString();
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
        void _securityMaster_SecMstrDataResponse(SecMasterBaseObj secMasterObj)
        {
            try
            {
                lock (_validationLocker)
                {
                    if (_dictRowsToValidate.ContainsKey(secMasterObj.TickerSymbol))
                    {
                        foreach (DataRow dr in _dictRowsToValidate[secMasterObj.TickerSymbol])
                        {
                            dr.SetColumnError("Symbol", null);
                            dr["CompanyName"] = secMasterObj.LongName;
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
                         row.SetColumnError("Symbol", "Symbol Not Validated !");
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

        private void grdCashDividends_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                if (e.Cell.Column.Header.Caption == "Symbol")
                {
                    DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                    if (String.IsNullOrEmpty(e.Cell.Text))
                    {
                        row.SetColumnError("Symbol", "Symbol can't be null!");
                    }
                    row["CompanyName"] = string.Empty;
                    string enteredSymbol = e.Cell.Row.Cells["Symbol"].Value.ToString();
                    ValidateSymbol(enteredSymbol, row);
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
    }
}