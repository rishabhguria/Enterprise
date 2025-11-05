using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Prana.PricingService2UI.EsignalDM
{
    public partial class DataManager : Form, ILiveFeedCallback, IPublishing
    {
        /// <summary>
        /// The is user entitlements available
        /// </summary>
        bool _isUserEntitlmentsAvailable = false;

        /// <summary>
        /// The local dm advised symbols count
        /// </summary>
        private int _localDMAdvisedSymbolsCount = int.MinValue;

        /// <summary>
        /// The sub services form
        /// </summary>
        private SubscribedServices _subServicesForm = null;

        /// <summary>
        /// The dm property form
        /// </summary>
        private DMProperties _dmProp = null;

        private DuplexProxyBase<ISubscription> _subscriptionProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager" /> class.
        /// </summary>
        public DataManager()
        {
            try
            {
                InitializeComponent();
                Disposed += DataManager_Disposed;

                MakeProxy();

                Load += DataManager_Load;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        async void DataManager_Load(object sender, EventArgs e)
        {
            try
            {
                _dmProp = new DMProperties();

                List<string> credentials = await PricingService2Manager.PricingService2Manager.GetInstance.DataManagerSetup();

                if (credentials != null)
                {
                    _dmProp.UserName = credentials[0];
                    _dmProp.Password = credentials[1];
                    _dmProp.ServerAddress = credentials[2];

                    _dmProp.SetUIFields();
                }

                ToggleRequestServiceButtons();
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
                _subscriptionProxy = new DuplexProxyBase<ISubscription>("PricingSubscriptionEndpointAddress", this);
                _subscriptionProxy.Subscribe(Topics.Topic_DMSymbolDataResponse, null);
                _subscriptionProxy.Subscribe(Topics.Topic_DMSymbolLimitResponse, null);
                _subscriptionProxy.Subscribe(Topics.Topic_DMSymbolLimitServicesResponse, null);
                _subscriptionProxy.Subscribe(Topics.Topic_DMServicesResponse, null);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void UnSubscribeProxy()
        {
            try
            {
                if (_subscriptionProxy != null)
                {
                    _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_DMSymbolDataResponse);
                    _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_DMSymbolLimitResponse);
                    _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_DMSymbolLimitServicesResponse);
                    _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_DMServicesResponse);
                    _subscriptionProxy.Dispose();
                    _subscriptionProxy = null;
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                if (data != null)
                    this.BeginInvoke((MethodInvoker)delegate () { DisplaySymbolData(data); });
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            //throw new NotImplementedException();
        }

        public void LiveFeedDisConnected()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Displays the symbol data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void DisplaySymbolData(SymbolData data)
        {
            try
            {
                this.CusipLabel.Text = data.CusipNo;
                this.AveVolLabel.Text = data.AverageVolume20Day.ToString("#.####");
                this.XdateLabel.Text = data.XDividendDate.Equals(DateTimeConstants.MinValue) ? string.Empty : data.XDividendDate.ToShortDateString();
                this.AnnualDivLabel.Text = data.AnnualDividend.ToString("#.####");
                this.DivIntervalLabel.Text = data.DividendInterval.ToString("#");
                this.DividendLabel.Text = data.Dividend.ToString("#.####");
                this.YieldLabel.Text = data.DividendYield.ToString("#.####");
                this.AssetCategoryLabel.Text = data.CategoryCode.ToString();
                this.CurrencyLabel.Text = data.CurencyCode;
                this.ListExgLabel.Text = data.ListedExchange;
                this.AskExgLabel.Text = data.AskExchange;
                this.BidExgLabel.Text = data.BidExchange;
                this.VWAPLabel.Text = data.VWAP.ToString("#.####");
                this.UpTimeLabel.Text = data.UpdateTime.Equals(DateTimeConstants.MinValue) ? string.Empty : data.UpdateTime.ToLocalTime().ToString();
                this.LastTickLabel.Text = data.LastTick;
                this.ToVolLabel.Text = data.TotalVolume.ToString("#");
                this.AskSizeLabel.Text = data.AskSize.ToString("#");
                this.BidSizeLabel.Text = data.BidSize.ToString("#");
                this.OpenLabel.Text = data.Open.ToString("#.####");
                this.LowLabel.Text = data.Low.ToString("#.####");
                this.HighLabel.Text = data.High.ToString("#.####");
                this.ChangeLabel.Text = data.Change.ToString("#.####");
                this.AskLabel.Text = data.Ask.ToString("#.####");
                this.BidLabel.Text = data.Bid.ToString("#.####");
                this.LastLabel.Text = data.LastPrice.ToString("#.####");
                this.SharesLabel.Text = data.SharesOutstanding.ToString("#");
                this.OpIntLabel.Text = data.OpenInterest.ToString("#.####");
                this.StrikeLabel.Text = data.StrikePrice.ToString("#.####");
                this.MultiplierLabel.Text = data.Multiplier.ToString("#.####");
                this.CompNameLabel.Text = data.FullCompanyName;
                this.expirationDateLabel.Text = data.ExpirationDate.Equals(DateTimeConstants.MinValue) ? string.Empty : data.ExpirationDate.ToShortDateString();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the RequestButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void RequestButton_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAllFields();

                await PricingService2Manager.PricingService2Manager.GetInstance.RequestSymbol(SymbolTextBox.Text);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Clears all fields.
        /// </summary>
        private void ClearAllFields()
        {
            try
            {
                this.CusipLabel.Text = string.Empty;
                this.AveVolLabel.Text = string.Empty;
                this.XdateLabel.Text = string.Empty;
                this.AnnualDivLabel.Text = string.Empty;
                this.DivIntervalLabel.Text = string.Empty;
                this.DividendLabel.Text = string.Empty;
                this.YieldLabel.Text = string.Empty;
                this.AssetCategoryLabel.Text = string.Empty;
                this.CurrencyLabel.Text = string.Empty;
                this.ListExgLabel.Text = string.Empty;
                this.AskExgLabel.Text = string.Empty;
                this.BidExgLabel.Text = string.Empty;
                this.VWAPLabel.Text = string.Empty;
                this.UpTimeLabel.Text = string.Empty;
                this.LastTickLabel.Text = string.Empty;
                this.ToVolLabel.Text = string.Empty;
                this.AskSizeLabel.Text = string.Empty;
                this.BidSizeLabel.Text = string.Empty;
                this.OpenLabel.Text = string.Empty;
                this.LowLabel.Text = string.Empty;
                this.HighLabel.Text = string.Empty;
                this.ChangeLabel.Text = string.Empty;
                this.AskLabel.Text = string.Empty;
                this.BidLabel.Text = string.Empty;
                this.LastLabel.Text = string.Empty;
                this.SharesLabel.Text = string.Empty;
                this.OpIntLabel.Text = string.Empty;
                this.StrikeLabel.Text = string.Empty;
                this.CompNameLabel.Text = string.Empty;
                this.expirationDateLabel.Text = string.Empty;
                this.MultiplierLabel.Text = string.Empty;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the PropertiesButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PropertiesButton_Click(object sender, EventArgs e)
        {
            try
            {
                _dmProp.ShowDialog();

                ToggleRequestServiceButtons();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the CredentialsUpdated event of the dmProp control.
        /// </summary>
        public void ToggleRequestServiceButtons()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(_dmProp.ServerAddress) || _dmProp.ServerAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                {
                    this.RequestButton.Enabled = false;
                    this.ServicesButton.Enabled = false;
                }
                else
                {
                    this.RequestButton.Enabled = true;
                    this.ServicesButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ServicesButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void ServicesButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_subServicesForm == null)
                {
                    _subServicesForm = new SubscribedServices();
                }
                if (!_isUserEntitlmentsAvailable)
                {
                    await PricingService2Manager.PricingService2Manager.GetInstance.GetServices();
                }
                _subServicesForm.ShowDialog();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the RestartFeedButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void RestartFeedButton_Click(object sender, EventArgs e)
        {
            try
            {
                await PricingService2Manager.PricingService2Manager.GetInstance.RestartLiveFeed();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        LiveFeedViewer _lfViewer = null;
        /// <summary>
        /// Handles the Click event of the ViewLiveFeed control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ViewLiveFeed_Click(object sender, EventArgs e)
        {
            try
            {
                if (_lfViewer == null)
                {
                    _lfViewer = new LiveFeedViewer(_localDMAdvisedSymbolsCount);
                    _lfViewer.FormClosed += _lfViewer_FormClosed;
                    _lfViewer.Show();
                }
                else
                    _lfViewer.BringToFront();
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

        private void _lfViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _lfViewer.FormClosed -= _lfViewer_FormClosed;
                _lfViewer = null;
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
                            case Topics.Topic_DMSymbolDataResponse:
                                DisplaySymbolData((SymbolData)e.EventData[0]);
                                break;
                            case Topics.Topic_DMSymbolLimitResponse:
                                SymbolData symbolData = (SymbolData)e.EventData[0];
                                SymbolLimitLabel.Text = "User Level 1 Symbol Limit: " + symbolData.LastPrice;
                                _localDMAdvisedSymbolsCount = (int)symbolData.Open;
                                break;
                            case Topics.Topic_DMSymbolLimitServicesResponse:
                                SymbolLimitLabel.Text = "User Level 1 Symbol Limit: " + (e.EventData[0]).ToString();
                                break;
                            case Topics.Topic_DMServicesResponse:
                                if (_subServicesForm != null)
                                {
                                    _subServicesForm.updateSource((List<DMServiceData>)e.EventData[0]);
                                    _isUserEntitlmentsAvailable = true;
                                }
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
            return "DataManagerUI";
        }
        #endregion

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Handles the Disposed event of the DataManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void DataManager_Disposed(object sender, EventArgs e)
        {
            try
            {
                await PricingService2Manager.PricingService2Manager.GetInstance.DataManagerClose();

                UnSubscribeProxy();

                _subServicesForm = null;
                _dmProp = null;

                if (_lfViewer != null)
                {
                    _lfViewer.FormClosed -= _lfViewer_FormClosed;
                    _lfViewer.Dispose();
                    _lfViewer = null;
                }
                if (components != null)
                {
                    components.Dispose();
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
