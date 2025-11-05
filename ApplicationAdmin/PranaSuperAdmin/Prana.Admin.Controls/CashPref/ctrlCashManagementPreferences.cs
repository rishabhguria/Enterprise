using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CashManagement;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    public partial class ctrlCashManagementPreferences : UserControl
    {
        /// <summary>
        /// From date
        /// </summary>
        private Dictionary<int, Tuple<DateTime, DateTime>> dictcashPref = new Dictionary<int, Tuple<DateTime, DateTime>>();
        private static Int64 id;

        private Dictionary<int, CashPreferences> _dictCashPreferences;
        private Dictionary<int, CashPreferences> _dictBackUpCashPreferences;
        private int _currAccount = 0;
        private string CollateralInterestValue;
        bool _isCollateralMarkPrice;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is collateral mark price validation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is collateral mark price validation; otherwise, <c>false</c>.
        /// </value>
        public bool IsCollateralMarkPrice
        {
            get { return _isCollateralMarkPrice; }
            set { _isCollateralMarkPrice = value; }
        }

        bool _isShowTillSettlementDate;
        public bool IsShowTillSettlmentDate
        {
            get { return _isShowTillSettlementDate; }
            set { _isShowTillSettlementDate = value; }
        }

        public ctrlCashManagementPreferences()
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    SetControl();
                }
                // chkbxBond.Checked = ObjCashPreferences.IsCalculateBondAccurals;
                //chkbxDividend.Checked = ObjCashPreferences.IsCalculateDividend;
                //chkbxPnL.Checked = ObjCashPreferences.IsCalculatePnL;

                //uDtCashManagementStartDate.DateTime = ObjCashPreferences.CashMgmtStartDate.Date;
                //txtMarginPercentage.Text = ObjCashPreferences.MarginPercentage.ToString();
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
        /// Bind the values and get the preferences in the cache
        /// </summary>
        public void SetControl()
        {
            try
            {
                SetAccountMultiComboBox();
                SetAccountComboBox();
                SetCollateralInterestComboBox();
                GetCashPreferences();
                GetCashCommissionPreferencesFromDB();
                chkBoxMultiple.Checked = false;
                CheckBoxCollateralMP.Checked = CachedDataManager.GetInstance.IsCollateralMarkPriceValidation();
                CheckBoxShowTillSettlementDate.Checked = CachedDataManager.GetInstance.IsShowTillSettlementDate();
                revaluationPreferenceControl.InitializeData();
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
        /// Added by: Bharat raturi, 1 jul 2014
        /// purpose: Get the dictionary of the accountID-CashPreference pairs from DB
        /// </summary>
        /// <returns>dictionary of the accountID-CashPreference pairs from DB</returns>
        internal void GetCashPreferences()
        {
            try
            {
                _dictCashPreferences = CompanyManager.GetCashPreferencesFromDB();
                _dictBackUpCashPreferences = new Dictionary<int, CashPreferences>();

                foreach (int accountID in _dictCashPreferences.Keys)
                {
                    CashPreferences objCashPref = _dictCashPreferences[accountID];
                    CashPreferences objBackUpCashPref = new CashPreferences();

                    objBackUpCashPref.AccountID = objCashPref.AccountID;
                    objBackUpCashPref.CashMgmtStartDate = objCashPref.CashMgmtStartDate;
                    objBackUpCashPref.MarginPercentage = objCashPref.MarginPercentage;
                    objBackUpCashPref.IsCalculatePnL = objCashPref.IsCalculatePnL;
                    objBackUpCashPref.IsCalculateDividend = objCashPref.IsCalculateDividend;
                    objBackUpCashPref.IsCalculateBondAccurals = objCashPref.IsCalculateBondAccurals;
                    objBackUpCashPref.IsCalculateCollateral = objCashPref.IsCalculateCollateral;
                    objBackUpCashPref.IsCalculateCollateralFrequencyInterest = objCashPref.IsCalculateCollateralFrequencyInterest;
                    objBackUpCashPref.IsRealizedPL = objCashPref.IsRealizedPL;
                    objBackUpCashPref.IsTotalPL = objCashPref.IsTotalPL;
                    objBackUpCashPref.IsAccruedTillSettlement = objCashPref.IsAccruedTillSettlement;
                    objBackUpCashPref.SymbolWiseRevaluationDate = objCashPref.SymbolWiseRevaluationDate;
                    objBackUpCashPref.IsCreateManualJournals = objCashPref.IsCreateManualJournals;
                    if (!_dictBackUpCashPreferences.ContainsKey(objBackUpCashPref.AccountID))
                    {
                        _dictBackUpCashPreferences.Add(objBackUpCashPref.AccountID, objBackUpCashPref);
                    }
                    RealizedPLchkbx.Checked = _dictCashPreferences[objBackUpCashPref.AccountID].IsRealizedPL ? true : false;
                    TotalPLchkbx.Checked = _dictCashPreferences[objBackUpCashPref.AccountID].IsTotalPL ? true : false;
                    CreateManualJournalschkbx.Checked = _dictCashPreferences[objBackUpCashPref.AccountID].IsCreateManualJournals ? true : false;
                    if (!CachedDataManager.GetInstance.GetCashPreferenceFundsDict().ContainsKey(accountID))
                        CachedDataManager.GetInstance.GetCashPreferenceFundsDict().Add(accountID, new Tuple<DateTime, bool>(objBackUpCashPref.CashMgmtStartDate, objBackUpCashPref.IsCalculateDividend));
                }

                //#region Temporarily Set To True Because of some further Enhancement

                ////ObjCashPreferences.IsCalculateBondAccurals = true;
                ////ObjCashPreferences.IsCalculateDividend = true;
                ////ObjCashPreferences.IsCalculatePnL = true;

                //#endregion
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
        /// Added by: Kashish Goyal, 01 Dec 2017
        /// Purpose: Get the dictionary of the AssetClass-IsChecked pairs from DB
        /// </summary>
        /// <returns>Dictionary of the AssetClass-IsChecked pairs from DB</returns>
        internal void GetCashCommissionPreferencesFromDB()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCashCommissionPreferences";
                DataSet dsCommCash = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (dsCommCash != null && dsCommCash.Tables.Count > 0 && dsCommCash.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCommCash.Tables[0].Rows)
                    {
                        switch (dr["AssetClass"].ToString())
                        {
                            case "Equity":
                                checkBoxEquity.Checked = Convert.ToBoolean(dr["IsChecked"]);
                                break;
                            case "EquityOption":
                                checkBoxEquityOption.Checked = Convert.ToBoolean(dr["IsChecked"]);
                                break;
                            case "Future":
                                checkBoxFuture.Checked = Convert.ToBoolean(dr["IsChecked"]);
                                break;
                            case "FutureOption":
                                checkBoxFutureOption.Checked = Convert.ToBoolean(dr["IsChecked"]);
                                break;
                            case "Fx":
                                checkBoxFX.Checked = Convert.ToBoolean(dr["IsChecked"]);
                                break;
                            case "EquitySwap":
                                checkBoxEquitySwap.Checked = Convert.ToBoolean(dr["IsChecked"]);
                                break;
                            case "FixedIncome":
                                checkBoxFixedIncome.Checked = Convert.ToBoolean(dr["IsChecked"]);
                                break;
                            case "PrivateEquity":
                                checkBoxPrivateEquity.Checked = Convert.ToBoolean(dr["IsChecked"]);
                                break;
                            case "FxForward":
                                checkBoxFxForward.Checked = Convert.ToBoolean(dr["IsChecked"]);
                                break;
                            case "ConvertibleBond":
                                checkBoxCD.Checked = Convert.ToBoolean(dr["IsChecked"]);
                                break;
                            case "CreditDefaultSwap":
                                checkBoxCDS.Checked = Convert.ToBoolean(dr["IsChecked"]);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetCollateralInterestComboBox()
        {
            List<EnumerationValue> listValues = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(CollateralCouponFrequency));
            CollateralInterest.DataSource = null;
            CollateralInterest.DataSource = listValues;
            CollateralInterest.DisplayMember = "DisplayText";
            CollateralInterest.ValueMember = "Value";
            CollateralInterest.Value = -1;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in CollateralInterest.DisplayLayout.Bands[0].Columns)
            {
                if (column.Key.Equals("DisplayText"))
                {
                    column.Hidden = false;
                }
                else
                {
                    column.Hidden = true;
                }
            }
            CollateralInterest.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }


        private void SetAccountMultiComboBox()
        {
            try
            {
                Dictionary<int, string> dictAccounts = new Dictionary<int, string>();
                //if (cmbMultiAccounts.GetNoOfTotalItems() > 0)
                //{
                //    foreach (Control ctrl in cmbMultiAccounts.Controls)
                //    {
                //        if (ctrl is CheckedListBox)
                //        {
                //            CheckedListBox chkCtrl = ctrl as CheckedListBox;
                //            chkCtrl.Items.Clear();
                //        }
                //    }
                //}
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
                    List<EnumerationValue> listValues = new List<EnumerationValue>();
                }
                //if (CachedDataManager.GetInstance.GetAccounts().Count == 0)
                //{
                //    cmbAccounts.SelectUnselectAll(CheckState.Checked);
                //    cmbAccounts.SelectUnselectAll(CheckState.Unchecked);
                //}
                //else
                //    cmbAccounts.SelectUnselectItems(CachedDataManager.GetInstance.GetAccounts(), CheckState.Unchecked);
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
                //cmbAccounts.DisplayLayout.Bands[0].SortedColumns.Add(cmbAccounts.DisplayLayout.Bands[0].Columns[0],false);
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

        #region IPreferences Members

        /// <summary>
        /// Save the changes in the db
        /// </summary>
        /// <returns></returns>
        public void SavePreferences()
        {
            try
            {
                CommonDataCache.CachedDataManager.GetInstance.UpdateandSavePranaPreference(null, null, null, null, null, null, null, null, null, null, null, null, null, _isCollateralMarkPrice, _isShowTillSettlementDate);
                SaveCommissionandFeePreferences();
                revaluationPreferenceControl.SaveRevaluationPreference();

                ValidatePreference();
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
                if (_dictCashPreferences != null)
                {
                    string xmlPreference = CreateXMLPreferences();
                    object[] paramPref = { xmlPreference };
                    object[] paramRevaluationPref = { xmlPreference, 2 };

                    int i = (int)DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCashPreferences", paramPref);
                    if (i > 0)
                    {
                        revaluationPreferenceControl.SaveRevaluationPreferenceToDB(paramRevaluationPref);
                        MessageBox.Show("Cash Management preferences saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SetControl();
                    }
                    else
                    {
                        MessageBox.Show("Problem occurred while saving the preferences.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //return false;
        }

        /// <summary>
        /// Save the commission Preferences changes in the db
        /// </summary>
        /// <returns></returns>
        public void SaveCommissionandFeePreferences()
        {
            try
            {
                string sProc = "P_SaveCashCommissionPreferences";
                string csv = checkBoxEquity.Checked + "," + checkBoxEquityOption.Checked + "," + checkBoxFuture.Checked + "," + checkBoxFutureOption.Checked + "," + checkBoxFX.Checked + "," + checkBoxEquitySwap.Checked + "," +
                             checkBoxFixedIncome.Checked + "," + checkBoxPrivateEquity.Checked + "," + checkBoxFxForward.Checked + "," + checkBoxCD.Checked + "," + checkBoxCDS.Checked;
                int i = (int)DatabaseManager.DatabaseManager.ExecuteNonQuery(sProc, new object[] { csv });
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
        /// Check if there is any change in the preferences
        /// </summary>
        /// <returns>true, if changed</returns>
        private bool IsPreferenceChanged()
        {
            try
            {
                if (_dictCashPreferences.Keys.Count != _dictBackUpCashPreferences.Keys.Count)
                {
                    return true;
                }
                else
                {
                    foreach (int accountID in _dictCashPreferences.Keys)
                    {
                        if (!_dictBackUpCashPreferences.ContainsKey(accountID))
                        {
                            return true;
                        }
                        if (!_dictBackUpCashPreferences[accountID].CashMgmtStartDate.Date.Equals(_dictCashPreferences[accountID].CashMgmtStartDate.Date)
                            || !_dictBackUpCashPreferences[accountID].IsCalculateCollateral.Equals(_dictCashPreferences[accountID].IsCalculateCollateral)
                            || !_dictBackUpCashPreferences[accountID].IsCalculateCollateralFrequencyInterest.Equals(_dictCashPreferences[accountID].IsCalculateCollateralFrequencyInterest)
                            || !_dictBackUpCashPreferences[accountID].IsCalculateBondAccurals.Equals(_dictCashPreferences[accountID].IsCalculateBondAccurals)
                            || !_dictBackUpCashPreferences[accountID].MarginPercentage.Equals(_dictCashPreferences[accountID].MarginPercentage)
                            || !_dictBackUpCashPreferences[accountID].IsCalculatePnL.Equals(_dictCashPreferences[accountID].IsCalculatePnL)
                            || !_dictBackUpCashPreferences[accountID].IsCalculateDividend.Equals(_dictCashPreferences[accountID].IsCalculateDividend)
                            || !_dictBackUpCashPreferences[accountID].IsRealizedPL.Equals(_dictCashPreferences[accountID].IsRealizedPL)
                            || !_dictBackUpCashPreferences[accountID].IsTotalPL.Equals(_dictCashPreferences[accountID].IsTotalPL)
                            || !_dictBackUpCashPreferences[accountID].IsCashSettlementEntriesVisible.Equals(_dictCashPreferences[accountID].IsCashSettlementEntriesVisible)
                            || !_dictBackUpCashPreferences[accountID].IsAccruedTillSettlement.Equals(_dictCashPreferences[accountID].IsAccruedTillSettlement)
                            || !_dictBackUpCashPreferences[accountID].IsCreateManualJournals.Equals(_dictCashPreferences[accountID].IsCreateManualJournals)

                            )
                        {
                            return true;
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
            return false;
        }

        /// <summary>
        /// Inform the user about the changes
        /// </summary>
        private bool ConfirmChanges()
        {
            try
            {
                bool isPrefChecked = CheckBoxPref.Checked;
                String strDelete = string.Empty;
                String strAppend = string.Empty;
                String strMsg = string.Empty;
                String strAccr = string.Empty;
                foreach (int accountID in _dictCashPreferences.Keys)
                {
                    if (_dictBackUpCashPreferences.ContainsKey(accountID) && isPrefChecked)
                    {
                        if (_dictBackUpCashPreferences[accountID].CashMgmtStartDate < _dictCashPreferences[accountID].CashMgmtStartDate)
                        {
                            strDelete += CachedDataManager.GetInstance.GetAccountText(accountID) + "    " + _dictCashPreferences[accountID].CashMgmtStartDate.ToShortDateString() + Environment.NewLine;
                        }
                        else if (_dictBackUpCashPreferences[accountID].CashMgmtStartDate > _dictCashPreferences[accountID].CashMgmtStartDate)
                        {
                            strAppend += CachedDataManager.GetInstance.GetAccountText(accountID) + "    " + _dictCashPreferences[accountID].CashMgmtStartDate.AddDays(1).ToShortDateString() + "    " + _dictBackUpCashPreferences[accountID].CashMgmtStartDate.ToShortDateString() + Environment.NewLine;
                        }
                    }
                    else if (!_dictBackUpCashPreferences.ContainsKey(accountID) && !isPrefChecked)
                    {
                        strAccr += CachedDataManager.GetInstance.GetAccountText(accountID) + Environment.NewLine;
                    }
                }
                if (!string.IsNullOrEmpty(strAccr))
                {
                    MessageBox.Show("Save Cash Management Start Date for following funds before proceeding.\n\n" + strAccr);
                    return false;
                }
                if (!string.IsNullOrEmpty(strDelete))
                {
                    strMsg += "For the following accounts transactions records on or before the mentioned date will be deleted.\n[Note: Opening Balances of the mentioned date will remain intact.]" + Environment.NewLine + strDelete;
                }
                if (!string.IsNullOrEmpty(strAppend))
                {
                    strMsg += "For the following accounts transactions records for mentioned time period have to be create manually." + Environment.NewLine + strAppend;
                }
                if (!string.IsNullOrEmpty(strMsg))
                {
                    if (MessageBox.Show(strMsg + "\n\nDo you want to continue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
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
            return false;
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
                dtPref.Columns.Add("CashMgmtStartDate", typeof(string));
                dtPref.Columns.Add("MarginPercentage", typeof(string));
                dtPref.Columns.Add("CalculatedCollateralInterestFrequency", typeof(string));
                dtPref.Columns.Add("CalculatedPnL", typeof(bool));
                dtPref.Columns.Add("CalculatedDividend", typeof(bool));
                dtPref.Columns.Add("CalculatedBondAccrual", typeof(bool));
                dtPref.Columns.Add("CalculatedCollateral", typeof(bool));
                dtPref.Columns.Add("CalculatedIsBreakRealizedPnlSubaccount", typeof(bool));
                dtPref.Columns.Add("CalculatedIsBreakTotalIntoTradingandfxPnl", typeof(bool));
                dtPref.Columns.Add("isStartDateAdvanced", typeof(bool));
                dtPref.Columns.Add("IsCashSettlementEntriesVisible", typeof(bool));
                dtPref.Columns.Add("IsAccruedTillSettlement", typeof(bool));
                dtPref.Columns.Add("SymbolWiseRevaluationDate", typeof(string));
                dtPref.Columns.Add("IsCreateManualJournals", typeof(string));

                DataSet dsPref = new DataSet("DsPref");
                bool isPrefEnabled = CheckBoxPref.Checked;
                bool isAccrDateEnabled = CheckBoxAccrualDate.Checked;

                Dictionary<int, CashPreferences> cpDictionary = isPrefEnabled ? _dictCashPreferences : _dictBackUpCashPreferences;
                Dictionary<int, CashPreferences> accrDictionary = isAccrDateEnabled ? _dictCashPreferences : _dictBackUpCashPreferences;
                foreach (int accountID in cpDictionary.Keys)
                {
                    DateTime dt = DateTime.MinValue;
                    if (accrDictionary.ContainsKey(accountID))
                        dt = accrDictionary[accountID].SymbolWiseRevaluationDate;
                    bool isStartDateAhead = false;
                    if (_dictBackUpCashPreferences.ContainsKey(accountID))
                    {
                        if (_dictBackUpCashPreferences[accountID].CashMgmtStartDate < cpDictionary[accountID].CashMgmtStartDate)
                            isStartDateAhead = true;
                    }
                    // CHMW-3141
                    dtPref.Rows.Add(accountID,
                        cpDictionary[accountID].CashMgmtStartDate,
                        cpDictionary[accountID].MarginPercentage,
                        cpDictionary[accountID].IsCalculateCollateralFrequencyInterest,
                        cpDictionary[accountID].IsCalculatePnL,
                        cpDictionary[accountID].IsCalculateDividend,
                        cpDictionary[accountID].IsCalculateBondAccurals,
                        cpDictionary[accountID].IsCalculateCollateral,
                        RealizedPLchkbx.Checked ? true : false,
                        TotalPLchkbx.Checked ? true : false,
                        isStartDateAhead,
                        cpDictionary[accountID].IsCashSettlementEntriesVisible,
                        cpDictionary[accountID].IsAccruedTillSettlement,
                        dt.Equals(DateTime.MinValue) ? (object)DBNull.Value : (object)dt,
                        CreateManualJournalschkbx.Checked ? true : false);

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
                if (cmbMultiAccounts.Visible && cmbMultiAccounts.GetNoOfCheckedItems() > 0)// && !string.IsNullOrEmpty(txtMarginPercentage.Text.Trim()))
                {
                    //if (_dictCashPreferences.Count > 0)
                    //{
                    //    _dictCashPreferences.Clear();
                    //}

                    foreach (KeyValuePair<int, string> kvPair in cmbMultiAccounts.GetSelectedItemsInDictionary())
                    {
                        CashPreferences objCashPreferences = new CashPreferences();
                        objCashPreferences.CashMgmtStartDate = uDtCashManagementStartDate.DateTime;
                        objCashPreferences.SymbolWiseRevaluationDate = udtFXRevaluationSymbolWiseDate.DateTime;
                        objCashPreferences.MarginPercentage = GetMarginPercentage();
                        objCashPreferences.IsCalculateCollateralFrequencyInterest = CollateralInterestValue;
                        objCashPreferences.IsCalculateCollateral = chkbxCollateral.Checked;
                        objCashPreferences.IsCalculateBondAccurals = chkbxBond.Checked;
                        objCashPreferences.IsCalculateDividend = chkbxDividend.Checked;
                        objCashPreferences.IsCalculatePnL = chkbxPnL.Checked;
                        objCashPreferences.IsRealizedPL = RealizedPLchkbx.Checked;
                        objCashPreferences.IsTotalPL = TotalPLchkbx.Checked;
                        objCashPreferences.IsCashSettlementEntriesVisible = chkbxSettlementEntries.Checked;
                        objCashPreferences.IsAccruedTillSettlement = chkbxAccruedInterest.Checked;
                        objCashPreferences.IsCreateManualJournals = CreateManualJournalschkbx.Checked;

                        if (!_dictCashPreferences.ContainsKey(kvPair.Key))
                        {
                            _dictCashPreferences.Add(kvPair.Key, objCashPreferences);
                        }
                        else
                        {
                            _dictCashPreferences[kvPair.Key] = objCashPreferences;
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
        /// Get the margin percentage
        /// </summary>
        /// <returns></returns>
        private double GetMarginPercentage()
        {
            double marginpercent = 0.0;
            try
            {
                if (!String.IsNullOrEmpty(txtMarginPercentage.Text.Trim()) && double.TryParse(txtMarginPercentage.Text.Trim(), out marginpercent))
                {
                    return double.Parse(txtMarginPercentage.Text);
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
            return marginpercent;
        }

        //public void RestoreDefault()
        //{
        //    //if (ObjCashPreferences != null)
        //    //{
        //    //    uDtCashManagementStartDate.DateTime = ObjCashPreferences.CashMgmtStartDate;
        //    //    txtMarginPercentage.Text = ObjCashPreferences.MarginPercentage.ToString();
        //    //}
        //}

        //public IPreferenceData GetPrefs()
        //{
        //    //CashManagementPreferences Preferences = new CashManagementPreferences();
        //    //return Preferences;
        //}

        //public event EventHandler SaveClicked;

        //private string _modulename = string.Empty;
        //public string SetModuleActive
        //{
        //    set
        //    {
        //        _modulename = value;

        //    }
        //}



        #endregion



        private void chkbx_MainOption_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbx_MainOption.CheckState == CheckState.Checked)
            {
                chkbxBond.Checked = true;
                chkbxDividend.Checked = true;
                chkbxPnL.Checked = true;
                chkbxCollateral.Checked = true;
            }
            else if (chkbx_MainOption.CheckState == CheckState.Unchecked)
            {
                chkbxBond.Checked = false;
                chkbxDividend.Checked = false;
                chkbxPnL.Checked = false;
                chkbxCollateral.Checked = false;
            }
        }

        #region uncommented by Ishan Gandhi, 29th October 2014
        private void ChangeStatus_chkbxMainOption()
        {
            if (chkbxPnL.Checked == false && chkbxDividend.Checked == false && chkbxBond.Checked == false && chkbxCollateral.Checked == false)
                chkbx_MainOption.Checked = false;
            else if (chkbxPnL.Checked == true && chkbxDividend.Checked == true && chkbxBond.Checked == true && chkbxCollateral.Checked == true)
                chkbx_MainOption.CheckState = CheckState.Checked;
            else
                chkbx_MainOption.CheckState = CheckState.Indeterminate;
        }

        private void chkbxPnL_CheckedChanged(object sender, EventArgs e)
        {
            ChangeStatus_chkbxMainOption();
        }

        private void chkbxDividend_CheckedChanged(object sender, EventArgs e)
        {
            ChangeStatus_chkbxMainOption();
        }

        private void chkbxBond_CheckedChanged(object sender, EventArgs e)
        {
            ChangeStatus_chkbxMainOption();
        }
        private void chkbxCollateral_CheckedChanged(object sender, EventArgs e)
        {
            ChangeStatus_chkbxMainOption();
        }
        #endregion

        private void chkBoxMultiple_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBoxMultiple.Checked)
                {
                    cmbMultiAccounts.Visible = true;
                    cmbAccounts.Visible = false;
                    cmbMultiAccounts.SelectUnselectAll(CheckState.Checked);
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
                    if (ValidatePreference())
                    {
                        if (_dictCashPreferences.ContainsKey(_currAccount))
                        {
                            CashPreferences objCashPreferences = _dictCashPreferences[_currAccount];
                            objCashPreferences.CashMgmtStartDate = uDtCashManagementStartDate.DateTime;
                            objCashPreferences.SymbolWiseRevaluationDate = udtFXRevaluationSymbolWiseDate.DateTime;
                            objCashPreferences.MarginPercentage = GetMarginPercentage(); //double.Parse(txtMarginPercentage.Text);
                            objCashPreferences.IsCalculateCollateralFrequencyInterest = CollateralInterestValue;
                            objCashPreferences.IsCalculateCollateral = chkbxCollateral.Checked;
                            objCashPreferences.IsCalculateBondAccurals = chkbxBond.Checked;
                            objCashPreferences.IsCalculateDividend = chkbxDividend.Checked;
                            objCashPreferences.IsCalculatePnL = chkbxPnL.Checked;
                            objCashPreferences.IsCashSettlementEntriesVisible = chkbxSettlementEntries.Checked;
                            objCashPreferences.IsAccruedTillSettlement = chkbxAccruedInterest.Checked;
                        }
                        else
                        {
                            CashPreferences objCashPreferences = new CashPreferences();
                            objCashPreferences.CashMgmtStartDate = uDtCashManagementStartDate.DateTime;
                            objCashPreferences.SymbolWiseRevaluationDate = udtFXRevaluationSymbolWiseDate.DateTime;
                            objCashPreferences.MarginPercentage = GetMarginPercentage(); //double.Parse(txtMarginPercentage.Text);
                            objCashPreferences.IsCalculateCollateralFrequencyInterest = CollateralInterestValue;
                            objCashPreferences.IsCalculateCollateral = chkbxCollateral.Checked;
                            objCashPreferences.IsCalculateBondAccurals = chkbxBond.Checked;
                            objCashPreferences.IsCalculateDividend = chkbxDividend.Checked;
                            objCashPreferences.IsCalculatePnL = chkbxPnL.Checked;
                            objCashPreferences.IsCashSettlementEntriesVisible = chkbxSettlementEntries.Checked;
                            objCashPreferences.IsAccruedTillSettlement = chkbxAccruedInterest.Checked;
                            _dictCashPreferences.Add(_currAccount, objCashPreferences);
                        }
                    }
                }
                if (!cmbAccounts.Text.Equals("-Select-"))// && uDtCashManagementStartDate.Value != null && !string.IsNullOrWhiteSpace(txtMarginPercentage.Text.Trim()))
                {
                    //int accountID = Convert.ToInt32(cmbAccounts.Value);
                    _currAccount = Convert.ToInt32(cmbAccounts.Value);
                    if (_dictCashPreferences.ContainsKey(_currAccount))
                    {
                        CashPreferences objCashPreferences = _dictCashPreferences[_currAccount];
                        uDtCashManagementStartDate.DateTime = objCashPreferences.CashMgmtStartDate;
                        if (objCashPreferences.SymbolWiseRevaluationDate != DateTime.MinValue)
                            udtFXRevaluationSymbolWiseDate.DateTime = objCashPreferences.SymbolWiseRevaluationDate;
                        else
                            udtFXRevaluationSymbolWiseDate.DateTime = DateTime.Now.Date;
                        txtMarginPercentage.Text = objCashPreferences.MarginPercentage.ToString();
                        CollateralInterest.Value = objCashPreferences.IsCalculateCollateralFrequencyInterest;
                        chkbxCollateral.Checked = objCashPreferences.IsCalculateCollateral;
                        chkbxBond.Checked = objCashPreferences.IsCalculateBondAccurals;
                        chkbxDividend.Checked = objCashPreferences.IsCalculateDividend;
                        chkbxPnL.Checked = objCashPreferences.IsCalculatePnL;
                        chkbxSettlementEntries.Checked = objCashPreferences.IsCashSettlementEntriesVisible;
                        chkbxAccruedInterest.Checked = objCashPreferences.IsAccruedTillSettlement;
                        return;
                    }
                }
                //else
                //{
                ResetInputs();
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
            //}
        }

        /// <summary>
        /// Reset the control
        /// </summary>
        private void ResetInputs()
        {
            try
            {
                uDtCashManagementStartDate.DateTime = DateTime.Now.Date;
                udtFXRevaluationSymbolWiseDate.DateTime = DateTime.Now.Date;
                txtMarginPercentage.Text = "0.0";
                chkbxCollateral.Checked = true;
                chkbxBond.Checked = true;
                chkbxDividend.Checked = true;
                chkbxPnL.Checked = true;
                ////CHMW-3141
                chkbxSettlementEntries.Checked = true;
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
        /// Check if the valid values are entered
        /// </summary>
        private bool ValidatePreference()
        {
            double marginPercent = 0.0;

            try
            {
                if (uDtCashManagementStartDate.Value == null)
                {
                    return false;
                    //errorProvider1.SetError(uDtCashManagementStartDate, "Enter a valid date");
                }
                else if (string.IsNullOrWhiteSpace(txtMarginPercentage.Text.Trim()))
                {
                    return false;
                    //errorProvider1.SetError(txtMarginPercentage, "Enter a valid margin percentage");
                }
                else if (!string.IsNullOrWhiteSpace(txtMarginPercentage.Text.Trim()) && !double.TryParse(txtMarginPercentage.Text.Trim(), out marginPercent))
                {
                    //errorProvider1.SetError(txtMarginPercentage, "Only numeric values are valid");
                    return false;
                }
                if (udtFXRevaluationSymbolWiseDate.Value == null)
                {
                    return false;
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

        private void ctrlCashManagementPreferences_Load(object sender, EventArgs e)
        {
            if (CustomThemeHelper.ApplyTheme)
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
        }

        /// <summary>
        /// Handles the Click event of the ubtnRunUtility control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ubtnRunUtility_Click(object sender, EventArgs e)
        {
            try
            {
                string fundIds = string.Empty;
                if (chkBoxMultiple.Checked)
                {
                    foreach (KeyValuePair<int, string> item in cmbMultiAccounts.GetSelectedItemsInDictionary())
                    {
                        fundIds += item.Key + ",";
                    }
                }
                else
                {
                    fundIds += Convert.ToInt32(cmbAccounts.Value);
                }
                if (ValidateAndDisplay(ref fundIds))
                {
                    LaunchAndRunUtility(fundIds);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Utility Failed. Please Contact Administrator.");
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Runs the utility.
        /// </summary>
        /// <param name="utilityForm">The utility form.</param>
        /// <param name="fundIds">The fund ids.</param>
        /// <returns></returns>
        private bool RunUtility(SymbolLevelAccrualUtility utilityForm, string fundIds)
        {
            try
            {
                InitialiseAccrualUtility();
                utilityForm.DispMessage("Utility Initialized");
                string[] fundArr = fundIds.Split(',');
                foreach (int fundID in dictcashPref.Keys)
                {
                    string FundId = fundID.ToString();
                    string FundName = string.Empty;
                    FundName = CachedDataManager.GetInstance.GetAccountText(fundID);
                    if (fundArr.Contains(FundId))
                    {
                        utilityForm.DispMessage("Entries generation Started for Fund: " + FundName);
                        CleanUpFundfromSymbolLevel(FundId);
                        Object[] parameters = { dictcashPref[Convert.ToInt32(FundId)].Item1, dictcashPref[Convert.ToInt32(FundId)].Item2, FundId };

                        QueryData queryData = new QueryData();
                        queryData.StoredProcedureName = "P_JournalEntriesCreationByUtility";
                        queryData.DictionaryDatabaseParameter.Add("@StartDate", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@StartDate",
                            ParameterType = DbType.DateTime,
                            ParameterValue = parameters[0]
                        });
                        queryData.DictionaryDatabaseParameter.Add("@EndDate", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@EndDate",
                            ParameterType = DbType.DateTime,
                            ParameterValue = parameters[1]
                        });
                        queryData.DictionaryDatabaseParameter.Add("@FundId", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@FundId",
                            ParameterType = DbType.String,
                            ParameterValue = parameters[2]
                        });

                        queryData.CommandTimeout = 172800;
                        using (DataSet activitiesDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData))
                        {
                            if (activitiesDataSet == null || activitiesDataSet.Tables.Count == 0)
                            {
                                utilityForm.DispMessage("No Data found in Fund: " + FundName);
                                continue;
                            }
                            FormatActivitiesDataSet(activitiesDataSet, ref id);
                            DataTable tblNewActivity = activitiesDataSet.Tables[0];
                            List<CashActivity> lsCashActivity = tblNewActivity.AsEnumerable().Select
                            (dr => new CashActivity
                            {
                                ActivityTypeId = Convert.ToInt32(dr["ActivityTypeId_FK"]),
                                AccountID = Convert.ToInt32(dr["FundID"]),
                                TransactionSource = (CashTransactionType)(Convert.ToInt32(dr["TransactionSource"])),
                                ActivitySource = (ActivitySource)(Convert.ToInt32(dr["ActivitySource"])),
                                Symbol = Convert.ToString(dr["Symbol"]),
                                Amount = Convert.ToDecimal(dr["Amount"]),
                                CurrencyID = Convert.ToInt32(dr["CurrencyID"]),
                                Description = Convert.ToString(dr["Description"]),
                                SideMultiplier = Convert.ToInt32(dr["SideMultiplier"]),
                                Date = Convert.ToDateTime(dr["TradeDate"]),
                                FXRate = dr["FxRate"] != DBNull.Value ? Convert.ToDouble(dr["FxRate"]) : 0,
                                FXConversionMethodOperator = dr["FXConversionMethodOperator"] != DBNull.Value ? Convert.ToString(dr["FXConversionMethodOperator"]) : string.Empty,
                                FKID = dr["FKID"] != DBNull.Value ? Convert.ToString(dr["FKID"]) : string.Empty,
                                ActivityId = Convert.ToString(dr["ActivityID"]),
                                UniqueKey = Convert.ToString(dr["UniqueKey"]),
                                ActivityNumber = Convert.ToInt32(dr["ActivityNumber"]),
                                ActivityType = Convert.ToString(dr["ActivityType"]),
                                ActivityState = Convert.ToString(dr["ActivityState"]).Equals("New") ? ApplicationConstants.TaxLotState.New : ApplicationConstants.TaxLotState.Deleted,
                                SubAccountID = (dr["SubAccountID"] != DBNull.Value) ? Convert.ToInt32(dr["SubAccountID"]) : 0,
                                UserId = Convert.ToInt32(dr["UserId"])//PRANA-9776
                            }
                            ).ToList();

                            List<Transaction> transactionList = CashHelperClass.CreateJournalEntry(lsCashActivity);
                            if (transactionList != null && transactionList.Count > 0)
                            {
                                List<Transaction> tradeTranList = transactionList.Where(((transaction => (transaction.GetTransactionSource() != (int)CashTransactionType.ManualJournalEntry) && (transaction.GetTransactionSource() != (int)CashTransactionType.OpeningBalance) && (transaction.GetTransactionSource() != (int)CashTransactionType.ImportedEditableData)))).ToList();

                                if (tradeTranList != null && tradeTranList.Count != 0)
                                {
                                    SaveOrUpdateSymbolLevelAccrual(tradeTranList);   //this one is for All journals other than Manual Journal
                                }
                            }

                            QueryData _queryData = new QueryData();
                            _queryData.StoredProcedureName = "P_CalculateAndSaveBalanceForUtility";
                            _queryData.DictionaryDatabaseParameter.Add("@StartDate", new DatabaseParameter()
                            {
                                IsOutParameter = false,
                                ParameterName = "@StartDate",
                                ParameterType = DbType.DateTime,
                                ParameterValue = parameters[0]
                            });
                            _queryData.DictionaryDatabaseParameter.Add("@EndDate", new DatabaseParameter()
                            {
                                IsOutParameter = false,
                                ParameterName = "@EndDate",
                                ParameterType = DbType.DateTime,
                                ParameterValue = parameters[1]
                            });
                            _queryData.DictionaryDatabaseParameter.Add("@FundId", new DatabaseParameter()
                            {
                                IsOutParameter = false,
                                ParameterName = "@FundId",
                                ParameterType = DbType.String,
                                ParameterValue = parameters[2]
                            });

                            _queryData.CommandTimeout = 172800;
                            DatabaseManager.DatabaseManager.ExecuteNonQuery(_queryData);
                        }
                        CleanupOldEntries(FundId);
                        utilityForm.DispMessage("Entries generation completed for Fund: " + FundName + " .");
                    }
                }
                return true;
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
            return false;
        }

        /// <summary>
        /// Launches the and run utility.
        /// </summary>
        /// <param name="utilityForm">The utility form.</param>
        /// <param name="fundIds">The fund ids.</param>
        /// <returns></returns>
        private async void LaunchAndRunUtility(string fundIds)
        {
            try
            {
                SymbolLevelAccrualUtility utilityForm = new SymbolLevelAccrualUtility();
                IntPtr handle = utilityForm.Handle;
                //utilityForm.TopLevel = false;
                //utilityForm.Parent = this;
                utilityForm.StartPosition = FormStartPosition.CenterParent;
                utilityForm.BeginInvoke(new System.Action(() => utilityForm.ShowDialog(this)));
                bool a = await System.Threading.Tasks.Task.Run(() => { return RunUtility(utilityForm, fundIds); });
                utilityForm.BeginInvoke(new System.Action(() => utilityForm.EnableFinish()));
                if (a)
                    utilityForm.BeginInvoke(new System.Action(() => utilityForm.DispMessage("Utility Completed Successfully.")));
                else
                    utilityForm.BeginInvoke(new System.Action(() => utilityForm.DispMessage("Utility Failed. Please Contact Administrator.")));
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
        /// Validates the and display.
        /// </summary>
        private bool ValidateAndDisplay(ref string fundIDs)
        {
            bool isSuccess = false;
            try
            {
                if (string.IsNullOrWhiteSpace(fundIDs) || fundIDs.Equals("-1"))
                {
                    MessageBox.Show("Please select any fund to run the utility", "No Fund Selected!");
                }
                else
                {
                    string removedIds = string.Empty;
                    string[] funds = fundIDs.Split(',');
                    fundIDs = string.Empty;
                    for (int i = 0; i < funds.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(funds[i]))
                        {
                            int fundId = Convert.ToInt32(funds[i]);
                            if (_dictBackUpCashPreferences == null || !_dictBackUpCashPreferences.ContainsKey(fundId) || _dictBackUpCashPreferences[fundId].SymbolWiseRevaluationDate.Equals(DateTime.MinValue))
                            {
                                string fundName = string.Empty;
                                fundName = "\"" + CachedDataManager.GetInstance.GetAccountText(fundId) + "\"";
                                if (removedIds.Equals(string.Empty))
                                    removedIds = fundName;
                                else
                                    removedIds += ", " + fundName;
                            }
                            else
                                fundIDs += funds[i] + ",";
                        }
                    }
                    DialogResult dr = DialogResult.OK;
                    if (!string.IsNullOrWhiteSpace(removedIds))
                    {
                        dr = MessageBox.Show("The utility won't run for funds " + removedIds + " as there is no Symbol Level Accruals Start Date set for them. \n\nPlease select OK to continue.", "No Symbol Level Accruals Start Date!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    }
                    if (!string.IsNullOrWhiteSpace(fundIDs) && dr.Equals(DialogResult.OK))
                    {
                        dr = MessageBox.Show(@" Please note the following:
 1). The utility will generate entries only till last reval Calc Date.
 2). Symbol Level Accrual Start Date will be picked from DB. (Not from UI)
 3). Utility will run only for selected funds.
 4). Client Release must be turned off while running this utility.

 Please select OK to continue.", "Symbol Level Accruals", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
                    }
                    if (dr.Equals(DialogResult.OK)) isSuccess = true;
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
            return isSuccess;
        }

        /// <summary>
        /// Cleanups the old symbol level journal entries and sub account balances.
        /// </summary>
        /// <param name="FundID">The fund identifier.</param>
        private static void CleanupOldEntries(string FundID)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CleanupSymbolLevelEntriesAfterRun";
                queryData.DictionaryDatabaseParameter.Add("@FundId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundId",
                    ParameterType = DbType.String,
                    ParameterValue = FundID
                });

                queryData.CommandTimeout = 300;
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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
        /// Initialises the accrual utility.
        /// </summary>
        void InitialiseAccrualUtility()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.Query = "Select cp.FundId,CashMgmtStartDate,LastCalcDate from T_CashPreferences cp inner join T_LastCalcDateRevaluation lcd on cp.FundID=lcd.FundID";
                DataTable cashPref = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData).Tables[0];
                foreach (DataRow row in cashPref.Rows)
                {
                    dictcashPref[Convert.ToInt32(row[0])] = new Tuple<DateTime, DateTime>(Convert.ToDateTime(row[1].ToString()), Convert.ToDateTime(row[2].ToString()));
                }
                id = GetMaxGeneratedIDFromDB();
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
        /// Cleans up fundfrom symbol level.
        /// </summary>
        /// <param name="FundID">The fund identifier.</param>
        private static void CleanUpFundfromSymbolLevel(string FundID)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CleanUpFundfromSymbolLevel";
                queryData.DictionaryDatabaseParameter.Add("@FundID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundID",
                    ParameterType = DbType.Int32,
                    ParameterValue = FundID
                });
                queryData.CommandTimeout = 300;
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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
        /// Cleans up fundfrom symbol level.
        /// </summary>
        /// <param name="FundID">The fund identifier.</param>
        private static void UpdateSymbolLevelLastCalcDate(string xml)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_UpdateSymbolLevelLastCalcDate";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = xml
                });

                queryData.CommandTimeout = 300;
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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
        /// Saves the bulk activity.
        /// PRANA-9776 New Parameter UserId
        /// </summary>
        /// <param name="dtData">The dt data.</param>
        /// <param name="FKIDs">The fki ds.</param>
        public static void SaveOrUpdateSymbolLevelAccrual(List<Transaction> transactionsToSave)
        {
            try
            {
                if (transactionsToSave.Count > 0)
                {
                    DataTable SymbolLevelAccrualJournalsData = new DataTable();
                    SymbolLevelAccrualJournalsData.Columns.Add("TransactionEntryID", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("TaxLotId", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("ActivityId_FK", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("AccountID", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("SubAcID", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("CurrencyID", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("Symbol", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("Description", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("TransactionDate", typeof(DateTime));
                    SymbolLevelAccrualJournalsData.Columns.Add("TransactionID", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("DR", typeof(decimal));
                    SymbolLevelAccrualJournalsData.Columns.Add("CR", typeof(decimal));
                    SymbolLevelAccrualJournalsData.Columns.Add("TransactionSource", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("TransactionNumber", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("EntryAccountSide", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("ActivitySource", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("FxRate", typeof(double));
                    SymbolLevelAccrualJournalsData.Columns.Add("FXConversionMethodOperator", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("ModifyDate", typeof(DateTime));
                    SymbolLevelAccrualJournalsData.Columns.Add("EntryDate", typeof(DateTime));
                    SymbolLevelAccrualJournalsData.Columns.Add("UserId", typeof(int));

                    Dictionary<string, DateTime> dictUpdateLastCalcDate = new Dictionary<string, DateTime>();
                    foreach (Transaction t in transactionsToSave)
                    {
                        string taxlotState = t.GetTaxlotState();

                        if (taxlotState != ApplicationConstants.TaxLotState.Deleted.ToString())
                        {
                            foreach (TransactionEntry trEntry in t.TransactionEntries)
                            {
                                string key = trEntry.AccountID + " " + trEntry.SubAcID + " " + trEntry.CurrencyID;
                                DateTime value = trEntry.TransactionDate;
                                if (!dictUpdateLastCalcDate.ContainsKey(key) || (value - dictUpdateLastCalcDate[key]).TotalDays <= 0)
                                {
                                    dictUpdateLastCalcDate[key] = value;
                                }
                                DataRow dr = SymbolLevelAccrualJournalsData.NewRow();
                                fillSymbollevelAccrualDataRow(dr, trEntry);
                                SymbolLevelAccrualJournalsData.Rows.Add(dr);
                            }
                        }
                    }
                    DataSet dataSet = new DataSet();

                    DataTable dataTable = new DataTable("LastCalcDate");
                    dataTable.Columns.Add("AccountID", typeof(int));
                    dataTable.Columns.Add("SubAcID", typeof(int));
                    dataTable.Columns.Add("CurrencyID", typeof(int));
                    dataTable.Columns.Add("TransactionDate", typeof(DateTime));
                    foreach (KeyValuePair<string, DateTime> item in dictUpdateLastCalcDate)
                    {
                        string key = item.Key;
                        string[] keySplitted = key.Split();
                        DateTime value = item.Value;
                        DataRow dr = dataTable.NewRow();
                        dr["AccountID"] = keySplitted[0];
                        dr["SubAcID"] = keySplitted[1];
                        dr["CurrencyID"] = keySplitted[2];
                        dr["TransactionDate"] = item.Value;
                        dataTable.Rows.Add(dr);
                    }
                    dataSet.Tables.Add(dataTable);
                    string xml = dataSet.GetXml();
                    UpdateSymbolLevelLastCalcDate(xml);
                    using (SqlConnection conn = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn))
                        {
                            sqlBulkCopy.DestinationTableName = "T_SymbolLevelAccrualsJournal";

                            sqlBulkCopy.ColumnMappings.Add("TransactionEntryID", "TransactionEntryID");
                            sqlBulkCopy.ColumnMappings.Add("TaxLotId", "TaxLotID");
                            sqlBulkCopy.ColumnMappings.Add("ActivityId_FK", "ActivityId_FK");
                            sqlBulkCopy.ColumnMappings.Add("AccountID", "FundID");
                            sqlBulkCopy.ColumnMappings.Add("SubAcID", "SubAccountID");
                            sqlBulkCopy.ColumnMappings.Add("CurrencyID", "CurrencyID");
                            sqlBulkCopy.ColumnMappings.Add("Symbol", "Symbol");
                            sqlBulkCopy.ColumnMappings.Add("Description", "PBDesc");
                            sqlBulkCopy.ColumnMappings.Add("TransactionDate", "TransactionDate");
                            sqlBulkCopy.ColumnMappings.Add("TransactionID", "TransactionID");
                            sqlBulkCopy.ColumnMappings.Add("DR", "DR");
                            sqlBulkCopy.ColumnMappings.Add("CR", "CR");
                            sqlBulkCopy.ColumnMappings.Add("TransactionSource", "TransactionSource");
                            sqlBulkCopy.ColumnMappings.Add("TransactionNumber", "TransactionNumber");
                            sqlBulkCopy.ColumnMappings.Add("EntryAccountSide", "AccountSide");
                            sqlBulkCopy.ColumnMappings.Add("ActivitySource", "ActivitySource");
                            //PRANA-9777
                            sqlBulkCopy.ColumnMappings.Add("FxRate", "FxRate");
                            sqlBulkCopy.ColumnMappings.Add("FXConversionMethodOperator", "FXConversionMethodOperator");
                            sqlBulkCopy.ColumnMappings.Add("ModifyDate", "ModifyDate"); //PRANA-9776
                            sqlBulkCopy.ColumnMappings.Add("EntryDate", "EntryDate");
                            sqlBulkCopy.ColumnMappings.Add("UserId", "UserId");
                            conn.Open();
                            //timeout set to 2 days.
                            sqlBulkCopy.BulkCopyTimeout = 172800;
                            sqlBulkCopy.WriteToServer(SymbolLevelAccrualJournalsData);
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
        static void fillSymbollevelAccrualDataRow(DataRow dr, TransactionEntry trEntry)
        {
            try
            {
                dr["TransactionEntryID"] = trEntry.TransactionEntryID;
                dr["TaxLotId"] = trEntry.TaxLotId;
                dr["ActivityId_FK"] = trEntry.ActivityId_FK;
                dr["AccountID"] = trEntry.AccountID;
                dr["SubAcID"] = trEntry.SubAcID;
                dr["CurrencyID"] = trEntry.CurrencyID;
                dr["Symbol"] = trEntry.Symbol;
                dr["Description"] = trEntry.Description;
                dr["TransactionDate"] = trEntry.TransactionDate;
                dr["TransactionID"] = trEntry.TransactionID;
                dr["DR"] = trEntry.DR;
                dr["CR"] = trEntry.CR;
                dr["TransactionSource"] = trEntry.TransactionSource;
                dr["TransactionNumber"] = trEntry.TransactionNumber;
                dr["EntryAccountSide"] = trEntry.EntryAccountSide;
                dr["ActivitySource"] = trEntry.ActivitySource;
                dr["FxRate"] = trEntry.FxRate;
                dr["FXConversionMethodOperator"] = trEntry.FXConversionMethodOperator;
                dr["ModifyDate"] = trEntry.ModifyDate;
                dr["EntryDate"] = trEntry.EntryDate;
                dr["UserId"] = trEntry.UserId;
            }
            catch
            {

            }
        }

        /// <summary>
        /// Formats the activities data set.
        /// </summary>
        /// <param name="activitiesDataSet">The activities data set.</param>
        /// <param name="id">The identifier.</param>
        public static void FormatActivitiesDataSet(DataSet activitiesDataSet, ref Int64 id)
        {
            try
            {
                if (activitiesDataSet != null && activitiesDataSet.Tables.Count > 0)
                {

                    activitiesDataSet.Tables[0].Columns.Add("ActivityID", typeof(System.String));
                    activitiesDataSet.Tables[0].Columns.Add("UniqueKey", typeof(System.String));
                    activitiesDataSet.Tables[0].Columns.Add("BalanceType", typeof(System.Int32));
                    activitiesDataSet.Tables[0].Columns.Add("ModifyDate", typeof(System.DateTime)); //PRANA-9777
                    activitiesDataSet.Tables[0].Columns.Add("EntryDate", typeof(System.DateTime)); //PRANA-9777
                    activitiesDataSet.Tables[0].Columns.Add("UserId", typeof(System.Int32)); //PRANA-9776

                    foreach (DataRow dr in activitiesDataSet.Tables[0].Rows)
                    {
                        dr["ActivityID"] = ++id;
                        dr["FKID"] = ++id;
                        dr["UniqueKey"] = dr["FKID"].ToString() + Convert.ToDateTime(dr["TradeDate"]).ToShortDateString() + Convert.ToInt32(dr["TransactionSource"]).ToString() + Convert.ToString(dr["ActivityNumber"]);
                        dr["BalanceType"] = 1;
                        dr["ModifyDate"] = DateTime.Now;
                        dr["EntryDate"] = DateTime.Now;
                        dr["UserId"] = -1;
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
        /// Picks up the max id from T_journal. This id is further used to generate the new distinct ids.
        /// </summary>
        /// <returns></returns>
        public static Int64 GetMaxGeneratedIDFromDB()
        {
            Int64 maxGeneratedID = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMaxGeneratedNumber";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value && row[0].ToString() != string.Empty)
                        {
                            maxGeneratedID = Int64.Parse(row[0].ToString());
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
            return maxGeneratedID;
        }


        /// <summary>
        /// Handles the CheckedChanged event of the CheckBoxPref control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void CheckBoxPref_CheckedChanged(object sender, System.EventArgs e)
        {
            ultraGroupBoxOthers.Enabled = CheckBoxPref.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the CheckBoxAccrualDate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void CheckBoxAccrualDate_CheckedChanged(object sender, System.EventArgs e)
        {
            ultraGroupAccrSymbol.Enabled = CheckBoxAccrualDate.Checked;
        }

        private void CollateralInterest_ValueChanged(object sender, EventArgs e)
        {
            if (CollateralInterest.Value != null)
            {
                CollateralInterestValue = CollateralInterest.Value.ToString();
            }
        }
        /// <summary>
        /// Handles the CheckedChanged event of the CheckBoxCollateralMP control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void CheckBoxCollateralMP_CheckedChanged(object sender, System.EventArgs e)
        {
            _isCollateralMarkPrice = CheckBoxCollateralMP.Checked;
        }

        private void CheckBoxShowTillSettlementDate_CheckedChanged(object sender, EventArgs e)
        {
            _isShowTillSettlementDate = CheckBoxShowTillSettlementDate.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxRevaluationPref control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void checkBoxRevaluationPref_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                grpRevaluationPref.Enabled = checkBoxRevaluationPref.Checked;
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

    }
}
