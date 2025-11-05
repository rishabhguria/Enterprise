using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Threading;
namespace Prana.SocketCommunication
{
    public class SocketConnection : IDisposable
    {
        private readonly object lockSocket = new object();
        private System.Timers.Timer _heartBeatTimer = new System.Timers.Timer();
        private System.Timers.Timer _timeOutTimer = new System.Timers.Timer();
        private Queue TransmitQueue = new Queue();
        ConnectionProperties _connProperties = null;
        private PranaInternalConstants.ConnectionStatus _connectionStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
        private readonly object lockTransmitQueue = new object();
        public event MessageReceivedDelegate MessageReceived;
        public event ExceptionDelegate SocketException;
        private const int MESSAGE_SIZE = 8;
        private Socket _socket;
        private string _userID, _userName;
        System.Text.Encoding _utf8 = System.Text.Encoding.UTF8;
        private int _heartBeatInterval = 120;
        private int _timeoutInterval = 120;
        private System.DateTime _lastMessageReceiveTime = System.DateTime.Now.ToUniversalTime();
        private ManualResetEvent StopEvent = new ManualResetEvent(false);
        private AutoResetEvent DataReady = new AutoResetEvent(false);
        public event EventHandler Disconnected;
        public event EventHandler Connected;
        IPEndPoint _ipHost = null;
        string _socketType = string.Empty;
        readonly object DisConnectLock = new object();
        private HandlerType _handlerType = HandlerType.NotSet;
        private ServiceEndPoint _serviceEndPoint;
        #region Properties

        public string UserName
        {

            get
            {
                return this._userName;
            }
            set
            {
                _userName = value;
            }
        }
        public HandlerType HandlerType
        {

            get
            {
                return this._handlerType;
            }
            set
            {
                _handlerType = value;
            }
        }

        public string UserID
        {

            get
            {
                return this._userID;
            }
            set
            {
                _userID = value;
            }
        }

        public PranaInternalConstants.ConnectionStatus ConnectionStatus
        {
            get { return _connectionStatus; }
        }

        public ConnectionProperties ConnProperties
        {
            get { return _connProperties; }
        }



        #endregion

        #region private methods

        #region beating related methods

        void _heartBeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SendMessage(new QueueMessage(AdminMessageHandler.CreateHeartBeatForUser()).ToString());
        }
        void _timeOutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan t1 = System.DateTime.Now.ToUniversalTime().Subtract(_lastMessageReceiveTime);
            TimeSpan t2 = new TimeSpan((long)_timeoutInterval * 10000);

            if (TimeSpan.Compare(t1, t2) == 1) // if allowed is greater than real time diff 
            {
                RaiseSocketException(new Exception("Disconnected due to Time out"), "Time out");
                _timeOutTimer.Stop();
                _heartBeatTimer.Stop();
            }
        }
        private void StopTimersAndUnwireEvents()
        {
            _heartBeatTimer.Stop();
            _heartBeatTimer.Elapsed -= new System.Timers.ElapsedEventHandler(_heartBeatTimer_Elapsed);
            _timeOutTimer.Stop();
            _timeOutTimer.Elapsed -= new System.Timers.ElapsedEventHandler(_timeOutTimer_Elapsed);
        }
        #endregion

        #region received Message formation

        void SendToManager(string msg)
        {
            _lastMessageReceiveTime = System.DateTime.Now.ToUniversalTime();
            //Removed @@ from the back of the string
            //msg = msg.Substring(0, msg.Length - 2);
            QueueMessage qMsg = new QueueMessage(msg);
            qMsg.ServiceEndPoint = _serviceEndPoint;

            if (MessageReceived != null)
                MessageReceived(this, new EventArgs<QueueMessage>(qMsg));

            msg = null;
        }

        private void GetLengthOfData(IAsyncResult ar)
        {
            try
            {
                StateObject sLengthOfData = (StateObject)ar.AsyncState;
                int read = 0;
                if (_socket == null)
                {
                    return;
                }
                else
                {
                    read = _socket.EndReceive(ar);
                    if (read > 0)
                    {
                        if (read == sLengthOfData.READ_LENGTH_OF_DATA)
                        {
                            int length = int.Parse(sLengthOfData.ReadBytes());
                            StateObject soMessage = new StateObject(length);
                            AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetStreamMsg);
                            try
                            {
                                _socket.BeginReceive(soMessage.Buffer, 0, soMessage.READ_LENGTH_OF_DATA, SocketFlags.None, GetStreamMsgCallback, soMessage);
                            }
                            catch (Exception ex)
                            {
                                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSOCKET);
                                if (rethrow)
                                {
                                    throw;
                                }
                                // Disconnect(null);
                            }
                        }
                        else
                        {
                            sLengthOfData.SetStateObjectForAppend(read);
                            AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetLengthOfData);
                            _socket.BeginReceive(sLengthOfData.Buffer, 0, sLengthOfData.READ_LENGTH_OF_DATA, SocketFlags.None, GetStreamMsgCallback, sLengthOfData);
                        }
                    }
                    else
                    {
                        RaiseSocketException(new Exception("Socket data is not available"), string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseSocketException(ex, string.Empty);
            }
        }

        public void GetStreamMsg(IAsyncResult ar)
        {
            try
            {
                StateObject sObject = (StateObject)ar.AsyncState;
                int read = _socket.EndReceive(ar);

                if (read > 0)
                {
                    if (read < sObject.READ_LENGTH_OF_DATA)
                    {
                        sObject.SetStateObjectForAppend(read);
                        AsyncCallback AppendStringCallback = new AsyncCallback(AppendString);
                        _socket.BeginReceive(sObject.Buffer, 0, sObject.READ_LENGTH_OF_DATA, SocketFlags.None, AppendStringCallback, sObject);
                    }
                    else
                    {
                        //sObject.Buffer = CompressionHelper.UnZip(sObject.Buffer);
                        string message = sObject.ReadBytes();
                        SendToManager(message);
                        WaitForMessages();
                    }
                }
                else
                {
                    RaiseSocketException(new Exception("Socket data is not available"), string.Empty);
                }

            }
            catch (Exception ex)
            {
                RaiseSocketException(ex, string.Empty);
            }
        }

        private void WaitForMessages()
        {
            try
            {
                StateObject sObject = new StateObject(MESSAGE_SIZE);
                AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetLengthOfData);
                if (_socket != null)
                    _socket.BeginReceive(sObject.Buffer, 0, sObject.READ_LENGTH_OF_DATA, SocketFlags.None, GetStreamMsgCallback, sObject);
            }
            catch (Exception ex)
            {
                RaiseSocketException(ex, string.Empty);
            }
        }

        private void AppendString(IAsyncResult ar)
        {
            try
            {
                StateObject sObject = (StateObject)ar.AsyncState;
                int read = _socket.EndReceive(ar);
                if (read > 0)
                {
                    if (read == sObject.READ_LENGTH_OF_DATA)
                    {
                        SendToManager(sObject.ReadBytes());
                        WaitForMessages();
                    }

                    else
                    {
                        sObject.SetStateObjectForAppend(read);
                        AsyncCallback AppendStringCallback = new AsyncCallback(AppendString);
                        _socket.BeginReceive(sObject.Buffer, 0, sObject.READ_LENGTH_OF_DATA, SocketFlags.None, AppendStringCallback, sObject);
                    }
                }
                else
                {
                    RaiseSocketException(new Exception("Socket data is not available"), string.Empty);
                }
            }
            catch (Exception ex)
            {
                RaiseSocketException(ex, string.Empty);
            }
        }

        #endregion

        private void SendThreadEntryPoint(object state)
        {
            try
            {
                Queue workQueue = new Queue();
                // loop...
                while (true)
                {
                    WaitHandle[] handles = new WaitHandle[2];
                    handles[0] = StopEvent;
                    handles[1] = DataReady;

                    if (WaitHandle.WaitAny(handles) == 0)
                    {
                        break;
                    }
                    else if (_socket != null && _socket.Connected)
                    {
                        lock (lockTransmitQueue)
                        {
                            workQueue.Clear();
                            foreach (string message in TransmitQueue)
                            {
                                workQueue.Enqueue(message);
                            }
                            TransmitQueue.Clear();
                        }
                        // loop the outbound messages...
                        foreach (string message in workQueue)
                        {
                            string msg = message;
                            //msg = message + "@";
                            int msgLength = _utf8.GetBytes(msg).Length;
                            string bBufLength = msgLength.ToString().PadLeft(MESSAGE_SIZE, '0');
                            Byte[] bBuf = _utf8.GetBytes(bBufLength + msg);
                            //StateObject so2 = new StateObject(bBuf.Length);

                            // send it...
                            //_socket.Send(CompressionHelper.Zip(bBuf));
                            //System.IAsyncResult iar;
                            _socket.Send(bBuf);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseSocketException(ex, ex.Message);
            }
        }
        private void DisConnectSocket(Exception ex)
        {
            try
            {
                //lock (_lockSocket)
                //{
                lock (DisConnectLock)
                {
                    if (_socket != null)
                    {
                        StopTimersAndUnwireEvents();
                        _connectionStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                        if (Disconnected != null)
                        {
                            Disconnected(this, null);
                        }
                        _socket.Shutdown(SocketShutdown.Both);
                        _socket.Close();
                        _socket = null;
                        StopEvent.Set();
                        if (ex.Message != string.Empty)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSOCKET);
                        }
                        //RetryConnection();
                    }
                }
            }
            // no need to throw exception if it is proxy exception
            catch (ProxyException)
            {

            }
            // no need to throw exception if it is CommunicationException raised when Closeing the channelObject becouse it's alredy aborted
            catch (CommunicationException)
            {

            }
            catch (Exception ex1)
            {
                Logger.HandleException(ex1, LoggingConstants.POLICY_LOGANDSOCKET);
            }
        }

        private void RaiseSocketException(Exception ex, string reason)
        {
            if (_socket != null)
            {
                //log the exception
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSOCKET);

                if (reason != string.Empty)
                    reason = " Reason " + reason;
                string ip = string.Empty;
                int port = 0;
                try
                {
                    IPEndPoint ipendpoint = (IPEndPoint)_socket.RemoteEndPoint;
                    port = ipendpoint.Port;
                    ip = ipendpoint.Address.ToString();
                }
                catch (Exception)
                {
                    Logger.HandleException(new Exception("Socket Is Null"), LoggingConstants.POLICY_LOGANDSOCKET);
                }

                if (_socketType == "Server")
                    SocketException(this, new EventArgs<Exception>(new Exception(_userName + "  Requested to disconnect : IP " + ip + " Port " + port.ToString() + reason)));
                else
                    SocketException(this, new EventArgs<Exception>(new Exception(_connProperties.ConnectedServerName + " Server Disconnected : IP " + ip + " Port " + port.ToString() + reason)));
            }
        }
        #endregion

        #region public methods

        public SocketConnection(Socket socketClient)
        {
            _socket = socketClient;
            _socketType = "Server";
            _connProperties = new ConnectionProperties();
            string[] data = _socket.LocalEndPoint.ToString().Split(':');
            if (data.Length == 2)
            {
                _connProperties.ServerIPAddress = data[0];
                _connProperties.Port = int.Parse(data[1]);
            }

        }

        public SocketConnection()
        {
            _socketType = "Client";
        }

        /// <summary>
        /// Setup the callback for recieved data and loss of conneciton
        /// </summary>
        /// <param name="app"></param>
        public void Connect()
        {
            try
            {
                int.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey("ServerHeartBeat"), out _heartBeatInterval);
                if (_heartBeatInterval != 0)
                {
                    _heartBeatTimer.Interval = _heartBeatInterval * 1000;
                    _heartBeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(_heartBeatTimer_Elapsed);
                    _heartBeatTimer.Start();
                    _timeoutInterval = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("ServerDisconnectInterval")) * 1000;
                    if (_timeoutInterval <= _heartBeatInterval)
                    {
                        //if disconnect timer less that heartbeat time set to twice+some constant of heartbeat time(one sec)
                        _timeoutInterval = _heartBeatInterval * 2 + 1000;
                    }
                    _timeOutTimer.Interval = _timeoutInterval;
                    _timeOutTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timeOutTimer_Elapsed);
                    _timeOutTimer.Start();
                }

                ThreadPool.QueueUserWorkItem(new WaitCallback(SendThreadEntryPoint));
                WaitForMessages();
            }
            catch (Exception ex)
            {
                Exception tempEx = new Exception("Nirvava: Recieve callback setup failed!", ex);
                bool rethrow = Logger.HandleException(tempEx, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void DisConnect(string reason)
        {
            try
            {
                DisConnectSocket(new Exception(reason));
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSOCKET);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public PranaInternalConstants.ConnectionStatus Connect(ConnectionProperties connProperties)
        {
            try
            {
                lock (lockSocket)
                {
                    _connectionStatus = PranaInternalConstants.ConnectionStatus.Connecting;
                    _heartBeatInterval = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("ClientHeartBeat")) * 1000;
                    _timeoutInterval = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("ClientDisconnectInterval")) * 1000;
                    _heartBeatTimer = new System.Timers.Timer();

                    _connProperties = connProperties;
                    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _ipHost = new IPEndPoint(IPAddress.Parse(connProperties.ServerIPAddress), connProperties.Port);

                    _socket.Connect(_ipHost);
                    //Beating
                    if (_heartBeatInterval != 0)
                    {
                        _heartBeatTimer.Interval = _heartBeatInterval;
                        _heartBeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(_heartBeatTimer_Elapsed);
                        _heartBeatTimer.Start();
                        if (_timeoutInterval != 0)
                        {
                            if (_timeoutInterval <= _heartBeatInterval)
                            {
                                //if disconnect timer less that heartbeat time set to twice+some constant of heartbeat time(one sec)
                                _timeoutInterval = _heartBeatInterval * 2 + 1000;
                            }
                            _timeOutTimer.Interval = _timeoutInterval;
                            _timeOutTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timeOutTimer_Elapsed);
                            _timeOutTimer.Start();
                        }
                    }

                    WaitForMessages();
                    if (_socket.Connected)
                    {
                        string hostName = _ipHost.Address.ToString();
                        int port = _ipHost.Port;
                        _serviceEndPoint = new ServiceEndPoint(hostName, port);
                    }
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SendThreadEntryPoint));
                    _connectionStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                    if (Connected != null)
                    {
                        Connected(this, null);
                    }
                }
            }
            catch (Exception ex)
            {
                _connectionStatus = PranaInternalConstants.ConnectionStatus.NOSERVER;
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSOCKET, 
                    $"Failed Socket connection to {connProperties.ConnectedServerName} server");
                if (Disconnected != null)
                {
                    Disconnected(this, null);
                }
                //RetryConnection();
            }
            return _connectionStatus;
        }

        public void SendMessage(string msg)
        {
            try
            {
                lock (lockTransmitQueue)
                {
                    TransmitQueue.Enqueue(msg);
                }
                DataReady.Set();
            }
            catch (Exception ex)
            {
                RaiseSocketException(ex, string.Empty);
            }
        }

        public void SendGenericMessage(String msg)
        {
            msg = msg + "\n";
            Byte[] bBuf = _utf8.GetBytes(msg);
            _socket.Send(bBuf);
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                _heartBeatTimer.Dispose();
                _timeOutTimer.Dispose();
                _socket.Dispose();
                StopEvent.Dispose();
                DataReady.Dispose();
            }
            // free native resources if there are any.          
        }
        #endregion
    }

    class StateObject
    {
        // public bool complete;

        public StateObject(int size)
        {
            _read_LENGTH_OF_DATA = size;
            buffer = new byte[_read_LENGTH_OF_DATA];
        }
        private byte[] buffer = new byte[8];
        private int _read_LENGTH_OF_DATA = 8;
        private string _readMsg = string.Empty;
        public void SetStateObjectForAppend(int read)
        {
            int difference = _read_LENGTH_OF_DATA - read;
            //LENGTH_OF_DATA = read;
            _read_LENGTH_OF_DATA = difference;
            _readMsg += System.Text.Encoding.UTF8.GetString(buffer, 0, read);
            buffer = new byte[difference];
        }
        public string ReadBytes()
        {
            _readMsg += System.Text.Encoding.UTF8.GetString(buffer, 0, _read_LENGTH_OF_DATA);
            return _readMsg;
        }

        public int READ_LENGTH_OF_DATA
        {
            get { return _read_LENGTH_OF_DATA; }
        }
        public byte[] Buffer
        {
            get { return buffer; }
        }
    }
}
