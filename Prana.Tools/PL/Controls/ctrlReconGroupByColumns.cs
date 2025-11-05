using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Tools.PL.Controls
{
    public partial class ctrlReconGroupByColumns : UserControl
    {
        private bool _isUnSavedChanges = false;
        private ReconTemplate _reconTemplate = null;

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlReconGroupByColumns()
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

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public void InitializeReconGroupByColumns()
        {
            try
            {
                lblAsset.Visible = false;
                ultraGroupSymbology.Visible = false;
                cmbAssets.Visible = false;
                BindAssetCombo();
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
        //modified by: sachin mishra 28 jan 2015
        //Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void BindAssetCombo()
        {
            try
            {
                List<EnumerationValue> listValues = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(AssetCategory));
                listValues.RemoveAll(delegate (EnumerationValue value)
                {
                    if (value.DisplayText.Equals(AssetCategory.Forex.ToString()) || value.DisplayText.Equals(AssetCategory.Cash.ToString())
                        || value.DisplayText.Equals(AssetCategory.Option.ToString()) || value.DisplayText.Equals(AssetCategory.None.ToString()))
                    {
                        return true;
                    }

                    return false;
                });
                cmbAssets.DataSource = null;
                cmbAssets.DataSource = listValues;
                cmbAssets.DisplayMember = "DisplayText";
                cmbAssets.ValueMember = "Value";
                cmbAssets.DataBind();
                cmbAssets.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                //cmbAssets.Value = -1;
                //DataTable dtAssets = ReconPrefManager.GetAssets();

                //cmbAssets.DataSource = dtAssets;
                //cmbAssets.DisplayMember = "Data";
                //cmbAssets.ValueMember = "Value";
                //cmbAssets.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                //cmbAssets.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="template"></param>
        internal void LoadGroupCriteria(ReconTemplate template)
        {
            try
            {
                _reconTemplate = template;
                //ctrlReconTemplate ctrlReconTemplateObj = new ctrlReconTemplate();
                UpdateGroupingDictionary(template);




                // checkedGroup.DataSource = groupingCriteria.DictGroupingCriteria;
                DataTable dtGroupingCriteria = ReconPrefManager.GetGroupingCriteria(template);
                checkedGroup.DataSource = dtGroupingCriteria;
                checkedGroup.DisplayMember = "Key";
                checkedGroup.ValueMember = "Value";
                //Set value in select all checkbox
                if (!template.GroupingCrieria.DictGroupingCriteria.ContainsValue(false))
                {
                    chkSelectAll.CheckState = CheckState.Checked;
                }
                else
                {
                    chkSelectAll.CheckState = CheckState.Unchecked;
                }

                int i = 0;
                DataColumn col = dtGroupingCriteria.Columns["Value"];

                foreach (DataRow row in dtGroupingCriteria.Rows)
                {
                    checkedGroup.SetItemChecked(i, Convert.ToBoolean(row[col]));
                    i++;
                }

                #region commented code

                //foreach(KeyValuePair<string, bool> entry in groupingCriteria._dictGroupingCriteria)
                //{
                //    //checkedGroup.DisplayMember = entry.Key;
                //   // checkedGroup.ValueMember = (entry.Value).ToString();
                //    checkedGroup.SetItemChecked(i, entry.Value);
                //    i++;
                //}

                //checkedGroup.SetItemChecked(0, template.GroupingCrieria.IsGroupByAccount );
                //checkedGroup.SetItemChecked(1, template.GroupingCrieria.IsGroupBySymbol);
                //checkedGroup.SetItemChecked(2, template.GroupingCrieria.IsGroupBySide);
                //checkedGroup.SetItemChecked(3, template.GroupingCrieria.IsGroupByBroker);
                //checkedGroup.SetItemChecked(4, template.GroupingCrieria.IsGroupbyMasterFund);

                //checkBoxAccount.Checked = template.GroupingCrieria.IsGroupByAccount;
                //checkboxSide.Checked = template.GroupingCrieria.IsGroupBySide;
                //checkBoxSymbol.Checked = template.GroupingCrieria.IsGroupBySymbol;
                //chkBoxMasterFund.Checked = template.GroupingCrieria.IsGroupbyMasterFund;
                // chkBoxBroker.Checked = template.GroupingCrieria.IsGroupByBroker;

                //if (checkBoxSymbol.Checked.Equals(true))

                #endregion commented code


                if (checkedGroup.GetItemCheckState(1) == CheckState.Checked)
                {

                    checkedGroup.SetItemChecked(1, true);
                    lblAsset.Visible = true;
                    ultraGroupSymbology.Visible = true;
                    cmbAssets.Visible = true;

                    if (template.GroupingCrieria.DictGroupingSymbology.Count > 0)
                    {
                        foreach (KeyValuePair<int, SymbologyCodesForRecon> kvp in template.GroupingCrieria.DictGroupingSymbology)
                        {
                            cmbAssets.Value = kvp.Key;

                            if (template.GroupingCrieria.DictGroupingSymbology[kvp.Key].Equals(SymbologyCodesForRecon.Bloomberg))
                            {
                                rbBloomberg.Checked = true;
                            }
                            if (template.GroupingCrieria.DictGroupingSymbology[kvp.Key].Equals(SymbologyCodesForRecon.IDCOOption))
                            {
                                rbIDCO.Checked = true;
                            }
                            if (template.GroupingCrieria.DictGroupingSymbology[kvp.Key].Equals(SymbologyCodesForRecon.SEDOL))
                            {
                                rbSedolSymbol.Checked = true;
                            }
                            if (template.GroupingCrieria.DictGroupingSymbology[kvp.Key].Equals(SymbologyCodesForRecon.OSIOption))
                            {
                                rbOSI.Checked = true;
                            }
                            if (template.GroupingCrieria.DictGroupingSymbology[kvp.Key].Equals(SymbologyCodesForRecon.Ticker))
                            {
                                rbTicker.Checked = true;
                            }
                            break;
                        }
                    }

                }
                else
                {
                    lblAsset.Visible = false;
                    ultraGroupSymbology.Visible = false;
                    cmbAssets.Visible = false;
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
        /// Created by: Pranay Deep , 07 Sept 2015
        /// Purpose   : http://jira.nirvanasolutions.com:8080/browse/PRANA-10644
        /// Work      : Update grouping criteria from the database
        /// </summary>
        /// <param name="template"></param>
        public void UpdateGroupingDictionary(ReconTemplate template)
        {
            //Modified By: Pranay Deep
            //http://jira.nirvanasolutions.com:8080/browse/PRANA-10910 and http://jira.nirvanasolutions.com:8080/browse/PRANA-10911
            DataSet dsReconGrouping = new DataSet();
            dsReconGrouping = Prana.ReconciliationNew.DatabaseManager.GetInstance().GetReconGroupingCriteria();
            DataTable dtRecReconGrouping = dsReconGrouping.Tables[0];
            dtRecReconGrouping.TableName = "Grouping";
            string data = null;

            #region Commented code

            //if (((template.ReconType).ToString()).Equals("Transaction"))
            //{
            //   // data = (dsReconGrouping.Tables[0].Rows[0][1]).ToString();
            //    data = (from Grouping in dtRecReconGrouping.AsEnumerable()
            //           where Grouping.Field<string>("ReconTypeName") == "Transaction"
            //           select Grouping.Field<string>("GroupingColumns")).First<string>();
            //}
            //else if (((template.ReconType).ToString()).Equals("Position"))
            //{
            //   // data = (dsReconGrouping.Tables[0].Rows[1][1]).ToString();
            //    data = (from Grouping in dtRecReconGrouping.AsEnumerable()
            //            where Grouping.Field<string>("ReconTypeName") == "Position"
            //            select Grouping.Field<string>("GroupingColumns")).First<string>();
            //}
            //else if (((template.ReconType).ToString()).Equals("PNL"))
            //{
            //   // data = (dsReconGrouping.Tables[0].Rows[2][1]).ToString();
            //    data = (from Grouping in dtRecReconGrouping.AsEnumerable()
            //            where Grouping.Field<string>("ReconTypeName") == "PNL"
            //            select Grouping.Field<string>("GroupingColumns")).First<string>();

            //}

            #endregion Commented code

            data = (from Grouping in dtRecReconGrouping.AsEnumerable()
                        //TODO: Create constant
                    where Grouping.Field<string>(ReconConstants.COLUMN_ReconTypeName) == ((template.ReconType).ToString())
                    select Grouping.Field<string>(ReconConstants.COLUMN_GroupingColumns)).First<string>();

            List<string> ListGroupingCriteria = data.Split(Seperators.SEPERATOR_14).ToList();

            foreach (string item in ListGroupingCriteria)
            {
                if (!template.GroupingCrieria.DictGroupingCriteria.ContainsKey(item))
                {
                    template.GroupingCrieria.DictGroupingCriteria.Add(item, false);
                }
            }
        }

        /// <summary>
        /// Modified by: Pranay Deep , 07 Sept 2015
        /// Purpose: http://jira.nirvanasolutions.com:8080/browse/PRANA-10644
        /// </summary>
        /// <param name="template"></param>
        internal void UpdateGroupCriteria(ReconTemplate template)
        {
            try
            {
                for (int index = 0; index < template.GroupingCrieria.DictGroupingCriteria.Count; index++)
                {
                    template.GroupingCrieria.DictGroupingCriteria[template.GroupingCrieria.DictGroupingCriteria.ElementAt(index).Key] = (checkedGroup.GetItemCheckState(index) == CheckState.Checked) ? Convert.ToBoolean(1) : Convert.ToBoolean(0);
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAssets_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_reconTemplate == null)
                {
                    return;
                }
                rbTicker.Enabled = true;
                rbBloomberg.Enabled = false;
                rbOSI.Enabled = false;
                rbIDCO.Enabled = false;
                rbSedolSymbol.Enabled = false;
                rbCUSIP.Enabled = false;

                int key = Convert.ToInt16(cmbAssets.Value);

                if (key > 0)
                {
                    AssetCategory category = (AssetCategory)key;

                    AssetCategory baseAsset = Mapper.GetBaseAssetCategory(category);

                    switch (baseAsset)
                    {
                        case AssetCategory.Equity:
                        case AssetCategory.PrivateEquity:
                        case AssetCategory.CreditDefaultSwap:
                        case AssetCategory.Indices:
                            rbBloomberg.Enabled = true;
                            rbCUSIP.Enabled = true;
                            rbSedolSymbol.Enabled = true;
                            break;

                        case AssetCategory.FixedIncome:
                        case AssetCategory.ConvertibleBond:
                            rbCUSIP.Enabled = true;
                            break;

                        case AssetCategory.Option:

                            rbOSI.Enabled = true;
                            rbIDCO.Enabled = true;
                            break;

                        default:
                            rbTicker.Checked = true;
                            break;
                    }

                    if (_reconTemplate.GroupingCrieria.DictGroupingSymbology.ContainsKey(key))
                    {
                        if (_reconTemplate.GroupingCrieria.DictGroupingSymbology[key].Equals(SymbologyCodesForRecon.Bloomberg))
                        {
                            rbBloomberg.Checked = true;
                        }
                        if (_reconTemplate.GroupingCrieria.DictGroupingSymbology[key].Equals(SymbologyCodesForRecon.IDCOOption))
                        {
                            rbIDCO.Checked = true;
                        }
                        if (_reconTemplate.GroupingCrieria.DictGroupingSymbology[key].Equals(SymbologyCodesForRecon.OSIOption))
                        {
                            rbOSI.Checked = true;
                        }
                        if (_reconTemplate.GroupingCrieria.DictGroupingSymbology[key].Equals(SymbologyCodesForRecon.Ticker))
                        {
                            rbTicker.Checked = true;
                        }
                        if (_reconTemplate.GroupingCrieria.DictGroupingSymbology[key].Equals(SymbologyCodesForRecon.SEDOL))
                        {
                            rbSedolSymbol.Checked = true;
                        }
                        if (_reconTemplate.GroupingCrieria.DictGroupingSymbology[key].Equals(SymbologyCodesForRecon.CUSIP))
                        {
                            rbCUSIP.Checked = true;
                        }
                    }
                    else
                    {
                        SymbologyCodesForRecon Symbology = SymbologyCodesForRecon.Ticker;
                        _reconTemplate.GroupingCrieria.DictGroupingSymbology.Add(key, Symbology);
                        rbTicker.Checked = true;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbTicker_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetSymbologyCodeForAsset();
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

        //int count = 1;
        ////Modified by: Pranay
        //private void checkedGroup_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        #region Commented code
        //        //  #region Account And Symbol grouping will be parallel, Either both are selected or none
        //        //CHMW-2306	[recon] Account is not coming multiple if I use only symbol grouping for recon
        //        //UltraCheckEditor editor = sender as UltraCheckEditor;
        //        //if (editor != null)
        //        //{
        //        //    if (editor.Text == checkBoxAccount.Text && editor.Checked != checkBoxSymbol.Checked)
        //        //    {
        //        //        checkBoxSymbol.Checked = editor.Checked;
        //        //    }
        //        //    else if (editor.Text == checkBoxSymbol.Text && editor.Checked != checkBoxAccount.Checked)
        //        //    {
        //        //        checkBoxAccount.Checked = editor.Checked;
        //        //    }
        //        //}
        //        // #endregion
        //        //checkedGroup.GetItemCheckState
        //        // MessageBox.Show((checkedGroup.SelectedValue).ToString());
        //        #endregion


        //        //checkedGroup.SelectedIndexChanged -= new System.EventHandler(this.checkedGroup_SelectedIndexChanged);
        //        if (checkedGroup.SelectedIndex == 0)//account
        //        {
        //            count++;
        //            ultraLabel1.Text = count.ToString();
        //           // bool b = true;
        //           // if (b)//checkedGroup(0))
        //            {
        //                count++;
        //                ultraLabel1.Text = count.ToString();

        //                lblAsset.Visible = true;
        //                ultraGroupSymbology.Visible = true;
        //                cmbAssets.Visible = true;
        //                checkedGroup.SetItemChecked(1, true);
        //            }
        //           // else
        //            {

        //                lblAsset.Visible = false;
        //                ultraGroupSymbology.Visible = false;
        //                cmbAssets.Visible = false;
        //                checkedGroup.SetItemChecked(1, false);
        //            }
        //        }
        //        //else if ((checkedGroup.Text).ToString() == "Symbol")//symbol
        //        //{
        //        //    if (checkedGroup.GetItemCheckState(1) == CheckState.Checked)
        //        //    {
        //        //        checkedGroup.SetItemChecked(0, true);
        //        //        lblAsset.Visible = true;
        //        //        ultraGroupSymbology.Visible = true;
        //        //        cmbAssets.Visible = true;
        //        //    }
        //        //    else
        //        //    {
        //        //        checkedGroup.SetItemChecked(0, false);
        //        //        lblAsset.Visible = false;
        //        //        ultraGroupSymbology.Visible = false;
        //        //        cmbAssets.Visible = false;
        //        //    }
        //        //}
        //        //checkedGroup.SelectedIndexChanged += new System.EventHandler(this.checkedGroup_SelectedIndexChanged);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}



        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void checkBoxSymbol_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //checkedGroup.DataSource = ctrlReconTemplate.GroupTable2;
        //      //  checkedGroup.DisplayMember = "Data";
        //      //  checkedGroup.ValueMember = "Value";

        //        #region Account And Symbol grouping will be parallel, Either both are selected or none
        //        //CHMW-2306	[recon] Account is not coming multiple if I use only symbol grouping for recon
        //        UltraCheckEditor editor = sender as UltraCheckEditor;
        //        if (editor != null)
        //        {
        //            if (editor.Text == checkBoxAccount.Text && editor.Checked != checkBoxSymbol.Checked)
        //            {
        //                checkBoxSymbol.Checked = editor.Checked;
        //            }
        //            else if (editor.Text == checkBoxSymbol.Text && editor.Checked != checkBoxAccount.Checked)
        //            {
        //                checkBoxAccount.Checked = editor.Checked;
        //            }
        //        }
        //        #endregion

        //        if (checkBoxSymbol.Checked)
        //        {
        //            lblAsset.Visible = true;
        //            ultraGroupSymbology.Visible = true;
        //            cmbAssets.Visible = true;
        //        }
        //        else
        //        {
        //            lblAsset.Visible = false;
        //            ultraGroupSymbology.Visible = false;
        //            cmbAssets.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}
        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void SetSymbologyCodeForAsset()
        {
            try
            {
                // _isUnSavedChanges = true;
                SymbologyCodesForRecon Symbology = SymbologyCodesForRecon.Ticker;
                if (rbBloomberg.Checked)
                {
                    Symbology = SymbologyCodesForRecon.Bloomberg;
                }
                if (rbOSI.Checked)
                {
                    Symbology = SymbologyCodesForRecon.OSIOption;
                }
                if (rbIDCO.Checked)
                {
                    Symbology = SymbologyCodesForRecon.IDCOOption;
                }

                if (rbSedolSymbol.Checked)
                {
                    Symbology = SymbologyCodesForRecon.SEDOL;
                }

                if (rbCUSIP.Checked)
                {
                    Symbology = SymbologyCodesForRecon.CUSIP;
                }
                int key = Convert.ToInt16(cmbAssets.Value);
                if (key > 0)
                {
                    if (_reconTemplate.GroupingCrieria.DictGroupingSymbology.ContainsKey(key))
                    {
                        SymbologyCodesForRecon symbologyPrevious = _reconTemplate.GroupingCrieria.DictGroupingSymbology[key];

                        if ((int)symbologyPrevious != (int)Symbology)
                        {
                            _isUnSavedChanges = true;
                        }

                        _reconTemplate.GroupingCrieria.DictGroupingSymbology[key] = Symbology;
                    }
                    ReconPrefManager.ReconPreferences.UpdateTemplates(_reconTemplate.TemplateName, _reconTemplate);
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
        /// modified by: Pranay Deep, 09 sept 2015
        /// purpose: handle the _isUnSavedChanges
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        internal bool IsGroupingChanged(ReconTemplate template)
        {
            try
            {
                if (!_isUnSavedChanges)
                {
                    //if (template.GroupingCrieria.IsGroupByAccount != ((checkedGroup.GetItemCheckState(0) == CheckState.Checked) ? Convert.ToBoolean(1) : Convert.ToBoolean(0)) || template.GroupingCrieria.IsGroupBySide != (checkedGroup.GetItemCheckState(2) == CheckState.Checked) ? Convert.ToBoolean(1) : Convert.ToBoolean(0)
                    //    || template.GroupingCrieria.IsGroupBySymbol != (checkedGroup.GetItemCheckState(1) == CheckState.Checked) ? Convert.ToBoolean(1) : Convert.ToBoolean(0) || template.GroupingCrieria.IsGroupByBroker != (checkedGroup.GetItemCheckState(3) == CheckState.Checked) ? Convert.ToBoolean(1) : Convert.ToBoolean(0) || template.GroupingCrieria.IsGroupbyMasterFund != (checkedGroup.GetItemCheckState(4) == CheckState.Checked) ? Convert.ToBoolean(1) : Convert.ToBoolean(0))
                    //{
                    //    _isUnSavedChanges = true;
                    //}

                    for (int index = 0; index < template.GroupingCrieria.DictGroupingCriteria.Count; index++)
                    {
                        if (template.GroupingCrieria.DictGroupingCriteria[template.GroupingCrieria.DictGroupingCriteria.ElementAt(index).Key] != (checkedGroup.GetItemCheckState(index) == CheckState.Checked) ? Convert.ToBoolean(1) : Convert.ToBoolean(0))
                        {
                            _isUnSavedChanges = true;
                        }
                    }


                }
                if (_isUnSavedChanges)
                {
                    _isUnSavedChanges = false;
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
            return _isUnSavedChanges;
        }


        /// <summary>
        /// Created By: Pranay Deep 13 Oct 2015
        /// This method is for provinding "Select All" functionality in the grouping 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                for (int i = 0; i < checkedGroup.Items.Count; i++)
                {
                    if (chkSelectAll.CheckState == CheckState.Checked)
                    {
                        checkedGroup.SetItemChecked(i, true);
                        lblAsset.Visible = true;
                        ultraGroupSymbology.Visible = true;
                        cmbAssets.Visible = true;
                    }
                    else
                    {
                        checkedGroup.SetItemChecked(i, false);
                        lblAsset.Visible = false;
                        ultraGroupSymbology.Visible = false;
                        cmbAssets.Visible = false;
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
        /// Created By: Pranay Deep 09 sept 2015
        /// This method is for handling Assest Visiblity when Symbol is checked 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkedGroup.Text.Equals("Symbol"))
                {
                    if (!checkedGroup.GetItemChecked(1) == true)
                    {
                        lblAsset.Visible = true;
                        ultraGroupSymbology.Visible = true;
                        cmbAssets.Visible = true;
                    }
                    else
                    {
                        lblAsset.Visible = false;
                        ultraGroupSymbology.Visible = false;
                        cmbAssets.Visible = false;
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
        /// "chkSelectAll.CheckedChanged" this event has been unwired here
        /// to handle http://jira.nirvanasolutions.com:8080/browse/PRANA-11598
        /// [Recon] "Select All" checkbox must be checked if all the items in the "checked list box" as checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedGroup_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if ((checkedGroup.CheckedItems.Count) == (checkedGroup.Items.Count))
                {
                    chkSelectAll.CheckState = CheckState.Checked;
                }
                else if (chkSelectAll.CheckState == CheckState.Checked)
                {

                    {
                        chkSelectAll.CheckedChanged -= new System.EventHandler(this.chkSelectAll_CheckedChanged);
                        chkSelectAll.CheckState = CheckState.Unchecked;
                        chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
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
    }
}
