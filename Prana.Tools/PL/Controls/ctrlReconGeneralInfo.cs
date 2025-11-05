using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
//using Prana.Reconciliation;
using Prana.Global;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace Prana.Tools
{
    public partial class ctrlReconGeneralInfo : UserControl
    {
        Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("-Select-", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);

        private bool _isUnSavedChanges = false;
        private ReconTemplate _reconTemplate;
        //int _clientID = int.MinValue;
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlReconGeneralInfo()
        {
            try
            {
                InitializeComponent();
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

        internal void LoadData(ReconTemplate template)
        {
            try
            {
                // _clientID = clientID;
                _reconTemplate = template;
                ctrlMatchingRules1.LoadMatchingRules(_reconTemplate);
                if (_reconTemplate.ReconFilters.DictAssets.Count == 0)
                    multiSelectDropDown1.SelectUnselectAll(CheckState.Checked);
                else
                    multiSelectDropDown1.SelectUnselectItems(_reconTemplate.ReconFilters.DictAssets, CheckState.Checked);
                //multiSelectDropDown1.SetTitleText(_reconTemplate.ReconFilters.DictAssets);

                //Dictionary<int, List<int>> dictPrimeBrokerAccountAssociation = CachedDataManager.GetInstance.GetDataSourceSubAccountAssociation();

                //assign prime broker dictionary here
                //List<GenericNameID> lstPrimeBrokers = CachedDataManager.GetInstance.GetAllPrimeBrokers();

                //Dictionary<int, string> dictAccounts = CachedDataManager.GetInstance.GetAccounts();
                ctrlPBAccountMapping1.setTemplate(_reconTemplate);
                ReconUIUtilities.BindThirdPartyCombo(cmbThirdPartyFormat, _reconTemplate.FormatName, ultraToolTipInfo1, ultraToolTipManager1);
                ReconUIUtilities.BindReconDateTypeCombo(cmbRunByDate, _reconTemplate.ReconDateType);
                ctrlReconGroupByColumns1.LoadGroupCriteria(_reconTemplate);
                //multiSelectDropDown_New1.SelectUnselectAll(dictPrimeBrokerAccountAssociation, lstPrimeBrokers, dictAccounts);
                //CHMW-2235	Taxlot recon implementation.
                if (template.ReconType == ReconType.TaxLot)
                {
                    ctrlReconGroupByColumns1.Visible = false;
                }
                else
                {
                    ctrlReconGroupByColumns1.Visible = true;
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
        #region created method in ReconUIUtilities
        //private void BindReconDateTypeCombo()
        //{
        //    try
        //    {
        //        List<EnumerationValue> listValues = new List<EnumerationValue>();// EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(ReconType));
        //        listValues.Insert(0, new EnumerationValue("Trade Date", ReconDateType.TradeDate.ToString()));
        //        listValues.Insert(1, new EnumerationValue("Process Date", ReconDateType.ProcessDate.ToString()));
        //        listValues.Insert(2, new EnumerationValue("Nirvana Process Date", ReconDateType.NirvanaProcessDate.ToString()));
        //        cmbRunByDate.DataSource = null;
        //        cmbRunByDate.DataSource = listValues;
        //        cmbRunByDate.DisplayMember = "DisplayText";
        //        cmbRunByDate.ValueMember = "Value";
        //        cmbRunByDate.DataBind();
        //        cmbRunByDate.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
        //        cmbRunByDate.DisplayLayout.Bands[0].Header.Enabled = false;
        //        cmbRunByDate.Value = _reconTemplate.ReconDateType;
        //        cmbRunByDate.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        ///// <summary>
        ///// Loads Data on the cmbThirdPartyFormat from db
        ///// </summary>
        //private void BindThirdPartyCombo()
        //{
        //    try
        //    {
        //        Dictionary<string, RunUpload> runUploadValueList = ReconPreferences.DictRunUpload;
        //        if (runUploadValueList != null)
        //        {
        //            cmbThirdPartyFormat.DataSource = runUploadValueList.ToArray();
        //            cmbThirdPartyFormat.DisplayMember = "Key";
        //            cmbThirdPartyFormat.ValueMember = "Key";
        //            //hides the column value
        //            cmbThirdPartyFormat.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
        //            cmbThirdPartyFormat.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //            //set column width equal to control
        //            cmbThirdPartyFormat.DisplayLayout.Bands[0].Columns["Key"].Width = cmbThirdPartyFormat.Size.Width;
        //            //set tool tip for every row
        //            int index = 0;
        //            foreach (string formatNames in runUploadValueList.Keys)
        //            {
        //                cmbThirdPartyFormat.Rows[index].ToolTipText = formatNames;
        //                index++;
        //            }
        //            //set the templete prefrence value to combo box
        //            if (!string.IsNullOrEmpty(_reconTemplate.FormatNam
        //            {
        //                cmbThirdPartyFormat.Text = _reconTemplate.FormatName;
        //            }
        //            ultraToolTipInfo1.ToolTipText = cmbThirdPartyFormat.Text;
        //            if (string.IsNullOrEmpty(cmbThirdPartyFormat.Text))
        //            {
        //                ultraToolTipInfo1.ToolTipText = "No File selected";
        //            }
        //            //ultraToolTipManager1.DisplayStyle = ToolTipDisplayStyle.Standard;
        //            ultraToolTipManager1.SetUltraToolTip(cmbThirdPartyFormat, ultraToolTipInfo1);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    // return null;
        //}
        #endregion

        /// <summary>
        /// Update ui data in user preferences
        /// </summary>
        /// <param name="reconTemplate"></param>
        /// <returns></returns>
        internal void UpdateData(ReconTemplate reconTemplate)
        {
            try
            {
                UpdateMatchingRules(reconTemplate);
                UpdatePBAccountMapping();
                UpdateAssetFilters(reconTemplate);
                ctrlReconGroupByColumns1.UpdateGroupCriteria(reconTemplate);

                reconTemplate.ReconDateType = (ReconDateType)Enum.Parse(typeof(ReconDateType), cmbRunByDate.Value.ToString());
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
        /// Update all the changes of asset filters in recon template preferences.
        /// </summary>
        /// <param name="reconTemplate"></param>
        private void UpdateAssetFilters(ReconTemplate reconTemplate)
        {
            try
            {
                //CHMW-2231	[Recon] - Asset is not saving properly
                //if (reconTemplate.ReconFilters.DictAssets.Count != multiSelectDropDown1.GetNoOfCheckedItems())
                {
                    //_isUnsavedChanges = true;
                    reconTemplate.ReconFilters.DictAssets.Clear();
                    //if all the assets are checked then asset filter should be blank and data for all the assets will be fetched
                    //this is done to remove unnecessarily joins from the DB
                    if (!multiSelectDropDown1.GetNoOfCheckedItems().Equals(multiSelectDropDown1.GetNoOfTotalItems()))
                    {
                        foreach (KeyValuePair<int, string> kvp in multiSelectDropDown1.GetSelectedItemsInDictionary())
                        {
                            reconTemplate.ReconFilters.DictAssets.Add(kvp.Key, kvp.Value);
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
        /// Update all the changes of asset filters in recon template preferences.
        /// </summary>
        /// <param name="reconTemplate"></param>
        private void UpdatePBAccountMapping()
        {
            try
            {
                ctrlPBAccountMapping1.UpdateCheckedAccounts();
                ctrlPBAccountMapping1.UpdateCheckedPB();
                //reconTemplate.ReconFilters.DictAccounts;
                //reconTemplate.ReconFilters.DictPrimeBrokers;


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
        internal void UpdateMatchingRules(ReconTemplate reconTemplate)
        {
            try
            {
                ctrlMatchingRules1.UpdateMatchingRules(reconTemplate);
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
        /// Asset filter update is separate user control and it should be synchronized with nirvana asset filters
        /// </summary>
        /// <param name="reconTemplate"></param>
        internal void UpdateDataForAssetFilters(ReconTemplate reconTemplate)
        {
            try
            {
                multiSelectDropDown1.SelectUnselectItems(reconTemplate.ReconFilters.DictAssets, CheckState.Checked);
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
        /// This method return true if there are any unsaved changes on the UI, otherwise it will return false
        /// </summary>
        /// <returns></returns>
        internal bool IsUnsavedChanges()
        {
            try
            {
                //check if changes are made in any of the user control
                //modified by:sachin mishra Purpose:CHMW-2633
                bool isctrlReconGroup = ctrlReconGroupByColumns1.IsGroupingChanged(_reconTemplate);
                bool isctrlMatchingRules = ctrlMatchingRules1.IsUnsavedChanges();
                bool isctrlPBAccountMapping = ctrlPBAccountMapping1.IsUnsavedChanges();
                bool ismultiSelectDropDown = multiSelectDropDown1.IsUnsavedChanges();
                if (isctrlReconGroup || isctrlMatchingRules || isctrlPBAccountMapping || ismultiSelectDropDown)
                {
                    _isUnSavedChanges = true;
                }
                if (_isUnSavedChanges)
                {
                    _isUnSavedChanges = false;
                    return true;
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
            return _isUnSavedChanges;
        }

        /// <summary>
        /// Using this method we bind a custom user control.
        /// This user control allows multi select drop down.
        /// </summary>
        private void BindAssetCategoryCombo()
        {
            try
            {
                if (multiSelectDropDown1.GetNoOfTotalItems() <= 0)
                {
                    Dictionary<int, string> dictAssets = new Dictionary<int, string>();
                    dictAssets = CommonDataCache.CachedDataManager.GetInstance.GetAllAssets();

                    //Remove cash from asset collection
                    int cashAssetId = int.MinValue;
                    foreach (KeyValuePair<int, string> kvp in dictAssets)
                    {
                        if (kvp.Value.Equals("Cash"))
                        {
                            cashAssetId = kvp.Key;
                        }
                    }
                    if (dictAssets.ContainsKey(cashAssetId))
                        dictAssets.Remove(cashAssetId);

                    Dictionary<int, string> bindableAssets = new Dictionary<int, string>();
                    foreach (KeyValuePair<int, string> kvp in dictAssets)
                    {
                        bindableAssets.Add(kvp.Key, kvp.Value);
                    }
                    #region Handling Equity Swaps
                    //CHMW-1869	[Recon] Equity Swaps asset class is not available in recon template
                    bindableAssets.Add(500, "EquitySwap");
                    #endregion
                    //PranaReleaseViewType pranaReleaseViewType = CachedDataManager.GetInstance.GetPranaReleaseViewType();
                    //CHMW-2783
                    //if (pranaReleaseViewType == PranaReleaseViewType.CHMiddleWare)
                    //{
                    multiSelectDropDown1.SetManualTheme(CustomThemeHelper.ApplyTheme);
                    //}
                    //add Assets to the check list default value will be unchecked
                    multiSelectDropDown1.AddItemsToTheCheckList(bindableAssets, CheckState.Unchecked);

                    //adjust checklistbox width according to the longest Asset Name
                    multiSelectDropDown1.AdjustCheckListBoxWidth();
                    multiSelectDropDown1.TitleText = "Asset";
                    multiSelectDropDown1.SetTitleText(0);
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

        private void ctrlReconGeneralInfo_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    BindAssetCategoryCombo();
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
                        CustomThemeHelper.SetThemeProperties(ctrlReconGroupByColumns1, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
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

        private void SetButtonsColor()
        {
            try
            {
                btnSetCustomColumns.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSetCustomColumns.ForeColor = System.Drawing.Color.White;
                btnSetCustomColumns.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSetCustomColumns.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSetCustomColumns.UseAppStyling = false;
                btnSetCustomColumns.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// change the prefrence in recontemplate  as value in third party file combo value changes
        /// cmbValueChanged event was not working here so AfterCloseUp event is used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbThirdPartyFormat_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                if (cmbThirdPartyFormat.SelectedRow != null && _reconTemplate != null)
                {

                    if (ctrlMatchingRules1.IsUnsavedChanges())
                    {
                        //DialogResult result = MessageBox.Show("There are some unsaved changes, Do you want to save?", "Reconciliation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                    }
                    //TODO: Update recon xslt path when user make any changes from admin
                    //if (_template.FormatName != cmbThirdPartyFormat.SelectedRow.ToolTipText)
                    {
                        string formatName = cmbThirdPartyFormat.SelectedRow.Cells["value"].Value.ToString();
                        if (ReconPreferences.DictRunUpload != null && ReconPreferences.DictRunUpload.Keys.Contains(formatName))
                        {
                            RunUpload runUpload = ReconPreferences.DictRunUpload[formatName];
                            if (runUpload == null)
                            {
                                return;
                            }
                            _isUnSavedChanges = true;
                            _reconTemplate.FormatName = cmbThirdPartyFormat.SelectedRow.ToolTipText;
                            _reconTemplate.XsltPath = runUpload.DataSourceXSLT;
                            #region Set Sp Name
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1079
                            _reconTemplate.SpName = runUpload.SPName;
                            if (string.IsNullOrEmpty(_reconTemplate.SpName)
                                //check if dataset exist
                                && _reconTemplate.DsMatchingRules != null
                                //check if tabel exist
                                && _reconTemplate.DsMatchingRules.Tables.Contains("Rule")
                                //Check the row count
                                && _reconTemplate.DsMatchingRules.Tables["Rule"].Select("Name='" + _reconTemplate.ReconType + "'").Length > 0)
                            {
                                _reconTemplate.SpName = _reconTemplate.DsMatchingRules.Tables["Rule"].Select("Name='" + _reconTemplate.ReconType + "'").First()["SP"].ToString();
                            }
                            #endregion
                            ultraToolTipInfo1.ToolTipText = cmbThirdPartyFormat.Text;
                            ultraToolTipManager1.SetUltraToolTip(cmbThirdPartyFormat, ultraToolTipInfo1);
                            #region update matching rules according to selected xslt file
                            string reconXSLTPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString() + @"\" + _reconTemplate.XsltPath;
                            if (File.Exists(reconXSLTPath))
                            {
                                //_reconTemplate.VisibleRules = XSLTReader.readNodesUnderGivenNodeName(reconXSLTPath, "PositionMaster");

                                ctrlMatchingRules1.LoadMatchingRules(_reconTemplate);
                            }
                            else
                            {
                                MessageBox.Show("XSLT file does not exist", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            #endregion
                            #region update account filter

                            DialogResult result = MessageBox.Show("Do you want to reload account filter from batch settings?", "Reconciliation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (DialogResult.Yes == result)
                            {
                                _reconTemplate.ReconFilters.DictAccounts.Clear();
                                _reconTemplate.ReconFilters.DictThirParty.Clear();
                                foreach (int accountID in runUpload.LstAccountID)
                                {
                                    _reconTemplate.ReconFilters.DictAccounts.Add(accountID, CachedDataManager.GetInstance.GetAccountText(accountID));
                                }
                                _reconTemplate.ReconFilters.DictThirParty.Add(runUpload.DataSourceNameIDValue.ID, runUpload.DataSourceNameIDValue.ShortName);
                                ctrlPBAccountMapping1.setTemplate(_reconTemplate);
                            }
                            #endregion
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
        internal void InitializeControls()
        {
            try
            {
                ctrlReconGroupByColumns1.InitializeReconGroupByColumns();
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

        // Added by Ankit Gupta on 29 Sep, 2014.
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1405
        internal void LoadMatchingRules(ReconTemplate template)
        {

            try
            {
                ctrlMatchingRules1.LoadMatchingRules(template);
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

        private void btnSetCustomColumns_Click(object sender, EventArgs e)
        {
            try
            {
                if (_reconTemplate == null)
                {
                    MessageBox.Show("Please select a template.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                if (string.IsNullOrEmpty(_reconTemplate.XsltPath))
                {
                    MessageBox.Show("Please select XSLT for the template.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                frmCustomColumn frm = new frmCustomColumn();
                frm.LoadData(_reconTemplate);
                frm.ShowDialog();
                ctrlMatchingRules1.LoadMatchingRules(_reconTemplate);
                UpdateMasterColumns();
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

        private void UpdateMasterColumns()
        {
            try
            {
                List<string> NirvanaColumns = new List<string>();
                List<string> PBColumns = new List<string>();
                //update checked new master columns
                foreach (DataRow NirvanaRow in _reconTemplate.DsMasterColumns.Tables[0].Rows)
                {
                    if (!NirvanaColumns.Contains(NirvanaRow[ReconConstants.COLUMN_Name].ToString()))
                        NirvanaColumns.Add(NirvanaRow[ReconConstants.COLUMN_Name].ToString());
                }
                //add selected columns for PBTable
                foreach (DataRow PBRow in _reconTemplate.DsMasterColumns.Tables[1].Rows)
                {
                    if (!PBColumns.Contains(PBRow[ReconConstants.COLUMN_Name].ToString()))
                        PBColumns.Add(PBRow[ReconConstants.COLUMN_Name].ToString());
                }
                ReconUtilities.UpdateExceptionReportLayout(_reconTemplate, NirvanaColumns, PBColumns, new List<string>());

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

        private void cmbRunByDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbRunByDate.Value != null && _reconTemplate.ReconDateType.ToString() != cmbRunByDate.Value.ToString())
                {
                    _isUnSavedChanges = true;
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
    }
}
