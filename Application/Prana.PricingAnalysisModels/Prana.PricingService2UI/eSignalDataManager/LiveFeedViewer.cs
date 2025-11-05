using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.Global;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Prana.PricingService2UI.EsignalDM
{
    public partial class LiveFeedViewer : Form, IPublishing
    {
        private const string CONST_SYMCNT = "Symbol Count: ";
        private const string CONST_LOCALSYMCNT = "Local DM advised Symbol Count: ";
        private DuplexProxyBase<ISubscription> _subscriptionProxy;

        /// <summary>
        /// The source
        /// </summary>
        BindingList<SymbolData> source;

        /// <summary>
        /// The live feed dictionary
        /// </summary>
        private List<SymbolData> _liveFeedlist = new List<SymbolData>();

        private int _localSymCnt;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveFeedViewer" /> class.
        /// </summary>
        public LiveFeedViewer(int localSymCnt)
        {
            InitializeComponent();
            _localSymCnt = localSymCnt;

            MakeProxy();
            Load += LiveFeedViewer_Load;
        }

        async void LiveFeedViewer_Load(object sender, EventArgs e)
        {
            try
            {
                await PricingService2Manager.PricingService2Manager.GetInstance.InitializeLiveFeedViewerData();

                List<SymbolData> livefeedcopy = await PricingService2Manager.PricingService2Manager.GetInstance.GetLiveFeedDataList();

                if (livefeedcopy != null)
                {
                    _liveFeedlist = new List<SymbolData>(livefeedcopy);
                    source = new BindingList<SymbolData>(livefeedcopy);
                }
                else
                    source = new BindingList<SymbolData>(new List<SymbolData>());

                mygrid.DataSource = source;
                SetGridColumnLayout();
                LoadGridLayout();
                ListCountLabel.Text = CONST_SYMCNT + _liveFeedlist.Count;

                if (_localSymCnt >= 0)
                    EsignalSymbolCount.Text = CONST_LOCALSYMCNT + _localSymCnt;
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
                _subscriptionProxy.Subscribe(Topics.Topic_DMLiveFeedData, null);
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
                    _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_DMLiveFeedData);
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

        /// <summary>
        /// Loads the grid layout.
        /// </summary>
        private void LoadGridLayout()
        {
            try
            {
                string startPath = Application.StartupPath;
                string pricingPreferencesPath = startPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME;
                string multiTradePrefFile = pricingPreferencesPath + "\\LiveFeedViewer.xml";
                if (File.Exists(multiTradePrefFile))
                {
                    mygrid.DisplayLayout.LoadFromXml(multiTradePrefFile, PropertyCategories.All);
                }
            }
            catch (Exception e)
            {
                Exception ex = new Exception("Error while loading layout for the Grid. This error can generally be resolved by removing the preferences file OR by saving the layout again.", e);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the grid column layout.
        /// </summary>
        private void SetGridColumnLayout()
        {
            try
            {
                mygrid.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                mygrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                mygrid.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
                mygrid.DisplayLayout.Override.FilterUIType = FilterUIType.HeaderIcons;
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
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_subscriptionProxy"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "components")]
        protected async override void Dispose(bool disposing)
        {
            if (disposing)
            {
                await PricingService2Manager.PricingService2Manager.GetInstance.StopLiveFeedViewerData();
                UnSubscribeProxy();

                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Handles the Click event of the saveLayoutToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string startPath = Application.StartupPath;
                string pricingPreferencesPath = startPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME;
                if (!Directory.Exists(pricingPreferencesPath))
                {
                    Directory.CreateDirectory(pricingPreferencesPath);
                }

                string multiTradePrefFile = pricingPreferencesPath + "\\LiveFeedViewer.xml";
                mygrid.DisplayLayout.SaveAsXml(multiTradePrefFile, PropertyCategories.All);
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
        /// Handles the Click event of the removeFilterToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void removeFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                mygrid.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                mygrid.ActiveRowScrollRegion.Scroll(RowScrollAction.Top);
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
                            case Topics.Topic_DMLiveFeedData:
                                lock (_liveFeedlist)
                                {
                                    List<SymbolData> liveFeed = (List<SymbolData>)e.EventData[0];
                                    foreach (SymbolData liveData in liveFeed)
                                    {
                                        int index = _liveFeedlist.FindIndex(a => a.Symbol.Equals(liveData.Symbol));
                                        if (index >= 0)
                                        {
                                            source[index] = liveData;
                                            _liveFeedlist[index] = liveData;
                                        }
                                        else
                                        {
                                            source.Add(liveData);
                                            _liveFeedlist.Add(liveData);
                                        }
                                    }
                                    this.BeginInvoke((MethodInvoker)delegate ()
                                    {
                                        mygrid.Update();
                                        mygrid.Refresh();
                                        ListCountLabel.Text = CONST_SYMCNT + _liveFeedlist.Count;
                                    });
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
            return "LiveFeedViewerUI";
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