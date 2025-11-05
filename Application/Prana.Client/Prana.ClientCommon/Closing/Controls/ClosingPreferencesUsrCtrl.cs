using ExportGridsData;
using Infragistics.Win;
//using Prana.PostTrade;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public partial class ClosingPreferencesUsrCtrl : UserControl, IPreferencesSavedClicked, IExportGridData
    {
        #region private variables

        private DataTable _dt = null;
        private Prana.BusinessObjects.CompanyUser _loginUser;
        //private ClosingPreferences _closingPreferencesMain;
        private const string HEADERCAPTION_ALLASSETSCLOSINGALGO = "All Assets Closing Algo";
        private const string HEADERCAPTION_ALLASSETSSECONDARYSORT = "All Assets Secondary Sort";
        private const string COLUMNCAPTION_ALLASSETSCLOSINGALGO = "AllAssetClosingAlgo";
        private const string COLUMNCAPTION_ALLASSETSSECONDARYSORT = "AllAssetSecondarySort";
        private const string COLUMNCAPTION_SECONDARYSORT = "SecondarySort";
        private const string COLUMNCAPTION_FUNDID = "FundID";
        private const string COLUMNCAPTION_ACCOUNTID = "AccountID"; // for change of caption of grid column from fundID to accountID
        private const string COLUMNCAPTION_ACCOUNTNAME = "AccountName"; // for change of caption of grid column from fundName to accountName
        private const string COLUMNCAPTION_FUND = "Fund";
        private const string COLUMNNAME_ASSET = "AssetName";
        private const string COLUMNNAME_FUND = "FundName";
        private const string COLUMNCAPTION_ACA = "ACA";
        private const string COLUMNCAPTION_MULTIPLE = "MULTIPLE";

        #endregion

        public ClosingPreferencesUsrCtrl()
        {
            InitializeComponent();
            InstanceManager.RegisterInstance(this);
        }

        public void SetUp(Prana.BusinessObjects.CompanyUser user)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                _loginUser = user;
                ClosingPrefManager.SetUp(_loginUser.CompanyUserID, Application.StartupPath);

                ClosingPreferences preferences = GetPreferencesFromDB();
                if (preferences != null)
                {
                    SetPreferences(preferences);
                }

                // CreateClosingServicesProxy();
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

        //private void CreateRelationinDataSet(ClosingPreferences preferences)
        //{
        //    try
        //    {
        //        //DataColumn[] parentColumns = null;
        //        //DataColumn[] childColumns = null;

        //        //parentColumns = new DataColumn[] { preferences.AccountingMethodsTable.Tables[0].Columns["AssetName"] };

        //        //childColumns = new DataColumn[] { preferences.AccountingMethodsTable.Tables[1].Columns["AssetName"] };

        //        //preferences.AccountingMethodsTable.Relations.Add(new DataRelation("Asset-Relation", parentColumns, childColumns));
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        private ClosingPreferences GetPreferencesFromDB()
        {
            try
            {
                return ClosingPrefManager.GetPreferences();
                //_closingPreferencesMain.AccountingMethods = ClosingPrefManager.GetAccountingMethods();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            return null;
        }


        public void SetPreferences(ClosingPreferences preferences)
        {
            try
            {
                SetClosingMethodology(preferences.ClosingMethodology);

                chkBoxFetchDataAutomatically.Checked = preferences.IsFetchDataAutomatically;
                txtPriceRoundoffDigits.Text = preferences.PriceRoundOffDigits.ToString();
                txtQtyRoundoffDigits.Text = preferences.QtyRoundoffDigits.ToString();
                chkAutoCloseStrategy.Checked = preferences.ClosingMethodology.IsAutoCloseStrategy;
                txtAutoOptExerciseValue.Text = preferences.AutoOptExerciseValue.ToString();
                chkCopyOpeningTradeAttributes.Checked = preferences.CopyOpeningTradeAttributes;
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


        public void ApplyFilters(ClosingTemplate template)
        {
            try
            {
                Dictionary<int, List<int>> dictMasterFundSubAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                List<string> accounts = GeneralUtilities.GetListFromString(template.GetCommaSeparatedAccounts(dictMasterFundSubAccountAssociation).ToString(), ',');
                List<string> Assets = GeneralUtilities.GetListFromString(template.GetCommaSeparatedAssets().ToString(), ',');

                //List<DataRow> rows = new List<DataRow>();
                //foreach (DataRow dr in _dt.Rows)
                //{
                //    DataRow newRow = _dt.NewRow();
                //    dr.ItemArray.CopyTo(newRow.ItemArray,0);
                //    rows.Add(newRow);

                //}
                for (int i = _dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = _dt.Rows[i];
                    //rows.Add(row);
                    AssetCategory category = (AssetCategory)Enum.Parse(typeof(AssetCategory), row["AssetName"].ToString());
                    int AssetID = (int)category;

                    if (Assets.Count > 0)
                    {
                        if (!Assets.Contains(AssetID.ToString()))
                        {
                            // dr.ItemArray = row.ItemArray;
                            if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached)
                            {
                                _dt.Rows.Remove(row);
                            }
                        }
                    }
                    if (accounts.Count > 0)
                    {
                        if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached)
                        {
                            if (!accounts.Contains(row["FundID"].ToString()))
                            {
                                _dt.Rows.Remove(row);

                            }
                        }
                    }
                }

                if (accounts.Count == 0)
                {
                    Dictionary<int, string> dictAccounts = CachedDataManager.GetInstance.GetAccounts();
                    foreach (int accountID in dictAccounts.Keys)
                    {
                        accounts.Add(accountID.ToString());
                    }
                }

                Dictionary<int, string> closingAssets = CachedDataManager.GetInstance.GetAllClosingAssets();
                if (Assets.Count == 0)
                {

                    foreach (int AssetID in closingAssets.Keys)
                    {
                        Assets.Add(AssetID.ToString());
                    }

                }
                foreach (string assetID in Assets)
                {
                    string AssetName = CachedDataManager.GetInstance.GetAssetText(int.Parse(assetID.ToString()));

                    if (closingAssets.ContainsValue(AssetName))
                    {

                        foreach (string accountID in accounts)
                        {
                            bool isalreadyAdded = false;
                            foreach (DataRow dr in _dt.Rows)
                            {
                                if (dr.RowState != DataRowState.Deleted && dr.RowState != DataRowState.Detached)
                                {
                                    AssetCategory category = (AssetCategory)Enum.Parse(typeof(AssetCategory), dr["AssetName"].ToString());
                                    int AssetID = (int)category;
                                    string AccountID = dr["FundID"].ToString();

                                    if (assetID == AssetID.ToString() && accountID == AccountID)
                                    {
                                        isalreadyAdded = true;
                                        break;
                                    }
                                }
                            }
                            if (!isalreadyAdded)
                            {
                                string AccountName = CachedDataManager.GetInstance.GetAccountText(int.Parse(accountID.ToString()));
                                _dt.Rows.Add(false, AssetName, AccountName, accountID, false, 2, 0);
                            }
                        }
                    }

                }

                _dt.AcceptChanges();
                grdClosingMethod.Refresh();
                grdClosingMethod.Rows.ExpandAll(true);
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

        public void SetClosingMethodology(ClosingMethodology closingMeth)
        {
            try
            {
                _dt = SetClosingMethodologyForNewAccounts(closingMeth);
                // _dt1 = closingMeth.AccountingMethodsTable.Tables[1];
                //_dt1 = preferences.AccountingMethodsTable.Tables[1];
                chkBoxIsShortWithBuyAndBTC.Checked = closingMeth.IsShortWithBuyandBTC;
                chkBoxOverrideGlobal.Checked = closingMeth.OverrideGlobal;
                chkBoxIsSellWithBTC.Checked = closingMeth.IsSellWithBTC;
                //http://jira.nirvanasolutions.com:8080/browse/SS-55

                //grdAccounting.DataSource = _dt;
                //  CreateRelationinDataSet(preferences);
                grdClosingMethod.DataSource = closingMeth.AccountingMethodsTable;
                SetGridFormatting();
                BindClosingMethodology();
                BindClosingAlgoCombo();
                BindSecondarySortCombo();
                BindClosingField();
                chkBoxOverrideDefault_CheckedChanged(null, null);
                cmbMethodology.Value = (int)closingMeth.GlobalClosingMethodology;
                cmbClosingAlgo.Value = (int)closingMeth.ClosingAlgo;
                cmbSecondarySort.Value = (int)closingMeth.SecondarySort;
                cmbClosingField.Value = (int)closingMeth.ClosingField;

                if (closingMeth.GlobalClosingMethodology != (int)(PostTradeEnums.CloseTradeMethodology.Automatic))
                {
                    cmbClosingAlgo.Enabled = false;
                    cmbSecondarySort.Enabled = false;
                    cmbClosingField.Enabled = false;
                }
                txtBoxLongTermTaxRate.Text = (closingMeth.LongTermTaxRate).ToString();
                txtBoxShortTermTaxRate.Text = (closingMeth.ShortTermTaxRate).ToString();
                chkAutoCloseStrategy.Checked = closingMeth.IsAutoCloseStrategy;
                checkExerciseAssigment.Checked = closingMeth.SplitunderlyingBasedOnPosition;

                if (grdClosingMethod.DisplayLayout.Bands[0].Columns[COLUMNNAME_ASSET].IsGroupByColumn)
                {
                    //grdClosingMethod.DisplayLayout.Bands[0].SortedColumns.Remove(COLUMNNAME_ASSET);
                    grdClosingMethod.DisplayLayout.Bands[0].SortedColumns.Remove(COLUMNNAME_FUND);
                }

                grdClosingMethod.Rows.ExpandAll(true);
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
        public DataTable SetClosingMethodologyForNewAccounts(ClosingMethodology closingMeth)
        {
            DataTable accountingMethodTable = closingMeth.AccountingMethodsTable.Tables[0];
            try
            {
                Dictionary<int, string> accounts = CachedDataManager.GetInstance.GetAccounts();
                Dictionary<int, string> closingAssets = CachedDataManager.GetInstance.GetAllClosingAssets();

                List<DataRow> accountsToRemove = new List<DataRow>();
                foreach (DataRow dr in accountingMethodTable.Rows)
                {
                    if (!accounts.ContainsKey(Convert.ToInt32(dr[3])))
                    {
                        accountsToRemove.Add(dr);
                    }
                }
                foreach (DataRow dr in accountsToRemove)
                {
                    accountingMethodTable.Rows.Remove(dr);
                }

                bool areClosingPreferencesCorrupt = false;
                foreach (KeyValuePair<int, string> account in accounts)
                {
                    if (accountingMethodTable.Select("FundID = '" + account.Key + "'").Count() == 0)
                    {
                        foreach (KeyValuePair<int, string> asset in closingAssets)
                        {
                            //TODO: Need to improve this logic
                            if (accountingMethodTable.Columns.Count == 7)
                            {
                                accountingMethodTable.Rows.Add(false, asset.Value, account.Value, account.Key, 2, 0, 0);
                            }
                            else if (accountingMethodTable.Columns.Count == 8)
                            {
                                accountingMethodTable.Rows.Add(false, asset.Value, account.Value, account.Key, false, 2, 0, 0);
                            }
                            else
                            {
                                areClosingPreferencesCorrupt = true;
                            }
                        }
                    }
                }
                if (areClosingPreferencesCorrupt)
                {
                    MessageBox.Show("Your closing preferences have inconsistent data!\nPlease restore them to default", "Preferences Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            return accountingMethodTable;
        }
        public ClosingMethodology GetClosingMethodology()
        {
            ClosingMethodology closingMeth = new ClosingMethodology();
            try
            {
                closingMeth.IsShortWithBuyandBTC = chkBoxIsShortWithBuyAndBTC.Checked;
                closingMeth.IsSellWithBTC = chkBoxIsSellWithBTC.Checked;
                closingMeth.OverrideGlobal = chkBoxOverrideGlobal.Checked;
                closingMeth.GlobalClosingMethodology = (int)cmbMethodology.Value;
                closingMeth.ClosingAlgo = closingMeth.GlobalClosingMethodology == 0 ? (PostTradeEnums.CloseTradeAlogrithm.MANUAL) : (PostTradeEnums.CloseTradeAlogrithm)cmbClosingAlgo.Value;
                closingMeth.SecondarySort = (PostTradeEnums.SecondarySortCriteria)cmbSecondarySort.Value;
                closingMeth.ClosingField = (PostTradeEnums.ClosingField)cmbClosingField.Value;
                double longTermTaxRate = 0.0, shortTermTaxRate = 0.0;
                double.TryParse(txtBoxLongTermTaxRate.Text.ToString(), out longTermTaxRate);
                double.TryParse(txtBoxShortTermTaxRate.Text.ToString(), out shortTermTaxRate);
                closingMeth.LongTermTaxRate = longTermTaxRate;
                closingMeth.ShortTermTaxRate = shortTermTaxRate;
                closingMeth.IsAutoCloseStrategy = chkAutoCloseStrategy.Checked;
                DataSet accountingMethods = new DataSet();
                _dt.AcceptChanges();
                accountingMethods.Tables.Add(_dt.Copy());
                // accountingMethods.Tables.Add(_dt1.Copy());

                // Add another Datatable to the dataset related to asset wise closing methods
                closingMeth.AccountingMethodsTable = accountingMethods;
                closingMeth.SplitunderlyingBasedOnPosition = checkExerciseAssigment.Checked;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return closingMeth;
        }


        //public void SetUp(DataTable dt)
        //{
        //    //_dt = dt;
        //    _dt = CachedDataManager.GetInstance.GetAccountingMethodsTable();
        //    grdAccounting.DataSource = _dt;
        //    SetGridFormatting();
        //    BindClosingAlgoCombo();

        //}

        private void BindClosingMethodology()
        {
            try
            {
                cmbMethodology.DataSource = null;
                List<EnumerationValue> lst = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.CloseTradeMethodology));
                //add select option at the start of the combobox items
                lst.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                //lst.Add(new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                cmbMethodology.DataSource = lst;
                cmbMethodology.DisplayMember = "DisplayText";
                cmbMethodology.ValueMember = "Value";
                cmbMethodology.Value = 1;
                Utils.UltraComboFilter(cmbMethodology, "DisplayText");
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

        private void BindClosingAlgoCombo()
        {
            try
            {
                List<EnumerationValue> ClosingAlgos = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.CloseTradeAlogrithm));
                List<EnumerationValue> ClosingAlgosWithoutPreset = new List<EnumerationValue>();
                foreach (EnumerationValue value in ClosingAlgos)
                {
                    //Modified By : Manvendra Jira : PRANA-10341
                    if (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.PRESET.ToString()) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.NONE.ToString())) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.MANUAL.ToString())) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.Multiple.ToString())))
                    {
                        ClosingAlgosWithoutPreset.Add(value);
                    }
                }
                cmbClosingAlgo.DataSource = null;
                cmbClosingAlgo.DataSource = ClosingAlgosWithoutPreset;
                cmbClosingAlgo.ValueMember = "Value";
                cmbClosingAlgo.DisplayMember = "DisplayText";
                cmbClosingAlgo.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbClosingAlgo.Value = 1;
                // cmbClosingAlgo.Enabled = false;
                Utils.UltraComboFilter(cmbClosingAlgo, "DisplayText");
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

        private void BindSecondarySortCombo()
        {
            try
            {
                List<EnumerationValue> SecondarySortCriteria = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.SecondarySortCriteria));
                // List<EnumerationValue> ClosingAlgosWithoutPreset = new List<EnumerationValue>();
                //foreach (EnumerationValue value in SecondarySortCriteria)
                //{

                //    SecondarySortCriteria.Add(value);

                //}
                cmbSecondarySort.DataSource = null;
                cmbSecondarySort.DataSource = SecondarySortCriteria;
                cmbSecondarySort.ValueMember = "Value";
                cmbSecondarySort.DisplayMember = "DisplayText";
                cmbSecondarySort.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbSecondarySort.Value = 0;
                //  cmbSecondarySort.Enabled = false;
                Utils.UltraComboFilter(cmbSecondarySort, "DisplayText");
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

        private void BindClosingField()
        {
            try
            {
                List<EnumerationValue> ClosingFields = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.ClosingField));
                //ClosingFields.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                cmbClosingField.DataSource = null;
                cmbClosingField.DataSource = ClosingFields;
                cmbClosingField.ValueMember = "Value";
                cmbClosingField.DisplayMember = "DisplayText";
                cmbClosingField.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbClosingField.Value = 0;
                Utils.UltraComboFilter(cmbClosingField, "DisplayText");
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



        private void SetGridFormatting()
        {
            try
            {
                //tpGetData.SetToolTip(btnSetToGlobal, "Set Global Closing Alogrithm/SecondarySort criteria for selected accounts");
                this.grdClosingMethod.Rows.ExpandAll(true);
                this.grdClosingMethod.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
                //this.grdClosingMethod.DisplayLayout.Bands[0].Columns["AssetName"].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                this.grdClosingMethod.DisplayLayout.Bands[0].Columns["Select"].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                // this.grdClosingMethod.DisplayLayout.Bands[0].Columns["IsACA"].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                grdClosingMethod.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
                // grid.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Collapsed;
                grdClosingMethod.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;

                grdClosingMethod.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
                //this.grdClosingMethod.DisplayLayout.Bands[0].Columns["ACA"].Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Right;

                //grdAccounting.DisplayLayout.Override.AllowColSizing = AllowColSizing.None;
                //grdAccounting.DisplayLayout.Override.HeaderAppearance.BorderAlpha = Alpha.Transparent;

                //UltraGridBand band = grdAccounting.DisplayLayout.Bands[0];
                UltraGridBand bandClosingMethod0 = grdClosingMethod.DisplayLayout.Bands[0];
                UltraGridBand bandClosingMethod1 = grdClosingMethod.DisplayLayout.Bands[0];

                bandClosingMethod1.Columns["SecondarySort"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                bandClosingMethod1.Columns["ClosingField"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;


                //band.Columns["ACA"].Header.Caption = "Select";

                //band.Columns[COLUMNCAPTION_ALLASSETSCLOSINGALGO].ValueList = GetClosingAlgoList(true);
                //band.Columns[COLUMNCAPTION_ALLASSETSCLOSINGALGO].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //band.Columns[COLUMNCAPTION_ALLASSETSCLOSINGALGO].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                //band.Columns[COLUMNCAPTION_ALLASSETSCLOSINGALGO].Width = 250;
                //band.Columns[COLUMNCAPTION_ALLASSETSCLOSINGALGO].Header.Caption = HEADERCAPTION_ALLASSETSCLOSINGALGO;

                ////Narendra Kumar Jangir May 15 2013
                ////There should be an option to select secondary sort for all accounts at once
                //band.Columns[COLUMNCAPTION_ALLASSETSSECONDARYSORT].ValueList = GetSecondarySortValueList(true);
                //band.Columns[COLUMNCAPTION_ALLASSETSSECONDARYSORT].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //band.Columns[COLUMNCAPTION_ALLASSETSSECONDARYSORT].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                //band.Columns[COLUMNCAPTION_ALLASSETSSECONDARYSORT].Width = 250;
                //band.Columns[COLUMNCAPTION_ALLASSETSSECONDARYSORT].Header.Caption = HEADERCAPTION_ALLASSETSSECONDARYSORT;


                Dictionary<int, string> dictClosingAssets = CachedDataManager.GetInstance.GetAllClosingAssets();
                Dictionary<int, string> dictAccounts = CachedDataManager.GetInstance.GetAccounts();
                foreach (string Assetname in dictClosingAssets.Values)
                {

                    foreach (string AccountName in dictAccounts.Values)
                    {
                        //band.Columns[Assetname].ValueList = GetClosingAlgoList(false);
                        //band.Columns[Assetname].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        //band.Columns[Assetname].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        //band.Columns[Assetname].Width = 90;
                        bandClosingMethod1.Columns["ClosingAlgo"].ValueList = GetClosingAlgoList(false);
                        bandClosingMethod1.Columns["SecondarySort"].ValueList = GetSecondarySortValueList(false);
                        bandClosingMethod1.Columns["ClosingField"].ValueList = GetClosingFieldValueList(false);
                        // bandClosingMethod1.Columns["IsACA"].Hidden = true;

                        //band.Columns[Assetname].Hidden = true;
                        //bandClosingMethod0.Columns[Assetname].Hidden = true;

                        //band.Columns[Assetname + COLUMNCAPTION_SECONDARYSORT].ValueList = GetSecondarySortValueList(false);
                        //band.Columns[Assetname + COLUMNCAPTION_SECONDARYSORT].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        //band.Columns[Assetname + COLUMNCAPTION_SECONDARYSORT].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        //band.Columns[Assetname + COLUMNCAPTION_SECONDARYSORT].Width = 90;
                        //band.Columns[Assetname + COLUMNCAPTION_SECONDARYSORT].Header.Caption = COLUMNCAPTION_SECONDARYSORT;
                        //band.Columns[Assetname + COLUMNCAPTION_SECONDARYSORT].Hidden = true;
                        //bandClosingMethod0.Columns[Assetname + COLUMNCAPTION_SECONDARYSORT].Hidden = true;
                        #region commented
                        //band.Columns["EquitySecondarySort"].ValueList = GetValueList(false);
                        //band.Columns["EquitySecondarySort"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        //band.Columns["EquitySecondarySort"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        //band.Columns["EquitySecondarySort"].Width = 70;

                        //band.Columns["Future"].ValueList = GetValueList(false);
                        //band.Columns["Future"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        //band.Columns["Future"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        //band.Columns["Future"].Width = 70;

                        //band.Columns["EquitySecondarySort"].ValueList = GetValueList(false);
                        //band.Columns["EquitySecondarySort"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        //band.Columns["EquitySecondarySort"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        //band.Columns["EquitySecondarySort"].Width = 70;

                        //band.Columns["EquityOption"].ValueList = GetValueList(false);
                        //band.Columns["EquityOption"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        //band.Columns["EquityOption"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        //band.Columns["EquityOption"].Width = 80;

                        //band.Columns["EquityOption"].ValueList = GetValueList(false);
                        //band.Columns["EquityOption"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        //band.Columns["EquityOption"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        //band.Columns["EquityOption"].Width = 80;

                        //band.Columns["FutureOption"].ValueList = GetValueList(false);
                        //band.Columns["FutureOption"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        //band.Columns["FutureOption"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        //band.Columns["FutureOption"].Width = 80;
                        #endregion
                    }
                }


                //band.Columns[COLUMNCAPTION_FUNDID].Hidden = true;
                //bandClosingMethod0.Columns[COLUMNCAPTION_ALLASSETSCLOSINGALGO].Hidden = true;
                //bandClosingMethod0.Columns[COLUMNCAPTION_ALLASSETSSECONDARYSORT].Hidden = true;

                bandClosingMethod1.Columns["Select"].Header.Caption = "";
                bandClosingMethod0.Columns["AssetName"].Width = 99;
                bandClosingMethod1.Columns["Select"].Width = 30;
                bandClosingMethod1.Columns["FundID"].Hidden = true;

                //bandClosingMethod0.Columns["SelectAll"].Hidden = true;

                //band.Columns[COLUMNCAPTION_FUND].CellActivation = Activation.NoEdit;
                //band.Columns[COLUMNCAPTION_FUND].Width = 120;
                //bandClosingMethod0.Columns[COLUMNCAPTION_FUND].Width = 200;

                //this.grdClosingMethod.DisplayLayout.Override.FixedRowStyle = FixedRowStyle.Top;

                //this.grdClosingMethod.Rows.FixedRows.Add(this.grdClosingMethod.Rows[4]);

                //this.grdClosingMethod.DisplayLayout.Rows[2].Fixed = true;

                bandClosingMethod0.Columns["AssetName"].CellClickAction = CellClickAction.RowSelect;
                bandClosingMethod1.Columns["FundName"].CellClickAction = CellClickAction.RowSelect;

                bandClosingMethod0.Columns["AssetName"].CellActivation = Activation.NoEdit;
                bandClosingMethod1.Columns["FundName"].CellActivation = Activation.NoEdit;
                bandClosingMethod0.Columns["FundName"].Header.Caption = COLUMNCAPTION_ACCOUNTNAME;
                bandClosingMethod0.Columns["FundID"].Header.Caption = COLUMNCAPTION_ACCOUNTID;
                bandClosingMethod1.SortedColumns.Add("FundName", true, true);
                //grdClosingMethod.DisplayLayout.Bands[0].Columns[COLUMNCAPTION_FUNDID].Hidden = true;
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
        private ValueList GetClosingAlgoList(bool isMultiple)
        {
            ValueList coll = new ValueList();
            try
            {
                int i = 0;
                Type enumType = typeof(PostTradeEnums.CloseTradeAlogrithm);
                string[] array = Enum.GetNames(enumType);

                foreach (string name in array)
                {
                    //Modified By : Manvendra Jira : PRANA-10341
                    if ((name != PostTradeEnums.CloseTradeAlogrithm.PRESET.ToString()) && (name != PostTradeEnums.CloseTradeAlogrithm.MANUAL.ToString()) && (name != PostTradeEnums.CloseTradeAlogrithm.NONE.ToString()) && (name != PostTradeEnums.CloseTradeAlogrithm.Multiple.ToString()))
                    {
                        i = Convert.ToInt32(Enum.Parse(enumType, name));
                        coll.ValueListItems.Add(i, name.ToString());
                    }
                }

                if (isMultiple)
                {
                    coll.ValueListItems.Add(i + 1, COLUMNCAPTION_MULTIPLE);
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
            return coll;
        }

        private ValueList GetSecondarySortValueList(bool isMultiple)
        {
            ValueList coll = new ValueList();
            try
            {
                int i = 0;
                Type enumType = typeof(PostTradeEnums.SecondarySortCriteria);
                string[] array = Enum.GetNames(enumType);

                foreach (string name in array)
                {
                    i = Convert.ToInt32(Enum.Parse(enumType, name));
                    coll.ValueListItems.Add(i, name.ToString());
                }
                if (isMultiple)
                {
                    coll.ValueListItems.Add(i + 1, COLUMNCAPTION_MULTIPLE);
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
            return coll;
        }

        private ValueList GetClosingFieldValueList(bool isMultiple)
        {
            ValueList coll = new ValueList();
            try
            {
                int i = 0;
                Type enumType = typeof(PostTradeEnums.ClosingField);
                string[] array = Enum.GetNames(enumType);

                foreach (string name in array)
                {
                    i = Convert.ToInt32(Enum.Parse(enumType, name));
                    coll.ValueListItems.Add(i, name.ToString());
                }
                if (isMultiple)
                {
                    coll.ValueListItems.Add(i + 1, COLUMNCAPTION_MULTIPLE);
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
            return coll;
        }

        //private ValueList GetValueListWithMultiple()
        //{
        //    int i = 0;
        //    Type enumType = typeof(PostTradeEnums.CloseTradeAlogrithm);
        //    string[] array = Enum.GetNames(enumType);
        //    ValueList coll = new ValueList();
        //    foreach (string name in array)
        //    {
        //        if (name != PostTradeEnums.CloseTradeAlogrithm.PRESET.ToString() && (name != PostTradeEnums.CloseTradeAlogrithm.ACA.ToString()) && name != PostTradeEnums.CloseTradeAlogrithm.NONE.ToString())
        //        {
        //            i = Convert.ToInt32(Enum.Parse(enumType, name));
        //            coll.ValueListItems.Add(i, name.ToString());
        //        }
        //    }
        //    coll.ValueListItems.Add(i + 1, COLUMNCAPTION_MULTIPLE);
        //    _bindedAlgoList = coll;
        //    return coll;
        //}

        private void chkBoxOverrideDefault_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //cmbSecondarySort.Enabled = true;

                //Narendra Kumar Jangir May 15 2013
                //Set enable/disable of closing algo and secondary sort criteria on the basis of closing methodology
                //forcefully call cmbMethodology_ValueChanged() method
                //object val = cmbMethodology.Value;
                //cmbMethodology.Value = null;
                //cmbMethodology.Value = val;

                if (chkBoxOverrideGlobal.Checked)
                    grdClosingMethod.Enabled = true;
                else
                    grdClosingMethod.Enabled = false;

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

        private ClosingPreferences GetLatestPrefData()
        {
            ClosingPreferences closingPreferences = new ClosingPreferences();
            try
            {
                closingPreferences.ClosingMethodology = GetClosingMethodology();
                closingPreferences.IsFetchDataAutomatically = chkBoxFetchDataAutomatically.Checked;

                #region Set roundoff digit preferences
                int priceRoundOffDigits, qtyRoundoffDigits;
                bool isPriceParsed = int.TryParse(txtPriceRoundoffDigits.Text, out priceRoundOffDigits);
                bool isQtyParsed = int.TryParse(txtQtyRoundoffDigits.Text, out qtyRoundoffDigits);
                if (isPriceParsed)
                    closingPreferences.PriceRoundOffDigits = priceRoundOffDigits;
                if (isQtyParsed)
                    closingPreferences.QtyRoundoffDigits = qtyRoundoffDigits;
                #endregion
                decimal autoOptExerciseValue;
                bool isAutoExerciseValueParsed = decimal.TryParse(txtAutoOptExerciseValue.Text, out autoOptExerciseValue);
                if (isAutoExerciseValueParsed)
                    closingPreferences.AutoOptExerciseValue = autoOptExerciseValue;
                closingPreferences.CopyOpeningTradeAttributes = chkCopyOpeningTradeAttributes.Checked;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            return closingPreferences;
        }
        private void SaveClosingPreferences(ClosingPreferences preferences)
        {
            try
            {
                ClosingPrefManager.SavePreferences(preferences, false);
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


        #region IPreferences Members


        public UserControl Reference()
        {
            return this;
        }

        public bool Save()
        {
            try
            {
                this.SaveClosingPreferences(GetLatestPrefData());

                if (SaveClicked != null)
                {
                    //SaveClicked(this, null);
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
            return true;
        }


        public void RestoreDefault()
        {
            try
            {
                SetPreferences(ClosingPrefManager.GetDefualtPreferences());
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


        public IPreferenceData GetPrefs()
        {
            ClosingPreferences closingPreferences = null;
            try
            {
                closingPreferences = GetLatestPrefData();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
            return closingPreferences;
        }

        public event EventHandler SaveClicked;
        private string _modulename = string.Empty;


        public string SetModuleActive
        {
            set
            {
                _modulename = value;

            }
        }


        #endregion

        private void txtBoxLongTermTaxRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (System.Text.RegularExpressions.Regex.IsMatch("^[0-9]*[.][0-9]*$", txtBoxLongTermTaxRate.Text))
                {
                    MessageBox.Show("Please enter only numbers.");
                    txtBoxLongTermTaxRate.Text.Remove(txtBoxLongTermTaxRate.Text.Length - 1);
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


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void grdClosingMethod_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            //CheckState currentCheckState = e.Column.GetHeaderCheckedState(e.Rows);
            //if(e.Column.ToString() == "AssetName")
            //    foreach (DataRow row in _dt1.Rows)
            //    {
            //        if (currentCheckState == CheckState.Checked)
            //        {
            //            row["Select"] = true;
            //        }
            //        else if (currentCheckState == CheckState.Unchecked)
            //        {
            //            row["Select"] = false;
            //        }
            //    }

            //grdClosingMethod.Refresh();

        }

        private void cmbMethodology_ValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (cmbMethodology.Value != null && (PostTradeEnums.CloseTradeMethodology)
                          cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Manual)
                {
                    cmbClosingAlgo.Enabled = false;
                    cmbSecondarySort.Enabled = false;
                    cmbClosingField.Enabled = false;
                }
                else if (cmbMethodology.Value != null && (PostTradeEnums.CloseTradeMethodology)
                          cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Automatic)
                {
                    cmbClosingAlgo.Enabled = cmbMethodology.Enabled;
                    cmbSecondarySort.Enabled = cmbMethodology.Enabled;
                    cmbClosingField.Enabled = cmbMethodology.Enabled;
                }
                else
                {
                    cmbClosingAlgo.Enabled = false;
                    cmbSecondarySort.Enabled = false;
                    cmbClosingField.Enabled = false;
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

        private void resetToGlobalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow row in _dt.Rows)
                {
                    if (chkBoxOverrideGlobal.Checked)
                    {
                        if ((bool)row["Select"])
                        {
                            row["ClosingAlgo"] = cmbClosingAlgo.Value;
                            row["SecondarySort"] = cmbSecondarySort.Value;
                            row["ClosingField"] = cmbClosingField.Value;
                        }
                    }
                    else
                    {
                        row["ClosingAlgo"] = cmbClosingAlgo.Value;
                        row["SecondarySort"] = cmbSecondarySort.Value;
                        row["ClosingField"] = cmbClosingField.Value;
                    }
                    grdClosingMethod.Refresh();
                    //row["Select"] = false;
                }
                //chkBoxOverrideGlobal.Checked = false;

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

        private void swapGroupingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridBand bandClosingMethod1 = grdClosingMethod.DisplayLayout.Bands[0];
                if (bandClosingMethod1.Columns[COLUMNNAME_ASSET].IsGroupByColumn)
                {
                    bandClosingMethod1.SortedColumns.Remove(COLUMNNAME_ASSET);
                    bandClosingMethod1.SortedColumns.Add(COLUMNNAME_FUND, true, true);
                }
                else
                {
                    if (bandClosingMethod1.SortedColumns.Count > 0)
                    {
                        if (bandClosingMethod1.Columns[COLUMNNAME_FUND].IsGroupByColumn)
                            bandClosingMethod1.SortedColumns.Remove(COLUMNNAME_FUND);
                        bandClosingMethod1.SortedColumns.Add(COLUMNNAME_ASSET, true, true);

                    }
                }
                this.grdClosingMethod.Rows.ExpandAll(true);

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

        private void chkBoxOverrideGlobal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBoxOverrideGlobal.Checked)
                    grdClosingMethod.Enabled = true;
                else
                    grdClosingMethod.Enabled = false;
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

        private void grdClosingMethod_AfterRowRegionScroll(object sender, RowScrollRegionEventArgs e)
        {

        }

        private void grdClosingMethod_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Point pt = grdClosingMethod.PointToScreen(e.Location);
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip.Show(pt);
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

        private void grdClosingMethod_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        private void toggleOutlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdClosingMethod.Rows[0].Expanded)
                {
                    grdClosingMethod.Rows.CollapseAll(true);
                }
                else
                    grdClosingMethod.Rows.ExpandAll(true);
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
        #region UnusedCode

        //internal void LoadClosingAlgo(ClosingTemplate template)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //internal void UpdateClosingPreferences(ClosingTemplate closingTemplate)
        //{

        //}

        //internal void GetData(ClosingTemplate closingTemplate)
        //{
        //    try
        //    {
        //        closingTemplate.ClosingMeth.DictAssetWiseMeth.Clear();
        //        foreach (DataRow row in _dt.Rows)
        //        {
        //            SerializableDictionary<string, ClosingMethodology> closingDict = new SerializableDictionary<string, ClosingMethodology>();
        //            string key = row["AssetName"].ToString();
        //            ClosingMethodology closingMethodology = new ClosingMethodology();
        //            foreach (DataRow row1 in _dt1.Rows)
        //            {
        //                ClosingMethodology closingMethodology1 = new ClosingMethodology();
        //                string key1 = row1["AssetName"].ToString();
        //                string keyAccount = row1["AccountName"].ToString();
        //                bool isSelected = (bool)row1["Select"];
        //                if (key1 == key && isSelected)
        //                {
        //                    closingMethodology1.ClosingAlgo = (PostTradeEnums.CloseTradeAlogrithm)Enum.Parse(typeof(PostTradeEnums.CloseTradeAlogrithm), row1["ClosingAlgo"].ToString());
        //                    closingMethodology1.SecondarySort = (PostTradeEnums.SecondarySortCriteria)Enum.Parse(typeof(PostTradeEnums.SecondarySortCriteria), row1["SecondarySort"].ToString());
        //                    closingMethodology1.IsACA = (bool)row1["IsACA"];
        //                    closingDict.Add(keyAccount, closingMethodology1);
        //                }
        //            }
        //            closingMethodology.DictAssetWiseMeth = closingDict;
        //            if (closingDict.Count > 0)
        //                closingTemplate.ClosingMeth.DictAssetWiseMeth.Add(key, closingMethodology);
        //            closingTemplate.ClosingMeth.IsSellWithBTC = chkBoxIsSellWithBTC.Checked;
        //            closingTemplate.ClosingMeth.IsShortWithBuyandBTC = chkBoxIsShortWithBuyAndBTC.Checked;
        //            closingTemplate.ClosingMeth.LongTermTaxRate = Double.Parse(txtBoxLongTermTaxRate.Text);
        //            closingTemplate.ClosingMeth.ShortTermTaxRate = Double.Parse(txtBoxShortTermTaxRate.Text);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }

        //    }
        //}

        //internal void LoadData(ClosingTemplate template)
        //{
        //    try
        //    {
        //        RestoreDefault();
        //        chkBoxOverrideGlobal.Checked = true;
        //        ClosingMethodology closingMethodology = template.ClosingMeth;
        //        Dictionary<string, ClosingMethodology> dictClosing = closingMethodology.DictAssetWiseMeth;

        //        if (closingMethodology.IsSellWithBTC)
        //            chkBoxIsSellWithBTC.Checked = true;
        //        else
        //            chkBoxIsSellWithBTC.Checked = false;

        //        if (closingMethodology.IsShortWithBuyandBTC)
        //            chkBoxIsShortWithBuyAndBTC.Checked = true;
        //        else
        //            chkBoxIsShortWithBuyAndBTC.Checked = false;

        //        txtBoxLongTermTaxRate.Text = closingMethodology.LongTermTaxRate.ToString();
        //        txtBoxShortTermTaxRate.Text = closingMethodology.ShortTermTaxRate.ToString();
        //        foreach (string assetName in dictClosing.Keys)
        //        {
        //            ClosingMethodology closingMethodologyAssetWise = dictClosing[assetName];
        //            Dictionary<string, ClosingMethodology> dictClosingAssetWise = closingMethodologyAssetWise.DictAssetWiseMeth;
        //            foreach (string accountName in dictClosingAssetWise.Keys)
        //            {
        //                ClosingMethodology closingMethodologyAccountWise = dictClosingAssetWise[accountName];
        //                int closingAlgo = (int) closingMethodologyAccountWise.ClosingAlgo;
        //                int secondarySort =(int) closingMethodologyAccountWise.SecondarySort;
        //                bool isACA = closingMethodologyAccountWise.IsACA;
        //                foreach (DataRow row in _dt1.Rows)
        //                {
        //                    if (row["AssetName"].ToString() == assetName && row["AccountName"].ToString() == accountName)
        //                    {
        //                        row["Select"] = true;
        //                        row["ClosingAlgo"] = closingAlgo;
        //                        row["SecondarySort"] = secondarySort;
        //                        row["IsACA"] = isACA;
        //                    }
        //                }
        //                //_dt1.Rows.Add()
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }

        //    }

        //}
        #endregion

        /// <summary>
        /// remove invalid preferences
        /// </summary>
        /// <returns>true to cancel closing event, false otherwise</returns>
        public bool RemoveInvalidNewPreferences()
        {
            // This method is required in allocation preferences so added it to IPreferencesSavedClicked interface
            // Defining this method here as it implements IPreferencesSavedClicked interface
            // returned false so that prefernces are closed in case of closing prerfernces
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7332
            return false;
        }

        private void grdClosingMethod_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdClosingMethod);
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

        private void grdClosingMethod_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        /// <summary>
        /// To export data for automation
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                // Create a new instance of the exporter
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                exporter.Export(grdClosingMethod, filePath);
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
    }
}
