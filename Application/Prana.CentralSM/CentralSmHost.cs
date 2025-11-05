using Infragistics.Win.UltraWinListView;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Prana.BlpDLWSAdapter.BusinessObject.Mappings;
using Prana.BusinessObjects;
using Prana.CentralSM;
using Prana.Global;
using Prana.Interfaces;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Prana.CentralSM
{
    public partial class CentralSmHost : Form
    {

        ServiceHost host;

        public CentralSmHost()
        {
            InitializeComponent();
            InformationReporter.GetInstance.InformationReceived += GetInstance_InformationReceived;
        }
        //delegate void SetTextCallback2(object sender, MsgEventArgs e);

        string WrapText(string text, double pixels, ref double heightOfLines, string fontFamily = "Calibri", float emSize = 11)
        {
            heightOfLines = 1;
            StringBuilder actualLine = new StringBuilder();
            double actualWidth = 0;
            try
            {
                string[] originalLines = text.Split(new string[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                //List<string> wrappedLines = new List<string>();
                double heigh = 11;
                foreach (var item in originalLines)
                {
                    FormattedText formatted = new FormattedText(item,
                        CultureInfo.CurrentCulture,
                        System.Windows.FlowDirection.LeftToRight,
                        new Typeface(fontFamily), emSize, System.Windows.Media.Brushes.Black);
                    heigh = formatted.Height;
                    actualWidth += formatted.Width;

                    if (actualWidth > pixels)
                    {
                        heightOfLines++;
                        actualLine.Append(Environment.NewLine);
                        actualLine.Append(item + " ");
                        actualWidth = formatted.Width;
                    }
                    else
                    {
                        actualLine.Append(item + " ");
                    }
                }
                heightOfLines = heightOfLines * heigh;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return actualLine.ToString();
        }

        void GetInstance_InformationReceived(object sender, EventArgs<string> e)
        {
            try
            {
                string message = e.Value;
                if (UIValidation.GetInstance().validate(listErrorLog))
                {
                    if (listErrorLog.InvokeRequired)
                    {
                        //SetTextCallback2 mi = new SetTextCallback2(GetInstance_InformationReceived);
                        //this.Invoke(mi, new object[] { message });

                        MethodInvoker del =
                           delegate
                           {
                               GetInstance_InformationReceived(sender, e);
                           };
                        this.Invoke(del);
                    }
                    else
                    {
                        message = message + " Time :=" + DateTime.Now.ToString();
                        double height = 1;
                        string itemText = WrapText(message, listErrorLog.Width - 85, ref height);
                        _keyNumber += 1;
                        //listErrorLog.Items.Add(_keyNumber.ToString(), itemText);
                        UltraListViewItem item = new UltraListViewItem((Object)itemText, null);
                        item.Key = _keyNumber.ToString();
                        listErrorLog.Items.Insert(0, item);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
        }

        object lockClientConnected = new object();

        ConcurrentDictionary<string, Tuple<ICentralSMSecurityCallback, DateTime>> _clientsConnected = new ConcurrentDictionary<string, Tuple<ICentralSMSecurityCallback, DateTime>>();

        CentralSMService _service;

        event StringHandler ClientDisconnectedEvent;

        private void btStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (host == null)
                {
                    _service = new CentralSMService();
                    _service.IsAliveEvent += service_ClientConnected;
                    ClientDisconnectedEvent += _service.Disconnect;
                    host = new ServiceHost(_service);
                }
                host.Open();
                btStart.Enabled = false;
                btStopServer.Enabled = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void service_ClientDisconnected(string clientName)
        {
            try
            {
                Tuple<ICentralSMSecurityCallback, DateTime> tempCalbackDateTimeTuple;
                if (_clientsConnected.TryRemove(clientName, out tempCalbackDateTimeTuple))
                {
                    UpdateUserList();
                    ClientDisconnectedEvent(this, new EventArgs<string>(clientName));
                    Logger.Write("Client disconnected " + clientName + " Time: " + tempCalbackDateTimeTuple.Item2.ToString(), ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
                    InformationReporter.GetInstance.Write("Client disconnected " + clientName);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        bool _isHeartbeatTimerRunning = false;

        System.Timers.Timer _heartbeatTimer;

        void StartHeartBeatTimer()
        {
            try
            {
                _isHeartbeatTimerRunning = true;
                _heartbeatTimer = new System.Timers.Timer(2000);
                _heartbeatTimer.AutoReset = true;
                _heartbeatTimer.Elapsed += _heartbeatTimer_Elapsed;
                _heartbeatTimer.Start();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void _heartbeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                ConcurrentDictionary<string, Tuple<ICentralSMSecurityCallback, DateTime>> clientsDisconnected = new ConcurrentDictionary<string, Tuple<ICentralSMSecurityCallback, DateTime>>(_clientsConnected.Where(x => DateTime.Now - x.Value.Item2 > new TimeSpan(0, 0, 2)));
                foreach (KeyValuePair<string, Tuple<ICentralSMSecurityCallback, DateTime>> kvp in clientsDisconnected)
                {
                    try
                    {
                        RetryHelper.Do(kvp.Value.Item1.IsAliveResp, new TimeSpan(0, 0, 1), 1);
                    }
                    catch (Exception)
                    {
                        service_ClientDisconnected(kvp.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void service_ClientConnected(object[] objects)
        {
            try
            {
                string clientName = objects[0].ToString();
                ICentralSMSecurityCallback securityCallback = objects[1] as ICentralSMSecurityCallback;
                if (!_clientsConnected.ContainsKey(clientName))
                {
                    Logger.Write("Client " + clientName + " connected at time " + DateTime.Now, ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
                    InformationReporter.GetInstance.Write("Client " + clientName + " connected.");
                    _clientsConnected.AddOrUpdate(clientName, new Tuple<ICentralSMSecurityCallback, DateTime>(securityCallback, DateTime.Now), (x, y) => new Tuple<ICentralSMSecurityCallback, DateTime>(securityCallback, DateTime.Now));
                    UpdateUserList();
                }
                _clientsConnected.AddOrUpdate(clientName, new Tuple<ICentralSMSecurityCallback, DateTime>(securityCallback, DateTime.Now), (x, y) => new Tuple<ICentralSMSecurityCallback, DateTime>(securityCallback, DateTime.Now));
                if (!_isHeartbeatTimerRunning)
                {
                    StartHeartBeatTimer();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btStopServer_Click(object sender, EventArgs e)
        {
            try
            {
                // modified by Suraj, 6, Aug 2014, Added confirmation message on closing Server.
                if (MessageBox.Show("Do you want to stop the server ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
                else
                {
                if (host != null)
                    host.Close();
                host = null;
                btStopServer.Enabled = false;
                btStart.Enabled = true;
            }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                //host.Abort(); 
            }
        }

        int _widthErrorList = 100;
        int _keyNumber = 1;
        private void CentralSmHost_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateUserList();
                listErrorLog.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
                listErrorLog.ViewSettingsList.ImageSize = Size.Empty;
                listErrorLog.SizeChanged += listErrorLog_SizeChanged;
                _widthErrorList = listErrorLog.Size.Width;
                btStopServer.Enabled = false;
                this.Text = "Central SM Server" + ", v" + ConfigurationHelper.Instance.GetAppSettingValueByKey("ApplicationVersion");   
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        void listErrorLog_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (_widthErrorList == listErrorLog.Size.Width)
                {
                    return;
                }
                _widthErrorList = listErrorLog.Size.Width;
                UltraListViewItemsCollection collect = DeepCopyHelper.Clone<UltraListViewItemsCollection>(listErrorLog.Items);
                UltraListView ulV = new UltraListView();
                foreach (UltraListViewItem it in listErrorLog.Items)
                {
                    ulV.Items.Add(it.Key, it.Text);
                }
                listErrorLog.Items.Clear();
                foreach (UltraListViewItem it in ulV.Items)
                {
                    double height = 1;
                    string itemText = WrapText(it.Text, listErrorLog.Width - 85, ref height);
                    listErrorLog.Items.Add(it.Key, itemText);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        delegate void SetTextCallback();

        private void UpdateUserList()
        {
            if (UIValidation.GetInstance().validate(listClientConnected))
            {
                if (listClientConnected.InvokeRequired)
                {
                    SetTextCallback mi = new SetTextCallback(UpdateUserList);
                    this.Invoke(mi, null);
                }
                else
                {
                    listClientConnected.DataSource = null;
                    listClientConnected.DataSource = _clientsConnected.Keys;
                }
            }
        }

        private void btReload_Click(object sender, EventArgs e)
        {
            try
            {
                BloombergSecurityTypeMapping.Instance.ReloadCache();
                HistoricalFields.Instance.ReloadCache();
                ExchangeCodeMapping.Instance.ReloadCache();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                MessageBox.Show("Error while reloading cache. Please contact administrator. Restart the server");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                listErrorLog.Items.Clear();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btOpenErrolLog_Click(object sender, EventArgs e)
        {
            try
            {
                string strFilePath = "Log\\trace_FlatFile Error Message Logging.log";

                if (File.Exists(strFilePath))
                {
                    Process.Start(strFilePath);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnOpenTrace_Click(object sender, EventArgs e)
        {
            try
            {
                string strFilePath = "Log\\trace_FlatFileTraceListener.log";

                if (File.Exists(strFilePath))
                {
                    Process.Start(strFilePath);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// handling on closing server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CentralSmHost_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            { 

                // modified by Suraj, 6, Aug 2014, Added confirmation message on closing Server.
                if (MessageBox.Show("Do you want to stop the server ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    e.Cancel = true;
                    this.Activate();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                if (host != null)
                    host.Close();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
