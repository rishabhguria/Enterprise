using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BusinessObjects.GreenFieldModels
{
    public sealed class ServiceStatusDto
    {
        public ServiceStatusDto(string serviceName, string serviceDisplayName, bool isLive, int timeInterval)
        {
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
            ServiceDisplayName = serviceDisplayName ?? throw new ArgumentNullException(nameof(serviceDisplayName));
            IsLive = isLive;
            TimeInterval = timeInterval;
            TimeStamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Unique name of the service reporting status.
        /// </summary>
        public string ServiceName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceDisplayName { get; private set; }

        /// <summary>
        /// When this status was recorded. Recommended to be in UTC.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Producer-reported liveness flag.
        /// </summary>
        public bool IsLive { get; set; }

        /// <summary>
        /// Each service has its own time interval (refer to "HeartBeatInterval" value of App.config) after which it sends its status through a Kakfa Topic
        /// </summary>
        public int TimeInterval { get; set; }

        public void UpdateStatus(bool status)
        {
            IsLive = status;
            TimeStamp = DateTime.UtcNow;
        }
    }
}
