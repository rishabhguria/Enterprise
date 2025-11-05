using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Import.BAL;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prana.Import.Controls
{
    public partial class ctrlImportReport : UserControl
    {

        #region variables
        Form _frmSymbolMismatch;
        ctrlSymbolMismatch crlSymbolMismatch = null;
        public int _executionID = int.MinValue;
        private List<UltraGridRow> _selectedColumnList = new List<UltraGridRow>();
        static int _userID = int.MinValue;
        static string _importReportFilePath = string.Empty;
        static string _importReportLayoutDirectoryPath = string.Empty;
        //static ImportReportLayout _importReportLayout = null;
        public static event EventHandler launchForm;
        TaskResult _taskResult = null;
        RunUpload _runUpload = null;
        ImportType _importType;
        public Dictionary<string, string> _dictReportData { get; set; }
        //public static ImportReportLayout ImportReportLayout
        //{
        //    get
        //    {
        //        if (_importReportLayout == null)
        //        {
        //            _importReportLayout = GetImportReportLayout();
        //        }
        //        return _importReportLayout;
        //    }
        //}

        #endregion
        public ctrlImportReport()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnImport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnImport.ForeColor = System.Drawing.Color.White;
                btnImport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnImport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnImport.UseAppStyling = false;
                btnImport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// File the Control with the details 
        /// </summary>
        /// <param name="dictReport">Dictionary of details</param>
        internal void FillReport()
        {
            try
            {

                //check if partial imported is to be done else hide the partial import button.
                if (_taskResult == null || _runUpload == null)
                {
                    splitContainer2.Panel2Collapsed = true;
                    // Status bar needs to be displayed only when it is opened from dashboard UI.
                    splitContainer3.Panel2Collapsed = true;
                }
                else
                {
                    // Initially, no message is needed on the Status bar.
                    ultraStatusBar1.Text = String.Empty;
                }

                SecMasterHelper.getInstance().GetAllDefaults();
                if (_dictReportData.ContainsKey("Date"))
                {
                    if (!string.IsNullOrEmpty(_dictReportData["Date"].ToString()))
                    {
                        txtStartDate.Text = DateTime.Parse(_dictReportData["Date"]).ToShortDateString();
                    }
                }
                if (_dictReportData.ContainsKey("FileType"))
                {
                    if (!string.IsNullOrEmpty(_dictReportData["FileType"].ToString()))
                    {
                        txtFileType.Text = _dictReportData["FileType"];
                        _importType = (ImportType)Enum.Parse(typeof(ImportType), _dictReportData["FileType"].ToString(), true);
                    }
                }
                if (_dictReportData.ContainsKey("AccountCount"))
                {
                    if (!string.IsNullOrEmpty(_dictReportData["AccountCount"].ToString()))
                    {
                        txtAccount.Text = _dictReportData["AccountCount"];
                    }
                }
                if (_dictReportData.ContainsKey("NonValidatedSymbols"))
                {
                    if (!string.IsNullOrEmpty(_dictReportData["NonValidatedSymbols"].ToString()))
                    {
                        txtSymFail.Text = _dictReportData["NonValidatedSymbols"];
                    }
                }
                if (_dictReportData.ContainsKey("PendingSymbols"))
                {
                    if (!string.IsNullOrEmpty(_dictReportData["PendingSymbols"].ToString()))
                    {
                        txtSymPending.Text = _dictReportData["PendingSymbols"];
                    }
                }
                if (_dictReportData.ContainsKey("ValidatedSymbols"))
                {
                    if (!string.IsNullOrEmpty(_dictReportData["ValidatedSymbols"].ToString()))
                    {
                        txtSymValid.Text = _dictReportData["ValidatedSymbols"];
                    }
                }
                if (_dictReportData.ContainsKey("ThirdPartyType"))
                {
                    if (!string.IsNullOrEmpty(_dictReportData["ThirdPartyType"].ToString()))
                    {
                        txtThirdParty.Text = _dictReportData["ThirdPartyType"];
                    }
                }
                if (_dictReportData.ContainsKey("TotalSymbols"))
                {
                    if (!string.IsNullOrEmpty(_dictReportData["TotalSymbols"].ToString()))
                    {
                        txttotalSymbol.Text = _dictReportData["TotalSymbols"];
                    }
                }
                string xmlFile = _dictReportData["FileName"];

                if (File.Exists(xmlFile))
                {
                    ultraStatusBar1.Text = "Please wait, data is loading...";
                    btnImport.Enabled = false;
                    BackgroundWorker bgw = new BackgroundWorker();
                    bgw.WorkerReportsProgress = true;
                    bgw.WorkerSupportsCancellation = true;
                    bgw.DoWork += bgw_DoWork;
                    bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
                    bgw.ProgressChanged += bgw_ProgressChanged;
                    object[] args = new object[1];
                    args[0] = xmlFile;
                    bgw.RunWorkerAsync(args);

                }
                else
                {
                    btnImport.Enabled = true;
                    ultraStatusBar1.Text = "File not found.";
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

        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ultraStatusBar1.Text = (e.ProgressPercentage.ToString() + "%");
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)
                {
                    DataSet ds = e.Result as DataSet;
                    if (ds != null)
                    {
                        // ds.ReadXml(xmlFile);

                        ds.Relations.Clear();
                        if (ds.Tables.Count > 0)
                        {
                            grdReport.DataSource = ds.Tables[0];
                            //set the header checkbox intially to uncheck state
                            if (grdReport.DisplayLayout.Bands[0].Columns.Exists("Select"))
                            {
                                grdReport.DisplayLayout.Bands[0].Columns["Select"].SetHeaderCheckedState(grdReport.Rows, CheckState.Unchecked);
                            }
                            btnImport.Enabled = true;
                            ultraStatusBar1.Text = "Done!";
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

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                object[] agrs = e.Argument as object[];
                string xmlFile = agrs[0] as string;
                if (xmlFile != null)
                {
                    DataSet ds = new DataSet();
                    using (FileStream filestream = File.OpenRead(xmlFile))
                    {
                        ds.EnforceConstraints = false;
                        BufferedStream buffered = new BufferedStream(filestream);
                        ds.ReadXml(buffered);
                        ds.EnforceConstraints = true;
                        worker.ReportProgress(50);
                    }
                    e.Result = ds;
                }
                else
                {
                    e.Cancel = true;
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

        private void grdReport_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraWinGridUtils.EnableFixedFilterRow(e);

                #region column chooser

                //Set the HeaderCheckBoxVisibility so it will display the CheckBox whenever a CheckEditor is used within the UltraGridColumn 
                grdReport.DisplayLayout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                //Set the HeaderCheckBoxAlignment so the CheckBox will appear to the Right of the caption. 
                grdReport.DisplayLayout.Override.HeaderCheckBoxAlignment = HeaderCheckBoxAlignment.Right;
                //Set the HeaderCheckBoxSynchronization so all rows within the GridBand will be synchronized with the CheckBox 
                grdReport.DisplayLayout.Override.HeaderCheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                // Set the RowSelectorHeaderStyle to ColumnChooserButton.
                grdReport.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                // Enable the RowSelectors. This is necessary because the column chooser
                // button is displayed over the row selectors in the column headers area.
                grdReport.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;

                #endregion

                UltraGridBand grdDataBand = null;
                grdDataBand = grdReport.DisplayLayout.Bands[0];

                SetGridColumns(grdDataBand);

                // load the saveout file if it exists
                LoadReportSaveLayoutXML();

                #region Add CheckBox Column

                if (!grdDataBand.Columns.Exists("Select"))
                {
                    grdDataBand.Columns.Add("Select", "Select");
                }
                UltraGridColumn colSelect = grdDataBand.Columns["Select"];
                colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                colSelect.Width = 50;
                colSelect.Header.Caption = "";
                colSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                colSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                colSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                colSelect.Header.VisiblePosition = 0;
                colSelect.CellActivation = Activation.AllowEdit;
                colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colSelect.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                #endregion
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
        private void grdReport_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //Commented by omshiv, TODO - Optimize painting here
                //if (e.Row.Band.Columns.Exists(ApplicationConstants.CONST_SEC_APPROVED_STATUS))
                //{
                //    UltraGridCell cell = e.Row.Cells[ApplicationConstants.CONST_SEC_APPROVED_STATUS];
                //    cell.Value = cell.Text;
                //    if (cell.Text.ToString() == "UnApproved")
                //    {
                //        foreach (UltraGridCell cel in e.Row.Cells)
                //        {
                //            cel.Appearance.ForeColor = Color.Red;
                //        }
                //    }
                //}

                //if (e.Row.Band.Columns.Exists(ApplicationConstants.CONST_VALIDATION_STATUS))
                //{
                //    UltraGridCell cell = e.Row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS];
                //    cell.Value = cell.Text;
                //    if (cell.Text.ToString() == "UnApproved")
                //    {
                //        foreach (UltraGridCell cel in e.Row.Cells)
                //        {
                //            cel.Appearance.ForeColor = Color.Red;
                //        }
                //    }
                //}

                if (e.Row.Band.Columns.Exists("ExecutingBroker") && e.Row.Band.Columns.Exists("CounterPartyID"))
                {
                    UltraGridCell cell = e.Row.Cells["ExecutingBroker"];
                    int counterPartyID = int.Parse(e.Row.Cells["CounterPartyID"].Value.ToString());
                    if (counterPartyID <= 0)
                    {
                        cell.Appearance.ForeColor = Color.Red;
                        cell.ActiveAppearance.ForeColor = Color.Red;
                        if (e.Row.Band.Columns.Exists("CreateCounterParty"))
                            e.Row.Cells["CreateCounterParty"].Activation = Activation.AllowEdit;
                    }
                    else
                    {
                        cell.Appearance.ForeColor = Color.Green;
                        cell.ActiveAppearance.ForeColor = Color.Green;
                        if (e.Row.Band.Columns.Exists("CreateCounterParty"))
                            e.Row.Cells["CreateCounterParty"].Activation = Activation.NoEdit;
                    }
                }
                //if (e.Row.Band.Columns.Exists("AccountName") && e.Row.Band.Columns.Exists("AccountID"))
                //{
                //    int accountID = int.Parse(e.Row.Cells["AccountID"].Value.ToString());
                //    UltraGridCell cell = e.Row.Cells["AccountName"];
                //    if (accountID <= 0)
                //    {
                //        cell.Appearance.ForeColor = Color.Red;
                //        cell.ActiveAppearance.ForeColor = Color.Red;
                //    }
                //    else
                //    {
                //        cell.Appearance.ForeColor = Color.Green;
                //        cell.ActiveAppearance.ForeColor = Color.Green;
                //    }
                //}
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

        private void grdReport_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                //if (e.Cell.GetType() == typeof(UltraGridFilterCell))
                //{
                //    return;
                //}
                //if (e.Cell.Row.Band.Columns.Exists(ApplicationConstants.CONST_SEC_APPROVED_STATUS))
                //{
                //    UltraGridCell cell = e.Cell.Row.Cells[ApplicationConstants.CONST_SEC_APPROVED_STATUS];
                //    cell.Value = cell.Text;
                //    if (cell.Text.ToString() == "Approved")
                //    {
                //        foreach (UltraGridCell cel in e.Cell.Row.Cells)
                //        {
                //            cel.Appearance.ForeColor = Color.Green;
                //        }
                //    }
                //}

                //if (e.Cell.Row.Band.Columns.Exists(ApplicationConstants.CONST_VALIDATION_STATUS))
                //{
                //    UltraGridCell cell = e.Cell.Row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS];
                //    cell.Value = cell.Text;
                //    if (cell.Text.ToString() == "Approved")
                //    {
                //        foreach (UltraGridCell cel in e.Cell.Row.Cells)
                //        {
                //            cel.Appearance.ForeColor = Color.Green;
                //        }
                //    }
                //}
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
        #region commented
        //private void Update(DataSet ds)
        //{
        //    try
        //    {
        //        if (ds == null || ds.Tables.Count == 0)
        //        {
        //            //toolStripStatusLabel1.Text = "No data found. Please refine your search!";
        //            return;
        //        }
        //        else
        //        {
        //            _secMasterUIobj.Clear();

        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                SecMasterUIObj secMasterUIobj = new SecMasterUIObj();
        //                Transformer.CreateObjThroughReflection(dr, secMasterUIobj);
        //                AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)secMasterUIobj.AssetID);

        //                switch (baseAssetCategory)
        //                {
        //                    case BusinessObjects.AppConstants.AssetCategory.Option:

        //                        DateTime expirationDate = DateTimeConstants.MinValue; ;
        //                        DateTime.TryParse(dr["OPTExpiration"].ToString(), out expirationDate);
        //                        secMasterUIobj.ExpirationDate = expirationDate;
        //                        if (dr.Table.Columns.Contains("OptionName") && dr["OptionName"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.LongName = dr["OptionName"].ToString();
        //                        }
        //                        if (dr.Table.Columns.Contains("Type") && dr["Type"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.PutOrCall = (Convert.ToInt32(dr["Type"]));
        //                        }
        //                        if (dr.Table.Columns.Contains("OSISymbol") && dr["OSISymbol"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.OSIOptionSymbol = dr["OSISymbol"].ToString();
        //                        }
        //                        if (dr.Table.Columns.Contains("IDCOSymbol") && dr["IDCOSymbol"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.IDCOOptionSymbol = dr["IDCOSymbol"].ToString();
        //                        }
        //                        if (dr.Table.Columns.Contains("OPRASymbol") && dr["OPRASymbol"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.OPRAOptionSymbol = dr["OPRASymbol"].ToString();
        //                        }
        //                        if (dr.Table.Columns.Contains("OPTMultiplier") && dr["OPTMultiplier"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.Multiplier = double.Parse(dr["OPTMultiplier"].ToString());
        //                        }

        //                        break;

        //                    case BusinessObjects.AppConstants.AssetCategory.Future:

        //                        if ((AssetCategory)secMasterUIobj.AssetID == AssetCategory.FXForward)
        //                        {
        //                            secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["FUTExpiration"].ToString());
        //                            if (dr.Table.Columns.Contains("FxContractName") && dr["FxContractName"] != System.DBNull.Value)
        //                            {
        //                                secMasterUIobj.LongName = dr["FxContractName"].ToString();
        //                            }
        //                            if (dr.Table.Columns.Contains("FxForwardMultiplier") && dr["FxForwardMultiplier"] != System.DBNull.Value)
        //                            {
        //                                secMasterUIobj.Multiplier = double.Parse(dr["FxForwardMultiplier"].ToString());
        //                            }
        //                            if (dr.Table.Columns.Contains("IsNDF") && dr["IsNDF"] != System.DBNull.Value)
        //                            {
        //                                secMasterUIobj.IsNDF = Convert.ToBoolean(dr["IsNDF"].ToString());
        //                            }
        //                            if (dr.Table.Columns.Contains("FixingDate") && dr["FixingDate"] != System.DBNull.Value)
        //                            {
        //                                secMasterUIobj.FixingDate = Convert.ToDateTime(dr["FixingDate"].ToString());
        //                            }
        //                        }
        //                        else
        //                        {
        //                            secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["FUTExpiration"].ToString());
        //                            if (dr.Table.Columns.Contains("FUTMultiplier") && dr["FUTMultiplier"] != System.DBNull.Value)
        //                            {
        //                                secMasterUIobj.Multiplier = double.Parse(dr["FUTMultiplier"].ToString());
        //                            }
        //                            if (dr.Table.Columns.Contains("FutureName") && dr["FutureName"] != System.DBNull.Value)
        //                            {
        //                                secMasterUIobj.LongName = dr["FutureName"].ToString();
        //                            }
        //                        }

        //                        break;
        //                    case BusinessObjects.AppConstants.AssetCategory.FX:

        //                        if (dr.Table.Columns.Contains("FxContractName") && dr["FxContractName"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.LongName = dr["FxContractName"].ToString();
        //                        }
        //                        if (dr.Table.Columns.Contains("FxMultiplier") && dr["FxMultiplier"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.Multiplier = Convert.ToDouble(dr["FxMultiplier"].ToString());
        //                        }
        //                        if (dr.Table.Columns.Contains("IsNDF") && dr["IsNDF"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.IsNDF = Convert.ToBoolean(dr["IsNDF"].ToString());
        //                        }
        //                        if (dr.Table.Columns.Contains("FixingDate") && dr["FixingDate"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.FixingDate = Convert.ToDateTime(dr["FixingDate"].ToString());
        //                        }
        //                        if (dr.Table.Columns.Contains("FxExpirationDate") && dr["FxExpirationDate"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["FxExpirationDate"].ToString());
        //                        }
        //                        else
        //                        {

        //                        }
        //                        break;
        //                    case BusinessObjects.AppConstants.AssetCategory.Indices:
        //                        if (dr.Table.Columns.Contains("IndexLongName") && dr["IndexLongName"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.LongName = dr["IndexLongName"].ToString();
        //                        }
        //                        break;
        //                    case BusinessObjects.AppConstants.AssetCategory.FixedIncome:
        //                        {
        //                            if (dr.Table.Columns.Contains("FixedIncomeLongName") && dr["FixedIncomeLongName"] != System.DBNull.Value)
        //                            {
        //                                secMasterUIobj.LongName = dr["FixedIncomeLongName"].ToString();
        //                            }
        //                            if (dr.Table.Columns.Contains("FIMultiplier") && dr["FIMultiplier"] != System.DBNull.Value)
        //                            {
        //                                secMasterUIobj.Multiplier = Convert.ToDouble(dr["FIMultiplier"].ToString());
        //                            }
        //                            if (dr.Table.Columns.Contains("MaturityDate") && dr["MaturityDate"] != System.DBNull.Value)
        //                            {
        //                                secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["MaturityDate"].ToString());
        //                            }
        //                            break;
        //                            //if (dr["CouponFrequency"] != System.DBNull.Value)
        //                            //{
        //                            //    secMasterUIobj.FreqID = Convert.ToInt32(dr["CouponFrequency"].ToString());
        //                            //}
        //                        }

        //                }


        //                _secMasterUIobj.Insert(_secMasterUIobj.Count, secMasterUIobj);// can use Add but left it unchanged

        //            }

        //            //TODO- Use paging contraol instead of getting data by Index, Check server side paging available or not. -om
        //            StringBuilder msgSB = new StringBuilder();
        //            if (_secMasterUIobj.Count == 0)
        //            {

        //            }
        //            else
        //            {

        //            }

        //            grdReport.DataSource = null;
        //            grdReport.DataSource = _secMasterUIobj;
        //            grdReport.DataBind();

        //            //set grid Columns according to selected View show all columns or only UDA - omshiv, march 2014
        //            //  cmbbxSMView_ValueChanged(this, null);

        //            // timer.Stop();
        //            // set column settings based on first Row Asset Type
        //            if (grdReport.Rows.Count > 0)
        //            {
        //                FxColumnSettings(grdReport.Rows[0]);
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

        //private void FxColumnSettings(UltraGridRow row)
        //{
        //    try
        //    {
        //        UltraGridColumn colLeadCurr = grdReport.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_LeadCurrencyID];
        //        UltraGridColumn colVsCurr = grdReport.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_VsCurrencyID];

        //        int assetID = int.Parse(row.Cells[OrderFields.PROPERTY_ASSET_ID].Value.ToString());
        //        AssetCategory assetCategory = (AssetCategory)assetID;

        //        if (assetCategory == AssetCategory.FX)
        //        {
        //            colLeadCurr.Header.VisiblePosition = 5;
        //            colVsCurr.Header.VisiblePosition = 6;
        //            colLeadCurr.Hidden = false;
        //            colVsCurr.Hidden = false;
        //        }
        //        else if (assetCategory == AssetCategory.FXForward)
        //        {
        //            colLeadCurr.Header.VisiblePosition = 5;
        //            colVsCurr.Header.VisiblePosition = 6;

        //            colLeadCurr.Hidden = false;
        //            colVsCurr.Hidden = false;

        //            UltraGridColumn colExpSett = grdReport.DisplayLayout.Bands[0].Columns["ExpirationDate"];
        //            colExpSett.Header.VisiblePosition = 7;
        //            colExpSett.Hidden = false;

        //            UltraGridColumn colUnderlyingSymbol = grdReport.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL];
        //            colUnderlyingSymbol.Header.VisiblePosition = 8;
        //            colUnderlyingSymbol.Hidden = false;
        //        }
        //        else
        //        {
        //            colLeadCurr.Hidden = true;
        //            colVsCurr.Hidden = true;
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

        private void SetGridColumns(UltraGridBand gridBand)
        {

            try
            {
                //int visiblePosition = 0;

                foreach (UltraGridColumn column in gridBand.Columns)
                {
                    column.CellActivation = Activation.ActivateOnly;
                    column.Hidden = true;
                    column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.IncludeHeader);
                }

                AddCheckBoxInGrid(gridBand);

                if (!string.IsNullOrEmpty(_importType.ToString()))
                {
                    List<Prana.Import.BAL.ImportReportColumnDetails.SetColumnPropFunction> listFuncToCall = ImportReportColumnDetails.Instance.InitializeData(_importType);
                    foreach (Prana.Import.BAL.ImportReportColumnDetails.SetColumnPropFunction deleg in listFuncToCall)
                    {
                        deleg.Invoke(gridBand);
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



        private static void AddCheckBoxInGrid(UltraGridBand gridBand)
        {
            try
            {
                if (!gridBand.Columns.Exists("Select"))
                {
                    gridBand.Columns.Add("Select", "Select");
                }
                UltraGridColumn colSelect = gridBand.Columns["Select"];
                colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                colSelect.Header.Caption = "";
                colSelect.DataType = typeof(Boolean);
                colSelect.CellActivation = Activation.AllowEdit;
                colSelect.Header.VisiblePosition = 0;
                colSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                colSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                colSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                colSelect.AllowRowFiltering = DefaultableBoolean.False;
                colSelect.Hidden = false;
                colSelect.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
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

        //private ValueList GetSideValueLists()
        //{
        //    #region side value lists
        //    ValueList valueListSides = new ValueList();
        //    try
        //    {
        //        valueListSides.ValueListItems.Clear();
        //        valueListSides.ValueListItems.Add(FIXConstants.SIDE_Buy, "Buy");
        //        valueListSides.ValueListItems.Add(FIXConstants.SIDE_Buy_Closed, "Buy to Close");
        //        valueListSides.ValueListItems.Add(FIXConstants.SIDE_Buy_Open, "Buy to Open");
        //        valueListSides.ValueListItems.Add(FIXConstants.SIDE_Sell, "Sell");
        //        valueListSides.ValueListItems.Add(FIXConstants.SIDE_Sell_Closed, "Sell to Close");
        //        valueListSides.ValueListItems.Add(FIXConstants.SIDE_Sell_Open, "Sell to Open");
        //        valueListSides.ValueListItems.Add(FIXConstants.SIDE_SellShort, "Sell short");
        //    }
        //    catch (Exception ex)
        //    {

        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return valueListSides;
        //    #endregion
        //}


        private void saveLayoutMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdReport != null)
                {
                    if (grdReport.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(_importReportFilePath))
                        {
                            grdReport.DisplayLayout.SaveAsXml(_importReportFilePath);
                            MessageBox.Show(this, "Layout Saved.", "Import Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //string filePath = @"D:\Nirvana\NirvanaCode\SourceCode\Dev\Prana\Application\Prana.Client\Prana\bin\Debug\ReconData";
                UltraGridFileExporter.LoadFilePathAndExport(grdReport, this);
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

        #region Layout Functions
        /// <summary>
        /// Function Saves the Layout
        /// </summary>
        //public void SaveGridsLayout()
        //{
        //    try
        //    {
        //        if (grdReport != null)
        //        {
        //            if (grdReport.DisplayLayout.Bands[0].Columns.Count > 0)
        //            {

        //                ImportReportLayout.ImportReportColumns = GetGridColumnLayout(grdReport);
        //            }
        //        }

        //        SaveImportReportLayout();
        //    }
        //    catch (Exception ex)
        //    {
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Returns the Layout as read from the Xml
        /// </summary>
        /// <returns></returns>
        //private static ImportReportLayout GetImportReportLayout()
        //{
        //    _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
        //    _importReportLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID + @"\ImportReportLayout\";
        //    string LayoutXMLFileName = "Transaction" + "ImportReportLayout.xml";
        //    _importReportFilePath = _importReportLayoutDirectoryPath + @"\" + LayoutXMLFileName;

        //    ImportReportLayout importLayout = new ImportReportLayout();
        //    try
        //    {
        //        if (!Directory.Exists(_importReportLayoutDirectoryPath))
        //        {
        //            Directory.CreateDirectory(_importReportLayoutDirectoryPath);
        //        }
        //        if (File.Exists(_importReportFilePath))
        //        {
        //            using (FileStream fs = File.OpenRead(_importReportFilePath))
        //            {
        //                XmlSerializer serializer = new XmlSerializer(typeof(ImportReportLayout));
        //                importLayout = (ImportReportLayout)serializer.Deserialize(fs);
        //            }
        //        }

        //        _importReportLayout = importLayout;
        //    }
        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion

        //    return importLayout;
        //}
        /// <summary>
        /// Function Returns a list of Columns of Grid grdReport with Properties as set.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        //public static List<ColumnData> GetGridColumnLayout(UltraGrid grid)
        //{
        //    List<ColumnData> listGridCols = new List<ColumnData>();
        //    UltraGridBand band = grid.DisplayLayout.Bands[0];
        //    try
        //    {
        //        foreach (UltraGridColumn gridCol in band.Columns)
        //        {
        //            ColumnData colData = new ColumnData();
        //            colData.Key = gridCol.Key;
        //            colData.Caption = gridCol.Header.Caption;
        //            colData.Format = gridCol.Format;
        //            colData.Hidden = gridCol.Hidden;
        //            colData.VisiblePosition = gridCol.Header.VisiblePosition;
        //            colData.Width = gridCol.Width;
        //            colData.ExcludeFromColumnChooser = gridCol.ExcludeFromColumnChooser;
        //            colData.IsGroupByColumn = gridCol.IsGroupByColumn;
        //            colData.Fixed = gridCol.Header.Fixed;
        //            colData.CellActivation = gridCol.CellActivation;

        //            // Sorted Columns
        //            colData.SortIndicator = gridCol.SortIndicator;

        //            //// Summary Settings
        //            //if (band.Summaries.Exists(gridCol.Key))
        //            //{
        //            //    string colSummKey = band.Summaries[gridCol.Key].CustomSummaryCalculator.ToString();
        //            //    colData.ColSummaryKey = (colSummKey.Contains(".")) ? colSummKey.Split('.')[2] : String.Empty;
        //            //    colData.ColSummaryFormat = band.Summaries[gridCol.Key].DisplayFormat;
        //            //}

        //            //Filter Settings
        //            foreach (FilterCondition fCond in band.ColumnFilters[gridCol.Key].FilterConditions)
        //            {
        //                colData.FilterConditionList.Add(fCond);
        //            }
        //            colData.FilterLogicalOperator = band.ColumnFilters[gridCol.Key].LogicalOperator;

        //            listGridCols.Add(colData);
        //        }
        //    }
        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion
        //    return listGridCols;
        //}

        /// <summary>
        /// Function Writes to the XMl the Layout(Columns and associated Properties) as User is using
        /// </summary>
        //public static void SaveImportReportLayout()
        //{
        //    try
        //    {

        //        using (XmlTextWriter writer = new XmlTextWriter(_importReportFilePath, Encoding.UTF8))
        //        {
        //            writer.Formatting = Formatting.Indented;
        //            XmlSerializer serializer;
        //            serializer = new XmlSerializer(typeof(ImportReportLayout));
        //            serializer.Serialize(writer, _importReportLayout);

        //            writer.Flush();
        //            writer.Close();
        //        }
        //    }
        //    #region catch
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion
        //}

        /// <summary>
        /// Function Sets the Grid Layout as it reads from the List of Columns Layout which are Columns read from XML
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="listColData"></param>
        //public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        //{
        //    List<ColumnData> listSortedGridCols = new List<ColumnData>();
        //    UltraGridBand band = grid.DisplayLayout.Bands[0];
        //    ColumnsCollection gridColumns = band.Columns;// Just for readability ;)
        //    listColData.Sort();

        //    try
        //    {

        //        // Hide All
        //        foreach (UltraGridColumn gridCol in gridColumns)
        //        {
        //            gridCol.Hidden = true;
        //        }

        //        //Set Columns Properties
        //        foreach (ColumnData colData in listColData)
        //        {
        //            if (gridColumns.Exists(colData.Key))
        //            {
        //                UltraGridColumn gridCol = gridColumns[colData.Key];
        //                gridCol.Width = colData.Width;
        //                gridCol.Format = colData.Format;
        //                gridCol.Header.Caption = colData.Caption;
        //                gridCol.Header.VisiblePosition = colData.VisiblePosition;
        //                gridCol.Hidden = colData.Hidden;
        //                gridCol.ExcludeFromColumnChooser = colData.ExcludeFromColumnChooser;
        //                gridCol.Header.Fixed = colData.Fixed;
        //                gridCol.SortIndicator = colData.SortIndicator;
        //                gridCol.CellActivation = Activation.NoEdit;

        //                // Sorted Columns
        //                if (colData.SortIndicator == SortIndicator.Descending || colData.SortIndicator == SortIndicator.Ascending)
        //                {
        //                    listSortedGridCols.Add(colData);
        //                }

        //                //Summary Settings
        //                //if (colData.ColSummaryKey != String.Empty)
        //                //{
        //                //    SummarySettings summary = band.Summaries.Add(gridCol.Key, SummaryType.Custom, riskSummFactory.GetSummaryCalculator(colData.ColSummaryKey), gridCol, SummaryPosition.UseSummaryPositionColumn, gridCol);
        //                //    summary.DisplayFormat = colData.ColSummaryFormat;
        //                //}

        //                // Filter Settings
        //                if (colData.FilterConditionList.Count > 0)
        //                {
        //                    band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
        //                    foreach (FilterCondition fCond in colData.FilterConditionList)
        //                    {
        //                        band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion

        //    // Sorted Columns are returned as they need to be handled after data is binded.
        //    //  return listSortedGridCols;
        //}
        #endregion

        /// <summary>
        /// Open the new UI on Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdReport_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "MismatchType")
                {
                    #region set variables

                    List<string> msg = new List<string>();
                    if (e.Cell.Row.Cells.Exists("MismatchDetails"))
                    {
                        msg = new List<string>(e.Cell.Row.Cells["MismatchDetails"].Value.ToString().Split('~'));
                    }
                    string ticker = string.Empty;
                    string uticker = string.Empty;
                    string CUSIP = string.Empty;
                    string uCUSIP = string.Empty;
                    string ISIN = string.Empty;
                    string uISIN = string.Empty;
                    string SEDOL = string.Empty;
                    string uSEDOL = string.Empty;
                    string BBCode = string.Empty;
                    string uBBCode = string.Empty;
                    string RIC = string.Empty;
                    string uRIC = string.Empty;
                    string OSIOptionSymbol = string.Empty;
                    string uOSIOptionSymbol = string.Empty;
                    string IDCOOptionSymbol = string.Empty;
                    string uIDCOOptionSymbol = string.Empty;
                    string OpraOptionSymbol = string.Empty;
                    string uOpraOptionSymbol = string.Empty;
                    string Multiplier = string.Empty;
                    string uMultiplier = string.Empty;
                    string Price = string.Empty;
                    string uPrice = string.Empty;
                    string currency = string.Empty;
                    string uCurrency = string.Empty;
                    if (e.Cell.Row.Cells.Exists("Symbol"))
                    {
                        ticker = e.Cell.Row.Cells["Symbol"].Value.ToString();
                        uticker = string.Empty;
                    }
                    if (e.Cell.Row.Cells.Exists("CUSIP"))
                    {
                        CUSIP = e.Cell.Row.Cells["CUSIP"].Value.ToString();
                        uCUSIP = string.Empty;

                    }
                    if (e.Cell.Row.Cells.Exists("ISIN"))
                    {
                        ISIN = e.Cell.Row.Cells["ISIN"].Value.ToString();
                        uISIN = string.Empty;

                    }
                    if (e.Cell.Row.Cells.Exists("SEDOL"))
                    {
                        SEDOL = e.Cell.Row.Cells["SEDOL"].Value.ToString();
                        uSEDOL = string.Empty;

                    }
                    if (e.Cell.Row.Cells.Exists("Bloomberg"))
                    {
                        BBCode = e.Cell.Row.Cells["Bloomberg"].Value.ToString();
                        uBBCode = string.Empty;

                    }
                    if (e.Cell.Row.Cells.Exists("RIC"))
                    {
                        RIC = e.Cell.Row.Cells["RIC"].Value.ToString();
                        uRIC = string.Empty;

                    }
                    if (e.Cell.Row.Cells.Exists("OSIOptionSymbol"))
                    {
                        OSIOptionSymbol = e.Cell.Row.Cells["OSIOptionSymbol"].Value.ToString();
                        uOSIOptionSymbol = string.Empty;

                    }
                    if (e.Cell.Row.Cells.Exists("IDCOOptionSymbol"))
                    {
                        IDCOOptionSymbol = e.Cell.Row.Cells["IDCOOptionSymbol"].Value.ToString();
                        uIDCOOptionSymbol = string.Empty;

                    }
                    if (e.Cell.Row.Cells.Exists("OpraOptionSymbol"))
                    {
                        OpraOptionSymbol = e.Cell.Row.Cells["OpraOptionSymbol"].Value.ToString();
                        uOpraOptionSymbol = string.Empty;

                    }
                    if (e.Cell.Row.Cells.Exists("Multiplier"))
                    {
                        Multiplier = e.Cell.Row.Cells["Multiplier"].Value.ToString();
                        uMultiplier = string.Empty;

                    }
                    if (e.Cell.Row.Cells.Exists("MarkPrice"))
                    {
                        Price = e.Cell.Row.Cells["MarkPrice"].Value.ToString();
                        uPrice = string.Empty;

                    }
                    if (e.Cell.Row.Cells.Exists("CurrencyID"))
                    {
                        //modified by amit 12.03.2015 ,checking if currency with specified key exist in the dictionary 
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-2899
                        Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                        if (e.Cell.Row.Cells["CurrencyID"].Value != null && dictCurrencies.ContainsKey(Convert.ToInt32(e.Cell.Row.Cells["CurrencyID"].Value)))
                        {
                            currency = dictCurrencies[Convert.ToInt32(e.Cell.Row.Cells["CurrencyID"].Value)].ToString();
                        }
                        uCurrency = string.Empty;
                    }
                    if (e.Cell.Text.Contains("Mismatch"))
                    {
                        if (msg.Contains("CUSIP"))
                        {
                            uCUSIP = msg[msg.IndexOf("CUSIP") + 1];
                        }
                        if (msg.Contains("ISIN"))
                        {
                            uISIN = msg[msg.IndexOf("ISIN") + 1];
                        }
                        if (msg.Contains("SEDOL"))
                        {
                            uSEDOL = msg[msg.IndexOf("SEDOL") + 1];
                        }
                        if (msg.Contains("BBCode"))
                        {
                            uBBCode = msg[msg.IndexOf("BBCode") + 1];
                        }
                        if (msg.Contains("RIC"))
                        {
                            uRIC = msg[msg.IndexOf("RIC") + 1];
                        }
                        if (msg.Contains("OSIOptionSymbol"))
                        {
                            uOSIOptionSymbol = msg[msg.IndexOf("OSIOptionSymbol") + 1];
                        }
                        if (msg.Contains("IDCOOptionSymbol"))
                        {
                            uIDCOOptionSymbol = msg[msg.IndexOf("IDCOOptionSymbol") + 1];
                        }
                        if (msg.Contains("OpraOptionSymbol"))
                        {
                            uOpraOptionSymbol = msg[msg.IndexOf("OpraOptionSymbol") + 1];
                        }
                        if (msg.Contains("Multiplier"))
                        {
                            uMultiplier = msg[msg.IndexOf("Multiplier") + 1];
                        }
                        if (msg.Contains("Price"))
                        {
                            uPrice = msg[msg.IndexOf("Price") + 1];
                        }
                        if (msg.Contains("Currency"))
                        {
                            uCurrency = CachedDataManager.GetInstance.GetAllCurrencies()[Convert.ToInt32(msg[msg.IndexOf("Currency") + 1])].ToString();
                        }
                    }
                    #endregion

                    #region Creating Datatable
                    DataTable dt = new DataTable();
                    if (!dt.Columns.Contains("Symbol"))
                    {
                        dt.Columns.Add("Symbol", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedSymbol"))
                    {
                        dt.Columns.Add("UploadedSymbol", typeof(string));
                    }
                    if (!dt.Columns.Contains("CUSIP"))
                    {
                        dt.Columns.Add("CUSIP", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedCUSIP"))
                    {
                        dt.Columns.Add("UploadedCUSIP", typeof(string));
                    }
                    if (!dt.Columns.Contains("ISIN"))
                    {
                        dt.Columns.Add("ISIN", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedISIN"))
                    {
                        dt.Columns.Add("UploadedISIN", typeof(string));
                    }
                    if (!dt.Columns.Contains("SEDOL"))
                    {
                        dt.Columns.Add("SEDOL", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedSEDOL"))
                    {
                        dt.Columns.Add("UploadedSEDOL", typeof(string));
                    }
                    if (!dt.Columns.Contains("BBCode"))
                    {
                        dt.Columns.Add("BBCode", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedBBCode"))
                    {
                        dt.Columns.Add("UploadedBBCode", typeof(string));
                    }
                    if (!dt.Columns.Contains("RIC"))
                    {
                        dt.Columns.Add("RIC", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedRIC"))
                    {
                        dt.Columns.Add("UploadedRIC", typeof(string));
                    }
                    if (!dt.Columns.Contains("OSIOptionSymbol"))
                    {
                        dt.Columns.Add("OSIOptionSymbol", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedOSIOptionSymbol"))
                    {
                        dt.Columns.Add("UploadedOSIOptionSymbol", typeof(string));
                    }
                    if (!dt.Columns.Contains("IDCOOptionSymbol"))
                    {
                        dt.Columns.Add("IDCOOptionSymbol", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedIDCOOptionSymbol"))
                    {
                        dt.Columns.Add("UploadedIDCOOptionSymbol", typeof(string));
                    }
                    if (!dt.Columns.Contains("OpraOptionSymbol"))
                    {
                        dt.Columns.Add("OpraOptionSymbol", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedOpraOptionSymbol"))
                    {
                        dt.Columns.Add("UploadedOpraOptionSymbol", typeof(string));
                    }
                    if (!dt.Columns.Contains("Multiplier"))
                    {
                        dt.Columns.Add("Multiplier", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedMultiplier"))
                    {
                        dt.Columns.Add("UploadedMultiplier", typeof(string));
                    }
                    if (!dt.Columns.Contains("Price"))
                    {
                        dt.Columns.Add("Price", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedPrice"))
                    {
                        dt.Columns.Add("UploadedPrice", typeof(string));
                    }
                    if (!dt.Columns.Contains("Currency"))
                    {
                        dt.Columns.Add("Currency", typeof(string));
                    }
                    if (!dt.Columns.Contains("UploadedCurrency"))
                    {
                        dt.Columns.Add("UploadedCurrency", typeof(string));
                    }
                    dt.Rows.Add(ticker, uticker, CUSIP, uCUSIP, ISIN, uISIN, SEDOL, uSEDOL, BBCode, uBBCode, RIC
                        , uRIC, OSIOptionSymbol, uOSIOptionSymbol, IDCOOptionSymbol, uIDCOOptionSymbol, OpraOptionSymbol,
                        uOpraOptionSymbol, Multiplier, uMultiplier, Price, uPrice, currency, uCurrency);

                    #endregion

                    //if form is not created
                    if (_frmSymbolMismatch == null || _frmSymbolMismatch.IsDisposed)
                    {  //set the form and grid properties
                        crlSymbolMismatch = new ctrlSymbolMismatch();
                        _frmSymbolMismatch = new Form();
                        _frmSymbolMismatch.Text = e.Cell.Text;
                        _frmSymbolMismatch.FormClosed += _frmSymbolMismatch_FormClosed;
                        SetThemeAtDynamicForm(_frmSymbolMismatch, crlSymbolMismatch);
                    }
                    else
                    {
                        //else previous form is bring to front
                        _frmSymbolMismatch.BringToFront();
                    }

                    //set the grid with data
                    crlSymbolMismatch.InitializeDataOnGrid(dt, true);

                    //crlSymbolMismatch.Dock = DockStyle.Fill;
                    _frmSymbolMismatch.Width = 730;
                    _frmSymbolMismatch.Height = 132;
                    _frmSymbolMismatch.Text = "Duplicate Symbol";
                    _frmSymbolMismatch.MaximumSize = new System.Drawing.Size(730, 132);
                    _frmSymbolMismatch.MinimumSize = new System.Drawing.Size(730, 132);
                    _frmSymbolMismatch.ShowIcon = false;
                    CustomThemeHelper.SetThemeProperties(_frmSymbolMismatch, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                    _frmSymbolMismatch.Show();
                }
                if (e.Cell.Column.Key == "CreateCounterParty" && e.Cell.Band.Columns.Exists("CounterPartyID"))
                {
                    int counterPartyID = int.Parse(e.Cell.Row.Cells["CounterPartyID"].Value.ToString());
                    if (counterPartyID <= 0)
                    {
                        launchForm = ImportManager.Instance.GetLaunchForm();
                        if (launchForm != null)
                        {
                            ListEventAargs args = new ListEventAargs();
                            args.listOfValues.Add(ApplicationConstants.CONST_DataMapping_UI.ToString());
                            args.listOfValues.Add("CounterPartyMapping.xml");
                            args.listOfValues.Add(txtThirdParty.Text);
                            launchForm(this, args);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ApplicationConstants.CONST_BROKER + " already exists.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// set the form to null if the form is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _frmSymbolMismatch_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_frmSymbolMismatch != null)
                {
                    _frmSymbolMismatch = null;
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
        /// sets theme at the form if the `
        /// </summary>
        /// <param name="dynamicForm"></param>
        /// <param name="control"></param>
        private void SetThemeAtDynamicForm(Form dynamicForm, Object control)
        {
            try
            {
                System.ComponentModel.IContainer dynamicComponents = new System.ComponentModel.Container();
                Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
                Infragistics.Win.Misc.UltraPanel dynamicForm_Fill_Panel;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Left;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Right;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Bottom;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Top;
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(dynamicComponents);
                dynamicForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
                dynamicForm_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).BeginInit();
                dynamicForm_Fill_Panel.SuspendLayout();
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1691
                //SuspendLayout();
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1.DesignerFlags = 1;
                ultraToolbarsManager1.DockWithinContainer = dynamicForm;
                ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
                ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
                ultraToolbarsManager1.IsGlassSupported = false;
                // 
                // frmReconCancelAmend_Fill_Panel
                // 
                dynamicForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
                dynamicForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
                dynamicForm_Fill_Panel.Location = new System.Drawing.Point(4, 52);
                dynamicForm_Fill_Panel.Name = "dynamicForm_Fill_Panel";
                dynamicForm_Fill_Panel.Size = new System.Drawing.Size(576, 261);
                dynamicForm_Fill_Panel.TabIndex = 0;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Left
                // 
                dynamicForm_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
                dynamicForm_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 52);
                dynamicForm_Toolbars_Dock_Area_Left.Name = "dynamicForm_Toolbars_Dock_Area_Left";
                dynamicForm_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 261);
                dynamicForm_Toolbars_Dock_Area_Left.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Right
                // 
                dynamicForm_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
                dynamicForm_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(580, 52);
                dynamicForm_Toolbars_Dock_Area_Right.Name = "dynamicForm_Toolbars_Dock_Area_Right";
                dynamicForm_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 261);
                dynamicForm_Toolbars_Dock_Area_Right.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Top
                // 
                dynamicForm_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
                dynamicForm_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
                dynamicForm_Toolbars_Dock_Area_Top.Name = "dynamicForm_Toolbars_Dock_Area_Top";
                dynamicForm_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(584, 52);
                dynamicForm_Toolbars_Dock_Area_Top.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Bottom
                // 
                dynamicForm_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
                dynamicForm_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 313);
                dynamicForm_Toolbars_Dock_Area_Bottom.Name = "dynamicForm_Toolbars_Dock_Area_Bottom";
                dynamicForm_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(584, 4);
                dynamicForm_Toolbars_Dock_Area_Bottom.ToolbarsManager = ultraToolbarsManager1;
                // 
                // frm
                //    
                if (control as UserControl == null)
                {
                    UltraGrid grid = control as UltraGrid;
                    grid.Dock = DockStyle.Fill;
                    dynamicForm.Controls.Add(grid);
                }
                else
                {
                    UserControl userControl = control as UserControl;
                    userControl.Dock = DockStyle.Fill;
                    dynamicForm.Controls.Add(userControl);
                }
                dynamicForm.Owner = this.FindForm();
                dynamicForm.ShowInTaskbar = false;
                dynamicForm.Size = new System.Drawing.Size(1107, 630);
                dynamicForm.Controls.Add(dynamicForm_Fill_Panel);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Left);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Right);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Bottom);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Top);
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).EndInit();
                dynamicForm_Fill_Panel.ResumeLayout(false);
                dynamicForm.ResumeLayout(false);
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
        /// Partial import the trades which are not imported
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtPartialImporttrades = new DataTable();
                dtPartialImporttrades = (grdReport.DataSource as DataTable).Copy();
                if (dtPartialImporttrades != null && _taskResult != null && _runUpload != null)
                {

                    if (grdReport.DisplayLayout.Bands[0].Columns.Exists("Select") && grdReport.DisplayLayout.Bands[0].Columns.Exists("ImportStatus") && grdReport.DisplayLayout.Bands[0].Columns.Exists("RowIndex"))
                    {
                        foreach (UltraGridRow row in grdReport.Rows)
                        {
                            if (row.Cells != null && (row.Cells["Select"].Value.ToString() == "False" || row.Cells["ImportStatus"].Value.ToString() == "Imported"))
                            {
                                int rowIndex = Convert.ToInt32(row.Cells["RowIndex"].Value.ToString());
                                DataRow[] result = dtPartialImporttrades.Select("RowIndex = '" + rowIndex.ToString() + "'");
                                if (result.Length >= 1)
                                {
                                    //remove the trades which are not checked or are already imported
                                    dtPartialImporttrades.Rows.Remove(result[0]);
                                }
                            }
                        }
                        DataSet dsPositionMaster = new DataSet();
                        dtPartialImporttrades.TableName = "ImportData";
                        dsPositionMaster.Tables.Add(dtPartialImporttrades.Copy());

                        if (dtPartialImporttrades.Rows.Count > 0)
                        {
                            ultraStatusBar1.Text = "Please see the import dashboard UI for current import status.";
                            _taskResult.TaskStatistics.Status = NirvanaTaskStatus.Importing;
                            // ImportManager importManager = new ImportManager();
                            //import the data in app                            

                            BackgroundWorker bgwWorkerImport = new BackgroundWorker();
                            bgwWorkerImport.DoWork += bgWorkerImport_DoWork;
                            object[] arguments = new object[2];
                            Tuple<TaskResult, RunUpload, DataSet> args = new Tuple<TaskResult, RunUpload, DataSet>(_taskResult, _runUpload, dsPositionMaster);
                            arguments[0] = args;
                            bgwWorkerImport.RunWorkerAsync(arguments);

                        }
                        else
                        {
                            ultraStatusBar1.Text = "No data to import.";
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

        private void bgWorkerImport_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];
                Tuple<TaskResult, RunUpload, DataSet> args = arguments[0] as Tuple<TaskResult, RunUpload, DataSet>;
                if (args != null)
                {
                    TaskResult taskResult = args.Item1;
                    RunUpload runUpload = args.Item2;
                    DataSet dsPositionMaster = args.Item3;
                    ImportManager.Instance.RunImportIntoApplication(runUpload, taskResult, true, dsPositionMaster);
                    ImportManager.Instance.SaveWorkflowResult();
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
        /// Load report layout xml if file exist
        /// </summary>
        private void LoadReportSaveLayoutXML()
        {
            try
            {
                _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _importReportLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID + @"\ImportReportLayout\";
                string LayoutXMLFileName = _importType + "ImportReportLayout.xml";
                _importReportFilePath = _importReportLayoutDirectoryPath + @"\" + LayoutXMLFileName;

                if (!Directory.Exists(_importReportLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_importReportLayoutDirectoryPath);
                }
                if (File.Exists(_importReportFilePath))
                {
                    grdReport.DisplayLayout.LoadFromXml(_importReportFilePath);
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

        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1989
        private void grdReport_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                if (grdReport.Rows.Count > 0 && grdReport.Rows.GetFilteredOutNonGroupByRows() != null)
                {
                    CheckState state = grdReport.DisplayLayout.Bands[0].Columns["Select"].GetHeaderCheckedState(grdReport.Rows);
                    UltraGridRow[] grdrows = grdReport.Rows.GetFilteredOutNonGroupByRows();
                    if (grdrows.Length > 0 && grdReport.Rows.Count > 0)
                    {
                        foreach (UltraGridRow row in grdrows)
                        {
                            if (state.Equals(CheckState.Checked))
                            {
                                row.Cells["Select"].Value = false;
                            }
                        }
                    }
                    foreach (UltraGridRow row in _selectedColumnList)
                    {
                        row.Cells["Select"].Value = true;
                    }
                    if (state == CheckState.Unchecked)
                    {
                        foreach (UltraGridRow row in grdReport.Rows)
                        {
                            if (row.Cells != null)
                            {
                                row.Cells["Select"].Value = false;
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

        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1989
        private void grdReport_BeforeHeaderCheckStateChanged(object sender, BeforeHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                if (grdReport.Rows.Count > 0)
                {
                    // Modified by Ankit Gupta on 16 Oct, 2014.
                    // To clear the list, before adding new selected items.
                    _selectedColumnList.Clear();
                    foreach (UltraGridRow row in grdReport.Rows)
                    {
                        if (row.Cells != null && Convert.ToBoolean(row.Cells["Select"].Value) == true)
                        {
                            _selectedColumnList.Add(row);
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

        internal void SetProperties(Dictionary<string, string> dictReportData, RunUpload runUpload, TaskResult result)
        {
            try
            {
                this._taskResult = result;
                this._runUpload = runUpload;
                this._dictReportData = dictReportData;
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
