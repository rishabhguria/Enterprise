using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System.Collections;
using Prana.ServerClientCommon;
using System.Threading;
using Prana.QueueManager;
using Prana.BusinessObjects;
 
namespace Prana.Server.Core
{


    
   

    public class SocketConnection
    {
        private System.Timers.Timer _heartBeatTimer = new System.Timers.Timer();
        private System.Timers.Timer _timeOutTimer = new System.Timers.Timer();
        private Queue TransmitQueue = new Queue();
        private object lockTransmitQueue = new object();
        public event MessageReceivedDelegate MessageReceived;
        public event ExceptionDelegate SocketException;
        private const int MESSAGE_SIZE = 8;
        private Socket  _socket;
        private string _userID,_userName;
        System.Text.Encoding _ascii = System.Text.Encoding.ASCII;
        private int _heartBeatInterval = 120;
        private int _timeoutInterval = 120;
        private System.DateTime _lastHeartBeatReceiveTime = System.DateTime.Now.ToUniversalTime();
        private ManualResetEvent StopEvent = new ManualResetEvent(false);
        private AutoResetEvent DataReady = new AutoResetEvent(false);

        public SocketConnection(Socket socketClient)
        {
            _socket = socketClient;
        }

       

        #region Properties

        public string UserName
        {

            get
            {
                return this._userName;
            }
            set 
            {
                _userName=value;
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

        //public DateTime LastHeartBeatReceiveTime
        //{
        //    set { _lastHeartBeatReceiveTime = value; }
        //}
       

        #endregion

        #region private methods

        #region beating related methods

        void _heartBeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SendMessage(PranaMessageFormatter.CreateHeartBeatForUser(_userID));
        }
        void _timeOutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan t1 = System.DateTime.Now.ToUniversalTime().Subtract(_lastHeartBeatReceiveTime);
            TimeSpan t2 = new TimeSpan(_timeoutInterval * 10000);

            if (TimeSpan.Compare(t1, t2) == 1) // if allowed is greater than real time diff 
            {
                SocketException(this, new Exception("Time out happened"));

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
            string message = msg.Substring(0, msg.Length - 2);
            switch (PranaMessageFormatter.GetMessageType(message))
            {
                //case FIXConstants.MSGLogon:

                //    break;
                //case FIXConstants.MSGLogout:

                //    break;
                case FIXConstants.MSGHeartbeat:
                    _lastHeartBeatReceiveTime = System.DateTime.Now.ToUniversalTime();
                    break;
                default:
                    if (MessageReceived != null)
                        MessageReceived(this, message);
                    break;
            }
        }

        public void GetLengthOfData(IAsyncResult ar)
        {
            try
            {
                StateObject sLengthOfData = (StateObject)ar.AsyncState;
                int read = 0;
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
                    SocketException(this, new Exception("User Requested to disconnect"));
                }



                // string message = "";
                // int length = 0;

                //// lock (lockTransmitQueue)
                // //{
                //     StateObject sObject = (StateObject)ar.AsyncState;
                //     int read = _socket.EndReceive(ar);
                //     if (read > 0)
                //     {
                //          message = System.Text.Encoding.ASCII.GetString(sObject.buffer, 0, read);
                //         length = int.Parse(message);
                //         StateObject so = new StateObject();
                //         so.buffer = new byte[length];
                //         AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetStreamMsg);
                //         if (_socket != null)
                //             _socket.BeginReceive(so.buffer, 0, length, SocketFlags.None, GetStreamMsgCallback, so);
                //     }
                // }
            }
            catch (Exception ex)
            {
                SocketException(this, ex);
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
                        string message = sObject.ReadBytes();

                        SendToManager(message);
                        WaitForMessages();
                    }
                }
                else
                {
                    SocketException(this, new Exception("User Requested to disconnect"));
                }

                //StateObject sObject = (StateObject)ar.AsyncState;
                //int read = _socket.EndReceive(ar);
                //if (read > 0)
                //{
                //    string message = System.Text.Encoding.ASCII.GetString(sObject.buffer, 0, read);
                //    MessageReceived(this, message);
                //    WaitForMessages();
                //}
            }
            catch (Exception ex)
            {
                SocketException(this, ex);
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
                //DisconnectSocket();
                SocketException(this, ex);
            }
        }

        public void AppendString(IAsyncResult ar)
        {
            try
            {
                StateObject sObject = (StateObject)ar.AsyncState;
                int read = _socket.EndReceive(ar);
                if (read > 0)
                {

                    if (read == sObject.READ_LENGTH_OF_DATA)
                    {
                        //QueueMessage queueMessage = new QueueMessage(string.Empty, sObject.ReadBytes(), int.MinValue);
                        //_socketQueue.SendMessage(queueMessage);

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
                    SocketException(this, new Exception("User Requested to disconnect"));
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

        #endregion

        private  void SendThreadEntryPoint(object state)
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
                            int msgLength=_ascii.GetBytes(msg).Length + 1;
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
            catch(Exception ex) 
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
        /// <summary>
        /// Setup the callback for recieved data and loss of conneciton
        /// </summary>
        /// <param name="app"></param>
        public void Connect()
        {
            try
            {
                
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["ServerHeartBeat"], out _heartBeatInterval);
                if (_heartBeatInterval != 0)
                {
                    _heartBeatTimer.Interval = _heartBeatInterval * 1000;
                    _heartBeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(_heartBeatTimer_Elapsed);
                    _heartBeatTimer.Start();
                    _timeoutInterval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ServerDisconnectInterval"]) * 1000;
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

                bool rethrow = ExceptionPolicy.HandleException(tempEx, ApplicationConstants.POLICY_LOGANDSHOW);

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
                SocketException(this, ex);
            }
        }
        public void AsynchSendCallback(System.IAsyncResult ar)
        {
            //ar.IsCompleted 
            try
            {
                // sanity check
                if (_socket == null || !_socket.Connected) return;
                int send = _socket.EndSend(ar);
            }
            catch { }
        }

        public void DisconnectSocket()
        {
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
                StopEvent.Set();
                StopTimersAndUnwireEvents();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
        }

       
        #endregion
    }

     class StateObject
    {
         public bool complete;
         
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
