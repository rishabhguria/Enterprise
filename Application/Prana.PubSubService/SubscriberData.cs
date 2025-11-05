using Prana.BusinessObjects;
using Prana.PubSubService.Interfaces;
using System.Collections.Generic;

namespace Prana.PubSubService
{
    class SubscriberData
    {
        private IPublishing _subscriber;
        public IPublishing Subscriber
        {
            get { return _subscriber; }
            set { _subscriber = value; }
        }

        private List<FilterData> _filters;
        public List<FilterData> Filters
        {
            get { return _filters; }
            set { _filters = value; }
        }

        public override bool Equals(object subscriber)
        {
            SubscriberData subscriberdata = subscriber as SubscriberData;
            if (_subscriber.Equals((subscriberdata.Subscriber)))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _subscriber.GetHashCode();
        }
    }
}
