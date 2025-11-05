using Prana.AmqpAdapter.Enums;
using System;
using System.Data;

namespace Prana.AmqpAdapter.EventArguments
{
    /// <summary>
    /// These arguments are sent when the connection status of a module changes
    /// </summary>
    public class DataReceivedEventArguments : EventArgs
    {
        public DataSet DsReceived { get; set; }
        public String MediaName { get; set; }
        public MediaType Mediatype { get; set; }
        public String RoutingKey { get; set; }
        public String JsonDataReceived { get; set; }

        public DataReceivedEventArguments(DataSet dsReceived, String mediaName, MediaType mediaType, String routingKey, string jsonDataReceived = null)
        {
            DsReceived = dsReceived;
            MediaName = mediaName;
            Mediatype = mediaType;
            RoutingKey = routingKey;
            JsonDataReceived = jsonDataReceived;
        }
    }
}
