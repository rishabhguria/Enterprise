using Prana.AmqpAdapter.EventArguments;
using System;

namespace Prana.AmqpAdapter.Delegates
{
    /// <summary>
    /// Data received event. Raised when data has been fetched from AMQP server
    /// </summary>
    /// <param name="dsReceived">Data is converted to json and then to dataset</param>
    /// <param name="mediaName">Name of media</param>
    /// <param name="mediaType">Media type</param>
    /// <param name="routingKey">Routing key if media type is direct exchange</param>
    public delegate void DataReceived(Object sender, DataReceivedEventArguments e);

    /// <summary>
    /// Informs that listener has been started on the basis  of given information
    /// </summary>
    /// <param name="mediaName">Name of media started</param>
    /// <param name="mediaType">Type of media connected</param>
    /// <param name="routingKey">routing key if media type is direct exchange</param>
    public delegate void ListenerStarted(Object sender, ListenerStartedEventArguments e);

    /// <summary>
    /// Informs when Listener has been stopped
    /// </summary>
    /// <param name="mediaName">Name of media</param>
    /// <param name="mediaType">Type of media</param>
    /// <param name="routingKey">Routing key if exchange if of type direct</param>
    /// <param name="cause">Cause to stop</param>
    public delegate void ListenerStopped(Object sender, ListenerStoppedEventArguments e);


}
