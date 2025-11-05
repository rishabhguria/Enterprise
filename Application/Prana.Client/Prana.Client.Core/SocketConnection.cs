using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Net;
using Prana.BusinessObjects;
using Prana.Global;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Prana.ServerClientCommon;
using Prana.Interfaces;
using System.Collections.Generic;
using Prana.QueueManager;
using System.Threading;

namespace Prana.Client.Core
{
    class SocketConnection
    {
        private string _userID, _userName;
        private const int MESSAGE_SIZE = 8;
        Socket _socket = null;
        private Queue TransmitQueue = new Queue();
        private object lockTransmitQueue = new object();
        ConnectionProperties _connProperties = null;
       // private static object _lockSocket = new object();
        IPEndPoint _ipHost = null;
        private static object _lockSocketQueue = new object();
        System.Text.Encoding _ascii = System.Text.Encoding.ASCII;
        public event MessageDelegate MessageReceived;
        private PranaInternalConstants.ConnectionStatus _connectionStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
        public event EventHandler Disconnected;
        private ManualResetEvent StopEvent = new ManualResetEvent(false);
        private AutoResetEvent DataReady = new AutoResetEvent(false);

        #region Beating Related Values
        private System.Timers.Timer _heartBeatTimer = new System.Timers.Timer();
        private System.DateTime _lastHeartBeatReceiveTime = System.DateTime.Now.ToUniversalTime();
        private int _heartBeatInterval = 60000;
        System.Timers.Timer _timeOutTimer = new System.Timers.Timer();
        private int _timeoutInterval = 121000;
        #endregion

        #region private methods

        void _heartBeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SendMessage(Prana.ServerClientCommon.PranaMessageFormatter.CreateHeartBeatForUser(_userID));
        }

        void _timeOutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                TimeSpan t1 = System.DateTime.Now.ToUniversalTime().Subtract(_lastHeartBeatReceiveTime);
                TimeSpan t2 = new TimeSpan(_timeoutInterval * 10000);

                if (TimeSpan.Compare(t1, t2) == 1) // if allowed is greater than real time diff 
                {
                    DisConnect("DisconnectTimer Elapsed");
                }


            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            //DisConnect(new Exception("EPNL Server taking too long to respond. DisConnected"));
        }

        #region Socket read Methods
        void SendToManager(string msg)
        {
            string message = msg.Substring(0, msg.Length - 2);
            switch (PranaMessageFormatter.GetMessageType(message))
            {
                case FIXConstants.MSGLogon:
                    // TBD
                    break;
                case FIXConstants.MSGLogout:
                    _connectionStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                    DisConnect("Logged out by Server");
                    break;
                case FIXConstants.MSGHeartbeat:
                    _connectionStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                    _lastHeartBeatReceiveTime = System.DateTime.Now.ToUniversalTime();
                    break;
                default:
                    if (MessageReceived != null)
                        MessageReceived(message);
                    break;
            }
        }

        private void WaitForMessages()
        {
            try
            {
                StateObject sObject = new StateObject(8);
                AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetLengthOfData);
               // lock (_lockSocket)
               // {
                    _socket.BeginReceive(sObject.Buffer, 0, sObject.READ_LENGTH_OF_DATA, SocketFlags.None, GetStreamMsgCallback, sObject);
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                DisConnectSocket(ex);
            }
        }

        public void GetStreamMsg(IAsyncResult ar)
        {
            try
            {



                StateObject sObject = (StateObject)ar.AsyncState;
                int read = 0;
              //  lock (_lockSocket)
               // {
                    read = _socket.EndReceive(ar);
                //}

                if (read > 0)
                {
                    if (read < sObject.READ_LENGTH_OF_DATA)
                    {
                        sObject.SetStateObjectForAppend(read);
                        AsyncCallback AppendStringCallback = new AsyncCallback(AppendString);
                        //lock (_lockSocket)
                       // {
                            _socket.BeginReceive(sObject.Buffer, 0, sObject.READ_LENGTH_OF_DATA, SocketFlags.None, AppendStringCallback, sObject);
                        //}
                    }
                    else
                    {
                        SendToManager(sObject.ReadBytes());
                        //string message = sObject.ReadBytes();
                        //QueueMessage queueMessage = new QueueMessage(string.Empty, message, int.MinValue);
                        //lock (_lockSocketQueue)
                        //{
                        //    _socketQueue.SendMessage(queueMessage);
                        //}
                        WaitForMessages();
                    }
                }
                else
                {
                    DisConnect("Server Disconnected");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                DisConnectSocket(ex);
            }
        }

        public void AppendString(IAsyncResult ar)
        {
            try
            {
                StateObject sObject = (StateObject)ar.AsyncState;
                int read = 0;
              //  lock (_lockSocket)
                //{
                    read = _socket.EndReceive(ar);
               // }
                if (read > 0)
                {

                    if (read == sObject.READ_LENGTH_OF_DATA)
                    {
                        SendToManager(sObject.ReadBytes());
                        //QueueMessage queueMessage = new QueueMessage(string.Empty, sObject.ReadBytes(), int.MinValue);
                        //lock (_lockSocketQueue)
                        //{
                        //    _socketQueue.SendMessage(queueMessage);
                        //}
                        WaitForMessages();
                    }

                    else
                    {
                        sObject.SetStateObjectForAppend(read);
                        AsyncCallback AppendStringCallback = new AsyncCallback(AppendString);
                       // lock (_lockSocket)
                       // {
                            _socket.BeginReceive(sObject.Buffer, 0, sObject.READ_LENGTH_OF_DATA, SocketFlags.None, AppendStringCallback, sObject);
                       // }
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
            }
        }

        public void GetLengthOfData(IAsyncResult ar)
        {
            string message = string.Empty;
            try
            {
                StateObject sLengthOfData = (StateObject)ar.AsyncState;
                int read = 0;
               // lock (_lockSocket)
               // {
                    read = _socket.EndReceive(ar);
               // }
                if (read > 0)
                {
                    if (read == sLengthOfData.READ_LENGTH_OF_DATA)
                    {
                        message = sLengthOfData.ReadBytes();
                        int length = int.Parse(message);
                        StateObject soMessage = new StateObject(length);
                        AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetStreamMsg);
                       // lock (_lockSocket)
                       // {
                            _socket.BeginReceive(soMessage.Buffer, 0, soMessage.READ_LENGTH_OF_DATA, SocketFlags.None, GetStreamMsgCallback, soMessage);
                       // }
                    }
                    else
                    {
                        sLengthOfData.SetStateObjectForAppend(read);
                        AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetLengthOfData);
                       // lock (_lockSocket)
                       // {
                            _socket.BeginReceive(sLengthOfData.Buffer, 0, sLengthOfData.READ_LENGTH_OF_DATA, SocketFlags.None, GetStreamMsgCallback, sLengthOfData);
                       // }
                    }
                }
                else
                {
                    DisConnect("Server Disconnected");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                DisConnectSocket(ex);

            }
        }

        #endregion

        private void DisConnectSocket(Exception ex)
        {

            try
            {

                //lock (_lockSocket)
                //{

                    if (_socket != null)
                    {
                        StopTimersAndUnwireEvents();
                        _connectionStatus = PranaInternalConstants.ConnectionStatus.NOSERVER;
                        if (Disconnected != null)
                        {
                            Disconnected(this, null);
                        }
                        _socket.Shutdown(SocketShutdown.Both);
                        _socket.Close();
                        _socket = null;
                        StopEvent.Set();
                        ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                    }
                //}

            }
            catch (Exception ex1)
            {

                ExceptionPolicy.HandleException(ex1, ApplicationConstants.POLICY_LOGANDSHOW);
            }



        }

        private void StopTimersAndUnwireEvents()
        {
            _heartBeatTimer.Stop();
            _heartBeatTimer.Elapsed -= new System.Timers.ElapsedEventHandler(_heartBeatTimer_Elapsed);
            _timeOutTimer.Stop();
            _timeOutTimer.Elapsed -= new System.Timers.ElapsedEventHandler(_timeOutTimer_Elapsed);
        }
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
                            string msg = string.Empty;
                            msg = message + "@";
                            int msgLength = _ascii.GetBytes(msg).Length + 1;
                            string bBufLength = msgLength.ToString().PadLeft(MESSAGE_SIZE, '0');
                            Byte[] bBuf = _ascii.GetBytes(bBufLength + msg + "@");
                            StateObject so2 = new StateObject(bBuf.Length);

                            // send it...
                            //System.IAsyncResult iar;
                            _socket.Send(bBuf);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region public methods

        public SocketConnection()
        {
            //_socketQueue = new QueueProcessor(this);
            //_socketQueue.MessageQueued += new MessageReceivedHandler(_socketQueue_MessageQueued);
        }

        public PranaInternalConstants.ConnectionStatus Connect(ConnectionProperties connProperties)
        {
            try
            {

                _heartBeatInterval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ClientHeartBeat"]) * 1000;
                _timeoutInterval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ClientDisconnectInterval"]) * 1000;
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
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendThreadEntryPoint));
                _connectionStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                //if (connProperties.User == null)
                //{
                //    _userID = connProperties.IdentifierID;
                //    _userName = connProperties.IdentifierName;
                //    SendMessage(Transformer.CreateLogOnMessage(_userID, _userName).ToString());
                //}
                //else
                //{

                //    _userID = connProperties.User.CompanyUserID.ToString();
                //    _userName = connProperties.User.FirstName + " " + connProperties.User.LastName;
                //    SendMessage(Transformer.CreateLogOnMessage(connProperties.User).ToString());
                //}

            }
            catch (Exception ex)
            {
                _connectionStatus = PranaInternalConstants.ConnectionStatus.NOSERVER;
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
            }
            return _connectionStatus;
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
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
                DisConnectSocket(ex);
            }
        }

        public PranaInternalConstants.ConnectionStatus ConnectionStatus
        {
            get { return _connectionStatus; }
        }
        #endregion
    }
    class StateObject
    {
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
            _readMsg += System.Text.Encoding.ASCII.GetString(buffer, 0, read);
            buffer = new byte[difference];

        }
        public string ReadBytes()
        {
            _readMsg += System.Text.Encoding.ASCII.GetString(buffer, 0, _read_LENGTH_OF_DATA);
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
