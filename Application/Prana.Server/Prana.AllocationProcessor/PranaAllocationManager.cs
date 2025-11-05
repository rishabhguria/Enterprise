using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.BusinessObjects.Enumerators;
using Prana.BusinessObjects.FIX;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static Prana.BusinessObjects.PranaServerConstants;
using static Prana.Global.ApplicationConstants;

namespace Prana.AllocationProcessor
{
    public static class PranaAllocationManager
    {
        public static Action<PranaMessage> NewEODMessageReceived;
        public static Action<Dictionary<string, List<RepeatingMessageFieldCollection>>, int, int> SendAUMsg;
        public static Action<Dictionary<string, string>, int, int> SendATMsg;
        /// <summary>
        /// This method is to save the block allocation details from Prana message
        /// </summary>
        /// <param name="pranaMessages"></param>
        public static void SaveBlockAllocationDetails(PranaMessage pranaMessage)
        {
            try
            {
                int thirdPartyBatchId = 0;
                DateTime runDate = DateTime.Today;
                int brokerConnectionType = 0;
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ThirdPartyBatchId))
                {
                    thirdPartyBatchId = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ThirdPartyBatchId].Value);
                }
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ThirdPartyRunDate))
                {
                    runDate = Convert.ToDateTime(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ThirdPartyRunDate].Value);
                }
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_BrokerConnectionType))
                {
                    brokerConnectionType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_BrokerConnectionType].Value);
                }
                var blockData = CreateBlockData(pranaMessage, brokerConnectionType);
                if (blockData != null)
                {
                    string allocId = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocID].Value;
                    string allocationData = string.Empty;
                    List<RepeatingMessageFieldCollection> messageFields = new List<RepeatingMessageFieldCollection>();
                    if(pranaMessage.FIXMessage.ChildGroups.ContainsKey(FIXConstants.TagNoAllocs))
                    {
                        RepeatingGroup taxlotGroup = pranaMessage.FIXMessage.ChildGroups[FIXConstants.TagNoAllocs];
                        if (taxlotGroup != null && taxlotGroup.CountField != null)
                        {
                            messageFields = DeepCopyHelper.Clone(taxlotGroup.MessageFields);
                            foreach (var messageFieldCollection in messageFields)
                            {
                                if (taxlotGroup.ChildGroups.ContainsKey(messageFieldCollection.ID.ToString()))
                                {
                                    var miscFeesGroup = taxlotGroup.ChildGroups[messageFieldCollection.ID.ToString()]["136"];
                                    messageFieldCollection.AddField(FIXConstants.TagMiscFeeAmt, miscFeesGroup.MessageFields[0][FIXConstants.TagMiscFeeAmt].Value);
                                }
                            }
                        }

                    }

                    if (pranaMessage.MessageType == FIXConstants.MSGConfirmation)
                    {
                        messageFields = CreateMessageFieldsForTaxlotLevelMsg(pranaMessage);
                        DataSet requiredData = GetDataToGenerateAUMsg(messageFields[0].GetField(FIXConstants.TagIndividualAllocID), allocId);
                        DataTable jMessage = requiredData.Tables[0];
                        DataTable toleranceProfile = requiredData.Tables[1];
                        int counterPartyId = Convert.ToInt32(requiredData.Tables[2].Rows[0][0]);
                        DateTime PMsgTransactTime = ((DateTime)requiredData.Tables[2].Rows[0][1]);
                        messageFields[0].AddField(FIXConstants.TagTransactTime, PMsgTransactTime.ToString(DateTimeConstants.NirvanaDateTimeFormat));
                        int affirmStatus = CompareWithJMsgAndGetAffirmStatus(messageFields, jMessage, toleranceProfile, blockData);
                        if (SendAUMsg != null)
                        {
                            Dictionary<string, List<RepeatingMessageFieldCollection>> messageFieldsByAllocId = new Dictionary<string, List<RepeatingMessageFieldCollection>>();
                            messageFieldsByAllocId.Add(allocId, messageFields);
                            SendAUMsg(messageFieldsByAllocId, counterPartyId, affirmStatus);
                        }
                    }
                    else if (pranaMessage.MessageType == FIXConstants.MSGConfirmationAck)
                    {
                        messageFields = CreateMessageFieldsForTaxlotLevelMsg(pranaMessage);
                    }
                    else if (pranaMessage.MessageType == FIXConstants.MSGAllocationReport)
                    {
                        int allocStatusASMsg = int.Parse(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocStatus].Value);
                        if (allocStatusASMsg != (int)BlockMatchStatus.Received)
                        {
                            DataSet requiredData = GetDataToGenerateATMsg(allocId);
                            DataTable correspondingJMessages = requiredData.Tables[0];
                            DataTable toleranceProfile = requiredData.Tables[1];
                            int counterPartyId = Convert.ToInt32(requiredData.Tables[2].Rows[0][0]);
                            int allocStatusATMsg;
                            if (allocStatusASMsg == (int)BlockMatchStatus.Accepted)
                            {
                                DataTable taxlotDetailsInASMsg = GetASMsgTaxlotDetails(messageFields);
                                allocStatusATMsg = CompareWithJMessagesAndGetAllocStatus(correspondingJMessages, taxlotDetailsInASMsg, toleranceProfile, messageFields);
                                blockData.MatchStatus = ((int)BlockMatchStatus.Accepted).ToString();
                            }
                            else
                            {
                                allocStatusATMsg = (int)BlockMatchStatus.Received;
                                blockData.MatchStatus = ((int)BlockMatchStatus.AccountLevelReject).ToString();
                                if (allocStatusASMsg == (int)BlockMatchStatus.BlockLevelReject)
                                {
                                    blockData.MatchStatus = ((int)BlockMatchStatus.BlockLevelReject).ToString();
                                    blockData.SubStatus = pranaMessage.FIXMessage.ExternalInformation.GetField(FIXConstants.TagAllocRejCode);
                                    messageFields.ForEach(messageField => messageField.AddField(FIXConstants.TagMatchStatus, pranaMessage.FIXMessage.ExternalInformation.GetField(FIXConstants.TagAllocRejCode)));
                                }
                                else if (allocStatusASMsg == (int)BlockMatchStatus.AccountLevelReject)
                                {
                                    blockData.SubStatus = ((int)BlockSubStatus.AccountLevelReject).ToString();
                                    messageFields.ForEach(messageField => messageField.AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.NA).ToString()));
                                }
                                else if (allocStatusASMsg == (int)BlockMatchStatus.Incomplete)
                                {
                                    blockData.SubStatus = ((int)BlockSubStatus.Incomplete).ToString();
                                    messageFields.ForEach(messageField => messageField.AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.NA).ToString()));
                                }
                                else
                                {
                                    blockData.SubStatus = ((int)BlockSubStatus.RejectedByIntermediary).ToString();
                                    messageFields.ForEach(messageField => messageField.AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.NA).ToString()));
                                }
                            }

                            if (SendATMsg != null)
                            {
                                var allocIdAllocReportIdPairs = new Dictionary<string, string>
                                {
                                    { allocId, pranaMessage.FIXMessage.ExternalInformation.GetField(FIXConstants.TagAllocReportID) }
                                };
                                if (allocStatusATMsg == (int)BlockMatchStatus.Accepted)
                                {
                                    SendATMsg(allocIdAllocReportIdPairs, counterPartyId, (int)BlockMatchStatus.Received);
                                }
                                SendATMsg(allocIdAllocReportIdPairs, counterPartyId, allocStatusATMsg);
                            }
                        }
                        else
                        {
                            blockData.MatchStatus = ((int)BlockMatchStatus.Received).ToString();
                            messageFields.ForEach(messageField => messageField.AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.NA).ToString()));
                        }
                    }
                    allocationData = CreateAllocationDataXml(messageFields, allocId, pranaMessage.MessageType, brokerConnectionType, int.Parse(blockData.MatchStatus));
                    if (allocationData != null)
                    {
                        var updatedRecord = SaveBlockAllocationDetails(thirdPartyBatchId, runDate, pranaMessage, brokerConnectionType, blockData, allocationData);
                        if(updatedRecord != null)
                        {
                            pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ThirdPartyBatchId, updatedRecord.Rows[0][0].ToString());
                            pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAllocStatus, updatedRecord.Rows[0][1].ToString());
                            if (NewEODMessageReceived != null)
                                NewEODMessageReceived(pranaMessage);
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

        private static List<RepeatingMessageFieldCollection> CreateMessageFieldsForTaxlotLevelMsg(PranaMessage pranaMessage)
        {
            List<RepeatingMessageFieldCollection> messageFields = new List<RepeatingMessageFieldCollection>();
            RepeatingMessageFieldCollection messageFieldCollection = new RepeatingMessageFieldCollection();
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAllocAccount))
            {
                messageFieldCollection.AddField(FIXConstants.TagAllocAccount, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocAccount].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAllocShares))
            {
                messageFieldCollection.AddField(FIXConstants.TagAllocShares, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocShares].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagIndividualAllocID))
            {
                messageFieldCollection.AddField(FIXConstants.TagIndividualAllocID, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagIndividualAllocID].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCommission))
            {
                messageFieldCollection.AddField(FIXConstants.TagCommission, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCommission].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAvgPx))
            {
                messageFieldCollection.AddField(FIXConstants.TagAvgPx, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagNetMoney))
            {
                messageFieldCollection.AddField(FIXConstants.TagNetMoney, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagNetMoney].Value);
            }
            if (pranaMessage.FIXMessage.ChildGroups.ContainsKey(FIXConstants.TagNoMiscFees))
            {
                messageFieldCollection.AddField(FIXConstants.TagMiscFeeAmt, pranaMessage.FIXMessage.ChildGroups[FIXConstants.TagNoMiscFees].MessageFields[0].GetField(FIXConstants.TagMiscFeeAmt));
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagText))
            {
                messageFieldCollection.AddField(FIXConstants.TagText, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagText].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTradeDate))
            {
                messageFieldCollection.AddField(FIXConstants.TagTradeDate, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagTradeDate].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagConfirmID))
            {
                messageFieldCollection.AddField(FIXConstants.TagConfirmID, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagConfirmID].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagConfirmRefID))
            {
                messageFieldCollection.AddField(FIXConstants.TagConfirmRefID, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagConfirmRefID].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagConfirmTransType))
            {
                messageFieldCollection.AddField(FIXConstants.TagConfirmTransType, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagConfirmTransType].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagConfirmType))
            {
                messageFieldCollection.AddField(FIXConstants.TagConfirmType, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagConfirmType].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagConfirmStatus))
            {
                messageFieldCollection.AddField(FIXConstants.TagConfirmStatus, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagConfirmStatus].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAffirmStatus))
            {
                messageFieldCollection.AddField(FIXConstants.TagAffirmStatus, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAffirmStatus].Value);
            }
            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagConfirmRejReason))
            {
                messageFieldCollection.AddField(FIXConstants.TagConfirmRejReason, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagConfirmRejReason].Value);
            }
            messageFields.Add(messageFieldCollection);
            return messageFields;
        }

        /// <summary>
        /// This method is to save block allocation details to DB
        /// </summary>
        /// <param name="thirdPartyBatchId"></param>
        /// <param name="rundDate"></param>
        /// <param name="messageType"></param>
        /// <param name="brokerConnectionType"></param>
        /// <param name="blockData"></param>
        /// <param name="allocationData"></param>
        private static DataTable SaveBlockAllocationDetails(int thirdPartyBatchId, DateTime rundDate, PranaMessage pranaMessage, int brokerConnectionType, ThirdPartyBlockLevelDetails blockData, string allocationData)
        {
            try
            {
                object[] parameter = new object[27];
                parameter[0] = string.IsNullOrEmpty(blockData.AveragePX) ? 0 : float.Parse(blockData.AveragePX);
                parameter[1] = blockData.Currency;
                parameter[2] = blockData.ISIN;
                parameter[3] = blockData.CUSIP;
                parameter[4] = blockData.Sedol;
                parameter[5] = string.IsNullOrEmpty(blockData.Quantity) ? 0 : float.Parse(blockData.Quantity);
                parameter[6] = blockData.Side;
                parameter[7] = blockData.Symbol;
                parameter[8] = DateTime.ParseExact(blockData.TransactionTime, DateTimeConstants.NirvanaDateTimeFormat, CultureInfo.InvariantCulture);
                parameter[9] = blockData.SettlementDate != null ? (DateTime?)DateTime.ParseExact(blockData.SettlementDate, new string[2] { DateTimeConstants.NirvanaDateTimeFormat_WithoutTime, DateTimeConstants.DateformatForClosing }, CultureInfo.InvariantCulture, DateTimeStyles.None) : null;
                parameter[10] = blockData.AllocationID;
                parameter[11] = blockData.AllocationTransactionType;
                if (blockData.TradeDate != null)
                    parameter[12] = DateTime.ParseExact(blockData.TradeDate, new string[2] { DateTimeConstants.NirvanaDateTimeFormat_WithoutTime, DateTimeConstants.DateformatForClosing }, CultureInfo.InvariantCulture, DateTimeStyles.None);
                parameter[13] = blockData.MatchStatus;
                parameter[14] = string.IsNullOrEmpty(blockData.NetAmount) ? 0 : float.Parse(blockData.NetAmount);
                parameter[15] = string.IsNullOrEmpty(blockData.Commission) ? 0 : float.Parse(blockData.Commission);
                parameter[16] = string.IsNullOrEmpty(blockData.GrossAmount) ? 0 : float.Parse(blockData.GrossAmount);
                parameter[17] = allocationData;
                parameter[18] = thirdPartyBatchId;
                parameter[19] = rundDate.ToString("yyyy-MM-dd");
                parameter[20] = brokerConnectionType;
                parameter[21] = pranaMessage.MessageType;
                parameter[22] = blockData.SubStatus;
                parameter[23] = blockData.Text;
                parameter[24] = blockData.AllocReportId;
                parameter[25] = DateTime.ParseExact(DateTimeConstants.GetCurrentTimeInFixFormat(), DateTimeConstants.NirvanaDateTimeFormat, CultureInfo.InvariantCulture);
                parameter[26] = CreateFixMessage(pranaMessage.FIXMessage);
                DataTable updatedRecord = new DataTable();
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_SaveThirdPartyBlockAllocations", parameter))
                {
                    updatedRecord.Load(reader);
                }
                return updatedRecord;
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

        /// <summary>
        /// Creates a FIX message string from the given FIXMessage object.
        /// </summary>
        /// <param name="msg">The FIXMessage object containing the message data.</param>
        /// <returns>A string representing the FIX message.</returns>
        private static string CreateFixMessage(FIXMessage msg)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                // Append external information
                sb.Append(msg.ExternalInformation.ToString());
                sb.Append(Seperators.SEPERATOR_3);

                // Append custom information
                sb.Append(msg.CustomInformation.ToString());
                sb.Append(Seperators.SEPERATOR_3);

                // Append all child groups
                foreach (RepeatingGroup group in msg.ChildGroups.Values)
                {
                    AddRepeatingGroup(sb, group);
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
            return sb.ToString();
        }

        /// <summary>
        /// Adds a repeating group to the StringBuilder.
        /// </summary>
        /// <param name="sb">The StringBuilder to append the group to.</param>
        /// <param name="group">The RepeatingGroup to be added.</param>
        private static void AddRepeatingGroup(StringBuilder sb, RepeatingGroup group)
        {
            try
            {
                // If the group has no count field, return
                if (group.CountField == null) return;

                // Append the count field value
                sb.Append(group.CountField.ToString());
                sb.Append(Seperators.DELIMITER);

                // Iterate through each collection of message fields
                foreach (var collection in group.MessageFields)
                {
                    // Iterate through each message field in the collection
                    foreach (MessageField field in collection.MessageFields)
                    {
                        // If the field is a repeating group and its value is greater than 0
                        if (field.IsRepeatingGroup && int.Parse(field.Value) > 0)
                        {
                            var collectionIdentifier = collection.ID.ToString();

                            // Check if the child group exists and add it recursively
                            if (group.ChildGroups.ContainsKey(collectionIdentifier) && group.ChildGroups[collectionIdentifier].ContainsKey(field.Tag))
                            {
                                AddRepeatingGroup(sb, group.ChildGroups[collectionIdentifier][field.Tag]);
                            }
                        }
                        else
                        {
                            // Append the count field value for non-repeating fields
                            sb.Append(field.ToString());
                            sb.Append(Seperators.DELIMITER);
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

        /// <summary>
        /// This method is to create Block level Data
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <returns>ThirdPartyBlockLevelDetails</returns>
        private static ThirdPartyBlockLevelDetails CreateBlockData(PranaMessage pranaMessage, int brokerConnectionType)
        {
            ThirdPartyBlockLevelDetails blockDetails = new ThirdPartyBlockLevelDetails();
            try
            {
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAvgPx))
                {
                    blockDetails.AveragePX = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCurrency))
                {
                    blockDetails.Currency = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCurrency].Value;
                }
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ISINSymbol))
                {
                    blockDetails.ISIN = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ISINSymbol].Value;
                }
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_SEDOLSymbol))
                {
                    blockDetails.Sedol = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_SEDOLSymbol].Value;
                }
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CusipSymbol))
                {
                    blockDetails.CUSIP = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CusipSymbol].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagShares))
                {
                    blockDetails.Quantity = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagShares].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSide))
                {
                    blockDetails.Side = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSide].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                {
                    blockDetails.Symbol = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTransactTime))
                {
                    blockDetails.TransactionTime = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagTransactTime].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagFutSettDate))
                {
                    blockDetails.SettlementDate = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagFutSettDate].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAllocID))
                {
                    blockDetails.AllocationID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocID].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAllocTransType))
                {
                    blockDetails.AllocationTransactionType = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocTransType].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTradeDate))
                {
                    blockDetails.TradeDate = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagTradeDate].Value;
                }
                if (pranaMessage.MessageType == FIXConstants.MSGAllocation)
                {
                    if (brokerConnectionType == (int)BrokerConnectionType.SendOnly)
                    {
                        blockDetails.MatchStatus = ((int)BlockMatchStatus.Accepted).ToString();
                        blockDetails.SubStatus = ((int)BlockSubStatus.ExactMatch).ToString();
                    }
                    else if (brokerConnectionType == (int)BrokerConnectionType.SendAndConfirmBack)
                    {
                        blockDetails.MatchStatus = ((int)BlockMatchStatus.Pending).ToString();
                        blockDetails.SubStatus = ((int)BlockSubStatus.SentToBroker).ToString();
                    }
                }
                else if (pranaMessage.MessageType == FIXConstants.MSGAllocationReportAck)
                {
                    blockDetails.MatchStatus = pranaMessage.FIXMessage.ExternalInformation.GetField(FIXConstants.TagAllocStatus).ToString();
                }

                if (pranaMessage.MessageType == FIXConstants.MSGAllocationACK)
                {
                    if (pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocStatus].Value.ToString() == ((int)BlockMatchStatus.Received).ToString())
                    {
                        blockDetails.SubStatus = ((int)BlockSubStatus.ReceiveByBroker).ToString();
                        blockDetails.MatchStatus = ((int)BlockMatchStatus.Pending).ToString();
                    }
                    else if (pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocStatus].Value.ToString() == ((int)BlockMatchStatus.BlockLevelReject).ToString())
                    {
                        blockDetails.SubStatus = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocRejCode].Value;
                        blockDetails.MatchStatus = ((int)BlockMatchStatus.BlockLevelReject).ToString();
                    }
                    else if (pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocStatus].Value.ToString() == ((int)BlockMatchStatus.Accepted).ToString())
                    {
                        blockDetails.SubStatus = ((int)BlockSubStatus.BlockAccepted).ToString();
                        blockDetails.MatchStatus = ((int)BlockMatchStatus.Pending).ToString();
                    }
                    else if (pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocStatus].Value.ToString() == ((int)BlockMatchStatus.AccountLevelReject).ToString())
                    {
                        blockDetails.SubStatus = pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAllocRejCode)
                            ? pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocRejCode].Value : ((int)BlockSubStatus.NA).ToString();
                        blockDetails.MatchStatus = ((int)BlockMatchStatus.MismatchedWithAllocRejCode).ToString();
                    }
                    else
                    {
                        blockDetails.SubStatus = pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAllocRejCode)
                            ? pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocRejCode].Value : ((int)AllocRejCode.NA).ToString();
                        blockDetails.MatchStatus = ((int)BlockMatchStatus.MismatchedWithAllocRejCode).ToString();
                    }
                }

                if (pranaMessage.MessageType == FIXConstants.MSGAllocationReport)
                {
                    blockDetails.MatchStatus = ((int)BlockMatchStatus.Pending).ToString();
                    blockDetails.SubStatus = ((int)BlockSubStatus.NA).ToString();
                }
                else if (pranaMessage.MessageType == FIXConstants.MSGConfirmation)
                {
                    blockDetails.MatchStatus = ((int)BlockMatchStatus.Pending).ToString();
                    blockDetails.SubStatus = ((int)BlockSubStatus.NA).ToString();
                }
                else if (pranaMessage.MessageType == FIXConstants.MSGConfirmationAck)
                {
                    blockDetails.MatchStatus = ((int)BlockMatchStatus.Pending).ToString();
                    blockDetails.SubStatus = ((int)BlockSubStatus.NA).ToString();
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagNetMoney))
                {
                    blockDetails.NetAmount = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagNetMoney].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCommission))
                {
                    blockDetails.Commission = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCommission].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagGrossTradeAmt))
                {
                    blockDetails.GrossAmount = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagGrossTradeAmt].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAllocReportID))
                {
                    blockDetails.AllocReportId = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAllocReportID].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagText))
                {
                    blockDetails.Text = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagText].Value;
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
            return blockDetails;
        }

        /// <summary>
        /// This method is to create Allocation Data in XML format
        /// </summary>
        /// <param name="messageFieldCollections"></param>
        /// <param name="allocId"></param>
        /// <param name="messageType"></param>
        /// <returns>Allocation DataTable in XML</returns>
        private static string CreateAllocationDataXml(List<RepeatingMessageFieldCollection> messageFieldCollections, string allocId, string messageType, int brokerConnectionType, int blockMatchStatus)
        {
            try
            {
                DataTable allocationData = new DataTable();
                allocationData.TableName = "Allocation";
                allocationData.Columns.Add("IndividualAllocID");
                allocationData.Columns.Add("AllocAccount");
                allocationData.Columns.Add("AllocQty");
                allocationData.Columns.Add("AllocAvgPx");
                allocationData.Columns.Add("Commission");
                allocationData.Columns.Add("MiscFeeAmt");
                allocationData.Columns.Add("AllocNetMoney");
                allocationData.Columns.Add("AllocID");
                allocationData.Columns.Add("MatchStatus");
                allocationData.Columns.Add("AllocText");
                allocationData.Columns.Add("TradeDate");
                allocationData.Columns.Add("ConfirmID");
                allocationData.Columns.Add("ConfirmRefID");
                allocationData.Columns.Add("ConfirmTransType");
                allocationData.Columns.Add("ConfirmType");
                allocationData.Columns.Add("ConfirmStatus");
                allocationData.Columns.Add("AffirmStatus");
                allocationData.Columns.Add("ConfirmRejReason");

                foreach (var messageField in messageFieldCollections)
                {
                    DataRow row = allocationData.NewRow();
                    if (messageField.ContainsKey(FIXConstants.TagIndividualAllocID))
                    {
                        row["IndividualAllocID"] = messageField[FIXConstants.TagIndividualAllocID].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagAllocAccount))
                    {
                        row["AllocAccount"] = messageField[FIXConstants.TagAllocAccount].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagAllocShares))
                    {
                        row["AllocQty"] = messageField[FIXConstants.TagAllocShares].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagAllocAvgPx))
                    {
                        row["AllocAvgPx"] = messageField[FIXConstants.TagAllocAvgPx].Value;
                    }
                    else if (messageType == FIXConstants.MSGConfirmation)
                    {
                        row["AllocAvgPx"] = messageField[FIXConstants.TagAvgPx].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagCommission))
                    {
                        row["Commission"] = messageField[FIXConstants.TagCommission].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagMiscFeeAmt))
                    {
                        row["MiscFeeAmt"] = messageField[FIXConstants.TagMiscFeeAmt].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagAllocNetMoney))
                    {
                        row["AllocNetMoney"] = messageField[FIXConstants.TagAllocNetMoney].Value;
                    }
                    else if (messageType == FIXConstants.MSGConfirmation)
                    {
                        row["AllocNetMoney"] = messageField[FIXConstants.TagNetMoney].Value;
                    }
                    row["AllocID"] = allocId;
                    if (messageType == FIXConstants.MSGAllocation)
                    {
                        row["MatchStatus"] = ((int)AllocMatchStatus.NA).ToString();
                    }
                    else if (messageType == FIXConstants.MSGAllocationReportAck)
                    {
                        row["MatchStatus"] = ((int)AllocMatchStatus.AllocationAcknowledged).ToString();
                    }
                    else if (messageType == FIXConstants.MSGAllocationACK)
                    {
                        row["MatchStatus"] = messageField[FIXConstants.TagIndividualAllocRejCode].Value;

                        if (messageField.ContainsKey(FIXConstants.TagAllocText))
                        {
                            row["AllocText"] = messageField[FIXConstants.TagAllocText].Value;
                        }
                    }
                    else if(messageType == FIXConstants.MSGAllocationReport)
                    {
                        if (messageField.ContainsKey(FIXConstants.TagMatchStatus))
                        {
                            row["MatchStatus"] = messageField[FIXConstants.TagMatchStatus].Value;
                        }
                    }
                    else if (messageType == FIXConstants.MSGConfirmation)
                    {
                        row["AllocText"] = messageField[FIXConstants.TagText].Value;
                        row["MatchStatus"] = messageField[FIXConstants.TagMatchStatus].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagTradeDate))
                    {
                        row["TradeDate"] = messageField[FIXConstants.TagTradeDate].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagConfirmID))
                    {
                        row["ConfirmID"] = messageField[FIXConstants.TagConfirmID].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagConfirmRefID))
                    {
                        row["ConfirmRefID"] = messageField[FIXConstants.TagConfirmRefID].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagConfirmTransType))
                    {
                        row["ConfirmTransType"] = messageField[FIXConstants.TagConfirmTransType].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagConfirmType))
                    {
                        row["ConfirmType"] = messageField[FIXConstants.TagConfirmType].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagConfirmStatus))
                    {
                        row["ConfirmStatus"] = messageField[FIXConstants.TagConfirmStatus].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagAffirmStatus))
                    {
                        row["AffirmStatus"] = messageField[FIXConstants.TagAffirmStatus].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagConfirmRejReason))
                    {
                        row["ConfirmRejReason"] = messageField[FIXConstants.TagConfirmRejReason].Value;
                    }

                    allocationData.Rows.Add(row);
                }

                using (StringWriter writer = new StringWriter())
                {
                    allocationData.WriteXml(writer);
                    return writer.ToString();
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

        /// <summary>
        /// This method gets J msg and Tolerance data to compare with AK msg
        /// </summary>
        /// <param name="individualAllocId"></param>
        /// <param name="allocId"></param>
        /// <returns></returns>
        private static DataSet GetDataToGenerateAUMsg(string individualAllocId, string allocId)
        {
            DataSet requiredData = new DataSet();
            try
            {
                object[] parameter = new object[2];
                parameter[0] = individualAllocId;
                parameter[1] = allocId;
                requiredData = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetThirdPartyJMsgAndToleranceData", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return requiredData;
        }

        /// <summary>
        /// This method check for various conditions of AK message and compares with J msg to get Affirm Status
        /// </summary>
        /// <param name="messageFields"></param>
        /// <param name="correspondingJMsg"></param>
        /// <param name="toleranceTable"></param>
        /// <param name="blockData"></param>
        /// <returns></returns>
        private static int CompareWithJMsgAndGetAffirmStatus(List<RepeatingMessageFieldCollection> messageFields, DataTable correspondingJMsg, DataTable toleranceTable, ThirdPartyBlockLevelDetails blockData)
        {
            try
            {
                if (messageFields[0].GetField(FIXConstants.TagConfirmStatus) == ((int)ConfirmStatus.Received).ToString())
                {
                    messageFields[0].AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.NA).ToString());
                    return (int)AffirmStatus.Received;
                }
                if (messageFields[0].GetField(FIXConstants.TagConfirmStatus) == ((int)ConfirmStatus.Confirmed).ToString())
                {
                    messageFields[0].AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.ExactMatch).ToString());
                    return (int)AffirmStatus.Affirmed;
                }
                if (messageFields[0].GetField(FIXConstants.TagConfirmStatus) == ((int)ConfirmStatus.MismatchedAccount).ToString())
                {
                    messageFields[0].AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.MismatchedAccount).ToString());
                    return (int)AffirmStatus.Received;
                }
                else if (messageFields[0].GetField(FIXConstants.TagConfirmStatus) == ((int)ConfirmStatus.MissingSettlementInstructions).ToString())
                {
                    messageFields[0].AddField(FIXConstants.TagMatchStatus, ((int)AllocMatchStatus.MissingSettlementInstructions).ToString());
                    return (int)AffirmStatus.Received;
                }

                object[] toleranceValues = (toleranceTable.Rows.Count != 0 ) ? toleranceTable.Rows[0].ItemArray : null;
                object[] JMsgFields = correspondingJMsg.Rows[0].ItemArray;

                if (toleranceValues != null && Convert.ToInt32(toleranceValues[0]) == (int)MatchingField.ToleranceInPercentage)
                {
                    for (int i = 0; i < JMsgFields.Length - 1; i++)
                    {
                        decimal toleranceValue = toleranceValues[i + 1] is DBNull ? 0 : Convert.ToDecimal(toleranceValues[i + 1]);
                        decimal JMsgField = JMsgFields[i] is DBNull ? 0 : Convert.ToDecimal(JMsgFields[i]);
                        toleranceValues[i + 1] = toleranceValue != 0 ? Math.Abs((toleranceValue / 100) * JMsgField) : 0;
                    }
                }

                List<decimal> reqFieldsAKMsg = new List<decimal>
                {
                    Convert.ToDecimal(messageFields[0].GetField(FIXConstants.TagAvgPx)),
                    Convert.ToDecimal(messageFields[0].GetField(FIXConstants.TagCommission)),
                    Convert.ToDecimal(messageFields[0].GetField(FIXConstants.TagMiscFeeAmt)),
                    Convert.ToDecimal(messageFields[0].GetField(FIXConstants.TagNetMoney))
                };

                int allocMatchStatus = (int)AllocMatchStatus.NA;
                bool isMismatchAlreadyFound = false;
                if (Convert.ToDecimal(JMsgFields[4]) != Convert.ToDecimal(messageFields[0].GetField(FIXConstants.TagAllocShares)))
                {
                    allocMatchStatus = (int)AllocMatchStatus.QtyMismatch;
                    isMismatchAlreadyFound = true;
                }

                for (int i = 0; i < reqFieldsAKMsg.Count; i++)
                {
                    decimal msgFieldAK = Convert.ToDecimal(reqFieldsAKMsg[i]);
                    decimal msgFieldJ = JMsgFields[i] is DBNull ? 0 : Convert.ToDecimal(JMsgFields[i]);
                    if (msgFieldAK != msgFieldJ)
                    {
                        decimal fieldValueLowerBound = (toleranceValues != null) ? msgFieldJ - Convert.ToDecimal(toleranceValues[i + 1]) : decimal.MinValue;
                        decimal fieldValueUpperBound = (toleranceValues != null) ? msgFieldJ + Convert.ToDecimal(toleranceValues[i + 1]) : decimal.MinValue;
                        if (toleranceValues != null && (fieldValueLowerBound <= msgFieldAK && msgFieldAK <= fieldValueUpperBound))
                        {
                            if (!isMismatchAlreadyFound)
                            {
                                allocMatchStatus = (int)AllocMatchStatus.ToleranceMatch;
                            }
                        }
                        else
                        {
                            if (isMismatchAlreadyFound)
                            {
                                allocMatchStatus = (int)AllocMatchStatus.Multiple;
                                break;
                            }
                            else
                            {
                                isMismatchAlreadyFound = true;
                                switch (i)
                                {
                                    case 0:
                                        allocMatchStatus = (int)AllocMatchStatus.AvgPx;
                                        break;
                                    case 1:
                                        allocMatchStatus = (int)AllocMatchStatus.Commission;
                                        break;
                                    case 2:
                                        allocMatchStatus = (int)AllocMatchStatus.MiscFee;
                                        break;
                                    case 3:
                                        allocMatchStatus = (int)AllocMatchStatus.NetMoney;
                                        break;
                                }
                                blockData.SubStatus = "Mismatch";
                            }
                        }
                    }
                }
                messageFields[0].AddField(FIXConstants.TagMatchStatus, allocMatchStatus.ToString());
                return (int)AffirmStatus.Received;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return (int)AffirmStatus.Received;
            }
        }

        /// <summary>
        /// This method compares AS message with J message and gets AllocStatus for AT msg
        /// </summary>
        /// <param name="correspondingJMessages"></param>
        /// <param name="taxlotDetailsInASMsg"></param>
        /// <param name="toleranceProfile"></param>
        /// <param name="messageFields"></param>
        /// <returns></returns>
        private static int CompareWithJMessagesAndGetAllocStatus(DataTable correspondingJMessages, DataTable taxlotDetailsInASMsg, DataTable toleranceProfile, List<RepeatingMessageFieldCollection> messageFields)
        {
            try
            {
                int allocStatus = (int)BlockMatchStatus.Accepted;
                //0th index represents Tolerance Factor type i.e. Value(1) or Percentage(2)
                object[] toleranceValues = (toleranceProfile.Rows.Count != 0) ? toleranceProfile.Rows[0].ItemArray : null;
                object[] toleranceFactors = DeepCopyHelper.Clone(toleranceValues);
                for (int rowIdx = 0; rowIdx < taxlotDetailsInASMsg.Rows.Count; rowIdx++)
                {
                    DataRow rowAS = taxlotDetailsInASMsg.Rows[rowIdx];
                    string individualAllocId = rowAS["IndividualAllocId"].ToString();
                    DataRow rowJ = correspondingJMessages.AsEnumerable().FirstOrDefault(row => row.Field<string>("IndividualAllocID") == individualAllocId);
                    
                    if (toleranceValues != null && Convert.ToInt32(toleranceValues[0]) == (int)MatchingField.ToleranceInPercentage)
                    {
                        for (int i = 1; i < toleranceValues.Length; i++)
                        {
                            decimal toleranceValue = toleranceValues[i] is DBNull ? 0 : Convert.ToDecimal(toleranceValues[i]);
                            decimal JMsgField = rowJ.ItemArray[i + 1] is DBNull ? 0 : Convert.ToDecimal(rowJ.ItemArray[i + 1]);
                            toleranceFactors[i] = toleranceValue != 0 ? Math.Abs(toleranceValue / 100 * JMsgField) : 0;
                        }
                    }
                    bool isMismatchAlreadyFound = false;
                    int allocMatchStatus = (int)AllocMatchStatus.Confirmed;

                    if (Convert.ToDecimal(rowJ.ItemArray[1]) != Convert.ToDecimal(rowAS.ItemArray[1]))
                    {
                        allocMatchStatus = (int)AllocMatchStatus.QtyMismatch;
                        isMismatchAlreadyFound = true;
                        allocStatus = (int)BlockMatchStatus.Received;
                    }
                    for (int i = 2; i < rowAS.ItemArray.Length; i++)
                    {
                        decimal msgFieldAS = Convert.ToDecimal(rowAS.ItemArray[i]);
                        decimal msgFieldJ = rowJ.ItemArray[i] is DBNull ? 0 : Convert.ToDecimal(rowJ.ItemArray[i]);
                        if (msgFieldAS != msgFieldJ)
                        {
                            allocStatus = (int)BlockMatchStatus.Received;
                            decimal fieldValueLowerBound = (toleranceFactors != null) ? msgFieldJ - Convert.ToDecimal(toleranceFactors[i - 1]) : decimal.MinValue;
                            decimal fieldValueUpperBound = (toleranceFactors != null) ? msgFieldJ + Convert.ToDecimal(toleranceFactors[i - 1]) : decimal.MinValue;
                            if (toleranceFactors != null && (fieldValueLowerBound <= msgFieldAS && msgFieldAS <= fieldValueUpperBound))
                            {
                                if (!isMismatchAlreadyFound)
                                {
                                    allocMatchStatus = (int)AllocMatchStatus.ToleranceMatch;
                                }
                            }
                            else
                            {
                                if (isMismatchAlreadyFound)
                                {
                                    allocMatchStatus = (int)AllocMatchStatus.Multiple;
                                    break;
                                }
                                else
                                {
                                    isMismatchAlreadyFound = true;
                                    switch (i)
                                    {
                                        case 2:
                                            allocMatchStatus = (int)AllocMatchStatus.AvgPx;
                                            break;
                                        case 3:
                                            allocMatchStatus = (int)AllocMatchStatus.Commission;
                                            break;
                                        case 4:
                                            allocMatchStatus = (int)AllocMatchStatus.MiscFee;
                                            break;
                                        case 5:
                                            allocMatchStatus = (int)AllocMatchStatus.NetMoney;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    messageFields[rowIdx].AddField(FIXConstants.TagMatchStatus, allocMatchStatus.ToString());
                }
                return allocStatus;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return (int)BlockMatchStatus.Received;
            }
        }

        /// <summary>
        /// This method gets the taxlot details of AS message
        /// </summary>
        /// <param name="messageFields"></param>
        /// <returns></returns>
        private static DataTable GetASMsgTaxlotDetails(List<RepeatingMessageFieldCollection> messageFields)
        {
            DataTable allocationData = new DataTable();
            try
            {
                allocationData.TableName = "Allocation";
                allocationData.Columns.Add("IndividualAllocID");
                allocationData.Columns.Add("AllocQty");
                allocationData.Columns.Add("AllocAvgPx");
                allocationData.Columns.Add("Commission");
                allocationData.Columns.Add("MiscFeeAmt");
                allocationData.Columns.Add("AllocNetMoney");

                foreach (var messageField in messageFields)
                {
                    DataRow row = allocationData.NewRow();
                    if (messageField.ContainsKey(FIXConstants.TagIndividualAllocID))
                    {
                        row["IndividualAllocID"] = messageField[FIXConstants.TagIndividualAllocID].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagAllocShares))
                    {
                        row["AllocQty"] = messageField[FIXConstants.TagAllocShares].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagAllocAvgPx))
                    {
                        row["AllocAvgPx"] = messageField[FIXConstants.TagAllocAvgPx].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagCommission))
                    {
                        row["Commission"] = messageField[FIXConstants.TagCommission].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagMiscFeeAmt))
                    {
                        row["MiscFeeAmt"] = messageField[FIXConstants.TagMiscFeeAmt].Value;
                    }
                    if (messageField.ContainsKey(FIXConstants.TagAllocNetMoney))
                    {
                        row["AllocNetMoney"] = messageField[FIXConstants.TagAllocNetMoney].Value;
                    }
                    allocationData.Rows.Add(row);
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
            return allocationData;
        }

        /// <summary>
        /// This method gets J message data for specified allocId
        /// </summary>
        /// <param name="allocId"></param>
        /// <returns></returns>
        private static DataSet GetDataToGenerateATMsg(string allocId)
        {
            DataSet requiredData = new DataSet();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = allocId;
                requiredData = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetThirdPartyDataForATMessage", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return requiredData;
        }
    }
}
