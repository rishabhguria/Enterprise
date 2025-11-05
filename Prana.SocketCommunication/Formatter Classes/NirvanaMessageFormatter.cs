using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prana.SocketCommunication
{
    /// <summary>
    /// Serializes Order or Basket Objects in string format  .. while creating a serilize method
    /// ensure that First Field is Message Type and Second field is Trading Account
    /// </summary>
    public class PranaMessageFormatter
    {
        #region EXposure PNL Related Methods

        public static string CreateExPnlCtrlMsg(string ctrlMsg, long sequenceNumber, ExPNLData exPNLDataType)
        {
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(PranaMessageConstants.MSG_ExpPNLCtrl);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(ctrlMsg);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(sequenceNumber);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append((int)exPNLDataType);
            str = sb.ToString();
            return str;
        }

        public static string[] FromExPnlCtrlMsg(string message)
        {
            string[] result = new string[3];
            String[] str;

            try
            {
                str = message.Split(Seperators.SEPERATOR_2);
                result[0] = str[1]; //ctrlMsg
                result[1] = str[2]; //sequenceNumber
                result[2] = str[3]; //exPNLDataType
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);
            }
            return result;
        }

        public static string CreateExPnlSubscriptionMsg(string userID, ExPNLSubscriptionType subscriptionType, ExPNLData exPNLDataType)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(PranaMessageConstants.MSG_ExpPNLSubscription);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(userID);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append((int)subscriptionType);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append((int)exPNLDataType);
            return sb.ToString();
        }

        public static string CreateExPnlRefreshDataMsg(string inputmessage, int userID)
        {
            return PranaMessageConstants.MSG_ExpPNLRefreshData + Seperators.SEPERATOR_2 + inputmessage + Seperators.SEPERATOR_2 + userID.ToString();
        }

        public static string CreatePrefUpdateMsg(bool useClosingMark, double xPercentOfAvgPrice)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(PranaMessageConstants.MSG_ExpPNLUpdatePreferences);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(useClosingMark.ToString());
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(xPercentOfAvgPrice.ToString());
            return sb.ToString();
        }

        public static void FromPrefUpdateMsg(string incomingPrefUpdateString, ref bool useClosingMark, ref double xPercentOfAvgVolume)
        {
            string[] str = incomingPrefUpdateString.Split(Seperators.SEPERATOR_2);
            useClosingMark = bool.Parse(str[1]);
            xPercentOfAvgVolume = double.Parse(str[2]);
        }

        public static string CreateLiveFeedSnapshotRequestMsg(List<string> symbolList)
        {
            string[] symbolArr = new string[symbolList.Count];
            symbolList.CopyTo(symbolArr, 0);

            string symbols = String.Join(Seperators.SEPERATOR_8, symbolArr);
            StringBuilder sb = new StringBuilder();
            sb.Append(PranaMessageConstants.MSG_GetLiveFeedSnapShot);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(symbols);
            return sb.ToString();
        }

        public static void FromLiveFeedSnapshotRequestMsg(string message, out List<string> symbolList)
        {
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_2);
                symbolList = new List<string>((IEnumerable<string>)str[1].Split(','));
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);
            }
        }

        public static string CreateExposureAndPnlOrderClearMsg(string tradingAccountID, List<string> IDsToBeCleared)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(tradingAccountID);
            sb.Append(Seperators.SEPERATOR_2);

            foreach (string id in IDsToBeCleared)
            {
                sb.Append(id + "$");
            }
            return sb.ToString();
        }

        public static List<string> FromExposureAndPnlOrderClearMsg(string message)
        {
            List<string> result;
            String[] str;

            try
            {
                str = message.Split(Seperators.SEPERATOR_2)[2].Split('$');
                result = new List<string>(str);
                //remove empty strings
                result.Remove(string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("Prana: Message Format Error.", ex);
            }
            return result;
        }

        public static string[] CreateLiveFeedSnapshotResponseChunk(List<SymbolData> l1List, int chunkSize)
        {
            string[] results = null;
            try
            {
                //TODO SK code can be changed to accomodate dynamic number of chunks
                int numberOfChunks = (int)Math.Ceiling((double)l1List.Count / chunkSize);
                results = new string[numberOfChunks];
                for (int i = 0; i < numberOfChunks; i++)
                {
                    StringBuilder chunk = new StringBuilder();
                    chunk.Append(PranaMessageConstants.MSG_GetLiveFeedSnapShot);
                    chunk.Append(Seperators.SEPERATOR_2);//0
                    chunk.Append(Seperators.SEPERATOR_8 + Seperators.SEPERATOR_5);//1 
                    for (int j = chunkSize * (i); j < chunkSize * (i + 1) && j < l1List.Count; j++)
                    {
                        SymbolData l1Data = l1List[j];
                        if (null != l1Data)
                        {
                            chunk.Append(l1Data.ToString());
                            chunk.Append(Seperators.SEPERATOR_6);
                        }
                    }
                    results[i] = chunk.ToString();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return results;
        }

        public static string CreateClientStatusMessage(string userID, bool clientStatus, List<string> lsRequestedItemIDs)
        {
            string strMSG = PranaMessageConstants.MSG_ExpPNLUserBusy + Seperators.SEPERATOR_2 + userID + Seperators.SEPERATOR_2 + clientStatus + Seperators.SEPERATOR_2;
            if (lsRequestedItemIDs != null && lsRequestedItemIDs.Count > 0)
            {
                StringBuilder strRequestedItemIDs = new StringBuilder();
                foreach (string itemID in lsRequestedItemIDs)
                {
                    strRequestedItemIDs.Append(itemID).Append(",");
                }

                if (strRequestedItemIDs.Length > 1)
                {
                    strRequestedItemIDs.Length--;
                    strMSG += strRequestedItemIDs;
                }
            }
            return strMSG;
        }

        public static void FromClientStatusMessage(string message, ref string userID, ref bool clientStatus, ref List<string> lsRequestedItemIDs)
        {
            string[] strValues = message.Split(Seperators.SEPERATOR_2);
            userID = strValues[1];
            clientStatus = bool.Parse(strValues[2]);
            if (strValues.Length > 3 && !string.IsNullOrEmpty(strValues[3]))
            {
                lsRequestedItemIDs = new List<string>((IEnumerable<string>)strValues[3].Split(','));
            }
        }

        public static void FromClientTaxLotRequest(string message, ref string userID, ref string groupedRowID, ref string callerGridName, ref int accountID, ref List<int> filteredAccountList)
        {
            string[] strValues = message.Split(Seperators.SEPERATOR_2);
            userID = strValues[1];
            groupedRowID = strValues[2];
            callerGridName = strValues[3];
            accountID = int.Parse(strValues[4]);
            filteredAccountList = strValues[5].Split(Seperators.SEPERATOR_14).Select(Int32.Parse).ToList();
        }

        public static string CreatePMPreferenceMessage(ExPNLPreferenceMsgType preferenceMsgType, string userID, string subMessage)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (preferenceMsgType.Equals(ExPNLPreferenceMsgType.FilterValueChanged))
                {
                    sb.Append(PranaMessageConstants.MSG_FilterDetails).Append(Seperators.SEPERATOR_2).Append(preferenceMsgType.ToString()).Append(Seperators.SEPERATOR_2).Append(userID).Append(Seperators.SEPERATOR_2).Append(subMessage);
                }
                else
                {
                    sb.Append(PranaMessageConstants.MSG_PMPreferences).Append(Seperators.SEPERATOR_2).Append(preferenceMsgType.ToString()).Append(Seperators.SEPERATOR_2).Append(userID).Append(Seperators.SEPERATOR_2).Append(subMessage);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return sb.ToString();
        }

        public static void FromPMPreferenceMessage(string message, out ExPNLPreferenceMsgType preferenceMSGType, out string userID, out string returnMessage)
        {
            string[] strValues = message.Split(Seperators.SEPERATOR_2);
            preferenceMSGType = (ExPNLPreferenceMsgType)Enum.Parse(typeof(ExPNLPreferenceMsgType), strValues[1]);
            userID = strValues[2];
            returnMessage = strValues[3];
        }
        #endregion

        /// <summary>
        /// Returns Message Type
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetMessageType(string message)
        {
            string messageToReturn = message.Substring(0, message.IndexOf(Seperators.SEPERATOR_2));
            return messageToReturn;
        }

        public static string GetSecondIndexField(string msg)
        {
            string secondMsgField = string.Empty;
            try
            {
                int indexofFirstSeperator = msg.IndexOf(Seperators.SEPERATOR_2);
                int lengthofSubString = msg.Length - indexofFirstSeperator - 1;
                string firstSubString = msg.Substring(indexofFirstSeperator + 1, lengthofSubString);
                secondMsgField = firstSubString.Substring(0, firstSubString.IndexOf(Seperators.SEPERATOR_2));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return secondMsgField;
        }

        public static void FromFilterChangedRequest(string msg, ref string userID, ref string tabKey, ref List<int> accountList, ref List<int> prevAccountList, ref string returnMsg)
        {
            try
            {
                string[] strValues = msg.Split(Seperators.SEPERATOR_2);
                userID = strValues[2];
                returnMsg = strValues[3];
                string[] subMessage = strValues[3].Split(Seperators.SEPERATOR_5);
                tabKey = subMessage[0];
                string[] allAccountList = subMessage[1].Split(Seperators.SEPERATOR_6);
                if (!string.IsNullOrEmpty(allAccountList[0]))
                {
                    accountList = allAccountList[0].Split(Seperators.SEPERATOR_14).Select(Int32.Parse).ToList();
                }
                if (!string.IsNullOrEmpty(allAccountList[1]))
                {
                    prevAccountList = allAccountList[1].Split(Seperators.SEPERATOR_14).Select(Int32.Parse).ToList();
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

        public static string CreateTaxLotRequest(string userID, string groupedRowID, string callerGridName, int accountID, List<int> filteredAccountList)
        {
            StringBuilder message = new StringBuilder();
            try
            {
                message.Append(PranaMessageConstants.MSG_EPNlTaxLotList);
                message.Append(Seperators.SEPERATOR_2);
                message.Append(userID);
                message.Append(Seperators.SEPERATOR_2);
                message.Append(groupedRowID);
                message.Append(Seperators.SEPERATOR_2);
                message.Append(callerGridName);
                message.Append(Seperators.SEPERATOR_2);
                message.Append(accountID.ToString());
                message.Append(Seperators.SEPERATOR_2);
                int count = 0;
                foreach (var accountId in filteredAccountList)
                {
                    count = count + 1;
                    message.Append(accountId);
                    if (count < filteredAccountList.Count)
                    {
                        message.Append(Seperators.SEPERATOR_14);
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
            return message.ToString();
        }
    }
}
