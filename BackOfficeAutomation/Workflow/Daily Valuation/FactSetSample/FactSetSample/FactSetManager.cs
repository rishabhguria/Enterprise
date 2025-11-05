using FactSet.Datafeed;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FactSetSample
{
    public partial class FactSetManager : Form
    {
        private RTConsumer _rtConsumer = new RTConsumer();
        public RTConsumer RTConsumer
        {
            get { return _rtConsumer; }
            set { _rtConsumer = value; }
        }

        private Dictionary<string, RTConsumer.RTSubscription> _dictSubscriptions = new Dictionary<string, RTConsumer.RTSubscription>();

        private string _clientConnectionUsername = ConfigurationManager.AppSettings["ClientConnectionUsername"].ToString().Trim();
        private string _clientConnectionPassword = ConfigurationManager.AppSettings["ClientConnectionPassword"].ToString().Trim();
        private string _clientConnectionHost = ConfigurationManager.AppSettings["ClientConnectionHost"].ToString().Trim();
        private string _clientConnectionPort = ConfigurationManager.AppSettings["ClientConnectionPort"].ToString().Trim();

        private string _configFactSetDataService = ConfigurationManager.AppSettings["FactSetDataService"].ToString().Trim();

        #region Constructor
        public FactSetManager()
        {
            InitializeComponent();

            _rtConsumer.ConnectCompleted += new EventHandler<ConnectCompletedEventArgs>(ClientConnectionHandler);
            _rtConsumer.DispatchCompleted += ClientDispatchCompleted;

            buttonSnapshot.Focus();
            buttonConnect_Click(null, null);
        }
        #endregion

        #region Private Methods
        private void ClientConnectionHandler(object sender, ConnectCompletedEventArgs e)
        {
            try
            {
                _rtConsumer = sender as RTConsumer;

                if (e.Cancelled)
                {
                    listBoxLog.Items.Add(string.Format("FactSet connection was cancelled: {0}"));

                    ConnectionStatus(false);
                }
                else if (e.Error != null)
                {
                    listBoxLog.Items.Add(string.Format("Error occurred while establishing a connection with FactSet: {0}", e.Error.Message));

                    ConnectionStatus(false);
                }
                else if (e.CtrlType == FactSet.Datafeed.ControlType.DISCONNECTED)
                {
                    listBoxLog.Items.Add(string.Format("Error occurred while establishing a connection with FactSet: {0}, {1}, {2}", e.CtrlType, e.CntrlMsg.Error, e.CntrlMsg.ErrorDescription));

                    ConnectionStatus(false);
                }
                else
                {
                    if (e.IsConnected)
                    {
                        listBoxLog.Items.Add(string.Format("Connected to FactSet: {0}, {1}", _rtConsumer.ConnectedToHost, e.CtrlType));

                        StringBuilder sb = new StringBuilder();
                        foreach (var svc in _rtConsumer.Services)
                        {
                            sb.Append(svc + "\t");
                        }
                        listBoxLog.Items.Add(string.Format("Connected Services: {0}", sb));

                        ConnectionStatus(true);
                    }
                    else
                    {
                        listBoxLog.Items.Add(string.Format("Disconnected from FactSet while making connection: {0}, {1}", e.CtrlType, e.CntrlMsg.ErrorDescription));

                        ConnectionStatus(false);
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxException.Items.Add(ex.StackTrace);
            }
        }

        private void ClientDispatchCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBoxLog.Items.Add("DispatchCompleted Error: " + e.Error.Message);
                Disconnect();
            }
            else
            {
                listBoxLog.Items.Add("Dispatch completed");
            }
        }

        private void SecurityResponseHandler(RTConsumer.RTSubscription rtSubscription, RTMessage rtMessage)
        {
            try
            {
                if (rtMessage.IsClosed || (rtMessage.IsComplete && rtSubscription.IsSnapshot))
                {
                    _rtConsumer.Cancel(rtSubscription);
                }

                if (rtMessage.IsError)
                {
                    listBoxLog.Items.Add(string.Format("Error: {0}, {1}, {2}", rtMessage.Key, rtMessage.Error, rtMessage.ErrorDescription));
                }
                else
                {
                    FillSymbolDataFromRTMessage(rtMessage, rtSubscription.IsSnapshot);
                }
            }
            catch (Exception ex)
            {
                listBoxException.Items.Add(ex.StackTrace);
            }
        }

        private void ConnectionStatus(bool isConnected)
        {
            _isConnected = isConnected;
            if (isConnected)
            {
                labelConnectionStatus.Text = "CONNECTED";

                textBoxFactSetSymbol.Enabled = true;
                buttonSnapshot.Enabled = true;
                buttonSubscription.Enabled = true;
                buttonCancel.Enabled = true;
                checkBoxWriteInFile.Enabled = true;

                buttonConnect.Text = "Disconnect";
            }
            else
            {
                labelConnectionStatus.Text = "DISCONNECTED";

                buttonConnect.Text = "Connect";
            }

            buttonConnect.Enabled = true;
        }

        private void FillSymbolDataFromRTMessage(RTMessage rtMessage, bool isSnapshotData)
        {
            try
            {
                listBoxLog.Items.Add(string.Format("FactSet security response received: {0}, IsSnapshotData: {1}", rtMessage.Key, isSnapshotData));

                listBoxResponse.Items.Add(rtMessage.ToString());

                if (checkBoxWriteInFile.Checked)
                {
                    using (StreamWriter writer = File.AppendText(Application.StartupPath + "\\Response.txt"))
                    {
                        writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss fff"));
                        writer.WriteLine("\n" + rtMessage.ToString());
                        writer.WriteLine("-------------------------------------------------------------------------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(string.Format("Error in parsing FactSet security response: {0}", rtMessage));

                listBoxException.Items.Add(ex.StackTrace);
            }
        }

        private void DisplayHScroll()
        {
            // Make sure no items are displayed partially.
            listBoxResponse.IntegralHeight = true;

            // Add items that are wide to the ListBox.
            for (int x = 0; x < 10; x++)
            {
                listBoxResponse.Items.Add("Item  " + x.ToString() + " is a very large value that requires scroll bars");
            }

            // Display a horizontal scroll bar.
            listBoxResponse.HorizontalScrollbar = true;

            // Create a Graphics object to use when determining the size of the largest item in the ListBox.
            Graphics g = listBoxResponse.CreateGraphics();

            // Determine the size for HorizontalExtent using the MeasureString method using the last item in the list.
            int hzSize = (int)g.MeasureString(listBoxResponse.Items[listBoxResponse.Items.Count - 1].ToString(), listBoxResponse.Font).Width;
            // Set the HorizontalExtent property.
            listBoxResponse.HorizontalExtent = hzSize;
        }
        #endregion

        #region ILiveFeedAdapter Methods
        public void Connect()
        {
            try
            {
                Disconnect();

                #region Client's Server-level Connection
                if (!string.IsNullOrWhiteSpace(_clientConnectionUsername) && !string.IsNullOrWhiteSpace(_clientConnectionPassword) && !string.IsNullOrWhiteSpace(_clientConnectionHost))
                {
                    _rtConsumer.ConnInfo = !string.IsNullOrEmpty(_clientConnectionPort) ?
                         RTConsumer.MakeConnInfo(_clientConnectionUsername, _clientConnectionPassword, _clientConnectionHost, int.Parse(_clientConnectionPort)) :
                         RTConsumer.MakeConnInfo(_clientConnectionUsername, _clientConnectionPassword, _clientConnectionHost);

                    listBoxLog.Items.Add(string.Format("Connecting to FactSet on {0}:XXXXXX@{1}", _clientConnectionUsername, _clientConnectionHost));

                    _rtConsumer.OptionsGreeksEnabled = false;
                    _rtConsumer.SetSendUnchangedFields(false);
                    _rtConsumer.ConnectAsync();
                }
                else
                {
                    listBoxLog.Items.Add("Not valid credentials saved in configuration");
                }
                #endregion
            }
            catch (Exception ex)
            {
                listBoxException.Items.Add(ex.StackTrace);
            }
        }

        public void Disconnect()
        {
            if (!string.IsNullOrWhiteSpace(_rtConsumer.ConnectedToHost) && _rtConsumer.IsConnected)
            {
                lock (_dictSubscriptions)
                {
                    //foreach (RTConsumer.RTSubscription rtSubscription in _dictSubscriptions.Values)
                    //{
                    //    if (rtSubscription != null && rtSubscription.IsSubscribed)
                    //    {
                    //        _rtConsumer.Cancel(rtSubscription);
                    //    }
                    //}
                    _dictSubscriptions.Clear();
                }

                _rtConsumer.Disconnect();
                ConnectionStatus(false);

                listBoxLog.Items.Add(string.Format("Disconnected from FactSet on {0}", _rtConsumer.ConnectedToHost));
            }
        }

        public void GetContinuousData(string symbol)
        {
            try
            {
                if (symbol != null)
                {
                    listBoxLog.Items.Add(string.Format("ContinuousData Request: {0}", symbol));

                    lock (_dictSubscriptions)
                    {
                        if (!_dictSubscriptions.ContainsKey(symbol))
                        {
                            RTRequest req = new RTRequest(_configFactSetDataService, symbol);
                            _dictSubscriptions.Add(symbol, _rtConsumer.MakeRequest(req, SecurityResponseHandler));
                        }
                        else
                        {
                            listBoxLog.Items.Add(string.Format("ContinuousData duplicate request received: {0}", symbol));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxException.Items.Add(ex.StackTrace);
            }
        }

        public void GetSnapShotData(string symbol)
        {
            try
            {
                if (symbol != null)
                {
                    listBoxLog.Items.Add(string.Format("SnapShotData Request: {0}", symbol));

                    RTRequest req = new RTRequest(_configFactSetDataService, symbol, true);
                    _rtConsumer.MakeRequest(req, SecurityResponseHandler);
                }
            }
            catch (Exception ex)
            {
                listBoxException.Items.Add(ex.StackTrace);
            }
        }

        public void DeleteSymbol(string symbol)
        {
            try
            {
                lock (_dictSubscriptions)
                {
                    if (_dictSubscriptions.ContainsKey(symbol))
                    {
                        if (!string.IsNullOrWhiteSpace(_rtConsumer.ConnectedToHost) && _rtConsumer.IsConnected)
                        {
                            _rtConsumer.Cancel(_dictSubscriptions[symbol]);

                            _dictSubscriptions[symbol] = null;
                            _dictSubscriptions.Remove(symbol);

                            listBoxLog.Items.Add(string.Format("Symbol: {0} subscription stopped", symbol));
                        }
                        else
                        {
                            listBoxLog.Items.Add(string.Format("No active connection with Factset. So unable to delete Symbol: {0}", symbol));
                        }
                    }
                    else
                    {
                        listBoxLog.Items.Add(string.Format("Symbol: {0} was not subscribed, so unable to delete it", symbol));
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxException.Items.Add(ex.StackTrace);
            }
        }
        #endregion 

        #region UI Events
        bool _isConnected = false;
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            buttonConnect.Enabled = false;

            if (_isConnected)
            {
                Disconnect();

                textBoxFactSetSymbol.Enabled = false;
                buttonSnapshot.Enabled = false;
                buttonSubscription.Enabled = false;
                buttonCancel.Enabled = false;
                checkBoxWriteInFile.Enabled = false;
            }
            else
            {
                Connect();
            }
        }

        private void textBoxFactSetSymbol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSnapshot_Click(this, new EventArgs());
            }
        }

        private void buttonSnapshot_Click(object sender, EventArgs e)
        {
            if (_isConnected && !string.IsNullOrWhiteSpace(textBoxFactSetSymbol.Text.ToUpper()))
                GetSnapShotData(textBoxFactSetSymbol.Text.ToUpper());
        }

        private void buttonSubscription_Click(object sender, EventArgs e)
        {
            if (_isConnected && !string.IsNullOrWhiteSpace(textBoxFactSetSymbol.Text.ToUpper()))
                GetContinuousData(textBoxFactSetSymbol.Text.ToUpper());
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (_isConnected && !string.IsNullOrWhiteSpace(textBoxFactSetSymbol.Text.ToUpper()))
                DeleteSymbol(textBoxFactSetSymbol.Text.ToUpper());
        }

        private void responseCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listBoxResponse.SelectedItem.ToString());
        }

        private void responseClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxResponse.Items.Clear();
        }

        private void logCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listBoxLog.SelectedItem.ToString());
        }

        private void logClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxLog.Items.Clear();
        }

        private void exceptionCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listBoxException.SelectedItem.ToString());
        }

        private void exceptionClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxException.Items.Clear();
        }

        private void listBoxResponse_MouseDown(object sender, MouseEventArgs e)
        {
            listBoxResponse.SelectedIndex = listBoxResponse.IndexFromPoint(e.X, e.Y);
        }

        private void listBoxLog_MouseDown(object sender, MouseEventArgs e)
        {
            listBoxLog.SelectedIndex = listBoxLog.IndexFromPoint(e.X, e.Y);
        }

        private void listBoxException_MouseDown(object sender, MouseEventArgs e)
        {
            listBoxException.SelectedIndex = listBoxException.IndexFromPoint(e.X, e.Y);
        }
        #endregion
    }
}
