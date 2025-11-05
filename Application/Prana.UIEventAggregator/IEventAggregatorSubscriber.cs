namespace Prana.UIEventAggregator
{
    /// <summary>
    /// Genertic class to handle the events thrown by event subsriber service
    /// </summary>
    /// <typeparam name="TEventType">The type of the event type.</typeparam>
    public interface IEventAggregatorSubscriber<TEventType>
    {
        void OnEventHandler(TEventType e);
    }
}
