using Infragistics.Win.UltraWinEditors;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public partial class ctrlStandardFilters : UserControl
    {
        public ctrlStandardFilters()
        {
            InitializeComponent();
            //FillBlankFilters();

        }

        private void FillBlankFilters()
        {
            try
            {
                FillAssetData();
                FillAccountData();
                FillMasterFundData();
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

        private void FillMasterFundData()
        {
            try
            {
                DataTable dtMasterFunds = GetMasterFunds();
                lstBoxMasterFund.DataSource = dtMasterFunds;
                lstBoxMasterFund.DisplayMember = "Data";
                lstBoxMasterFund.ValueMember = "Value";
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

        private void FillAccountData()
        {
            try
            {
                DataTable dtAccounts = GetAccounts();
                lstBoxAccount.DataSource = dtAccounts;
                lstBoxAccount.DisplayMember = "Data";
                lstBoxAccount.ValueMember = "Value";
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

        private void FillAssetData()
        {
            try
            {
                DataTable dtAssets = GetAssets();
                lstBoxAsset.DataSource = dtAssets;
                lstBoxAsset.DisplayMember = "Data";
                lstBoxAsset.ValueMember = "Value";
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
        private DataTable GetAssets()
        {
            Dictionary<int, string> dictAssets = null;
            DataTable dtAssets = new DataTable();

            try
            {
                dictAssets = CommonDataCache.CachedDataManager.GetInstance.GetAllAssets();
                dtAssets.Columns.Add("Value");
                dtAssets.Columns.Add("Data");
                object[] rowAsset = new object[2];
                foreach (KeyValuePair<int, string> keyVal in dictAssets)
                {
                    if (!keyVal.Value.Equals("Cash"))
                    {
                        rowAsset[0] = keyVal.Key;
                        rowAsset[1] = keyVal.Value;
                        dtAssets.Rows.Add(rowAsset);
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
            return dtAssets;

        }
        private DataTable GetAccounts()
        {
            Dictionary<int, string> dictAccounts = null;
            DataTable dtAccounts = new DataTable();

            try
            {
                dictAccounts = CommonDataCache.CachedDataManager.GetInstance.GetAccountsWithFullName();
                dtAccounts.Columns.Add("Value");
                dtAccounts.Columns.Add("Data");
                object[] rowAccount = new object[2];
                foreach (KeyValuePair<int, string> keyVal in dictAccounts)
                {
                    rowAccount[0] = keyVal.Key;
                    rowAccount[1] = keyVal.Value;
                    dtAccounts.Rows.Add(rowAccount);
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
            return dtAccounts;
        }
        private DataTable GetMasterFunds()
        {
            DataTable dtMasterFunds = new DataTable();
            try
            {
                Dictionary<int, string> dictMasterFunds = null;
                dictMasterFunds = CachedDataManager.GetInstance.GetAllMasterFunds();
                dtMasterFunds.Columns.Add("Value");
                dtMasterFunds.Columns.Add("Data");
                object[] rowAccount = new object[2];
                foreach (KeyValuePair<int, string> keyVal in dictMasterFunds)
                {
                    rowAccount[0] = keyVal.Key;
                    rowAccount[1] = keyVal.Value;
                    dtMasterFunds.Rows.Add(rowAccount);
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
            return dtMasterFunds;
        }

        private void CheckUncheckAll(string key, bool isChecked)
        {
            try
            {
                if (key == "cbAssets")
                {
                    if (isChecked)
                        for (int j = 0; j < lstBoxAsset.Items.Count; j++)
                        {
                            lstBoxAsset.SetItemChecked(j, !isChecked);
                        }
                    lstBoxAsset.Enabled = !isChecked;
                }
                else if (key == "cbAccounts")
                {
                    if (isChecked)
                        for (int j = 0; j < lstBoxAccount.Items.Count; j++)
                        {
                            lstBoxAccount.SetItemChecked(j, !isChecked);
                        }
                    lstBoxAccount.Enabled = !isChecked;
                }
                else
                {
                    if (isChecked)
                        for (int j = 0; j < lstBoxMasterFund.Items.Count; j++)
                        {
                            lstBoxMasterFund.SetItemChecked(j, !isChecked);
                        }
                    lstBoxMasterFund.Enabled = !isChecked;
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


        private void cbAccount_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                string key = cbAccount.Tag.ToString();
                bool isChecked = cbAccount.Checked;
                CheckUncheckAll(key, isChecked);
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

        private void cbMasterFund_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                string key = cbMasterFund.Tag.ToString();
                bool isChecked = cbMasterFund.Checked;
                CheckUncheckAll(key, isChecked);
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

        internal void UpdateStandardFilters(ClosingTemplate closingTemplate)
        {
            try
            {
                closingTemplate.ListAssetFilters.Clear();
                closingTemplate.ListAccountFliters.Clear();
                closingTemplate.ListMasterFundFilters.Clear();
                if (cbAsset.Checked == true)
                {
                    //Dictionary<int, string> dictAssets = CommonDataCache.CachedDataManager.GetInstance.GetAllAssets();
                    //foreach (int key in dictAssets.Keys)
                    //{
                    //    listAsset.Add(key);
                    //}
                    // closingTemplate.ListAssetFilters = listAsset;
                }
                else
                {
                    for (int i = 0; i < lstBoxAsset.CheckedItems.Count; i++)
                    {
                        int AssetID = int.Parse(((DataRow)(((DataRowView)((lstBoxAsset.CheckedItems[i]))).Row)).ItemArray[0].ToString());

                        closingTemplate.ListAssetFilters.Add(AssetID);
                    }
                }
                if (cbAccount.Checked == true)
                {
                    //Dictionary<int, string> dictAccounts = CommonDataCache.CachedDataManager.GetInstance.GetAccountsWithFullName();
                    //foreach (int key in dictAccounts.Keys)
                    //{
                    //    listAccount.Add(key);
                    //}
                    //closingTemplate.ListAccountFliters = listAccount;
                }
                else
                {
                    for (int i = 0; i < lstBoxAccount.CheckedItems.Count; i++)
                    {
                        int AccountID = int.Parse(((DataRow)(((DataRowView)((lstBoxAccount.CheckedItems[i]))).Row)).ItemArray[0].ToString());

                        closingTemplate.ListAccountFliters.Add(AccountID);
                    }
                }
                if (cbMasterFund.Checked == true || lstBoxMasterFund.CheckedItems.Count == lstBoxMasterFund.Items.Count)
                {
                    //Dictionary<int, string> dictMasterFunds = CachedDataManager.GetInstance.GetAllMasterFunds();
                    //foreach (int key in dictMasterFunds.Keys)
                    //{
                    //    listMasterFund.Add(key);
                    //}
                    //closingTemplate.ListAccountFliters = listMasterFund;
                }
                else
                {
                    for (int i = 0; i < lstBoxMasterFund.CheckedItems.Count; i++)
                    {
                        int MasterFundID = int.Parse(((DataRow)(((DataRowView)((lstBoxMasterFund.CheckedItems[i]))).Row)).ItemArray[0].ToString());

                        closingTemplate.ListMasterFundFilters.Add(MasterFundID);
                    }
                }

                closingTemplate.FromDate = DateTime.Parse(dtFromDate.Value.ToString()).Date;
                closingTemplate.ToDate = DateTime.Parse(dtToDate.Value.ToString()).Date;
                closingTemplate.UseCurrentDate = ultraCheckEditor1.Checked;
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

        internal void LoadStandardFilters(Prana.BusinessObjects.ClosingTemplate template)
        {
            try
            {
                if (lstBoxAsset.Items.Count == 0 || lstBoxAccount.Items.Count == 0 || lstBoxMasterFund.Items.Count == 0)
                    FillBlankFilters();

                if (template.ListAssetFilters.Count == 0 && template.ListAccountFliters.Count == 0 && template.ListMasterFundFilters.Count == 0)
                {
                    LoadBlankFilters();
                }

                if (template.ListAssetFilters.Count == 0)
                {
                    cbAsset.Checked = true;
                    lstBoxAsset.Enabled = false;
                }
                else
                    cbAsset.Checked = false;

                for (int j = 0; j < lstBoxAsset.Items.Count; j++)
                {
                    int assetId = int.Parse(((DataRow)(((DataRowView)((lstBoxAsset.Items[j]))).Row)).ItemArray[0].ToString());
                    foreach (int key in template.ListAssetFilters)
                    {
                        if (key == assetId)
                        {
                            lstBoxAsset.SetItemChecked(j, true);
                            break;
                        }
                        else
                            lstBoxAsset.SetItemChecked(j, false);
                    }
                }


                if (template.ListAccountFliters.Count == 0)
                {
                    cbAccount.Checked = true;
                    lstBoxAccount.Enabled = false;
                }
                else
                    cbAccount.Checked = false;

                for (int j = 0; j < lstBoxAccount.Items.Count; j++)
                {
                    int assetId = int.Parse(((DataRow)(((DataRowView)((lstBoxAccount.Items[j]))).Row)).ItemArray[0].ToString());
                    foreach (int key in template.ListAccountFliters)
                    {
                        if (key == assetId)
                        {
                            lstBoxAccount.SetItemChecked(j, true);
                            break;
                        }
                        else
                            lstBoxAccount.SetItemChecked(j, false);
                    }
                }

                if (template.ListMasterFundFilters.Count == 0)
                {
                    cbMasterFund.Checked = true;
                    lstBoxMasterFund.Enabled = false;
                }
                else
                    cbMasterFund.Checked = false;

                for (int j = 0; j < lstBoxMasterFund.Items.Count; j++)
                {
                    int assetId = int.Parse(((DataRow)(((DataRowView)((lstBoxMasterFund.Items[j]))).Row)).ItemArray[0].ToString());
                    foreach (int key in template.ListMasterFundFilters)
                    {
                        if (key == assetId)
                        {
                            lstBoxMasterFund.SetItemChecked(j, true);
                            break;
                        }
                        else
                            lstBoxMasterFund.SetItemChecked(j, false);
                    }
                }


                dtFromDate.Value = template.FromDate;
                dtToDate.Value = template.ToDate;
                ultraCheckEditor1.Checked = template.UseCurrentDate;

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

        private void LoadBlankFilters()
        {
            try
            {
                cbAsset.Checked = true;
                cbAccount.Checked = true;
                cbMasterFund.Checked = true;


                for (int j = 0; j < lstBoxAsset.Items.Count; j++)
                {
                    lstBoxAsset.SetItemChecked(j, false);
                    lstBoxAsset.Enabled = false;
                }

                for (int j = 0; j < lstBoxAccount.Items.Count; j++)
                {
                    lstBoxAccount.SetItemChecked(j, false);
                    lstBoxAccount.Enabled = false;
                }

                for (int j = 0; j < lstBoxMasterFund.Items.Count; j++)
                {
                    lstBoxMasterFund.SetItemChecked(j, false);
                    lstBoxMasterFund.Enabled = false;
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

        private void cbAsset_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                string key = cbAsset.Tag.ToString();
                bool isChecked = cbAsset.Checked;
                CheckUncheckAll(key, isChecked);
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

        private void lstBoxAsset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstBoxAsset.Items.Count; i++)
            {


                if (lstBoxAsset.GetItemRectangle(i).Contains(lstBoxAsset.PointToClient(MousePosition)))
                {
                    switch (lstBoxAsset.GetItemCheckState(i))
                    {
                        case CheckState.Checked:
                            lstBoxAsset.SetItemCheckState(i, CheckState.Unchecked);
                            break;
                        case CheckState.Indeterminate:
                        case CheckState.Unchecked:
                            lstBoxAsset.SetItemCheckState(i, CheckState.Checked);
                            break;
                    }

                }

            }
        }

        private void lstBoxMasterFund_SelectedIndexChanged(object sender, EventArgs e)
        {



            for (int i = 0; i < lstBoxMasterFund.Items.Count; i++)
            {
                if (lstBoxMasterFund.GetItemRectangle(i).Contains(lstBoxMasterFund.PointToClient(MousePosition)))
                {
                    switch (lstBoxMasterFund.GetItemCheckState(i))
                    {
                        case CheckState.Checked:
                            lstBoxMasterFund.SetItemCheckState(i, CheckState.Unchecked);
                            break;
                        case CheckState.Indeterminate:
                        case CheckState.Unchecked:
                            lstBoxMasterFund.SetItemCheckState(i, CheckState.Checked);
                            break;
                    }

                }

            }

        }

        private void lstBoxAccount_SelectedIndexChanged(object sender, EventArgs e)
        {

            for (int i = 0; i < lstBoxAccount.Items.Count; i++)
            {


                if (lstBoxAccount.GetItemRectangle(i).Contains(lstBoxAccount.PointToClient(MousePosition)))
                {
                    switch (lstBoxAccount.GetItemCheckState(i))
                    {
                        case CheckState.Checked:
                            lstBoxAccount.SetItemCheckState(i, CheckState.Unchecked);
                            break;
                        case CheckState.Indeterminate:
                        case CheckState.Unchecked:
                            lstBoxAccount.SetItemCheckState(i, CheckState.Checked);
                            break;
                    }

                }

            }
        }


        public void EnableDisableControl(string rootTemplateType, bool isPreviewMode)
        {
            if (!isPreviewMode)
            {
                if (rootTemplateType.Equals(ClosingType.Closing.ToString()))
                {
                    dtFromDate.Enabled = false;
                    dtToDate.Enabled = true;
                }
                else
                {
                    dtToDate.Enabled = false;
                    dtFromDate.Enabled = true;
                }
            }
            else
            {
                dtFromDate.Enabled = true;
                dtToDate.Enabled = true;
            }
        }

        private void ultraCheckEditor1_CheckedChanged(object sender, EventArgs e)
        {
            //if (ultraCheckEditor1.Checked)
            //{
            //    dtFromDate.Enabled = false;
            //    dtToDate.Enabled = false;
            //}
            //else
            //{

            //        dtFromDate.Enabled = true;
            //        dtToDate.Enabled = true;

            //}
        }

        // Checks that the To date and From Date Range is Valid or Not
        private void OnDateValueChanged(object sender, System.EventArgs e)
        {
            UltraDateTimeEditor ultraDateTimeEditor = (UltraDateTimeEditor)sender;
            if (dtToDate.DateTime.Date < dtFromDate.DateTime.Date)
            {
                MessageBox.Show("To date cannot be less than From date", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (ultraDateTimeEditor.Name.Equals(dtToDate.Name))
                    ultraDateTimeEditor.DateTime = dtFromDate.DateTime;
                else
                    ultraDateTimeEditor.DateTime = dtToDate.DateTime;
            }
        }

    }
}
