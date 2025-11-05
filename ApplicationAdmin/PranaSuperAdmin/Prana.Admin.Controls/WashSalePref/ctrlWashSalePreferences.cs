using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.Controls.WashSalePref
{
    public partial class ctrlWashSalePreferences : UserControl
    {

        private Dictionary<int, DateTime> _dictWashSalePreferences;
        private Dictionary<int, DateTime> _dictBackUpWashSalePreferences;
        Dictionary<int, CashPreferences> _dictCashPreferences;

        private int _currAccount = 0;

        public ctrlWashSalePreferences()
        {
            InitializeComponent();
            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                SetControl();
            }
        }

        public void SetControl()
        {
            try
            {
                SetAccountMultiComboBox();
                SetAccountComboBox();
                GetWashSalePreferencesFromDB();
                chkBoxMultiple.Checked = false;
                _dictCashPreferences = CompanyManager.GetCashPreferencesFromDB();
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
        /// Save the changes in the db
        /// </summary>
        /// <returns></returns>
        public void SavePreferences()
        {
            try
            {
                if (cmbMultiAccounts.Visible)
                {
                    ApplyChanges();
                }
                else if (cmbAccounts.Visible)
                {
                    cmbAccounts_ValueChanged(null, null);
                }
                if (!ConfirmChanges())
                {
                    return;
                }
                if (_dictWashSalePreferences != null)
                {
                    object[] parameter = { CreateXMLPreferences() };

                    // Fetch the accounts that have trade dates on or before WashSaleStartDate
                    DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetAccountIDBeforeWashSaleDate", parameter);
                    String strDelete = string.Empty;
                    String strMsg = string.Empty;
                    string strAccount = "account";
                    bool flag = true;

                    if (ds.Tables[0].Rows.Count > 1)
                        strAccount = "accounts";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        strDelete += dr["AccountName"].ToString() + "   " + Convert.ToDateTime(dr["colWashSaleStartDate"]).ToShortDateString() + Environment.NewLine;
                    }
                    if (!string.IsNullOrEmpty(strDelete))
                    {
                        strMsg += "For the following " + strAccount + " the Wash Sale Transactions Records on or before the mentioned date will be deleted.\n\n" + strDelete;
                    }
                    if (!string.IsNullOrEmpty(strMsg))
                    {
                        if (MessageBox.Show(strMsg + "\nDo you want to continue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        // Save the selected WashSaleStartDate in DB 
                        int i = (int)DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveWashSalePreferences", parameter);
                        if (i > 0)
                        {
                            MessageBox.Show("Start Date changed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Logger.LoggerWrite("Wash Sale start date changed by UserId: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + " UserName: " + CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, true);
                            SetControl();
                        }
                        else if (i <= -1)
                        {
                            MessageBox.Show("Problem occurred while saving the preferences.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Create the XML of the records for Saving into DB
        /// </summary>
        /// <returns>XML document of Preferences</returns>
        private String CreateXMLPreferences()
        {
            String xmlPreference = string.Empty;
            try
            {
                DataTable dtPref = new DataTable("DtPref");
                dtPref.Columns.Add("AccountID", typeof(int));
                dtPref.Columns.Add("WashSaleStartDate", typeof(string));
                DataSet dsPref = new DataSet("DsPref");
                foreach (int accountID in _dictWashSalePreferences.Keys)
                {
                    if (!_dictBackUpWashSalePreferences.ContainsKey(accountID) || _dictWashSalePreferences[accountID].Date != _dictBackUpWashSalePreferences[accountID].Date)
                    {
                        dtPref.Rows.Add(accountID, _dictWashSalePreferences[accountID]);
                    }
                }
                dsPref.Tables.Add(dtPref);
                xmlPreference = dsPref.GetXml();
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
            return xmlPreference;
        }

        /// <summary>
        /// Apply the changes for the selected accounts
        /// </summary>
        private void ApplyChanges()
        {
            try
            {
                if (cmbMultiAccounts.GetNoOfCheckedItems() > 0)
                {
                    foreach (KeyValuePair<int, string> kvPair in cmbMultiAccounts.GetSelectedItemsInDictionary())
                    {
                        if (!_dictWashSalePreferences.ContainsKey(kvPair.Key))
                        {
                            _dictWashSalePreferences.Add(kvPair.Key, uDtWashSaleStartDate.DateTime);
                        }
                        else
                        {
                            _dictWashSalePreferences[kvPair.Key] = uDtWashSaleStartDate.DateTime;
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
        /// Return false if wash sale date is prior to the cash management date
        /// </summary>
        private bool ConfirmChanges()
        {
            try
            {
                foreach (int accountID in _dictWashSalePreferences.Keys)
                {
                    if (_dictBackUpWashSalePreferences.ContainsKey(accountID) && _dictCashPreferences.ContainsKey(accountID)
                        && _dictWashSalePreferences[accountID].Date != _dictBackUpWashSalePreferences[accountID].Date
                        && _dictWashSalePreferences[accountID].Date < _dictCashPreferences[accountID].CashMgmtStartDate)
                    {
                        return false;
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
            return true;
        }

        private void SetAccountMultiComboBox()
        {
            try
            {
                Dictionary<int, string> dictAccounts = new Dictionary<int, string>();

                if (cmbMultiAccounts.GetNoOfTotalItems() <= 0)
                {
                    dictAccounts = CachedDataManager.GetInstance.GetAccounts();
                    cmbMultiAccounts.SetManualTheme(false);
                    //add Assets to the check list default value will be unchecked
                    cmbMultiAccounts.AddItemsToTheCheckList(dictAccounts, CheckState.Unchecked);
                    //adjust checklistbox width according to the longest Asset Name
                    cmbMultiAccounts.AdjustCheckListBoxWidth();
                    cmbMultiAccounts.TitleText = "Account";
                    cmbMultiAccounts.SetTitleText(0);
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

        private void SetAccountComboBox()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();

                foreach (KeyValuePair<int, string> kvp in CachedDataManager.GetInstance.GetAccounts())
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
                    listValues.Add(value);
                }
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                cmbAccounts.DataSource = null;
                cmbAccounts.DataSource = listValues;
                cmbAccounts.DisplayMember = "DisplayText";
                cmbAccounts.ValueMember = "Value";
                cmbAccounts.DataBind();
                cmbAccounts.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbAccounts.Value = -1;
                cmbAccounts.DisplayLayout.Bands[0].ColHeadersVisible = false;
                cmbAccounts.DisplayLayout.Bands[0].Columns[1].Width = cmbAccounts.DisplayLayout.Bands[0].Columns[1].CalculateAutoResizeWidth(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand, true);
                cmbAccounts.DisplayLayout.Bands[0].Columns[0].Width = cmbAccounts.DisplayLayout.Bands[0].Columns[0].CalculateAutoResizeWidth(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand, true);
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

        private void chkBoxMultiple_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBoxMultiple.Checked)
                {
                    cmbMultiAccounts.Visible = true;
                    cmbAccounts.Visible = false;
                    cmbMultiAccounts.SelectUnselectAll(CheckState.Unchecked);
                }
                else
                {
                    cmbMultiAccounts.Visible = false;
                    cmbAccounts.Visible = true;
                    cmbAccounts.Location = cmbMultiAccounts.Location;
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
        private void ctrlWashSalePreferences_Load(object sender, EventArgs e)
        {
            if (CustomThemeHelper.ApplyTheme)
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
        }

        internal void GetWashSalePreferencesFromDB()
        {
            try
            {
                _dictWashSalePreferences = new Dictionary<int, DateTime>();
                _dictBackUpWashSalePreferences = new Dictionary<int, DateTime>();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllWashSalePreferences";

                DataSet dsWashSale = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (dsWashSale != null && dsWashSale.Tables.Count > 0 && dsWashSale.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsWashSale.Tables[0].Rows)
                    {
                        int accountId;
                        DateTime washSaleStartDate;
                        if (!string.IsNullOrEmpty(dr["FundID"].ToString()) && !string.IsNullOrEmpty(dr["WashSaleStartDate"].ToString()))
                        {
                            accountId = Convert.ToInt32(dr["FundID"]);
                            washSaleStartDate = Convert.ToDateTime(dr["WashSaleStartDate"]);
                            if (!_dictWashSalePreferences.ContainsKey(accountId))
                            {
                                _dictWashSalePreferences.Add(accountId, washSaleStartDate);
                            }
                            if (!_dictBackUpWashSalePreferences.ContainsKey(accountId))
                            {
                                _dictBackUpWashSalePreferences.Add(accountId, washSaleStartDate);
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
        /// Save the preferences of the previous account and load the preferences of the new selected account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccounts_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_currAccount > 0)
                {
                    if (uDtWashSaleStartDate.Value != null)
                    {
                        if (_dictWashSalePreferences.ContainsKey(_currAccount))
                        {
                            _dictWashSalePreferences[_currAccount] = uDtWashSaleStartDate.DateTime;
                        }
                        else
                        {
                            _dictWashSalePreferences.Add(_currAccount, uDtWashSaleStartDate.DateTime);
                        }
                    }
                }
                if (!cmbAccounts.Text.Equals("-Select-"))
                {
                    _currAccount = Convert.ToInt32(cmbAccounts.Value);
                    if (_dictWashSalePreferences.ContainsKey(_currAccount))
                    {
                        uDtWashSaleStartDate.DateTime = _dictWashSalePreferences[_currAccount];
                        return;
                    }
                }
                uDtWashSaleStartDate.DateTime = DateTime.Now.Date;
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
    }
}



