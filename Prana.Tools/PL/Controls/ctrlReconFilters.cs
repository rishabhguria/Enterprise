using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlReconFilters : UserControl
    {
        #region Private Member Variable

        private bool _isControlInitialized = false;

        private bool _isUnsavedChanges = false;

        #endregion

        #region Private Methods
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)        /// </summary>
        private void LoadBlankReconFilters()
        {
            try
            {
                cbAssets.Checked = true;
                cbAUEC.Checked = true;
                cbBrokers.Checked = true;
                cbAccounts.Checked = true;
                cbMasterFunds.Checked = true;
                cbPrimeBroker.Checked = true;


                for (int j = 0; j < checkedAssets.Items.Count; j++)
                {
                    checkedAssets.SetItemChecked(j, false);
                    checkedAssets.Enabled = false;
                }

                for (int j = 0; j < checkedListAUECs.Items.Count; j++)
                {
                    checkedListAUECs.SetItemChecked(j, false);
                    checkedListAUECs.Enabled = false;
                }

                for (int j = 0; j < checkedBrokers.Items.Count; j++)
                {
                    checkedBrokers.SetItemChecked(j, false);
                    checkedBrokers.Enabled = false;
                }

                for (int j = 0; j < checkedListAccounts.Items.Count; j++)
                {
                    checkedListAccounts.SetItemChecked(j, false);
                    checkedListAccounts.Enabled = false;
                }

                for (int j = 0; j < checkedListMasterFunds.Items.Count; j++)
                {
                    checkedListMasterFunds.SetItemChecked(j, false);
                    checkedListMasterFunds.Enabled = false;
                }

                for (int j = 0; j < checkedListPrimeBrokers.Items.Count; j++)
                {
                    checkedListPrimeBrokers.SetItemChecked(j, false);
                    checkedListPrimeBrokers.Enabled = false;
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

        private void GetReconFilters()
        {
            try
            {
                DataTable dtAssets = ReconPrefManager.GetAssets();
                checkedAssets.DataSource = dtAssets;
                checkedAssets.DisplayMember = "Data";
                checkedAssets.ValueMember = "Value";

                DataTable dtAUECs = ReconPrefManager.GetAUECs();
                checkedListAUECs.DataSource = dtAUECs;
                checkedListAUECs.DisplayMember = "Data";
                checkedListAUECs.ValueMember = "Value";

                DataTable dtAccounts = ReconPrefManager.GetAccounts();
                checkedListAccounts.DataSource = dtAccounts;
                checkedListAccounts.DisplayMember = "Data";
                checkedListAccounts.ValueMember = "Value";

                DataTable dtCounterParties = ReconPrefManager.GetBrokers();
                checkedBrokers.DataSource = dtCounterParties;
                checkedBrokers.DisplayMember = "Data";
                checkedBrokers.ValueMember = "Value";

                DataTable dtMasterFunds = ReconPrefManager.GetMasterFunds();
                checkedListMasterFunds.DataSource = dtMasterFunds;
                checkedListMasterFunds.DisplayMember = "Data";
                checkedListMasterFunds.ValueMember = "Value";

                DataTable dtPrimeBrokers = ReconPrefManager.GetPrimeBrokers();
                checkedListPrimeBrokers.DataSource = dtPrimeBrokers;
                checkedListPrimeBrokers.DisplayMember = "Data";
                checkedListPrimeBrokers.ValueMember = "Value";
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
        private void CheckIFUnsavedChanges(ReconTemplate template)
        {
            try
            {

                if (template.ReconFilters.DictAssets.Count != checkedAssets.CheckedItems.Count || template.ReconFilters.DictAUECs.Count != checkedListAUECs.CheckedItems.Count
                   || template.ReconFilters.DictBrokers.Count != checkedBrokers.CheckedItems.Count || template.ReconFilters.DictAccounts.Count != checkedListAccounts.CheckedItems.Count
                   || template.ReconFilters.DictMasterFunds.Count != checkedListMasterFunds.CheckedItems.Count || template.ReconFilters.DictPrimeBrokers.Count != checkedListPrimeBrokers.CheckedItems.Count)
                {
                    _isUnsavedChanges = true;
                    return;
                }
                //if(template.GroupingCrieria.IsGroupByAccount != cb

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
        /// <param name="checkboxName"></param>
        /// <param name="checkOrUncheck"></param>
        private void CheckUncheckAll(string checkboxName, bool checkOrUncheck)
        {
            try
            {

                if (checkboxName.Equals(cbAssets.Tag.ToString()))
                {
                    if (checkOrUncheck)
                    {
                        checkedAssets.Enabled = true;
                        for (int j = 0; j < checkedAssets.Items.Count; j++)
                        {
                            checkedAssets.SetItemChecked(j, !checkOrUncheck);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < checkedAssets.Items.Count; j++)
                        {
                            checkedAssets.SetItemChecked(j, checkOrUncheck);
                        }
                        checkedAssets.Enabled = false;
                    }
                }
                if (checkboxName.Equals(cbAUEC.Tag.ToString()))
                {
                    if (checkOrUncheck)
                    {
                        checkedListAUECs.Enabled = true;
                        for (int j = 0; j < checkedListAUECs.Items.Count; j++)
                        {
                            checkedListAUECs.SetItemChecked(j, !checkOrUncheck);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < checkedListAUECs.Items.Count; j++)
                        {
                            checkedListAUECs.SetItemChecked(j, checkOrUncheck);
                        }
                        checkedListAUECs.Enabled = false;
                    }
                }
                if (checkboxName.Equals(cbBrokers.Tag.ToString()))
                {
                    if (checkOrUncheck)
                    {
                        checkedBrokers.Enabled = true;
                        for (int j = 0; j < checkedBrokers.Items.Count; j++)
                        {
                            checkedBrokers.SetItemChecked(j, !checkOrUncheck);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < checkedBrokers.Items.Count; j++)
                        {
                            checkedBrokers.SetItemChecked(j, checkOrUncheck);
                        }
                        checkedBrokers.Enabled = false;
                    }
                }
                if (checkboxName.Equals(cbAccounts.Tag.ToString()))
                {
                    if (checkOrUncheck)
                    {
                        checkedListAccounts.Enabled = true;
                        for (int j = 0; j < checkedListAccounts.Items.Count; j++)
                        {
                            checkedListAccounts.SetItemChecked(j, !checkOrUncheck);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < checkedListAccounts.Items.Count; j++)
                        {
                            checkedListAccounts.SetItemChecked(j, checkOrUncheck);
                        }
                        checkedListAccounts.Enabled = false;
                    }
                }
                if (checkboxName.Equals(cbMasterFunds.Tag.ToString()))
                {
                    if (checkOrUncheck)
                    {
                        checkedListMasterFunds.Enabled = true;
                        for (int j = 0; j < checkedListMasterFunds.Items.Count; j++)
                        {
                            checkedListMasterFunds.SetItemChecked(j, !checkOrUncheck);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < checkedListMasterFunds.Items.Count; j++)
                        {
                            checkedListMasterFunds.SetItemChecked(j, checkOrUncheck);
                        }
                        checkedListMasterFunds.Enabled = false;
                    }
                }

                if (checkboxName.Equals(cbPrimeBroker.Tag.ToString()))
                {

                    if (checkOrUncheck)
                    {
                        checkedListPrimeBrokers.Enabled = true;
                        for (int j = 0; j < checkedListPrimeBrokers.Items.Count; j++)
                        {
                            checkedListPrimeBrokers.SetItemChecked(j, !checkOrUncheck);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < checkedListPrimeBrokers.Items.Count; j++)
                        {
                            checkedListPrimeBrokers.SetItemChecked(j, checkOrUncheck);
                        }
                        checkedListPrimeBrokers.Enabled = false;
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



        #endregion

        #region Constructors
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlReconFilters()
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
        #endregion

        #region Events

        private void ctrlReconFilters_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.BackColor = System.Drawing.Color.FromArgb(209, 210, 212);

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

        #region Combo Events
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPrimeBroker_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string key = cbPrimeBroker.Tag.ToString();
                if (cbPrimeBroker.Checked)
                {
                    CheckUncheckAll(key, false);
                }
                else
                {
                    CheckUncheckAll(key, true);
                }
                //_isUnsavedChanges = true;
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
        private void cbAssets_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string key = cbAssets.Tag.ToString();
                if (cbAssets.Checked)
                {
                    CheckUncheckAll(key, false);
                }
                else
                {
                    CheckUncheckAll(key, true);
                }
                //_isUnsavedChanges = true;
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
        private void cbBrokers_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string key = cbBrokers.Tag.ToString();
                if (cbBrokers.Checked)
                {
                    CheckUncheckAll(key, false);
                }
                else
                {
                    CheckUncheckAll(key, true);
                }
                //_isUnsavedChanges = true;
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
        private void cbAccounts_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string key = cbAccounts.Tag.ToString();
                if (cbAccounts.Checked)
                {
                    CheckUncheckAll(key, false);
                }
                else
                {
                    CheckUncheckAll(key, true);
                }
                //_isUnsavedChanges = true;
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
        private void cbAUEC_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string key = cbAUEC.Tag.ToString();
                if (cbAUEC.Checked)
                {
                    CheckUncheckAll(key, false);
                }
                else
                {
                    CheckUncheckAll(key, true);
                }
                //_isUnsavedChanges = true;
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
        private void cbMasterFunds_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                string key = cbMasterFunds.Tag.ToString();
                if (cbMasterFunds.Checked)
                {
                    CheckUncheckAll(key, false);
                }
                else
                {
                    CheckUncheckAll(key, true);
                }
                //_isUnsavedChanges = true;
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

        #region CheckBox Events

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListPrimeBrokers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {

                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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
        private void checkedAssets_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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
        private void checkedBrokers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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
        private void checkedListAccounts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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
        private void checkedListAUECs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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
        private void checkedListMasterFunds_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {

                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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



        #endregion

        #region Internal Methods


        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="template"></param>
        internal void LoadReconFilters(ReconTemplate template)
        {
            try
            {
                //if (template.ReconFilters.ListAssets.Count == 0 && template.ReconFilters.ListAUECs.Count == 0 && template.ReconFilters.ListBrokers.Count == 0 && template.ReconFilters.ListAccounts.Count == 0 && template.ReconFilters.ListMasterFunds.Count == 0 && template.ReconFilters.ListPrimeBrokers.Count == 0)
                //{

                if (checkedAssets.Items.Count == 0 || checkedBrokers.Items.Count == 0 || checkedListAUECs.Items.Count == 0 || checkedListAccounts.Items.Count == 0 || checkedListMasterFunds.Items.Count == 0 || checkedListPrimeBrokers.Items.Count == 0)
                {
                    InitializeReconFiltersTab();

                }
                //else
                //{

                if (template.ReconFilters.DictAccounts.Count == 0 && template.ReconFilters.DictMasterFunds.Count == 0 && template.ReconFilters.DictPrimeBrokers.Count == 0
                    && template.ReconFilters.DictBrokers.Count == 0 && template.ReconFilters.DictAUECs.Count == 0 && template.ReconFilters.DictAssets.Count == 0)
                {
                    LoadBlankReconFilters();
                }
                //Narendra Kumar Jangir 2012/08/17
                //all filters are by default selected
                if (template.ReconFilters.DictAssets.Count == 0)
                {
                    cbAssets.Checked = true;
                    checkedAssets.Enabled = false;
                }
                else
                {
                    cbAssets.Checked = false;
                }
                for (int j = 0; j < checkedAssets.Items.Count; j++)
                {
                    int assetId = int.Parse(((DataRow)(((DataRowView)((checkedAssets.Items[j]))).Row)).ItemArray[0].ToString());
                    if (template.ReconFilters.DictAssets.ContainsKey(assetId))
                    {
                        checkedAssets.SetItemChecked(j, true);
                    }
                    else
                    {
                        checkedAssets.SetItemChecked(j, false);
                    }
                }

                if (template.ReconFilters.DictPrimeBrokers.Count == 0)
                {
                    cbPrimeBroker.Checked = true;
                    checkedListPrimeBrokers.Enabled = false;
                }
                else
                {
                    cbPrimeBroker.Checked = false;
                }
                for (int j = 0; j < checkedListPrimeBrokers.Items.Count; j++)
                {
                    int pbId = int.Parse(((DataRow)(((DataRowView)((checkedListPrimeBrokers.Items[j]))).Row)).ItemArray[0].ToString());
                    if (template.ReconFilters.DictPrimeBrokers.ContainsKey(pbId))
                    {
                        checkedListPrimeBrokers.SetItemChecked(j, true);
                    }
                    else
                    {
                        checkedListPrimeBrokers.SetItemChecked(j, false);
                    }
                }

                if (template.ReconFilters.DictMasterFunds.Count == 0)
                {
                    cbMasterFunds.Checked = true;
                    checkedListMasterFunds.Enabled = false;
                }
                else
                {
                    cbMasterFunds.Checked = false;
                }
                for (int j = 0; j < checkedListMasterFunds.Items.Count; j++)
                {
                    int masterFundId = int.Parse(((DataRow)(((DataRowView)((checkedListMasterFunds.Items[j]))).Row)).ItemArray[0].ToString());
                    if (template.ReconFilters.DictMasterFunds.ContainsKey(masterFundId))
                    {
                        checkedListMasterFunds.SetItemChecked(j, true);
                    }
                    else
                    {
                        checkedListMasterFunds.SetItemChecked(j, false);
                    }
                }

                if (template.ReconFilters.DictAccounts.Count == 0)
                {
                    cbAccounts.Checked = true;
                    checkedListAccounts.Enabled = false;
                }
                else
                {
                    cbAccounts.Checked = false;
                }
                for (int j = 0; j < checkedListAccounts.Items.Count; j++)
                {
                    int accountId = int.Parse(((DataRow)(((DataRowView)((checkedListAccounts.Items[j]))).Row)).ItemArray[0].ToString());
                    if (template.ReconFilters.DictAccounts.ContainsKey(accountId))
                    {
                        checkedListAccounts.SetItemChecked(j, true);
                    }
                    else
                    {
                        checkedListAccounts.SetItemChecked(j, false);
                    }
                }

                //if (template.ReconFilters.DictAccounts.Count == checkedListAccounts.Items.Count)
                //{
                //    cbAccounts.Checked = true;
                //}
                if (template.ReconFilters.DictBrokers.Count == 0)
                {
                    cbBrokers.Checked = true;
                    checkedBrokers.Enabled = false;
                }
                else
                {
                    cbBrokers.Checked = false;
                }
                for (int j = 0; j < checkedBrokers.Items.Count; j++)
                {
                    int brokerId = int.Parse(((DataRow)(((DataRowView)((checkedBrokers.Items[j]))).Row)).ItemArray[0].ToString());
                    if (template.ReconFilters.DictBrokers.ContainsKey(brokerId))
                    {
                        checkedBrokers.SetItemChecked(j, true);
                    }
                    else
                    {
                        checkedBrokers.SetItemChecked(j, false);
                    }
                }

                //if (template.ReconFilters.DictBrokers.Count == checkedBrokers.Items.Count)
                //{
                //    cbBrokers.Checked = true;
                //}

                if (template.ReconFilters.DictAUECs.Count == 0)
                {
                    cbAUEC.Checked = true;
                    checkedListAUECs.Enabled = false;
                }
                else
                {
                    cbAUEC.Checked = false;
                }
                for (int j = 0; j < checkedListAUECs.Items.Count; j++)
                {
                    int auecId = int.Parse(((DataRow)(((DataRowView)((checkedListAUECs.Items[j]))).Row)).ItemArray[0].ToString());
                    if (template.ReconFilters.DictAUECs.ContainsKey(auecId))
                    {
                        checkedListAUECs.SetItemChecked(j, true);
                    }
                    else
                    {
                        checkedListAUECs.SetItemChecked(j, false);
                    }
                }

                //if (template.ReconFilters.DictAUECs.Count == checkedListAUECs.Items.Count)
                //{
                //    cbAUEC.Checked = true;
                //}
                //   }
                //grdXSLTmapping.DataSource = template.DtXSLTMapping;
                //grdMatchingRules.DataSource = template.DtMatchingRules;
                _isControlInitialized = true;
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
        /// <param name="template"></param>
        internal void UpdateReconFilters(ReconTemplate template)
        {

            try
            {
                CheckIFUnsavedChanges(template);
                template.ReconFilters.DictAssets.Clear();
                template.ReconFilters.DictAUECs.Clear();
                template.ReconFilters.DictBrokers.Clear();
                template.ReconFilters.DictAccounts.Clear();
                template.ReconFilters.DictMasterFunds.Clear();
                template.ReconFilters.DictPrimeBrokers.Clear();

                for (int i = 0, count = checkedAssets.CheckedItems.Count; i < count; i++)
                {
                    int assetId = int.Parse(((DataRow)(((DataRowView)((checkedAssets.CheckedItems[i]))).Row)).ItemArray[0].ToString());
                    string assetText = ((DataRow)(((DataRowView)((checkedAssets.CheckedItems[i]))).Row)).ItemArray[1].ToString();
                    if (!template.ReconFilters.DictAssets.ContainsKey(assetId))
                        template.ReconFilters.DictAssets.Add(assetId, assetText);
                }

                for (int i = 0, count = checkedListAUECs.CheckedItems.Count; i < count; i++)
                {
                    int AUECId = int.Parse(((DataRow)(((DataRowView)((checkedListAUECs.CheckedItems[i]))).Row)).ItemArray[0].ToString());
                    string AUECText = ((DataRow)(((DataRowView)((checkedListAUECs.CheckedItems[i]))).Row)).ItemArray[1].ToString();
                    if (!template.ReconFilters.DictAUECs.ContainsKey(AUECId))
                        template.ReconFilters.DictAUECs.Add(AUECId, AUECText);
                }
                for (int i = 0, count = checkedBrokers.CheckedItems.Count; i < count; i++)
                {
                    int brokerId = int.Parse(((DataRow)(((DataRowView)((checkedBrokers.CheckedItems[i]))).Row)).ItemArray[0].ToString());
                    string brokerName = ((DataRow)(((DataRowView)((checkedBrokers.CheckedItems[i]))).Row)).ItemArray[1].ToString();
                    if (!template.ReconFilters.DictBrokers.ContainsKey(brokerId))
                        template.ReconFilters.DictBrokers.Add(brokerId, brokerName);
                }


                for (int i = 0, count = checkedListAccounts.CheckedItems.Count; i < count; i++)
                {
                    int accountId = int.Parse(((DataRow)(((DataRowView)((checkedListAccounts.CheckedItems[i]))).Row)).ItemArray[0].ToString());
                    string AccountName = ((DataRow)(((DataRowView)((checkedListAccounts.CheckedItems[i]))).Row)).ItemArray[1].ToString();
                    if (!template.ReconFilters.DictAccounts.ContainsKey(accountId))
                        template.ReconFilters.DictAccounts.Add(accountId, AccountName);
                }


                for (int i = 0, count = checkedListMasterFunds.CheckedItems.Count; i < count; i++)
                {
                    int masterFundId = int.Parse(((DataRow)(((DataRowView)((checkedListMasterFunds.CheckedItems[i]))).Row)).ItemArray[0].ToString());
                    string masterFundName = ((DataRow)(((DataRowView)((checkedListMasterFunds.CheckedItems[i]))).Row)).ItemArray[1].ToString();
                    if (!template.ReconFilters.DictMasterFunds.ContainsKey(masterFundId))
                        template.ReconFilters.DictMasterFunds.Add(masterFundId, masterFundName);
                }

                for (int i = 0, count = checkedListPrimeBrokers.CheckedItems.Count; i < count; i++)
                {
                    int primeBroker = int.Parse(((DataRow)(((DataRowView)((checkedListPrimeBrokers.CheckedItems[i]))).Row)).ItemArray[0].ToString());
                    string PBName = ((DataRow)(((DataRowView)((checkedListPrimeBrokers.CheckedItems[i]))).Row)).ItemArray[1].ToString();
                    if (!template.ReconFilters.DictPrimeBrokers.ContainsKey(primeBroker))
                        template.ReconFilters.DictPrimeBrokers.Add(primeBroker, PBName);
                }
                _isControlInitialized = false;
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

        #region Public Properties
        public bool IsUnsavedChanges
        {
            get { return _isUnsavedChanges; }
            set { _isUnsavedChanges = value; }
        }


        #endregion

        #region Public Methods
        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void InitializeReconFiltersTab()
        {
            try
            {
                GetReconFilters();
                // LoadBlankReconFilters();

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

    }
}
