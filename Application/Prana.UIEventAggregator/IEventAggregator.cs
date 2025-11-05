using System;
using System.Threading;

namespace Prana.UIEventAggregator
{
    /// <summary>
    /// Interface for classes that support event aggregator services
    /// </summary>
    public interface IEventAggregator
    {
        void PublishEvent<TEventType>(TEventType eventToPublish);
        void SubsribeEvent(Object subscriber, SynchronizationContext synchronizationContext);
    }
}
