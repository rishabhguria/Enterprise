using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects.FIX
{
    [Serializable]
    public class PranaMessage
    {
        private string _messageType = string.Empty;
        private string _tradingAccountID = string.Empty;
        private string _userID = string.Empty;
        FIXMessage _fixMessage = null;
        FIXMessageList _fixMessages = null;

        public PranaMessage()
        {
            _fixMessage = new FIXMessage();
            _fixMessages = new FIXMessageList();
        }

        public PranaMessage(Order order)
        {
        }

        public PranaMessage(OrderSingle order)
        {
        }

        public PranaMessage(string msgType, int tradingAccts)
        {
            _messageType = msgType;
            _fixMessage = new FIXMessage();
            _fixMessages = new FIXMessageList();
        }

        public PranaMessage(string message)
        {
            string[] strList = message.Split(Seperators.SEPERATOR_2);
            if (strList[0] != string.Empty)
            {
                _messageType = strList[0];
            }
            if (strList[1] != string.Empty)
            {
                _tradingAccountID = strList[1];
            }
            if (strList[2] != string.Empty)
            {
                _fixMessage = new FIXMessage(strList[2]);
            }
            if (strList[3] != string.Empty)
            {
                _fixMessages = new FIXMessageList(strList[3]);
            }
        }

        public FIXMessage FIXMessage
        {
            get { return _fixMessage; }
            set { _fixMessage = value; }
        }

        public FIXMessageList FIXMessageList
        {
            get { return _fixMessages; }
        }

        public string MessageType
        {
            set { _messageType = value; }
            get { return _messageType; }
        }

        public override string ToString()
        {
            string strSingleMsg = string.Empty;

            if (_fixMessage != null)
            {
                strSingleMsg = _fixMessage.ToString();
            }
            string strMsgList = string.Empty;
            if (_fixMessages != null)
            {
                strMsgList = _fixMessages.ToString();
            }
            return _messageType + Seperators.SEPERATOR_2 + _tradingAccountID + Seperators.SEPERATOR_2 + strSingleMsg + Seperators.SEPERATOR_2 + strMsgList;
        }

        public string TradingAccountID
        {
            get
            {

                if (_tradingAccountID != string.Empty)
                    return _tradingAccountID;
                else if (_fixMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TradingAccountID))
                    return _fixMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradingAccountID].Value;
                else
                    return string.Empty;
            }
            set
            {
                _tradingAccountID = value;
            }
        }

        public string UserID
        {
            get
            {
                return string.IsNullOrEmpty(_userID) && _fixMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CompanyUserID) ?
                       _fixMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value : _userID;
            }
            set
            {
                _tradingAccountID = value;
                _userID = value;
            }
        }

        public string UserIDForCompliance
        {
            get
            {
                if (_userID != string.Empty)
                    return _userID;
                else if (_fixMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ModifiedUserId))
                {
                    return _fixMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ModifiedUserId].Value;
                }
                else if (_fixMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CompanyUserID))
                    return _fixMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value;
                else
                    return string.Empty;
            }
        }

        public PranaMessage Clone()
        {
            PranaMessage PranaMsg = new PranaMessage();
            PranaMsg.FIXMessage = _fixMessage.Clone();
            PranaMsg.MessageType = _messageType;
            foreach (string requiredField in _fixMessage.RequiredFixFields)
            {
                PranaMsg.FIXMessage.RequiredFixFields.Add(requiredField);
            }
            return PranaMsg;
        }

        public String getFixMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_fixMessage.getHeaderFields());
            sb.Append(_fixMessage.getBodyFields());
            int length = sb.Length;

            String msgExcludingcheckSum = FIXConstants.TagBeginString + "=FIX.4.2" + Seperators.DELIMITER + FIXConstants.TagBodyLength + "=" + length + Seperators.DELIMITER + sb.ToString();
            return msgExcludingcheckSum;
        }

        public void createFromFixMessage(String msg)
        {
            try
            {
                _fixMessage = new FIXMessage();
                _fixMessage.CreateFIXMessage(msg);

                _messageType = _fixMessage.ExternalInformation.GetField(FIXConstants.TagMsgType);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw new Exception("Problem while parsing data received from FIX. Received Message is: " + msg, ex);
                }
            }
        }

        public void CreateSideFromOpenCloseTags()
        {
            if (!_fixMessage.ExternalInformation.ContainsKey(FIXConstants.TagOpenClose))
                return;
            if (!_fixMessage.ExternalInformation.ContainsKey(FIXConstants.TagSide))
                return;

            string side = _fixMessage.ExternalInformation[FIXConstants.TagSide].Value.Trim();
            string openClose = _fixMessage.ExternalInformation[FIXConstants.TagOpenClose].Value.Trim();
            switch (openClose)
            {
                case FIXConstants.Open:
                    if (side == FIXConstants.SIDE_Buy)
                        _fixMessage.ExternalInformation[FIXConstants.TagSide].Value = FIXConstants.SIDE_Buy_Open;
                    else if (side == FIXConstants.SIDE_Sell)
                        _fixMessage.ExternalInformation[FIXConstants.TagSide].Value = FIXConstants.SIDE_Sell_Open;
                    break;
                case FIXConstants.Close:

                    if (side == FIXConstants.SIDE_Buy)
                        _fixMessage.ExternalInformation[FIXConstants.TagSide].Value = FIXConstants.SIDE_Buy_Closed;
                    else if (side == FIXConstants.SIDE_Sell)
                        _fixMessage.ExternalInformation[FIXConstants.TagSide].Value = FIXConstants.SIDE_Sell_Closed;
                    break;
            }
        }
    }

    [Serializable]
    public class FIXMessage
    {
        public FIXMessage()
        { }

        public FIXMessage(MessageFieldCollection externDictMessage, MessageFieldCollection internDictMessage, MessageFieldCollection custDictMessage)
        {
            _externDictMessage = externDictMessage;
            _internDictMessage = internDictMessage;
            _customDictMessage = custDictMessage;
        }

        public void CreateFIXMessage(String msg)
        {
            _externDictMessage = new MessageFieldCollection(msg);
            _internDictMessage = new MessageFieldCollection();
            _customDictMessage = new MessageFieldCollection();
        }

        List<string> _requiredFixFields = new List<string>();
        public List<string> RequiredFixFields
        {
            get { return _requiredFixFields; }
            set { _requiredFixFields = value; }
        }

        List<string> _optionalFixFields = new List<string>();
        public List<string> OptionalFixFields
        {
            get { return _optionalFixFields; }
            set { _optionalFixFields = value; }
        }

        MessageFieldCollection _externDictMessage = new MessageFieldCollection();
        MessageFieldCollection _internDictMessage = new MessageFieldCollection();
        MessageFieldCollection _customDictMessage = new MessageFieldCollection();
        SerializableDictionary<string, RepeatingGroup> _childGroups = new SerializableDictionary<string, RepeatingGroup>();

        public String getMsgType()
        {
            return _externDictMessage.GetField(FIXConstants.TagMsgType);
        }

        public FIXMessage(string message)
        {
            string[] data = message.Split(Seperators.SEPERATOR_3);
            string[] externList = data[0].Split(Seperators.DELIMITER);
            string[] internList = data[1].Split(Seperators.DELIMITER);
            string[] customList = data[2].Split(Seperators.DELIMITER);
            _externDictMessage.AddFields(externList);
            _internDictMessage.AddFields(internList);
            MessageField messageFieldAlgoTags = _internDictMessage.GetAllMessageFieldForTag(CustomFIXConstants.CUST_TAG_AlgoProperties);
            if (messageFieldAlgoTags != null)
            {
                string[] lsAlgoProp = messageFieldAlgoTags.Value.Replace(Seperators.SEPERATOR_6, Seperators.SEPERATOR_12).Split(Seperators.SEPERATOR_5);
                if (lsAlgoProp != null && lsAlgoProp.Length > 0)
                {
                    _externDictMessage.AddFields(lsAlgoProp);
                }
            }
            _customDictMessage.AddFields(customList);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_externDictMessage);
            sb.Append(Seperators.SEPERATOR_3);
            sb.Append(_internDictMessage);
            sb.Append(Seperators.SEPERATOR_3);
            sb.Append(_customDictMessage);
            sb.Append(Seperators.SEPERATOR_3);
            return sb.ToString();
        }

        public MessageFieldCollection ExternalInformation
        {
            get { return _externDictMessage; }
        }

        public SerializableDictionary<string, RepeatingGroup> ChildGroups
        {
            get { return _childGroups; }
            set { _childGroups = value; }
        }

        public string getHeaderFields()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_externDictMessage.GetFieldWithValue(FIXConstants.TagMsgType));
            sb.Append(_externDictMessage.GetFieldWithValue(FIXConstants.TagSenderCompID));
            if (_externDictMessage.ContainsKey(FIXConstants.TagSendingTime))
            {
                sb.Append(_externDictMessage.GetFieldWithValue(FIXConstants.TagSendingTime));
            }
            sb.Append(_externDictMessage.GetFieldWithValue(FIXConstants.TagTargetCompID));
            return sb.ToString();
        }

        public MessageFieldCollection getBodyFields()
        {
            MessageFieldCollection cloned = new MessageFieldCollection();
            foreach (MessageField field in _externDictMessage.MessageFields)
            {
                if (!isHeaderField(field.Tag))
                {
                    cloned.AddField(field.Tag, field.Value);
                }
            }
            return cloned;
        }

        public MessageFieldCollection CustomInformation
        {
            get { return _customDictMessage; }
        }

        public MessageFieldCollection InternalInformation
        {
            get { return _internDictMessage; }
        }

        static bool isHeaderField(string field)
        {
            switch (field)
            {
                case FIXConstants.TagBeginString:
                case FIXConstants.TagBodyLength:
                case FIXConstants.TagMsgType:
                case FIXConstants.TagSenderCompID:
                case FIXConstants.TagTargetCompID:
                case FIXConstants.TagOnBehalfOfCompID:
                case FIXConstants.TagDeliverToCompID:
                case FIXConstants.TagSecureDataLen:
                case FIXConstants.TagMsgSeqNum:
                case FIXConstants.TagSenderSubID:
                case FIXConstants.TagSenderLocationID:
                case FIXConstants.TagTargetSubID:
                case FIXConstants.TagTargetLocationID:
                case FIXConstants.TagOnBehalfOfSubID:
                case FIXConstants.TagOnBehalfOfLocationID:
                case FIXConstants.TagDeliverToSubID:
                case FIXConstants.TagDeliverToLocationID:
                case FIXConstants.TagPossDupFlag:
                case FIXConstants.TagPossResend:
                case FIXConstants.TagSendingTime:
                case FIXConstants.TagOrigSendingTime:
                case FIXConstants.TagXmlDataLen:
                case FIXConstants.TagXmlData:
                case FIXConstants.TagMessageEncoding:
                case FIXConstants.TagLastMsgSeqNumProcessed:
                case FIXConstants.TagOnBehalfOfSendingTime:
                    return true;
                default:
                    return false;
            }
        }

        public FIXMessage Clone()
        {
            FIXMessage fixMsg = new FIXMessage(_externDictMessage.Clone(), _internDictMessage.Clone(), _customDictMessage.Clone());
            return fixMsg;
        }

        public void CloneInternalInfo(FIXMessage fixMsg)
        {
            string tempPranaSeqNumber = _internDictMessage[CustomFIXConstants.CUST_TAG_OrderSeqNumber].Value;

            _internDictMessage = new MessageFieldCollection(fixMsg.InternalInformation.MessageFields);

            _internDictMessage.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, tempPranaSeqNumber);
        }
    }

    [Serializable]
    public class FIXMessageList
    {
        string _basketID = string.Empty;
        string _groupID = string.Empty;
        string _waveID = string.Empty;
        string _tradedBasketID = string.Empty;

        List<FIXMessage> _fixMessages = null;
        public override string ToString()
        {
            StringBuilder strMsgList = new StringBuilder();
            strMsgList.Append(_basketID).Append(Seperators.SEPERATOR_1).Append(_groupID).Append(Seperators.SEPERATOR_1).Append(_waveID).Append(Seperators.SEPERATOR_1).Append(_tradedBasketID).Append(Seperators.SEPERATOR_1).Append(_userID).Append(Seperators.SEPERATOR_1);

            if (_fixMessages != null)
            {
                foreach (FIXMessage msg in _fixMessages)
                {
                    strMsgList.Append(Seperators.SEPERATOR_4).Append(msg.ToString());
                }
            }
            return strMsgList.ToString();
        }

        public FIXMessageList()
        {
            _fixMessages = new List<FIXMessage>();
        }

        public FIXMessageList(string message)
        {
            if (message != string.Empty)
            {
                _fixMessages = new List<FIXMessage>();
                string[] listOfProps = message.Split(Seperators.SEPERATOR_1);
                if (listOfProps.Length > 0)
                {
                    _basketID = listOfProps[0];
                    _groupID = listOfProps[1];
                    _waveID = listOfProps[2];
                    _tradedBasketID = listOfProps[3];
                    _userID = listOfProps[4];
                    string[] listOfMessage = listOfProps[5].Split(Seperators.SEPERATOR_4);
                    foreach (string strMsg in listOfMessage)
                    {
                        if (strMsg != string.Empty)
                        {
                            FIXMessage msg = new FIXMessage(strMsg);
                            _fixMessages.Add(msg);
                        }
                    }
                }
            }
        }

        public string TradedBasketID
        {
            set { _tradedBasketID = value; }
            get { return _tradedBasketID; }
        }

        public string GroupID
        {
            set { _groupID = value; }
            get { return _groupID; }
        }

        public string WaveID
        {
            set { _waveID = value; }
            get { return _waveID; }
        }

        public string BasketID
        {
            set { _basketID = value; }
            get { return _basketID; }
        }

        string _userID = string.Empty;
        public string UserID
        {
            set { _userID = value; }
            get { return _userID; }
        }

        public void AddMessage(FIXMessage fixMessage)
        {
            _fixMessages.Add(fixMessage);
        }

        public List<FIXMessage> ListMessages
        {
            get { return _fixMessages; }
        }
    }

    [Serializable]
    public class MessageField
    {
        public MessageField()
        { }
        public MessageField(string tag, string value)
        {
            _tag = tag;
            _value = value;
        }

        public MessageField(string tagValuePair)
        {
            string[] temp = tagValuePair.Split('=');
            _tag = temp[0];
            if (temp.Length > 2)
            {
                _value = tagValuePair.Substring(tagValuePair.IndexOf('=') + 1);
            }
            else
            {
                _value = temp[1];
            }
        }

        private string _tag;
        public string Tag
        {
            get { return _tag; }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private bool _isRepeatingGroup;
        public bool IsRepeatingGroup
        {
            get { return _isRepeatingGroup; }
            set { _isRepeatingGroup = value; }
        }

        public override string ToString()
        {
            return _tag + "=" + _value;
        }

        public string getString()
        {
            return _tag + "=" + _value + Seperators.DELIMITER;
        }
    }

    [Serializable]
    public class MessageFieldCollection
    {
        private readonly object lockcoll = new object();

        public MessageFieldCollection()
        { 
        }

        public MessageFieldCollection(String msg)
        {
            lock (lockcoll)
            {
                _messageFieldColl = new Dictionary<string, MessageField>();
                string[] externList = msg.Split(Seperators.DELIMITER);
                foreach (string tagValuePair in externList)
                {

                    string trimmedFieldData = tagValuePair.Trim();
                    if (!trimmedFieldData.Equals(string.Empty))
                    {
                        MessageField field = new MessageField(trimmedFieldData);
                        _messageFieldColl.Add(field.Tag, field);
                    }
                }
            }
        }

        public MessageField this[string key]
        {
            get
            {
                if (_messageFieldColl.ContainsKey(key))
                {
                    return _messageFieldColl[key];
                }
                return null;
            }
        }

        /// <summary>
        /// To fetch value of specific tab in MessageField
        /// use indexer otherwise
        /// </summary>
        /// <param name="key">Tag to be found</param>
        /// <returns>Collection of MessageField for given tag</returns>
        public MessageField GetAllMessageFieldForTag(string key)
        {
            if (_messageFieldColl.ContainsKey(key))
            {
                return _messageFieldColl[key];
            }
            return null;
        }

        Dictionary<string, MessageField> _messageFieldColl = new Dictionary<string, MessageField>();
        public MessageFieldCollection(List<MessageField> collection)
        {
            lock (lockcoll)
            {
                foreach (MessageField messageField in collection)
                {
                    _messageFieldColl.Add(messageField.Tag, new MessageField(messageField.Tag,messageField.Value));
                }
            }
        }

        public MessageFieldCollection(Dictionary<string, MessageField> messageFieldColl)
        {
            lock (lockcoll)
            {
                foreach (KeyValuePair<string, MessageField> item in messageFieldColl)
                {
                    _messageFieldColl.Add(item.Key, new MessageField(item.Key, item.Value.Value));
                }
            }
        }

        public void UpdateCollection(List<MessageField> collection)
        {
            // Create a new dictionary to hold the updated collection
            Dictionary<string, MessageField> updatedColl = new Dictionary<string, MessageField>();
            foreach (MessageField messageField in collection)
            {
                updatedColl.Add(messageField.Tag, messageField);
            }
            lock (lockcoll)
            {
                _messageFieldColl = updatedColl;
            }
        }

        public void AddFields(string[] tagValuePairs)
        {
            foreach (string item in tagValuePairs)
            {
                if (item != null && item != string.Empty)
                {
                    string[] itemArray = item.Split('=');

                    if (itemArray.Length > 2)
                    {
                        AddField(itemArray[0], item.Substring(item.IndexOf('=') + 1));
                    }
                    else
                    {
                        AddField(itemArray[0], itemArray[1]);
                    }
                }
            }
        }

        public void AddField(string tag, string value)
        {
            try
            {
                if (!_messageFieldColl.ContainsKey(tag))
                {
                    lock (lockcoll)
                    {
                        _messageFieldColl.Add(tag, new MessageField(tag, value));
                    }
                }
                else
                {
                    _messageFieldColl[tag].Value = value;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public string GetField(string tag)
        {
            return _messageFieldColl[tag].Value;
        }

        public Int64 GetInt64Field(string tag)
        {
            return Int64.Parse(_messageFieldColl[tag].Value);
        }

        public string GetFieldWithValue(string tag)
        {
            MessageField field = _messageFieldColl[tag];
            return field.getString();
        }

        public void RemoveField(string tag)
        {
            lock (lockcoll)
            {
                _messageFieldColl.Remove(tag);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            lock (lockcoll)
            {
                foreach (KeyValuePair<string, MessageField> item in _messageFieldColl)
                {
                    sb.Append(item.Value.ToString());
                    sb.Append(Seperators.DELIMITER);
                }
            }
            return sb.ToString();
        }

        public List<MessageField> MessageFields
        {
            get
            {
                List<MessageField> messageList = new List<MessageField>();
                lock (lockcoll)
                {
                    foreach (KeyValuePair<string, MessageField> item in _messageFieldColl)
                    {
                        messageList.Add(item.Value);
                    }
                }
                return messageList;
            }
        }

        public MessageFieldCollection Clone()
        {
            MessageFieldCollection newMessageFieldCollection = new MessageFieldCollection(_messageFieldColl);
            return newMessageFieldCollection;
        }

        public bool ContainsKey(string tag)
        {
            return _messageFieldColl.ContainsKey(tag);
        }
    }

    [Serializable]
    public class RepeatingMessageFieldCollection : MessageFieldCollection
    {
        public Guid ID { get; set; }

        public RepeatingMessageFieldCollection()
        {
            ID = Guid.NewGuid();
        }
    }
}