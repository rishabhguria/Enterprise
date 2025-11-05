using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

namespace Prana.Tools.PL.SecMaster
{
    public partial class ctrlAccountWiseUDA : UserControl
    {

        DataTable _dtAccountwiseUDAData = new DataTable();
        private ValueList _accounts = new ValueList();
        private ValueList _UDAAssets = new ValueList();
        private ValueList _UDASecurityTypes = new ValueList();
        private ValueList _UDASectors = new ValueList();
        private ValueList _UDASubSectors = new ValueList();
        private ValueList _UDACountry = new ValueList();
        //private ValueList _approvalSatus = new ValueList();
        Dictionary<int, string> _dictUserAccounts = new Dictionary<int, string>();
        static int _userID = int.MinValue;
        static string _accountWiseUDAFilePath = string.Empty;
        static string _accountWiseUDALayoutDirectoryPath = string.Empty;
        DataTable _dtAccountwiseUDADeletedData = new DataTable();

        public delegate void AsyncDelegateRootSymbol(DataTable dt);
        ISecurityMasterServices _securityMaster = null;
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlAccountWiseUDA()
        {
            try
            {
                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

                btnAddRow.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAddRow.ForeColor = System.Drawing.Color.White;
                btnAddRow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAddRow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAddRow.UseAppStyling = false;
                btnAddRow.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// <param name="securityMaster"></param>
        public void SetUp(ISecurityMasterServices securityMaster)
        {
            try
            {
                SetUserPermissions();

                _securityMaster = securityMaster;
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    _securityMaster = securityMaster;
                    _securityMaster.AccountWiseUDADataResponse += new EventHandler<EventArgs<DataSet>>(_securityMaster_AccountWiseUDADataResponse);
                    _securityMaster.ResponseCompleted += new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
                    _securityMaster.UDAAttributesResponse += new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                    _securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);

                    QueueMessage reqAccountUDAData = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_FUND_SYMBOL_UDA_REQUEST, string.Empty);
                    reqAccountUDAData.RequestID = System.Guid.NewGuid().ToString();
                    _securityMaster.SendMessage(reqAccountUDAData);

                    _securityMaster.GetAllUDAAtrributes();
                    _dictUserAccounts.Clear();
                    AccountCollection accountcollection = CachedDataManager.GetInstance.GetUserAccounts();

                    //data to be stored for permitted Accounts for the user only
                    foreach (Prana.BusinessObjects.Account account in accountcollection)
                    {
                        if (account.AccountID != int.MinValue)
                        {
                            if (!_dictUserAccounts.ContainsKey(account.AccountID))
                            {
                                _dictUserAccounts.Add(account.AccountID, account.FullName);
                            }
                            else
                            {
                                _dictUserAccounts[account.AccountID] = account.FullName;
                            }
                        }
                    }

                    FillCommonDataToValueList(_accounts, _dictUserAccounts);
                }
                else
                {
                    ultraStatusBar1.Text = "Trade server disconnected!";
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
        /// Set user based permission
        /// 
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// 
        private void SetUserPermissions()
        {
            try
            {
                if (grdData != null)
                {
                    grdData.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                    btnAddRow.Enabled = true;
                    btnSave.Enabled = true;
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

        void _securityMaster_AccountWiseUDADataResponse(object sender, EventArgs<DataSet> e)
        {
            DataSet ds = e.Value;
            try
            {
                if (ds.Tables.Count > 0)
                {
                    DataTable dtUDADataWithPermittedAccounts = new DataTable();
                    dtUDADataWithPermittedAccounts = ds.Tables[0].Clone();

                    if (!ds.Tables[0].Columns.Contains("IsDeleted"))
                    {
                        DataColumn colDelete = new DataColumn("IsDeleted");
                        colDelete.DataType = typeof(Boolean);
                        colDelete.DefaultValue = false;
                        ds.Tables[0].Columns.Add(colDelete);
                    }

                    if (ds.Tables[0].Columns.Contains("AccountID"))
                    {
                        var selectedRows = from row in ds.Tables[0].AsEnumerable()
                                           where _dictUserAccounts.ContainsKey(Convert.ToInt32(row["AccountID"].ToString()))
                                           select row;

                        if (selectedRows != null && selectedRows.Count() > 0)
                            dtUDADataWithPermittedAccounts = selectedRows.CopyToDataTable();

                        _dtAccountwiseUDAData = dtUDADataWithPermittedAccounts;
                        BindData(_dtAccountwiseUDAData);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void BindData(DataTable dt)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    AsyncDelegateRootSymbol del = new AsyncDelegateRootSymbol(BindData);
                    this.BeginInvoke(del, new object[] { dt });
                }
                else
                {
                    grdData.DataSource = dt;
                    grdData.DataBind();

                    //  DataTable userPermittedAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                    //if (userPermittedAccounts != null)
                    //{
                    //    ucbAccount.DataSource = userPermittedAccounts;
                    //    ucbAccount.DisplayLayout.Bands[0].Columns["AccountID"].Hidden = true;
                    //    ucbAccount.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;
                    //    ucbAccount.DisplayLayout.Bands[0].Columns["ThirdPartyID"].Hidden = true;
                    //}
                    if (grdData != null)
                    {
                        UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];

                        if (!gridBand.Columns.Exists("DeleteButton"))
                        {
                            UltraGridColumn btnDelete = gridBand.Columns.Add("DeleteButton");
                            btnDelete.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                            btnDelete.Header.Caption = "Delete";
                            btnDelete.NullText = "Delete";
                            btnDelete.Width = 50;
                            btnDelete.Header.VisiblePosition = 0;
                            btnDelete.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        }

                        if (!gridBand.Columns.Exists("IsDeleted"))
                        {
                            UltraGridColumn colDelete = gridBand.Columns.Add("IsDeleted");
                            colDelete.Header.Caption = "IsDeleted";
                            colDelete.Hidden = true;
                            colDelete.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                            colDelete.CellActivation = Activation.NoEdit;
                        }

                        gridBand.Columns["PrimarySymbol"].CharacterCasing = CharacterCasing.Upper;
                        gridBand.Columns["PrimarySymbol"].Width = 120;
                        gridBand.Columns["PrimarySymbol"].Header.VisiblePosition = 2;
                        gridBand.Columns["PrimarySymbol"].Header.Caption = "Symbol";


                        //if (ucbAccount.DataSource != null)
                        //{
                        //    if (!ucbAccount.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                        //    {
                        //        UltraGridColumn cbAccount = ucbAccount.DisplayLayout.Bands[0].Columns.Add();
                        //        cbAccount.Key = "Selected";
                        //        cbAccount.Header.Caption = string.Empty;
                        //        cbAccount.Width = 25;
                        //        cbAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        //        cbAccount.DataType = typeof(bool);
                        //        cbAccount.Header.VisiblePosition = 1;
                        //    }
                        //    ucbAccount.CheckedListSettings.CheckStateMember = "Selected";
                        //    ucbAccount.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                        //    ucbAccount.CheckedListSettings.ListSeparator = " , ";
                        //    ucbAccount.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                        //    ucbAccount.DisplayMember = "AccountName";
                        //    ucbAccount.ValueMember = "AccountID";
                        //    ucbAccount.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                        //}

                        UltraGridColumn colAccount = gridBand.Columns["AccountID"];
                        colAccount.Header.Caption = "Account";
                        colAccount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colAccount.ValueList = _accounts;
                        colAccount.NullText = ApplicationConstants.C_COMBO_SELECT;
                        colAccount.Header.Column.Width = 100;
                        colAccount.Header.VisiblePosition = 1;
                        //   colAccount.EditorComponent = ucbAccount;

                        UltraGridColumn colUDAAsset = gridBand.Columns["UDAAssetClassID"];
                        colUDAAsset.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colUDAAsset.Header.Caption = "Asset";
                        colUDAAsset.ValueList = _UDAAssets;
                        colUDAAsset.NullText = ApplicationConstants.C_COMBO_SELECT;
                        colUDAAsset.Header.Column.Width = 100;
                        colUDAAsset.Header.VisiblePosition = 3;

                        UltraGridColumn colUDAUDASector = gridBand.Columns["UDASectorID"];
                        colUDAUDASector.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colUDAUDASector.Header.Caption = "Sector";
                        colUDAUDASector.ValueList = _UDASectors;
                        colUDAUDASector.Header.Column.Width = 100;
                        colUDAUDASector.Header.VisiblePosition = 4;
                        colUDAUDASector.NullText = ApplicationConstants.C_COMBO_SELECT;

                        UltraGridColumn colUDASubSector = gridBand.Columns["UDASubSectorID"];
                        colUDASubSector.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colUDASubSector.Header.Caption = "SubSector";
                        colUDASubSector.ValueList = _UDASubSectors;
                        colUDASubSector.Header.Column.Width = 70;
                        colUDASubSector.Header.VisiblePosition = 5;
                        colUDASubSector.NullText = ApplicationConstants.C_COMBO_SELECT;

                        UltraGridColumn colUDASecurityType = gridBand.Columns["UDASecurityTypeID"];
                        colUDASecurityType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colUDASecurityType.Header.Caption = "Security";
                        colUDASecurityType.ValueList = _UDASecurityTypes;
                        colUDASecurityType.Header.Column.Width = 100;
                        colUDASecurityType.Header.VisiblePosition = 6;
                        colUDASecurityType.NullText = ApplicationConstants.C_COMBO_SELECT;

                        UltraGridColumn colUDACountry = gridBand.Columns["UDACountryID"];
                        colUDACountry.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colUDACountry.Header.Caption = "Country";
                        colUDACountry.ValueList = _UDACountry;
                        colUDACountry.Header.Column.Width = 80;
                        colUDACountry.Header.VisiblePosition = 7;
                        colUDACountry.NullText = ApplicationConstants.C_COMBO_SELECT;

                        gridBand.Columns["IsApproved"].Hidden = true;
                        gridBand.Columns["IsApproved"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                        gridBand.Columns["IsApproved"].CellActivation = Activation.NoEdit;
                        gridBand.Columns["ApprovedBy"].Hidden = true;
                        gridBand.Columns["ApprovedBy"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                        gridBand.Columns["ApprovedBy"].CellActivation = Activation.NoEdit;
                        gridBand.Columns["Symbol_pk"].Hidden = true;
                        gridBand.Columns["Symbol_pk"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                        gridBand.Columns["Symbol_pk"].CellActivation = Activation.NoEdit;
                        gridBand.Columns["ModifiedBy"].Hidden = true;
                        gridBand.Columns["ModifiedBy"].CellActivation = Activation.NoEdit;
                        gridBand.Columns["CreatedBy"].Hidden = true;
                        gridBand.Columns["CreatedBy"].CellActivation = Activation.NoEdit;
                        gridBand.Columns["CreationDate"].Hidden = true;
                        gridBand.Columns["CreationDate"].CellActivation = Activation.NoEdit;
                        gridBand.Columns["ModifiedDate"].Hidden = true;
                        gridBand.Columns["ModifiedDate"].CellActivation = Activation.NoEdit;

                        foreach (UltraGridRow gridRow in grdData.Rows)
                        {
                            if (gridRow.Band.Columns.Exists("PrimarySymbol") && gridRow.Band.Columns.Exists("FundID"))
                            {
                                gridRow.Cells["PrimarySymbol"].Activation = Activation.NoEdit;
                                gridRow.Cells["FundID"].Activation = Activation.NoEdit;
                            }
                        }
                    }

                    // load the saveout file if it exists
                    LoadReportSaveLayoutXML();
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

        DataRow CurrentRowInValidationProcess = null;
        System.Timers.Timer _timer = new System.Timers.Timer();
        int interval = 0;

        void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                _timer.Stop();
                if (CurrentRowInValidationProcess != null && secMasterObj != null)
                {
                    // Purpose : In case of invalid symbol from BB AUECID is 0 but symbol_pk generated 
                    //modified by amit on 18.03.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-2949
                    if (secMasterObj.AUECID > 0)
                    {
                        if (grdData.DisplayLayout.Bands[0].Columns.Exists("Symbol_pk"))
                            CurrentRowInValidationProcess["Symbol_pk"] = secMasterObj.Symbol_PK;
                    }
                }
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
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="qMsg"></param>
        void _securityMaster_ResponseCompleted(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                ultraStatusBar1.Text = e.Value.Message.ToString();
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

        void StartTimer()
        {
            // default validation time increased to 5 minutes to get response from bloomberg
            if (interval == 0)
                interval = 300000;
            _timer = new System.Timers.Timer(interval);
            _timer.AutoReset = false;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true;
        }
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (CurrentRowInValidationProcess != null)
                    CurrentRowInValidationProcess.SetColumnError("PrimarySymbol", "Symbol Not Validated !");
                CurrentRowInValidationProcess = null;
                _timer.Stop();
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="row"></param>
        public void ValidateSymbol(string text, DataRow row)
        {
            try
            {
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    SecMasterRequestObj reqObj = new SecMasterRequestObj();
                    reqObj.AddData(text, ApplicationConstants.PranaSymbology);
                    reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    reqObj.HashCode = this.GetHashCode();

                    if (CurrentRowInValidationProcess != null)
                        CurrentRowInValidationProcess.SetColumnError("PrimarySymbol", "Symbol Not Validated !");

                    CurrentRowInValidationProcess = row;

                    _securityMaster.SendRequest(reqObj);
                    StartTimer();
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

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdData != null && _dtAccountwiseUDAData.Columns.Contains("PrimarySymbol"))
                {
                    DataRow dr = _dtAccountwiseUDAData.NewRow();
                    _dtAccountwiseUDAData.Rows.Add(dr);
                    _dtAccountwiseUDAData.AcceptChanges();
                    BindData(_dtAccountwiseUDAData);
                    grdData.Focus();
                    if (grdData.Rows.Count > 0)
                    {
                        Infragistics.Win.UltraWinGrid.UltraGridRow grdRow = grdData.Rows[grdData.Rows.Count - 1];
                        if (grdRow.Band.Columns.Exists("PrimarySymbol") && grdRow.Band.Columns.Exists("FundID"))
                        {
                            grdRow.Cells["PrimarySymbol"].Activation = Activation.AllowEdit;
                            grdRow.Cells["FundID"].Activation = Activation.AllowEdit;
                        }
                        grdRow.Activate();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Prana.Utilities.MiscUtilities.PranaBinaryFormatter binaryFormatter = new Prana.Utilities.MiscUtilities.PranaBinaryFormatter();

                string errorMsg = string.Empty;
                DataTable updatedDT = new DataTable();
                updatedDT = _dtAccountwiseUDAData.Clone();

                //List<string> symbolList = new List<string>();

                errorMsg = GetUdatedData(errorMsg, updatedDT);
                if (string.IsNullOrWhiteSpace(errorMsg))
                {
                    if (updatedDT.Rows.Count > 0)
                    {
                        DataSet dsUpdatedData = new DataSet();
                        updatedDT.TableName = "Table";
                        dsUpdatedData.Tables.Add(updatedDT);
                        string saveRequest = binaryFormatter.Serialize(dsUpdatedData);
                        QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_FUND_SYMBOL_UDA_SAVE, saveRequest);
                        qMsg.RequestID = System.Guid.NewGuid().ToString();
                        _securityMaster.SendMessage(qMsg);
                        ultraStatusBar1.Text = "Data Saved : " + DateTime.Now.ToString();
                    }
                    else
                    {
                        ultraStatusBar1.Text = "Data already saved!";
                    }
                }
                else
                {
                    MessageBox.Show(errorMsg, "Prana Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private string GetUdatedData(string errorMsg, DataTable updatedDT)
        {
            try
            {
                foreach (DataRow dr in _dtAccountwiseUDAData.Rows)
                {
                    DataRowState state = dr.RowState;
                    if (state == DataRowState.Modified || state == DataRowState.Added)
                    {
                        string symbol = dr["PrimarySymbol"].ToString();

                        if (String.IsNullOrEmpty(dr["FundID"].ToString()))
                        {
                            errorMsg = "Please select a account.";
                            break;
                        }

                        if (String.IsNullOrEmpty(symbol))
                        {
                            errorMsg = "Symbol field can not be empty.";
                            break;
                        }
                        if (String.IsNullOrEmpty(dr["Symbol_pk"].ToString()))
                        {
                            errorMsg = "Symbol not valid.";
                            break;
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
                        if (string.IsNullOrWhiteSpace(dr["IsApproved"].ToString()))
                        {
                            dr["IsApproved"] = true;
                        }
                        if (string.IsNullOrWhiteSpace(dr["ModifiedBy"].ToString()))
                        {
                            dr["ModifiedBy"] = CachedDataManager.GetInstance.LoggedInUser.ShortName + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyName;
                        }
                        if (string.IsNullOrWhiteSpace(dr["ApprovedBy"].ToString()))
                        {
                            dr["ApprovedBy"] = CachedDataManager.GetInstance.LoggedInUser.ShortName + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyName;
                        }
                        if (string.IsNullOrWhiteSpace(dr["CreatedBy"].ToString()))
                        {
                            dr["CreatedBy"] = CachedDataManager.GetInstance.LoggedInUser.ShortName + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyName;
                        }

                        dr["IsDeleted"] = false;
                        updatedDT.Rows.Add(dr.ItemArray);
                        dr.AcceptChanges();
                    }
                }

                if (_dtAccountwiseUDADeletedData.Rows.Count > 0)
                {
                    foreach (DataRow dr in _dtAccountwiseUDADeletedData.Rows)
                    {
                        dr["IsDeleted"] = true;
                        updatedDT.Rows.Add(dr.ItemArray);
                        dr.AcceptChanges();
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

                    grdData.DisplayLayout.Bands[0].Columns["PrimarySymbol"].FilterComparisonType = FilterComparisonType.CaseInsensitive;
                    grdData.DisplayLayout.Bands[0].ColumnFilters["PrimarySymbol"].LogicalOperator = FilterLogicalOperator.Or;
                    grdData.DisplayLayout.Bands[0].ColumnFilters["PrimarySymbol"].FilterConditions.Add(FilterComparisionOperator.Contains, text);
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Header.Caption == "Symbol")
                {
                    string enteredSymbol = e.Cell.Row.Cells["PrimarySymbol"].Value.ToString();
                    DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                    ValidateSymbol(enteredSymbol, row);
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

        //private void UnwireEvents()
        //{
        //    try
        //    {
        //        if (_securityMaster != null)
        //        {
        //            _securityMaster.AccountWiseUDADataResponse -= new EventHandler<EventArgs<DataSet>>(_securityMaster_AccountWiseUDADataResponse);
        //            //new SymbolLookUpDataResponse(_securityMaster_AccountWiseUDADataResponse);
        //            _securityMaster.ResponseCompleted -= new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
        //            //new CompletedReceivedDelegate(_securityMaster_ResponseCompleted);
        //            _securityMaster.UDAAttributesResponse -= new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
        //            //new UDAAttributesDataResponse(_securityMaster_UDAAttributesResponse);
        //            _securityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<ISecMasterBase>>(_securityMaster_SecMstrDataResponse);
        //            //new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);
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
        //}

        /// <summary>
        /// Load report layout xml if file exist
        /// 
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void LoadReportSaveLayoutXML()
        {
            try
            {
                _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _accountWiseUDALayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
                _accountWiseUDAFilePath = _accountWiseUDALayoutDirectoryPath + @"\AccountWiseUDALayout.xml";

                if (!Directory.Exists(_accountWiseUDALayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_accountWiseUDALayoutDirectoryPath);
                }
                if (File.Exists(_accountWiseUDAFilePath))
                {
                    grdData.DisplayLayout.LoadFromXml(_accountWiseUDAFilePath);
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

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdData != null)
                {
                    if (grdData.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(_accountWiseUDAFilePath))
                        {
                            grdData.DisplayLayout.SaveAsXml(_accountWiseUDAFilePath);
                            MessageBox.Show(this, "Layout Saved.", "Account Wise UDA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdData_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridRow row = e.Cell.Row;
                if (e.Cell.Column.Key == "DeleteButton")
                {
                    if (!string.IsNullOrEmpty(row.Cells["FundID"].Value.ToString()))
                    {
                        DataRowView deleteRow = (DataRowView)row.ListObject;
                        _dtAccountwiseUDADeletedData = _dtAccountwiseUDAData.Clone();
                        _dtAccountwiseUDADeletedData.Rows.Add(deleteRow.Row.ItemArray);
                        _dtAccountwiseUDAData.Rows.Remove(deleteRow.Row);
                    }
                }
                //e.Cell.Row.Delete(false);
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
    }
}
