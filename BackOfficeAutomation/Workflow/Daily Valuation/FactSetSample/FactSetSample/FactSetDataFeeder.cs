using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactSet.Datafeed;

namespace FactSetSample
{
    public class FactSetDataFeeder
    {
        public static string midcloseprice = "";
        public static string pricevalue = "MID_CLOSE";
        private static RTConsumer _rtConsumer = new RTConsumer();

        public static RTConsumer RTConsumer
        {
            get { return _rtConsumer; }
            set { _rtConsumer = value; }
        }
        private static Dictionary<string, RTConsumer.RTSubscription> _dictSubscriptions = new Dictionary<string, RTConsumer.RTSubscription>();
        private static string _clientConnectionUsername = "NIRVANA-1094667";
        private static string _clientConnectionPassword = "JhscRoPlf2RXS0zb";
        private static string _clientConnectionHost = "api-stage.df.factset.com";
        private static string _clientConnectionPort = "";

        private static string _configFactSetDataService = "FDS1";
        public FactSetDataFeeder()
        {
            //InitializeComponent();

            _rtConsumer.ConnectCompleted += new EventHandler<ConnectCompletedEventArgs>(ClientConnectionHandler);
            _rtConsumer.DispatchCompleted += ClientDispatchCompleted;

            //buttonSnapshot.Focus();
           // buttonConnect_Click(null, null);
        }
      

        public static void Connect()
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

                    Console.WriteLine(string.Format("Connecting to FactSet on {0}:XXXXXX@{1}", _clientConnectionUsername, _clientConnectionHost));

                    _rtConsumer.OptionsGreeksEnabled = false;
                    _rtConsumer.SetSendUnchangedFields(false);
                    _rtConsumer.ConnectAsync();
                }
                else
                {
                    Console.WriteLine("Not valid credentials saved in configuration");
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        public static void Disconnect()
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
                //ConnectionStatus(false);

                Console.WriteLine(string.Format("Disconnected from FactSet on {0}", _rtConsumer.ConnectedToHost));
            }
        }

        public static void GetSnapShotData(string symbol)
        {
            try
            {
                if (symbol != null)
                {
                    Console.WriteLine(string.Format("SnapShotData Request: {0}", symbol));

                    RTRequest req = new RTRequest(_configFactSetDataService, symbol, true);
                    _rtConsumer.MakeRequest(req, SecurityResponseHandler);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        private static void SecurityResponseHandler(RTConsumer.RTSubscription rtSubscription, RTMessage rtMessage)
        {
            try
            {
                midcloseprice = "";
                if (rtMessage.IsClosed || (rtMessage.IsComplete && rtSubscription.IsSnapshot))
                {
                    _rtConsumer.Cancel(rtSubscription);
                }

                if (rtMessage.IsError)
                {
                    Console.WriteLine(string.Format("Error: {0}, {1}, {2}", rtMessage.Key, rtMessage.Error, rtMessage.ErrorDescription));
                }
                else
                {

                    Console.WriteLine("this is message " + rtMessage);
                     FidField MidCloseField = rtMessage.GetField(RTFieldId.MID_CLOSE);
                   
                    if (MidCloseField != null)
                    {
                        string MidCloseValue = MidCloseField.Value;
                        midcloseprice = MidCloseValue;
                        Console.WriteLine("Mid_CLOSE Value: " + MidCloseValue);
                    }

                    
                    Console.WriteLine("this is snapshot " + rtSubscription.IsSnapshot);
                    Console.WriteLine("this is error " + rtMessage.IsError);
                    //FillSymbolDataFromRTMessage(rtMessage, rtSubscription.IsSnapshot);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        private static void ClientConnectionHandler(object sender, ConnectCompletedEventArgs e)
        {
            try
            {
                _rtConsumer = sender as RTConsumer;

                if (e.Cancelled)
                {
                    Console.WriteLine(string.Format("FactSet connection was cancelled: {0}"));

                    //ConnectionStatus(false);
                }
                else if (e.Error != null)
                {
                    Console.WriteLine(string.Format("Error occurred while establishing a connection with FactSet: {0}", e.Error.Message));

                    //ConnectionStatus(false);
                }
                else if (e.CtrlType == FactSet.Datafeed.ControlType.DISCONNECTED)
                {
                    Console.WriteLine(string.Format("Error occurred while establishing a connection with FactSet: {0}, {1}, {2}", e.CtrlType, e.CntrlMsg.Error, e.CntrlMsg.ErrorDescription));

                    //ConnectionStatus(false);
                }
                else
                {
                    if (e.IsConnected)
                    {
                        Console.WriteLine(string.Format("Connected to FactSet: {0}, {1}", _rtConsumer.ConnectedToHost, e.CtrlType));

                        System.Text.StringBuilder sb = new StringBuilder();
                        foreach (var svc in _rtConsumer.Services)
                        {
                            sb.Append(svc + "\t");
                        }
                        Console.WriteLine(string.Format("Connected Services: {0}", sb));

                        // ConnectionStatus(true);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Disconnected from FactSet while making connection: {0}, {1}", e.CtrlType, e.CntrlMsg.ErrorDescription));

                        //ConnectionStatus(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void ClientDispatchCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine("DispatchCompleted Error: " + e.Error.Message);
                Disconnect();
            }
            else
            {
                Console.WriteLine("Dispatch completed");
            }
        }


    }
}
