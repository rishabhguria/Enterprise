using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.LiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class FutureRootSymbolUI : Form
    {
        DataTable _dtRootSymbolData = new DataTable();
        String _searchTxt = string.Empty;
        private ValueList _UDAAssets = new ValueList();
        private ValueList _UDASecurityTypes = new ValueList();
        private ValueList _UDASectors = new ValueList();
        private ValueList _UDASubSectors = new ValueList();
        private ValueList _UDACountry = new ValueList();
        private ValueList _approvalSatus = new ValueList();

        public event EventHandler<EventArgs<DataTable>> FurureRootDataSaved;
        public delegate void AsyncDelegateRootSymbol(DataTable dt);
        private delegate void UIThreadMarsheller(object sender, EventArgs<SecMasterGlobalPreferences> e);

        /// <summary>
        /// delegate to invoke EnableDisableUIElements
        /// </summary>
        /// <param name="isEnable"></param>
        /// <param name="message"></param>
        public delegate void EnableDisableUIElements(bool isEnable, string message);

        /// <summary>
        /// Occurs when [].
        /// </summary>
        public event EventHandler<EventArgs<bool>> SetDynamicUDAEvent;

        ISecurityMasterServices _securityMaster = null;
        bool _UseCutoffTimeChckbxChanged = false;

        /// <summary>
        /// Dynamic UDA Cache
        /// </summary>
        private SerializableDictionary<string, DynamicUDA> _dynamicUDACache = new SerializableDictionary<string, DynamicUDA>();

        /// <summary>
        /// Event to update dynamic UDA cache 
        /// </summary>
        public event EventHandler<EventArgs<bool>> UpdateDynamicUDACache;

        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public FutureRootSymbolUI()
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

        internal void SetUp(ISecurityMasterServices securityMaster)
        {
            try
            {
                _securityMaster = securityMaster;
                _securityMaster.FutureRootSymbolDataResponse += new EventHandler<EventArgs<DataSet>>(_securityMaster_FutureRootSymbolDataResponse);
                //new SymbolLookUpDataResponse(_securityMaster_FutureRootSymbolDataResponse);
                _securityMaster.ResponseCompleted += new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
                //new CompletedReceivedDelegate(_securityMaster_ResponseCompleted);
                _securityMaster.SMGlobalPrefencesResponse += new EventHandler<EventArgs<SecMasterGlobalPreferences>>(_securityMaster_SMGlobalPrefencesResponse);
                //new SMPrefencesResponse(_securityMaster_SMGlobalPrefencesResponse);
                _securityMaster.UDAAttributesResponse += new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                //new UDAAttributesDataResponse(_securityMaster_UDAAttributesResponse);
                _securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                //new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);

                QueueMessage reqRootData = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_FutureMultiplierREQ, string.Empty);
                reqRootData.RequestID = System.Guid.NewGuid().ToString();
                _securityMaster.SendMessage(reqRootData);

                // diable UI, PRANA-9815
                EnableDisableUI(false, "Getting Data...");

                QueueMessage reqPref = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_GetPrefREQ, string.Empty);
                reqRootData.RequestID = System.Guid.NewGuid().ToString();
                _securityMaster.SendMessage(reqPref);

                if (_securityMaster != null)
                {
                    _securityMaster.GetAllUDAAtrributes();
                }
                btnDelete.Visible = false;
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
        /// Handle UDA attributes response
        /// </summary>
        /// <param name="UDADataCol"></param>
        void _securityMaster_UDAAttributesResponse(object sender, EventArgs<Dictionary<string, Dictionary<int, string>>> e)
        {
            try
            {
                Dictionary<string, Dictionary<int, string>> UDADataCol = e.Value;
                foreach (KeyValuePair<string, Dictionary<int, string>> UDAData in UDADataCol)
                {
                    switch (UDAData.Key)
                    {
                        case SecMasterConstants.CONST_UDASector:
                            FillCommonDataToValueList(_UDASectors, UDAData.Value);
                            break;

                        case SecMasterConstants.CONST_UDASecurityType:
                            FillCommonDataToValueList(_UDASecurityTypes, UDAData.Value);
                            break;

                        case SecMasterConstants.CONST_UDASubSector:
                            FillCommonDataToValueList(_UDASubSectors, UDAData.Value);
                            break;

                        case SecMasterConstants.CONST_UDAAsset:
                            FillCommonDataToValueList(_UDAAssets, UDAData.Value);
                            break;

                        case SecMasterConstants.CONST_UDACountry:
                            FillCommonDataToValueList(_UDACountry, UDAData.Value);
                            break;
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

        private void FillCommonDataToValueList(ValueList globalvalueList, Dictionary<int, string> dictData)
        {
            try
            {
                globalvalueList.ValueListItems.Clear();
                globalvalueList.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictData)
                {
                    globalvalueList.ValueListItems.Add(item.Key, item.Value);
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
        /// 
        /// </summary>
        /// <param name="preferences"></param>
        void _securityMaster_SMGlobalPrefencesResponse(object sender, EventArgs<SecMasterGlobalPreferences> e)
        {
            try
            {
                UIThreadMarsheller mi = new UIThreadMarsheller(_securityMaster_SMGlobalPrefencesResponse);
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { sender, e });
                    }
                    else
                    {
                        chkBoxCutOffTime.Checked = e.Value.UseCutOffTime;
                        _UseCutoffTimeChckbxChanged = false;
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

        void _securityMaster_ResponseCompleted(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                toolStripStatusLabel1.Text = e.Value.Message.ToString();
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

        void _securityMaster_FutureRootSymbolDataResponse(object sender, EventArgs<DataSet> e)
        {
            DataSet ds = e.Value;
            try
            {
                if (ds.Tables.Count > 0)
                {
                    _dtRootSymbolData = ds.Tables[0];
                    BindData(_dtRootSymbolData);
                    // enable UI, PRANA-9815
                    EnableDisableUI(true, "");
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Prana.Utilities.MiscUtilities.PranaBinaryFormatter binaryFormatter = new Prana.Utilities.MiscUtilities.PranaBinaryFormatter();
                SecMasterGlobalPreferences preferences = new SecMasterGlobalPreferences();
                preferences.UseCutOffTime = chkBoxCutOffTime.Checked;

                string errorMsg = string.Empty;
                DataTable updatedDT = new DataTable();
                updatedDT = _dtRootSymbolData.Clone();

                List<string> symbolList = new List<string>();

                if (UpdateDynamicUDACache != null)
                    UpdateDynamicUDACache(sender, new EventArgs<bool>(true));

                errorMsg = GetUpdatedData(errorMsg, updatedDT, symbolList);
                if (string.IsNullOrWhiteSpace(errorMsg))
                {
                    if (updatedDT.Rows.Count > 0)
                    {
                        // Update &amp; value to & for saving in xml only for modified rows
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-9493
                        foreach (DataRow dr in updatedDT.Rows)
                        {
                            foreach (string uda in _dynamicUDACache.Keys)
                            {
                                string temp = dr[uda].ToString();
                                temp = temp.Replace("&amp;", "&");
                                temp = temp.Replace("&lt;", "<");
                                temp = temp.Replace("&gt;", ">");
                                temp = temp.Replace("&quot;", "?");
                                dr[uda] = temp;
                            }
                        }
                        _dtRootSymbolData.AcceptChanges();
                        string saveRequest = binaryFormatter.Serialize(updatedDT);
                        QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_FutureMultiplierSave, saveRequest);
                        qMsg.RequestID = System.Guid.NewGuid().ToString();
                        _securityMaster.SendMessage(qMsg);

                        if (FurureRootDataSaved != null)
                        {
                            FurureRootDataSaved(sender, new EventArgs<DataTable>(updatedDT));
                        }
                        // diable UI, PRANA-9815, disable UI only in case of saving future root data, PRANA-9963
                        EnableDisableUI(false, "Saving data, Please wait... ");
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Data already saved!";
                    }
                }
                else
                {
                    MessageBox.Show(errorMsg, "Prana Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (_UseCutoffTimeChckbxChanged)
                {
                    _UseCutoffTimeChckbxChanged = false;
                    string PrefsaveRequest = binaryFormatter.Serialize(preferences);
                    QueueMessage Msg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_SavePrefREQ, PrefsaveRequest);
                    Msg.RequestID = System.Guid.NewGuid().ToString();
                    _securityMaster.SendMessage(Msg);
                    toolStripStatusLabel1.Text = "Data Saved : " + DateTime.Now.ToString();
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
        /// get modified rows for root symbols
        /// </summary>
        /// <param name="errorMsg">The error message</param>
        /// <param name="updatedDT">Table for storing updated root symbols</param>
        /// <param name="symbolList">List of all root symbols</param>
        /// <returns></returns>
        private string GetUpdatedData(string errorMsg, DataTable updatedDT, List<string> symbolList)
        {
            try
            {
                foreach (DataRow dr in _dtRootSymbolData.Rows)
                {
                    // check for duplicate rows before checking state,PRANA-11242
                    string symbol = dr["Symbol"].ToString();
                    string exchange = dr["Exchange"].ToString();
                    string symbolexg = symbol.ToUpper() + " " + exchange.ToUpper();
                    if (symbolList.Contains(symbolexg))
                    {
                        errorMsg = "Duplicate Symbol Exchange Pair: " + symbol + " " + exchange;
                        break;
                    }
                    else
                    {
                        symbolList.Add(symbolexg);
                    }
                    DataRowState state = dr.RowState;
                    if (state == DataRowState.Modified || state == DataRowState.Added)
                    {
                        if (String.IsNullOrEmpty(symbol))
                        {
                            errorMsg = "Symbol field can not be empty.";
                            break;
                        }

                        if (string.IsNullOrWhiteSpace(dr["Multiplier"].ToString()))
                        {
                            errorMsg = "Please Enter Valid Multiplier for: " + symbol;
                            break;
                        }
                        // Added regular expression validation for dynamic uda value input 
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-9369
                        foreach (string uda in _dynamicUDACache.Keys)
                        {
                            if (!Regex.IsMatch(dr[uda].ToString(), SecMasterConstants.CONST_DynamicUDAValueRegex))
                            {
                                errorMsg = "Please correct value of " + uda + " for " + symbol + ". It should contain only alphanumeric and some special characters. \n\nAllowed special characters are: &/:@*!^%-$_#~(=)`{}\"[];'><?| ";
                                break;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(dr["CutOffTime"].ToString().Trim()))
                        {
                            dr["CutOffTime"] = Convert.ToDateTime(dr["CutOffTime"].ToString()).ToLongTimeString();
                        }

                        // Kuldeep A.: putting "minvalue" in case UDA fields are coming as blank as if they are passed as blank then 
                        // data werenot saving into DB.
                        if (string.IsNullOrWhiteSpace(dr["UDAAssetClassID"].ToString()))
                        {
                            dr["UDAAssetClassID"] = int.MinValue.ToString();
                        }
                        if (string.IsNullOrWhiteSpace(dr["UDASecurityTypeID"].ToString()))
                        {
                            dr["UDASecurityTypeID"] = int.MinValue.ToString();
                        }
                        if (string.IsNullOrWhiteSpace(dr["UDASectorID"].ToString()))
                        {
                            dr["UDASectorID"] = int.MinValue.ToString();
                        }
                        if (string.IsNullOrWhiteSpace(dr["UDASubSectorID"].ToString()))
                        {
                            dr["UDASubSectorID"] = int.MinValue.ToString();
                        }
                        if (string.IsNullOrWhiteSpace(dr["UDACountryID"].ToString()))
                        {
                            dr["UDACountryID"] = int.MinValue.ToString();
                        }
                        if (string.IsNullOrWhiteSpace(dr["IsCurrencyFuture"].ToString()))
                        {
                            dr["IsCurrencyFuture"] = false;
                        }
                        updatedDT.Rows.Add(dr.ItemArray);
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
            return errorMsg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public void BindData(DataTable dt)
        {
            try
            {
                // kuldeep please check why not validating
                // if (UIValidation.GetInstance().validate(this))
                // {
                if (this.InvokeRequired)
                {
                    AsyncDelegateRootSymbol del = new AsyncDelegateRootSymbol(BindData);
                    this.BeginInvoke(del, new object[] { dt });
                }
                else
                {
                    grdData.DataSource = dt;
                    grdData.DataBind();
                    grdData.DisplayLayout.Bands[0].Columns["Multiplier"].Width = 76;
                    grdData.DisplayLayout.Bands[0].Columns["Multiplier"].Header.VisiblePosition = 4;

                    grdData.DisplayLayout.Bands[0].Columns["Symbol"].CharacterCasing = CharacterCasing.Upper;
                    grdData.DisplayLayout.Bands[0].Columns["Symbol"].Width = 50;
                    grdData.DisplayLayout.Bands[0].Columns["Symbol"].Header.VisiblePosition = 1;

                    grdData.DisplayLayout.Bands[0].Columns["PSSymbol"].CharacterCasing = CharacterCasing.Upper;
                    grdData.DisplayLayout.Bands[0].Columns["PSSymbol"].Width = 80;
                    grdData.DisplayLayout.Bands[0].Columns["PSSymbol"].Header.VisiblePosition = 7;

                    grdData.DisplayLayout.Bands[0].Columns["UnderlyingSymbol"].CharacterCasing = CharacterCasing.Upper;
                    grdData.DisplayLayout.Bands[0].Columns["UnderlyingSymbol"].Width = 80;
                    grdData.DisplayLayout.Bands[0].Columns["UnderlyingSymbol"].Header.VisiblePosition = 5;

                    grdData.DisplayLayout.Bands[0].Columns["CutOffTime"].CharacterCasing = CharacterCasing.Upper;
                    grdData.DisplayLayout.Bands[0].Columns["CutOffTime"].Width = 80;
                    grdData.DisplayLayout.Bands[0].Columns["CutOffTime"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.TimeWithSpin;
                    grdData.DisplayLayout.Bands[0].Columns["CutOffTime"].Header.VisiblePosition = 8;

                    grdData.DisplayLayout.Bands[0].Columns["Exchange"].CharacterCasing = CharacterCasing.Upper;
                    grdData.DisplayLayout.Bands[0].Columns["Exchange"].Width = 80;
                    grdData.DisplayLayout.Bands[0].Columns["Exchange"].Header.Caption = "Exchange Suffix";
                    grdData.DisplayLayout.Bands[0].Columns["Exchange"].Header.VisiblePosition = 2;

                    grdData.DisplayLayout.Bands[0].Columns["ProxyRoot"].CharacterCasing = CharacterCasing.Upper;
                    grdData.DisplayLayout.Bands[0].Columns["ProxyRoot"].Width = 80;
                    grdData.DisplayLayout.Bands[0].Columns["ProxyRoot"].Header.Caption = "Proxy Root";
                    grdData.DisplayLayout.Bands[0].Columns["ProxyRoot"].Header.VisiblePosition = 6;


                    grdData.DisplayLayout.Bands[0].Columns["Exchange"].Header.Caption = "Exchange Suffix";

                    UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];

                    UltraGridColumn colCutOffTime = gridBand.Columns["CutOffTime"];
                    colCutOffTime.Header.Caption = "Cut Off Time";
                    colCutOffTime.Header.Column.Width = 70;
                    colCutOffTime.Header.VisiblePosition = 7;

                    UltraGridColumn colUDAAsset = gridBand.Columns["UDAAssetClassID"];
                    colUDAAsset.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUDAAsset.Header.Caption = "Asset";
                    colUDAAsset.ValueList = _UDAAssets;
                    colUDAAsset.NullText = ApplicationConstants.C_COMBO_SELECT;
                    colUDAAsset.Header.Column.Width = 70;

                    UltraGridColumn colUDAUDASector = gridBand.Columns["UDASectorID"];
                    colUDAUDASector.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUDAUDASector.Header.Caption = "Sector";
                    colUDAUDASector.ValueList = _UDASectors;
                    colUDAUDASector.Header.Column.Width = 70;
                    colUDAUDASector.NullText = ApplicationConstants.C_COMBO_SELECT;

                    UltraGridColumn colUDASubSector = gridBand.Columns["UDASubSectorID"];
                    colUDASubSector.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUDASubSector.Header.Caption = "SubSector";
                    colUDASubSector.ValueList = _UDASubSectors;
                    colUDASubSector.Header.Column.Width = 70;
                    colUDASubSector.NullText = ApplicationConstants.C_COMBO_SELECT;

                    UltraGridColumn colUDASecurityType = gridBand.Columns["UDASecurityTypeID"];
                    colUDASecurityType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUDASecurityType.Header.Caption = "Security";
                    colUDASecurityType.ValueList = _UDASecurityTypes;
                    colUDASecurityType.Header.Column.Width = 70;
                    colUDASecurityType.NullText = ApplicationConstants.C_COMBO_SELECT;

                    UltraGridColumn colUDACountry = gridBand.Columns["UDACountryID"];
                    colUDACountry.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUDACountry.Header.Caption = "Country";
                    colUDACountry.ValueList = _UDACountry;
                    colUDACountry.Header.Column.Width = 70;
                    colUDACountry.NullText = ApplicationConstants.C_COMBO_SELECT;

                    UltraGridColumn colIsCurrencyFuture = gridBand.Columns["IsCurrencyFuture"];
                    colIsCurrencyFuture.Header.Caption = "Currency Future";
                    colIsCurrencyFuture.Header.Column.Width = 80;

                    UltraGridColumn colBBGRoot = gridBand.Columns[BloombergSapiConstants.CONST_BLOOMBERG_BBG_ROOT];
                    colBBGRoot.Header.Caption = BloombergSapiConstants.CAPTION_BLOOMBERG_BBG_ROOT;
                    colBBGRoot.Header.Column.Width = 90;
                    colBBGRoot.Header.VisiblePosition = 3;

                    UltraGridColumn colBBGYellowKey = gridBand.Columns[BloombergSapiConstants.CONST_BLOOMBERG_BBG_YELLOW_KEY];
                    colBBGYellowKey.Header.Caption = BloombergSapiConstants.CAPTION_BLOOMBERG_BBG_YELLOW_KEY;
                    colBBGYellowKey.Header.Column.Width = 130;
                    

                    if (!string.IsNullOrEmpty(_searchTxt))
                    {
                        btnGetData_Click(this, null);
                    }

                    // Load saved layout for future root UI if it exists
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-9096
                    String path = Application.StartupPath + "\\Prana Preferences\\" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "\\" + "FutureRoot.xml";
                    if (File.Exists(path))
                    {
                        grdData.DisplayLayout.Load(path,
                            PropertyCategories.AppearanceCollection |
                        PropertyCategories.Bands |
                        PropertyCategories.ColScrollRegions |
                       PropertyCategories.ColumnFilters |
                        PropertyCategories.General |
                       PropertyCategories.Groups |
                        PropertyCategories.RowScrollRegions |
                       PropertyCategories.SortedColumns |
                       PropertyCategories.Summaries |
                       PropertyCategories.UnboundColumns
                            );
                    }

                    // Set Dynamic Columns, set these columns after load of saved layout as dynamic UDA column can be changes to textbox or dropdown based on master values
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-9854
                    if (SetDynamicUDAEvent != null)
                        SetDynamicUDAEvent(this, new EventArgs<bool>(true));

                    // update dynamic uda cache on binding data to grid, PRANA-9986
                    if (UpdateDynamicUDACache != null)
                        UpdateDynamicUDACache(this, new EventArgs<bool>(true));
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

        private void UnwireEvents()
        {
            try
            {
                if (_securityMaster != null)
                {
                    _securityMaster.FutureRootSymbolDataResponse -= new EventHandler<EventArgs<DataSet>>(_securityMaster_FutureRootSymbolDataResponse);
                    //new SymbolLookUpDataResponse(_securityMaster_FutureRootSymbolDataResponse);
                    _securityMaster.ResponseCompleted -= new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
                    //new CompletedReceivedDelegate(_securityMaster_ResponseCompleted);
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-6375
                    _securityMaster.UDAAttributesResponse -= new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                    _securityMaster.SMGlobalPrefencesResponse -= new EventHandler<EventArgs<SecMasterGlobalPreferences>>(_securityMaster_SMGlobalPrefencesResponse);
                    _securityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
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
                txtSearch.Text = string.Empty;
                btnGetData_Click(this, null);

                DataRow dr = _dtRootSymbolData.NewRow();
                //added to set the dynamic uda row value to default uda value, PRANA-10592
                dr = RowDefaultData(dr);
                _dtRootSymbolData.Rows.Add(dr);
                BindData(_dtRootSymbolData);

                //Added to clear all filters, PRANA-10528
                grdData.DisplayLayout.Bands[0].Override.RowFilterMode = RowFilterMode.AllRowsInBand;
                grdData.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                grdData.Focus();
                Infragistics.Win.UltraWinGrid.UltraGridRow grdRow = grdData.Rows[grdData.Rows.Count - 1];
                grdRow.Activate();
                grdRow.Cells["Symbol"].Activate();
                grdData.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdData.ActiveRow != null)
                {
                    DataRowView deleteRow = (DataRowView)grdData.ActiveRow.ListObject;
                    _dtRootSymbolData.Rows.Remove(deleteRow.Row);
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

        private void grdData_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdData);
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
        /// handle when "use cut off time" checkbox value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBoxCutOffTime_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //save useCutOfftime value only on it changed
                _UseCutoffTimeChckbxChanged = true;
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

        #region UnderLying Symbol Validation Section

        DataRow CurrentRowInValidationProcess = null;
        void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                _timer.Stop();
                CurrentRowInValidationProcess = null;
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

        public void ValidateSymbol(string text, DataRow row)
        {
            if (_securityMaster != null && _securityMaster.IsConnected)
            {
                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                reqObj.AddData(text, ApplicationConstants.PranaSymbology);
                reqObj.HashCode = this.GetHashCode();
                if (CurrentRowInValidationProcess != null)
                {
                    CurrentRowInValidationProcess.SetColumnError("UnderlyingSymbol", "Symbol Not Validated !");
                }
                CurrentRowInValidationProcess = row;
                _securityMaster.SendRequest(reqObj);
                StartTimer();
            }
        }

        System.Timers.Timer _timer;
        double interval = double.MinValue;
        void StartTimer()
        {
            if (interval == double.MinValue)
                interval = Convert.ToDouble(ConfigurationManager.AppSettings["SymbolValidationTimeOut"]);
            if (interval == 0)
                interval = 3000;
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true; // Enable it
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (CurrentRowInValidationProcess != null)
                CurrentRowInValidationProcess.SetColumnError("UnderlyingSymbol", "Symbol Not Validated !");
            CurrentRowInValidationProcess = null;
            _timer.Stop();
        }

        private void grdData_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Header.Caption == "UnderlyingSymbol")
            {
                string enteredSymbol = e.Cell.Row.Cells["UnderlyingSymbol"].Value.ToString();
                DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                ValidateSymbol(enteredSymbol, row);
            }
        }
        /// <summary>
        /// Cell change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_CellChange(object sender, CellEventArgs e)
        {
            DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
            if (e.Cell.Column.Header.Caption == "UnderlyingSymbol")
            {

                row.SetColumnError("UnderlyingSymbol", "");
            }
            // Added to replace digits with zero if they are blank in time value to avoid data error, PRANA-12049
            if (e.Cell.Column.Key == "CutOffTime")
            {
                if (e.Cell.Text.Contains("_"))
                    e.Cell.Value = e.Cell.Text.Substring(0, e.Cell.Text.LastIndexOf(" ") + 1).Replace("_", "0");
            }
            if (row.RowState == DataRowState.Unchanged)
                row.SetModified();
        }
        #endregion

        /// <summary>
        /// Search data in grid 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            //Search the text in the root data ..       
            string txt = txtSearch.Text.Trim().ToUpperInvariant();
            try
            {
                if (grdData.Rows.Count > 0)
                {
                    //Empty textbox will load add data
                    if (string.IsNullOrWhiteSpace(txt))
                    {
                        grdData.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                    }
                    else
                    {
                        //Text without space search...
                        if (!txt.Contains(" "))
                        {
                            SetGridFilters(txt);
                        }
                        else
                        {
                            // Multiple Search like ES 6A....
                            string[] txtDetails = txt.Split(' ');
                            foreach (string text in txtDetails)
                            {
                                SetGridFilters(text);
                            }
                        }
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// set filters on grid
        /// </summary>
        /// <param name="text"></param>
        private void SetGridFilters(string text)
        {
            try
            {
                grdData.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                if (!string.IsNullOrEmpty(text))
                {
                    grdData.DisplayLayout.Bands[0].ColumnFilters.LogicalOperator = FilterLogicalOperator.Or;
                    grdData.DisplayLayout.Bands[0].Columns["Symbol"].FilterComparisonType = FilterComparisonType.CaseInsensitive;
                    grdData.DisplayLayout.Bands[0].ColumnFilters["Symbol"].LogicalOperator = FilterLogicalOperator.Or;
                    grdData.DisplayLayout.Bands[0].ColumnFilters["Symbol"].FilterConditions.Add(FilterComparisionOperator.Contains, text);
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
        /// search root data on load
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="filterString"></param>
        internal void HandleOnLoadRequest(string filterString)
        {
            try
            {
                txtSearch.Text = filterString;
                _searchTxt = filterString;
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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FutureRootSymbolUI_Load(object sender, EventArgs e)
        {
            try
            {
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.Font = new Font("Century Gothic", 9F);
                    this.txtSearch.Value = string.Empty;
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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnAddRow.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAddRow.ForeColor = System.Drawing.Color.White;
                btnAddRow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAddRow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAddRow.UseAppStyling = false;
                btnAddRow.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetData.ForeColor = System.Drawing.Color.White;
                btnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetData.UseAppStyling = false;
                btnGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDelete.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
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
        /// Get valuelist from list
        /// </summary>
        /// <param name="values">The list</param>
        /// <returns>The valuelist</returns>
        private ValueList GetValueList(Dictionary<string, string> values)
        {
            // Setting valuelist datavalue as Master Value value instead of key so that text should be saved to database
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-8908
            //TODO: Set valuelist keys and value properly, also set value of cell to text on selecting valuer from dropdown
            ValueList list = new ValueList();
            try
            {
                list.ValueListItems.Add("Undefined", int.MinValue.ToString());
                foreach (string key in values.Keys)
                {
                    list.ValueListItems.Add(values[key], key);
                }
                list.DisplayStyle = ValueListDisplayStyle.DataValue;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return list;
        }

        /// <summary>
        /// Set DynamicUDA columns
        /// </summary>
        /// <param name="dynamicUDAcache">Dictionary of dynamic UDA columns</param>
        internal void SetDynamicUDA(Dictionary<string, DynamicUDA> dynamicUDAcache)
        {
            try
            {
                UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];

                if (dynamicUDAcache != null && dynamicUDAcache.Count > 0)
                {
                    foreach (string uda in dynamicUDAcache.Keys)
                    {
                        if (gridBand.Columns.Exists(uda))
                        {
                            UltraGridColumn gridUDAcolumn = gridBand.Columns[uda];
                            gridUDAcolumn.Header.Caption = dynamicUDAcache[uda].HeaderCaption.ToString();
                            gridUDAcolumn.Header.Column.Width = 70;

                            if (dynamicUDAcache[uda].MasterValues != null && dynamicUDAcache[uda].MasterValues.Count > 0)
                            {
                                gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                                gridUDAcolumn.ValueList = GetValueList(dynamicUDAcache[uda].MasterValues);
                            }
                            else
                            {
                                gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedTextEditor;
                            }
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
        /// Add dynamic UDAs to grdData
        /// </summary>        
        internal void AddDynamicUDA(Dictionary<string, DynamicUDA> dynamicUDAcache)
        {
            try
            {
                if (dynamicUDAcache != null && dynamicUDAcache.Count > 0)
                {
                    foreach (string uda in dynamicUDAcache.Keys)
                    {
                        if (!grdData.DisplayLayout.Bands[0].Columns.Exists(uda))
                        {
                            UltraGridColumn gridUDAcolumn = grdData.DisplayLayout.Bands[0].Columns.Add(uda);
                            gridUDAcolumn.Header.Caption = dynamicUDAcache[uda].HeaderCaption.ToString();
                            gridUDAcolumn.Header.Column.Width = 100;

                            if (dynamicUDAcache[uda].MasterValues != null && dynamicUDAcache[uda].MasterValues.Count > 0)
                            {
                                gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                                gridUDAcolumn.ValueList = GetValueList(dynamicUDAcache[uda].MasterValues);
                            }
                            else
                            {
                                gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedTextEditor;
                            }
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
        /// Save layout for future root grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String filePath = Application.StartupPath + "\\Prana Preferences\\" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "\\" + "FutureRoot.xml";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                grdData.DisplayLayout.Save(filePath,
                    PropertyCategories.AppearanceCollection |
                    PropertyCategories.Bands |
                    PropertyCategories.ColScrollRegions |
                   PropertyCategories.ColumnFilters |
                    PropertyCategories.General |
                   PropertyCategories.Groups |
                    PropertyCategories.RowScrollRegions |
                   PropertyCategories.SortedColumns |
                   PropertyCategories.Summaries |
                   PropertyCategories.UnboundColumns
                   );
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
        /// Sets context menu on right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                UIElement element = grdData.DisplayLayout.UIElement.ElementFromPoint(e.Location);
                if (element != null)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        grdData.ContextMenuStrip = contextMenu;
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
        /// Update dynamic UDA cache 
        /// </summary>
        /// <param name="dynamicUDAcache"></param>
        internal void UpdateDynamicUDA(SerializableDictionary<string, DynamicUDA> dynamicUDAcache)
        {
            try
            {
                if (dynamicUDAcache != null && dynamicUDAcache.Count > 0)
                    _dynamicUDACache = dynamicUDAcache;
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
        /// Enable Disable UI elements
        /// </summary>
        /// <param name="isEnable"></param>
        /// <param name="message"></param>
        private void EnableDisableUI(bool isEnable, string message)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    EnableDisableUIElements del = new EnableDisableUIElements(EnableDisableUI);
                    this.BeginInvoke(del, new object[] { isEnable, message });
                }
                else
                {
                    FutureRootSymbolUI_Fill_Panel.Enabled = isEnable;
                    toolStripStatusLabel1.Text = message + DateTime.Now.ToString();
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
        /// enable UI after data saved response is recieved
        /// </summary>
        /// <param name="saveResponse"></param>
        internal void AfterDataSaved(string saveResponse)
        {
            try
            {
                if (saveResponse == "Success")
                    EnableDisableUI(true, "Data Saved ");
                else
                    EnableDisableUI(true, saveResponse);
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
        /// Add to set dynamic row values to default uda value, PRANA-10592
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public DataRow RowDefaultData(DataRow dr)
        {
            try
            {
                foreach (string duda in _dynamicUDACache.Keys)
                {
                    if (dr.Table.Columns.Contains(duda))
                    {
                        dr[duda] = _dynamicUDACache[duda].DefaultValue;
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
            return dr;
        }

        private void grdData_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();

        }
    }
}