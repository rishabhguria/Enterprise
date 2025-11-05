using AdapterClient;
using AdapterClient.FIX;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Prana.FixAdapters
{
    public class CameronFixAdapter : IFixEngineAdapter
    {
        bool _loggingEnabled = false;
        internal IAdapterClient client = null;
        FixPartyDetails _fixPartyDetails;
        public event EventHandler<EventArgs<FixPartyDetails>> FixConnectionEvent;
        public event EventHandler<EventArgs<PranaMessage>> MessageReceivedEvent;
        private IQueueProcessor _cpsentQueue;
        private IQueueProcessor _cpReceivedQueue;

        #region message format Conversion Methods
        private bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));
        BufferBlock<Message> dataBuffer;
        public Message CreateFixMessage(PranaMessage PranaMsg)
        {
            try
            {
                Message fixMessage = new Message();
                string msgType = PranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value;
                fixMessage.SetField(FIXConstants.TagMsgType, PranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value);

                foreach (string tag in PranaMsg.FIXMessage.RequiredFixFields)
                {
                    if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(tag))
                    {
                        fixMessage.SetField(tag, PranaMsg.FIXMessage.ExternalInformation[tag].Value);
                    }
                }

                foreach (string tag in PranaMsg.FIXMessage.OptionalFixFields)
                {
                    if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(tag))
                    {
                        fixMessage.SetField(tag, PranaMsg.FIXMessage.ExternalInformation[tag].Value);
                    }
                }

                foreach (MessageField messageField in PranaMsg.FIXMessage.CustomInformation.MessageFields)
                {
                    fixMessage.SetField(messageField.Tag, messageField.Value);
                }

                if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSide))
                {
                    CreateOpenCloseTags(fixMessage);
                }

                if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTransactTime))
                {
                    fixMessage.SetField(FIXConstants.TagTransactTime, DateTimeConstants.GetCurrentTimeInFixFormat());
                }

                if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExpireTime))
                {
                    if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExpireTime) && PranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTimeInForce))
                    {
                        if (PranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AUECID))
                        {
                            int auecID = int.Parse(PranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECID].Value);

                            string expireTime = PranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagExpireTime].Value.ToString();
                            if (PranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagTimeInForce].Value.Equals(FIXConstants.TIF_GTD) && !string.IsNullOrEmpty(expireTime) && !expireTime.Equals("N/A"))
                            {
                                DateTime dateFormatted = Convert.ToDateTime(expireTime);
                                Prana.BusinessObjects.TimeZone auecTimeZone = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAUECTimeZone(auecID);
                                DateTime utcDate = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(dateFormatted, auecTimeZone);
                                fixMessage.SetField(FIXConstants.TagExpireTime, utcDate.ToString(DateTimeConstants.NirvanaDateTimeFormat));
                            }
                            else if (string.IsNullOrEmpty(expireTime) || expireTime.Equals("N/A"))
                            {
                                fixMessage.RemoveField(FIXConstants.TagExpireTime);
                            }
                        }
                    }
                }
                if (PranaMsg.MessageType == FIXConstants.MSGAllocation)
                {
                    foreach (RepeatingGroup group in PranaMsg.FIXMessage.ChildGroups.Values)
                    {
                        AddRepeatingGroupToFixMessage(fixMessage, group);
                    }
                }
                if (PranaMsg.MessageType == FIXConstants.MSGConfirmationAck)
                {
                    fixMessage.RemoveField(FIXConstants.TagAllocID);
                }
                return fixMessage;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        private static void AddRepeatingGroupToFixMessage(Message fixMessage, RepeatingGroup group)
        {
            try
            {
                if (group.CountField == null) return;
                fixMessage.SetFieldAllowingDuplicates(group.CountField.Tag, group.CountField.Value);
                foreach (var collection in group.MessageFields)
                {
                    foreach (MessageField field in collection.MessageFields)
                    {
                        if (field.IsRepeatingGroup && int.Parse(field.Value) > 0)
                        {
                            var collectionIdentifier = collection.ID.ToString();
                            if (group.ChildGroups.ContainsKey(collectionIdentifier) && group.ChildGroups[collectionIdentifier].ContainsKey(field.Tag))
                            {
                                AddRepeatingGroupToFixMessage(fixMessage, group.ChildGroups[collectionIdentifier][field.Tag]);
                            }
                        }
                        else
                        {
                            string tag = GetTagValue(field.Tag);
                            fixMessage.SetFieldAllowingDuplicates(tag, field.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private static string GetTagValue(string tag)
        {
            switch (tag)
            {
                case CustomFIXConstants.CUST_TAG_PartyID1:
                case CustomFIXConstants.CUST_TAG_PartyID2:
                case CustomFIXConstants.CUST_TAG_PartyID3:
                case CustomFIXConstants.CUST_TAG_PartyID4:
                case CustomFIXConstants.CUST_TAG_PartyID5:
                    return FIXConstants.TagPartyID;
                case CustomFIXConstants.CUST_TAG_PartyIDSource1:
                case CustomFIXConstants.CUST_TAG_PartyIDSource2:
                case CustomFIXConstants.CUST_TAG_PartyIDSource3:
                case CustomFIXConstants.CUST_TAG_PartyIDSource4:
                case CustomFIXConstants.CUST_TAG_PartyIDSource5:
                    return FIXConstants.TagPartyIDSource;
                case CustomFIXConstants.CUST_TAG_PartyRole1:
                case CustomFIXConstants.CUST_TAG_PartyRole2:
                case CustomFIXConstants.CUST_TAG_PartyRole3:
                case CustomFIXConstants.CUST_TAG_PartyRole4:
                case CustomFIXConstants.CUST_TAG_PartyRole5:
                    return FIXConstants.TagPartyRole;
                case CustomFIXConstants.CUST_TAG_NestedPartyID1:
                case CustomFIXConstants.CUST_TAG_NestedPartyID2:
                case CustomFIXConstants.CUST_TAG_NestedPartyID3:
                case CustomFIXConstants.CUST_TAG_NestedPartyID4:
                case CustomFIXConstants.CUST_TAG_NestedPartyID5:
                    return FIXConstants.TagNestedPartyID;
                case CustomFIXConstants.CUST_TAG_NestedPartyIDSource1:
                case CustomFIXConstants.CUST_TAG_NestedPartyIDSource2:
                case CustomFIXConstants.CUST_TAG_NestedPartyIDSource3:
                case CustomFIXConstants.CUST_TAG_NestedPartyIDSource4:
                case CustomFIXConstants.CUST_TAG_NestedPartyIDSource5:
                    return FIXConstants.TagNestedPartyIDSource;
                case CustomFIXConstants.CUST_TAG_NestedPartyRole1:
                case CustomFIXConstants.CUST_TAG_NestedPartyRole2:
                case CustomFIXConstants.CUST_TAG_NestedPartyRole3:
                case CustomFIXConstants.CUST_TAG_NestedPartyRole4:
                case CustomFIXConstants.CUST_TAG_NestedPartyRole5:
                    return FIXConstants.TagNestedPartyRole;
                case CustomFIXConstants.CUST_TAG_MiscFeeAmt1:
                case CustomFIXConstants.CUST_TAG_MiscFeeAmt2:
                case CustomFIXConstants.CUST_TAG_MiscFeeAmt3:
                case CustomFIXConstants.CUST_TAG_MiscFeeAmt4:
                case CustomFIXConstants.CUST_TAG_MiscFeeAmt5:
                case CustomFIXConstants.CUST_TAG_MiscFeeAmt6:
                case CustomFIXConstants.CUST_TAG_MiscFeeAmt7:
                case CustomFIXConstants.CUST_TAG_MiscFeeAmt8:
                case CustomFIXConstants.CUST_TAG_MiscFeeAmt9:
                    return FIXConstants.TagMiscFeeAmt;
                case CustomFIXConstants.CUST_TAG_MiscFeeCurr1:
                case CustomFIXConstants.CUST_TAG_MiscFeeCurr2:
                case CustomFIXConstants.CUST_TAG_MiscFeeCurr3:
                case CustomFIXConstants.CUST_TAG_MiscFeeCurr4:
                case CustomFIXConstants.CUST_TAG_MiscFeeCurr5:
                case CustomFIXConstants.CUST_TAG_MiscFeeCurr6:
                case CustomFIXConstants.CUST_TAG_MiscFeeCurr7:
                case CustomFIXConstants.CUST_TAG_MiscFeeCurr8:
                case CustomFIXConstants.CUST_TAG_MiscFeeCurr9:
                    return FIXConstants.TagMiscFeeCurr;
                case CustomFIXConstants.CUST_TAG_MiscFeeType1:
                case CustomFIXConstants.CUST_TAG_MiscFeeType2:
                case CustomFIXConstants.CUST_TAG_MiscFeeType3:
                case CustomFIXConstants.CUST_TAG_MiscFeeType4:
                case CustomFIXConstants.CUST_TAG_MiscFeeType5:
                case CustomFIXConstants.CUST_TAG_MiscFeeType6:
                case CustomFIXConstants.CUST_TAG_MiscFeeType7:
                case CustomFIXConstants.CUST_TAG_MiscFeeType8:
                case CustomFIXConstants.CUST_TAG_MiscFeeType9:
                    return FIXConstants.TagMiscFeeType;
                case CustomFIXConstants.CUST_TAG_MiscFeeBasis1:
                case CustomFIXConstants.CUST_TAG_MiscFeeBasis2:
                case CustomFIXConstants.CUST_TAG_MiscFeeBasis3:
                case CustomFIXConstants.CUST_TAG_MiscFeeBasis4:
                case CustomFIXConstants.CUST_TAG_MiscFeeBasis5:
                case CustomFIXConstants.CUST_TAG_MiscFeeBasis6:
                case CustomFIXConstants.CUST_TAG_MiscFeeBasis7:
                case CustomFIXConstants.CUST_TAG_MiscFeeBasis8:
                case CustomFIXConstants.CUST_TAG_MiscFeeBasis9:
                    return FIXConstants.TagMiscFeeBasis;
                default:
                    return tag;
            }
        }

        public PranaMessage CreatePranaMessage(Message fixMessage)
        {
            PranaMessage pranaMessage = new PranaMessage();
            foreach (Field field in fixMessage.Fields)
            {
                pranaMessage.FIXMessage.ExternalInformation.AddField(field.Name, field.Value);
            }
            pranaMessage.MessageType = fixMessage[FIXConstants.TagMsgType].Value;
            pranaMessage.CreateSideFromOpenCloseTags();

            if(pranaMessage.MessageType == FIXConstants.MSGAllocationACK
                || pranaMessage.MessageType == FIXConstants.MSGAllocationReport
                || pranaMessage.MessageType == FIXConstants.MSGConfirmation)
            {
                //Get the available repeating groups for current message
                var repeatingGroups = GetRepeatingGroupForMessage(pranaMessage.MessageType);
                if (repeatingGroups != null && repeatingGroups.Count > 0)
                {
                    foreach (var repeatingIdentifierTag in repeatingGroups.Keys)
                    {
                        if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(repeatingIdentifierTag)
                            && int.Parse(pranaMessage.FIXMessage.ExternalInformation[repeatingIdentifierTag].Value) > 0)
                        {
                            RepeatingGroup childGroup = null;
                            int index = 0;
                            UpdateChildCollectionFromFixMessage(pranaMessage, ref childGroup, fixMessage, repeatingIdentifierTag, repeatingGroups[repeatingIdentifierTag].RepeatingFixFields, ref index);
                            if(childGroup != null && childGroup.MessageFields.Count > 0)
                            {
                                pranaMessage.FIXMessage.ChildGroups.Add(repeatingIdentifierTag, childGroup);
                            }
                        }
                    }
                }
            }
            
            return pranaMessage;
        }

        private void UpdateChildCollectionFromFixMessage(PranaMessage pranaMessage, ref RepeatingGroup repeatingGroup, Message fixMessage, string countTagName, Dictionary<string, RepeatingFixField> childTags, ref int index)
        {
            try
            {
                while (index < fixMessage.Fields.Count)
                {
                    Field field = (Field)fixMessage.Fields[index];
                    if (field.Name == countTagName)
                        break;
                    index++;
                }
                if (index < fixMessage.Fields.Count)
                {
                    RepeatingMessageFieldCollection currentRepeatingGroupFields = new RepeatingMessageFieldCollection();
                    repeatingGroup = new RepeatingGroup(new MessageField(countTagName, ((Field)fixMessage.Fields[index]).Value));
                    repeatingGroup.CountField.IsRepeatingGroup = true;
                    pranaMessage.FIXMessage.ExternalInformation.RemoveField(countTagName);
                    repeatingGroup.MessageFields.Add(currentRepeatingGroupFields);
                    index++;
                    for (; index < fixMessage.Fields.Count; index++)
                    {
                        Field field = (Field)fixMessage.Fields[index];
                        if (childTags.ContainsKey(field.Name))
                        {
                            if (currentRepeatingGroupFields.ContainsKey(field.Name))
                            {
                                currentRepeatingGroupFields = new RepeatingMessageFieldCollection();
                                repeatingGroup.MessageFields.Add(currentRepeatingGroupFields);
                            }
                            if (childTags[field.Name].IsRepeatingTag)
                            {
                                currentRepeatingGroupFields.AddField(field.Name, field.Value);
                                pranaMessage.FIXMessage.ExternalInformation.RemoveField(field.Name);
                                if (int.Parse(field.Value) > 0)
                                {
                                    currentRepeatingGroupFields[field.Name].IsRepeatingGroup = true;
                                    RepeatingGroup nestedRepeatingGroup = null;
                                    UpdateChildCollectionFromFixMessage(pranaMessage, ref nestedRepeatingGroup, fixMessage, field.Name, childTags[field.Name].RepeatingFixFields, ref index);
                                    index--;
                                    if(nestedRepeatingGroup != null && nestedRepeatingGroup.MessageFields.Count > 0)
                                    {
                                        if (!repeatingGroup.ChildGroups.ContainsKey(currentRepeatingGroupFields.ID.ToString()))
                                        {
                                            repeatingGroup.ChildGroups.Add(currentRepeatingGroupFields.ID.ToString(), new SerializableDictionary<string, RepeatingGroup>());
                                        }
                                        repeatingGroup.ChildGroups[currentRepeatingGroupFields.ID.ToString()].Add(field.Name, nestedRepeatingGroup);
                                    }
                                }
                            }
                            currentRepeatingGroupFields.AddField(field.Name, field.Value);
                            pranaMessage.FIXMessage.ExternalInformation.RemoveField(field.Name);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        
        private void CreateOpenCloseTags(Message fixMessage)
        {
            switch (fixMessage[FIXConstants.TagSide].Value)
            {
                case FIXConstants.SIDE_Buy_Open:
                    fixMessage.SetField(FIXConstants.TagSide, FIXConstants.SIDE_Buy);
                    fixMessage.SetField(FIXConstants.TagOpenClose, FIXConstants.Open);
                    break;
                case FIXConstants.SIDE_Buy_Closed:
                    fixMessage.SetField(FIXConstants.TagSide, FIXConstants.SIDE_Buy);
                    fixMessage.SetField(FIXConstants.TagOpenClose, FIXConstants.Close);
                    break;
                case FIXConstants.SIDE_Sell_Open:
                    fixMessage.SetField(FIXConstants.TagSide, FIXConstants.SIDE_Sell);
                    fixMessage.SetField(FIXConstants.TagOpenClose, FIXConstants.Open);
                    break;
                case FIXConstants.SIDE_Sell_Closed:
                    fixMessage.SetField(FIXConstants.TagSide, FIXConstants.SIDE_Sell);
                    fixMessage.SetField(FIXConstants.TagOpenClose, FIXConstants.Close);
                    break;
            }
        }

        public PranaMessage CreatePranaMessageFromFixMessage(string message)
        {
            RawTransformer rt = new RawTransformer();
            Message msg = rt.toMessage(message);
            return CreatePranaMessage(msg);
        }
        #endregion

        #region IFixEngineAdapter methods
        public void SetUp(IQueueProcessor cpReceivedQueue, IQueueProcessor cpsentQueue)
        {
            _loggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LOGGINGENABLED"]);
            _cpReceivedQueue = cpReceivedQueue;
            _cpsentQueue = cpsentQueue;
            dataBuffer = new BufferBlock<Message>();
            System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeBufferMessageAsync(dataBuffer)).ConfigureAwait(false);
        }

        public void Connect(FixPartyDetails fixPartyDetails)
        {
            try
            {
                _fixPartyDetails = fixPartyDetails;
                AdapterProperties props = new AdapterProperties();

                props[AdapterProperties.ARG_HOSTNAME] = _fixPartyDetails.HostName;
                props[AdapterProperties.ARG_PORT] = _fixPartyDetails.Port;
                props[AdapterProperties.ARG_USE_ACKS] = _fixPartyDetails.UseAcknowledge;
                props[AdapterProperties.ARG_ACK_TIMEOUT] = _fixPartyDetails.AcknowledgeTime;
                if (client == null)
                {
                    client = new SocketClient(props);

                    try
                    {
                        client.MessageEvent += new AdapterClient.MessageEventHandler(IncomingMessage);
                        client.ExceptionEvent += new AdapterClient.ExceptionEventHandler(IncomingException);
                        client.ConnectionEvent += new AdapterClient.ConnectionEventHandler(client_ConnectionEvent);
                        client.Connect();
                    }
                    catch (Exception)
                    {
                        _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                    }

                    if (client.Connected)
                    {

                        _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                        _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;

                        if (FixConnectionEvent != null)
                        {
                            FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
                        }
                    }
                    else
                    {
                        client = null;
                        if (FixConnectionEvent != null)
                        {
                            FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
                        }
                    }
                }
                else
                {
                    Logger.HandleException(new Exception("Already Connected:=" + _fixPartyDetails.PartyName), LoggingConstants.POLICY_LOGANDSHOW);
                }
            }
            catch (Exception ex)
            {

                Exception ex1 = new Exception("For CounterParty : " + _fixPartyDetails.PartyName + " Exception =" + ex.Message);
                Logger.HandleException(ex1, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public void SendMessage(PranaMessage pranaMsg)
        {
            try
            {
                Message message = CreateFixMessage(pranaMsg);
                message.SetField(Constants.TAGTargetCompID, _fixPartyDetails.TargetCompID);
                message.SetField(Constants.TAGSenderCompID, _fixPartyDetails.SenderCompID);
                client.SendMessage(message);

                if (_loggingEnabled)
                {
                    QueueMessage qMsg = new QueueMessage("String", "", "", message.ToString());
                    _cpsentQueue.SendMessage(qMsg);
                }

                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow Out5] Trade Goes Out To Broker In CameronFixAdapter, Fix Message: " + Convert.ToString(pranaMsg.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
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

        async System.Threading.Tasks.Task<Message> ConsumeBufferMessageAsync(IReceivableSourceBlock<Message> source)
        {
            try
            {
                while (await source.OutputAvailableAsync())
                {
                    Message message;
                    while (source.TryReceive(out message))
                    {
                        PranaMessage pranaMsg = CreatePranaMessage(message);
                        if (_enableTradeFlowLogging)
                        {
                            try
                            {
                                Logger.LoggerWrite("[Trade-Flow2] Before Consume Buffer In CameronFixAdapter, Fix Message: " + Convert.ToString(pranaMsg.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                            }
                        }

                        if (MessageReceivedEvent != null)
                        {
                            MessageReceivedEvent(null, new EventArgs<PranaMessage>(pranaMsg));
                        }

                        if (_enableTradeFlowLogging)
                        {
                            try
                            {
                                Logger.LoggerWrite("[Trade-Flow2] After Consume Buffer In CameronFixAdapter, Fix Message: " + Convert.ToString(pranaMsg.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                            }
                        }
                    }
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

            return null;
        }

        void BufferMessage(ITargetBlock<Message> target, Message message)
        {
            try
            {
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow1] Before Post Buffer In CameronFixAdapter, Fix Message: " + Convert.ToString(message), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                    }
                }
                target.Post(message);
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow1] After Post Buffer In CameronFixAdapter, Fix Message: " + Convert.ToString(message), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                    }
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

        private void IncomingMessage(object sender, AdapterClient.MessageEventArgs args)
        {
            Message msg = args.FixMessage;
            try
            {
                if (msg[AdapterClient.FIX.Constants.TAGMsgType].Value == AdapterClient.FIX.Constants.MSGLogon)
                {
                    HandleLogOnMessage();
                    Logger.HandleException(new Exception("CounterParty " + _fixPartyDetails.PartyName + " Connected"), LoggingConstants.POLICY_LOGANDSHOW);
                }
                else if (msg[AdapterClient.FIX.Constants.TAGMsgType].Value == AdapterClient.FIX.Constants.MSGLogout)
                {
                    HandleLogOutMessage();
                    Logger.HandleException(new Exception("CounterParty " + _fixPartyDetails.PartyName + " DisConnected"), LoggingConstants.POLICY_LOGANDSHOW);
                }
                else
                {
                    BufferMessage(dataBuffer, msg);
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

        private void IncomingException(object sender, AdapterClient.ExceptionEventArgs args)
        {
            try
            {
                if ((client != null && !client.Connected))
                {
                    Disconnect();
                }
                string onMsg = string.Empty;
                if (args.OnMessage != null)
                {
                    onMsg = ". Exception On Message =" + args.OnMessage.ToString();
                }
                Logger.HandleException(new Exception("For CounterParty : " + _fixPartyDetails.PartyName + "  Exception = " + args.RaisedException.Message + onMsg, args.RaisedException), LoggingConstants.POLICY_LOGANDSHOW);


                if (args.RaisedException.Message == "java.io.IOException: Not logged onto Fix session")
                {
                    _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        void client_ConnectionEvent(object sender, ConnectionEventArgs args)
        {
            try
            {
                SocketClient socketClient = (SocketClient)sender;
                if (socketClient.Connected)
                {
                    _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                }
                else
                {
                    _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                }
                if (args.Status.ToUpper().Contains("CONNECTED") && !args.Status.ToUpper().Contains("FALSE") && !args.Status.ToUpper().Contains("DISCONNECTED"))
                {
                    _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                }
                else
                {
                    _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                }
                if (FixConnectionEvent != null)
                {
                    FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
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

        public void ReProcessMessage(PranaMessage pranaMsg)
        {
            if (MessageReceivedEvent != null)
            {
                MessageReceivedEvent(null, new EventArgs<PranaMessage>(pranaMsg));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposable)
        {

            if (client != null)
            {
                try
                {
                    client.Disconnect();

                    int retries = 0;
                    while (client.Connected && retries < 10)
                    {
                        Thread.Sleep(1000);
                        retries++;
                    }
                    Environment.Exit(0);

                }
                catch (Exception ex)
                {
                    Logger.HandleException(new Exception("Unable to terminate connection: " + ex.ToString() + ex.StackTrace), LoggingConstants.POLICY_LOGANDSHOW);
                }
            }
        }

        public void Disconnect()
        {
            try
            {
                if (client != null)
                {
                    try
                    {
                        client.Disconnect();
                        while (client.Connected)
                        {
                            Thread.Sleep(1000);
                        }
                        client = null;
                        _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                        _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                        if (FixConnectionEvent != null)
                        {
                            FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
                        }
                    }
                    catch (Exception ex)
                    {
                        client = null;
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                        if (rethrow)
                        {
                            throw;
                        }
                    }
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
        #endregion

        public void HandleLogOnMessage()
        {
            try
            {
                _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                if (FixConnectionEvent != null)
                {
                    FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
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

        public void HandleLogOutMessage()
        {
            _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
            if (FixConnectionEvent != null)
            {
                FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
            }
        }

        /// <summary>
        /// This method is for fetching repeating groups for particular message
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns>Repeating Groups available for that message</returns>
        private Dictionary<string, RepeatingFixField> GetRepeatingGroupForMessage(string messageType)
        {
            try
            {
                if (FixDictionaryHelper.RepeatingGroupDictionary.ContainsKey(messageType))
                {
                    return FixDictionaryHelper.RepeatingGroupDictionary[messageType];
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
            return null;
        }
    }
}