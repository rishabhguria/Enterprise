using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class MappingForm : Form, IMappingFile
    {
        //DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public string _primeBroker = string.Empty;
        // to Store the originally selected value in ultra combo.
        string _cmbbxMappingOriginalSelectedValue = string.Empty;
        bool _isAddingNewRow = false;
        string serverPath = string.Empty;

        public MappingForm()
        {
            try
            {
                InitializeComponent();
                //  XmlDocument xmlDocMapping = new XmlDocument();
                //xmlDocMapping.Load(xmlMappingPath);
                ReadFileDetails();
                SetUserPermissions();
                MappingFormHelper.SetUp(this.GetHashCode());
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
        /// Set user based permission
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void SetUserPermissions()
        {
            try
            {
                btnAddRow.Enabled = true;
                btnDelete.Enabled = true;
                btnSave.Enabled = true;
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

        void _secMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                //CHMW-3036	[Mapping Form] Cross thread error.
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => _secMaster_SecMstrDataResponse(sender, e)));
                }
                else
                {
                    SecMasterBaseObj secMasterObj = e.Value;
                    //CHMW-3153	[Mapping Form] Object reference error after closing mapping form in a scenario.
                    if (grdData != null && !grdData.IsDisposed && grdData.Rows != null && grdData.Rows.Count > 0)
                    {
                        foreach (UltraGridRow row in grdData.Rows)
                        {
                            if (row.Cells["PranaSymbol"].Value != null && secMasterObj != null)
                            {
                                if (row.Cells["PranaSymbol"].Value.ToString() == secMasterObj.TickerSymbol)
                                {
                                    row.Cells["CompanyName"].Value = secMasterObj.LongName;
                                }
                            }
                        }
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
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void ReadFileDetails()
        {
            try
            {
                DataTable mappingFileData = MappingFormHelper.GetFileListTable(Application.StartupPath);
                if (mappingFileData != null)
                {
                    cmbbxMapping.DataSource = mappingFileData;
                    cmbbxMapping.ValueMember = "Name";
                    cmbbxMapping.DisplayMember = "DisplayName";
                    cmbbxMapping.DataBind();
                    cmbbxMapping.DisplayLayout.Bands[0].Columns["Name"].Hidden = true;
                    cmbbxMapping.Value = int.MinValue;
                    cmbbxPrimeBroker.Enabled = false;
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
        /// Prompt user that information is saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //For changes was done to add a counter party on the fly.
                //MappedBrokerCode is New, that means user added a new counter party, in this case change MappedBrokerCode with NewBrokerCode the save the changes in XML
                if (cmbbxMapping.Value.ToString() != int.MinValue.ToString())
                {
                    if (string.IsNullOrEmpty(serverPath) && ThirdPartyClientManager.ServiceInnerChannel != null)
                    {
                        serverPath = ThirdPartyClientManager.ServiceInnerChannel.GetServerPath();
                    }
                    if (!string.IsNullOrEmpty(serverPath))
                    {
                        bool isNewCounterPartyAdded = false;
                        if (cmbbxMapping.Text.Equals("CounterPartyMapping") && ds.Tables.Count > 0)
                        {
                            isNewCounterPartyAdded = MappingFormHelper.SaveCounterPartyVenueDetails(ds);
                        }

                        // Validate data, before saving to xml, blank fields not allowed.    
                        if (cmbbxMapping.Text.Equals("AccountMapping", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (grdData.ActiveRow != null && grdData.ActiveRow.Cells != null && grdData.ActiveRow.Cells.Count >= 4)
                            {
                                if (string.IsNullOrWhiteSpace(grdData.ActiveRow.Cells[0].Text))
                                {
                                    MessageBox.Show(this, "Account Name cannot be blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                if (string.IsNullOrWhiteSpace(grdData.ActiveRow.Cells[1].Text))
                                {
                                    MessageBox.Show(this, "Please enter PB Account Code.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                if (string.IsNullOrWhiteSpace(grdData.ActiveRow.Cells[2].Text))
                                {
                                    MessageBox.Show(this, "Please enter PB Account Name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                        MappingFormHelper.WriteDataSet(ds, cmbbxMapping.Value.ToString(), Application.StartupPath);

                        string sourceFolder = Application.StartupPath + "\\MappingFiles\\ReconMappingXml";
                        string destFolder = serverPath + "\\MappingFiles\\ReconMappingXml";

                        CopyFolder(sourceFolder, destFolder);

                        _isAddingNewRow = false;
                        if (isNewCounterPartyAdded)
                        {
                            MessageBox.Show(this, "New Broker added, reload settings from main menu to update the broker in cache.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(this, "Mapping Saved", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        //to reload the updated xml we assign the data source again.
                        cmbbxMapping.Value = cmbbxMapping.Value;
                    }
                    else
                    {
                        MessageBox.Show(this, "Unable to save because Trade Service is not connected", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        /// Copy a folder named ReconMappingXml from client to trade server side.
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        private void CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                string[] files = Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string destFile = Path.Combine(destFolder, Path.GetFileName(file));
                    File.Copy(file, destFile, true);
                }

                string[] subDirectories = Directory.GetDirectories(sourceFolder);
                foreach (string subDirectory in subDirectories)
                {
                    string destSubDirectory = Path.Combine(destFolder, Path.GetFileName(subDirectory));
                    CopyFolder(subDirectory, destSubDirectory);
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

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (!cmbbxMapping.Value.ToString().Equals(int.MinValue.ToString()))
                {
                    DataRow dr = ds.Tables[1].NewRow();
                    dr[cmbbxPrimeBroker.ValueMember] = cmbbxPrimeBroker.Value;
                    ds.Tables[1].Rows.Add(dr);
                    grdData.ActiveRow = grdData.Rows[(grdData.Rows.Count - 1)];   // To be Revisited.
                    _isAddingNewRow = true;
                }
                else
                {
                    MessageBox.Show("Select Mapping", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdData.ActiveRow != null)
                {
                    grdData.ActiveRow.Delete(true);
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
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8844
                //Modified by sachin mishra 15/06/15
                if (cmbbxMapping.Text.Equals("CounterPartyMapping"))
                {

                    string brokerCode = e.Cell.Value.ToString().ToLower();
                    foreach (UltraGridRow row in grdData.Rows)
                    {
                        if (!String.IsNullOrEmpty(brokerCode) && row.Index != e.Cell.Row.Index && row.Cells["BrokerCode"].Text.ToString().ToLower().Equals(brokerCode))
                        {
                            MessageBox.Show("Records can not be duplicate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cell.Row.Cells["BrokerCode"].Value = string.Empty;
                            break;
                        }
                    }
                }

                string value = e.Cell.Value.ToString();
                if (e.Cell.Column.Key == "PranaSymbol")
                {
                    MappingFormHelper.SendSMRequest(value);
                }
                else if (e.Cell.Column.Key == "PranaBrokerID")
                {
                    e.Cell.Row.Cells["PranaBroker"].Value = e.Cell.Text;
                }
                else if (e.Cell.Column.Key == "PranaAccountID")
                {
                    e.Cell.Row.Cells["PranaAccount"].Value = e.Cell.Text;
                }
                else if (e.Cell.Column.Key == "PranaAssetID")
                {
                    e.Cell.Row.Cells["PranaAsset"].Value = e.Cell.Text;
                }
                else if (e.Cell.Column.Key == "SideTagValue")
                {
                    e.Cell.Row.Cells["Side"].Value = e.Cell.Text;
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



        private void cmbbxMapping_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbbxMapping.Value.ToString() != int.MinValue.ToString())
                {
                    ds = MappingFormHelper.GetDataSet(cmbbxMapping.Value.ToString(), Application.StartupPath);
                    if (ds.Tables.Count == 2)
                    {
                        // if there is no error in XML format, update the originally selected value.
                        _cmbbxMappingOriginalSelectedValue = cmbbxMapping.Value.ToString();
                        ClearGrid();

                        grdData.DataSource = ds;
                        grdData.DataBind();
                        grdData.DataMember = ds.Tables[1].ToString();

                        BindPrimeBrokerCombo(ds.Tables[0]);
                        //CHMW-2472	[Client][MappingForm]Only Corresponding accounts should be displayed
                        //if (grdData.DisplayLayout.Bands[0].Columns.Exists("PranaAccount"))
                        //{
                        //    grdData.DisplayLayout.Bands[0].Columns["PranaAccount"].ValueList = GetAccountValueList();
                        //    grdData.DisplayLayout.Bands[0].Columns["PranaAccount"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        //}
                        if (grdData.DisplayLayout.Bands[0].Columns.Exists("PranaImportTag"))
                        {
                            grdData.DisplayLayout.Bands[0].Columns["PranaImportTag"].ValueList = MappingFormHelper.GetPranaImportTagValueList();
                            grdData.DisplayLayout.Bands[0].Columns["PranaImportTag"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        }
                        if (grdData.DisplayLayout.Bands[0].Columns.Exists("PranaSymbol"))
                        {
                            grdData.DisplayLayout.Bands[0].Columns["PranaSymbol"].CharacterCasing = CharacterCasing.Upper;
                        }
                        else if (grdData.DisplayLayout.Bands[0].Columns.Exists("PranaBrokerID"))
                        {
                            //grdData.DisplayLayout.Bands[0].Columns.Exists("PranaBrokerID")
                            if (!grdData.DisplayLayout.Bands[0].Columns.Exists("PranaBroker"))
                            {
                                grdData.DisplayLayout.Bands[0].Columns.Add("PranaBroker");
                                ds.Tables[0].Columns.Add("PranaBroker");
                            }
                            grdData.DisplayLayout.Bands[0].Columns["PranaBroker"].Hidden = true;
                            grdData.DisplayLayout.Bands[0].Columns["PranaBrokerID"].ValueList = MappingFormHelper.GetCounterPartyValueList(false);
                            grdData.DisplayLayout.Bands[0].Columns["PranaBrokerID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        }
                        else if (grdData.DisplayLayout.Bands[0].Columns.Exists("PranaAccountID"))
                        {
                            if (!grdData.DisplayLayout.Bands[0].Columns.Exists("PranaAccount"))
                            {
                                grdData.DisplayLayout.Bands[0].Columns.Add("PranaAccount");
                                ds.Tables[0].Columns.Add("PranaAccount");
                            }
                            grdData.DisplayLayout.Bands[0].Columns["PranaAccount"].Hidden = true;

                            grdData.DisplayLayout.Bands[0].Columns["PranaAccountID"].ValueList = MappingFormHelper.GetAccountCollectionValueList();
                        }
                        else if (grdData.DisplayLayout.Bands[0].Columns.Exists("PranaAssetID"))
                        {
                            if (!grdData.DisplayLayout.Bands[0].Columns.Exists("PranaAsset"))
                            {
                                grdData.DisplayLayout.Bands[0].Columns.Add("PranaAsset");
                                ds.Tables[0].Columns.Add("PranaAsset");
                            }
                            grdData.DisplayLayout.Bands[0].Columns["PranaAsset"].Hidden = true;
                            grdData.DisplayLayout.Bands[0].Columns["PranaAssetID"].ValueList = MappingFormHelper.GetAssetValueList();
                        }
                        else if (grdData.DisplayLayout.Bands[0].Columns.Exists("SideTagValue"))
                        {
                            if (!grdData.DisplayLayout.Bands[0].Columns.Exists("Side"))
                            {
                                grdData.DisplayLayout.Bands[0].Columns.Add("Side");

                                ds.Tables[0].Columns.Add("Side");
                            }
                            grdData.DisplayLayout.Bands[0].Columns["Side"].Hidden = true;
                            Dictionary<string, string> sides = MappingFormHelper.GetAllOrderSides();
                            grdData.DisplayLayout.Bands[0].Columns["SideTagValue"].ValueList = MappingFormHelper.GetValueList<string>(sides); ;
                        }
                        #region commented
                        //else if (grdData.DisplayLayout.Bands[0].Columns.Exists("PranaSubAccount"))
                        //{
                        //    ValueList subAccount = new ValueList();
                        //    DataSet dsAccounts = CachedDataManager.GetInstance.GetMasterCategorySubCategoryTablesFromDB();
                        //    if (dsAccounts != null && dsAccounts.Tables.Count > 0 && dsAccounts.Tables[0].Rows.Count > 0)
                        //    {
                        //        if (!dsAccounts.Relations.Contains("SubCashAccounts"))
                        //        {
                        //            //dsAccounts.Relations.Add("SubCashAccounts", dsAccounts.Tables["CashAccounts"].Columns["AccountID"], dsAccounts.Tables["SubCashAccounts"].Columns["AccountID"], false);
                        //        }
                        //        foreach (DataRow dr in dsAccounts.Tables["SubCashAccounts"].Rows)
                        //        {
                        //            ////if (!dr["Type"].ToString().Contains("Accrued"))
                        //            ////{
                        //            //DataRow[] subAccounts = dr.GetChildRows("SubCashAccounts");
                        //            //foreach (DataRow row in subAccounts)
                        //            //{
                        //            subAccount.ValueListItems.Add(dr["Acronym"].ToString(), dr["Acronym"].ToString());
                        //            //}
                        //            //}
                        //        }
                        //    }
                        //    grdData.DisplayLayout.Bands[0].Columns["PranaSubAccount"].ValueList = subAccount;
                        //    if (_cashTransactionType != null && _cashTransactionType.Count > 0)
                        //    {
                        //        DataSet dataSet = (DataSet)grdData.DataSource;
                        //        foreach (string activityType in _cashTransactionType)
                        //        {
                        //            DataRow dr = dataSet.Tables[1].NewRow();
                        //            dr[cmbbxPrimeBroker.ValueMember] = cmbbxPrimeBroker.Value;
                        //            dr["PBSubAccountCode"] = activityType;
                        //            dataSet.Tables[1].Rows.Add(dr);
                        //        }
                        //    }
                        //}
                        #endregion
                        else if (grdData.DisplayLayout.Bands[0].Columns.Exists("PBSubAccountCode"))
                        {
                            if (grdData.DisplayLayout.Bands[0].Columns.Exists("ActivityType"))
                            {
                                grdData.DisplayLayout.Bands[0].Columns["ActivityType"].ValueList = MappingFormHelper.GetActivityValueList();
                                grdData.DisplayLayout.Bands[0].Columns["ActivityType"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                            }
                            if (grdData.DisplayLayout.Bands[0].Columns.Exists("CashValueType"))
                            {
                                grdData.DisplayLayout.Bands[0].Columns["CashValueType"].ValueList = MappingFormHelper.GetLongShortValueList();
                                grdData.DisplayLayout.Bands[0].Columns["CashValueType"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                            }
                            if (_cashTransactionType != null && _cashTransactionType.Count > 0)
                            {
                                DataSet dataSet = (DataSet)grdData.DataSource;
                                foreach (string cashTransaction in _cashTransactionType)
                                {
                                    DataRow dr = dataSet.Tables[1].NewRow();
                                    dr[cmbbxPrimeBroker.ValueMember] = cmbbxPrimeBroker.Value;
                                    if (dataSet.Tables[0].Columns.Contains("ActivityType"))
                                    {
                                        dr["ActivityType"] = cashTransaction;
                                    }
                                    dataSet.Tables[1].Rows.Add(dr);
                                }
                            }

                        }
                        else if (grdData.DisplayLayout.Bands[0].Columns.Exists("BrokerCode"))
                        {
                            if (!grdData.DisplayLayout.Bands[0].Columns.Exists("BrokerCode"))
                            {
                                grdData.DisplayLayout.Bands[0].Columns.Add("BrokerCode");
                                ds.Tables[0].Columns.Add("BrokerCode");
                                grdData.DisplayLayout.Bands[0].Columns.Add("MappedBrokerCode");
                                ds.Tables[0].Columns.Add("MappedBrokerCode");
                                grdData.DisplayLayout.Bands[0].Columns.Add("NewBrokerCode");
                                ds.Tables[0].Columns.Add("NewBrokerCode");
                            }

                            grdData.DisplayLayout.Bands[0].Columns["MappedBrokerCode"].ValueList = MappingFormHelper.GetCounterPartyValueList(true);
                        }

                    }
                    else
                    {
                        // Modified by Ankit Gupta on 29 Sep, 2014.
                        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1365
                        // If XML is not in correct format, give a prompt to the user, and retain original selection.
                        MessageBox.Show("XML not in Correct Format", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (cmbbxMapping.IsItemInList(_cmbbxMappingOriginalSelectedValue))
                        {
                            cmbbxMapping.Value = _cmbbxMappingOriginalSelectedValue;
                        }
                        cmbbxMapping.ToggleDropdown();
                        return;
                        //ClearGrid();
                    }
                }
                else
                {
                    ClearGrid();
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




        private void cmbbxPrimeBroker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbbxPrimeBroker.DataSource != null && cmbbxPrimeBroker.Value != null)
                {
                    SetVisibleRow(cmbbxPrimeBroker.Value.ToString(), cmbbxPrimeBroker.ValueMember);
                    //CHMW-2472	[Client][MappingForm]Only Corresponding accounts should be displayed
                    // http://jira.nirvanasolutions.com:8080/browse/WH-26
                    // Displaying all accounts for Prana mode, 'permitted accounts requirement' was for CHMW only.
                    if (grdData.DisplayLayout.Bands[0].Columns.Exists("PranaAccount"))
                    {
                        grdData.DisplayLayout.Bands[0].Columns["PranaAccount"].ValueList = MappingFormHelper.GetAccountValueList();
                        grdData.DisplayLayout.Bands[0].Columns["PranaAccount"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
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
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="rowsToBind"></param>
        /// <param name="idColumn"></param>
        private void SetVisibleRow(string rowsToBind, string idColumn)
        {
            try
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in grdData.Rows)
                {
                    if (row.Cells[idColumn].Value.ToString() == rowsToBind)
                    {
                        row.Hidden = false;
                    }
                    else
                    {
                        row.Hidden = true;
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
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        private void ClearGrid()
        {
            try
            {
                cmbbxPrimeBroker.Enabled = false;
                grdData.DataSource = null;
                grdData.DataMember = null;
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
        /// Bind all prime brokers/Third party available to the prime broker combo box.
        /// For CH users bind all the third parties automatically
        /// 
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="dt"></param>
        private void BindPrimeBrokerCombo(DataTable dt)
        {
            try
            {
                MappingFormHelper.BindAllThirdParties(dt);

                if (dt != null)
                {
                    cmbbxPrimeBroker.Enabled = true;
                    cmbbxPrimeBroker.DataSource = null;
                    cmbbxPrimeBroker.DataSource = dt;
                    cmbbxPrimeBroker.ValueMember = dt.Columns[0].Caption;
                    cmbbxPrimeBroker.DisplayMember = dt.Columns[1].Caption;
                    cmbbxPrimeBroker.DataBind();
                    cmbbxPrimeBroker.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    if (dt.Rows.Count > 0)
                    {
                        string value = string.Empty;
                        if (string.IsNullOrWhiteSpace(_primeBroker))
                            value = dt.Rows[0].ItemArray[0].ToString();
                        else
                        {
                            if (dt.Columns.Contains("Name"))
                            {
                                DataRow[] dataRows = dt.Select("Name =  '" + _primeBroker + "'");
                                foreach (DataRow row in dataRows)
                                {
                                    value = row[0].ToString();
                                }
                            }
                            _primeBroker = string.Empty;
                        }
                        cmbbxPrimeBroker.Value = value;
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

        #region IMappingFile Members

        public Form Reference()
        {
            return this;
        }


        public ISecurityMasterServices SecurityMaster
        {
            set { NewUtilities.SecurityMaster = value; }
        }

        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MappingForm_Load(object sender, EventArgs e)
        {
            try
            {
                NewUtilities.SecurityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_secMaster_SecMstrDataResponse);
                this.Disposed += new EventHandler(MappingForm_Disposed);

                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RECONCILATION);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }

                if (ThirdPartyClientManager.ServiceInnerChannel != null && ThirdPartyClientManager.ServiceInnerChannel != null)
                {
                    serverPath = ThirdPartyClientManager.ServiceInnerChannel.GetServerPath();
                }
                else
                {
                    MessageBox.Show(this, "Trade Service is not connected", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void SetButtonsColor()
        {
            try
            {
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAddRow.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAddRow.ForeColor = System.Drawing.Color.White;
                btnAddRow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAddRow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAddRow.UseAppStyling = false;
                btnAddRow.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDelete.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDelete.ForeColor = System.Drawing.Color.White;
                btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDelete.UseAppStyling = false;
                btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MappingForm_Disposed(object sender, EventArgs e)
        {
            try
            {
                ThirdPartyClientManager.Dispose();
                if (MappingClosed != null)
                {
                    MappingClosed(this, EventArgs.Empty);
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



        public event EventHandler MappingClosed;

        #endregion


        #region IMappingFile Members

        List<string> _cashTransactionType;
        public List<string> activityType
        {
            set
            {
                _cashTransactionType = value;
                DataTable dt = (DataTable)cmbbxMapping.DataSource;
                //dt.Select("SubAccountMapping");
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dr[dc].ToString().Contains("CounterParty"))
                        {
                            if (value != null && value.Count == 2)
                                _primeBroker = value[2];
                            //int i = dt.Rows.IndexOf(dr);
                            cmbbxMapping.Value = dr[1].ToString();
                            break;
                        }

                    }

                }
            }

        }
        #endregion
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region  code for custom column chooser
        //private CustomColumnChooser _customColumnChooserDialog = new CustomColumnChooser();
        //private void grdData_ShowCustomColumnChooserDialog()
        //{
        //    try
        //    {
        //        if (this._customColumnChooserDialog == null || this._customColumnChooserDialog.IsDisposed)
        //        {
        //            _customColumnChooserDialog = new CustomColumnChooser();
        //        }
        //        if (this._customColumnChooserDialog.Owner == null)
        //        {
        //            this._customColumnChooserDialog.Owner = this.FindForm();
        //        }
        //        if (this._customColumnChooserDialog.Grid == null)
        //        {
        //            this._customColumnChooserDialog.Grid = this.grdData;
        //        }
        //        this._customColumnChooserDialog.Show();
        //        this._customColumnChooserDialog.DesktopLocation = this._customColumnChooserDialog.Owner.DesktopLocation;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        #endregion
        private void grdData_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdData);

                //this.grdData.BeforeColumnChooserDisplayed -= new BeforeColumnChooserDisplayedEventHandler(grdData_BeforeColumnChooserDisplayed);
                //if (this.grdData.DisplayLayout.Bands[0].Columns.Count > 0)
                //{
                //    this.grdData.ShowColumnChooser(this.grdData.DisplayLayout.Bands[0], false, "Column Chooser", false, new Rectangle(new Point(this.grdData.FindForm().Location.X, this.grdData.FindForm().Location.Y), new Size(200, 300)));
                //}
                //this.grdData.BeforeColumnChooserDisplayed += new BeforeColumnChooserDisplayedEventHandler(grdData_BeforeColumnChooserDisplayed);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //For counter party mapping make New broker code column editable only if user want to add a new counter party.
                if (cmbbxMapping.Text.Equals("CounterPartyMapping"))
                {
                    if (e.Row.Band.Columns.Exists("MappedBrokerCode") && e.Row.Band.Columns.Exists("NewBrokerCode") && e.Row.Cells["MappedBrokerCode"].Text.Equals("New"))
                    {
                        e.Row.Cells["NewBrokerCode"].Activation = Activation.AllowEdit;
                    }
                    else if (e.Row.Band.Columns.Exists("NewBrokerCode"))
                    {
                        e.Row.Cells["NewBrokerCode"].Activation = Activation.NoEdit;
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
        /// Before exit edit mode event for grdData on the form, used to validate the information entered by the user.
        /// This code makes sure that user does not leaves the fields blank.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            try
            {
                if (!_isAddingNewRow && cmbbxMapping.Text.Equals("AccountMapping", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (grdData.ActiveRow != null && grdData.ActiveRow.Cells != null && grdData.ActiveRow.Cells.Count >= 4)
                    {
                        if (string.IsNullOrWhiteSpace(grdData.ActiveRow.Cells[0].Text))
                        {
                            MessageBox.Show("Account Name cannot be blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            e.Cancel = true;
                        }
                        if (string.IsNullOrWhiteSpace(grdData.ActiveRow.Cells[1].Text))
                        {
                            MessageBox.Show("Please enter PB Account Code.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            e.Cancel = true;
                        }
                        if (string.IsNullOrWhiteSpace(grdData.ActiveRow.Cells[2].Text))
                        {
                            MessageBox.Show("Please enter PB Account Name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            e.Cancel = true;
                        }
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
        /// Initialize row event for the combo box, used to add tool tip text for each row.
        /// Added by Ankit Gupta on 17 Nov, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1847
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbbxMapping_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists("DisplayName"))
                {
                    e.Row.ToolTipText = e.Row.Cells["DisplayName"].Text;
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
        /// Initialize row event for the combo box, used to add tool tip text for each row.
        /// Added by Ankit Gupta on 17 Nov, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1847
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbbxPrimeBroker_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists("Name"))
                {
                    e.Row.ToolTipText = e.Row.Cells["Name"].Text;
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

        private void grdData_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();

        }

        /// <summary>
        /// Save data into Excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMappingUIExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdData != null && grdData.Rows.Count > 0 && grdData.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    ExcelAndPrintUtilities excelAndPrintUtilities = new ExcelAndPrintUtilities();
                    List<UltraGrid> grd = new List<UltraGrid>();
                    grd.Add(grdData);
                    excelAndPrintUtilities.ExportToExcel(grd, "Mapping Xmls", true);
                }
                else
                {
                    MessageBox.Show("Nothing to Export!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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