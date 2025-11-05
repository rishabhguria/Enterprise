using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
namespace Prana.MonitoringProcessor
{
    public class PranaMonitoringProcessor
    {
        #region IProcessingUnit Members
        IQueueProcessor _outMonitoringQueue = null;
        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        static PranaMonitoringProcessor _pranaMonitoringProcessor = null;

        static PranaMonitoringProcessor()
        {
            _pranaMonitoringProcessor = new PranaMonitoringProcessor();
        }

        public static PranaMonitoringProcessor GetInstance
        {
            get
            {
                return _pranaMonitoringProcessor;
            }
        }

        public void Initlise(IQueueProcessor outMonitoringQueue)
        {
            _outMonitoringQueue = outMonitoringQueue;
        }

        public void ProcessMessage(Prana.BusinessObjects.QueueMessage qMsg)
        {

            try
            {
                if (_outMonitoringQueue != null)
                {
                    qMsg.Message = binaryFormatter.Serialize(qMsg.Message);
                    _outMonitoringQueue.SendMessage(qMsg);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SendStatusMessages(string entity, bool status)
        {
            if (status)
            {
                SendStatusMessages(entity, PranaInternalConstants.ConnectionStatus.CONNECTED);
            }
            else
            {
                SendStatusMessages(entity, PranaInternalConstants.ConnectionStatus.DISCONNECTED);
            }
        }

        public void SendStatusMessages(string entity, PranaInternalConstants.ConnectionStatus status)
        {
            try
            {
                QueueMessage qmsg = new QueueMessage();
                qmsg.MsgType = CustomFIXConstants.MSG_StatusMessge;
                qmsg.Message = entity + ":" + ((int)status).ToString();
                ProcessMessage(qmsg);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
