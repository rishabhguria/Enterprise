namespace Prana.OrderProcessor
{

    //internal class NotificationSender
    //{
    //    /// <summary>
    //    /// Output queue on which _notification will be sent 
    //    /// </summary>
    //    String _notificationExchange = String.Empty;

    //    /// <summary>
    //    /// AmqpServer host name
    //    /// </summary>
    //    //String _hostName = String.Empty;

    //    /// <summary>
    //    /// AmqpHelper, will be used for sending order to order output queue. 
    //    /// Trade sent using this instance will be checked against pre-trade
    //    /// compliance.
    //    /// </summary>
    //    //AmqpHelper _amqpHelper;


    //    internal NotificationSender()
    //    {
    //        InitializeAMQPHelper();

    //    }
    //    /// <summary>
    //    /// initialize the amqp helper 
    //    /// </summary>
    //    private void InitializeAMQPHelper()
    //    {
    //        _notificationExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings,ConfigurationHelper.CONFIGKEY_NotificationExchange);
    //        //_hostName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings,ConfigurationHelper.CONFIGKEY_AmqpServer);
    //        AmqpHelper.InitializeSender("Notification", _notificationExchange, MediaType.Exchange_Direct);
    //        //_amqpHelper = AmqpHelper.ForExchange(_hostName, _notificationExchange);
    //    }


    //    /// <summary>
    //    /// send notification object to client notification UI
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    internal void SendNotificationToClient(object obj, String routingKey)
    //    {
    //        try
    //        {
    //            //AmqpHelper.SendObject(obj, "Notification", routingKey);
    //            AmqpHelper.SendObject(obj, "Notification", "All");
    //            //_amqpHelper.SendObject(obj, routingKey);

    //        }
    //        catch (Exception ex)
    //        {

    //            // Invoke our policy that is responsible for making sure no secure information
    //            // gets out of our layer.
    //            bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);

    //            if (rethrow)
    //            {
    //                throw;
    //            }
    //        }
    //    }   
    //}
}
