using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using Prana.MessageProcessor;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Prana.AllocationProcessor;
using static Prana.Global.ApplicationConstants;

namespace Prana.ThirdPartyManager.BusinessLogic
{
    public class ThirdPartyFixManager
    {
        #region singleton
        private ThirdPartyFixManager() { }

        private static ThirdPartyFixManager instance = new ThirdPartyFixManager();
        public static ThirdPartyFixManager Instance
        {
            get { return instance; }
        }
        #endregion

        public void WireEvents()
        {
            PranaAllocationManager.SendAUMsg += SendAUMessage;
            PranaAllocationManager.SendATMsg += SendATMessage;
        }

        public bool SendbatchData(ThirdPartyBatch batch, DateTime runDate, EventHandler<StatusEventArgs> OnStatus)
        {
            try
            {
                if(OnStatus != null)
                    OnStatus(this, new StatusEventArgs(ThirdPartyConstants.STATUS_GENERATING_FIX_MESSAGES));

                DataSet dataSource = new DataSet();
                dataSource.ReadXml(new StringReader(batch.SerializedDataSet));
                if (dataSource.Tables.Count > 1)
                {
                    List<PranaMessage> pranaMessages = Transformer.CreatePranaMessages(dataSource.Tables[0], true);
                    if(OnStatus != null)
                        OnStatus(this, new StatusEventArgs(ThirdPartyConstants.STATUS_FIX_GENERATION_SUCCESSFUL));
                    Dictionary<string, List<MessageFieldCollection>> entityWiseTaxlots = GetAllocIdWiseMessageFields(dataSource);

                    foreach (PranaMessage msg in pranaMessages)
                    {
                        msg.MessageType = FIXConstants.MSGAllocation;
                        msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CounterPartyID, batch.CounterPartyID.ToString());
                        msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGAllocation);
                        msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagTransactTime, DateTimeConstants.GetCurrentTimeInFixFormat());
                        msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ThirdPartyBatchId, batch.ThirdPartyBatchId.ToString());
                        msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ThirdPartyRunDate, runDate.ToString());

                        if (batch.BrokerConnectionType == EnumHelper.GetDescriptionWithDescriptionAttribute(PranaServerConstants.BrokerConnectionType.SendOnly))
                        {
                            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_BrokerConnectionType, ((int)PranaServerConstants.BrokerConnectionType.SendOnly).ToString());
                        }
                        else if (batch.BrokerConnectionType == EnumHelper.GetDescriptionWithDescriptionAttribute(PranaServerConstants.BrokerConnectionType.SendAndConfirmBack))
                        {
                            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_BrokerConnectionType, ((int)PranaServerConstants.BrokerConnectionType.SendAndConfirmBack).ToString());
                        }

                        msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ThirdPartyJobName, batch.Description);
                        string entityId = msg.FIXMessage.ExternalInformation[FIXConstants.TagAllocID].Value;

                        if (!string.IsNullOrEmpty(entityId) && batch.OrderDetail.ContainsKey(entityId))
                        {
                            // Create a comma-separated string for OrderID
                            string orderIDString = string.Join(",", batch.OrderDetail[entityId].Select(detail => detail.OrderID));
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrderID, orderIDString);

                            // Create a comma-separated string for CLOrderID
                            string clOrderIDString = string.Join(",", batch.OrderDetail[entityId].Select(detail => detail.CLOrderID));
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, clOrderIDString);
                        }

                        if (!string.IsNullOrEmpty(msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CusipSymbol]?.Value))
                        {
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagIDSource, FIXConstants.SECURITYIDSOURCE_Cusip);
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSecurityID, msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CusipSymbol].Value);
                        }
                        else if (!string.IsNullOrEmpty(msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_SEDOLSymbol]?.Value))
                        {
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagIDSource, FIXConstants.SECURITYIDSOURCE_Sedol);
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSecurityID, msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_SEDOLSymbol].Value);
                        }
                        else if (!string.IsNullOrEmpty(msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ISINSymbol]?.Value))
                        {
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagIDSource, FIXConstants.SECURITYIDSOURCE_Isin);
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSecurityID, msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ISINSymbol].Value);
                        }
                        AddRepeatingGroups(entityWiseTaxlots, msg, entityId);

                        FixMessageValidator.ValidateMessage(msg);

                        MessageEngine.GetInstance()._dummyQueue_MessageQueued(msg);
                    }
                    return true;
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
            return false;
        }
   
        /// <summary>
        /// Adds the repeating groups.
        /// </summary>
        /// <param name="entityWiseTaxlots">The entity wise taxlots.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="entityId">The entity identifier.</param>
        private void AddRepeatingGroups(Dictionary<string, List<MessageFieldCollection>> entityWiseTaxlots, PranaMessage msg, string entityId)
        {
            try
            {
                MessageField countField = new MessageField(FIXConstants.TagNoAllocs, entityWiseTaxlots[entityId].Count.ToString());
                RepeatingGroup group = new RepeatingGroup(countField);
                foreach(var messageFieldCollection in entityWiseTaxlots[entityId])
                {
                    var newMessageFieldCollection = new RepeatingMessageFieldCollection();
                    foreach(var messageField in messageFieldCollection.MessageFields)
                    {
                        if(messageField.Tag == FIXConstants.TagNoMiscFees)
                        {
                            newMessageFieldCollection.AddField(messageField.Tag, messageField.Value);
                            newMessageFieldCollection[messageField.Tag].IsRepeatingGroup = true;
                            RepeatingGroup miscFeesGroup = new RepeatingGroup(new MessageField(FIXConstants.TagNoMiscFees, messageField.Value));
                            miscFeesGroup.MessageFields.Add(new RepeatingMessageFieldCollection());
                            var identifier = newMessageFieldCollection.ID.ToString();
                            if (!group.ChildGroups.ContainsKey(identifier))
                            {
                                group.ChildGroups.Add(identifier, new SerializableDictionary<string, RepeatingGroup>());
                            }
                            group.ChildGroups[identifier].Add(FIXConstants.TagNoMiscFees, miscFeesGroup);
                        }
                        else if(messageField.Tag == FIXConstants.TagMiscFeeAmt || messageField.Tag == FIXConstants.TagMiscFeeBasis
                            || messageField.Tag == FIXConstants.TagMiscFeeCurr || messageField.Tag == FIXConstants.TagMiscFeeType)
                        {
                            group.ChildGroups[newMessageFieldCollection.ID.ToString()][FIXConstants.TagNoMiscFees]
                                .MessageFields[0].AddField(messageField.Tag, messageField.Value);
                        }
                        else
                        {
                            newMessageFieldCollection.AddField(messageField.Tag, messageField.Value);
                        }
                    }
                    group.MessageFields.Add(newMessageFieldCollection);
                }
                msg.FIXMessage.ChildGroups.Add(FIXConstants.TagNoAllocs, group);

                if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagNoPartyIDs) && msg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PartyID1))
                {
                    MessageField partyCountField = msg.FIXMessage.ExternalInformation[FIXConstants.TagNoPartyIDs];
                    msg.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagNoPartyIDs);
                    RepeatingGroup partyGroup = new RepeatingGroup(partyCountField);

                    partyGroup.MessageFields = GetPartyGroupMessageFieldsCollection(msg);
                    msg.FIXMessage.ChildGroups.Add(FIXConstants.TagNoPartyIDs, partyGroup);
                }

                if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagNoOrders))
                {
                    MessageField ordCountField = msg.FIXMessage.ExternalInformation[FIXConstants.TagNoOrders];
                    msg.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagNoOrders);
                    RepeatingGroup ordGroup = new RepeatingGroup(ordCountField);

                    ordGroup.MessageFields = GetOrdGroupMessageFieldsCollection(msg);
                    ordGroup.CountField.Value = ordGroup.MessageFields.Count.ToString();
                    msg.FIXMessage.ChildGroups.Add(FIXConstants.TagNoOrders, ordGroup);
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

        /// <summary>
        /// Gets the ord group message fields collection.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        private List<RepeatingMessageFieldCollection> GetOrdGroupMessageFieldsCollection(PranaMessage msg)
        {
            List<RepeatingMessageFieldCollection> ordFieldsCollection = new List<RepeatingMessageFieldCollection>();
            try
            {
                int clOrdCount = 0;
                if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                {
                    string[] clOrderIDs = msg.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value.Split(',');
                    msg.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagClOrdID);
                    foreach (string clOrdID in clOrderIDs)
                    {
                        ordFieldsCollection.Add(new RepeatingMessageFieldCollection());
                        ordFieldsCollection[clOrdCount].AddField(FIXConstants.TagClOrdID, clOrdID);
                        clOrdCount++;
                    }
                }
                int ordCount = 0;
                if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID))
                {
                    string[] orderIDs = msg.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value.Split(',');
                    msg.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagOrderID);
                    foreach (string ordID in orderIDs)
                    {
                        if (ordCount >= clOrdCount)
                            ordFieldsCollection.Add(new RepeatingMessageFieldCollection());
                        ordFieldsCollection[ordCount].AddField(FIXConstants.TagOrderID, ordID);
                        ordCount++;
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
            return ordFieldsCollection;
        }

        /// <summary>
        /// Gets the party group message fields collection.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        private List<RepeatingMessageFieldCollection> GetPartyGroupMessageFieldsCollection(PranaMessage msg)
        {
            List<RepeatingMessageFieldCollection> partyFieldsCollection = new List<RepeatingMessageFieldCollection>();
            try
            {
                int partyCount = 0;
                foreach (MessageField field in msg.FIXMessage.InternalInformation.MessageFields)
                {
                    switch (field.Tag)
                    {

                        case CustomFIXConstants.CUST_TAG_PartyID1:
                        case CustomFIXConstants.CUST_TAG_PartyID2:
                        case CustomFIXConstants.CUST_TAG_PartyID3:
                        case CustomFIXConstants.CUST_TAG_PartyID4:
                        case CustomFIXConstants.CUST_TAG_PartyID5:
                            partyFieldsCollection.Add(new RepeatingMessageFieldCollection());
                            partyFieldsCollection[partyCount].AddField(field.Tag, field.Value);
                            partyCount++;
                            break;
                        case CustomFIXConstants.CUST_TAG_PartyIDSource1:
                        case CustomFIXConstants.CUST_TAG_PartyIDSource2:
                        case CustomFIXConstants.CUST_TAG_PartyIDSource3:
                        case CustomFIXConstants.CUST_TAG_PartyIDSource4:
                        case CustomFIXConstants.CUST_TAG_PartyIDSource5:
                            partyFieldsCollection[partyCount].AddField(field.Tag, field.Value);
                            break;
                        case CustomFIXConstants.CUST_TAG_PartyRole1:
                        case CustomFIXConstants.CUST_TAG_PartyRole2:
                        case CustomFIXConstants.CUST_TAG_PartyRole3:
                        case CustomFIXConstants.CUST_TAG_PartyRole4:
                        case CustomFIXConstants.CUST_TAG_PartyRole5:
                            partyFieldsCollection[partyCount].AddField(field.Tag, field.Value);
                            break;
                        default:
                            break;
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
            return partyFieldsCollection;
        }

        /// <summary>
        /// Gets the alloc identifier wise message fields.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        private static Dictionary<string, List<MessageFieldCollection>> GetAllocIdWiseMessageFields(DataSet dataSource)
        {
            Dictionary<string, List<MessageFieldCollection>> allocWiseTaxlots = new Dictionary<string, List<MessageFieldCollection>>();
            try
            {
                List<DataColumn> columns = dataSource.Tables[1].Columns.Cast<DataColumn>().ToList();

                foreach (DataRow row in dataSource.Tables[1].Rows)
                {
                    List<MessageField> messageFields = Transformer.GetMessageFieldsFromDataColoumnList(columns, row, true);
                    MessageFieldCollection fieldColl = new MessageFieldCollection(messageFields);
                    if (fieldColl.ContainsKey(FIXConstants.TagAllocID))
                    {
                        string allocId = fieldColl[FIXConstants.TagAllocID].Value;
                        fieldColl.RemoveField(FIXConstants.TagAllocID);
                        if (allocWiseTaxlots.ContainsKey(allocId))
                        {
                            allocWiseTaxlots[allocId].Add(fieldColl);
                        }
                        else
                            allocWiseTaxlots.Add(allocId, new List<MessageFieldCollection> { fieldColl });
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
            return allocWiseTaxlots;
        }

        /// <summary>
        /// This method sends AU message after setting appropriate tags
        /// </summary>
        /// <param name="messageFieldsByAllocId"></param>
        /// <param name="counterPartyID"></param>
        /// <param name="affirmStatus"></param>
        public static void SendAUMessage(Dictionary<string, List<RepeatingMessageFieldCollection>> messageFieldsByAllocId, int counterPartyID, int affirmStatus)
        {
            try
            {
                foreach (var allocId in messageFieldsByAllocId.Keys)
                {
                    foreach (RepeatingMessageFieldCollection messageFields in messageFieldsByAllocId[allocId])
                    {
                        var msg = new PranaMessage();
                        msg.MessageType = FIXConstants.MSGConfirmationAck;
                        msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CounterPartyID, counterPartyID.ToString());
                        msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGConfirmationAck);
                        msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagConfirmID, messageFields.GetField(FIXConstants.TagConfirmID));
                        msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagTradeDate, messageFields.GetField(FIXConstants.TagTradeDate));
                        msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagTransactTime, messageFields.GetField(FIXConstants.TagTransactTime));
                        msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAffirmStatus, affirmStatus.ToString());
                        if (affirmStatus == (int)AffirmStatus.Rejected)
                        {
                            if (messageFields.GetField(FIXConstants.TagConfirmStatus) == ConfirmStatus.MismatchedAccount.ToString())
                            {
                                msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagConfirmRejReason, ((int)ConfirmRejReason.MismatchedAccount).ToString());
                            }
                            else if (messageFields.GetField(FIXConstants.TagConfirmStatus) == ConfirmStatus.MissingSettlementInstructions.ToString())
                            {
                                msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagConfirmRejReason, ((int)ConfirmRejReason.MissingSettlementInstructions).ToString());
                            }
                            else
                            {
                                msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagConfirmRejReason, ((int)ConfirmRejReason.Other).ToString());
                            }
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.Uncompared).ToString());
                        }
                        else if (affirmStatus == (int)AffirmStatus.Received)
                        {
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.Uncompared).ToString());
                        }
                        else
                        {
                            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.Compared).ToString());
                        }
                        FixMessageValidator.ValidateMessage(msg);
                        msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAllocID, allocId);
                        MessageEngine.GetInstance()._dummyQueue_MessageQueued(msg);
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

        /// <summary>
        /// This method sends AT msg for specified allocId and allocStatus
        /// </summary>
        /// <param name="allocIdAllocReportIdPairs"></param>
        /// <param name="counterPartyID"></param>
        /// <param name="allocStatus"></param>
        public static void SendATMessage(Dictionary<string, string> allocIdAllocReportIdPairs, int counterPartyID, int allocStatus)
        {
            try
            {
                foreach (KeyValuePair<string, string> allocIdAllocReportIdPair in allocIdAllocReportIdPairs)
                {
                    var msg = new PranaMessage();
                    msg.MessageType = FIXConstants.MSGAllocationReportAck;
                    msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CounterPartyID, counterPartyID.ToString());
                    msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGAllocationReportAck);
                    msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAllocReportID, allocIdAllocReportIdPair.Value);
                    msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagTransactTime, DateTimeConstants.GetCurrentTimeInFixFormat());
                    msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAllocID, allocIdAllocReportIdPair.Key);
                    msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAllocStatus, allocStatus.ToString());
                    if (allocStatus == (int)BlockMatchStatus.Incomplete)
                    {
                        msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.AdvisoryOrAlert).ToString());
                    }
                    FixMessageValidator.ValidateMessage(msg);
                    MessageEngine.GetInstance()._dummyQueue_MessageQueued(msg);
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
    }
}