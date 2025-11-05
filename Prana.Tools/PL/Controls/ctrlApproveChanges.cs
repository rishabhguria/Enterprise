using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.ReconciliationNew;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Prana.Tools.PL.Controls
{
    public partial class ctrlApproveChanges : UserControl, ILiveFeedCallback, IPublishing
    {

        bool _isBtnViewClicked = true;

        EventHandler DisableReconOutputUI;
        EventHandler DisableMarkPriceAppend;
        //private bool _isChangesApproved = false;
        //public bool IsChangesApproved
        //{
        //get { return _isChangesApproved; }
        //set { _isChangesApproved = value; }
        //}
        bool _isFetchingData = false;
        bool _isHeaderCheckBoxChecked = false;
        List<int> _accountIDUnlocked = new List<int>();
        StringBuilder _accountUnlocked = new StringBuilder();
        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        //public static event EventHandler launchForm;
        BackgroundWorker _bgApproveClickWorker;
        SerializableDictionary<string, int> _amendmends = new SerializableDictionary<string, int>();

        public ctrlApproveChanges()
        {
            try
            {
                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode())
                {
                    WireEvents();
                    BindTypeCombo();
                    CreatePricingServiceProxy();
                    SetUserPermissions();
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btnGetUnapprovedChanges.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetUnapprovedChanges.ForeColor = System.Drawing.Color.White;
                btnGetUnapprovedChanges.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetUnapprovedChanges.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetUnapprovedChanges.UseAppStyling = false;
                btnGetUnapprovedChanges.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnView.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnView.ForeColor = System.Drawing.Color.White;
                btnView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnView.UseAppStyling = false;
                btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnApprove.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnApprove.ForeColor = System.Drawing.Color.White;
                btnApprove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnApprove.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnApprove.UseAppStyling = false;
                btnApprove.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRescindChanges.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRescindChanges.ForeColor = System.Drawing.Color.White;
                btnRescindChanges.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRescindChanges.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRescindChanges.UseAppStyling = false;
                btnRescindChanges.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExportExcel.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExportExcel.ForeColor = System.Drawing.Color.White;
                btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExportExcel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExportExcel.UseAppStyling = false;
                btnExportExcel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// </summary>
        private void SetUserPermissions()
        {
            try
            {
                btnApprove.Enabled = false;
                btnRescindChanges.Enabled = false;
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
        /// Add try catch block in leftover methods in Project
        /// Added by: sachin mishra 
        /// </summary>
        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServicesProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
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

        private void BindTypeCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                listValues.Insert(1, new EnumerationValue("Recon", 0));
                listValues.Insert(2, new EnumerationValue("Mark Price", 1));

                cmbReconType.DataSource = null;
                cmbReconType.DataSource = listValues;
                cmbReconType.DisplayMember = "DisplayText";
                cmbReconType.ValueMember = "Value";
                cmbReconType.DataBind();
                cmbReconType.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbReconType.Value = -1;
                cmbReconType.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
        /// added by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project
        /// </summary>
        private void WireEvents()
        {
            try
            {
                _bgApproveClickWorker = new BackgroundWorker();
                _bgApproveClickWorker.DoWork += new DoWorkEventHandler(_bgApproveClickWorker_DoWork);
                _bgApproveClickWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgApproveClickWorker_RunWorkerCompleted);
                _bgApproveClickWorker.WorkerSupportsCancellation = true;
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

        private void _bgApproveMarkPriceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets cancelled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    MessageBox.Show("Operation has been cancelled!", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Selected mark prices approved", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Show updated xmls on the approval UI on basis of button click so that date range can be checked
                    // date range is not to be checked while refreshing in case if data in grid is set by btnGetUnapprovedChanges click
                    if (_isBtnViewClicked)
                    {
                        btnView_Click(this.btnView, null);
                    }
                    else
                    {
                        btnView_Click(this.btnGetUnapprovedChanges, null);
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

        private void _bgApproveMarkPriceWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ////TODO: Do following task in backgroundworker
                //if (!_bgApproveMarkPriceWorker.CancellationPending)//checks for cancel request
                //{
                //object[] arguments = e.Argument as object[];
                DataTable dtMarkPrice = e.Argument as DataTable;
                if (dtMarkPrice != null)
                {
                    dtMarkPrice.TableName = "dataMarkPrice";
                    _pricingServicesProxy.InnerChannel.ApproveMarkPrices(dtMarkPrice);
                    //AllocationManager.GetInstance().MakeNewCancelAmendChanges(dictApprovedChanges);
                }
                //}
                //else
                //    e.Cancel = true;
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

        private void _bgApproveClickWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //TODO: Do following task in backgroundworker
                if (!_bgApproveClickWorker.CancellationPending)//checks for cancel request
                {
                    object[] arguments = e.Argument as object[];
                    Dictionary<string, List<ApprovedChanges>> dictApprovedChanges = arguments[0] as Dictionary<string, List<ApprovedChanges>>;
                    if (dictApprovedChanges != null)
                    {
                        //As per discussion with Narender this is only required for CHMW so removing call from PRANA.
                        //AllocationManager.GetInstance().MakeNewCancelAmendChanges(dictApprovedChanges);

                        #region Save taxlot workflow status  - modified by omshiv
                        try
                        {
                            ConcurrentDictionary<string, NirvanaWorkFlowsStats> dictTaxlotStates = new ConcurrentDictionary<string, NirvanaWorkFlowsStats>();
                            foreach (String taxlot in dictApprovedChanges.Keys)
                            {
                                dictTaxlotStates.TryAdd(taxlot, NirvanaWorkFlowsStats.ReconApproved);
                            }
                            ReconUtilities.SaveTaxLotWorkflowStates(dictTaxlotStates);
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
                        #endregion
                    }
                }
                else
                    e.Cancel = true;
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

        private void _bgApproveClickWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets cancelled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    MessageBox.Show("Operation has been cancelled!", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(_noOfBreaksApproved + " change(s) approved", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Show updated xmls on the approval UI on basis of button click so that date range can be checked
                    // date range is not to be checked while refreshing in case if data in grid is set by btnGetUnapprovedChanges click
                    if (_isBtnViewClicked)
                    {

                        btnView_Click(this.btnView, null);
                    }
                    else
                    {
                        btnView_Click(this.btnGetUnapprovedChanges, null);
                    }
                    DisableReconOutputUI(this, null);
                    //_isChangesApproved = true;
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


        private DataTable getGridDataTableSchema()
        {
            DataTable table = new DataTable();
            try
            {
                //creates grid datatable and defines schema
                table.Columns.Add(ReconConstants.COLUMN_Checkbox, typeof(bool));
                table.Columns.Add(ReconConstants.COLUMN_TradeDate, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_TaxLotID, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_Client, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_Group, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_Account, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_ThirdParty, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_Symbol, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_ReconType, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_BreakType, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_ApplicationData, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_ThirdPartyData, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_AmendedData, typeof(string));
                table.Columns.Add(ReconConstants.COLUMN_ChangedBy, typeof(string));
                //adds column index which stores the value of row index in xml file
                table.Columns.Add(ReconConstants.COLUMN_ApproveChangesXmlRowIndex, typeof(string));
                //adds column path which stores the value of path of the xml file
                table.Columns.Add(ReconConstants.COLUMN_ApproveChangesXmlPath, typeof(string));
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
            return table;
        }
        int _noOfBreaksApproved = 0;
        private Dictionary<string, List<ApprovedChanges>> getApprovedChanges()
        {
            _noOfBreaksApproved = 0;
            Dictionary<string, List<ApprovedChanges>> dictApprovedChanges = new Dictionary<string, List<ApprovedChanges>>();
            //this xml will contain xml path, datatable to remove comlications of writing files multiple times
            Dictionary<string, DataTable> dictXMLs = new Dictionary<string, DataTable>();
            try
            {
                List<ApprovedChanges> lstApprovedChanges = null;
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3729
                if (!UltraWinGridUtils.IsGrouppingAppliedOnGrid(grdApproveChange))
                {
                    foreach (UltraGridRow row in grdApproveChange.Rows)
                    {
                        bool value = (bool)(row.Cells[ReconConstants.COLUMN_Checkbox].Value);
                        //checks if checkbox is checked or not
                        if (value == true)
                        {
                            _noOfBreaksApproved++;
                            ApprovedChanges approvedChanges = new ApprovedChanges();
                            //add value to the table which are checked
                            approvedChanges.TaxlotID = row.Cells[ReconConstants.COLUMN_TaxLotID].Text;
                            if (row.Cells[ReconConstants.COLUMN_BreakType].Text.Equals(AmendedTaxLotStatus.Deleted.ToString()))
                            {
                                approvedChanges.TaxlotStatus = AmendedTaxLotStatus.Deleted;
                            }
                            else
                            {
                                approvedChanges.TaxlotStatus = AmendedTaxLotStatus.ValueChanged;
                                approvedChanges.ColumnName = row.Cells[ReconConstants.COLUMN_BreakType].Text;
                                approvedChanges.OldValue = row.Cells[ReconConstants.COLUMN_ApplicationData].Text;
                                approvedChanges.NewValue = row.Cells[ReconConstants.COLUMN_AmendedData].Text;
                            }
                            if (!dictApprovedChanges.ContainsKey(approvedChanges.TaxlotID))
                            {
                                lstApprovedChanges = new List<ApprovedChanges>();
                                lstApprovedChanges.Add(approvedChanges);
                                dictApprovedChanges.Add(approvedChanges.TaxlotID, lstApprovedChanges);
                            }
                            else
                            {
                                dictApprovedChanges[approvedChanges.TaxlotID].Add(approvedChanges);
                            }

                            DataSet ds = new DataSet();
                            //get path from hidden row 
                            string path = row.Cells[ReconConstants.COLUMN_ApproveChangesXmlPath].Value.ToString();
                            DataTable dt = new DataTable();
                            //Read datatable and add to dictionary if not already read
                            if (!dictXMLs.ContainsKey(path))
                            {
                                //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                                ds = XMLUtilities.ReadXmlUsingBufferedStream(path);
                                //ds.ReadXml(path);

                                dt = ds.Tables[0];
                                dictXMLs.Add(path, dt);
                            }
                            else
                            {
                                dt = dictXMLs[path];
                            }

                            int rowIndex = Convert.ToInt32(row.Cells[ReconConstants.COLUMN_ApproveChangesXmlRowIndex].Text);
                            if (dt.Columns.Contains(ReconConstants.COLUMN_TaxLotStatus))
                            {
                                dt.Rows[rowIndex][ReconConstants.COLUMN_TaxLotStatus] = string.Empty;
                            }
                            else
                            {
                                //If changes are approved then amended data will be copied to application data
                                row.Cells[ReconConstants.COLUMN_ApplicationData].Value = row.Cells[ReconConstants.COLUMN_AmendedData].Text;
                                //index of the row in xml file                       
                                dt.Rows[rowIndex][ReconConstants.CONST_OriginalValue + row.Cells[ReconConstants.COLUMN_BreakType].Text] = dt.Rows[rowIndex][ReconConstants.CONST_Nirvana + row.Cells[ReconConstants.COLUMN_BreakType].Text];
                                List<string> changedColumns = new List<string>(dt.Rows[rowIndex][ReconConstants.COLUMN_ChangedColumns].ToString().Split(Char.Parse(Seperators.SEPERATOR_8)));
                                changedColumns.Remove(row.Cells[ReconConstants.COLUMN_BreakType].Text);
                                dt.Rows[rowIndex][ReconConstants.COLUMN_ChangedColumns] = string.Join(Seperators.SEPERATOR_8, changedColumns.ToArray());
                            }
                        }
                    }
                }

                //Write all xmls at once
                foreach (KeyValuePair<string, DataTable> kvp in dictXMLs)
                {
                    DataTable dt = kvp.Value;

                    int amendmentCount = 0;
                    //update amendments file with amendment count
                    if (dt.Columns.Contains(ReconConstants.COLUMN_ChangedColumns))
                    {
                        amendmentCount = (from DataRow row in dt.Rows where !(string.IsNullOrEmpty(row[ReconConstants.COLUMN_ChangedColumns].ToString())) select row).Count();
                    }
                    if (dt.Columns.Contains(ReconConstants.COLUMN_TaxLotStatus))
                    {
                        amendmentCount = (from DataRow row in dt.Rows where !(string.IsNullOrEmpty(row[ReconConstants.COLUMN_TaxLotStatus].ToString())) select row).Count();
                    }
                    string path = kvp.Key.Substring(kvp.Key.IndexOf(ApplicationConstants.RECON_DATA_DIRECTORY) - 1);
                    _amendmends[path] = amendmentCount;


                    //sort table as per modification for next approve changes in descnding order to get modified rows on top
                    string sortingColumnOrder = ReconConstants.COLUMN_ChangedColumns + " Desc," + ReconConstants.COLUMN_TaxLotStatus + " Desc";
                    //write sorted xml
                    DataTable dtSorted = ReconManager.sortDataTable(dt, sortingColumnOrder, null);
                    XMLUtilities.WriteXML(dtSorted, kvp.Key);
                    //dtSorted.WriteXml(kvp.Key);
                }
                //write the amendment File;
                ReconUtilities.WriteAmendmentDictionary(_amendmends);
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
            return dictApprovedChanges;
        }

        /// <summary>
        /// integrated with button view click and and button get Unapproved Changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                _isFetchingData = true;
                //Enable controls disabled due to save of amendments from reconciliation
                DisableControls(false);
                //check from which button the method is invoked
                Infragistics.Win.Misc.UltraButton btn = (sender as Infragistics.Win.Misc.UltraButton);
                if (btn == null)
                {
                    return;
                }
                // Date range is not to be checked
                else if (btn.Text == "Get UnApproved Changes")
                {
                    _isBtnViewClicked = false;
                }
                // Date range is to be checked
                else if (btn.Text == "View")
                {
                    _isBtnViewClicked = true;
                }
                DataTable table = getGridDataTableSchema();
                //"-Select- selected in Type Combo
                if (Convert.ToInt32(cmbReconType.Value) == -1)
                {
                    MessageBox.Show("Select approval type first", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (DateTime.Compare(((DateTime)dtEndDate.Value), ((DateTime)dtStartDate.Value)) < 0)
                {
                    MessageBox.Show("End Date cannot be less than start date", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Convert.ToInt32(cmbReconType.Value) == 1)
                {
                    //modified by omshiv, 14, Aug 2014, if "Get All UnApproved changes" click then we will not consider selected date range on UI  
                    DateTime startDate = DateTimeConstants.MinValue;
                    DateTime endDate = DateTime.MaxValue;

                    //if "View" button clicked then get date range from UI
                    if (_isBtnViewClicked)
                    {
                        startDate = Convert.ToDateTime(dtStartDate.Value);
                        endDate = Convert.ToDateTime(dtEndDate.Value);
                    }
                    BindGridWithUnapprovedMarkPrices(startDate, endDate);
                    _isFetchingData = false;
                    return;
                }


                DataTable temptable = getGridDataTableSchema();

                _amendmends = ReconUtilities.LoadAmendmentDictionary();
                if (_amendmends != null)
                {
                    //DataSet dsamentments = new DataSet();
                    //dsamentments.ReadXml(amendmentsFilePath);
                    //if (!dsamentments.Tables.Contains("ReconAmendments"))
                    //{
                    //    return;
                    //}
                    //DataTable amentments = dsamentments.Tables["ReconAmendments"];
                    ////if column does not exist or file is empty then return
                    //if (amentments.Rows.Count == 0 || !amentments.Columns.Contains("AmendedRecords") || !amentments.Columns.Contains("FilePath"))
                    //{
                    //    return;
                    //}
                    //List<DataRow> lstrowamentments = (from DataRow row in amentments.Rows where Convert.ToInt32(row["AmendedRecords"].ToString()) > 0 select row).ToList();

                    foreach (string path in _amendmends.Keys)
                    {
                        string reconFilePath = Application.StartupPath + path;
                        if (_amendmends[path] == 0 || !File.Exists(reconFilePath))
                        {
                            continue;
                        }

                        List<string> temp = new List<string>(Path.GetFileNameWithoutExtension(reconFilePath).Split(Seperators.SEPERATOR_6));
                        DateTime fileFromDate = DateTime.MinValue;
                        DateTime fileToDate = DateTime.MinValue;
                        //skip if dateTime from file cannot be parsed
                        if (temp.Count != 6)//Fields added in File name
                        {
                            continue;
                        }
                        //if "Get UnApproved Changes" button is click then date is not needed to be checked
                        if (_isBtnViewClicked)
                        {
                            if (!(DateTime.TryParseExact(temp[1], ApplicationConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out fileFromDate)
                                && DateTime.TryParseExact(temp[2], ApplicationConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out fileToDate)))
                            {
                                return;
                            }
                            //skip if the file is out of range

                            if (fileToDate < ((DateTime)dtStartDate.Value) || fileFromDate > ((DateTime)dtEndDate.Value))
                            {
                                continue;
                            }
                        }
                        DataSet ds = new DataSet();
                        //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                        ds = XMLUtilities.ReadXmlUsingBufferedStream(reconFilePath);
                        //ds.ReadXml(reconFilePath);
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-426
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];
                            int rowIndex = -1;
                            if (dt.Columns.Contains(ReconConstants.CONST_Nirvana + "TradeDate"))
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    rowIndex++;
                                    DateTime rowDate = DateTime.MinValue;
                                    //if "Get UnApproved Changes" button is click then date is not needed to be checked 
                                    if (dt.Columns.Contains(ReconConstants.CONST_Nirvana + "TradeDate") && DateTime.TryParse(row[ReconConstants.CONST_Nirvana + "TradeDate"].ToString(), out rowDate) && (rowDate.Date > ((DateTime)dtEndDate.Value).Date || rowDate.Date < ((DateTime)dtStartDate.Value).Date) && _isBtnViewClicked)
                                    {
                                        continue;
                                    }
                                    string date = rowDate.ToString();
                                    if (dt.Columns.Contains(ReconConstants.COLUMN_ChangedColumns) || dt.Columns.Contains(ReconConstants.COLUMN_TaxLotStatus))
                                    {
                                        //date extraction from file name                                       
                                        string taxLotId = string.Empty;
                                        string accountName = string.Empty;
                                        string symbol = string.Empty;
                                        string changedBy = string.Empty;
                                        string PrimeBroker = string.Empty;
                                        string templateDirectory = Directory.GetParent(reconFilePath).ToString();
                                        string reconDirectory = Directory.GetParent(templateDirectory).ToString();
                                        string reconType = Path.GetFileName(Path.GetDirectoryName(templateDirectory));
                                        //get client name from id using dictionary
                                        string clientName = CachedDataManager.GetCompanyText(Convert.ToInt32(Path.GetFileName(Path.GetDirectoryName(reconDirectory))));

                                        if (dt.Columns.Contains(ReconConstants.COLUMN_TaxLotID))
                                        {
                                            taxLotId = row[ReconConstants.COLUMN_TaxLotID].ToString();
                                        }
                                        if (dt.Columns.Contains(ReconConstants.COLUMN_UserLoggedIN))
                                        {
                                            changedBy = row[ReconConstants.COLUMN_UserLoggedIN].ToString();
                                        }
                                        else
                                        {
                                            changedBy = CachedDataManager.GetInstance.LoggedInUser.FirstName + " " + CachedDataManager.GetInstance.LoggedInUser.LastName;
                                        }
                                        if (dt.Columns.Contains(ReconConstants.COLUMN_AccountName))
                                        {
                                            accountName = row[ReconConstants.COLUMN_AccountName].ToString();
                                        }
                                        int accountID = CachedDataManager.GetInstance.GetAccountID(accountName);
                                        if (!CachedDataManager.GetInstance.GetUserAccounts().Contains(accountID))
                                        {
                                            continue;
                                        }
                                        if (dt.Columns.Contains(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_PrimeBroker))
                                        {
                                            PrimeBroker = row[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_PrimeBroker].ToString();
                                        }
                                        //pick prime broker name from cache if prime broker column does not exist in XML files
                                        if (string.IsNullOrEmpty(PrimeBroker.Trim()))
                                        {

                                            int pbID = int.MinValue;
                                            foreach (KeyValuePair<int, List<int>> item in CachedDataManagerRecon.GetInstance.GetAllCompanyThirdPartyAccounts())
                                            {
                                                if (item.Value.Contains(accountID))
                                                {
                                                    pbID = item.Key;
                                                    break;
                                                }
                                            }
                                            if (pbID != int.MinValue && CachedDataManager.GetInstance.GetAllThirdParties().ContainsKey(pbID))
                                            {
                                                PrimeBroker = CachedDataManager.GetInstance.GetAllThirdParties()[pbID];
                                            }
                                        }

                                        if (dt.Columns.Contains(ReconConstants.COLUMN_Symbol))
                                        {
                                            symbol = row[ReconConstants.COLUMN_Symbol].ToString();
                                        }

                                        if (dt.Columns.Contains(ReconConstants.COLUMN_TaxLotStatus) && row[ReconConstants.COLUMN_TaxLotStatus].ToString().Equals(AmendedTaxLotStatus.Deleted.ToString()))
                                        {
                                            temptable.Rows.Add(false, date, taxLotId, clientName, string.Empty, accountName, PrimeBroker, symbol, reconType, row[ReconConstants.COLUMN_TaxLotStatus].ToString(), string.Empty, string.Empty, string.Empty, string.Empty, rowIndex, reconFilePath);
                                        }
                                        else if (dt.Columns.Contains(ReconConstants.COLUMN_ChangedColumns))
                                        {
                                            //array of string which contain the column name which are changed
                                            string[] colChanged = row[ReconConstants.COLUMN_ChangedColumns].ToString().Split(',');
                                            foreach (string column in colChanged)
                                            {
                                                if (!string.IsNullOrEmpty(column))
                                                {
                                                    //gets column name to fetch value from data table of xml file
                                                    string originalValueColName = ReconConstants.CONST_OriginalValue + column;
                                                    string nirColName = ReconConstants.CONST_Nirvana + column;
                                                    string brokerColName = ReconConstants.CONST_Broker + column;
                                                    string nirValue = string.Empty;
                                                    string brokervalue = string.Empty;
                                                    string originalValue = string.Empty;
                                                    if (dt.Columns.Contains(nirColName))
                                                    {
                                                        nirValue = row[nirColName].ToString();
                                                    }
                                                    if (dt.Columns.Contains(brokerColName))
                                                    {
                                                        brokervalue = row[brokerColName].ToString();
                                                    }
                                                    if (dt.Columns.Contains(originalValueColName))
                                                    {
                                                        originalValue = row[originalValueColName].ToString();
                                                    }
                                                    ////modified by amit. changes done for http://jira.nirvanasolutions.com:8080/browse/CHMW-3576
                                                    //if (column.Equals(OrderFields.PROPERTY_SETTLEMENTCURRENCY))
                                                    //{
                                                    //    Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                                                    //    if (dt.Columns.Contains(nirColName))
                                                    //    {
                                                    //        nirValue = dictCurrencies[Int32.Parse(nirValue)];
                                                    //    }
                                                    //    if (dt.Columns.Contains(originalValueColName))
                                                    //    {
                                                    //        if (Int32.Parse(originalValue) == 0)
                                                    //            originalValue = ApplicationConstants.C_COMBO_NONE;
                                                    //        else
                                                    //            originalValue = dictCurrencies[Int32.Parse(originalValue)];
                                                    //    }
                                                    //}

                                                    //data row format as per description
                                                    temptable.Rows.Add(false, date, taxLotId, clientName, string.Empty, accountName, PrimeBroker, symbol, reconType, column, originalValue, brokervalue, nirValue, changedBy, rowIndex, reconFilePath);
                                                }
                                            }
                                        }
                                    }
                                }//end of row in xml file
                            }
                        }
                    }//end of row in amendments files

                    //end of xml file
                    //DateTime.ParseExact(dateText, dateformat, CultureInfo.InvariantCulture);
                    // path = @"C:\Users\aman.seth\Desktop\Transaction_03-24-2014.xml";
                    //set grid data table
                    table.Merge(temptable);
                }
                grdApproveChange.DataSource = null;
                grdApproveChange.DataSource = table;
                //set the header checkBox o unchecked state initially
                if (grdApproveChange.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.COLUMN_Checkbox))
                {
                    grdApproveChange.DisplayLayout.Bands[0].Columns[ReconConstants.COLUMN_Checkbox].SetHeaderCheckedState(grdApproveChange.Rows, CheckState.Unchecked);
                }
                _isFetchingData = false;
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

        private void BindGridWithUnapprovedMarkPrices(DateTime startDate, DateTime endDate)
        {
            try
            {
                DataTable dtMarkPriceSource = new DataTable();
                dtMarkPriceSource = _pricingServicesProxy.InnerChannel.GetUnapprovedMarkPrices(startDate, endDate);
                dtMarkPriceSource.TableName = "UnapprovedMarkPrices";
                if (dtMarkPriceSource != null)
                {
                    grdApproveChange.DataSource = null;
                    grdApproveChange.DataSource = dtMarkPriceSource;
                    //set the header checkBox o unchecked state initially
                    if (grdApproveChange.DisplayLayout.Bands[0].Columns.Exists("chkApprove"))
                    {
                        grdApproveChange.DisplayLayout.Bands[0].Columns["chkApprove"].SetHeaderCheckedState(grdApproveChange.Rows, CheckState.Unchecked);
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

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = grdApproveChange.DataSource as DataTable;
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return;
                }

                if (dt.TableName.Equals("UnapprovedMarkPrices"))
                {
                    bool countApprove = false;
                    foreach (UltraGridRow row in grdApproveChange.Rows)
                    {
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3715
                        if (grdApproveChange.DisplayLayout.Bands[0].Columns.Exists("chkApprove") && row.Cells != null && Convert.ToBoolean(row.Cells["chkApprove"].Value))
                        {
                            DisableMarkPriceAppend(this, null);
                            //_isChangesApproved = true;
                            countApprove = true;
                            break;
                        }
                    }
                    if (!countApprove)
                    {
                        MessageBox.Show("Select at least one mark price for approval", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    DataTable dtMprice = CreateMarkPriceIDTable();
                    //ApproveMarkPrices(dtMprice);
                    //added by: Bharat Raturi, 20 may 2014
                    //purpose: 
                    BackgroundWorker _bgApproveMarkPriceWorker = new BackgroundWorker();
                    _bgApproveMarkPriceWorker.DoWork += new DoWorkEventHandler(_bgApproveMarkPriceWorker_DoWork);
                    _bgApproveMarkPriceWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgApproveMarkPriceWorker_RunWorkerCompleted);
                    _bgApproveMarkPriceWorker.WorkerSupportsCancellation = true;
                    if (_bgApproveMarkPriceWorker.IsBusy)
                    {
                        MessageBox.Show("Please wait while previous changes are approved", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _bgApproveMarkPriceWorker.RunWorkerAsync(dtMprice);
                    }
                    return;
                }
                //TODO: Do following task in backgroundworker

                Dictionary<string, List<ApprovedChanges>> dictApprovedChanges = getApprovedChanges();
                if (dictApprovedChanges != null && dictApprovedChanges.Count > 0)
                {
                    DisableReconOutputUI(this, null);
                    //_isChangesApproved = true;
                    object[] arguments = new object[1];
                    arguments[0] = dictApprovedChanges;
                    if (_bgApproveClickWorker.IsBusy)
                    {
                        MessageBox.Show("Please wait while previous changes are approved", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _bgApproveClickWorker.RunWorkerAsync(arguments);
                    }
                }
                else
                    MessageBox.Show("No changes to approve", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                #region commented
                //try
                //{
                //    //TODO: Do following task in backgroundworker
                //    Dictionary<string, List<ApprovedChanges>> dictApprovedChanges = getApprovedChanges();
                //    if (dictApprovedChanges.Count > 0)
                //    {
                //        AllocationManager.GetInstance().MakeNewCancelAmendChanges(dictApprovedChanges);
                //        MessageBox.Show(_noOfBreaksApproved + " change(s) approved", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }

                //    //Show updated xmls on the approval UI                
                //    btView_Click(null,null);
                //}
                //catch (Exception ex)
                //{
                //    // Invoke our policy that is responsible for making sure no secure information
                //    // gets out of our layer.
                //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                //    if (rethrow)
                //    {
                //        throw;
                //    }
                //}
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

        /// <summary>
        /// Create the datatable of IDs of the mark price
        /// </summary>
        /// <param name="dt">Dt holding the mark price details</param>
        private DataTable CreateMarkPriceIDTable()
        {
            DataTable dtMarkPriceID = new DataTable();
            dtMarkPriceID.Columns.Add("MarkPriceID", typeof(int));
            try
            {
                if (grdApproveChange.DataSource != null && grdApproveChange.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in grdApproveChange.Rows)
                    {
                        if (!string.IsNullOrWhiteSpace(row.Cells["DayMarkPriceID"].Value.ToString()) && Convert.ToBoolean(row.Cells["chkApprove"].Value))
                        {
                            int markPriceID = Convert.ToInt32(row.Cells["DayMarkPriceID"].Value);
                            dtMarkPriceID.Rows.Add(markPriceID);
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
            return dtMarkPriceID;
        }

        /// <summary>
        /// Approve mark prices and save them in db
        /// </summary>
        //private void ApproveMarkPrices()
        //{

        //}

        //revert the changes made in xml file
        private void btnRescindChanges_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt1 = grdApproveChange.DataSource as DataTable;
                if (dt1 == null || dt1.Rows.Count <= 0)
                {
                    return;
                }

                if (dt1.TableName.Equals("UnapprovedMarkPrices"))
                {
                    bool countRescind = false;
                    foreach (UltraGridRow row in grdApproveChange.Rows)
                    {
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3715
                        if (grdApproveChange.DisplayLayout.Bands[0].Columns.Exists("chkApprove") && row.Cells != null && Convert.ToBoolean(row.Cells["chkApprove"].Value))
                        {
                            DisableMarkPriceAppend(this, null);
                            //_isChangesApproved = true;
                            countRescind = true;
                            break;
                        }
                    }
                    if (!countRescind)
                    {
                        MessageBox.Show("Select at least one mark price to rescind", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    DataTable dtMprice = CreateMarkPriceIDTable();
                    //ApproveMarkPrices(dtMprice);
                    //added by: Bharat Raturi, 20 may 2014
                    //purpose: 
                    BackgroundWorker _bgRescindMarkPriceWorker = new BackgroundWorker();
                    _bgRescindMarkPriceWorker.DoWork += new DoWorkEventHandler(_bgRescindMarkPriceWorker_DoWork);
                    _bgRescindMarkPriceWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgRescindMarkPriceWorker_RunWorkerCompleted);
                    _bgRescindMarkPriceWorker.WorkerSupportsCancellation = true;
                    if (_bgRescindMarkPriceWorker.IsBusy)
                    {
                        MessageBox.Show("Please wait while previous changes are rescinded", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _bgRescindMarkPriceWorker.RunWorkerAsync(dtMprice);
                    }
                    return;
                }
                int noOfChangesRescinded = 0;
                Dictionary<string, DataTable> dictXMLs = new Dictionary<string, DataTable>();
                ConcurrentDictionary<string, NirvanaWorkFlowsStats> dictRescindedTaxlots = new ConcurrentDictionary<string, NirvanaWorkFlowsStats>();
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3729
                if (!UltraWinGridUtils.IsGrouppingAppliedOnGrid(grdApproveChange))
                {
                    foreach (UltraGridRow row in grdApproveChange.Rows)
                    {
                        bool value = (bool)(row.Cells[ReconConstants.COLUMN_Checkbox].Value);
                        if (value == true)
                        {
                            DisableReconOutputUI(this, null);
                            //_isChangesApproved = true;
                            noOfChangesRescinded++;
                            row.Cells[ReconConstants.COLUMN_ApplicationData].Value = row.Cells[ReconConstants.COLUMN_AmendedData].Text;
                            DataSet ds = new DataSet();
                            //get path from hidden row 
                            string path = row.Cells[ReconConstants.COLUMN_ApproveChangesXmlPath].Value.ToString();
                            DataTable dt = new DataTable();

                            if (!dictXMLs.ContainsKey(path))
                            {
                                //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                                //ds.ReadXml(path);
                                ds = XMLUtilities.ReadXmlUsingBufferedStream(path);
                                dt = ds.Tables[0];
                                dictXMLs.Add(path, dt);
                            }
                            else
                            {
                                dt = dictXMLs[path];
                            }

                            //index of the row in xml file
                            //if the row is deleted
                            int rowIndex = Convert.ToInt32(row.Cells[ReconConstants.COLUMN_ApproveChangesXmlRowIndex].Text);

                            if (row.Cells[ReconConstants.COLUMN_BreakType].Text == AmendedTaxLotStatus.Deleted.ToString() && dt.Columns.Contains(ReconConstants.COLUMN_TaxLotStatus))
                            {
                                dt.Rows[rowIndex][ReconConstants.COLUMN_TaxLotStatus] = string.Empty;
                            }
                            else
                            {
                                // There are modifications in the row
                                dt.Rows[rowIndex][ReconConstants.CONST_Nirvana + row.Cells[ReconConstants.COLUMN_BreakType].Text] = dt.Rows[rowIndex][ReconConstants.CONST_OriginalValue + row.Cells[ReconConstants.COLUMN_BreakType].Text];
                                List<string> changedColumns = new List<string>(dt.Rows[rowIndex][ReconConstants.COLUMN_ChangedColumns].ToString().Split(Char.Parse(Seperators.SEPERATOR_8)));
                                changedColumns.Remove(row.Cells[ReconConstants.COLUMN_BreakType].Text);
                                dt.Rows[rowIndex][ReconConstants.COLUMN_ChangedColumns] = string.Join(Seperators.SEPERATOR_8, changedColumns.ToArray());
                            }

                            #region getting data to Taxlot State
                            String taxlotID = row.Cells[ReconConstants.COLUMN_Checkbox].Text;
                            if (!string.IsNullOrWhiteSpace(taxlotID) && !dictRescindedTaxlots.ContainsKey(taxlotID))
                                dictRescindedTaxlots.TryAdd(taxlotID, NirvanaWorkFlowsStats.FailedReconciliation);
                            #endregion
                        }
                    }
                }
                //Write all xmls at once

                foreach (KeyValuePair<string, DataTable> kvp in dictXMLs)
                {
                    DataTable dt = kvp.Value;
                    int amendmentCount = 0;
                    //update amendments file with amendment count
                    if (dt.Columns.Contains(ReconConstants.COLUMN_ChangedColumns))
                    {
                        amendmentCount = (from DataRow row in dt.Rows where !(string.IsNullOrEmpty(row[ReconConstants.COLUMN_ChangedColumns].ToString())) select row).Count();
                    }
                    if (dt.Columns.Contains(ReconConstants.COLUMN_TaxLotStatus))
                    {
                        amendmentCount += (from DataRow row in dt.Rows where !(string.IsNullOrEmpty(row[ReconConstants.COLUMN_TaxLotStatus].ToString())) select row).Count();
                    }
                    string path = kvp.Key.Substring(kvp.Key.IndexOf(ApplicationConstants.RECON_DATA_DIRECTORY) - 1);
                    _amendmends[path] = amendmentCount;

                    //sort table as per modification for next approve changes in descnding order to get modified rows on top
                    string sortingColumnOrder = ReconConstants.COLUMN_ChangedColumns + " Desc," + ReconConstants.COLUMN_TaxLotStatus + " Desc";
                    //write sorted xml
                    ReconManager.sortDataTable(dt, sortingColumnOrder, null).WriteXml(kvp.Key);

                }
                //write the amendment File
                ReconUtilities.WriteAmendmentDictionary(_amendmends);

                if (dictXMLs.Count > 0)
                {
                    MessageBox.Show(noOfChangesRescinded + " change(s) rescinded", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //Show updated xmls on the approval UI on basis of button click so that date range can be checked
                // date range is not to be checked while refreshing in case if data in grid is set by btnGetUnapprovedChanges click
                if (_isBtnViewClicked)
                {
                    btnView_Click(this.btnView, null);
                }
                else
                {
                    btnView_Click(this.btnGetUnapprovedChanges, null);
                }

                if (dictRescindedTaxlots.Count > 0)
                {
                    ReconUtilities.SaveTaxLotWorkflowStates(dictRescindedTaxlots);
                }

                //_isChangesApproved = true;
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



        private void _bgRescindMarkPriceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets cancelled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    MessageBox.Show("Operation has been cancelled!", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Selected mark prices rescinded", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Show updated xmls on the approval UI on basis of button click so that date range can be checked
                    // date range is not to be checked while refreshing in case if data in grid is set by btnGetUnapprovedChanges click
                    if (_isBtnViewClicked)
                    {
                        btnView_Click(this.btnView, null);
                        //_isChangesApproved = true;
                    }
                    else
                    {
                        btnView_Click(this.btnGetUnapprovedChanges, null);
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

        private void _bgRescindMarkPriceWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ////TODO: Do following task in backgroundworker
                //if (!_bgApproveMarkPriceWorker.CancellationPending)//checks for cancel request
                //{
                //object[] arguments = e.Argument as object[];
                DataTable dtMarkPrice = e.Argument as DataTable;
                if (dtMarkPrice != null)
                {
                    dtMarkPrice.TableName = "dataMarkPrice";
                    _pricingServicesProxy.InnerChannel.RescindMarkPrices(dtMarkPrice);
                    //AllocationManager.GetInstance().MakeNewCancelAmendChanges(dictApprovedChanges);
                }
                //}
                //else
                //    e.Cancel = true;
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
        //saves file in excel
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridFileExporter.LoadFilePathAndExport(grdApproveChange, this);
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

        private void grdApproveChange_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //add filters on grid if there are rows in grid
                if (grdApproveChange.Rows.Count > 0)
                {
                    UltraWinGridUtils.EnableFixedFilterRow(e);
                }
                //Set the HeaderCheckBoxVisibility so it will display the CheckBox whenever a CheckEditor is used within the UltraGridColumn 
                grdApproveChange.DisplayLayout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                //Set the HeaderCheckBoxSynchronization so all rows within the GridBand will be synchronized with the CheckBox 
                grdApproveChange.DisplayLayout.Override.HeaderCheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                if (grdApproveChange.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.COLUMN_Checkbox))
                {
                    grdApproveChange.DisplayLayout.Bands[0].Columns[ReconConstants.COLUMN_Checkbox].Width = 40;
                }


                // Set the RowSelectorHeaderStyle to ColumnChooserButton.
                grdApproveChange.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                // Enable the RowSelectors. This is necessary because the column chooser
                // button is displayed over the row selectors in the column headers area.
                grdApproveChange.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
                UltraGridBand band = e.Layout.Bands[0];

                DataTable dt = grdApproveChange.DataSource as DataTable;
                if (!dt.TableName.Equals("UnapprovedMarkPrices"))
                {
                    //altering header caption of all columns
                    band.Columns[ReconConstants.COLUMN_Checkbox].Header.Caption = string.Empty;
                    band.Columns[ReconConstants.COLUMN_TradeDate].Header.Caption = ReconConstants.CAPTION_TradeDate;
                    band.Columns[ReconConstants.COLUMN_TaxLotID].Header.Caption = ReconConstants.CAPTION_TaxLotID;
                    band.Columns[ReconConstants.COLUMN_Client].Header.Caption = ReconConstants.CAPTION_Client;
                    band.Columns[ReconConstants.COLUMN_Group].Header.Caption = ReconConstants.CAPTION_Group;
                    band.Columns[ReconConstants.COLUMN_Account].Header.Caption = ReconConstants.CAPTION_Account;
                    band.Columns[ReconConstants.COLUMN_ThirdParty].Header.Caption = ReconConstants.CAPTION_ThirdParty;
                    band.Columns[ReconConstants.COLUMN_Symbol].Header.Caption = ReconConstants.CAPTION_Symbol;
                    band.Columns[ReconConstants.COLUMN_ReconType].Header.Caption = ReconConstants.CAPTION_ReconType;
                    band.Columns[ReconConstants.COLUMN_BreakType].Header.Caption = ReconConstants.CAPTION_BreakType;
                    band.Columns[ReconConstants.COLUMN_ApplicationData].Header.Caption = ReconConstants.CAPTION_ApplicationData;
                    band.Columns[ReconConstants.COLUMN_ThirdPartyData].Header.Caption = ReconConstants.CAPTION_ThirdPartyData;
                    band.Columns[ReconConstants.COLUMN_AmendedData].Header.Caption = ReconConstants.CAPTION_AmendedData;
                    band.Columns[ReconConstants.COLUMN_ChangedBy].Header.Caption = ReconConstants.CAPTION_ChangedBy;

                    //setting row visible index
                    band.Columns[ReconConstants.COLUMN_Checkbox].Header.VisiblePosition = 0;
                    band.Columns[ReconConstants.COLUMN_TradeDate].Header.VisiblePosition = 1;
                    band.Columns[ReconConstants.COLUMN_TaxLotID].Header.VisiblePosition = 2;
                    band.Columns[ReconConstants.COLUMN_Client].Header.VisiblePosition = 3;
                    band.Columns[ReconConstants.COLUMN_Group].Header.VisiblePosition = 4;
                    band.Columns[ReconConstants.COLUMN_Account].Header.VisiblePosition = 5;
                    band.Columns[ReconConstants.COLUMN_ThirdParty].Header.VisiblePosition = 6;
                    band.Columns[ReconConstants.COLUMN_Symbol].Header.VisiblePosition = 7;
                    band.Columns[ReconConstants.COLUMN_ReconType].Header.VisiblePosition = 8;
                    band.Columns[ReconConstants.COLUMN_BreakType].Header.VisiblePosition = 9;
                    band.Columns[ReconConstants.COLUMN_ApplicationData].Header.VisiblePosition = 10;
                    band.Columns[ReconConstants.COLUMN_ThirdPartyData].Header.VisiblePosition = 11;
                    band.Columns[ReconConstants.COLUMN_AmendedData].Header.VisiblePosition = 12;
                    band.Columns[ReconConstants.COLUMN_ChangedBy].Header.VisiblePosition = 13;

                    band.Columns[ReconConstants.COLUMN_ApproveChangesXmlRowIndex].Header.VisiblePosition = 14;
                    band.Columns[ReconConstants.COLUMN_ApproveChangesXmlPath].Header.VisiblePosition = 15;
                    band.Columns[ReconConstants.COLUMN_ApproveChangesXmlRowIndex].Hidden = true;
                    band.Columns[ReconConstants.COLUMN_ApproveChangesXmlPath].Hidden = true;
                    //band.Columns[ReconConstants.COLUMN_ApproveChangesXmlRowIndex].Hidden = true;
                    //band.Columns[ReconConstants.COLUMN_ApproveChangesXmlPath].Hidden = true;

                    //Disable row editing
                    foreach (UltraGridColumn column in band.Columns)
                    {
                        column.CellActivation = Activation.NoEdit;
                    }
                    //allow edit in checkbox column
                    band.Columns[ReconConstants.COLUMN_Checkbox].CellActivation = Activation.AllowEdit;
                }
                else
                {
                    if (!band.Columns.Exists("chkApprove"))
                    {
                        band.Columns.Add("chkApprove");
                    }
                    UltraGridColumn colApprove = band.Columns["chkApprove"];
                    colApprove.DataType = typeof(bool);
                    colApprove.Header.Caption = "Select";
                    colApprove.Header.VisiblePosition = 1;

                    if (band.Columns.Exists("FundName"))
                    {
                        UltraGridColumn colAccountName = band.Columns["FundName"];
                        colAccountName.Header.Caption = "Account";
                        colAccountName.Header.VisiblePosition = 2;
                    }
                    if (band.Columns.Exists("DayMarkPriceID"))
                    {
                        band.Columns["DayMarkPriceID"].Hidden = true;
                    }
                    if (band.Columns.Exists("Date"))
                    {
                        UltraGridColumn coldate = band.Columns["Date"];
                        coldate.Header.Caption = "Date";
                        coldate.Header.VisiblePosition = 3;
                    }
                    if (band.Columns.Exists("Symbol"))
                    {
                        UltraGridColumn colSymbol = band.Columns["Symbol"];
                        colSymbol.Header.Caption = "Symbol";
                        colSymbol.Header.VisiblePosition = 4;
                    }

                    if (band.Columns.Exists("FundID"))
                    {
                        band.Columns["FundID"].Hidden = true;
                    }
                    if (band.Columns.Exists("FinalMarkPrice"))
                    {
                        UltraGridColumn colChangedPrice = band.Columns["FinalMarkPrice"];
                        colChangedPrice.Header.Caption = "Actual Mark Price";
                        colChangedPrice.Header.VisiblePosition = 5;
                    }
                    if (band.Columns.Exists("AmendedMarkPrice"))
                    {
                        UltraGridColumn colActualPrice = band.Columns["AmendedMarkPrice"];
                        colActualPrice.Header.Caption = "Amended Mark Price";
                        colActualPrice.Header.VisiblePosition = 6;
                    }
                    foreach (UltraGridColumn col in grdApproveChange.DisplayLayout.Bands[0].Columns)
                    {
                        if (!col.Key.Equals("chkApprove"))
                        {
                            col.CellActivation = Activation.NoEdit;
                        }
                        else
                        {
                            col.CellActivation = Activation.AllowEdit;
                        }
                    }
                }

                //colour alternate rows
                grdApproveChange.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Gray;
                grdApproveChange.DisplayLayout.Override.RowAlternateAppearance.BackColor2 = Color.DarkGray;
                grdApproveChange.DisplayLayout.Override.RowAlternateAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
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
        /// Unique message method implemntation
        ///
        /// added by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project
        /// </summary>
        /// <returns>The message (name of the control here)</returns>
        public string getReceiverUniqueName()
        {
            try
            {
                return "ctrlApproveChanges";
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
            return null;
        }

        public void Publish(MessageData e, string topicName)
        {
            try
            {

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

        public void LiveFeedConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void LiveFeedDisConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            //Implementation not required
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }
        /// <summary>
        /// Check if the trade being edited is having its accounts locked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdApproveChange_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                if (!e.Cell.Row.Cells.Exists(ReconConstants.COLUMN_Account))
                {
                    return;
                }
                if (e.Cell.GetType() == typeof(UltraGridFilterCell))
                {
                    return;
                }
                string rowAccountname = e.Cell.Row.Cells[ReconConstants.COLUMN_Account].Text;
                int accountID = CachedDataManager.GetInstance.GetAccountID(rowAccountname);
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1132
                if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue)
                {
                    if (_isHeaderCheckBoxChecked)
                    {
                        if (!_accountUnlocked.ToString().Contains(rowAccountname))
                        {
                            _accountUnlocked.Append(rowAccountname).Append(',');
                            _accountIDUnlocked.Add(accountID);
                        }
                        e.Cancel = true;
                        return;
                    }
                    if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking " + rowAccountname + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        List<int> newAccountsToBelocked = new List<int>();
                        newAccountsToBelocked.Add(accountID);
                        newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                        if (ReconUtilities.SetAccountsLockStatus(newAccountsToBelocked))
                        {
                            MessageBox.Show("The lock for " + rowAccountname + " has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(rowAccountname + " is currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //cancel the update
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        // user clicked no
                        //cancel the update
                        e.Cancel = true;
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
        /// Account lock implementation on header checkbox  click of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdApproveChange_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                _isHeaderCheckBoxChecked = false;
                //if (_accountUnlocked.Length > 0)
                //{
                //    if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking following accounts " + _accountUnlocked.ToString().Substring(0, _accountUnlocked.Length - 1) + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {
                //        List<int> newAccountsToBelocked = new List<int>();
                //        newAccountsToBelocked.AddRange(_accountIDUnlocked);
                //        newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                //        if (ReconUtilities.SetAccountsLockStatus(newAccountsToBelocked))
                //        {

                //            MessageBox.Show("The lock for accounts has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            //update Locks in cache
                //            CachedDataManager.GetInstance.SetLockedAccounts(newAccountsToBelocked);
                //        }
                //        else
                //        {
                //            MessageBox.Show("CashAccounts are currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        }
                //    }
                //}

                //_accountUnlocked = new StringBuilder();
                //_accountIDUnlocked = new List<int>();
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
        /// Account lock implementation on header checkbox  click of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdApproveChange_BeforeHeaderCheckStateChanged(object sender, BeforeHeaderCheckStateChangedEventArgs e)
        {

            try
            {
                _isHeaderCheckBoxChecked = true;
                if (!_isFetchingData && e.NewCheckState != CheckState.Indeterminate)
                {
                    if (grdApproveChange.DataSource != null)
                    {
                        DataTable dt = (DataTable)grdApproveChange.DataSource;
                        if (dt != null)
                        {
                            List<string> lstAccountName = (from DataRow dr in dt.Rows select (string)dr[ReconConstants.COLUMN_Account]).Distinct().ToList();
                            List<string> accountUnlocked = new List<string>();
                            List<int> accountIDUnlocked = new List<int>();
                            lstAccountName.ForEach(x =>
                            {
                                int accountID = CachedDataManager.GetInstance.GetAccountID(x);
                                if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue)
                                {
                                    accountIDUnlocked.Add(accountID);
                                    accountUnlocked.Add(x);
                                }
                            });

                            if (accountIDUnlocked.Count > 0)
                            {
                                if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking following accounts " + string.Join(Seperators.SEPERATOR_8, accountUnlocked.ToArray()) + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    List<int> newAccountsToBelocked = new List<int>();
                                    newAccountsToBelocked.AddRange(accountIDUnlocked);
                                    newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                                    if (ReconUtilities.SetAccountsLockStatus(newAccountsToBelocked))
                                    {
                                        MessageBox.Show("The lock for accounts has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        //update Locks in cache
                                        CachedDataManager.GetInstance.SetLockedAccounts(newAccountsToBelocked);
                                    }
                                    else
                                    {
                                        e.Cancel = true;
                                        _isHeaderCheckBoxChecked = false;
                                        MessageBox.Show("Accounts are currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    e.Cancel = true;
                                    _isHeaderCheckBoxChecked = false;
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
        /// Enable contols on Grid click
        /// </summary>
        /// <param name="isDisableControls"></param>
        internal void DisableControls(bool isDisableControls)
        {
            try
            {
                if (isDisableControls)
                {
                    btnApprove.Enabled = false;
                    btnRescindChanges.Enabled = false;
                    ultraStatusBar1.Text = @"Data modified, please click on ""View""/""Get UnApproved Changes/"" button to get the updated data.";
                }
                else
                {
                    //_isChangesApproved = false;
                    btnApprove.Enabled = true;
                    btnRescindChanges.Enabled = true;
                    ultraStatusBar1.Text = string.Empty;
                    SetUserPermissions();
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
        /// Filter cell value changed for grdApproveChange.
        /// Added by Ankit Gupta on 30 Oct, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1328
        /// Filtering grouped data should be smooth.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdApproveChange_FilterCellValueChanged(object sender, FilterCellValueChangedEventArgs e)
        {
            try
            {
                String filterCondition = e.FilterCell.Text;
                grdApproveChange.Rows.ColumnFilters[e.FilterCell.Column.Key].FilterConditions.Clear();
                grdApproveChange.Rows.ColumnFilters[e.FilterCell.Column.Key].FilterConditions.Add(new FilterCondition(FilterComparisionOperator.StartsWith, filterCondition));
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
        /// Initialize Row event of grid    
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdApproveChange_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //[Recon] User is able to see unapproved changes of mark price of accounts for which user do not have permission
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1962
                if (e.Row.Cells.Exists(ReconConstants.COLUMN_Account) && e.Row.Cells[ReconConstants.COLUMN_Account].Value != null)
                {
                    string accountName = e.Row.Cells[ReconConstants.COLUMN_Account].Value.ToString();
                    int accountID = CachedDataManager.GetInstance.GetAccountID(accountName);
                    if (!CachedDataManager.GetInstance.GetUserAccounts().Contains(accountID))
                    {
                        e.Row.Hidden = true;
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

        // <summary>
        /// Update event of disabling other controls if amendments are made
        /// </summary>
        /// <param name="DisableApproveChanges"></param>
        internal void UpdateEvent(EventHandler evntDisableReconOutputUI, EventHandler eventDisableMarkPriceAppend)
        {
            try
            {
                DisableReconOutputUI = evntDisableReconOutputUI;
                DisableMarkPriceAppend = eventDisableMarkPriceAppend;
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

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}