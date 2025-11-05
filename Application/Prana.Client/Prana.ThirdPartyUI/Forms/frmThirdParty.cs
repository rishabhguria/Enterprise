using ExportGridsData;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.ThirdPartyUI.Forms;
using Prana.TradeManager.Extension;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using static Prana.Global.ApplicationConstants;

namespace Prana.ThirdPartyUI
{
    public partial class frmThirdParty : Form, IThirdPartyReport, IPublishing, IDisposable, IExportGridData
    {
        private static bool DisablePrompts = false;
        int LastRowIndex = -1;
        // To keep the state whether the caption has been once updated or not
        private static bool _isCaptionUpdated = false;
        private string _outputFilePath = string.Empty;
        private SynchronizationContext _uiSynchronizationContext;
        ICommunicationManager _tradeCommunicationManager;
        private CompanyUser _loginUser;
        private DuplexProxyBase<ISubscription> _subscriptionProxy;

        // Main Class for Batch Process       
        ThirdPartyBatches Jobs;

        private const string HEADCOL_CHECKBOX = "HEADERCHECK";
        private const string HEADCOL_DESCRIPTION = "DESCRIPTION";
        private const string HEADCOL_THIRDPARTYNAME = "THIRDPARTYNAME";
        private const string HEADCOL_FILEFORMATNAME = "FILEFORMATNAME";
        private const string HEADCOL_THIRDPARTYTYPENAME = "THIRDPARTYTYPENAME";
        private const string HEADCOL_TRANSMISSIONTYPE = "TRANSMISSIONTYPE";
        private const string HEADCOL_FIXCONNECTIONSTATUS = "FIXCONNECTIONSTATUS";
        private const string HEADCOL_BROKERCONNECTIONTYPE = "BROKERCONNECTIONTYPE";
        private const string HEADCOL_ALLOCATIONMATCHSTATUS = "ALLOCATIONMATCHSTATUS";
        private const string HEADCOL_AUTOMATEDBATCHSTATUS = "AutomatedBatchStatus";
        private const string HEADCOL_VIEW = "VIEW";
        private const string HEADCOL_EXPORT = "EXPORT";
        private const string HEADCOL_SEND = "SEND";
        private const string HEADCOL_ACTIVE = "Active";

        private readonly object _locker = new object();

        public ICommunicationManager TradeCommunicationManager
        {
            set { _tradeCommunicationManager = value; }
        }

        /// <summary>
        /// Gets or sets the login user.
        /// </summary>
        /// <value>The login user.</value>
        /// <remarks></remarks>
        public CompanyUser LoginUser
        {
            get
            {
                return _loginUser;
            }
            set
            {
                _loginUser = value;
            }
        }

        /// <summary>
        /// Occurs when [third party flat file closed].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler ThirdPartyFlatFileClosed;
        public virtual event EventHandler<EventArgs<string, DateTime, DateTime>> GoToAllocationClicked = null;

        public frmThirdParty()
        {
            try
            {
                InitializeComponent();
                menuStrip1.RenderMode = ToolStripRenderMode.Professional;
                menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MenuStripRenderer());
                TradeManagerExtension.GetInstance().CounterPartyStatusUpdate += ThirdParty_CounterPartyStatusUpdate;
                _uiSynchronizationContext = SynchronizationContext.Current;
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

        private void ThirdParty_CounterPartyStatusUpdate(object sender, EventArgs<CounterPartyDetails> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            ThirdParty_CounterPartyStatusUpdate(sender, e);
                        }));
                    }
                    else
                    {
                        if (e.Value != null && e.Value.OriginatorType == PranaServerConstants.OriginatorType.Allocation)
                        {
                            foreach (UltraGridRow row in GrdJob.Rows)
                            {
                                ThirdPartyBatch order = row.ListObject as ThirdPartyBatch;
                                if (order.TransmissionType.Equals(((int)TransmissionType.FIX).ToString()) && e.Value.CounterPartyID == order.CounterPartyID)
                                {
                                    if (e.Value.ConnStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                                    {
                                        row.Cells[HEADCOL_FIXCONNECTIONSTATUS].Value = ThirdPartyConstants.FIX_CONNECTIONSTATUS_CONNECTED;
                                        row.Cells[HEADCOL_FIXCONNECTIONSTATUS].Appearance.ForeColor = Color.Green;
                                    }
                                    else
                                    {
                                        row.Cells[HEADCOL_FIXCONNECTIONSTATUS].Value = ThirdPartyConstants.FIX_CONNECTIONSTATUS_DISCONNECTED;
                                        row.Cells[HEADCOL_FIXCONNECTIONSTATUS].Appearance.ForeColor = Color.Red;
                                    }

                                    row.Cells[HEADCOL_BROKERCONNECTIONTYPE].Value = EnumHelper.GetDescriptionWithDescriptionAttribute(e.Value.BrokerConnectionType);
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

        private static void ClearPreviousLayouts()
        {
            string[] files = Directory.GetFiles(".\\Prana Preferences\\EOD\\Layouts\\", "*.layout");
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        private void SetCheckState()
        {
            try
            {
                CurrencyManager cm = (CurrencyManager)BindingContext[GrdJob.DataSource];
                cm.SuspendBinding();
                foreach (UltraGridRow row in GrdJob.Rows)
                {
                    if ((bool)row.Cells[HEADCOL_ACTIVE].Value == false)
                        row.Hidden = true;
                }
                cm.ResumeBinding();
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

        private void frmThirdParty_Load(object sender, EventArgs e)
        {
            try
            {
                CreateFolders(".\\EOD");
                CreateFolders(".\\Prana Preferences\\EOD\\Layouts");
                CreateFolders(".\\EOD\\Archives");
                MakeProxy();

                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Remove(tabPage3);

                //Text = string.Format("Third Party Manager [{0}]", ThirdPartyManagerEx.GetCompanyName());
                Text = "Third Party Manager";
                tsLoad_Click(sender, e);
                if (!CustomThemeHelper.ApplyTheme)
                {
                    SetAppearancesWithoutTheme();
                }
                else
                {
                    SetAppearanceWithTheme();
                    CustomThemeHelper.SetThemeProperties(GrdJob as UltraGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY_CUSTOM);
                    CustomThemeHelper.SetThemeProperties(dataView, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY_CUSTOM);
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                InstanceManager.RegisterInstance(this);
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

        private void tsLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPreviousLayouts();

                ThirdPartyClientManager.ServiceInnerChannel.SyncThirdPartyFileWithDataBase();

                List<ThirdPartyBatch> lst = ThirdPartyClientManager.ServiceInnerChannel.LoadThirdPartyBacthes(RunDate.Value);

                ThirdPartyBatches thirdPartyBatches = new ThirdPartyBatches();
                foreach (ThirdPartyBatch batch in lst)
                {
                    thirdPartyBatches.Add(batch);
                    batch.CompanyId = CachedDataManager.GetInstance.LoggedInUser.CompanyID;
                    batch.UserId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                    if (batch.AllowedFixTransmission != null && batch.AllowedFixTransmission.Value)
                    {
                        batch.FIXConnectionStatus = TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(batch.CounterPartyID, PranaServerConstants.OriginatorTypeCategory.EOD) == PranaInternalConstants.ConnectionStatus.CONNECTED ?
                                ThirdPartyConstants.FIX_CONNECTIONSTATUS_CONNECTED : ThirdPartyConstants.FIX_CONNECTIONSTATUS_DISCONNECTED;
                        batch.BrokerConnectionType = EnumHelper.GetDescriptionWithDescriptionAttribute(TradeManagerExtension.GetInstance().GetBrokerConnectionType(batch.CounterPartyID));
                    }
                }

                Jobs = thirdPartyBatches;
                GrdJob.DataSource = null;
                GrdJob.DataSource = Jobs;

                UpdateColumnsUI(GrdJob);
                SetCheckState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GrdJob_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                if (GrdJob.Rows.Count > 0)
                {
                    UltraGridBand band = GrdJob.DisplayLayout.Bands[0];

                    band.Override.AllowColMoving = AllowColMoving.NotAllowed;
                    GrdJob.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
                    GrdJob.DisplayLayout.GroupByBox.Hidden = true;
                    GrdJob.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
                    GrdJob.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;

                    band.Columns.Add(HEADCOL_CHECKBOX, "");
                    band.Columns[HEADCOL_CHECKBOX].DataType = typeof(bool);
                    band.Columns[HEADCOL_CHECKBOX].CellClickAction = CellClickAction.EditAndSelectText;
                    band.Columns[HEADCOL_CHECKBOX].Hidden = false;
                    band.Columns[HEADCOL_CHECKBOX].Header.VisiblePosition = 0;
                    band.Columns[HEADCOL_CHECKBOX].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                    band.Columns[HEADCOL_CHECKBOX].SortIndicator = SortIndicator.Disabled;
                    band.Columns[HEADCOL_CHECKBOX].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                    band.Columns[HEADCOL_CHECKBOX].Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                    band.Columns[HEADCOL_CHECKBOX].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.RowsCollection;
                    band.Columns[HEADCOL_CHECKBOX].SetHeaderCheckedState(GrdJob.Rows, true);

                    band.Columns[HEADCOL_DESCRIPTION].Header.Caption = "Job Name";
                    band.Columns[HEADCOL_DESCRIPTION].Header.VisiblePosition = 1;
                    //band.Columns[HEADCOL_DESCRIPTION].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                    band.Columns[HEADCOL_DESCRIPTION].CellActivation = Activation.NoEdit;

                    band.Columns["IsLevel2Data"].Hidden = true;
                    band.Columns["Active"].Hidden = true;
                    band.Columns["IncludeSent"].Hidden = true;
                    band.Columns["AllowedFixTransmission"].Hidden = true;
                    band.Columns["FtpName"].Hidden = true;
                    band.Columns["GnuPGName"].Hidden = true;
                    band.Columns["EmailDataName"].Hidden = true;
                    band.Columns["EmailLogName"].Hidden = true;

                    band.Columns[HEADCOL_THIRDPARTYNAME].Header.Caption = "Party Name";
                    band.Columns[HEADCOL_THIRDPARTYNAME].Header.VisiblePosition = 2;
                    band.Columns[HEADCOL_THIRDPARTYNAME].CellActivation = Activation.NoEdit;

                    band.Columns[HEADCOL_THIRDPARTYTYPENAME].Header.Caption = "Third Party Type";
                    band.Columns[HEADCOL_THIRDPARTYTYPENAME].Header.VisiblePosition = 3;
                    band.Columns[HEADCOL_THIRDPARTYTYPENAME].CellActivation = Activation.NoEdit;

                    band.Columns[HEADCOL_TRANSMISSIONTYPE].Header.Caption = "Transmission Type";
                    band.Columns[HEADCOL_TRANSMISSIONTYPE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    band.Columns[HEADCOL_TRANSMISSIONTYPE].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    band.Columns[HEADCOL_TRANSMISSIONTYPE].ValueList = GetTransmissionTypeValues();
                    band.Columns[HEADCOL_TRANSMISSIONTYPE].Header.VisiblePosition = 4;

                    band.Columns[HEADCOL_FILEFORMATNAME].Header.Caption = "File Format";
                    band.Columns[HEADCOL_FILEFORMATNAME].Header.VisiblePosition = 5;
                    band.Columns[HEADCOL_FILEFORMATNAME].CellActivation = Activation.NoEdit;

                    band.Columns[HEADCOL_FIXCONNECTIONSTATUS].Header.Caption = "FIX Connection Status";
                    band.Columns[HEADCOL_FIXCONNECTIONSTATUS].Header.VisiblePosition = 6;
                    band.Columns[HEADCOL_FIXCONNECTIONSTATUS].CellActivation = Activation.NoEdit;

                    band.Columns[HEADCOL_BROKERCONNECTIONTYPE].Header.Caption = "Broker Connection Type";
                    band.Columns[HEADCOL_BROKERCONNECTIONTYPE].Header.VisiblePosition = 7;
                    band.Columns[HEADCOL_BROKERCONNECTIONTYPE].CellActivation = Activation.NoEdit;

                    band.Columns[HEADCOL_AUTOMATEDBATCHSTATUS].Header.Caption = "Automated Batch Status";
                    band.Columns[HEADCOL_AUTOMATEDBATCHSTATUS].Header.VisiblePosition = 8;
                    band.Columns[HEADCOL_AUTOMATEDBATCHSTATUS].CellActivation = Activation.NoEdit;

                    band.Columns[HEADCOL_ALLOCATIONMATCHSTATUS].Header.Caption = "Allocation Match Status";
                    band.Columns[HEADCOL_ALLOCATIONMATCHSTATUS].Header.VisiblePosition = 9;
                    band.Columns[HEADCOL_ALLOCATIONMATCHSTATUS].Width = 100;
                    band.Columns[HEADCOL_ALLOCATIONMATCHSTATUS].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    band.Columns[HEADCOL_ALLOCATIONMATCHSTATUS].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                    band.Columns[HEADCOL_ALLOCATIONMATCHSTATUS].SortIndicator = SortIndicator.Disabled;
                    band.Columns[HEADCOL_ALLOCATIONMATCHSTATUS].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;                  

                    band.Columns.Add(HEADCOL_VIEW, "View");
                    band.Columns[HEADCOL_VIEW].Header.VisiblePosition = 10;
                    band.Columns[HEADCOL_VIEW].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    band.Columns[HEADCOL_VIEW].Width = 70;
                    band.Columns[HEADCOL_VIEW].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    band.Columns[HEADCOL_VIEW].Header.Caption = "View";
                    band.Columns[HEADCOL_VIEW].NullText = "View";
                    band.Columns[HEADCOL_VIEW].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                    band.Columns[HEADCOL_VIEW].SortIndicator = SortIndicator.Disabled;

                    band.Columns.Add(HEADCOL_EXPORT, "Export");
                    band.Columns[HEADCOL_EXPORT].Header.VisiblePosition = 11;
                    band.Columns[HEADCOL_EXPORT].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    band.Columns[HEADCOL_EXPORT].Width = 70;
                    band.Columns[HEADCOL_EXPORT].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    band.Columns[HEADCOL_EXPORT].Header.Caption = "Export";
                    band.Columns[HEADCOL_EXPORT].NullText = "Export";
                    band.Columns[HEADCOL_EXPORT].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                    band.Columns[HEADCOL_EXPORT].SortIndicator = SortIndicator.Disabled;

                    band.Columns.Add(HEADCOL_SEND, "Send");
                    band.Columns[HEADCOL_SEND].Header.VisiblePosition = 12;
                    band.Columns[HEADCOL_SEND].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    band.Columns[HEADCOL_SEND].Width = 70;
                    band.Columns[HEADCOL_SEND].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    band.Columns[HEADCOL_SEND].Header.Caption = "Send";
                    band.Columns[HEADCOL_SEND].NullText = "Send";
                    band.Columns[HEADCOL_SEND].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                    band.Columns[HEADCOL_SEND].SortIndicator = SortIndicator.Disabled;

                    UltraGridLayout layout = e.Layout;
                    layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                    foreach (UltraGridColumn column in band.Columns)
                    {
                        if (column.Key == HEADCOL_SEND || column.Key == HEADCOL_EXPORT || column.Key == HEADCOL_VIEW)
                            column.MinWidth = column.MaxWidth = 70;
                        if (column.Key == HEADCOL_CHECKBOX)
                            column.MinWidth = column.MaxWidth = 24;
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
        /// Handles the AfterCellUpdate event of the GrdJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void GrdJob_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                ThirdPartyBatch thirdPartyBatch = e.Cell.Row.ListObject as ThirdPartyBatch;
                if (e.Cell.Column.Key == "TransmissionType")
                {
                    thirdPartyBatch.SerializedDataSet = null;
                    thirdPartyBatch.SerializedDataSource = null;
                    if (int.Parse(thirdPartyBatch.TransmissionType).Equals((int)TransmissionType.File))
                    {
                        GrdJob.Rows[e.Cell.Row.Index].Cells[HEADCOL_FIXCONNECTIONSTATUS].Value = string.Empty;
                        GrdJob.Rows[e.Cell.Row.Index].Cells[HEADCOL_BROKERCONNECTIONTYPE].Value = string.Empty;
                    }
                    else
                    {
                        if (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(thirdPartyBatch.CounterPartyID, PranaServerConstants.OriginatorTypeCategory.EOD) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            GrdJob.Rows[e.Cell.Row.Index].Cells[HEADCOL_FIXCONNECTIONSTATUS].Value = ThirdPartyConstants.FIX_CONNECTIONSTATUS_CONNECTED;
                            GrdJob.Rows[e.Cell.Row.Index].Cells[HEADCOL_FIXCONNECTIONSTATUS].Appearance.ForeColor = Color.Green;
                        }
                        else
                        {
                            GrdJob.Rows[e.Cell.Row.Index].Cells[HEADCOL_FIXCONNECTIONSTATUS].Value = ThirdPartyConstants.FIX_CONNECTIONSTATUS_DISCONNECTED;
                            GrdJob.Rows[e.Cell.Row.Index].Cells[HEADCOL_FIXCONNECTIONSTATUS].Appearance.ForeColor = Color.Red;
                        }
                        
                        GrdJob.Rows[e.Cell.Row.Index].Cells[HEADCOL_BROKERCONNECTIONTYPE].Value = EnumHelper.GetDescriptionWithDescriptionAttribute(TradeManagerExtension.GetInstance().GetBrokerConnectionType(thirdPartyBatch.CounterPartyID));
                    }

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        SetAutomatedBatchStatus(thirdPartyBatch, e.Cell.Row.Index);
                    });

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        var allocationMatchStatus = ThirdPartyClientManager.ServiceInnerChannel.GetAllocationMatchStatusForBatch(thirdPartyBatch.ThirdPartyBatchId, RunDate.Value.ToShortDateString(), int.Parse(thirdPartyBatch.TransmissionType).Equals((int)TransmissionType.File));
                        UpdateAllocationMatchStatusButton(e.Cell.Row, allocationMatchStatus);
                    });
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
        /// Getting Transmission Types
        /// </summary>
        /// <returns></returns>
        private ValueList GetTransmissionTypeValues()
        {
            ValueList valueList = null;
            try
            {
                valueList = new ValueList();
                string[] transmissionTypes = Enum.GetNames(typeof(TransmissionType));
                int i = 0;
                foreach (var transmissionType in transmissionTypes)
                {
                    valueList.ValueListItems.Add(i, transmissionType);
                    i++;
                }
                return valueList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return valueList;
        }

        private void GrdJob_ClickCellButton(object sender, CellEventArgs e)
        {
            var isAnotherTaskStarted = false;
            try
            {
                btnView.Enabled = false;
                btnExport.Enabled = false;
                btnSend.Enabled = false;
                GrdJob.Enabled = false;

                ChangeCellAppearance();

                ThirdPartyBatch batch = (ThirdPartyBatch)e.Cell.Row.ListObject;
                if (ThirdPartyClientManager.ServiceInnerChannel.GetFormat(batch) == null)
                {
                    MessageBox.Show("This batch doesn't exist", "Batch Doesn't Exist", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                tabPage1.Text = "Active Tax Lots - " + batch.Description;
                try
                {
                    if (e.Cell.Row.Index == -1 || e.Cell.Row.Index == -1) return;
                    string HeaderText = e.Cell.Column.Header.Caption;

                    if (dataView.DataSource == null)
                        txtOutput.Text = "";
                    else if (batch.Format != null && File.Exists(batch.Format.FilePath))
                    {
                        if (!batch.Format.FileName.ToLower().Contains(".xls"))
                            txtOutput.Text = File.ReadAllText(batch.Format.FilePath);
                    }
                    dataView.SetColumnHidden();
                    dataView.Refresh();
                    if (HeaderText == "View")
                    {
                        System.Threading.Tasks.Task.Run(() => View(batch, e.Cell));
                        isAnotherTaskStarted = true;
                    }
                    else if (HeaderText == "Export")
                    {
                        System.Threading.Tasks.Task.Run(() => Export(batch, e.Cell));
                        isAnotherTaskStarted = true;
                    }
                    else if (HeaderText == "Send")
                    {
                        if (CheckFixConnectionAndDisplayMessage(batch))
                        {
                            System.Threading.Tasks.Task.Run(() => Send(batch, e.Cell));
                            isAnotherTaskStarted = true;
                        }
                    }
                    else if (HeaderText == "Allocation Match Status" && batch.TransmissionType.Equals(((int)TransmissionType.FIX).ToString())
                        && batch.BrokerConnectionType == EnumHelper.GetDescriptionWithDescriptionAttribute(PranaServerConstants.BrokerConnectionType.SendAndConfirmBack)
                        && batch.AllocationMatchStatus != EnumHelper.GetDescriptionWithDescriptionAttribute(AllocationMatchStatus.NotSent))
                    {
                        BatchDetails batchDetails = new BatchDetails();
                        batchDetails.GoToAllocationClicked += GoToAllocationClicked;
                        batchDetails.SetUp(RunDate.Value, batch);
                        batchDetails.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    ThirdPartyClientManager.ServiceInnerChannel.LogMessage(batch.LogFile, ex.Message);
                    OnMessage(this, new MessageEventArgs("Message sending failed for " + batch.Description, "", batch.Description, batch.LogFile));
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
            finally
            {
                if (!isAnotherTaskStarted)
                {
                    EnableGridAndButtons(e.Cell);
                }
            }
        }

        private void Send(ThirdPartyBatch batch, UltraGridCell cell)
        {
            try
            {
                if (batch.FtpId != 0 || batch.EmailDataId != 0 || batch.TransmissionType.Equals(((int)TransmissionType.FIX).ToString()))
                {
                    SelectTabPage5();

                    if (!string.IsNullOrEmpty(batch.SerializedDataSource))
                    {
                        var selectedDataXml = string.Empty;
                        if (batch.TransmissionType.Equals(((int)TransmissionType.FIX).ToString()))
                        {
                            selectedDataXml = dataView.GetSelectedDataXml(batch.SerializedDataSource);
                        }
                        else
                        {
                            var selectedDataSet = dataView.GetFilteredRowDataSet();
                            if (selectedDataSet != null)
                                selectedDataXml = selectedDataSet.GetXml();
                        }

                        if (!string.IsNullOrEmpty(selectedDataXml))
                            batch.SerializedDataSet = selectedDataXml;
                        else
                            batch.SerializedDataSet = batch.SerializedDataSource;
                    }

                    ThirdPartyClientManager.ServiceInnerChannel.SendFile(batch, RunDate.Value, CachedDataManager.GetInstance.LoggedInUser.FirstName);

                }
                _outputFilePath = string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                EnableGridAndButtons(cell, true);
            }
        }

        private void SelectTabPage5()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker mi = delegate { SelectTabPage5(); };
                    this.BeginInvoke(mi);
                }
                else
                {
                    tabControl1.SelectedTab = tabPage5;
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

        private void Export(ThirdPartyBatch batch, UltraGridCell cell)
        {
            try
            {
                var dataSource = new DataSet();
                if (!string.IsNullOrEmpty(batch.SerializedDataSource))
                {
                    dataSource = dataView.GetFilteredRowDataSet();

                    if (dataSource != null)
                        batch.SerializedDataSet = dataSource.GetXml();
                    else
                        batch.SerializedDataSet = null;
                }
                ThirdPartyClientManager.ServiceInnerChannel.ExportFile(batch, RunDate.Value);
                //if (updatedBatch == null)
                //{
                //    txtOutput.Text = "";
                //    if (!string.IsNullOrEmpty(batch.SerializedDataSource))
                //    {
                //        dataSource.ReadXml(new StringReader(batch.SerializedDataSource));
                //    }
                //    if (batch.Format != null && File.Exists(batch.Format.FilePath) && dataSource != null && dataSource.Tables[0].Rows.Count > 0)
                //    {
                //        if (!batch.Format.FileName.ToLower().Contains(".xls"))
                //        {
                //            txtOutput.Text = File.ReadAllText(batch.Format.FilePath);
                //        }
                //        tabControl1.SelectedTab = tabPage4;
                //        _outputFilePath = batch.Format.FilePath;
                //    }
                //    return;
                //}
                //if (batch.Format != null && batch.Format.FileName.ToLower().Contains(".xls"))
                //{
                //    txtOutput.Text = "";
                //}
                ////if sent already, and user cancels, don't continue
                //if (batch.Format != null)
                //    LoadOutput(batch.Format.FileName, batch.Description, batch.LogFile);
                //if (batch.Format != null)
                //    _outputFilePath = batch.Format.FilePath;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                EnableGridAndButtons(cell);
            }
        }

        private void EnableGridAndButtons(UltraGridCell cell, bool isDataClear = false)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker mi = delegate { EnableGridAndButtons(cell, isDataClear); };
                    this.BeginInvoke(mi);
                }
                else
                {
                    if (isDataClear)
                    {
                        ThirdPartyBatch tpbatch = (ThirdPartyBatch)cell.Row.ListObject;
                        tpbatch.SerializedDataSet = null;
                        tpbatch.SerializedDataSource = null;
                        dataView.DataSource = null;
                    }

                    if (DateTime.Compare(RunDate.Value, DateTime.Today.AddDays(-5)) >= 1)
                    {
                        btnView.Enabled = true;
                        btnExport.Enabled = true;
                        btnSend.Enabled = true;
                    }
                    
                    GrdJob.Enabled = true;

                    if (!cell.Column.Key.ToUpper().Equals(HEADCOL_ALLOCATIONMATCHSTATUS))
                    {
                        cell.ButtonAppearance.BackColor = Color.FromArgb(178, 178, 178);
                        cell.ButtonAppearance.BackColor2 = Color.FromArgb(178, 178, 178);
                        cell.ButtonAppearance.ForeColor = Color.Black;
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

        private void View(ThirdPartyBatch batch, UltraGridCell cell)
        {
            try
            {
                bool isAllocated = !ThirdPartyClientManager.ServiceInnerChannel.CheckForUnallocatedTrades(RunDate.Value);

                if (isAllocated || MessageBox.Show(this, "There are some trades which are still unallocated.Do you still want to continue with Flat File generation?", "Flat File Generation Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    batch.SerializedDataSet = null;
                    batch.SerializedDataSource = null;
                    var updatedBatch = ThirdPartyClientManager.ServiceInnerChannel.View(batch, RunDate.Value, true);
                    Jobs[cell.Row.Index] = updatedBatch;
                    batch = Jobs[cell.Row.Index] as ThirdPartyBatch;
                    var dataSource = new DataSet();
                    if (!string.IsNullOrEmpty(batch.SerializedDataSource))
                        dataSource.ReadXml(new StringReader(batch.SerializedDataSource));
                    UpdateUIElements(dataSource, batch);
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
            finally
            {
                EnableGridAndButtons(cell);
            }
        }

        private void UpdateUIElements(DataSet dataSource, ThirdPartyBatch batch)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker mi = delegate { UpdateUIElements(dataSource, batch); };
                    this.BeginInvoke(mi);
                }
                else
                {
                    if (!_isCaptionUpdated)
                    {
                        dataView.UpdateCaptions(dataSource);
                    }
                    _isCaptionUpdated = true;
                    dataView.DataSource = dataSource;
                    dataView.OrderDetails = batch.OrderDetail;
                    dataView.AddCheckBoxinGrid();
                    dataView.SetColumnHidden();
                    dataView.SetHeaderChecked();
                    dataView.Refresh();
                    dataView.SaveState(batch.LayoutFile);
                    txtOutput.Text = "";
                    _outputFilePath = string.Empty;
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

        private void ChangeCellAppearance()
        {
            try
            {
                foreach (UltraGridRow row in this.GrdJob.Rows)
                {
                    UltraGridCell cellView = row.Cells[HEADCOL_VIEW];
                    UltraGridCell cellExport = row.Cells[HEADCOL_EXPORT];
                    UltraGridCell cellSend = row.Cells[HEADCOL_SEND];
                    cellView.ButtonAppearance.BackColor = Color.White;
                    cellView.ButtonAppearance.BackColor2 = Color.White;
                    cellView.ButtonAppearance.ForeColor = Color.Black;
                    cellExport.ButtonAppearance.BackColor = Color.White;
                    cellExport.ButtonAppearance.BackColor2 = Color.White;
                    cellExport.ButtonAppearance.ForeColor = Color.Black;
                    cellSend.ButtonAppearance.BackColor = Color.White;
                    cellSend.ButtonAppearance.BackColor2 = Color.White;
                    cellSend.ButtonAppearance.ForeColor = Color.Black;
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

        private void GrdJob_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                ThirdPartyBatch batch = (ThirdPartyBatch)e.Cell.Row.ListObject;
                if (ThirdPartyClientManager.ServiceInnerChannel.GetFormat(batch) == null)
                {
                    MessageBox.Show("This batch doesn't exist", "Batch Doesn't Exist", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                tabPage1.Text = "Active Tax Lots - " + batch.Description;

                if (e.Cell.Row.Index == -1 || e.Cell.Row.Index == -1) return;
                string HeaderText = e.Cell.Column.Header.Caption;

                if (dataView.DataSource == null)
                    txtOutput.Text = "";
                else if (batch.Format != null && File.Exists(batch.Format.FilePath))
                {
                    if (!batch.Format.FileName.ToLower().Contains(".xls"))
                        txtOutput.Text = File.ReadAllText(batch.Format.FilePath);
                }
                dataView.SetColumnHidden();
                dataView.Refresh();
                var dataSource = new DataSet();
                if (!string.IsNullOrEmpty(batch.SerializedDataSource))
                {
                    dataSource.ReadXml(new StringReader(batch.SerializedDataSource));
                    if (!_isCaptionUpdated)
                    {
                        dataView.UpdateCaptions(dataSource);
                        _isCaptionUpdated = true;
                    }
                }

                if (batch == null)
                {
                    LastRowIndex = e.Cell.Row.Index;
                    return;
                }
                if (LastRowIndex == -1)
                    LastRowIndex = e.Cell.Row.Index;
                else
                {
                    dataView.SaveState(((ThirdPartyBatch)Jobs[LastRowIndex]).LayoutFile);
                }

                dataView.LoadState(((ThirdPartyBatch)Jobs[e.Cell.Row.Index]).LayoutFile);
                dataView.DataSource = null;
                if (!string.IsNullOrEmpty(((ThirdPartyBatch)Jobs[e.Cell.Row.Index]).SerializedDataSource))
                {
                    dataSource = new DataSet();
                    dataSource.ReadXml(new StringReader(((ThirdPartyBatch)Jobs[e.Cell.Row.Index]).SerializedDataSource));
                    dataView.DataSource = dataSource;
                    dataView.AddCheckBoxinGrid();
                    var filteredDataSet = dataView.GetFilteredRowDataSet();
                    if (filteredDataSet != null)
                    {
                        ((ThirdPartyBatch)Jobs[e.Cell.Row.Index]).SerializedDataSet = filteredDataSet.GetXml();
                    }
                }
                else
                {
                    dataView.DataSource = dataSource;
                    dataView.AddCheckBoxinGrid();
                }

                dataView.SetColumnHidden();
                dataView.Refresh();
                LastRowIndex = e.Cell.Row.Index;
                //ChangeCellAppearance();
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

        private void GrdJob_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void OutPut_txtOuputClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_outputFilePath) && !string.IsNullOrWhiteSpace(_outputFilePath))
                {
                    ThirdPartyBatch batch = (ThirdPartyBatch)GrdJob.ActiveRow.ListObject;
                    String delimiter = string.Empty;
                    if (batch != null && ThirdPartyClientManager.ServiceInnerChannel.GetFormat(batch) != null)
                    {
                        delimiter = ThirdPartyClientManager.ServiceInnerChannel.GetFormat(batch).Delimiter;
                    }
                    frmThirdPartyEditDetails edit = frmThirdPartyEditDetails.GetInstance(_outputFilePath, delimiter);
                    edit.ShowDialog();
                    LoadOutput(_outputFilePath, batch.Description, batch.LogFile);
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
        /// Called when [message].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Prana.ThirdPartyReport.MessageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        void OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                string logFile = e.LogFile;
                string description = e.Description;
                string statusMessage = e.StatusFormattedMessage;
                ThirdPartyBatch batch = null;
                if (GrdJob.ActiveRow != null && GrdJob.ActiveRow.ListObject != null)
                    batch = GrdJob.ActiveRow.ListObject as ThirdPartyBatch;

                if (string.IsNullOrEmpty(statusMessage))
                {
                    if (batch != null)
                    {
                        statusMessage = string.Format("{4} - [{0}]  {3} {1}{2}",
                            batch.Description, e.Message, Environment.NewLine, RunDate.Value.ToShortDateString(), DateTime.Now.ToShortTimeString());
                        ThirdPartyClientManager.ServiceInnerChannel.LogMessage(batch.LogFile, statusMessage);
                    }
                    else
                    {
                        statusMessage = string.Format("{4} - [{0}]  {3} {1}{2}",
                             "", e.Message, Environment.NewLine, RunDate.Value.ToShortDateString(), DateTime.Now.ToShortTimeString());
                    }
                }

                if (batch != null && !string.IsNullOrEmpty(logFile) && !string.IsNullOrEmpty(description)
                   && !string.IsNullOrEmpty(batch.LogFile) && !string.IsNullOrEmpty(batch.Description)
                   && batch.LogFile.Equals(logFile) && batch.Description.Equals(description))
                {
                    WriteStatusMessageonUI(e.Message, statusMessage);
                }

                if (!DisablePrompts)
                {
                    MessageBox.Show(e.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Called when [status].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Prana.BusinessObjects.StatusEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        void OnStatus(object sender, StatusEventArgs e)
        {
            try
            {
                string logFile = e.LogFile;
                string description = e.Description;
                string statusMessage = e.StatusFormattedMessage;
                string txt = e.Text;
                ThirdPartyBatch activeRowBatch = null;
                if (GrdJob.ActiveRow != null && GrdJob.ActiveRow.ListObject != null)
                    activeRowBatch = GrdJob.ActiveRow.ListObject as ThirdPartyBatch;

                if (string.IsNullOrEmpty(statusMessage))
                {
                    if (activeRowBatch != null)
                    {
                        statusMessage = string.Format("{4} - [{0}]  {3} {1}{2}", activeRowBatch.Description, txt, Environment.NewLine, RunDate.Value.ToShortDateString(), DateTime.Now.ToShortTimeString());
                        ThirdPartyClientManager.ServiceInnerChannel.LogMessage(activeRowBatch.LogFile, statusMessage);
                    }
                    else
                    {
                        statusMessage = string.Format("{4} - [{0}]  {3} {1}{2}", "", txt, Environment.NewLine, RunDate.Value.ToShortDateString(), DateTime.Now.ToShortTimeString());
                    }
                }

                if (activeRowBatch != null && !string.IsNullOrEmpty(logFile) && !string.IsNullOrEmpty(description)
                    && !string.IsNullOrEmpty(activeRowBatch.LogFile) && !string.IsNullOrEmpty(activeRowBatch.Description)
                    && activeRowBatch.LogFile.Equals(logFile) && activeRowBatch.Description.Equals(description))
                {
                    WriteStatusMessageonUI(txt, statusMessage);
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void WriteStatusMessageonUI(string txtMessage, string statusFormattedMessage)
        {
            try
            {
                _uiSynchronizationContext.Send(new SendOrPostCallback(
                    delegate (object state)
                    {
                        tsStatus.Text = txtMessage;
                        txtLog.AppendText(statusFormattedMessage);
                        txtLog.SelectionStart = txtLog.Text.Length;
                        txtLog.ScrollToCaret();
                    }
                ), null);
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Loads the output.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <remarks></remarks>
        private void LoadOutput(string file, string description, string logFile)
        {
            try
            {
                if (File.Exists(file) && !file.ToLower().Contains(".xls"))
                {
                    txtOutput.Text = File.ReadAllText(file);
                    tabControl1.SelectedTab = tabPage4;
                }
            }
            catch (Exception ex)
            {
                OnMessage(this, new MessageEventArgs(ex.Message, "", description, logFile));
            }
        }

        /// <summary>
        /// Create necessary folders for storing EOD files
        /// </summary>
        /// <param name="folders"></param>
        private void CreateFolders(params string[] folders)
        {
            try
            {
                foreach (string folder in folders)
                {
                    if (Directory.Exists(folder) == false)
                    {
                        Directory.CreateDirectory(folder);
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

        private void SetAppearanceWithTheme()
        {
            try
            {

                this.imageList1.TransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.tsStatus.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(232)))), ((int)(((byte)(233)))));
                this.tsLoad.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.btnView.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.btnExport.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.btnSend.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.tsToggleBatch.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(232)))), ((int)(((byte)(233)))));

                this.tabPage4.BackColor = System.Drawing.Color.FromArgb(231, 232, 233);
                this.tabPage4.ForeColor = System.Drawing.Color.Black;

                this.tabPage5.BackColor = System.Drawing.Color.FromArgb(231, 232, 233);
                this.tabPage5.ForeColor = System.Drawing.Color.Black;

                this.txtOutput.BackColor = System.Drawing.Color.FromArgb(231, 232, 233);
                this.txtOutput.ForeColor = System.Drawing.Color.Black;

                this.txtLog.BackColor = System.Drawing.Color.FromArgb(231, 232, 233);
                this.txtLog.ForeColor = System.Drawing.Color.Black;

                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
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

        private void SetAppearancesWithoutTheme()
        {
            try
            {
                this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
                this.tsStatus.ImageTransparentColor = System.Drawing.Color.Fuchsia;
                this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                this.tsLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.btnView.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.btnSend.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsToggleBatch.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                txtLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
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
        /// Handles the Click event of the tsToggleBatch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void tsToggleBatch_Click(object sender, EventArgs e)
        {
            try
            {
                SetCheckState();
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
        /// Handles the Click event of the exitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to Exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result.Equals(DialogResult.Yes))
                {
                    Close();
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
        /// Handles the Click event of the tableManagerToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void tableManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmThirdPartyEditor editor = new frmThirdPartyEditor())
                {
                    editor.ShowDialog();
                    tsLoad_Click(sender, e);
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
        /// References this instance.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public Form Reference()
        {
            return this;
        }

        /// <summary>
        /// Handles the FormClosing event of the frmThirdParty control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void frmThirdParty_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ThirdPartyFlatFileClosed != null)
                {
                    ThirdPartyFlatFileClosed(this, EventArgs.Empty);
                }
                InstanceManager.ReleaseInstance(typeof(frmThirdParty));
                Dispose();
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
        /// Handles the Click event of the btnView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("View all active Jobs?", "View", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) return;
                DisablePrompts = true;
                this.btnView.Enabled = false;
                this.toolStrip1.Enabled = false;
                this.tabControl1.SelectTab(this.tabPage5);
                ChangeCellAppearance();
                Application.DoEvents();
                ViewAllActiveJobs();
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
        /// Handles the Click event of the btnExport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Export all active Jobs?", "Export", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) return;
                DisablePrompts = true;
                this.btnExport.Enabled = false;
                this.toolStrip1.Enabled = false;
                this.tabControl1.SelectTab(this.tabPage5);
                ChangeCellAppearance();
                Application.DoEvents();
                ExportAllActiveJobsAsync();
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
        /// Handles the Click event of the btnSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Send all active Jobs?", "Send", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) return;
                DisablePrompts = true;
                this.btnSend.Enabled = false;
                this.toolStrip1.Enabled = false;
                this.tabControl1.SelectTab(this.tabPage5);
                ChangeCellAppearance();
                Application.DoEvents();
                SendAllActiveJobsAsync();
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
        /// Handles the View event of the worker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ViewAllActiveJobs()
        {
            try
            {
                UltraGridRow[] rows = GrdJob.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells[HEADCOL_CHECKBOX].Text.Equals("True"))
                    {
                        ThirdPartyBatch batch = row.ListObject as ThirdPartyBatch;
                        if (batch.Active)
                        {
                            if (ThirdPartyClientManager.ServiceInnerChannel.CheckForUnallocatedTrades(this.RunDate.Value))
                            {
                                this.OnMessage(this, new MessageEventArgs(" Warning: There are some trades which are still unallocated.", "", batch.Description, batch.LogFile));
                            }
                            this.GrdJob.ActiveRow = row;
                            this.OnMessage(this, new MessageEventArgs(string.Format("\r\n* * * Processing View {0} * * *\r\n", batch.Description), "", batch.Description, batch.LogFile));
                            var updatedBatch = ThirdPartyClientManager.ServiceInnerChannel.View(batch, this.RunDate.Value, true);
                            Jobs[row.Index] = updatedBatch;
                        }
                    }
                }
                _isCaptionUpdated = true;
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
            finally
            {
                this.btnView.Enabled = true;
                this.toolStrip1.Enabled = true;
                DisablePrompts = false;
            }
        }

        /// <summary>
        /// Handles the Export event of the worker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private async void ExportAllActiveJobsAsync()
        {
            try
            {
                UltraGridRow[] rows = GrdJob.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells[HEADCOL_CHECKBOX].Text.Equals("True"))
                    {
                        ThirdPartyBatch batch = row.ListObject as ThirdPartyBatch;
                        if (batch.Active)
                        {
                            batch.SerializedDataSet = batch.SerializedDataSource;
                            var dataSource = new DataSet();
                            if (!string.IsNullOrEmpty(batch.SerializedDataSet))
                            {
                                dataSource.ReadXml(new StringReader(batch.SerializedDataSet));
                            }
                            this.dataView.DataSource = dataSource;
                            this.dataView.AddCheckBoxinGrid();
                            this.dataView.SetColumnHidden();
                            var dataSet = dataView.GetFilteredRowDataSet(true);
                            if (dataSet != null)
                                batch.SerializedDataSet = dataSet.GetXml();
                            this.GrdJob.ActiveRow = row;
                            this.OnMessage(this, new MessageEventArgs(string.Format("\r\n* * * Exporting {0} * * *\r\n", batch.Description), "", batch.Description, batch.LogFile));
                            await System.Threading.Tasks.Task.Run(() =>
                            {
                                ThirdPartyClientManager.ServiceInnerChannel.ExportFile(batch, this.RunDate.Value);
                            });
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
            finally
            {
                this.btnExport.Enabled = true;
                this.toolStrip1.Enabled = true;
                DisablePrompts = false;
            }

        }

        /// <summary>
        /// Handles the Send event of the worker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void SendAllActiveJobsAsync()
        {
            try
            {
                UltraGridRow[] rows = GrdJob.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells[HEADCOL_CHECKBOX].Text.Equals("True"))
                    {
                        ThirdPartyBatch batch = row.ListObject as ThirdPartyBatch;
                        if (batch.Active && CheckFixConnectionAndDisplayMessage(batch))
                        {
                            this.GrdJob.ActiveRow = row;
                            this.OnMessage(this, new MessageEventArgs(string.Format("\r\n* * * Sending {0} * * *\r\n", batch.Description), "", batch.Description, batch.LogFile));
                            ThirdPartyClientManager.ServiceInnerChannel.SendFile(batch, RunDate.Value, CachedDataManager.GetInstance.LoggedInUser.FirstName);
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
            finally
            {
                this.btnSend.Enabled = true;
                this.toolStrip1.Enabled = true;
                DisablePrompts = false;
            }
        }

        private void MakeProxy()
        {
            try
            {
                _subscriptionProxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _subscriptionProxy.Subscribe(Topics.Topic_ThirdPartyStatusMessage, null);
                _subscriptionProxy.Subscribe(Topics.Topic_ThirdPartyMessage, null);
                _subscriptionProxy.Subscribe(Topics.Topic_ThirdPartyDuplicateFileConfirmation, null);
                _subscriptionProxy.Subscribe(Topics.Topic_ThirdPartyAllocationMatchStatusUpdate, null);
                _subscriptionProxy.Subscribe(Topics.Topic_ThirdPartyMismatchFileConfirmation, null);
                _subscriptionProxy.Subscribe(Topics.Topic_ThirdPartyAutomatedBatchStatus, null);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { Publish(e, topicName); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        lock (_locker)
                        {
                            object[] dataList = (object[])e.EventData;
                            switch (e.TopicName)
                            {
                                case Topics.Topic_ThirdPartyMessage:
                                    var messageArgs = (MessageEventArgs)dataList[0];
                                    OnMessage(this, messageArgs);
                                    break;
                                case Topics.Topic_ThirdPartyStatusMessage:
                                    var statusMessageArgs = (StatusEventArgs)dataList[0];
                                    OnStatus(this, statusMessageArgs);
                                    break;
                                case Topics.Topic_ThirdPartyDuplicateFileConfirmation:
                                    var message = (string)dataList[0];
                                    var batch = (ThirdPartyBatch)dataList[1];
                                    if (MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        batch.HaveOverridePBDuplicateFileWarning = true;
                                        ThirdPartyClientManager.ServiceInnerChannel.EncryptAndSendFile(batch, RunDate.Value, CachedDataManager.GetInstance.LoggedInUser.FirstName);
                                    }
                                    else OnStatus(this, new StatusEventArgs("File(s) already sent", batch.LogFile, batch.Description));
                                    break;
                                case Topics.Topic_ThirdPartyAllocationMatchStatusUpdate:
                                    var thirdPartyAllocationMatchDetails = (ThirdPartyAllocationMatchDetails)dataList[0];
                                    UpdateAllocationMatchStatus(thirdPartyAllocationMatchDetails);
                                    break;
                                case Topics.Topic_ThirdPartyMismatchFileConfirmation:
                                    message = (string)dataList[0];
                                    batch = (ThirdPartyBatch)dataList[1];
                                    if (MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        batch.HaveFoundPBMismatchOverride = true;
                                        ThirdPartyClientManager.ServiceInnerChannel.EncryptAndSendFile(batch, RunDate.Value, CachedDataManager.GetInstance.LoggedInUser.FirstName);
                                    }
                                    break;
                                case Topics.Topic_ThirdPartyAutomatedBatchStatus:
                                    Dictionary<int, string> dictAutomatedBatchStatus = Global.Utilities.JsonHelper.DeserializeToObject<Dictionary<int, string>>(dataList[0].ToString());
                                    bool isSingleBatchUpdate = (bool)dataList[1];
                                    UpdateThirdPartyAutomatedBatchStatus(dictAutomatedBatchStatus, isSingleBatchUpdate);
                                    break;                             
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
        /// Updates Allocation Match Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateAllocationMatchStatus(ThirdPartyAllocationMatchDetails thirdPartyAllocationMatchDetails)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            UpdateAllocationMatchStatus(thirdPartyAllocationMatchDetails);
                        }));
                    }
                    else
                    {
                        foreach (UltraGridRow row in GrdJob.Rows)
                        {
                            ThirdPartyBatch thirdPartyBatch = row.ListObject as ThirdPartyBatch;
                            if (thirdPartyBatch.ThirdPartyBatchId == thirdPartyAllocationMatchDetails.ThirdPartyBatchId)
                            {
                                UpdateAllocationMatchStatusButton(row, thirdPartyAllocationMatchDetails.AllocationMatchStatus);
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
        /// Updates UI of GrdJob columns
        /// </summary>
        /// <param name="GrdJob">GrdJob</param>
        private void UpdateColumnsUI(PranaUltraGrid grid)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            UpdateColumnsUI(grid);
                        }));
                    }
                    else
                    {
                        bool isInvalidRunDate = false;
                        if(DateTime.Compare(RunDate.Value, DateTime.Today.AddDays(-5)) < 1)
                        {
                            isInvalidRunDate = true;
                        }
                        foreach (var row in grid.Rows)
                        {
                            if (row.Cells["AllowedFixTransmission"].Value == null || !bool.Parse(row.Cells["AllowedFixTransmission"].Value.ToString()) || !((ThirdPartyBatch)Jobs[row.Index]).FileEnabled)
                            {
                                row.Cells[HEADCOL_TRANSMISSIONTYPE].Activation = Activation.Disabled;
                            }
                            if (row.Cells[HEADCOL_TRANSMISSIONTYPE].Value.Equals(((int)TransmissionType.FIX).ToString()))
                            {
                                row.Cells[HEADCOL_FIXCONNECTIONSTATUS].Appearance.ForeColor = row.Cells[HEADCOL_FIXCONNECTIONSTATUS].Value.ToString() == ThirdPartyConstants.FIX_CONNECTIONSTATUS_CONNECTED ? Color.Green : Color.Red;
                            }

                            string result = row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].Value.ToString();
                            AllocationMatchStatus status = AllocationMatchStatus.NotSent;
                            Enum.TryParse(result, out status);
                            UpdateAllocationMatchStatusButton(row, status);

                            if (isInvalidRunDate)
                            {
                                row.Cells[HEADCOL_VIEW].Activation = Activation.Disabled;
                                row.Cells[HEADCOL_EXPORT].Activation = Activation.Disabled;
                                row.Cells[HEADCOL_SEND].Activation = Activation.Disabled;
                                btnView.Enabled = false;
                                btnExport.Enabled = false;
                                btnSend.Enabled = false;
                            }
                            else
                            {
                                btnView.Enabled = true;
                                btnExport.Enabled = true;
                                btnSend.Enabled = true;
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
        /// Updates Allocation Match Status Button
        /// </summary>
        /// <param name="row">Row for which button needs update.</param>
        /// <param name="status">Allocation Match Status</param>
        private void UpdateAllocationMatchStatusButton(UltraGridRow row, AllocationMatchStatus status)
        {
            try
            {
                row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].Value = EnumHelper.GetDescriptionWithDescriptionAttribute(status);
                if(status == AllocationMatchStatus.NotSent || status == AllocationMatchStatus.Pending)
                {
                    row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].ButtonAppearance.Reset();
                }
                else
                {
                    row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].ButtonAppearance.ForeColor = Color.White;
                    row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].ButtonAppearance.BackGradientStyle = GradientStyle.None;

                    switch (status)
                    {
                        case AllocationMatchStatus.CompleteMatch:
                            row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].ButtonAppearance.BackColor = Color.Green;
                            break;

                        case AllocationMatchStatus.PartialMismatch:
                            row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].ButtonAppearance.BackColor = Color.Red;
                            break;

                        case AllocationMatchStatus.CompleteMismatch:
                            row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].ButtonAppearance.BackColor = Color.Red;
                            break;

                        case AllocationMatchStatus.PartialMatch:
                            row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].ButtonAppearance.BackColor = Color.Orange;
                            break;
                        case AllocationMatchStatus.PendingAcknowledgment:
                            row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].ButtonAppearance.BackColor = ColorTranslator.FromHtml("#FFBF00");
                            break;
                        case AllocationMatchStatus.AllocationAcknowledged:
                            row.Cells[HEADCOL_ALLOCATIONMATCHSTATUS].ButtonAppearance.BackColor = ColorTranslator.FromHtml("#00827F");
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

        public string getReceiverUniqueName()
        {
            return "ThirdPartyForm";
        }

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IDisposable members
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    TradeManagerExtension.GetInstance().CounterPartyStatusUpdate -= ThirdParty_CounterPartyStatusUpdate;
                    if (Jobs != null)
                    {
                        Jobs = null;
                    }
                    if (_subscriptionProxy != null && _subscriptionProxy.InnerChannel != null)
                    {
                        _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_ThirdPartyAllocationMatchStatusUpdate);
                        _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_ThirdPartyDuplicateFileConfirmation);
                        _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_ThirdPartyMessage);
                        _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_ThirdPartyStatusMessage);
                        _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_ThirdPartyMismatchFileConfirmation);
                        _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_ThirdPartyAutomatedBatchStatus);
                        _subscriptionProxy.Dispose();
                    }
                    ThirdPartyClientManager.Dispose();
                    if (components != null)
                    {
                        components.Dispose();
                    }

                    base.Dispose(disposing);
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

        /// <summary>
        /// This class is added for change the color of MenuStripItem on selection
        /// </summary>
        public class MenuStripRenderer : ProfessionalColorTable
        {
            public override Color MenuItemPressedGradientBegin
            {
                get { return Color.FromArgb(158, 156, 157); }
            }
            public override Color MenuItemPressedGradientMiddle
            {
                get { return Color.FromArgb(158, 156, 157); }
            }
            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.FromArgb(158, 156, 157); }
            }
        }

        /// <summary>
        /// Checks the FIX connection status and shows a warning message when FIX line is down.
        /// </summary>
        /// <param name="batch"></param>
        private bool CheckFixConnectionAndDisplayMessage(ThirdPartyBatch batch)
        {
            try
            {
                if (batch.ThirdPartyTypeId == (int)ThirdPartyNodeType.ExecutingBroker && batch.FIXConnectionStatus == ThirdPartyConstants.FIX_CONNECTIONSTATUS_DISCONNECTED)
                {
                    MessageBox.Show(ThirdPartyConstants.FIX_DISCONNECTION_MESSAGE, ThirdPartyConstants.FIX_DISCONNECTION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnStatus(this, new StatusEventArgs(ThirdPartyConstants.STATUS_FIX_GENERATION_UNSUCCESSFUL, batch.LogFile, batch.Description));
                    OnStatus(this, new StatusEventArgs(ThirdPartyConstants.STATUS_ALLOCATION_INSTRUCTIONS_FAILED, batch.LogFile, batch.Description));
                    return false;
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
            return true;
        }

        /// <summary>
        /// Handles the Click event of the FileLogToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FileLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FileLogs fileLogs = new FileLogs();
                fileLogs.LoadData(RunDate.Value);
                fileLogs.ShowDialog();
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
        /// Initializes a row in the grid for displaying ThirdPartyBatch data.
        /// </summary>
        /// <param name="sender">The sender object (usually the grid).</param>
        /// <param name="e">The InitializeRowEventArgs containing row information.</param>
        private void GrdJob_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                ThirdPartyBatch thirdPartyBatch = e.Row.ListObject as ThirdPartyBatch;

                // Set the automated batch status using the ThirdPartyBatchId and row index
                SetAutomatedBatchStatus(thirdPartyBatch, e.Row.Index);
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
        /// Updates the automated batch status for third-party batches.
        /// </summary>
        /// <param name="dictAutomatedBatchStatus">A dictionary containing batch IDs and their corresponding statuses.</param>
        /// <param name="isSingleBatchUpdate"></param>
        private void UpdateThirdPartyAutomatedBatchStatus(Dictionary<int, string> dictAutomatedBatchStatus, bool isSingleBatchUpdate)
        {
            try
            {
                DateTime currentESTTime = BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, BusinessObjects.TimeZoneInfo.EasternTimeZone);

                if (RunDate.Value.Date == currentESTTime.Date)
                {
                    // Update the batch status for each row in the grid
                    foreach (UltraGridRow row in GrdJob.Rows)
                    {
                        ThirdPartyBatch thirdPartyBatch = row.ListObject as ThirdPartyBatch;

                        if (dictAutomatedBatchStatus.ContainsKey(thirdPartyBatch.ThirdPartyBatchId))
                            thirdPartyBatch.FixAutomatedBatchStatus = dictAutomatedBatchStatus[thirdPartyBatch.ThirdPartyBatchId];
                        else if (!isSingleBatchUpdate)
                            thirdPartyBatch.FixAutomatedBatchStatus = ThirdPartyConstants.CONST_AUTOMATED_BATCH_STATUS_NO_BATCH_SET;

                        SetAutomatedBatchStatus(thirdPartyBatch, row.Index);
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
        /// Sets the automated batch status for a specific ThirdPartyBatch
        /// </summary>
        /// <param name="thirdPartyBatch"></param>
        /// <param name="index">The index of the row in the grid.</param>
        private void SetAutomatedBatchStatus(ThirdPartyBatch thirdPartyBatch, int index)
        {
            try
            {
                if (GrdJob.Rows[index].Cells[HEADCOL_TRANSMISSIONTYPE].Value.Equals(((int)TransmissionType.FIX).ToString()))
                {
                    string status = thirdPartyBatch.FixAutomatedBatchStatus;

                    GrdJob.Rows[index].Cells[HEADCOL_AUTOMATEDBATCHSTATUS].Appearance.ForeColor = status.EndsWith("G") ? Color.Green : status.EndsWith("R") ? Color.Red : Color.Black;

                    if(status.EndsWith("G") || status.EndsWith("R"))
                        status = status.Substring(0, status.Length - 1);

                    GrdJob.Rows[index].Cells[HEADCOL_AUTOMATEDBATCHSTATUS].Value = status;
                }
                else
                {
                    GrdJob.Rows[index].Cells[HEADCOL_AUTOMATEDBATCHSTATUS].Value = ThirdPartyConstants.CONST_AUTOMATED_BATCH_STATUS_NA;
                    GrdJob.Rows[index].Cells[HEADCOL_AUTOMATEDBATCHSTATUS].Appearance.ForeColor = Color.Black;
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
        /// used To export data for automation.
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
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
                exporter.Export(GrdJob, filePath);
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
