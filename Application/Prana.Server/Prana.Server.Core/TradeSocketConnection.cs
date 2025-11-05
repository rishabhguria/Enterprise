//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Net.Sockets;
//using System.Net;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
//using Prana.Global;
//using System.Collections;
//using Prana.ServerClientCommon;
//using System.Threading;
//using Prana.QueueManager;
//namespace Prana.Server.Core
//{





//    public class TradeSocketConnection
//    {
//        private System.Timers.Timer _heartBeatTimer = new System.Timers.Timer();
//        public event MessageReceivedDelegate MessageReceived;
//        //public event MessageReceivedDelegate Connected;
//        public event ExceptionDelegate SocketException;
//        private const int sizeBytes = 8;
//        //private const int  _maxLengthOfData = 64000;
//        //private byte[] recByte = new byte[sizeBytes];
//        //private byte[] ReadByte = new byte[sizeBytes];
//        private StringBuilder myBuilder = new StringBuilder();
//        private Socket _socketClient;
//        private string _userID, _userName;
//        //private byte[] m_byBuff = new byte[56];
//        System.Text.Encoding _ascii = System.Text.Encoding.ASCII;
//        private int _timeoutInterval = 8000;
//        private System.DateTime _lastHeartBeatReceiveTime = System.DateTime.Now.ToUniversalTime();
//        //  private object lockSocketObject = new object();


//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="sock">client socket conneciton this object represents</param>
//        public TradeSocketConnection(Socket socketClient)
//        {
//            _socketClient = socketClient;



//        }




//        /// <summary>
//        /// Setup the callback for recieved data and loss of conneciton
//        /// </summary>
//        /// <param name="app"></param>
//        public void Connect()
//        {
//            try
//            {
//                int heartBeat = 0;
//                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["ServerHeartBeat"], out heartBeat);
//                if (heartBeat != 0)
//                {
//                    _heartBeatTimer.Interval = heartBeat * 1000;
//                    _heartBeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(_heartBeatTimer_Elapsed);
//                    _heartBeatTimer.Start();
//                }
//                WaitForMessages();
//            }
//            catch (Exception ex)
//            {
//                Exception tempEx = new Exception("Nirvava: Recieve callback setup failed!", ex);

//                bool rethrow = ExceptionPolicy.HandleException(tempEx, ApplicationConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        void _heartBeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
//        {
//            SendMessage(Prana.ServerClientCommon.Transformer.CreateHeartBeatForUser(_userID).ToString());
//        }



//        public void GetLengthOfData(IAsyncResult ar)
//        {
//            try
//            {
//                StateObject sLengthOfData = (StateObject)ar.AsyncState;
//                int read = 0;
//                read = _socketClient.EndReceive(ar);
//                if (read > 0)
//                {
//                    if (read == sLengthOfData.READ_LENGTH_OF_DATA)
//                    {
//                        int length = int.Parse(sLengthOfData.ReadBytes());
//                        StateObject soMessage = new StateObject(length);
//                        AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetStreamMsg);
//                        try
//                        {
//                            _socketClient.BeginReceive(soMessage.Buffer, 0, soMessage.READ_LENGTH_OF_DATA, SocketFlags.None, GetStreamMsgCallback, soMessage);
//                        }
//                        catch (Exception ex)
//                        {
//                            // Disconnect(null);
//                        }
//                    }
//                    else
//                    {
//                        sLengthOfData.SetStateObjectForAppend(read);
//                        AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetLengthOfData);
//                        _socketClient.BeginReceive(sLengthOfData.Buffer, 0, sLengthOfData.READ_LENGTH_OF_DATA, SocketFlags.None, GetStreamMsgCallback, sLengthOfData);
//                    }
//                }




//                // string message = "";
//                // int length = 0;

//                //// lock (lockSocketObject)
//                // //{
//                //     StateObject sObject = (StateObject)ar.AsyncState;
//                //     int read = _socketClient.EndReceive(ar);
//                //     if (read > 0)
//                //     {
//                //          message = System.Text.Encoding.ASCII.GetString(sObject.buffer, 0, read);
//                //         length = int.Parse(message);
//                //         StateObject so = new StateObject();
//                //         so.buffer = new byte[length];
//                //         AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetStreamMsg);
//                //         if (_socketClient != null)
//                //             _socketClient.BeginReceive(so.buffer, 0, length, SocketFlags.None, GetStreamMsgCallback, so);
//                //     }
//                // }
//            }
//            catch (Exception ex)
//            {
//                SocketException(this, ex);
//            }
//        }

//        public void GetStreamMsg(IAsyncResult ar)
//        {
//            try
//            {
//                StateObject sObject = (StateObject)ar.AsyncState;
//                int read = _socketClient.EndReceive(ar);

//                if (read > 0)
//                {
//                    if (read < sObject.READ_LENGTH_OF_DATA)
//                    {
//                        sObject.SetStateObjectForAppend(read);
//                        AsyncCallback AppendStringCallback = new AsyncCallback(AppendString);
//                        _socketClient.BeginReceive(sObject.Buffer, 0, sObject.READ_LENGTH_OF_DATA, SocketFlags.None, AppendStringCallback, sObject);
//                    }
//                    else
//                    {
//                        string message = sObject.ReadBytes();

//                        MessageReceived(this, message);
//                        WaitForMessages();
//                    }
//                }

//                //StateObject sObject = (StateObject)ar.AsyncState;
//                //int read = _socketClient.EndReceive(ar);
//                //if (read > 0)
//                //{
//                //    string message = System.Text.Encoding.ASCII.GetString(sObject.buffer, 0, read);
//                //    MessageReceived(this, message);
//                //    WaitForMessages();
//                //}
//            }
//            catch (Exception ex)
//            {
//                SocketException(this, ex);
//            }
//        }
//        private void WaitForMessages()
//        {
//            try
//            {
//                StateObject sObject = new StateObject(sizeBytes);
//                AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetLengthOfData);
//                if (_socketClient != null)
//                    _socketClient.BeginReceive(sObject.Buffer, 0, sObject.READ_LENGTH_OF_DATA, SocketFlags.None, GetStreamMsgCallback, sObject);
//            }
//            catch (Exception ex)
//            {
//                //DisconnectSocket();
//                SocketException(this, ex);
//            }
//        }

//        public class StateObject
//        {
//            public StateObject(int size)
//            {
//                _read_LENGTH_OF_DATA = size;
//                buffer = new byte[_read_LENGTH_OF_DATA];

//            }
//            private byte[] buffer = new byte[8];
//            private int _read_LENGTH_OF_DATA = 8;
//            private string _readMsg = string.Empty;
//            public void SetStateObjectForAppend(int read)
//            {
//                int difference = _read_LENGTH_OF_DATA - read;
//                //LENGTH_OF_DATA = read;
//                _read_LENGTH_OF_DATA = difference;
//                _readMsg += System.Text.Encoding.ASCII.GetString(buffer, 0, read);
//                buffer = new byte[difference];

//            }
//            public string ReadBytes()
//            {
//                _readMsg += System.Text.Encoding.ASCII.GetString(buffer, 0, _read_LENGTH_OF_DATA);
//                return _readMsg;
//            }

//            public int READ_LENGTH_OF_DATA
//            {
//                get { return _read_LENGTH_OF_DATA; }
//            }
//            public byte[] Buffer
//            {
//                get { return buffer; }
//            }
//        }
//        public void AppendString(IAsyncResult ar)
//        {
//            try
//            {
//                StateObject sObject = (StateObject)ar.AsyncState;
//                int read = _socketClient.EndReceive(ar);
//                if (read > 0)
//                {

//                    if (read == sObject.READ_LENGTH_OF_DATA)
//                    {
//                        //QueueMessage queueMessage = new QueueMessage(string.Empty, sObject.ReadBytes(), int.MinValue);
//                        //_socketQueue.SendMessage(queueMessage);
//                        string message = sObject.ReadBytes();
//                        MessageReceived(this, message);
//                        WaitForMessages();
//                    }

//                    else
//                    {
//                        sObject.SetStateObjectForAppend(read);
//                        AsyncCallback AppendStringCallback = new AsyncCallback(AppendString);
//                        _socketClient.BeginReceive(sObject.Buffer, 0, sObject.READ_LENGTH_OF_DATA, SocketFlags.None, AppendStringCallback, sObject);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {

//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }


//        #region Properties
//        public string UserName
//        {

//            get
//            {
//                return this._userName;
//            }
//            set
//            {
//                _userName = value;
//            }
//        }

//        public string UserID
//        {

//            get
//            {
//                return this._userID;
//            }
//            set
//            {
//                _userID = value;
//            }
//        }
//        public DateTime LastHeartBeatReceiveTime
//        {
//            set { _lastHeartBeatReceiveTime = value; }
//        }



//        #endregion

//        public void SendMessage(string msg)
//        {
//            try
//            {
//                //if (_socketClient != null)
//                //{
//                // lock (lockSocketObject)
//                // {
//                msg = msg + "@";
//                string bBufLength = (_ascii.GetBytes(msg).Length + 1).ToString().PadLeft(sizeBytes, '0');
//                Byte[] bBuf = _ascii.GetBytes(bBufLength + msg + "@");
//                if (_socketClient != null && _socketClient.Connected)
//                    _socketClient.Send(bBuf);
//                // }
//                // }
//            }
//            catch (Exception ex)
//            {
//                SocketException(this, ex);
//            }
//        }


//        public void DisconnectSocket()
//        {
//            try
//            {
//                // lock (lockSocketObject)
//                // {
//                //string message = PranaMessageFormatter.CreateLogOutMessageForUser(_userID);
//                //SendMessage(message);
//                _socketClient.Shutdown(SocketShutdown.Both);
//                _socketClient.Close();
//                _socketClient = null;
//                //  }

//                //}
//                //_socketClient = null;
//            }
//            catch (Exception ex)
//            {
//                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
//            }
//        }
//        public bool ISBeatingTimedOut()
//        {
//            int difference = (System.DateTime.Now.ToUniversalTime().Second - _lastHeartBeatReceiveTime.Second) * 1000;
//            if (difference > _timeoutInterval)
//            {
//                return true;
//            }
//            else
//            {
//                // _lastHeartBeatReceiveTime = System.DateTime.Now.ToUniversalTime();
//                SendMessage(Prana.ServerClientCommon.Transformer.CreateHeartBeatForUser(_userID).ToString());
//                return false;
//            }
//        }
//    }



//}
