using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.Global;
using Prana.Global.Properties;
using Prana.LogManager;
using Prana.CoreService.Interfaces;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using Prana.PubSubService.Interfaces;

namespace Prana.ExpnlServiceUI
{
    public partial class ExpnlServiceUI : Form, IPublishing
    {
        #region Variables
        private DuplexProxyBase<ISubscription> _subscriptionProxy;
        private ClearanceTimeSetUpForm _clearanceForm = null;

        private ClientHeartbeatManager<IExpnlService> _expnlServiceClientHeartbeatManager;
        #endregion

        #region Constructor
        public ExpnlServiceUI()
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();

            Text += " - v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        #region Private Methods
        private void ExpnlServiceUI_Load(object sender, EventArgs e)
        {
            try
            {
                _expnlServiceClientHeartbeatManager = new ClientHeartbeatManager<IExpnlService>("ExpnlServiceEndpointAddress");
                _expnlServiceClientHeartbeatManager.ConnectedEvent += ExpnlServiceClientHeartbeatManager_ConnectedEvent;
                _expnlServiceClientHeartbeatManager.DisconnectedEvent += ExpnlServiceClientHeartbeatManager_DisconnectedEvent;
                _expnlServiceClientHeartbeatManager.AnotherInstanceSubscribedEvent += ExpnlServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent;
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

        private async void ExpnlServiceClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { ExpnlServiceClientHeartbeatManager_ConnectedEvent(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        ultraButtonStopServiceAndUI.Enabled = true;
                        ultraLabelExpnlServiceConnectionStatus.Text = "Connected";

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

        private void ExpnlServiceClientHeartbeatManager_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { ExpnlServiceClientHeartbeatManager_DisconnectedEvent(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        System.Threading.Tasks.Task.Factory.StartNew(() => DisposeProxy()).ConfigureAwait(false);

                        ultraLabelExpnlServiceConnectionStatus.Text = "Disconnected";
                        ultraButtonStopServiceAndUI.Enabled = false;
                        listBoxClientsConnected.DataSource = null;

                        DisableControls();
                        DisplayPricingStatus(false);
                        DisplayTradeStatus(false);
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

        private void ExpnlServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { ExpnlServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent(sender, e); };
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
                    _subscriptionProxy = new DuplexProxyBase<ISubscription>("ExpnlSubscriptionEndpointAddress", this);
                    _subscriptionProxy.Subscribe(Topics.Topic_ExpnlServiceLogsData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_ExpnlServiceConnectedUserData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_ExpnlServicePricingConnectionData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_ExpnlServiceTradeConnectionData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_ExpnlServiceLiveFeedConnectionData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_ExpnlServiceCompressionData, null);
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

        private async System.Threading.Tasks.Task InitialDataRequestToContainerService()
        {
            try
            {
                if (await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetDebugModeStatus())
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

                numericUpDownRefreshInterval.Value = await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetRefreshTimeInterval();

                if (await ExpnlServiceManager.ExpnlServiceManager.GetInstance.IsDataDumperEnabled())
                {
                    ultraButtonDataDumper.Visible = true;

                    if (await ExpnlServiceManager.ExpnlServiceManager.GetInstance.IsDataDumperRunning())
                        ultraButtonDataDumper.Text = "Stop Data Dumper";
                    else
                        ultraButtonDataDumper.Text = "Start Data Dumper";
                }
                else
                {
                    ultraButtonDataDumper.Visible = false;
                }

                await ExpnlServiceManager.ExpnlServiceManager.GetInstance.RequestStartupData();
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

        private void DisplayLiveFeedStatus(bool isConnected)
        {
            try
            {
                if (isConnected)
                {
                    ultraLabelLivefeedStatus.Text = "Connected";
                }
                else
                {
                    ultraLabelLivefeedStatus.Text = "Disconnected";
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

        private void DisplayPricingStatus(bool isConnected)
        {
            try
            {
                if (isConnected)
                {
                    ultraLabelPricingServiceConnectionStatus.Text = "Connected";
                }
                else
                {
                    ultraLabelPricingServiceConnectionStatus.Text = "Disconnected";
                    DisplayLiveFeedStatus(false);
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

        private void DisplayTradeStatus(bool isConnected)
        {
            try
            {
                if (isConnected)
                {
                    ultraLabelTradeServiceConnectionStatus.Text = "Connected";
                }
                else
                {
                    ultraLabelTradeServiceConnectionStatus.Text = "Disconnected";
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

        private void DisplayCompressionName(string compressionName)
        {
            try
            {
                ultraLabelCompressionName.Text = compressionName + " Compression";
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
                ultraButtonOpenLog.Enabled = true;
                ultraButtonLoadLog.Enabled = true;
                ultraButtonClearLog.Enabled = true;
                ultraButtonClearanceSetup.Enabled = true;
                ultraCheckEditorDebugMode.Enabled = true;
                ultraButtonDataDumper.Enabled = true;
                ultraButtonRefreshData.Enabled = true;
                ultraButtonDisconnectUser.Enabled = true;
                numericUpDownRefreshInterval.Enabled = true;
                ultraButtonUpdateRefreshTimeInterval.Enabled = true;
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
                ultraButtonOpenLog.Enabled = false;
                ultraButtonLoadLog.Enabled = false;
                ultraButtonClearLog.Enabled = false;
                ultraButtonClearanceSetup.Enabled = false;
                ultraCheckEditorDebugMode.Enabled = false;
                ultraButtonDataDumper.Enabled = false;
                ultraButtonRefreshData.Enabled = false;
                ultraButtonDisconnectUser.Enabled = false;
                numericUpDownRefreshInterval.Enabled = false;
                ultraButtonUpdateRefreshTimeInterval.Enabled = false;
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

        #region UI Events
        private async void ultraButtonOpenLog_Click(object sender, EventArgs e)
        {
            try
            {
                Byte[] buffer = await ExpnlServiceManager.ExpnlServiceManager.GetInstance.OpenLog();

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
                Byte[] buffer = await ExpnlServiceManager.ExpnlServiceManager.GetInstance.LoadLog();
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

        private async void ultraButtonClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxErrorMessages.Items.Clear();

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
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
                DialogResult result = MessageBox.Show("Do you wish to stop ExpnlService & UI?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (result == DialogResult.Yes)
                {
                    await ExpnlServiceManager.ExpnlServiceManager.GetInstance.StopService();
                    Application.Exit();
                }
            }
            catch (CommunicationException)
            {
                Application.Exit();
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

        private async void ultraButtonClearanceSetup_Click(object sender, EventArgs e)
        {
            try
            {
                if (_clearanceForm == null)
                {
                    _clearanceForm = new ClearanceTimeSetUpForm();
                    _clearanceForm.Disposed += new EventHandler(_clearanceForm_Disposed);
                    _clearanceForm.ClearanceUpdated += new EventHandler<EventArgs<Dictionary<int, DateTime>>>(_clearanceForm_ClearanceUpdated);

                    Dictionary<int, DateTime> dbclearanceTime = await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetDBClearanceTime();
                    if (dbclearanceTime != null)
                    {
                        await _clearanceForm.DrawFromDBClearanceTimes(dbclearanceTime);
                    }
                }
                _clearanceForm.Show();
                _clearanceForm.BringToFront();
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

        private async void _clearanceForm_ClearanceUpdated(object sender, EventArgs<Dictionary<int, DateTime>> e)
        {
            try
            {
                await ExpnlServiceManager.ExpnlServiceManager.GetInstance.UpdateClearance(e.Value);
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

        private void _clearanceForm_Disposed(object sender, EventArgs e)
        {
            try
            {
                _clearanceForm.ClearanceUpdated -= new EventHandler<EventArgs<Dictionary<int, DateTime>>>(_clearanceForm_ClearanceUpdated);
                _clearanceForm.Disposed -= new EventHandler(_clearanceForm_Disposed);
                _clearanceForm = null;
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

        private async void ultraButtonUpdateRefreshTimeInterval_Click(object sender, EventArgs e)
        {
            try
            {
                await ExpnlServiceManager.ExpnlServiceManager.GetInstance.UpdateRefreshTimeInterval(Convert.ToInt32(numericUpDownRefreshInterval.Value));
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

        private async void ultraButtonRefreshData_Click(object sender, EventArgs e)
        {
            try
            {
                await ExpnlServiceManager.ExpnlServiceManager.GetInstance.RefreshData();
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

        private async void ultraButtonDisconnectUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxClientsConnected.SelectedItem != null)
                {
                    await _expnlServiceClientHeartbeatManager.UnSubscribe(listBoxClientsConnected.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("Please Select a User", "ExPnL Calculator");
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

        private async void ultraButtonDataDumper_Click(object sender, EventArgs e)
        {
            try
            {
                ultraButtonDataDumper.Enabled = true;
                if (ultraButtonDataDumper.Text.Equals("Start Data Dumper"))
                {
                    await ExpnlServiceManager.ExpnlServiceManager.GetInstance.StartDataDumper();
                    ultraButtonDataDumper.Text = "Stop Data Dumper";
                }
                else
                {
                    DialogResult result = MessageBox.Show("Do you wish to stop Data Dumper ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                    if (result == DialogResult.Yes)
                    {
                        await ExpnlServiceManager.ExpnlServiceManager.GetInstance.StopDataDumper();
                        ultraButtonDataDumper.Text = "Start Data Dumper";
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

        private async void ultraCheckEditorDebugMode_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                await ExpnlServiceManager.ExpnlServiceManager.GetInstance.SetDebugModeStatus(ultraCheckEditorDebugMode.Checked);
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

        private async void copyToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (message.ToString() != string.Empty)
                    {
                        Clipboard.SetText(message.ToString(), TextDataFormat.Text);
                    }
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
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

        private async void ultraPopupControlContainerClientServicesStatus_Opened(object sender, EventArgs e)
        {
            try
            {
                listBoxClientServicesStatus.DisplayMember = "Name";
                listBoxClientServicesStatus.DataSource = null;

                // Showing loading image 
                pictureBoxLoading.Enabled = true;
                pictureBoxLoading.Image = Resources.loading;
                pictureBoxLoading.Visible = true;

                // Updating status
                listBoxClientServicesStatus.DataSource = await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetClientServicesStatus();
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
                            case Topics.Topic_ExpnlServiceLogsData:
                                listBoxErrorMessages.Items.Add((e.EventData[0]).ToString());
                                break;
                            case Topics.Topic_ExpnlServiceConnectedUserData:
                                object selectedItem = listBoxClientsConnected.SelectedItem;
                                listBoxClientsConnected.DataSource = null;
                                listBoxClientsConnected.DataSource = (List<string>)e.EventData[0];

                                if (selectedItem != null && listBoxClientsConnected.Items.Contains(selectedItem))
                                    listBoxClientsConnected.SelectedItem = selectedItem;
                                break;
                            case Topics.Topic_ExpnlServicePricingConnectionData:
                                DisplayPricingStatus((bool)e.EventData[0]);
                                break;
                            case Topics.Topic_ExpnlServiceTradeConnectionData:
                                DisplayTradeStatus((bool)e.EventData[0]);
                                break;
                            case Topics.Topic_ExpnlServiceLiveFeedConnectionData:
                                DisplayLiveFeedStatus((bool)e.EventData[0]);
                                break;
                            case Topics.Topic_ExpnlServiceCompressionData:
                                DisplayCompressionName(e.EventData[0].ToString());
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
            return "ExpnlServiceUI";
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
