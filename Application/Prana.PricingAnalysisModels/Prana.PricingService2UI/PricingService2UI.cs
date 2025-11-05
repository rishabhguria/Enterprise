using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.CoreService.Interfaces;
using Prana.Global;
using Prana.Global.Properties;
using Prana.LogManager;
using Prana.PricingService2UI.APIDataViewer;
using Prana.PricingService2UI.EsignalDM;
using Prana.PricingService2UI.MarketDataMonitoring;
using Prana.PricingService2UI.SecondaryMarketDataMonitoring;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

namespace Prana.PricingService2UI
{
    public partial class PricingService2UI : Form, IPublishing
    {
        #region Variables
        private DuplexProxyBase<ISubscription> _subscriptionProxy;
        private MarketDataProvider? feedProvider = null;
        private FactSetContractType? factsetContractType = null;
        private SecondaryMarketDataProvider? secondaryFeedProvider = null;
        private DataManager dm = null;
        private MarketDataMonitoringUI _marketDataMonitoringUI = null;
        private BloombergProperties _bloombergPropertiesUI = null;
        LiveFeedDataViewer dataViwer = null;
        private ClientHeartbeatManager<IPricingService2> _pricingService2ClientHeartbeatManager;
        #endregion

        #region Constructor
        public PricingService2UI()
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();

            Text += " - v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        #region Private Methods
        private void PricingService2UI_Load(object sender, EventArgs e)
        {
            try
            {
                _pricingService2ClientHeartbeatManager = new ClientHeartbeatManager<IPricingService2>("PricingService2EndpointAddress");
                _pricingService2ClientHeartbeatManager.ConnectedEvent += PricingService2ClientHeartbeatManager_ConnectedEvent;
                _pricingService2ClientHeartbeatManager.DisconnectedEvent += PricingService2ClientHeartbeatManager_DisconnectedEvent;
                _pricingService2ClientHeartbeatManager.AnotherInstanceSubscribedEvent += PricingService2ClientHeartbeatManager_AnotherInstanceSubscribedEvent;
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

        private async void PricingService2ClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { PricingService2ClientHeartbeatManager_ConnectedEvent(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        ultraLabelPricingServiceConnectionStatus.Text = "PricingService2 Connected";

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

        private void PricingService2ClientHeartbeatManager_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { PricingService2ClientHeartbeatManager_DisconnectedEvent(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        System.Threading.Tasks.Task.Factory.StartNew(() => DisposeProxy()).ConfigureAwait(false);

                        ultraLabelPricingServiceConnectionStatus.Text = "PricingService2 Disconnected";
                        ultraLabelLivefeedStatus.Text = "Livefeed Disconnected";

                        listBoxClientsConnected.DataSource = null;
                        DisableControls();
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

        private void PricingService2ClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { PricingService2ClientHeartbeatManager_AnotherInstanceSubscribedEvent(sender, e); };
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
                    _subscriptionProxy = new DuplexProxyBase<ISubscription>("PricingSubscriptionEndpointAddress", this);
                    _subscriptionProxy.Subscribe(Topics.Topic_PricingService2LogsData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_PricingService2ConnectedUserData, null);
                    _subscriptionProxy.Subscribe(Topics.Topic_PricingService2LiveFeedConnectionData, null);
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

        private async System.Threading.Tasks.Task GetPreferences()
        {
            try
            {
                comboBoxPricingModels.DisplayMember = "DisplayText";
                comboBoxPricingModels.ValueMember = "Value";
                comboBoxPricingModels.DataSource = await PricingService2Manager.PricingService2Manager.GetInstance.GetPricingModels();

                WinDaleParams windaleParams = await PricingService2Manager.PricingService2Manager.GetInstance.GetWinDaleParams();
                comboBoxPricingModels.SelectedValue = windaleParams.PricingModel;
                numericUpDownBinomialSteps.Value = windaleParams.BinomialSteps;
                numericUpDownVolatilityIterations.Value = windaleParams.VolatilityIterations;
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

        private async System.Threading.Tasks.Task InitialDataRequestToContainerService()
        {
            try
            {
                if (await PricingService2Manager.PricingService2Manager.GetInstance.GetDebugModeStatus())
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

                await GetPreferences();

                if (await PricingService2Manager.PricingService2Manager.GetInstance.GetRiskLoggingStatus())
                {
                    this.ultraCheckEditorRiskLogging.CheckedChanged -= new System.EventHandler(this.ultraCheckEditorRiskLogging_CheckedChanged);
                    ultraCheckEditorRiskLogging.Checked = true;
                    this.ultraCheckEditorRiskLogging.CheckedChanged += new System.EventHandler(this.ultraCheckEditorRiskLogging_CheckedChanged);
                }
                else
                {
                    this.ultraCheckEditorRiskLogging.CheckedChanged -= new System.EventHandler(this.ultraCheckEditorRiskLogging_CheckedChanged);
                    ultraCheckEditorRiskLogging.Checked = false;
                    this.ultraCheckEditorRiskLogging.CheckedChanged += new System.EventHandler(this.ultraCheckEditorRiskLogging_CheckedChanged);
                }

                this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.Value = await PricingService2Manager.PricingService2Manager.GetInstance.GetRiskAPIEachCalDurationThreasholdToLogUpdateTime();

                feedProvider = await PricingService2Manager.PricingService2Manager.GetInstance.GetFeedProvider();
                if (feedProvider.Equals(MarketDataProvider.FactSet) || feedProvider.Equals(MarketDataProvider.ACTIV) || feedProvider.Equals(MarketDataProvider.SAPI))
                {
                    btnMarketDataMonitoring.Visible = true;
                    btnMarketDataMonitoring.Enabled = true;
                    ultraButtonDataManager.Visible = false;

                    if (feedProvider.Equals(MarketDataProvider.FactSet))
                    {
                        factsetContractType = await PricingService2Manager.PricingService2Manager.GetInstance.GetFactsetContractType();
                    }
                }
                else if (feedProvider.Equals(MarketDataProvider.None))
                    btnMarketDataMonitoring.Visible = ultraButtonDataManager.Visible = false;
                else
                {
                    ultraButtonDataManager.Visible = true;
                    btnMarketDataMonitoring.Visible = false;
                }

                secondaryFeedProvider = await PricingService2Manager.PricingService2Manager.GetInstance.GetSecondaryFeedProvider();
                if (!secondaryFeedProvider.Equals(SecondaryMarketDataProvider.None))
                {
                    btnSecondaryMarketData.Visible = true;
                    btnSecondaryMarketData.Enabled = true;
                }
                else
                {
                    btnSecondaryMarketData.Visible = false;
                    btnSecondaryMarketData.Enabled = false;
                }

                await PricingService2Manager.PricingService2Manager.GetInstance.RequestStartupData();
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

        private void dm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                dm.FormClosed -= dm_FormClosed;
                dm = null;
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
        /// Handles the FormClosed event of the dm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void marketDataMonitoringUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _marketDataMonitoringUI.FormClosed -= marketDataMonitoringUI_FormClosed;
                _marketDataMonitoringUI = null;
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
            //dm.Dispose();
        }

        private void DisplayLiveFeedStatus(bool isConnected)
        {
            try
            {
                if (isConnected)
                {
                    ultraLabelLivefeedStatus.Text = "Livefeed Connected";
                }
                else
                {
                    ultraLabelLivefeedStatus.Text = "Livefeed Disconnected";
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

        private void EnableControls()
        {
            try
            {
                comboBoxPricingModels.Enabled = true;
                numericUpDownBinomialSteps.Enabled = true;
                numericUpDownVolatilityIterations.Enabled = true;
                ultraButtonUpdateValues.Enabled = true;
                ultraButtonOpenLog.Enabled = true;
                ultraButtonLoadLog.Enabled = true;
                ultraButtonClearLog.Enabled = true;
                ultraButtonDataManager.Enabled = true;
                btnMarketDataMonitoring.Enabled = true;
                ultraButtonStopServiceAndUI.Enabled = true;
                ultraCheckEditorDebugMode.Enabled = true;
                ultraCheckEditorRiskLogging.Enabled = true;
                ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.Enabled = true;
                ultraButtonDisconnectUser.Enabled = true;
                btnSecondaryMarketData.Enabled = true;
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
                comboBoxPricingModels.Enabled = false;
                numericUpDownBinomialSteps.Enabled = false;
                numericUpDownVolatilityIterations.Enabled = false;
                ultraButtonUpdateValues.Enabled = false;
                ultraLabelMarketDataProvider.Visible = false;
                ultraLabelMarketDataProviderValue.Visible = false;
                ultraLabelLicenseType.Visible = false;
                ultraButtonOpenLog.Enabled = false;
                ultraButtonLoadLog.Enabled = false;
                ultraButtonClearLog.Enabled = false;
                ultraButtonDataManager.Enabled = false;
                btnMarketDataMonitoring.Enabled = false;
                ultraButtonStopServiceAndUI.Enabled = false;
                ultraCheckEditorDebugMode.Enabled = false;
                ultraCheckEditorRiskLogging.Enabled = false;
                ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.Enabled = false;
                ultraButtonDisconnectUser.Enabled = false;
                btnSecondaryMarketData.Enabled = false;
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
                Byte[] buffer = await PricingService2Manager.PricingService2Manager.GetInstance.OpenLog();

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
                Byte[] buffer = await PricingService2Manager.PricingService2Manager.GetInstance.LoadLog();
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
                DialogResult result = MessageBox.Show("Do you wish to stop the PricingService2 & UI?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (result == DialogResult.Yes)
                {
                    await PricingService2Manager.PricingService2Manager.GetInstance.StopService();
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

        private void ultraButtonSecondaryMarketDataDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (secondaryFeedProvider == SecondaryMarketDataProvider.BloombergDLWS)
                {
                    if (_bloombergPropertiesUI == null)
                    {
                        _bloombergPropertiesUI = new BloombergProperties();
                        _bloombergPropertiesUI.FormClosed += _bloombergPropertiesUI_FormClosed;
                        _bloombergPropertiesUI.ShowDialog();
                    }
                    else
                    {
                        _bloombergPropertiesUI.BringToFront();
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

        private void _bloombergPropertiesUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_bloombergPropertiesUI != null)
                {
                    _bloombergPropertiesUI.FormClosed -= _bloombergPropertiesUI_FormClosed;
                    _bloombergPropertiesUI = null;
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

        private void ultraButtonDataManager_Click(object sender, EventArgs e)
        {
            try
            {
                if (feedProvider == MarketDataProvider.Esignal)
                {
                    if (dm == null)
                    {
                        dm = new DataManager();
                        dm.FormClosed += dm_FormClosed;
                        dm.Show();
                    }
                    else
                        dm.BringToFront();
                }
                else if (feedProvider == MarketDataProvider.FactSet || feedProvider == MarketDataProvider.ACTIV || feedProvider == MarketDataProvider.SAPI)
                {
                    if (_marketDataMonitoringUI == null)
                    {
                        _marketDataMonitoringUI = new MarketDataMonitoringUI(feedProvider);
                        _marketDataMonitoringUI.FormClosed += marketDataMonitoringUI_FormClosed;
                        _marketDataMonitoringUI.Show();
                    }
                    else
                        _marketDataMonitoringUI.BringToFront();
                }
                else if (feedProvider == MarketDataProvider.API)
                {
                    if (dataViwer == null)
                    {
                        dataViwer = new LiveFeedDataViewer();
                        //dataViwer.PricingService = _pricingService;

                        dataViwer.FormClosed += dataViwer_FormClosed;
                        dataViwer.Show();
                    }
                    else
                    {
                        dataViwer.WindowState = FormWindowState.Maximized;
                        dataViwer.BringToFront();
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
        /// dataViwer_FormClosed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataViwer_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                dataViwer = null;
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
                await PricingService2Manager.PricingService2Manager.GetInstance.SetDebugModeStatus(ultraCheckEditorDebugMode.Checked);
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

        private async void ultraCheckEditorRiskLogging_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                await PricingService2Manager.PricingService2Manager.GetInstance.SetRiskLoggingStatus(ultraCheckEditorRiskLogging.Checked);
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
                    await _pricingService2ClientHeartbeatManager.UnSubscribe(listBoxClientsConnected.SelectedItem.ToString());
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

        private async void ultraButtonUpdateValues_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxPricingModels.SelectedValue == null)
                {
                    return;
                }

                WinDaleParams winDaleParams = new WinDaleParams();
                winDaleParams.BinomialSteps = Convert.ToInt32(numericUpDownBinomialSteps.Value);
                winDaleParams.PricingModel = Convert.ToInt32(comboBoxPricingModels.SelectedValue);
                winDaleParams.VolatilityIterations = Convert.ToInt32(numericUpDownVolatilityIterations.Value);

                await PricingService2Manager.PricingService2Manager.GetInstance.SaveWinDaleParams(winDaleParams);
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

        private async void ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                await PricingService2Manager.PricingService2Manager.GetInstance.RiskAPIEachCalDurationThreasholdToLogUpdateTime(Convert.ToInt32(ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.Value));
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
                listBoxClientServicesStatus.DataSource = await PricingService2Manager.PricingService2Manager.GetInstance.GetClientServicesStatus();
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
        public async void Publish(MessageData e, string topicName)
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
                            case Topics.Topic_PricingService2LogsData:
                                listBoxErrorMessages.Items.Add((e.EventData[0]).ToString());
                                break;
                            case Topics.Topic_PricingService2ConnectedUserData:
                                object selectedItem = listBoxClientsConnected.SelectedItem;
                                listBoxClientsConnected.DataSource = null;
                                listBoxClientsConnected.DataSource = (List<string>)e.EventData[0];

                                if (selectedItem != null && listBoxClientsConnected.Items.Contains(selectedItem))
                                    listBoxClientsConnected.SelectedItem = selectedItem;
                                break;
                            case Topics.Topic_PricingService2LiveFeedConnectionData:
                                DisplayLiveFeedStatus((bool)e.EventData[0]);
                                if (feedProvider != null)
                                {
                                    ultraLabelMarketDataProvider.Visible = true;
                                    ultraLabelMarketDataProviderValue.Visible = true;
                                    ultraLabelMarketDataProviderValue.Text = Enum.GetName(typeof(MarketDataProvider), feedProvider);
                                    if (factsetContractType != null)
                                    {
                                        ultraLabelLicenseType.Visible = true;
                                        ultraLabelLicenseType.Text = "License type: " + Enum.GetName(typeof(FactSetContractType), factsetContractType);
                                    }
                                    else
                                    {
                                        ultraLabelLicenseType.Visible = false;
                                    }
                                }
                                break;
                        }
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

        public string getReceiverUniqueName()
        {
            return "PricingService2UI";
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