using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;

namespace Prana.SocketCommunication
{
    public class AdminMessageHandler
    {
        public static PranaMessage CreateLogOnMessage(ConnectionProperties connProperties)
        {
            CompanyUser user = connProperties.User;
            PranaMessage msg = new PranaMessage(FIXConstants.MSGLogon, int.MinValue);
            msg.FIXMessage.InternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGLogon);
            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_HandlerType, ((int)connProperties.HandlerType).ToString());

            if (user != null)
            {
                msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CompanyUserID, user.CompanyUserID.ToString());
                msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CompanyUserName, user.FirstName + " " + user.LastName);
                string tradingAccts = string.Empty;
                for (int i = 0; i < user.TradingAccounts.Count; i++)
                {
                    string tradingacc = ((TradingAccount)user.TradingAccounts[i]).TradingAccountID.ToString();
                    if (tradingAccts != string.Empty)
                    {
                        tradingAccts = tradingAccts + Seperators.SEPERATOR_1 + tradingacc;
                    }
                    else
                    {
                        tradingAccts = tradingacc;
                    }
                }
                msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_TradingAccountID, tradingAccts);
            }
            else
            {
                msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CompanyUserID, connProperties.IdentifierID);
                msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CompanyUserName, connProperties.IdentifierName);

            }

            return msg;
        }

        public static ConnectionProperties GetConnectionProperties(string logOnMsg)
        {
            ConnectionProperties connProperties = new ConnectionProperties();

            PranaMessage pranaMsg = new PranaMessage(logOnMsg);
            connProperties.IdentifierID = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value;
            connProperties.IdentifierName = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserName].Value;
            if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TradingAccountID))
            {
                string strtradingAccts = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradingAccountID].Value;
                connProperties.TradingAccounts = Prana.Utilities.MiscUtilities.GeneralUtilities.GetListFromString(strtradingAccts, Prana.BusinessObjects.Seperators.SEPERATOR_1);
            }
            if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ServiceEndPoint))
            {
                string[] data = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ServiceEndPoint].Value.Split(':');
                if (data.Length == 2)
                {
                    connProperties.ServerIPAddress = data[0];
                    connProperties.Port = int.Parse(data[1]);
                }
            }
            connProperties.HandlerType = (HandlerType)int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_HandlerType].Value);
            return connProperties;
        }

        public static PranaMessage CreateHeartBeatForUser()
        {
            PranaMessage msg = new PranaMessage(FIXConstants.MSGHeartbeat, int.MinValue);
            msg.FIXMessage.InternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGHeartbeat);
            return msg;
        }

        public static PranaMessage CreateCounterPartyStatusReport(int connectionID, int partyID, string partyName, int status, string ipAddress, int port, PranaServerConstants.OriginatorType originatorType, PranaServerConstants.BrokerConnectionType brokerConnectionType)
        {
            PranaMessage msg = new PranaMessage(CustomFIXConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REPORT, int.MinValue);
            msg.FIXMessage.InternalInformation.AddField(FIXConstants.TagMsgType, CustomFIXConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REPORT);
            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CounterPartyID, partyID.ToString());
            msg.FIXMessage.InternalInformation.AddField(FIXConstants.TagTargetCompID, partyName);
            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CounterPartyStatus, status.ToString());
            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ServiceEndPoint, ipAddress + ":" + port);
            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OriginatorType, ((int)originatorType).ToString());
            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_BrokerConnectionType, ((int)brokerConnectionType).ToString());
            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ConnectionID, connectionID.ToString());
            return msg;
        }
    }
}
