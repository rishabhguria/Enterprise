using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.Global;
using Prana.LogManager;
using Prana.CoreService.Interfaces;
using Prana.Utilities;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using Prana.PubSubService.Interfaces;
using Prana.Global.Utilities;

namespace Prana.TradeServiceUI
{
    public partial class TradeServiceUI : Form, IPublishing
    {
        #region Variables
        private bool _pending = false;
        private DuplexProxyBase<ISubscription> _subscriptionProxy;
        private List<UsrCtrlConnectionStatus> _allCounterPartyConnectionControls = new List<UsrCtrlConnectionStatus>();

        private bool _pricingService2ConnectionStatus = false;

        private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;
        #endregion

        #region Constructor
        public TradeServiceUI()
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();

            Text += " - v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        #region Private Methods
        private void TradeServiceUI_Load(object sender, EventArgs e)
        {
            try
            {
                _tradeServiceClientHeartbeatManager = new ClientHeartbeatManager<ITradeService>("TradeServiceEndpointAddress");
                _tradeServiceClientHeartbeatManager.ConnectedEvent += TradeServiceClientHeartbeatManager_ConnectedEvent;
                _tradeServiceClientHeartbeatManager.DisconnectedEvent += TradeServiceClientHeartbeatManager_DisconnectedEvent;
                _tradeServiceClientHeartbeatManager.AnotherInstanceSubscribedEvent += TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent;
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

        private async void TradeServiceClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { TradeServiceClientHeartbeatManager_ConnectedEvent(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        ultraLabelTradeServiceConnectionStatus.Text = "TradeService Connected";

                        MakeProxy();

                        await InitialDataRequestToContainerService();

                        EnableControls();
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

        private void TradeServiceClientHeartbeatManager_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { TradeServiceClientHeartbeatManager_DisconnectedEvent(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        System.Threading.Tasks.Task.Factory.StartNew(() => DisposeProxy()).ConfigureAwait(false);

                        ultraLabelTradeServiceConnectionStatus.Text = "TradeService Disconnected";

                        _allCounterPartyConnectionControls = new List<UsrCtrlConnectionStatus>();
                        listBoxClientsConnected.DataSource = null;
                        DisableControls();
                        ResetControls();
                        DisplayPricingService2Status(false);
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

        private void TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        if (listBoxErrorMessages.Items.Count == 0 || !listBoxErrorMessages.Items[listBoxErrorMessages.Items.Count - 1].ToString().Contains(string.Format(PranaMessageConstants.MSG_AnotherInstanceSubscribed, e.Value, e.Value2)))
                            listBoxErrorMessages.Items.Add(DateTime.Now.ToString("M/d/yyyy hh:mm:ss tt") + " : " + string.Format(PranaMessageConstants.MSG_AnotherInstanceSubscribed, e.Value, e.Value2));
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

        private void MakeProxy()
        {
            try
            {
                if (_subscriptionProxy == null)
                {
                    _subscriptionProxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                    _subscriptionProxy.Subscribe(Topics.Topic_TradeServiceLogsData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_TradeServiceConnectedUserData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_TradeServicePricingConnectionData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_TradeServiceFixConnectionData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_TradeServiceFixAutoConnectionStatus, null);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void DisposeProxy()
        {
            try
            {
                if (_subscriptionProxy != null)
                {
                    _subscriptionProxy.UnSubscribe();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void AddMessagesToolStripMenuItems(List<string> processorNames, ToolStripMenuItem toolStrip)
        {
            try
            {
                foreach (string processorName in processorNames)
                {
                    ToolStripItem messagesToolStripItem = new ToolStripMenuItem();
                    messagesToolStripItem.Name = processorName;
                    messagesToolStripItem.Size = new System.Drawing.Size(66, 20);
                    messagesToolStripItem.Text = processorName;
                    toolStrip.DropDownItems.Add(messagesToolStripItem);
                    messagesToolStripItem.Click += new EventHandler(messagesToolStripItem_Click);
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

        private void AddErrorMessagesToolStripMenuItems(List<string> processorNames, ToolStripMenuItem toolStrip)
        {
            try
            {
                foreach (string processorName in processorNames)
                {
                    ToolStripItem errorMessagesToolStripItem = new ToolStripMenuItem();
                    errorMessagesToolStripItem.Name = processorName;
                    errorMessagesToolStripItem.Size = new System.Drawing.Size(66, 20);
                    errorMessagesToolStripItem.Text = processorName;
                    toolStrip.DropDownItems.Add(errorMessagesToolStripItem);
                    errorMessagesToolStripItem.Click += new EventHandler(errorMessagesToolStripItem_Click);
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

        private void BindMessageGrid(System.Data.DataTable dt)
        {
            try
            {
                ultraGridMessages.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.None;
                ultraGridMessages.DataSource = null;
                ultraGridMessages.DataSource = dt;
                ultraGridMessages.DataBind();
                if (ultraGridMessages.DataSource != null)
                {
                    if (ultraGridMessages.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        ShowDisplayColumns(ultraGridMessages);
                        ultraGridMessages.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
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

        private void BindMessageGrid(OrderCollection orderCollection)
        {
            try
            {
                ultraGridMessages.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.None;
                ultraGridMessages.DataSource = null;
                ultraGridMessages.DataSource = orderCollection;
                ultraGridMessages.DataBind();

                if (ultraGridMessages.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    ultraGridMessages.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                    ShowDisplayColumns(ultraGridMessages);
                }

                String path = Application.StartupPath + "\\FixMssingTrades.xml";
                if (File.Exists(path))
                {
                    ultraGridMessages.DisplayLayout.LoadFromXml(path);
                }
                if (ultraGridMessages.DisplayLayout.Bands[0].Columns.Exists("CounterPartyName"))
                {
                    ultraGridMessages.DisplayLayout.Bands[0].Columns["CounterPartyName"].Header.Caption = ApplicationConstants.CONST_BROKER;
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

        private void ShowDisplayColumns(UltraGrid grid)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns["CounterPartyName"].Header.Caption = ApplicationConstants.CONST_BROKER;
                string[] ColumnNames = Enum.GetNames(typeof(OrderFields.ServerDisplayColumns));
                int i = 0;

                foreach (UltraGridColumn column in grid.DisplayLayout.Bands[0].Columns)
                {
                    column.Hidden = true;
                }
                foreach (string columnName in ColumnNames)
                {
                    UltraGridColumn column = grid.DisplayLayout.Bands[0].Columns[columnName];
                    column.Hidden = false;
                    column.Header.VisiblePosition = i;
                    i++;
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

        private async System.Threading.Tasks.Task SetCounterPartiesUI()
        {
            try
            {
                int count = 0;
                _allCounterPartyConnectionControls = new List<UsrCtrlConnectionStatus>();

                Dictionary<int, FixPartyDetails> fixPartyDetails = await TradeServiceManager.TradeServiceManager.GetInstance.GetFixAllPartyDetails();
                foreach (KeyValuePair<int, FixPartyDetails> fixParty in fixPartyDetails)
                {
                    UsrCtrlConnectionStatus usrCtrlConnectionStatus = new UsrCtrlConnectionStatus(fixParty.Key, fixParty.Value.PartyName);
                    _allCounterPartyConnectionControls.Add(usrCtrlConnectionStatus);
                    ultraPanelFixConnections.ClientArea.Controls.Add(usrCtrlConnectionStatus);
                    usrCtrlConnectionStatus.Location = new Point(0, count * usrCtrlConnectionStatus.Height + 10);
                    usrCtrlConnectionStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    count++;
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

        private void DisplayPricingService2Status(bool isConnected)
        {
            try
            {
                if (isConnected)
                {
                    ultraLabelPricingService2ConnectionStatus.Text = "PricingService2 Connected";
                }
                else
                {
                    ultraLabelPricingService2ConnectionStatus.Text = "PricingService2 Disconnected";
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

        private DialogResult GetOrderIDFromInputBox(ref string OrderID)
        {
            DialogResult result = DialogResult.None;
            try
            {
                OrderID = InputBox.ShowInputBox("Enter OrderID", out result).Trim();

                // Don't do anything if user has hit cancel or closed the dialog from left top "X"
                if (result == DialogResult.Cancel)
                    return result;

                if (!GeneralUtilities.CheckOrderIDValidation(OrderID))
                {
                    MessageBox.Show(this, "Invalid OrderID.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    OrderID = string.Empty;
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
            return result;
        }

        private async System.Threading.Tasks.Task InitialDataRequestToContainerService()
        {
            try
            {
                if (await TradeServiceManager.TradeServiceManager.GetInstance.GetDebugModeStatus())
                {
                    this.ultraCheckEditorDebugMode.CheckedChanged -= new System.EventHandler(this.ultraCheckEditorDebugMode_CheckedChanged);
                    ultraCheckEditorDebugMode.Checked = true;
                    this.ultraCheckEditorDebugMode.CheckedChanged += new System.EventHandler(this.ultraCheckEditorDebugMode_CheckedChanged);
                }
                else
                {
                    this.ultraCheckEditorDebugMode.CheckedChanged -= new System.EventHandler(this.ultraCheckEditorDebugMode_CheckedChanged);
                    ultraCheckEditorDebugMode.Checked = false;
                    this.ultraCheckEditorDebugMode.CheckedChanged += new System.EventHandler(this.ultraCheckEditorDebugMode_CheckedChanged);
                }

                List<string> processorNames = await TradeServiceManager.TradeServiceManager.GetInstance.FetchProcessorNames();
                AddMessagesToolStripMenuItems(processorNames, showMessagesToolStripMenuItem);
                AddErrorMessagesToolStripMenuItems(processorNames, showErrorMessagesToolStripMenuItem);
                await SetCounterPartiesUI();

                if (await TradeServiceManager.TradeServiceManager.GetInstance.IsComplianceModulePermitted())
                {
                    pendingPreTradeComplianceToolStripMenuItem.Visible = true;
                }
                else
                {
                    pendingPreTradeComplianceToolStripMenuItem.Visible = false;
                }

                await TradeServiceManager.TradeServiceManager.GetInstance.RequestStartupData();
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

        private void EnableControls()
        {
            try
            {
                menuStrip1.Enabled = true;
                ultraCheckEditorFixConnectionsAutoReconnect.Enabled = true;
                ultraGridMessages.Enabled = true;
                ultraCheckEditorDebugMode.Enabled = true;
                ultraButtonDisconnectUser.Enabled = true;
                ultraButtonLoadLog.Enabled = true;
                ultraButtonClearLog.Enabled = true;
                ultraButtonReloadRules.Enabled = true;
                ultraButtonReloadXslt.Enabled = true;
                ultraButtonStopServiceAndUI.Enabled = true;
                ultraPanelFixConnections.Enabled = true;
                ultraButtonGetMessageStatus.Enabled = true;
                ultraButtonMoveOldTrade.Enabled = true;
                ultraButtonOpenLog.Enabled = true;
                ultraButtonExcel.Enabled = true;
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

        private void DisableControls()
        {
            try
            {
                menuStrip1.Enabled = false;
                ultraCheckEditorFixConnectionsAutoReconnect.Enabled = false;
                ultraGridMessages.Enabled = false;
                ultraCheckEditorDebugMode.Enabled = false;
                ultraButtonDisconnectUser.Enabled = false;
                ultraButtonLoadLog.Enabled = false;
                ultraButtonClearLog.Enabled = false;
                ultraButtonReloadRules.Enabled = false;
                ultraButtonReloadXslt.Enabled = false;
                ultraButtonStopServiceAndUI.Enabled = false;
                ultraPanelFixConnections.Enabled = false;
                ultraButtonGetMessageStatus.Enabled = false;
                ultraButtonMoveOldTrade.Enabled = false;
                ultraButtonOpenLog.Enabled = false;
                ultraButtonExcel.Enabled = false;
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

        private void ResetControls()
        {
            try
            {
                if (showMessagesToolStripMenuItem.DropDownItems != null)
                    showMessagesToolStripMenuItem.DropDownItems.Clear();

                if (showErrorMessagesToolStripMenuItem.DropDownItems != null)
                    showErrorMessagesToolStripMenuItem.DropDownItems.Clear();

                _allCounterPartyConnectionControls.Clear();
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
        /// Handles the Click event of the manualDropsToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private async void manualDropsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            await TradeServiceManager.TradeServiceManager.GetInstance.SendManualDropsOnFix();
        }
        #endregion

        #region UI Events
        private void ultraGridMessages_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                Order order = e.Row.ListObject as Order;
                e.Row.Appearance.BackColor = Color.Black;
                if (order != null)
                {
                    if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_New)
                    {
                        e.Row.Appearance.ForeColor = Color.LightBlue;
                    }
                    else if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Filled)
                    {
                        e.Row.Appearance.ForeColor = Color.FromArgb(177, 216, 64);
                    }
                    else if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PartiallyFilled)
                    {
                        e.Row.Appearance.ForeColor = Color.Blue;
                    }
                    else if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                    {
                        e.Row.Appearance.ForeColor = Color.White;
                    }
                    else if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected)
                    {
                        e.Row.Appearance.ForeColor = Color.Red;
                    }
                    else if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled)
                    {
                        e.Row.Appearance.ForeColor = Color.Yellow;
                    }
                    else if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver)
                    {
                        e.Row.Appearance.ForeColor = Color.Gray;
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

        private async void ultraButtonOpenLog_Click(object sender, EventArgs e)
        {
            try
            {
                Byte[] buffer = await TradeServiceManager.TradeServiceManager.GetInstance.OpenLog();

                System.IO.File.WriteAllText(@"log.txt", Encoding.Unicode.GetString(buffer, 0, buffer.Length));
                System.Diagnostics.Process.Start(@"log.txt");
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

        private async void ultraButtonLoadLog_Click(object sender, EventArgs e)
        {
            try
            {
                Byte[] buffer = await TradeServiceManager.TradeServiceManager.GetInstance.LoadLog();
                string[] lines = Encoding.Unicode.GetString(buffer, 0, buffer.Length).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                listBoxErrorMessages.Items.Clear();
                listBoxErrorMessages.Items.AddRange(lines);

                if (listBoxErrorMessages.Items.Count > 1)
                {
                    listBoxErrorMessages.Items.RemoveAt(listBoxErrorMessages.Items.Count - 1);
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

        private void ultraButtonClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxErrorMessages.Items.Clear();
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

        private async void ultraButtonStopServiceAndUI_Click(object sender, EventArgs e)
        {
            try
            {
                if (!await TradeServiceManager.TradeServiceManager.GetInstance.IsTradeServiceReadyForClose())
                {
                    DialogResult result = MessageBox.Show("Do you wish to stop the TradeService & UI?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                    if (result == DialogResult.Yes)
                    {
                        ultraButtonStopServiceAndUI.Enabled = false;
                        listBoxErrorMessages.Items.Add("Started saving any unsaved orders/groups at " + DateTime.Now);

                        try
                        {
                            await TradeServiceManager.TradeServiceManager.GetInstance.StopService();
                            Application.Exit();
                        }
                        catch (CommunicationException)
                        {
                            Application.Exit();
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

        private async void ultraButtonReloadRules_Click(object sender, EventArgs e)
        {
            try
            {
                await TradeServiceManager.TradeServiceManager.GetInstance.ReloadRules();
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

        private async void ultraButtonReloadXslt_Click(object sender, EventArgs e)
        {
            try
            {
                await TradeServiceManager.TradeServiceManager.GetInstance.ReloadXslt();
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

        private void ultraButtonReloadXslt_MouseHover(object sender, EventArgs e)
        {
            try
            {
                toolTip1.SetToolTip(this.ultraButtonReloadXslt, "Reload Symbol Transformation Xslt");
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

        private async void ultraButtonGetMessageStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraGridMessages.ActiveRow != null)
                {
                    Order order = (Order)ultraGridMessages.ActiveRow.ListObject;
                    await TradeServiceManager.TradeServiceManager.GetInstance.GetMessageStatus(order);
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

        private async void ultraButtonMoveOldTrade_Click(object sender, EventArgs e)
        {
            try
            {
                string oldText = ultraButtonMoveOldTrade.Text;
                ultraButtonMoveOldTrade.Text = "Moving...";

                await TradeServiceManager.TradeServiceManager.GetInstance.MoveOldTrade();

                ultraButtonMoveOldTrade.Text = oldText;
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

        private async void ultraButtonExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelAndPrintUtilities excelAndPrintUtilities = new ExcelAndPrintUtilities();
                await excelAndPrintUtilities.ExportToExcelAsync(ultraGridMessages);
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

        private async void ultraButtonDisconnectUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxClientsConnected.SelectedItem != null)
                {
                    await _tradeServiceClientHeartbeatManager.UnSubscribe(listBoxClientsConnected.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("Please Select a User");
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

        private async void ultraCheckEditorDebugMode_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                await TradeServiceManager.TradeServiceManager.GetInstance.SetDebugModeStatus(ultraCheckEditorDebugMode.Checked);
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

        private async void ultraCheckEditorFixConnectionsAutoReconnect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                await TradeServiceManager.TradeServiceManager.GetInstance.SetFixConnectionsAutoReconnectStatus(ultraCheckEditorFixConnectionsAutoReconnect.Checked);
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

        private async void messagesToolStripItem_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                BindMessageGrid(await TradeServiceManager.TradeServiceManager.GetInstance.ShowMessagesForProcessor(item.Name));
                _pending = false;
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

        private async void errorMessagesToolStripItem_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                BindMessageGrid(await TradeServiceManager.TradeServiceManager.GetInstance.ShowErrorMessagesForProcessor(item.Name));
                _pending = false;
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

        private async void receivedFromClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> response = await TradeServiceManager.TradeServiceManager.GetInstance.PersistedMessagesReceivedFromClient();
                BindMessageGrid(Prana.Utilities.MiscUtilities.DataTableToListConverter.GetDataTableFromList(response));
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

        private async void sentToBrokerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> response = await TradeServiceManager.TradeServiceManager.GetInstance.PersistedMessagesSentToBroker();
                BindMessageGrid(Prana.Utilities.MiscUtilities.DataTableToListConverter.GetDataTableFromList(response));
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

        private async void receivedFromBrokerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> response = await TradeServiceManager.TradeServiceManager.GetInstance.PersistedMessagesReceivedFromBroker();
                BindMessageGrid(Prana.Utilities.MiscUtilities.DataTableToListConverter.GetDataTableFromList(response));
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

        private async void pendingPreTradeComplianceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BindMessageGrid(await TradeServiceManager.TradeServiceManager.GetInstance.PendingPreTradeCompliance());
                _pending = true;
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

        private async void pendingApprovalTradesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BindMessageGrid(await TradeServiceManager.TradeServiceManager.GetInstance.PendingApprovalTrades());
                _pending = true;
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

        private async void refreshCacheClosingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Please check no user is working with closing data." + Environment.NewLine + "Do you want to refresh closing data cache?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    await TradeServiceManager.TradeServiceManager.GetInstance.RefreshCacheClosing();
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
        /// This method handles the click event of Allocation/Closing Preference menu option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void refreshCacheAllocationPreferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Please check no user is working with allocation preference." + Environment.NewLine + "Do you want to refresh allocation preference cache?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    await TradeServiceManager.TradeServiceManager.GetInstance.RefreshPreferenceCache();
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

        private async void clearOrderCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await TradeServiceManager.TradeServiceManager.GetInstance.ClearFixTradeOrderCache();
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

        private async void clearOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string OrderID = string.Empty;
            try
            {
                DialogResult result = GetOrderIDFromInputBox(ref OrderID);
                if (OrderID != string.Empty && result == DialogResult.OK)
                {
                    await TradeServiceManager.TradeServiceManager.GetInstance.ClearFixTradeOrder(OrderID);
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

        private void ultraGridMessages_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (_pending)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        Point point = new System.Drawing.Point(e.X, e.Y);
                        UIElement uiElement = ((UltraGridBase)sender).DisplayLayout.UIElement.ElementFromPoint(point);
                        UltraGridRow row = (UltraGridRow)uiElement.GetContext(typeof(UltraGridRow));

                        if (row != null && row.Selected)
                        {
                            this.ultraGridMessages.ContextMenuStrip = contextMenuStrip2;
                        }
                        else
                        {
                            this.ultraGridMessages.ContextMenuStrip = contextMenuStrip1;
                        }
                    }
                }
                else
                {
                    this.ultraGridMessages.ContextMenuStrip = contextMenuStrip1;
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

        private async void allowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Order order = (Prana.BusinessObjects.Order)ultraGridMessages.ActiveRow.ListObject;
                await TradeServiceManager.TradeServiceManager.GetInstance.OverideTrade(true, order.OrderID);
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

        private async void blockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Order order = (Prana.BusinessObjects.Order)ultraGridMessages.ActiveRow.ListObject;
                await TradeServiceManager.TradeServiceManager.GetInstance.OverideTrade(false, order.OrderID);
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

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String filePath = Application.StartupPath + "\\FixMssingTrades.xml";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                ultraGridMessages.DisplayLayout.SaveAsXml(filePath);
                InformationReporter.GetInstance.Write("Grid layout has been Saved!");
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

        private async void processAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraGridMessages.ActiveRow != null && ultraGridMessages.ActiveRow.IsDataRow)
                {
                    Order pranaOrder = ultraGridMessages.ActiveRow.ListObject as Order;
                    DataRowView dataRowView = ultraGridMessages.ActiveRow.ListObject as DataRowView;
                    if (dataRowView != null)
                    {
                        DataRow dataRow = (ultraGridMessages.ActiveRow.ListObject as DataRowView).Row;

                        await TradeServiceManager.TradeServiceManager.GetInstance.ReProcessMsg(JsonHelper.SerializeObject(dataRow));
                    }
                    else if (pranaOrder != null)
                    {
                        await TradeServiceManager.TradeServiceManager.GetInstance.ReProcessMsg2(pranaOrder);
                    }
                }
                else
                {
                    InformationReporter.GetInstance.Write("Please select a trade to re-process");
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

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                if (listBoxErrorMessages.SelectedItems != null)
                {
                    foreach (string var in listBoxErrorMessages.SelectedItems)
                    {
                        message.Append(var.ToString());
                        message.Append(Environment.NewLine);
                    }
                }
                if (message.ToString() != string.Empty)
                {
                    Clipboard.SetText(message.ToString(), TextDataFormat.Text);
                }
                else
                {
                    MessageBox.Show("Please select text to copy", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async void ultraPopupControlContainerClientServicesStatus_Opened(object sender, EventArgs e)
        {
            try
            {
                listBoxClientServicesStatus.DisplayMember = "Name";
                listBoxClientServicesStatus.DataSource = null;

                // Showing loading image 
                pictureBoxLoading.Enabled = true;
                pictureBoxLoading.Image = Prana.Global.Properties.Resources.loading;
                pictureBoxLoading.Visible = true;

                // Updating status
                listBoxClientServicesStatus.DataSource = await TradeServiceManager.TradeServiceManager.GetInstance.GetClientServicesStatus();
                listBoxClientServicesStatus.SelectedIndex = -1;
            }
            catch (CommunicationException)
            {
                listBoxClientServicesStatus.DataSource = new List<HostedService>();
            }
            catch (Exception ex)
            {
                listBoxClientServicesStatus.DataSource = null;

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                // Hiding loading image
                pictureBoxLoading.Visible = false;
                pictureBoxLoading.Enabled = false;
            }
        }

        private void listBoxClientServicesStatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index != -1)
                {
                    HostedService service = (sender as ListBox).Items[e.Index] as HostedService;

                    e.DrawBackground();
                    Graphics g = e.Graphics;

                    if (service.IsStarted)
                        g.FillRectangle(new SolidBrush(Color.LightGreen), e.Bounds);
                    else
                        g.FillRectangle(new SolidBrush(Color.FromArgb(128, 239, 0, 0)), e.Bounds);

                    g.DrawString(service.Name, e.Font, new SolidBrush(Color.Black), e.Bounds, StringFormat.GenericDefault);

                    e.DrawFocusRectangle();
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
        #endregion

        #region IPublishing Methods
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
                        switch (e.TopicName)
                        {
                            case Topics.Topic_TradeServiceLogsData:
                                List<string> messageList = (List<string>)e.EventData[0];
                                listBoxErrorMessages.Items.AddRange(messageList.ToArray());
                                break;
                            case Topics.Topic_TradeServiceConnectedUserData:
                                object selectedItem = listBoxClientsConnected.SelectedItem;
                                listBoxClientsConnected.DataSource = null;
                                listBoxClientsConnected.DataSource = (List<string>)e.EventData[0];

                                if (selectedItem != null && listBoxClientsConnected.Items.Contains(selectedItem))
                                    listBoxClientsConnected.SelectedItem = selectedItem;
                                break;
                            case Topics.Topic_TradeServicePricingConnectionData:
                                _pricingService2ConnectionStatus = (bool)e.EventData[0];
                                DisplayPricingService2Status(_pricingService2ConnectionStatus);
                                break;
                            case Topics.Topic_TradeServiceFixConnectionData:
                                foreach (UsrCtrlConnectionStatus control in _allCounterPartyConnectionControls)
                                {
                                    control.UserCtrlConnectionStatusUpdate((FixPartyDetails)e.EventData[0]);
                                }
                                break;
                            case Topics.Topic_TradeServiceFixAutoConnectionStatus:
                                this.ultraCheckEditorFixConnectionsAutoReconnect.CheckedChanged -= new System.EventHandler(this.ultraCheckEditorFixConnectionsAutoReconnect_CheckedChanged);
                                ultraCheckEditorFixConnectionsAutoReconnect.Checked = (bool)e.EventData[0];
                                this.ultraCheckEditorFixConnectionsAutoReconnect.CheckedChanged += new System.EventHandler(this.ultraCheckEditorFixConnectionsAutoReconnect_CheckedChanged);
                                break;
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

        public string getReceiverUniqueName()
        {
            return "TradeServiceUI";
        }
        #endregion

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}